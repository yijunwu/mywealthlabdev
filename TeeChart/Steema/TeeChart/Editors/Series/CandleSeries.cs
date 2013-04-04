namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CandleSeries : BaseSeriesForm
    {
        private ButtonColor BDownColor;
        private ButtonPen BHighLow;
        private ButtonColor BUpColor;
        private ButtonPen button1;
        private Candle candle;
        private CheckBox CBDark3D;
        private CheckBox CBDraw3D;
        private CheckBox CBShowClose;
        private CheckBox CBShowOpen;
        private Container components;
        private Label label1;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private GroupBox RGStyle;
        private NumericUpDown UDWidth;

        public CandleSeries()
        {
            this.InitializeComponent();
        }

        public CandleSeries(Series s) : this()
        {
            this.candle = (Candle) s;
        }

        private void BDownColor_Click(object sender, EventArgs e)
        {
            this.candle.DownCloseColor = this.BDownColor.Color;
        }

        private void BUpColor_Click(object sender, EventArgs e)
        {
            this.candle.UpCloseColor = this.BUpColor.Color;
        }

        private void CBDark3D_CheckedChanged(object sender, EventArgs e)
        {
            this.candle.point.Dark3D = this.CBDark3D.Checked;
        }

        private void CBDraw3D_CheckedChanged(object sender, EventArgs e)
        {
            this.candle.Pointer.Draw3D = this.CBDraw3D.Checked;
        }

        private void CBShowClose_CheckedChanged(object sender, EventArgs e)
        {
            this.candle.ShowClose = this.CBShowClose.Checked;
        }

        private void CBShowOpen_CheckedChanged(object sender, EventArgs e)
        {
            this.candle.ShowOpen = this.CBShowOpen.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableDisableButtons()
        {
            this.CBShowOpen.Enabled = this.candle.Style == CandleStyles.CandleBar;
            this.CBShowClose.Enabled = this.candle.Style == CandleStyles.CandleBar;
            this.BHighLow.Enabled = this.candle.Style == CandleStyles.CandleStick;
            this.CBDraw3D.Enabled = (this.candle.Style == CandleStyles.CandleStick) || (this.candle.Style == CandleStyles.OpenClose);
            this.CBDark3D.Enabled = (this.candle.Style == CandleStyles.CandleStick) || (this.candle.Style == CandleStyles.OpenClose);
        }

        private void InitializeComponent()
        {
            this.BUpColor = new ButtonColor();
            this.BDownColor = new ButtonColor();
            this.button1 = new ButtonPen();
            this.RGStyle = new GroupBox();
            this.radioButton4 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.label1 = new Label();
            this.UDWidth = new NumericUpDown();
            this.CBShowOpen = new CheckBox();
            this.CBShowClose = new CheckBox();
            this.CBDraw3D = new CheckBox();
            this.CBDark3D = new CheckBox();
            this.numericUpDown1 = new NumericUpDown();
            this.label2 = new Label();
            this.BHighLow = new ButtonPen();
            this.RGStyle.SuspendLayout();
            this.UDWidth.BeginInit();
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            this.BUpColor.Color = Color.Empty;
            this.BUpColor.Location = new Point(0x80, 8);
            this.BUpColor.Name = "BUpColor";
            this.BUpColor.Size = new Size(0x83, 0x17);
            this.BUpColor.TabIndex = 0;
            this.BUpColor.Text = "&Up Close...";
            this.BUpColor.Click += new EventHandler(this.BUpColor_Click);
            this.BDownColor.Color = Color.Empty;
            this.BDownColor.Location = new Point(0x80, 40);
            this.BDownColor.Name = "BDownColor";
            this.BDownColor.Size = new Size(0x83, 0x17);
            this.BDownColor.TabIndex = 1;
            this.BDownColor.Text = "&Down Close...";
            this.BDownColor.Click += new EventHandler(this.BDownColor_Click);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(8, 0x7f);
            this.button1.Name = "button1";
            this.button1.TabIndex = 5;
            this.button1.Text = "Bo&rder...";
            this.RGStyle.Controls.Add(this.radioButton4);
            this.RGStyle.Controls.Add(this.radioButton3);
            this.RGStyle.Controls.Add(this.radioButton2);
            this.RGStyle.Controls.Add(this.radioButton1);
            this.RGStyle.Location = new Point(6, 4);
            this.RGStyle.Name = "RGStyle";
            this.RGStyle.Size = new Size(0x74, 0x5c);
            this.RGStyle.TabIndex = 3;
            this.RGStyle.TabStop = false;
            this.RGStyle.Text = "Style:";
            this.radioButton4.FlatStyle = FlatStyle.Flat;
            this.radioButton4.Location = new Point(8, 70);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(0x60, 0x10);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "&Line";
            this.radioButton4.CheckedChanged += new EventHandler(this.radioButton4_CheckedChanged);
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(8, 0x34);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x63, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "&Open Close";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(8, 0x22);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x60, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Bar";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x60, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&Stick";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(10, 0x68);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4b, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "Candle &Width:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.UDWidth.BorderStyle = BorderStyle.FixedSingle;
            this.UDWidth.Location = new Point(90, 0x66);
            this.UDWidth.Name = "UDWidth";
            this.UDWidth.Size = new Size(0x38, 20);
            this.UDWidth.TabIndex = 4;
            this.UDWidth.TextAlign = HorizontalAlignment.Right;
            this.UDWidth.TextChanged += new EventHandler(this.UDWidth_ValueChanged);
            this.UDWidth.ValueChanged += new EventHandler(this.UDWidth_ValueChanged);
            this.CBShowOpen.FlatStyle = FlatStyle.Flat;
            this.CBShowOpen.Location = new Point(160, 0x68);
            this.CBShowOpen.Name = "CBShowOpen";
            this.CBShowOpen.Size = new Size(0x6a, 0x10);
            this.CBShowOpen.TabIndex = 7;
            this.CBShowOpen.Text = "Show O&pen";
            this.CBShowOpen.CheckedChanged += new EventHandler(this.CBShowOpen_CheckedChanged);
            this.CBShowClose.FlatStyle = FlatStyle.Flat;
            this.CBShowClose.Location = new Point(160, 120);
            this.CBShowClose.Name = "CBShowClose";
            this.CBShowClose.Size = new Size(0x6a, 0x10);
            this.CBShowClose.TabIndex = 8;
            this.CBShowClose.Text = "Show &Close";
            this.CBShowClose.CheckedChanged += new EventHandler(this.CBShowClose_CheckedChanged);
            this.CBDraw3D.FlatStyle = FlatStyle.Flat;
            this.CBDraw3D.Location = new Point(160, 0x90);
            this.CBDraw3D.Name = "CBDraw3D";
            this.CBDraw3D.Size = new Size(0x60, 0x10);
            this.CBDraw3D.TabIndex = 9;
            this.CBDraw3D.Text = "Draw &3D";
            this.CBDraw3D.CheckedChanged += new EventHandler(this.CBDraw3D_CheckedChanged);
            this.CBDark3D.FlatStyle = FlatStyle.Flat;
            this.CBDark3D.Location = new Point(160, 160);
            this.CBDark3D.Name = "CBDark3D";
            this.CBDark3D.Size = new Size(0x60, 0x10);
            this.CBDark3D.TabIndex = 10;
            this.CBDark3D.Text = "Dar&k 3D";
            this.CBDark3D.CheckedChanged += new EventHandler(this.CBDark3D_CheckedChanged);
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0x58, 0x9b);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x38, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(8, 0x9d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4d, 0x10);
            this.label2.TabIndex = 10;
            this.label2.Text = "&Transparency:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.BHighLow.FlatStyle = FlatStyle.Flat;
            this.BHighLow.Location = new Point(0x80, 0x48);
            this.BHighLow.Name = "BHighLow";
            this.BHighLow.Size = new Size(0x83, 0x17);
            this.BHighLow.TabIndex = 2;
            this.BHighLow.Text = "&High Low...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(270, 0xb5);
            base.Controls.Add(this.BHighLow);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.CBDark3D);
            base.Controls.Add(this.CBDraw3D);
            base.Controls.Add(this.CBShowClose);
            base.Controls.Add(this.CBShowOpen);
            base.Controls.Add(this.UDWidth);
            base.Controls.Add(this.RGStyle);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.BDownColor);
            base.Controls.Add(this.BUpColor);
            base.Name = "CandleSeries";
            this.RGStyle.ResumeLayout(false);
            this.UDWidth.EndInit();
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.candle.Transparency = (int) this.numericUpDown1.Value;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.candle.Style = CandleStyles.CandleStick;
            this.EnableDisableButtons();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.candle.Style = CandleStyles.CandleBar;
            this.EnableDisableButtons();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.candle.Style = CandleStyles.OpenClose;
            this.EnableDisableButtons();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.candle.Style = CandleStyles.Line;
            this.EnableDisableButtons();
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.candle != null)
            {
                this.CBShowClose.Checked = this.candle.ShowClose;
                this.CBShowOpen.Checked = this.candle.ShowOpen;
                this.BHighLow.Pen = this.candle.HighLowPen;
                this.CBDark3D.Checked = this.candle.Dark3D;
                this.CBDraw3D.Checked = this.candle.Pointer.Draw3D;
                this.UDWidth.Value = this.candle.CandleWidth;
                this.numericUpDown1.Value = this.candle.Transparency;
                this.BDownColor.Color = this.candle.DownCloseColor;
                this.BUpColor.Color = this.candle.UpCloseColor;
                this.button1.Pen = this.candle.Pen;
                if (this.candle.Style == CandleStyles.CandleStick)
                {
                    this.radioButton1.Checked = true;
                }
                else if (this.candle.Style == CandleStyles.CandleBar)
                {
                    this.radioButton2.Checked = true;
                }
                else if (this.candle.Style == CandleStyles.OpenClose)
                {
                    this.radioButton3.Checked = true;
                }
                else
                {
                    this.radioButton4.Checked = true;
                }
            }
        }

        private void UDWidth_ValueChanged(object sender, EventArgs e)
        {
            this.candle.CandleWidth = (int) this.UDWidth.Value;
        }
    }
}

