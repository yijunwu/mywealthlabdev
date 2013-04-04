namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;

    public class VarianceFunction : Function
    {
        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            if (firstIndex == -1)
            {
                firstIndex = 0;
            }
            if (lastIndex == -1)
            {
                lastIndex = s.Count - 1;
            }
            int num = (lastIndex - firstIndex) + 1;
            if (num <= 0)
            {
                return 0.0;
            }
            double num2 = s.mandatory.Total / ((double) num);
            if (num != s.Count)
            {
                num2 = 0.0;
                for (int j = firstIndex; j <= lastIndex; j++)
                {
                    num2 += s.mandatory[j];
                }
                num2 /= (double) num;
            }
            double num4 = 0.0;
            for (int i = firstIndex; i <= lastIndex; i++)
            {
                num4 += (s.mandatory[i] - num2) * (s.mandatory[i] - num2);
            }
            return (num4 / ((double) num));
        }

        public override double CalculateMany(ArrayList sourceSeries, int valueIndex)
        {
            int count = sourceSeries.Count;
            if (count <= 0)
            {
                return 0.0;
            }
            double num2 = 0.0;
            for (int i = 0; i < count; i++)
            {
                num2 += (sourceSeries[i] as Series).mandatory[valueIndex];
            }
            num2 /= (double) count;
            double num4 = 0.0;
            for (int j = 0; j < count; j++)
            {
                num4 += ((sourceSeries[j] as Series).mandatory[valueIndex] - num2) * ((sourceSeries[j] as Series).mandatory[valueIndex] - num2);
            }
            return (num4 / ((double) count));
        }

        public override string Description()
        {
            return Texts.FunctionVariance;
        }
    }
}

