namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class Custom : CustomPoint
    {
        protected ChartBrush bAreaBrush;
        protected bool bClickableLine;
        protected bool bDark3D;
        private int BottomPos;
        private bool colorEachLine;
        protected bool drawArea;
        protected bool drawLine;
        private ChartBrush intBrush;
        private bool invertedStairs;
        private bool IsLastValue;
        private int lineHeight;
        private int OldBottomPos;
        private Color OldColor;
        private int OldX;
        private int OldY;
        private ChartPen outLine;
        protected ChartPen pAreaLines;
        private bool stairs;
        private Color tmpColor;
        private double tmpDark3DRatio;

        public Custom() : this(null)
        {
        }

        public Custom(Chart c) : base(c)
        {
            this.bClickableLine = true;
            this.colorEachLine = true;
            this.bDark3D = true;
            this.drawLine = true;
            this.OldColor = Utils.EmptyColor;
            this.intBrush = new ChartBrush(c);
        }

        public override void AssignFormat(Series source)
        {
            base.AssignFormat(source);
            if (source is Custom)
            {
                this.stairs = (source as Custom).Stairs;
                this.bClickableLine = (source as Custom).ClickableLine;
                this.lineHeight = (source as Custom).LineHeight;
                this.outLine = (source as Custom).OutLine.Clone() as ChartPen;
                if (this.bAreaBrush != null)
                {
                    if ((source as Custom).bAreaBrush != null)
                    {
                        this.bAreaBrush = (source as Custom).bAreaBrush.Clone() as ChartBrush;
                    }
                    else
                    {
                        this.bAreaBrush = null;
                    }
                }
            }
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            this.InternalCalcMargin(!base.yMandatory, true, ref LeftMargin, ref RightMargin);
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            this.InternalCalcMargin(base.yMandatory, false, ref TopMargin, ref BottomMargin);
            if (((this.lineHeight > 0) && !this.drawArea) && (base.chart.aspect.view3D && (this.lineHeight > BottomMargin)))
            {
                BottomMargin = this.lineHeight;
            }
        }

        private int CalcYPosLeftRight(double yLimit, int anotherIndex, int valueIndex)
        {
            double num = base.vxValues[anotherIndex];
            double num2 = base.vxValues[valueIndex] - num;
            if (num2 == 0.0)
            {
                return this.CalcYPos(anotherIndex);
            }
            double num3 = base.vyValues[anotherIndex];
            return base.GetVertAxis.CalcYPosValue((1.0 * num3) + (((yLimit - num) * (base.vyValues[valueIndex] - num3)) / num2));
        }

        private bool CheckPointInLine(Point P, int tmpX, int tmpY, int OldXPos, int OldYPos)
        {
            if ((base.chart != null) && base.chart.Aspect.View3D)
            {
                Point[] poly = new Point[] { new Point(tmpX, tmpY), new Point(tmpX + base.chart.seriesWidth3D, tmpY - base.chart.seriesHeight3D), new Point(OldXPos + base.chart.seriesWidth3D, OldYPos - base.chart.seriesHeight3D), new Point(OldXPos, OldYPos) };
                return Graphics3D.PointInPolygon(P, poly);
            }
            if (!this.stairs)
            {
                return Graphics3D.PointInLineTolerance(P, tmpX, tmpY, OldXPos, OldYPos, 3);
            }
            if (this.invertedStairs)
            {
                if (!this.PointInVertLine(P, OldXPos, OldYPos, tmpY))
                {
                    return this.PointInHorizLine(P, OldXPos, tmpY, tmpX);
                }
                return true;
            }
            if (!this.PointInHorizLine(P, OldXPos, OldYPos, tmpX))
            {
                return this.PointInVertLine(P, tmpX, OldYPos, tmpY);
            }
            return true;
        }

        public override int Clicked(int x, int y)
        {
            if (base.chart != null)
            {
                base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
            }
            int num2 = base.Clicked(x, y);
            if (((num2 == -1) && (base.firstVisible > -1)) && (base.lastVisible > -1))
            {
                int firstVisible;
                Point p = new Point(x, y);
                int oldXPos = 0;
                int oldYPos = 0;
                if (this.ClickableLine)
                {
                    firstVisible = Math.Max(0, base.firstVisible - 1);
                }
                else
                {
                    firstVisible = base.firstVisible;
                }
                for (int i = firstVisible; i <= base.lastVisible; i++)
                {
                    int tmpX = this.CalcXPos(i);
                    int tmpY = this.CalcYPos(i);
                    if (base.Pointer.Visible && this.ClickedPointer(i, tmpX, tmpY, x, y))
                    {
                        base.OnClickPointer(i, x, y);
                        return i;
                    }
                    if ((tmpX == x) && (tmpY == y))
                    {
                        return i;
                    }
                    if ((i > firstVisible) && this.bClickableLine)
                    {
                        if (!this.CheckPointInLine(p, tmpX, tmpY, oldXPos, oldYPos))
                        {
                            if (!this.drawArea)
                            {
                                goto Label_016F;
                            }
                            Point[] poly = new Point[] { new Point(oldXPos, oldYPos), new Point(tmpX, tmpY), new Point(tmpX, this.GetOriginPos(i)), new Point(oldXPos, this.GetOriginPos(i - 1)) };
                            if (!Graphics3D.PointInPolygon(p, poly))
                            {
                                goto Label_016F;
                            }
                        }
                        return (i - 1);
                    }
                Label_016F:
                    oldXPos = tmpX;
                    oldYPos = tmpY;
                }
            }
            return num2;
        }

        protected internal override void DoBeforeDrawChart()
        {
            if (((base.iColors == null) || (this.Brush.Color == Color.Transparent)) && (this.Brush.Color != base.Color))
            {
                base.Color = this.Brush.Color;
            }
            base.DoBeforeDrawChart();
        }

        public override void Draw()
        {
            if ((this.outLine != null) && this.outLine.Visible)
            {
                Color color = base.Color;
                base.Color = this.outLine.Color;
                ChartPen pen = base.LinePen.Clone() as ChartPen;
                base.LinePen = this.outLine;
                int width = this.outLine.Width;
                base.LinePen.Width = ((2 * pen.Width) + this.outLine.Width) + 1;
                base.Draw();
                base.LinePen = pen;
                base.Color = color;
                this.outLine.Width = width;
            }
            base.Draw();
        }

        private void DrawArea(Color BrushColor, int x, int y)
        {
            Rectangle rectangle;
            Graphics3D g = base.chart.graphics3D;
            bool visible = false;
            if (base.ColorEach)
            {
                this.intBrush = this.bAreaBrush.Clone() as ChartBrush;
                this.intBrush.Visible = true;
                this.intBrush.Color = BrushColor;
                if (this.intBrush.Gradient.Visible)
                {
                    this.intBrush.Gradient.StartColor = BrushColor;
                    this.intBrush.Gradient.MiddleColor = Utils.EmptyColor;
                    this.intBrush.Gradient.EndColor = Utils.CalcColorBlend(BrushColor, Color.White, 60);
                }
                g.Brush = this.intBrush;
            }
            else
            {
                this.bAreaBrush.Color = BrushColor;
                visible = this.bAreaBrush.Visible;
                this.bAreaBrush.Visible = true;
                g.Brush = this.bAreaBrush;
            }
            if (base.chart.Aspect.View3D && this.IsLastValue)
            {
                Color emptyColor = Utils.EmptyColor;
                if (this.Dark3D)
                {
                    emptyColor = g.Brush.Color;
                    g.Brush.ApplyDark(0x40);
                }
                if (base.yMandatory)
                {
                    g.RectangleZ(x, y, this.BottomPos, base.StartZ, base.EndZ);
                }
                else
                {
                    g.RectangleY(x, y, this.BottomPos, base.StartZ, base.EndZ);
                }
                if (this.Dark3D)
                {
                    g.Brush.Color = emptyColor;
                }
            }
            if (this.stairs)
            {
                int num;
                int bottomPos;
                if (this.invertedStairs)
                {
                    num = base.yMandatory ? y : x;
                    bottomPos = this.BottomPos;
                }
                else
                {
                    num = base.yMandatory ? this.OldY : this.OldX;
                    bottomPos = this.OldBottomPos;
                }
                if (base.yMandatory)
                {
                    if (this.pAreaLines.Visible)
                    {
                        rectangle = Utils.FromLTRB(this.OldX, num, x, bottomPos + 1);
                    }
                    else
                    {
                        rectangle = Utils.FromLTRB(this.OldX, num, x + 2, bottomPos + 1);
                    }
                }
                else if (this.pAreaLines.Visible)
                {
                    rectangle = Utils.FromLTRB(bottomPos, y, num - 1, this.OldY);
                }
                else
                {
                    rectangle = Utils.FromLTRB(bottomPos, y, num - 1, this.OldY + 2);
                }
                if (base.chart.Aspect.View3D)
                {
                    g.Rectangle(rectangle, base.StartZ);
                    if (g.SupportsFullRotation)
                    {
                        g.Rectangle(rectangle, base.EndZ);
                    }
                }
                else
                {
                    g.Rectangle(rectangle);
                }
            }
            else
            {
                Point point;
                Point point2;
                Point point4;
                if (base.yMandatory)
                {
                    if (this.pAreaLines.Visible)
                    {
                        point = new Point(this.OldX, this.OldBottomPos);
                    }
                    else
                    {
                        point = new Point(this.OldX - 1, this.OldBottomPos);
                    }
                    point4 = new Point(x, this.BottomPos);
                }
                else
                {
                    if (this.pAreaLines.Visible)
                    {
                        point = new Point(this.OldBottomPos, this.OldY);
                    }
                    else
                    {
                        point = new Point(this.OldBottomPos, this.OldY + 1);
                    }
                    point4 = new Point(this.BottomPos, y);
                }
                if (this.pAreaLines.Visible)
                {
                    point2 = new Point(this.OldX, this.OldY);
                }
                else if (base.yMandatory)
                {
                    point2 = new Point(this.OldX - 1, this.OldY);
                }
                else
                {
                    point2 = new Point(this.OldX, this.OldY + 1);
                }
                Point point3 = new Point(x, y);
                if (base.chart.Aspect.View3D)
                {
                    int endZ;
                    if ((base.chart.Aspect.Rotation > 90) && (base.chart.Aspect.Rotation < 270))
                    {
                        endZ = base.endZ;
                    }
                    else
                    {
                        endZ = base.startZ;
                    }
                    g.Plane(point, point2, point3, point4, endZ);
                }
                else if ((this.Brush != null) && this.Brush.GradientVisible)
                {
                    Point[] p = new Point[] { point, point2, point3, point4 };
                    g.ClipPolygon(p);
                    int num4 = base.CalcPosValue(base.mandatory.Maximum);
                    int height = base.CalcPosValue(base.mandatory.Minimum);
                    if (base.yMandatory)
                    {
                        rectangle = new Rectangle(this.OldX, num4, x, height);
                    }
                    else
                    {
                        rectangle = new Rectangle(height, this.OldY, num4, y);
                    }
                    this.Brush.Gradient.Draw(g, rectangle);
                    g.UnClip();
                    this.Brush.Visible = false;
                    if (this.pAreaLines.bVisible)
                    {
                        if (base.yMandatory)
                        {
                            g.VerticalLine(this.OldX, this.OldY, this.OldBottomPos);
                        }
                        else
                        {
                            g.HorizontalLine(this.OldBottomPos, this.OldX, this.OldY);
                        }
                    }
                }
                else
                {
                    g.IPoints[0] = point;
                    g.IPoints[1] = point2;
                    g.IPoints[2] = point3;
                    g.IPoints[3] = point4;
                    g.PolygonFour();
                }
                if (g.SupportsFullRotation)
                {
                    g.Plane(point, point2, point3, point4, base.EndZ);
                }
                if (base.LinePen.Visible)
                {
                    Color color = base.LinePen.Color;
                    g.Pen = base.LinePen;
                    if (base.ColorEach)
                    {
                        g.Pen.Color = Utils.DarkenColor(BrushColor, 60);
                    }
                    Point[] pointArray = new Point[] { new Point(this.OldX, this.OldY), new Point(x, y) };
                    g.Polyline(base.StartZ, pointArray);
                    base.LinePen.Color = color;
                }
            }
            if (!base.ColorEach)
            {
                this.bAreaBrush.Visible = visible;
            }
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            Color tmpColor = (valueIndex == -1) ? base.Color : this.LegendItemColor(valueIndex);
            bool visible = false;
            if (base.Pointer.Visible)
            {
                if (this.drawLine)
                {
                    this.DrawLine(g, false, tmpColor, rect);
                }
                if (((base.point.Color != base.Color) && (base.point.Color != tmpColor)) && !base.ColorEach)
                {
                    tmpColor = base.point.Color;
                }
                PointerStyles style = base.point.Style;
                base.OnGetPointerStyle(valueIndex, ref style, ref tmpColor);
                base.point.Style = style;
                base.point.DrawLegendShape(g, tmpColor, rect, base.LinePen.bVisible);
            }
            else
            {
                if (base.ColorEach)
                {
                    g.Pen.Color = Utils.DarkenColor(tmpColor, 60);
                }
                if (this.drawLine && !this.drawArea)
                {
                    this.DrawLine(g, base.chart.aspect.view3D, tmpColor, rect);
                }
                else
                {
                    if (this.drawArea)
                    {
                        visible = g.Brush.Visible;
                        g.Brush.Visible = true;
                    }
                    base.DrawLegendShape(g, valueIndex, rect);
                    if (this.drawArea)
                    {
                        g.Brush.Visible = visible;
                    }
                }
            }
        }

        private void DrawLine(Graphics3D g, bool DrawRectangle, Color tmpColor, Rectangle Rect)
        {
            Color emptyColor = Utils.EmptyColor;
            Color color = Utils.EmptyColor;
            if (base.Chart.Legend.Symbol.DefaultPen)
            {
                emptyColor = this.Brush.Color;
                color = base.LinePen.Color;
                this.LinePrepareCanvas(g, (((base.ColorEach && !base.point.Visible) || (this.colorEachLine && base.point.Visible)) || !base.point.Visible) ? tmpColor : base.Color);
            }
            if (DrawRectangle)
            {
                g.Brush.ImageMode = ImageMode.Stretch;
                g.Rectangle(Rect);
            }
            else if (base.LinePen.Visible)
            {
                g.HorizontalLine(Rect.X, Rect.Right, Convert.ToInt32((double) (((double) (Rect.Y + Rect.Bottom)) / 2.0)));
            }
            if (base.Chart.Legend.Symbol.DefaultPen)
            {
                this.Brush.Color = emptyColor;
                base.LinePen.Color = color;
            }
        }

        private void DrawPoint(bool drawOldPointer, int valueIndex, int x, int y)
        {
            bool flag2 = false;
            Graphics3D g = base.chart.graphics3D;
            if (g.CanvasType == CanvasType.HotSpot)
            {
                flag2 = true;
            }
            if (((x == this.OldX) && (y == this.OldY)) || ((base.TreatNulls != TreatNullsStyle.Ignore) && base.IsNull(valueIndex)))
            {
                if (((x != this.OldX) || (y != this.OldY)) && (base.IsNull(valueIndex) && base.point.Visible))
                {
                    if (this.OldColor.ToArgb() != Color.Transparent.ToArgb())
                    {
                        base.DrawPointer(this.OldX, this.OldY, this.OldColor, valueIndex - 1);
                    }
                }
                else if (((x == this.OldX) && (y == this.OldY)) && (base.point.Visible && this.IsLastValue))
                {
                    if (base.IsNull(valueIndex))
                    {
                        base.DrawPointer(this.OldX, this.OldY, this.OldColor, valueIndex - 1);
                    }
                    else
                    {
                        base.DrawPointer(x, y, this.tmpColor, valueIndex);
                    }
                }
            }
            else
            {
                Color color2;
                Color color;
                if (base.chart.aspect.view3D && (this.drawArea || this.drawLine))
                {
                    Color areaBrushColor;
                    g.Pen = base.LinePen;
                    g.Brush = this.Brush;
                    if (this.colorEachLine || this.drawArea)
                    {
                        areaBrushColor = this.GetAreaBrushColor(this.tmpColor);
                    }
                    else
                    {
                        areaBrushColor = base.Color;
                    }
                    color = base.LinePen.Color;
                    color2 = this.Brush.Color;
                    this.Brush.Color = areaBrushColor;
                    Point point = new Point(x, y);
                    Point point2 = new Point(this.OldX, this.OldY);
                    if (this.stairs)
                    {
                        if (this.invertedStairs)
                        {
                            if (this.bDark3D)
                            {
                                g.Brush.ApplyDark(0x40);
                            }
                            if (base.yMandatory)
                            {
                                g.RectangleZ(point2.X, point2.Y, y, base.StartZ, base.EndZ);
                            }
                            else
                            {
                                g.RectangleY(point.X, y, this.OldX, base.StartZ, base.EndZ);
                            }
                            if (this.bDark3D)
                            {
                                g.Brush.Color = areaBrushColor;
                            }
                            if (base.yMandatory)
                            {
                                g.RectangleY(point.X, point.Y, this.OldX, base.StartZ, base.EndZ);
                            }
                            else
                            {
                                g.RectangleZ(x, point2.Y, y, base.StartZ, base.EndZ);
                            }
                        }
                        else
                        {
                            if (base.yMandatory)
                            {
                                g.RectangleY(point2.X, point2.Y, x, base.StartZ, base.EndZ);
                            }
                            else
                            {
                                g.RectangleZ(this.OldX, point2.Y, y, base.StartZ, base.EndZ);
                            }
                            if (this.bDark3D)
                            {
                                g.Brush.ApplyDark(0x40);
                            }
                            if (base.yMandatory)
                            {
                                g.RectangleZ(point.X, point.Y, this.OldY, base.StartZ, base.EndZ);
                            }
                            else
                            {
                                g.RectangleY(this.OldX, y, x, base.StartZ, base.EndZ);
                            }
                            if (this.bDark3D)
                            {
                                g.Brush.Color = areaBrushColor;
                            }
                        }
                    }
                    else
                    {
                        bool flag3 = false;
                        if ((this.lineHeight > 0) && !this.drawArea)
                        {
                            Point3D pointd = new Point3D(point, base.StartZ);
                            Point3D pointd2 = new Point3D(point, base.EndZ);
                            Point3D pointd3 = new Point3D(point.X, point.Y + this.lineHeight, base.StartZ);
                            Point3D pointd4 = new Point3D(point.X, point.Y + this.lineHeight, base.EndZ);
                            Point3D pointd5 = new Point3D(point2, base.StartZ);
                            Point3D pointd6 = new Point3D(point2, base.EndZ);
                            Point3D pointd7 = new Point3D(point2.X, point2.Y + this.lineHeight, base.StartZ);
                            Point3D pointd8 = new Point3D(point2.X, point2.Y + this.lineHeight, base.EndZ);
                            Point3D[] p = new Point3D[] { pointd2, pointd4, pointd8, pointd6 };
                            Point3D[] pointdArray2 = new Point3D[] { pointd7, pointd8, pointd4, pointd3 };
                            Point3D[] pointdArray3 = new Point3D[] { pointd5, pointd, pointd3, pointd7 };
                            Point3D[] pointdArray4 = new Point3D[] { pointd5, pointd, pointd2, pointd6 };
                            g.Polygon(pointdArray2);
                            g.Polygon(p);
                            g.Polygon(pointdArray4);
                            g.Polygon(pointdArray3);
                            if (this.IsLastValue)
                            {
                                g.RectangleZ(point.X, point.Y, point.Y + this.lineHeight, base.StartZ, base.EndZ);
                            }
                            flag3 = true;
                        }
                        bool flag = this.bDark3D && !g.SupportsFullRotation;
                        if (flag)
                        {
                            int num = point.X - point2.X;
                            if (((num != 0) && (this.tmpDark3DRatio != 0.0)) && (((point2.Y - point.Y) / num) > this.tmpDark3DRatio))
                            {
                                g.Brush.ApplyDark(0x40);
                                if (((this.lineHeight > 0) && !this.drawArea) && (Math.Abs((int) (point.Y - point2.Y)) > (this.lineHeight + 1)))
                                {
                                    point.Y += this.lineHeight;
                                    point2.Y += this.lineHeight;
                                }
                            }
                        }
                        if (g.Monochrome)
                        {
                            g.Brush.Color = Color.White;
                        }
                        if (!flag3)
                        {
                            g.Plane(point, point2, base.StartZ, base.EndZ);
                        }
                        if (flag)
                        {
                            g.Brush.Color = areaBrushColor;
                        }
                    }
                    this.Brush.Color = color2;
                    base.LinePen.Color = color;
                }
                if (this.drawArea)
                {
                    Color aColor = this.GetAreaBrushColor(this.tmpColor);
                    color = this.pAreaLines.Color;
                    if (this.pAreaLines.Visible)
                    {
                        if (base.ColorEach)
                        {
                            this.pAreaLines.Color = Utils.DarkenColor(aColor, 60);
                        }
                    }
                    else
                    {
                        this.pAreaLines.Color = aColor;
                    }
                    g.Pen = this.pAreaLines;
                    if (aColor != Color.Transparent)
                    {
                        this.DrawArea(aColor, x, y);
                    }
                    this.pAreaLines.Color = color;
                }
                else if (((!base.chart.Aspect.View3D && this.drawLine) && base.LinePen.Visible) && ((g.CanvasType != CanvasType.HotSpot) || ((g.CanvasType == CanvasType.HotSpot) && this.ClickableLine)))
                {
                    color2 = this.Brush.Color;
                    color = base.LinePen.Color;
                    this.LinePrepareCanvas(g, (((base.ColorEach && !base.point.Visible) || (this.colorEachLine && base.point.Visible)) || !base.point.Visible) ? this.tmpColor : base.Color);
                    if (this.stairs)
                    {
                        if (this.invertedStairs)
                        {
                            g.VerticalLine(this.OldX, this.OldY, y);
                        }
                        else
                        {
                            g.HorizontalLine(this.OldX, x, this.OldY);
                        }
                        g.LineTo(x, y);
                    }
                    else
                    {
                        g.Line(this.OldX, this.OldY, x, y);
                    }
                    this.Brush.Color = color2;
                    base.LinePen.Color = color;
                }
                if (base.point.Visible && drawOldPointer)
                {
                    if (flag2)
                    {
                        base.DrawPointer(x, y, this.tmpColor, valueIndex);
                    }
                    else
                    {
                        if (valueIndex > 0)
                        {
                            if (!base.IsNull(valueIndex - 1))
                            {
                                base.DrawPointer(this.OldX, this.OldY, this.OldColor, valueIndex - 1);
                            }
                            else
                            {
                                int index = valueIndex - 1;
                                while (index > -1)
                                {
                                    if (!base.IsNull(index))
                                    {
                                        break;
                                    }
                                    index--;
                                }
                                if (index != (valueIndex - 1))
                                {
                                    Color color5 = this.ValueColor(index);
                                    base.DrawPointer(this.OldX, this.OldY, color5, index);
                                }
                            }
                        }
                        if (this.IsLastValue && !base.IsNull(valueIndex))
                        {
                            if (!this.DrawValuesForward())
                            {
                                base.DrawPointer(this.OldX, this.OldY, this.OldColor, valueIndex + 1);
                            }
                            base.DrawPointer(x, y, this.tmpColor, valueIndex);
                        }
                    }
                }
            }
        }

        public override void DrawValue(int valueIndex)
        {
            bool flag = false;
            Graphics3D graphicsd = base.chart.graphics3D;
            this.tmpColor = this.ValueColor(valueIndex);
            if (base.IsNull(valueIndex) && (base.TreatNulls == TreatNullsStyle.Ignore))
            {
                this.tmpColor = base.Color;
            }
            int x = this.CalcXPos(valueIndex);
            int y = this.CalcYPos(valueIndex);
            if (base.IsNull(valueIndex) && (base.TreatNulls == TreatNullsStyle.Skip))
            {
                x = this.OldX;
                y = this.OldY;
            }
            if (base.IsNull(valueIndex - 1) && (base.TreatNulls == TreatNullsStyle.DoNotPaint))
            {
                this.OldX = x;
                this.OldY = y;
            }
            this.BottomPos = this.GetOriginPos(valueIndex);
            int num = base.FirstDisplayedIndex();
            if (graphicsd.CanvasType == CanvasType.HotSpot)
            {
                flag = true;
            }
            if (this.DrawValuesForward())
            {
                this.IsLastValue = valueIndex == base.LastVisibleIndex;
            }
            else
            {
                this.IsLastValue = valueIndex == base.FirstVisibleIndex;
            }
            if (valueIndex == num)
            {
                if (this.bDark3D)
                {
                    if (base.chart.seriesWidth3D != 0)
                    {
                        this.tmpDark3DRatio = Math.Abs((int) (base.chart.seriesHeight3D / base.chart.seriesWidth3D));
                    }
                    else
                    {
                        this.tmpDark3DRatio = 1.0;
                    }
                }
                if ((num == base.FirstVisibleIndex) && (valueIndex > 0))
                {
                    if (this.drawArea)
                    {
                        this.OldX = this.CalcXPos(valueIndex - 1);
                        this.OldY = this.CalcYPos(valueIndex - 1);
                        this.OldBottomPos = this.GetOriginPos(valueIndex - 1);
                    }
                    else
                    {
                        Rectangle chartRect = base.chart.ChartRect;
                        this.OldX = base.GetHorizAxis.Inverted ? chartRect.Right : chartRect.Left;
                        if (this.stairs)
                        {
                            this.OldY = this.CalcYPos(valueIndex - 1);
                        }
                        else
                        {
                            this.OldY = this.CalcYPosLeftRight(base.XScreenToValue(this.OldX), valueIndex - 1, valueIndex);
                        }
                    }
                    if (!base.IsNull(valueIndex - 1))
                    {
                        this.DrawPoint(false, valueIndex, x, y);
                    }
                }
                if (flag && !base.IsNull(valueIndex))
                {
                    base.DrawPointer(x, y, this.tmpColor, valueIndex);
                }
                if ((this.IsLastValue && base.point.Visible) && !base.IsNull(valueIndex))
                {
                    base.DrawPointer(x, y, this.tmpColor, valueIndex);
                }
                if ((graphicsd.SupportsFullRotation && this.drawArea) && base.chart.aspect.view3D)
                {
                    graphicsd.RectangleZ(x, y, this.BottomPos, base.StartZ, base.EndZ);
                }
            }
            else if (flag)
            {
                if (!base.IsNull(valueIndex + 1) && !base.IsNull(valueIndex))
                {
                    this.DrawPoint(true, valueIndex, x, y);
                }
                else if (base.IsNull(valueIndex + 1))
                {
                    base.DrawPointer(x, y, this.tmpColor, valueIndex + 1);
                }
            }
            else
            {
                this.DrawPoint(true, valueIndex, x, y);
            }
            this.OldX = x;
            this.OldY = y;
            this.OldBottomPos = this.BottomPos;
            this.OldColor = this.tmpColor;
        }

        protected Color GetAreaBrushColor(Color c)
        {
            Color color = c;
            if (((this.bAreaBrush != null) && this.bAreaBrush.Visible) && !Utils.ColorIsEmpty(this.bAreaBrush.Color))
            {
                color = this.bAreaBrush.Color;
            }
            return color;
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        private void InternalCalcMargin(bool sameSide, bool horizontal, ref int a, ref int b)
        {
            if (horizontal)
            {
                base.Pointer.CalcHorizMargins(ref a, ref b);
            }
            else
            {
                base.Pointer.CalcVerticalMargins(ref a, ref b);
            }
            if (this.drawLine)
            {
                if (this.stairs)
                {
                    a = Math.Max(a, base.LinePen.Width);
                    b = Math.Max(b, base.LinePen.Width + 1);
                }
                if ((this.outLine != null) && this.outLine.Visible)
                {
                    a = Math.Max(a, this.outLine.Width);
                    b = Math.Max(b, this.outLine.Width);
                }
            }
            if (base.marks.visible && sameSide)
            {
                if (base.yMandatory)
                {
                    a = Math.Max(a, base.marks.ArrowLength);
                }
                else
                {
                    b = Math.Max(b, base.marks.ArrowLength);
                }
            }
            if (base.marks.visible && sameSide)
            {
                int num = base.marks.Callout.Length + base.marks.Callout.Distance;
                if (base.yMandatory)
                {
                    a = Math.Max(b, num);
                }
                else
                {
                    b = Math.Max(a, num);
                }
            }
        }

        private void LinePrepareCanvas(Graphics3D g, Color tmpColor)
        {
            if (g.Monochrome)
            {
                tmpColor = Color.White;
            }
            g.Brush = this.Brush;
            g.Pen = base.LinePen;
            if (base.chart.Aspect.View3D)
            {
                g.Brush.Color = tmpColor;
            }
            else
            {
                g.Pen.Color = tmpColor;
            }
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            if (this.bAreaBrush != null)
            {
                this.bAreaBrush.Color = color;
            }
            base.LinePen.Color = Utils.DarkenColor(color, 50);
        }

        private bool PointInHorizLine(Point P, int x0, int y0, int x1)
        {
            return Graphics3D.PointInLineTolerance(P, x0, y0, x1, y0, 3);
        }

        private bool PointInVertLine(Point P, int x0, int y0, int y1)
        {
            return Graphics3D.PointInLineTolerance(P, x0, y0, x0, y1, 3);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.bAreaBrush != null)
            {
                this.bAreaBrush.Chart = c;
            }
            if (this.pAreaLines != null)
            {
                this.pAreaLines.Chart = c;
            }
            if (this.outLine != null)
            {
                this.outLine.Chart = c;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets Brush characteristics.")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [DefaultValue(true), Description("Allows mouse clicks over the line drawn between points.")]
        public bool ClickableLine
        {
            get
            {
                return this.bClickableLine;
            }
            set
            {
                base.SetBooleanProperty(ref this.bClickableLine, value);
            }
        }

        [Description("Enables/Disables the coloring of each connecting line of a series."), DefaultValue(true), Category("Appearance")]
        public bool ColorEachLine
        {
            get
            {
                return this.colorEachLine;
            }
            set
            {
                base.SetBooleanProperty(ref this.colorEachLine, value);
            }
        }

        [DefaultValue(true), Description("Darkens parts of 3D Line Series to add depth."), Category("Appearance")]
        public bool Dark3D
        {
            get
            {
                return this.bDark3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.bDark3D, value);
            }
        }

        [DefaultValue(false), Description("Changes the direction of the step, when true.")]
        public bool InvertedStairs
        {
            get
            {
                return this.invertedStairs;
            }
            set
            {
                base.SetBooleanProperty(ref this.invertedStairs, value);
            }
        }

        [DefaultValue(0), Description("Defines the Height of the line in pixels.")]
        public int LineHeight
        {
            get
            {
                return this.lineHeight;
            }
            set
            {
                base.SetIntegerProperty(ref this.lineHeight, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Sets Opacity level from 0 to 100%")]
        public int Opacity
        {
            get
            {
                return (100 - this.Transparency);
            }
            set
            {
                this.Transparency = 100 - value;
            }
        }

        [Description("Pen for Series Line's outer pen."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DefaultValue((string) null)]
        public ChartPen OutLine
        {
            get
            {
                if (this.outLine == null)
                {
                    this.outLine = new ChartPen(base.chart, Color.Black, false, LineCap.Round);
                }
                return this.outLine;
            }
        }

        [DefaultValue(false), Description("Steps line joining adjacent points.")]
        public bool Stairs
        {
            get
            {
                return this.stairs;
            }
            set
            {
                base.SetBooleanProperty(ref this.stairs, value);
            }
        }

        [Description("Sets Transparency level from 0 to 100%"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0), Category("Appearance")]
        public int Transparency
        {
            get
            {
                return base.bBrush.Transparency;
            }
            set
            {
                base.bBrush.Transparency = value;
            }
        }
    }
}

