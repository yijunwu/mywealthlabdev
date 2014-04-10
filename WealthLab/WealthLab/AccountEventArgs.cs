namespace WealthLab
{
    using System;

    public class AccountEventArgs : EventArgs
    {
        private WealthLab.Account account;

        public AccountEventArgs(WealthLab.Account account)
        {
            this.account = account;
        }

        public WealthLab.Account Account
        {
            get
            {
                return this.account;
            }
        }
    }
}

