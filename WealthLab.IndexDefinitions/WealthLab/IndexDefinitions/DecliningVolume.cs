namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class DecliningVolume : IndexDefinition
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
                    if (bars2.Close[num3] <= bars2.Close[num3 - 1])
                    {
                        open += bars2.Volume[num3];
                    }
                }
                indexToDate.Add(dt, open, open, open, open, open);
            }
        }

        public override string Description
        {
            get
            {
                return "The total volume of the symbols in the selected DataSet that fell in price.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Declining Volume";
            }
        }

        public override string Prefix
        {
            get
            {
                return "DV";
            }
        }
    }
}

