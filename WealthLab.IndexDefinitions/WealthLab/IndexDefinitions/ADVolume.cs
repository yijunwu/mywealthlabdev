namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class ADVolume : IndexDefinition
    {
        public override void SetIndexValues(List<Bars> bars, List<int> barNumbers, DateTime dt, Bars indexToDate)
        {
            if (bars.Count != 0)
            {
                double open = 0.0;
                double num2 = 0.0;
                for (int i = 0; i < bars.Count; i++)
                {
                    num2 = 0.0;
                    Bars bars2 = bars[i];
                    int num4 = barNumbers[i];
                    if (bars2.Close[num4] > bars2.Close[num4 - 1])
                    {
                        num2 = bars2.Volume[num4];
                    }
                    else if (bars2.Close[num4] < bars2.Close[num4 - 1])
                    {
                        num2 = -bars2.Volume[num4];
                    }
                    else
                    {
                        num2 = 0.0;
                    }
                    open += num2;
                }
                indexToDate.Add(dt, open, open, open, open, open);
            }
        }

        public override string Description
        {
            get
            {
                return "The total volume of symbols in the selected DataSet that rose in price minus the total volume of the symbols that declined in price.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "AD Volume";
            }
        }

        public override string Prefix
        {
            get
            {
                return "ADV";
            }
        }
    }
}

