namespace WealthLab
{
    using System;

    public class AccountEventArgs : EventArgs
    {
        private WealthLab.Account account_0;

        public AccountEventArgs(WealthLab.Account account)
        {
            this.account_0 = account;
        }

        public WealthLab.Account Account
        {
            get
            {
                return this.account_0;
            }
        }
    }
}

