namespace WealthLab
{
    using System;

    public class FundamentalSelectedEventArgs : EventArgs
    {
        private FundamentalDataProvider fundamentalDataProvider_0;
        private string string_0;

        public FundamentalSelectedEventArgs(FundamentalDataProvider provider, string itemName)
        {
            this.fundamentalDataProvider_0 = provider;
            this.string_0 = itemName;
        }

        public string ItemName
        {
            get
            {
                return this.string_0;
            }
        }

        public FundamentalDataProvider Provider
        {
            get
            {
                return this.fundamentalDataProvider_0;
            }
        }
    }
}

