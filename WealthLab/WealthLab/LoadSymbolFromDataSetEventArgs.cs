namespace WealthLab
{
    using System;

    public class LoadSymbolFromDataSetEventArgs : EventArgs
    {
        private WealthLab.Bars bars;
        private string dataSetName;
        private string symbol;

        public LoadSymbolFromDataSetEventArgs(string dataSetName, string symbol)
        {
            this.dataSetName = dataSetName;
            this.symbol = symbol;
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars;
            }
            set
            {
                this.bars = value;
            }
        }

        public string DataSetName
        {
            get
            {
                return this.dataSetName;
            }
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
        }
    }
}

