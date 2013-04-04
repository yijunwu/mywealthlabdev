namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class Graphics3DHotSpot : Graphics3D
    {
        private ArrayList bounds;
        private int clickTolerance;
        protected int currentX;
        protected int currentY;
        private Steema.TeeChart.Styles.PolygonStyle polygonStyle;
        private GraphicsPath tmpPath;

        public Graphics3DHotSpot(Chart c) : base(c)
        {
            this.bounds = new ArrayList();
            this.tmpPath = new GraphicsPath();
            this.clickTolerance = 2;
            base.iCanvasType = CanvasType.HotSpot;
        }

        public void AddBounds(Steema.TeeChart.Styles.PolygonStyle PStyle, params Point[] Points)
        {
            foreach (Point point in Points)
            {
                this.Bounds.Add(point.X);
                this.Bounds.Add(point.Y);
            }
            this.PolygonStyle = PStyle;
        }

        public void AddBounds(Steema.TeeChart.Styles.PolygonStyle PStyle, params PointF[] Points)
        {
            foreach (PointF tf in Points)
            {
                this.Bounds.Add(Convert.ToInt32(tf.X));
                this.Bounds.Add(Convert.ToInt32(tf.Y));
            }
            this.PolygonStyle = PStyle;
        }

        public void AddBounds(Steema.TeeChart.Styles.PolygonStyle PStyle, int[] Points)
        {
            foreach (int num in Points)
            {
                this.Bounds.Add(num);
            }
            this.PolygonStyle = PStyle;
        }

        public override void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle)
        {
        }

        public override void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
        }

        public override void ClearClipRegions()
        {
        }

        public override void ClipCube(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            base.ClipCube(rect, minZ, maxZ);
        }

        public override void ClipEllipse(System.Drawing.Rectangle r)
        {
        }

        public override void ClipPolygon(params Point[] p)
        {
        }

        public override void ClipRectangle(System.Drawing.Rectangle r)
        {
        }

        protected override void DoDrawString(int x, int y, string text, ChartBrush aBrush)
        {
        }

        public override void Draw(System.Drawing.Rectangle r, Image image, bool transparent)
        {
        }

        public override void Draw(int x, int y, Image image)
        {
        }

        public override void DrawBeziers(params Point[] p)
        {
        }

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
        }

        public override void Ellipse(int x1, int y1, int x2, int y2)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            if ((rect.Height == rect.Width) && (this.PolygonStyle != Steema.TeeChart.Styles.PolygonStyle.Poly))
            {
                this.AddBounds(Steema.TeeChart.Styles.PolygonStyle.Circle, this.GetCircleFromRectangle(rect));
            }
            else
            {
                this.tmpPath.AddEllipse(rect);
                this.tmpPath.Flatten();
                this.AddBounds(Steema.TeeChart.Styles.PolygonStyle.Poly, this.tmpPath.PathPoints);
                this.tmpPath.Reset();
            }
        }

        public override void EndBlending()
        {
            base.EndBlending();
        }

        public override void EraseBackground(int left, int top, int right, int bottom)
        {
        }

        public override void FillRegion(Brush brush, Region region)
        {
        }

        public int[] GetBounds()
        {
            int[] numArray = (int[]) this.Bounds.ToArray(typeof(int));
            this.Bounds.Clear();
            return numArray;
        }

        public override Region GetChartPolygon(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            return base.GetChartPolygon(rect, minZ, maxZ);
        }

        private int[] GetCircleFromRectangle(System.Drawing.Rectangle Rect)
        {
            int num = Rect.Left + ((Rect.Right - Rect.Left) / 2);
            int num2 = Rect.Top + ((Rect.Bottom - Rect.Top) / 2);
            int num3 = Rect.Height / 2;
            return new int[] { num, num2, num3 };
        }

        public override void HorizontalLine(int left, int right, int y)
        {
        }

        protected internal override void InitWindow(Graphics graphics, Aspect a, System.Drawing.Rectangle r, int MaxDepth)
        {
            base.g = graphics;
            base.InitWindow(graphics, a, r, MaxDepth);
        }

        protected override void InternalCylinder(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool dark3D, int conePercent)
        {
            base.InternalCylinder(vertical, r, z0, z1, dark3D, conePercent);
        }

        public override void Invalidate()
        {
            base.Invalidate();
        }

        protected override void Line(ChartPen p, Point a, Point b)
        {
        }

        public override void Line(int x0, int y0, int x1, int y1)
        {
            this.tmpPath.AddLine(x0, y0, x1, y1);
            if ((x0 != x1) && (y0 != y1))
            {
                this.tmpPath.Widen(new Pen(Utils.EmptyColor, (float) this.ClickTolerance));
            }
            this.tmpPath.Flatten();
            PointF[] pathPoints = this.tmpPath.PathPoints;
            PointF[] array = new PointF[pathPoints.Length + 1];
            pathPoints.CopyTo(array, 0);
            array.SetValue(pathPoints[0], pathPoints.Length);
            this.AddBounds(Steema.TeeChart.Styles.PolygonStyle.Poly, array);
            this.tmpPath.Reset();
        }

        public override void LineTo(int x, int y)
        {
            this.tmpPath.AddLine(this.currentX, this.currentY, x, y);
            if ((this.currentX != x) && (this.currentY != y))
            {
                this.tmpPath.Widen(new Pen(Utils.EmptyColor, (float) this.ClickTolerance));
            }
            this.MoveTo(x, y);
            this.AddBounds(Steema.TeeChart.Styles.PolygonStyle.Poly, this.tmpPath.PathPoints);
            this.tmpPath.Reset();
        }

        public override SizeF MeasureString(ChartFont f, string text)
        {
            return new SizeF();
        }

        public override void MoveTo(int x, int y)
        {
            this.currentX = x;
            this.currentY = y;
        }

        public override void Pixel(int x, int y, int z, Color color)
        {
            base.Calc3DPos(ref x, ref y, z);
            this.tmpPath.AddLine(x, y, x + 1, y + 1);
            this.tmpPath.Widen(new Pen(Utils.EmptyColor, (float) this.ClickTolerance));
            this.AddBounds(Steema.TeeChart.Styles.PolygonStyle.Poly, this.tmpPath.PathPoints);
            this.tmpPath.Reset();
        }

        public override void Polygon(params PointDouble[] p)
        {
            this.Polygon(PointDouble.Round(p));
        }

        public override void Polygon(params Point[] p)
        {
            this.AddBounds(Steema.TeeChart.Styles.PolygonStyle.Poly, p);
        }

        protected internal override void PolygonFour()
        {
            Point[] p = new Point[] { base.IPoints[0], base.IPoints[1], base.IPoints[2], base.IPoints[3], base.IPoints[0] };
            this.Polygon(p);
        }

        public override void Polyline(params Point[] p)
        {
        }

        public override void Polyline(int z, params Point[] p)
        {
        }

        public override void PrepareDrawImage()
        {
        }

        public override void Rectangle(System.Drawing.Rectangle r)
        {
            if (this.PolygonStyle == Steema.TeeChart.Styles.PolygonStyle.Poly)
            {
                int[] points = new int[] { r.X, r.Y, r.X + r.Width, r.Y, r.X + r.Width, r.Y + r.Height, r.X, r.Y + r.Height, r.X, r.Y };
                this.AddBounds(Steema.TeeChart.Styles.PolygonStyle.Poly, points);
            }
            else
            {
                int[] numArray2 = new int[] { r.Left, r.Top, r.Right, r.Bottom };
                this.AddBounds(Steema.TeeChart.Styles.PolygonStyle.Rect, numArray2);
            }
        }

        public override void RectangleY(int left, int top, int right, int z0, int z1)
        {
        }

        public override void RectangleZ(int left, int top, int bottom, int z0, int z1)
        {
        }

        public override void RotateLabel(int x, int y, string text, double rotDegree)
        {
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
        }

        public override void SetClipRegion(Region region)
        {
        }

        public override void SetTextAlign(StringAlignment a)
        {
        }

        public override void ShowImage(Graphics g)
        {
            base.ShowImage(g);
        }

        protected internal override void TransparentEllipse(int x1, int y1, int x2, int y2)
        {
        }

        public override void UnClip()
        {
        }

        public override void VerticalLine(int x, int top, int bottom)
        {
        }

        public ArrayList Bounds
        {
            get
            {
                return this.bounds;
            }
            set
            {
                this.bounds = value;
            }
        }

        public int ClickTolerance
        {
            get
            {
                return this.clickTolerance;
            }
            set
            {
                this.clickTolerance = value;
            }
        }

        public Steema.TeeChart.Styles.PolygonStyle PolygonStyle
        {
            get
            {
                return this.polygonStyle;
            }
            set
            {
                this.polygonStyle = value;
            }
        }
    }
}

