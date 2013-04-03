namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class MACD : DataSeries
    {
        private DataSeries dataSeries_1;
        private double double_1;
        private double double_2;

        public MACD(DataSeries source, string description) : base(source, description)
        {
            if (source.Count != 0)
            {
                this.dataSeries_1 = source;
                base.FirstValidValue = 1 + source.FirstValidValue;
                this.double_1 = source[0];
                this.double_2 = this.double_1;
                for (int i = 1; i < source.Count; i++)
                {
                    double num = source[i] - this.double_1;
                    num *= 0.075;
                    this.double_1 += num;
                    num = source[i] - this.double_2;
                    num *= 0.15;
                    this.double_2 += num;
                    base[i] = this.double_2 - this.double_1;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.dataSeries_1.PartialValue != double.NaN) && (this.dataSeries_1.Count != 0))
            {
                double num = this.dataSeries_1.PartialValue - this.double_1;
                num *= 0.075;
                double num2 = this.double_1 + num;
                num = this.dataSeries_1.PartialValue - this.double_2;
                num *= 0.15;
                double num3 = this.double_2 + num;
                base.PartialValue = num3 - num2;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static MACD Series(DataSeries source)
        {
            string key = "MACD(" + source.Description + ")";
            if (source.Cache.ContainsKey(key))
            {
                return (MACD) source.Cache[key];
            }
            MACD macd = new MACD(source, key);
            source.Cache[key] = macd;
            return macd;
        }
    }
}

