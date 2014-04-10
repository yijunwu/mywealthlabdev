namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public class Account
    {
        private bool isPaperAccount;
        private DateTime accountValueTimeStamp = new DateTime();
        private double availableCash;
        private double buyingPower;
        private double accountValue;
        private List<AccountPosition> positions = new List<AccountPosition>();
        private string accountNumber = "";

        public override string ToString()
        {
            return this.AccountNumber;
        }

        public string AccountNumber
        {
            get
            {
                return this.accountNumber;
            }
            set
            {
                this.accountNumber = value;
            }
        }

        public double AccountValue
        {
            get
            {
                return this.accountValue;
            }
            set
            {
                this.accountValue = value;
            }
        }

        public DateTime AccountValueTimeStamp
        {
            get
            {
                return this.accountValueTimeStamp;
            }
            set
            {
                this.accountValueTimeStamp = value;
            }
        }

        public WealthLab.AutoTradingMode AutoTradingMode
        {
            get
            {
                if (!this.IsPaperAccount)
                {
                    return WealthLab.AutoTradingMode.Live;
                }
                return WealthLab.AutoTradingMode.Paper;
            }
        }

        public double AvailableCash
        {
            get
            {
                return this.availableCash;
            }
            set
            {
                this.availableCash = value;
            }
        }

        public double BuyingPower
        {
            get
            {
                return this.buyingPower;
            }
            set
            {
                this.buyingPower = value;
            }
        }

        public bool IsPaperAccount
        {
            get
            {
                return this.isPaperAccount;
            }
            set
            {
                this.isPaperAccount = value;
            }
        }

        public List<AccountPosition> Positions
        {
            get
            {
                return this.positions;
            }
            set
            {
                this.positions = value;
            }
        }
    }
}

