﻿using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using GreenField.ServiceCaller;
using Microsoft.Practices.Prism.ViewModel;
using GreenField.ServiceCaller.MeetingDefinitions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Commands;
using GreenField.Gadgets.Models;
using Microsoft.Practices.Prism.Regions;
using GreenField.Common;
using GreenField.Gadgets.Views;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using GreenField.Gadgets.Helpers;
//using System.Configuration;



namespace GreenField.Gadgets.ViewModels
{
    public class ViewModelMeetingConfigSchedule : NotificationObject
    {

        #region Fields
        private Boolean calculationFlag = false;
        private DateTime SUNDAY = new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Local);
        private DateTime MONDAY = new DateTime(2012, 1, 2, 0, 0, 0, DateTimeKind.Local);
        private DateTime TUESDAY = new DateTime(2012, 1, 3, 0, 0, 0, DateTimeKind.Local);
        private DateTime WEDNESDAY = new DateTime(2012, 1, 4, 0, 0, 0, DateTimeKind.Local);
        private DateTime THURSDAY = new DateTime(2012, 1, 5, 0, 0, 0, DateTimeKind.Local);
        private DateTime FRIDAY = new DateTime(2012, 1, 6, 0, 0, 0, DateTimeKind.Local);
        private DateTime SATURDAY = new DateTime(2012, 1, 7, 0, 0, 0, DateTimeKind.Local);

        // private ManageMeetings _manageMeetings;
        /// <summary>
        /// Event Aggregator
        /// </summary>
        private IEventAggregator _eventAggregator;

        /// <summary>
        /// Instance of Service Caller Class
        /// </summary>
        private IDBInteractivity _dbInteractivity;

        /// <summary>
        /// Instance of LoggerFacade
        /// </summary>
        private ILoggerFacade _logger;

        /// <summary>
        /// IsActive is true when parent control is displayed on UI
        /// </summary>
        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                if (value)
                {
                    if (_dbInteractivity != null)
                    {
                        BusyIndicatorNotification(true, "Retrieving Investment Committee Meeting configuration details...");
                        _dbInteractivity.GetMeetingConfigSchedule(GetMeetingConfigScheduleCallbackMethod);
                    }
                }
            }
        }
        #endregion

        #region Constructor
        public ViewModelMeetingConfigSchedule(DashboardGadgetParam param)
        {

            _dbInteractivity = param.DBInteractivity;
            _logger = param.LoggerFacade;
            _eventAggregator = param.EventAggregator;

            while(SUNDAY.DayOfWeek != DayOfWeek.Sunday)
            {
                SUNDAY = SUNDAY.AddDays(1);
                MONDAY = MONDAY.AddDays(1);
                TUESDAY = TUESDAY.AddDays(1);
                WEDNESDAY = WEDNESDAY.AddDays(1);
                THURSDAY = THURSDAY.AddDays(1);
                FRIDAY = FRIDAY.AddDays(1);
                SATURDAY = SATURDAY.AddDays(1);
            }
        }

        #endregion

        #region Properties
        #region Binded Properties

        public List<DateTime> PresentationDay
        {
            get
            {
                List<DateTime> _presentationDay = new List<DateTime>();
                _presentationDay.Add(MONDAY);
                _presentationDay.Add(TUESDAY);
                _presentationDay.Add(WEDNESDAY);
                _presentationDay.Add(THURSDAY);
                _presentationDay.Add(FRIDAY);
                return _presentationDay;
            }
        }

        private DateTime _selectedPresentationDay;
        public DateTime SelectedPresentationDay
        {
            get { return _selectedPresentationDay; }
            set
            {
                _selectedPresentationDay = value;
                RaisePropertyChanged(() => this.SelectedPresentationDay);
                if (calculationFlag && SelectedPresentationDateTime != null)
                {
                    SelectedPresentationDateTime = SelectedPresentationDay.Add(
                        Convert.ToDateTime(SelectedPresentationDateTime).TimeOfDay);
                }
            }
        }

        private DateTime? _selectedPresentationDateTime;
        public DateTime? SelectedPresentationDateTime
        {
            get { return _selectedPresentationDateTime; }
            set
            {
                if (value != null)
                {
                    _selectedPresentationDateTime = SelectedPresentationDay.Add(Convert.ToDateTime(value).TimeOfDay);
                    RaisePropertyChanged(() => this.SelectedPresentationDateTime);
                }

                if (calculationFlag)
                {
                    CalculateDeadlines();
                }
            }
        }

        private float _configPresentationDeadline;
        public float ConfigPresentationDeadline
        {
            get { return _configPresentationDeadline; }
            set
            {
                _configPresentationDeadline = value;
            }
        }

        private float _configPreMeetingVotingDeadLine;
        public float ConfigPreMeetingVotingDeadLine
        {
            get { return _configPreMeetingVotingDeadLine; }
            set
            {
                _configPreMeetingVotingDeadLine = value;
            }
        }

        public List<string> PresentationTimeZone
        {
            get
            {
                List<string> _timeZones = new List<string>();
                _timeZones.Add("EST");
                _timeZones.Add("PST");
                _timeZones.Add("UTC");
                return _timeZones;
            }
        }

        private string _selectedTimeZone;
        public string SelectedTimeZone
        {
            get { return _selectedTimeZone; }
            set
            {
                _selectedTimeZone = value;
                RaisePropertyChanged(() => this.SelectedTimeZone);
                if (calculationFlag)
                {
                    CalculateDeadlines();
                }
            }
        }

        private DateTime _presentationDeadline;
        public DateTime PresentationDeadline
        {
            get { return _presentationDeadline; }
            set
            {
                _presentationDeadline = value;
                RaisePropertyChanged(() => this.PresentationDeadline);
            }
        }

        private DateTime _preMeetingVotingDeadline;
        public DateTime PreMeetingVotingDeadline
        {
            get { return _preMeetingVotingDeadline; }
            set
            {
                _preMeetingVotingDeadline = value;
                RaisePropertyChanged(() => this.PreMeetingVotingDeadline);
            }
        }

        #region Busy Indicator
        /// <summary>
        /// Busy Indicator Status
        /// </summary>
        private bool _busyIndicatorIsBusy;
        public bool BusyIndicatorIsBusy
        {
            get { return _busyIndicatorIsBusy; }
            set
            {
                _busyIndicatorIsBusy = value;
                RaisePropertyChanged(() => this.BusyIndicatorIsBusy);
            }
        }

        /// <summary>
        /// Busy Indicator Content
        /// </summary>
        private string _busyIndicatorContent;
        public string BusyIndicatorContent
        {
            get { return _busyIndicatorContent; }
            set
            {
                _busyIndicatorContent = value;
                RaisePropertyChanged(() => this.BusyIndicatorContent);
            }
        }
        #endregion

        #endregion

        #region ICommand Properties

        public ICommand SubmitCommand
        {
            get { return new DelegateCommand<object>(MeetingConfigurationSaveItem); }
        }
        #endregion


        #endregion

        #region ICommand Methods

        private void MeetingConfigurationSaveItem(object param)
        {
            MeetingConfigurationSchedule config = new MeetingConfigurationSchedule();
            config.PresentationDateTime = Convert.ToDateTime(SelectedPresentationDateTime).ToUniversalTime();
            config.PresentationTimeZone = "UTC";//SelectedTimeZone;
            config.PresentationDeadline = PresentationDeadline.ToUniversalTime();
            config.PreMeetingVotingDeadline = PreMeetingVotingDeadline.ToUniversalTime();
            config.CreatedBy = "System";
            config.CreatedOn = DateTime.UtcNow;
            config.ModifiedBy = "System";
            config.ModifiedOn = DateTime.UtcNow;

            // _dbInteractivity.CreateMeetingConfigSchedule(config, (msg) => { MessageBox.Show(msg); });
            if (_dbInteractivity != null)
            {
                BusyIndicatorNotification(true, "Updating Investment Committee Meeting Schedule...");
                _dbInteractivity.UpdateMeetingConfigSchedule(UserSession.SessionManager.SESSION.UserName, config, UpdateMeetingConfigCallback);
            }
        }

        

        #endregion

        #region Helper Methods

        private void SelectionRaisePropertyChanged()
        {
            RaisePropertyChanged(() => this.SubmitCommand);
        }

        private void CalculateDeadlines()
        {
            if (SelectedPresentationDateTime == null)
                return;

            PresentationDeadline = CalcPresentationDeadline((Convert.ToDateTime(SelectedPresentationDateTime)), ConfigPresentationDeadline);

            PreMeetingVotingDeadline = Convert.ToDateTime(SelectedPresentationDateTime).AddHours(0 - ConfigPreMeetingVotingDeadLine);
        }

        public DateTime CalcPresentationDeadline(DateTime dt, float duration)
        {
            //float days = duration / 24;


            TimeSpan tDays = new TimeSpan(Convert.ToInt16(duration), Convert.ToInt16((duration - Convert.ToInt16(duration)) * 60), 0);

            DateTime newTime = dt - tDays;
            DateTime counter = newTime;

            int offs = 0;
            while (counter.Date <= dt.Date)
            {
                if (counter.DayOfWeek.Equals(DayOfWeek.Saturday) || counter.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    offs++;
                }

                counter = counter.AddDays(1);

            }

            newTime = newTime.AddDays(0 - offs);
            if (newTime.DayOfWeek.Equals(DayOfWeek.Saturday))
            {
                newTime = newTime.AddDays(-1);

            }
            else if (newTime.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                newTime = newTime.AddDays(-2);
            }

            return newTime;
        }

        /// <summary>
        /// Busy Indicator Notification
        /// </summary>
        /// <param name="showBusyIndicator"></param>
        /// <param name="message"></param>
        public void BusyIndicatorNotification(bool showBusyIndicator = false, String message = null)
        {
            if (message != null)
                BusyIndicatorContent = message;
            BusyIndicatorIsBusy = showBusyIndicator;
        }


        #endregion

        #region CallBack Methods
        private void GetMeetingConfigScheduleCallbackMethod(MeetingConfigurationSchedule val)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (val != null)
                {
                    SelectedPresentationDay = val.PresentationDateTime.Date;
                    SelectedPresentationDateTime = val.PresentationDateTime;
                    SelectedTimeZone = val.PresentationTimeZone;
                    PresentationDeadline = val.PresentationDeadline;
                    PreMeetingVotingDeadline = val.PreMeetingVotingDeadline;
                    ConfigPresentationDeadline = (float)val.ConfigurablePresentationDeadline;
                    ConfigPreMeetingVotingDeadLine = (float)val.ConfigurablePreMeetingVotingDeadline;
                    calculationFlag = true;
                }
                else
                {
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                }

            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);
            }
            finally
            {
                Logging.LogEndMethod(_logger, methodNamespace);
                BusyIndicatorNotification();
            }
        }

        private void UpdateMeetingConfigCallback(Boolean? result)
        {
            string methodNamespace = String.Format("{0}.{1}", GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Logging.LogBeginMethod(_logger, methodNamespace);
            try
            {
                if (result != null)
                {
                    Logging.LogMethodParameter(_logger, methodNamespace, result, 1);
                    Prompt.ShowDialog("Investment Committee Schedule has been successfully updated");
                }
                else
                {
                    Logging.LogMethodParameterNull(_logger, methodNamespace, 1);
                    Prompt.ShowDialog("An error occurred while updating Investment Committee Schedule");                    
                }
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog("Message: " + ex.Message + "\nStackTrace: " + Logging.StackTraceToString(ex), "Exception", MessageBoxButton.OK);
                Logging.LogException(_logger, ex);                
            }
            finally
            {
                Logging.LogEndMethod(_logger, methodNamespace);
                BusyIndicatorNotification();
            }
        }
        #endregion

        #region EventUnSubscribe
        /// <summary>
        /// Method that disposes the events
        /// </summary>
        public void Dispose()
        {

        }

        #endregion



    }
}
