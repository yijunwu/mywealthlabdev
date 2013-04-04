namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    public class Graphics3DGdiPlus : Graphics3D
    {
        protected int currentX;
        protected int currentY;
        private double slopeAngle;
        private Bitmap tmpBmp;

        public Graphics3DGdiPlus() : base(null)
        {
        }

        public Graphics3DGdiPlus(Chart c) : base(c)
        {
            base.iCanvasType = CanvasType.GDIplus;
        }

        private applyBounds applyFixedBounds(bool vertical)
        {
            if (((base.Chart.Aspect.Rotation > 340) || vertical) || (base.Chart.Aspect.Chart3DPercent < 0x10))
            {
                return applyBounds.vertFixed;
            }
            if (base.Chart.Aspect.Rotation == 270)
            {
                return applyBounds.horizFixed;
            }
            if (base.Chart.Aspect.Elevation < 0x113)
            {
                return applyBounds.horizFixed;
            }
            if ((base.Chart.Aspect.Rotation > 320) && (base.Chart.Aspect.Elevation < 0x137))
            {
                return applyBounds.vertFixed;
            }
            if ((base.Chart.Aspect.Rotation < 320) && (base.Chart.Aspect.Elevation < 0x11d))
            {
                return applyBounds.horizFixed;
            }
            if ((base.Chart.Aspect.Rotation < 290) && (base.Chart.Aspect.Elevation < 320))
            {
                return applyBounds.horizFixed;
            }
            return applyBounds.variableEllipse;
        }

        public override void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle)
        {
            if (base.Pen.bVisible)
            {
                System.Drawing.Rectangle rect = System.Drawing.Rectangle.FromLTRB(x1, y1, x2, y2);
                base.g.DrawArc(base.Pen.DrawingPen, rect, startAngle, sweepAngle);
            }
        }

        public override void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            if (base.Pen.bVisible)
            {
                float num7;
                int num = x2 - x1;
                int num2 = y2 - y1;
                double num3 = (num * 0.5) + x1;
                double num4 = (num2 * 0.5) + y1;
                float startAngle = 360f - Graphics3D.Rad2Deg(Math.Atan2((y2 - num4) - ((y3 - num4) - (y1 - num4)), x3 - num3));
                float num6 = 360f - Graphics3D.Rad2Deg(Math.Atan2((y2 - num4) - ((y4 - num4) - (y1 - num4)), x4 - num3));
                System.Drawing.Rectangle rect = System.Drawing.Rectangle.FromLTRB(x1, y1, x2, y2);
                if (startAngle < num6)
                {
                    num7 = Math.Abs((float) (num6 - startAngle));
                }
                else
                {
                    num7 = Math.Abs((float) (num6 + (360f - startAngle)));
                }
                base.g.DrawArc(base.Pen.DrawingPen, rect, startAngle, num7);
            }
        }

        [Description("Removes all clipping regions applied to Chart Drawing including Client Clip Regions.")]
        public override void ClearClipRegions()
        {
            base.oldRegion = new Region(new System.Drawing.Rectangle(0, 0, 0, 0));
            base.hasClipRegion = false;
            this.UnClip();
        }

        public override void ClipEllipse(System.Drawing.Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(r);
            this.SetClipRegion(new Region(path));
        }

        public override void ClipPolygon(params Point[] p)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(p);
            this.SetClipRegion(new Region(path));
        }

        public override void ClipRectangle(System.Drawing.Rectangle r)
        {
            this.SetClipRegion(new Region(base.CorrectRectangle(r)));
        }

        private bool CullCheck(GraphicsPath gP, int z0, int z1)
        {
            int x = Utils.Round(gP.GetBounds().Left);
            int num2 = Utils.Round(gP.GetBounds().Left);
            int y = Utils.Round(gP.GetBounds().Top);
            int num4 = Utils.Round(gP.GetBounds().Top);
            base.Calc3DPos(ref x, ref y, z0);
            base.Calc3DPos(ref num2, ref num4, z1);
            if (x > num2)
            {
                return false;
            }
            return true;
        }

        protected override void DoDrawString(int x, int y, string text, ChartBrush aBrush)
        {
            Brush drawingBrush;
            if (aBrush.GradientVisible)
            {
                drawingBrush = aBrush.Gradient.DrawingBrush(base.g.MeasureString(text, base.Font.DrawingFont).ToSize());
            }
            else
            {
                drawingBrush = aBrush.DrawingBrush;
            }
            base.g.DrawString(text, base.Font.DrawingFont, drawingBrush, (float) x, (float) y, base.stringFormat);
        }

        public void Draw(Point[] points, Image image, bool transparent)
        {
            Point[] destPoints = new Point[] { points[0], points[1], points[3] };
            System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(new Point(0, 0), image.Size);
            if (transparent)
            {
                ImageAttributes imageAttr = new ImageAttributes();
                Color pixel = new Bitmap(image).GetPixel(1, 1);
                imageAttr.SetColorKey(pixel, pixel);
                base.g.DrawImage(image, destPoints, srcRect, GraphicsUnit.Pixel, imageAttr);
            }
            else
            {
                base.g.DrawImage(image, destPoints, srcRect, GraphicsUnit.Pixel);
            }
        }

        public override void Draw(System.Drawing.Rectangle r, Image image, bool transparent)
        {
            if (transparent)
            {
                ImageAttributes imageAttr = new ImageAttributes();
                Color pixel = new Bitmap(image).GetPixel(1, 1);
                imageAttr.SetColorKey(pixel, pixel);
                base.g.DrawImage(image, r, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            }
            else
            {
                base.g.DrawImage(image, r);
            }
        }

        public override void Draw(int x, int y, Image image)
        {
            base.g.DrawImageUnscaled(image, x, y);
        }

        public override void DrawBeziers(params Point[] p)
        {
            base.g.DrawBeziers(base.Pen.DrawingPen, p);
        }

        private void DrawBrushImage(Point[] points)
        {
            if (base.IsRectangle(points))
            {
                this.DrawBrushImage(base.PolygonBounds(points));
            }
            else if (base.Brush.ImageTransparent)
            {
                this.Draw(points, base.Brush.Image, true);
            }
            else
            {
                this.Draw(points, base.Brush.Image, false);
            }
        }

        private void DrawBrushImage(PointF[] points)
        {
            Point[] pointArray = new Point[points.Length];
            for (int i = 0; i < pointArray.Length; i++)
            {
                pointArray[i] = Point.Round(points[i]);
            }
        }

        private void DrawBrushImage(System.Drawing.Rectangle rect)
        {
            if ((base.Brush.ImageMode == ImageMode.Normal) || (base.Brush.ImageMode == ImageMode.Center))
            {
                base.Brush.Solid = true;
                base.g.FillRectangle(base.Brush.DrawingBrush, rect);
                base.Brush.Solid = false;
            }
            if (base.Brush.ImageTransparent)
            {
                base.Draw(rect, base.Brush.Image, base.Brush.ImageMode, true);
            }
            else
            {
                base.Draw(rect, base.Brush.Image, base.Brush.ImageMode, false);
            }
        }

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            base.g.DrawPath(pen, path);
        }

        public override void Ellipse(int x1, int y1, int x2, int y2)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            if (base.Brush.visible)
            {
                if (base.Brush.GradientVisible)
                {
                    base.g.FillEllipse(base.Brush.Gradient.DrawingBrush(rect), rect);
                }
                else
                {
                    base.g.FillEllipse(base.Brush.DrawingBrush, rect);
                }
            }
            if (base.Pen.bVisible)
            {
                base.g.DrawEllipse(base.Pen.DrawingPen, rect);
            }
        }

        public override void EraseBackground(int left, int top, int right, int bottom)
        {
            System.Drawing.Rectangle rect = System.Drawing.Rectangle.FromLTRB(left, top, right, bottom);
            if (base.Brush.GradientVisible)
            {
                base.g.FillRectangle(base.Brush.Gradient.DrawingBrush(rect), rect);
            }
            else
            {
                base.g.FillRectangle(base.Brush.DrawingBrush, rect);
            }
        }

        public override void FillRegion(Brush brush, Region region)
        {
            base.g.FillRegion(brush, region);
        }

        protected void Full3DCylinder(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool dark3D, int conePercent)
        {
            Point[] pointArray;
            System.Drawing.Rectangle rectangle;
            PointF tf;
            PointF tf2;
            PointF tf3;
            PointF tf4;
            System.Drawing.Rectangle rectangle3;
            int left = r.Left;
            int x = r.Left;
            int top = r.Top;
            int y = r.Top;
            int num5 = r.Top;
            int num6 = r.Top;
            int right = r.Right;
            int num8 = r.Right;
            int bottom = r.Bottom;
            int num10 = r.Bottom;
            if (!vertical)
            {
                base.Calc3DPos(ref left, ref top, z0);
                base.Calc3DPos(ref x, ref y, z1);
                base.Calc3DPos(ref right, ref num5, z0);
                base.Calc3DPos(ref num8, ref num6, z1);
                this.slopeAngle = (Math.Atan((double) (((num5 - top) * 1f) / ((right - left) * 1f))) * 180.0) / 3.1415926535897931;
                int num11 = r.Top + r.Height;
                int num12 = r.Left;
                base.Calc3DPos(ref num12, ref num11, z0);
                if (num12 == left)
                {
                    num12 = left + 1;
                }
                if (num11 == top)
                {
                    num11 = top + 1;
                }
                if ((x == left) || (x < left))
                {
                    x = left + 1;
                }
                pointArray = new Point[] { new Point(left, top), new Point(x, y), new Point(num12, num11) };
                rectangle = new System.Drawing.Rectangle(left, top, Math.Abs((int) (x - left)), r.Height);
            }
            else
            {
                base.Calc3DPos(ref left, ref bottom, z0);
                base.Calc3DPos(ref x, ref num10, z1);
                int num13 = r.Bottom;
                int num14 = r.Left + r.Width;
                base.Calc3DPos(ref num14, ref num13, z0);
                if (num14 == left)
                {
                    num14 = left + 1;
                }
                if (num13 == bottom)
                {
                    num13 = bottom + 1;
                }
                if (bottom == num10)
                {
                    num10 = bottom + 1;
                }
                pointArray = new Point[] { new Point(left, bottom), new Point(x, num10), new Point(num14, num13) };
                rectangle = new System.Drawing.Rectangle(left, bottom, r.Width, Math.Abs((int) (num10 - bottom)));
            }
            Matrix m = new Matrix(rectangle, pointArray);
            base.g.Transform = m;
            GraphicsPath gPath = new GraphicsPath();
            int width = x - left;
            GraphicsPath path2 = new GraphicsPath();
            if (!vertical)
            {
                path2 = this.PlotCylinderEllipse(false, m, gPath, left, top, width, r.Height, out tf, out tf2);
            }
            else
            {
                this.PlotCylinderEllipse(true, m, gPath, left, bottom, r.Width, Math.Abs((int) (num10 - bottom)), out tf, out tf2);
            }
            Region region = new Region(gPath);
            int num16 = 0;
            int height = r.Height;
            if (!vertical)
            {
                int num18 = 0;
                int num19 = 0;
                if (conePercent < 100)
                {
                    num16 = Convert.ToInt32((double) ((((double) ((r.Height * (100 - conePercent)) * 1f)) / 100.0) / 2.0));
                    Convert.ToInt32((double) ((((double) ((r.Width * (100 - conePercent)) * 1f)) / 100.0) / 2.0));
                    int num20 = ((r.Height - (num16 * 2)) > 0) ? (r.Height - (num16 * 2)) : 1;
                    System.Drawing.Rectangle rectangle2 = new System.Drawing.Rectangle(r.X, r.Y + num16, r.Width, num20);
                    height = rectangle2.Height;
                    num5 = rectangle2.Top;
                    num6 = rectangle2.Top;
                    right = rectangle2.Right;
                    num8 = rectangle2.Right;
                    base.Calc3DPos(ref right, ref num5, z0);
                    base.Calc3DPos(ref num8, ref num6, z1);
                    top = num5;
                    y = num6;
                    num18 = rectangle2.Top + rectangle2.Height;
                    num19 = rectangle2.Right;
                }
                else
                {
                    top = num5;
                    y = num6;
                    num18 = r.Top + r.Height;
                    num19 = r.Right;
                }
                base.Calc3DPos(ref num19, ref num18, z0);
                if (num19 == right)
                {
                    num19 = right + 1;
                }
                if (num18 == top)
                {
                    num18 = top + 1;
                }
                if (num8 == right)
                {
                    num8 = right + 1;
                }
                pointArray = new Point[] { new Point(right, top), new Point(num8, y), new Point(num19, num18) };
                rectangle = new System.Drawing.Rectangle(right, top, Math.Abs((int) (num8 - right)), height);
            }
            else
            {
                left = r.Left;
                x = r.Left;
                top = r.Top;
                y = r.Top;
                base.Calc3DPos(ref left, ref top, z0);
                base.Calc3DPos(ref x, ref y, z1);
                int num21 = r.Top;
                int num22 = r.Left + r.Width;
                base.Calc3DPos(ref num22, ref num21, z0);
                if (num22 == left)
                {
                    num22 = left + 1;
                }
                if (num21 == top)
                {
                    num21 = top + 1;
                }
                if (top == y)
                {
                    y = top + 1;
                }
                pointArray = new Point[] { new Point(left, top), new Point(x, y), new Point(num22, num21) };
                rectangle = new System.Drawing.Rectangle(left, top, r.Width, Math.Abs((int) (y - top)));
            }
            m = new Matrix(rectangle, pointArray);
            base.g.Transform = m;
            GraphicsPath path3 = new GraphicsPath();
            if (!vertical)
            {
                this.PlotCylinderEllipse(false, m, path3, right, top, Math.Abs((int) (num8 - right)), height, out tf3, out tf4);
            }
            else
            {
                this.PlotCylinderEllipse(true, m, path3, left, top, r.Width, Math.Abs((int) (y - top)), out tf3, out tf4);
            }
            Region region2 = new Region(path3);
            PointF[] points = new PointF[] { tf3, tf4, tf2, tf };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(points);
            Region region3 = new Region(path);
            region2.Union(region);
            region2.Union(region3);
            if (vertical)
            {
                rectangle3 = new System.Drawing.Rectangle(Utils.Round(path3.GetBounds().Left), Utils.Round(path3.GetBounds().Top), Utils.Round(Math.Abs((float) (gPath.GetBounds().Right - path3.GetBounds().Left))) + 1, Utils.Round(Math.Abs((float) (gPath.GetBounds().Bottom - path3.GetBounds().Top))) + 1);
            }
            else
            {
                rectangle3 = new System.Drawing.Rectangle(Utils.Round(gPath.GetBounds().Left), Utils.Round(gPath.GetBounds().Top), Utils.Round(Math.Abs((float) (path3.GetBounds().Right - gPath.GetBounds().Left))), Utils.Round(Math.Abs((float) (path3.GetBounds().Bottom - gPath.GetBounds().Top))));
            }
            if (base.Brush.visible)
            {
                if (base.Brush.GradientVisible)
                {
                    float num23 = Math.Abs((float) (tf4.X - tf2.X));
                    float num24 = Math.Abs((float) (tf4.Y - tf2.Y));
                    base.Brush.Gradient.Angle = 180.0 - (Math.Atan2((double) num23, (double) num24) / 0.017453292519943295);
                    base.g.FillRegion(base.Brush.Gradient.DrawingBrush(rectangle3), region2);
                    Color color1 = base.Pen.Color;
                    Color color = base.Brush.Color;
                    base.Brush.Color = Graphics3D.TransparentColor(70, base.Brush.Gradient.StartColor);
                    if (this.CullCheck(path3, z0, z1))
                    {
                        base.g.FillPath(base.Brush.DrawingBrush, path3);
                    }
                    base.Brush.Color = color;
                }
                else
                {
                    base.g.FillRegion(base.Brush.DrawingBrush, region2);
                }
            }
            if (base.Pen.Visible)
            {
                base.g.DrawLine(base.Pen.DrawingPen, tf4, tf2);
                base.g.DrawLine(base.Pen.DrawingPen, tf3, tf);
                if (this.CullCheck(path3, z0, z1))
                {
                    base.g.DrawPath(base.Pen.DrawingPen, path3);
                }
            }
            if (!vertical)
            {
                ArrayList list = new ArrayList();
                if (base.Pen.Visible)
                {
                    foreach (PointF tf5 in path2.PathPoints)
                    {
                        if (tf5.X < ((tf2.X < tf.X) ? tf2.X : tf.X))
                        {
                            list.Add(new PointF(tf5.X, tf5.Y));
                        }
                    }
                    if (list.Count > 0)
                    {
                        PointF[] tfArray2 = (PointF[]) list.ToArray(typeof(PointF));
                        base.g.DrawCurve(base.Pen.DrawingPen, tfArray2);
                    }
                }
            }
        }

        private bool getBoundaryPoint(PathData pData, ref float xPoint, ref float yPoint, ref bool isSet, ref double tmpDiff, int i)
        {
            double num = (Math.Atan((double) (((pData.Points[i].Y - pData.Points[i - 1].Y) * 1f) / ((pData.Points[i].X - pData.Points[i - 1].X) * 1f))) * 180.0) / 3.1415926535897931;
            if (Math.Abs((double) (num - this.slopeAngle)) < Math.Abs(tmpDiff))
            {
                isSet = true;
                xPoint = pData.Points[i].X;
                yPoint = pData.Points[i].Y;
                tmpDiff = Math.Abs((double) (num - this.slopeAngle));
            }
            else
            {
                if (isSet)
                {
                    return true;
                }
                tmpDiff = Math.Abs((double) (num - this.slopeAngle));
            }
            return false;
        }

        private void GetFixedBounds(bool vertical, PathData pData, ref float xPoint, ref float yPoint, bool upper, bool upDown)
        {
            bool flag = false;
            xPoint = pData.Points[0].X;
            yPoint = pData.Points[0].Y;
            for (int i = 0; i < pData.Points.GetUpperBound(0); i++)
            {
                if ((upDown && ((upper && ((vertical && (pData.Points[i].X > xPoint)) || (!vertical && (pData.Points[i].Y > yPoint)))) || (!upper && ((vertical && (pData.Points[i].X < xPoint)) || (!vertical && (pData.Points[i].Y < yPoint)))))) || (!upDown && ((upper && ((!vertical && (pData.Points[i].X > xPoint)) || (vertical && (pData.Points[i].Y > yPoint)))) || (!upper && ((!vertical && (pData.Points[i].X < xPoint)) || (vertical && (pData.Points[i].Y < yPoint)))))))
                {
                    xPoint = pData.Points[i].X;
                    yPoint = pData.Points[i].Y;
                    flag = true;
                }
                else if (flag)
                {
                    return;
                }
            }
        }

        protected void GetInferiorBounds(bool vertical, PathData pData, ref float xPoint, ref float yPoint)
        {
            if (this.applyFixedBounds(vertical) == applyBounds.vertFixed)
            {
                this.GetFixedBounds(vertical, pData, ref xPoint, ref yPoint, false, true);
            }
            else if (this.applyFixedBounds(vertical) == applyBounds.horizFixed)
            {
                this.GetFixedBounds(vertical, pData, ref xPoint, ref yPoint, false, false);
            }
            else
            {
                this.GetVariableBounds(vertical, pData, ref xPoint, ref yPoint, false);
            }
        }

        protected void GetSuperiorBounds(bool vertical, PathData pData, ref float xPoint, ref float yPoint)
        {
            if (this.applyFixedBounds(vertical) == applyBounds.vertFixed)
            {
                this.GetFixedBounds(vertical, pData, ref xPoint, ref yPoint, true, true);
            }
            else if (this.applyFixedBounds(vertical) == applyBounds.horizFixed)
            {
                this.GetFixedBounds(vertical, pData, ref xPoint, ref yPoint, true, false);
            }
            else
            {
                this.GetVariableBounds(vertical, pData, ref xPoint, ref yPoint, true);
            }
        }

        private void GetVariableBounds(bool vertical, PathData pData, ref float xPoint, ref float yPoint, bool upper)
        {
            double tmpDiff = 360.0;
            bool isSet = false;
            xPoint = pData.Points[0].X;
            yPoint = pData.Points[0].Y;
            if (upper)
            {
                for (int i = pData.Points.GetUpperBound(0) - 1; i > 0; i--)
                {
                    if ((i < (pData.Points.GetUpperBound(0) - 2)) && this.getBoundaryPoint(pData, ref xPoint, ref yPoint, ref isSet, ref tmpDiff, i))
                    {
                        return;
                    }
                }
            }
            else
            {
                for (int j = 0; j < pData.Points.GetUpperBound(0); j++)
                {
                    if ((j > 1) && this.getBoundaryPoint(pData, ref xPoint, ref yPoint, ref isSet, ref tmpDiff, j))
                    {
                        return;
                    }
                }
            }
        }

        public override void HorizontalLine(int left, int right, int y)
        {
            this.Line(left, y, right, y);
        }

        protected internal override void InitWindow(System.Drawing.Graphics graphics, Aspect a, System.Drawing.Rectangle r, int MaxDepth)
        {
            base.g = graphics;
            base.InitWindow(graphics, a, r, MaxDepth);
            if (!base.metafiling)
            {
                base.g.SmoothingMode = base.aSmoothingMode;
                base.g.TextRenderingHint = base.aTextRenderingHint;
            }
        }

        public void IniWindow(System.Drawing.Graphics graphics, Aspect a, System.Drawing.Rectangle r, int MaxDepth)
        {
            this.InitWindow(graphics, a, r, MaxDepth);
        }

        protected override void InternalCylinder(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool dark3D, int conePercent)
        {
            if ((conePercent < 100) && vertical)
            {
                base.InternalCylinder(vertical, r, z0, z1, dark3D, conePercent);
            }
            else if ((conePercent < 100) && !vertical)
            {
                if (!vertical && (conePercent > 0))
                {
                    this.Full3DCylinder(vertical, r, z0, z1, dark3D, conePercent);
                }
                else
                {
                    base.InternalCylinder(false, r, z0, z1, dark3D, conePercent);
                }
            }
            else if (base.aspect.Zoom > 9)
            {
                this.Full3DCylinder(vertical, r, z0, z1, dark3D, conePercent);
            }
        }

        protected override void Line(ChartPen p, Point a, Point b)
        {
            base.g.DrawLine(p.DrawingPen, a, b);
            base.MoveTo(b);
        }

        public override void Line(int x0, int y0, int x1, int y1)
        {
            base.g.DrawLine(base.Pen.DrawingPen, x0, y0, x1, y1);
            this.MoveTo(x1, y1);
        }

        public override void LineTo(int x, int y)
        {
            base.g.DrawLine(base.Pen.DrawingPen, this.currentX, this.currentY, x, y);
            this.MoveTo(x, y);
        }

        public override SizeF MeasureString(ChartFont f, string text)
        {
            if (this.tmpBmp == null)
            {
                this.tmpBmp = new Bitmap(1, 1);
            }
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(this.tmpBmp);
            graphics.SmoothingMode = base.SmoothingMode;
            graphics.TextRenderingHint = base.TextRenderingHint;
            return graphics.MeasureString(text, f.DrawingFont);
        }

        public override void MoveTo(int x, int y)
        {
            this.currentX = x;
            this.currentY = y;
        }

        public override void Pie(int x1, int y1, int x2, int y2, double startAngle, double endAngle)
        {
            int num = -Utils.Round(startAngle);
            int num2 = -Utils.Round((double) (endAngle - startAngle));
            System.Drawing.Rectangle rect = System.Drawing.Rectangle.FromLTRB(x1, y1, x2, y2);
            if (base.Brush.visible)
            {
                if (base.Brush.GradientVisible)
                {
                    base.g.FillPie(base.Brush.Gradient.DrawingBrush(rect), rect, (float) num, (float) num2);
                }
                else
                {
                    base.g.FillPie(base.Brush.DrawingBrush, rect, (float) num, (float) num2);
                }
            }
            if (base.Pen.bVisible)
            {
                base.g.DrawPie(base.Pen.DrawingPen, rect, (float) num, (float) num2);
            }
        }

        public override void Pixel(int x, int y, int z, Color color)
        {
            Color color2 = base.Pen.Color;
            base.Pen.Color = color;
            base.Calc3DPos(ref x, ref y, z);
            base.g.DrawLine(base.Pen.DrawingPen, x, y, x + 1, y + 1);
            base.Pen.Color = color2;
        }

        protected GraphicsPath PlotCylinderEllipse(bool vertical, Matrix m, GraphicsPath gPath, int left, int top, int width, int height, out PointF p1, out PointF p2)
        {
            float xPoint = 0f;
            float yPoint = 0f;
            float num3 = 0f;
            float num4 = 0f;
            gPath.AddEllipse(left, top, width, height);
            gPath.Transform(m);
            gPath.Flatten();
            base.g.ResetTransform();
            if (base.Pen.Visible)
            {
                base.g.DrawPath(base.Pen.DrawingPen, gPath);
            }
            this.GetInferiorBounds(vertical, gPath.PathData, ref xPoint, ref yPoint);
            this.GetSuperiorBounds(vertical, gPath.PathData, ref num3, ref num4);
            p1 = new PointF(xPoint, yPoint);
            p2 = new PointF(num3, num4);
            return gPath;
        }

        public override void Polygon(params PointDouble[] p)
        {
            PointF[] points = PointDouble.RoundF(p);
            if (base.Brush.Visible)
            {
                if (base.Brush.GradientVisible)
                {
                    if (base.Brush.Gradient.Style.Visible)
                    {
                        base.g.FillPolygon(base.Brush.Gradient.DrawingBrush(p), points);
                    }
                    else if ((base.Brush.Gradient.CustomTargetPolygon != null) && (base.Brush.Gradient.CustomTargetPolygon.Length > 0))
                    {
                        base.g.FillPolygon(base.Brush.Gradient.DrawingBrush(base.PolygonRect(base.Brush.Gradient.CustomTargetPolygon)), points);
                    }
                    else
                    {
                        base.g.FillPolygon(base.Brush.Gradient.DrawingBrush(base.PolygonRect(points)), points);
                    }
                }
                else if ((base.Brush.Image != null) && (p.Length > 3))
                {
                    this.DrawBrushImage(points);
                }
                else
                {
                    base.g.FillPolygon(base.Brush.DrawingBrush, points);
                }
            }
            if (base.Pen.Visible && (p.Length > 1))
            {
                base.g.DrawPolygon(base.Pen.DrawingPen, points);
            }
        }

        public override void Polygon(params Point[] p)
        {
            if (base.Brush.visible)
            {
                if (base.Brush.GradientVisible)
                {
                    if (base.Brush.Gradient.Style.Visible)
                    {
                        base.g.FillPolygon(base.Brush.Gradient.DrawingBrush(p), p);
                    }
                    else if ((base.Brush.Gradient.CustomTargetPolygon != null) && (base.Brush.Gradient.CustomTargetPolygon.Length > 0))
                    {
                        base.g.FillPolygon(base.Brush.Gradient.DrawingBrush(base.PolygonRect(base.Brush.Gradient.CustomTargetPolygon)), p);
                    }
                    else
                    {
                        base.g.FillPolygon(base.Brush.Gradient.DrawingBrush(base.PolygonRect(p)), p);
                    }
                }
                else if ((base.Brush.Image != null) && (p.Length > 3))
                {
                    this.DrawBrushImage(p);
                }
                else
                {
                    base.g.FillPolygon(base.Brush.DrawingBrush, p);
                }
            }
            if (base.Pen.Visible && (p.Length > 1))
            {
                base.g.DrawPolygon(base.Pen.DrawingPen, p);
            }
        }

        public override void Polyline(params Point[] p)
        {
            if (base.Pen.bVisible && (p.Length > 1))
            {
                base.g.DrawLines(base.Pen.DrawingPen, p);
            }
        }

        public override void PrepareDrawImage()
        {
            base.g.InterpolationMode = InterpolationMode.NearestNeighbor;
            base.g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }

        public override void Rectangle(System.Drawing.Rectangle r)
        {
            System.Drawing.Rectangle rectangle;
            if ((base.SmoothingMode == SmoothingMode.HighQuality) || (base.SmoothingMode == SmoothingMode.AntiAlias))
            {
                if ((r.Width > 1) && (r.Height > 1))
                {
                    rectangle = new System.Drawing.Rectangle(r.Left, r.Top, r.Width - 1, r.Height - 1);
                }
                else
                {
                    rectangle = r;
                }
            }
            else
            {
                rectangle = r;
            }
            if (base.Brush.visible)
            {
                if (base.Brush.GradientVisible)
                {
                    base.g.FillRectangle(base.Brush.Gradient.DrawingBrush(rectangle), rectangle);
                }
                else if (base.Brush.Image != null)
                {
                    this.DrawBrushImage(rectangle);
                }
                else
                {
                    base.g.FillRectangle(base.Brush.DrawingBrush, rectangle);
                }
            }
            if (base.Pen.bVisible)
            {
                r.Width--;
                r.Height--;
                base.g.DrawRectangle(base.Pen.DrawingPen, r);
            }
        }

        public override void RotateLabel(int x, int y, string text, double rotDegree)
        {
            Matrix transform = base.g.Transform;
            transform.RotateAt((float) (360.0 - rotDegree), (PointF) new Point(x, y));
            base.g.MultiplyTransform(transform);
            if (base.Font.ShouldDrawShadow())
            {
                this.DoDrawString(x + base.Font.shadow.Width, y + base.Font.shadow.Height, text, base.Font.shadow.Brush);
            }
            this.DoDrawString(x, y, text, base.Font.Brush);
            base.g.ResetTransform();
        }

        public override void SetClipRegion(Region region)
        {
            if (base.hasClipRegion)
            {
                region.Intersect(base.oldRegion);
            }
            base.g.Clip = region;
            base.oldRegion = region;
            base.hasClipRegion = true;
        }

        public override void ShowImage(System.Drawing.Graphics g)
        {
            base.ShowImage(g);
            g = null;
        }

        protected internal override void TransparentEllipse(int x1, int y1, int x2, int y2)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            SolidBrush brush = new SolidBrush(base.chart.Panel.Color);
            base.g.FillEllipse(brush, rect);
        }

        [Description("Removes any internal clipping regions applied to Chart Drawing.")]
        public override void UnClip()
        {
            base.g.Clip.Dispose();
            base.g.ResetClip();
        }

        public override bool ValidState()
        {
            return (base.g != null);
        }

        public override void VerticalLine(int x, int top, int bottom)
        {
            this.Line(x, top, x, bottom);
        }

        public System.Drawing.Graphics Graphics
        {
            get
            {
                return base.g;
            }
            set
            {
                base.g = value;
            }
        }
    }
}

