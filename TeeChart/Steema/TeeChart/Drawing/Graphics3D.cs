namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Themes;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public abstract class Graphics3D : TeeBase
    {
        protected System.Drawing.Drawing2D.SmoothingMode aSmoothingMode;
        protected internal Aspect aspect;
        protected System.Drawing.Text.TextRenderingHint aTextRenderingHint;
        private BufferedGraphics backBuffer;
        private BufferedGraphicsContext backBufferContext;
        private ChartBrush bBrush;
        private System.Drawing.Rectangle bounds;
        private bool buffered;
        private const int BytesPerPixel = 4;
        private double c1;
        private double c2;
        private double c2c1;
        private double c2c3;
        private double c2s1;
        private double c2s3;
        private double c3;
        private static Color[] colorPalette;
        internal const int DarkColorQuantity = 0x40;
        internal const int DarkerColorQuantity = 0x80;
        internal bool Dirty;
        public Point[] FourPoints;
        protected internal Graphics g;
        protected internal int gradientZ;
        protected internal bool hasClipRegion;
        protected Steema.TeeChart.Drawing.CanvasType iCanvasType;
        private int iDisabledRotation;
        protected ChartFont ifont;
        private bool IHasPerspec;
        private bool IHasTilt;
        private const double Inv255 = 0.00392156862745098;
        private double IOrthoX;
        private double IOrthoY;
        public PointDouble[] IPointDoubles;
        protected internal Point[] IPoints;
        protected Point3DDouble irotationCenter;
        private bool is3D;
        protected internal bool IsOpenGL;
        private bool isOrthogonal;
        private double IZoomFactor;
        private double IZoomPerspec;
        private bool IZoomText;
        private SizeF layoutArea;
        internal bool metafiling;
        public bool Monochrome;
        protected internal Region oldRegion;
        private ChartPen pen;
        private const double PerspecFactor = 0.0066666666666666671;
        private Pie3D pie3D;
        public const double PiStep = 0.017453292519943295;
        private double s1;
        private double s2;
        private double s3;
        protected StringFormat stringFormat;
        private bool supports3DText;
        private double tempXX;
        private double tempXZ;
        private double tempYX;
        private double tempYZ;
        private const int WeightFactor = 0x186a0;
        private int xCenter;
        private int xCenterOffset;
        private int yCenter;
        private int yCenterOffset;
        private int zCenter;

        public Graphics3D(Chart c) : base(c)
        {
            this.FourPoints = new Point[4];
            this.aTextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.aSmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.stringFormat = new StringFormat();
            this.buffered = true;
            this.irotationCenter = new Point3DDouble();
            this.IPoints = new Point[4];
            this.IPointDoubles = new PointDouble[4];
            this.Dirty = true;
            this.layoutArea = new SizeF(1000f, 1000f);
            this.oldRegion = new Region(new System.Drawing.Rectangle(0, 0, 0, 0));
            this.Pen.chart = c;
            this.Brush.chart = c;
            this.Font.chart = c;
            this.aspect = base.chart.aspect;
        }

        private Region AddRightRegion(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            Point[] points = new Point[6];
            points[0] = this.Calc3DPoint(rect.X, rect.Bottom, minZ);
            points[1] = this.Calc3DPoint(rect.X, rect.Y, minZ);
            Point point = this.Calc3DPoint(rect.X, rect.Y, maxZ);
            Point point2 = this.Calc3DPoint(rect.Right, rect.Y, minZ);
            points[2] = (point2.Y < point.Y) ? point2 : point;
            points[3] = this.Calc3DPoint(rect.Right, rect.Y, maxZ);
            point = this.Calc3DPoint(rect.Right, rect.Bottom, maxZ);
            point2 = this.Calc3DPoint(rect.Right, rect.Y, minZ);
            points[4] = (point2.X > point.X) ? point2 : point;
            points[5] = this.Calc3DPoint(rect.Right, rect.Bottom, minZ);
            if (points[5].X < points[0].X)
            {
                points[0].X = points[5].X;
                if (point2.Y < points[0].Y)
                {
                    points[0].Y = point2.Y;
                }
            }
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(points);
            return new Region(path);
        }

        public static void ApplyBright(ref Color c, byte howMuch)
        {
            byte r = c.R;
            byte g = c.G;
            byte b = c.B;
            if ((r + howMuch) < 0x100)
            {
                r = (byte) (r + howMuch);
            }
            else
            {
                r = 0xff;
            }
            if ((g + howMuch) < 0x100)
            {
                g = (byte) (g + howMuch);
            }
            else
            {
                g = 0xff;
            }
            if ((b + howMuch) < 0x100)
            {
                b = (byte) (b + howMuch);
            }
            else
            {
                b = 0xff;
            }
            c = Utils.FromArgb(r, g, b);
        }

        public static void ApplyDark(ref Color c, byte howMuch)
        {
            byte r = c.R;
            byte g = c.G;
            byte b = c.B;
            if (r > howMuch)
            {
                r = (byte) (r - howMuch);
            }
            else
            {
                r = 0;
            }
            if (g > howMuch)
            {
                g = (byte) (g - howMuch);
            }
            else
            {
                g = 0;
            }
            if (b > howMuch)
            {
                b = (byte) (b - howMuch);
            }
            else
            {
                b = 0;
            }
            c = Utils.FromArgb(r, g, b);
        }

        public void Arc(System.Drawing.Rectangle rect, float startAngle, float sweepAngle)
        {
            this.Arc(rect.Left, rect.Top, rect.Right, rect.Bottom, startAngle, sweepAngle);
        }

        public abstract void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle);
        public abstract void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4);
        public virtual void Arrow(bool filled, Point fromPoint, Point toPoint, int headWidth, int headHeight, int z)
        {
            int num = toPoint.X - fromPoint.X;
            int num2 = fromPoint.Y - toPoint.Y;
            double num3 = Math.Sqrt((double) ((num * num) + (num2 * num2)));
            if (num3 > 0.0)
            {
                ArrowPoint point = new ArrowPoint {
                    z = z,
                    g = this
                };
                int num4 = headWidth;
                int num5 = Math.Min(Utils.Round(num3), headHeight);
                point.SinA = ((double) num2) / num3;
                point.CosA = ((double) num) / num3;
                double num6 = (toPoint.X * point.CosA) - (toPoint.Y * point.SinA);
                double num7 = (toPoint.X * point.SinA) + (toPoint.Y * point.CosA);
                point.x = num6 - num5;
                point.y = num7 - (num4 * 0.5);
                Point p = point.Calc();
                point.y = num7 + (num4 * 0.5);
                Point point3 = point.Calc();
                if (filled)
                {
                    double num8 = num4 * 0.25;
                    point.y = num7 - num8;
                    Point point4 = point.Calc();
                    point.y = num7 + num8;
                    Point point5 = point.Calc();
                    point.x = (fromPoint.X * point.CosA) - (fromPoint.Y * point.SinA);
                    point.y = num7 - num8;
                    Point point6 = point.Calc();
                    point.y = num7 + num8;
                    Point point7 = point.Calc();
                    Point[] pointArray = new Point[] { point7, point6, point4, p, this.Calc3DPoint(toPoint, z), point3, point5 };
                    this.Polygon(pointArray);
                }
                else
                {
                    this.MoveTo(fromPoint, z);
                    this.LineTo(toPoint, z);
                    this.LineTo(point3, z);
                    this.MoveTo(toPoint, z);
                    this.LineTo(p, z);
                }
            }
        }

        public Point Calc3DPoint(Point3D p)
        {
            return this.Calc3DPoint(p.X, p.Y, p.Z);
        }

        public Point Calc3DPoint(Point p, int z)
        {
            this.Calc3DPos(ref p, p.X, p.Y, z);
            return p;
        }

        public Point Calc3DPoint(int x, int y, int z)
        {
            this.Calc3DPos(ref x, ref y, z);
            return new Point(x, y);
        }

        public void Calc3DPos(ref PointDouble p, PointDouble source, double z)
        {
            this.Calc3DPos(ref p, source.X, source.Y, z);
        }

        public void Calc3DPos(ref double x, ref double y, double z)
        {
            if (this.aspect.orthogonal)
            {
                x = (this.IZoomFactor * ((x - this.xCenter) + (this.IOrthoX * z))) + this.xCenterOffset;
                y = (this.IZoomFactor * ((y - this.yCenter) - (this.IOrthoY * z))) + this.yCenterOffset;
            }
            else if (this.aspect.view3D)
            {
                double iZoomFactor;
                z -= this.zCenter;
                x -= this.xCenter;
                y -= this.yCenter;
                double num3 = (z * this.c2) - (x * this.s2);
                if (this.IHasPerspec)
                {
                    iZoomFactor = this.IZoomFactor / (1.0 + (this.IZoomPerspec * ((num3 * this.c1) + (y * this.s1))));
                }
                else
                {
                    iZoomFactor = this.IZoomFactor;
                }
                if (this.IHasTilt)
                {
                    double num = (x * this.c2) + (z * this.s2);
                    double num2 = (y * this.c1) - (num3 * this.s1);
                    x = (((num * this.c3) - (num2 * this.s3)) * iZoomFactor) + this.xCenterOffset;
                    y = (((num2 * this.c3) + (num * this.s3)) * iZoomFactor) + this.yCenterOffset;
                }
                else
                {
                    x = (((x * this.c2) + (z * this.s2)) * iZoomFactor) + this.xCenterOffset;
                    y = (((y * this.c1) - (num3 * this.s1)) * iZoomFactor) + this.yCenterOffset;
                }
            }
        }

        public void Calc3DPos(ref Point p, Point source, int z)
        {
            this.Calc3DPos(ref p, source.X, source.Y, z);
        }

        public void Calc3DPos(ref int x, ref int y, int z)
        {
            if (this.aspect.orthogonal)
            {
                x = Utils.Round((double) (this.IZoomFactor * (((double) (x - this.xCenter)) + (this.IOrthoX * z)))) + this.xCenterOffset;
                y = Utils.Round((double) (this.IZoomFactor * (((double) (y - this.yCenter)) - (this.IOrthoY * z)))) + this.yCenterOffset;
            }
            else if (this.aspect.view3D)
            {
                double iZoomFactor;
                z -= this.zCenter;
                x -= this.xCenter;
                y -= this.yCenter;
                double num3 = (z * this.c2) - (((double) x) * this.s2);
                if (this.IHasPerspec)
                {
                    iZoomFactor = this.IZoomFactor / (1.0 + (this.IZoomPerspec * ((num3 * this.c1) + (((double) y) * this.s1))));
                }
                else
                {
                    iZoomFactor = this.IZoomFactor;
                }
                if (this.IHasTilt)
                {
                    double num = (((double) x) * this.c2) + (z * this.s2);
                    double num2 = (((double) y) * this.c1) - (num3 * this.s1);
                    x = Utils.Round((double) (((num * this.c3) - (num2 * this.s3)) * iZoomFactor)) + this.xCenterOffset;
                    y = Utils.Round((double) (((num2 * this.c3) + (num * this.s3)) * iZoomFactor)) + this.yCenterOffset;
                }
                else
                {
                    x = Utils.Round((double) (((((double) x) * this.c2) + (z * this.s2)) * iZoomFactor)) + this.xCenterOffset;
                    y = Utils.Round((double) (((((double) y) * this.c1) - (num3 * this.s1)) * iZoomFactor)) + this.yCenterOffset;
                }
            }
        }

        public void Calc3DPos(ref float x, ref float y, int z)
        {
            if (this.aspect.orthogonal)
            {
                x = ((float) (this.IZoomFactor * ((x - this.xCenter) + (this.IOrthoX * z)))) + this.xCenterOffset;
                y = ((float) (this.IZoomFactor * ((y - this.yCenter) - (this.IOrthoY * z)))) + this.yCenterOffset;
            }
            else if (this.aspect.view3D)
            {
                double iZoomFactor;
                z -= this.zCenter;
                x -= this.xCenter;
                y -= this.yCenter;
                float num3 = (float) ((z * this.c2) - (((double) x) * this.s2));
                if (this.IHasPerspec)
                {
                    iZoomFactor = this.IZoomFactor / (1.0 + (this.IZoomPerspec * ((num3 * this.c1) + (((double) y) * this.s1))));
                }
                else
                {
                    iZoomFactor = this.IZoomFactor;
                }
                if (this.IHasTilt)
                {
                    float num = (float) ((((double) x) * this.c2) + (z * this.s2));
                    float num2 = (float) ((((double) y) * this.c1) - (num3 * this.s1));
                    x = ((float) (((num * this.c3) - (num2 * this.s3)) * iZoomFactor)) + this.xCenterOffset;
                    y = ((float) (((num2 * this.c3) + (num * this.s3)) * iZoomFactor)) + this.yCenterOffset;
                }
                else
                {
                    x = ((float) (((((double) x) * this.c2) + (z * this.s2)) * iZoomFactor)) + this.xCenterOffset;
                    y = ((float) (((((double) y) * this.c1) - (num3 * this.s1)) * iZoomFactor)) + this.yCenterOffset;
                }
            }
        }

        public virtual void Calc3DPos(ref PointDouble p, double x, double y, double z)
        {
            this.Calc3DPos(ref x, ref y, z);
            p.X = x;
            p.Y = y;
        }

        public virtual void Calc3DPos(ref Point p, int x, int y, int z)
        {
            this.Calc3DPos(ref x, ref y, z);
            p.X = x;
            p.Y = y;
        }

        protected void CalcArcAngles(int X1, int Y1, int X2, int Y2, int X3, int Y3, int X4, int Y4, out double startangle, out double sweepangle)
        {
            int num = (X2 + X1) / 2;
            int num2 = (Y2 + Y1) / 2;
            int num1 = (X2 - X1) / 2;
            int num3 = (Y2 - Y1) / 2;
            startangle = Math.Atan2((double) (num2 - Y3), (double) (X3 - num));
            if (startangle < 0.0)
            {
                startangle += 6.2831853071795862;
            }
            startangle *= 57.295779513082323;
            sweepangle = Math.Atan2((double) (num2 - Y4), (double) (X4 - num));
            if (sweepangle < 0.0)
            {
                sweepangle += 6.2831853071795862;
            }
            sweepangle *= 57.295779513082323;
            sweepangle -= startangle;
        }

        protected void CalcArcPoints(int X1, int Y1, int X2, int Y2, double startAngle, double sweepAngle, out int X3, out int Y3, out int X4, out int Y4)
        {
            double d = startAngle + sweepAngle;
            int num2 = (X2 + X1) / 2;
            int num3 = (Y2 + Y1) / 2;
            int num4 = (X2 - X1) / 2;
            int num5 = (Y2 - Y1) / 2;
            startAngle *= 0.017453292519943295;
            d *= 0.017453292519943295;
            X3 = num2 + Utils.Round((double) (num4 * Math.Cos(startAngle)));
            Y3 = num3 - Utils.Round((double) (num5 * Math.Sin(startAngle)));
            X4 = num2 + Utils.Round((double) (num4 * Math.Cos(d)));
            Y4 = num3 - Utils.Round((double) (num5 * Math.Sin(d)));
        }

        public static void CalcBitmap(RGB[,] Lines, out Bitmap bitmap)
        {
            int length = Lines.GetLength(0);
            int width = Lines.GetLength(1);
            bitmap = new Bitmap(width, length);
            if ((length > 1) && (width > 1))
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(new Point(0, 0), bitmap.Size);
                BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
                IntPtr ptr = bitmapdata.Scan0;
                int stride = bitmapdata.Stride;
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int ofs = (i * stride) + (4 * j);
                        Marshal.WriteByte(ptr, ofs + 3, Lines[i, j].Alpha);
                        Marshal.WriteByte(ptr, ofs + 2, Lines[i, j].Red);
                        Marshal.WriteByte(ptr, ofs + 1, Lines[i, j].Green);
                        Marshal.WriteByte(ptr, ofs, Lines[i, j].Blue);
                    }
                }
                bitmap.UnlockBits(bitmapdata);
            }
        }

        private static LineOrientations CalcLineParameters(Point PointA, Point PointB, out double Slope, out double Intercept)
        {
            Point point = new Point(PointA.X - PointB.X, PointA.Y - PointB.Y);
            if ((point.X == 0) && (point.Y == 0))
            {
                Slope = 0.0;
                Intercept = 0.0;
                return LineOrientations.Point;
            }
            int introduced1 = Math.Abs(point.X);
            if (introduced1 >= Math.Abs(point.Y))
            {
                try
                {
                    double introduced2 = Convert.ToDouble(point.Y);
                    Slope = introduced2 / Convert.ToDouble(point.X);
                }
                catch
                {
                    Slope = 0.0;
                }
                Intercept = PointA.Y - (PointA.X * Slope);
                return LineOrientations.Horizontal;
            }
            try
            {
                double introduced4 = Convert.ToDouble(point.X);
                Slope = introduced4 / Convert.ToDouble(point.Y);
            }
            catch
            {
                Slope = 0.0;
            }
            Intercept = PointA.X - (PointA.Y * Slope);
            return LineOrientations.Vertical;
        }

        public static void CalcLines(out RGB[,] Lines, Bitmap bitmap)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            Lines = new RGB[height, width];
            if ((height > 1) && (width > 1))
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(new Point(0, 0), bitmap.Size);
                BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
                IntPtr ptr = bitmapdata.Scan0;
                int stride = bitmapdata.Stride;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int ofs = (i * stride) + (4 * j);
                        RGB rgb = new RGB {
                            Alpha = Marshal.ReadByte(ptr, ofs + 3),
                            Red = Marshal.ReadByte(ptr, ofs + 2),
                            Green = Marshal.ReadByte(ptr, ofs + 1),
                            Blue = Marshal.ReadByte(ptr, ofs)
                        };
                        Lines[i, j] = rgb;  ///WYJ fix, original: *(Lines[i, j]) = rgb;  
                    }
                }
                bitmap.UnlockBits(bitmapdata);
            }
        }

        internal void CalcPerspective(System.Drawing.Rectangle r)
        {
            this.IHasPerspec = this.aspect.Perspective > 0;
            if (this.IHasPerspec)
            {
                int num = r.Right - r.Left;
                if (num < 1)
                {
                    num = 1;
                }
                this.IZoomPerspec = (this.IZoomFactor * this.aspect.Perspective) * (0.0066666666666666671 / ((double) num));
            }
        }

        public System.Drawing.Rectangle CalcRect3D(System.Drawing.Rectangle r, int z)
        {
            int x = r.X;
            int y = r.Y;
            this.Calc3DPos(ref x, ref y, z);
            int right = r.Right;
            int bottom = r.Bottom;
            this.Calc3DPos(ref right, ref bottom, z);
            r.X = x;
            r.Y = y;
            r.Width = right - r.X;
            r.Height = bottom - r.Y;
            return r;
        }

        public System.Drawing.RectangleF CalcRect3D(System.Drawing.RectangleF r, int z)
        {
            float x = r.X;
            float y = r.Y;
            this.Calc3DPos(ref x, ref y, z);
            float right = r.Right;
            float bottom = r.Bottom;
            this.Calc3DPos(ref right, ref bottom, z);
            r.X = x;
            r.Y = y;
            r.Width = right - r.X;
            r.Height = bottom - r.Y;
            return r;
        }

        internal void CalcTrigValues()
        {
            double num2;
            double tilt;
            double num = 0.0;
            if (!this.aspect.orthogonal)
            {
                num2 = this.aspect.Rotation * -1;
                num = this.aspect.Elevation * -1;
                tilt = this.aspect.Tilt;
            }
            else
            {
                num = 0.0;
                num2 = 0.0;
                tilt = 0.0;
            }
            this.IHasPerspec = false;
            this.IZoomPerspec = 0.0;
            this.s1 = Math.Sin(num * 0.017453292519943295);
            this.c1 = Math.Cos(num * 0.017453292519943295);
            this.s2 = Math.Sin(num2 * 0.017453292519943295);
            this.c2 = Math.Cos(num2 * 0.017453292519943295);
            this.s3 = Math.Sin(tilt * 0.017453292519943295);
            this.c3 = Math.Cos(tilt * 0.017453292519943295);
            this.IHasTilt = tilt != 0.0;
            this.c2s3 = this.c2 * this.s3;
            this.c2c3 = Math.Max((double) 1E-05, (double) (this.c2 * this.c3));
            this.tempXX = Math.Max((double) 1E-05, (double) (((this.s1 * this.s2) * this.s3) + (this.c1 * this.c3)));
            this.tempYX = ((this.c3 * this.s1) * this.s2) - (this.c1 * this.s3);
            this.tempXZ = ((this.c1 * this.s2) * this.s3) - (this.c3 * this.s1);
            this.tempYZ = ((this.c1 * this.c3) * this.s2) + (this.s1 * this.s3);
            this.c2s1 = this.c2 * this.s1;
            this.c2c1 = this.c1 * this.c2;
        }

        public void Calculate2DPosition(ref int x, ref int y, int z)
        {
            if (this.IZoomFactor != 0.0)
            {
                double num2 = 1.0 / this.IZoomFactor;
                if (this.aspect.orthogonal)
                {
                    x = Utils.Round((double) ((((double) (x - this.xCenterOffset)) * num2) - (this.IOrthoX * z))) + this.xCenter;
                    y = Utils.Round((double) ((((double) (y - this.yCenterOffset)) * num2) + (this.IOrthoY * z))) + this.yCenter;
                }
                else if ((this.aspect.view3D && (this.tempXX != 0.0)) && (this.c2c3 != 0.0))
                {
                    int num = x;
                    z -= this.zCenter;
                    x = Utils.Round((double) (((((num - this.xCenterOffset) * num2) - (z * this.tempXZ)) - (((double) (y - this.yCenter)) * this.c2s3)) / this.tempXX)) + this.xCenter;
                    y = Utils.Round((double) ((((((double) (y - this.yCenterOffset)) * num2) - (z * this.tempYZ)) - ((num - this.xCenter) * this.tempYX)) / this.c2c3)) + this.yCenter;
                }
            }
        }

        public Point Calculate3DPosition(Point3D P)
        {
            return this.Calculate3DPosition(P.X, P.Y, P.Z);
        }

        public Point Calculate3DPosition(Point P, int z)
        {
            return this.Calculate3DPosition(P.X, P.Y, z);
        }

        public Point Calculate3DPosition(int x, int y, int z)
        {
            this.Calc3DPos(ref x, ref y, z);
            return new Point(x, y);
        }

        private static int CalcWeight(int x, int middle, int width, double smoothness)
        {
            if (width == 0)
            {
                return 0x186a0;
            }
            return Utils.Round((double) (Math.Pow(smoothness, -(1.0 / ((double) width)) * Utils.Sqr((double) (middle - x))) * 100000.0));
        }

        public virtual void Changed(object o)
        {
        }

        public abstract void ClearClipRegions();
        public virtual void ClipCube(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            if (this.aspect.View3D)
            {
                Aspect aspect = this.aspect;
                if ((aspect.Elevation == 270) && ((aspect.Rotation == 270) || (aspect.Rotation == 360)))
                {
                    Point point = this.Calc3DPoint(rect.X, rect.Y, minZ);
                    Point point2 = this.Calc3DPoint(rect.Right, rect.Y, maxZ);
                    this.ClipRectangle(point.X, point.Y, point2.X, point2.Y);
                }
                else if (aspect.Orthogonal)
                {
                    if (aspect.OrthoAngle > 90)
                    {
                        this.ClipToLeft(rect, minZ, maxZ);
                    }
                    else
                    {
                        this.ClipToRight(rect, minZ, maxZ);
                    }
                }
                else if (aspect.Rotation >= 270)
                {
                    this.ClipToRight(rect, minZ, maxZ);
                }
            }
            else
            {
                this.ClipRectangle(rect.X + 1, rect.Y + 1, rect.Right - 1, rect.Bottom - 1);
            }
        }

        public abstract void ClipEllipse(System.Drawing.Rectangle r);
        public abstract void ClipPolygon(params Point[] p);
        public abstract void ClipRectangle(System.Drawing.Rectangle r);
        public void ClipRectangle(int left, int top, int right, int bottom)
        {
            this.ClipRectangle(Utils.FromLTRB(left, top, right, bottom));
        }

        private void ClipToLeft(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            Point[] p = new Point[] { this.Calc3DPoint(rect.X, rect.Bottom, minZ), this.Calc3DPoint(rect.X, rect.Bottom, maxZ), this.Calc3DPoint(rect.X, rect.Y, maxZ), this.Calc3DPoint(rect.Right, rect.Y, maxZ), this.Calc3DPoint(rect.Right, rect.Y, minZ), this.Calc3DPoint(rect.Right, rect.Bottom, minZ) };
            this.ClipPolygon(p);
        }

        private void ClipToRight(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            Point[] p = new Point[6];
            p[0] = this.Calc3DPoint(rect.X, rect.Bottom, minZ);
            p[1] = this.Calc3DPoint(rect.X, rect.Y, minZ);
            Point point = this.Calc3DPoint(rect.X, rect.Y, maxZ);
            Point point2 = this.Calc3DPoint(rect.Right, rect.Y, minZ);
            p[2] = (point2.Y < point.Y) ? point2 : point;
            p[3] = this.Calc3DPoint(rect.Right, rect.Y, maxZ);
            point = this.Calc3DPoint(rect.Right, rect.Bottom, maxZ);
            point2 = this.Calc3DPoint(rect.Right, rect.Y, minZ);
            p[4] = (point2.X > point.X) ? point2 : point;
            p[5] = this.Calc3DPoint(rect.Right, rect.Bottom, minZ);
            if (p[5].X < p[0].X)
            {
                p[0].X = p[5].X;
                if (point2.Y < p[0].Y)
                {
                    p[0].Y = point2.Y;
                }
            }
            this.ClipPolygon(p);
        }

        public void Cone(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool darkSides)
        {
            this.InternalCylinder(vertical, r, z0, z1, darkSides, 0);
        }

        public void Cone(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool darkSides, int conePercent)
        {
            this.InternalCylinder(vertical, r, z0, z1, darkSides, conePercent);
        }

        public void Cone(bool vertical, int left, int top, int right, int bottom, int z0, int z1, bool darkSides)
        {
            this.InternalCylinder(vertical, Utils.FromLTRB(left, top, right, bottom), z0, z1, darkSides, 0);
        }

        public bool ConvexHull(ref Point[] p, out int size)
        {
            size = p.Length;
            if (p.Length != 3)
            {
                if (p.Length < 3)
                {
                    return false;
                }
                int y = 0x989680;
                int x = 0x989680;
                int index = 0;
                int lowerBound = p.GetLowerBound(0);
                int upperBound = p.GetUpperBound(0);
                int high = upperBound - 1;
                for (int i = lowerBound; i <= upperBound; i++)
                {
                    if (p[i].Y == y)
                    {
                        if (p[i].X > x)
                        {
                            x = p[i].X;
                            index = i;
                        }
                    }
                    else if (p[i].Y < y)
                    {
                        y = p[i].Y;
                        x = p[i].X;
                        index = i;
                    }
                }
                Point point = p[index];
                p[index] = p[upperBound];
                double[] angles = new double[high + 1];
                for (int j = p.GetLowerBound(0); j <= high; j++)
                {
                    int num8 = point.X - p[j].X;
                    int num9 = point.Y - p[j].Y;
                    double num11 = Math.Sqrt((double) ((num8 * num8) + (num9 * num9)));
                    angles[j] = (num11 == 0.0) ? 0.0 : (((double) num8) / num11);
                }
                QuickSortAngle(ref p, ref angles, p.GetLowerBound(0), high);
                int num12 = 1;
                do
                {
                    bool flag;
                    if (num12 == 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        Point point3;
                        Point point2 = p[num12 - 1];
                        if (num12 == high)
                        {
                            point3 = point;
                        }
                        else
                        {
                            point3 = p[num12 + 1];
                        }
                        flag = (((point2.X - p[num12].X) * (point3.Y - p[num12].Y)) - ((point3.X - p[num12].X) * (point2.Y - p[num12].Y))) < 0;
                    }
                    if (flag)
                    {
                        num12++;
                    }
                    else
                    {
                        int num13 = high;
                        if (num12 != num13)
                        {
                            for (int k = num12; k < num13; k++)
                            {
                                p[k] = p[k + 1];
                            }
                        }
                        high = num13 - 1;
                        num12--;
                    }
                }
                while (num12 != high);
                high++;
                p[high] = point;
                size = high + 1;
            }
            return true;
        }

        protected internal double CorrectAngle(double rotAngle)
        {
            double num = rotAngle;
            if (num > 360.0)
            {
                num -= 360.0;
            }
            if (num < 0.0)
            {
                num += 360.0;
            }
            return num;
        }

        protected internal int CorrectAngle(int rotAngle)
        {
            return Convert.ToInt32(this.CorrectAngle(Convert.ToDouble(rotAngle)));
        }

        protected System.Drawing.Rectangle CorrectRectangle(System.Drawing.Rectangle r)
        {
            if (r.Height < 0)
            {
                r.Y = r.Bottom;
                r.Height = -r.Height;
            }
            if (r.Width < 0)
            {
                r.X = r.Right;
                r.Width = -r.Width;
            }
            return r;
        }

        public static bool CrossingLines(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, out double x, out double y)
        {
            double num = y2 - y1;
            double num2 = y4 - y3;
            double num3 = x2 - x1;
            double num4 = x4 - x3;
            double num5 = num / num3;
            double num6 = num2 / num4;
            double num7 = y1 - (num5 * x1);
            double num8 = y3 - (num6 * x3);
            double num9 = num6 - num5;
            if (Math.Abs(num9) > 1E-15)
            {
                x = (num7 - num8) / num9;
            }
            else
            {
                x = x2;
            }
            y = (num5 * x) + num7;
            double num10 = (x1 * y2) - (x2 * y1);
            double num11 = (x3 * y4) - (x4 * y3);
            return (((((((num * x3) - (num3 * y3)) - num10) != 0.0) ^ ((((num * x4) - (num3 * y4)) - num10) != 0.0)) && (((((num2 * x1) - (num4 * y1)) - num11) != 0.0) ^ ((((num2 * x2) - (num4 * y2)) - num11) != 0.0))) || ((((((num * x3) - (num3 * y3)) - num10) > 0.0) ^ ((((num * x4) - (num3 * y4)) - num10) > 0.0)) && (((((num2 * x1) - (num4 * y1)) - num11) > 0.0) ^ ((((num2 * x2) - (num4 * y2)) - num11) > 0.0))));
        }

        public void Cube(System.Drawing.Rectangle r, int z0, int z1)
        {
            this.Cube(r.X, r.Y, r.Right, r.Bottom, z0, z1, true);
        }

        public void Cube(System.Drawing.Rectangle r, int z0, int z1, bool darkSides)
        {
            this.Cube(r.X, r.Y, r.Right, r.Bottom, z0, z1, darkSides);
        }

        public virtual void Cube(int left, int top, int right, int bottom, int z0, int z1, bool darkSides)
        {
            Color c = this.Brush.Color;
            Point point = this.Calc3DPoint(left, top, z0);
            Point point2 = this.Calc3DPoint(right, top, z0);
            Point point3 = this.Calc3DPoint(right, bottom, z0);
            Point point4 = this.Calc3DPoint(right, top, z1);
            this.IPoints[0] = point;
            this.IPoints[1] = point2;
            this.IPoints[2] = point3;
            this.IPoints[3] = this.Calc3DPoint(left, bottom, z0);
            if (this.Culling() > 0.0)
            {
                this.PolygonFour();
            }
            else
            {
                this.Calc3DPos(ref this.IPoints[0], left, top, z1);
                this.Calc3DPos(ref this.IPoints[1], right, top, z1);
                this.Calc3DPos(ref this.IPoints[2], right, bottom, z1);
                this.Calc3DPos(ref this.IPoints[3], left, bottom, z1);
                this.PolygonFour();
            }
            this.Calc3DPos(ref this.IPoints[2], right, bottom, z1);
            this.IPoints[0] = point2;
            this.IPoints[1] = point4;
            this.IPoints[3] = point3;
            if (this.Culling() > 0.0)
            {
                if (darkSides)
                {
                    this.InternalApplyDark(c, 0x80);
                }
                this.PolygonFour();
            }
            this.IPoints[0] = point;
            this.Calc3DPos(ref this.IPoints[1], left, top, z1);
            this.Calc3DPos(ref this.IPoints[2], left, bottom, z1);
            this.Calc3DPos(ref this.IPoints[3], left, bottom, z0);
            double num = ((this.IPoints[3].X - this.IPoints[0].X) * (this.IPoints[1].Y - this.IPoints[0].Y)) - ((this.IPoints[1].X - this.IPoints[0].X) * (this.IPoints[3].Y - this.IPoints[0].Y));
            if (num > 0.0)
            {
                if (darkSides)
                {
                    this.InternalApplyDark(c, 0x80);
                }
                this.PolygonFour();
            }
            if (((this.Culling() > 0.0) && (this.Brush.Color.A < 0xfe)) && (this.Pen.Color != Color.Transparent))
            {
                bool visible = this.Brush.Visible;
                this.Brush.Visible = false;
                Color color = this.Pen.Color;
                this.Pen.Color = Color.FromArgb(0xff - this.Brush.Color.A, this.Pen.Color.R, this.Pen.Color.G, this.Pen.Color.B);
                this.Polyline(this.IPoints);
                this.Brush.Visible = visible;
                this.Pen.Color = color;
            }
            this.Calc3DPos(ref this.IPoints[3], left, top, z1);
            num = ((point.X - point2.X) * (point4.Y - point2.Y)) - ((point4.X - point2.X) * (point.Y - point2.Y));
            if (num > 0.0)
            {
                this.IPoints[0] = point;
                this.IPoints[1] = point2;
                this.IPoints[2] = point4;
                if (darkSides)
                {
                    this.InternalApplyDark(c, 0x40);
                }
                this.PolygonFour();
            }
            this.Calc3DPos(ref this.IPoints[0], left, bottom, z0);
            this.Calc3DPos(ref this.IPoints[2], right, bottom, z1);
            this.Calc3DPos(ref this.IPoints[1], left, bottom, z1);
            this.IPoints[3] = point3;
            if (this.Culling() < 0.0)
            {
                if (darkSides)
                {
                    this.InternalApplyDark(c, 0x40);
                }
                this.PolygonFour();
            }
            else if ((this.Brush.Color.A < 0xfe) && (this.Pen.Color != Color.Transparent))
            {
                bool flag2 = this.Brush.Visible;
                this.Brush.Visible = false;
                Color color3 = this.Pen.Color;
                this.Pen.Color = Color.FromArgb(0xff - this.Brush.Color.A, this.Pen.Color.R, this.Pen.Color.G, this.Pen.Color.B);
                this.Polyline(this.IPoints);
                this.Brush.Visible = flag2;
                this.Pen.Color = color3;
            }
            this.Brush.Color = c;
        }

        internal static bool Cull(Point[] p)
        {
            return Cull(p[0], p[1], p[2]);
        }

        internal static bool Cull(Point p0, Point p1, Point p2)
        {
            return ((((p0.X - p1.X) * (p2.Y - p1.Y)) - ((p2.X - p1.X) * (p0.Y - p1.Y))) < 0);
        }

        private double Culling()
        {
            return (double) (((this.IPoints[3].X - this.IPoints[2].X) * (this.IPoints[1].Y - this.IPoints[2].Y)) - ((this.IPoints[1].X - this.IPoints[2].X) * (this.IPoints[3].Y - this.IPoints[2].Y)));
        }

        public void Cylinder(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool darkSides)
        {
            this.InternalCylinder(vertical, r, z0, z1, darkSides, 100);
        }

        public virtual void DisableRotation()
        {
        }

        protected internal void DisposeObj()
        {
            if (this.ifont != null)
            {
                this.ifont.Dispose();
                this.ifont = null;
            }
            if (this.pen != null)
            {
                this.pen.Dispose();
                this.pen = null;
            }
            if (this.bBrush != null)
            {
                this.bBrush.Dispose();
                this.bBrush = null;
            }
        }

        private void DoBevelRect(System.Drawing.Rectangle rect, ChartPen a, ChartPen b)
        {
            Point point = new Point(rect.Right - 1, rect.Y);
            Point point2 = new Point(rect.X, rect.Bottom - 1);
            Point point3 = new Point(rect.Right - 1, rect.Bottom - 1);
            this.Line(a, rect.Location, point);
            this.Line(a, rect.Location, point2);
            this.Line(b, point2, point3);
            this.Line(b, point3, point);
        }

        protected abstract void DoDrawString(int x, int y, string text, ChartBrush aBrush);
        public abstract void Draw(System.Drawing.Rectangle r, Image image, bool transparent);
        public abstract void Draw(int x, int y, Image image);
        public void Draw(System.Drawing.Rectangle r, Image image, ImageMode mode, bool transparent)
        {
            if (mode == ImageMode.Center)
            {
                r.X += (r.Width - image.Width) / 2;
                r.Y += (r.Height - image.Height) / 2;
                r.Width = image.Width;
                r.Height = image.Height;
            }
            else if (mode == ImageMode.Normal)
            {
                r.Width = image.Width;
                r.Height = image.Height;
            }
            else if (mode == ImageMode.Tile)
            {
                if ((image.Width > 0) && (image.Height > 0))
                {
                    int num = 0;
                    do
                    {
                        int num2 = 0;
                        do
                        {
                            this.Draw(new System.Drawing.Rectangle(r.X + num2, r.Y + num, image.Width, image.Height), image, transparent);
                            num2 += image.Width;
                        }
                        while (num2 < r.Width);
                        num += image.Height;
                    }
                    while (num < r.Height);
                }
                return;
            }
            this.Draw(r, image, transparent);
        }

        public abstract void DrawBeziers(params Point[] p);
        public virtual void DrawBeziers(int z, params Point[] p)
        {
            for (int i = 0; i <= p.GetUpperBound(0); i++)
            {
                p[i] = this.Calc3DPoint(p[i], z);
            }
            this.DrawBeziers(p);
        }

        private void DrawBorder(System.Drawing.Rectangle rect, int BevelWidth, ChartPen pen, int borderRound)
        {
            GraphicsPath path = new GraphicsPath();
            new Region(System.Drawing.Rectangle.Empty);
            System.Drawing.Rectangle rectangle = rect;
            if (pen.Visible)
            {
                GraphicsPath path2;
                byte[] buffer;
                if (borderRound > 0)
                {
                    path2 = new GraphicsPath(PointDouble.RoundF(this.GetClipRoundRectangle(out buffer, rectangle, borderRound, borderRound, 0)), buffer);
                    path.AddPath(path2, false);
                }
                else
                {
                    path.AddRectangle(rectangle);
                }
                rectangle.Inflate(-BevelWidth, -BevelWidth);
                if (borderRound > 0)
                {
                    path2 = new GraphicsPath(PointDouble.RoundF(this.GetClipRoundRectangle(out buffer, rectangle, borderRound, borderRound, BevelWidth)), buffer);
                    path.AddPath(path2, false);
                }
                else
                {
                    path.AddRectangle(rectangle);
                }
                this.DrawPath(pen.DrawingPen, path);
            }
        }

        public abstract void DrawPath(System.Drawing.Pen pen, GraphicsPath path);
        public virtual void Elevate(int angle)
        {
        }

        public void Ellipse(System.Drawing.Rectangle r)
        {
            this.Ellipse(r.X, r.Y, r.Right, r.Bottom);
        }

        public void Ellipse(System.Drawing.Rectangle r, int z)
        {
            this.Ellipse(r.X, r.Y, r.Right, r.Bottom, z);
        }

        public void Ellipse(System.Drawing.Rectangle r, int z, double angle)
        {
            this.Ellipse(r.Left, r.Top, r.Right, r.Bottom, z, angle);
        }

        public abstract void Ellipse(int x1, int y1, int x2, int y2);
        public virtual void Ellipse(int x1, int y1, int x2, int y2, int z)
        {
            this.Calc3DPos(ref x1, ref y1, z);
            this.Calc3DPos(ref x2, ref y2, z);
            this.Ellipse(x1, y1, x2, y2);
        }

        public void Ellipse(int left, int top, int right, int bottom, int z, double angle)
        {
            Point[] p = new Point[0x40];
            Point[] pointArray2 = new Point[3];
            double num5 = (right + left) * 0.5;
            double num6 = (bottom + top) * 0.5;
            double num7 = (right - left) * 0.5;
            double num8 = (bottom - top) * 0.5;
            angle *= 0.017453292519943295;
            double num9 = 0.099733100113961692;
            double num10 = Math.Sin(angle);
            double num11 = Math.Cos(angle);
            for (int i = 0; i < 0x40; i++)
            {
                double num3 = Math.Sin(i * num9);
                double num4 = Math.Cos(i * num9);
                double num = num7 * num3;
                double num2 = num8 * num4;
                p[i].X = Utils.Round((double) (num5 + ((num * num11) + (num2 * num10))));
                p[i].Y = Utils.Round((double) (num6 + ((-num * num10) + (num2 * num11))));
            }
            if (this.Brush.Visible)
            {
                bool visible = this.Pen.Visible;
                this.Pen.Visible = false;
                int num13 = Utils.Round(num5);
                int num14 = Utils.Round(num6);
                for (int j = 1; j < 0x40; j++)
                {
                    pointArray2[0].X = num13;
                    pointArray2[0].Y = num14;
                    pointArray2[1] = p[j - 1];
                    pointArray2[2] = p[j];
                    this.Polygon(z, pointArray2);
                }
                pointArray2[0].X = num13;
                pointArray2[0].Y = num14;
                pointArray2[1] = p[0x3f];
                pointArray2[2] = p[0];
                this.Polygon(z, pointArray2);
                this.Pen.Visible = visible;
            }
            if (this.Pen.Visible)
            {
                this.Polyline(z, p);
            }
        }

        public void EllipseEnh(int x1, int y1, int x2, int y2)
        {
            Color baseColor = this.Brush.Color;
            this.Brush.Color = this.SpecialColor(baseColor);
            int transparency = this.Brush.Transparency;
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            int width = Utils.Round((double) (r.Width * 0.84));
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(r.Left + Utils.Round((double) (r.Width * 0.08)), r.Top + Utils.Round((double) (r.Height * 0.03)), width, Utils.Round((double) (r.Height * 0.94)));
            this.Pen.Visible = false;
            System.Drawing.Rectangle chartRect = base.chart.ChartRect;
            Region region = this.AddRightRegion(chartRect, 0, base.chart.Aspect.Width3D);
            this.MixClip(region);
            this.Ellipse(r);
            if (((x2 - x1) < 5) || ((y2 - y1) < 5))
            {
                this.UnClip();
            }
            else
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(r);
                Region region2 = new Region(path);
                region2.Intersect(region);
                this.MixClip(region2);
                GraphicsPath path2 = new GraphicsPath();
                System.Drawing.Rectangle rectangle4 = new System.Drawing.Rectangle(r.X + Utils.Round((double) ((r.Width / 10) * 0.5)), r.Y + ((r.Height / 10) * 4), (r.Width / 10) * 9, (r.Height / 10) * 7);
                if ((rectangle4.Width < 5) || (rectangle4.Height < 5))
                {
                    this.UnClip();
                }
                else
                {
                    path2.AddEllipse(rectangle4);
                    PathGradientBrush brush = new PathGradientBrush(path2);
                    Color c = baseColor;
                    ApplyBright(ref c, 0x80);
                    brush.CenterColor = c;
                    Color[] colorArray = new Color[] { Color.Transparent };
                    brush.SurroundColors = colorArray;
                    this.FillRectangle(brush, r.X, r.Y, r.Width, r.Height);
                    this.UnClip();
                    this.Brush.Transparency = 0;
                    this.Brush.Gradient.Visible = true;
                    this.Brush.Gradient.StartColor = Color.White;
                    this.Brush.Gradient.MiddleColor = Color.Transparent;
                    this.Brush.Gradient.EndColor = Color.Transparent;
                    path = new GraphicsPath();
                    path.AddEllipse(rect);
                    Region region3 = new Region(new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width, Utils.Round((double) (rect.Height * 0.7))));
                    Region region4 = new Region(path);
                    region4.Intersect(region3);
                    region4.Intersect(region);
                    this.MixClip(region4);
                    if (base.Chart.Graphics3D.CanvasType != Steema.TeeChart.Drawing.CanvasType.HotSpot)
                    {
                        this.Rectangle(rect.X, rect.Y, rect.Right, rect.Y + Utils.Round((double) (rect.Height * 0.9)));
                    }
                    this.UnClip();
                }
            }
        }

        public virtual void EllipseEnh(int x1, int y1, int x2, int y2, int z)
        {
            this.Calc3DPos(ref x1, ref y1, z);
            this.Calc3DPos(ref x2, ref y2, z);
            this.EllipseEnh(x1, y1, x2, y2);
        }

        public virtual void EnableRotation()
        {
        }

        public virtual void EndBlending()
        {
        }

        public abstract void EraseBackground(int left, int top, int right, int bottom);
        private void FillBorder(System.Drawing.Rectangle rect, int BevelWidth, ChartBrush brush, int borderRound)
        {
            System.Drawing.Brush drawingBrush;
            GraphicsPath path = new GraphicsPath();
            Region region = new Region(System.Drawing.Rectangle.Empty);
            System.Drawing.Rectangle rectangle = rect;
            if (brush.GradientVisible)
            {
                drawingBrush = brush.Gradient.DrawingBrush(rect);
            }
            else
            {
                drawingBrush = brush.DrawingBrush;
            }
            if (brush.Visible)
            {
                GraphicsPath path2;
                byte[] buffer;
                if (borderRound > 0)
                {
                    path2 = new GraphicsPath(PointDouble.RoundF(this.GetClipRoundRectangle(out buffer, rectangle, borderRound, borderRound, 0)), buffer);
                    path.AddPath(path2, false);
                }
                else
                {
                    path.AddRectangle(rectangle);
                }
                region.Union(path);
                path.Reset();
                rectangle.Inflate(-BevelWidth, -BevelWidth);
                if (borderRound > 0)
                {
                    path2 = new GraphicsPath(PointDouble.RoundF(this.GetClipRoundRectangle(out buffer, rectangle, borderRound, borderRound, BevelWidth)), buffer);
                    path.AddPath(path2, false);
                }
                else
                {
                    path.AddRectangle(rectangle);
                }
                region.Xor(path);
                this.FillRegion(drawingBrush, region);
            }
        }

        public virtual void FillRectangle(System.Drawing.Brush brush, int x, int y, int width, int height)
        {
            ChartBrush brush2 = null;
            try
            {
                brush2 = new ChartBrush(base.Chart, brush);
                this.Brush = brush2;
                bool visible = this.Pen.Visible;
                this.Pen.Visible = false;
                this.Rectangle(new System.Drawing.Rectangle(x, y, width, height));
                this.Pen.Visible = visible;
            }
            finally
            {
                brush2.Dispose();
            }
        }

        public abstract void FillRegion(System.Drawing.Brush brush, Region region);
        public int FontTextHeight(ChartFont f)
        {
            return Utils.Round(this.TextHeight(f, "W"));
        }

        public Point[] FourPointsFromRect(System.Drawing.Rectangle r, int z)
        {
            return new Point[] { this.Calc3DPoint(r.Location, z), this.Calc3DPoint(r.Right, r.Top, z), this.Calc3DPoint(r.Right, r.Bottom, z), this.Calc3DPoint(r.Left, r.Bottom, z) };
        }

        public virtual void FrontPlaneBegin()
        {
            if (this.iDisabledRotation == 0)
            {
                this.DisableRotation();
            }
            this.iDisabledRotation++;
        }

        public virtual void FrontPlaneEnd()
        {
            this.iDisabledRotation--;
            if (this.iDisabledRotation == 0)
            {
                this.EnableRotation();
            }
        }

        public virtual Region GetChartPolygon(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            if (this.aspect.View3D)
            {
                Point point = this.Calc3DPoint(rect.X, rect.Y, minZ);
                Point point2 = this.Calc3DPoint(rect.Right, rect.Y, maxZ);
                return new Region(new System.Drawing.Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y));
            }
            return new Region(new System.Drawing.Rectangle(rect.X + 1, rect.Y + 1, ((rect.Right - 1) - rect.X) + 1, ((rect.Bottom - 1) - rect.Y) + 1));
        }

        public PointDouble[] GetClipRoundRectangle(System.Drawing.Rectangle rect, int borderRound, int width)
        {
            return this.GetClipRoundRectangle(rect, borderRound, borderRound, width);
        }

        public PointDouble[] GetClipRoundRectangle(System.Drawing.Rectangle rect, int roundWidth, int roundHeight, int width)
        {
            byte[] buffer;
            return this.GetClipRoundRectangle(out buffer, rect, roundWidth, roundHeight, width);
        }

        public PointDouble[] GetClipRoundRectangle(out byte[] pathPointType, System.Drawing.Rectangle rect, int roundWidth, int roundHeight, int width)
        {
            PointDouble[] numArray;
            byte[] buffer;
            roundWidth -= width;
            roundHeight -= width;
            Size size = new Size(roundWidth * 2, roundHeight * 2);
            Point location = rect.Location;
            System.Drawing.Rectangle rectBounds = new System.Drawing.Rectangle(location.X, location.Y, size.Width, size.Height);
            location.X = rect.Right - size.Width;
            System.Drawing.Rectangle rectangle2 = new System.Drawing.Rectangle(location.X, location.Y, size.Width, size.Height);
            location.Y = rect.Bottom - size.Height;
            System.Drawing.Rectangle rectangle3 = new System.Drawing.Rectangle(location.X, location.Y, size.Width, size.Height);
            location.X = rect.Left;
            System.Drawing.Rectangle rectangle4 = new System.Drawing.Rectangle(location.X, location.Y, size.Width, size.Height);
            int index = 0;
            if ((roundHeight <= 0) || (roundWidth <= 0))
            {
                numArray = new PointDouble[4];
                buffer = new byte[4];
                PointDouble num3 = PointDouble.FromPoint(rect.Location);
                numArray[0] = num3;
                buffer[0] = 0;
                num3.X = rect.Right - size.Width;
                numArray[1] = num3;
                buffer[1] = 1;
                num3.Y = rect.Bottom - size.Height;
                numArray[2] = num3;
                buffer[2] = 1;
                num3.X = rect.Left;
                numArray[3] = num3;
                buffer[3] = 0x81;
            }
            else
            {
                int num;
                numArray = new PointDouble[360];
                buffer = new byte[360];
                numArray[index] = this.PointFromEllipse(rectBounds, 90.0);
                buffer[index] = 0;
                index++;
                for (num = 0x5b; num < 180; num++)
                {
                    numArray[index] = this.PointFromEllipse(rectBounds, (double) num);
                    buffer[index] = 1;
                    index++;
                }
                for (num = 180; num < 270; num++)
                {
                    numArray[index] = this.PointFromEllipse(rectangle4, (double) num);
                    buffer[index] = 1;
                    index++;
                }
                for (num = 270; num < 360; num++)
                {
                    numArray[index] = this.PointFromEllipse(rectangle3, (double) num);
                    buffer[index] = 1;
                    index++;
                }
                for (num = 0; num < 0x59; num++)
                {
                    numArray[index] = this.PointFromEllipse(rectangle2, (double) num);
                    buffer[index] = 1;
                    index++;
                }
                numArray[index] = this.PointFromEllipse(rectangle2, 90.0);
                buffer[index] = 0x81;
            }
            pathPointType = buffer;
            return numArray;
        }

        public static Color GetDefaultColor(int index)
        {
            return ColorPalette[index % ColorPalette.Length];
        }

        public static HatchStyle GetDefaultPattern(int index)
        {
            HatchStyle[] styleArray2 = new HatchStyle[0x12];
            styleArray2[1] = HatchStyle.Vertical;
            styleArray2[2] = HatchStyle.ForwardDiagonal;
            styleArray2[3] = HatchStyle.BackwardDiagonal;
            styleArray2[4] = HatchStyle.Cross;
            styleArray2[5] = HatchStyle.DiagonalCross;
            styleArray2[6] = HatchStyle.DiagonalBrick;
            styleArray2[7] = HatchStyle.Divot;
            styleArray2[8] = HatchStyle.LargeConfetti;
            styleArray2[9] = HatchStyle.OutlinedDiamond;
            styleArray2[10] = HatchStyle.Plaid;
            styleArray2[11] = HatchStyle.Shingle;
            styleArray2[12] = HatchStyle.SolidDiamond;
            styleArray2[13] = HatchStyle.Sphere;
            styleArray2[14] = HatchStyle.Trellis;
            styleArray2[15] = HatchStyle.Wave;
            styleArray2[0x10] = HatchStyle.Weave;
            styleArray2[0x11] = HatchStyle.ZigZag;
            HatchStyle[] styleArray = styleArray2;
            return styleArray[index % styleArray.Length];
        }

        protected virtual PointXYZ GetRotationCenter()
        {
            int x = Utils.Round(this.irotationCenter.X);
            int y = Utils.Round(this.irotationCenter.Y);
            return new PointXYZ(x, y, Utils.Round(this.irotationCenter.Z));
        }

        protected virtual bool GetSupports3DText()
        {
            return false;
        }

        public void HorizCone(System.Drawing.Rectangle r, int z0, int z1, bool darkSides, int conePercent)
        {
            this.InternalCylinder(false, r, z0, z1, darkSides, conePercent);
        }

        public abstract void HorizontalLine(int left, int right, int y);
        public virtual void HorizontalLine(int left, int right, int y, int z)
        {
            int num = y;
            this.Calc3DPos(ref left, ref num, z);
            this.Calc3DPos(ref right, ref y, z);
            this.Line(left, num, right, y);
        }

        protected internal virtual void InitWindow(Graphics graphics, Aspect a, System.Drawing.Rectangle r, int MaxDepth)
        {
            this.bounds = r;
            this.aspect = a;
            this.IZoomFactor = 1.0;
            this.oldRegion = new Region(new System.Drawing.Rectangle(0, 0, 0, 0));
            this.hasClipRegion = false;
            this.is3D = this.aspect.View3D;
            if (this.is3D)
            {
                this.isOrthogonal = this.aspect.orthogonal;
                if (this.isOrthogonal)
                {
                    double orthoAngle = this.aspect.OrthoAngle;
                    if (orthoAngle > 90.0)
                    {
                        this.IOrthoX = -1.0;
                        orthoAngle = 180.0 - orthoAngle;
                    }
                    else
                    {
                        this.IOrthoX = 1.0;
                    }
                    double num2 = Math.Sin(this.aspect.OrthoAngle * 0.017453292519943295);
                    double num3 = Math.Cos(this.aspect.OrthoAngle * 0.017453292519943295);
                    this.IOrthoY = (num3 < 0.01) ? 1.0 : (num2 / num3);
                }
                this.IZoomFactor = 0.01 * this.aspect.Zoom;
                this.IZoomText = this.aspect.ZoomText;
            }
            this.CalcTrigValues();
        }

        private void InternalApplyDark(Color c, byte quantity)
        {
            this.Brush.ApplyDark(c, quantity);
        }

        protected virtual void InternalCylinder(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool dark3D, int conePercent)
        {
            Color backColor;
            int num;
            int num2;
            int num4;
            int num6;
            double num8;
            double num9;
            Point3D[] pointdArray = new Point3D[0x10];
            Point[] p = new Point[4];
            if (this.Brush.Solid)
            {
                backColor = this.Brush.Color;
            }
            else
            {
                backColor = this.BackColor;
            }
            int num5 = (z1 - z0) / 2;
            int num3 = (z1 + z0) / 2;
            if (vertical)
            {
                num4 = (r.Right - r.X) / 2;
                num2 = (r.Right + r.X) / 2;
                num = r.Bottom - r.Y;
                for (num6 = 0; num6 < 0x10; num6++)
                {
                    Utils.SinCos((num6 - 3) * 0.39269908169872414, out num8, out num9);
                    pointdArray[num6].X = num2 + Utils.Round((double) (num8 * num4));
                    pointdArray[num6].Y = r.Y;
                    pointdArray[num6].Z = num3 - Utils.Round((double) (num9 * num5));
                }
            }
            else
            {
                num4 = (r.Bottom - r.Y) / 2;
                num2 = (r.Bottom + r.Y) / 2;
                num = r.Right - r.X;
                for (num6 = 0; num6 < 0x10; num6++)
                {
                    Utils.SinCos((num6 - 4) * 0.39269908169872414, out num8, out num9);
                    pointdArray[num6].X = r.Right;
                    pointdArray[num6].Y = num2 + Utils.Round((double) (num8 * num4));
                    pointdArray[num6].Z = num3 - Utils.Round((double) (num9 * num5));
                }
            }
            num4 = Utils.Round((double) ((num4 * conePercent) * 0.01));
            num5 = Utils.Round((double) ((num5 * conePercent) * 0.01));
            if (vertical)
            {
                p[1] = this.Calc3DPoint(pointdArray[0].X, pointdArray[0].Y + num, pointdArray[0].Z);
                Utils.SinCos(-1.1780972450961724, out num8, out num9);
                pointdArray[0].X = num2 + Utils.Round((double) (num8 * num4));
                pointdArray[0].Z = num3 - Utils.Round((double) (num9 * num5));
            }
            else
            {
                p[1] = this.Calc3DPoint(pointdArray[0].X - num, pointdArray[0].Y, pointdArray[0].Z);
                Utils.SinCos(-1.5707963267948966, out num8, out num9);
                pointdArray[0].Y = num2 + Utils.Round((double) (num8 * num4));
                pointdArray[0].Z = num3 - Utils.Round((double) (num9 * num5));
            }
            Point point = p[1];
            p[0] = this.Calc3DPoint(pointdArray[0]);
            int num7 = 0;
            num6 = 1;
            while (num6 < 0x10)
            {
                if (vertical)
                {
                    p[2] = this.Calc3DPoint(pointdArray[num6].X, pointdArray[num6].Y + num, pointdArray[num6].Z);
                    Utils.SinCos((num6 - 3) * 0.39269908169872414, out num8, out num9);
                    pointdArray[num6].X = num2 + Utils.Round((double) (num8 * num4));
                    pointdArray[num6].Z = num3 - Utils.Round((double) (num9 * num5));
                }
                else
                {
                    p[2] = this.Calc3DPoint(pointdArray[num6].X - num, pointdArray[num6].Y, pointdArray[num6].Z);
                    Utils.SinCos((num6 - 4) * 0.39269908169872414, out num8, out num9);
                    pointdArray[num6].Y = num2 + Utils.Round((double) (num8 * num4));
                    pointdArray[num6].Z = num3 - Utils.Round((double) (num9 * num5));
                }
                p[3] = this.Calc3DPoint(pointdArray[num6]);
                if (!Cull(p[0], p[1], p[2]))
                {
                    if (dark3D)
                    {
                        this.InternalApplyDark(backColor, Convert.ToByte((int) (0x10 * num7)));
                    }
                    Point[] pointArray2 = new Point[] { p[0], p[1], p[2], p[3] };
                    this.Polygon(pointArray2);
                }
                p[0] = p[3];
                p[1] = p[2];
                num7++;
                num6++;
            }
            p[3] = this.Calc3DPoint(pointdArray[0]);
            p[2] = point;
            if (!Cull(p[0], p[1], p[2]))
            {
                this.Polygon(p);
            }
            this.Calc3DPos(ref p[0], 0, 0, 0);
            this.Calc3DPos(ref p[1], 0, 10, 0);
            this.Calc3DPos(ref p[2], 0, 10, 10);
            bool flag = Cull(p);
            Point[] pointArray3 = new Point[0x10];
            if (!vertical && flag)
            {
                if (conePercent == 0)
                {
                    conePercent = 1;
                }
                for (int i = 0; i < 0x10; i++)
                {
                    pointArray3[num6 - 1] = this.Calculate3DPosition(r.X, Utils.Round((double) ((100.0 * pointdArray[i].Y) / ((double) conePercent))), Utils.Round((double) ((100.0 * pointdArray[i].Z) / ((double) conePercent))));
                }
            }
            else
            {
                for (int j = 0; j < 0x10; j++)
                {
                    pointArray3[j] = this.Calc3DPoint(pointdArray[j]);
                }
            }
            if (dark3D)
            {
                this.InternalApplyDark(backColor, Convert.ToByte((int) (0x10 * num7)));
            }
            this.Polygon(pointArray3);
            this.Brush.Color = backColor;
        }

        protected bool IsRectangle(params Point[] p)
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < p.Length; i++)
            {
                for (int j = i + 1; j < p.Length; j++)
                {
                    if (p[i].X == p[j].X)
                    {
                        num++;
                    }
                    if (p[i].Y == p[j].Y)
                    {
                        num2++;
                    }
                }
            }
            return ((num >= 2) && (num2 >= 2));
        }

        public void Line(Point p0, Point p1)
        {
            this.Line(p0.X, p0.Y, p1.X, p1.Y);
        }

        protected abstract void Line(ChartPen p, Point a, Point b);
        public void Line(Point p0, Point p1, int z)
        {
            this.Line(p0.X, p0.Y, p1.X, p1.Y, z);
        }

        public abstract void Line(int x0, int y0, int x1, int y1);
        public virtual void Line(int x0, int y0, int x1, int y1, int z)
        {
            this.Calc3DPos(ref x0, ref y0, z);
            this.Calc3DPos(ref x1, ref y1, z);
            this.Line(x0, y0, x1, y1);
        }

        public virtual void Line(int x0, int y0, int z0, int x1, int y1, int z1)
        {
            int x = x0;
            int y = y0;
            this.Calc3DPos(ref x, ref y, z0);
            this.Calc3DPos(ref x1, ref y1, z1);
            this.Line(x, y, x1, y1);
        }

        public virtual void LineTo(Point3D p)
        {
            this.Calc3DPos(ref p.X, ref p.Y, p.Z);
            this.LineTo(p.X, p.Y);
        }

        public void LineTo(Point p, int z)
        {
            this.LineTo(p.X, p.Y, z);
        }

        public abstract void LineTo(int x, int y);
        public virtual void LineTo(int x, int y, int z)
        {
            this.Calc3DPos(ref x, ref y, z);
            this.LineTo(x, y);
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use Line method.")]
        public void LineWithZ(int x, int y, int x1, int y1, int z)
        {
            this.Line(x, y, x1, y1, z);
        }

        public abstract SizeF MeasureString(ChartFont f, string text);
        private void MixClip(Region region)
        {
            Region clip = this.g.Clip;
            if (!clip.IsEmpty(this.g))
            {
                region.Intersect(clip);
            }
            this.g.Clip = region;
        }

        public virtual void MoveTo(Point3D p)
        {
            this.Calc3DPos(ref p.X, ref p.Y, p.Z);
            this.MoveTo(p.X, p.Y);
        }

        public void MoveTo(Point p)
        {
            this.MoveTo(p.X, p.Y);
        }

        public virtual void MoveTo(Point p, int z)
        {
            int x = p.X;
            int y = p.Y;
            this.Calc3DPos(ref x, ref y, z);
            this.MoveTo(x, y);
        }

        public abstract void MoveTo(int x, int y);
        public virtual void MoveTo(int x, int y, int z)
        {
            this.Calc3DPos(ref x, ref y, z);
            this.MoveTo(x, y);
        }

        public void OrientRectangle(ref System.Drawing.Rectangle rect)
        {
            if (rect.Left > rect.Right)
            {
                rect.X = rect.Right;
                rect.Width = Math.Abs(rect.Width);
            }
            if (rect.Top > rect.Bottom)
            {
                rect.Y = rect.Bottom;
                rect.Height = Math.Abs(rect.Height);
            }
        }

        public void PaintBevel(BevelStyles bevel, System.Drawing.Rectangle rect, int width, Color one, Color two)
        {
            ChartPen pen;
            ChartPen pen2;
            if (bevel == BevelStyles.Raised)
            {
                pen = new ChartPen(one);
                pen2 = new ChartPen(two);
            }
            else
            {
                pen = new ChartPen(two);
                pen2 = new ChartPen(one);
            }
            int num = width;
            while (num > 0)
            {
                num--;
                this.DoBevelRect(rect, pen, pen2);
                rect.Inflate(-1, -1);
            }
            pen.Dispose();
            pen2.Dispose();
        }

        public void PaintImageBevel(System.Drawing.Rectangle rect, int width, ChartPen pen, ChartBrush brush, int borderRound)
        {
            if (!pen.Visible && (borderRound > 0))
            {
                rect.Inflate(-1, -1);
            }
            this.FillBorder(rect, width, brush, borderRound);
            this.DrawBorder(rect, width, pen, borderRound);
        }

        public virtual void Pie(int x1, int y1, int x2, int y2, double startAngle, double endAngle)
        {
        }

        public void Pie(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            double num;
            double num2;
            this.CalcArcAngles(x1, y1, x2, y2, x3, y3, x4, y4, out num, out num2);
            this.Pie(x1, y1, x2, y2, num, num2);
        }

        public virtual void Pie(int xCenter, int yCenter, int xRadius, int yRadius, int z0, int z1, double startAngle, double endAngle, bool darkSides, bool drawSides, int donutPercent, int bevelPercent, EdgeStyles edgeStyle)
        {
            if (this.pie3D == null)
            {
                this.pie3D = new Pie3D(this);
            }
            this.pie3D.Pie(xCenter, yCenter, xRadius, yRadius, z0, z1, startAngle, endAngle, darkSides, drawSides, donutPercent, bevelPercent, edgeStyle);
        }

        public virtual void Pie(int xCenter, int yCenter, int xOffset, int yOffset, int xRadius, int yRadius, int z0, int z1, double startAngle, double endAngle, bool darkSides, bool drawSides, int donutPercent, int bevelPercent, EdgeStyles edgeStyle)
        {
            try
            {
                this.Pie(xCenter + xOffset, yCenter - yOffset, xRadius, yRadius, z0, z1, startAngle, endAngle, darkSides, drawSides, donutPercent, bevelPercent, edgeStyle);
            }
            finally
            {
                Thread.Sleep(0);
            }
        }

        public abstract void Pixel(int x, int y, int z, Color color);
        public void Plane(int z0, int z1, params Point[] p)
        {
            this.Calc3DPos(ref this.IPoints[0], p[0].X, p[0].Y, z0);
            this.Calc3DPos(ref this.IPoints[1], p[1].X, p[1].Y, z0);
            this.Calc3DPos(ref this.IPoints[2], p[2].X, p[2].Y, z1);
            this.Calc3DPos(ref this.IPoints[3], p[3].X, p[3].Y, z1);
            this.PolygonFour();
        }

        public virtual void Plane(Point p1, Point p2, int z0, int z1)
        {
            this.Calc3DPos(ref this.IPoints[0], p1.X, p1.Y, z0);
            this.Calc3DPos(ref this.IPoints[1], p2.X, p2.Y, z0);
            this.Calc3DPos(ref this.IPoints[2], p2.X, p2.Y, z1);
            this.Calc3DPos(ref this.IPoints[3], p1.X, p1.Y, z1);
            this.PolygonFour();
        }

        public virtual void Plane(Point p1, Point p2, Point p3, Point p4, int z)
        {
            this.Calc3DPos(ref this.IPoints[0], p1.X, p1.Y, z);
            this.Calc3DPos(ref this.IPoints[1], p2.X, p2.Y, z);
            this.Calc3DPos(ref this.IPoints[2], p3.X, p3.Y, z);
            this.Calc3DPos(ref this.IPoints[3], p4.X, p4.Y, z);
            this.PolygonFour();
        }

        public virtual void PlaneFour3D(int z0, int z1, Point[] p)
        {
            this.IPoints[0] = this.Calc3DPoint(p[0].X, p[0].Y, z0);
            this.IPoints[1] = this.Calc3DPoint(p[1].X, p[1].Y, z0);
            this.IPoints[2] = this.Calc3DPoint(p[2].X, p[2].Y, z1);
            this.IPoints[3] = this.Calc3DPoint(p[3].X, p[3].Y, z1);
            this.PolygonFour();
        }

        public static Point PointAtDistance(Point From, Point To, int distance)
        {
            int x = To.X;
            int y = To.Y;
            if (To.X != From.X)
            {
                double a = Math.Atan2((double) (To.Y - From.Y), (double) (To.X - From.X));
                double num4 = Math.Sin(a);
                double num5 = Math.Cos(a);
                x -= Utils.Round((double) (distance * num5));
                y -= Utils.Round((double) (distance * num4));
            }
            else if (To.Y < From.Y)
            {
                y += distance;
            }
            else
            {
                y -= distance;
            }
            return new Point(x, y);
        }

        public PointDouble PointFromCircle(System.Drawing.Rectangle rectBounds, double degrees)
        {
            return this.PointFromCircle(rectBounds, degrees, 0, false);
        }

        public PointDouble PointFromCircle(System.Drawing.Rectangle rectBounds, double degrees, int zPos)
        {
            return this.PointFromCircle(rectBounds, degrees, zPos, false);
        }

        public PointDouble PointFromCircle(System.Drawing.Rectangle rectBounds, double degrees, int zPos, bool clockWise)
        {
            int num;
            int num2;
            RectCenter(rectBounds, out num, out num2);
            double d = (3.1415926535897931 * degrees) / 180.0;
            double num4 = ((float) rectBounds.Width) / 2f;
            double num5 = ((float) rectBounds.Height) / 2f;
            double x = num + (num4 * Math.Cos(d));
            double y = 0.0;
            if (clockWise)
            {
                y = num2 + (num5 * Math.Sin(d));
            }
            else
            {
                y = num2 - (num5 * Math.Sin(d));
            }
            PointDouble empty = PointDouble.Empty;
            if (zPos > 0)
            {
                this.Calc3DPos(ref empty, x, y, (double) zPos);
                return empty;
            }
            return new PointDouble(x, y);
        }

        public PointDouble PointFromEllipse(System.Drawing.Rectangle rectBounds, double degrees)
        {
            return this.PointFromEllipse(rectBounds, degrees, 0);
        }

        public PointDouble PointFromEllipse(System.Drawing.Rectangle rectBounds, double degrees, int zPos)
        {
            int num;
            int num2;
            RectCenter(rectBounds, out num, out num2);
            double d = (3.1415926535897931 * degrees) / 180.0;
            double num4 = ((float) rectBounds.Width) / 2f;
            double num5 = ((float) rectBounds.Height) / 2f;
            bool flag = num4 > num5;
            double num6 = flag ? num4 : num5;
            double num7 = flag ? num5 : num4;
            double num8 = Math.Sqrt(1.0 - ((num7 * num7) / (num6 * num6)));
            double num9 = num7 / Math.Sqrt(1.0 - ((num8 * num8) * Math.Pow(flag ? Math.Cos(d) : Math.Sin(d), 2.0)));
            double x = num + (num9 * Math.Cos(d));
            double y = num2 - (num9 * Math.Sin(d));
            PointDouble empty = PointDouble.Empty;
            if (zPos > 0)
            {
                this.Calc3DPos(ref empty, x, y, (double) zPos);
                return empty;
            }
            return new PointDouble(x, y);
        }

        public PointDouble PointFromSpiral(System.Drawing.Rectangle rectBounds, double degrees, double twist)
        {
            int num;
            int num2;
            RectCenter(rectBounds, out num, out num2);
            double d = (3.1415926535897931 * degrees) / 180.0;
            double num4 = ((float) rectBounds.Width) / 2f;
            double num5 = ((float) rectBounds.Height) / 2f;
            double num6 = (num4 > num5) ? num5 : num4;
            double num7 = num6 * Math.Exp(d * twist);
            double x = num + (num7 * Math.Cos(d));
            double y = num2 + (num7 * Math.Sin(d));
            return new PointDouble(x, y);
        }

        public static bool PointInEllipse(Point p, System.Drawing.Rectangle rect)
        {
            int num;
            int num2;
            RectCenter(rect, out num, out num2);
            int num3 = Utils.Round(Utils.Sqr((double) (num - rect.X)));
            int num4 = Utils.Round(Utils.Sqr((double) (num2 - rect.Y)));
            return (((num3 != 0) && (num4 != 0)) && (((Utils.Sqr((double) (p.X - num)) / ((double) num3)) + (Utils.Sqr((double) (p.Y - num2)) / ((double) num4))) <= 1.0));
        }

        public static bool PointInEllipse(Point p, int left, int top, int right, int bottom)
        {
            return PointInEllipse(p, Utils.FromLTRB(left, top, right, bottom));
        }

        public static bool PointInHorizTriangle(Point p, int y0, int y1, int x0, int x1)
        {
            Point[] poly = new Point[] { new Point(x0, y0), new Point(x1, (y0 + y1) / 2), new Point(x0, y1) };
            return PointInPolygon(p, poly);
        }

        public static bool PointInLine(Point p, Point fromPoint, Point toPoint)
        {
            return PointInLineTolerance(p, fromPoint.X, fromPoint.Y, toPoint.X, toPoint.Y, 0);
        }

        public static bool PointInLineTolerance(Point p, int px, int py, int qx, int qy, int tolerance)
        {
            double num;
            double num2;
            Point pointA = new Point(px, py);
            Point pointB = new Point(qx, qy);
            switch (CalcLineParameters(pointA, pointB, out num2, out num))
            {
                case LineOrientations.Point:
                    tolerance /= 2;
                    return System.Drawing.Rectangle.FromLTRB(pointA.X - tolerance, pointA.Y - tolerance, pointB.X + tolerance, pointB.Y + tolerance).Contains(p.X, p.Y);

                case LineOrientations.Horizontal:
                    if ((p.X < Math.Min(pointA.X, pointB.X)) || (p.X > Math.Max(pointA.X, pointB.X)))
                    {
                        break;
                    }
                    return (Math.Abs((int) (Utils.Round((double) ((num2 * p.X) + num)) - p.Y)) <= tolerance);

                case LineOrientations.Vertical:
                    if ((p.Y < Math.Min(pointA.Y, pointB.Y)) || (p.Y > Math.Max(pointA.Y, pointB.Y)))
                    {
                        break;
                    }
                    return (Math.Abs((int) (Utils.Round((double) ((num2 * p.Y) + num)) - p.X)) <= tolerance);
            }
            return false;
        }

        public static bool PointInPolygon(Point p, Triangle2D triangle)
        {
            Point[] poly = new Point[] { triangle.p0, triangle.p1, triangle.p2 };
            return PointInPolygon(p, poly);
        }

        public static bool PointInPolygon(Point p, params Point[] poly)
        {
            bool flag = false;
            int upperBound = poly.GetUpperBound(0);
            int num2 = upperBound;
            for (int i = 0; i <= num2; i++)
            {
                if ((((poly[i].Y <= p.Y) && (p.Y < poly[upperBound].Y)) || ((poly[upperBound].Y <= p.Y) && (p.Y < poly[i].Y))) && (p.X < ((((poly[upperBound].X - poly[i].X) * (p.Y - poly[i].Y)) / (poly[upperBound].Y - poly[i].Y)) + poly[i].X)))
                {
                    flag = !flag;
                }
                upperBound = i;
            }
            return flag;
        }

        [Obsolete("Please use Rectangle.Contains method."), EditorBrowsable(EditorBrowsableState.Never)]
        public static bool PointInRect(System.Drawing.Rectangle rect, int x, int y)
        {
            return rect.Contains(x, y);
        }

        public static bool PointInTriangle(Point p, Point p0, Point p1, Point p2)
        {
            Point[] poly = new Point[] { p0, p1, p2 };
            return PointInPolygon(p, poly);
        }

        public static bool PointInTriangle(Point p, int x0, int x1, int y0, int y1)
        {
            Point[] poly = new Point[] { new Point(x0, y0), new Point((x0 + x1) / 2, y1), new Point(x1, y0) };
            return PointInPolygon(p, poly);
        }

        public virtual void Polygon(params Point3D[] p)
        {
            Point[] pointArray = new Point[p.Length];
            for (int i = 0; i < pointArray.Length; i++)
            {
                pointArray[i] = this.Calc3DPoint(p[i]);
            }
            this.Polygon(pointArray);
        }

        public abstract void Polygon(params PointDouble[] p);
        public abstract void Polygon(params Point[] p);
        public virtual void Polygon(int z, params Point[] p)
        {
            for (int i = 0; i <= p.GetUpperBound(0); i++)
            {
                p[i] = this.Calc3DPoint(p[i], z);
            }
            this.Polygon(p);
        }

        public System.Drawing.Rectangle PolygonBounds(params Point[] p)
        {
            int length = p.Length;
            if (length > 0)
            {
                System.Drawing.Rectangle rectangle = Utils.FromLTRB(p[0].X, p[0].Y, p[0].X, p[0].Y);
                for (int i = 0; i < length; i++)
                {
                    Point point = p[i];
                    if (point.X < rectangle.Left)
                    {
                        rectangle.X = point.X;
                    }
                    else if (point.X > rectangle.Right)
                    {
                        rectangle.Width = point.X - rectangle.Left;
                    }
                    if (point.Y < rectangle.Top)
                    {
                        rectangle.Y = point.Y;
                    }
                    else if (point.Y > rectangle.Bottom)
                    {
                        rectangle.Height = point.Y - rectangle.Top;
                    }
                }
                return rectangle;
            }
            return System.Drawing.Rectangle.Empty;
        }

        protected internal virtual void PolygonFour()
        {
            this.Polygon(this.IPoints);
        }

        public virtual void PolygonFourDouble()
        {
            this.Polygon(this.IPointDoubles);
        }

        public System.Drawing.Rectangle PolygonRect(params Point[] p)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle();
            if (p.GetUpperBound(0) > 1)
            {
                rectangle.X = p[0].X;
                rectangle.Width = 0;
                rectangle.Y = p[0].Y;
                rectangle.Height = 0;
                for (int i = 1; i <= p.GetUpperBound(0); i++)
                {
                    if (p[i].X < rectangle.X)
                    {
                        rectangle.Width = rectangle.Right - p[i].X;
                        rectangle.X = p[i].X;
                    }
                    if (p[i].X > rectangle.Right)
                    {
                        rectangle.Width += p[i].X - rectangle.Right;
                    }
                    if (p[i].Y < rectangle.Y)
                    {
                        rectangle.Height = rectangle.Bottom - p[i].Y;
                        rectangle.Y = p[i].Y;
                    }
                    if (p[i].Y > rectangle.Bottom)
                    {
                        rectangle.Height += p[i].Y - rectangle.Bottom;
                    }
                }
            }
            return rectangle;
        }

        public System.Drawing.RectangleF PolygonRect(params PointF[] p)
        {
            System.Drawing.RectangleF ef = new System.Drawing.RectangleF();
            if (p.GetUpperBound(0) > 1)
            {
                ef.X = p[0].X;
                ef.Width = 0f;
                ef.Y = p[0].Y;
                ef.Height = 0f;
                for (int i = 1; i <= p.GetUpperBound(0); i++)
                {
                    if (p[i].X < ef.X)
                    {
                        ef.Width = ef.Right - p[i].X;
                        ef.X = p[i].X;
                    }
                    if (p[i].X > ef.Right)
                    {
                        ef.Width += p[i].X - ef.Right;
                    }
                    if (p[i].Y < ef.Y)
                    {
                        ef.Height = ef.Bottom - p[i].Y;
                        ef.Y = p[i].Y;
                    }
                    if (p[i].Y > ef.Bottom)
                    {
                        ef.Height += p[i].Y - ef.Bottom;
                    }
                }
            }
            return ef;
        }

        public abstract void Polyline(params Point[] p);
        public virtual void Polyline(int z, params Point[] p)
        {
            int length = p.Length;
            Point[] pointArray = new Point[length];
            for (int i = 0; i < length; i++)
            {
                pointArray[i] = this.Calc3DPoint(p[i], z);
            }
            this.Polyline(pointArray);
        }

        public virtual void PopMatrix()
        {
        }

        public abstract void PrepareDrawImage();
        public virtual void Projection(int maxDepth, System.Drawing.Rectangle r)
        {
            this.xCenter = Utils.Round((double) ((r.X + (r.Width * 0.5)) + this.RotationCenter.X));
            this.yCenter = Utils.Round((double) ((r.Y + (r.Height * 0.5)) + this.RotationCenter.Y));
            this.zCenter = Utils.Round((double) ((maxDepth * 0.5) + this.RotationCenter.Z));
            this.xCenterOffset = this.xCenter + this.aspect.HorizOffset;
            this.yCenterOffset = this.yCenter + this.aspect.VertOffset;
            this.CalcPerspective(r);
        }

        public virtual void PushMatrix()
        {
        }

        public void Pyramid(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool darkSides)
        {
            this.Pyramid(vertical, r.X, r.Y, r.Right, r.Bottom, z0, z1, darkSides);
        }

        public virtual void Pyramid(bool vertical, int left, int top, int right, int bottom, int z0, int z1, bool darkSides)
        {
            Point point;
            Point point2;
            Point point3;
            Point point5;
            Color c = this.Brush.Solid ? this.Brush.Color : this.BackColor;
            if (vertical)
            {
                if (top != bottom)
                {
                    point = this.Calc3DPoint(left, bottom, z0);
                    point2 = this.Calc3DPoint(right, bottom, z0);
                    point5 = this.Calc3DPoint((left + right) / 2, top, (z0 + z1) / 2);
                    Point[] p = new Point[] { point, point5, point2 };
                    this.Polygon(p);
                    point3 = this.Calc3DPoint(left, bottom, z1);
                    if ((top < bottom) && (point3.Y < point5.Y))
                    {
                        Point[] pointArray2 = new Point[] { point, point5, point3 };
                        this.Polygon(pointArray2);
                    }
                    if (darkSides)
                    {
                        this.InternalApplyDark(c, 0x80);
                    }
                    Point point4 = this.Calc3DPoint(right, bottom, z1);
                    Point[] pointArray3 = new Point[] { point2, point5, point4 };
                    this.Polygon(pointArray3);
                    if ((top < bottom) && (point3.Y < point5.Y))
                    {
                        Point[] pointArray4 = new Point[] { point5, point3, point4 };
                        this.Polygon(pointArray4);
                    }
                }
                if (top >= bottom)
                {
                    if (darkSides)
                    {
                        this.InternalApplyDark(c, 0x40);
                    }
                    this.RectangleY(left, bottom, right, z0, z1);
                }
            }
            else
            {
                if (left != right)
                {
                    point = this.Calc3DPoint(left, top, z0);
                    point2 = this.Calc3DPoint(left, bottom, z0);
                    point5 = this.Calc3DPoint(right, (top + bottom) / 2, (z0 + z1) / 2);
                    Point[] pointArray5 = new Point[] { point, point5, point2 };
                    this.Polygon(pointArray5);
                    if (darkSides)
                    {
                        this.InternalApplyDark(c, 0x40);
                    }
                    point3 = this.Calc3DPoint(left, top, z1);
                    Point[] pointArray6 = new Point[] { point, point5, point3 };
                    this.Polygon(pointArray6);
                }
                if (left >= right)
                {
                    if (darkSides)
                    {
                        this.InternalApplyDark(c, 0x80);
                    }
                    this.RectangleZ(left, top, bottom, z0, z1);
                }
            }
            this.Brush.Color = c;
        }

        public virtual void PyramidTrunc(System.Drawing.Rectangle r, int startZ, int endZ, int truncX, int truncZ)
        {
            new InternalPyramidTrunc { r = r, StartZ = startZ, EndZ = endZ, TruncX = truncX, TruncZ = truncZ }.Draw(this);
        }

        internal static void QuickSortAngle(ref Point[] p, ref double[] angles, int low, int high)
        {
            int index = low;
            int num2 = high;
            double num4 = angles[(index + num2) / 2];
            do
            {
                while (angles[index] < num4)
                {
                    index++;
                }
                while (angles[num2] > num4)
                {
                    num2--;
                }
                if (index <= num2)
                {
                    Point point = p[index];
                    p[index] = p[num2];
                    p[num2] = point;
                    double num3 = angles[index];
                    angles[index] = angles[num2];
                    angles[num2] = num3;
                    index++;
                    num2--;
                }
            }
            while (index <= num2);
            if (num2 > low)
            {
                QuickSortAngle(ref p, ref angles, low, num2);
            }
            if (index < high)
            {
                QuickSortAngle(ref p, ref angles, index, high);
            }
        }

        protected static float Rad2Deg(double radian)
        {
            return (float) ((180.0 * radian) / 3.1415926535897931);
        }

        public abstract void Rectangle(System.Drawing.Rectangle r);
        public virtual void Rectangle(System.Drawing.Rectangle r, int z)
        {
            this.Calc3DPos(ref this.IPoints[0], r.X, r.Y, z);
            this.Calc3DPos(ref this.IPoints[1], r.Right, r.Y, z);
            this.Calc3DPos(ref this.IPoints[2], r.Right, r.Bottom, z);
            this.Calc3DPos(ref this.IPoints[3], r.X, r.Bottom, z);
            this.PolygonFour();
        }

        public void Rectangle(int left, int top, int right, int bottom)
        {
            int num;
            int num2;
            if (left > right)
            {
                num = left - right;
                left = right;
            }
            else
            {
                num = right - left;
            }
            if (top > bottom)
            {
                num2 = top - bottom;
                top = bottom;
            }
            else
            {
                num2 = bottom - top;
            }
            this.Rectangle(new System.Drawing.Rectangle(left, top, num, num2));
        }

        public void Rectangle(int left, int top, int right, int bottom, int z)
        {
            this.Rectangle(Utils.FromLTRB(left, top, right, bottom), z);
        }

        public void RectangleF(System.Drawing.RectangleF r)
        {
            this.Rectangle(Utils.ToRectangle(r));
        }

        public void RectangleF(float left, float top, float right, float bottom, int z)
        {
            float width = right - left;
            float height = bottom - top;
            this.Rectangle(Utils.ToRectangle(new System.Drawing.RectangleF(left, top, width, height)), z);
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use Rectangle method.")]
        public void RectangleWithZ(System.Drawing.Rectangle r, int z)
        {
            this.Rectangle(r, z);
        }

        public virtual void RectangleY(int left, int top, int right, int z0, int z1)
        {
            this.Calc3DPos(ref this.IPoints[0], left, top, z0);
            this.Calc3DPos(ref this.IPoints[1], right, top, z0);
            this.Calc3DPos(ref this.IPoints[2], right, top, z1);
            this.Calc3DPos(ref this.IPoints[3], left, top, z1);
            this.PolygonFour();
        }

        public virtual void RectangleZ(int left, int top, int bottom, int z0, int z1)
        {
            this.Calc3DPos(ref this.IPoints[0], left, top, z0);
            this.Calc3DPos(ref this.IPoints[1], left, top, z1);
            this.Calc3DPos(ref this.IPoints[2], left, bottom, z1);
            this.Calc3DPos(ref this.IPoints[3], left, bottom, z0);
            this.PolygonFour();
        }

        public static void RectCenter(System.Drawing.Rectangle r, out int x, out int y)
        {
            x = (r.Left + r.Right) / 2;
            y = (r.Top + r.Bottom) / 2;
        }

        public System.Drawing.Rectangle RectFromPolygon(int num, params PointDouble[] p)
        {
            return this.RectFromPolygon(num, PointDouble.Round(p));
        }

        public System.Drawing.Rectangle RectFromPolygon(int num, params Point[] p)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(p[0].X, p[0].Y, 0, 0);
            for (int i = 1; i < num; i++)
            {
                if (p[i].X < rectangle.X)
                {
                    rectangle.X = p[i].X;
                }
                else if (p[i].X > rectangle.Right)
                {
                    rectangle.Width = p[i].X - rectangle.X;
                }
                if (p[i].Y < rectangle.Y)
                {
                    rectangle.Y = p[i].Y;
                }
                else if (p[i].Y > rectangle.Bottom)
                {
                    rectangle.Height = p[i].Y - rectangle.Y;
                }
            }
            rectangle.Width++;
            rectangle.Height++;
            return rectangle;
        }

        public System.Drawing.Rectangle RectFromRectZ(System.Drawing.Rectangle r, int z)
        {
            return this.RectFromPolygon(4, this.FourPointsFromRect(r, z));
        }

        public virtual void Rotate(int angle)
        {
        }

        public abstract void RotateLabel(int x, int y, string text, double rotDegree);
        public virtual void RotateLabel(int x, int y, int z, string text, double rotDegree)
        {
            this.Calc3DPos(ref x, ref y, z);
            float size = -1f;
            if (this.IZoomText && (this.IZoomFactor != -1.0))
            {
                size = this.Font.DrawingFont.Size;
                int num2 = Utils.Round(Math.Max((double) 1.0, (double) (this.IZoomFactor * size)));
                if (size != num2)
                {
                    this.Font.Size = num2;
                }
            }
            this.RotateLabel(x, y, text, rotDegree);
            if (this.IZoomText && (this.IZoomFactor != -1.0))
            {
                this.Font.Size = Utils.Round(Math.Max(1f, size));
            }
        }

        internal static void RotatePoint(ref Point p, int ax, int ay, Point tmpCenter, double tmpCos, double tmpSin)
        {
            p.X = tmpCenter.X + Utils.Round((double) ((ax * tmpCos) + (ay * tmpSin)));
            p.Y = tmpCenter.Y + Utils.Round((double) ((-ax * tmpSin) + (ay * tmpCos)));
        }

        internal static Point[] RotateRectangle(System.Drawing.Rectangle r, int angle)
        {
            int num;
            int num2;
            RectCenter(r, out num, out num2);
            Point tmpCenter = new Point(num, num2);
            double a = angle * Utils.PiStep;
            double tmpSin = Math.Sin(a);
            double tmpCos = Math.Cos(a);
            System.Drawing.Rectangle rectangle = r;
            rectangle.Offset(-tmpCenter.X, -tmpCenter.Y);
            Point[] pointArray = new Point[4];
            RotatePoint(ref pointArray[0], rectangle.X, rectangle.Y, tmpCenter, tmpCos, tmpSin);
            RotatePoint(ref pointArray[1], rectangle.Right, rectangle.Y, tmpCenter, tmpCos, tmpSin);
            RotatePoint(ref pointArray[2], rectangle.Right, rectangle.Bottom, tmpCenter, tmpCos, tmpSin);
            RotatePoint(ref pointArray[3], rectangle.X, rectangle.Bottom, tmpCenter, tmpCos, tmpSin);
            return pointArray;
        }

        public void RoundRectangle(System.Drawing.Rectangle r)
        {
            this.RoundRectangle(r, 8, 8);
        }

        public void RoundRectangle(System.Drawing.Rectangle r, int round)
        {
            this.RoundRectangle(r, round, round);
        }

        public void RoundRectangle(System.Drawing.Rectangle r, int roundWidth, int roundHeight)
        {
            PointDouble[] p = this.GetClipRoundRectangle(r, roundWidth, roundHeight, 0);
            this.Polygon(p);
        }

        public abstract void SetClipRegion(Region region);
        protected virtual void SetRotationCenter(PointXYZ val)
        {
            this.irotationCenter = new Point3DDouble();
            this.irotationCenter.X = val.X;
            this.irotationCenter.Y = val.Y;
            this.irotationCenter.Z = val.Z;
            this.Invalidate();
        }

        public virtual void SetTextAlign(StringAlignment a)
        {
            this.stringFormat.Alignment = a;
        }

        public static void ShadowSmooth(Bitmap bitmap, Bitmap back, int left, int top, int width, int height, int horz, int vert, double smoothness, bool fullDraw, Graphics3D aCanvas, bool clip)
        {
            RGB[,] rgbArray;
            int num11;
            int num12;
            int num15;
            int num16;
            int num17;
            int num18;
            int num19;
            int num21;
            int num22;
            double num25;
            bool flag;
            RGB rgb;
            int stride = 0;
            int num2 = 0;
            BitmapData bitmapdata = null;
            BitmapData data2 = null;
            IntPtr zero = IntPtr.Zero;
            IntPtr ptr = IntPtr.Zero;
            CalcLines(out rgbArray, bitmap);
            if (back == null)
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);
                bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
                zero = bitmapdata.Scan0;
                stride = bitmapdata.Stride;
            }
            else
            {
                System.Drawing.Rectangle rectangle2 = new System.Drawing.Rectangle(0, 0, back.Width, back.Height);
                data2 = back.LockBits(rectangle2, ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
                ptr = data2.Scan0;
                num2 = data2.Stride;
            }
            int num5 = width;
            int num6 = height;
            RGB[,] rgbArray2 = new RGB[num6, num5];
            if (horz < 0)
            {
                horz = -horz;
                left -= 2 * horz;
                fullDraw = true;
            }
            if (vert < 0)
            {
                vert = -vert;
                top -= 2 * vert;
                fullDraw = true;
            }
            int num7 = (horz * 2) + 1;
            int num8 = (vert * 2) + 1;
            smoothness = 0.01 * (smoothness + 180.0);
            int[] numArray = new int[num7];
            int index = 0;
            while (index < num7)
            {
                numArray[index] = CalcWeight(index - horz, 0, horz, smoothness);
                index++;
            }
            int num9 = (left < 0) ? -left : 0;
            int num10 = (top < 0) ? -top : 0;
            if (back != null)
            {
                if ((num5 + left) >= back.Width)
                {
                    num11 = (back.Width - left) - 1;
                }
                else
                {
                    num11 = num5 - 1;
                }
                if ((num6 + top) >= back.Height)
                {
                    num12 = (back.Height - top) - 1;
                }
                else
                {
                    num12 = num6 - 1;
                }
            }
            else
            {
                num11 = num5 - 1;
                num12 = num6 - 1;
            }
            int num13 = num5 - num7;
            int num14 = num6 - num8;
            int num20 = num10;
            while (num20 <= num12)
            {
                flag = fullDraw || (num20 >= num14);
                num21 = num9;
                while (num21 <= num11)
                {
                    if (flag || (num21 >= num13))
                    {
                        num15 = 0;
                        num16 = 0;
                        num17 = 0;
                        num18 = 0;
                        num22 = 0;
                        int num23 = num21 - horz;
                        if (num23 < 0)
                        {
                            index = -num23;
                            num23 = 0;
                        }
                        else
                        {
                            index = 0;
                        }
                        while (index < num7)
                        {
                            if (num23 >= num5)
                            {
                                break;
                            }
                            num19 = numArray[index];
                            rgb = rgbArray[num20, num23];  ///WYJ fix, original: rgb = *(rgbArray[num20, num23]);
                            num15 += Convert.ToInt32(rgb.Alpha) * num19;
                            num16 += Convert.ToInt32(rgb.Red) * num19;
                            num17 += Convert.ToInt32(rgb.Green) * num19;
                            num18 += Convert.ToInt32(rgb.Blue) * num19;
                            num22 += num19;
                            index++;
                            num23++;
                        }
                        if (num22 == 0)
                        {
                            rgbArray2[num20, num21].Alpha = Convert.ToByte(num15);
                            rgbArray2[num20, num21].Red = Convert.ToByte(num16);
                            rgbArray2[num20, num21].Green = Convert.ToByte(num17);
                            rgbArray2[num20, num21].Blue = Convert.ToByte(num18);
                        }
                        else
                        {
                            num25 = 1.0 / ((double) num22);
                            rgbArray2[num20, num21].Alpha = Convert.ToByte(Utils.Round((double) (num15 * num25)));
                            rgbArray2[num20, num21].Red = Convert.ToByte(Utils.Round((double) (num16 * num25)));
                            rgbArray2[num20, num21].Green = Convert.ToByte(Utils.Round((double) (num17 * num25)));
                            rgbArray2[num20, num21].Blue = Convert.ToByte(Utils.Round((double) (num18 * num25)));
                        }
                    }
                    num21++;
                }
                num20++;
            }
            numArray = new int[num8];
            index = 0;
            while (index < num8)
            {
                numArray[index] = CalcWeight(index - vert, 0, vert, smoothness);
                index++;
            }
            for (num21 = num9; num21 <= num11; num21++)
            {
                flag = fullDraw || (num21 >= num13);
                for (num20 = num10; num20 <= num12; num20++)
                {
                    int num3;
                    if (!flag && (num20 < num14))
                    {
                        continue;
                    }
                    num15 = 0;
                    num16 = 0;
                    num17 = 0;
                    num18 = 0;
                    num22 = 0;
                    int num24 = num20 - vert;
                    if (num24 < 0)
                    {
                        index = -num24;
                        num24 = 0;
                    }
                    else
                    {
                        index = 0;
                    }
                    while (index < num8)
                    {
                        if (num24 >= num6)
                        {
                            break;
                        }
                        num19 = numArray[index];
                        rgb = rgbArray2[num24, num21];  ///WYJ fix, original: rgb = *(rgbArray2[num24, num21]);
                        num15 += Convert.ToInt32(rgb.Alpha) * num19;
                        num16 += Convert.ToInt32(rgb.Red) * num19;
                        num17 += Convert.ToInt32(rgb.Green) * num19;
                        num18 += Convert.ToInt32(rgb.Blue) * num19;
                        num22 += num19;
                        index++;
                        num24++;
                    }
                    if (back != null)
                    {
                        num3 = ((num20 + top) * num2) + (4 * (num21 + left));
                        if (((num16 == num17) && (num16 == num18)) && (num16 == num15))
                        {
                            num25 = num16 * (0.00392156862745098 / ((double) num22));
                            Marshal.WriteByte(ptr, num3 + 3, (byte) Utils.Round((double) (num25 * Convert.ToInt32(Marshal.ReadByte(ptr, num3 + 3)))));
                            Marshal.WriteByte(ptr, num3 + 2, (byte) Utils.Round((double) (num25 * Convert.ToInt32(Marshal.ReadByte(ptr, num3 + 2)))));
                            Marshal.WriteByte(ptr, num3 + 1, (byte) Utils.Round((double) (num25 * Convert.ToInt32(Marshal.ReadByte(ptr, num3 + 1)))));
                            Marshal.WriteByte(ptr, num3, (byte) Utils.Round((double) (num25 * Convert.ToInt32(Marshal.ReadByte(ptr, num3)))));
                        }
                        else
                        {
                            num25 = 0.00392156862745098 / ((double) num22);
                            Marshal.WriteByte(ptr, num3 + 3, (byte) Utils.Round((double) ((num15 * num25) * Convert.ToInt32(Marshal.ReadByte(ptr, num3 + 3)))));
                            Marshal.WriteByte(ptr, num3 + 2, (byte) Utils.Round((double) ((num16 * num25) * Convert.ToInt32(Marshal.ReadByte(ptr, num3 + 2)))));
                            Marshal.WriteByte(ptr, num3 + 1, (byte) Utils.Round((double) ((num17 * num25) * Convert.ToInt32(Marshal.ReadByte(ptr, num3 + 1)))));
                            Marshal.WriteByte(ptr, num3, (byte) Utils.Round((double) ((num18 * num25) * Convert.ToInt32(Marshal.ReadByte(ptr, num3)))));
                        }
                    }
                    else
                    {
                        num3 = (num20 * stride) + (4 * num21);
                        num25 = 1.0 / ((double) num22);
                        Marshal.WriteByte(zero, num3 + 3, (byte) Utils.Round((double) (num15 * num25)));
                        Marshal.WriteByte(zero, num3 + 2, (byte) Utils.Round((double) (num16 * num25)));
                        Marshal.WriteByte(zero, num3 + 1, (byte) Utils.Round((double) (num17 * num25)));
                        Marshal.WriteByte(zero, num3, (byte) Utils.Round((double) (num18 * num25)));
                    }
                }
            }
            numArray = null;
            rgbArray = null;
            if (back == null)
            {
                bitmap.UnlockBits(bitmapdata);
            }
            else
            {
                back.UnlockBits(data2);
            }
            aCanvas.Draw(0, 0, back);
        }

        public virtual void ShowImage(Graphics g)
        {
            this.DisposeObj();
            this.Dirty = false;
        }

        internal static Point[] SliceArray(ref Array source, int length)
        {
            Point[] destinationArray = new Point[length];
            Array.Copy(source, destinationArray, length);
            return destinationArray;
        }

        internal static PointDouble[] SliceArrayD(ref Array source, int length)
        {
            PointDouble[] destinationArray = new PointDouble[length];
            Array.Copy(source, destinationArray, length);
            return destinationArray;
        }

        internal static PointDouble[] SliceArrayDouble(ref Array source, int length)
        {
            PointDouble[] destinationArray = new PointDouble[length];
            Array.Copy(source, destinationArray, length);
            return destinationArray;
        }

        private Color SpecialColor(Color baseColor)
        {
            if (baseColor == Color.FromArgb(baseColor.A, 0xff, 0xff, 0xff))
            {
                baseColor = Color.FromArgb(baseColor.A, 0xd4, 0xd4, 0xd4);
            }
            if (baseColor == Color.FromArgb(baseColor.A, 0xff, 0xff, 0))
            {
                baseColor = Color.FromArgb(baseColor.A, 0xd4, 0xd4, 0);
            }
            return baseColor;
        }

        public virtual void Sphere(int x, int y, int z, double radius)
        {
            int num = Utils.Round(radius);
            this.Ellipse(x - num, y - num, x + num, y + num, z);
        }

        public virtual void SphereEnh(int x1, int y1, int x2, int y2)
        {
            Color baseColor = this.Brush.Color;
            this.Brush.Color = this.SpecialColor(baseColor);
            int transparency = this.Brush.Transparency;
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            int width = Utils.Round((double) (r.Width * 0.84));
            new System.Drawing.Rectangle(r.Left + Utils.Round((double) (r.Width * 0.08)), r.Top + Utils.Round((double) (r.Height * 0.03)), width, Utils.Round((double) (r.Height * 0.94)));
            this.Pen.Visible = false;
            System.Drawing.Rectangle chartRect = base.chart.ChartRect;
            Region region = this.AddRightRegion(chartRect, 0, base.chart.Aspect.Width3D);
            if (!this.hasClipRegion)
            {
                this.MixClip(region);
            }
            this.Ellipse(r);
            if (((x2 - x1) < 5) || ((y2 - y1) < 5))
            {
                if (!this.hasClipRegion)
                {
                    this.UnClip();
                }
            }
            else
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(r);
                Region region2 = new Region(path);
                region2.Intersect(region);
                if (!this.hasClipRegion)
                {
                    this.MixClip(region2);
                }
                GraphicsPath path2 = new GraphicsPath();
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(r.X - (r.Width / 10), r.Y - (r.Height / 10), r.Width, r.Height);
                if ((rect.Width < 5) || (rect.Height < 5))
                {
                    if (!this.hasClipRegion)
                    {
                        this.UnClip();
                    }
                }
                else
                {
                    path2.AddEllipse(rect);
                    PathGradientBrush brush = new PathGradientBrush(path2);
                    Color c = baseColor;
                    ApplyBright(ref c, 0x80);
                    brush.CenterColor = c;
                    Color[] colorArray = new Color[] { Color.Transparent };
                    brush.SurroundColors = colorArray;
                    this.FillRectangle(brush, r.X, r.Y, r.Width, r.Height);
                    if (!this.hasClipRegion)
                    {
                        this.UnClip();
                    }
                }
            }
        }

        public virtual void SphereEnh(int x1, int y1, int x2, int y2, int z)
        {
            this.Calc3DPos(ref x1, ref y1, z);
            this.Calc3DPos(ref x2, ref y2, z);
            this.SphereEnh(x1, y1, x2, y2);
        }

        public virtual void StartBlending(double transparency)
        {
        }

        public virtual void Surface3D(SurfaceStyle style, bool sameBrush, int numXValues, int numZValues, Surface surface)
        {
        }

        public virtual bool TeeCull(Point[] P)
        {
            return this.TeeCull(P[0], P[1], P[2]);
        }

        public virtual bool TeeCull(Point p0, Point p1, Point p2)
        {
            return ((((p0.X - p1.X) * (p2.Y - p1.Y)) - ((p2.X - p1.X) * (p0.Y - p1.Y))) < 0);
        }

        public virtual float TextHeight(string text)
        {
            return this.MeasureString(this.Font, text).Height;
        }

        public virtual float TextHeight(ChartFont f, string text)
        {
            return this.MeasureString(f, text).Height;
        }

        public virtual void TextOut(int x, int y, string text)
        {
            this.TextOut(this.Font, x, y, text);
        }

        public void TextOut(ChartFont f, int x, int y, string text)
        {
            if (f.ShouldDrawShadow())
            {
                this.DoDrawString(x + f.shadow.Width, y + f.shadow.Height, text, f.shadow.Brush);
            }
            this.DoDrawString(x, y, text, f.Brush);
        }

        public virtual void TextOut(int x, int y, int z, string text)
        {
            this.Calc3DPos(ref x, ref y, z);
            float size = -1f;
            if (this.IZoomText && (this.IZoomFactor != -1.0))
            {
                size = this.Font.DrawingFont.Size;
                int num2 = Utils.Round(Math.Max((double) 1.0, (double) (this.IZoomFactor * size)));
                if (size != num2)
                {
                    this.Font.Size = num2;
                }
            }
            this.TextOut(x, y, text);
            if (this.IZoomText && (this.IZoomFactor != -1.0))
            {
                this.Font.Size = Utils.Round(Math.Max(1f, size));
            }
        }

        public virtual float TextWidth(string text)
        {
            return this.MeasureString(this.Font, text).Width;
        }

        public virtual float TextWidth(ChartFont f, string text)
        {
            return this.MeasureString(f, text).Width;
        }

        public virtual void Translate(int x, int y, int z)
        {
        }

        public static int Transparency(Color color)
        {
            int a = color.A;
            if (a != 0xff)
            {
                return Utils.Round((double) (0.39216 * (0xff - a)));
            }
            return 0;
        }

        public static Color TransparentColor(int transparency, Color color)
        {
            return Color.FromArgb(Utils.Round((double) ((100 - transparency) * 2.55)), color.R, color.G, color.B);
        }

        protected internal abstract void TransparentEllipse(int x1, int y1, int x2, int y2);
        internal void TransparentEllipse(int x1, int y1, int x2, int y2, int z)
        {
            this.Calc3DPos(ref x1, ref y1, z);
            this.Calc3DPos(ref x2, ref y2, z);
            this.TransparentEllipse(x1, y1, x2, y2);
        }

        public virtual void Triangle(Triangle3D p)
        {
            Point[] pointArray = new Point[] { this.Calc3DPoint(p.p0), this.Calc3DPoint(p.p1), this.Calc3DPoint(p.p2) };
            this.Polygon(pointArray);
        }

        public virtual void Triangle(Point p0, Point p1, Point p2, int z)
        {
            p0 = this.Calc3DPoint(p0.X, p0.Y, z);
            p1 = this.Calc3DPoint(p1.X, p1.Y, z);
            p2 = this.Calc3DPoint(p2.X, p2.Y, z);
            Point[] p = new Point[] { p0, p1, p2 };
            this.Polygon(p);
        }

        public abstract void UnClip();
        [Obsolete("Please use UnClip() method")]
        public void UnClipRectangle()
        {
            this.UnClip();
        }

        public virtual bool ValidState()
        {
            return true;
        }

        public abstract void VerticalLine(int x, int top, int bottom);
        public virtual void VerticalLine(int x, int top, int bottom, int z)
        {
            int num = x;
            this.Calc3DPos(ref num, ref top, z);
            this.Calc3DPos(ref x, ref bottom, z);
            this.Line(num, top, x, bottom);
        }

        public virtual void ZLine(int x, int y, int z0, int z1)
        {
            int num = x;
            int num2 = y;
            this.Calc3DPos(ref x, ref y, z0);
            this.Calc3DPos(ref num, ref num2, z1);
            this.Line(x, y, num, num2);
        }

        public BufferedGraphics BackBuffer
        {
            get
            {
                return this.backBuffer;
            }
            set
            {
                this.backBuffer = value;
            }
        }

        public BufferedGraphicsContext BackBufferContext
        {
            get
            {
                if (this.backBufferContext == null)
                {
                    this.backBufferContext = BufferedGraphicsManager.Current;
                }
                return this.backBufferContext;
            }
            set
            {
                this.backBufferContext = value;
            }
        }

        [Description("Sets / returns the color used to fill behind text or non-solid brush styles.")]
        public Color BackColor
        {
            get
            {
                return this.Brush.Color;
            }
            set
            {
                this.Brush.Color = value;
            }
        }

        [Description("Determines Brush used to fill the Canvas draw rectangle background.")]
        public ChartBrush Brush
        {
            get
            {
                if (this.bBrush == null)
                {
                    this.bBrush = new ChartBrush(null);
                }
                return this.bBrush;
            }
            set
            {
                this.bBrush = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Description(" Gets the type of canvas used to draw the chart.")]
        public Steema.TeeChart.Drawing.CanvasType CanvasType
        {
            get
            {
                return this.iCanvasType;
            }
        }

        [Description("Returns the centre Horizontal co-ordinate of the Chart.")]
        public int ChartXCenter
        {
            get
            {
                return this.XCenter;
            }
        }

        [Description("Returns the middle Vertical coordinate of the Chart.")]
        public int ChartYCenter
        {
            get
            {
                return this.YCenter;
            }
        }

        public static Color[] ColorPalette
        {
            get
            {
                if (colorPalette == null)
                {
                    colorPalette = Theme.TeeChartPalette;
                }
                return colorPalette;
            }
            set
            {
                colorPalette = value;
            }
        }

        [Description("Determines Font for outputted text when using the Drawing.")]
        public ChartFont Font
        {
            get
            {
                if (this.ifont == null)
                {
                    this.ifont = new ChartFont(null);
                }
                return this.ifont;
            }
            set
            {
                this.ifont = value;
            }
        }

        [Description("Defines the Height of the Font in pixels.")]
        public int FontHeight
        {
            get
            {
                return Utils.Round(this.TextHeight("W"));
            }
        }

        [Description("Gets and sets the Brush.Gradient properties of the Canvas.")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
            set
            {
                this.Brush.Gradient = value;
            }
        }

        internal double IZoomfactor
        {
            get
            {
                return this.IZoomFactor;
            }
            set
            {
                this.IZoomFactor = value;
            }
        }

        [Description("Indicates the kind of pen used to draw Canvas lines.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(Color.Black);
                }
                return this.pen;
            }
            set
            {
                this.pen = value;
            }
        }

        [Description("Sets the Pixel location (using X,Y,Z) of the centre of rotation.")]
        public PointXYZ RotationCenter
        {
            get
            {
                return this.GetRotationCenter();
            }
            set
            {
                this.SetRotationCenter(value);
            }
        }

        [Description("Sets the Pixel location (using X,Y,Z) of the centre of rotation.")]
        public Point3DDouble RotationCenterFloat
        {
            get
            {
                return this.irotationCenter;
            }
            set
            {
                this.irotationCenter = value;
                this.Invalidate();
            }
        }

        [Description("Returns the height, in pixels, of the Chart Panel.")]
        public static int ScreenHeight
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Height;
            }
        }

        [Description("Returns the width, in pixels, of the Chart Panel.")]
        public static int ScreenWidth
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Width;
            }
        }

        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            get
            {
                return this.aSmoothingMode;
            }
            set
            {
                if (this.aSmoothingMode != value)
                {
                    this.aSmoothingMode = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Returns if Canvas supports 3D Text or not.")]
        public bool Supports3DText
        {
            get
            {
                return this.GetSupports3DText();
            }
            set
            {
                this.supports3DText = value;
            }
        }

        [Description("Returns if Canvas can do rotation and elevation of more than 90 degree.")]
        public virtual bool SupportsFullRotation
        {
            get
            {
                return false;
            }
        }

        [Description("Sets the alignment used when displaying text using TextOut or TextOut3D."), DefaultValue(0)]
        public StringAlignment TextAlign
        {
            get
            {
                return this.stringFormat.Alignment;
            }
            set
            {
                this.SetTextAlign(value);
            }
        }

        public System.Drawing.Text.TextRenderingHint TextRenderingHint
        {
            get
            {
                return this.aTextRenderingHint;
            }
            set
            {
                if (this.aTextRenderingHint != value)
                {
                    this.aTextRenderingHint = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Draws items to an internal canvas to prevent flickering on screen.")]
        public bool UseBuffer
        {
            get
            {
                return this.buffered;
            }
            set
            {
                this.buffered = value;
                if (base.chart.parent != null)
                {
                    base.chart.parent.DoSetControlStyle();
                }
            }
        }

        [Description("Obtain the X coordinate of the pixel location of the center of the 3D Canvas.")]
        public int XCenter
        {
            get
            {
                return this.xCenter;
            }
            set
            {
                this.xCenter = value;
            }
        }

        [Description("Obtain the Y coordinate of the pixel location of the center of the 3D Canvas.")]
        public int YCenter
        {
            get
            {
                return this.yCenter;
            }
            set
            {
                this.yCenter = value;
            }
        }

        private class ArrowPoint
        {
            internal double CosA;
            internal Graphics3D g;
            internal double SinA;
            internal double x;
            internal double y;
            internal int z;

            public Point Calc()
            {
                Point p = new Point(Utils.Round((double) ((this.x * this.CosA) + (this.y * this.SinA))), Utils.Round((double) ((-this.x * this.SinA) + (this.y * this.CosA))));
                return this.g.Calc3DPoint(p, this.z);
            }
        }

        private class InternalPyramidTrunc
        {
            internal int EndZ;
            internal Graphics3D g;
            internal int MidX;
            internal int MidZ;
            internal Rectangle r;
            internal int StartZ;
            internal int TruncX;
            internal int TruncZ;

            private void BottomCover()
            {
                this.g.RectangleY(this.r.X, this.r.Bottom, this.r.Right, this.StartZ, this.EndZ);
            }

            public void Draw(Graphics3D gr)
            {
                this.g = gr;
                this.MidX = (this.r.X + this.r.Right) / 2;
                this.MidZ = (this.StartZ + this.EndZ) / 2;
                if (this.r.Bottom > this.r.Y)
                {
                    this.BottomCover();
                }
                else
                {
                    this.TopCover();
                }
                this.FrontWall(this.MidZ + this.TruncZ, this.EndZ);
                this.SideWall(this.r.X, this.MidX - this.TruncX, this.StartZ, this.EndZ);
                this.FrontWall(this.MidZ - this.TruncZ, this.StartZ);
                this.SideWall(this.r.Right, this.MidX + this.TruncX, this.StartZ, this.EndZ);
                if (this.r.Bottom > this.r.Y)
                {
                    this.TopCover();
                }
                else
                {
                    this.BottomCover();
                }
            }

            private void FrontWall(int StartZ, int EndZ)
            {
                Point[] p = new Point[4];
                p[0].X = this.MidX - this.TruncX;
                p[0].Y = this.r.Y;
                p[1].X = this.MidX + this.TruncX;
                p[1].Y = this.r.Y;
                p[2].X = this.r.Right;
                p[2].Y = this.r.Bottom;
                p[3].X = this.r.X;
                p[3].Y = this.r.Bottom;
                this.g.PlaneFour3D(StartZ, EndZ, p);
            }

            private void SideWall(int HorizPos1, int HorizPos2, int StartZ, int EndZ)
            {
                this.g.IPoints[0] = this.g.Calc3DPoint(HorizPos2, this.r.Y, this.MidZ - this.TruncZ);
                this.g.IPoints[1] = this.g.Calc3DPoint(HorizPos2, this.r.Y, this.MidZ + this.TruncZ);
                this.g.IPoints[2] = this.g.Calc3DPoint(HorizPos1, this.r.Bottom, EndZ);
                this.g.IPoints[3] = this.g.Calc3DPoint(HorizPos1, this.r.Bottom, StartZ);
                this.g.PolygonFour();
            }

            private void TopCover()
            {
                if (this.TruncX != 0)
                {
                    this.g.RectangleY(this.MidX - this.TruncX, this.r.Y, this.MidX + this.TruncX, this.MidZ - this.TruncZ, this.MidZ + this.TruncZ);
                }
            }
        }

        private enum LineOrientations
        {
            Point,
            Horizontal,
            Vertical
        }

        public class Pie3D
        {
            private bool drawEntirePie;
            private PointDouble end0;
            private PointDouble end1;
            private PointDouble endB0;
            private PointDouble endB1;
            private PointDouble endD0;
            private PointDouble endD1;
            private Graphics3D g;
            private ChartBrush gradientBrush;
            private bool hasBevel;
            private bool iDrawSides;
            private int iEndAngle;
            private bool isDonut;
            private int iStartAngle;
            private ArrayList list = new ArrayList();
            private PointDouble middle0;
            private PointDouble middle1;
            public Color OldColor;
            private PointDouble[] points;
            private PointDouble start0;
            private PointDouble start1;
            private PointDouble startB0;
            private PointDouble startB1;
            private PointDouble startD0;
            private PointDouble startD1;
            private int sweepAngle;
            private Rectangle z0BevelRect;
            private Rectangle z0DonutRect;
            private Rectangle z0Rect;
            private Rectangle z1BevelRect;
            private Rectangle z1DonutRect;
            private Rectangle z1Rect;
            private int zPos;

            public Pie3D(Graphics3D graphics)
            {
                this.g = graphics;
            }

            private void DoCurvedGradient(int zDepth)
            {
                this.DoTopGradient(zDepth);
                if ((((this.iStartAngle <= 0x127) && (this.iEndAngle >= 0x14f)) || ((this.iStartAngle >= 0x127) && (this.iStartAngle <= 0x14f))) || ((this.iEndAngle >= 0x127) && (this.iEndAngle <= 0x14f)))
                {
                    double iStartAngle;
                    double iEndAngle;
                    this.list.Clear();
                    for (double i = 295.0; i <= 335.0; i++)
                    {
                        this.list.Add(this.PointD(this.z1BevelRect, i, zDepth));
                    }
                    for (double j = 335.0; j >= 295.0; j--)
                    {
                        this.list.Add(this.PointD(this.z0BevelRect, j, zDepth));
                    }
                    PointDouble[] numArray = (PointDouble[]) this.list.ToArray(typeof(PointDouble));
                    this.g.Brush.Gradient.Style.CenterXOffset = 0;
                    this.g.Brush.Gradient.Style.CenterYOffset = 0;
                    this.g.Brush.Gradient.CustomTargetPolygon = PointDouble.Round(numArray);
                    this.list.Clear();
                    if ((this.iStartAngle <= 0x127) && (this.iEndAngle >= 0x14f))
                    {
                        iStartAngle = 295.0;
                        iEndAngle = 335.0;
                    }
                    else if ((this.iStartAngle >= 0x127) && (this.iStartAngle <= 0x14f))
                    {
                        iStartAngle = this.iStartAngle;
                        iEndAngle = 335.0;
                    }
                    else if ((this.iEndAngle >= 0x127) && (this.iEndAngle <= 0x14f))
                    {
                        iStartAngle = 295.0;
                        iEndAngle = this.iEndAngle;
                    }
                    else
                    {
                        iStartAngle = 0.0;
                        iEndAngle = 0.0;
                    }
                    for (double k = iStartAngle; k <= iEndAngle; k++)
                    {
                        this.list.Add(this.PointD(this.z1BevelRect, k, zDepth));
                    }
                    for (double m = iEndAngle; m >= iStartAngle; m--)
                    {
                        this.list.Add(this.PointD(this.z0BevelRect, m, zDepth));
                    }
                    this.g.Polygon((PointDouble[]) this.list.ToArray(typeof(PointDouble)));
                }
            }

            private void DoFlatGradient(int zDepth)
            {
                this.DoTopGradient(zDepth);
                if ((((this.iStartAngle <= 0x127) && (this.iEndAngle >= 0x14f)) || ((this.iStartAngle >= 0x127) && (this.iStartAngle <= 0x14f))) || ((this.iEndAngle >= 0x127) && (this.iEndAngle <= 0x14f)))
                {
                    double iStartAngle;
                    double iEndAngle;
                    this.list.Clear();
                    for (double i = 295.0; i <= 335.0; i++)
                    {
                        this.list.Add(this.PointD(this.z1BevelRect, i, zDepth));
                    }
                    for (double j = 335.0; j >= 295.0; j--)
                    {
                        this.list.Add(this.PointD(this.z0BevelRect, j, zDepth));
                    }
                    PointDouble[] numArray = (PointDouble[]) this.list.ToArray(typeof(PointDouble));
                    this.gradientBrush.Gradient.Style.Visible = false;
                    this.gradientBrush.Gradient.Angle = 135.0;
                    this.gradientBrush.Gradient.CustomTargetPolygon = PointDouble.Round(numArray);
                    this.list.Clear();
                    if ((this.iStartAngle <= 0x127) && (this.iEndAngle >= 0x14f))
                    {
                        iStartAngle = 295.0;
                        iEndAngle = 335.0;
                    }
                    else if ((this.iStartAngle >= 0x127) && (this.iStartAngle <= 0x14f))
                    {
                        iStartAngle = this.iStartAngle;
                        iEndAngle = 335.0;
                    }
                    else if ((this.iEndAngle >= 0x127) && (this.iEndAngle <= 0x14f))
                    {
                        iStartAngle = 295.0;
                        iEndAngle = this.iEndAngle;
                    }
                    else
                    {
                        iStartAngle = 0.0;
                        iEndAngle = 0.0;
                    }
                    for (double k = iStartAngle; k <= iEndAngle; k++)
                    {
                        this.list.Add(this.PointD(this.z1BevelRect, k, zDepth));
                    }
                    for (double m = iEndAngle; m >= iStartAngle; m--)
                    {
                        this.list.Add(this.PointD(this.z0BevelRect, m, zDepth));
                    }
                    this.g.Polygon((PointDouble[]) this.list.ToArray(typeof(PointDouble)));
                }
            }

            private void DoTopGradient(int zDepth)
            {
                this.g.Pen.Visible = false;
                this.list.Clear();
                for (double i = 0.0; i <= 360.0; i++)
                {
                    this.list.Add(this.PointD(this.z0BevelRect, i, zDepth));
                }
                PointDouble[] numArray = (PointDouble[]) this.list.ToArray(typeof(PointDouble));
                this.gradientBrush.Gradient.Visible = true;
                this.gradientBrush.Gradient.Style.Visible = true;
                this.gradientBrush.Gradient.StartColor = Color.Transparent;
                this.gradientBrush.Gradient.MiddleColor = Color.White;
                this.gradientBrush.Gradient.EndColor = Color.Transparent;
                this.gradientBrush.Gradient.Style.Direction = PathGradientMode.Radial;
                this.gradientBrush.Gradient.Style.CenterXOffset = -Utils.Round((double) (((double) this.z0BevelRect.Width) / 2.0));
                this.gradientBrush.Gradient.Style.CenterYOffset = -Utils.Round((double) (((double) this.z0BevelRect.Height) / 2.0));
                this.gradientBrush.Gradient.CustomTargetPolygon = PointDouble.Round(numArray);
                this.list.Clear();
                if (!this.isDonut)
                {
                    this.list.Add(this.middle0);
                }
                for (double j = this.iStartAngle; j <= this.iEndAngle; j++)
                {
                    this.list.Add(this.PointD(this.z0BevelRect, j, zDepth));
                }
                if (this.isDonut)
                {
                    for (int k = this.iEndAngle; k >= this.iStartAngle; k--)
                    {
                        this.list.Add(this.PointD(this.z0DonutRect, (double) k, zDepth));
                    }
                }
                this.g.Polygon((PointDouble[]) this.list.ToArray(typeof(PointDouble)));
            }

            private void DrawBack(int startAng, int endAng)
            {
                bool isDonut = this.isDonut;
                this.list.Clear();
                if (((!this.drawEntirePie && isDonut) && ((this.start0.Y > this.middle0.Y) && (this.end0.Y > this.middle0.Y))) && (this.sweepAngle < 180))
                {
                    isDonut = false;
                }
                if (isDonut)
                {
                    for (int i = startAng; i <= endAng; i++)
                    {
                        this.list.Add(this.PointD(this.z0DonutRect, (double) i, this.zPos));
                    }
                    for (int j = endAng; j >= startAng; j--)
                    {
                        this.list.Add(this.PointD(this.z1DonutRect, (double) j, this.zPos));
                    }
                    this.DrawPoints();
                }
            }

            private void DrawBevel(int startAng, int endAng)
            {
                this.DrawBevel(startAng, endAng, false);
            }

            private void DrawBevel(int startAng, int endAng, bool split)
            {
                if (this.hasBevel)
                {
                    int num = startAng;
                    int num2 = endAng;
                    if (split)
                    {
                        bool visible = this.g.Pen.Visible;
                        this.g.Pen.Visible = false;
                        for (int i = 180; i >= 0; i--)
                        {
                            this.list.Add(this.PointD(this.z0BevelRect, (double) i, this.zPos));
                        }
                        for (int j = 0; j <= 180; j++)
                        {
                            this.list.Add(this.PointD(this.z1BevelRect, (double) j, this.zPos));
                        }
                        this.DrawPoints();
                        for (int k = num2; k >= 180; k--)
                        {
                            this.list.Add(this.PointD(this.z1BevelRect, (double) k, this.zPos));
                        }
                        for (int m = 180; m <= num2; m++)
                        {
                            this.list.Add(this.PointD(this.z0BevelRect, (double) m, this.zPos));
                        }
                        this.DrawPoints();
                        if (num > 0)
                        {
                            for (int n = 360; n >= num; n--)
                            {
                                this.list.Add(this.PointD(this.z1BevelRect, (double) n, this.zPos));
                            }
                            for (int num8 = num; num8 <= 360; num8++)
                            {
                                this.list.Add(this.PointD(this.z0BevelRect, (double) num8, this.zPos));
                            }
                            this.DrawPoints();
                        }
                        this.g.Pen.Visible = visible;
                    }
                    else
                    {
                        if (num2 < num)
                        {
                            num2 += 360;
                        }
                        for (int num9 = num; num9 <= num2; num9++)
                        {
                            this.list.Add(this.PointD(this.z0BevelRect, (double) num9, this.zPos));
                        }
                        for (int num10 = num2; num10 >= num; num10--)
                        {
                            this.list.Add(this.PointD(this.z1BevelRect, (double) num10, this.zPos));
                        }
                    }
                    this.DrawPoints();
                }
            }

            private void DrawBottom(int startAng, int endAng)
            {
                if (this.drawEntirePie)
                {
                    if ((this.sweepAngle < 360) && !this.isDonut)
                    {
                        this.list.Add(this.middle1);
                    }
                    for (int i = startAng; i <= endAng; i++)
                    {
                        this.list.Add(this.PointD(this.z1Rect, (double) i, this.zPos));
                    }
                    if (this.isDonut)
                    {
                        for (int j = endAng; j >= startAng; j--)
                        {
                            this.list.Add(this.PointD(this.z1DonutRect, (double) j, this.zPos));
                        }
                    }
                    if ((this.sweepAngle < 360) && !this.isDonut)
                    {
                        this.list.Add(this.middle1);
                    }
                    this.DrawPoints();
                }
            }

            private void DrawFront(int startAng, int endAng)
            {
                bool flag = true;
                if ((!this.drawEntirePie && (this.start0.Y < this.middle0.Y)) && ((this.end0.Y < this.middle0.Y) && (this.sweepAngle < 180)))
                {
                    flag = false;
                    this.DrawBevel(startAng, endAng);
                }
                if (flag)
                {
                    this.list.Clear();
                    int num = endAng;
                    int num2 = startAng;
                    while (num > 360)
                    {
                        num -= 360;
                    }
                    while (num < -360)
                    {
                        num += 360;
                    }
                    while (num2 > 360)
                    {
                        num2 -= 360;
                    }
                    while (num2 < -360)
                    {
                        num2 += 360;
                    }
                    if (num < 0)
                    {
                        num = 360 + num;
                    }
                    if (num2 < 0)
                    {
                        num2 = 360 + num2;
                    }
                    if ((this.start0.Y > this.middle0.Y) && (this.end0.Y < this.middle0.Y))
                    {
                        if (this.drawEntirePie)
                        {
                            if (this.hasBevel)
                            {
                                for (int j = 0; j <= num; j++)
                                {
                                    this.list.Add(this.PointD(this.z1BevelRect, (double) j, this.zPos));
                                }
                            }
                            else
                            {
                                for (int k = 0; k <= num; k++)
                                {
                                    this.list.Add(this.PointD(this.z0Rect, (double) k, this.zPos));
                                }
                            }
                            for (int i = num; i >= 0; i--)
                            {
                                this.list.Add(this.PointD(this.z1Rect, (double) i, this.zPos));
                            }
                            this.DrawPoints();
                        }
                        if (num2 > 0)
                        {
                            for (int m = 360; m >= num2; m--)
                            {
                                this.list.Add(this.PointD(this.z1Rect, (double) m, this.zPos));
                            }
                            if (this.hasBevel)
                            {
                                for (int n = num2; n <= 360; n++)
                                {
                                    this.list.Add(this.PointD(this.z1BevelRect, (double) n, this.zPos));
                                }
                            }
                            else
                            {
                                for (int num8 = num2; num8 <= 360; num8++)
                                {
                                    this.list.Add(this.PointD(this.z0Rect, (double) num8, this.zPos));
                                }
                            }
                            this.DrawPoints();
                        }
                        this.DrawBevel(num2, num);
                    }
                    else if ((this.start0.Y < this.middle0.Y) && (this.end0.Y > this.middle0.Y))
                    {
                        if (this.drawEntirePie)
                        {
                            if (this.hasBevel)
                            {
                                for (int num9 = 180; num9 >= num2; num9--)
                                {
                                    this.list.Add(this.PointD(this.z1BevelRect, (double) num9, this.zPos));
                                }
                            }
                            else
                            {
                                for (int num10 = 180; num10 >= num2; num10--)
                                {
                                    this.list.Add(this.PointD(this.z0Rect, (double) num10, this.zPos));
                                }
                            }
                            for (int num11 = num2; num11 <= 180; num11++)
                            {
                                this.list.Add(this.PointD(this.z1Rect, (double) num11, this.zPos));
                            }
                            this.DrawPoints();
                        }
                        for (int num12 = 180; num12 <= num; num12++)
                        {
                            this.list.Add(this.PointD(this.z1Rect, (double) num12, this.zPos));
                        }
                        if (this.hasBevel)
                        {
                            for (int num13 = num; num13 >= 180; num13--)
                            {
                                this.list.Add(this.PointD(this.z1BevelRect, (double) num13, this.zPos));
                            }
                        }
                        else
                        {
                            for (int num14 = num; num14 >= 180; num14--)
                            {
                                this.list.Add(this.PointD(this.z0Rect, (double) num14, this.zPos));
                            }
                        }
                        this.DrawPoints();
                        this.DrawBevel(num2, num);
                    }
                    else if (((this.start0.Y >= this.middle0.Y) && (this.end0.Y > this.middle0.Y)) && (this.sweepAngle > 180))
                    {
                        if (this.hasBevel)
                        {
                            for (int num15 = 180; num15 >= 0; num15--)
                            {
                                this.list.Add(this.PointD(this.z1BevelRect, (double) num15, this.zPos));
                            }
                        }
                        else
                        {
                            for (int num16 = 180; num16 >= 0; num16--)
                            {
                                this.list.Add(this.PointD(this.z0Rect, (double) num16, this.zPos));
                            }
                        }
                        for (int num17 = 0; num17 <= 180; num17++)
                        {
                            this.list.Add(this.PointD(this.z1Rect, (double) num17, this.zPos));
                        }
                        this.DrawPoints();
                        for (int num18 = num; num18 >= 180; num18--)
                        {
                            this.list.Add(this.PointD(this.z1Rect, (double) num18, this.zPos));
                        }
                        if (this.hasBevel)
                        {
                            for (int num19 = 180; num19 <= num; num19++)
                            {
                                this.list.Add(this.PointD(this.z1BevelRect, (double) num19, this.zPos));
                            }
                        }
                        else
                        {
                            for (int num20 = 180; num20 <= num; num20++)
                            {
                                this.list.Add(this.PointD(this.z0Rect, (double) num20, this.zPos));
                            }
                        }
                        this.DrawPoints();
                        if (num2 > 0)
                        {
                            for (int num21 = 360; num21 >= num2; num21--)
                            {
                                this.list.Add(this.PointD(this.z1Rect, (double) num21, this.zPos));
                            }
                            if (this.hasBevel)
                            {
                                for (int num22 = num2; num22 <= 360; num22++)
                                {
                                    this.list.Add(this.PointD(this.z1BevelRect, (double) num22, this.zPos));
                                }
                            }
                            else
                            {
                                for (int num23 = num2; num23 <= 360; num23++)
                                {
                                    this.list.Add(this.PointD(this.z0Rect, (double) num23, this.zPos));
                                }
                            }
                            this.DrawPoints();
                        }
                        this.DrawBevel(num2, num, true);
                    }
                    else
                    {
                        if (num < num2)
                        {
                            num += 360;
                        }
                        if (this.hasBevel)
                        {
                            for (int num24 = num2; num24 <= num; num24++)
                            {
                                this.list.Add(this.PointD(this.z1BevelRect, (double) num24, this.zPos));
                            }
                        }
                        else
                        {
                            for (int num25 = num2; num25 <= num; num25++)
                            {
                                this.list.Add(this.PointD(this.z0Rect, (double) num25, this.zPos));
                            }
                        }
                        for (int num26 = num; num26 >= num2; num26--)
                        {
                            this.list.Add(this.PointD(this.z1Rect, (double) num26, this.zPos));
                        }
                        this.DrawPoints();
                        this.DrawBevel(num2, num);
                    }
                }
            }

            private void DrawLeft()
            {
                if ((this.sweepAngle < 360) && (this.drawEntirePie || this.iDrawSides))
                {
                    this.list.Clear();
                    if (this.isDonut)
                    {
                        if (this.hasBevel)
                        {
                            this.list.Add(this.endD0);
                            this.list.Add(this.endB0);
                            this.list.Add(this.endB1);
                            this.list.Add(this.end1);
                            this.list.Add(this.endD1);
                            this.DrawPoints();
                        }
                        else
                        {
                            this.g.IPointDoubles[0] = this.endD0;
                            this.g.IPointDoubles[1] = this.end0;
                            this.g.IPointDoubles[2] = this.end1;
                            this.g.IPointDoubles[3] = this.endD1;
                            this.g.PolygonFourDouble();
                        }
                    }
                    else if (this.hasBevel)
                    {
                        this.list.Add(this.middle0);
                        this.list.Add(this.endB0);
                        this.list.Add(this.endB1);
                        this.list.Add(this.end1);
                        this.list.Add(this.middle1);
                        this.DrawPoints();
                    }
                    else
                    {
                        this.g.IPointDoubles[0] = this.middle0;
                        this.g.IPointDoubles[1] = this.end0;
                        this.g.IPointDoubles[2] = this.end1;
                        this.g.IPointDoubles[3] = this.middle1;
                        this.g.PolygonFourDouble();
                    }
                }
            }

            private void DrawLighting(EdgeStyles edgeStyle)
            {
                if (this.hasBevel)
                {
                    if (this.gradientBrush == null)
                    {
                        this.gradientBrush = new ChartBrush(this.g.Chart);
                    }
                    bool visible = this.g.Pen.Visible;
                    this.g.Brush = this.gradientBrush;
                    switch (edgeStyle)
                    {
                        case EdgeStyles.Flat:
                            this.DoFlatGradient(this.zPos);
                            break;

                        case EdgeStyles.Curved:
                            this.DoCurvedGradient(this.zPos);
                            break;
                    }
                    this.g.Pen.Visible = visible;
                }
            }

            private void DrawPoints()
            {
                if (this.list.Count > 0)
                {
                    this.points = (PointDouble[]) this.list.ToArray(typeof(PointDouble));
                    this.g.Polygon(this.points);
                    this.list.Clear();
                }
            }

            private void DrawRight()
            {
                if ((this.sweepAngle < 360) && (this.drawEntirePie || this.iDrawSides))
                {
                    this.list.Clear();
                    if (this.isDonut)
                    {
                        if (this.hasBevel)
                        {
                            this.list.Add(this.startD0);
                            this.list.Add(this.startB0);
                            this.list.Add(this.startB1);
                            this.list.Add(this.start1);
                            this.list.Add(this.startD1);
                            this.DrawPoints();
                        }
                        else
                        {
                            this.g.IPointDoubles[0] = this.startD0;
                            this.g.IPointDoubles[1] = this.start0;
                            this.g.IPointDoubles[2] = this.start1;
                            this.g.IPointDoubles[3] = this.startD1;
                            this.g.PolygonFourDouble();
                        }
                    }
                    else if (this.hasBevel)
                    {
                        this.list.Add(this.middle0);
                        this.list.Add(this.startB0);
                        this.list.Add(this.startB1);
                        this.list.Add(this.start1);
                        this.list.Add(this.middle1);
                        this.DrawPoints();
                    }
                    else
                    {
                        this.g.IPointDoubles[0] = this.middle0;
                        this.g.IPointDoubles[1] = this.start0;
                        this.g.IPointDoubles[2] = this.start1;
                        this.g.IPointDoubles[3] = this.middle1;
                        this.g.PolygonFourDouble();
                    }
                }
            }

            private void DrawSides(int startAng, int endAng)
            {
                this.list.Clear();
                if (this.end0.X > this.middle0.X)
                {
                    if (this.start0.X < this.middle0.X)
                    {
                        this.DrawBack(startAng, endAng);
                        this.DrawRight();
                        this.DrawLeft();
                        this.DrawFront(startAng, endAng);
                    }
                    else if (this.sweepAngle > 180)
                    {
                        this.DrawBack(startAng, endAng);
                        this.DrawRight();
                        this.DrawLeft();
                        this.DrawFront(startAng, endAng);
                    }
                    else
                    {
                        this.DrawLeft();
                        this.DrawBack(startAng, endAng);
                        this.DrawFront(startAng, endAng);
                        this.DrawRight();
                    }
                }
                else if (this.start0.X < this.middle0.X)
                {
                    if (this.sweepAngle > 180)
                    {
                        this.DrawBack(startAng, endAng);
                        this.DrawLeft();
                        this.DrawRight();
                        this.DrawFront(startAng, endAng);
                    }
                    else
                    {
                        this.DrawRight();
                        this.DrawBack(startAng, endAng);
                        this.DrawFront(startAng, endAng);
                        this.DrawLeft();
                    }
                }
                else if (this.sweepAngle > 180)
                {
                    this.DrawBack(startAng, endAng);
                    this.DrawLeft();
                    this.DrawFront(startAng, endAng);
                    this.DrawRight();
                }
                else
                {
                    this.DrawBack(startAng, endAng);
                    this.DrawFront(startAng, endAng);
                    this.DrawRight();
                    this.DrawLeft();
                }
            }

            private void DrawTop(int startAng, int endAng)
            {
                this.list.Clear();
                if ((this.sweepAngle < 360) && !this.isDonut)
                {
                    this.list.Add(this.middle0);
                }
                if (this.hasBevel)
                {
                    for (int i = startAng; i <= endAng; i++)
                    {
                        this.list.Add(this.PointD(this.z0BevelRect, (double) i, this.zPos));
                    }
                }
                else
                {
                    for (int j = startAng; j <= endAng; j++)
                    {
                        this.list.Add(this.PointD(this.z0Rect, (double) j, this.zPos));
                    }
                }
                if (this.isDonut)
                {
                    for (int k = endAng; k >= startAng; k--)
                    {
                        this.list.Add(this.PointD(this.z0DonutRect, (double) k, this.zPos));
                    }
                }
                if ((this.sweepAngle < 360) && !this.isDonut)
                {
                    this.list.Add(this.middle0);
                }
                this.DrawPoints();
            }

            public void Pie(int xCenter, int yCenter, int xRadius, int yRadius, int z0, int z1, double startAngle, double endAngle, bool darkSides, bool drawSides, int donutPercent, int bevelPercent, EdgeStyles edgeStyle)
            {
                Point empty = Point.Empty;
                this.isDonut = false;
                this.hasBevel = false;
                empty.X = xCenter;
                empty.Y = yCenter;
                int left = empty.X - xRadius;
                int right = empty.X + xRadius;
                int top = empty.Y - yRadius;
                int bottom = empty.Y + yRadius;
                this.zPos = z1 - z0;
                this.z0Rect = Utils.FromLTRB(left, top, right, bottom);
                this.z1Rect = Utils.FromLTRB(this.z0Rect.Left, this.z0Rect.Top, this.z0Rect.Right, this.z0Rect.Bottom);
                this.z1Rect.Offset(0, this.zPos);
                this.iDrawSides = drawSides;
                this.iStartAngle = Utils.Round((double) ((startAngle * 180.0) / 3.1415926535897931));
                this.iEndAngle = Utils.Round((double) ((endAngle * 180.0) / 3.1415926535897931));
                if (this.iEndAngle < this.iStartAngle)
                {
                    this.iEndAngle += 360;
                }
                if (this.g.Brush.Solid)
                {
                    this.OldColor = this.g.Brush.Color;
                }
                else
                {
                    this.OldColor = this.g.BackColor;
                }
                Color emptyColor = Utils.EmptyColor;
                this.drawEntirePie = (this.g.Brush.Transparency > 0) && this.g.Chart.Aspect.View3D;
                if (donutPercent > 0)
                {
                    int num5 = Utils.Round((double) ((donutPercent * xRadius) * 0.01));
                    int num6 = Utils.Round((double) ((donutPercent * yRadius) * 0.01));
                    left = empty.X - num5;
                    right = empty.X + num5;
                    top = empty.Y - num6;
                    bottom = empty.Y + num6;
                    this.z0DonutRect = Utils.FromLTRB(left, top, right, bottom);
                    this.z1DonutRect = Utils.FromLTRB(this.z0DonutRect.Left, this.z0DonutRect.Top, this.z0DonutRect.Right, this.z0DonutRect.Bottom);
                    this.z1DonutRect.Offset(0, this.zPos);
                    this.startD0 = this.PointD(this.z0DonutRect, (double) this.iStartAngle, this.zPos);
                    this.endD0 = this.PointD(this.z0DonutRect, (double) this.iEndAngle, this.zPos);
                    this.startD1 = this.PointD(this.z1DonutRect, (double) this.iStartAngle, this.zPos);
                    this.endD1 = this.PointD(this.z1DonutRect, (double) this.iEndAngle, this.zPos);
                    this.isDonut = true;
                }
                if (bevelPercent > 0)
                {
                    double num7 = (bevelPercent * this.zPos) * 0.01;
                    left = Utils.Round((double) (empty.X - (xRadius - num7)));
                    right = Utils.Round((double) (empty.X + (xRadius - num7)));
                    top = Utils.Round((double) (empty.Y - (yRadius - num7)));
                    bottom = Utils.Round((double) (empty.Y + (yRadius - num7)));
                    this.z0BevelRect = Utils.FromLTRB(left, top, right, bottom);
                    this.z1BevelRect = Utils.FromLTRB(this.z0Rect.Left, this.z0Rect.Top, this.z0Rect.Right, this.z0Rect.Bottom);
                    this.z1BevelRect.Offset(0, Utils.Round(num7));
                    this.startB0 = this.PointD(this.z0BevelRect, (double) this.iStartAngle, this.zPos);
                    this.endB0 = this.PointD(this.z0BevelRect, (double) this.iEndAngle, this.zPos);
                    this.startB1 = this.PointD(this.z1BevelRect, (double) this.iStartAngle, this.zPos);
                    this.endB1 = this.PointD(this.z1BevelRect, (double) this.iEndAngle, this.zPos);
                    this.hasBevel = true;
                }
                this.g.Calc3DPos(ref this.middle0, (double) empty.X, (double) empty.Y, (double) this.zPos);
                this.g.Calc3DPos(ref this.middle1, (double) empty.X, (double) (empty.Y + this.zPos), (double) this.zPos);
                this.start0 = this.PointD(this.z0Rect, (double) this.iStartAngle, this.zPos);
                this.end0 = this.PointD(this.z0Rect, (double) this.iEndAngle, this.zPos);
                this.start1 = this.PointD(this.z1Rect, (double) this.iStartAngle, this.zPos);
                this.end1 = this.PointD(this.z1Rect, (double) this.iEndAngle, this.zPos);
                this.sweepAngle = this.iEndAngle - this.iStartAngle;
                this.DrawBottom(this.iStartAngle, this.iEndAngle);
                bool flag = this.g.Pen.Color == this.OldColor;
                if (darkSides)
                {
                    this.g.InternalApplyDark(this.OldColor, 0x20);
                    if (flag)
                    {
                        Color c = this.g.Pen.Color;
                        Graphics3D.ApplyDark(ref c, 0x20);
                        this.g.Pen.Color = c;
                    }
                }
                this.DrawSides(this.iStartAngle, this.iEndAngle);
                if (darkSides)
                {
                    if (this.g.Brush.Solid)
                    {
                        this.g.Brush.Color = this.OldColor;
                    }
                    else
                    {
                        this.g.BackColor = this.OldColor;
                    }
                    if (flag)
                    {
                        this.g.Pen.Color = this.OldColor;
                    }
                }
                this.DrawTop(this.iStartAngle, this.iEndAngle);
                this.DrawLighting(edgeStyle);
            }

            public PointDouble PointD(Rectangle rect, double angle, int z)
            {
                return this.g.PointFromCircle(rect, angle, z);
            }
        }
    }
}

