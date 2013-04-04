namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;

    public class CustomGauge : Series
    {
        public static Color[] BlackPalette = new Color[] { 
            Utils.FromArgb(40, 40, 40), Utils.FromArgb(50, 50, 50), Utils.FromArgb(100, 100, 100), Utils.FromArgb(15, 15, 15), Utils.FromArgb(100, 100, 100), Utils.FromArgb(15, 15, 15), Utils.FromArgb(100, 40, 40), Utils.EmptyColor, Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100), Utils.EmptyColor, Utils.FromArgb(30, 200, 30), Utils.FromArgb(200, 0x73, 60), Utils.FromArgb(200, 0x73, 60), Utils.FromArgb(200, 0x73, 60), Utils.FromArgb(150, 50, 10), 
            Utils.FromArgb(130, 130, 130), Utils.FromArgb(130, 130, 130), Utils.EmptyColor, Utils.FromArgb(30, 30, 30), Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100)
         };
        public static Color[] BluesPalette = new Color[] { 
            Utils.FromArgb(0, 0, 0xfe), Utils.FromArgb(170, 0xce, 0xff), Utils.FromArgb(0x1a, 0x3a, 0x68), Utils.FromArgb(0x93, 0xac, 0xd1), Utils.FromArgb(0xcf, 220, 0xe9), Utils.FromArgb(0x93, 0xac, 0xd1), Utils.FromArgb(0x13, 0xf7, 0x1c), Utils.EmptyColor, Utils.FromArgb(0xcf, 220, 0xe9), Utils.FromArgb(0, 0, 0xfe), Utils.EmptyColor, Utils.FromArgb(100, 40, 40), Utils.FromArgb(0xab, 0x8b, 0x48), Utils.FromArgb(0xfd, 190, 0x5b), Utils.EmptyColor, Utils.FromArgb(0xff, 0xd5, 0x8e), 
            Utils.FromArgb(4, 0x55, 0x83), Utils.FromArgb(9, 0x5c, 0x8a), Utils.EmptyColor, Utils.FromArgb(0x16, 0x42, 0x70), Utils.FromArgb(0xcf, 220, 0xe9), Utils.FromArgb(0xcf, 220, 0xe9), Utils.FromArgb(100, 100, 100), Utils.FromArgb(100, 100, 100)
         };
        private ChartBrush faceBrush;
        protected double fgreenLineEndValue;
        protected double fgreenLineStartValue;
        protected double fmaximum;
        protected double fminimum;
        private TFrame frame;
        protected double fredLineEndValue;
        protected double fredLineStartValue;
        private Color[] gaugeColorPalette;
        private GaugeSeriesPointer greenLine;
        private GaugeSeriesPointer hand;
        private bool horizontal;
        protected Rectangle INewRectangle;
        protected Rectangle IOrigRectangle;
        protected double IRange;
        private int minorTickDistance;
        private GaugeSeriesPointer minorTicks;
        private GaugeSeriesPointer redLine;
        private GaugeSeriesPointer ticks;

        public event GaugesChangeHandler ValueChanged;

        public CustomGauge() : this(null)
        {
        }

        public CustomGauge(Chart c) : base(c)
        {
            this.horizontal = true;
        }

        protected override void AddSampleValues(int numValues)
        {
            Random random = new Random();
            double maximum = this.Maximum;
            if (maximum == double.PositiveInfinity)
            {
                maximum = 1000.0;
            }
            this.Value = this.Minimum + ((maximum - this.Minimum) * random.NextDouble());
        }

        protected override void Dispose(bool disposing)
        {
            if (this.Axis != null)
            {
                this.SetDefaultAxis();
            }
            base.Dispose(disposing);
        }

        protected internal override void DoBeforeDrawChart()
        {
            this.SetAxisOnce();
            base.DoBeforeDrawChart();
        }

        public override void Draw()
        {
            if (this.Axis != null)
            {
                this.SetValues();
                this.DrawFrame();
                Graphics3D g = this.PrepareGraphics(null, this.FaceBrush);
                this.DrawFace(g);
                g = this.PrepareGraphics(this.Axis.AxisPen, null);
                this.DrawAxis(g);
                g = this.PrepareGraphics(null, null);
                this.DrawHand(g);
            }
        }

        protected virtual void DrawAxis(Graphics3D g)
        {
        }

        protected virtual void DrawAxisMinorTick(Graphics3D g, Point Inner, Point Outer)
        {
            Point innerPlus = new Point(Inner.X + Utils.Round((float) (this.MinorTicks.HorizSize / 2)), Inner.Y);
            Point innerMinus = new Point(Inner.X - Utils.Round((float) (this.MinorTicks.HorizSize / 2)), Inner.Y);
            Point outerPlus = new Point(Inner.X + Utils.Round((float) (this.MinorTicks.HorizSize / 2)), Inner.Y + this.MinorTicks.VertSize);
            Point outerMinus = new Point(Inner.X - Utils.Round((float) (this.MinorTicks.HorizSize / 2)), Inner.Y + this.MinorTicks.VertSize);
            this.MinorTicks.Draw(g, Inner, innerPlus, innerMinus, Outer, outerPlus, outerMinus);
        }

        protected virtual void DrawAxisTick(Graphics3D g, Point Inner, Point InnerPlus, Point InnerMinus, Point Outer, Point OuterPlus, Point OuterMinus)
        {
            this.Ticks.Draw(g, Inner, InnerPlus, InnerMinus, Outer, OuterPlus, OuterMinus);
        }

        protected virtual void DrawColorLines(Graphics3D g)
        {
        }

        protected virtual void DrawFace(Graphics3D g)
        {
        }

        protected virtual void DrawFrame()
        {
            if (this.Frame.Visible)
            {
                this.Frame.Draw(this.IOrigRectangle);
            }
        }

        protected virtual void DrawHand(Graphics3D g)
        {
        }

        protected virtual Steema.TeeChart.Axis GetAxis()
        {
            return null;
        }

        public static Color GetGaugePaletteColor(int Index, Color[] GaugeColorPalette)
        {
            while (Index > (GaugeColorPalette.Length - 1))
            {
                Index -= GaugeColorPalette.Length;
            }
            return GaugeColorPalette[Index];
        }

        protected virtual bool IsDefaultAxis()
        {
            bool visible = this.Axis != null;
            if (visible)
            {
                visible = this.Axis.Labels.Visible;
            }
            if (visible)
            {
                visible = this.Axis.Labels.Font.Name == "Verdana";
            }
            if (visible)
            {
                visible = this.Axis.Labels.Font.Size == 8;
            }
            if (visible)
            {
                visible = !this.Axis.Labels.Font.Bold;
            }
            if (visible)
            {
                visible = !this.Axis.Labels.Font.Italic;
            }
            if (visible)
            {
                visible = (this.Axis.Labels.Font.Color == Color.FromArgb(0xff, 0, 0, 0)) || (this.Axis.Labels.Font.Color == Color.Black);
            }
            if (visible)
            {
                visible = !this.Axis.Labels.Font.Strikeout;
            }
            if (visible)
            {
                visible = !this.Axis.Labels.Font.Underline;
            }
            if (visible)
            {
                visible = this.Axis.Title.Visible;
            }
            if (visible)
            {
                visible = this.Axis.Title.Font.Name == "Verdana";
            }
            if (visible)
            {
                visible = Utils.IsNullOrEmpty(this.Axis.Title.Caption);
            }
            if (visible)
            {
                visible = this.Axis.Title.Font.Size == 8;
            }
            if (visible)
            {
                visible = !this.Axis.Title.Font.Bold;
            }
            if (visible)
            {
                visible = !this.Axis.Title.Font.Italic;
            }
            if (visible)
            {
                visible = (this.Axis.Title.Font.Color == Color.FromArgb(0xff, 0, 0, 0)) || (this.Axis.Title.Font.Color == Color.Black);
            }
            if (visible)
            {
                visible = !this.Axis.Title.Font.Strikeout;
            }
            if (visible)
            {
                visible = !this.Axis.Title.Font.Underline;
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.Color == Color.FromArgb(0xff, 0x40, 0x40, 0x40);
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.Width == 2;
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.Visible;
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.DashCap == DashCap.Flat;
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.DashPattern == null;
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.EndCap == LineCap.Flat;
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.Style == DashStyle.Solid;
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.Transparency == 0;
            }
            return visible;
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        protected virtual Graphics3D PrepareGraphics(ChartPen pen, ChartBrush brush)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            if (pen != null)
            {
                graphicsd.Pen = pen;
            }
            if (brush != null)
            {
                graphicsd.Brush = brush;
            }
            return graphicsd;
        }

        protected virtual void SetAxisOnce()
        {
            if (this.IsDefaultAxis())
            {
                this.Axis.AxisPen.Visible = true;
                this.Axis.Labels.Visible = true;
                this.Axis.Labels.Font.Color = GetGaugePaletteColor(20, this.GaugeColorPalette);
                this.Axis.Title.Visible = true;
                this.Axis.Title.Caption = "Axis Title %";
                this.Axis.Title.Font.Color = GetGaugePaletteColor(0x15, this.GaugeColorPalette);
                this.Axis.Labels.Font.Name = "Arial";
                this.Axis.Labels.Font.Size = 12;
                this.Axis.Labels.Font.Bold = true;
            }
        }

        private void SetDefaultAxis()
        {
            this.Axis.AxisPen.Visible = true;
            this.Axis.Labels.Visible = true;
            this.Axis.Labels.Font.Color = Color.Black;
            this.Axis.Title.Font.Color = Color.Black;
            this.Axis.Title.Visible = true;
            this.Axis.Labels.Font.Name = "Verdana";
            this.Axis.Title.Caption = "";
            this.Axis.Labels.Font.Size = 8;
            this.Axis.Labels.Font.Bold = false;
            this.Axis.Ticks.Length = 4;
            this.Axis.MinorTicks.Length = 2;
        }

        protected virtual void SetValue(double value)
        {
            if (this.Value != value)
            {
                base.mandatory[0] = value;
                this.OnValueChanged(new EventArgs());
                this.Invalidate();
            }
        }

        protected virtual void SetValues()
        {
            this.IRange = this.Maximum - this.Minimum;
            if (this.Frame.Visible)
            {
                this.INewRectangle.Inflate(-this.Frame.CalcWidth(this.INewRectangle), -this.Frame.CalcWidth(this.INewRectangle));
            }
        }

        [Description("Gets the LinearGauges's Axis characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Axis Axis
        {
            get
            {
                return this.GetAxis();
            }
        }

        [Description("ChartBrush characteristics of the CustomGauge's face."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush FaceBrush
        {
            get
            {
                if (this.faceBrush == null)
                {
                    this.faceBrush = new ChartBrush(base.chart);
                    this.faceBrush.Visible = true;
                    this.faceBrush.Solid = true;
                    this.faceBrush.Transparency = 0;
                    this.faceBrush.Gradient.Visible = true;
                    this.faceBrush.Gradient.StartColor = GetGaugePaletteColor(3, this.GaugeColorPalette);
                    this.faceBrush.Gradient.MiddleColor = GetGaugePaletteColor(4, this.GaugeColorPalette);
                    this.faceBrush.Gradient.EndColor = GetGaugePaletteColor(5, this.GaugeColorPalette);
                    this.faceBrush.Gradient.Style.Visible = true;
                    this.faceBrush.Gradient.Style.Direction = PathGradientMode.Radial;
                    this.faceBrush.Gradient.Style.CenterXOffset = Utils.Round((double) (this.INewRectangle.Width * 0.2));
                    this.faceBrush.Gradient.Style.CenterYOffset = Utils.Round((double) (this.INewRectangle.Height * -0.2));
                }
                return this.faceBrush;
            }
            set
            {
                this.faceBrush = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The Frame is the object which surrounds the LinearGauge and which consists of three configurable Bands: Inner, Middle and Outer.")]
        public TFrame Frame
        {
            get
            {
                if (this.frame == null)
                {
                    this.frame = new TFrame(base.chart, this.GaugeColorPalette, false);
                }
                return this.frame;
            }
            set
            {
                this.frame = value;
            }
        }

        [Description("Gets and sets the CircularGauge's color palette.")]
        public Color[] GaugeColorPalette
        {
            get
            {
                if ((this.gaugeColorPalette == null) || (this.gaugeColorPalette.Length == 0))
                {
                    this.gaugeColorPalette = Graphics3D.ColorPalette;
                }
                return this.gaugeColorPalette;
            }
            set
            {
                this.gaugeColorPalette = value;
                this.Frame.GaugeColorPalette = this.gaugeColorPalette;
                this.redLine = null;
                this.greenLine = null;
                this.hand = null;
                this.faceBrush = null;
                this.Invalidate();
            }
        }

        [Description("The 'greenline' object is the curved shape with a gradient which is used to signal a low or minimum value."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeSeriesPointer GreenLine
        {
            get
            {
                if (this.greenLine == null)
                {
                    this.greenLine = new GaugeSeriesPointer(base.chart, this);
                    this.greenLine.Style = GaugePointerStyles.ColorLine;
                    this.greenLine.VertSize = 5;
                    this.greenLine.Brush.Visible = true;
                    this.greenLine.Brush.Color = Color.Black;
                    this.greenLine.Gradient.Visible = true;
                    if (this.Horizontal)
                    {
                        this.greenLine.Gradient.Direction = LinearGradientMode.Horizontal;
                        this.greenLine.Gradient.Sigma = true;
                        this.greenLine.Gradient.SigmaFocus = 0f;
                        this.greenLine.Gradient.SigmaScale = 1f;
                        this.greenLine.Gradient.StartColor = GetGaugePaletteColor(9, this.GaugeColorPalette);
                        this.greenLine.Gradient.MiddleColor = GetGaugePaletteColor(10, this.GaugeColorPalette);
                        this.greenLine.Gradient.EndColor = GetGaugePaletteColor(11, this.GaugeColorPalette);
                    }
                    else
                    {
                        this.greenLine.Gradient.Direction = LinearGradientMode.Vertical;
                        this.greenLine.Gradient.Sigma = true;
                        this.greenLine.Gradient.SigmaFocus = 0f;
                        this.greenLine.Gradient.SigmaScale = 1f;
                        this.greenLine.Gradient.StartColor = GetGaugePaletteColor(11, this.GaugeColorPalette);
                        this.greenLine.Gradient.MiddleColor = GetGaugePaletteColor(10, this.GaugeColorPalette);
                        this.greenLine.Gradient.EndColor = GetGaugePaletteColor(9, this.GaugeColorPalette);
                    }
                }
                return this.greenLine;
            }
            set
            {
                this.greenLine = value;
            }
        }

        [DefaultValue(70), Description("Gets and sets the value at which the 'greenline' marker ends.")]
        public double GreenLineEndValue
        {
            get
            {
                return Math.Min(this.fgreenLineEndValue, this.Maximum);
            }
            set
            {
                if (this.fgreenLineEndValue != value)
                {
                    this.fgreenLineEndValue = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("Gets and sets the value at which the 'greenline' marker starts.")]
        public double GreenLineStartValue
        {
            get
            {
                return Math.Max(this.fgreenLineStartValue, this.Minimum);
            }
            set
            {
                if (this.fgreenLineStartValue != value)
                {
                    this.fgreenLineStartValue = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets and sets the Hand (or needle) object characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeSeriesPointer Hand
        {
            get
            {
                if (this.hand == null)
                {
                    this.hand = new GaugeSeriesPointer(base.chart, this);
                    this.hand.Visible = true;
                    this.hand.Style = GaugePointerStyles.Hand;
                    this.hand.Brush.Visible = true;
                    this.hand.Brush.Color = Color.Red;
                    this.hand.Brush.Transparency = 70;
                    this.hand.Pen.Color = GetGaugePaletteColor(12, this.GaugeColorPalette);
                }
                return this.hand;
            }
            set
            {
                this.hand = value;
            }
        }

        [Description("Gets and sets whether the LinearGauge should be displayed horizontally or vertically."), DefaultValue(true)]
        public bool Horizontal
        {
            get
            {
                return this.horizontal;
            }
            set
            {
                base.SetBooleanProperty(ref this.horizontal, value);
                this.SetAxisOnce();
            }
        }

        [Description("Gets or sets the maximum value for the gauge.")]
        public double Maximum
        {
            get
            {
                return this.fmaximum;
            }
            set
            {
                if (this.fmaximum != value)
                {
                    this.fmaximum = value;
                    this.Value = Math.Min(this.Maximum, this.Value);
                    this.Invalidate();
                }
            }
        }

        [Description("Gets or sets the minimum value for the gauge.")]
        public double Minimum
        {
            get
            {
                return this.fminimum;
            }
            set
            {
                if (this.fminimum != value)
                {
                    this.fminimum = value;
                    this.Value = Math.Max(this.Minimum, this.Value);
                    this.Invalidate();
                }
            }
        }

        [Description("The minor tick offset."), DefaultValue(0)]
        public int MinorTickDistance
        {
            get
            {
                return this.minorTickDistance;
            }
            set
            {
                if (this.minorTickDistance != value)
                {
                    this.minorTickDistance = value;
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gets and sets the characteristics of the Axis' minor tick marks.")]
        public GaugeSeriesPointer MinorTicks
        {
            get
            {
                if (this.minorTicks == null)
                {
                    this.minorTicks = new GaugeSeriesPointer(base.chart, this);
                    this.minorTicks.Style = GaugePointerStyles.MinorTick;
                    this.minorTicks.Color = Color.Transparent;
                    this.minorTicks.VertSize = 1;
                    this.minorTicks.HorizSize = 1;
                    this.minorTicks.Pen.Color = GetGaugePaletteColor(0x17, this.GaugeColorPalette);
                }
                return this.minorTicks;
            }
            set
            {
                this.minorTicks = value;
            }
        }

        [Description("The 'redline' object is the curved shape with a gradient which is used to signal a high or maximum value."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeSeriesPointer RedLine
        {
            get
            {
                if (this.redLine == null)
                {
                    this.redLine = new GaugeSeriesPointer(base.chart, this);
                    this.redLine.Style = GaugePointerStyles.ColorLine;
                    this.redLine.VertSize = 5;
                    this.redLine.Brush.Visible = true;
                    this.redLine.Brush.Color = Color.Black;
                    this.redLine.Gradient.Visible = true;
                    if (this.Horizontal)
                    {
                        this.redLine.Gradient.Direction = LinearGradientMode.Horizontal;
                        this.redLine.Gradient.Sigma = true;
                        this.redLine.Gradient.SigmaFocus = 0f;
                        this.redLine.Gradient.SigmaScale = 1f;
                        this.redLine.Gradient.StartColor = GetGaugePaletteColor(6, this.GaugeColorPalette);
                        this.redLine.Gradient.MiddleColor = GetGaugePaletteColor(7, this.GaugeColorPalette);
                        this.redLine.Gradient.EndColor = GetGaugePaletteColor(8, this.GaugeColorPalette);
                    }
                    else
                    {
                        this.redLine.Gradient.Direction = LinearGradientMode.Vertical;
                        this.redLine.Gradient.Sigma = true;
                        this.redLine.Gradient.SigmaFocus = 0f;
                        this.redLine.Gradient.SigmaScale = 1f;
                        this.redLine.Gradient.StartColor = GetGaugePaletteColor(8, this.GaugeColorPalette);
                        this.redLine.Gradient.MiddleColor = GetGaugePaletteColor(7, this.GaugeColorPalette);
                        this.redLine.Gradient.EndColor = GetGaugePaletteColor(6, this.GaugeColorPalette);
                    }
                }
                return this.redLine;
            }
            set
            {
                this.redLine = value;
            }
        }

        [DefaultValue(100), Description("Gets and sets the value at which the 'redline' marker ends.")]
        public double RedLineEndValue
        {
            get
            {
                return Math.Min(this.fredLineEndValue, this.Maximum);
            }
            set
            {
                if (this.fredLineEndValue != value)
                {
                    this.fredLineEndValue = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(80), Description("Gets and sets the value at which the 'redline' marker starts.")]
        public double RedLineStartValue
        {
            get
            {
                return Math.Max(this.fredLineStartValue, this.Minimum);
            }
            set
            {
                if (this.fredLineStartValue != value)
                {
                    this.fredLineStartValue = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets and sets the characteristics of the Axis' major tick marks."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeSeriesPointer Ticks
        {
            get
            {
                if (this.ticks == null)
                {
                    this.ticks = new GaugeSeriesPointer(base.chart, this);
                    this.ticks.Style = GaugePointerStyles.Tick;
                    this.ticks.Color = Color.Transparent;
                    this.ticks.VertSize = 20;
                    this.ticks.Pen.Color = GetGaugePaletteColor(0x16, this.GaugeColorPalette);
                }
                return this.ticks;
            }
            set
            {
                this.ticks = value;
            }
        }

        [Description("Gets or sets the position of gauge arrow line.")]
        public double Value
        {
            get
            {
                if (base.Count == 0)
                {
                    base.Add(0);
                }
                return Math.Min(base.mandatory[0], this.Maximum);
            }
            set
            {
                this.SetValue(value);
            }
        }
    }
}

