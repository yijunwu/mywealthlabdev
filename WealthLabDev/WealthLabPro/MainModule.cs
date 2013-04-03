namespace WealthLabPro
{
    using Fidelity.Components;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Media;
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;
    using WealthLab.Commissions;
    using WealthLab.PosSizers;
    using WealthLabPro.Properties;

    [ToolboxItem(false)]
    public class MainModule : UserControl, IComparer<StaticDataProvider>, IConnectionStatus, IAuthenticationHost, IMenuItemAdder
    {
        private AssemblyLoader assemblyLoader_0;
        private AssemblyLoader assemblyLoader_1;
        private AssemblyLoader assemblyLoader_2;
        internal AssemblyLoader assemblyLoader_3;
        private AssemblyLoader assemblyLoader_4;
        private AuthenticationProvider authenticationProvider_0;
        private BarDataRangeSelecter barRange;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private WealthLab.BrokerProvider brokerProvider_0;
        private ChartRenderer chartRenderer_0;
        private DataSourceManager dataSourceManager_0;
        private DateTime dateTime_0;
        private DrawingObjectManager drawingObjectManager_0;
        private System.Windows.Forms.HelpProvider helpProvider_0;
        private IContainer icontainer_0;
        private static readonly ILog ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static MainModule Instance = new MainModule();
        private int int_0;
        private int int_1;
        private List<string> list_0;
        private List<IPerformanceVisualizer> list_1;
        private List<IPerformanceVisualizer> list_2;
        private List<Strategy> list_3;
        private List<Account> list_4;
        private List<string> list_5;
        private List<DynamicMenuItem> list_6;
        private List<string> list_7;
        private List<Optimizer> list_8;
        private List<PosSizer> list_9;
        private SettingsManager settingsManager_0;
        private StrategyManager strategyManager_0;
        private StrategyManager strategyManager_1;
        private StreamingDataProvider streamingDataProvider_0;
        private string string_0;
        private string string_1;
        private System.Windows.Forms.Timer timer_0;
        private WealthLab.TradeManager tradeManager_0;
        private TradingSystemExecutor tradingSystemExecutor_0;

        public MainModule()
        {
            bool flag4;
            List<System.Type>.Enumerator enumerator;
            List<IPerformanceVisualizer>.Enumerator enumerator2;
            this.list_0 = new List<string>();
            this.list_1 = new List<IPerformanceVisualizer>();
            this.list_2 = new List<IPerformanceVisualizer>();
            this.string_0 = "";
            this.list_3 = new List<Strategy>();
            this.list_4 = new List<Account>();
            this.string_1 = "";
            this.list_5 = new List<string>();
            this.list_6 = new List<DynamicMenuItem>();
            this.list_7 = new List<string>();
            this.bool_4 = true;
            this.list_8 = new List<Optimizer>();
            this.list_9 = new List<PosSizer>();
            this.dateTime_0 = DateTime.MinValue;
            AssemblyLoader.LogFileName = this.DataPath + @"\Assemblies.wll";
            if (!base.DesignMode)
            {
                AssemblyLoader loader = new AssemblyLoader {
                    DLLNameFilter = "WealthLab.DataProviders",
                    BaseClass = "AuthenticationProvider",
                    Path = this.AppPath
                };
                if (loader.Types.Count == 0)
                {
                    loader.DLLNameFilter = "WealthLab.International";
                    loader.Path = this.AppPath;
                }
                if (loader.Types.Count == 0)
                {
                    loader.DLLNameFilter = "EduAuthProvider";
                    loader.Path = this.AppPath;
                }
                if (loader.Types.Count == 0)
                {
                    loader.DLLNameFilter = "WealthLab.FIL";
                    loader.Path = this.AppPath;
                }
                if (loader.Types.Count == 0)
                {
                    MessageBox.Show("Could not find an Authentication Provider, terminating");
                    Environment.Exit(2);
                }
                this.authenticationProvider_0 = (AuthenticationProvider) loader.CreateInstance(loader.Types[0]);
                this.authenticationProvider_0.PreInitialize();
            }
            this.string_1 = this.DataPath + @"\Accounts.txt";
            this.InitializeComponent();
            CustomIndexManager.Initialize(this.DataPath, this.dataSourceManager_0);
            if (Application.ProductName == "WealthLabPro")
            {
                this.chartRenderer_0.FundamentalGlyphs = "split;dividend;earnings per share;";
            }
            if (base.DesignMode)
            {
                return;
            }
            Instance = this;
            Application.ApplicationExit += new EventHandler(this.MainModule_Click);
            if (!Directory.Exists(this.DataPath + @"\Strategies"))
            {
                this.method_7(Path.GetDirectoryName(Application.ExecutablePath) + @"\Data", this.DataPath);
            }
            string path = this.DataPath + @"\Workspaces";
            string str3 = Path.GetDirectoryName(Application.ExecutablePath) + @"\Data\Workspaces";
            if (Directory.Exists(str3))
            {
                if (Directory.Exists(path))
                {
                    foreach (string str4 in Directory.GetFiles(str3))
                    {
                        string str5 = path + @"\" + Path.GetFileName(str4);
                        if (!File.Exists(str5))
                        {
                            File.Copy(str4, str5);
                        }
                    }
                }
                else
                {
                    this.method_7(str3, path);
                }
            }
            this.authenticationProvider_0.Initialize(this.DataSources, this);
            AssemblyLoader loader2 = new AssemblyLoader {
                BaseClass = "MenuItemHook",
                Path = this.AppPath
            };
            foreach (System.Type type in loader2.Types)
            {
                ((MenuItemHook) loader2.CreateInstance(type)).AddMenuItems(this);
            }
            this.dataSourceManager_0.AuthProvider = this.authenticationProvider_0;
            this.dataSourceManager_0.RootPath = this.DataPath;
            this.drawingObjectManager_0.RootPath = this.DataPath;
            this.settingsManager_0.RootPath = this.DataPath;
            this.strategyManager_0.RootPath = this.DataPath;
            BarsLoader.RootPath = this.DataPath;
            BarsLoader.LoadSymbolInfo();
            int num = this.settingsManager_0.Get("StrategyNetworkPathCount", 0);
            for (int i = 0; i < num; i++)
            {
                string item = this.settingsManager_0.Get("StrategyNetworkPath" + i, "");
                this.StrategyNetworkPaths.Add(item);
                this.strategyManager_0.LoadStrategiesFromNetworkPath(item);
            }
            MarketHours.RootPath = this.AppPath + @"\Data";
            this.dataSourceManager_0.SettingsHost = this.settingsManager_0;
            this.tradeManager_0.SettingsHost = this.settingsManager_0;
            AssemblyLoader loader3 = new AssemblyLoader {
                BaseClass = "BrokerProvider",
                Path = this.AppPath
            };
            if (loader3.Types.Count > 0)
            {
                foreach (System.Type type2 in loader3.Types)
                {
                    bool flag = false;
                    foreach (Attribute attribute in type2.GetCustomAttributes(true))
                    {
                        if (attribute is ProductionBrokerProvider)
                        {
                            goto Label_04C9;
                        }
                    }
                    goto Label_04CC;
                Label_04C9:
                    flag = true;
                Label_04CC:
                    if (flag)
                    {
                        this.brokerProvider_0 = (WealthLab.BrokerProvider) loader3.CreateInstance(type2);
                        this.brokerProvider_0.Initialize(this.TradeManager, this.authenticationProvider_0);
                        if (this.brokerProvider_0 is ICustomSettings)
                        {
                            (this.brokerProvider_0 as ICustomSettings).ReadSettings(this.Settings);
                        }
                        this.TradeManager.BrokerProvider = this.brokerProvider_0;
                    }
                }
            }
            bool flag2 = (DateTime.Now > this.NextAuthRequired) || this.AuthProvider.ForceAuthentication;
            bool flag3 = false;
            if (!flag2)
            {
                flag3 = (this.NextAuthRequired - DateTime.Now.Date) <= new TimeSpan(5, 0, 0, 0);
                TimeSpan span = (TimeSpan) (this.NextAuthRequired - DateTime.Now.Date);
                int days = span.Days;
                if (flag3)
                {
                    if (this.AuthProvider.ShowGracePeriodWarning)
                    {
                        if (MessageBox.Show("You must log in within the next " + days.ToString() + " days to continue to use " + Instance.AuthProvider.ApplicationName + ".  Do you want to Log in now?", "Log In", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            flag3 = false;
                        }
                    }
                    else
                    {
                        flag3 = false;
                    }
                }
            }
            if (flag2 | flag3)
            {
                if (flag2 && this.AuthProvider.ShowGracePeriodWarning)
                {
                    //MessageBox.Show("You must log in to continue using " + Instance.AuthProvider.ApplicationName + ".", "Log In", MessageBoxButtons.OK); ///WYJ fix
                }
                if (!this.Authenticate() && false ) //&& flag2) ///WYJ fix
                {
                    MessageBox.Show("Log in required, terminating");
                    this.authenticationProvider_0.Close();
                    Environment.Exit(1);
                }
            }
            if (this.dataSourceManager_0.DataSources.Count == 0)
            {
                StaticDataProvider provider = this.dataSourceManager_0.FindProvider("FidelityStaticProvider");
                if (provider != null)
                {
                    provider.Initialize(this.dataSourceManager_0);
                    DataSource source = new DataSource(provider) {
                        DSString = "AA,AIG,AXP,BA,C,CAT,DD,DIS,GE,GM,HD,HON,HPQ,IBM,INTC,JNJ,JPM,KO,MCD,MMM,MO,MRK,MSFT,PFE,PG,T,UTX,VZ,WMT,XOM",
                        Name = "Dow 30",
                        Scale = BarScale.Daily
                    };
                    this.dataSourceManager_0.Add(source);
                }
            }
            this.assemblyLoader_0.Path = this.AppPath;
            List<IPerformanceVisualizer> list = new List<IPerformanceVisualizer>();
            foreach (System.Type type3 in this.assemblyLoader_0.Types)
            {
                try
                {
                    Control control = (Control) this.assemblyLoader_0.CreateInstance(type3);
                    IPerformanceVisualizer visualizer = control as IPerformanceVisualizer;
                    list.Add(visualizer);
                }
                catch (Exception exception)
                {
                    ilog_0.Error("Inside ManiModule, trying to create the instance of Visualizers");
                    ilog_0.Error(" The exception message is " + exception.Message);
                    if (exception.InnerException != null)
                    {
                        ilog_0.Error("The inner Exception is " + exception.InnerException.Message);
                    }
                }
            }
            foreach (string str8 in this.settingsManager_0.Get("PVOrder", "Performance|By Symbol|Trades|Equity Curve|Drawdown|Profit Distribution|By Period|MAE/MFE").Split(new char[] { '|' }))
            {
                using (enumerator2 = list.GetEnumerator())
                {
                    IPerformanceVisualizer current;
                    while (enumerator2.MoveNext())
                    {
                        current = enumerator2.Current;
                        if (current.TabText == str8)
                        {
                            goto Label_0850;
                        }
                    }
                    continue;
                Label_0850:
                    this.list_1.Add(current);
                    list.Remove(current);
                }
            }
            foreach (IPerformanceVisualizer visualizer3 in list)
            {
                this.list_1.Add(visualizer3);
            }
            foreach (string str10 in this.settingsManager_0.Get("PVChecked", "Performance|By Symbol|Trades|Equity Curve|Drawdown|Profit Distribution|By Period").Split(new char[] { '|' }))
            {
                using (enumerator2 = this.list_1.GetEnumerator())
                {
                    IPerformanceVisualizer visualizer4;
                    while (enumerator2.MoveNext())
                    {
                        visualizer4 = enumerator2.Current;
                        if (visualizer4.TabText == str10)
                        {
                            goto Label_0934;
                        }
                    }
                    continue;
                Label_0934:
                    this.list_2.Add(visualizer4);
                }
            }
            this.chartRenderer_0.BarSpacing = this.settingsManager_0.Get("BarSpacing", 6);
            this.chartRenderer_0.BackgroundColor = this.settingsManager_0.Get("ChartBackgroundColor", Color.White);
            this.chartRenderer_0.UpBarColor = this.settingsManager_0.Get("ChartUpBarColor", Color.Green);
            this.chartRenderer_0.DownBarColor = this.settingsManager_0.Get("ChartDownBarColor", Color.Red);
            this.chartRenderer_0.UpBarVolumeColor = this.settingsManager_0.Get("ChartUpVolumeColor", Color.Green);
            this.chartRenderer_0.DownBarVolumeColor = this.settingsManager_0.Get("ChartDownVolumeColor", Color.Red);
            this.chartRenderer_0.GridlineColor = this.settingsManager_0.Get("ChartGridlineColor", Color.Gainsboro);
            this.chartRenderer_0.MarginRightColor = this.settingsManager_0.Get("ChartRightMarginColor", Color.Gainsboro);
            this.chartRenderer_0.MarginBottomColor = this.settingsManager_0.Get("ChartBottomMarginColor", Color.Navy);
            this.chartRenderer_0.PaneSeparatorColor = this.settingsManager_0.Get("ChartPaneSeparatorColor", Color.Black);
            this.chartRenderer_0.AxisFont = this.settingsManager_0.Get("ChartFont", this.chartRenderer_0.AxisFont);
            this.chartRenderer_0.FundamentalGlyphs = this.settingsManager_0.Get("FundamentalsCharted", this.chartRenderer_0.FundamentalGlyphs);
            this.chartRenderer_0.LogScale = this.settingsManager_0.Get("LogScale", false);
            this.chartRenderer_0.TitleFont = this.settingsManager_0.Get("TitleFont", this.chartRenderer_0.TitleFont);
            this.chartRenderer_0.HorizontalGridines = this.settingsManager_0.Get("HorizontalGridlines", true);
            this.chartRenderer_0.VerticalGridlines = this.settingsManager_0.Get("VerticalGridlines", true);
            this.chartRenderer_0.PaneSeparatorVisible = this.settingsManager_0.Get("PaneSeparators", true);
            this.assemblyLoader_1.Path = this.AppPath;
            this.tradingSystemExecutor_0.ApplyCommission = this.settingsManager_0.Get("ApplyCommissions", true);
            string str11 = this.settingsManager_0.Get("Commission", "FidelityFlatRate");
            System.Type type4 = this.method_3();
            using (enumerator = this.assemblyLoader_1.Types.GetEnumerator())
            {
                System.Type type5;
                while (enumerator.MoveNext())
                {
                    type5 = enumerator.Current;
                    if (type5.Name == str11)
                    {
                        goto Label_0C02;
                    }
                }
                goto Label_0C16;
            Label_0C02:
                type4 = type5;
            }
        Label_0C16:
            this.tradingSystemExecutor_0.Commission = (Commission) this.assemblyLoader_1.CreateInstance(type4);
            if (this.tradingSystemExecutor_0.Commission is ICustomSettings)
            {
                (this.tradingSystemExecutor_0.Commission as ICustomSettings).ReadSettings(this.Settings);
            }
            this.assemblyLoader_4.Path = this.AppPath;
            foreach (System.Type type6 in this.assemblyLoader_4.Types)
            {
                PosSizer sizer = (PosSizer) this.assemblyLoader_4.CreateInstance(type6);
                this.list_9.Add(sizer);
                if (sizer is ICustomSettings)
                {
                    (sizer as ICustomSettings).ReadSettings(this.Settings);
                }
            }
            this.list_9.Sort(new CompareByFriendlyName());
            TradingSystemExecutor.PosSizers = this.list_9;
            this.assemblyLoader_3.Path = this.AppPath;
            foreach (System.Type type7 in this.assemblyLoader_3.Types)
            {
                Optimizer optimizer = (Optimizer) this.assemblyLoader_3.CreateInstance(type7);
                this.list_8.Add(optimizer);
            }
            string str12 = this.settingsManager_0.Get("PositionSize", "");
            if (str12 != "")
            {
                this.tradingSystemExecutor_0.PosSize = PositionSize.Parse(str12);
            }
            string str13 = this.settingsManager_0.Get("DataRange", "");
            if (str13 != "")
            {
                this.barRange.DataRange = BarDataRange.Parse(str13);
            }
            this.tradingSystemExecutor_0.EnableSlippage = this.settingsManager_0.Get("EnableSlippage", false);
            this.tradingSystemExecutor_0.LimitOrderSlippage = this.settingsManager_0.Get("LimitOrderSlippage", false);
            this.tradingSystemExecutor_0.SlippageUnits = this.settingsManager_0.Get("SlippageUnits", (double) 0.1);
            this.tradingSystemExecutor_0.SlippageTicks = this.settingsManager_0.Get("SlippageTicks", 1);
            this.tradingSystemExecutor_0.RoundLots = this.settingsManager_0.Get("RoundLots", false);
            this.tradingSystemExecutor_0.RoundLots50 = this.settingsManager_0.Get("RoundLots50", false);
            this.tradingSystemExecutor_0.LimitDaySimulation = this.settingsManager_0.Get("LimitDaySimulation", false);
            this.tradingSystemExecutor_0.ApplyInterest = this.settingsManager_0.Get("ApplyInterest", false);
            this.tradingSystemExecutor_0.CashRate = this.settingsManager_0.Get("CashRate", (double) 1.0);
            this.tradingSystemExecutor_0.MarginRate = this.settingsManager_0.Get("MarginRate", (double) 7.0);
            this.tradingSystemExecutor_0.ApplyDividends = this.settingsManager_0.Get("ApplyDividends", false);
            this.tradingSystemExecutor_0.ReduceQtyBasedOnVolume = this.settingsManager_0.Get("ReduceQtyBasedOnVolume", false);
            this.tradingSystemExecutor_0.RedcuceQtyPct = this.settingsManager_0.Get("ReduceQtyPct", (double) 10.0);
            this.tradingSystemExecutor_0.WorstTradeSimulation = this.settingsManager_0.Get("WorstTradeSimulation", false);
            this.tradingSystemExecutor_0.BenchmarkSymbol = this.settingsManager_0.Get("BenchmarkSymbol", string.Empty);
            this.tradingSystemExecutor_0.BenchmarkBuyAndHoldON = this.settingsManager_0.Get("BenchmarkBuyAndHoldON", false);
            this.tradingSystemExecutor_0.PricingDecimalPlaces = this.settingsManager_0.Get(DecimalsManager.Instance.PricingKey, 2);
            this.tradingSystemExecutor_0.NoDecimalRoundingForLimitStopPrice = this.settingsManager_0.Get("NoDecimalRoundingForLimitStopPrice", false);
            string str14 = this.settingsManager_0.Get("StreamingProvider", "FidelityACTIVStreamingProvider");
            if (str14.CompareTo("FidelityStreamingProvider") == 0)
            {
                str14 = "FidelityACTIVStreamingProvider";
            }
            this.assemblyLoader_2.Path = this.AppPath;
            using (enumerator = this.assemblyLoader_2.Types.GetEnumerator())
            {
                System.Type type8;
                while (enumerator.MoveNext())
                {
                    type8 = enumerator.Current;
                    if (type8.Name == str14)
                    {
                        goto Label_1075;
                    }
                }
                goto Label_109D;
            Label_1075:
                this.StreamingProvider = (StreamingDataProvider) this.assemblyLoader_2.CreateInstance(type8);
            }
        Label_109D:
            flag4 = this.settingsManager_0.Get("BadTickFilter", false);
            double threshold = this.settingsManager_0.Get("BadTickThreshold", (double) 20.0);
            StreamingDataProvider.SetBadTickFilterSettings(flag4, threshold);
            this.strategyManager_1.RootPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Data";
            this.strategyManager_1.LoadStrategies();
            foreach (Strategy strategy in this.strategyManager_1.Strategies)
            {
                if (this.strategyManager_0.LookupID(strategy.ID.ToString()) == null)
                {
                    this.strategyManager_0.AddFolder("Newly Installed Strategies", false);
                    this.strategyManager_0.SaveStrategy(strategy, "Newly Installed Strategies", "");
                }
            }
            string str15 = this.DataPath + @"\StrategyMRU.txt";
            if (File.Exists(str15))
            {
                foreach (string str16 in File.ReadAllLines(str15))
                {
                    Strategy iD = this.Strategies.LookupID(str16);
                    if ((iD != null) && !this.list_3.Contains(iD))
                    {
                        this.list_3.Add(iD);
                    }
                }
            }
            this.tradeManager_0.RootPath = this.DataPath;
            this.tradeManager_0.DefaultAccountNumber = this.DefaultAccountNumber;
            try
            {
                this.tradeManager_0.LoadOrdersAndHistory();
            }
            catch
            {
            }
            foreach (Order order in this.tradeManager_0.Orders)
            {
                Guid strategyID = order.StrategyID;
                order.Strategy = this.strategyManager_0.LookupID(order.StrategyID.ToString());
            }
            WealthLab.TradeManager.DisablePortfolioSynch = this.settingsManager_0.Get(WealthLab.TradeManager.DisablePortfolioSynchKey, false);
            BarsLoader.FuturesMode = this.settingsManager_0.Get("FuturesMode", true);
            this.tradeManager_0.AlwaysExitAllSharesInPosition = this.settingsManager_0.Get("ExitFullPosition", false) && !WealthLab.TradeManager.DisablePortfolioSynch;
            this.tradeManager_0.SameBarExits = this.settingsManager_0.Get("SameBarExits", false);
            this.tradeManager_0.EnableCashThreshold = this.settingsManager_0.Get("EnableCashThreshold", false);
            this.tradeManager_0.CashThreshold = this.settingsManager_0.Get("CashThreshold", 0);
            this.tradeManager_0.EnableBuyingPowerThreshold = this.settingsManager_0.Get("EnableBuyingPowerThreshold", false);
            this.tradeManager_0.BuyingPowerThreshold = this.settingsManager_0.Get("BuyingPowerThreshold", 0);
            if (this.settingsManager_0.Get("ScheduledUpdateTime_Local", "") == "")
            {
                string str17 = this.settingsManager_0.Get("ScheduledUpdateTime", "");
                if (str17 != "")
                {
                    int hour = int.Parse(str17.Substring(0, 2));
                    DateTime local = new DateTime(0x7db, 1, 1, hour, 0, 0);
                    string str18 = TimeZoneInformation.ToLocalTime(TimeZoneInformation.ToUniversalTime("Eastern Standard Time", local), TimeZoneInformation.CurrentTimeZone.Name).Hour.ToString("D2") + ":00";
                    this.settingsManager_0.Set("ScheduledUpdateTime_Local", str18);
                    this.settingsManager_0.SaveSettings();
                }
            }
            this.timer_0.Enabled = true;
            if (File.Exists(this.string_1))
            {
                string[] strArray5 = File.ReadAllLines(this.string_1);
                this.list_5.Clear();
                if (strArray5 != null)
                {
                    foreach (string str19 in strArray5)
                    {
                        this.list_5.Add(this.method_11(str19));
                    }
                }
            }
            if (this.BrokerProvider != null)
            {
                foreach (Account account in this.BrokerProvider.Accounts)
                {
                    if (!this.list_5.Contains(account.AccountNumber))
                    {
                        this.list_5.Add(account.AccountNumber);
                    }
                }
            }
            foreach (Strategy strategy3 in this.strategyManager_0.Strategies)
            {
                if ((strategy3.AccountNumber != "") && !this.AccountNumbers.Contains(strategy3.AccountNumber))
                {
                    strategy3.AccountNumber = "";
                }
            }
            DecimalsManager.Instance.SetValues(this.settingsManager_0);
        }

        public List<string> AccountTradeTypes(string acct)
        {
            return this.AccountTradeTypes(acct, "");
        }

        public List<string> AccountTradeTypes(string acct, string action)
        {
            List<string> list = new List<string>();
            try
            {
                list = this.BrokerProvider.AccountTradeTypesAllowed(acct, action);
            }
            catch
            {
                list.Add("");
            }
            return list;
        }

        public void AddMenuItem(string text, string mainMenuItemText, string subMenuItemText, ClickMenuItem callback)
        {
            this.AddMenuItem(text, mainMenuItemText, subMenuItemText, callback, null);
        }

        public void AddMenuItem(string text, string mainMenuItemText, string subMenuItemText, ClickMenuItem callback, Image itemImage)
        {
            DynamicMenuItem item = new DynamicMenuItem(text, mainMenuItemText, subMenuItemText, callback, itemImage);
            this.list_6.Add(item);
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).AddDynamicMenuItem(item);
                }
            }
        }

        public void AddStrategyToMRU(Strategy strategy_0)
        {
            Strategy item = null;
            using (List<Strategy>.Enumerator enumerator = this.list_3.GetEnumerator())
            {
                Strategy current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (strategy_0.ID == current.ID)
                    {
                        goto Label_0037;
                    }
                }
                goto Label_0049;
            Label_0037:
                item = current;
            }
        Label_0049:
            if (item != null)
            {
                this.list_3.Remove(item);
            }
            this.list_3.Insert(0, strategy_0);
            while (this.list_3.Count > 10)
            {
                this.list_3.RemoveAt(10);
            }
            this.SaveStrategyMRU();
        }

        public void AddWorkspaceMenuItem(string workspace)
        {
            this.list_7.Add(workspace);
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).AddWorkspaceMenuItem(workspace);
                }
            }
        }

        public bool Authenticate()
        {
            if (this.IsAuthenticated)
            {
                return true;
            }
            int daysBeforeNextAuthRequired = 0;
            string str = "";
            if (this.authenticationProvider_0.Authenticate(ref daysBeforeNextAuthRequired, ref str))
            {
                this.IsAuthenticated = true;
                DateTime getCurrentDateTime = this.AuthProvider.GetCurrentDateTime;
                this.NextAuthRequired = getCurrentDateTime.Date + new TimeSpan(daysBeforeNextAuthRequired, 0, 0, 0);
                this.Settings.SaveSettings();
                if (str != "")
                {
                    MessageBox.Show(str);
                }
                return true;
            }
            if (str != null)
            {
                string str2;
                if (str.Trim() == "")
                {
                    str2 = "Log In failure.";
                }
                else
                {
                    str2 = "Log In failure: " + str;
                }
                MessageBox.Show(str2, "Log In");
            }
            return false;
        }

        public int Compare(StaticDataProvider staticDataProvider_0, StaticDataProvider staticDataProvider_1)
        {
            return staticDataProvider_0.FriendlyName.CompareTo(staticDataProvider_1.FriendlyName);
        }

        public void Connect()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).Connect();
                }
            }
        }

        public void Connect(bool reconnect)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).Connect(reconnect);
                }
            }
        }

        public void ContextSensitiveHelp(string keyword)
        {
            Help.ShowHelp(this, this.AppPath + @"\WLNetUserGuide.chm", HelpNavigator.Topic, keyword);
        }

        public void CopyListViewToClipboard(ListView listView_0)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ColumnHeader header in listView_0.Columns)
            {
                builder.Append(header.Text);
                builder.Append('\t');
            }
            builder.Append(Environment.NewLine);
            foreach (ListViewItem item in listView_0.Items)
            {
                builder.Append(item.Text);
                for (int i = 1; i < item.SubItems.Count; i++)
                {
                    builder.Append('\t');
                    builder.Append(item.SubItems[i].Text);
                }
                builder.Append(Environment.NewLine);
            }
            try
            {
                Clipboard.SetText(builder.ToString());
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public void CreateNewDataSource()
        {
            NewDataSourceForm form = new NewDataSourceForm();
            foreach (StaticDataProvider provider in this.dataSourceManager_0.Providers)
            {
                if (provider.FriendlyName == "Fidelity Investments")
                {
                    form.AddProvider(provider);
                }
            }
            List<StaticDataProvider> list = new List<StaticDataProvider>();
            foreach (StaticDataProvider provider3 in this.dataSourceManager_0.Providers)
            {
                if ((provider3.FriendlyName != "Fidelity Investments") && !provider3.InternalUseOnly)
                {
                    list.Add(provider3);
                }
            }
            list.Sort(this);
            foreach (StaticDataProvider provider2 in list)
            {
                form.AddProvider(provider2);
            }
            form.ShowDialog();
        }

        public string DefaultAccountTradeType(string account)
        {
            return this.DefaultAccountTradeType(account, "");
        }

        public string DefaultAccountTradeType(string account, string action)
        {
            List<string> list = this.AccountTradeTypes(account, action);
            if (list.Count > 0)
            {
                return list[0];
            }
            return "";
        }

        public void DeleteStrategyFromMRU(Strategy strategy_0)
        {
            if (this.list_3.Contains(strategy_0))
            {
                this.list_3.Remove(strategy_0);
            }
            this.SaveStrategyMRU();
        }

        public void Disconnect()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).Disconnect();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GetPositionAccountTradeType(AccountPosition acctPos)
        {
            return this.BrokerProvider.GetPositionAccountTradeType(acctPos);
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            PositionSize size = new PositionSize();
            this.timer_0 = new System.Windows.Forms.Timer(this.icontainer_0);
            this.helpProvider_0 = new System.Windows.Forms.HelpProvider();
            this.settingsManager_0 = new SettingsManager(this.icontainer_0);
            this.assemblyLoader_0 = new AssemblyLoader(this.icontainer_0);
            this.assemblyLoader_1 = new AssemblyLoader(this.icontainer_0);
            this.assemblyLoader_2 = new AssemblyLoader(this.icontainer_0);
            this.assemblyLoader_3 = new AssemblyLoader(this.icontainer_0);
            this.barRange = new BarDataRangeSelecter();
            this.dataSourceManager_0 = new DataSourceManager(this.icontainer_0);
            this.chartRenderer_0 = new ChartRenderer(this.icontainer_0);
            this.strategyManager_0 = new StrategyManager(this.icontainer_0);
            this.drawingObjectManager_0 = new DrawingObjectManager(this.icontainer_0);
            this.tradingSystemExecutor_0 = new TradingSystemExecutor(this.icontainer_0);
            this.strategyManager_1 = new StrategyManager(this.icontainer_0);
            this.tradeManager_0 = new WealthLab.TradeManager(this.icontainer_0);
            this.assemblyLoader_4 = new AssemblyLoader(this.icontainer_0);
            base.SuspendLayout();
            this.timer_0.Interval = 0xea60;
            this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
            this.helpProvider_0.HelpNamespace = "WLNetUserGuide.chm";
            this.settingsManager_0.FileName = "WealthLabConfig.txt";
            this.settingsManager_0.IsEncrypted = false;
            this.settingsManager_0.RootPath = null;
            this.assemblyLoader_0.BaseClass = "";
            this.assemblyLoader_0.DLLNameFilter = "";
            this.assemblyLoader_0.Interface = "IPerformanceVisualizer";
            this.assemblyLoader_0.Path = null;
            this.assemblyLoader_0.PathMask = "*.dll";
            this.assemblyLoader_1.BaseClass = "Commission";
            this.assemblyLoader_1.DLLNameFilter = "";
            this.assemblyLoader_1.Interface = null;
            this.assemblyLoader_1.Path = null;
            this.assemblyLoader_1.PathMask = "*.dll";
            this.assemblyLoader_2.BaseClass = "StreamingDataProvider";
            this.assemblyLoader_2.DLLNameFilter = "";
            this.assemblyLoader_2.Interface = null;
            this.assemblyLoader_2.Path = null;
            this.assemblyLoader_2.PathMask = "*.dll";
            this.assemblyLoader_3.BaseClass = "Optimizer";
            this.assemblyLoader_3.DLLNameFilter = "";
            this.assemblyLoader_3.Interface = null;
            this.assemblyLoader_3.Path = null;
            this.assemblyLoader_3.PathMask = "*.dll";
            this.barRange.BackColor = Color.AliceBlue;
            this.barRange.IsStreaming = false;
            this.barRange.Location = new Point(4, 0x1f);
            this.barRange.Name = "barRange";
            this.barRange.Size = new Size(0x79, 20);
            this.barRange.TabIndex = 1;
            this.dataSourceManager_0.OnDemandUpdatesEnabled = true;
            this.dataSourceManager_0.RootPath = null;
            this.dataSourceManager_0.StockSplitDataAdjusted += new EventHandler<StockSplitEventArgs>(this.method_5);
            this.chartRenderer_0.AxisFont = new Font("Tahoma", 7f);
            this.chartRenderer_0.BackgroundColor = Color.WhiteSmoke;
            this.chartRenderer_0.BarSpacing = 4;
            this.chartRenderer_0.DownBarColor = Color.Red;
            this.chartRenderer_0.DownBarVolumeColor = Color.Red;
            this.chartRenderer_0.Executor = null;
            this.chartRenderer_0.Fundamentals = null;
            this.chartRenderer_0.FundamentalsVisible = true;
            this.chartRenderer_0.GridlineColor = Color.Gainsboro;
            this.chartRenderer_0.HorizontalGridines = true;
            this.chartRenderer_0.IndicatorLabelsVisible = false;
            this.chartRenderer_0.LogScale = false;
            this.chartRenderer_0.MarginBottomColor = Color.Navy;
            this.chartRenderer_0.MarginBottomHeight = 20;
            this.chartRenderer_0.MarginRightColor = Color.Gainsboro;
            this.chartRenderer_0.MarginRightWidth = 40;
            this.chartRenderer_0.PaneSeparatorColor = Color.Black;
            this.chartRenderer_0.PaneSeparatorVisible = true;
            this.chartRenderer_0.PlotStops = false;
            this.chartRenderer_0.RightPaddingBars = 0;
            this.chartRenderer_0.ScrollOffset = 0;
            this.chartRenderer_0.TitleFont = new Font("Verdana", 8f);
            this.chartRenderer_0.TradeAnnotationsVisible = true;
            this.chartRenderer_0.TradeArrowsVisible = true;
            this.chartRenderer_0.TradeCirclesVisible = true;
            this.chartRenderer_0.UpBarColor = Color.Green;
            this.chartRenderer_0.UpBarVolumeColor = Color.Green;
            this.chartRenderer_0.VerticalGridlines = true;
            this.chartRenderer_0.VolumePaneVisible = true;
            this.strategyManager_0.RootPath = null;
            this.drawingObjectManager_0.ChartBookName = "Standard";
            this.drawingObjectManager_0.RootPath = null;
            this.tradingSystemExecutor_0.ApplyCommission = false;
            this.tradingSystemExecutor_0.ApplyDividends = false;
            this.tradingSystemExecutor_0.ApplyInterest = false;
            this.tradingSystemExecutor_0.BarsLoader = null;
            this.tradingSystemExecutor_0.BuildEquityCurves = true;
            this.tradingSystemExecutor_0.CashRate = 0.0;
            this.tradingSystemExecutor_0.EnableSlippage = false;
            this.tradingSystemExecutor_0.ExceptionEvents = false;
            this.tradingSystemExecutor_0.FundamentalsLoader = null;
            this.tradingSystemExecutor_0.IsStreaming = false;
            this.tradingSystemExecutor_0.LimitDaySimulation = false;
            this.tradingSystemExecutor_0.LimitOrderSlippage = false;
            this.tradingSystemExecutor_0.MarginRate = 0.0;
            this.tradingSystemExecutor_0.OverrideShareSize = 0.0;
            size.DollarSize = 5000.0;
            size.MarginFactor = 1.0;
            size.Mode = PosSizeMode.RawProfitDollar;
            size.OverrideShareSize = 0.0;
            size.PctSize = 10.0;
            size.RawProfitDollarSize = 5000.0;
            size.RawProfitShareSize = 100.0;
            size.RiskSize = 3.0;
            size.ShareSize = 100.0;
            size.SimuScriptName = "";
            size.StartingCapital = 100000.0;
            this.tradingSystemExecutor_0.PosSize = size;
            this.tradingSystemExecutor_0.RedcuceQtyPct = 10.0;
            this.tradingSystemExecutor_0.ReduceQtyBasedOnVolume = false;
            this.tradingSystemExecutor_0.Renderer = null;
            this.tradingSystemExecutor_0.RoundLots = false;
            this.tradingSystemExecutor_0.RoundLots50 = false;
            this.tradingSystemExecutor_0.SlippageTicks = 1;
            this.tradingSystemExecutor_0.SlippageUnits = 1.0;
            this.tradingSystemExecutor_0.StrategyName = "";
            this.tradingSystemExecutor_0.WorstTradeSimulation = false;
            this.strategyManager_1.RootPath = null;
            this.tradeManager_0.AlwaysExitAllSharesInPosition = false;
            this.tradeManager_0.AutoTradingEnabled = AutoTradingMode.Off;
            this.tradeManager_0.BuyingPowerThreshold = 0.0;
            this.tradeManager_0.CashThreshold = 0.0;
            this.tradeManager_0.EnableBuyingPowerThreshold = false;
            this.tradeManager_0.EnableCashThreshold = false;
            this.tradeManager_0.RootPath = null;
            this.tradeManager_0.SameBarExits = false;
            this.tradeManager_0.SettingsHost = null;
            this.tradeManager_0.HistoryItemAdded += new EventHandler<HistoricalTradeEventArgs>(this.method_16);
            this.tradeManager_0.PositionRemoved += new EventHandler<AccountPositionEventArgs>(this.method_22);
            this.tradeManager_0.PositionAdded += new EventHandler<AccountPositionEventArgs>(this.method_20);
            this.tradeManager_0.PositionsUpdated += new EventHandler<AccountEventArgs>(this.method_19);
            this.tradeManager_0.HistoryItemUpdated += new EventHandler<HistoricalTradeEventArgs>(this.method_15);
            this.tradeManager_0.OrderRemoved += new EventHandler<OrderEventArgs>(this.method_18);
            this.tradeManager_0.OrdersUpdated += new EventHandler<EventArgs>(this.method_8);
            this.tradeManager_0.StatusBarUpdated += new EventHandler<StringEventArgs>(this.method_24);
            this.tradeManager_0.OrderAdded += new EventHandler<OrderEventArgs>(this.method_14);
            this.tradeManager_0.AccountUpdated += new EventHandler<AccountEventArgs>(this.method_17);
            this.tradeManager_0.QuoteUpdated += new EventHandler<QuoteEventArgs>(this.method_23);
            this.tradeManager_0.OrderChanged += new EventHandler<OrderEventArgs>(this.method_13);
            this.tradeManager_0.OrderStatusUpdated += new EventHandler<OrderEventArgs>(this.method_13);
            this.tradeManager_0.PositionChanged += new EventHandler<AccountPositionEventArgs>(this.method_21);
            this.assemblyLoader_4.BaseClass = "PosSizer";
            this.assemblyLoader_4.DLLNameFilter = "";
            this.assemblyLoader_4.Interface = null;
            this.assemblyLoader_4.Path = null;
            this.assemblyLoader_4.PathMask = "*.dll";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.barRange);
            this.helpProvider_0.SetHelpKeyword(this, "introduction.htm");
            this.helpProvider_0.SetHelpNavigator(this, HelpNavigator.Topic);
            base.Name = "MainModule";
            this.helpProvider_0.SetShowHelp(this, true);
            base.Click += new EventHandler(this.MainModule_Click);
            base.ResumeLayout(false);
        }

        public Bars LoadExternalSymbol(string dataSetName, string symbol)
        {
            Bars bars = null;
            using (IEnumerator<DataSource> enumerator = this.dataSourceManager_0.DataSources.GetEnumerator())
            {
                DataSource current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Name == dataSetName)
                    {
                        goto Label_0035;
                    }
                }
                return bars;
            Label_0035:
                bars = current.Provider.RequestData(current, symbol, DateTime.MinValue, DateTime.MaxValue, 0, false);
            }
            return bars;
        }

        public Bars LoadExternalSymbol(string symbol, BarScale scale, int barInterval, bool includePartialBar)
        {
            BarDataScale scale3;
            using (IEnumerator<DataSource> enumerator = this.dataSourceManager_0.DataSources.GetEnumerator())
            {
                Bars bars3;
                while (enumerator.MoveNext())
                {
                    DataSource current = enumerator.Current;
                    if (((current.Scale == scale) && (current.BarInterval == barInterval)) && current.Symbols.Contains(symbol))
                    {
                        bars3 = current.Provider.RequestData(current, symbol, DateTime.MinValue, DateTime.MaxValue, 0, includePartialBar);
                        if ((bars3 != null) && (bars3.Count > 0))
                        {
                            goto Label_0075;
                        }
                    }
                }
                goto Label_0090;
            Label_0075:
                this.method_4(bars3);
                return bars3;
            }
        Label_0090:
            using (IEnumerator<StaticDataProvider> enumerator3 = this.dataSourceManager_0.Providers.GetEnumerator())
            {
                Bars bars4;
                while (enumerator3.MoveNext())
                {
                    StaticDataProvider provider = enumerator3.Current;
                    if (provider.SupportsDynamicUpdate(scale))
                    {
                        DataSource source3 = new DataSource(provider) {
                            Scale = scale,
                            BarInterval = barInterval
                        };
                        bars4 = provider.RequestData(source3, symbol, DateTime.MinValue, DateTime.MaxValue, 0, includePartialBar);
                        if ((bars4 != null) && (bars4.Count > 0))
                        {
                            goto Label_0103;
                        }
                    }
                }
                goto Label_0120;
            Label_0103:
                this.method_4(bars4);
                return bars4;
            }
        Label_0120:
            scale3 = new BarDataScale(scale, barInterval);
            using (IEnumerator<DataSource> enumerator2 = this.dataSourceManager_0.DataSources.GetEnumerator())
            {
                Bars bars;
                while (enumerator2.MoveNext())
                {
                    DataSource source = enumerator2.Current;
                    if (source.BarDataScale.CanConvertTo(scale3) && source.Symbols.Contains(symbol))
                    {
                        bars = source.Provider.RequestData(source, symbol, DateTime.MinValue, DateTime.MaxValue, 0, includePartialBar);
                        if (bars != null)
                        {
                            bars = BarScaleConverter.ReScale(bars, scale, barInterval);
                            if ((bars != null) && (bars.Count > 0))
                            {
                                goto Label_01A2;
                            }
                        }
                    }
                }
                return null;
            Label_01A2:
                this.method_4(bars);
                return bars;
            }
        }

        public void LoginSuccessful()
        {
            this.IsAuthenticated = true;
            if ((this.streamingDataProvider_0 != null) && this.streamingDataProvider_0.StreamingAtDisconnect)
            {
                this.streamingDataProvider_0.ConnectStreaming(this);
            }
            if (this.BrokerProvider != null)
            {
                this.BrokerProvider.Accounts.Clear();
                this.BrokerProvider.RequestUpdates();
                this.settingsManager_0.Set("LoggedIn", true);
                foreach (Account account in this.BrokerProvider.Accounts)
                {
                    for (int i = account.Positions.Count - 1; i >= 0; i--)
                    {
                        if (account.Positions[i].Quantity == 0.0)
                        {
                            account.Positions.RemoveAt(i);
                        }
                    }
                }
                if (this.BrokerProvider.Accounts.Count > 0)
                {
                    this.list_5.Clear();
                    string[] contents = new string[this.BrokerProvider.Accounts.Count];
                    int index = 0;
                    foreach (Account account2 in this.BrokerProvider.Accounts)
                    {
                        this.list_5.Add(account2.AccountNumber);
                        contents[index] = this.method_9(account2.AccountNumber);
                        index++;
                    }
                    FileNameValidator.ValidateFileName(this.string_1);
                    File.WriteAllLines(this.string_1, contents);
                }
                this.BrokerProvider.RequestOrderStatusUpdates(null);
            }
            if (OrdersAlertsForm.Instance != null)
            {
                OrdersAlertsForm.Instance.ShowLoggedInState();
            }
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).ShowLoggedInState(true);
                }
            }
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.ShowLoggedInState();
            }
        }

        public void LoginUnsuccessful()
        {
            this.IsAuthenticated = false;
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).ShowLoggedInState(false);
                }
            }
            if (OrdersAlertsForm.Instance != null)
            {
                OrdersAlertsForm.Instance.ShowLoggedInState();
            }
        }

        private void MainModule_Click(object sender, EventArgs e)
        {
            AssemblyLoader.CloseLog();
            this.SaveSettings();
        }

        private void method_0()
        {
            string str = this.dateTime_0.Ticks.ToString();
            StringBuilder builder = new StringBuilder();
            builder.Append(str.ToString());
            if (this.NicAdressesCount == 0)
            {
                builder.Append(";none");
            }
            else
            {
                this.int_0 = 0;
                while (this.int_0 < this.NicAdressesCount)
                {
                    builder.Append(";" + this.NicAddress);
                    this.int_0++;
                }
            }
            string str2 = this.method_9(builder.ToString());
            this.Settings.Set(this._newGrace, str2);
        }

        private void method_1()
        {
            string str = this.dateTime_0.Subtract(new TimeSpan(this.AuthProvider.GracePeriod, 0, 0, 0)).Ticks.ToString();
            str = this.method_9(str);
            FileNameValidator.ValidateFileName(this._authFile);
            File.WriteAllText(this._authFile, str);
        }

        internal string method_10(string string_2, string string_3)
        {
            return Cryptography.Crypt(string_2, string_3, true);
        }

        internal string method_11(string string_2)
        {
            return Cryptography.Crypt(string_2, this._password, false);
        }

        internal string method_12(string string_2, string string_3)
        {
            return Cryptography.Crypt(string_2, string_3, false);
        }

        private void method_13(object sender, OrderEventArgs e)
        {
            if (OrdersAlertsForm.Instance != null)
            {
                OrdersAlertsForm.Instance.OrderStatusUpdated(e.Order);
            }
        }

        private void method_14(object sender, OrderEventArgs e)
        {
            if ((OrdersAlertsForm.Instance == null) && this.settingsManager_0.Get("AutoOpenOrders", true))
            {
                this.FirstMainForm.OpenOrderManager();
            }
            else if (OrdersAlertsForm.Instance != null)
            {
                OrdersAlertsForm.Instance.OrderAdded(e.Order);
            }
            if ((OrdersAlertsForm.Instance != null) && this.settingsManager_0.Get("SwitchToAccount", true))
            {
                OrdersAlertsForm.Instance.SwitchToAccount(e.Order.Account);
            }
        }

        private void method_15(object sender, HistoricalTradeEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.TradeHistoryItemUpdated(e.HistoricalTrade);
            }
        }

        private void method_16(object sender, HistoricalTradeEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.TradeHistoryItemAdded(e.HistoricalTrade);
            }
        }

        private void method_17(object sender, AccountEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.AccountUpdated(e.Account);
            }
        }

        private void method_18(object sender, OrderEventArgs e)
        {
            if (OrdersAlertsForm.Instance != null)
            {
                OrdersAlertsForm.Instance.OrderRemoved(e.Order);
            }
        }

        private void method_19(object sender, AccountEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.PositionsUpdated(e.Account);
            }
        }

        private void method_2(object object_0)
        {
            ((SoundPlayer) object_0).PlaySync();
            this.bool_3 = false;
        }

        private void method_20(object sender, AccountPositionEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.PositionAdded(e.AccountPosition);
            }
        }

        private void method_21(object sender, AccountPositionEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.PositionChanged(e.AccountPosition);
            }
        }

        private void method_22(object sender, AccountPositionEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.PositionRemoved(e.AccountPosition);
            }
        }

        private void method_23(object sender, QuoteEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).UpdateTradeTicketQuote(e.Quote);
                }
            }
        }

        private void method_24(object sender, StringEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).PrintStatus(e.Message);
                }
            }
        }

        internal void method_25(Alert alert_0, bool bool_5)
        {
            int num6;
            string subject = string.Empty;
            if (alert_0.OrderType != OrderType.Market)
            {
                subject = alert_0.AlertType.ToString() + " " + alert_0.Shares.ToString() + " " + alert_0.Symbol + " @" + alert_0.OrderType.ToString() + " " + alert_0.Price.ToString();
            }
            else
            {
                subject = alert_0.AlertType.ToString() + " " + alert_0.Shares.ToString() + " " + alert_0.Symbol + " @" + alert_0.OrderType.ToString();
            }
            StringBuilder builder = new StringBuilder();
            if (Application.ProductName == "WealthLabPro")
            {
                builder.Append("Wealth-Lab Pro Trade Alert");
            }
            else
            {
                builder.Append("Wealth-Lab Dev Trade Alert");
            }
            builder.AppendLine();
            builder.AppendLine();
            builder.Append(alert_0.AlertType.ToString());
            builder.Append(" " + alert_0.Shares.ToString());
            if (alert_0.OrderType != OrderType.Market)
            {
                builder.Append(string.Concat(new object[] { " Shares of ", alert_0.Symbol, " at ", alert_0.OrderType, " Price ", alert_0.Price.ToString() }));
            }
            else
            {
                builder.Append(string.Concat(new object[] { " Shares of ", alert_0.Symbol, " at ", alert_0.OrderType }));
            }
            string host = Instance.Settings.Get("EmailSMTPHost", string.Empty);
            string s = Instance.Settings.Get("EmailSMTPPort", string.Empty);
            string address = Instance.Settings.Get("EmailAddresses", string.Empty).Replace("~!", "\r\n");
            string credUser = Instance.Settings.Get("EmailUserID", "");
            string credPass = Instance.method_11(Instance.Settings.Get("EmailPassword", string.Empty));
            bool sSL = Instance.Settings.Get("EmailSSL", false);
            int.TryParse(s, out num6);
            new WLPEmail(host, num6, sSL, credUser, credPass, address, subject, builder.ToString()).Enqueue(Application.ProductName);
        }

        internal void method_26(List<Alert> list_10)
        {
            foreach (Alert alert in list_10)
            {
                this.method_25(alert, true);
            }
        }

        internal void method_27(string string_2, int int_2, bool bool_5, string string_3, string string_4, string string_5)
        {
            string_5 = string_5.Replace(" ", string.Empty);
            if (Application.ProductName == "WealthLabPro")
            {
                new WLPEmail(string_2, int_2, bool_5, string_3, string_4, string_5, "Test Mail Wealth Lab Pro", "This is a test mail from Wealth Lab Pro").Enqueue(Application.ProductName);
            }
            else
            {
                new WLPEmail(string_2, int_2, bool_5, string_3, string_4, string_5, "Test Mail Wealth Lab Dev", "This is a test mail from Wealth Lab Dev").Enqueue(Application.ProductName);
            }
        }

        private System.Type method_3()
        {
            return typeof(FidelityFlatRate);
        }

        private void method_4(Bars bars_0)
        {
            using (IEnumerator<SymbolInfo> enumerator = BarsLoader.SymbolInfo.GetEnumerator())
            {
                SymbolInfo current;
                SymbolInfo info2;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Symbol == bars_0.Symbol)
                    {
                        goto Label_0032;
                    }
                }
                return;
            Label_0032:
                info2 = new SymbolInfo(current);
                bars_0.SymbolInfo = info2;
            }
        }

        private void method_5(object sender, StockSplitEventArgs e)
        {
            this.drawingObjectManager_0.SplitAdjustDrawingObjects(e.Symbol, e.SplitFactor, e.ExDate);
        }

        private void method_6()
        {
            this.settingsManager_0.Set("BarSpacing", this.chartRenderer_0.BarSpacing);
            this.settingsManager_0.Set("ChartBackgroundColor", this.chartRenderer_0.BackgroundColor);
            this.settingsManager_0.Set("ChartUpBarColor", this.chartRenderer_0.UpBarColor);
            this.settingsManager_0.Set("ChartDownBarColor", this.chartRenderer_0.DownBarColor);
            this.settingsManager_0.Set("ChartUpVolumeColor", this.chartRenderer_0.UpBarVolumeColor);
            this.settingsManager_0.Set("ChartDownVolumeColor", this.chartRenderer_0.DownBarVolumeColor);
            this.settingsManager_0.Set("ChartGridlineColor", this.chartRenderer_0.GridlineColor);
            this.settingsManager_0.Set("ChartRightMarginColor", this.chartRenderer_0.MarginRightColor);
            this.settingsManager_0.Set("ChartBottomMarginColor", this.chartRenderer_0.MarginBottomColor);
            this.settingsManager_0.Set("ChartPaneSeparatorColor", this.chartRenderer_0.PaneSeparatorColor);
            this.settingsManager_0.Set("HorizontalGridlines", this.chartRenderer_0.HorizontalGridines);
            this.settingsManager_0.Set("VerticalGridlines", this.chartRenderer_0.VerticalGridlines);
            this.settingsManager_0.Set("PaneSeparators", this.chartRenderer_0.PaneSeparatorVisible);
            this.settingsManager_0.Set("ChartFont", this.chartRenderer_0.AxisFont);
            this.settingsManager_0.Set("FundamentalsCharted", this.chartRenderer_0.FundamentalGlyphs);
            this.settingsManager_0.Set("LogScale", this.chartRenderer_0.LogScale);
            this.settingsManager_0.Set("TitleFont", this.chartRenderer_0.TitleFont);
        }

        private void method_7(string string_2, string string_3)
        {
            if (Directory.Exists(string_2))
            {
                if (!Directory.Exists(string_3))
                {
                    Directory.CreateDirectory(string_3);
                }
                foreach (string str in Directory.GetDirectories(string_2))
                {
                    string[] strArray2 = str.Split(new char[] { '\\' });
                    string str2 = strArray2[strArray2.Length - 1];
                    string str3 = string_2 + @"\" + str2;
                    string str4 = string_3 + @"\" + str2;
                    this.method_7(str3, str4);
                }
                foreach (string str5 in Directory.GetFiles(string_2))
                {
                    File.Copy(str5, string_3 + @"\" + Path.GetFileName(str5));
                }
            }
        }

        private void method_8(object sender, EventArgs e)
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is MainForm)
                    {
                        (form as MainForm).OrdersUpdated();
                    }
                }
                if (OrdersAlertsForm.Instance != null)
                {
                    OrdersAlertsForm.Instance.UpdateStatusBar();
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        internal string method_9(string string_2)
        {
            return Cryptography.Crypt(string_2, this._password, true);
        }

        public bool NavigateToThirdPartySite(string string_2)
        {
            bool flag2;
            bool flag = true;
            if (string.IsNullOrEmpty(this.AuthProvider.ThirdPartySiteWarning))
            {
                return flag;
            }
            try
            {
                Uri uri = new Uri(string_2);
                string str = "ThirdPartySite:" + uri.Host.ToUpper();
                if (!this.Settings.Get(str, false))
                {
                    DontShowAgainForm form = new DontShowAgainForm("Third Party Site Warning", this.AuthProvider.ThirdPartySiteWarning + " (Site: " + uri.Host + ")") {
                        CancelButtonVisible = true
                    };
                    flag = form.ShowDialog() == DialogResult.OK;
                    if (form.DontShowAgain)
                    {
                        Instance.Settings.Set(str, true);
                    }
                }
                return flag;
            }
            catch
            {
                flag2 = true;
            }
            return flag2;
        }

        public static void NotImplemented()
        {
            MessageBox.Show("Not implemented yet, try back soon!");
        }

        public void PlaySound(string soundFile)
        {
            if (!this.bool_3 && File.Exists(soundFile))
            {
                StreamReader reader = new StreamReader(soundFile);
                this.PlaySound(reader.BaseStream, false);
            }
        }

        public void PlaySound(Stream stream, bool prioritize)
        {
            if (!this.bool_3 || prioritize)
            {
                SoundPlayer parameter = new SoundPlayer(stream);
                this.bool_3 = true;
                new Thread(new ParameterizedThreadStart(this.method_2)).Start(parameter);
            }
        }

        public void PlaySoundPrioritized(string soundFile)
        {
            if (File.Exists(soundFile))
            {
                StreamReader reader = new StreamReader(soundFile);
                this.PlaySound(reader.BaseStream, true);
            }
        }

        public void SaveSettings()
        {
            lock (this)
            {
                bool flag2 = true;
                if (MainForm.TemperFi > 0)
                {
                    flag2 = false;
                }
                this.method_6();
                this.settingsManager_0.Set("PositionSize", this.tradingSystemExecutor_0.PosSize.ToString());
                this.settingsManager_0.Set("DataRange", this.barRange.DataRange.ToString());
                if (flag2)
                {
                    this.settingsManager_0.Set("EnableSlippage", this.tradingSystemExecutor_0.EnableSlippage);
                    this.settingsManager_0.Set("LimitOrderSlippage", this.tradingSystemExecutor_0.LimitOrderSlippage);
                    this.settingsManager_0.Set("SlippageUnits", this.tradingSystemExecutor_0.SlippageUnits);
                    this.settingsManager_0.Set("SlippageTicks", this.tradingSystemExecutor_0.SlippageTicks);
                }
                this.settingsManager_0.Set("RoundLots", this.tradingSystemExecutor_0.RoundLots);
                this.settingsManager_0.Set("RoundLots50", this.tradingSystemExecutor_0.RoundLots50);
                this.settingsManager_0.Set("ApplyCommissions", this.tradingSystemExecutor_0.ApplyCommission);
                this.settingsManager_0.Set("LimitDaySimulation", this.tradingSystemExecutor_0.LimitDaySimulation);
                this.settingsManager_0.Set("ApplyInterest", this.tradingSystemExecutor_0.ApplyInterest);
                this.settingsManager_0.Set("CashRate", this.tradingSystemExecutor_0.CashRate);
                this.settingsManager_0.Set("MarginRate", this.tradingSystemExecutor_0.MarginRate);
                this.settingsManager_0.Set("ApplyDividends", this.tradingSystemExecutor_0.ApplyDividends);
                if (this.streamingDataProvider_0 != null)
                {
                    this.settingsManager_0.Set("StreamingProvider", this.streamingDataProvider_0.GetType().Name);
                }
                this.settingsManager_0.Set("FuturesMode", BarsLoader.FuturesMode);
                this.settingsManager_0.Set("ReduceQtyBasedOnVolume", this.tradingSystemExecutor_0.ReduceQtyBasedOnVolume);
                this.settingsManager_0.Set("ReduceQtyPct", this.tradingSystemExecutor_0.RedcuceQtyPct);
                this.settingsManager_0.Set("WorstTradeSimulation", this.tradingSystemExecutor_0.WorstTradeSimulation);
                this.settingsManager_0.Set("BenchmarkSymbol", this.tradingSystemExecutor_0.BenchmarkSymbol);
                this.settingsManager_0.Set("BenchmarkBuyAndHoldON", this.tradingSystemExecutor_0.BenchmarkBuyAndHoldON);
                this.settingsManager_0.Set(DecimalsManager.Instance.PricingKey, this.tradingSystemExecutor_0.PricingDecimalPlaces);
                this.settingsManager_0.Set("NoDecimalRoundingForLimitStopPrice", this.tradingSystemExecutor_0.NoDecimalRoundingForLimitStopPrice);
                this.settingsManager_0.Set("SameBarExits", this.tradeManager_0.SameBarExits);
                this.settingsManager_0.Set("EnableCashThreshold", this.tradeManager_0.EnableCashThreshold);
                this.settingsManager_0.Set("CashThreshold", this.tradeManager_0.CashThreshold);
                this.settingsManager_0.Set("EnableBuyingPowerThreshold", this.tradeManager_0.EnableBuyingPowerThreshold);
                this.settingsManager_0.Set("BuyingPowerThreshold", this.tradeManager_0.BuyingPowerThreshold);
                this.settingsManager_0.SaveSettings();
                bool badTickFilter = this.settingsManager_0.Get("BadTickFilter", false);
                double threshold = this.settingsManager_0.Get("BadTickThreshold", (double) 20.0);
                StreamingDataProvider.SetBadTickFilterSettings(badTickFilter, threshold);
                this.settingsManager_0.Set("StrategyNetworkPathCount", this.StrategyNetworkPaths.Count);
                for (int i = 0; i < this.StrategyNetworkPaths.Count; i++)
                {
                    this.settingsManager_0.Set("StrategyNetworkPath" + i, this.StrategyNetworkPaths[i]);
                }
            }
        }

        public void SaveStrategyMRU()
        {
            string fileName = this.DataPath + @"\StrategyMRU.txt";
            string[] contents = new string[this.list_3.Count];
            for (int i = 0; i < this.list_3.Count; i++)
            {
                contents[i] = this.list_3[i].ID.ToString();
            }
            FileNameValidator.ValidateFileName(fileName);
            File.WriteAllLines(fileName, contents);
            if (HomeForm.Instance != null)
            {
                HomeForm.Instance.LoadStrategyMRU();
            }
        }

        public void SetOnDemand(bool onDemandOn)
        {
            this.dataSourceManager_0.OnDemandUpdatesEnabled = onDemandOn;
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).SetOnDemand(this.DataSources.OnDemandUpdatesEnabled);
                }
            }
            if (DataManagerForm.Instance != null)
            {
                DataManagerForm.Instance.SetOnDemand(onDemandOn);
            }
            this.settingsManager_0.Set("OnDemandDataEnabled", onDemandOn);
        }

        public bool ShouldOrderBePlaced(string accountNumber)
        {
            if (accountNumber.StartsWith("PaperAccount"))
            {
                return (this.AutoTradingEnabled == AutoTradingMode.Paper);
            }
            return (this.AutoTradingEnabled == AutoTradingMode.Live);
        }

        public bool ShouldOrderBePlaced(Alert alert)
        {
            if (alert.Account == "")
            {
                return false;
            }
            if (alert.Account.StartsWith("PaperAccount"))
            {
                return (this.AutoTradingEnabled == AutoTradingMode.Paper);
            }
            return (this.AutoTradingEnabled == AutoTradingMode.Live);
        }

        public void StatusUpdate(ConnStatus status, int StatusCode, string Message)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                Form form = Application.OpenForms[i];
                if (form is MainForm)
                {
                    (form as MainForm).StatusUpdate(status, StatusCode, Message);
                }
            }
        }

        public void StreamingSymbolsUpdated(string symbolString)
        {
            this.settingsManager_0.Set("StreamingSymbols", symbolString);
            string[] symbols = symbolString.Split(new char[] { ',' });
            this.FirstMainForm.UpdateStreamingSymbols(symbols);
        }

        private void timer_0_Tick(object sender, EventArgs e)
        {
            if (this.Settings.Get("ScheduledDataUpdates", false))
            {
                if (this.Settings.Get("LastScheduledUpdate", DateTime.MinValue) >= DateTime.Now.Date)
                {
                    this.bool_4 = false;
                }
                else
                {
                    int hour = int.Parse(this.Settings.Get("ScheduledUpdateTime_Local", "07:00").Split(new char[] { ':' })[0]);
                    DateTime now = DateTime.Now;
                    int num2 = this.settingsManager_0.Get("RandomMinute", -1);
                    if (num2 == -1)
                    {
                        num2 = new Random().Next(60);
                        this.settingsManager_0.Set("RandomMinute", num2);
                    }
                    DateTime time4 = DateTime.Now;
                    DateTime time5 = new DateTime(now.Year, now.Month, now.Day, hour, num2, 0);
                    if (time4 >= time5)
                    {
                        this.Settings.Set("LastScheduledUpdate", time4.Date);
                        this.Settings.SaveSettings();
                        if (!this.bool_4)
                        {
                            DataManagerForm instance = DataManagerForm.Instance;
                            if (instance == null)
                            {
                                instance = this.FirstMainForm.CreateDataManager();
                            }
                            instance.UpdateProviders();
                        }
                    }
                    this.bool_4 = false;
                }
            }
        }

        public bool UnAuthenticate()
        {
            try
            {
                if (this.IsAuthenticated)
                {
                    this.IsAuthenticated = false;
                    this.authenticationProvider_0.UnAuthenticate();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void UpdateChartColorsAndStyle()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).UpdateChartColorsAndStyle();
                }
            }
        }

        public void UpdateDaysBeforeNextLogin(int days)
        {
            this.NextAuthRequired = DateTime.Now.Date + new TimeSpan(days, 0, 0, 0);
            this.Settings.SaveSettings();
        }

        public bool WindowWithTitleExists(string title, Form exclude)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm as MainForm == null)
                {
                    continue;
                }
                Form[] mdiChildren = openForm.MdiChildren;
                int num = 0;
                while (num < (int)mdiChildren.Length)
                {
                    Form form = mdiChildren[num];
                    if (!(form.Text == title) || form == exclude)
                    {
                        num++;
                    }
                    else
                    {
                        bool flag = true;
                        return flag;
                    }
                }
            }
            return false;
        }

        private string _authFile
        {
            get
            {
                return (Application.LocalUserAppDataPath + @"\ucc.txt");
            }
        }

        private string _grace
        {
            get
            {
                return "NBR";
            }
        }

        private string _newGrace
        {
            get
            {
                return "Marwa";
            }
        }

        private string _password
        {
            get
            {
                return "nkvnbeia84dfkjcbg;krsadxhf=8ik5khutdvlkjlfdhsfliud";
            }
        }

        public IList<string> AccountNumbers
        {
            get
            {
                return this.list_5;
            }
        }

        public string AppPath
        {
            get
            {
                return Path.GetDirectoryName(Application.ExecutablePath);
            }
        }

        public AuthenticationProvider AuthProvider
        {
            get
            {
                return this.authenticationProvider_0;
            }
        }

        public AutoTradingMode AutoTradingEnabled
        {
            get
            {
                return this.TradeManager.AutoTradingEnabled;
            }
            set
            {
                this.TradeManager.AutoTradingEnabled = value;
                foreach (Form form in Application.OpenForms)
                {
                    if (form is MainForm)
                    {
                        (form as MainForm).ShowAutoTradingState(value);
                    }
                }
            }
        }

        public WealthLab.BrokerProvider BrokerProvider
        {
            get
            {
                return this.brokerProvider_0;
            }
        }

        public string DataPath
        {
            get
            {
                string path = Application.UserAppDataPath + @"\Data";
                if (this.authenticationProvider_0 != null)
                {
                    path = path.Replace("WealthLabPro", this.authenticationProvider_0.UserDataPathToken);
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public BarDataRange DataRange
        {
            get
            {
                return this.barRange.DataRange;
            }
            set
            {
                this.barRange.DataRange = value;
            }
        }

        public DataSourceManager DataSources
        {
            get
            {
                return this.dataSourceManager_0;
            }
        }

        public string DefaultAccountNumber
        {
            get
            {
                string str = this.Settings.Get("DefaultAccount", "");
                str = this.method_11(str);
                if (str == null)
                {
                    str = "";
                }
                if (!(str == ""))
                {
                    return str;
                }
                if ((this.BrokerProvider != null) && (this.BrokerProvider.Accounts.Count > 0))
                {
                    return this.BrokerProvider.Accounts[0].AccountNumber;
                }
                if (this.AccountNumbers.Count > 0)
                {
                    return this.AccountNumbers[0];
                }
                return "";
            }
            set
            {
                if (value != "")
                {
                    this.Settings.Set("DefaultAccount", this.method_9(value));
                    this.Settings.SaveSettings();
                    this.TradeManager.DefaultAccountNumber = value;
                }
            }
        }

        public List<DynamicMenuItem> DynamicMenuitems
        {
            get
            {
                return this.list_6;
            }
        }

        public bool EmailSettingsAvailable
        {
            get
            {
                return !string.IsNullOrEmpty(this.Settings.Get("EmailSMTPHost", string.Empty));
            }
        }

        public TradingSystemExecutor Executor
        {
            get
            {
                return this.tradingSystemExecutor_0;
            }
        }

        public MainForm FirstMainForm
        {
            get
            {
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm as MainForm == null)
                    {
                        continue;
                    }
                    MainForm mainForm = openForm as MainForm;
                    if (!mainForm.IsFirstMainForm)
                    {
                        continue;
                    }
                    MainForm mainForm1 = mainForm;
                    return mainForm1;
                }
                return null;
            }
        }

        public DateTime GracePeriodEndDate
        {
            get
            {
                return this.NextAuthRequired;
            }
        }

        public System.Windows.Forms.HelpProvider HelpProvider
        {
            get
            {
                return this.helpProvider_0;
            }
        }

        public bool InGracePeriod
        {
            get
            {
                return this.settingsManager_0.Get("LoggedIn", false);
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return this.bool_1;
            }
            internal set
            {
                this.bool_1 = value;
            }
        }

        private DateTime NextAuthRequired
        {
            get
            {
                if (this.dateTime_0 == DateTime.MinValue)
                {
                    if (!this.Settings.ContainsKey(this._newGrace))
                    {
                        if (File.Exists(this._authFile))
                        {
                            string str = File.ReadAllText(this._authFile);
                            str = this.method_11(str);
                            DateTime time2 = new DateTime(long.Parse(str));
                            this.dateTime_0 = time2 + new TimeSpan(this.AuthProvider.GracePeriod, 0, 0, 0);
                            this.method_0();
                        }
                        else
                        {
                            DateTime now = DateTime.Now;
                            try
                            {
                                now = this.AuthProvider.GetCurrentDateTime;
                            }
                            catch
                            {
                                Application.Exit();
                            }
                            this.dateTime_0 = now + new TimeSpan(this.authenticationProvider_0.GracePeriod, 0, 0, 0);
                        }
                    }
                    else
                    {
                        bool flag = false;
                        string str2 = this.Settings.Get(this._newGrace, "");
                        this.int_0 = 0;
                        while (this.int_0 < this.NicAdressesCount)
                        {
                            try
                            {
                                long ticks = long.Parse(this.method_12(str2, this.NicAddress));
                                this.dateTime_0 = new DateTime(ticks);
                                flag = true;
                                break;
                            }
                            catch
                            {
                                this.int_0++;
                                continue;
                            }
                        }
                        if (!flag)
                        {
                            try
                            {
                                string str4 = this.method_11(str2);
                                if (!string.IsNullOrEmpty(str4))
                                {
                                    long num2;
                                    string[] strArray = str4.Split(new char[] { ';' });
                                    if (long.TryParse(strArray[0], out num2) && (strArray.Length > 1))
                                    {
                                        if (((strArray.Length == 1) && (strArray[1] == "none")) && (this.NicAdressesCount == 0))
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            for (int i = 1; i < strArray.Length; i++)
                                            {
                                                this.int_0 = 0;
                                                while (this.int_0 < this.NicAdressesCount)
                                                {
                                                    if (this.NicAddress == strArray[i])
                                                    {
                                                        goto Label_0149;
                                                    }
                                                    this.int_0++;
                                                }
                                                goto Label_014C;
                                            Label_0149:
                                                flag = true;
                                            Label_014C:
                                                if (flag)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        if (flag)
                                        {
                                            this.dateTime_0 = new DateTime(num2);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (this.AuthProvider.NextAuthRequired < this.dateTime_0)
                    {
                        this.dateTime_0 = this.AuthProvider.NextAuthRequired;
                        this.method_0();
                        this.method_1();
                    }
                }
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
                this.method_0();
                this.method_1();
            }
        }

        private string NicAddress
        {
            get
            {
                string str;
                try
                {
                    int num = 0;
                    NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                    int num1 = 0;
                    while (num1 < (int)allNetworkInterfaces.Length)
                    {
                        NetworkInterface networkInterface = allNetworkInterfaces[num1];
                        if (num != this.int_0)
                        {
                            num++;
                            num1++;
                        }
                        else
                        {
                            str = networkInterface.GetPhysicalAddress().ToString();
                            return str;
                        }
                    }
                    str = "None";
                }
                catch
                {
                    str = "None";
                }
                return str;
            }
        }

        private int NicAdressesCount
        {
            get
            {
                if (this.int_1 == 0)
                {
                    this.int_1 = NetworkInterface.GetAllNetworkInterfaces().Length;
                }
                return this.int_1;
            }
        }

        public List<Optimizer> Optimizers
        {
            get
            {
                return this.list_8;
            }
        }

        public List<PosSizer> PosSizers
        {
            get
            {
                return this.list_9;
            }
        }

        public ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer_0;
            }
        }

        public SettingsManager Settings
        {
            get
            {
                return this.settingsManager_0;
            }
        }

        public StrategyManager Strategies
        {
            get
            {
                return this.strategyManager_0;
            }
        }

        public List<Strategy> StrategyMRU
        {
            get
            {
                return this.list_3;
            }
        }

        public List<string> StrategyNetworkPaths
        {
            get
            {
                return this.list_0;
            }
        }

        public string StrategyTemplateCode
        {
            get
            {
                if (!this.bool_0)
                {
                    string path = this.DataPath + @"\StrategyTemplate.txt";
                    this.bool_0 = true;
                    if (File.Exists(path))
                    {
                        this.string_0 = File.ReadAllText(path);
                    }
                    else
                    {
                        this.string_0 = Resources.StrategyTemplate;
                    }
                }
                return this.string_0;
            }
            set
            {
                string fileName = this.DataPath + @"\StrategyTemplate.txt";
                this.string_0 = value;
                this.bool_0 = true;
                FileNameValidator.ValidateFileName(fileName);
                File.WriteAllText(fileName, this.string_0);
            }
        }

        public StreamingDataProvider StreamingProvider
        {
            get
            {
                return this.streamingDataProvider_0;
            }
            internal set
            {
                if (((this.streamingDataProvider_0 == null) || (value == null)) || (this.streamingDataProvider_0.FriendlyName != value.FriendlyName))
                {
                    if (this.streamingDataProvider_0 != null)
                    {
                        this.streamingDataProvider_0.DisconnectStreaming();
                    }
                    this.streamingDataProvider_0 = value;
                    if (this.streamingDataProvider_0 != null)
                    {
                        this.streamingDataProvider_0.Initialize(this.DataSources);
                    }
                }
            }
        }

        public bool StreamingWasClicked
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

        public WealthLab.TradeManager TradeManager
        {
            get
            {
                return this.tradeManager_0;
            }
        }

        public ICollection<IPerformanceVisualizer> Visualizers
        {
            get
            {
                return this.list_1;
            }
        }

        public ICollection<IPerformanceVisualizer> VisualizersChecked
        {
            get
            {
                return this.list_2;
            }
        }

        public List<string> WorkspaceMenuItems
        {
            get
            {
                return this.list_7;
            }
        }
    }
}

