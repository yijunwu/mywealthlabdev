namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class AdvancingDeclining : IndexDefinition
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
                    if (bars2.Close[num3] > bars2.Close[num3 - 1])
                    {
                        open++;
                    }
                    else if (bars2.Close[num3] < bars2.Close[num3 - 1])
                    {
                        open--;
                    }
                }
                indexToDate.Add(dt, open, open, open, open, open);
            }
        }

        public override string Description
        {
            get
            {
                return "Produces the Advancing Issues minus Declining Issues breadth indicator. This is the basis for several more complex breadth measurements, including the McClellan Oscillator.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Advancing-Declining";
            }
        }

        public override string Prefix
        {
            get
            {
                return "A-D";
            }
        }
    }
}

