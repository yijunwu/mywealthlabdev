namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using System;

    public class ExpTrendFunction : BaseTrend
    {
        public ExpTrendFunction() : this(null)
        {
        }

        public ExpTrendFunction(Chart c) : base(c)
        {
            base.iTrendStyle = TrendStyles.Exponential;
        }

        public override string Description()
        {
            return Texts.FunctionExpTrend;
        }
    }
}

