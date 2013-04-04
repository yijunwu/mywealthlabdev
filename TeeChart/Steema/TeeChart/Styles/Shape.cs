namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Steema.TeeChart.Styles.Shape), "SeriesIcons.Shape.bmp")]
    public class Shape : Series
    {
        private TextShape shape;
        private ShapeStyles style;
        private ShapeTextHorizAlign textHorizAlign;
        private ShapeTextVertAlign textVertAlign;
        private int tmpMidX;
        private int tmpMidY;
        private double x0;
        private double x1;
        private ShapeXYStyles xyStyle;
        private double y0;
        private double y1;

        public Shape() : this(null)
        {
        }

        public Shape(Chart c) : base(c)
        {
            this.shape = new TextShape();
            this.style = ShapeStyles.Circle;
            this.xyStyle = ShapeXYStyles.Axis;
            this.textVertAlign = ShapeTextVertAlign.Center;
            this.textHorizAlign = ShapeTextHorizAlign.Center;
            this.shape.Chart = c;
            this.shape.bBrush = base.bBrush;
            base.calcVisiblePoints = false;
        }

        private void AddPoints()
        {
            if (((this.X1 - this.X0) > 0.0) || ((this.Y1 - this.Y0) > 0.0))
            {
                this.ClearLists();
                base.Add(this.X0, this.Y0);
                base.Add(this.X1, this.Y1);
            }
        }

        protected override void AddSampleValues(int numValues)
        {
            base.Add((double) 0.0, (double) 0.0);
            base.Add((double) 100.0, (double) 100.0);
        }

        protected internal override void CalcZOrder()
        {
            if (base.UseAxis)
            {
                base.CalcZOrder();
            }
        }

        public override int Clicked(int x, int y)
        {
            int num;
            int num2;
            bool flag;
            if (base.chart != null)
            {
                base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
            }
            Point p = new Point(x, y);
            Rectangle shapeRectangle = this.GetShapeRectangle();
            Graphics3D.RectCenter(shapeRectangle, out num, out num2);
            switch (this.style)
            {
                case ShapeStyles.Circle:
                    flag = Graphics3D.PointInEllipse(p, shapeRectangle);
                    break;

                case ShapeStyles.VertLine:
                    flag = Graphics3D.PointInLineTolerance(p, num, shapeRectangle.Y, num, shapeRectangle.Bottom, 3);
                    break;

                case ShapeStyles.HorizLine:
                    flag = Graphics3D.PointInLineTolerance(p, shapeRectangle.X, num2, shapeRectangle.Right, num2, 3);
                    break;

                case ShapeStyles.Triangle:
                case ShapeStyles.Pyramid:
                    flag = Graphics3D.PointInTriangle(p, shapeRectangle.X, shapeRectangle.Right, shapeRectangle.Bottom, shapeRectangle.Y);
                    break;

                case ShapeStyles.InvertTriangle:
                case ShapeStyles.InvertPyramid:
                    flag = Graphics3D.PointInTriangle(p, shapeRectangle.X, shapeRectangle.Right, shapeRectangle.Y, shapeRectangle.Bottom);
                    break;

                case ShapeStyles.Line:
                    flag = Graphics3D.PointInLineTolerance(p, shapeRectangle.X, shapeRectangle.Y, shapeRectangle.Right, shapeRectangle.Bottom, 3);
                    break;

                case ShapeStyles.Diamond:
                {
                    Point[] poly = new Point[] { new Point(num, shapeRectangle.Y), new Point(shapeRectangle.Right, num2), new Point(num, shapeRectangle.Bottom), new Point(shapeRectangle.X, num2) };
                    flag = Graphics3D.PointInPolygon(p, poly);
                    break;
                }
                default:
                    flag = shapeRectangle.Contains(x, y);
                    break;
            }
            if (!flag)
            {
                return -1;
            }
            return 0;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Rectangle);
            AddSubChart(Texts.VertLine);
            AddSubChart(Texts.HorizLine);
            AddSubChart(Texts.Ellipse);
            AddSubChart(Texts.DownTri);
            AddSubChart(Texts.Line);
            AddSubChart(Texts.Diamond);
            AddSubChart(Texts.Cube);
            AddSubChart(Texts.Cross);
            AddSubChart(Texts.DiagCross);
            AddSubChart(Texts.Star);
            AddSubChart(Texts.Pyramid);
            AddSubChart(Texts.InvPyramid);
            AddSubChart(Texts.Hollow);
        }

        protected internal override void DoBeforeDrawChart()
        {
            this.AddPoints();
            base.DoBeforeDrawChart();
        }

        private void DoGradient(Graphics3D g, bool Is3D, Rectangle r)
        {
            if (!this.Transparent && this.Gradient.Visible)
            {
                Rectangle rectangle = Is3D ? g.CalcRect3D(r, base.MiddleZ) : r;
                if (this.style == ShapeStyles.Circle)
                {
                    g.ClipEllipse(rectangle);
                }
                this.Gradient.Draw(g, rectangle);
                if (this.style == ShapeStyles.Circle)
                {
                    g.ClearClipRegions();
                }
            }
        }

        private void DrawCross2D(Rectangle r)
        {
            base.chart.graphics3D.VerticalLine(this.tmpMidX, r.Y, r.Bottom + 1);
            base.chart.graphics3D.HorizontalLine(r.X, r.Right + 1, this.tmpMidY);
        }

        private void DrawCross3D(Rectangle r)
        {
            base.chart.Graphics3D.VerticalLine(this.tmpMidX, r.Y, r.Bottom, base.MiddleZ);
            base.chart.Graphics3D.HorizontalLine(r.X, r.Right, this.tmpMidY, base.MiddleZ);
        }

        private void DrawDiagonalCross2D(Rectangle r)
        {
            base.chart.graphics3D.Line(r.X, r.Y, r.Right + 1, r.Bottom + 1);
            base.chart.graphics3D.Line(r.X, r.Bottom, r.Right + 1, r.Y - 1);
        }

        private void DrawDiagonalCross3D(Rectangle r)
        {
            base.chart.graphics3D.Line(r.X, r.Y, r.Right, r.Bottom, base.MiddleZ);
            base.chart.graphics3D.Line(r.X, r.Bottom, r.Right, r.Y, base.MiddleZ);
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            this.DrawShape(g, false, rect);
        }

        private void DrawShape(bool Is3D, Rectangle r)
        {
            this.DrawShape(base.chart.graphics3D, Is3D, r);
        }

        private void DrawShape(Graphics3D g, bool Is3D, Rectangle r)
        {
            g.Pen = this.Pen;
            g.Brush = this.Brush;
            bool visible = this.Brush.Visible;
            if (this.Transparent)
            {
                g.Brush.Visible = false;
            }
            Graphics3D.RectCenter(r, out this.tmpMidX, out this.tmpMidY);
            if (Is3D)
            {
                switch (this.style)
                {
                    case ShapeStyles.Rectangle:
                        this.DoGradient(g, Is3D, r);
                        g.Rectangle(r, base.MiddleZ);
                        break;

                    case ShapeStyles.Circle:
                        this.DoGradient(g, Is3D, r);
                        g.Ellipse(r, base.MiddleZ);
                        break;

                    case ShapeStyles.VertLine:
                        g.VerticalLine(this.tmpMidX, r.Y, r.Bottom, base.MiddleZ);
                        break;

                    case ShapeStyles.HorizLine:
                        g.HorizontalLine(r.X, r.Right, this.tmpMidY, base.MiddleZ);
                        break;

                    case ShapeStyles.Triangle:
                        g.Triangle(new Point(r.X, r.Bottom), new Point(this.tmpMidX, r.Y), new Point(r.Right, r.Bottom), base.MiddleZ);
                        break;

                    case ShapeStyles.InvertTriangle:
                        g.Triangle(new Point(r.X, r.Y), new Point(this.tmpMidX, r.Bottom), new Point(r.Right, r.Y), base.MiddleZ);
                        break;

                    case ShapeStyles.Line:
                        g.Line(r.X, r.Y, r.Right, r.Bottom, base.MiddleZ);
                        break;

                    case ShapeStyles.Diamond:
                        g.Plane(new Point(r.X, this.tmpMidY), new Point(this.tmpMidX, r.Y), new Point(r.Right, this.tmpMidY), new Point(this.tmpMidX, r.Bottom), base.MiddleZ);
                        break;

                    case ShapeStyles.Cube:
                        g.Cube(r, base.StartZ, base.EndZ, !this.Transparent);
                        break;

                    case ShapeStyles.Cross:
                        this.DrawCross3D(r);
                        break;

                    case ShapeStyles.DiagCross:
                        this.DrawDiagonalCross3D(r);
                        break;

                    case ShapeStyles.Star:
                        this.DrawCross3D(r);
                        this.DrawDiagonalCross3D(r);
                        break;

                    case ShapeStyles.Pyramid:
                        g.Pyramid(true, r, base.StartZ, base.EndZ, !this.Transparent);
                        break;

                    case ShapeStyles.InvertPyramid:
                        g.Pyramid(true, r.X, r.Bottom, r.Right, r.Y, base.StartZ, base.EndZ, !this.Transparent);
                        break;
                }
            }
            else
            {
                switch (this.style)
                {
                    case ShapeStyles.Rectangle:
                    {
                        if (this.Format.ShapeStyle != TextShapeStyle.RoundRectangle)
                        {
                            this.DoGradient(g, Is3D, r);
                            g.Rectangle(Utils.FromLTRB(r.X, r.Y, r.Right + 1, r.Bottom + 1));
                            break;
                        }
                        int roundWidth = ((r.Width > 12) && (r.Height > 12)) ? 12 : 2;
                        g.RoundRectangle(r, roundWidth, roundWidth);
                        break;
                    }
                    case ShapeStyles.Circle:
                        this.DoGradient(g, Is3D, r);
                        g.Ellipse(r);
                        break;

                    case ShapeStyles.VertLine:
                        g.VerticalLine(this.tmpMidX, r.Y, r.Bottom);
                        break;

                    case ShapeStyles.HorizLine:
                        g.HorizontalLine(r.X, r.Right + 1, this.tmpMidY);
                        break;

                    case ShapeStyles.Triangle:
                    case ShapeStyles.Pyramid:
                    {
                        Point[] p = new Point[] { new Point(r.X, r.Bottom), new Point(this.tmpMidX, r.Y), new Point(r.Right, r.Bottom) };
                        g.Polygon(p);
                        break;
                    }
                    case ShapeStyles.InvertTriangle:
                    case ShapeStyles.InvertPyramid:
                    {
                        Point[] pointArray2 = new Point[] { new Point(r.X, r.Y), new Point(this.tmpMidX, r.Bottom), new Point(r.Right, r.Y) };
                        g.Polygon(pointArray2);
                        break;
                    }
                    case ShapeStyles.Line:
                        g.Line(r.X, r.Y, r.Right, r.Bottom);
                        break;

                    case ShapeStyles.Diamond:
                    {
                        Point[] pointArray3 = new Point[] { new Point(r.X, this.tmpMidY), new Point(this.tmpMidX, r.Y), new Point(r.Right, this.tmpMidY), new Point(this.tmpMidX, r.Bottom) };
                        g.Polygon(pointArray3);
                        break;
                    }
                    case ShapeStyles.Cube:
                        g.Rectangle(r);
                        break;

                    case ShapeStyles.Cross:
                        this.DrawCross2D(r);
                        break;

                    case ShapeStyles.DiagCross:
                        this.DrawDiagonalCross2D(r);
                        break;

                    case ShapeStyles.Star:
                        this.DrawCross2D(r);
                        this.DrawDiagonalCross2D(r);
                        break;
                }
            }
            this.Brush.Visible = visible;
        }

        private void DrawText(Rectangle r)
        {
            int num = 4;
            int x = 0;
            int num3 = 0;
            int num4 = 0;
            int y = 0;
            int top = 0;
            int num7 = 0;
            Graphics3D graphicsd = base.Chart.Graphics3D;
            if (this.Text.Length > 0)
            {
                graphicsd.Font = this.Format.Font;
                num3 = Utils.Round(graphicsd.TextHeight(this.Font, "H"));
                Graphics3D.RectCenter(r, out num4, out y);
                switch (this.textVertAlign)
                {
                    case ShapeTextVertAlign.Top:
                        top = r.Top;
                        break;

                    case ShapeTextVertAlign.Center:
                        top = y - Utils.Round((double) (((double) (num3 * this.Text.Length)) / 2.0));
                        break;

                    default:
                        top = r.Bottom - (num3 * this.Text.Length);
                        break;
                }
                for (int i = 0; i < this.Text.Length; i++)
                {
                    num7 = Utils.Round(graphicsd.TextWidth(this.Text[i]));
                    switch (this.textHorizAlign)
                    {
                        case ShapeTextHorizAlign.Left:
                            x = (r.Left + this.Pen.Width) + num;
                            break;

                        case ShapeTextHorizAlign.Center:
                            x = num4 - (num7 / 2);
                            break;

                        default:
                            x = ((r.Right - this.Pen.Width) - num7) - num;
                            break;
                    }
                    if (this.XYStyle == ShapeXYStyles.Pixels)
                    {
                        graphicsd.TextOut(x, top, this.Text[i]);
                    }
                    else
                    {
                        graphicsd.TextOut(x, top, base.StartZ, this.Text[i]);
                    }
                    top += num3;
                }
            }
        }

        public override void DrawValue(int valueIndex)
        {
            if ((base.Count == 2) && (valueIndex == 0))
            {
                Rectangle adjustedRectangle = this.GetAdjustedRectangle();
                if (adjustedRectangle.IntersectsWith(base.chart.ChartRect))
                {
                    bool flag = (this.xyStyle != ShapeXYStyles.Pixels) && base.chart.aspect.View3D;
                    this.DrawShape(flag, (this.style == ShapeStyles.Line) ? this.GetShapeRectangle() : adjustedRectangle);
                    this.DrawText(adjustedRectangle);
                }
            }
        }

        private Rectangle GetAdjustedRectangle()
        {
            Rectangle shapeRectangle = this.GetShapeRectangle();
            base.chart.Graphics3D.OrientRectangle(ref shapeRectangle);
            if (shapeRectangle.Top == shapeRectangle.Bottom)
            {
                shapeRectangle.Height = 1;
            }
            if (shapeRectangle.Left == shapeRectangle.Right)
            {
                shapeRectangle.Width = 1;
            }
            return shapeRectangle;
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        private Rectangle GetShapeRectangle()
        {
            int num;
            int num2;
            int num3;
            int num4;
            switch (this.xyStyle)
            {
                case ShapeXYStyles.Pixels:
                    num = Utils.Round(this.X0);
                    num3 = Utils.Round(this.Y0);
                    num2 = Utils.Round(this.X1);
                    num4 = Utils.Round(this.Y1);
                    break;

                case ShapeXYStyles.Axis:
                    num = this.CalcXPos(0);
                    num3 = this.CalcYPos(0);
                    num2 = this.CalcXPos(1);
                    num4 = this.CalcYPos(1);
                    break;

                default:
                    num = this.CalcXPos(0);
                    num3 = this.CalcYPos(0);
                    num2 = num + Utils.Round(this.X1);
                    num4 = num3 + Utils.Round(this.Y1);
                    break;
            }
            return Utils.FromLTRB(num, num3, num2, num4);
        }

        public override bool IsValidSourceOf(Series s)
        {
            return (s is Steema.TeeChart.Styles.Shape);
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            if (!base.ColorEach)
            {
                Color color2 = Utils.DarkenColor(color, 60);
                this.Pen.Color = color2;
            }
        }

        protected override bool MoreSameZOrder()
        {
            return false;
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.Font.Color = Color.White;
            this.Font.Size = 12;
            this.shape.Lines = new string[1];
            base.FillSampleValues();
            if (base.chart.Series.IndexOf(this) == 1)
            {
                this.style = ShapeStyles.Circle;
                this.Text[0] = Texts.ShapeGallery1;
            }
            else
            {
                this.style = ShapeStyles.Triangle;
                this.Text[0] = Texts.ShapeGallery2;
            }
        }

        protected override void SetChart(Chart c)
        {
            if ((this.shape != null) && (this.shape.chart != c))
            {
                this.shape.Chart = c;
            }
            base.SetChart(c);
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.Style = ShapeStyles.Rectangle;
                    return;

                case 2:
                    this.Style = ShapeStyles.VertLine;
                    return;

                case 3:
                    this.Style = ShapeStyles.HorizLine;
                    return;

                case 4:
                    this.Style = ShapeStyles.Circle;
                    return;

                case 5:
                    this.Style = ShapeStyles.InvertTriangle;
                    return;

                case 6:
                    this.Style = ShapeStyles.Line;
                    return;

                case 7:
                    this.Style = ShapeStyles.Diamond;
                    return;

                case 8:
                    this.Style = ShapeStyles.Cube;
                    return;

                case 9:
                    this.Style = ShapeStyles.Cross;
                    return;

                case 10:
                    this.Style = ShapeStyles.DiagCross;
                    return;

                case 11:
                    this.Style = ShapeStyles.Star;
                    return;

                case 12:
                    this.Style = ShapeStyles.Pyramid;
                    return;

                case 13:
                    this.Style = ShapeStyles.InvertPyramid;
                    return;

                case 14:
                    this.Transparent = !this.Transparent;
                    return;
            }
            base.SetSubGallery(index);
        }

        [Description("Defines the brush used to fill shape background."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                return this.shape.Brush;
            }
        }

        [Description("")]
        public override string Description
        {
            get
            {
                return Texts.GalleryShape;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines the font attributes used to output ShapeSeries.")]
        public ChartFont Font
        {
            get
            {
                return this.shape.Font;
            }
        }

        public TextShape Format
        {
            get
            {
                return this.shape;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Gets Gradient fill characteristics for the ShapeSeries Shape.")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.shape.Gradient;
            }
        }

        [Description("Horizontally aligns the text.")]
        public ShapeTextHorizAlign HorizAlignment
        {
            get
            {
                return this.textHorizAlign;
            }
            set
            {
                if (this.textHorizAlign != value)
                {
                    this.textHorizAlign = value;
                }
                base.Repaint();
            }
        }

        [Category("Appearance"), Description("Defines pen to draw Series Shape."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen Pen
        {
            get
            {
                return this.shape.Pen;
            }
        }

        [Description("Defines how a TChartShape component appears on a Chart.")]
        public ShapeStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
                this.Invalidate();
            }
        }

        [Description("Displays customized strings inside Shapes.")]
        public string[] Text
        {
            get
            {
                if (this.shape.Lines == null)
                {
                    this.shape.Lines = new string[0];
                }
                return this.shape.Lines;
            }
            set
            {
                this.shape.Lines = value;
            }
        }

        [Description("Allows Shape Brush attributes to fill the Shape interior."), DefaultValue(false)]
        public bool Transparent
        {
            get
            {
                return this.shape.Transparent;
            }
            set
            {
                this.shape.Transparent = value;
            }
        }

        [Description("Sets the vertical alignment of Text within a TChartShape Series shape.")]
        public ShapeTextVertAlign VertAlignment
        {
            get
            {
                return this.textVertAlign;
            }
            set
            {
                if (this.textVertAlign != value)
                {
                    this.textVertAlign = value;
                }
                base.Repaint();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Description("Coordinate used to define the englobing ShapeSeries rectangle.")]
        public double X0
        {
            get
            {
                return this.x0;
            }
            set
            {
                this.x0 = value;
            }
        }

        [Description("Coordinate used to define the englobing ShapeSeries rectangle."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double X1
        {
            get
            {
                return this.x1;
            }
            set
            {
                this.x1 = value;
            }
        }

        [Description("")]
        public ShapeXYStyles XYStyle
        {
            get
            {
                return this.xyStyle;
            }
            set
            {
                if (this.xyStyle != value)
                {
                    this.xyStyle = value;
                    base.Repaint();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Description("Coordinate used to define the englobing ShapeSeries rectangle.")]
        public double Y0
        {
            get
            {
                return this.y0;
            }
            set
            {
                this.y0 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Description("Coordinate used to define the englobing ShapeSeries rectangle.")]
        public double Y1
        {
            get
            {
                return this.y1;
            }
            set
            {
                this.y1 = value;
            }
        }
    }
}

