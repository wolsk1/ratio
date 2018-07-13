namespace VolskSoft.Alfa
{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;   
    using System.Threading;

    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [WebServiceBindingAttribute(Name="ProviderSoap", Namespace="http://tempuri.org/")]
    public partial class Provider : SoapHttpClientProtocol {
        
        private SendOrPostCallback GetSettingsOperationCompleted;
        
        private SendOrPostCallback RefreshOperationCompleted;
        
        private SendOrPostCallback GetSettingsVersionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;        
        
        public Provider() {
            this.Url = "http://localhost/ws/config/provider.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }        
        
        public event GetSettingsCompletedEventHandler GetSettingsCompleted;
        public event RefreshCompletedEventHandler RefreshCompleted;
        public event GetSettingsVersionCompletedEventHandler GetSettingsVersionCompleted;
        
        
        [SoapDocumentMethodAttribute("http://tempuri.org/GetSettings", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public SettingsData[] GetSettings() {
            object[] results = this.Invoke("GetSettings", new object[0]);
            return ((SettingsData[])(results[0]));
        }
        
        
        public void GetSettingsAsync() {
            this.GetSettingsAsync(null);
        }
        
        
        public void GetSettingsAsync(object userState) {
            if ((this.GetSettingsOperationCompleted == null)) {
                this.GetSettingsOperationCompleted = new SendOrPostCallback(this.OnGetSettingsOperationCompleted);
            }
            this.InvokeAsync("GetSettings", new object[0], this.GetSettingsOperationCompleted, userState);
        }
        
        private void OnGetSettingsOperationCompleted(object arg) {
            if ((this.GetSettingsCompleted != null)) {
                InvokeCompletedEventArgs invokeArgs = ((InvokeCompletedEventArgs)(arg));
                this.GetSettingsCompleted(this, new GetSettingsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        
        [SoapDocumentMethodAttribute("http://tempuri.org/Refresh", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public void Refresh() {
            this.Invoke("Refresh", new object[0]);
        }
        
        
        public void RefreshAsync() {
            this.RefreshAsync(null);
        }
        
        
        public void RefreshAsync(object userState) {
            if ((this.RefreshOperationCompleted == null)) {
                this.RefreshOperationCompleted = new SendOrPostCallback(this.OnRefreshOperationCompleted);
            }
            this.InvokeAsync("Refresh", new object[0], this.RefreshOperationCompleted, userState);
        }
        
        private void OnRefreshOperationCompleted(object arg) {
            if ((this.RefreshCompleted != null)) {
                InvokeCompletedEventArgs invokeArgs = ((InvokeCompletedEventArgs)(arg));
                this.RefreshCompleted(this, new AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        
        [SoapDocumentMethodAttribute("http://tempuri.org/GetSettingsVersion", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public long GetSettingsVersion() {
            object[] results = this.Invoke("GetSettingsVersion", new object[0]);
            return ((long)(results[0]));
        }
        
        
        public void GetSettingsVersionAsync() {
            this.GetSettingsVersionAsync(null);
        }
        
        
        public void GetSettingsVersionAsync(object userState) {
            if ((this.GetSettingsVersionOperationCompleted == null)) {
                this.GetSettingsVersionOperationCompleted = new SendOrPostCallback(this.OnGetSettingsVersionOperationCompleted);
            }
            this.InvokeAsync("GetSettingsVersion", new object[0], this.GetSettingsVersionOperationCompleted, userState);
        }
        
        private void OnGetSettingsVersionOperationCompleted(object arg) {
            if ((this.GetSettingsVersionCompleted != null)) {
                InvokeCompletedEventArgs invokeArgs = ((InvokeCompletedEventArgs)(arg));
                this.GetSettingsVersionCompleted(this, new GetSettingsVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }   
    
   
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SettingsData {
        
        private string keyField;
        
        private string valueField;
        
        
        public string Key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
        
        
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }   

    public delegate void GetSettingsCompletedEventHandler(object sender, GetSettingsCompletedEventArgs e);
    
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    public partial class GetSettingsCompletedEventArgs : AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSettingsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        
        public SettingsData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((SettingsData[])(this.results[0]));
            }
        }
    }
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void RefreshCompletedEventHandler(object sender, AsyncCompletedEventArgs e);
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetSettingsVersionCompletedEventHandler(object sender, GetSettingsVersionCompletedEventArgs e);
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    public partial class GetSettingsVersionCompletedEventArgs : AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSettingsVersionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591