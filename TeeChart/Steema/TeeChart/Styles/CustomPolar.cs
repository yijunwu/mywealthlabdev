namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class CustomPolar : Circular
    {
        private bool circleLabels;
        private ChartFont circleLabelsFont;
        private bool circleLabelsInside;
        private bool circleLabelsRot;
        private ChartPen circlePen;
        private bool clockWiseLabels;
        private bool closeCircle;
        private ChartFont font;
        protected int IMaxValuesCount;
        protected SeriesPointer iPointer;
        private int labelsMargin;
        private int OldX;
        private int OldY;
        private ChartPen pen;
        private TreatNullsStyle treatNulls;

        public event GetPointerStyleEventHandler GetPointerStyle;

        public CustomPolar() : this(null)
        {
        }

        public CustomPolar(Chart c) : base(c)
        {
            this.closeCircle = true;
            this.treatNulls = TreatNullsStyle.Ignore;
            this.circleLabelsFont = new ChartFont(base.chart);
            this.circlePen = new ChartPen(base.chart, Color.Black);
            this.labelsMargin = 3;
        }

        public override void AssignFormat(Series source)
        {
            base.AssignFormat(source);
            if (source is CustomPolar)
            {
                this.circleLabels = (source as CustomPolar).CircleLabels;
                this.circleLabelsInside = (source as CustomPolar).CircleLabelsInside;
                this.circleLabelsRot = (source as CustomPolar).CircleLabelsRotated;
                this.labelsMargin = (source as CustomPolar).LabelsMargin;
                this.clockWiseLabels = (source as CustomPolar).ClockWiseLabels;
                this.closeCircle = (source as CustomPolar).CloseCircle;
                this.treatNulls = (source as CustomPolar).TreatNulls;
                this.circleLabelsFont = (source as CustomPolar).CircleLabelsFont.Clone() as ChartFont;
                this.circlePen = (source as CustomPolar).CirclePen.Clone() as ChartPen;
                this.iPointer = (source as CustomPolar).Pointer.Clone() as SeriesPointer;
                this.pen = (source as CustomPolar).Pen.Clone() as ChartPen;
                this.font = (source as CustomPolar).Font.Clone() as ChartFont;
            }
        }

        private Color CalcValueColor(int valueIndex)
        {
            Color color = this.ValueColor(valueIndex);
            if (base.IsNull(valueIndex) && (this.treatNulls == TreatNullsStyle.Ignore))
            {
                color = base.ColorEach ? base.GetDefaultColor(valueIndex) : base.Color;
            }
            if (this.Transparency > 0)
            {
                color = Graphics3D.TransparentColor(this.Transparency, color);
            }
            return color;
        }

        public override int CalcXPos(int valueIndex)
        {
            int num;
            int num2;
            this.CalcXYPos(valueIndex, (double) base.XRadius, out num2, out num);
            return num2;
        }

        private void CalcXYPos(int valueIndex, double aRadius, out int x, out int y)
        {
            this.CalcXYPos(this.GetXValue(valueIndex), base.vyValues[valueIndex], aRadius, out x, out y);
        }

        protected internal void CalcXYPos(double xvalue, double yvalue, double aRadius, out int x, out int y)
        {
            double num = base.GetVertAxis.Maximum - base.GetVertAxis.Minimum;
            double num2 = yvalue - base.GetVertAxis.Minimum;
            if ((num == 0.0) || (num2 < 0.0))
            {
                x = base.CircleXCenter;
                y = base.CircleYCenter;
            }
            else
            {
                double aXRadius = (num2 * aRadius) / num;
                this.AngleToPos(Utils.PiStep * xvalue, aXRadius, aXRadius, out x, out y);
            }
        }

        public override int CalcYPos(int valueIndex)
        {
            int num;
            int num2;
            this.CalcXYPos(valueIndex, (double) base.YRadius, out num, out num2);
            return num2;
        }

        public override int Clicked(int x, int y)
        {
            Point old = new Point(0, 0);
            Point p = new Point(0, 0);
            Point tmp = new Point(0, 0);
            Point tmpCenter = new Point(0, 0);
            int lastVisibleIndex = base.Clicked(x, y);
            if (((lastVisibleIndex == -1) && (base.FirstVisibleIndex > -1)) && (base.LastVisibleIndex > 1))
            {
                if (base.chart != null)
                {
                    base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
                }
                old.X = 0;
                old.Y = 0;
                p.X = x;
                p.Y = y;
                tmpCenter.X = base.CircleXCenter;
                tmpCenter.Y = base.CircleYCenter;
                for (int i = base.FirstVisibleIndex; i <= base.LastVisibleIndex; i++)
                {
                    tmp.X = this.CalcXPos(i);
                    tmp.Y = this.CalcYPos(i);
                    PointerStyles style = this.Pointer.Style;
                    this.OnGetPointerStyle(i, ref style);
                    if ((tmp.X == x) && (tmp.Y == y))
                    {
                        lastVisibleIndex = i;
                        break;
                    }
                    if ((this.Pointer.Visible && (style != PointerStyles.Nothing)) && ((Math.Abs((int) (tmp.X - x)) < this.Pointer.HorizSize) && (Math.Abs((int) (tmp.Y - y)) < this.Pointer.VertSize)))
                    {
                        lastVisibleIndex = i;
                        break;
                    }
                    if ((i > base.FirstVisibleIndex) && this.ClickedSegment(p, tmpCenter, tmp, old))
                    {
                        lastVisibleIndex = i - 1;
                        break;
                    }
                    old = tmp;
                }
                if (((lastVisibleIndex == -1) && this.CloseCircle) && (base.LastVisibleIndex > base.FirstVisibleIndex))
                {
                    old.X = this.CalcXPos(base.FirstVisibleIndex);
                    old.Y = this.CalcYPos(base.FirstVisibleIndex);
                    if (this.ClickedSegment(p, tmpCenter, tmp, old))
                    {
                        lastVisibleIndex = base.LastVisibleIndex;
                    }
                }
            }
            return lastVisibleIndex;
        }

        private bool ClickedSegment(Point P, Point tmpCenter, Point tmp, Point Old)
        {
            return ((this.Brush.Visible && Graphics3D.PointInTriangle(P, tmpCenter, tmp, Old)) || (this.Pen.Visible && Graphics3D.PointInLine(P, tmp, Old)));
        }

        protected internal override void DoAfterDrawValues()
        {
            if (!base.chart.Axes.DrawBehind)
            {
                bool flag = false;
                for (int i = base.chart.Series.IndexOf(this) + 1; i < base.chart.Series.Count; i++)
                {
                    if (base.chart[i] is CustomPolar)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    this.DrawAxis();
                }
            }
            base.DoAfterDrawValues();
        }

        protected internal override void DoBeforeDrawValues()
        {
            bool flag = false;
            for (int i = 0; i < base.chart.Series.Count; i++)
            {
                if (base.chart[i].Active && (base.chart[i] is CustomPolar))
                {
                    if (base.chart[i] == this)
                    {
                        if ((!flag && this.circleLabels) && !this.circleLabelsInside)
                        {
                            base.chart.graphics3D.Font = this.circleLabelsFont;
                            int num = base.chart.graphics3D.FontHeight + 2;
                            Rectangle chartRect = new Rectangle();
                            chartRect = base.chart.ChartRect;
                            chartRect.Y += num;
                            chartRect.Height -= 2 * num;
                            num = Utils.Round(base.chart.graphics3D.TextWidth("360"));
                            chartRect.X += num;
                            chartRect.Width -= 2 * num;
                            base.chart.ChartRect = chartRect;
                        }
                        break;
                    }
                    flag = true;
                }
            }
            base.DoBeforeDrawValues();
            flag = false;
            for (int j = 0; j < base.chart.Series.Count; j++)
            {
                if (base.chart[j].Active && (base.chart[j] is CustomPolar))
                {
                    if (base.chart[j] == this)
                    {
                        if (!flag)
                        {
                            this.DrawCircle();
                            if (base.chart.Axes.DrawBehind)
                            {
                                this.DrawAxis();
                                return;
                            }
                        }
                        break;
                    }
                    flag = true;
                }
            }
        }

        private void DoDraw(int valueIndex)
        {
            int x = this.CalcXPos(valueIndex);
            int y = this.CalcYPos(valueIndex);
            this.LinePrepareCanvas(base.chart.graphics3D, valueIndex);
            this.InternalDrawValue(valueIndex, x, y);
            this.OldX = x;
            this.OldY = y;
        }

        public override void Draw()
        {
            base.Draw();
            if (this.iPointer.Visible)
            {
                for (int i = base.firstVisible; i <= base.lastVisible; i++)
                {
                    if ((this.treatNulls == TreatNullsStyle.Ignore) || !base.IsNull(i))
                    {
                        Color aColor = this.CalcValueColor(i);
                        PointerStyles style = this.iPointer.Style;
                        this.OnGetPointerStyle(i, ref style, ref aColor);
                        this.iPointer.Draw(this.CalcXPos(i), this.CalcYPos(i), aColor, style);
                    }
                }
            }
        }

        private void DrawAngleLabel(double angle, int index)
        {
            int num;
            int num2;
            double num4;
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Font = this.circleLabelsFont;
            int fontHeight = graphicsd.FontHeight;
            if (angle >= 360.0)
            {
                angle -= 360.0;
            }
            string circleLabel = this.GetCircleLabel(angle, index);
            int xRadius = base.XRadius;
            int yRadius = base.YRadius;
            if (this.CircleLabelsInside)
            {
                xRadius -= Utils.Round(graphicsd.TextWidth("   "));
                yRadius -= Utils.Round(graphicsd.TextHeight(circleLabel));
                num4 = ((double) -this.labelsMargin) / 100.0;
            }
            else
            {
                num4 = ((double) this.labelsMargin) / 100.0;
            }
            xRadius += Utils.Round((double) (num4 * xRadius));
            yRadius += Utils.Round((double) (num4 * yRadius));
            this.AngleToPos(angle * 0.017453292519943295, (double) xRadius, (double) yRadius, out num, out num2);
            angle += base.RotationAngle;
            double a = angle * 0.017453292519943295;
            if (this.circleLabelsRot)
            {
                if ((angle > 90.0) && (angle < 270.0))
                {
                    num += Utils.Round((double) ((0.5 * fontHeight) * Math.Sin(angle * 0.017453292519943295)));
                    num2 += Utils.Round((double) ((0.5 * fontHeight) * Math.Cos(angle * 0.017453292519943295)));
                }
                else
                {
                    num -= Utils.Round((double) ((0.5 * fontHeight) * Math.Sin(a)));
                    num2 -= Utils.Round((double) ((0.5 * fontHeight) * Math.Cos(a)));
                }
            }
            if (angle >= 360.0)
            {
                angle -= 360.0;
            }
            if (this.circleLabelsRot)
            {
                if ((angle > 90.0) && (angle < 270.0))
                {
                    graphicsd.TextAlign = StringAlignment.Far;
                    angle += 180.0;
                }
                else
                {
                    graphicsd.TextAlign = StringAlignment.Near;
                }
                graphicsd.RotateLabel(num, num2, base.EndZ, circleLabel, angle);
            }
            else
            {
                if ((angle == 0.0) || (angle == 180.0))
                {
                    num2 -= fontHeight / 2;
                }
                else if ((angle > 0.0) && (angle < 180.0))
                {
                    num2 -= fontHeight;
                }
                if ((angle == 90.0) || (angle == 270.0))
                {
                    graphicsd.TextAlign = StringAlignment.Center;
                }
                else if (this.circleLabelsInside)
                {
                    if ((angle > 90.0) && (angle < 270.0))
                    {
                        graphicsd.TextAlign = StringAlignment.Near;
                    }
                    else
                    {
                        graphicsd.TextAlign = StringAlignment.Far;
                    }
                }
                else if ((angle > 90.0) && (angle < 270.0))
                {
                    graphicsd.TextAlign = StringAlignment.Far;
                }
                else
                {
                    graphicsd.TextAlign = StringAlignment.Near;
                }
                int num3 = Utils.Round((float) (graphicsd.TextWidth("0") / 2f));
                if (angle == 0.0)
                {
                    num += num3;
                }
                else if (angle == 180.0)
                {
                    num -= num3;
                }
                graphicsd.TextOut(num, num2, base.EndZ, circleLabel);
            }
        }

        private void DrawAxis()
        {
            this.DrawXGrid();
            this.DrawYGrid();
            if (base.chart.Axes.Visible)
            {
                int num;
                if (base.chart.Axes.Right.Visible)
                {
                    num = base.CircleXCenter + base.chart.Axes.Right.SizeTickAxis();
                    base.chart.Axes.Right.Draw(num, num + base.chart.Axes.Right.SizeLabels(), base.CircleXCenter, false, base.chart.Axes.Left.Minimum, base.chart.Axes.Left.Maximum, base.chart.Axes.Left.Increment, base.CircleYCenter - base.YRadius, base.CircleYCenter);
                }
                if (this.IMaxValuesCount == 0)
                {
                    Axis left = base.chart.Axes.Left;
                    if (left.Visible)
                    {
                        left.InternalSetInverted(true);
                        num = base.CircleXCenter - left.SizeTickAxis();
                        left.Draw(num, num - left.SizeLabels(), base.CircleXCenter, false, base.CircleYCenter, base.CircleYCenter + base.YRadius);
                        left.InternalSetInverted(false);
                    }
                    if (base.chart.Axes.Top.Visible)
                    {
                        base.chart.Axes.Top.InternalSetInverted(true);
                        num = base.CircleYCenter - base.chart.Axes.Top.SizeTickAxis();
                        base.chart.Axes.Top.Draw(num, num - base.chart.Axes.Top.SizeLabels(), base.CircleYCenter, false, left.Minimum, left.Maximum, left.Increment, base.CircleXCenter - base.XRadius, base.CircleXCenter);
                        base.chart.Axes.Top.InternalSetInverted(false);
                    }
                    if (base.chart.Axes.Bottom.Visible)
                    {
                        num = base.CircleYCenter + base.chart.Axes.Bottom.SizeTickAxis();
                        base.chart.Axes.Bottom.Draw(num, num + base.chart.Axes.Bottom.SizeLabels(), base.CircleYCenter, false, left.Minimum, left.Maximum, left.Increment, base.CircleXCenter, base.CircleXCenter + base.XRadius);
                    }
                }
            }
        }

        private void DrawCircle()
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            bool visible = graphicsd.Brush.Visible;
            Color color = graphicsd.Brush.Color;
            bool solid = graphicsd.Brush.Solid;
            Gradient gradient = graphicsd.Brush.Gradient.Clone() as Gradient;
            if (Utils.ColorIsEmpty(base.CircleBackColor) && (base.CalcCircleGradient() == null))
            {
                graphicsd.Brush.Visible = false;
            }
            else
            {
                graphicsd.Brush.Visible = true;
                graphicsd.Brush.Solid = true;
                graphicsd.Brush.Color = base.CalcCircleBackColor();
                graphicsd.Brush.Gradient = base.CircleGradient;
            }
            graphicsd.Pen = this.CirclePen;
            this.DrawPolarCircle(base.CircleWidth / 2, base.CircleHeight / 2, base.EndZ);
            graphicsd.Brush.Solid = solid;
            graphicsd.Brush.Visible = visible;
            graphicsd.Brush.Color = color;
            graphicsd.Brush.Gradient = gradient;
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            if (this.Pen.Visible)
            {
                this.LinePrepareCanvas(g, valueIndex);
                g.HorizontalLine(rect.X, rect.Right, (rect.Y + rect.Bottom) / 2);
            }
            if (this.iPointer.Visible)
            {
                Color color = (valueIndex == -1) ? base.Color : this.CalcValueColor(valueIndex);
                if (this.GetPointerStyle != null)
                {
                    this.iPointer.Color = color;
                    PointerStyles style = this.iPointer.Style;
                    this.GetPointerStyle(this, new GetPointerStyleEventArgs(valueIndex, style));
                    color = this.iPointer.Color;
                }
                this.iPointer.DrawLegendShape(g, color, rect, this.Pen.Visible);
            }
            else if (!this.Pen.Visible)
            {
                base.DrawLegendShape(g, valueIndex, rect);
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            base.Marks.ApplyArrowLength(ref position);
            base.DrawMark(valueIndex, s, position);
        }

        protected virtual void DrawPolarCircle(int HalfWidth, int HalfHeight, int Z)
        {
            if (this.IMaxValuesCount == 0)
            {
                base.chart.graphics3D.Ellipse(base.CircleXCenter - HalfWidth, base.CircleYCenter - HalfHeight, base.CircleXCenter + HalfWidth, base.CircleYCenter + HalfHeight, Z);
            }
            else
            {
                int num2;
                int num3;
                double num = (Utils.PiStep * 360.0) / ((double) this.IMaxValuesCount);
                this.AngleToPos(0.0, (double) HalfWidth, (double) HalfHeight, out num2, out num3);
                base.chart.graphics3D.MoveTo(num2, num3, Z);
                for (int i = 0; i <= this.IMaxValuesCount; i++)
                {
                    int num4;
                    int num5;
                    this.AngleToPos(i * num, (double) HalfWidth, (double) HalfHeight, out num4, out num5);
                    if (base.chart.graphics3D.Brush.Visible)
                    {
                        this.FillTriangle(num2, num3, num4, num5, Z);
                    }
                    base.chart.graphics3D.LineTo(num4, num5, Z);
                    num2 = num4;
                    num3 = num5;
                }
            }
        }

        public void DrawRing(double value, int z)
        {
            double num = base.GetVertAxis.Maximum - base.GetVertAxis.Minimum;
            if (num != 0.0)
            {
                num = (value - base.GetVertAxis.Minimum) / num;
                this.DrawPolarCircle(Utils.Round((double) (num * base.XRadius)), Utils.Round((double) (num * base.YRadius)), z);
            }
        }

        public override void DrawValue(int valueIndex)
        {
            if (this.treatNulls == TreatNullsStyle.Ignore)
            {
                this.DoDraw(valueIndex);
            }
            else if (base.IsNull(valueIndex))
            {
                if (this.treatNulls == TreatNullsStyle.DoNotPaint)
                {
                    int index = valueIndex + 1;
                    if (index == base.Count)
                    {
                        index = 0;
                    }
                    this.OldX = this.CalcXPos(index);
                    this.OldY = this.CalcYPos(index);
                    base.chart.graphics3D.MoveTo(this.OldX, this.OldY, base.startZ);
                }
            }
            else
            {
                this.DoDraw(valueIndex);
            }
        }

        private void DrawXGrid()
        {
            if (base.GetHorizAxis.Grid.Visible || this.circleLabels)
            {
                double increment = base.GetHorizAxis.Increment;
                if (increment <= 0.0)
                {
                    increment = 10.0;
                }
                this.SetGridCanvas(base.GetHorizAxis);
                int index = 0;
                double angle = 0.0;
                while (angle < 360.0)
                {
                    if (this.circleLabels)
                    {
                        this.DrawAngleLabel(angle, index);
                    }
                    if (base.GetHorizAxis.Grid.Visible)
                    {
                        int num;
                        int num2;
                        this.AngleToPos(Utils.PiStep * angle, (double) base.XRadius, (double) base.YRadius, out num, out num2);
                        base.chart.graphics3D.Line(base.CircleXCenter, base.CircleYCenter, num, num2, base.EndZ);
                    }
                    angle += increment;
                    index++;
                }
                base.chart.graphics3D.Brush.Visible = true;
            }
        }

        private void DrawYGrid()
        {
            if (base.GetVertAxis.Grid.Visible)
            {
                double calcIncrement = base.GetVertAxis.CalcIncrement;
                if (calcIncrement > 0.0)
                {
                    this.SetGridCanvas(base.GetVertAxis);
                    double maximum = base.GetVertAxis.Maximum / calcIncrement;
                    if ((Math.Abs(maximum) < 2147483647.0) && (Math.Abs((double) ((base.GetVertAxis.Maximum - base.GetVertAxis.Minimum) / calcIncrement)) < 10000.0))
                    {
                        if (base.GetVertAxis.Labels.RoundFirstLabel)
                        {
                            maximum = calcIncrement * Utils.Round(maximum);
                        }
                        else
                        {
                            maximum = base.GetVertAxis.Maximum;
                        }
                        if (!base.GetVertAxis.Labels.OnAxis)
                        {
                            while (maximum >= base.GetVertAxis.Maximum)
                            {
                                maximum -= calcIncrement;
                            }
                            while (maximum > base.GetVertAxis.Minimum)
                            {
                                this.DrawRing(maximum, base.EndZ);
                                maximum -= calcIncrement;
                            }
                        }
                        else
                        {
                            while (maximum > base.GetVertAxis.Maximum)
                            {
                                maximum -= calcIncrement;
                            }
                            while (maximum >= base.GetVertAxis.Minimum)
                            {
                                this.DrawRing(maximum, base.EndZ);
                                maximum -= calcIncrement;
                            }
                        }
                    }
                    base.chart.graphics3D.Brush.Visible = true;
                }
            }
        }

        public void DrawZone(double Min, double Max, int z)
        {
            double num = base.GetVertAxis.Maximum - base.GetVertAxis.Minimum;
            if (num != 0.0)
            {
                num = (Max - base.GetVertAxis.Minimum) / num;
                this.DrawPolarCircle(Utils.Round((double) (num * base.XRadius)), Utils.Round((double) (num * base.YRadius)), z);
            }
            num = base.GetVertAxis.Maximum - base.GetVertAxis.Minimum;
            if (num != 0.0)
            {
                num = (Min - base.GetVertAxis.Minimum) / num;
                int num2 = Utils.Round((double) (num * base.XRadius));
                int num3 = Utils.Round((double) (num * base.YRadius));
                if (this.IMaxValuesCount == 0)
                {
                    base.chart.graphics3D.TransparentEllipse(base.CircleXCenter - num2, base.CircleYCenter - num3, base.CircleXCenter + num2, base.CircleYCenter + num3, z);
                }
            }
        }

        private void FillTriangle(int aX, int aY, int bX, int bY, int z)
        {
            DashStyle style = base.chart.graphics3D.Pen.Style;
            base.chart.graphics3D.Triangle(new Point(aX, aY), new Point(bX, bY), new Point(base.CircleXCenter, base.CircleYCenter), z);
        }

        private double GetAngleIncrement()
        {
            if (base.chart == null)
            {
                return 10.0;
            }
            double increment = base.GetHorizAxis.Increment;
            if (increment == 0.0)
            {
                increment = 10.0;
            }
            return increment;
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        protected virtual string GetCircleLabel(double angle, int index)
        {
            double num = this.clockWiseLabels ? (360.0 - angle) : angle;
            if (num == 360.0)
            {
                num = 0.0;
            }
            return (num.ToString() + "\x00ba");
        }

        private double GetRadiusIncrement()
        {
            if (base.chart != null)
            {
                return base.GetVertAxis.Increment;
            }
            return 0.0;
        }

        protected virtual double GetXValue(int valueIndex)
        {
            return base.vxValues[valueIndex];
        }

        protected virtual void InternalDrawValue(int valueIndex, int X, int Y)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if (valueIndex == base.firstVisible)
            {
                graphicsd.MoveTo(X, Y, base.startZ);
            }
            else
            {
                if ((X != this.OldX) || (Y != this.OldY))
                {
                    this.TryFillTriangle(valueIndex, X, Y);
                    graphicsd.LineTo(X, Y, base.startZ);
                }
                if ((valueIndex == base.lastVisible) && this.closeCircle)
                {
                    if (base.ColorEach && base.bBrush.Visible)
                    {
                        this.Pen.Color = this.ValueColor(0);
                    }
                    this.OldX = X;
                    this.OldY = Y;
                    X = this.CalcXPos(0);
                    Y = this.CalcYPos(0);
                    this.TryFillTriangle(valueIndex, X, Y);
                    graphicsd.LineTo(X, Y, base.startZ);
                    X = this.OldX;
                    Y = this.OldY;
                }
            }
        }

        protected internal override Color LegendItemColor(int index)
        {
            return this.CalcValueColor(index);
        }

        private void LinePrepareCanvas(Graphics3D g, int valueIndex)
        {
            if (this.Pen.Visible)
            {
                Color color;
                if (valueIndex == -1)
                {
                    color = base.Color;
                }
                else if (Utils.ColorIsEmpty(this.Pen.Color))
                {
                    color = this.ValueColor(valueIndex);
                }
                else
                {
                    color = this.Pen.Color;
                }
                g.Pen = this.Pen;
                g.Pen.Color = color;
            }
            else
            {
                g.Pen.Visible = false;
            }
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            this.Pointer.Gradient.StartColor = color;
            this.Pen.Color = color;
        }

        public virtual void OnGetPointerStyle(int valueIndex, ref PointerStyles style)
        {
            Color aColor = this.Pointer.Color;
            this.OnGetPointerStyle(valueIndex, ref style, ref aColor);
        }

        public virtual void OnGetPointerStyle(int valueIndex, ref PointerStyles style, ref Color aColor)
        {
            if (this.GetPointerStyle != null)
            {
                GetPointerStyleEventArgs e = new GetPointerStyleEventArgs(valueIndex, style) {
                    Color = aColor
                };
                this.GetPointerStyle(this, e);
                style = e.Style;
                aColor = e.Color;
            }
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.Circled = true;
            base.GetHorizAxis.Increment = 90.0;
            base.chart.aspect.Chart3DPercent = 5;
            base.chart.axes.Right.Labels.Visible = false;
            base.chart.axes.Top.Labels.Visible = false;
            base.chart.aspect.Orthogonal = false;
            base.chart.aspect.Elevation = 360;
            base.chart.aspect.Zoom = 90;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.iPointer != null)
            {
                this.iPointer.Chart = base.chart;
            }
            if ((base.chart != null) && base.DesignMode)
            {
                base.chart.aspect.view3D = false;
            }
            if (this.font != null)
            {
                this.font.Chart = base.chart;
            }
            if (this.pen != null)
            {
                this.pen.Chart = base.chart;
            }
            if (this.circlePen != null)
            {
                this.circlePen.Chart = base.chart;
            }
            if (this.circleLabelsFont != null)
            {
                this.circleLabelsFont.Chart = base.chart;
            }
        }

        private void SetGridCanvas(Axis axis)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Brush.Visible = false;
            graphicsd.Pen = axis.Grid;
            if (Utils.ColorIsEmpty(graphicsd.Pen.Color))
            {
                graphicsd.Pen.Color = Color.Gray;
            }
        }

        private void TryFillTriangle(int valueIndex, int x, int y)
        {
            if (this.Brush.Visible)
            {
                base.chart.Graphics3D.Brush = this.Brush;
                base.chart.Graphics3D.Brush.Color = this.CalcValueColor(valueIndex);
                this.FillTriangle(this.OldX, this.OldY, x, y, base.StartZ);
                this.LinePrepareCanvas(base.chart.graphics3D, valueIndex);
            }
        }

        [Description("Sets angle in degrees to draw the dividing grid lines."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double AngleIncrement
        {
            get
            {
                return this.GetAngleIncrement();
            }
            set
            {
                if (base.chart != null)
                {
                    if (base.GetHorizAxis == null)
                    {
                        base.RecalcGetAxis();
                    }
                    base.GetHorizAxis.Increment = value;
                }
            }
        }

        [Description("Gets list of angle values for each polar point.")]
        public ValueList AngleValues
        {
            get
            {
                return base.vxValues;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets Polar Back Brush.")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [Description("Sets CicleLabel properties."), DefaultValue(false)]
        public bool CircleLabels
        {
            get
            {
                return this.circleLabels;
            }
            set
            {
                base.SetBooleanProperty(ref this.circleLabels, value);
            }
        }

        [Description("Determines Font characteristics for the labels of a circular chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartFont CircleLabelsFont
        {
            get
            {
                return this.circleLabelsFont;
            }
        }

        [DefaultValue(false), Description("Displays the axis labels inside the circle area.")]
        public bool CircleLabelsInside
        {
            get
            {
                return this.circleLabelsInside;
            }
            set
            {
                base.SetBooleanProperty(ref this.circleLabelsInside, value);
            }
        }

        [DefaultValue(false)]
        public bool CircleLabelsRotated
        {
            get
            {
                return this.circleLabelsRot;
            }
            set
            {
                base.SetBooleanProperty(ref this.circleLabelsRot, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines the pen used to draw the outmost circle.")]
        public ChartPen CirclePen
        {
            get
            {
                return this.circlePen;
            }
        }

        [Description("Displays of the circle labels clockwise."), DefaultValue(false)]
        public bool ClockWiseLabels
        {
            get
            {
                return this.clockWiseLabels;
            }
            set
            {
                base.SetBooleanProperty(ref this.clockWiseLabels, value);
            }
        }

        [DefaultValue(true), Description("Draws a Line between the last and first coordinates.")]
        public bool CloseCircle
        {
            get
            {
                return this.closeCircle;
            }
            set
            {
                base.SetBooleanProperty(ref this.closeCircle, value);
            }
        }

        [Description("Sets the label font."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartFont Font
        {
            get
            {
                if (this.font == null)
                {
                    this.font = new ChartFont(base.chart);
                }
                return this.font;
            }
        }

        [Description("Defines the distance between the CustomPolar circle and the labels."), DefaultValue(3)]
        public int LabelsMargin
        {
            get
            {
                return this.labelsMargin;
            }
            set
            {
                base.SetIntegerProperty(ref this.labelsMargin, value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Pen used to draw the Line connecting PolarSeries points.")]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("")]
        public SeriesPointer Pointer
        {
            get
            {
                if (this.iPointer == null)
                {
                    this.iPointer = new SeriesPointer(base.chart, this);
                }
                return this.iPointer;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Determines the increment used to draw the ring grid lines.")]
        public double RadiusIncrement
        {
            get
            {
                return this.GetRadiusIncrement();
            }
            set
            {
                if (base.chart != null)
                {
                    base.GetVertAxis.Increment = value;
                }
            }
        }

        [Description("Gets list of radius values for each polar point.")]
        public ValueList RadiusValues
        {
            get
            {
                return base.vyValues;
            }
        }

        [DefaultValue(0), Description("Sets Transparency level from 0 to 100%."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance")]
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

        [Description("Defines how null points are treated."), DefaultValue(2)]
        public TreatNullsStyle TreatNulls
        {
            get
            {
                return this.treatNulls;
            }
            set
            {
                this.treatNulls = value;
                this.Invalidate();
            }
        }

        public delegate void GetPointerStyleEventHandler(CustomPolar series, GetPointerStyleEventArgs e);
    }
}

