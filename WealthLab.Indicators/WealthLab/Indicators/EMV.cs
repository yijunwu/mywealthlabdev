namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class EMV : DataSeries
    {
        private Bars bars_1;
        private DataSeries dataSeries_1;
        private int int_1;

        public EMV(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = period;
            if (this.int_1 < 1)
            {
                this.int_1 = 1;
            }
            base.FirstValidValue = this.int_1;
            if (this.int_1 > bars.Count)
            {
                base.FirstValidValue = bars.Count;
            }
            else
            {
                DataSeries series = Highest.Series(bars.High, this.int_1);
                DataSeries series2 = Lowest.Series(bars.Low, this.int_1);
                this.dataSeries_1 = (DataSeries) ((series + series2) / 2.0);
                DataSeries series3 = this.dataSeries_1 - (this.dataSeries_1 >> this.int_1);
                DataSeries series4 = (DataSeries) ((Sum.Series(this.bars_1.Volume, this.int_1) / 1000000.0) / (series - series2));
                for (int i = this.int_1; i < base.Count; i++)
                {
                    if (series4[i] == 0.0)
                    {
                        base[i] = base[i - 1];
                    }
                    else
                    {
                        base[i] = series3[i] / series4[i];
                    }
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.bars_1.Count >= this.int_1) && !double.IsNaN(this.bars_1.Close.PartialValue))
            {
                int num = this.bars_1.Count - 1;
                double num2 = Math.Max(Highest.Value(num, this.bars_1.High, this.int_1 - 1), this.bars_1.High.PartialValue);
                double num3 = Math.Min(Lowest.Value(num, this.bars_1.Low, this.int_1 - 1), this.bars_1.Low.PartialValue);
                double num4 = (num2 + num3) / 2.0;
                double num5 = num4 - this.dataSeries_1[num - (this.int_1 - 1)];
                double num6 = ((Sum.Value(num, this.bars_1.Volume, this.int_1 - 1) + this.bars_1.Volume.PartialValue) / 1000000.0) / (num2 - num3);
                if (num6 == 0.0)
                {
                    base.PartialValue = base[num];
                }
                else
                {
                    base.PartialValue = num5 / num6;
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static EMV Series(Bars bars, int period)
        {
            string key = "EMV(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (EMV) bars.Cache[key];
            }
            EMV emv = new EMV(bars, period, key);
            bars.Cache[key] = emv;
            return emv;
        }
    }
}

