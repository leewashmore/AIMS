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

namespace GreenField.Gadgets.Views
{
    public partial class ViewTopContributor : ViewBaseUserControl
    {
        public ViewTopContributor(ViewModelTopContributor DataContextSource)
        {
            InitializeComponent();
            this.DataContext = DataContextSource;
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
