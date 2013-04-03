namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class TradableTrendlineSettings : TrendlineSettings
    {
        private ComboBox cmbTradeType;
        private GroupBox grpTrading;
        private IContainer icontainer_2;
        private Label lblCross;
        private Label lblTrade;
        private Label lblTrendline;
        private RadioButton rbAbove;
        private RadioButton rbBelow;

        public TradableTrendlineSettings()
        {
            this.InitializeComponent_1();
            this.grpTrading.SuspendLayout();
            base.SuspendLayout();
            int x = (base.grpOptions.Location.X + base.grpOptions.Size.Width) + 7;
            int y = this.grpTrading.Location.Y;
            int width = this.grpTrading.Size.Width;
            int height = (base.grpOptions.Size.Height + base.grpOptions.Location.Y) - this.grpTrading.Location.Y;
            this.grpTrading.Location = new Point(x, y);
            this.grpTrading.Size = new Size(width, height);
            width = (this.grpTrading.Location.X + this.grpTrading.Size.Width) + 7;
            height = (base.grpOptions.Location.Y + base.grpOptions.Size.Height) + 7;
            base.Size = new Size(width, height);
            this.grpTrading.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_2 != null))
            {
                this.icontainer_2.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent_1()
        {
            this.grpTrading = new GroupBox();
            this.lblTrade = new Label();
            this.cmbTradeType = new ComboBox();
            this.lblCross = new Label();
            this.rbAbove = new RadioButton();
            this.rbBelow = new RadioButton();
            this.lblTrendline = new Label();
            this.grpTrading.SuspendLayout();
            base.SuspendLayout();
            this.grpTrading.Controls.Add(this.lblTrendline);
            this.grpTrading.Controls.Add(this.rbBelow);
            this.grpTrading.Controls.Add(this.rbAbove);
            this.grpTrading.Controls.Add(this.lblCross);
            this.grpTrading.Controls.Add(this.cmbTradeType);
            this.grpTrading.Controls.Add(this.lblTrade);
            this.grpTrading.Location = new Point(0xd4, 4);
            this.grpTrading.Name = "grpTrading";
            this.grpTrading.Size = new Size(0xb5, 290);
            this.grpTrading.TabIndex = 3;
            this.grpTrading.TabStop = false;
            this.grpTrading.Text = "Trades";
            this.lblTrade.Location = new Point(7, 20);
            this.lblTrade.Name = "lblTrade";
            this.lblTrade.Size = new Size(0x9e, 30);
            this.lblTrade.TabIndex = 0;
            this.lblTrade.Text = "Trigger a Trade Alert when the following takes place:";
            this.cmbTradeType.FormattingEnabled = true;
            this.cmbTradeType.Items.AddRange(new object[] { "Buy", "Sell", "Short", "Cover" });
            this.cmbTradeType.Location = new Point(10, 0x3e);
            this.cmbTradeType.Name = "cmbTradeType";
            this.cmbTradeType.Size = new Size(0x79, 0x15);
            this.cmbTradeType.TabIndex = 1;
            this.lblCross.AutoSize = true;
            this.lblCross.Location = new Point(10, 0x5b);
            this.lblCross.Name = "lblCross";
            this.lblCross.Size = new Size(0x6f, 13);
            this.lblCross.TabIndex = 2;
            this.lblCross.Text = "when Price crosses ...";
            this.rbAbove.AutoSize = true;
            this.rbAbove.Checked = true;
            this.rbAbove.Location = new Point(13, 0x6f);
            this.rbAbove.Name = "rbAbove";
            this.rbAbove.Size = new Size(0x38, 0x11);
            this.rbAbove.TabIndex = 3;
            this.rbAbove.TabStop = true;
            this.rbAbove.Text = "Above";
            this.rbAbove.UseVisualStyleBackColor = true;
            this.rbBelow.AutoSize = true;
            this.rbBelow.Location = new Point(13, 130);
            this.rbBelow.Name = "rbBelow";
            this.rbBelow.Size = new Size(0x36, 0x11);
            this.rbBelow.TabIndex = 4;
            this.rbBelow.Text = "Below";
            this.rbBelow.UseVisualStyleBackColor = true;
            this.lblTrendline.AutoSize = true;
            this.lblTrendline.Location = new Point(13, 150);
            this.lblTrendline.Name = "lblTrendline";
            this.lblTrendline.Size = new Size(0x6c, 13);
            this.lblTrendline.TabIndex = 5;
            this.lblTrendline.Text = "... Tradable Trendline";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.grpTrading);
            base.Name = "TradableTrendlineSettings";
            base.Size = new Size(0x193, 0x12e);
            base.Controls.SetChildIndex(this.grpTrading, 0);
            this.grpTrading.ResumeLayout(false);
            this.grpTrading.PerformLayout();
            base.ResumeLayout(false);
        }

        public bool AbovePrice
        {
            get
            {
                return this.rbAbove.Checked;
            }
            set
            {
                this.rbAbove.Checked = value;
                this.rbBelow.Checked = !value;
            }
        }

        public TradeType AlertType
        {
            get
            {
                return (TradeType) this.cmbTradeType.SelectedIndex;
            }
            set
            {
                this.cmbTradeType.SelectedIndex = (int) value;
            }
        }
    }
}

