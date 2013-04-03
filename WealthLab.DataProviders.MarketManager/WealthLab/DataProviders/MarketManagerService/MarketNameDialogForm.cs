namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class MarketNameDialogForm : DialogFormBase
    {
        private IContainer icontainer_1;
        private Label lblMarketName;
        private string string_0;
        private TextBox txtMarketName;

        public MarketNameDialogForm()
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
            this.txtMarketName = new TextBox();
            this.lblMarketName = new Label();
            ((ISupportInitialize) base.errProvider).BeginInit();
            base.SuspendLayout();
            base.btnOk.TabIndex = 1;
            base.btnOk.Click += new EventHandler(this.method_0);
            base.errProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            base.errProvider.RightToLeft = true;
            base.button1.TabIndex = 2;
            this.txtMarketName.Location = new Point(0x3e, 12);
            this.txtMarketName.Name = "txtMarketName";
            this.txtMarketName.Size = new Size(0xf9, 20);
            this.txtMarketName.TabIndex = 0;
            this.lblMarketName.AutoSize = true;
            this.lblMarketName.Location = new Point(10, 15);
            this.lblMarketName.Name = "lblMarketName";
            this.lblMarketName.Size = new Size(0x26, 13);
            this.lblMarketName.TabIndex = 8;
            this.lblMarketName.Text = "Name:";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x143, 0x51);
            base.Controls.Add(this.txtMarketName);
            base.Controls.Add(this.lblMarketName);
            base.Name = "MarketNameDialogForm";
            this.Text = "Market Name";
            base.Controls.SetChildIndex(base.button1, 0);
            base.Controls.SetChildIndex(base.btnOk, 0);
            base.Controls.SetChildIndex(this.lblMarketName, 0);
            base.Controls.SetChildIndex(this.txtMarketName, 0);
            ((ISupportInitialize) base.errProvider).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(object sender, EventArgs e)
        {
            base.errProvider.Clear();
            if (this.string_0 == this.MarketName)
            {
                base.DialogResult = DialogResult.Cancel;
            }
            if (this.MarketName.Trim() == string.Empty)
            {
                base.errProvider.SetError(this.txtMarketName, "Market name can't be empty.");
                return;
            }
            using (List<MarketInfo>.Enumerator enumerator = MarketManager.ArrayOfMarketInfo.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    MarketInfo current = enumerator.Current;
                    if (current.Name == this.MarketName)
                    {
                        goto Label_0087;
                    }
                }
                goto Label_00AD;
            Label_0087:
                base.errProvider.SetError(this.txtMarketName, "This Market name already exists.");
                return;
            }
        Label_00AD:
            base.DialogResult = DialogResult.OK;
        }

        public string MarketName
        {
            get
            {
                return this.txtMarketName.Text.Trim();
            }
            set
            {
                this.string_0 = value;
                this.txtMarketName.Text = value;
            }
        }
    }
}

