namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;

    [ToolboxBitmap(typeof(FibonacciTool), "ToolsIcons.FibonacciTool.bmp")]
    public class FibonacciTool : ToolSeries
    {
        private double[] defaultfab;
        private FibonacciStyle drawstyle;
        private double endx;
        private double endy;
        private int labelsangle;
        private ChartFont lblfnt;
        private FibonacciLevels levels;
        private bool showlabels;
        private Point sp;
        private double startx;
        private double starty;
        private ChartPen trendpen;

        public FibonacciTool() : this(null)
        {
        }

        public FibonacciTool(Chart c) : base(c)
        {
            this.defaultfab = new double[] { 38.2, 50.0, 61.8 };
            this.levels = new FibonacciLevels();
            this.sp = new Point(-1, -1);
            this.showlabels = true;
            this.labelsangle = 90;
            this.levels.tool = this;
            this.CreateDefaultLevels();
        }

        private void Arc(Graphics3D g, Point center, int rad, bool upper)
        {
            int num = center.X - rad;
            int num2 = center.X + rad;
            int num3 = center.Y - rad;
            int num4 = center.Y + rad;
            if (upper)
            {
                g.Arc(num, num3, num2, num4, 0f, 180f);
            }
            else
            {
                g.Arc(num, num3, num2, num4, 180f, 180f);
            }
        }

        protected override void Assign(Tool t)
        {
            base.Assign(t);
            FibonacciTool tool = t as FibonacciTool;
            tool.StartX = this.StartX;
            tool.StartY = this.StartY;
            tool.EndX = this.EndX;
            tool.EndY = this.EndY;
            tool.DrawStyle = this.DrawStyle;
            tool.ShowLabels = this.showlabels;
            tool.LabelsAngle = this.labelsangle;
        }

        public Point AxisPoint(double x, double y)
        {
            return new Point(base.GetHorizAxis.CalcPosValue(x), base.GetVertAxis.CalcPosValue(y));
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            this.ClipDrawingRegion();
            if (e is AfterDrawEventArgs)
            {
                if (this.TrendPen.Visible)
                {
                    Graphics3D graphicsd = base.Chart.Graphics3D;
                    if ((base.Chart != null) && graphicsd.ValidState())
                    {
                        graphicsd.Pen = this.TrendPen;
                        graphicsd.Line(this.AxisPoint(this.startx, this.starty), this.AxisPoint(this.endx, this.endy));
                    }
                }
                if ((this.levels.Count > 0) && (this.AxisPoint(this.startx, this.starty) != this.AxisPoint(this.endx, this.endy)))
                {
                    switch (this.drawstyle)
                    {
                        case FibonacciStyle.Arc:
                            this.sp = this.AxisPoint(this.endx, this.endy);
                            break;

                        case FibonacciStyle.Fan:
                            this.sp = this.AxisPoint(this.startx, this.starty);
                            break;
                    }
                    foreach (FibonacciItem item in this.levels)
                    {
                        this.DrawLevel(item);
                    }
                }
            }
            base.Chart.Graphics3D.ClearClipRegions();
        }

        protected void ClipDrawingRegion()
        {
            Rectangle chartRect = new Rectangle();
            if (base.iSeries != null)
            {
                chartRect = new Rectangle {
                    X = base.GetHorizAxis.IStartPos,
                    Y = base.GetVertAxis.IStartPos,
                    Width = base.GetHorizAxis.IEndPos - chartRect.X,
                    Height = base.GetVertAxis.IEndPos - chartRect.Y
                };
            }
            else
            {
                chartRect = base.Chart.ChartRect;
            }
            if (base.Chart.CanClip())
            {
                base.Chart.Graphics3D.ClipCube(chartRect, 0, base.Chart.Aspect.Width3D);
            }
        }

        public void CreateDefaultLevels()
        {
            this.levels.Clear();
            for (int i = this.defaultfab.GetLowerBound(0); i <= this.defaultfab.GetUpperBound(0); i++)
            {
                FibonacciItem l = new FibonacciItem(this, this.defaultfab[i]);
                this.levels.Add(l);
            }
        }

        private void DrawLevel(FibonacciItem lvl)
        {
            Graphics3D g = base.Chart.Graphics3D;
            if ((base.Chart != null) && g.ValidState())
            {
                g.Pen = lvl.Pen;
                switch (this.drawstyle)
                {
                    case FibonacciStyle.Arc:
                    {
                        int rad = (int) Math.Round((double) ((this.Radius() * lvl.Value) / 100.0));
                        this.Arc(g, this.sp, rad, this.endy > this.starty);
                        if (!this.showlabels)
                        {
                            break;
                        }
                        g.Font = this.LabelsFont;
                        g.RotateLabel(this.sp.X - rad, this.sp.Y, lvl.Value.ToString("0.0"), (double) this.labelsangle);
                        return;
                    }
                    case FibonacciStyle.Fan:
                    {
                        double x = (base.iSeries != null) ? base.Series.XValues[base.iSeries.Count - 1] : this.endx;
                        double num3 = (((this.endy - this.starty) / (this.endx - this.startx)) * (100.0 - lvl.Value)) / 100.0;
                        double y = this.starty + (num3 * (x - this.startx));
                        Point endp = this.AxisPoint(x, y);
                        this.Fan(g, this.sp, endp);
                        if (this.showlabels)
                        {
                            g.Font = this.LabelsFont;
                            g.RotateLabel(endp.X, endp.Y, lvl.Value.ToString("0.0"), (double) this.labelsangle);
                        }
                        break;
                    }
                    default:
                        return;
                }
            }
        }

        private void Fan(Graphics3D g, Point startp, Point endp)
        {
            g.MoveTo(startp);
            g.LineTo(endp.X, endp.Y);
        }

        private int Radius()
        {
            int num = base.GetHorizAxis.CalcSizeValue(Math.Abs((double) (this.endx - this.startx)));
            int num2 = base.GetVertAxis.CalcSizeValue(Math.Abs((double) (this.endy - this.starty)));
            return (int) Math.Round(Math.Sqrt(Math.Pow((double) num, 2.0) + Math.Pow((double) num2, 2.0)));
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.FibonacciTool;
            }
        }

        [DefaultValue(0)]
        public FibonacciStyle DrawStyle
        {
            get
            {
                return this.drawstyle;
            }
            set
            {
                this.drawstyle = value;
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Description("Trendline end point x coordinate")]
        public double EndX
        {
            get
            {
                return this.endx;
            }
            set
            {
                this.endx = value;
                this.Invalidate();
            }
        }

        [Description("Trendline end point y coordinate"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double EndY
        {
            get
            {
                return this.endy;
            }
            set
            {
                this.endy = value;
                this.Invalidate();
            }
        }

        [DefaultValue(90)]
        public int LabelsAngle
        {
            get
            {
                return this.labelsangle;
            }
            set
            {
                base.SetIntegerProperty(ref this.labelsangle, value);
            }
        }

        public ChartFont LabelsFont
        {
            get
            {
                if (this.lblfnt == null)
                {
                    this.lblfnt = new ChartFont(base.Chart);
                }
                return this.lblfnt;
            }
        }

        [Description("Returns the list of Fibonacci levels."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FibonacciLevels Levels
        {
            get
            {
                return this.levels;
            }
        }

        [DefaultValue(true)]
        public bool ShowLabels
        {
            get
            {
                return this.showlabels;
            }
            set
            {
                base.SetBooleanProperty(ref this.showlabels, value);
            }
        }

        [Description("Trendline start point x coordinate"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double StartX
        {
            get
            {
                return this.startx;
            }
            set
            {
                this.startx = value;
                this.Invalidate();
            }
        }

        [Description("Trendline start point y coordinate"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double StartY
        {
            get
            {
                return this.starty;
            }
            set
            {
                this.starty = value;
                this.Invalidate();
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.FibonacciToolDesc;
            }
        }

        [Description("Trend pen characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen TrendPen
        {
            get
            {
                if (this.trendpen == null)
                {
                    this.trendpen = new ChartPen(base.Chart, Color.Red);
                }
                return this.trendpen;
            }
        }

        public class FibonacciItem
        {
            private ChartPen pen;
            private FibonacciTool tool;
            private double val;

            public FibonacciItem(FibonacciTool owner, double value)
            {
                this.val = value;
                this.tool = owner;
                this.tool.Levels.Add(this);
            }

            [Description("Fibonacci level pen properties.")]
            public ChartPen Pen
            {
                get
                {
                    if (this.pen == null)
                    {
                        this.pen = new ChartPen(this.tool.Chart, Color.Black);
                    }
                    return this.pen;
                }
            }

            public double Value
            {
                get
                {
                    return this.val;
                }
                set
                {
                    this.val = value;
                }
            }
        }

        public class FibonacciLevels : CollectionBase
        {
            internal FibonacciTool tool;

            public int Add(FibonacciTool.FibonacciItem l)
            {
                int index = base.List.IndexOf(l);
                if (index != -1)
                {
                    return index;
                }
                return base.List.Add(l);
            }

            public void Clear()
            {
                base.Clear();
                this.tool.Invalidate();
            }

            public int IndexOf(FibonacciTool.FibonacciItem l)
            {
                return base.List.IndexOf(l);
            }

            public void Remove(FibonacciTool.FibonacciItem s)
            {
                base.List.Remove(s);
                this.tool.Invalidate();
            }

            public FibonacciTool.FibonacciItem this[int index]
            {
                get
                {
                    return (FibonacciTool.FibonacciItem) base.List[index];
                }
            }
        }
    }
}

