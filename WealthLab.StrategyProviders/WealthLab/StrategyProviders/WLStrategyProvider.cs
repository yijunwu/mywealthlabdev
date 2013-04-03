namespace WealthLab.StrategyProviders
{
    using MS123.Web.Strategies;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.StrategyProviders.WLWebServices;

    public class WLStrategyProvider : StrategyProvider
    {
        protected bool _cancel;
        private const string _Home = "http://www.wealth-lab.com/";
        private const string _LastDownloadKey = "LastStrategyDownload";
        private List<Strategy> _Strategies = new List<Strategy>();
        private const string _StrategyDownloadService = "StrategyWebservice.asmx";
        private const string _StrategyDownloadURL = "http://www.wealth-lab.com/WebServices/";
        private const string _StrategyDownloadURLKey = "StrategyDownloadURLOverride";
        private string _webServiceURL = "";
        private WLLogin _wll = new WLLogin();

        public override void CancelDownload()
        {
            this._cancel = true;
        }

        public override void DownloadStrategies(StrategyPubType stp, DateTime startDate)
        {
            StrategyWebservice webservice = new StrategyWebservice();
            int numberOfDaysBack = 0;
            if (!startDate.Equals(DateTime.MinValue))
            {
                TimeSpan span = (TimeSpan) (DateTime.Today - startDate);
                numberOfDaysBack = span.Days + 1;
            }
            webservice.Url = this._WebService;
            if ((stp & StrategyPubType.Private) > 0)
            {
                if (this._wll.ShowDialog() == DialogResult.OK)
                {
                    string[] sStrategies = webservice.GetPrivateStrategies(numberOfDaysBack, this._wll.Username, this._wll.Password);
                    if (this._cancel)
                    {
                        return;
                    }
                    this.ProcessStrategies(sStrategies, false);
                }
                else if ((stp & StrategyPubType.Public) == 0)
                {
                    base.StrategyHost.CancelDownload = true;
                }
            }
            if (!this._cancel)
            {
                if ((stp & StrategyPubType.Public) > 0)
                {
                    string[] publicStrategies = webservice.GetPublicStrategies(numberOfDaysBack);
                    if (this._cancel)
                    {
                        return;
                    }
                    this.ProcessStrategies(publicStrategies, true);
                }
                this.LastDownload = DateTime.Today;
            }
        }

        private void ProcessStrategies(string[] sStrategies, bool isPublic)
        {
            for (int i = 0; i < sStrategies.Length; i++)
            {
                if (this._cancel)
                {
                    return;
                }
                StrategyHandler handler = StrategyHandler.DeserializeFromString(sStrategies[i]);
                Strategy item = (Strategy) XmlSerializer.FromXml(handler.StrategyXML, typeof(Strategy));
                bool flag = false;
                foreach (Strategy strategy2 in this._Strategies)
                {
                    if (strategy2.ID == item.ID)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    this._Strategies.Add(item);
                    string str = isPublic ? "Downloaded Public (Uncategorized)" : "Downloaded Private";
                    base.StrategyHost.SaveStrategy(item, string.IsNullOrEmpty(handler.Folder) ? str : ((handler.Folder == "Not Selected") ? str : handler.Folder));
                }
            }
        }

        private string _WebService
        {
            get
            {
                return (this._WebServiceURL + "StrategyWebservice.asmx");
            }
        }

        private string _WebServiceURL
        {
            get
            {
                if (string.IsNullOrEmpty(this._webServiceURL))
                {
                    this._webServiceURL = base.SettingsHost.Get("StrategyDownloadURLOverride", "http://www.wealth-lab.com/WebServices/");
                }
                return this._webServiceURL;
            }
        }

        public override string Description
        {
            get
            {
                return "Public and private strategies from Wealth-Lab.com";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Wealth-Lab.com";
            }
        }

        public override DateTime LastDownload
        {
            get
            {
                return base.SettingsHost.Get("LastStrategyDownload", DateTime.Today.Subtract(new TimeSpan(30, 0, 0, 0)));
            }
            set
            {
                base.SettingsHost.Set("LastStrategyDownload", value);
            }
        }

        public override string StrategyProviderURL
        {
            get
            {
                return "http://www.wealth-lab.com/";
            }
        }

        public override StrategyPubType StrategyTypesProvided
        {
            get
            {
                return (StrategyPubType.Public | StrategyPubType.Private);
            }
        }
    }
}

