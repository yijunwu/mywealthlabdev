namespace WealthLabPro
{
    using Fidelity.Components;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;
    using WealthLabPro.Properties;

    public class ChartForm : Form, IWorkspace, IConnectionStatus, ICDOBehavior, IVisualizerHost, IWealthScriptProvider
    {
        private Alerts alerts_0;
        private AutoResetEvent autoResetEvent_0 = new AutoResetEvent(false);
        internal BarDataRangeSelecter barRange;
        private WealthLab.Bars bars_0;
        private WealthLab.Bars bars_1;
        internal BarsLoader barsLoader_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_10;
        private bool bool_11;
        private bool bool_12;
        private bool bool_13;
        private bool bool_14;
        private bool bool_15;
        [CompilerGenerated]
        private bool bool_16;
        [CompilerGenerated]
        private bool bool_17;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private bool bool_6;
        private bool bool_7;
        private bool bool_8;
        private bool bool_9;
        private ToolStripButton btnLink;
        internal ToolStripButton btnPV;
        private Button btnRunAll;
        private Button btnRunAllCancel;
        private ToolStripButton btnStreaming;
        private Builder builder_0;
        private Chart chart;
        private ChartDrawingObject chartDrawingObject_0;
        private ChartRenderer chartRenderer_0;
        private CombinationStrategyBuilder combinationStrategyBuilder_0;
        private WealthLab.DataSource dataSource_0;
        private WealthLab.DataSource dataSource_1;
        [CompilerGenerated]
        private WealthLab.DataSource dataSource_2;
        private Description description_0;
        private DialogResult dialogResult_0;
        private Dictionary<WealthLab.DataSource, WealthLab.BarDataScale> dictionary_0 = new Dictionary<WealthLab.DataSource, WealthLab.BarDataScale>();
        private Dictionary<string, string> dictionary_1 = new Dictionary<string, string>();
        private DrawingObjectManager drawingObjectManager_0;
        private DrawingObjectManager drawingObjectManager_1;
        private ToolStripMenuItem editBarToolStripMenuItem;
        private Editor editor_0;
        private ToolStripMenuItem enableDisableStreamingHiddenMenuItem;
        internal FundamentalsLoader fundamentalsLoader_0;
        private GroupBox grpMultiSymbol;
        private IContainer icontainer_0;
        private static readonly ILog ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ImageList imageList_0;
        private IndicatorDragDropManager indicatorDragDropManager_0;
        private static int int_0 = 0;
        private int int_1;
        private int int_2 = -1;
        private int int_3 = -1;
        private int int_4 = -1;
        [CompilerGenerated]
        private int int_5;
        private Label lblProgress;
        private Label lblRunAllStatus;
        private Label lblStatus;
        private Label lblSymbol;
        private List<WealthLab.Bars> list_0 = new List<WealthLab.Bars>();
        private List<TabPage> list_1 = new List<TabPage>();
        private List<TabPage> list_2 = new List<TabPage>();
        private MarketHours marketHours_0;
        private ToolStripMenuItem mniAddDifferent;
        private ToolStripMenuItem mniAddStrategy;
        private ToolStripMenuItem mniAddSymbolToDataSet;
        private ToolStripMenuItem mniChartBuy;
        private ToolStripMenuItem mniChartCover;
        private ToolStripMenuItem mniChartOptions;
        private ToolStripMenuItem mniChartSell;
        private ToolStripMenuItem mniChartShort;
        private ToolStripMenuItem mniChartStyleSettings;
        private ToolStripMenuItem mniCopyChart;
        private ToolStripMenuItem mniCopyPriceData;
        private ToolStripMenuItem mniDeleteDrawingObject;
        private ToolStripMenuItem mniDeleteIndicator;
        private ToolStripMenuItem mniDrawingObjectProperties;
        private ToolStripMenuItem mniIndicatorProperties;
        private ToolStripMenuItem mniPlotIndicator;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniPrintAll;
        private ToolStripMenuItem mniPushCode;
        private ToolStripMenuItem mniReload;
        private ToolStripMenuItem mniStockSplit;
        private WealthLabPro.Optimization optimization_0;
        private TabPage pageChart;  ///WYJ note: the tab page for chart
        private ToolStripMenuItem plotAFundamentalDataItemOnTheChartToolStripMenuItem;
        private Panel pnlMultiSymbol;
        private Panel pnlSymbol;
        private ContextMenuStrip popupChart;
        internal PositionSizeSelecter posSize;
        private ProgressBar progRunAll;
        private ToolStripSeparator sepBuySell;
        private ToolStripSeparator sepCopyChart;
        private ToolStripSeparator sepDrawing;
        private ToolStripSeparator sepIndicators;
        private ToolStripSeparator sepPlot;
        private ToolStripSeparator sepShortCover;
        private StatusStrip status;
        private ToolStripStatusLabel statusAddStrategy;
        private ToolStripStatusLabel statusAddToSC;
        private StatusStrip statusChart;
        private ToolStripStatusLabel statusOptimize;
        private ToolStripStatusLabel stlblBar;
        private ToolStripStatusLabel stlblBars;
        private ToolStripStatusLabel stlblBHPerBar;
        private ToolStripStatusLabel stlblBHProfit;
        private ToolStripStatusLabel stlblClose;
        private ToolStripStatusLabel stlblDate;
        private ToolStripStatusLabel stlblHigh;
        private ToolStripStatusLabel stlblLastDate;
        private ToolStripStatusLabel stlblLow;
        private ToolStripStatusLabel stlblMouse;
        private ToolStripStatusLabel stlblOpen;
        private ToolStripStatusLabel stlblPerBar;
        private ToolStripStatusLabel stlblProfit;
        private ToolStripStatusLabel stlblVolume;
        private WealthLab.Strategy strategy_0;
        private StreamingChartManager streamingChartManager_0;
        private string string_0 = "";
        [CompilerGenerated]
        private string string_1;
        private TabControl tabChart;
        private TabPage tabPage_0;
        private Thread thread_0;
        private System.Windows.Forms.Timer timer_0;
        private ToolStripSeparator toolStripSeparator1;
        private TradingSystemExecutor tradingSystemExecutor_0;
        private TradingSystemExecutor tradingSystemExecutor_1;
        private TextBox txtSymbol;
        private WealthLab.WealthScript wealthScript_0;

        public ChartForm()
        {
            int_0++;
            this.int_1 = int_0;
            this.InitializeComponent();
            if (Application.ProductName == "WealthLabPro")
            {
                this.chartRenderer_0.FundamentalGlyphs = "Split;Dividend;";
            }
            this.chart.DataScaleChange += new EventHandler<ScaleChangeEventArgs>(this.method_0);
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
            int moveBy = 0;
            switch (e.KeyCode)
            {
                /*case Keys.Left:
                    if (e.Control)
                        moveBy = 40;
                    else
                        moveBy = 10;
                    this.chart.ScrollBy(moveBy);
                    break;
                            
                case Keys.Right:
                    if (e.Control)
                        moveBy = -40;
                    else
                        moveBy = -10;
                    this.chart.ScrollBy(moveBy);///WYJ fix
                    break; */
                case Keys.Up:
                    this.IncreaseBarSpacing();
                    break;
                case Keys.Down:
                    this.DecreaseBarSpacing();
                    break;
            }
            /*
            if (e.KeyData == (Keys.Control | Keys.Q))
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is MainForm)
                    {
                        (form as MainForm).btnCrossHair_Click(null, null);
                    }
                }
            } */
                

        }

        /*private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
                return;
                ChartForm activeChartWindow = null;
                if (activeChartWindow != null)
                {
                    activeChartWindow.PressEscape();
                }
            }
        } */

        public void AddToStrategyMonitor()
        {
            this.MyMainForm.CreateStrategyCenter();
            if (this.Strategy.Name == string.Empty)
            {
                this.SaveStrategyAs();
                if (this.Strategy.Name == string.Empty)
                {
                    MessageBox.Show("An untitled strategy cannot be added to the Strategy Monitor.", "Please Save Strategy", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            StrategyCenterForm.Instance.AddStrategyToStrategyCenter(this.Strategy);
        }

        public string ApplicationName()
        {
            return MainModule.Instance.AuthProvider.ApplicationName;
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            this.btnLink.Checked = !this.btnLink.Checked;
        }

        private void btnPV_Click(object sender, EventArgs e)
        {
            this.btnPV.Checked = !this.btnPV.Checked;
            if (!this.bool_7)
            {
                this.Strategy.UsePreferredValues = this.btnPV.Checked;
                this.NeedSave = true;
                if (!this.btnPV.Checked && !this.Strategy.RestoreSavedParameterValues(this.WealthScript))
                {
                    this.WealthScript.RestoreParameterDefaults();
                }
                if (((this.Symbol != null) && (this.Symbol != "")) && !this.bool_10)
                {
                    this.GoButtonPressed(this.Symbol, true);
                }
            }
        }

        private void btnRunAll_Click(object sender, EventArgs e)
        {
            this.RunOnAllSymbols();
        }

        private void btnRunAllCancel_Click(object sender, EventArgs e)
        {
            this.bool_11 = true;
            this.bool_10 = false;
        }

        private void btnStreaming_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsBusy)
            {
                this.thread_0.Abort(AbortReason.Streaming);
                this.thread_0 = null;
            }
            this.tradingSystemExecutor_1.IsStreaming = this.btnStreaming.Checked;
            if (this.alerts_0 != null)
            {
                this.alerts_0.IsStreaming = this.btnStreaming.Checked;
            }
            if (this.description_0 != null)
            {
                this.description_0.PopulateActivationSection();
            }
            if (this.btnStreaming.Checked)
            {
                this.statusOptimize.Visible = false;
                this.streamingChartManager_0.ConnectionStatus = MainModule.Instance;
                this.streamingChartManager_0.Provider = MainModule.Instance.StreamingProvider;
                if (this.streamingChartManager_0.Provider == null)
                {
                    MessageBox.Show("There is no Streaming Provider selected, please select one in Preferences/Streaming");
                    this.btnStreaming.Checked = false;
                    return;
                }
                this.bars_1 = null;
                this.bool_3 = true;
                this.GoButtonPressed(this.Symbol, true);
                this.timer_0.Enabled = true;
            }
            else
            {
                this.method_2();
                this.timer_0.Enabled = false;
                this.streamingChartManager_0.StopStreaming();
                this.bool_3 = false;
                if (!this.bool_5 && ((this.Strategy == null) || (this.Strategy.StrategyType != StrategyType.CombinedStrategy)))
                {
                    this.GoButtonPressed(this.Symbol, true);
                }
            }
            this.MyMainForm.SynchDataRangeControl(this);
            this.MyMainForm.EnableSliders();
        }

        private void chart_Click(object sender, EventArgs e)
        {
            this.pnlSymbol.Visible = false;
        }

        private void chart_DoubleClick(object sender, EventArgs e)
        {
            if (!this.method_66())
            {
                if (this.drawingObjectManager_0.SelectedDrawingObject != null)
                {
                    this.mniDrawingObjectProperties_Click(this, EventArgs.Empty);
                }
                else if (this.indicatorDragDropManager_0.SelectedIndicator != null)
                {
                    this.mniIndicatorProperties_Click(this, EventArgs.Empty);
                }
                else if (this.AllowEditBarData && this.DataSource.Provider.DataStore.ContainsSymbol(this.Bars.Symbol, this.Bars.Scale, this.Bars.BarInterval))
                {
                    this.method_59(this.int_2);
                }
                else
                {
                    string str;
                    if (!this.btnStreaming.Checked)
                    {
                        str = "Bar Data Editor is not available for this chart.";
                    }
                    else
                    {
                        str = "Bar Data Editor is not available for streaming charts. Please disable streaming and then try again.";
                    }
                    MessageBox.Show(str, "Bar Data Editor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void chart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((this.tabChart.SelectedIndex == 0) && !this.pnlMultiSymbol.Visible)
            {
                this.pnlSymbol.Visible = true;
                this.txtSymbol.Text = e.KeyChar.ToString();
                this.txtSymbol.Select(1, 0);
                this.txtSymbol.Focus();
            }
        }

        private void chart_MouseLeave(object sender, EventArgs e)
        {
            this.method_9(this, new BarNumberEventArgs(-1, 0.0, null));
        }

        private void ChartForm_Activated(object sender, EventArgs e)
        {
            if (this.tabChart.SelectedIndex == 0)
            {
                this.chart.Focus();
            }
            else if (((this.tabChart.SelectedTab != null) && (this.tabChart.SelectedTab.Text == "Editor")) && (this.editor_0 != null))
            {
                this.editor_0.FocusEditor();
            }
            this.MyMainForm.EnableEditAndPrintMenu();
        }

        private void ChartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.thread_0 != null)
            {
                if (this.thread_0.IsAlive)
                {
                    this.thread_0.Abort();
                }
                this.thread_0 = null;
            }
            this.MyMainForm.ItemRemoved(this);
            this.list_0.Clear();
            this.tradingSystemExecutor_0.Clear();
            this.tradingSystemExecutor_1.Clear();
            if (this.IsStreaming && (this.streamingChartManager_0.Provider != null))
            {
                this.streamingChartManager_0.Provider.UnSubscribe(this.Symbol, this.streamingChartManager_0);
            }
            if (this.optimization_0 != null)
            {
                this.optimization_0.Close();
            }
            this.MyMainForm.EnableControls(true);
        }

        private void ChartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.NeedSave || (this.ParametersNeedSave && MainModule.Instance.Settings.Get("RememberParameterSliders", false)))
            {
                string name;
                if (this.Strategy != null)
                {
                    name = this.Strategy.Name;
                }
                else
                {
                    name = "[Untitled]";
                }
                switch (MessageBox.Show("Save changes to Strategy: " + name, "Save Strategy?", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;

                    case DialogResult.Yes:
                        if (this.Strategy != null)
                        {
                            e.Cancel = !this.SaveStrategy();
                        }
                        else
                        {
                            e.Cancel = !this.SaveStrategyAs();
                        }
                        break;
                }
            }
        }

        private void ChartForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 0x1b)
            {
                this.PressEscape();
            }
        }

        private void ChartForm_Load(object sender, EventArgs e)
        {
            this.btnStreaming.Visible = MainModule.Instance.AuthProvider.AllowStreaming;
            this.barsLoader_0.StartDate = DateTime.MinValue;
            this.barsLoader_0.EndDate = DateTime.MaxValue;
            this.UpdateChartColorsAndStyle(false);
            this.chart.Renderer = this.chartRenderer_0;
            this.indicatorDragDropManager_0.Fundamentals = this.fundamentalsLoader_0;
            this.drawingObjectManager_0.RootPath = MainModule.Instance.DataPath;
            this.drawingObjectManager_1.RootPath = MainModule.Instance.DataPath;
            this.drawingObjectManager_0.SettingsHost = MainModule.Instance.Settings;
            this.indicatorDragDropManager_0.SettingsHost = MainModule.Instance.Settings;
            this.fundamentalsLoader_0.DataHost = MainModule.Instance.DataSources;
            if (!this.fundamentalsLoader_0.HasDragDropFundamentals)
            {
                this.plotAFundamentalDataItemOnTheChartToolStripMenuItem.Visible = false;
            }
            this.mniPushCode.Visible = this.indicatorDragDropManager_0.HasDragDroppedIndicators;
            this.MyMainForm.ItemAdded(this);
            if (MainModule.Instance.StreamingWasClicked)
            {
                this.btnStreaming.PerformClick();
            }
            this.drawingObjectManager_0.NewDrawingObjectAdded += new EventHandler(this.drawingObjectManager_0_NewDrawingObjectAdded);
            bool positions = this.drawingObjectManager_0.HasObjectsThatCanTriggerAlerts || (this.Strategy != null);
            this.MyMainForm.SetDataPanelState(true, true, positions, this.Strategy != null, (this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy));
        }

        private void ChartForm_Resize(object sender, EventArgs e)
        {
            if (this.StatusPanelsVisible)
            {
                this.chart.Height = (this.pageChart.Height - this.statusChart.Height) - 9;
            }
            else
            {
                this.chart.Height = this.pageChart.Height - 4;
            }
        }

        private void ChartForm_TextChanged(object sender, EventArgs e)
        {
            this.MyMainForm.ItemChanged(this);
        }

        public void ClearDrawingObjects()
        {
            this.drawingObjectManager_0.Clear();
            this.drawingObjectManager_0.SaveDrawingObjects(this.Bars);
            this.chart.DoInvalidate();
            bool positions = this.drawingObjectManager_0.HasObjectsThatCanTriggerAlerts || (this.Strategy != null);
            this.MyMainForm.SetDataPanelState(true, true, positions, this.Strategy != null, (this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy));
        }

        public void CompileSource()
        {
            if (this.editor_0 != null)
            {
                this.editor_0.Compile();
            }
        }

        public void CompileStrategyCode()
        {
            if (this.editor_0 != null)
            {
                this.editor_0.Compile();
            }
        }

        public void Connect()
        {
        }

        public void Connect(bool reconnect)
        {
            if (reconnect && this.bool_14)
            {
                this.IsStreaming = true;
                this.bool_14 = false;
            }
            this.bool_15 = false;
        }

        public void CopyListViewToClipboard(ListView listView_0)
        {
            MainModule.Instance.CopyListViewToClipboard(listView_0);
        }

        public void DataSourceSelected(WealthLab.DataSource dataSource_3)
        {
            this.DataSource = dataSource_3;
            if (dataSource_3 != null)
            {
                if (this.MyMainForm.SelectingNodeForFormCreation && (this.MyMainForm.Symbol == ""))
                {
                    this.MyMainForm.SelectingNodeForFormCreation = false;
                }
                if (this.dictionary_0.ContainsKey(dataSource_3))
                {
                    this.barsLoader_0.BarDataScale = this.dictionary_0[dataSource_3];
                }
                else if (dataSource_3.IsIntraday)
                {
                    this.barsLoader_0.BarDataScale = dataSource_3.BarDataScale;
                }
                else
                {
                    WealthLab.BarDataScale barDataScale = dataSource_3.BarDataScale;
                    WealthLab.BarDataScale scale2 = this.MyMainForm.BarDataScale;
                    if (barDataScale.CanConvertTo(scale2))
                    {
                        this.barsLoader_0.BarDataScale = scale2;
                    }
                    else
                    {
                        this.barsLoader_0.BarDataScale = barDataScale;
                    }
                }
            }
        }

        public void DecreaseBarSpacing()
        {
            if (this.chartRenderer_0.BarSpacing > 1)
            {
                this.chartRenderer_0.BarSpacing--;
                this.chart.DoInvalidate();
                MainModule.Instance.Renderer.BarSpacing = this.chartRenderer_0.BarSpacing;
            }
        }

        public void Disconnect()
        {
            if (!this.bool_15)
            {
                this.bool_14 = this.IsStreaming;
            }
            this.bool_15 = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void drawingObjectManager_0_NewDrawingObjectAdded(object sender, EventArgs e)
        {
            bool positions = this.drawingObjectManager_0.HasObjectsThatCanTriggerAlerts || (this.Strategy != null);
            this.MyMainForm.SetDataPanelState(true, true, positions, this.Strategy != null, (this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy));
        }

        private void editBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.method_59(this.int_4);
        }

        public void EditCopy()
        {
            if (this.tabChart.SelectedTab == this.pageChart)
            {
                this.mniCopyChart.PerformClick();
            }
            else
            {
                if ((this.description_0 != null) && this.tabChart.SelectedTab.Text.Contains("Strategy Summary"))
                {
                    this.description_0.CopyToClipboard();
                }
                if ((this.alerts_0 != null) && this.tabChart.SelectedTab.Text.Contains("Alert"))
                {
                    this.alerts_0.CopyToClipboard();
                }
                if ((this.editor_0 != null) && (this.tabChart.SelectedTab.Text == "Editor"))
                {
                    this.editor_0.EditCopy();
                }
                IPerformanceVisualizer selectedVisualizer = this.SelectedVisualizer;
                if ((selectedVisualizer != null) && selectedVisualizer.SupportClipboardCopy)
                {
                    selectedVisualizer.CopyToClipboard();
                }
            }
        }

        public void EditCut()
        {
            if ((this.editor_0 != null) && (this.tabChart.SelectedTab.Text == "Editor"))
            {
                this.editor_0.EditCut();
            }
        }

        public void EditDelete()
        {
            if ((this.editor_0 != null) && (this.tabChart.SelectedTab.Text == "Editor"))
            {
                this.editor_0.EditDelete();
            }
        }

        public void EditFind()
        {
            if ((this.editor_0 != null) && (this.tabChart.SelectedTab.Text == "Editor"))
            {
                this.editor_0.EditFind();
            }
        }

        public void EditFindReplace()
        {
            if ((this.editor_0 != null) && (this.tabChart.SelectedTab.Text == "Editor"))
            {
                this.editor_0.EditFindReplace();
            }
        }

        public void EditPaste()
        {
            if ((this.editor_0 != null) && (this.tabChart.SelectedTab.Text == "Editor"))
            {
                this.editor_0.EditPaste();
            }
        }

        public void EditUndo()
        {
            if ((this.editor_0 != null) && (this.tabChart.SelectedTab.Text == "Editor"))
            {
                this.editor_0.EditUndo();
            }
        }

        private void enableDisableStreamingHiddenMenuItem_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Alt) > Keys.None)
            {
                this.MyMainForm.SetStreamingGlobal(!this.btnStreaming.Checked);
            }
            else
            {
                this.btnStreaming.Checked = !this.btnStreaming.Checked;
                MainModule.Instance.StreamingWasClicked = this.btnStreaming.Checked;
            }
        }

        public WealthLab.BarDataScale GetBarDataScaleForDataSource(WealthLab.DataSource dataSource_3)
        {
            if ((dataSource_3 != null) && this.dictionary_0.ContainsKey(dataSource_3))
            {
                return this.dictionary_0[dataSource_3];
            }
            return dataSource_3.BarDataScale;
        }

        public TradingSystemExecutor GetExecutor()
        {
            TradingSystemExecutor executor = MainModule.Instance.Executor;
            if (executor.FundamentalsLoader == null)
            {
                executor.FundamentalsLoader = this.fundamentalsLoader_0;
            }
            return executor;
        }

        public void GetPageSettings(ref PageSettings pageSettings)
        {
            pageSettings = this.MyMainForm.DefaultPageSettings;
        }

        public void GoButtonPressed(string symbol, bool force)
        {
            if (!this.bool_10 || force)
            {
                if ((symbol == "") && !this.bool_3)
                {
                    this.RunOnAllSymbols();
                }
                else if (symbol != "")
                {
                    this.Symbol = symbol;
                    this.bool_10 = false;
                    this.ShowMultiSymbolControls(false);
                    this.method_16();
                }
            }
        }

        public void IncreaseBarSpacing()
        {
            this.chartRenderer_0.BarSpacing++;
            this.chart.DoInvalidate();
            MainModule.Instance.Renderer.BarSpacing = this.chartRenderer_0.BarSpacing;
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ChartForm));
            WealthLab.PositionSize size = new WealthLab.PositionSize();
            WealthLab.PositionSize size2 = new WealthLab.PositionSize();
            this.status = new StatusStrip();
            this.btnStreaming = new ToolStripButton();
            this.btnPV = new ToolStripButton();
            this.btnLink = new ToolStripButton();
            this.statusAddStrategy = new ToolStripStatusLabel();
            this.statusAddToSC = new ToolStripStatusLabel();
            this.statusOptimize = new ToolStripStatusLabel();
            this.stlblProfit = new ToolStripStatusLabel();
            this.stlblBHProfit = new ToolStripStatusLabel();
            this.stlblPerBar = new ToolStripStatusLabel();
            this.stlblBHPerBar = new ToolStripStatusLabel();
            this.stlblLastDate = new ToolStripStatusLabel();
            this.stlblBars = new ToolStripStatusLabel();
            this.popupChart = new ContextMenuStrip(this.icontainer_0);
            this.mniChartBuy = new ToolStripMenuItem();
            this.mniChartSell = new ToolStripMenuItem();
            this.sepBuySell = new ToolStripSeparator();
            this.mniChartShort = new ToolStripMenuItem();
            this.mniChartCover = new ToolStripMenuItem();
            this.sepShortCover = new ToolStripSeparator();
            this.mniAddStrategy = new ToolStripMenuItem();
            this.mniAddDifferent = new ToolStripMenuItem();
            this.mniPlotIndicator = new ToolStripMenuItem();
            this.plotAFundamentalDataItemOnTheChartToolStripMenuItem = new ToolStripMenuItem();
            this.mniPushCode = new ToolStripMenuItem();
            this.sepPlot = new ToolStripSeparator();
            this.mniCopyChart = new ToolStripMenuItem();
            this.mniCopyPriceData = new ToolStripMenuItem();
            this.mniAddSymbolToDataSet = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.mniPrintAll = new ToolStripMenuItem();
            this.sepCopyChart = new ToolStripSeparator();
            this.mniIndicatorProperties = new ToolStripMenuItem();
            this.mniDeleteIndicator = new ToolStripMenuItem();
            this.sepIndicators = new ToolStripSeparator();
            this.mniDrawingObjectProperties = new ToolStripMenuItem();
            this.mniDeleteDrawingObject = new ToolStripMenuItem();
            this.sepDrawing = new ToolStripSeparator();
            this.mniChartOptions = new ToolStripMenuItem();
            this.mniChartStyleSettings = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.mniReload = new ToolStripMenuItem();
            this.mniStockSplit = new ToolStripMenuItem();
            this.enableDisableStreamingHiddenMenuItem = new ToolStripMenuItem();
            this.editBarToolStripMenuItem = new ToolStripMenuItem();
            this.tabChart = new TabControl();
            this.pageChart = new TabPage();
            this.pnlSymbol = new Panel();
            this.txtSymbol = new TextBox();
            this.lblSymbol = new Label();
            this.statusChart = new StatusStrip();
            this.stlblBar = new ToolStripStatusLabel();
            this.stlblDate = new ToolStripStatusLabel();
            this.stlblOpen = new ToolStripStatusLabel();
            this.stlblHigh = new ToolStripStatusLabel();
            this.stlblLow = new ToolStripStatusLabel();
            this.stlblClose = new ToolStripStatusLabel();
            this.stlblVolume = new ToolStripStatusLabel();
            this.stlblMouse = new ToolStripStatusLabel();
            this.pnlMultiSymbol = new Panel();
            this.barRange = new BarDataRangeSelecter();
            this.grpMultiSymbol = new GroupBox();
            this.btnRunAllCancel = new Button();
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.lblRunAllStatus = new Label();
            this.lblStatus = new Label();
            this.progRunAll = new ProgressBar();
            this.lblProgress = new Label();
            this.btnRunAll = new Button();
            this.posSize = new PositionSizeSelecter();
            this.chart = new Chart();
            this.indicatorDragDropManager_0 = new IndicatorDragDropManager(this.icontainer_0);
            this.drawingObjectManager_0 = new DrawingObjectManager(this.icontainer_0);
            this.timer_0 = new System.Windows.Forms.Timer(this.icontainer_0);
            this.chartRenderer_0 = new ChartRenderer(this.icontainer_0);
            this.tradingSystemExecutor_1 = new TradingSystemExecutor(this.icontainer_0);
            this.barsLoader_0 = new BarsLoader(this.icontainer_0);
            this.fundamentalsLoader_0 = new FundamentalsLoader(this.icontainer_0);
            this.tradingSystemExecutor_0 = new TradingSystemExecutor(this.icontainer_0);
            this.streamingChartManager_0 = new StreamingChartManager(this.icontainer_0);
            this.marketHours_0 = new MarketHours(this.icontainer_0);
            this.drawingObjectManager_1 = new DrawingObjectManager(this.icontainer_0);
            this.status.SuspendLayout();
            this.popupChart.SuspendLayout();
            this.tabChart.SuspendLayout();
            this.pageChart.SuspendLayout();
            this.pnlSymbol.SuspendLayout();
            this.statusChart.SuspendLayout();
            this.pnlMultiSymbol.SuspendLayout();
            this.grpMultiSymbol.SuspendLayout();
            base.SuspendLayout();
            this.status.Items.AddRange(new ToolStripItem[] { this.btnStreaming, this.btnPV, this.btnLink, this.statusAddStrategy, this.statusAddToSC, this.statusOptimize, this.stlblProfit, this.stlblBHProfit, this.stlblPerBar, this.stlblBHPerBar, this.stlblLastDate, this.stlblBars });
            this.status.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.status.Location = new Point(0, 0x1b5);
            this.status.Name = "status";
            this.status.ShowItemToolTips = true;
            this.status.Size = new Size(0x3e3, 0x16);
            this.status.TabIndex = 0;
            this.status.Text = "statusStrip1";
            this.btnStreaming.Alignment = ToolStripItemAlignment.Right;
            this.btnStreaming.Image = (Image) manager.GetObject("btnStreaming.Image");
            this.btnStreaming.ImageTransparentColor = Color.Magenta;
            this.btnStreaming.Name = "btnStreaming";
            this.btnStreaming.Size = new Size(0x3d, 20);
            this.btnStreaming.Text = "Stream";
            this.btnStreaming.ToolTipText = "Enable/Disable Streaming";
            this.btnStreaming.CheckedChanged += new EventHandler(this.btnStreaming_CheckedChanged);
            this.btnStreaming.Click += new EventHandler(this.enableDisableStreamingHiddenMenuItem_Click);
            this.btnPV.Alignment = ToolStripItemAlignment.Right;
            this.btnPV.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnPV.Image = (Image) manager.GetObject("btnPV.Image");
            this.btnPV.ImageTransparentColor = Color.White;
            this.btnPV.Name = "btnPV";
            this.btnPV.Size = new Size(0x17, 20);
            this.btnPV.Text = "toolStripButton1";
            this.btnPV.ToolTipText = "Use Preferred Strategy Parameter Values";
            this.btnPV.Visible = false;
            this.btnPV.Click += new EventHandler(this.btnPV_Click);
            this.btnLink.Alignment = ToolStripItemAlignment.Right;
            this.btnLink.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLink.Image = (Image) manager.GetObject("btnLink.Image");
            this.btnLink.ImageTransparentColor = Color.Magenta;
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new Size(0x17, 20);
            this.btnLink.Text = "toolStripButton1";
            this.btnLink.ToolTipText = "Always update linked charts when Symbol changes";
            this.btnLink.Click += new EventHandler(this.btnLink_Click);
            this.statusAddStrategy.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusAddStrategy.IsLink = true;
            this.statusAddStrategy.Name = "statusAddStrategy";
            this.statusAddStrategy.Size = new Size(0x61, 0x11);
            this.statusAddStrategy.Text = "Open Strategy ...";
            this.statusAddStrategy.Click += new EventHandler(this.mniAddDifferent_Click);
            this.statusAddToSC.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusAddToSC.IsLink = true;
            this.statusAddToSC.Name = "statusAddToSC";
            this.statusAddToSC.Size = new Size(0x2f, 0x11);
            this.statusAddToSC.Text = "Monitor";
            this.statusAddToSC.Visible = false;
            this.statusAddToSC.Click += new EventHandler(this.statusAddToSC_Click);
            this.statusOptimize.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusOptimize.IsLink = true;
            this.statusOptimize.Name = "statusOptimize";
            this.statusOptimize.Size = new Size(0x34, 0x11);
            this.statusOptimize.Text = "Optimize";
            this.statusOptimize.Visible = false;
            this.statusOptimize.Click += new EventHandler(this.statusOptimize_Click);
            this.stlblProfit.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblProfit.Name = "stlblProfit";
            this.stlblProfit.Size = new Size(0x5c, 0x11);
            this.stlblProfit.Text = "Net Profit: $0.00";
            this.stlblProfit.ToolTipText = "Net Profit of the Strategy";
            this.stlblProfit.Visible = false;
            this.stlblBHProfit.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblBHProfit.Name = "stlblBHProfit";
            this.stlblBHProfit.Size = new Size(0x6c, 0x11);
            this.stlblBHProfit.Text = "BH Net Profit: $0.00";
            this.stlblBHProfit.ToolTipText = "Buy and Hold Net Profit";
            this.stlblBHProfit.Visible = false;
            this.stlblPerBar.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblPerBar.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.stlblPerBar.Name = "stlblPerBar";
            this.stlblPerBar.Size = new Size(0x7b, 0x11);
            this.stlblPerBar.Text = "Profit per Bar: $0.00";
            this.stlblPerBar.ToolTipText = "Efficiency of the Strategy";
            this.stlblPerBar.Visible = false;
            this.stlblBHPerBar.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblBHPerBar.Name = "stlblBHPerBar";
            this.stlblBHPerBar.Size = new Size(0x7e, 0x11);
            this.stlblBHPerBar.Text = "BH Profit per Bar: $0.00";
            this.stlblBHPerBar.ToolTipText = "Buy and Hold Efficiency";
            this.stlblBHPerBar.Visible = false;
            this.stlblLastDate.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblLastDate.Name = "stlblLastDate";
            this.stlblLastDate.Size = new Size(0x6c, 0x11);
            this.stlblLastDate.Text = "Last Date: 1/1/2007";
            this.stlblLastDate.ToolTipText = "The most recent Date in the Chart";
            this.stlblBars.Name = "stlblBars";
            this.stlblBars.Size = new Size(0x29, 0x11);
            this.stlblBars.Text = "Bars: 0";
            this.stlblBars.ToolTipText = "Number of Bars in the Chart";
            this.popupChart.Items.AddRange(new ToolStripItem[] { 
                this.mniChartBuy, this.mniChartSell, this.sepBuySell, this.mniChartShort, this.mniChartCover, this.sepShortCover, this.mniAddStrategy, this.mniAddDifferent, this.mniPlotIndicator, this.plotAFundamentalDataItemOnTheChartToolStripMenuItem, this.mniPushCode, this.sepPlot, this.mniCopyChart, this.mniCopyPriceData, this.mniAddSymbolToDataSet, this.mniPrint, 
                this.mniPrintAll, this.sepCopyChart, this.mniIndicatorProperties, this.mniDeleteIndicator, this.sepIndicators, this.mniDrawingObjectProperties, this.mniDeleteDrawingObject, this.sepDrawing, this.mniChartOptions, this.mniChartStyleSettings, this.toolStripSeparator1, this.mniReload, this.mniStockSplit, this.enableDisableStreamingHiddenMenuItem, this.editBarToolStripMenuItem
             });
            this.popupChart.Name = "popupChart";
            this.popupChart.Size = new Size(0x182, 0x23e);
            this.popupChart.Opening += new CancelEventHandler(this.popupChart_Opening);
            this.mniChartBuy.Image = (Image) manager.GetObject("mniChartBuy.Image");
            this.mniChartBuy.ImageTransparentColor = Color.Silver;
            this.mniChartBuy.Name = "mniChartBuy";
            this.mniChartBuy.Size = new Size(0x181, 0x16);
            this.mniChartBuy.Text = "Buy 100";
            this.mniChartBuy.Click += new EventHandler(this.mniChartBuy_Click);
            this.mniChartSell.Image = (Image) manager.GetObject("mniChartSell.Image");
            this.mniChartSell.ImageTransparentColor = Color.Silver;
            this.mniChartSell.Name = "mniChartSell";
            this.mniChartSell.Size = new Size(0x181, 0x16);
            this.mniChartSell.Text = "Sell 100";
            this.mniChartSell.Click += new EventHandler(this.mniChartSell_Click);
            this.sepBuySell.Name = "sepBuySell";
            this.sepBuySell.Size = new Size(0x17e, 6);
            this.mniChartShort.Image = (Image) manager.GetObject("mniChartShort.Image");
            this.mniChartShort.ImageTransparentColor = Color.Silver;
            this.mniChartShort.Name = "mniChartShort";
            this.mniChartShort.Size = new Size(0x181, 0x16);
            this.mniChartShort.Text = "Short 100";
            this.mniChartShort.Click += new EventHandler(this.mniChartShort_Click);
            this.mniChartCover.Image = (Image) manager.GetObject("mniChartCover.Image");
            this.mniChartCover.ImageTransparentColor = Color.Silver;
            this.mniChartCover.Name = "mniChartCover";
            this.mniChartCover.Size = new Size(0x181, 0x16);
            this.mniChartCover.Text = "Cover 100";
            this.mniChartCover.Click += new EventHandler(this.mniChartCover_Click);
            this.sepShortCover.Name = "sepShortCover";
            this.sepShortCover.Size = new Size(0x17e, 6);
            this.mniAddStrategy.Image = (Image) manager.GetObject("mniAddStrategy.Image");
            this.mniAddStrategy.ImageTransparentColor = Color.Fuchsia;
            this.mniAddStrategy.Name = "mniAddStrategy";
            this.mniAddStrategy.Size = new Size(0x181, 0x16);
            this.mniAddStrategy.Text = "Open a Strategy ...";
            this.mniAddStrategy.Click += new EventHandler(this.mniAddDifferent_Click);
            this.mniAddDifferent.Image = (Image) manager.GetObject("mniAddDifferent.Image");
            this.mniAddDifferent.ImageTransparentColor = Color.Fuchsia;
            this.mniAddDifferent.Name = "mniAddDifferent";
            this.mniAddDifferent.Size = new Size(0x181, 0x16);
            this.mniAddDifferent.Text = "Open a different Strategy ...";
            this.mniAddDifferent.Visible = false;
            this.mniAddDifferent.Click += new EventHandler(this.mniAddDifferent_Click);
            this.mniPlotIndicator.Image = (Image) manager.GetObject("mniPlotIndicator.Image");
            this.mniPlotIndicator.ImageTransparentColor = Color.Fuchsia;
            this.mniPlotIndicator.Name = "mniPlotIndicator";
            this.mniPlotIndicator.Size = new Size(0x181, 0x16);
            this.mniPlotIndicator.Text = "Plot an Indicator on the Chart ...";
            this.mniPlotIndicator.Click += new EventHandler(this.mniPlotIndicator_Click);
            this.plotAFundamentalDataItemOnTheChartToolStripMenuItem.Image = (Image) manager.GetObject("plotAFundamentalDataItemOnTheChartToolStripMenuItem.Image");
            this.plotAFundamentalDataItemOnTheChartToolStripMenuItem.ImageTransparentColor = Color.Fuchsia;
            this.plotAFundamentalDataItemOnTheChartToolStripMenuItem.Name = "plotAFundamentalDataItemOnTheChartToolStripMenuItem";
            this.plotAFundamentalDataItemOnTheChartToolStripMenuItem.Size = new Size(0x181, 0x16);
            this.plotAFundamentalDataItemOnTheChartToolStripMenuItem.Text = "Plot a Fundamental Data Item on the Chart ...";
            this.plotAFundamentalDataItemOnTheChartToolStripMenuItem.Click += new EventHandler(this.plotAFundamentalDataItemOnTheChartToolStripMenuItem_Click);
            this.mniPushCode.Image = Resources.wlp_push;
            this.mniPushCode.ImageTransparentColor = Color.Fuchsia;
            this.mniPushCode.Name = "mniPushCode";
            this.mniPushCode.Size = new Size(0x181, 0x16);
            this.mniPushCode.Text = "Push Indicator(s) and Fundamental Item(s) into Strategy Code";
            this.mniPushCode.Visible = false;
            this.mniPushCode.Click += new EventHandler(this.mniPushCode_Click);
            this.sepPlot.Name = "sepPlot";
            this.sepPlot.Size = new Size(0x17e, 6);
            this.mniCopyChart.Image = (Image) manager.GetObject("mniCopyChart.Image");
            this.mniCopyChart.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyChart.Name = "mniCopyChart";
            this.mniCopyChart.Size = new Size(0x181, 0x16);
            this.mniCopyChart.Text = "Copy Chart image to Clipboard";
            this.mniCopyChart.Click += new EventHandler(this.mniCopyChart_Click);
            this.mniCopyPriceData.ImageTransparentColor = Color.Fuchsia;
            this.mniCopyPriceData.Name = "mniCopyPriceData";
            this.mniCopyPriceData.Size = new Size(0x181, 0x16);
            this.mniCopyPriceData.Text = "Copy Price Data to Clipboard";
            this.mniCopyPriceData.Click += new EventHandler(this.mniCopyPriceData_Click);
            this.mniAddSymbolToDataSet.Enabled = false;
            this.mniAddSymbolToDataSet.Name = "mniAddSymbolToDataSet";
            this.mniAddSymbolToDataSet.Size = new Size(0x181, 0x16);
            this.mniAddSymbolToDataSet.Text = "Add this Symbol to the selected DataSet";
            this.mniAddSymbolToDataSet.Click += new EventHandler(this.mniAddSymbolToDataSet_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0x181, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.ToolTipText = "Print the chart image";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.mniPrintAll.Name = "mniPrintAll";
            this.mniPrintAll.Size = new Size(0x181, 0x16);
            this.mniPrintAll.Text = "Print All";
            this.mniPrintAll.ToolTipText = "Print content from all tabs";
            this.mniPrintAll.Click += new EventHandler(this.mniPrintAll_Click);
            this.sepCopyChart.Name = "sepCopyChart";
            this.sepCopyChart.Size = new Size(0x17e, 6);
            this.mniIndicatorProperties.Image = (Image) manager.GetObject("mniIndicatorProperties.Image");
            this.mniIndicatorProperties.ImageTransparentColor = Color.Fuchsia;
            this.mniIndicatorProperties.Name = "mniIndicatorProperties";
            this.mniIndicatorProperties.Size = new Size(0x181, 0x16);
            this.mniIndicatorProperties.Text = "Change Indicator Properties";
            this.mniIndicatorProperties.Visible = false;
            this.mniIndicatorProperties.Click += new EventHandler(this.mniIndicatorProperties_Click);
            this.mniDeleteIndicator.Image = (Image) manager.GetObject("mniDeleteIndicator.Image");
            this.mniDeleteIndicator.ImageTransparentColor = Color.Fuchsia;
            this.mniDeleteIndicator.Name = "mniDeleteIndicator";
            this.mniDeleteIndicator.Size = new Size(0x181, 0x16);
            this.mniDeleteIndicator.Text = "Delete Indicator";
            this.mniDeleteIndicator.Visible = false;
            this.mniDeleteIndicator.Click += new EventHandler(this.mniDeleteIndicator_Click);
            this.sepIndicators.Name = "sepIndicators";
            this.sepIndicators.Size = new Size(0x17e, 6);
            this.sepIndicators.Visible = false;
            this.mniDrawingObjectProperties.Image = (Image) manager.GetObject("mniDrawingObjectProperties.Image");
            this.mniDrawingObjectProperties.ImageTransparentColor = Color.Fuchsia;
            this.mniDrawingObjectProperties.Name = "mniDrawingObjectProperties";
            this.mniDrawingObjectProperties.Size = new Size(0x181, 0x16);
            this.mniDrawingObjectProperties.Text = "Change Drawing Object Properties";
            this.mniDrawingObjectProperties.Visible = false;
            this.mniDrawingObjectProperties.Click += new EventHandler(this.mniDrawingObjectProperties_Click);
            this.mniDeleteDrawingObject.Image = (Image) manager.GetObject("mniDeleteDrawingObject.Image");
            this.mniDeleteDrawingObject.ImageTransparentColor = Color.Fuchsia;
            this.mniDeleteDrawingObject.Name = "mniDeleteDrawingObject";
            this.mniDeleteDrawingObject.Size = new Size(0x181, 0x16);
            this.mniDeleteDrawingObject.Text = "Delete Drawing Object";
            this.mniDeleteDrawingObject.Visible = false;
            this.mniDeleteDrawingObject.Click += new EventHandler(this.mniDeleteDrawingObject_Click);
            this.sepDrawing.Name = "sepDrawing";
            this.sepDrawing.Size = new Size(0x17e, 6);
            this.sepDrawing.Visible = false;
            this.mniChartOptions.Image = (Image) manager.GetObject("mniChartOptions.Image");
            this.mniChartOptions.ImageTransparentColor = Color.Fuchsia;
            this.mniChartOptions.Name = "mniChartOptions";
            this.mniChartOptions.ShortcutKeys = Keys.Control | Keys.F12;
            this.mniChartOptions.Size = new Size(0x181, 0x16);
            this.mniChartOptions.Text = "Chart Colors and Styles ...";
            this.mniChartOptions.Click += new EventHandler(this.mniChartOptions_Click);
            this.mniChartStyleSettings.Enabled = false;
            this.mniChartStyleSettings.Name = "mniChartStyleSettings";
            this.mniChartStyleSettings.ShortcutKeys = Keys.Control | Keys.Y;
            this.mniChartStyleSettings.Size = new Size(0x181, 0x16);
            this.mniChartStyleSettings.Text = "Chart Style Settings ...";
            this.mniChartStyleSettings.Click += new EventHandler(this.mniChartStyleSettings_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0x17e, 6);
            this.mniReload.Enabled = false;
            this.mniReload.Name = "mniReload";
            this.mniReload.Size = new Size(0x181, 0x16);
            this.mniReload.Text = "Reload Chart History";
            this.mniReload.Click += new EventHandler(this.mniReload_Click);
            this.mniStockSplit.Enabled = false;
            this.mniStockSplit.Name = "mniStockSplit";
            this.mniStockSplit.Size = new Size(0x181, 0x16);
            this.mniStockSplit.Text = "Process a Stock Split ...";
            this.mniStockSplit.Click += new EventHandler(this.mniStockSplit_Click);
            this.enableDisableStreamingHiddenMenuItem.Name = "enableDisableStreamingHiddenMenuItem";
            this.enableDisableStreamingHiddenMenuItem.ShortcutKeys = Keys.Alt | Keys.Control | Keys.Z;
            this.enableDisableStreamingHiddenMenuItem.Size = new Size(0x181, 0x16);
            this.enableDisableStreamingHiddenMenuItem.Text = "Enable/Disable Streaming";
            this.enableDisableStreamingHiddenMenuItem.Visible = false;
            this.enableDisableStreamingHiddenMenuItem.Click += new EventHandler(this.enableDisableStreamingHiddenMenuItem_Click);
            this.editBarToolStripMenuItem.Image = (Image) manager.GetObject("editBarToolStripMenuItem.Image");
            this.editBarToolStripMenuItem.Name = "editBarToolStripMenuItem";
            this.editBarToolStripMenuItem.Size = new Size(0x181, 0x16);
            this.editBarToolStripMenuItem.Text = "Edit Bar Data";
            this.editBarToolStripMenuItem.Click += new EventHandler(this.editBarToolStripMenuItem_Click);
            this.tabChart.Controls.Add(this.pageChart);
            this.tabChart.Dock = DockStyle.Fill; ///WYJ fix: original: .Fill;
            this.tabChart.ItemSize = new Size(0x2a, 0x12);
            this.tabChart.Location = new Point(0, 0);
            this.tabChart.Name = "tabChart";
            this.tabChart.SelectedIndex = 0;
            this.tabChart.Size = new Size(0x3e3, 0x1b5);
            this.tabChart.TabIndex = 3;
            this.tabChart.SelectedIndexChanged += new EventHandler(this.ChartForm_Activated);
            this.pageChart.Controls.Add(this.pnlSymbol);
            this.pageChart.Controls.Add(this.statusChart);
            this.pageChart.Controls.Add(this.pnlMultiSymbol);
            this.pageChart.Controls.Add(this.chart);

            ///WYJ fix, add data view to chart window
            Panel pnlDataView = new Panel();
            pnlDataView.Location = new Point(600, 4);
            pnlDataView.Name = "dataView";
            pnlDataView.Size = new Size(600,200);
            pnlDataView.ForeColor = Color.Blue;
            pnlDataView.BackColor = Color.Red;
            //pnlDataView.SizingGrip = false;
            //pnlDataView.TabIndex = 1;
            pnlDataView.Text = "Data View";
            this.pageChart.Controls.Add(pnlDataView);

            this.pageChart.Location = new Point(4, 0x16);
            this.pageChart.Name = "pageChart";
            this.pageChart.Padding = new Padding(3);
            this.pageChart.Size = new Size(0x3db, 0x19b);
            this.pageChart.TabIndex = 0;
            this.pageChart.Text = "Chart";
            this.pageChart.UseVisualStyleBackColor = true;
            this.pnlSymbol.Controls.Add(this.txtSymbol);
            this.pnlSymbol.Controls.Add(this.lblSymbol);
            this.pnlSymbol.Location = new Point(9, 7);
            this.pnlSymbol.Name = "pnlSymbol";
            this.pnlSymbol.Size = new Size(0xbd, 0x1c);
            this.pnlSymbol.TabIndex = 15;
            this.pnlSymbol.Visible = false;
            this.txtSymbol.AcceptsReturn = true;
            this.txtSymbol.CharacterCasing = CharacterCasing.Upper;
            this.txtSymbol.Location = new Point(0x53, 4);
            this.txtSymbol.Multiline = true;
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new Size(100, 20);
            this.txtSymbol.TabIndex = 1;
            this.txtSymbol.KeyPress += new KeyPressEventHandler(this.txtSymbol_KeyPress);
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Location = new Point(4, 4);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new Size(0x48, 13);
            this.lblSymbol.TabIndex = 0;
            this.lblSymbol.Text = "Enter Symbol:";
            this.statusChart.Items.AddRange(new ToolStripItem[] { this.stlblBar, this.stlblDate, this.stlblOpen, this.stlblHigh, this.stlblLow, this.stlblClose, this.stlblVolume, this.stlblMouse });
            this.statusChart.Location = new Point(3, 0x182);
            this.statusChart.Name = "statusChart";
            this.statusChart.Size = new Size(0x3d5, 0x16);
            this.statusChart.SizingGrip = false;
            this.statusChart.TabIndex = 1;
            this.statusChart.Text = "statusStrip1";
            this.stlblBar.AutoSize = false;
            this.stlblBar.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblBar.Name = "stlblBar";
            this.stlblBar.Size = new Size(120, 0x11);
            this.stlblBar.Spring = true;
            this.stlblBar.Text = "Bar:";
            this.stlblBar.TextAlign = ContentAlignment.MiddleLeft;
            this.stlblDate.AutoSize = false;
            this.stlblDate.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblDate.Name = "stlblDate";
            this.stlblDate.Size = new Size(120, 0x11);
            this.stlblDate.Spring = true;
            this.stlblDate.TextAlign = ContentAlignment.MiddleLeft;
            this.stlblOpen.AutoSize = false;
            this.stlblOpen.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblOpen.Name = "stlblOpen";
            this.stlblOpen.Size = new Size(120, 0x11);
            this.stlblOpen.Spring = true;
            this.stlblOpen.Text = "O:";
            this.stlblOpen.TextAlign = ContentAlignment.MiddleLeft;
            this.stlblHigh.AutoSize = false;
            this.stlblHigh.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblHigh.Name = "stlblHigh";
            this.stlblHigh.Size = new Size(120, 0x11);
            this.stlblHigh.Spring = true;
            this.stlblHigh.Text = "H:";
            this.stlblHigh.TextAlign = ContentAlignment.MiddleLeft;
            this.stlblLow.AutoSize = false;
            this.stlblLow.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblLow.Name = "stlblLow";
            this.stlblLow.Size = new Size(120, 0x11);
            this.stlblLow.Spring = true;
            this.stlblLow.Text = "L:";
            this.stlblLow.TextAlign = ContentAlignment.MiddleLeft;
            this.stlblClose.AutoSize = false;
            this.stlblClose.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblClose.Name = "stlblClose";
            this.stlblClose.Size = new Size(120, 0x11);
            this.stlblClose.Spring = true;
            this.stlblClose.Text = "C:";
            this.stlblClose.TextAlign = ContentAlignment.MiddleLeft;
            this.stlblVolume.AutoSize = false;
            this.stlblVolume.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.stlblVolume.Name = "stlblVolume";
            this.stlblVolume.Size = new Size(120, 0x11);
            this.stlblVolume.Spring = true;
            this.stlblVolume.Text = "V:";
            this.stlblVolume.TextAlign = ContentAlignment.MiddleLeft;
            this.stlblMouse.AutoSize = false;
            this.stlblMouse.Name = "stlblMouse";
            this.stlblMouse.Padding = new Padding(2, 0, 2, 0);
            this.stlblMouse.Size = new Size(120, 0x11);
            this.stlblMouse.Spring = true;
            this.stlblMouse.Text = "Mouse:";
            this.stlblMouse.TextAlign = ContentAlignment.MiddleLeft;
            this.pnlMultiSymbol.BackColor = SystemColors.Control;
            this.pnlMultiSymbol.Controls.Add(this.barRange);
            this.pnlMultiSymbol.Controls.Add(this.grpMultiSymbol);
            this.pnlMultiSymbol.Controls.Add(this.posSize);
            this.pnlMultiSymbol.Dock = DockStyle.Fill;
            this.pnlMultiSymbol.Location = new Point(3, 3);
            this.pnlMultiSymbol.Name = "pnlMultiSymbol";
            this.pnlMultiSymbol.Size = new Size(0x3d5, 0x195);
            this.pnlMultiSymbol.TabIndex = 14;
            this.pnlMultiSymbol.Visible = false;
            this.barRange.BackColor = Color.AliceBlue;
            this.barRange.IsStreaming = false;
            this.barRange.Location = new Point(5, 0x98);
            this.barRange.Name = "barRange";
            this.barRange.Size = new Size(0x5f, 20);
            this.barRange.TabIndex = 13;
            this.barRange.Visible = false;
            this.grpMultiSymbol.Controls.Add(this.btnRunAllCancel);
            this.grpMultiSymbol.Controls.Add(this.lblRunAllStatus);
            this.grpMultiSymbol.Controls.Add(this.lblStatus);
            this.grpMultiSymbol.Controls.Add(this.progRunAll);
            this.grpMultiSymbol.Controls.Add(this.lblProgress);
            this.grpMultiSymbol.Controls.Add(this.btnRunAll);
            this.grpMultiSymbol.Location = new Point(5, 3);
            this.grpMultiSymbol.Name = "grpMultiSymbol";
            this.grpMultiSymbol.Size = new Size(0x13b, 0x85);
            this.grpMultiSymbol.TabIndex = 0;
            this.grpMultiSymbol.TabStop = false;
            this.grpMultiSymbol.Text = "Multi Symbol Backtest";
            this.btnRunAllCancel.Enabled = false;
            this.btnRunAllCancel.ImageAlign = ContentAlignment.MiddleLeft;
            this.btnRunAllCancel.ImageIndex = 1;
            this.btnRunAllCancel.ImageList = this.imageList_0;
            this.btnRunAllCancel.Location = new Point(0xe7, 0x43);
            this.btnRunAllCancel.Name = "btnRunAllCancel";
            this.btnRunAllCancel.Size = new Size(0x4b, 0x17);
            this.btnRunAllCancel.TabIndex = 5;
            this.btnRunAllCancel.Text = "Cancel";
            this.btnRunAllCancel.UseVisualStyleBackColor = true;
            this.btnRunAllCancel.Click += new EventHandler(this.btnRunAllCancel_Click);
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "Execute.bmp");
            this.imageList_0.Images.SetKeyName(1, "Delete.bmp");
            this.lblRunAllStatus.AutoSize = true;
            this.lblRunAllStatus.ForeColor = SystemColors.ActiveCaption;
            this.lblRunAllStatus.Location = new Point(0x39, 0x6b);
            this.lblRunAllStatus.Name = "lblRunAllStatus";
            this.lblRunAllStatus.Size = new Size(0x61, 13);
            this.lblRunAllStatus.TabIndex = 4;
            this.lblRunAllStatus.Text = "Waiting to Execute";
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new Point(11, 0x6b);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(40, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status:";
            this.progRunAll.Location = new Point(10, 0x43);
            this.progRunAll.Name = "progRunAll";
            this.progRunAll.Size = new Size(0xd6, 0x17);
            this.progRunAll.TabIndex = 2;
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new Point(7, 50);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new Size(0x30, 13);
            this.lblProgress.TabIndex = 1;
            this.lblProgress.Text = "Progress";
            this.btnRunAll.ImageAlign = ContentAlignment.MiddleLeft;
            this.btnRunAll.ImageIndex = 0;
            this.btnRunAll.ImageList = this.imageList_0;
            this.btnRunAll.Location = new Point(7, 20);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new Size(0x12b, 0x17);
            this.btnRunAll.TabIndex = 0;
            this.btnRunAll.Text = "Backtest the Strategy on all Symbols in the DataSet";
            this.btnRunAll.UseVisualStyleBackColor = true;
            this.btnRunAll.Click += new EventHandler(this.btnRunAll_Click);
            this.posSize.BackColor = Color.Honeydew;
            this.posSize.CombinationStrategyChildMode = false;
            this.posSize.Location = new Point(5, 0xb2);
            this.posSize.Name = "posSize";
            this.posSize.Size = new Size(0x5f, 20);
            this.posSize.TabIndex = 12;
            this.posSize.Visible = false;
            this.chart.AllowDrop = true;
            //this.chart.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.chart.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top; ///WYJ fix
            this.chart.ContextMenuStrip = this.popupChart;
            this.chart.Cursor = Cursors.Default;
            this.chart.DragDropManager = this.indicatorDragDropManager_0;
            this.chart.DrawingManager = this.drawingObjectManager_0;
            this.chart.FundamentalTooltipVisible = true;
            this.chart.HandleFont = new Font("Tahoma", 7f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.chart.IndicatorTooltipVisible = true;
            this.chart.Location = new Point(3, 3);
            this.chart.MultiSymbolMode = false;
            this.chart.Name = "chart";
            this.chart.PriceTooltipVisible = true;
            this.chart.Renderer = null;
            this.chart.ScrollBarVisible = true;
            this.chart.Size = new Size(0x3d5, 380);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            this.chart.DoubleClick += new EventHandler(this.chart_DoubleClick);
            this.chart.MouseLeave += new EventHandler(this.chart_MouseLeave);
            this.chart.Click += new EventHandler(this.chart_Click);
            this.chart.MouseWheelMoved += new EventHandler<MouseEventArgs>(this.mouseWheelHandler);
            this.chart.DrawingObjectOperationCompleted += new EventHandler<EventArgs>(this.method_29);
            this.chart.OnException += new EventHandler<ExceptionEventArgs>(this.method_72);
            this.chart.KeyPress += new KeyPressEventHandler(this.chart_KeyPress);
            this.chart.MouseMoveBarNumber += new EventHandler<BarNumberEventArgs>(this.method_9);

            //base.KeyDown += new KeyEventHandler(this.MainForm_KeyDown);
            //base.PreviewKeyDown += new PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);

            this.indicatorDragDropManager_0.Fundamentals = null;
            this.indicatorDragDropManager_0.IndicatorDropped += new EventHandler<DroppedIndicatorEventArgs>(this.method_42);
            this.drawingObjectManager_0.ChartBookName = "Standard";
            this.drawingObjectManager_0.RootPath = null;
            this.timer_0.Interval = 0x14d;
            this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
            this.chartRenderer_0.AxisFont = new Font("Tahoma", 7f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.chartRenderer_0.BackgroundColor = Color.White;
            this.chartRenderer_0.BarSpacing = 6;
            this.chartRenderer_0.DownBarColor = Color.Red;
            this.chartRenderer_0.DownBarVolumeColor = Color.Teal;
            this.chartRenderer_0.Executor = this.tradingSystemExecutor_1;
            this.chartRenderer_0.FundamentalGlyphs = "";
            this.chartRenderer_0.Fundamentals = this.fundamentalsLoader_0;
            this.chartRenderer_0.FundamentalsVisible = true;
            this.chartRenderer_0.GridlineColor = Color.Gainsboro;
            this.chartRenderer_0.HorizontalGridines = true;
            this.chartRenderer_0.IndicatorLabelsVisible = true;
            this.chartRenderer_0.LogScale = false;
            this.chartRenderer_0.MarginBottomColor = Color.Navy;
            this.chartRenderer_0.MarginBottomHeight = 20;
            this.chartRenderer_0.MarginRightColor = Color.Gainsboro;
            this.chartRenderer_0.MarginRightWidth = 50;
            this.chartRenderer_0.PaneSeparatorColor = Color.Black;
            this.chartRenderer_0.PaneSeparatorVisible = true;
            this.chartRenderer_0.PlotStops = false;
            this.chartRenderer_0.RightPaddingBars = 0;
            this.chartRenderer_0.ScrollOffset = 0;
            this.chartRenderer_0.TitleFont = new Font("Verdana", 8f);
            this.chartRenderer_0.TradeAnnotationsVisible = true;
            this.chartRenderer_0.TradeArrowsVisible = true;
            this.chartRenderer_0.TradeCirclesVisible = true;
            this.chartRenderer_0.UpBarColor = Color.Navy;
            this.chartRenderer_0.UpBarVolumeColor = Color.Teal;
            this.chartRenderer_0.VerticalGridlines = true;
            this.chartRenderer_0.VolumePaneVisible = true;
            this.tradingSystemExecutor_1.ApplyCommission = false;
            this.tradingSystemExecutor_1.ApplyDividends = false;
            this.tradingSystemExecutor_1.ApplyInterest = false;
            this.tradingSystemExecutor_1.BarsLoader = this.barsLoader_0;
            this.tradingSystemExecutor_1.BenchmarkBuyAndHoldON = false;
            this.tradingSystemExecutor_1.BenchmarkSymbol = null;
            this.tradingSystemExecutor_1.BuildEquityCurves = true;
            this.tradingSystemExecutor_1.CashRate = 0.0;
            this.tradingSystemExecutor_1.EnableSlippage = false;
            this.tradingSystemExecutor_1.ExceptionEvents = true;
            this.tradingSystemExecutor_1.FundamentalsLoader = this.fundamentalsLoader_0;
            this.tradingSystemExecutor_1.IsStreaming = false;
            this.tradingSystemExecutor_1.LimitDaySimulation = false;
            this.tradingSystemExecutor_1.LimitOrderSlippage = false;
            this.tradingSystemExecutor_1.MarginRate = 0.0;
            this.tradingSystemExecutor_1.NoDecimalRoundingForLimitStopPrice = false;
            this.tradingSystemExecutor_1.OverrideShareSize = 0.0;
            size.DollarSize = 5000.0;
            size.MarginFactor = 1.0;
            size.Mode = PosSizeMode.RawProfitDollar;
            size.OverrideShareSize = 0.0;
            size.PctSize = 10.0;
            size.PosSizerConfig = "";
            size.RawProfitDollarSize = 5000.0;
            size.RawProfitShareSize = 100.0;
            size.RiskSize = 3.0;
            size.ShareSize = 100.0;
            size.SimuScriptName = "";
            size.StartingCapital = 50000.0;
            this.tradingSystemExecutor_1.PosSize = size;
            this.tradingSystemExecutor_1.PricingDecimalPlaces = 0;
            this.tradingSystemExecutor_1.RedcuceQtyPct = 10.0;
            this.tradingSystemExecutor_1.ReduceQtyBasedOnVolume = false;
            this.tradingSystemExecutor_1.Renderer = this.chartRenderer_0;
            this.tradingSystemExecutor_1.RoundLots = false;
            this.tradingSystemExecutor_1.RoundLots50 = false;
            this.tradingSystemExecutor_1.SlippageTicks = 1;
            this.tradingSystemExecutor_1.SlippageUnits = 1.0;
            this.tradingSystemExecutor_1.Strategy = null;
            this.tradingSystemExecutor_1.StrategyName = "";
            this.tradingSystemExecutor_1.WorstTradeSimulation = false;
            this.tradingSystemExecutor_1.ChartBitmapRequested += new EventHandler<ChartBitmapEventArgs>(this.method_38);
            this.tradingSystemExecutor_1.ExecutionCompletedForSymbol += new EventHandler<BarsEventArgs>(this.method_30);
            this.tradingSystemExecutor_1.LookupStrategy += new EventHandler<StrategyEventArgs>(this.method_73);
            this.tradingSystemExecutor_1.TrendlineGetValue += new EventHandler<TrendLineEventArgs>(this.method_46);
            this.tradingSystemExecutor_1.ClearDebugWindow += new EventHandler<EventArgs>(this.method_52);
            this.tradingSystemExecutor_1.SetParameterValues += new EventHandler<StrategyParameterEventArgs>(this.method_69);
            this.tradingSystemExecutor_1.LookupDataSource += new EventHandler<DataSourceLookupEventArgs>(this.method_74);
            this.tradingSystemExecutor_1.FlushDebugWindow += new EventHandler<EventArgs>(this.method_35);
            this.tradingSystemExecutor_1.PrintToStatusBar += new EventHandler<DebugStringEventArgs>(this.method_37);
            this.tradingSystemExecutor_1.WealthScriptException += new EventHandler<WSExceptionEventArgs>(this.method_33);
            this.tradingSystemExecutor_1.ExternalSymbolRequested += new EventHandler<LoadSymbolEventArgs>(this.method_34);
            this.tradingSystemExecutor_1.ExternalSymbolFromDataSetRequested += new EventHandler<LoadSymbolFromDataSetEventArgs>(this.method_67);
            this.tradingSystemExecutor_1.ExecutionCompletedForChildStrategySymbol += new EventHandler<BarsEventArgs>(this.method_75);
            this.barsLoader_0.AutoConvertScale = true;
            this.barsLoader_0.AutoCreateProvider = true;
            this.barsLoader_0.BarInterval = 0;
            this.barsLoader_0.EndDate = new DateTime(0L);
            this.barsLoader_0.IncludePartialBar = false;
            this.barsLoader_0.MaxBars = 0;
            this.barsLoader_0.OverrideOnDemand = false;
            this.barsLoader_0.OverrideOnDemandValue = false;
            this.barsLoader_0.Scale = BarScale.Daily;
            this.barsLoader_0.StartDate = new DateTime(0x7cb, 1, 1, 0, 0, 0, 0);
            this.tradingSystemExecutor_0.ApplyCommission = false;
            this.tradingSystemExecutor_0.ApplyDividends = false;
            this.tradingSystemExecutor_0.ApplyInterest = false;
            this.tradingSystemExecutor_0.BarsLoader = this.barsLoader_0;
            this.tradingSystemExecutor_0.BenchmarkBuyAndHoldON = false;
            this.tradingSystemExecutor_0.BenchmarkSymbol = null;
            this.tradingSystemExecutor_0.BuildEquityCurves = false;
            this.tradingSystemExecutor_0.CashRate = 0.0;
            this.tradingSystemExecutor_0.EnableSlippage = false;
            this.tradingSystemExecutor_0.ExceptionEvents = false;
            this.tradingSystemExecutor_0.FundamentalsLoader = this.fundamentalsLoader_0;
            this.tradingSystemExecutor_0.IsStreaming = false;
            this.tradingSystemExecutor_0.LimitDaySimulation = false;
            this.tradingSystemExecutor_0.LimitOrderSlippage = false;
            this.tradingSystemExecutor_0.MarginRate = 0.0;
            this.tradingSystemExecutor_0.NoDecimalRoundingForLimitStopPrice = false;
            this.tradingSystemExecutor_0.OverrideShareSize = 0.0;
            size2.DollarSize = 5000.0;
            size2.MarginFactor = 1.0;
            size2.Mode = PosSizeMode.RawProfitDollar;
            size2.OverrideShareSize = 0.0;
            size2.PctSize = 10.0;
            size2.PosSizerConfig = "";
            size2.RawProfitDollarSize = 5000.0;
            size2.RawProfitShareSize = 100.0;
            size2.RiskSize = 3.0;
            size2.ShareSize = 100.0;
            size2.SimuScriptName = "";
            size2.StartingCapital = 50000.0;
            this.tradingSystemExecutor_0.PosSize = size2;
            this.tradingSystemExecutor_0.PricingDecimalPlaces = 0;
            this.tradingSystemExecutor_0.RedcuceQtyPct = 10.0;
            this.tradingSystemExecutor_0.ReduceQtyBasedOnVolume = false;
            this.tradingSystemExecutor_0.Renderer = this.chartRenderer_0;
            this.tradingSystemExecutor_0.RoundLots = false;
            this.tradingSystemExecutor_0.RoundLots50 = false;
            this.tradingSystemExecutor_0.SlippageTicks = 1;
            this.tradingSystemExecutor_0.SlippageUnits = 1.0;
            this.tradingSystemExecutor_0.Strategy = null;
            this.tradingSystemExecutor_0.StrategyName = "";
            this.tradingSystemExecutor_0.WorstTradeSimulation = false;
            this.tradingSystemExecutor_0.SetParameterValues += new EventHandler<StrategyParameterEventArgs>(this.method_69);
            this.tradingSystemExecutor_0.ExternalSymbolRequested += new EventHandler<LoadSymbolEventArgs>(this.method_34);
            this.tradingSystemExecutor_0.ExternalSymbolFromDataSetRequested += new EventHandler<LoadSymbolFromDataSetEventArgs>(this.method_67);
            this.streamingChartManager_0.BarsLocked += new EventHandler<EventArgs>(this.method_70);
            this.streamingChartManager_0.NewBar += new EventHandler<EventArgs>(this.method_40);
            this.drawingObjectManager_1.ChartBookName = "Standard";
            this.drawingObjectManager_1.RootPath = null;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x3e3, 0x1cb);
            base.Controls.Add(this.tabChart);
            base.Controls.Add(this.status);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.KeyPreview = true;
            base.Name = "ChartForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.WindowsDefaultBounds;
            this.Text = "Chart";
            base.Load += new EventHandler(this.ChartForm_Load);
            base.Activated += new EventHandler(this.ChartForm_Activated);
            base.FormClosed += new FormClosedEventHandler(this.ChartForm_FormClosed);
            base.FormClosing += new FormClosingEventHandler(this.ChartForm_FormClosing);
            base.Resize += new EventHandler(this.ChartForm_Resize);
            base.KeyDown += new KeyEventHandler(this.ChartForm_KeyDown);
            base.TextChanged += new EventHandler(this.ChartForm_TextChanged);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.popupChart.ResumeLayout(false);
            this.tabChart.ResumeLayout(false);
            this.pageChart.ResumeLayout(false);
            this.pageChart.PerformLayout();
            this.pnlSymbol.ResumeLayout(false);
            this.pnlSymbol.PerformLayout();
            this.statusChart.ResumeLayout(false);
            this.statusChart.PerformLayout();
            this.pnlMultiSymbol.ResumeLayout(false);
            this.grpMultiSymbol.ResumeLayout(false);
            this.grpMultiSymbol.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadDragDropIndicators(string indicatorString)
        {
            this.indicatorDragDropManager_0.Clear();
            if ((indicatorString != "") && (indicatorString != null))
            {
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(indicatorString));
                try
                {
                    stream.Position = 0L;
                    this.indicatorDragDropManager_0.ReadFromStream(stream);
                }
                finally
                {
                    stream.Close();
                }
            }
            this.mniPushCode.Visible = this.indicatorDragDropManager_0.HasDragDroppedIndicators;
        }

        public void LoadWorkspaceItems(IList<string> items, int version)
        {
            bool flag;
            bool flag2;
            this.bool_13 = true;
            if (items[0] == "")
            {
                this.DataSource = null;
            }
            else
            {
                this.DataSource = MainModule.Instance.DataSources.FindDataSource(items[0]);
            }
            this.Symbol = items[1];
            this.BarDataScale = WealthLab.BarDataScale.Parse(items[2]);
            this.DataRange = BarDataRange.Parse(items[3]);
            this.PositionSize = WealthLab.PositionSize.Parse(items[4]);
            this.tradingSystemExecutor_1.PosSize = this.PositionSize;
            this.StatusPanelsVisible = bool.Parse(items[5]);
            this.chartRenderer_0.IndicatorLabelsVisible = bool.Parse(items[6]);
            this.chartRenderer_0.FundamentalsVisible = bool.Parse(items[7]);
            this.btnLink.Checked = bool.Parse(items[8]);
            this.btnStreaming.Checked = bool.Parse(items[9]);
            if (items[5] != "")
            {
                this.Strategy = MainModule.Instance.Strategies.LookupID(items[10]);
            }
            if (version >= 2)
            {
                this.chartRenderer_0.LogScale = bool.Parse(items[11]);
            }
            if ((version >= 3) && (flag2 = bool.Parse(items[12])))
            {
                if (this.alerts_0 == null)
                {
                    this.method_51();
                }
                this.alerts_0.AutoStage = flag2;
            }
            int num = 13;
            if (version >= 4)
            {
                int num2 = int.Parse(items[num++]) + num;
                this.list_1.Clear();
                while (num < num2)
                {
                    string str = items[num];
                    string[] strArray = str.Split(new char[] { '|' });
                    string str3 = "";
                    string str2 = "";
                    if (strArray.Length >= 2)
                    {
                        str3 = strArray[0];
                        if (strArray.Length > 2)
                        {
                            str2 = str.Substring(str.IndexOf('|') + 1);
                        }
                        else
                        {
                            str2 = strArray[1];
                        }
                    }
                    else
                    {
                        str3 = str;
                    }
                    using (IEnumerator<IPerformanceVisualizer> enumerator = MainModule.Instance.Visualizers.GetEnumerator())
                    {
                        IPerformanceVisualizer current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (current.TabText == str3)
                            {
                                ///goto  Label_024A;   ///WYJ fix, simplify the flow
                                this.method_31(current);
                                if (current is ISettingsProvider)
                                {
                                    this.dictionary_1[str3] = str2;
                                }
                                break;
                            }
                        }
                    }
                    num++;
                }
            }
            if ((version >= 5) && (num < items.Count))
            {
                this.LoadDragDropIndicators(items[num++]);
            }
            if ((version >= 6) && (num < items.Count))
            {
                this.chartRenderer_0.RestoreResizedPanes(items[num++]);
            }
            if (((version >= 7) && (num < items.Count)) && (flag = bool.Parse(items[num++])))
            {
                if (this.alerts_0 == null)
                {
                    this.method_51();
                }
                this.alerts_0.EmailAlerts = flag;
            }
            if ((version >= 8) && (num < items.Count))
            {
                this.chartRenderer_0.RestoreHiddenPaneOrigHeights(items[num++]);
            }
        }

        public string LookupStrategyName(string strategyID)
        {
            WealthLab.Strategy iD = MainModule.Instance.Strategies.LookupID(strategyID);
            if (iD != null)
            {
                return iD.Name;
            }
            return ("Unknown Strategy: " + strategyID);
        }

        private void method_0(object sender, ScaleChangeEventArgs e)
        {
            if (this.method_17(e.ChartScale))
            {
                this.chart.CancelScaleChange();
            }
            else
            {
                this.MyMainForm.ChangeScale(e.ChartScale.Scale, e.ChartScale.BarInterval);
                this.MyMainForm.BarDataScale = e.ChartScale;
            }
        }

        private void method_1()
        {
            if (this.tabChart != null)
            {
                foreach (TabPage page in this.tabChart.TabPages)
                {
                    if (page != null)
                    {
                        page.Cursor = Cursors.Default;
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }

        private string method_10(int int_6)
        {
            string str = this.Bars.Date[int_6].ToShortDateString();
            if (this.Bars.IsIntraday)
            {
                str = str + " " + this.Bars.Date[int_6].ToShortTimeString();
            }
            return str;
        }

        private void method_11(int int_6, bool bool_18, bool bool_19) ///WYJ fix: update datawindow
        {
            if (DataWindowForm.Instance != null)
            {
                DataWindowForm instance = DataWindowForm.Instance;
                instance.method_0(this);
                this.int_3 = (int_6 == -1) ? this.int_3 : int_6;
                string format = "N" + this.Bars.SymbolInfo.Decimals;
                if (bool_18)
                {
                    instance.method_2(DataWindowType.Date, this.method_10(this.int_3));
                    instance.method_2(DataWindowType.Open, this.Bars.Open[this.int_3].ToString(format));
                    instance.method_2(DataWindowType.High, this.Bars.High[this.int_3].ToString(format));
                    instance.method_2(DataWindowType.Low, this.Bars.Low[this.int_3].ToString(format));
                    instance.method_2(DataWindowType.Close, this.Bars.Close[this.int_3].ToString(format));
                    instance.method_2(DataWindowType.Volume, this.Bars.Volume[this.int_3].ToString("N0"));
                }
                if (bool_19)
                {
                    instance.method_4();
                }
                foreach (ChartPane pane in this.chart.Renderer.Panes)
                {
                    if (pane.Description != "V")
                    {
                        foreach (PlottedSymbol symbol in pane.PlottedSymbols)
                        {
                            string str2 = symbol.Bars.Symbol;
                            instance.method_3("Open(" + str2 + ")", symbol.Bars.Open[this.int_3].ToString(format));
                            instance.method_3("High(" + str2 + ")", symbol.Bars.High[this.int_3].ToString(format));
                            instance.method_3("Low(" + str2 + ")", symbol.Bars.Low[this.int_3].ToString(format));
                            instance.method_3("Close(" + str2 + ")", symbol.Bars.Close[this.int_3].ToString(format));
                            instance.method_3("Volume(" + str2 + ")", symbol.Bars.Volume[this.int_3].ToString("N0"));
                        }
                        foreach (PlottedIndicator indicator in pane.PlottedIndicators)
                        {
                            instance.method_3(indicator.Series.Description, indicator.Series[this.int_3].ToString("N" + DecimalsManager.Instance.Indicator));
                        }
                    }
                }
                instance.method_1();
            }
        }

        private void method_12(bool bool_18)
        {
            if (this.MyMainForm.ActiveMdiChild == this)
            {
                this.MyMainForm.EnableControls(bool_18);
            }
            if (this.editor_0 != null)
            {
                this.editor_0.EnableControls(bool_18);
            }
            if (bool_18)
            {
                this.Cursor = Cursors.Default;
                this.chart.Mode = ChartMode.Normal;
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                this.chart.Mode = ChartMode.Wait;
            }
            this.posSize.Enabled = bool_18;
            if (this.editor_0 != null)
            {
                this.editor_0.EnableControls(bool_18);
            }
            foreach (TabPage page in this.tabChart.TabPages)
            {
                if (page.Controls.Count > 0)
                {
                    Control control = page.Controls[0];
                    if (control is IPerformanceVisualizer)
                    {
                        (control as IPerformanceVisualizer).EnableControls(bool_18);
                    }
                }
            }
            if (bool_18)
            {
                this.method_15();
            }
        }

        private void method_13(TradingSystemExecutor tradingSystemExecutor_2, WealthLab.Bars bars_2, bool bool_18)
        {
            if (this.combinationStrategyBuilder_0 != null)
            {
                if (this.MyMainForm.SelectingNodeForFormCreation)
                {
                    this.MyMainForm.SelectingNodeForFormCreation = false;
                    throw new Exception("CS Error");
                }
                if (this.Strategy.CombinedStrategyChildren.Count == 0)
                {
                    MessageBox.Show("Please add at least one Child Strategy to the Combined Strategy.");
                    throw new Exception("CS Error");
                }
                using (List<CombinedStrategyInfo>.Enumerator enumerator = this.Strategy.CombinedStrategyChildren.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        CombinedStrategyInfo current = enumerator.Current;
                        if (MainModule.Instance.Strategies.LookupID(current.StrategyID.ToString()) == null)
                        {
                            ///goto  Label_00AB;  ///WYJ fix, simplify the flow
                            MessageBox.Show("One or more the Child Strategies could not be located, and were possibly deleted.  Please remove the offending Child Strategy.");
                            throw new Exception("CS Error");
                        }
                    }
                }
                if (this.combinationStrategyBuilder_0.CheckForCircularReference())
                {
                    MessageBox.Show("The Child Strategies assigned to this Combined Strategy would create a never-ending circular reference if executed, please remove one or more of the Child Strategies.");
                    throw new Exception("CS Error");
                }
                base.Invoke(new Delegate43(this.method_14));
            }
            
            this.CSDataSource = this.DataSource;
            this.CSSymbol = this.Symbol;
            this.chartRenderer_0.PaneSeparatorVisible = MainModule.Instance.Renderer.PaneSeparatorVisible;
            try
            {
                this.bool_4 = false;
                tradingSystemExecutor_2.DataSet = this.DataSource;
                DateTime now = DateTime.Now;
                tradingSystemExecutor_2.StrategyName = this.Strategy.Name;
                tradingSystemExecutor_2.StrategyWindowID = this.int_1;
                if (bool_18)
                {
                    tradingSystemExecutor_2.Execute(this.Strategy, this.WealthScript, bars_2, this.list_0);
                }
                else
                {
                    tradingSystemExecutor_2.Execute(this.Strategy, this.WealthScript, bars_2);
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (this.editor_0 != null)
                {
                    this.editor_0.ExecutionTime = span;
                }
            }
            catch (ThreadAbortException exception)
            {
                AbortReason unknown = AbortReason.Unknown;
                if (exception.ExceptionState != null)
                {
                    Exception exception2;
                    unknown = AbortReason.Unknown;
                    unknown = AbortReason.Unknown;
                    switch (((AbortReason) exception.ExceptionState))
                    {
                        case AbortReason.Bars:
                            return;

                        case AbortReason.ESC:
                            exception2 = new Exception("Thread Aborted. User pressed ESC.");
                            this.method_26(-100, exception2);
                            return;

                        case AbortReason.Streaming:
                            exception2 = new Exception("Thread Aborted. Streaming status changed by user.");
                            this.method_26(-100, exception2);
                            return;
                    }
                }
                else
                {
                    unknown = AbortReason.Unknown;
                    unknown = AbortReason.Unknown;
                }
                this.method_26(-100, exception);
            }
            catch (Exception exception3)
            {
                this.method_26(-100, exception3);
            }
        }

        private void method_14()
        {
            this.combinationStrategyBuilder_0.AssignMaxToProgressControl();
        }

        private void method_15()
        {
            string str;
            if (this.Strategy != null)
            {
                str = "Strategy: ";
                if (this.Strategy.Name == "")
                {
                    str = str + "[Untitled]";
                }
                else
                {
                    str = str + this.Strategy.Name;
                    if (this.NeedSave)
                    {
                        str = str + "*";
                    }
                }
            }
            else
            {
                str = "Chart";
            }
            if (this.bool_10)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, " - ", this.DataSource.Name, " (", this.DataSource.Symbols.Count, " Symbols) " }) + this.barsLoader_0.BarDataScale.ToString();
            }
            else if (this.Bars != null)
            {
                string str2 = str;
                str = str2 + " - " + this.Bars.Symbol + " " + this.Bars.DataScale.ToString();
            }
            this.Text = str;
        }

        private void method_16()
        {
            this.bool_6 = false;
            if (!this.IsBusy && ((this.builder_0 == null) || this.builder_0.CompileIfNeeded()))
            {
                this.mniAddSymbolToDataSet.Enabled = false;
                if (((this.DataSource != null) && this.DataSource.Provider.CanModifySymbols) && !this.DataSource.Symbols.Contains(this.Symbol))
                {
                    this.mniAddSymbolToDataSet.Enabled = true;
                }
                if ((this.DataSource != null) && (this.DataSource.Provider != null))
                {
                    this.mniReload.Enabled = this.DataSource.Provider.CanDeleteSymbolDataFile;
                    this.mniStockSplit.Enabled = this.DataSource.Provider.CanEditSymbolDataFile;
                }
                else
                {
                    this.mniReload.Enabled = false;
                    this.mniStockSplit.Enabled = false;
                }
                if (!this.method_17(this.BarDataScale) && (this.DataSource != null))
                {
                    this.dataSource_0 = this.DataSource;
                    this.list_0.Clear();
                    if (this.bool_10 && !this.bool_3)
                    {
                        this.dataSource_1 = this.DataSource;
                        if (this.DataSource.Symbols.Count == 0)
                        {
                            return;
                        }
                        this.btnRunAllCancel.Enabled = true;
                        string local1 = this.DataSource.Symbols[0];
                        this.progRunAll.Maximum = (this.dataSource_0.Symbols.Count + 1) + 4;
                        this.lblRunAllStatus.Text = "Collecting Data ...";
                    }
                    else
                    {
                        if (this.Symbol == "")
                        {
                            return;
                        }
                        this.progRunAll.Maximum = 6;
                    }
                    this.dictionary_0[this.DataSource] = new WealthLab.BarDataScale(this.BarDataScale.Scale, this.BarDataScale.BarInterval);
                    this.method_12(false);
                    this.tradingSystemExecutor_1.Clear();
                    this.drawingObjectManager_0.Clear();
                    this.chart.Mode = ChartMode.Wait;
                    this.tradingSystemExecutor_1.ApplySettings(MainModule.Instance.Executor);
                    this.tradingSystemExecutor_1.PosSize = this.PositionSize;
                    if (this.combinationStrategyBuilder_0 != null)
                    {
                        this.tradingSystemExecutor_1.PosSize.StartingCapital = this.combinationStrategyBuilder_0.StartingEquity;
                        this.tradingSystemExecutor_1.PosSize.MarginFactor = this.combinationStrategyBuilder_0.MarginFactor;
                    }
                    this.method_19().ConfigureBarsLoader(this.barsLoader_0);
                    this.chartRenderer_0.UpBarColor = MainModule.Instance.Renderer.UpBarColor;
                    this.chartRenderer_0.DownBarColor = MainModule.Instance.Renderer.DownBarColor;
                    if (this.editor_0 != null)
                    {
                        this.editor_0.ClearErrors();
                    }
                    if ((this.thread_0 != null) && this.thread_0.IsAlive)
                    {
                        this.thread_0.Abort();
                    }
                    this.thread_0 = new Thread(new ThreadStart(this.method_21));
                    this.thread_0.IsBackground = true;
                    this.thread_0.Start();
                }
            }
        }

        private bool method_17(WealthLab.BarDataScale barDataScale_0)
        {
            if (!this.IsStreaming)
            {
                if (barDataScale_0.Scale != BarScale.Minute)
                {
                    return false;
                }
                if ((!this.IsStreaming && MainModule.Instance.DataSources.OnDemandUpdatesEnabled) && ((this.DataSource != null) && this.DataSource.Provider.WarnUserAboutDelay(this.Symbol, barDataScale_0.Scale, barDataScale_0.BarInterval)))
                {
                    if ((this.barsLoader_0.StartDate == DateTime.MinValue) && (this.barsLoader_0.EndDate == DateTime.MaxValue))
                    {
                        return this.method_18();
                    }
                }
                else if ((((MainModule.Instance.DataSources.OnDemandUpdatesEnabled && (this.DataSource.Provider != null)) && ((this.DataSource.Provider.DataStore != null) && (this.Symbol != ""))) && (!this.DataSource.Provider.DataStore.ContainsSymbol(this.Symbol, this.BarDataScale.Scale, this.BarDataScale.BarInterval) && (((this.barsLoader_0.EndDate != DateTime.MaxValue) || (this.barsLoader_0.StartDate != DateTime.MinValue)) || (this.barsLoader_0.MaxBars > 0)))) && (this.DataSource.Provider.GetChunkCount(this.DataSource, this.Symbol, this.barsLoader_0.StartDate, this.barsLoader_0.EndDate) > 1))
                {
                    return this.method_18();
                }
            }
            return false;
        }

        private bool method_18()
        {
            if (!MainModule.Instance.Settings.Get("DataDownloadWarning", false))
            {
                DontShowAgainForm form = new DontShowAgainForm("About to Download Data", "It may take a few minutes to download all of the data for this Symbol.  You can avoid delays by creating and updating a DataSet that contains the Symbols you want to maintain.  Press Cancel below to cancel the request.") {
                    CancelButtonVisible = true
                };
                bool flag = form.ShowDialog() == DialogResult.OK;
                if (form.DontShowAgain)
                {
                    MainModule.Instance.Settings.Set("DataDownloadWarning", true);
                }
                if (!flag)
                {
                    if (this.Bars != null)
                    {
                        this.BarDataScale = new WealthLab.BarDataScale(this.Bars.Scale, this.Bars.BarInterval);
                        this.Symbol = this.Bars.Symbol;
                    }
                    this.bool_6 = true;
                    this.MyMainForm.ActivateMdiChild();
                    return true;
                }
            }
            return false;
        }

        private BarDataRange method_19()
        {
            if (this.IsStreaming)
            {
                BarDataRange range = BarDataRange.Parse(this.barRange.DataRange.ToString());
                range.EndDate = DateTime.Now.Date.AddDays(1.0);
                return range;
            }
            return this.barRange.DataRange;
        }

        private void method_2()
        {
            this.statusOptimize.Visible = ((!this.IsStreaming && (this.optimization_0 == null)) && (this.Strategy != null)) && (this.Strategy.StrategyType != StrategyType.CombinedStrategy);
        }

        private void method_20()
        {
            this.tabChart.SelectedTab.Cursor = Cursors.WaitCursor;
        }

        private void method_21()
        {
            Exception exception = null;
            try
            {
                base.Invoke(new Delegate46(this.method_20));
                this.list_0.Clear();
                if (this.bool_3)
                {
                    if (this.bars_1 == null)
                    {
                        this.bars_1 = this.streamingChartManager_0.StartStreaming(this.DataSource, this.Symbol, this.BarDataScale, this.method_19());
                    }
                    this.method_28(this.bars_1);
                    this.list_0.Add(this.bars_1);
                    this.method_23();
                }
                else if (this.bool_10)
                {
                    foreach (string str in this.dataSource_0.Symbols)
                    {
                        WealthLab.Bars data = this.barsLoader_0.GetData(this.dataSource_0, str);
                        if ((this.Bars != null) && (this.Bars.Symbol == data.Symbol))
                        {
                            this.method_28(data);
                        }
                        this.list_0.Add(data);
                        this.method_23();
                        if (this.bool_11)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    WealthLab.Bars bars = this.barsLoader_0.GetData(this.dataSource_0, this.Symbol);
                    this.method_28(bars);
                    this.list_0.Add(bars);
                    this.method_23();
                }
                if (((this.WealthScript != null) || ((this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy))) && !this.bool_11)
                {
                    if (!this.bool_9)
                    {
                        this.method_13(this.tradingSystemExecutor_1, this.bars_0, this.bool_10);
                    }
                    else
                    {
                        this.bool_9 = false;
                    }
                }
                this.method_23();
                this.barsLoader_0.OverrideOnDemand = false;
            }
            catch (Exception exception2)
            {
                exception = exception2;
            }
            try
            {
                base.Invoke(new Delegate47(this.method_24), new object[] { exception });
            }
            catch (InvalidOperationException)
            {
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("The system is running low on memory.  Please close the currently open Chart or Strategy window.");
            }
        }

        private void method_22()
        {
            int num;
            bool flag;
            PositionType positionType;
            bool flag1;
            bool flag2;
            this.btnRunAllCancel.Enabled = false;
            if (!this.bool_10)
            {
                this.lblRunAllStatus.Text = "";
            }
            else
            {
                this.lblRunAllStatus.Text = string.Concat("Tested on ", this.list_0.Count, " Symbols");
            }
            this.progRunAll.Value = 0;
            this.progRunAll.Enabled = false;
            if (this.combinationStrategyBuilder_0 != null)
            {
                this.combinationStrategyBuilder_0.ResetProgressBar();
            }
            if (this.chart.Bars != null)
            {
                this.indicatorDragDropManager_0.CreateDragDropIndicators();
                this.drawingObjectManager_0.LoadDrawingObjects(this.chart.Bars);
            }
            if (this.chartRenderer_0.PricePane != null && this.chartRenderer_0.PricePane.LogScale != this.chartRenderer_0.LogScale)
            {
                this.chartRenderer_0.LogScale = this.chartRenderer_0.PricePane.LogScale;
                this.MyMainForm.SetLogScaleButtonState(this.chartRenderer_0.LogScale);
            }
            this.chart.MultiSymbolMode = this.bool_10;
            this.chart.DoInvalidate();
            if (this.editor_0 != null)
            {
                this.editor_0.UpdateExecutionTime();
            }
            this.method_39();
            if (this.tradingSystemExecutor_1.RiskStopLevelNotSet && this.Strategy != null)
            {
                DialogResult dialogResult = MessageBox.Show("Strategy code must set RiskStopLevel in order to use Maximum Risk position size.");
            }
            if (this.alerts_0 != null)
            {
                this.alerts_0.Clear();
            }
            if (this.WealthScript != null || this.combinationStrategyBuilder_0 != null)
            {
                SystemResults results = this.tradingSystemExecutor_1.Performance.Results;
                SystemResults resultsBuyHold = this.tradingSystemExecutor_1.Performance.ResultsBuyHold;
                double netProfit = results.NetProfit;
                this.stlblProfit.Text = string.Concat("Net Profit: ", netProfit.ToString("C"));
                double netProfit1 = resultsBuyHold.NetProfit;
                this.stlblBHProfit.Text = string.Concat("BH Net Profit: ", netProfit1.ToString("C"));
                if (!this.posSize.PositionSize.RawProfitMode)
                {
                    double aPR = results.APR;
                    this.stlblPerBar.Text = string.Concat("APR: ", aPR.ToString("N2"), "%");
                    this.stlblPerBar.ToolTipText = "Annual Percentage Rate of Return of the Strategy";
                    double aPR1 = resultsBuyHold.APR;
                    this.stlblBHPerBar.Text = string.Concat("BH APR: ", aPR1.ToString("N2"), "%");
                    this.stlblBHPerBar.ToolTipText = "Annual Percentage Rate of Return of Buy and Hold";
                    this.method_32(results.APR, resultsBuyHold.APR);
                }
                else
                {
                    double profitPerBar = results.ProfitPerBar;
                    this.stlblPerBar.Text = string.Concat("Profit per Bar: ", profitPerBar.ToString("C"));
                    this.stlblPerBar.ToolTipText = "Profit per Bar measures the Efficiency of the Strategy";
                    double profitPerBar1 = resultsBuyHold.ProfitPerBar;
                    this.stlblBHPerBar.Text = string.Concat("BH Profit per Bar: ", profitPerBar1.ToString("C"));
                    this.stlblBHPerBar.ToolTipText = "Profit per Bar of Buy and Hold";
                    this.method_32(results.ProfitPerBar, resultsBuyHold.ProfitPerBar);
                }
                this.method_8();
                this.list_2.Clear();
                foreach (TabPage list1 in this.list_1)
                {
                    UserControl item = list1.Controls[0] as UserControl;
                    IPerformanceVisualizer performanceVisualizer = item as IPerformanceVisualizer;
                    flag = (!this.bool_10 ? (int)(performanceVisualizer.AppliesTo & VisualizerAppliesTo.SingleSymbol) > 0 : (int)(performanceVisualizer.AppliesTo & VisualizerAppliesTo.MultiSymbol) > 0);
                    if (!this.posSize.PositionSize.RawProfitMode)
                    {
                        flag1 = (!flag ? false : (int)(performanceVisualizer.AppliesTo & VisualizerAppliesTo.PortfolioSim) > 0);
                        flag = flag1;
                    }
                    else
                    {
                        flag2 = (!flag ? false : (int)(performanceVisualizer.AppliesTo & VisualizerAppliesTo.RawProfit) > 0);
                        flag = flag2;
                    }
                    if ((int)(performanceVisualizer.AppliesTo & VisualizerAppliesTo.CombinationStrategy) > 0 && this.Strategy.StrategyType != StrategyType.CombinedStrategy)
                    {
                        flag = false;
                    }
                    if (this.Strategy.StrategyType == StrategyType.CombinedStrategy)
                    {
                        Type type = performanceVisualizer.GetType();
                        object[] customAttributes = type.GetCustomAttributes(true);
                        int num1 = 0;
                        while (num1 < (int)customAttributes.Length)
                        {
                            Attribute attribute = (Attribute)customAttributes[num1];
                            if (attribute is PVComboProhibitor)
                            {
                                flag = false;
                                goto Label0;
                            }
                            else
                            {
                                num1++;
                            }
                        }
                    }
                Label0:
                    if (!flag)
                    {
                        continue;
                    }
                    this.list_2.Add(list1);
                }
                num = (this.Strategy == null || this.Strategy.StrategyType != StrategyType.Compiled ? 4 : 3);
                bool flag3 = false;
                int num2 = 0;
                if (this.optimization_0 != null)
                {
                    num2 = 1;
                    IEnumerator enumerator = this.tabChart.TabPages.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            if (enumerator.MoveNext())
                            {
                                TabPage current = (TabPage)enumerator.Current;
                                if (current.Text == "Optimization" && current != this.tabChart.TabPages[this.tabChart.TabPages.Count - 1])
                                {
                                    flag3 = true;
                                    num++;
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
                bool tabCount = this.tabChart.TabCount != this.list_2.Count + num + num2;
                if (flag3)
                {
                    tabCount = true;
                }
                if (!tabCount)
                {
                    int num3 = 0;
                    while (num3 < this.list_2.Count)
                    {
                        if (this.list_2[num3] == this.tabChart.TabPages[num3 + num])
                        {
                            num3++;
                        }
                        else
                        {
                            tabCount = true;
                            goto Label1;
                        }
                    }
                }
            Label1:
                if (tabCount)
                {
                    List<TabPage> tabPages = new List<TabPage>();
                    for (int i = 0; i < num; i++)
                    {
                        if (this.tabChart.TabPages[i].Text != "Optimization")
                        {
                            tabPages.Add(this.tabChart.TabPages[i]);
                        }
                    }
                    TabPage selectedTab = this.tabChart.SelectedTab;
                    this.tabChart.TabPages.Clear();
                    for (int j = 0; j < tabPages.Count; j++)
                    {
                        this.tabChart.TabPages.Add(tabPages[j]);
                    }
                    foreach (TabPage list2 in this.list_2)
                    {
                        this.tabChart.TabPages.Add(list2);
                    }
                    if (this.optimization_0 != null)
                    {
                        this.tabChart.TabPages.Add("Optimization");
                        TabPage tabPage = this.tabChart.TabPages[this.tabChart.TabPages.Count - 1];
                        tabPage.Controls.Add(this.optimization_0);
                    }
                    if (selectedTab == null || !this.tabChart.TabPages.Contains(selectedTab))
                    {
                        if (selectedTab != null && selectedTab.Text == "Optimization")
                        {
                            this.SelectTab("Optimization");
                        }
                    }
                    else
                    {
                        this.tabChart.SelectedTab = selectedTab;
                    }
                }
                foreach (TabPage tabPage1 in this.tabChart.TabPages)
                {
                    if (tabPage1.Controls.Count == 0)
                    {
                        continue;
                    }
                    Control control = tabPage1.Controls[0];
                    if (control as IPerformanceVisualizer == null)
                    {
                        continue;
                    }
                    IPerformanceVisualizer item1 = control as IPerformanceVisualizer;
                    if (item1 == null)
                    {
                        continue;
                    }
                    try
                    {
                        item1.CreateVisualization(this.tradingSystemExecutor_1.Performance, this);
                        if (this.bool_13 && item1 is ISettingsProvider && this.dictionary_1.Count > 0 && this.dictionary_1[item1.TabText] != "")
                        {
                            (item1 as ISettingsProvider).SettingsString = this.dictionary_1[item1.TabText];
                        }
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        string[] str = new string[] { "Error in Visualizer: ", tabPage1.ToString(), ", removing.", Environment.NewLine, exception.Message };
                        DialogResult dialogResult1 = MessageBox.Show(string.Concat(str));
                        this.tabChart.TabPages.Remove(tabPage1);
                        this.tabChart.Refresh();
                    }
                }
                foreach (Alert alert in results.Alerts)
                {
                    if (alert.Account == null)
                    {
                        alert.Account = this.AccountNumber;
                    }
                    alert.DataSet = this.DataSource;
                    alert.DataScale = this.BarDataScale;
                    alert.DataRange = this.DataRange;
                    alert.PosSize = this.PositionSize;
                }
                this.alerts_0.Populate(results);
                if (results.Alerts.Count > 0)
                {
                    this.method_57();
                }
                if (this.alerts_0.AutoStage && this.IsStreaming)
                {
                    if (results.Alerts.Count <= 0)
                    {
                        if (MainModule.Instance.ShouldOrderBePlaced(this.AccountNumber))
                        {
                            MainModule.Instance.TradeManager.CancelStrategyOrders(this.AccountNumber, this.Strategy, this.Symbol, this.BarDataScale);
                        }
                    }
                    else
                    {
                        MainModule.Instance.TradeManager.AddAlerts(results.Alerts, MainModule.Instance.ShouldOrderBePlaced(results.Alerts[0]), true);
                    }
                }
                if (this.alerts_0.EmailAlerts && this.IsStreaming && results.Alerts.Count > 0)
                {
                    MainModule.Instance.method_26(results.Alerts);
                }
                this.SetupChartTradeMenuItems();
                if (this.Symbol != "")
                {
                    this.MyMainForm.paramSliders.Refresh();
                }
                this.MyMainForm.ExecuteCompleted();
                this.thread_0 = null;
            }
            bool flag4 = false;
            foreach (ChartDrawingObject drawingObject in this.drawingObjectManager_0.DrawingObjects)
            {
                if (drawingObject.Bars == null)
                {
                    continue;
                }
                bool flag5 = false;
                List<ChartDrawingObjectHandle>.Enumerator enumerator1 = drawingObject.Handles.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator1.MoveNext())
                        {
                            ChartDrawingObjectHandle chartDrawingObjectHandle = enumerator1.Current;
                            if (chartDrawingObjectHandle.Bar == -1)
                            {
                                flag5 = true;
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
                    ((IDisposable)enumerator1).Dispose();
                }
                if (flag5)
                {
                    continue;
                }
                TradeType tradeType = TradeType.Buy;
                string str1 = "";
                if (!drawingObject.TriggerAlert(this.Bars, ref tradeType, ref str1))
                {
                    continue;
                }
                this.tradingSystemExecutor_1.RiskStopLevelNotSet = false;
                this.tradingSystemExecutor_1.PosSize = this.posSize.PositionSize;
                if (tradeType != TradeType.Buy)
                {
                    if (tradeType == TradeType.Sell)
                    {
                        goto Label4;
                    }
                    positionType = PositionType.Short;
                    goto Label2;
                }
            Label4:
                positionType = PositionType.Long;
            Label2:
                double num4 = this.tradingSystemExecutor_1.CalcPositionSize(this.Bars, this.Bars.Count, this.Bars.Close[this.Bars.Count - 1], positionType, 0, this.posSize.PositionSize.StartingCapital);
                Alert positionSize = new Alert(this.Strategy, this.Bars, this.Bars.Date[this.Bars.Count - 1], tradeType, OrderType.Market, num4, str1);
                positionSize.PosSize = this.PositionSize;
                positionSize.ChartDrawingObject = drawingObject;
                if (this.posSize.PositionSize.Mode == PosSizeMode.MaxRisk)
                {
                    flag4 = true;
                }
                this.method_51();
                positionSize.Account = MainModule.Instance.DefaultAccountNumber;
                this.alerts_0.AddAlert(positionSize);
                this.method_57();
                if (this.alerts_0.AutoStage && this.IsStreaming)
                {
                    MainModule.Instance.TradeManager.AddAlert(positionSize, MainModule.Instance.ShouldOrderBePlaced(positionSize), true);
                }
                if (!this.alerts_0.EmailAlerts || !this.IsStreaming)
                {
                    continue;
                }
                MainModule.Instance.method_25(positionSize, MainModule.Instance.ShouldOrderBePlaced(positionSize));
            }
            if (flag4)
            {
                MessageBox.Show("Tradable Trendlines cannot use Maximum Risk position size.");
            }
            if (this.alerts_0 != null)
            {
                this.alerts_0.UpdateStatus();
            }
            this.method_12(true);
            if (this.bool_10 && this.tabChart.SelectedIndex == 0)
            {
                this.SelectTab("Performance");
            }
            if (this.bool_12 && this.bool_10 && DebugForm.Instance != null)
            {
                DebugForm.Instance.BringToFront();
            }
            this.method_36();
            int tradesNSF = this.tradingSystemExecutor_1.Performance.Results.TradesNSF;
            if (tradesNSF > 0 && !MainModule.Instance.Settings.Get("DontShowMissingTradeWarning", false))
            {
                string str2 = " trades were not included in the backtest results due to insufficient simulated capital.  Use Raw Profit mode to ensure all trades are always included.  You can find the number of trades not included at the bottom of the Trades list.";
                if (this.PositionSize.Mode == PosSizeMode.SimuScript)
                {
                    str2 = " trades were not included in the backtest results due to insufficient simulated capital, or dropped by the selected PosSizer.  Use Raw Profit mode to ensure all trades are always included.  You can find the number of trades not included at the bottom of the Trades list.";
                }
                DontShowAgainForm dontShowAgainForm = new DontShowAgainForm("Warning", string.Concat(tradesNSF, str2));
                dontShowAgainForm.ShowDialog();
                MainModule.Instance.Settings.Set("DontShowMissingTradeWarning", dontShowAgainForm.DontShowAgain);
            }
            if (this.ChildScrollBar > 0)
            {
                this.chart.ScrollToBar(this.ChildScrollBar);
                this.ChildScrollBar = -1;
            }
        }

        private void method_23()
        {
            this.method_26(this.progRunAll.Value + 1, null);
        }

        private void method_24(Exception exception_0)
        {
            if (exception_0 != null)
            {
                this.btnRunAllCancel.Enabled = false;
                if (this.editor_0 != null)
                {
                    this.SelectTab("Editor");
                    this.editor_0.DisplayRuntimeError(exception_0);
                }
                else if ((this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy))
                {
                    if (exception_0.Message != "CS Error")
                    {
                        MessageBox.Show(exception_0.Message, Application.ProductName);
                    }
                }
                else
                {
                    MessageBox.Show(exception_0.Message, Application.ProductName);
                }
                this.method_12(true);
            }
            else
            {
                this.method_22();
            }
            if (this.tabChart.SelectedTab != null)
            {
                this.tabChart.SelectedTab.Cursor = Cursors.Default;
                if ((this.tabChart.SelectedTab.Text == "Editor") && (this.editor_0 != null))
                {
                    this.editor_0.FocusEditor();
                    this.editor_0.Cursor = Cursors.Default;
                }
            }
            foreach (TabPage page in this.tabChart.TabPages)
            {
                if (page != null)
                {
                    page.Cursor = Cursors.Default;
                }
            }
            this.Cursor = Cursors.Default;
            if (this.bool_11)
            {
                this.lblRunAllStatus.Text = "User Canceled Backtest";
                this.bool_11 = false;
            }
        }

        private void method_25()
        {
            this.combinationStrategyBuilder_0.UpdateProgressBar();
        }

        private void method_26(int int_6, object object_0)
        {
            base.Invoke(new Delegate48(this.method_27), new object[] { int_6, object_0 });
        }

        private void method_27(int int_6, object object_0)
        {
            if (int_6 == -100)
            {
                Exception exception = (Exception) object_0;
                if (this.bool_10)
                {
                    if (DebugForm.Instance == null)
                    {
                        DebugForm.Instance = new DebugForm();
                        DebugForm.Instance.Show();
                    }
                    DebugForm.Instance.AddLine("Error processing symbol " + this.tradingSystemExecutor_1.BarsBeingProcessed.Symbol + "\t" + exception.Message);
                    this.bool_12 = true;
                    this.thread_0 = null;
                }
                else if (this.editor_0 != null)
                {
                    if (exception.Message.ToUpper().Contains("ABORTED") && this.IsStreaming)
                    {
                        this.editor_0.DisplayRuntimeError(exception);
                    }
                    else
                    {
                        this.SelectTab("Editor");
                        this.editor_0.DisplayRuntimeError(exception);
                    }
                    this.thread_0 = null;
                }
                else
                {
                    MessageBox.Show("Runtime Error: " + exception.Message);
                    this.thread_0 = null;
                }
                this.method_12(true);
            }
            else if (int_6 == 0)
            {
                WealthLab.Bars bars = (WealthLab.Bars) object_0;
                this.Bars = bars;
                this.bars_0 = bars;
                this.autoResetEvent_0.Set();
            }
            else
            {
                try
                {
                    this.progRunAll.Value = int_6;
                }
                catch (ArgumentOutOfRangeException)
                {
                }
                if ((this.progRunAll.Value < this.DataSource.Symbols.Count) && (this.list_0[this.list_0.Count - 1] != null))
                {
                    this.lblRunAllStatus.Text = "Collecting Data ... " + this.list_0[this.list_0.Count - 1].Symbol;
                }
                else if (this.progRunAll.Value == this.DataSource.Symbols.Count)
                {
                    this.lblRunAllStatus.Text = "Executing Strategy ...";
                }
                else
                {
                    this.lblRunAllStatus.Text = "Compiling Performance Results ...";
                }
            }
        }

        private void method_28(WealthLab.Bars bars_2)
        {
            this.bars_0 = null;
            this.method_26(0, bars_2);
            this.autoResetEvent_0.WaitOne();
        }

        private void method_29(object sender, EventArgs e)
        {
            this.MyMainForm.ClearDrawingObjectSelectedTool();
        }

        private void method_3(string string_2)
        {
            if ((string_2 != null) && (string_2.Length > 0))
            {
                this.Renderer.RestoreResizedPanes(string_2);
            }
        }

        private void method_30(object sender, BarsEventArgs e)
        {
            if (this.bool_10 && (e.Bars.Symbol == this.DataSource.Symbols[this.DataSource.Symbols.Count - 1]))
            {
                this.method_23();
            }
        }

        private void method_31(IPerformanceVisualizer iperformanceVisualizer_0)
        {
            Control control = (Control) Activator.CreateInstance(iperformanceVisualizer_0.GetType());
            if (control != null)
            {
                control.Dock = DockStyle.Fill;
                TabPage item = new TabPage(iperformanceVisualizer_0.TabText);
                item.Controls.Add(control);
                item.Tag = control;
                this.list_1.Add(item);
            }
        }

        private void method_32(double double_0, double double_1)
        {
            if (double_0 < 0.0)
            {
                this.stlblPerBar.ForeColor = Color.Red;
            }
            else if (double_0 > double_1)
            {
                this.stlblPerBar.ForeColor = Color.Green;
            }
            else
            {
                this.stlblPerBar.ForeColor = Color.Gray;
            }
            if (double_1 < 0.0)
            {
                this.stlblBHPerBar.ForeColor = Color.Red;
            }
            else if (double_1 >= double_0)
            {
                this.stlblBHPerBar.ForeColor = Color.Green;
            }
            else
            {
                this.stlblBHPerBar.ForeColor = Color.Gray;
            }
        }

        private void method_33(object sender, WSExceptionEventArgs e)
        {
            this.method_26(-100, e.Exception);
        }

        internal void method_34(object sender, LoadSymbolEventArgs e)
        {
            e.SymbolData = MainModule.Instance.LoadExternalSymbol(e.Symbol, e.Scale, e.BarInterval, this.btnStreaming.Checked);
        }

        private void method_35(object sender, EventArgs e)
        {
            base.Invoke(new Delegate49(this.method_36));
        }

        private void method_36()
        {
            if ((DebugForm.Instance == null) && (this.tradingSystemExecutor_1.DebugStrings.Count > 0))
            {
                DebugForm.Instance = new DebugForm();
                DebugForm.Instance.Show();
            }
            if (DebugForm.Instance != null)
            {
                DebugForm.Instance.Flush(this.tradingSystemExecutor_1.DebugStrings);
            }
        }

        private void method_37(object sender, DebugStringEventArgs e)
        {
            this.MyMainForm.PrintStatus(e.DebugMessage);
        }

        private void method_38(object sender, ChartBitmapEventArgs e)
        {
            this.chartRenderer_0.Executing = false;
            e.Bitmap = this.chart.GetChartBitmap(e.Width, e.Height);
            this.chartRenderer_0.Executing = true;
        }

        private void method_39()
        {
            if (this.description_0 != null)
            {
                string symbol = this.Symbol;
                if (this.bool_10)
                {
                    symbol = "";
                }
                this.description_0.SetDescriptiveFields(this.Bars, this.BarDataScale, this.DataRange, this.PositionSize, this.DataSource, symbol, this.WealthScript, this.tradingSystemExecutor_1.Performance.Results, this.tradingSystemExecutor_1.Performance.ResultsBuyHold, this.btnPV.Checked);
            }
        }

        private void method_4()
        {
            if ((this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy))
            {
                this.statusAddToSC.Visible = false;
            }
            else
            {
                this.statusAddToSC.Visible = true;
            }
        }

        private void method_40(object sender, EventArgs e)
        {
            base.Invoke(new Delegate50(this.method_41));
        }

        private void method_41()
        {
            this.bool_3 = true;
            if (this.tabChart.SelectedTab != null)
            {
                this.tabChart.SelectedTab.Cursor = Cursors.WaitCursor;
                if ((this.tabChart.SelectedTab.Text == "Editor") && (this.editor_0 != null))
                {
                    this.editor_0.Cursor = Cursors.WaitCursor;
                }
            }
            if (this.Bars != null)
            {
                this.Bars.Cache.Clear();
            }
            this.chartRenderer_0.ClearBarsObject();
            this.GoButtonPressed(this.Symbol, false);
            if (((this.tradingSystemExecutor_1.Performance.Results.Alerts.Count == 0) || !MainModule.Instance.Settings.Get("SoundsStrategyWindow", false)) && MainModule.Instance.Settings.Get("SoundsRealTime", true))
            {
                string defaultValue = MainModule.Instance.AppPath + @"\Data\Sounds\DIGITAL.wav";
                string path = MainModule.Instance.Settings.Get("SoundsRealTime_File", defaultValue);
                if (!File.Exists(path))
                {
                    path = defaultValue;
                    MainModule.Instance.Settings.Set("SoundsRealTime_File", path);
                }
                MainModule.Instance.PlaySound(path);
            }
        }

        private void method_42(object sender, DroppedIndicatorEventArgs e)
        {
            if (MainModule.Instance.Settings.Get("SoundsIndicators", true))
            {
                MainModule.Instance.PlaySound(Resources.kerchunk, false);
            }
            this.NeedSave = true;
            this.drawingObjectManager_0.LoadDrawingObjects(this.chart.Bars);
            this.mniPushCode.Visible = this.indicatorDragDropManager_0.HasDragDroppedIndicators;
        }

        private void method_43()
        {
            this.bool_8 = MainModule.Instance.DataSources.OnDemandUpdatesEnabled;
            try
            {
                this.DataSource.Provider.CheckConnectionWithServer();
                this.DataSource.Provider.DeleteSymbolDataFile(this.DataSource, this.Symbol);
                MainModule.Instance.DataSources.OnDemandUpdatesEnabled = true;
                this.DataSource.Provider.RequestData(this.DataSource, this.Symbol, DateTime.MinValue, DateTime.MaxValue, 0, false);
                base.Invoke(new Delegate43(this.method_44));
            }
            catch (Exception exception)
            {
                base.Invoke(new Delegate44(this.method_45), new object[] { exception });
            }
        }

        private void method_44()
        {
            MessageBox.Show("Chart data Reloaded");
            MainModule.Instance.DataSources.OnDemandUpdatesEnabled = this.bool_8;
            this.Cursor = Cursors.Default;
            this.chart.Mode = ChartMode.Normal;
            this.thread_0 = null;
            this.GoButtonPressed(this.Symbol, true);
        }

        private void method_45(Exception exception_0)
        {
            this.Cursor = Cursors.Default;
            this.chart.Mode = ChartMode.Normal;
            MessageBox.Show("Reload failed: " + exception_0.Message);
        }

        internal void method_46(object sender, TrendLineEventArgs e)
        {
            if (!this.bool_4)
            {
                this.bool_4 = true;
                this.drawingObjectManager_1.LoadDrawingObjects(this.tradingSystemExecutor_1.BarsBeingProcessed);
            }
            using (List<ChartDrawingObject>.Enumerator enumerator = this.drawingObjectManager_1.DrawingObjects.GetEnumerator())
            {
                ChartDrawingObject current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Name == e.TrendlineName)
                    {
                        ///goto  Label_005F;  ///WYJ fix, simplify the flow
                        if (current is CDOTrendline)
                        {
                            CDOTrendline trendline = current as CDOTrendline;
                            trendline.Renderer = this.chartRenderer_0;
                            e.Value = this.tradingSystemExecutor_1.WealthScriptExecuting.LineExtendY((double)trendline.LeftHandle.Bar, trendline.LeftHandle.Value, (double)trendline.RightHandle.Bar, trendline.RightHandle.Value, (double)e.Bar);
                        }
                        return;
                    }
                }
                return;
            }
        }

        private void method_47()
        {
            this.Strategy.PanelSize = this.chartRenderer_0.SavePaneSizes();
        }

        private void method_48()
        {
            string str;
            this.method_49(out str);
            this.Strategy.Indicators = str;
        }

        private void method_49(out string string_2)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                this.indicatorDragDropManager_0.SaveToStream(stream);
                stream.Position = 0L;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int) stream.Length);
                string_2 = Convert.ToBase64String(buffer);
            }
            finally
            {
                stream.Close();
            }
        }

        internal void method_5()
        {
            this.indicatorDragDropManager_0.Clear();
            this.mniPushCode.Visible = this.indicatorDragDropManager_0.HasDragDroppedIndicators;
            this.chart.DoInvalidate();
            if (DataWindowForm.Instance != null)
            {
                DataWindowForm.Instance.method_4();
            }
        }

        ///WYJ fix, original name method_50
        private void mouseWheelHandler(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                this.IncreaseBarSpacing();
            }
            else if (e.Delta < 0)
            {
                this.DecreaseBarSpacing();
            }
        }

        private void method_51()
        {
            if (this.alerts_0 == null)
            {
                this.tabChart.TabPages.Add("Alerts");
                this.alerts_0 = new Alerts(this);
                this.alerts_0.Page = this.tabChart.TabPages[this.tabChart.TabPages.Count - 1];
                this.alerts_0.IsStreaming = this.IsStreaming;
                this.alerts_0.Dock = DockStyle.Fill;
                this.tabPage_0 = this.tabChart.TabPages[this.tabChart.TabCount - 1];
                this.tabPage_0.Controls.Add(this.alerts_0);
            }
        }

        private void method_52(object sender, EventArgs e)
        {
            base.Invoke(new Delegate51(this.method_53));
        }

        private void method_53()
        {
            if (DebugForm.Instance != null)
            {
                DebugForm.Instance.Clear();
            }
        }

        private Bitmap method_54()
        {
            return this.chart.GetChartBitmap();
        }

        private void method_55(ref DataObject dataObject_0)
        {
            int num = 0;
            string str = "";
            if (this.MultiSymbolMode)
            {
                dataObject_0.SetData(PrintReport.fmtSymbol.Name, this.DataSource.Name);
            }
            else
            {
                dataObject_0.SetData(PrintReport.fmtSymbol.Name, this.Symbol);
                num = this.chart.Bars.Count - 1;
                str = this.chart.Bars.Date[num].ToShortDateString();
                if (this.chart.Bars.IsIntraday)
                {
                    str = str + " " + this.chart.Bars.Date[num].ToShortTimeString();
                }
            }
            if (this.Strategy != null)
            {
                dataObject_0.SetData(PrintReport.fmtStrategy.Name, this.Strategy.Name);
                SystemResults results = this.tradingSystemExecutor_1.Performance.Results;
                SystemResults resultsBuyHold = this.tradingSystemExecutor_1.Performance.ResultsBuyHold;
                string data = "Starting Capital: " + this.PositionSize.StartingCapital.ToString("C") + " | Position Sizing: " + this.PositionSize.Text + " | Data Range: " + this.DataRange.Text + " | Scale: " + this.BarDataScale.ToString();
                if (!this.MultiSymbolMode)
                {
                    data = (data + " | Bars: " + this.chart.Bars.Count) + " | Last Bar date: " + str;
                }
                string str4 = data;
                data = str4 + "\nNet Profit: " + results.NetProfit.ToString("C") + " | BH Net Profit: " + resultsBuyHold.NetProfit.ToString("C");
                if (this.posSize.PositionSize.RawProfitMode)
                {
                    string str5 = data;
                    data = str5 + " | Raw Profit Mode |  Profit per Bar: " + results.ProfitPerBar.ToString("C") + " | BH Profit per Bar: " + resultsBuyHold.ProfitPerBar.ToString("C");
                }
                else
                {
                    string str3 = data;
                    data = str3 + " | Portfolio Simulation |  APR: " + results.APR.ToString("N2") + "% | BH APR: " + resultsBuyHold.APR.ToString("N2") + "%";
                }
                dataObject_0.SetData(PrintReport.fmtDetails.Name, data);
            }
            else
            {
                dataObject_0.SetData(PrintReport.fmtDetails.Name, string.Concat(new object[] { "Data Range: ", this.DataRange.Text, " | Scale: ", this.BarDataScale.ToString(), " | Bars: ", this.chart.Bars.Count, " | Last Bar date: ", str }));
            }
        }

        private void method_56()
        {
            PageSettings pageSettings = new PageSettings();
            this.GetPageSettings(ref pageSettings);
            DataObject obj2 = new DataObject();
            obj2.SetData(PrintReport.fmtBaseTitle.Name, MainModule.Instance.AuthProvider.ApplicationName);
            if (this.Strategy != null)
            {
                obj2.SetData(PrintReport.fmtTitle.Name, "Strategy");
            }
            else
            {
                obj2.SetData(PrintReport.fmtTitle.Name, "Chart");
            }
            this.method_55(ref obj2);
            obj2.SetData(PrintReport.fmtGraphic.Name, this.method_54());
            PrintReport report = new PrintReport(obj2, true) {
                BasePrintTitle = MainModule.Instance.AuthProvider.ApplicationName
            };
            report.UseDefaultDisclosure();
            report.ShowPrintDialog = this.ShowPrintDialog();
            report.ShowPrintPreview = this.ShowPrintPreview();
            report.PrintGraphicReport(pageSettings);
        }

        private void method_57()
        {
            if (MainModule.Instance.Settings.Get("SoundsStrategyWindow", true))
            {
                string defaultValue = MainModule.Instance.AppPath + @"\Data\Sounds\alert4.wav";
                string path = MainModule.Instance.Settings.Get("SoundsStrategyWindow_File", defaultValue);
                if (!File.Exists(path))
                {
                    path = defaultValue;
                    MainModule.Instance.Settings.Set("SoundsStrategyWindow_File", path);
                }
                MainModule.Instance.PlaySoundPrioritized(path);
            }
        }

        private void method_58(TradeType tradeType_0)
        {
            if (this.Bars != null)
            {
                double tag = (double) this.mniChartBuy.Tag;
                Alert alert = new Alert(this.Strategy, this.Bars, DateTime.Now, tradeType_0, OrderType.Market, tag, "Chart") {
                    Account = MainModule.Instance.DefaultAccountNumber
                };
                MainModule.Instance.TradeManager.AddAlert(alert, MainModule.Instance.ShouldOrderBePlaced(alert), false);
            }
        }

        ///WYJ fix, code from Reflector, workable but deprecated because of having too many goto statements. Try version from ILSpy
        /*
        private void method_59(int int_6)
        {
            double high = this.Bars.High[int_6];
            double num2 = this.Bars.Low[int_6];
            double open = this.Bars.Open[int_6];
            double close = this.Bars.Close[int_6];
            double volume = this.Bars.Volume[int_6];
            DateTime time = this.Bars.Date[int_6];
            DateTime local1 = this.Bars.Date[int_6];
            DateTime local2 = this.Bars.Date[int_6];
            DataEditor editor = new DataEditor(this.Bars.Date[int_6], open, high, num2, close, volume, this.Bars.IsIntraday);
        Label_0297:
            editor.ShowDialog();
            if (editor.Action == DataEditor.Operation.Cancel)
            {
                return;
            }
            WealthLab.Bars bars = null;
            switch (editor.Action)
            {
                case DataEditor.Operation.Remove:
                    bars = new WealthLab.Bars(this.Bars.Symbol, this.Bars.Scale, this.Bars.BarInterval);
                    this.DataSource.Provider.DataStore.LoadBarsObject(bars);
                    if (!bars.IsIntraday)
                    {
                        this.method_61(editor.Date, bars);
                        break;
                    }
                    this.method_61(editor.DateAndTime, bars);
                    break;

                case DataEditor.Operation.Ok:
                {
                    bool flag = false;
                    bool flag2 = false;
                    if (!this.Bars.IsIntraday)
                    {
                        DateTime time2 = this.Bars.Date[int_6];
                        if (time2.Date != editor.Date.Date)
                        {
                            flag = true;
                            flag2 = true;
                        }
                    }
                    else if (this.Bars.Date[int_6] != editor.DateAndTime)
                    {
                        flag = true;
                        flag2 = true;
                    }
                    if (!flag && (((this.Bars.Open[int_6] != editor.OPEN) || (this.Bars.High[int_6] != editor.HIGH)) || (((this.Bars.Low[int_6] != editor.LOW) || (this.Bars.Close[int_6] != editor.CLOSE)) || (this.Bars.Volume[int_6] != editor.VOLUME))))
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        return;
                    }
                    bars = new WealthLab.Bars(this.Bars.Symbol, this.Bars.Scale, this.Bars.BarInterval);
                    this.DataSource.Provider.DataStore.LoadBarsObject(bars);
                    int num6 = this.method_65(time, bars);
                    if (!flag2)
                    {
                        bars.Open[num6] = editor.OPEN;
                        bars.High[num6] = editor.HIGH;
                        bars.Low[num6] = editor.LOW;
                        bars.Close[num6] = editor.CLOSE;
                        bars.Volume[num6] = editor.VOLUME;
                        if (bars.IsIntraday)
                        {
                            this.method_60(editor.DateAndTime, bars);
                        }
                        else
                        {
                            this.method_60(editor.Date, bars);
                        }
                        goto Label_04CC;
                    }
                    if (this.method_63(editor.DateAndTime, this.Bars.Scale, this.Bars.BarInterval))
                    {
                        if (!this.method_64(editor.DateAndTime, bars))
                        {
                            if (bars.IsIntraday)
                            {
                                int num7 = this.method_62(editor.DateAndTime, bars);
                                this.DataSource.Provider.DataStore.InsertBar(bars, num7, editor.DateAndTime, editor.OPEN, editor.HIGH, editor.LOW, editor.CLOSE, editor.VOLUME);
                                this.method_60(editor.DateAndTime, bars);
                            }
                            else
                            {
                                int num9 = this.method_62(editor.Date, bars);
                                this.DataSource.Provider.DataStore.InsertBar(bars, num9, editor.Date.Date, editor.OPEN, editor.HIGH, editor.LOW, editor.CLOSE, editor.VOLUME);
                                this.method_60(editor.Date, bars);
                            }
                            goto Label_04CC;
                        }
                        MessageBox.Show("Invalid Date/Time. Can't create a bar when one already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Date/Time. Date/Time must fit within the chart scale.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    goto Label_0297;
                }
                default:
                    goto Label_04CC;
            }
            int num8 = 0;
            if (bars.IsIntraday)
            {
                num8 = this.method_65(editor.DateAndTime, bars);
            }
            else
            {
                num8 = this.method_65(editor.Date, bars);
            }
            bars.Delete(num8);
        Label_04CC:
            this.DataSource.Provider.SaveEditedSymbolDataFile(this.DataSource, bars);
            this.barsLoader_0.OverrideOnDemand = true;
            this.GoButtonPressed(this.Symbol, true);
        } */

        ///WYJ fix, code from ILSpy
        // WealthLabPro.ChartForm
        private void method_59(int int_6)
        {
            double high = this.Bars.High[int_6];
            double double_ = this.Bars.Low[int_6];
            double open = this.Bars.Open[int_6];
            double close = this.Bars.Close[int_6];
            double volume = this.Bars.Volume[int_6];
            DateTime dateTime_ = this.Bars.Date[int_6];
            DateTime arg_7F_0 = this.Bars.Date[int_6];
            DateTime arg_91_0 = this.Bars.Date[int_6];
            DataEditor dataEditor = new DataEditor(this.Bars.Date[int_6], open, high, double_, close, volume, this.Bars.IsIntraday);
            Bars bars;
            int num;
            while (true)
            {
                dataEditor.ShowDialog();
                if (dataEditor.Action != DataEditor.Operation.Cancel)
                {
                    bars = null;
                    switch (dataEditor.Action)
                    {
                        case DataEditor.Operation.Remove:
                            bars = new Bars(this.Bars.Symbol, this.Bars.Scale, this.Bars.BarInterval);
                            this.DataSource.Provider.DataStore.LoadBarsObject(bars);
                            if (bars.IsIntraday)
                            {
                                this.method_61(dataEditor.DateAndTime, bars);
                            }
                            else
                            {
                                this.method_61(dataEditor.Date, bars);
                            }
                            int num2;
                            if (bars.IsIntraday)
                            {
                                num2 = this.method_65(dataEditor.DateAndTime, bars);
                            }
                            else
                            {
                                num2 = this.method_65(dataEditor.Date, bars);
                            }
                            bars.Delete(num2);
                            goto IL_4CC;
                                        
                        case DataEditor.Operation.Ok:
                            {
                                bool flag = false;
                                bool flag2 = false;
                                if (this.Bars.IsIntraday)
                                {
                                    if (this.Bars.Date[int_6] != dataEditor.DateAndTime)
                                    {
                                        flag = true;
                                        flag2 = true;
                                    }
                                }
                                else
                                {
                                    if (this.Bars.Date[int_6].Date != dataEditor.Date.Date)
                                    {
                                        flag = true;
                                        flag2 = true;
                                    }
                                }
                                if (!flag && (this.Bars.Open[int_6] != dataEditor.OPEN || this.Bars.High[int_6] != dataEditor.HIGH || this.Bars.Low[int_6] != dataEditor.LOW || this.Bars.Close[int_6] != dataEditor.CLOSE || this.Bars.Volume[int_6] != dataEditor.VOLUME))
                                {
                                    flag = true;
                                }
                                if (!flag)
                                {
                                    return;
                                }
                                bars = new Bars(this.Bars.Symbol, this.Bars.Scale, this.Bars.BarInterval);
                                this.DataSource.Provider.DataStore.LoadBarsObject(bars);
                                num = this.method_65(dateTime_, bars);
                                if (!flag2)
                                {
                                    ///goto  IL_43A;  ///WYJ fix, simplify the flow
                                    bars.Open[num] = dataEditor.OPEN;
                                    bars.High[num] = dataEditor.HIGH;
                                    bars.Low[num] = dataEditor.LOW;
                                    bars.Close[num] = dataEditor.CLOSE;
                                    bars.Volume[num] = dataEditor.VOLUME;
                                    if (bars.IsIntraday)
                                    {
                                        this.method_60(dataEditor.DateAndTime, bars);
                                    }
                                    else
                                    {
                                        this.method_60(dataEditor.Date, bars);
                                    }
                                    goto IL_4CC;
                                }
                                if (!this.method_63(dataEditor.DateAndTime, this.Bars.Scale, this.Bars.BarInterval))
                                {
                                    MessageBox.Show("Invalid Date/Time. Date/Time must fit within the chart scale.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                    continue;
                                }
                                if (this.method_64(dataEditor.DateAndTime, bars))
                                {
                                    MessageBox.Show("Invalid Date/Time. Can't create a bar when one already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                    continue;
                                }
                                if (bars.IsIntraday)
                                {
                                    int num3 = this.method_62(dataEditor.DateAndTime, bars);
                                    this.DataSource.Provider.DataStore.InsertBar(bars, num3, dataEditor.DateAndTime, dataEditor.OPEN, dataEditor.HIGH, dataEditor.LOW, dataEditor.CLOSE, dataEditor.VOLUME);
                                    this.method_60(dataEditor.DateAndTime, bars);
                                    goto IL_4CC;
                                }
                                int num4 = this.method_62(dataEditor.Date, bars);
                                this.DataSource.Provider.DataStore.InsertBar(bars, num4, dataEditor.Date.Date, dataEditor.OPEN, dataEditor.HIGH, dataEditor.LOW, dataEditor.CLOSE, dataEditor.VOLUME);
                                this.method_60(dataEditor.Date, bars);
                                goto IL_4CC;
                            }
                    }
                    break;
                }
                return;
            }
        
        IL_4CC:
            this.DataSource.Provider.SaveEditedSymbolDataFile(this.DataSource, bars);
            this.barsLoader_0.OverrideOnDemand = true;
            this.GoButtonPressed(this.Symbol, true);
        }


        internal void method_6()
        {
            if (this.indicatorDragDropManager_0.HasDragDroppedIndicators)
            {
                if (!MainModule.Instance.Settings.Get("DontShowPushIndicatorWarning", false))
                {
                    DontShowAgainForm dontShowAgainForm = new DontShowAgainForm("Confirm", "After pushing indicators and fundamental items to the strategy code, you will no longer be able to interact with them using the mouse. Do you want to continue?");
                    dontShowAgainForm.CancelButtonVisible = true;
                    dontShowAgainForm.MakeYesNo();
                    if (dontShowAgainForm.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        MainModule.Instance.Settings.Set("DontShowPushIndicatorWarning", dontShowAgainForm.DontShowAgain);
                    }
                }
                if (this.Strategy != null)
                {
                    if (this.editor_0 == null)
                    {
                        if (this.builder_0 != null)
                        {
                            Strategy strategy = new Strategy();
                            strategy.StrategyType = StrategyType.Script;
                            strategy.Code = this.builder_0.GenerateCode();
                            strategy.Code = this.indicatorDragDropManager_0.PushIndicatorsCode(strategy.Code);
                            ChartForm chartForm = this.MyMainForm.CreateNewStrategyWindow(true);
                            chartForm.Strategy = strategy;
                            chartForm.Show();
                            chartForm.BringToFront();
                            chartForm.GoButtonPressed(this.Symbol, true);
                            chartForm.SelectTab("Editor");
                            chartForm.NeedSave = true;
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Strategy strategyTemplateCode = new Strategy();
                    strategyTemplateCode.StrategyType = StrategyType.Script;
                    strategyTemplateCode.Code = MainModule.Instance.StrategyTemplateCode;
                    this.strategy_0 = strategyTemplateCode;
                    this.method_48();
                    this.Strategy = strategyTemplateCode;
                }
                string str = this.indicatorDragDropManager_0.PushIndicatorsCode(this.editor_0.Code);
                if (string.Compare(str, this.editor_0.Code) != 0)
                {
                    this.indicatorDragDropManager_0.Clear();
                    this.editor_0.Code = str;
                    this.CompileSource();
                    this.GoButtonPressed(this.Symbol, true);
                    this.SelectTab("Editor");
                    this.NeedSave = true;
                    this.ParametersNeedSave = true;
                }
                this.mniPushCode.Visible = this.indicatorDragDropManager_0.HasDragDroppedIndicators;
                return;
            }
            else
            {
                MessageBox.Show("There are no dropped indicators to push.");
                return;
            }
        }

        private void method_60(DateTime dateTime_0, WealthLab.Bars bars_2)
        {
            if (bars_2.IsIntraday)
            {
                if (!bars_2.UserEditedDates.Contains(dateTime_0))
                {
                    bars_2.UserEditedDates.Add(dateTime_0);
                }
            }
            else if (!bars_2.UserEditedDates.Contains(dateTime_0.Date))
            {
                bars_2.UserEditedDates.Add(dateTime_0.Date);
            }
        }

        private void method_61(DateTime dateTime_0, WealthLab.Bars bars_2)
        {
            if (bars_2.IsIntraday)
            {
                if (bars_2.UserEditedDates.Contains(dateTime_0))
                {
                    bars_2.UserEditedDates.Remove(dateTime_0);
                }
            }
            else if (bars_2.UserEditedDates.Contains(dateTime_0.Date))
            {
                bars_2.UserEditedDates.Remove(dateTime_0.Date);
            }
        }

        private int method_62(DateTime dateTime_0, WealthLab.Bars bars_2)
        {
            int count = bars_2.Count;
            for (int i = 0; i < bars_2.Count; i++)
            {
                if (bars_2.Date[i] > dateTime_0)
                {
                    return i;
                }
            }
            return count;
        }

        private bool method_63(DateTime dateTime_0, BarScale barScale_0, int int_6)
        {
            switch (barScale_0)
            {
                case BarScale.Daily:
                case BarScale.Weekly:
                case BarScale.Monthly:
                case BarScale.Quarterly:
                case BarScale.Yearly:
                    return true;

                case BarScale.Minute:
                    if ((dateTime_0.Minute % int_6) != 0)
                    {
                        break;
                    }
                    return true;
            }
            return false;
        }

        ///WYJ fix, code from Reflector, workable, but deprecated because of having too many goto statements. Try version from ILSpy
        /*
        private bool method_64(DateTime dateTime_0, WealthLab.Bars bars_2)
        {
            for (int i = 0; i < bars_2.Count; i++)
            {
                DateTime time;
                DateTime time8;
                DateTime time13;
                DateTime time15;
                DateTime time21;
                DateTime time31;
                DateTime time33;
                DateTime time35;
                DateTime time39;
                DateTime time42;
                if (bars_2.Date[i] != dateTime_0)
                {
                    switch (bars_2.Scale)
                    {
                        case BarScale.Weekly:
                        {
                            DateTime time37 = bars_2.Date[i];
                            if (time37.Year == dateTime_0.Year)
                            {
                                DateTime time38 = bars_2.Date[i];
                                if (time38.Month == dateTime_0.Month)
                                {
                                    DateTime time25 = bars_2.Date[i];
                                    switch (time25.DayOfWeek)
                                    {
                                        case DayOfWeek.Monday:
                                            goto Label_011A;

                                        case DayOfWeek.Tuesday:
                                            goto Label_01A8;

                                        case DayOfWeek.Wednesday:
                                            goto Label_0236;

                                        case DayOfWeek.Thursday:
                                            goto Label_02C4;

                                        case DayOfWeek.Friday:
                                            goto Label_0352;

                                        case DayOfWeek.Saturday:
                                            goto Label_03E0;
                                    }
                                }
                            }
                            continue;
                        }
                        case BarScale.Monthly:
                        {
                            DateTime time17 = bars_2.Date[i];
                            if (time17.Year != dateTime_0.Year)
                            {
                                continue;
                            }
                            DateTime time18 = bars_2.Date[i];
                            if (time18.Month != dateTime_0.Month)
                            {
                                continue;
                            }
                            return true;
                        }
                        case BarScale.Minute:
                        case BarScale.Second:
                        case BarScale.Tick:
                        {
                            continue;
                        }
                        case BarScale.Quarterly:
                            goto Label_0470;

                        case BarScale.Yearly:
                        {
                            DateTime time41 = bars_2.Date[i];
                            if (time41.Year == dateTime_0.Year)
                            {
                                return true;
                            }
                            continue;
                        }
                        default:
                        {
                            continue;
                        }
                    }
                    DateTime time23 = bars_2.Date[i];
                    if (time23.Day > (dateTime_0.Day + 6))
                    {
                        continue;
                    }
                    DateTime time24 = bars_2.Date[i];
                    if (dateTime_0.Day < time24.Day)
                    {
                        continue;
                    }
                }
                return true;
            Label_011A:
                time8 = bars_2.Date[i];
                if (time8.Day <= (dateTime_0.Day + 5))
                {
                    DateTime time9 = bars_2.Date[i];
                    if (dateTime_0.Day >= (time9.Day - 1))
                    {
                        goto Label_0687;
                    }
                }
                DateTime time26 = bars_2.Date[i];
                if (time26.Day < (dateTime_0.Day - 1))
                {
                    continue;
                }
                DateTime time27 = bars_2.Date[i];
                if (dateTime_0.Day > (time27.Day + 5))
                {
                    continue;
                }
                goto Label_0687;
            Label_01A8:
                time13 = bars_2.Date[i];
                if (time13.Day <= (dateTime_0.Day + 4))
                {
                    DateTime time14 = bars_2.Date[i];
                    if (dateTime_0.Day >= (time14.Day - 2))
                    {
                        goto Label_0689;
                    }
                }
                DateTime time5 = bars_2.Date[i];
                if (time5.Day < (dateTime_0.Day - 2))
                {
                    continue;
                }
                DateTime time6 = bars_2.Date[i];
                if (dateTime_0.Day > (time6.Day + 4))
                {
                    continue;
                }
                goto Label_0689;
            Label_0236:
                time15 = bars_2.Date[i];
                if (time15.Day <= (dateTime_0.Day + 3))
                {
                    DateTime time16 = bars_2.Date[i];
                    if (dateTime_0.Day >= (time16.Day - 3))
                    {
                        goto Label_068B;
                    }
                }
                DateTime time11 = bars_2.Date[i];
                if (time11.Day < (dateTime_0.Day - 3))
                {
                    continue;
                }
                DateTime time12 = bars_2.Date[i];
                if (dateTime_0.Day > (time12.Day + 2))
                {
                    continue;
                }
                goto Label_068B;
            Label_02C4:
                time39 = bars_2.Date[i];
                if (time39.Day <= (dateTime_0.Day + 2))
                {
                    DateTime time40 = bars_2.Date[i];
                    if (dateTime_0.Day >= (time40.Day - 2))
                    {
                        goto Label_068D;
                    }
                }
                DateTime time19 = bars_2.Date[i];
                if (time19.Day < (dateTime_0.Day - 4))
                {
                    continue;
                }
                DateTime time20 = bars_2.Date[i];
                if (dateTime_0.Day > (time20.Day + 4))
                {
                    continue;
                }
                goto Label_068D;
            Label_0352:
                time33 = bars_2.Date[i];
                if (time33.Day <= (dateTime_0.Day + 1))
                {
                    DateTime time34 = bars_2.Date[i];
                    if (dateTime_0.Day >= (time34.Day - 1))
                    {
                        goto Label_068F;
                    }
                }
                DateTime time28 = bars_2.Date[i];
                if (time28.Day < (dateTime_0.Day - 5))
                {
                    continue;
                }
                DateTime time29 = bars_2.Date[i];
                if (dateTime_0.Day > (time29.Day + 5))
                {
                    continue;
                }
                goto Label_068F;
            Label_03E0:
                time31 = bars_2.Date[i];
                if (time31.Day < (dateTime_0.Day - 6))
                {
                    continue;
                }
                DateTime time32 = bars_2.Date[i];
                if (dateTime_0.Day > time32.Day)
                {
                    continue;
                }
                return true;
            Label_0470:
                time = bars_2.Date[i];
                if (time.Year != dateTime_0.Year)
                {
                    continue;
                }
                DateTime time2 = bars_2.Date[i];
                if (time2.Month != 1)
                {
                    DateTime time4 = bars_2.Date[i];
                    if (time4.Month != 2)
                    {
                        DateTime time30 = bars_2.Date[i];
                        if (time30.Month != 3)
                        {
                            goto Label_0500;
                        }
                    }
                }
                if (((dateTime_0.Month == 1) || (dateTime_0.Month == 2)) || (dateTime_0.Month == 3))
                {
                    return true;
                }
            Label_0500:
                time35 = bars_2.Date[i];
                if (time35.Month != 4)
                {
                    DateTime time36 = bars_2.Date[i];
                    if (time36.Month != 5)
                    {
                        DateTime time3 = bars_2.Date[i];
                        if (time3.Month != 6)
                        {
                            goto Label_0570;
                        }
                    }
                }
                if (((dateTime_0.Month == 4) || (dateTime_0.Month == 5)) || (dateTime_0.Month == 6))
                {
                    return true;
                }
            Label_0570:
                time42 = bars_2.Date[i];
                if (time42.Month != 7)
                {
                    DateTime time43 = bars_2.Date[i];
                    if (time43.Month != 8)
                    {
                        DateTime time10 = bars_2.Date[i];
                        if (time10.Month != 9)
                        {
                            goto Label_05E3;
                        }
                    }
                }
                if (((dateTime_0.Month == 7) || (dateTime_0.Month == 8)) || (dateTime_0.Month == 9))
                {
                    return true;
                }
            Label_05E3:
                time21 = bars_2.Date[i];
                if (time21.Month != 10)
                {
                    DateTime time22 = bars_2.Date[i];
                    if (time22.Month != 11)
                    {
                        DateTime time7 = bars_2.Date[i];
                        if (time7.Month != 12)
                        {
                            continue;
                        }
                    }
                }
                if (((dateTime_0.Month == 10) || (dateTime_0.Month == 11)) || (dateTime_0.Month == 12))
                {
                    return true;
                }
            }
            return false;
        Label_0687:
            return true;
        Label_0689:
            return true;
        Label_068B:
            return true;
        Label_068D:
            return true;
        Label_068F:
            return true;
        } */

        ///WYJ fix, code from ILSpy
        private bool method_64(DateTime dateTime_0, Bars bars_2)
        {
            for (int i = 0; i < bars_2.Count; i++)
            {
                if (bars_2.Date[i] == dateTime_0)
                {
                    return true;
                }
                switch (bars_2.Scale)
                {
                    case BarScale.Weekly:
                        if (bars_2.Date[i].Year == dateTime_0.Year && bars_2.Date[i].Month == dateTime_0.Month)
                        {
                            switch (bars_2.Date[i].DayOfWeek)
                            {
                                case DayOfWeek.Sunday:
                                    if (bars_2.Date[i].Day <= dateTime_0.Day + 6 && dateTime_0.Day >= bars_2.Date[i].Day)
                                    {
                                        return true;
                                    }
                                    break;
                                case DayOfWeek.Monday:
                                    if (bars_2.Date[i].Day > dateTime_0.Day + 5 || dateTime_0.Day < bars_2.Date[i].Day - 1)
                                    {
                                        if (bars_2.Date[i].Day < dateTime_0.Day - 1 || dateTime_0.Day > bars_2.Date[i].Day + 5)
                                        {
                                            break;
                                        }
                                    }
                                    return true;
                                case DayOfWeek.Tuesday:
                                    if (bars_2.Date[i].Day > dateTime_0.Day + 4 || dateTime_0.Day < bars_2.Date[i].Day - 2)
                                    {
                                        if (bars_2.Date[i].Day < dateTime_0.Day - 2 || dateTime_0.Day > bars_2.Date[i].Day + 4)
                                        {
                                            break;
                                        }
                                    }
                                    return true;
                                case DayOfWeek.Wednesday:
                                    if (bars_2.Date[i].Day > dateTime_0.Day + 3 || dateTime_0.Day < bars_2.Date[i].Day - 3)
                                    {
                                        if (bars_2.Date[i].Day < dateTime_0.Day - 3 || dateTime_0.Day > bars_2.Date[i].Day + 2)
                                        {
                                            break;
                                        }
                                    }
                                    return true;
                                case DayOfWeek.Thursday:
                                    if (bars_2.Date[i].Day > dateTime_0.Day + 2 || dateTime_0.Day < bars_2.Date[i].Day - 2)
                                    {
                                        if (bars_2.Date[i].Day < dateTime_0.Day - 4 || dateTime_0.Day > bars_2.Date[i].Day + 4)
                                        {
                                            break;
                                        }
                                    }
                                    return true;
                                case DayOfWeek.Friday:
                                    if (bars_2.Date[i].Day > dateTime_0.Day + 1 || dateTime_0.Day < bars_2.Date[i].Day - 1)
                                    {
                                        if (bars_2.Date[i].Day < dateTime_0.Day - 5 || dateTime_0.Day > bars_2.Date[i].Day + 5)
                                        {
                                            break;
                                        }
                                    }
                                    return true;
                                case DayOfWeek.Saturday:
                                    if (bars_2.Date[i].Day >= dateTime_0.Day - 6 && dateTime_0.Day <= bars_2.Date[i].Day)
                                    {
                                        return true;
                                    }
                                    break;
                            }
                        }
                        break;
                    case BarScale.Monthly:
                        if (bars_2.Date[i].Year == dateTime_0.Year && bars_2.Date[i].Month == dateTime_0.Month)
                        {
                            return true;
                        }
                        break;
                    case BarScale.Quarterly:
                        if (bars_2.Date[i].Year == dateTime_0.Year)
                        {
                            if (bars_2.Date[i].Month == 1 || bars_2.Date[i].Month == 2 || bars_2.Date[i].Month == 3)
                            {
                                if (dateTime_0.Month == 1 || dateTime_0.Month == 2 || dateTime_0.Month == 3)
                                {
                                    return true;
                                }
                            }
                            if (bars_2.Date[i].Month == 4 || bars_2.Date[i].Month == 5 || bars_2.Date[i].Month == 6)
                            {
                                if (dateTime_0.Month == 4 || dateTime_0.Month == 5 || dateTime_0.Month == 6)
                                {
                                    return true;
                                }
                            }
                            if (bars_2.Date[i].Month == 7 || bars_2.Date[i].Month == 8 || bars_2.Date[i].Month == 9)
                            {
                                if (dateTime_0.Month == 7 || dateTime_0.Month == 8 || dateTime_0.Month == 9)
                                {
                                    return true;
                                }
                            }
                            if (bars_2.Date[i].Month == 10 || bars_2.Date[i].Month == 11 || bars_2.Date[i].Month == 12)
                            {
                                if (dateTime_0.Month == 10 || dateTime_0.Month == 11 || dateTime_0.Month == 12)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case BarScale.Yearly:
                        if (bars_2.Date[i].Year == dateTime_0.Year)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }


        private int method_65(DateTime dateTime_0, WealthLab.Bars bars_2)
        {
            for (int i = 0; i < bars_2.Count; i++)
            {
                if (bars_2.Date[i] == dateTime_0)
                {
                    return i;
                }
            }
            return -1;
        }

        private bool method_66()
        {
            return (((this.DataSource == null) && string.IsNullOrEmpty(this.Symbol)) || (this.Bars == null));
        }

        internal void method_67(object sender, LoadSymbolFromDataSetEventArgs e)
        {
            e.Bars = MainModule.Instance.LoadExternalSymbol(e.DataSetName, e.Symbol);
        }

        internal void method_68()
        {
            if (this.editor_0 != null)
            {
                this.editor_0.method_1(this.WealthScript);
            }
        }

        private void method_69(object sender, StrategyParameterEventArgs e)
        {
            if ((!this.IsOptimizing && !this.SliderValueChanging) && this.btnPV.Checked)
            {
                this.Strategy.LoadPreferredValues(e.Symbol, e.WealthScript);
            }
            this.SliderValueChanging = false;
        }

        internal void method_7(ChartForm chartForm_0)
        {
            while (chartForm_0.IsBusy)
            {
                Application.DoEvents();
            }
        }

        private void method_70(object sender, EventArgs e)
        {
            if (this.thread_0 != null)
            {
                this.thread_0.Abort(AbortReason.Bars);
            }
            if (this.thread_0 != null)
            {
                this.thread_0.Join();
            }
            this.thread_0 = null;
        }

        internal void method_71(DraggedFundamentalItem draggedFundamentalItem_0)
        {
            if ((this.Bars != null) && (this.chartRenderer_0.PricePane != null))
            {
                this.indicatorDragDropManager_0.ProcessDroppedFundamentalItem(draggedFundamentalItem_0);
                this.mniPushCode.Visible = this.indicatorDragDropManager_0.HasDragDroppedIndicators;
                this.NeedSave = true;
            }
        }

        private void method_72(object sender, ExceptionEventArgs e)
        {
            if (ilog_0.IsErrorEnabled)
            {
                ilog_0.Error(e.Exception.Message);
            }
        }

        private void method_73(object sender, StrategyEventArgs e)
        {
            e.Strategy = MainModule.Instance.Strategies.LookupID(e.StrategyID);
            WealthLab.WealthScript wealthScriptObject = MainModule.Instance.Strategies.GetWealthScriptObject(e.Strategy);
            e.Strategy.Tag = wealthScriptObject;
        }

        private void method_74(object sender, DataSourceLookupEventArgs e)
        {
            e.DataSource = MainModule.Instance.DataSources.FindDataSource(e.DataSourceName);
        }

        private void method_75(object sender, BarsEventArgs e)
        {
            base.Invoke(new Delegate43(this.method_25));
        }

        private void method_8()
        {
            if (!this.bool_2)
            {
                this.bool_2 = true;
                this.method_51();
                if (!this.bool_13)
                {
                    foreach (IPerformanceVisualizer visualizer in MainModule.Instance.VisualizersChecked)
                    {
                        this.method_31(visualizer);
                    }
                }
            }
        }

        private void method_9(object sender, BarNumberEventArgs e)
        {
            int barNumber = e.BarNumber;
            if (barNumber != this.int_2)
            {
                this.int_2 = barNumber;
                if (barNumber >= 0)
                {
                    string format = "N" + this.Bars.SymbolInfo.Decimals;
                    this.stlblBar.Text = "Bar: " + barNumber.ToString("N0");
                    string str2 = this.method_10(barNumber);
                    this.stlblDate.Text = str2;
                    this.stlblOpen.Text = "O: " + this.Bars.Open[barNumber].ToString(format);
                    this.stlblHigh.Text = "H: " + this.Bars.High[barNumber].ToString(format);
                    this.stlblLow.Text = "L: " + this.Bars.Low[barNumber].ToString(format);
                    this.stlblClose.Text = "C: " + this.Bars.Close[barNumber].ToString(format);
                    this.stlblVolume.Text = "V: " + this.Bars.Volume[barNumber].ToString("N0");
                    this.method_11(barNumber, true, false);
                }
                else
                {
                    this.stlblBar.Text = "Bar:";
                    this.stlblDate.Text = "";
                    this.stlblOpen.Text = "";
                    this.stlblHigh.Text = "";
                    this.stlblLow.Text = "";
                    this.stlblClose.Text = "";
                    this.stlblVolume.Text = "";
                }
            }
            if (e.Pane != null)
            {
                this.stlblMouse.Text = "Mouse: " + e.Pane.FormatChartValue(e.Value);
            }
            else
            {
                this.stlblMouse.Text = "Mouse: ";
            }
            bool flag = this.drawingObjectManager_0.SelectedDrawingObject != null;
            this.mniDeleteDrawingObject.Visible = flag;
            this.mniDrawingObjectProperties.Visible = flag && (this.drawingObjectManager_0.SelectedDrawingObject is ICustomSettings);
            this.sepDrawing.Visible = this.mniDeleteDrawingObject.Visible;
            if (!flag || (this.drawingObjectManager_0.SelectedDrawingObject != this.chartDrawingObject_0))
            {
                for (int i = this.popupChart.Items.Count - 1; i >= 0; i--)
                {
                    ToolStripItem item = this.popupChart.Items[i];
                    if (item.Tag is string)
                    {
                        this.popupChart.Items.RemoveAt(i);
                    }
                }
                this.chartDrawingObject_0 = null;
            }
            if (flag)
            {
                ChartDrawingObject selectedDrawingObject = this.drawingObjectManager_0.SelectedDrawingObject;
                if ((selectedDrawingObject != null) && (selectedDrawingObject != this.chartDrawingObject_0))
                {
                    this.chartDrawingObject_0 = selectedDrawingObject;
                    selectedDrawingObject.RegisterExtendedBehaviors(this);
                }
            }
            this.mniDeleteIndicator.Visible = (this.indicatorDragDropManager_0.SelectedIndicator != null) && this.indicatorDragDropManager_0.SelectedIndicator.DragAndDrop;
            this.mniIndicatorProperties.Visible = this.mniDeleteIndicator.Visible;
            this.sepIndicators.Visible = this.mniDeleteIndicator.Visible;
        }

        private void mniAddDifferent_Click(object sender, EventArgs e)
        {
            StrategyExplorerForm form = new StrategyExplorerForm {
                Text = "Select Strategy to Open"
            };
            this.dialogResult_0 = form.ShowDialog();
            this.chart.Refresh();
            Application.DoEvents();
            switch (this.dialogResult_0)
            {
                case DialogResult.Yes:
                {
                    WealthLab.Strategy strategy = new WealthLab.Strategy {
                        StrategyType = StrategyType.Script,
                        Code = MainModule.Instance.StrategyTemplateCode
                    };
                    this.Strategy = strategy;
                    this.chart.Refresh();
                    Application.DoEvents();
                    this.GoButtonPressed(this.Symbol, true);
                    this.MyMainForm.BuildParameterSliders();
                    this.MyMainForm.method_23("S");
                    this.SelectTab("Editor");
                    this.MyMainForm.SetDataPanelState(true, true, true, true, strategy.StrategyType == StrategyType.CombinedStrategy);
                    return;
                }
                case DialogResult.No:
                {
                    WealthLab.Strategy strategy2 = new WealthLab.Strategy {
                        StrategyType = StrategyType.Rules
                    };
                    this.Strategy = strategy2;
                    this.chart.Refresh();
                    Application.DoEvents();
                    this.GoButtonPressed(this.Symbol, true);
                    this.MyMainForm.BuildParameterSliders();
                    this.MyMainForm.method_23("S");
                    this.SelectTab("Rules");
                    this.MyMainForm.SetDataPanelState(true, true, true, true, strategy2.StrategyType == StrategyType.CombinedStrategy);
                    return;
                }
                case DialogResult.OK:
                    if (form.StrategiesSelected.Count != 0)
                    {
                        this.Strategy = form.StrategiesSelected[0];
                        this.chart.Refresh();
                        Application.DoEvents();
                        this.GoButtonPressed(this.Symbol, true);
                        this.MyMainForm.BuildParameterSliders();
                        this.MyMainForm.method_23("S");
                        this.MyMainForm.SetDataPanelState(true, true, true, true, this.Strategy.StrategyType == StrategyType.CombinedStrategy);
                        MainModule.Instance.Strategies.LoadStrategyParameters(this.Strategy, this.WealthScript);
                        MainModule.Instance.AddStrategyToMRU(this.Strategy);
                    }
                    return;
            }
        }

        private void mniAddSymbolToDataSet_Click(object sender, EventArgs e)
        {
            if (((this.DataSource != null) && (this.Symbol != null)) && ((this.Symbol != "") && !this.DataSource.Symbols.Contains(this.Symbol)))
            {
                MainModule.Instance.DataSources.AddSymbol(this.DataSource, this.Symbol);
            }
        }

        private void mniChartBuy_Click(object sender, EventArgs e)
        {
            this.method_58(TradeType.Buy);
        }

        private void mniChartCover_Click(object sender, EventArgs e)
        {
            this.method_58(TradeType.Cover);
        }

        private void mniChartOptions_Click(object sender, EventArgs e)
        {
            new PreferencesForm().ShowDialog("Chart Colors and Styles");
        }

        private void mniChartSell_Click(object sender, EventArgs e)
        {
            this.method_58(TradeType.Sell);
        }

        private void mniChartShort_Click(object sender, EventArgs e)
        {
            this.method_58(TradeType.Short);
        }

        private void mniChartStyleSettings_Click(object sender, EventArgs e)
        {
            if (this.ChartStyle is ICustomSettings)
            {
                ICustomSettings chartStyle = this.ChartStyle as ICustomSettings;
                ChartSettingsForm form = new ChartSettingsForm {
                    Text = this.ChartStyle.FriendlyName + " Chart Style Settings"
                };
                UserControl settingsUI = chartStyle.GetSettingsUI();
                form.AddSettingsUI(settingsUI);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    chartStyle.ChangeSettings(settingsUI);
                    chartStyle.WriteSettings(MainModule.Instance.Settings);
                    this.chartRenderer_0.BarSpacing = this.chartRenderer_0.BarSpacing;
                    this.ChartStyle.Initialize();
                    this.chart.DoInvalidate();
                }
            }
        }

        private void mniCopyChart_Click(object sender, EventArgs e)
        {
            this.chart.CopyToClipboard();
        }

        private void mniCopyPriceData_Click(object sender, EventArgs e)
        {
            if (this.Bars != null)
            {
                StringBuilder builder = new StringBuilder();
                string str = "Date\t";
                if (this.Bars.IsIntraday)
                {
                    str = str + "Time\t";
                }
                str = str + "Open\tHigh\tLow\tClose\tVolume\t";
                foreach (ChartPane pane2 in this.chartRenderer_0.Panes)
                {
                    foreach (PlottedIndicator indicator2 in pane2.PlottedIndicators)
                    {
                        if (indicator2.Series.Description != "Volume")
                        {
                            str = str + indicator2.Series.Description + "\t";
                        }
                    }
                }
                builder.AppendLine(str);
                for (int i = 0; i < this.Bars.Count; i++)
                {
                    string str2 = this.Bars.Date[i].ToShortDateString() + "\t";
                    if (this.Bars.IsIntraday)
                    {
                        str2 = str2 + this.Bars.Date[i].ToShortTimeString() + "\t";
                    }
                    str2 = ((((str2 + this.Bars.Open[i] + "\t") + this.Bars.High[i] + "\t") + this.Bars.Low[i] + "\t") + this.Bars.Close[i] + "\t") + this.Bars.Volume[i] + "\t";
                    foreach (ChartPane pane in this.chartRenderer_0.Panes)
                    {
                        foreach (PlottedIndicator indicator in pane.PlottedIndicators)
                        {
                            if (indicator.Series.Description != "Volume")
                            {
                                str2 = str2 + indicator.Series[i] + "\t";
                            }
                        }
                    }
                    builder.AppendLine(str2);
                }
                try
                {
                    Clipboard.SetDataObject(builder.ToString(), true, 2, 0x3e8);
                }
                catch (ExternalException)
                {
                    MessageBox.Show("Copy to clipboard was blocked by another process.  Please try again", "ClipBoard Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void mniDeleteDrawingObject_Click(object sender, EventArgs e)
        {
            this.drawingObjectManager_0.RemoveDrawingObject(this.drawingObjectManager_0.SelectedDrawingObject);
            this.drawingObjectManager_0.SaveDrawingObjects(this.Bars);
            this.chart.DoInvalidate();
            bool positions = this.drawingObjectManager_0.HasObjectsThatCanTriggerAlerts || (this.Strategy != null);
            this.MyMainForm.SetDataPanelState(true, true, positions, this.Strategy != null, (this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy));
        }

        private void mniDeleteIndicator_Click(object sender, EventArgs e)
        {
            PlottedIndicator selectedIndicator = this.indicatorDragDropManager_0.SelectedIndicator;
            if (selectedIndicator != null)
            {
                this.indicatorDragDropManager_0.RemoveIndicator(selectedIndicator);
                this.chart.DoInvalidate();
                if (MainModule.Instance.Settings.Get("SoundsIndicators", true))
                {
                    MainModule.Instance.PlaySound(Resources.Metronome2, false);
                }
                this.NeedSave = true;
                if (DataWindowForm.Instance != null)
                {
                    this.method_11(this.int_2, false, true);
                }
            }
            this.mniPushCode.Visible = this.indicatorDragDropManager_0.HasDragDroppedIndicators;
        }

        private void mniDrawingObjectProperties_Click(object sender, EventArgs e)
        {
            if (this.drawingObjectManager_0.ChangeDrawingObjectSettings(this.drawingObjectManager_0.SelectedDrawingObject))
            {
                this.drawingObjectManager_0.SaveDrawingObjects(this.Bars);
                this.chart.DoInvalidate();
            }
        }

        private void mniIndicatorProperties_Click(object sender, EventArgs e)
        {
            PlottedIndicator selectedIndicator = this.indicatorDragDropManager_0.SelectedIndicator;
            if ((selectedIndicator != null) && this.indicatorDragDropManager_0.EditIndicator(selectedIndicator))
            {
                this.chart.DoInvalidate();
                this.NeedSave = true;
            }
        }

        private void mniPlotIndicator_Click(object sender, EventArgs e)
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

        private void mniPrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void mniPrintAll_Click(object sender, EventArgs e)
        {
            this.PrintAll();
        }

        private void mniPushCode_Click(object sender, EventArgs e)
        {
            this.method_6();
        }

        private void mniReload_Click(object sender, EventArgs e)
        {
            if (((this.DataSource != null) && (this.DataSource.Provider != null)) && (this.Symbol != ""))
            {
                this.Cursor = Cursors.WaitCursor;
                this.chart.Mode = ChartMode.Wait;
                if ((this.thread_0 != null) && this.thread_0.IsAlive)
                {
                    this.thread_0.Abort();
                }
                this.thread_0 = new Thread(new ThreadStart(this.method_43));
                this.thread_0.IsBackground = true;
                this.thread_0.Start();
            }
        }

        private void mniStockSplit_Click(object sender, EventArgs e)
        {
            if (((this.DataSource != null) && (this.DataSource.Provider != null)) && (this.Symbol != ""))
            {
                StockSplitForm form = new StockSplitForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    MainModule.Instance.DataSources.ProcessStockSplit(this.DataSource, this.Symbol, form.SplitDate, form.Factor);
                    this.GoButtonPressed(this.Symbol, true);
                    MessageBox.Show("Stock Split processed");
                }
            }
        }

        private void plotAFundamentalDataItemOnTheChartToolStripMenuItem_Click(object sender, EventArgs e)
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

        public void PlotIndicator(IndicatorHelper helper)
        {
            if ((this.Bars != null) && (this.chartRenderer_0.PricePane != null))
            {
                this.indicatorDragDropManager_0.ProcessDroppedIndicatorHelper(helper, this.chartRenderer_0.PricePane);
                this.mniPushCode.Visible = this.indicatorDragDropManager_0.HasDragDroppedIndicators;
                this.NeedSave = true;
            }
        }

        private void popupChart_Opening(object sender, CancelEventArgs e)
        {
            if (this.method_66())
            {
                e.Cancel = true;
            }
            else
            {
                this.editBarToolStripMenuItem.Enabled = this.AllowEditBarData;
                this.int_4 = this.int_2;
            }
        }

        public void PositionSizeChanged(string symbol)
        {
            if (this.DataSource != null)
            {
                if (this.bool_10)
                {
                    if (this.list_0 != null)
                    {
                        this.method_12(false);
                        this.Cursor = Cursors.WaitCursor;
                        this.chart.Mode = ChartMode.Wait;
                        try
                        {
                            this.tradingSystemExecutor_1.ApplySettings(MainModule.Instance.Executor);
                            this.tradingSystemExecutor_1.ApplyPositionSize();
                            this.method_22();
                        }
                        finally
                        {
                            this.method_12(true);
                            this.Cursor = Cursors.Default;
                            this.chart.Mode = ChartMode.Normal;
                        }
                    }
                }
                else
                {
                    this.ResetStreaming();
                    this.GoButtonPressed(symbol, false);
                }
            }
        }

        public void PreferredValuesChanged()
        {
            this.SaveStrategy();
            if ((this.btnPV.Checked && !this.bool_10) && ((this.Symbol != null) && (this.Symbol != "")))
            {
                this.GoButtonPressed(this.Symbol, true);
            }
        }

        public void PressEscape()
        {
            if (this.IsBusy)
            {
                this.thread_0.Abort(AbortReason.ESC);
                this.thread_0 = null;
            }
            this.Cursor = Cursors.Default;
            this.chart.Mode = ChartMode.Normal;
        }

        public void Print()
        {
            if (this.tabChart.SelectedTab.Text == "Chart")
            {
                this.method_56();
            }
            else if (this.tabChart.SelectedTab.Text == "Strategy Summary")
            {
                if (this.description_0 != null)
                {
                    this.description_0.Print();
                }
            }
            else if (this.tabChart.SelectedTab.Text.Contains("Alert"))
            {
                if (this.alerts_0 != null)
                {
                    this.alerts_0.Print();
                }
            }
            else if ((this.Strategy != null) && (this.SelectedVisualizer != null))
            {
                IPerformanceVisualizer selectedVisualizer = this.SelectedVisualizer;
                if ((selectedVisualizer != null) && selectedVisualizer.SupportsPrint)
                {
                    selectedVisualizer.Print();
                }
            }
        }

        public void PrintAll()
        {
            foreach (TabPage page in this.tabChart.TabPages)
            {
                if (page.Text.Contains("Chart"))
                {
                    this.method_56();
                }
                else if (!page.Text.Contains("Editor") && !page.Text.Contains("Rule"))
                {
                    if (page.Text.Contains("Strategy Summary"))
                    {
                        if (this.description_0 != null)
                        {
                            this.description_0.Print();
                        }
                    }
                    else if (page.Text.Contains("Alert"))
                    {
                        if (this.alerts_0 != null)
                        {
                            this.alerts_0.Print();
                        }
                    }
                    else if (this.strategy_0 != null)
                    {
                        UserControl control = page.Controls[0] as UserControl;
                        if (control is IPerformanceVisualizer)
                        {
                            IPerformanceVisualizer visualizer = control as IPerformanceVisualizer;
                            if ((visualizer != null) && visualizer.SupportsPrint)
                            {
                                visualizer.Print();
                            }
                        }
                    }
                }
            }
        }

        public void RefreshChart()
        {
            this.chart.DoInvalidate();
        }

        public void RefreshOptimizerViews()
        {
            if (this.optimization_0 != null)
            {
                this.optimization_0.RefreshViews();
            }
        }

        public void RegisterBehavior(string behaviorName, Image image, EventHandler eventHandler_0)
        {
            if (!string.IsNullOrEmpty(behaviorName))
            {
                ToolStripItem item;
                int num2 = -1;
                string[] strArray = behaviorName.Split(new char[] { '|' });
                string str = null;
                string str2 = null;
                if (strArray.Length == 2)
                {
                    str = strArray[0].Trim();
                    str2 = strArray[1].Trim();
                    if (!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str))
                    {
                        for (int i = 0; i < this.popupChart.Items.Count; i++)
                        {
                            if (this.popupChart.Items[i].Text == str2)
                            {
                                behaviorName = str;
                                num2 = i;
                                break;
                            }
                        }
                    }
                }
                if (num2 < 0)
                {
                    item = this.popupChart.Items.Add(behaviorName, image, eventHandler_0);
                }
                else
                {
                    item = new ToolStripMenuItem(behaviorName, image, eventHandler_0);
                    this.popupChart.Items.Insert(num2 + 1, item);
                }
                item.Tag = behaviorName;
            }
        }

        public void ReloadOptimizationParameters()
        {
            if (this.optimization_0 != null)
            {
                this.optimization_0.LoadParameterList();
            }
        }

        public void RememberStrategySettings()
        {
            if (this.DataSource == null)
            {
                this.Strategy.DataSetName = "";
            }
            else
            {
                this.Strategy.DataSetName = this.DataSource.Name;
            }
            if (this.MultiSymbolMode)
            {
                this.Strategy.Symbol = "";
            }
            else
            {
                this.Strategy.Symbol = this.Symbol;
            }
            this.Strategy.DataScale = this.BarDataScale;
            this.Strategy.PositionSize = this.PositionSize;
            this.Strategy.DataRange = this.DataRange;
        }

        public void ResetStreaming()
        {
            this.bars_1 = null;
        }

        public void RestoreBarSpacing()
        {
            this.chartRenderer_0.BarSpacing = 6;
            this.chart.DoInvalidate();
            MainModule.Instance.Renderer.BarSpacing = this.chartRenderer_0.BarSpacing;
        }

        public void RunOnAllSymbols()
        {
            if (((((this.Strategy == null) || (this.Strategy.StrategyType != StrategyType.CombinedStrategy)) ? 0 : 1) != 0) || (this.WealthScript != null))
            {
                if (this.IsStreaming)
                {
                    if (MessageBox.Show("Streaming mode will be disabled for this Strategy Window.  Continue?", "Multi-Symbol Backtest", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    this.bool_5 = true;
                    try
                    {
                        this.btnStreaming.Checked = false;
                    }
                    finally
                    {
                        this.bool_5 = false;
                    }
                }
                this.BarDataScale = this.MyMainForm.BarDataScale;
                this.bool_10 = true;
                this.bool_12 = false;
                this.bool_11 = false;
                this.progRunAll.Enabled = true;
                this.progRunAll.Value = 0;
                this.method_16();
            }
        }

        public bool SaveStrategy()
        {
            if (this.Strategy != null)
            {
                if (this.Strategy.Name == "")
                {
                    return this.SaveStrategyAs();
                }
                if (((this.editor_0 != null) || (this.builder_0 != null)) || (this.combinationStrategyBuilder_0 != null))
                {
                    if (this.editor_0 != null)
                    {
                        this.Strategy.Code = this.editor_0.Code;
                    }
                    if (this.builder_0 != null)
                    {
                        try
                        {
                            this.Strategy.Code = this.builder_0.GenerateCode();
                        }
                        catch
                        {
                            this.Strategy.Code = "";
                        }
                        this.builder_0.PushRulesToStrategy(this.Strategy);
                    }
                    this.method_48();
                    this.method_47();
                    this.RememberStrategySettings();
                    if (this.ParametersNeedSave && MainModule.Instance.Settings.Get("RememberParameterSliders", false))
                    {
                        MainModule.Instance.Strategies.SaveParameterValues(this.Strategy, this.WealthScript);
                    }
                    MainModule.Instance.Strategies.SaveStrategy(this.Strategy);
                }
                MainModule.Instance.AddStrategyToMRU(this.Strategy);
                this.NeedSave = false;
            }
            return true;
        }

        public bool SaveStrategyAs()
        {
            bool flag2 = (this.Strategy != null) && (this.Strategy.Name == "");
            bool flag = false;
            SaveStrategyAsForm form = new SaveStrategyAsForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (this.Strategy == null)
                {
                    WealthLab.Strategy item = new WealthLab.Strategy {
                        StrategyType = StrategyType.Script,
                        Code = MainModule.Instance.StrategyTemplateCode
                    };
                    if ((item.FileName.Length > 0) && (item.StrategyType != StrategyType.Compiled))
                    {
                        MainModule.Instance.Strategies.Strategies.Remove(item);
                        WealthLab.Strategy strategy3 = WealthLab.Strategy.FromFile(item.FileName);
                        strategy3.ID = item.ID;
                        strategy3.Folder = item.Folder;
                        strategy3.FileName = item.FileName;
                        strategy3.RestoreSavedParameterValues(this.WealthScript);
                        strategy3.LoadPreferredValues(this.Symbol, this.WealthScript);
                        MainModule.Instance.Strategies.Strategies.Add(strategy3);
                    }
                    this.strategy_0 = item;
                    this.method_48();
                    this.method_47();
                    this.Strategy = item;
                    flag = true;
                }
                WealthLab.Strategy strategy = new WealthLab.Strategy {
                    StrategyType = this.strategy_0.StrategyType,
                    CombinedStrategyChildren = this.strategy_0.CombinedStrategyChildren,
                    Name = form.StrategyName
                };
                if (this.editor_0 != null)
                {
                    strategy.Code = this.editor_0.Code;
                }
                else
                {
                    strategy.Code = this.strategy_0.Code;
                }
                if (this.builder_0 != null)
                {
                    try
                    {
                        strategy.Code = this.builder_0.GenerateCode();
                    }
                    catch
                    {
                        strategy.Code = "";
                    }
                    if (flag2)
                    {
                        this.builder_0.PushRulesToStrategy(this.Strategy);
                    }
                }
                strategy.Description = this.Strategy.Description;
                strategy.Indicators = this.Strategy.Indicators;
                strategy.References = this.Strategy.References;
                if (MainModule.Instance.Settings.Get("RememberParameterSliders", false))
                {
                    strategy.PreferredValues = this.Strategy.PreferredValues;
                    strategy.ParameterValues = this.Strategy.ParameterValues;
                }
                if (this.builder_0 != null)
                {
                    this.builder_0.PushRulesToStrategy(strategy);
                    strategy.Code = this.builder_0.GenerateCode();
                }
                strategy.SinglePosition = this.Strategy.SinglePosition;
                this.method_4();
                this.method_2();
                this.strategy_0 = strategy;
                this.description_0.Strategy = this.strategy_0;
                MainModule.Instance.AddStrategyToMRU(this.Strategy);
                this.RememberStrategySettings();
                this.method_48();
                this.method_47();
                MainModule.Instance.Strategies.SaveStrategy(this.strategy_0, form.FolderName, form.NetworkPath);
                if (this.ParametersNeedSave && MainModule.Instance.Settings.Get("RememberParameterSliders", false))
                {
                    MainModule.Instance.Strategies.SaveParameterValues(this.strategy_0, this.WealthScript);
                    this.ParametersNeedSave = false;
                }
                this.NeedSave = false;
            }
            this.method_15();
            if (flag)
            {
                this.GoButtonPressed(this.Symbol, false);
                this.MyMainForm.ActivateMdiChild();
            }
            return !this.NeedSave;
        }

        public int SaveWorkspaceItems(IList<string> items)
        {
            string str;
            if (this.DataSource == null)
            {
                items.Add("");
            }
            else
            {
                items.Add(this.DataSource.Name);
            }
            items.Add(this.Symbol);
            items.Add(this.BarDataScale.ToString());
            items.Add(this.DataRange.ToString());
            items.Add(this.PositionSize.ToString());
            items.Add(this.StatusPanelsVisible.ToString());
            items.Add(this.chartRenderer_0.IndicatorLabelsVisible.ToString());
            items.Add(this.chartRenderer_0.FundamentalsVisible.ToString());
            items.Add(this.btnLink.Checked.ToString());
            items.Add(this.btnStreaming.Checked.ToString());
            if (this.Strategy == null)
            {
                items.Add("");
            }
            else
            {
                items.Add(this.Strategy.ID.ToString());
            }
            items.Add(this.chartRenderer_0.LogScale.ToString());
            items.Add(this.AutoStage.ToString());
            items.Add(this.list_2.Count.ToString());
            foreach (TabPage page in this.list_2)
            {
                UserControl control = page.Controls[0] as UserControl;
                IPerformanceVisualizer visualizer = control as IPerformanceVisualizer;
                StringBuilder builder = new StringBuilder();
                builder.Append(visualizer.TabText);
                if (visualizer is ISettingsProvider)
                {
                    builder.Append("|");
                    builder.Append((visualizer as ISettingsProvider).SettingsString);
                }
                items.Add(builder.ToString());
            }
            this.method_49(out str);
            items.Add(str);
            items.Add(this.chartRenderer_0.SavePaneSizes());
            items.Add(this.EmailAlerts.ToString());
            items.Add(this.chartRenderer_0.SaveHiddenPaneOrigHeights());
            return 8;
        }

        public void SelectAll()
        {
            if ((this.editor_0 != null) && (this.tabChart.SelectedTab.Text == "Editor"))
            {
                this.editor_0.SelectAll();
            }
        }

        public void SelectChartTab()
        {
            this.tabChart.SelectedTab = this.pageChart;
        }

        public void SelectPosition(Position position)
        {
            if (this.Strategy.StrategyType != StrategyType.CombinedStrategy)
            {
                this.tabChart.SelectedTab = this.pageChart;
                this.ShowMultiSymbolControls(false);
                if (position.Bars != this.Bars)
                {
                    this.MyMainForm.SelectTreeNode(this.DataSource, position.Bars.Symbol);
                }
                if (this.chart.Bars != this.Bars)
                {
                    this.chart.Bars = this.Bars;
                }
                this.chart.ScrollToBar(position.EntryBar);
                return;
            }
            WealthLab.Strategy iD = MainModule.Instance.Strategies.LookupID(position.StrategyID.ToString());
            if (iD == null)
            {
                return;
            }
            CombinedStrategyInfo info = null;
            using (List<CombinedStrategyInfo>.Enumerator enumerator = this.Strategy.CombinedStrategyChildren.GetEnumerator())
            {
                CombinedStrategyInfo current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.StrategyID.ToString() == position.StrategyID)
                    {
                        ///goto  Label_0080;  ///WYJ fix, simplify the flow
                        info = current;
                        break;
                    }
                }
            }
            if (info == null)
            {
                return;
            }
            this.MyMainForm.BarDataScale = info.DataScale;
            this.MyMainForm.PositionSize = info.PositionSize;
            ChartForm form = this.MyMainForm.OpenStrategyWindow(iD);
            form.BringToFront();
            form.IsStreaming = false;
            form.btnPV.Visible = true;
            form.btnPV.Checked = info.UsePreferredValues;
            this.method_7(form);
            if (info.UseDefaultDataSet)
            {
                this.MyMainForm.SelectTreeNode(this.CSDataSource, this.CSSymbol);
                this.method_7(form);
                if (this.CSSymbol == "")
                {
                    form.GoButtonPressed("", true);
                    this.method_7(form);
                }
                if (form.Symbol == "")
                {
                    this.MyMainForm.SelectTreeNode(this.DataSource, position.Symbol);
                    this.method_7(form);
                    form.SelectChartTab();
                }
            }
            else
            {
                WealthLab.DataSource source = MainModule.Instance.DataSources.FindDataSource(info.DataSetName);
                if (source == null)
                {
                    return;
                }
                if (info.Symbol == "")
                {
                    this.MyMainForm.SelectTreeNode(source, "");
                    form.GoButtonPressed("", true);
                    this.method_7(form);
                    if (form.Symbol == "")
                    {
                        this.MyMainForm.SelectTreeNode(source, position.Symbol);
                        this.method_7(form);
                        form.SelectChartTab();
                    }
                }
                else
                {
                    this.MyMainForm.SelectTreeNode(source, position.Symbol);
                    this.method_7(form);
                }
            }
            form.Bars = position.Bars;
            form.ChildScrollBar = position.EntryBar;
            form.chart.ScrollToBar(position.EntryBar);
        }

        public void SelectPV(string string_2)
        {
            this.SelectTab(string_2);
        }

        public void SelectSymbol(string symbol)
        {
            this.tabChart.SelectedTab = this.pageChart;
            this.ShowMultiSymbolControls(false);
            this.MyMainForm.SelectTreeNode(this.DataSource, symbol);
        }

        public void SelectTab(string tabText)
        {
            if (this.tabChart.SelectedTab.Text != "Optimization")
            {
                IEnumerator enumerator = this.tabChart.TabPages.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            TabPage current = (TabPage)enumerator.Current;
                            if (current.Text == tabText)
                            {
                                this.tabChart.SelectedTab = current;
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
                return;
            }
            else
            {
                return;
            }
        }

        public void SetBarDataScaleForDataSource(WealthLab.DataSource dataSource_3, WealthLab.BarDataScale barDataScale_0)
        {
            if (dataSource_3 != null)
            {
                this.dictionary_0[dataSource_3] = barDataScale_0;
            }
        }

        public void SetChartCursor(Cursor cursor)
        {
            this.chart.Cursor = cursor;
        }

        public void SetScaleToUseForDataSet(WealthLab.DataSource dataSource_3, WealthLab.BarDataScale barDataScale_0)
        {
            this.dictionary_0[dataSource_3] = barDataScale_0;
        }

        public void SetupChartTradeMenuItems()
        {
            if ((this.Bars != null) && (this.Bars.Count > 0))
            {
                double num;
                if (this.tradingSystemExecutor_1.Performance.Results.EquityCurve.Count == 0)
                {
                    num = this.tradingSystemExecutor_1.CalcPositionSize(this.Bars, this.Bars.Count, this.Bars.Close[this.Bars.Count - 1], PositionType.Long, 0.0, this.PositionSize.StartingCapital);
                }
                else
                {
                    num = this.tradingSystemExecutor_1.CalcPositionSize(this.Bars, this.Bars.Count, this.Bars.Close[this.Bars.Count - 1], PositionType.Long, 0.0);
                }
                if (num > 0.0)
                {
                    string str = string.Concat(new object[] { " ", num, " ", this.Bars.Symbol, " (", this.Bars.SecurityName, ")" });
                    this.mniChartBuy.Text = "Buy" + str;
                    this.mniChartSell.Text = "Sell" + str;
                    this.mniChartShort.Text = "Short" + str;
                    this.mniChartCover.Text = "Cover" + str;
                    this.mniChartBuy.Enabled = true;
                    this.mniChartSell.Enabled = true;
                    this.mniChartShort.Enabled = true;
                    this.mniChartCover.Enabled = true;
                    this.mniChartBuy.Tag = num;
                }
                else
                {
                    this.mniChartBuy.Enabled = false;
                    this.mniChartSell.Enabled = false;
                    this.mniChartShort.Enabled = false;
                    this.mniChartCover.Enabled = false;
                }
            }
            else
            {
                this.mniChartBuy.Enabled = false;
                this.mniChartSell.Enabled = false;
                this.mniChartSell.Enabled = false;
                this.mniChartCover.Enabled = false;
            }
        }

        public void ShowAutoTradingState(AutoTradingMode mode)
        {
            if (this.alerts_0 != null)
            {
                this.alerts_0.ShowAutoTradingState(mode);
            }
        }

        public void ShowLoggedInState(bool loggedIn)
        {
            if (this.alerts_0 != null)
            {
                this.alerts_0.ShowLoggedInState(loggedIn);
            }
        }

        public void ShowMultiSymbolControls(bool show)
        {
            bool flag2;
            if ((flag2 = (this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy)) || (this.WealthScript != null))
            {
                bool flag = (show || this.bool_10) ? (this.WealthScript != null) : false;
                if (flag2)
                {
                    flag = show || this.bool_10;
                }
                this.pnlMultiSymbol.Visible = show;
                if (show)
                {
                    this.pageChart.Text = "DataSet";
                    this.btnStreaming.Enabled = false;
                }
                else
                {
                    this.pageChart.Text = "Chart";
                    if ((this.Strategy == null) || (this.Strategy.StrategyType != StrategyType.CombinedStrategy))
                    {
                        this.btnStreaming.Enabled = true;
                    }
                }
                this.btnRunAll.Visible = flag;
                if (this.DataSource != null)
                {
                    this.btnRunAll.Text = "Backtest on all Symbols in " + this.DataSource.Name;
                }
                if (flag2)
                {
                    this.btnRunAll.Text = "Backtest Combination Strategy";
                    this.grpMultiSymbol.Text = "Combination Strategy Backtest";
                }
                else
                {
                    this.grpMultiSymbol.Text = "Multi Symbol Backtest";
                }
                this.progRunAll.Visible = flag;
                this.btnRunAllCancel.Visible = flag;
                this.lblRunAllStatus.Visible = flag;
                this.progRunAll.Visible = flag;
            }
        }

        public bool ShowPrintDialog()
        {
            return !MainModule.Instance.Settings.Get("HidePrintDialog", false);
        }

        public bool ShowPrintPreview()
        {
            return !MainModule.Instance.Settings.Get("HidePrintPreview", false);
        }

        private void statusAddToSC_Click(object sender, EventArgs e)
        {
            this.AddToStrategyMonitor();
        }

        private void statusOptimize_Click(object sender, EventArgs e)
        {
            this.optimization_0 = new WealthLabPro.Optimization();
            this.optimization_0.Dock = DockStyle.Fill;
            this.tabChart.TabPages.Add("Optimization");
            TabPage page = this.tabChart.TabPages[this.tabChart.TabPages.Count - 1];
            page.Controls.Add(this.optimization_0);
            this.optimization_0.Initialize(this);
            this.statusOptimize.Visible = false;
            this.tabChart.SelectedTab = page;
            if (base.Height < 510)
            {
                base.Height = 510;
            }
            this.btnStreaming.Visible = false;
        }

        public void StatusUpdate(ConnStatus status, int StatusCode, string Message)
        {
        }

        public void StrategySummary(ref DataObject printObject)
        {
            this.method_55(ref printObject);
        }

        public void SymbolSelected(WealthLab.DataSource dataSource_3, string symbol)
        {
            this.DataSourceSelected(dataSource_3);
            this.Symbol = symbol;
            if (this.dataSource_1 != this.DataSource)
            {
                this.bool_10 = false;
                this.ShowMultiSymbolControls(false);
            }
            this.chart.MultiSymbolMode = this.bool_10;
            if (this.bool_10)
            {
                using (List<WealthLab.Bars>.Enumerator enumerator = this.list_0.GetEnumerator())
                {
                    WealthLab.Bars current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.Symbol == symbol)
                        {
                            ///goto  Label_0079;  ///WYJ fix, simplify the flow
                            this.Bars = current;
                            if (this.WealthScript != null)
                            {
                                this.WealthScript.Renderer = this.chartRenderer_0;
                            }
                            this.method_13(this.tradingSystemExecutor_0, current, false);
                            this.tradingSystemExecutor_0.Clear();
                            this.indicatorDragDropManager_0.CreateDragDropIndicators();
                            this.chart.DoInvalidate();
                            this.method_15();
                            return;
                        }
                    }
                    return;
                }
            }
            if ((this.Strategy == null) || (this.Strategy.StrategyType != StrategyType.CombinedStrategy))
            {
                this.bool_10 = false;
                this.ShowMultiSymbolControls(false);
                this.method_16();
            }
        }

        private void timer_0_Tick(object sender, EventArgs e)
        {
            if (this.streamingChartManager_0.GhostBarHasUpdated)
            {
                this.chart.DoInvalidate();
            }
        }

        private void txtSymbol_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\r':
                    this.pnlSymbol.Visible = false;
                    this.MyMainForm.Symbol = this.txtSymbol.Text;
                    this.chart.Focus();
                    break;

                case '\x001b':
                    this.pnlSymbol.Visible = false;
                    this.chart.Focus();
                    break;
            }
        }

        public void UpdateChartColorsAndStyle(bool refresh)
        {
            this.chartRenderer_0.AssignProperties(MainModule.Instance.Renderer);
            SettingsManager settings = MainModule.Instance.Settings;
            this.chart.PriceTooltipVisible = settings.Get("PriceTooltip", true);
            this.chart.IndicatorTooltipVisible = settings.Get("IndicatorTooltip", true);
            this.chart.FundamentalTooltipVisible = settings.Get("FundamentalTooltip", true);
            if (refresh)
            {
                this.chart.DoInvalidate();
            }
        }

        public bool AbortedRequest
        {
            get
            {
                return this.bool_6;
            }
        }

        public string AccountNumber
        {
            get
            {
                if ((this.Strategy != null) && (this.Strategy.AccountNumber != ""))
                {
                    return this.Strategy.AccountNumber;
                }
                return MainModule.Instance.DefaultAccountNumber;
            }
        }

        private bool AllowEditBarData
        {
            get
            {
                if (this.method_66())
                {
                    return false;
                }
                WealthLab.BarDataScale barDataScale = this.DataSource.BarDataScale;
                WealthLab.BarDataScale dataScale = this.Bars.DataScale;
                if (barDataScale != dataScale)
                {
                    return false;
                }
                return (((this.int_2 != -1) && (this.DataSource.Provider.DataStore != null)) && (!this.btnStreaming.Checked && this.DataSource.Provider.CanEditSymbolDataFile));
            }
        }

        public bool AutoStage
        {
            get
            {
                if (this.alerts_0 == null)
                {
                    return false;
                }
                return this.alerts_0.AutoStage;
            }
            set
            {
                if (this.description_0 != null)
                {
                    this.description_0.PopulateActivationSection();
                }
            }
        }

        public WealthLab.BarDataScale BarDataScale
        {
            get
            {
                return this.barsLoader_0.BarDataScale;
            }
            set
            {
                this.barsLoader_0.BarDataScale = value;
            }
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.chart.Bars;
            }
            set
            {
                if (!base.DesignMode)
                {
                    this.chart.Bars = value;
                    this.drawingObjectManager_0.LoadDrawingObjects(value);
                    bool positions = this.drawingObjectManager_0.HasObjectsThatCanTriggerAlerts || (this.Strategy != null);
                    if (!this.IsStreaming)
                    {
                        this.MyMainForm.SetDataPanelState(true, true, positions, this.Strategy != null, (this.Strategy != null) && (this.Strategy.StrategyType == StrategyType.CombinedStrategy));
                    }
                    if (value.Count > 0)
                    {
                        int num = value.Count - 1;
                        string str = value.Date[num].ToShortDateString();
                        if (value.IsIntraday)
                        {
                            str = str + " " + value.Date[num].ToShortTimeString();
                        }
                        this.stlblLastDate.Text = "Last Date: " + str;
                    }
                    else
                    {
                        this.stlblLastDate.Text = "Last Date:";
                    }
                    this.stlblBars.Text = "Bars: " + value.Count.ToString("N0");
                    this.SetupChartTradeMenuItems();
                }
            }
        }

        public WealthLab.ChartStyle ChartStyle
        {
            get
            {
                return this.chart.ChartStyle;
            }
            set
            {
                if (value is ICustomSettings)
                {
                    (value as ICustomSettings).ReadSettings(MainModule.Instance.Settings);
                }
                this.chart.ChartStyle = value;
                this.chart.DoInvalidate();
                this.mniChartStyleSettings.Enabled = value is ICustomSettings;
            }
        }

        internal int ChildScrollBar
        {
            [CompilerGenerated]
            get
            {
                return this.int_5;
            }
            [CompilerGenerated]
            set
            {
                this.int_5 = value;
            }
        }

        public WealthLab.DataSource CSDataSource
        {
            [CompilerGenerated]
            get
            {
                return this.dataSource_2;
            }
            [CompilerGenerated]
            set
            {
                this.dataSource_2 = value;
            }
        }

        public string CSSymbol
        {
            [CompilerGenerated]
            get
            {
                return this.string_1;
            }
            [CompilerGenerated]
            set
            {
                this.string_1 = value;
            }
        }

        public int CurrentTabIndex
        {
            get
            {
                return this.tabChart.SelectedIndex;
            }
        }

        public string CurrentTabName
        {
            get
            {
                if (this.tabChart.SelectedTab == null)
                {
                    return "";
                }
                return this.tabChart.SelectedTab.Text;
            }
        }

        public BarDataRange DataRange
        {
            get
            {
                return BarDataRange.Parse(this.barRange.DataRange.ToString());
            }
            set
            {
                BarDataRange range = BarDataRange.Parse(value.ToString());
                this.barRange.DataRange = range;
            }
        }

        public WealthLab.DataSource DataSource
        {
            get
            {
                return this.dataSource_0;
            }
            set
            {
                this.dataSource_0 = value;
            }
        }

        public bool DisconnectedWhileStreaming
        {
            get
            {
                return this.bool_14;
            }
        }

        public string EditorCode
        {
            get
            {
                if (this.editor_0 != null)
                {
                    return this.editor_0.Code;
                }
                return "";
            }
            set
            {
                if (this.editor_0 != null)
                {
                    this.editor_0.Code = value;
                }
            }
        }

        public bool EmailAlerts
        {
            get
            {
                if (this.alerts_0 == null)
                {
                    return false;
                }
                return this.alerts_0.EmailAlerts;
            }
            set
            {
                if (this.description_0 != null)
                {
                    this.description_0.PopulateActivationSection();
                }
            }
        }

        public bool HasEditor
        {
            get
            {
                return (this.editor_0 != null);
            }
        }

        public bool IsBusy
        {
            get
            {
                return ((this.thread_0 != null) && this.thread_0.IsAlive);
            }
        }

        public bool IsOptimizing
        {
            [CompilerGenerated]
            get
            {
                return this.bool_17;
            }
            [CompilerGenerated]
            set
            {
                this.bool_17 = value;
            }
        }

        public bool IsStreaming
        {
            get
            {
                return this.btnStreaming.Checked;
            }
            set
            {
                if (value != this.btnStreaming.Checked)
                {
                    this.btnStreaming.Checked = value;
                }
            }
        }

        public bool LinkedToSymbol
        {
            get
            {
                return this.btnLink.Checked;
            }
        }

        public bool MultiSymbolMode
        {
            get
            {
                return this.bool_10;
            }
        }

        public MainForm MyMainForm
        {
            get
            {
                return (base.MdiParent as MainForm);
            }
        }

        public bool NeedSave
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                if (this.bool_0 != value)
                {
                    this.bool_0 = value;
                    if (this.bool_0)
                    {
                        this.bool_1 = false;
                    }
                    this.method_15();
                    this.btnPV.Visible = ((this.strategy_0 != null) && (this.strategy_0.PreferredValues != null)) && (this.strategy_0.PreferredValues != "");
                    if (!value && (this.optimization_0 != null))
                    {
                        this.optimization_0.DisableRollback();
                    }
                }
            }
        }

        public WealthLabPro.Optimization Optimization
        {
            get
            {
                return this.optimization_0;
            }
        }

        public bool ParametersNeedSave
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

        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return WealthLab.PositionSize.Parse(this.posSize.PositionSize.ToString());
            }
            set
            {
                WealthLab.PositionSize size = WealthLab.PositionSize.Parse(value.ToString());
                this.posSize.PositionSize = size;
                this.SetupChartTradeMenuItems();
            }
        }

        public ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer_0;
            }
        }

        public IPerformanceVisualizer SelectedVisualizer
        {
            get
            {
                TabPage selectedTab = this.tabChart.SelectedTab;
                if (selectedTab == null)
                {
                    return null;
                }
                return (selectedTab.Tag as IPerformanceVisualizer);
            }
        }

        public bool SliderValueChanging
        {
            [CompilerGenerated]
            get
            {
                return this.bool_16;
            }
            [CompilerGenerated]
            set
            {
                this.bool_16 = value;
            }
        }

        public bool StatusPanelsVisible
        {
            get
            {
                return this.status.Visible;
            }
            set
            {
                this.status.Visible = value;
                this.statusChart.Visible = value;
                if (value)
                {
                    this.chart.Height = (this.pageChart.Height - this.statusChart.Height) - 9;
                }
                else
                {
                    this.chart.Height = this.pageChart.Height - 4;
                }
            }
        }

        public WealthLab.Strategy Strategy
        {
            get
            {
                return this.strategy_0;
            }
            set
            {
                this.strategy_0 = value;
                if (this.strategy_0 == null)
                {
                    this.btnPV.Visible = false;
                    this.btnPV.Checked = false;
                }
                else
                {
                    this.btnPV.Visible = (this.strategy_0.PreferredValues != null) && (this.strategy_0.PreferredValues != "");
                    this.statusAddStrategy.Visible = false;
                    this.method_2();
                    this.method_4();
                    this.mniAddStrategy.Visible = false;
                    this.mniAddDifferent.Visible = true;
                    this.stlblProfit.Visible = true;
                    this.stlblPerBar.Visible = true;
                    this.stlblBHProfit.Visible = true;
                    this.stlblBHPerBar.Visible = true;
                    if ((this.tabChart.TabCount == 1) || (this.tabChart.TabCount == 2))
                    {
                        this.tabChart.TabPages.Add("Strategy Summary");
                        this.description_0 = new Description();
                        this.description_0.Dock = DockStyle.Fill;
                        TabPage page4 = this.tabChart.TabPages[this.tabChart.TabCount - 1];
                        page4.Controls.Add(this.description_0);
                        if (this.strategy_0.StrategyType == StrategyType.Script)
                        {
                            if (this.editor_0 == null)
                            {
                                this.editor_0 = new Editor(this);
                                this.editor_0.Dock = DockStyle.Fill;
                                this.tabChart.TabPages.Add("Editor");
                                TabPage page2 = this.tabChart.TabPages[this.tabChart.TabCount - 1];
                                page2.Controls.Add(this.editor_0);
                            }
                        }
                        else if (this.strategy_0.StrategyType == StrategyType.Rules)
                        {
                            if (this.builder_0 == null)
                            {
                                this.builder_0 = new Builder();
                                this.builder_0.Strategy = this.Strategy;
                                this.builder_0.Dock = DockStyle.Fill;
                                this.tabChart.TabPages.Add("Rules");
                                TabPage page3 = this.tabChart.TabPages[this.tabChart.TabCount - 1];
                                page3.Controls.Add(this.builder_0);
                                this.NeedSave = false;
                                this.builder_0.CompileRules();
                            }
                        }
                        else if (this.strategy_0.StrategyType == StrategyType.CombinedStrategy)
                        {
                            if (this.combinationStrategyBuilder_0 == null)
                            {
                                this.combinationStrategyBuilder_0 = new CombinationStrategyBuilder(this);
                                this.combinationStrategyBuilder_0.CombinationStrategy = this.Strategy;
                                this.combinationStrategyBuilder_0.Dock = DockStyle.Fill;
                                this.tabChart.TabPages.Add("Combination Strategy");
                                TabPage page = this.tabChart.TabPages[this.tabChart.TabCount - 1];
                                page.Controls.Add(this.combinationStrategyBuilder_0);
                                this.NeedSave = false;
                                base.Invoke(new Delegate45(this.method_1));
                                if (base.Height < 580)
                                {
                                    base.Height = 580;
                                }
                            }
                        }
                        else
                        {
                            this.WealthScript = MainModule.Instance.Strategies.GetWealthScriptObject(this.strategy_0);
                        }
                    }
                    this.method_15();
                    if (this.editor_0 != null)
                    {
                        this.editor_0.Code = this.strategy_0.Code;
                        this.editor_0.Compile();
                    }
                    if (this.builder_0 != null)
                    {
                        this.builder_0.Strategy = this.Strategy;
                    }
                    this.description_0.Strategy = this.strategy_0;
                    this.method_39();
                    this.LoadDragDropIndicators(this.strategy_0.Indicators);
                    this.method_3(this.strategy_0.PanelSize);
                    this.bool_7 = true;
                    this.btnPV.Checked = this.Strategy.UsePreferredValues;
                    this.bool_7 = false;
                    if (this.Strategy.StrategyType == StrategyType.CombinedStrategy)
                    {
                        if (this.btnStreaming.Checked)
                        {
                            this.btnStreaming.Checked = false;
                        }
                        this.btnStreaming.Enabled = false;
                    }
                    this.NeedSave = false;
                }
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
                if ((this.IsStreaming && (this.string_0 != null)) && this.string_0.Trim().StartsWith("%"))
                {
                    MessageBox.Show("Unable to stream the Index-Lab symbol \"" + this.string_0 + "\". Data streaming will be disconnected.");
                    this.IsStreaming = false;
                }
            }
        }

        public WealthLab.WealthScript WealthScript
        {
            get
            {
                return this.wealthScript_0;
            }
            set
            {
                this.wealthScript_0 = value;
            }
        }

        private delegate void Delegate43();

        private delegate void Delegate44(Exception exception_0);

        private delegate void Delegate45();

        private delegate void Delegate46();

        private delegate void Delegate47(Exception exception_0);

        private delegate void Delegate48(int int_0, object object_0);

        private delegate void Delegate49();

        private delegate void Delegate50();

        private delegate void Delegate51();
    }
}

