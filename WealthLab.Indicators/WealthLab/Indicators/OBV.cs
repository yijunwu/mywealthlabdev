namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class OBV : DataSeries
    {
        private Bars bars_1;

        public OBV(Bars bars, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            double num = 0.0;
            for (int i = 1; i < bars.Count; i++)
            {
                if (bars.Close[i] > bars.Close[i - 1])
                {
                    num += bars.Volume[i];
                }
                else if (bars.Close[i] < bars.Close[i - 1])
                {
                    num -= bars.Volume[i];
                }
                base[i] = num;
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN))
            {
                double num = 0.0;
                for (int i = 0; i < this.bars_1.Count; i++)
                {
                    if (this.bars_1.Close[i] > this.bars_1.Close[i - 1])
                    {
                        num += this.bars_1.Volume[i];
                    }
                    else if (this.bars_1.Close[i] < this.bars_1.Close[i - 1])
                    {
                        num -= this.bars_1.Volume[i];
                    }
                    base.PartialValue = num;
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static OBV Series(Bars bars)
        {
            string key = "OBV()";
            if (bars.Cache.ContainsKey(key))
            {
                return (OBV) bars.Cache[key];
            }
            OBV obv = new OBV(bars, key);
            bars.Cache[key] = obv;
            return obv;
        }
    }
}

