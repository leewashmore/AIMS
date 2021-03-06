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
using GreenField.Gadgets.Helpers;
using GreenField.Common;
using Telerik.Windows.Controls;
using GreenField.ServiceCaller;

namespace GreenField.Gadgets.Views
{
    public partial class ViewQuarterlyResultsComparison : ViewBaseUserControl
    {
        #region Constructor
        public ViewQuarterlyResultsComparison(ViewModelQuarterlyResultsComparison dataContextSource)
        {
            InitializeComponent();
            this.DataContext = dataContextSource;
            this.DataContextQuarterlyResultsComparison = dataContextSource; 
            dataContextSource.quarterlyResultsComoarisonDataLoadedEvent +=
           new DataRetrievalProgressIndicatorEventHandler(dataContextSource_quarterlyResultsComoarisonDataLoadedEvent);
        }
        #endregion

        /// <summary>
        /// Data Retrieval Indicator
        /// </summary>
        /// <param name="e"></param>
        void dataContextSource_quarterlyResultsComoarisonDataLoadedEvent(DataRetrievalProgressIndicatorEventArgs e)
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

        #region RemoveEvents
        /// <summary>
        /// Disposing events
        /// </summary>
        public override void Dispose()
        {
            this.DataContextQuarterlyResultsComparison.quarterlyResultsComoarisonDataLoadedEvent -= new DataRetrievalProgressIndicatorEventHandler(dataContextSource_quarterlyResultsComoarisonDataLoadedEvent);            
            this.DataContextQuarterlyResultsComparison = null;
            this.DataContext = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Property of the type of View Model for this view
        /// </summary>
        private ViewModelQuarterlyResultsComparison _dataContextQuarterlyResultsComparison;
        public ViewModelQuarterlyResultsComparison DataContextQuarterlyResultsComparison
        {
            get { return _dataContextQuarterlyResultsComparison; }
            set { _dataContextQuarterlyResultsComparison = value; }
        }

        /// <summary>
        /// True if gadget is currently on display
        /// </summary>
        private bool _isActive;
        public override bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                if (this.DataContext != null)
                    ((ViewModelQuarterlyResultsComparison)this.DataContext).IsActive = _isActive;
            }
        }
        #endregion

        #region Export To Excel Methods
        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                ChildExportOptions childExportOptions = new ChildExportOptions(new List<RadExportOptions>
                {
                    new RadExportOptions() 
                    {
                        Element = this.dgQuarterlyComparison,
                        ElementName = "Quarterly Comparison Results",
                        ExportFilterOption = RadExportFilterOption.RADGRIDVIEW_EXCEL_EXPORT_FILTER
                    } 
                }, "Export Options: " + GadgetNames.QUARTERLY_RESULTS_COMPARISON);
                childExportOptions.Show();
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Printing the DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                ChildExportOptions childExportOptions = new ChildExportOptions(new List<RadExportOptions>
                {
                    new RadExportOptions() 
                    {
                        Element = this.dgQuarterlyComparison,
                        ElementName = "Quarterly Comparison Results",
                        ExportFilterOption = RadExportFilterOption.RADGRIDVIEW_PRINT_FILTER,
                        RichTextBox = this.RichTextBox
                    } 
                }, "Export Options: " + GadgetNames.QUARTERLY_RESULTS_COMPARISON);
                childExportOptions.Show();
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Event handler when user wants to Export the Grid to PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                ChildExportOptions childExportOptions = new ChildExportOptions(new List<RadExportOptions>
                {
                    new RadExportOptions() 
                    {
                        Element = this.dgQuarterlyComparison,
                        ElementName = "Quarterly Comparison Results",
                        ExportFilterOption = RadExportFilterOption.RADGRIDVIEW_PDF_EXPORT_FILTER,
                        RichTextBox = this.RichTextBox
                    } 
                }, "Export Options: " + GadgetNames.QUARTERLY_RESULTS_COMPARISON);
                childExportOptions.Show();
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
            }
        }

        private void dgQuarterlyResults_ElementExporting(object sender, GridViewElementExportingEventArgs e)
        {
            RadGridView_ElementExport.ElementExporting(e, isGroupFootersVisible: false);
        }

        #endregion

    }
}
