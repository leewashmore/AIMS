﻿using System.Linq;
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

namespace GreenField.DashboardModule.Views
{
    [Export]
    public partial class ViewDashboardPortfolioBenchmarkSummary : UserControl, INavigationAware
    {
        #region Fields
        private IEventAggregator _eventAggregator;
        private ILoggerFacade _logger;
        private IDBInteractivity _dBInteractivity;
        #endregion

        [ImportingConstructor]
        public ViewDashboardPortfolioBenchmarkSummary(ILoggerFacade logger, IEventAggregator eventAggregator,
            IDBInteractivity dbInteractivity)
        {
            InitializeComponent();

            _eventAggregator = eventAggregator;
            _logger = logger;
            _dBInteractivity = dbInteractivity;

            _eventAggregator.GetEvent<DashboardGadgetLoad>().Subscribe(HandleDashboardGadgetLoad);

        }

        public void HandleDashboardGadgetLoad(DashboardGadgetPayload payload)
        {
            if (this.rtvDashboard.Items.Count > 0)
                return;

            DashboardGadgetParam param = new DashboardGadgetParam()
            {
                DashboardGadgetPayload = payload,
                DBInteractivity = _dBInteractivity,
                EventAggregator = _eventAggregator,
                LoggerFacade = _logger
            };

            /* TODO Hide for now
            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.BENCHMARKS_MULTILINE_BENCHMARK, Foreground = new SolidColorBrush(Colors.Black), FontSize = 12, FontFamily = new FontFamily("Arial") },
                RestoredHeight=300,
                Content = new ViewMultiLineBenchmark(new ViewModelMultiLineBenchmark(param))
            });
            
            */

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.BENCHMARK_TOP_TEN_CONSTITUENTS, Foreground = new SolidColorBrush(Colors.Black), FontSize = 12, FontFamily = new FontFamily("Arial") },
                RestoredHeight = 300,
                Content = new ViewTopBenchmarkSecurities(new ViewModelTopBenchmarkSecurities(param))
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.BENCHMARK_HOLDINGS_SECTOR_PIECHART, Foreground = new SolidColorBrush(Colors.Black), FontSize = 12, FontFamily = new FontFamily("Arial") },
                RestoredHeight = 320,
                Content = new ViewHoldingsPieChart(new ViewModelHoldingsPieChart(param))
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.BENCHMARK_HOLDINGS_REGION_PIECHART, Foreground = new SolidColorBrush(Colors.Black), FontSize = 12, FontFamily = new FontFamily("Arial") },
                RestoredHeight = 320,
                Content = new ViewHoldingsPieChartRegion(new ViewModelHoldingsPieChartRegion(param))
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
            foreach (RadTileViewItem item in this.rtvDashboard.Items)
            {
                ViewBaseUserControl control = (ViewBaseUserControl)item.Content;
                if (control != null)
                    control.IsActive = value;
            }
        }
    }
}
