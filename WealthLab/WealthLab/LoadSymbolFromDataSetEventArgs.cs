namespace WealthLab
{
    using System;

    public class LoadSymbolFromDataSetEventArgs : EventArgs
    {
        private WealthLab.Bars bars_0;
        private string string_0;
        private string string_1;

        public LoadSymbolFromDataSetEventArgs(string dataSetName, string symbol)
        {
            this.string_0 = dataSetName;
            this.string_1 = symbol;
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars_0;
            }
            set
            {
                this.bars_0 = value;
            }
        }

        public string DataSetName
        {
            get
            {
                return this.string_0;
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_1;
            }
        }
    }
}

