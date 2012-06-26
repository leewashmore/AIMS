﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GreenField.Common;
using GreenField.Gadgets.Models;
using Microsoft.Practices.Prism.ViewModel;
using GreenField.ServiceCaller.ExternalResearchDefinitions;
using GreenField.DataContracts;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;
using GreenField.ServiceCaller;
using Microsoft.Practices.Prism.Logging;
using System.Linq;
using GreenField.Gadgets.Helpers;
using Microsoft.Practices.Prism.Commands;

namespace GreenField.Gadgets.ViewModels
{
    public class ViewModelBalanceSheet : NotificationObject
    {
        #region Fields
        /// <summary>
        /// MEF Singletons
        /// </summary>
        private IEventAggregator _eventAggregator;
        private IDBInteractivity _dbInteractivity;
        private ILoggerFacade _logger;
        private EntitySelectionData _entitySelectionData;
        #endregion

        #region Constructor
        public ViewModelBalanceSheet(DashboardGadgetParam param)
        {
            _logger = param.LoggerFacade;
            _dbInteractivity = param.DBInteractivity;
            _eventAggregator = param.EventAggregator;
            _entitySelectionData = param.DashboardGadgetPayload.EntitySelectionData;

            PeriodColumns.PeriodColumnNavigate += (e) =>
            {
                if (e.PeriodColumnNamespace == GetType().FullName)
                {
                    BusyIndicatorNotification(true, "Retrieving data for updated time span");
                    PeriodRecord = PeriodColumns.SetPeriodRecord(e.PeriodColumnNavigationDirection == PeriodColumns.NavigationDirection.LEFT ? --Iterator : ++Iterator);
                    PeriodColumnHeader = PeriodColumns.SetColumnHeaders(PeriodRecord);
                    FinancialStatementDisplayInfo = PeriodColumns.SetPeriodColumnDisplayInfo(FinancialStatementInfo, PeriodRecord, CurrencySource[SelectedCurrency], ReportedMonth);
                    BusyIndicatorNotification();
                }
            };

            if (_eventAggregator != null)
            {
                _eventAggregator.GetEvent<SecurityReferenceSetEvent>().Subscribe(HandleSecurityReferenceSetEvent);
            }

            if (_entitySelectionData != null)
            {
                HandleSecurityReferenceSetEvent(_entitySelectionData);
            }
        }
        #endregion

        #region Properties
        #region Financial Statement Information
        private List<PeriodColumnDisplayData> _financialStatementDisplayInfo;
        public List<PeriodColumnDisplayData> FinancialStatementDisplayInfo
        {
            get { return _financialStatementDisplayInfo; }
            set
            {
                _financialStatementDisplayInfo = value;
                RaisePropertyChanged(() => this.FinancialStatementDisplayInfo);
            }
        }

        private List<FinancialStatementData> _financialStatementInfo;
        public List<FinancialStatementData> FinancialStatementInfo
        {
            get
            {
                if (_financialStatementInfo == null)
                    _financialStatementInfo = new List<FinancialStatementData>();
                return _financialStatementInfo;
            }
            set
            {
                if (_financialStatementInfo != value)
                {
                    _financialStatementInfo = value;
                    SetFinancialStatementDisplayInfo();
                }
            }
        }
        #endregion

        #region Period Information
        public int Iterator { get; set; }

        private PeriodRecord _periodRecord;
        public PeriodRecord PeriodRecord
        {
            get
            {
                if (_periodRecord == null)
                    _periodRecord = PeriodColumns.SetPeriodRecord();
                return _periodRecord;
            }
            set { _periodRecord = value; }
        }
        #endregion

        #region Period Column Headers
        private List<String> _periodColumnHeader;
        public List<String> PeriodColumnHeader
        {
            get
            {
                if (_periodColumnHeader == null)
                    _periodColumnHeader = PeriodColumns.SetColumnHeaders(PeriodRecord);
                return _periodColumnHeader;
            }
            set
            {
                _periodColumnHeader = value;
                RaisePropertyChanged(() => this.PeriodColumnHeader);
                if (value != null)
                {
                    PeriodColumns.RaisePeriodColumnUpdateCompleted(new PeriodColumns.PeriodColumnUpdateEventArg()
                    {
                        PeriodColumnNamespace = GetType().FullName,
                        EntitySelectionData = _entitySelectionData,
                        PeriodColumnHeader = value,
                        PeriodRecord = PeriodRecord,
                        PeriodIsYearly = PeriodIsYearly
                    });
                }
            }
        }
        #endregion

        public IssuerReferenceData IssuerReferenceInfo { get; set; }

        private Int32 _selectedDataSource;
        public Int32 SelectedDataSource
        {
            get { return _selectedDataSource; }
            set
            {
                _selectedDataSource = value;
                RaisePropertyChanged(() => this.SelectedDataSource);
                RetrieveIssuerReferenceDataCallbackMethod(IssuerReferenceInfo);
            }
        }

        private Int32 _selectedPeriodType;
        public Int32 SelectedPeriodType
        {
            get { return _selectedPeriodType; }
            set
            {
                _selectedPeriodType = value;
                RaisePropertyChanged(() => this.SelectedPeriodType);
                RetrieveIssuerReferenceDataCallbackMethod(IssuerReferenceInfo);
            }
        }

        #region Calendarization Option
        public Int32? ReportedMonth { get; set; }
        public Int32? OriginalReportedMonth { get; set; }

        private List<String> _fiscalTypeInfo;
        public List<String> FiscalTypeInfo
        {
            get { return _fiscalTypeInfo; }
            set
            {
                if (_fiscalTypeInfo != value)
                {
                    _fiscalTypeInfo = value;
                    RaisePropertyChanged(() => this.FiscalTypeInfo);
                }
            }
        }

        private Int32 _selectedFiscalType;
        public Int32 SelectedFiscalType
        {
            get { return _selectedFiscalType; }
            set
            {
                if (_selectedFiscalType != value)
                {
                    _selectedFiscalType = value;
                    RaisePropertyChanged(() => this.SelectedFiscalType);
                    ReportedMonth = value == 0 ? OriginalReportedMonth : 12;
                    SetFinancialStatementDisplayInfo();
                }
            }
        }
        #endregion

        #region Currency Option
        private List<String> _currencySource;
        public List<String> CurrencySource
        {
            get { return _currencySource; }
            set
            {
                if (_currencySource != value)
                {
                    _currencySource = value;
                    RaisePropertyChanged(() => this.CurrencySource);
                }
            }
        }

        private Int32 _selectedCurrency;
        public Int32 SelectedCurrency
        {
            get { return _selectedCurrency; }
            set
            {
                if (_selectedCurrency != value)
                {
                    _selectedCurrency = value;
                    RaisePropertyChanged(() => this.SelectedCurrency);
                    SetFinancialStatementDisplayInfo();
                }
            }
        }
        #endregion

        #region Period Option: Yearly / Quarterly
        private bool _periodIsYearly = true;
        public bool PeriodIsYearly
        {
            get { return _periodIsYearly; }
            set { _periodIsYearly = value; }
        }
        private bool? _yearlyPeriodChecked = true;
        public bool? YearlyPeriodChecked
        {
            get { return _yearlyPeriodChecked; }
            set
            {
                if (_yearlyPeriodChecked != value)
                {
                    _yearlyPeriodChecked = value;
                    RaisePropertyChanged(() => this.YearlyPeriodChecked);
                    if (value == true)
                    {
                        PeriodIsYearly = true;
                        PeriodRecord = PeriodColumns.SetPeriodRecord(Iterator);
                        PeriodColumnHeader = PeriodColumns.SetColumnHeaders(PeriodRecord);
                    }
                }
            }
        }

        private bool? _quarterlyPeriodChecked = false;
        public bool? QuarterlyPeriodChecked
        {
            get { return _quarterlyPeriodChecked; }
            set
            {
                if (_quarterlyPeriodChecked != value)
                {
                    _quarterlyPeriodChecked = value;
                    RaisePropertyChanged(() => this.QuarterlyPeriodChecked);
                    if (value == true)
                    {
                        PeriodIsYearly = false;
                        PeriodRecord = PeriodColumns.SetPeriodRecord(Iterator);
                        PeriodColumnHeader = PeriodColumns.SetColumnHeaders(PeriodRecord);
                    }
                }
            }
        }
        #endregion

        #region Security Information
        private String _securityLongName;
        public String SecurityLongName
        {
            get { return _securityLongName; }
            set
            {
                if (_securityLongName != value)
                {
                    _securityLongName = value;
                    RaisePropertyChanged(() => this.SecurityLongName);
                }
            }
        }

        private String _securityShortName;
        public String SecurityShortName
        {
            get { return _securityShortName; }
            set
            {
                if (_securityShortName != value)
                {
                    _securityShortName = value;
                    RaisePropertyChanged(() => this.SecurityShortName);
                }
            }
        }

        #endregion

        #region Busy Indicator
        private bool _busyIndicatorIsBusy;
        public bool BusyIndicatorIsBusy
        {
            get { return _busyIndicatorIsBusy; }
            set
            {
                _busyIndicatorIsBusy = value;
                RaisePropertyChanged(() => this.BusyIndicatorIsBusy);
            }
        }

        private string _busyIndicatorContent;
        public string BusyIndicatorContent
        {
            get { return _busyIndicatorContent; }
            set
            {
                _busyIndicatorContent = value;
                RaisePropertyChanged(() => this.BusyIndicatorContent);
            }
        }
        #endregion
        #endregion

        #region Event Handlers
        public void HandleSecurityReferenceSetEvent(EntitySelectionData result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (result != null)
                {
                    Logging.LogMethodParameter(_logger, methodNamespace, result, 1);
                    _entitySelectionData = result;
                    SecurityLongName = result.LongName;
                    SecurityShortName = result.ShortName;

                    if (_entitySelectionData != null)
                    {
                        BusyIndicatorNotification(true, "Retrieving Issuer Details based on selected security");
                        _dbInteractivity.RetrieveIssuerReferenceData(result, RetrieveIssuerReferenceDataCallbackMethod);
                    }
                }
                else
                {
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                }
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            Logging.LogEndMethod(_logger, methodNamespace);
        }
        #endregion

        #region Helper Methods
        public void Dispose()
        {
            _eventAggregator.GetEvent<SecurityReferenceSetEvent>().Unsubscribe(HandleSecurityReferenceSetEvent);
        }

        public void SetFinancialStatementDisplayInfo()
        {
            BusyIndicatorNotification(true, "Updating Financial Statement Information based on selected preference");
            if (CurrencySource != null)
                FinancialStatementDisplayInfo = PeriodColumns.SetPeriodColumnDisplayInfo(FinancialStatementInfo, PeriodRecord, CurrencySource[SelectedCurrency], ReportedMonth);
            else
                FinancialStatementDisplayInfo = null;
            BusyIndicatorNotification();
            //PeriodColumn.NavigationCompleted -= new PeriodColumnNavigationEventHandler(SetFinancialStatementDisplayInfo);
        }

        public void BusyIndicatorNotification(bool showBusyIndicator = false, String message = null)
        {
            if (message != null)
                BusyIndicatorContent = message;
            BusyIndicatorIsBusy = showBusyIndicator;
        }

        private String GetMonth(int? month)
        {
            switch (month)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sep";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                default:
                    throw new InvalidCastException();
            }
        }
        #endregion

        #region Callback Methods
        public void RetrieveIssuerReferenceDataCallbackMethod(IssuerReferenceData result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (result != null)
                {
                    if (IssuerReferenceInfo != result)
                        IssuerReferenceInfo = result;

                    BusyIndicatorNotification(true, "Retrieving Financial Statement Data for ");
                    FinancialStatementDataSource selectedFinancialStatementDataSource = SelectedDataSource == 0
                        ? FinancialStatementDataSource.REUTERS
                        : (SelectedDataSource == 1
                            ? FinancialStatementDataSource.PRIMARY
                            : FinancialStatementDataSource.INDUSTRY);

                    FinancialStatementPeriodType selectedFinancialStatementPeriodType = SelectedPeriodType == 0
                        ? FinancialStatementPeriodType.ANNUAL
                        : FinancialStatementPeriodType.QUARTERLY;

                    _dbInteractivity.RetrieveFinancialStatementData(result.IssuerId, selectedFinancialStatementDataSource
                        , selectedFinancialStatementPeriodType, FinancialStatementFiscalType.FISCAL, FinancialStatementStatementType.BALANCE_SHEET, result.CurrencyCode, RetrieveFinancialStatementDataCallbackMethod);
                }
                else
                {
                    Prompt.ShowDialog("No Issuer linked to the entity " + _entitySelectionData.LongName + " (" + _entitySelectionData.ShortName + " : " + _entitySelectionData.InstrumentID + ")");
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                }
                BusyIndicatorNotification();
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            Logging.LogEndMethod(_logger, methodNamespace);
        }

        public void RetrieveFinancialStatementDataCallbackMethod(List<FinancialStatementData> result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (result != null)
                {
                    PeriodRecord = PeriodColumns.SetPeriodRecord(Iterator);
                    PeriodColumnHeader = PeriodColumns.SetColumnHeaders(PeriodRecord);

                    if (result.Count != 0)
                    {
                        if (result[0].REPORTED_MONTH == 12)
                        {
                            FiscalTypeInfo = new List<string> { "Calendar End Date" };
                            SelectedFiscalType = 0;
                            ReportedMonth = 12;
                            OriginalReportedMonth = 12;
                        }
                        else
                        {
                            FiscalTypeInfo = new List<string> { "Reported End Date (" + GetMonth(result[0].REPORTED_MONTH) + ")", "Calendar End Date" };
                            SelectedFiscalType = 0;
                            ReportedMonth = result[0].REPORTED_MONTH;
                            OriginalReportedMonth = result[0].REPORTED_MONTH;
                        }
                        CurrencySource = result.OrderBy(record => record.CURRENCY).Select(record => record.CURRENCY).Distinct().ToList();
                        SelectedCurrency = CurrencySource.IndexOf(CurrencySource.Where(record => record != "USD").FirstOrDefault());
                    }
                    else
                    {
                        FiscalTypeInfo = null;
                        SelectedFiscalType = 0;
                        ReportedMonth = 12;
                        OriginalReportedMonth = 12;
                        CurrencySource = null;
                        SelectedCurrency = 0;
                    }

                    FinancialStatementInfo = result;
                }
                else
                {
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                }
                BusyIndicatorNotification();
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            Logging.LogEndMethod(_logger, methodNamespace);
        }
        #endregion
    }
}
