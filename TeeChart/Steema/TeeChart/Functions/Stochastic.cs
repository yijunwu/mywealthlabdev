namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class Stochastic : Moving
    {
        private double[] dens;
        private double[] nums;

        public Stochastic() : this(null)
        {
        }

        public Stochastic(Chart c) : base(c)
        {
            base.SingleSource = true;
            base.HideSourceList = true;
        }

        public override void AddPoints(Array source)
        {
            this.nums = new double[((Series) source.GetValue(0)).Count];
            this.dens = new double[((Series) source.GetValue(0)).Count];
            base.AddPoints(source);
        }

        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            double num = 0.0;
            ValueList yValueList = s.GetYValueList(Texts.ValuesLow);
            ValueList list2 = s.GetYValueList(Texts.ValuesHigh);
            double num2 = yValueList[firstIndex];
            double num3 = list2[firstIndex];
            for (int i = firstIndex; i <= lastIndex; i++)
            {
                if (yValueList[i] < num2)
                {
                    num2 = yValueList[i];
                }
                if (list2[i] > num3)
                {
                    num3 = list2[i];
                }
            }
            ValueList list3 = base.ValueList(s);
            this.nums[lastIndex] = list3[lastIndex] - num2;
            this.dens[lastIndex] = num3 - num2;
            if (num3 != num2)
            {
                num = 100.0 * (this.nums[lastIndex] / this.dens[lastIndex]);
            }
            return num;
        }

        public override string Description()
        {
            return Texts.FunctionStochastic;
        }
    }
}

