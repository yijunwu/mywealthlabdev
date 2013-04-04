namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [ToolboxBitmap(typeof(VolumePipe), "SeriesIcons.VolumePipe.bmp")]
    public class VolumePipe : Series
    {
        public Point[] BoundingPoints;
        private int conePercent;
        private int IDiff;
        private int IMax;
        private int IMin;
        public ArrayList IPolyList;
        private int lastX;
        private int lastYDisp;
        private int leftWall;
        private ChartPen linesPen;
        private bool lineTakesPointColor;
        private int overallWidth;
        private int rightWall;
        private int separation;
        private int totalPxArea;
        private double totalVals;
        private ChartBrush vBrush;

        public VolumePipe() : this(null)
        {
        }

        public VolumePipe(Chart c) : base(c)
        {
            this.BoundingPoints = new Point[4];
            this.IPolyList = new ArrayList();
            this.vBrush = new ChartBrush();
            base.ColorEach = true;
            this.Brush.Gradient.defaultVisible = true;
            this.Brush.Gradient.Visible = this.Brush.Gradient.defaultVisible;
            this.conePercent = 30;
            base.UseAxis = false;
            this.SetSeriesColor(Color.DarkGray);
        }

        protected override void AddSampleValues(int numValues)
        {
            string[] strArray = new string[5];
            Series.SeriesRandom random = new Series.SeriesRandom();
            strArray[0] = Texts.PieSample1;
            strArray[1] = Texts.PieSample2;
            strArray[2] = Texts.PieSample3;
            strArray[3] = Texts.PieSample4;
            strArray[4] = Texts.PieSample5;
            for (int i = 0; i < numValues; i++)
            {
                base.Add((double) (random.Random() * 100.0), strArray[i % 5]);
            }
        }

        private int CalcSegment(int Counter, double Val)
        {
            double num7;
            double num = 0.0;
            for (int i = 0; i <= Counter; i++)
            {
                num += base.YValues[i];
            }
            if (this.totalVals == 0.0)
            {
                return 0;
            }
            double num2 = (num / this.totalVals) * 100.0;
            double num3 = (num2 * this.totalPxArea) * 0.01;
            double num4 = this.leftWall - this.rightWall;
            double x = -1.0 * (((2.0 * num3) + ((this.leftWall - this.rightWall) * this.overallWidth)) + (2.0 * (this.totalPxArea - num3)));
            double num6 = (2.0 * num3) * this.overallWidth;
            if (num4 != 0.0)
            {
                num7 = (-x - Math.Sqrt(Math.Pow(x, 2.0) - ((4.0 * num4) * num6))) / (2.0 * num4);
            }
            else
            {
                num7 = 0.0;
            }
            return Utils.Round(num7);
        }

        public override int Clicked(int x, int y)
        {
            int num = base.Clicked(x, y);
            if (((num > -1) && (base.FirstVisibleIndex > -1)) && (base.LastVisibleIndex > -1))
            {
                for (int i = base.FirstVisibleIndex; i < base.LastVisibleIndex; i++)
                {
                    if (Graphics3D.PointInPolygon(new Point(x, y), this.IPolyList[i] as Point[]))
                    {
                        return i;
                    }
                }
            }
            return num;
        }

        protected override void Dispose(bool disposing)
        {
            if ((disposing && base.Visible) && (base.GetVertAxis != null))
            {
                base.GetVertAxis.Visible = true;
            }
            base.Dispose(disposing);
        }

        protected internal override void DoBeforeDrawChart()
        {
            base.DoBeforeDrawChart();
            if (base.Visible && (base.GetVertAxis != null))
            {
                base.GetVertAxis.Visible = false;
            }
        }

        public override void Draw()
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            base.Draw();
            if (this.conePercent > 100)
            {
                this.conePercent = 100;
            }
            this.GetBoundingRectangle();
            this.IMin = this.BoundingPoints[0].Y;
            this.IMax = this.BoundingPoints[1].Y;
            this.IDiff = this.IMax - this.IMin;
            this.leftWall = this.BoundingPoints[3].Y - this.BoundingPoints[0].Y;
            this.rightWall = this.BoundingPoints[2].Y - this.BoundingPoints[1].Y;
            this.overallWidth = this.BoundingPoints[1].X - this.BoundingPoints[0].X;
            this.totalPxArea = Utils.Round((double) (this.overallWidth * ((this.leftWall + this.rightWall) * 0.5)));
            this.totalVals = base.YValues.TotalABS;
            this.IPolyList.Clear();
            this.lastX = this.BoundingPoints[0].X;
            this.lastYDisp = 0;
            if (this.overallWidth != 0)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (!base.IsNull(i))
                    {
                        Point[] pointArray = new Point[4];
                        int num = this.CalcSegment(i, base.YValues[i]) + this.BoundingPoints[0].X;
                        int num3 = Utils.Round((double) ((Convert.ToDouble((int) (num - this.BoundingPoints[0].X)) / Convert.ToDouble(this.overallWidth)) * Convert.ToDouble(this.IDiff)));
                        pointArray[0] = new Point(num + (this.separation * i), this.BoundingPoints[3].Y - num3);
                        pointArray[1] = new Point(num + (this.separation * i), this.BoundingPoints[0].Y + num3);
                        pointArray[2] = new Point(this.lastX + (this.separation * i), this.BoundingPoints[0].Y + this.lastYDisp);
                        pointArray[3] = new Point(this.lastX + (this.separation * i), this.BoundingPoints[3].Y - this.lastYDisp);
                        this.IPolyList.Add(pointArray);
                        this.lastYDisp = num3;
                        base.bBrush.Color = this.ValueColor(i);
                        if (this.lineTakesPointColor && this.LinesPen.Visible)
                        {
                            this.LinesPen.Color = this.ValueColor(i);
                        }
                        graphicsd.Brush = base.bBrush;
                        graphicsd.Pen = this.LinesPen;
                        this.setBrush(this.ValueColor(i), ref this.bBrush);
                        if (base.chart.Aspect.View3D)
                        {
                            Rectangle r = new Rectangle(pointArray[2].X, pointArray[2].Y, pointArray[0].X - pointArray[2].X, pointArray[3].Y - pointArray[2].Y);
                            int conePercent = Convert.ToInt32((double) ((((pointArray[0].Y - pointArray[1].Y) * 1f) / ((pointArray[3].Y - pointArray[2].Y) * 1f)) * 100.0));
                            graphicsd.Cone(false, r, base.StartZ, base.EndZ, false, conePercent);
                        }
                        else
                        {
                            graphicsd.Polygon(pointArray);
                        }
                        this.lastX = num;
                    }
                    else if (this.LinesPen.Visible)
                    {
                        graphicsd.Brush.Visible = false;
                        graphicsd.Pen = this.LinesPen;
                        if (base.chart.Aspect.View3D)
                        {
                            graphicsd.Polygon(base.StartZ, this.BoundingPoints);
                        }
                        else
                        {
                            graphicsd.Polygon(this.BoundingPoints);
                        }
                    }
                }
            }
        }

        protected internal override void DrawMark(int valueIndex, string st, SeriesMarks.Position aPosition)
        {
            SeriesMarks.Position position = new SeriesMarks.Position();
            Rectangle chartRect = base.chart.ChartRect;
            Point[] pointArray = this.IPolyList[valueIndex] as Point[];
            position = aPosition;
            if (!aPosition.Custom)
            {
                int num = 5;
                int y = pointArray[2].Y + ((pointArray[1].Y - pointArray[2].Y) / 2);
                int x = (pointArray[2].X - (position.Width / 2)) + ((pointArray[0].X - pointArray[2].X) / 2);
                if ((valueIndex > 0) && ((base.Marks.Positions[valueIndex - 1].LeftTop.X + base.Marks.Positions[valueIndex - 1].Width) > x))
                {
                    y = (base.Marks.Positions[valueIndex - 1].LeftTop.Y + base.Marks.Positions[valueIndex - 1].Height) + num;
                }
                position.LeftTop = new Point(x, y);
            }
            base.DrawMark(valueIndex, st, position);
        }

        public override void DrawValue(int index)
        {
        }

        private void GetBoundingRectangle()
        {
            Rectangle chartRect = base.chart.ChartRect;
            if (base.Marks.Visible)
            {
                chartRect.Y += this.GetMaxMarkHeight();
            }
            this.BoundingPoints[0] = new Point(chartRect.Left + 2, chartRect.Top + 2);
            this.BoundingPoints[1] = new Point(chartRect.Right - 2, Utils.Round((double) (chartRect.Top + ((chartRect.Bottom - chartRect.Top) * ((((double) this.ConePercent) / 2.0) * 0.01)))));
            this.BoundingPoints[2] = new Point(chartRect.Right - 2, Utils.Round((double) (chartRect.Bottom - ((chartRect.Bottom - chartRect.Top) * ((((double) this.ConePercent) / 2.0) * 0.01)))));
            this.BoundingPoints[3] = new Point(chartRect.Left + 2, chartRect.Bottom - 2);
        }

        private int GetMaxMarkHeight()
        {
            int num2 = 0;
            for (int i = base.FirstVisibleIndex; i < base.LastVisibleIndex; i++)
            {
                int num3;
                string markText = this.GetMarkText(i);
                base.chart.MultiLineTextWidth(markText, out num3);
                if (num2 < (base.chart.Graphics3D.FontHeight * num3))
                {
                    num2 = base.chart.Graphics3D.FontHeight * num3;
                }
            }
            return num2;
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            this.LinesPen.Color = color;
        }

        protected internal override int NumSampleValues()
        {
            return 5;
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            this.setBrush(this.ValueColor(valueIndex), ref aBrush);
            this.vBrush = aBrush;
        }

        private void setBrush(Color color, ref ChartBrush aBrush)
        {
            if (aBrush.Gradient.Visible)
            {
                if (!base.chart.aspect.view3D)
                {
                    aBrush.Gradient.Angle = 0.0;
                    aBrush.Gradient.Direction = LinearGradientMode.Vertical;
                }
                aBrush.Gradient.StartColor = color;
                aBrush.Gradient.MiddleColor = Color.White;
                aBrush.Gradient.EndColor = color;
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.linesPen != null)
            {
                this.linesPen.Chart = c;
            }
            if (this.Brush != null)
            {
                this.Brush.Chart = c;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets Brush characteristics for the VolumePipe Series.")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [DefaultValue(30), Description("Sets the Cone percentage.")]
        public int ConePercent
        {
            get
            {
                return this.conePercent;
            }
            set
            {
                this.conePercent = value;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryVolumePipe;
            }
        }

        [Description("Sets LinePen color to each Point Color."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DefaultValue(false), Category("Appearance")]
        public bool LineAsPointColor
        {
            get
            {
                return this.lineTakesPointColor;
            }
            set
            {
                base.SetBooleanProperty(ref this.lineTakesPointColor, value);
            }
        }

        [Description("Sets LinePen characteristics for the VolumePipe Series."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen LinesPen
        {
            get
            {
                if (this.linesPen == null)
                {
                    this.linesPen = new ChartPen(base.Chart, Color.Black, true);
                }
                return this.linesPen;
            }
            set
            {
                this.linesPen = value;
            }
        }

        [Description("Sets the Cone percentage."), DefaultValue(0)]
        public int Separation
        {
            get
            {
                return this.separation;
            }
            set
            {
                base.SetIntegerProperty(ref this.separation, value);
            }
        }
    }
}

