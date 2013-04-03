namespace WealthLabPro
{
    using CtrlLib;
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using WealthLab;

    public class PreferencesForm : Form
    {
        private AssemblyLoader assemblyLoader_0;
        private AssemblyLoader assemblyLoader_1;
        private Bars bars_0 = new Bars("QQQQ", BarScale.Daily, 0);
        private bool bool_0;
        private bool bool_1 = true;
        private Button btnAddFund;
        private Button btnApply;
        private Button btnBrowse_Quotes;
        private Button btnBrowse_RealTimeStrategy;
        private Button btnBrowse_StrategyMonitor;
        private Button btnBrowse_StrategyWindow;
        private Button btnCalculate;
        private Button btnCancel;
        private Button btnChangeFont;
        private Button btnChangeSymbolFont;
        private Button btnMovePvDown;
        private Button btnMovePvUp;
        private Button btnOK;
        private Button btnRemoveFund;
        private Button btnTestMail;
        private CheckBox cbApplyCharts;
        private CheckBox cbAutoOpenOrders;
        private CheckBox cbBadTickFilter;
        private CheckBox cbBenchmarkBH;
        private CheckBox cbBuyingPowerThreshold;
        private CheckBox cbCashThreshold;
        private CheckBox cbCommission;
        private CheckBox cbDisablePortfolioSynch;
        private CheckBox cbDividends;
        private CheckBox cbExitAll;
        private CheckBox cbExpand;
        private CheckBox cbFundamentalTooltip;
        private CheckBox cbHorizontalGridlines;
        private CheckBox cbIndicatorTooltip;
        private CheckBox cbInterest;
        private CheckBox cbLimitDays;
        private CheckBox cbLimitSlippage;
        private CheckBox cbNoDecimalRoundingForLimitStopPrice;
        private CheckBox cbPaneSeparators;
        private CheckBox cbPriceTooltip;
        private CheckBox cbPrintDialogOff;
        private CheckBox cbPrintPreviewOff;
        private CheckBox cbReduceQty;
        private CheckBox cbRememberData;
        private CheckBox cbRememberParamValues;
        private CheckBox cbRememberPositionSize;
        private CheckBox cbRememberRange;
        private CheckBox cbRememberScale;
        private CheckBox cbRound50;
        private CheckBox cbRoundLots;
        private CheckBox cbSameBarExit;
        private CheckBox cbShowHome;
        private CheckBox cbSlippage;
        private CheckBox cbSoundQuotes;
        private CheckBox cbSoundsIndicators;
        private CheckBox cbSoundsParameters;
        private CheckBox cbSoundsRealTime;
        private CheckBox cbSoundStategyMonitor;
        private CheckBox cbSoundStrategyWindow;
        private CheckBox cbSwitchAccount;
        private CheckBox cbVerticalGridlines;
        private CheckBox cbWorstTradeSimulation;
        private CheckBox chkAuthenticateWithPassword;
        private CheckBox chkSSL;
        private ComboBox cmbCommAction;
        private ComboBox cmbCommOrder;
        private ComboBox cmbDefaultAccount;
        private ColorPickerPanel colorBackground;
        private ColorPickerPanel colorBottomMargin;
        private ColorPickerPanel colorDownBars;
        private ColorPickerPanel colorDownVolume;
        private ColorPickerPanel colorGridlines;
        private ColorPickerPanel colorPaneSep;
        private ColorPickerPanel colorRightMargin;
        private ColorPickerPanel colorUpBars;
        private ColorPickerPanel colorUpVolume;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private FontDialog fontDialog_0;
        private FundamentalsLoader fundamentalsLoader_0;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox grpChartColors;
        private GroupBox grpChartFont;
        private GroupBox grpCommission;
        private GroupBox grpCommissionDesc;
        private GroupBox grpDecimalRoundingOnOrder;
        private GroupBox grpDefault;
        private GroupBox grpEmailProperties;
        private GroupBox grpLimit;
        private GroupBox grpLimitShares;
        private GroupBox grpMargin;
        private GroupBox grpOptions;
        private GroupBox grpOtherSounds;
        private GroupBox grpRoundLots;
        private GroupBox grpSlippage;
        private GroupBox grpSoundAlerts;
        private GroupBox grpStreamingProvider;
        private GroupBox grpStreamingSymbols;
        private GroupBox grpTestCommission;
        private GroupBox grpTestMail;
        private GroupBox grpThresholds;
        private GroupBox grpTooltips;
        private GroupBox grpTradingOptions;
        private GroupBox grpVisDesc;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private ImageList imageList_1;
        private NumericUpDown indicatorDecimalPlaces;
        private Label label1;
        private Label label2;
        private Label label3;
        private ListBox lbCommission;
        private Label lblAxisFont;
        private Label lblBadTickFilter;
        private Label lblBadTickThreshold;
        private Label lblBottomMarginColor;
        private Label lblCashRate;
        private Label lblChartAnnotations;
        private Label lblChartBackground;
        private Label lblCommAction;
        private Label lblCommissionDesc;
        private Label lblCommOrder;
        private Label lblCommPrice;
        private Label lblCommResult;
        private Label lblCommShares;
        private Label lblCommTest;
        private Label lblDefault;
        private Label lblDownBarColor;
        private Label lblDownVolumeColor;
        private Label lblEmailAddresses;
        private Label lblExitAllWarning;
        private Label lblFutures;
        private Label lblFuturesSlippage;
        private Label lblGridlineColor;
        private Label lblHomePageOverride;
        private Label lblLimitDays;
        private Label lblMarginRate;
        private Label lblPaneSepColor;
        private Label lblPassword;
        private Label lblPV;
        private Label lblPVDesc;
        private Label lblReduceQtyPct;
        private Label lblRemember;
        private Label lblRightMarginColor;
        private Label lblSamebar;
        private Label lblSampleText;
        private Label lblSlippage;
        private Label lblSlippageEquities;
        private Label lblSMTPHost;
        private Label lblSMTPPort;
        private Label lblStreaming;
        private Label lblStreamingDesc;
        private Label lblStreamingDetails;
        private Label lblStreamingProvider;
        private Label lblTicker;
        private Label lblTitleFont;
        private Label lblTitleFontSample;
        private Label lblTradingThresholds;
        private Label lblUpBarColor;
        private Label lblUpVolume;
        private Label lblUserID;
        private LinkLabel linkSettings;
        private LinkLabel linkStreamingMore;
        private ListView lvFundAvailable;
        private ListView lvFundSelected;
        private ListView lvPV;
        private ListView lvStreaming;
        private NumEdit numBadTick;
        private NumEdit numBuyingPowerThreshold;
        private NumEdit numCashRate;
        private NumEdit numCashThreshold;
        private NumericUpDown numCommPrice;
        private NumericUpDown numCommShares;
        private NumericUpDown numFuturesSlippage;
        private NumEdit numMarginRate;
        private NumEdit numReduceQty;
        private NumericUpDown numSlippage;
        private OpenFileDialog openFileDialog_0;
        private PictureBox picStreaming;
        private Panel pnlAdvanced;
        private Panel pnlBadTickFilter;
        private Panel pnlBottom;
        private Panel pnlChartAnnotations;
        private Panel pnlCommissions;
        private Panel pnlCS;
        private Panel pnlEA;
        private Panel pnlPV;
        private Panel pnlSlippage;
        private Panel pnlSounds;
        private Panel pnlStreaming;
        private Panel pnlTradeSim;
        private Panel pnlTtrading;
        private NumericUpDown pricingDecimalPlaces;
        private SplitContainer split;
        private SymbolParser symbolParser_0;
        private ToolStrip toolbar;
        private TreeView tree;
        private TextBox txtBHSymbol;
        private TextBox txtBoxQuotes_SoundPath;
        private TextBox txtBoxRealTime_SoundPath;
        private TextBox txtBoxStrategyMonitor_SoundPath;
        private TextBox txtBoxStrategyWindow_SoundPath;
        private TextBox txtEmailAddresses;
        private TextBox txtPassword;
        private TextBox txtSMTPHost;
        private TextBox txtSMTPPort;
        private TextBox txtStreaming;
        private TextBox txtUserID;

        public PreferencesForm()
        {
            this.InitializeComponent();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (this.cbBenchmarkBH.Checked && string.IsNullOrEmpty(this.txtBHSymbol.Text.Trim()))
            {
                MessageBox.Show("Please enter a symbol for the Benchmark Buy & Hold preference", Application.ProductName);
            }
            else if (this.method_9())
            {
                this.method_1();
                this.btnApply.Enabled = false;
            }
        }

        private void btnBrowse_Quotes_Click(object sender, EventArgs e)
        {
            this.txtBoxQuotes_SoundPath.Text = this.method_5(this.txtBoxQuotes_SoundPath.Text);
        }

        private void btnBrowse_RealTimeStrategy_Click(object sender, EventArgs e)
        {
            this.txtBoxRealTime_SoundPath.Text = this.method_5(this.txtBoxRealTime_SoundPath.Text);
        }

        private void btnBrowse_StrategyMonitor_Click(object sender, EventArgs e)
        {
            this.txtBoxStrategyMonitor_SoundPath.Text = this.method_5(this.txtBoxStrategyMonitor_SoundPath.Text);
        }

        private void btnBrowse_StrategyWindow_Click(object sender, EventArgs e)
        {
            this.txtBoxStrategyWindow_SoundPath.Text = this.method_5(this.txtBoxStrategyWindow_SoundPath.Text);
        }

        private void btnChangeFont_Click(object sender, EventArgs e)
        {
            this.fontDialog_0.Font = this.lblSampleText.Font;
            if (this.fontDialog_0.ShowDialog() == DialogResult.OK)
            {
                this.lblSampleText.Font = this.fontDialog_0.Font;
            }
        }

        private void btnChangeSymbolFont_Click(object sender, EventArgs e)
        {
            this.fontDialog_0.Font = this.lblTitleFontSample.Font;
            if (this.fontDialog_0.ShowDialog() == DialogResult.OK)
            {
                this.lblTitleFontSample.Font = this.fontDialog_0.Font;
            }
        }

        private void btnMovePvDown_Click(object sender, EventArgs e)
        {
            if (this.lvPV.SelectedItems.Count != 0)
            {
                ListViewItem item = this.lvPV.SelectedItems[0];
                if (item.Index != (this.lvPV.Items.Count - 1))
                {
                    int index = item.Index;
                    this.lvPV.Items.Remove(item);
                    this.lvPV.Items.Insert(index + 1, item);
                    item.Selected = true;
                    this.btnApply.Enabled = true;
                }
            }
        }

        private void btnMovePvUp_Click(object sender, EventArgs e)
        {
            if (this.lvPV.SelectedItems.Count != 0)
            {
                ListViewItem item = this.lvPV.SelectedItems[0];
                if (item.Index != 0)
                {
                    int index = item.Index;
                    this.lvPV.Items.Remove(item);
                    this.lvPV.Items.Insert(index - 1, item);
                    item.Selected = true;
                    this.btnApply.Enabled = true;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.cbBenchmarkBH.Checked && string.IsNullOrEmpty(this.txtBHSymbol.Text.Trim()))
            {
                MessageBox.Show("Please enter a symbol for the Benchmark Buy & Hold preference", Application.ProductName);
            }
            else if (this.method_9())
            {
                this.method_1();
                base.Close();
            }
        }

        private void btnTestMail_Click(object sender, EventArgs e)
        {
            if (this.method_9())
            {
                int num;
                int.TryParse(this.txtSMTPPort.Text, out num);
                try
                {
                    MainModule.Instance.method_27(this.txtSMTPHost.Text, num, this.chkSSL.Checked, this.txtUserID.Text, this.txtPassword.Text, this.txtEmailAddresses.Text.Trim());
                    MessageBox.Show("Message Sent Successfully", Application.ProductName);
                }
                catch (Exception exception)
                {
                    WLPException exception2 = new WLPException();
                    if (exception.InnerException != null)
                    {
                        exception2.ExceptionDetails = exception.InnerException.Message;
                    }
                    else
                    {
                        exception2.ExceptionDetails = exception.Message;
                    }
                    exception2.ShowDialog();
                }
            }
        }

        private void cbAutoOpenOrders_CheckStateChanged(object sender, EventArgs e)
        {
            this.cbSwitchAccount.Enabled = this.cbAutoOpenOrders.Checked;
        }

        private void cbBenchmarkBH_CheckedChanged(object sender, EventArgs e)
        {
            this.btnApply.Enabled = true;
            this.txtBHSymbol.Enabled = this.cbBenchmarkBH.Checked;
        }

        private void cbDisablePortfolioSynch_CheckedChanged(object sender, EventArgs e)
        {
            this.cbExitAll.Enabled = !this.cbDisablePortfolioSynch.Checked;
            this.btnApply.Enabled = true;
        }

        private void cbInterest_CheckedChanged(object sender, EventArgs e)
        {
            this.btnApply.Enabled = true;
        }

        private void cbRoundLots_CheckedChanged(object sender, EventArgs e)
        {
            this.cbRound50.Enabled = this.cbRoundLots.Checked;
            this.cbInterest_CheckedChanged(sender, e);
        }

        private void cbSlippage_CheckedChanged(object sender, EventArgs e)
        {
            this.cbLimitSlippage.Enabled = this.cbSlippage.Checked;
            this.cbInterest_CheckedChanged(sender, e);
        }

        private void cbSoundQuotes_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbSoundQuotes.Checked)
            {
                this.txtBoxQuotes_SoundPath.Enabled = true;
                this.btnBrowse_Quotes.Enabled = true;
            }
            else
            {
                this.txtBoxQuotes_SoundPath.Enabled = false;
                this.btnBrowse_Quotes.Enabled = false;
            }
            this.cbInterest_CheckedChanged(sender, e);
        }

        private void cbSoundsRealTime_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbSoundsRealTime.Checked)
            {
                this.txtBoxRealTime_SoundPath.Enabled = true;
                this.btnBrowse_RealTimeStrategy.Enabled = true;
            }
            else
            {
                this.txtBoxRealTime_SoundPath.Enabled = false;
                this.btnBrowse_RealTimeStrategy.Enabled = false;
            }
            this.cbInterest_CheckedChanged(sender, e);
        }

        private void cbSoundStategyMonitor_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbSoundStategyMonitor.Checked)
            {
                this.txtBoxStrategyMonitor_SoundPath.Enabled = true;
                this.btnBrowse_StrategyMonitor.Enabled = true;
            }
            else
            {
                this.txtBoxStrategyMonitor_SoundPath.Enabled = false;
                this.btnBrowse_StrategyMonitor.Enabled = false;
            }
            this.cbInterest_CheckedChanged(sender, e);
        }

        private void cbSoundStrategyWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbSoundStrategyWindow.Checked)
            {
                this.txtBoxStrategyWindow_SoundPath.Enabled = true;
                this.btnBrowse_StrategyWindow.Enabled = true;
            }
            else
            {
                this.txtBoxStrategyWindow_SoundPath.Enabled = false;
                this.btnBrowse_StrategyWindow.Enabled = false;
            }
            this.cbInterest_CheckedChanged(sender, e);
        }

        private void chkAuthenticateWithPassword_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPassword.Enabled = this.chkAuthenticateWithPassword.Checked;
            this.btnApply.Enabled = true;
        }

        private void chkSSL_CheckedChanged(object sender, EventArgs e)
        {
            this.btnApply.Enabled = true;
        }

        private void cmbCommAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbInterest_CheckedChanged(sender, e);
            this.method_4();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void indicatorDecimalPlaces_ValueChanged(object sender, EventArgs e)
        {
            this.btnApply.Enabled = true;
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            TreeNode node = new TreeNode("Chart Colors and Styles");
            TreeNode node2 = new TreeNode("Chart Annotations");
            TreeNode node3 = new TreeNode("Bad Tick Filter");
            TreeNode node4 = new TreeNode("Streaming Data", new TreeNode[] { node3 });
            TreeNode node5 = new TreeNode("Performance Visualizers");
            TreeNode node6 = new TreeNode("Commissions");
            TreeNode node7 = new TreeNode("Backtest Settings");
            TreeNode node8 = new TreeNode("Slippage and Round Lots");
            TreeNode node9 = new TreeNode("Sounds");
            TreeNode node10 = new TreeNode("Advanced Options");
            TreeNode node11 = new TreeNode("Trading");
            TreeNode node12 = new TreeNode("Email Settings");
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PreferencesForm));
            this.toolbar = new ToolStrip();
            this.pnlBottom = new Panel();
            this.btnApply = new Button();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.cbApplyCharts = new CheckBox();
            this.split = new SplitContainer();
            this.tree = new TreeView();
            this.grpDecimalRoundingOnOrder = new GroupBox();
            this.cbNoDecimalRoundingForLimitStopPrice = new CheckBox();
            this.pnlTtrading = new Panel();
            this.grpThresholds = new GroupBox();
            this.numBuyingPowerThreshold = new NumEdit();
            this.numCashThreshold = new NumEdit();
            this.cbBuyingPowerThreshold = new CheckBox();
            this.cbCashThreshold = new CheckBox();
            this.lblTradingThresholds = new Label();
            this.grpTradingOptions = new GroupBox();
            this.cbDisablePortfolioSynch = new CheckBox();
            this.lblSamebar = new Label();
            this.cbSameBarExit = new CheckBox();
            this.lblExitAllWarning = new Label();
            this.cbExitAll = new CheckBox();
            this.grpDefault = new GroupBox();
            this.cmbDefaultAccount = new ComboBox();
            this.lblDefault = new Label();
            this.pnlSlippage = new Panel();
            this.grpRoundLots = new GroupBox();
            this.cbRound50 = new CheckBox();
            this.cbRoundLots = new CheckBox();
            this.grpSlippage = new GroupBox();
            this.lblFuturesSlippage = new Label();
            this.numFuturesSlippage = new NumericUpDown();
            this.lblFutures = new Label();
            this.lblSlippageEquities = new Label();
            this.numSlippage = new NumericUpDown();
            this.lblSlippage = new Label();
            this.cbLimitSlippage = new CheckBox();
            this.cbSlippage = new CheckBox();
            this.pnlEA = new Panel();
            this.groupBox2 = new GroupBox();
            this.lblPassword = new Label();
            this.chkSSL = new CheckBox();
            this.txtPassword = new TextBox();
            this.chkAuthenticateWithPassword = new CheckBox();
            this.txtUserID = new TextBox();
            this.lblUserID = new Label();
            this.grpEmailProperties = new GroupBox();
            this.txtEmailAddresses = new TextBox();
            this.txtSMTPPort = new TextBox();
            this.lblEmailAddresses = new Label();
            this.txtSMTPHost = new TextBox();
            this.lblSMTPHost = new Label();
            this.lblSMTPPort = new Label();
            this.grpTestMail = new GroupBox();
            this.btnTestMail = new Button();
            this.pnlCommissions = new Panel();
            this.grpTestCommission = new GroupBox();
            this.btnCalculate = new Button();
            this.lblCommTest = new Label();
            this.lblCommResult = new Label();
            this.numCommPrice = new NumericUpDown();
            this.lblCommPrice = new Label();
            this.cmbCommOrder = new ComboBox();
            this.lblCommOrder = new Label();
            this.numCommShares = new NumericUpDown();
            this.lblCommShares = new Label();
            this.cmbCommAction = new ComboBox();
            this.lblCommAction = new Label();
            this.grpCommissionDesc = new GroupBox();
            this.lblCommissionDesc = new Label();
            this.grpCommission = new GroupBox();
            this.linkSettings = new LinkLabel();
            this.lbCommission = new ListBox();
            this.cbCommission = new CheckBox();
            this.pnlChartAnnotations = new Panel();
            this.btnRemoveFund = new Button();
            this.btnAddFund = new Button();
            this.lvFundAvailable = new ListView();
            this.columnHeader_2 = new ColumnHeader();
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.lvFundSelected = new ListView();
            this.columnHeader_3 = new ColumnHeader();
            this.lblChartAnnotations = new Label();
            this.pnlCS = new Panel();
            this.grpChartFont = new GroupBox();
            this.btnChangeSymbolFont = new Button();
            this.lblTitleFontSample = new Label();
            this.lblTitleFont = new Label();
            this.btnChangeFont = new Button();
            this.lblSampleText = new Label();
            this.lblAxisFont = new Label();
            this.grpTooltips = new GroupBox();
            this.cbFundamentalTooltip = new CheckBox();
            this.cbIndicatorTooltip = new CheckBox();
            this.cbPriceTooltip = new CheckBox();
            this.grpOptions = new GroupBox();
            this.cbPaneSeparators = new CheckBox();
            this.cbVerticalGridlines = new CheckBox();
            this.cbHorizontalGridlines = new CheckBox();
            this.grpChartColors = new GroupBox();
            this.colorPaneSep = new ColorPickerPanel();
            this.lblPaneSepColor = new Label();
            this.colorBottomMargin = new ColorPickerPanel();
            this.lblBottomMarginColor = new Label();
            this.colorRightMargin = new ColorPickerPanel();
            this.lblRightMarginColor = new Label();
            this.colorDownVolume = new ColorPickerPanel();
            this.lblDownVolumeColor = new Label();
            this.colorUpVolume = new ColorPickerPanel();
            this.lblUpVolume = new Label();
            this.colorGridlines = new ColorPickerPanel();
            this.lblGridlineColor = new Label();
            this.colorDownBars = new ColorPickerPanel();
            this.lblDownBarColor = new Label();
            this.colorUpBars = new ColorPickerPanel();
            this.lblUpBarColor = new Label();
            this.colorBackground = new ColorPickerPanel();
            this.lblChartBackground = new Label();
            this.pnlPV = new Panel();
            this.btnMovePvDown = new Button();
            this.imageList_1 = new ImageList(this.icontainer_0);
            this.btnMovePvUp = new Button();
            this.grpVisDesc = new GroupBox();
            this.lblPVDesc = new Label();
            this.lvPV = new ListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.lblPV = new Label();
            this.pnlBadTickFilter = new Panel();
            this.numBadTick = new NumEdit();
            this.lblBadTickThreshold = new Label();
            this.cbBadTickFilter = new CheckBox();
            this.lblBadTickFilter = new Label();
            this.pnlStreaming = new Panel();
            this.grpStreamingSymbols = new GroupBox();
            this.txtStreaming = new TextBox();
            this.lblTicker = new Label();
            this.grpStreamingProvider = new GroupBox();
            this.lblStreamingProvider = new Label();
            this.lvStreaming = new ListView();
            this.columnHeader_4 = new ColumnHeader();
            this.picStreaming = new PictureBox();
            this.linkStreamingMore = new LinkLabel();
            this.lblStreamingDesc = new Label();
            this.lblStreamingDetails = new Label();
            this.lblStreaming = new Label();
            this.pnlSounds = new Panel();
            this.grpSoundAlerts = new GroupBox();
            this.btnBrowse_RealTimeStrategy = new Button();
            this.txtBoxRealTime_SoundPath = new TextBox();
            this.cbSoundsRealTime = new CheckBox();
            this.btnBrowse_StrategyMonitor = new Button();
            this.btnBrowse_StrategyWindow = new Button();
            this.txtBoxStrategyMonitor_SoundPath = new TextBox();
            this.cbSoundStategyMonitor = new CheckBox();
            this.txtBoxStrategyWindow_SoundPath = new TextBox();
            this.cbSoundStrategyWindow = new CheckBox();
            this.btnBrowse_Quotes = new Button();
            this.txtBoxQuotes_SoundPath = new TextBox();
            this.cbSoundQuotes = new CheckBox();
            this.grpOtherSounds = new GroupBox();
            this.cbSoundsParameters = new CheckBox();
            this.cbSoundsIndicators = new CheckBox();
            this.pnlAdvanced = new Panel();
            this.groupBox5 = new GroupBox();
            this.label2 = new Label();
            this.indicatorDecimalPlaces = new NumericUpDown();
            this.label3 = new Label();
            this.pricingDecimalPlaces = new NumericUpDown();
            this.cbSwitchAccount = new CheckBox();
            this.cbAutoOpenOrders = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.cbPrintDialogOff = new CheckBox();
            this.cbPrintPreviewOff = new CheckBox();
            this.cbShowHome = new CheckBox();
            this.lblHomePageOverride = new Label();
            this.cbRememberParamValues = new CheckBox();
            this.cbRememberPositionSize = new CheckBox();
            this.cbRememberRange = new CheckBox();
            this.cbRememberScale = new CheckBox();
            this.cbRememberData = new CheckBox();
            this.lblRemember = new Label();
            this.cbExpand = new CheckBox();
            this.pnlTradeSim = new Panel();
            this.groupBox4 = new GroupBox();
            this.txtBHSymbol = new TextBox();
            this.label1 = new Label();
            this.cbBenchmarkBH = new CheckBox();
            this.groupBox3 = new GroupBox();
            this.cbWorstTradeSimulation = new CheckBox();
            this.grpLimitShares = new GroupBox();
            this.lblReduceQtyPct = new Label();
            this.numReduceQty = new NumEdit();
            this.cbReduceQty = new CheckBox();
            this.grpLimit = new GroupBox();
            this.lblLimitDays = new Label();
            this.cbLimitDays = new CheckBox();
            this.grpMargin = new GroupBox();
            this.cbDividends = new CheckBox();
            this.numMarginRate = new NumEdit();
            this.lblMarginRate = new Label();
            this.numCashRate = new NumEdit();
            this.lblCashRate = new Label();
            this.cbInterest = new CheckBox();
            this.fontDialog_0 = new FontDialog();
            this.openFileDialog_0 = new OpenFileDialog();
            this.assemblyLoader_0 = new AssemblyLoader(this.icontainer_0);
            this.assemblyLoader_1 = new AssemblyLoader(this.icontainer_0);
            this.fundamentalsLoader_0 = new FundamentalsLoader(this.icontainer_0);
            this.symbolParser_0 = new SymbolParser(this.icontainer_0);
            this.pnlBottom.SuspendLayout();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.grpDecimalRoundingOnOrder.SuspendLayout();
            this.pnlTtrading.SuspendLayout();
            this.grpThresholds.SuspendLayout();
            this.grpTradingOptions.SuspendLayout();
            this.grpDefault.SuspendLayout();
            this.pnlSlippage.SuspendLayout();
            this.grpRoundLots.SuspendLayout();
            this.grpSlippage.SuspendLayout();
            this.numFuturesSlippage.BeginInit();
            this.numSlippage.BeginInit();
            this.pnlEA.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpEmailProperties.SuspendLayout();
            this.grpTestMail.SuspendLayout();
            this.pnlCommissions.SuspendLayout();
            this.grpTestCommission.SuspendLayout();
            this.numCommPrice.BeginInit();
            this.numCommShares.BeginInit();
            this.grpCommissionDesc.SuspendLayout();
            this.grpCommission.SuspendLayout();
            this.pnlChartAnnotations.SuspendLayout();
            this.pnlCS.SuspendLayout();
            this.grpChartFont.SuspendLayout();
            this.grpTooltips.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.grpChartColors.SuspendLayout();
            this.pnlPV.SuspendLayout();
            this.grpVisDesc.SuspendLayout();
            this.pnlBadTickFilter.SuspendLayout();
            this.pnlStreaming.SuspendLayout();
            this.grpStreamingSymbols.SuspendLayout();
            this.grpStreamingProvider.SuspendLayout();
            ((ISupportInitialize) this.picStreaming).BeginInit();
            this.pnlSounds.SuspendLayout();
            this.grpSoundAlerts.SuspendLayout();
            this.grpOtherSounds.SuspendLayout();
            this.pnlAdvanced.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.indicatorDecimalPlaces.BeginInit();
            this.pricingDecimalPlaces.BeginInit();
            this.groupBox1.SuspendLayout();
            this.pnlTradeSim.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpLimitShares.SuspendLayout();
            this.grpLimit.SuspendLayout();
            this.grpMargin.SuspendLayout();
            base.SuspendLayout();
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x22c, 0x19);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "toolStrip1";
            this.pnlBottom.Controls.Add(this.btnApply);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.cbApplyCharts);
            this.pnlBottom.Dock = DockStyle.Bottom;
            this.pnlBottom.Location = new Point(0, 0x1c5);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new Size(0x22c, 30);
            this.pnlBottom.TabIndex = 1;
            this.btnApply.Enabled = false;
            this.btnApply.Location = new Point(0x1dc, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new Size(0x4b, 0x17);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new EventHandler(this.btnApply_Click);
            this.btnOK.Location = new Point(0x130, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x181, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.cbApplyCharts.AutoSize = true;
            this.cbApplyCharts.Checked = true;
            this.cbApplyCharts.CheckState = CheckState.Checked;
            this.cbApplyCharts.Location = new Point(12, 6);
            this.cbApplyCharts.Name = "cbApplyCharts";
            this.cbApplyCharts.Size = new Size(0x120, 0x11);
            this.cbApplyCharts.TabIndex = 0;
            this.cbApplyCharts.Text = "Apply Chart Colors and Style changes to all open Charts";
            this.cbApplyCharts.UseVisualStyleBackColor = true;
            this.split.Dock = DockStyle.Fill;
            this.split.IsSplitterFixed = true;
            this.split.Location = new Point(0, 0x19);
            this.split.Name = "split";
            this.split.Panel1.Controls.Add(this.tree);
            this.split.Panel2.Controls.Add(this.pnlAdvanced);
            this.split.Panel2.Controls.Add(this.pnlTradeSim);
            this.split.Panel2.Controls.Add(this.pnlTtrading);
            this.split.Panel2.Controls.Add(this.pnlSlippage);
            this.split.Panel2.Controls.Add(this.pnlEA);
            this.split.Panel2.Controls.Add(this.pnlCommissions);
            this.split.Panel2.Controls.Add(this.pnlChartAnnotations);
            this.split.Panel2.Controls.Add(this.pnlCS);
            this.split.Panel2.Controls.Add(this.pnlPV);
            this.split.Panel2.Controls.Add(this.pnlBadTickFilter);
            this.split.Panel2.Controls.Add(this.pnlStreaming);
            this.split.Panel2.Controls.Add(this.pnlSounds);
            this.split.Size = new Size(0x22c, 0x1ac);
            this.split.SplitterDistance = 0xb9;
            this.split.TabIndex = 2;
            this.tree.Dock = DockStyle.Fill;
            this.tree.HideSelection = false;
            this.tree.Location = new Point(0, 0);
            this.tree.Name = "tree";
            node.Name = "nodeChartColors";
            node.Tag = "CS";
            node.Text = "Chart Colors and Styles";
            node2.Name = "nodeAnnotations";
            node2.Tag = "CA";
            node2.Text = "Chart Annotations";
            node3.Name = "nodeBadTick";
            node3.Tag = "BTF";
            node3.Text = "Bad Tick Filter";
            node4.Name = "nodeStreaming";
            node4.Tag = "ST";
            node4.Text = "Streaming Data";
            node5.Name = "nodeVisualizers";
            node5.Tag = "PV";
            node5.Text = "Performance Visualizers";
            node6.Name = "nodeCommissions";
            node6.Tag = "CM";
            node6.Text = "Commissions";
            node7.Name = "nodeTradeSim";
            node7.Tag = "TR";
            node7.Text = "Backtest Settings";
            node8.Name = "nodeSlippage";
            node8.Tag = "SL";
            node8.Text = "Slippage and Round Lots";
            node9.Name = "nodeSounds";
            node9.Tag = "SND";
            node9.Text = "Sounds";
            node10.Name = "nodeAdvanced";
            node10.Tag = "AD";
            node10.Text = "Advanced Options";
            node11.Name = "nodeTrading";
            node11.Tag = "TRADING";
            node11.Text = "Trading";
            node12.Name = "nodeEmailAlerts";
            node12.Tag = "EA";
            node12.Text = "Email Settings";
            this.tree.Nodes.AddRange(new TreeNode[] { node, node2, node4, node5, node6, node7, node8, node9, node10, node11, node12 });
            this.tree.ShowPlusMinus = false;
            this.tree.ShowRootLines = false;
            this.tree.Size = new Size(0xb9, 0x1ac);
            this.tree.TabIndex = 0;
            this.tree.AfterSelect += new TreeViewEventHandler(this.tree_AfterSelect);
            this.tree.BeforeSelect += new TreeViewCancelEventHandler(this.tree_BeforeSelect);
            this.pnlTradeSim.Controls.Add(this.grpDecimalRoundingOnOrder);
            this.grpDecimalRoundingOnOrder.Controls.Add(this.cbNoDecimalRoundingForLimitStopPrice);
            this.grpDecimalRoundingOnOrder.Location = new Point(6, 0x17b);
            this.grpDecimalRoundingOnOrder.Name = "grpDecimalRoundingOnOrder";
            this.grpDecimalRoundingOnOrder.Size = new Size(0x15b, 0x29);
            this.grpDecimalRoundingOnOrder.TabIndex = 6;
            this.grpDecimalRoundingOnOrder.TabStop = false;
            this.cbNoDecimalRoundingForLimitStopPrice.Location = new Point(11, 8);
            this.cbNoDecimalRoundingForLimitStopPrice.Name = "cbNoDecimalRoundingForLimitStopPrice";
            this.cbNoDecimalRoundingForLimitStopPrice.Size = new Size(0x13d, 0x20);
            this.cbNoDecimalRoundingForLimitStopPrice.TabIndex = 0;
            this.cbNoDecimalRoundingForLimitStopPrice.Text = "Turn off limit/stop order rounding entirely";
            this.cbNoDecimalRoundingForLimitStopPrice.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.pnlTtrading.Controls.Add(this.grpThresholds);
            this.pnlTtrading.Controls.Add(this.grpTradingOptions);
            this.pnlTtrading.Controls.Add(this.grpDefault);
            this.pnlTtrading.Dock = DockStyle.Fill;
            this.pnlTtrading.Location = new Point(0, 0);
            this.pnlTtrading.Name = "pnlTtrading";
            this.pnlTtrading.Size = new Size(0x16f, 0x1ac);
            this.pnlTtrading.TabIndex = 2;
            this.pnlTtrading.Tag = "TRADING";
            this.grpThresholds.Controls.Add(this.numBuyingPowerThreshold);
            this.grpThresholds.Controls.Add(this.numCashThreshold);
            this.grpThresholds.Controls.Add(this.cbBuyingPowerThreshold);
            this.grpThresholds.Controls.Add(this.cbCashThreshold);
            this.grpThresholds.Controls.Add(this.lblTradingThresholds);
            this.grpThresholds.Location = new Point(6, 0x111);
            this.grpThresholds.Name = "grpThresholds";
            this.grpThresholds.Size = new Size(0x164, 0x72);
            this.grpThresholds.TabIndex = 2;
            this.grpThresholds.TabStop = false;
            this.grpThresholds.Text = "Trading Thresholds";
            this.numBuyingPowerThreshold.InputType = NumEdit.NumEditType.Double;
            this.numBuyingPowerThreshold.Location = new Point(0xc3, 0x4e);
            this.numBuyingPowerThreshold.Name = "numBuyingPowerThreshold";
            this.numBuyingPowerThreshold.Size = new Size(0x93, 20);
            this.numBuyingPowerThreshold.TabIndex = 5;
            this.numBuyingPowerThreshold.TextChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.numCashThreshold.InputType = NumEdit.NumEditType.Double;
            this.numCashThreshold.Location = new Point(0xc3, 0x34);
            this.numCashThreshold.Name = "numCashThreshold";
            this.numCashThreshold.Size = new Size(0x93, 20);
            this.numCashThreshold.TabIndex = 4;
            this.numCashThreshold.TextChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbBuyingPowerThreshold.AutoSize = true;
            this.cbBuyingPowerThreshold.Location = new Point(9, 0x4d);
            this.cbBuyingPowerThreshold.Name = "cbBuyingPowerThreshold";
            this.cbBuyingPowerThreshold.Size = new Size(180, 0x11);
            this.cbBuyingPowerThreshold.TabIndex = 2;
            this.cbBuyingPowerThreshold.Text = "Enable Buying Power Threshold;";
            this.cbBuyingPowerThreshold.UseVisualStyleBackColor = true;
            this.cbBuyingPowerThreshold.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbCashThreshold.AutoSize = true;
            this.cbCashThreshold.Location = new Point(9, 0x36);
            this.cbCashThreshold.Name = "cbCashThreshold";
            this.cbCashThreshold.Size = new Size(0x8b, 0x11);
            this.cbCashThreshold.TabIndex = 1;
            this.cbCashThreshold.Text = "Enable Cash Threshold:";
            this.cbCashThreshold.UseVisualStyleBackColor = true;
            this.cbCashThreshold.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblTradingThresholds.Location = new Point(6, 0x13);
            this.lblTradingThresholds.Name = "lblTradingThresholds";
            this.lblTradingThresholds.Size = new Size(0x152, 0x1f);
            this.lblTradingThresholds.TabIndex = 0;
            this.lblTradingThresholds.Text = "Do not enter new Positions if Cash or Buying Power falls below the values specified below:";
            this.grpTradingOptions.Controls.Add(this.cbDisablePortfolioSynch);
            this.grpTradingOptions.Controls.Add(this.lblSamebar);
            this.grpTradingOptions.Controls.Add(this.cbSameBarExit);
            this.grpTradingOptions.Controls.Add(this.lblExitAllWarning);
            this.grpTradingOptions.Controls.Add(this.cbExitAll);
            this.grpTradingOptions.Location = new Point(6, 0x5c);
            this.grpTradingOptions.Name = "grpTradingOptions";
            this.grpTradingOptions.Size = new Size(0x164, 0xb3);
            this.grpTradingOptions.TabIndex = 1;
            this.grpTradingOptions.TabStop = false;
            this.grpTradingOptions.Text = "Trading Options";
            this.cbDisablePortfolioSynch.AutoSize = true;
            this.cbDisablePortfolioSynch.Location = new Point(10, 0x13);
            this.cbDisablePortfolioSynch.Name = "cbDisablePortfolioSynch";
            this.cbDisablePortfolioSynch.Size = new Size(0x87, 0x11);
            this.cbDisablePortfolioSynch.TabIndex = 3;
            this.cbDisablePortfolioSynch.Text = "Disable Portfolio Synch";
            this.cbDisablePortfolioSynch.UseVisualStyleBackColor = true;
            this.cbDisablePortfolioSynch.CheckedChanged += new EventHandler(this.cbDisablePortfolioSynch_CheckedChanged);
            this.lblSamebar.Location = new Point(10, 0x7f);
            this.lblSamebar.Name = "lblSamebar";
            this.lblSamebar.Size = new Size(0x14e, 0x20);
            this.lblSamebar.TabIndex = 3;
            this.lblSamebar.Text = "(Strategies must set RiskStopLevel to issue a same bar stop exit, and AutoProfitLevel for a same bar limit exit.)";
            this.cbSameBarExit.AutoSize = true;
            this.cbSameBarExit.Location = new Point(10, 0x69);
            this.cbSameBarExit.Name = "cbSameBarExit";
            this.cbSameBarExit.Size = new Size(0xcc, 0x11);
            this.cbSameBarExit.TabIndex = 2;
            this.cbSameBarExit.Text = "Allow Same Bar Exits for Auto-Trading";
            this.cbSameBarExit.UseVisualStyleBackColor = true;
            this.cbSameBarExit.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblExitAllWarning.Location = new Point(9, 0x48);
            this.lblExitAllWarning.Name = "lblExitAllWarning";
            this.lblExitAllWarning.Size = new Size(0x155, 30);
            this.lblExitAllWarning.TabIndex = 1;
            this.lblExitAllWarning.Text = "(Warning: Enabling the option above may result in your actual Positions becoming out of synch with your backtest simulated Positions.)";
            this.cbExitAll.Location = new Point(10, 0x27);
            this.cbExitAll.Name = "cbExitAll";
            this.cbExitAll.Size = new Size(0x14c, 0x22);
            this.cbExitAll.TabIndex = 0;
            this.cbExitAll.Text = "Exit Orders (Sell and Cover) should always exit the full Position currently held on a per trade type basis.";
            this.cbExitAll.UseVisualStyleBackColor = true;
            this.cbExitAll.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.grpDefault.Controls.Add(this.cmbDefaultAccount);
            this.grpDefault.Controls.Add(this.lblDefault);
            this.grpDefault.Location = new Point(6, 7);
            this.grpDefault.Name = "grpDefault";
            this.grpDefault.Size = new Size(0x164, 0x4e);
            this.grpDefault.TabIndex = 0;
            this.grpDefault.TabStop = false;
            this.grpDefault.Text = "Default Account";
            this.cmbDefaultAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDefaultAccount.FormattingEnabled = true;
            this.cmbDefaultAccount.Location = new Point(9, 0x27);
            this.cmbDefaultAccount.Name = "cmbDefaultAccount";
            this.cmbDefaultAccount.Size = new Size(0x14d, 0x15);
            this.cmbDefaultAccount.TabIndex = 1;
            this.cmbDefaultAccount.SelectedIndexChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblDefault.AutoSize = true;
            this.lblDefault.Location = new Point(6, 0x11);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Size = new Size(0x156, 13);
            this.lblDefault.TabIndex = 0;
            this.lblDefault.Text = "Select the Account to use as the default for Strategy generated Orders:";
            this.pnlSlippage.Controls.Add(this.grpRoundLots);
            this.pnlSlippage.Controls.Add(this.grpSlippage);
            this.pnlSlippage.Dock = DockStyle.Fill;
            this.pnlSlippage.Location = new Point(0, 0);
            this.pnlSlippage.Name = "pnlSlippage";
            this.pnlSlippage.Size = new Size(0x16f, 0x1ac);
            this.pnlSlippage.TabIndex = 2;
            this.pnlSlippage.Tag = "SL";
            this.grpRoundLots.Controls.Add(this.cbRound50);
            this.grpRoundLots.Controls.Add(this.cbRoundLots);
            this.grpRoundLots.Location = new Point(3, 220);
            this.grpRoundLots.Name = "grpRoundLots";
            this.grpRoundLots.Size = new Size(0x166, 0x44);
            this.grpRoundLots.TabIndex = 1;
            this.grpRoundLots.TabStop = false;
            this.grpRoundLots.Text = "Round Lots";
            this.cbRound50.AutoSize = true;
            this.cbRound50.Enabled = false;
            this.cbRound50.Location = new Point(9, 0x24);
            this.cbRound50.Name = "cbRound50";
            this.cbRound50.Size = new Size(0x13f, 0x11);
            this.cbRound50.TabIndex = 1;
            this.cbRound50.Text = "Round stock positions of less than 50 shares up to 100 shares";
            this.cbRound50.UseVisualStyleBackColor = true;
            this.cbRound50.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbRoundLots.AutoSize = true;
            this.cbRoundLots.Location = new Point(9, 20);
            this.cbRoundLots.Name = "cbRoundLots";
            this.cbRoundLots.Size = new Size(0xec, 0x11);
            this.cbRoundLots.TabIndex = 0;
            this.cbRoundLots.Text = "Round stock positions to nearest 100 shares";
            this.cbRoundLots.UseVisualStyleBackColor = true;
            this.cbRoundLots.CheckedChanged += new EventHandler(this.cbRoundLots_CheckedChanged);
            this.grpSlippage.Controls.Add(this.lblFuturesSlippage);
            this.grpSlippage.Controls.Add(this.numFuturesSlippage);
            this.grpSlippage.Controls.Add(this.lblFutures);
            this.grpSlippage.Controls.Add(this.lblSlippageEquities);
            this.grpSlippage.Controls.Add(this.numSlippage);
            this.grpSlippage.Controls.Add(this.lblSlippage);
            this.grpSlippage.Controls.Add(this.cbLimitSlippage);
            this.grpSlippage.Controls.Add(this.cbSlippage);
            this.grpSlippage.Location = new Point(4, 4);
            this.grpSlippage.Name = "grpSlippage";
            this.grpSlippage.Size = new Size(0x166, 0xd0);
            this.grpSlippage.TabIndex = 0;
            this.grpSlippage.TabStop = false;
            this.grpSlippage.Text = "Slippage Settings";
            this.lblFuturesSlippage.Location = new Point(0x3d, 0xa7);
            this.lblFuturesSlippage.Name = "lblFuturesSlippage";
            this.lblFuturesSlippage.Size = new Size(0x107, 0x1d);
            this.lblFuturesSlippage.TabIndex = 7;
            this.lblFuturesSlippage.Text = "Tick Slippage.  Entry and exit prices are adjusted by this number of ticks.,";
            this.numFuturesSlippage.Location = new Point(15, 0xa7);
            int[] bits = new int[4];
            bits[0] = 0x186a0;
            this.numFuturesSlippage.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numFuturesSlippage.Minimum = new decimal(numArray2);
            this.numFuturesSlippage.Name = "numFuturesSlippage";
            this.numFuturesSlippage.Size = new Size(0x2b, 20);
            this.numFuturesSlippage.TabIndex = 6;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.numFuturesSlippage.Value = new decimal(numArray3);
            this.numFuturesSlippage.ValueChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblFutures.AutoSize = true;
            this.lblFutures.Location = new Point(12, 0x97);
            this.lblFutures.Name = "lblFutures";
            this.lblFutures.Size = new Size(0x3f, 13);
            this.lblFutures.TabIndex = 5;
            this.lblFutures.Text = "For Futures:";
            this.lblSlippageEquities.AutoSize = true;
            this.lblSlippageEquities.Location = new Point(10, 0x5f);
            this.lblSlippageEquities.Name = "lblSlippageEquities";
            this.lblSlippageEquities.Size = new Size(0x41, 13);
            this.lblSlippageEquities.TabIndex = 4;
            this.lblSlippageEquities.Text = "For Equities:";
            this.numSlippage.DecimalPlaces = 2;
            int[] numArray4 = new int[4];
            numArray4[0] = 1;
            numArray4[3] = 0x20000;
            this.numSlippage.Increment = new decimal(numArray4);
            this.numSlippage.Location = new Point(12, 0x6f);
            int[] numArray5 = new int[4];
            numArray5[0] = 0x3e8;
            this.numSlippage.Maximum = new decimal(numArray5);
            this.numSlippage.Name = "numSlippage";
            this.numSlippage.Size = new Size(0x2e, 20);
            this.numSlippage.TabIndex = 3;
            int[] numArray6 = new int[4];
            numArray6[0] = 10;
            numArray6[3] = 0x20000;
            this.numSlippage.Value = new decimal(numArray6);
            this.numSlippage.ValueChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblSlippage.Location = new Point(0x3d, 110);
            this.lblSlippage.Name = "lblSlippage";
            this.lblSlippage.Size = new Size(0x11c, 0x1f);
            this.lblSlippage.TabIndex = 2;
            this.lblSlippage.Text = "Percentage Slippage.  Entry and exit prices are adjusted by this percentage amount.";
            this.cbLimitSlippage.Enabled = false;
            this.cbLimitSlippage.Location = new Point(9, 0x3b);
            this.cbLimitSlippage.Name = "cbLimitSlippage";
            this.cbLimitSlippage.Size = new Size(0x156, 0x21);
            this.cbLimitSlippage.TabIndex = 1;
            this.cbLimitSlippage.Text = "Activate Slippage for Limit Orders (order will fail to execute if price does not penetrate Slippage-adjusted limit price)";
            this.cbLimitSlippage.UseVisualStyleBackColor = true;
            this.cbLimitSlippage.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbSlippage.Location = new Point(9, 0x18);
            this.cbSlippage.Name = "cbSlippage";
            this.cbSlippage.Size = new Size(0x156, 0x20);
            this.cbSlippage.TabIndex = 0;
            this.cbSlippage.Text = "Activate Slippage for Market, AtClose and Stop Orders (will adversely adjust entry and exit prices)";
            this.cbSlippage.UseVisualStyleBackColor = true;
            this.cbSlippage.CheckedChanged += new EventHandler(this.cbSlippage_CheckedChanged);
            this.pnlEA.Controls.Add(this.groupBox2);
            this.pnlEA.Controls.Add(this.grpEmailProperties);
            this.pnlEA.Controls.Add(this.grpTestMail);
            this.pnlEA.Dock = DockStyle.Fill;
            this.pnlEA.Location = new Point(0, 0);
            this.pnlEA.Name = "pnlEA";
            this.pnlEA.Size = new Size(0x16f, 0x1ac);
            this.pnlEA.TabIndex = 14;
            this.pnlEA.Tag = "EA";
            this.groupBox2.Controls.Add(this.lblPassword);
            this.groupBox2.Controls.Add(this.chkSSL);
            this.groupBox2.Controls.Add(this.txtPassword);
            this.groupBox2.Controls.Add(this.chkAuthenticateWithPassword);
            this.groupBox2.Controls.Add(this.txtUserID);
            this.groupBox2.Controls.Add(this.lblUserID);
            this.groupBox2.Location = new Point(0x11, 0x9f);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x14c, 150);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Authentication Details";
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new Point(4, 0x48);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(0x35, 13);
            this.lblPassword.TabIndex = 13;
            this.lblPassword.Text = "Password";
            this.chkSSL.AutoSize = true;
            this.chkSSL.Location = new Point(8, 0x65);
            this.chkSSL.Name = "chkSSL";
            this.chkSSL.Size = new Size(0x2e, 0x11);
            this.chkSSL.TabIndex = 0x10;
            this.chkSSL.Text = "SSL";
            this.chkSSL.UseVisualStyleBackColor = true;
            this.chkSSL.CheckedChanged += new EventHandler(this.chkSSL_CheckedChanged);
            this.txtPassword.Location = new Point(0x62, 70);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(0x8f, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.TextChanged += new EventHandler(this.txtSMTPHost_TextChanged);
            this.chkAuthenticateWithPassword.AutoSize = true;
            this.chkAuthenticateWithPassword.Location = new Point(7, 0x2f);
            this.chkAuthenticateWithPassword.Name = "chkAuthenticateWithPassword";
            this.chkAuthenticateWithPassword.Size = new Size(0x7e, 0x11);
            this.chkAuthenticateWithPassword.TabIndex = 1;
            this.chkAuthenticateWithPassword.Text = "Login With Password";
            this.chkAuthenticateWithPassword.UseVisualStyleBackColor = true;
            this.chkAuthenticateWithPassword.CheckedChanged += new EventHandler(this.chkAuthenticateWithPassword_CheckedChanged);
            this.txtUserID.Location = new Point(0x61, 0x16);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new Size(0x8f, 20);
            this.txtUserID.TabIndex = 0;
            this.txtUserID.TextChanged += new EventHandler(this.txtSMTPHost_TextChanged);
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new Point(3, 0x19);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new Size(0x2b, 13);
            this.lblUserID.TabIndex = 5;
            this.lblUserID.Text = "User ID";
            this.grpEmailProperties.Controls.Add(this.txtEmailAddresses);
            this.grpEmailProperties.Controls.Add(this.txtSMTPPort);
            this.grpEmailProperties.Controls.Add(this.lblEmailAddresses);
            this.grpEmailProperties.Controls.Add(this.txtSMTPHost);
            this.grpEmailProperties.Controls.Add(this.lblSMTPHost);
            this.grpEmailProperties.Controls.Add(this.lblSMTPPort);
            this.grpEmailProperties.Location = new Point(0x10, 3);
            this.grpEmailProperties.Name = "grpEmailProperties";
            this.grpEmailProperties.Size = new Size(0x14d, 150);
            this.grpEmailProperties.TabIndex = 9;
            this.grpEmailProperties.TabStop = false;
            this.grpEmailProperties.Text = "Email Properties";
            this.txtEmailAddresses.Location = new Point(0x63, 0x59);
            this.txtEmailAddresses.Multiline = true;
            this.txtEmailAddresses.Name = "txtEmailAddresses";
            this.txtEmailAddresses.Size = new Size(0x8f, 0x30);
            this.txtEmailAddresses.TabIndex = 2;
            this.txtEmailAddresses.TextChanged += new EventHandler(this.txtSMTPHost_TextChanged);
            this.txtEmailAddresses.Leave += new EventHandler(this.txtEmailAddresses_Leave);
            this.txtEmailAddresses.Enter += new EventHandler(this.txtEmailAddresses_Enter);
            this.txtEmailAddresses.Validating += new CancelEventHandler(this.txtEmailAddresses_Validating);
            this.txtSMTPPort.Location = new Point(0x63, 0x3a);
            this.txtSMTPPort.Name = "txtSMTPPort";
            this.txtSMTPPort.Size = new Size(0x8f, 20);
            this.txtSMTPPort.TabIndex = 1;
            this.txtSMTPPort.TextChanged += new EventHandler(this.txtSMTPHost_TextChanged);
            this.txtSMTPPort.KeyPress += new KeyPressEventHandler(this.txtSMTPPort_KeyPress);
            this.lblEmailAddresses.AutoSize = true;
            this.lblEmailAddresses.Location = new Point(6, 0x5e);
            this.lblEmailAddresses.Name = "lblEmailAddresses";
            this.lblEmailAddresses.Size = new Size(0x54, 13);
            this.lblEmailAddresses.TabIndex = 7;
            this.lblEmailAddresses.Text = "Email Addresses";
            this.txtSMTPHost.Location = new Point(0x62, 0x1a);
            this.txtSMTPHost.Name = "txtSMTPHost";
            this.txtSMTPHost.Size = new Size(0x8f, 20);
            this.txtSMTPHost.TabIndex = 0;
            this.txtSMTPHost.TextChanged += new EventHandler(this.txtSMTPHost_TextChanged);
            this.lblSMTPHost.AutoSize = true;
            this.lblSMTPHost.Location = new Point(6, 0x1d);
            this.lblSMTPHost.Name = "lblSMTPHost";
            this.lblSMTPHost.Size = new Size(0x3e, 13);
            this.lblSMTPHost.TabIndex = 8;
            this.lblSMTPHost.Text = "SMTP Host";
            this.lblSMTPPort.AutoSize = true;
            this.lblSMTPPort.Location = new Point(6, 0x3d);
            this.lblSMTPPort.Name = "lblSMTPPort";
            this.lblSMTPPort.Size = new Size(0x3b, 13);
            this.lblSMTPPort.TabIndex = 6;
            this.lblSMTPPort.Text = "SMTP Port";
            this.grpTestMail.Controls.Add(this.btnTestMail);
            this.grpTestMail.Location = new Point(0x12, 0x13b);
            this.grpTestMail.Name = "grpTestMail";
            this.grpTestMail.Size = new Size(330, 0x3a);
            this.grpTestMail.TabIndex = 11;
            this.grpTestMail.TabStop = false;
            this.grpTestMail.Text = "Test Mail";
            this.btnTestMail.Location = new Point(0x5e, 15);
            this.btnTestMail.Name = "btnTestMail";
            this.btnTestMail.Size = new Size(0x90, 0x25);
            this.btnTestMail.TabIndex = 0;
            this.btnTestMail.Text = "Send Test Mail";
            this.btnTestMail.UseVisualStyleBackColor = true;
            this.btnTestMail.Click += new EventHandler(this.btnTestMail_Click);
            this.pnlCommissions.Controls.Add(this.grpTestCommission);
            this.pnlCommissions.Controls.Add(this.grpCommissionDesc);
            this.pnlCommissions.Controls.Add(this.grpCommission);
            this.pnlCommissions.Controls.Add(this.cbCommission);
            this.pnlCommissions.Dock = DockStyle.Fill;
            this.pnlCommissions.Location = new Point(0, 0);
            this.pnlCommissions.Name = "pnlCommissions";
            this.pnlCommissions.Size = new Size(0x16f, 0x1ac);
            this.pnlCommissions.TabIndex = 8;
            this.pnlCommissions.Tag = "CM";
            this.grpTestCommission.Controls.Add(this.btnCalculate);
            this.grpTestCommission.Controls.Add(this.lblCommTest);
            this.grpTestCommission.Controls.Add(this.lblCommResult);
            this.grpTestCommission.Controls.Add(this.numCommPrice);
            this.grpTestCommission.Controls.Add(this.lblCommPrice);
            this.grpTestCommission.Controls.Add(this.cmbCommOrder);
            this.grpTestCommission.Controls.Add(this.lblCommOrder);
            this.grpTestCommission.Controls.Add(this.numCommShares);
            this.grpTestCommission.Controls.Add(this.lblCommShares);
            this.grpTestCommission.Controls.Add(this.cmbCommAction);
            this.grpTestCommission.Controls.Add(this.lblCommAction);
            this.grpTestCommission.Location = new Point(0xe7, 0x20);
            this.grpTestCommission.Name = "grpTestCommission";
            this.grpTestCommission.Size = new Size(0x7c, 0xe1);
            this.grpTestCommission.TabIndex = 3;
            this.grpTestCommission.TabStop = false;
            this.grpTestCommission.Text = "Test Commission";
            this.btnCalculate.Location = new Point(10, 0xc2);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new Size(0x6b, 0x17);
            this.btnCalculate.TabIndex = 10;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new EventHandler(this.cmbCommAction_SelectedIndexChanged);
            this.lblCommTest.AutoSize = true;
            this.lblCommTest.ForeColor = Color.Red;
            this.lblCommTest.Location = new Point(7, 0xa7);
            this.lblCommTest.Name = "lblCommTest";
            this.lblCommTest.Size = new Size(0x22, 13);
            this.lblCommTest.TabIndex = 9;
            this.lblCommTest.Text = "$0.00";
            this.lblCommTest.Click += new EventHandler(this.cmbCommAction_SelectedIndexChanged);
            this.lblCommResult.AutoSize = true;
            this.lblCommResult.Location = new Point(5, 0x91);
            this.lblCommResult.Name = "lblCommResult";
            this.lblCommResult.Size = new Size(0x70, 13);
            this.lblCommResult.TabIndex = 8;
            this.lblCommResult.Text = "Resulting Commission:";
            this.lblCommResult.Click += new EventHandler(this.cmbCommAction_SelectedIndexChanged);
            this.numCommPrice.DecimalPlaces = 2;
            this.numCommPrice.Location = new Point(0x37, 0x66);
            int[] numArray7 = new int[4];
            numArray7[0] = 0xf4240;
            this.numCommPrice.Maximum = new decimal(numArray7);
            int[] numArray8 = new int[4];
            numArray8[0] = 1;
            numArray8[3] = 0x20000;
            this.numCommPrice.Minimum = new decimal(numArray8);
            this.numCommPrice.Name = "numCommPrice";
            this.numCommPrice.Size = new Size(0x3e, 20);
            this.numCommPrice.TabIndex = 7;
            int[] numArray9 = new int[4];
            numArray9[0] = 1;
            this.numCommPrice.Value = new decimal(numArray9);
            this.numCommPrice.ValueChanged += new EventHandler(this.cmbCommAction_SelectedIndexChanged);
            this.lblCommPrice.AutoSize = true;
            this.lblCommPrice.Location = new Point(6, 0x66);
            this.lblCommPrice.Name = "lblCommPrice";
            this.lblCommPrice.Size = new Size(0x22, 13);
            this.lblCommPrice.TabIndex = 6;
            this.lblCommPrice.Text = "Price:";
            this.cmbCommOrder.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCommOrder.FormattingEnabled = true;
            this.cmbCommOrder.Items.AddRange(new object[] { "Market", "Limit", "Stop", "AtClose" });
            this.cmbCommOrder.Location = new Point(0x37, 0x4e);
            this.cmbCommOrder.Name = "cmbCommOrder";
            this.cmbCommOrder.Size = new Size(0x3e, 0x15);
            this.cmbCommOrder.TabIndex = 5;
            this.cmbCommOrder.SelectedIndexChanged += new EventHandler(this.cmbCommAction_SelectedIndexChanged);
            this.lblCommOrder.AutoSize = true;
            this.lblCommOrder.Location = new Point(6, 0x4e);
            this.lblCommOrder.Name = "lblCommOrder";
            this.lblCommOrder.Size = new Size(0x24, 13);
            this.lblCommOrder.TabIndex = 4;
            this.lblCommOrder.Text = "Order:";
            this.numCommShares.Location = new Point(0x37, 0x37);
            int[] numArray10 = new int[4];
            numArray10[0] = 0xf4240;
            this.numCommShares.Maximum = new decimal(numArray10);
            int[] numArray11 = new int[4];
            numArray11[0] = 1;
            this.numCommShares.Minimum = new decimal(numArray11);
            this.numCommShares.Name = "numCommShares";
            this.numCommShares.Size = new Size(0x3e, 20);
            this.numCommShares.TabIndex = 3;
            int[] numArray12 = new int[4];
            numArray12[0] = 1;
            this.numCommShares.Value = new decimal(numArray12);
            this.numCommShares.ValueChanged += new EventHandler(this.cmbCommAction_SelectedIndexChanged);
            this.lblCommShares.AutoSize = true;
            this.lblCommShares.Location = new Point(6, 0x37);
            this.lblCommShares.Name = "lblCommShares";
            this.lblCommShares.Size = new Size(0x2b, 13);
            this.lblCommShares.TabIndex = 2;
            this.lblCommShares.Text = "Shares:";
            this.cmbCommAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCommAction.FormattingEnabled = true;
            this.cmbCommAction.Items.AddRange(new object[] { "Buy", "Sell", "Short", "Cover" });
            this.cmbCommAction.Location = new Point(0x37, 0x1f);
            this.cmbCommAction.Name = "cmbCommAction";
            this.cmbCommAction.Size = new Size(0x3e, 0x15);
            this.cmbCommAction.TabIndex = 1;
            this.cmbCommAction.SelectedIndexChanged += new EventHandler(this.cmbCommAction_SelectedIndexChanged);
            this.lblCommAction.AutoSize = true;
            this.lblCommAction.Location = new Point(6, 0x1f);
            this.lblCommAction.Name = "lblCommAction";
            this.lblCommAction.Size = new Size(40, 13);
            this.lblCommAction.TabIndex = 0;
            this.lblCommAction.Text = "Action:";
            this.grpCommissionDesc.Controls.Add(this.lblCommissionDesc);
            this.grpCommissionDesc.Location = new Point(10, 0x108);
            this.grpCommissionDesc.Name = "grpCommissionDesc";
            this.grpCommissionDesc.Size = new Size(0x159, 0x6d);
            this.grpCommissionDesc.TabIndex = 2;
            this.grpCommissionDesc.TabStop = false;
            this.grpCommissionDesc.Text = "Commission Description";
            this.lblCommissionDesc.Location = new Point(10, 20);
            this.lblCommissionDesc.Name = "lblCommissionDesc";
            this.lblCommissionDesc.Size = new Size(0x149, 0x4d);
            this.lblCommissionDesc.TabIndex = 0;
            this.grpCommission.Controls.Add(this.linkSettings);
            this.grpCommission.Controls.Add(this.lbCommission);
            this.grpCommission.Location = new Point(10, 0x20);
            this.grpCommission.Name = "grpCommission";
            this.grpCommission.Size = new Size(0xd7, 0xe1);
            this.grpCommission.TabIndex = 1;
            this.grpCommission.TabStop = false;
            this.grpCommission.Text = "Select a Commission Structure";
            this.linkSettings.AutoSize = true;
            this.linkSettings.Location = new Point(0x67, 0xc7);
            this.linkSettings.Name = "linkSettings";
            this.linkSettings.Size = new Size(0x67, 13);
            this.linkSettings.TabIndex = 1;
            this.linkSettings.TabStop = true;
            this.linkSettings.Text = "Commission Settings";
            this.linkSettings.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkSettings_LinkClicked);
            this.lbCommission.FormattingEnabled = true;
            this.lbCommission.Location = new Point(10, 0x13);
            this.lbCommission.Name = "lbCommission";
            this.lbCommission.Size = new Size(0xc4, 0xad);
            this.lbCommission.Sorted = true;
            this.lbCommission.TabIndex = 0;
            this.lbCommission.SelectedIndexChanged += new EventHandler(this.lbCommission_SelectedIndexChanged);
            this.cbCommission.AutoSize = true;
            this.cbCommission.Location = new Point(10, 8);
            this.cbCommission.Name = "cbCommission";
            this.cbCommission.Size = new Size(0xce, 0x11);
            this.cbCommission.TabIndex = 0;
            this.cbCommission.Text = "Apply Commissions to simulated trades";
            this.cbCommission.UseVisualStyleBackColor = true;
            this.cbCommission.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.pnlChartAnnotations.Controls.Add(this.btnRemoveFund);
            this.pnlChartAnnotations.Controls.Add(this.btnAddFund);
            this.pnlChartAnnotations.Controls.Add(this.lvFundAvailable);
            this.pnlChartAnnotations.Controls.Add(this.lvFundSelected);
            this.pnlChartAnnotations.Controls.Add(this.lblChartAnnotations);
            this.pnlChartAnnotations.Dock = DockStyle.Fill;
            this.pnlChartAnnotations.Location = new Point(0, 0);
            this.pnlChartAnnotations.Name = "pnlChartAnnotations";
            this.pnlChartAnnotations.Size = new Size(0x16f, 0x1ac);
            this.pnlChartAnnotations.TabIndex = 5;
            this.pnlChartAnnotations.Tag = "CA";
            this.btnRemoveFund.Location = new Point(0xa8, 0x52);
            this.btnRemoveFund.Name = "btnRemoveFund";
            this.btnRemoveFund.Size = new Size(0x1b, 0x17);
            this.btnRemoveFund.TabIndex = 7;
            this.btnRemoveFund.Text = "<";
            this.btnRemoveFund.UseVisualStyleBackColor = true;
            this.btnRemoveFund.Click += new EventHandler(this.lvFundSelected_DoubleClick);
            this.btnAddFund.Location = new Point(0xa8, 0x39);
            this.btnAddFund.Name = "btnAddFund";
            this.btnAddFund.Size = new Size(0x1b, 0x17);
            this.btnAddFund.TabIndex = 6;
            this.btnAddFund.Text = ">";
            this.btnAddFund.UseVisualStyleBackColor = true;
            this.btnAddFund.Click += new EventHandler(this.lvFundAvailable_DoubleClick);
            this.lvFundAvailable.Columns.AddRange(new ColumnHeader[] { this.columnHeader_2 });
            this.lvFundAvailable.FullRowSelect = true;
            this.lvFundAvailable.HideSelection = false;
            this.lvFundAvailable.Location = new Point(13, 0x38);
            this.lvFundAvailable.Name = "lvFundAvailable";
            this.lvFundAvailable.Size = new Size(0x99, 0x13d);
            this.lvFundAvailable.SmallImageList = this.imageList_0;
            this.lvFundAvailable.TabIndex = 5;
            this.lvFundAvailable.UseCompatibleStateImageBehavior = false;
            this.lvFundAvailable.View = View.Details;
            this.lvFundAvailable.DoubleClick += new EventHandler(this.lvFundAvailable_DoubleClick);
            this.columnHeader_2.Text = "Available Items";
            this.columnHeader_2.Width = 140;
            this.imageList_0.ColorDepth = ColorDepth.Depth8Bit;
            this.imageList_0.ImageSize = new Size(0x10, 0x10);
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.lvFundSelected.Columns.AddRange(new ColumnHeader[] { this.columnHeader_3 });
            this.lvFundSelected.FullRowSelect = true;
            this.lvFundSelected.HideSelection = false;
            this.lvFundSelected.Location = new Point(0xc4, 0x38);
            this.lvFundSelected.Name = "lvFundSelected";
            this.lvFundSelected.Size = new Size(0x99, 0x13d);
            this.lvFundSelected.SmallImageList = this.imageList_0;
            this.lvFundSelected.TabIndex = 4;
            this.lvFundSelected.UseCompatibleStateImageBehavior = false;
            this.lvFundSelected.View = View.Details;
            this.lvFundSelected.DoubleClick += new EventHandler(this.lvFundSelected_DoubleClick);
            this.columnHeader_3.Text = "Selected Items";
            this.columnHeader_3.Width = 140;
            this.lblChartAnnotations.Location = new Point(10, 8);
            this.lblChartAnnotations.Name = "lblChartAnnotations";
            this.lblChartAnnotations.Size = new Size(0x159, 0x2d);
            this.lblChartAnnotations.TabIndex = 0;
            this.lblChartAnnotations.Text = "You can annotate the chart with various Fundamental data points.  Select the Fundamental data items that you want to appear on the chart below.";
            this.pnlCS.Controls.Add(this.grpChartFont);
            this.pnlCS.Controls.Add(this.grpTooltips);
            this.pnlCS.Controls.Add(this.grpOptions);
            this.pnlCS.Controls.Add(this.grpChartColors);
            this.pnlCS.Dock = DockStyle.Fill;
            this.pnlCS.Location = new Point(0, 0);
            this.pnlCS.Name = "pnlCS";
            this.pnlCS.Size = new Size(0x16f, 0x1ac);
            this.pnlCS.TabIndex = 0;
            this.pnlCS.Tag = "CS";
            this.grpChartFont.Controls.Add(this.btnChangeSymbolFont);
            this.grpChartFont.Controls.Add(this.lblTitleFontSample);
            this.grpChartFont.Controls.Add(this.lblTitleFont);
            this.grpChartFont.Controls.Add(this.btnChangeFont);
            this.grpChartFont.Controls.Add(this.lblSampleText);
            this.grpChartFont.Controls.Add(this.lblAxisFont);
            this.grpChartFont.Location = new Point(10, 220);
            this.grpChartFont.Name = "grpChartFont";
            this.grpChartFont.Size = new Size(0x159, 0x51);
            this.grpChartFont.TabIndex = 4;
            this.grpChartFont.TabStop = false;
            this.grpChartFont.Text = "Chart Fonts";
            this.btnChangeSymbolFont.Location = new Point(0xfb, 0x2a);
            this.btnChangeSymbolFont.Name = "btnChangeSymbolFont";
            this.btnChangeSymbolFont.Size = new Size(0x4b, 0x17);
            this.btnChangeSymbolFont.TabIndex = 5;
            this.btnChangeSymbolFont.Text = "Change ...";
            this.btnChangeSymbolFont.UseVisualStyleBackColor = true;
            this.btnChangeSymbolFont.Click += new EventHandler(this.btnChangeSymbolFont_Click);
            this.lblTitleFontSample.AutoSize = true;
            this.lblTitleFontSample.BackColor = Color.White;
            this.lblTitleFontSample.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblTitleFontSample.ForeColor = Color.Black;
            this.lblTitleFontSample.Location = new Point(0x7f, 0x30);
            this.lblTitleFontSample.Name = "lblTitleFontSample";
            this.lblTitleFontSample.Size = new Size(0x3a, 13);
            this.lblTitleFontSample.TabIndex = 4;
            this.lblTitleFontSample.Text = "123456.78";
            this.lblTitleFont.AutoSize = true;
            this.lblTitleFont.Location = new Point(10, 0x30);
            this.lblTitleFont.Name = "lblTitleFont";
            this.lblTitleFont.Size = new Size(0x67, 13);
            this.lblTitleFont.TabIndex = 3;
            this.lblTitleFont.Text = "Symbol && Co. Name:";
            this.btnChangeFont.Location = new Point(0xfb, 13);
            this.btnChangeFont.Name = "btnChangeFont";
            this.btnChangeFont.Size = new Size(0x4b, 0x17);
            this.btnChangeFont.TabIndex = 2;
            this.btnChangeFont.Text = "Change ...";
            this.btnChangeFont.UseVisualStyleBackColor = true;
            this.btnChangeFont.Click += new EventHandler(this.btnChangeFont_Click);
            this.lblSampleText.AutoSize = true;
            this.lblSampleText.BackColor = Color.White;
            this.lblSampleText.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblSampleText.ForeColor = Color.Black;
            this.lblSampleText.Location = new Point(0x7f, 20);
            this.lblSampleText.Name = "lblSampleText";
            this.lblSampleText.Size = new Size(0x3a, 13);
            this.lblSampleText.TabIndex = 1;
            this.lblSampleText.Text = "123456.78";
            this.lblAxisFont.AutoSize = true;
            this.lblAxisFont.Location = new Point(10, 20);
            this.lblAxisFont.Name = "lblAxisFont";
            this.lblAxisFont.Size = new Size(0x4e, 13);
            this.lblAxisFont.TabIndex = 0;
            this.lblAxisFont.Text = "Axis && Margins:";
            this.grpTooltips.Controls.Add(this.cbFundamentalTooltip);
            this.grpTooltips.Controls.Add(this.cbIndicatorTooltip);
            this.grpTooltips.Controls.Add(this.cbPriceTooltip);
            this.grpTooltips.Location = new Point(0xb7, 0x76);
            this.grpTooltips.Name = "grpTooltips";
            this.grpTooltips.Size = new Size(0xac, 0x5f);
            this.grpTooltips.TabIndex = 3;
            this.grpTooltips.TabStop = false;
            this.grpTooltips.Text = "Tooltips";
            this.cbFundamentalTooltip.AutoSize = true;
            this.cbFundamentalTooltip.Location = new Point(13, 0x44);
            this.cbFundamentalTooltip.Name = "cbFundamentalTooltip";
            this.cbFundamentalTooltip.Size = new Size(0x73, 0x11);
            this.cbFundamentalTooltip.TabIndex = 2;
            this.cbFundamentalTooltip.Text = "Fundamental Items";
            this.cbFundamentalTooltip.UseVisualStyleBackColor = true;
            this.cbFundamentalTooltip.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbIndicatorTooltip.AutoSize = true;
            this.cbIndicatorTooltip.Location = new Point(13, 0x2c);
            this.cbIndicatorTooltip.Name = "cbIndicatorTooltip";
            this.cbIndicatorTooltip.Size = new Size(0x48, 0x11);
            this.cbIndicatorTooltip.TabIndex = 1;
            this.cbIndicatorTooltip.Text = "Indicators";
            this.cbIndicatorTooltip.UseVisualStyleBackColor = true;
            this.cbIndicatorTooltip.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbPriceTooltip.AutoSize = true;
            this.cbPriceTooltip.Location = new Point(13, 20);
            this.cbPriceTooltip.Name = "cbPriceTooltip";
            this.cbPriceTooltip.Size = new Size(0x37, 0x11);
            this.cbPriceTooltip.TabIndex = 0;
            this.cbPriceTooltip.Text = "Prices";
            this.cbPriceTooltip.UseVisualStyleBackColor = true;
            this.cbPriceTooltip.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.grpOptions.Controls.Add(this.cbPaneSeparators);
            this.grpOptions.Controls.Add(this.cbVerticalGridlines);
            this.grpOptions.Controls.Add(this.cbHorizontalGridlines);
            this.grpOptions.Location = new Point(10, 0x76);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new Size(0xa6, 0x5f);
            this.grpOptions.TabIndex = 2;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Chart Options";
            this.cbPaneSeparators.AutoSize = true;
            this.cbPaneSeparators.Location = new Point(10, 0x44);
            this.cbPaneSeparators.Name = "cbPaneSeparators";
            this.cbPaneSeparators.Size = new Size(0x80, 0x11);
            this.cbPaneSeparators.TabIndex = 2;
            this.cbPaneSeparators.Text = "Pane Separator Lines";
            this.cbPaneSeparators.UseVisualStyleBackColor = true;
            this.cbPaneSeparators.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbVerticalGridlines.AutoSize = true;
            this.cbVerticalGridlines.Location = new Point(10, 0x2c);
            this.cbVerticalGridlines.Name = "cbVerticalGridlines";
            this.cbVerticalGridlines.Size = new Size(0x68, 0x11);
            this.cbVerticalGridlines.TabIndex = 1;
            this.cbVerticalGridlines.Text = "Vertical Gridlines";
            this.cbVerticalGridlines.UseVisualStyleBackColor = true;
            this.cbVerticalGridlines.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbHorizontalGridlines.AutoSize = true;
            this.cbHorizontalGridlines.Location = new Point(10, 20);
            this.cbHorizontalGridlines.Name = "cbHorizontalGridlines";
            this.cbHorizontalGridlines.Size = new Size(0x74, 0x11);
            this.cbHorizontalGridlines.TabIndex = 0;
            this.cbHorizontalGridlines.Text = "Horizontal Gridlines";
            this.cbHorizontalGridlines.UseVisualStyleBackColor = true;
            this.cbHorizontalGridlines.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.grpChartColors.Controls.Add(this.colorPaneSep);
            this.grpChartColors.Controls.Add(this.lblPaneSepColor);
            this.grpChartColors.Controls.Add(this.colorBottomMargin);
            this.grpChartColors.Controls.Add(this.lblBottomMarginColor);
            this.grpChartColors.Controls.Add(this.colorRightMargin);
            this.grpChartColors.Controls.Add(this.lblRightMarginColor);
            this.grpChartColors.Controls.Add(this.colorDownVolume);
            this.grpChartColors.Controls.Add(this.lblDownVolumeColor);
            this.grpChartColors.Controls.Add(this.colorUpVolume);
            this.grpChartColors.Controls.Add(this.lblUpVolume);
            this.grpChartColors.Controls.Add(this.colorGridlines);
            this.grpChartColors.Controls.Add(this.lblGridlineColor);
            this.grpChartColors.Controls.Add(this.colorDownBars);
            this.grpChartColors.Controls.Add(this.lblDownBarColor);
            this.grpChartColors.Controls.Add(this.colorUpBars);
            this.grpChartColors.Controls.Add(this.lblUpBarColor);
            this.grpChartColors.Controls.Add(this.colorBackground);
            this.grpChartColors.Controls.Add(this.lblChartBackground);
            this.grpChartColors.Location = new Point(10, 4);
            this.grpChartColors.Name = "grpChartColors";
            this.grpChartColors.Size = new Size(0x159, 0x6b);
            this.grpChartColors.TabIndex = 0;
            this.grpChartColors.TabStop = false;
            this.grpChartColors.Text = "Chart Colors";
            this.colorPaneSep.Cursor = Cursors.Hand;
            this.colorPaneSep.DrawOutline = true;
            this.colorPaneSep.Location = new Point(0x106, 0x44);
            this.colorPaneSep.Name = "colorPaneSep";
            this.colorPaneSep.OutlineColor = Color.Black;
            this.colorPaneSep.Size = new Size(0x40, 13);
            this.colorPaneSep.TabIndex = 0x11;
            this.colorPaneSep.Text = "colorPickerPanel2";
            this.colorPaneSep.ColorChanged += new EventHandler<EventArgs>(this.cbInterest_CheckedChanged);
            this.lblPaneSepColor.AutoSize = true;
            this.lblPaneSepColor.Location = new Point(0xb1, 0x44);
            this.lblPaneSepColor.Name = "lblPaneSepColor";
            this.lblPaneSepColor.Size = new Size(0x54, 13);
            this.lblPaneSepColor.TabIndex = 0x10;
            this.lblPaneSepColor.Text = "Pane Separator:";
            this.colorBottomMargin.Cursor = Cursors.Hand;
            this.colorBottomMargin.DrawOutline = true;
            this.colorBottomMargin.Location = new Point(0x5c, 0x54);
            this.colorBottomMargin.Name = "colorBottomMargin";
            this.colorBottomMargin.OutlineColor = Color.Black;
            this.colorBottomMargin.Size = new Size(0x40, 13);
            this.colorBottomMargin.TabIndex = 15;
            this.colorBottomMargin.Text = "colorPickerPanel2";
            this.colorBottomMargin.ColorChanged += new EventHandler<EventArgs>(this.cbInterest_CheckedChanged);
            this.lblBottomMarginColor.AutoSize = true;
            this.lblBottomMarginColor.Location = new Point(7, 0x54);
            this.lblBottomMarginColor.Name = "lblBottomMarginColor";
            this.lblBottomMarginColor.Size = new Size(0x4e, 13);
            this.lblBottomMarginColor.TabIndex = 14;
            this.lblBottomMarginColor.Text = "Bottom Margin:";
            this.colorRightMargin.Cursor = Cursors.Hand;
            this.colorRightMargin.DrawOutline = true;
            this.colorRightMargin.Location = new Point(0x5c, 0x44);
            this.colorRightMargin.Name = "colorRightMargin";
            this.colorRightMargin.OutlineColor = Color.Black;
            this.colorRightMargin.Size = new Size(0x40, 13);
            this.colorRightMargin.TabIndex = 13;
            this.colorRightMargin.Text = "colorPickerPanel1";
            this.colorRightMargin.ColorChanged += new EventHandler<EventArgs>(this.method_6);
            this.lblRightMarginColor.AutoSize = true;
            this.lblRightMarginColor.Location = new Point(7, 0x44);
            this.lblRightMarginColor.Name = "lblRightMarginColor";
            this.lblRightMarginColor.Size = new Size(70, 13);
            this.lblRightMarginColor.TabIndex = 12;
            this.lblRightMarginColor.Text = "Right Margin:";
            this.colorDownVolume.Cursor = Cursors.Hand;
            this.colorDownVolume.DrawOutline = true;
            this.colorDownVolume.Location = new Point(0x106, 0x34);
            this.colorDownVolume.Name = "colorDownVolume";
            this.colorDownVolume.OutlineColor = Color.Black;
            this.colorDownVolume.Size = new Size(0x40, 13);
            this.colorDownVolume.TabIndex = 11;
            this.colorDownVolume.Text = "colorPickerPanel2";
            this.colorDownVolume.ColorChanged += new EventHandler<EventArgs>(this.cbInterest_CheckedChanged);
            this.lblDownVolumeColor.AutoSize = true;
            this.lblDownVolumeColor.Location = new Point(0xb1, 0x34);
            this.lblDownVolumeColor.Name = "lblDownVolumeColor";
            this.lblDownVolumeColor.Size = new Size(0x4c, 13);
            this.lblDownVolumeColor.TabIndex = 10;
            this.lblDownVolumeColor.Text = "Down Volume:";
            this.colorUpVolume.Cursor = Cursors.Hand;
            this.colorUpVolume.DrawOutline = true;
            this.colorUpVolume.Location = new Point(0x106, 0x24);
            this.colorUpVolume.Name = "colorUpVolume";
            this.colorUpVolume.OutlineColor = Color.Black;
            this.colorUpVolume.Size = new Size(0x40, 13);
            this.colorUpVolume.TabIndex = 9;
            this.colorUpVolume.Text = "colorPickerPanel1";
            this.colorUpVolume.ColorChanged += new EventHandler<EventArgs>(this.cbInterest_CheckedChanged);
            this.lblUpVolume.AutoSize = true;
            this.lblUpVolume.Location = new Point(0xb1, 0x24);
            this.lblUpVolume.Name = "lblUpVolume";
            this.lblUpVolume.Size = new Size(0x3e, 13);
            this.lblUpVolume.TabIndex = 8;
            this.lblUpVolume.Text = "Up Volume:";
            this.colorGridlines.Cursor = Cursors.Hand;
            this.colorGridlines.DrawOutline = true;
            this.colorGridlines.Location = new Point(0x106, 20);
            this.colorGridlines.Name = "colorGridlines";
            this.colorGridlines.OutlineColor = Color.Black;
            this.colorGridlines.Size = new Size(0x40, 13);
            this.colorGridlines.TabIndex = 7;
            this.colorGridlines.Text = "colorPickerPanel1";
            this.colorGridlines.ColorChanged += new EventHandler<EventArgs>(this.cbInterest_CheckedChanged);
            this.lblGridlineColor.AutoSize = true;
            this.lblGridlineColor.Location = new Point(0xb1, 20);
            this.lblGridlineColor.Name = "lblGridlineColor";
            this.lblGridlineColor.Size = new Size(50, 13);
            this.lblGridlineColor.TabIndex = 6;
            this.lblGridlineColor.Text = "Gridlines:";
            this.colorDownBars.Cursor = Cursors.Hand;
            this.colorDownBars.DrawOutline = true;
            this.colorDownBars.Location = new Point(0x5c, 0x34);
            this.colorDownBars.Name = "colorDownBars";
            this.colorDownBars.OutlineColor = Color.Black;
            this.colorDownBars.Size = new Size(0x40, 13);
            this.colorDownBars.TabIndex = 5;
            this.colorDownBars.Text = "colorPickerPanel2";
            this.colorDownBars.ColorChanged += new EventHandler<EventArgs>(this.cbInterest_CheckedChanged);
            this.lblDownBarColor.AutoSize = true;
            this.lblDownBarColor.Location = new Point(7, 0x34);
            this.lblDownBarColor.Name = "lblDownBarColor";
            this.lblDownBarColor.Size = new Size(0x3e, 13);
            this.lblDownBarColor.TabIndex = 4;
            this.lblDownBarColor.Text = "Down Bars:";
            this.colorUpBars.Cursor = Cursors.Hand;
            this.colorUpBars.DrawOutline = true;
            this.colorUpBars.Location = new Point(0x5c, 0x24);
            this.colorUpBars.Name = "colorUpBars";
            this.colorUpBars.OutlineColor = Color.Black;
            this.colorUpBars.Size = new Size(0x40, 13);
            this.colorUpBars.TabIndex = 3;
            this.colorUpBars.Text = "colorPickerPanel1";
            this.colorUpBars.ColorChanged += new EventHandler<EventArgs>(this.cbInterest_CheckedChanged);
            this.lblUpBarColor.AutoSize = true;
            this.lblUpBarColor.Location = new Point(7, 0x24);
            this.lblUpBarColor.Name = "lblUpBarColor";
            this.lblUpBarColor.Size = new Size(0x30, 13);
            this.lblUpBarColor.TabIndex = 2;
            this.lblUpBarColor.Text = "Up Bars:";
            this.colorBackground.Cursor = Cursors.Hand;
            this.colorBackground.DrawOutline = true;
            this.colorBackground.Location = new Point(0x5c, 20);
            this.colorBackground.Name = "colorBackground";
            this.colorBackground.OutlineColor = Color.Black;
            this.colorBackground.Size = new Size(0x40, 13);
            this.colorBackground.TabIndex = 1;
            this.colorBackground.Text = "colorPickerPanel1";
            this.colorBackground.ColorChanged += new EventHandler<EventArgs>(this.method_2);
            this.lblChartBackground.AutoSize = true;
            this.lblChartBackground.Location = new Point(7, 20);
            this.lblChartBackground.Name = "lblChartBackground";
            this.lblChartBackground.Size = new Size(0x44, 13);
            this.lblChartBackground.TabIndex = 0;
            this.lblChartBackground.Text = "Background:";
            this.pnlPV.Controls.Add(this.btnMovePvDown);
            this.pnlPV.Controls.Add(this.btnMovePvUp);
            this.pnlPV.Controls.Add(this.grpVisDesc);
            this.pnlPV.Controls.Add(this.lvPV);
            this.pnlPV.Controls.Add(this.lblPV);
            this.pnlPV.Dock = DockStyle.Fill;
            this.pnlPV.Location = new Point(0, 0);
            this.pnlPV.Name = "pnlPV";
            this.pnlPV.Size = new Size(0x16f, 0x1ac);
            this.pnlPV.TabIndex = 1;
            this.pnlPV.Tag = "PV";
            this.btnMovePvDown.ImageAlign = ContentAlignment.MiddleLeft;
            this.btnMovePvDown.ImageIndex = 1;
            this.btnMovePvDown.ImageList = this.imageList_1;
            this.btnMovePvDown.Location = new Point(0x6f, 0xff);
            this.btnMovePvDown.Name = "btnMovePvDown";
            this.btnMovePvDown.Size = new Size(0x5f, 0x17);
            this.btnMovePvDown.TabIndex = 4;
            this.btnMovePvDown.Text = "Move Down";
            this.btnMovePvDown.UseVisualStyleBackColor = true;
            this.btnMovePvDown.Click += new EventHandler(this.btnMovePvDown_Click);
            this.imageList_1.ImageStream = (ImageListStreamer) manager.GetObject("imagesButtons.ImageStream");
            this.imageList_1.TransparentColor = Color.Fuchsia;
            this.imageList_1.Images.SetKeyName(0, "MoveUp.bmp");
            this.imageList_1.Images.SetKeyName(1, "MoveDown.bmp");
            this.btnMovePvUp.ImageAlign = ContentAlignment.MiddleLeft;
            this.btnMovePvUp.ImageIndex = 0;
            this.btnMovePvUp.ImageList = this.imageList_1;
            this.btnMovePvUp.Location = new Point(10, 0xff);
            this.btnMovePvUp.Name = "btnMovePvUp";
            this.btnMovePvUp.Size = new Size(0x5f, 0x17);
            this.btnMovePvUp.TabIndex = 0;
            this.btnMovePvUp.Text = "Move Up";
            this.btnMovePvUp.UseVisualStyleBackColor = true;
            this.btnMovePvUp.Click += new EventHandler(this.btnMovePvUp_Click);
            this.grpVisDesc.Controls.Add(this.lblPVDesc);
            this.grpVisDesc.Location = new Point(10, 0x11c);
            this.grpVisDesc.Name = "grpVisDesc";
            this.grpVisDesc.Size = new Size(0x159, 0x5f);
            this.grpVisDesc.TabIndex = 2;
            this.grpVisDesc.TabStop = false;
            this.grpVisDesc.Text = "Visualizer Description";
            this.lblPVDesc.Location = new Point(7, 20);
            this.lblPVDesc.Name = "lblPVDesc";
            this.lblPVDesc.Size = new Size(0x14c, 0x48);
            this.lblPVDesc.TabIndex = 0;
            this.lvPV.CheckBoxes = true;
            this.lvPV.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1 });
            this.lvPV.FullRowSelect = true;
            this.lvPV.HideSelection = false;
            this.lvPV.Location = new Point(10, 0x40);
            this.lvPV.MultiSelect = false;
            this.lvPV.Name = "lvPV";
            this.lvPV.Size = new Size(0x159, 0xbb);
            this.lvPV.TabIndex = 1;
            this.lvPV.UseCompatibleStateImageBehavior = false;
            this.lvPV.View = View.Details;
            this.lvPV.ItemChecked += new ItemCheckedEventHandler(this.lvPV_ItemChecked);
            this.lvPV.SelectedIndexChanged += new EventHandler(this.lvPV_SelectedIndexChanged);
            this.columnHeader_0.Text = "Visualizer Name";
            this.columnHeader_0.Width = 200;
            this.columnHeader_1.Text = "Applies to";
            this.columnHeader_1.Width = 120;
            this.lblPV.Location = new Point(7, 4);
            this.lblPV.Name = "lblPV";
            this.lblPV.Size = new Size(0x163, 0x38);
            this.lblPV.TabIndex = 0;
            this.lblPV.Text = manager.GetString("lblPV.Text");
            this.pnlBadTickFilter.Controls.Add(this.numBadTick);
            this.pnlBadTickFilter.Controls.Add(this.lblBadTickThreshold);
            this.pnlBadTickFilter.Controls.Add(this.cbBadTickFilter);
            this.pnlBadTickFilter.Controls.Add(this.lblBadTickFilter);
            this.pnlBadTickFilter.Dock = DockStyle.Fill;
            this.pnlBadTickFilter.Location = new Point(0, 0);
            this.pnlBadTickFilter.Name = "pnlBadTickFilter";
            this.pnlBadTickFilter.Size = new Size(0x16f, 0x1ac);
            this.pnlBadTickFilter.TabIndex = 13;
            this.pnlBadTickFilter.Tag = "BTF";
            this.numBadTick.InputType = NumEdit.NumEditType.Double;
            this.numBadTick.Location = new Point(0x8b, 0x49);
            this.numBadTick.Name = "numBadTick";
            this.numBadTick.Size = new Size(0x39, 20);
            this.numBadTick.TabIndex = 3;
            this.numBadTick.TextChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblBadTickThreshold.AutoSize = true;
            this.lblBadTickThreshold.Location = new Point(10, 0x4c);
            this.lblBadTickThreshold.Name = "lblBadTickThreshold";
            this.lblBadTickThreshold.Size = new Size(0x7b, 13);
            this.lblBadTickThreshold.TabIndex = 2;
            this.lblBadTickThreshold.Text = " Bad Tick Threshold (%):";
            this.cbBadTickFilter.AutoSize = true;
            this.cbBadTickFilter.Location = new Point(13, 0x30);
            this.cbBadTickFilter.Name = "cbBadTickFilter";
            this.cbBadTickFilter.Size = new Size(130, 0x11);
            this.cbBadTickFilter.TabIndex = 1;
            this.cbBadTickFilter.Text = "Enable Bad Tick Filter";
            this.cbBadTickFilter.UseVisualStyleBackColor = true;
            this.cbBadTickFilter.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblBadTickFilter.Location = new Point(10, 4);
            this.lblBadTickFilter.Name = "lblBadTickFilter";
            this.lblBadTickFilter.Size = new Size(0x158, 0x2c);
            this.lblBadTickFilter.TabIndex = 0;
            this.lblBadTickFilter.Text = "The Bad Tick Filter option disregards bad incoming ticks in Streaming Charts and the Quotes tool.  A tick is considered \"bad\" if it too far away from the previous tick.";
            this.pnlStreaming.Controls.Add(this.grpStreamingSymbols);
            this.pnlStreaming.Controls.Add(this.grpStreamingProvider);
            this.pnlStreaming.Dock = DockStyle.Fill;
            this.pnlStreaming.Location = new Point(0, 0);
            this.pnlStreaming.Name = "pnlStreaming";
            this.pnlStreaming.Size = new Size(0x16f, 0x1ac);
            this.pnlStreaming.TabIndex = 7;
            this.pnlStreaming.Tag = "ST";
            this.grpStreamingSymbols.Controls.Add(this.txtStreaming);
            this.grpStreamingSymbols.Controls.Add(this.lblTicker);
            this.grpStreamingSymbols.Location = new Point(11, 0x102);
            this.grpStreamingSymbols.Name = "grpStreamingSymbols";
            this.grpStreamingSymbols.Size = new Size(0x158, 100);
            this.grpStreamingSymbols.TabIndex = 1;
            this.grpStreamingSymbols.TabStop = false;
            this.grpStreamingSymbols.Text = "Streaming Ticker Symbols";
            this.txtStreaming.CharacterCasing = CharacterCasing.Upper;
            this.txtStreaming.Location = new Point(0xd3, 0x11);
            this.txtStreaming.Multiline = true;
            this.txtStreaming.Name = "txtStreaming";
            this.txtStreaming.ScrollBars = ScrollBars.Vertical;
            this.txtStreaming.Size = new Size(0x7e, 0x47);
            this.txtStreaming.TabIndex = 1;
            this.txtStreaming.Text = ".DJI, .IXIC, .SPX";
            this.txtStreaming.WordWrap = false;
            this.txtStreaming.TextChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblTicker.Location = new Point(9, 20);
            this.lblTicker.Name = "lblTicker";
            this.lblTicker.Size = new Size(0xc4, 0x44);
            this.lblTicker.TabIndex = 0;
            this.lblTicker.Text = "The Symbol(s) entered here will appear on the main Wealth-Lab status bar, and update during market hours when Streaming data is activated. Separate Symbols with commas.";
            this.grpStreamingProvider.Controls.Add(this.lblStreamingProvider);
            this.grpStreamingProvider.Controls.Add(this.lvStreaming);
            this.grpStreamingProvider.Controls.Add(this.picStreaming);
            this.grpStreamingProvider.Controls.Add(this.linkStreamingMore);
            this.grpStreamingProvider.Controls.Add(this.lblStreamingDesc);
            this.grpStreamingProvider.Controls.Add(this.lblStreamingDetails);
            this.grpStreamingProvider.Controls.Add(this.lblStreaming);
            this.grpStreamingProvider.Location = new Point(11, 8);
            this.grpStreamingProvider.Name = "grpStreamingProvider";
            this.grpStreamingProvider.Size = new Size(0x159, 0xf1);
            this.grpStreamingProvider.TabIndex = 0;
            this.grpStreamingProvider.TabStop = false;
            this.grpStreamingProvider.Text = "Streaming Data Provider";
            this.lblStreamingProvider.AutoSize = true;
            this.lblStreamingProvider.ForeColor = SystemColors.ActiveCaption;
            this.lblStreamingProvider.Location = new Point(0x77, 0xa7);
            this.lblStreamingProvider.Name = "lblStreamingProvider";
            this.lblStreamingProvider.Size = new Size(0, 13);
            this.lblStreamingProvider.TabIndex = 12;
            this.lvStreaming.CheckBoxes = true;
            this.lvStreaming.Columns.AddRange(new ColumnHeader[] { this.columnHeader_4 });
            this.lvStreaming.FullRowSelect = true;
            this.lvStreaming.HeaderStyle = ColumnHeaderStyle.None;
            this.lvStreaming.HideSelection = false;
            this.lvStreaming.Location = new Point(12, 0x40);
            this.lvStreaming.MultiSelect = false;
            this.lvStreaming.Name = "lvStreaming";
            this.lvStreaming.Size = new Size(0x145, 0x61);
            this.lvStreaming.SmallImageList = this.imageList_1;
            this.lvStreaming.TabIndex = 11;
            this.lvStreaming.UseCompatibleStateImageBehavior = false;
            this.lvStreaming.View = View.Details;
            this.lvStreaming.ItemChecked += new ItemCheckedEventHandler(this.lvStreaming_ItemChecked);
            this.lvStreaming.SelectedIndexChanged += new EventHandler(this.lvStreaming_SelectedIndexChanged);
            this.columnHeader_4.Text = "Provider";
            this.columnHeader_4.Width = 300;
            this.picStreaming.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.picStreaming.Location = new Point(12, 0xa7);
            this.picStreaming.Name = "picStreaming";
            this.picStreaming.Size = new Size(0x10, 0x10);
            this.picStreaming.TabIndex = 10;
            this.picStreaming.TabStop = false;
            this.linkStreamingMore.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.linkStreamingMore.AutoSize = true;
            this.linkStreamingMore.Location = new Point(9, 0xdf);
            this.linkStreamingMore.Name = "linkStreamingMore";
            this.linkStreamingMore.Size = new Size(0x2b, 13);
            this.linkStreamingMore.TabIndex = 9;
            this.linkStreamingMore.TabStop = true;
            this.linkStreamingMore.Text = "More ...";
            this.linkStreamingMore.Visible = false;
            this.lblStreamingDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblStreamingDesc.Location = new Point(0x23, 0xb9);
            this.lblStreamingDesc.Name = "lblStreamingDesc";
            this.lblStreamingDesc.Size = new Size(0x12e, 0x27);
            this.lblStreamingDesc.TabIndex = 8;
            this.lblStreamingDetails.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblStreamingDetails.AutoSize = true;
            this.lblStreamingDetails.Location = new Point(0x1d, 0xa7);
            this.lblStreamingDetails.Name = "lblStreamingDetails";
            this.lblStreamingDetails.Size = new Size(0x54, 13);
            this.lblStreamingDetails.TabIndex = 7;
            this.lblStreamingDetails.Text = "Provider Details:";
            this.lblStreaming.Location = new Point(9, 20);
            this.lblStreaming.Name = "lblStreaming";
            this.lblStreaming.Size = new Size(0x148, 0x2c);
            this.lblStreaming.TabIndex = 0;
            this.lblStreaming.Text = "Select a Streaming Data Provider to use below.  This Provider will be used when Streaming is activated in a Chart or Strategy Window, and in Streaming Quote Windows.";
            this.pnlSounds.Controls.Add(this.grpSoundAlerts);
            this.pnlSounds.Controls.Add(this.grpOtherSounds);
            this.pnlSounds.Dock = DockStyle.Fill;
            this.pnlSounds.Location = new Point(0, 0);
            this.pnlSounds.Name = "pnlSounds";
            this.pnlSounds.Size = new Size(0x16f, 0x1ac);
            this.pnlSounds.TabIndex = 8;
            this.pnlSounds.Tag = "SND";
            this.grpSoundAlerts.Controls.Add(this.btnBrowse_RealTimeStrategy);
            this.grpSoundAlerts.Controls.Add(this.txtBoxRealTime_SoundPath);
            this.grpSoundAlerts.Controls.Add(this.cbSoundsRealTime);
            this.grpSoundAlerts.Controls.Add(this.btnBrowse_StrategyMonitor);
            this.grpSoundAlerts.Controls.Add(this.btnBrowse_StrategyWindow);
            this.grpSoundAlerts.Controls.Add(this.txtBoxStrategyMonitor_SoundPath);
            this.grpSoundAlerts.Controls.Add(this.cbSoundStategyMonitor);
            this.grpSoundAlerts.Controls.Add(this.txtBoxStrategyWindow_SoundPath);
            this.grpSoundAlerts.Controls.Add(this.cbSoundStrategyWindow);
            this.grpSoundAlerts.Controls.Add(this.btnBrowse_Quotes);
            this.grpSoundAlerts.Controls.Add(this.txtBoxQuotes_SoundPath);
            this.grpSoundAlerts.Controls.Add(this.cbSoundQuotes);
            this.grpSoundAlerts.Location = new Point(7, 4);
            this.grpSoundAlerts.Name = "grpSoundAlerts";
            this.grpSoundAlerts.Size = new Size(0x163, 0xdb);
            this.grpSoundAlerts.TabIndex = 0;
            this.grpSoundAlerts.TabStop = false;
            this.grpSoundAlerts.Text = "Alert and Trade Sounds";
            this.btnBrowse_RealTimeStrategy.Location = new Point(0x13e, 0xbb);
            this.btnBrowse_RealTimeStrategy.Name = "btnBrowse_RealTimeStrategy";
            this.btnBrowse_RealTimeStrategy.Size = new Size(0x1d, 0x17);
            this.btnBrowse_RealTimeStrategy.TabIndex = 11;
            this.btnBrowse_RealTimeStrategy.Text = "...";
            this.btnBrowse_RealTimeStrategy.UseVisualStyleBackColor = true;
            this.btnBrowse_RealTimeStrategy.Click += new EventHandler(this.btnBrowse_RealTimeStrategy_Click);
            this.txtBoxRealTime_SoundPath.Location = new Point(6, 0xbd);
            this.txtBoxRealTime_SoundPath.Name = "txtBoxRealTime_SoundPath";
            this.txtBoxRealTime_SoundPath.Size = new Size(0x132, 20);
            this.txtBoxRealTime_SoundPath.TabIndex = 10;
            this.cbSoundsRealTime.AutoSize = true;
            this.cbSoundsRealTime.Checked = true;
            this.cbSoundsRealTime.CheckState = CheckState.Checked;
            this.cbSoundsRealTime.Location = new Point(6, 170);
            this.cbSoundsRealTime.Name = "cbSoundsRealTime";
            this.cbSoundsRealTime.Size = new Size(0xcb, 0x11);
            this.cbSoundsRealTime.TabIndex = 9;
            this.cbSoundsRealTime.Text = "New Bar Arrival in Real-Time Strategy";
            this.cbSoundsRealTime.UseVisualStyleBackColor = true;
            this.cbSoundsRealTime.CheckedChanged += new EventHandler(this.cbSoundsRealTime_CheckedChanged);
            this.btnBrowse_StrategyMonitor.Location = new Point(0x13e, 140);
            this.btnBrowse_StrategyMonitor.Name = "btnBrowse_StrategyMonitor";
            this.btnBrowse_StrategyMonitor.Size = new Size(0x1d, 0x17);
            this.btnBrowse_StrategyMonitor.TabIndex = 8;
            this.btnBrowse_StrategyMonitor.Text = "...";
            this.btnBrowse_StrategyMonitor.UseVisualStyleBackColor = true;
            this.btnBrowse_StrategyMonitor.Click += new EventHandler(this.btnBrowse_StrategyMonitor_Click);
            this.btnBrowse_StrategyWindow.Location = new Point(0x13e, 0x5b);
            this.btnBrowse_StrategyWindow.Name = "btnBrowse_StrategyWindow";
            this.btnBrowse_StrategyWindow.Size = new Size(0x1d, 0x17);
            this.btnBrowse_StrategyWindow.TabIndex = 7;
            this.btnBrowse_StrategyWindow.Text = "...";
            this.btnBrowse_StrategyWindow.UseVisualStyleBackColor = true;
            this.btnBrowse_StrategyWindow.Click += new EventHandler(this.btnBrowse_StrategyWindow_Click);
            this.txtBoxStrategyMonitor_SoundPath.Location = new Point(6, 0x8e);
            this.txtBoxStrategyMonitor_SoundPath.Name = "txtBoxStrategyMonitor_SoundPath";
            this.txtBoxStrategyMonitor_SoundPath.Size = new Size(0x132, 20);
            this.txtBoxStrategyMonitor_SoundPath.TabIndex = 6;
            this.cbSoundStategyMonitor.AutoSize = true;
            this.cbSoundStategyMonitor.Checked = true;
            this.cbSoundStategyMonitor.CheckState = CheckState.Checked;
            this.cbSoundStategyMonitor.Location = new Point(7, 0x7d);
            this.cbSoundStategyMonitor.Name = "cbSoundStategyMonitor";
            this.cbSoundStategyMonitor.Size = new Size(0xc6, 0x11);
            this.cbSoundStategyMonitor.TabIndex = 5;
            this.cbSoundStategyMonitor.Text = "Alert Triggered from Strategy Monitor";
            this.cbSoundStategyMonitor.UseVisualStyleBackColor = true;
            this.cbSoundStategyMonitor.CheckedChanged += new EventHandler(this.cbSoundStategyMonitor_CheckedChanged);
            this.txtBoxStrategyWindow_SoundPath.Location = new Point(6, 0x5c);
            this.txtBoxStrategyWindow_SoundPath.Name = "txtBoxStrategyWindow_SoundPath";
            this.txtBoxStrategyWindow_SoundPath.Size = new Size(0x132, 20);
            this.txtBoxStrategyWindow_SoundPath.TabIndex = 4;
            this.cbSoundStrategyWindow.AutoSize = true;
            this.cbSoundStrategyWindow.Checked = true;
            this.cbSoundStrategyWindow.CheckState = CheckState.Checked;
            this.cbSoundStrategyWindow.Location = new Point(7, 0x4b);
            this.cbSoundStrategyWindow.Name = "cbSoundStrategyWindow";
            this.cbSoundStrategyWindow.Size = new Size(0xca, 0x11);
            this.cbSoundStrategyWindow.TabIndex = 3;
            this.cbSoundStrategyWindow.Text = "Alert Triggered from Strategy Window";
            this.cbSoundStrategyWindow.UseVisualStyleBackColor = true;
            this.cbSoundStrategyWindow.CheckedChanged += new EventHandler(this.cbSoundStrategyWindow_CheckedChanged);
            this.btnBrowse_Quotes.Location = new Point(0x13e, 0x2a);
            this.btnBrowse_Quotes.Name = "btnBrowse_Quotes";
            this.btnBrowse_Quotes.Size = new Size(0x1d, 0x17);
            this.btnBrowse_Quotes.TabIndex = 2;
            this.btnBrowse_Quotes.Text = "...";
            this.btnBrowse_Quotes.UseVisualStyleBackColor = true;
            this.btnBrowse_Quotes.Click += new EventHandler(this.btnBrowse_Quotes_Click);
            this.txtBoxQuotes_SoundPath.Location = new Point(7, 0x2d);
            this.txtBoxQuotes_SoundPath.Name = "txtBoxQuotes_SoundPath";
            this.txtBoxQuotes_SoundPath.Size = new Size(0x132, 20);
            this.txtBoxQuotes_SoundPath.TabIndex = 1;
            this.cbSoundQuotes.AutoSize = true;
            this.cbSoundQuotes.Checked = true;
            this.cbSoundQuotes.CheckState = CheckState.Checked;
            this.cbSoundQuotes.Location = new Point(7, 0x17);
            this.cbSoundQuotes.Name = "cbSoundQuotes";
            this.cbSoundQuotes.Size = new Size(0x9b, 0x11);
            this.cbSoundQuotes.TabIndex = 0;
            this.cbSoundQuotes.Text = "Alert Triggered from Quotes";
            this.cbSoundQuotes.UseVisualStyleBackColor = true;
            this.cbSoundQuotes.CheckedChanged += new EventHandler(this.cbSoundQuotes_CheckedChanged);
            this.grpOtherSounds.Controls.Add(this.cbSoundsParameters);
            this.grpOtherSounds.Controls.Add(this.cbSoundsIndicators);
            this.grpOtherSounds.Location = new Point(7, 0xe2);
            this.grpOtherSounds.Name = "grpOtherSounds";
            this.grpOtherSounds.Size = new Size(0x163, 100);
            this.grpOtherSounds.TabIndex = 1;
            this.grpOtherSounds.TabStop = false;
            this.grpOtherSounds.Text = "Other Sounds";
            this.cbSoundsParameters.AutoSize = true;
            this.cbSoundsParameters.Checked = true;
            this.cbSoundsParameters.CheckState = CheckState.Checked;
            this.cbSoundsParameters.Location = new Point(6, 0x2b);
            this.cbSoundsParameters.Name = "cbSoundsParameters";
            this.cbSoundsParameters.Size = new Size(0xb9, 0x11);
            this.cbSoundsParameters.TabIndex = 1;
            this.cbSoundsParameters.Text = "Changing Parameter Slider values";
            this.cbSoundsParameters.UseVisualStyleBackColor = true;
            this.cbSoundsParameters.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbSoundsIndicators.AutoSize = true;
            this.cbSoundsIndicators.Checked = true;
            this.cbSoundsIndicators.CheckState = CheckState.Checked;
            this.cbSoundsIndicators.Location = new Point(7, 20);
            this.cbSoundsIndicators.Name = "cbSoundsIndicators";
            this.cbSoundsIndicators.Size = new Size(260, 0x11);
            this.cbSoundsIndicators.TabIndex = 0;
            this.cbSoundsIndicators.Text = "Chart interaction (Indicators and Drawing Objects)";
            this.cbSoundsIndicators.UseVisualStyleBackColor = true;
            this.cbSoundsIndicators.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.pnlAdvanced.Controls.Add(this.groupBox5);
            this.pnlAdvanced.Controls.Add(this.cbSwitchAccount);
            this.pnlAdvanced.Controls.Add(this.cbAutoOpenOrders);
            this.pnlAdvanced.Controls.Add(this.groupBox1);
            this.pnlAdvanced.Controls.Add(this.cbShowHome);
            this.pnlAdvanced.Controls.Add(this.lblHomePageOverride);
            this.pnlAdvanced.Controls.Add(this.cbRememberParamValues);
            this.pnlAdvanced.Controls.Add(this.cbRememberPositionSize);
            this.pnlAdvanced.Controls.Add(this.cbRememberRange);
            this.pnlAdvanced.Controls.Add(this.cbRememberScale);
            this.pnlAdvanced.Controls.Add(this.cbRememberData);
            this.pnlAdvanced.Controls.Add(this.lblRemember);
            this.pnlAdvanced.Controls.Add(this.cbExpand);
            this.pnlAdvanced.Dock = DockStyle.Fill;
            this.pnlAdvanced.ForeColor = SystemColors.ControlText;
            this.pnlAdvanced.Location = new Point(0, 0);
            this.pnlAdvanced.Name = "pnlAdvanced";
            this.pnlAdvanced.Size = new Size(0x16f, 0x1ac);
            this.pnlAdvanced.TabIndex = 2;
            this.pnlAdvanced.Tag = "AD";
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.indicatorDecimalPlaces);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.pricingDecimalPlaces);
            this.groupBox5.Location = new Point(5, 0x158);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(0x165, 0x4b);
            this.groupBox5.TabIndex = 0x12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Decimal Places";
            this.label2.Location = new Point(10, 0x11);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x7b, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Pricing Decimal Places";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.indicatorDecimalPlaces.Location = new Point(0x8d, 0x2b);
            int[] numArray13 = new int[4];
            numArray13[0] = 7;
            this.indicatorDecimalPlaces.Maximum = new decimal(numArray13);
            this.indicatorDecimalPlaces.Name = "indicatorDecimalPlaces";
            this.indicatorDecimalPlaces.Size = new Size(0x23, 20);
            this.indicatorDecimalPlaces.TabIndex = 0x11;
            int[] numArray14 = new int[4];
            numArray14[0] = 4;
            this.indicatorDecimalPlaces.Value = new decimal(numArray14);
            this.indicatorDecimalPlaces.ValueChanged += new EventHandler(this.indicatorDecimalPlaces_ValueChanged);
            this.label3.Location = new Point(10, 0x2b);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x81, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Indicator Decimal Places";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.pricingDecimalPlaces.Location = new Point(0x8d, 0x11);
            int[] numArray15 = new int[4];
            numArray15[0] = 7;
            this.pricingDecimalPlaces.Maximum = new decimal(numArray15);
            this.pricingDecimalPlaces.Name = "pricingDecimalPlaces";
            this.pricingDecimalPlaces.Size = new Size(0x23, 20);
            this.pricingDecimalPlaces.TabIndex = 0x10;
            int[] numArray16 = new int[4];
            numArray16[0] = 2;
            this.pricingDecimalPlaces.Value = new decimal(numArray16);
            this.pricingDecimalPlaces.ValueChanged += new EventHandler(this.pricingDecimalPlaces_ValueChanged);
            this.cbSwitchAccount.AutoSize = true;
            this.cbSwitchAccount.Checked = true;
            this.cbSwitchAccount.CheckState = CheckState.Checked;
            this.cbSwitchAccount.Location = new Point(0x17, 0xf6);
            this.cbSwitchAccount.Name = "cbSwitchAccount";
            this.cbSwitchAccount.Size = new Size(0x120, 0x11);
            this.cbSwitchAccount.TabIndex = 13;
            this.cbSwitchAccount.Text = "Also, switch to view the Orders for that Trade's Account";
            this.cbSwitchAccount.UseVisualStyleBackColor = true;
            this.cbSwitchAccount.Click += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbAutoOpenOrders.AutoSize = true;
            this.cbAutoOpenOrders.Checked = true;
            this.cbAutoOpenOrders.CheckState = CheckState.Checked;
            this.cbAutoOpenOrders.Location = new Point(4, 0xdf);
            this.cbAutoOpenOrders.Name = "cbAutoOpenOrders";
            this.cbAutoOpenOrders.Size = new Size(0x133, 0x11);
            this.cbAutoOpenOrders.TabIndex = 12;
            this.cbAutoOpenOrders.Text = "Open the Orders window when a Trade is Staged or Placed";
            this.cbAutoOpenOrders.UseVisualStyleBackColor = true;
            this.cbAutoOpenOrders.CheckStateChanged += new EventHandler(this.cbAutoOpenOrders_CheckStateChanged);
            this.cbAutoOpenOrders.Click += new EventHandler(this.cbInterest_CheckedChanged);
            this.groupBox1.Controls.Add(this.cbPrintDialogOff);
            this.groupBox1.Controls.Add(this.cbPrintPreviewOff);
            this.groupBox1.Location = new Point(5, 270);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x165, 0x44);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Printing";
            this.cbPrintDialogOff.AutoSize = true;
            this.cbPrintDialogOff.Location = new Point(10, 0x27);
            this.cbPrintDialogOff.Name = "cbPrintDialogOff";
            this.cbPrintDialogOff.Size = new Size(0x103, 0x11);
            this.cbPrintDialogOff.TabIndex = 10;
            this.cbPrintDialogOff.Text = "Do not show the print dialog when printing reports";
            this.cbPrintDialogOff.UseVisualStyleBackColor = true;
            this.cbPrintPreviewOff.AutoSize = true;
            this.cbPrintPreviewOff.Location = new Point(10, 0x11);
            this.cbPrintPreviewOff.Name = "cbPrintPreviewOff";
            this.cbPrintPreviewOff.Size = new Size(250, 0x11);
            this.cbPrintPreviewOff.TabIndex = 9;
            this.cbPrintPreviewOff.Text = "Do not show print preview when printing reports";
            this.cbPrintPreviewOff.UseVisualStyleBackColor = true;
            this.cbShowHome.AutoSize = true;
            this.cbShowHome.Checked = true;
            this.cbShowHome.CheckState = CheckState.Checked;
            this.cbShowHome.Location = new Point(4, 4);
            this.cbShowHome.Name = "cbShowHome";
            this.cbShowHome.Size = new Size(0xb6, 0x11);
            this.cbShowHome.TabIndex = 0;
            this.cbShowHome.Text = "Show the Home Page on Startup";
            this.cbShowHome.UseVisualStyleBackColor = true;
            this.cbShowHome.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblHomePageOverride.Location = new Point(4, 0x18);
            this.lblHomePageOverride.Name = "lblHomePageOverride";
            this.lblHomePageOverride.Size = new Size(0x160, 0x2d);
            this.lblHomePageOverride.TabIndex = 8;
            this.lblHomePageOverride.Text = "This option is overridden if you have saved a default Workspace.  The Home Page will then be shown if it was in the Workspace that you saved as default.";
            this.cbRememberParamValues.AutoSize = true;
            this.cbRememberParamValues.Location = new Point(0x17, 0xc4);
            this.cbRememberParamValues.Name = "cbRememberParamValues";
            this.cbRememberParamValues.Size = new Size(0x8a, 0x11);
            this.cbRememberParamValues.TabIndex = 7;
            this.cbRememberParamValues.Text = "Parameter Slider Values";
            this.cbRememberParamValues.UseVisualStyleBackColor = true;
            this.cbRememberParamValues.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbRememberPositionSize.AutoSize = true;
            this.cbRememberPositionSize.Location = new Point(0x17, 0xb5);
            this.cbRememberPositionSize.Name = "cbRememberPositionSize";
            this.cbRememberPositionSize.Size = new Size(0x56, 0x11);
            this.cbRememberPositionSize.TabIndex = 6;
            this.cbRememberPositionSize.Text = "Position Size";
            this.cbRememberPositionSize.UseVisualStyleBackColor = true;
            this.cbRememberPositionSize.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbRememberRange.AutoSize = true;
            this.cbRememberRange.Location = new Point(0x17, 0xa6);
            this.cbRememberRange.Name = "cbRememberRange";
            this.cbRememberRange.Size = new Size(0x54, 0x11);
            this.cbRememberRange.TabIndex = 5;
            this.cbRememberRange.Text = "Data Range";
            this.cbRememberRange.UseVisualStyleBackColor = true;
            this.cbRememberRange.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbRememberScale.AutoSize = true;
            this.cbRememberScale.Location = new Point(0x17, 150);
            this.cbRememberScale.Name = "cbRememberScale";
            this.cbRememberScale.Size = new Size(0x4f, 0x11);
            this.cbRememberScale.TabIndex = 4;
            this.cbRememberScale.Text = "Data Scale";
            this.cbRememberScale.UseVisualStyleBackColor = true;
            this.cbRememberScale.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.cbRememberData.AutoSize = true;
            this.cbRememberData.Location = new Point(0x17, 0x86);
            this.cbRememberData.Name = "cbRememberData";
            this.cbRememberData.Size = new Size(0x68, 0x11);
            this.cbRememberData.TabIndex = 3;
            this.cbRememberData.Text = "DataSet/Symbol";
            this.cbRememberData.UseVisualStyleBackColor = true;
            this.cbRememberData.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblRemember.Location = new Point(4, 100);
            this.lblRemember.Name = "lblRemember";
            this.lblRemember.Size = new Size(0x159, 0x1c);
            this.lblRemember.TabIndex = 2;
            this.lblRemember.Text = "Save the following items with a Strategy, and remember them the next time the Strategy is opened:";
            this.cbExpand.AutoSize = true;
            this.cbExpand.Checked = true;
            this.cbExpand.CheckState = CheckState.Checked;
            this.cbExpand.Location = new Point(4, 0x4c);
            this.cbExpand.Name = "cbExpand";
            this.cbExpand.Size = new Size(0xd3, 0x11);
            this.cbExpand.TabIndex = 1;
            this.cbExpand.Text = "Expand first DataSet in Tree on Startup";
            this.cbExpand.UseVisualStyleBackColor = true;
            this.cbExpand.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.pnlTradeSim.Controls.Add(this.groupBox4);
            this.pnlTradeSim.Controls.Add(this.groupBox3);
            this.pnlTradeSim.Controls.Add(this.grpLimitShares);
            this.pnlTradeSim.Controls.Add(this.grpLimit);
            this.pnlTradeSim.Controls.Add(this.grpMargin);
            this.pnlTradeSim.Dock = DockStyle.Fill;
            this.pnlTradeSim.Location = new Point(0, 0);
            this.pnlTradeSim.Name = "pnlTradeSim";
            this.pnlTradeSim.Size = new Size(0x16f, 0x1ac);
            this.pnlTradeSim.TabIndex = 2;
            this.pnlTradeSim.Tag = "TR";
            this.groupBox4.Controls.Add(this.txtBHSymbol);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.cbBenchmarkBH);
            this.groupBox4.Location = new Point(6, 0x153);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0x15b, 0x27);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.txtBHSymbol.Location = new Point(0xe2, 12);
            this.txtBHSymbol.Name = "txtBHSymbol";
            this.txtBHSymbol.Size = new Size(0x30, 20);
            this.txtBHSymbol.TabIndex = 2;
            this.txtBHSymbol.TextChanged += new EventHandler(this.txtBHSymbol_TextChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(170, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Symbol";
            this.cbBenchmarkBH.Location = new Point(11, 8);
            this.cbBenchmarkBH.Name = "cbBenchmarkBH";
            this.cbBenchmarkBH.Size = new Size(0x98, 0x1c);
            this.cbBenchmarkBH.TabIndex = 0;
            this.cbBenchmarkBH.Text = "Benchmark Buy && Hold";
            this.cbBenchmarkBH.UseVisualStyleBackColor = true;
            this.cbBenchmarkBH.CheckedChanged += new EventHandler(this.cbBenchmarkBH_CheckedChanged);
            this.groupBox3.Controls.Add(this.cbWorstTradeSimulation);
            this.groupBox3.Location = new Point(6, 0x129);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x15b, 0x29);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.cbWorstTradeSimulation.Location = new Point(11, 8);
            this.cbWorstTradeSimulation.Name = "cbWorstTradeSimulation";
            this.cbWorstTradeSimulation.Size = new Size(0x141, 0x1c);
            this.cbWorstTradeSimulation.TabIndex = 0;
            this.cbWorstTradeSimulation.Text = "Use Worst Trades in Portfolio Simulation.";
            this.cbWorstTradeSimulation.UseVisualStyleBackColor = true;
            this.grpLimitShares.Controls.Add(this.lblReduceQtyPct);
            this.grpLimitShares.Controls.Add(this.numReduceQty);
            this.grpLimitShares.Controls.Add(this.cbReduceQty);
            this.grpLimitShares.Location = new Point(7, 0xf1);
            this.grpLimitShares.Name = "grpLimitShares";
            this.grpLimitShares.Size = new Size(0x15b, 0x36);
            this.grpLimitShares.TabIndex = 4;
            this.grpLimitShares.TabStop = false;
            this.grpLimitShares.Text = "Reduce Quantity based on Volume";
            this.lblReduceQtyPct.AutoSize = true;
            this.lblReduceQtyPct.Location = new Point(0xff, 0x1d);
            this.lblReduceQtyPct.Name = "lblReduceQtyPct";
            this.lblReduceQtyPct.Size = new Size(15, 13);
            this.lblReduceQtyPct.TabIndex = 4;
            this.lblReduceQtyPct.Text = "%";
            this.numReduceQty.InputType = NumEdit.NumEditType.Double;
            this.numReduceQty.Location = new Point(0xc6, 0x18);
            this.numReduceQty.Name = "numReduceQty";
            this.numReduceQty.Size = new Size(0x34, 20);
            this.numReduceQty.TabIndex = 3;
            this.numReduceQty.Text = "10.0";
            this.numReduceQty.TextChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.numReduceQty.Leave += new EventHandler(this.numReduceQty_Leave);
            this.cbReduceQty.Location = new Point(11, 12);
            this.cbReduceQty.Name = "cbReduceQty";
            this.cbReduceQty.Size = new Size(0xb5, 40);
            this.cbReduceQty.TabIndex = 0;
            this.cbReduceQty.Text = "Limit a Position's Quantity to a Percentage of the Bar's Volume:";
            this.cbReduceQty.UseVisualStyleBackColor = true;
            this.cbReduceQty.Click += new EventHandler(this.cbInterest_CheckedChanged);
            this.grpLimit.Controls.Add(this.lblLimitDays);
            this.grpLimit.Controls.Add(this.cbLimitDays);
            this.grpLimit.Location = new Point(8, 0x8e);
            this.grpLimit.Name = "grpLimit";
            this.grpLimit.Size = new Size(0x15b, 0x61);
            this.grpLimit.TabIndex = 3;
            this.grpLimit.TabStop = false;
            this.grpLimit.Text = "Limit-Days Simulation";
            this.lblLimitDays.Location = new Point(6, 0x25);
            this.lblLimitDays.Name = "lblLimitDays";
            this.lblLimitDays.Size = new Size(0x14e, 0x34);
            this.lblLimitDays.TabIndex = 2;
            this.lblLimitDays.Text = manager.GetString("lblLimitDays.Text");
            this.cbLimitDays.AutoSize = true;
            this.cbLimitDays.Location = new Point(7, 0x11);
            this.cbLimitDays.Name = "cbLimitDays";
            this.cbLimitDays.Size = new Size(0x126, 0x11);
            this.cbLimitDays.TabIndex = 1;
            this.cbLimitDays.Text = "Prohibit trades in direction of market moves on Limit Days";
            this.cbLimitDays.UseVisualStyleBackColor = true;
            this.cbLimitDays.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.grpMargin.Controls.Add(this.cbDividends);
            this.grpMargin.Controls.Add(this.numMarginRate);
            this.grpMargin.Controls.Add(this.lblMarginRate);
            this.grpMargin.Controls.Add(this.numCashRate);
            this.grpMargin.Controls.Add(this.lblCashRate);
            this.grpMargin.Controls.Add(this.cbInterest);
            this.grpMargin.Location = new Point(9, 1);
            this.grpMargin.Name = "grpMargin";
            this.grpMargin.Size = new Size(0x15a, 0x8b);
            this.grpMargin.TabIndex = 2;
            this.grpMargin.TabStop = false;
            this.grpMargin.Text = "Interest and Dividends";
            this.cbDividends.Location = new Point(8, 100);
            this.cbDividends.Name = "cbDividends";
            this.cbDividends.Size = new Size(0x13a, 0x21);
            this.cbDividends.TabIndex = 5;
            this.cbDividends.Text = "Apply dividends to backtest results when using Portfolio Simulation mode";
            this.cbDividends.UseVisualStyleBackColor = true;
            this.cbDividends.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.numMarginRate.InputType = NumEdit.NumEditType.Double;
            this.numMarginRate.Location = new Point(210, 0x4d);
            this.numMarginRate.Name = "numMarginRate";
            this.numMarginRate.Size = new Size(0x34, 20);
            this.numMarginRate.TabIndex = 4;
            this.numMarginRate.Text = "7.0";
            this.numMarginRate.TextChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblMarginRate.AutoSize = true;
            this.lblMarginRate.Location = new Point(0x1f, 0x4f);
            this.lblMarginRate.Name = "lblMarginRate";
            this.lblMarginRate.Size = new Size(0xb0, 13);
            this.lblMarginRate.TabIndex = 3;
            this.lblMarginRate.Text = "Interest rate on margin loan amount:";
            this.numCashRate.InputType = NumEdit.NumEditType.Double;
            this.numCashRate.Location = new Point(210, 0x35);
            this.numCashRate.Name = "numCashRate";
            this.numCashRate.Size = new Size(0x34, 20);
            this.numCashRate.TabIndex = 2;
            this.numCashRate.Text = "1.0";
            this.numCashRate.TextChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.lblCashRate.AutoSize = true;
            this.lblCashRate.Location = new Point(0x1f, 0x38);
            this.lblCashRate.Name = "lblCashRate";
            this.lblCashRate.Size = new Size(0x9f, 13);
            this.lblCashRate.TabIndex = 1;
            this.lblCashRate.Text = "Return rate for uninvested cash:";
            this.cbInterest.Location = new Point(8, 0x17);
            this.cbInterest.Name = "cbInterest";
            this.cbInterest.Size = new Size(0x14b, 0x22);
            this.cbInterest.TabIndex = 0;
            this.cbInterest.Text = "Apply interest rates below when running a backtest using Portfolio Simulation mode";
            this.cbInterest.UseVisualStyleBackColor = true;
            this.cbInterest.CheckedChanged += new EventHandler(this.cbInterest_CheckedChanged);
            this.assemblyLoader_0.BaseClass = "Commission";
            this.assemblyLoader_0.DLLNameFilter = "";
            this.assemblyLoader_0.Interface = null;
            this.assemblyLoader_0.Path = null;
            this.assemblyLoader_0.PathMask = "*.dll";
            this.assemblyLoader_1.BaseClass = "StreamingDataProvider";
            this.assemblyLoader_1.DLLNameFilter = "";
            this.assemblyLoader_1.Interface = null;
            this.assemblyLoader_1.Path = null;
            this.assemblyLoader_1.PathMask = "*.dll";
            this.symbolParser_0.Text = null;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x22c, 0x1e3);
            base.Controls.Add(this.split);
            base.Controls.Add(this.pnlBottom);
            base.Controls.Add(this.toolbar);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "PreferencesForm";
            this.Text = "Preferences";
            base.Load += new EventHandler(this.PreferencesForm_Load);
            base.Activated += new EventHandler(this.PreferencesForm_Activated);
            base.FormClosed += new FormClosedEventHandler(this.PreferencesForm_FormClosed);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            this.split.ResumeLayout(false);
            this.pnlTtrading.ResumeLayout(false);
            this.grpThresholds.ResumeLayout(false);
            this.grpThresholds.PerformLayout();
            this.grpTradingOptions.ResumeLayout(false);
            this.grpTradingOptions.PerformLayout();
            this.grpDefault.ResumeLayout(false);
            this.grpDefault.PerformLayout();
            this.pnlSlippage.ResumeLayout(false);
            this.grpRoundLots.ResumeLayout(false);
            this.grpRoundLots.PerformLayout();
            this.grpSlippage.ResumeLayout(false);
            this.grpSlippage.PerformLayout();
            this.numFuturesSlippage.EndInit();
            this.numSlippage.EndInit();
            this.pnlEA.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpEmailProperties.ResumeLayout(false);
            this.grpEmailProperties.PerformLayout();
            this.grpTestMail.ResumeLayout(false);
            this.pnlCommissions.ResumeLayout(false);
            this.pnlCommissions.PerformLayout();
            this.grpTestCommission.ResumeLayout(false);
            this.grpTestCommission.PerformLayout();
            this.numCommPrice.EndInit();
            this.numCommShares.EndInit();
            this.grpCommissionDesc.ResumeLayout(false);
            this.grpCommission.ResumeLayout(false);
            this.grpCommission.PerformLayout();
            this.pnlChartAnnotations.ResumeLayout(false);
            this.pnlCS.ResumeLayout(false);
            this.grpChartFont.ResumeLayout(false);
            this.grpChartFont.PerformLayout();
            this.grpTooltips.ResumeLayout(false);
            this.grpTooltips.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpChartColors.ResumeLayout(false);
            this.grpChartColors.PerformLayout();
            this.pnlPV.ResumeLayout(false);
            this.grpVisDesc.ResumeLayout(false);
            this.pnlBadTickFilter.ResumeLayout(false);
            this.pnlBadTickFilter.PerformLayout();
            this.pnlStreaming.ResumeLayout(false);
            this.grpStreamingSymbols.ResumeLayout(false);
            this.grpStreamingSymbols.PerformLayout();
            this.grpStreamingProvider.ResumeLayout(false);
            this.grpStreamingProvider.PerformLayout();
            ((ISupportInitialize) this.picStreaming).EndInit();
            this.pnlSounds.ResumeLayout(false);
            this.grpSoundAlerts.ResumeLayout(false);
            this.grpSoundAlerts.PerformLayout();
            this.grpOtherSounds.ResumeLayout(false);
            this.grpOtherSounds.PerformLayout();
            this.pnlAdvanced.ResumeLayout(false);
            this.pnlAdvanced.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.indicatorDecimalPlaces.EndInit();
            this.pricingDecimalPlaces.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlTradeSim.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.grpLimitShares.ResumeLayout(false);
            this.grpLimitShares.PerformLayout();
            this.grpLimit.ResumeLayout(false);
            this.grpLimit.PerformLayout();
            this.grpMargin.ResumeLayout(false);
            this.grpMargin.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lbCommission_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lbCommission.SelectedItem != null)
            {
                Commission selectedItem = (Commission) this.lbCommission.SelectedItem;
                this.lblCommissionDesc.Text = selectedItem.Description;
                this.method_4();
                this.linkSettings.Visible = selectedItem is ICustomSettings;
            }
        }

        private void linkSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Commission selectedItem = (Commission) this.lbCommission.SelectedItem;
            ICustomSettings settings = selectedItem as ICustomSettings;
            if (settings != null)
            {
                settings.ReadSettings(MainModule.Instance.Settings);
                ChartSettingsForm form = new ChartSettingsForm {
                    Text = "Commission Settings"
                };
                UserControl settingsUI = settings.GetSettingsUI();
                form.AddSettingsUI(settingsUI);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    settings.ChangeSettings(settingsUI);
                    settings.WriteSettings(MainModule.Instance.Settings);
                    this.btnCalculate.PerformClick();
                }
                this.lbCommission_SelectedIndexChanged(this.lbCommission, EventArgs.Empty);
            }
        }

        private void lvFundAvailable_DoubleClick(object sender, EventArgs e)
        {
            this.method_3(this.lvFundAvailable, this.lvFundSelected);
            this.btnApply.Enabled = true;
        }

        private void lvFundSelected_DoubleClick(object sender, EventArgs e)
        {
            this.method_3(this.lvFundSelected, this.lvFundAvailable);
            this.btnApply.Enabled = true;
        }

        private void lvPV_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            this.btnApply.Enabled = true;
        }

        private void lvPV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvPV.SelectedItems.Count != 0)
            {
                ListViewItem item = this.lvPV.SelectedItems[0];
                if (item != null)
                {
                    IPerformanceVisualizer tag = (IPerformanceVisualizer) item.Tag;
                    this.lblPVDesc.Text = tag.Description;
                }
            }
        }

        private void lvStreaming_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            this.btnApply.Enabled = true;
            if (e.Item.Checked)
            {
                foreach (ListViewItem item in this.lvStreaming.Items)
                {
                    if (item != e.Item)
                    {
                        item.Checked = false;
                    }
                }
            }
        }

        private void lvStreaming_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvStreaming.SelectedItems.Count != 0)
            {
                ListViewItem item = this.lvStreaming.SelectedItems[0];
                StreamingDataProvider tag = (StreamingDataProvider) item.Tag;
                this.lblStreamingProvider.Text = tag.FriendlyName;
                Bitmap glyph = tag.Glyph;
                glyph.MakeTransparent(Color.Fuchsia);
                this.picStreaming.Image = glyph;
                this.lblStreamingDesc.Text = tag.Description;
                this.linkStreamingMore.Visible = tag.URL != "";
            }
        }

        private void method_0()
        {
            if ((this.bool_0 && (this.tree.SelectedNode.Tag.ToString() == "EA")) && !MainModule.Instance.Settings.Get("EmailWarning", false))
            {
                this.bool_1 = false;
                DontShowAgainForm form = new DontShowAgainForm(Application.ProductName, "Your PC/anti-spam or security software/email provider/ISP may filter or delay some messages, especially when sending large numbers of Email Alerts. If you plan to use Auto-Email in this manner, please configure accordingly.") {
                    CancelButtonVisible = true
                };
                form.ShowDialog();
                if (form.DontShowAgain)
                {
                    MainModule.Instance.Settings.Set("EmailWarning", true);
                }
            }
        }

        private void method_1()
        {
            SettingsManager settings = MainModule.Instance.Settings;
            string str = "";
            string str2 = "";
            MainModule.Instance.Visualizers.Clear();
            MainModule.Instance.VisualizersChecked.Clear();
            foreach (ListViewItem item in this.lvPV.Items)
            {
                IPerformanceVisualizer tag = (IPerformanceVisualizer) item.Tag;
                MainModule.Instance.Visualizers.Add(tag);
                if (str != "")
                {
                    str = str + "|";
                }
                str = str + tag.TabText;
                if (item.Checked)
                {
                    MainModule.Instance.VisualizersChecked.Add(tag);
                    if (str2 != "")
                    {
                        str2 = str2 + "|";
                    }
                    str2 = str2 + tag.TabText;
                }
            }
            settings.Set("PVOrder", str);
            settings.Set("PVChecked", str2);
            ChartRenderer renderer = MainModule.Instance.Renderer;
            renderer.BackgroundColor = this.colorBackground.BackColor;
            renderer.UpBarColor = this.colorUpBars.BackColor;
            renderer.DownBarColor = this.colorDownBars.BackColor;
            renderer.UpBarVolumeColor = this.colorUpVolume.BackColor;
            renderer.DownBarVolumeColor = this.colorDownVolume.BackColor;
            renderer.GridlineColor = this.colorGridlines.BackColor;
            renderer.PaneSeparatorColor = this.colorPaneSep.BackColor;
            renderer.MarginRightColor = this.colorRightMargin.BackColor;
            renderer.MarginBottomColor = this.colorBottomMargin.BackColor;
            renderer.AxisFont = this.lblSampleText.Font;
            renderer.TitleFont = this.lblTitleFontSample.Font;
            renderer.HorizontalGridines = this.cbHorizontalGridlines.Checked;
            renderer.VerticalGridlines = this.cbVerticalGridlines.Checked;
            renderer.PaneSeparatorVisible = this.cbPaneSeparators.Checked;
            settings.Set("PriceTooltip", this.cbPriceTooltip.Checked);
            settings.Set("IndicatorTooltip", this.cbIndicatorTooltip.Checked);
            settings.Set("FundamentalTooltip", this.cbFundamentalTooltip.Checked);
            string str3 = "";
            foreach (ListViewItem item3 in this.lvFundSelected.Items)
            {
                str3 = str3 + item3.Text + ";";
            }
            renderer.FundamentalGlyphs = str3;
            if (this.cbApplyCharts.Checked)
            {
                MainModule.Instance.UpdateChartColorsAndStyle();
            }
            MainModule.Instance.Executor.ApplyCommission = this.cbCommission.Checked;
            if (this.lbCommission.SelectedItem != null)
            {
                Commission selectedItem = (Commission) this.lbCommission.SelectedItem;
                MainModule.Instance.Executor.Commission = selectedItem;
                settings.Set("Commission", selectedItem.GetType().Name);
            }
            TradingSystemExecutor executor = MainModule.Instance.Executor;
            executor.EnableSlippage = this.cbSlippage.Checked;
            executor.LimitOrderSlippage = this.cbLimitSlippage.Checked;
            executor.SlippageUnits = (double) this.numSlippage.Value;
            executor.SlippageTicks = (int) this.numFuturesSlippage.Value;
            executor.RoundLots = this.cbRoundLots.Checked;
            executor.RoundLots50 = this.cbRound50.Checked;
            executor.LimitDaySimulation = this.cbLimitDays.Checked;
            executor.ApplyInterest = this.cbInterest.Checked;
            executor.CashRate = (double) this.numCashRate.Value;
            executor.MarginRate = (double) this.numMarginRate.Value;
            executor.ApplyDividends = this.cbDividends.Checked;
            executor.ReduceQtyBasedOnVolume = this.cbReduceQty.Checked;
            executor.NoDecimalRoundingForLimitStopPrice = this.cbNoDecimalRoundingForLimitStopPrice.Checked;
            executor.RedcuceQtyPct = (double) this.numReduceQty.Value;
            executor.WorstTradeSimulation = this.cbWorstTradeSimulation.Checked;
            executor.PricingDecimalPlaces = (int) this.pricingDecimalPlaces.Value;
            executor.BenchmarkSymbol = this.txtBHSymbol.Text;
            executor.BenchmarkBuyAndHoldON = this.cbBenchmarkBH.Checked;
            settings.Set("ShowHomePage", this.cbShowHome.Checked);
            if (HomeForm.Instance != null)
            {
                HomeForm.Instance.SetShowOnStartup(this.cbShowHome.Checked);
            }
            settings.Set("ExpandFirstDataSet", this.cbExpand.Checked);
            settings.Set("RememberStrategyData", this.cbRememberData.Checked);
            settings.Set("RememberStrategyPositionSize", this.cbRememberPositionSize.Checked);
            settings.Set("RememberStrategyRange", this.cbRememberRange.Checked);
            settings.Set("RememberStrategyScale", this.cbRememberScale.Checked);
            settings.Set("RememberParameterSliders", this.cbRememberParamValues.Checked);
            settings.Set("ApplyChartColors", this.cbApplyCharts.Checked);
            settings.Set("HidePrintPreview", this.cbPrintPreviewOff.Checked);
            settings.Set("HidePrintDialog", this.cbPrintDialogOff.Checked);
            settings.Set("AutoOpenOrders", this.cbAutoOpenOrders.Checked);
            settings.Set("SwitchToAccount", this.cbSwitchAccount.Checked);
            settings.Set("ExitFullPosition", this.cbExitAll.Checked);
            settings.Set(DecimalsManager.Instance.IndicatorKey, (int) this.indicatorDecimalPlaces.Value);
            settings.Set(DecimalsManager.Instance.PricingKey, (int) this.pricingDecimalPlaces.Value);
            DecimalsManager.Instance.SetValues(settings);
            settings.Set(TradeManager.DisablePortfolioSynchKey, this.cbDisablePortfolioSynch.Checked);
            TradeManager.DisablePortfolioSynch = this.cbDisablePortfolioSynch.Checked;
            settings.Set("SoundsQuotes", this.cbSoundQuotes.Checked);
            settings.Set("SoundsQuotes_File", this.txtBoxQuotes_SoundPath.Text);
            settings.Set("SoundsStrategyMonitor", this.cbSoundStategyMonitor.Checked);
            settings.Set("SoundsStrategyMonitor_File", this.txtBoxStrategyMonitor_SoundPath.Text);
            settings.Set("SoundsStrategyWindow", this.cbSoundStrategyWindow.Checked);
            settings.Set("SoundsStrategyWindow_File", this.txtBoxStrategyWindow_SoundPath.Text);
            settings.Set("SoundsRealTime", this.cbSoundsRealTime.Checked);
            settings.Set("SoundsRealTime_File", this.txtBoxRealTime_SoundPath.Text);
            settings.Set("SoundsIndicators", this.cbSoundsIndicators.Checked);
            settings.Set("SoundsParameters", this.cbSoundsParameters.Checked);
            settings.Set("NoDecimalRoundingForLimitStopPrice", this.cbNoDecimalRoundingForLimitStopPrice.Checked);
            foreach (ListViewItem item2 in this.lvStreaming.Items)
            {
                if (item2.Checked)
                {
                    StreamingDataProvider provider = (StreamingDataProvider) item2.Tag;
                    MainModule.Instance.StreamingProvider = provider;
                }
            }
            string symbolString = "";
            this.symbolParser_0.Text = this.txtStreaming.Text;
            for (int i = 0; i < this.symbolParser_0.Symbols.Count; i++)
            {
                symbolString = symbolString + this.symbolParser_0.Symbols[i];
                if (i < (this.symbolParser_0.Symbols.Count - 1))
                {
                    symbolString = symbolString + ",";
                }
            }
            MainModule.Instance.StreamingSymbolsUpdated(symbolString);
            settings.Set("BadTickFilter", this.cbBadTickFilter.Checked);
            settings.Set("BadTickThreshold", (double) this.numBadTick.Value);
            if (this.cmbDefaultAccount.SelectedIndex >= 0)
            {
                MainModule.Instance.DefaultAccountNumber = (string) this.cmbDefaultAccount.SelectedItem;
            }
            TradeManager tradeManager = MainModule.Instance.TradeManager;
            tradeManager.AlwaysExitAllSharesInPosition = this.cbExitAll.Checked && this.cbExitAll.Enabled;
            tradeManager.EnableCashThreshold = this.cbCashThreshold.Checked;
            tradeManager.EnableBuyingPowerThreshold = this.cbBuyingPowerThreshold.Checked;
            tradeManager.CashThreshold = (double) this.numCashThreshold.Value;
            tradeManager.BuyingPowerThreshold = (double) this.numBuyingPowerThreshold.Value;
            tradeManager.SameBarExits = this.cbSameBarExit.Checked;
            settings.Set("EmailSMTPHost", this.txtSMTPHost.Text.Trim());
            settings.Set("EmailSMTPPort", this.txtSMTPPort.Text.Trim());
            settings.Set("EmailAddresses", this.txtEmailAddresses.Text.Replace("\r\n", "~!").Replace(" ", string.Empty).Trim());
            settings.Set("EmailUserID", this.txtUserID.Text.Trim());
            settings.Set("EmailAuthenticateWithPassword", this.chkAuthenticateWithPassword.Checked);
            settings.Set("EmailPassword", MainModule.Instance.method_9(this.txtPassword.Text.Trim()));
            settings.Set("EmailSSL", this.chkSSL.Checked);
            MainModule.Instance.SaveSettings();
        }

        private void method_2(object sender, EventArgs e)
        {
            this.cbInterest_CheckedChanged(sender, e);
            this.lblTitleFontSample.BackColor = this.colorBackground.BackColor;
            this.lblTitleFontSample.ForeColor = ChartRenderer.ReverseColor(this.colorBackground.BackColor);
        }

        private void method_3(ListView listView_0, ListView listView_1)
        {
            for (int i = listView_0.Items.Count - 1; i >= 0; i--)
            {
                ListViewItem item = listView_0.Items[i];
                if (listView_0.Items[i].Selected)
                {
                    listView_0.Items.Remove(item);
                    listView_1.Items.Add(item);
                }
            }
        }

        private void method_4()
        {
            if (this.lbCommission.SelectedItem != null)
            {
                Commission selectedItem = (Commission) this.lbCommission.SelectedItem;
                TradeType tradeType = (TradeType) Enum.Parse(typeof(TradeType), this.cmbCommAction.Text);
                double shares = (double) this.numCommShares.Value;
                OrderType orderType = (OrderType) Enum.Parse(typeof(OrderType), this.cmbCommOrder.Text);
                double orderPrice = (double) this.numCommPrice.Value;
                this.lblCommTest.Text = selectedItem.Calculate(tradeType, orderType, orderPrice, shares, this.bars_0).ToString("C");
            }
        }

        private string method_5(string string_0)
        {
            string fileName;
            try
            {
                this.openFileDialog_0.FileName = string_0;
                this.openFileDialog_0.InitialDirectory = Path.GetDirectoryName(string_0);
            }
            catch
            {
            }
            this.openFileDialog_0.Filter = "Wave files (*.wav)|*.wav";
            this.openFileDialog_0.FilterIndex = 1;
            this.openFileDialog_0.RestoreDirectory = true;
            if (this.openFileDialog_0.ShowDialog() != DialogResult.OK)
            {
                return string_0;
            }
            try
            {
                if (this.openFileDialog_0.OpenFile() != null)
                {
                    fileName = this.openFileDialog_0.FileName;
                }
                else
                {
                    return string_0;
                }
            }
            catch (Exception)
            {
                return string_0;
            }
            return fileName;
        }

        private void method_6(object sender, EventArgs e)
        {
            this.cbInterest_CheckedChanged(sender, e);
            this.lblSampleText.BackColor = this.colorRightMargin.BackColor;
            this.lblSampleText.ForeColor = ChartRenderer.ReverseColor(this.colorRightMargin.BackColor);
        }

        private bool method_7(string string_0)
        {
            string_0 = string_0.Trim();
            Regex regex = new Regex(@"^[a-zA-Z0-9][\w\.-]{0,28}@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            if (!regex.IsMatch(string_0))
            {
                return false;
            }
            return true;
        }

        private bool method_8(string string_0)
        {
            string_0 = string_0.Replace(" ", string.Empty);
            string_0 = string_0.Replace("\r\n", ";");
            string_0 = string_0.TrimEnd(new char[] { ';' });
            string[] strArray = string_0.Split(new char[] { ';' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (!this.method_7(strArray[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private bool method_9()
        {
            if (!this.chkAuthenticateWithPassword.Checked)
            {
                if ((((this.txtSMTPHost.Text.Trim() == string.Empty) && (this.txtSMTPPort.Text.Trim() == string.Empty)) && ((this.txtEmailAddresses.Text.Trim() == string.Empty) && (this.txtUserID.Text.Trim() == string.Empty))) || (((this.txtSMTPHost.Text.Trim() != string.Empty) && (this.txtSMTPPort.Text.Trim() != string.Empty)) && ((this.txtEmailAddresses.Text.Trim() != string.Empty) && (this.txtUserID.Text.Trim() != string.Empty))))
                {
                    return true;
                }
            }
            else if ((((this.txtSMTPHost.Text.Trim() == string.Empty) && (this.txtSMTPPort.Text.Trim() == string.Empty)) && (((this.txtEmailAddresses.Text.Trim() == string.Empty) && (this.txtUserID.Text.Trim() == string.Empty)) && (this.txtPassword.Text.Trim() == string.Empty))) || ((((this.txtSMTPHost.Text.Trim() != string.Empty) && (this.txtSMTPPort.Text.Trim() != string.Empty)) && ((this.txtEmailAddresses.Text.Trim() != string.Empty) && (this.txtUserID.Text.Trim() != string.Empty))) && (this.txtPassword.Text.Trim() != string.Empty)))
            {
                return true;
            }
            MessageBox.Show(" Please enter all the required data ", Application.ProductName);
            return false;
        }

        private void numReduceQty_Leave(object sender, EventArgs e)
        {
            if (this.numReduceQty.Value < 0.1M)
            {
                this.numReduceQty.Text = "0.1";
            }
            else if (this.numReduceQty.Value > 100M)
            {
                this.numReduceQty.Text = "100.0";
            }
        }

        private void PreferencesForm_Activated(object sender, EventArgs e)
        {
            if (this.bool_1)
            {
                this.method_0();
            }
        }

        private void PreferencesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            base.Tag = this.tree.SelectedNode.Tag;
            MainModule.Instance.Settings.Set(this, "PreferencesForm");
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            TradingSystemExecutor executor;
            this.cmbCommAction.SelectedIndex = 0;
            this.cmbCommOrder.SelectedIndex = 0;
            this.tree.ExpandAll();
            SettingsManager settings = MainModule.Instance.Settings;
            if (!MainModule.Instance.AuthProvider.AllowStreaming)
            {
                for (int i = 0; i < this.tree.Nodes.Count; i++)
                {
                    if (this.tree.Nodes[i].Text == "Streaming Data")
                    {
                        this.tree.Nodes.RemoveAt(i);
                        break;
                    }
                }
            }
            settings.Get(this, "PreferencesForm");
            if (this.tree.SelectedNode == null)
            {
                this.tree.SelectedNode = this.tree.Nodes[0];
                this.pnlCS.BringToFront();
                if ((base.Tag != null) && (base.Tag is string))
                {
                    string tag = base.Tag as string;
                    using (IEnumerator enumerator2 = this.tree.Nodes.GetEnumerator())
                    {
                        TreeNode current;
                        while (enumerator2.MoveNext())
                        {
                            current = (TreeNode) enumerator2.Current;
                            if (((string) current.Tag) == tag)
                            {
                                goto Label_013E;
                            }
                        }
                        goto Label_0170;
                    Label_013E:
                        this.tree.SelectedNode = current;
                        this.tree_AfterSelect(this, new TreeViewEventArgs(current));
                    }
                }
            }
        Label_0170:
            foreach (IPerformanceVisualizer visualizer in MainModule.Instance.Visualizers)
            {
                ListViewItem item = this.lvPV.Items.Add(visualizer.TabText);
                item.Tag = visualizer;
                string text = "";
                if (visualizer.AppliesTo == VisualizerAppliesTo.All)
                {
                    text = "All Backtests";
                }
                else
                {
                    if ((visualizer.AppliesTo & VisualizerAppliesTo.MultiSymbol) == 0)
                    {
                        text = "Single Symbol";
                    }
                    else if ((visualizer.AppliesTo & VisualizerAppliesTo.SingleSymbol) == 0)
                    {
                        text = "Multi Symbol";
                    }
                    if ((visualizer.AppliesTo & VisualizerAppliesTo.PortfolioSim) == 0)
                    {
                        if (text != "")
                        {
                            text = text + ", ";
                        }
                        text = text + "Raw Profit Mode";
                    }
                    else if ((visualizer.AppliesTo & VisualizerAppliesTo.RawProfit) == 0)
                    {
                        if (text != "")
                        {
                            text = text + ", ";
                        }
                        text = text + "Portfolio Simulation";
                    }
                    else if ((visualizer.AppliesTo & VisualizerAppliesTo.CombinationStrategy) == VisualizerAppliesTo.CombinationStrategy)
                    {
                        text = "Combination Strategies";
                    }
                }
                item.SubItems.Add(text);
            }
            foreach (IPerformanceVisualizer visualizer2 in MainModule.Instance.VisualizersChecked)
            {
                using (IEnumerator enumerator5 = this.lvPV.Items.GetEnumerator())
                {
                    ListViewItem item2;
                    while (enumerator5.MoveNext())
                    {
                        item2 = (ListViewItem) enumerator5.Current;
                        if (item2.Text == visualizer2.TabText)
                        {
                            goto Label_02EF;
                        }
                    }
                    continue;
                Label_02EF:
                    item2.Checked = true;
                }
            }
            ChartRenderer renderer = MainModule.Instance.Renderer;
            this.colorBackground.BackColor = renderer.BackgroundColor;
            this.method_2(this, EventArgs.Empty);
            this.colorUpBars.BackColor = renderer.UpBarColor;
            this.colorDownBars.BackColor = renderer.DownBarColor;
            this.colorUpVolume.BackColor = renderer.UpBarVolumeColor;
            this.colorDownVolume.BackColor = renderer.DownBarVolumeColor;
            this.colorGridlines.BackColor = renderer.GridlineColor;
            this.colorRightMargin.BackColor = renderer.MarginRightColor;
            this.method_6(this, EventArgs.Empty);
            this.colorBottomMargin.BackColor = renderer.MarginBottomColor;
            this.colorPaneSep.BackColor = renderer.PaneSeparatorColor;
            this.lblSampleText.Font = renderer.AxisFont;
            this.lblTitleFontSample.Font = renderer.TitleFont;
            this.cbHorizontalGridlines.Checked = renderer.HorizontalGridines;
            this.cbVerticalGridlines.Checked = renderer.VerticalGridlines;
            this.cbPaneSeparators.Checked = renderer.PaneSeparatorVisible;
            this.cbPriceTooltip.Checked = settings.Get("PriceTooltip", true);
            this.cbIndicatorTooltip.Checked = settings.Get("IndicatorTooltip", true);
            this.cbFundamentalTooltip.Checked = settings.Get("FundamentalTooltip", true);
            this.fundamentalsLoader_0.DataHost = MainModule.Instance.DataSources;
            IList<FundamentalItem> chartableItems = this.fundamentalsLoader_0.ChartableItems;
            string fundamentalGlyphs = renderer.FundamentalGlyphs;
            foreach (FundamentalItem item4 in chartableItems)
            {
                ListViewItem item3;
                if (!fundamentalGlyphs.StartsWith(item4.Name + ";") && !fundamentalGlyphs.Contains(";" + item4.Name + ";"))
                {
                    item3 = this.lvFundAvailable.Items.Add(item4.Name);
                }
                else
                {
                    item3 = this.lvFundSelected.Items.Add(item4.Name);
                }
                item3.Tag = item4;
                this.imageList_0.Images.Add(item4.Glyph);
                item3.ImageIndex = this.imageList_0.Images.Count - 1;
            }
            this.assemblyLoader_0.Path = MainModule.Instance.AppPath;
            foreach (System.Type type in this.assemblyLoader_0.Types)
            {
                Commission commission = (Commission) this.assemblyLoader_0.CreateInstance(type);
                if (commission is ICustomSettings)
                {
                    (commission as ICustomSettings).ReadSettings(MainModule.Instance.Settings);
                }
                this.lbCommission.Items.Add(commission);
            }
            this.cbCommission.Checked = MainModule.Instance.Executor.ApplyCommission;
            if (MainModule.Instance.Executor.Commission != null)
            {
                string name = MainModule.Instance.Executor.Commission.GetType().Name;
                using (IEnumerator enumerator6 = this.lbCommission.Items.GetEnumerator())
                {
                    object obj2;
                    while (enumerator6.MoveNext())
                    {
                        obj2 = enumerator6.Current;
                        if (obj2.GetType().Name == name)
                        {
                            goto Label_06A7;
                        }
                    }
                    goto Label_06CB;
                Label_06A7:
                    this.lbCommission.SelectedItem = obj2;
                }
            }
        Label_06CB:
            executor = MainModule.Instance.Executor;
            this.cbSlippage.Checked = executor.EnableSlippage;
            this.cbLimitSlippage.Checked = executor.LimitOrderSlippage;
            this.numSlippage.Value = (decimal) executor.SlippageUnits;
            this.numFuturesSlippage.Value = executor.SlippageTicks;
            this.cbRoundLots.Checked = executor.RoundLots;
            this.cbRound50.Checked = executor.RoundLots50;
            this.cbLimitDays.Checked = executor.LimitDaySimulation;
            this.cbInterest.Checked = executor.ApplyInterest;
            this.numCashRate.Text = executor.CashRate.ToString();
            this.numMarginRate.Text = executor.MarginRate.ToString();
            this.cbDividends.Checked = executor.ApplyDividends;
            this.cbReduceQty.Checked = executor.ReduceQtyBasedOnVolume;
            this.cbNoDecimalRoundingForLimitStopPrice.Checked = executor.NoDecimalRoundingForLimitStopPrice;
            this.numReduceQty.Text = executor.RedcuceQtyPct.ToString();
            this.cbWorstTradeSimulation.Checked = executor.WorstTradeSimulation;
            this.cbBenchmarkBH.Checked = executor.BenchmarkBuyAndHoldON;
            this.txtBHSymbol.Text = executor.BenchmarkSymbol;
            this.txtBHSymbol.Enabled = this.cbBenchmarkBH.Checked;
            this.cbShowHome.Checked = settings.Get("ShowHomePage", true);
            this.cbExpand.Checked = settings.Get("ExpandFirstDataSet", true);
            this.cbRememberData.Checked = settings.Get("RememberStrategyData", false);
            this.cbRememberPositionSize.Checked = settings.Get("RememberStrategyPositionSize", false);
            this.cbRememberRange.Checked = settings.Get("RememberStrategyRange", false);
            this.cbRememberScale.Checked = settings.Get("RememberStrategyScale", false);
            this.cbRememberParamValues.Checked = settings.Get("RememberParameterSliders", false);
            this.cbApplyCharts.Checked = settings.Get("ApplyChartColors", true);
            this.cbPrintPreviewOff.Checked = settings.Get("HidePrintPreview", false);
            this.cbPrintDialogOff.Checked = settings.Get("HidePrintDialog", false);
            this.cbAutoOpenOrders.Checked = settings.Get("AutoOpenOrders", true);
            this.cbSwitchAccount.Checked = settings.Get("SwitchToAccount", true);
            this.indicatorDecimalPlaces.Value = settings.Get(DecimalsManager.Instance.IndicatorKey, DecimalsManager.Instance.Indicator);
            this.pricingDecimalPlaces.Value = settings.Get(DecimalsManager.Instance.PricingKey, DecimalsManager.Instance.Pricing);
            this.cbDisablePortfolioSynch.Checked = settings.Get(TradeManager.DisablePortfolioSynchKey, false);
            this.cbNoDecimalRoundingForLimitStopPrice.Checked = settings.Get("NoDecimalRoundingForLimitStopPrice", false);
            this.cbSoundQuotes.Checked = settings.Get("SoundsQuotes", true);
            string defaultValue = MainModule.Instance.AppPath + @"\Data\Sounds\alert1.wav";
            string path = settings.Get("SoundsQuotes_File", defaultValue);
            if (!File.Exists(path))
            {
                path = defaultValue;
            }
            this.txtBoxQuotes_SoundPath.Text = path;
            if (!this.cbSoundQuotes.Checked)
            {
                this.txtBoxQuotes_SoundPath.Enabled = false;
                this.btnBrowse_Quotes.Enabled = false;
            }
            this.cbSoundStategyMonitor.Checked = settings.Get("SoundsStrategyMonitor", true);
            defaultValue = MainModule.Instance.AppPath + @"\Data\Sounds\alert2.wav";
            path = settings.Get("SoundsStrategyMonitor_File", defaultValue);
            if (!File.Exists(path))
            {
                path = defaultValue;
            }
            this.txtBoxStrategyMonitor_SoundPath.Text = path;
            if (!this.cbSoundStategyMonitor.Checked)
            {
                this.txtBoxStrategyMonitor_SoundPath.Enabled = false;
                this.btnBrowse_StrategyMonitor.Enabled = false;
            }
            this.cbSoundStrategyWindow.Checked = settings.Get("SoundsStrategyWindow", true);
            defaultValue = MainModule.Instance.AppPath + @"\Data\Sounds\alert4.wav";
            path = settings.Get("SoundsStrategyWindow_File", defaultValue);
            if (!File.Exists(path))
            {
                path = defaultValue;
            }
            this.txtBoxStrategyWindow_SoundPath.Text = path;
            if (!this.cbSoundStrategyWindow.Checked)
            {
                this.txtBoxStrategyWindow_SoundPath.Enabled = false;
                this.btnBrowse_StrategyWindow.Enabled = false;
            }
            this.cbSoundsRealTime.Checked = settings.Get("SoundsRealTime", true);
            defaultValue = MainModule.Instance.AppPath + @"\Data\Sounds\DIGITAL.wav";
            path = settings.Get("SoundsRealTime_File", defaultValue);
            if (!File.Exists(path))
            {
                path = defaultValue;
            }
            this.txtBoxRealTime_SoundPath.Text = path;
            if (!this.cbSoundsRealTime.Checked)
            {
                this.txtBoxRealTime_SoundPath.Enabled = false;
                this.btnBrowse_RealTimeStrategy.Enabled = false;
            }
            this.cbSoundsIndicators.Checked = settings.Get("SoundsIndicators", true);
            this.cbSoundsParameters.Checked = settings.Get("SoundsParameters", true);
            this.assemblyLoader_1.Path = MainModule.Instance.AppPath;
            foreach (System.Type type2 in this.assemblyLoader_1.Types)
            {
                StreamingDataProvider provider = (StreamingDataProvider) this.assemblyLoader_1.CreateInstance(type2);
                this.imageList_1.Images.Add(provider.Glyph);
                ListViewItem item5 = this.lvStreaming.Items.Add(provider.FriendlyName);
                item5.Tag = provider;
                item5.ImageIndex = this.imageList_1.Images.Count - 1;
            }
            this.txtStreaming.Text = settings.Get("StreamingSymbols", ".DJI, .IXIC, .SPX");
            if (MainModule.Instance.StreamingProvider != null)
            {
                string friendlyName = MainModule.Instance.StreamingProvider.FriendlyName;
                using (IEnumerator enumerator8 = this.lvStreaming.Items.GetEnumerator())
                {
                    ListViewItem item6;
                    while (enumerator8.MoveNext())
                    {
                        item6 = (ListViewItem) enumerator8.Current;
                        if (item6.Text == friendlyName)
                        {
                            goto Label_0D1D;
                        }
                    }
                    goto Label_0D44;
                Label_0D1D:
                    item6.Checked = true;
                    item6.Selected = true;
                }
            }
        Label_0D44:
            this.cbBadTickFilter.Checked = settings.Get("BadTickFilter", false);
            this.numBadTick.Text = settings.Get("BadTickThreshold", (double) 20.0).ToString();
            MainModule.Instance.HelpProvider.SetHelpNavigator(this, HelpNavigator.Topic);
            MainModule.Instance.HelpProvider.SetHelpKeyword(this, "Preferences2.htm");
            foreach (string str6 in MainModule.Instance.AccountNumbers)
            {
                this.cmbDefaultAccount.Items.Add(str6);
            }
            this.cmbDefaultAccount.SelectedIndex = this.cmbDefaultAccount.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
            if ((this.cmbDefaultAccount.SelectedIndex == -1) && (this.cmbDefaultAccount.Items.Count > 0))
            {
                this.cmbDefaultAccount.SelectedIndex = 0;
            }
            TradeManager tradeManager = MainModule.Instance.TradeManager;
            this.cbExitAll.Checked = settings.Get("ExitFullPosition", false);
            this.cbCashThreshold.Checked = tradeManager.EnableCashThreshold;
            this.cbBuyingPowerThreshold.Checked = tradeManager.EnableBuyingPowerThreshold;
            this.numCashThreshold.Text = tradeManager.CashThreshold.ToString();
            this.numBuyingPowerThreshold.Text = tradeManager.BuyingPowerThreshold.ToString();
            this.cbSameBarExit.Checked = tradeManager.SameBarExits;
            this.txtSMTPHost.Text = settings.Get("EmailSMTPHost", string.Empty);
            this.txtSMTPPort.Text = settings.Get("EmailSMTPPort", string.Empty);
            string str5 = settings.Get("EmailAddresses", string.Empty).Replace("~!", "\r\n");
            this.txtEmailAddresses.Text = str5;
            this.txtUserID.Text = settings.Get("EmailUserID", "");
            this.chkAuthenticateWithPassword.Checked = settings.Get("EmailAuthenticateWithPassword", false);
            this.txtPassword.Text = MainModule.Instance.method_11(settings.Get("EmailPassword", string.Empty));
            if (!this.chkAuthenticateWithPassword.Checked)
            {
                this.txtPassword.Enabled = false;
            }
            this.chkSSL.Checked = settings.Get("EmailSSL", false);
            this.btnApply.Enabled = false;
            this.bool_0 = true;
        }

        private void pricingDecimalPlaces_ValueChanged(object sender, EventArgs e)
        {
            this.btnApply.Enabled = true;
        }

        public DialogResult ShowDialog(string section)
        {
            using (IEnumerator enumerator = this.tree.Nodes.GetEnumerator())
            {
                TreeNode current;
                while (enumerator.MoveNext())
                {
                    current = (TreeNode) enumerator.Current;
                    if (current.Text == section)
                    {
                        goto Label_0038;
                    }
                }
                goto Label_0057;
            Label_0038:
                this.tree.SelectedNode = current;
            }
        Label_0057:
            return base.ShowDialog();
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string tag = (string) e.Node.Tag;
            using (IEnumerator enumerator = this.split.Panel2.Controls.GetEnumerator())
            {
                Panel panel;
                while (enumerator.MoveNext())
                {
                    Control current = (Control) enumerator.Current;
                    if (current is Panel)
                    {
                        panel = current as Panel;
                        string str2 = panel.Tag as string;
                        if ((str2 != null) && (str2 == tag))
                        {
                            goto Label_006A;
                        }
                    }
                }
                goto Label_0086;
            Label_006A:
                panel.BringToFront();
            }
        Label_0086:
            this.cbApplyCharts.Visible = (tag == "CS") || (tag == "CA");
            this.method_0();
        }

        private void tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (!this.method_9())
            {
                e.Cancel = true;
            }
            if (this.cbBenchmarkBH.Checked && string.IsNullOrEmpty(this.txtBHSymbol.Text.Trim()))
            {
                MessageBox.Show("Please enter a symbol for the Benchmark Buy & Hold preference", Application.ProductName);
                e.Cancel = true;
            }
        }

        private void txtBHSymbol_TextChanged(object sender, EventArgs e)
        {
            this.btnApply.Enabled = true;
        }

        private void txtEmailAddresses_Enter(object sender, EventArgs e)
        {
            base.AcceptButton = null;
        }

        private void txtEmailAddresses_Leave(object sender, EventArgs e)
        {
            base.AcceptButton = this.btnOK;
        }

        private void txtEmailAddresses_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtEmailAddresses.Text.Trim()) && !this.method_8(this.txtEmailAddresses.Text))
            {
                MessageBox.Show("Please enter emails in proper format", Application.ProductName);
                this.txtEmailAddresses.SelectAll();
                e.Cancel = true;
            }
        }

        private void txtSMTPHost_TextChanged(object sender, EventArgs e)
        {
            this.btnApply.Enabled = true;
        }

        private void txtSMTPPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            int result = 0;
            if (e.KeyChar != '\b')
            {
                e.Handled = !int.TryParse(e.KeyChar.ToString(), out result);
            }
        }
    }
}

