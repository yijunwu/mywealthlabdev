namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(Gauges), "SeriesIcons.Gauges.bmp"), Description("Gauges Series.")]
    public class Gauges : Circular
    {
        private double FAngle;
        private SeriesPointer FCenter;
        private int FDistance;
        private SeriesPointer FEndPoint;
        private bool FLabelsInside;
        private double FMax;
        private double FMin;
        private int FMinorDistance;
        private Steema.TeeChart.Styles.HandStyle FStyle;
        private Point ICenter;
        private ChartPen pen;
        private const int TeeHandDistance = 30;

        public event GaugesChangeHandler OnChange;

        public Gauges() : this(null)
        {
        }

        public Gauges(Chart c) : base(c)
        {
            this.ICenter = new Point(0, 0);
            this.FStyle = Steema.TeeChart.Styles.HandStyle.Line;
            this.FDistance = 30;
            base.Circled = true;
            base.ShowInLegend = false;
            this.FLabelsInside = true;
            this.FCenter = new SeriesPointer(c, this);
            this.FCenter.Brush.Solid = true;
            this.FCenter.Visible = true;
            this.FCenter.Style = PointerStyles.Circle;
            this.FCenter.HorizSize = 8;
            this.FCenter.VertSize = 8;
            this.FCenter.Gradient.Visible = true;
            this.FEndPoint = new SeriesPointer(c, this);
            this.FEndPoint.Visible = false;
            this.FEndPoint.Brush.Solid = true;
            this.FEndPoint.Style = PointerStyles.Circle;
            this.FEndPoint.HorizSize = 3;
            this.FEndPoint.VertSize = 3;
            this.FEndPoint.Gradient.Visible = false;
            this.FEndPoint.Color = Color.White;
            this.FCenter.Gradient.StartColor = Color.White;
            this.FCenter.Gradient.EndColor = Color.Black;
            this.FCenter.Color = Color.Black;
            base.Add(0);
            this.Maximum = 100.0;
            this.TotalAngle = 90.0;
            base.RotationAngle = 0x87;
        }

        protected override void AddSampleValues(int numValues)
        {
            Random random = new Random();
            this.Value = this.Minimum + ((this.Maximum - this.Minimum) * random.NextDouble());
        }

        private Steema.TeeChart.Axis Axis()
        {
            return base.GetVertAxis;
        }

        private void CalcLinePoints(out Point P0, out Point P1)
        {
            double num;
            double num2 = this.Maximum - this.Minimum;
            if (num2 == 0.0)
            {
                num = 1.5707963267948966;
            }
            else
            {
                num = this.TotalAngle - (((this.Value - this.Minimum) * this.TotalAngle) / num2);
                num = Utils.PiStep * ((360.0 - num) + base.RotationAngle);
            }
            P1 = this.CalcPoint(num, this.ICenter, (double) base.XRadius, (double) base.YRadius);
            int aDist = this.HandDistance + this.SizePointer(this.FEndPoint);
            if (aDist > 0)
            {
                P1 = Utils.PointAtDistance(this.ICenter, P1, aDist);
            }
            if (this.FCenter.Visible && (this.FCenter.Style == PointerStyles.Circle))
            {
                P0 = Utils.PointAtDistance(P1, this.ICenter, this.SizePointer(this.FCenter) / 2);
            }
            else
            {
                P0 = this.ICenter;
            }
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

        public override void Draw()
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            if (this.Axis() != null)
            {
                double num;
                double num3;
                double num6;
                double num7;
                if (this.Axis().Increment == 0.0)
                {
                    this.Axis().Increment = 10.0;
                }
                this.ICenter.X = graphicsd.ChartXCenter;
                this.ICenter.Y = graphicsd.ChartYCenter;
                double num2 = this.Maximum - this.Minimum;
                double increment = this.Axis().Increment;
                if (this.FCenter.Visible)
                {
                    this.FCenter.Draw(this.ICenter.X, this.ICenter.Y, this.FCenter.Brush.Color, this.FCenter.Style);
                }
                if (this.Axis().Ticks.Visible || this.Axis().Labels.Visible)
                {
                    graphicsd.Font = this.Axis().Labels.Font;
                    graphicsd.Pen = this.Axis().Ticks;
                    num6 = base.XRadius - this.Axis().Ticks.Length;
                    num7 = base.YRadius - this.Axis().Ticks.Length;
                    int fontHeight = graphicsd.FontHeight;
                    if ((num2 != 0.0) && (increment != 0.0))
                    {
                        num3 = 0.0;
                        do
                        {
                            num = this.TotalAngle - ((num3 * this.TotalAngle) / num2);
                            num = Utils.PiStep * ((360.0 - num) + base.RotationAngle);
                            Point point = this.CalcPoint(num, this.ICenter, num6, num7);
                            Point point2 = this.CalcPoint(num, this.ICenter, (double) base.XRadius, (double) base.YRadius);
                            if (this.Axis().Ticks.Visible)
                            {
                                graphicsd.Line(point, point2);
                            }
                            if (this.Axis().Labels.Visible)
                            {
                                string text = Utils.FormatFloat(base.ValueFormat, num3 + this.Minimum);
                                if (!this.LabelsInside)
                                {
                                    point = this.CalcPoint(num, this.ICenter, (double) (base.XRadius + fontHeight), (double) (base.YRadius + fontHeight));
                                }
                                point.X -= Utils.Round((double) (graphicsd.TextWidth(text) * 0.5));
                                graphicsd.TextOut(point.X, point.Y, text);
                            }
                            num3 += increment;
                        }
                        while (num3 <= num2);
                    }
                }
                if (((num2 != 0.0) && this.Axis().MinorTicks.Visible) && (this.Axis().MinorTickCount > 0))
                {
                    graphicsd.Pen = this.Axis().MinorTicks;
                    num6 = (base.XRadius - this.Axis().MinorTicks.Length) - this.MinorTickDistance;
                    num7 = (base.YRadius - this.Axis().MinorTicks.Length) - this.MinorTickDistance;
                    if (increment != 0.0)
                    {
                        double num5 = increment / ((double) (this.Axis().MinorTickCount + 1));
                        num3 = 0.0;
                        do
                        {
                            for (int i = 1; i <= this.Axis().MinorTickCount; i++)
                            {
                                num = this.TotalAngle - (((num3 + (i * num5)) * this.TotalAngle) / num2);
                                num = Utils.PiStep * ((360.0 - num) + base.RotationAngle);
                                graphicsd.Line(this.CalcPoint(num, this.ICenter, num6, num7), this.CalcPoint(num, this.ICenter, (double) (base.XRadius - this.MinorTickDistance), (double) (base.YRadius - this.MinorTickDistance)));
                            }
                            num3 += increment;
                        }
                        while (num3 <= (num2 - increment));
                    }
                }
                if (this.Axis().Visible)
                {
                    this.DrawAxis();
                }
            }
            if (this.Pen.Visible)
            {
                this.DrawValueLine();
            }
        }

        private void DrawAxis()
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            graphicsd.Pen = this.Axis().AxisPen;
            double num3 = 0.0;
            double num2 = this.Maximum - num3;
            if (num2 != 0.0)
            {
                double angle = this.TotalAngle - ((num3 * this.TotalAngle) / num2);
                angle = Utils.PiStep * ((360.0 - angle) + base.RotationAngle);
                Point point = this.CalcPoint(angle, this.ICenter, (double) base.XRadius, (double) base.YRadius);
                angle = this.TotalAngle - ((this.Maximum * this.TotalAngle) / num2);
                angle = Utils.PiStep * ((360.0 - angle) + base.RotationAngle);
                Point point2 = this.CalcPoint(angle, this.ICenter, (double) base.XRadius, (double) base.YRadius);
                graphicsd.Arc(base.CircleRect.Left, base.CircleRect.Top, base.CircleRect.Right, base.CircleRect.Bottom, point.X, point.Y, point2.X, point2.Y);
            }
        }

        private void DrawValueLine()
        {
            Point point;
            Point point2;
            this.CalcLinePoints(out point2, out point);
            Graphics3D graphicsd = base.chart.Graphics3D;
            graphicsd.Pen = this.Pen;
            if (this.HandStyle == Steema.TeeChart.Styles.HandStyle.Line)
            {
                graphicsd.Line(point2, point);
            }
            else
            {
                double angle = Math.Atan2((double) (point.Y - point2.X), (double) (point.X - point2.Y));
                Point[] p = new Point[] { this.CalcPoint(angle, point2, 4.0, 4.0), point, this.CalcPoint(angle + 3.1415926535897931, point2, 4.0, 4.0) };
                graphicsd.Polygon(p);
            }
            point = Utils.PointAtDistance(this.ICenter, point, -this.SizePointer(this.FEndPoint) / 2);
            if (this.FEndPoint.Visible)
            {
                this.FEndPoint.Draw(point.X, point.Y, this.FEndPoint.Brush.Color, this.FEndPoint.Style);
            }
        }

        public override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            base.chart.Aspect.Chart3DPercent = 0;
            base.chart.Axes.Left.Labels.Visible = false;
            this.Center.VertSize = 3;
            this.Center.HorizSize = 3;
            this.Pen.Color = Color.Blue;
            this.HandDistance = 5;
            this.Value = 70.0;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.FCenter != null)
            {
                this.FCenter.Chart = c;
            }
            if (this.FEndPoint != null)
            {
                this.FEndPoint.Chart = c;
            }
            if (base.chart != null)
            {
                base.chart.Aspect.View3D = false;
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

        [Description("Returns a sub-object with properties that control the appearance of a shape at the middle of gauge."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public SeriesPointer Center
        {
            get
            {
                return this.FCenter;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryGauge;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Returns a sub-object with properties that control the appearance of a shape at the end of the gauge arrow.")]
        public SeriesPointer EndPoint
        {
            get
            {
                return this.FEndPoint;
            }
        }

        [DefaultValue(30), Description("The amount in pixels that define a gap between the gauge axis and the end of the gauge arrow line.")]
        public int HandDistance
        {
            get
            {
                return this.FDistance;
            }
            set
            {
                base.SetIntegerProperty(ref this.FDistance, value);
            }
        }

        [Description("Style of Gauge hand."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), DefaultValue(0)]
        public Steema.TeeChart.Styles.HandStyle HandStyle
        {
            get
            {
                return this.FStyle;
            }
            set
            {
                if (this.FStyle != value)
                {
                    this.FStyle = value;
                }
                base.Repaint();
            }
        }

        [Description("Defines where to display gauge axis labels."), DefaultValue(true)]
        public bool LabelsInside
        {
            get
            {
                return this.FLabelsInside;
            }
            set
            {
                base.SetBooleanProperty(ref this.FLabelsInside, value);
            }
        }

        [Description("Gets or sets the maximum value for the gauge.")]
        public double Maximum
        {
            get
            {
                return this.FMax;
            }
            set
            {
                base.SetDoubleProperty(ref this.FMax, value);
                this.Value = Math.Min(this.Maximum, this.Value);
            }
        }

        [Description("Gets or sets the minimum value for the gauge.")]
        public double Minimum
        {
            get
            {
                return this.FMin;
            }
            set
            {
                base.SetDoubleProperty(ref this.FMin, value);
                this.Value = Math.Max(this.Minimum, this.Value);
            }
        }

        [DefaultValue(0), Description("Set to a value bigger than zero to display the axis minor ticks (ticks inside normal ticks) displaced from the axis line the specified number of pixels.")]
        public int MinorTickDistance
        {
            get
            {
                return this.FMinorDistance;
            }
            set
            {
                base.SetIntegerProperty(ref this.FMinorDistance, value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Line pen for Gauge.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart, Color.Black);
                }
                return this.pen;
            }
        }

        [DefaultValue(90), Description("Controls the size in degrees for the gauge axis.")]
        public double TotalAngle
        {
            get
            {
                return this.FAngle;
            }
            set
            {
                base.SetDoubleProperty(ref this.FAngle, value);
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
                    base.Repaint();
                    if (this.OnChange != null)
                    {
                        this.OnChange(this, new EventArgs());
                    }
                }
            }
        }
    }
}

