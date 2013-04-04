namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using System;

    public class ModeFunction : CustomSorted
    {
        public ModeFunction() : this(null)
        {
        }

        public ModeFunction(Chart c) : base(c)
        {
        }

        protected internal override double CalcResult()
        {
            double num = 0.0;
            if (base.tmp.GetUpperBound(0) > -1)
            {
                num = base.tmp[0];
            }
            else
            {
                num = 0.0;
            }
            if (base.tmp.GetUpperBound(0) > 0)
            {
                int num2 = 1;
                int num3 = 0;
                for (int i = 1; i <= base.tmp.GetUpperBound(0); i++)
                {
                    if (base.tmp[i - 1] == base.tmp[i])
                    {
                        num2++;
                    }
                    else
                    {
                        if (num2 > num3)
                        {
                            num = base.tmp[i - 1];
                            num3 = num2;
                        }
                        num2 = 1;
                    }
                }
                if (num2 > num3)
                {
                    num = base.tmp[base.tmp.GetUpperBound(0)];
                }
            }
            return num;
        }

        public override string Description()
        {
            return Texts.FunctionMode;
        }
    }
}

