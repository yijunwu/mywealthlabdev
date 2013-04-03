namespace WealthLab
{
    using System;

    public class TrendLineEventArgs : EventArgs
    {
        private double double_0;
        private int int_0;
        private string string_0;

        public TrendLineEventArgs(string trendlineName, int int_1)
        {
            this.string_0 = trendlineName;
            this.int_0 = int_1;
        }

        public int Bar
        {
            get
            {
                return this.int_0;
            }
        }

        public string TrendlineName
        {
            get
            {
                return this.string_0;
            }
        }

        public double Value
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
    }
}

