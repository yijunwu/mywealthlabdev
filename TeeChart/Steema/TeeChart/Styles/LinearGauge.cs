namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [ToolboxBitmap(typeof(LinearGauge), "SeriesIcons.LinearGauge.bmp")]
    public class LinearGauge : CustomGauge
    {
        private Rectangle IAxisRectangle;
        private double maxValue;
        private GaugeSeriesPointer maxValueIndicator;
        private bool useValueColorPalette;
        private ChartBrush valueAreaBrush;
        private Color[] valueColorPalette;

        public LinearGauge() : this(null)
        {
        }

        public LinearGauge(Chart c) : base(c)
        {
            base.Add(0);
            base.calcVisiblePoints = false;
            base.fmaximum = 100.0;
            base.fredLineStartValue = 80.0;
            base.fredLineEndValue = 100.0;
            base.fgreenLineStartValue = 0.0;
            base.fgreenLineEndValue = 40.0;
            this.useValueColorPalette = false;
            base.GaugeColorPalette = CustomGauge.BlackPalette;
            base.Hand.Pen.Visible = false;
        }

        private void CalcAxisRectangle()
        {
            int vertSize;
            int num2;
            int num3;
            int num4;
            if (base.Horizontal)
            {
                vertSize = base.Ticks.VertSize;
                num2 = Utils.Round((double) (this.INewRectangle.Width * 0.8));
                num3 = (this.INewRectangle.X + (this.INewRectangle.Width / 2)) - (num2 / 2);
                num4 = this.INewRectangle.Y + ((this.INewRectangle.Height - vertSize) / 2);
            }
            else
            {
                vertSize = Utils.Round((double) (this.INewRectangle.Height * 0.8));
                num2 = base.Ticks.VertSize;
                num3 = (this.INewRectangle.X + (this.INewRectangle.Width / 2)) - (num2 / 2);
                num4 = this.INewRectangle.Y + ((this.INewRectangle.Height - vertSize) / 2);
            }
            this.IAxisRectangle = new Rectangle(num3, num4, num2, vertSize);
        }

        private void CalcNewRectangle()
        {
            base.INewRectangle = Utils.FromLTRB(this.IOrigRectangle.Left, this.IOrigRectangle.Top, this.IOrigRectangle.Right, this.IOrigRectangle.Bottom);
        }

        private void CalcOrigRectangle()
        {
            int num;
            int num8;
            base.IOrigRectangle = base.Chart.ChartRect;
            bool flag = true;
            if (base.Horizontal)
            {
                num = this.IOrigRectangle.Height / 2;
            }
            else
            {
                num = this.IOrigRectangle.Width / 2;
            }
            int count = base.Chart.Series.Count;
            int index = base.Chart.Series.IndexOf(this);
            if (base.Horizontal)
            {
                num8 = this.IOrigRectangle.Height / count;
            }
            else
            {
                num8 = this.IOrigRectangle.Width / count;
            }
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
                int height;
                int width;
                if (base.Horizontal)
                {
                    height = Math.Min(num, num8) - 3;
                    width = this.IOrigRectangle.Width;
                }
                else
                {
                    width = Math.Min(num, num8) - 3;
                    height = this.IOrigRectangle.Height;
                }
                int x = this.IOrigRectangle.X;
                int y = this.IOrigRectangle.Y;
                if (count == 1)
                {
                    if (base.Horizontal)
                    {
                        y += num / 2;
                    }
                    else
                    {
                        x += num / 2;
                    }
                }
                else if (base.Horizontal)
                {
                    y += num8 * index;
                }
                else
                {
                    x += num8 * index;
                }
                base.IOrigRectangle = new Rectangle(x, y, width, height);
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (base.Axis != null)
            {
                Graphics3D g = this.PrepareGraphics(this.MaxValueIndicator.Pen, this.MaxValueIndicator.Brush);
                this.DrawMaxValueIndicator(g);
            }
        }

        protected override void DrawAxis(Graphics3D g)
        {
            int num8 = 0;
            if ((base.Axis != null) && base.Axis.Visible)
            {
                int width;
                base.Axis.Increment = base.IRange / 10.0;
                double increment = base.Axis.Increment;
                this.CalcAxisRectangle();
                g.Pen = base.Axis.AxisPen;
                g.Brush = this.ValueAreaBrush;
                if (base.Axis.AxisPen.Visible && (base.Axis.AxisPen.Width > 0))
                {
                    this.IAxisRectangle.Inflate(base.Axis.AxisPen.Width, base.Axis.AxisPen.Width);
                    this.IAxisRectangle.Offset(Utils.Round((float) (base.Axis.AxisPen.Width / 2)), Utils.Round((float) (base.Axis.AxisPen.Width / 2)));
                }
                g.Rectangle(this.IAxisRectangle);
                if (base.Axis.AxisPen.Visible && (base.Axis.AxisPen.Width > 0))
                {
                    this.IAxisRectangle.Inflate(-base.Axis.AxisPen.Width, -base.Axis.AxisPen.Width);
                    this.IAxisRectangle.Offset(-Utils.Round((float) (base.Axis.AxisPen.Width / 2)), -Utils.Round((float) (base.Axis.AxisPen.Width / 2)));
                }
                if (base.Horizontal)
                {
                    width = this.IAxisRectangle.Width;
                }
                else
                {
                    width = this.IAxisRectangle.Height;
                }
                if (base.Axis.Title.Visible && !Utils.IsNullOrEmpty(base.Axis.Title.Caption))
                {
                    int num11;
                    int num12;
                    g.Font = base.Axis.Title.Font;
                    float num9 = g.TextWidth(base.Axis.Title.Caption);
                    float fontHeight = g.FontHeight;
                    if (base.Horizontal)
                    {
                        num11 = Utils.Round((double) ((this.IAxisRectangle.Left + (width / 2)) - (num9 * 0.6)));
                        num12 = Utils.Round((double) (this.IAxisRectangle.Bottom + (fontHeight * 0.5)));
                    }
                    else
                    {
                        num11 = Utils.Round((double) (this.IAxisRectangle.Right + (fontHeight * 0.5)));
                        num12 = Utils.Round((double) ((this.IAxisRectangle.Top + (width / 2)) + (num9 * 0.6)));
                    }
                    if (base.Horizontal)
                    {
                        g.TextOut(num11, num12, base.Axis.Title.Caption);
                    }
                    else
                    {
                        g.RotateLabel(num11, num12, base.Axis.Title.Caption, 90.0);
                    }
                }
                if (base.Axis.Ticks.Visible || base.Axis.Labels.Visible)
                {
                    double minimum;
                    Point point;
                    Point point2;
                    double num7;
                    base.Axis.Ticks.Length = base.Ticks.VertSize;
                    base.Axis.Ticks.Assign(base.Ticks.Pen);
                    g.Font = base.Axis.Labels.Font;
                    g.Pen = base.Ticks.Pen;
                    int num1 = g.FontHeight;
                    if ((base.IRange != 0.0) && (increment != 0.0))
                    {
                        string str;
                        double num4;
                        if (base.Horizontal)
                        {
                            minimum = base.Minimum;
                        }
                        else
                        {
                            minimum = base.Maximum;
                        }
                        num7 = ((double) width) / base.IRange;
                        num7 *= increment;
                        if (base.Horizontal)
                        {
                            do
                            {
                                point = new Point(this.IAxisRectangle.X + Utils.Round((double) (num7 * num8)), this.IAxisRectangle.Y);
                                point2 = new Point(point.X, point.Y + this.IAxisRectangle.Height);
                                Point innerMinus = new Point(point.X - Utils.Round((double) (base.Ticks.HorizSize * 0.5)), point.Y);
                                Point innerPlus = new Point(point.X + Utils.Round((double) (base.Ticks.HorizSize * 0.5)), point.Y);
                                Point outerMinus = new Point(point2.X - Utils.Round((double) (base.Ticks.HorizSize * 0.5)), point2.Y);
                                Point outerPlus = new Point(point2.X + Utils.Round((double) (base.Ticks.HorizSize * 0.5)), point2.Y);
                                this.DrawAxisTick(g, point, innerPlus, innerMinus, point2, outerPlus, outerMinus);
                                if (base.Axis.Labels.Visible)
                                {
                                    str = Utils.FormatFloat(base.ValueFormat, minimum);
                                    num4 = g.TextWidth(str);
                                    g.TextOut(point.X - Utils.Round((double) (num4 * 0.6)), point.Y - g.FontHeight, str);
                                }
                                minimum += increment;
                                num8++;
                            }
                            while ((minimum <= base.Maximum) || (minimum < base.Maximum));
                        }
                        else
                        {
                            do
                            {
                                point = new Point(this.IAxisRectangle.Left, this.IAxisRectangle.Top + Utils.Round((double) (num7 * num8)));
                                point2 = new Point(point.X + this.IAxisRectangle.Width, point.Y);
                                Point point7 = new Point(point.X, point.Y - Utils.Round((double) (base.Ticks.HorizSize * 0.5)));
                                Point point8 = new Point(point.X, point.Y + Utils.Round((double) (base.Ticks.HorizSize * 0.5)));
                                Point point9 = new Point(point2.X, point2.Y - Utils.Round((double) (base.Ticks.HorizSize * 0.5)));
                                Point point10 = new Point(point2.X, point2.Y + Utils.Round((double) (base.Ticks.HorizSize * 0.5)));
                                this.DrawAxisTick(g, point, point8, point7, point2, point10, point9);
                                if (base.Axis.Labels.Visible)
                                {
                                    str = Utils.FormatFloat(base.ValueFormat, minimum);
                                    num4 = g.TextWidth(str);
                                    g.TextOut(point.X - Utils.Round((double) (num4 * 1.1)), point.Y - Utils.Round((double) (g.FontHeight * 0.4)), str);
                                }
                                minimum -= increment;
                                num8++;
                            }
                            while ((minimum >= base.Minimum) || (minimum > base.Minimum));
                        }
                    }
                    if (((base.IRange != 0.0) && base.Axis.MinorTicks.Visible) && (base.Axis.MinorTickCount > 0))
                    {
                        if (base.Horizontal)
                        {
                            base.Axis.MinorTicks.Length = base.MinorTicks.VertSize;
                        }
                        else
                        {
                            base.Axis.MinorTicks.Length = base.MinorTicks.HorizSize;
                        }
                        base.Axis.MinorTicks.Assign(base.MinorTicks.Pen);
                        g.Pen = base.MinorTicks.Pen;
                        if (increment != 0.0)
                        {
                            double num3 = increment / ((double) (base.Axis.MinorTickCount + 1));
                            minimum = base.Minimum;
                            num8 = 0;
                            num7 = ((double) width) / base.IRange;
                            double num13 = num7;
                            num13 *= num3;
                            num7 *= increment;
                            do
                            {
                                for (int i = 1; i <= base.Axis.MinorTickCount; i++)
                                {
                                    if (base.Horizontal)
                                    {
                                        point = new Point((this.IAxisRectangle.X + Utils.Round((double) (num7 * num8))) + Utils.Round((double) (num13 * i)), this.IAxisRectangle.Y + base.MinorTickDistance);
                                        point2 = new Point(point.X, point.Y + ((this.IAxisRectangle.Height - base.Axis.AxisPen.Width) / 2));
                                    }
                                    else
                                    {
                                        point = new Point(this.IAxisRectangle.Left + base.MinorTickDistance, (this.IAxisRectangle.Top + Utils.Round((double) (num7 * num8))) + Utils.Round((double) (num13 * i)));
                                        point2 = new Point(point.X + ((this.IAxisRectangle.Width - base.Axis.AxisPen.Width) / 2), point.Y);
                                    }
                                    this.DrawAxisMinorTick(g, point, point2);
                                }
                                num8++;
                                minimum += increment;
                            }
                            while (minimum <= (base.Maximum - increment));
                        }
                    }
                }
            }
            this.DrawColorLines(g);
        }

        protected override void DrawColorLines(Graphics3D g)
        {
            double num;
            double num2;
            double num3;
            if (base.RedLine.Visible)
            {
                if (base.Horizontal)
                {
                    num = ((double) this.IAxisRectangle.Width) / base.IRange;
                }
                else
                {
                    num = ((double) this.IAxisRectangle.Height) / base.IRange;
                }
                num2 = num * (base.RedLineStartValue - base.Minimum);
                num3 = num * (base.RedLineEndValue - base.Minimum);
                g = this.PrepareGraphics(base.RedLine.Pen, base.RedLine.Brush);
                if (base.Horizontal)
                {
                    g.Rectangle(Utils.FromLTRB(this.IAxisRectangle.Left + Utils.Round(num2), this.IAxisRectangle.Bottom + base.Axis.AxisPen.Width, this.IAxisRectangle.Left + Utils.Round(num3), (this.IAxisRectangle.Bottom + base.Axis.AxisPen.Width) + base.RedLine.VertSize));
                }
                else
                {
                    g.Rectangle(Utils.FromLTRB(this.IAxisRectangle.Right + base.Axis.AxisPen.Width, this.IAxisRectangle.Bottom - Utils.Round(num3), (this.IAxisRectangle.Right + base.Axis.AxisPen.Width) + base.RedLine.VertSize, this.IAxisRectangle.Bottom - Utils.Round(num2)));
                }
            }
            if (base.GreenLine.Visible)
            {
                if (base.Horizontal)
                {
                    num = ((double) this.IAxisRectangle.Width) / base.IRange;
                }
                else
                {
                    num = ((double) this.IAxisRectangle.Height) / base.IRange;
                }
                num2 = num * (base.GreenLineStartValue - base.Minimum);
                num3 = num * (base.GreenLineEndValue - base.Minimum);
                g = this.PrepareGraphics(base.GreenLine.Pen, base.GreenLine.Brush);
                if (base.Horizontal)
                {
                    g.Rectangle(Utils.FromLTRB(this.IAxisRectangle.Left + Utils.Round(num2), this.IAxisRectangle.Bottom + base.Axis.AxisPen.Width, this.IAxisRectangle.Left + Utils.Round(num3), (this.IAxisRectangle.Bottom + base.Axis.AxisPen.Width) + base.GreenLine.VertSize));
                }
                else
                {
                    g.Rectangle(Utils.FromLTRB(this.IAxisRectangle.Right + base.Axis.AxisPen.Width, this.IAxisRectangle.Bottom - Utils.Round(num3), (this.IAxisRectangle.Right + base.Axis.AxisPen.Width) + base.GreenLine.VertSize, this.IAxisRectangle.Bottom - Utils.Round(num2)));
                }
            }
        }

        protected override void DrawFace(Graphics3D g)
        {
            g.Rectangle(base.INewRectangle);
        }

        protected override void DrawHand(Graphics3D g)
        {
            if (base.Hand.Visible)
            {
                double num;
                if (base.Horizontal)
                {
                    num = ((double) this.IAxisRectangle.Width) / base.IRange;
                }
                else
                {
                    num = ((double) this.IAxisRectangle.Height) / base.IRange;
                }
                num *= base.Value - base.Minimum;
                g = this.PrepareGraphics(base.Hand.Pen, base.Hand.Brush);
                if (this.useValueColorPalette)
                {
                    base.Hand.Brush.Gradient.Visible = true;
                    if (base.Horizontal)
                    {
                        base.Hand.Brush.Gradient.Direction = LinearGradientMode.Horizontal;
                        base.Hand.Brush.Gradient.ExtendedColorPalette = this.ValueColorPalette;
                        base.Hand.Brush.Gradient.CustomTargetRectangle = this.IAxisRectangle;
                    }
                    else
                    {
                        base.Hand.Brush.Gradient.Direction = LinearGradientMode.Vertical;
                        base.Hand.Brush.Gradient.ExtendedColorPalette = this.ValueColorPalette;
                        base.Hand.Brush.Gradient.CustomTargetRectangle = this.IAxisRectangle;
                    }
                }
                else
                {
                    base.Hand.Brush.Gradient.Visible = false;
                }
                if (base.Horizontal)
                {
                    g.Rectangle(Utils.FromLTRB(this.IAxisRectangle.Left, this.IAxisRectangle.Top, this.IAxisRectangle.Left + Utils.Round(num), this.IAxisRectangle.Bottom + Utils.Round((float) (base.Axis.AxisPen.Width / 2))));
                }
                else
                {
                    g.Rectangle(Utils.FromLTRB(this.IAxisRectangle.Left, this.IAxisRectangle.Top + (this.IAxisRectangle.Height - Utils.Round(num)), this.IAxisRectangle.Right + Utils.Round((float) (base.Axis.AxisPen.Width / 2)), this.IAxisRectangle.Bottom + Utils.Round((float) (base.Axis.AxisPen.Width / 2))));
                }
            }
        }

        private void DrawMaxValueIndicator(Graphics3D g)
        {
            if (this.MaxValueIndicator.Visible)
            {
                if (base.Horizontal)
                {
                    this.MaxValueIndicator.Draw(g, false, this.IAxisRectangle.Left + this.GetPixelValue(this.maxValue), this.IAxisRectangle.Bottom, this.MaxValueIndicator.HorizSize, this.MaxValueIndicator.VertSize, this.MaxValueIndicator.Brush.Color, this.MaxValueIndicator.Style);
                }
                else
                {
                    this.MaxValueIndicator.Draw(g, false, this.IAxisRectangle.Right, this.IAxisRectangle.Top + (this.IAxisRectangle.Height - this.GetPixelValue(this.maxValue)), this.MaxValueIndicator.HorizSize, this.MaxValueIndicator.VertSize, this.MaxValueIndicator.Brush.Color, this.MaxValueIndicator.Style);
                }
            }
        }

        protected override Axis GetAxis()
        {
            if (base.Horizontal)
            {
                return base.GetHorizAxis;
            }
            return base.GetVertAxis;
        }

        private int GetPixelValue(double value)
        {
            double num = base.Horizontal ? (((double) this.IAxisRectangle.Width) / base.IRange) : (((double) this.IAxisRectangle.Height) / base.IRange);
            return Utils.Round((double) ((value - base.Minimum) * num));
        }

        public override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            base.Axis.Labels.Visible = false;
            base.Axis.Ticks.Visible = false;
            base.Ticks.VertSize = 5;
            base.RedLine.VertSize = 3;
            base.GreenLine.VertSize = 3;
            this.MaxValueIndicator.VertSize = 3;
            base.Hand.Color = Color.Red;
            base.Value = 70.0;
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if (value != null)
            {
                base.IOrigRectangle = Rectangle.Empty;
                base.INewRectangle = Rectangle.Empty;
                base.UseAxis = false;
                base.ShowInLegend = false;
                base.Frame.chart = value;
            }
        }

        protected override void SetValue(double value)
        {
            if (value > this.maxValue)
            {
                this.maxValue = value;
            }
            base.SetValue(value);
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
                return Texts.GalleryLinearGauge;
            }
        }

        [Description("A visible marker for the maximum value reached by the LinearGauge."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeSeriesPointer MaxValueIndicator
        {
            get
            {
                if (this.maxValueIndicator == null)
                {
                    this.maxValueIndicator = new GaugeSeriesPointer(base.Chart, this);
                    if (base.Horizontal)
                    {
                        this.maxValueIndicator.Style = 2;
                    }
                    else
                    {
                        this.maxValueIndicator.Style = 10;
                    }
                    this.maxValueIndicator.Brush.Visible = true;
                    this.maxValueIndicator.Brush.Color = Color.Red;
                    this.maxValueIndicator.VertSize = 5;
                }
                return this.maxValueIndicator;
            }
            set
            {
                this.maxValueIndicator = value;
            }
        }

        [Description("An array of Colors defining a custom gradient color scheme.")]
        public bool UseValueColorPalette
        {
            get
            {
                return this.useValueColorPalette;
            }
            set
            {
                if (this.useValueColorPalette != value)
                {
                    this.useValueColorPalette = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Brush characteristics of the area behind the value, or hand, pointer."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush ValueAreaBrush
        {
            get
            {
                if (this.valueAreaBrush == null)
                {
                    this.valueAreaBrush = new ChartBrush(base.Chart, false);
                }
                return this.valueAreaBrush;
            }
            set
            {
                this.valueAreaBrush = value;
            }
        }

        [Description("An array of Colors defining a custom gradient color scheme.")]
        public Color[] ValueColorPalette
        {
            get
            {
                if (this.valueColorPalette == null)
                {
                    this.valueColorPalette = new Color[] { Color.Green, Color.Yellow, Color.Orange, Color.Red };
                }
                return this.valueColorPalette;
            }
            set
            {
                this.valueColorPalette = value;
                this.Invalidate();
            }
        }
    }
}

