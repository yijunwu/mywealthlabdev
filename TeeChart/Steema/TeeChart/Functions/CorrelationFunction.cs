namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class CorrelationFunction : BaseTrend
    {
        private void AddPoint(double x, double y)
        {
            if (base.Series.yMandatory)
            {
                base.Series.Add(x, y);
            }
            else
            {
                base.Series.Add(y, x);
            }
        }

        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            return base.Coefficient(s, firstIndex, lastIndex);
        }

        protected override void CalculatePeriod(Series source, double tmpX, int firstIndex, int lastIndex)
        {
            double num = this.Calculate(source, firstIndex, lastIndex);
            if (base.Series.AllowSinglePoint)
            {
                base.Series.Add(num);
            }
            else
            {
                this.AddPoint(source.notMandatory.Minimum, num);
                this.AddPoint(source.notMandatory.Maximum, num);
            }
        }

        public override string Description()
        {
            return Texts.FunctionCorrelation;
        }
    }
}

