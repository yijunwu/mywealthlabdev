namespace WealthLabPro
{
    using CtrlLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using GroupBox = System.Windows.Forms.GroupBox;

    public class StockSplitForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private DateTimePicker dtpSplit;
        private GroupBox grpDate;
        private GroupBox grpSplitBasis;
        private IContainer icontainer_0;
        private Label lblFor;
        private Label lblSplitDate;
        private NumEdit numDenominator;
        private NumEdit numNumerator;

        public StockSplitForm()
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
            this.grpDate = new GroupBox();
            this.dtpSplit = new DateTimePicker();
            this.lblSplitDate = new Label();
            this.grpSplitBasis = new GroupBox();
            this.lblFor = new Label();
            this.numDenominator = new NumEdit();
            this.numNumerator = new NumEdit();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.grpDate.SuspendLayout();
            this.grpSplitBasis.SuspendLayout();
            base.SuspendLayout();
            this.grpDate.Controls.Add(this.dtpSplit);
            this.grpDate.Controls.Add(this.lblSplitDate);
            this.grpDate.Location = new Point(12, 12);
            this.grpDate.Name = "grpDate";
            this.grpDate.Size = new Size(0xda, 100);
            this.grpDate.TabIndex = 0;
            this.grpDate.TabStop = false;
            this.grpDate.Text = "Stock Split Date";
            this.dtpSplit.Format = DateTimePickerFormat.Short;
            this.dtpSplit.Location = new Point(10, 0x45);
            this.dtpSplit.Name = "dtpSplit";
            this.dtpSplit.Size = new Size(0x71, 20);
            this.dtpSplit.TabIndex = 1;
            this.lblSplitDate.Location = new Point(7, 20);
            this.lblSplitDate.Name = "lblSplitDate";
            this.lblSplitDate.Size = new Size(0xcd, 0x2d);
            this.lblSplitDate.TabIndex = 0;
            this.lblSplitDate.Text = "Select the last date before the stock split.  This is the last date that prices were at their pre-split values.";
            this.grpSplitBasis.Controls.Add(this.lblFor);
            this.grpSplitBasis.Controls.Add(this.numDenominator);
            this.grpSplitBasis.Controls.Add(this.numNumerator);
            this.grpSplitBasis.Location = new Point(0xed, 13);
            this.grpSplitBasis.Name = "grpSplitBasis";
            this.grpSplitBasis.Size = new Size(0x90, 100);
            this.grpSplitBasis.TabIndex = 1;
            this.grpSplitBasis.TabStop = false;
            this.grpSplitBasis.Text = "Split Basis";
            this.lblFor.AutoSize = true;
            this.lblFor.Location = new Point(0x3b, 0x19);
            this.lblFor.Name = "lblFor";
            this.lblFor.Size = new Size(0x13, 13);
            this.lblFor.TabIndex = 2;
            this.lblFor.Text = "for";
            this.numDenominator.InputType = NumEdit.NumEditType.Integer;
            this.numDenominator.Location = new Point(0x54, 0x13);
            this.numDenominator.Name = "numDenominator";
            this.numDenominator.Size = new Size(0x25, 20);
            this.numDenominator.TabIndex = 1;
            this.numDenominator.Text = "1";
            this.numNumerator.InputType = NumEdit.NumEditType.Integer;
            this.numNumerator.Location = new Point(15, 0x13);
            this.numNumerator.Name = "numNumerator";
            this.numNumerator.Size = new Size(0x25, 20);
            this.numNumerator.TabIndex = 0;
            this.numNumerator.Text = "2";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x130, 0x84);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new Point(0xdf, 0x84);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x187, 0xa7);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.grpSplitBasis);
            base.Controls.Add(this.grpDate);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "StockSplitForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Stock Split";
            this.grpDate.ResumeLayout(false);
            this.grpSplitBasis.ResumeLayout(false);
            this.grpSplitBasis.PerformLayout();
            base.ResumeLayout(false);
        }

        public double Factor
        {
            get
            {
                try
                {
                    return (double) (this.numDenominator.Value / this.numNumerator.Value);
                }
                catch
                {
                    return 1.0;
                }
            }
        }

        public DateTime SplitDate
        {
            get
            {
                return this.dtpSplit.Value.Date;
            }
        }
    }
}

