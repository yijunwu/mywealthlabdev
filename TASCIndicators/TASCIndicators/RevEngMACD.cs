namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class RevEngMACD : DataSeries
    {
        private DataSeries ds;
        private double level;
        private int period1;
        private int period2;

        public RevEngMACD(DataSeries ds, int period1, int period2, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period1 = period1;
            this.period2 = period2;
            base.FirstValidValue = Math.Max(period1, period2) * 3;
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = this.PMACDzero(i, ds, period1, period2);
            }
        }

        public RevEngMACD(DataSeries ds, int period1, int period2, double level, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period1 = period1;
            this.period2 = period2;
            this.level = level;
            base.FirstValidValue = Math.Max(period1, period2) * 3;
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = this.PMACDlevel(level, i, ds, period1, period2);
            }
        }

        private double PMACDeq(int bar, DataSeries price, int periodX, int periodY)
        {
            EMACalculation modern = EMACalculation.Modern;
            double num = 2.0 / (1.0 + periodX);
            double num2 = 2.0 / (1.0 + periodY);
            return (((EMA.Series(price, periodX, modern)[bar] * num) - (EMA.Series(price, periodY, modern)[bar] * num2)) / (num - num2));
        }

        private double PMACDlevel(double level, int bar, DataSeries price, int periodX, int periodY)
        {
            EMACalculation modern = EMACalculation.Modern;
            double num = 2.0 / (1.0 + periodX);
            double num2 = 2.0 / (1.0 + periodY);
            double num3 = 1.0 - num;
            double num4 = 1.0 - num2;
            return (((level + (EMA.Series(price, periodY, modern)[bar] * num4)) - (EMA.Series(price, periodX, modern)[bar] * num3)) / (num - num2));
        }

        private double PMACDzero(int bar, DataSeries price, int periodX, int periodY)
        {
            return this.PMACDlevel(0.0, bar, price, periodX, periodY);
        }

        public static RevEngMACD Series(DataSeries ds, int period1, int period2)
        {
            string key = string.Concat(new object[] { "RevEngMACD(", ds.Description, ",", period1, ",", period2, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (RevEngMACD) ds.Cache[key];
            }
            RevEngMACD gmacd = new RevEngMACD(ds, period1, period2, key);
            ds.Cache[key] = gmacd;
            return gmacd;
        }
    }
}

