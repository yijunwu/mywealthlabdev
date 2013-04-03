namespace WealthLab.DataProviders.Helper
{
    using System;
    using WealthLab;

    public class SymbolInfo
    {
        public int Interval;
        public string Name;
        public BarScale Scale;
        public DateTime StartDate;

        public SymbolInfo()
        {
        }

        public SymbolInfo(string name, BarScale scale, int interval, DateTime startDate)
        {
            this.Name = name;
            this.Scale = scale;
            this.Interval = interval;
            this.StartDate = startDate;
        }
    }
}

