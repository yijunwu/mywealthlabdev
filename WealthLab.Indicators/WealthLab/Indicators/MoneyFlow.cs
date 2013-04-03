namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class MoneyFlow : DataSeries
    {
        private Bars bars_1;

        public MoneyFlow(Bars bars, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            for (int i = 0; i < bars.Count; i++)
            {
                base[i] = (((bars.High[i] + bars.Low[i]) + bars.Close[i]) / 3.0) * bars.Volume[i];
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.bars_1.Count != 0) && (this.bars_1.Close.PartialValue != double.NaN)) && ((this.bars_1.Low.PartialValue != double.NaN) && (this.bars_1.High.PartialValue != double.NaN)))
            {
                base.PartialValue = (((this.bars_1.High.PartialValue + this.bars_1.Low.PartialValue) + this.bars_1.Close.PartialValue) / 3.0) * this.bars_1.Volume.PartialValue;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static MoneyFlow Series(Bars bars)
        {
            string key = "MoneyFlow()";
            if (bars.Cache.ContainsKey(key))
            {
                return (MoneyFlow) bars.Cache[key];
            }
            MoneyFlow flow = new MoneyFlow(bars, key);
            bars.Cache[key] = flow;
            return flow;
        }
    }
}

