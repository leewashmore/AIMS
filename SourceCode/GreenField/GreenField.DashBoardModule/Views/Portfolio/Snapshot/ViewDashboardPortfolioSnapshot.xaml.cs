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

namespace GreenField.DashboardModule.Views
{
    [Export]
    public partial class ViewDashboardPortfolioSnapshot : UserControl
    {
        #region Fields
        private IEventAggregator _eventAggregator;
        private ILoggerFacade _logger;
        private IDBInteractivity _dBInteractivity;
        #endregion

        [ImportingConstructor]
        public ViewDashboardPortfolioSnapshot(ILoggerFacade logger, IEventAggregator eventAggregator,
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

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.HOLDINGS_TOP_TEN_HOLDINGS, Foreground = new SolidColorBrush(Colors.White), FontSize = 8, FontFamily = new FontFamily("Arial") },
                RestoredHeight = 400,
                Content = new ViewTopHoldings(new ViewModelTopHoldings(param))
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                RestoredHeight = 400,
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.HOLDINGS_VALUE_GROWTH, Foreground = new SolidColorBrush(Colors.White), FontSize = 8, FontFamily = new FontFamily("Arial") },
                Content = null
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                RestoredHeight = 400,
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.HOLDINGS_REGION_BREAKDOWN, Foreground = new SolidColorBrush(Colors.White), FontSize = 8, FontFamily = new FontFamily("Arial") },
                Content = new ViewRegionBreakdown(new ViewModelRegionBreakdown(param))
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                RestoredHeight = 400,
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.HOLDINGS_VALUATION_QUALITY_GROWTH_MEASURES, Foreground = new SolidColorBrush(Colors.White), FontSize = 8, FontFamily = new FontFamily("Arial") },
                Content = null
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.HOLDINGS_SECTOR_BREAKDOWN, Foreground = new SolidColorBrush(Colors.White), FontSize = 8, FontFamily = new FontFamily("Arial") },
                RestoredHeight = 400,
                Content = new ViewSectorBreakdown(new ViewModelSectorBreakdown(param))
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                RestoredHeight = 400,
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.HOLDINGS_RISK_RETURN, Foreground = new SolidColorBrush(Colors.White), FontSize = 8, FontFamily = new FontFamily("Arial") },
                Content = new ViewPortfolioRiskReturns(new ViewModelPortfolioRiskReturns(param))
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                RestoredHeight = 400,
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.HOLDINGS_ASSET_ALLOCATION, Foreground = new SolidColorBrush(Colors.White), FontSize = 8, FontFamily = new FontFamily("Arial") },
                Content = new ViewAssetAllocation(new ViewModelAssetAllocation(param))
            });

            this.rtvDashboard.Items.Add(new RadTileViewItem
            {
                RestoredHeight = 400,
                Header = new Telerik.Windows.Controls.HeaderedContentControl { Content = GadgetNames.HOLDINGS_MARKET_CAPITALIZATION, Foreground = new SolidColorBrush(Colors.White), FontSize = 8, FontFamily = new FontFamily("Arial") },
                Content = new ViewMarketCapitalization(new ViewModelMarketCapitalization(param))
            });
            
        }
    }
}