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
using Microsoft.Practices.Prism.ViewModel;
using GreenField.Common.Helper;
using GreenField.ServiceCaller;
using Microsoft.Practices.Prism.Logging;
using GreenField.DataContracts;
using Microsoft.Practices.Prism.Events;
using GreenField.Common;
using GreenField.Gadgets.Models;
using System.Collections.Generic;
using GreenField.ServiceCaller.ExternalResearchDefinitions;
using GreenField.Gadgets.Helpers;
using System.Collections.ObjectModel;

namespace GreenField.Gadgets.ViewModels
{
    public class ViewModelEstimates : NotificationObject
    {
        #region PrivateVariables

        /// <summary>
        /// Instance of Service Caller
        /// </summary>
        private IDBInteractivity _dbInteractivity;

        /// <summary>
        /// LoggerFacade
        /// </summary>
        private ILoggerFacade _logger;

        /// <summary>
        /// Selected Security
        /// </summary>
        private EntitySelectionData _entitySelectionData;

        /// <summary>
        /// Event Aggregator
        /// </summary>
        private IEventAggregator _eventAggregator;

        /// <summary>
        /// Selected Date
        /// </summary>
        private DateTime? _effectiveDate;


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="param">Dashboard Gadget Payload</param>
        public ViewModelEstimates(DashboardGadgetParam param)
        {
            _dbInteractivity = param.DBInteractivity;
            _logger = param.LoggerFacade;
            _eventAggregator = param.EventAggregator;

            EntitySelectionInfo = param.DashboardGadgetPayload.EntitySelectionData;
            if (_eventAggregator != null)
            {
                _eventAggregator.GetEvent<SecurityReferenceSetEvent>().Subscribe(HandleSecurityReferenceSetEvent);
            }

            if (EntitySelectionInfo != null)
            {
                HandleSecurityReferenceSetEvent(EntitySelectionInfo);
            }
        }

        #endregion

        #region PropertyDeclaration

        private EntitySelectionData _entitySelectionInfo;
        public EntitySelectionData EntitySelectionInfo
        {
            get
            {
                return _entitySelectionInfo;
            }
            set { _entitySelectionInfo = value; }
        }

        /// <summary>
        /// Pivoted Financial Information to be dispayed on grid
        /// </summary>
        private List<PeriodColumnDisplayData> _consensusEstimateDetailDisplayInfo;
        public List<PeriodColumnDisplayData> ConsensusEstimateDetailDisplayInfo
        {
            get
            {
                return _consensusEstimateDetailDisplayInfo;
            }
            set
            {
                _consensusEstimateDetailDisplayInfo = value;
                RaisePropertyChanged(() => this.ConsensusEstimateDetailDisplayInfo);
            }
        }

        /// <summary>
        /// Unpivoted ConsensusEstimatesDetail Information received from stored procedure
        /// </summary>
        private List<ConsensusEstimateMedian> _consensusEstimateDetailInfo;
        public List<ConsensusEstimateMedian> ConsensusEstimateDetailInfo
        {
            get
            {
                if (_consensusEstimateDetailInfo == null)
                    _consensusEstimateDetailInfo = new List<ConsensusEstimateMedian>();
                return _consensusEstimateDetailInfo;
            }
            set
            {
                if (_consensusEstimateDetailInfo != value)
                {
                    _consensusEstimateDetailInfo = value;
                    SetConsensusEstimateMedianDisplayInfo();
                }
            }
        }

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

        #region Currency Option
        /// <summary>
        /// Stores Reported issuer domicile currency and "USD"
        /// </summary>
        private ObservableCollection<String> _currencyInfo = new ObservableCollection<string> { "USD" };
        public ObservableCollection<String> CurrencyInfo
        {
            get { return _currencyInfo; }
            set
            {
                if (_currencyInfo != value)
                {
                    _currencyInfo = value;
                    RaisePropertyChanged(() => this.CurrencyInfo);
                }
            }
        }

        /// <summary>
        /// Stores selected currency
        /// </summary>
        private String _selectedCurrency = "USD";
        public String SelectedCurrency
        {
            get { return _selectedCurrency; }
            set
            {
                if (_selectedCurrency != value)
                {
                    if (_selectedCurrency != value)
                    {
                        _selectedCurrency = value;
                        RaisePropertyChanged(() => this.SelectedCurrency);
                        RetrieveConsensusEstimatesMedianData();
                    }
                }
            }
        }
        #endregion

        #region Period Type
        /// <summary>
        /// Stores FinancialStatementPeriodType Enum Items
        /// </summary>
        public List<FinancialStatementPeriodType> PeriodTypeInfo
        {
            get { return EnumUtils.GetEnumDescriptions<FinancialStatementPeriodType>(); }
        }

        /// <summary>
        /// Stores selected FinancialStatementPeriodType
        /// </summary>
        private FinancialStatementPeriodType _selectedPeriodType = FinancialStatementPeriodType.ANNUAL;
        public FinancialStatementPeriodType SelectedPeriodType
        {
            get { return _selectedPeriodType; }
            set
            {
                if (_selectedPeriodType != value)
                {
                    _selectedPeriodType = value;
                    RaisePropertyChanged(() => this.SelectedPeriodType);
                    RetrieveConsensusEstimatesMedianData();
                }
            }
        }
        #endregion

        #region Period Column Headers
        /// <summary>
        /// Stores period column headers
        /// </summary>
        private List<String> _periodColumnHeader;
        public List<String> PeriodColumnHeader
        {
            get
            {
                if (_periodColumnHeader == null)
                    _periodColumnHeader = PeriodColumns.SetColumnHeaders(PeriodRecord, false);
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
                        EntitySelectionData = EntitySelectionInfo,
                        PeriodColumnHeader = value,
                        PeriodRecord = PeriodRecord,
                        PeriodIsYearly = SelectedPeriodType == FinancialStatementPeriodType.ANNUAL
                    });
                }
            }
        }
        #endregion

        #region Period Information
        /// <summary>
        /// Iteration Count
        /// </summary>
        public Int32 Iterator { get; set; }

        /// <summary>
        /// Period Record storing period information based on iteration
        /// </summary>
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

        #region Issuer Details

        /// <summary>
        /// Stores Issuer related data
        /// </summary>
        /// 
        private IssuerReferenceData _issuerReferenceInfo;
        public IssuerReferenceData IssuerReferenceInfo
        {
            get { return _issuerReferenceInfo; }
            set
            {
                if (_issuerReferenceInfo != value)
                {
                    _issuerReferenceInfo = value;
                    if (value != null)
                    {
                        CurrencyInfo = new ObservableCollection<String> { IssuerReferenceInfo.CurrencyCode };
                        if (IssuerReferenceInfo.CurrencyCode != "USD")
                            CurrencyInfo.Add("USD");

                        SelectedCurrency = CurrencyInfo[0];
                    }
                }
            }
        }
        #endregion

        #endregion

        #region HelperMethods

        public void SetConsensusEstimateMedianDisplayInfo()
        {
            BusyIndicatorNotification(true, "Updating information based on selected preference");

            PeriodRecord periodRecord = new PeriodRecord();
            ConsensusEstimateDetailDisplayInfo = PeriodColumns.SetPeriodColumnDisplayInfo(ConsensusEstimateDetailInfo, out periodRecord,
                PeriodColumns.SetPeriodRecord(Iterator));
            PeriodRecord = periodRecord;
            PeriodColumnHeader = PeriodColumns.SetColumnHeaders(PeriodRecord, false);

            BusyIndicatorNotification();
        }

        public void BusyIndicatorNotification(bool showBusyIndicator = false, String message = null)
        {
            if (message != null)
                BusyIndicatorContent = message;
            BusyIndicatorIsBusy = showBusyIndicator;
        }

        private void RetrieveConsensusEstimatesMedianData()
        {
            _dbInteractivity.RetrieveConsensusEstimatesMedianData(IssuerReferenceInfo.IssuerId,
                                                                SelectedPeriodType,
                                                                SelectedCurrency,
                                                                RetrieveConsensusEstimateDataCallbackMethod);
        }

        #endregion

        #region EventHandlers

        public void HandleSecurityReferenceSetEvent(EntitySelectionData result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (result != null)
                {
                    Logging.LogMethodParameter(_logger, methodNamespace, result, 1);
                    EntitySelectionInfo = result;

                    if (EntitySelectionInfo != null)
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

        #region Callback Methods

        public void RetrieveIssuerReferenceDataCallbackMethod(IssuerReferenceData result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (result != null)
                {
                    IssuerReferenceInfo = result;
                }
                else
                {
                    Prompt.ShowDialog("No Issuer linked to the entity " + EntitySelectionInfo.LongName + " (" + EntitySelectionInfo.ShortName + " : " + EntitySelectionInfo.InstrumentID + ")");
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

        public void RetrieveConsensusEstimateDataCallbackMethod(List<ConsensusEstimateMedian> result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (result != null)
                {
                    ConsensusEstimateDetailInfo = result;
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

        #region EventsUnsubscribe

        public void Dispose()
        {
            //Implement Dispose Here
        }

        #endregion
    }
}
