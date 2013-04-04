namespace Steema.TeeChart
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class DepthAxis : Axis
    {
        public DepthAxis(bool horiz, bool isOtherSide, Chart c) : base(horiz, isOtherSide, c)
        {
            base.IsDepthAxis = true;
            base.bVisible = false;
        }

        protected override AxisLabelStyle InternalCalcLabelStyle()
        {
            AxisLabelStyle text = AxisLabelStyle.Text;
            foreach (Series series in base.chart.series)
            {
                if (series.bActive && (series.HasZValues || (series.MinZValue() != series.MaxZValue())))
                {
                    text = AxisLabelStyle.Value;
                }
            }
            return text;
        }

        protected override void SetInverted(bool Value)
        {
            base.SetInverted(Value);
            if (this == base.Chart.Axes.Depth)
            {
                base.Chart.Axes.DepthTop.inverted = base.Inverted;
            }
            else
            {
                base.Chart.Axes.Depth.inverted = base.Inverted;
            }
        }

        [Description("Show/hide Axis."), DefaultValue(false), Category("Axis")]
        public bool Visible
        {
            get
            {
                return base.bVisible;
            }
            set
            {
                base.Visible = value;
            }
        }
    }
}

