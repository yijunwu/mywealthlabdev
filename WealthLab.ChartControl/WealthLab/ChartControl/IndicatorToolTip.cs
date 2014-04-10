namespace WealthLab.ChartControl
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class IndicatorToolTip : UserControl
    {
        private IContainer components;
        private int barNum = -1;
        private Label lblDateVal;
        private Label lblValue;
        private PlottedIndicator plottedIndicator;

        public IndicatorToolTip()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void IndicatorToolTip_MouseMove(object sender, MouseEventArgs e)
        {
            base.Visible = false;
        }

        private void InitializeComponent()
        {
            this.lblValue = new Label();
            this.lblDateVal = new Label();
            base.SuspendLayout();
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new Point(4, 0x13);
            this.lblValue.Margin = new Padding(3, 0, 3, 2);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new Size(0x74, 13);
            this.lblValue.TabIndex = 0;
            this.lblValue.Text = "SMA(Close,20) = 12.34";
            this.lblValue.MouseMove += new MouseEventHandler(this.IndicatorToolTip_MouseMove);
            this.lblDateVal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblDateVal.Location = new Point(3, 3);
            this.lblDateVal.Name = "lblDateVal";
            this.lblDateVal.Size = new Size(0x74, 13);
            this.lblDateVal.TabIndex = 8;
            this.lblDateVal.Text = "1/1/2001";
            this.lblDateVal.TextAlign = ContentAlignment.MiddleLeft;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = SystemColors.Info;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.lblDateVal);
            base.Controls.Add(this.lblValue);
            this.ForeColor = SystemColors.InfoText;
            base.Name = "IndicatorToolTip";
            base.Size = new Size(0x7b, 0x22);
            base.MouseMove += new MouseEventHandler(this.IndicatorToolTip_MouseMove);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void RenderValue(PlottedIndicator plottedIndicator_1, int barNumber)
        {
            if (this.RepositionRequired(plottedIndicator_1, barNumber))
            {
                this.plottedIndicator = plottedIndicator_1;
                this.barNum = barNumber;
                DataSeries series = plottedIndicator_1.Series;
                string str = series.Date[barNumber].ToShortDateString();
                DateTime time2 = series.Date[barNumber];
                if (!time2.ToShortTimeString().Equals("12:00 AM"))
                {
                    str = str + " " + series.Date[barNumber].ToShortTimeString();
                }
                this.lblDateVal.Text = str;
                this.lblValue.Text = series.Description + " = " + series[barNumber].ToString("N" + DecimalsManager.Instance.Indicator);
                this.lblValue.ForeColor = plottedIndicator_1.Color;
            }
        }

        public bool RepositionRequired(PlottedIndicator plottedIndicator_1, int barNumber)
        {
            if (plottedIndicator_1 == this.plottedIndicator)
            {
                return (barNumber != this.barNum);
            }
            return true;
        }

        public void Reset()
        {
            this.barNum = -1;
        }
    }
}

