namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class AdvanceDeclineLine : IndexDefinition
    {
        public override void SetIndexValues(List<Bars> bars, List<int> barNumbers, DateTime dt, Bars indexToDate)
        {
            if (bars.Count != 0)
            {
                double num = 0.0;
                for (int i = 0; i < bars.Count; i++)
                {
                    Bars bars2 = bars[i];
                    int num3 = barNumbers[i];
                    if (bars2.Close[num3] > bars2.Close[num3 - 1])
                    {
                        num++;
                    }
                    else if (bars2.Close[num3] < bars2.Close[num3 - 1])
                    {
                        num--;
                    }
                }
                double open = indexToDate.Open[indexToDate.Count - 1] + num;
                double high = indexToDate.High[indexToDate.Count - 1] + num;
                double num6 = indexToDate.Low[indexToDate.Count - 1] + num;
                double close = indexToDate.Close[indexToDate.Count - 1] + num;
                double volume = indexToDate.Volume[indexToDate.Count - 1] + num;
                indexToDate.Add(dt, open, high, num6, close, volume);
            }
        }

        public override string Description
        {
            get
            {
                return "Creates the Advance/Decline line, which is the cumulated sum of Advancing Issues minus Declining Issues. The Advance/Decline line is one of the most widely used breadth indicators in technical analysis.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Advance Decline Line";
            }
        }

        public override string Prefix
        {
            get
            {
                return "AD";
            }
        }
    }
}

