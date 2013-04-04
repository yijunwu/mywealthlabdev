namespace TASCIndicators
{
    using System;
    using WealthLab;
    using WealthLab.Indicators;

    public class RevEngRSI : DataSeries
    {
        private DataSeries ADC;
        private DataSeries AUC;
        private DataSeries DC;
        private DataSeries ds;
        private int period;
        private double RSIVal;
        private DataSeries UC;

        public RevEngRSI(DataSeries ds, int period, double RSIVal, string description) : base(ds, description)
        {
            this.ds = ds;
            this.period = period;
            this.RSIVal = RSIVal;
            this.UC = new DataSeries(ds, "UpChg");
            this.DC = new DataSeries(ds, "DnChg");
            this.UC.FirstValidValue = ds.FirstValidValue + 1;
            this.DC.FirstValidValue = ds.FirstValidValue + 1;
            for (int i = 1; i < base.Count; i++)
            {
                if (ds[i] > ds[i - 1])
                {
                    this.UC[i] = ds[i] - ds[i - 1];
                }
                else
                {
                    this.DC[i] = ds[i - 1] - ds[i];
                }
            }
            this.AUC = EMA.Series(this.UC, (2 * period) - 1, EMACalculation.Modern);
            this.ADC = EMA.Series(this.DC, (2 * period) - 1, EMACalculation.Modern);
            base.FirstValidValue = (ds.FirstValidValue + (2 * period)) - 1;
            if (base.FirstValidValue > ds.Count)
            {
                base.FirstValidValue = ds.Count;
            }
            for (int j = base.FirstValidValue; j < ds.Count; j++)
            {
                double num3 = (period - 1) * (((this.ADC[j] * RSIVal) / (100.0 - RSIVal)) - this.AUC[j]);
                if (num3 < 0.0)
                {
                    num3 *= (100.0 - RSIVal) / RSIVal;
                }
                base[j] = ds[j] + num3;
            }
        }

        public override void CalculatePartialValue()
        {
            int count = this.ds.Count;
            if (count < base.FirstValidValue)
            {
                base.PartialValue = 0.0;
            }
            else
            {
                if (this.ds.PartialValue > this.ds[count - 1])
                {
                    this.UC.PartialValue = this.ds.PartialValue - this.ds[count - 1];
                    this.DC.PartialValue = 0.0;
                }
                else
                {
                    this.UC.PartialValue = 0.0;
                    this.DC.PartialValue = this.ds[count - 1] - this.ds.PartialValue;
                }
                this.AUC.CalculatePartialValue();
                this.ADC.CalculatePartialValue();
                double num2 = (this.period - 1) * (((this.ADC.PartialValue * this.RSIVal) / (100.0 - this.RSIVal)) - this.AUC.PartialValue);
                if (num2 < 0.0)
                {
                    num2 *= (100.0 - this.RSIVal) / this.RSIVal;
                }
                base.PartialValue = this.ds.PartialValue + num2;
            }
        }

        public static RevEngRSI Series(DataSeries ds, int period, double RSIVal)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "RevEngRSI(", ds.Description, ",", period, ",", RSIVal, ")" });
            if (ds.Cache.ContainsKey(key))
            {
                return (RevEngRSI) ds.Cache[key];
            }
            ds.Cache[key] = series = new RevEngRSI(ds, period, RSIVal, key);
            return (RevEngRSI) series;
        }
    }
}

