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
using System.Windows.Media.Imaging;

namespace GreenField.DashboardModule.Views
{
    [Export]
    public partial class ViewDashboardCompanyValuationDiscountedCashFlow : UserControl, INavigationAware
    {
        #region Fields
        private IEventAggregator _eventAggregator;
        private ILoggerFacade _logger;
        private IDBInteractivity _dBInteractivity;

        private List<string> _EPS_BVPS;
        public List<string> EPS_BVPS
        {
            get
            {
                if (_EPS_BVPS == null)
                {
                    _EPS_BVPS = new List<string>();
                }
                return _EPS_BVPS;
            }
            set { _EPS_BVPS = value; }
        }


        private List<Table> _dcfReport;
        public List<Table> DCFReport
        {
            get
            {
                if (_dcfReport == null)
                {
                    _dcfReport = new List<Table>();
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
            DCFReport = new List<Table>();
            RadDocument mergedDocument = new RadDocument();

            RadDocument finalReport = new RadDocument();
            int i = 0;
            foreach (RadTileViewItem item in this.rtvDashboard.Items)
            {
                ViewBaseUserControl control = (ViewBaseUserControl)item.Content;
                Table table = control.CreateDocument();
                if (table != null)
                {
                    DCFReport.Add(table);
                }

                //if ((item.Content as Telerik.Windows.Controls.HeaderedContentControl).Content == "FORWARD EPS")
                //{
                //    EPS_BVPS = control.EPS_BVPS();
                //}
            }
            if (DCFReport != null)
            {
                finalReport = MergeDocuments(DCFReport);
                finalReport.SectionDefaultPageMargin = new Telerik.Windows.Documents.Layout.Padding() { All = 10 };
                PDFExporter.ExportPDF_RadDocument(finalReport, 10);
            }
        }

        private RadDocument _finalReport;
        public RadDocument FinalReport
        {
            get
            {
                if (_finalReport == null)
                    _finalReport = new RadDocument();
                return _finalReport;
            }
            set { _finalReport = value; }
        }


        /// <summary>
        /// Method to Merge Multiple RadDocuments
        /// </summary>
        /// <param name="tables">Array of type RadDocuments</param>
        /// <returns>Merged Documents</returns>
        private RadDocument MergeDocuments(List<Table> tables)
        {
            RadDocument mergedDocument = new RadDocument();
            Telerik.Windows.Documents.Model.Section section = new Telerik.Windows.Documents.Model.Section();
            Telerik.Windows.Documents.Model.Section newSection = new Telerik.Windows.Documents.Model.Section();
            Table documentTable = new Table(tables.Count(), 1);
            mergedDocument.Sections.Add(section);
            mergedDocument.Sections.Add(newSection);
            int i = 0;
            foreach (Table item in tables)
            {
                Telerik.Windows.Documents.Model.Paragraph para = new Telerik.Windows.Documents.Model.Paragraph() { SpacingBefore = 10 };
                if (i < 4)
                {
                    Telerik.Windows.Documents.Model.Span span = new Telerik.Windows.Documents.Model.Span(ReturnGadgetName(i));
                    span.FontSize = 10;
                    para.Children.Add(span);
                    section.Blocks.Add(para);
                    section.Blocks.Add(item);
                }
                else
                {
                    Telerik.Windows.Documents.Model.Span span = new Telerik.Windows.Documents.Model.Span(ReturnGadgetName(i));
                    span.FontSize = 10;
                    para.Children.Add(span);
                    newSection.Blocks.Add(para);
                    newSection.Blocks.Add(item);
                }
                i++;
            }
            return mergedDocument;
        }

        private string ReturnGadgetName(int order)
        {
            if (EPS_BVPS.Count == 0)
            {
                EPS_BVPS.Add(" ");
                EPS_BVPS.Add(" ");
            }

            switch (order)
            {
                case 0:
                    return "ASSUMPTIONS";
                case 1:
                    return "FREE CASH FLOWS";
                case 2:
                    return "TERMINAL VALUE CALCULATIONS";
                case 3:
                    return "SUMMARY";
                case 4:
                    return "SENSITIVITY";
                case 5:
                    return "SENSITIVITY EPS ";// + "EPS= " + Convert.ToString(EPS_BVPS[0]);
                case 6:
                    return "SENSITIVITY BVPS ";// + "BVPS= " + Convert.ToString(EPS_BVPS[1]);
                default:
                    return string.Empty;
            }
        }
    }
}