namespace WealthLab.ChartControl
{
    using System;
    using WealthLab;

    public class DraggedFundamentalItem
    {
        private FundamentalDataProvider fundamentalDataProvider;
        private string itemName;

        public DraggedFundamentalItem(FundamentalDataProvider provider, string itemName)
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

