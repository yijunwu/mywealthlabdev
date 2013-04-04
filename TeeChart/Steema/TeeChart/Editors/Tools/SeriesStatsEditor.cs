namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SeriesStatsEditor : ToolSeriesEditor, IStopComboBoxTranslate
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private IContainer components;
        private RichTextBox richTextBox1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private SeriesStats tool;

        public SeriesStatsEditor()
        {
            this.InitializeComponent();
        }

        public SeriesStatsEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            base.setting = true;
            this.tool = (SeriesStats) t;
            base.SetTool(this.tool, null);
            if (this.tool != null)
            {
                this.checkBox6.Text = Texts.FunctionCorrelation;
                this.checkBox5.Text = Texts.FunctionMedian;
                this.checkBox4.Text = Texts.FunctionHigh;
                this.checkBox3.Text = Texts.FunctionLow;
                this.checkBox2.Text = Texts.FunctionTrend;
                this.checkBox1.Text = Texts.FunctionAverage;
            }
            base.setting = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckBox tag = (CheckBox) (sender as Button).Tag;
            SeriesEditor.ShowEditor((tag.Tag as Function).Series);
        }

        private void CBSeries_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            base.CBSeries_SelectedIndexChanged(sender, e);
            this.richTextBox1.Clear();
            if ((this.tool != null) && (this.tool.Series != null))
            {
                this.richTextBox1.Text = this.tool.Statistics;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = this.checkBox1.Checked;
            this.CheckFunction(this.checkBox1, typeof(Average));
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.button2.Enabled = this.checkBox2.Checked;
            this.CheckFunction(this.checkBox2, typeof(TrendFunction));
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.button3.Enabled = this.checkBox3.Checked;
            this.CheckFunction(this.checkBox3, typeof(Low));
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            this.button4.Enabled = this.checkBox4.Checked;
            this.CheckFunction(this.checkBox4, typeof(High));
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            this.button5.Enabled = this.checkBox5.Checked;
            this.CheckFunction(this.checkBox5, typeof(MedianFunction));
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            this.button6.Enabled = this.checkBox6.Checked;
            this.CheckFunction(this.checkBox6, typeof(CorrelationFunction));
        }

        private void CheckFunction(CheckBox cbox, System.Type ftype)
        {
            Function function = this.tool.StatFunction(ftype);
            if (function != null)
            {
                function.Series.Visible = cbox.Checked;
            }
            else if (cbox.Checked)
            {
                function = Activator.CreateInstance(ftype) as Function;
                Series s = Activator.CreateInstance(this.tool.Series.GetType()) as Series;
                function.series = s;
                function.Series.DataSource = this.tool.Series;
                function.Series.Function = function;
                this.tool.Chart.Series.Add(s);
                cbox.Tag = function;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableCheckBox(Button button, System.Type ftype)
        {
            CheckBox tag = (CheckBox) button.Tag;
            tag.Enabled = (this.tool != null) & (this.tool.Series != null);
            if (this.tool != null)
            {
                tag.Tag = this.tool.StatFunction(ftype);
                tag.Checked = ((tag.Tag != null) && ((tag.Tag as Function).Series != null)) && (tag.Tag as Function).Series.Active;
                button.Enabled = tag.Checked;
            }
        }

        private void EnableControls()
        {
            this.EnableCheckBox(this.button1, typeof(Average));
            this.EnableCheckBox(this.button2, typeof(TrendFunction));
            this.EnableCheckBox(this.button3, typeof(Low));
            this.EnableCheckBox(this.button4, typeof(High));
            this.EnableCheckBox(this.button5, typeof(MedianFunction));
            this.EnableCheckBox(this.button6, typeof(CorrelationFunction));
        }

        public ComboBox[] GetComboBoxes()
        {
            return new ComboBox[] { base.CBSeries };
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.button6 = new Button();
            this.checkBox6 = new CheckBox();
            this.button5 = new Button();
            this.checkBox5 = new CheckBox();
            this.button4 = new Button();
            this.checkBox4 = new CheckBox();
            this.button3 = new Button();
            this.checkBox3 = new CheckBox();
            this.button2 = new Button();
            this.checkBox2 = new CheckBox();
            this.button1 = new Button();
            this.checkBox1 = new CheckBox();
            this.tabPage2 = new TabPage();
            this.richTextBox1 = new RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            base.SuspendLayout();
            base.CBSeries.SelectedIndexChanged += new EventHandler(this.CBSeries_SelectedIndexChanged_1);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new Point(12, 0x2d);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0xce, 0xd4);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabPage1.Controls.Add(this.button6);
            this.tabPage1.Controls.Add(this.checkBox6);
            this.tabPage1.Controls.Add(this.button5);
            this.tabPage1.Controls.Add(this.checkBox5);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.checkBox4);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.checkBox3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.checkBox2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(0xc6, 0xba);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.button6.FlatStyle = FlatStyle.Flat;
            this.button6.Location = new Point(0x6b, 0x9f);
            this.button6.Name = "button6";
            this.button6.Size = new Size(0x4b, 0x17);
            this.button6.TabIndex = 11;
            this.button6.Text = "Edit";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new EventHandler(this.button1_Click);
            this.checkBox6.AutoSize = true;
            this.checkBox6.FlatStyle = FlatStyle.Flat;
            this.checkBox6.Location = new Point(6, 0xa2);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new Size(0x49, 0x11);
            this.checkBox6.TabIndex = 10;
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new EventHandler(this.checkBox6_CheckedChanged);
            this.button5.FlatStyle = FlatStyle.Flat;
            this.button5.Location = new Point(0x6b, 130);
            this.button5.Name = "button5";
            this.button5.Size = new Size(0x4b, 0x17);
            this.button5.TabIndex = 9;
            this.button5.Text = "Edit";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new EventHandler(this.button1_Click);
            this.checkBox5.AutoSize = true;
            this.checkBox5.FlatStyle = FlatStyle.Flat;
            this.checkBox5.Location = new Point(6, 0x85);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new Size(0x3a, 0x11);
            this.checkBox5.TabIndex = 8;
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new EventHandler(this.checkBox5_CheckedChanged);
            this.button4.FlatStyle = FlatStyle.Flat;
            this.button4.Location = new Point(0x6b, 0x65);
            this.button4.Name = "button4";
            this.button4.Size = new Size(0x4b, 0x17);
            this.button4.TabIndex = 7;
            this.button4.Text = "Edit";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new EventHandler(this.button1_Click);
            this.checkBox4.AutoSize = true;
            this.checkBox4.FlatStyle = FlatStyle.Flat;
            this.checkBox4.Location = new Point(6, 0x68);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new Size(0x2d, 0x11);
            this.checkBox4.TabIndex = 6;
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new EventHandler(this.checkBox4_CheckedChanged);
            this.button3.FlatStyle = FlatStyle.Flat;
            this.button3.Location = new Point(0x6b, 0x48);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x4b, 0x17);
            this.button3.TabIndex = 5;
            this.button3.Text = "Edit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button1_Click);
            this.checkBox3.AutoSize = true;
            this.checkBox3.FlatStyle = FlatStyle.Flat;
            this.checkBox3.Location = new Point(6, 0x4b);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new Size(0x2b, 0x11);
            this.checkBox3.TabIndex = 4;
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x6b, 0x2b);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 3;
            this.button2.Text = "Edit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button1_Click);
            this.checkBox2.AutoSize = true;
            this.checkBox2.FlatStyle = FlatStyle.Flat;
            this.checkBox2.Location = new Point(6, 0x2e);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(0x33, 0x11);
            this.checkBox2.TabIndex = 2;
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x6b, 14);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 1;
            this.button1.Text = "Edit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.checkBox1.AutoSize = true;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(6, 0x11);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x3f, 0x11);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.tabPage2.Controls.Add(this.richTextBox1);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(0xc6, 0xba);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Info";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.richTextBox1.BackColor = SystemColors.HighlightText;
            this.richTextBox1.Dock = DockStyle.Fill;
            this.richTextBox1.Location = new Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new Size(0xc0, 180);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            base.ClientSize = new Size(0xf5, 0x107);
            base.Controls.Add(this.tabControl1);
            base.Name = "SeriesStatsEditor";
            base.Load += new EventHandler(this.SeriesStatsEditor_Load);
            base.Controls.SetChildIndex(this.tabControl1, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void SeriesStatsEditor_Load(object sender, EventArgs e)
        {
            this.button1.Tag = this.checkBox1;
            this.button2.Tag = this.checkBox2;
            this.button3.Tag = this.checkBox3;
            this.button4.Tag = this.checkBox4;
            this.button5.Tag = this.checkBox5;
            this.button6.Tag = this.checkBox6;
            this.EnableControls();
            this.richTextBox1.Clear();
            if (this.tool.Series != null)
            {
                this.richTextBox1.Text = this.tool.Statistics;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabPage2)
            {
                this.richTextBox1.Clear();
                if (this.tool.Series != null)
                {
                    this.richTextBox1.Text = this.tool.Statistics;
                }
            }
        }
    }
}

