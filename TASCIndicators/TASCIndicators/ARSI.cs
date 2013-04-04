namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class ARSI : DataSeries
    {
        private double _dnMove;
        private DataSeries _pc;
        private int _period;
        private DataSeries _sourceSeries;
        private double _upMove;

        public ARSI(DataSeries ds, int period, string description) : base(ds, description)
        {
            this._sourceSeries = ds;
            this._period = period;
            base.FirstValidValue = period - 1;
            if ((base.FirstValidValue > ds.Count) || (base.FirstValidValue < 0))
            {
                base.FirstValidValue = ds.Count;
            }
            if (ds.Count >= period)
            {
                Momentum source = Momentum.Series(ds, 1);
                this._pc = CumUp.Series(source, 1);
                this._pc /= this._pc;
                DataSeries series = Sum.Series(this._pc, period);
                DataSeries series2 = ((DataSeries) period) - series;
                base[0] = ds[0];
                double num = 0.0;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                for (int i = 1; i < (period - 1); i++)
                {
                    if (source[i] >= 0.0)
                    {
                        num += source[i];
                    }
                    else
                    {
                        num2 += Math.Abs(source[i]);
                    }
                }
                num /= (double) period;
                num2 /= (double) period;
                num3 = num / num2;
                base[period - 1] = 100.0 - (100.0 / (1.0 + num3));
                for (int j = period; j < ds.Count; j++)
                {
                    num4 = (source[j] >= 0.0) ? source[j] : 0.0;
                    if (series[j] != 0.0)
                    {
                        num += (num4 - num) / series[j];
                    }
                    num4 = (source[j] < 0.0) ? Math.Abs(source[j]) : 0.0;
                    if (series2[j] != 0.0)
                    {
                        num2 += (num4 - num2) / series2[j];
                    }
                    double num7 = (num2 != 0.0) ? num2 : 1.0;
                    num3 = num / num7;
                    base[j] = 100.0 - (100.0 / (1.0 + num3));
                    if (double.IsNaN(base[j]))
                    {
                        num3 = 0.0;
                    }
                }
                this._upMove = num;
                this._dnMove = num2;
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this._sourceSeries.Count < this._period) || double.IsNaN(this._sourceSeries.PartialValue))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                double num = this._upMove;
                double num2 = this._dnMove;
                int num3 = this._sourceSeries.Count - 1;
                double num4 = 0.0;
                double num5 = this._sourceSeries.PartialValue - this._sourceSeries[num3];
                int num6 = (num5 >= 0.0) ? 1 : 0;
                double num7 = Sum.Value(num3, this._pc, this._period - 1) + num6;
                double num8 = this._period - num7;
                num4 = (num5 >= 0.0) ? num5 : 0.0;
                if (num7 != 0.0)
                {
                    num += (num4 - this._upMove) / num7;
                }
                num4 = (num5 < 0.0) ? Math.Abs(num5) : 0.0;
                if (num8 != 0.0)
                {
                    num2 += (num4 - this._dnMove) / num8;
                }
                double num9 = (num2 > 0.0) ? num2 : 1.0;
                base.PartialValue = 100.0 - (100.0 / (1.0 + (num / num9)));
            }
        }

        public static ARSI Series(DataSeries ds, int period)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "ARSI(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (ARSI) ds.Cache[key];
            }
            ds.Cache[key] = series = new ARSI(ds, period, key);
            return (ARSI) series;
        }
    }
}

