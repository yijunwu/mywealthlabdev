namespace WealthLabPro
{
    using CtrlLib;
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;

    public class Optimization : UserControl, ISettingsHost, IOptimizationHost, IPrintHost
    {
        private AssemblyLoader assemblyLoader_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private Button btnAddParameter;
        private Button btnApplyChanges;
        private Button btnBegin;
        private Button btnCancel;
        private Button btnEstimate;
        private Button btnRemoveParam;
        private Button btnRollback;
        private Button btnSettings;
        private CheckBox cbAverage;
        private WealthLabPro.ChartForm chartForm_0;
        private ComboBox cmbMethod;
        private ComboBox cmbMetric;
        private ComboBox cmbScorecard;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private ColumnHeader columnHeader_7;
        private ColumnHeader columnHeader_8;
        private DataSource dataSource_0;
        private DateTime dateTime_0;
        private DateTime dateTime_1;
        private DateTime dateTime_2;
        private Exception exception_0;
        private GroupBox grpEdit;
        private GroupBox grpMethod;
        private GroupBox grpOptimize;
        private GroupBox grpParameters;
        private IContainer components;
        private ISettingsHost isettingsHost_0;
        private Label lblDefault;
        private Label lblDescription;
        private Label lblElaped;
        private Label lblEstimate;
        private Label lblMethod;
        private Label lblMetric;
        private Label lblRemaining;
        private Label lblRuleBased;
        private Label lblRunsRequired;
        private Label lblScorecard;
        private Label lblStart;
        private Label lblStep;
        private Label lblStop;
        private Label lblTimeElapsed;
        private Label lblTimeRemaining;
        private List<Bars> list_0 = new List<Bars>();
        private List<double> list_1 = new List<double>();
        private List<string> list_2 = new List<string>();
        private Label lnlName;
        private ListView lvErrors;
        private ListView lvParameters;
        private SortableListView lvResults;
        private WealthLabPro.MainForm mainForm_0;
        private ToolStripMenuItem mniCopyResults;
        private ToolStripMenuItem mniLoadResults;
        private ToolStripMenuItem mniPrintResults;
        private ToolStripMenuItem mniPVAllThisRow;
        private ToolStripMenuItem mniPVAvgHighest;
        private ToolStripMenuItem mniPVAvgLowest;
        private ToolStripMenuItem mniPVHighest;
        private ToolStripMenuItem mniPVLowest;
        private ToolStripMenuItem mniPVThisRow;
        private ToolStripMenuItem mniSaveResults;
        private ToolStripMenuItem mniSetDefault;
        private NumEdit numDefault;
        private NumEdit numStart;
        private NumEdit numStep;
        private NumEdit numStop;
        private OpenFileDialog openFileDialog_0;
        private OptimizationResultList optimizationResultList_0 = new OptimizationResultList();
        private Optimizer optimizer_0;
        private TabPage pageControl;
        private TabPage pageOptErrors;
        private TabPage pageResults;
        private Panel pnlResults;
        private ContextMenuStrip popupResults;
        private PositionSize positionSize_0;
        private ProgressBar progOpt;
        private SaveFileDialog saveFileDialog_0;
        private ToolStripSeparator sep2;
        private ToolStripSeparator sep3;
        private ToolStripSeparator sepCopy;
        private StrategyScorecard strategyScorecard_0;
        private string string_0;
        private SystemPerformance systemPerformance_0;
        private TabControl tabOptimize;
        private ToolStripSeparator toolStripSeparator_0;
        private TradingSystemExecutor tradingSystemExecutor_0;
        private TextBox txtName;

        public Optimization()
        {
            this.InitializeComponent();
            this.isettingsHost_0 = MainModule.Instance.Settings;
        }

        private void btnAddParameter_Click(object sender, EventArgs e)
        {
            StrategyParameter item = new StrategyParameter("", 1.0, 1.0, 10.0, 1.0);
            this.WealthScript.Parameters.Add(item);
            item.Name = this.method_10();
            this.method_0(item);
            this.UpdateRunsRequired();
            this.lvParameters.Items[this.lvParameters.Items.Count - 1].Selected = true;
            this.ApplyNeeded = true;
            this.lvParameters.Focus();
        }

        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            this.updateEditorForParameters();
            this.ApplyNeeded = false;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            this.method_4();
            if (this.WealthScript == null)
            {
                MessageBox.Show("Could not optimize this Strategy, please check for errors in the Strategy.");
            }
            else
            {
                this.optimizer_0.WealthScript = this.WealthScript;
                this.list_1.Clear();
                foreach (StrategyParameter parameter in this.WealthScript.Parameters)
                {
                    this.list_1.Add(parameter.Value);
                }
                this.optimizationResultList_0.StrategyID = this.chartForm_0.Strategy.ID.ToString();
                this.optimizationResultList_0.Scorecard = this.strategyScorecard_0.FriendlyName;
                this.optimizationResultList_0.OptimizationMethod = this.optimizer_0.FriendlyName;
                this.lvErrors.Items.Clear();
                if (this.chartForm_0.DataSource == null)
                {
                    MessageBox.Show("Please click a DataSet or Symbol in the Tree to optimize on.");
                }
                else
                {
                    this.optimizationResultList_0.Clear();
                    this.method_5();
                    string selectedItem = this.cmbMetric.SelectedItem as string;
                    this.cmbMetric.Items.Clear();
                    for (int i = this.WealthScript.Parameters.Count + 1; i < this.lvResults.Columns.Count; i++)
                    {
                        this.cmbMetric.Items.Add(this.lvResults.Columns[i].Text);
                    }
                    if (selectedItem != null)
                    {
                        this.cmbMetric.SelectedIndex = this.cmbMetric.Items.IndexOf(selectedItem);
                    }
                    if ((this.cmbMetric.SelectedIndex == -1) && (this.cmbMetric.Items.Count > 0))
                    {
                        this.cmbMetric.SelectedIndex = 0;
                    }
                    for (int j = this.WealthScript.Parameters.Count + 1; j < this.lvResults.Columns.Count; j++)
                    {
                        ColumnHeader header = this.lvResults.Columns[j];
                        switch ((header.Tag as string))
                        {
                            case "N":
                            case "C":
                                this.optimizationResultList_0.Names.Add(header.Text);
                                break;
                        }
                    }
                    if ((this.optimizer_0.NumberOfRuns <= 10000.0) || (MessageBox.Show("This Optimization requires a large number of runs and the potential for significant processing time.  Continue anyway?", "Optimization", MessageBoxButtons.YesNo) != DialogResult.No))
                    {
                        this.bool_1 = false;
                        this.Cursor = Cursors.WaitCursor;
                        this.string_0 = this.mainForm_0.Symbol;
                        this.dataSource_0 = this.mainForm_0.DataSource;
                        this.positionSize_0 = this.chartForm_0.posSize.PositionSize;
                        this.bool_4 = this.string_0 == "";
                        this.progOpt.Value = 0;
                        try
                        {
                            this.progOpt.Maximum = (int) this.optimizer_0.NumberOfRuns;
                        }
                        catch
                        {
                        }
                        if (this.chartForm_0.posSize.PositionSize.RawProfitMode && (this.string_0 == ""))
                        {
                            this.progOpt.Maximum *= this.dataSource_0.Symbols.Count;
                        }
                        this.btnBegin.Enabled = false;
                        this.btnCancel.Enabled = true;
                        this.chartForm_0.IsOptimizing = true;
                        this.btnEstimate.Enabled = false;
                        this.dateTime_0 = DateTime.Now;
                        new Thread(new ThreadStart(this.method_6)) { IsBackground = true }.Start();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.bool_1 = true;
        }

        private void btnEstimate_Click(object sender, EventArgs e)
        {
            if (this.chartForm_0.DataSource == null)
            {
                MessageBox.Show("Please click a DataSet or Symbol in the Tree to Estimate.");
            }
            else
            {
                this.string_0 = this.mainForm_0.Symbol;
                this.dataSource_0 = this.mainForm_0.DataSource;
                this.positionSize_0 = this.chartForm_0.posSize.PositionSize;
                this.btnEstimate.Enabled = false;
                this.btnBegin.Enabled = false;
                new Thread(new ThreadStart(this.method_13)) { IsBackground = true }.Start();
            }
        }

        private void btnRemoveParam_Click(object sender, EventArgs e)
        {
            if (this.lvParameters.SelectedItems.Count == 1)
            {
                this.WealthScript.Parameters.Remove(this.SelectedParameter);
                this.LoadParameterList();
                this.UpdateRunsRequired();
                this.ApplyNeeded = true;
            }
        }

        private void btnRollback_Click(object sender, EventArgs e)
        {
            this.chartForm_0.CompileSource();
            Strategy iD = MainModule.Instance.Strategies.LookupID(this.chartForm_0.Strategy.ID.ToString());
            if (iD != null)
            {
                this.chartForm_0.Strategy = iD;
            }
            this.LoadParameterList();
            MessageBox.Show("Your changes have been rolled back and the Strategy Parameters refreshed from the Editor Code.");
            this.chartForm_0.MyMainForm.BuildParameterSliders();
            this.ApplyNeeded = false;
            this.btnRollback.Enabled = false;
            this.optimizer_0.WealthScript = this.WealthScript;
            this.UpdateRunsRequired();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            OptimizerSettings settings = new OptimizerSettings();
            ICustomSettings settings2 = this.optimizer_0 as ICustomSettings;
            UserControl settingsUI = settings2.GetSettingsUI();
            settings.AddUserControl(settingsUI);
            if (settings.ShowDialog(this.chartForm_0) == DialogResult.OK)
            {
                settings2.ChangeSettings(settingsUI);
                settings2.WriteSettings(this);
                this.UpdateRunsRequired();
            }
        }

        private void cbAverage_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbAverage.Visible)
            {
                if (this.cbAverage.Checked)
                {
                    this.method_9(this.optimizationResultList_0.AverageResultList);
                }
                else
                {
                    this.method_9(this.optimizationResultList_0);
                }
            }
        }

        public void Close()
        {
            this.isettingsHost_0.Set("Optimization.Method", this.cmbMethod.SelectedItem.ToString());
            this.isettingsHost_0.Set("Optimization.Scorecard", this.cmbScorecard.SelectedItem.ToString());
        }

        private void cmbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (this.tabOptimize.TabPages.Count > 3)
            {
                this.tabOptimize.TabPages.RemoveAt(3);
            }
            this.optimizer_0 = this.cmbMethod.SelectedItem as Optimizer;
            if (this.optimizer_0 != null)
            {
                this.lblDescription.Text = this.optimizer_0.Description;
                this.optimizer_0.WealthScript = this.WealthScript;
                this.optimizer_0.Strategy = this.chartForm_0.Strategy;
                ICustomSettings settings = this.optimizer_0 as ICustomSettings;
                if (settings != null)
                {
                    this.btnSettings.Enabled = true;
                    settings.ReadSettings(this);
                }
                else
                {
                    this.btnSettings.Enabled = false;
                }
                this.optimizer_0.Initialize();
                this.UpdateRunsRequired();
            }
        }

        private void cmbScorecard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbScorecard.SelectedItem != null)
            {
                this.strategyScorecard_0 = (StrategyScorecard) this.cmbScorecard.SelectedItem;
                if (this.cmbMetric.Items.Count == 0)
                {
                    string selectedItem = this.cmbMetric.SelectedItem as string;
                    IList<string> list = this.chartForm_0.posSize.PositionSize.RawProfitMode ? this.strategyScorecard_0.ColumnHeadersRawProfit : this.strategyScorecard_0.ColumnHeadersPortfolioSim;
                    this.cmbMetric.Items.Clear();
                    foreach (string str2 in list)
                    {
                        this.cmbMetric.Items.Add(str2);
                    }
                    if (selectedItem != null)
                    {
                        if (this.cmbMetric.Items.IndexOf(selectedItem) >= 0)
                        {
                            this.cmbMetric.SelectedIndex = this.cmbMetric.Items.IndexOf(selectedItem);
                        }
                        else if (this.cmbMetric.Items.Count > 0)
                        {
                            this.cmbMetric.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        public bool ContainsKey(string string_1)
        {
            return this.isettingsHost_0.ContainsKey(this.method_1(string_1));
        }

        public void CreateTab(string text, UserControl userControl_0)
        {
            this.tabOptimize.TabPages.Add(text);
            TabPage page = this.tabOptimize.TabPages[this.tabOptimize.TabPages.Count - 1];
            page.Controls.Add(userControl_0);
            userControl_0.Dock = DockStyle.Fill;
        }

        public void DisableRollback()
        {
            this.btnRollback.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Get(string string_1, bool defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_1), defaultValue);
        }

        public DateTime Get(string string_1, DateTime defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_1), defaultValue);
        }

        public double Get(string string_1, double defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_1), defaultValue);
        }

        public Color Get(string string_1, Color defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_1), defaultValue);
        }

        public Font Get(string string_1, Font defaultFont)
        {
            return this.isettingsHost_0.Get(this.method_1(string_1), defaultFont);
        }

        public int Get(string string_1, int defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_1), defaultValue);
        }

        public string Get(string string_1, string defaultValue)
        {
            return this.isettingsHost_0.Get(this.method_1(string_1), defaultValue);
        }

        public bool Get(Form form_0, string string_1)
        {
            return this.isettingsHost_0.Get(form_0, this.method_1(string_1));
        }

        public PageSettings GetPageSettings()
        {
            return this._pageSettings;
        }

        public DataObject GetReportTemplate(string title)
        {
            DataObject obj2 = new DataObject();
            string str = " | ";
            obj2.SetData(PrintReport.fmtBaseTitle.Name, MainModule.Instance.AuthProvider.ApplicationName);
            obj2.SetData(PrintReport.fmtTitle.Name, title);
            if (this.bool_4)
            {
                obj2.SetData(PrintReport.fmtSymbol.Name, this.dataSource_0.Name);
            }
            else
            {
                obj2.SetData(PrintReport.fmtSymbol.Name, this.string_0);
            }
            if (this.chartForm_0.Strategy != null)
            {
                obj2.SetData(PrintReport.fmtStrategy.Name, this.chartForm_0.Strategy.Name);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("Optimizer: ");
            builder.Append(this.optimizer_0.FriendlyName);
            builder.Append(str);
            builder.Append("Parameters: ");
            if (this.WealthScript != null)
            {
                bool flag = true;
                foreach (StrategyParameter parameter in this.WealthScript.Parameters)
                {
                    if (!flag)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(parameter.Name);
                    flag = false;
                }
            }
            builder.Append(str);
            builder.Append("Scorecard: ");
            builder.Append(this.strategyScorecard_0.FriendlyName);
            builder.Append(str);
            builder.AppendLine();
            if (this.positionSize_0.RawProfitMode)
            {
                builder.Append("Raw Profit Mode");
            }
            else
            {
                builder.Append("Portfolio Simulation");
                builder.Append(str);
                builder.Append("Starting Capital: ");
                builder.Append(this.positionSize_0.StartingCapital.ToString("C"));
            }
            builder.Append(str);
            builder.Append("Scale: ");
            builder.Append(this.dataSource_0.BarDataScale.ToString());
            builder.Append(str);
            builder.Append("Data Range: ");
            builder.Append(this.chartForm_0.barRangeSelector.DataRange.Text);
            builder.Append(str);
            builder.Append("Position Sizing: ");
            builder.Append(this.positionSize_0.Text);
            if (!this.bool_4 && (this.list_0.Count > 0))
            {
                Bars bars = this.list_0[0];
                builder.Append(str);
                builder.Append("Bars: ");
                builder.Append(bars.Count.ToString());
                int num = 0;
                string str2 = "";
                num = bars.Count - 1;
                str2 = bars.Date[num].ToShortDateString();
                if (bars.IsIntraday)
                {
                    str2 = str2 + " " + bars.Date[num].ToShortTimeString();
                }
                builder.Append(str);
                builder.Append("Last Bar date: ");
                builder.Append(str2);
            }
            obj2.SetData(PrintReport.fmtDetails.Name, builder.ToString());
            return obj2;
        }

        public void Initialize(ChartForm chartForm_1)
        {
            this.chartForm_0 = chartForm_1;
            this.mainForm_0 = chartForm_1.MyMainForm;
            Strategy strategy = chartForm_1.Strategy;
            WealthScript wealthScript = chartForm_1.WealthScript;
            if (strategy != null && wealthScript != null && strategy.ParameterValues.Count == wealthScript.Parameters.Count)
            {
                for (int i = 0; i < strategy.ParameterValues.Count; i++)
                {
                    wealthScript.Parameters[i].DefaultValue = strategy.ParameterValues[i];
                }
            }
            this.LoadParameterList();
            this.tradingSystemExecutor_0.BarsLoader = this.chartForm_0.barsLoader;
            this.tradingSystemExecutor_0.FundamentalsLoader = this.chartForm_0.fundamentalsLoader_0;
            this.tradingSystemExecutor_0.StrategyName = this.chartForm_0.Strategy.Name;
            this.assemblyLoader_0.Path = MainModule.Instance.AppPath;
            int count = 0;
            string str = this.isettingsHost_0.Get("Optimization.Scorecard", "");
            foreach (Type type in this.assemblyLoader_0.Types)
            {
                StrategyScorecard strategyScorecard = (StrategyScorecard)this.assemblyLoader_0.CreateInstance(type);
                this.cmbScorecard.Items.Add(strategyScorecard);
                if (strategyScorecard.FriendlyName != str)
                {
                    continue;
                }
                count = this.cmbScorecard.Items.Count - 1;
            }
            this.cmbScorecard.SelectedIndex = count;
            str = this.isettingsHost_0.Get("Optimization.Method", "");
            foreach (Optimizer optimizer in MainModule.Instance.Optimizers)
            {
                Optimizer optimizer1 = (Optimizer)MainModule.Instance.assemblyLoader_Optimizer.CreateInstance(optimizer.GetType());
                optimizer1.Host = this;
                optimizer1.PrintHost = this;
                this.cmbMethod.Items.Add(optimizer1);
                if (optimizer.FriendlyName != str)
                {
                    continue;
                }
                this.cmbMethod.SelectedItem = optimizer1;
            }
            if (this.cmbMethod.SelectedItem == null)
            {
                IEnumerator enumerator = this.cmbMethod.Items.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            Optimizer current = (Optimizer)enumerator.Current;
                            if (current.FriendlyName == "Exhaustive")
                            {
                                this.cmbMethod.SelectedItem = current;
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
                if (this.cmbMethod.SelectedItem == null && this.cmbMethod.Items.Count > 0)
                {
                    this.cmbMethod.SelectedItem = this.cmbMethod.Items[0];
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            PositionSize size = new PositionSize();
            this.tabOptimize = new TabControl();
            this.pageControl = new TabPage();
            this.grpOptimize = new GroupBox();
            this.lblTimeRemaining = new Label();
            this.lblRemaining = new Label();
            this.lblTimeElapsed = new Label();
            this.lblElaped = new Label();
            this.btnCancel = new Button();
            this.progOpt = new ProgressBar();
            this.btnBegin = new Button();
            this.lblEstimate = new Label();
            this.btnEstimate = new Button();
            this.grpMethod = new GroupBox();
            this.lblRunsRequired = new Label();
            this.btnSettings = new Button();
            this.lblDescription = new Label();
            this.cmbMethod = new ComboBox();
            this.lblMethod = new Label();
            this.grpParameters = new GroupBox();
            this.lblRuleBased = new Label();
            this.btnRollback = new Button();
            this.btnApplyChanges = new Button();
            this.btnRemoveParam = new Button();
            this.btnAddParameter = new Button();
            this.grpEdit = new GroupBox();
            this.txtName = new TextBox();
            this.lnlName = new Label();
            this.numStep = new NumEdit();
            this.lblStep = new Label();
            this.numStop = new NumEdit();
            this.lblStop = new Label();
            this.numStart = new NumEdit();
            this.numDefault = new NumEdit();
            this.lblStart = new Label();
            this.lblDefault = new Label();
            this.lvParameters = new ListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.pageResults = new TabPage();
            this.lvResults = new SortableListView();
            this.columnHeader_5 = new ColumnHeader();
            this.popupResults = new ContextMenuStrip(this.components);
            this.mniCopyResults = new ToolStripMenuItem();
            this.mniSaveResults = new ToolStripMenuItem();
            this.mniLoadResults = new ToolStripMenuItem();
            this.mniSetDefault = new ToolStripMenuItem();
            this.sepCopy = new ToolStripSeparator();
            this.mniPVHighest = new ToolStripMenuItem();
            this.mniPVLowest = new ToolStripMenuItem();
            this.toolStripSeparator_0 = new ToolStripSeparator();
            this.mniPVAvgHighest = new ToolStripMenuItem();
            this.mniPVAvgLowest = new ToolStripMenuItem();
            this.sep2 = new ToolStripSeparator();
            this.mniPVThisRow = new ToolStripMenuItem();
            this.mniPVAllThisRow = new ToolStripMenuItem();
            this.sep3 = new ToolStripSeparator();
            this.mniPrintResults = new ToolStripMenuItem();
            this.pnlResults = new Panel();
            this.cbAverage = new CheckBox();
            this.cmbMetric = new ComboBox();
            this.lblMetric = new Label();
            this.cmbScorecard = new ComboBox();
            this.lblScorecard = new Label();
            this.pageOptErrors = new TabPage();
            this.lvErrors = new ListView();
            this.columnHeader_6 = new ColumnHeader();
            this.columnHeader_7 = new ColumnHeader();
            this.columnHeader_8 = new ColumnHeader();
            this.assemblyLoader_0 = new AssemblyLoader(this.components);
            this.saveFileDialog_0 = new SaveFileDialog();
            this.openFileDialog_0 = new OpenFileDialog();
            this.tradingSystemExecutor_0 = new TradingSystemExecutor(this.components);
            this.tabOptimize.SuspendLayout();
            this.pageControl.SuspendLayout();
            this.grpOptimize.SuspendLayout();
            this.grpMethod.SuspendLayout();
            this.grpParameters.SuspendLayout();
            this.grpEdit.SuspendLayout();
            this.pageResults.SuspendLayout();
            this.popupResults.SuspendLayout();
            this.pnlResults.SuspendLayout();
            this.pageOptErrors.SuspendLayout();
            base.SuspendLayout();
            this.tabOptimize.Controls.Add(this.pageControl);
            this.tabOptimize.Controls.Add(this.pageResults);
            this.tabOptimize.Controls.Add(this.pageOptErrors);
            this.tabOptimize.Dock = DockStyle.Fill;
            this.tabOptimize.Location = new Point(0, 0);
            this.tabOptimize.Name = "tabOptimize";
            this.tabOptimize.SelectedIndex = 0;
            this.tabOptimize.Size = new Size(0x2ab, 0x1b7);
            this.tabOptimize.TabIndex = 0;
            this.pageControl.Controls.Add(this.grpOptimize);
            this.pageControl.Controls.Add(this.grpMethod);
            this.pageControl.Controls.Add(this.grpParameters);
            this.pageControl.Location = new Point(4, 0x16);
            this.pageControl.Name = "pageControl";
            this.pageControl.Padding = new Padding(3);
            this.pageControl.Size = new Size(0x2a3, 0x19d);
            this.pageControl.TabIndex = 0;
            this.pageControl.Text = "Optimization Control";
            this.pageControl.UseVisualStyleBackColor = true;
            this.grpOptimize.Controls.Add(this.lblTimeRemaining);
            this.grpOptimize.Controls.Add(this.lblRemaining);
            this.grpOptimize.Controls.Add(this.lblTimeElapsed);
            this.grpOptimize.Controls.Add(this.lblElaped);
            this.grpOptimize.Controls.Add(this.btnCancel);
            this.grpOptimize.Controls.Add(this.progOpt);
            this.grpOptimize.Controls.Add(this.btnBegin);
            this.grpOptimize.Controls.Add(this.lblEstimate);
            this.grpOptimize.Controls.Add(this.btnEstimate);
            this.grpOptimize.Location = new Point(0x119, 0xfc);
            this.grpOptimize.Name = "grpOptimize";
            this.grpOptimize.Size = new Size(0x17d, 0x97);
            this.grpOptimize.TabIndex = 2;
            this.grpOptimize.TabStop = false;
            this.grpOptimize.Text = "Optimize";
            this.lblTimeRemaining.AutoSize = true;
            this.lblTimeRemaining.ForeColor = Color.Navy;
            this.lblTimeRemaining.Location = new Point(0xf2, 130);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new Size(0x53, 13);
            this.lblTimeRemaining.TabIndex = 8;
            this.lblTimeRemaining.Text = "Time Remaining";
            this.lblTimeRemaining.Visible = false;
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Location = new Point(0xb0, 130);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new Size(60, 13);
            this.lblRemaining.TabIndex = 7;
            this.lblRemaining.Text = "Remaining:";
            this.lblRemaining.Visible = false;
            this.lblTimeElapsed.AutoSize = true;
            this.lblTimeElapsed.ForeColor = Color.Navy;
            this.lblTimeElapsed.Location = new Point(0x3d, 130);
            this.lblTimeElapsed.Name = "lblTimeElapsed";
            this.lblTimeElapsed.Size = new Size(0x47, 13);
            this.lblTimeElapsed.TabIndex = 6;
            this.lblTimeElapsed.Text = "Time Elapsed";
            this.lblTimeElapsed.Visible = false;
            this.lblElaped.AutoSize = true;
            this.lblElaped.Location = new Point(7, 130);
            this.lblElaped.Name = "lblElaped";
            this.lblElaped.Size = new Size(0x30, 13);
            this.lblElaped.TabIndex = 5;
            this.lblElaped.Text = "Elapsed:";
            this.lblElaped.Visible = false;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new Point(0x7f, 0x45);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x7c, 0x17);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel Optimization";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.progOpt.Location = new Point(6, 0x62);
            this.progOpt.Name = "progOpt";
            this.progOpt.Size = new Size(0x171, 0x17);
            this.progOpt.TabIndex = 3;
            this.btnBegin.Location = new Point(6, 0x45);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new Size(0x73, 0x17);
            this.btnBegin.TabIndex = 2;
            this.btnBegin.Text = "Begin Optimization";
            this.btnBegin.UseVisualStyleBackColor = true;
            this.btnBegin.Click += new EventHandler(this.btnBegin_Click);
            this.lblEstimate.AutoSize = true;
            this.lblEstimate.ForeColor = Color.Navy;
            this.lblEstimate.Location = new Point(0x5c, 30);
            this.lblEstimate.Name = "lblEstimate";
            this.lblEstimate.Size = new Size(0, 13);
            this.lblEstimate.TabIndex = 1;
            this.btnEstimate.Location = new Point(6, 0x19);
            this.btnEstimate.Name = "btnEstimate";
            this.btnEstimate.Size = new Size(0x4b, 0x17);
            this.btnEstimate.TabIndex = 0;
            this.btnEstimate.Text = "Estimate";
            this.btnEstimate.UseVisualStyleBackColor = true;
            this.btnEstimate.Click += new EventHandler(this.btnEstimate_Click);
            this.grpMethod.Controls.Add(this.lblRunsRequired);
            this.grpMethod.Controls.Add(this.btnSettings);
            this.grpMethod.Controls.Add(this.lblDescription);
            this.grpMethod.Controls.Add(this.cmbMethod);
            this.grpMethod.Controls.Add(this.lblMethod);
            this.grpMethod.Location = new Point(6, 0xfc);
            this.grpMethod.Name = "grpMethod";
            this.grpMethod.Size = new Size(0x10d, 0x97);
            this.grpMethod.TabIndex = 1;
            this.grpMethod.TabStop = false;
            this.grpMethod.Text = "Optimization Method";
            this.lblRunsRequired.AutoSize = true;
            this.lblRunsRequired.Location = new Point(9, 0x7d);
            this.lblRunsRequired.Name = "lblRunsRequired";
            this.lblRunsRequired.Size = new Size(90, 13);
            this.lblRunsRequired.TabIndex = 4;
            this.lblRunsRequired.Text = "Runs Required: 0";
            this.btnSettings.Enabled = false;
            this.btnSettings.Location = new Point(0x9a, 120);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new Size(0x68, 0x17);
            this.btnSettings.TabIndex = 3;
            this.btnSettings.Text = "Settings ...";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new EventHandler(this.btnSettings_Click);
            this.lblDescription.Location = new Point(9, 0x45);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(0xf9, 0x30);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description of Optimization Method.";
            this.cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Location = new Point(9, 0x29);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new Size(0xf9, 0x15);
            this.cmbMethod.TabIndex = 1;
            this.cmbMethod.SelectedIndexChanged += new EventHandler(this.cmbMethod_SelectedIndexChanged);
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new Point(6, 0x19);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new Size(0x9a, 13);
            this.lblMethod.TabIndex = 0;
            this.lblMethod.Text = "Select an Optimization Method:";
            this.grpParameters.Controls.Add(this.lblRuleBased);
            this.grpParameters.Controls.Add(this.btnRollback);
            this.grpParameters.Controls.Add(this.btnApplyChanges);
            this.grpParameters.Controls.Add(this.btnRemoveParam);
            this.grpParameters.Controls.Add(this.btnAddParameter);
            this.grpParameters.Controls.Add(this.grpEdit);
            this.grpParameters.Controls.Add(this.lvParameters);
            this.grpParameters.Location = new Point(6, 6);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new Size(0x290, 240);
            this.grpParameters.TabIndex = 0;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Strategy Parameters";
            this.lblRuleBased.AutoSize = true;
            this.lblRuleBased.Location = new Point(6, 0xd5);
            this.lblRuleBased.Name = "lblRuleBased";
            this.lblRuleBased.Size = new Size(0x169, 13);
            this.lblRuleBased.TabIndex = 6;
            this.lblRuleBased.Text = "This is a Rule-Based Strategy and the Parameters cannot be modified here.";
            this.btnRollback.Enabled = false;
            this.btnRollback.Location = new Point(0x1c3, 0xd0);
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.Size = new Size(0xb5, 0x17);
            this.btnRollback.TabIndex = 5;
            this.btnRollback.Text = "Rollback Changes";
            this.btnRollback.UseVisualStyleBackColor = true;
            this.btnRollback.Click += new EventHandler(this.btnRollback_Click);
            this.btnApplyChanges.Enabled = false;
            this.btnApplyChanges.Location = new Point(0x1c3, 0xb3);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new Size(0xb5, 0x17);
            this.btnApplyChanges.TabIndex = 4;
            this.btnApplyChanges.Text = "Apply Changes to Code";
            this.btnApplyChanges.UseVisualStyleBackColor = true;
            this.btnApplyChanges.Click += new EventHandler(this.btnApplyChanges_Click);
            this.btnRemoveParam.Location = new Point(0x9a, 0xd0);
            this.btnRemoveParam.Name = "btnRemoveParam";
            this.btnRemoveParam.Size = new Size(0xc6, 0x17);
            this.btnRemoveParam.TabIndex = 3;
            this.btnRemoveParam.Text = "Remove Selected Parameter";
            this.btnRemoveParam.UseVisualStyleBackColor = true;
            this.btnRemoveParam.Click += new EventHandler(this.btnRemoveParam_Click);
            this.btnAddParameter.Location = new Point(6, 0xd0);
            this.btnAddParameter.Name = "btnAddParameter";
            this.btnAddParameter.Size = new Size(0x8e, 0x17);
            this.btnAddParameter.TabIndex = 2;
            this.btnAddParameter.Text = "Add New Parameter";
            this.btnAddParameter.UseVisualStyleBackColor = true;
            this.btnAddParameter.Click += new EventHandler(this.btnAddParameter_Click);
            this.grpEdit.Controls.Add(this.txtName);
            this.grpEdit.Controls.Add(this.lnlName);
            this.grpEdit.Controls.Add(this.numStep);
            this.grpEdit.Controls.Add(this.lblStep);
            this.grpEdit.Controls.Add(this.numStop);
            this.grpEdit.Controls.Add(this.lblStop);
            this.grpEdit.Controls.Add(this.numStart);
            this.grpEdit.Controls.Add(this.numDefault);
            this.grpEdit.Controls.Add(this.lblStart);
            this.grpEdit.Controls.Add(this.lblDefault);
            this.grpEdit.Location = new Point(0x1c3, 0x13);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.Size = new Size(0xb5, 0x9a);
            this.grpEdit.TabIndex = 1;
            this.grpEdit.TabStop = false;
            this.grpEdit.Text = "Edit Selected Parameter";
            this.txtName.Enabled = false;
            this.txtName.Location = new Point(0x4b, 0x15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(100, 20);
            this.txtName.TabIndex = 9;
            this.txtName.TextChanged += new EventHandler(this.txtName_TextChanged);
            this.lnlName.AutoSize = true;
            this.lnlName.Location = new Point(7, 0x18);
            this.lnlName.Name = "lnlName";
            this.lnlName.Size = new Size(0x26, 13);
            this.lnlName.TabIndex = 8;
            this.lnlName.Text = "Name:";
            this.numStep.Enabled = false;
            this.numStep.InputType = NumEdit.NumEditType.Double;
            this.numStep.Location = new Point(0x5e, 0x7d);
            this.numStep.Name = "numStep";
            this.numStep.Size = new Size(0x51, 20);
            this.numStep.TabIndex = 7;
            this.numStep.TextChanged += new EventHandler(this.numStep_TextChanged);
            this.numStep.Leave += new EventHandler(this.numDefault_Leave);
            this.lblStep.AutoSize = true;
            this.lblStep.Location = new Point(7, 0x80);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new Size(0x20, 13);
            this.lblStep.TabIndex = 6;
            this.lblStep.Text = "Step:";
            this.numStop.Enabled = false;
            this.numStop.InputType = NumEdit.NumEditType.Double;
            this.numStop.Location = new Point(0x5e, 0x63);
            this.numStop.Name = "numStop";
            this.numStop.Size = new Size(0x51, 20);
            this.numStop.TabIndex = 5;
            this.numStop.TextChanged += new EventHandler(this.numStop_TextChanged);
            this.numStop.Leave += new EventHandler(this.numDefault_Leave);
            this.lblStop.AutoSize = true;
            this.lblStop.Location = new Point(7, 0x66);
            this.lblStop.Name = "lblStop";
            this.lblStop.Size = new Size(0x20, 13);
            this.lblStop.TabIndex = 4;
            this.lblStop.Text = "Stop:";
            this.numStart.Enabled = false;
            this.numStart.InputType = NumEdit.NumEditType.Double;
            this.numStart.Location = new Point(0x5e, 0x49);
            this.numStart.Name = "numStart";
            this.numStart.Size = new Size(0x51, 20);
            this.numStart.TabIndex = 3;
            this.numStart.TextChanged += new EventHandler(this.numStart_TextChanged);
            this.numStart.Leave += new EventHandler(this.numDefault_Leave);
            this.numDefault.Enabled = false;
            this.numDefault.InputType = NumEdit.NumEditType.Double;
            this.numDefault.Location = new Point(0x5e, 0x2f);
            this.numDefault.Name = "numDefault";
            this.numDefault.Size = new Size(0x51, 20);
            this.numDefault.TabIndex = 2;
            this.numDefault.TextChanged += new EventHandler(this.numDefault_TextChanged);
            this.numDefault.Leave += new EventHandler(this.numDefault_Leave);
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new Point(7, 0x4c);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new Size(0x20, 13);
            this.lblStart.TabIndex = 1;
            this.lblStart.Text = "Start:";
            this.lblDefault.AutoSize = true;
            this.lblDefault.Location = new Point(7, 50);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Size = new Size(0x2c, 13);
            this.lblDefault.TabIndex = 0;
            this.lblDefault.Text = "Default:";
            this.lvParameters.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4 });
            this.lvParameters.FullRowSelect = true;
            this.lvParameters.HideSelection = false;
            this.lvParameters.Location = new Point(6, 0x13);
            this.lvParameters.MultiSelect = false;
            this.lvParameters.Name = "lvParameters";
            this.lvParameters.Size = new Size(0x1b6, 0xb7);
            this.lvParameters.TabIndex = 0;
            this.lvParameters.UseCompatibleStateImageBehavior = false;
            this.lvParameters.View = View.Details;
            this.lvParameters.SelectedIndexChanged += new EventHandler(this.lvParameters_SelectedIndexChanged);
            this.columnHeader_0.Text = "Parameter";
            this.columnHeader_0.Width = 120;
            this.columnHeader_1.Text = "Default";
            this.columnHeader_1.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_1.Width = 70;
            this.columnHeader_2.Text = "Start";
            this.columnHeader_2.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_2.Width = 70;
            this.columnHeader_3.Text = "Stop";
            this.columnHeader_3.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_3.Width = 70;
            this.columnHeader_4.Text = "Step";
            this.columnHeader_4.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_4.Width = 70;
            this.pageResults.Controls.Add(this.lvResults);
            this.pageResults.Controls.Add(this.pnlResults);
            this.pageResults.Location = new Point(4, 0x16);
            this.pageResults.Name = "pageResults";
            this.pageResults.Padding = new Padding(3);
            this.pageResults.Size = new Size(0x2a3, 0x19d);
            this.pageResults.TabIndex = 1;
            this.pageResults.Text = "Results";
            this.pageResults.UseVisualStyleBackColor = true;
            this.lvResults.Columns.AddRange(new ColumnHeader[] { this.columnHeader_5 });
            this.lvResults.ContextMenuStrip = this.popupResults;
            this.lvResults.Dock = DockStyle.Fill;
            this.lvResults.FullRowSelect = true;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new Point(3, 0x71);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new Size(0x29d, 0x129);
            this.lvResults.TabIndex = 2;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = View.Details;
            this.lvResults.DoubleClick += new EventHandler(this.lvResults_DoubleClick);
            this.columnHeader_5.Text = "Parameters";
            this.columnHeader_5.Width = 120;
            this.popupResults.Items.AddRange(new ToolStripItem[] { this.mniCopyResults, this.mniSaveResults, this.mniLoadResults, this.mniSetDefault, this.sepCopy, this.mniPVHighest, this.mniPVLowest, this.toolStripSeparator_0, this.mniPVAvgHighest, this.mniPVAvgLowest, this.sep2, this.mniPVThisRow, this.mniPVAllThisRow, this.sep3, this.mniPrintResults });
            this.popupResults.Name = "popupResults";
            this.popupResults.Size = new Size(0x1e3, 270);
            this.popupResults.Opening += new CancelEventHandler(this.popupResults_Opening);
            this.mniCopyResults.Name = "mniCopyResults";
            this.mniCopyResults.Size = new Size(0x1e2, 0x16);
            this.mniCopyResults.Text = "Copy to Clipboard";
            this.mniCopyResults.Click += new EventHandler(this.mniCopyResults_Click);
            this.mniSaveResults.Name = "mniSaveResults";
            this.mniSaveResults.Size = new Size(0x1e2, 0x16);
            this.mniSaveResults.Text = "Save to File ...";
            this.mniSaveResults.Click += new EventHandler(this.mniSaveResults_Click);
            this.mniLoadResults.Name = "mniLoadResults";
            this.mniLoadResults.Size = new Size(0x1e2, 0x16);
            this.mniLoadResults.Text = "Load from File ...";
            this.mniLoadResults.Click += new EventHandler(this.mniLoadResults_Click);
            this.mniSetDefault.Name = "mniSetDefault";
            this.mniSetDefault.Size = new Size(0x1e2, 0x16);
            this.mniSetDefault.Text = "Set these Parameter Values as Default for the Strategy";
            this.mniSetDefault.Click += new EventHandler(this.mniSetDefault_Click);
            this.sepCopy.Name = "sepCopy";
            this.sepCopy.Size = new Size(0x1df, 6);
            this.mniPVHighest.Name = "mniPVHighest";
            this.mniPVHighest.Size = new Size(0x1e2, 0x16);
            this.mniPVHighest.Text = "Assign Preferred Values based on the highest metric value per symbol.";
            this.mniPVHighest.Click += new EventHandler(this.mniPVHighest_Click);
            this.mniPVLowest.Name = "mniPVLowest";
            this.mniPVLowest.Size = new Size(0x1e2, 0x16);
            this.mniPVLowest.Text = "Assign Preferred Values based on the lowest metric value per symbol.";
            this.mniPVLowest.Click += new EventHandler(this.mniPVLowest_Click);
            this.toolStripSeparator_0.Name = "sep";
            this.toolStripSeparator_0.Size = new Size(0x1df, 6);
            this.mniPVAvgHighest.Name = "mniPVAvgHighest";
            this.mniPVAvgHighest.Size = new Size(0x1e2, 0x16);
            this.mniPVAvgHighest.Text = "Assign Preferred values based on the highest average metric value for all symbols.";
            this.mniPVAvgHighest.TextAlign = ContentAlignment.MiddleRight;
            this.mniPVAvgHighest.Click += new EventHandler(this.mniPVAvgHighest_Click);
            this.mniPVAvgLowest.Name = "mniPVAvgLowest";
            this.mniPVAvgLowest.Size = new Size(0x1e2, 0x16);
            this.mniPVAvgLowest.Text = "Assign Preferred values based on the lowest average metric value for all symbols.";
            this.mniPVAvgLowest.Click += new EventHandler(this.mniPVAvgLowest_Click);
            this.sep2.Name = "sep2";
            this.sep2.Size = new Size(0x1df, 6);
            this.mniPVThisRow.Name = "mniPVThisRow";
            this.mniPVThisRow.Size = new Size(0x1e2, 0x16);
            this.mniPVThisRow.Text = "Assign Preferred Values to the selected Symbol from this Row";
            this.mniPVThisRow.Click += new EventHandler(this.mniPVThisRow_Click);
            this.mniPVAllThisRow.Name = "mniPVAllThisRow";
            this.mniPVAllThisRow.Size = new Size(0x1e2, 0x16);
            this.mniPVAllThisRow.Text = "Assign Preferred Values to all Symbols from this Row";
            this.mniPVAllThisRow.Click += new EventHandler(this.mniPVAllThisRow_Click);
            this.sep3.Name = "sep3";
            this.sep3.Size = new Size(0x1df, 6);
            this.mniPrintResults.Name = "mniPrintResults";
            this.mniPrintResults.Size = new Size(0x1e2, 0x16);
            this.mniPrintResults.Text = "Print";
            this.mniPrintResults.Click += new EventHandler(this.mniPrintResults_Click);
            this.pnlResults.Controls.Add(this.cbAverage);
            this.pnlResults.Controls.Add(this.cmbMetric);
            this.pnlResults.Controls.Add(this.lblMetric);
            this.pnlResults.Controls.Add(this.cmbScorecard);
            this.pnlResults.Controls.Add(this.lblScorecard);
            this.pnlResults.Dock = DockStyle.Top;
            this.pnlResults.Location = new Point(3, 3);
            this.pnlResults.Name = "pnlResults";
            this.pnlResults.Size = new Size(0x29d, 110);
            this.pnlResults.TabIndex = 1;
            this.cbAverage.AutoSize = true;
            this.cbAverage.Location = new Point(7, 0x55);
            this.cbAverage.Name = "cbAverage";
            this.cbAverage.Size = new Size(0xda, 0x11);
            this.cbAverage.TabIndex = 4;
            this.cbAverage.Text = "View the Average Results for all Symbols";
            this.cbAverage.UseVisualStyleBackColor = true;
            this.cbAverage.Visible = false;
            this.cbAverage.CheckedChanged += new EventHandler(this.cbAverage_CheckedChanged);
            this.cmbMetric.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMetric.FormattingEnabled = true;
            this.cmbMetric.Location = new Point(280, 0x39);
            this.cmbMetric.Name = "cmbMetric";
            this.cmbMetric.Size = new Size(0x115, 0x15);
            this.cmbMetric.TabIndex = 3;
            this.lblMetric.Location = new Point(0x115, 8);
            this.lblMetric.Name = "lblMetric";
            this.lblMetric.Size = new Size(0x14b, 0x2e);
            this.lblMetric.TabIndex = 2;
            this.lblMetric.Text = "When you right click the Results grid, you can assign Preferred Values based on the highest and lowest values for a specific metric.  Select here the metric to use for assigning Preferred Values.";
            this.cmbScorecard.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbScorecard.FormattingEnabled = true;
            this.cmbScorecard.Location = new Point(7, 0x39);
            this.cmbScorecard.Name = "cmbScorecard";
            this.cmbScorecard.Size = new Size(0x108, 0x15);
            this.cmbScorecard.TabIndex = 1;
            this.cmbScorecard.SelectedIndexChanged += new EventHandler(this.cmbScorecard_SelectedIndexChanged);
            this.lblScorecard.Location = new Point(4, 8);
            this.lblScorecard.Name = "lblScorecard";
            this.lblScorecard.Size = new Size(0x10b, 0x2e);
            this.lblScorecard.TabIndex = 0;
            this.lblScorecard.Text = "Select a Strategy Scorecard that defines the Optimization Results you want to see.  The change will take effect the next time you Begin an Optimization.";
            this.pageOptErrors.Controls.Add(this.lvErrors);
            this.pageOptErrors.Location = new Point(4, 0x16);
            this.pageOptErrors.Name = "pageOptErrors";
            this.pageOptErrors.Padding = new Padding(3);
            this.pageOptErrors.Size = new Size(0x2a3, 0x19d);
            this.pageOptErrors.TabIndex = 2;
            this.pageOptErrors.Text = "Errors";
            this.pageOptErrors.UseVisualStyleBackColor = true;
            this.lvErrors.Columns.AddRange(new ColumnHeader[] { this.columnHeader_6, this.columnHeader_7, this.columnHeader_8 });
            this.lvErrors.Dock = DockStyle.Fill;
            this.lvErrors.FullRowSelect = true;
            this.lvErrors.Location = new Point(3, 3);
            this.lvErrors.Name = "lvErrors";
            this.lvErrors.Size = new Size(0x29d, 0x197);
            this.lvErrors.TabIndex = 0;
            this.lvErrors.UseCompatibleStateImageBehavior = false;
            this.lvErrors.View = View.Details;
            this.columnHeader_6.Text = "Symbol";
            this.columnHeader_7.Text = "Parameters";
            this.columnHeader_7.Width = 80;
            this.columnHeader_8.Text = "Error";
            this.columnHeader_8.Width = 0x1ec;
            this.assemblyLoader_0.BaseClass = "StrategyScorecard";
            this.assemblyLoader_0.DLLNameFilter = "";
            this.assemblyLoader_0.Interface = null;
            this.assemblyLoader_0.Path = null;
            this.assemblyLoader_0.PathMask = "*.dll";
            this.saveFileDialog_0.DefaultExt = "xml";
            this.saveFileDialog_0.Filter = "XML Files(*.xml)|*.xml";
            this.saveFileDialog_0.Title = "Save Optimization Results";
            this.openFileDialog_0.FileName = "openFileDialog1";
            this.openFileDialog_0.Filter = "XML Files(*.xml)|*.xml";
            this.openFileDialog_0.Title = "Open Saved Optimization Results";
            this.tradingSystemExecutor_0.ApplyCommission = false;
            this.tradingSystemExecutor_0.ApplyDividends = false;
            this.tradingSystemExecutor_0.ApplyInterest = false;
            this.tradingSystemExecutor_0.BarsLoader = null;
            this.tradingSystemExecutor_0.BenchmarkBuyAndHoldON = false;
            this.tradingSystemExecutor_0.BenchmarkSymbol = null;
            this.tradingSystemExecutor_0.BuildEquityCurves = true;
            this.tradingSystemExecutor_0.CashRate = 0.0;
            this.tradingSystemExecutor_0.EnableSlippage = false;
            this.tradingSystemExecutor_0.ExceptionEvents = false;
            this.tradingSystemExecutor_0.FundamentalsLoader = null;
            this.tradingSystemExecutor_0.IsStreaming = false;
            this.tradingSystemExecutor_0.LimitDaySimulation = false;
            this.tradingSystemExecutor_0.LimitOrderSlippage = false;
            this.tradingSystemExecutor_0.MarginRate = 0.0;
            this.tradingSystemExecutor_0.NoDecimalRoundingForLimitStopPrice = false;
            this.tradingSystemExecutor_0.OverrideShareSize = 0.0;
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
            size.StartingCapital = 100000.0;
            this.tradingSystemExecutor_0.PosSize = size;
            this.tradingSystemExecutor_0.PricingDecimalPlaces = 0;
            this.tradingSystemExecutor_0.RedcuceQtyPct = 10.0;
            this.tradingSystemExecutor_0.ReduceQtyBasedOnVolume = false;
            this.tradingSystemExecutor_0.Renderer = null;
            this.tradingSystemExecutor_0.RoundLots = false;
            this.tradingSystemExecutor_0.RoundLots50 = false;
            this.tradingSystemExecutor_0.SlippageTicks = 1;
            this.tradingSystemExecutor_0.SlippageUnits = 1.0;
            this.tradingSystemExecutor_0.StrategyName = "";
            this.tradingSystemExecutor_0.WorstTradeSimulation = false;
            this.tradingSystemExecutor_0.ExternalSymbolFromDataSetRequested += new EventHandler<LoadSymbolFromDataSetEventArgs>(this.externalSymbolFromDataSetRequestedEventHandler);
            this.tradingSystemExecutor_0.ExternalSymbolRequested += new EventHandler<LoadSymbolEventArgs>(this.externalSymbolRequestedEventHandler);
            this.tradingSystemExecutor_0.TrendlineGetValue += new EventHandler<TrendLineEventArgs>(this.trendLineGetValueEventHandler);
            this.tradingSystemExecutor_0.LookupStrategy += new EventHandler<StrategyEventArgs>(this.method_22);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(this.tabOptimize);
            base.Name = "Optimization";
            base.Size = new Size(0x2ab, 0x1b7);
            base.Load += new EventHandler(this.Optimization_Load);
            this.tabOptimize.ResumeLayout(false);
            this.pageControl.ResumeLayout(false);
            this.grpOptimize.ResumeLayout(false);
            this.grpOptimize.PerformLayout();
            this.grpMethod.ResumeLayout(false);
            this.grpMethod.PerformLayout();
            this.grpParameters.ResumeLayout(false);
            this.grpParameters.PerformLayout();
            this.grpEdit.ResumeLayout(false);
            this.grpEdit.PerformLayout();
            this.pageResults.ResumeLayout(false);
            this.popupResults.ResumeLayout(false);
            this.pnlResults.ResumeLayout(false);
            this.pnlResults.PerformLayout();
            this.pageOptErrors.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void LoadParameterList()
        {
            this.lvParameters.Items.Clear();
            if (this.WealthScript != null)
            {
                foreach (StrategyParameter parameter in this.WealthScript.Parameters)
                {
                    this.method_0(parameter);
                }
            }
        }

        private void lvParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = this.lvParameters.SelectedItems.Count == 1;
            this.txtName.Enabled = flag;
            this.numDefault.Enabled = flag;
            this.numStart.Enabled = flag;
            this.numStop.Enabled = flag;
            this.numStep.Enabled = flag;
            if (flag)
            {
                StrategyParameter tag = (StrategyParameter) this.lvParameters.SelectedItems[0].Tag;
                this.bool_3 = true;
                this.txtName.Text = tag.NameEdited;
                this.numDefault.Text = tag.DefaultValue.ToString();
                this.numStart.Text = tag.Start.ToString();
                this.numStop.Text = tag.Stop.ToString();
                this.numStep.Text = tag.Step.ToString();
                this.bool_3 = false;
            }
        }

        private void lvResults_DoubleClick(object sender, EventArgs e)
        {
            if (this.lvResults.SelectedItems.Count == 1)
            {
                ListViewItem item = this.lvResults.SelectedItems[0];
                OptimizationResult tag = item.Tag as OptimizationResult;
                if (tag.Symbol != "<Average>")
                {
                    for (int i = 0; i < this.WealthScript.Parameters.Count; i++)
                    {
                        this.WealthScript.Parameters[i].Value = tag.ParameterValues[i];
                    }
                    this.chartForm_0.IsOptimizing = true;
                    if (this.bool_0 && (tag.Symbol != this.chartForm_0.Symbol))
                    {
                        this.chartForm_0.MyMainForm.SelectTreeNode(this.dataSource_0, tag.Symbol);
                    }
                    else
                    {
                        this.chartForm_0.GoButtonPressed(this.mainForm_0.Symbol, true);
                    }
                    this.chartForm_0.IsOptimizing = false;
                    this.mainForm_0.BuildParameterSliders();
                    this.chartForm_0.ParametersNeedSave = true;
                    this.mainForm_0.EnableSliders();
                    this.chartForm_0.SelectChartTab();
                }
            }
        }

        private void method_0(StrategyParameter strategyParameter_0)
        {
            ListViewItem item = this.lvParameters.Items.Add(strategyParameter_0.Name);
            item.Tag = strategyParameter_0;
            item.SubItems.Add(strategyParameter_0.DefaultValue.ToString());
            item.SubItems.Add(strategyParameter_0.Start.ToString());
            item.SubItems.Add(strategyParameter_0.Stop.ToString());
            item.SubItems.Add(strategyParameter_0.Step.ToString());
        }

        private string method_1(string string_1)
        {
            return (this.optimizer_0.FriendlyName + "." + string_1);
        }

        private string method_10()
        {
            string str = "";
            int num = 1;
            do
            {
                str = "Parameter " + num;
                num++;
            }
            while (this.method_11(str));
            return str;
        }

        private bool method_11(string string_1)
        {
            foreach (ListViewItem item in this.lvParameters.Items)
            {
                if (item.Text != string_1)
                {
                    continue;
                }
                bool flag = true;
                return flag;
            }
            return false;
        }

        ///WYJ fix, original signature: private void method_12()
        private void updateEditorForParameters()
        {
            try
            {
                this.chartForm_0.updateEditorForParameters();
                this.chartForm_0.NeedSave = true;
                this.chartForm_0.MyMainForm.BuildParameterSliders();
                MessageBox.Show("Your changes have been applied to the Editor Code.");
            }
            catch (Exception exception)
            {
                MessageBox.Show("There was a problem applying the changes to the Editor Code: " + exception.Message);
            }
        }

        private void method_13()
        {
            this.method_2();
            this.dateTime_1 = DateTime.Now;
            this.method_2();
            this.tradingSystemExecutor_0.ApplySettings(MainModule.Instance.Executor);
            this.tradingSystemExecutor_0.PosSize = this.chartForm_0.posSize.PositionSize;
            try
            {
                this.tradingSystemExecutor_0.DataSet = this.dataSource_0;
                this.tradingSystemExecutor_0.Execute(this.optimizer_0.Strategy, this.WealthScript, null, this.list_0);
            }
            catch
            {
            }
            foreach (Bars bars in this.list_0)
            {
                bars.Cache.Clear();
            }
            base.Invoke(new Delegate63(this.method_14));
        }

        private void method_14()
        {
            this.btnEstimate.Enabled = true;
            this.btnBegin.Enabled = true;
            this.dateTime_2 = DateTime.Now;
            TimeSpan span = (TimeSpan) (this.dateTime_2 - this.dateTime_1);
            double numberOfRuns = this.optimizer_0.NumberOfRuns;
            long ticks = span.Ticks * ((long) numberOfRuns);
            ticks *= 2L;
            span = new TimeSpan(ticks);
            if (numberOfRuns < 1000000.0)
            {
                this.lblEstimate.Text = this.method_15(span);
            }
            else
            {
                this.lblEstimate.Text = "Too many Runs";
            }
        }

        private string method_15(TimeSpan timeSpan_0)
        {
            string str = "";
            if (timeSpan_0.TotalDays >= 1.0)
            {
                str = timeSpan_0.TotalDays.ToString("N0") + " days ";
            }
            if ((timeSpan_0.TotalDays >= 1.0) || (timeSpan_0.Hours > 0))
            {
                str = str + timeSpan_0.Hours + " hours ";
            }
            if ((timeSpan_0.TotalHours > 0.0) || (timeSpan_0.Minutes > 0))
            {
                str = str + timeSpan_0.Minutes + " min ";
            }
            return (str + timeSpan_0.Seconds + " sec");
        }

        private void method_16(double double_0)
        {
            string text = this.cmbMetric.Text;
            foreach (string str2 in this.optimizationResultList_0.Symbols)
            {
                List<double> values = this.optimizationResultList_0.FindBestMetric(str2, text, double_0);
                this.chartForm_0.Strategy.StorePreferredValues(str2, this.WealthScript, values);
            }
            this.chartForm_0.NeedSave = true;
            MessageBox.Show("Preferred Values set for " + this.optimizationResultList_0.Symbols.Count + " Symbol(s).");
            this.chartForm_0.PreferredValuesChanged();
        }

        private void method_17(double double_0)
        {
            OptimizationResultList averageResultList = this.optimizationResultList_0;
            if (!this.positionSize_0.RawProfitMode)
            {
                averageResultList = averageResultList.AverageResultList;
            }
            if (averageResultList.Results.Count != 0)
            {
                string symbol = averageResultList.Results[0].Symbol;
                List<double> values = averageResultList.FindBestMetric(symbol, this.cmbMetric.Text, double_0);
                int count = 1;
                if (this.bool_4)
                {
                    foreach (string str in this.dataSource_0.Symbols)
                    {
                        this.chartForm_0.Strategy.StorePreferredValues(str, this.WealthScript, values);
                    }
                    count = this.dataSource_0.Symbols.Count;
                }
                else
                {
                    this.chartForm_0.Strategy.StorePreferredValues(this.string_0, this.WealthScript, values);
                }
                this.chartForm_0.NeedSave = true;
                MessageBox.Show("Preferred Values set for " + count + " Symbol(s).");
                this.chartForm_0.PreferredValuesChanged();
            }
        }

        private void method_18(string string_1, ListView listView_0, Bitmap bitmap_0, bool bool_5)
        {
            DataObject reportTemplate = this.GetReportTemplate(string_1);
            if (listView_0 != null)
            {
                reportTemplate.SetData(PrintReport.fmtListView.Name, listView_0);
            }
            if (bitmap_0 != null)
            {
                reportTemplate.SetData(PrintReport.fmtGraphic.Name, bitmap_0);
            }
            new PrintReport(reportTemplate, bool_5) { ShowPrintPreview = this.ShowPrintPreview(), ShowPrintDialog = this.ShowPrintDialog() }.PrintGraphicReport(this._pageSettings);
        }

        ///WYJ fix, original signature: private void method_19(object sender, LoadSymbolEventArgs e)
        private void externalSymbolRequestedEventHandler(object sender, LoadSymbolEventArgs e)
        {
            this.chartForm_0.externalSymbolRequestedEventHandler(sender, e);
        }

        private void method_2()
        {
            this.chartForm_0.barRangeSelector.DataRange.ConfigureBarsLoader(this.tradingSystemExecutor_0.BarsLoader);
            this.tradingSystemExecutor_0.PosSize = this.chartForm_0.posSize.PositionSize;
            this.list_0.Clear();
            if (this.string_0 == "")
            {
                foreach (string str in this.dataSource_0.Symbols)
                {
                    this.method_3(str);
                }
            }
            else
            {
                this.method_3(this.string_0);
            }
        }

        ///WYJ fix, original signature: private void method_20(object sender, LoadSymbolFromDataSetEventArgs e)
        private void externalSymbolFromDataSetRequestedEventHandler(object sender, LoadSymbolFromDataSetEventArgs e)
        {
            this.chartForm_0.externalSymbolFromDataSetRequestedEventHandler(sender, e);
        }

        ///WYJ fix, original signature: private void method_21(object sender, TrendLineEventArgs e)
        private void trendLineGetValueEventHandler(object sender, TrendLineEventArgs e)
        {
            this.chartForm_0.trendLineGetValueEventHandler(sender, e);
        }

        private void method_22(object sender, StrategyEventArgs e)
        {
            e.Strategy = MainModule.Instance.Strategies.LookupID(e.StrategyID);
        }

        private void method_3(string string_1)
        {
            Bars data = this.chartForm_0.barsLoader.GetData(this.chartForm_0.DataSource, string_1);
            this.list_0.Add(data);
        }

        private void method_4()
        {
            this.list_2.Clear();
            StrategyScorecard selectedItem = this.cmbScorecard.SelectedItem as StrategyScorecard;
            IList<string> list = this.chartForm_0.posSize.PositionSize.RawProfitMode ? selectedItem.ColumnHeadersRawProfit : selectedItem.ColumnHeadersPortfolioSim;
            this.list_2.Clear();
            foreach (string str in list)
            {
                this.list_2.Add(str);
            }
        }

        private void method_5()
        {
            this.lvResults.BeginUpdate();
            this.lvResults.Items.Clear();
            this.lvResults.Columns.Clear();
            this.lvResults.Columns.Add("Symbol").Width = 60;
            foreach (StrategyParameter parameter in this.WealthScript.Parameters)
            {
                ColumnHeader header2 = this.lvResults.Columns.Add(parameter.Name);
                header2.Tag = "N";
                header2.Width = 70;
            }
            this.strategyScorecard_0.SetupListViewColumns(this.lvResults, this.chartForm_0.posSize.PositionSize, this.WealthScript.Parameters.Count + 1);
        }

        ///WYJ fix, code from Reflector, workable, but deprecated because of having too many goto statements. Try code from ILSpy.
        private void method_6()  ///WYJ note, probably the method to run optimization
        {
            this.method_2();
            this.tradingSystemExecutor_0.ApplySettings(MainModule.Instance.Executor);
            this.tradingSystemExecutor_0.PosSize = this.chartForm_0.posSize.PositionSize;
            if (!this.tradingSystemExecutor_0.PosSize.RawProfitMode)
            {
                this.bool_0 = false;
                this.optimizer_0.FirstRun();
            Label_01AE:
                while (!this.bool_1)
                {
                    this.exception_0 = null;
                    try
                    {
                        this.tradingSystemExecutor_0.DataSet = this.dataSource_0;
                        this.tradingSystemExecutor_0.Execute(this.optimizer_0.Strategy, this.WealthScript, null, this.list_0);
                    }
                    catch (Exception exception2)
                    {
                        this.exception_0 = exception2;
                    }
                    this.systemPerformance_0 = this.tradingSystemExecutor_0.Performance;
                    this.bool_2 = true;
                    try
                    {
                        base.Invoke(new Delegate63(this.method_7));
                        while (this.bool_2)
                        {
                            Thread.Sleep(10);
                        }
                    }
                    catch
                    {
                        this.bool_2 = false;
                    }
                    foreach (Bars bars in this.list_0)
                    {
                        bars.Cache.Clear();
                    }
                    if (this.optimizer_0.NextRun(this.systemPerformance_0, this.optimizationResultList_0.Results[this.optimizationResultList_0.Results.Count - 1]))
                    {
                        ///goto  Label_01AE;  ///WYJ fix, simplify the flow
                        continue;
                    }
                    else
                        break;
                } 
                ///goto  Label_02CA;
                base.Invoke(new Delegate63(this.method_8));
                return;
            }
            this.bool_0 = true;
            string str = this.string_0;
            using (List<Bars>.Enumerator enumerator = this.list_0.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                Label_0066:
                    Bars current = enumerator.Current;
                    if (this.bool_1)
                    {
                        ///goto  Label_0190;
                        break;
                    }
                    this.string_0 = current.Symbol;
                    this.optimizer_0.FirstRun();
                Label_009E:
                    while (!this.bool_1)
                    {
                        List<Bars> barsCollection = new List<Bars> {
                            current
                        };
                        this.exception_0 = null;
                        try
                        {
                            this.tradingSystemExecutor_0.DataSet = this.dataSource_0;
                            this.tradingSystemExecutor_0.Execute(this.optimizer_0.Strategy, this.WealthScript, null, barsCollection);
                        }
                        catch (Exception exception)
                        {
                            this.exception_0 = exception;
                        }
                        ///goto  Label_0153; ///WYJ fix, simplify the flow
                        this.systemPerformance_0 = this.tradingSystemExecutor_0.Performance;
                        this.bool_2 = true;
                        base.Invoke(new Delegate63(this.method_7));

                        while (this.bool_2)
                        {
                            Thread.Sleep(10);
                        }
                        current.Cache.Clear();
                        if (this.optimizer_0.NextRun(this.systemPerformance_0, this.optimizationResultList_0.Results[this.optimizationResultList_0.Results.Count - 1]))
                        {
                            ///goto  Label_009E;
                            continue;
                        }
                        else
                            break;
                    }
                    //goto  Label_0066;
                    continue;
                }
            }

        Label_0190:
            this.string_0 = str;
        Label_02CA:
            base.Invoke(new Delegate63(this.method_8));
        } 

        ///WYJ fix, code from ILSpy
        /*
        private void method_6()
        {
            this.method_2();
            this.tradingSystemExecutor_0.ApplySettings(MainModule.Instance.Executor);
            this.tradingSystemExecutor_0.PosSize = this.chartForm_0.posSize.PositionSize;
            if (this.tradingSystemExecutor_0.PosSize.RawProfitMode)
            {
                this.bool_0 = true;
                string text = this.string_0;
                foreach (Bars current in this.list_0)
                {
                    if (this.bool_1)
                    {
                        break;
                    }
                    this.string_0 = current.Symbol;
                    this.optimizer_0.FirstRun();
                    while (!this.bool_1)
                    {
                        List<Bars> list = new List<Bars>();
                        list.Add(current);
                        this.exception_0 = null;
                        try
                        {
                            this.tradingSystemExecutor_0.DataSet = this.dataSource_0;
                            this.tradingSystemExecutor_0.Execute(this.optimizer_0.Strategy, this.WealthScript, null, list);
                            goto IL_153;
                        }
                        catch (Exception ex)
                        {
                            this.exception_0 = ex;
                            goto IL_153;
                        }
                        goto IL_FB;
                    IL_102:
                        if (!this.bool_2)
                        {
                            current.Cache.Clear();
                            if (!this.optimizer_0.NextRun(this.systemPerformance_0, this.optimizationResultList_0.Results[this.optimizationResultList_0.Results.Count - 1]))
                            {
                                break;
                            }
                            continue;
                        }
                    IL_FB:
                        Thread.Sleep(10);
                        goto IL_102;
                    IL_153:
                        this.systemPerformance_0 = this.tradingSystemExecutor_0.Performance;
                        this.bool_2 = true;
                        base.Invoke(new Optimization.Delegate63(this.method_7));
                        goto IL_102;
                    }
                }
                this.string_0 = text;
            }
            else
            {
                this.bool_0 = false;
                this.optimizer_0.FirstRun();
                while (!this.bool_1)
                {
                    this.exception_0 = null;
                    try
                    {
                        this.tradingSystemExecutor_0.DataSet = this.dataSource_0;
                        this.tradingSystemExecutor_0.Execute(this.optimizer_0.Strategy, this.WealthScript, null, this.list_0);
                        goto IL_2AD;
                    }
                    catch (Exception ex2)
                    {
                        this.exception_0 = ex2;
                        goto IL_2AD;
                    }
                    try
                    {
                    IL_208:
                        base.Invoke(new Optimization.Delegate63(this.method_7));
                        while (this.bool_2)
                        {
                            Thread.Sleep(10);
                        }
                    }
                    catch
                    {
                        this.bool_2 = false;
                    }
                    foreach (Bars current2 in this.list_0)
                    {
                        current2.Cache.Clear();
                    }
                    if (!this.optimizer_0.NextRun(this.systemPerformance_0, this.optimizationResultList_0.Results[this.optimizationResultList_0.Results.Count - 1]))
                    {
                        break;
                    }
                    continue;
                IL_2AD:
                    this.systemPerformance_0 = this.tradingSystemExecutor_0.Performance;
                    this.bool_2 = true;
                    goto IL_208;
                }
            }
            base.Invoke(new Optimization.Delegate63(this.method_8));
        } */


        private void method_7()
        {
            ListViewItem item = null;
            if (this.exception_0 != null)
            {
                item = this.lvErrors.Items.Add(this.string_0);
                string str = "";
                foreach (StrategyParameter parameter2 in this.WealthScript.Parameters)
                {
                    if (str != "")
                    {
                        str = str + ",";
                    }
                    str = str + parameter2.Value;
                }
                item.SubItems.Add(str);
                item.SubItems.Add(this.exception_0.Message);
            }
            TimeSpan span = (TimeSpan) (DateTime.Now - this.dateTime_0);
            this.lblTimeElapsed.Text = this.method_15(span);
            long ticks = span.Ticks;
            double num3 = ((double) this.progOpt.Value) / ((double) this.progOpt.Maximum);
            double num4 = 1.0 - num3;
            double num5 = 1.0 / num3;
            long num6 = (long) (ticks * num5);
            long num7 = (long) (num6 * num4);
            TimeSpan span2 = new TimeSpan(num7);
            this.lblTimeRemaining.Text = this.method_15(span2);
            if (!this.lblElaped.Visible)
            {
                this.lblElaped.Visible = true;
                this.lblTimeElapsed.Visible = true;
                this.lblRemaining.Visible = true;
                this.lblTimeRemaining.Visible = true;
            }
            try
            {
                this.progOpt.Value++;
                this.progOpt.Refresh();
            }
            catch
            {
            }
            string text = (this.string_0 == "") ? ("(" + this.dataSource_0.Name + ")") : this.string_0;
            ListViewItem item2 = this.lvResults.Items.Add(text);
            for (int i = 0; i < this.WealthScript.Parameters.Count; i++)
            {
                item2.SubItems.Add(this.WealthScript.Parameters[i].Value.ToString());
            }
            this.strategyScorecard_0.PopulateScorecard(item2, this.systemPerformance_0);
            OptimizationResult result = new OptimizationResult(text);
            foreach (StrategyParameter parameter in this.WealthScript.Parameters)
            {
                result.ParameterValues.Add(parameter.Value);
            }
            int num = this.WealthScript.Parameters.Count + 1;

            while (true)
            {
                if (num >= this.lvResults.Columns.Count)
                {
                    this.optimizationResultList_0.Add(result);
                    item2.Tag = result;
                    if (item != null)
                    {
                        item.Tag = result;
                    }
                    this.bool_2 = false;
                    break;
                }
                else
                {
                    ColumnHeader header = this.lvResults.Columns[num];
                    string tag = header.Tag as string;
                    double num8 = 0.0;
                    string s = item2.SubItems[num].Text;
                    switch (tag)
                    {
                        case "N":
                            try
                            {
                                num8 = double.Parse(s, NumberStyles.Number);
                            }
                            catch
                            {
                            }
                            result.Results.Add(num8);
                            break;

                        case "C":
                            try
                            {
                                double.Parse(s, NumberStyles.Currency);
                            }
                            catch
                            {
                            }
                            result.Results.Add(num8);
                            break;
                    }
                    num++;
                    ///goto  Label_02AB;  ///WYJ fix, simplify the flow
                    continue;
                }
            }
        }

        private void method_8()
        {
            this.lvResults.EndUpdate();
            this.btnBegin.Enabled = true;
            this.btnCancel.Enabled = false;
            this.btnEstimate.Enabled = true;
            this.Cursor = Cursors.Default;
            this.cbAverage.Visible = this.bool_0;
            if (!this.cbAverage.Visible)
            {
                this.cbAverage.Checked = false;
            }
            this.chartForm_0.IsOptimizing = false;
            this.optimizer_0.RunCompleted(this.optimizationResultList_0);
            if (this.bool_1)
            {
                MessageBox.Show("Optimization canceled.");
            }
            else
            {
                MessageBox.Show("Optimization completed.");
            }
            this.tabOptimize.SelectedTab = this.pageResults;
            this.progOpt.Value = 0;
            for (int i = 0; i < this.list_1.Count; i++)
            {
                this.WealthScript.Parameters[i].Value = this.list_1[i];
            }
        }

        private void method_9(OptimizationResultList optimizationResultList_1)
        {
            this.Cursor = Cursors.WaitCursor;
            this.lvResults.BeginUpdate();
            this.lvResults.Items.Clear();
            foreach (OptimizationResult result in optimizationResultList_1.Results)
            {
                ListViewItem item = this.lvResults.Items.Add(result.Symbol);
                item.Tag = result;
                foreach (double num in result.ParameterValues)
                {
                    item.SubItems.Add(num.ToString());
                }
                for (int i = 0; i < result.Results.Count; i++)
                {
                    string format = "N2";
                    if (result.Symbol != "<Average>")
                    {
                        format = this.strategyScorecard_0.GetFormatCode(optimizationResultList_1.Names[i]);
                    }
                    double num3 = result.Results[i];
                    item.SubItems.Add(num3.ToString(format));
                }
                Application.DoEvents();
            }
            this.lvResults.EndUpdate();
            this.Cursor = Cursors.Default;
        }

        private void mniCopyResults_Click(object sender, EventArgs e)
        {
            MainModule.Instance.CopyListViewToClipboard(this.lvResults);
        }

        private void mniLoadResults_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_0.ShowDialog(this) == DialogResult.OK)
            {
                OptimizationResultList optimizationResultList = OptimizationResultList.LoadFromFile(this.openFileDialog_0.FileName);
                if (this.optimizer_0.FriendlyName != optimizationResultList.OptimizationMethod)
                {
                    Optimizer optimizer = null;
                    IEnumerator enumerator = this.cmbMethod.Items.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            if (enumerator.MoveNext())
                            {
                                Optimizer current = (Optimizer)enumerator.Current;
                                if (current.FriendlyName == optimizationResultList.OptimizationMethod)
                                {
                                    optimizer = current;
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
                    if (optimizer != null)
                    {
                        this.cmbMethod.SelectedItem = optimizer;
                    }
                    else
                    {
                        MessageBox.Show("Could not locate the Optimization Method used to produce these Results.");
                        return;
                    }
                }
                if (this.cmbScorecard.Text != optimizationResultList.Scorecard)
                {
                    StrategyScorecard strategyScorecard = null;
                    IEnumerator enumerator1 = this.cmbScorecard.Items.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            if (enumerator1.MoveNext())
                            {
                                StrategyScorecard current1 = (StrategyScorecard)enumerator1.Current;
                                if (current1.FriendlyName == optimizationResultList.Scorecard)
                                {
                                    strategyScorecard = current1;
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
                        IDisposable disposable1 = enumerator1 as IDisposable;
                        if (disposable1 != null)
                        {
                            disposable1.Dispose();
                        }
                    }
                    if (strategyScorecard == null)
                    {
                        MessageBox.Show("Could not locate the ScoreCard used to represent the Results.");
                        return;
                    }
                    else
                    {
                        this.cmbScorecard.SelectedItem = strategyScorecard;
                    }
                }
                Guid guid = new Guid(optimizationResultList.StrategyID);
                if (guid != this.chartForm_0.Strategy.ID)
                {
                    Strategy strategy = MainModule.Instance.Strategies.LookupID(optimizationResultList.StrategyID);
                    if (strategy != null)
                    {
                        this.chartForm_0.Strategy = strategy;
                        this.LoadParameterList();
                        if (this.chartForm_0.Symbol != "")
                        {
                            this.chartForm_0.GoButtonPressed(this.chartForm_0.Symbol, true);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not locate the Strategy that these Optimization Results were based on.");
                        return;
                    }
                }
                if (optimizationResultList.Results.Count > 0 && this.chartForm_0.WealthScript != null)
                {
                    OptimizationResult item = optimizationResultList.Results[0];
                    if (item.ParameterValues.Count != this.chartForm_0.WealthScript.Parameters.Count)
                    {
                        MessageBox.Show("The number of Parameters in the saved Results does not match the number of Parameters in the Strategy.  Cannot load the Results.");
                        return;
                    }
                }
                this.optimizationResultList_0 = optimizationResultList;
                this.method_5();
                this.lvResults.EndUpdate();
                this.method_9(this.optimizationResultList_0);
                this.optimizer_0.WealthScript = this.WealthScript;
                this.optimizer_0.RunCompleted(this.optimizationResultList_0);
            }
        }

        private void mniPrintResults_Click(object sender, EventArgs e)
        {
            this.Print("Optimization Results", this.lvResults, true);
        }

        private void mniPVAllThisRow_Click(object sender, EventArgs e)
        {
            OptimizationResult tag = this.lvResults.SelectedItems[0].Tag as OptimizationResult;
            foreach (string str in this.dataSource_0.Symbols)
            {
                this.chartForm_0.Strategy.StorePreferredValues(str, this.WealthScript, tag.ParameterValues);
            }
            this.chartForm_0.NeedSave = true;
            MessageBox.Show("Preferred Values set for " + this.dataSource_0.Symbols.Count + " Symbol(s).");
            this.chartForm_0.PreferredValuesChanged();
        }

        private void mniPVAvgHighest_Click(object sender, EventArgs e)
        {
            this.method_17(1.0);
        }

        private void mniPVAvgLowest_Click(object sender, EventArgs e)
        {
            this.method_17(-1.0);
        }

        private void mniPVHighest_Click(object sender, EventArgs e)
        {
            this.method_16(1.0);
        }

        private void mniPVLowest_Click(object sender, EventArgs e)
        {
            this.method_16(-1.0);
        }

        private void mniPVThisRow_Click(object sender, EventArgs e)
        {
            OptimizationResult tag = this.lvResults.SelectedItems[0].Tag as OptimizationResult;
            this.chartForm_0.Strategy.StorePreferredValues(tag.Symbol, this.WealthScript, tag.ParameterValues);
            this.chartForm_0.NeedSave = true;
            MessageBox.Show("Preferred Values set for Symbol " + tag.Symbol);
            this.chartForm_0.PreferredValuesChanged();
        }

        private void mniSaveResults_Click(object sender, EventArgs e)
        {
            if (this.optimizationResultList_0 != null)
            {
                this.saveFileDialog_0.InitialDirectory = MainModule.Instance.DataPath;
                this.openFileDialog_0.InitialDirectory = this.saveFileDialog_0.InitialDirectory;
                if (this.saveFileDialog_0.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = this.saveFileDialog_0.FileName;
                    FileNameValidator.ValidateFileName(fileName);
                    this.optimizationResultList_0.SaveToFile(fileName);
                    MessageBox.Show("Optimization Result set saved to: " + fileName);
                }
            }
        }

        private void mniSetDefault_Click(object sender, EventArgs e)
        {
            if (this.lvResults.SelectedItems.Count == 1)
            {
                OptimizationResult tag = (OptimizationResult) this.lvResults.SelectedItems[0].Tag;
                for (int i = 0; i < this.WealthScript.Parameters.Count; i++)
                {
                    this.WealthScript.Parameters[i].DefaultValue = tag.ParameterValues[i];
                }
                this.LoadParameterList();
                this.updateEditorForParameters();
            }
        }

        private void numDefault_Leave(object sender, EventArgs e)
        {
            StrategyParameter selectedParameter = this.SelectedParameter;
            if (selectedParameter != null)
            {
                if (!((selectedParameter.Stop > selectedParameter.Start) ? (selectedParameter.Step > 0.0) : (selectedParameter.Step < 0.0)))
                {
                    selectedParameter.Step = (selectedParameter.Stop > selectedParameter.Start) ? ((double) 1) : ((double) (-1));
                    this.numStep.Text = selectedParameter.Step.ToString();
                }
                while (true)
                {
                    double num4 = (selectedParameter.Stop - selectedParameter.Start) / selectedParameter.Step;
                    if ((num4 > 10000.0) || (selectedParameter.Step < -7.9228162514264338E+28))
                    {
                        double num3 = selectedParameter.Step * 10.0;
                        this.numStep.Text = num3.ToString();
                        this.lvParameters.SelectedItems[0].SubItems[4].Text = this.numStep.Text;
                        selectedParameter.Step = num3;
                    }
                    if (num4 <= 10000.0)
                    {
                        double start;
                        if (selectedParameter.Step > 0.0)
                        {
                            start = selectedParameter.Start;
                            while (start < selectedParameter.DefaultValue)
                            {
                                start += selectedParameter.Step;
                            }
                            if (start < selectedParameter.Start)
                            {
                                start = selectedParameter.Start;
                            }
                            if (start > selectedParameter.Stop)
                            {
                                start = selectedParameter.Stop;
                            }
                        }
                        else
                        {
                            start = selectedParameter.Stop;
                            while (start < selectedParameter.DefaultValue)
                            {
                                start -= selectedParameter.Step;
                            }
                            if (start > selectedParameter.Start)
                            {
                                start = selectedParameter.Start;
                            }
                            if (start < selectedParameter.Stop)
                            {
                                start = selectedParameter.Stop;
                            }
                        }
                        selectedParameter.DefaultValue = start;
                        this.numDefault.Text = start.ToString();
                        return;
                    }
                }
            }
        }

        private void numDefault_TextChanged(object sender, EventArgs e)
        {
            if (!this.bool_3 && (this.SelectedParameter != null))
            {
                this.SelectedParameter.DefaultValue = (double) this.numDefault.Value;
                this.lvParameters.SelectedItems[0].SubItems[1].Text = this.SelectedParameter.DefaultValue.ToString();
                this.ApplyNeeded = true;
            }
        }

        private void numStart_TextChanged(object sender, EventArgs e)
        {
            if (!this.bool_3 && (this.SelectedParameter != null))
            {
                this.SelectedParameter.Start = (double) this.numStart.Value;
                this.lvParameters.SelectedItems[0].SubItems[2].Text = this.SelectedParameter.Start.ToString();
                this.UpdateRunsRequired();
                this.ApplyNeeded = true;
            }
        }

        private void numStep_TextChanged(object sender, EventArgs e)
        {
            if (!this.bool_3 && (this.SelectedParameter != null))
            {
                this.SelectedParameter.Step = (double) this.numStep.Value;
                this.lvParameters.SelectedItems[0].SubItems[4].Text = this.SelectedParameter.Step.ToString();
                this.UpdateRunsRequired();
                this.ApplyNeeded = true;
            }
        }

        private void numStop_TextChanged(object sender, EventArgs e)
        {
            if (!this.bool_3 && (this.SelectedParameter != null))
            {
                this.SelectedParameter.Stop = (double) this.numStop.Value;
                this.lvParameters.SelectedItems[0].SubItems[3].Text = this.SelectedParameter.Stop.ToString();
                this.UpdateRunsRequired();
                this.ApplyNeeded = true;
            }
        }

        private void Optimization_Load(object sender, EventArgs e)
        {
            bool flag = this.chartForm_0.Strategy.StrategyType == StrategyType.Script;
            if (this.chartForm_0.Strategy.StrategyType == StrategyType.Compiled)
            {
                this.lblRuleBased.Text = "This is a pre-compiled Strategy and the Parameters cannot be changed here.";
            }
            this.lblRuleBased.Visible = !flag;
            this.btnAddParameter.Visible = flag;
            this.btnRemoveParam.Visible = flag;
            this.btnApplyChanges.Visible = flag;
            this.btnRollback.Visible = flag;
            this.txtName.ReadOnly = !flag;
            this.numDefault.ReadOnly = !flag;
            this.numStart.ReadOnly = !flag;
            this.numStop.ReadOnly = !flag;
            this.numStep.ReadOnly = !flag;
        }

        private void popupResults_Opening(object sender, CancelEventArgs e)
        {
            if (this.lvResults.SelectedItems.Count != 1)
            {
                this.mniPVHighest.Enabled = false;
                this.mniPVLowest.Enabled = false;
                this.mniPVAvgHighest.Enabled = false;
                this.mniPVAvgLowest.Enabled = false;
                this.mniPVThisRow.Enabled = false;
                this.mniPVAllThisRow.Enabled = false;
                this.mniSetDefault.Visible = false;
            }
            else
            {
                this.mniPVHighest.Enabled = true;
                this.mniPVLowest.Enabled = true;
                this.mniPVAvgHighest.Enabled = this.bool_0;
                this.mniPVAvgLowest.Enabled = this.bool_0;
                this.mniPVThisRow.Enabled = !this.lvResults.SelectedItems[0].Text.StartsWith("(");
                this.mniPVAllThisRow.Enabled = this.bool_4;
                this.mniSetDefault.Visible = true;
            }
        }

        public void Print(string title, Bitmap graphic, bool bUseDefaultDisclosure)
        {
            this.method_18(title, null, graphic, bUseDefaultDisclosure);
        }

        public void Print(string title, ListView listView_0, bool bUseDefaultDisclosure)
        {
            this.method_18(title, listView_0, null, bUseDefaultDisclosure);
        }

        public void RefreshViews()
        {
            if (this.optimizer_0 != null)
            {
                this.optimizer_0.RefreshViews();
            }
        }

        public void Set(string string_1, bool value)
        {
            this.isettingsHost_0.Set(this.method_1(string_1), value);
        }

        public void Set(string string_1, DateTime value)
        {
            this.isettingsHost_0.Set(this.method_1(string_1), value);
        }

        public void Set(string string_1, double value)
        {
            this.isettingsHost_0.Set(this.method_1(string_1), value);
        }

        public void Set(string string_1, Color color)
        {
            this.isettingsHost_0.Set(this.method_1(string_1), color);
        }

        public void Set(string string_1, Font value)
        {
            this.isettingsHost_0.Set(this.method_1(string_1), value);
        }

        public void Set(string string_1, int value)
        {
            this.isettingsHost_0.Set(this.method_1(string_1), value);
        }

        public void Set(string string_1, string value)
        {
            this.isettingsHost_0.Set(this.method_1(string_1), value);
        }

        public void Set(Form form_0, string string_1)
        {
            this.isettingsHost_0.Set(form_0, this.method_1(string_1));
        }

        public bool ShowPrintDialog()
        {
            return !MainModule.Instance.Settings.Get("HidePrintDialog", false);
        }

        public bool ShowPrintPreview()
        {
            return !MainModule.Instance.Settings.Get("HidePrintPreview", false);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!this.bool_3 && (this.SelectedParameter != null))
            {
                this.lvParameters.SelectedItems[0].Text = this.txtName.Text;
                this.SelectedParameter.NameEdited = this.txtName.Text;
                this.ApplyNeeded = true;
            }
        }

        public void UpdateRunsRequired()
        {
            int count = 1;
            if ((this.chartForm_0.MyMainForm.Symbol == "") && (this.chartForm_0.DataSource != null))
            {
                count = this.chartForm_0.DataSource.Symbols.Count;
            }
            this.lblRunsRequired.Text = "Runs Required: " + (this.optimizer_0.NumberOfRuns * count);
        }

        private PageSettings _pageSettings
        {
            get
            {
                return this.mainForm_0.DefaultPageSettings;
            }
        }

        public bool ApplyNeeded
        {
            get
            {
                return this.btnApplyChanges.Enabled;
            }
            set
            {
                this.btnApplyChanges.Enabled = value;
                if (value)
                {
                    this.btnRollback.Enabled = true;
                }
            }
        }

        public WealthLabPro.ChartForm ChartForm
        {
            get
            {
                return this.chartForm_0;
            }
        }

        public WealthLabPro.MainForm MainForm
        {
            get
            {
                return this.mainForm_0;
            }
        }

        public IList<string> MetricNames
        {
            get
            {
                if (this.list_2.Count == 0)
                {
                    Control.CheckForIllegalCrossThreadCalls = false;
                    this.method_4();
                    Control.CheckForIllegalCrossThreadCalls = true;
                }
                return this.list_2;
            }
        }

        public StrategyParameter SelectedParameter
        {
            get
            {
                if (this.lvParameters.SelectedItems.Count != 1)
                {
                    return null;
                }
                return (this.lvParameters.SelectedItems[0].Tag as StrategyParameter);
            }
        }

        public WealthLab.WealthScript WealthScript
        {
            get
            {
                return this.chartForm_0.WealthScript;
            }
        }

        private delegate void Delegate63();
    }
}

