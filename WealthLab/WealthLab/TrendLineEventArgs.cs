namespace WealthLab
{
    using System;

    public class TrendLineEventArgs : EventArgs
    {
        private double doubleValue;
        private int bar;
        private string trendlineName;

        public TrendLineEventArgs(string trendlineName, int int_1)
        {
            this.trendlineName = trendlineName;
            this.bar = int_1;
        }

        public int Bar
        {
            get
            {
                return this.bar;
            }
        }

        public string TrendlineName
        {
            get
            {
                return this.trendlineName;
            }
        }

        public double Value
        {
            get
            {
                return this.doubleValue;
            }
            set
            {
                this.doubleValue = value;
            }
        }
    }
}

