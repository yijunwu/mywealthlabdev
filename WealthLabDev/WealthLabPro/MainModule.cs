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
        private AssemblyLoader assemblyLoader_PerformanceVisualizer;   ///WYJ fix, original name: assemblyLoader_0
        private AssemblyLoader assemblyLoader_Commission;   ///WYJ fix, original name: assemblyLoader_1
        private AssemblyLoader assemblyLoader_StreamingDataProvider;   ///WYJ fix, original name: assemblyLoader_2
        internal AssemblyLoader assemblyLoader_Optimizer;   ///WYJ fix, original name: assemblyLoader_3
        private AssemblyLoader assemblyLoader_PosSizer;   ///WYJ fix, original name: assemblyLoader_4
        private AuthenticationProvider authenticationProvider;
        private BarDataRangeSelecter barRange;
        private bool strategyTemplateCodeInited;
        private bool isAuthenticated;
        private bool streamingWasClicked;
        private bool soundPlaying; ///WYJ fix, original signature: bool_3
        private bool bool_4;
        private WealthLab.BrokerProvider brokerProvider;
        private ChartRenderer chartRenderer;
        private DataSourceManager dataSourceManager;
        private DateTime nextAuthRequired;   ///WYJ fix, original name: dateTime_0
        private DrawingObjectManager drawingObjectManager_0;
        private System.Windows.Forms.HelpProvider helpProvider;
        private IContainer components;
        private static readonly ILog ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static MainModule Instance = new MainModule();
        private int indexNicAddress;  ///WYJ fix, original name: int_0
        private int nicAdressesCount;
        private List<string> strategyNetworkPaths;
        private List<IPerformanceVisualizer> visualizers;
        private List<IPerformanceVisualizer> visualizersChecked;
        private List<Strategy> strategyMRU;   ///WYJ fix, original name: list_3
        private List<Account> list_4;   ///WYJ note, useless list
        private List<string> accountNumbers;   ///WYJ fix, original name: list_5
        private List<DynamicMenuItem> dynamicMenuitems;
        private List<string> workspaceMenuItems;
        private List<Optimizer> optimizers;
        private List<PosSizer> posSizers;
        private SettingsManager settingsManager;
        private StrategyManager strategyManager_0;
        private StrategyManager strategyManager_1;   ///WYJ note, only for temp use, in MainModule()
        private StreamingDataProvider streamingDataProvider;
        private string strategyTemplateCode;
        private string accountFile;   ///WYJ fix, original name: string_1
        private System.Windows.Forms.Timer timer_0;
        private WealthLab.TradeManager tradeManager;
        private TradingSystemExecutor tradingSystemExecutor;

        public MainModule()
        {
            List<System.Type>.Enumerator enumerator;
            List<IPerformanceVisualizer>.Enumerator enumerator2;
            this.strategyNetworkPaths = new List<string>();
            this.visualizers = new List<IPerformanceVisualizer>();
            this.visualizersChecked = new List<IPerformanceVisualizer>();
            this.strategyTemplateCode = "";
            this.strategyMRU = new List<Strategy>();
            this.list_4 = new List<Account>();
            this.accountFile = "";
            this.accountNumbers = new List<string>();
            this.dynamicMenuitems = new List<DynamicMenuItem>();
            this.workspaceMenuItems = new List<string>();
            this.bool_4 = true;
            this.optimizers = new List<Optimizer>();
            this.posSizers = new List<PosSizer>();
            this.nextAuthRequired = DateTime.MinValue;
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
                this.authenticationProvider = (AuthenticationProvider) loader.CreateInstance(loader.Types[0]);
                this.authenticationProvider.PreInitialize();
            }
            this.accountFile = this.DataPath + @"\Accounts.txt";
            this.InitializeComponent();
            CustomIndexManager.Initialize(this.DataPath, this.dataSourceManager);
            if (Application.ProductName == "WealthLabPro")
            {
                this.chartRenderer.FundamentalGlyphs = "split;dividend;earnings per share;";
            }
            if (base.DesignMode)
            {
                return;
            }
            Instance = this;
            Application.ApplicationExit += new EventHandler(this.MainModule_Click);
            if (!Directory.Exists(this.DataPath + @"\Strategies"))
            {
                this.copyAllFiles(Path.GetDirectoryName(Application.ExecutablePath) + @"\Data", this.DataPath);
            }
            string path = this.DataPath + @"\Workspaces";
            string str3 = Path.GetDirectoryName(Application.ExecutablePath) + @"\Data\Workspaces";
            if (Directory.Exists(str3))
            {
                if (Directory.Exists(path))
                {
                    foreach (string srcFile in Directory.GetFiles(str3))
                    {
                        string destFile = path + @"\" + Path.GetFileName(srcFile);
                        if (!File.Exists(destFile))
                        {
                            File.Copy(srcFile, destFile);
                        }
                    }
                }
                else
                {
                    this.copyAllFiles(str3, path);
                }
            }
            this.authenticationProvider.Initialize(this.DataSources, this);
            AssemblyLoader loader2 = new AssemblyLoader {
                BaseClass = "MenuItemHook",
                Path = this.AppPath
            };
            foreach (System.Type type in loader2.Types)
            {
                ((MenuItemHook) loader2.CreateInstance(type)).AddMenuItems(this);
            }
            this.dataSourceManager.AuthProvider = this.authenticationProvider;
            this.dataSourceManager.RootPath = this.DataPath;
            this.drawingObjectManager_0.RootPath = this.DataPath;
            this.settingsManager.RootPath = this.DataPath;
            this.strategyManager_0.RootPath = this.DataPath;
            BarsLoader.RootPath = this.DataPath;
            BarsLoader.LoadSymbolInfo();
            int num = this.settingsManager.Get("StrategyNetworkPathCount", 0);
            for (int i = 0; i < num; i++)
            {
                string item = this.settingsManager.Get("StrategyNetworkPath" + i, "");
                this.StrategyNetworkPaths.Add(item);
                this.strategyManager_0.LoadStrategiesFromNetworkPath(item);
            }
            MarketHours.RootPath = this.AppPath + @"\Data";
            this.dataSourceManager.SettingsHost = this.settingsManager;
            this.tradeManager.SettingsHost = this.settingsManager;
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
                            ///goto  Label_04C9;  ///WYJ fix, simplify the flow
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        this.brokerProvider = (WealthLab.BrokerProvider) loader3.CreateInstance(type2);
                        this.brokerProvider.Initialize(this.TradeManager, this.authenticationProvider);
                        if (this.brokerProvider is ICustomSettings)
                        {
                            (this.brokerProvider as ICustomSettings).ReadSettings(this.Settings);
                        }
                        this.TradeManager.BrokerProvider = this.brokerProvider;
                    }
                }
            }
            bool authRequiredNow = (DateTime.Now > this.NextAuthRequired) || this.AuthProvider.ForceAuthentication;
            bool chooseToAuthNow = false;
            if (!authRequiredNow)
            {
                chooseToAuthNow = (this.NextAuthRequired - DateTime.Now.Date) <= new TimeSpan(5, 0, 0, 0);
                TimeSpan span = (TimeSpan) (this.NextAuthRequired - DateTime.Now.Date);
                int days = span.Days;
                if (chooseToAuthNow)
                {
                    if (this.AuthProvider.ShowGracePeriodWarning)
                    {
                        if (MessageBox.Show("You must log in within the next " + days.ToString() + " days to continue to use " + Instance.AuthProvider.ApplicationName + ".  Do you want to Log in now?", "Log In", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            chooseToAuthNow = false;
                        }
                    }
                    else
                    {
                        chooseToAuthNow = false;
                    }
                }
            }
            if (authRequiredNow | chooseToAuthNow)
            {
                if (authRequiredNow && this.AuthProvider.ShowGracePeriodWarning)
                {
                    //MessageBox.Show("You must log in to continue using " + Instance.AuthProvider.ApplicationName + ".", "Log In", MessageBoxButtons.OK); ///WYJ fix
                }
                if (!this.Authenticate() && false ) //&& flag2) ///WYJ fix
                {
                    MessageBox.Show("Log in required, terminating");
                    this.authenticationProvider.Close();
                    Environment.Exit(1);
                }
            }
            if (this.dataSourceManager.DataSources.Count == 0)
            {
                StaticDataProvider provider = this.dataSourceManager.FindProvider("FidelityStaticProvider");
                if (provider != null)
                {
                    provider.Initialize(this.dataSourceManager);
                    DataSource source = new DataSource(provider) {
                        DSString = "AA,AIG,AXP,BA,C,CAT,DD,DIS,GE,GM,HD,HON,HPQ,IBM,INTC,JNJ,JPM,KO,MCD,MMM,MO,MRK,MSFT,PFE,PG,T,UTX,VZ,WMT,XOM",
                        Name = "Dow 30",
                        Scale = BarScale.Daily
                    };
                    this.dataSourceManager.Add(source);
                }
            }
            this.assemblyLoader_PerformanceVisualizer.Path = this.AppPath;
            List<IPerformanceVisualizer> list = new List<IPerformanceVisualizer>();
            foreach (System.Type type3 in this.assemblyLoader_PerformanceVisualizer.Types)
            {
                try
                {
                    Control control = (Control) this.assemblyLoader_PerformanceVisualizer.CreateInstance(type3);
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
            foreach (string str8 in this.settingsManager.Get("PVOrder", "Performance|By Symbol|Trades|Equity Curve|Drawdown|Profit Distribution|By Period|MAE/MFE").Split(new char[] { '|' }))
            {
                using (enumerator2 = list.GetEnumerator())
                {
                    IPerformanceVisualizer current;
                    while (enumerator2.MoveNext())
                    {
                        current = enumerator2.Current;
                        if (current.TabText == str8)
                        {
                            ///goto  Label_0850;  ///WYJ fix, simplify the flow
                            this.visualizers.Add(current);
                            list.Remove(current);
                            break;
                        }
                    }
                }
            }
            foreach (IPerformanceVisualizer visualizer3 in list)
            {
                this.visualizers.Add(visualizer3);
            }
            foreach (string str10 in this.settingsManager.Get("PVChecked", "Performance|By Symbol|Trades|Equity Curve|Drawdown|Profit Distribution|By Period").Split(new char[] { '|' }))
            {
                using (enumerator2 = this.visualizers.GetEnumerator())
                {
                    IPerformanceVisualizer visualizer4;
                    while (enumerator2.MoveNext())
                    {
                        visualizer4 = enumerator2.Current;
                        if (visualizer4.TabText == str10)
                        {
                            ///goto  Label_0934;  ///WYJ fix, simplify the flow
                            this.visualizersChecked.Add(visualizer4);
                            break;
                        }
                    }
                }
            }
            this.chartRenderer.BarSpacing = this.settingsManager.Get("BarSpacing", 6);
            this.chartRenderer.BackgroundColor = this.settingsManager.Get("ChartBackgroundColor", Color.White);
            this.chartRenderer.UpBarColor = this.settingsManager.Get("ChartUpBarColor", Color.Green);
            this.chartRenderer.DownBarColor = this.settingsManager.Get("ChartDownBarColor", Color.Red);
            this.chartRenderer.UpBarVolumeColor = this.settingsManager.Get("ChartUpVolumeColor", Color.Green);
            this.chartRenderer.DownBarVolumeColor = this.settingsManager.Get("ChartDownVolumeColor", Color.Red);
            this.chartRenderer.GridlineColor = this.settingsManager.Get("ChartGridlineColor", Color.Gainsboro);
            this.chartRenderer.MarginRightColor = this.settingsManager.Get("ChartRightMarginColor", Color.Gainsboro);
            this.chartRenderer.MarginBottomColor = this.settingsManager.Get("ChartBottomMarginColor", Color.Navy);
            this.chartRenderer.PaneSeparatorColor = this.settingsManager.Get("ChartPaneSeparatorColor", Color.Black);
            this.chartRenderer.AxisFont = this.settingsManager.Get("ChartFont", this.chartRenderer.AxisFont);
            this.chartRenderer.FundamentalGlyphs = this.settingsManager.Get("FundamentalsCharted", this.chartRenderer.FundamentalGlyphs);
            this.chartRenderer.LogScale = this.settingsManager.Get("LogScale", false);
            this.chartRenderer.TitleFont = this.settingsManager.Get("TitleFont", this.chartRenderer.TitleFont);
            this.chartRenderer.HorizontalGridines = this.settingsManager.Get("HorizontalGridlines", true);
            this.chartRenderer.VerticalGridlines = this.settingsManager.Get("VerticalGridlines", true);
            this.chartRenderer.PaneSeparatorVisible = this.settingsManager.Get("PaneSeparators", true);
            this.assemblyLoader_Commission.Path = this.AppPath;
            this.tradingSystemExecutor.ApplyCommission = this.settingsManager.Get("ApplyCommissions", true);
            string str11 = this.settingsManager.Get("Commission", "FidelityFlatRate");
            System.Type commissionClass = typeof(FidelityFlatRate); ///this.method_3();  ///WYJ fix, inline the method
            using (enumerator = this.assemblyLoader_Commission.Types.GetEnumerator())
            {
                System.Type type5;
                while (enumerator.MoveNext())
                {
                    type5 = enumerator.Current;
                    if (type5.Name == str11)
                    {
                        ///goto  Label_0C02;  ///WYJ fix, simplify the flow
                        commissionClass = type5;
                        break;
                    }
                }
            }
            this.tradingSystemExecutor.Commission = (Commission) this.assemblyLoader_Commission.CreateInstance(commissionClass);
            if (this.tradingSystemExecutor.Commission is ICustomSettings)
            {
                (this.tradingSystemExecutor.Commission as ICustomSettings).ReadSettings(this.Settings);
            }
            this.assemblyLoader_PosSizer.Path = this.AppPath;
            foreach (System.Type type6 in this.assemblyLoader_PosSizer.Types)
            {
                PosSizer sizer = (PosSizer) this.assemblyLoader_PosSizer.CreateInstance(type6);
                this.posSizers.Add(sizer);
                if (sizer is ICustomSettings)
                {
                    (sizer as ICustomSettings).ReadSettings(this.Settings);
                }
            }
            this.posSizers.Sort(new CompareByFriendlyName());
            TradingSystemExecutor.PosSizers = this.posSizers;
            this.assemblyLoader_Optimizer.Path = this.AppPath;
            foreach (System.Type type7 in this.assemblyLoader_Optimizer.Types)
            {
                Optimizer optimizer = (Optimizer) this.assemblyLoader_Optimizer.CreateInstance(type7);
                this.optimizers.Add(optimizer);
            }
            string str12 = this.settingsManager.Get("PositionSize", "");
            if (str12 != "")
            {
                this.tradingSystemExecutor.PosSize = PositionSize.Parse(str12);
            }
            string str13 = this.settingsManager.Get("DataRange", "");
            if (str13 != "")
            {
                this.barRange.DataRange = BarDataRange.Parse(str13);
            }
            this.tradingSystemExecutor.EnableSlippage = this.settingsManager.Get("EnableSlippage", false);
            this.tradingSystemExecutor.LimitOrderSlippage = this.settingsManager.Get("LimitOrderSlippage", false);
            this.tradingSystemExecutor.SlippageUnits = this.settingsManager.Get("SlippageUnits", (double) 0.1);
            this.tradingSystemExecutor.SlippageTicks = this.settingsManager.Get("SlippageTicks", 1);
            this.tradingSystemExecutor.RoundLots = this.settingsManager.Get("RoundLots", false);
            this.tradingSystemExecutor.RoundLots50 = this.settingsManager.Get("RoundLots50", false);
            this.tradingSystemExecutor.LimitDaySimulation = this.settingsManager.Get("LimitDaySimulation", false);
            this.tradingSystemExecutor.ApplyInterest = this.settingsManager.Get("ApplyInterest", false);
            this.tradingSystemExecutor.CashRate = this.settingsManager.Get("CashRate", (double) 1.0);
            this.tradingSystemExecutor.MarginRate = this.settingsManager.Get("MarginRate", (double) 7.0);
            this.tradingSystemExecutor.ApplyDividends = this.settingsManager.Get("ApplyDividends", false);
            this.tradingSystemExecutor.ReduceQtyBasedOnVolume = this.settingsManager.Get("ReduceQtyBasedOnVolume", false);
            this.tradingSystemExecutor.RedcuceQtyPct = this.settingsManager.Get("ReduceQtyPct", (double) 10.0);
            this.tradingSystemExecutor.WorstTradeSimulation = this.settingsManager.Get("WorstTradeSimulation", false);
            this.tradingSystemExecutor.BenchmarkSymbol = this.settingsManager.Get("BenchmarkSymbol", string.Empty);
            this.tradingSystemExecutor.BenchmarkBuyAndHoldON = this.settingsManager.Get("BenchmarkBuyAndHoldON", false);
            this.tradingSystemExecutor.PricingDecimalPlaces = this.settingsManager.Get(DecimalsManager.Instance.PricingKey, 2);
            this.tradingSystemExecutor.NoDecimalRoundingForLimitStopPrice = this.settingsManager.Get("NoDecimalRoundingForLimitStopPrice", false);
            string str14 = this.settingsManager.Get("StreamingProvider", "FidelityACTIVStreamingProvider");
            if (str14.CompareTo("FidelityStreamingProvider") == 0)
            {
                str14 = "FidelityACTIVStreamingProvider";
            }
            this.assemblyLoader_StreamingDataProvider.Path = this.AppPath;
            using (enumerator = this.assemblyLoader_StreamingDataProvider.Types.GetEnumerator())
            {
                System.Type type8;
                while (enumerator.MoveNext())
                {
                    type8 = enumerator.Current;
                    if (type8.Name == str14)
                    {
                        ///goto  Label_1075; ///WYJ fix, simplify the flow
                        this.StreamingProvider = (StreamingDataProvider)this.assemblyLoader_StreamingDataProvider.CreateInstance(type8);
                        break; 
                    }
                }
            }
            bool badTickFilter = this.settingsManager.Get("BadTickFilter", false);
            double threshold = this.settingsManager.Get("BadTickThreshold", (double) 20.0);
            StreamingDataProvider.SetBadTickFilterSettings(badTickFilter, threshold);

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
                    if ((iD != null) && !this.strategyMRU.Contains(iD))
                    {
                        this.strategyMRU.Add(iD);
                    }
                }
            }
            this.tradeManager.RootPath = this.DataPath;
            this.tradeManager.DefaultAccountNumber = this.DefaultAccountNumber;
            try
            {
                this.tradeManager.LoadOrdersAndHistory();
            }
            catch
            {
            }
            foreach (Order order in this.tradeManager.Orders)
            {
                Guid strategyID = order.StrategyID;
                order.Strategy = this.strategyManager_0.LookupID(order.StrategyID.ToString());
            }
            WealthLab.TradeManager.DisablePortfolioSynch = this.settingsManager.Get(WealthLab.TradeManager.DisablePortfolioSynchKey, false);
            BarsLoader.FuturesMode = this.settingsManager.Get("FuturesMode", true);
            this.tradeManager.AlwaysExitAllSharesInPosition = this.settingsManager.Get("ExitFullPosition", false) && !WealthLab.TradeManager.DisablePortfolioSynch;
            this.tradeManager.SameBarExits = this.settingsManager.Get("SameBarExits", false);
            this.tradeManager.EnableCashThreshold = this.settingsManager.Get("EnableCashThreshold", false);
            this.tradeManager.CashThreshold = this.settingsManager.Get("CashThreshold", 0);
            this.tradeManager.EnableBuyingPowerThreshold = this.settingsManager.Get("EnableBuyingPowerThreshold", false);
            this.tradeManager.BuyingPowerThreshold = this.settingsManager.Get("BuyingPowerThreshold", 0);
            if (this.settingsManager.Get("ScheduledUpdateTime_Local", "") == "")
            {
                string str17 = this.settingsManager.Get("ScheduledUpdateTime", "");
                if (str17 != "")
                {
                    int hour = int.Parse(str17.Substring(0, 2));
                    DateTime local = new DateTime(0x7db, 1, 1, hour, 0, 0);
                    string str18 = TimeZoneInformation.ToLocalTime(TimeZoneInformation.ToUniversalTime("Eastern Standard Time", local), TimeZoneInformation.CurrentTimeZone.Name).Hour.ToString("D2") + ":00";
                    this.settingsManager.Set("ScheduledUpdateTime_Local", str18);
                    this.settingsManager.SaveSettings();
                }
            }
            this.timer_0.Enabled = true;
            if (File.Exists(this.accountFile))
            {
                string[] strArray5 = File.ReadAllLines(this.accountFile);
                this.accountNumbers.Clear();
                if (strArray5 != null)
                {
                    foreach (string str19 in strArray5)
                    {
                        this.accountNumbers.Add(this.decrypt(str19));
                    }
                }
            }
            if (this.BrokerProvider != null)
            {
                foreach (Account account in this.BrokerProvider.Accounts)
                {
                    if (!this.accountNumbers.Contains(account.AccountNumber))
                    {
                        this.accountNumbers.Add(account.AccountNumber);
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
            DecimalsManager.Instance.SetValues(this.settingsManager);
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
            this.dynamicMenuitems.Add(item);
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
            using (List<Strategy>.Enumerator enumerator = this.strategyMRU.GetEnumerator())
            {
                Strategy current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (strategy_0.ID == current.ID)
                    {
                        ///goto  Label_0037;  ///WYJ fix, simplify the flow
                        item = current;
                        break;
                    }
                }
            }
            if (item != null)
            {
                this.strategyMRU.Remove(item);
            }
            this.strategyMRU.Insert(0, strategy_0);
            while (this.strategyMRU.Count > 10)
            {
                this.strategyMRU.RemoveAt(10);
            }
            this.SaveStrategyMRU();
        }

        public void AddWorkspaceMenuItem(string workspace)
        {
            this.workspaceMenuItems.Add(workspace);
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
            if (this.authenticationProvider.Authenticate(ref daysBeforeNextAuthRequired, ref str))
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
            foreach (StaticDataProvider provider in this.dataSourceManager.Providers)
            {
                if (provider.FriendlyName == "Fidelity Investments")
                {
                    form.AddProvider(provider);
                }
            }
            List<StaticDataProvider> list = new List<StaticDataProvider>();
            foreach (StaticDataProvider provider3 in this.dataSourceManager.Providers)
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
            if (this.strategyMRU.Contains(strategy_0))
            {
                this.strategyMRU.Remove(strategy_0);
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GetPositionAccountTradeType(AccountPosition acctPos)
        {
            return this.BrokerProvider.GetPositionAccountTradeType(acctPos);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            PositionSize size = new PositionSize();
            this.timer_0 = new System.Windows.Forms.Timer(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.settingsManager = new SettingsManager(this.components);
            this.assemblyLoader_PerformanceVisualizer = new AssemblyLoader(this.components);
            this.assemblyLoader_Commission = new AssemblyLoader(this.components);
            this.assemblyLoader_StreamingDataProvider = new AssemblyLoader(this.components);
            this.assemblyLoader_Optimizer = new AssemblyLoader(this.components);
            this.barRange = new BarDataRangeSelecter();
            this.dataSourceManager = new DataSourceManager(this.components);
            this.chartRenderer = new ChartRenderer(this.components);
            this.strategyManager_0 = new StrategyManager(this.components);
            this.drawingObjectManager_0 = new DrawingObjectManager(this.components);
            this.tradingSystemExecutor = new TradingSystemExecutor(this.components);
            this.strategyManager_1 = new StrategyManager(this.components);
            this.tradeManager = new WealthLab.TradeManager(this.components);
            this.assemblyLoader_PosSizer = new AssemblyLoader(this.components);
            base.SuspendLayout();
            this.timer_0.Interval = 0xea60;
            this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
            this.helpProvider.HelpNamespace = "WLNetUserGuide.chm";
            this.settingsManager.FileName = "WealthLabConfig.txt";
            this.settingsManager.IsEncrypted = false;
            this.settingsManager.RootPath = null;
            this.assemblyLoader_PerformanceVisualizer.BaseClass = "";
            this.assemblyLoader_PerformanceVisualizer.DLLNameFilter = "";
            this.assemblyLoader_PerformanceVisualizer.Interface = "IPerformanceVisualizer";
            this.assemblyLoader_PerformanceVisualizer.Path = null;
            this.assemblyLoader_PerformanceVisualizer.PathMask = "*.dll";
            this.assemblyLoader_Commission.BaseClass = "Commission";
            this.assemblyLoader_Commission.DLLNameFilter = "";
            this.assemblyLoader_Commission.Interface = null;
            this.assemblyLoader_Commission.Path = null;
            this.assemblyLoader_Commission.PathMask = "*.dll";
            this.assemblyLoader_StreamingDataProvider.BaseClass = "StreamingDataProvider";
            this.assemblyLoader_StreamingDataProvider.DLLNameFilter = "";
            this.assemblyLoader_StreamingDataProvider.Interface = null;
            this.assemblyLoader_StreamingDataProvider.Path = null;
            this.assemblyLoader_StreamingDataProvider.PathMask = "*.dll";
            this.assemblyLoader_Optimizer.BaseClass = "Optimizer";
            this.assemblyLoader_Optimizer.DLLNameFilter = "";
            this.assemblyLoader_Optimizer.Interface = null;
            this.assemblyLoader_Optimizer.Path = null;
            this.assemblyLoader_Optimizer.PathMask = "*.dll";
            this.barRange.BackColor = Color.AliceBlue;
            this.barRange.IsStreaming = false;
            this.barRange.Location = new Point(4, 0x1f);
            this.barRange.Name = "barRange";
            this.barRange.Size = new Size(0x79, 20);
            this.barRange.TabIndex = 1;
            this.dataSourceManager.OnDemandUpdatesEnabled = true;
            this.dataSourceManager.RootPath = null;
            this.dataSourceManager.StockSplitDataAdjusted += new EventHandler<StockSplitEventArgs>(this.method_5);
            this.chartRenderer.AxisFont = new Font("Tahoma", 7f);
            this.chartRenderer.BackgroundColor = Color.WhiteSmoke;
            this.chartRenderer.BarSpacing = 4;
            this.chartRenderer.DownBarColor = Color.Red;
            this.chartRenderer.DownBarVolumeColor = Color.Red;
            this.chartRenderer.Executor = null;
            this.chartRenderer.Fundamentals = null;
            this.chartRenderer.FundamentalsVisible = true;
            this.chartRenderer.GridlineColor = Color.Gainsboro;
            this.chartRenderer.HorizontalGridines = true;
            this.chartRenderer.IndicatorLabelsVisible = false;
            this.chartRenderer.LogScale = false;
            this.chartRenderer.MarginBottomColor = Color.Navy;
            this.chartRenderer.MarginBottomHeight = 20;
            this.chartRenderer.MarginRightColor = Color.Gainsboro;
            this.chartRenderer.MarginRightWidth = 40;
            this.chartRenderer.PaneSeparatorColor = Color.Black;
            this.chartRenderer.PaneSeparatorVisible = true;
            this.chartRenderer.PlotStops = false;
            this.chartRenderer.RightPaddingBars = 0;
            this.chartRenderer.ScrollOffset = 0;
            this.chartRenderer.TitleFont = new Font("Verdana", 8f);
            this.chartRenderer.TradeAnnotationsVisible = true;
            this.chartRenderer.TradeArrowsVisible = true;
            this.chartRenderer.TradeCirclesVisible = true;
            this.chartRenderer.UpBarColor = Color.Green;
            this.chartRenderer.UpBarVolumeColor = Color.Green;
            this.chartRenderer.VerticalGridlines = true;
            this.chartRenderer.VolumePaneVisible = true;
            this.strategyManager_0.RootPath = null;
            this.drawingObjectManager_0.ChartBookName = "Standard";
            this.drawingObjectManager_0.RootPath = null;
            this.tradingSystemExecutor.ApplyCommission = false;
            this.tradingSystemExecutor.ApplyDividends = false;
            this.tradingSystemExecutor.ApplyInterest = false;
            this.tradingSystemExecutor.BarsLoader = null;
            this.tradingSystemExecutor.BuildEquityCurves = true;
            this.tradingSystemExecutor.CashRate = 0.0;
            this.tradingSystemExecutor.EnableSlippage = false;
            this.tradingSystemExecutor.ExceptionEvents = false;
            this.tradingSystemExecutor.FundamentalsLoader = null;
            this.tradingSystemExecutor.IsStreaming = false;
            this.tradingSystemExecutor.LimitDaySimulation = false;
            this.tradingSystemExecutor.LimitOrderSlippage = false;
            this.tradingSystemExecutor.MarginRate = 0.0;
            this.tradingSystemExecutor.OverrideShareSize = 0.0;
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
            this.tradingSystemExecutor.PosSize = size;
            this.tradingSystemExecutor.RedcuceQtyPct = 10.0;
            this.tradingSystemExecutor.ReduceQtyBasedOnVolume = false;
            this.tradingSystemExecutor.Renderer = null;
            this.tradingSystemExecutor.RoundLots = false;
            this.tradingSystemExecutor.RoundLots50 = false;
            this.tradingSystemExecutor.SlippageTicks = 1;
            this.tradingSystemExecutor.SlippageUnits = 1.0;
            this.tradingSystemExecutor.StrategyName = "";
            this.tradingSystemExecutor.WorstTradeSimulation = false;
            this.strategyManager_1.RootPath = null;
            this.tradeManager.AlwaysExitAllSharesInPosition = false;
            this.tradeManager.AutoTradingEnabled = AutoTradingMode.Off;
            this.tradeManager.BuyingPowerThreshold = 0.0;
            this.tradeManager.CashThreshold = 0.0;
            this.tradeManager.EnableBuyingPowerThreshold = false;
            this.tradeManager.EnableCashThreshold = false;
            this.tradeManager.RootPath = null;
            this.tradeManager.SameBarExits = false;
            this.tradeManager.SettingsHost = null;
            this.tradeManager.HistoryItemAdded += new EventHandler<HistoricalTradeEventArgs>(this.historyItemAddedEventHandler);
            this.tradeManager.PositionRemoved += new EventHandler<AccountPositionEventArgs>(this.positionRemovedEventHandler);
            this.tradeManager.PositionAdded += new EventHandler<AccountPositionEventArgs>(this.positionAddedEventHandler);
            this.tradeManager.PositionsUpdated += new EventHandler<AccountEventArgs>(this.positionsUpdatedEventHandler);
            this.tradeManager.HistoryItemUpdated += new EventHandler<HistoricalTradeEventArgs>(this.historyItemUpdatedEventHandler);
            this.tradeManager.OrderRemoved += new EventHandler<OrderEventArgs>(this.orderRemovedEventHandler);
            this.tradeManager.OrdersUpdated += new EventHandler<EventArgs>(this.ordersUpdatedEventHandler);
            this.tradeManager.StatusBarUpdated += new EventHandler<StringEventArgs>(this.statusBarUpdatedEventHandler);
            this.tradeManager.OrderAdded += new EventHandler<OrderEventArgs>(this.orderAddedEventHandler);
            this.tradeManager.AccountUpdated += new EventHandler<AccountEventArgs>(this.accountUpdatedEventHandler);
            this.tradeManager.QuoteUpdated += new EventHandler<QuoteEventArgs>(this.quoteUpdatedEventHandler);
            this.tradeManager.OrderChanged += new EventHandler<OrderEventArgs>(this.orderStatusUpdatedEventHandler);
            this.tradeManager.OrderStatusUpdated += new EventHandler<OrderEventArgs>(this.orderStatusUpdatedEventHandler);
            this.tradeManager.PositionChanged += new EventHandler<AccountPositionEventArgs>(this.positionChangedEventHandler);
            this.assemblyLoader_PosSizer.BaseClass = "PosSizer";
            this.assemblyLoader_PosSizer.DLLNameFilter = "";
            this.assemblyLoader_PosSizer.Interface = null;
            this.assemblyLoader_PosSizer.Path = null;
            this.assemblyLoader_PosSizer.PathMask = "*.dll";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(this.barRange);
            this.helpProvider.SetHelpKeyword(this, "introduction.htm");
            this.helpProvider.SetHelpNavigator(this, HelpNavigator.Topic);
            base.Name = "MainModule";
            this.helpProvider.SetShowHelp(this, true);
            base.Click += new EventHandler(this.MainModule_Click);
            base.ResumeLayout(false);
        }

        public Bars LoadExternalSymbol(string dataSetName, string symbol)
        {
            Bars bars = null;
            using (IEnumerator<DataSource> enumerator = this.dataSourceManager.DataSources.GetEnumerator())
            {
                DataSource current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Name == dataSetName)
                    {
                        ///goto  Label_0035;  ///WYJ fix, simplify the flow
                        bars = current.Provider.RequestData(current, symbol, DateTime.MinValue, DateTime.MaxValue, 0, false);
                        return bars; 
                    }
                }
                return bars;
            }
        }

        public Bars LoadExternalSymbol(string symbol, BarScale scale, int barInterval, bool includePartialBar)
        {
            BarDataScale scale3;
            using (IEnumerator<DataSource> enumerator = this.dataSourceManager.DataSources.GetEnumerator())
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
                            ///goto  Label_0075;  ///WYJ fix, simplify the flow
                            this.lookUpSymbolInfo(bars3);
                            return bars3;
                        }
                    }
                }
            }
            using (IEnumerator<StaticDataProvider> enumerator3 = this.dataSourceManager.Providers.GetEnumerator())
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
                            ///goto  Label_0103;  ///WYJ fix, simplify the flow
                            this.lookUpSymbolInfo(bars4);
                            return bars4;
                        }
                    }
                }
            }
            scale3 = new BarDataScale(scale, barInterval);
            using (IEnumerator<DataSource> enumerator2 = this.dataSourceManager.DataSources.GetEnumerator())
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
                                ///goto  Label_01A2;  ///WYJ fix, simplify the flow
                                this.lookUpSymbolInfo(bars);
                                return bars;
                            }
                        }
                    }
                }
                return null;
            }
        }

        public void LoginSuccessful()
        {
            this.IsAuthenticated = true;
            if ((this.streamingDataProvider != null) && this.streamingDataProvider.StreamingAtDisconnect)
            {
                this.streamingDataProvider.ConnectStreaming(this);
            }
            if (this.BrokerProvider != null)
            {
                this.BrokerProvider.Accounts.Clear();
                this.BrokerProvider.RequestUpdates();
                this.settingsManager.Set("LoggedIn", true);
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
                    this.accountNumbers.Clear();
                    string[] contents = new string[this.BrokerProvider.Accounts.Count];
                    int index = 0;
                    foreach (Account account2 in this.BrokerProvider.Accounts)
                    {
                        this.accountNumbers.Add(account2.AccountNumber);
                        contents[index] = this.encrypt(account2.AccountNumber);
                        index++;
                    }
                    FileNameValidator.ValidateFileName(this.accountFile);
                    File.WriteAllLines(this.accountFile, contents);
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

        ///WYJ fix, original signature: private void method_0()
        private void saveNextAuthRequiredToSettings()
        {
            string str = this.nextAuthRequired.Ticks.ToString();
            StringBuilder builder = new StringBuilder();
            builder.Append(str.ToString());
            if (this.NicAdressesCount == 0)
            {
                builder.Append(";none");
            }
            else
            {
                this.indexNicAddress = 0;   ///WYJ note, after the loop, indexNicAddress points to last NicAddress
                while (this.indexNicAddress < this.NicAdressesCount)
                {
                    builder.Append(";" + this.NicAddress);
                    this.indexNicAddress++;
                }
            }
            string str2 = this.encrypt(builder.ToString());
            this.Settings.Set(this._newGrace, str2);
        }

        ///WYJ fix, original signature: private void method_1()
        private void saveNextAuthRequiredToFile()
        {
            string str = this.nextAuthRequired.Subtract(new TimeSpan(this.AuthProvider.GracePeriod, 0, 0, 0)).Ticks.ToString();
            str = this.encrypt(str);
            FileNameValidator.ValidateFileName(this._authFile);
            File.WriteAllText(this._authFile, str);
        }

        ///WYJ fix, original signature: internal string method_10(string string_2, string string_3)
        internal string encryptWith(string string_2, string string_3)
        {
            return Cryptography.Crypt(string_2, string_3, true);
        }

        ///WYJ fix, original signature: internal string method_11(string string_2)
        internal string decrypt(string string_2)
        {
            return Cryptography.Crypt(string_2, this._password, false);
        }

        ///WYJ fix, original signature: internal string method_12(string string_2, string string_3)
        internal string decryptWith(string string_2, string string_3)
        {
            return Cryptography.Crypt(string_2, string_3, false);
        }

        ///WYJ fix, original signature: private void method_13(object sender, OrderEventArgs e)
        private void orderStatusUpdatedEventHandler(object sender, OrderEventArgs e)
        {
            if (OrdersAlertsForm.Instance != null)
            {
                OrdersAlertsForm.Instance.OrderStatusUpdated(e.Order);
            }
        }

        ///WYJ fix, original signature: private void method_14(object sender, OrderEventArgs e)
        private void orderAddedEventHandler(object sender, OrderEventArgs e)
        {
            if ((OrdersAlertsForm.Instance == null) && this.settingsManager.Get("AutoOpenOrders", true))
            {
                this.FirstMainForm.OpenOrderManager();
            }
            else if (OrdersAlertsForm.Instance != null)
            {
                OrdersAlertsForm.Instance.OrderAdded(e.Order);
            }
            if ((OrdersAlertsForm.Instance != null) && this.settingsManager.Get("SwitchToAccount", true))
            {
                OrdersAlertsForm.Instance.SwitchToAccount(e.Order.Account);
            }
        }

        ///WYJ fix, original signature: private void method_15(object sender, HistoricalTradeEventArgs e)
        private void historyItemUpdatedEventHandler(object sender, HistoricalTradeEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.TradeHistoryItemUpdated(e.HistoricalTrade);
            }
        }

        ///WYJ fix, original signature: private void method_16(object sender, HistoricalTradeEventArgs e)
        private void historyItemAddedEventHandler(object sender, HistoricalTradeEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.TradeHistoryItemAdded(e.HistoricalTrade);
            }
        }

        ///WYJ fix, original signature: private void method_17(object sender, AccountEventArgs e)
        private void accountUpdatedEventHandler(object sender, AccountEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.AccountUpdated(e.Account);
            }
        }

        ///WYJ fix, original signature: private void method_18(object sender, OrderEventArgs e)
        private void orderRemovedEventHandler(object sender, OrderEventArgs e)
        {
            if (OrdersAlertsForm.Instance != null)
            {
                OrdersAlertsForm.Instance.OrderRemoved(e.Order);
            }
        }

        ///WYJ fix, original signature: private void method_19(object sender, AccountEventArgs e)
        private void positionsUpdatedEventHandler(object sender, AccountEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.PositionsUpdated(e.Account);
            }
        }

        ///WYJ fix, original signature: private void method_2(object object_0)
        private void doPlaySound(object player)
        {
            ((SoundPlayer) player).PlaySync();
            this.soundPlaying = false;
        }

        ///WYJ fix, original signature: private void method_20(object sender, AccountPositionEventArgs e)
        private void positionAddedEventHandler(object sender, AccountPositionEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.PositionAdded(e.AccountPosition);
            }
        }

        ///WYJ fix, original signature: private void method_21(object sender, AccountPositionEventArgs e)
        private void positionChangedEventHandler(object sender, AccountPositionEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.PositionChanged(e.AccountPosition);
            }
        }

        ///WYJ fix, original signature: private void method_22(object sender, AccountPositionEventArgs e)
        private void positionRemovedEventHandler(object sender, AccountPositionEventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.PositionRemoved(e.AccountPosition);
            }
        }

        ///WYJ fix, original signature: private void method_23(object sender, QuoteEventArgs e)
        private void quoteUpdatedEventHandler(object sender, QuoteEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).UpdateTradeTicketQuote(e.Quote);
                }
            }
        }

        ///WYJ fix, original signature: private void method_24(object sender, StringEventArgs e)
        private void statusBarUpdatedEventHandler(object sender, StringEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    (form as MainForm).UpdateStatus(e.Message);
                }
            }
        }

        ///WYJ fix, original signature: internal void method_25(Alert alert_0, bool bool_5)
        internal void emailAlert(Alert alert_0, bool bool_ShouldOrderBePlaced)   ///WYJ note, bool_ShouldOrderBePlaced is not used
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
            string credPass = Instance.decrypt(Instance.Settings.Get("EmailPassword", string.Empty));
            bool sSL = Instance.Settings.Get("EmailSSL", false);
            int.TryParse(s, out num6);
            new WLPEmail(host, num6, sSL, credUser, credPass, address, subject, builder.ToString()).Enqueue(Application.ProductName);
        }

        ///WYJ fix, original signature: internal void method_26(List<Alert> list_10)
        internal void emailAlerts(List<Alert> list_10)
        {
            foreach (Alert alert in list_10)
            {
                this.emailAlert(alert, true);
            }
        }

        ///WYJ fix, original signature: internal void method_27(string string_2, int int_2, bool bool_5, string string_3, string string_4, string string_5)
        internal void sendTestEmail(string string_2, int int_2, bool bool_5, string string_3, string string_4, string string_5)
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

        ///WYJ fix, inlined, not needed anymore
        /*
        private System.Type method_3()
        {
            return typeof(FidelityFlatRate);
        } */

        ///WYJ fix, original signature: private void method_4(Bars bars_0)
        private void lookUpSymbolInfo(Bars bars_0)
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
                        ///goto  Label_0032; ///WYJ fix, simplify the flow
                        info2 = new SymbolInfo(current);
                        bars_0.SymbolInfo = info2;
                        return;
                    }
                }
                return;
            }
        }

        private void method_5(object sender, StockSplitEventArgs e)
        {
            this.drawingObjectManager_0.SplitAdjustDrawingObjects(e.Symbol, e.SplitFactor, e.ExDate);
        }

        ///WYJ fix, original signature: private void method_6()
        private void saveChartRendererSettings()
        {
            this.settingsManager.Set("BarSpacing", this.chartRenderer.BarSpacing);
            this.settingsManager.Set("ChartBackgroundColor", this.chartRenderer.BackgroundColor);
            this.settingsManager.Set("ChartUpBarColor", this.chartRenderer.UpBarColor);
            this.settingsManager.Set("ChartDownBarColor", this.chartRenderer.DownBarColor);
            this.settingsManager.Set("ChartUpVolumeColor", this.chartRenderer.UpBarVolumeColor);
            this.settingsManager.Set("ChartDownVolumeColor", this.chartRenderer.DownBarVolumeColor);
            this.settingsManager.Set("ChartGridlineColor", this.chartRenderer.GridlineColor);
            this.settingsManager.Set("ChartRightMarginColor", this.chartRenderer.MarginRightColor);
            this.settingsManager.Set("ChartBottomMarginColor", this.chartRenderer.MarginBottomColor);
            this.settingsManager.Set("ChartPaneSeparatorColor", this.chartRenderer.PaneSeparatorColor);
            this.settingsManager.Set("HorizontalGridlines", this.chartRenderer.HorizontalGridines);
            this.settingsManager.Set("VerticalGridlines", this.chartRenderer.VerticalGridlines);
            this.settingsManager.Set("PaneSeparators", this.chartRenderer.PaneSeparatorVisible);
            this.settingsManager.Set("ChartFont", this.chartRenderer.AxisFont);
            this.settingsManager.Set("FundamentalsCharted", this.chartRenderer.FundamentalGlyphs);
            this.settingsManager.Set("LogScale", this.chartRenderer.LogScale);
            this.settingsManager.Set("TitleFont", this.chartRenderer.TitleFont);
        }

        ///WYJ fix, original signature: private void method_7(string string_2, string string_3)
        private void copyAllFiles(string sourceDir, string destDir)
        {
            if (Directory.Exists(sourceDir))
            {
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                foreach (string str in Directory.GetDirectories(sourceDir))
                {
                    string[] strArray2 = str.Split(new char[] { '\\' });
                    string str2 = strArray2[strArray2.Length - 1];
                    string str3 = sourceDir + @"\" + str2;
                    string str4 = destDir + @"\" + str2;
                    this.copyAllFiles(str3, str4);
                }
                foreach (string str5 in Directory.GetFiles(sourceDir))
                {
                    File.Copy(str5, destDir + @"\" + Path.GetFileName(str5));
                }
            }
        }

        ///WYJ fix, original signature: private void method_8(object sender, EventArgs e)
        private void ordersUpdatedEventHandler(object sender, EventArgs e)
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

        ///WYJ fix, original signature: internal string method_9(string string_2)
        internal string encrypt(string string_2)
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
            if (!this.soundPlaying && File.Exists(soundFile))
            {
                StreamReader reader = new StreamReader(soundFile);
                this.PlaySound(reader.BaseStream, false);
            }
        }

        public void PlaySound(Stream stream, bool prioritize)
        {
            if (!this.soundPlaying || prioritize)
            {
                SoundPlayer parameter = new SoundPlayer(stream);
                this.soundPlaying = true;
                new Thread(new ParameterizedThreadStart(this.doPlaySound)).Start(parameter);
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
                this.saveChartRendererSettings();
                this.settingsManager.Set("PositionSize", this.tradingSystemExecutor.PosSize.ToString());
                this.settingsManager.Set("DataRange", this.barRange.DataRange.ToString());
                if (flag2)
                {
                    this.settingsManager.Set("EnableSlippage", this.tradingSystemExecutor.EnableSlippage);
                    this.settingsManager.Set("LimitOrderSlippage", this.tradingSystemExecutor.LimitOrderSlippage);
                    this.settingsManager.Set("SlippageUnits", this.tradingSystemExecutor.SlippageUnits);
                    this.settingsManager.Set("SlippageTicks", this.tradingSystemExecutor.SlippageTicks);
                }
                this.settingsManager.Set("RoundLots", this.tradingSystemExecutor.RoundLots);
                this.settingsManager.Set("RoundLots50", this.tradingSystemExecutor.RoundLots50);
                this.settingsManager.Set("ApplyCommissions", this.tradingSystemExecutor.ApplyCommission);
                this.settingsManager.Set("LimitDaySimulation", this.tradingSystemExecutor.LimitDaySimulation);
                this.settingsManager.Set("ApplyInterest", this.tradingSystemExecutor.ApplyInterest);
                this.settingsManager.Set("CashRate", this.tradingSystemExecutor.CashRate);
                this.settingsManager.Set("MarginRate", this.tradingSystemExecutor.MarginRate);
                this.settingsManager.Set("ApplyDividends", this.tradingSystemExecutor.ApplyDividends);
                if (this.streamingDataProvider != null)
                {
                    this.settingsManager.Set("StreamingProvider", this.streamingDataProvider.GetType().Name);
                }
                this.settingsManager.Set("FuturesMode", BarsLoader.FuturesMode);
                this.settingsManager.Set("ReduceQtyBasedOnVolume", this.tradingSystemExecutor.ReduceQtyBasedOnVolume);
                this.settingsManager.Set("ReduceQtyPct", this.tradingSystemExecutor.RedcuceQtyPct);
                this.settingsManager.Set("WorstTradeSimulation", this.tradingSystemExecutor.WorstTradeSimulation);
                this.settingsManager.Set("BenchmarkSymbol", this.tradingSystemExecutor.BenchmarkSymbol);
                this.settingsManager.Set("BenchmarkBuyAndHoldON", this.tradingSystemExecutor.BenchmarkBuyAndHoldON);
                this.settingsManager.Set(DecimalsManager.Instance.PricingKey, this.tradingSystemExecutor.PricingDecimalPlaces);
                this.settingsManager.Set("NoDecimalRoundingForLimitStopPrice", this.tradingSystemExecutor.NoDecimalRoundingForLimitStopPrice);
                this.settingsManager.Set("SameBarExits", this.tradeManager.SameBarExits);
                this.settingsManager.Set("EnableCashThreshold", this.tradeManager.EnableCashThreshold);
                this.settingsManager.Set("CashThreshold", this.tradeManager.CashThreshold);
                this.settingsManager.Set("EnableBuyingPowerThreshold", this.tradeManager.EnableBuyingPowerThreshold);
                this.settingsManager.Set("BuyingPowerThreshold", this.tradeManager.BuyingPowerThreshold);
                this.settingsManager.SaveSettings();
                bool badTickFilter = this.settingsManager.Get("BadTickFilter", false);
                double threshold = this.settingsManager.Get("BadTickThreshold", (double) 20.0);
                StreamingDataProvider.SetBadTickFilterSettings(badTickFilter, threshold);
                this.settingsManager.Set("StrategyNetworkPathCount", this.StrategyNetworkPaths.Count);
                for (int i = 0; i < this.StrategyNetworkPaths.Count; i++)
                {
                    this.settingsManager.Set("StrategyNetworkPath" + i, this.StrategyNetworkPaths[i]);
                }
            }
        }

        public void SaveStrategyMRU()
        {
            string fileName = this.DataPath + @"\StrategyMRU.txt";
            string[] contents = new string[this.strategyMRU.Count];
            for (int i = 0; i < this.strategyMRU.Count; i++)
            {
                contents[i] = this.strategyMRU[i].ID.ToString();
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
            this.dataSourceManager.OnDemandUpdatesEnabled = onDemandOn;
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
            this.settingsManager.Set("OnDemandDataEnabled", onDemandOn);
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
            this.settingsManager.Set("StreamingSymbols", symbolString);
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
                    int num2 = this.settingsManager.Get("RandomMinute", -1);
                    if (num2 == -1)
                    {
                        num2 = new Random().Next(60);
                        this.settingsManager.Set("RandomMinute", num2);
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
                    this.authenticationProvider.UnAuthenticate();
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
                return this.accountNumbers;
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
                return this.authenticationProvider;
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
                return this.brokerProvider;
            }
        }

        public string DataPath
        {
            get
            {
                string path = Application.UserAppDataPath + @"\Data";
                if (this.authenticationProvider != null)
                {
                    path = path.Replace("WealthLabPro", this.authenticationProvider.UserDataPathToken);
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
                return this.dataSourceManager;
            }
        }

        public string DefaultAccountNumber
        {
            get
            {
                string str = this.Settings.Get("DefaultAccount", "");
                str = this.decrypt(str);
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
                    this.Settings.Set("DefaultAccount", this.encrypt(value));
                    this.Settings.SaveSettings();
                    this.TradeManager.DefaultAccountNumber = value;
                }
            }
        }

        public List<DynamicMenuItem> DynamicMenuitems
        {
            get
            {
                return this.dynamicMenuitems;
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
                return this.tradingSystemExecutor;
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
                return this.helpProvider;
            }
        }

        public bool InGracePeriod
        {
            get
            {
                return this.settingsManager.Get("LoggedIn", false);
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return this.isAuthenticated;
            }
            internal set
            {
                this.isAuthenticated = value;
            }
        }

        private DateTime NextAuthRequired
        {
            get
            {
                if (this.nextAuthRequired == DateTime.MinValue)
                {
                    if (!this.Settings.ContainsKey(this._newGrace))
                    {
                        if (File.Exists(this._authFile))
                        {
                            string str = File.ReadAllText(this._authFile);
                            str = this.decrypt(str);
                            DateTime time2 = new DateTime(long.Parse(str));
                            this.nextAuthRequired = time2 + new TimeSpan(this.AuthProvider.GracePeriod, 0, 0, 0);
                            this.saveNextAuthRequiredToSettings();
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
                            this.nextAuthRequired = now + new TimeSpan(this.authenticationProvider.GracePeriod, 0, 0, 0);
                        }
                    }
                    else
                    {
                        bool nextAuthRequiredRetrieved = false;   ///WYJ fix, original name: flag
                        string str2 = this.Settings.Get(this._newGrace, "");
                        this.indexNicAddress = 0;
                        while (this.indexNicAddress < this.NicAdressesCount)
                        {
                            try
                            {
                                long ticks = long.Parse(this.decryptWith(str2, this.NicAddress));
                                this.nextAuthRequired = new DateTime(ticks);
                                nextAuthRequiredRetrieved = true;
                                break;
                            }
                            catch
                            {
                                this.indexNicAddress++;
                                continue;
                            }
                        }
                        if (!nextAuthRequiredRetrieved)
                        {
                            try
                            {
                                string str4 = this.decrypt(str2);
                                if (!string.IsNullOrEmpty(str4))
                                {
                                    long num2;
                                    string[] nicAddresses = str4.Split(new char[] { ';' });
                                    if (long.TryParse(nicAddresses[0], out num2) && (nicAddresses.Length > 1))
                                    {
                                        if (((nicAddresses.Length == 1) && (nicAddresses[1] == "none")) && (this.NicAdressesCount == 0))
                                        {
                                            nextAuthRequiredRetrieved = true;
                                        }
                                        else
                                        {
                                            for (int i = 1; i < nicAddresses.Length; i++)
                                            {
                                                this.indexNicAddress = 0;
                                                while (this.indexNicAddress < this.NicAdressesCount)
                                                {
                                                    if (this.NicAddress == nicAddresses[i])
                                                    {
                                                        ///goto  Label_0149;  ///WYJ fix, simplify the flow 
                                                        nextAuthRequiredRetrieved = true;
                                                        break;
                                                    }
                                                    this.indexNicAddress++;   ///WYJ note, indexNicAddress represents the position of NicAddress in the array
                                                }
                                                if (nextAuthRequiredRetrieved)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        if (nextAuthRequiredRetrieved)
                                        {
                                            this.nextAuthRequired = new DateTime(num2);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (this.AuthProvider.NextAuthRequired < this.nextAuthRequired)
                    {
                        this.nextAuthRequired = this.AuthProvider.NextAuthRequired;
                        this.saveNextAuthRequiredToSettings();
                        this.saveNextAuthRequiredToFile();
                    }
                }
                return this.nextAuthRequired;
            }
            set
            {
                this.nextAuthRequired = value;
                this.saveNextAuthRequiredToSettings();
                this.saveNextAuthRequiredToFile();
            }
        }

        private string NicAddress
        {
            get
            {
                string str;
                try
                {
                    ///int num = 0;
                    NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                    int num1 = 0;
                    while (num1 < (int)allNetworkInterfaces.Length)
                    {
                        NetworkInterface networkInterface = allNetworkInterfaces[num1];
                        if (num1 != this.indexNicAddress)   ///WYJ fix, original: if (num != this.int_0)
                        {
                            ///num++;
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
                if (this.nicAdressesCount == 0)
                {
                    this.nicAdressesCount = NetworkInterface.GetAllNetworkInterfaces().Length;
                }
                return this.nicAdressesCount;
            }
        }

        public List<Optimizer> Optimizers
        {
            get
            {
                return this.optimizers;
            }
        }

        public List<PosSizer> PosSizers
        {
            get
            {
                return this.posSizers;
            }
        }

        public ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer;
            }
        }

        public SettingsManager Settings
        {
            get
            {
                return this.settingsManager;
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
                return this.strategyMRU;
            }
        }

        public List<string> StrategyNetworkPaths
        {
            get
            {
                return this.strategyNetworkPaths;
            }
        }

        public string StrategyTemplateCode
        {
            get
            {
                if (!this.strategyTemplateCodeInited)
                {
                    string path = this.DataPath + @"\StrategyTemplate.txt";
                    this.strategyTemplateCodeInited = true;
                    if (File.Exists(path))
                    {
                        this.strategyTemplateCode = File.ReadAllText(path);
                    }
                    else
                    {
                        this.strategyTemplateCode = Resources.StrategyTemplate;
                    }
                }
                return this.strategyTemplateCode;
            }
            set
            {
                string fileName = this.DataPath + @"\StrategyTemplate.txt";
                this.strategyTemplateCode = value;
                this.strategyTemplateCodeInited = true;
                FileNameValidator.ValidateFileName(fileName);
                File.WriteAllText(fileName, this.strategyTemplateCode);
            }
        }

        public StreamingDataProvider StreamingProvider
        {
            get
            {
                return this.streamingDataProvider;
            }
            internal set
            {
                if (((this.streamingDataProvider == null) || (value == null)) || (this.streamingDataProvider.FriendlyName != value.FriendlyName))
                {
                    if (this.streamingDataProvider != null)
                    {
                        this.streamingDataProvider.DisconnectStreaming();
                    }
                    this.streamingDataProvider = value;
                    if (this.streamingDataProvider != null)
                    {
                        this.streamingDataProvider.Initialize(this.DataSources);
                    }
                }
            }
        }

        public bool StreamingWasClicked
        {
            get
            {
                return this.streamingWasClicked;
            }
            set
            {
                this.streamingWasClicked = value;
            }
        }

        public WealthLab.TradeManager TradeManager
        {
            get
            {
                return this.tradeManager;
            }
        }

        public ICollection<IPerformanceVisualizer> Visualizers
        {
            get
            {
                return this.visualizers;
            }
        }

        public ICollection<IPerformanceVisualizer> VisualizersChecked
        {
            get
            {
                return this.visualizersChecked;
            }
        }

        public List<string> WorkspaceMenuItems
        {
            get
            {
                return this.workspaceMenuItems;
            }
        }
    }
}

