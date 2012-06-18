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

namespace GreenField.Gadgets.Views
{
    public partial class ViewConsensusEstimatesDetails : UserControl
    {
        public ViewConsensusEstimatesDetails(ViewModelConsensusEstimatesDetails dataContextSource)
        {
            InitializeComponent();
            this.DataContext = dataContextSource;
        }
    }
}
