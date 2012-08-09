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
//using Ashmore.Emm.GreenField.ICP.Meeting.Module.ViewModels;
using GreenField.Gadgets.ViewModels;
using GreenField.Gadgets.Helpers;

namespace GreenField.Gadgets.Views
{
    public partial class ViewMeetingsAgenda : ViewBaseUserControl
    {
        #region Properties
        /// <summary>
        /// property to set data context
        /// </summary>
        private ViewModelMeetingsAgenda _dataContextViewModelMeetingsAgenda;
        public ViewModelMeetingsAgenda DataContextViewModelMeetingsAgenda
        {
            get { return _dataContextViewModelMeetingsAgenda; }
            set { _dataContextViewModelMeetingsAgenda = value; }
        }



        /// <summary>
        /// property to set IsActive variable of View Model
        /// </summary>
        private bool _isActive;
        public override bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                if (DataContextViewModelMeetingsAgenda != null) //DataContext instance
                    DataContextViewModelMeetingsAgenda.IsActive = _isActive;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataContextSource"></param>
        public ViewMeetingsAgenda(ViewModelMeetingsAgenda dataContextSource)
        {
            InitializeComponent();
            this.DataContext = dataContextSource;
            this.DataContextViewModelMeetingsAgenda = dataContextSource;
        }
        #endregion

        #region Dispose Method
        /// <summary>
        /// method to dispose all running events
        /// </summary>
        public override void Dispose()
        {
            this.DataContextViewModelMeetingsAgenda.Dispose();
            this.DataContextViewModelMeetingsAgenda = null;
            this.DataContext = null;
        }
        #endregion
    }
}

