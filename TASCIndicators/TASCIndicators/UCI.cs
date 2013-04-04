namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class UCI : DataSeries
    {
        private DataSeries ds;
        private DataSeries dvs;
        private DataSeries lry;

        public UCI(DataSeries ds, int period, int volaperiod, string description) : base(ds, description)
        {
            this.ds = ds;
            if (period < 12)
            {
                period = 12;
            }
            period /= 2;
            DataSeries series = EMA.Series(ds, period / 2, EMACalculation.Modern) / EMA.Series(ds, period, EMACalculation.Modern);
            this.lry = LinearReg.Series(series, period / 2);
            this.dvs = DVS.Series(ds, volaperiod);
            base.FirstValidValue = Math.Max(this.dvs.FirstValidValue, ((ds.FirstValidValue + period) + (period / 2)) - 2);
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                if (this.dvs[i] != 0.0)
                {
                    base[i] = (10000.0 * (this.lry[i] - 1.0)) / this.dvs[i];
                }
            }
        }

        public override void CalculatePartialValue()
        {
            this.lry.CalculatePartialValue();
            this.dvs.CalculatePartialValue();
            if (double.IsNaN(this.lry.PartialValue) || double.IsNaN(this.dvs.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else if (this.dvs.PartialValue == 0.0)
            {
                base.PartialValue = 0.0;
            }
            else
            {
                base.PartialValue = (10000.0 * (this.lry.PartialValue - 1.0)) / this.dvs.PartialValue;
            }
        }

        public static UCI Series(DataSeries ds, int period, int volaper)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "UCI(", ds.Description, ",", period, ",", volaper, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (UCI) ds.Cache[key];
            }
            ds.Cache[key] = series = new UCI(ds, period, volaper, key);
            return (UCI) series;
        }
    }
}

