namespace WealthLab.StrategyProviders.WLWebServices
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Web.Services;
    using System.Web.Services.Description;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using WealthLab.StrategyProviders.Properties;

    [DesignerCategory("code"), WebServiceBinding(Name="StrategyWebserviceSoap", Namespace="http://www.wealth-lab.com/WebServices/"), DebuggerStepThrough, GeneratedCode("System.Web.Services", "4.0.30319.1")]
    public class StrategyWebservice : SoapHttpClientProtocol
    {
        private SendOrPostCallback GetPrivateStrategiesOperationCompleted;
        private SendOrPostCallback GetPublicStrategiesOperationCompleted;
        private bool useDefaultCredentialsSetExplicitly;

        public event GetPrivateStrategiesCompletedEventHandler GetPrivateStrategiesCompleted;

        public event GetPublicStrategiesCompletedEventHandler GetPublicStrategiesCompleted;

        public StrategyWebservice()
        {
            this.Url = Settings.Default.WealthLab_StrategyProviders_WLWebServices_StrategyWebservice;
            if (this.IsLocalFileSystemWebService(this.Url))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        [SoapDocumentMethod("http://www.wealth-lab.com/WebServices/GetPrivateStrategies", RequestNamespace="http://www.wealth-lab.com/WebServices/", ResponseNamespace="http://www.wealth-lab.com/WebServices/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string[] GetPrivateStrategies(int numberOfDaysBack, string username, [XmlElement(DataType="base64Binary")] byte[] password)
        {
            return (string[]) base.Invoke("GetPrivateStrategies", new object[] { numberOfDaysBack, username, password })[0];
        }

        public void GetPrivateStrategiesAsync(int numberOfDaysBack, string username, byte[] password)
        {
            this.GetPrivateStrategiesAsync(numberOfDaysBack, username, password, null);
        }

        public void GetPrivateStrategiesAsync(int numberOfDaysBack, string username, byte[] password, object userState)
        {
            if (this.GetPrivateStrategiesOperationCompleted == null)
            {
                this.GetPrivateStrategiesOperationCompleted = new SendOrPostCallback(this.OnGetPrivateStrategiesOperationCompleted);
            }
            base.InvokeAsync("GetPrivateStrategies", new object[] { numberOfDaysBack, username, password }, this.GetPrivateStrategiesOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://www.wealth-lab.com/WebServices/GetPublicStrategies", RequestNamespace="http://www.wealth-lab.com/WebServices/", ResponseNamespace="http://www.wealth-lab.com/WebServices/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string[] GetPublicStrategies(int numberOfDaysBack)
        {
            return (string[]) base.Invoke("GetPublicStrategies", new object[] { numberOfDaysBack })[0];
        }

        public void GetPublicStrategiesAsync(int numberOfDaysBack)
        {
            this.GetPublicStrategiesAsync(numberOfDaysBack, null);
        }

        public void GetPublicStrategiesAsync(int numberOfDaysBack, object userState)
        {
            if (this.GetPublicStrategiesOperationCompleted == null)
            {
                this.GetPublicStrategiesOperationCompleted = new SendOrPostCallback(this.OnGetPublicStrategiesOperationCompleted);
            }
            base.InvokeAsync("GetPublicStrategies", new object[] { numberOfDaysBack }, this.GetPublicStrategiesOperationCompleted, userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if ((url == null) || (url == string.Empty))
            {
                return false;
            }
            Uri uri = new Uri(url);
            return ((uri.Port >= 0x400) && (string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0));
        }

        private void OnGetPrivateStrategiesOperationCompleted(object arg)
        {
            if (this.GetPrivateStrategiesCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.GetPrivateStrategiesCompleted(this, new GetPrivateStrategiesCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnGetPublicStrategiesOperationCompleted(object arg)
        {
            if (this.GetPublicStrategiesCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.GetPublicStrategiesCompleted(this, new GetPublicStrategiesCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        public string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly) && !this.IsLocalFileSystemWebService(value))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
    }
}

