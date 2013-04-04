namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(Pie), "SeriesIcons.Pie.bmp")]
    public class Pie : Circular
    {
        public PieAngle[] Angles;
        private int angleSize;
        private bool autoMarkPosition;
        public const int BelongsToOther = -1;
        private int bevelPercent;
        private bool dark3D;
        private bool darkPen;
        private EdgeStyles edgeStyle;
        private int explodeBiggest;
        private ExplodedSliceList explodedSlice;
        protected int iDonutPercent;
        internal int IniX;
        internal int IniY;
        private Rectangle iOldChartRect;
        private bool IsExploded;
        private MultiPies multiPie;
        private Color oldPenColor;
        private const int OtherFlag = 0x7fffffff;
        private PieOtherSlice otherSlice;
        private ChartPen pen;
        private PieMarks piemarks;
        private PieShadow shadow;
        private SliceValueList sliceHeight;
        private int[] sortedSlice;
        private bool usePatterns;

        public Pie() : this(null)
        {
        }

        public Pie(Chart c) : base(c)
        {
            this.angleSize = 360;
            this.autoMarkPosition = true;
            this.dark3D = true;
            this.edgeStyle = EdgeStyles.None;
            this.explodedSlice = new ExplodedSliceList(0);
            this.sliceHeight = new SliceValueList();
            base.bColorEach = true;
            base.Marks.Visible = true;
            this.pen = new ChartPen(base.chart, Color.Black);
            base.marks.Arrow.Color = Color.Black;
            base.marks.Arrow.defaultColor = Color.Black;
            base.marks.Callout.Length = 8;
            base.marks.defaultVisible = true;
            base.marks.defaultArrowLength = 8;
            base.UseSeriesColor = false;
            this.sliceHeight.OwnerSeries = this;
            base.YValues.Name = Texts.ValuesPie;
        }

        protected override void AddSampleValues(int numValues)
        {
            string[] strArray = new string[] { Texts.PieSample1, Texts.PieSample2, Texts.PieSample3, Texts.PieSample4, Texts.PieSample5, Texts.PieSample6, Texts.PieSample7, Texts.PieSample8 };
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 0; i < numValues; i++)
            {
                base.Add((double) (1 + Utils.Round((double) (1000.0 * random.Random()))), strArray[i % 8]);
            }
        }

        public bool BelongsToOtherSlice(int valueIndex)
        {
            return (base.notMandatory[valueIndex] == -1.0);
        }

        private Point CalcAngle(double angle, int OffX, int OffY)
        {
            int num;
            int num2;
            this.AngleToPos(angle, (double) base.XRadius, (double) base.YRadius, out num, out num2);
            num += OffX;
            num2 -= OffY;
            return base.chart.Graphics3D.Calculate3DPosition(num, num2, base.EndZ);
        }

        private void CalcAngles()
        {
            double totalABS;
            double num = 6.2831853071795862 * (((double) this.angleSize) / 360.0);
            if ((this.OtherSlice.Style == PieOtherStyles.None) && (base.FirstVisibleIndex != -1))
            {
                totalABS = 0.0;
                for (int j = base.FirstVisibleIndex; j <= base.LastVisibleIndex; j++)
                {
                    totalABS += Math.Abs(base.mandatory[j]);
                }
            }
            else
            {
                totalABS = base.mandatory.TotalABS;
            }
            double num4 = (totalABS != 0.0) ? (num / totalABS) : 0.0;
            this.Angles = new PieAngle[base.Count];
            double num5 = 0.0;
            for (int i = base.FirstVisibleIndex; i <= base.LastVisibleIndex; i++)
            {
                this.Angles[i] = new PieAngle();
                this.Angles[i].StartAngle = (i == base.FirstVisibleIndex) ? 0.0 : this.Angles[i - 1].EndAngle;
                if (totalABS != 0.0)
                {
                    if (!this.BelongsToOtherSlice(i))
                    {
                        num5 += Math.Abs(base.mandatory[i]);
                    }
                    if (num5 == totalABS)
                    {
                        this.Angles[i].EndAngle = num;
                    }
                    else
                    {
                        this.Angles[i].EndAngle = num5 * num4;
                    }
                    if ((this.Angles[i].EndAngle - this.Angles[i].StartAngle) > num)
                    {
                        this.Angles[i].EndAngle = this.Angles[i].StartAngle + num;
                    }
                }
                else
                {
                    this.Angles[i].EndAngle = num;
                }
                this.Angles[i].MidAngle = (this.Angles[i].StartAngle + this.Angles[i].EndAngle) * 0.5;
            }
        }

        private int CalcClickedPie(int x, int y)
        {
            if (((this.Angles != null) && (this.Angles.Length > 0)) && (base.chart != null))
            {
                Point[] poly = new Point[0x20];
                Point p = new Point(x, y);
                for (int i = 0; i < base.Count; i++)
                {
                    int num2;
                    int num3;
                    this.CalcExplodedOffset(i, out num2, out num3);
                    poly[0] = this.CalcAngle(this.Angles[i].StartAngle, num2, num3);
                    poly[1] = base.chart.graphics3D.Calculate3DPosition(base.CircleXCenter + num2, base.CircleYCenter - num3, base.EndZ);
                    double num = (this.Angles[i].EndAngle - this.Angles[i].StartAngle) / 30.0;
                    for (int j = 2; j < 0x20; j++)
                    {
                        poly[j] = this.CalcAngle(this.Angles[i].EndAngle - ((j - 2) * num), num2, num3);
                    }
                    if (Graphics3D.PointInPolygon(p, poly))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void CalcExplodeBiggest()
        {
            int index = base.YValues.IndexOf(base.YValues.Maximum);
            if (index != -1)
            {
                this.explodedSlice[index] = this.explodeBiggest;
            }
        }

        private void CalcExplodedOffset(int valueIndex, out int offsetX, out int offsetY)
        {
            offsetX = 0;
            offsetY = 0;
            if (this.IsExploded)
            {
                double num = this.explodedSlice[valueIndex];
                if (num > 0.0)
                {
                    double midAngle = this.Angles[valueIndex].MidAngle;
                    double resultSin = 0.0;
                    double resultCos = 0.0;
                    Utils.SinCos(midAngle + base.rotDegree, out resultSin, out resultCos);
                    num *= 0.01;
                    offsetX = Utils.Round((double) ((base.iXRadius * num) * resultCos));
                    offsetY = Utils.Round((double) ((base.iYRadius * num) * resultSin));
                }
            }
        }

        protected void CalcExplodedRadius(int valueIndex, out int aXRadius, out int aYRadius)
        {
            double num = 1.0 + (this.explodedSlice[valueIndex] * 0.01);
            aXRadius = Utils.Round((double) (base.iXRadius * num));
            aYRadius = Utils.Round((double) (base.iYRadius * num));
        }

        public override int CalcXPos(int valueIndex)
        {
            if (base.vxValues[valueIndex] == 2147483647.0)
            {
                return 0;
            }
            return base.CalcXPos(valueIndex);
        }

        protected override void ClearLists()
        {
            base.ClearLists();
            this.explodedSlice.Clear();
            this.sliceHeight.Clear();
        }

        public override int Clicked(int x, int y)
        {
            int num = base.Clicked(x, y);
            if (num == -1)
            {
                num = this.CalcClickedPie(x, y);
            }
            return num;
        }

        private int CompareSlice(int A, int B)
        {
            double totalAngle = (6.2831853071795862 * this.angleSize) / 360.0;
            double angleSlice = this.GetAngleSlice(this.sortedSlice[A], totalAngle);
            double num3 = this.GetAngleSlice(this.sortedSlice[B], totalAngle);
            if (angleSlice < num3)
            {
                return -1;
            }
            if (angleSlice > num3)
            {
                return 1;
            }
            return 0;
        }

        protected internal override int CountLegendItems()
        {
            int num = 0;
            for (int i = 0; i < base.Count; i++)
            {
                if (this.BelongsToOtherSlice(i))
                {
                    num++;
                }
            }
            if ((base.chart.Legend != null) && (base.chart.Legend == this.OtherSlice.Legend))
            {
                return num;
            }
            return (base.Count - num);
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Patterns);
            AddSubChart(Texts.Exploded);
            AddSubChart(Texts.Shadow);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.SemiPie);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.DarkPen);
        }

        private void DisableRotation()
        {
            base.chart.aspect.Orthogonal = false;
            base.chart.aspect.Rotation = 0;
            base.chart.aspect.Elevation = 0x131;
        }

        protected internal override void DoAfterDrawValues()
        {
            base.chart.ChartRect = this.iOldChartRect;
            base.DoAfterDrawValues();
        }

        protected internal override void DoBeforeDrawChart()
        {
            if (this.PieValues.Order != ValueListOrder.None)
            {
                this.PieValues.Sort();
            }
            this.RemoveOtherSlice();
            SeriesMarks.Position position = this.OtherMarkCustom();
            base.XValues.FillSequence();
            if (((this.otherSlice != null) && (this.otherSlice.Style != PieOtherStyles.None)) && (base.YValues.TotalABS > 0.0))
            {
                bool flag = false;
                double y = 0.0;
                int num3 = 0;
                while (num3 < base.Count)
                {
                    double num = base.YValues[num3];
                    if (this.otherSlice.Style == PieOtherStyles.BelowPercent)
                    {
                        num = (num * 100.0) / base.YValues.TotalABS;
                    }
                    if (num < this.otherSlice.Value)
                    {
                        y += base.YValues[num3];
                        base.XValues[num3] = -1.0;
                        flag = true;
                    }
                    num3++;
                }
                if (flag)
                {
                    num3 = base.Add((double) 2147483647.0, y, this.otherSlice.Text, this.otherSlice.Color);
                    base.YValues.statsOk = false;
                    double totalABS = base.YValues.TotalABS;
                    base.YValues.totalABS = totalABS - y;
                    base.YValues.statsOk = true;
                    if (position != null)
                    {
                        base.Marks.Positions[num3] = position;
                    }
                }
            }
        }

        protected internal override void DoBeforeDrawValues()
        {
            this.iOldChartRect = base.chart.ChartRect;
            if (this.multiPie == MultiPies.Automatic)
            {
                this.guessRectangle();
            }
            base.DoBeforeDrawValues();
        }

        public override void Draw()
        {
            if (this.explodeBiggest > 0)
            {
                this.CalcExplodeBiggest();
            }
            int valueIndex = -1;
            int num2 = 0;
            int count = base.Count;
            for (int i = 0; i < this.explodedSlice.Count; i++)
            {
                if (this.explodedSlice[i] > num2)
                {
                    num2 = Utils.Round((float) this.explodedSlice[i]);
                    valueIndex = i;
                }
            }
            this.CalcAngles();
            this.IsExploded = (valueIndex != -1) || (this.sliceHeight.Count > 0);
            if (valueIndex != -1)
            {
                int num5;
                int num6;
                this.CalcExplodedOffset(valueIndex, out num5, out num6);
                base.CircleRect.Inflate(-Math.Abs(num5) / 2, -Math.Abs(num6) / 2);
                base.AdjustCircleRect();
                base.CalcRadius();
            }
            this.AngleToPos(0.0, (double) base.iXRadius, (double) base.iYRadius, out this.IniX, out this.IniY);
            Graphics3D g = base.chart.graphics3D;
            Rectangle chartRect = base.chart.ChartRect;
            if ((this.OtherSlice.Legend != null) && this.OtherSlice.Legend.Visible)
            {
                Legend legend = base.chart.Legend;
                base.chart.legend = this.OtherSlice.Legend;
                base.chart.DoDrawLegend(ref chartRect);
                base.chart.legend = legend;
            }
            if (this.ShouldDrawShadow())
            {
                this.shadow.Draw(g, base.iCircleXCenter - base.iXRadius, (base.iCircleYCenter - base.iYRadius) + (base.EndZ - base.StartZ), base.iCircleXCenter + base.iXRadius, (base.iCircleYCenter + base.iYRadius) + (base.EndZ - base.StartZ), base.EndZ - base.StartZ);
            }
            if (base.chart.Aspect.View3D && !g.SupportsFullRotation)
            {
                this.sortedSlice = new int[count];
                for (int j = 0; j < count; j++)
                {
                    this.sortedSlice[j] = j;
                }
                Utils.Sort(0, count - 1, new Utils.CompareEventHandler(this.CompareSlice), new Utils.SwapEventHandler(this.SwapSlice));
                for (int k = 0; k < count; k++)
                {
                    this.DrawValue(this.sortedSlice[k]);
                }
            }
            else
            {
                base.Draw();
            }
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            base.DrawLegendShape(g, valueIndex, rect);
            this.Pen.Color = this.oldPenColor;
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            int num4;
            int num5;
            this.CalcExplodedOffset(valueIndex, out num4, out num5);
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.PushMatrix();
            graphicsd.Translate(0, base.iCircleYCenter, 0);
            if (!this.BelongsToOtherSlice(valueIndex))
            {
                int num;
                int num2;
                this.CalcExplodedRadius(valueIndex, out num, out num2);
                double midAngle = this.Angles[valueIndex].MidAngle;
                base.Marks.zPosition = this.SliceEndZ(valueIndex);
                position.ArrowFix = true;
                int x = 0;
                int y = 0;
                int distance = base.Marks.Callout.Length + base.Marks.Callout.Distance;
                this.AngleToPos(midAngle, (double) (num + distance), (double) (num2 + distance), out x, out y);
                position.ArrowTo.X = x;
                position.ArrowTo.Y = y;
                distance = base.Marks.Callout.Distance;
                this.AngleToPos(midAngle, (double) (num + distance), (double) (num2 + distance), out x, out y);
                position.ArrowFrom.X = x;
                position.ArrowFrom.Y = y;
                if (position.ArrowTo.X > base.iCircleXCenter)
                {
                    position.LeftTop.X = position.ArrowTo.X;
                }
                else
                {
                    position.LeftTop.X = position.ArrowTo.X - position.Width;
                }
                if (position.ArrowTo.Y > base.iCircleYCenter)
                {
                    position.LeftTop.Y = position.ArrowTo.Y;
                }
                else
                {
                    position.LeftTop.Y = position.ArrowTo.Y - position.Height;
                }
                if (this.MarksPie.VertCenter)
                {
                    int num9 = position.Height / 2;
                    if (position.ArrowTo.Y > base.iCircleYCenter)
                    {
                        position.ArrowTo.Y += num9;
                    }
                    else
                    {
                        position.ArrowTo.Y -= num9;
                    }
                }
                if (this.MarksPie.LegSize == 0)
                {
                    position.HasMid = false;
                    position.MidPoint.X = 0;
                    position.MidPoint.Y = 0;
                }
                else
                {
                    position.HasMid = true;
                    if (position.ArrowTo.X > base.iCircleXCenter)
                    {
                        if ((position.ArrowTo.X - this.MarksPie.LegSize) < position.ArrowFrom.X)
                        {
                            position.MidPoint.X = position.ArrowFrom.X;
                            position.ArrowTo.X += this.MarksPie.LegSize;
                            position.LeftTop.X = position.ArrowTo.X;
                        }
                        else
                        {
                            position.MidPoint.X = position.ArrowTo.X - this.MarksPie.LegSize;
                        }
                    }
                    else if ((position.ArrowTo.X + this.MarksPie.LegSize) > position.ArrowFrom.X)
                    {
                        position.MidPoint.X = position.ArrowFrom.X;
                        position.ArrowTo.X = position.ArrowFrom.X - this.MarksPie.LegSize;
                        position.LeftTop.X = position.ArrowTo.X - position.Width;
                    }
                    else
                    {
                        position.MidPoint.X = position.ArrowTo.X + this.MarksPie.LegSize;
                    }
                    position.MidPoint.Y = position.ArrowTo.Y;
                }
                if (this.AutoMarkPosition)
                {
                    base.Marks.AntiOverlap(base.firstVisible, valueIndex, position);
                }
                base.DrawMark(valueIndex, s, position);
                graphicsd.PopMatrix();
            }
        }

        protected override void DrawMarksSeries(Series s, ref bool ActiveRegion)
        {
            base.chart.graphics3D.Elevate(90);
            base.DrawMarksSeries(s, ref ActiveRegion);
        }

        protected internal void DrawPie(int valueIndex)
        {
            this.DrawPie(base.chart.graphics3D, valueIndex);
        }

        protected internal void DrawPie(Graphics3D g, int valueIndex)
        {
            int num;
            int num2;
            this.CalcExplodedOffset(valueIndex, out num, out num2);
            if (this.AngleSize < 360)
            {
                this.IsExploded = true;
            }
            g.Pie(base.iCircleXCenter, base.iCircleYCenter, num, num2, base.iXRadius, base.iYRadius, base.StartZ, this.SliceEndZ(valueIndex), this.Angles[valueIndex].StartAngle + base.rotDegree, this.Angles[valueIndex].EndAngle + base.rotDegree, this.dark3D, this.IsExploded, this.iDonutPercent, this.bevelPercent, this.EdgeStyle);
        }

        public override void DrawValue(int valueIndex)
        {
            if (((base.CircleWidth > 4) && (base.CircleHeight > 4)) && !this.BelongsToOtherSlice(valueIndex))
            {
                if (this.usePatterns || base.chart.graphics3D.Monochrome)
                {
                    base.bBrush.Style = Graphics3D.GetDefaultPattern(valueIndex);
                }
                else
                {
                    base.bBrush.Solid = true;
                }
                Color aColor = base.chart.graphics3D.Monochrome ? Color.Black : this.ValueColor(valueIndex);
                base.chart.SetBrushCanvas(aColor, base.bBrush, base.CalcCircleBackColor());
                Color color = this.Pen.Color;
                this.PreparePiePen(base.chart.graphics3D, valueIndex);
                this.DrawPie(valueIndex);
                this.Pen.Color = color;
            }
        }

        public override void GalleryChanged3D(bool is3D)
        {
            base.GalleryChanged3D(is3D);
            this.DisableRotation();
            base.Circled = !base.chart.Aspect.View3D;
        }

        private double GetAngleSlice(int index, double TotalAngle)
        {
            double num = this.Angles[index].MidAngle + base.rotDegree;
            if (num > TotalAngle)
            {
                num -= TotalAngle;
            }
            if (num > (0.25 * TotalAngle))
            {
                num -= 0.25 * TotalAngle;
                if (num > 3.1415926535897931)
                {
                    num = TotalAngle - num;
                }
                return num;
            }
            return ((0.25 * TotalAngle) - num);
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        private void guessRectangle()
        {
            int num = this.pieCount();
            if (num > 1)
            {
                int num2 = this.pieIndex();
                Rectangle chartRect = base.chart.ChartRect;
                int width = chartRect.Width;
                int height = chartRect.Height;
                int num5 = (int) Math.Round(Math.Sqrt((double) num));
                chartRect.X += (num2 % num5) * (width / num5);
                chartRect.Width = width / num5;
                int num6 = (int) Math.Round((double) (0.5 + Math.Sqrt((double) num)));
                chartRect.Y += (num2 / num5) * (height / num6);
                chartRect.Height = height / num6;
                base.chart.ChartRect = base.chart.Graphics3D.CalcRect3D(chartRect, 0);
            }
        }

        protected internal override int LegendToValueIndex(int legendIndex)
        {
            int num = -1;
            bool flag2 = (base.chart.Legend != null) && (base.chart.Legend == this.OtherSlice.Legend);
            for (int i = 0; i < base.Count; i++)
            {
                bool flag = this.BelongsToOtherSlice(i);
                if ((flag2 && flag) || (!flag2 && !flag))
                {
                    num++;
                    if (num == legendIndex)
                    {
                        return i;
                    }
                }
            }
            return legendIndex;
        }

        protected internal override int NumSampleValues()
        {
            return 8;
        }

        private SeriesMarks.Position OtherMarkCustom()
        {
            SeriesMarks.Position source = null;
            for (int i = 0; i < base.Count; i++)
            {
                if (base.vxValues[i] == 2147483647.0)
                {
                    SeriesMarks.Position position2 = base.Marks.Positions[i];
                    if ((position2 != null) && position2.Custom)
                    {
                        source = new SeriesMarks.Position();
                        position2.Assign(source);
                    }
                    return source;
                }
            }
            return source;
        }

        private int pieCount()
        {
            int num = 0;
            foreach (Series series in base.chart.Series)
            {
                if (series.Active && base.SameClass(series))
                {
                    num++;
                }
            }
            return num;
        }

        private int pieIndex()
        {
            int num = 0;
            foreach (Series series in base.chart.Series)
            {
                if (series == this)
                {
                    return num;
                }
                if (series.Active && base.SameClass(series))
                {
                    num++;
                }
            }
            return num;
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.FillSampleValues(8);
            base.chart.Aspect.Chart3DPercent = 0x4b;
            base.Marks.Callout.Length = 0;
            base.Marks.DrawEvery = 1;
            this.DisableRotation();
            this.ColorEach = IsEnabled;
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            base.PrepareLegendCanvas(g, valueIndex, ref backColor, ref aBrush);
            this.oldPenColor = this.Pen.Color;
            this.PreparePiePen(g, valueIndex);
            if (this.usePatterns || g.Monochrome)
            {
                aBrush.Style = Graphics3D.GetDefaultPattern(valueIndex);
            }
            else
            {
                aBrush.Solid = true;
            }
        }

        private void PreparePiePen(Graphics3D g, int valueIndex)
        {
            if (this.Pen.Visible)
            {
                g.Pen = this.Pen;
                if (this.ColorEach)
                {
                    g.Pen.Color = Utils.DarkenColor(this.ValueColor(valueIndex), 60);
                }
                else if (this.darkPen)
                {
                    Color c = this.ValueColor(valueIndex);
                    Graphics3D.ApplyDark(ref c, 0x80);
                    g.Pen.Color = c;
                }
            }
            else
            {
                Color color2 = this.ValueColor(valueIndex);
                if (this.Brush.Transparency > 0)
                {
                    color2 = Color.FromArgb(Utils.Round((float) (this.Brush.Transparency / 2)), color2.R, color2.G, color2.B);
                }
                g.Pen.Visible = true;
                g.Pen.Color = color2;
            }
        }

        private void RemoveOtherSlice()
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base.vxValues.Value[i] == 2147483647.0)
                {
                    this.Delete(i);
                    return;
                }
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pen != null)
            {
                this.pen.Chart = base.chart;
            }
            if (this.shadow != null)
            {
                this.shadow.Chart = base.chart;
            }
        }

        protected void SetDonutPercent(int value)
        {
            base.SetIntegerProperty(ref this.iDonutPercent, value);
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.UsePatterns = true;
                    return;

                case 2:
                    this.ExplodeBiggest = 30;
                    return;

                case 3:
                    this.Shadow.Visible = true;
                    this.Shadow.Width = 10;
                    this.Shadow.Height = 10;
                    return;

                case 4:
                    base.Marks.Visible = true;
                    this.Clear();
                    base.Add((double) 30.0, "A");
                    base.Add((double) 70.0, "B");
                    return;

                case 5:
                    this.AngleSize = 180;
                    return;

                case 6:
                    this.Pen.Visible = false;
                    return;

                case 7:
                    this.DarkPen = true;
                    return;
            }
        }

        private bool ShouldDrawShadow()
        {
            if (((this.shadow == null) || !this.shadow.bVisible) || Utils.ColorIsEmpty(this.shadow.Color))
            {
                return false;
            }
            if (this.shadow.Width == 0)
            {
                return (this.shadow.Height != 0);
            }
            return true;
        }

        private int SliceEndZ(int ValueIndex)
        {
            if (this.sliceHeight.Count > ValueIndex)
            {
                return (base.StartZ + Utils.Round((double) (((base.EndZ - base.StartZ) * this.sliceHeight[ValueIndex]) * 0.01)));
            }
            return base.EndZ;
        }

        private void SwapSlice(int a, int b)
        {
            int num = this.sortedSlice[a];
            this.sortedSlice[a] = this.sortedSlice[b];
            this.sortedSlice[b] = num;
        }

        internal override void SwapValueIndex(int a, int b)
        {
            base.SwapValueIndex(a, b);
            if (this.explodedSlice.Count > 0)
            {
                this.explodedSlice.Exchange(a, b);
            }
            if (this.sliceHeight.Count > 0)
            {
                this.sliceHeight.Exchange(a, b);
            }
        }

        [Description("Total angle in degrees (0 to 360) for all slices."), DefaultValue(360)]
        public int AngleSize
        {
            get
            {
                return this.angleSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.angleSize, value);
            }
        }

        [DefaultValue(true), Description("If true, marks will be displayed trying to not overlap one to each other.")]
        public bool AutoMarkPosition
        {
            get
            {
                return this.autoMarkPosition;
            }
            set
            {
                base.SetBooleanProperty(ref this.autoMarkPosition, value);
            }
        }

        [DefaultValue(0), Description("Gets and sets the bevel as a percentage of the pie's depth.")]
        public int BevelPercent
        {
            get
            {
                return this.bevelPercent;
            }
            set
            {
                base.SetIntegerProperty(ref this.bevelPercent, value);
            }
        }

        [Description("Brush fill for PieSeries."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [Description("Draws points with different preset Colors."), DefaultValue(true)]
        public bool ColorEach
        {
            get
            {
                return base.ColorEach;
            }
            set
            {
                base.ColorEach = value;
            }
        }

        [Category("Appearance"), DefaultValue(true), Description("Darkens side of 3D pie section to add depth.")]
        public bool Dark3D
        {
            get
            {
                return this.dark3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.dark3D, value);
            }
        }

        [Description("Darkens pie slice borders."), DefaultValue(false)]
        public bool DarkPen
        {
            get
            {
                return this.darkPen;
            }
            set
            {
                base.SetBooleanProperty(ref this.darkPen, value);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryPie;
            }
        }

        [Description("Gets and sets the EdgeStyle of the bevel."), DefaultValue(typeof(EdgeStyles), "None")]
        public EdgeStyles EdgeStyle
        {
            get
            {
                return this.edgeStyle;
            }
            set
            {
                if (this.edgeStyle != value)
                {
                    this.edgeStyle = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("Displaces the biggest slice from centre by value set.")]
        public int ExplodeBiggest
        {
            get
            {
                return this.explodeBiggest;
            }
            set
            {
                base.SetIntegerProperty(ref this.explodeBiggest, value);
                this.CalcExplodeBiggest();
            }
        }

        [Description("Accesses the properties for exploding any Pie slice.")]
        public ExplodedSliceList ExplodedSlice
        {
            get
            {
                return this.explodedSlice;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Accesses the pie series marks leg and vertical position.")]
        public PieMarks MarksPie
        {
            get
            {
                if (this.piemarks == null)
                {
                    this.piemarks = new PieMarks(base.chart, this);
                }
                return this.piemarks;
            }
            set
            {
                this.piemarks = value;
            }
        }

        [Description("Sets automatic Pie positioning when multiple Pie series exist.")]
        public MultiPies MultiPie
        {
            get
            {
                return this.multiPie;
            }
            set
            {
                if (this.multiPie != value)
                {
                    this.multiPie = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Accesses the OtherSlice properties."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PieOtherSlice OtherSlice
        {
            get
            {
                if (this.otherSlice == null)
                {
                    this.otherSlice = new PieOtherSlice(base.chart, this);
                }
                return this.otherSlice;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Line pen for Pie.")]
        public ChartPen Pen
        {
            get
            {
                return this.pen;
            }
        }

        [Description("Stores the Pie slice values.")]
        public ValueList PieValues
        {
            get
            {
                return base.vyValues;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines the offset shadow of the PieSeries.")]
        public PieShadow Shadow
        {
            get
            {
                if (this.shadow == null)
                {
                    this.shadow = new PieShadow(base.chart);
                }
                return this.shadow;
            }
        }

        [Description("Use SliceHeight array property to specify a different height for each pie slice.")]
        public SliceValueList SliceHeight
        {
            get
            {
                return this.sliceHeight;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets Transparency level from 0 to 100%.")]
        public int Transparency
        {
            get
            {
                return this.Brush.Transparency;
            }
            set
            {
                this.Brush.Transparency = value;
            }
        }

        [DefaultValue(false), Description("Fills Pie Sectors with different Brush pattern styles.")]
        public bool UsePatterns
        {
            get
            {
                return this.usePatterns;
            }
            set
            {
                base.SetBooleanProperty(ref this.usePatterns, value);
            }
        }

        public class ExplodedSliceList : List<int>
        {
            public ExplodedSliceList(int capacity) : base(capacity)
            {
            }

            internal void Exchange(int a, int b)
            {
                int num = this[a];
                this[a] = this[b];
                this[b] = num;
            }

            public int this[int index]
            {
                get
                {
                    if (index < base.Count)
                    {
                        return base[index];
                    }
                    return 0;
                }
                set
                {
                    while (base.Count <= index)
                    {
                        base.Add(0);
                    }
                    base[index] = value;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PieAngle
        {
            public double StartAngle;
            public double MidAngle;
            public double EndAngle;
            public bool Contains(double angle)
            {
                return ((angle >= this.StartAngle) && (angle <= this.EndAngle));
            }
        }

        public class PieMarks : TeeBase
        {
            private int legsize;
            private Series series;
            private bool vertcenter;

            public PieMarks(Chart c, Series s) : base(c)
            {
                if (this.series == null)
                {
                    this.series = s;
                }
            }

            public int LegSize
            {
                get
                {
                    return this.legsize;
                }
                set
                {
                    if (this.legsize != value)
                    {
                        this.legsize = value;
                        if (this.series != null)
                        {
                            this.series.Repaint();
                        }
                    }
                }
            }

            public bool VertCenter
            {
                get
                {
                    return this.vertcenter;
                }
                set
                {
                    if (this.vertcenter != value)
                    {
                        this.vertcenter = value;
                        if (this.series != null)
                        {
                            this.series.Repaint();
                        }
                    }
                }
            }
        }

        public class PieOtherSlice : TeeBase
        {
            private double aValue;
            private System.Drawing.Color color;
            private Steema.TeeChart.Legend legend;
            private Series series;
            private PieOtherStyles style;
            private string text;

            public PieOtherSlice(Chart c, Series s) : base(c)
            {
                this.text = "";
                if (this.series == null)
                {
                    this.series = s;
                }
            }

            private Steema.TeeChart.Legend GetLegend()
            {
                if (this.legend == null)
                {
                    this.legend = new Steema.TeeChart.Legend(this.series.Chart);
                    this.legend.Visible = false;
                    this.legend.Series = this.series;
                }
                return this.legend;
            }

            private void SetLegend(Steema.TeeChart.Legend l)
            {
                if (this.legend != null)
                {
                    this.legend = l;
                    this.legend.Series = this.series;
                }
            }

            [Category("Appearance"), Description("Sets the Color of the OtherSlice.")]
            public System.Drawing.Color Color
            {
                get
                {
                    return this.color;
                }
                set
                {
                    base.SetColorProperty(ref this.color, value);
                }
            }

            [Description("PieOtherSlice Legend.")]
            public Steema.TeeChart.Legend Legend
            {
                get
                {
                    return this.GetLegend();
                }
                set
                {
                    this.SetLegend(value);
                }
            }

            [Description("Sets either value or percentage to group 'other' Pie slice.")]
            public PieOtherStyles Style
            {
                get
                {
                    return this.style;
                }
                set
                {
                    if (this.style != value)
                    {
                        this.style = value;
                        this.Invalidate();
                    }
                }
            }

            [Description("Title for otherSlice.")]
            public string Text
            {
                get
                {
                    return this.text;
                }
                set
                {
                    base.SetStringProperty(ref this.text, value);
                }
            }

            [Description("Value (value or percentage) for Otherslice grouping.")]
            public double Value
            {
                get
                {
                    return this.aValue;
                }
                set
                {
                    base.SetDoubleProperty(ref this.aValue, value);
                }
            }
        }

        public class PieShadow : Shadow
        {
            public PieShadow(Chart c) : base(c)
            {
                base.bBrush.defaultColor = Color.DarkGray;
                base.bBrush.color = Color.DarkGray;
                base.defaultVisible = false;
                base.bVisible = false;
                base.Width = 20;
                base.Height = 20;
            }
        }

        public class SliceValueList : List<int>
        {
            public Series OwnerSeries;

            internal void Exchange(int a, int b)
            {
                int num = this[a];
                this[a] = this[b];
                this[b] = num;
            }

            public int this[int index]
            {
                get
                {
                    if (index < base.Count)
                    {
                        return base[index];
                    }
                    return 0;
                }
                set
                {
                    while (index >= base.Count)
                    {
                        base.Add(0);
                    }
                    if (this[index] != value)
                    {
                        base[index] = value;
                        this.OwnerSeries.Repaint();
                    }
                }
            }
        }
    }
}

