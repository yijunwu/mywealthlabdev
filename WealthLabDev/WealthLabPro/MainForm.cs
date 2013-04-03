namespace WealthLabPro
{
    using CtrlLib;
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;
    using WealthLab.ChartStyles;
    using WealthLabPro.Properties;

    public class MainForm : Form, IItemTracker<ChartForm>, IItemTracker<QuotesForm>, IConnectionStatus, INotifier
    {
        private AccountTypeSelector accountTypeSelector1;
        private AssemblyLoader assemblyLoader_0;
        private static bool bool_0 = true;
        private static bool bool_1 = false;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private bool bool_6;
        private bool bool_7;
        private bool bool_8;
        [CompilerGenerated]
        private bool bool_9;
        private ToolStripButton btnAcctsPositions;
        private ToolStripButton btnBarChart;
        private ToolStripButton btnCandleStyle;
        private ToolStripButton btnClearDrawingObjects;
        private ToolStripButton btnClearIndicators;
        private Button btnCloseTradeTicket;
        private ToolStripButton btnCrossHair;
        private ToolStripButton btnDataManager;
        private ToolStripButton btnDataWindow;
        private ToolStripButton btnDecreaseSpacing;
        private Button btnDockDown;
        private Button btnDockUp;
        private ToolStripButton btnFundamental;
        private ToolStripButton btnFundamentalsTB2;
        private ToolStripButton btnFundamentalsVisible;
        private Button btnGo;
        private ToolStripButton btnHelp;
        private ToolStripButton btnHome;
        private ToolStripButton btnIncreaseSpacing;
        private ToolStripButton btnIndicators;
        private ToolStripButton btnIndicatorsTB2;
        private ToolStripButton btnLabelsVisible;
        private ToolStripButton btnLinear;
        private ToolStripButton btnLineChart;
        private ToolStripButton btnLog;
        private ToolStripButton btnLogin;
        private Button btnLoginTradeTicket;
        private ToolStripButton btnOpenStrategy;
        private ToolStripButton btnOrdersAlerts;
        private Button btnPlaceOrder;
        private ToolStripButton btnPreferences;
        private ToolStripButton btnPreferencesTB;
        private ToolStripButton btnPushCode;
        private ToolStripButton btnRestoreSpacing;
        private ToolStripButton btnSave;
        private ToolStripButton btnSaveAs;
        private Button btnStageOrder;
        private ToolStripButton btnStatusBarVisible;
        private ToolStripButton btnStrategyCenter;
        private ToolStripButton btnTrade;
        private ToolStripButton btnTradeTicket;
        private ToolStripButton btnTrendline;
        private ChartForm chartForm_0;
        private ComboBox cmbAccount;
        private ComboBox cmbSymbol;
        private ComboBox cmbTradeAction;
        private ComboBox cmbTradeOrder;
        private ComboBox cmbTradeRoute;
        private ComboBox cmbTradeTIF;
        private Color color_0 = Color.FromArgb(0xb8, 0xbf, 0xd3);
        private Color color_1 = Color.FromArgb(0xf8, 0xf8, 0xf8);
        private BarDataRangeSelecter dataRange;
        private static Delegate21 delegate21_0 = new Delegate21(MainForm.smethod_0);
        private DrawingObjectManager drawingObjectManager_0;
        private ToolStripDropDownButton dropdownCharts;
        private ToolStripDropDownButton dropdownQuotes;
        private ToolStripMenuItem executeStrategyHiddenMenuItem;
        private IContainer icontainer_0;
        private static int int_0 = 0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private static int int_5 = 0;
        private static int int_6 = 0;
        private Label lblAcctType;
        private Label lblAsk;
        private Label lblAsOf;
        private Label lblBid;
        private ToolStripLabel lblDataSets;
        private Label lblLast;
        private ToolStripLabel lblParameters;
        private Label lblPositions;
        private Label lblRange;
        private Label lblScale;
        private ToolStripLabel lblSpacing;
        private Label lblSymbol;
        private Label lblTradeAcct;
        private Label lblTradeAction;
        private Label lblTradeOrder;
        private Label lblTradePrice;
        private Label lblTradeQty;
        private Label lblTradeSymbol;
        private Label lblTradeTIF;
        private Label lbTradeDirected;
        private ToolStripLabel linkNewDataSet;
        private LinkLabel linkRerun;
        private LinkLabel linkResetParams;
        private LinkLabel linkSaveParams;
        private static List<MainForm> list_0 = new List<MainForm>();
        private List<string> list_1 = new List<string>();
        private static MainForm mainForm_0;
        private MenuStrip menuMain;
        private ToolStripMenuItem mniAbout;
        private ToolStripMenuItem mniAccounts;
        private ToolStripMenuItem mniCascade;
        private ToolStripMenuItem mniChartFront;
        private ToolStripMenuItem mniClose;
        private ToolStripMenuItem mniCloseWorkspace;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniCut;
        private ToolStripMenuItem mniDataManager;
        private ToolStripMenuItem mniDataWindow;
        private ToolStripMenuItem mniDebug;
        private ToolStripMenuItem mniDelete;
        private ToolStripMenuItem mniEdit;
        private ToolStripMenuItem mniExit;
        private ToolStripMenuItem mniFidelityCom;
        private ToolStripMenuItem mniFile;
        private ToolStripMenuItem mniFind;
        private ToolStripMenuItem mniFindReplace;
        private ToolStripMenuItem mniFundamentals;
        private ToolStripMenuItem mniHelp;
        private ToolStripMenuItem mniHomePage;
        private ToolStripMenuItem mniIndexManager;
        private ToolStripMenuItem mniIndicators;
        private ToolStripMenuItem mniLanguageGuide;
        private ToolStripMenuItem mniLoadWorkSpace;
        private ToolStripMenuItem mniLogin;
        private ToolStripMenuItem mniNavIcons;
        private ToolStripMenuItem mniNew;
        private ToolStripMenuItem mniNewBuilder;
        private ToolStripMenuItem mniNewChart;
        private ToolStripMenuItem mniNewChart2;
        private ToolStripMenuItem mniNewChartDD;
        private ToolStripMenuItem mniNewCode2;
        private ToolStripMenuItem mniNewCombinationStrategy;
        private ToolStripMenuItem mniNewDataSet;
        private ToolStripMenuItem mniNewDataSetTB;
        private ToolStripMenuItem mniNewEditor;
        private ToolStripMenuItem mniNewMultiStrategyBuilder;
        private ToolStripMenuItem mniNewMultiStrategyBuilder2;
        private ToolStripMenuItem mniNewQuote;
        private ToolStripMenuItem mniNewQuote2;
        private ToolStripMenuItem mniNewQuoteDD;
        private ToolStripMenuItem mniNewRules2;
        private ToolStripMenuItem mniNewStrategyRulesDD;
        private ToolStripMenuItem mniNewWorkspace2;
        private ToolStripMenuItem mniNewWorkspace3;
        private ToolStripMenuItem mniNewWorkspaceTB;
        private ToolStripMenuItem mniOnDemand;
        private ToolStripMenuItem mniOpenStrategy;
        private ToolStripMenuItem mniOpenStrategy2;
        private ToolStripMenuItem mniOpenWorkspace;
        private ToolStripMenuItem mniOrderManager;
        private ToolStripMenuItem mniPaste;
        private ToolStripMenuItem mniPreferences;
        private ToolStripMenuItem mniPreferences2;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniQuickRef;
        private ToolStripMenuItem mniQuickRef2;
        private ToolStripMenuItem mniQuoteAll;
        private ToolStripMenuItem mniSaveStrategy;
        private ToolStripMenuItem mniSaveStrategyAs;
        private ToolStripMenuItem mniSaveWorkSpace;
        private ToolStripMenuItem mniSelectAll;
        private ToolStripMenuItem mniSetDefaultWorkspace;
        private ToolStripMenuItem mniSetTemplate;
        private ToolStripMenuItem mniSoftwareUpgrade;
        private ToolStripMenuItem mniStorePreferredValues;
        private ToolStripMenuItem mniStrategyCenter;
        private ToolStripMenuItem mniStrategyRanking;
        private ToolStripMenuItem mniTileHorizontally;
        private ToolStripMenuItem mniTileVertically;
        private ToolStripMenuItem mniTools;
        private ToolStripMenuItem mniUndoDelete;
        private ToolStripMenuItem mniUserGuide;
        private ToolStripMenuItem mniView;
        private ToolStripMenuItem mniViewDataPanel;
        private ToolStripMenuItem mniViewDrawingBar;
        private ToolStripMenuItem mniViewNavBar;
        private ToolStripMenuItem mniViewStatusBar;
        private ToolStripMenuItem mniViewToolbar;
        private ToolStripMenuItem mniViewTradeTicket;
        private ToolStripMenuItem mniWealthLabCom;
        private ToolStripMenuItem mniWindow;
        private ToolStripMenuItem mniWorkspaces;
        private ToolStripMenuItem newStrategyFromCodeToolStripMenuItem;
        private NumEdit numTradePrice;
        private NumEdit numTradeQty;
        private OpenFileDialog openFileDialog_0;
        private PageSettings pageSettings_0 = new PageSettings();
        private PageSetupDialog pageSetupDialog_0;
        internal ParameterSlidersContainer paramSliders;
        private Panel pnlParamBase;
        private Panel pnlParamBaseLinkParams;
        private Panel pnlTrade;
        private Panel pnlTree;
        private ContextMenuStrip popupPreferredValues;
        private PositionSizeSelecter posSize;
        private PrintPreviewDialog printPreviewDialog;
        private SaveFileDialog saveFileDialog_0;
        private ScaleSelecter scale;
        private ToolStripSeparator sepBarSpacing;
        private ToolStripSeparator sepChart;
        private ToolStripSeparator sepChart2;
        private ToolStripSeparator sepChartFile;
        private ToolStripSeparator sepChartFront;
        private ToolStripSeparator sepChartStyles;
        private ToolStripSeparator sepDataManager;
        private ToolStripSeparator sepDeleteDrawing;
        private ToolStripSeparator sepExit;
        private ToolStripSeparator sepHelp;
        private ToolStripSeparator sepHelp2;
        private ToolStripSeparator sepIndicators;
        private ToolStripSeparator sepNew;
        private ToolStripSeparator sepNew2;
        private ToolStripSeparator sepNewQuote;
        private ToolStripSeparator sepNewStrategy;
        private ToolStripSeparator sepPreferences;
        private ToolStripSeparator sepQuickRef;
        private ToolStripSeparator sepQuote2;
        private ToolStripSeparator sepQuoteFront;
        private ToolStripSeparator sepSave;
        private ToolStripSeparator sepSaveWorkspace;
        private ToolStripSeparator sepSelectAll;
        private ToolStripSeparator sepSpacing;
        private ToolStripSeparator sepTools;
        private ToolStripSeparator sepUpgrade;
        private ToolStripSeparator sepView;
        private SplitContainer splitContainerDataPane;
        private Splitter splitter;
        private StatusStrip status;
        private ToolStripStatusLabel statusActive;
        private ToolStripProgressBar statusDownloadProgressBar;
        private ToolStripStatusLabel statusMessage;
        private ToolStripStatusLabel statusOrders;
        private ToolStripStatusLabel statusSofwareDownload;
        private ToolStripStatusLabel statusStreamingProvider;
        private ToolStripStatusLabel statusStreamingStatus;
        private ToolStripStatusLabel statusStreamingSymbolsOff;
        private ToolStripStatusLabel statusStreamingSymbolsOn;
        private ToolStripStatusLabel stlblHolder;
        private StreamingQuoteManager streamingQuoteManager_0;
        private string string_0;
        private ToolStripMenuItem symbolInfoManagerToolStripMenuItem;
        private System.Windows.Forms.Timer timer_0;
        private System.Windows.Forms.Timer timer_1;
        private System.Windows.Forms.Timer timer_2;
        private ToolStrip toolbar;
        private ToolStrip toolbarDataSets;
        private ToolStrip toolbarDrawing;
        private ToolStrip toolbarNav;
        private ToolStrip toolbarParameters;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator7;
        private DataSourceTreeView treeDataSources;
        private ToolStripLabel tslblChartStyles;
        private ToolStripLabel tslblOptions;
        private ToolStripDropDownButton tsmMoreChartStyles;
        private TextBox txtTradeSymbol;

        public MainForm()
        {
            this.InitializeComponent();
            MainModule.Instance.HelpProvider.SetHelpNavigator(this.pnlTree, HelpNavigator.Topic);
            MainModule.Instance.HelpProvider.SetHelpKeyword(this.pnlTree, "datapanelorientation.htm");
            this.pageSetupDialog_0.PageSettings = this.pageSettings_0;
        }

        public void ActivateMdiChild()
        {
            this.MainForm_MdiChildActivate(this, EventArgs.Empty);
        }

        public void AddDynamicMenuItem(DynamicMenuItem dynamicMenuItem_0)
        {
            IEnumerator enumerator = this.menuMain.Items.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ToolStripItem current = (ToolStripItem)enumerator.Current;
                        if (current is ToolStripMenuItem)
                        {
                            ToolStripMenuItem toolStripMenuItem = current as ToolStripMenuItem;
                            if (current.Text == dynamicMenuItem_0.MainMenuItemText)
                            {
                                for (int i = 0; i < toolStripMenuItem.DropDownItems.Count; i++)
                                {
                                    if (toolStripMenuItem.DropDownItems[i] is ToolStripMenuItem)
                                    {
                                        ToolStripMenuItem item = toolStripMenuItem.DropDownItems[i] as ToolStripMenuItem;
                                        if (item.Text == dynamicMenuItem_0.SubMenuItemText)
                                        {
                                            ToolStripMenuItem itemImage = new ToolStripMenuItem(dynamicMenuItem_0.Text);
                                            itemImage.Click += new EventHandler(dynamicMenuItem_0.OnClick.Invoke);
                                            if (dynamicMenuItem_0.ItemImage != null)
                                            {
                                                itemImage.Image = dynamicMenuItem_0.ItemImage;
                                            }
                                            if (toolStripMenuItem.DropDownItems[i - 1].Text == dynamicMenuItem_0.Text)
                                            {
                                                goto Label0;
                                            }
                                            toolStripMenuItem.DropDownItems.Insert(i, itemImage);
                                            goto Label0;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            Label0: return;
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public void AddWorkspaceMenuItem(string workspace)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(workspace);
            item.Click += new EventHandler(this.method_40);
            this.mniWorkspaces.DropDownItems.Add(item);
        }

        private void btnAcctsPositions_Click(object sender, EventArgs e)
        {
            if (AccountsPositionsForm.Instance != null)
            {
                AccountsPositionsForm.Instance.MyMainForm.BringToFront();
                AccountsPositionsForm.Instance.BringToFront();
                AccountsPositionsForm.Instance.WindowState = FormWindowState.Normal;
            }
            else
            {
                new AccountsPositionsForm { MdiParent = this }.Show();
            }
        }

        private void btnClearDrawingObjects_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.ClearDrawingObjects();
            }
        }

        private void btnClearIndicators_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.method_5();
            }
        }

        private void btnCloseTradeTicket_Click(object sender, EventArgs e)
        {
            this.mniViewTradeTicket.Checked = !this.mniViewTradeTicket.Checked;
            this.btnTradeTicket.Checked = this.mniViewTradeTicket.Checked;
            this.pnlTrade.Visible = this.mniViewTradeTicket.Checked;
            MainModule.Instance.Settings.Set("ShowTradeTicket", this.mniViewTradeTicket.Checked);
        }

        public void btnCrossHair_Click(object sender, EventArgs e)
        {
            if (sender == null || (sender as ToolStripItem).Name == "btnCrossHair")
            {
                this.btnCrossHair.Checked = !this.btnCrossHair.Checked;
            }
            Chart.DisplayCrossHair = this.btnCrossHair.Checked;
            foreach (Form form in base.MdiChildren)
            {
                if (form is ChartForm)
                {
                    (form as ChartForm).RefreshChart();
                }
            }
        }

        private void btnDataManager_Click(object sender, EventArgs e)
        {
            this.CreateDataManager();
        }

        private void btnDataWindow_Click(object sender, EventArgs e)
        {
            if (DataWindowForm.Instance == null)
            {
                new DataWindowForm().Show(this);
            }
            else
            {
                DataWindowForm.Instance.Close();
            }
        }

        private void btnDecreaseSpacing_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.DecreaseBarSpacing();
            }
        }

        private void btnDockDown_Click(object sender, EventArgs e)
        {
            this.pnlTrade.Dock = DockStyle.Bottom;
            this.btnDockDown.Visible = false;
            this.btnDockUp.Visible = true;
            MainModule.Instance.Settings.Set("TradeTicketDockBottom", true);
        }

        private void btnDockUp_Click(object sender, EventArgs e)
        {
            this.pnlTrade.Dock = DockStyle.Top;
            this.btnDockUp.Visible = false;
            this.btnDockDown.Visible = true;
            MainModule.Instance.Settings.Set("TradeTicketDockBottom", false);
        }

        private void btnFundamental_VisibleChanged(object sender, EventArgs e)
        {
            if (this.btnFundamental.Visible && !this.bool_6)
            {
                this.btnFundamental.Visible = this.bool_6;
            }
        }

        private void btnFundamentalsTB2_Click(object sender, EventArgs e)
        {
            if (FundamentalsForm.Instance != null)
            {
                FundamentalsForm.Instance.BringToFront();
            }
            else
            {
                new FundamentalsForm().Show();
            }
        }

        private void btnFundamentalsTB2_VisibleChanged(object sender, EventArgs e)
        {
            if (this.btnFundamentalsTB2.Visible && !this.bool_6)
            {
                this.btnFundamentalsTB2.Visible = this.bool_6;
            }
        }

        private void btnFundamentalsVisible_Click(object sender, EventArgs e)
        {
            this.btnFundamentalsVisible.Checked = !this.btnFundamentalsVisible.Checked;
            MainModule.Instance.Settings.Set("FundamentalItemsVisible", this.btnFundamentalsVisible.Checked);
            if ((Control.ModifierKeys & Keys.Alt) > Keys.None)
            {
                foreach (Form form in base.MdiChildren)
                {
                    if (form is ChartForm)
                    {
                        ChartForm form2 = form as ChartForm;
                        form2.Renderer.FundamentalsVisible = this.btnFundamentalsVisible.Checked;
                        form2.RefreshChart();
                    }
                }
            }
            else
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if (activeChartWindow != null)
                {
                    activeChartWindow.Renderer.FundamentalsVisible = this.btnFundamentalsVisible.Checked;
                    activeChartWindow.RefreshChart();
                }
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (base.ActiveMdiChild != null)
            {
                if (base.ActiveMdiChild is ChartForm)
                {
                    ChartForm activeChartWindow = this.ActiveChartWindow;
                    if (activeChartWindow != null)
                    {
                        if ((activeChartWindow.DataSource == null) && (this.treeDataSources.Nodes.Count > 0))
                        {
                            activeChartWindow.DataSource = (WealthLab.DataSource) this.treeDataSources.Nodes[0].Tag;
                        }
                        activeChartWindow.ResetStreaming();
                        activeChartWindow.GoButtonPressed(this.cmbSymbol.Text, true);
                        this.linkRerun.Visible = false;
                        this.treeDataSources.SelectSymbol(activeChartWindow.DataSource, this.cmbSymbol.Text);
                    }
                }
                else if (base.ActiveMdiChild is StrategyRanking)
                {
                    StrategyRanking activeMdiChild = base.ActiveMdiChild as StrategyRanking;
                    if (activeMdiChild != null)
                    {
                        activeMdiChild.ExecuteRankings();
                    }
                }
                this.method_7();
                if (!this.cmbSymbol.Items.Contains(this.cmbSymbol.Text))
                {
                    this.cmbSymbol.Items.Add(this.cmbSymbol.Text);
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string keyword = "introduction.htm";
            if (base.ActiveMdiChild != null)
            {
                string name = base.ActiveMdiChild.Name;
                if (name != null)
                {
                    int num2;
                    if (Class62.dictionary_1 == null)
                    {
                        Dictionary<string, int> dictionary1 = new Dictionary<string, int>(9);
                        dictionary1.Add("ChartForm", 0);
                        dictionary1.Add("HomeForm", 1);
                        dictionary1.Add("DataManagerForm", 2);
                        dictionary1.Add("StrategyCenterForm", 3);
                        dictionary1.Add("QuotesForm", 4);
                        dictionary1.Add("AccountsPositionsForm", 5);
                        dictionary1.Add("OrdersAlertsForm", 6);
                        dictionary1.Add("SymbolManagerForm", 7);
                        dictionary1.Add("IndexManagerForm", 8);
                        Class62.dictionary_1 = dictionary1;
                    }
                    if (Class62.dictionary_1.TryGetValue(name, out num2))
                    {
                        switch (num2)
                        {
                            case 0:
                            {
                                ChartForm activeMdiChild = base.ActiveMdiChild as ChartForm;
                                string currentTabName = activeMdiChild.CurrentTabName;
                                if (currentTabName != null)
                                {
                                    int num;
                                    if (Class62.dictionary_2 == null)
                                    {
                                        Dictionary<string, int> dictionary2 = new Dictionary<string, int>(12);
                                        dictionary2.Add("Chart", 0);
                                        dictionary2.Add("Editor", 1);
                                        dictionary2.Add("Strategy Summary", 2);
                                        dictionary2.Add("Rules", 3);
                                        dictionary2.Add("Performance", 4);
                                        dictionary2.Add("By Symbol", 5);
                                        dictionary2.Add("Trades", 6);
                                        dictionary2.Add("Equity Curve", 7);
                                        dictionary2.Add("Drawdown", 8);
                                        dictionary2.Add("Profit Distribution", 9);
                                        dictionary2.Add("By Period", 10);
                                        dictionary2.Add("MAE/MFE", 11);
                                        Class62.dictionary_2 = dictionary2;
                                    }
                                    if (Class62.dictionary_2.TryGetValue(currentTabName, out num))
                                    {
                                        switch (num)
                                        {
                                            case 0:
                                                if (!activeMdiChild.HasEditor && ((activeMdiChild.Strategy == null) || (activeMdiChild.Strategy.StrategyType != StrategyType.Rules)))
                                                {
                                                    keyword = "Charting.htm";
                                                }
                                                else
                                                {
                                                    keyword = "strategywindow.htm";
                                                }
                                                goto Label_0309;

                                            case 1:
                                                keyword = "editor.htm";
                                                goto Label_0309;

                                            case 2:
                                                keyword = "strategysummary.htm";
                                                goto Label_0309;

                                            case 3:
                                                keyword = "rules_view.htm";
                                                goto Label_0309;

                                            case 4:
                                                keyword = "performance.htm";
                                                goto Label_0309;

                                            case 5:
                                                keyword = "bysymbol.htm";
                                                goto Label_0309;

                                            case 6:
                                                keyword = "trades.htm";
                                                goto Label_0309;

                                            case 7:
                                                keyword = "equitycurve.htm";
                                                goto Label_0309;

                                            case 8:
                                                keyword = "drawdown.htm";
                                                goto Label_0309;

                                            case 9:
                                                keyword = "profitdistribution.htm";
                                                goto Label_0309;

                                            case 10:
                                                keyword = "byperiod.htm";
                                                goto Label_0309;

                                            case 11:
                                                keyword = "maemfe.htm";
                                                goto Label_0309;
                                        }
                                    }
                                }
                                if (activeMdiChild.CurrentTabName.Contains("Alert"))
                                {
                                    keyword = "alertview.htm";
                                }
                                keyword = "strategywindow.htm";
                                goto Label_0309;
                            }
                            case 1:
                                keyword = "home_page.htm";
                                goto Label_0309;

                            case 2:
                                keyword = "datamanager.htm";
                                goto Label_0309;

                            case 3:
                                keyword = "strategymonitor.htm";
                                goto Label_0309;

                            case 4:
                                keyword = "quotes.htm";
                                goto Label_0309;

                            case 5:
                                keyword = "accountbalancespos.htm";
                                goto Label_0309;

                            case 6:
                                keyword = "orders.htm";
                                goto Label_0309;

                            case 7:
                                keyword = "symbol_info_manager.htm";
                                goto Label_0309;

                            case 8:
                                keyword = "INDEXLAB.htm";
                                goto Label_0309;
                        }
                    }
                }
                keyword = "introduction.htm";
            }
        Label_0309:
            MainModule.Instance.ContextSensitiveHelp(keyword);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (HomeForm.Instance != null)
            {
                HomeForm.Instance.MyMainForm.BringToFront();
                HomeForm.Instance.BringToFront();
                HomeForm.Instance.WindowState = FormWindowState.Normal;
            }
            else
            {
                new HomeForm { MdiParent = this }.Show();
            }
        }

        private void btnIncreaseSpacing_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.IncreaseBarSpacing();
            }
        }

        private void btnIndicatorsTB2_Click(object sender, EventArgs e)
        {
            if (IndicatorsForm.Instance != null)
            {
                IndicatorsForm.Instance.BringToFront();
            }
            else
            {
                new IndicatorsForm().Show();
            }
        }

        private void btnLabelsVisible_Click(object sender, EventArgs e)
        {
            this.btnLabelsVisible.Checked = !this.btnLabelsVisible.Checked;
            MainModule.Instance.Settings.Set("IndicatorLabelsVisible", this.btnLabelsVisible.Checked);
            if ((Control.ModifierKeys & Keys.Alt) > Keys.None)
            {
                foreach (Form form in base.MdiChildren)
                {
                    if (form is ChartForm)
                    {
                        ChartForm form2 = form as ChartForm;
                        form2.Renderer.IndicatorLabelsVisible = this.btnLabelsVisible.Checked;
                        form2.RefreshChart();
                    }
                }
            }
            else
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if (activeChartWindow != null)
                {
                    activeChartWindow.Renderer.IndicatorLabelsVisible = this.btnLabelsVisible.Checked;
                    activeChartWindow.RefreshChart();
                }
            }
        }

        private void btnLinear_Click(object sender, EventArgs e)
        {
            MainModule.Instance.Renderer.LogScale = false;
            this.btnLinear.Checked = true;
            this.btnLog.Checked = false;
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.Renderer.LogScale = false;
                activeChartWindow.RefreshChart();
            }
        }

        private void btnLineChart_Click(object sender, EventArgs e)
        {
            this.btnBarChart.Checked = false;
            this.btnCandleStyle.Checked = false;
            this.btnLineChart.Checked = false;
            foreach (ToolStripItem item in this.tsmMoreChartStyles.DropDownItems)
            {
                ToolStripButton button = item as ToolStripButton;
                if (button != null)
                {
                    button.Checked = false;
                }
            }
            (sender as ToolStripButton).Checked = true;
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                ChartStyle style = this.method_5();
                if ((activeChartWindow.Renderer.ChartStyle == null) || (style.FriendlyName != activeChartWindow.Renderer.ChartStyle.FriendlyName))
                {
                    activeChartWindow.ChartStyle = style;
                    MainModule.Instance.Settings.Set("ChartStyleSelected", activeChartWindow.ChartStyle.FriendlyName);
                    activeChartWindow.UpdateChartColorsAndStyle(true);
                }
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            MainModule.Instance.Renderer.LogScale = true;
            this.btnLinear.Checked = false;
            this.btnLog.Checked = true;
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.Renderer.LogScale = true;
                activeChartWindow.RefreshChart();
            }
        }

        private void btnLoginTradeTicket_Click(object sender, EventArgs e)
        {
            if (!MainModule.Instance.IsAuthenticated)
            {
                MainModule.Instance.Authenticate();
            }
            else
            {
                StreamingDataProvider streamingProvider = MainModule.Instance.StreamingProvider;
                if ((streamingProvider != null) && streamingProvider.IsConnected)
                {
                    streamingProvider.DisconnectStreaming(this);
                }
                MainModule.Instance.UnAuthenticate();
            }
        }

        private void btnOpenStrategy_Click(object sender, EventArgs e)
        {
            this.OpenStrategyExplorer();
        }

        private void btnOrdersAlerts_Click(object sender, EventArgs e)
        {
            this.OpenOrderManager();
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            Alert alert = this.method_25();
            MainModule.Instance.TradeManager.AddAlert(alert, true, false);
        }

        private void btnPreferencesTB_Click(object sender, EventArgs e)
        {
            new PreferencesForm().ShowDialog();
        }

        private void btnPushCode_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.method_6();
            }
        }

        private void btnRestoreSpacing_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.RestoreBarSpacing();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.SaveStrategy();
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.SaveStrategyAs();
            }
        }

        private void btnStageOrder_Click(object sender, EventArgs e)
        {
            Alert alert = this.method_25();
            MainModule.Instance.TradeManager.AddAlert(alert, false, false);
        }

        private void btnStatusBarVisible_Click(object sender, EventArgs e)
        {
            this.btnStatusBarVisible.Checked = !this.btnStatusBarVisible.Checked;
            MainModule.Instance.Settings.Set("StatusBarVisible", this.btnStatusBarVisible.Checked);
            if ((Control.ModifierKeys & Keys.Alt) > Keys.None)
            {
                foreach (Form form in base.MdiChildren)
                {
                    if (form is ChartForm)
                    {
                        ChartForm form2 = form as ChartForm;
                        form2.StatusPanelsVisible = this.btnStatusBarVisible.Checked;
                    }
                }
            }
            else
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if (activeChartWindow != null)
                {
                    activeChartWindow.StatusPanelsVisible = this.btnStatusBarVisible.Checked;
                }
            }
        }

        private void btnStrategyCenter_Click(object sender, EventArgs e)
        {
            this.OpenStrategyCenter();
        }

        private void btnTradeTicket_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.btnTradeTicket.Checked)
            {
                this.btnTradeTicket.Text = "Hide Trade Ticket";
            }
            else
            {
                this.btnTradeTicket.Text = "Show Trade Ticket";
            }
        }

        private void btnTradeTicket_Click(object sender, EventArgs e)
        {
            this.mniViewTradeTicket.Checked = !this.mniViewTradeTicket.Checked;
            this.btnTrade.Checked = this.mniViewTradeTicket.Checked;
            this.pnlTrade.Visible = this.mniViewTradeTicket.Checked;
            this.btnTradeTicket.Checked = this.mniViewTradeTicket.Checked;
            MainModule.Instance.Settings.Set("ShowTradeTicket", this.mniViewTradeTicket.Checked);
        }

        public void BuildParameterSliders()
        {
            WealthScript wealthScript = null;
            this.linkRerun.Visible = false;
            Form activeMdiChild = base.ActiveMdiChild;
            if (activeMdiChild is IWealthScriptProvider)
            {
                IWealthScriptProvider provider = activeMdiChild as IWealthScriptProvider;
                wealthScript = provider.WealthScript;
            }
            this.paramSliders.WealthScript = wealthScript;
            if (wealthScript != null)
            {
                this.linkSaveParams.Visible = wealthScript.Parameters.Count > 0;
                this.linkResetParams.Visible = this.linkSaveParams.Visible;
                this.linkSaveParams.Enabled = false;
                this.linkResetParams.Enabled = false;
            }
            else
            {
                this.linkSaveParams.Visible = false;
                this.linkResetParams.Visible = false;
            }
            this.EnableSliders();
            if (this.paramSliders.Enabled)
            {
                if (activeMdiChild is IWealthScriptProvider)
                {
                    if ((activeMdiChild as IWealthScriptProvider).ParametersNeedSave)
                    {
                        this.lblParameters.ForeColor = Color.Red;
                        this.method_35(true);
                    }
                    else
                    {
                        this.lblParameters.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                        this.method_35(false);
                    }
                }
                else
                {
                    this.lblParameters.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                    this.method_35(false);
                }
            }
        }

        public void ChangeScale(BarScale scale, int barInterval)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                if ((activeChartWindow.Strategy == null) || (activeChartWindow.Strategy.StrategyType != StrategyType.CombinedStrategy))
                {
                    activeChartWindow.BarDataScale = new WealthLab.BarDataScale(scale, barInterval);
                    activeChartWindow.ResetStreaming();
                    activeChartWindow.GoButtonPressed(this.cmbSymbol.Text, false);
                }
            }
            else if (base.ActiveMdiChild is StrategyRanking)
            {
                StrategyRanking activeMdiChild = base.ActiveMdiChild as StrategyRanking;
                activeMdiChild.BarInterval = barInterval;
                activeMdiChild.BarScale = scale;
                activeMdiChild.BarDataScale = new WealthLab.BarDataScale(scale, barInterval);
            }
            this.method_17();
        }

        public void ClearDrawingObjectSelectedTool()
        {
            foreach (ToolStripItem item in this.toolbarDrawing.Items)
            {
                if (item is ToolStripButton)
                {
                    ToolStripButton button = item as ToolStripButton;
                    if (button.Checked)
                    {
                        button.Checked = false;
                    }
                }
            }
            this.method_12(Cursors.Default);
        }

        public void CloseAndReopenStrategy(ChartForm chartForm_1, Strategy strategy_0)
        {
            Rectangle bounds = chartForm_1.Bounds;
            chartForm_1.Close();
            ChartForm form = this.OpenStrategyWindow(strategy_0);
            form.SelectTab("Editor");
            form.Bounds = bounds;
        }

        private void cmbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.method_41();
            this.cmbTradeTIF_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void cmbSymbol_DropDownClosed(object sender, EventArgs e)
        {
            int selectedIndex = this.cmbSymbol.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.cmbSymbol.Text = (string) this.cmbSymbol.Items[selectedIndex];
                this.btnGo.PerformClick();
            }
        }

        private void cmbSymbol_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }

        private void cmbTradeAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainModule.Instance.BrokerProvider != null)
            {
                BrokerProvider brokerProvider = MainModule.Instance.BrokerProvider;
                string text = this.cmbTradeRoute.Text;
                string action = this.cmbTradeAction.Text;
                string str3 = this.cmbTradeOrder.Text;
                this.cmbTradeOrder.Items.Clear();
                if (brokerProvider.AllowOrderTypeForRoute(text, OrderType.Market))
                {
                    this.cmbTradeOrder.Items.Add("Market");
                }
                if (brokerProvider.AllowOrderTypeForRoute(text, OrderType.Limit))
                {
                    this.cmbTradeOrder.Items.Add("Limit");
                }
                if (brokerProvider.AllowOrderTypeForRoute(text, OrderType.Stop))
                {
                    this.cmbTradeOrder.Items.Add("Stop");
                }
                foreach (string str4 in brokerProvider.ExtendedOrderTypesAllowed(this.cmbAccount.Text, text, action))
                {
                    this.cmbTradeOrder.Items.Add(str4);
                }
                this.cmbTradeOrder.SelectedIndex = this.cmbTradeOrder.Items.IndexOf(str3);
                if (this.cmbTradeOrder.SelectedIndex == -1)
                {
                    this.cmbTradeOrder.SelectedIndex = 0;
                }
                this.accountTypeSelector1.InitAccountTradeType(this.cmbAccount.Text, this.cmbTradeAction.Text);
            }
            this.cmbTradeTIF_SelectedIndexChanged(this, e);
        }

        private void cmbTradeOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainModule.Instance.BrokerProvider != null)
            {
                BrokerProvider brokerProvider = MainModule.Instance.BrokerProvider;
                string text = this.cmbTradeRoute.Text;
                string str = this.cmbTradeTIF.Text;
                this.cmbTradeTIF.Items.Clear();
                foreach (string str2 in brokerProvider.TifsAllowed(this.cmbAccount.Text, this.cmbTradeRoute.Text, this.cmbTradeOrder.Text))
                {
                    this.cmbTradeTIF.Items.Add(str2);
                }
                this.cmbTradeTIF.SelectedIndex = this.cmbTradeTIF.Items.IndexOf(str);
                if ((this.cmbTradeTIF.SelectedIndex == -1) && (this.cmbTradeTIF.Items.Count > 0))
                {
                    this.cmbTradeTIF.SelectedIndex = 0;
                }
                string orderType = this.cmbTradeOrder.Text;
                if (brokerProvider.AllowDecimalsForExtendedOrderType(orderType))
                {
                    this.numTradePrice.InputType = NumEdit.NumEditType.Double;
                }
                else
                {
                    this.numTradePrice.InputType = NumEdit.NumEditType.Integer;
                    if (this.numTradePrice.Value != ((int) this.numTradePrice.Value))
                    {
                        this.numTradePrice.Text = ((int) this.numTradePrice.Value).ToString();
                    }
                }
            }
            this.cmbTradeTIF_SelectedIndexChanged(this, e);
        }

        private void cmbTradeRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainModule.Instance.BrokerProvider != null)
            {
                BrokerProvider brokerProvider = MainModule.Instance.BrokerProvider;
                string text = this.cmbTradeRoute.Text;
                string action = this.cmbTradeAction.Text;
                string str3 = this.cmbTradeTIF.Text;
                this.cmbTradeTIF.Items.Clear();
                foreach (string str4 in brokerProvider.TifsAllowed(this.cmbAccount.Text, this.cmbTradeRoute.Text, this.cmbTradeOrder.Text))
                {
                    this.cmbTradeTIF.Items.Add(str4);
                }
                this.cmbTradeTIF.SelectedIndex = this.cmbTradeTIF.Items.IndexOf(str3);
                if ((this.cmbTradeTIF.SelectedIndex == -1) && (this.cmbTradeTIF.Items.Count > 0))
                {
                    this.cmbTradeTIF.SelectedIndex = 0;
                }
                string str5 = this.cmbTradeOrder.Text;
                this.cmbTradeOrder.Items.Clear();
                if (brokerProvider.AllowOrderTypeForRoute(text, OrderType.Market))
                {
                    this.cmbTradeOrder.Items.Add("Market");
                }
                if (brokerProvider.AllowOrderTypeForRoute(text, OrderType.Limit))
                {
                    this.cmbTradeOrder.Items.Add("Limit");
                }
                if (brokerProvider.AllowOrderTypeForRoute(text, OrderType.Stop))
                {
                    this.cmbTradeOrder.Items.Add("Stop");
                }
                foreach (string str6 in brokerProvider.ExtendedOrderTypesAllowed(this.cmbAccount.Text, text, action))
                {
                    this.cmbTradeOrder.Items.Add(str6);
                }
                this.cmbTradeOrder.SelectedIndex = this.cmbTradeOrder.Items.IndexOf(str5);
                if (this.cmbTradeOrder.SelectedIndex == -1)
                {
                    this.cmbTradeOrder.SelectedIndex = 0;
                }
            }
            this.cmbTradeTIF_SelectedIndexChanged(this, e);
        }

        private void cmbTradeTIF_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = false;
            if (((((this.cmbAccount.SelectedIndex >= 0) && (this.txtTradeSymbol.Text != "")) && ((this.cmbTradeAction.SelectedIndex >= 0) && (this.numTradeQty.Value > 0M))) && ((this.cmbTradeOrder.SelectedIndex >= 0) && ((this.cmbTradeOrder.Text == "Market") || (this.numTradePrice.Value > 0M)))) && ((this.cmbTradeRoute.SelectedIndex >= 0) && (this.cmbTradeTIF.SelectedIndex >= 0)))
            {
                flag = true;
            }
            this.btnPlaceOrder.Enabled = flag;
            this.btnStageOrder.Enabled = flag;
            this.numTradePrice.Enabled = this.cmbTradeOrder.Text != "Market";
        }

        public void Connect()
        {
            base.Invoke(new Delegate39(this.method_30), new object[] { false });
        }

        public void Connect(bool reconnect)
        {
            base.Invoke(new Delegate39(this.method_30), new object[] { reconnect });
            this.bool_8 = false;
        }

        public ChartForm CreateChartWindow(bool selectNode)
        {
            ChartForm form = new ChartForm {
                MdiParent = this
            };
            string str = MainModule.Instance.Settings.Get("ChartStyleSelected", "");
            if (str != "")
            {
                form.ChartStyle = this.method_6(str);
            }
            else
            {
                form.ChartStyle = this.method_5();
            }
            form.Renderer.LogScale = MainModule.Instance.Renderer.LogScale;
            form.DataSourceSelected(this.DataSource);
            form.PositionSize = this.posSize.PositionSize;
            form.DataRange = this.dataRange.DataRange;
            form.BarDataScale = this.scale.DataScale;
            form.SetBarDataScaleForDataSource(this.DataSource, this.scale.DataScale);
            form.Renderer.IndicatorLabelsVisible = this.btnLabelsVisible.Checked;
            form.StatusPanelsVisible = this.btnStatusBarVisible.Checked;
            form.Renderer.FundamentalsVisible = this.btnFundamentalsVisible.Checked;
            form.Show();
            if (selectNode)
            {
                this.SelectingNodeForFormCreation = true;
                this.SelectNode();
                this.treeDataSources.SelectSymbol(form.DataSource, form.Symbol);
            }
            return form;
        }

        public DataManagerForm CreateDataManager()
        {
            if (DataManagerForm.Instance == null)
            {
                DataManagerForm form = new DataManagerForm {
                    MdiParent = this
                };
                form.Show();
                return form;
            }
            DataManagerForm.Instance.MdiParent.BringToFront();
            DataManagerForm.Instance.BringToFront();
            DataManagerForm.Instance.WindowState = FormWindowState.Normal;
            return DataManagerForm.Instance;
        }

        public IndexManagerForm CreateIndexManager()
        {
            if (IndexManagerForm.Instance == null)
            {
                IndexManagerForm form = new IndexManagerForm {
                    MdiParent = this
                };
                form.Show();
                return form;
            }
            IndexManagerForm.Instance.MdiParent.BringToFront();
            IndexManagerForm.Instance.BringToFront();
            IndexManagerForm.Instance.WindowState = FormWindowState.Normal;
            return IndexManagerForm.Instance;
        }

        public ChartForm CreateNewMultiStrategyWindow()
        {
            Strategy strategy = new Strategy {
                StrategyType = StrategyType.CombinedStrategy
            };
            ChartForm form = this.CreateChartWindow(true);
            form.Strategy = strategy;
            this.MainForm_MdiChildActivate(this, EventArgs.Empty);
            form.SelectTab("Combination Strategy");
            return form;
        }

        public ChartForm CreateNewStrategyWindow(bool editor)
        {
            Strategy strategy = new Strategy();
            if (editor)
            {
                strategy.StrategyType = StrategyType.Script;
                strategy.Code = MainModule.Instance.StrategyTemplateCode;
                strategy.References = MainModule.Instance.Settings.Get("TemplateReferences", "");
            }
            else
            {
                strategy.StrategyType = StrategyType.Rules;
            }
            ChartForm form = this.CreateChartWindow(true);
            form.Strategy = strategy;
            this.MainForm_MdiChildActivate(this, EventArgs.Empty);
            if (editor)
            {
                form.SelectTab("Editor");
                return form;
            }
            form.SelectTab("Rules");
            return form;
        }

        public StrategyCenterForm CreateStrategyCenter()
        {
            if (StrategyCenterForm.Instance == null)
            {
                StrategyCenterForm.Instance = new StrategyCenterForm();
                StrategyCenterForm.Instance.MdiParent = this;
                StrategyCenterForm.Instance.Show();
            }
            StrategyCenterForm.Instance.BringToFront();
            StrategyCenterForm.Instance.WindowState = FormWindowState.Normal;
            return StrategyCenterForm.Instance;
        }

        public void Disconnect()
        {
            if (!this.bool_7)
            {
                base.Invoke(new Delegate40(this.method_31));
            }
            this.bool_8 = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public void EnableControls(bool enable)
        {
            this.btnGo.Enabled = enable;
            this.toolbar.Enabled = enable;
        }

        public void EnableEditAndPrintMenu()
        {
            if (base.ActiveMdiChild == null)
            {
                this.mniEdit.Enabled = false;
                this.mniPrint.Enabled = false;
            }
            else if (base.ActiveMdiChild is AccountsPositionsForm)
            {
                this.mniPrint.Enabled = true;
                this.method_2();
            }
            else if (base.ActiveMdiChild is OrdersAlertsForm)
            {
                this.mniPrint.Enabled = true;
                this.method_2();
            }
            else if (base.ActiveMdiChild is StrategyCenterForm)
            {
                this.mniPrint.Enabled = true;
                this.method_2();
            }
            else if (base.ActiveMdiChild is QuotesForm)
            {
                this.mniPrint.Enabled = true;
                this.method_2();
            }
            else if (base.ActiveMdiChild is StrategyRanking)
            {
                this.mniPrint.Enabled = true;
                this.method_2();
            }
            else
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if (activeChartWindow == null)
                {
                    this.mniEdit.Enabled = false;
                    this.mniPrint.Enabled = false;
                }
                else if (activeChartWindow.CurrentTabName == "Editor")
                {
                    this.mniEdit.Enabled = true;
                    this.mniPrint.Enabled = true;
                    this.mniUndoDelete.Enabled = true;
                    this.mniCut.Enabled = true;
                    this.mniCopy.Enabled = true;
                    this.mniPaste.Enabled = true;
                    this.mniDelete.Enabled = true;
                    this.mniSelectAll.Enabled = true;
                    this.mniFind.Enabled = true;
                    this.mniFindReplace.Enabled = true;
                    this.mniSetTemplate.Enabled = true;
                }
                else if (activeChartWindow.CurrentTabName == "Chart")
                {
                    this.mniEdit.Enabled = true;
                    this.mniUndoDelete.Enabled = false;
                    this.mniCut.Enabled = false;
                    this.mniCopy.Enabled = true;
                    this.mniPaste.Enabled = false;
                    this.mniDelete.Enabled = false;
                    this.mniSelectAll.Enabled = false;
                    this.mniFind.Enabled = false;
                    this.mniFindReplace.Enabled = false;
                    this.mniSetTemplate.Enabled = false;
                    this.mniPrint.Enabled = true;
                }
                else if (activeChartWindow.CurrentTabName.Contains("Alert"))
                {
                    this.mniPrint.Enabled = true;
                    this.method_2();
                }
                else if (activeChartWindow.CurrentTabName.Contains("Strategy Summary"))
                {
                    this.mniPrint.Enabled = true;
                    this.method_2();
                }
                else
                {
                    IPerformanceVisualizer selectedVisualizer = activeChartWindow.SelectedVisualizer;
                    if (selectedVisualizer == null)
                    {
                        this.mniEdit.Enabled = false;
                        this.mniPrint.Enabled = false;
                    }
                    else
                    {
                        this.mniUndoDelete.Enabled = false;
                        this.mniCut.Enabled = false;
                        this.mniCopy.Enabled = selectedVisualizer.SupportClipboardCopy;
                        this.mniEdit.Enabled = this.mniCopy.Enabled;
                        this.mniPrint.Enabled = selectedVisualizer.SupportsPrint;
                        this.mniPaste.Enabled = false;
                        this.mniDelete.Enabled = false;
                        this.mniSelectAll.Enabled = false;
                        this.mniFind.Enabled = false;
                        this.mniFindReplace.Enabled = false;
                        this.mniSetTemplate.Enabled = false;
                    }
                }
            }
        }

        public void EnableSliders()
        {
            if (this.ActiveChartWindow != null)
            {
                this.paramSliders.Enabled = !this.ActiveChartWindow.IsStreaming;
            }
            else
            {
                this.paramSliders.Enabled = true;
            }
            this.linkRerun.Enabled = this.paramSliders.Enabled;
            this.linkResetParams.Enabled = this.paramSliders.Enabled;
            this.linkSaveParams.Enabled = this.paramSliders.Enabled;
        }

        public void ExecuteCompleted()
        {
            this.linkRerun.Visible = false;
        }

        public ChartForm FindStrategyFormByTag(Strategy strategy_0, object object_0)
        {
            foreach (Form form2 in base.MdiChildren)
            {
                if (form2 is ChartForm)
                {
                    ChartForm form = form2 as ChartForm;
                    if ((form2.Tag == object_0) && (form.Strategy == strategy_0))
                    {
                        return form;
                    }
                }
            }
            return null;
        }

        public QuotesForm GetQuoteForm(AutoTradingMode mode)
        {
            for (int i = base.MdiChildren.Length - 1; i >= 0; i--)
            {
                if (base.MdiChildren[i] is QuotesForm)
                {
                    QuotesForm form = base.MdiChildren[i] as QuotesForm;
                    if (form.AutoTradingMode == mode)
                    {
                        return form;
                    }
                }
            }
            QuotesForm form2 = new QuotesForm {
                MdiParent = this,
                AutoTradingMode = mode
            };
            form2.Show();
            return form2;
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(MainForm));
            this.lblAsk = new Label();
            this.menuMain = new MenuStrip();
            this.mniFile = new ToolStripMenuItem();
            this.mniNew = new ToolStripMenuItem();
            this.mniNewChart = new ToolStripMenuItem();
            this.mniNewBuilder = new ToolStripMenuItem();
            this.mniNewEditor = new ToolStripMenuItem();
            this.mniNewMultiStrategyBuilder = new ToolStripMenuItem();
            this.sepNewStrategy = new ToolStripSeparator();
            this.mniNewWorkspace2 = new ToolStripMenuItem();
            this.mniNewQuote = new ToolStripMenuItem();
            this.sepNewQuote = new ToolStripSeparator();
            this.mniNewDataSet = new ToolStripMenuItem();
            this.mniOpenStrategy = new ToolStripMenuItem();
            this.mniOpenWorkspace = new ToolStripMenuItem();
            this.mniClose = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.sepSave = new ToolStripSeparator();
            this.mniSaveStrategy = new ToolStripMenuItem();
            this.mniSaveStrategyAs = new ToolStripMenuItem();
            this.sepChart = new ToolStripSeparator();
            this.mniPreferences2 = new ToolStripMenuItem();
            this.sepPreferences = new ToolStripSeparator();
            this.mniLogin = new ToolStripMenuItem();
            this.mniOnDemand = new ToolStripMenuItem();
            this.sepExit = new ToolStripSeparator();
            this.mniCloseWorkspace = new ToolStripMenuItem();
            this.mniExit = new ToolStripMenuItem();
            this.mniEdit = new ToolStripMenuItem();
            this.mniUndoDelete = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.mniCut = new ToolStripMenuItem();
            this.mniCopy = new ToolStripMenuItem();
            this.mniPaste = new ToolStripMenuItem();
            this.mniDelete = new ToolStripMenuItem();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.mniSelectAll = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.mniFind = new ToolStripMenuItem();
            this.mniFindReplace = new ToolStripMenuItem();
            this.sepSelectAll = new ToolStripSeparator();
            this.mniSetTemplate = new ToolStripMenuItem();
            this.mniView = new ToolStripMenuItem();
            this.mniViewTradeTicket = new ToolStripMenuItem();
            this.mniViewDataPanel = new ToolStripMenuItem();
            this.mniViewStatusBar = new ToolStripMenuItem();
            this.sepView = new ToolStripSeparator();
            this.mniViewNavBar = new ToolStripMenuItem();
            this.mniNavIcons = new ToolStripMenuItem();
            this.mniViewToolbar = new ToolStripMenuItem();
            this.mniViewDrawingBar = new ToolStripMenuItem();
            this.mniDataWindow = new ToolStripMenuItem();
            this.mniTools = new ToolStripMenuItem();
            this.mniHomePage = new ToolStripMenuItem();
            this.mniAccounts = new ToolStripMenuItem();
            this.mniOrderManager = new ToolStripMenuItem();
            this.mniStrategyCenter = new ToolStripMenuItem();
            this.mniDataManager = new ToolStripMenuItem();
            this.symbolInfoManagerToolStripMenuItem = new ToolStripMenuItem();
            this.mniStrategyRanking = new ToolStripMenuItem();
            this.mniIndexManager = new ToolStripMenuItem();
            this.sepTools = new ToolStripSeparator();
            this.mniIndicators = new ToolStripMenuItem();
            this.mniFundamentals = new ToolStripMenuItem();
            this.sepIndicators = new ToolStripSeparator();
            this.mniDebug = new ToolStripMenuItem();
            this.mniQuickRef = new ToolStripMenuItem();
            this.sepQuickRef = new ToolStripSeparator();
            this.mniPreferences = new ToolStripMenuItem();
            this.executeStrategyHiddenMenuItem = new ToolStripMenuItem();
            this.mniWorkspaces = new ToolStripMenuItem();
            this.mniNewWorkspace3 = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.mniLoadWorkSpace = new ToolStripMenuItem();
            this.mniSaveWorkSpace = new ToolStripMenuItem();
            this.sepSaveWorkspace = new ToolStripSeparator();
            this.mniSetDefaultWorkspace = new ToolStripMenuItem();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.mniWindow = new ToolStripMenuItem();
            this.mniCascade = new ToolStripMenuItem();
            this.mniTileHorizontally = new ToolStripMenuItem();
            this.mniTileVertically = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.mniHelp = new ToolStripMenuItem();
            this.mniUserGuide = new ToolStripMenuItem();
            this.mniQuickRef2 = new ToolStripMenuItem();
            this.mniLanguageGuide = new ToolStripMenuItem();
            this.sepHelp = new ToolStripSeparator();
            this.mniFidelityCom = new ToolStripMenuItem();
            this.mniWealthLabCom = new ToolStripMenuItem();
            this.sepHelp2 = new ToolStripSeparator();
            this.mniSoftwareUpgrade = new ToolStripMenuItem();
            this.sepUpgrade = new ToolStripSeparator();
            this.mniAbout = new ToolStripMenuItem();
            this.status = new StatusStrip();
            this.statusMessage = new ToolStripStatusLabel();
            this.statusOrders = new ToolStripStatusLabel();
            this.statusActive = new ToolStripStatusLabel();
            this.statusStreamingProvider = new ToolStripStatusLabel();
            this.statusStreamingStatus = new ToolStripStatusLabel();
            this.statusStreamingSymbolsOff = new ToolStripStatusLabel();
            this.statusStreamingSymbolsOn = new ToolStripStatusLabel();
            this.statusSofwareDownload = new ToolStripStatusLabel();
            this.statusDownloadProgressBar = new ToolStripProgressBar();
            this.stlblHolder = new ToolStripStatusLabel();
            this.toolbarNav = new ToolStrip();
            this.btnHome = new ToolStripButton();
            this.dropdownCharts = new ToolStripDropDownButton();
            this.mniNewChart2 = new ToolStripMenuItem();
            this.mniNewRules2 = new ToolStripMenuItem();
            this.mniNewCode2 = new ToolStripMenuItem();
            this.mniNewMultiStrategyBuilder2 = new ToolStripMenuItem();
            this.mniOpenStrategy2 = new ToolStripMenuItem();
            this.sepChart2 = new ToolStripSeparator();
            this.mniChartFront = new ToolStripMenuItem();
            this.sepChartFront = new ToolStripSeparator();
            this.btnStrategyCenter = new ToolStripButton();
            this.dropdownQuotes = new ToolStripDropDownButton();
            this.mniNewQuote2 = new ToolStripMenuItem();
            this.sepQuote2 = new ToolStripSeparator();
            this.mniQuoteAll = new ToolStripMenuItem();
            this.sepQuoteFront = new ToolStripSeparator();
            this.btnOrdersAlerts = new ToolStripButton();
            this.btnAcctsPositions = new ToolStripButton();
            this.btnDataManager = new ToolStripButton();
            this.sepDataManager = new ToolStripSeparator();
            this.btnIndicators = new ToolStripButton();
            this.btnFundamental = new ToolStripButton();
            this.btnHelp = new ToolStripButton();
            this.btnPreferences = new ToolStripButton();
            this.btnTrade = new ToolStripButton();
            this.toolbar = new ToolStrip();
            this.toolStripDropDownButton1 = new ToolStripDropDownButton();
            this.mniNewChartDD = new ToolStripMenuItem();
            this.mniNewStrategyRulesDD = new ToolStripMenuItem();
            this.newStrategyFromCodeToolStripMenuItem = new ToolStripMenuItem();
            this.mniNewCombinationStrategy = new ToolStripMenuItem();
            this.sepNew = new ToolStripSeparator();
            this.mniNewWorkspaceTB = new ToolStripMenuItem();
            this.mniNewQuoteDD = new ToolStripMenuItem();
            this.sepNew2 = new ToolStripSeparator();
            this.mniNewDataSetTB = new ToolStripMenuItem();
            this.btnOpenStrategy = new ToolStripButton();
            this.btnSave = new ToolStripButton();
            this.btnSaveAs = new ToolStripButton();
            this.sepChartFile = new ToolStripSeparator();
            this.btnLogin = new ToolStripButton();
            this.sepSpacing = new ToolStripSeparator();
            this.lblSpacing = new ToolStripLabel();
            this.btnIncreaseSpacing = new ToolStripButton();
            this.btnRestoreSpacing = new ToolStripButton();
            this.btnDecreaseSpacing = new ToolStripButton();
            this.sepBarSpacing = new ToolStripSeparator();
            this.tslblChartStyles = new ToolStripLabel();
            this.btnCandleStyle = new ToolStripButton();
            this.btnBarChart = new ToolStripButton();
            this.btnLineChart = new ToolStripButton();
            this.tsmMoreChartStyles = new ToolStripDropDownButton();
            this.btnLinear = new ToolStripButton();
            this.btnLog = new ToolStripButton();
            this.sepChartStyles = new ToolStripSeparator();
            this.tslblOptions = new ToolStripLabel();
            this.btnLabelsVisible = new ToolStripButton();
            this.btnStatusBarVisible = new ToolStripButton();
            this.btnFundamentalsVisible = new ToolStripButton();
            this.btnDataWindow = new ToolStripButton();
            this.btnIndicatorsTB2 = new ToolStripButton();
            this.btnFundamentalsTB2 = new ToolStripButton();
            this.btnClearIndicators = new ToolStripButton();
            this.btnPushCode = new ToolStripButton();
            this.btnTradeTicket = new ToolStripButton();
            this.btnPreferencesTB = new ToolStripButton();
            this.pnlTree = new Panel();
            this.splitContainerDataPane = new SplitContainer();
            this.treeDataSources = new DataSourceTreeView();
            this.toolbarDataSets = new ToolStrip();
            this.lblDataSets = new ToolStripLabel();
            this.linkNewDataSet = new ToolStripLabel();
            this.toolbarParameters = new ToolStrip();
            this.lblParameters = new ToolStripLabel();
            this.pnlParamBase = new Panel();
            this.pnlParamBaseLinkParams = new Panel();
            this.linkRerun = new LinkLabel();
            this.linkResetParams = new LinkLabel();
            this.linkSaveParams = new LinkLabel();
            this.paramSliders = new ParameterSlidersContainer();
            this.popupPreferredValues = new ContextMenuStrip(this.icontainer_0);
            this.mniStorePreferredValues = new ToolStripMenuItem();
            this.lblScale = new Label();
            this.scale = new ScaleSelecter();
            this.cmbSymbol = new ComboBox();
            this.btnGo = new Button();
            this.lblSymbol = new Label();
            this.lblPositions = new Label();
            this.lblRange = new Label();
            this.posSize = new PositionSizeSelecter();
            this.dataRange = new BarDataRangeSelecter();
            this.toolbarDrawing = new ToolStrip();
            this.btnClearDrawingObjects = new ToolStripButton();
            this.btnCrossHair = new ToolStripButton();
            this.sepDeleteDrawing = new ToolStripSeparator();
            this.btnTrendline = new ToolStripButton();
            this.splitter = new Splitter();
            ///WYJ fix
            this.splitter.DoubleClick += /*this.splitContainerDataPane_DoubleClick;*/ new EventHandler(this.mniViewDataPanel_Click);
            this.saveFileDialog_0 = new SaveFileDialog();
            this.openFileDialog_0 = new OpenFileDialog();
            this.timer_0 = new System.Windows.Forms.Timer(this.icontainer_0);
            this.timer_1 = new System.Windows.Forms.Timer(this.icontainer_0);
            this.pnlTrade = new Panel();
            this.accountTypeSelector1 = new AccountTypeSelector(this.icontainer_0);
            this.lblBid = new Label();
            this.lblAsOf = new Label();
            this.lblLast = new Label();
            this.lblAcctType = new Label();
            this.btnDockUp = new Button();
            this.btnCloseTradeTicket = new Button();
            this.btnDockDown = new Button();
            this.btnLoginTradeTicket = new Button();
            this.numTradePrice = new NumEdit();
            this.numTradeQty = new NumEdit();
            this.txtTradeSymbol = new TextBox();
            this.btnStageOrder = new Button();
            this.btnPlaceOrder = new Button();
            this.lbTradeDirected = new Label();
            this.cmbTradeRoute = new ComboBox();
            this.lblTradeTIF = new Label();
            this.cmbTradeTIF = new ComboBox();
            this.lblTradePrice = new Label();
            this.lblTradeOrder = new Label();
            this.cmbTradeOrder = new ComboBox();
            this.lblTradeQty = new Label();
            this.lblTradeAction = new Label();
            this.cmbTradeAction = new ComboBox();
            this.lblTradeSymbol = new Label();
            this.lblTradeAcct = new Label();
            this.cmbAccount = new ComboBox();
            this.printPreviewDialog = new PrintPreviewDialog();
            this.pageSetupDialog_0 = new PageSetupDialog();
            this.timer_2 = new System.Windows.Forms.Timer(this.icontainer_0);
            this.assemblyLoader_0 = new AssemblyLoader(this.icontainer_0);
            this.drawingObjectManager_0 = new DrawingObjectManager(this.icontainer_0);
            this.streamingQuoteManager_0 = new StreamingQuoteManager(this.icontainer_0);
            this.menuMain.SuspendLayout();
            this.status.SuspendLayout();
            this.toolbarNav.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.pnlTree.SuspendLayout();
            this.splitContainerDataPane.BeginInit();
            this.splitContainerDataPane.Panel1.SuspendLayout();
            this.splitContainerDataPane.Panel2.SuspendLayout();
            this.splitContainerDataPane.SuspendLayout();
            this.toolbarDataSets.SuspendLayout();
            this.toolbarParameters.SuspendLayout();
            this.pnlParamBase.SuspendLayout();
            this.pnlParamBaseLinkParams.SuspendLayout();
            this.popupPreferredValues.SuspendLayout();
            this.toolbarDrawing.SuspendLayout();
            this.pnlTrade.SuspendLayout();
            base.SuspendLayout();
            this.lblAsk.Location = new Point(0x298, 0x19);
            this.lblAsk.Name = "lblAsk";
            this.lblAsk.Size = new Size(0x75, 11);
            this.lblAsk.TabIndex = 0x2a;
            this.lblAsk.Text = "Ask:";
            this.lblAsk.Visible = false;
            this.menuMain.Items.AddRange(new ToolStripItem[] { this.mniFile, this.mniEdit, this.mniView, this.mniTools, this.mniWorkspaces, this.mniWindow, this.mniHelp });
            this.menuMain.Location = new Point(0, 0);
            this.menuMain.MdiWindowListItem = this.mniWindow;
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new Size(0x404, 0x18);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            this.mniFile.DropDownItems.AddRange(new ToolStripItem[] { this.mniNew, this.mniOpenStrategy, this.mniOpenWorkspace, this.mniClose, this.mniPrint, this.sepSave, this.mniSaveStrategy, this.mniSaveStrategyAs, this.sepChart, this.mniPreferences2, this.sepPreferences, this.mniLogin, this.mniOnDemand, this.sepExit, this.mniCloseWorkspace, this.mniExit });
            this.mniFile.Name = "mniFile";
            this.mniFile.Size = new Size(0x23, 20);
            this.mniFile.Text = "&File";
            this.mniNew.DropDownItems.AddRange(new ToolStripItem[] { this.mniNewChart, this.mniNewBuilder, this.mniNewEditor, this.mniNewMultiStrategyBuilder, this.sepNewStrategy, this.mniNewWorkspace2, this.mniNewQuote, this.sepNewQuote, this.mniNewDataSet });
            this.mniNew.Image = (Image) manager.GetObject("mniNew.Image");
            this.mniNew.ImageTransparentColor = Color.Fuchsia;
            this.mniNew.Name = "mniNew";
            this.mniNew.Size = new Size(0x105, 0x16);
            this.mniNew.Text = "&New";
            this.mniNewChart.Image = (Image) manager.GetObject("mniNewChart.Image");
            this.mniNewChart.Name = "mniNewChart";
            this.mniNewChart.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
            this.mniNewChart.Size = new Size(300, 0x16);
            this.mniNewChart.Text = "New &Chart Window";
            this.mniNewChart.Click += new EventHandler(this.mniNewChartDD_Click);
            this.mniNewBuilder.Image = (Image) manager.GetObject("mniNewBuilder.Image");
            this.mniNewBuilder.ImageTransparentColor = Color.Fuchsia;
            this.mniNewBuilder.Name = "mniNewBuilder";
            this.mniNewBuilder.ShortcutKeys = Keys.Control | Keys.Shift | Keys.R;
            this.mniNewBuilder.Size = new Size(300, 0x16);
            this.mniNewBuilder.Text = "New Strategy from &Rules";
            this.mniNewBuilder.Click += new EventHandler(this.mniNewBuilder_Click);
            this.mniNewEditor.Image = (Image) manager.GetObject("mniNewEditor.Image");
            this.mniNewEditor.ImageTransparentColor = Color.Fuchsia;
            this.mniNewEditor.Name = "mniNewEditor";
            this.mniNewEditor.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            this.mniNewEditor.Size = new Size(300, 0x16);
            this.mniNewEditor.Text = "New &Strategy from Code";
            this.mniNewEditor.Click += new EventHandler(this.newStrategyFromCodeToolStripMenuItem_Click);
            this.mniNewMultiStrategyBuilder.Image = (Image) manager.GetObject("mniNewMultiStrategyBuilder.Image");
            this.mniNewMultiStrategyBuilder.Name = "mniNewMultiStrategyBuilder";
            this.mniNewMultiStrategyBuilder.ShortcutKeys = Keys.Control | Keys.Shift | Keys.M;
            this.mniNewMultiStrategyBuilder.Size = new Size(300, 0x16);
            this.mniNewMultiStrategyBuilder.Text = "New Combination Strategy";
            this.mniNewMultiStrategyBuilder.Click += new EventHandler(this.mniNewCombinationStrategy_Click);
            this.sepNewStrategy.Name = "sepNewStrategy";
            this.sepNewStrategy.Size = new Size(0x129, 6);
            this.mniNewWorkspace2.Name = "mniNewWorkspace2";
            this.mniNewWorkspace2.ShortcutKeys = Keys.Control | Keys.Shift | Keys.W;
            this.mniNewWorkspace2.Size = new Size(300, 0x16);
            this.mniNewWorkspace2.Text = "New Main &Workspace Window";
            this.mniNewWorkspace2.Click += new EventHandler(this.mniNewWorkspaceTB_Click);
            this.mniNewQuote.Image = (Image) manager.GetObject("mniNewQuote.Image");
            this.mniNewQuote.ImageTransparentColor = Color.Fuchsia;
            this.mniNewQuote.Name = "mniNewQuote";
            this.mniNewQuote.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Q;
            this.mniNewQuote.Size = new Size(300, 0x16);
            this.mniNewQuote.Text = "New &Quote Window";
            this.mniNewQuote.Click += new EventHandler(this.mniNewQuoteDD_Click);
            this.sepNewQuote.Name = "sepNewQuote";
            this.sepNewQuote.Size = new Size(0x129, 6);
            this.mniNewDataSet.Image = (Image) manager.GetObject("mniNewDataSet.Image");
            this.mniNewDataSet.ImageTransparentColor = Color.Fuchsia;
            this.mniNewDataSet.Name = "mniNewDataSet";
            this.mniNewDataSet.ShortcutKeys = Keys.Control | Keys.Shift | Keys.D;
            this.mniNewDataSet.Size = new Size(300, 0x16);
            this.mniNewDataSet.Text = "New &DataSet ...";
            this.mniNewDataSet.Click += new EventHandler(this.linkNewDataSet_Click);
            this.mniOpenStrategy.Image = (Image) manager.GetObject("mniOpenStrategy.Image");
            this.mniOpenStrategy.ImageTransparentColor = Color.Fuchsia;
            this.mniOpenStrategy.Name = "mniOpenStrategy";
            this.mniOpenStrategy.ShortcutKeys = Keys.Control | Keys.O;
            this.mniOpenStrategy.Size = new Size(0x105, 0x16);
            this.mniOpenStrategy.Text = "Open &Strategy ...";
            this.mniOpenStrategy.Click += new EventHandler(this.btnOpenStrategy_Click);
            this.mniOpenWorkspace.Name = "mniOpenWorkspace";
            this.mniOpenWorkspace.ShortcutKeys = Keys.Control | Keys.W;
            this.mniOpenWorkspace.Size = new Size(0x105, 0x16);
            this.mniOpenWorkspace.Text = "Open &Workspace ...";
            this.mniOpenWorkspace.Click += new EventHandler(this.mniLoadWorkSpace_Click);
            this.mniClose.Name = "mniClose";
            this.mniClose.ShortcutKeyDisplayString = "Ctrl + F4";
            this.mniClose.Size = new Size(0x105, 0x16);
            this.mniClose.Text = "&Close";
            this.mniClose.Click += new EventHandler(this.mniClose_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0x105, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.ToolTipText = "Print content from selected Tab";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.sepSave.Name = "sepSave";
            this.sepSave.Size = new Size(0x102, 6);
            this.mniSaveStrategy.Image = (Image) manager.GetObject("mniSaveStrategy.Image");
            this.mniSaveStrategy.ImageTransparentColor = Color.Fuchsia;
            this.mniSaveStrategy.Name = "mniSaveStrategy";
            this.mniSaveStrategy.ShortcutKeyDisplayString = "";
            this.mniSaveStrategy.ShortcutKeys = Keys.Control | Keys.S;
            this.mniSaveStrategy.Size = new Size(0x105, 0x16);
            this.mniSaveStrategy.Text = "&Save";
            this.mniSaveStrategy.ToolTipText = "Save changes made to the Strategy";
            this.mniSaveStrategy.Click += new EventHandler(this.btnSave_Click);
            this.mniSaveStrategyAs.Image = (Image) manager.GetObject("mniSaveStrategyAs.Image");
            this.mniSaveStrategyAs.ImageTransparentColor = Color.Fuchsia;
            this.mniSaveStrategyAs.Name = "mniSaveStrategyAs";
            this.mniSaveStrategyAs.ShowShortcutKeys = false;
            this.mniSaveStrategyAs.Size = new Size(0x105, 0x16);
            this.mniSaveStrategyAs.Text = "Save &As ...";
            this.mniSaveStrategyAs.ToolTipText = "Save the current Strategy with a new Name ...";
            this.mniSaveStrategyAs.Click += new EventHandler(this.btnSaveAs_Click);
            this.sepChart.Name = "sepChart";
            this.sepChart.Size = new Size(0x102, 6);
            this.mniPreferences2.Image = (Image) manager.GetObject("mniPreferences2.Image");
            this.mniPreferences2.ImageTransparentColor = Color.Fuchsia;
            this.mniPreferences2.Name = "mniPreferences2";
            this.mniPreferences2.ShortcutKeys = Keys.F12;
            this.mniPreferences2.Size = new Size(0x105, 0x16);
            this.mniPreferences2.Text = "&Preferences";
            this.mniPreferences2.Click += new EventHandler(this.btnPreferencesTB_Click);
            this.sepPreferences.Name = "sepPreferences";
            this.sepPreferences.Size = new Size(0x102, 6);
            this.mniLogin.Image = (Image) manager.GetObject("mniLogin.Image");
            this.mniLogin.ImageTransparentColor = Color.Fuchsia;
            this.mniLogin.Name = "mniLogin";
            this.mniLogin.ShortcutKeys = Keys.Control | Keys.L;
            this.mniLogin.Size = new Size(0x105, 0x16);
            this.mniLogin.Text = "Fidelity Account &Log in";
            this.mniLogin.Click += new EventHandler(this.btnLoginTradeTicket_Click);
            this.mniOnDemand.Checked = true;
            this.mniOnDemand.CheckState = CheckState.Checked;
            this.mniOnDemand.Name = "mniOnDemand";
            this.mniOnDemand.ShortcutKeys = Keys.Alt | Keys.Control | Keys.L;
            this.mniOnDemand.Size = new Size(0x105, 0x16);
            this.mniOnDemand.Text = "&Update Data on Demand";
            this.mniOnDemand.Click += new EventHandler(this.mniOnDemand_Click);
            this.sepExit.Name = "sepExit";
            this.sepExit.Size = new Size(0x102, 6);
            this.mniCloseWorkspace.Name = "mniCloseWorkspace";
            this.mniCloseWorkspace.Size = new Size(0x105, 0x16);
            this.mniCloseWorkspace.Text = "Clos&e this Workspace Window";
            this.mniCloseWorkspace.Visible = false;
            this.mniCloseWorkspace.Click += new EventHandler(this.mniCloseWorkspace_Click);
            this.mniExit.Name = "mniExit";
            this.mniExit.ShortcutKeys = Keys.Alt | Keys.F4;
            this.mniExit.Size = new Size(0x105, 0x16);
            this.mniExit.Text = "E&xit";
            this.mniExit.Click += new EventHandler(this.mniExit_Click);
            this.mniEdit.DropDownItems.AddRange(new ToolStripItem[] { this.mniUndoDelete, this.toolStripSeparator1, this.mniCut, this.mniCopy, this.mniPaste, this.mniDelete, this.toolStripSeparator5, this.mniSelectAll, this.toolStripSeparator4, this.mniFind, this.mniFindReplace, this.sepSelectAll, this.mniSetTemplate });
            this.mniEdit.Enabled = false;
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.Size = new Size(0x25, 20);
            this.mniEdit.Text = "&Edit";
            this.mniUndoDelete.Image = (Image) manager.GetObject("mniUndoDelete.Image");
            this.mniUndoDelete.ImageTransparentColor = Color.Fuchsia;
            this.mniUndoDelete.Name = "mniUndoDelete";
            this.mniUndoDelete.ShortcutKeyDisplayString = "Ctrl+Z";
            this.mniUndoDelete.Size = new Size(0xe4, 0x16);
            this.mniUndoDelete.Text = "&Undo";
            this.mniUndoDelete.Click += new EventHandler(this.mniUndoDelete_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0xe1, 6);
            this.mniCut.Image = (Image) manager.GetObject("mniCut.Image");
            this.mniCut.ImageTransparentColor = Color.Fuchsia;
            this.mniCut.Name = "mniCut";
            this.mniCut.ShortcutKeyDisplayString = "Ctrl+X";
            this.mniCut.Size = new Size(0xe4, 0x16);
            this.mniCut.Text = "Cu&t";
            this.mniCut.Click += new EventHandler(this.mniCut_Click);
            this.mniCopy.Image = (Image) manager.GetObject("mniCopy.Image");
            this.mniCopy.ImageTransparentColor = Color.Fuchsia;
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.ShortcutKeyDisplayString = "Ctrl+c";
            this.mniCopy.Size = new Size(0xe4, 0x16);
            this.mniCopy.Text = "&Copy";
            this.mniCopy.Click += new EventHandler(this.mniCopy_Click);
            this.mniPaste.Image = (Image) manager.GetObject("mniPaste.Image");
            this.mniPaste.ImageTransparentColor = Color.Fuchsia;
            this.mniPaste.Name = "mniPaste";
            this.mniPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.mniPaste.Size = new Size(0xe4, 0x16);
            this.mniPaste.Text = "&Paste";
            this.mniPaste.Click += new EventHandler(this.mniPaste_Click);
            this.mniDelete.Image = (Image) manager.GetObject("mniDelete.Image");
            this.mniDelete.ImageTransparentColor = Color.Fuchsia;
            this.mniDelete.Name = "mniDelete";
            this.mniDelete.ShortcutKeys = Keys.Control | Keys.Delete;
            this.mniDelete.Size = new Size(0xe4, 0x16);
            this.mniDelete.Text = "&Delete";
            this.mniDelete.Click += new EventHandler(this.mniDelete_Click);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(0xe1, 6);
            this.mniSelectAll.Name = "mniSelectAll";
            this.mniSelectAll.ShortcutKeys = Keys.Control | Keys.A;
            this.mniSelectAll.Size = new Size(0xe4, 0x16);
            this.mniSelectAll.Text = "Select A&ll";
            this.mniSelectAll.Click += new EventHandler(this.mniSelectAll_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(0xe1, 6);
            this.mniFind.ImageTransparentColor = Color.Fuchsia;
            this.mniFind.Name = "mniFind";
            this.mniFind.ShortcutKeys = Keys.Control | Keys.F;
            this.mniFind.Size = new Size(0xe4, 0x16);
            this.mniFind.Text = "&Find";
            this.mniFind.Click += new EventHandler(this.mniFind_Click);
            this.mniFindReplace.ImageTransparentColor = Color.Fuchsia;
            this.mniFindReplace.Name = "mniFindReplace";
            this.mniFindReplace.ShortcutKeys = Keys.Control | Keys.H;
            this.mniFindReplace.Size = new Size(0xe4, 0x16);
            this.mniFindReplace.Text = "Find && &Replace";
            this.mniFindReplace.Click += new EventHandler(this.mniFindReplace_Click);
            this.sepSelectAll.Name = "sepSelectAll";
            this.sepSelectAll.Size = new Size(0xe1, 6);
            this.mniSetTemplate.Name = "mniSetTemplate";
            this.mniSetTemplate.Size = new Size(0xe4, 0x16);
            this.mniSetTemplate.Text = "&Set as Default Template Code";
            this.mniSetTemplate.Click += new EventHandler(this.mniSetTemplate_Click);
            this.mniView.DropDownItems.AddRange(new ToolStripItem[] { this.mniViewTradeTicket, this.mniViewDataPanel, this.mniViewStatusBar, this.sepView, this.mniViewNavBar, this.mniNavIcons, this.mniViewToolbar, this.mniViewDrawingBar, this.mniDataWindow });
            this.mniView.Name = "mniView";
            this.mniView.Size = new Size(0x29, 20);
            this.mniView.Text = "&View";
            this.mniViewTradeTicket.Name = "mniViewTradeTicket";
            this.mniViewTradeTicket.ShortcutKeys = Keys.Control | Keys.T;
            this.mniViewTradeTicket.Size = new Size(0xb8, 0x16);
            this.mniViewTradeTicket.Text = "&Trade Ticket";
            this.mniViewTradeTicket.Click += new EventHandler(this.btnTradeTicket_Click);
            this.mniViewDataPanel.Name = "mniViewDataPanel";
            this.mniViewDataPanel.ShortcutKeys = Keys.Control | Keys.D;
            this.mniViewDataPanel.Size = new Size(0xb8, 0x16);
            this.mniViewDataPanel.Text = "&Data Panel";
            this.mniViewDataPanel.Click += new EventHandler(this.mniViewDataPanel_Click);
            this.mniViewStatusBar.Name = "mniViewStatusBar";
            this.mniViewStatusBar.Size = new Size(0xb8, 0x16);
            this.mniViewStatusBar.Text = "&Status Bar";
            this.mniViewStatusBar.Click += new EventHandler(this.mniViewStatusBar_Click);
            this.sepView.Name = "sepView";
            this.sepView.Size = new Size(0xb5, 6);
            this.mniViewNavBar.Name = "mniViewNavBar";
            this.mniViewNavBar.Size = new Size(0xb8, 0x16);
            this.mniViewNavBar.Text = "Navigation &Bar";
            this.mniViewNavBar.Click += new EventHandler(this.mniViewNavBar_Click);
            this.mniNavIcons.Name = "mniNavIcons";
            this.mniNavIcons.Size = new Size(0xb8, 0x16);
            this.mniNavIcons.Text = "Navigation Bar &Icons";
            this.mniNavIcons.Click += new EventHandler(this.mniNavIcons_Click);
            this.mniViewToolbar.Name = "mniViewToolbar";
            this.mniViewToolbar.Size = new Size(0xb8, 0x16);
            this.mniViewToolbar.Text = "&Function Toolbar";
            this.mniViewToolbar.Click += new EventHandler(this.mniViewToolbar_Click);
            this.mniViewDrawingBar.Name = "mniViewDrawingBar";
            this.mniViewDrawingBar.Size = new Size(0xb8, 0x16);
            this.mniViewDrawingBar.Text = "D&rawing Toolbar";
            this.mniViewDrawingBar.Click += new EventHandler(this.mniViewDrawingBar_Click);
            this.mniDataWindow.Name = "mniDataWindow";
            this.mniDataWindow.Size = new Size(0xb8, 0x16);
            this.mniDataWindow.Tag = "CS";
            this.mniDataWindow.Text = "Data &Window";
            this.mniDataWindow.Visible = false;
            this.mniDataWindow.Click += new EventHandler(this.btnDataWindow_Click);
            this.mniTools.DropDownItems.AddRange(new ToolStripItem[] { 
                this.mniHomePage, this.mniAccounts, this.mniOrderManager, this.mniStrategyCenter, this.mniDataManager, this.symbolInfoManagerToolStripMenuItem, this.mniStrategyRanking, this.mniIndexManager, this.sepTools, this.mniIndicators, this.mniFundamentals, this.sepIndicators, this.mniDebug, this.mniQuickRef, this.sepQuickRef, this.mniPreferences, 
                this.executeStrategyHiddenMenuItem
             });
            this.mniTools.Name = "mniTools";
            this.mniTools.Size = new Size(0x2c, 20);
            this.mniTools.Text = "&Tools";
            this.mniHomePage.Image = (Image) manager.GetObject("mniHomePage.Image");
            this.mniHomePage.ImageTransparentColor = Color.Fuchsia;
            this.mniHomePage.Name = "mniHomePage";
            this.mniHomePage.Size = new Size(0x121, 0x16);
            this.mniHomePage.Text = "&Home Page";
            this.mniHomePage.Click += new EventHandler(this.btnHome_Click);
            this.mniAccounts.Image = (Image) manager.GetObject("mniAccounts.Image");
            this.mniAccounts.ImageTransparentColor = Color.Fuchsia;
            this.mniAccounts.Name = "mniAccounts";
            this.mniAccounts.ShortcutKeys = Keys.Alt | Keys.Control | Keys.T;
            this.mniAccounts.Size = new Size(0x121, 0x16);
            this.mniAccounts.Text = "&Accounts";
            this.mniAccounts.Click += new EventHandler(this.btnAcctsPositions_Click);
            this.mniOrderManager.Image = (Image) manager.GetObject("mniOrderManager.Image");
            this.mniOrderManager.ImageTransparentColor = Color.Fuchsia;
            this.mniOrderManager.Name = "mniOrderManager";
            this.mniOrderManager.ShortcutKeys = Keys.Control | Keys.R;
            this.mniOrderManager.Size = new Size(0x121, 0x16);
            this.mniOrderManager.Text = "&Orders";
            this.mniOrderManager.Click += new EventHandler(this.btnOrdersAlerts_Click);
            this.mniStrategyCenter.Image = (Image) manager.GetObject("mniStrategyCenter.Image");
            this.mniStrategyCenter.ImageTransparentColor = Color.Fuchsia;
            this.mniStrategyCenter.Name = "mniStrategyCenter";
            this.mniStrategyCenter.ShortcutKeys = Keys.F3;
            this.mniStrategyCenter.Size = new Size(0x121, 0x16);
            this.mniStrategyCenter.Text = "&Strategy Monitor";
            this.mniStrategyCenter.Click += new EventHandler(this.btnStrategyCenter_Click);
            this.mniDataManager.Image = (Image) manager.GetObject("mniDataManager.Image");
            this.mniDataManager.ImageTransparentColor = Color.Fuchsia;
            this.mniDataManager.Name = "mniDataManager";
            this.mniDataManager.ShortcutKeys = Keys.Control | Keys.M;
            this.mniDataManager.Size = new Size(0x121, 0x16);
            this.mniDataManager.Text = "&Data Manager";
            this.mniDataManager.Click += new EventHandler(this.btnDataManager_Click);
            this.symbolInfoManagerToolStripMenuItem.Image = (Image) manager.GetObject("symbolInfoManagerToolStripMenuItem.Image");
            this.symbolInfoManagerToolStripMenuItem.ImageTransparentColor = Color.Fuchsia;
            this.symbolInfoManagerToolStripMenuItem.Name = "symbolInfoManagerToolStripMenuItem";
            this.symbolInfoManagerToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.Control | Keys.F;
            this.symbolInfoManagerToolStripMenuItem.Size = new Size(0x121, 0x16);
            this.symbolInfoManagerToolStripMenuItem.Text = "Symbol &Info Manager";
            this.symbolInfoManagerToolStripMenuItem.Click += new EventHandler(this.symbolInfoManagerToolStripMenuItem_Click);
            this.mniStrategyRanking.Image = (Image) manager.GetObject("mniStrategyRanking.Image");
            this.mniStrategyRanking.Name = "mniStrategyRanking";
            this.mniStrategyRanking.ShortcutKeys = Keys.Alt | Keys.Control | Keys.R;
            this.mniStrategyRanking.Size = new Size(0x121, 0x16);
            this.mniStrategyRanking.Text = "Strategy &Ranking";
            this.mniStrategyRanking.Click += new EventHandler(this.mniStrategyRanking_Click);
            this.mniIndexManager.Image = (Image) manager.GetObject("mniIndexManager.Image");
            this.mniIndexManager.Name = "mniIndexManager";
            this.mniIndexManager.ShortcutKeys = Keys.Alt | Keys.Control | Keys.I;
            this.mniIndexManager.Size = new Size(0x121, 0x16);
            this.mniIndexManager.Text = "Index-Lab \x00ae";
            this.mniIndexManager.Click += new EventHandler(this.mniIndexManager_Click);
            this.sepTools.Name = "sepTools";
            this.sepTools.Size = new Size(0x11e, 6);
            this.mniIndicators.Image = (Image) manager.GetObject("mniIndicators.Image");
            this.mniIndicators.ImageTransparentColor = Color.Fuchsia;
            this.mniIndicators.Name = "mniIndicators";
            this.mniIndicators.ShortcutKeys = Keys.Control | Keys.F11;
            this.mniIndicators.Size = new Size(0x121, 0x16);
            this.mniIndicators.Text = "&Technical Indicators";
            this.mniIndicators.Click += new EventHandler(this.btnIndicatorsTB2_Click);
            this.mniFundamentals.Image = (Image) manager.GetObject("mniFundamentals.Image");
            this.mniFundamentals.ImageTransparentColor = Color.Fuchsia;
            this.mniFundamentals.Name = "mniFundamentals";
            this.mniFundamentals.ShortcutKeys = Keys.Control | Keys.U;
            this.mniFundamentals.Size = new Size(0x121, 0x16);
            this.mniFundamentals.Text = "&Fundamental Data Items";
            this.mniFundamentals.Click += new EventHandler(this.btnFundamentalsTB2_Click);
            this.mniFundamentals.VisibleChanged += new EventHandler(this.mniFundamentals_VisibleChanged);
            this.sepIndicators.Name = "sepIndicators";
            this.sepIndicators.Size = new Size(0x11e, 6);
            this.mniDebug.Image = (Image) manager.GetObject("mniDebug.Image");
            this.mniDebug.ImageTransparentColor = Color.Fuchsia;
            this.mniDebug.Name = "mniDebug";
            this.mniDebug.ShortcutKeys = Keys.Alt | Keys.Control | Keys.D;
            this.mniDebug.Size = new Size(0x121, 0x16);
            this.mniDebug.Text = "Debug and Error Message &Log";
            this.mniDebug.Click += new EventHandler(this.mniDebug_Click);
            this.mniQuickRef.Image = (Image) manager.GetObject("mniQuickRef.Image");
            this.mniQuickRef.ImageTransparentColor = Color.Fuchsia;
            this.mniQuickRef.Name = "mniQuickRef";
            this.mniQuickRef.ShortcutKeys = Keys.F11;
            this.mniQuickRef.Size = new Size(0x121, 0x16);
            this.mniQuickRef.Text = "WealthScript &QuickRef";
            this.mniQuickRef.Click += new EventHandler(this.mniQuickRef2_Click);
            this.sepQuickRef.Name = "sepQuickRef";
            this.sepQuickRef.Size = new Size(0x11e, 6);
            this.mniPreferences.Image = (Image) manager.GetObject("mniPreferences.Image");
            this.mniPreferences.ImageTransparentColor = Color.Silver;
            this.mniPreferences.Name = "mniPreferences";
            this.mniPreferences.ShortcutKeys = Keys.F12;
            this.mniPreferences.Size = new Size(0x121, 0x16);
            this.mniPreferences.Text = "&Preferences";
            this.mniPreferences.Click += new EventHandler(this.btnPreferencesTB_Click);
            this.executeStrategyHiddenMenuItem.Name = "executeStrategyHiddenMenuItem";
            this.executeStrategyHiddenMenuItem.ShortcutKeys = Keys.F5;
            this.executeStrategyHiddenMenuItem.Size = new Size(0x121, 0x16);
            this.executeStrategyHiddenMenuItem.Text = "&Execute Strategy";
            this.executeStrategyHiddenMenuItem.Visible = false;
            this.executeStrategyHiddenMenuItem.Click += new EventHandler(this.btnGo_Click);
            this.mniWorkspaces.DropDownItems.AddRange(new ToolStripItem[] { this.mniNewWorkspace3, this.toolStripSeparator3, this.mniLoadWorkSpace, this.mniSaveWorkSpace, this.sepSaveWorkspace, this.mniSetDefaultWorkspace, this.toolStripSeparator7 });
            this.mniWorkspaces.Name = "mniWorkspaces";
            this.mniWorkspaces.Size = new Size(0x4d, 20);
            this.mniWorkspaces.Text = "W&orkspaces";
            this.mniNewWorkspace3.Name = "mniNewWorkspace3";
            this.mniNewWorkspace3.ShortcutKeys = Keys.Control | Keys.Shift | Keys.W;
            this.mniNewWorkspace3.Size = new Size(300, 0x16);
            this.mniNewWorkspace3.Text = "New Main &Workspace Window";
            this.mniNewWorkspace3.Click += new EventHandler(this.mniNewWorkspaceTB_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(0x129, 6);
            this.mniLoadWorkSpace.Name = "mniLoadWorkSpace";
            this.mniLoadWorkSpace.ShortcutKeys = Keys.Control | Keys.W;
            this.mniLoadWorkSpace.Size = new Size(300, 0x16);
            this.mniLoadWorkSpace.Text = "&Open Workspace ...";
            this.mniLoadWorkSpace.Click += new EventHandler(this.mniLoadWorkSpace_Click);
            this.mniSaveWorkSpace.Name = "mniSaveWorkSpace";
            this.mniSaveWorkSpace.Size = new Size(300, 0x16);
            this.mniSaveWorkSpace.Text = "&Save Workspace ...";
            this.mniSaveWorkSpace.Click += new EventHandler(this.mniSaveWorkSpace_Click);
            this.sepSaveWorkspace.Name = "sepSaveWorkspace";
            this.sepSaveWorkspace.Size = new Size(0x129, 6);
            this.mniSetDefaultWorkspace.Name = "mniSetDefaultWorkspace";
            this.mniSetDefaultWorkspace.Size = new Size(300, 0x16);
            this.mniSetDefaultWorkspace.Text = "Set as &Default Workspace";
            this.mniSetDefaultWorkspace.Click += new EventHandler(this.mniSetDefaultWorkspace_Click);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(0x129, 6);
            this.mniWindow.DropDownItems.AddRange(new ToolStripItem[] { this.mniCascade, this.mniTileHorizontally, this.mniTileVertically, this.toolStripSeparator2 });
            this.mniWindow.Name = "mniWindow";
            this.mniWindow.Size = new Size(0x39, 20);
            this.mniWindow.Text = "&Window";
            this.mniCascade.Name = "mniCascade";
            this.mniCascade.Size = new Size(160, 0x16);
            this.mniCascade.Text = "&Cascade";
            this.mniCascade.Click += new EventHandler(this.mniCascade_Click);
            this.mniTileHorizontally.Name = "mniTileHorizontally";
            this.mniTileHorizontally.Size = new Size(160, 0x16);
            this.mniTileHorizontally.Text = "Tile &Horizontally";
            this.mniTileHorizontally.Click += new EventHandler(this.mniTileHorizontally_Click);
            this.mniTileVertically.Name = "mniTileVertically";
            this.mniTileVertically.Size = new Size(160, 0x16);
            this.mniTileVertically.Text = "Tile &Vertically";
            this.mniTileVertically.Click += new EventHandler(this.mniTileVertically_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(0x9d, 6);
            this.mniHelp.DropDownItems.AddRange(new ToolStripItem[] { this.mniUserGuide, this.mniQuickRef2, this.mniLanguageGuide, this.sepHelp, this.mniFidelityCom, this.mniWealthLabCom, this.sepHelp2, this.mniSoftwareUpgrade, this.sepUpgrade, this.mniAbout });
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.ShortcutKeys = Keys.F1;
            this.mniHelp.Size = new Size(40, 20);
            this.mniHelp.Text = "&Help";
            this.mniUserGuide.Image = (Image) manager.GetObject("mniUserGuide.Image");
            this.mniUserGuide.ImageTransparentColor = Color.Fuchsia;
            this.mniUserGuide.Name = "mniUserGuide";
            this.mniUserGuide.ShortcutKeys = Keys.F1;
            this.mniUserGuide.Size = new Size(0xf1, 0x16);
            this.mniUserGuide.Text = "Wealth-Lab &User Guide";
            this.mniUserGuide.Click += new EventHandler(this.btnHelp_Click);
            this.mniQuickRef2.Image = (Image) manager.GetObject("mniQuickRef2.Image");
            this.mniQuickRef2.ImageTransparentColor = Color.Fuchsia;
            this.mniQuickRef2.Name = "mniQuickRef2";
            this.mniQuickRef2.ShortcutKeys = Keys.F11;
            this.mniQuickRef2.Size = new Size(0xf1, 0x16);
            this.mniQuickRef2.Text = "WealthScript &QuickRef";
            this.mniQuickRef2.Click += new EventHandler(this.mniQuickRef2_Click);
            this.mniLanguageGuide.Name = "mniLanguageGuide";
            this.mniLanguageGuide.Size = new Size(0xf1, 0x16);
            this.mniLanguageGuide.Text = "WealthScript Programming Guide";
            this.mniLanguageGuide.Click += new EventHandler(this.mniLanguageGuide_Click);
            this.sepHelp.Name = "sepHelp";
            this.sepHelp.Size = new Size(0xee, 6);
            this.mniFidelityCom.Name = "mniFidelityCom";
            this.mniFidelityCom.Size = new Size(0xf1, 0x16);
            this.mniFidelityCom.Text = "&Fidelity.com";
            this.mniFidelityCom.Click += new EventHandler(this.mniFidelityCom_Click);
            this.mniWealthLabCom.Name = "mniWealthLabCom";
            this.mniWealthLabCom.Size = new Size(0xf1, 0x16);
            this.mniWealthLabCom.Text = "&Wealth-Lab.com";
            this.mniWealthLabCom.Click += new EventHandler(this.mniWealthLabCom_Click);
            this.sepHelp2.Name = "sepHelp2";
            this.sepHelp2.Size = new Size(0xee, 6);
            this.mniSoftwareUpgrade.Name = "mniSoftwareUpgrade";
            this.mniSoftwareUpgrade.Size = new Size(0xf1, 0x16);
            this.mniSoftwareUpgrade.Text = "Software Upgrade";
            this.mniSoftwareUpgrade.Click += new EventHandler(this.mniSoftwareUpgrade_Click);
            this.sepUpgrade.Name = "sepUpgrade";
            this.sepUpgrade.Size = new Size(0xee, 6);
            this.mniAbout.Name = "mniAbout";
            this.mniAbout.Size = new Size(0xf1, 0x16);
            this.mniAbout.Text = "&About Wealth-Lab Pro ...";
            this.mniAbout.Click += new EventHandler(this.mniAbout_Click);
            this.status.Items.AddRange(new ToolStripItem[] { this.statusMessage, this.statusOrders, this.statusActive, this.statusStreamingProvider, this.statusStreamingStatus, this.statusStreamingSymbolsOff, this.statusStreamingSymbolsOn, this.statusSofwareDownload, this.statusDownloadProgressBar });
            this.status.Location = new Point(0, 0x1ca);
            this.status.Name = "status";
            this.status.ShowItemToolTips = true;
            this.status.Size = new Size(0x404, 0x16);
            this.status.TabIndex = 6;
            this.status.Text = "statusStrip1";
            this.statusMessage.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusMessage.Name = "statusMessage";
            this.statusMessage.Size = new Size(0xc9, 0x11);
            this.statusMessage.Text = "Strategy Status Message (click to clear)";
            this.statusMessage.Click += new EventHandler(this.statusMessage_Click);
            this.statusOrders.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusOrders.IsLink = true;
            this.statusOrders.LinkBehavior = LinkBehavior.HoverUnderline;
            this.statusOrders.Name = "statusOrders";
            this.statusOrders.Size = new Size(0x39, 0x11);
            this.statusOrders.Text = "Orders: 0";
            this.statusOrders.TextAlign = ContentAlignment.MiddleLeft;
            this.statusOrders.Click += new EventHandler(this.statusActive_Click);
            this.statusActive.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusActive.IsLink = true;
            this.statusActive.LinkBehavior = LinkBehavior.HoverUnderline;
            this.statusActive.Name = "statusActive";
            this.statusActive.Size = new Size(0x51, 0x11);
            this.statusActive.Text = "Active Orders:";
            this.statusActive.Click += new EventHandler(this.statusActive_Click);
            this.statusStreamingProvider.Image = (Image) manager.GetObject("statusStreamingProvider.Image");
            this.statusStreamingProvider.ImageTransparentColor = Color.Fuchsia;
            this.statusStreamingProvider.LinkColor = Color.Red;
            this.statusStreamingProvider.Name = "statusStreamingProvider";
            this.statusStreamingProvider.Size = new Size(0x76, 0x11);
            this.statusStreamingProvider.Text = "Streaming Provider:";
            this.statusStreamingProvider.Visible = false;
            this.statusStreamingProvider.Click += new EventHandler(this.statusStreamingProvider_Click);
            this.statusStreamingStatus.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusStreamingStatus.ForeColor = Color.Green;
            this.statusStreamingStatus.LinkColor = Color.Red;
            this.statusStreamingStatus.Name = "statusStreamingStatus";
            this.statusStreamingStatus.Size = new Size(0x21, 20);
            this.statusStreamingStatus.Text = "(OK)";
            this.statusStreamingStatus.Visible = false;
            this.statusStreamingStatus.Click += new EventHandler(this.statusStreamingStatus_Click);
            this.statusStreamingSymbolsOff.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusStreamingSymbolsOff.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.statusStreamingSymbolsOff.Image = Resources.StreamingSymbolsOn;
            this.statusStreamingSymbolsOff.ImageTransparentColor = Color.Fuchsia;
            this.statusStreamingSymbolsOff.Name = "statusStreamingSymbolsOff";
            this.statusStreamingSymbolsOff.Size = new Size(20, 20);
            this.statusStreamingSymbolsOff.ToolTipText = "Click to turn off Streaming for Symbols in Status Bar";
            this.statusStreamingSymbolsOff.Visible = false;
            this.statusStreamingSymbolsOff.Click += new EventHandler(this.statusStreamingSymbolsOff_Click);
            this.statusStreamingSymbolsOn.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusStreamingSymbolsOn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.statusStreamingSymbolsOn.Image = Resources.StreamingSymbolsOff;
            this.statusStreamingSymbolsOn.ImageTransparentColor = Color.Fuchsia;
            this.statusStreamingSymbolsOn.Name = "statusStreamingSymbolsOn";
            this.statusStreamingSymbolsOn.Size = new Size(20, 20);
            this.statusStreamingSymbolsOn.ToolTipText = "Click to turn on Streaming for Symbols in Status Bar";
            this.statusStreamingSymbolsOn.Visible = false;
            this.statusStreamingSymbolsOn.Click += new EventHandler(this.statusStreamingSymbolsOn_Click);
            this.statusSofwareDownload.Name = "statusSofwareDownload";
            this.statusSofwareDownload.Size = new Size(0x69, 0x11);
            this.statusSofwareDownload.Text = "Software Download:";
            this.statusSofwareDownload.Visible = false;
            this.statusDownloadProgressBar.Name = "statusDownloadProgressBar";
            this.statusDownloadProgressBar.Size = new Size(100, 0x10);
            this.statusDownloadProgressBar.Style = ProgressBarStyle.Continuous;
            this.statusDownloadProgressBar.ToolTipText = "Software Upgrade Progress";
            this.statusDownloadProgressBar.Visible = false;
            this.stlblHolder.Name = "stlblHolder";
            this.stlblHolder.Size = new Size(0x3b, 0x11);
            this.stlblHolder.Text = "Status: OK";
            this.toolbarNav.BackColor = Color.FromArgb(0xf8, 0xf8, 0xf8);
            this.toolbarNav.GripMargin = new Padding(0);
            this.toolbarNav.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarNav.Items.AddRange(new ToolStripItem[] { this.btnHome, this.dropdownCharts, this.btnStrategyCenter, this.dropdownQuotes, this.btnOrdersAlerts, this.btnAcctsPositions, this.btnDataManager, this.sepDataManager, this.btnIndicators, this.btnFundamental, this.btnHelp, this.btnPreferences, this.btnTrade });
            this.toolbarNav.Location = new Point(0, 0x18);
            this.toolbarNav.Name = "toolbarNav";
            this.toolbarNav.Size = new Size(0x404, 0x19);
            this.toolbarNav.TabIndex = 2;
            this.toolbarNav.Text = "toolStrip1";
            this.btnHome.Image = (Image) manager.GetObject("btnHome.Image");
            this.btnHome.ImageTransparentColor = Color.Magenta;
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new Size(0x36, 0x16);
            this.btnHome.Tag = "P";
            this.btnHome.Text = "Home";
            this.btnHome.Click += new EventHandler(this.btnHome_Click);
            this.dropdownCharts.DropDownItems.AddRange(new ToolStripItem[] { this.mniNewChart2, this.mniNewRules2, this.mniNewCode2, this.mniNewMultiStrategyBuilder2, this.mniOpenStrategy2, this.sepChart2, this.mniChartFront, this.sepChartFront });
            this.dropdownCharts.Image = (Image) manager.GetObject("dropdownCharts.Image");
            this.dropdownCharts.ImageTransparentColor = Color.Magenta;
            this.dropdownCharts.Name = "dropdownCharts";
            this.dropdownCharts.Size = new Size(130, 0x16);
            this.dropdownCharts.Tag = "P";
            this.dropdownCharts.Text = "Charts && Strategies";
            this.dropdownCharts.ToolTipText = "Charts & Strategies";
            this.mniNewChart2.Image = (Image) manager.GetObject("mniNewChart2.Image");
            this.mniNewChart2.Name = "mniNewChart2";
            this.mniNewChart2.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
            this.mniNewChart2.Size = new Size(0x11b, 0x16);
            this.mniNewChart2.Text = "New Chart";
            this.mniNewChart2.Click += new EventHandler(this.mniNewChartDD_Click);
            this.mniNewRules2.Image = (Image) manager.GetObject("mniNewRules2.Image");
            this.mniNewRules2.ImageTransparentColor = Color.Fuchsia;
            this.mniNewRules2.Name = "mniNewRules2";
            this.mniNewRules2.ShortcutKeys = Keys.Control | Keys.Shift | Keys.R;
            this.mniNewRules2.Size = new Size(0x11b, 0x16);
            this.mniNewRules2.Text = "New Strategy from Rules";
            this.mniNewRules2.Click += new EventHandler(this.mniNewStrategyRulesDD_Click);
            this.mniNewCode2.Image = (Image) manager.GetObject("mniNewCode2.Image");
            this.mniNewCode2.ImageTransparentColor = Color.Fuchsia;
            this.mniNewCode2.Name = "mniNewCode2";
            this.mniNewCode2.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            this.mniNewCode2.Size = new Size(0x11b, 0x16);
            this.mniNewCode2.Text = "New Strategy from Code";
            this.mniNewCode2.Click += new EventHandler(this.newStrategyFromCodeToolStripMenuItem_Click);
            this.mniNewMultiStrategyBuilder2.Image = (Image) manager.GetObject("mniNewMultiStrategyBuilder2.Image");
            this.mniNewMultiStrategyBuilder2.Name = "mniNewMultiStrategyBuilder2";
            this.mniNewMultiStrategyBuilder2.ShortcutKeys = Keys.Control | Keys.Shift | Keys.M;
            this.mniNewMultiStrategyBuilder2.Size = new Size(0x11b, 0x16);
            this.mniNewMultiStrategyBuilder2.Text = "New Combination Strategy";
            this.mniNewMultiStrategyBuilder2.Click += new EventHandler(this.mniNewCombinationStrategy_Click);
            this.mniOpenStrategy2.Image = (Image) manager.GetObject("mniOpenStrategy2.Image");
            this.mniOpenStrategy2.ImageTransparentColor = Color.Fuchsia;
            this.mniOpenStrategy2.Name = "mniOpenStrategy2";
            this.mniOpenStrategy2.ShortcutKeys = Keys.Control | Keys.O;
            this.mniOpenStrategy2.Size = new Size(0x11b, 0x16);
            this.mniOpenStrategy2.Text = "Open Strategy ...";
            this.mniOpenStrategy2.Click += new EventHandler(this.btnOpenStrategy_Click);
            this.sepChart2.Name = "sepChart2";
            this.sepChart2.Size = new Size(280, 6);
            this.mniChartFront.Name = "mniChartFront";
            this.mniChartFront.Size = new Size(0x11b, 0x16);
            this.mniChartFront.Text = "Bring all to front";
            this.mniChartFront.Click += new EventHandler(this.mniChartFront_Click);
            this.sepChartFront.Name = "sepChartFront";
            this.sepChartFront.Size = new Size(280, 6);
            this.btnStrategyCenter.Image = (Image) manager.GetObject("btnStrategyCenter.Image");
            this.btnStrategyCenter.ImageTransparentColor = Color.Magenta;
            this.btnStrategyCenter.Name = "btnStrategyCenter";
            this.btnStrategyCenter.Size = new Size(0x6c, 0x16);
            this.btnStrategyCenter.Tag = "P";
            this.btnStrategyCenter.Text = "Strategy Monitor";
            this.btnStrategyCenter.ToolTipText = "Allows you to Activate Strategies and automatically produce Alerts and Trades";
            this.btnStrategyCenter.Click += new EventHandler(this.btnStrategyCenter_Click);
            this.dropdownQuotes.DropDownItems.AddRange(new ToolStripItem[] { this.mniNewQuote2, this.sepQuote2, this.mniQuoteAll, this.sepQuoteFront });
            this.dropdownQuotes.Image = (Image) manager.GetObject("dropdownQuotes.Image");
            this.dropdownQuotes.ImageTransparentColor = Color.Magenta;
            this.dropdownQuotes.Name = "dropdownQuotes";
            this.dropdownQuotes.Size = new Size(0x47, 0x16);
            this.dropdownQuotes.Tag = "P";
            this.dropdownQuotes.Text = "Quotes";
            this.dropdownQuotes.ToolTipText = "Quotes";
            this.mniNewQuote2.Image = (Image) manager.GetObject("mniNewQuote2.Image");
            this.mniNewQuote2.ImageTransparentColor = Color.Fuchsia;
            this.mniNewQuote2.Name = "mniNewQuote2";
            this.mniNewQuote2.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Q;
            this.mniNewQuote2.Size = new Size(250, 0x16);
            this.mniNewQuote2.Text = "New Quote Window";
            this.mniNewQuote2.Click += new EventHandler(this.mniNewQuoteDD_Click);
            this.sepQuote2.Name = "sepQuote2";
            this.sepQuote2.Size = new Size(0xf7, 6);
            this.mniQuoteAll.Name = "mniQuoteAll";
            this.mniQuoteAll.Size = new Size(250, 0x16);
            this.mniQuoteAll.Text = "Bring all to front";
            this.mniQuoteAll.Click += new EventHandler(this.mniQuoteAll_Click);
            this.sepQuoteFront.Name = "sepQuoteFront";
            this.sepQuoteFront.Size = new Size(0xf7, 6);
            this.btnOrdersAlerts.Image = (Image) manager.GetObject("btnOrdersAlerts.Image");
            this.btnOrdersAlerts.ImageTransparentColor = Color.Magenta;
            this.btnOrdersAlerts.Name = "btnOrdersAlerts";
            this.btnOrdersAlerts.Size = new Size(60, 0x16);
            this.btnOrdersAlerts.Tag = "P";
            this.btnOrdersAlerts.Text = "Orders";
            this.btnOrdersAlerts.ToolTipText = "Open the Order Manager";
            this.btnOrdersAlerts.Click += new EventHandler(this.btnOrdersAlerts_Click);
            this.btnAcctsPositions.Image = (Image) manager.GetObject("btnAcctsPositions.Image");
            this.btnAcctsPositions.ImageTransparentColor = Color.Magenta;
            this.btnAcctsPositions.Name = "btnAcctsPositions";
            this.btnAcctsPositions.Size = new Size(0x47, 0x16);
            this.btnAcctsPositions.Tag = "P";
            this.btnAcctsPositions.Text = "Accounts";
            this.btnAcctsPositions.ToolTipText = "Open the Balances and Positions Manager";
            this.btnAcctsPositions.Click += new EventHandler(this.btnAcctsPositions_Click);
            this.btnDataManager.Image = (Image) manager.GetObject("btnDataManager.Image");
            this.btnDataManager.ImageTransparentColor = Color.Magenta;
            this.btnDataManager.Name = "btnDataManager";
            this.btnDataManager.Size = new Size(0x5f, 0x16);
            this.btnDataManager.Tag = "P";
            this.btnDataManager.Text = "Data Manager";
            this.btnDataManager.Click += new EventHandler(this.btnDataManager_Click);
            this.sepDataManager.Name = "sepDataManager";
            this.sepDataManager.Size = new Size(6, 0x19);
            this.btnIndicators.Image = (Image) manager.GetObject("btnIndicators.Image");
            this.btnIndicators.ImageTransparentColor = Color.Magenta;
            this.btnIndicators.Name = "btnIndicators";
            this.btnIndicators.Size = new Size(0x4b, 0x16);
            this.btnIndicators.Tag = "I";
            this.btnIndicators.Text = "Indicators";
            this.btnIndicators.ToolTipText = "Open the Indicators Window";
            this.btnIndicators.Click += new EventHandler(this.btnIndicatorsTB2_Click);
            this.btnFundamental.Image = (Image) manager.GetObject("btnFundamental.Image");
            this.btnFundamental.ImageTransparentColor = Color.Magenta;
            this.btnFundamental.Name = "btnFundamental";
            this.btnFundamental.Size = new Size(0x5e, 0x16);
            this.btnFundamental.Tag = "I";
            this.btnFundamental.Text = "Fundamentals";
            this.btnFundamental.ToolTipText = "Open the Fundamental Items Window";
            this.btnFundamental.Click += new EventHandler(this.btnFundamentalsTB2_Click);
            this.btnFundamental.VisibleChanged += new EventHandler(this.btnFundamental_VisibleChanged);
            this.btnHelp.Alignment = ToolStripItemAlignment.Right;
            this.btnHelp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnHelp.Image = (Image) manager.GetObject("btnHelp.Image");
            this.btnHelp.ImageTransparentColor = Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x17, 0x16);
            this.btnHelp.Text = "toolStripButton1";
            this.btnHelp.ToolTipText = "Wealth-Lab Pro Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnPreferences.Alignment = ToolStripItemAlignment.Right;
            this.btnPreferences.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnPreferences.Image = (Image) manager.GetObject("btnPreferences.Image");
            this.btnPreferences.ImageTransparentColor = Color.Silver;
            this.btnPreferences.Name = "btnPreferences";
            this.btnPreferences.Size = new Size(0x17, 0x16);
            this.btnPreferences.Text = "toolStripButton1";
            this.btnPreferences.ToolTipText = "Preferences";
            this.btnPreferences.Click += new EventHandler(this.btnPreferencesTB_Click);
            this.btnTrade.Alignment = ToolStripItemAlignment.Right;
            this.btnTrade.Checked = true;
            this.btnTrade.CheckState = CheckState.Checked;
            this.btnTrade.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrade.Image = (Image) manager.GetObject("btnTrade.Image");
            this.btnTrade.ImageTransparentColor = Color.Magenta;
            this.btnTrade.Name = "btnTrade";
            this.btnTrade.Size = new Size(0x17, 0x16);
            this.btnTrade.Text = "toolStripButton1";
            this.btnTrade.ToolTipText = "Show/Hide the Trade Ticket";
            this.btnTrade.Visible = false;
            this.btnTrade.Click += new EventHandler(this.btnTradeTicket_Click);
            this.toolbar.BackColor = Color.FromArgb(0xb8, 0xbf, 0xd3);
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { 
                this.toolStripDropDownButton1, this.btnOpenStrategy, this.btnSave, this.btnSaveAs, this.sepChartFile, this.btnLogin, this.sepSpacing, this.lblSpacing, this.btnIncreaseSpacing, this.btnRestoreSpacing, this.btnDecreaseSpacing, this.sepBarSpacing, this.tslblChartStyles, this.btnCandleStyle, this.btnBarChart, this.btnLineChart, 
                this.tsmMoreChartStyles, this.btnLinear, this.btnLog, this.sepChartStyles, this.tslblOptions, this.btnLabelsVisible, this.btnStatusBarVisible, this.btnFundamentalsVisible, this.btnDataWindow, this.btnIndicatorsTB2, this.btnFundamentalsTB2, this.btnClearIndicators, this.btnPushCode, this.btnTradeTicket, this.btnPreferencesTB
             });
            this.toolbar.Location = new Point(0, 0x31);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x404, 0x19);
            this.toolbar.TabIndex = 3;
            this.toolbar.Text = "toolStrip1";
            this.toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { this.mniNewChartDD, this.mniNewStrategyRulesDD, this.newStrategyFromCodeToolStripMenuItem, this.mniNewCombinationStrategy, this.sepNew, this.mniNewWorkspaceTB, this.mniNewQuoteDD, this.sepNew2, this.mniNewDataSetTB });
            this.toolStripDropDownButton1.Image = (Image) manager.GetObject("toolStripDropDownButton1.Image");
            this.toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new Size(0x39, 0x16);
            this.toolStripDropDownButton1.Tag = "*";
            this.toolStripDropDownButton1.Text = "New";
            this.toolStripDropDownButton1.ToolTipText = "Open a New Chart Window";
            this.mniNewChartDD.Image = (Image) manager.GetObject("mniNewChartDD.Image");
            this.mniNewChartDD.Name = "mniNewChartDD";
            this.mniNewChartDD.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
            this.mniNewChartDD.Size = new Size(300, 0x16);
            this.mniNewChartDD.Text = "New Chart Window";
            this.mniNewChartDD.Click += new EventHandler(this.mniNewChartDD_Click);
            this.mniNewStrategyRulesDD.Image = (Image) manager.GetObject("mniNewStrategyRulesDD.Image");
            this.mniNewStrategyRulesDD.ImageTransparentColor = Color.Fuchsia;
            this.mniNewStrategyRulesDD.Name = "mniNewStrategyRulesDD";
            this.mniNewStrategyRulesDD.ShortcutKeys = Keys.Control | Keys.Shift | Keys.R;
            this.mniNewStrategyRulesDD.Size = new Size(300, 0x16);
            this.mniNewStrategyRulesDD.Text = "New Strategy from Rules";
            this.mniNewStrategyRulesDD.Click += new EventHandler(this.mniNewStrategyRulesDD_Click);
            this.newStrategyFromCodeToolStripMenuItem.Image = (Image) manager.GetObject("newStrategyFromCodeToolStripMenuItem.Image");
            this.newStrategyFromCodeToolStripMenuItem.ImageTransparentColor = Color.Fuchsia;
            this.newStrategyFromCodeToolStripMenuItem.Name = "newStrategyFromCodeToolStripMenuItem";
            this.newStrategyFromCodeToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            this.newStrategyFromCodeToolStripMenuItem.Size = new Size(300, 0x16);
            this.newStrategyFromCodeToolStripMenuItem.Text = "New Strategy from Code";
            this.newStrategyFromCodeToolStripMenuItem.Click += new EventHandler(this.newStrategyFromCodeToolStripMenuItem_Click);
            this.mniNewCombinationStrategy.Image = (Image) manager.GetObject("mniNewCombinationStrategy.Image");
            this.mniNewCombinationStrategy.Name = "mniNewCombinationStrategy";
            this.mniNewCombinationStrategy.ShortcutKeys = Keys.Control | Keys.Shift | Keys.M;
            this.mniNewCombinationStrategy.Size = new Size(300, 0x16);
            this.mniNewCombinationStrategy.Text = "New Combination Strategy";
            this.mniNewCombinationStrategy.Click += new EventHandler(this.mniNewCombinationStrategy_Click);
            this.sepNew.Name = "sepNew";
            this.sepNew.Size = new Size(0x129, 6);
            this.mniNewWorkspaceTB.Name = "mniNewWorkspaceTB";
            this.mniNewWorkspaceTB.ShortcutKeys = Keys.Control | Keys.Shift | Keys.W;
            this.mniNewWorkspaceTB.Size = new Size(300, 0x16);
            this.mniNewWorkspaceTB.Text = "New Main Workspace Window";
            this.mniNewWorkspaceTB.Click += new EventHandler(this.mniNewWorkspaceTB_Click);
            this.mniNewQuoteDD.Image = (Image) manager.GetObject("mniNewQuoteDD.Image");
            this.mniNewQuoteDD.ImageTransparentColor = Color.Fuchsia;
            this.mniNewQuoteDD.Name = "mniNewQuoteDD";
            this.mniNewQuoteDD.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Q;
            this.mniNewQuoteDD.Size = new Size(300, 0x16);
            this.mniNewQuoteDD.Text = "New Quote Window";
            this.mniNewQuoteDD.Click += new EventHandler(this.mniNewQuoteDD_Click);
            this.sepNew2.Name = "sepNew2";
            this.sepNew2.Size = new Size(0x129, 6);
            this.mniNewDataSetTB.Image = (Image) manager.GetObject("mniNewDataSetTB.Image");
            this.mniNewDataSetTB.ImageTransparentColor = Color.Fuchsia;
            this.mniNewDataSetTB.Name = "mniNewDataSetTB";
            this.mniNewDataSetTB.ShortcutKeys = Keys.Control | Keys.Shift | Keys.D;
            this.mniNewDataSetTB.Size = new Size(300, 0x16);
            this.mniNewDataSetTB.Text = "New DataSet ...";
            this.mniNewDataSetTB.Click += new EventHandler(this.linkNewDataSet_Click);
            this.btnOpenStrategy.Image = (Image) manager.GetObject("btnOpenStrategy.Image");
            this.btnOpenStrategy.ImageTransparentColor = Color.Magenta;
            this.btnOpenStrategy.Name = "btnOpenStrategy";
            this.btnOpenStrategy.Size = new Size(0x62, 0x16);
            this.btnOpenStrategy.Tag = "*";
            this.btnOpenStrategy.Text = "Open Strategy";
            this.btnOpenStrategy.ToolTipText = "Open an existing Strategy";
            this.btnOpenStrategy.Click += new EventHandler(this.btnOpenStrategy_Click);
            this.btnSave.Image = (Image) manager.GetObject("btnSave.Image");
            this.btnSave.ImageTransparentColor = Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(0x33, 0x16);
            this.btnSave.Tag = "S";
            this.btnSave.Text = "Save";
            this.btnSave.ToolTipText = "Save Strategy";
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.btnSaveAs.Image = (Image) manager.GetObject("btnSaveAs.Image");
            this.btnSaveAs.ImageTransparentColor = Color.Magenta;
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new Size(0x42, 0x16);
            this.btnSaveAs.Tag = "CS";
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.ToolTipText = "Save Strategy As";
            this.btnSaveAs.Click += new EventHandler(this.btnSaveAs_Click);
            this.sepChartFile.Name = "sepChartFile";
            this.sepChartFile.Size = new Size(6, 0x19);
            this.sepChartFile.Tag = "*";
            this.btnLogin.Image = (Image) manager.GetObject("btnLogin.Image");
            this.btnLogin.ImageTransparentColor = Color.Magenta;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new Size(0x69, 0x16);
            this.btnLogin.Tag = "*";
            this.btnLogin.Text = "Log in to Fidelity";
            this.btnLogin.Visible = false;
            this.btnLogin.Click += new EventHandler(this.btnLoginTradeTicket_Click);
            this.sepSpacing.Name = "sepSpacing";
            this.sepSpacing.Size = new Size(6, 0x19);
            this.sepSpacing.Tag = "*";
            this.lblSpacing.Name = "lblSpacing";
            this.lblSpacing.Size = new Size(0x30, 0x16);
            this.lblSpacing.Tag = "CS";
            this.lblSpacing.Text = "Spacing:";
            this.btnIncreaseSpacing.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnIncreaseSpacing.Image = (Image) manager.GetObject("btnIncreaseSpacing.Image");
            this.btnIncreaseSpacing.ImageTransparentColor = Color.Silver;
            this.btnIncreaseSpacing.Name = "btnIncreaseSpacing";
            this.btnIncreaseSpacing.Size = new Size(0x17, 0x16);
            this.btnIncreaseSpacing.Tag = "CS";
            this.btnIncreaseSpacing.Text = "Increase Bar Spacing";
            this.btnIncreaseSpacing.Click += new EventHandler(this.btnIncreaseSpacing_Click);
            this.btnRestoreSpacing.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnRestoreSpacing.Image = (Image) manager.GetObject("btnRestoreSpacing.Image");
            this.btnRestoreSpacing.ImageTransparentColor = Color.Silver;
            this.btnRestoreSpacing.Name = "btnRestoreSpacing";
            this.btnRestoreSpacing.Size = new Size(0x17, 0x16);
            this.btnRestoreSpacing.Tag = "CS";
            this.btnRestoreSpacing.Text = "toolStripButton2";
            this.btnRestoreSpacing.ToolTipText = "Restore Bar Spacing";
            this.btnRestoreSpacing.Click += new EventHandler(this.btnRestoreSpacing_Click);
            this.btnDecreaseSpacing.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnDecreaseSpacing.Image = (Image) manager.GetObject("btnDecreaseSpacing.Image");
            this.btnDecreaseSpacing.ImageTransparentColor = Color.Silver;
            this.btnDecreaseSpacing.Name = "btnDecreaseSpacing";
            this.btnDecreaseSpacing.Size = new Size(0x17, 0x16);
            this.btnDecreaseSpacing.Tag = "CS";
            this.btnDecreaseSpacing.Text = "Decrease Bar Spacing";
            this.btnDecreaseSpacing.Click += new EventHandler(this.btnDecreaseSpacing_Click);
            this.sepBarSpacing.Name = "sepBarSpacing";
            this.sepBarSpacing.Size = new Size(6, 0x19);
            this.sepBarSpacing.Tag = "CS";
            this.tslblChartStyles.Name = "tslblChartStyles";
            this.tslblChartStyles.Size = new Size(0x41, 0x16);
            this.tslblChartStyles.Tag = "CS";
            this.tslblChartStyles.Text = "Chart Style:";
            this.btnCandleStyle.Checked = true;
            this.btnCandleStyle.CheckState = CheckState.Checked;
            this.btnCandleStyle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnCandleStyle.Image = (Image) manager.GetObject("btnCandleStyle.Image");
            this.btnCandleStyle.ImageTransparentColor = Color.Silver;
            this.btnCandleStyle.Name = "btnCandleStyle";
            this.btnCandleStyle.Size = new Size(0x17, 0x16);
            this.btnCandleStyle.Tag = "CS";
            this.btnCandleStyle.Text = "toolStripButton3";
            this.btnCandleStyle.ToolTipText = "Candle Chart Style";
            this.btnCandleStyle.Click += new EventHandler(this.btnLineChart_Click);
            this.btnBarChart.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnBarChart.Image = (Image) manager.GetObject("btnBarChart.Image");
            this.btnBarChart.ImageTransparentColor = Color.Silver;
            this.btnBarChart.Name = "btnBarChart";
            this.btnBarChart.Size = new Size(0x17, 0x16);
            this.btnBarChart.Tag = "CS";
            this.btnBarChart.Text = "toolStripButton2";
            this.btnBarChart.ToolTipText = "Bar Chart Style";
            this.btnBarChart.Click += new EventHandler(this.btnLineChart_Click);
            this.btnLineChart.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLineChart.Image = (Image) manager.GetObject("btnLineChart.Image");
            this.btnLineChart.ImageTransparentColor = Color.Silver;
            this.btnLineChart.Name = "btnLineChart";
            this.btnLineChart.Size = new Size(0x17, 0x16);
            this.btnLineChart.Tag = "CS";
            this.btnLineChart.Text = "toolStripButton1";
            this.btnLineChart.ToolTipText = "Line Chart Style";
            this.btnLineChart.Click += new EventHandler(this.btnLineChart_Click);
            this.tsmMoreChartStyles.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsmMoreChartStyles.Image = (Image) manager.GetObject("tsmMoreChartStyles.Image");
            this.tsmMoreChartStyles.ImageTransparentColor = Color.Magenta;
            this.tsmMoreChartStyles.Name = "tsmMoreChartStyles";
            this.tsmMoreChartStyles.Size = new Size(0x2c, 0x16);
            this.tsmMoreChartStyles.Tag = "CS";
            this.tsmMoreChartStyles.Text = "More";
            this.tsmMoreChartStyles.ToolTipText = "More Chart Styles";
            this.btnLinear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLinear.Image = (Image) manager.GetObject("btnLinear.Image");
            this.btnLinear.ImageTransparentColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.btnLinear.Name = "btnLinear";
            this.btnLinear.Size = new Size(0x17, 0x16);
            this.btnLinear.Tag = "CS";
            this.btnLinear.Text = "toolStripButton1";
            this.btnLinear.ToolTipText = "Linear Axis Scale Chart";
            this.btnLinear.Click += new EventHandler(this.btnLinear_Click);
            this.btnLog.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLog.Image = (Image) manager.GetObject("btnLog.Image");
            this.btnLog.ImageTransparentColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new Size(0x17, 0x16);
            this.btnLog.Tag = "CS";
            this.btnLog.Text = "toolStripButton2";
            this.btnLog.ToolTipText = "Semi-Log Axis Scale Chart";
            this.btnLog.Click += new EventHandler(this.btnLog_Click);
            this.sepChartStyles.Name = "sepChartStyles";
            this.sepChartStyles.Size = new Size(6, 0x19);
            this.sepChartStyles.Tag = "CS";
            this.tslblOptions.Name = "tslblOptions";
            this.tslblOptions.Size = new Size(0x25, 0x16);
            this.tslblOptions.Tag = "CS";
            this.tslblOptions.Text = "Show:";
            this.btnLabelsVisible.Checked = true;
            this.btnLabelsVisible.CheckState = CheckState.Checked;
            this.btnLabelsVisible.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLabelsVisible.Image = (Image) manager.GetObject("btnLabelsVisible.Image");
            this.btnLabelsVisible.ImageTransparentColor = Color.Fuchsia;
            this.btnLabelsVisible.Name = "btnLabelsVisible";
            this.btnLabelsVisible.Size = new Size(0x17, 0x16);
            this.btnLabelsVisible.Tag = "CS";
            this.btnLabelsVisible.Text = "toolStripButton1";
            this.btnLabelsVisible.ToolTipText = "Show Indicator Labels on Chart";
            this.btnLabelsVisible.Click += new EventHandler(this.btnLabelsVisible_Click);
            this.btnStatusBarVisible.Checked = true;
            this.btnStatusBarVisible.CheckState = CheckState.Checked;
            this.btnStatusBarVisible.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnStatusBarVisible.Image = (Image) manager.GetObject("btnStatusBarVisible.Image");
            this.btnStatusBarVisible.ImageTransparentColor = Color.Magenta;
            this.btnStatusBarVisible.Name = "btnStatusBarVisible";
            this.btnStatusBarVisible.Size = new Size(0x17, 0x16);
            this.btnStatusBarVisible.Tag = "CS";
            this.btnStatusBarVisible.Text = "toolStripButton1";
            this.btnStatusBarVisible.ToolTipText = "Show Status Bars on Chart";
            this.btnStatusBarVisible.Click += new EventHandler(this.btnStatusBarVisible_Click);
            this.btnFundamentalsVisible.Checked = true;
            this.btnFundamentalsVisible.CheckState = CheckState.Checked;
            this.btnFundamentalsVisible.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnFundamentalsVisible.Image = (Image) manager.GetObject("btnFundamentalsVisible.Image");
            this.btnFundamentalsVisible.ImageTransparentColor = Color.Magenta;
            this.btnFundamentalsVisible.Name = "btnFundamentalsVisible";
            this.btnFundamentalsVisible.Size = new Size(0x17, 0x16);
            this.btnFundamentalsVisible.Tag = "CS";
            this.btnFundamentalsVisible.Text = "toolStripButton1";
            this.btnFundamentalsVisible.ToolTipText = "Show selected Fundamental Data Items on Chart";
            this.btnFundamentalsVisible.Click += new EventHandler(this.btnFundamentalsVisible_Click);
            this.btnDataWindow.CheckOnClick = true;
            this.btnDataWindow.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnDataWindow.Image = (Image) manager.GetObject("btnDataWindow.Image");
            this.btnDataWindow.ImageTransparentColor = Color.Magenta;
            this.btnDataWindow.Name = "btnDataWindow";
            this.btnDataWindow.Size = new Size(0x17, 0x16);
            this.btnDataWindow.Tag = "CS";
            this.btnDataWindow.Text = "Data Window";
            
            ///WYJ fix: 
            //this.btnDataWindow.Short= "Data Window";
            this.btnDataWindow.Click += new EventHandler(this.btnDataWindow_Click);
            this.btnIndicatorsTB2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnIndicatorsTB2.Image = (Image) manager.GetObject("btnIndicatorsTB2.Image");
            this.btnIndicatorsTB2.ImageTransparentColor = Color.Magenta;
            this.btnIndicatorsTB2.Name = "btnIndicatorsTB2";
            this.btnIndicatorsTB2.Size = new Size(0x17, 0x16);
            this.btnIndicatorsTB2.Tag = "CS";
            this.btnIndicatorsTB2.Text = "toolStripButton1";
            this.btnIndicatorsTB2.ToolTipText = "Plot Technical Indicators";
            this.btnIndicatorsTB2.Click += new EventHandler(this.btnIndicatorsTB2_Click);
            this.btnFundamentalsTB2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnFundamentalsTB2.Image = (Image) manager.GetObject("btnFundamentalsTB2.Image");
            this.btnFundamentalsTB2.ImageTransparentColor = Color.Magenta;
            this.btnFundamentalsTB2.Name = "btnFundamentalsTB2";
            this.btnFundamentalsTB2.Size = new Size(0x17, 0x16);
            this.btnFundamentalsTB2.Tag = "CS";
            this.btnFundamentalsTB2.Text = "toolStripButton2";
            this.btnFundamentalsTB2.ToolTipText = "Plot Fundamental Data Items";
            this.btnFundamentalsTB2.Click += new EventHandler(this.btnFundamentalsTB2_Click);
            this.btnFundamentalsTB2.VisibleChanged += new EventHandler(this.btnFundamentalsTB2_VisibleChanged);
            this.btnClearIndicators.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnClearIndicators.Image = (Image) manager.GetObject("btnClearIndicators.Image");
            this.btnClearIndicators.ImageTransparentColor = Color.Silver;
            this.btnClearIndicators.Name = "btnClearIndicators";
            this.btnClearIndicators.Size = new Size(0x17, 0x16);
            this.btnClearIndicators.Tag = "CS";
            this.btnClearIndicators.Text = "toolStripButton2";
            this.btnClearIndicators.ToolTipText = "Clear Drag and Drop Indicators";
            this.btnClearIndicators.Click += new EventHandler(this.btnClearIndicators_Click);
            this.btnPushCode.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnPushCode.Image = Resources.wlp_push;
            this.btnPushCode.ImageTransparentColor = Color.Magenta;
            this.btnPushCode.Name = "btnPushCode";
            this.btnPushCode.Size = new Size(0x17, 0x16);
            this.btnPushCode.Tag = "CS";
            this.btnPushCode.Text = "toolStripButton1";
            this.btnPushCode.ToolTipText = "Push all Indicators and Fundamental Items into the Strategy Code";
            this.btnPushCode.Click += new EventHandler(this.btnPushCode_Click);
            this.btnTradeTicket.Checked = true;
            this.btnTradeTicket.CheckState = CheckState.Checked;
            this.btnTradeTicket.Image = (Image) manager.GetObject("btnTradeTicket.Image");
            this.btnTradeTicket.ImageTransparentColor = Color.Magenta;
            this.btnTradeTicket.Name = "btnTradeTicket";
            this.btnTradeTicket.Size = new Size(110, 0x16);
            this.btnTradeTicket.Tag = "G";
            this.btnTradeTicket.Text = "Hide Trade Ticket";
            this.btnTradeTicket.ToolTipText = "Show/Hide the Trade Ticket";
            this.btnTradeTicket.Visible = false;
            this.btnTradeTicket.CheckStateChanged += new EventHandler(this.btnTradeTicket_CheckStateChanged);
            this.btnTradeTicket.Click += new EventHandler(this.btnTradeTicket_Click);
            this.btnPreferencesTB.Image = (Image) manager.GetObject("btnPreferencesTB.Image");
            this.btnPreferencesTB.ImageTransparentColor = Color.Magenta;
            this.btnPreferencesTB.Name = "btnPreferencesTB";
            this.btnPreferencesTB.Size = new Size(0x55, 0x16);
            this.btnPreferencesTB.Tag = "G";
            this.btnPreferencesTB.Text = "Preferences";
            this.btnPreferencesTB.Click += new EventHandler(this.btnPreferencesTB_Click);
            this.pnlTree.Controls.Add(this.splitContainerDataPane);
            this.pnlTree.Controls.Add(this.lblScale);
            this.pnlTree.Controls.Add(this.scale);
            this.pnlTree.Controls.Add(this.cmbSymbol);
            this.pnlTree.Controls.Add(this.btnGo);
            this.pnlTree.Controls.Add(this.lblSymbol);
            this.pnlTree.Controls.Add(this.lblPositions);
            this.pnlTree.Controls.Add(this.lblRange);
            this.pnlTree.Controls.Add(this.posSize);
            this.pnlTree.Controls.Add(this.dataRange);
            this.pnlTree.Dock = DockStyle.Left;
            this.pnlTree.Location = new Point(0, 0x4a);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Size = new Size(0xac, 0x180);
            this.pnlTree.TabIndex = 4;
            this.splitContainerDataPane.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerDataPane.BackColor = SystemColors.Control;
            this.splitContainerDataPane.FixedPanel = FixedPanel.Panel2;
            this.splitContainerDataPane.Location = new Point(4, 0x66);
            this.splitContainerDataPane.Name = "splitContainerDataPane";
            this.splitContainerDataPane.Orientation = Orientation.Horizontal;
            this.splitContainerDataPane.Panel1.Controls.Add(this.treeDataSources);
            this.splitContainerDataPane.Panel1.Controls.Add(this.toolbarDataSets);
            this.splitContainerDataPane.Panel1.RightToLeft = RightToLeft.No;
            this.splitContainerDataPane.Panel1MinSize = 100;
            this.splitContainerDataPane.Panel2.Controls.Add(this.toolbarParameters);
            this.splitContainerDataPane.Panel2.Controls.Add(this.pnlParamBase);
            this.splitContainerDataPane.Panel2.RightToLeft = RightToLeft.No;
            this.splitContainerDataPane.Panel2MinSize = 100;
            this.splitContainerDataPane.Size = new Size(0xa2, 0x11a);
            this.splitContainerDataPane.SplitterDistance = 0x97;
            this.splitContainerDataPane.TabIndex = 13;
            this.splitContainerDataPane.TabStop = false;
            //this.splitContainerDataPane.DoubleClick += splitContainerDataPane_DoubleClick; ///WYJ fix

            this.treeDataSources.AllowDrop = true;
            this.treeDataSources.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeDataSources.HideSelection = false;
            this.treeDataSources.ImageIndex = 0;
            this.treeDataSources.Location = new Point(0, 20);
            this.treeDataSources.Name = "treeDataSources";
            this.treeDataSources.SelectedImageIndex = 0;
            this.treeDataSources.Size = new Size(0xa2, 0x83);
            this.treeDataSources.TabIndex = 8;
            this.treeDataSources.DataSourceSelected += new EventHandler<DataSourceEventArgs>(this.method_13);
            this.treeDataSources.SymbolSelected += new EventHandler<DataSourceSymbolEventArgs>(this.method_15);
            this.treeDataSources.NewDataSourceClicked += new EventHandler<EventArgs>(this.linkNewDataSet_Click);
            this.treeDataSources.DataManagerClicked += new EventHandler<EventArgs>(this.btnDataManager_Click);
            this.treeDataSources.IndexManagerClicked += new EventHandler<EventArgs>(this.method_45);
            this.treeDataSources.DataSourceTreeViewRenameClicked += new EventHandler<EventArgs>(this.method_46);
            this.treeDataSources.ItemDrag += new ItemDragEventHandler(this.treeDataSources_ItemDrag);
            this.treeDataSources.DragOver += new DragEventHandler(this.treeDataSources_DragOver);
            this.treeDataSources.DoubleClick += new EventHandler(this.treeDataSources_DoubleClick);
            this.treeDataSources.MouseDown += new MouseEventHandler(this.treeDataSources_MouseDown);
            this.toolbarDataSets.AutoSize = false;
            this.toolbarDataSets.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarDataSets.Items.AddRange(new ToolStripItem[] { this.lblDataSets, this.linkNewDataSet });
            this.toolbarDataSets.Location = new Point(0, 0);
            this.toolbarDataSets.Name = "toolbarDataSets";
            this.toolbarDataSets.Size = new Size(0xa2, 0x13);
            this.toolbarDataSets.TabIndex = 7;
            this.toolbarDataSets.Text = "toolStrip1";
            this.lblDataSets.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblDataSets.Name = "lblDataSets";
            this.lblDataSets.Size = new Size(0x3b, 0x10);
            this.lblDataSets.Text = "DataSets";
            this.linkNewDataSet.Alignment = ToolStripItemAlignment.Right;
            this.linkNewDataSet.IsLink = true;
            this.linkNewDataSet.Name = "linkNewDataSet";
            this.linkNewDataSet.Size = new Size(0x2b, 0x10);
            this.linkNewDataSet.Text = "New ...";
            this.linkNewDataSet.Click += new EventHandler(this.linkNewDataSet_Click);
            this.toolbarParameters.AutoSize = false;
            this.toolbarParameters.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarParameters.Items.AddRange(new ToolStripItem[] { this.lblParameters });
            this.toolbarParameters.Location = new Point(0, 0);
            this.toolbarParameters.Name = "toolbarParameters";
            this.toolbarParameters.Size = new Size(0xa2, 0x16);
            this.toolbarParameters.TabIndex = 9;
            this.toolbarParameters.Text = "Strategy Parameters";
            this.lblParameters.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblParameters.Name = "lblParameters";
            this.lblParameters.Size = new Size(0x7f, 0x13);
            this.lblParameters.Text = "Strategy Parameters";
            this.pnlParamBase.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlParamBase.BackColor = SystemColors.Window;
            this.pnlParamBase.Controls.Add(this.pnlParamBaseLinkParams);
            this.pnlParamBase.Controls.Add(this.paramSliders);
            this.pnlParamBase.Location = new Point(0, 0x16);
            this.pnlParamBase.Name = "pnlParamBase";
            this.pnlParamBase.Size = new Size(0xa2, 0x69);
            this.pnlParamBase.TabIndex = 10;
            this.pnlParamBase.Paint += new PaintEventHandler(this.pnlParamBase_Paint);
            this.pnlParamBaseLinkParams.BackColor = SystemColors.Window;
            this.pnlParamBaseLinkParams.Controls.Add(this.linkRerun);
            this.pnlParamBaseLinkParams.Controls.Add(this.linkResetParams);
            this.pnlParamBaseLinkParams.Controls.Add(this.linkSaveParams);
            this.pnlParamBaseLinkParams.Dock = DockStyle.Bottom;
            this.pnlParamBaseLinkParams.Location = new Point(0, 0x56);
            this.pnlParamBaseLinkParams.Name = "pnlParamBaseLinkParams";
            this.pnlParamBaseLinkParams.Size = new Size(0xa2, 0x13);
            this.pnlParamBaseLinkParams.TabIndex = 0x15;
            this.linkRerun.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.linkRerun.Location = new Point(3, 0);
            this.linkRerun.Name = "linkRerun";
            this.linkRerun.Size = new Size(0x54, 13);
            this.linkRerun.TabIndex = 3;
            this.linkRerun.TabStop = true;
            this.linkRerun.Text = "Re-run Backtest";
            this.linkRerun.Visible = false;
            this.linkRerun.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkRerun_LinkClicked);
            this.linkResetParams.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.linkResetParams.Location = new Point(0x5d, 0);
            this.linkResetParams.Name = "linkResetParams";
            this.linkResetParams.Size = new Size(0x23, 13);
            this.linkResetParams.TabIndex = 1;
            this.linkResetParams.TabStop = true;
            this.linkResetParams.Text = "Reset";
            this.linkResetParams.Visible = false;
            this.linkResetParams.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkResetParams_LinkClicked);
            this.linkSaveParams.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.linkSaveParams.Location = new Point(3, 0);
            this.linkSaveParams.Name = "linkSaveParams";
            this.linkSaveParams.Size = new Size(0x59, 13);
            this.linkSaveParams.TabIndex = 0;
            this.linkSaveParams.TabStop = true;
            this.linkSaveParams.Text = "Save Parameters";
            this.linkSaveParams.Visible = false;
            this.linkSaveParams.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkSaveParams_LinkClicked);
            this.paramSliders.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.paramSliders.ContextMenuStrip = this.popupPreferredValues;
            this.paramSliders.Location = new Point(0, 3);
            this.paramSliders.Name = "paramSliders";
            this.paramSliders.Size = new Size(0xa2, 0x52);
            this.paramSliders.TabIndex = 2;
            this.paramSliders.WealthScript = null;
            this.paramSliders.SliderValueChanged += new EventHandler<EventArgs>(this.method_34);
            this.paramSliders.SliderMouseDown += new EventHandler<EventArgs>(this.method_43);
            this.popupPreferredValues.Items.AddRange(new ToolStripItem[] { this.mniStorePreferredValues });
            this.popupPreferredValues.Name = "popupPreferredValues";
            this.popupPreferredValues.Size = new Size(0x1af, 0x1a);
            this.mniStorePreferredValues.Name = "mniStorePreferredValues";
            this.mniStorePreferredValues.Size = new Size(430, 0x16);
            this.mniStorePreferredValues.Text = "Store these Parameter Values as the Preferred Values for the Symbol(s)";
            this.mniStorePreferredValues.Click += new EventHandler(this.mniStorePreferredValues_Click);
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new Point(3, 4);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new Size(0x25, 13);
            this.lblScale.TabIndex = 12;
            this.lblScale.Text = "Scale:";
            this.scale.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.scale.BackColor = Color.Cornsilk;
            this.scale.Location = new Point(0x4f, 4);
            this.scale.Name = "scale";
            this.scale.Size = new Size(0x55, 20);
            this.scale.SM = false;
            this.scale.TabIndex = 11;
            this.scale.ScaleChanged += new EventHandler<EventArgs>(this.method_33);
            this.cmbSymbol.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.cmbSymbol.FormattingEnabled = true;
            this.cmbSymbol.Location = new Point(0x39, 0x4b);
            this.cmbSymbol.Name = "cmbSymbol";
            this.cmbSymbol.Size = new Size(0x49, 0x15);
            this.cmbSymbol.Sorted = true;
            this.cmbSymbol.TabIndex = 5;
            this.cmbSymbol.DropDownClosed += new EventHandler(this.cmbSymbol_DropDownClosed);
            this.cmbSymbol.KeyPress += new KeyPressEventHandler(this.cmbSymbol_KeyPress);
            this.btnGo.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnGo.Enabled = false;
            this.btnGo.Location = new Point(0x87, 0x4b);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new Size(0x1d, 0x16);
            this.btnGo.TabIndex = 6;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new EventHandler(this.btnGo_Click);
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblSymbol.Location = new Point(3, 0x4e);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new Size(0x33, 13);
            this.lblSymbol.TabIndex = 4;
            this.lblSymbol.Text = "Symbol:";
            this.lblPositions.AutoSize = true;
            this.lblPositions.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblPositions.Location = new Point(3, 0x31);
            this.lblPositions.Name = "lblPositions";
            this.lblPositions.Size = new Size(70, 13);
            this.lblPositions.TabIndex = 2;
            this.lblPositions.Text = "Position Size:";
            this.lblRange.AutoSize = true;
            this.lblRange.Location = new Point(3, 0x1a);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new Size(0x44, 13);
            this.lblRange.TabIndex = 0;
            this.lblRange.Text = "Data Range:";
            this.posSize.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.posSize.BackColor = Color.Honeydew;
            this.posSize.CombinationStrategyChildMode = false;
            this.posSize.Location = new Point(0x4f, 0x30);
            this.posSize.Name = "posSize";
            this.posSize.Size = new Size(0x55, 20);
            this.posSize.TabIndex = 3;
            this.posSize.PositionSizeChanged += new EventHandler<EventArgs>(this.method_10);
            this.dataRange.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.dataRange.BackColor = Color.AliceBlue;
            this.dataRange.IsStreaming = false;
            this.dataRange.Location = new Point(0x4f, 0x1a);
            this.dataRange.Name = "dataRange";
            this.dataRange.Size = new Size(0x55, 20);
            this.dataRange.TabIndex = 1;
            this.dataRange.DataRangeChanged += new EventHandler<EventArgs>(this.method_9);
            this.toolbarDrawing.Dock = DockStyle.Right; ///WYJ fix
            this.toolbarDrawing.GripStyle = ToolStripGripStyle.Hidden;
            //this.toolbarDrawing.Items.AddRange(new ToolStripItem[] { this.btnClearDrawingObjects, this.btnCrossHair, this.sepDeleteDrawing, this.btnTrendline });
            this.mniNewDataSetTB.Visible = false;
            this.mniNewDataSetTB.ShortcutKeys = Keys.Control | Keys.P;
            this.toolbarDrawing.Items.AddRange(new ToolStripItem[] { this.btnClearDrawingObjects, this.btnCrossHair, this.sepDeleteDrawing, this.btnTrendline, this.mniNewDataSetTB });
            this.toolbarDrawing.Location = new Point(0x3ec, 0x4a);
            this.toolbarDrawing.Name = "toolbarDrawing";
            this.toolbarDrawing.Size = new Size(0x18, 0x180);
            this.toolbarDrawing.TabIndex = 0x11;
            this.toolbarDrawing.Text = "toolStrip1";
            this.btnClearDrawingObjects.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnClearDrawingObjects.Image = (Image) manager.GetObject("btnClearDrawingObjects.Image");
            this.btnClearDrawingObjects.ImageTransparentColor = Color.Magenta;
            this.btnClearDrawingObjects.Name = "btnClearDrawingObjects";
            this.btnClearDrawingObjects.Size = new Size(0x15, 20);
            this.btnClearDrawingObjects.Text = "toolStripButton1";
            this.btnClearDrawingObjects.ToolTipText = "Clear Drawing Objects";
            this.btnClearDrawingObjects.Click += new EventHandler(this.btnClearDrawingObjects_Click);
            this.btnCrossHair.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnCrossHair.Image = (Image) manager.GetObject("btnCrossHair.Image");
            this.btnCrossHair.ImageTransparentColor = Color.Magenta;
            this.btnCrossHair.Name = "btnCrossHair";
            this.btnCrossHair.Size = new Size(0x15, 20);
            this.btnCrossHair.Text = "Cross Hair";
            this.btnCrossHair.Click += new EventHandler(this.btnCrossHair_Click);
            this.sepDeleteDrawing.Name = "sepDeleteDrawing";
            this.sepDeleteDrawing.Size = new Size(0x15, 6);
            this.btnTrendline.Name = "btnTrendline";
            this.btnTrendline.Size = new Size(0x15, 4);
            this.splitter.BackColor = SystemColors.ControlDark;
            this.splitter.Location = new Point(0xac, 0x4a);
            this.splitter.Name = "splitter";
            this.splitter.Size = new Size(3, 0x180);
            this.splitter.TabIndex = 5;
            this.splitter.TabStop = false;
            this.saveFileDialog_0.DefaultExt = "ws";
            this.saveFileDialog_0.Filter = "Workspace files|*.ws";
            this.saveFileDialog_0.RestoreDirectory = true;
            this.saveFileDialog_0.Title = "Save Workspace";
            this.openFileDialog_0.DefaultExt = "ws";
            this.openFileDialog_0.Filter = "Workspace files|*.ws";
            this.openFileDialog_0.RestoreDirectory = true;
            this.openFileDialog_0.Title = "Open Workspace";
            this.timer_0.Interval = 0x14d;
            this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
            this.timer_1.Tick += new EventHandler(this.timer_1_Tick);
            this.pnlTrade.BackColor = Color.LightSlateGray;
            this.pnlTrade.Controls.Add(this.accountTypeSelector1);
            this.pnlTrade.Controls.Add(this.lblBid);
            this.pnlTrade.Controls.Add(this.lblAsk);
            this.pnlTrade.Controls.Add(this.lblAsOf);
            this.pnlTrade.Controls.Add(this.lblLast);
            this.pnlTrade.Controls.Add(this.lblAcctType);
            this.pnlTrade.Controls.Add(this.btnDockUp);
            this.pnlTrade.Controls.Add(this.btnCloseTradeTicket);
            this.pnlTrade.Controls.Add(this.btnDockDown);
            this.pnlTrade.Controls.Add(this.btnLoginTradeTicket);
            this.pnlTrade.Controls.Add(this.numTradePrice);
            this.pnlTrade.Controls.Add(this.numTradeQty);
            this.pnlTrade.Controls.Add(this.txtTradeSymbol);
            this.pnlTrade.Controls.Add(this.btnStageOrder);
            this.pnlTrade.Controls.Add(this.btnPlaceOrder);
            this.pnlTrade.Controls.Add(this.lbTradeDirected);
            this.pnlTrade.Controls.Add(this.cmbTradeRoute);
            this.pnlTrade.Controls.Add(this.lblTradeTIF);
            this.pnlTrade.Controls.Add(this.cmbTradeTIF);
            this.pnlTrade.Controls.Add(this.lblTradePrice);
            this.pnlTrade.Controls.Add(this.lblTradeOrder);
            this.pnlTrade.Controls.Add(this.cmbTradeOrder);
            this.pnlTrade.Controls.Add(this.lblTradeQty);
            this.pnlTrade.Controls.Add(this.lblTradeAction);
            this.pnlTrade.Controls.Add(this.cmbTradeAction);
            this.pnlTrade.Controls.Add(this.lblTradeSymbol);
            this.pnlTrade.Controls.Add(this.lblTradeAcct);
            this.pnlTrade.Controls.Add(this.cmbAccount);
            this.pnlTrade.Dock = DockStyle.Top;
            this.pnlTrade.Location = new Point(0xaf, 0x4a);
            this.pnlTrade.Name = "pnlTrade";
            this.pnlTrade.Size = new Size(0x33d, 0x35);
            this.pnlTrade.TabIndex = 0x13;
            this.accountTypeSelector1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.accountTypeSelector1.FormattingEnabled = true;
            this.accountTypeSelector1.IgnoreCalls = false;
            this.accountTypeSelector1.Location = new Point(0x21e, 11);
            this.accountTypeSelector1.Name = "accountTypeSelector1";
            this.accountTypeSelector1.Size = new Size(0x3b, 0x15);
            this.accountTypeSelector1.TabIndex = 0x2f;
            this.lblBid.Location = new Point(0x298, 14);
            this.lblBid.Name = "lblBid";
            this.lblBid.Size = new Size(0x75, 11);
            this.lblBid.TabIndex = 0x2b;
            this.lblBid.Text = "Bid:";
            this.lblBid.Visible = false;
            this.lblAsOf.Location = new Point(0x298, 0x24);
            this.lblAsOf.Name = "lblAsOf";
            this.lblAsOf.Size = new Size(0xa2, 11);
            this.lblAsOf.TabIndex = 0x2c;
            this.lblAsOf.Text = "As of ";
            this.lblAsOf.Visible = false;
            this.lblLast.Location = new Point(0x298, 3);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new Size(0x75, 11);
            this.lblLast.TabIndex = 0x29;
            this.lblLast.Text = "Last:";
            this.lblLast.Visible = false;
            this.lblAcctType.AutoSize = true;
            this.lblAcctType.ForeColor = Color.White;
            this.lblAcctType.Location = new Point(0x21e, 0x23);
            this.lblAcctType.Name = "lblAcctType";
            this.lblAcctType.Size = new Size(0x3e, 13);
            this.lblAcctType.TabIndex = 0x2e;
            this.lblAcctType.Text = "Trade Type";
            this.btnDockUp.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnDockUp.FlatAppearance.BorderSize = 0;
            this.btnDockUp.FlatStyle = FlatStyle.Flat;
            this.btnDockUp.Image = (Image) manager.GetObject("btnDockUp.Image");
            this.btnDockUp.Location = new Point(0x331, 0x29);
            this.btnDockUp.Name = "btnDockUp";
            this.btnDockUp.Size = new Size(12, 12);
            this.btnDockUp.TabIndex = 40;
            this.btnDockUp.UseVisualStyleBackColor = true;
            this.btnDockUp.Visible = false;
            this.btnDockUp.Click += new EventHandler(this.btnDockUp_Click);
            this.btnCloseTradeTicket.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnCloseTradeTicket.FlatAppearance.BorderSize = 0;
            this.btnCloseTradeTicket.FlatStyle = FlatStyle.Flat;
            this.btnCloseTradeTicket.Image = (Image) manager.GetObject("btnCloseTradeTicket.Image");
            this.btnCloseTradeTicket.Location = new Point(0x330, 0);
            this.btnCloseTradeTicket.Name = "btnCloseTradeTicket";
            this.btnCloseTradeTicket.Size = new Size(12, 12);
            this.btnCloseTradeTicket.TabIndex = 0x25;
            this.btnCloseTradeTicket.UseVisualStyleBackColor = true;
            this.btnCloseTradeTicket.Click += new EventHandler(this.btnCloseTradeTicket_Click);
            this.btnDockDown.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnDockDown.FlatAppearance.BorderSize = 0;
            this.btnDockDown.FlatStyle = FlatStyle.Flat;
            this.btnDockDown.Image = (Image) manager.GetObject("btnDockDown.Image");
            this.btnDockDown.Location = new Point(0x330, 0x29);
            this.btnDockDown.Name = "btnDockDown";
            this.btnDockDown.Size = new Size(12, 12);
            this.btnDockDown.TabIndex = 0x27;
            this.btnDockDown.UseVisualStyleBackColor = true;
            this.btnDockDown.Click += new EventHandler(this.btnDockDown_Click);
            this.btnLoginTradeTicket.Location = new Point(0x25c, 10);
            this.btnLoginTradeTicket.Name = "btnLoginTradeTicket";
            this.btnLoginTradeTicket.Size = new Size(0x62, 0x17);
            this.btnLoginTradeTicket.TabIndex = 0x26;
            this.btnLoginTradeTicket.Text = "Log in";
            this.btnLoginTradeTicket.UseVisualStyleBackColor = true;
            this.btnLoginTradeTicket.Click += new EventHandler(this.btnLoginTradeTicket_Click);
            this.numTradePrice.InputType = NumEdit.NumEditType.Double;
            this.numTradePrice.Location = new Point(0x16d, 11);
            this.numTradePrice.Name = "numTradePrice";
            this.numTradePrice.Size = new Size(0x40, 20);
            this.numTradePrice.TabIndex = 5;
            this.numTradePrice.TextChanged += new EventHandler(this.cmbTradeTIF_SelectedIndexChanged);
            this.numTradeQty.InputType = NumEdit.NumEditType.Integer;
            this.numTradeQty.Location = new Point(0xd4, 11);
            this.numTradeQty.Name = "numTradeQty";
            this.numTradeQty.Size = new Size(0x39, 20);
            this.numTradeQty.TabIndex = 3;
            this.numTradeQty.Text = "100";
            this.numTradeQty.TextChanged += new EventHandler(this.cmbTradeTIF_SelectedIndexChanged);
            this.txtTradeSymbol.CharacterCasing = CharacterCasing.Upper;
            this.txtTradeSymbol.Location = new Point(0x69, 11);
            this.txtTradeSymbol.Name = "txtTradeSymbol";
            this.txtTradeSymbol.Size = new Size(0x37, 20);
            this.txtTradeSymbol.TabIndex = 1;
            this.txtTradeSymbol.TextChanged += new EventHandler(this.cmbTradeTIF_SelectedIndexChanged);
            this.txtTradeSymbol.Leave += new EventHandler(this.txtTradeSymbol_Leave);
            this.btnStageOrder.Enabled = false;
            this.btnStageOrder.Location = new Point(0x25c, 0x1c);
            this.btnStageOrder.Name = "btnStageOrder";
            this.btnStageOrder.Size = new Size(0x38, 0x15);
            this.btnStageOrder.TabIndex = 10;
            this.btnStageOrder.Text = "Stage";
            this.btnStageOrder.UseVisualStyleBackColor = true;
            this.btnStageOrder.Visible = false;
            this.btnStageOrder.Click += new EventHandler(this.btnStageOrder_Click);
            this.btnPlaceOrder.Enabled = false;
            this.btnPlaceOrder.Location = new Point(0x25c, 3);
            this.btnPlaceOrder.Name = "btnPlaceOrder";
            this.btnPlaceOrder.Size = new Size(0x38, 0x15);
            this.btnPlaceOrder.TabIndex = 8;
            this.btnPlaceOrder.Text = "Place";
            this.btnPlaceOrder.UseVisualStyleBackColor = true;
            this.btnPlaceOrder.Visible = false;
            this.btnPlaceOrder.Click += new EventHandler(this.btnPlaceOrder_Click);
            this.lbTradeDirected.AutoSize = true;
            this.lbTradeDirected.ForeColor = Color.White;
            this.lbTradeDirected.Location = new Point(0x1ad, 0x23);
            this.lbTradeDirected.Name = "lbTradeDirected";
            this.lbTradeDirected.Size = new Size(0x24, 13);
            this.lbTradeDirected.TabIndex = 0x24;
            this.lbTradeDirected.Text = "Route";
            this.cmbTradeRoute.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTradeRoute.FormattingEnabled = true;
            this.cmbTradeRoute.Location = new Point(0x1ad, 11);
            this.cmbTradeRoute.Name = "cmbTradeRoute";
            this.cmbTradeRoute.Size = new Size(0x39, 0x15);
            this.cmbTradeRoute.TabIndex = 6;
            this.cmbTradeRoute.SelectedIndexChanged += new EventHandler(this.cmbTradeRoute_SelectedIndexChanged);
            this.lblTradeTIF.AutoSize = true;
            this.lblTradeTIF.ForeColor = Color.White;
            this.lblTradeTIF.Location = new Point(0x1e6, 0x23);
            this.lblTradeTIF.Name = "lblTradeTIF";
            this.lblTradeTIF.Size = new Size(0x17, 13);
            this.lblTradeTIF.TabIndex = 0x23;
            this.lblTradeTIF.Text = "TIF";
            this.cmbTradeTIF.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTradeTIF.FormattingEnabled = true;
            this.cmbTradeTIF.Location = new Point(0x1e6, 11);
            this.cmbTradeTIF.Name = "cmbTradeTIF";
            this.cmbTradeTIF.Size = new Size(0x38, 0x15);
            this.cmbTradeTIF.TabIndex = 7;
            this.cmbTradeTIF.SelectedIndexChanged += new EventHandler(this.cmbTradeTIF_SelectedIndexChanged);
            this.lblTradePrice.AutoSize = true;
            this.lblTradePrice.ForeColor = Color.White;
            this.lblTradePrice.Location = new Point(0x16d, 0x23);
            this.lblTradePrice.Name = "lblTradePrice";
            this.lblTradePrice.Size = new Size(0x2b, 13);
            this.lblTradePrice.TabIndex = 0x22;
            this.lblTradePrice.Text = "Amount";
            this.lblTradeOrder.AutoSize = true;
            this.lblTradeOrder.ForeColor = Color.White;
            this.lblTradeOrder.Location = new Point(0x10d, 0x23);
            this.lblTradeOrder.Name = "lblTradeOrder";
            this.lblTradeOrder.Size = new Size(60, 13);
            this.lblTradeOrder.TabIndex = 0x21;
            this.lblTradeOrder.Text = "Order Type";
            this.cmbTradeOrder.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTradeOrder.FormattingEnabled = true;
            this.cmbTradeOrder.Location = new Point(0x10d, 11);
            this.cmbTradeOrder.Name = "cmbTradeOrder";
            this.cmbTradeOrder.Size = new Size(0x60, 0x15);
            this.cmbTradeOrder.TabIndex = 4;
            this.cmbTradeOrder.SelectedIndexChanged += new EventHandler(this.cmbTradeOrder_SelectedIndexChanged);
            this.lblTradeQty.AutoSize = true;
            this.lblTradeQty.ForeColor = Color.White;
            this.lblTradeQty.Location = new Point(0xd4, 0x24);
            this.lblTradeQty.Name = "lblTradeQty";
            this.lblTradeQty.Size = new Size(0x2e, 13);
            this.lblTradeQty.TabIndex = 0x20;
            this.lblTradeQty.Text = "Quantity";
            this.lblTradeAction.AutoSize = true;
            this.lblTradeAction.ForeColor = Color.White;
            this.lblTradeAction.Location = new Point(160, 0x23);
            this.lblTradeAction.Name = "lblTradeAction";
            this.lblTradeAction.Size = new Size(0x25, 13);
            this.lblTradeAction.TabIndex = 0x1f;
            this.lblTradeAction.Text = "Action";
            this.cmbTradeAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTradeAction.FormattingEnabled = true;
            this.cmbTradeAction.Items.AddRange(new object[] { "Buy", "Sell", "Short", "Cover" });
            this.cmbTradeAction.Location = new Point(160, 11);
            this.cmbTradeAction.Name = "cmbTradeAction";
            this.cmbTradeAction.Size = new Size(0x34, 0x15);
            this.cmbTradeAction.TabIndex = 2;
            this.cmbTradeAction.SelectedIndexChanged += new EventHandler(this.cmbTradeAction_SelectedIndexChanged);
            this.lblTradeSymbol.AutoSize = true;
            this.lblTradeSymbol.ForeColor = Color.White;
            this.lblTradeSymbol.Location = new Point(0x69, 0x23);
            this.lblTradeSymbol.Name = "lblTradeSymbol";
            this.lblTradeSymbol.Size = new Size(0x29, 13);
            this.lblTradeSymbol.TabIndex = 30;
            this.lblTradeSymbol.Text = "Symbol";
            this.lblTradeAcct.AutoSize = true;
            this.lblTradeAcct.ForeColor = Color.White;
            this.lblTradeAcct.Location = new Point(6, 0x23);
            this.lblTradeAcct.Name = "lblTradeAcct";
            this.lblTradeAcct.Size = new Size(0x2f, 13);
            this.lblTradeAcct.TabIndex = 0x1d;
            this.lblTradeAcct.Text = "Account";
            this.cmbAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Location = new Point(6, 11);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new Size(0x63, 0x15);
            this.cmbAccount.TabIndex = 0;
            this.cmbAccount.SelectedIndexChanged += new EventHandler(this.cmbAccount_SelectedIndexChanged);
            this.printPreviewDialog.AutoScrollMargin = new Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new Size(0, 0);
            this.printPreviewDialog.ClientSize = new Size(400, 300);
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = (Icon) manager.GetObject("printPreviewDialog.Icon");
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            this.timer_2.Enabled = true;
            this.timer_2.Tick += new EventHandler(this.timer_2_Tick);
            this.assemblyLoader_0.BaseClass = "ChartStyle";
            this.assemblyLoader_0.DLLNameFilter = "";
            this.assemblyLoader_0.Interface = null;
            this.assemblyLoader_0.Path = null;
            this.assemblyLoader_0.PathMask = "*.dll";
            this.drawingObjectManager_0.ChartBookName = "Standard";
            this.drawingObjectManager_0.RootPath = null;
            base.AcceptButton = this.btnGo;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            base.ClientSize = new Size(0x404, 480);
            base.Controls.Add(this.pnlTrade);
            base.Controls.Add(this.splitter);
            base.Controls.Add(this.toolbarDrawing);
            base.Controls.Add(this.pnlTree);
            base.Controls.Add(this.toolbar);
            base.Controls.Add(this.toolbarNav);
            base.Controls.Add(this.status);
            base.Controls.Add(this.menuMain);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.IsMdiContainer = true;
            base.KeyPreview = true;
            base.Location = new Point(20, 20);
            base.MainMenuStrip = this.menuMain;
            this.MinimumSize = new Size(320, 180);
            base.Name = "MainForm";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Application Name";
            base.Activated += new EventHandler(this.MainForm_Activated);
            base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            base.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
            base.Load += new EventHandler(this.MainForm_Load);
            base.MdiChildActivate += new EventHandler(this.MainForm_MdiChildActivate);
            base.KeyDown += new KeyEventHandler(this.MainForm_KeyDown);
            base.PreviewKeyDown += new PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.toolbarNav.ResumeLayout(false);
            this.toolbarNav.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.pnlTree.ResumeLayout(false);
            this.pnlTree.PerformLayout();
            this.splitContainerDataPane.Panel1.ResumeLayout(false);
            this.splitContainerDataPane.Panel2.ResumeLayout(false);
            this.splitContainerDataPane.EndInit();
            this.splitContainerDataPane.ResumeLayout(false);
            this.toolbarDataSets.ResumeLayout(false);
            this.toolbarDataSets.PerformLayout();
            this.toolbarParameters.ResumeLayout(false);
            this.toolbarParameters.PerformLayout();
            this.pnlParamBase.ResumeLayout(false);
            this.pnlParamBaseLinkParams.ResumeLayout(false);
            this.popupPreferredValues.ResumeLayout(false);
            this.toolbarDrawing.ResumeLayout(false);
            this.toolbarDrawing.PerformLayout();
            this.pnlTrade.ResumeLayout(false);
            this.pnlTrade.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        // ///WYJ fix 
        /*
        private void splitContainerDataPane_DoubleClick(object sender, EventArgs e)
        {
            splitContainerDataPane.Panel1Collapsed = !splitContainerDataPane.Panel1Collapsed;
        } */

        public void ItemAdded(ChartForm item)
        {
            this.int_1++;
            this.dropdownCharts.Text = "Charts && Strategies (" + this.int_1 + ")";
            ToolStripMenuItem item2 = new ToolStripMenuItem(item.Text) {
                Text = "Chart",
                Tag = item
            };
            item2.Click += new EventHandler(this.method_28);
            item2.Checked = true;
            this.dropdownCharts.DropDownItems.Add(item2);
        }

        public void ItemAdded(QuotesForm item)
        {
            this.int_2++;
            this.dropdownQuotes.Text = "Quotes (" + this.int_2 + ")";
            ToolStripMenuItem item2 = new ToolStripMenuItem(item.Text) {
                Text = "Quote",
                Tag = item
            };
            item2.Click += new EventHandler(this.method_29);
            item2.Checked = true;
            this.dropdownQuotes.DropDownItems.Add(item2);
        }

        public void ItemChanged(ChartForm item)
        {
            IEnumerator enumerator = this.dropdownCharts.DropDownItems.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ToolStripItem current = (ToolStripItem)enumerator.Current;
                        if (current.Tag == item)
                        {
                            current.Text = item.Text;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public void ItemChanged(QuotesForm item)
        {
            IEnumerator enumerator = this.dropdownQuotes.DropDownItems.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ToolStripItem current = (ToolStripItem)enumerator.Current;
                        if (current.Tag == item)
                        {
                            current.Text = item.Text;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public void ItemRemoved(QuotesForm item)
        {
            IEnumerator enumerator = this.dropdownQuotes.DropDownItems.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ToolStripItem current = (ToolStripItem)enumerator.Current;
                        if (current.Tag == item)
                        {
                            this.dropdownQuotes.DropDownItems.Remove(current);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            MainForm int2 = this;
            int2.int_2 = int2.int_2 - 1;
            if (this.int_2 != 0)
            {
                this.dropdownQuotes.Text = string.Concat("Quotes (", this.int_2, ")");
                return;
            }
            else
            {
                this.dropdownQuotes.Text = "Quotes";
                return;
            }
        }

        public void ItemRemoved(ChartForm item)
        {
            IEnumerator enumerator = this.dropdownCharts.DropDownItems.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ToolStripItem current = (ToolStripItem)enumerator.Current;
                        if (current.Tag == item)
                        {
                            this.dropdownCharts.DropDownItems.Remove(current);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            MainForm int1 = this;
            int1.int_1 = int1.int_1 - 1;
            if (this.int_1 != 0)
            {
                this.dropdownCharts.Text = string.Concat("Charts && Strategies (", this.int_1, ")");
                return;
            }
            else
            {
                this.dropdownCharts.Text = "Charts && Strategies";
                return;
            }
        }

        private void linkNewDataSet_Click(object sender, EventArgs e)
        {
            MainModule.Instance.CreateNewDataSource();
        }

        private void linkRerun_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.btnGo.PerformClick();
        }

        private void linkResetParams_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (base.ActiveMdiChild is IWealthScriptProvider)
            {
                IWealthScriptProvider activeMdiChild = base.ActiveMdiChild as IWealthScriptProvider;
                if (!MainModule.Instance.Strategies.LoadStrategyParameters(activeMdiChild.Strategy, activeMdiChild.WealthScript))
                {
                    foreach (StrategyParameter parameter in activeMdiChild.WealthScript.Parameters)
                    {
                        parameter.Reset();
                    }
                }
                activeMdiChild.ParametersNeedSave = false;
                this.BuildParameterSliders();
            }
            if (this.ActiveChartWindow != null)
            {
                this.btnGo.PerformClick();
            }
        }

        private void linkSaveParams_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (base.ActiveMdiChild is IWealthScriptProvider)
            {
                IWealthScriptProvider activeMdiChild = base.ActiveMdiChild as IWealthScriptProvider;
                if (((activeMdiChild.Strategy != null) && (activeMdiChild.WealthScript != null)) && activeMdiChild.SaveStrategy())
                {
                    MainModule.Instance.Strategies.SaveParameterValues(activeMdiChild.Strategy, activeMdiChild.WealthScript);
                    activeMdiChild.ParametersNeedSave = false;
                    this.lblParameters.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                    this.method_35(activeMdiChild.ParametersNeedSave);
                }
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            mainForm_0 = this;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.bool_2)
            {
                MainModule.Instance.Settings.Set(this, "MainForm");
            }
            MainModule.Instance.Settings.Set("DataTreeWidth", this.pnlTree.Width);
            MainModule.Instance.DataSources.UnregisterObserver(this.treeDataSources);
            Interlocked.Decrement(ref int_0);
            this.method_18();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((this.bool_2 && (int_0 > 1)) && !this.method_19())
            {
                e.Cancel = true;
            }
            else if (this.bool_2)
            {
                this.bool_7 = true;
                StreamingDataProvider streamingProvider = MainModule.Instance.StreamingProvider;
                if ((streamingProvider != null) && streamingProvider.IsConnected)
                {
                    streamingProvider.DisconnectStreaming(this);
                }
                AuthenticationProvider authProvider = MainModule.Instance.AuthProvider;
                if (authProvider != null)
                {
                    authProvider.Close();
                }
            }
        }

        /// <summary>
        /// ///WYJ fix
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Control | Keys.Right:
                case Keys.Control | Keys.Left:
                case Keys.Control | Keys.Up:
                case Keys.Control | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    if (e.Control)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
        }

        private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 0x1b)
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if (activeChartWindow != null)
                {
                    activeChartWindow.PressEscape();
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string applicationName;
            Interlocked.Increment(ref int_0);
            this.bool_2 = int_0 == 1;
            this.method_18();
            mainForm_0 = this;
            this.mniNewQuote.Visible = MainModule.Instance.AuthProvider.AllowStreaming;
            this.mniNewQuoteDD.Visible = MainModule.Instance.AuthProvider.AllowStreaming;
            this.dropdownQuotes.Visible = MainModule.Instance.AuthProvider.AllowStreaming;
            if (IntPtr.Size == 8)
            {
                applicationName = MainModule.Instance.AuthProvider.ApplicationName + "    64-bit Edition";
            }
            else
            {
                applicationName = MainModule.Instance.AuthProvider.ApplicationName;
            }
            this.Text = applicationName;
            this.mniLogin.Text = MainModule.Instance.AuthProvider.LoginPhrase;
            this.mniLogin.Image = MainModule.Instance.AuthProvider.Glyph;
            this.mniAbout.Text = "&About " + MainModule.Instance.AuthProvider.ApplicationName + " ...";
            this.btnHelp.ToolTipText = MainModule.Instance.AuthProvider.ApplicationName + " Help";
            this.btnLabelsVisible.Checked = MainModule.Instance.Settings.Get("IndicatorLabelsVisible", true);
            this.btnStatusBarVisible.Checked = MainModule.Instance.Settings.Get("StatusBarVisible", true);
            this.btnFundamentalsVisible.Checked = MainModule.Instance.Settings.Get("FundamentalItemsVisible", true);
            this.mniOnDemand.Checked = MainModule.Instance.Settings.Get("OnDemandDataEnabled", true);
            MainModule.Instance.DataSources.OnDemandUpdatesEnabled = this.mniOnDemand.Checked;
            this.pnlTree.Width = MainModule.Instance.Settings.Get("DataTreeWidth", 0xac);
            this.SetDataPanelState(false, false, false, false, false);
            if (this.bool_2)
            {
                this.mniViewTradeTicket.Checked = MainModule.Instance.Settings.Get("ShowTradeTicket", true);
                AuthenticationProvider authProvider = MainModule.Instance.AuthProvider;
                authProvider.OnInstallerDownloadComplete += new AuthenticationProvider._DownloadFileCompleted(this.method_36);
                authProvider.OnInstallerDownloadProgressChanged += new AuthenticationProvider._DownloadProgressChanged(this.method_37);
                this.btnLogin.Text = authProvider.LoginPhrase;
                this.btnLogin.Visible = authProvider.LoginButtonVisible;
                this.mniSoftwareUpgrade.Visible = authProvider.SupportsSoftwareUpgrade;
                this.sepUpgrade.Visible = authProvider.SupportsSoftwareUpgrade;
            }
            this.btnLogin.Image = MainModule.Instance.AuthProvider.Glyph;
            this.ShowLoggedInState(MainModule.Instance.IsAuthenticated);
            bool showTradeTicket = MainModule.Instance.AuthProvider.ShowTradeTicket;
            this.pnlTrade.Visible = this.mniViewTradeTicket.Checked && showTradeTicket;
            this.mniViewTradeTicket.Visible = showTradeTicket;
            this.btnTrade.Visible = showTradeTicket;
            this.btnTradeTicket.Visible = showTradeTicket;
            this.method_38(MainModule.Instance.Settings.Get("TradeTicketDockBottom", false));
            this.btnTradeTicket.Checked = this.pnlTrade.Visible;
            this.btnTrade.Checked = this.pnlTrade.Visible;
            this.mniViewNavBar.Checked = MainModule.Instance.Settings.Get("ShowNavBar", true);
            this.toolbarNav.Visible = this.mniViewNavBar.Checked;
            this.mniNavIcons.Checked = MainModule.Instance.Settings.Get("ShowNavBarIcons", false);
            this.ShowNavBarIcons();
            this.mniViewToolbar.Checked = MainModule.Instance.Settings.Get("ShowToolbar", true);
            this.toolbar.Visible = this.mniViewToolbar.Checked;
            this.mniViewDataPanel.Checked = MainModule.Instance.Settings.Get("ShowDataPanel", true);
            this.pnlTree.Visible = this.mniViewDataPanel.Checked;
            this.mniViewDrawingBar.Checked = MainModule.Instance.Settings.Get("ShowDrawingBar", true);
            this.toolbarDrawing.Visible = this.mniViewDrawingBar.Checked;
            this.mniViewStatusBar.Checked = MainModule.Instance.Settings.Get("ShowStatusBar", true);
            this.status.Visible = this.mniViewStatusBar.Checked;
            this.BuildParameterSliders();
            this.posSize.PositionSize = MainModule.Instance.Executor.PosSize;
            this.dataRange.DataRange = MainModule.Instance.DataRange;
            MainModule.Instance.DataSources.RegisterObserver(this.treeDataSources);
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            base.Width = bounds.Width - 40;
            base.Height = bounds.Height - 80;
            this.drawingObjectManager_0.RootPath = MainModule.Instance.DataPath;
            this.treeDataSources.Populate(MainModule.Instance.DataSources);
            if (MainModule.Instance.Settings.Get("ExpandFirstDataSet", true) && (this.treeDataSources.Nodes.Count > 0))
            {
                this.treeDataSources.Nodes[0].Expand();
            }
            this.assemblyLoader_0.Path = Path.GetDirectoryName(Application.ExecutablePath);
            string str2 = MainModule.Instance.Settings.Get("ChartStyleSelected", "CandleStick");
            foreach (System.Type type in this.assemblyLoader_0.Types)
            {
                ChartStyle style = (ChartStyle) this.assemblyLoader_0.CreateInstance(type);
                if (style is CandleChartStyle)
                {
                    this.btnCandleStyle.Tag = style;
                    this.btnCandleStyle.Checked = style.FriendlyName == str2;
                }
                else if (style is BarChartStyle)
                {
                    this.btnBarChart.Tag = style;
                    this.btnBarChart.Checked = style.FriendlyName == str2;
                }
                else if (style is LineChartStyle)
                {
                    this.btnLineChart.Tag = style;
                    this.btnLineChart.Checked = style.FriendlyName == str2;
                }
                else
                {
                    ToolStripButton button = new ToolStripButton(style.FriendlyName, style.Glyph) {
                        DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                    };
                    this.tsmMoreChartStyles.DropDownItems.Add(button);
                    button.ImageTransparentColor = Color.Fuchsia;
                    button.Tag = style;
                    button.Click += new EventHandler(this.btnLineChart_Click);
                    button.Checked = style.FriendlyName == str2;
                }
            }
            this.string_0 = MainModule.Instance.DataPath + @"\Workspaces";
            if (!Directory.Exists(this.string_0))
            {
                Directory.CreateDirectory(this.string_0);
            }
            if (bool_0)
            {
                MainModule.Instance.Settings.Get(this, "MainForm");
                bool_0 = false;
                this.bool_3 = true;
                string path = MainModule.Instance.DataPath + @"\Workspaces\Default.ws";
                if (System.IO.File.Exists(path))
                {
                    this.loadWorkSpace(path);
                    if (base.MdiChildren.Length == 0)
                    {
                        this.method_23("G");
                    }
                }
                else if (MainModule.Instance.Settings.Get("ShowHomePage", true))
                {
                    this.btnHome.PerformClick();
                }
                else
                {
                    this.method_23("G");
                }
            }
            AssemblyLoader loader = new AssemblyLoader {
                BaseClass = "DrawingObjectHelper",
                Path = Path.GetDirectoryName(Application.ExecutablePath)
            };
            for (DrawingObjectHelper.ToolBarGroup group = DrawingObjectHelper.ToolBarGroup.None; group < DrawingObjectHelper.ToolBarGroup.UserDefined; group += 1)
            {
                int num = 0;
                DrawingObjectHelper helper = null;
                foreach (System.Type type2 in loader.Types)
                {
                    helper = (DrawingObjectHelper) loader.CreateInstance(type2);
                    if (helper.Grouping == group)
                    {
                        ToolStripButton button2 = new ToolStripButton(helper.FriendlyName, helper.Glyph, new EventHandler(this.method_11));
                        string str4 = string.Format("{0} \nGroup: {1}", helper.Description, helper.Grouping.ToString());
                        button2.ToolTipText = str4;
                        button2.Tag = helper.DrawingObjectType;
                        button2.DisplayStyle = ToolStripItemDisplayStyle.Image;
                        button2.ImageTransparentColor = Color.White;
                        this.toolbarDrawing.Items.Add(button2);
                        num++;
                    }
                }
                if ((num != 0) && helper.GroupSeparator(group))
                {
                    this.toolbarDrawing.Items.Add(new ToolStripSeparator());
                }
            }
            this.status.Visible = this.IsFirstMainForm && this.mniViewStatusBar.Checked;
            this.mniViewStatusBar.Checked = this.IsFirstMainForm && this.mniViewStatusBar.Checked;
            this.accountTypeSelector1.IgnoreCalls = true;
            this.cmbAccount.Items.Clear();
            foreach (string str5 in MainModule.Instance.AccountNumbers)
            {
                this.cmbAccount.Items.Add(str5);
            }
            this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
            if ((this.cmbAccount.SelectedIndex == -1) && (this.cmbAccount.Items.Count > 0))
            {
                this.cmbAccount.SelectedIndex = 0;
            }
            if (MainModule.Instance.BrokerProvider != null)
            {
                this.cmbTradeRoute.Items.Clear();
                foreach (string str6 in MainModule.Instance.BrokerProvider.Routes)
                {
                    this.cmbTradeRoute.Items.Add(str6);
                }
                if (this.cmbTradeRoute.Items.Count > 0)
                {
                    this.cmbTradeRoute.SelectedIndex = 0;
                }
                this.accountTypeSelector1.IgnoreCalls = false;
                this.accountTypeSelector1.InitAccountTradeType(this.cmbAccount.Text, this.cmbTradeAction.Text);
            }
            this.OrdersUpdated();
            foreach (DynamicMenuItem item in MainModule.Instance.DynamicMenuitems)
            {
                this.AddDynamicMenuItem(item);
            }
            foreach (string str7 in MainModule.Instance.WorkspaceMenuItems)
            {
                this.AddWorkspaceMenuItem(str7);
            }
            FundamentalsLoader loader2 = new FundamentalsLoader {
                DataHost = MainModule.Instance.DataSources
            };
            this.bool_6 = loader2.HasDragDropFundamentals;
            this.btnFundamental.Visible = this.bool_6;
            this.mniFundamentals.Visible = this.bool_6;
            loader2.Dispose();
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            this.btnAcctsPositions.BackColor = this.color_1;
            this.btnOrdersAlerts.BackColor = this.color_1;
            this.btnStrategyCenter.BackColor = this.color_1;
            this.btnHome.BackColor = this.color_1;
            this.dropdownCharts.BackColor = this.color_1;
            this.dropdownQuotes.BackColor = this.color_1;
            this.btnDataManager.BackColor = this.color_1;
            this.BuildParameterSliders();
            if (base.ActiveMdiChild == null)
            {
                this.method_23("G");
                this.EnableControls(true);
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is HomeForm)
            {
                this.method_23("G");
                this.EnableControls(true);
                this.btnHome.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is SymbolManagerForm)
            {
                this.method_23("G");
                this.EnableControls(true);
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is AccountsPositionsForm)
            {
                this.method_23("G");
                this.EnableControls(true);
                this.btnAcctsPositions.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is OrdersAlertsForm)
            {
                this.method_23("G");
                this.EnableControls(true);
                this.btnOrdersAlerts.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is StrategyCenterForm)
            {
                this.method_23("G");
                this.EnableControls(true);
                this.btnStrategyCenter.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is DataManagerForm)
            {
                this.method_23("G");
                this.EnableControls(true);
                this.btnDataManager.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is QuotesForm)
            {
                this.method_23("G");
                this.EnableControls(true);
                this.dropdownQuotes.BackColor = this.color_0;
                foreach (ToolStripItem item3 in this.dropdownQuotes.DropDownItems)
                {
                    ToolStripMenuItem item4 = item3 as ToolStripMenuItem;
                    if (item4 != null)
                    {
                        item4.Checked = item4.Tag == base.ActiveMdiChild;
                    }
                }
                this.SetDataPanelState(false, true, false, false, false);
            }
            else if (base.ActiveMdiChild is StrategyRanking)
            {
                this.method_23("G");
                this.EnableControls(true);
                this.SetDataPanelState(true, true, true, true, false);
                StrategyRanking activeMdiChild = base.ActiveMdiChild as StrategyRanking;
                activeMdiChild.Activate(true);
                if ((activeMdiChild.DataSet != null) && (activeMdiChild.Symbol != ""))
                {
                    this.treeDataSources.SelectSymbol(activeMdiChild.DataSet, activeMdiChild.Symbol);
                    this.cmbSymbol.Text = activeMdiChild.Symbol;
                }
                else if (activeMdiChild.DataSet != null)
                {
                    this.treeDataSources.SelectDataSource(activeMdiChild.DataSet);
                }
                this.method_8();
                if (activeMdiChild.DataRange != null)
                {
                    this.dataRange.DataRange = activeMdiChild.DataRange;
                }
                if (activeMdiChild.PositionSize != null)
                {
                    this.posSize.PositionSize = activeMdiChild.PositionSize;
                }
                WealthLab.BarDataScale barDataScale = activeMdiChild.BarDataScale;
                this.scale.DataScale = activeMdiChild.BarDataScale;
                if (activeMdiChild.MultiSymbolMode)
                {
                    this.cmbSymbol.Text = "";
                }
                activeMdiChild.Activate(false);
            }
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                this.EnableControls(!activeChartWindow.IsBusy);
                this.dropdownCharts.BackColor = this.color_0;
                foreach (ToolStripItem item in this.dropdownCharts.DropDownItems)
                {
                    ToolStripMenuItem item2 = item as ToolStripMenuItem;
                    if (item2 != null)
                    {
                        item2.Checked = item2.Tag == activeChartWindow;
                    }
                }
                if (activeChartWindow.Strategy != null)
                {
                    this.method_23("S");
                    StrategyType strategyType = activeChartWindow.Strategy.StrategyType;
                    this.SetDataPanelState(true, true, true, true, activeChartWindow.Strategy.StrategyType == StrategyType.CombinedStrategy);
                }
                else
                {
                    this.method_23("C");
                    this.SetDataPanelState(true, true, false, false, false);
                }
                if ((activeChartWindow.DataSource != null) && (activeChartWindow.Symbol != ""))
                {
                    this.treeDataSources.SelectSymbol(activeChartWindow.DataSource, activeChartWindow.Symbol);
                    this.cmbSymbol.Text = activeChartWindow.Symbol;
                }
                this.method_8();
                System.Type type = activeChartWindow.ChartStyle.GetType();
                this.btnBarChart.Checked = ((ChartStyle) this.btnBarChart.Tag).GetType() == type;
                this.btnCandleStyle.Checked = ((ChartStyle) this.btnCandleStyle.Tag).GetType() == type;
                this.btnLineChart.Checked = ((ChartStyle) this.btnLineChart.Tag).GetType() == type;
                foreach (ToolStripItem item5 in this.tsmMoreChartStyles.DropDownItems)
                {
                    ToolStripButton button = item5 as ToolStripButton;
                    button.Checked = ((ChartStyle) button.Tag).GetType() == type;
                }
                this.dataRange.DataRange = activeChartWindow.DataRange;
                this.dataRange.IsStreaming = activeChartWindow.IsStreaming;
                this.posSize.PositionSize = activeChartWindow.PositionSize;
                this.scale.DataScale = activeChartWindow.BarDataScale;
                this.btnLabelsVisible.Checked = activeChartWindow.Renderer.IndicatorLabelsVisible;
                this.btnStatusBarVisible.Checked = activeChartWindow.StatusPanelsVisible;
                this.btnFundamentalsVisible.Checked = activeChartWindow.Renderer.FundamentalsVisible;
                if (activeChartWindow.MultiSymbolMode)
                {
                    this.cmbSymbol.Text = "";
                }
                this.btnLinear.Checked = !activeChartWindow.Renderer.LogScale;
                this.btnLog.Checked = !this.btnLinear.Checked;
                this.EnableControls(!activeChartWindow.IsBusy);
            }
            this.EnableEditAndPrintMenu();
        }

        private void method_0()
        {
            if (OrdersAlertsForm.Instance != null)
            {
                OrdersAlertsForm.Instance.MyMainForm.BringToFront();
                OrdersAlertsForm.Instance.BringToFront();
                OrdersAlertsForm.Instance.WindowState = FormWindowState.Normal;
            }
            else
            {
                new OrdersAlertsForm { MdiParent = this }.Show();
            }
        }

        private void method_1()
        {
            new StrategyRanking { MdiParent = this }.Show();
        }

        private void method_10(object sender, EventArgs e)
        {
            MainModule.Instance.Executor.PosSize = this.posSize.PositionSize;
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                if ((activeChartWindow.Strategy == null) || (activeChartWindow.Strategy.StrategyType != StrategyType.CombinedStrategy))
                {
                    activeChartWindow.PositionSize = this.posSize.PositionSize;
                    activeChartWindow.PositionSizeChanged(this.cmbSymbol.Text);
                }
            }
            else if (base.ActiveMdiChild is StrategyRanking)
            {
                StrategyRanking activeMdiChild = base.ActiveMdiChild as StrategyRanking;
                activeMdiChild.PositionSize = this.posSize.PositionSize;
            }
        }

        private void method_11(object sender, EventArgs e)
        {
            this.ClearDrawingObjectSelectedTool();
            ToolStripButton button = sender as ToolStripButton;
            button.Checked = true;
            Chart.TypeOfObjectToDraw = (System.Type) button.Tag;
            this.method_12(Cursors.Cross);
            if (Chart.DisplayCrossHair)
            {
                this.btnCrossHair_Click(sender, e);
            }
        }

        private void method_12(Cursor cursor_0)
        {
            foreach (Form form in base.MdiChildren)
            {
                if (form is ChartForm)
                {
                    (form as ChartForm).SetChartCursor(cursor_0);
                }
            }
        }

        private void method_13(object sender, DataSourceEventArgs e)
        {
            this.btnGo.Enabled = true;
            this.cmbSymbol.Text = "";
            if (base.ActiveMdiChild is StrategyRanking)
            {
                (base.ActiveMdiChild as StrategyRanking).UpdateDataSource(e.DataSource, "");
                this.method_8();
            }
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if ((activeChartWindow != null) && !activeChartWindow.IsBusy)
            {
                activeChartWindow.DataSourceSelected(e.DataSource);
                this.method_8();
                if ((activeChartWindow.Strategy == null) || (activeChartWindow.Strategy.StrategyType != StrategyType.CombinedStrategy))
                {
                    activeChartWindow.SelectTab("DataSet");
                }
                if (activeChartWindow.Optimization != null)
                {
                    activeChartWindow.Optimization.UpdateRunsRequired();
                }
            }
            else
            {
                this.BarDataScale = e.DataSource.BarDataScale;
            }
            this.method_16(true);
        }

        internal void method_14()
        {
            this.treeDataSources.SelectedNode = this.treeDataSources.Nodes[0];
        }

        private void method_15(object sender, DataSourceSymbolEventArgs e)
        {
            if (!this.bool_5)
            {
                this.btnGo.Enabled = true;
                this.cmbSymbol.Text = e.Symbol;
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if ((activeChartWindow != null) && !activeChartWindow.IsBusy)
                {
                    activeChartWindow.ResetStreaming();
                    activeChartWindow.SymbolSelected(e.DataSource, e.Symbol);
                    if (activeChartWindow.AbortedRequest)
                    {
                        Application.DoEvents();
                        this.chartForm_0 = activeChartWindow;
                        this.timer_1.Enabled = true;
                        return;
                    }
                    this.method_8();
                    this.linkRerun.Visible = false;
                    if (activeChartWindow.Optimization != null)
                    {
                        activeChartWindow.Optimization.UpdateRunsRequired();
                    }
                }
                else
                {
                    this.BarDataScale = e.DataSource.BarDataScale;
                }
                if (base.ActiveMdiChild is StrategyRanking)
                {
                    StrategyRanking activeMdiChild = base.ActiveMdiChild as StrategyRanking;
                    if (!activeMdiChild.IsBusy)
                    {
                        activeMdiChild.UpdateDataSource(e.DataSource, e.Symbol);
                    }
                    this.method_8();
                }
                this.method_7();
                this.method_16(false);
            }
        }

        private void method_16(bool bool_10)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.ShowMultiSymbolControls(bool_10);
            }
        }

        private void method_17()
        {
            if (this.treeDataSources.SelectedNode != null)
            {
                bool flag = (this.treeDataSources.SelectedNode.Level == 0) && (this.cmbSymbol.Text == string.Empty);
                this.method_16(flag);
            }
        }

        private void method_18()
        {
            foreach (Form form2 in Application.OpenForms)
            {
                if (form2 is MainForm)
                {
                    MainForm form = form2 as MainForm;
                    form.mniCloseWorkspace.Visible = (int_0 > 1) && !form.bool_2;
                }
            }
        }

        private bool method_19()
        {
            return (bool_1 || ((int_0 == 1) || (MessageBox.Show("Close all Workspaces and shut down Wealth-Lab?", "Exit Wealth-Lab", MessageBoxButtons.YesNo) == DialogResult.Yes)));
        }

        private void method_2()
        {
            this.mniEdit.Enabled = true;
            this.mniCopy.Enabled = true;
            this.mniUndoDelete.Enabled = false;
            this.mniCut.Enabled = false;
            this.mniPaste.Enabled = false;
            this.mniDelete.Enabled = false;
            this.mniSelectAll.Enabled = false;
            this.mniFind.Enabled = false;
            this.mniFindReplace.Enabled = false;
            this.mniSetTemplate.Enabled = false;
        }

        private bool method_20(string string_1)
        {
            List<string> list = new List<string>();
            List<string> items = new List<string>();
            string item = "TradeTicketBottom=";
            if (this.pnlTrade.Dock == DockStyle.Bottom)
            {
                item = item + "Yes";
            }
            else
            {
                item = item + "No";
            }
            list.Add(item);
            string str2 = "TradeTicketState=";
            if (this.pnlTrade.Visible)
            {
                str2 = str2 + "Yes";
            }
            else
            {
                str2 = str2 + "No";
            }
            list.Add(str2);
            if (DataWindowForm.Instance != null)
            {
                list.Add("DataWindow=" + string.Concat(new object[] { DataWindowForm.Instance.Location.X, ",", DataWindowForm.Instance.Location.Y, ",", DataWindowForm.Instance.Size.Width, ",", DataWindowForm.Instance.Size.Height }));
            }
            foreach (Form form in base.MdiChildren)
            {
                if (form is IWorkspace)
                {
                    list.Add("Type=" + form.GetType().Name);
                    list.Add("Bounds=" + string.Concat(new object[] { form.Location.X, ",", form.Location.Y, ",", form.Size.Width, ",", form.Size.Height }));
                    IWorkspace workspace = form as IWorkspace;
                    items.Clear();
                    list.Add("Version=" + workspace.SaveWorkspaceItems(items).ToString());
                    list.Add("ItemCount=" + items.Count.ToString());
                    foreach (string str5 in items)
                    {
                        list.Add(str5);
                    }
                }
            }
            string[] contents = list.ToArray();
            try
            {
                FileNameValidator.ValidateFileName(string_1);
                System.IO.File.WriteAllLines(string_1, contents);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
        }

        //original signature: private void method_21(string string_1)
        private void loadWorkSpace(string filePath)
        {
            string[] strArray2;
            this.bool_8 = false;
            try
            {
                strArray2 = System.IO.File.ReadAllLines(filePath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }
            for (int i = base.MdiChildren.Length - 1; i >= 0; i--)
            {
                base.MdiChildren[i].Close();
            }
            if (DataWindowForm.Instance != null)
            {
                this.btnDataWindow.PerformClick();
            }
            List<string> items = new List<string>();
            int index = 0;
            while (index < strArray2.Length)
            {
                if (strArray2[index].StartsWith("TradeTicketBottom="))
                {
                    this.method_38(strArray2[index].Contains("Yes"));
                    index++;
                    continue;
                }
                if (strArray2[index].StartsWith("TradeTicketState="))
                {
                    this.method_39(strArray2[index].Contains("Yes"));
                    index++;
                    continue;
                }
                if (strArray2[index].StartsWith("DataWindow="))
                {
                    int num2;
                    string[] strArray = this.method_22(strArray2[index++]).Split(new char[] { ',' });
                    Rectangle rectangle = new Rectangle();
                    if (int.TryParse(strArray[0], out num2))
                    {
                        rectangle.X = num2;
                        if (int.TryParse(strArray[1], out num2))
                        {
                            rectangle.Y = num2;
                            if (int.TryParse(strArray[2], out num2))
                            {
                                rectangle.Width = num2;
                                if (int.TryParse(strArray[3], out num2))
                                {
                                    rectangle.Height = num2;
                                    this.btnDataWindow.PerformClick();
                                    if (DataWindowForm.Instance != null)
                                    {
                                        DataWindowForm.Instance.SetBounds(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                                    }
                                }
                            }
                        }
                    }
                    continue;
                }
                string str = this.method_22(strArray2[index++]);
                string str4 = this.method_22(strArray2[index++]);
                Rectangle rectangle2 = new Rectangle();
                string[] strArray3 = str4.Split(new char[] { ',' });
                rectangle2.X = int.Parse(strArray3[0]);
                rectangle2.Y = int.Parse(strArray3[1]);
                rectangle2.Width = int.Parse(strArray3[2]);
                rectangle2.Height = int.Parse(strArray3[3]);
                int version = int.Parse(this.method_22(strArray2[index++]));
                int num4 = int.Parse(this.method_22(strArray2[index++]));
                items.Clear();
                while (num4 > 0)
                {
                    items.Add(strArray2[index++]);
                    num4--;
                }
                Form form = null;
                IWorkspace workspace = null;
                string key = str;
                if (key != null)
                {
                    int num6;
                    if (Class62.dictionary_0 == null)
                    {
                        Dictionary<string, int> dictionary1 = new Dictionary<string, int>(9);
                        dictionary1.Add("ChartForm", 0);
                        dictionary1.Add("HomeForm", 1);
                        dictionary1.Add("DataManagerForm", 2);
                        dictionary1.Add("StrategyCenterForm", 3);
                        dictionary1.Add("QuotesForm", 4);
                        dictionary1.Add("AccountsPositionsForm", 5);
                        dictionary1.Add("OrdersAlertsForm", 6);
                        dictionary1.Add("StrategyRanking", 7);
                        dictionary1.Add("DataWindowForm", 8);
                        Class62.dictionary_0 = dictionary1;
                    }
                    if (Class62.dictionary_0.TryGetValue(key, out num6))
                    {
                        switch (num6)
                        {
                            case 0:
                                form = this.CreateChartWindow(false);
                                break;

                            case 1:
                                form = new HomeForm();
                                break;

                            case 2:
                                form = new DataManagerForm();
                                break;

                            case 3:
                                if (StrategyCenterForm.Instance == null)
                                {
                                    goto Label_0390;
                                }
                                MessageBox.Show("There is a Strategy Monitor already open, cannot open one in this Workspace window");
                                break;

                            case 4:
                                form = new QuotesForm();
                                break;

                            case 5:
                                if (!MainModule.Instance.Authenticate())
                                {
                                    continue;
                                }
                                form = new AccountsPositionsForm();
                                break;

                            case 6:
                                form = new OrdersAlertsForm();
                                break;

                            case 7:
                                form = new StrategyRanking();
                                break;

                            case 8:
                                form = new DataWindowForm();
                                break;
                        }
                    }
                }
                goto Label_03CD;
            Label_0390:
                form = new StrategyCenterForm();
            Label_03CD:
                if (form != null)
                {
                    workspace = form as IWorkspace;
                }
                if ((form != null) && (workspace != null))
                {
                    form.MdiParent = this;
                    form.Show();
                    form.SetBounds(rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height);
                    if (form is ChartForm)
                    {
                        ChartForm form2 = form as ChartForm;
                        if (form2.IsStreaming)
                        {
                            form2.ResetStreaming();
                        }
                        if (this.bool_8)
                        {
                            items[9] = false.ToString();
                        }
                    }
                    workspace.LoadWorkspaceItems(items, version);
                    if (form is ChartForm)
                    {
                        ChartForm form3 = form as ChartForm;
                        this.treeDataSources.SelectSymbol(form3.DataSource, form3.Symbol);
                        form3.GoButtonPressed(form3.Symbol, true);
                    }
                }
            }
            if (base.ActiveMdiChild != null)
            {
                this.MainForm_MdiChildActivate(this, EventArgs.Empty);
            }
        }

        private string method_22(string string_1)
        {
            int index = string_1.IndexOf('=');
            return string_1.Substring(index + 1);
        }

        internal void method_23(string string_1)
        {
            foreach (ToolStripItem item in this.toolbar.Items)
            {
                if (item.Tag != null)
                {
                    if (item.Tag is ChartStyle)
                    {
                        item.Visible = (string_1 == "C") || (string_1 == "S");
                    }
                    else
                    {
                        string tag = item.Tag as string;
                        if (tag != null)
                        {
                            if (tag == "*")
                            {
                                item.Visible = true;
                            }
                            else
                            {
                                item.Visible = tag.Contains(string_1);
                            }
                        }
                    }
                }
            }
            if (this.btnSave.Visible)
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if (activeChartWindow == null)
                {
                    this.btnSave.Visible = false;
                }
                else if (activeChartWindow.Strategy == null)
                {
                    this.btnSave.Visible = false;
                }
                else
                {
                    this.btnSave.Visible = activeChartWindow.Strategy.StrategyType != StrategyType.Compiled;
                }
            }
            this.mniSaveStrategy.Visible = this.btnSave.Visible;
            this.mniSaveStrategyAs.Visible = this.btnSaveAs.Visible;
            this.sepChart.Visible = this.btnSaveAs.Visible;
            if (this.btnTradeTicket.Visible && !this.IsFirstMainForm)
            {
                this.btnTradeTicket.Visible = false;
            }
            if (!MainModule.Instance.AuthProvider.ShowTradeTicket)
            {
                this.btnTradeTicket.Visible = false;
            }
            this.mniDataWindow.Visible = (this.mniDataWindow.Tag as string).Contains(string_1);
            if (!MainModule.Instance.AuthProvider.LoginButtonVisible)
            {
                this.btnLogin.Visible = false;
                this.mniLogin.Visible = false;
                this.sepSpacing.Visible = false;
            }
        }

        private void method_24(object sender, EventArgs e)
        {
            Alert alert = this.method_25();
            MainModule.Instance.TradeManager.AddAlert(alert, false, false);
        }

        private Alert method_25()
        {
            Alert alert = new Alert {
                Account = this.cmbAccount.Text,
                AlertDate = DateTime.Now,
                AlertType = (TradeType) System.Enum.Parse(typeof(TradeType), this.cmbTradeAction.Text)
            };
            string text = this.cmbTradeOrder.Text;
            alert.ExtendedOrderType = "";
            alert.OrderType = OrderType.Market;
            if (text == "Stop")
            {
                alert.OrderType = OrderType.Stop;
            }
            else if (text == "Limit")
            {
                alert.OrderType = OrderType.Limit;
            }
            else if (text != "Market")
            {
                alert.ExtendedOrderType = text;
            }
            alert.Price = (double) this.numTradePrice.Value;
            alert.Shares = (int) this.numTradeQty.Value;
            alert.Symbol = this.txtTradeSymbol.Text;
            alert.Route = this.cmbTradeRoute.Text;
            alert.TIF = this.cmbTradeTIF.Text;
            if (MainModule.Instance.BrokerProvider != null)
            {
                alert.AlertDate = MainModule.Instance.BrokerProvider.ConvertToMarketTimeZone(alert.Symbol, alert.AlertDate);
            }
            alert.AccountTradeType = this.accountTypeSelector1.Text;
            return alert;
        }

        private void method_26(object sender, EventArgs e)
        {
            MainModule.NotImplemented();
        }

        private void method_27(object sender, EventArgs e)
        {
            this.mniViewTradeTicket.PerformClick();
        }

        private void method_28(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            ChartForm tag = (ChartForm) item.Tag;
            tag.BringToFront();
            tag.WindowState = FormWindowState.Normal;
        }

        private void method_29(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            QuotesForm tag = (QuotesForm) item.Tag;
            tag.BringToFront();
            tag.WindowState = FormWindowState.Normal;
        }

        private void method_3(string string_1)
        {
            if (DebugForm.Instance == null)
            {
                DebugForm.Instance = new DebugForm();
                DebugForm.Instance.Show();
            }
            DebugForm.Instance.AddLine(string_1);
        }

        private void method_30(bool bool_10)
        {
            this.statusStreamingProvider.Text = MainModule.Instance.StreamingProvider.FriendlyName;
            this.statusStreamingProvider.Image = Resources.Streaming;
            this.statusStreamingProvider.Visible = true;
            this.statusStreamingProvider.ForeColor = Color.Black;
            this.statusStreamingProvider.IsLink = false;
            this.statusStreamingProvider.LinkBehavior = LinkBehavior.NeverUnderline;
            this.statusStreamingStatus.Text = "(Connected)";
            this.statusStreamingStatus.ForeColor = Color.Green;
            this.statusStreamingStatus.Visible = true;
            this.statusStreamingStatus.IsLink = false;
            this.statusStreamingStatus.LinkBehavior = LinkBehavior.NeverUnderline;
            this.bool_4 = true;
            if (this.IsFirstMainForm)
            {
                this.timer_0.Enabled = true;
                string str = MainModule.Instance.Settings.Get("StreamingSymbols", ".DJI,.IXIC,.SPX");
                string[] symbols = str.Split(new char[] { ',' });
                if (str.Trim() == "")
                {
                    symbols = new string[0];
                }
                this.UpdateStreamingSymbols(symbols);
            }
            this.statusStreamingSymbolsOff.Visible = true;
            this.statusStreamingSymbolsOn.Visible = false;
            if (bool_10)
            {
                foreach (Form form2 in base.MdiChildren)
                {
                    if (form2 is ChartForm)
                    {
                        ChartForm form = form2 as ChartForm;
                        if (form.DisconnectedWhileStreaming)
                        {
                            form.Connect(bool_10);
                        }
                        if (form.IsStreaming)
                        {
                            form.IsStreaming = false;
                            form.IsStreaming = true;
                        }
                    }
                    if (form2 is QuotesForm)
                    {
                        (form2 as QuotesForm).Connect(bool_10);
                    }
                    if (form2 is AccountsPositionsForm)
                    {
                        (form2 as AccountsPositionsForm).Connect(bool_10);
                    }
                }
            }
        }

        private void method_31()
        {
            this.bool_4 = false;
            if (this.IsFirstMainForm)
            {
                this.timer_0.Enabled = false;
            }
            this.statusStreamingStatus.Text = "(Disconnected)";
            this.statusStreamingProvider.Image = Resources.StreamingDisconnect;
            this.statusStreamingStatus.ForeColor = Color.Red;
            this.statusStreamingStatus.IsLink = true;
            this.statusStreamingStatus.LinkBehavior = LinkBehavior.AlwaysUnderline;
            this.statusStreamingProvider.ForeColor = Color.Red;
            this.statusStreamingProvider.IsLink = true;
            this.statusStreamingProvider.LinkBehavior = LinkBehavior.AlwaysUnderline;
            foreach (string str in this.list_1)
            {
                this.streamingQuoteManager_0.Unsubscribe(str);
            }
            this.list_1.Clear();
            foreach (Form form in base.MdiChildren)
            {
                if (form is ChartForm)
                {
                    ChartForm form3 = form as ChartForm;
                    form3.Disconnect();
                    form3.IsStreaming = false;
                }
                if (form is AccountsPositionsForm)
                {
                    (form as AccountsPositionsForm).Disconnect();
                }
            }
        }

        private void method_32(ConnStatus connStatus_0, int int_7, string string_1)
        {
            switch (connStatus_0)
            {
                case ConnStatus.OK:
                    this.statusStreamingStatus.ForeColor = Color.Green;
                    break;

                case ConnStatus.Warning:
                    this.statusStreamingStatus.ForeColor = Color.Olive;
                    break;

                default:
                    this.statusStreamingStatus.ForeColor = Color.Red;
                    break;
            }
            if (int_7 != 0)
            {
                string_1 = int_7 + ": " + string_1;
            }
            this.statusStreamingStatus.Text = string_1;
        }

        private void method_33(object sender, EventArgs e)
        {
            if (this.DataSource != null)
            {
                bool flag = this.DataSource.Provider.SupportsDynamicUpdate(this.scale.DataScale.Scale);
                bool flag2 = this.DataSource.BarDataScale.CanConvertTo(this.scale.DataScale);
                if (!flag && !flag2)
                {
                    MessageBox.Show(string.Format("{0}{1} data scales are not supported by the data provider: {2}", (this.scale.DataScale.BarInterval != 0) ? (this.scale.DataScale.BarInterval.ToString() + " ") : "", this.scale.DataScale.Scale.ToString(), this.DataSource.Provider.FriendlyName), Application.ProductName);
                    this.scale.DataScale = this.scale.PreviousDataScale;
                    return;
                }
            }
            this.ChangeScale(this.scale.DataScale.Scale, this.scale.DataScale.BarInterval);
        }

        private void method_34(object sender, EventArgs e)
        {
            if ((this.ActiveChartWindow == null) || !this.ActiveChartWindow.IsBusy)
            {
                ParameterSlider slider = (ParameterSlider) sender;
                StrategyParameter parameter = slider.Parameter;
                try
                {
                    if (this.ActiveChartWindow != null)
                    {
                        if (!this.ActiveChartWindow.MultiSymbolMode)
                        {
                            if (this.Symbol == "")
                            {
                                this.ActiveChartWindow.RefreshOptimizerViews();
                                return;
                            }
                            this.ActiveChartWindow.ParametersNeedSave = true;
                            if (MainModule.Instance.Settings.Get("SoundsParameters", true))
                            {
                                MainModule.Instance.PlaySound(Resources.shortbep, false);
                            }
                            this.ActiveChartWindow.SliderValueChanging = true;
                            this.btnGo.PerformClick();
                        }
                        if ((this.ActiveChartWindow != null) && this.ActiveChartWindow.MultiSymbolMode)
                        {
                            this.linkRerun.Visible = true;
                        }
                        this.ActiveChartWindow.ParametersNeedSave = true;
                        this.lblParameters.ForeColor = Color.Red;
                        this.method_35(true);
                        this.ActiveChartWindow.RefreshOptimizerViews();
                    }
                    else if (base.ActiveMdiChild is IWealthScriptProvider)
                    {
                        (base.ActiveMdiChild as IWealthScriptProvider).ParametersNeedSave = true;
                        if (MainModule.Instance.Settings.Get("SoundsParameters", true))
                        {
                            MainModule.Instance.PlaySound(Resources.shortbep, false);
                        }
                        this.lblParameters.ForeColor = Color.Red;
                        this.method_35(true);
                    }
                }
                catch
                {
                }
            }
        }

        private void method_35(bool bool_10)
        {
            this.linkSaveParams.Enabled = bool_10;
            this.linkResetParams.Enabled = bool_10;
        }

        private void method_36(object sender, AsyncCompletedEventArgs e)
        {
            this.statusSofwareDownload.Visible = false;
            this.statusDownloadProgressBar.Visible = false;
        }

        private void method_37(object sender, DownloadProgressChangedEventArgs e)
        {
            this.statusSofwareDownload.Visible = true;
            this.statusDownloadProgressBar.Visible = true;
            this.statusDownloadProgressBar.ProgressBar.Value = e.ProgressPercentage;
        }

        private void method_38(bool bool_10)
        {
            if (bool_10)
            {
                this.pnlTrade.Dock = DockStyle.Bottom;
            }
            else
            {
                this.pnlTrade.Dock = DockStyle.Top;
            }
            this.btnDockDown.Visible = !bool_10;
            this.btnDockUp.Visible = bool_10;
        }

        private void method_39(bool bool_10)
        {
            this.mniViewTradeTicket.Checked = bool_10;
            this.pnlTrade.Visible = this.mniViewTradeTicket.Checked;
            this.btnTradeTicket.Checked = this.pnlTrade.Visible;
            this.btnTrade.Checked = this.pnlTrade.Visible;
        }

        private void method_4(string string_1)
        {
            this.statusMessage.Text = string_1;
            this.status.Refresh();
        }

        private void method_40(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string str = this.string_0 + @"\" + item.Text.Replace("&", "") + ".ws";
            this.loadWorkSpace(str);
        }

        private void method_41()
        {
            bool loggedIn = false;
            if (this.cmbAccount.SelectedIndex >= 0)
            {
                string text = this.cmbAccount.Text;
                Account account = MainModule.Instance.TradeManager.FindAccount(text);
                if (account != null)
                {
                    if (account.IsPaperAccount)
                    {
                        loggedIn = true;
                    }
                    else
                    {
                        loggedIn = MainModule.Instance.AuthProvider.LoggedIn;
                    }
                }
                if (MainModule.Instance.BrokerProvider != null)
                {
                    IList<string> list2 = MainModule.Instance.BrokerProvider.RoutesForAccountNumber(text);
                    string str4 = this.cmbTradeRoute.Text;
                    this.cmbTradeRoute.Items.Clear();
                    foreach (string str3 in list2)
                    {
                        this.cmbTradeRoute.Items.Add(str3);
                    }
                    if (this.cmbTradeRoute.Items.Contains(str4))
                    {
                        this.cmbTradeRoute.SelectedIndex = this.cmbTradeRoute.Items.IndexOf(str4);
                    }
                    else
                    {
                        this.cmbTradeRoute.SelectedIndex = 0;
                    }
                    IList<string> list = MainModule.Instance.BrokerProvider.ExtendedOrderTypesAllowed(this.cmbAccount.Text, this.cmbTradeRoute.Text, this.cmbTradeAction.Text);
                    string str5 = this.cmbTradeOrder.Text;
                    this.cmbTradeOrder.Items.Clear();
                    if (MainModule.Instance.BrokerProvider.AllowOrderTypeForRoute(this.cmbTradeRoute.Text, OrderType.Market))
                    {
                        this.cmbTradeOrder.Items.Add("Market");
                    }
                    if (MainModule.Instance.BrokerProvider.AllowOrderTypeForRoute(this.cmbTradeRoute.Text, OrderType.Limit))
                    {
                        this.cmbTradeOrder.Items.Add("Limit");
                    }
                    if (MainModule.Instance.BrokerProvider.AllowOrderTypeForRoute(this.cmbTradeRoute.Text, OrderType.Stop))
                    {
                        this.cmbTradeOrder.Items.Add("Stop");
                    }
                    foreach (string str2 in list)
                    {
                        this.cmbTradeOrder.Items.Add(str2);
                    }
                    if (this.cmbTradeOrder.Items.Contains(str5))
                    {
                        this.cmbTradeOrder.SelectedIndex = this.cmbTradeOrder.Items.IndexOf(str5);
                    }
                    else
                    {
                        this.cmbTradeOrder.SelectedIndex = 0;
                    }
                    MainModule.Instance.BrokerProvider.TifsAllowed(this.cmbAccount.Text, this.cmbTradeRoute.Text, this.cmbTradeOrder.Text);
                    this.accountTypeSelector1.InitAccountTradeType(this.cmbAccount.Text, this.cmbTradeAction.Text);
                }
            }
            if (MainModule.Instance.BrokerProvider != null)
            {
                this.btnLoginTradeTicket.Text = MainModule.Instance.AuthProvider.LoginPhrase;
            }
            this.btnLoginTradeTicket.Visible = !loggedIn;
            this.btnPlaceOrder.Visible = loggedIn;
            this.btnStageOrder.Visible = loggedIn;
            this.lblLast.Visible = loggedIn;
            this.lblBid.Visible = loggedIn;
            this.lblAsk.Visible = loggedIn;
            this.lblAsOf.Visible = loggedIn;
            this.txtTradeSymbol_Leave(null, null);
        }

        internal void method_42(DraggedFundamentalItem draggedFundamentalItem_0)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.method_71(draggedFundamentalItem_0);
            }
        }

        private void method_43(object sender, EventArgs e)
        {
            if (!MainModule.Instance.Settings.Get("DontShowOptMessage", false))
            {
                string label = "You can use the \"Optimize\" link on the bottom of the Strategy Window to Optimize the Strategy Parameter values.  You can also right click here to assign Preferred Parameter Values for this Symbol.";
                DontShowAgainForm form = new DontShowAgainForm("Strategy Parameters", label);
                form.ShowDialog(this);
                MainModule.Instance.Settings.Set("DontShowOptMessage", form.DontShowAgain);
                this.paramSliders.StopMouseDrag();
            }
        }

        private void method_44(int int_7)
        {
            if (this.ActiveChartWindow != null)
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if (activeChartWindow.Strategy != null)
                {
                    Strategy strategy = activeChartWindow.Strategy;
                    while (int_7-- > 0)
                    {
                        this.CloseAndReopenStrategy(activeChartWindow, strategy);
                    }
                }
            }
        }

        private void method_45(object sender, EventArgs e)
        {
            this.CreateIndexManager();
        }

        private void method_46(object sender, EventArgs e)
        {
        }

        internal void method_47()
        {
            this.btnDataWindow.Checked = DataWindowForm.Instance != null;
            this.mniDataWindow.Checked = DataWindowForm.Instance != null;
        }

        private ChartStyle method_5()
        {
            ChartStyle tag = null;
            if (!this.btnBarChart.Checked)
            {
                if (!this.btnCandleStyle.Checked)
                {
                    if (!this.btnLineChart.Checked)
                    {
                        IEnumerator enumerator = this.tsmMoreChartStyles.DropDownItems.GetEnumerator();
                        try
                        {
                            while (true)
                            {
                                if (enumerator.MoveNext())
                                {
                                    ToolStripItem current = (ToolStripItem)enumerator.Current;
                                    if (current is ToolStripButton)
                                    {
                                        ToolStripButton toolStripButton = current as ToolStripButton;
                                        if (toolStripButton.Checked)
                                        {
                                            tag = (ChartStyle)toolStripButton.Tag;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable = enumerator as IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                    }
                    else
                    {
                        tag = (ChartStyle)this.btnLineChart.Tag;
                    }
                }
                else
                {
                    tag = (ChartStyle)this.btnCandleStyle.Tag;
                }
            }
            else
            {
                tag = (ChartStyle)this.btnBarChart.Tag;
            }
            if (tag != null)
            {
                Type type = tag.GetType();
                tag = (ChartStyle)this.assemblyLoader_0.CreateInstance(type);
                return tag;
            }
            else
            {
                return null;
            }
        }

        private ChartStyle method_6(string string_1)
        {
            ChartStyle chartStyle = null;
            ChartStyle tag = (ChartStyle)this.btnBarChart.Tag;
            if (tag.FriendlyName != string_1)
            {
                tag = (ChartStyle)this.btnCandleStyle.Tag;
                if (tag.FriendlyName != string_1)
                {
                    tag = (ChartStyle)this.btnLineChart.Tag;
                    if (tag.FriendlyName != string_1)
                    {
                        IEnumerator enumerator = this.tsmMoreChartStyles.DropDownItems.GetEnumerator();
                        try
                        {
                            while (true)
                            {
                                if (enumerator.MoveNext())
                                {
                                    ToolStripItem current = (ToolStripItem)enumerator.Current;
                                    if (current is ToolStripButton)
                                    {
                                        tag = (ChartStyle)current.Tag;
                                        if (tag.FriendlyName == string_1)
                                        {
                                            chartStyle = tag;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable = enumerator as IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                    }
                    else
                    {
                        chartStyle = tag;
                    }
                }
                else
                {
                    chartStyle = tag;
                }
            }
            else
            {
                chartStyle = tag;
            }
            if (chartStyle != null)
            {
                Type type = chartStyle.GetType();
                chartStyle = (ChartStyle)this.assemblyLoader_0.CreateInstance(type);
                return chartStyle;
            }
            else
            {
                return null;
            }
        }

        private void method_7()
        {
            foreach (Form form2 in base.MdiChildren)
            {
                if (form2 is ChartForm)
                {
                    ChartForm form = form2 as ChartForm;
                    if ((form != this.ActiveChartWindow) && form.LinkedToSymbol)
                    {
                        form.ResetStreaming();
                        form.DataSource = this.DataSource;
                        form.GoButtonPressed(this.cmbSymbol.Text, false);
                    }
                }
            }
        }

        private void method_8()
        {
            if (this.DataSource != null)
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if (activeChartWindow != null)
                {
                    WealthLab.BarDataScale barDataScale = activeChartWindow.BarDataScale;
                    this.BarDataScale = barDataScale;
                }
            }
        }

        private void method_9(object sender, EventArgs e)
        {
            MainModule.Instance.DataRange = this.dataRange.DataRange;
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.DataRange = this.dataRange.DataRange;
                activeChartWindow.ResetStreaming();
                if ((activeChartWindow.Strategy == null) || (activeChartWindow.Strategy.StrategyType != StrategyType.CombinedStrategy))
                {
                    activeChartWindow.GoButtonPressed(this.cmbSymbol.Text, true);
                }
            }
            else if (base.ActiveMdiChild is StrategyRanking)
            {
                StrategyRanking activeMdiChild = base.ActiveMdiChild as StrategyRanking;
                activeMdiChild.DataRange = this.dataRange.DataRange;
            }
        }

        private void mniAbout_Click(object sender, EventArgs e)
        {
            using (AboutBox1 box = new AboutBox1())
            {
                box.ShowDialog();
            }
        }

        private void mniCascade_Click(object sender, EventArgs e)
        {
            base.LayoutMdi(MdiLayout.Cascade);
        }

        private void mniChartFront_Click(object sender, EventArgs e)
        {
            bool flag = false;
            foreach (Form form in base.MdiChildren)
            {
                if (form is ChartForm)
                {
                    form.BringToFront();
                    form.WindowState = FormWindowState.Normal;
                    flag = true;
                }
            }
            if (!flag)
            {
                this.mniNewChart.PerformClick();
            }
        }

        private void mniClose_Click(object sender, EventArgs e)
        {
            if (base.ActiveMdiChild != null)
            {
                base.ActiveMdiChild.Close();
            }
        }

        private void mniCloseWorkspace_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void mniCopy_Click(object sender, EventArgs e)
        {
            if (base.ActiveMdiChild is ChartForm)
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if (activeChartWindow != null)
                {
                    activeChartWindow.EditCopy();
                }
            }
            else if (base.ActiveMdiChild is AccountsPositionsForm)
            {
                AccountsPositionsForm activeMdiChild = base.ActiveMdiChild as AccountsPositionsForm;
                if (activeMdiChild != null)
                {
                    activeMdiChild.CopyToClipboard();
                }
            }
            else if (base.ActiveMdiChild is OrdersAlertsForm)
            {
                OrdersAlertsForm form5 = base.ActiveMdiChild as OrdersAlertsForm;
                if (form5 != null)
                {
                    form5.CopyToClipboard();
                }
            }
            else if (base.ActiveMdiChild is StrategyCenterForm)
            {
                StrategyCenterForm form3 = base.ActiveMdiChild as StrategyCenterForm;
                if (form3 != null)
                {
                    form3.CopyToClipboard();
                }
            }
            else if (base.ActiveMdiChild is QuotesForm)
            {
                QuotesForm form2 = base.ActiveMdiChild as QuotesForm;
                if (form2 != null)
                {
                    form2.CopyToClipboard();
                }
            }
            else if (base.ActiveMdiChild is StrategyRanking)
            {
                StrategyRanking ranking = base.ActiveMdiChild as StrategyRanking;
                if (ranking != null)
                {
                    ranking.CopyToClipboard();
                }
            }
        }

        private void mniCut_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.EditCut();
            }
        }

        private void mniDebug_Click(object sender, EventArgs e)
        {
            if (DebugForm.Instance == null)
            {
                DebugForm.Instance = new DebugForm();
                DebugForm.Instance.Show();
            }
            DebugForm.Instance.BringToFront();
        }

        private void mniDelete_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.EditDelete();
            }
        }

        private void mniExit_Click(object sender, EventArgs e)
        {
            bool_1 = true;
            Application.Exit();
        }

        private void mniFidelityCom_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.fidelity.com");
        }

        private void mniFind_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.EditFind();
            }
        }

        private void mniFindReplace_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.EditFindReplace();
            }
        }

        private void mniFundamentals_VisibleChanged(object sender, EventArgs e)
        {
            if (this.mniFundamentals.Visible && !this.bool_6)
            {
                this.mniFundamentals.Visible = this.bool_6;
            }
        }

        private void mniIndexManager_Click(object sender, EventArgs e)
        {
            this.CreateIndexManager();
        }

        private void mniLanguageGuide_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + @"\WealthScriptGuide.chm";
            if (System.IO.File.Exists(path))
            {
                Help.ShowHelp(this, path);
            }
            else
            {
                MessageBox.Show("The WealthScript Language Guide is not currently available.  Please check back in a subsequent release.", "WealthScript Language Guide", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void mniLoadWorkSpace_Click(object sender, EventArgs e)
        {
            this.openFileDialog_0.InitialDirectory = this.string_0;
            if (this.openFileDialog_0.ShowDialog() != DialogResult.Cancel)
            {
                string fileName = this.openFileDialog_0.FileName;
                GC.Collect();
                this.loadWorkSpace(fileName);
            }
        }

        private void mniNavIcons_Click(object sender, EventArgs e)
        {
            this.mniNavIcons.Checked = !this.mniNavIcons.Checked;
            this.ShowNavBarIcons();
            MainModule.Instance.Settings.Set("ShowNavBarIcons", this.mniNavIcons.Checked);
        }

        private void mniNewBuilder_Click(object sender, EventArgs e)
        {
            this.CreateNewStrategyWindow(false);
        }

        private void mniNewChartDD_Click(object sender, EventArgs e)
        {
            this.CreateChartWindow(true);
        }

        private void mniNewCombinationStrategy_Click(object sender, EventArgs e)
        {
            this.CreateNewMultiStrategyWindow();
        }

        private void mniNewQuoteDD_Click(object sender, EventArgs e)
        {
            new QuotesForm { MdiParent = this, AutoTradingMode = AutoTradingMode.Live }.Show();
        }

        private void mniNewStrategyRulesDD_Click(object sender, EventArgs e)
        {
            this.CreateNewStrategyWindow(false);
        }

        private void mniNewWorkspaceTB_Click(object sender, EventArgs e)
        {
            MainForm form = new MainForm();
            form.Show();
            form.method_23("G");
        }

        private void mniOnDemand_Click(object sender, EventArgs e)
        {
            MainModule.Instance.SetOnDemand(!this.mniOnDemand.Checked);
        }

        private void mniPaste_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.EditPaste();
            }
        }

        private void mniPrint_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (base.ActiveMdiChild != null)
            {
                if (base.ActiveMdiChild is ChartForm)
                {
                    ChartForm activeChartWindow = this.ActiveChartWindow;
                    if (activeChartWindow != null)
                    {
                        activeChartWindow.Print();
                    }
                }
                else if (base.ActiveMdiChild is AccountsPositionsForm)
                {
                    AccountsPositionsForm activeMdiChild = base.ActiveMdiChild as AccountsPositionsForm;
                    if (activeMdiChild != null)
                    {
                        activeMdiChild.Print();
                    }
                }
                else if (base.ActiveMdiChild is OrdersAlertsForm)
                {
                    OrdersAlertsForm form5 = base.ActiveMdiChild as OrdersAlertsForm;
                    if (form5 != null)
                    {
                        form5.Print();
                    }
                }
                else if (base.ActiveMdiChild is StrategyCenterForm)
                {
                    StrategyCenterForm form2 = base.ActiveMdiChild as StrategyCenterForm;
                    if (form2 != null)
                    {
                        form2.Print();
                    }
                }
                else if (base.ActiveMdiChild is QuotesForm)
                {
                    QuotesForm form = base.ActiveMdiChild as QuotesForm;
                    if (form != null)
                    {
                        form.Print();
                    }
                }
                else if (base.ActiveMdiChild is StrategyRanking)
                {
                    StrategyRanking ranking = base.ActiveMdiChild as StrategyRanking;
                    if (ranking != null)
                    {
                        ranking.Print();
                    }
                }
            }
        }

        private void mniQuickRef2_Click(object sender, EventArgs e)
        {
            if (QuickRefForm.Instance == null)
            {
                new QuickRefForm().Show();
            }
            else
            {
                QuickRefForm.Instance.BringToFront();
            }
        }

        private void mniQuoteAll_Click(object sender, EventArgs e)
        {
            foreach (Form form in base.MdiChildren)
            {
                if (form is QuotesForm)
                {
                    form.BringToFront();
                    form.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void mniSaveWorkSpace_Click(object sender, EventArgs e)
        {
            this.saveFileDialog_0.InitialDirectory = this.string_0;
            if (this.saveFileDialog_0.ShowDialog() != DialogResult.Cancel)
            {
                this.method_20(this.saveFileDialog_0.FileName);
            }
        }

        private void mniSelectAll_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.SelectAll();
            }
        }

        private void mniSetDefaultWorkspace_Click(object sender, EventArgs e)
        {
            string path = MainModule.Instance.DataPath + @"\Workspaces";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (this.method_20(path + @"\Default.ws"))
            {
                MessageBox.Show("Default Workspace saved");
            }
        }

        private void mniSetTemplate_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if ((activeChartWindow != null) && (MessageBox.Show("Do you want the code that is currently in the Editor window to be used whenever a new Strategy window is created?", "Set Default Template Code", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                MainModule.Instance.StrategyTemplateCode = activeChartWindow.EditorCode;
                if (activeChartWindow.Strategy != null)
                {
                    MainModule.Instance.Settings.Set("TemplateReferences", activeChartWindow.Strategy.References);
                    MainModule.Instance.SaveSettings();
                }
                MessageBox.Show("Template Code was set");
            }
        }

        private void mniSoftwareUpgrade_Click(object sender, EventArgs e)
        {
            if (this.statusSofwareDownload.Visible)
            {
                MessageBox.Show(this, "A software upgrade download is in progress.", Application.ProductName);
            }
            else if (!MainModule.Instance.AuthProvider.DoUpgradeCheck())
            {
                MessageBox.Show(this, "The installed version of " + Application.ProductName + " is current.", Application.ProductName);
            }
        }

        private void mniStorePreferredValues_Click(object sender, EventArgs e)
        {
            if (this.ActiveChartWindow != null)
            {
                if (!this.ActiveChartWindow.MultiSymbolMode)
                {
                    this.ActiveChartWindow.Strategy.StorePreferredValues(this.ActiveChartWindow.Symbol, this.ActiveChartWindow.WealthScript);
                    this.ActiveChartWindow.NeedSave = true;
                }
                else
                {
                    foreach (string str in this.ActiveChartWindow.DataSource.Symbols)
                    {
                        this.ActiveChartWindow.Strategy.StorePreferredValues(str, this.ActiveChartWindow.WealthScript);
                    }
                    this.ActiveChartWindow.NeedSave = true;
                }
            }
        }

        private void mniStrategyRanking_Click(object sender, EventArgs e)
        {
            this.OpenStrategyRanking();
        }

        private void mniTileHorizontally_Click(object sender, EventArgs e)
        {
            base.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void mniTileVertically_Click(object sender, EventArgs e)
        {
            base.LayoutMdi(MdiLayout.TileVertical);
        }

        private void mniUndoDelete_Click(object sender, EventArgs e)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.EditUndo();
            }
        }

        private void mniViewDataPanel_Click(object sender, EventArgs e)
        {
            this.mniViewDataPanel.Checked = !this.mniViewDataPanel.Checked;
            this.pnlTree.Visible = this.mniViewDataPanel.Checked;
            MainModule.Instance.Settings.Set("ShowDataPanel", this.mniViewDataPanel.Checked);
        }

        private void mniViewDrawingBar_Click(object sender, EventArgs e)
        {
            this.mniViewDrawingBar.Checked = !this.mniViewDrawingBar.Checked;
            this.toolbarDrawing.Visible = this.mniViewDrawingBar.Checked;
            MainModule.Instance.Settings.Set("ShowDrawingBar", this.mniViewDrawingBar.Checked);
        }

        private void mniViewNavBar_Click(object sender, EventArgs e)
        {
            this.mniViewNavBar.Checked = !this.mniViewNavBar.Checked;
            this.toolbarNav.Visible = this.mniViewNavBar.Checked;
            MainModule.Instance.Settings.Set("ShowNavBar", this.mniViewNavBar.Checked);
        }

        private void mniViewStatusBar_Click(object sender, EventArgs e)
        {
            this.mniViewStatusBar.Checked = !this.mniViewStatusBar.Checked;
            this.status.Visible = this.mniViewStatusBar.Checked;
            MainModule.Instance.Settings.Set("ShowStatusBar", this.mniViewStatusBar.Checked);
        }

        private void mniViewToolbar_Click(object sender, EventArgs e)
        {
            this.mniViewToolbar.Checked = !this.mniViewToolbar.Checked;
            this.toolbar.Visible = this.mniViewToolbar.Checked;
            MainModule.Instance.Settings.Set("ShowToolbar", this.mniViewToolbar.Checked);
        }

        private void mniWealthLabCom_Click(object sender, EventArgs e)
        {
            string str = "http://www.wealth-lab.com";
            if (MainModule.Instance.NavigateToThirdPartySite(str))
            {
                Process.Start(str);
            }
        }

        private void newStrategyFromCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateNewStrategyWindow(true);
        }

        public void OpenOrderManager()
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate34(this.method_0));
            }
            else
            {
                this.method_0();
            }
        }

        public void OpenStrategyCenter()
        {
            if (StrategyCenterForm.Instance != null)
            {
                StrategyCenterForm.Instance.MyMainForm.BringToFront();
                StrategyCenterForm.Instance.BringToFront();
                StrategyCenterForm.Instance.WindowState = FormWindowState.Normal;
            }
            else
            {
                new StrategyCenterForm { MdiParent = this }.Show();
            }
        }

        public void OpenStrategyExplorer()
        {
            StrategyExplorerForm form = new StrategyExplorerForm {
                Text = "Select one or more Strategies to Open"
            };
            switch (form.ShowDialog())
            {
                case DialogResult.Yes:
                    this.CreateNewStrategyWindow(true).Show();
                    return;

                case DialogResult.No:
                    this.CreateNewStrategyWindow(false).Show();
                    break;

                case DialogResult.OK:
                    foreach (Strategy strategy in form.StrategiesSelected)
                    {
                        Strategy strategy2;
                        MainModule.Instance.Strategies.Strategies.Remove(strategy);
                        if (strategy.StrategyType != StrategyType.Compiled)
                        {
                            try
                            {
                                strategy2 = Strategy.FromFile(strategy.FileName);
                            }
                            catch (FileNotFoundException)
                            {
                                MessageBox.Show("Could not open the Strategy: \"" + strategy.Name + "\".  It was possibly moved out of this folder outside Wealth-Lab.  Please restart Wealth-Lab.", "Open Strategy");
                                break;
                            }
                            strategy2.Folder = strategy.Folder;
                            strategy2.FileName = strategy.FileName;
                            strategy2.ID = strategy.ID;
                            strategy2.Description = strategy.Description;
                        }
                        else
                        {
                            strategy2 = strategy;
                        }
                        ChartForm form2 = this.OpenStrategyWindow(strategy2);
                        MainModule.Instance.Strategies.Strategies.Add(strategy2);
                        form2.Show();
                        MainModule.Instance.Strategies.LoadStrategyParameters(form2.Strategy, form2.WealthScript);
                    }
                    this.BuildParameterSliders();
                    return;

                default:
                    return;
            }
        }

        public void OpenStrategyRanking()
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate35(this.method_1));
            }
            else
            {
                this.method_1();
            }
        }

        public ChartForm OpenStrategyWindow(Strategy strategy_0)
        {
            return this.OpenStrategyWindow(strategy_0, true);
        }

        public ChartForm OpenStrategyWindow(Strategy strategy_0, bool executeOnSelectedSymbol)
        {
            return this.OpenStrategyWindow(strategy_0, executeOnSelectedSymbol, true);
        }

        public ChartForm OpenStrategyWindow(Strategy strategy_0, bool executeOnSelectedSymbol, bool useAdvancedSettings)
        {
            ChartForm form = this.CreateChartWindow(false);
            form.Strategy = strategy_0;
            this.method_23("S");
            if (strategy_0.StrategyType == StrategyType.CombinedStrategy)
            {
                form.SelectTab("Combination Strategy");
            }
            else
            {
                form.SelectTab("Chart");
            }
            if (useAdvancedSettings && MainModule.Instance.Settings.Get("RememberStrategyScale", false))
            {
                this.BarDataScale = strategy_0.DataScale;
                form.SetBarDataScaleForDataSource(this.DataSource, strategy_0.DataScale);
            }
            if (useAdvancedSettings && MainModule.Instance.Settings.Get("RememberStrategyPositionSize", false))
            {
                this.posSize.PositionSize = strategy_0.PositionSize;
                form.PositionSize = strategy_0.PositionSize;
            }
            if (useAdvancedSettings && MainModule.Instance.Settings.Get("RememberStrategyRange", false))
            {
                this.dataRange.DataRange = strategy_0.DataRange;
                form.DataRange = strategy_0.DataRange;
            }
            bool flag = false;
            if ((useAdvancedSettings && MainModule.Instance.Settings.Get("RememberStrategyData", false)) && (strategy_0.DataSetName != ""))
            {
                WealthLab.DataSource source = MainModule.Instance.DataSources.FindDataSource(strategy_0.DataSetName);
                if (source != null)
                {
                    if (MainModule.Instance.Settings.Get("RememberStrategyScale", false))
                    {
                        form.SetBarDataScaleForDataSource(source, strategy_0.DataScale);
                    }
                    flag = true;
                    if (strategy_0.Symbol != "")
                    {
                        this.treeDataSources.SelectSymbol(source, strategy_0.Symbol);
                        this.method_15(this.treeDataSources, new DataSourceSymbolEventArgs(this.treeDataSources.DataSource, this.treeDataSources.Symbol));
                    }
                    else
                    {
                        this.treeDataSources.SelectDataSource(source);
                        this.method_13(this.treeDataSources, new DataSourceEventArgs(this.treeDataSources.DataSource));
                    }
                }
            }
            if ((!flag && (this.treeDataSources.SelectedNode != null)) && executeOnSelectedSymbol)
            {
                if ((this.treeDataSources.Symbol != null) && (this.treeDataSources.Symbol != ""))
                {
                    this.SelectingNodeForFormCreation = true;
                    this.method_15(this.treeDataSources, new DataSourceSymbolEventArgs(this.treeDataSources.DataSource, this.treeDataSources.Symbol));
                }
                else
                {
                    this.method_13(this.treeDataSources, new DataSourceEventArgs(this.treeDataSources.DataSource));
                }
            }
            this.SetDataPanelState(true, true, true, true, strategy_0.StrategyType == StrategyType.CombinedStrategy);
            MainModule.Instance.AddStrategyToMRU(strategy_0);
            return form;
        }

        public void OrdersUpdated()
        {
            base.Invoke(new OrdersUpdatedCallback(this.OrdersUpdatedThreadSafe));
        }

        public void OrdersUpdatedThreadSafe()
        {
            this.statusOrders.Text = "Orders: " + MainModule.Instance.TradeManager.OrderCount;
            this.statusActive.Text = "Active Orders: " + MainModule.Instance.TradeManager.ActiveOrderCount;
        }

        public void PlotIndicator(IndicatorHelper helper)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.PlotIndicator(helper);
            }
        }

        private void pnlParamBase_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(0xa7, 0xa6, 170));
            Brush brush = new SolidBrush(this.pnlParamBase.BackColor);
            using (brush)
            {
                e.Graphics.FillRectangle(brush, 0, 0, this.pnlParamBase.Width, this.pnlParamBase.Height);
            }
            using (pen)
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.pnlParamBase.Width - 1, this.pnlParamBase.Height - 1);
            }
        }

        public void PrintStatus(string message)
        {
            base.Invoke(new Delegate38(this.method_4), new object[] { message });
        }

        public void SelectNode()
        {
            if ((this.treeDataSources.DataSource != null) && (this.cmbSymbol.Text != string.Empty))
            {
                this.method_15(this, new DataSourceSymbolEventArgs(this.treeDataSources.DataSource, this.cmbSymbol.Text));
            }
            else if (this.treeDataSources.Nodes.Count > 0)
            {
                if (this.treeDataSources.Symbol != "")
                {
                    this.method_15(this, new DataSourceSymbolEventArgs(this.treeDataSources.DataSource, this.treeDataSources.Symbol));
                }
                else
                {
                    this.treeDataSources.Nodes[0].Expand();
                    if (this.treeDataSources.Nodes[0].Nodes.Count > 0)
                    {
                        this.treeDataSources.SelectedNode = this.treeDataSources.FindNode(MainModule.Instance.DataSources.FindDataSource("Dow 30"), "AA");
                        if (this.treeDataSources.SelectedNode == null)
                        {
                            this.treeDataSources.SelectedNode = this.treeDataSources.Nodes[0].Nodes[0];
                        }
                    }
                }
            }
        }

        public void SelectTreeNode(WealthLab.DataSource dataSource_0, string symbol)
        {
            TreeNode node = this.treeDataSources.FindNode(dataSource_0, symbol);
            if (node != null)
            {
                this.treeDataSources.SelectedNode = null;
                this.treeDataSources.ClearLastSelection();
                this.treeDataSources.SelectedNode = node;
            }
        }

        public void SetDataPanelState(bool settings, bool tree, bool positions, bool parameters, bool combinationStrategy)
        {
            Color red = Color.FromKnownColor(KnownColor.ControlText);
            Color color2 = Color.FromKnownColor(KnownColor.GrayText);
            Color color3 = settings ? red : color2;
            this.lblRange.ForeColor = color3;
            this.dataRange.ForeColor = color3;
            this.lblSymbol.ForeColor = color3;
            this.cmbSymbol.ForeColor = color3;
            this.btnGo.ForeColor = color3;
            this.lblScale.ForeColor = combinationStrategy ? color2 : color3;
            this.scale.ForeColor = this.lblScale.ForeColor;
            this.cmbSymbol.Enabled = settings;
            color3 = tree ? red : color2;
            this.lblDataSets.ForeColor = color3;
            this.treeDataSources.ForeColor = color3;
            color3 = positions ? red : color2;
            if (combinationStrategy)
            {
                color3 = color2;
            }
            this.lblPositions.ForeColor = color3;
            this.posSize.ForeColor = color3;
            if ((base.ActiveMdiChild is IWealthScriptProvider) && (base.ActiveMdiChild as IWealthScriptProvider).ParametersNeedSave)
            {
                red = Color.Red;
            }
            color3 = parameters ? red : color2;
            this.lblParameters.ForeColor = color3;
            this.method_35(color3 == Color.Red);
        }

        public void SetLogScaleButtonState(bool logScale)
        {
            this.btnLinear.Checked = !logScale;
            this.btnLog.Checked = logScale;
        }

        public void SetOnDemand(bool onDemandOn)
        {
            this.mniOnDemand.Checked = onDemandOn;
        }

        public void SetStreamingGlobal(bool streaming)
        {
            foreach (Form form in base.MdiChildren)
            {
                if (form is ChartForm)
                {
                    ChartForm form2 = form as ChartForm;
                    form2.IsStreaming = streaming;
                }
            }
            this.EnableSliders();
        }

        public void ShowAutoTradingState(AutoTradingMode mode)
        {
            foreach (Form form in base.MdiChildren)
            {
                if (form is ChartForm)
                {
                    (form as ChartForm).ShowAutoTradingState(mode);
                }
                else if (form is QuotesForm)
                {
                    (form as QuotesForm).ShowAutoTradingState(mode);
                }
                else if (form is StrategyCenterForm)
                {
                    (form as StrategyCenterForm).ShowAutoTradingState(mode);
                }
            }
        }

        public void ShowLoggedInState(bool loggedIn)
        {
            base.Invoke(new Delegate36(this.ShowLoggedInStateTreadSafe), new object[] { loggedIn });
        }

        public void ShowLoggedInStateTreadSafe(bool loggedIn)
        {
            this.btnLogin.Checked = loggedIn;
            if (loggedIn)
            {
                this.btnLogin.Text = MainModule.Instance.AuthProvider.LoggedInPhrase;
                this.mniLogin.Text = MainModule.Instance.AuthProvider.LoggedInPhrase;
            }
            else
            {
                this.btnLogin.Text = MainModule.Instance.AuthProvider.LoginPhrase;
                this.mniLogin.Text = MainModule.Instance.AuthProvider.LoginPhrase;
            }
            if (!loggedIn)
            {
                ChartForm activeChartWindow = this.ActiveChartWindow;
                if ((activeChartWindow != null) && activeChartWindow.IsStreaming)
                {
                    activeChartWindow.IsStreaming = false;
                }
            }
            if (loggedIn && (MainModule.Instance.BrokerProvider != null))
            {
                string text = this.cmbAccount.Text;
                this.cmbAccount.Items.Clear();
                foreach (Account account in MainModule.Instance.BrokerProvider.Accounts)
                {
                    this.cmbAccount.Items.Add(account.AccountNumber);
                }
                this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(text);
                if (this.cmbAccount.SelectedIndex == -1)
                {
                    this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
                }
                if ((this.cmbAccount.SelectedIndex == -1) && (this.cmbAccount.Items.Count > 0))
                {
                    this.cmbAccount.SelectedIndex = 0;
                }
                string str = this.cmbTradeRoute.Text;
                this.cmbTradeRoute.Items.Clear();
                foreach (string str3 in MainModule.Instance.BrokerProvider.Routes)
                {
                    this.cmbTradeRoute.Items.Add(str3);
                }
                this.cmbTradeRoute.SelectedIndex = this.cmbTradeRoute.Items.IndexOf(str);
                if ((this.cmbTradeRoute.SelectedIndex == -1) && (this.cmbTradeRoute.Items.Count > 0))
                {
                    this.cmbTradeRoute.SelectedIndex = 0;
                }
            }
            this.method_41();
            foreach (Form form2 in base.MdiChildren)
            {
                if (form2 is ChartForm)
                {
                    (form2 as ChartForm).ShowLoggedInState(loggedIn);
                }
            }
        }

        public void ShowNavBarIcons()
        {
            foreach (ToolStripItem item in this.toolbarNav.Items)
            {
                if (!(item.Tag is string))
                {
                    break;
                }
                string tag = item.Tag as string;
                if (tag == "P")
                {
                    if (this.mniNavIcons.Checked)
                    {
                        item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        item.Padding = new Padding(0, 0, 0, 0);
                    }
                    else
                    {
                        item.DisplayStyle = ToolStripItemDisplayStyle.Text;
                        item.Padding = new Padding(0, 0, 0x10, 0);
                    }
                }
            }
        }

        private static void smethod_0(bool bool_10)
        {
            if (bool_10)
            {
                int_5 = 1;
            }
            int_6 = new Random().Next(5);
        }

        private void statusActive_Click(object sender, EventArgs e)
        {
            this.OpenOrderManager();
        }

        private void statusMessage_Click(object sender, EventArgs e)
        {
            this.statusMessage.Text = "";
        }

        private void statusStreamingProvider_Click(object sender, EventArgs e)
        {
            StreamingDataProvider streamingProvider = MainModule.Instance.StreamingProvider;
            if ((streamingProvider != null) && !streamingProvider.IsConnected)
            {
                streamingProvider.ConnectStreaming(this);
            }
        }

        private void statusStreamingStatus_Click(object sender, EventArgs e)
        {
            StreamingDataProvider streamingProvider = MainModule.Instance.StreamingProvider;
            if ((streamingProvider != null) && !streamingProvider.IsConnected)
            {
                streamingProvider.ConnectStreaming(this);
            }
        }

        private void statusStreamingSymbolsOff_Click(object sender, EventArgs e)
        {
            foreach (string str in this.list_1)
            {
                this.streamingQuoteManager_0.Unsubscribe(str);
            }
            this.statusStreamingSymbolsOff.Visible = false;
            this.statusStreamingSymbolsOn.Visible = true;
        }

        private void statusStreamingSymbolsOn_Click(object sender, EventArgs e)
        {
            if ((MainModule.Instance.StreamingProvider == null) || MainModule.Instance.StreamingProvider.IsConnected)
            {
                foreach (string str in this.list_1)
                {
                    this.streamingQuoteManager_0.Subscribe(str);
                }
                this.statusStreamingSymbolsOn.Visible = false;
                this.statusStreamingSymbolsOff.Visible = true;
            }
        }

        public void StatusUpdate(ConnStatus status, int StatusCode, string Message)
        {
            base.Invoke(new Delegate41(this.method_32), new object[] { status, StatusCode, Message });
        }

        private void symbolInfoManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SymbolManagerForm.Instance != null)
            {
                SymbolManagerForm.Instance.MdiParent.BringToFront();
                SymbolManagerForm.Instance.BringToFront();
                SymbolManagerForm.Instance.WindowState = FormWindowState.Normal;
            }
            else
            {
                new SymbolManagerForm { MdiParent = this }.Show();
            }
        }

        public void SynchDataRangeControl(ChartForm chartForm_1)
        {
            this.dataRange.DataRange = chartForm_1.DataRange;
            this.dataRange.IsStreaming = chartForm_1.IsStreaming;
            this.dataRange.UpdateText();
        }

        private void timer_0_Tick(object sender, EventArgs e)
        {
            if (this.streamingQuoteManager_0.FreshQuoteReady)
            {
                foreach (ToolStripItem item in this.status.Items)
                {
                    if ((item.Tag != null) && (item is ToolStripStatusLabel))
                    {
                        ToolStripStatusLabel label = item as ToolStripStatusLabel;
                        string tag = (string) item.Tag;
                        Quote lastQuote = this.streamingQuoteManager_0.GetLastQuote(tag);
                        if (lastQuote != null)
                        {
                            if (lastQuote.PreviousClose != 0.0)
                            {
                                double num3 = lastQuote.Price - lastQuote.PreviousClose;
                                string str2 = num3.ToString("N2");
                                if (num3 >= 0.0)
                                {
                                    str2 = "+" + str2;
                                }
                                if (lastQuote.Price > lastQuote.PreviousClose)
                                {
                                    label.ForeColor = Color.Green;
                                }
                                else if (lastQuote.Price < lastQuote.PreviousClose)
                                {
                                    label.ForeColor = Color.Red;
                                }
                                label.Text = tag + " " + lastQuote.Price.ToString("N2") + " (" + str2 + ")";
                            }
                            else
                            {
                                label.Text = tag + " " + lastQuote.Price.ToString("N2");
                            }
                        }
                    }
                }
            }
        }

        private void timer_1_Tick(object sender, EventArgs e)
        {
            this.timer_1.Enabled = false;
            this.bool_5 = true;
            try
            {
                this.treeDataSources.SelectedNode = this.treeDataSources.FindNode(this.chartForm_0.DataSource, this.chartForm_0.Symbol);
            }
            finally
            {
                this.bool_5 = false;
            }
        }

        private void timer_2_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            if (int_5 > 0)
            {
                switch (int_5)
                {
                    case 1:
                        int_5 = random.Next(2, 5);
                        MainModule.Instance.Executor.TNP = _tamperCode;
                        if (MainModule.Instance.AuthProvider is INotifier)
                        {
                            INotifier authProvider = MainModule.Instance.AuthProvider as INotifier;
                            authProvider.TNP = _tamperCode;
                        }
                        goto Label_01A2;

                    case 2:
                        this.method_44(random.Next(100, 0x3e8));
                        goto Label_01A2;

                    case 3:
                        if (--int_6 < 0)
                        {
                            Application.Exit();
                        }
                        goto Label_01A2;
                }
                this.Disconnect();
                MainModule.Instance.AutoTradingEnabled = AutoTradingMode.Off;
                int_5 = random.Next(2, 5);
            }
            else
            {
                int num2 = 0;
                if (MainModule.Instance.Executor.TNP < _secureCodeMin)
                {
                    num2++;
                }
                if (MainModule.Instance.AuthProvider is INotifier)
                {
                    INotifier notifier3 = MainModule.Instance.AuthProvider as INotifier;
                    if (notifier3.TNP < _secureCodeMin)
                    {
                        num2 += 2;
                    }
                }
                if (num2 > 0)
                {
                    int_5 = random.Next(2, 5);
                    if (num2 == 2)
                    {
                        MainModule.Instance.Executor.TNP = _tamperCode;
                    }
                    else if ((num2 == 1) && (MainModule.Instance.AuthProvider is INotifier))
                    {
                        INotifier notifier = MainModule.Instance.AuthProvider as INotifier;
                        notifier.TNP = _tamperCode;
                    }
                }
                else if (--int_6 <= 0)
                {
                    this.timer_2.Stop();
                }
            }
        Label_01A2:
            this.timer_2.Interval = random.Next(0x493e0);
        }

        private void treeDataSources_DoubleClick(object sender, EventArgs e)
        {
            Cursor cursor = this.treeDataSources.Cursor;
            this.treeDataSources.Cursor = Cursors.WaitCursor;
            try
            {
                TreeNode selectedNode = this.treeDataSources.SelectedNode;
                if ((selectedNode != null) && (selectedNode.Level != 0))
                {
                    if (this.ActiveChartWindow == null)
                    {
                        this.CreateChartWindow(false);
                        this.treeDataSources.SelectSymbol(this.DataSource, selectedNode.Text);
                        this.method_15(this, new DataSourceSymbolEventArgs(this.DataSource, selectedNode.Text));
                    }
                    else
                    {
                        this.ActiveChartWindow.SelectTab("Chart");
                    }
                }
            }
            finally
            {
                this.treeDataSources.Cursor = cursor;
            }
        }

        private void treeDataSources_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void treeDataSources_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode nodeAt = this.treeDataSources.GetNodeAt(this.int_3, this.int_4);
            if (nodeAt != null)
            {
                this.treeDataSources.DoDragDrop(nodeAt, DragDropEffects.Copy);
            }
        }

        private void treeDataSources_MouseDown(object sender, MouseEventArgs e)
        {
            this.int_3 = e.X;
            this.int_4 = e.Y;
        }

        private void txtTradeSymbol_Leave(object sender, EventArgs e)
        {
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            if (MainModule.Instance.BrokerProvider != null)
            {
                if (this.txtTradeSymbol.Text.Length > 0)
                {
                    Quote quote = null;
                    try
                    {
                        quote = MainModule.Instance.BrokerProvider.GetQuote(this.txtTradeSymbol.Text);
                    }
                    catch
                    {
                    }
                    if (quote != null)
                    {
                        this.lblLast.Text = "Last: $" + quote.Price.ToString();
                        this.lblBid.Text = "Bid: $" + quote.Bid.ToString();
                        this.lblAsk.Text = "Ask: $" + quote.Ask.ToString();
                        this.lblAsOf.Text = "As of " + quote.TimeStamp.ToShortDateString() + " " + quote.TimeStamp.ToLongTimeString();
                    }
                    else
                    {
                        this.lblLast.Text = "Last: ";
                        this.lblBid.Text = "Bid: ";
                        this.lblAsk.Text = "Ask: ";
                        this.lblAsOf.Text = "As of ";
                    }
                }
                else
                {
                    this.lblLast.Text = "Last: ";
                    this.lblBid.Text = "Bid: ";
                    this.lblAsk.Text = "Ask: ";
                    this.lblAsOf.Text = "As of ";
                }
            }
            this.Cursor = cursor;
        }

        public void UpdateChartColorsAndStyle()
        {
            foreach (Form form in base.MdiChildren)
            {
                if (form is ChartForm)
                {
                    (form as ChartForm).UpdateChartColorsAndStyle(true);
                }
            }
        }

        public void UpdateStreamingSymbols(IList<string> symbols)
        {
            if (this.bool_4)
            {
                foreach (string str2 in this.list_1)
                {
                    if (!symbols.Contains(str2))
                    {
                        this.streamingQuoteManager_0.Unsubscribe(str2);
                    }
                }
                for (int i = this.status.Items.Count - 1; i >= 0; i--)
                {
                    if (this.status.Items[i].Tag != null)
                    {
                        this.status.Items.Remove(this.status.Items[i]);
                    }
                }
                foreach (string str in symbols)
                {
                    ToolStripStatusLabel label = new ToolStripStatusLabel(str) {
                        BorderSides = ToolStripStatusLabelBorderSides.Right
                    };
                    this.status.Items.Add(label);
                    label.Tag = str;
                }
                this.streamingQuoteManager_0.ConnectionStatus = this;
                this.streamingQuoteManager_0.Provider = MainModule.Instance.StreamingProvider;
                foreach (string str3 in symbols)
                {
                    if (!this.list_1.Contains(str3))
                    {
                        this.streamingQuoteManager_0.Subscribe(str3);
                    }
                }
            }
            this.list_1.Clear();
            foreach (string str4 in symbols)
            {
                this.list_1.Add(str4);
            }
        }

        public void UpdateTradeTicketQuote(Quote quote)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate42(this.UpdateTradeTicketQuoteCallbackThreadSafe), new object[] { quote });
            }
            else
            {
                this.UpdateTradeTicketQuoteCallbackThreadSafe(quote);
            }
        }

        public void UpdateTradeTicketQuoteCallbackThreadSafe(Quote quote)
        {
            if (this.txtTradeSymbol.Text == quote.Symbol)
            {
                this.lblLast.Text = "Last: $" + quote.Price.ToString();
                this.lblBid.Text = "Bid: $" + quote.Bid.ToString();
                this.lblAsk.Text = "Ask: $" + quote.Ask.ToString();
                this.lblAsOf.Text = "As of " + quote.TimeStamp.ToShortDateString() + " " + quote.TimeStamp.ToLongTimeString();
            }
        }

        private static double _secureCode
        {
            get
            {
                return DateTime.Now.Add(new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)).ToOADate();
            }
        }

        private static double _secureCodeMin
        {
            get
            {
                return DateTime.FromOADate(_secureCode).Subtract(new TimeSpan(0, 0, 0, 1)).ToOADate();
            }
        }

        private static double _tamperCode
        {
            get
            {
                return DateTime.Now.ToOADate();
            }
        }

        public ChartForm ActiveChartWindow
        {
            get
            {
                if (base.ActiveMdiChild == null)
                {
                    return null;
                }
                return (base.ActiveMdiChild as ChartForm);
            }
        }

        public WealthLab.BarDataScale BarDataScale
        {
            get
            {
                return this.scale.DataScale;
            }
            set
            {
                this.scale.DataScale = value;
            }
        }

        public WealthLab.DataSource DataSource
        {
            get
            {
                TreeNode selectedNode = this.treeDataSources.SelectedNode;
                if (selectedNode == null)
                {
                    return null;
                }
                return (WealthLab.DataSource) selectedNode.Tag;
            }
        }

        public PageSettings DefaultPageSettings
        {
            get
            {
                return this.pageSettings_0;
            }
            set
            {
                this.pageSettings_0 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool IsFirstMainForm
        {
            get
            {
                return this.bool_3;
            }
        }

        public static MainForm LastActivated
        {
            get
            {
                return mainForm_0;
            }
        }

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return this.posSize.PositionSize;
            }
            set
            {
                this.posSize.PositionSize = value;
            }
        }

        public bool SelectingNodeForFormCreation
        {
            [CompilerGenerated]
            get
            {
                return this.bool_9;
            }
            [CompilerGenerated]
            set
            {
                this.bool_9 = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.cmbSymbol.Text;
            }
            internal set
            {
                this.cmbSymbol.Text = value;
                this.btnGo_Click(this, EventArgs.Empty);
            }
        }

        internal static int TemperFi
        {
            get
            {
                return int_5;
            }
        }

        public double TNP
        {
            get
            {
                if (int_5 > 0)
                {
                    return _tamperCode;
                }
                return _secureCode;
            }
            set
            {
                if ((value < _secureCodeMin) && (int_5 == 0))
                {
                    smethod_0(true);
                }
            }
        }

        private delegate void Delegate34();

        private delegate void Delegate35();

        private delegate void Delegate36(bool bool_0);

        private delegate void Delegate37(string string_0);

        private delegate void Delegate38(string string_0);

        private delegate void Delegate39(bool bool_0);

        private delegate void Delegate40();

        private delegate void Delegate41(ConnStatus connStatus_0, int int_0, string string_0);

        private delegate void Delegate42(Quote quote_0);

        public delegate void OrdersUpdatedCallback();
    }
}

