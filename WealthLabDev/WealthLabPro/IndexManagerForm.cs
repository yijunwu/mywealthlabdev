namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.IndexDefinitions;

    public class IndexManagerForm : Form, IItemTracker<DataSource>, IIndexManagerUI
    {
        private Button btnBuildNext1;
        private Button btnBuildNext2;
        private Button btnBuildNext3;
        private Button btnBuildPrev2;
        private Button btnBuildPrev3;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private IContainer icontainer_0;
        public static IndexManagerForm Instance;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label lblDescription;
        private Label lblPrefix;
        private List<CustomIndex> list_0 = new List<CustomIndex>();
        private ListBox lstCustomIndexes;
        private DataSourceListView lvDataSets;
        private SortableListView lvIndices;
        private ListView lwIndexDefs;
        private TabPage pageIndexBuildConfirm;
        private TabPage pageIndexBuilder;
        private TabPage pageIndexBuildParameters;
        private TabPage pageIndexBuildSelect;
        private TabPage pageIndices;
        private Panel pnlConfirmationPageParameters;
        private Panel pnlIndexInformation;
        private Panel pnlParameters;
        private StaticDataProvider staticDataProvider_0;
        private const string string_0 = "%";
        private TabControl tabBuild;
        private TabControl tabMain;
        private TextBox txtPrefix;

        public IndexManagerForm()
        {
            this.InitializeComponent();
            Instance = this;
            CustomIndexManager.Instance.AddIndexManagerUI(this);
            MainModule.Instance.DataSources.RegisterObserver(this);
        }

        private void btnBuildNext1_Click(object sender, EventArgs e)
        {
            if (this.SelectedIndexDef == null)
            {
                MessageBox.Show("Please select an Index Definition.", "Index-Lab");
            }
            else if (this.lvDataSets.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select one or more DataSets.", "Index-Lab");
            }
            else if (this.SelectedIndexDef.SupportsParameters && this.SelectedIndexDef.NeedsSeparateUIForParameters)
            {
                this.SelectedIndexDef.ClearUserInterface();
                UserControl parameterUserInterface = this.SelectedIndexDef.ParameterUserInterface;
                this.pnlParameters.Controls.Clear();
                this.pnlParameters.Controls.Add(parameterUserInterface);
                parameterUserInterface.Parent = this.pnlParameters;
                parameterUserInterface.Dock = DockStyle.Fill;
                parameterUserInterface.Visible = true;
                this.tabBuild.SelectedTab = this.pageIndexBuildParameters;
            }
            else
            {
                this.method_1();
                this.tabBuild.SelectedTab = this.pageIndexBuildConfirm;
            }
        }

        private void btnBuildNext2_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            if (!this.SelectedIndexDef.ValidateUserInput(ref errMsg))
            {
                MessageBox.Show(errMsg, "Index-Lab");
            }
            else
            {
                this.method_1();
                this.tabBuild.SelectedTab = this.pageIndexBuildConfirm;
            }
        }

        private void btnBuildNext3_Click(object sender, EventArgs e)
        {
            if (this.SelectedIndexDef.SupportsParameters && !this.SelectedIndexDef.NeedsSeparateUIForParameters)
            {
                string errMsg = "";
                if (!this.SelectedIndexDef.ValidateUserInput(ref errMsg))
                {
                    MessageBox.Show(errMsg, "Index-Lab");
                    return;
                }
            }
            List<string> list = new List<string>();
            for (int i = 0; i < this.lstCustomIndexes.Items.Count; i++)
            {
                CustomIndex index = (CustomIndex) this.lstCustomIndexes.Items[i];
                if (!index.SaveSettings())
                {
                    this.lstCustomIndexes.SelectedIndex = i;
                    return;
                }
                index.Symbol = "%" + index.Symbol;
                if (File.Exists(RootPath + @"\" + index.Symbol + ".xml") || list.Contains(index.Symbol))
                {
                    MessageBox.Show("Index already exists.", "Error");
                    this.lstCustomIndexes.SelectedIndex = i;
                    return;
                }
                list.Add(index.Symbol);
            }
            foreach (CustomIndex index2 in this.list_0)
            {
                if (this.SelectedIndexDef.SupportsParameters && !this.SelectedIndexDef.NeedsSeparateUIForParameters)
                {
                    index2.Parameters = this.SelectedIndexDef.ParameterString;
                }
                string fileName = RootPath + @"\" + index2.Symbol + ".xml";
                FileNameValidator.ValidateFileName(fileName);
                index2.SaveToFile(fileName);
                index2.DataSourceParent.Provider.DataStore.SaveDummyBarsObject(new Bars(index2.Symbol, index2.Scale, index2.BarInterval));
                MainModule.Instance.DataSources.AddSymbol(index2.DataSourceParent, index2.Symbol);
                bool flag = false;
                using (IEnumerator<DataSource> enumerator = MainModule.Instance.DataSources.DataSources.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        DataSource current = enumerator.Current;
                        if (index2.DataSourceParent.Name == current.Name)
                        {
                            ///goto  Label_01E5;  ///WYJ fix, simplify the flow
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    MainModule.Instance.DataSources.Add(index2.DataSourceParent);
                }
            }
            CustomIndexManager.Instance.AddIndices(this.list_0);
            if (this.SelectedIndexDef is AggregateIndicatorIndex)
            {
                MessageBox.Show("Indicator has been created successfully", "Index-Lab");
            }
            else
            {
                MessageBox.Show("Index has been created successfully", "Index-Lab");
            }
            this.IndexManagerForm_Load(this, null);
            this.tabMain.SelectedTab = this.pageIndices;
            this.tabBuild.SelectedTab = this.pageIndexBuildSelect;
        }

        private void btnBuildPrev2_Click(object sender, EventArgs e)
        {
            this.tabBuild.SelectedTab = this.pageIndexBuildSelect;
        }

        private void btnBuildPrev3_Click(object sender, EventArgs e)
        {
            if (this.SelectedIndexDef.SupportsParameters && this.SelectedIndexDef.NeedsSeparateUIForParameters)
            {
                this.tabBuild.SelectedTab = this.pageIndexBuildParameters;
            }
            else
            {
                this.tabBuild.SelectedTab = this.pageIndexBuildSelect;
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

        private void IndexManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CustomIndexManager.Instance.RemoveIndexManagerUI(this);
            Instance = null;
        }

        private void IndexManagerForm_Load(object sender, EventArgs e)
        {
            this.lvIndices.Items.Clear();
            CustomIndexManager.Instance.InitListView(this.lvIndices);
            List<IndexDefinition> indexDefinitions = CustomIndexManager.Instance.IndexDefinitions;
            this.lwIndexDefs.Items.Clear();
            foreach (IndexDefinition definition2 in indexDefinitions)
            {
                definition2.DataHost = MainModule.Instance.DataSources;
                if (definition2 is AggregateIndicatorIndex)
                {
                    ListViewItem item2 = new ListViewItem(definition2.FriendlyName, this.lwIndexDefs.Groups[0]) {
                        Tag = definition2
                    };
                    this.lwIndexDefs.Items.Add(item2);
                }
            }
            foreach (IndexDefinition definition in indexDefinitions)
            {
                definition.DataHost = MainModule.Instance.DataSources;
                if (!(definition is AggregateIndicatorIndex))
                {
                    ListViewItem item = new ListViewItem(definition.FriendlyName, this.lwIndexDefs.Groups[1]) {
                        Tag = definition
                    };
                    this.lwIndexDefs.Items.Add(item);
                }
            }
            this.lvDataSets.Populate(MainModule.Instance.DataSources, false);
            this.lvDataSets.MultiSelect = true;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(IndexManagerForm));
            ListViewGroup group = new ListViewGroup("Indicators", HorizontalAlignment.Left);
            ListViewGroup group2 = new ListViewGroup("Indexes", HorizontalAlignment.Left);
            this.tabMain = new TabControl();
            this.pageIndices = new TabPage();
            this.lvIndices = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.label1 = new Label();
            this.pageIndexBuilder = new TabPage();
            this.tabBuild = new TabControl();
            this.pageIndexBuildSelect = new TabPage();
            this.label2 = new Label();
            this.btnBuildNext1 = new Button();
            this.lblPrefix = new Label();
            this.groupBox1 = new GroupBox();
            this.lwIndexDefs = new ListView();
            this.lblDescription = new Label();
            this.txtPrefix = new TextBox();
            this.groupBox2 = new GroupBox();
            this.lvDataSets = new DataSourceListView(); ///WYJ fix, work around for the Code Metrics problem
            this.pageIndexBuildParameters = new TabPage();
            this.pnlParameters = new Panel();
            this.btnBuildPrev2 = new Button();
            this.btnBuildNext2 = new Button();
            this.pageIndexBuildConfirm = new TabPage();
            this.pnlConfirmationPageParameters = new Panel();
            this.pnlIndexInformation = new Panel();
            this.groupBox3 = new GroupBox();
            this.lstCustomIndexes = new ListBox();
            this.btnBuildPrev3 = new Button();
            this.btnBuildNext3 = new Button();
            this.label3 = new Label();
            this.tabMain.SuspendLayout();
            this.pageIndices.SuspendLayout();
            this.pageIndexBuilder.SuspendLayout();
            this.tabBuild.SuspendLayout();
            this.pageIndexBuildSelect.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pageIndexBuildParameters.SuspendLayout();
            this.pageIndexBuildConfirm.SuspendLayout();
            this.groupBox3.SuspendLayout();
            base.SuspendLayout();
            this.tabMain.Controls.Add(this.pageIndices);
            this.tabMain.Controls.Add(this.pageIndexBuilder);
            this.tabMain.Dock = DockStyle.Fill;
            this.tabMain.ItemSize = new Size(0x57, 0x12);
            this.tabMain.Location = new Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new Size(0x256, 0x1ab);
            this.tabMain.TabIndex = 0;
            this.pageIndices.Controls.Add(this.lvIndices);
            this.pageIndices.Controls.Add(this.label1);
            this.pageIndices.Location = new Point(4, 0x16);
            this.pageIndices.Name = "pageIndices";
            this.pageIndices.Padding = new Padding(3);
            this.pageIndices.Size = new Size(590, 0x191);
            this.pageIndices.TabIndex = 0;
            this.pageIndices.Text = "Custom Indexes";
            this.pageIndices.UseVisualStyleBackColor = true;
            this.lvIndices.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvIndices.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3 });
            this.lvIndices.Location = new Point(12, 40);
            this.lvIndices.Name = "lvIndices";
            this.lvIndices.Size = new Size(570, 0x143);
            this.lvIndices.TabIndex = 1;
            this.lvIndices.UseCompatibleStateImageBehavior = false;
            this.lvIndices.View = View.Details;
            this.columnHeader_0.Text = "Index Symbol";
            this.columnHeader_0.Width = 80;
            this.columnHeader_1.Text = "Index Definition";
            this.columnHeader_1.Width = 120;
            this.columnHeader_2.Text = "DataSet";
            this.columnHeader_2.Width = 120;
            this.columnHeader_3.Text = "Parameters";
            this.columnHeader_3.Width = 200;
            this.label1.Location = new Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x23d, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            this.pageIndexBuilder.Controls.Add(this.tabBuild);
            this.pageIndexBuilder.Location = new Point(4, 0x16);
            this.pageIndexBuilder.Name = "pageIndexBuilder";
            this.pageIndexBuilder.Padding = new Padding(3);
            this.pageIndexBuilder.Size = new Size(590, 0x191);
            this.pageIndexBuilder.TabIndex = 1;
            this.pageIndexBuilder.Text = "Index Builder";
            this.pageIndexBuilder.UseVisualStyleBackColor = true;
            this.tabBuild.Appearance = TabAppearance.FlatButtons;
            this.tabBuild.Controls.Add(this.pageIndexBuildSelect);
            this.tabBuild.Controls.Add(this.pageIndexBuildParameters);
            this.tabBuild.Controls.Add(this.pageIndexBuildConfirm);
            this.tabBuild.Dock = DockStyle.Fill;
            this.tabBuild.ItemSize = new Size(0, 1);
            this.tabBuild.Location = new Point(3, 3);
            this.tabBuild.Name = "tabBuild";
            this.tabBuild.SelectedIndex = 0;
            this.tabBuild.Size = new Size(0x248, 0x18b);
            this.tabBuild.SizeMode = TabSizeMode.Fixed;
            this.tabBuild.TabIndex = 0;
            this.pageIndexBuildSelect.Controls.Add(this.label2);
            this.pageIndexBuildSelect.Controls.Add(this.btnBuildNext1);
            this.pageIndexBuildSelect.Controls.Add(this.lblPrefix);
            this.pageIndexBuildSelect.Controls.Add(this.groupBox1);
            this.pageIndexBuildSelect.Controls.Add(this.txtPrefix);
            this.pageIndexBuildSelect.Controls.Add(this.groupBox2);
            this.pageIndexBuildSelect.Location = new Point(4, 5);
            this.pageIndexBuildSelect.Name = "pageIndexBuildSelect";
            this.pageIndexBuildSelect.Padding = new Padding(3);
            this.pageIndexBuildSelect.Size = new Size(0x240, 0x182);
            this.pageIndexBuildSelect.TabIndex = 0;
            this.pageIndexBuildSelect.UseVisualStyleBackColor = true;
            this.label2.Location = new Point(12, 3);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x225, 0x23);
            this.label2.TabIndex = 12;
            this.label2.Text = "Select an Index Definition and one or more DataSets below.";
            this.btnBuildNext1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnBuildNext1.Location = new Point(0x1e6, 0x165);
            this.btnBuildNext1.Name = "btnBuildNext1";
            this.btnBuildNext1.Size = new Size(0x4b, 0x17);
            this.btnBuildNext1.TabIndex = 11;
            this.btnBuildNext1.Text = "Next";
            this.btnBuildNext1.UseVisualStyleBackColor = true;
            this.btnBuildNext1.Click += new EventHandler(this.btnBuildNext1_Click);
            this.lblPrefix.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblPrefix.AutoSize = true;
            this.lblPrefix.Location = new Point(2, 390);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new Size(0x119, 13);
            this.lblPrefix.TabIndex = 9;
            this.lblPrefix.Text = "Select the Prefix to use at the beginning of Index Symbols:";
            this.groupBox1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.lwIndexDefs);
            this.groupBox1.Controls.Add(this.lblDescription);
            this.groupBox1.Location = new Point(5, 0x29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xfd, 310);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Index Definitions";
            this.lwIndexDefs.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lwIndexDefs.FullRowSelect = true;
            group.Header = "Indicators";
            group.Name = "IndicatorsGroup";
            group2.Header = "Indexes";
            group2.Name = "GeneralIndexesGroup";
            this.lwIndexDefs.Groups.AddRange(new ListViewGroup[] { group, group2 });
            this.lwIndexDefs.HideSelection = false;
            this.lwIndexDefs.Location = new Point(6, 0x13);
            this.lwIndexDefs.MultiSelect = false;
            this.lwIndexDefs.Name = "lwIndexDefs";
            this.lwIndexDefs.Size = new Size(0xf1, 0xcd);
            this.lwIndexDefs.TabIndex = 2;
            this.lwIndexDefs.TileSize = new Size(0xa5, 0x11);
            this.lwIndexDefs.UseCompatibleStateImageBehavior = false;
            this.lwIndexDefs.View = View.Tile;
            this.lwIndexDefs.SelectedIndexChanged += new EventHandler(this.lwIndexDefs_SelectedIndexChanged);
            this.lblDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblDescription.ForeColor = Color.Navy;
            this.lblDescription.Location = new Point(7, 0xe3);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(240, 80);
            this.lblDescription.TabIndex = 1;
            this.txtPrefix.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.txtPrefix.Location = new Point(0x121, 0x183);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new Size(100, 20);
            this.txtPrefix.TabIndex = 10;
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.lvDataSets);
            this.groupBox2.Location = new Point(0x108, 0x29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x129, 310);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DataSets";
            this.lvDataSets.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvDataSets.FullRowSelect = true;
            this.lvDataSets.HideSelection = false;
            this.lvDataSets.Location = new Point(6, 0x13);
            this.lvDataSets.MultiSelect = false;
            this.lvDataSets.Name = "lvDataSets";
            this.lvDataSets.Size = new Size(0x11d, 0x11d);
            this.lvDataSets.TabIndex = 0;
            this.lvDataSets.UseCompatibleStateImageBehavior = false;
            this.lvDataSets.View = View.Details;
            this.pageIndexBuildParameters.Controls.Add(this.pnlParameters);
            this.pageIndexBuildParameters.Controls.Add(this.btnBuildPrev2);
            this.pageIndexBuildParameters.Controls.Add(this.btnBuildNext2);
            this.pageIndexBuildParameters.Location = new Point(4, 5);
            this.pageIndexBuildParameters.Name = "pageIndexBuildParameters";
            this.pageIndexBuildParameters.Padding = new Padding(3);
            this.pageIndexBuildParameters.Size = new Size(0x240, 0x182);
            this.pageIndexBuildParameters.TabIndex = 1;
            this.pageIndexBuildParameters.UseVisualStyleBackColor = true;
            this.pnlParameters.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlParameters.Location = new Point(8, 13);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.Size = new Size(0x231, 0x14d);
            this.pnlParameters.TabIndex = 14;
            this.btnBuildPrev2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnBuildPrev2.Location = new Point(0x19e, 0x165);
            this.btnBuildPrev2.Name = "btnBuildPrev2";
            this.btnBuildPrev2.Size = new Size(0x4b, 0x17);
            this.btnBuildPrev2.TabIndex = 13;
            this.btnBuildPrev2.Text = "Previous";
            this.btnBuildPrev2.UseVisualStyleBackColor = true;
            this.btnBuildPrev2.Click += new EventHandler(this.btnBuildPrev2_Click);
            this.btnBuildNext2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnBuildNext2.Location = new Point(0x1ef, 0x165);
            this.btnBuildNext2.Name = "btnBuildNext2";
            this.btnBuildNext2.Size = new Size(0x4b, 0x17);
            this.btnBuildNext2.TabIndex = 12;
            this.btnBuildNext2.Text = "Next";
            this.btnBuildNext2.UseVisualStyleBackColor = true;
            this.btnBuildNext2.Click += new EventHandler(this.btnBuildNext2_Click);
            this.pageIndexBuildConfirm.Controls.Add(this.pnlConfirmationPageParameters);
            this.pageIndexBuildConfirm.Controls.Add(this.pnlIndexInformation);
            this.pageIndexBuildConfirm.Controls.Add(this.groupBox3);
            this.pageIndexBuildConfirm.Controls.Add(this.btnBuildPrev3);
            this.pageIndexBuildConfirm.Controls.Add(this.btnBuildNext3);
            this.pageIndexBuildConfirm.Controls.Add(this.label3);
            this.pageIndexBuildConfirm.Location = new Point(4, 5);
            this.pageIndexBuildConfirm.Name = "pageIndexBuildConfirm";
            this.pageIndexBuildConfirm.Padding = new Padding(3);
            this.pageIndexBuildConfirm.Size = new Size(0x240, 0x182);
            this.pageIndexBuildConfirm.TabIndex = 2;
            this.pageIndexBuildConfirm.UseVisualStyleBackColor = true;
            this.pnlConfirmationPageParameters.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlConfirmationPageParameters.Location = new Point(0xdf, 230);
            this.pnlConfirmationPageParameters.Name = "pnlConfirmationPageParameters";
            this.pnlConfirmationPageParameters.Size = new Size(0x156, 0x6c);
            this.pnlConfirmationPageParameters.TabIndex = 0x18;
            this.pnlIndexInformation.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlIndexInformation.Location = new Point(0xdf, 6);
            this.pnlIndexInformation.Name = "pnlIndexInformation";
            this.pnlIndexInformation.Size = new Size(350, 0xd8);
            this.pnlIndexInformation.TabIndex = 0x17;
            this.groupBox3.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox3.Controls.Add(this.lstCustomIndexes);
            this.groupBox3.Location = new Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xd6, 0x15c);
            this.groupBox3.TabIndex = 0x16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Indexes";
            this.lstCustomIndexes.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lstCustomIndexes.FormattingEnabled = true;
            this.lstCustomIndexes.Location = new Point(2, 0x13);
            this.lstCustomIndexes.Name = "lstCustomIndexes";
            this.lstCustomIndexes.Size = new Size(210, 160);
            this.lstCustomIndexes.Sorted = true;
            this.lstCustomIndexes.TabIndex = 1;
            this.lstCustomIndexes.SelectedIndexChanged += new EventHandler(this.lstCustomIndexes_SelectedIndexChanged);
            this.btnBuildPrev3.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnBuildPrev3.Location = new Point(410, 0x165);
            this.btnBuildPrev3.Name = "btnBuildPrev3";
            this.btnBuildPrev3.Size = new Size(0x4b, 0x17);
            this.btnBuildPrev3.TabIndex = 15;
            this.btnBuildPrev3.Text = "Previous";
            this.btnBuildPrev3.UseVisualStyleBackColor = true;
            this.btnBuildPrev3.Click += new EventHandler(this.btnBuildPrev3_Click);
            this.btnBuildNext3.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnBuildNext3.Location = new Point(0x1eb, 0x165);
            this.btnBuildNext3.Name = "btnBuildNext3";
            this.btnBuildNext3.Size = new Size(0x4b, 0x17);
            this.btnBuildNext3.TabIndex = 14;
            this.btnBuildNext3.Text = "Finish";
            this.btnBuildNext3.UseVisualStyleBackColor = true;
            this.btnBuildNext3.Click += new EventHandler(this.btnBuildNext3_Click);
            this.label3.Dock = DockStyle.Fill;
            this.label3.Location = new Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new Size(570, 380);
            this.label3.TabIndex = 0;
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.label3.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x256, 0x1ab);
            base.Controls.Add(this.tabMain);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "IndexManagerForm";
            this.Text = "Index-Lab \x00ae";
            base.FormClosed += new FormClosedEventHandler(this.IndexManagerForm_FormClosed);
            base.Load += new EventHandler(this.IndexManagerForm_Load);
            this.tabMain.ResumeLayout(false);
            this.pageIndices.ResumeLayout(false);
            this.pageIndexBuilder.ResumeLayout(false);
            this.tabBuild.ResumeLayout(false);
            this.pageIndexBuildSelect.ResumeLayout(false);
            this.pageIndexBuildSelect.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.pageIndexBuildParameters.ResumeLayout(false);
            this.pageIndexBuildConfirm.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void ItemAdded(DataSource item)
        {
            this.lvDataSets.Populate(MainModule.Instance.DataSources, false);
            this.lvDataSets.MultiSelect = true;
        }

        public void ItemChanged(DataSource item)
        {
            this.lvDataSets.Populate(MainModule.Instance.DataSources, false);
            this.lvDataSets.MultiSelect = true;
        }

        public void ItemRemoved(DataSource item)
        {
            this.lvDataSets.Populate(MainModule.Instance.DataSources, false);
            this.lvDataSets.MultiSelect = true;
        }

        private void lstCustomIndexes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomIndex selectedItem = (CustomIndex) this.lstCustomIndexes.SelectedItem;
            IndexInformationControl indexInformation = selectedItem.IndexInformation;
            this.pnlIndexInformation.Controls.Clear();
            this.pnlIndexInformation.Controls.Add(indexInformation);
            indexInformation.Parent = this.pnlIndexInformation;
            indexInformation.Dock = DockStyle.Fill;
            indexInformation.Visible = true;
        }

        private void lwIndexDefs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.lwIndexDefs.SelectedItems != null) && (this.lwIndexDefs.SelectedItems.Count > 0))
            {
                ListViewItem item = this.lwIndexDefs.SelectedItems[0];
                IndexDefinition tag = (IndexDefinition) item.Tag;
                this.lblDescription.Text = tag.Description;
                this.txtPrefix.Text = tag.Prefix;
            }
        }

        private void method_0(DataSource dataSource_0, DataSource dataSource_1)
        {
            string symbol = (this.SelectedIndexDef.Prefix + "_" + dataSource_1.Name).Replace(' ', '_').ToUpper();
            CustomIndex item = CustomIndexManager.Instance.CreateNewIndex(symbol, this.SelectedIndexDef.ParameterString, this.SelectedIndexDef.GetType().GUID, dataSource_1.Name, dataSource_0.Scale, dataSource_0.BarInterval, dataSource_0.Name, dataSource_0);
            this.list_0.Add(item);
        }

        private void method_1()
        {
            this.list_0.Clear();
            this.lstCustomIndexes.Items.Clear();
            bool flag = false;
            
            foreach (ListViewItem item in this.lvDataSets.SelectedItems)
            {
                DataSource tag = (DataSource) item.Tag;
                flag = false;
                using (IEnumerator<DataSource> enumerator2 = MainModule.Instance.DataSources.DataSources.GetEnumerator())
                {
                    DataSource current;
                    while (enumerator2.MoveNext())
                    {
                        current = enumerator2.Current;
                        if ((current.IsIndexLabDataset && (current.Scale == tag.Scale)) && (current.BarInterval == tag.BarInterval))
                        {
                            ///goto  Label_00A7; ///WYJ fix, simplify the flow
                            flag = true;
                            this.method_0(current, tag);
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    DataSource source3 = this.method_2(tag);
                    this.method_0(source3, tag);
                }
            }
            foreach (CustomIndex index in this.list_0)
            {
                this.lstCustomIndexes.Items.Add(index);
                index.IndexInformation.Fillup(index);
            }
            if ((this.lstCustomIndexes.Items.Count > 0) && (this.lstCustomIndexes.SelectedIndex < 0))
            {
                this.lstCustomIndexes.SelectedIndex = 0;
            }
            if (this.SelectedIndexDef.SupportsParameters && !this.SelectedIndexDef.NeedsSeparateUIForParameters)
            {
                this.SelectedIndexDef.ClearUserInterface();
                UserControl parameterUserInterface = this.SelectedIndexDef.ParameterUserInterface;
                this.pnlConfirmationPageParameters.Controls.Clear();
                this.pnlConfirmationPageParameters.Controls.Add(parameterUserInterface);
                parameterUserInterface.Parent = this.pnlConfirmationPageParameters;
                parameterUserInterface.Dock = DockStyle.Fill;
                parameterUserInterface.Visible = true;
            }
            else
            {
                this.pnlConfirmationPageParameters.Controls.Clear();
            }
        }

        private DataSource method_2(DataSource dataSource_0)
        {
            string str;
            this.staticDataProvider_0 = MainModule.Instance.DataSources.FindProvider("IndexStaticProvider");
            DataSource source = new DataSource(this.staticDataProvider_0) {
                Scale = dataSource_0.Scale,
                BarInterval = dataSource_0.BarInterval
            };
            if (!dataSource_0.IsIntraday)
            {
                str = "Index-Lab " + dataSource_0.Scale.ToString();
            }
            else
            {
                str = "Index-Lab " + dataSource_0.BarInterval.ToString() + "-" + dataSource_0.Scale.ToString();
            }
            List<string> list = new List<string>();
            bool flag = false;
            foreach (DataSource source2 in MainModule.Instance.DataSources.DataSources)
            {
                string item = source2.Name.Trim();
                if (item == str)
                {
                    flag = true;
                }
                else if (item.StartsWith(str))
                {
                    list.Add(item);
                }
            }
            int num = 1;
            while (flag)
            {
                string str2 = str + " (" + num.ToString() + ")";
                if (list.Contains(str2))
                {
                    flag = true;
                    num++;
                }
                else
                {
                    flag = false;
                    str = str2;
                }
            }
            source.Name = str;
            return source;
        }

        public void RefreshCustomIndexesList()
        {
            this.lvIndices.Items.Clear();
            CustomIndexManager.Instance.InitListView(this.lvIndices);
        }

        public StaticDataProvider ProviderSelected
        {
            get
            {
                return this.staticDataProvider_0;
            }
        }

        public static string RootPath
        {
            get
            {
                string path = MainModule.Instance.DataSources.RootPath + @"\" + CustomIndex.FolderName + @"\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public IndexDefinition SelectedIndexDef
        {
            get
            {
                if (this.lwIndexDefs.SelectedItems.Count == 0)
                {
                    return null;
                }
                ListViewItem item = this.lwIndexDefs.SelectedItems[0];
                return (item.Tag as IndexDefinition);
            }
        }
    }
}

