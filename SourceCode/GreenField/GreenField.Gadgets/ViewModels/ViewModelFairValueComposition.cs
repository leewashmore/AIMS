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
using Microsoft.Practices.Prism.Events;
using GreenField.ServiceCaller;
using Microsoft.Practices.Prism.Logging;
using GreenField.Common;
using GreenField.DataContracts;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.Generic;
using System.Linq;
using GreenField.Gadgets.Helpers;
using Microsoft.Practices.Prism.Commands;

namespace GreenField.Gadgets.ViewModels
{
    public class ViewModelFairValueComposition : NotificationObject
    {
        #region PRIVATE FIELDS
        //MEF Singletons

        /// <summary>
        /// private member object of the IEventAggregator for event aggregation
        /// </summary>
        private IEventAggregator _eventAggregator;

        /// <summary>
        /// private member object of the IDBInteractivity for interaction with the Service Caller
        /// </summary>
        private IDBInteractivity _dbInteractivity;

        /// <summary>
        /// private member object of ILoggerFacade for logging
        /// </summary>
        public ILoggerFacade _logger;


        /// <summary>
        /// Private member to store Selected Security ID
        /// </summary>
        private EntitySelectionData _securitySelectionData = null;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Stores fcf arranged data
        /// </summary>
        private RangeObservableCollection<FairValueData> _fairValueCompositionData
            = new RangeObservableCollection<FairValueData>();
        public RangeObservableCollection<FairValueData> FairValueCompositionData
        {
            get
            {
                return _fairValueCompositionData;
            }
            set
            {
                if (FairValueCompositionData != value)
                {
                    _fairValueCompositionData = value;
                    RaisePropertyChanged(() => this.FairValueCompositionData);
                }
            }
        }

        /// <summary>
        /// List of DataId and Corresponding Measures
        /// </summary>
        private List<Measure> _measuresData;
        public List<Measure> MeasuresData
        {
            get
            {
                if (_measuresData == null)
                {
                    _measuresData = new List<Measure>();
                }
                return _measuresData;
            }
            set
            {
                _measuresData = value;
                this.RaisePropertyChanged(() => this.MeasuresData);
            }
        }



        /// <summary>
        /// IsActive is true when parent control is displayed on UI
        /// </summary>
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                }
                if (_securitySelectionData != null && IsActive)
                {
                    if (_securitySelectionData.InstrumentID != null && _securitySelectionData.InstrumentID != string.Empty)
                    {
                        CallingWebMethod();
                    }
                }
            }
        }
        /// <summary>
        /// Busy Indicator Status
        /// </summary>
        private bool _busyIndicatorStatus;
        public bool BusyIndicatorStatus
        {
            get
            {
                return _busyIndicatorStatus;
            }
            set
            {
                _busyIndicatorStatus = value;
                this.RaisePropertyChanged(() => this.BusyIndicatorStatus);
            }
        }

        /// <summary>
        /// Instance when a sell property of specific row is edited in the gridView
        /// </summary>
        private FairValueData _editedSellPropertyFairValueRow;
        public FairValueData EditedSellPropertyFairValueRow
        {
            get
            {
                return _editedSellPropertyFairValueRow;
            }
            set
            {
                _editedSellPropertyFairValueRow = value;
                RetrieveInstanceWithUpdatedUpsideValue(_editedSellPropertyFairValueRow);
            }
        }

        /// <summary>
        /// Instance when a sell property of specific row is edited in the gridView
        /// </summary>
        private FairValueData _editedMeasurePropertyFairValueRow;
        public FairValueData EditedMeasurePropertyFairValueRow
        {
            get
            {
                return _editedMeasurePropertyFairValueRow;
            }
            set
            {
                _editedMeasurePropertyFairValueRow = value;
                UpdateRowAssociatedWithSource(_editedMeasurePropertyFairValueRow);
            }
        }

        private bool _isReadOnly = true;
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                this.RaisePropertyChanged(() => this.IsReadOnly);
            }
        }

        #endregion

        #region CONSTRUCTOR
        public ViewModelFairValueComposition(DashboardGadgetParam param)
        {
            _eventAggregator = param.EventAggregator;
            _dbInteractivity = param.DBInteractivity;
            _logger = param.LoggerFacade;
            _securitySelectionData = param.DashboardGadgetPayload.EntitySelectionData;
            MeasuresData = GetMeasureList();

            if (_eventAggregator != null)
            {
                _eventAggregator.GetEvent<SecurityReferenceSetEvent>().Subscribe(HandleSecurityReferenceSet);
            }
            if (_securitySelectionData != null && IsActive)
            {
                if (_securitySelectionData.InstrumentID != null && _securitySelectionData.InstrumentID != string.Empty)
                {
                    CallingWebMethod();
                }
            }

        }

        #endregion

        #region ICommand

        public ICommand SaveFairValueChangeCommand
        {
            get { return new DelegateCommand<object>(SaveFairValueChangeCommandMethod); }
        }

        #endregion

        #region ICommand Methods

        private void SaveFairValueChangeCommandMethod(object param)
        {
            if (FairValueCompositionData != null && _dbInteractivity != null)
            {
                BusyIndicatorStatus = true;

                List<FairValueCompositionSummaryData> data = new List<FairValueCompositionSummaryData>();

                var updatedItems = FairValueCompositionData.Where(p => p.IsUpdated == true).ToList();

                foreach (FairValueData item in updatedItems)
                {
                    data.Add(new FairValueCompositionSummaryData()
                    {
                        Source = item.Source,
                        Measure = item.Measure,
                        DataId = item.DataId,
                        Buy = item.Buy,
                        Sell = item.Sell,
                        Upside = item.Upside,
                        Date = item.Date
                    });
                }

                if (data.Count > 0)
                {
                    _dbInteractivity.SaveUpdatedFairValueData(_securitySelectionData, data
                        , RetrieveFairValueCompositionSummaryDataCallbackMethod);
                }

            }
        }

        #endregion

        #region EVENTHANDLERS

        /// <summary>
        /// Event Handler to subscribed event 'SecurityReferenceSet'
        /// </summary>
        /// <param name="securityReferenceData">SecurityReferenceData</param>
        public void HandleSecurityReferenceSet(EntitySelectionData entitySelectionData)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (entitySelectionData != null)
                {
                    Logging.LogMethodParameter(_logger, methodNamespace, entitySelectionData, 1);
                    _securitySelectionData = entitySelectionData;

                    if (_securitySelectionData.InstrumentID != null && _securitySelectionData.InstrumentID != string.Empty)
                    {
                        CallingWebMethod();
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
        }

        private void RetrieveInstanceWithUpdatedUpsideValue(FairValueData editedFairValueRow)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (_securitySelectionData != null && editedFairValueRow != null)
                {
                    Logging.LogMethodParameter(_logger, methodNamespace, _securitySelectionData, 1);

                    FairValueCompositionSummaryData editedData = new FairValueCompositionSummaryData()
                    {
                        Source = editedFairValueRow.Source,
                        Measure = editedFairValueRow.Measure,
                        Buy = editedFairValueRow.Buy,
                        Sell = editedFairValueRow.Sell,
                        Upside = editedFairValueRow.Upside,
                        Date = editedFairValueRow.Date,
                        DataId = editedFairValueRow.DataId,
                    };
                    _dbInteractivity.RetrieveFairValueDataWithNewUpside(_securitySelectionData, editedData,
                        RetrieveFairValueDataWithNewUpsideCallbackMethod);
                    BusyIndicatorStatus = true;

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
        }

        private void UpdateRowAssociatedWithSource(FairValueData _editedMeasurePropertyFairValueRow)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {

                if (_editedMeasurePropertyFairValueRow != null)
                {
                    var item = FairValueCompositionData.FirstOrDefault(i => i.Source == _editedMeasurePropertyFairValueRow.Source);

                    if (item != null)
                    {
                        item.Buy = 0;
                        item.Sell = 0;
                        item.Upside = null;
                        item.Date = DateTime.Now;
                        item.IsUpdated = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
        }

        #endregion

        #region CALLBACK METHOD
        /// <summary>
        /// Callback method that assigns value to the BAsicDataInfo property
        /// </summary>
        /// <param name="result">basic data </param>
        private void RetrieveFairValueCompositionSummaryDataCallbackMethod(List<FairValueCompositionSummaryData> result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {                
                RangeObservableCollection<FairValueData> temp = new RangeObservableCollection<FairValueData>();               

                Logging.LogMethodParameter(_logger, methodNamespace, result, 1);

                if (result != null)
                {
                    SetIsEditableProprty(result);
                }

                temp.Add(new FairValueData()
                {
                    Source = "Primary Analyst",
                    Measure = result.Select(a => a.Source).Contains("Primary Analyst") ? result.Where(a => a.Source == "Primary Analyst").Select(a => a.Measure).FirstOrDefault() : null,
                    Buy = result.Select(a => a.Source).Contains("Primary Analyst") ? result.Where(a => a.Source == "Primary Analyst").Select(a => a.Buy).FirstOrDefault() : null,
                    Sell = result.Select(a => a.Source).Contains("Primary Analyst") ? result.Where(a => a.Source == "Primary Analyst").Select(a => a.Sell).FirstOrDefault() : null,
                    Upside = result.Select(a => a.Source).Contains("Primary Analyst") ? result.Where(a => a.Source == "Primary Analyst").Select(a => a.Upside).FirstOrDefault() : null,
                    Date = result.Select(a => a.Source).Contains("Primary Analyst") ? result.Where(a => a.Source == "Primary Analyst").Select(a => a.Date).FirstOrDefault() : null,
                    DataId = result.Select(a => a.Source).Contains("Primary Analyst") ? result.Where(a => a.Source == "Primary Analyst").Select(a => a.DataId).FirstOrDefault() : null,
                });
                temp.Add(new FairValueData()
                {
                    Source = "Industry Analyst",
                    Measure = result.Select(a => a.Source).Contains("Industry Analyst") ? result.Where(a => a.Source == "Industry Analyst").Select(a => a.Measure).FirstOrDefault() : null,
                    Buy = result.Select(a => a.Source).Contains("Industry Analyst") ? result.Where(a => a.Source == "Industry Analyst").Select(a => a.Buy).FirstOrDefault() : null,
                    Sell = result.Select(a => a.Source).Contains("Industry Analyst") ? result.Where(a => a.Source == "Industry Analyst").Select(a => a.Sell).FirstOrDefault() : null,
                    Upside = result.Select(a => a.Source).Contains("Industry Analyst") ? result.Where(a => a.Source == "Industry Analyst").Select(a => a.Upside).FirstOrDefault() : null,
                    Date = result.Select(a => a.Source).Contains("Industry Analyst") ? result.Where(a => a.Source == "Industry Analyst").Select(a => a.Date).FirstOrDefault() : null,
                    DataId = result.Select(a => a.Source).Contains("Industry Analyst") ? result.Where(a => a.Source == "Industry Analyst").Select(a => a.DataId).FirstOrDefault() : null,
                });
                if (result != null && result.Count > 0)
                {
                    foreach (FairValueCompositionSummaryData item in result)
                    {
                        if (item.Source == "Primary Analyst" || item.Source == "Industry Analyst")
                        {                            
                            continue;
                        }
                        else
                        {
                            temp.Add(new FairValueData()
                            {
                                Source = item.Source,
                                Measure = item.Measure,
                                Buy = item.Buy,
                                Sell = item.Sell,
                                Upside = item.Upside,
                                Date = item.Date,
                                DataId = item.DataId,
                            });
                        }
                    }
                }
                FairValueCompositionData = temp;
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            finally { BusyIndicatorStatus = false; }
            Logging.LogEndMethod(_logger, methodNamespace);
        }
        

        /// <summary>
        /// Callback method that assigns value to the BAsicDataInfo property
        /// </summary>
        /// <param name="result">basic data </param>
        private void RetrieveFairValueDataWithNewUpsideCallbackMethod(FairValueCompositionSummaryData result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                Logging.LogMethodParameter(_logger, methodNamespace, result, 1);

                if (result != null)
                {
                    var item = FairValueCompositionData.FirstOrDefault(i => i.Source == result.Source);

                    if (item != null)
                    {
                        item.Upside = result.Upside;
                        item.IsUpdated = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            finally { BusyIndicatorStatus = false; }
            Logging.LogEndMethod(_logger, methodNamespace);
        }

        #endregion

        #region SERVICE CALL METOHD
        /// <summary>
        /// Calls web service method
        /// </summary>
        private void CallingWebMethod()
        {
            if (_securitySelectionData != null && IsActive)
            {
                _dbInteractivity.RetrieveFairValueCompostionSummary(_securitySelectionData, RetrieveFairValueCompositionSummaryDataCallbackMethod);
                BusyIndicatorStatus = true;
            }

        }
        #endregion

        #region EventUnSubscribe

        public void Dispose()
        {
            _eventAggregator.GetEvent<SecurityReferenceSetEvent>().Unsubscribe(HandleSecurityReferenceSet);
        }
        #endregion

        #region Helper Method

        public List<Measure> GetMeasureList()
        {
            MeasuresData.Add(new Measure() { DataId = 236, Measures = "Forward Dividend Yield" });
            MeasuresData.Add(new Measure() { DataId = 198, Measures = "Forward EV/EBITDA" });
            MeasuresData.Add(new Measure() { DataId = 246, Measures = "Forward EV/EBITDA relative to Country" });
            MeasuresData.Add(new Measure() { DataId = 247, Measures = "Forward EV/EBITDA relative to Industry" });
            MeasuresData.Add(new Measure() { DataId = 248, Measures = "Forward EV/EBITDA relative to Country Industry" });
            MeasuresData.Add(new Measure() { DataId = 237, Measures = "Forward EV/Revenue" });
            MeasuresData.Add(new Measure() { DataId = 238, Measures = "Forward P/NAV" });
            MeasuresData.Add(new Measure() { DataId = 239, Measures = "Forward P/Appraisal Value" });
            MeasuresData.Add(new Measure() { DataId = 188, Measures = "Forward P/BV" });
            MeasuresData.Add(new Measure() { DataId = 249, Measures = "Forward P/BV relative to Country" });
            MeasuresData.Add(new Measure() { DataId = 250, Measures = "Forward P/BV relative to Industry" });
            MeasuresData.Add(new Measure() { DataId = 251, Measures = "Forward P/BV relative to Country Industry" });
            MeasuresData.Add(new Measure() { DataId = 189, Measures = "Forward P/CE" });
            MeasuresData.Add(new Measure() { DataId = 187, Measures = "Forward P/E" });
            MeasuresData.Add(new Measure() { DataId = 252, Measures = "Forward P/E relative to Country" });
            MeasuresData.Add(new Measure() { DataId = 253, Measures = "Forward P/E relative to Industry" });
            MeasuresData.Add(new Measure() { DataId = 254, Measures = "Forward P/E relative to Country Industry" });
            MeasuresData.Add(new Measure() { DataId = 241, Measures = "Forward P/E to 2 Year Growth" });
            MeasuresData.Add(new Measure() { DataId = 242, Measures = "Forward P/E to 3 Year Growth" });
            MeasuresData.Add(new Measure() { DataId = 245, Measures = "Forward P/Embedded Value" });
            MeasuresData.Add(new Measure() { DataId = 197, Measures = "Forward P/Revenue" });

            return MeasuresData;
        }

        /// <summary>
        /// Set IsEditable based upon the primary/industry analyst property
        /// </summary>
        /// <param name="item"></param>
        private void SetIsEditableProprty(List<FairValueCompositionSummaryData> item)
        {
            string user = UserSession.SessionManager.SESSION.UserName;

            string primaryAnalyst = (from p in item
                                     select p.PrimaryAnalyst).FirstOrDefault();

            string industryAnalyst = (from p in item
                                     select p.IndustryAnalyst).FirstOrDefault();

            if (String.Equals(user, primaryAnalyst, StringComparison.InvariantCultureIgnoreCase)
                || String.Equals(user, industryAnalyst, StringComparison.InvariantCultureIgnoreCase))
            {
                IsReadOnly = false;
            }
        }

        #endregion
    }
}
