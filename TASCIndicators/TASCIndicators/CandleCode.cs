namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class CandleCode : DataSeries
    {
        private DataSeries B_Lower;
        private DataSeries B_Upper;
        private DataSeries BS;
        private Bars ds;
        private DataSeries L_Lower;
        private DataSeries L_Upper;
        private DataSeries LS;
        private DataSeries U_Lower;
        private DataSeries U_Upper;
        private DataSeries US;

        public CandleCode(Bars ds, string description) : base(ds, description)
        {
            this.ds = ds;
            this.US = new DataSeries(ds, "UpperShadow");
            this.BS = new DataSeries(ds, "BodySize");
            this.LS = new DataSeries(ds, "LowerShadow");
            for (int i = 0; i < ds.Count; i++)
            {
                if (ds.Open[i] > ds.Close[i])
                {
                    this.US[i] = ds.High[i] - ds.Open[i];
                    this.BS[i] = ds.Open[i] - ds.Close[i];
                    this.LS[i] = ds.Close[i] - ds.Low[i];
                }
                else
                {
                    this.US[i] = ds.High[i] - ds.Close[i];
                    this.BS[i] = ds.Close[i] - ds.Open[i];
                    this.LS[i] = ds.Open[i] - ds.Low[i];
                }
            }
            this.U_Upper = BBandUpper.Series(this.US, 20, 0.5);
            this.U_Lower = BBandLower.Series(this.US, 20, 0.5);
            this.B_Upper = BBandUpper.Series(this.BS, 20, 0.5);
            this.B_Lower = BBandLower.Series(this.BS, 20, 0.5);
            this.L_Upper = BBandUpper.Series(this.LS, 20, 0.5);
            this.L_Lower = BBandLower.Series(this.LS, 20, 0.5);
            base.FirstValidValue = 20;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int j = base.FirstValidValue; j < ds.Count; j++)
            {
                int num3 = 0x2a;
                if (this.US[j] > this.U_Upper[j])
                {
                    num3 += 0x10;
                }
                else if (this.US[j] < this.U_Lower[j])
                {
                    num3 -= 0x10;
                }
                if (this.BS[j] > this.B_Upper[j])
                {
                    num3 += 4;
                }
                else if (this.BS[j] < this.B_Lower[j])
                {
                    num3 -= 4;
                }
                if (this.LS[j] > this.L_Upper[j])
                {
                    num3++;
                }
                else if (this.LS[j] < this.L_Lower[j])
                {
                    num3--;
                }
                if (ds.Open[j] > ds.Close[j])
                {
                    num3 = -num3;
                }
                base[j] = num3;
            }
        }

        public override void CalculatePartialValue()
        {
            if (this.ds.Open.PartialValue > this.ds.Close.PartialValue)
            {
                this.US.PartialValue = this.ds.High.PartialValue - this.ds.Open.PartialValue;
                this.BS.PartialValue = this.ds.Open.PartialValue - this.ds.Close.PartialValue;
                this.LS.PartialValue = this.ds.Close.PartialValue - this.ds.Low.PartialValue;
            }
            else
            {
                this.US.PartialValue = this.ds.High.PartialValue - this.ds.Close.PartialValue;
                this.BS.PartialValue = this.ds.Close.PartialValue - this.ds.Open.PartialValue;
                this.LS.PartialValue = this.ds.Open.PartialValue - this.ds.Low.PartialValue;
            }
            int num = 0x2a;
            this.US.CalculatePartialValue();
            if (this.US.PartialValue > this.U_Upper.PartialValue)
            {
                num += 0x10;
            }
            else if (this.US.PartialValue < this.U_Lower.PartialValue)
            {
                num -= 0x10;
            }
            this.BS.CalculatePartialValue();
            if (this.BS.PartialValue > this.B_Upper.PartialValue)
            {
                num += 4;
            }
            else if (this.BS.PartialValue < this.B_Lower.PartialValue)
            {
                num -= 4;
            }
            this.LS.CalculatePartialValue();
            if (this.LS.PartialValue > this.L_Upper.PartialValue)
            {
                num++;
            }
            else if (this.LS.PartialValue < this.L_Lower.PartialValue)
            {
                num--;
            }
            if (this.ds.Open.PartialValue > this.ds.Close.PartialValue)
            {
                num = -num;
            }
            base.PartialValue = num;
        }

        public static CandleCode Series(Bars ds)
        {
            DataSeries series;
            string key = "CandleCode()";
            if (ds.Cache.ContainsKey(key))
            {
                return (CandleCode) ds.Cache[key];
            }
            ds.Cache[key] = series = new CandleCode(ds, key);
            return (CandleCode) series;
        }
    }
}

