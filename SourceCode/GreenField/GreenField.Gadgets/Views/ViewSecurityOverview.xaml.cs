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
using GreenField.Gadgets.ViewModels;
using GreenField.Gadgets.Helpers;

namespace GreenField.Gadgets.Views
{
    public partial class ViewSecurityOverview : ViewBaseUserControl
    {
        public ViewSecurityOverview( ViewModelSecurityOverview dataContextSource)
        {
            InitializeComponent();
            this.DataContext = dataContextSource;
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
