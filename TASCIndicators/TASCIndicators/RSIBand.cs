namespace TASCIndicators
{
    using System;
    using WealthLab;

    public class RSIBand : DataSeries
    {
        private DataSeries _ds;
        private double _N;
        private double _P;
        private int _period;
        private double _rsiTargetLevel;

        public RSIBand(DataSeries ds, int period, double rsiTargetLevel, string description) : base(ds, description)
        {
            this._ds = ds;
            this._period = period;
            this._rsiTargetLevel = rsiTargetLevel;
            base.FirstValidValue = (period - 1) + ds.FirstValidValue;
            double num = 0.0;
            this._P = 0.0;
            this._N = 0.0;
            base[0] = ds[0];
            for (int i = 1; i < ds.Count; i++)
            {
                double num3 = ds[i] - ds[i - 1];
                double num4 = 0.0;
                double num5 = 0.0;
                if (num3 > 0.0)
                {
                    num4 = num3;
                }
                if (num3 < 0.0)
                {
                    num5 = -num3;
                }
                if (base[i - 1] > ds[i - 1])
                {
                    num = ((ds[i - 1] + this._P) - (this._P * period)) - ((((this._N * period) - this._N) * rsiTargetLevel) / (rsiTargetLevel - 100.0));
                }
                else
                {
                    num = (((((ds[i - 1] - this._N) - this._P) + (this._N * period)) + (this._P * period)) + ((100.0 * this._P) / rsiTargetLevel)) - (((100.0 * this._P) * period) / rsiTargetLevel);
                }
                if ((num - ds[i]) > (0.1 * ds[i]))
                {
                    num = ds[i] * 1.1;
                }
                else if ((num - ds[i]) < (-0.1 * ds[i]))
                {
                    num = ds[i] * 0.9;
                }
                this._P = (((period - 1) * this._P) + num4) / ((double) period);
                this._N = (((period - 1) * this._N) + num5) / ((double) period);
                base[i] = num;
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this._ds.Count;
            if ((count < this._period) || double.IsNaN(this._ds.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num2 = this._ds.PartialValue - this._ds[count - 1];
                if (base[count - 1] > this._ds[count - 1])
                {
                    base.PartialValue = ((this._ds[count - 1] + this._P) - (this._P * this._period)) - ((((this._N * this._period) - this._N) * this._rsiTargetLevel) / (this._rsiTargetLevel - 100.0));
                }
                else
                {
                    base.PartialValue = (((((this._ds[count - 1] - this._N) - this._P) + (this._N * this._period)) + (this._P * this._period)) + ((100.0 * this._P) / this._rsiTargetLevel)) - (((100.0 * this._P) * this._period) / this._rsiTargetLevel);
                }
                if ((base.PartialValue - this._ds.PartialValue) > (0.1 * this._ds.PartialValue))
                {
                    base.PartialValue = this._ds.PartialValue * 1.1;
                }
                else if ((base.PartialValue - this._ds.PartialValue) < (-0.1 * this._ds.PartialValue))
                {
                    base.PartialValue = this._ds.PartialValue * 0.9;
                }
            }
        }

        public static RSIBand Series(DataSeries ds, int period, double rsiTargetLevel)
        {
            string key = string.Concat(new object[] { "RSIBand(", ds.Description, ",", period, ",", rsiTargetLevel, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (RSIBand) ds.Cache[key];
            }
            RSIBand band = new RSIBand(ds, period, rsiTargetLevel, key);
            ds.Cache[key] = band;
            return band;
        }
    }
}

