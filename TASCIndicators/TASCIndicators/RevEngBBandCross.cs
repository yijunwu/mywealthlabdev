namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class RevEngBBandCross : DataSeries
    {
        private DataSeries _sdSer;
        private DataSeries _smaSer;
        private DataSeries _sumK;
        private double devs;
        private double devs2;
        private DataSeries ds;
        private DataSeries ds2;
        private double k1;
        private double k2;
        private int period;
        private double period_sr;
        private double periodM1_sr;
        private int upper;

        public RevEngBBandCross(DataSeries ds, int period, double Devs, bool Upper, string description) : base(ds, description)
        {
            this.upper = 1;
            this.ds = ds;
            this.ds2 = ds * ds;
            this.period = period;
            this.period_sr = Math.Sqrt((double) period);
            this.periodM1_sr = Math.Sqrt((double) (period - 1));
            if (!Upper)
            {
                this.upper = -1;
            }
            this.devs = Devs;
            this.devs2 = Devs * Devs;
            if (!Upper)
            {
                this.upper = -1;
            }
            this._smaSer = SMA.Series(ds, period);
            this._sdSer = StdDev.Series(ds, period, StdDevCalculation.Population);
            this._sumK = Sum.Series(this.ds2, period);
            this.k1 = this.devs * Math.Sqrt(((double) period) / ((period - 1) - this.devs2));
            this.k2 = this.devs * Math.Sqrt(((double) (period - 1)) / ((period - 1) - this.devs2));
            base.FirstValidValue = (ds.FirstValidValue + period) - 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int i = base.FirstValidValue; i < ds.Count; i++)
            {
                base[i] = this._smaSer[i] - ((this.upper * this.K(i)) * this._sdSer[i]);
            }
        }

        private double K(int currBar)
        {
            double num = this._sumK[currBar];
            num = Math.Sqrt(Math.Max((double) 0.0, (double) (num - (this.period * this._smaSer[currBar]))));
            if (Math.Abs((double) ((num / this.period_sr) - this._sdSer[currBar])) < Math.Abs((double) ((num / this.periodM1_sr) - this._sdSer[currBar])))
            {
                return this.k1;
            }
            return this.k2;
        }

        public static RevEngBBandCross Series(DataSeries ds, int period, double Devs, bool Upper)
        {
            DataSeries series;
            string str = "Upper";
            if (!Upper)
            {
                str = "Lower";
            }
            string key = string.Concat(new object[] { "RevEngBBandCross(", ds.Description, ",", period, ",", Devs, ",", str, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (RevEngBBandCross) ds.Cache[key];
            }
            ds.Cache[key] = series = new RevEngBBandCross(ds, period, Devs, Upper, key);
            return (RevEngBBandCross) series;
        }
    }
}

