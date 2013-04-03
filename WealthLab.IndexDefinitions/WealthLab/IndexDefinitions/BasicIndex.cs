namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class BasicIndex : IndexDefinition
    {
        public override void ClearUserInterface()
        {
        }

        public override void SetIndexValues(List<Bars> bars, List<int> barNumbers, DateTime dt, Bars indexToDate)
        {
            double open = 0.0;
            double high = 0.0;
            double num3 = 0.0;
            double close = 0.0;
            double volume = 0.0;
            if (bars.Count != 0)
            {
                for (int i = 0; i < bars.Count; i++)
                {
                    Bars bars2 = bars[i];
                    int num7 = barNumbers[i];
                    open += bars2.Open[num7];
                    high += bars2.High[num7];
                    num3 += bars2.Low[num7];
                    close += bars2.Close[num7];
                    volume += bars2.Volume[num7];
                }
                open /= (double) bars.Count;
                high /= (double) bars.Count;
                num3 /= (double) bars.Count;
                close /= (double) bars.Count;
                volume /= (double) bars.Count;
                indexToDate.Add(dt, open, high, num3, close, volume);
            }
        }

        public override string Description
        {
            get
            {
                return "A basic index that averages the values of the symbols provided. Warning: Index definitions that average only price may not be useful in backtesting due to stock splits, which are \"future events\" that affect historical prices.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Basic Index";
            }
        }

        public override string ParameterString
        {
            get
            {
                return "";
            }
        }

        public override string Prefix
        {
            get
            {
                return "I";
            }
        }
    }
}

