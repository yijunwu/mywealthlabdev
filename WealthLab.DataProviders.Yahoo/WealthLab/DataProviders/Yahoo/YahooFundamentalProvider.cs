namespace WealthLab.DataProviders.Yahoo
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;
    using WealthLab.DataProviders.Yahoo.Properties;

    public class YahooFundamentalProvider : FundamentalDataProvider
    {
        public FundamentalDataStore _fds;
        private object object_0 = new object();
        private string[] symbolSpecificItemsProvided = new string[] { "Split (Yahoo! Finance)", "Dividend (Yahoo! Finance)" };
        private string[] nonSymbolSpecificItemsProvided = new string[0];

        public override FundamentalItem CreateItem(string itemName)
        {
            if (itemName == "Split (Yahoo! Finance)")
            {
                return new FundamentalItemYahooSplit("Split (Yahoo! Finance)");
            }
            if (itemName == "Dividend (Yahoo! Finance)")
            {
                return new FundamentalItemYahooDividend("Dividend (Yahoo! Finance)");
            }
            return new FundamentalItem(itemName);
        }

        public override void Initialize(IDataHost dataHost)
        {
            base.Initialize(dataHost);
            this._fds = new FundamentalDataStore(dataHost, this);
        }

        public override IList<FundamentalItem> RequestItems(string itemName)
        {
            return this._fds.RequestNonSymbolItems(itemName);
        }

        public override IList<FundamentalItem> RequestItems(string symbol, string itemName)
        {
            return this._fds.RequestSymbolItems(symbol, itemName);
        }

        public void UpdateData(string symbol, List<FundamentalItem> items, string itemName)
        {
            lock (this.object_0)
            {
                this._fds.UpdateSymbolItems(symbol, itemName, items);
                this._fds.SaveSymbolItems();
            }
        }

        public override string Description
        {
            get
            {
                return "Delivers historical split and dividend data for securities.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Yahoo! Fundamental data";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Yahoo;
            }
        }

        public override IList<string> NonSymbolSpecificItemsProvided
        {
            get
            {
                return this.nonSymbolSpecificItemsProvided;
            }
        }

        public override IList<string> SymbolSpecificItemsProvided
        {
            get
            {
                return this.symbolSpecificItemsProvided;
            }
        }
    }
}

