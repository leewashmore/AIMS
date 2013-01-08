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
using System.Diagnostics;

namespace GreenField.IssuerShares.Controls
{
    public class LoadedCommunicationStateModel : ICommunicationStateModel
    {
        [DebuggerStepThrough]
        public void Accept(ICommunicationStateModelResolver resolver)
        {
            resolver.Resolve(this);
        }
    }
}
