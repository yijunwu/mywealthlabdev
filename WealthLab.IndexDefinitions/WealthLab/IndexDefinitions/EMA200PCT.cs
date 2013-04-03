namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using WealthLab;
    using WealthLab.Indicators;

    public class EMA200PCT : IndexDefinition
    {
        public override void SetIndexValues(List<Bars> bars, List<int> barNumbers, DateTime dt, Bars indexToDate)
        {
            if (bars.Count != 0)
            {
                int num = 0;
                double open = 0.0;
                for (int i = 0; i < bars.Count; i++)
                {
                    Bars bars2 = bars[i];
                    int num4 = barNumbers[i];
                    if (num4 >= EMA.Series(bars2.Close, 200, EMACalculation.Modern).FirstValidValue)
                    {
                        num++;
                        if (bars2.Close[num4] > EMA.Series(bars2.Close, 200, 0)[num4])
                        {
                            open++;
                        }
                    }
                }
                if (num > 0)
                {
                    open /= (double) num;
                    open *= 100.0;
                    indexToDate.Add(dt, open, open, open, open, open);
                }
            }
        }

        public override string Description
        {
            get
            {
                return "An indicator that ranges from 0 to 100, and expresses the percentage of symbols in the selected DataSet that are above an Exponential Moving Average of a 200 period.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "EMA200PCT";
            }
        }

        public override string Prefix
        {
            get
            {
                return "EMA200PCT";
            }
        }
    }
}

