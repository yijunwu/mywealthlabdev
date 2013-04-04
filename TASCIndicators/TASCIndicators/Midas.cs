namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class Midas : DataSeries
    {
        private double _cumPV;
        private double _cumPVst;
        private double _cumV;
        private double _cumVst;
        private Bars ds;

        public Midas(Bars ds, int startBar, string description) : base(ds, description)
        {
            this.ds = ds;
            base.FirstValidValue = startBar;
            if ((base.FirstValidValue > ds.Count) || (base.FirstValidValue < 0))
            {
                base.FirstValidValue = ds.Count;
            }
            else
            {
                DataSeries series = AveragePrice.Series(ds);
                for (int i = 0; i <= base.FirstValidValue; i++)
                {
                    base[i] = series[i];
                    this._cumV += ds.Volume[i];
                    this._cumPV += series[i] * ds.Volume[i];
                }
                this._cumVst = this._cumV;
                this._cumPVst = this._cumPV;
                for (int j = base.FirstValidValue + 1; j < ds.Count; j++)
                {
                    this._cumV += ds.Volume[j];
                    this._cumPV += series[j] * ds.Volume[j];
                    double num3 = this._cumV - this._cumVst;
                    num3 = (num3 == 0.0) ? 1.0 : num3;
                    base[j] = (this._cumPV - this._cumPVst) / num3;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            this._cumV += this.ds.Volume.PartialValue;
            this._cumPV += ((this.ds.High.PartialValue + this.ds.Low.PartialValue) / 2.0) * this.ds.Volume.PartialValue;
            double num = this._cumV - this._cumVst;
            num = (num == 0.0) ? 1.0 : num;
            base.PartialValue = (this._cumPV - this._cumPVst) / num;
        }

        public static Midas Series(Bars ds, int startBar)
        {
            DataSeries series;
            string key = "Midas(" + startBar + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (Midas) ds.Cache[key];
            }
            ds.Cache[key] = series = new Midas(ds, startBar, key);
            return (Midas) series;
        }
    }
}

