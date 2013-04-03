namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class DailyChange : IndexDefinition
    {
        public override void SetIndexValues(List<Bars> bars, List<int> barNumbers, DateTime dt, Bars indexToDate)
        {
            if (bars.Count != 0)
            {
                double open = 0.0;
                for (int i = 0; i < bars.Count; i++)
                {
                    Bars bars2 = bars[i];
                    int num3 = barNumbers[i];
                    open += ((bars2.Close[num3] - bars2.Close[num3 - 1]) / bars2.Close[num3 - 1]) + 1.0;
                }
                open /= (double) bars.Count;
                if (indexToDate.Count == 1)
                {
                    indexToDate.Open[0] = 100.0;
                    indexToDate.Close[0] = 100.0;
                    indexToDate.High[0] = 100.0;
                    indexToDate.Volume[0] = 100.0;
                    indexToDate.Low[0] = 100.0;
                }
                if (indexToDate.Close[indexToDate.Count - 1] != 0.0)
                {
                    open *= indexToDate.Close[indexToDate.Count - 1];
                }
                indexToDate.Add(dt, open, open, open, open, open);
            }
        }

        public override string Description
        {
            get
            {
                return "Creates a custom index with a equal dollar weighting scheme rebalanced daily. The Index starts at 100. For each bar, the IndexScript takes the average daily percentage change and multiplies it by the previous value of the Index. ";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Daily Change";
            }
        }

        public override string Prefix
        {
            get
            {
                return "IDX";
            }
        }
    }
}

