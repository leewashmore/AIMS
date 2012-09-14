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
using System.ComponentModel.Composition;
using GreenField.ServiceCaller;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using GreenField.Common.Helper;
using Telerik.Windows.Controls;
using System.Reflection;
using GreenField.DashboardModule.Helpers;
using GreenField.Common;
using GreenField.DashBoardModule.Helpers;
using GreenField.Gadgets.Views;
using GreenField.Gadgets.ViewModels;
using Microsoft.Practices.Prism.Regions;
using GreenField.Gadgets.Helpers;
using Telerik.Windows.Documents.Model;

namespace GreenField.DashboardModule.Views
{
    [Export]
    public partial class ViewDashboardCompanyValuationDiscountedCashFlow : UserControl, INavigationAware
    {
        #region Fields
        private IEventAggregator _eventAggregator;
        private ILoggerFacade _logger;
        private IDBInteractivity _dBInteractivity;

        private RadDocument[] _dcfReport;
        public RadDocument[] DCFReport
        {
            get
            {
                if (_dcfReport == null)
                { 
                    _dcfReport= new RadDocument[7];
                }
                return _dcfReport;
            }
            set
            {
                _dcfReport = value;
            }
        }

        #endregion

        [ImportingConstructor]
        public ViewDashboardCompanyValuationDiscountedCashFlow(ILoggerFacade logger, IEventAggregator eventAggregator,
            IDBInteractivity dbInteractivity)
        {
            InitializeComponent();

            _eventAggregator = eventAggregator;
            _logger = logger;
            _dBInteractivity = dbInteractivity;

            _eventAggregator.GetEvent<DashboardGadgetLoad>().Subscribe(HandleDashboardGadgetLoad);

            //this.tbHeader.Text = GadgetNames.HOLDINGS_DISCOUNTED_CASH_FLOW;

        }

        public void HandleDashboardGadgetLoad(DashboardGadgetPayload payload)
        {
            //if (this.cctrDashboardContent.Content != null)
            //    return;
            if (this.rtvDashboard.Items.Count > 0)
                return;
            DashboardGadgetParam param = new DashboardGadgetParam()
            {
                DashboardGadgetPayload = payload,
                DBInteractivity = _dBInteractivity,
                EventAggregator = _eventAggregator,
                LoggerFacade = _logger
            };

            ViewModelDCF _viewModel = new ViewModelDCF(param);

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl
                {
                    Content = "Assumptions",
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = 12,
                    FontFamily = new FontFamily("Arial")
                },
                Content = new ViewAnalysisSummary(_viewModel)
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl
                {
                    Content = GadgetNames.HOLDINGS_FREE_CASH_FLOW,
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = 12,
                    FontFamily = new FontFamily("Arial")
                },
                Content = new ViewFreeCashFlows(new ViewModelFreeCashFlows(param))
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl
                {
                    Content = "Terminal Value Calculations",
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = 12,
                    FontFamily = new FontFamily("Arial")
                },
                Content = new ViewTerminalValueCalculations(_viewModel)
            });
            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl
                {
                    Content = "DCF Summary",
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = 12,
                    FontFamily = new FontFamily("Arial")
                },
                Content = new ViewDCFSummary(_viewModel)
            });
            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl
                {
                    Content = "Sensitivity",
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = 12,
                    FontFamily = new FontFamily("Arial")
                },
                Content = new ViewSensitivity(_viewModel)
            });
            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl
                {
                    Content = "FORWARD EPS",
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = 12,
                    FontFamily = new FontFamily("Arial")
                },
                Content = new ViewSensitivityEPS(_viewModel)
            });
            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl
                {
                    Content = "FORWARD BVPS",
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = 12,
                    FontFamily = new FontFamily("Arial")
                },
                Content = new ViewSensitivityBVPS(_viewModel)
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            SetIsActiveOnDahsboardItems(false);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SetIsActiveOnDahsboardItems(true);
        }
        private void SetIsActiveOnDahsboardItems(bool value)
        {
            int a = rtvDashboard.Items.Count;
            foreach (RadTileViewItem item in this.rtvDashboard.Items)
            {
                ViewBaseUserControl control = (ViewBaseUserControl)item.Content;
                if (control != null)
                    control.IsActive = value;
            }
        }

        /// <summary>
        /// Generate DCF Report PDF
        /// </summary>
        /// <param name="sender">Sender of Event</param>
        /// <param name="e"></param>
        private void btnPDF_Click(object sender, RoutedEventArgs e)
        {
            RadDocument finalReport = new RadDocument();
            int i = 0;
            foreach (RadTileViewItem item in this.rtvDashboard.Items)
            {
                ViewBaseUserControl control = (ViewBaseUserControl)item.Content;
                RadDocument document = control.CreateDocument();
                if (document != null)
                {
                    DCFReport[i] = document;
                    
                }
                else
                {
                    DCFReport[i] = new RadDocument();
                }
                i++;
            }
            if (DCFReport != null)
            {
                finalReport = MergeDocuments(DCFReport);
                PDFExporter.ExportPDF_RadDocument(finalReport, 12);
            }
        }

        /// <summary>
        /// Method to Merge Multiple RadDocuments
        /// </summary>
        /// <param name="documents">Array of type RadDocuments</param>
        /// <returns>Merged Documents</returns>
        private RadDocument MergeDocuments(RadDocument[] documents)
        {
            RadDocument mergedDocument = new RadDocument();           
            foreach (RadDocument document in documents)
            {
                foreach (Telerik.Windows.Documents.Model.Section section in document.Sections)
                {
                    Telerik.Windows.Documents.Model.Section copySection = section.CreateDeepCopy() as Telerik.Windows.Documents.Model.Section;
                    document.Sections.Remove(section);
                    mergedDocument.Sections.Add(copySection);
                }
            }
            return mergedDocument;
        }
    }
}
