namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PolarSeries : BaseSeriesForm
    {
        private Button BBrush;
        private Button BFont;
        protected Button BPen;
        private Button BPiePen;
        private ButtonColor button1;
        private CheckBox CBAngleLabels;
        protected CheckBox CBClockWise;
        protected CheckBox CBClose;
        protected CheckBox CBColorEach;
        private CheckBox CBInside;
        private CheckBox CBLabelsRot;
        private CircledSeries circledEditor;
        private Container components;
        protected Label label1;
        protected Label label2;
        private Label label3;
        private Label label4;
        private Steema.TeeChart.Editors.SeriesPointer pointerEditor;
        internal CustomPolar polar;
        protected TabControl tabControl1;
        protected TabPage tabPage1;
        protected TabPage tabPage2;
        protected NumericUpDown UDAngleInc;
        private NumericUpDown udMargin;
        protected NumericUpDown UDRadiusInc;
        private NumericUpDown UDTransp;

        public PolarSeries()
        {
            this.InitializeComponent();
        }

        public PolarSeries(Series s) : this()
        {
            if (s is WindRose)
            {
                this.UDAngleInc.Visible = false;
                this.label1.Visible = false;
            }
            this.SetPolar((CustomPolar) s);
        }

        private void BBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.polar.Brush);
        }

        private void BFont_Click(object sender, EventArgs e)
        {
            EditorUtils.EditFont(this.polar.CircleLabelsFont);
        }

        private void BPen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.polar.Pen);
        }

        private void BPiePen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.polar.CirclePen);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.polar.Color = this.button1.Color;
        }

        private void CBAngleLabels_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.CircleLabels = this.CBAngleLabels.Checked;
            this.EnableLabels();
        }

        private void CBClockWise_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.ClockWiseLabels = this.CBClockWise.Checked;
        }

        private void CBClose_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.CloseCircle = this.CBClose.Checked;
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.ColorEach = this.CBColorEach.Checked;
        }

        private void CBInside_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.CircleLabelsInside = this.CBInside.Checked;
        }

        private void CBLabelsRot_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.CircleLabelsRotated = this.CBLabelsRot.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableLabels()
        {
            this.CBLabelsRot.Enabled = this.CBAngleLabels.Enabled;
            this.CBClockWise.Enabled = this.CBAngleLabels.Enabled;
            this.BFont.Enabled = this.CBAngleLabels.Enabled;
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.BPen = new Button();
            this.BBrush = new Button();
            this.BPiePen = new Button();
            this.CBClose = new CheckBox();
            this.UDAngleInc = new NumericUpDown();
            this.UDRadiusInc = new NumericUpDown();
            this.UDTransp = new NumericUpDown();
            this.BFont = new Button();
            this.CBClockWise = new CheckBox();
            this.CBLabelsRot = new CheckBox();
            this.CBInside = new CheckBox();
            this.CBAngleLabels = new CheckBox();
            this.button1 = new ButtonColor();
            this.CBColorEach = new CheckBox();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.tabPage2 = new TabPage();
            this.label4 = new Label();
            this.udMargin = new NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.UDAngleInc.BeginInit();
            this.UDRadiusInc.BeginInit();
            this.UDTransp.BeginInit();
            this.udMargin.BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 0x38);
            this.label1.Name = "label1";
            this.label1.Size = new Size(90, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "Angle &Increment:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(8, 0x58);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x60, 0x10);
            this.label2.TabIndex = 6;
            this.label2.Text = "&Radius Increment:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x18, 120);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4d, 0x10);
            this.label3.TabIndex = 8;
            this.label3.Text = "Transparenc&y:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.BPen.FlatStyle = FlatStyle.Flat;
            this.BPen.Location = new Point(0x10, 0x10);
            this.BPen.Name = "BPen";
            this.BPen.TabIndex = 0;
            this.BPen.Text = "&Pen...";
            this.BPen.Click += new EventHandler(this.BPen_Click);
            this.BBrush.FlatStyle = FlatStyle.Flat;
            this.BBrush.Location = new Point(0x70, 0x10);
            this.BBrush.Name = "BBrush";
            this.BBrush.TabIndex = 2;
            this.BBrush.Text = "Pa&ttern...";
            this.BBrush.Click += new EventHandler(this.BBrush_Click);
            this.BPiePen.FlatStyle = FlatStyle.Flat;
            this.BPiePen.Location = new Point(240, 0x58);
            this.BPiePen.Name = "BPiePen";
            this.BPiePen.TabIndex = 3;
            this.BPiePen.Text = "&Circle...";
            this.BPiePen.Click += new EventHandler(this.BPiePen_Click);
            this.CBClose.Checked = true;
            this.CBClose.CheckState = CheckState.Checked;
            this.CBClose.FlatStyle = FlatStyle.Flat;
            this.CBClose.Location = new Point(240, 120);
            this.CBClose.Name = "CBClose";
            this.CBClose.Size = new Size(0x88, 0x11);
            this.CBClose.TabIndex = 1;
            this.CBClose.Text = "C&lose Circle";
            this.CBClose.CheckedChanged += new EventHandler(this.CBClose_CheckedChanged);
            this.UDAngleInc.BorderStyle = BorderStyle.FixedSingle;
            this.UDAngleInc.Location = new Point(0x70, 0x38);
            int[] bits = new int[4];
            bits[0] = 360;
            this.UDAngleInc.Maximum = new decimal(bits);
            this.UDAngleInc.Name = "UDAngleInc";
            this.UDAngleInc.Size = new Size(0x4e, 20);
            this.UDAngleInc.TabIndex = 5;
            this.UDAngleInc.TextAlign = HorizontalAlignment.Right;
            this.UDAngleInc.TextChanged += new EventHandler(this.UDAngleInc_ValueChanged);
            this.UDAngleInc.ValueChanged += new EventHandler(this.UDAngleInc_ValueChanged);
            this.UDRadiusInc.BorderStyle = BorderStyle.FixedSingle;
            this.UDRadiusInc.Location = new Point(0x70, 0x58);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x7fff;
            this.UDRadiusInc.Maximum = new decimal(numArray2);
            this.UDRadiusInc.Name = "UDRadiusInc";
            this.UDRadiusInc.Size = new Size(0x4e, 20);
            this.UDRadiusInc.TabIndex = 7;
            this.UDRadiusInc.TextAlign = HorizontalAlignment.Right;
            this.UDRadiusInc.TextChanged += new EventHandler(this.UDRadiusInc_ValueChanged);
            this.UDRadiusInc.ValueChanged += new EventHandler(this.UDRadiusInc_ValueChanged);
            this.UDTransp.BorderStyle = BorderStyle.FixedSingle;
            this.UDTransp.Location = new Point(0x70, 120);
            this.UDTransp.Name = "UDTransp";
            this.UDTransp.Size = new Size(60, 20);
            this.UDTransp.TabIndex = 9;
            this.UDTransp.TextAlign = HorizontalAlignment.Right;
            this.UDTransp.TextChanged += new EventHandler(this.UDTransp_TextChanged);
            this.UDTransp.ValueChanged += new EventHandler(this.UDTransp_ValueChanged);
            this.BFont.FlatStyle = FlatStyle.Flat;
            this.BFont.Location = new Point(0x88, 80);
            this.BFont.Name = "BFont";
            this.BFont.TabIndex = 4;
            this.BFont.Text = "&Font...";
            this.BFont.Click += new EventHandler(this.BFont_Click);
            this.CBClockWise.FlatStyle = FlatStyle.Flat;
            this.CBClockWise.Location = new Point(0x88, 0x10);
            this.CBClockWise.Name = "CBClockWise";
            this.CBClockWise.Size = new Size(0x90, 0x11);
            this.CBClockWise.TabIndex = 3;
            this.CBClockWise.Text = "Clock&Wise";
            this.CBClockWise.CheckedChanged += new EventHandler(this.CBClockWise_CheckedChanged);
            this.CBLabelsRot.FlatStyle = FlatStyle.Flat;
            this.CBLabelsRot.Location = new Point(8, 40);
            this.CBLabelsRot.Name = "CBLabelsRot";
            this.CBLabelsRot.Size = new Size(120, 0x11);
            this.CBLabelsRot.TabIndex = 2;
            this.CBLabelsRot.Text = "R&otated";
            this.CBLabelsRot.CheckedChanged += new EventHandler(this.CBLabelsRot_CheckedChanged);
            this.CBInside.FlatStyle = FlatStyle.Flat;
            this.CBInside.Location = new Point(0x88, 40);
            this.CBInside.Name = "CBInside";
            this.CBInside.Size = new Size(0x90, 0x10);
            this.CBInside.TabIndex = 1;
            this.CBInside.Text = "I&nside";
            this.CBInside.CheckedChanged += new EventHandler(this.CBInside_CheckedChanged);
            this.CBAngleLabels.FlatStyle = FlatStyle.Flat;
            this.CBAngleLabels.Location = new Point(8, 0x10);
            this.CBAngleLabels.Name = "CBAngleLabels";
            this.CBAngleLabels.Size = new Size(0x70, 0x10);
            this.CBAngleLabels.TabIndex = 0;
            this.CBAngleLabels.Text = "&Visible";
            this.CBAngleLabels.CheckedChanged += new EventHandler(this.CBAngleLabels_CheckedChanged);
            this.button1.Color = Color.Empty;
            this.button1.Location = new Point(240, 0x10);
            this.button1.Name = "button1";
            this.button1.TabIndex = 10;
            this.button1.Text = "&Color...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(240, 0x30);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x88, 0x18);
            this.CBColorEach.TabIndex = 12;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x188, 0xd6);
            this.tabControl1.TabIndex = 13;
            this.tabPage1.Controls.Add(this.BPen);
            this.tabPage1.Controls.Add(this.BBrush);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.CBColorEach);
            this.tabPage1.Controls.Add(this.BPiePen);
            this.tabPage1.Controls.Add(this.CBClose);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.UDAngleInc);
            this.tabPage1.Controls.Add(this.UDRadiusInc);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.UDTransp);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x180, 0xbc);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.udMargin);
            this.tabPage2.Controls.Add(this.CBAngleLabels);
            this.tabPage2.Controls.Add(this.CBLabelsRot);
            this.tabPage2.Controls.Add(this.CBClockWise);
            this.tabPage2.Controls.Add(this.CBInside);
            this.tabPage2.Controls.Add(this.BFont);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x180, 0xbc);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Labels";
            this.label4.Location = new Point(8, 0x40);
            this.label4.Name = "label4";
            this.label4.Size = new Size(100, 0x10);
            this.label4.TabIndex = 6;
            this.label4.Text = "Margin %:";
            this.udMargin.Location = new Point(8, 80);
            this.udMargin.Name = "udMargin";
            this.udMargin.Size = new Size(0x38, 20);
            this.udMargin.TabIndex = 5;
            this.udMargin.Click += new EventHandler(this.udMargin_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x188, 0xd6);
            base.Controls.Add(this.tabControl1);
            base.Name = "PolarSeries";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.UDAngleInc.EndInit();
            this.UDRadiusInc.EndInit();
            this.UDTransp.EndInit();
            this.udMargin.EndInit();
            base.ResumeLayout(false);
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.polar != null)
            {
                if (this.pointerEditor == null)
                {
                    this.pointerEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(Parent, this.polar.Pointer);
                }
                if (this.circledEditor == null)
                {
                    this.circledEditor = CircledSeries.InsertForm(Parent, this.polar);
                }
            }
        }

        protected void SetPolar(CustomPolar s)
        {
            this.polar = s;
            this.CBAngleLabels.Checked = this.polar.CircleLabels;
            this.CBClockWise.Checked = this.polar.ClockWiseLabels;
            this.CBClose.Checked = this.polar.CloseCircle;
            this.CBColorEach.Checked = this.polar.ColorEach;
            this.CBInside.Checked = this.polar.CircleLabelsInside;
            this.CBLabelsRot.Checked = this.polar.CircleLabelsRotated;
            this.UDTransp.Value = this.polar.Transparency;
            this.button1.Color = this.polar.Color;
            this.udMargin.Value = this.polar.LabelsMargin;
        }

        private void UDAngleInc_ValueChanged(object sender, EventArgs e)
        {
            this.polar.AngleIncrement = (int) this.UDAngleInc.Value;
        }

        private void udMargin_Click(object sender, EventArgs e)
        {
            this.polar.LabelsMargin = (int) this.udMargin.Value;
        }

        private void UDRadiusInc_ValueChanged(object sender, EventArgs e)
        {
            this.polar.RadiusIncrement = (int) this.UDRadiusInc.Value;
        }

        private void UDTransp_TextChanged(object sender, EventArgs e)
        {
            this.UDTransp_ValueChanged(sender, e);
        }

        private void UDTransp_ValueChanged(object sender, EventArgs e)
        {
            this.polar.Transparency = (int) this.UDTransp.Value;
        }
    }
}

