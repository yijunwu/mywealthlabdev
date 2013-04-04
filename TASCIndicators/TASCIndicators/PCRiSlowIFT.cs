namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class PCRiSlowIFT : DataSeries
    {
        private DataSeries ds;

        public PCRiSlowIFT(DataSeries pcr, int rainbowPeriod, int wmaSmoothingPeriod, int rsiPeriod, string description) : base(pcr, description)
        {
            this.ds = pcr;
            base.FirstValidValue = Math.Max(rainbowPeriod * 3, wmaSmoothingPeriod);
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
            DataSeries series = new DataSeries(pcr, "PCRI Rainbow (slow)");
            DataSeries series2 = WMA.Series(pcr, rainbowPeriod);
            for (int j = 1; j <= 9; j++)
            {
                series2 = WMA.Series(series2, rainbowPeriod);
                series += series2;
            }
            series = (DataSeries) (series / 10.0);
            DataSeries series3 = WMA.Series(series, wmaSmoothingPeriod);
            DataSeries series4 = (DataSeries) (0.1 * (RSI.Series(series3, rsiPeriod) - 50.0));
            for (int k = base.FirstValidValue; k < this.ds.Count; k++)
            {
                double num4 = Math.Exp(2.0 * series4[k]);
                base[k] = 50.0 * (((num4 - 1.0) / (num4 + 1.0)) + 1.0);
            }
        }

        public override void CalculatePartialValue()
        {
        }

        public static PCRiSlowIFT Series(DataSeries pcr, int rainbowPeriod, int wmaSmoothingPeriod, int rsiPeriod)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "PCRiSlowIFT(", pcr.Description, ",", rainbowPeriod, ",", wmaSmoothingPeriod, ",", rsiPeriod, ")" });
            if (pcr.Cache.ContainsKey(key))
            {
                return (PCRiSlowIFT) pcr.Cache[key];
            }
            pcr.Cache[key] = series = new PCRiSlowIFT(pcr, rainbowPeriod, wmaSmoothingPeriod, rsiPeriod, key);
            return (PCRiSlowIFT) series;
        }
    }
}

