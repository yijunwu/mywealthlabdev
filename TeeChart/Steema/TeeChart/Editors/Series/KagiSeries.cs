namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class KagiSeries : BaseSeriesForm
    {
        private ButtonPen buttonPenDS;
        private ButtonPen buttonPenUS;
        private Steema.TeeChart.Editors.SeriesPointer buyEditor;
        private CheckBox cbAbsolute;
        private CheckBox cbBuy;
        private CheckBox cbSell;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private GroupBox RGSymbols;
        private Steema.TeeChart.Editors.SeriesPointer sellEditor;
        private Kagi series;
        private TextBox textBox1;

        public KagiSeries()
        {
            this.InitializeComponent();
        }

        public KagiSeries(Series s) : this()
        {
            this.series = (Kagi) s;
        }

        private void cbAbsolute_CheckedChanged(object sender, EventArgs e)
        {
            this.series.AbsoluteReversal = this.cbAbsolute.Checked;
        }

        private void cbBuy_CheckedChanged(object sender, EventArgs e)
        {
            this.series.BuySymbol.Visible = this.cbBuy.Checked;
        }

        private void cbSell_CheckedChanged(object sender, EventArgs e)
        {
            this.series.SellSymbol.Visible = this.cbSell.Checked;
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
            this.buttonPenUS = new ButtonPen();
            this.buttonPenDS = new ButtonPen();
            this.RGSymbols = new GroupBox();
            this.cbBuy = new CheckBox();
            this.cbSell = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.label1 = new Label();
            this.textBox1 = new TextBox();
            this.cbAbsolute = new CheckBox();
            this.RGSymbols.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.buttonPenUS.FlatStyle = FlatStyle.Flat;
            this.buttonPenUS.Location = new Point(12, 12);
            this.buttonPenUS.Name = "buttonPenUS";
            this.buttonPenUS.Size = new Size(0x4b, 0x17);
            this.buttonPenUS.TabIndex = 0;
            this.buttonPenUS.Text = "Upswing";
            this.buttonPenDS.FlatStyle = FlatStyle.Flat;
            this.buttonPenDS.Location = new Point(12, 0x29);
            this.buttonPenDS.Name = "buttonPenDS";
            this.buttonPenDS.Size = new Size(0x4b, 0x17);
            this.buttonPenDS.TabIndex = 1;
            this.buttonPenDS.Text = "Downswing";
            this.RGSymbols.Controls.Add(this.cbBuy);
            this.RGSymbols.Controls.Add(this.cbSell);
            this.RGSymbols.Location = new Point(0x5d, 0x59);
            this.RGSymbols.Name = "RGSymbols";
            this.RGSymbols.Size = new Size(0x8a, 0x47);
            this.RGSymbols.TabIndex = 4;
            this.RGSymbols.TabStop = false;
            this.RGSymbols.Text = "Symbols:";
            this.cbBuy.AutoSize = true;
            this.cbBuy.FlatStyle = FlatStyle.Flat;
            this.cbBuy.Location = new Point(6, 0x2a);
            this.cbBuy.Name = "cbBuy";
            this.cbBuy.Size = new Size(0x69, 0x11);
            this.cbBuy.TabIndex = 1;
            this.cbBuy.Text = "Show buy symbol";
            this.cbBuy.CheckedChanged += new EventHandler(this.cbBuy_CheckedChanged);
            this.cbSell.AutoSize = true;
            this.cbSell.FlatStyle = FlatStyle.Flat;
            this.cbSell.Location = new Point(6, 0x13);
            this.cbSell.Name = "cbSell";
            this.cbSell.Size = new Size(0x67, 0x11);
            this.cbSell.TabIndex = 0;
            this.cbSell.Text = "Show sell symbol";
            this.cbSell.CheckedChanged += new EventHandler(this.cbSell_CheckedChanged);
            this.groupBox1.Controls.Add(this.cbAbsolute);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(0x5d, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x8a, 0x4d);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reversal:";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 0x1d);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2e, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Amount:";
            this.textBox1.Location = new Point(0x3a, 0x1a);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(70, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.cbAbsolute.AutoSize = true;
            this.cbAbsolute.FlatStyle = FlatStyle.Flat;
            this.cbAbsolute.Location = new Point(0x3a, 0x34);
            this.cbAbsolute.Name = "cbAbsolute";
            this.cbAbsolute.Size = new Size(0x40, 0x11);
            this.cbAbsolute.TabIndex = 2;
            this.cbAbsolute.Text = "Absolute";
            this.cbAbsolute.CheckedChanged += new EventHandler(this.cbAbsolute_CheckedChanged);
            base.ClientSize = new Size(0xf6, 0xab);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.RGSymbols);
            base.Controls.Add(this.buttonPenDS);
            base.Controls.Add(this.buttonPenUS);
            base.Name = "KagiSeries";
            this.RGSymbols.ResumeLayout(false);
            this.RGSymbols.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                this.buttonPenUS.Pen = this.series.UpSwing;
                this.buttonPenDS.Pen = this.series.DownSwing;
                this.cbBuy.Checked = this.series.BuySymbol.Visible;
                this.cbSell.Checked = this.series.SellSymbol.Visible;
                this.cbAbsolute.Checked = this.series.AbsoluteReversal;
                this.textBox1.Text = this.series.ReversalAmount.ToString();
                if (this.sellEditor == null)
                {
                    this.sellEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(Parent, this.series.SellSymbol, "Sell");
                }
                if (this.buyEditor == null)
                {
                    this.buyEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(Parent, this.series.BuySymbol, "Buy");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.series.ReversalAmount = Utils.StringToDouble(this.textBox1.Text, 0.03);
        }
    }
}

