namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class ToolSeries : Tool
    {
        protected Steema.TeeChart.Styles.Series iSeries;

        protected ToolSeries() : this((Chart) null)
        {
        }

        protected ToolSeries(Chart c) : base(c)
        {
        }

        protected ToolSeries(Steema.TeeChart.Styles.Series s) : base(s.chart)
        {
            this.iSeries = s;
        }

        protected override void Assign(Tool t)
        {
            base.Assign(t);
            ToolSeries series = t as ToolSeries;
            series.Series = this.Series;
        }

        protected virtual void SetSeries(Steema.TeeChart.Styles.Series value)
        {
            this.iSeries = value;
            this.Invalidate();
        }

        [Browsable(false)]
        public Axis GetHorizAxis
        {
            get
            {
                if (this.iSeries != null)
                {
                    return this.iSeries.GetHorizAxis;
                }
                return base.chart.Axes.Bottom;
            }
        }

        [Browsable(false)]
        public Axis GetVertAxis
        {
            get
            {
                if (this.iSeries != null)
                {
                    return this.iSeries.GetVertAxis;
                }
                return base.chart.Axes.Left;
            }
        }

        [Description("Sets Series with which Tools are associated."), DefaultValue((string) null)]
        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.iSeries;
            }
            set
            {
                this.SetSeries(value);
            }
        }
    }
}

