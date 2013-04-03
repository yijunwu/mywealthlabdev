namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    public class SpecialHoursDialogForm : DialogFormBase
    {
        private DateTimePicker dtCloseTime;
        private DateTimePicker dtDate;
        private DateTimePicker dtOpenTime;
        private IContainer icontainer_1;
        private Label lblCloseTime;
        private Label lblDate;
        private Label lblOpenTime;
        private WealthLab.MarketSpecialHours marketSpecialHours_0;

        public SpecialHoursDialogForm()
        {
            this.InitializeComponent_1();
            this.dtOpenTime.Value = new DateTime(0x7d7, 9, 0x13, 9, 30, 0);
            this.dtCloseTime.Value = new DateTime(0x7d7, 9, 0x13, 13, 0, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_1 != null))
            {
                this.icontainer_1.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool EqualsMarketSpecialHours(WealthLab.MarketSpecialHours hours1, WealthLab.MarketSpecialHours hours2)
        {
            return ((hours1.Date.Equals(hours2.Date) && hours1.CloseTimeNative.Equals(hours2.CloseTimeNative)) && hours1.OpenTimeNative.Equals(hours2.OpenTimeNative));
        }

        private void InitializeComponent_1()
        {
            this.lblDate = new Label();
            this.dtDate = new DateTimePicker();
            this.dtOpenTime = new DateTimePicker();
            this.lblOpenTime = new Label();
            this.dtCloseTime = new DateTimePicker();
            this.lblCloseTime = new Label();
            ((ISupportInitialize) base.errProvider).BeginInit();
            base.SuspendLayout();
            base.btnOk.Location = new Point(0x4d, 0x60);
            base.btnOk.Click += new EventHandler(this.method_0);
            base.button1.Location = new Point(0x9b, 0x60);
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new Point(10, 15);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new Size(0x21, 13);
            this.lblDate.TabIndex = 9;
            this.lblDate.Text = "Date:";
            this.dtDate.Format = DateTimePickerFormat.Short;
            this.dtDate.Location = new Point(0x3e, 12);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new Size(0x74, 20);
            this.dtDate.TabIndex = 10;
            this.dtOpenTime.Format = DateTimePickerFormat.Time;
            this.dtOpenTime.Location = new Point(0x59, 0x26);
            this.dtOpenTime.Name = "dtOpenTime";
            this.dtOpenTime.ShowUpDown = true;
            this.dtOpenTime.Size = new Size(0x59, 20);
            this.dtOpenTime.TabIndex = 12;
            this.lblOpenTime.AutoSize = true;
            this.lblOpenTime.Location = new Point(10, 0x29);
            this.lblOpenTime.Name = "lblOpenTime";
            this.lblOpenTime.Size = new Size(0x3e, 13);
            this.lblOpenTime.TabIndex = 11;
            this.lblOpenTime.Text = "Open Time:";
            this.dtCloseTime.Format = DateTimePickerFormat.Time;
            this.dtCloseTime.Location = new Point(0x59, 0x40);
            this.dtCloseTime.Name = "dtCloseTime";
            this.dtCloseTime.ShowUpDown = true;
            this.dtCloseTime.Size = new Size(0x59, 20);
            this.dtCloseTime.TabIndex = 14;
            this.lblCloseTime.AutoSize = true;
            this.lblCloseTime.Location = new Point(10, 0x44);
            this.lblCloseTime.Name = "lblCloseTime";
            this.lblCloseTime.Size = new Size(0x3e, 13);
            this.lblCloseTime.TabIndex = 13;
            this.lblCloseTime.Text = "Close Time:";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0xef, 0x84);
            base.Controls.Add(this.dtCloseTime);
            base.Controls.Add(this.lblCloseTime);
            base.Controls.Add(this.dtOpenTime);
            base.Controls.Add(this.lblOpenTime);
            base.Controls.Add(this.dtDate);
            base.Controls.Add(this.lblDate);
            base.Name = "SpecialHoursDialogForm";
            this.Text = "Special Hours";
            base.Controls.SetChildIndex(base.btnOk, 0);
            base.Controls.SetChildIndex(base.button1, 0);
            base.Controls.SetChildIndex(this.lblDate, 0);
            base.Controls.SetChildIndex(this.dtDate, 0);
            base.Controls.SetChildIndex(this.lblOpenTime, 0);
            base.Controls.SetChildIndex(this.dtOpenTime, 0);
            base.Controls.SetChildIndex(this.lblCloseTime, 0);
            base.Controls.SetChildIndex(this.dtCloseTime, 0);
            ((ISupportInitialize) base.errProvider).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(object sender, EventArgs e)
        {
            base.errProvider.Clear();
            if ((this.marketSpecialHours_0 != null) && this.EqualsMarketSpecialHours(this.MarketSpecialHours, this.marketSpecialHours_0))
            {
                base.DialogResult = DialogResult.Cancel;
                return;
            }
            if ((this.marketSpecialHours_0 != null) && this.MarketSpecialHours.Date.Equals(this.marketSpecialHours_0.Date))
            {
                base.DialogResult = DialogResult.OK;
                return;
            }
            using (List<WealthLab.MarketSpecialHours>.Enumerator enumerator = base.CurrentMarketInfo.SpecialHours.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    WealthLab.MarketSpecialHours current = enumerator.Current;
                    if (current.Date.Equals(this.MarketSpecialHours.Date))
                    {
                        goto Label_00A1;
                    }
                }
                goto Label_00C7;
            Label_00A1:
                base.errProvider.SetError(this.dtDate, "This date already exists");
                return;
            }
        Label_00C7:
            base.DialogResult = DialogResult.OK;
        }

        public WealthLab.MarketSpecialHours MarketSpecialHours
        {
            get
            {
                WealthLab.MarketSpecialHours hours;
                return new WealthLab.MarketSpecialHours { Date = this.dtDate.Value.Date, OpenTimeNative = hours.Date.Add(this.dtOpenTime.Value.TimeOfDay), CloseTimeNative = hours.Date.Add(this.dtCloseTime.Value.TimeOfDay) };
            }
            set
            {
                this.marketSpecialHours_0 = value;
                this.dtDate.Value = value.Date;
                this.dtOpenTime.Value = value.OpenTimeNative;
                this.dtCloseTime.Value = value.CloseTimeNative;
            }
        }
    }
}

