﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GreenField.Gadgets.ViewModels;
using GreenField.Common;
using GreenField.Gadgets.Helpers;

namespace GreenField.Gadgets.Views
{
    /// <summary>
    /// View Class for Performance Grid that has ViewModelPerformanceGrid as its data source
    /// </summary>
    public partial class ViewPerformanceGrid : ViewBaseUserControl
    {
        #region Constructor
        /// <summary>
        /// Constructor for the class having ViewModelPerformanceGrid as its data context
        /// </summary>
        /// <param name="dataContextSource"></param>
        public ViewPerformanceGrid(ViewModelPerformanceGrid dataContextSource)
        {
            InitializeComponent();
            this.DataContext = dataContextSource;
            dataContextSource.performanceGridDataLoadedEvent +=
            new DataRetrievalProgressIndicatorEventHandler(dataContextSource_performanceGridDataLoadedEvent);
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Data Retrieval Indicator
        /// </summary>
        /// <param name="e"></param>
        void dataContextSource_performanceGridDataLoadedEvent(DataRetrievalProgressIndicatorEventArgs e)
        {
            if (e.ShowBusy)
            {
               
                this.busyIndicatorGrid.IsBusy = true;         
            }
            else
            {
                this.busyIndicatorGrid.IsBusy = false;
            }
        }

        /// <summary>
        /// True is gadget is currently on display
        /// </summary>
        private bool _isActive;
        public override bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                if (this.DataContext != null)
                    ((ViewModelPerformanceGrid)DataContext).IsActive = _isActive;
            }
        }
        #endregion

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        private void dgPerformance_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            GroupedGridRowLoadedHandler.Implement(e);
        }
    }
}
