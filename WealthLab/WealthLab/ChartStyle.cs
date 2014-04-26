namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public abstract class ChartStyle
    {
        private WealthLab.Bars bars;
        private ChartRenderer chartRenderer;

        protected ChartStyle()
        {
        }

        public int ConvertBarToX(int int_0)
        {
            return this.chartRenderer.ConvertBarToX(int_0);
        }

        public int ConvertXToBar(int int_0)
        {
            return this.chartRenderer.ConvertXToBar(int_0);
        }

        public Color GetBarColor(int int_0)
        {
            return this.chartRenderer.GetBarColor(this.bars, int_0);
        }

        public int GetBarWidth(int int_0)
        {
            return this.chartRenderer.GetBarWidth(int_0);
        }

        public abstract void Initialize();
        protected internal abstract void InitializeBarWidths();
        internal void method_0(PlottedSymbol plottedSymbol_0, ChartPane chartPane_0, Graphics graphics_0)
        {
            WealthLab.Bars bars = this.bars;
            this.bars = plottedSymbol_0.Bars;
            this.chartRenderer.backupAndSetColorsAndPane(plottedSymbol_0, chartPane_0);
            try
            {
                this.RenderBars(graphics_0);
            }
            finally
            {
                this.bars = bars;
                this.chartRenderer.restoreColorsAndPane();
            }
        }

        public abstract void RenderBars(Graphics graphics_0);
        public void SetBarWidth(int int_0, int value)
        {
            this.chartRenderer.SetBarWidth(int_0, value);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.chartRenderer.BackgroundColor;
            }
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.bars;
            }
            internal set
            {
                if (this.bars != value)
                {
                    this.bars = value;
                    if (this.bars != null)
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
                return this.chartRenderer.BarSpacing;
            }
        }

        public int ChartHeight
        {
            get
            {
                return this.chartRenderer.Height;
            }
        }

        public int ChartWidth
        {
            get
            {
                return this.chartRenderer.Width;
            }
        }

        public abstract string FriendlyName { get; }

        public Color GhostBarColor
        {
            get
            {
                if (this.Bars.Close.PartialValue > this.Bars.Open.PartialValue)
                {
                    return this.chartRenderer.UpBarColor;
                }
                return this.chartRenderer.DownBarColor;
            }
        }

        public int GhostBarXPosition
        {
            get
            {
                return ((this.chartRenderer.ChartWidth - this.chartRenderer.MarginRightWidth) - (this.chartRenderer.RightPaddingBars * this.chartRenderer.BarSpacing));
            }
        }

        public abstract Bitmap Glyph { get; }

        public bool HorizontalGridlines
        {
            get
            {
                return this.chartRenderer.HorizontalGridines;
            }
            set
            {
                this.chartRenderer.HorizontalGridines = value;
            }
        }

        public int LeftEdgeBar
        {
            get
            {
                return this.chartRenderer.LeftEdgeBar;
            }
        }

        public IList<ChartPane> Panes
        {
            get
            {
                return this.chartRenderer.Panes;
            }
        }

        public ChartPane PricePane
        {
            get
            {
                return this.chartRenderer.PricePane;
            }
        }

        internal ChartRenderer Renderer
        {
            get
            {
                return this.chartRenderer;
            }
            set
            {
                this.chartRenderer = value;
            }
        }

        public int RightEdgeBar
        {
            get
            {
                return this.chartRenderer.RightEdgeBar;
            }
        }

        public bool ShouldDrawGhostBar
        {
            get
            {
                bool flag = ((this.Bars != null) && !double.IsNaN(this.Bars.Open.PartialValue)) && (this.RightEdgeBar == (this.Bars.Count - 1));
                if (this.chartRenderer.RightPaddingBars == 0)
                {
                    return flag;
                }
                if (!flag)
                {
                    return false;
                }
                return (this.chartRenderer.ScrollOffset == 0);
            }
        }

        public bool VerticalGridlines
        {
            get
            {
                return this.chartRenderer.VerticalGridlines;
            }
            set
            {
                this.chartRenderer.VerticalGridlines = value;
            }
        }

        public ChartPane VolumePane
        {
            get
            {
                return this.chartRenderer.VolumePane;
            }
        }
    }
}

