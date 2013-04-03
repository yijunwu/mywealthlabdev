namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class AccumDist : DataSeries
    {
        private Bars bars_1;

        public AccumDist(Bars bars, string description) : base(bars, description)
        {
            DataSeries series;
            if (base.Cache.ContainsKey("DiffHighLow"))
            {
                series = base.Cache["DiffHighLow"];
            }
            else
            {
                series = bars.High - bars.Low;
                base.Cache["DiffHighLow"] = series;
            }
            this.bars_1 = bars;
            double num = 0.0;
            for (int i = 0; i < bars.Count; i++)
            {
                if (bars.High[i] != bars.Low[i])
                {
                    num += (((bars.Close[i] - bars.Low[i]) - (bars.High[i] - bars.Close[i])) / (bars.High[i] - bars.Low[i])) * bars.Volume[i];
                }
                base[i] = num;
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN))
            {
                base.PartialValue = base[base.Count - 1] + ((((this.bars_1.Close.PartialValue - this.bars_1.Low.PartialValue) - (this.bars_1.High.PartialValue - this.bars_1.Close.PartialValue)) / (this.bars_1.High.PartialValue - this.bars_1.Low.PartialValue)) * this.bars_1.Volume.PartialValue);
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static AccumDist Series(Bars bars)
        {
            string key = "AccumDist()";
            if (bars.Cache.ContainsKey(key))
            {
                return (AccumDist) bars.Cache[key];
            }
            AccumDist dist = new AccumDist(bars, key);
            bars.Cache[key] = dist;
            return dist;
        }
    }
}

