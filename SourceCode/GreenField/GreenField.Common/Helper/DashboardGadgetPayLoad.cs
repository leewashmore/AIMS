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
using GreenField.ServiceCaller.SecurityReferenceDefinitions;
using GreenField.ServiceCaller.BenchmarkHoldingsPerformanceDefinitions;
using System.Collections.Generic;

namespace GreenField.Common.Helper
{
    public class DashboardGadgetPayload
    {
        public EntitySelectionData EntitySelectionData { get; set; }

        public PortfolioSelectionData PortfolioSelectionData { get; set; }

        public BenchmarkSelectionData BenchmarkSelectionData { get; set; }

        public DateTime? EffectiveDate { get; set; }
        
        public PeriodSelectionData PeriodSelectionData{ get; set; }        

        public MarketSnapshotSelectionData MarketSnapshotSelectionData { get; set; }

        public KeyValuePair<String, String> HoldingDataFilter { get; set; }
    }
}
