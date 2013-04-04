namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class FVE : DataSeries
    {
        private Bars ds;
        private DataSeries MFSer;
        private int period;
        private DataSeries SMAMFSer;
        private DataSeries SMAVol;

        public FVE(Bars ds, int period, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            if ((period < 1) || (period > (ds.Count + 1)))
            {
                period = ds.Count + 1;
            }
            base.FirstValidValue = period;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            this.MFSer = new DataSeries(ds, "FVE_MF");
            for (int i = 1; i < ds.Count; i++)
            {
                double num2 = ((8.0 * ds.Close[i]) - (ds.High[i] + ds.Low[i])) - (2.0 * ((ds.Close[i - 1] + ds.High[i - 1]) + ds.Low[i - 1]));
                if (num2 > (0.018 * ds.Close[i]))
                {
                    this.MFSer[i] = ds.Volume[i];
                }
                else if (num2 < (-0.018 * ds.Close[i]))
                {
                    this.MFSer[i] = -ds.Volume[i];
                }
            }
            this.SMAMFSer = SMA.Series(this.MFSer, period);
            this.SMAVol = SMA.Series(ds.Volume, period);
            for (int j = 0; j < ds.Count; j++)
            {
                if (this.SMAVol[j] > 0.0)
                {
                    base[j] = (100.0 * this.SMAMFSer[j]) / this.SMAVol[j];
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.period < 1) || (this.period > (this.ds.Count + 1)))
            {
                base.PartialValue = double.NaN;
            }
            else
            {
                int count = this.ds.Count;
                double num2 = ((8.0 * this.ds.Close.PartialValue) - (this.ds.High.PartialValue + this.ds.Low.PartialValue)) - (2.0 * ((this.ds.Close[count - 1] + this.ds.High[count - 1]) + this.ds.Low[count - 1]));
                if (num2 > (0.018 * this.ds.Close.PartialValue))
                {
                    this.MFSer.PartialValue = this.ds.Volume.PartialValue;
                }
                else if (num2 < (-0.018 * this.ds.Close.PartialValue))
                {
                    this.MFSer.PartialValue = -this.ds.Volume.PartialValue;
                }
                else
                {
                    this.MFSer.PartialValue = 0.0;
                }
                this.SMAMFSer.CalculatePartialValue();
                this.SMAVol.CalculatePartialValue();
                if (this.SMAVol.PartialValue > 0.0)
                {
                    base.PartialValue = (100.0 * this.SMAMFSer.PartialValue) / this.SMAVol.PartialValue;
                }
                else
                {
                    base.PartialValue = 0.0;
                }
            }
        }

        public static FVE Series(Bars ds, int period)
        {
            DataSeries series;
            string key = "FVE(" + period + ")";
            if (ds.Cache.ContainsKey(key))
            {
                return (FVE) ds.Cache[key];
            }
            ds.Cache[key] = series = new FVE(ds, period, key);
            return (FVE) series;
        }
    }
}

