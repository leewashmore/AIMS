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
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.50826.0
// 
namespace GreenField.ServiceCaller.ExternalResearchDefinitions {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FinancialStatementData", Namespace="http://schemas.datacontract.org/2004/07/GreenField.DAL", IsReference=true)]
    public partial class FinancialStatementData : GreenField.ServiceCaller.ExternalResearchDefinitions.ComplexObject {
        
        private System.Nullable<decimal> AMOUNTField;
        
        private string AMOUNT_TYPEField;
        
        private string BOLD_FONTField;
        
        private string CALCULATION_DIAGRAMField;
        
        private string DATA_DESCField;
        
        private int DATA_IDField;
        
        private int DECIMALSField;
        
        private string GROUP_NAMEField;
        
        private string PERIODField;
        
        private string PERIOD_TYPEField;
        
        private string ROOT_SOURCEField;
        
        private System.DateTime ROOT_SOURCE_DATEField;
        
        private int SORT_ORDERField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> AMOUNT {
            get {
                return this.AMOUNTField;
            }
            set {
                if ((this.AMOUNTField.Equals(value) != true)) {
                    this.AMOUNTField = value;
                    this.RaisePropertyChanged("AMOUNT");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AMOUNT_TYPE {
            get {
                return this.AMOUNT_TYPEField;
            }
            set {
                if ((object.ReferenceEquals(this.AMOUNT_TYPEField, value) != true)) {
                    this.AMOUNT_TYPEField = value;
                    this.RaisePropertyChanged("AMOUNT_TYPE");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BOLD_FONT {
            get {
                return this.BOLD_FONTField;
            }
            set {
                if ((object.ReferenceEquals(this.BOLD_FONTField, value) != true)) {
                    this.BOLD_FONTField = value;
                    this.RaisePropertyChanged("BOLD_FONT");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CALCULATION_DIAGRAM {
            get {
                return this.CALCULATION_DIAGRAMField;
            }
            set {
                if ((object.ReferenceEquals(this.CALCULATION_DIAGRAMField, value) != true)) {
                    this.CALCULATION_DIAGRAMField = value;
                    this.RaisePropertyChanged("CALCULATION_DIAGRAM");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DATA_DESC {
            get {
                return this.DATA_DESCField;
            }
            set {
                if ((object.ReferenceEquals(this.DATA_DESCField, value) != true)) {
                    this.DATA_DESCField = value;
                    this.RaisePropertyChanged("DATA_DESC");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int DATA_ID {
            get {
                return this.DATA_IDField;
            }
            set {
                if ((this.DATA_IDField.Equals(value) != true)) {
                    this.DATA_IDField = value;
                    this.RaisePropertyChanged("DATA_ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int DECIMALS {
            get {
                return this.DECIMALSField;
            }
            set {
                if ((this.DECIMALSField.Equals(value) != true)) {
                    this.DECIMALSField = value;
                    this.RaisePropertyChanged("DECIMALS");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GROUP_NAME {
            get {
                return this.GROUP_NAMEField;
            }
            set {
                if ((object.ReferenceEquals(this.GROUP_NAMEField, value) != true)) {
                    this.GROUP_NAMEField = value;
                    this.RaisePropertyChanged("GROUP_NAME");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PERIOD {
            get {
                return this.PERIODField;
            }
            set {
                if ((object.ReferenceEquals(this.PERIODField, value) != true)) {
                    this.PERIODField = value;
                    this.RaisePropertyChanged("PERIOD");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PERIOD_TYPE {
            get {
                return this.PERIOD_TYPEField;
            }
            set {
                if ((object.ReferenceEquals(this.PERIOD_TYPEField, value) != true)) {
                    this.PERIOD_TYPEField = value;
                    this.RaisePropertyChanged("PERIOD_TYPE");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ROOT_SOURCE {
            get {
                return this.ROOT_SOURCEField;
            }
            set {
                if ((object.ReferenceEquals(this.ROOT_SOURCEField, value) != true)) {
                    this.ROOT_SOURCEField = value;
                    this.RaisePropertyChanged("ROOT_SOURCE");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ROOT_SOURCE_DATE {
            get {
                return this.ROOT_SOURCE_DATEField;
            }
            set {
                if ((this.ROOT_SOURCE_DATEField.Equals(value) != true)) {
                    this.ROOT_SOURCE_DATEField = value;
                    this.RaisePropertyChanged("ROOT_SOURCE_DATE");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SORT_ORDER {
            get {
                return this.SORT_ORDERField;
            }
            set {
                if ((this.SORT_ORDERField.Equals(value) != true)) {
                    this.SORT_ORDERField = value;
                    this.RaisePropertyChanged("SORT_ORDER");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StructuralObject", Namespace="http://schemas.datacontract.org/2004/07/System.Data.Objects.DataClasses", IsReference=true)]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(GreenField.ServiceCaller.ExternalResearchDefinitions.ComplexObject))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData))]
    public partial class StructuralObject : object, System.ComponentModel.INotifyPropertyChanged {
        
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ComplexObject", Namespace="http://schemas.datacontract.org/2004/07/System.Data.Objects.DataClasses", IsReference=true)]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData))]
    public partial class ComplexObject : GreenField.ServiceCaller.ExternalResearchDefinitions.StructuralObject {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceFault", Namespace="http://schemas.datacontract.org/2004/07/GreenField.Web.Helpers.Service_Faults")]
    public partial class ServiceFault : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string DescriptionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ExternalResearchDefinitions.ExternalResearchOperations")]
    public interface ExternalResearchOperations {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ExternalResearchOperations/GetFinancialStatement", ReplyAction="http://tempuri.org/ExternalResearchOperations/GetFinancialStatementResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(GreenField.ServiceCaller.ExternalResearchDefinitions.ServiceFault), Action="http://tempuri.org/ExternalResearchOperations/GetFinancialStatementServiceFaultFa" +
            "ult", Name="ServiceFault", Namespace="http://schemas.datacontract.org/2004/07/GreenField.Web.Helpers.Service_Faults")]
        System.IAsyncResult BeginGetFinancialStatement(string issuerID, GreenField.DataContracts.FinancialStatementDataSource dataSource, GreenField.DataContracts.FinancialStatementPeriodType periodType, GreenField.DataContracts.FinancialStatementFiscalType fiscalType, GreenField.DataContracts.FinancialStatementStatementType statementType, string currency, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData> EndGetFinancialStatement(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ExternalResearchOperationsChannel : GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetFinancialStatementCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetFinancialStatementCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ExternalResearchOperationsClient : System.ServiceModel.ClientBase<GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations>, GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations {
        
        private BeginOperationDelegate onBeginGetFinancialStatementDelegate;
        
        private EndOperationDelegate onEndGetFinancialStatementDelegate;
        
        private System.Threading.SendOrPostCallback onGetFinancialStatementCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ExternalResearchOperationsClient() {
        }
        
        public ExternalResearchOperationsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ExternalResearchOperationsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExternalResearchOperationsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExternalResearchOperationsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<GetFinancialStatementCompletedEventArgs> GetFinancialStatementCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations.BeginGetFinancialStatement(string issuerID, GreenField.DataContracts.FinancialStatementDataSource dataSource, GreenField.DataContracts.FinancialStatementPeriodType periodType, GreenField.DataContracts.FinancialStatementFiscalType fiscalType, GreenField.DataContracts.FinancialStatementStatementType statementType, string currency, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetFinancialStatement(issuerID, dataSource, periodType, fiscalType, statementType, currency, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData> GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations.EndGetFinancialStatement(System.IAsyncResult result) {
            return base.Channel.EndGetFinancialStatement(result);
        }
        
        private System.IAsyncResult OnBeginGetFinancialStatement(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string issuerID = ((string)(inValues[0]));
            GreenField.DataContracts.FinancialStatementDataSource dataSource = ((GreenField.DataContracts.FinancialStatementDataSource)(inValues[1]));
            GreenField.DataContracts.FinancialStatementPeriodType periodType = ((GreenField.DataContracts.FinancialStatementPeriodType)(inValues[2]));
            GreenField.DataContracts.FinancialStatementFiscalType fiscalType = ((GreenField.DataContracts.FinancialStatementFiscalType)(inValues[3]));
            GreenField.DataContracts.FinancialStatementStatementType statementType = ((GreenField.DataContracts.FinancialStatementStatementType)(inValues[4]));
            string currency = ((string)(inValues[5]));
            return ((GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations)(this)).BeginGetFinancialStatement(issuerID, dataSource, periodType, fiscalType, statementType, currency, callback, asyncState);
        }
        
        private object[] OnEndGetFinancialStatement(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData> retVal = ((GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations)(this)).EndGetFinancialStatement(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetFinancialStatementCompleted(object state) {
            if ((this.GetFinancialStatementCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetFinancialStatementCompleted(this, new GetFinancialStatementCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetFinancialStatementAsync(string issuerID, GreenField.DataContracts.FinancialStatementDataSource dataSource, GreenField.DataContracts.FinancialStatementPeriodType periodType, GreenField.DataContracts.FinancialStatementFiscalType fiscalType, GreenField.DataContracts.FinancialStatementStatementType statementType, string currency) {
            this.GetFinancialStatementAsync(issuerID, dataSource, periodType, fiscalType, statementType, currency, null);
        }
        
        public void GetFinancialStatementAsync(string issuerID, GreenField.DataContracts.FinancialStatementDataSource dataSource, GreenField.DataContracts.FinancialStatementPeriodType periodType, GreenField.DataContracts.FinancialStatementFiscalType fiscalType, GreenField.DataContracts.FinancialStatementStatementType statementType, string currency, object userState) {
            if ((this.onBeginGetFinancialStatementDelegate == null)) {
                this.onBeginGetFinancialStatementDelegate = new BeginOperationDelegate(this.OnBeginGetFinancialStatement);
            }
            if ((this.onEndGetFinancialStatementDelegate == null)) {
                this.onEndGetFinancialStatementDelegate = new EndOperationDelegate(this.OnEndGetFinancialStatement);
            }
            if ((this.onGetFinancialStatementCompletedDelegate == null)) {
                this.onGetFinancialStatementCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetFinancialStatementCompleted);
            }
            base.InvokeAsync(this.onBeginGetFinancialStatementDelegate, new object[] {
                        issuerID,
                        dataSource,
                        periodType,
                        fiscalType,
                        statementType,
                        currency}, this.onEndGetFinancialStatementDelegate, this.onGetFinancialStatementCompletedDelegate, userState);
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
        
        protected override GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations CreateChannel() {
            return new ExternalResearchOperationsClientChannel(this);
        }
        
        private class ExternalResearchOperationsClientChannel : ChannelBase<GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations>, GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations {
            
            public ExternalResearchOperationsClientChannel(System.ServiceModel.ClientBase<GreenField.ServiceCaller.ExternalResearchDefinitions.ExternalResearchOperations> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetFinancialStatement(string issuerID, GreenField.DataContracts.FinancialStatementDataSource dataSource, GreenField.DataContracts.FinancialStatementPeriodType periodType, GreenField.DataContracts.FinancialStatementFiscalType fiscalType, GreenField.DataContracts.FinancialStatementStatementType statementType, string currency, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[6];
                _args[0] = issuerID;
                _args[1] = dataSource;
                _args[2] = periodType;
                _args[3] = fiscalType;
                _args[4] = statementType;
                _args[5] = currency;
                System.IAsyncResult _result = base.BeginInvoke("GetFinancialStatement", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData> EndGetFinancialStatement(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData> _result = ((System.Collections.ObjectModel.ObservableCollection<GreenField.ServiceCaller.ExternalResearchDefinitions.FinancialStatementData>)(base.EndInvoke("GetFinancialStatement", _args, result)));
                return _result;
            }
        }
    }
}
