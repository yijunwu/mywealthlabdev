namespace WealthLab.Extensions.Server
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.Services;
    using System.Web.Services.Description;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using WealthLab.Extensions.Properties;

    [DebuggerStepThrough, DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.3053"), WebServiceBinding(Name="ExtensionManagerSoap", Namespace="http://tempuri.org/")]
    public class ExtensionManager : SoapHttpClientProtocol
    {
        private bool bool_0;
        private SendOrPostCallback sendOrPostCallback_0;
        private SendOrPostCallback sendOrPostCallback_1;
        private SendOrPostCallback sendOrPostCallback_2;
        private SendOrPostCallback sendOrPostCallback_3;
        private SendOrPostCallback sendOrPostCallback_4;
        private SendOrPostCallback sendOrPostCallback_5;
        private SendOrPostCallback sendOrPostCallback_6;
        private SendOrPostCallback sendOrPostCallback_7;

        private GetMoreExtensionsUrlCompletedEventHandler getMoreExtensionsUrlCompletedEventHandler_0;

        private GetChangeLogUrlCompletedEventHandler getChangeLogUrlCompletedEventHandler_0;

        private GetExtensionVersionsCompletedEventHandler getExtensionVersionsCompletedEventHandler_0;

        private GetExtensionInfoAttributesCompletedEventHandler getExtensionInfoAttributesCompletedEventHandler_0;

        private GetDownloadUrlsCompletedEventHandler getDownloadUrlsCompletedEventHandler_0;

        private RecordExtensionDownloadsCompletedEventHandler recordExtensionDownloadsCompletedEventHandler_0;

        private GetExtensionKeyCompletedEventHandler getExtensionKeyCompletedEventHandler_0;

        private GetExtensionIVCompletedEventHandler getExtensionIVCompletedEventHandler_0;

        public event GetChangeLogUrlCompletedEventHandler GetChangeLogUrlCompleted
        {
            add
            {
                this.getChangeLogUrlCompletedEventHandler_0 += value;
            }
            remove
            {
                this.getChangeLogUrlCompletedEventHandler_0 -= value;
            }
        }

        public event GetDownloadUrlsCompletedEventHandler GetDownloadUrlsCompleted
        {
            add
            {
                this.getDownloadUrlsCompletedEventHandler_0 += value;
            }
            remove
            {
                this.getDownloadUrlsCompletedEventHandler_0 -= value;
            }
        }

        public event GetExtensionInfoAttributesCompletedEventHandler GetExtensionInfoAttributesCompleted
        {
            add
            {
                this.getExtensionInfoAttributesCompletedEventHandler_0 += value;
            }
            remove
            {
                this.getExtensionInfoAttributesCompletedEventHandler_0 -= value;
            }
        }

        public event GetExtensionIVCompletedEventHandler GetExtensionIVCompleted
        {
            add
            {
                this.getExtensionIVCompletedEventHandler_0 += value;
            }
            remove
            {
                this.getExtensionIVCompletedEventHandler_0 -= value;
            }
        }

        public event GetExtensionKeyCompletedEventHandler GetExtensionKeyCompleted
        {
            add
            {
                this.getExtensionKeyCompletedEventHandler_0 += value;
            }
            remove
            {
                this.getExtensionKeyCompletedEventHandler_0 -= value;
            }
        }

        public event GetExtensionVersionsCompletedEventHandler GetExtensionVersionsCompleted
        {
            add
            {
                this.getExtensionVersionsCompletedEventHandler_0 += value;
            }
            remove
            {
                this.getExtensionVersionsCompletedEventHandler_0 -= value;
            }
        }

        public event GetMoreExtensionsUrlCompletedEventHandler GetMoreExtensionsUrlCompleted
        {
            add
            {
                this.getMoreExtensionsUrlCompletedEventHandler_0 += value;
            }
            remove
            {
                this.getMoreExtensionsUrlCompletedEventHandler_0 -= value;
            }
        }

        public event RecordExtensionDownloadsCompletedEventHandler RecordExtensionDownloadsCompleted
        {
            add
            {
                this.recordExtensionDownloadsCompletedEventHandler_0 += value;
            }
            remove
            {
                this.recordExtensionDownloadsCompletedEventHandler_0 -= value;
            }
        }
        public ExtensionManager()
        {
            this.Url = Settings.Default.WealthLab_Extensions_net_wli5_ExtensionWebservice;
            if (this.method_8(this.Url))
            {
                this.UseDefaultCredentials = true;
                this.bool_0 = false;
            }
            else
            {
                this.bool_0 = true;
            }
        }

        public void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        [SoapDocumentMethod("http://tempuri.org/GetChangeLogUrl", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string GetChangeLogUrl(string ExtensionName)
        {
            return (string) base.Invoke("GetChangeLogUrl", new object[] { ExtensionName })[0];
        }

        public void GetChangeLogUrlAsync(string ExtensionName)
        {
            this.GetChangeLogUrlAsync(ExtensionName, null);
        }

        public void GetChangeLogUrlAsync(string ExtensionName, object userState)
        {
            if (this.sendOrPostCallback_1 == null)
            {
                this.sendOrPostCallback_1 = new SendOrPostCallback(this.method_1);
            }
            base.InvokeAsync("GetChangeLogUrl", new object[] { ExtensionName }, this.sendOrPostCallback_1, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/GetDownloadUrls", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string[] GetDownloadUrls(string ExtensionName, string ExtensionVersion)
        {
            return (string[]) base.Invoke("GetDownloadUrls", new object[] { ExtensionName, ExtensionVersion })[0];
        }

        public void GetDownloadUrlsAsync(string ExtensionName, string ExtensionVersion)
        {
            this.GetDownloadUrlsAsync(ExtensionName, ExtensionVersion, null);
        }

        public void GetDownloadUrlsAsync(string ExtensionName, string ExtensionVersion, object userState)
        {
            if (this.sendOrPostCallback_4 == null)
            {
                this.sendOrPostCallback_4 = new SendOrPostCallback(this.method_4);
            }
            base.InvokeAsync("GetDownloadUrls", new object[] { ExtensionName, ExtensionVersion }, this.sendOrPostCallback_4, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/GetExtensionInfoAttributes", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string[] GetExtensionInfoAttributes()
        {
            return (string[]) base.Invoke("GetExtensionInfoAttributes", new object[0])[0];
        }

        public void GetExtensionInfoAttributesAsync()
        {
            this.GetExtensionInfoAttributesAsync(null);
        }

        public void GetExtensionInfoAttributesAsync(object userState)
        {
            if (this.sendOrPostCallback_3 == null)
            {
                this.sendOrPostCallback_3 = new SendOrPostCallback(this.method_3);
            }
            base.InvokeAsync("GetExtensionInfoAttributes", new object[0], this.sendOrPostCallback_3, userState);
        }

        [return: XmlElement(DataType="base64Binary")]
        [SoapDocumentMethod("http://tempuri.org/GetExtensionIV", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public byte[] GetExtensionIV(string ExtensionName, string ExtensionVersion)
        {
            return (byte[]) base.Invoke("GetExtensionIV", new object[] { ExtensionName, ExtensionVersion })[0];
        }

        public void GetExtensionIVAsync(string ExtensionName, string ExtensionVersion)
        {
            this.GetExtensionIVAsync(ExtensionName, ExtensionVersion, null);
        }

        public void GetExtensionIVAsync(string ExtensionName, string ExtensionVersion, object userState)
        {
            if (this.sendOrPostCallback_7 == null)
            {
                this.sendOrPostCallback_7 = new SendOrPostCallback(this.method_7);
            }
            base.InvokeAsync("GetExtensionIV", new object[] { ExtensionName, ExtensionVersion }, this.sendOrPostCallback_7, userState);
        }

        [return: XmlElement(DataType="base64Binary")]
        [SoapDocumentMethod("http://tempuri.org/GetExtensionKey", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public byte[] GetExtensionKey(string ExtensionName, string ExtensionVersion)
        {
            return (byte[]) base.Invoke("GetExtensionKey", new object[] { ExtensionName, ExtensionVersion })[0];
        }

        public void GetExtensionKeyAsync(string ExtensionName, string ExtensionVersion)
        {
            this.GetExtensionKeyAsync(ExtensionName, ExtensionVersion, null);
        }

        public void GetExtensionKeyAsync(string ExtensionName, string ExtensionVersion, object userState)
        {
            if (this.sendOrPostCallback_6 == null)
            {
                this.sendOrPostCallback_6 = new SendOrPostCallback(this.method_6);
            }
            base.InvokeAsync("GetExtensionKey", new object[] { ExtensionName, ExtensionVersion }, this.sendOrPostCallback_6, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/GetExtensionVersions", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string[] GetExtensionVersions(string ExtensionName)
        {
            return (string[]) base.Invoke("GetExtensionVersions", new object[] { ExtensionName })[0];
        }

        public void GetExtensionVersionsAsync(string ExtensionName)
        {
            this.GetExtensionVersionsAsync(ExtensionName, null);
        }

        public void GetExtensionVersionsAsync(string ExtensionName, object userState)
        {
            if (this.sendOrPostCallback_2 == null)
            {
                this.sendOrPostCallback_2 = new SendOrPostCallback(this.method_2);
            }
            base.InvokeAsync("GetExtensionVersions", new object[] { ExtensionName }, this.sendOrPostCallback_2, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/GetMoreExtensionsUrl", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string GetMoreExtensionsUrl()
        {
            return (string) base.Invoke("GetMoreExtensionsUrl", new object[0])[0];
        }

        public void GetMoreExtensionsUrlAsync()
        {
            this.GetMoreExtensionsUrlAsync(null);
        }

        public void GetMoreExtensionsUrlAsync(object userState)
        {
            if (this.sendOrPostCallback_0 == null)
            {
                this.sendOrPostCallback_0 = new SendOrPostCallback(this.method_0);
            }
            base.InvokeAsync("GetMoreExtensionsUrl", new object[0], this.sendOrPostCallback_0, userState);
        }

        private void method_0(object object_0)
        {
            if (this.getMoreExtensionsUrlCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.getMoreExtensionsUrlCompletedEventHandler_0(this, new GetMoreExtensionsUrlCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_1(object object_0)
        {
            if (this.getChangeLogUrlCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.getChangeLogUrlCompletedEventHandler_0(this, new GetChangeLogUrlCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_2(object object_0)
        {
            if (this.getExtensionVersionsCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.getExtensionVersionsCompletedEventHandler_0(this, new GetExtensionVersionsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_3(object object_0)
        {
            if (this.getExtensionInfoAttributesCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.getExtensionInfoAttributesCompletedEventHandler_0(this, new GetExtensionInfoAttributesCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_4(object object_0)
        {
            if (this.getDownloadUrlsCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.getDownloadUrlsCompletedEventHandler_0(this, new GetDownloadUrlsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_5(object object_0)
        {
            if (this.recordExtensionDownloadsCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.recordExtensionDownloadsCompletedEventHandler_0(this, new AsyncCompletedEventArgs(args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_6(object object_0)
        {
            if (this.getExtensionKeyCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.getExtensionKeyCompletedEventHandler_0(this, new GetExtensionKeyCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void method_7(object object_0)
        {
            if (this.getExtensionIVCompletedEventHandler_0 != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) object_0;
                this.getExtensionIVCompletedEventHandler_0(this, new GetExtensionIVCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private bool method_8(string string_0)
        {
            if ((string_0 == null) || (string_0 == string.Empty))
            {
                return false;
            }
            Uri uri = new Uri(string_0);
            return ((uri.Port >= 0x400) && (string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0));
        }

        [SoapDocumentMethod("http://tempuri.org/RecordExtensionDownloads", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public void RecordExtensionDownloads(string extensionName, string extensionVersion, string username, DateTime date, string string_0)
        {
            base.Invoke("RecordExtensionDownloads", new object[] { extensionName, extensionVersion, username, date, string_0 });
        }

        public void RecordExtensionDownloadsAsync(string extensionName, string extensionVersion, string username, DateTime date, string string_0)
        {
            this.RecordExtensionDownloadsAsync(extensionName, extensionVersion, username, date, string_0, null);
        }

        public void RecordExtensionDownloadsAsync(string extensionName, string extensionVersion, string username, DateTime date, string string_0, object userState)
        {
            if (this.sendOrPostCallback_5 == null)
            {
                this.sendOrPostCallback_5 = new SendOrPostCallback(this.method_5);
            }
            base.InvokeAsync("RecordExtensionDownloads", new object[] { extensionName, extensionVersion, username, date, string_0 }, this.sendOrPostCallback_5, userState);
        }

        public string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((this.method_8(base.Url) && !this.bool_0) && !this.method_8(value))
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

