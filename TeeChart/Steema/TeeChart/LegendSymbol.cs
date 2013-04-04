namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class LegendSymbol : TeeBase
    {
        internal bool continuous;
        private bool defaultPen;
        private Legend ILegend;
        private ChartPen iPen;
        internal LegendSymbolPosition position;
        private Steema.TeeChart.Drawing.Shadow shadow;
        private bool squared;
        private bool visible;
        private int width;
        private LegendSymbolSize widthUnits;

        public event SymbolDrawEventHandler OnSymbolDraw;

        public LegendSymbol(Legend legend) : base(legend.chart)
        {
            this.defaultPen = true;
            this.width = 20;
            this.visible = true;
            this.ILegend = legend;
        }

        internal int CalcWidth(int value)
        {
            if (!this.visible)
            {
                return 0;
            }
            if (this.squared)
            {
                return (this.ILegend.CalcItemHeight() - 5);
            }
            if (this.widthUnits == LegendSymbolSize.Percent)
            {
                return Utils.Round((double) ((this.width * value) * 0.01));
            }
            return this.width;
        }

        protected internal virtual void DoOnSymbolDraw(object sender, Series series, int valueIndex, Rectangle R)
        {
            if (this.OnSymbolDraw != null)
            {
                SymbolDrawEventArgs e = new SymbolDrawEventArgs(series, valueIndex, R);
                this.OnSymbolDraw(sender, e);
            }
        }

        internal void Draw(Rectangle R)
        {
            this.Draw(R, base.Chart.Graphics3D);
        }

        internal void Draw(Rectangle R, Graphics3D tmp)
        {
            ChartBrush brush = new ChartBrush();
            brush = tmp.Brush;
            if ((this.shadow.Size != new Size(0, 0)) && this.shadow.Visible)
            {
                this.shadow.Draw(tmp, R);
            }
            tmp.Pen = this.Pen;
            tmp.Brush = brush;
            if (this.Pen.Visible)
            {
                tmp.Rectangle(R);
            }
        }

        protected virtual bool ShouldSerializePen()
        {
            return !this.DefaultPen;
        }

        [Description("Adjoins the different legend rectangles when true."), DefaultValue(false)]
        public bool Continous
        {
            get
            {
                return this.continuous;
            }
            set
            {
                base.SetBooleanProperty(ref this.continuous, value);
            }
        }

        [Description("Uses series pen properties to draw a border around the coloured box legend symbol, when true. "), DefaultValue(true)]
        public bool DefaultPen
        {
            get
            {
                return this.defaultPen;
            }
            set
            {
                base.SetBooleanProperty(ref this.defaultPen, value);
            }
        }

        [Description("Pen used to draw a border around the color box legend symbols."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (this.iPen == null)
                {
                    this.iPen = new ChartPen(this.ILegend.Chart);
                }
                return this.iPen;
            }
        }

        [DefaultValue(0), Description("Sets the position of the Legend color rectangles.")]
        public LegendSymbolPosition Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Sets a shadow around the Legend symbols."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Shadow Shadow
        {
            get
            {
                if (this.shadow == null)
                {
                    this.shadow = new Steema.TeeChart.Drawing.Shadow(this.ILegend.Chart, 0);
                }
                return this.shadow;
            }
        }

        [DefaultValue(false), Description("When true, the legend symbol will be resized to square shaped.")]
        public bool Squared
        {
            get
            {
                return this.squared;
            }
            set
            {
                base.SetBooleanProperty(ref this.squared, value);
            }
        }

        [DefaultValue(true), Description("Shows or hides Legend symbols.")]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                base.SetBooleanProperty(ref this.visible, value);
            }
        }

        [Description("Defines the width of the color rectangles."), DefaultValue(20)]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                base.SetIntegerProperty(ref this.width, value);
            }
        }

        [DefaultValue(0), Description("Defines the Width units for the width of Symbol.")]
        public LegendSymbolSize WidthUnits
        {
            get
            {
                return this.widthUnits;
            }
            set
            {
                if (this.widthUnits != value)
                {
                    this.widthUnits = value;
                    this.Invalidate();
                }
            }
        }
    }
}

