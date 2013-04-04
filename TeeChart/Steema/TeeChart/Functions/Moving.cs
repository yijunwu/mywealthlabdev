namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class Moving : Function
    {
        public Moving() : this(null)
        {
        }

        public Moving(Chart c) : base(c)
        {
            base.dPeriod = 1.0;
            base.SingleSource = true;
        }

        protected override void DoCalculation(Series source, ValueList notMandatorySource)
        {
            int index = 1;
            if (base.PeriodStyle == PeriodStyles.NumPoints)
            {
                index = Utils.Round(base.dPeriod);
            }
            else if (base.PeriodStyle == PeriodStyles.Range)
            {
                index = source.XValues.IndexOf(source.XValues.Minimum + base.dPeriod);
                if (index < 1)
                {
                    index = 1;
                }
            }
            for (int i = index - 1; i < source.Count; i += index)
            {
                base.AddFunctionXY(source.yMandatory, notMandatorySource[i], this.Calculate(source, (i - index) + 1, i));
            }
        }
    }
}

