namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;

    [ToolboxBitmap(typeof(CircularGauge), "SeriesIcons.CircularGauge.bmp"), Description("CircularGauge Series.")]
    public class CircularGauge : Circular
    {
        private GaugeSeriesPointer center;
        private SeriesPointer endPoint;
        private ChartBrush faceBrush;
        private TFrame frame;
        private Color[] gaugeColorPalette;
        private GaugeSeriesPointer greenLine;
        private double greenLineEndValue;
        private double greenLineStartValue;
        private GaugeSeriesPointer hand;
        private int handDistance;
        private int handOffset;
        private float IAngleInc;
        private Point ICenter;
        private Rectangle INewRectangle;
        private Rectangle IOrigRectangle;
        private double IRange;
        private float IStartAngle;
        private bool labelsInside;
        private double maximum;
        private double minimum;
        private int minorTickDistance;
        private GaugeSeriesPointer minorTicks;
        private GaugeSeriesPointer redLine;
        private double redLineEndValue;
        private double redLineStartValue;
        private bool rotateLabels;
        private GaugeSeriesPointer ticks;
        private double totalAngle;

        public event GaugesChangeHandler ValueChanged;

        public CircularGauge() : this(null)
        {
        }

        public CircularGauge(Chart c) : base(c)
        {
            base.Add(0);
            this.handDistance = 80;
            this.totalAngle = 300.0;
            this.maximum = 100.0;
            this.redLineStartValue = 80.0;
            this.redLineEndValue = 100.0;
            this.greenLineStartValue = 0.0;
            this.greenLineEndValue = 70.0;
            this.labelsInside = true;
            this.rotateLabels = true;
            this.minorTickDistance = 3;
            this.handOffset = 30;
            this.GaugeColorPalette = CustomGauge.BlackPalette;
        }

        protected override void AddSampleValues(int numValues)
        {
            Random random = new Random();
            this.Value = this.Minimum + ((this.Maximum - this.Minimum) * random.NextDouble());
        }

        private double CalcAngleFromLength(Point Point, double Length)
        {
            double xRadius;
            double d = 0.0;
            if (base.Circled)
            {
                xRadius = base.XRadius;
            }
            else
            {
                double num2 = Math.Abs((int) (this.ICenter.X - Point.X));
                double num3 = Math.Abs((int) (this.ICenter.Y - Point.Y));
                xRadius = Math.Sqrt((num2 * num2) + (num3 * num3));
            }
            double num5 = (xRadius * xRadius) * 2.0;
            d = num5 - (Length * Length);
            d /= num5;
            return Math.Acos(d);
        }

        private int CalcDistance(int distance)
        {
            if (distance > 0)
            {
                double num = Math.Min(base.XRadius, base.YRadius);
                num /= 100.0;
                return Utils.Round((double) ((distance * num) / 2.0));
            }
            return 0;
        }

        private Point CalcPoint(double Angle, Point Center, double RadiusX, double RadiusY)
        {
            double num;
            double num2;
            Point point = new Point(0, 0);
            Utils.SinCos(Angle, out num, out num2);
            point.X = Center.X - Utils.Round((double) (RadiusX * num2));
            point.Y = Center.Y - Utils.Round((double) (RadiusY * num));
            return point;
        }

        private float CalcStartAngle()
        {
            float num = 360f - ((float) this.TotalAngle);
            return ((num / 2f) + base.RotationAngle);
        }

        private float CalcSweepAngle()
        {
            return (float) this.TotalAngle;
        }

        private int CalcValue(double value)
        {
            double num = value;
            num *= this.IAngleInc;
            num += this.IStartAngle;
            return Utils.Round(num);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
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

        private void DrawAxis(Graphics3D g)
        {
            if ((this.Axis != null) && this.Axis.Visible)
            {
                double num;
                double num2;
                double num5;
                double num7;
                double num8;
                g.Arc(this.INewRectangle.Left, this.INewRectangle.Top, this.INewRectangle.Right, this.INewRectangle.Bottom, this.IStartAngle + 90f, this.CalcSweepAngle());
                double increment = this.Axis.Increment;
                if (increment == 0.0)
                {
                    increment = 10.0;
                }
                if (this.Axis.Title.Visible && !Utils.IsNullOrEmpty(this.Axis.Title.Caption))
                {
                    g.Font = this.Axis.Title.Font;
                    float num12 = g.TextWidth(this.Axis.Title.Caption);
                    float fontHeight = g.FontHeight;
                    int x = base.CircleXCenter - Utils.Round((double) ((((double) base.XRadius) / 2.5) + (((double) num12) / 2.0)));
                    int y = base.CircleYCenter - Utils.Round((double) (((double) fontHeight) / 2.0));
                    g.TextOut(x, y, this.Axis.Title.Caption);
                }
                if (this.Axis.Ticks.Visible || this.Axis.Labels.Visible)
                {
                    this.Axis.Ticks.Length = this.Ticks.VertSize;
                    this.Axis.Ticks.Assign(this.Ticks.Pen);
                    g.Font = this.Axis.Labels.Font;
                    g.Pen = this.Ticks.Pen;
                    num7 = base.XRadius - this.Axis.Ticks.Length;
                    num8 = base.YRadius - this.Axis.Ticks.Length;
                    int num10 = g.FontHeight;
                    if ((this.IRange != 0.0) && (increment != 0.0))
                    {
                        num2 = 0.0;
                        do
                        {
                            num5 = (this.IStartAngle - 90f) + ((num2 * this.TotalAngle) / this.IRange);
                            num = Utils.PiStep * num5;
                            Point inner = this.CalcPoint(num, this.ICenter, num7, num8);
                            Point outer = this.CalcPoint(num, this.ICenter, (double) base.XRadius, (double) base.YRadius);
                            double angle = Utils.PiStep * (num5 + (this.Ticks.HorizSize * 0.1));
                            Point innerPlus = this.CalcPoint(angle, this.ICenter, num7, num8);
                            angle = Utils.PiStep * (num5 - (this.Ticks.HorizSize * 0.1));
                            Point innerMinus = this.CalcPoint(angle, this.ICenter, num7, num8);
                            angle = Utils.PiStep * (num5 + (this.Ticks.HorizSize * 0.2));
                            Point outerPlus = this.CalcPoint(angle, this.ICenter, (double) base.XRadius, (double) base.YRadius);
                            angle = Utils.PiStep * (num5 - (this.Ticks.HorizSize * 0.2));
                            Point outerMinus = this.CalcPoint(angle, this.ICenter, (double) base.XRadius, (double) base.YRadius);
                            this.DrawAxisTick(g, inner, innerPlus, innerMinus, outer, outerPlus, outerMinus);
                            if (this.Axis.Labels.Visible)
                            {
                                double num6;
                                string text = Utils.FormatFloat(base.ValueFormat, num2 + this.Minimum);
                                double length = g.TextWidth(text);
                                if (this.RotateLabels)
                                {
                                    num6 = this.CalcAngleFromLength(outer, length) / 2.0;
                                    if (this.LabelsInside)
                                    {
                                        inner = this.CalcPoint(num - num6, this.ICenter, num7 - (num10 / 4), num8 - (num10 / 4));
                                    }
                                    else
                                    {
                                        inner = this.CalcPoint(num - num6, this.ICenter, (double) (base.XRadius + num10), (double) (base.YRadius + num10));
                                    }
                                    g.RotateLabel(inner.X, inner.Y, text, 90.0 - num5);
                                }
                                else
                                {
                                    if (this.LabelsInside)
                                    {
                                        inner = this.CalcPoint(num, this.ICenter, num7 - (num10 / 2), num8 - (num10 / 2));
                                        num6 = num5 - 90.0;
                                        if (num6 < 0.0)
                                        {
                                            num6 *= -1.0;
                                        }
                                        inner.Y -= Utils.Round((double) (num10 * (0.0055555555555555558 * num6)));
                                    }
                                    else
                                    {
                                        inner = this.CalcPoint(num, this.ICenter, (double) (base.XRadius + num10), (double) (base.YRadius + num10));
                                    }
                                    num6 = num5;
                                    if (num6 > 180.0)
                                    {
                                        num6 = 180.0 - (num6 - 180.0);
                                    }
                                    if (num6 < 0.0)
                                    {
                                        num6 *= -1.0;
                                    }
                                    inner.X -= Utils.Round((double) (length * (0.0055555555555555558 * num6)));
                                    g.TextOut(inner.X, inner.Y, text);
                                }
                            }
                            num2 += increment;
                        }
                        while (((num2 <= this.IRange) && (this.TotalAngle <= 360.0)) || ((num2 < this.IRange) && (this.TotalAngle > 360.0)));
                    }
                }
                if (((this.IRange != 0.0) && this.Axis.MinorTicks.Visible) && (this.Axis.MinorTickCount > 0))
                {
                    this.Axis.MinorTicks.Length = this.MinorTicks.VertSize;
                    this.Axis.MinorTicks.Assign(this.MinorTicks.Pen);
                    g.Pen = this.MinorTicks.Pen;
                    num7 = (base.XRadius - this.Axis.MinorTicks.Length) - this.MinorTickDistance;
                    num8 = (base.YRadius - this.Axis.MinorTicks.Length) - this.MinorTickDistance;
                    if (increment != 0.0)
                    {
                        double num4 = increment / ((double) (this.Axis.MinorTickCount + 1));
                        num2 = 0.0;
                        do
                        {
                            for (int i = 1; i <= this.Axis.MinorTickCount; i++)
                            {
                                num5 = (this.IStartAngle - 90f) + (((num2 + (i * num4)) * this.TotalAngle) / this.IRange);
                                num = Utils.PiStep * num5;
                                this.DrawAxisMinorTick(g, this.CalcPoint(num, this.ICenter, num7, num8), this.CalcPoint(num, this.ICenter, (double) (base.XRadius - this.MinorTickDistance), (double) (base.YRadius - this.MinorTickDistance)));
                            }
                            num2 += increment;
                        }
                        while (num2 <= (this.IRange - increment));
                    }
                }
            }
        }

        private void DrawAxisMinorTick(Graphics3D g, Point Inner, Point Outer)
        {
            this.MinorTicks.Draw(g, Inner, new Point(0, 0), new Point(0, 0), Outer, new Point(0, 0), new Point(0, 0));
        }

        private void DrawAxisTick(Graphics3D g, Point Inner, Point InnerPlus, Point InnerMinus, Point Outer, Point OuterPlus, Point OuterMinus)
        {
            this.Ticks.Draw(g, Inner, InnerPlus, InnerMinus, Outer, OuterPlus, OuterMinus);
        }

        private void DrawCenter(Graphics3D g)
        {
            if (this.Center.Visible)
            {
                this.Center.Draw(g, false, base.CircleXCenter, base.CircleYCenter, this.Center.HorizSize, this.Center.VertSize, this.Center.Brush.Color, this.Center.Style);
            }
        }

        private void DrawCenterShadow(Graphics3D g)
        {
            if (this.Center.Visible)
            {
                this.Center.DrawShadow(g, 0f, base.CircleXCenter, base.CircleYCenter, this.Center.HorizSize, this.Center.VertSize, this.Center.Style);
            }
        }

        private void DrawColorLines(Graphics3D g)
        {
            if (this.RedLine.Visible)
            {
                Rectangle iNewRectangle = this.INewRectangle;
                double num = Math.Min(base.XRadius, base.YRadius);
                num /= 5.0;
                int width = Utils.Round(num) * -1;
                if (this.Axis.Labels.Visible)
                {
                    if (this.RotateLabels)
                    {
                        width -= g.FontTextHeight(this.Axis.Labels.Font);
                    }
                    else
                    {
                        width -= (int) g.TextWidth(this.Axis.Labels.Font, this.Maximum.ToString());
                    }
                }
                iNewRectangle.Inflate(width, width);
                this.RedLine.DrawColorLine(g, this.CalcValue(this.RedLineStartValue), this.CalcValue(this.RedLineEndValue), iNewRectangle);
            }
            if (this.GreenLine.Visible)
            {
                Rectangle container = this.INewRectangle;
                double num3 = Math.Min(base.XRadius, base.YRadius);
                num3 /= 5.0;
                int num4 = Utils.Round(num3) * -1;
                if (this.Axis.Labels.Visible)
                {
                    if (this.RotateLabels)
                    {
                        num4 -= g.FontTextHeight(this.Axis.Labels.Font);
                    }
                    else
                    {
                        num4 -= (int) g.TextWidth(this.Axis.Labels.Font, this.Maximum.ToString());
                    }
                }
                container.Inflate(num4, num4);
                this.GreenLine.DrawColorLine(g, this.CalcValue(this.GreenLineStartValue), this.CalcValue(this.GreenLineEndValue), container);
            }
        }

        private void DrawEnd(Graphics3D g)
        {
            if (this.EndPoint.Visible)
            {
                this.EndPoint.Draw(base.CircleXCenter, (base.CircleYCenter + (this.Hand.VertSize * 2)) + this.CalcDistance(this.HandOffset), this.EndPoint.Brush.Color, this.EndPoint.Style);
            }
        }

        private void DrawFace(Graphics3D g)
        {
            g.Ellipse(this.INewRectangle);
            this.DrawColorLines(g);
        }

        private void DrawFrame()
        {
            if (this.Frame.Visible)
            {
                this.Frame.Draw(this.IOrigRectangle);
            }
        }

        private void DrawHand(Graphics3D g)
        {
            try
            {
                if (this.Hand.Visible)
                {
                    float angle = this.CalcValue((double) (((float) this.Value) - ((float) this.Minimum)));
                    this.DrawCenterShadow(g);
                    Matrix matrix = new Matrix();
                    matrix.RotateAt(angle, new PointF((float) base.CircleXCenter, (float) base.CircleYCenter));
                    g.g.Transform = matrix;
                    this.Hand.VertSize = this.CalcDistance(this.HandDistance);
                    this.Hand.DrawShadow(g, angle, base.CircleXCenter, (base.CircleYCenter + this.Hand.VertSize) - this.CalcDistance(this.HandOffset), this.Hand.HorizSize, this.Hand.VertSize + this.CalcDistance(this.HandOffset), this.Hand.Style);
                    this.Hand.Draw(g, false, base.CircleXCenter, (base.CircleYCenter + this.Hand.VertSize) - this.CalcDistance(this.HandOffset), this.Hand.HorizSize, this.Hand.VertSize + this.CalcDistance(this.HandOffset), this.Hand.Brush.Color, this.Hand.Style);
                }
            }
            finally
            {
                g.g.ResetTransform();
            }
            this.DrawEnd(g);
            this.DrawCenter(g);
        }

        private bool IsDefaultAxis()
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
                visible = !this.Axis.Title.Font.Strikeout;
            }
            if (visible)
            {
                visible = !this.Axis.Title.Font.Underline;
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.Visible;
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
                visible = this.Axis.AxisPen.DashPattern == null;
            }
            if (visible)
            {
                visible = (this.Axis.Labels.Font.Color == Color.FromArgb(0xff, 0, 0, 0)) || (this.Axis.Labels.Font.Color == Color.Black);
            }
            if (visible)
            {
                visible = (this.Axis.Labels.Font.Color == Color.FromArgb(0xff, 0, 0, 0)) || (this.Axis.Labels.Font.Color == Color.Black);
            }
            if (visible)
            {
                visible = this.Axis.AxisPen.DashCap == DashCap.Flat;
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

        public override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            this.Axis.Visible = false;
            this.Center.VertSize = 3;
            this.Center.HorizSize = 3;
            this.Hand.HorizSize = 2;
            this.HandDistance = 100;
            this.RedLine.VertSize = 5;
            this.Value = 70.0;
        }

        private Graphics3D PrepareGraphics(ChartPen pen, ChartBrush brush)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            graphicsd.Pen = pen;
            graphicsd.Brush = brush;
            return graphicsd;
        }

        private void SetAxisOnce()
        {
            if (this.IsDefaultAxis())
            {
                this.Axis.AxisPen.Visible = true;
                this.Axis.Labels.Visible = true;
                this.Axis.Labels.Font.Color = CustomGauge.GetGaugePaletteColor(20, this.GaugeColorPalette);
                this.Axis.Title.Visible = true;
                this.Axis.Title.Font.Color = CustomGauge.GetGaugePaletteColor(0x15, this.GaugeColorPalette);
                this.Axis.Labels.Font.Name = "Arial";
                this.Axis.Labels.Font.Size = 12;
                this.Axis.Labels.Font.Bold = true;
            }
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if (value != null)
            {
                base.Circled = true;
                base.ShowInLegend = false;
                this.ICenter = Point.Empty;
                this.INewRectangle = Rectangle.Empty;
                this.IOrigRectangle = Rectangle.Empty;
                this.Frame.Chart = value;
            }
        }

        private void SetDefaultAxis()
        {
            if (this.Axis != null)
            {
                this.Axis.AxisPen.Visible = true;
                this.Axis.Labels.Visible = true;
                this.Axis.Labels.Font.Color = Color.Black;
                this.Axis.Title.Font.Color = Color.Black;
                this.Axis.Title.Visible = true;
                this.Axis.Labels.Font.Name = "Verdana";
                this.Axis.Labels.Font.Size = 8;
                this.Axis.Labels.Font.Bold = false;
                this.Axis.Ticks.Length = 4;
                this.Axis.MinorTicks.Length = 2;
            }
        }

        private void SetValues()
        {
            this.ICenter.X = base.CircleXCenter;
            this.ICenter.Y = base.CircleYCenter;
            this.IRange = this.Maximum - this.Minimum;
            this.IStartAngle = this.CalcStartAngle();
            this.IAngleInc = ((float) this.TotalAngle) / ((float) this.IRange);
            this.INewRectangle = Utils.FromLTRB(base.CircleRect.Left, base.CircleRect.Top, base.CircleRect.Right, base.CircleRect.Bottom);
            this.IOrigRectangle = Utils.FromLTRB(base.CircleRect.Left, base.CircleRect.Top, base.CircleRect.Right, base.CircleRect.Bottom);
            if (this.Frame.Visible)
            {
                this.INewRectangle.Inflate(-this.Frame.CalcWidth(this.INewRectangle), -this.Frame.CalcWidth(this.INewRectangle));
                base.iCircleWidth = this.INewRectangle.Width;
                base.iCircleHeight = this.INewRectangle.Height;
                base.CalcRadius();
            }
        }

        private int SizePointer(SeriesPointer APointer)
        {
            if (APointer.Visible)
            {
                int num = 2 * APointer.VertSize;
                if (APointer.Pen.Visible)
                {
                    num += APointer.Pen.Width;
                }
                return num;
            }
            return 0;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Gets the CircularGauges's Axis characteristics.")]
        public Steema.TeeChart.Axis Axis
        {
            get
            {
                return base.GetVertAxis;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Returns a sub-object with properties that control the appearance of a shape at the middle of gauge.")]
        public GaugeSeriesPointer Center
        {
            get
            {
                if (this.center == null)
                {
                    this.center = new GaugeSeriesPointer(base.chart, this);
                    this.center.Brush.Solid = true;
                    this.center.Visible = true;
                    this.center.Style = GaugePointerStyles.Center;
                    this.center.HorizSize = 0x10;
                    this.center.VertSize = 0x10;
                    this.center.Pen.Color = CustomGauge.GetGaugePaletteColor(0x10, this.GaugeColorPalette);
                    this.center.Pen.Visible = false;
                    this.center.Pen.Width = 2;
                    this.center.Brush.Visible = true;
                    this.center.Brush.Color = Color.Black;
                    this.center.Shadow.Color = Color.Black;
                    this.center.Gradient.Visible = true;
                    this.center.Gradient.StartColor = CustomGauge.GetGaugePaletteColor(0x11, this.GaugeColorPalette);
                    this.center.Gradient.MiddleColor = CustomGauge.GetGaugePaletteColor(0x12, this.GaugeColorPalette);
                    this.center.Gradient.EndColor = CustomGauge.GetGaugePaletteColor(0x13, this.GaugeColorPalette);
                    this.center.Shadow.Transparency = 70;
                }
                return this.center;
            }
            set
            {
                this.center = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryCircularGauge;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Returns a sub-object with properties that control the appearance of a shape at the end of the gauge arrow.")]
        public SeriesPointer EndPoint
        {
            get
            {
                if (this.endPoint == null)
                {
                    this.endPoint = new SeriesPointer(base.chart, this);
                    this.endPoint.Visible = false;
                    this.endPoint.Brush.Solid = true;
                    this.endPoint.Color = Color.White;
                    this.endPoint.Style = PointerStyles.Circle;
                    this.endPoint.HorizSize = 3;
                    this.endPoint.VertSize = 3;
                    this.endPoint.Gradient.Visible = false;
                }
                return this.endPoint;
            }
            set
            {
                this.endPoint = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("ChartBrush characteristics of the CircularGauge's face.")]
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
                    this.faceBrush.Gradient.StartColor = CustomGauge.GetGaugePaletteColor(3, this.GaugeColorPalette);
                    this.faceBrush.Gradient.MiddleColor = CustomGauge.GetGaugePaletteColor(4, this.GaugeColorPalette);
                    this.faceBrush.Gradient.EndColor = CustomGauge.GetGaugePaletteColor(5, this.GaugeColorPalette);
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

        [Description("The Frame is the object which surrounds the CircularGauge and which consists of three configurable Bands: Inner, Middle and Outer."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TFrame Frame
        {
            get
            {
                if (this.frame == null)
                {
                    this.frame = new TFrame(base.chart, this.GaugeColorPalette);
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
                this.center = null;
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
                    this.greenLine.VertSize = 3;
                    this.greenLine.Brush.Visible = true;
                    this.greenLine.Brush.Color = Color.Black;
                    this.greenLine.Gradient.Sigma = true;
                    this.greenLine.Gradient.SigmaFocus = 0f;
                    this.greenLine.Gradient.SigmaScale = 1f;
                    this.greenLine.Gradient.Visible = true;
                    this.greenLine.Gradient.StartColor = CustomGauge.GetGaugePaletteColor(9, this.GaugeColorPalette);
                    this.greenLine.Gradient.MiddleColor = CustomGauge.GetGaugePaletteColor(10, this.GaugeColorPalette);
                    this.greenLine.Gradient.EndColor = CustomGauge.GetGaugePaletteColor(11, this.GaugeColorPalette);
                }
                return this.greenLine;
            }
            set
            {
                this.greenLine = value;
            }
        }

        [Description("Gets and sets the value at which the 'greenline' marker ends."), DefaultValue(70)]
        public double GreenLineEndValue
        {
            get
            {
                return this.greenLineEndValue;
            }
            set
            {
                if (this.greenLineEndValue != value)
                {
                    this.greenLineEndValue = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets and sets the value at which the 'greenline' marker starts."), DefaultValue(0)]
        public double GreenLineStartValue
        {
            get
            {
                return this.greenLineStartValue;
            }
            set
            {
                if (this.greenLineStartValue != value)
                {
                    this.greenLineStartValue = value;
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gets and sets the Hand (or needle) object characteristics.")]
        public GaugeSeriesPointer Hand
        {
            get
            {
                if (this.hand == null)
                {
                    this.hand = new GaugeSeriesPointer(base.chart, this);
                    this.hand.Visible = true;
                    this.hand.Style = GaugePointerStyles.Hand;
                    this.hand.HorizSize = 5;
                    this.hand.VertSize = this.HandDistance;
                    this.hand.Brush.Visible = true;
                    this.hand.Brush.Color = Color.Black;
                    this.hand.Gradient.Direction = LinearGradientMode.Horizontal;
                    this.hand.Shadow.Color = Color.Black;
                    this.hand.Pen.Color = CustomGauge.GetGaugePaletteColor(12, this.GaugeColorPalette);
                    this.hand.Pen.Visible = false;
                    this.hand.Pen.Width = 1;
                    this.hand.Gradient.Visible = true;
                    this.hand.Gradient.StartColor = CustomGauge.GetGaugePaletteColor(13, this.GaugeColorPalette);
                    this.hand.Gradient.StartColor = CustomGauge.GetGaugePaletteColor(14, this.GaugeColorPalette);
                    this.hand.Gradient.EndColor = CustomGauge.GetGaugePaletteColor(15, this.GaugeColorPalette);
                    this.hand.Shadow.Transparency = 70;
                }
                return this.hand;
            }
            set
            {
                this.hand = value;
            }
        }

        [DefaultValue(80), Description("The amount in pixels that define a gap between the gauge axis and the end of the gauge arrow line.")]
        public int HandDistance
        {
            get
            {
                return this.handDistance;
            }
            set
            {
                if (this.handDistance != value)
                {
                    this.handDistance = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Defines the pixel distance between the end of the hand and its center of rotation."), DefaultValue(30)]
        public int HandOffset
        {
            get
            {
                return this.handOffset;
            }
            set
            {
                if (this.handOffset != value)
                {
                    this.handOffset = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Defines where to display gauge axis labels."), DefaultValue(true)]
        public bool LabelsInside
        {
            get
            {
                return this.labelsInside;
            }
            set
            {
                if (this.labelsInside != value)
                {
                    this.labelsInside = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets or sets the maximum value for the gauge.")]
        public double Maximum
        {
            get
            {
                return this.maximum;
            }
            set
            {
                if (this.maximum != value)
                {
                    this.maximum = value;
                    this.Value = Math.Max(this.Maximum, this.Value);
                    this.Invalidate();
                }
            }
        }

        [Description("Gets or sets the minimum value for the gauge.")]
        public double Minimum
        {
            get
            {
                return this.minimum;
            }
            set
            {
                if (this.minimum != value)
                {
                    this.minimum = value;
                    this.Value = Math.Min(this.Minimum, this.Value);
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(3), Description("The distance between the minor ticks and the CircularGauge curved axis.")]
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

        [Description("Gets and sets the characteristics of the Axis' minor tick marks."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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
                    this.minorTicks.Pen.Color = CustomGauge.GetGaugePaletteColor(0x17, this.GaugeColorPalette);
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
                    this.redLine.VertSize = 3;
                    this.redLine.Brush.Visible = true;
                    this.redLine.Brush.Color = Color.Black;
                    this.redLine.Gradient.Sigma = true;
                    this.redLine.Gradient.SigmaFocus = 0f;
                    this.redLine.Gradient.SigmaScale = 1f;
                    this.redLine.Gradient.Visible = true;
                    this.redLine.Gradient.StartColor = CustomGauge.GetGaugePaletteColor(6, this.GaugeColorPalette);
                    this.redLine.Gradient.MiddleColor = CustomGauge.GetGaugePaletteColor(7, this.GaugeColorPalette);
                    this.redLine.Gradient.EndColor = CustomGauge.GetGaugePaletteColor(8, this.GaugeColorPalette);
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
                return this.redLineEndValue;
            }
            set
            {
                if (this.redLineEndValue != value)
                {
                    this.redLineEndValue = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(80), Description("Gets and sets the value at which the 'redline' marker starts.")]
        public double RedLineStartValue
        {
            get
            {
                return this.redLineStartValue;
            }
            set
            {
                if (this.redLineStartValue != value)
                {
                    this.redLineStartValue = value;
                    this.Invalidate();
                }
            }
        }

        [Description("When true, labels are rotated so to appear parallel to the curved CircularGauge axis."), DefaultValue(true)]
        public bool RotateLabels
        {
            get
            {
                return this.rotateLabels;
            }
            set
            {
                if (this.rotateLabels != value)
                {
                    this.rotateLabels = value;
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
                    this.ticks.VertSize = 12;
                    this.ticks.Pen.Color = CustomGauge.GetGaugePaletteColor(0x16, this.GaugeColorPalette);
                }
                return this.ticks;
            }
            set
            {
                this.ticks = value;
            }
        }

        [Description("Controls the size in degrees for the gauge axis."), DefaultValue(300)]
        public double TotalAngle
        {
            get
            {
                return this.totalAngle;
            }
            set
            {
                if (this.totalAngle != value)
                {
                    this.totalAngle = value;
                    this.Invalidate();
                }
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
                return base.mandatory[0];
            }
            set
            {
                if (this.Value != value)
                {
                    base.mandatory[0] = value;
                    this.OnValueChanged(new EventArgs());
                    this.Invalidate();
                }
            }
        }
    }
}

