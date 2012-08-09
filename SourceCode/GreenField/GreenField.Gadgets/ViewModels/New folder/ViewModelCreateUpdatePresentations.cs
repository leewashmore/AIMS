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
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;
using Ashmore.Emm.GreenField.BusinessLogic;
using Ashmore.Emm.GreenField.ICP.Meeting.Module.Model;
using Ashmore.Emm.GreenField.Common;
using Ashmore.Emm.GreenField.BusinessLogic.MeetingServiceReference;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;

namespace Ashmore.Emm.GreenField.ICP.Meeting.Module.ViewModels
{
    [Export]
    public class ViewModelCreateUpdatePresentations : NotificationObject, INavigationAware
    {
        [Import]
        public IRegionManager _regionManager { private get; set; }

        ManageMeetings _manageMeetings;

        [ImportingConstructor]
        public ViewModelCreateUpdatePresentations(ManageMeetings manageMeetings)
        {
            _manageMeetings = manageMeetings;
        }

        #region Properties

        private ObservableCollection<ICPAttachmentInfo> _attachmentInfo;
        public ObservableCollection<ICPAttachmentInfo> AttachmentInfo
        {
            get 
            {
                if (_attachmentInfo == null)
                    _attachmentInfo = new ObservableCollection<ICPAttachmentInfo>();
                return _attachmentInfo; 
            }
            set
            {
                if (_attachmentInfo != value)
                {
                    _attachmentInfo = value;
                    RaisePropertyChanged(() => this.AttachmentInfo);
                }
            }
        }

        private bool _securitySelected = false;
        public bool SecuritySelected
        {
            get 
            {
                if (ViewPluginFlag == ViewPluginFlagEnumeration.Update)
                    _securitySelected = true;
                return _securitySelected; 
            }
            set
            {
                if (_securitySelected != value)
                {
                    _securitySelected = value;
                    RaisePropertyChanged(() => this.SecuritySelected);
                    RaisePropertyChanged(() => this.SaveCommand);
                    RaisePropertyChanged(() => this.BrowseCommand);
                }
            }
        }

        private string _uploadedFileName;
        public string UploadedFileName
        {
            get { return _uploadedFileName; }
            set
            {
                if (_uploadedFileName != value)
                {
                    _uploadedFileName = value;
                    RaisePropertyChanged(() => this.UploadedFileName);
                }
            }
        }

        private ViewPluginFlagEnumeration _viewPluginFlag;
        public ViewPluginFlagEnumeration ViewPluginFlag
        {
            get
            {
                return _viewPluginFlag;
            }
            set
            {
                _viewPluginFlag = value;
                RaisePropertyChanged(() => this.ViewPluginFlag);
                RaisePropertyChanged(() => this.SecuritySelectionVisibility);
                RaisePropertyChanged(() => this.PresenterSelectionVisibility);
            }
        }

        private ObservableCollection<ICPSecurityInfo> _securityInfo;
        public ObservableCollection<ICPSecurityInfo> SecurityInfo
        {
            get
            {
                if (ViewPluginFlag == ViewPluginFlagEnumeration.Create)
                {
                    _securityInfo = new ObservableCollection<ICPSecurityInfo>();
                    ICPSecurityInfo s1 = new ICPSecurityInfo
                    {
                        SecurityBuyRange = "20",
                        SecurityCashPosition = "0",
                        SecurityCountry = "India",
                        SecurityCountryCode = "IND",
                        SecurityGlobalActiveWeight = "0",
                        SecurityIndustry = "Chemical",
                        SecurityLastClosingPrice = "25.6",
                        SecurityMarketCapitalization = "6459800",
                        SecurityMSCIIMIWeight = "0",
                        SecurityMSCIStdWeight = "0",
                        SecurityName = "Reliance Capital Ltd.",
                        SecurityPFVMeasure = "PE2012",
                        SecurityPosition = "0",
                        SecuritySellRange = "30",
                        SecurityTicker = "REL"                        
                    };
                    ICPSecurityInfo s2 = new ICPSecurityInfo
                    {
                        SecurityBuyRange = "28",
                        SecurityCashPosition = "0",
                        SecurityCountry = "United States",
                        SecurityCountryCode = "US",
                        SecurityGlobalActiveWeight = "0",
                        SecurityIndustry = "Internet",
                        SecurityLastClosingPrice = "25.6",
                        SecurityMarketCapitalization = "56,894,000",
                        SecurityMSCIIMIWeight = "0",
                        SecurityMSCIStdWeight = "0",
                        SecurityName = "Facebook",
                        SecurityPFVMeasure = "PE2012",
                        SecurityPosition = "0",
                        SecuritySellRange = "33",
                        SecurityTicker = "FCB"
                    };
                    _securityInfo.Add(s1);
                    _securityInfo.Add(s2);
                }
                    
                return _securityInfo;
            }
            set
            {
                _securityInfo = value;
                RaisePropertyChanged(() => this.PresentationInfo);
            }
        }

        private ICPSecurityInfo _selectedSecurityInfo;
        public ICPSecurityInfo SelectedSecurityInfo
        {
            get { return _selectedSecurityInfo; }
            set
            {
                _selectedSecurityInfo = value;
                RaisePropertyChanged(() => this.SelectedSecurityInfo);
                SecuritySelected = true;
                PresentationInfo = new ICPPresentationInfo
                {
                    Presenter = "Rahul Vig",
                    SecurityBuyRange = value.SecurityBuyRange,
                    SecurityBuySellRange = value.SecurityBuySellRange,
                    SecurityCashPosition = value.SecurityCashPosition,
                    SecurityCountry = value.SecurityCountry,
                    SecurityCountryCode = value.SecurityCountryCode,
                    SecurityGlobalActiveWeight = value.SecurityGlobalActiveWeight,
                    SecurityIndustry = value.SecurityIndustry,
                    SecurityLastClosingPrice = value.SecurityLastClosingPrice,
                    SecurityMarketCapitalization = value.SecurityMarketCapitalization,
                    SecurityMSCIIMIWeight = value.SecurityMSCIIMIWeight,
                    SecurityMSCIStdWeight = value.SecurityMSCIStdWeight,
                    SecurityName = value.SecurityName,
                    SecurityPFVMeasure =value.SecurityPFVMeasure,
                    SecurityPosition = value.SecurityPosition,
                    SecurityRecommendation  =value.SecurityRecommendation,
                    SecuritySellRange = value.SecuritySellRange,
                    SecurityTicker = value.SecurityTicker,
                    Status = "In Progress"                    
                };
            }
        }
        
        private ICPPresentationInfo _presentationInfo;
        public ICPPresentationInfo PresentationInfo
        {
            get
            {
                if (_presentationInfo == null)
                    return new ICPPresentationInfo();
                return _presentationInfo;
            }
            set
            {
                _presentationInfo = value;
                RaisePropertyChanged(() => this.PresentationInfo);
            }
        }

        private Visibility _securitySelectionVisibility = Visibility.Visible;
        public Visibility SecuritySelectionVisibility
        {
            get 
            {
                switch (ViewPluginFlag)
                {
                    case ViewPluginFlagEnumeration.Create:
                        _securitySelectionVisibility = Visibility.Visible;
                        break;
                    case ViewPluginFlagEnumeration.Update:
                        _securitySelectionVisibility = Visibility.Collapsed;
                        break;
                }
                return _securitySelectionVisibility; }
        }

        private Visibility _presenterSelectionVisibility = Visibility.Collapsed;
        public Visibility PresenterSelectionVisibility
        {
            get
            {
                switch (ViewPluginFlag)
                {
                    case ViewPluginFlagEnumeration.Create:
                        _presenterSelectionVisibility = Visibility.Collapsed;
                        break;
                    case ViewPluginFlagEnumeration.Update:
                        _presenterSelectionVisibility = Visibility.Visible;
                        break;
                }
                return _presenterSelectionVisibility;
            }
        }

        public ICommand SaveCommand
        {
            get { return new DelegateCommand<object>(ICPPresentationsSaveItem, SecuritySelectionValidation);  }
        }

        public ICommand BrowseCommand
        {
            get
            {
                return new DelegateCommand<object>(ICPPresentationsBrowse, SecuritySelectionValidation);
            }
        }

        #endregion

        #region ICommand Methods

        private void ICPPresentationsSaveItem(object param)
        {
            ObservableCollection<AttachedFileInfo> UpdatedAttachmentInfo = new ObservableCollection<AttachedFileInfo>();
                            
            switch (ViewPluginFlag)
            {
                case ViewPluginFlagEnumeration.Create:
                    _manageMeetings.CreatePresentation(PresentationInfo.ConvertToDB()
                        , (presentationID) =>
                        {
                            if (presentationID != null)
                            {
                                this.PresentationInfo.PresentationID = long.Parse(presentationID.ToString());

                                foreach (ICPAttachmentInfo attachment in AttachmentInfo.Where(t => t.AttachmentVisibility == Visibility.Visible))
                                {
                                    attachment.PresentationID = this.PresentationInfo.PresentationID;
                                    UpdatedAttachmentInfo.Add(attachment.ConvertToDB());
                                }

                                if (UpdatedAttachmentInfo != null)
                                    _manageMeetings.CreateFileInfo(UpdatedAttachmentInfo, (fmsg) => { MessageBox.Show(fmsg); });
                            }
                        });
                    break;
                case ViewPluginFlagEnumeration.Update:
                    break;
            }

            
            
            _regionManager.RequestNavigate(RegionNames.MainRegion, new Uri("ViewPresentations", UriKind.Relative));
        }

        public void ICPPresentationsBrowse(object param)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*"
            };
            
            if (openFile.ShowDialog() == true)
            {
                UploadedFileName = openFile.File.Name;
                string serializedData = GetFileSerializedData(openFile.File.OpenRead());

                AttachmentInfo.Add(new ICPAttachmentInfo ()
                {
                    AttachmentID = AttachmentInfo.Count,
                    AttachmentName = UploadedFileName,
                    AttachmentSerializedData = serializedData
                });
            }
        }

        private bool SecuritySelectionValidation(object param)
        {
            return SecuritySelected;
        }

        #endregion

        #region Helper Methods

        private string GetFileSerializedData(FileStream fileStream)
        {
            byte[] bytes = new byte[fileStream.Length];

            int numBytesToRead = (int)fileStream.Length;
            int numBytesRead = 0;

            while (numBytesToRead > 0)
            {
                int n = fileStream.Read(bytes, numBytesRead, numBytesToRead);
                if (n == 0) break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
            numBytesToRead = bytes.Length;

            StringBuilder sb = new StringBuilder();
            StringWriter wr = new StringWriter(sb);

            XmlSerializer serializer = new XmlSerializer(typeof(byte[]));
            
            serializer.Serialize(wr, bytes);
            return sb.ToString();
        }

        #endregion

        #region INavigationAware methods

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ViewPluginFlag = (navigationContext.NavigationService.Region.Context as ICPNavigationInfo).ViewPluginFlagEnumerationObject;
            PresentationInfo = (navigationContext.NavigationService.Region.Context as ICPNavigationInfo).PresentationInfoObject;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        #endregion

        

    }
}
