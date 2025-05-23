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
        private static bool menuTriggeredExit = false;   ///WYJ fix, original name: bool_1
        private bool isFirstMainForm_2;   ///WYJ fix, original name: bool_2
        private bool isFirstMainForm;
        private bool bool_4;
        private bool dataSourceTreeSelectedNodeLocked;   ///WYJ fix, original signature: bool_5
        private bool dragDropFundamentalsExist;   ///WYJ fix, original name: bool_6
        private bool formClosing;   ///WYJ fix, original signature: bool_7
        private bool disconnected;   ///WYJ fix, original signature: bool_8
        [CompilerGenerated]
        private bool selectingNodeForFormCreation;   ///WYJ fix, original name: bool_9
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
        private IContainer components;
        private static int mainFormCount = 0;   ///WYJ fix, original name: int_0
        private int chartFormCount;   ///WYJ fix, original name: int_1
        private int quotesFormCount;   ///WYJ fix, original name: int_2
        private int mouseX;   ///WYJ fix, original signature: int_3
        private int mouseY;   ///WYJ fix, original signature: int_4
        private static int int_5 = 0;
        private static int randomDelay = 0;   ///WYJ fix, original name: int_6
        private Label lblAcctType;
        private Label lblAsk;
        private Label lblAsOf;
        private Label lblBid;
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
        private static List<MainForm> list_0 = new List<MainForm>();
        private List<string> streamingSymbols = new List<string>();   ///WYJ fix, original name: list_1
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

        private ToolStripMenuItem mniCrossHair;

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
        private string workspaceDir;   ///WYJ fix, original name: string_0
        private ToolStripMenuItem symbolInfoManagerToolStripMenuItem;
        private System.Windows.Forms.Timer timer_0;
        private System.Windows.Forms.Timer timer_1;
        private System.Windows.Forms.Timer timer_2;
        private ToolStrip toolbar;
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
        private ToolStripLabel tslblChartStyles;
        private ToolStripLabel tslblOptions;
        private ToolStripDropDownButton tsmMoreChartStyles;
        private DataSourceTreeView treeDataSources;
        private ToolStrip toolbarDataSets;
        private ToolStripLabel lblDataSets;
        private ToolStripLabel linkNewDataSet;
        private Panel pnlParamBaseLinkParams;
        private LinkLabel linkRerun;
        private LinkLabel linkResetParams;
        private LinkLabel linkSaveParams;
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
            item.Click += new EventHandler(this.workspaceMenuItems_Click);
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
                activeChartWindow.clearDragDropIndicators();
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
            if (sender == null || (sender as ToolStripItem).Name.Contains("CrossHair"))
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
            if (this.btnFundamental.Visible && !this.dragDropFundamentalsExist)
            {
                this.btnFundamental.Visible = this.dragDropFundamentalsExist;
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
            if (this.btnFundamentalsTB2.Visible && !this.dragDropFundamentalsExist)
            {
                this.btnFundamentalsTB2.Visible = this.dragDropFundamentalsExist;
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
                this.informChartFormsForSymbolChange();
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
                                                goto  Label_ShowHelp;

                                            case 1:
                                                keyword = "editor.htm";
                                                goto  Label_ShowHelp;

                                            case 2:
                                                keyword = "strategysummary.htm";
                                                goto  Label_ShowHelp;

                                            case 3:
                                                keyword = "rules_view.htm";
                                                goto  Label_ShowHelp;

                                            case 4:
                                                keyword = "performance.htm";
                                                goto  Label_ShowHelp;

                                            case 5:
                                                keyword = "bysymbol.htm";
                                                goto  Label_ShowHelp;

                                            case 6:
                                                keyword = "trades.htm";
                                                goto  Label_ShowHelp;

                                            case 7:
                                                keyword = "equitycurve.htm";
                                                goto  Label_ShowHelp;

                                            case 8:
                                                keyword = "drawdown.htm";
                                                goto  Label_ShowHelp;

                                            case 9:
                                                keyword = "profitdistribution.htm";
                                                goto  Label_ShowHelp;

                                            case 10:
                                                keyword = "byperiod.htm";
                                                goto  Label_ShowHelp;

                                            case 11:
                                                keyword = "maemfe.htm";
                                                goto  Label_ShowHelp;
                                        }
                                    }
                                }
                                if (activeMdiChild.CurrentTabName.Contains("Alert"))
                                {
                                    keyword = "alertview.htm";
                                }
                                keyword = "strategywindow.htm";
                                goto  Label_ShowHelp;
                            }
                            case 1:
                                keyword = "home_page.htm";
                                goto  Label_ShowHelp;

                            case 2:
                                keyword = "datamanager.htm";
                                goto  Label_ShowHelp;

                            case 3:
                                keyword = "strategymonitor.htm";
                                goto  Label_ShowHelp;

                            case 4:
                                keyword = "quotes.htm";
                                goto  Label_ShowHelp;

                            case 5:
                                keyword = "accountbalancespos.htm";
                                goto  Label_ShowHelp;

                            case 6:
                                keyword = "orders.htm";
                                goto  Label_ShowHelp;

                            case 7:
                                keyword = "symbol_info_manager.htm";
                                goto  Label_ShowHelp;

                            case 8:
                                keyword = "INDEXLAB.htm";
                                goto  Label_ShowHelp;
                        }
                    }
                }
                keyword = "introduction.htm";
            }
        Label_ShowHelp:
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

        ///WYJ fix, original signature: private void btnLineChart_Click(object sender, EventArgs e)
        private void chartStyleButtons_Click(object sender, EventArgs e)
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
                ChartStyle style = this.getSelectedChartStyle();
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
            Alert alert = this.createAlert();
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
                activeChartWindow.pushCode();
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
            Alert alert = this.createAlert();
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
                        this.enableParamLinkButtons(true);
                    }
                    else
                    {
                        this.lblParameters.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                        this.enableParamLinkButtons(false);
                    }
                }
                else
                {
                    this.lblParameters.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                    this.enableParamLinkButtons(false);
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
            ///this.method_17();   ///WYJ fix, inline method_17
            if (this.treeDataSources.SelectedNode != null)
            {
                bool flag = (this.treeDataSources.SelectedNode.Level == 0) && (this.cmbSymbol.Text == string.Empty);
                this.chartFormShowMultiSymbol(flag);
            }
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
            this.setCursorForChartForms(Cursors.Default);
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
            base.Invoke(new Delegate39(this.doConnect), new object[] { false });
        }

        public void Connect(bool reconnect)
        {
            base.Invoke(new Delegate39(this.doConnect), new object[] { reconnect });
            this.disconnected = false;
        }

        public ChartForm CreateChartWindow(bool selectNode)
        {
            ChartForm form = new ChartForm {
                MdiParent = this
            };
            string str = MainModule.Instance.Settings.Get("ChartStyleSelected", "");
            if (str != "")
            {
                form.ChartStyle = this.getChartStyleByName(str);
            }
            else
            {
                form.ChartStyle = this.getSelectedChartStyle();
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
            //form.Size = new System.Drawing.Size(new Point(300, 300)); ///WYJ fix, specify the size of chart window - Not here, but in the loadWorkspace method
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
            if (!this.formClosing)
            {
                base.Invoke(new Delegate40(this.doDisconnect));
            }
            this.disconnected = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
                this.editMenuDefaultEnablement();
            }
            else if (base.ActiveMdiChild is OrdersAlertsForm)
            {
                this.mniPrint.Enabled = true;
                this.editMenuDefaultEnablement();
            }
            else if (base.ActiveMdiChild is StrategyCenterForm)
            {
                this.mniPrint.Enabled = true;
                this.editMenuDefaultEnablement();
            }
            else if (base.ActiveMdiChild is QuotesForm)
            {
                this.mniPrint.Enabled = true;
                this.editMenuDefaultEnablement();
            }
            else if (base.ActiveMdiChild is StrategyRanking)
            {
                this.mniPrint.Enabled = true;
                this.editMenuDefaultEnablement();
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
                    this.editMenuDefaultEnablement();
                }
                else if (activeChartWindow.CurrentTabName.Contains("Strategy Summary"))
                {
                    this.mniPrint.Enabled = true;
                    this.editMenuDefaultEnablement();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblAsk = new System.Windows.Forms.Label();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewChart = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewBuilder = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewMultiStrategyBuilder = new System.Windows.Forms.ToolStripMenuItem();
            this.sepNewStrategy = new System.Windows.Forms.ToolStripSeparator();
            this.mniNewWorkspace2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewQuote = new System.Windows.Forms.ToolStripMenuItem();
            this.sepNewQuote = new System.Windows.Forms.ToolStripSeparator();
            this.mniNewDataSet = new System.Windows.Forms.ToolStripMenuItem();
            this.mniOpenStrategy = new System.Windows.Forms.ToolStripMenuItem();
            this.mniOpenWorkspace = new System.Windows.Forms.ToolStripMenuItem();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.sepSave = new System.Windows.Forms.ToolStripSeparator();
            this.mniSaveStrategy = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSaveStrategyAs = new System.Windows.Forms.ToolStripMenuItem();
            this.sepChart = new System.Windows.Forms.ToolStripSeparator();
            this.mniPreferences2 = new System.Windows.Forms.ToolStripMenuItem();
            this.sepPreferences = new System.Windows.Forms.ToolStripSeparator();
            this.mniLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.mniOnDemand = new System.Windows.Forms.ToolStripMenuItem();
            this.sepExit = new System.Windows.Forms.ToolStripSeparator();
            this.mniCloseWorkspace = new System.Windows.Forms.ToolStripMenuItem();
            this.mniExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniUndoDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mniCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mniDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFindReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.sepSelectAll = new System.Windows.Forms.ToolStripSeparator();
            this.mniSetTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.mniView = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewTradeTicket = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewDataPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.sepView = new System.Windows.Forms.ToolStripSeparator();
            this.mniViewNavBar = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNavIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewDrawingBar = new System.Windows.Forms.ToolStripMenuItem();
            this.mniDataWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHomePage = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniOrderManager = new System.Windows.Forms.ToolStripMenuItem();
            this.mniStrategyCenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mniDataManager = new System.Windows.Forms.ToolStripMenuItem();
            this.symbolInfoManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniStrategyRanking = new System.Windows.Forms.ToolStripMenuItem();
            this.mniIndexManager = new System.Windows.Forms.ToolStripMenuItem();
            this.sepTools = new System.Windows.Forms.ToolStripSeparator();
            this.mniIndicators = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFundamentals = new System.Windows.Forms.ToolStripMenuItem();
            this.sepIndicators = new System.Windows.Forms.ToolStripSeparator();
            this.mniDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.mniQuickRef = new System.Windows.Forms.ToolStripMenuItem();
            this.sepQuickRef = new System.Windows.Forms.ToolStripSeparator();
            this.mniPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.executeStrategyHiddenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewWorkspace3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniLoadWorkSpace = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSaveWorkSpace = new System.Windows.Forms.ToolStripMenuItem();
            this.sepSaveWorkspace = new System.Windows.Forms.ToolStripSeparator();
            this.mniSetDefaultWorkspace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mniWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mniCascade = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTileHorizontally = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTileVertically = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniUserGuide = new System.Windows.Forms.ToolStripMenuItem();
            this.mniQuickRef2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLanguageGuide = new System.Windows.Forms.ToolStripMenuItem();
            this.sepHelp = new System.Windows.Forms.ToolStripSeparator();
            this.mniFidelityCom = new System.Windows.Forms.ToolStripMenuItem();
            this.mniWealthLabCom = new System.Windows.Forms.ToolStripMenuItem();
            this.sepHelp2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniSoftwareUpgrade = new System.Windows.Forms.ToolStripMenuItem();
            this.sepUpgrade = new System.Windows.Forms.ToolStripSeparator();
            this.mniAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.status = new System.Windows.Forms.StatusStrip();
            this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusOrders = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusActive = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStreamingProvider = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStreamingStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStreamingSymbolsOff = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStreamingSymbolsOn = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusSofwareDownload = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusDownloadProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.stlblHolder = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolbarNav = new System.Windows.Forms.ToolStrip();
            this.btnHome = new System.Windows.Forms.ToolStripButton();
            this.dropdownCharts = new System.Windows.Forms.ToolStripDropDownButton();
            this.mniNewChart2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewRules2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewCode2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewMultiStrategyBuilder2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniOpenStrategy2 = new System.Windows.Forms.ToolStripMenuItem();
            this.sepChart2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniChartFront = new System.Windows.Forms.ToolStripMenuItem();
            this.sepChartFront = new System.Windows.Forms.ToolStripSeparator();
            this.btnStrategyCenter = new System.Windows.Forms.ToolStripButton();
            this.dropdownQuotes = new System.Windows.Forms.ToolStripDropDownButton();
            this.mniNewQuote2 = new System.Windows.Forms.ToolStripMenuItem();
            this.sepQuote2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniQuoteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.sepQuoteFront = new System.Windows.Forms.ToolStripSeparator();
            this.btnOrdersAlerts = new System.Windows.Forms.ToolStripButton();
            this.btnAcctsPositions = new System.Windows.Forms.ToolStripButton();
            this.btnDataManager = new System.Windows.Forms.ToolStripButton();
            this.sepDataManager = new System.Windows.Forms.ToolStripSeparator();
            this.btnIndicators = new System.Windows.Forms.ToolStripButton();
            this.btnFundamental = new System.Windows.Forms.ToolStripButton();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnPreferences = new System.Windows.Forms.ToolStripButton();
            this.btnTrade = new System.Windows.Forms.ToolStripButton();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.mniNewChartDD = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewStrategyRulesDD = new System.Windows.Forms.ToolStripMenuItem();
            this.newStrategyFromCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewCombinationStrategy = new System.Windows.Forms.ToolStripMenuItem();
            this.sepNew = new System.Windows.Forms.ToolStripSeparator();
            this.mniNewWorkspaceTB = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNewQuoteDD = new System.Windows.Forms.ToolStripMenuItem();
            this.sepNew2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniNewDataSetTB = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenStrategy = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
            this.sepChartFile = new System.Windows.Forms.ToolStripSeparator();
            this.btnLogin = new System.Windows.Forms.ToolStripButton();
            this.sepSpacing = new System.Windows.Forms.ToolStripSeparator();
            this.lblSpacing = new System.Windows.Forms.ToolStripLabel();
            this.btnIncreaseSpacing = new System.Windows.Forms.ToolStripButton();
            this.btnRestoreSpacing = new System.Windows.Forms.ToolStripButton();
            this.btnDecreaseSpacing = new System.Windows.Forms.ToolStripButton();
            this.sepBarSpacing = new System.Windows.Forms.ToolStripSeparator();
            this.tslblChartStyles = new System.Windows.Forms.ToolStripLabel();
            this.btnCandleStyle = new System.Windows.Forms.ToolStripButton();
            this.btnBarChart = new System.Windows.Forms.ToolStripButton();
            this.btnLineChart = new System.Windows.Forms.ToolStripButton();
            this.tsmMoreChartStyles = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnLinear = new System.Windows.Forms.ToolStripButton();
            this.btnLog = new System.Windows.Forms.ToolStripButton();
            this.sepChartStyles = new System.Windows.Forms.ToolStripSeparator();
            this.tslblOptions = new System.Windows.Forms.ToolStripLabel();
            this.btnLabelsVisible = new System.Windows.Forms.ToolStripButton();
            this.btnStatusBarVisible = new System.Windows.Forms.ToolStripButton();
            this.btnFundamentalsVisible = new System.Windows.Forms.ToolStripButton();
            this.btnDataWindow = new System.Windows.Forms.ToolStripButton();
            this.btnIndicatorsTB2 = new System.Windows.Forms.ToolStripButton();
            this.btnFundamentalsTB2 = new System.Windows.Forms.ToolStripButton();
            this.btnClearIndicators = new System.Windows.Forms.ToolStripButton();
            this.btnPushCode = new System.Windows.Forms.ToolStripButton();
            this.btnTradeTicket = new System.Windows.Forms.ToolStripButton();
            this.btnPreferencesTB = new System.Windows.Forms.ToolStripButton();
            this.pnlTree = new System.Windows.Forms.Panel();
            this.splitContainerDataPane = new System.Windows.Forms.SplitContainer();
            this.treeDataSources = new WealthLab.DataSourceTreeView();
            this.toolbarDataSets = new System.Windows.Forms.ToolStrip();
            this.lblDataSets = new System.Windows.Forms.ToolStripLabel();
            this.linkNewDataSet = new System.Windows.Forms.ToolStripLabel();
            this.toolbarParameters = new System.Windows.Forms.ToolStrip();
            this.lblParameters = new System.Windows.Forms.ToolStripLabel();
            this.pnlParamBase = new System.Windows.Forms.Panel();
            this.pnlParamBaseLinkParams = new System.Windows.Forms.Panel();
            this.linkRerun = new System.Windows.Forms.LinkLabel();
            this.linkResetParams = new System.Windows.Forms.LinkLabel();
            this.linkSaveParams = new System.Windows.Forms.LinkLabel();
            this.paramSliders = new WealthLab.ParameterSlidersContainer();
            this.popupPreferredValues = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mniStorePreferredValues = new System.Windows.Forms.ToolStripMenuItem();
            this.lblScale = new System.Windows.Forms.Label();
            this.scale = new WealthLabPro.ScaleSelecter();
            this.cmbSymbol = new System.Windows.Forms.ComboBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.lblSymbol = new System.Windows.Forms.Label();
            this.lblPositions = new System.Windows.Forms.Label();
            this.lblRange = new System.Windows.Forms.Label();
            this.posSize = new WealthLabPro.PositionSizeSelecter();
            this.dataRange = new WealthLabPro.BarDataRangeSelecter();
            this.toolbarDrawing = new System.Windows.Forms.ToolStrip();
            this.btnClearDrawingObjects = new System.Windows.Forms.ToolStripButton();
            this.btnCrossHair = new System.Windows.Forms.ToolStripButton();
            this.sepDeleteDrawing = new System.Windows.Forms.ToolStripSeparator();
            this.splitter = new System.Windows.Forms.Splitter();
            this.saveFileDialog_0 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog_0 = new System.Windows.Forms.OpenFileDialog();
            this.timer_0 = new System.Windows.Forms.Timer(this.components);
            this.timer_1 = new System.Windows.Forms.Timer(this.components);
            this.pnlTrade = new System.Windows.Forms.Panel();
            this.accountTypeSelector1 = new WealthLabPro.AccountTypeSelector(this.components);
            this.lblBid = new System.Windows.Forms.Label();
            this.lblAsOf = new System.Windows.Forms.Label();
            this.lblLast = new System.Windows.Forms.Label();
            this.lblAcctType = new System.Windows.Forms.Label();
            this.btnDockUp = new System.Windows.Forms.Button();
            this.btnCloseTradeTicket = new System.Windows.Forms.Button();
            this.btnDockDown = new System.Windows.Forms.Button();
            this.btnLoginTradeTicket = new System.Windows.Forms.Button();
            this.numTradePrice = new CtrlLib.NumEdit();
            this.numTradeQty = new CtrlLib.NumEdit();
            this.txtTradeSymbol = new System.Windows.Forms.TextBox();
            this.btnStageOrder = new System.Windows.Forms.Button();
            this.btnPlaceOrder = new System.Windows.Forms.Button();
            this.lbTradeDirected = new System.Windows.Forms.Label();
            this.cmbTradeRoute = new System.Windows.Forms.ComboBox();
            this.lblTradeTIF = new System.Windows.Forms.Label();
            this.cmbTradeTIF = new System.Windows.Forms.ComboBox();
            this.lblTradePrice = new System.Windows.Forms.Label();
            this.lblTradeOrder = new System.Windows.Forms.Label();
            this.cmbTradeOrder = new System.Windows.Forms.ComboBox();
            this.lblTradeQty = new System.Windows.Forms.Label();
            this.lblTradeAction = new System.Windows.Forms.Label();
            this.cmbTradeAction = new System.Windows.Forms.ComboBox();
            this.lblTradeSymbol = new System.Windows.Forms.Label();
            this.lblTradeAcct = new System.Windows.Forms.Label();
            this.cmbAccount = new System.Windows.Forms.ComboBox();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.pageSetupDialog_0 = new System.Windows.Forms.PageSetupDialog();
            this.timer_2 = new System.Windows.Forms.Timer(this.components);
            this.assemblyLoader_0 = new Fidelity.Components.AssemblyLoader(this.components);
            this.drawingObjectManager_0 = new WealthLab.ChartControl.DrawingObjectManager(this.components);
            this.streamingQuoteManager_0 = new WealthLab.StreamingQuoteManager(this.components);
            this.menuMain.SuspendLayout();
            this.status.SuspendLayout();
            this.toolbarNav.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.pnlTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDataPane)).BeginInit();
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
            this.SuspendLayout();
            // 
            // lblAsk
            // 
            this.lblAsk.Location = new System.Drawing.Point(664, 25);
            this.lblAsk.Name = "lblAsk";
            this.lblAsk.Size = new System.Drawing.Size(117, 11);
            this.lblAsk.TabIndex = 42;
            this.lblAsk.Text = "Ask:";
            this.lblAsk.Visible = false;
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
            this.mniEdit,
            this.mniView,
            this.mniTools,
            this.mniWorkspaces,
            this.mniWindow,
            this.mniHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.MdiWindowListItem = this.mniWindow;
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1028, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // mniFile
            // 
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniNew,
            this.mniOpenStrategy,
            this.mniOpenWorkspace,
            this.mniClose,
            this.mniPrint,
            this.sepSave,
            this.mniSaveStrategy,
            this.mniSaveStrategyAs,
            this.sepChart,
            this.mniPreferences2,
            this.sepPreferences,
            this.mniLogin,
            this.mniOnDemand,
            this.sepExit,
            this.mniCloseWorkspace,
            this.mniExit});
            this.mniFile.Name = "mniFile";
            this.mniFile.Size = new System.Drawing.Size(35, 20);
            this.mniFile.Text = "&File";
            // 
            // mniNew
            // 
            this.mniNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniNewChart,
            this.mniNewBuilder,
            this.mniNewEditor,
            this.mniNewMultiStrategyBuilder,
            this.sepNewStrategy,
            this.mniNewWorkspace2,
            this.mniNewQuote,
            this.sepNewQuote,
            this.mniNewDataSet});
            this.mniNew.Image = ((System.Drawing.Image)(resources.GetObject("mniNew.Image")));
            this.mniNew.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNew.Name = "mniNew";
            this.mniNew.Size = new System.Drawing.Size(250, 22);
            this.mniNew.Text = "&New";
            // 
            // mniNewChart
            // 
            this.mniNewChart.Image = ((System.Drawing.Image)(resources.GetObject("mniNewChart.Image")));
            this.mniNewChart.Name = "mniNewChart";
            this.mniNewChart.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.C)));
            this.mniNewChart.Size = new System.Drawing.Size(289, 22);
            this.mniNewChart.Text = "New &Chart Window";
            this.mniNewChart.Click += new System.EventHandler(this.mniNewChartDD_Click);
            // 
            // mniNewBuilder
            // 
            this.mniNewBuilder.Image = ((System.Drawing.Image)(resources.GetObject("mniNewBuilder.Image")));
            this.mniNewBuilder.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewBuilder.Name = "mniNewBuilder";
            this.mniNewBuilder.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.R)));
            this.mniNewBuilder.Size = new System.Drawing.Size(289, 22);
            this.mniNewBuilder.Text = "New Strategy from &Rules";
            this.mniNewBuilder.Click += new System.EventHandler(this.mniNewBuilder_Click);
            // 
            // mniNewEditor
            // 
            this.mniNewEditor.Image = ((System.Drawing.Image)(resources.GetObject("mniNewEditor.Image")));
            this.mniNewEditor.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewEditor.Name = "mniNewEditor";
            this.mniNewEditor.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.mniNewEditor.Size = new System.Drawing.Size(289, 22);
            this.mniNewEditor.Text = "New &Strategy from Code";
            this.mniNewEditor.Click += new System.EventHandler(this.newStrategyFromCodeToolStripMenuItem_Click);
            // 
            // mniNewMultiStrategyBuilder
            // 
            this.mniNewMultiStrategyBuilder.Image = ((System.Drawing.Image)(resources.GetObject("mniNewMultiStrategyBuilder.Image")));
            this.mniNewMultiStrategyBuilder.Name = "mniNewMultiStrategyBuilder";
            this.mniNewMultiStrategyBuilder.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.M)));
            this.mniNewMultiStrategyBuilder.Size = new System.Drawing.Size(289, 22);
            this.mniNewMultiStrategyBuilder.Text = "New Combination Strategy";
            this.mniNewMultiStrategyBuilder.Click += new System.EventHandler(this.mniNewCombinationStrategy_Click);
            // 
            // sepNewStrategy
            // 
            this.sepNewStrategy.Name = "sepNewStrategy";
            this.sepNewStrategy.Size = new System.Drawing.Size(286, 6);
            // 
            // mniNewWorkspace2
            // 
            this.mniNewWorkspace2.Name = "mniNewWorkspace2";
            this.mniNewWorkspace2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.W)));
            this.mniNewWorkspace2.Size = new System.Drawing.Size(289, 22);
            this.mniNewWorkspace2.Text = "New Main &Workspace Window";
            this.mniNewWorkspace2.Click += new System.EventHandler(this.mniNewWorkspaceTB_Click);
            // 
            // mniNewQuote
            // 
            this.mniNewQuote.Image = ((System.Drawing.Image)(resources.GetObject("mniNewQuote.Image")));
            this.mniNewQuote.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewQuote.Name = "mniNewQuote";
            this.mniNewQuote.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.Q)));
            this.mniNewQuote.Size = new System.Drawing.Size(289, 22);
            this.mniNewQuote.Text = "New &Quote Window";
            this.mniNewQuote.Click += new System.EventHandler(this.mniNewQuoteDD_Click);
            // 
            // sepNewQuote
            // 
            this.sepNewQuote.Name = "sepNewQuote";
            this.sepNewQuote.Size = new System.Drawing.Size(286, 6);
            // 
            // mniNewDataSet
            // 
            this.mniNewDataSet.Image = ((System.Drawing.Image)(resources.GetObject("mniNewDataSet.Image")));
            this.mniNewDataSet.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewDataSet.Name = "mniNewDataSet";
            this.mniNewDataSet.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.D)));
            this.mniNewDataSet.Size = new System.Drawing.Size(289, 22);
            this.mniNewDataSet.Text = "New &DataSet ...";
            this.mniNewDataSet.Click += new System.EventHandler(this.linkNewDataSet_Click);
            // 
            // mniOpenStrategy
            // 
            this.mniOpenStrategy.Image = ((System.Drawing.Image)(resources.GetObject("mniOpenStrategy.Image")));
            this.mniOpenStrategy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniOpenStrategy.Name = "mniOpenStrategy";
            this.mniOpenStrategy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mniOpenStrategy.Size = new System.Drawing.Size(250, 22);
            this.mniOpenStrategy.Text = "Open &Strategy ...";
            this.mniOpenStrategy.Click += new System.EventHandler(this.btnOpenStrategy_Click);
            // 
            // mniOpenWorkspace
            // 
            this.mniOpenWorkspace.Name = "mniOpenWorkspace";
            this.mniOpenWorkspace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mniOpenWorkspace.Size = new System.Drawing.Size(250, 22);
            this.mniOpenWorkspace.Text = "Open &Workspace ...";
            this.mniOpenWorkspace.Click += new System.EventHandler(this.mniLoadWorkSpace_Click);
            // 
            // mniClose
            // 
            this.mniClose.Name = "mniClose";
            this.mniClose.ShortcutKeyDisplayString = "Ctrl + F4";
            this.mniClose.Size = new System.Drawing.Size(250, 22);
            this.mniClose.Text = "&Close";
            this.mniClose.Click += new System.EventHandler(this.mniClose_Click);
            // 
            // mniPrint
            // 
            this.mniPrint.Image = ((System.Drawing.Image)(resources.GetObject("mniPrint.Image")));
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new System.Drawing.Size(250, 22);
            this.mniPrint.Text = "Print";
            this.mniPrint.ToolTipText = "Print content from selected Tab";
            this.mniPrint.Click += new System.EventHandler(this.mniPrint_Click);
            // 
            // sepSave
            // 
            this.sepSave.Name = "sepSave";
            this.sepSave.Size = new System.Drawing.Size(247, 6);
            // 
            // mniSaveStrategy
            // 
            this.mniSaveStrategy.Image = ((System.Drawing.Image)(resources.GetObject("mniSaveStrategy.Image")));
            this.mniSaveStrategy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniSaveStrategy.Name = "mniSaveStrategy";
            this.mniSaveStrategy.ShortcutKeyDisplayString = "";
            this.mniSaveStrategy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mniSaveStrategy.Size = new System.Drawing.Size(250, 22);
            this.mniSaveStrategy.Text = "&Save";
            this.mniSaveStrategy.ToolTipText = "Save changes made to the Strategy";
            this.mniSaveStrategy.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // mniSaveStrategyAs
            // 
            this.mniSaveStrategyAs.Image = ((System.Drawing.Image)(resources.GetObject("mniSaveStrategyAs.Image")));
            this.mniSaveStrategyAs.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniSaveStrategyAs.Name = "mniSaveStrategyAs";
            this.mniSaveStrategyAs.ShowShortcutKeys = false;
            this.mniSaveStrategyAs.Size = new System.Drawing.Size(250, 22);
            this.mniSaveStrategyAs.Text = "Save &As ...";
            this.mniSaveStrategyAs.ToolTipText = "Save the current Strategy with a new Name ...";
            this.mniSaveStrategyAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // sepChart
            // 
            this.sepChart.Name = "sepChart";
            this.sepChart.Size = new System.Drawing.Size(247, 6);
            // 
            // mniPreferences2
            // 
            this.mniPreferences2.Image = ((System.Drawing.Image)(resources.GetObject("mniPreferences2.Image")));
            this.mniPreferences2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniPreferences2.Name = "mniPreferences2";
            this.mniPreferences2.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.mniPreferences2.Size = new System.Drawing.Size(250, 22);
            this.mniPreferences2.Text = "&Preferences";
            this.mniPreferences2.Click += new System.EventHandler(this.btnPreferencesTB_Click);
            // 
            // sepPreferences
            // 
            this.sepPreferences.Name = "sepPreferences";
            this.sepPreferences.Size = new System.Drawing.Size(247, 6);
            // 
            // mniLogin
            // 
            this.mniLogin.Image = ((System.Drawing.Image)(resources.GetObject("mniLogin.Image")));
            this.mniLogin.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniLogin.Name = "mniLogin";
            this.mniLogin.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.mniLogin.Size = new System.Drawing.Size(250, 22);
            this.mniLogin.Text = "Fidelity Account &Log in";
            this.mniLogin.Click += new System.EventHandler(this.btnLoginTradeTicket_Click);
            // 
            // mniOnDemand
            // 
            this.mniOnDemand.Checked = true;
            this.mniOnDemand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mniOnDemand.Name = "mniOnDemand";
            this.mniOnDemand.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.L)));
            this.mniOnDemand.Size = new System.Drawing.Size(250, 22);
            this.mniOnDemand.Text = "&Update Data on Demand";
            this.mniOnDemand.Click += new System.EventHandler(this.mniOnDemand_Click);
            // 
            // sepExit
            // 
            this.sepExit.Name = "sepExit";
            this.sepExit.Size = new System.Drawing.Size(247, 6);
            // 
            // mniCloseWorkspace
            // 
            this.mniCloseWorkspace.Name = "mniCloseWorkspace";
            this.mniCloseWorkspace.Size = new System.Drawing.Size(250, 22);
            this.mniCloseWorkspace.Text = "Clos&e this Workspace Window";
            this.mniCloseWorkspace.Visible = false;
            this.mniCloseWorkspace.Click += new System.EventHandler(this.mniCloseWorkspace_Click);
            // 
            // mniExit
            // 
            this.mniExit.Name = "mniExit";
            this.mniExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mniExit.Size = new System.Drawing.Size(250, 22);
            this.mniExit.Text = "E&xit";
            this.mniExit.Click += new System.EventHandler(this.mniExit_Click);
            // 
            // mniEdit
            // 
            this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniUndoDelete,
            this.toolStripSeparator1,
            this.mniCut,
            this.mniCopy,
            this.mniPaste,
            this.mniDelete,
            this.toolStripSeparator5,
            this.mniSelectAll,
            this.toolStripSeparator4,
            this.mniFind,
            this.mniFindReplace,
            this.sepSelectAll,
            this.mniSetTemplate});
            this.mniEdit.Enabled = false;
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.Size = new System.Drawing.Size(37, 20);
            this.mniEdit.Text = "&Edit";
            // 
            // mniUndoDelete
            // 
            this.mniUndoDelete.Image = ((System.Drawing.Image)(resources.GetObject("mniUndoDelete.Image")));
            this.mniUndoDelete.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniUndoDelete.Name = "mniUndoDelete";
            this.mniUndoDelete.ShortcutKeyDisplayString = "Ctrl+Z";
            this.mniUndoDelete.Size = new System.Drawing.Size(217, 22);
            this.mniUndoDelete.Text = "&Undo";
            this.mniUndoDelete.Click += new System.EventHandler(this.mniUndoDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(214, 6);
            // 
            // mniCut
            // 
            this.mniCut.Image = ((System.Drawing.Image)(resources.GetObject("mniCut.Image")));
            this.mniCut.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniCut.Name = "mniCut";
            this.mniCut.ShortcutKeyDisplayString = "Ctrl+X";
            this.mniCut.Size = new System.Drawing.Size(217, 22);
            this.mniCut.Text = "Cu&t";
            this.mniCut.Click += new System.EventHandler(this.mniCut_Click);
            // 
            // mniCopy
            // 
            this.mniCopy.Image = ((System.Drawing.Image)(resources.GetObject("mniCopy.Image")));
            this.mniCopy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.ShortcutKeyDisplayString = "Ctrl+c";
            this.mniCopy.Size = new System.Drawing.Size(217, 22);
            this.mniCopy.Text = "&Copy";
            this.mniCopy.Click += new System.EventHandler(this.mniCopy_Click);
            // 
            // mniPaste
            // 
            this.mniPaste.Image = ((System.Drawing.Image)(resources.GetObject("mniPaste.Image")));
            this.mniPaste.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniPaste.Name = "mniPaste";
            this.mniPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.mniPaste.Size = new System.Drawing.Size(217, 22);
            this.mniPaste.Text = "&Paste";
            this.mniPaste.Click += new System.EventHandler(this.mniPaste_Click);
            // 
            // mniDelete
            // 
            this.mniDelete.Image = ((System.Drawing.Image)(resources.GetObject("mniDelete.Image")));
            this.mniDelete.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniDelete.Name = "mniDelete";
            this.mniDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.mniDelete.Size = new System.Drawing.Size(217, 22);
            this.mniDelete.Text = "&Delete";
            this.mniDelete.Click += new System.EventHandler(this.mniDelete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(214, 6);
            // 
            // mniSelectAll
            // 
            this.mniSelectAll.Name = "mniSelectAll";
            this.mniSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mniSelectAll.Size = new System.Drawing.Size(217, 22);
            this.mniSelectAll.Text = "Select A&ll";
            this.mniSelectAll.Click += new System.EventHandler(this.mniSelectAll_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(214, 6);
            // 
            // mniFind
            // 
            this.mniFind.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniFind.Name = "mniFind";
            this.mniFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.mniFind.Size = new System.Drawing.Size(217, 22);
            this.mniFind.Text = "&Find";
            this.mniFind.Click += new System.EventHandler(this.mniFind_Click);
            // 
            // mniFindReplace
            // 
            this.mniFindReplace.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniFindReplace.Name = "mniFindReplace";
            this.mniFindReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.mniFindReplace.Size = new System.Drawing.Size(217, 22);
            this.mniFindReplace.Text = "Find && &Replace";
            this.mniFindReplace.Click += new System.EventHandler(this.mniFindReplace_Click);
            // 
            // sepSelectAll
            // 
            this.sepSelectAll.Name = "sepSelectAll";
            this.sepSelectAll.Size = new System.Drawing.Size(214, 6);
            // 
            // mniSetTemplate
            // 
            this.mniSetTemplate.Name = "mniSetTemplate";
            this.mniSetTemplate.Size = new System.Drawing.Size(217, 22);
            this.mniSetTemplate.Text = "&Set as Default Template Code";
            this.mniSetTemplate.Click += new System.EventHandler(this.mniSetTemplate_Click);
            // 
            // mniView
            // 
            this.mniView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniViewTradeTicket,
            this.mniViewDataPanel,
            this.mniViewStatusBar,
            this.sepView,
            this.mniViewNavBar,
            this.mniNavIcons,
            this.mniViewToolbar,
            this.mniViewDrawingBar,
            this.mniDataWindow});
            this.mniView.Name = "mniView";
            this.mniView.Size = new System.Drawing.Size(41, 20);
            this.mniView.Text = "&View";
            // 
            // mniViewTradeTicket
            // 
            this.mniViewTradeTicket.Name = "mniViewTradeTicket";
            this.mniViewTradeTicket.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.mniViewTradeTicket.Size = new System.Drawing.Size(173, 22);
            this.mniViewTradeTicket.Text = "&Trade Ticket";
            this.mniViewTradeTicket.Click += new System.EventHandler(this.btnTradeTicket_Click);
            // 
            // mniViewDataPanel
            // 
            this.mniViewDataPanel.Name = "mniViewDataPanel";
            this.mniViewDataPanel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mniViewDataPanel.Size = new System.Drawing.Size(173, 22);
            this.mniViewDataPanel.Text = "&Data Panel";
            this.mniViewDataPanel.Click += new System.EventHandler(this.mniViewDataPanel_Click);
            // 
            // mniViewStatusBar
            // 
            this.mniViewStatusBar.Name = "mniViewStatusBar";
            this.mniViewStatusBar.Size = new System.Drawing.Size(173, 22);
            this.mniViewStatusBar.Text = "&Status Bar";
            this.mniViewStatusBar.Click += new System.EventHandler(this.mniViewStatusBar_Click);
            // 
            // sepView
            // 
            this.sepView.Name = "sepView";
            this.sepView.Size = new System.Drawing.Size(170, 6);
            // 
            // mniViewNavBar
            // 
            this.mniViewNavBar.Name = "mniViewNavBar";
            this.mniViewNavBar.Size = new System.Drawing.Size(173, 22);
            this.mniViewNavBar.Text = "Navigation &Bar";
            this.mniViewNavBar.Click += new System.EventHandler(this.mniViewNavBar_Click);
            // 
            // mniNavIcons
            // 
            this.mniNavIcons.Name = "mniNavIcons";
            this.mniNavIcons.Size = new System.Drawing.Size(173, 22);
            this.mniNavIcons.Text = "Navigation Bar &Icons";
            this.mniNavIcons.Click += new System.EventHandler(this.mniNavIcons_Click);
            // 
            // mniViewToolbar
            // 
            this.mniViewToolbar.Name = "mniViewToolbar";
            this.mniViewToolbar.Size = new System.Drawing.Size(173, 22);
            this.mniViewToolbar.Text = "&Function Toolbar";
            this.mniViewToolbar.Click += new System.EventHandler(this.mniViewToolbar_Click);
            // 
            // mniViewDrawingBar
            // 
            this.mniViewDrawingBar.Name = "mniViewDrawingBar";
            this.mniViewDrawingBar.Size = new System.Drawing.Size(173, 22);
            this.mniViewDrawingBar.Text = "D&rawing Toolbar";
            this.mniViewDrawingBar.Click += new System.EventHandler(this.mniViewDrawingBar_Click);
            // 
            // mniDataWindow
            // 
            this.mniDataWindow.Name = "mniDataWindow";
            this.mniDataWindow.Size = new System.Drawing.Size(173, 22);
            this.mniDataWindow.Tag = "CS";
            this.mniDataWindow.Text = "Data &Window";
            this.mniDataWindow.Visible = false;
            this.mniDataWindow.Click += new System.EventHandler(this.btnDataWindow_Click);
            // 
            // mniTools
            // 
            this.mniTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniHomePage,
            this.mniAccounts,
            this.mniOrderManager,
            this.mniStrategyCenter,
            this.mniDataManager,
            this.symbolInfoManagerToolStripMenuItem,
            this.mniStrategyRanking,
            this.mniIndexManager,
            this.sepTools,
            this.mniIndicators,
            this.mniFundamentals,
            this.sepIndicators,
            this.mniDebug,
            this.mniQuickRef,
            this.sepQuickRef,
            this.mniPreferences,
            this.executeStrategyHiddenMenuItem});
            this.mniTools.Name = "mniTools";
            this.mniTools.Size = new System.Drawing.Size(44, 20);
            this.mniTools.Text = "&Tools";
            // 
            // mniHomePage
            // 
            this.mniHomePage.Image = ((System.Drawing.Image)(resources.GetObject("mniHomePage.Image")));
            this.mniHomePage.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniHomePage.Name = "mniHomePage";
            this.mniHomePage.Size = new System.Drawing.Size(278, 22);
            this.mniHomePage.Text = "&Home Page";
            this.mniHomePage.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // mniAccounts
            // 
            this.mniAccounts.Image = ((System.Drawing.Image)(resources.GetObject("mniAccounts.Image")));
            this.mniAccounts.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniAccounts.Name = "mniAccounts";
            this.mniAccounts.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.T)));
            this.mniAccounts.Size = new System.Drawing.Size(278, 22);
            this.mniAccounts.Text = "&Accounts";
            this.mniAccounts.Click += new System.EventHandler(this.btnAcctsPositions_Click);
            // 
            // mniOrderManager
            // 
            this.mniOrderManager.Image = ((System.Drawing.Image)(resources.GetObject("mniOrderManager.Image")));
            this.mniOrderManager.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniOrderManager.Name = "mniOrderManager";
            this.mniOrderManager.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mniOrderManager.Size = new System.Drawing.Size(278, 22);
            this.mniOrderManager.Text = "&Orders";
            this.mniOrderManager.Click += new System.EventHandler(this.btnOrdersAlerts_Click);
            // 
            // mniStrategyCenter
            // 
            this.mniStrategyCenter.Image = ((System.Drawing.Image)(resources.GetObject("mniStrategyCenter.Image")));
            this.mniStrategyCenter.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniStrategyCenter.Name = "mniStrategyCenter";
            this.mniStrategyCenter.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.mniStrategyCenter.Size = new System.Drawing.Size(278, 22);
            this.mniStrategyCenter.Text = "&Strategy Monitor";
            this.mniStrategyCenter.Click += new System.EventHandler(this.btnStrategyCenter_Click);
            // 
            // mniDataManager
            // 
            this.mniDataManager.Image = ((System.Drawing.Image)(resources.GetObject("mniDataManager.Image")));
            this.mniDataManager.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniDataManager.Name = "mniDataManager";
            this.mniDataManager.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mniDataManager.Size = new System.Drawing.Size(278, 22);
            this.mniDataManager.Text = "&Data Manager";
            this.mniDataManager.Click += new System.EventHandler(this.btnDataManager_Click);
            // 
            // symbolInfoManagerToolStripMenuItem
            // 
            this.symbolInfoManagerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("symbolInfoManagerToolStripMenuItem.Image")));
            this.symbolInfoManagerToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.symbolInfoManagerToolStripMenuItem.Name = "symbolInfoManagerToolStripMenuItem";
            this.symbolInfoManagerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.F)));
            this.symbolInfoManagerToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.symbolInfoManagerToolStripMenuItem.Text = "Symbol &Info Manager";
            this.symbolInfoManagerToolStripMenuItem.Click += new System.EventHandler(this.symbolInfoManagerToolStripMenuItem_Click);
            // 
            // mniStrategyRanking
            // 
            this.mniStrategyRanking.Image = ((System.Drawing.Image)(resources.GetObject("mniStrategyRanking.Image")));
            this.mniStrategyRanking.Name = "mniStrategyRanking";
            this.mniStrategyRanking.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.R)));
            this.mniStrategyRanking.Size = new System.Drawing.Size(278, 22);
            this.mniStrategyRanking.Text = "Strategy &Ranking";
            this.mniStrategyRanking.Click += new System.EventHandler(this.mniStrategyRanking_Click);
            // 
            // mniIndexManager
            // 
            this.mniIndexManager.Image = ((System.Drawing.Image)(resources.GetObject("mniIndexManager.Image")));
            this.mniIndexManager.Name = "mniIndexManager";
            this.mniIndexManager.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.I)));
            this.mniIndexManager.Size = new System.Drawing.Size(278, 22);
            this.mniIndexManager.Text = "Index-Lab ®";
            this.mniIndexManager.Click += new System.EventHandler(this.mniIndexManager_Click);
            // 
            // sepTools
            // 
            this.sepTools.Name = "sepTools";
            this.sepTools.Size = new System.Drawing.Size(275, 6);
            // 
            // mniIndicators
            // 
            this.mniIndicators.Image = ((System.Drawing.Image)(resources.GetObject("mniIndicators.Image")));
            this.mniIndicators.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniIndicators.Name = "mniIndicators";
            this.mniIndicators.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F11)));
            this.mniIndicators.Size = new System.Drawing.Size(278, 22);
            this.mniIndicators.Text = "&Technical Indicators";
            this.mniIndicators.Click += new System.EventHandler(this.btnIndicatorsTB2_Click);
            // 
            // mniFundamentals
            // 
            this.mniFundamentals.Image = ((System.Drawing.Image)(resources.GetObject("mniFundamentals.Image")));
            this.mniFundamentals.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniFundamentals.Name = "mniFundamentals";
            this.mniFundamentals.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.mniFundamentals.Size = new System.Drawing.Size(278, 22);
            this.mniFundamentals.Text = "&Fundamental Data Items";
            this.mniFundamentals.Click += new System.EventHandler(this.btnFundamentalsTB2_Click);
            this.mniFundamentals.VisibleChanged += new System.EventHandler(this.mniFundamentals_VisibleChanged);
            // 
            // sepIndicators
            // 
            this.sepIndicators.Name = "sepIndicators";
            this.sepIndicators.Size = new System.Drawing.Size(275, 6);
            // 
            // mniDebug
            // 
            this.mniDebug.Image = ((System.Drawing.Image)(resources.GetObject("mniDebug.Image")));
            this.mniDebug.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniDebug.Name = "mniDebug";
            this.mniDebug.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.D)));
            this.mniDebug.Size = new System.Drawing.Size(278, 22);
            this.mniDebug.Text = "Debug and Error Message &Log";
            this.mniDebug.Click += new System.EventHandler(this.mniDebug_Click);
            // 
            // mniQuickRef
            // 
            this.mniQuickRef.Image = ((System.Drawing.Image)(resources.GetObject("mniQuickRef.Image")));
            this.mniQuickRef.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniQuickRef.Name = "mniQuickRef";
            this.mniQuickRef.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.mniQuickRef.Size = new System.Drawing.Size(278, 22);
            this.mniQuickRef.Text = "WealthScript &QuickRef";
            this.mniQuickRef.Click += new System.EventHandler(this.mniQuickRef2_Click);
            // 
            // sepQuickRef
            // 
            this.sepQuickRef.Name = "sepQuickRef";
            this.sepQuickRef.Size = new System.Drawing.Size(275, 6);
            // 
            // mniPreferences
            // 
            this.mniPreferences.Image = ((System.Drawing.Image)(resources.GetObject("mniPreferences.Image")));
            this.mniPreferences.ImageTransparentColor = System.Drawing.Color.Silver;
            this.mniPreferences.Name = "mniPreferences";
            this.mniPreferences.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.mniPreferences.Size = new System.Drawing.Size(278, 22);
            this.mniPreferences.Text = "&Preferences";
            this.mniPreferences.Click += new System.EventHandler(this.btnPreferencesTB_Click);
            // 
            // executeStrategyHiddenMenuItem
            // 
            this.executeStrategyHiddenMenuItem.Name = "executeStrategyHiddenMenuItem";
            this.executeStrategyHiddenMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.executeStrategyHiddenMenuItem.Size = new System.Drawing.Size(278, 22);
            this.executeStrategyHiddenMenuItem.Text = "&Execute Strategy";
            this.executeStrategyHiddenMenuItem.Visible = false;
            this.executeStrategyHiddenMenuItem.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // mniWorkspaces
            // 
            this.mniWorkspaces.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniNewWorkspace3,
            this.toolStripSeparator3,
            this.mniLoadWorkSpace,
            this.mniSaveWorkSpace,
            this.sepSaveWorkspace,
            this.mniSetDefaultWorkspace,
            this.toolStripSeparator7});
            this.mniWorkspaces.Name = "mniWorkspaces";
            this.mniWorkspaces.Size = new System.Drawing.Size(77, 20);
            this.mniWorkspaces.Text = "W&orkspaces";
            // 
            // mniNewWorkspace3
            // 
            this.mniNewWorkspace3.Name = "mniNewWorkspace3";
            this.mniNewWorkspace3.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.W)));
            this.mniNewWorkspace3.Size = new System.Drawing.Size(289, 22);
            this.mniNewWorkspace3.Text = "New Main &Workspace Window";
            this.mniNewWorkspace3.Click += new System.EventHandler(this.mniNewWorkspaceTB_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(286, 6);
            // 
            // mniLoadWorkSpace
            // 
            this.mniLoadWorkSpace.Name = "mniLoadWorkSpace";
            this.mniLoadWorkSpace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mniLoadWorkSpace.Size = new System.Drawing.Size(289, 22);
            this.mniLoadWorkSpace.Text = "&Open Workspace ...";
            this.mniLoadWorkSpace.Click += new System.EventHandler(this.mniLoadWorkSpace_Click);
            // 
            // mniSaveWorkSpace
            // 
            this.mniSaveWorkSpace.Name = "mniSaveWorkSpace";
            this.mniSaveWorkSpace.Size = new System.Drawing.Size(289, 22);
            this.mniSaveWorkSpace.Text = "&Save Workspace ...";
            this.mniSaveWorkSpace.Click += new System.EventHandler(this.mniSaveWorkSpace_Click);
            // 
            // sepSaveWorkspace
            // 
            this.sepSaveWorkspace.Name = "sepSaveWorkspace";
            this.sepSaveWorkspace.Size = new System.Drawing.Size(286, 6);
            // 
            // mniSetDefaultWorkspace
            // 
            this.mniSetDefaultWorkspace.Name = "mniSetDefaultWorkspace";
            this.mniSetDefaultWorkspace.Size = new System.Drawing.Size(289, 22);
            this.mniSetDefaultWorkspace.Text = "Set as &Default Workspace";
            this.mniSetDefaultWorkspace.Click += new System.EventHandler(this.mniSetDefaultWorkspace_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(286, 6);
            // 
            // mniWindow
            // 
            this.mniWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniCascade,
            this.mniTileHorizontally,
            this.mniTileVertically,
            this.toolStripSeparator2});
            this.mniWindow.Name = "mniWindow";
            this.mniWindow.Size = new System.Drawing.Size(57, 20);
            this.mniWindow.Text = "&Window";
            // 
            // mniCascade
            // 
            this.mniCascade.Name = "mniCascade";
            this.mniCascade.Size = new System.Drawing.Size(149, 22);
            this.mniCascade.Text = "&Cascade";
            this.mniCascade.Click += new System.EventHandler(this.mniCascade_Click);
            // 
            // mniTileHorizontally
            // 
            this.mniTileHorizontally.Name = "mniTileHorizontally";
            this.mniTileHorizontally.Size = new System.Drawing.Size(149, 22);
            this.mniTileHorizontally.Text = "Tile &Horizontally";
            this.mniTileHorizontally.Click += new System.EventHandler(this.mniTileHorizontally_Click);
            // 
            // mniTileVertically
            // 
            this.mniTileVertically.Name = "mniTileVertically";
            this.mniTileVertically.Size = new System.Drawing.Size(149, 22);
            this.mniTileVertically.Text = "Tile &Vertically";
            this.mniTileVertically.Click += new System.EventHandler(this.mniTileVertically_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(146, 6);
            // 
            // mniHelp
            // 
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniUserGuide,
            this.mniQuickRef2,
            this.mniLanguageGuide,
            this.sepHelp,
            this.mniFidelityCom,
            this.mniWealthLabCom,
            this.sepHelp2,
            this.mniSoftwareUpgrade,
            this.sepUpgrade,
            this.mniAbout});
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mniHelp.Size = new System.Drawing.Size(40, 20);
            this.mniHelp.Text = "&Help";
            // 
            // mniUserGuide
            // 
            this.mniUserGuide.Image = ((System.Drawing.Image)(resources.GetObject("mniUserGuide.Image")));
            this.mniUserGuide.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniUserGuide.Name = "mniUserGuide";
            this.mniUserGuide.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mniUserGuide.Size = new System.Drawing.Size(230, 22);
            this.mniUserGuide.Text = "Wealth-Lab &User Guide";
            this.mniUserGuide.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // mniQuickRef2
            // 
            this.mniQuickRef2.Image = ((System.Drawing.Image)(resources.GetObject("mniQuickRef2.Image")));
            this.mniQuickRef2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniQuickRef2.Name = "mniQuickRef2";
            this.mniQuickRef2.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.mniQuickRef2.Size = new System.Drawing.Size(230, 22);
            this.mniQuickRef2.Text = "WealthScript &QuickRef";
            this.mniQuickRef2.Click += new System.EventHandler(this.mniQuickRef2_Click);
            // 
            // mniLanguageGuide
            // 
            this.mniLanguageGuide.Name = "mniLanguageGuide";
            this.mniLanguageGuide.Size = new System.Drawing.Size(230, 22);
            this.mniLanguageGuide.Text = "WealthScript Programming Guide";
            this.mniLanguageGuide.Click += new System.EventHandler(this.mniLanguageGuide_Click);
            // 
            // sepHelp
            // 
            this.sepHelp.Name = "sepHelp";
            this.sepHelp.Size = new System.Drawing.Size(227, 6);
            // 
            // mniFidelityCom
            // 
            this.mniFidelityCom.Name = "mniFidelityCom";
            this.mniFidelityCom.Size = new System.Drawing.Size(230, 22);
            this.mniFidelityCom.Text = "&Fidelity.com";
            this.mniFidelityCom.Click += new System.EventHandler(this.mniFidelityCom_Click);
            // 
            // mniWealthLabCom
            // 
            this.mniWealthLabCom.Name = "mniWealthLabCom";
            this.mniWealthLabCom.Size = new System.Drawing.Size(230, 22);
            this.mniWealthLabCom.Text = "&Wealth-Lab.com";
            this.mniWealthLabCom.Click += new System.EventHandler(this.mniWealthLabCom_Click);
            // 
            // sepHelp2
            // 
            this.sepHelp2.Name = "sepHelp2";
            this.sepHelp2.Size = new System.Drawing.Size(227, 6);
            // 
            // mniSoftwareUpgrade
            // 
            this.mniSoftwareUpgrade.Name = "mniSoftwareUpgrade";
            this.mniSoftwareUpgrade.Size = new System.Drawing.Size(230, 22);
            this.mniSoftwareUpgrade.Text = "Software Upgrade";
            this.mniSoftwareUpgrade.Click += new System.EventHandler(this.mniSoftwareUpgrade_Click);
            // 
            // sepUpgrade
            // 
            this.sepUpgrade.Name = "sepUpgrade";
            this.sepUpgrade.Size = new System.Drawing.Size(227, 6);
            // 
            // mniAbout
            // 
            this.mniAbout.Name = "mniAbout";
            this.mniAbout.Size = new System.Drawing.Size(230, 22);
            this.mniAbout.Text = "&About Wealth-Lab Developer ...";
            this.mniAbout.Click += new System.EventHandler(this.mniAbout_Click);
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage,
            this.statusOrders,
            this.statusActive,
            this.statusStreamingProvider,
            this.statusStreamingStatus,
            this.statusStreamingSymbolsOff,
            this.statusStreamingSymbolsOn,
            this.statusSofwareDownload,
            this.statusDownloadProgressBar});
            this.status.Location = new System.Drawing.Point(0, 458);
            this.status.Name = "status";
            this.status.ShowItemToolTips = true;
            this.status.Size = new System.Drawing.Size(1028, 22);
            this.status.TabIndex = 6;
            this.status.Text = "statusStrip1";
            // 
            // statusMessage
            // 
            this.statusMessage.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusMessage.Name = "statusMessage";
            this.statusMessage.Size = new System.Drawing.Size(201, 17);
            this.statusMessage.Text = "Strategy Status Message (click to clear)";
            this.statusMessage.Click += new System.EventHandler(this.statusMessage_Click);
            // 
            // statusOrders
            // 
            this.statusOrders.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusOrders.IsLink = true;
            this.statusOrders.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.statusOrders.Name = "statusOrders";
            this.statusOrders.Size = new System.Drawing.Size(57, 17);
            this.statusOrders.Text = "Orders: 0";
            this.statusOrders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusOrders.Click += new System.EventHandler(this.statusActive_Click);
            // 
            // statusActive
            // 
            this.statusActive.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusActive.IsLink = true;
            this.statusActive.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.statusActive.Name = "statusActive";
            this.statusActive.Size = new System.Drawing.Size(81, 17);
            this.statusActive.Text = "Active Orders:";
            this.statusActive.Click += new System.EventHandler(this.statusActive_Click);
            // 
            // statusStreamingProvider
            // 
            this.statusStreamingProvider.Image = ((System.Drawing.Image)(resources.GetObject("statusStreamingProvider.Image")));
            this.statusStreamingProvider.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.statusStreamingProvider.LinkColor = System.Drawing.Color.Red;
            this.statusStreamingProvider.Name = "statusStreamingProvider";
            this.statusStreamingProvider.Size = new System.Drawing.Size(118, 17);
            this.statusStreamingProvider.Text = "Streaming Provider:";
            this.statusStreamingProvider.Visible = false;
            this.statusStreamingProvider.Click += new System.EventHandler(this.statusStreamingProvider_Click);
            // 
            // statusStreamingStatus
            // 
            this.statusStreamingStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusStreamingStatus.ForeColor = System.Drawing.Color.Green;
            this.statusStreamingStatus.LinkColor = System.Drawing.Color.Red;
            this.statusStreamingStatus.Name = "statusStreamingStatus";
            this.statusStreamingStatus.Size = new System.Drawing.Size(33, 20);
            this.statusStreamingStatus.Text = "(OK)";
            this.statusStreamingStatus.Visible = false;
            this.statusStreamingStatus.Click += new System.EventHandler(this.statusStreamingStatus_Click);
            // 
            // statusStreamingSymbolsOff
            // 
            this.statusStreamingSymbolsOff.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusStreamingSymbolsOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.statusStreamingSymbolsOff.Image = ((System.Drawing.Image)(resources.GetObject("statusStreamingSymbolsOff.Image")));
            this.statusStreamingSymbolsOff.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.statusStreamingSymbolsOff.Name = "statusStreamingSymbolsOff";
            this.statusStreamingSymbolsOff.Size = new System.Drawing.Size(20, 20);
            this.statusStreamingSymbolsOff.ToolTipText = "Click to turn off Streaming for Symbols in Status Bar";
            this.statusStreamingSymbolsOff.Visible = false;
            this.statusStreamingSymbolsOff.Click += new System.EventHandler(this.statusStreamingSymbolsOff_Click);
            // 
            // statusStreamingSymbolsOn
            // 
            this.statusStreamingSymbolsOn.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusStreamingSymbolsOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.statusStreamingSymbolsOn.Image = ((System.Drawing.Image)(resources.GetObject("statusStreamingSymbolsOn.Image")));
            this.statusStreamingSymbolsOn.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.statusStreamingSymbolsOn.Name = "statusStreamingSymbolsOn";
            this.statusStreamingSymbolsOn.Size = new System.Drawing.Size(20, 20);
            this.statusStreamingSymbolsOn.ToolTipText = "Click to turn on Streaming for Symbols in Status Bar";
            this.statusStreamingSymbolsOn.Visible = false;
            this.statusStreamingSymbolsOn.Click += new System.EventHandler(this.statusStreamingSymbolsOn_Click);
            // 
            // statusSofwareDownload
            // 
            this.statusSofwareDownload.Name = "statusSofwareDownload";
            this.statusSofwareDownload.Size = new System.Drawing.Size(105, 17);
            this.statusSofwareDownload.Text = "Software Download:";
            this.statusSofwareDownload.Visible = false;
            // 
            // statusDownloadProgressBar
            // 
            this.statusDownloadProgressBar.Name = "statusDownloadProgressBar";
            this.statusDownloadProgressBar.Size = new System.Drawing.Size(100, 16);
            this.statusDownloadProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.statusDownloadProgressBar.ToolTipText = "Software Upgrade Progress";
            this.statusDownloadProgressBar.Visible = false;
            // 
            // stlblHolder
            // 
            this.stlblHolder.Name = "stlblHolder";
            this.stlblHolder.Size = new System.Drawing.Size(59, 17);
            this.stlblHolder.Text = "Status: OK";
            // 
            // toolbarNav
            // 
            this.toolbarNav.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.toolbarNav.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolbarNav.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarNav.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnHome,
            this.dropdownCharts,
            this.btnStrategyCenter,
            this.dropdownQuotes,
            this.btnOrdersAlerts,
            this.btnAcctsPositions,
            this.btnDataManager,
            this.sepDataManager,
            this.btnIndicators,
            this.btnFundamental,
            this.btnHelp,
            this.btnPreferences,
            this.btnTrade});
            this.toolbarNav.Location = new System.Drawing.Point(0, 24);
            this.toolbarNav.Name = "toolbarNav";
            this.toolbarNav.Size = new System.Drawing.Size(1028, 25);
            this.toolbarNav.TabIndex = 2;
            this.toolbarNav.Text = "toolStrip1";
            // 
            // btnHome
            // 
            this.btnHome.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.Image")));
            this.btnHome.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(54, 22);
            this.btnHome.Tag = "P";
            this.btnHome.Text = "Home";
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // dropdownCharts
            // 
            this.dropdownCharts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniNewChart2,
            this.mniNewRules2,
            this.mniNewCode2,
            this.mniNewMultiStrategyBuilder2,
            this.mniOpenStrategy2,
            this.sepChart2,
            this.mniChartFront,
            this.sepChartFront});
            this.dropdownCharts.Image = ((System.Drawing.Image)(resources.GetObject("dropdownCharts.Image")));
            this.dropdownCharts.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dropdownCharts.Name = "dropdownCharts";
            this.dropdownCharts.Size = new System.Drawing.Size(130, 22);
            this.dropdownCharts.Tag = "P";
            this.dropdownCharts.Text = "Charts && Strategies";
            this.dropdownCharts.ToolTipText = "Charts & Strategies";
            // 
            // mniNewChart2
            // 
            this.mniNewChart2.Image = ((System.Drawing.Image)(resources.GetObject("mniNewChart2.Image")));
            this.mniNewChart2.Name = "mniNewChart2";
            this.mniNewChart2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.C)));
            this.mniNewChart2.Size = new System.Drawing.Size(272, 22);
            this.mniNewChart2.Text = "New Chart";
            this.mniNewChart2.Click += new System.EventHandler(this.mniNewChartDD_Click);
            // 
            // mniNewRules2
            // 
            this.mniNewRules2.Image = ((System.Drawing.Image)(resources.GetObject("mniNewRules2.Image")));
            this.mniNewRules2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewRules2.Name = "mniNewRules2";
            this.mniNewRules2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.R)));
            this.mniNewRules2.Size = new System.Drawing.Size(272, 22);
            this.mniNewRules2.Text = "New Strategy from Rules";
            this.mniNewRules2.Click += new System.EventHandler(this.mniNewStrategyRulesDD_Click);
            // 
            // mniNewCode2
            // 
            this.mniNewCode2.Image = ((System.Drawing.Image)(resources.GetObject("mniNewCode2.Image")));
            this.mniNewCode2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewCode2.Name = "mniNewCode2";
            this.mniNewCode2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.mniNewCode2.Size = new System.Drawing.Size(272, 22);
            this.mniNewCode2.Text = "New Strategy from Code";
            this.mniNewCode2.Click += new System.EventHandler(this.newStrategyFromCodeToolStripMenuItem_Click);
            // 
            // mniNewMultiStrategyBuilder2
            // 
            this.mniNewMultiStrategyBuilder2.Image = ((System.Drawing.Image)(resources.GetObject("mniNewMultiStrategyBuilder2.Image")));
            this.mniNewMultiStrategyBuilder2.Name = "mniNewMultiStrategyBuilder2";
            this.mniNewMultiStrategyBuilder2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.M)));
            this.mniNewMultiStrategyBuilder2.Size = new System.Drawing.Size(272, 22);
            this.mniNewMultiStrategyBuilder2.Text = "New Combination Strategy";
            this.mniNewMultiStrategyBuilder2.Click += new System.EventHandler(this.mniNewCombinationStrategy_Click);
            // 
            // mniOpenStrategy2
            // 
            this.mniOpenStrategy2.Image = ((System.Drawing.Image)(resources.GetObject("mniOpenStrategy2.Image")));
            this.mniOpenStrategy2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniOpenStrategy2.Name = "mniOpenStrategy2";
            this.mniOpenStrategy2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mniOpenStrategy2.Size = new System.Drawing.Size(272, 22);
            this.mniOpenStrategy2.Text = "Open Strategy ...";
            this.mniOpenStrategy2.Click += new System.EventHandler(this.btnOpenStrategy_Click);
            // 
            // sepChart2
            // 
            this.sepChart2.Name = "sepChart2";
            this.sepChart2.Size = new System.Drawing.Size(269, 6);
            // 
            // mniChartFront
            // 
            this.mniChartFront.Name = "mniChartFront";
            this.mniChartFront.Size = new System.Drawing.Size(272, 22);
            this.mniChartFront.Text = "Bring all to front";
            this.mniChartFront.Click += new System.EventHandler(this.mniChartFront_Click);
            // 
            // sepChartFront
            // 
            this.sepChartFront.Name = "sepChartFront";
            this.sepChartFront.Size = new System.Drawing.Size(269, 6);
            // 
            // btnStrategyCenter
            // 
            this.btnStrategyCenter.Image = ((System.Drawing.Image)(resources.GetObject("btnStrategyCenter.Image")));
            this.btnStrategyCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStrategyCenter.Name = "btnStrategyCenter";
            this.btnStrategyCenter.Size = new System.Drawing.Size(108, 22);
            this.btnStrategyCenter.Tag = "P";
            this.btnStrategyCenter.Text = "Strategy Monitor";
            this.btnStrategyCenter.ToolTipText = "Allows you to Activate Strategies and automatically produce Alerts and Trades";
            this.btnStrategyCenter.Click += new System.EventHandler(this.btnStrategyCenter_Click);
            // 
            // dropdownQuotes
            // 
            this.dropdownQuotes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniNewQuote2,
            this.sepQuote2,
            this.mniQuoteAll,
            this.sepQuoteFront});
            this.dropdownQuotes.Image = ((System.Drawing.Image)(resources.GetObject("dropdownQuotes.Image")));
            this.dropdownQuotes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dropdownQuotes.Name = "dropdownQuotes";
            this.dropdownQuotes.Size = new System.Drawing.Size(71, 22);
            this.dropdownQuotes.Tag = "P";
            this.dropdownQuotes.Text = "Quotes";
            this.dropdownQuotes.ToolTipText = "Quotes";
            // 
            // mniNewQuote2
            // 
            this.mniNewQuote2.Image = ((System.Drawing.Image)(resources.GetObject("mniNewQuote2.Image")));
            this.mniNewQuote2.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewQuote2.Name = "mniNewQuote2";
            this.mniNewQuote2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.Q)));
            this.mniNewQuote2.Size = new System.Drawing.Size(239, 22);
            this.mniNewQuote2.Text = "New Quote Window";
            this.mniNewQuote2.Click += new System.EventHandler(this.mniNewQuoteDD_Click);
            // 
            // sepQuote2
            // 
            this.sepQuote2.Name = "sepQuote2";
            this.sepQuote2.Size = new System.Drawing.Size(236, 6);
            // 
            // mniQuoteAll
            // 
            this.mniQuoteAll.Name = "mniQuoteAll";
            this.mniQuoteAll.Size = new System.Drawing.Size(239, 22);
            this.mniQuoteAll.Text = "Bring all to front";
            this.mniQuoteAll.Click += new System.EventHandler(this.mniQuoteAll_Click);
            // 
            // sepQuoteFront
            // 
            this.sepQuoteFront.Name = "sepQuoteFront";
            this.sepQuoteFront.Size = new System.Drawing.Size(236, 6);
            // 
            // btnOrdersAlerts
            // 
            this.btnOrdersAlerts.Image = ((System.Drawing.Image)(resources.GetObject("btnOrdersAlerts.Image")));
            this.btnOrdersAlerts.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOrdersAlerts.Name = "btnOrdersAlerts";
            this.btnOrdersAlerts.Size = new System.Drawing.Size(60, 22);
            this.btnOrdersAlerts.Tag = "P";
            this.btnOrdersAlerts.Text = "Orders";
            this.btnOrdersAlerts.ToolTipText = "Open the Order Manager";
            this.btnOrdersAlerts.Click += new System.EventHandler(this.btnOrdersAlerts_Click);
            // 
            // btnAcctsPositions
            // 
            this.btnAcctsPositions.Image = ((System.Drawing.Image)(resources.GetObject("btnAcctsPositions.Image")));
            this.btnAcctsPositions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAcctsPositions.Name = "btnAcctsPositions";
            this.btnAcctsPositions.Size = new System.Drawing.Size(71, 22);
            this.btnAcctsPositions.Tag = "P";
            this.btnAcctsPositions.Text = "Accounts";
            this.btnAcctsPositions.ToolTipText = "Open the Balances and Positions Manager";
            this.btnAcctsPositions.Click += new System.EventHandler(this.btnAcctsPositions_Click);
            // 
            // btnDataManager
            // 
            this.btnDataManager.Image = ((System.Drawing.Image)(resources.GetObject("btnDataManager.Image")));
            this.btnDataManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDataManager.Name = "btnDataManager";
            this.btnDataManager.Size = new System.Drawing.Size(95, 22);
            this.btnDataManager.Tag = "P";
            this.btnDataManager.Text = "Data Manager";
            this.btnDataManager.Click += new System.EventHandler(this.btnDataManager_Click);
            // 
            // sepDataManager
            // 
            this.sepDataManager.Name = "sepDataManager";
            this.sepDataManager.Size = new System.Drawing.Size(6, 25);
            // 
            // btnIndicators
            // 
            this.btnIndicators.Image = ((System.Drawing.Image)(resources.GetObject("btnIndicators.Image")));
            this.btnIndicators.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnIndicators.Name = "btnIndicators";
            this.btnIndicators.Size = new System.Drawing.Size(75, 22);
            this.btnIndicators.Tag = "I";
            this.btnIndicators.Text = "Indicators";
            this.btnIndicators.ToolTipText = "Open the Indicators Window";
            this.btnIndicators.Click += new System.EventHandler(this.btnIndicatorsTB2_Click);
            // 
            // btnFundamental
            // 
            this.btnFundamental.Image = ((System.Drawing.Image)(resources.GetObject("btnFundamental.Image")));
            this.btnFundamental.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFundamental.Name = "btnFundamental";
            this.btnFundamental.Size = new System.Drawing.Size(94, 22);
            this.btnFundamental.Tag = "I";
            this.btnFundamental.Text = "Fundamentals";
            this.btnFundamental.ToolTipText = "Open the Fundamental Items Window";
            this.btnFundamental.Click += new System.EventHandler(this.btnFundamentalsTB2_Click);
            this.btnFundamental.VisibleChanged += new System.EventHandler(this.btnFundamental_VisibleChanged);
            // 
            // btnHelp
            // 
            this.btnHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(23, 22);
            this.btnHelp.Text = "toolStripButton1";
            this.btnHelp.ToolTipText = "Wealth-Lab Pro Help";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnPreferences
            // 
            this.btnPreferences.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnPreferences.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPreferences.Image = ((System.Drawing.Image)(resources.GetObject("btnPreferences.Image")));
            this.btnPreferences.ImageTransparentColor = System.Drawing.Color.Silver;
            this.btnPreferences.Name = "btnPreferences";
            this.btnPreferences.Size = new System.Drawing.Size(23, 22);
            this.btnPreferences.Text = "toolStripButton1";
            this.btnPreferences.ToolTipText = "Preferences";
            this.btnPreferences.Click += new System.EventHandler(this.btnPreferencesTB_Click);
            // 
            // btnTrade
            // 
            this.btnTrade.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnTrade.Checked = true;
            this.btnTrade.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnTrade.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTrade.Image = ((System.Drawing.Image)(resources.GetObject("btnTrade.Image")));
            this.btnTrade.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTrade.Name = "btnTrade";
            this.btnTrade.Size = new System.Drawing.Size(23, 22);
            this.btnTrade.Text = "toolStripButton1";
            this.btnTrade.ToolTipText = "Show/Hide the Trade Ticket";
            this.btnTrade.Visible = false;
            this.btnTrade.Click += new System.EventHandler(this.btnTradeTicket_Click);
            // 
            // toolbar
            // 
            this.toolbar.AutoSize = false;
            this.toolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(191)))), ((int)(((byte)(211)))));
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.btnOpenStrategy,
            this.btnSave,
            this.btnSaveAs,
            this.sepChartFile,
            this.btnLogin,
            this.sepSpacing,
            this.lblSpacing,
            this.btnIncreaseSpacing,
            this.btnRestoreSpacing,
            this.btnDecreaseSpacing,
            this.sepBarSpacing,
            this.tslblChartStyles,
            this.btnCandleStyle,
            this.btnBarChart,
            this.btnLineChart,
            this.tsmMoreChartStyles,
            this.btnLinear,
            this.btnLog,
            this.sepChartStyles,
            this.tslblOptions,
            this.btnLabelsVisible,
            this.btnStatusBarVisible,
            this.btnFundamentalsVisible,
            this.btnDataWindow,
            this.btnIndicatorsTB2,
            this.btnFundamentalsTB2,
            this.btnClearIndicators,
            this.btnPushCode,
            this.btnTradeTicket,
            this.btnPreferencesTB});
            this.toolbar.Location = new System.Drawing.Point(0, 49);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(1028, 28);
            this.toolbar.TabIndex = 3;
            this.toolbar.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniNewChartDD,
            this.mniNewStrategyRulesDD,
            this.newStrategyFromCodeToolStripMenuItem,
            this.mniNewCombinationStrategy,
            this.sepNew,
            this.mniNewWorkspaceTB,
            this.mniNewQuoteDD,
            this.sepNew2,
            this.mniNewDataSetTB});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(57, 25);
            this.toolStripDropDownButton1.Tag = "*";
            this.toolStripDropDownButton1.Text = "New";
            this.toolStripDropDownButton1.ToolTipText = "Open a New Chart Window";
            // 
            // mniNewChartDD
            // 
            this.mniNewChartDD.Image = ((System.Drawing.Image)(resources.GetObject("mniNewChartDD.Image")));
            this.mniNewChartDD.Name = "mniNewChartDD";
            this.mniNewChartDD.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.C)));
            this.mniNewChartDD.Size = new System.Drawing.Size(289, 22);
            this.mniNewChartDD.Text = "New Chart Window";
            this.mniNewChartDD.Click += new System.EventHandler(this.mniNewChartDD_Click);
            // 
            // mniNewStrategyRulesDD
            // 
            this.mniNewStrategyRulesDD.Image = ((System.Drawing.Image)(resources.GetObject("mniNewStrategyRulesDD.Image")));
            this.mniNewStrategyRulesDD.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewStrategyRulesDD.Name = "mniNewStrategyRulesDD";
            this.mniNewStrategyRulesDD.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.R)));
            this.mniNewStrategyRulesDD.Size = new System.Drawing.Size(289, 22);
            this.mniNewStrategyRulesDD.Text = "New Strategy from Rules";
            this.mniNewStrategyRulesDD.Click += new System.EventHandler(this.mniNewStrategyRulesDD_Click);
            // 
            // newStrategyFromCodeToolStripMenuItem
            // 
            this.newStrategyFromCodeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newStrategyFromCodeToolStripMenuItem.Image")));
            this.newStrategyFromCodeToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.newStrategyFromCodeToolStripMenuItem.Name = "newStrategyFromCodeToolStripMenuItem";
            this.newStrategyFromCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.newStrategyFromCodeToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.newStrategyFromCodeToolStripMenuItem.Text = "New Strategy from Code";
            this.newStrategyFromCodeToolStripMenuItem.Click += new System.EventHandler(this.newStrategyFromCodeToolStripMenuItem_Click);
            // 
            // mniNewCombinationStrategy
            // 
            this.mniNewCombinationStrategy.Image = ((System.Drawing.Image)(resources.GetObject("mniNewCombinationStrategy.Image")));
            this.mniNewCombinationStrategy.Name = "mniNewCombinationStrategy";
            this.mniNewCombinationStrategy.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.M)));
            this.mniNewCombinationStrategy.Size = new System.Drawing.Size(289, 22);
            this.mniNewCombinationStrategy.Text = "New Combination Strategy";
            this.mniNewCombinationStrategy.Click += new System.EventHandler(this.mniNewCombinationStrategy_Click);
            // 
            // sepNew
            // 
            this.sepNew.Name = "sepNew";
            this.sepNew.Size = new System.Drawing.Size(286, 6);
            // 
            // mniNewWorkspaceTB
            // 
            this.mniNewWorkspaceTB.Name = "mniNewWorkspaceTB";
            this.mniNewWorkspaceTB.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.W)));
            this.mniNewWorkspaceTB.Size = new System.Drawing.Size(289, 22);
            this.mniNewWorkspaceTB.Text = "New Main Workspace Window";
            this.mniNewWorkspaceTB.Click += new System.EventHandler(this.mniNewWorkspaceTB_Click);
            // 
            // mniNewQuoteDD
            // 
            this.mniNewQuoteDD.Image = ((System.Drawing.Image)(resources.GetObject("mniNewQuoteDD.Image")));
            this.mniNewQuoteDD.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewQuoteDD.Name = "mniNewQuoteDD";
            this.mniNewQuoteDD.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.Q)));
            this.mniNewQuoteDD.Size = new System.Drawing.Size(289, 22);
            this.mniNewQuoteDD.Text = "New Quote Window";
            this.mniNewQuoteDD.Click += new System.EventHandler(this.mniNewQuoteDD_Click);
            // 
            // sepNew2
            // 
            this.sepNew2.Name = "sepNew2";
            this.sepNew2.Size = new System.Drawing.Size(286, 6);
            // 
            // mniNewDataSetTB
            // 
            this.mniNewDataSetTB.Image = ((System.Drawing.Image)(resources.GetObject("mniNewDataSetTB.Image")));
            this.mniNewDataSetTB.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mniNewDataSetTB.Name = "mniNewDataSetTB";
            this.mniNewDataSetTB.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.D)));
            this.mniNewDataSetTB.Size = new System.Drawing.Size(289, 22);
            this.mniNewDataSetTB.Text = "New DataSet ...";
            this.mniNewDataSetTB.Click += new System.EventHandler(this.linkNewDataSet_Click);
            // 
            // btnOpenStrategy
            // 
            this.btnOpenStrategy.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenStrategy.Image")));
            this.btnOpenStrategy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenStrategy.Name = "btnOpenStrategy";
            this.btnOpenStrategy.Size = new System.Drawing.Size(98, 25);
            this.btnOpenStrategy.Tag = "*";
            this.btnOpenStrategy.Text = "Open Strategy";
            this.btnOpenStrategy.ToolTipText = "Open an existing Strategy";
            this.btnOpenStrategy.Click += new System.EventHandler(this.btnOpenStrategy_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(51, 25);
            this.btnSave.Tag = "S";
            this.btnSave.Text = "Save";
            this.btnSave.ToolTipText = "Save Strategy";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
            this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(66, 25);
            this.btnSaveAs.Tag = "CS";
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.ToolTipText = "Save Strategy As";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // sepChartFile
            // 
            this.sepChartFile.Name = "sepChartFile";
            this.sepChartFile.Size = new System.Drawing.Size(6, 28);
            this.sepChartFile.Tag = "*";
            // 
            // btnLogin
            // 
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(105, 25);
            this.btnLogin.Tag = "*";
            this.btnLogin.Text = "Log in to Fidelity";
            this.btnLogin.Visible = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLoginTradeTicket_Click);
            // 
            // sepSpacing
            // 
            this.sepSpacing.Name = "sepSpacing";
            this.sepSpacing.Size = new System.Drawing.Size(6, 28);
            this.sepSpacing.Tag = "*";
            // 
            // lblSpacing
            // 
            this.lblSpacing.Name = "lblSpacing";
            this.lblSpacing.Size = new System.Drawing.Size(48, 25);
            this.lblSpacing.Tag = "CS";
            this.lblSpacing.Text = "Spacing:";
            // 
            // btnIncreaseSpacing
            // 
            this.btnIncreaseSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnIncreaseSpacing.Image = ((System.Drawing.Image)(resources.GetObject("btnIncreaseSpacing.Image")));
            this.btnIncreaseSpacing.ImageTransparentColor = System.Drawing.Color.Silver;
            this.btnIncreaseSpacing.Name = "btnIncreaseSpacing";
            this.btnIncreaseSpacing.Size = new System.Drawing.Size(23, 25);
            this.btnIncreaseSpacing.Tag = "CS";
            this.btnIncreaseSpacing.Text = "Increase Bar Spacing";
            this.btnIncreaseSpacing.Click += new System.EventHandler(this.btnIncreaseSpacing_Click);
            // 
            // btnRestoreSpacing
            // 
            this.btnRestoreSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRestoreSpacing.Image = ((System.Drawing.Image)(resources.GetObject("btnRestoreSpacing.Image")));
            this.btnRestoreSpacing.ImageTransparentColor = System.Drawing.Color.Silver;
            this.btnRestoreSpacing.Name = "btnRestoreSpacing";
            this.btnRestoreSpacing.Size = new System.Drawing.Size(23, 25);
            this.btnRestoreSpacing.Tag = "CS";
            this.btnRestoreSpacing.Text = "toolStripButton2";
            this.btnRestoreSpacing.ToolTipText = "Restore Bar Spacing";
            this.btnRestoreSpacing.Click += new System.EventHandler(this.btnRestoreSpacing_Click);
            // 
            // btnDecreaseSpacing
            // 
            this.btnDecreaseSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDecreaseSpacing.Image = ((System.Drawing.Image)(resources.GetObject("btnDecreaseSpacing.Image")));
            this.btnDecreaseSpacing.ImageTransparentColor = System.Drawing.Color.Silver;
            this.btnDecreaseSpacing.Name = "btnDecreaseSpacing";
            this.btnDecreaseSpacing.Size = new System.Drawing.Size(23, 25);
            this.btnDecreaseSpacing.Tag = "CS";
            this.btnDecreaseSpacing.Text = "Decrease Bar Spacing";
            this.btnDecreaseSpacing.Click += new System.EventHandler(this.btnDecreaseSpacing_Click);
            // 
            // sepBarSpacing
            // 
            this.sepBarSpacing.Name = "sepBarSpacing";
            this.sepBarSpacing.Size = new System.Drawing.Size(6, 28);
            this.sepBarSpacing.Tag = "CS";
            // 
            // tslblChartStyles
            // 
            this.tslblChartStyles.Name = "tslblChartStyles";
            this.tslblChartStyles.Size = new System.Drawing.Size(65, 25);
            this.tslblChartStyles.Tag = "CS";
            this.tslblChartStyles.Text = "Chart Style:";
            // 
            // btnCandleStyle
            // 
            this.btnCandleStyle.Checked = true;
            this.btnCandleStyle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnCandleStyle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCandleStyle.Image = ((System.Drawing.Image)(resources.GetObject("btnCandleStyle.Image")));
            this.btnCandleStyle.ImageTransparentColor = System.Drawing.Color.Silver;
            this.btnCandleStyle.Name = "btnCandleStyle";
            this.btnCandleStyle.Size = new System.Drawing.Size(23, 25);
            this.btnCandleStyle.Tag = "CS";
            this.btnCandleStyle.Text = "toolStripButton3";
            this.btnCandleStyle.ToolTipText = "Candle Chart Style";
            this.btnCandleStyle.Click += new System.EventHandler(this.chartStyleButtons_Click);
            // 
            // btnBarChart
            // 
            this.btnBarChart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBarChart.Image = ((System.Drawing.Image)(resources.GetObject("btnBarChart.Image")));
            this.btnBarChart.ImageTransparentColor = System.Drawing.Color.Silver;
            this.btnBarChart.Name = "btnBarChart";
            this.btnBarChart.Size = new System.Drawing.Size(23, 25);
            this.btnBarChart.Tag = "CS";
            this.btnBarChart.Text = "toolStripButton2";
            this.btnBarChart.ToolTipText = "Bar Chart Style";
            this.btnBarChart.Click += new System.EventHandler(this.chartStyleButtons_Click);
            // 
            // btnLineChart
            // 
            this.btnLineChart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLineChart.Image = ((System.Drawing.Image)(resources.GetObject("btnLineChart.Image")));
            this.btnLineChart.ImageTransparentColor = System.Drawing.Color.Silver;
            this.btnLineChart.Name = "btnLineChart";
            this.btnLineChart.Size = new System.Drawing.Size(23, 25);
            this.btnLineChart.Tag = "CS";
            this.btnLineChart.Text = "toolStripButton1";
            this.btnLineChart.ToolTipText = "Line Chart Style";
            this.btnLineChart.Click += new System.EventHandler(this.chartStyleButtons_Click);
            // 
            // tsmMoreChartStyles
            // 
            this.tsmMoreChartStyles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmMoreChartStyles.Image = ((System.Drawing.Image)(resources.GetObject("tsmMoreChartStyles.Image")));
            this.tsmMoreChartStyles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmMoreChartStyles.Name = "tsmMoreChartStyles";
            this.tsmMoreChartStyles.Size = new System.Drawing.Size(44, 25);
            this.tsmMoreChartStyles.Tag = "CS";
            this.tsmMoreChartStyles.Text = "More";
            this.tsmMoreChartStyles.ToolTipText = "More Chart Styles";
            // 
            // btnLinear
            // 
            this.btnLinear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLinear.Image = ((System.Drawing.Image)(resources.GetObject("btnLinear.Image")));
            this.btnLinear.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLinear.Name = "btnLinear";
            this.btnLinear.Size = new System.Drawing.Size(23, 25);
            this.btnLinear.Tag = "CS";
            this.btnLinear.Text = "toolStripButton1";
            this.btnLinear.ToolTipText = "Linear Axis Scale Chart";
            this.btnLinear.Click += new System.EventHandler(this.btnLinear_Click);
            // 
            // btnLog
            // 
            this.btnLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLog.Image = ((System.Drawing.Image)(resources.GetObject("btnLog.Image")));
            this.btnLog.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(23, 25);
            this.btnLog.Tag = "CS";
            this.btnLog.Text = "toolStripButton2";
            this.btnLog.ToolTipText = "Semi-Log Axis Scale Chart";
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // sepChartStyles
            // 
            this.sepChartStyles.Name = "sepChartStyles";
            this.sepChartStyles.Size = new System.Drawing.Size(6, 28);
            this.sepChartStyles.Tag = "CS";
            // 
            // tslblOptions
            // 
            this.tslblOptions.Name = "tslblOptions";
            this.tslblOptions.Size = new System.Drawing.Size(37, 25);
            this.tslblOptions.Tag = "CS";
            this.tslblOptions.Text = "Show:";
            // 
            // btnLabelsVisible
            // 
            this.btnLabelsVisible.Checked = true;
            this.btnLabelsVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnLabelsVisible.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLabelsVisible.Image = ((System.Drawing.Image)(resources.GetObject("btnLabelsVisible.Image")));
            this.btnLabelsVisible.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.btnLabelsVisible.Name = "btnLabelsVisible";
            this.btnLabelsVisible.Size = new System.Drawing.Size(23, 25);
            this.btnLabelsVisible.Tag = "CS";
            this.btnLabelsVisible.Text = "toolStripButton1";
            this.btnLabelsVisible.ToolTipText = "Show Indicator Labels on Chart";
            this.btnLabelsVisible.Click += new System.EventHandler(this.btnLabelsVisible_Click);
            // 
            // btnStatusBarVisible
            // 
            this.btnStatusBarVisible.Checked = true;
            this.btnStatusBarVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnStatusBarVisible.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStatusBarVisible.Image = ((System.Drawing.Image)(resources.GetObject("btnStatusBarVisible.Image")));
            this.btnStatusBarVisible.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStatusBarVisible.Name = "btnStatusBarVisible";
            this.btnStatusBarVisible.Size = new System.Drawing.Size(23, 25);
            this.btnStatusBarVisible.Tag = "CS";
            this.btnStatusBarVisible.Text = "toolStripButton1";
            this.btnStatusBarVisible.ToolTipText = "Show Status Bars on Chart";
            this.btnStatusBarVisible.Click += new System.EventHandler(this.btnStatusBarVisible_Click);
            // 
            // btnFundamentalsVisible
            // 
            this.btnFundamentalsVisible.Checked = true;
            this.btnFundamentalsVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnFundamentalsVisible.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFundamentalsVisible.Image = ((System.Drawing.Image)(resources.GetObject("btnFundamentalsVisible.Image")));
            this.btnFundamentalsVisible.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFundamentalsVisible.Name = "btnFundamentalsVisible";
            this.btnFundamentalsVisible.Size = new System.Drawing.Size(23, 25);
            this.btnFundamentalsVisible.Tag = "CS";
            this.btnFundamentalsVisible.Text = "toolStripButton1";
            this.btnFundamentalsVisible.ToolTipText = "Show selected Fundamental Data Items on Chart";
            this.btnFundamentalsVisible.Click += new System.EventHandler(this.btnFundamentalsVisible_Click);
            // 
            // btnDataWindow
            // 
            this.btnDataWindow.CheckOnClick = true;
            this.btnDataWindow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDataWindow.Image = ((System.Drawing.Image)(resources.GetObject("btnDataWindow.Image")));
            this.btnDataWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDataWindow.Name = "btnDataWindow";
            this.btnDataWindow.Size = new System.Drawing.Size(23, 25);
            this.btnDataWindow.Tag = "CS";
            this.btnDataWindow.Text = "Data Window";
            this.btnDataWindow.Click += new System.EventHandler(this.btnDataWindow_Click);
            // 
            // btnIndicatorsTB2
            // 
            this.btnIndicatorsTB2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnIndicatorsTB2.Image = ((System.Drawing.Image)(resources.GetObject("btnIndicatorsTB2.Image")));
            this.btnIndicatorsTB2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnIndicatorsTB2.Name = "btnIndicatorsTB2";
            this.btnIndicatorsTB2.Size = new System.Drawing.Size(23, 25);
            this.btnIndicatorsTB2.Tag = "CS";
            this.btnIndicatorsTB2.Text = "toolStripButton1";
            this.btnIndicatorsTB2.ToolTipText = "Plot Technical Indicators";
            this.btnIndicatorsTB2.Click += new System.EventHandler(this.btnIndicatorsTB2_Click);
            // 
            // btnFundamentalsTB2
            // 
            this.btnFundamentalsTB2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFundamentalsTB2.Image = ((System.Drawing.Image)(resources.GetObject("btnFundamentalsTB2.Image")));
            this.btnFundamentalsTB2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFundamentalsTB2.Name = "btnFundamentalsTB2";
            this.btnFundamentalsTB2.Size = new System.Drawing.Size(23, 25);
            this.btnFundamentalsTB2.Tag = "CS";
            this.btnFundamentalsTB2.Text = "toolStripButton2";
            this.btnFundamentalsTB2.ToolTipText = "Plot Fundamental Data Items";
            this.btnFundamentalsTB2.Click += new System.EventHandler(this.btnFundamentalsTB2_Click);
            this.btnFundamentalsTB2.VisibleChanged += new System.EventHandler(this.btnFundamentalsTB2_VisibleChanged);
            // 
            // btnClearIndicators
            // 
            this.btnClearIndicators.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearIndicators.Image = ((System.Drawing.Image)(resources.GetObject("btnClearIndicators.Image")));
            this.btnClearIndicators.ImageTransparentColor = System.Drawing.Color.Silver;
            this.btnClearIndicators.Name = "btnClearIndicators";
            this.btnClearIndicators.Size = new System.Drawing.Size(23, 25);
            this.btnClearIndicators.Tag = "CS";
            this.btnClearIndicators.Text = "toolStripButton2";
            this.btnClearIndicators.ToolTipText = "Clear Drag and Drop Indicators";
            this.btnClearIndicators.Click += new System.EventHandler(this.btnClearIndicators_Click);
            // 
            // btnPushCode
            // 
            this.btnPushCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPushCode.Image = ((System.Drawing.Image)(resources.GetObject("btnPushCode.Image")));
            this.btnPushCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPushCode.Name = "btnPushCode";
            this.btnPushCode.Size = new System.Drawing.Size(23, 25);
            this.btnPushCode.Tag = "CS";
            this.btnPushCode.Text = "toolStripButton1";
            this.btnPushCode.ToolTipText = "Push all Indicators and Fundamental Items into the Strategy Code";
            this.btnPushCode.Click += new System.EventHandler(this.btnPushCode_Click);
            // 
            // btnTradeTicket
            // 
            this.btnTradeTicket.Checked = true;
            this.btnTradeTicket.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnTradeTicket.Image = ((System.Drawing.Image)(resources.GetObject("btnTradeTicket.Image")));
            this.btnTradeTicket.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTradeTicket.Name = "btnTradeTicket";
            this.btnTradeTicket.Size = new System.Drawing.Size(110, 25);
            this.btnTradeTicket.Tag = "G";
            this.btnTradeTicket.Text = "Hide Trade Ticket";
            this.btnTradeTicket.ToolTipText = "Show/Hide the Trade Ticket";
            this.btnTradeTicket.Visible = false;
            this.btnTradeTicket.CheckStateChanged += new System.EventHandler(this.btnTradeTicket_CheckStateChanged);
            this.btnTradeTicket.Click += new System.EventHandler(this.btnTradeTicket_Click);
            // 
            // btnPreferencesTB
            // 
            this.btnPreferencesTB.Image = ((System.Drawing.Image)(resources.GetObject("btnPreferencesTB.Image")));
            this.btnPreferencesTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreferencesTB.Name = "btnPreferencesTB";
            this.btnPreferencesTB.Size = new System.Drawing.Size(85, 25);
            this.btnPreferencesTB.Tag = "G";
            this.btnPreferencesTB.Text = "Preferences";
            this.btnPreferencesTB.Click += new System.EventHandler(this.btnPreferencesTB_Click);
            // 
            // pnlTree
            // 
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
            this.pnlTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTree.Location = new System.Drawing.Point(0, 77);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Size = new System.Drawing.Size(206, 381);
            this.pnlTree.TabIndex = 4;
            // 
            // splitContainerDataPane
            // 
            this.splitContainerDataPane.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerDataPane.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerDataPane.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerDataPane.Location = new System.Drawing.Point(4, 102);
            this.splitContainerDataPane.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.splitContainerDataPane.Name = "splitContainerDataPane";
            this.splitContainerDataPane.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerDataPane.Panel1
            // 
            this.splitContainerDataPane.Panel1.Controls.Add(this.treeDataSources);
            this.splitContainerDataPane.Panel1.Controls.Add(this.toolbarDataSets);
            this.splitContainerDataPane.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainerDataPane.Panel1MinSize = 100;
            // 
            // splitContainerDataPane.Panel2
            // 
            this.splitContainerDataPane.Panel2.Controls.Add(this.toolbarParameters);
            this.splitContainerDataPane.Panel2.Controls.Add(this.pnlParamBase);
            this.splitContainerDataPane.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainerDataPane.Panel2MinSize = 100;
            this.splitContainerDataPane.Size = new System.Drawing.Size(199, 279);
            this.splitContainerDataPane.SplitterDistance = 140;
            this.splitContainerDataPane.TabIndex = 13;
            this.splitContainerDataPane.TabStop = false;
            // 
            // treeDataSources
            // 
            this.treeDataSources.AllowDrop = true;
            this.treeDataSources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeDataSources.HideSelection = false;
            this.treeDataSources.ImageIndex = 0;
            this.treeDataSources.Location = new System.Drawing.Point(0, 20);
            this.treeDataSources.Name = "treeDataSources";
            this.treeDataSources.SelectedImageIndex = 0;
            this.treeDataSources.Size = new System.Drawing.Size(199, 120);
            this.treeDataSources.TabIndex = 8;
            this.treeDataSources.DataManagerClicked += new System.EventHandler<System.EventArgs>(this.btnDataManager_Click);
            this.treeDataSources.DataSourceSelected += new System.EventHandler<WealthLab.DataSourceEventArgs>(this.dataSourceSelectedEventHandler);
            this.treeDataSources.DataSourceTreeViewRenameClicked += new System.EventHandler<System.EventArgs>(this.dataSourceTreeViewRenameClickedEventHandler);
            this.treeDataSources.IndexManagerClicked += new System.EventHandler<System.EventArgs>(this.indexManagerClickedEventHandler);
            this.treeDataSources.NewDataSourceClicked += new System.EventHandler<System.EventArgs>(this.linkNewDataSet_Click);
            this.treeDataSources.SymbolSelected += new System.EventHandler<WealthLab.DataSourceSymbolEventArgs>(this.symbolSelectionChangeEventHandler);
            this.treeDataSources.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeDataSources_ItemDrag);
            this.treeDataSources.DragOver += new System.Windows.Forms.DragEventHandler(this.treeDataSources_DragOver);
            this.treeDataSources.DoubleClick += new System.EventHandler(this.treeDataSources_DoubleClick);
            this.treeDataSources.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeDataSources_MouseDown);
            // 
            // toolbarDataSets
            // 
            this.toolbarDataSets.AutoSize = false;
            this.toolbarDataSets.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.toolbarDataSets.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarDataSets.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblDataSets,
            this.linkNewDataSet});
            this.toolbarDataSets.Location = new System.Drawing.Point(0, 0);
            this.toolbarDataSets.Name = "toolbarDataSets";
            this.toolbarDataSets.Size = new System.Drawing.Size(199, 19);
            this.toolbarDataSets.TabIndex = 7;
            this.toolbarDataSets.Text = "toolStrip1";
            // 
            // lblDataSets
            // 
            this.lblDataSets.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDataSets.Name = "lblDataSets";
            this.lblDataSets.Size = new System.Drawing.Size(59, 16);
            this.lblDataSets.Text = "DataSets";
            // 
            // linkNewDataSet
            // 
            this.linkNewDataSet.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.linkNewDataSet.IsLink = true;
            this.linkNewDataSet.Name = "linkNewDataSet";
            this.linkNewDataSet.Size = new System.Drawing.Size(43, 16);
            this.linkNewDataSet.Text = "New ...";
            this.linkNewDataSet.Click += new System.EventHandler(this.linkNewDataSet_Click);
            // 
            // toolbarParameters
            // 
            this.toolbarParameters.AutoSize = false;
            this.toolbarParameters.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarParameters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblParameters});
            this.toolbarParameters.Location = new System.Drawing.Point(0, 0);
            this.toolbarParameters.Name = "toolbarParameters";
            this.toolbarParameters.Size = new System.Drawing.Size(199, 22);
            this.toolbarParameters.TabIndex = 9;
            this.toolbarParameters.Text = "Strategy Parameters";
            // 
            // lblParameters
            // 
            this.lblParameters.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblParameters.Name = "lblParameters";
            this.lblParameters.Size = new System.Drawing.Size(127, 19);
            this.lblParameters.Text = "Strategy Parameters";
            // 
            // pnlParamBase
            // 
            this.pnlParamBase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlParamBase.BackColor = System.Drawing.SystemColors.Window;
            this.pnlParamBase.Controls.Add(this.pnlParamBaseLinkParams);
            this.pnlParamBase.Controls.Add(this.paramSliders);
            this.pnlParamBase.Location = new System.Drawing.Point(0, 22);
            this.pnlParamBase.Name = "pnlParamBase";
            this.pnlParamBase.Size = new System.Drawing.Size(199, 113);
            this.pnlParamBase.TabIndex = 10;
            this.pnlParamBase.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlParamBase_Paint);
            // 
            // pnlParamBaseLinkParams
            // 
            this.pnlParamBaseLinkParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlParamBaseLinkParams.BackColor = System.Drawing.SystemColors.Window;
            this.pnlParamBaseLinkParams.Controls.Add(this.linkRerun);
            this.pnlParamBaseLinkParams.Controls.Add(this.linkResetParams);
            this.pnlParamBaseLinkParams.Controls.Add(this.linkSaveParams);
            this.pnlParamBaseLinkParams.Location = new System.Drawing.Point(3, 91);
            this.pnlParamBaseLinkParams.Name = "pnlParamBaseLinkParams";
            this.pnlParamBaseLinkParams.Size = new System.Drawing.Size(193, 22);
            this.pnlParamBaseLinkParams.TabIndex = 21;
            // 
            // linkRerun
            // 
            this.linkRerun.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkRerun.Location = new System.Drawing.Point(3, 4);
            this.linkRerun.Name = "linkRerun";
            this.linkRerun.Size = new System.Drawing.Size(84, 13);
            this.linkRerun.TabIndex = 3;
            this.linkRerun.TabStop = true;
            this.linkRerun.Text = "Re-run Backtest";
            this.linkRerun.Visible = false;
            this.linkRerun.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRerun_LinkClicked);
            // 
            // linkResetParams
            // 
            this.linkResetParams.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkResetParams.Location = new System.Drawing.Point(93, 4);
            this.linkResetParams.Name = "linkResetParams";
            this.linkResetParams.Size = new System.Drawing.Size(35, 13);
            this.linkResetParams.TabIndex = 1;
            this.linkResetParams.TabStop = true;
            this.linkResetParams.Text = "Reset";
            this.linkResetParams.Visible = false;
            this.linkResetParams.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkResetParams_LinkClicked);
            // 
            // linkSaveParams
            // 
            this.linkSaveParams.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkSaveParams.Location = new System.Drawing.Point(3, 3);
            this.linkSaveParams.Name = "linkSaveParams";
            this.linkSaveParams.Size = new System.Drawing.Size(89, 13);
            this.linkSaveParams.TabIndex = 0;
            this.linkSaveParams.TabStop = true;
            this.linkSaveParams.Text = "Save Parameters";
            this.linkSaveParams.Visible = false;
            this.linkSaveParams.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSaveParams_LinkClicked);
            // 
            // paramSliders
            // 
            this.paramSliders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.paramSliders.ContextMenuStrip = this.popupPreferredValues;
            this.paramSliders.Location = new System.Drawing.Point(3, 1);
            this.paramSliders.Name = "paramSliders";
            this.paramSliders.Size = new System.Drawing.Size(193, 87);
            this.paramSliders.TabIndex = 2;
            this.paramSliders.WealthScript = null;
            this.paramSliders.SliderMouseDown += new System.EventHandler<System.EventArgs>(this.sliderMouseDownEventHandler);
            this.paramSliders.SliderValueChanged += new System.EventHandler<System.EventArgs>(this.sliderValueChangedEventHandler);
            // 
            // popupPreferredValues
            // 
            this.popupPreferredValues.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniStorePreferredValues});
            this.popupPreferredValues.Name = "popupPreferredValues";
            this.popupPreferredValues.Size = new System.Drawing.Size(420, 26);
            // 
            // mniStorePreferredValues
            // 
            this.mniStorePreferredValues.Name = "mniStorePreferredValues";
            this.mniStorePreferredValues.Size = new System.Drawing.Size(419, 22);
            this.mniStorePreferredValues.Text = "Store these Parameter Values as the Preferred Values for the Symbol(s)";
            this.mniStorePreferredValues.Click += new System.EventHandler(this.mniStorePreferredValues_Click);
            // 
            // lblScale
            // 
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new System.Drawing.Point(3, 4);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(37, 13);
            this.lblScale.TabIndex = 12;
            this.lblScale.Text = "Scale:";
            // 
            // scale
            // 
            this.scale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scale.BackColor = System.Drawing.Color.Cornsilk;
            this.scale.Location = new System.Drawing.Point(79, 4);
            this.scale.Name = "scale";
            this.scale.Size = new System.Drawing.Size(124, 20);
            this.scale.SM = false;
            this.scale.TabIndex = 11;
            this.scale.ScaleChanged += new System.EventHandler<System.EventArgs>(this.scaleChangedEventHandler);
            // 
            // cmbSymbol
            // 
            this.cmbSymbol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSymbol.FormattingEnabled = true;
            this.cmbSymbol.Location = new System.Drawing.Point(57, 75);
            this.cmbSymbol.Name = "cmbSymbol";
            this.cmbSymbol.Size = new System.Drawing.Size(107, 21);
            this.cmbSymbol.Sorted = true;
            this.cmbSymbol.TabIndex = 5;
            this.cmbSymbol.DropDownClosed += new System.EventHandler(this.cmbSymbol_DropDownClosed);
            this.cmbSymbol.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbSymbol_KeyPress);
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Enabled = false;
            this.btnGo.Location = new System.Drawing.Point(169, 75);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(34, 22);
            this.btnGo.TabIndex = 6;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lblSymbol
            // 
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbol.Location = new System.Drawing.Point(3, 78);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(51, 13);
            this.lblSymbol.TabIndex = 4;
            this.lblSymbol.Text = "Symbol:";
            // 
            // lblPositions
            // 
            this.lblPositions.AutoSize = true;
            this.lblPositions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPositions.Location = new System.Drawing.Point(3, 49);
            this.lblPositions.Name = "lblPositions";
            this.lblPositions.Size = new System.Drawing.Size(70, 13);
            this.lblPositions.TabIndex = 2;
            this.lblPositions.Text = "Position Size:";
            // 
            // lblRange
            // 
            this.lblRange.AutoSize = true;
            this.lblRange.Location = new System.Drawing.Point(3, 26);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(68, 13);
            this.lblRange.TabIndex = 0;
            this.lblRange.Text = "Data Range:";
            // 
            // posSize
            // 
            this.posSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.posSize.BackColor = System.Drawing.Color.Honeydew;
            this.posSize.CombinationStrategyChildMode = false;
            this.posSize.Location = new System.Drawing.Point(79, 48);
            this.posSize.Name = "posSize";
            this.posSize.Size = new System.Drawing.Size(124, 20);
            this.posSize.TabIndex = 3;
            this.posSize.PositionSizeChanged += new System.EventHandler<System.EventArgs>(this.positionSizeChangedEventHandler);
            // 
            // dataRange
            // 
            this.dataRange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataRange.BackColor = System.Drawing.Color.AliceBlue;
            this.dataRange.IsStreaming = false;
            this.dataRange.Location = new System.Drawing.Point(79, 26);
            this.dataRange.Name = "dataRange";
            this.dataRange.Size = new System.Drawing.Size(124, 20);
            this.dataRange.TabIndex = 1;
            this.dataRange.DataRangeChanged += new System.EventHandler<System.EventArgs>(this.dataRangeChangedEventHandler);
            // 
            // toolbarDrawing
            // 
            this.toolbarDrawing.AllowDrop = true;
            this.toolbarDrawing.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClearDrawingObjects,
            this.btnCrossHair,
            this.sepDeleteDrawing});
            this.toolbarDrawing.Location = new System.Drawing.Point(206, 77);
            this.toolbarDrawing.Name = "toolbarDrawing";
            this.toolbarDrawing.Size = new System.Drawing.Size(822, 25);
            this.toolbarDrawing.TabIndex = 17;
            this.toolbarDrawing.Text = "toolStrip1";
            // 
            // btnClearDrawingObjects
            // 
            this.btnClearDrawingObjects.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearDrawingObjects.Image = ((System.Drawing.Image)(resources.GetObject("btnClearDrawingObjects.Image")));
            this.btnClearDrawingObjects.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearDrawingObjects.Name = "btnClearDrawingObjects";
            this.btnClearDrawingObjects.Size = new System.Drawing.Size(23, 22);
            this.btnClearDrawingObjects.Text = "toolStripButton1";
            this.btnClearDrawingObjects.ToolTipText = "Clear Drawing Objects";
            this.btnClearDrawingObjects.Click += new System.EventHandler(this.btnClearDrawingObjects_Click);
            // 
            // btnCrossHair
            // 
            this.btnCrossHair.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCrossHair.Image = ((System.Drawing.Image)(resources.GetObject("btnCrossHair.Image")));
            this.btnCrossHair.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCrossHair.Name = "btnCrossHair";
            this.btnCrossHair.Size = new System.Drawing.Size(23, 22);
            this.btnCrossHair.Text = "Cross Hair";
            this.btnCrossHair.Click += new System.EventHandler(this.btnCrossHair_Click);
            // 
            // sepDeleteDrawing
            // 
            this.sepDeleteDrawing.Name = "sepDeleteDrawing";
            this.sepDeleteDrawing.Size = new System.Drawing.Size(6, 25);
            // 
            // splitter
            // 
            this.splitter.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter.Location = new System.Drawing.Point(206, 102);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 356);
            this.splitter.TabIndex = 5;
            this.splitter.TabStop = false;
            this.splitter.DoubleClick += new System.EventHandler(this.mniViewDataPanel_Click);
            // 
            // saveFileDialog_0
            // 
            this.saveFileDialog_0.DefaultExt = "ws";
            this.saveFileDialog_0.Filter = "Workspace files|*.ws";
            this.saveFileDialog_0.RestoreDirectory = true;
            this.saveFileDialog_0.Title = "Save Workspace";
            // 
            // openFileDialog_0
            // 
            this.openFileDialog_0.DefaultExt = "ws";
            this.openFileDialog_0.Filter = "Workspace files|*.ws";
            this.openFileDialog_0.RestoreDirectory = true;
            this.openFileDialog_0.Title = "Open Workspace";
            // 
            // timer_0
            // 
            this.timer_0.Interval = 333;
            this.timer_0.Tick += new System.EventHandler(this.timer_0_Tick);
            // 
            // timer_1
            // 
            this.timer_1.Tick += new System.EventHandler(this.timer_1_Tick);
            // 
            // pnlTrade
            // 
            this.pnlTrade.BackColor = System.Drawing.Color.LightSlateGray;
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
            this.pnlTrade.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTrade.Location = new System.Drawing.Point(209, 102);
            this.pnlTrade.Name = "pnlTrade";
            this.pnlTrade.Size = new System.Drawing.Size(819, 121);
            this.pnlTrade.TabIndex = 19;
            // 
            // accountTypeSelector1
            // 
            this.accountTypeSelector1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.accountTypeSelector1.FormattingEnabled = true;
            this.accountTypeSelector1.IgnoreCalls = false;
            this.accountTypeSelector1.Location = new System.Drawing.Point(542, 11);
            this.accountTypeSelector1.Name = "accountTypeSelector1";
            this.accountTypeSelector1.Size = new System.Drawing.Size(59, 21);
            this.accountTypeSelector1.TabIndex = 47;
            // 
            // lblBid
            // 
            this.lblBid.Location = new System.Drawing.Point(664, 14);
            this.lblBid.Name = "lblBid";
            this.lblBid.Size = new System.Drawing.Size(117, 11);
            this.lblBid.TabIndex = 43;
            this.lblBid.Text = "Bid:";
            this.lblBid.Visible = false;
            // 
            // lblAsOf
            // 
            this.lblAsOf.Location = new System.Drawing.Point(664, 36);
            this.lblAsOf.Name = "lblAsOf";
            this.lblAsOf.Size = new System.Drawing.Size(162, 11);
            this.lblAsOf.TabIndex = 44;
            this.lblAsOf.Text = "As of ";
            this.lblAsOf.Visible = false;
            // 
            // lblLast
            // 
            this.lblLast.Location = new System.Drawing.Point(664, 3);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(117, 11);
            this.lblLast.TabIndex = 41;
            this.lblLast.Text = "Last:";
            this.lblLast.Visible = false;
            // 
            // lblAcctType
            // 
            this.lblAcctType.AutoSize = true;
            this.lblAcctType.ForeColor = System.Drawing.Color.White;
            this.lblAcctType.Location = new System.Drawing.Point(542, 35);
            this.lblAcctType.Name = "lblAcctType";
            this.lblAcctType.Size = new System.Drawing.Size(62, 13);
            this.lblAcctType.TabIndex = 46;
            this.lblAcctType.Text = "Trade Type";
            // 
            // btnDockUp
            // 
            this.btnDockUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDockUp.FlatAppearance.BorderSize = 0;
            this.btnDockUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDockUp.Image = ((System.Drawing.Image)(resources.GetObject("btnDockUp.Image")));
            this.btnDockUp.Location = new System.Drawing.Point(807, 109);
            this.btnDockUp.Name = "btnDockUp";
            this.btnDockUp.Size = new System.Drawing.Size(12, 12);
            this.btnDockUp.TabIndex = 40;
            this.btnDockUp.UseVisualStyleBackColor = true;
            this.btnDockUp.Visible = false;
            this.btnDockUp.Click += new System.EventHandler(this.btnDockUp_Click);
            // 
            // btnCloseTradeTicket
            // 
            this.btnCloseTradeTicket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseTradeTicket.FlatAppearance.BorderSize = 0;
            this.btnCloseTradeTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseTradeTicket.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseTradeTicket.Image")));
            this.btnCloseTradeTicket.Location = new System.Drawing.Point(806, 0);
            this.btnCloseTradeTicket.Name = "btnCloseTradeTicket";
            this.btnCloseTradeTicket.Size = new System.Drawing.Size(12, 12);
            this.btnCloseTradeTicket.TabIndex = 37;
            this.btnCloseTradeTicket.UseVisualStyleBackColor = true;
            this.btnCloseTradeTicket.Click += new System.EventHandler(this.btnCloseTradeTicket_Click);
            // 
            // btnDockDown
            // 
            this.btnDockDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDockDown.FlatAppearance.BorderSize = 0;
            this.btnDockDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDockDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDockDown.Image")));
            this.btnDockDown.Location = new System.Drawing.Point(806, 109);
            this.btnDockDown.Name = "btnDockDown";
            this.btnDockDown.Size = new System.Drawing.Size(12, 12);
            this.btnDockDown.TabIndex = 39;
            this.btnDockDown.UseVisualStyleBackColor = true;
            this.btnDockDown.Click += new System.EventHandler(this.btnDockDown_Click);
            // 
            // btnLoginTradeTicket
            // 
            this.btnLoginTradeTicket.Location = new System.Drawing.Point(604, 10);
            this.btnLoginTradeTicket.Name = "btnLoginTradeTicket";
            this.btnLoginTradeTicket.Size = new System.Drawing.Size(98, 23);
            this.btnLoginTradeTicket.TabIndex = 38;
            this.btnLoginTradeTicket.Text = "Log in";
            this.btnLoginTradeTicket.UseVisualStyleBackColor = true;
            this.btnLoginTradeTicket.Click += new System.EventHandler(this.btnLoginTradeTicket_Click);
            // 
            // numTradePrice
            // 
            this.numTradePrice.InputType = CtrlLib.NumEdit.NumEditType.Double;
            this.numTradePrice.Location = new System.Drawing.Point(365, 11);
            this.numTradePrice.Name = "numTradePrice";
            this.numTradePrice.Size = new System.Drawing.Size(64, 20);
            this.numTradePrice.TabIndex = 5;
            this.numTradePrice.TextChanged += new System.EventHandler(this.cmbTradeTIF_SelectedIndexChanged);
            // 
            // numTradeQty
            // 
            this.numTradeQty.InputType = CtrlLib.NumEdit.NumEditType.Integer;
            this.numTradeQty.Location = new System.Drawing.Point(212, 11);
            this.numTradeQty.Name = "numTradeQty";
            this.numTradeQty.Size = new System.Drawing.Size(57, 20);
            this.numTradeQty.TabIndex = 3;
            this.numTradeQty.Text = "100";
            this.numTradeQty.TextChanged += new System.EventHandler(this.cmbTradeTIF_SelectedIndexChanged);
            // 
            // txtTradeSymbol
            // 
            this.txtTradeSymbol.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTradeSymbol.Location = new System.Drawing.Point(105, 11);
            this.txtTradeSymbol.Name = "txtTradeSymbol";
            this.txtTradeSymbol.Size = new System.Drawing.Size(55, 20);
            this.txtTradeSymbol.TabIndex = 1;
            this.txtTradeSymbol.TextChanged += new System.EventHandler(this.cmbTradeTIF_SelectedIndexChanged);
            this.txtTradeSymbol.Leave += new System.EventHandler(this.txtTradeSymbol_Leave);
            // 
            // btnStageOrder
            // 
            this.btnStageOrder.Enabled = false;
            this.btnStageOrder.Location = new System.Drawing.Point(604, 28);
            this.btnStageOrder.Name = "btnStageOrder";
            this.btnStageOrder.Size = new System.Drawing.Size(56, 21);
            this.btnStageOrder.TabIndex = 10;
            this.btnStageOrder.Text = "Stage";
            this.btnStageOrder.UseVisualStyleBackColor = true;
            this.btnStageOrder.Visible = false;
            this.btnStageOrder.Click += new System.EventHandler(this.btnStageOrder_Click);
            // 
            // btnPlaceOrder
            // 
            this.btnPlaceOrder.Enabled = false;
            this.btnPlaceOrder.Location = new System.Drawing.Point(604, 3);
            this.btnPlaceOrder.Name = "btnPlaceOrder";
            this.btnPlaceOrder.Size = new System.Drawing.Size(56, 21);
            this.btnPlaceOrder.TabIndex = 8;
            this.btnPlaceOrder.Text = "Place";
            this.btnPlaceOrder.UseVisualStyleBackColor = true;
            this.btnPlaceOrder.Visible = false;
            this.btnPlaceOrder.Click += new System.EventHandler(this.btnPlaceOrder_Click);
            // 
            // lbTradeDirected
            // 
            this.lbTradeDirected.AutoSize = true;
            this.lbTradeDirected.ForeColor = System.Drawing.Color.White;
            this.lbTradeDirected.Location = new System.Drawing.Point(429, 35);
            this.lbTradeDirected.Name = "lbTradeDirected";
            this.lbTradeDirected.Size = new System.Drawing.Size(36, 13);
            this.lbTradeDirected.TabIndex = 36;
            this.lbTradeDirected.Text = "Route";
            // 
            // cmbTradeRoute
            // 
            this.cmbTradeRoute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTradeRoute.FormattingEnabled = true;
            this.cmbTradeRoute.Location = new System.Drawing.Point(429, 11);
            this.cmbTradeRoute.Name = "cmbTradeRoute";
            this.cmbTradeRoute.Size = new System.Drawing.Size(57, 21);
            this.cmbTradeRoute.TabIndex = 6;
            this.cmbTradeRoute.SelectedIndexChanged += new System.EventHandler(this.cmbTradeRoute_SelectedIndexChanged);
            // 
            // lblTradeTIF
            // 
            this.lblTradeTIF.AutoSize = true;
            this.lblTradeTIF.ForeColor = System.Drawing.Color.White;
            this.lblTradeTIF.Location = new System.Drawing.Point(486, 35);
            this.lblTradeTIF.Name = "lblTradeTIF";
            this.lblTradeTIF.Size = new System.Drawing.Size(23, 13);
            this.lblTradeTIF.TabIndex = 35;
            this.lblTradeTIF.Text = "TIF";
            // 
            // cmbTradeTIF
            // 
            this.cmbTradeTIF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTradeTIF.FormattingEnabled = true;
            this.cmbTradeTIF.Location = new System.Drawing.Point(486, 11);
            this.cmbTradeTIF.Name = "cmbTradeTIF";
            this.cmbTradeTIF.Size = new System.Drawing.Size(56, 21);
            this.cmbTradeTIF.TabIndex = 7;
            this.cmbTradeTIF.SelectedIndexChanged += new System.EventHandler(this.cmbTradeTIF_SelectedIndexChanged);
            // 
            // lblTradePrice
            // 
            this.lblTradePrice.AutoSize = true;
            this.lblTradePrice.ForeColor = System.Drawing.Color.White;
            this.lblTradePrice.Location = new System.Drawing.Point(365, 35);
            this.lblTradePrice.Name = "lblTradePrice";
            this.lblTradePrice.Size = new System.Drawing.Size(43, 13);
            this.lblTradePrice.TabIndex = 34;
            this.lblTradePrice.Text = "Amount";
            // 
            // lblTradeOrder
            // 
            this.lblTradeOrder.AutoSize = true;
            this.lblTradeOrder.ForeColor = System.Drawing.Color.White;
            this.lblTradeOrder.Location = new System.Drawing.Point(269, 35);
            this.lblTradeOrder.Name = "lblTradeOrder";
            this.lblTradeOrder.Size = new System.Drawing.Size(60, 13);
            this.lblTradeOrder.TabIndex = 33;
            this.lblTradeOrder.Text = "Order Type";
            // 
            // cmbTradeOrder
            // 
            this.cmbTradeOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTradeOrder.FormattingEnabled = true;
            this.cmbTradeOrder.Location = new System.Drawing.Point(269, 11);
            this.cmbTradeOrder.Name = "cmbTradeOrder";
            this.cmbTradeOrder.Size = new System.Drawing.Size(96, 21);
            this.cmbTradeOrder.TabIndex = 4;
            this.cmbTradeOrder.SelectedIndexChanged += new System.EventHandler(this.cmbTradeOrder_SelectedIndexChanged);
            // 
            // lblTradeQty
            // 
            this.lblTradeQty.AutoSize = true;
            this.lblTradeQty.ForeColor = System.Drawing.Color.White;
            this.lblTradeQty.Location = new System.Drawing.Point(212, 36);
            this.lblTradeQty.Name = "lblTradeQty";
            this.lblTradeQty.Size = new System.Drawing.Size(46, 13);
            this.lblTradeQty.TabIndex = 32;
            this.lblTradeQty.Text = "Quantity";
            // 
            // lblTradeAction
            // 
            this.lblTradeAction.AutoSize = true;
            this.lblTradeAction.ForeColor = System.Drawing.Color.White;
            this.lblTradeAction.Location = new System.Drawing.Point(160, 35);
            this.lblTradeAction.Name = "lblTradeAction";
            this.lblTradeAction.Size = new System.Drawing.Size(37, 13);
            this.lblTradeAction.TabIndex = 31;
            this.lblTradeAction.Text = "Action";
            // 
            // cmbTradeAction
            // 
            this.cmbTradeAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTradeAction.FormattingEnabled = true;
            this.cmbTradeAction.Items.AddRange(new object[] {
            "Buy",
            "Sell",
            "Short",
            "Cover"});
            this.cmbTradeAction.Location = new System.Drawing.Point(160, 11);
            this.cmbTradeAction.Name = "cmbTradeAction";
            this.cmbTradeAction.Size = new System.Drawing.Size(52, 21);
            this.cmbTradeAction.TabIndex = 2;
            this.cmbTradeAction.SelectedIndexChanged += new System.EventHandler(this.cmbTradeAction_SelectedIndexChanged);
            // 
            // lblTradeSymbol
            // 
            this.lblTradeSymbol.AutoSize = true;
            this.lblTradeSymbol.ForeColor = System.Drawing.Color.White;
            this.lblTradeSymbol.Location = new System.Drawing.Point(105, 35);
            this.lblTradeSymbol.Name = "lblTradeSymbol";
            this.lblTradeSymbol.Size = new System.Drawing.Size(41, 13);
            this.lblTradeSymbol.TabIndex = 30;
            this.lblTradeSymbol.Text = "Symbol";
            // 
            // lblTradeAcct
            // 
            this.lblTradeAcct.AutoSize = true;
            this.lblTradeAcct.ForeColor = System.Drawing.Color.White;
            this.lblTradeAcct.Location = new System.Drawing.Point(6, 35);
            this.lblTradeAcct.Name = "lblTradeAcct";
            this.lblTradeAcct.Size = new System.Drawing.Size(47, 13);
            this.lblTradeAcct.TabIndex = 29;
            this.lblTradeAcct.Text = "Account";
            // 
            // cmbAccount
            // 
            this.cmbAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Location = new System.Drawing.Point(6, 11);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new System.Drawing.Size(99, 21);
            this.cmbAccount.TabIndex = 0;
            this.cmbAccount.SelectedIndexChanged += new System.EventHandler(this.cmbAccount_SelectedIndexChanged);
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // timer_2
            // 
            this.timer_2.Enabled = true;
            this.timer_2.Tick += new System.EventHandler(this.timer_2_Tick);
            // 
            // assemblyLoader_0
            // 
            this.assemblyLoader_0.BaseClass = "ChartStyle";
            this.assemblyLoader_0.DLLNameFilter = "";
            this.assemblyLoader_0.Interface = null;
            this.assemblyLoader_0.Path = null;
            this.assemblyLoader_0.PathMask = "*.dll";
            // 
            // drawingObjectManager_0
            // 
            this.drawingObjectManager_0.ChartBookName = "Standard";
            this.drawingObjectManager_0.RootPath = null;
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1028, 480);
            this.Controls.Add(this.pnlTrade);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.toolbarDrawing);
            this.Controls.Add(this.pnlTree);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.toolbarNav);
            this.Controls.Add(this.status);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(20, 20);
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(320, 180);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Application Name";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDataPane)).EndInit();
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // ///WYJ fix 
        /*
        private void splitContainerDataPane_DoubleClick(object sender, EventArgs e)
        {
            splitContainerDataPane.Panel1Collapsed = !splitContainerDataPane.Panel1Collapsed;
        } */

        public void ItemAdded(ChartForm item)
        {
            this.chartFormCount++;
            this.dropdownCharts.Text = "Charts && Strategies (" + this.chartFormCount + ")";
            ToolStripMenuItem item2 = new ToolStripMenuItem(item.Text) {
                Text = "Chart",
                Tag = item
            };
            item2.Click += new EventHandler(this.chartFormMenuItemClickEventHandler);
            item2.Checked = true;
            this.dropdownCharts.DropDownItems.Add(item2);
        }

        public void ItemAdded(QuotesForm item)
        {
            this.quotesFormCount++;
            this.dropdownQuotes.Text = "Quotes (" + this.quotesFormCount + ")";
            ToolStripMenuItem item2 = new ToolStripMenuItem(item.Text) {
                Text = "Quote",
                Tag = item
            };
            item2.Click += new EventHandler(this.quotesFormMenuItemClickEventHandler);
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
            int2.quotesFormCount = int2.quotesFormCount - 1;
            if (this.quotesFormCount != 0)
            {
                this.dropdownQuotes.Text = string.Concat("Quotes (", this.quotesFormCount, ")");
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
            int1.chartFormCount = int1.chartFormCount - 1;
            if (this.chartFormCount != 0)
            {
                this.dropdownCharts.Text = string.Concat("Charts && Strategies (", this.chartFormCount, ")");
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
                    this.enableParamLinkButtons(activeMdiChild.ParametersNeedSave);
                }
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            mainForm_0 = this;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.isFirstMainForm_2)
            {
                MainModule.Instance.Settings.Set(this, "MainForm");
            }
            MainModule.Instance.Settings.Set("DataTreeWidth", this.pnlTree.Width);
            MainModule.Instance.DataSources.UnregisterObserver(this.treeDataSources);
            Interlocked.Decrement(ref mainFormCount);
            this.showOrHideCloseWSMenuItems();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isFirstMainForm_2 && (mainFormCount > 1)) 
            {
                if (!this.confirmExit())
                    e.Cancel = true;
            }
            else if (this.isFirstMainForm_2)
            {
                this.formClosing = true;
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

        ///WYJ fix
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
            Interlocked.Increment(ref mainFormCount);
            this.isFirstMainForm_2 = mainFormCount == 1;
            this.showOrHideCloseWSMenuItems();
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
            if (this.isFirstMainForm_2)
            {
                this.mniViewTradeTicket.Checked = MainModule.Instance.Settings.Get("ShowTradeTicket", true);
                AuthenticationProvider authProvider = MainModule.Instance.AuthProvider;
                authProvider.OnInstallerDownloadComplete += new AuthenticationProvider._DownloadFileCompleted(this.authProviderInstallerDownloadCompleteEventHandler);
                authProvider.OnInstallerDownloadProgressChanged += new AuthenticationProvider._DownloadProgressChanged(this.authProviderInstallerDownloadProgrChangedEventHandler);
                this.btnLogin.Text = authProvider.LoginPhrase;
                this.btnLogin.Visible = authProvider.LoginButtonVisible;
                this.mniSoftwareUpgrade.Visible = authProvider.SupportsSoftwareUpgrade;
                this.sepUpgrade.Visible = authProvider.SupportsSoftwareUpgrade;
            }
            this.btnLogin.Image = MainModule.Instance.AuthProvider.Glyph;
            this.ShowLoggedInState(MainModule.Instance.IsAuthenticated);
            bool showTradeTicket = MainModule.Instance.AuthProvider.ShowTradeTicket;  ///WYJ note, false if not overridden
            this.pnlTrade.Visible = this.mniViewTradeTicket.Checked && showTradeTicket;
            this.mniViewTradeTicket.Visible = showTradeTicket;
            this.btnTrade.Visible = showTradeTicket;
            this.btnTradeTicket.Visible = showTradeTicket;
            this.dockTradePanel(MainModule.Instance.Settings.Get("TradeTicketDockBottom", false));
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
            this.treeDataSources.PopulateWithFilter(MainModule.Instance.DataSources); ///WYJ fix, work around for the Code Metrics problem
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
                    button.Click += new EventHandler(this.chartStyleButtons_Click);
                    button.Checked = style.FriendlyName == str2;
                }
            }
            this.workspaceDir = MainModule.Instance.DataPath + @"\Workspaces";
            if (!Directory.Exists(this.workspaceDir))
            {
                Directory.CreateDirectory(this.workspaceDir);
            }
            if (bool_0)
            {
                MainModule.Instance.Settings.Get(this, "MainForm");
                bool_0 = false;
                this.isFirstMainForm = true;
                string path = MainModule.Instance.DataPath + @"\Workspaces\Default.ws";
                if (System.IO.File.Exists(path))
                {
                    this.loadWorkSpace(path);
                    if (base.MdiChildren.Length == 0)
                    {
                        this.showOrToolStripItemsForMdiChild("G");
                    }
                }
                else if (MainModule.Instance.Settings.Get("ShowHomePage", true))
                {
                    this.btnHome.PerformClick();
                }
                else
                {
                    this.showOrToolStripItemsForMdiChild("G");
                }
            }
            AssemblyLoader loader = new AssemblyLoader {
                BaseClass = "DrawingObjectHelper",
                Path = Path.GetDirectoryName(Application.ExecutablePath)
            };

            ///WYJ note, add the drawing object tool strip items, group by group
            for (DrawingObjectHelper.ToolBarGroup group = DrawingObjectHelper.ToolBarGroup.None; group < DrawingObjectHelper.ToolBarGroup.UserDefined; group += 1)
            {
                int num = 0;
                DrawingObjectHelper helper = null;
                foreach (System.Type type2 in loader.Types)
                {
                    helper = (DrawingObjectHelper) loader.CreateInstance(type2);
                    if (helper.Grouping == group)
                    {
                        ToolStripButton button2 = new ToolStripButton(helper.FriendlyName, helper.Glyph, new EventHandler(this.drawingObjectToolStripItem_Click));
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
            foreach (string str7 in MainModule.Instance.WorkspaceMenuItems)   ///WYJ note, MainModule.Instance.WorkspaceMenuItems is always empty
            {
                this.AddWorkspaceMenuItem(str7);
            }
            FundamentalsLoader loader2 = new FundamentalsLoader {
                DataHost = MainModule.Instance.DataSources
            };
            this.dragDropFundamentalsExist = loader2.HasDragDropFundamentals;
            this.btnFundamental.Visible = this.dragDropFundamentalsExist;
            this.mniFundamentals.Visible = this.dragDropFundamentalsExist;
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
                this.showOrToolStripItemsForMdiChild("G");
                this.EnableControls(true);
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is HomeForm)
            {
                this.showOrToolStripItemsForMdiChild("G");
                this.EnableControls(true);
                this.btnHome.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is SymbolManagerForm)
            {
                this.showOrToolStripItemsForMdiChild("G");
                this.EnableControls(true);
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is AccountsPositionsForm)
            {
                this.showOrToolStripItemsForMdiChild("G");
                this.EnableControls(true);
                this.btnAcctsPositions.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is OrdersAlertsForm)
            {
                this.showOrToolStripItemsForMdiChild("G");
                this.EnableControls(true);
                this.btnOrdersAlerts.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is StrategyCenterForm)
            {
                this.showOrToolStripItemsForMdiChild("G");
                this.EnableControls(true);
                this.btnStrategyCenter.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is DataManagerForm)
            {
                this.showOrToolStripItemsForMdiChild("G");
                this.EnableControls(true);
                this.btnDataManager.BackColor = this.color_0;
                this.SetDataPanelState(false, false, false, false, false);
            }
            else if (base.ActiveMdiChild is QuotesForm)
            {
                this.showOrToolStripItemsForMdiChild("G");
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
                this.showOrToolStripItemsForMdiChild("G");
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
                this.copyScaleFromActiveChartForm();
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
                    this.showOrToolStripItemsForMdiChild("S");
                    StrategyType strategyType = activeChartWindow.Strategy.StrategyType;
                    this.SetDataPanelState(true, true, true, true, activeChartWindow.Strategy.StrategyType == StrategyType.CombinedStrategy);
                }
                else
                {
                    this.showOrToolStripItemsForMdiChild("C");
                    this.SetDataPanelState(true, true, false, false, false);
                }
                if ((activeChartWindow.DataSource != null) && (activeChartWindow.Symbol != ""))
                {
                    this.treeDataSources.SelectSymbol(activeChartWindow.DataSource, activeChartWindow.Symbol);
                    this.cmbSymbol.Text = activeChartWindow.Symbol;
                }
                this.copyScaleFromActiveChartForm();
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

        ///WYJ fix, original signature: private void method_0()
        private void doOpenOrderManager()
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

        ///WYJ fix, original signature: private void method_1()
        private void doOpenStrategyRanking()
        {
            new StrategyRanking { MdiParent = this }.Show();
        }

        ///WYJ fix, original signature: private void method_10(object sender, EventArgs e)
        private void positionSizeChangedEventHandler(object sender, EventArgs e)
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

        ///WYJ fix, original signature: private void method_11(object sender, EventArgs e)
        private void drawingObjectToolStripItem_Click(object sender, EventArgs e)
        {
            this.ClearDrawingObjectSelectedTool();
            ToolStripButton button = sender as ToolStripButton;
            button.Checked = true;
            Chart.TypeOfObjectToDraw = (System.Type) button.Tag;
            this.setCursorForChartForms(Cursors.Cross);
            if (Chart.DisplayCrossHair)
            {
                this.btnCrossHair_Click(sender, e);
            }
        }

        ///WYJ fix, original signature: private void method_12(Cursor cursor_0)
        private void setCursorForChartForms(Cursor cursor_0)
        {
            foreach (Form form in base.MdiChildren)
            {
                if (form is ChartForm)
                {
                    (form as ChartForm).SetChartCursor(cursor_0);
                }
            }
        }

        private void dataSourceSelectedEventHandler(object sender, DataSourceEventArgs e)
        {
            this.btnGo.Enabled = true;
            this.cmbSymbol.Text = "";
            if (base.ActiveMdiChild is StrategyRanking)
            {
                (base.ActiveMdiChild as StrategyRanking).UpdateDataSource(e.DataSource, "");
                this.copyScaleFromActiveChartForm();
            }
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if ((activeChartWindow != null) && !activeChartWindow.IsBusy)
            {
                activeChartWindow.DataSourceSelected(e.DataSource);
                this.copyScaleFromActiveChartForm();
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
            this.chartFormShowMultiSymbol(true);
        }

        ///WYJ fix, original signature: internal void method_14()
        internal void selectFirstDataSourceNode()
        {
            this.treeDataSources.SelectedNode = this.treeDataSources.Nodes[0];
        }

        ///WYJ fix, original signature: private void method_15(object sender, DataSourceSymbolEventArgs e)
        private void symbolSelectionChangeEventHandler(object sender, DataSourceSymbolEventArgs e)
        {
            if (!this.dataSourceTreeSelectedNodeLocked)
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
                    this.copyScaleFromActiveChartForm();
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
                    this.copyScaleFromActiveChartForm();
                }
                this.informChartFormsForSymbolChange();
                this.chartFormShowMultiSymbol(false);
            }
        }

        ///WYJ fix, original signature: private void method_16(bool bool_10)
        private void chartFormShowMultiSymbol(bool bool_10)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.ShowMultiSymbolControls(bool_10);
            }
        }

        ///WYJ note, inlined this method, not needed anymore
        /*
        private void method_17()
        {
            if (this.treeDataSources.SelectedNode != null)
            {
                bool flag = (this.treeDataSources.SelectedNode.Level == 0) && (this.cmbSymbol.Text == string.Empty);
                this.chartFormShowMultiSymbol(flag);
            }
        }*/

        ///WYJ fix, original signature: private void method_18()
        ///WYJ note, show the Close WorkSpace menu item only when it's not the first workspace window
        private void showOrHideCloseWSMenuItems()
        {
            foreach (Form form2 in Application.OpenForms)
            {
                if (form2 is MainForm)
                {
                    MainForm form = form2 as MainForm;
                    form.mniCloseWorkspace.Visible = (mainFormCount > 1) && !form.isFirstMainForm_2;
                }
            }
        }

        ///WYJ fix, original signature: private bool method_19()
        private bool confirmExit()
        {
            return (menuTriggeredExit || ((mainFormCount == 1) || (MessageBox.Show("Close all Workspaces and shut down Wealth-Lab?", "Exit Wealth-Lab", MessageBoxButtons.YesNo) == DialogResult.Yes)));
        }

        ///WYJ fix, original signature: private void method_2()
        private void editMenuDefaultEnablement()
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

        ///WYJ fix, original signature: private bool method_20(string string_1)
        private bool saveWorkSpace(string filePath)
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
                FileNameValidator.ValidateFileName(filePath);
                System.IO.File.WriteAllLines(filePath, contents);
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
            this.disconnected = false;
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
                    this.dockTradePanel(strArray2[index].Contains("Yes"));
                    index++;
                    continue;
                }
                if (strArray2[index].StartsWith("TradeTicketState="))
                {
                    this.showOrHideTradePanel(strArray2[index].Contains("Yes"));
                    index++;
                    continue;
                }
                if (strArray2[index].StartsWith("DataWindow="))
                {
                    int num2;
                    string[] strArray = this.getValueString(strArray2[index++]).Split(new char[] { ',' });
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
                string str = this.getValueString(strArray2[index++]);
                string str4 = this.getValueString(strArray2[index++]);
                Rectangle rectangle2 = new Rectangle();
                string[] strArray3 = str4.Split(new char[] { ',' });
                rectangle2.X = int.Parse(strArray3[0]);
                rectangle2.Y = int.Parse(strArray3[1]);
                rectangle2.Width = int.Parse(strArray3[2]);
                rectangle2.Height = int.Parse(strArray3[3]);
                int version = int.Parse(this.getValueString(strArray2[index++]));
                int num4 = int.Parse(this.getValueString(strArray2[index++]));
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
                                    //goto  Label_0390; ///WYJ fix, simplify the flow
                                    form = new StrategyCenterForm();
                                }
                                else
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
                if (form != null)
                {
                    workspace = form as IWorkspace;
                }
                if ((form != null) && (workspace != null))
                {
                    form.MdiParent = this;
                    form.Show();
                    form.SetBounds(rectangle2.X+5, rectangle2.Y+23, rectangle2.Width+100, rectangle2.Height - 35); ///WYJ fix, change the size and location of the chart window
                    form.WindowState = FormWindowState.Maximized; ///WYJ fix, maximize the chart window
                    if (form is ChartForm)
                    {
                        ChartForm form2 = form as ChartForm;
                        if (form2.IsStreaming)
                        {
                            form2.ResetStreaming();
                        }
                        if (this.disconnected)
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

        ///WYJ fix, original signature: private string method_22(string string_1)
        private string getValueString(string string_1)
        {
            int index = string_1.IndexOf('=');
            return string_1.Substring(index + 1);
        }

        ///WYJ fix, original signature: internal void method_23(string string_1)
        ///WYJ note, "G" for general, like HomeWindow, see MainForm_MdiChildActivate(); "C" for chart; "S" for Strategy
        internal void showOrToolStripItemsForMdiChild(string string_1)
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

        ///WYJ note, never used
        private void method_24(object sender, EventArgs e)
        {
            Alert alert = this.createAlert();
            MainModule.Instance.TradeManager.AddAlert(alert, false, false);
        }

        ///WYJ fix, original signature: private Alert method_25()
        private Alert createAlert()
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

        ///WYJ note, never used
        private void method_26(object sender, EventArgs e)
        {
            MainModule.NotImplemented();
        }

        ///WYJ note, never used
        private void method_27(object sender, EventArgs e)
        {
            this.mniViewTradeTicket.PerformClick();
        }

        ///WYJ fix, original signature: private void method_28(object sender, EventArgs e)
        private void chartFormMenuItemClickEventHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            ChartForm tag = (ChartForm) item.Tag;
            tag.BringToFront();
            tag.WindowState = FormWindowState.Normal;
        }

        ///WYJ fix, original signature: private void method_29(object sender, EventArgs e)
        private void quotesFormMenuItemClickEventHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            QuotesForm tag = (QuotesForm) item.Tag;
            tag.BringToFront();
            tag.WindowState = FormWindowState.Normal;
        }

        ///WYJ note, never used
        private void method_3(string string_1)
        {
            if (DebugForm.Instance == null)
            {
                DebugForm.Instance = new DebugForm();
                DebugForm.Instance.Show();
            }
            DebugForm.Instance.AddLine(string_1);
        }

        ///WYJ fix, original signature: private void method_30(bool reconnect)
        private void doConnect(bool reconnect)
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
            if (reconnect)
            {
                foreach (Form form2 in base.MdiChildren)
                {
                    if (form2 is ChartForm)
                    {
                        ChartForm form = form2 as ChartForm;
                        if (form.DisconnectedWhileStreaming)
                        {
                            form.Connect(reconnect);
                        }
                        if (form.IsStreaming)
                        {
                            form.IsStreaming = false;
                            form.IsStreaming = true;
                        }
                    }
                    if (form2 is QuotesForm)
                    {
                        (form2 as QuotesForm).Connect(reconnect);
                    }
                    if (form2 is AccountsPositionsForm)
                    {
                        (form2 as AccountsPositionsForm).Connect(reconnect);
                    }
                }
            }
        }

        ///WYJ fix, original signature: private void method_31()
        private void doDisconnect()
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
            foreach (string str in this.streamingSymbols)
            {
                this.streamingQuoteManager_0.Unsubscribe(str);
            }
            this.streamingSymbols.Clear();
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

        ///WYJ fix, original signature: private void method_32(ConnStatus connStatus_0, int int_7, string string_1)
        private void doStatusUpdate(ConnStatus status, int statusCode, string message)
        {
            switch (status)
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
            if (statusCode != 0)
            {
                message = statusCode + ": " + message;
            }
            this.statusStreamingStatus.Text = message;
        }

        ///WYJ fix, original signature: private void method_33(object sender, EventArgs e)
        private void scaleChangedEventHandler(object sender, EventArgs e)
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

        ///WYJ fix, original signature: private void method_34(object sender, EventArgs e)
        private void sliderValueChangedEventHandler(object sender, EventArgs e)
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
                        this.enableParamLinkButtons(true);
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
                        this.enableParamLinkButtons(true);
                    }
                }
                catch
                {
                }
            }
        }

        ///WYJ fix, original signature: private void method_35(bool bool_10)
        private void enableParamLinkButtons(bool enable)
        {
            this.linkSaveParams.Enabled = enable;
            this.linkResetParams.Enabled = enable;
        }

        ///WYJ fix, original signature: private void method_36(object sender, AsyncCompletedEventArgs e)
        private void authProviderInstallerDownloadCompleteEventHandler(object sender, AsyncCompletedEventArgs e)
        {
            this.statusSofwareDownload.Visible = false;
            this.statusDownloadProgressBar.Visible = false;
        }

        ///WYJ fix, original signature: private void method_37(object sender, DownloadProgressChangedEventArgs e)
        private void authProviderInstallerDownloadProgrChangedEventHandler(object sender, DownloadProgressChangedEventArgs e)
        {
            this.statusSofwareDownload.Visible = true;
            this.statusDownloadProgressBar.Visible = true;
            this.statusDownloadProgressBar.ProgressBar.Value = e.ProgressPercentage;
        }

        ///WYJ fix, original signature: private void method_38(bool bool_10)
        private void dockTradePanel(bool dockBottom)
        {
            if (dockBottom)
            {
                this.pnlTrade.Dock = DockStyle.Bottom;
            }
            else
            {
                this.pnlTrade.Dock = DockStyle.Top;
            }
            this.btnDockDown.Visible = !dockBottom;
            this.btnDockUp.Visible = dockBottom;
        }

        ///WYJ fix, original signature: private void method_39(bool bool_10)
        private void showOrHideTradePanel(bool viewTradeTicket)
        {
            this.mniViewTradeTicket.Checked = viewTradeTicket;
            this.pnlTrade.Visible = this.mniViewTradeTicket.Checked;
            this.btnTradeTicket.Checked = this.pnlTrade.Visible;
            this.btnTrade.Checked = this.pnlTrade.Visible;
        }

        ///WYJ fix, original signature: private void method_4(string string_1)
        private void updateStatusBar(string string_1)
        {
            this.statusMessage.Text = string_1;
            this.status.Refresh();
        }

        ///WYJ fix, original signature: private void method_40(object sender, EventArgs e)
        ///WYJ note, act as menu item click handler, but it seems that the menu items are never added, hence this method is never called
        private void workspaceMenuItems_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string str = this.workspaceDir + @"\" + item.Text.Replace("&", "") + ".ws";
            this.loadWorkSpace(str);
        }

        ///WYJ fix, original signature: private void method_41()
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

        ///WYJ fix, original signature: internal void method_42(DraggedFundamentalItem draggedFundamentalItem_0)
        internal void onFundamentalItemDropped(DraggedFundamentalItem draggedFundamentalItem_0)
        {
            ChartForm activeChartWindow = this.ActiveChartWindow;
            if (activeChartWindow != null)
            {
                activeChartWindow.processDroppedFundamentalItem(draggedFundamentalItem_0);
            }
        }

        ///WYJ fix, original signature: private void method_43(object sender, EventArgs e)
        private void sliderMouseDownEventHandler(object sender, EventArgs e)
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

        ///WYJ fix, original signature: private void method_45(object sender, EventArgs e)
        private void indexManagerClickedEventHandler(object sender, EventArgs e)
        {
            this.CreateIndexManager();
        }

        ///WYJ fix, original name: private void method_46(object sender, EventArgs e)
        private void dataSourceTreeViewRenameClickedEventHandler(object sender, EventArgs e)
        {
        }

        ///WYJ fix, original name: internal void method_47()
        internal void checkUncheckDataPanelMniAndBtn()
        {
            this.btnDataWindow.Checked = DataWindowForm.Instance != null;
            this.mniDataWindow.Checked = DataWindowForm.Instance != null;
        }

        ///WYJ fix, original signature: private ChartStyle method_5()
        private ChartStyle getSelectedChartStyle()
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

        ///WYJ fix, original signature: private ChartStyle method6(string string_1) 
        private ChartStyle getChartStyleByName(string chartStyleName)
        {
            ChartStyle chartStyle = null;
            ChartStyle tag = (ChartStyle)this.btnBarChart.Tag;
            if (tag.FriendlyName != chartStyleName)
            {
                tag = (ChartStyle)this.btnCandleStyle.Tag;
                if (tag.FriendlyName != chartStyleName)
                {
                    tag = (ChartStyle)this.btnLineChart.Tag;
                    if (tag.FriendlyName != chartStyleName)
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
                                        if (tag.FriendlyName == chartStyleName)
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

        ///WYJ fix, original signature: private void method_7()
        private void informChartFormsForSymbolChange()
        {
            foreach (Form mdiChild in base.MdiChildren)
            {
                if (mdiChild is ChartForm)
                {
                    ChartForm form = mdiChild as ChartForm;
                    if ((form != this.ActiveChartWindow) && form.LinkedToSymbol)
                    {
                        form.ResetStreaming();
                        form.DataSource = this.DataSource;
                        form.GoButtonPressed(this.cmbSymbol.Text, false);
                    }
                }
            }
        }

        ///WYJ fix, original signature: private void method_8()
        ///WYJ note, it seems that the bar Data scale is copied to MainForm and then assigned back to ChartForm. See ChartForm.RunOnAllSymbols()
        private void copyScaleFromActiveChartForm()
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

        ///WYJ fix, original signature: private void method_9(object sender, EventArgs e)
        private void dataRangeChangedEventHandler(object sender, EventArgs e)
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
            menuTriggeredExit = true;
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
            if (this.mniFundamentals.Visible && !this.dragDropFundamentalsExist)
            {
                this.mniFundamentals.Visible = this.dragDropFundamentalsExist;
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
            this.openFileDialog_0.InitialDirectory = this.workspaceDir;
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
            form.showOrToolStripItemsForMdiChild("G");
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
            this.saveFileDialog_0.InitialDirectory = this.workspaceDir;
            if (this.saveFileDialog_0.ShowDialog() != DialogResult.Cancel)
            {
                this.saveWorkSpace(this.saveFileDialog_0.FileName);
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
            if (this.saveWorkSpace(path + @"\Default.ws"))
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
                base.Invoke(new Delegate34(this.doOpenOrderManager));
            }
            else
            {
                this.doOpenOrderManager();
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
                case DialogResult.Yes:   ///New strategy from code
                    this.CreateNewStrategyWindow(true).Show();
                    return;

                case DialogResult.No:   ///New strategy from rules
                    this.CreateNewStrategyWindow(false).Show();
                    break;

                case DialogResult.OK:   ///open existing strategy
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
                        ChartForm chartForm = this.OpenStrategyWindow(strategy2);
                        MainModule.Instance.Strategies.Strategies.Add(strategy2);
                        chartForm.Show();
                        MainModule.Instance.Strategies.LoadStrategyParameters(chartForm.Strategy, chartForm.WealthScript);
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
                base.Invoke(new Delegate35(this.doOpenStrategyRanking));
            }
            else
            {
                this.doOpenStrategyRanking();
            }
        }

        public ChartForm OpenStrategyWindow(Strategy strategy)
        {
            //return this.OpenStrategyWindow(strategy_0, true);   ///WYJ fix, inline the method call
            return this.OpenStrategyWindow(strategy, true, true);
        }

        public ChartForm OpenStrategyWindow(Strategy strategy, bool executeOnSelectedSymbol)
        {
            return this.OpenStrategyWindow(strategy, executeOnSelectedSymbol, true);
        }

        ///WYJ fix, original signature: public ChartForm OpenStrategyWindow(Strategy strategy_0, bool executeOnSelectedSymbol, bool useAdvancedSettings)
        public ChartForm OpenStrategyWindow(Strategy strategy, bool executeOnSelectedSymbol, bool useAdvancedSettings)
        {
            ChartForm form = this.CreateChartWindow(false);
            form.Strategy = strategy;
            
            this.showOrToolStripItemsForMdiChild("S");
            if (strategy.StrategyType == StrategyType.CombinedStrategy)
            {
                form.SelectTab("Combination Strategy");
            }
            else
            {
                form.SelectTab("Chart");
            }
            if (useAdvancedSettings && MainModule.Instance.Settings.Get("RememberStrategyScale", false))
            {
                this.BarDataScale = strategy.DataScale;
                form.SetBarDataScaleForDataSource(this.DataSource, strategy.DataScale);
            }
            if (useAdvancedSettings && MainModule.Instance.Settings.Get("RememberStrategyPositionSize", false))
            {
                this.posSize.PositionSize = strategy.PositionSize;
                form.PositionSize = strategy.PositionSize;
            }
            if (useAdvancedSettings && MainModule.Instance.Settings.Get("RememberStrategyRange", false))
            {
                this.dataRange.DataRange = strategy.DataRange;
                form.DataRange = strategy.DataRange;
            }
            bool flag = false;
            if ((useAdvancedSettings && MainModule.Instance.Settings.Get("RememberStrategyData", false)) && (strategy.DataSetName != ""))
            {
                WealthLab.DataSource source = MainModule.Instance.DataSources.FindDataSource(strategy.DataSetName);
                if (source != null)
                {
                    if (MainModule.Instance.Settings.Get("RememberStrategyScale", false))
                    {
                        form.SetBarDataScaleForDataSource(source, strategy.DataScale);
                    }
                    flag = true;
                    if (strategy.Symbol != "")
                    {
                        this.treeDataSources.SelectSymbol(source, strategy.Symbol);
                        this.symbolSelectionChangeEventHandler(this.treeDataSources, new DataSourceSymbolEventArgs(this.treeDataSources.DataSource, this.treeDataSources.Symbol));
                    }
                    else
                    {
                        this.treeDataSources.SelectDataSource(source);
                        this.dataSourceSelectedEventHandler(this.treeDataSources, new DataSourceEventArgs(this.treeDataSources.DataSource));
                    }
                }
            }
            if ((!flag && (this.treeDataSources.SelectedNode != null)) && executeOnSelectedSymbol)
            {
                if ((this.treeDataSources.Symbol != null) && (this.treeDataSources.Symbol != ""))
                {
                    this.SelectingNodeForFormCreation = true;
                    this.symbolSelectionChangeEventHandler(this.treeDataSources, new DataSourceSymbolEventArgs(this.treeDataSources.DataSource, this.treeDataSources.Symbol));
                }
                else
                {
                    this.dataSourceSelectedEventHandler(this.treeDataSources, new DataSourceEventArgs(this.treeDataSources.DataSource));
                }
            }
            this.SetDataPanelState(true, true, true, true, strategy.StrategyType == StrategyType.CombinedStrategy);
            MainModule.Instance.AddStrategyToMRU(strategy);
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

        ///WYJ fix, original signature: public void PrintStatus(string message)
        public void UpdateStatus(string message)
        {
            base.Invoke(new Delegate38(this.updateStatusBar), new object[] { message });
        }

        public void SelectNode()
        {
            if ((this.treeDataSources.DataSource != null) && (this.cmbSymbol.Text != string.Empty))
            {
                this.symbolSelectionChangeEventHandler(this, new DataSourceSymbolEventArgs(this.treeDataSources.DataSource, this.cmbSymbol.Text));
            }
            else if (this.treeDataSources.Nodes.Count > 0)
            {
                if (this.treeDataSources.Symbol != "")
                {
                    this.symbolSelectionChangeEventHandler(this, new DataSourceSymbolEventArgs(this.treeDataSources.DataSource, this.treeDataSources.Symbol));
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
            this.enableParamLinkButtons(color3 == Color.Red);
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
            randomDelay = new Random().Next(5);
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
            foreach (string str in this.streamingSymbols)
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
                foreach (string str in this.streamingSymbols)
                {
                    this.streamingQuoteManager_0.Subscribe(str);
                }
                this.statusStreamingSymbolsOn.Visible = false;
                this.statusStreamingSymbolsOff.Visible = true;
            }
        }

        public void StatusUpdate(ConnStatus status, int StatusCode, string Message)
        {
            base.Invoke(new Delegate41(this.doStatusUpdate), new object[] { status, StatusCode, Message });
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
            this.dataSourceTreeSelectedNodeLocked = true;
            try
            {
                this.treeDataSources.SelectedNode = this.treeDataSources.FindNode(this.chartForm_0.DataSource, this.chartForm_0.Symbol);
            }
            finally
            {
                this.dataSourceTreeSelectedNodeLocked = false;
            }
        }

        private void timer_2_Tick(object sender, EventArgs e)
        {
            ///WYJ note: TNP might be some tick number indicating the time left before next check point of authentication
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
                        goto  Label_01A2;

                    case 2:
                        this.method_44(random.Next(100, 0x3e8));
                        goto  Label_01A2;

                    case 3:
                        if (--randomDelay < 0)
                        {
                            Application.Exit();
                        }
                        goto  Label_01A2;
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
                else if (--randomDelay <= 0)
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
                        this.symbolSelectionChangeEventHandler(this, new DataSourceSymbolEventArgs(this.DataSource, selectedNode.Text));
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
            TreeNode nodeAt = this.treeDataSources.GetNodeAt(this.mouseX, this.mouseY);
            if (nodeAt != null)
            {
                this.treeDataSources.DoDragDrop(nodeAt, DragDropEffects.Copy);
            }
        }

        private void treeDataSources_MouseDown(object sender, MouseEventArgs e)
        {
            this.mouseX = e.X;
            this.mouseY = e.Y;
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
                foreach (string str2 in this.streamingSymbols)
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
                    if (!this.streamingSymbols.Contains(str3))
                    {
                        this.streamingQuoteManager_0.Subscribe(str3);
                    }
                }
            }
            this.streamingSymbols.Clear();
            foreach (string str4 in symbols)
            {
                this.streamingSymbols.Add(str4);
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
                return this.isFirstMainForm;
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
                return this.selectingNodeForFormCreation;
            }
            [CompilerGenerated]
            set
            {
                this.selectingNodeForFormCreation = value;
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

