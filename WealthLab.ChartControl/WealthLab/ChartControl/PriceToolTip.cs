namespace WealthLab.ChartControl
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class PriceToolTip : UserControl
    {
        private Bars bars;
        private IContainer icontainer_0;
        private int barNum = -1;
        private Label lblChange;
        private Label lblChangePct;
        private Label lblChangePctVal;
        private Label lblChangeVal;
        private Label lblClose;
        private Label lblCloseVal;
        private Label lblDateVal;
        private Label lblHigh;
        private Label lblHighVal;
        private Label lblLow;
        private Label lblLowVal;
        private Label lblOpen;
        private Label lblOpenVal;

        public PriceToolTip()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblOpen = new Label();
            this.lblHigh = new Label();
            this.lblLow = new Label();
            this.lblClose = new Label();
            this.lblChange = new Label();
            this.lblChangePct = new Label();
            this.lblDateVal = new Label();
            this.lblOpenVal = new Label();
            this.lblHighVal = new Label();
            this.lblLowVal = new Label();
            this.lblCloseVal = new Label();
            this.lblChangeVal = new Label();
            this.lblChangePctVal = new Label();
            base.SuspendLayout();
            this.lblOpen.AutoSize = true;
            this.lblOpen.Location = new Point(4, 0x11);
            this.lblOpen.Name = "lblOpen";
            this.lblOpen.Size = new Size(0x21, 13);
            this.lblOpen.TabIndex = 1;
            this.lblOpen.Text = "Open";
            this.lblOpen.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblHigh.AutoSize = true;
            this.lblHigh.Location = new Point(4, 30);
            this.lblHigh.Name = "lblHigh";
            this.lblHigh.Size = new Size(0x1d, 13);
            this.lblHigh.TabIndex = 2;
            this.lblHigh.Text = "High";
            this.lblHigh.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblLow.AutoSize = true;
            this.lblLow.Location = new Point(4, 0x2b);
            this.lblLow.Name = "lblLow";
            this.lblLow.Size = new Size(0x1b, 13);
            this.lblLow.TabIndex = 3;
            this.lblLow.Text = "Low";
            this.lblLow.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblClose.AutoSize = true;
            this.lblClose.Location = new Point(4, 0x38);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new Size(0x21, 13);
            this.lblClose.TabIndex = 4;
            this.lblClose.Text = "Close";
            this.lblClose.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblChange.AutoSize = true;
            this.lblChange.Location = new Point(4, 0x45);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new Size(0x2c, 13);
            this.lblChange.TabIndex = 5;
            this.lblChange.Text = "Change";
            this.lblChange.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblChangePct.AutoSize = true;
            this.lblChangePct.Location = new Point(4, 0x52);
            this.lblChangePct.Name = "lblChangePct";
            this.lblChangePct.Size = new Size(0x37, 13);
            this.lblChangePct.TabIndex = 6;
            this.lblChangePct.Text = "Change %";
            this.lblChangePct.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblDateVal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblDateVal.Location = new Point(3, 4);
            this.lblDateVal.Name = "lblDateVal";
            this.lblDateVal.Size = new Size(0x74, 13);
            this.lblDateVal.TabIndex = 7;
            this.lblDateVal.Text = "1/1/2001";
            this.lblDateVal.TextAlign = ContentAlignment.MiddleLeft;
            this.lblDateVal.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblOpenVal.Location = new Point(0x37, 0x10);
            this.lblOpenVal.Name = "lblOpenVal";
            this.lblOpenVal.Size = new Size(0x40, 14);
            this.lblOpenVal.TabIndex = 8;
            this.lblOpenVal.Text = "$12.34";
            this.lblOpenVal.TextAlign = ContentAlignment.MiddleRight;
            this.lblOpenVal.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblHighVal.Location = new Point(0x37, 30);
            this.lblHighVal.Name = "lblHighVal";
            this.lblHighVal.Size = new Size(0x40, 14);
            this.lblHighVal.TabIndex = 9;
            this.lblHighVal.Text = "$12.34";
            this.lblHighVal.TextAlign = ContentAlignment.MiddleRight;
            this.lblHighVal.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblLowVal.Location = new Point(0x37, 0x2a);
            this.lblLowVal.Name = "lblLowVal";
            this.lblLowVal.Size = new Size(0x40, 14);
            this.lblLowVal.TabIndex = 10;
            this.lblLowVal.Text = "$12.34";
            this.lblLowVal.TextAlign = ContentAlignment.MiddleRight;
            this.lblLowVal.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblCloseVal.Location = new Point(0x37, 0x37);
            this.lblCloseVal.Name = "lblCloseVal";
            this.lblCloseVal.Size = new Size(0x40, 14);
            this.lblCloseVal.TabIndex = 11;
            this.lblCloseVal.Text = "$12.34";
            this.lblCloseVal.TextAlign = ContentAlignment.MiddleRight;
            this.lblCloseVal.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblChangeVal.ForeColor = Color.Green;
            this.lblChangeVal.Location = new Point(0x36, 0x44);
            this.lblChangeVal.Name = "lblChangeVal";
            this.lblChangeVal.Size = new Size(0x40, 14);
            this.lblChangeVal.TabIndex = 12;
            this.lblChangeVal.Text = "$12.34";
            this.lblChangeVal.TextAlign = ContentAlignment.MiddleRight;
            this.lblChangeVal.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            this.lblChangePctVal.ForeColor = Color.Green;
            this.lblChangePctVal.Location = new Point(0x36, 0x51);
            this.lblChangePctVal.Name = "lblChangePctVal";
            this.lblChangePctVal.Size = new Size(0x40, 14);
            this.lblChangePctVal.TabIndex = 13;
            this.lblChangePctVal.Text = "$12.34";
            this.lblChangePctVal.TextAlign = ContentAlignment.MiddleRight;
            this.lblChangePctVal.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Info;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.lblChangePctVal);
            base.Controls.Add(this.lblChangeVal);
            base.Controls.Add(this.lblCloseVal);
            base.Controls.Add(this.lblLowVal);
            base.Controls.Add(this.lblHighVal);
            base.Controls.Add(this.lblOpenVal);
            base.Controls.Add(this.lblDateVal);
            base.Controls.Add(this.lblChangePct);
            base.Controls.Add(this.lblChange);
            base.Controls.Add(this.lblClose);
            base.Controls.Add(this.lblLow);
            base.Controls.Add(this.lblHigh);
            base.Controls.Add(this.lblOpen);
            this.ForeColor = SystemColors.InfoText;
            base.Name = "PriceToolTip";
            base.Size = new Size(0x7b, 0x63);
            base.MouseMove += new MouseEventHandler(this.PriceToolTip_MouseMove);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void PriceToolTip_MouseMove(object sender, MouseEventArgs e)
        {
            base.Visible = false;
        }

        public void RenderValues(Bars bars, int barNumber)
        {
            if (this.RepositionRequired(bars, barNumber))
            {
                this.bars = bars;
                this.barNum = barNumber;
                string str = bars.Date[barNumber].ToShortDateString();
                if (bars.IsIntraday)
                {
                    str = str + " " + bars.Date[barNumber].ToShortTimeString();
                }
                this.lblDateVal.Text = str;
                string format = "N" + bars.SymbolInfo.Decimals;
                this.lblOpenVal.Text = bars.Open[barNumber].ToString(format);
                this.lblHighVal.Text = bars.High[barNumber].ToString(format);
                this.lblLowVal.Text = bars.Low[barNumber].ToString(format);
                this.lblCloseVal.Text = bars.Close[barNumber].ToString(format);
                if (barNumber > 0)
                {
                    double num5 = bars.Close[barNumber] - bars.Close[barNumber - 1];
                    this.lblChangeVal.Text = num5.ToString(format);
                    num5 = (num5 * 100.0) / bars.Close[barNumber - 1];
                    this.lblChangePctVal.Text = num5.ToString("N2") + "%";
                    if (num5 > 0.0)
                    {
                        this.lblChangeVal.ForeColor = Color.Green;
                        this.lblChangePctVal.ForeColor = Color.Green;
                    }
                    else
                    {
                        this.lblChangeVal.ForeColor = Color.Red;
                        this.lblChangePctVal.ForeColor = Color.Red;
                    }
                }
                else
                {
                    this.lblChangeVal.Text = "";
                    this.lblChangePctVal.Text = "";
                }
            }
        }

        public bool RepositionRequired(Bars bars, int barNumber)
        {
            if (bars == this.bars)
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

