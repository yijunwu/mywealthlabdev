namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class PCRiFast : DataSeries
    {
        private DataSeries ds;

        public PCRiFast(DataSeries pcr, int rsiPeriod, int wmaPeriod, string description) : base(pcr, description)
        {
            this.ds = pcr;
            base.FirstValidValue = Math.Max(rsiPeriod * 3, wmaPeriod);
            if (base.FirstValidValue > pcr.Count)
            {
                base.FirstValidValue = pcr.Count;
            }
            for (int i = 0; i < pcr.Count; i++)
            {
                if (pcr[i] > 0.9)
                {
                    pcr[i] = 0.9;
                }
                else if (pcr[i] < 0.45)
                {
                    pcr[i] = 0.45;
                }
            }
            pcr = TEMA.Series(pcr, 5, EMACalculation.Modern);
            DataSeries series = new DataSeries(pcr, "PCRI Rainbow (fast)");
            DataSeries series2 = WMA.Series(pcr, 2);
            for (int j = 1; j <= 9; j++)
            {
                series2 = WMA.Series(series2, 2);
                series += series2;
            }
            series = (DataSeries) (series / 10.0);
            series = RSI.Series(series, rsiPeriod);
            for (int k = wmaPeriod - 1; k < pcr.Count; k++)
            {
                base[k] = WMA.Series(series, wmaPeriod)[k];
            }
        }

        public override void CalculatePartialValue()
        {
        }

        public static PCRiFast Series(DataSeries pcr, int rsiPeriod, int wmaPeriod)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "PCRiFast(", pcr.Description, ",", rsiPeriod, ",", wmaPeriod, ")" });
            if (pcr.Cache.ContainsKey(key))
            {
                return (PCRiFast) pcr.Cache[key];
            }
            pcr.Cache[key] = series = new PCRiFast(pcr, rsiPeriod, wmaPeriod, key);
            return (PCRiFast) series;
        }
    }
}

