﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using GreenField.Common;
using GreenField.DataContracts;
using GreenField.ServiceCaller;

namespace GreenField.Gadgets.ViewModels
{
    /// <summary>
    /// View model for ViewRelativePerformanceSectorActivePosition class
    /// </summary>
    public class ViewModelRelativePerformanceSectorActivePosition : NotificationObject
    {
        #region Fields
        //MEF Singletons
        private IEventAggregator eventAggregator;
        private IDBInteractivity dbInteractivity;
        private ILoggerFacade logger;

        // selection data
        private PortfolioSelectionData portfolioSelectionDataInfo;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="param">DashboardGadgetParam</param>
        public ViewModelRelativePerformanceSectorActivePosition(DashboardGadgetParam param)
        {
            eventAggregator = param.EventAggregator;
            dbInteractivity = param.DBInteractivity;
            logger = param.LoggerFacade;
            portfolioSelectionDataInfo = param.DashboardGadgetPayload.PortfolioSelectionData;
            Period = param.DashboardGadgetPayload.PeriodSelectionData;
            EffectiveDate = param.DashboardGadgetPayload.EffectiveDate;

            if (effectiveDateInfo != null && portfolioSelectionDataInfo != null && Period != null && IsActive)
            {
                dbInteractivity.RetrieveRelativePerformanceSectorActivePositionData(portfolioSelectionDataInfo,Convert.ToDateTime(effectiveDateInfo),periodInfo,
                    RetrieveRelativePerformanceSectorActivePositionDataCallbackMethod);
                BusyIndicatorStatus = true;
            }
            
            if (eventAggregator != null)
            {
                eventAggregator.GetEvent<PortfolioReferenceSetEvent>().Subscribe(HandlePortfolioReferenceSet);
                eventAggregator.GetEvent<EffectiveDateReferenceSetEvent>().Subscribe(HandleEffectiveDateSet);
                eventAggregator.GetEvent<PeriodReferenceSetEvent>().Subscribe(HandlePeriodReferenceSet);
            }
        }
        #endregion

        #region Properties
        #region UI Fields
        /// <summary>
        /// Contains data to be displayed in the gadget
        /// </summary>
        private ObservableCollection<RelativePerformanceActivePositionData> relativePerformanceActivePositionInfo;
        public ObservableCollection<RelativePerformanceActivePositionData> RelativePerformanceActivePositionInfo
        {
            get { return relativePerformanceActivePositionInfo; }
            set
            {
                if (relativePerformanceActivePositionInfo != value)
                {
                    relativePerformanceActivePositionInfo = value;
                    RaisePropertyChanged(() => this.RelativePerformanceActivePositionInfo);
                }
            }
        }

        /// <summary>
        /// Effective date selected
        /// </summary>
        private DateTime? effectiveDateInfo;
        public DateTime? EffectiveDate
        {
            get { return effectiveDateInfo; }
            set
            {
                if (effectiveDateInfo != value)
                {
                    effectiveDateInfo = value;
                    RaisePropertyChanged(() => EffectiveDate);
                }
            }
        }

        /// <summary>
        /// Period selected
        /// </summary>
        private string periodInfo;
        public string Period
        {
            get { return periodInfo; }
            set
            {
                if (periodInfo != value)
                {
                    periodInfo = value;
                    RaisePropertyChanged(() => Period);
                }
            }
        }

        /// <summary>
        /// property to contain status value for busy indicator of the gadget
        /// </summary>
        private bool busyIndicatorStatus;
        public bool BusyIndicatorStatus
        {
            get { return busyIndicatorStatus; }
            set
            {
                if (busyIndicatorStatus != value)
                {
                    busyIndicatorStatus = value;
                    RaisePropertyChanged(() => BusyIndicatorStatus);
                }
            }
        }

        /// <summary>
        /// IsActive is true when parent control is displayed on UI
        /// </summary>
        private bool isActive;
        public bool IsActive
        {
            get{ return isActive; }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    if (effectiveDateInfo != null && portfolioSelectionDataInfo != null && Period != null && isActive)
                    {
                        dbInteractivity.RetrieveRelativePerformanceSectorActivePositionData(portfolioSelectionDataInfo, Convert.ToDateTime(effectiveDateInfo), 
                            periodInfo, RetrieveRelativePerformanceSectorActivePositionDataCallbackMethod);
                        BusyIndicatorStatus = true;
                    }
                }
            }
        }
        #endregion
        #endregion
               
        #region Event Handlers
        /// <summary>
        /// Event Handler to subscribed event 'PortfolioReferenceSetEvent'
        /// </summary>
        /// <param name="portfolioSelectionData">PortfolioSelectionData</param>
        public void HandlePortfolioReferenceSet(PortfolioSelectionData portfolioSelectionData)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(logger, methodNamespace);
            try
            {
                if (portfolioSelectionData != null)
                {
                    Logging.LogMethodParameter(logger, methodNamespace, portfolioSelectionData, 1);
                    portfolioSelectionDataInfo = portfolioSelectionData;
                    if (EffectiveDate != null && portfolioSelectionDataInfo != null && Period != null && IsActive)
                    {
                        dbInteractivity.RetrieveRelativePerformanceSectorActivePositionData(portfolioSelectionDataInfo,Convert.ToDateTime(effectiveDateInfo),
                            periodInfo, RetrieveRelativePerformanceSectorActivePositionDataCallbackMethod);
                        BusyIndicatorStatus = true;
                    }
                }
                else
                {
                    Logging.LogMethodParameterNull(logger, methodNamespace, 1);
                }
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(logger, ex);
            }
            Logging.LogEndMethod(logger, methodNamespace);
        }

        /// <summary>
        /// Event Handler to subscribed event 'EffectiveDateSet'
        /// </summary>
        /// <param name="effectiveDate"></param>
        public void HandleEffectiveDateSet(DateTime effectiveDate)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(logger, methodNamespace);
            try
            {
                if (effectiveDate != null)
                {
                    Logging.LogMethodParameter(logger, methodNamespace, effectiveDate, 1);
                    EffectiveDate = effectiveDate;
                    if (EffectiveDate != null && portfolioSelectionDataInfo != null && Period != null && IsActive)
                    {
                        dbInteractivity.RetrieveRelativePerformanceSectorActivePositionData(portfolioSelectionDataInfo, Convert.ToDateTime(effectiveDateInfo),
                            periodInfo, RetrieveRelativePerformanceSectorActivePositionDataCallbackMethod);
                        BusyIndicatorStatus = true;
                    }
                }
                else
                {
                    Logging.LogMethodParameterNull(logger, methodNamespace, 1);
                }
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(logger, ex);
            }
            Logging.LogEndMethod(logger, methodNamespace);
        }

        /// <summary>
        /// Event Handler to subscribed event 'PeriodReferenceSetEvent'
        /// </summary>
        /// <param name="period"></param>
        public void HandlePeriodReferenceSet(string period)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(logger, methodNamespace);
            try
            {
                if (period != null)
                {
                    Logging.LogMethodParameter(logger, methodNamespace, period, 1);
                    Period = period;
                    if (EffectiveDate != null && portfolioSelectionDataInfo != null && Period != null && IsActive)
                    {
                       dbInteractivity.RetrieveRelativePerformanceSectorActivePositionData(portfolioSelectionDataInfo, Convert.ToDateTime(effectiveDateInfo),
                           periodInfo, RetrieveRelativePerformanceSectorActivePositionDataCallbackMethod);
                       BusyIndicatorStatus = true;
                    }
                }
                else
                {
                    Logging.LogMethodParameterNull(logger, methodNamespace, 1);
                }
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(logger, ex);
            }
            Logging.LogEndMethod(logger, methodNamespace);
        }

        #endregion

        #region Callback Methods
        /// <summary>
        /// Callback method for RetrieveRelativePerformanceSectorActivePositionData Service call
        /// </summary>
        /// <param name="result">RelativePerformanceActivePositionData Collection</param>
        private void RetrieveRelativePerformanceSectorActivePositionDataCallbackMethod(List<RelativePerformanceActivePositionData> result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(logger, methodNamespace);
            try
            {
                if (result != null)
                {
                    Logging.LogMethodParameter(logger, methodNamespace, result, 1);
                    RelativePerformanceActivePositionInfo = new ObservableCollection<RelativePerformanceActivePositionData>(result);
                }
                else
                {
                    Logging.LogMethodParameterNull(logger, methodNamespace, 1);
                }               
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(logger, ex);
            }
            finally
            {
                BusyIndicatorStatus = false;
            }
            Logging.LogEndMethod(logger, methodNamespace);
        }
        #endregion

        #region Dispose Method
        /// <summary>
        /// method to dispose all subscribed events
        /// </summary>
        public void Dispose()
        {
            eventAggregator.GetEvent<PortfolioReferenceSetEvent>().Unsubscribe(HandlePortfolioReferenceSet);
            eventAggregator.GetEvent<EffectiveDateReferenceSetEvent>().Unsubscribe(HandleEffectiveDateSet);
            eventAggregator.GetEvent<PeriodReferenceSetEvent>().Unsubscribe(HandlePeriodReferenceSet);
        }
        #endregion
    }
}
