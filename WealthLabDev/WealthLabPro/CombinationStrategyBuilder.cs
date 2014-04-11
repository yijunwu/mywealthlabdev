namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WealthLab;

    public class CombinationStrategyBuilder : UserControl
    {
        private bool bool_0 = true;
        private WebBrowser browser;
        private Button btnAdd;
        private Button btnRemove;
        private ToolStripButton btnRun;
        private CheckBox cbDefaultDataSet;
        private CheckBox cbUsePreferredValues;
        private ChartForm chartForm_0;
        private ComboBox cmbAccounts;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private double startingEquity = 100000.0;
        private double marginFactor = 1.0;
        private GroupBox grpAllocations;
        private GroupBox grpDataSet;
        private GroupBox grpDollars;
        private GroupBox grpParameters;
        private IContainer components;
        private ImageList imageList_0;
        private ImageList imageList_1;
        private const int int_0 = 0;
        private const int int_1 = 1;
        private const int int_2 = 2;
        private const int int_3 = 3;
        private const int int_4 = 4;
        private const int int_5 = 5;
        private Label label8;
        private ToolStripLabel lblMargin;
        private ToolStripLabel lblMarginRatio;
        private Label lblPositions;
        private Label lblPriority;
        private ToolStripLabel lblProgress;
        private Label lblScale;
        private ToolStripLabel lblStartingCapital;
        private SortableListView lvStrategies;
        private NumericUpDown numDollar;
        private NumericUpDown numPctEquity;
        private NumericUpDown numPriorityOfStrategy;
        private ParameterSlidersContainer paramSlidersContainer;
        private PositionSizeSelecter posSize;
        private ToolStripProgressBar progress;
        private RadioButton rbDollar;
        private RadioButton rbPctEquity;
        private ScaleSelecter scale;
        [CompilerGenerated]
        private Strategy combinationStrategy;
        private TreeView strategyTree;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStrip toolStrip3;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolTip toolTip_0;
        private DataSourceTreeView treeDataSet;
        private ToolStripTextBox txtMargin;
        private ToolStripTextBox txtStartingEquity;

        public CombinationStrategyBuilder(ChartForm myParent)
        {
            this.InitializeComponent();
            this.chartForm_0 = myParent;
        }

        public void AssignMaxToProgressControl()
        {
            int num = 0;
            foreach (CombinedStrategyInfo info in this.CombinationStrategy.CombinedStrategyChildren)
            {
                if (info.UseDefaultDataSet && (this.chartForm_0.MyMainForm.DataSource != null))
                {
                    if ((this.chartForm_0.MyMainForm.Symbol != "") && (this.chartForm_0.MyMainForm.Symbol != null))
                    {
                        num++;
                    }
                    else
                    {
                        num += this.chartForm_0.DataSource.Symbols.Count;
                    }
                }
                if ((info.Symbol != "") && (info.Symbol != null))
                {
                    num++;
                }
                else
                {
                    DataSource source = MainModule.Instance.DataSources.FindDataSource(info.DataSetName);
                    if (source != null)
                    {
                        num += source.Symbols.Count;
                    }
                }
            }
            this.progress.Value = 0;
            this.progress.Minimum = 0;
            this.progress.Maximum = num;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.strategyTree.SelectedNode;
            if ((selectedNode != null) && (selectedNode.Tag != null))
            {
                Strategy tag = (Strategy) selectedNode.Tag;
                this.method_4(tag);
                CombinedStrategyInfo info2 = new CombinedStrategyInfo {
                    StrategyID = tag.ID
                };
                this.CombinationStrategy.CombinedStrategyChildren.Add(info2);
                bool flag = this.CheckForCircularReference();
                this.CombinationStrategy.CombinedStrategyChildren.Remove(info2);
                if (flag)
                {
                    MessageBox.Show("This Strategy is itself a Combined Stragtegy, and adding it would cause a circular reference.  Please select a different Strategy to add.");
                }
                else
                {
                    ListViewItem item = this.lvStrategies.Items.Add(tag.Name);
                    CombinedStrategyInfo info = new CombinedStrategyInfo {
                        StrategyID = tag.ID,
                        Name = tag.Name,
                        AccountNumber = MainModule.Instance.DefaultAccountNumber
                    };
                    item.Tag = info;
                    this.CombinationStrategy.CombinedStrategyChildren.Add(info);
                    this.method_5(item, info);
                    this.method_7(true);
                    WealthScript wealthScriptObject = MainModule.Instance.Strategies.GetWealthScriptObject(tag);
                    this.paramSlidersContainer.WealthScript = wealthScriptObject;
                    info.Tag = wealthScriptObject;
                    if (info.ParameterValues == null)
                    {
                        info.ParameterValues = new List<double>();
                    }
                    info.ParameterValues.Clear();
                    if (wealthScriptObject != null)
                    {
                        foreach (StrategyParameter parameter in wealthScriptObject.Parameters)
                        {
                            info.ParameterValues.Add(parameter.Value);
                        }
                    }
                    item.Selected = true;
                }
            }
            else
            {
                MessageBox.Show("Please select strategy from Available strategies", "Combination Strategy", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.lvStrategies.SelectedItems.Count > 0)
            {
                if (DialogResult.OK == MessageBox.Show("Are you sure you want to remove this strategy?", "Combination Strategy", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    this.method_8();
                }
            }
            else
            {
                MessageBox.Show("Please select a strategy to remove.", "Combination Strategy", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (this.chartForm_0.MyMainForm.DataSource == null)
            {
                this.chartForm_0.MyMainForm.method_14();
            }
            PositionSize positionSize = this.chartForm_0.posSize.PositionSize;
            positionSize.Mode = PosSizeMode.Dollar;
            positionSize.StartingCapital = this.StartingEquity;
            positionSize.MarginFactor = this.MarginFactor;
            this.chartForm_0.posSize.PositionSize = positionSize;
            if (this.chartForm_0.MyMainForm.SelectingNodeForFormCreation)
            {
                this.chartForm_0.MyMainForm.SelectingNodeForFormCreation = false;
            }
            if (this.chartForm_0.MyMainForm.Symbol == "")
            {
                this.chartForm_0.ShowMultiSymbolControls(true);
                this.chartForm_0.RunOnAllSymbols();
            }
            else
            {
                this.chartForm_0.GoButtonPressed(this.chartForm_0.MyMainForm.Symbol, true);
                this.chartForm_0.MyMainForm.SelectTreeNode(this.chartForm_0.MyMainForm.DataSource, this.chartForm_0.MyMainForm.Symbol);
            }
        }

        private void cbDefaultDataSet_CheckedChanged(object sender, EventArgs e)
        {
            this.treeDataSet.Enabled = !this.cbDefaultDataSet.Checked;
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if ((selectedChild != null) && !this.bool_0)
            {
                selectedChild.UseDefaultDataSet = this.cbDefaultDataSet.Checked;
                this.chartForm_0.NeedSave = true;
                this.method_6();
                this.method_14();
            }
        }

        private void cbUsePreferredValues_CheckedChanged(object sender, EventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.UsePreferredValues = this.cbUsePreferredValues.Checked;
                    this.paramSlidersContainer.Enabled = !this.cbUsePreferredValues.Checked;
                    this.method_14();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        public bool CheckForCircularReference()
        {
            List<Strategy> list = new List<Strategy>();
            this.method_3(this.CombinationStrategy, list);
            return (list.Count > 500);
        }

        private void cmbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.AccountNumber = (string) this.cmbAccounts.Items[this.cmbAccounts.SelectedIndex];
                    this.method_6();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        private void CombinationStrategyBuilder_Load(object sender, EventArgs e)
        {
            this.method_7(false);
            this.toolTip_0.SetToolTip(this.cbDefaultDataSet, "Select this option to allow the Combined Strategy to be run for whatever DataSet is selected in the standard DataSet Tree.");
            this.method_2();
            this.method_1();
            if (this.CombinationStrategy.CombinedStrategyChildren.Count > 0)
            {
                this.method_0();
                this.method_7(true);
                if (this.lvStrategies.Items.Count > 0)
                {
                    this.lvStrategies.Items[0].Selected = true;
                }
            }
            else
            {
                File.WriteAllText(MainModule.Instance.DataPath + @"\temp.html", "<html>\r\n\r\n\t<head>\r\n\t\t<meta http-equiv=\"Content-Language\" content=\"en-us\">\r\n\t\t\t<meta name=\"GENERATOR\" content=\"Microsoft FrontPage 5.0\">\r\n\t\t\t\t<meta name=\"ProgId\" content=\"FrontPage.Editor.Document\">\r\n\t\t\t\t\t<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">\r\n\t\t\t\t\t\t<title>New Page 1</title>\r\n\t\t\t\t\t\t<style>\r\n\t\t\t\t\t\t\t<!--\r\n\t\t\t\t\t\t\th1           { font-family: Tahoma; font-size: 12pt; font-weight: bold }\r\n\t\t\t\t\t\t\t-->\r\n\t\t\t\t\t\t\t</style>\r\n\t\t\t\t\t\t</head>\r\n\r\n\t\t\t\t\t<body>\r\n\r\n\t\t\t\t\t\t<h1><font face=\"Verdana\" size=\"2\">Getting Started:</font></h1>\r\n\t\t\t\t\t\t<p class=\"MsoNormal\"><font size=\"2\"><span style=\"font-family: Verdana\">To build a Combination strategy, select two or more strategies from \"Available Strategies\" (left) and add them to \"Your Selected Strategies\" (right). Configure the settings of each strategy and run a test to see how they perform.</span></font></p>\r\n\r\n\t\t\t\t\t\t</body>\r\n\r\n\t\t\t\t\t</html>");
                this.browser.Navigate(MainModule.Instance.DataPath + @"\temp.html");
            }
            if (this.chartForm_0.Strategy.StartingEquity > 0.0)
            {
                this.txtStartingEquity.Text = this.chartForm_0.Strategy.StartingEquity.ToString();
            }
            else
            {
                this.txtStartingEquity.Text = this.chartForm_0.MyMainForm.PositionSize.StartingCapital.ToString();
            }
            if (this.chartForm_0.Strategy.MarginFactor > 0.0)
            {
                this.txtMargin.Text = this.chartForm_0.Strategy.MarginFactor.ToString();
            }
            this.chartForm_0.NeedSave = false;
            MainModule.Instance.HelpProvider.SetHelpNavigator(this, HelpNavigator.Topic);
            MainModule.Instance.HelpProvider.SetHelpKeyword(this, "combination_strategy.htm");
            int[] bits = new int[4];
            bits[0] = 100 * ((int) this.marginFactor);
            this.numPctEquity.Maximum = new decimal(bits);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(CombinationStrategyBuilder));
            this.toolStrip1 = new ToolStrip();
            this.btnRun = new ToolStripButton();
            this.lblStartingCapital = new ToolStripLabel();
            this.txtStartingEquity = new ToolStripTextBox();
            this.lblMargin = new ToolStripLabel();
            this.txtMargin = new ToolStripTextBox();
            this.lblMarginRatio = new ToolStripLabel();
            this.lvStrategies = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.strategyTree = new TreeView();
            this.imageList_0 = new ImageList(this.components);
            this.btnAdd = new Button();
            this.btnRemove = new Button();
            this.toolStrip2 = new ToolStrip();
            this.toolStripLabel1 = new ToolStripLabel();
            this.toolStrip3 = new ToolStrip();
            this.toolStripLabel2 = new ToolStripLabel();
            this.imageList_1 = new ImageList(this.components);
            this.grpDataSet = new GroupBox();
            this.treeDataSet = new DataSourceTreeView();
            this.cbDefaultDataSet = new CheckBox();
            this.toolTip_0 = new ToolTip(this.components);
            this.grpDollars = new GroupBox();
            this.cbUsePreferredValues = new CheckBox();
            this.cmbAccounts = new ComboBox();
            this.label8 = new Label();
            this.lblScale = new Label();
            this.scale = new ScaleSelecter();
            this.lblPositions = new Label();
            this.posSize = new PositionSizeSelecter();
            this.grpParameters = new GroupBox();
            this.paramSlidersContainer = new ParameterSlidersContainer();
            this.grpAllocations = new GroupBox();
            this.numPriorityOfStrategy = new NumericUpDown();
            this.lblPriority = new Label();
            this.numPctEquity = new NumericUpDown();
            this.rbPctEquity = new RadioButton();
            this.numDollar = new NumericUpDown();
            this.rbDollar = new RadioButton();
            this.browser = new WebBrowser();
            this.lblProgress = new ToolStripLabel();
            this.progress = new ToolStripProgressBar();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.grpDataSet.SuspendLayout();
            this.grpDollars.SuspendLayout();
            this.grpParameters.SuspendLayout();
            this.grpAllocations.SuspendLayout();
            this.numPriorityOfStrategy.BeginInit();
            this.numPctEquity.BeginInit();
            this.numDollar.BeginInit();
            base.SuspendLayout();
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.btnRun, this.lblStartingCapital, this.txtStartingEquity, this.lblMargin, this.txtMargin, this.lblMarginRatio, this.toolStripSeparator1, this.lblProgress, this.progress });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x325, 0x19);
            this.toolStrip1.TabIndex = 0x29;
            this.toolStrip1.Text = "toolStrip1";
            this.btnRun.Image = (Image) resources.GetObject("btnRun.Image");
            this.btnRun.ImageTransparentColor = Color.Fuchsia;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new Size(110, 0x16);
            this.btnRun.Text = "Run the Strategy";
            this.btnRun.Click += new EventHandler(this.btnRun_Click);
            this.lblStartingCapital.Name = "lblStartingCapital";
            this.lblStartingCapital.Size = new Size(0x52, 0x16);
            this.lblStartingCapital.Text = "Starting Equity:";
            this.txtStartingEquity.Name = "txtStartingEquity";
            this.txtStartingEquity.Size = new Size(100, 0x19);
            this.txtStartingEquity.TextChanged += new EventHandler(this.txtStartingEquity_TextChanged);
            this.lblMargin.Name = "lblMargin";
            this.lblMargin.Size = new Size(0x4d, 0x16);
            this.lblMargin.Text = "Margin Factor:";
            this.txtMargin.Name = "txtMargin";
            this.txtMargin.Size = new Size(0x18, 0x19);
            this.txtMargin.Text = "1";
            this.txtMargin.TextChanged += new EventHandler(this.txtMargin_TextChanged);
            this.lblMarginRatio.Name = "lblMarginRatio";
            this.lblMarginRatio.Size = new Size(20, 0x16);
            this.lblMarginRatio.Text = ": 1";
            this.lvStrategies.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lvStrategies.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_3, this.columnHeader_2, this.columnHeader_5, this.columnHeader_6, this.columnHeader_4 });
            this.lvStrategies.FullRowSelect = true;
            this.lvStrategies.HideSelection = false;
            this.lvStrategies.Location = new Point(410, 60);
            this.lvStrategies.MultiSelect = false;
            this.lvStrategies.Name = "lvStrategies";
            this.lvStrategies.Size = new Size(0x183, 0xc7);
            this.lvStrategies.TabIndex = 3;
            this.lvStrategies.UseCompatibleStateImageBehavior = false;
            this.lvStrategies.View = View.Details;
            this.lvStrategies.SelectedIndexChanged += new EventHandler(this.lvStrategies_SelectedIndexChanged);
            this.columnHeader_0.Text = "Strategy Name";
            this.columnHeader_0.Width = 100;
            this.columnHeader_1.Text = "Account";
            this.columnHeader_3.Text = "Equity Allocation";
            this.columnHeader_3.Width = 90;
            this.columnHeader_2.Text = "Priority";
            this.columnHeader_5.Text = "Pos Sizing";
            this.columnHeader_5.Width = 70;
            this.columnHeader_6.Text = "DataSet";
            this.columnHeader_4.Text = "Data Scale";
            this.columnHeader_4.Width = 70;
            this.strategyTree.HideSelection = false;
            this.strategyTree.ImageIndex = 0;
            this.strategyTree.ImageList = this.imageList_0;
            this.strategyTree.Location = new Point(5, 0x3d);
            this.strategyTree.Name = "strategyTree";
            this.strategyTree.SelectedImageIndex = 0;
            this.strategyTree.Size = new Size(0x173, 0xc6);
            this.strategyTree.TabIndex = 0;
            this.strategyTree.DoubleClick += new EventHandler(this.strategyTree_DoubleClick);
            this.imageList_0.ImageStream = (ImageListStreamer) resources.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "FolderClosed.bmp");
            this.imageList_0.Images.SetKeyName(1, "FolderOpen.bmp");
            this.imageList_0.Images.SetKeyName(2, "assembly.bmp");
            this.imageList_0.Images.SetKeyName(3, "Editor.bmp");
            this.imageList_0.Images.SetKeyName(4, "Wizard.bmp");
            this.imageList_0.Images.SetKeyName(5, "precompiled.bmp");
            this.btnAdd.Location = new Point(0x17e, 0x4b);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x16, 0x17);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.btnRemove.Location = new Point(0x17e, 0x68);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(0x16, 0x17);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            this.toolStrip2.AutoSize = false;
            this.toolStrip2.Dock = DockStyle.None;
            this.toolStrip2.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new ToolStripItem[] { this.toolStripLabel1 });
            this.toolStrip2.Location = new Point(5, 0x23);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new Size(0x173, 0x17);
            this.toolStrip2.TabIndex = 0x36;
            this.toolStrip2.Text = "toolStrip2";
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(0x66, 20);
            this.toolStripLabel1.Text = "Available Strategies";
            this.toolStrip3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.toolStrip3.AutoSize = false;
            this.toolStrip3.Dock = DockStyle.None;
            this.toolStrip3.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip3.Items.AddRange(new ToolStripItem[] { this.toolStripLabel2 });
            this.toolStrip3.Location = new Point(410, 0x23);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new Size(0x183, 0x16);
            this.toolStrip3.TabIndex = 0x37;
            this.toolStrip3.Text = "toolStrip3";
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new Size(0x7d, 0x13);
            this.toolStripLabel2.Text = "Your Selected Strategies";
            this.imageList_1.ImageStream = (ImageListStreamer) resources.GetObject("imageList1.ImageStream");
            this.imageList_1.TransparentColor = Color.Fuchsia;
            this.imageList_1.Images.SetKeyName(0, "Execute.bmp");
            this.imageList_1.Images.SetKeyName(1, "Delete.bmp");
            this.grpDataSet.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.grpDataSet.Controls.Add(this.treeDataSet);
            this.grpDataSet.Controls.Add(this.cbDefaultDataSet);
            this.grpDataSet.Location = new Point(0x25e, 0x109);
            this.grpDataSet.Name = "grpDataSet";
            this.grpDataSet.Size = new Size(0xbd, 0xe0);
            this.grpDataSet.TabIndex = 7;
            this.grpDataSet.TabStop = false;
            this.grpDataSet.Text = "DataSet";
            this.treeDataSet.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeDataSet.HideSelection = false;
            this.treeDataSet.ImageIndex = 0;
            this.treeDataSet.Location = new Point(6, 0x21);
            this.treeDataSet.Name = "treeDataSet";
            this.treeDataSet.SelectedImageIndex = 0;
            this.treeDataSet.Size = new Size(0xaf, 0xb9);
            this.treeDataSet.TabIndex = 1;
            this.treeDataSet.SymbolSelected += new EventHandler<DataSourceSymbolEventArgs>(this.method_12);
            this.treeDataSet.DataSourceSelected += new EventHandler<DataSourceEventArgs>(this.method_11);
            this.cbDefaultDataSet.AutoSize = true;
            this.cbDefaultDataSet.Location = new Point(6, 14);
            this.cbDefaultDataSet.Name = "cbDefaultDataSet";
            this.cbDefaultDataSet.Size = new Size(0x7a, 0x11);
            this.cbDefaultDataSet.TabIndex = 0;
            this.cbDefaultDataSet.Text = "Use default DataSet";
            this.cbDefaultDataSet.UseVisualStyleBackColor = true;
            this.cbDefaultDataSet.CheckedChanged += new EventHandler(this.cbDefaultDataSet_CheckedChanged);
            this.grpDollars.Controls.Add(this.cbUsePreferredValues);
            this.grpDollars.Controls.Add(this.cmbAccounts);
            this.grpDollars.Controls.Add(this.label8);
            this.grpDollars.Controls.Add(this.lblScale);
            this.grpDollars.Controls.Add(this.scale);
            this.grpDollars.Controls.Add(this.lblPositions);
            this.grpDollars.Controls.Add(this.posSize);
            this.grpDollars.Location = new Point(0xc9, 0x109);
            this.grpDollars.Name = "grpDollars";
            this.grpDollars.Size = new Size(0xd5, 0x8a);
            this.grpDollars.TabIndex = 6;
            this.grpDollars.TabStop = false;
            this.grpDollars.Text = "Settings";
            this.cbUsePreferredValues.Location = new Point(12, 90);
            this.cbUsePreferredValues.Name = "cbUsePreferredValues";
            this.cbUsePreferredValues.Size = new Size(0x9e, 0x26);
            this.cbUsePreferredValues.TabIndex = 3;
            this.cbUsePreferredValues.Text = "Use Preferred Strategy Parameter Values";
            this.cbUsePreferredValues.UseVisualStyleBackColor = true;
            this.cbUsePreferredValues.CheckedChanged += new EventHandler(this.cbUsePreferredValues_CheckedChanged);
            this.cmbAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccounts.FormattingEnabled = true;
            this.cmbAccounts.Location = new Point(0x54, 12);
            this.cmbAccounts.Name = "cmbAccounts";
            this.cmbAccounts.Size = new Size(0x7b, 0x15);
            this.cmbAccounts.TabIndex = 0;
            this.cmbAccounts.SelectedIndexChanged += new EventHandler(this.cmbAccounts_SelectedIndexChanged);
            this.label8.AutoSize = true;
            this.label8.Location = new Point(6, 0x10);
            this.label8.Name = "label8";
            this.label8.Size = new Size(50, 13);
            this.label8.TabIndex = 0x17;
            this.label8.Text = "Account:";
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new Point(6, 0x2a);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new Size(0x25, 13);
            this.lblScale.TabIndex = 0x16;
            this.lblScale.Text = "Scale:";
            this.scale.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.scale.BackColor = Color.Cornsilk;
            this.scale.Location = new Point(0x54, 40);
            this.scale.Name = "scale";
            this.scale.Size = new Size(0x7b, 20);
            this.scale.SM = false;
            this.scale.TabIndex = 1;
            this.scale.ScaleChanged += new EventHandler<EventArgs>(this.method_9);
            this.lblPositions.AutoSize = true;
            this.lblPositions.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblPositions.Location = new Point(8, 0x40);
            this.lblPositions.Name = "lblPositions";
            this.lblPositions.Size = new Size(70, 13);
            this.lblPositions.TabIndex = 0x13;
            this.lblPositions.Text = "Position Size:";
            this.posSize.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.posSize.BackColor = Color.Honeydew;
            this.posSize.CombinationStrategyChildMode = true;
            this.posSize.Location = new Point(0x54, 0x42);
            this.posSize.Name = "posSize";
            this.posSize.Size = new Size(0x7b, 20);
            this.posSize.TabIndex = 2;
            this.posSize.PositionSizeChanged += new EventHandler<EventArgs>(this.method_10);
            this.grpParameters.Controls.Add(this.paramSlidersContainer);
            this.grpParameters.Location = new Point(0x1a6, 0x109);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new Size(0xb1, 0x8a);
            this.grpParameters.TabIndex = 5;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Parameters";
            this.paramSlidersContainer.BackColor = SystemColors.Control;
            this.paramSlidersContainer.Location = new Point(6, 0x11);
            this.paramSlidersContainer.Name = "paramSlidersContainer";
            this.paramSlidersContainer.Size = new Size(0xa5, 0x6c);
            this.paramSlidersContainer.TabIndex = 1;
            this.paramSlidersContainer.WealthScript = null;
            this.paramSlidersContainer.SliderValueChanged += new EventHandler<EventArgs>(this.method_13);
            this.grpAllocations.Controls.Add(this.numPriorityOfStrategy);
            this.grpAllocations.Controls.Add(this.lblPriority);
            this.grpAllocations.Controls.Add(this.numPctEquity);
            this.grpAllocations.Controls.Add(this.rbPctEquity);
            this.grpAllocations.Controls.Add(this.numDollar);
            this.grpAllocations.Controls.Add(this.rbDollar);
            this.grpAllocations.Location = new Point(7, 0x109);
            this.grpAllocations.Name = "grpAllocations";
            this.grpAllocations.Size = new Size(0xbc, 0x8a);
            this.grpAllocations.TabIndex = 4;
            this.grpAllocations.TabStop = false;
            this.grpAllocations.Text = "Allocation";
            this.numPriorityOfStrategy.Location = new Point(0x73, 0x58);
            int[] bits = new int[4];
            bits[0] = 0xf423f;
            this.numPriorityOfStrategy.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numPriorityOfStrategy.Minimum = new decimal(numArray2);
            this.numPriorityOfStrategy.Name = "numPriorityOfStrategy";
            this.numPriorityOfStrategy.Size = new Size(0x43, 20);
            this.numPriorityOfStrategy.TabIndex = 4;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.numPriorityOfStrategy.Value = new decimal(numArray3);
            this.numPriorityOfStrategy.ValueChanged += new EventHandler(this.numPriorityOfStrategy_ValueChanged);
            this.lblPriority.AutoSize = true;
            this.lblPriority.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblPriority.Location = new Point(5, 90);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new Size(80, 13);
            this.lblPriority.TabIndex = 0x39;
            this.lblPriority.Text = "Strategy Priority";
            this.numPctEquity.DecimalPlaces = 2;
            this.numPctEquity.ForeColor = SystemColors.ControlText;
            this.numPctEquity.Location = new Point(0x72, 0x2e);
            int[] numArray4 = new int[4];
            numArray4[0] = 1;
            this.numPctEquity.Minimum = new decimal(numArray4);
            this.numPctEquity.Name = "numPctEquity";
            this.numPctEquity.Size = new Size(0x43, 20);
            this.numPctEquity.TabIndex = 3;
            int[] numArray5 = new int[4];
            numArray5[0] = 10;
            this.numPctEquity.Value = new decimal(numArray5);
            this.numPctEquity.ValueChanged += new EventHandler(this.numPctEquity_ValueChanged);
            this.rbPctEquity.Location = new Point(8, 0x26);
            this.rbPctEquity.Name = "rbPctEquity";
            this.rbPctEquity.Size = new Size(0x6c, 0x20);
            this.rbPctEquity.TabIndex = 2;
            this.rbPctEquity.TabStop = true;
            this.rbPctEquity.Text = "Percent of Starting Equity";
            this.rbPctEquity.UseVisualStyleBackColor = true;
            this.rbPctEquity.CheckedChanged += new EventHandler(this.rbDollar_CheckedChanged);
            this.numDollar.ForeColor = SystemColors.ControlText;
            int[] numArray6 = new int[4];
            numArray6[0] = 0x3e8;
            this.numDollar.Increment = new decimal(numArray6);
            this.numDollar.Location = new Point(0x72, 0x13);
            int[] numArray7 = new int[4];
            numArray7[0] = 0x5f5e100;
            this.numDollar.Maximum = new decimal(numArray7);
            int[] numArray8 = new int[4];
            numArray8[0] = 1;
            this.numDollar.Minimum = new decimal(numArray8);
            this.numDollar.Name = "numDollar";
            this.numDollar.Size = new Size(0x44, 20);
            this.numDollar.TabIndex = 1;
            int[] numArray9 = new int[4];
            numArray9[0] = 0x1388;
            this.numDollar.Value = new decimal(numArray9);
            this.numDollar.ValueChanged += new EventHandler(this.numDollar_ValueChanged);
            this.rbDollar.AutoSize = true;
            this.rbDollar.Checked = true;
            this.rbDollar.Location = new Point(8, 0x13);
            this.rbDollar.Name = "rbDollar";
            this.rbDollar.Size = new Size(80, 0x11);
            this.rbDollar.TabIndex = 0;
            this.rbDollar.TabStop = true;
            this.rbDollar.Text = "Fixed Dollar";
            this.rbDollar.UseVisualStyleBackColor = true;
            this.rbDollar.CheckedChanged += new EventHandler(this.rbDollar_CheckedChanged);
            this.browser.AllowWebBrowserDrop = false;
            this.browser.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.browser.Location = new Point(7, 0x199);
            this.browser.MinimumSize = new Size(20, 20);
            this.browser.Name = "browser";
            this.browser.Size = new Size(0x250, 80);
            this.browser.TabIndex = 8;
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new Size(0x61, 0x16);
            this.lblProgress.Text = "Backtest Progress:";
            this.progress.Name = "progress";
            this.progress.Size = new Size(100, 0x16);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(this.browser);
            base.Controls.Add(this.grpAllocations);
            base.Controls.Add(this.grpParameters);
            base.Controls.Add(this.grpDollars);
            base.Controls.Add(this.grpDataSet);
            base.Controls.Add(this.toolStrip3);
            base.Controls.Add(this.toolStrip2);
            base.Controls.Add(this.btnRemove);
            base.Controls.Add(this.btnAdd);
            base.Controls.Add(this.strategyTree);
            base.Controls.Add(this.lvStrategies);
            base.Controls.Add(this.toolStrip1);
            base.Name = "CombinationStrategyBuilder";
            base.Size = new Size(0x325, 0x1f1);
            base.Load += new EventHandler(this.CombinationStrategyBuilder_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.grpDataSet.ResumeLayout(false);
            this.grpDataSet.PerformLayout();
            this.grpDollars.ResumeLayout(false);
            this.grpDollars.PerformLayout();
            this.grpParameters.ResumeLayout(false);
            this.grpAllocations.ResumeLayout(false);
            this.grpAllocations.PerformLayout();
            this.numPriorityOfStrategy.EndInit();
            this.numPctEquity.EndInit();
            this.numDollar.EndInit();
            base.ResumeLayout(false);
        }

        private void lvStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bool_0 = true;
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild == null)
            {
                this.method_7(false);
            }
            else
            {
                this.method_7(true);
                this.rbDollar.Checked = selectedChild.Allocation.Mode == PosSizeMode.Dollar;
                this.rbPctEquity.Checked = !this.rbDollar.Checked;
                this.numDollar.Value = (decimal) selectedChild.Allocation.DollarSize;
                this.numPctEquity.Value = (decimal) selectedChild.Allocation.PctSize;
                this.numPriorityOfStrategy.Value = selectedChild.Priority;
                this.cmbAccounts.SelectedIndex = this.cmbAccounts.Items.IndexOf(selectedChild.AccountNumber);
                this.scale.DataScale = selectedChild.DataScale;
                this.posSize.PositionSize = selectedChild.PositionSize;
                this.cbDefaultDataSet.Checked = selectedChild.UseDefaultDataSet;
                DataSource source = MainModule.Instance.DataSources.FindDataSource(selectedChild.DataSetName);
                if (source == null)
                {
                    this.treeDataSet.SelectedNode = null;
                }
                else if (selectedChild.Symbol == "")
                {
                    this.treeDataSet.SelectDataSource(source);
                }
                else
                {
                    this.treeDataSet.SelectSymbol(source, selectedChild.Symbol);
                }
                if (selectedChild.GetStrategy(MainModule.Instance.Strategies) != null)
                {
                    File.WriteAllText(MainModule.Instance.DataPath + @"\temp.html", selectedChild.GetStrategy(MainModule.Instance.Strategies).Description);
                }
                else
                {
                    File.WriteAllText(MainModule.Instance.DataPath + @"\temp.html", "");
                }
                this.browser.Navigate(MainModule.Instance.DataPath + @"\temp.html");
                this.paramSlidersContainer.WealthScript = (WealthScript) this.SelectedChild.Tag;
                for (int i = 0; i < this.SelectedChild.ParameterValues.Count; i++)
                {
                    this.paramSlidersContainer.WealthScript.Parameters[i].Value = this.SelectedChild.ParameterValues[i];
                }
                this.cbUsePreferredValues.Checked = selectedChild.UsePreferredValues;
                this.paramSlidersContainer.Enabled = !this.cbUsePreferredValues.Checked;
                if (selectedChild.Allocation.Mode == PosSizeMode.Dollar)
                {
                    this.posSize.PositionSize.StartingCapital = selectedChild.Allocation.DollarSize;
                }
                else
                {
                    this.posSize.PositionSize.StartingCapital = this.StartingEquity * (selectedChild.Allocation.PctSize / 100.0);
                }
                this.bool_0 = false;
                this.method_14();
                bool flag = false;
                Strategy iD = MainModule.Instance.Strategies.LookupID(selectedChild.StrategyID.ToString());
                if (iD != null)
                {
                    flag = iD.StrategyType == StrategyType.CombinedStrategy;
                }
                this.grpDollars.Enabled = !flag;
                this.grpParameters.Enabled = !flag;
                this.grpDataSet.Enabled = !flag;
            }
        }

        private void method_0()
        {
            foreach (CombinedStrategyInfo info in this.CombinationStrategy.CombinedStrategyChildren)
            {
                bool flag = false;
                using (IEnumerator<Strategy> enumerator2 = MainModule.Instance.Strategies.Strategies.GetEnumerator())
                {
                    ListViewItem item;
                    Strategy current;
                    while (enumerator2.MoveNext())
                    {
                        current = enumerator2.Current;
                        if (info.StrategyID == current.ID)
                        {
                            ///goto  Label_0065;  ///WYJ fix, simplify the flow
                            item = this.lvStrategies.Items.Add(current.Name);
                            for (int i = 1; i <= this.lvStrategies.Columns.Count; i++)
                            {
                                item.SubItems.Add("");
                            }
                            item.Tag = info;
                            this.method_5(item, info);
                            WealthScript wealthScriptObject = MainModule.Instance.Strategies.GetWealthScriptObject(current);
                            info.Tag = wealthScriptObject;
                            this.cbUsePreferredValues.Checked = info.UsePreferredValues;
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    ListViewItem item2 = this.lvStrategies.Items.Add(info.Name);
                    for (int j = 1; j <= this.lvStrategies.Columns.Count; j++)
                    {
                        item2.SubItems.Add("");
                    }
                    this.method_5(item2, info);
                    item2.Tag = info;
                    this.cbUsePreferredValues.Checked = info.UsePreferredValues;
                }
            }
        }

        private void method_1()
        {
            if (MainModule.Instance.BrokerProvider != null)
            {
                foreach (string str in MainModule.Instance.AccountNumbers)
                {
                    this.cmbAccounts.Items.Add(str);
                }
            }
            this.cmbAccounts.SelectedIndex = this.cmbAccounts.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
            this.cmbAccounts.SelectedIndex = -1;
            this.treeDataSet.Populate(MainModule.Instance.DataSources);
        }

        private void method_10(object sender, EventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.PositionSize = this.posSize.PositionSize;
                    this.method_6();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        private void method_11(object sender, DataSourceEventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.DataSetName = e.DataSource.Name;
                    selectedChild.Symbol = "";
                    this.method_6();
                    this.method_14();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        private void method_12(object sender, DataSourceSymbolEventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.DataSetName = e.DataSource.Name;
                    selectedChild.Symbol = e.Symbol;
                    this.method_6();
                    this.method_14();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        private void method_13(object sender, EventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    if (selectedChild.ParameterValues == null)
                    {
                        selectedChild.ParameterValues = new List<double>();
                    }
                    selectedChild.ParameterValues.Clear();
                    foreach (StrategyParameter parameter in this.paramSlidersContainer.WealthScript.Parameters)
                    {
                        selectedChild.ParameterValues.Add(parameter.Value);
                    }
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        private void method_14()
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (this.cbUsePreferredValues.Checked)
                {
                    if (this.cbDefaultDataSet.Checked)
                    {
                        if (this.chartForm_0.MyMainForm.Symbol == "")
                        {
                            this.paramSlidersContainer.Visible = false;
                        }
                        else
                        {
                            selectedChild.GetStrategy(MainModule.Instance.Strategies).LoadPreferredValues(this.chartForm_0.MyMainForm.Symbol, (WealthScript) selectedChild.Tag);
                            this.paramSlidersContainer.Visible = true;
                        }
                    }
                    else if (selectedChild.Symbol == "")
                    {
                        this.paramSlidersContainer.Visible = false;
                    }
                    else
                    {
                        this.paramSlidersContainer.Visible = true;
                        selectedChild.GetStrategy(MainModule.Instance.Strategies).LoadPreferredValues(selectedChild.Symbol, (WealthScript) selectedChild.Tag);
                    }
                    this.paramSlidersContainer.WealthScript = (WealthScript) selectedChild.Tag;
                }
                else
                {
                    this.paramSlidersContainer.Visible = true;
                }
            }
        }

        private void method_15(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("Introduction.htm");
        }

        private void method_2()
        {
            foreach (string folderName in MainModule.Instance.Strategies.FolderNames)
            {
                bool flag = false;
                IEnumerator enumerator = this.strategyTree.Nodes.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            TreeNode current = (TreeNode)enumerator.Current;
                            if (current.Text == folderName)
                            {
                                flag = true;
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
                if (flag)
                {
                    continue;
                }
                TreeNode treeNode = this.strategyTree.Nodes.Add(folderName);
                treeNode.ImageIndex = 0;
                treeNode.SelectedImageIndex = 1;
                foreach (Strategy strategy in MainModule.Instance.Strategies.Strategies)
                {
                    if (strategy.Folder != folderName)
                    {
                        continue;
                    }
                    TreeNode treeNode1 = treeNode.Nodes.Add(strategy.Name);
                    treeNode1.Tag = strategy;
                    treeNode1.ImageIndex = 5;
                    treeNode1.SelectedImageIndex = 5;
                }
            }
        }

        private void method_3(Strategy strategy_1, List<Strategy> list_0)
        {
            if ((list_0.Count <= 500) && (strategy_1.StrategyType == StrategyType.CombinedStrategy))
            {
                foreach (CombinedStrategyInfo info in strategy_1.CombinedStrategyChildren)
                {
                    Strategy iD = MainModule.Instance.Strategies.LookupID(info.StrategyID.ToString());
                    if ((iD != null) && (iD.StrategyType == StrategyType.CombinedStrategy))
                    {
                        list_0.Add(iD);
                        this.method_3(iD, list_0);
                    }
                }
            }
        }

        private void method_4(Strategy strategy_1)
        {
            using (List<CombinedStrategyInfo>.Enumerator enumerator = this.CombinationStrategy.CombinedStrategyChildren.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    CombinedStrategyInfo current = enumerator.Current;
                    if (strategy_1.ID == current.StrategyID)
                    {
                        ///goto  Label_003A;  ///WYJ fix, simplify the flow
                        MessageBox.Show("Strategy you are trying to add has already been added to Combination Strategy", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                return;
            }
        }

        private void method_5(ListViewItem listViewItem_0, CombinedStrategyInfo combinedStrategyInfo_0)
        {
            listViewItem_0.SubItems.Clear();
            listViewItem_0.Text = combinedStrategyInfo_0.Name;
            listViewItem_0.SubItems.Add(combinedStrategyInfo_0.AccountNumber);
            listViewItem_0.SubItems.Add(combinedStrategyInfo_0.Allocation.Text);
            listViewItem_0.SubItems.Add(combinedStrategyInfo_0.Priority.ToString());
            listViewItem_0.SubItems.Add(combinedStrategyInfo_0.PositionSize.Text);
            string dataSetName = combinedStrategyInfo_0.DataSetName;
            if (combinedStrategyInfo_0.UseDefaultDataSet)
            {
                dataSetName = "(Default)";
            }
            else if (combinedStrategyInfo_0.Symbol != "")
            {
                dataSetName = combinedStrategyInfo_0.Symbol;
            }
            listViewItem_0.SubItems.Add(dataSetName);
            listViewItem_0.SubItems.Add(combinedStrategyInfo_0.DataScale.ToString());
        }

        private void method_6()
        {
            if (this.lvStrategies.SelectedIndices.Count != 0)
            {
                int num = this.lvStrategies.SelectedIndices[0];
                ListViewItem item = this.lvStrategies.Items[num];
                CombinedStrategyInfo tag = (CombinedStrategyInfo) item.Tag;
                this.method_5(item, tag);
            }
        }

        private void method_7(bool bool_1)
        {
            this.grpAllocations.Enabled = bool_1;
            this.grpDataSet.Enabled = bool_1;
            this.grpDollars.Enabled = bool_1;
            this.grpParameters.Enabled = bool_1;
        }

        private void method_8()
        {
            this.CombinationStrategy.CombinedStrategyChildren.Remove(this.SelectedChild);
            int num = this.lvStrategies.SelectedIndices[0];
            this.lvStrategies.Items.Remove(this.lvStrategies.SelectedItems[0]);
            if (num > 0)
            {
                this.lvStrategies.Items[num - 1].Selected = true;
            }
        }

        private void method_9(object sender, EventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.DataScale = this.scale.DataScale;
                    this.method_6();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        private void numDollar_ValueChanged(object sender, EventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.Allocation.DollarSize = (double) this.numDollar.Value;
                    if (selectedChild.Allocation.Mode == PosSizeMode.Dollar)
                    {
                        this.posSize.PositionSize.StartingCapital = selectedChild.Allocation.DollarSize;
                    }
                    else
                    {
                        this.posSize.PositionSize.StartingCapital = this.StartingEquity * (selectedChild.Allocation.PctSize / 100.0);
                    }
                    this.method_6();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        private void numPctEquity_ValueChanged(object sender, EventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.Allocation.PctSize = (double) this.numPctEquity.Value;
                    if (selectedChild.Allocation.Mode == PosSizeMode.Dollar)
                    {
                        this.posSize.PositionSize.StartingCapital = selectedChild.Allocation.DollarSize;
                    }
                    else
                    {
                        this.posSize.PositionSize.StartingCapital = this.StartingEquity * (selectedChild.Allocation.PctSize / 100.0);
                    }
                    this.method_6();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        private void numPriorityOfStrategy_ValueChanged(object sender, EventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.Priority = (int) this.numPriorityOfStrategy.Value;
                    this.method_6();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        private void rbDollar_CheckedChanged(object sender, EventArgs e)
        {
            CombinedStrategyInfo selectedChild = this.SelectedChild;
            if (selectedChild != null)
            {
                if (!this.bool_0)
                {
                    selectedChild.Allocation.Mode = this.rbDollar.Checked ? PosSizeMode.Dollar : PosSizeMode.PctEquity;
                    if (selectedChild.Allocation.Mode == PosSizeMode.Dollar)
                    {
                        this.posSize.PositionSize.StartingCapital = selectedChild.Allocation.DollarSize;
                    }
                    else
                    {
                        this.posSize.PositionSize.StartingCapital = this.StartingEquity * (selectedChild.Allocation.PctSize / 100.0);
                    }
                    this.method_6();
                }
                this.chartForm_0.NeedSave = true;
            }
        }

        public void ResetProgressBar()
        {
            this.progress.Value = 0;
        }

        private void strategyTree_DoubleClick(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.strategyTree.SelectedNode;
            if ((selectedNode != null) && (selectedNode.Tag != null))
            {
                this.btnAdd.PerformClick();
            }
        }

        private void txtMargin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.marginFactor = int.Parse(this.txtMargin.Text);
                if (this.marginFactor <= 0.0)
                {
                    this.marginFactor = 1.0;
                    this.txtMargin.Text = "1";
                }
                this.chartForm_0.Strategy.MarginFactor = this.marginFactor;
                this.chartForm_0.NeedSave = true;
            }
            catch
            {
                if (this.txtMargin.Text != "")
                {
                    this.txtMargin.Text = this.MarginFactor.ToString();
                }
            }
            int[] bits = new int[4];
            bits[0] = 100 * ((int) this.marginFactor);
            this.numPctEquity.Maximum = new decimal(bits);
        }

        private void txtStartingEquity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.startingEquity = int.Parse(this.txtStartingEquity.Text);
                if (this.startingEquity < 0.0)
                {
                    this.startingEquity = 100000.0;
                    this.txtStartingEquity.Text = "100000.00";
                }
                this.chartForm_0.Strategy.StartingEquity = this.startingEquity;
                this.chartForm_0.NeedSave = true;
                this.lvStrategies_SelectedIndexChanged(this, EventArgs.Empty);
            }
            catch
            {
                if (this.txtStartingEquity.Text != "")
                {
                    this.txtStartingEquity.Text = this.StartingEquity.ToString();
                }
            }
        }

        public void UpdateProgressBar()
        {
            this.progress.Value++;
        }

        public Strategy CombinationStrategy
        {
            [CompilerGenerated]
            get
            {
                return this.combinationStrategy;
            }
            [CompilerGenerated]
            set
            {
                this.combinationStrategy = value;
            }
        }

        public double MarginFactor
        {
            get
            {
                return this.marginFactor;
            }
        }

        private CombinedStrategyInfo SelectedChild
        {
            get
            {
                if (this.lvStrategies.SelectedIndices.Count == 0)
                {
                    return null;
                }
                int num = this.lvStrategies.SelectedIndices[0];
                return (CombinedStrategyInfo) this.lvStrategies.Items[num].Tag;
            }
        }

        public double StartingEquity
        {
            get
            {
                return this.startingEquity;
            }
        }
    }
}

