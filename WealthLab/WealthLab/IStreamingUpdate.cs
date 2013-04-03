namespace WealthLab
{
    using System;

    public interface IStreamingUpdate
    {
        void Heartbeat(DateTime timeStamp);
        void Heartbeat(DateTime timeStamp, MarketInfo marketInfo);
        void UpdateMiniBar(Quote quote_0, double open, double high, double double_0);
        void UpdateQuote(Quote quote_0);
        void UpdateStreamingBar(string symbol, int barInterval, double open, double high, double double_0, double close, double volume, DateTime timeStamp, string debugInfo);
    }
}

