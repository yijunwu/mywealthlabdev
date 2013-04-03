namespace WealthLab.International.CustomersWebService
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
    using WealthLab.International.Properties;

    [GeneratedCode("System.Web.Services", "4.0.30319.1"), WebServiceBinding(Name="CustomersWebServiceSoap", Namespace="http://www.wealth-lab.com/"), DesignerCategory("code"), DebuggerStepThrough]
    public class CustomersWebService : SoapHttpClientProtocol
    {
        private bool bool_0;
        private SendOrPostCallback sendOrPostCallback_0;
        private SendOrPostCallback sendOrPostCallback_1;
        private SendOrPostCallback sendOrPostCallback_2;
        private SendOrPostCallback sendOrPostCallback_3;
        private SendOrPostCallback sendOrPostCallback_4;
        private ServiceHeader serviceHeader_0;

        public event ActivateKeyCompletedEventHandler ActivateKeyCompleted;

        public event ActivateTrialCompletedEventHandler ActivateTrialCompleted;

        public event AuthEducationalCompletedEventHandler AuthEducationalCompleted;

        public event AuthTrialCompletedEventHandler AuthTrialCompleted;

        public event TCompletedEventHandler TCompleted;

        public CustomersWebService()
        {
            this.Url = Settings.Default.WealthLab_International_localhost_CustomersWebService;
            if (this.method_5(this.Url))
            {
                this.UseDefaultCredentials = true;
                this.bool_0 = false;
            }
            else
            {
                this.bool_0 = true;
            }
        }

        [return: XmlElement("error")]
        [SoapHeader("ServiceHeaderValue"), SoapDocumentMethod("http://www.wealth-lab.com/ActivateKey", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string ActivateKey(ref string paramStr)
        {
            object[] objArray = base.Invoke("ActivateKey", new object[] { paramStr });
            paramStr = (string) objArray[1];
            return (string) objArray[0];
        }

        public void ActivateKeyAsync(string paramStr)
        {
            this.ActivateKeyAsync(paramStr, null);
        }

        public void ActivateKeyAsync(string paramStr, object userState)
        {
            if (this.sendOrPostCallback_0 == null)
            {
                this.sendOrPostCallback_0 = new SendOrPostCallback(this.method_0);
            }
            base.InvokeAsync("ActivateKey", new object[] { paramStr }, this.sendOrPostCallback_0, userState);
        }

        [return: XmlElement("error")]
        [SoapDocumentMethod("http://www.wealth-lab.com/ActivateTrial", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped), SoapHeader("ServiceHeaderValue")]
        public string ActivateTrial(ref string paramStr)
        {
            object[] objArray = base.Invoke("ActivateTrial", new object[] { paramStr });
            paramStr = (string) objArray[1];
            return (string) objArray[0];
        }

        public void ActivateTrialAsync(string paramStr)
        {
            this.ActivateTrialAsync(paramStr, null);
        }

        public void ActivateTrialAsync(string paramStr, object userState)
        {
            if (this.sendOrPostCallback_4 == null)
            {
                this.sendOrPostCallback_4 = new SendOrPostCallback(this.method_4);
            }
            base.InvokeAsync("ActivateTrial", new object[] { paramStr }, this.sendOrPostCallback_4, userState);
        }

        [return: XmlElement("error")]
        [SoapDocumentMethod("http://www.wealth-lab.com/AuthEducational", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string AuthEducational(ref string paramStr)
        {
            object[] objArray = base.Invoke("AuthEducational", new object[] { paramStr });
            paramStr = (string) objArray[1];
            return (string) objArray[0];
        }

        public void AuthEducationalAsync(string paramStr)
        {
            this.AuthEducationalAsync(paramStr, null);
        }

        public void AuthEducationalAsync(string paramStr, object userState)
        {
            if (this.sendOrPostCallback_1 == null)
            {
                this.sendOrPostCallback_1 = new SendOrPostCallback(this.method_1);
            }
            base.InvokeAsync("AuthEducational", new object[] { paramStr }, this.sendOrPostCallback_1, userState);
        }

        [return: XmlElement("error")]
        [SoapDocumentMethod("http://www.wealth-lab.com/AuthTrial", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped), SoapHeader("ServiceHeaderValue")]
        public string AuthTrial(ref string paramStr)
        {
            object[] objArray = base.Invoke("AuthTrial", new object[] { paramStr });
            paramStr = (string) objArray[1];
            return (string) objArray[0];
        }

        public void AuthTrialAsync(string paramStr)
        {
            this.AuthTrialAsync(paramStr, null);
        }

        public void AuthTrialAsync(string paramStr, object userState)
        {
            if (this.sendOrPostCallback_3 == null)
            {
                this.sendOrPostCallback_3 = new SendOrPostCallback(this.method_3);
            }
            base.InvokeAsync("AuthTrial", new object[] { paramStr }, this.sendOrPostCallback_3, userState);
        }

        public void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private void method_0(object object_0)
        {
            if (this.activateKeyCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.activateKeyCompletedEventHandler_0(this, new ActivateKeyCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_1(object object_0)
        {
            if (this.authEducationalCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.authEducationalCompletedEventHandler_0(this, new AuthEducationalCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_2(object object_0)
        {
            if (this.tcompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.tcompletedEventHandler_0(this, new AsyncCompletedEventArgs(args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_3(object object_0)
        {
            if (this.authTrialCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.authTrialCompletedEventHandler_0(this, new AuthTrialCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_4(object object_0)
        {
            if (this.activateTrialCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.activateTrialCompletedEventHandler_0(this, new ActivateTrialCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private bool method_5(string string_0)
        {
            if ((string_0 == null) || (string_0 == string.Empty))
            {
                return false;
            }
            Uri uri = new Uri(string_0);
            return ((uri.Port >= 0x400) && (string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0));
        }

        [SoapDocumentMethod("http://www.wealth-lab.com/T", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public void T(AuthEducationalParams authParams)
        {
            base.Invoke("T", new object[] { authParams });
        }

        public void TAsync(AuthEducationalParams authParams)
        {
            this.TAsync(authParams, null);
        }

        public void TAsync(AuthEducationalParams authParams, object userState)
        {
            if (this.sendOrPostCallback_2 == null)
            {
                this.sendOrPostCallback_2 = new SendOrPostCallback(this.method_2);
            }
            base.InvokeAsync("T", new object[] { authParams }, this.sendOrPostCallback_2, userState);
        }

        public ServiceHeader ServiceHeaderValue
        {
            get
            {
                return this.serviceHeader_0;
            }
            set
            {
                this.serviceHeader_0 = value;
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
                if ((this.method_5(base.Url) && !this.bool_0) && !this.method_5(value))
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
                this.bool_0 = true;
            }
        }
    }
}

