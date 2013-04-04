namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class PCRiSlow : DataSeries
    {
        private DataSeries ds;

        public PCRiSlow(DataSeries pcr, int rainbowPeriod, int wmaSmoothingPeriod, string description) : base(pcr, description)
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
            for (int k = wmaSmoothingPeriod - 1; k < pcr.Count; k++)
            {
                base[k] = WMA.Series(series, wmaSmoothingPeriod)[k];
            }
        }

        public override void CalculatePartialValue()
        {
        }

        public static PCRiSlow Series(DataSeries pcr, int rainbowPeriod, int wmaSmoothingPeriod)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "PCRiSlow(", pcr.Description, ",", rainbowPeriod, ",", wmaSmoothingPeriod, ")" });
            if (pcr.Cache.ContainsKey(key))
            {
                return (PCRiSlow) pcr.Cache[key];
            }
            pcr.Cache[key] = series = new PCRiSlow(pcr, rainbowPeriod, wmaSmoothingPeriod, key);
            return (PCRiSlow) series;
        }
    }
}

