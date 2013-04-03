namespace WealthLab.Rules
{
    using System;
    using WealthLab;

    public abstract class MarketSentiment
    {
        protected string m_sExchange;
        protected WealthScript m_ws;

        protected MarketSentiment()
        {
        }

        public DataSeries getExternalSeries(string symbol, string series, bool synch)
        {
            DataSeries volume;
            Bars externalSymbol = this.m_ws.GetExternalSymbol(symbol, synch);
            if (series == "Volume")
            {
                volume = externalSymbol.Volume;
            }
            else
            {
                volume = externalSymbol.Close;
            }
            if (synch)
            {
                this.m_ws.Synchronize(volume);
            }
            return volume;
        }

        public string getSeriesType(string series)
        {
            if (series == "Volume")
            {
                return series;
            }
            return "Issues";
        }

        public string Exchange
        {
            set
            {
                this.m_sExchange = value;
            }
        }
    }
}

