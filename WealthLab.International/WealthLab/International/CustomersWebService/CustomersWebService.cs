using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using WealthLab.International.Properties;

namespace WealthLab.International.CustomersWebService
{
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [GeneratedCode("System.Web.Services", "4.0.30319.1")]
    [WebServiceBinding(Name="CustomersWebServiceSoap", Namespace="http://www.wealth-lab.com/")]
    public class CustomersWebService : SoapHttpClientProtocol
    {
        private ServiceHeader serviceHeader_0;

        private SendOrPostCallback sendOrPostCallback_0;

        private SendOrPostCallback sendOrPostCallback_1;

        private SendOrPostCallback sendOrPostCallback_2;

        private SendOrPostCallback sendOrPostCallback_3;

        private SendOrPostCallback sendOrPostCallback_4;

        private bool bool_0;

        private ActivateKeyCompletedEventHandler activateKeyCompletedEventHandler_0;

        private AuthEducationalCompletedEventHandler authEducationalCompletedEventHandler_0;

        private TCompletedEventHandler tcompletedEventHandler_0;

        private AuthTrialCompletedEventHandler authTrialCompletedEventHandler_0;

        private ActivateTrialCompletedEventHandler activateTrialCompletedEventHandler_0;

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
                if (this.method_5(base.Url) && !this.bool_0 && !this.method_5(value))
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

        public CustomersWebService()
        {
            this.Url = Settings.Default.WealthLab_International_localhost_CustomersWebService;
            if (!this.method_5(this.Url))
            {
                this.bool_0 = true;
                return;
            }
            else
            {
                this.UseDefaultCredentials = true;
                this.bool_0 = false;
                return;
            }
        }

        [SoapDocumentMethod("http://www.wealth-lab.com/ActivateKey", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        [SoapHeader("ServiceHeaderValue")]
        public string ActivateKey(ref string paramStr)
        {
            object[] objArray = new object[] { paramStr };
            object[] objArray1 = base.Invoke("ActivateKey", objArray);
            paramStr = (string)objArray1[1];
            return (string)objArray1[0];
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
            object[] objArray = new object[] { paramStr };
            base.InvokeAsync("ActivateKey", objArray, this.sendOrPostCallback_0, userState);
        }

        [SoapDocumentMethod("http://www.wealth-lab.com/ActivateTrial", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        [SoapHeader("ServiceHeaderValue")]
        public string ActivateTrial(ref string paramStr)
        {
            object[] objArray = new object[] { paramStr };
            object[] objArray1 = base.Invoke("ActivateTrial", objArray);
            paramStr = (string)objArray1[1];
            return (string)objArray1[0];
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
            object[] objArray = new object[] { paramStr };
            base.InvokeAsync("ActivateTrial", objArray, this.sendOrPostCallback_4, userState);
        }

        [SoapDocumentMethod("http://www.wealth-lab.com/AuthEducational", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string AuthEducational(ref string paramStr)
        {
            object[] objArray = new object[] { paramStr };
            object[] objArray1 = base.Invoke("AuthEducational", objArray);
            paramStr = (string)objArray1[1];
            return (string)objArray1[0];
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
            object[] objArray = new object[] { paramStr };
            base.InvokeAsync("AuthEducational", objArray, this.sendOrPostCallback_1, userState);
        }

        [SoapDocumentMethod("http://www.wealth-lab.com/AuthTrial", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        [SoapHeader("ServiceHeaderValue")]
        public string AuthTrial(ref string paramStr)
        {
            object[] objArray = new object[] { paramStr };
            object[] objArray1 = base.Invoke("AuthTrial", objArray);
            paramStr = (string)objArray1[1];
            return (string)objArray1[0];
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
            object[] objArray = new object[] { paramStr };
            base.InvokeAsync("AuthTrial", objArray, this.sendOrPostCallback_3, userState);
        }

        public void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private void method_0(object object_0)
        {
            if (this.activateKeyCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs object0 = (InvokeCompletedEventArgs)object_0;
                this.activateKeyCompletedEventHandler_0(this, new ActivateKeyCompletedEventArgs(object0.Results, object0.Error, object0.Cancelled, object0.UserState));
            }
        }

        private void method_1(object object_0)
        {
            if (this.authEducationalCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs object0 = (InvokeCompletedEventArgs)object_0;
                this.authEducationalCompletedEventHandler_0(this, new AuthEducationalCompletedEventArgs(object0.Results, object0.Error, object0.Cancelled, object0.UserState));
            }
        }

        private void method_2(object object_0)
        {
            if (this.tcompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs object0 = (InvokeCompletedEventArgs)object_0;
                this.tcompletedEventHandler_0(this, new AsyncCompletedEventArgs(object0.Error, object0.Cancelled, object0.UserState));
            }
        }

        private void method_3(object object_0)
        {
            if (this.authTrialCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs object0 = (InvokeCompletedEventArgs)object_0;
                this.authTrialCompletedEventHandler_0(this, new AuthTrialCompletedEventArgs(object0.Results, object0.Error, object0.Cancelled, object0.UserState));
            }
        }

        private void method_4(object object_0)
        {
            if (this.activateTrialCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs object0 = (InvokeCompletedEventArgs)object_0;
                this.activateTrialCompletedEventHandler_0(this, new ActivateTrialCompletedEventArgs(object0.Results, object0.Error, object0.Cancelled, object0.UserState));
            }
        }

        private bool method_5(string string_0)
        {
            if (string_0 == null || string_0 == string.Empty)
            {
                return false;
            }
            else
            {
                Uri uri = new Uri(string_0);
                if (uri.Port < 1024 || string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [SoapDocumentMethod("http://www.wealth-lab.com/T", RequestNamespace="http://www.wealth-lab.com/", ResponseNamespace="http://www.wealth-lab.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public void T(AuthEducationalParams authParams)
        {
            object[] objArray = new object[] { authParams };
            base.Invoke("T", objArray);
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
            object[] objArray = new object[] { authParams };
            base.InvokeAsync("T", objArray, this.sendOrPostCallback_2, userState);
        }

        public event ActivateKeyCompletedEventHandler ActivateKeyCompleted
        {
            add
            {
                ActivateKeyCompletedEventHandler activateKeyCompletedEventHandler;
                ActivateKeyCompletedEventHandler activateKeyCompletedEventHandler0 = this.activateKeyCompletedEventHandler_0;
                do
                {
                    activateKeyCompletedEventHandler = activateKeyCompletedEventHandler0;
                    ActivateKeyCompletedEventHandler activateKeyCompletedEventHandler1 = (ActivateKeyCompletedEventHandler)Delegate.Combine(activateKeyCompletedEventHandler, value);
                    activateKeyCompletedEventHandler0 = Interlocked.CompareExchange<ActivateKeyCompletedEventHandler>(ref this.activateKeyCompletedEventHandler_0, activateKeyCompletedEventHandler1, activateKeyCompletedEventHandler);
                }
                while (activateKeyCompletedEventHandler0 != activateKeyCompletedEventHandler);
            }
            remove
            {
                ActivateKeyCompletedEventHandler activateKeyCompletedEventHandler;
                ActivateKeyCompletedEventHandler activateKeyCompletedEventHandler0 = this.activateKeyCompletedEventHandler_0;
                do
                {
                    activateKeyCompletedEventHandler = activateKeyCompletedEventHandler0;
                    ActivateKeyCompletedEventHandler activateKeyCompletedEventHandler1 = (ActivateKeyCompletedEventHandler)Delegate.Remove(activateKeyCompletedEventHandler, value);
                    activateKeyCompletedEventHandler0 = Interlocked.CompareExchange<ActivateKeyCompletedEventHandler>(ref this.activateKeyCompletedEventHandler_0, activateKeyCompletedEventHandler1, activateKeyCompletedEventHandler);
                }
                while (activateKeyCompletedEventHandler0 != activateKeyCompletedEventHandler);
            }
        }

        public event ActivateTrialCompletedEventHandler ActivateTrialCompleted
        {
            add
            {
                ActivateTrialCompletedEventHandler activateTrialCompletedEventHandler;
                ActivateTrialCompletedEventHandler activateTrialCompletedEventHandler0 = this.activateTrialCompletedEventHandler_0;
                do
                {
                    activateTrialCompletedEventHandler = activateTrialCompletedEventHandler0;
                    ActivateTrialCompletedEventHandler activateTrialCompletedEventHandler1 = (ActivateTrialCompletedEventHandler)Delegate.Combine(activateTrialCompletedEventHandler, value);
                    activateTrialCompletedEventHandler0 = Interlocked.CompareExchange<ActivateTrialCompletedEventHandler>(ref this.activateTrialCompletedEventHandler_0, activateTrialCompletedEventHandler1, activateTrialCompletedEventHandler);
                }
                while (activateTrialCompletedEventHandler0 != activateTrialCompletedEventHandler);
            }
            remove
            {
                ActivateTrialCompletedEventHandler activateTrialCompletedEventHandler;
                ActivateTrialCompletedEventHandler activateTrialCompletedEventHandler0 = this.activateTrialCompletedEventHandler_0;
                do
                {
                    activateTrialCompletedEventHandler = activateTrialCompletedEventHandler0;
                    ActivateTrialCompletedEventHandler activateTrialCompletedEventHandler1 = (ActivateTrialCompletedEventHandler)Delegate.Remove(activateTrialCompletedEventHandler, value);
                    activateTrialCompletedEventHandler0 = Interlocked.CompareExchange<ActivateTrialCompletedEventHandler>(ref this.activateTrialCompletedEventHandler_0, activateTrialCompletedEventHandler1, activateTrialCompletedEventHandler);
                }
                while (activateTrialCompletedEventHandler0 != activateTrialCompletedEventHandler);
            }
        }

        public event AuthEducationalCompletedEventHandler AuthEducationalCompleted
        {
            add
            {
                AuthEducationalCompletedEventHandler authEducationalCompletedEventHandler;
                AuthEducationalCompletedEventHandler authEducationalCompletedEventHandler0 = this.authEducationalCompletedEventHandler_0;
                do
                {
                    authEducationalCompletedEventHandler = authEducationalCompletedEventHandler0;
                    AuthEducationalCompletedEventHandler authEducationalCompletedEventHandler1 = (AuthEducationalCompletedEventHandler)Delegate.Combine(authEducationalCompletedEventHandler, value);
                    authEducationalCompletedEventHandler0 = Interlocked.CompareExchange<AuthEducationalCompletedEventHandler>(ref this.authEducationalCompletedEventHandler_0, authEducationalCompletedEventHandler1, authEducationalCompletedEventHandler);
                }
                while (authEducationalCompletedEventHandler0 != authEducationalCompletedEventHandler);
            }
            remove
            {
                AuthEducationalCompletedEventHandler authEducationalCompletedEventHandler;
                AuthEducationalCompletedEventHandler authEducationalCompletedEventHandler0 = this.authEducationalCompletedEventHandler_0;
                do
                {
                    authEducationalCompletedEventHandler = authEducationalCompletedEventHandler0;
                    AuthEducationalCompletedEventHandler authEducationalCompletedEventHandler1 = (AuthEducationalCompletedEventHandler)Delegate.Remove(authEducationalCompletedEventHandler, value);
                    authEducationalCompletedEventHandler0 = Interlocked.CompareExchange<AuthEducationalCompletedEventHandler>(ref this.authEducationalCompletedEventHandler_0, authEducationalCompletedEventHandler1, authEducationalCompletedEventHandler);
                }
                while (authEducationalCompletedEventHandler0 != authEducationalCompletedEventHandler);
            }
        }

        public event AuthTrialCompletedEventHandler AuthTrialCompleted
        {
            add
            {
                AuthTrialCompletedEventHandler authTrialCompletedEventHandler;
                AuthTrialCompletedEventHandler authTrialCompletedEventHandler0 = this.authTrialCompletedEventHandler_0;
                do
                {
                    authTrialCompletedEventHandler = authTrialCompletedEventHandler0;
                    AuthTrialCompletedEventHandler authTrialCompletedEventHandler1 = (AuthTrialCompletedEventHandler)Delegate.Combine(authTrialCompletedEventHandler, value);
                    authTrialCompletedEventHandler0 = Interlocked.CompareExchange<AuthTrialCompletedEventHandler>(ref this.authTrialCompletedEventHandler_0, authTrialCompletedEventHandler1, authTrialCompletedEventHandler);
                }
                while (authTrialCompletedEventHandler0 != authTrialCompletedEventHandler);
            }
            remove
            {
                AuthTrialCompletedEventHandler authTrialCompletedEventHandler;
                AuthTrialCompletedEventHandler authTrialCompletedEventHandler0 = this.authTrialCompletedEventHandler_0;
                do
                {
                    authTrialCompletedEventHandler = authTrialCompletedEventHandler0;
                    AuthTrialCompletedEventHandler authTrialCompletedEventHandler1 = (AuthTrialCompletedEventHandler)Delegate.Remove(authTrialCompletedEventHandler, value);
                    authTrialCompletedEventHandler0 = Interlocked.CompareExchange<AuthTrialCompletedEventHandler>(ref this.authTrialCompletedEventHandler_0, authTrialCompletedEventHandler1, authTrialCompletedEventHandler);
                }
                while (authTrialCompletedEventHandler0 != authTrialCompletedEventHandler);
            }
        }

        public event TCompletedEventHandler TCompleted
        {
            add
            {
                TCompletedEventHandler tCompletedEventHandler;
                TCompletedEventHandler tcompletedEventHandler0 = this.tcompletedEventHandler_0;
                do
                {
                    tCompletedEventHandler = tcompletedEventHandler0;
                    TCompletedEventHandler tCompletedEventHandler1 = (TCompletedEventHandler)Delegate.Combine(tCompletedEventHandler, value);
                    tcompletedEventHandler0 = Interlocked.CompareExchange<TCompletedEventHandler>(ref this.tcompletedEventHandler_0, tCompletedEventHandler1, tCompletedEventHandler);
                }
                while (tcompletedEventHandler0 != tCompletedEventHandler);
            }
            remove
            {
                TCompletedEventHandler tCompletedEventHandler;
                TCompletedEventHandler tcompletedEventHandler0 = this.tcompletedEventHandler_0;
                do
                {
                    tCompletedEventHandler = tcompletedEventHandler0;
                    TCompletedEventHandler tCompletedEventHandler1 = (TCompletedEventHandler)Delegate.Remove(tCompletedEventHandler, value);
                    tcompletedEventHandler0 = Interlocked.CompareExchange<TCompletedEventHandler>(ref this.tcompletedEventHandler_0, tCompletedEventHandler1, tCompletedEventHandler);
                }
                while (tcompletedEventHandler0 != tCompletedEventHandler);
            }
        }
    }
}

