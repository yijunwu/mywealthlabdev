namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Text;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLabPro.Properties;

    [ToolboxItem(false)]
    public class Builder : UserControl
    {
        private bool bool_0 = true;
        private bool bool_1;
        private Button btnAdd;
        private ToolStripButton btnConvert;
        private ToolStripButton btnExecute;
        private ToolStripButton btnOpenNew;
        private Button btnRemove;
        private ToolStripButton btnViewCode;
        private static CultureInfo cultureInfo_0 = new CultureInfo("en-US");
        private GroupBox grpParameters;
        private GroupBox grpPositionManagement;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private static int int_0 = 0x1d;
        private static int int_1 = 0;
        private static int int_2 = 1;
        private static int int_3 = 2;
        private static int int_4 = 3;
        private static int int_5 = 4;
        private static int int_6 = 5;
        private static int int_7 = 6;
        private static int int_8 = 7;
        private static int int_9 = 8;
        private List<StrategyRule> list_0 = new List<StrategyRule>();
        private TabPage pageConditions;
        private TabPage pageEntryExit;
        private Panel pnlParams;
        private RadioButton rbMulti;
        private RadioButton rbSingle;
        private ToolStripSeparator sepConvert;
        private SplitContainer splitContainer1;
        private WealthLab.Strategy strategy_0;
        private StrategyBuilder strategyBuilder_0;
        private TabControl tabBuilder;
        private ToolStrip toolbar;
        private ToolTip toolTip_0;
        private TreeView treeConditions;
        private TreeView treeEntryExits;
        private TreeView treeRules;
        private TextBox txtDescription;
        private WealthScriptCompiler wealthScriptCompiler_0;

        public Builder()
        {
            this.InitializeComponent();
            if (!base.DesignMode && (MainModule.Instance != null))
            {
                this.strategyBuilder_0.RootPath = MainModule.Instance.AppPath;
                this.strategyBuilder_0.LoadAllRules(MainModule.Instance.AppPath + @"\Data\Rules");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode;
            if (this.tabBuilder.SelectedIndex == 0)
            {
                selectedNode = this.treeEntryExits.SelectedNode;
            }
            else
            {
                selectedNode = this.treeConditions.SelectedNode;
            }
            if (selectedNode != null)
            {
                Rule tag = (Rule) selectedNode.Tag;
                if (tag != null)
                {
                    if (tag.IsEntryExit)
                    {
                        this.treeRules.SelectedNode = null;
                    }
                    this.method_1(selectedNode);
                }
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("If you convert this Strategy to a Code-Based Strategy, you will no longer be able to modify it using the Strategy Builder.  You will have to make changes in the Code Editor.  Do you want to proceed?", "Convert to Code Based Strategy", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (!this.CompileRules())
                {
                    MessageBox.Show("Please be sure you have created a valid Strategy before converting it.");
                }
                else
                {
                    this.Strategy.Code = this.GenerateCode();
                    this.Strategy.StrategyType = StrategyType.Script;
                    if (this.MyChartForm.SaveStrategy())
                    {
                        this.MyChartForm.MyMainForm.CloseAndReopenStrategy(this.MyChartForm, this.MyChartForm.Strategy);
                    }
                    else
                    {
                        this.Strategy.StrategyType = StrategyType.Rules;
                    }
                }
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            this.treeRules.Focus();
            this.MyChartForm.GoButtonPressed(this.MyChartForm.Symbol, true);
            this.MyChartForm.MyMainForm.BuildParameterSliders();
            if (!this.bool_1)
            {
                this.MyChartForm.SelectTab("Chart");
            }
        }

        private void btnOpenNew_Click(object sender, EventArgs e)
        {
            ChartForm form2 = (base.ParentForm.ParentForm as MainForm).CreateNewStrategyWindow(true);
            form2.EditorCode = this.GenerateCode();
            form2.Show();
            form2.BringToFront();
            form2.CompileSource();
            form2.SelectTab("Editor");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.treeRules.SelectedNode;
            if (selectedNode != null)
            {
                this.treeRules.Nodes.Remove(selectedNode);
                this.bool_0 = true;
                this.MyChartForm.NeedSave = true;
            }
        }

        private void btnViewCode_Click(object sender, EventArgs e)
        {
            string str = this.GenerateCode();
            new WizardCodeForm { Code = str }.ShowDialog(this);
        }

        private void Builder_Load(object sender, EventArgs e)
        {
            this.treeEntryExits.BeginUpdate();
            this.treeConditions.BeginUpdate();
            Rule rule = new Rule {
                RuleType = RuleType.OrDivider,
                Name = "OR Divider"
            };
            TreeNode node = this.treeConditions.Nodes.Add("OR Divider");
            node.ImageIndex = int_6;
            node.SelectedImageIndex = node.ImageIndex;
            node.Tag = rule;
            Rule rule2 = new Rule {
                RuleType = RuleType.MultiCondition,
                Name = "Multi-Condition",
                Description = "Group a set of conditions for the Entry/Exit rule. Specify the required number of these conditions that must be true to make the condition true and specify the period to lookback over to check if the conditions were true."
            };
            List<RuleParameter> list = new List<RuleParameter>();
            RuleParameter item = new RuleParameter {
                DefaultValue = "1",
                Value = "1",
                Name = "Required Conditions",
                ParamType = RuleParamType.Integer,
                Start = 1.0,
                Stop = 20.0,
                Step = 1.0
            };
            list.Add(item);
            item = new RuleParameter {
                DefaultValue = "1",
                Value = "1",
                Name = "Lookback Period",
                ParamType = RuleParamType.Integer,
                Start = 1.0,
                Stop = 20.0,
                Step = 1.0
            };
            list.Add(item);
            rule2.Parameters = list;
            node = this.treeConditions.Nodes.Add("Multi-Condition Group");
            node.ImageIndex = int_9;
            node.SelectedImageIndex = node.ImageIndex;
            node.Tag = rule2;
            foreach (Rule rule3 in this.strategyBuilder_0.Rules)
            {
                if (rule3.RuleType == RuleType.Condition)
                {
                    node = this.method_0(this.treeConditions, rule3.Category);
                }
                else
                {
                    node = this.method_0(this.treeEntryExits, rule3.Category);
                }
                TreeNode node3 = this.method_5(rule3);
                node.Nodes.Add(node3);
            }
            foreach (TreeNode node2 in this.treeEntryExits.Nodes)
            {
                if ((node2.Text == "Basic Entries (Long)") || (node2.Text == "Basic Exits (Long)"))
                {
                    node2.Expand();
                }
            }
            this.treeEntryExits.EndUpdate();
            this.treeConditions.EndUpdate();
        }

        public bool CompileIfNeeded()
        {
            bool flag;
            if (!this.bool_0)
            {
                return true;
            }
            if (flag = this.CompileRules())
            {
                this.MyChartForm.MyMainForm.BuildParameterSliders();
            }
            return flag;
        }

        public bool CompileRules()
        {
            bool flag3;
            this.bool_1 = false;
            string str = "";
            try
            {
                str = this.GenerateCode();
                bool flag = str.Contains("[Click here to select an I");
                bool flag2 = str.Contains("[Click here to select a F");
                if (flag && flag2)
                {
                    MessageBox.Show("Please select Indicators and Fundamental items for your Rule parameters");
                    this.bool_1 = true;
                    return false;
                }
                if (flag)
                {
                    MessageBox.Show("Please select Indicators for your Rule parameters");
                    this.bool_1 = true;
                    return false;
                }
                if (flag2)
                {
                    MessageBox.Show("Please select Fundamental items for your Rule parameters");
                    this.bool_1 = true;
                    return false;
                }
                this.wealthScriptCompiler_0.SourceCode = str;
                this.MyChartForm.WealthScript = this.wealthScriptCompiler_0.CompileSource("");
                if (this.wealthScriptCompiler_0.CompilerErrors.Count > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine("There were errors attempting to compile the Strategy:");
                    for (int i = 0; i < this.wealthScriptCompiler_0.CompilerErrors.Count; i++)
                    {
                        string str2 = this.wealthScriptCompiler_0.CompilerErrors[i].ToString();
                        builder.AppendLine(str2);
                        this.bool_1 = true;
                    }
                    MessageBox.Show(builder.ToString());
                    return false;
                }
                this.MyChartForm.ReloadOptimizationParameters();
                this.bool_0 = false;
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error generating code from Rules: " + exception.Message);
                this.bool_1 = true;
                flag3 = false;
            }
            return flag3;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GenerateCode()
        {
            this.list_0.Clear();
            foreach (TreeNode node in this.treeRules.Nodes)
            {
                if (node.Level == 0)
                {
                    StrategyRule tag = (StrategyRule) node.Tag;
                    tag.Conditions.Clear();
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        Rule item = (Rule) node2.Tag;
                        tag.Conditions.Add(item);
                    }
                    this.list_0.Add(tag);
                }
            }
            return this.strategyBuilder_0.BuildCode(this.list_0, this.rbSingle.Checked);
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Builder));
            this.tabBuilder = new TabControl();
            this.pageEntryExit = new TabPage();
            this.treeEntryExits = new TreeView();
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.pageConditions = new TabPage();
            this.treeConditions = new TreeView();
            this.treeRules = new TreeView();
            this.grpParameters = new GroupBox();
            this.pnlParams = new Panel();
            this.grpPositionManagement = new GroupBox();
            this.rbMulti = new RadioButton();
            this.rbSingle = new RadioButton();
            this.toolbar = new ToolStrip();
            this.btnExecute = new ToolStripButton();
            this.btnViewCode = new ToolStripButton();
            this.btnOpenNew = new ToolStripButton();
            this.sepConvert = new ToolStripSeparator();
            this.btnConvert = new ToolStripButton();
            this.btnAdd = new Button();
            this.btnRemove = new Button();
            this.txtDescription = new TextBox();
            this.splitContainer1 = new SplitContainer();
            this.toolTip_0 = new ToolTip(this.icontainer_0);
            this.strategyBuilder_0 = new StrategyBuilder(this.icontainer_0);
            this.wealthScriptCompiler_0 = new WealthScriptCompiler(this.icontainer_0);
            this.tabBuilder.SuspendLayout();
            this.pageEntryExit.SuspendLayout();
            this.pageConditions.SuspendLayout();
            this.grpParameters.SuspendLayout();
            this.grpPositionManagement.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            base.SuspendLayout();
            this.tabBuilder.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabBuilder.Controls.Add(this.pageEntryExit);
            this.tabBuilder.Controls.Add(this.pageConditions);
            this.tabBuilder.Location = new Point(0, 0);
            this.tabBuilder.Name = "tabBuilder";
            this.tabBuilder.SelectedIndex = 0;
            this.tabBuilder.Size = new Size(0x111, 0x148);
            this.tabBuilder.TabIndex = 1;
            this.pageEntryExit.Controls.Add(this.treeEntryExits);
            this.pageEntryExit.Location = new Point(4, 0x16);
            this.pageEntryExit.Name = "pageEntryExit";
            this.pageEntryExit.Padding = new Padding(3);
            this.pageEntryExit.Size = new Size(0x109, 0x12e);
            this.pageEntryExit.TabIndex = 0;
            this.pageEntryExit.Text = "Entries and Exits";
            this.pageEntryExit.UseVisualStyleBackColor = true;
            this.treeEntryExits.AllowDrop = true;
            this.treeEntryExits.Dock = DockStyle.Fill;
            this.treeEntryExits.HideSelection = false;
            this.treeEntryExits.ImageIndex = 0;
            this.treeEntryExits.ImageList = this.imageList_0;
            this.treeEntryExits.Location = new Point(3, 3);
            this.treeEntryExits.Name = "treeEntryExits";
            this.treeEntryExits.SelectedImageIndex = 0;
            this.treeEntryExits.Size = new Size(0x103, 0x128);
            this.treeEntryExits.TabIndex = 0;
            this.treeEntryExits.DoubleClick += new EventHandler(this.btnAdd_Click);
            this.treeEntryExits.DragDrop += new DragEventHandler(this.treeConditions_DragDrop);
            this.treeEntryExits.AfterSelect += new TreeViewEventHandler(this.treeConditions_AfterSelect);
            this.treeEntryExits.DragEnter += new DragEventHandler(this.treeRules_DragEnter);
            this.treeEntryExits.ItemDrag += new ItemDragEventHandler(this.treeEntryExits_ItemDrag);
            this.treeEntryExits.DragOver += new DragEventHandler(this.treeRules_DragEnter);
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.White;
            this.imageList_0.Images.SetKeyName(0, "BuyArrow.bmp");
            this.imageList_0.Images.SetKeyName(1, "SellArrow.bmp");
            this.imageList_0.Images.SetKeyName(2, "ShortArrow.bmp");
            this.imageList_0.Images.SetKeyName(3, "CoverArrow.bmp");
            this.imageList_0.Images.SetKeyName(4, "Condition.bmp");
            this.imageList_0.Images.SetKeyName(5, "Or.bmp");
            this.imageList_0.Images.SetKeyName(6, "FolderOpen.bmp");
            this.imageList_0.Images.SetKeyName(7, "FolderClosed.bmp");
            this.imageList_0.Images.SetKeyName(8, "MultiCondition.bmp");
            this.pageConditions.Controls.Add(this.treeConditions);
            this.pageConditions.Location = new Point(4, 0x16);
            this.pageConditions.Name = "pageConditions";
            this.pageConditions.Padding = new Padding(3);
            this.pageConditions.Size = new Size(0x109, 0x12e);
            this.pageConditions.TabIndex = 1;
            this.pageConditions.Text = "Conditions";
            this.pageConditions.UseVisualStyleBackColor = true;
            this.treeConditions.AllowDrop = true;
            this.treeConditions.Dock = DockStyle.Fill;
            this.treeConditions.HideSelection = false;
            this.treeConditions.ImageIndex = 0;
            this.treeConditions.ImageList = this.imageList_0;
            this.treeConditions.Location = new Point(3, 3);
            this.treeConditions.Name = "treeConditions";
            this.treeConditions.SelectedImageIndex = 0;
            this.treeConditions.Size = new Size(0x103, 0x128);
            this.treeConditions.TabIndex = 0;
            this.treeConditions.DoubleClick += new EventHandler(this.btnAdd_Click);
            this.treeConditions.DragDrop += new DragEventHandler(this.treeConditions_DragDrop);
            this.treeConditions.AfterSelect += new TreeViewEventHandler(this.treeConditions_AfterSelect);
            this.treeConditions.DragEnter += new DragEventHandler(this.treeRules_DragEnter);
            this.treeConditions.ItemDrag += new ItemDragEventHandler(this.treeConditions_ItemDrag);
            this.treeConditions.DragOver += new DragEventHandler(this.treeRules_DragEnter);
            this.treeRules.AllowDrop = true;
            this.treeRules.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeRules.HideSelection = false;
            this.treeRules.ImageIndex = 0;
            this.treeRules.ImageList = this.imageList_0;
            this.treeRules.Location = new Point(0x1f, 3);
            this.treeRules.Name = "treeRules";
            this.treeRules.SelectedImageIndex = 0;
            this.treeRules.Size = new Size(0x158, 0xcb);
            this.treeRules.TabIndex = 3;
            this.treeRules.DragDrop += new DragEventHandler(this.treeRules_DragDrop);
            this.treeRules.AfterSelect += new TreeViewEventHandler(this.treeRules_AfterSelect);
            this.treeRules.DragEnter += new DragEventHandler(this.treeRules_DragEnter);
            this.treeRules.KeyDown += new KeyEventHandler(this.treeRules_KeyDown);
            this.treeRules.ItemDrag += new ItemDragEventHandler(this.treeRules_ItemDrag);
            this.treeRules.DragOver += new DragEventHandler(this.treeRules_DragOver);
            this.grpParameters.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.grpParameters.Controls.Add(this.pnlParams);
            this.grpParameters.Location = new Point(0x1f, 0xd4);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new Size(0x158, 0x88);
            this.grpParameters.TabIndex = 4;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Parameters (change Rule Parameters below)";
            this.pnlParams.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pnlParams.AutoScroll = true;
            this.pnlParams.Location = new Point(7, 20);
            this.pnlParams.Name = "pnlParams";
            this.pnlParams.Size = new Size(0x14b, 110);
            this.pnlParams.TabIndex = 0;
            this.pnlParams.Resize += new EventHandler(this.pnlParams_Resize);
            this.grpPositionManagement.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.grpPositionManagement.Controls.Add(this.rbMulti);
            this.grpPositionManagement.Controls.Add(this.rbSingle);
            this.grpPositionManagement.Location = new Point(0x1f, 0x162);
            this.grpPositionManagement.Name = "grpPositionManagement";
            this.grpPositionManagement.Size = new Size(0x158, 0x2e);
            this.grpPositionManagement.TabIndex = 5;
            this.grpPositionManagement.TabStop = false;
            this.grpPositionManagement.Text = "Position Management";
            this.rbMulti.AutoSize = true;
            this.rbMulti.Location = new Point(0x86, 20);
            this.rbMulti.Name = "rbMulti";
            this.rbMulti.Size = new Size(0xac, 0x11);
            this.rbMulti.TabIndex = 1;
            this.rbMulti.TabStop = true;
            this.rbMulti.Text = "Multiple open Positions allowed";
            this.rbMulti.UseVisualStyleBackColor = true;
            this.rbMulti.CheckedChanged += new EventHandler(this.rbSingle_CheckedChanged);
            this.rbSingle.AutoSize = true;
            this.rbSingle.Checked = true;
            this.rbSingle.Location = new Point(7, 20);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.Size = new Size(0x79, 0x11);
            this.rbSingle.TabIndex = 0;
            this.rbSingle.TabStop = true;
            this.rbSingle.Text = "Single open Position";
            this.rbSingle.UseVisualStyleBackColor = true;
            this.rbSingle.CheckedChanged += new EventHandler(this.rbSingle_CheckedChanged);
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.btnExecute, this.btnViewCode, this.btnOpenNew, this.sepConvert, this.btnConvert });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x293, 0x19);
            this.toolbar.TabIndex = 6;
            this.toolbar.Text = "toolStrip1";
            this.btnExecute.Image = (Image) manager.GetObject("btnExecute.Image");
            this.btnExecute.ImageTransparentColor = Color.Magenta;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new Size(0x75, 0x16);
            this.btnExecute.Text = "Run the Strategy";
            this.btnExecute.Click += new EventHandler(this.btnExecute_Click);
            this.btnViewCode.Image = (Image) manager.GetObject("btnViewCode.Image");
            this.btnViewCode.ImageTransparentColor = Color.Magenta;
            this.btnViewCode.Name = "btnViewCode";
            this.btnViewCode.Size = new Size(0x84, 0x16);
            this.btnViewCode.Text = "View Strategy Code";
            this.btnViewCode.Click += new EventHandler(this.btnViewCode_Click);
            this.btnOpenNew.Image = (Image) manager.GetObject("btnOpenNew.Image");
            this.btnOpenNew.ImageTransparentColor = Color.Magenta;
            this.btnOpenNew.Name = "btnOpenNew";
            this.btnOpenNew.Size = new Size(0xde, 0x16);
            this.btnOpenNew.Text = "Open Code in new Strategy Window";
            this.btnOpenNew.Click += new EventHandler(this.btnOpenNew_Click);
            this.sepConvert.Name = "sepConvert";
            this.sepConvert.Size = new Size(6, 0x19);
            this.btnConvert.Image = (Image) manager.GetObject("btnConvert.Image");
            this.btnConvert.ImageTransparentColor = Color.Magenta;
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new Size(200, 20);
            this.btnConvert.Text = "Convert to Code-Based Strategy";
            this.btnConvert.Click += new EventHandler(this.btnConvert_Click);
            this.btnAdd.Location = new Point(3, 0x2d);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(0x17, 0x17);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.btnRemove.Location = new Point(3, 0x4a);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(0x17, 0x17);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            this.txtDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.txtDescription.BackColor = SystemColors.Info;
            this.txtDescription.Location = new Point(0, 0x14e);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(0x111, 0x42);
            this.txtDescription.TabIndex = 9;
            this.splitContainer1.BackColor = SystemColors.ButtonShadow;
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0x19);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.BackColor = SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.txtDescription);
            this.splitContainer1.Panel1.Controls.Add(this.tabBuilder);
            this.splitContainer1.Panel2.BackColor = SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.grpParameters);
            this.splitContainer1.Panel2.Controls.Add(this.btnAdd);
            this.splitContainer1.Panel2.Controls.Add(this.btnRemove);
            this.splitContainer1.Panel2.Controls.Add(this.treeRules);
            this.splitContainer1.Panel2.Controls.Add(this.grpPositionManagement);
            this.splitContainer1.Size = new Size(0x293, 0x193);
            this.splitContainer1.SplitterDistance = 0x114;
            this.splitContainer1.TabIndex = 1;
            this.toolTip_0.ShowAlways = true;
            this.strategyBuilder_0.RootPath = "";
            this.wealthScriptCompiler_0.SourceCode = null;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.splitContainer1);
            base.Controls.Add(this.toolbar);
            base.Name = "Builder";
            base.Size = new Size(0x293, 0x1ac);
            base.Load += new EventHandler(this.Builder_Load);
            this.tabBuilder.ResumeLayout(false);
            this.pageEntryExit.ResumeLayout(false);
            this.pageConditions.ResumeLayout(false);
            this.grpParameters.ResumeLayout(false);
            this.grpPositionManagement.ResumeLayout(false);
            this.grpPositionManagement.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private TreeNode method_0(TreeView treeView_0, string string_0)
        {
            foreach (TreeNode node in treeView_0.Nodes)
            {
                if (node.Level != 0 || !(node.Text == string_0))
                {
                    continue;
                }
                TreeNode treeNode = node;
                return treeNode;
            }
            TreeNode int8 = treeView_0.Nodes.Add(string_0);
            int8.ImageIndex = Builder.int_8;
            int8.SelectedImageIndex = Builder.int_7;
            return int8;
        }

        private void method_1(TreeNode treeNode_0)
        {
            if (treeNode_0 != null)
            {
                Rule tag = (Rule) treeNode_0.Tag;
                if (tag != null)
                {
                    bool flag = treeNode_0.TreeView == this.treeRules;
                    TreeNode parent = null;
                    TreeNode selectedNode = this.treeRules.SelectedNode;
                    if ((tag.RuleType != RuleType.OrDivider) && (tag.RuleType != RuleType.MultiCondition))
                    {
                        if (tag.RuleType != RuleType.Condition)
                        {
                            if (selectedNode == null)
                            {
                                parent = this.treeRules.Nodes.Add(treeNode_0.Text);
                            }
                            else
                            {
                                if (selectedNode.Level == 1)
                                {
                                    selectedNode = this.method_3(selectedNode);
                                }
                                if (selectedNode == null)
                                {
                                    parent = this.treeRules.Nodes.Add(treeNode_0.Text);
                                }
                                else
                                {
                                    parent = this.treeRules.Nodes.Insert(selectedNode.Index, treeNode_0.Text);
                                }
                            }
                        }
                        else
                        {
                            if (selectedNode == null)
                            {
                                selectedNode = this.method_2();
                            }
                            if (selectedNode == null)
                            {
                                MessageBox.Show("Conditions must be dropped on an Entry or Exit");
                                return;
                            }
                            if (selectedNode.Level == 1)
                            {
                                parent = selectedNode.Parent.Nodes.Insert(selectedNode.Index, treeNode_0.Text);
                            }
                            else
                            {
                                parent = selectedNode.Nodes.Add(treeNode_0.Text);
                            }
                        }
                    }
                    else
                    {
                        if (selectedNode == null)
                        {
                            selectedNode = this.method_2();
                        }
                        if (selectedNode == null)
                        {
                            return;
                        }
                        if (selectedNode.Level == 1)
                        {
                            parent = selectedNode.Parent.Nodes.Insert(selectedNode.Index, treeNode_0.Text);
                        }
                        else
                        {
                            parent = selectedNode.Nodes.Add(treeNode_0.Text);
                        }
                    }
                    if (parent != null)
                    {
                        Rule rule;
                        if (tag.IsEntryExit)
                        {
                            rule = new StrategyRule(tag);
                        }
                        else
                        {
                            rule = new Rule(tag);
                        }
                        parent.Tag = rule;
                        parent.ImageIndex = treeNode_0.ImageIndex;
                        parent.SelectedImageIndex = treeNode_0.SelectedImageIndex;
                    }
                    this.treeRules.SelectedNode = parent;
                    if (flag)
                    {
                        this.treeRules.Nodes.Remove(treeNode_0);
                        foreach (TreeNode node3 in treeNode_0.Nodes)
                        {
                            parent.Nodes.Add(node3);
                        }
                    }
                    if (parent.Level == 1)
                    {
                        parent = parent.Parent;
                    }
                    parent.Expand();
                    this.bool_0 = true;
                    this.MyChartForm.NeedSave = true;
                }
            }
        }

        private void method_10(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            RuleFundamentalsForm form = new RuleFundamentalsForm(box.Text);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                box.Text = form.ItemName;
                this.bool_0 = true;
                this.MyChartForm.NeedSave = true;
            }
        }

        private void method_11(object sender, EventArgs e)
        {
            Button button = sender as Button;
            RuleParameter tag = button.Tag as RuleParameter;
            tag.ExposeAsSlider = !tag.ExposeAsSlider;
            if (tag.ExposeAsSlider)
            {
                button.BackColor = Color.Red;
            }
            else
            {
                button.BackColor = Color.FromKnownColor(KnownColor.Control);
            }
            this.bool_0 = true;
            try
            {
                this.CompileRules();
            }
            catch (Exception)
            {
            }
            this.MyChartForm.MyMainForm.BuildParameterSliders();
            this.MyChartForm.ReloadOptimizationParameters();
        }

        private bool method_12(RuleParameter ruleParameter_0)
        {
            IndicatorHelper helper = this.strategyBuilder_0.FindRuleIndicatorHelper(ruleParameter_0);
            if (helper != null)
            {
                foreach (object obj2 in helper.ParameterDefaultValues)
                {
                    if ((obj2 is RangeBoundDouble) || (obj2 is RangeBoundInt32))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void method_13(object sender, EventArgs e)
        {
            LinkLabel label = sender as LinkLabel;
            RuleParameter tag = label.Tag as RuleParameter;
            InputForm form = new InputForm("Rename Rule", "Rule Name:", tag.DisplayName, CharacterCasing.Normal);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                tag.DisplayName = form.Input;
                label.Text = form.Input;
                try
                {
                    this.CompileRules();
                }
                catch (Exception)
                {
                }
                this.MyChartForm.MyMainForm.BuildParameterSliders();
                this.MyChartForm.ReloadOptimizationParameters();
            }
        }

        private TreeNode method_2()
        {
            for (int i = this.treeRules.Nodes.Count - 1; i >= 0; i--)
            {
                TreeNode node = this.treeRules.Nodes[i];
                if (node.Level == 0)
                {
                    return node;
                }
            }
            return null;
        }

        private TreeNode method_3(TreeNode treeNode_0)
        {
            for (int i = treeNode_0.Index + 1; i < this.treeRules.Nodes.Count; i++)
            {
                TreeNode node = this.treeRules.Nodes[i];
                if (node.Level == 0)
                {
                    return node;
                }
            }
            return null;
        }

        private void method_4()
        {
            this.treeRules.BeginUpdate();
            this.treeRules.Nodes.Clear();
            foreach (StrategyRule rule in this.Strategy.Rules)
            {
                StrategyRule rule2 = new StrategyRule(rule);
                TreeNode node = this.method_5(rule2);
                this.treeRules.Nodes.Add(node);
                foreach (Rule rule3 in rule.Conditions)
                {
                    Rule rule4 = new Rule(rule3);
                    TreeNode node2 = this.method_5(rule4);
                    node.Nodes.Add(node2);
                }
            }
            this.treeRules.ExpandAll();
            this.treeRules.EndUpdate();
            this.rbSingle.Checked = this.Strategy.SinglePosition;
            this.rbMulti.Checked = !this.rbSingle.Checked;
            this.bool_0 = true;
        }

        private TreeNode method_5(Rule rule_0)
        {
            TreeNode node = new TreeNode(rule_0.Name) {
                Tag = rule_0
            };
            switch (rule_0.RuleType)
            {
                case RuleType.LongEntry:
                    node.ImageIndex = int_1;
                    break;

                case RuleType.LongExit:
                    node.ImageIndex = int_2;
                    break;

                case RuleType.ShortEntry:
                    node.ImageIndex = int_3;
                    break;

                case RuleType.ShortExit:
                    node.ImageIndex = int_4;
                    break;

                case RuleType.Condition:
                    node.ImageIndex = int_5;
                    break;

                case RuleType.OrDivider:
                    node.ImageIndex = int_6;
                    break;

                case RuleType.MultiCondition:
                    node.ImageIndex = int_9;
                    break;
            }
            node.SelectedImageIndex = node.ImageIndex;
            return node;
        }

        private void method_6(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                RuleParameter tag = control.Tag as RuleParameter;
                if (tag != null)
                {
                    tag.Value = control.Text;
                    this.MyChartForm.NeedSave = true;
                    this.bool_0 = true;
                }
            }
        }

        private void method_7(object sender, EventArgs e)
        {
            NumericUpDown down = sender as NumericUpDown;
            if (down != null)
            {
                RuleParameter tag = down.Tag as RuleParameter;
                if (tag != null)
                {
                    tag.Value = down.Value.ToString(cultureInfo_0);
                    this.MyChartForm.NeedSave = true;
                    this.bool_0 = true;
                }
            }
        }

        private void method_8(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;
            if (box != null)
            {
                RuleParameter tag = box.Tag as RuleParameter;
                if (tag != null)
                {
                    if (box.Checked)
                    {
                        tag.Value = "true";
                    }
                    else
                    {
                        tag.Value = "false";
                    }
                    this.MyChartForm.NeedSave = true;
                    this.bool_0 = true;
                }
            }
        }

        private void method_9(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            RuleParameter tag = box.Tag as RuleParameter;
            RuleIndicatorsForm form = new RuleIndicatorsForm(box.Text) {
                IndicatorHelpers = this.strategyBuilder_0.IndicatorHelpers
            };
            IndicatorHelper helper = this.strategyBuilder_0.FindRuleIndicatorHelper(tag);
            if (helper != null)
            {
                helper.ParameterDisplayNames.Clear();
                foreach (string str2 in tag.IndicatorParameterDisplayNames)
                {
                    helper.ParameterDisplayNames.Add(str2);
                }
                if (helper.ParameterDisplayNames.Count < helper.ParameterDescriptions.Count)
                {
                    helper.ParameterDisplayNames.Clear();
                    foreach (string str3 in helper.ParameterDescriptions)
                    {
                        helper.ParameterDisplayNames.Add(str3);
                    }
                }
            }
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                box.Text = form.IndicatorString;
                this.bool_0 = true;
                this.MyChartForm.NeedSave = true;
                if (!this.method_12(tag))
                {
                    tag.ExposeAsSlider = false;
                }
                this.treeRules_AfterSelect(this, new TreeViewEventArgs(this.treeRules.SelectedNode));
                tag.IndicatorParameterDisplayNames.Clear();
                foreach (string str in form.IndicatorHelper.ParameterDisplayNames)
                {
                    tag.IndicatorParameterDisplayNames.Add(str);
                }
                try
                {
                    this.CompileRules();
                }
                catch (Exception)
                {
                }
                this.MyChartForm.MyMainForm.BuildParameterSliders();
                this.MyChartForm.ReloadOptimizationParameters();
            }
        }

        private void pnlParams_Resize(object sender, EventArgs e)
        {
            int num = 0;
            while (num < (this.pnlParams.Controls.Count - 1))
            {
                num++;
                num++;
                Control control = this.pnlParams.Controls[num];
                int num2 = 0x62;
                if ((num + 1) < this.pnlParams.Controls.Count)
                {
                    if (this.pnlParams.Controls[num + 1].Width == 2)
                    {
                        num2 = 2;
                    }
                    else if (this.pnlParams.Width < 0x139)
                    {
                        num2 = (this.pnlParams.Width - 0x75) / 2;
                    }
                }
                control.Width = this.pnlParams.Width - (0x75 + num2);
                num++;
                if (num < this.pnlParams.Controls.Count)
                {
                    control = this.pnlParams.Controls[num];
                    control.Left = this.pnlParams.Width - (5 + num2);
                    control.Width = num2;
                    num++;
                }
            }
        }

        public void PushRulesToStrategy(WealthLab.Strategy strategy_1)
        {
            strategy_1.Rules.Clear();
            foreach (TreeNode node in this.treeRules.Nodes)
            {
                if (node.Level == 0)
                {
                    StrategyRule tag = (StrategyRule) node.Tag;
                    tag.Conditions.Clear();
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        Rule item = (Rule) node2.Tag;
                        tag.Conditions.Add(item);
                    }
                    strategy_1.Rules.Add(tag);
                }
            }
            strategy_1.SinglePosition = this.rbSingle.Checked;
        }

        private void rbSingle_CheckedChanged(object sender, EventArgs e)
        {
            this.bool_0 = true;
            if (this.MyChartForm != null)
            {
                this.MyChartForm.NeedSave = true;
            }
        }

        private void treeConditions_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((e.Node != null) && (e.Node.Level == 1))
            {
                Rule tag = (Rule) e.Node.Tag;
                this.txtDescription.Text = tag.Description;
            }
            else
            {
                this.txtDescription.Text = "";
            }
        }

        private void treeConditions_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode data = (TreeNode) e.Data.GetData(typeof(TreeNode));
            if (data.TreeView == this.treeRules)
            {
                this.bool_0 = true;
                this.MyChartForm.NeedSave = true;
                TreeNode nextNode = new TreeNode();
                if (data.NextNode != null)
                {
                    nextNode = data.NextNode;
                }
                else
                {
                    nextNode = data.Parent;
                }
                this.treeRules.Nodes.Remove(data);
                this.treeRules.SelectedNode = nextNode;
            }
        }

        private void treeConditions_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode item = (TreeNode) e.Item;
            if (item.Tag != null)
            {
                this.treeConditions.DoDragDrop(item, DragDropEffects.Copy);
            }
        }

        private void treeEntryExits_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode item = (TreeNode) e.Item;
            if (item.Level == 1)
            {
                this.treeEntryExits.DoDragDrop(item, DragDropEffects.Copy);
            }
        }

        private void treeRules_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            this.pnlParams.Controls.Clear();
            if (node == null)
            {
                return;
            }
            Rule tag = (Rule) node.Tag;
            if (tag == null)
            {
                return;
            }
            int num = 0;
            using (List<RuleParameter>.Enumerator enumerator = tag.Parameters.GetEnumerator())
            {
                NumericUpDown down;
                ComboBox box6;
                string[] strArray2;
                int num2;
                string str;
                int num3;
                ///Label_003E:  ///WYJ fix, simplify the flow
                while (enumerator.MoveNext())
                {
                    RuleParameter current = enumerator.Current;
                    Button button = new Button {
                        FlatStyle = FlatStyle.Flat,
                        Width = int_0,
                        Height = 0x10,
                        Left = 4,
                        Top = num
                    };
                    this.pnlParams.Controls.Add(button);
                    button.Tag = current;
                    button.Image = Resources.Slider;
                    if (current.ExposeAsSlider)
                    {
                        button.BackColor = Color.Red;
                    }
                    else
                    {
                        button.BackColor = Color.FromKnownColor(KnownColor.Control);
                    }
                    if (current.ParamType == RuleParamType.Indicator)
                    {
                        this.toolTip_0.SetToolTip(button, "Click to expose this indicator's parameters as sliders in the Strategy Parameters window");
                    }
                    else
                    {
                        this.toolTip_0.SetToolTip(button, "Click to expose parameter as slider in the Strategy Parameters window");
                    }
                    button.Visible = ((current.ParamType == RuleParamType.Integer) || (current.ParamType == RuleParamType.Float)) ? (((current.Start != 0.0) || (current.Stop != 0.0)) ? (current.Step != 0.0) : false) : false;
                    if ((current.ParamType == RuleParamType.Indicator) && this.method_12(current))
                    {
                        button.Visible = true;
                    }
                    button.Tag = current;
                    button.Click += new EventHandler(this.method_11);
                    Label label = new LinkLabel {
                        AutoSize = false,
                        AutoEllipsis = true,
                        Width = ((0x61 - int_0) - 4) + 40,
                        Height = 13,
                        Left = (4 + int_0) + 4,
                        Top = num,
                        Text = current.DisplayName
                    };
                    label.Click += new EventHandler(this.method_13);
                    label.Tag = current;
                    this.pnlParams.Controls.Add(label);
                    Control control = null;
                    switch (current.ParamType)
                    {
                        case RuleParamType.Integer:
                        case RuleParamType.Float:
                            down = new NumericUpDown();
                            if (current.ParamType == RuleParamType.Float)
                            {
                                down.DecimalPlaces = 2;
                            }
                            down.Minimum = -79228162514264337593543950335M;
                            down.Maximum = 79228162514264337593543950335M;
                            if (current.Step == 0.0)
                            {
                                down.Increment = 1M;
                            }
                            else
                            {
                                down.Increment = (decimal) current.Step;
                            }
                            try
                            {
                                down.Value = (decimal) double.Parse(current.Value, cultureInfo_0);
                            }
                            catch
                            {
                                down.Value = (decimal) double.Parse(current.DefaultValue, cultureInfo_0);
                            }
                            ///goto  Label_06CB;  ///WYJ fix, simplify the flow
                            down.ValueChanged += new EventHandler(this.method_7);
                            control = down;
                            //goto  Label_05CB;
                            break;

                        case RuleParamType.String:
                        {
                            TextBox box = new TextBox {
                                Text = current.Value
                            };
                            box.TextChanged += new EventHandler(this.method_6);
                            control = box;
                            break;
                        }
                        case RuleParamType.Boolean:
                        {
                            CheckBox box4 = new CheckBox {
                                Text = label.Text
                            };
                            label.Text = "";
                            box4.Checked = current.Value == "true";
                            box4.CheckedChanged += new EventHandler(this.method_8);
                            control = box4;
                            break;
                        }
                        case RuleParamType.Indicator:
                            TextBox box2 = new TextBox();
                                
                            box2.ReadOnly = true;
                            box2.Font = new Font(box2.Font, FontStyle.Underline);
                            box2.Cursor = Cursors.Hand;
                            box2.Text = current.Value;
                                
                            box2.TextChanged += new EventHandler(this.method_6);
                            box2.Click += new EventHandler(this.method_9);
                            control = box2;
                            break;

                        case RuleParamType.Fundamental:
                            TextBox box3 = new TextBox();

                            box3.ReadOnly = true;
                            box3.Font = new Font(box3.Font, FontStyle.Underline);
                            box3.Cursor = Cursors.Hand;
                            box3.Text = current.Value;

                            box3.TextChanged += new EventHandler(this.method_6);
                            box3.Click += new EventHandler(this.method_10);
                            control = box3;
                            break;

                        case RuleParamType.StandardDataSeries:
                        {
                            ComboBox box5 = new ComboBox {
                                DropDownStyle = ComboBoxStyle.DropDownList
                            };
                            box5.Items.Add("Open");
                            box5.Items.Add("High");
                            box5.Items.Add("Low");
                            box5.Items.Add("Close");
                            box5.Items.Add("Volume");
                            if (current.Value == "")
                            {
                                current.Value = "Close";
                            }
                            box5.Text = current.Value;
                            box5.SelectedIndexChanged += new EventHandler(this.method_6);
                            control = box5;
                            break;
                        }
                        case RuleParamType.ListOfStrings:
                            box6 = new ComboBox {
                                DropDownStyle = ComboBoxStyle.DropDownList
                            };
                            strArray2 = current.DefaultValue.Split(new char[] { ';' });
                            num2 = 0;
                            //goto  Label_0561;
                            //Label_0561:
                            while (num2 < strArray2.Length)
                            {
                                ///goto Label_052D;
                            //Label_052D:
                                str = strArray2[num2];
                                if (str.Trim() != "")
                                {
                                    box6.Items.Add(str.Trim());
                                }
                                num2++;
                            }
                            if (box6.Items.Count == 0)
                            {
                                box6.Items.Add("Value");
                            }
                            box6.SelectedIndex = box6.Items.IndexOf(current.Value);
                            if (box6.SelectedIndex == -1)
                            {
                                box6.SelectedIndex = 0;
                            }
                            box6.SelectedIndexChanged += new EventHandler(this.method_6);
                            control = box6;
                            break;

                        default:
                            throw new InvalidOperationException("Unexpected rule parameter type: " + current.ParamType);
                    }
                
                //Label_05CB:
                    num3 = 0x62;
                    if (control != null)
                    {
                        control.Tag = current;
                        control.Left = 0x93;
                        control.Top = num;
                        if (current.Decoration.Length > 0)
                        {
                            if (this.pnlParams.Width < 0x139)
                            {
                                num3 = (this.pnlParams.Width - 0x75) / 2;
                            }
                        }
                        else
                        {
                            num3 = 2;
                        }
                        control.Width = (this.pnlParams.Width - (0x75 + num3)) - 30;
                        this.pnlParams.Controls.Add(control);
                    }
                    Label label2 = new Label {
                        AutoSize = false,
                        AutoEllipsis = true,
                        Left = this.pnlParams.Width - (5 + num3),
                        Width = num3,
                        Height = 13,
                        Top = num,
                        Text = current.Decoration
                    };
                    this.pnlParams.Controls.Add(label2);
                    num += 0x18;
                }
            }
        }

        private void treeRules_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode data = (TreeNode) e.Data.GetData(typeof(TreeNode));
            this.method_1(data);
        }

        private void treeRules_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void treeRules_DragOver(object sender, DragEventArgs e)
        {
            Point pt = this.treeRules.PointToClient(new Point(e.X, e.Y));
            TreeNode nodeAt = this.treeRules.GetNodeAt(pt);
            this.treeRules.SelectedNode = nodeAt;
        }

        private void treeRules_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode item = (TreeNode) e.Item;
            this.treeRules.DoDragDrop(item, DragDropEffects.Copy);
        }

        private void treeRules_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.btnRemove.PerformClick();
            }
        }

        private ChartForm MyChartForm
        {
            get
            {
                return (base.ParentForm as ChartForm);
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
                if (this.strategy_0 != null)
                {
                    this.method_4();
                }
            }
        }
    }
}

