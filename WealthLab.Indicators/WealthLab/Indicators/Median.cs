namespace WealthLab.Indicators
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class Median : DataSeries
    {
        private static Class50 class50_0 = new Class50();
        private DataSeries dataSeries_1;
        private int int_1;

        public Median(DataSeries source, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = (period - 1) + source.FirstValidValue;
            List<double> list = new List<double>();
            for (int i = period - 1; i < source.Count; i++)
            {
                int num;
                list.Clear();
                for (int j = 0; j < period; j++)
                {
                    list.Add(source[i - j]);
                }
                list.Sort(class50_0);
                if ((period % 2) == 1)
                {
                    num = period / 2;
                    base[i] = list[num];
                }
                else
                {
                    num = (period / 2) - 1;
                    base[i] = (list[num] + list[num + 1]) / 2.0;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if ((this.dataSeries_1.PartialValue != double.NaN) && (this.dataSeries_1.Count >= (this.int_1 - 1)))
            {
                List<double> list = new List<double> {
                    this.dataSeries_1.PartialValue
                };
                for (int i = 0; i < (this.int_1 - 1); i++)
                {
                    list.Add(this.dataSeries_1[(this.dataSeries_1.Count - 1) - i]);
                }
                list.Sort(class50_0);
                if ((this.int_1 % 2) == 1)
                {
                    int num2 = this.int_1 / 2;
                    base.PartialValue = list[num2];
                }
                else
                {
                    int num3 = (this.int_1 / 2) - 1;
                    base.PartialValue = (list[num3] + list[num3 + 1]) / 2.0;
                }
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static Median Series(DataSeries source, int period)
        {
            string key = string.Concat(new object[] { "Median(", source.Description, ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (Median) source.Cache[key];
            }
            Median median = new Median(source, period, key);
            source.Cache[key] = median;
            return median;
        }

        public static double Value(int int_2, DataSeries source, int period)
        {
            if (int_2 < (period - 1))
            {
                return 0.0;
            }
            List<double> list = new List<double>();
            for (int i = 0; i < period; i++)
            {
                list.Add(source[int_2 - i]);
            }
            list.Sort(class50_0);
            if ((period % 2) == 1)
            {
                int num = period / 2;
                return list[num];
            }
            int num3 = (period / 2) - 1;
            return ((list[num3] + list[num3 + 1]) / 2.0);
        }

        internal class Class50 : IComparer<double>
        {
            public int Compare(double double_0, double double_1)
            {
                return double_0.CompareTo(double_1);
            }
        }
    }
}

