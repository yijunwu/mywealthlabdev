namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;

    public class TableLegend : Shape
    {
        private ChartFont font;
        private bool fontseriescolor;
        private bool otherside;
        private LegendSymbol symbol;

        public TableLegend() : this(null)
        {
        }

        public TableLegend(Chart c) : base(c)
        {
        }

        private int CalcSymbolHeight()
        {
            return this.symbol.Width;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.symbol != null)
            {
                this.symbol.Chart = c;
            }
        }

        public ChartFont Font
        {
            get
            {
                if (this.font == null)
                {
                    this.font = new ChartFont(base.Chart);
                }
                return this.font;
            }
            set
            {
                this.font = value;
            }
        }

        public bool FontSeriesColor
        {
            get
            {
                return this.fontseriescolor;
            }
            set
            {
                if (value != this.fontseriescolor)
                {
                    this.fontseriescolor = value;
                    this.Invalidate();
                }
            }
        }

        public bool OtherSide
        {
            get
            {
                return this.otherside;
            }
            set
            {
                if (value != this.otherside)
                {
                    this.otherside = value;
                    this.Invalidate();
                }
            }
        }

        public LegendSymbol Symbol
        {
            get
            {
                if (this.symbol == null)
                {
                    this.symbol = new LegendSymbol(base.chart.Legend);
                }
                return this.symbol;
            }
            set
            {
                this.symbol = value;
            }
        }
    }
}

