namespace WealthLab
{
    using System;

    public class AccountPositionEventArgs : EventArgs
    {
        private WealthLab.AccountPosition accountPosition_0;

        public AccountPositionEventArgs(WealthLab.AccountPosition accountPosition_1)
        {
            this.accountPosition_0 = accountPosition_1;
        }

        public WealthLab.AccountPosition AccountPosition
        {
            get
            {
                return this.accountPosition_0;
            }
        }
    }
}

