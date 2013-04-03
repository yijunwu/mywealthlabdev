namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ProvidersForm : DialogFormBase
    {
        private CheckedListBox clbProviders;
        private IContainer icontainer_1;
        private Label lblInfo;

        public ProvidersForm()
        {
            this.InitializeComponent_1();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_1 != null))
            {
                this.icontainer_1.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent_1()
        {
            this.lblInfo = new Label();
            this.clbProviders = new CheckedListBox();
            ((ISupportInitialize) base.errProvider).BeginInit();
            base.SuspendLayout();
            base.btnOk.Location = new Point(0xa1, 230);
            base.btnOk.Click += new EventHandler(this.method_0);
            base.button1.Location = new Point(0xef, 230);
            this.lblInfo.Location = new Point(9, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new Size(0x12e, 0x26);
            this.lblInfo.TabIndex = 8;
            this.lblInfo.Text = "The following providers are supported by the Market Manager. Enable Market Manager support for the provider(s):";
            this.clbProviders.FormattingEnabled = true;
            this.clbProviders.Location = new Point(12, 50);
            this.clbProviders.Name = "clbProviders";
            this.clbProviders.Size = new Size(0x12b, 0xa9);
            this.clbProviders.TabIndex = 9;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x143, 0x10a);
            base.Controls.Add(this.lblInfo);
            base.Controls.Add(this.clbProviders);
            base.Name = "ProvidersForm";
            this.Text = "Providers";
            base.Shown += new EventHandler(this.ProvidersForm_Shown);
            base.Controls.SetChildIndex(this.clbProviders, 0);
            base.Controls.SetChildIndex(this.lblInfo, 0);
            base.Controls.SetChildIndex(base.btnOk, 0);
            base.Controls.SetChildIndex(base.button1, 0);
            ((ISupportInitialize) base.errProvider).EndInit();
            base.ResumeLayout(false);
        }

        private void method_0(object sender, EventArgs e)
        {
            foreach (MarketManagerInfoAttribute attribute in MarketManager.Providers.Items)
            {
                attribute.Enabled = false;
            }
            foreach (object obj2 in this.clbProviders.CheckedItems)
            {
                (obj2 as MarketManagerInfoAttribute).Enabled = true;
            }
            base.DialogResult = DialogResult.OK;
        }

        private void ProvidersForm_Shown(object sender, EventArgs e)
        {
            foreach (MarketManagerInfoAttribute attribute in MarketManager.Providers.Items)
            {
                this.clbProviders.Items.Add(attribute, attribute.Enabled);
            }
        }
    }
}

