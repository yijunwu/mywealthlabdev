namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SubChartEditor : Form
    {
        private Button bAdd;
        private Button bDelete;
        private Button bDown;
        private Button bRename;
        private Button bUp;
        private CheckBox cbTransparent;
        private ChartEditor chartEditor;
        private IContainer components;
        private ImageList imageList1;
        private ListBox lbCharts;
        private Label lHeight;
        private Label lLeft;
        private Label lTop;
        private Label lWidth;
        private bool oldGradientVisible;
        private System.Windows.Forms.Panel panel1;
        private TabPage tabChart;
        private TabControl tabControl;
        private TabPage tabPosition;
        private SubChartTool tool;
        private NumericUpDown udHeight;
        private NumericUpDown udLeft;
        private NumericUpDown udTop;
        private NumericUpDown udWidth;

        public SubChartEditor()
        {
            this.InitializeComponent();
        }

        public SubChartEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.tool = s as SubChartTool;
            this.lbCharts.Items.Clear();
            this.udWidth.TextChanged += new EventHandler(this.udWidth_ValueChanged);
            this.udTop.TextChanged += new EventHandler(this.udTop_ValueChanged);
            this.udHeight.TextChanged += new EventHandler(this.udHeight_ValueChanged);
            this.udLeft.TextChanged += new EventHandler(this.udLeft_ValueChanged);
            if (this.tool != null)
            {
                for (int i = 0; i < this.tool.Charts.Count; i++)
                {
                    if (Utils.IsNullOrEmpty(this.tool.Charts[i].Chart.Name))
                    {
                        this.lbCharts.Items.Add("Chart" + Convert.ToString((int) (i + 1)));
                    }
                    else
                    {
                        this.lbCharts.Items.Add(this.tool.Charts[i].Chart.Name);
                    }
                }
            }
            if (this.lbCharts.Items.Count > 0)
            {
                this.lbCharts.SelectedIndex = 0;
            }
            this.lbCharts_Click(this, EventArgs.Empty);
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            string str = "Chart" + this.tool.Charts.Count.ToString();
            if (TextInput.Query(Texts.SubChartNewChart, Texts.SubChartChartName + ":", ref str))
            {
                this.tool.Charts.AddChart(str);
                this.lbCharts.Items.Add(str);
                this.lbCharts.SelectedIndex = this.lbCharts.Items.Count - 1;
                this.lbCharts_Click(this, EventArgs.Empty);
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (Utils.YesNoDelete(this.CurrentChartName()))
            {
                this.tool.Charts.RemoveAt(this.lbCharts.SelectedIndex);
                this.lbCharts.Items.RemoveAt(this.lbCharts.SelectedIndex);
                this.lbCharts_Click(this, EventArgs.Empty);
            }
        }

        private void bDown_Click(object sender, EventArgs e)
        {
            if ((this.lbCharts.SelectedIndex != -1) && (this.lbCharts.SelectedIndex < (this.lbCharts.Items.Count - 1)))
            {
                this.SwapChart(this.lbCharts.SelectedIndex, this.lbCharts.SelectedIndex + 1);
            }
        }

        private void bRename_Click(object sender, EventArgs e)
        {
            string str = this.CurrentChartName();
            if (TextInput.Query(Texts.SubChartRenameChart, Texts.SubChartNewName + ":", ref str))
            {
                this.CurrentChart().Chart.Name = str;
                this.CurrentChart().Chart.Header.Text = str;
                this.lbCharts.Items[this.lbCharts.SelectedIndex] = str;
            }
        }

        private void bUp_Click(object sender, EventArgs e)
        {
            if (this.lbCharts.SelectedIndex > 0)
            {
                this.SwapChart(this.lbCharts.SelectedIndex, this.lbCharts.SelectedIndex - 1);
            }
        }

        private void cbTransparent_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbTransparent.Checked)
            {
                this.oldGradientVisible = this.CurrentChart().Chart.Panel.Gradient.Visible;
                this.CurrentChart().Chart.Panel.Gradient.Visible = false;
                this.CurrentChart().Chart.Panel.Color = Color.Transparent;
            }
            else
            {
                this.CurrentChart().Chart.Panel.Gradient.Visible = this.oldGradientVisible;
                this.CurrentChart().Chart.Panel.Color = SystemColors.Control;
            }
        }

        private SubChart CurrentChart()
        {
            return this.tool.Charts[this.lbCharts.SelectedIndex];
        }

        private string CurrentChartName()
        {
            string name = this.CurrentChart().Chart.Name;
            if (Utils.IsNullOrEmpty(name))
            {
                name = "Chart" + Convert.ToString((int) (this.lbCharts.SelectedIndex + 1));
            }
            return name;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableButtons()
        {
            this.tabControl.Enabled = this.lbCharts.Items.Count > 0;
            this.udLeft.Enabled = this.tabControl.Enabled;
            this.udTop.Enabled = this.tabControl.Enabled;
            this.udHeight.Enabled = this.tabControl.Enabled;
            this.udWidth.Enabled = this.tabControl.Enabled;
            this.lLeft.Enabled = this.tabControl.Enabled;
            this.lTop.Enabled = this.tabControl.Enabled;
            this.lHeight.Enabled = this.tabControl.Enabled;
            this.lWidth.Enabled = this.tabControl.Enabled;
            this.bUp.Enabled = this.lbCharts.SelectedIndex > 0;
            this.bDown.Enabled = this.lbCharts.SelectedIndex < (this.lbCharts.Items.Count - 1);
            this.bDelete.Enabled = this.lbCharts.SelectedIndex != -1;
            this.bRename.Enabled = this.bDelete.Enabled;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SubChartEditor));
            this.panel1 = new System.Windows.Forms.Panel();
            this.bDown = new Button();
            this.imageList1 = new ImageList(this.components);
            this.bUp = new Button();
            this.lbCharts = new ListBox();
            this.bRename = new Button();
            this.bDelete = new Button();
            this.bAdd = new Button();
            this.tabControl = new TabControl();
            this.tabPosition = new TabPage();
            this.cbTransparent = new CheckBox();
            this.lHeight = new Label();
            this.lWidth = new Label();
            this.lTop = new Label();
            this.lLeft = new Label();
            this.udHeight = new NumericUpDown();
            this.udWidth = new NumericUpDown();
            this.udTop = new NumericUpDown();
            this.udLeft = new NumericUpDown();
            this.tabChart = new TabPage();
            this.panel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPosition.SuspendLayout();
            this.udHeight.BeginInit();
            this.udWidth.BeginInit();
            this.udTop.BeginInit();
            this.udLeft.BeginInit();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.bDown);
            this.panel1.Controls.Add(this.bUp);
            this.panel1.Controls.Add(this.lbCharts);
            this.panel1.Controls.Add(this.bRename);
            this.panel1.Controls.Add(this.bDelete);
            this.panel1.Controls.Add(this.bAdd);
            this.panel1.Dock = DockStyle.Left;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x56, 0xd3);
            this.panel1.TabIndex = 1;
            this.bDown.BackColor = Color.Silver;
            this.bDown.Enabled = false;
            this.bDown.FlatStyle = FlatStyle.Flat;
            this.bDown.ImageKey = "Down.bmp";
            this.bDown.ImageList = this.imageList1;
            this.bDown.Location = new Point(0x29, 0xb0);
            this.bDown.Name = "bDown";
            this.bDown.Size = new Size(0x17, 0x17);
            this.bDown.TabIndex = 5;
            this.bDown.UseVisualStyleBackColor = false;
            this.bDown.Click += new EventHandler(this.bDown_Click);
            this.imageList1.ImageStream = (ImageListStreamer) manager.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Up.bmp");
            this.imageList1.Images.SetKeyName(1, "Down.bmp");
            this.bUp.BackColor = Color.Silver;
            this.bUp.Enabled = false;
            this.bUp.FlatStyle = FlatStyle.Flat;
            this.bUp.ImageKey = "Up.bmp";
            this.bUp.ImageList = this.imageList1;
            this.bUp.Location = new Point(12, 0xb0);
            this.bUp.Name = "bUp";
            this.bUp.Size = new Size(0x17, 0x17);
            this.bUp.TabIndex = 4;
            this.bUp.UseVisualStyleBackColor = false;
            this.bUp.Click += new EventHandler(this.bUp_Click);
            this.lbCharts.FormattingEnabled = true;
            this.lbCharts.Location = new Point(5, 0x58);
            this.lbCharts.Name = "lbCharts";
            this.lbCharts.Size = new Size(0x4b, 0x52);
            this.lbCharts.TabIndex = 3;
            this.lbCharts.Click += new EventHandler(this.lbCharts_Click);
            this.bRename.Enabled = false;
            this.bRename.FlatStyle = FlatStyle.Flat;
            this.bRename.Location = new Point(5, 0x3d);
            this.bRename.Name = "bRename";
            this.bRename.Size = new Size(0x4b, 0x17);
            this.bRename.TabIndex = 2;
            this.bRename.Text = "&Rename...";
            this.bRename.UseVisualStyleBackColor = true;
            this.bRename.Click += new EventHandler(this.bRename_Click);
            this.bDelete.Enabled = false;
            this.bDelete.FlatStyle = FlatStyle.Flat;
            this.bDelete.Location = new Point(5, 0x20);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new Size(0x4b, 0x17);
            this.bDelete.TabIndex = 1;
            this.bDelete.Text = "&Delete...";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new EventHandler(this.bDelete_Click);
            this.bAdd.FlatStyle = FlatStyle.Flat;
            this.bAdd.Location = new Point(5, 3);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new Size(0x4b, 0x17);
            this.bAdd.TabIndex = 0;
            this.bAdd.Text = "&Add...";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new EventHandler(this.bAdd_Click);
            this.tabControl.Controls.Add(this.tabPosition);
            this.tabControl.Controls.Add(this.tabChart);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Enabled = false;
            this.tabControl.Location = new Point(0x56, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x99, 0xd3);
            this.tabControl.TabIndex = 2;
            this.tabPosition.Controls.Add(this.cbTransparent);
            this.tabPosition.Controls.Add(this.lHeight);
            this.tabPosition.Controls.Add(this.lWidth);
            this.tabPosition.Controls.Add(this.lTop);
            this.tabPosition.Controls.Add(this.lLeft);
            this.tabPosition.Controls.Add(this.udHeight);
            this.tabPosition.Controls.Add(this.udWidth);
            this.tabPosition.Controls.Add(this.udTop);
            this.tabPosition.Controls.Add(this.udLeft);
            this.tabPosition.Location = new Point(4, 0x16);
            this.tabPosition.Name = "tabPosition";
            this.tabPosition.Padding = new Padding(3);
            this.tabPosition.Size = new Size(0x91, 0xb9);
            this.tabPosition.TabIndex = 0;
            this.tabPosition.Text = "Position";
            this.tabPosition.UseVisualStyleBackColor = true;
            this.cbTransparent.AutoSize = true;
            this.cbTransparent.Location = new Point(0x23, 0x76);
            this.cbTransparent.Name = "cbTransparent";
            this.cbTransparent.Size = new Size(0x53, 0x11);
            this.cbTransparent.TabIndex = 8;
            this.cbTransparent.Text = "Tr&ansparent";
            this.cbTransparent.UseVisualStyleBackColor = true;
            this.cbTransparent.CheckedChanged += new EventHandler(this.cbTransparent_CheckedChanged);
            this.lHeight.AutoSize = true;
            this.lHeight.Location = new Point(3, 0x5d);
            this.lHeight.Name = "lHeight";
            this.lHeight.Size = new Size(0x29, 13);
            this.lHeight.TabIndex = 7;
            this.lHeight.Text = "&Height:";
            this.lWidth.AutoSize = true;
            this.lWidth.Location = new Point(6, 0x44);
            this.lWidth.Name = "lWidth";
            this.lWidth.Size = new Size(0x26, 13);
            this.lWidth.TabIndex = 6;
            this.lWidth.Text = "&Width:";
            this.lTop.AutoSize = true;
            this.lTop.Location = new Point(14, 0x27);
            this.lTop.Name = "lTop";
            this.lTop.Size = new Size(0x1d, 13);
            this.lTop.TabIndex = 5;
            this.lTop.Text = "&Top:";
            this.lLeft.AutoSize = true;
            this.lLeft.Location = new Point(15, 10);
            this.lLeft.Name = "lLeft";
            this.lLeft.Size = new Size(0x1c, 13);
            this.lLeft.TabIndex = 4;
            this.lLeft.Text = "&Left:";
            this.udHeight.Enabled = false;
            this.udHeight.Location = new Point(0x31, 0x5c);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.udHeight.Maximum = new decimal(bits);
            this.udHeight.Name = "udHeight";
            this.udHeight.Size = new Size(0x45, 20);
            this.udHeight.TabIndex = 3;
            this.udHeight.ValueChanged += new EventHandler(this.udHeight_ValueChanged);
            this.udWidth.Enabled = false;
            this.udWidth.Location = new Point(0x31, 0x42);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x3e8;
            this.udWidth.Maximum = new decimal(numArray2);
            this.udWidth.Name = "udWidth";
            this.udWidth.Size = new Size(0x45, 20);
            this.udWidth.TabIndex = 2;
            this.udWidth.ValueChanged += new EventHandler(this.udWidth_ValueChanged);
            this.udTop.Enabled = false;
            this.udTop.Location = new Point(0x31, 0x25);
            int[] numArray3 = new int[4];
            numArray3[0] = 0x3e8;
            this.udTop.Maximum = new decimal(numArray3);
            this.udTop.Name = "udTop";
            this.udTop.Size = new Size(0x45, 20);
            this.udTop.TabIndex = 1;
            this.udTop.ValueChanged += new EventHandler(this.udTop_ValueChanged);
            this.udLeft.Enabled = false;
            this.udLeft.Location = new Point(0x31, 8);
            int[] numArray4 = new int[4];
            numArray4[0] = 0x3e8;
            this.udLeft.Maximum = new decimal(numArray4);
            this.udLeft.Name = "udLeft";
            this.udLeft.Size = new Size(0x45, 20);
            this.udLeft.TabIndex = 0;
            this.udLeft.ValueChanged += new EventHandler(this.udLeft_ValueChanged);
            this.tabChart.Location = new Point(4, 0x16);
            this.tabChart.Name = "tabChart";
            this.tabChart.Padding = new Padding(3);
            this.tabChart.Size = new Size(0x91, 0xb9);
            this.tabChart.TabIndex = 1;
            this.tabChart.Text = "Chart";
            this.tabChart.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0xef, 0xd3);
            base.Controls.Add(this.tabControl);
            base.Controls.Add(this.panel1);
            base.Name = "SubChartEditor";
            this.Text = "SubChartEditor";
            this.panel1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPosition.ResumeLayout(false);
            this.tabPosition.PerformLayout();
            this.udHeight.EndInit();
            this.udWidth.EndInit();
            this.udTop.EndInit();
            this.udLeft.EndInit();
            base.ResumeLayout(false);
        }

        private void lbCharts_Click(object sender, EventArgs e)
        {
            this.EnableButtons();
            this.tabChart.Controls.Clear();
            this.chartEditor = null;
            if (this.lbCharts.SelectedIndex != -1)
            {
                this.chartEditor = new ChartEditor(this.CurrentChart().Chart.Chart);
                EditorUtils.InsertForm(this.chartEditor, this.tabChart);
                this.udLeft.Value = this.CurrentChart().Chart.Left;
                this.udTop.Value = this.CurrentChart().Chart.Top;
                this.udHeight.Value = this.CurrentChart().Chart.Height;
                this.udWidth.Value = this.CurrentChart().Chart.Width;
                this.cbTransparent.Checked = (this.CurrentChart().Chart.Panel.Color == Color.Transparent) || (this.CurrentChart().Chart.Panel.Color == Color.FromArgb(0, 0xff, 0xff, 0xff));
                if (base.Visible)
                {
                    this.tabControl_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
        }

        private void SwapChart(int a, int b)
        {
            SubChart chart = this.tool.Charts[a];
            SubChart chart2 = this.tool.Charts[b];
            this.tool.Charts[a] = chart2;
            this.tool.Charts[b] = chart;
            this.lbCharts.Items[a] = chart2.Chart.Name;
            this.lbCharts.Items[b] = chart.Chart.Name;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tabChart = this.tabChart;
            TabPage selectedTab = this.tabControl.SelectedTab;
        }

        private void udHeight_ValueChanged(object sender, EventArgs e)
        {
            if (base.Visible)
            {
                this.CurrentChart().Chart.Height = (int) this.udHeight.Value;
            }
        }

        private void udLeft_ValueChanged(object sender, EventArgs e)
        {
            if (base.Visible)
            {
                this.CurrentChart().Chart.Left = (int) this.udLeft.Value;
            }
        }

        private void udTop_ValueChanged(object sender, EventArgs e)
        {
            if (base.Visible)
            {
                this.CurrentChart().Chart.Top = (int) this.udTop.Value;
            }
        }

        private void udWidth_ValueChanged(object sender, EventArgs e)
        {
            if (base.Visible)
            {
                this.CurrentChart().Chart.Width = (int) this.udWidth.Value;
            }
        }
    }
}

