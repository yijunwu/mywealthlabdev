namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;

    [ToolboxBitmap(typeof(NumericGauge), "SeriesIcons.NumericGauge.bmp")]
    public class NumericGauge : CustomGauge
    {
        private DigitalFont digitalFontType;
        public static Color[] LCDPalette = new Color[] { 
            Utils.FromArgb(40, 40, 40), Utils.FromArgb(50, 50, 50), Utils.FromArgb(100, 100, 100), Utils.FromArgb(170, 170, 130), Utils.FromArgb(170, 170, 130), Utils.FromArgb(170, 170, 130), Utils.FromArgb(100, 40, 40), Utils.EmptyColor, Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100), Utils.EmptyColor, Utils.FromArgb(30, 200, 30), Utils.FromArgb(200, 0x73, 60), Utils.FromArgb(200, 0x73, 60), Utils.FromArgb(200, 0x73, 60), Utils.FromArgb(150, 50, 10), 
            Utils.FromArgb(130, 130, 130), Utils.FromArgb(130, 130, 130), Utils.EmptyColor, Utils.FromArgb(30, 30, 30), Utils.FromArgb(40, 40, 40), Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100)
         };
        public static Color[] LEDPalette = new Color[] { 
            Utils.FromArgb(5, 0x37, 0x7d), Utils.FromArgb(10, 120, 200), Utils.FromArgb(5, 90, 160), Utils.FromArgb(10, 10, 10), Utils.FromArgb(10, 10, 10), Utils.FromArgb(10, 10, 10), Utils.FromArgb(100, 40, 40), Utils.EmptyColor, Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100), Utils.EmptyColor, Utils.FromArgb(30, 200, 30), Utils.FromArgb(200, 0x73, 60), Utils.FromArgb(200, 0x73, 60), Utils.FromArgb(200, 0x73, 60), Utils.FromArgb(150, 50, 10), 
            Utils.FromArgb(130, 130, 130), Utils.FromArgb(130, 130, 130), Utils.EmptyColor, Utils.FromArgb(30, 30, 30), Utils.FromArgb(0xff, 30, 30), Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100)
         };
        private MarkersCollection markers;
        private Marker textMarker;
        private Marker unitsMarker;
        private Marker valueMarker;

        public NumericGauge() : this(null)
        {
        }

        public NumericGauge(Chart c) : base(c)
        {
            base.Add(0);
            base.GaugeColorPalette = LCDPalette;
            base.fmaximum = double.PositiveInfinity;
            this.digitalFontType = DigitalFont.Bar;
            this.Markers.Add(this.valueMarker = new Marker("", 0x24, ~AnnotationPositions.LeftTop, StringAlignment.Far, CustomGauge.GetGaugePaletteColor(20, base.GaugeColorPalette), CustomGauge.GetGaugePaletteColor(4, base.GaugeColorPalette)));
            this.Markers.Add(this.unitsMarker = new Marker("MHz", 0x12, AnnotationPositions.LeftBottom, StringAlignment.Center, CustomGauge.GetGaugePaletteColor(20, base.GaugeColorPalette), CustomGauge.GetGaugePaletteColor(4, base.GaugeColorPalette)));
            this.Markers.Add(this.textMarker = new Marker("FREQ", 11, AnnotationPositions.LeftTop, StringAlignment.Center, CustomGauge.GetGaugePaletteColor(4, base.GaugeColorPalette), CustomGauge.GetGaugePaletteColor(20, base.GaugeColorPalette)));
            base.FaceBrush.Gradient.Style.Visible = false;
        }

        private void CalcNewRectangle()
        {
            base.INewRectangle = Utils.FromLTRB(this.IOrigRectangle.Left, this.IOrigRectangle.Top, this.IOrigRectangle.Right, this.IOrigRectangle.Bottom);
        }

        private void CalcOrigRectangle()
        {
            base.IOrigRectangle = base.Chart.ChartRect;
            bool flag = true;
            int num = this.IOrigRectangle.Height / 2;
            int count = base.Chart.Series.Count;
            int index = base.Chart.Series.IndexOf(this);
            int num8 = this.IOrigRectangle.Height / count;
            foreach (Series series in base.Chart.Series)
            {
                if (!(series is CustomGauge))
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                int height = Math.Min(num, num8) - 3;
                int width = Utils.Round((float) ((this.IOrigRectangle.Width / 3) * 2));
                int x = this.IOrigRectangle.X + Utils.Round((float) (this.IOrigRectangle.Width / 6));
                int y = this.IOrigRectangle.Y;
                if (count == 1)
                {
                    y += num / 2;
                }
                else
                {
                    y += num8 * index;
                }
                base.IOrigRectangle = new Rectangle(x, y, width, height);
            }
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.LEDGauge);
        }

        protected override void Dispose(bool disposing)
        {
            foreach (Tool tool in this.Markers)
            {
                tool.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void DrawFace(Graphics3D g)
        {
            g.Rectangle(base.INewRectangle);
        }

        protected override void DrawHand(Graphics3D g)
        {
            foreach (Tool tool in this.Markers)
            {
                if (!tool.Active)
                {
                    continue;
                }
                Marker marker = tool as Marker;
                marker.IRectangle = base.INewRectangle;
                if (marker.UsePalette)
                {
                    if (marker == this.textMarker)
                    {
                        marker.Shape.Font.Color = CustomGauge.GetGaugePaletteColor(4, base.GaugeColorPalette);
                        marker.Shape.Color = CustomGauge.GetGaugePaletteColor(20, base.GaugeColorPalette);
                    }
                    else
                    {
                        marker.Shape.Font.Color = CustomGauge.GetGaugePaletteColor(20, base.GaugeColorPalette);
                        marker.Shape.Color = CustomGauge.GetGaugePaletteColor(4, base.GaugeColorPalette);
                    }
                }
                if (marker.Shape.Font.UsePrivateFont)
                {
                    switch (this.DigitalFontType)
                    {
                        case DigitalFont.Bar:
                            marker.Shape.Font.UsePrivateFont = true;
                            marker.Shape.Font.Name = "DS-Digital";
                            break;

                        case DigitalFont.Dot:
                            marker.Shape.Font.UsePrivateFont = true;
                            marker.Shape.Font.Name = "Elektra";
                            break;
                    }
                }
                this.valueMarker.Text = base.Value.ToString("N", CultureInfo.CurrentCulture.NumberFormat);
                marker.DrawText();
            }
        }

        protected override Axis GetAxis()
        {
            return base.GetHorizAxis;
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.unitsMarker.Active = false;
            this.textMarker.Active = false;
            this.valueMarker.Shape.Font.Size = 7;
            this.valueMarker.Centered = true;
            this.valueMarker.Shape.Shadow.Visible = false;
            this.valueMarker.Shape.Pen.Visible = false;
            this.valueMarker.Text = "360";
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if (value != null)
            {
                base.UseAxis = false;
                base.ShowInLegend = false;
                base.calcVisiblePoints = false;
                base.Frame.chart = value;
                this.Markers.SetParentChart(value);
            }
        }

        public override void SetSubGallery(int index)
        {
            if (index == 1)
            {
                this.DigitalFontType = DigitalFont.Dot;
                base.GaugeColorPalette = LEDPalette;
            }
        }

        protected override void SetValues()
        {
            this.CalcOrigRectangle();
            this.CalcNewRectangle();
            base.SetValues();
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryNumericGauge;
            }
        }

        [DefaultValue(typeof(DigitalFont), "Bar"), Description("The style of digital font.")]
        public DigitalFont DigitalFontType
        {
            get
            {
                return this.digitalFontType;
            }
            set
            {
                this.digitalFontType = value;
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("The collection of Numeric gauge markers for displaying text or numbers.")]
        public MarkersCollection Markers
        {
            get
            {
                if (this.markers == null)
                {
                    this.markers = new MarkersCollection(base.Chart);
                }
                return this.markers;
            }
            set
            {
                this.markers = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Marker TextMarker
        {
            get
            {
                return this.textMarker;
            }
            set
            {
                this.textMarker = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Marker UnitsMarker
        {
            get
            {
                return this.unitsMarker;
            }
            set
            {
                this.unitsMarker = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Marker ValueMarker
        {
            get
            {
                return this.valueMarker;
            }
            set
            {
                this.valueMarker = value;
            }
        }
    }
}

