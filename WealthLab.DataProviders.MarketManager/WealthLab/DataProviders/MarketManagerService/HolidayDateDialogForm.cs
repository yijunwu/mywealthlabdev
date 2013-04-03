namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class HolidayDateDialogForm : DialogFormBase
    {
        private DateTime dateTime_0;
        private DateTimePicker dtHolidayDate;
        private IContainer icontainer_1;
        private Label lblHolidayDate;

        public HolidayDateDialogForm()
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
            this.lblHolidayDate = new Label();
            this.dtHolidayDate = new DateTimePicker();
            ((ISupportInitialize) base.errProvider).BeginInit();
            base.SuspendLayout();
            base.btnOk.Location = new Point(0x65, 0x2d);
            base.btnOk.Click += new EventHandler(this.method_0);
            base.button1.Location = new Point(0xb3, 0x2d);
            this.lblHolidayDate.AutoSize = true;
            this.lblHolidayDate.Location = new Point(10, 15);
            this.lblHolidayDate.Name = "lblHolidayDate";
            this.lblHolidayDate.Size = new Size(0x21, 13);
            this.lblHolidayDate.TabIndex = 9;
            this.lblHolidayDate.Text = "Date:";
            this.dtHolidayDate.Format = DateTimePickerFormat.Short;
            this.dtHolidayDate.Location = new Point(0x3e, 12);
            this.dtHolidayDate.Name = "dtHolidayDate";
            this.dtHolidayDate.Size = new Size(0xbd, 20);
            this.dtHolidayDate.TabIndex = 10;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x107, 0x51);
            base.Controls.Add(this.dtHolidayDate);
            base.Controls.Add(this.lblHolidayDate);
            base.Name = "HolidayDateDialogForm";
            this.Text = "Holiday Date";
            base.Controls.SetChildIndex(base.btnOk, 0);
            base.Controls.SetChildIndex(base.button1, 0);
            base.Controls.SetChildIndex(this.lblHolidayDate, 0);
            base.Controls.SetChildIndex(this.dtHolidayDate, 0);
            ((ISupportInitialize) base.errProvider).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(object sender, EventArgs e)
        {
            base.errProvider.Clear();
            if (this.HolidayDate.Equals(this.dateTime_0))
            {
                base.DialogResult = DialogResult.Cancel;
            }
            if (base.CurrentMarketInfo.Holidays.Contains(this.HolidayDate))
            {
                base.errProvider.SetError(this.dtHolidayDate, "This date already exists.");
            }
            else
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        public DateTime HolidayDate
        {
            get
            {
                return this.dtHolidayDate.Value.Date;
            }
            set
            {
                this.dateTime_0 = value;
                this.dtHolidayDate.Value = value;
            }
        }
    }
}

