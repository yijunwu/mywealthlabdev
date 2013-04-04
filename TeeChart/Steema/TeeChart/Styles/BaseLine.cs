namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;

    [Serializable]
    public abstract class BaseLine : Series
    {
        private ChartPen pLinePen;
        private TreatNullsStyle treatnulls;

        protected BaseLine() : this(null)
        {
        }

        protected BaseLine(Chart c) : base(c)
        {
        }

        public override void AssignFormat(Series source)
        {
            base.AssignFormat(source);
            if (source is BaseLine)
            {
                this.treatnulls = (source as BaseLine).TreatNulls;
            }
        }

        protected internal double CalcMinMaxValue(bool isMin)
        {
            bool flag = true;
            double d = 0.0;
            double num2 = 0.0;
            for (int i = 0; i < base.Count; i++)
            {
                if (!base.IsNull(i) || (this.TreatNulls == TreatNullsStyle.Ignore))
                {
                    num2 = base.mandatory[i];
                    if (flag)
                    {
                        d = num2;
                        flag = false;
                    }
                    else if (isMin)
                    {
                        if (double.IsNegativeInfinity(d))
                        {
                            d = num2;
                        }
                        else
                        {
                            d = Math.Min(d, num2);
                        }
                    }
                    else if (double.IsPositiveInfinity(d))
                    {
                        d = num2;
                    }
                    else
                    {
                        d = Math.Max(d, num2);
                    }
                }
            }
            return d;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            this.LinePen.Chart = base.chart;
        }

        [Category("Appearance"), Description("Determines pen to draw the line connecting all points."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen LinePen
        {
            get
            {
                if (this.pLinePen == null)
                {
                    this.pLinePen = new ChartPen(Utils.EmptyColor);
                }
                return this.pLinePen;
            }
            set
            {
                this.pLinePen = value;
            }
        }

        [DefaultValue(0), Description("Defines how null points are treated.")]
        public TreatNullsStyle TreatNulls
        {
            get
            {
                return this.treatnulls;
            }
            set
            {
                if (this.treatnulls != value)
                {
                    this.treatnulls = value;
                    base.Repaint();
                }
            }
        }
    }
}

