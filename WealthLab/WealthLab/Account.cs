namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public class Account
    {
        private bool bool_0;
        private DateTime dateTime_0 = new DateTime();
        private double double_0;
        private double double_1;
        private double double_2;
        private List<AccountPosition> list_0 = new List<AccountPosition>();
        private string string_0 = "";

        public override string ToString()
        {
            return this.AccountNumber;
        }

        public string AccountNumber
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public double AccountValue
        {
            get
            {
                return this.double_2;
            }
            set
            {
                this.double_2 = value;
            }
        }

        public DateTime AccountValueTimeStamp
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
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
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }

        public double BuyingPower
        {
            get
            {
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
            }
        }

        public bool IsPaperAccount
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public List<AccountPosition> Positions
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }
    }
}

