namespace WealthLab.IndexDefinitions
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class NumericParameter : UserControl
    {
        private decimal _defaultValue;
        private IContainer components;
        private Label ctrlDescription;
        private NumericUpDown ctrlNumber;

        public NumericParameter(string description, decimal defaultValue, int decimalPlaces, decimal maxValue, decimal minValue, decimal increment)
        {
            this.InitializeComponent();
            this._defaultValue = defaultValue;
            this.ctrlDescription.Text = description;
            int x = (this.ctrlDescription.Location.X + this.ctrlDescription.Width) + 10;
            int y = this.ctrlNumber.Location.Y;
            this.ctrlNumber.Location = new Point(x, y);
            this.ctrlNumber.Maximum = maxValue;
            this.ctrlNumber.Minimum = minValue;
            this.ctrlNumber.Value = this._defaultValue;
            this.ctrlNumber.DecimalPlaces = decimalPlaces;
            this.ctrlNumber.Increment = increment;
            base.Width = (this.ctrlNumber.Location.X + this.ctrlNumber.Width) + 10;
        }

        public void ClearFields()
        {
            this.ctrlNumber.Value = this._defaultValue;
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
            this.ctrlNumber = new NumericUpDown();
            this.ctrlDescription = new Label();
            this.ctrlNumber.BeginInit();
            base.SuspendLayout();
            this.ctrlNumber.Location = new Point(0xb3, 12);
            this.ctrlNumber.Name = "ctrlNumber";
            this.ctrlNumber.Size = new Size(120, 20);
            this.ctrlNumber.TabIndex = 3;
            this.ctrlDescription.AutoSize = true;
            this.ctrlDescription.Location = new Point(100, 14);
            this.ctrlDescription.Name = "ctrlDescription";
            this.ctrlDescription.Size = new Size(0x23, 13);
            this.ctrlDescription.TabIndex = 4;
            this.ctrlDescription.Text = "label1";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.ctrlDescription);
            base.Controls.Add(this.ctrlNumber);
            base.Name = "NumericParameter";
            base.Size = new Size(0x13b, 40);
            this.ctrlNumber.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public decimal Value
        {
            get
            {
                return this.ctrlNumber.Value;
            }
        }
    }
}

