namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class Quote
    {
        private DateTime dateTime_0;
        private double double_0;
        private double double_1;
        private double double_2;
        private double double_3;
        private double double_4;
        [CompilerGenerated]
        private double double_5;
        private string string_0;

        public Quote()
        {
        }

        public Quote(Quote quote_0)
        {
            this.double_3 = quote_0.Ask;
            this.double_2 = quote_0.Bid;
            this.double_4 = quote_0.PreviousClose;
            this.double_0 = quote_0.Price;
            this.double_1 = quote_0.Size;
            this.string_0 = quote_0.Symbol;
            this.dateTime_0 = quote_0.TimeStamp;
        }

        public double DistanceFrom(Quote quote_0)
        {
            double num = (quote_0.Price - this.Price) / quote_0.Price;
            if (num < 0.0)
            {
                num = -num;
            }
            return num;
        }

        public override string ToString()
        {
            return (this.Symbol + " " + this.Price);
        }

        public double Ask
        {
            get
            {
                return this.double_3;
            }
            set
            {
                this.double_3 = value;
            }
        }

        public double Bid
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

        public double Open
        {
            [CompilerGenerated]
            get
            {
                return this.double_5;
            }
            [CompilerGenerated]
            set
            {
                this.double_5 = value;
            }
        }

        public double PreviousClose
        {
            get
            {
                return this.double_4;
            }
            set
            {
                this.double_4 = value;
            }
        }

        public double Price
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

        public double Size
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

        public string Symbol
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

        public DateTime TimeStamp
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
    }
}

