namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using System;

    public class MedianFunction : CustomSorted
    {
        public MedianFunction() : this(null)
        {
        }

        public MedianFunction(Chart c) : base(c)
        {
        }

        protected internal override double CalcResult()
        {
            int index = (base.iCount + 1) / 2;
            double num = base.tmp[index];
            if (Utils.Odd(base.iCount))
            {
                num = (num + base.tmp[index - 1]) * 0.5;
            }
            return num;
        }

        public override string Description()
        {
            return Texts.FunctionMedian;
        }
    }
}

