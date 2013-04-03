namespace WealthLab
{
    using System;
    using System.IO;

    public class BarDataStore : DataStore
    {
        public BarDataStore(IDataHost dataHost, StaticDataProvider provider) : base(dataHost.BaseDataFolder, provider.GetType().Name, "WL")
        {
        }

        public void InsertBar(Bars bars, int int_0, DateTime dateTime_0, double open, double high, double double_0, double close, double volume)
        {
            bars.method_4(int_0, dateTime_0, open, high, double_0, close, volume);
        }

        public void LoadBarsObject(Bars bars)
        {
            this.LoadBarsObject(bars, DateTime.MinValue, DateTime.MaxValue, 0);
        }

        public void LoadBarsObject(Bars bars, DateTime startDate, DateTime endDate, int maxBars)
        {
            string path = base.FileNameForBars(bars);
            if (File.Exists(path))
            {
                bars.LoadFromFile(path, startDate, endDate, maxBars);
            }
        }

        public void SaveBarsObject(Bars bars)
        {
            if (bars.Count > 0)
            {
                string fileName = base.FileNameForBars(bars);
                bars.SaveToFile(fileName);
            }
        }

        public void SaveDummyBarsObject(Bars bars)
        {
            string fileName = base.FileNameForBars(bars);
            bars.SaveToFile(fileName);
        }

        public override DateTime SymbolLastUpdated(string symbol, BarScale scale, int barInterval)
        {
            new BarDataScale(scale, barInterval);
            if (File.Exists(base.FileNameForSymbol(symbol, scale, barInterval)))
            {
                Bars bars = new Bars(symbol, scale, barInterval);
                this.LoadBarsObject(bars, DateTime.MinValue, DateTime.MaxValue, 1);
                if (bars.Count > 0)
                {
                    return bars.Date[bars.Count - 1];
                }
            }
            return DateTime.MinValue;
        }
    }
}

