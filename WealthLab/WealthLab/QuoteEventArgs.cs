namespace WealthLab
{
    using System;

    public class QuoteEventArgs : EventArgs
    {
        private WealthLab.Quote quote_0;

        public QuoteEventArgs(WealthLab.Quote quote)
        {
            this.quote_0 = quote;
        }

        public WealthLab.Quote Quote
        {
            get
            {
                return this.quote_0;
            }
        }
    }
}

