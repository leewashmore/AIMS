﻿using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Telerik.Windows.Controls;
using GreenField.Common;
using GreenField.Common.Helper;
using GreenField.Gadgets.Helpers;
using GreenField.ServiceCaller;

namespace GreenField.DashboardModule.Views
{
    /// <summary>
    /// Code behind for ViewDashboardCompanyEstimatesComparison
    /// </summary>
    [Export]
    public partial class ViewDashboardCompanyEstimatesComparison : UserControl, INavigationAware
    {
        #region Fields
        /// <summary>
        /// MEF event aggreagator instance
        /// </summary>
        private IEventAggregator eventAggregator;

        /// <summary>
        /// Logging instance
        /// </summary>
        private ILoggerFacade logger;

        /// <summary>
        /// Service caller instance
        /// </summary>
        private IDBInteractivity dBInteractivity;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger1">ILoggerFacade</param>
        /// <param name="eventAggregator1">IEventAggregator</param>
        /// <param name="dbInteractivity">IDBInteractivity</param>        
        [ImportingConstructor]
        public ViewDashboardCompanyEstimatesComparison(ILoggerFacade logger1, IEventAggregator eventAggregator1,
            IDBInteractivity dbInteractivity)
        {
            InitializeComponent();
            eventAggregator = eventAggregator1;
            logger = logger1;    
            dBInteractivity = dbInteractivity;
            eventAggregator.GetEvent<DashboardGadgetLoad>().Subscribe(HandleDashboardGadgetLoad);            
        }
        #endregion

        #region Event Handler
        /// <summary>
        /// DashboardGadgetLoad Event Handler
        /// </summary>
        /// <param name="payload">DashboardGadgetPayload</param>
        public void HandleDashboardGadgetLoad(DashboardGadgetPayload payload)
        {
            if (this.rtvDashboard.Items.Count > 0)
            {
                return;
            }
            DashboardGadgetParam param = new DashboardGadgetParam()
            {
                DashboardGadgetPayload = payload,
                DBInteractivity = dBInteractivity,
                EventAggregator = eventAggregator,
                LoggerFacade = logger
            };

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                
                Header = "",//GadgetNames.EXTERNAL_RESEARCH_CONSENSUS_COMPARISON_CHART,
                Content = null
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {

                Header = "",//GadgetNames.EXTERNAL_RESEARCH_CONSENSUS_ESTIMATES_SUMMARY,
                Content = null
            });            
        }
        #endregion

        #region INavigationAware methods
        /// <summary>
        /// Returns true if satisfies requisite condition
        /// </summary>
        /// <param name="navigationContext">NavigationContext</param>
        /// <returns>True/False</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// Executed on navigation from this view
        /// </summary>
        /// <param name="navigationContext">NavigationContext</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            SetIsActiveOnDahsboardItems(false);
        }

        /// <summary>
        /// Executed on navigation to this view
        /// </summary>
        /// <param name="navigationContext">NavigationContext</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SetIsActiveOnDahsboardItems(true);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Set IsActive property on Dashboard content
        /// </summary>
        /// <param name="value">IsActive value</param>
        private void SetIsActiveOnDahsboardItems(bool value)
        {
            foreach (RadTileViewItem item in this.rtvDashboard.Items)
            {
                ViewBaseUserControl control = (ViewBaseUserControl)item.Content;
                if (control != null)
                    control.IsActive = value;
            }
        }
        #endregion
    }
}
