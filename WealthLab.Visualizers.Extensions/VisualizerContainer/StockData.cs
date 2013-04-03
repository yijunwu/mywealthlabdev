namespace VisualizerContainer
{
    using System;
    using System.Runtime.CompilerServices;

    public class StockData
    {
        public StockData()
        {
        }

        public StockData(DateTime date, double open, double high, double low, double close, long volume, double adjClose)
        {
            this.Date = date;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
            this.AdjClose = adjClose;
        }

        public double AdjClose { get; set; }

        public double Close { get; set; }

        public DateTime Date { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Open { get; set; }

        public long Volume { get; set; }
    }
}

