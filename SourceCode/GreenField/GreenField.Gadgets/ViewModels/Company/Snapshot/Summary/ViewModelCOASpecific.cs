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
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Commands;
using GreenField.Common;
using GreenField.ServiceCaller.ExternalResearchDefinitions;
using GreenField.DataContracts;
using GreenField.DataContracts.DataContracts;
using GreenField.Gadgets.Helpers;
using GreenField.Gadgets.Models;
using GreenField.ServiceCaller;

namespace GreenField.Gadgets.ViewModels
{
    /// <summary>
    /// View Model for COASpecific Gadgets With Period Columns
    /// </summary>
    public class ViewModelCOASpecific : NotificationObject
    {
        #region Fields
        /// <summary>
        /// MEF Singletons
        /// </summary>
        private IEventAggregator _eventAggregator;
        private IDBInteractivity _dbInteractivity;
        public ILoggerFacade _logger;
        private String defaultGadgetDesc;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="param">DashBoardGadgetParam</param>
        public ViewModelCOASpecific(DashboardGadgetParam param)
        {
            _logger = param.LoggerFacade;
            _dbInteractivity = param.DBInteractivity;
            _eventAggregator = param.EventAggregator;
            EntitySelectionInfo = param.DashboardGadgetPayload.EntitySelectionData;
            PeriodColumns.PeriodColumnNavigate += new PeriodColumnNavigationEvent(PeriodColumns_PeriodColumnNavigate);
            if (_eventAggregator != null)
            {
                _eventAggregator.GetEvent<SecurityReferenceSetEvent>().Subscribe(HandleSecurityReferenceSetEvent);
            }
            if (EntitySelectionInfo != null && IsActive)
            {
                HandleSecurityReferenceSetEvent(EntitySelectionInfo);
            }
        }
        #endregion

        #region Properties
        #region Period Information
        /// <summary>
        /// Iteration Count
        /// </summary>
        public Int32 Iterator { get; set; }

        /// <summary>
        /// Period Record storing period information based on iteration
        /// </summary>
        private PeriodRecord periodRecord;
        public PeriodRecord PeriodRecord
        {
            get
            {
                if (periodRecord == null)
                {
                    periodRecord = PeriodColumns.SetPeriodRecord(defaultHistoricalYearCount: 2, defaultHistoricalQuarterCount: 2,
                    netColumnCount: 5);
                }
                return periodRecord;
            }
            set { periodRecord = value; }
        }
        #endregion

        #region IsActive
        /// <summary>
        /// IsActive is true when parent control is displayed on UI
        /// </summary>
        private bool isActive;
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    if ((EntitySelectionInfo != null) && isActive)
                    {
                        RaisePropertyChanged(() => this.EntitySelectionInfo);
                        BusyIndicatorNotification(true, "Retrieving Issuer Details based on selected security");
                        _dbInteractivity.RetrieveIssuerReferenceData(EntitySelectionInfo, RetrieveIssuerReferenceDataCallbackMethod);
                    }
                }
            }
        }
        #endregion

        #region Issuer Details

        /// <summary>
        /// Stores Issuer related data
        /// </summary>      
        private IssuerReferenceData issuerReferenceInfo;
        public IssuerReferenceData IssuerReferenceInfo
        {
            get { return issuerReferenceInfo; }
            set
            {
                if (issuerReferenceInfo != value)
                {
                    issuerReferenceInfo = value;
                    if (value != null)
                    {
                        CurrencyInfo = new ObservableCollection<String> { IssuerReferenceInfo.CurrencyCode };
                        if (IssuerReferenceInfo.CurrencyCode != "USD")
                        {
                            CurrencyInfo.Add("USD");
                        }
                        SelectedCurrency = "USD";
                    }
                }
            }
        }
        #endregion

        #region Currency Option
        /// <summary>
        /// Stores Reported issuer domicile currency and "USD"
        /// </summary>
        private ObservableCollection<String> currencyInfo = new ObservableCollection<string> { "USD" };
        public ObservableCollection<String> CurrencyInfo
        {
            get { return currencyInfo; }
            set
            {
                if (currencyInfo != value)
                {
                    currencyInfo = value;
                    RaisePropertyChanged(() => this.CurrencyInfo);
                }
            }
        }

        /// <summary>
        /// Stores selected currency
        /// </summary>
        private String selectedCurrency = "USD";
        public String SelectedCurrency
        {
            get { return selectedCurrency; }
            set
            {
                selectedCurrency = value;
                RaisePropertyChanged(() => this.SelectedCurrency);
                RetrieveCOASpecificData();
            }
        }
        #endregion

        #region Security Information
        /// <summary>
        /// List to store entity information
        /// </summary>
        private EntitySelectionData entitySelectionInfo;
        public EntitySelectionData EntitySelectionInfo
        {
            get { return entitySelectionInfo; }
            set
            {
                entitySelectionInfo = value;
                if (IsActive)
                {
                    RaisePropertyChanged(() => this.EntitySelectionInfo);
                }
            }
        }
        #endregion

        #region Busy Indicator
        /// <summary>
        /// Busy Indicator status property
        /// </summary>
        private bool busyIndicatorIsBusy;
        public bool BusyIndicatorIsBusy
        {
            get { return busyIndicatorIsBusy; }
            set
            {
                busyIndicatorIsBusy = value;
                RaisePropertyChanged(() => this.BusyIndicatorIsBusy);
            }
        }

        /// <summary>
        /// Content of busy indicator
        /// </summary>
        private string busyIndicatorContent;
        public string BusyIndicatorContent
        {
            get { return busyIndicatorContent; }
            set
            {
                busyIndicatorContent = value;
                RaisePropertyChanged(() => this.BusyIndicatorContent);
            }
        }
        #endregion

        #region Data Source
        /// <summary>
        /// Stores FinancialStatementDataSource Enum Items
        /// </summary>
        public List<FinancialStatementDataSource> DataSourceInfo
        {
            get { return EnumUtils.GetEnumDescriptions<FinancialStatementDataSource>(); }
        }

        /// <summary>
        /// Stores selected FinancialStatementDataSource
        /// </summary>
        private FinancialStatementDataSource _selectedDataSource = FinancialStatementDataSource.PRIMARY;
        public FinancialStatementDataSource SelectedDataSource
        {
            get { return _selectedDataSource; }
            set
            {
                if (_selectedDataSource != value)
                {
                    _selectedDataSource = value;
                    RaisePropertyChanged(() => this.SelectedDataSource);
                    RetrieveCOASpecificData();
                }
            }
        }
        #endregion

        #region Calendarization Option
        /// <summary>
        /// Stores FinancialStatementFiscalType Enum Items
        /// </summary>
        public List<FinancialStatementFiscalType> FiscalTypeInfo
        {
            get { return EnumUtils.GetEnumDescriptions<FinancialStatementFiscalType>(); }
        }

        /// <summary>
        /// Stores selected FinancialStatementFiscalType
        /// </summary>
        private FinancialStatementFiscalType _selectedFiscalType = FinancialStatementFiscalType.FISCAL;
        public FinancialStatementFiscalType SelectedFiscalType
        {
            get { return _selectedFiscalType; }
            set
            {
                if (_selectedFiscalType != value)
                {
                    if (_selectedFiscalType != value)
                    {
                        _selectedFiscalType = value;
                        RaisePropertyChanged(() => this.SelectedFiscalType);
                        RetrieveCOASpecificData();
                    }
                }
            }
        }
        #endregion

        #region COASpecificData List
        /// <summary>
        /// List that stores the names of the gadget
        /// </summary>
        private List<String> coaSpecificGadgetNameInfo;
        public List<String> COASpecificGadgetNameInfo
        {
            get { return coaSpecificGadgetNameInfo; }
            set
            {
                if (coaSpecificGadgetNameInfo != value)
                {
                    coaSpecificGadgetNameInfo = value;
                    RaisePropertyChanged(() => this.COASpecificGadgetNameInfo);
                }
            }

        }

        /// <summary>
        /// Selected Gadget Name
        /// </summary>
        private String selectedCOASpecificGadgetNameInfo;
        public String SelectedCOASpecificGadgetNameInfo
        {
            get { return selectedCOASpecificGadgetNameInfo; }
            set
            {
                if (selectedCOASpecificGadgetNameInfo != value)
                {
                    selectedCOASpecificGadgetNameInfo = value;
                    COASpecificFilteredInfo = new ObservableCollection<COASpecificData>
                        (COASpecificInfo.Where(t => t.GroupDescription == value && t.PeriodYear != 2300));
                    AddToComboBoxSeries.Clear();
                    RaisePropertyChanged(() => this.SelectedCOASpecificGadgetNameInfo);
                }
            }
        }

        /// <summary>
        /// List storing whole COA data 
        /// </summary>
        private List<COASpecificData> coaSpecificInfo;
        public List<COASpecificData> COASpecificInfo
        {
            get { return coaSpecificInfo; }
            set
            {
                if (coaSpecificInfo != value)
                {
                    coaSpecificInfo = value;
                    COASpecificGadgetNameInfo = value.Select(t => t.GroupDescription).Distinct().ToList();
                    SelectedCOASpecificGadgetNameInfo = value.Select(t => t.GroupDescription).FirstOrDefault();
                    RaisePropertyChanged(() => this.COASpecificInfo);
                    SetCOASpecificDisplayInfo();
                }
            }
        }

        /// <summary>
        /// List that stores the filtered COA specific Information
        /// </summary>
        private ObservableCollection<COASpecificData> coaSpecificFilterdInfo = new ObservableCollection<COASpecificData>();
        public ObservableCollection<COASpecificData> COASpecificFilteredInfo
        {
            get { return coaSpecificFilterdInfo; }
            set
            {
                if (value != null)
                {
                    coaSpecificFilterdInfo = value;
                    List<String> defaultSeries = COASpecificFilteredInfo.Select(t => t.Description).Distinct().ToList();
                    ComparisonSeries.Clear();
                    foreach (String t in defaultSeries)
                    {
                        GadgetWithPeriodColumns entry = new GadgetWithPeriodColumns();
                        entry.GridId = null;
                        entry.GadgetName = null;
                        entry.GadgetDesc = t;
                        entry.Amount = null;
                        entry.PeriodYear = null;
                        ComparisonSeries.Add(entry);
                    }
                    RaisePropertyChanged(() => this.COASpecificFilteredInfo);
                }
            }
        }

        /// <summary>
        /// List containing all series under combo box add to chart
        /// </summary>
        private ObservableCollection<GadgetWithPeriodColumns> comparisonSeries = new ObservableCollection<GadgetWithPeriodColumns>();
        public ObservableCollection<GadgetWithPeriodColumns> ComparisonSeries
        {
            get { return comparisonSeries; }
            set
            {
                comparisonSeries = value;
                RaisePropertyChanged(() => this.ComparisonSeries);
            }
        }

        /// <summary>
        /// List containing all series in the  combo box add to chart
        /// </summary>
        private ObservableCollection<String> addToComboBoxSeries = new ObservableCollection<String>();
        public ObservableCollection<String> AddToComboBoxSeries
        {
            get { return addToComboBoxSeries; }
            set
            {
                addToComboBoxSeries = value;
                RaisePropertyChanged(() => this.AddToComboBoxSeries);
            }
        }

        /// <summary>
        /// Pivoted COA Specific  Information to be dispayed on grid
        /// </summary>
        private List<PeriodColumnDisplayData> _coaSpecificDisplayInfo;
        public List<PeriodColumnDisplayData> COASpecificDisplayInfo
        {
            get { return _coaSpecificDisplayInfo; }
            set
            {
                _coaSpecificDisplayInfo = value;
                RaisePropertyChanged(() => this.COASpecificDisplayInfo);
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
                {
                    _periodColumnHeader = PeriodColumns.SetColumnHeaders(PeriodRecord, false);
                }
                return _periodColumnHeader;
            }
            set
            {
                _periodColumnHeader = value;
                RaisePropertyChanged(() => this.PeriodColumnHeader);
                if (value != null)
                {
                    PeriodColumns.RaisePeriodColumnUpdateCompleted(new PeriodColumnUpdateEventArg()
                    {
                        PeriodColumnNamespace = GetType().FullName,
                        PeriodColumnHeader = value,
                        PeriodRecord = PeriodRecord,
                        PeriodIsYearly = true
                    });
                }
            }
        }
        #endregion

        #region ICommand
        /// <summary>
        /// Delete Series from Chart
        /// </summary>
        public ICommand DeleteCommand
        {
            get { return new DelegateCommand<object>(DeleteCommandMethod); }
        }

        /// <summary>
        /// Add to chart method
        /// </summary>
        public ICommand AddCommand
        {
            get { return new DelegateCommand<object>(AddCommandMethod); }
        }
        #endregion

        #region Selected Series
        /// <summary>
        /// Stores selected Series From Combo Box
        /// </summary>
        private String _selectedSeriesCB;
        public String SelectedSeriesCB
        {
            get { return _selectedSeriesCB; }
            set
            {
                _selectedSeriesCB = value;
                RaisePropertyChanged(() => this.SelectedSeriesCB);
            }
        }
        #endregion
        #endregion

        #region CallbackMethods
        /// <summary>
        /// Retrieve COA Specific data callback method
        /// </summary>
        /// <param name="result">list of type COASpecificData</param>
        void RetrieveCOASpecificDataCallbackMethod(List<COASpecificData> result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (result != null)
                {
                    COASpecificInfo = result;
                }
                else
                {
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                }
                BusyIndicatorNotification();
            }
            catch (Exception ex)
            {
                BusyIndicatorNotification();
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            Logging.LogEndMethod(_logger, methodNamespace);
        }

        /// <summary>
        /// RetrieveIssuerReferenceData Callback Method - assigns IssuerReferenceInfo and calls RetrieveFinancialStatementData
        /// </summary>
        /// <param name="result">IssuerReferenceData</param>
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
                    Prompt.ShowDialog("No Issuer linked to the entity " + EntitySelectionInfo.LongName + " (" + EntitySelectionInfo.ShortName +
                        " : " + EntitySelectionInfo.InstrumentID + ")");
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                }
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
                BusyIndicatorNotification();
            }
            Logging.LogEndMethod(_logger, methodNamespace);
        }
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
                    EntitySelectionInfo = result;
                    if (EntitySelectionInfo != null && IsActive)
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
        /// <summary>
        /// Disposes Events
        /// </summary>
        public void Dispose()
        {
            _eventAggregator.GetEvent<SecurityReferenceSetEvent>().Unsubscribe(HandleSecurityReferenceSetEvent);
        }

        /// <summary>
        /// Notifies the busy Indicator
        /// </summary>
        /// <param name="showBusyIndicator"></param>
        /// <param name="message"></param>
        public void BusyIndicatorNotification(bool showBusyIndicator = false, String message = null)
        {
            if (message != null)
            {
                BusyIndicatorContent = message;
            }
            BusyIndicatorIsBusy = showBusyIndicator;
        }

        /// <summary>
        /// Retrieves COA Specific data
        /// </summary>
        private void RetrieveCOASpecificData()
        {
            if (IssuerReferenceInfo != null)
            {
                COASpecificInfo = new List<COASpecificData>();
                BusyIndicatorNotification(true, "Retrieving Data based on selected security");
                _dbInteractivity.RetrieveCOASpecificData(IssuerReferenceInfo.IssuerId, IssuerReferenceInfo.SecurityId,
                    SelectedDataSource, SelectedFiscalType, SelectedCurrency, RetrieveCOASpecificDataCallbackMethod);
            }
        }

        /// <summary>
        /// Sets COA Specific Display Info on the grid
        /// </summary>
        public void SetCOASpecificDisplayInfo()
        {
            BusyIndicatorNotification(true, "Updating information based on selected preference");
            PeriodRecord periodRecord = PeriodColumns.SetPeriodRecord(Iterator, 3, 4, 6, false);
            COASpecificDisplayInfo = PeriodColumns.SetPeriodColumnDisplayInfo(COASpecificInfo, out periodRecord, periodRecord, subGroups: null);
            PeriodRecord = periodRecord;
            PeriodColumnHeader = PeriodColumns.SetColumnHeaders(PeriodRecord, true);
            BusyIndicatorNotification();
        }

        /// <summary>
        /// Navigates between various periods
        /// </summary>
        /// <param name="e"></param>
        void PeriodColumns_PeriodColumnNavigate(PeriodColumnNavigationEventArg e)
        {
            if (e.PeriodColumnNamespace == GetType().FullName)
            {
                Iterator = e.PeriodColumnNavigationDirection == NavigationDirection.LEFT ? Iterator - 1 : Iterator + 1;
                SetCOASpecificDisplayInfo();
            }
        }
        #endregion

        #region ICommand Methods
        /// <summary>
        /// Delete Series from Chart
        /// </summary>
        /// <param name="param"></param>
        private void DeleteCommandMethod(object param)
        {
            GadgetWithPeriodColumns a = param as GadgetWithPeriodColumns;
            List<COASpecificData> removeItem = new List<COASpecificData>();
            removeItem = COASpecificFilteredInfo.Where(w => w.Description == a.GadgetDesc).ToList();
            if (removeItem != null)
                foreach (COASpecificData r in removeItem)
                {
                    COASpecificFilteredInfo.Remove(r);
                }
            ComparisonSeries.Remove(a);
            AddToComboBoxSeries.Add(a.GadgetDesc);
        }

        /// <summary>
        /// Add to Chart Command Method
        /// </summary>
        /// <param name="param"></param>            
        private void AddCommandMethod(object param)
        {
            GadgetWithPeriodColumns entry = new GadgetWithPeriodColumns();
            if (SelectedSeriesCB != null)
            {
                entry.GridId = null;
                entry.GadgetName = null;
                entry.GadgetDesc = SelectedSeriesCB;
                entry.Amount = null;
                entry.PeriodYear = null;
                ComparisonSeries.Add(entry);
            }
            List<COASpecificData> addItem = new List<COASpecificData>();
            if (selectedCOASpecificGadgetNameInfo == null)
            {
                addItem = (COASpecificInfo.Where(t => t.GroupDescription == defaultGadgetDesc && t.Description == SelectedSeriesCB &&
                    t.PeriodYear != 2300)).ToList();
            }
            else
            {
                addItem = (COASpecificInfo.Where(t => t.GroupDescription == selectedCOASpecificGadgetNameInfo && t.Description ==
                      SelectedSeriesCB && t.PeriodYear != 2300)).ToList();
            }
            if (addItem != null)
                foreach (COASpecificData r in addItem)
                {
                    COASpecificFilteredInfo.Add(r);
                }
            AddToComboBoxSeries.Remove(SelectedSeriesCB);
        }
        #endregion

        #region Events
        /// <summary>
        /// Event for the notification of Data Load Completion
        /// </summary>
        public event DataRetrievalProgressIndicatorEventHandler CoaSpecificDataLoadedEvent;
        #endregion
    }
}
