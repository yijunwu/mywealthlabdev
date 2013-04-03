namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using WealthLab;
    using WealthLab.Indicators;

    public class RSI14 : IndexDefinition
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
                    if (num4 >= RSI.Series(bars2.Close, 14).FirstValidValue)
                    {
                        num++;
                        open += RSI.Series(bars2.Close, 14)[num4];
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
                return "An indicator that returns an average of the Relative Strength Index (RSI) of a 14 period for the symbols in the selected DataSet.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "RSI14";
            }
        }

        public override string Prefix
        {
            get
            {
                return "RSI14";
            }
        }
    }
}

