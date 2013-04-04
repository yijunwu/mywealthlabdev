namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(IsoSurface), "SeriesIcons.IsoSurface.bmp")]
    public class IsoSurface : Surface
    {
        private ChartPen bandPen;
        private bool hasImage;
        private int iCalcYPos;
        private Point[] iPoints2D;
        private bool isZ;
        private bool iTransp;
        private Point3D[] P3D;
        private int paletteLength;
        private SurfaceSides sides;
        private int tmpMax;
        private int tmpMin;
        private double tmpValue;
        private bool useY;
        private ValueList v;
        private ValueList xv;
        private double yPosition;
        private ValueList zv;

        public IsoSurface() : this(null)
        {
        }

        public IsoSurface(Chart c) : base(c)
        {
            this.sides = new SurfaceSides(this);
            base.UseColorRange = false;
            base.UsePalette = true;
        }

        private int CalcFirstLevel()
        {
            int num2;
            int[] numArray = new int[] { base.valueIndex0, base.valueIndex1, base.valueIndex2, base.valueIndex3 };
            this.tmpMin = numArray[0];
            double num3 = this.v[this.tmpMin];
            double num4 = num3;
            this.tmpMax = this.tmpMin;
            for (num2 = 1; num2 <= 3; num2++)
            {
                if (this.v[numArray[num2]] < num3)
                {
                    num3 = this.v[numArray[num2]];
                    this.tmpMin = numArray[num2];
                }
                else if (this.v[numArray[num2]] > num4)
                {
                    num4 = this.v[numArray[num2]];
                    this.tmpMax = numArray[num2];
                }
            }
            for (num2 = 0; num2 < this.paletteLength; num2++)
            {
                GridPalette palette = base.Palette[num2];
                if (palette.UpToValue > num3)
                {
                    return num2;
                }
            }
            return -1;
        }

        private void CalcLR(int index, ref int l, ref int r)
        {
            if (index == base.valueIndex0)
            {
                l = base.valueIndex3;
                r = base.valueIndex1;
            }
            else if (index == base.valueIndex1)
            {
                l = base.valueIndex0;
                r = base.valueIndex2;
            }
            else if (index == base.valueIndex2)
            {
                l = base.valueIndex1;
                r = base.valueIndex3;
            }
            else
            {
                l = base.valueIndex2;
                r = base.valueIndex0;
            }
        }

        private int CalcOposite(int index)
        {
            if (index == base.valueIndex3)
            {
                return base.valueIndex1;
            }
            if (index == base.valueIndex1)
            {
                return base.valueIndex3;
            }
            if (index == base.valueIndex0)
            {
                return base.valueIndex2;
            }
            return base.valueIndex0;
        }

        private Point3D CalcPoint(int index)
        {
            Point3D empty = Point3D.Empty;
            empty.X = this.CalcXPos(index);
            if (base.chart.Aspect.View3D)
            {
                if (this.UseYPosition)
                {
                    empty.Y = this.iCalcYPos;
                }
                else
                {
                    empty.Y = this.CalcYPos(index);
                }
                empty.Z = base.CalcZPos(index);
                return empty;
            }
            empty.Y = base.GetVertAxis.CalcYPosValue(base.ZValues[index]);
            return empty;
        }

        private void CalcXZ(int A, int B, ref Point3D P)
        {
            P.X = this.CalcXPos(A);
            if (base.chart.Aspect.View3D)
            {
                P.Z = base.CalcZPos(A);
            }
            else
            {
                P.Y = base.GetVertAxis.CalcYPosValue(base.ZValues[A]);
            }
            double num = this.v[B] - this.v[A];
            if (num == 0.0)
            {
                num = 1.0;
            }
            num = (this.tmpValue - this.v[A]) / num;
            if (this.xv[A] != this.xv[B])
            {
                P.X -= Utils.Round((double) ((P.X - this.CalcXPos(B)) * num));
            }
            if (this.zv[A] != this.zv[B])
            {
                if (base.chart.Aspect.View3D)
                {
                    P.Z -= Utils.Round((double) ((P.Z - base.CalcZPos(B)) * num));
                }
                else
                {
                    P.Y -= Utils.Round((double) ((P.Y - base.GetVertAxis.CalcYPosValue(base.ZValues[B])) * num));
                }
            }
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            AddSubChart(Texts.Normal);
            AddSubChart(Texts.ColorRange);
            AddSubChart(Texts.WireFrame);
            AddSubChart(Texts.DotFrame);
            AddSubChart(Texts.Sides);
            AddSubChart(Texts.NoBorder);
        }

        public override void Draw()
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            bool supportsFullRotation = graphicsd.SupportsFullRotation;
            this.xv = base.XValues;
            this.zv = base.ZValues;
            this.v = base.mandatory;
            this.hasImage = base.Brush.Image != null;
            if (base.UseColorRange)
            {
                this.GeneratePalette();
            }
            this.iTransp = (base.Brush.Transparency > 0) && !graphicsd.SupportsFullRotation;
            this.paletteLength = base.Palette.Count;
            this.iCalcYPos = base.CalcYPosValue(this.YPosition);
            base.Draw();
            this.iPoints2D = null;
        }

        protected internal override void DrawCell(int x, int z)
        {
            Point[] p = new Point[4];
            Graphics3D graphicsd1 = base.chart.Graphics3D;
            if (base.FourGridIndex(x, z) && (this.InternalColor(base.valueIndex0) != Utils.EmptyColor))
            {
                int level = this.CalcFirstLevel();
                if (level != -1)
                {
                    if (this.iTransp && base.Chart.Aspect.View3D)
                    {
                        if (this.UseYPosition)
                        {
                            base.Points[0].Y = this.iCalcYPos;
                            base.Points[1].Y = this.iCalcYPos;
                            base.Points[2].Y = this.iCalcYPos;
                            base.Points[3].Y = this.iCalcYPos;
                        }
                        this.PointsTo2D(base.CalcZPos(base.valueIndex0), base.CalcZPos(base.valueIndex2), ref p);
                    }
                    this.LoopLevels(level);
                }
            }
        }

        private void DrawLine(Point[] P, int z0, int z1)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            if (this.Sides.Pen.Visible)
            {
                graphicsd.Pen = this.Sides.Pen;
                graphicsd.MoveTo(P[1].X, P[1].Y - 1, z0);
                graphicsd.LineTo(P[2].X, P[2].Y - 1, z1);
            }
        }

        private void DrawPenLessPolygon(Point3D[] point3D)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            bool visible = graphicsd.Pen.Visible;
            graphicsd.Pen.Visible = false;
            graphicsd.Polygon(point3D);
            graphicsd.Pen.Visible = visible;
        }

        private void DrawPenLessPolygon(params Point[] point)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            bool visible = graphicsd.Pen.Visible;
            graphicsd.Pen.Visible = false;
            graphicsd.Polygon(point);
            graphicsd.Pen.Visible = visible;
        }

        private void DrawPenLessPolygon3D(params Point3D[] point)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            bool visible = graphicsd.Pen.Visible;
            graphicsd.Pen.Visible = false;
            graphicsd.Polygon(point);
            graphicsd.Pen.Visible = visible;
        }

        protected override void DrawSidePortion(Point[] P, int z0, int z1)
        {
            double num8;
            Graphics3D graphicsd = base.chart.Graphics3D;
            this.P3D = new Point3D[4];
            int y = P[0].Y;
            int num2 = P[3].Y;
            this.isZ = z0 != z1;
            if (num2 == y)
            {
                num8 = 0.0;
            }
            else if (this.isZ)
            {
                num8 = Math.Abs((double) (Convert.ToDouble((int) (z1 - z0)) / Convert.ToDouble((int) (num2 - y))));
                this.P3D[0].X = P[0].X;
                this.P3D[1].X = P[1].X;
                this.P3D[2].X = P[2].X;
                this.P3D[3].X = P[3].X;
                this.P3D[0].Z = z0;
                this.P3D[1].Z = z0;
                this.P3D[2].Z = z1;
                this.P3D[3].Z = z1;
            }
            else
            {
                num8 = Math.Abs((double) (Convert.ToDouble((int) (P[0].X - P[3].X)) / Convert.ToDouble((int) (num2 - y))));
            }
            int a = y;
            int b = num2;
            if (y > num2)
            {
                Utils.SwapInteger(ref a, ref b);
            }
            int num5 = P[1].Y;
            graphicsd.Brush = base.SideBrush;
            graphicsd.Pen = base.SideLines;
            for (int i = 0; i < this.paletteLength; i++)
            {
                int num6 = base.GetVertAxis.CalcYPosValue(base.Palette[i].UpToValue);
                if (num6 < num5)
                {
                    P[1].Y = num5;
                    P[2].Y = num5;
                    if (num6 < b)
                    {
                        if (num6 < a)
                        {
                            num6 = a;
                        }
                        if (num5 > b)
                        {
                            P[0].Y = b;
                            P[3].Y = b;
                            this.Fill(false, i, P, z0, z1);
                            this.DrawLine(P, z0, z1);
                            P[1].Y = b;
                            P[2].Y = b;
                        }
                        P[0].Y = num6;
                        P[3].Y = num6;
                        int num7 = Utils.Round((double) (num8 * Convert.ToDouble((int) (num6 - a))));
                        if (num2 == b)
                        {
                            if (this.isZ)
                            {
                                this.P3D[3].Z = this.P3D[0].Z - num7;
                            }
                            else
                            {
                                P[3].X = P[0].X - num7;
                            }
                            this.Fill(false, i, P, z0, z1);
                            if (this.isZ)
                            {
                                this.P3D[2].Z = this.P3D[3].Z;
                            }
                            else
                            {
                                P[2].X = P[3].X;
                            }
                        }
                        else
                        {
                            if (this.isZ)
                            {
                                this.P3D[0].Z = this.P3D[3].Z + num7;
                            }
                            else
                            {
                                P[0].X = P[3].X + num7;
                            }
                            this.Fill(false, i, P, z0, z1);
                            if (this.isZ)
                            {
                                this.P3D[1].Z = this.P3D[0].Z;
                            }
                            else
                            {
                                P[1].X = P[0].X;
                            }
                        }
                    }
                    else
                    {
                        P[0].Y = num6;
                        P[3].Y = num6;
                        this.Fill(true, i, P, z0, z1);
                    }
                    if (num6 <= a)
                    {
                        return;
                    }
                    num5 = num6;
                    this.DrawLine(P, z0, z1);
                }
            }
        }

        private void Fill(bool outLine, int t, Point[] P, int z0, int z1)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            if (this.Sides.Levels)
            {
                graphicsd.Brush.Color = base.Palette[t].Color;
                if (graphicsd.Brush.Color == Color.Transparent)
                {
                    return;
                }
            }
            if (this.isZ)
            {
                this.P3D[0].Y = P[0].Y;
                this.P3D[1].Y = P[1].Y;
                this.P3D[2].Y = P[2].Y;
                this.P3D[3].Y = P[3].Y;
                if (base.SideLines.Visible)
                {
                    graphicsd.Polygon(this.P3D);
                }
                else
                {
                    this.DrawPenLessPolygon(this.P3D);
                }
            }
            else if (base.SideLines.Visible)
            {
                graphicsd.PlaneFour3D(z0, z1, P);
            }
            else
            {
                bool visible = graphicsd.Pen.Visible;
                graphicsd.Pen.Visible = false;
                graphicsd.PlaneFour3D(z0, z1, P);
                graphicsd.Pen.Visible = visible;
            }
        }

        private void FillPolygon(params Point3D[] points)
        {
            if (base.chart.Aspect.View3D)
            {
                this.DrawPenLessPolygon3D(points);
            }
            else
            {
                int length = points.Length;
                this.iPoints2D = (Point[]) Utils.SetLength(this.iPoints2D, length, typeof(Point));
                for (int i = 0; i < length; i++)
                {
                    this.iPoints2D[i].X = points[i].X;
                    this.iPoints2D[i].Y = points[i].Y;
                }
                this.DrawPenLessPolygon(this.iPoints2D);
            }
        }

        private void GeneratePalette()
        {
            for (int i = 0; i < this.paletteLength; i++)
            {
                GridPalette palette = base.Palette[i];
                palette.Color = base.GetValueColorValue(palette.UpToValue);
                base.Palette[i] = palette;
            }
        }

        private Color InternalColor(int valueIndex)
        {
            return this.ValueColor(valueIndex);
        }

        private void LoopLevels(int level)
        {
            int index = this.CalcOposite(this.tmpMin);
            Point3D empty = Point3D.Empty;
            Point3D p = Point3D.Empty;
            Point3D pointd8 = Point3D.Empty;
            Point3D pointd9 = Point3D.Empty;
            Graphics3D graphicsd = base.chart.Graphics3D;
            Point[] point = new Point[4];
            int l = 0;
            int r = 0;
            Point3D pointd = this.CalcPoint(this.tmpMin);
            Point3D pointd2 = pointd;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            int tmpMin = this.tmpMin;
            int num5 = tmpMin;
            this.CalcLR(this.tmpMin, ref l, ref r);
            Point3D pointd3 = this.CalcPoint(l);
            Point3D pointd4 = this.CalcPoint(r);
            Point3D pointd5 = this.CalcPoint(index);
            while (level < this.paletteLength)
            {
                this.tmpValue = base.Palette[level].UpToValue;
                Color color = base.Brush.Color;
                bool visible = base.Brush.Visible;
                if (base.Palette[level].Color == Utils.EmptyColor)
                {
                    this.PrepareBrush(Color.Transparent);
                }
                else
                {
                    this.PrepareBrush(base.Palette[level].Color);
                }
                if (!flag3 && (this.tmpValue >= this.v[this.tmpMax]))
                {
                    if (base.chart.Aspect.View3D)
                    {
                        point[0] = graphicsd.Calculate3DPosition(pointd.X, pointd.Y, pointd.Z);
                        point[1] = graphicsd.Calculate3DPosition(pointd3.X, pointd3.Y, pointd3.Z);
                        point[2] = graphicsd.Calculate3DPosition(pointd5.X, pointd5.Y, pointd5.Z);
                        point[3] = graphicsd.Calculate3DPosition(pointd4.X, pointd4.Y, pointd4.Z);
                        this.DrawPenLessPolygon(point);
                        return;
                    }
                    Point3D[] points = new Point3D[] { pointd, pointd3, pointd5, pointd4 };
                    this.FillPolygon(points);
                    return;
                }
                if (base.chart.Aspect.View3D)
                {
                    if (this.UseYPosition)
                    {
                        empty.Y = this.iCalcYPos;
                    }
                    else
                    {
                        empty.Y = base.CalcYPosValue(Math.Min(this.v[this.tmpMax], this.tmpValue));
                    }
                    p.Y = empty.Y;
                }
                if (!flag && (this.tmpValue >= this.v[l]))
                {
                    flag = true;
                    tmpMin = index;
                    if (flag2 && (this.tmpValue >= this.v[num5]))
                    {
                        Point3D[] pointdArray2 = new Point3D[] { pointd, pointd2, this.CalcPoint(num5), pointd3 };
                        this.FillPolygon(pointdArray2);
                    }
                    else if (!flag2 && (this.tmpValue >= this.v[r]))
                    {
                        flag2 = true;
                        num5 = index;
                        if (this.tmpValue >= this.v[index])
                        {
                            flag4 = true;
                            Point3D[] pointdArray3 = new Point3D[] { pointd, pointd2, pointd4, pointd5, pointd3 };
                            this.FillPolygon(pointdArray3);
                        }
                        else
                        {
                            this.CalcXZ(tmpMin, l, ref empty);
                            this.CalcXZ(num5, r, ref p);
                            Point3D[] pointdArray4 = new Point3D[] { pointd2, pointd, pointd3, empty, p, pointd4 };
                            this.FillPolygon(pointdArray4);
                        }
                    }
                    else if (this.tmpValue >= this.v[index])
                    {
                        if (flag4)
                        {
                            Point3D[] pointdArray5 = new Point3D[] { pointd, pointd3, pointd8 };
                            this.FillPolygon(pointdArray5);
                        }
                        else
                        {
                            flag4 = true;
                            tmpMin = index;
                            this.CalcXZ(tmpMin, r, ref empty);
                            this.CalcXZ(num5, r, ref p);
                            Point3D[] pointdArray6 = new Point3D[] { pointd, pointd3, pointd5, empty, p };
                            this.FillPolygon(pointdArray6);
                        }
                    }
                    else
                    {
                        this.CalcXZ(l, tmpMin, ref empty);
                        this.CalcXZ(num5, r, ref p);
                        Point3D[] pointdArray7 = new Point3D[] { pointd3, pointd, pointd2, p, empty };
                        this.FillPolygon(pointdArray7);
                    }
                }
                else if (!flag2 && (this.tmpValue >= this.v[r]))
                {
                    flag2 = true;
                    if (flag4)
                    {
                        if (flag5)
                        {
                            pointd = pointd9;
                            Point3D[] pointdArray8 = new Point3D[] { pointd2, pointd4, pointd9 };
                            this.FillPolygon(pointdArray8);
                        }
                        else
                        {
                            Point3D[] pointdArray9 = new Point3D[] { pointd, pointd2, pointd4 };
                            this.FillPolygon(pointdArray9);
                        }
                    }
                    else
                    {
                        num5 = index;
                        if (flag && (this.tmpValue >= this.v[tmpMin]))
                        {
                            Point3D[] pointdArray10 = new Point3D[] { pointd, pointd2, pointd4, this.CalcPoint(tmpMin) };
                            this.FillPolygon(pointdArray10);
                        }
                        else if (!flag && (this.tmpValue >= this.v[l]))
                        {
                            flag = true;
                            tmpMin = index;
                            this.CalcXZ(tmpMin, l, ref empty);
                            this.CalcXZ(num5, r, ref p);
                            Point3D[] pointdArray11 = new Point3D[] { pointd, pointd2, pointd3, empty, p, pointd4 };
                            this.FillPolygon(pointdArray11);
                        }
                        else if (this.tmpValue >= this.v[index])
                        {
                            flag4 = true;
                            this.CalcXZ(l, tmpMin, ref empty);
                            this.CalcXZ(num5, l, ref p);
                            Point3D[] pointdArray12 = new Point3D[] { pointd, pointd2, pointd4, pointd5, p, empty };
                            this.FillPolygon(pointdArray12);
                        }
                        else
                        {
                            this.CalcXZ(l, tmpMin, ref empty);
                            this.CalcXZ(num5, r, ref p);
                            Point3D[] pointdArray13 = new Point3D[] { pointd, pointd2, pointd4, p, empty };
                            this.FillPolygon(pointdArray13);
                        }
                    }
                }
                else if (this.tmpValue >= this.v[index])
                {
                    flag4 = true;
                    if (flag && flag2)
                    {
                        Point3D[] pointdArray14 = new Point3D[] { pointd, pointd2, pointd5 };
                        this.FillPolygon(pointdArray14);
                    }
                    else if (flag2)
                    {
                        num5 = index;
                        this.CalcXZ(l, tmpMin, ref empty);
                        this.CalcXZ(l, num5, ref p);
                        Point3D[] pointdArray15 = new Point3D[] { pointd, pointd2, pointd5, p, empty };
                        this.FillPolygon(pointdArray15);
                    }
                    else if (flag)
                    {
                        tmpMin = index;
                        this.CalcXZ(r, tmpMin, ref empty);
                        this.CalcXZ(r, num5, ref p);
                        Point3D[] pointdArray16 = new Point3D[] { pointd, pointd2, p, empty, pointd5 };
                        this.FillPolygon(pointdArray16);
                    }
                    else
                    {
                        flag5 = true;
                        this.CalcXZ(l, tmpMin, ref empty);
                        this.CalcXZ(r, num5, ref p);
                        pointd8.Y = empty.Y;
                        pointd9.Y = p.Y;
                        this.CalcXZ(l, index, ref pointd8);
                        this.CalcXZ(r, index, ref pointd9);
                        Point3D[] pointdArray17 = new Point3D[] { pointd, empty, pointd8, pointd5, pointd9, p };
                        this.FillPolygon(pointdArray17);
                        pointd2 = pointd8;
                    }
                }
                else
                {
                    this.CalcXZ(l, tmpMin, ref empty);
                    this.CalcXZ(num5, r, ref p);
                    Point3D[] pointdArray18 = new Point3D[] { pointd, pointd2, p, empty };
                    this.FillPolygon(pointdArray18);
                }
                base.Brush.Color = color;
                base.Brush.Visible = visible;
                if ((this.BandPen.Visible || base.WireFrame) || base.DotFrame)
                {
                    if (base.DotFrame)
                    {
                        graphicsd.Pixel(pointd2.X, pointd2.Y, pointd2.Z, base.Palette[level].Color);
                    }
                    else if (base.WireFrame)
                    {
                        Color color2 = this.BandPen.Color;
                        graphicsd.Pen = this.BandPen;
                        graphicsd.Pen.Color = base.Palette[level].Color;
                        graphicsd.MoveTo(pointd);
                        graphicsd.LineTo(pointd2);
                        this.BandPen.Color = color2;
                    }
                    else
                    {
                        graphicsd.Pen = this.BandPen;
                        graphicsd.MoveTo(pointd);
                        graphicsd.LineTo(pointd2);
                    }
                }
                if (this.tmpValue > this.v[this.tmpMax])
                {
                    return;
                }
                pointd = empty;
                pointd2 = p;
                flag3 = true;
                level++;
            }
        }

        private void PointsTo2D(int z0, int z1, ref Point[] P)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            P[0] = graphicsd.Calculate3DPosition(base.Points[0], z0);
            P[1] = graphicsd.Calculate3DPosition(base.Points[1], z0);
            P[2] = graphicsd.Calculate3DPosition(base.Points[2], z1);
            P[3] = graphicsd.Calculate3DPosition(base.Points[3], z1);
        }

        private void PrepareBrush(Color tmpColor)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            graphicsd.Brush = base.Brush;
            if (!base.WireFrame && !base.DotFrame)
            {
                if (base.Brush.Color.A < 0xff)
                {
                    tmpColor = Utils.FromArgb(base.Brush.Color.A, tmpColor);
                }
                graphicsd.Brush.Color = tmpColor;
            }
            else
            {
                graphicsd.Brush.Visible = false;
            }
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.BandPen.Visible = false;
        }

        protected override bool SetSameBrush()
        {
            return true;
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 0:
                    base.UseColorRange = false;
                    base.UsePalette = false;
                    return;

                case 1:
                    base.UseColorRange = true;
                    base.UsePalette = false;
                    return;

                case 2:
                    base.WireFrame = true;
                    this.BandPen.Visible = true;
                    return;

                case 3:
                    base.DotFrame = true;
                    this.BandPen.Visible = true;
                    return;

                case 4:
                    base.SideBrush.Visible = true;
                    return;

                case 5:
                    this.BandPen.Visible = false;
                    return;
            }
        }

        protected override bool ShouldDrawFast()
        {
            return false;
        }

        protected override bool ShouldDrawSides()
        {
            if (!base.ShouldDrawSides())
            {
                return this.Sides.Pen.Visible;
            }
            return true;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen BandPen
        {
            get
            {
                if (this.bandPen == null)
                {
                    this.bandPen = new ChartPen(base.chart, Color.Black, true);
                }
                return this.bandPen;
            }
            set
            {
                this.bandPen = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryIsoSurface;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SurfaceSides Sides
        {
            get
            {
                return this.sides;
            }
            set
            {
                this.sides = value;
            }
        }

        public bool UseYPosition
        {
            get
            {
                return this.useY;
            }
            set
            {
                this.useY = value;
            }
        }

        public double YPosition
        {
            get
            {
                return this.yPosition;
            }
            set
            {
                this.yPosition = value;
            }
        }
    }
}

