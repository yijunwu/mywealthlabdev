namespace WealthLab
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class AuthenticationProvider
    {
        private AuthenticationProvider._DownloadFileCompleted _DownloadFileCompleted_0;
        private AuthenticationProvider._DownloadProgressChanged _DownloadProgressChanged_0;


        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private IAuthenticationHost iauthenticationHost_0;
        private IDataHost idataHost_0;

        public event AuthenticationProvider._DownloadFileCompleted OnInstallerDownloadComplete
        {
            add
            {
                AuthenticationProvider._DownloadFileCompleted __DownloadFileCompleted;
                AuthenticationProvider._DownloadFileCompleted _DownloadFileCompleted0 = this._DownloadFileCompleted_0;
                do
                {
                    __DownloadFileCompleted = _DownloadFileCompleted0;
                    AuthenticationProvider._DownloadFileCompleted __DownloadFileCompleted1 = (AuthenticationProvider._DownloadFileCompleted)Delegate.Combine(__DownloadFileCompleted, value);
                    _DownloadFileCompleted0 = Interlocked.CompareExchange<AuthenticationProvider._DownloadFileCompleted>(ref this._DownloadFileCompleted_0, __DownloadFileCompleted1, __DownloadFileCompleted);
                }
                while (_DownloadFileCompleted0 != __DownloadFileCompleted);
            }
            remove
            {
                AuthenticationProvider._DownloadFileCompleted __DownloadFileCompleted;
                AuthenticationProvider._DownloadFileCompleted _DownloadFileCompleted0 = this._DownloadFileCompleted_0;
                do
                {
                    __DownloadFileCompleted = _DownloadFileCompleted0;
                    AuthenticationProvider._DownloadFileCompleted __DownloadFileCompleted1 = (AuthenticationProvider._DownloadFileCompleted)Delegate.Remove(__DownloadFileCompleted, value);
                    _DownloadFileCompleted0 = Interlocked.CompareExchange<AuthenticationProvider._DownloadFileCompleted>(ref this._DownloadFileCompleted_0, __DownloadFileCompleted1, __DownloadFileCompleted);
                }
                while (_DownloadFileCompleted0 != __DownloadFileCompleted);
            }
        }

        public event AuthenticationProvider._DownloadProgressChanged OnInstallerDownloadProgressChanged
        {
            add
            {
                AuthenticationProvider._DownloadProgressChanged __DownloadProgressChanged;
                AuthenticationProvider._DownloadProgressChanged _DownloadProgressChanged0 = this._DownloadProgressChanged_0;
                do
                {
                    __DownloadProgressChanged = _DownloadProgressChanged0;
                    AuthenticationProvider._DownloadProgressChanged __DownloadProgressChanged1 = (AuthenticationProvider._DownloadProgressChanged)Delegate.Combine(__DownloadProgressChanged, value);
                    _DownloadProgressChanged0 = Interlocked.CompareExchange<AuthenticationProvider._DownloadProgressChanged>(ref this._DownloadProgressChanged_0, __DownloadProgressChanged1, __DownloadProgressChanged);
                }
                while (_DownloadProgressChanged0 != __DownloadProgressChanged);
            }
            remove
            {
                AuthenticationProvider._DownloadProgressChanged __DownloadProgressChanged;
                AuthenticationProvider._DownloadProgressChanged _DownloadProgressChanged0 = this._DownloadProgressChanged_0;
                do
                {
                    __DownloadProgressChanged = _DownloadProgressChanged0;
                    AuthenticationProvider._DownloadProgressChanged __DownloadProgressChanged1 = (AuthenticationProvider._DownloadProgressChanged)Delegate.Remove(__DownloadProgressChanged, value);
                    _DownloadProgressChanged0 = Interlocked.CompareExchange<AuthenticationProvider._DownloadProgressChanged>(ref this._DownloadProgressChanged_0, __DownloadProgressChanged1, __DownloadProgressChanged);
                }
                while (_DownloadProgressChanged0 != __DownloadProgressChanged);
            }
        }

        protected AuthenticationProvider()
        {
        }

        public abstract bool Authenticate(ref int daysBeforeNextAuthRequired, ref string string_0);
        public virtual void Close()
        {
        }

        public virtual bool DoUpgradeCheck()
        {
            return false;
        }

        protected virtual void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (this._DownloadFileCompleted_0 != null)
            {
                Delegate[] invocationList = this._DownloadFileCompleted_0.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    invocationList[i].DynamicInvoke(new object[] { sender, e });
                }
            }
        }

        protected virtual void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (this._DownloadProgressChanged_0 != null)
            {
                Delegate[] invocationList = this._DownloadProgressChanged_0.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    invocationList[i].DynamicInvoke(new object[] { sender, e });
                }
            }
        }

        public virtual void Initialize(IDataHost dataHost, IAuthenticationHost authHost)
        {
            this.idataHost_0 = dataHost;
            this.iauthenticationHost_0 = authHost;
        }

        public virtual void PreInitialize()
        {
        }

        public virtual bool UnAuthenticate()
        {
            return true;
        }

        public bool AllowAutoTrading
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public bool AllowStreaming
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public abstract string ApplicationName { get; }

        public abstract string ApplicationVersion { get; }

        public IAuthenticationHost AuthHost
        {
            get
            {
                return this.iauthenticationHost_0;
            }
        }

        public IDataHost DataHost
        {
            get
            {
                return this.idataHost_0;
            }
        }

        public virtual bool ForceAuthentication
        {
            get
            {
                return false;
            }
        }

        public virtual DateTime GetCurrentDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public abstract Bitmap Glyph { get; }

        public abstract int GracePeriod { get; }

        public bool LoggedIn
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
                if (this.bool_0)
                {
                    this.iauthenticationHost_0.LoginSuccessful();
                }
                else
                {
                    this.iauthenticationHost_0.LoginUnsuccessful();
                }
            }
        }

        public abstract string LoggedInPhrase { get; }

        public virtual bool LoginButtonVisible
        {
            get
            {
                return true;
            }
        }

        public abstract string LoginPhrase { get; }

        public abstract string Name { get; }

        public virtual DateTime NextAuthRequired
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        public virtual bool ShowGracePeriodWarning
        {
            get
            {
                return true;
            }
        }

        public virtual bool ShowTradeTicket
        {
            get
            {
                return false;
            }
        }

        public virtual bool SupportsSoftwareUpgrade
        {
            get
            {
                return false;
            }
        }

        public virtual string ThirdPartySiteWarning
        {
            get
            {
                return "";
            }
        }

        public virtual string UserDataPathToken
        {
            get
            {
                return "WealthLabPro";
            }
        }

        public abstract string WhatsNewLink { get; }

        public delegate void _DownloadFileCompleted(object sender, AsyncCompletedEventArgs e);

        public delegate void _DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e);
    }
}

