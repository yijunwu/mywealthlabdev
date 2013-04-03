namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using WealthLab;
    using WealthLab.Indicators;

    public class Above200SMA : IndexDefinition
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
                    if (num4 >= SMA.Series(bars2.Close, 200).FirstValidValue)
                    {
                        num++;
                        if (bars2.Close[num4] > SMA.Series(bars2.Close, 200)[num4])
                        {
                            open++;
                        }
                    }
                }
                if (num > 0)
                {
                    open /= (double) num;
                    indexToDate.Add(dt, open, open, open, open, open);
                }
            }
        }

        public override string Description
        {
            get
            {
                return "An indicator that ranges from 0 to 100, and expresses the percentage of the symbols in the selected DataSet that are above a Simple Moving Average of a 200 period.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Above 200 SMA";
            }
        }

        public override string Prefix
        {
            get
            {
                return "AB200";
            }
        }
    }
}

