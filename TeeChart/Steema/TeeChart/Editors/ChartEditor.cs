namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors.Export;
    using Steema.TeeChart.Editors.Tools;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class ChartEditor : Form, ITeeEventListener
    {
        private AspectEditor aspectEditor;
        private AxesEditor axesEditor;
        private Button BAddGroup;
        private Button BDeleteGroup;
        private Button bHelp;
        private Button BRenameGroup;
        private Button bSeriesDown;
        private Button bSeriesUp;
        private Button button1;
        private Button buttonAddSeries;
        private Button buttonChangeSeries;
        private Button buttonCloneSeries;
        private Button buttonDeleteSeries;
        private Button buttonTitleSeries;
        private ComboBox CBSeries;
        private Steema.TeeChart.Chart Chart;
        private ChartListBox chartListBox1;
        private CheckBox checkBox2;
        private IContainer components;
        private DataGrid dataGrid1;
        private DataTable dataTable;
        private int DeltaGroup;
        private ExportEditor exportEditor;
        private GeneralEditor generalEditor;
        private GLEditor glEditor;
        public string HelpFileName;
        private Label label4;
        private Label label7;
        private CheckedListBox LBGroups;
        private Steema.TeeChart.Editors.LegendEditor legendEditor;
        private LinkLabel linkLabel1;
        private Steema.TeeChart.Editors.PageEditor pageEditor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private Steema.TeeChart.Editors.PanelEditor panelEditor;
        private System.Windows.Forms.Panel panelSeries;
        private PictureBox pictureBox2;
        private PrintPreview printPreview;
        private SeriesEditor seriesEditor;
        private Splitter splitter1;
        private TabPage tabAspect;
        private TabPage tabAxes;
        private TabPage tabChart;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabControl tabControl3;
        private TabPage tabData;
        private TabPage tabExport;
        private TabPage tabGeneral;
        private TabPage tabLegend;
        private TabPage tabOpenGL;
        private TabPage tabPage10;
        private TabPage tabPage11;
        private TabPage tabPage3;
        private TabPage tabPage9;
        private TabPage tabPageSeries;
        private TabPage tabPaging;
        private TabPage tabPanel;
        private TabPage tabPrint;
        private TabPage tabSeries;
        private TabPage tabThemes;
        private TabPage tabTitles;
        private TabPage tabTools;
        private TabPage tabWalls;
        private ThemeEditor themeEditor;
        private TitleEditor titlesEditor;
        private ToolsEditor toolsEditor;
        private Steema.TeeChart.Editors.WallEditor[] wallEditor;

        public ChartEditor()
        {
            this.wallEditor = new Steema.TeeChart.Editors.WallEditor[4];
            this.HelpFileName = "";
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.InitializeComponent();
            this.chartListBox1.Chart = null;
        }

        public ChartEditor(Steema.TeeChart.Chart c) : this()
        {
            this.Chart = c;
            if (this.Chart != null)
            {
                this.Chart.Listeners.Add(this);
            }
            if (!this.Chart.iOpenGL)
            {
                if (this.tabControl1.Controls.Contains(this.tabOpenGL))
                {
                    this.tabControl1.Controls.Remove(this.tabOpenGL);
                }
            }
            else if (!this.tabControl1.Controls.Contains(this.tabOpenGL))
            {
                this.tabControl1.Controls.Add(this.tabOpenGL);
            }
            this.seriesEditor = new SeriesEditor();
            EditorUtils.GetUpDown(this.bSeriesUp, this.bSeriesDown);
            base.Width = Convert.ToInt32(Texts.DefaultEditorSize);
            base.Height = Convert.ToInt32(Texts.DefaultEditorHeight);
            this.chartListBox1.SetChart(c);
            this.AddSeries();
            this.ActivateGroup(false);
            ArrayList excludeChildren = new ArrayList();
            excludeChildren.Add(this.CBSeries);
            excludeChildren.Add(this.seriesEditor.cbCursor);
            EditorUtils.Translate(this, excludeChildren);
            if (this.Text.Length == 0)
            {
                this.Text = string.Format(Texts.Editing, c.ToString());
            }
        }

        private void ActivateGroup(bool activate)
        {
            this.BAddGroup.Visible = activate;
            this.BDeleteGroup.Visible = activate;
            this.BRenameGroup.Visible = activate;
            this.LBGroups.Visible = activate;
            this.splitter1.Visible = activate;
        }

        private Steema.TeeChart.Styles.Series AddFromGallery()
        {
            Steema.TeeChart.Styles.Series component = ChartGallery.CreateNew(this.Chart, null);
            if (component != null)
            {
                if (this.Chart.iWorldMaps && (component is IWorldMaps))
                {
                    ((IWorldMaps) component).InGallery = false;
                }
                component.Chart = this.Chart;
                if (this.Chart.AddToContainer(component))
                {
                    component.FillSampleValues();
                }
                this.AddSeries();
                this.chartListBox1.SelectedSeries = component;
            }
            return component;
        }

        private void AddSeries()
        {
            this.CBSeries.Items.Clear();
            foreach (Steema.TeeChart.Styles.Series series in this.Chart.Series)
            {
                if (!series.InternalUse)
                {
                    this.CBSeries.Items.Add(series);
                }
            }
            this.chartListBox1.FillSeries(null);
            if (this.chartListBox1.Items.Count > 0)
            {
                this.chartListBox1.SelectedIndex = 0;
            }
            this.EnableButtons();
        }

        private void BAddGroup_Click(object sender, EventArgs e)
        {
            string str = "";
            if (TextInput.Query(Texts.AddNewGroup, Texts.GroupName, ref str))
            {
                this.Chart.series.AddGroup(str);
                this.FillGroups();
                this.LBGroups.SelectedIndex = this.LBGroups.Items.Count - 1;
                this.LBGroups_SelectedIndexChanged(this.LBGroups, null);
            }
        }

        private void BDeleteGroup_Click(object sender, EventArgs e)
        {
            if (Utils.YesNoDelete(this.LBGroups.Items[this.LBGroups.SelectedIndex].ToString()))
            {
                this.Chart.series.Groups.RemoveAt(this.LBGroups.SelectedIndex - this.DeltaGroup);
                this.FillGroups();
            }
        }

        private void BRenameGroup_Click(object sender, EventArgs e)
        {
            string str = this.LBGroups.Items[this.LBGroups.SelectedIndex].ToString();
            if (TextInput.Query(Texts.ChangeGroupName, Texts.GroupName, ref str))
            {
                this.CurrentGroup().Name = str;
                this.FillGroups();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Styles.Series series = this.AddFromGallery();
            if ((series != null) && (series.Function != null))
            {
                this.tabControl1.SelectedTab = this.tabPageSeries;
                this.seriesEditor.ShowSeriesSource();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = (this.HelpFileName.Length == 0) ? "TeeChartNet3.chm" : this.HelpFileName;
            Help.ShowHelp(this, url);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.chartListBox1.SelectedIndex;
            if (selectedIndex > 0)
            {
                this.Chart.Series.Exchange(selectedIndex, selectedIndex - 1);
                this.AddSeries();
                this.chartListBox1.ClearSelected();
                this.chartListBox1.SelectedIndex = selectedIndex - 1;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.chartListBox1.SelectedIndex;
            if (selectedIndex < (this.Chart.Series.Count - 1))
            {
                this.Chart.Series.Exchange(selectedIndex, selectedIndex + 1);
                this.AddSeries();
                this.chartListBox1.ClearSelected();
                this.chartListBox1.SelectedIndex = selectedIndex + 1;
            }
        }

        private void buttonChangeSeries_Click(object sender, EventArgs e)
        {
            this.chartListBox1.ChangeTypeSeries();
            this.CBSeries_SelectedIndexChanged(sender, e);
        }

        private void buttonCloneSeries_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Styles.Series component = this.Series.Clone() as Steema.TeeChart.Styles.Series;
            component.Chart.AddToContainer(component);
            this.AddSeries();
            this.chartListBox1.SelectedSeries = component;
        }

        private void buttonDeleteSeries_Click(object sender, EventArgs e)
        {
            this.chartListBox1.DeleteSeries();
            this.EnableButtons();
        }

        private void buttonTitleSeries_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Styles.Series series = this.Series;
            string str = series.ToString();
            if (TextInput.Query(Texts.ChangeSeriesTitle, Texts.NewSeriesTitle, ref str))
            {
                series.Title = str;
                this.chartListBox1.Items[this.chartListBox1.SelectedIndex] = series;
                this.chartListBox1.SelectedSeries = series;
                this.CBSeries.Items.Clear();
                foreach (Steema.TeeChart.Styles.Series series2 in this.Chart.Series)
                {
                    if (!series2.InternalUse)
                    {
                        this.CBSeries.Items.Add(series2);
                    }
                }
            }
        }

        private void CBSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.chartListBox1.SelectedIndex != this.CBSeries.SelectedIndex) && (this.CBSeries.SelectedIndex != -1))
            {
                this.chartListBox1.ClearSelected();
                this.chartListBox1.SelectedIndex = this.CBSeries.SelectedIndex;
            }
            this.panelSeries.Controls.Clear();
            if ((this.CBSeries.SelectedIndex != -1) && !this.Series.InternalUse)
            {
                if (this.seriesEditor != null)
                {
                    this.seriesEditor.Dispose();
                    this.seriesEditor = new SeriesEditor(this.Series, this.panelSeries);
                }
                this.label7.BackColor = this.Series.Color;
                this.label7.Visible = this.Series.UseSeriesColor;
                this.label7.Enabled = this.label7.Visible;
                this.label4.Text = this.Series.ToString();
                this.pictureBox2.Image = this.Series.GetBitmapEditor();
                ArrayList excludeChildren = new ArrayList();
                excludeChildren.Add(this.CBSeries);
                excludeChildren.Add(this.chartListBox1);
                excludeChildren.Add(this.label4);
                if (this.titlesEditor != null)
                {
                    excludeChildren.Add(this.titlesEditor.textTitle);
                }
                if (this.axesEditor != null)
                {
                    excludeChildren.Add(this.axesEditor.axisEditor.eTitle);
                }
                excludeChildren.Add(this.seriesEditor.cbCursor);
                EditorUtils.Translate(this, excludeChildren);
            }
        }

        private void ChartEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Chart.parent != null)
            {
                IContainer chartContainer = null;
                chartContainer = this.Chart.ChartContainer;
                if (chartContainer != null)
                {
                    foreach (object obj2 in chartContainer.Components)
                    {
                        for (int i = 0; i < Utils.SeriesTypesCount; i++)
                        {
                            if (obj2.GetType() == Utils.SeriesTypesOf[i])
                            {
                                Steema.TeeChart.Styles.Series.SeriesDesigner designer = new Steema.TeeChart.Styles.Series.SeriesDesigner();
                                designer.Initialize((Steema.TeeChart.Styles.Series) obj2);
                                designer.Refresh();
                                break;
                            }
                        }
                        for (int j = 0; j < Utils.ToolTypesCount; j++)
                        {
                            if (obj2.GetType() == Utils.ToolTypesOf[j])
                            {
                                Steema.TeeChart.Tools.Tool.ToolDesigner designer2 = new Steema.TeeChart.Tools.Tool.ToolDesigner();
                                designer2.Initialize((Steema.TeeChart.Tools.Tool) obj2);
                                designer2.Refresh();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void ChartEditor_Load(object sender, EventArgs e)
        {
            base.Icon = EditorUtils.TChartIcon();
            this.linkLabel1.Visible = ((this.Chart != null) && (this.Chart.parent != null)) && (this.Chart.parent.GetContainer() != null);
            if (this.linkLabel1.Visible)
            {
                this.bHelp.Visible = true;
            }
            if (this.LBGroups.Visible)
            {
                this.FillGroups();
            }
        }

        private void chartListBox1_DoubleClick(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabPageSeries;
        }

        private void chartListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableArrowButtons();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.Chart.Walls.Visible = this.checkBox2.Checked;
        }

        private SeriesGroup CurrentGroup()
        {
            if (((this.DeltaGroup != 1) || (this.LBGroups.SelectedIndex != 0)) && (this.LBGroups.SelectedIndex >= 0))
            {
                return this.Chart.series.Groups[this.LBGroups.SelectedIndex - this.DeltaGroup];
            }
            return null;
        }

        private Wall CurrentWall()
        {
            switch (this.tabControl3.SelectedIndex)
            {
                case 0:
                    return this.Chart.Walls.Left;

                case 1:
                    return this.Chart.Walls.Right;

                case 2:
                    return this.Chart.Walls.Back;
            }
            return this.Chart.Walls.Bottom;
        }

        private void dataGrid1_DataSourceChanged(object sender, EventArgs e)
        {
            if (this.chartListBox1.SelectedSeries != null)
            {
                this.chartListBox1.SelectedSeries.manualData = true;
                this.chartListBox1.SelectedSeries.CheckDataSource();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.Chart != null)
            {
                this.Chart.RemoveListener(this);
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableArrowButtons()
        {
            this.bSeriesUp.Enabled = this.chartListBox1.SelectedIndex > 0;
            this.bSeriesDown.Enabled = this.chartListBox1.SelectedIndex < (this.chartListBox1.Items.Count - 1);
        }

        private void EnableButtons()
        {
            bool flag = this.chartListBox1.SelectedIndex != -1;
            this.buttonDeleteSeries.Enabled = flag;
            this.buttonTitleSeries.Enabled = flag;
            this.buttonCloneSeries.Enabled = flag;
            this.buttonChangeSeries.Enabled = flag;
            this.EnableArrowButtons();
        }

        private void EnableGroupButtons()
        {
            this.BDeleteGroup.Enabled = this.LBGroups.SelectedIndex >= this.DeltaGroup;
            this.BRenameGroup.Enabled = this.LBGroups.SelectedIndex >= this.DeltaGroup;
        }

        private void FillGroups()
        {
            int selectedIndex = this.LBGroups.SelectedIndex;
            if (selectedIndex < 0)
            {
                selectedIndex = 0;
            }
            this.LBGroups.SelectedIndexChanged -= new EventHandler(this.LBGroups_SelectedIndexChanged);
            this.LBGroups.ItemCheck -= new ItemCheckEventHandler(this.LBGroups_ItemCheck);
            try
            {
                this.LBGroups.Items.Clear();
                if (this.DeltaGroup == 1)
                {
                    this.LBGroups.Items.Add(Texts.All);
                    this.LBGroups.SetItemChecked(0, this.Chart.series.AllActive);
                }
                foreach (SeriesGroup group in this.Chart.series.Groups)
                {
                    switch (group.Active)
                    {
                        case SeriesGroupActive.Yes:
                            this.LBGroups.Items.Add(group.Name, CheckState.Checked);
                            break;

                        case SeriesGroupActive.No:
                            this.LBGroups.Items.Add(group.Name, CheckState.Unchecked);
                            break;

                        case SeriesGroupActive.Some:
                            this.LBGroups.Items.Add(group.Name, CheckState.Indeterminate);
                            break;
                    }
                }
                if (selectedIndex > (this.LBGroups.Items.Count - 1))
                {
                    selectedIndex = this.LBGroups.Items.Count - 1;
                }
                if (this.LBGroups.Items.Count > 0)
                {
                    this.LBGroups.SelectedIndex = selectedIndex;
                }
            }
            finally
            {
                this.LBGroups.SelectedIndexChanged += new EventHandler(this.LBGroups_SelectedIndexChanged);
                this.LBGroups.ItemCheck += new ItemCheckEventHandler(this.LBGroups_ItemCheck);
            }
            this.LBGroups_SelectedIndexChanged(this.LBGroups, null);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tabControl2 = new TabControl();
            this.tabSeries = new TabPage();
            this.panel12 = new System.Windows.Forms.Panel();
            this.splitter1 = new Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.chartListBox1 = new ChartListBox(this.components);
            this.LBGroups = new CheckedListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BRenameGroup = new Button();
            this.BDeleteGroup = new Button();
            this.BAddGroup = new Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bSeriesDown = new Button();
            this.bSeriesUp = new Button();
            this.buttonChangeSeries = new Button();
            this.buttonCloneSeries = new Button();
            this.buttonTitleSeries = new Button();
            this.buttonDeleteSeries = new Button();
            this.buttonAddSeries = new Button();
            this.tabPanel = new TabPage();
            this.tabAxes = new TabPage();
            this.tabGeneral = new TabPage();
            this.tabTitles = new TabPage();
            this.tabWalls = new TabPage();
            this.tabControl3 = new TabControl();
            this.tabPage3 = new TabPage();
            this.tabPage9 = new TabPage();
            this.tabPage10 = new TabPage();
            this.tabPage11 = new TabPage();
            this.checkBox2 = new CheckBox();
            this.tabPaging = new TabPage();
            this.tabLegend = new TabPage();
            this.tabAspect = new TabPage();
            this.tabOpenGL = new TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tabControl1 = new TabControl();
            this.tabChart = new TabPage();
            this.tabPageSeries = new TabPage();
            this.panelSeries = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label4 = new Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label7 = new Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new PictureBox();
            this.CBSeries = new ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabPrint = new TabPage();
            this.tabExport = new TabPage();
            this.tabTools = new TabPage();
            this.tabThemes = new TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new LinkLabel();
            this.bHelp = new Button();
            this.button1 = new Button();
            this.tabControl2.SuspendLayout();
            this.tabSeries.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabWalls.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabChart.SuspendLayout();
            this.tabPageSeries.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabData = new TabPage();
            this.dataGrid1 = new DataGrid();
            this.dataGrid1.BeginInit();
            this.tabData.SuspendLayout();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.tabControl2.Controls.Add(this.tabSeries);
            this.tabControl2.Controls.Add(this.tabPanel);
            this.tabControl2.Controls.Add(this.tabAxes);
            this.tabControl2.Controls.Add(this.tabGeneral);
            this.tabControl2.Controls.Add(this.tabTitles);
            this.tabControl2.Controls.Add(this.tabWalls);
            this.tabControl2.Controls.Add(this.tabPaging);
            this.tabControl2.Controls.Add(this.tabLegend);
            this.tabControl2.Controls.Add(this.tabAspect);
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.HotTrack = true;
            this.tabControl2.Location = new Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.ShowToolTips = true;
            this.tabControl2.Size = new Size(0x18e, 250);
            this.tabControl2.TabIndex = 0;
            this.tabControl2.SelectedIndexChanged += new EventHandler(this.tabControl2_SelectedIndexChanged);
            this.tabSeries.Controls.Add(this.panel12);
            this.tabSeries.Controls.Add(this.panel3);
            this.tabSeries.Controls.Add(this.panel1);
            this.tabSeries.Location = new Point(4, 0x16);
            this.tabSeries.Name = "tabSeries";
            this.tabSeries.Size = new Size(390, 0xe0);
            this.tabSeries.TabIndex = 0;
            this.tabSeries.Text = "Series";
            this.panel12.Controls.Add(this.splitter1);
            this.panel12.Controls.Add(this.panel5);
            this.panel12.Controls.Add(this.LBGroups);
            this.panel12.Dock = DockStyle.Fill;
            this.panel12.Location = new Point(0, 0x18);
            this.panel12.Name = "panel12";
            this.panel12.Size = new Size(0x11d, 200);
            this.panel12.TabIndex = 3;
            this.splitter1.Location = new Point(0x58, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(3, 200);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.panel5.Controls.Add(this.chartListBox1);
            this.panel5.Dock = DockStyle.Fill;
            this.panel5.DockPadding.Left = 3;
            this.panel5.Location = new Point(0x58, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(0xc5, 200);
            this.panel5.TabIndex = 2;
            this.chartListBox1.AllowDrop = true;
            this.chartListBox1.Dock = DockStyle.Fill;
            this.chartListBox1.IntegralHeight = false;
            this.chartListBox1.Location = new Point(3, 0);
            this.chartListBox1.Name = "chartListBox1";
            this.chartListBox1.OtherItems = null;
            this.chartListBox1.SeriesGroup = null;
            this.chartListBox1.Size = new Size(0xc2, 200);
            this.chartListBox1.TabIndex = 0;
            this.chartListBox1.SelectedIndexChanged += new EventHandler(this.chartListBox1_SelectedIndexChanged);
            this.chartListBox1.DoubleClick += new EventHandler(this.chartListBox1_DoubleClick);
            this.LBGroups.AllowDrop = true;
            this.LBGroups.Dock = DockStyle.Left;
            this.LBGroups.IntegralHeight = false;
            this.LBGroups.Location = new Point(0, 0);
            this.LBGroups.Name = "LBGroups";
            this.LBGroups.Size = new Size(0x58, 200);
            this.LBGroups.TabIndex = 0;
            this.LBGroups.DragOver += new DragEventHandler(this.LBGroups_DragOver);
            this.LBGroups.SelectedIndexChanged += new EventHandler(this.LBGroups_SelectedIndexChanged);
            this.LBGroups.ItemCheck += new ItemCheckEventHandler(this.LBGroups_ItemCheck);
            this.LBGroups.DragDrop += new DragEventHandler(this.LBGroups_DragDrop);
            this.panel3.Controls.Add(this.BRenameGroup);
            this.panel3.Controls.Add(this.BDeleteGroup);
            this.panel3.Controls.Add(this.BAddGroup);
            this.panel3.Dock = DockStyle.Top;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x11d, 0x18);
            this.panel3.TabIndex = 0;
            this.BRenameGroup.Enabled = false;
            this.BRenameGroup.FlatStyle = FlatStyle.Flat;
            this.BRenameGroup.Location = new Point(0x3f, 2);
            this.BRenameGroup.Name = "BRenameGroup";
            this.BRenameGroup.Size = new Size(0x18, 0x13);
            this.BRenameGroup.TabIndex = 2;
            this.BRenameGroup.Text = "...";
            this.BRenameGroup.Click += new EventHandler(this.BRenameGroup_Click);
            this.BDeleteGroup.Enabled = false;
            this.BDeleteGroup.FlatStyle = FlatStyle.Flat;
            this.BDeleteGroup.Location = new Point(0x23, 2);
            this.BDeleteGroup.Name = "BDeleteGroup";
            this.BDeleteGroup.Size = new Size(0x18, 0x13);
            this.BDeleteGroup.TabIndex = 1;
            this.BDeleteGroup.Text = "-";
            this.BDeleteGroup.Click += new EventHandler(this.BDeleteGroup_Click);
            this.BAddGroup.FlatStyle = FlatStyle.Flat;
            this.BAddGroup.Location = new Point(8, 2);
            this.BAddGroup.Name = "BAddGroup";
            this.BAddGroup.Size = new Size(0x18, 0x13);
            this.BAddGroup.TabIndex = 0;
            this.BAddGroup.Text = "+";
            this.BAddGroup.Click += new EventHandler(this.BAddGroup_Click);
            this.panel1.Controls.Add(this.bSeriesDown);
            this.panel1.Controls.Add(this.bSeriesUp);
            this.panel1.Controls.Add(this.buttonChangeSeries);
            this.panel1.Controls.Add(this.buttonCloneSeries);
            this.panel1.Controls.Add(this.buttonTitleSeries);
            this.panel1.Controls.Add(this.buttonDeleteSeries);
            this.panel1.Controls.Add(this.buttonAddSeries);
            this.panel1.Dock = DockStyle.Right;
            this.panel1.Location = new Point(0x11d, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x69, 0xe0);
            this.panel1.TabIndex = 1;
            this.bSeriesDown.BackColor = Color.Silver;
            this.bSeriesDown.Enabled = false;
            this.bSeriesDown.FlatStyle = FlatStyle.Flat;
            this.bSeriesDown.Location = new Point(0x3a, 0x10);
            this.bSeriesDown.Name = "bSeriesDown";
            this.bSeriesDown.Size = new Size(0x18, 0x17);
            this.bSeriesDown.TabIndex = 1;
            this.bSeriesDown.Click += new EventHandler(this.button6_Click);
            this.bSeriesUp.BackColor = Color.Silver;
            this.bSeriesUp.Enabled = false;
            this.bSeriesUp.FlatStyle = FlatStyle.Flat;
            this.bSeriesUp.Location = new Point(0x18, 0x10);
            this.bSeriesUp.Name = "bSeriesUp";
            this.bSeriesUp.Size = new Size(0x19, 0x17);
            this.bSeriesUp.TabIndex = 0;
            this.bSeriesUp.Click += new EventHandler(this.button5_Click);
            this.buttonChangeSeries.Enabled = false;
            this.buttonChangeSeries.FlatStyle = FlatStyle.Flat;
            this.buttonChangeSeries.Location = new Point(0x12, 0xbf);
            this.buttonChangeSeries.Name = "buttonChangeSeries";
            this.buttonChangeSeries.Size = new Size(0x4b, 0x17);
            this.buttonChangeSeries.TabIndex = 6;
            this.buttonChangeSeries.Text = "&Change...";
            this.buttonChangeSeries.Click += new EventHandler(this.buttonChangeSeries_Click);
            this.buttonCloneSeries.Enabled = false;
            this.buttonCloneSeries.FlatStyle = FlatStyle.Flat;
            this.buttonCloneSeries.Location = new Point(0x12, 0x9b);
            this.buttonCloneSeries.Name = "buttonCloneSeries";
            this.buttonCloneSeries.Size = new Size(0x4b, 0x17);
            this.buttonCloneSeries.TabIndex = 5;
            this.buttonCloneSeries.Text = "Cl&one";
            this.buttonCloneSeries.Click += new EventHandler(this.buttonCloneSeries_Click);
            this.buttonTitleSeries.Enabled = false;
            this.buttonTitleSeries.FlatStyle = FlatStyle.Flat;
            this.buttonTitleSeries.Location = new Point(0x12, 0x77);
            this.buttonTitleSeries.Name = "buttonTitleSeries";
            this.buttonTitleSeries.Size = new Size(0x4b, 0x17);
            this.buttonTitleSeries.TabIndex = 4;
            this.buttonTitleSeries.Text = "&Title...";
            this.buttonTitleSeries.Click += new EventHandler(this.buttonTitleSeries_Click);
            this.buttonDeleteSeries.Enabled = false;
            this.buttonDeleteSeries.FlatStyle = FlatStyle.Flat;
            this.buttonDeleteSeries.Location = new Point(0x12, 0x53);
            this.buttonDeleteSeries.Name = "buttonDeleteSeries";
            this.buttonDeleteSeries.Size = new Size(0x4b, 0x17);
            this.buttonDeleteSeries.TabIndex = 3;
            this.buttonDeleteSeries.Text = "&Delete...";
            this.buttonDeleteSeries.Click += new EventHandler(this.buttonDeleteSeries_Click);
            this.buttonAddSeries.FlatStyle = FlatStyle.Flat;
            this.buttonAddSeries.Location = new Point(0x12, 0x2f);
            this.buttonAddSeries.Name = "buttonAddSeries";
            this.buttonAddSeries.Size = new Size(0x4b, 0x17);
            this.buttonAddSeries.TabIndex = 2;
            this.buttonAddSeries.Text = "&Add...";
            this.buttonAddSeries.Click += new EventHandler(this.button2_Click);
            this.tabPanel.Location = new Point(4, 0x16);
            this.tabPanel.Name = "tabPanel";
            this.tabPanel.Size = new Size(390, 0xe0);
            this.tabPanel.TabIndex = 5;
            this.tabPanel.Text = "Panel";
            this.tabAxes.Location = new Point(4, 0x16);
            this.tabAxes.Name = "tabAxes";
            this.tabAxes.Size = new Size(390, 0xe0);
            this.tabAxes.TabIndex = 2;
            this.tabAxes.Text = "Axes";
            this.tabGeneral.Location = new Point(4, 0x16);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new Size(390, 0xe0);
            this.tabGeneral.TabIndex = 1;
            this.tabGeneral.Text = "General";
            this.tabTitles.Location = new Point(4, 0x16);
            this.tabTitles.Name = "tabTitles";
            this.tabTitles.Size = new Size(390, 0xe0);
            this.tabTitles.TabIndex = 4;
            this.tabTitles.Text = "Titles";
            this.tabWalls.Controls.Add(this.tabControl3);
            this.tabWalls.Controls.Add(this.checkBox2);
            this.tabWalls.Location = new Point(4, 0x16);
            this.tabWalls.Name = "tabWalls";
            this.tabWalls.Size = new Size(390, 0xe0);
            this.tabWalls.TabIndex = 7;
            this.tabWalls.Text = "Walls";
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Controls.Add(this.tabPage9);
            this.tabControl3.Controls.Add(this.tabPage10);
            this.tabControl3.Controls.Add(this.tabPage11);
            this.tabControl3.HotTrack = true;
            this.tabControl3.Location = new Point(0x3d, 0x20);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new Size(0x116, 0xb7);
            this.tabControl3.TabIndex = 1;
            this.tabControl3.SelectedIndexChanged += new EventHandler(this.tabControl3_SelectedIndexChanged);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(270, 0x9d);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Left";
            this.tabPage9.Location = new Point(4, 0x16);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Size = new Size(270, 0x9d);
            this.tabPage9.TabIndex = 1;
            this.tabPage9.Text = "Right";
            this.tabPage10.Location = new Point(4, 0x16);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Size = new Size(270, 0x9d);
            this.tabPage10.TabIndex = 2;
            this.tabPage10.Text = "Back";
            this.tabPage11.Location = new Point(4, 0x16);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new Size(270, 0x9d);
            this.tabPage11.TabIndex = 3;
            this.tabPage11.Text = "Bottom";
            this.checkBox2.FlatStyle = FlatStyle.Flat;
            this.checkBox2.Location = new Point(7, 2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(0xe1, 0x18);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Visible Walls";
            this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.tabPaging.Location = new Point(4, 0x16);
            this.tabPaging.Name = "tabPaging";
            this.tabPaging.Size = new Size(390, 0xe0);
            this.tabPaging.TabIndex = 6;
            this.tabPaging.Text = "Paging";
            this.tabLegend.Location = new Point(4, 0x16);
            this.tabLegend.Name = "tabLegend";
            this.tabLegend.Size = new Size(390, 0xe0);
            this.tabLegend.TabIndex = 3;
            this.tabLegend.Text = "Legend";
            this.tabAspect.Location = new Point(4, 0x16);
            this.tabAspect.Name = "tabAspect";
            this.tabAspect.Size = new Size(390, 0xe0);
            this.tabAspect.TabIndex = 8;
            this.tabAspect.Text = "3D";
            this.tabOpenGL.Location = new Point(4, 0x19);
            this.tabOpenGL.Name = "tabOpenGL";
            this.tabOpenGL.Size = new Size(0x18e, 250);
            this.tabOpenGL.TabIndex = 8;
            this.tabOpenGL.Text = "OpenGL";
            this.panel6.Location = new Point(0x110, 8);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(0x18, 0x18);
            this.panel6.TabIndex = 0;
            this.tabControl1.Appearance = TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabChart);
            this.tabControl1.Controls.Add(this.tabPageSeries);
            this.tabControl1.Controls.Add(this.tabData);
            this.tabControl1.Controls.Add(this.tabPrint);
            this.tabControl1.Controls.Add(this.tabExport);
            this.tabControl1.Controls.Add(this.tabTools);
            this.tabControl1.Controls.Add(this.tabThemes);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new Size(0x196, 0x117);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabChart.Controls.Add(this.tabControl2);
            this.tabChart.Location = new Point(4, 0x19);
            this.tabChart.Name = "tabChart";
            this.tabChart.Size = new Size(0x18e, 250);
            this.tabChart.TabIndex = 0;
            this.tabChart.Text = "Chart";
            this.tabPageSeries.Controls.Add(this.panelSeries);
            this.tabPageSeries.Controls.Add(this.panel8);
            this.tabPageSeries.Location = new Point(4, 0x19);
            this.tabPageSeries.Name = "tabPageSeries";
            this.tabPageSeries.Size = new Size(0x18e, 250);
            this.tabPageSeries.TabIndex = 1;
            this.tabPageSeries.Text = "Series";
            this.panelSeries.Dock = DockStyle.Fill;
            this.panelSeries.Location = new Point(0, 0x20);
            this.panelSeries.Name = "panelSeries";
            this.panelSeries.Size = new Size(0x18e, 0xda);
            this.panelSeries.TabIndex = 1;
            this.panel8.Controls.Add(this.label4);
            this.panel8.Controls.Add(this.panel10);
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Controls.Add(this.pictureBox2);
            this.panel8.Controls.Add(this.CBSeries);
            this.panel8.Dock = DockStyle.Top;
            this.panel8.Location = new Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new Size(0x18e, 0x20);
            this.panel8.TabIndex = 0;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xce, 11);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0, 0x10);
            this.label4.TabIndex = 4;
            this.label4.UseMnemonic = false;
            this.panel10.Controls.Add(this.label7);
            this.panel10.Dock = DockStyle.Right;
            this.panel10.Location = new Point(360, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new Size(0x26, 0x20);
            this.panel10.TabIndex = 3;
            this.label7.BackColor = Color.Red;
            this.label7.BorderStyle = BorderStyle.Fixed3D;
            this.label7.Cursor = Cursors.Hand;
            this.label7.Enabled = false;
            this.label7.FlatStyle = FlatStyle.Flat;
            this.label7.Location = new Point(6, 5);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x18, 0x18);
            this.label7.TabIndex = 0;
            this.label7.UseMnemonic = false;
            this.label7.Click += new EventHandler(this.label7_Click);
            this.panel9.Location = new Point(0x134, 6);
            this.panel9.Name = "panel9";
            this.panel9.Size = new Size(1, 1);
            this.panel9.TabIndex = 2;
            this.pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            this.pictureBox2.Location = new Point(0xa9, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(0x18, 0x18);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.CBSeries.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBSeries.DropDownWidth = 160;
            this.CBSeries.Location = new Point(4, 6);
            this.CBSeries.Name = "CBSeries";
            this.CBSeries.Size = new Size(160, 0x15);
            this.CBSeries.TabIndex = 0;
            this.CBSeries.SelectedIndexChanged += new EventHandler(this.CBSeries_SelectedIndexChanged);
            this.tabData.Controls.Add(this.panel4);
            this.tabData.Location = new Point(4, 0x19);
            this.tabData.Name = "tabData";
            this.tabData.Size = new Size(0x18e, 250);
            this.tabData.TabIndex = 7;
            this.tabData.Text = "Data";
            this.panel4.Controls.Add(this.dataGrid1);
            this.panel4.Dock = DockStyle.Fill;
            this.panel4.Location = new Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(0x18e, 250);
            this.panel4.TabIndex = 0;
            this.dataGrid1.DataMember = "";
            this.dataGrid1.Dock = DockStyle.Fill;
            this.dataGrid1.HeaderForeColor = SystemColors.ControlText;
            this.dataGrid1.Location = new Point(0, 0);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new Size(0x18e, 250);
            this.dataGrid1.TabIndex = 0;
            this.tabPrint.Location = new Point(4, 0x19);
            this.tabPrint.Name = "tabPrint";
            this.tabPrint.Size = new Size(0x18e, 250);
            this.tabPrint.TabIndex = 4;
            this.tabPrint.Text = "Print";
            this.tabExport.Location = new Point(4, 0x19);
            this.tabExport.Name = "tabExport";
            this.tabExport.Size = new Size(0x18e, 250);
            this.tabExport.TabIndex = 3;
            this.tabExport.Text = "Export";
            this.tabTools.Location = new Point(4, 0x19);
            this.tabTools.Name = "tabTools";
            this.tabTools.Size = new Size(0x18e, 250);
            this.tabTools.TabIndex = 5;
            this.tabTools.Text = "Tools";
            this.tabThemes.Location = new Point(4, 0x19);
            this.tabThemes.Name = "tabThemes";
            this.tabThemes.Size = new Size(0x18e, 250);
            this.tabThemes.TabIndex = 6;
            this.tabThemes.Text = "Themes";
            this.panel2.BorderStyle = BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.linkLabel1);
            this.panel2.Controls.Add(this.bHelp);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 0x117);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x196, 0x26);
            this.panel2.TabIndex = 1;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.FlatStyle = FlatStyle.Flat;
            this.linkLabel1.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.linkLabel1.Location = new Point(0x8e, 10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(0x72, 0x10);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "www.Steema.com";
            this.linkLabel1.UseMnemonic = false;
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.bHelp.FlatStyle = FlatStyle.Flat;
            this.bHelp.Location = new Point(0x18, 7);
            this.bHelp.Name = "bHelp";
            this.bHelp.Size = new Size(0x4b, 0x17);
            this.bHelp.TabIndex = 0;
            this.bHelp.Text = "&Help...";
            this.bHelp.Click += new EventHandler(this.button3_Click);
            this.button1.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.button1.DialogResult = DialogResult.Cancel;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(310, 8);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.Click += new EventHandler(this.button1_Click);
            base.CancelButton = this.button1;
            base.ClientSize = new Size(0x196, 0x13d);
            base.Controls.Add(this.tabControl1);
            base.Controls.Add(this.panel2);
            base.HelpButton = true;
            base.Name = "ChartEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TeeChart Editor";
            base.Load += new EventHandler(this.ChartEditor_Load);
            base.FormClosing += new FormClosingEventHandler(this.ChartEditor_FormClosing);
            this.tabControl2.ResumeLayout(false);
            this.tabSeries.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabWalls.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabChart.ResumeLayout(false);
            this.tabPageSeries.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.dataGrid1.EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            base.ResumeLayout(false);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.label7.BackColor = ColorEditor.Choose(this.label7.BackColor, this);
            this.Series.Color = this.label7.BackColor;
        }

        private void LBGroups_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.LBGroups.PointToClient(new Point(e.X, e.Y));
            int index = this.LBGroups.IndexFromPoint(p);
            string[] formats = e.Data.GetFormats();
            if (((index >= 0) && (formats.Length > 0)) && ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy))
            {
                Steema.TeeChart.Styles.Series data = (Steema.TeeChart.Styles.Series) e.Data.GetData(formats[0]);
                if ((index == 0) && (this.DeltaGroup == 1))
                {
                    if (this.LBGroups.SelectedIndex >= this.DeltaGroup)
                    {
                        this.CurrentGroup().Series.Remove(data);
                    }
                    this.RefreshGroups(index);
                }
                else if (this.Chart.series.Groups[index - this.DeltaGroup].Series.IndexOf(data) < 0)
                {
                    this.Chart.series.Groups[index - this.DeltaGroup].Series.Add(data);
                    if (this.LBGroups.SelectedIndex >= this.DeltaGroup)
                    {
                        this.CurrentGroup().Series.Remove(data);
                    }
                    this.RefreshGroups(index);
                }
            }
        }

        private void LBGroups_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            Point p = this.LBGroups.PointToClient(new Point(e.X, e.Y));
            int num = this.LBGroups.IndexFromPoint(p);
            string[] formats = e.Data.GetFormats();
            if ((num >= 0) && (formats.Length > 0))
            {
                object data = e.Data.GetData(formats[0]);
                if (data is Steema.TeeChart.Styles.Series)
                {
                    Steema.TeeChart.Styles.Series s = (Steema.TeeChart.Styles.Series) data;
                    if ((s.chart == this.Chart) && ((num < this.DeltaGroup) || (this.Chart.series.Groups[num - this.DeltaGroup].Series.IndexOf(s) < 0)))
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private void LBGroups_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if ((this.DeltaGroup == 1) && (e.Index == 0))
            {
                this.Chart.series.AllActive = e.NewValue == CheckState.Checked;
                this.FillGroups();
            }
            else
            {
                this.LBGroups.ItemCheck -= new ItemCheckEventHandler(this.LBGroups_ItemCheck);
                try
                {
                    switch (e.NewValue)
                    {
                        case CheckState.Unchecked:
                            this.CurrentGroup().Active = SeriesGroupActive.No;
                            break;

                        case CheckState.Checked:
                            this.CurrentGroup().Active = SeriesGroupActive.Yes;
                            break;

                        case CheckState.Indeterminate:
                            this.CurrentGroup().Active = SeriesGroupActive.Some;
                            break;
                    }
                    if (this.DeltaGroup == 1)
                    {
                        this.LBGroups.SetItemChecked(0, this.Chart.series.AllActive);
                    }
                }
                finally
                {
                    this.LBGroups.ItemCheck += new ItemCheckEventHandler(this.LBGroups_ItemCheck);
                }
            }
            this.EnableGroupButtons();
        }

        private void LBGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chartListBox1.SeriesGroup = this.CurrentGroup();
            this.EnableGroupButtons();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.Links[this.linkLabel1.Links.IndexOf(e.Link)].Visited = true;
            Process.Start(this.linkLabel1.Text);
        }

        private void RecursiveOne(Control[] c)
        {
            foreach (Control control in c)
            {
                if (control is TabControl)
                {
                    (control as TabControl).HotTrack = true;
                    Control[] array = new Control[control.Controls.Count];
                    control.Controls.CopyTo(array, 0);
                    this.RecursiveTwo(array);
                }
            }
        }

        private void RecursiveTwo(Control[] c)
        {
            foreach (Control control in c)
            {
                if (control is TabControl)
                {
                    (control as TabControl).HotTrack = true;
                    Control[] array = new Control[control.Controls.Count];
                    control.Controls.CopyTo(array, 0);
                    this.RecursiveOne(array);
                }
            }
        }

        private void RefreshGroups(int index)
        {
            this.FillGroups();
            if (index < this.LBGroups.Items.Count)
            {
                this.LBGroups.SelectedIndex = index;
                this.LBGroups_SelectedIndexChanged(this.LBGroups, null);
            }
        }

        internal void SetDefaultTab(ChartEditorTabs defaultTab)
        {
            switch (defaultTab)
            {
                case ChartEditorTabs.Main:
                    break;

                case ChartEditorTabs.General:
                    this.tabControl2.SelectedTab = this.tabGeneral;
                    this.tabControl2_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Axes:
                    this.tabControl2.SelectedTab = this.tabAxes;
                    this.tabControl2_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Legend:
                    this.tabControl2.SelectedTab = this.tabLegend;
                    this.tabControl2_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Walls:
                    this.tabControl2.SelectedTab = this.tabWalls;
                    this.tabControl2_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Aspect:
                    this.tabControl2.SelectedTab = this.tabAspect;
                    this.tabControl2_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Panel:
                    this.tabControl2.SelectedTab = this.tabPanel;
                    this.tabControl2_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Page:
                    this.tabControl2.SelectedTab = this.tabPaging;
                    this.tabControl2_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Series:
                    this.tabControl1.SelectedIndex = this.tabControl1.TabPages.IndexOf(this.tabPageSeries);
                    this.tabControl1_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Tools:
                    this.tabControl1.SelectedTab = this.tabTools;
                    this.tabControl1_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Titles:
                    this.tabControl2.SelectedTab = this.tabTitles;
                    this.tabControl2_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Export:
                    this.tabControl1.SelectedTab = this.tabExport;
                    this.tabControl1_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Print:
                    this.tabControl1.SelectedTab = this.tabPrint;
                    this.tabControl1_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Themes:
                    this.tabControl1.SelectedTab = this.tabThemes;
                    this.tabControl1_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.Data:
                    this.tabControl1.SelectedTab = this.tabData;
                    this.tabControl1_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.SeriesDataSource:
                    this.tabControl1.SelectedTab = this.tabPageSeries;
                    if (this.CBSeries.SelectedIndex != -1)
                    {
                        this.seriesEditor.tabControl1.SelectedTab = this.seriesEditor.tabSource;
                    }
                    this.tabControl1_SelectedIndexChanged(null, EventArgs.Empty);
                    return;

                case ChartEditorTabs.SeriesList:
                    this.tabControl2.SelectedTab = this.tabSeries;
                    this.tabControl2_SelectedIndexChanged(null, EventArgs.Empty);
                    break;

                default:
                    return;
            }
        }

        internal void SetHideTabs(ChartEditorTabs[] hideTabs)
        {
            TabControl control = new TabControl();
            SeriesEditor.HideSourceTab = false;
            ChartEditorTabs[] tabsArray = hideTabs;
            for (int i = 0; i < tabsArray.Length; i++)
            {
                switch (tabsArray[i])
                {
                    case ChartEditorTabs.Main:
                        this.tabChart.Parent = control;
                        break;

                    case ChartEditorTabs.General:
                        this.tabGeneral.Parent = control;
                        break;

                    case ChartEditorTabs.Axes:
                        this.tabAxes.Parent = control;
                        break;

                    case ChartEditorTabs.Legend:
                        this.tabLegend.Parent = control;
                        break;

                    case ChartEditorTabs.Walls:
                        this.tabWalls.Parent = control;
                        break;

                    case ChartEditorTabs.Aspect:
                        this.tabAspect.Parent = control;
                        break;

                    case ChartEditorTabs.Panel:
                        this.tabPanel.Parent = control;
                        break;

                    case ChartEditorTabs.Page:
                        this.tabPaging.Parent = control;
                        break;

                    case ChartEditorTabs.Series:
                        this.tabPageSeries.Parent = control;
                        break;

                    case ChartEditorTabs.Tools:
                        this.tabTools.Parent = control;
                        break;

                    case ChartEditorTabs.Titles:
                        this.tabTitles.Parent = control;
                        break;

                    case ChartEditorTabs.Export:
                        this.tabExport.Parent = control;
                        break;

                    case ChartEditorTabs.Print:
                        this.tabPrint.Parent = control;
                        break;

                    case ChartEditorTabs.Themes:
                        this.tabThemes.Parent = control;
                        break;

                    case ChartEditorTabs.Data:
                        this.tabData.Parent = control;
                        break;

                    case ChartEditorTabs.SeriesDataSource:
                        SeriesEditor.HideSourceTab = true;
                        this.seriesEditor.tabSource.Parent = control;
                        this.seriesEditor.tabControl1.SelectedIndex = 0;
                        break;

                    case ChartEditorTabs.SeriesList:
                        this.tabSeries.Parent = control;
                        break;
                }
            }
            this.tabControl1.SelectedTab = this.tabChart;
            this.tabControl2.SelectedTab = this.tabSeries;
        }

        internal void SetHighLightTabs()
        {
            Control[] array = new Control[base.Controls.Count];
            array = new Control[base.Controls.Count];
            base.Controls.CopyTo(array, 0);
            this.RecursiveOne(array);
        }

        internal void SetOptions(ChartEditorOptions[] options)
        {
            this.buttonAddSeries.Visible = false;
            this.buttonChangeSeries.Visible = false;
            this.buttonCloneSeries.Visible = false;
            this.buttonDeleteSeries.Visible = false;
            this.bHelp.Visible = false;
            this.buttonTitleSeries.Visible = false;
            this.chartListBox1.EnableChangeType = false;
            this.ActivateGroup(false);
            this.DeltaGroup = 0;
            ChartEditorOptions[] optionsArray = options;
            for (int i = 0; i < optionsArray.Length; i++)
            {
                switch (optionsArray[i])
                {
                    case ChartEditorOptions.Add:
                        this.buttonAddSeries.Visible = true;
                        break;

                    case ChartEditorOptions.Delete:
                        this.buttonDeleteSeries.Visible = true;
                        break;

                    case ChartEditorOptions.Change:
                        this.buttonChangeSeries.Visible = true;
                        this.chartListBox1.EnableChangeType = true;
                        break;

                    case ChartEditorOptions.Clone:
                        this.buttonCloneSeries.Visible = true;
                        break;

                    case ChartEditorOptions.Title:
                        this.buttonTitleSeries.Visible = true;
                        break;

                    case ChartEditorOptions.Help:
                        this.bHelp.Visible = true;
                        break;

                    case ChartEditorOptions.Groups:
                        this.ActivateGroup(true);
                        break;

                    case ChartEditorOptions.GroupAll:
                        this.DeltaGroup = 1;
                        break;
                }
            }
        }

        public static bool ShowModal(Steema.TeeChart.Chart c)
        {
            return ShowModal(c, ChartEditorTabs.Main);
        }

        public static bool ShowModal(Steema.TeeChart.Styles.Series s)
        {
            return ShowModal(s, ChartEditorTabs.Main);
        }

        public static bool ShowModal(Steema.TeeChart.Chart c, ChartEditorTabs defaultTab)
        {
            using (ChartEditor editor = new ChartEditor(c))
            {
                editor.SetDefaultTab(defaultTab);
                return (editor.ShowDialog() == DialogResult.OK);
            }
        }

        public static bool ShowModal(Steema.TeeChart.Styles.Series s, ChartEditorTabs defaultTab)
        {
            using (ChartEditor editor = new ChartEditor(s.chart))
            {
                editor.ShowSeriesEditor(s);
                editor.SetDefaultTab(defaultTab);
                return (editor.ShowDialog() == DialogResult.OK);
            }
        }

        internal void ShowSeriesEditor(Steema.TeeChart.Styles.Series s)
        {
            this.chartListBox1.SelectedSeries = s;
            this.CBSeries.SelectedItem = s;
            this.tabControl1.SelectedTab = this.tabPageSeries;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabData)
            {
                Steema.TeeChart.Styles.Series selectedSeries = this.chartListBox1.SelectedSeries;
                this.dataGrid1.DataBindings.Clear();
                if (selectedSeries != null)
                {
                    DataSet chartData;
                    if (!(selectedSeries.DataSource is DataSet))
                    {
                        if (selectedSeries.DataSource is Steema.TeeChart.Styles.Series)
                        {
                            chartData = selectedSeries.GetChartData();
                            this.dataTable = chartData.Tables[0];
                            this.dataGrid1.SetDataBinding(chartData, this.dataTable.TableName);
                            this.dataTable.RowChanged += new DataRowChangeEventHandler(this.dataGrid1_DataSourceChanged);
                        }
                    }
                    else
                    {
                        chartData = selectedSeries.DataSource as DataSet;
                        if (chartData.Tables.Count == 1)
                        {
                            this.dataTable = chartData.Tables[0];
                            this.dataGrid1.SetDataBinding(chartData, this.dataTable.TableName);
                            this.dataTable.RowChanged += new DataRowChangeEventHandler(this.dataGrid1_DataSourceChanged);
                        }
                    }
                }
            }
            else if ((this.tabControl1.SelectedTab == this.tabPrint) & (this.printPreview == null))
            {
                this.printPreview = new PrintPreview(this.Chart, this.tabPrint);
                EditorUtils.Translate(this.printPreview);
            }
            else if (this.tabControl1.SelectedTab == this.tabPrint)
            {
                this.printPreview.RefreshView();
            }
            else if ((this.tabControl1.SelectedTab == this.tabExport) & (this.exportEditor == null))
            {
                this.exportEditor = new ExportEditor(this.Chart, this.tabExport);
                EditorUtils.Translate(this.exportEditor);
            }
            else if (this.tabControl1.SelectedTab == this.tabTools)
            {
                if (this.toolsEditor == null)
                {
                    this.toolsEditor = new ToolsEditor(this.Chart.Tools, this.tabTools);
                    ArrayList excludeChildren = new ArrayList();
                    excludeChildren.Add(this.toolsEditor.toolsForm);
                    EditorUtils.Translate(this.toolsEditor, excludeChildren);
                }
                else
                {
                    this.toolsEditor.Reload();
                }
            }
            else if (this.tabControl1.SelectedTab == this.tabPageSeries)
            {
                int selectedIndex = this.chartListBox1.SelectedIndex;
                this.AddSeries();
                this.chartListBox1.ClearSelected();
                this.chartListBox1.SelectedIndex = selectedIndex;
                if (this.CBSeries.SelectedIndex != this.chartListBox1.SelectedIndex)
                {
                    this.CBSeries.SelectedIndex = this.chartListBox1.SelectedIndex;
                }
            }
            else if (this.tabControl1.SelectedTab == this.tabThemes)
            {
                if (this.themeEditor == null)
                {
                    this.themeEditor = new ThemeEditor(this.Chart, this.tabThemes);
                    EditorUtils.Translate(this.themeEditor);
                }
                else
                {
                    this.themeEditor.InitializeThemePainter();
                }
            }
            else if (((this.tabControl1.SelectedTab == this.tabOpenGL) && (this.tabOpenGL.Parent != null)) && (this.glEditor == null))
            {
                this.glEditor = new GLEditor(this.Chart, this.tabOpenGL);
                EditorUtils.Translate(this.glEditor);
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl2.SelectedTab == this.tabWalls)
            {
                this.checkBox2.Checked = this.Chart.Walls.Visible;
                this.tabControl3_SelectedIndexChanged(sender, e);
            }
            else if (this.tabControl2.SelectedTab == this.tabPanel)
            {
                this.panelEditor = new Steema.TeeChart.Editors.PanelEditor(this.Chart.Panel, this.tabPanel);
                EditorUtils.Translate(this.panelEditor);
            }
            else if ((this.tabControl2.SelectedTab == this.tabAxes) & (this.axesEditor == null))
            {
                this.axesEditor = new AxesEditor(this.Chart, this.tabAxes);
                EditorUtils.Translate(this.axesEditor);
            }
            else if ((this.tabControl2.SelectedTab == this.tabAspect) & (this.aspectEditor == null))
            {
                this.aspectEditor = new AspectEditor(this.Chart, this.tabAspect);
                EditorUtils.Translate(this.aspectEditor);
            }
            else if ((this.tabControl2.SelectedTab == this.tabLegend) & (this.legendEditor == null))
            {
                this.legendEditor = new Steema.TeeChart.Editors.LegendEditor(this.Chart, this.tabLegend);
                EditorUtils.Translate(this.legendEditor);
            }
            else if ((this.tabControl2.SelectedTab == this.tabGeneral) & (this.generalEditor == null))
            {
                this.generalEditor = new GeneralEditor(this.Chart, this.tabGeneral);
                ArrayList excludeChildren = new ArrayList();
                excludeChildren.Add(this.generalEditor.cbCursor);
                EditorUtils.Translate(this.generalEditor, excludeChildren);
            }
            else if ((this.tabControl2.SelectedTab == this.tabTitles) & (this.titlesEditor == null))
            {
                this.titlesEditor = new TitleEditor(this.Chart, this.tabTitles);
                ArrayList list2 = new ArrayList();
                list2.Add(this.titlesEditor.textTitle);
                EditorUtils.Translate(this.titlesEditor, list2);
            }
            else if (this.tabControl2.SelectedTab == this.tabPaging)
            {
                this.pageEditor = new Steema.TeeChart.Editors.PageEditor(this.Chart.Page, this.tabPaging);
                EditorUtils.Translate(this.pageEditor);
            }
        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.wallEditor[this.tabControl3.SelectedIndex] == null)
            {
                this.wallEditor[this.tabControl3.SelectedIndex] = new Steema.TeeChart.Editors.WallEditor(this.CurrentWall(), this.tabControl3.SelectedTab);
                EditorUtils.Translate(this.wallEditor[this.tabControl3.SelectedIndex]);
            }
        }

        public void TeeEvent(Steema.TeeChart.TeeEvent e)
        {
            if ((e is SeriesEvent) && (((SeriesEvent) e).Event == SeriesEventStyle.ChangeColor))
            {
                this.label7.BackColor = ((SeriesEvent) e).Series.Color;
            }
        }

        private Steema.TeeChart.Styles.Series Series
        {
            get
            {
                if ((this.chartListBox1.Items.Count != 0) && (this.chartListBox1.SelectedItem != null))
                {
                    return (this.chartListBox1.SelectedItem as Steema.TeeChart.Styles.Series);
                }
                return null;
            }
        }
    }
}

