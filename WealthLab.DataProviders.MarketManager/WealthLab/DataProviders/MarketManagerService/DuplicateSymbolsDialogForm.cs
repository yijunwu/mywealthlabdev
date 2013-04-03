namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DuplicateSymbolsDialogForm : DialogFormBase
    {
        private IContainer icontainer_1;
        private Label lblAction;
        private Label lblInfo;
        private RadioButton rbCancel;
        private RadioButton rbFromOtherMarkets;
        private RadioButton rbFromThisMarket;
        private TextBox txtDuplicateSymbols;

        public DuplicateSymbolsDialogForm()
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
            this.txtDuplicateSymbols = new TextBox();
            this.lblAction = new Label();
            this.rbFromOtherMarkets = new RadioButton();
            this.rbFromThisMarket = new RadioButton();
            this.rbCancel = new RadioButton();
            ((ISupportInitialize) base.errProvider).BeginInit();
            base.SuspendLayout();
            base.btnOk.Location = new Point(0xa2, 0xee);
            base.btnOk.Click += new EventHandler(this.method_0);
            base.button1.Location = new Point(240, 0xee);
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new Point(12, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new Size(0x113, 13);
            this.lblInfo.TabIndex = 7;
            this.lblInfo.Text = "The following symbol(s) are duplicate in different markets:";
            this.txtDuplicateSymbols.Location = new Point(15, 0x19);
            this.txtDuplicateSymbols.Multiline = true;
            this.txtDuplicateSymbols.Name = "txtDuplicateSymbols";
            this.txtDuplicateSymbols.ReadOnly = true;
            this.txtDuplicateSymbols.ScrollBars = ScrollBars.Both;
            this.txtDuplicateSymbols.Size = new Size(0x129, 0x5f);
            this.txtDuplicateSymbols.TabIndex = 8;
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new Point(12, 0x80);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new Size(0xd6, 13);
            this.lblAction.TabIndex = 9;
            this.lblAction.Text = "Choose one of the possible actions:";
            this.rbFromOtherMarkets.AutoSize = true;
            this.rbFromOtherMarkets.Location = new Point(15, 0x90);
            this.rbFromOtherMarkets.Name = "rbFromOtherMarkets";
            this.rbFromOtherMarkets.Size = new Size(0xce, 0x11);
            this.rbFromOtherMarkets.TabIndex = 10;
            this.rbFromOtherMarkets.Text = "Remove duplicate symbol(s) from other markets";
            this.rbFromOtherMarkets.UseVisualStyleBackColor = true;
            this.rbFromThisMarket.AutoSize = true;
            this.rbFromThisMarket.Location = new Point(15, 0xa7);
            this.rbFromThisMarket.Name = "rbFromThisMarket";
            this.rbFromThisMarket.Size = new Size(0xc3, 0x11);
            this.rbFromThisMarket.TabIndex = 11;
            this.rbFromThisMarket.Text = "Remove duplicate symbol(s) from this market";
            this.rbFromThisMarket.UseVisualStyleBackColor = true;
            this.rbCancel.AutoSize = true;
            this.rbCancel.Checked = true;
            this.rbCancel.Location = new Point(15, 190);
            this.rbCancel.Name = "rbCancel";
            this.rbCancel.Size = new Size(190, 0x11);
            this.rbCancel.TabIndex = 12;
            this.rbCancel.TabStop = true;
            this.rbCancel.Text = "Undo symbol change";
            this.rbCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x144, 0x112);
            base.Controls.Add(this.rbCancel);
            base.Controls.Add(this.rbFromThisMarket);
            base.Controls.Add(this.lblInfo);
            base.Controls.Add(this.txtDuplicateSymbols);
            base.Controls.Add(this.rbFromOtherMarkets);
            base.Controls.Add(this.lblAction);
            base.Name = "DuplicateSymbolsDialogForm";
            this.Text = "Duplicate Symbols";
            base.Controls.SetChildIndex(this.lblAction, 0);
            base.Controls.SetChildIndex(this.rbFromOtherMarkets, 0);
            base.Controls.SetChildIndex(this.txtDuplicateSymbols, 0);
            base.Controls.SetChildIndex(this.lblInfo, 0);
            base.Controls.SetChildIndex(base.btnOk, 0);
            base.Controls.SetChildIndex(base.button1, 0);
            base.Controls.SetChildIndex(this.rbFromThisMarket, 0);
            base.Controls.SetChildIndex(this.rbCancel, 0);
            ((ISupportInitialize) base.errProvider).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(object sender, EventArgs e)
        {
            if (this.rbCancel.Checked)
            {
                base.DialogResult = DialogResult.Cancel;
            }
            else
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        public WealthLab.DataProviders.MarketManagerService.DeleteDuplicateOption DeleteDuplicateOption
        {
            get
            {
                if (this.rbFromOtherMarkets.Checked)
                {
                    return WealthLab.DataProviders.MarketManagerService.DeleteDuplicateOption.FromOtherMarkets;
                }
                return WealthLab.DataProviders.MarketManagerService.DeleteDuplicateOption.FromThisMarket;
            }
        }

        public Symbols DuplicateSymbols
        {
            set
            {
                this.txtDuplicateSymbols.Text = value.Text;
            }
        }
    }
}

