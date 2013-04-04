namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class TFrame : TeeBase
    {
        private bool circled;
        private Color[] gaugeColorPalette;
        private ChartBrush innerBand;
        private ChartBrush middleBand;
        private ChartBrush outerBand;
        private ChartBrush tmpBrush;
        private bool visible;
        private int width;

        public TFrame() : this(null, null)
        {
        }

        public TFrame(Chart c, Color[] gColorPalette) : base(c)
        {
            this.width = 10;
            this.visible = true;
            this.tmpBrush = new ChartBrush(c, Color.Transparent, true);
            this.gaugeColorPalette = gColorPalette;
            this.circled = true;
        }

        public TFrame(Chart c, Color[] gColorPalette, bool circle) : this(c, gColorPalette)
        {
            this.circled = circle;
        }

        public int CalcWidth(Rectangle Rect)
        {
            double num = Math.Min(Rect.Width, Rect.Height);
            num /= 100.0;
            return Utils.Round((double) (num * this.Width));
        }

        public void Draw(Rectangle Rect)
        {
            Graphics3D graphicsd = this.PrepareGraphics(this.OuterBand);
            double num = ((double) this.CalcWidth(Rect)) / 10.0;
            if (this.circled)
            {
                graphicsd.Ellipse(Rect);
            }
            else
            {
                graphicsd.Rectangle(Rect);
            }
            graphicsd = this.PrepareGraphics(this.MiddleBand);
            Rect.Inflate(-Utils.Round((double) (num * 1.0)), -Utils.Round((double) (num * 1.0)));
            if (this.circled)
            {
                graphicsd.Ellipse(Rect);
            }
            else
            {
                graphicsd.Rectangle(Rect);
            }
            graphicsd = this.PrepareGraphics(this.InnerBand);
            Rect.Inflate(-Utils.Round((double) (num * 7.0)), -Utils.Round((double) (num * 7.0)));
            if (this.circled)
            {
                graphicsd.Ellipse(Rect);
            }
            else
            {
                graphicsd.Rectangle(Rect);
            }
            graphicsd = this.PrepareGraphics(this.tmpBrush);
            Rect.Inflate(-Utils.Round((double) (num * 2.0)), -Utils.Round((double) (num * 2.0)));
            if (this.circled)
            {
                graphicsd.Ellipse(Rect);
            }
            else
            {
                graphicsd.Rectangle(Rect);
            }
        }

        private Graphics3D PrepareGraphics(ChartBrush brush)
        {
            Graphics3D graphicsd = base.Chart.Graphics3D;
            graphicsd.Brush = brush;
            return graphicsd;
        }

        [Description("Toggles the drawing of a circular or rectangular frame.")]
        public bool Circled
        {
            get
            {
                return this.circled;
            }
            set
            {
                this.circled = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Gets and sets the CircularGauge's color palette.")]
        public Color[] GaugeColorPalette
        {
            get
            {
                return this.gaugeColorPalette;
            }
            set
            {
                this.gaugeColorPalette = value;
                this.outerBand = null;
                this.innerBand = null;
                this.middleBand = null;
            }
        }

        [Description("Gets and sets the InnerBand's ChartBrush characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush InnerBand
        {
            get
            {
                if (this.innerBand == null)
                {
                    this.innerBand = new ChartBrush(base.chart, CustomGauge.GetGaugePaletteColor(2, this.GaugeColorPalette), true);
                }
                return this.innerBand;
            }
            set
            {
                this.innerBand = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gets and sets the MiddleBand's ChartBrush characteristics.")]
        public ChartBrush MiddleBand
        {
            get
            {
                if (this.middleBand == null)
                {
                    this.middleBand = new ChartBrush(base.chart, CustomGauge.GetGaugePaletteColor(1, this.GaugeColorPalette), true);
                }
                return this.middleBand;
            }
            set
            {
                this.middleBand = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gets and sets the OuterBand's ChartBrush characteristics.")]
        public ChartBrush OuterBand
        {
            get
            {
                if (this.outerBand == null)
                {
                    this.outerBand = new ChartBrush(base.chart, CustomGauge.GetGaugePaletteColor(0, this.GaugeColorPalette), true);
                }
                return this.outerBand;
            }
            set
            {
                this.outerBand = value;
            }
        }

        [DefaultValue(true), Description("Gets sets the visibility of the TFrame object.")]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;
            }
        }

        [Description("Gets and sets the width of the TFrame object as a percentage of its bounding rectangle."), DefaultValue(10)]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }
}

