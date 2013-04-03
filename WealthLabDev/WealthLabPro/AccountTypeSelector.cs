namespace WealthLabPro
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxBitmap(typeof(AccountTypeSelector), "AccountTypeSelector")]
    public class AccountTypeSelector : ComboBox
    {
        private bool bool_0;
        [CompilerGenerated]
        private bool bool_1;
        private Dictionary<string, string> dictionary_0;
        private IContainer icontainer_0;
        private string string_0;
        private string string_1;
        private string string_2;

        public AccountTypeSelector()
        {
            this.string_0 = "";
            this.string_1 = "";
            this.string_2 = "";
            this.dictionary_0 = new Dictionary<string, string>();
            this.method_3();
            base.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public AccountTypeSelector(IContainer container)
        {
            this.string_0 = "";
            this.string_1 = "";
            this.string_2 = "";
            this.dictionary_0 = new Dictionary<string, string>();
            container.Add(this);
            this.method_3();
            base.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public void InitAccountTradeType(string account, string action)
        {
            if (!this.IgnoreCalls && (((this.string_1 != account) || (this.method_0(this.string_2) != this.method_0(action))) || (this.bool_0 != MainModule.Instance.AuthProvider.LoggedIn)))
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    this.string_0 = this.Text;
                    if (!string.IsNullOrEmpty(this._key))
                    {
                        if (this.dictionary_0.ContainsKey(this._key))
                        {
                            this.dictionary_0.Remove(this._key);
                        }
                        this.dictionary_0.Add(this._key, this.string_0);
                    }
                }
                this.string_1 = account;
                this.string_2 = action;
                this.bool_0 = MainModule.Instance.AuthProvider.LoggedIn;
                IList<string> list = MainModule.Instance.AccountTradeTypes(account, action);
                base.BeginUpdate();
                base.Items.Clear();
                foreach (string str in list)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        base.Items.Add(str);
                    }
                }
                base.EndUpdate();
                this.method_1();
            }
        }

        private string method_0(string string_3)
        {
            if (string.IsNullOrEmpty(string_3))
            {
                return "";
            }
            if ((string_3 == TradeType.Buy.ToString()) || (string_3 == TradeType.Sell.ToString()))
            {
                return TradeType.Buy.ToString();
            }
            if (!(string_3 == TradeType.Cover.ToString()) && !(string_3 == TradeType.Short.ToString()))
            {
                return "";
            }
            return TradeType.Short.ToString();
        }

        private void method_1()
        {
            string str;
            if (!this.dictionary_0.TryGetValue(this._key, out str) || !this.method_2(str))
            {
                str = MainModule.Instance.DefaultAccountTradeType(this.string_1, this.string_2);
                if ((!this.method_2(str) && !this.method_2(this.string_0)) && (base.Items.Count > 0))
                {
                    this.SelectedIndex = 0;
                }
            }
        }

        private bool method_2(string string_3)
        {
            if (base.Items.Count <= 0)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(string_3))
            {
                this.SelectedIndex = base.Items.IndexOf(string_3);
            }
            else
            {
                this.SelectedIndex = 0;
            }
            return (this.SelectedIndex > -1);
        }

        private void method_3()
        {
            this.icontainer_0 = new Container();
        }

        public bool SelectAccountTradeType(string accountTradeType, bool force)
        {
            if (this.method_2(accountTradeType))
            {
                return true;
            }
            if (!force)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(accountTradeType))
            {
                base.Items.Add(accountTradeType);
            }
            return this.method_2(accountTradeType);
        }

        private string _key
        {
            get
            {
                return (this.string_1 + this.method_0(this.string_2));
            }
        }

        public bool IgnoreCalls
        {
            [CompilerGenerated]
            get
            {
                return this.bool_1;
            }
            [CompilerGenerated]
            set
            {
                this.bool_1 = value;
            }
        }
    }
}

