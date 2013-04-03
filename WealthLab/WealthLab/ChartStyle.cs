namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public abstract class ChartStyle
    {
        private WealthLab.Bars bars_0;
        private ChartRenderer chartRenderer_0;

        protected ChartStyle()
        {
        }

        public int ConvertBarToX(int int_0)
        {
            return this.chartRenderer_0.ConvertBarToX(int_0);
        }

        public int ConvertXToBar(int int_0)
        {
            return this.chartRenderer_0.ConvertXToBar(int_0);
        }

        public Color GetBarColor(int int_0)
        {
            return this.chartRenderer_0.GetBarColor(this.bars_0, int_0);
        }

        public int GetBarWidth(int int_0)
        {
            return this.chartRenderer_0.GetBarWidth(int_0);
        }

        public abstract void Initialize();
        protected internal abstract void InitializeBarWidths();
        internal void method_0(PlottedSymbol plottedSymbol_0, ChartPane chartPane_0, Graphics graphics_0)
        {
            WealthLab.Bars bars = this.bars_0;
            this.bars_0 = plottedSymbol_0.Bars;
            this.chartRenderer_0.method_11(plottedSymbol_0, chartPane_0);
            try
            {
                this.RenderBars(graphics_0);
            }
            finally
            {
                this.bars_0 = bars;
                this.chartRenderer_0.method_12();
            }
        }

        public abstract void RenderBars(Graphics graphics_0);
        public void SetBarWidth(int int_0, int value)
        {
            this.chartRenderer_0.SetBarWidth(int_0, value);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.chartRenderer_0.BackgroundColor;
            }
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars_0;
            }
            internal set
            {
                if (this.bars_0 != value)
                {
                    this.bars_0 = value;
                    if (this.bars_0 != null)
                    {
                        this.Initialize();
                    }
                }
            }
        }

        public int BarSpacing
        {
            get
            {
                return this.chartRenderer_0.BarSpacing;
            }
        }

        public int ChartHeight
        {
            get
            {
                return this.chartRenderer_0.Height;
            }
        }

        public int ChartWidth
        {
            get
            {
                return this.chartRenderer_0.Width;
            }
        }

        public abstract string FriendlyName { get; }

        public Color GhostBarColor
        {
            get
            {
                if (this.Bars.Close.PartialValue > this.Bars.Open.PartialValue)
                {
                    return this.chartRenderer_0.UpBarColor;
                }
                return this.chartRenderer_0.DownBarColor;
            }
        }

        public int GhostBarXPosition
        {
            get
            {
                return ((this.chartRenderer_0.ChartWidth - this.chartRenderer_0.MarginRightWidth) - (this.chartRenderer_0.RightPaddingBars * this.chartRenderer_0.BarSpacing));
            }
        }

        public abstract Bitmap Glyph { get; }

        public bool HorizontalGridlines
        {
            get
            {
                return this.chartRenderer_0.HorizontalGridines;
            }
            set
            {
                this.chartRenderer_0.HorizontalGridines = value;
            }
        }

        public int LeftEdgeBar
        {
            get
            {
                return this.chartRenderer_0.LeftEdgeBar;
            }
        }

        public IList<ChartPane> Panes
        {
            get
            {
                return this.chartRenderer_0.Panes;
            }
        }

        public ChartPane PricePane
        {
            get
            {
                return this.chartRenderer_0.PricePane;
            }
        }

        internal ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer_0;
            }
            set
            {
                this.chartRenderer_0 = value;
            }
        }

        public int RightEdgeBar
        {
            get
            {
                return this.chartRenderer_0.RightEdgeBar;
            }
        }

        public bool ShouldDrawGhostBar
        {
            get
            {
                bool flag = ((this.Bars != null) && !double.IsNaN(this.Bars.Open.PartialValue)) && (this.RightEdgeBar == (this.Bars.Count - 1));
                if (this.chartRenderer_0.RightPaddingBars == 0)
                {
                    return flag;
                }
                if (!flag)
                {
                    return false;
                }
                return (this.chartRenderer_0.ScrollOffset == 0);
            }
        }

        public bool VerticalGridlines
        {
            get
            {
                return this.chartRenderer_0.VerticalGridlines;
            }
            set
            {
                this.chartRenderer_0.VerticalGridlines = value;
            }
        }

        public ChartPane VolumePane
        {
            get
            {
                return this.chartRenderer_0.VolumePane;
            }
        }
    }
}

