namespace CtrlLib
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(NumEdit), "BANumEdit")]
    public class NumEdit : TextBox
    {
        private NumEditType m_inpType;

        public NumEdit()
        {
            this.InputType = NumEditType.Integer;
            this.ContextMenu = new ContextMenu();
        }

        private bool IsValid(string val, bool user)
        {
            bool flag = true;
            if (!val.Equals("") && !val.Equals(string.Empty))
            {
                if (user && val.Equals("-"))
                {
                    return flag;
                }
                try
                {
                    switch (this.m_inpType)
                    {
                        case NumEditType.Currency:
                        {
                            decimal num = decimal.Parse(val);
                            int index = val.IndexOf(".");
                            if (index != -1)
                            {
                                flag = val.Substring(index).Length <= 3;
                            }
                            return flag;
                        }
                        case NumEditType.Decimal:
                        {
                            decimal num5 = decimal.Parse(val);
                            return flag;
                        }
                        case NumEditType.Single:
                        {
                            float num3 = float.Parse(val);
                            return flag;
                        }
                        case NumEditType.Double:
                        {
                            double num4 = double.Parse(val);
                            return flag;
                        }
                        case NumEditType.SmallInteger:
                        {
                            short num6 = short.Parse(val);
                            return flag;
                        }
                        case NumEditType.Integer:
                        {
                            int num7 = int.Parse(val);
                            return flag;
                        }
                        case NumEditType.LargeInteger:
                        {
                            long num8 = long.Parse(val);
                            return flag;
                        }
                    }
                    throw new ApplicationException();
                }
                catch
                {
                    flag = false;
                }
            }
            return flag;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;
            if (!char.IsControl(keyChar))
            {
                if (keyChar.ToString() == " ")
                {
                    e.Handled = true;
                    return;
                }
                string val = base.Text.Substring(0, base.SelectionStart) + keyChar.ToString() + base.Text.Substring(base.SelectionStart + base.SelectionLength);
                if (!this.IsValid(val, true))
                {
                    e.Handled = true;
                }
            }
            if ((((this.m_inpType == NumEditType.Currency) || (this.m_inpType == NumEditType.Single)) || (this.m_inpType == NumEditType.Double)) && ((base.Text.Length == 0) && (keyChar == '.')))
            {
                base.Text = "0.";
                base.Select(2, 0);
            }
            base.OnKeyPress(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (base.Text != "")
            {
                if (!this.IsValid(base.Text, false))
                {
                    base.Text = "";
                }
                else if (double.Parse(base.Text) == 0.0)
                {
                    base.Text = "0";
                }
            }
            base.OnLeave(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData == (Keys.Control | Keys.V)) || (keyData == (Keys.Shift | Keys.Insert)))
            {
                IDataObject dataObject = Clipboard.GetDataObject();
                string val = base.Text.Substring(0, base.SelectionStart) + ((string) dataObject.GetData(DataFormats.Text)) + base.Text.Substring(base.SelectionStart + base.SelectionLength);
                if (!this.IsValid(val, true))
                {
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        [Category("Behavior"), Description("Sets the numeric type allowed")]
        public NumEditType InputType
        {
            get
            {
                return this.m_inpType;
            }
            set
            {
                this.m_inpType = value;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (this.IsValid(value, true))
                {
                    base.Text = value;
                }
            }
        }

        public decimal Value
        {
            get
            {
                decimal num;
                decimal.TryParse(this.Text, out num);
                return num;
            }
        }

        public enum NumEditType
        {
            Currency,
            Decimal,
            Single,
            Double,
            SmallInteger,
            Integer,
            LargeInteger
        }
    }
}

