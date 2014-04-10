namespace WealthLab
{
    using System;

    public class AccountPositionEventArgs : EventArgs
    {
        private WealthLab.AccountPosition accountPosition;

        public AccountPositionEventArgs(WealthLab.AccountPosition accountPosition_1)
        {
            this.accountPosition = accountPosition_1;
        }

        public WealthLab.AccountPosition AccountPosition
        {
            get
            {
                return this.accountPosition;
            }
        }
    }
}

