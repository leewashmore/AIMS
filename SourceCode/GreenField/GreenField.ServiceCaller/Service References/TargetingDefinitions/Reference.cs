﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.60310.0
// 
namespace GreenField.ServiceCaller.TargetingDefinitions {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RootModel", Namespace="http://schemas.datacontract.org/2004/07/TopDown.FacingClient.ManagingBga")]
    public partial class RootModel : object, System.ComponentModel.INotifyPropertyChanged {
        
        private GreenField.ServiceCaller.TargetingDefinitions.BroadGlobalActivePortfolioModel BroadGlobalActiveProtfolioField;
        
        private GreenField.ServiceCaller.TargetingDefinitions.FactorModel FactorsField;
        
        private GreenField.ServiceCaller.TargetingDefinitions.TargetingTypeModel TargetingTypeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public GreenField.ServiceCaller.TargetingDefinitions.BroadGlobalActivePortfolioModel BroadGlobalActiveProtfolio {
            get {
                return this.BroadGlobalActiveProtfolioField;
            }
            set {
                if ((object.ReferenceEquals(this.BroadGlobalActiveProtfolioField, value) != true)) {
                    this.BroadGlobalActiveProtfolioField = value;
                    this.RaisePropertyChanged("BroadGlobalActiveProtfolio");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public GreenField.ServiceCaller.TargetingDefinitions.FactorModel Factors {
            get {
                return this.FactorsField;
            }
            set {
                if ((object.ReferenceEquals(this.FactorsField, value) != true)) {
                    this.FactorsField = value;
                    this.RaisePropertyChanged("Factors");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public GreenField.ServiceCaller.TargetingDefinitions.TargetingTypeModel TargetingType {
            get {
                return this.TargetingTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.TargetingTypeField, value) != true)) {
                    this.TargetingTypeField = value;
                    this.RaisePropertyChanged("TargetingType");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BroadGlobalActivePortfolioModel", Namespace="http://schemas.datacontract.org/2004/07/TopDown.FacingClient")]
    public partial class BroadGlobalActivePortfolioModel : GreenField.ServiceCaller.TargetingDefinitions.PortfolioModel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FactorModel", Namespace="http://schemas.datacontract.org/2004/07/TopDown.FacingClient.ManagingBga")]
    public partial class FactorModel : object, System.ComponentModel.INotifyPropertyChanged {
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TargetingTypeModel", Namespace="http://schemas.datacontract.org/2004/07/TopDown.FacingClient")]
    public partial class TargetingTypeModel : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int IdField;
        
        private string NameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PortfolioModel", Namespace="http://schemas.datacontract.org/2004/07/TopDown.FacingClient")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(GreenField.ServiceCaller.TargetingDefinitions.BroadGlobalActivePortfolioModel))]
    public partial class PortfolioModel : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string IdField;
        
        private string NameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TargetingDefinitions.IFacade")]
    public interface IFacade {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IFacade/GetBgaModel", ReplyAction="http://tempuri.org/IFacade/GetBgaModelResponse")]
        System.IAsyncResult BeginGetBgaModel(int targetingTypeId, string bgaPortfolioId, System.DateTime benchmarkDate, System.AsyncCallback callback, object asyncState);
        
        GreenField.ServiceCaller.TargetingDefinitions.RootModel EndGetBgaModel(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFacadeChannel : GreenField.ServiceCaller.TargetingDefinitions.IFacade, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetBgaModelCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetBgaModelCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public GreenField.ServiceCaller.TargetingDefinitions.RootModel Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((GreenField.ServiceCaller.TargetingDefinitions.RootModel)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FacadeClient : System.ServiceModel.ClientBase<GreenField.ServiceCaller.TargetingDefinitions.IFacade>, GreenField.ServiceCaller.TargetingDefinitions.IFacade {
        
        private BeginOperationDelegate onBeginGetBgaModelDelegate;
        
        private EndOperationDelegate onEndGetBgaModelDelegate;
        
        private System.Threading.SendOrPostCallback onGetBgaModelCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public FacadeClient() {
        }
        
        public FacadeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FacadeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FacadeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FacadeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetBgaModelCompletedEventArgs> GetBgaModelCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult GreenField.ServiceCaller.TargetingDefinitions.IFacade.BeginGetBgaModel(int targetingTypeId, string bgaPortfolioId, System.DateTime benchmarkDate, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetBgaModel(targetingTypeId, bgaPortfolioId, benchmarkDate, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GreenField.ServiceCaller.TargetingDefinitions.RootModel GreenField.ServiceCaller.TargetingDefinitions.IFacade.EndGetBgaModel(System.IAsyncResult result) {
            return base.Channel.EndGetBgaModel(result);
        }
        
        private System.IAsyncResult OnBeginGetBgaModel(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int targetingTypeId = ((int)(inValues[0]));
            string bgaPortfolioId = ((string)(inValues[1]));
            System.DateTime benchmarkDate = ((System.DateTime)(inValues[2]));
            return ((GreenField.ServiceCaller.TargetingDefinitions.IFacade)(this)).BeginGetBgaModel(targetingTypeId, bgaPortfolioId, benchmarkDate, callback, asyncState);
        }
        
        private object[] OnEndGetBgaModel(System.IAsyncResult result) {
            GreenField.ServiceCaller.TargetingDefinitions.RootModel retVal = ((GreenField.ServiceCaller.TargetingDefinitions.IFacade)(this)).EndGetBgaModel(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetBgaModelCompleted(object state) {
            if ((this.GetBgaModelCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetBgaModelCompleted(this, new GetBgaModelCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetBgaModelAsync(int targetingTypeId, string bgaPortfolioId, System.DateTime benchmarkDate) {
            this.GetBgaModelAsync(targetingTypeId, bgaPortfolioId, benchmarkDate, null);
        }
        
        public void GetBgaModelAsync(int targetingTypeId, string bgaPortfolioId, System.DateTime benchmarkDate, object userState) {
            if ((this.onBeginGetBgaModelDelegate == null)) {
                this.onBeginGetBgaModelDelegate = new BeginOperationDelegate(this.OnBeginGetBgaModel);
            }
            if ((this.onEndGetBgaModelDelegate == null)) {
                this.onEndGetBgaModelDelegate = new EndOperationDelegate(this.OnEndGetBgaModel);
            }
            if ((this.onGetBgaModelCompletedDelegate == null)) {
                this.onGetBgaModelCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetBgaModelCompleted);
            }
            base.InvokeAsync(this.onBeginGetBgaModelDelegate, new object[] {
                        targetingTypeId,
                        bgaPortfolioId,
                        benchmarkDate}, this.onEndGetBgaModelDelegate, this.onGetBgaModelCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override GreenField.ServiceCaller.TargetingDefinitions.IFacade CreateChannel() {
            return new FacadeClientChannel(this);
        }
        
        private class FacadeClientChannel : ChannelBase<GreenField.ServiceCaller.TargetingDefinitions.IFacade>, GreenField.ServiceCaller.TargetingDefinitions.IFacade {
            
            public FacadeClientChannel(System.ServiceModel.ClientBase<GreenField.ServiceCaller.TargetingDefinitions.IFacade> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetBgaModel(int targetingTypeId, string bgaPortfolioId, System.DateTime benchmarkDate, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[3];
                _args[0] = targetingTypeId;
                _args[1] = bgaPortfolioId;
                _args[2] = benchmarkDate;
                System.IAsyncResult _result = base.BeginInvoke("GetBgaModel", _args, callback, asyncState);
                return _result;
            }
            
            public GreenField.ServiceCaller.TargetingDefinitions.RootModel EndGetBgaModel(System.IAsyncResult result) {
                object[] _args = new object[0];
                GreenField.ServiceCaller.TargetingDefinitions.RootModel _result = ((GreenField.ServiceCaller.TargetingDefinitions.RootModel)(base.EndInvoke("GetBgaModel", _args, result)));
                return _result;
            }
        }
    }
}
