namespace WealthLab.Indicators
{
    using System;
    using System.Globalization;
    using WealthLab;

    public class FIR : DataSeries
    {
        private static CultureInfo cultureInfo_0 = new CultureInfo("en-US");
        private DataSeries dataSeries_1;
        private double[] double_1;

        public FIR(DataSeries source, string weights, string description) : base(source, description)
        {
            double[] weightValues = smethod_0(weights);
            this.dataSeries_1 = source;
            this.double_1 = weightValues;
            base.FirstValidValue = (weightValues.Length - 1) + source.FirstValidValue;
            for (int i = 0; i < source.Count; i++)
            {
                base[i] = Value(i, source, weightValues);
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.dataSeries_1.Count >= (this.double_1.Length - 1)) && (this.dataSeries_1.PartialValue != double.NaN))
            {
                double num4 = this.dataSeries_1.PartialValue * this.double_1[this.double_1.Length - 1];
                double num = this.double_1[this.double_1.Length - 1];
                for (int i = 0; i < (this.double_1.Length - 1); i++)
                {
                    num += this.double_1[i];
                    int num3 = (i - this.double_1.Length) + 1;
                    num4 += this.dataSeries_1[this.dataSeries_1.Count + num3] * this.double_1[i];
                }
                base.PartialValue = num4 / num;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static FIR Series(DataSeries source, string weights)
        {
            string key = "FIR(" + source.Description + "," + weights + ")";
            if (source.Cache.ContainsKey(key))
            {
                return (FIR) source.Cache[key];
            }
            FIR fir = new FIR(source, weights, key);
            source.Cache[key] = fir;
            return fir;
        }

        private static double[] smethod_0(string string_1)
        {
            string[] strArray = string_1.Split(new char[] { ',' });
            double[] numArray = new double[strArray.Length];
            int index = 0;
            foreach (string str in strArray)
            {
                double num3;
                try
                {
                    num3 = double.Parse(str, cultureInfo_0);
                }
                catch
                {
                    num3 = 1.0;
                }
                numArray[index] = num3;
                index++;
            }
            return numArray;
        }

        public static double Value(int int_1, DataSeries source, string weights)
        {
            double[] weightValues = smethod_0(weights);
            return Value(int_1, source, weightValues);
        }

        public static double Value(int int_1, DataSeries source, params double[] weightValues)
        {
            if (int_1 < (weightValues.Length - 1))
            {
                return 0.0;
            }
            double num4 = 0.0;
            double num = 0.0;
            for (int i = 0; i < weightValues.Length; i++)
            {
                num += weightValues[i];
                int num3 = (i - weightValues.Length) + 1;
                num4 += source[int_1 + num3] * weightValues[i];
            }
            return (num4 / num);
        }
    }
}

