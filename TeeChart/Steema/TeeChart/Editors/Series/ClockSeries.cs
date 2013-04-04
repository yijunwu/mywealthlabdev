namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ClockSeries : PolarSeries
    {
        private Button bHour;
        private Button bMinute;
        private Button bRadius;
        private Button bSecond;
        private CheckBox cbRoman;
        private IContainer components;
        private Clock series;

        public ClockSeries()
        {
            this.InitializeComponent();
            base.tabPage2.Controls.Add(this.cbRoman);
            base.tabPage1.Controls.Add(this.bRadius);
            base.tabPage1.Controls.Add(this.bHour);
            base.tabPage1.Controls.Add(this.bSecond);
            base.tabPage1.Controls.Add(this.bMinute);
        }

        public ClockSeries(Series s) : this()
        {
            this.series = (Clock) s;
            base.SetPolar(this.series);
            base.tabControl1.SelectedIndex = 1;
            this.cbRoman.Checked = this.series.Style == ClockStyles.Roman;
            base.BPen.Visible = false;
            base.label1.Visible = false;
            base.label2.Visible = false;
            base.UDAngleInc.Visible = false;
            base.UDRadiusInc.Visible = false;
            base.CBColorEach.Visible = false;
            base.CBClose.Visible = false;
            base.CBClockWise.Visible = false;
        }

        private void bHour_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.PenHours);
        }

        private void bMinute_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.PenMinutes);
        }

        private void bRadius_Click(object sender, EventArgs e)
        {
            if (this.series.GetHorizAxis != null)
            {
                PenEditor.Edit(this.series.GetHorizAxis.Grid);
            }
        }

        private void bSecond_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.PenSeconds);
        }

        private void cbRoman_Click(object sender, EventArgs e)
        {
            if (this.cbRoman.Checked)
            {
                this.series.Style = ClockStyles.Roman;
            }
            else
            {
                this.series.Style = ClockStyles.Decimal;
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

        private void InitializeComponent()
        {
            this.cbRoman = new CheckBox();
            this.bRadius = new Button();
            this.bHour = new Button();
            this.bMinute = new Button();
            this.bSecond = new Button();
            base.SuspendLayout();
            base.tabControl1.Name = "tabControl1";
            base.tabControl1.Size = new Size(0x188, 0xc5);
            this.cbRoman.Location = new Point(0x100, 0x10);
            this.cbRoman.Name = "cbRoman";
            this.cbRoman.Size = new Size(0x70, 0x18);
            this.cbRoman.TabIndex = 7;
            this.cbRoman.Text = "Roman";
            this.cbRoman.Click += new EventHandler(this.cbRoman_Click);
            this.bRadius.FlatStyle = FlatStyle.Flat;
            this.bRadius.Location = new Point(0x10, 0x38);
            this.bRadius.Name = "bRadius";
            this.bRadius.TabIndex = 13;
            this.bRadius.Text = "R&adius...";
            this.bRadius.Click += new EventHandler(this.bRadius_Click);
            this.bHour.FlatStyle = FlatStyle.Flat;
            this.bHour.Location = new Point(0x10, 0x58);
            this.bHour.Name = "bHour";
            this.bHour.TabIndex = 15;
            this.bHour.Text = "&Hours...";
            this.bHour.Click += new EventHandler(this.bHour_Click);
            this.bMinute.FlatStyle = FlatStyle.Flat;
            this.bMinute.Location = new Point(0x70, 0x58);
            this.bMinute.Name = "bMinute";
            this.bMinute.TabIndex = 0x10;
            this.bMinute.Text = "&Minutes...";
            this.bMinute.Click += new EventHandler(this.bMinute_Click);
            this.bSecond.FlatStyle = FlatStyle.Flat;
            this.bSecond.Location = new Point(240, 120);
            this.bSecond.Name = "bSecond";
            this.bSecond.TabIndex = 0x11;
            this.bSecond.Text = "&Seconds...";
            this.bSecond.Click += new EventHandler(this.bSecond_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x188, 0xc5);
            base.Name = "ClockSeries";
            base.ResumeLayout(false);
        }
    }
}

