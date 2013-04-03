namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab.DataProviders.MarketManagerService.Properties;

    public class MarketMangerForm : Form
    {
        private IContainer icontainer_0;
        private MarketManagerControl marketManagerControl_0 = new MarketManagerControl();

        public MarketMangerForm()
        {
            this.marketManagerControl_0.Parent = this;
            this.marketManagerControl_0.Dock = DockStyle.Fill;
            this.marketManagerControl_0.Visible = true;
            base.Width = 650;
            base.Height = 500;
            this.Text = "Market Manager";
            base.Icon = Resources.clock1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void method_0()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(MarketMangerForm));
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x26f, 0x166);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "MarketMangerForm";
            this.Text = "MarketMangerForm";
            base.ResumeLayout(false);
        }
    }
}

