namespace WealthLab
{
    using System;

    public class QuoteEventArgs : EventArgs
    {
        private WealthLab.Quote quote;

        public QuoteEventArgs(WealthLab.Quote quote)
        {
            this.quote = quote;
        }

        public WealthLab.Quote Quote
        {
            get
            {
                return this.quote;
            }
        }
    }
}

