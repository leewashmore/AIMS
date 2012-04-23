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
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Events;
using GreenField.ServiceCaller;
using Microsoft.Practices.Prism.Logging;
using GreenField.ServiceCaller.SecurityReferenceDefinitions;
using System.Linq;
using GreenField.Common;
using Microsoft.Practices.Prism.ViewModel;
using GreenField.Gadgets.Models;
using System.Collections.Generic;
using GreenField.ServiceCaller.BenchmarkHoldingsPerformanceDefinitions;

namespace GreenField.Gadgets.ViewModels
{
    /// <summary>
    /// view model for ViewRegionBreakDown
    /// </summary>
    public class ViewModelRegionBreakdown : NotificationObject
    {
        #region Fields
        
        /// <summary>
        /// MEF Singletons
        /// </summary>
        private IEventAggregator _eventAggregator;
        private IDBInteractivity _dbInteractivity;
        private ILoggerFacade _logger;

        /// <summary>
        /// DashboardGadgetPayLoad fields
        /// </summary>
        private PortfolioSelectionData _PortfolioSelectionData;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="param">DashBoardGadgetParam</param>
        public ViewModelRegionBreakdown(DashboardGadgetParam param)    
        {
            _eventAggregator = param.EventAggregator;
            _dbInteractivity = param.DBInteractivity;
            _logger = param.LoggerFacade;

            _PortfolioSelectionData = param.DashboardGadgetPayload.PortfolioSelectionData;
            EffectiveDate = param.DashboardGadgetPayload.EffectiveDate;

            if (EffectiveDate != null && _PortfolioSelectionData != null)
            {
                _dbInteractivity.RetrieveRegionBreakdownData(_PortfolioSelectionData, _effectiveDate, RetrieveRegionBreakdownDataCallbackMethod);
            }

            if (_eventAggregator != null)
            {
                _eventAggregator.GetEvent<PortfolioReferenceSetEvent>().Subscribe(HandlePortfolioReferenceSet);
                _eventAggregator.GetEvent<EffectiveDateReferenceSetEvent>().Subscribe(HandleEffectiveDateSet);
            }
        }
        #endregion

        #region Properties
        #region UI Fields

        /// <summary>
        /// contains data for the grid in the gadget
        /// </summary>
        private ObservableCollection<RegionBreakdownData> _regionBreakdownInfo;
        public ObservableCollection<RegionBreakdownData> RegionBreakdownInfo
        {
            get { return _regionBreakdownInfo; }
            set
            {
                if (_regionBreakdownInfo != value)
                {
                    _regionBreakdownInfo = value;
                    RaisePropertyChanged(() => this.RegionBreakdownInfo);
                }
            }
        }

        /// <summary>
        /// contains data for the chart in the gadget
        /// </summary>
        private ObservableCollection<RegionSpecificData> _regionSpecificInfo;
        public ObservableCollection<RegionSpecificData> RegionSpecificInfo
        {
            get { return _regionSpecificInfo; }
            set
            {
                if (_regionSpecificInfo != value)
                {
                    _regionSpecificInfo = value;
                    RaisePropertyChanged(() => this.RegionSpecificInfo);
                }
            }
        }

        private DateTime _effectiveDate;
        public DateTime EffectiveDate
        {
            get { return _effectiveDate; }
            set 
            {
                if (_effectiveDate != value)
                {
                    _effectiveDate = value;
                    RaisePropertyChanged(() => EffectiveDate);
                }
            }
        }
        
        #endregion
        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler to subscribed event 'FundReferenceSetEvent'
        /// </summary>
        /// <param name="PortfolioSelectionData">PortfolioSelectionData</param>
        public void HandlePortfolioReferenceSet(PortfolioSelectionData PortfolioSelectionData)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);

            try
            {
                if (PortfolioSelectionData != null)
                {
                    Logging.LogMethodParameter(_logger, methodNamespace, PortfolioSelectionData, 1);
                    _PortfolioSelectionData = PortfolioSelectionData;
                    if (EffectiveDate != null && _PortfolioSelectionData != null)
                    {
                        _dbInteractivity.RetrieveRegionBreakdownData(_PortfolioSelectionData, _effectiveDate, RetrieveRegionBreakdownDataCallbackMethod);
                    }
                }
                else
                {
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            Logging.LogEndMethod(_logger, methodNamespace);
        }

        /// <summary>
        /// Event Handler to subscribed event 'EffectiveDateSet'
        /// </summary>
        /// <param name="effectiveDate">DateTime</param>
        public void HandleEffectiveDateSet(DateTime effectiveDate)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (effectiveDate != null)
                {
                    Logging.LogMethodParameter(_logger, methodNamespace, effectiveDate, 1);
                    EffectiveDate = effectiveDate;
                    if (EffectiveDate != null && _PortfolioSelectionData != null)
                    {
                        _dbInteractivity.RetrieveRegionBreakdownData(_PortfolioSelectionData,_effectiveDate, RetrieveRegionBreakdownDataCallbackMethod);
                    }
                }
                else
                {
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            Logging.LogEndMethod(_logger, methodNamespace);
        }
      
        #endregion

        #region Callback Methods

        /// <summary>
        /// Callback method for RetrieveRegionBreakdownData service call
        /// </summary>
        /// <param name="regionBreakdownData">RegionBreakdownData collection</param>
        private void RetrieveRegionBreakdownDataCallbackMethod(List<RegionBreakdownData> regionBreakdownData)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (regionBreakdownData != null)
                {
                    Logging.LogMethodParameter(_logger, methodNamespace, regionBreakdownData, 1);
                    RegionBreakdownInfo = new ObservableCollection<RegionBreakdownData>(regionBreakdownData);
                    foreach (RegionBreakdownData item in RegionBreakdownInfo)
                    {
                        if (RegionSpecificInfo == null)
                        {
                            RegionSpecificInfo = new ObservableCollection<RegionSpecificData>();
                        }
                        if (RegionSpecificInfo.Where(i => i.Region == item.Region).Count().Equals(0))
                        {
                            RegionSpecificInfo.Add(new RegionSpecificData()
                            {
                                Region = item.Region,
                                PortfolioShare = RegionBreakdownInfo.Where(t => t.Region == item.Region).Sum(r => r.PortfolioShare)
                            });
                        }
                    }

                }
                else
                {
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            Logging.LogEndMethod(_logger, methodNamespace);
        }
        #endregion

        #region Dispose Method

        public void Dispose()
        {
            _eventAggregator.GetEvent<PortfolioReferenceSetEvent>().Unsubscribe(HandlePortfolioReferenceSet);
            _eventAggregator.GetEvent<EffectiveDateReferenceSetEvent>().Unsubscribe(HandleEffectiveDateSet);

        }

        #endregion
    }
}
