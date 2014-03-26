namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class Quote
    {
        private DateTime timeStamp;
        private double price;
        private double size;
        private double bid;
        private double ask;
        private double previousClose;
        [CompilerGenerated]
        private double open;
        private string symbol;

        public Quote()
        {
        }

        public Quote(Quote quote_0)
        {
            this.ask = quote_0.Ask;
            this.bid = quote_0.Bid;
            this.previousClose = quote_0.PreviousClose;
            this.price = quote_0.Price;
            this.size = quote_0.Size;
            this.symbol = quote_0.Symbol;
            this.timeStamp = quote_0.TimeStamp;
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
                return this.ask;
            }
            set
            {
                this.ask = value;
            }
        }

        public double Bid
        {
            get
            {
                return this.bid;
            }
            set
            {
                this.bid = value;
            }
        }

        public double Open
        {
            [CompilerGenerated]
            get
            {
                return this.open;
            }
            [CompilerGenerated]
            set
            {
                this.open = value;
            }
        }

        public double PreviousClose
        {
            get
            {
                return this.previousClose;
            }
            set
            {
                this.previousClose = value;
            }
        }

        public double Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }

        public double Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return this.timeStamp;
            }
            set
            {
                this.timeStamp = value;
            }
        }
    }
}

