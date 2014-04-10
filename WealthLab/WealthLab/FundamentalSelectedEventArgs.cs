namespace WealthLab
{
    using System;

    public class FundamentalSelectedEventArgs : EventArgs
    {
        private FundamentalDataProvider fundamentalDataProvider;
        private string itemName;

        public FundamentalSelectedEventArgs(FundamentalDataProvider provider, string itemName)
        {
            this.fundamentalDataProvider = provider;
            this.itemName = itemName;
        }

        public string ItemName
        {
            get
            {
                return this.itemName;
            }
        }

        public FundamentalDataProvider Provider
        {
            get
            {
                return this.fundamentalDataProvider;
            }
        }
    }
}

