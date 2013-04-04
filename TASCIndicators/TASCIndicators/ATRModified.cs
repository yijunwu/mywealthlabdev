namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class ATRModified : DataSeries
    {
        private int _period;
        private Bars _sB;
        private DataSeries HiLo;
        private DataSeries sma15HiLo;

        public ATRModified(Bars ds, int period, string description) : base(ds, description)
        {
            this._sB = ds;
            this._period = period;
            this.HiLo = ds.High - ds.Low;
            this.sma15HiLo = (DataSeries) (1.5 * SMA.Series(this.HiLo, period));
            base.FirstValidValue = period - 1;
            if ((base.FirstValidValue > ds.Count) || (base.FirstValidValue < 0))
            {
                base.FirstValidValue = ds.Count;
            }
            base[0] = 0.0;
            int num = period - 1;
            for (int i = 1; i < ds.Count; i++)
            {
                double num3 = this.TRModified(i);
                if (i < period)
                {
                    base[i] = ((base[i - 1] * (i - 1)) + num3) / ((double) i);
                }
                else
                {
                    base[i] = ((base[i - 1] * num) + num3) / ((double) period);
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this._sB.Count < this._period) || double.IsNaN(this._sB.Close.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                int count = this._sB.Count;
                double partialValue = this._sB.Close.PartialValue;
                double num2 = this._sB.High.PartialValue;
                double num3 = this._sB.Low.PartialValue;
                double num4 = ((this.sma15HiLo[count - 1] / 1.5) * this._period) - this.sma15HiLo[count - this._period];
                num4 = (1.5 * (num4 + (num2 - num3))) / ((double) this._period);
                double num5 = ((num2 - num3) < num4) ? this.HiLo[count] : num4;
                double num6 = (num3 <= this._sB.High[count - 1]) ? (num2 - this._sB.Close[count - 1]) : ((num2 - this._sB.Close[count - 1]) - ((num3 - this._sB.High[count - 1]) / 2.0));
                double num7 = (num2 >= this._sB.Low[count - 1]) ? (this._sB.Close[count - 1] - num3) : ((this._sB.Close[count - 1] - num3) - ((this._sB.Low[count - 1] - num2) / 2.0));
                double num9 = Math.Max(Math.Max(num5, num6), num7);
                base.PartialValue = ((base[count - 1] * (this._period - 1)) + num9) / ((double) this._period);
            }
        }

        public static ATRModified Series(Bars ds, int period)
        {
            DataSeries series;
            string key = "ATRModified(" + period + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (ATRModified) ds.Cache[key];
            }
            ds.Cache[key] = series = new ATRModified(ds, period, key);
            return (ATRModified) series;
        }

        private double TRModified(int bar)
        {
            double num = (this.HiLo[bar] < this.sma15HiLo[bar]) ? this.HiLo[bar] : this.sma15HiLo[bar];
            double num2 = (this._sB.Low[bar] <= this._sB.High[bar - 1]) ? (this._sB.High[bar] - this._sB.Close[bar - 1]) : ((this._sB.High[bar] - this._sB.Close[bar - 1]) - ((this._sB.Low[bar] - this._sB.High[bar - 1]) / 2.0));
            double num3 = (this._sB.High[bar] >= this._sB.Low[bar - 1]) ? (this._sB.Close[bar - 1] - this._sB.Low[bar]) : ((this._sB.Close[bar - 1] - this._sB.Low[bar]) - ((this._sB.Low[bar - 1] - this._sB.High[bar]) / 2.0));
            return Math.Max(Math.Max(num, num2), num3);
        }
    }
}

