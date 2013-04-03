namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class MAMA : DataSeries
    {
        private DataSeries dataSeries_1;
        private double double_1;
        private double double_2;

        public MAMA(DataSeries source, double FastLimit, double SlowLimit, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.double_1 = FastLimit;
            this.double_2 = SlowLimit;
            double[] numArray = new double[source.Count];
            double[] numArray2 = new double[source.Count];
            double[] numArray3 = new double[source.Count];
            double[] numArray4 = new double[source.Count];
            double num = 0.0;
            double num8 = 0.0;
            double num9 = 0.0;
            double num10 = 0.0;
            double num11 = 0.0;
            double num12 = 0.0;
            double num14 = 0.0;
            double num17 = 0.0;
            double num18 = 0.0;
            float num19 = (float) FastLimit;
            float num20 = (float) SlowLimit;
            string str = string.Concat(new object[] { "FAMA(", source.Description, ", ", num19, ", ", num20, ")" });
            FAMA fama = new FAMA(source, (double) num19, (double) num20, str);
            if (!source.Cache.ContainsKey(str))
            {
                source.Cache[str] = fama;
            }
            for (int i = 0; i < 5; i++)
            {
                numArray[i] = 0.0;
                numArray2[i] = 0.0;
                numArray3[i] = 0.0;
            }
            for (int j = 6; j < base.Count; j++)
            {
                numArray[j] = ((((4.0 * source[j]) + (3.0 * source[j - 1])) + (2.0 * source[j - 2])) + source[j - 3]) / 10.0;
                numArray2[j] = ((((0.0962 * numArray[j]) + (0.5769 * numArray[j - 2])) - (0.5769 * numArray[j - 4])) - (0.0962 * numArray[j - 6])) * ((0.075 * num8) + 0.54);
                numArray3[j] = ((((0.0962 * numArray2[j]) + (0.5769 * numArray2[j - 2])) - (0.5769 * numArray2[j - 4])) - (0.0962 * numArray2[j - 6])) * ((0.075 * num8) + 0.54);
                numArray4[j] = numArray2[j - 3];
                double num2 = ((((0.0962 * numArray4[j]) + (0.5769 * numArray4[j - 2])) - (0.5769 * numArray4[j - 4])) - (0.0962 * numArray4[j - 6])) * ((0.075 * num8) + 0.54);
                double num3 = ((((0.0962 * numArray3[j]) + (0.5769 * numArray3[j - 2])) - (0.5769 * numArray3[j - 4])) - (0.0962 * numArray3[j - 6])) * ((0.075 * num8) + 0.54);
                double num4 = numArray4[j] - num3;
                double num5 = numArray3[j] + num2;
                num4 = (0.2 * num4) + (0.8 * num9);
                num5 = (0.2 * num5) + (0.8 * num10);
                double num6 = (num4 * num9) + (num5 * num10);
                double num7 = (num4 * num10) - (num5 * num9);
                num6 = (0.2 * num6) + (0.8 * num11);
                num7 = (0.2 * num7) + (0.8 * num12);
                if ((num7 != 0.0) && (num6 != 0.0))
                {
                    num = 360.0 / Math.Atan(num7 / num6);
                }
                if (num > (1.5 * num8))
                {
                    num = 1.5 * num8;
                }
                if (num < (0.67 * num8))
                {
                    num = 0.67 * num8;
                }
                if (num < 6.0)
                {
                    num = 6.0;
                }
                if (num > 50.0)
                {
                    num = 50.0;
                }
                num = (0.2 * num) + (0.8 * num8);
                double num13 = (0.33 * num) + (0.67 * num14);
                if (numArray4[j] != 0.0)
                {
                    num17 = 57.295779513082323 * Math.Atan(numArray3[j] / numArray4[j]);
                }
                double num15 = num18 - num17;
                if (num15 < 1.0)
                {
                    num15 = 1.0;
                }
                double num16 = ((double) num19) / num15;
                if (num16 < num20)
                {
                    num16 = num20;
                }
                if (num16 > num19)
                {
                    num16 = num19;
                }
                base[j] = (num16 * source[j]) + ((1.0 - num16) * base[j - 1]);
                fama[j] = ((0.5 * num16) * base[j]) + ((1.0 - (0.5 * num16)) * fama[j - 1]);
                num8 = num;
                num9 = num4;
                num10 = num5;
                num11 = num6;
                num12 = num7;
                num14 = num13;
                num18 = num17;
            }
            numArray = new double[0];
            numArray2 = new double[0];
            numArray3 = new double[0];
            numArray4 = new double[0];
            base.FirstValidValue = 40;
            fama.FirstValidValue = 40;
        }

        public static MAMA Series(DataSeries source, double fastLimit, double slowLimit)
        {
            DataSeries series;
            string key = string.Concat(new object[] { "MAMA(", source.Description, ", ", fastLimit, ", ", slowLimit, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (MAMA) source.Cache[key];
            }
            source.Cache[key] = series = new MAMA(source, fastLimit, slowLimit, key);
            return (MAMA) series;
        }
    }
}

