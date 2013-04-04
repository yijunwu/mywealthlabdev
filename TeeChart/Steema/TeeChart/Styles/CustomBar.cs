namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;

    public class CustomBar : Series
    {
        private bool autoMarkPosition;
        protected internal int barSizePercent;
        private BarStyles barStyle;
        protected bool bDark3D;
        protected bool bUseOrigin;
        protected int conePercent;
        protected int customBarSize;
        protected double dOrigin;
        private int FDepthPercent;
        private bool gradientRelative;
        private int[] groups;
        protected Rectangle iBarBounds;
        protected int IBarSize;
        private int IMaxBarPoints;
        protected MultiBars iMultiBar;
        private ChartBrush intBrush;
        private ChartPen intPen;
        protected int INumBars;
        protected int IOrderPos;
        protected int IPreviousCount;
        protected Color NormalBarColor;
        private int numGroups;
        private int offsetPercent;
        protected ChartPen pPen;
        private bool sideMargins;
        private int stackGroup;
        private ChartPen tickLines;

        public event GetBarStyleEventHandler GetBarStyle;

        public CustomBar() : this(null)
        {
        }

        public CustomBar(Chart c) : base(c)
        {
            this.autoMarkPosition = true;
            this.barSizePercent = 70;
            this.bDark3D = true;
            this.iMultiBar = MultiBars.Side;
            this.sideMargins = true;
            this.bUseOrigin = true;
            this.iBarBounds = new Rectangle(0, 0, 0, 0);
            this.groups = new int[100];
            base.Marks.Visible = true;
            base.marks.defaultVisible = true;
            base.marks.Callout.Length = 20;
            base.marks.defaultArrowLength = 20;
            this.FDepthPercent = 50;
            base.mandatory.Name = Texts.ValuesBar;
            this.intBrush = new ChartBrush(c);
            this.intPen = new ChartPen(c);
        }

        protected int ApplyBarOffset(int position)
        {
            int num = position;
            if (this.offsetPercent != 0)
            {
                num += Utils.Round((double) ((this.offsetPercent * this.IBarSize) * 0.01));
            }
            return num;
        }

        public int BarMargin()
        {
            int iBarSize = this.IBarSize;
            if (this.iMultiBar != MultiBars.SideAll)
            {
                iBarSize *= this.INumBars;
            }
            if (!this.sideMargins)
            {
                iBarSize /= 2;
            }
            return iBarSize;
        }

        public void BarRectangle(Color barColor, Rectangle r)
        {
            this.BarRectangle(barColor, r.X, r.Y, r.Right, r.Bottom);
        }

        public void BarRectangle(Color barColor, int aLeft, int aTop, int aRight, int aBottom)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            if (base.bBrush.Solid)
            {
                bool visible;
                Color color;
                if ((aRight == aLeft) || (aTop == aBottom))
                {
                    visible = graphicsd.Pen.Visible;
                    color = graphicsd.Pen.Color;
                    graphicsd.Pen.Color = this.Brush.Color;
                    graphicsd.Pen.Visible = true;
                    graphicsd.Line(aLeft, aTop, aRight, aBottom);
                    graphicsd.Pen.Color = color;
                    graphicsd.Pen.Visible = visible;
                }
                else if (((Math.Abs((int) (aRight - aLeft)) < this.Pen.Width) || (Math.Abs((int) (aBottom - aTop)) < this.Pen.Width)) && this.Pen.Visible)
                {
                    visible = graphicsd.Pen.Visible;
                    color = graphicsd.Brush.Color;
                    graphicsd.Pen.Visible = false;
                    graphicsd.Brush.Color = this.Pen.Color;
                    graphicsd.Rectangle(aLeft, aTop, aRight, aBottom);
                    graphicsd.Pen.Visible = visible;
                    graphicsd.Brush.Color = color;
                }
                else
                {
                    graphicsd.Rectangle(aLeft, aTop, aRight, aBottom);
                }
            }
            else
            {
                graphicsd.Rectangle(aLeft, aTop, aRight, aBottom);
            }
        }

        protected internal override void CalcDepthPositions()
        {
            base.CalcDepthPositions();
            if (this.DepthPercent != 0)
            {
                int num = Utils.Round((double) (((base.EndZ - base.StartZ) * (100 - this.DepthPercent)) * 0.005));
                base.startZ = base.StartZ + num;
                base.endZ = base.EndZ - num;
            }
        }

        protected int CalcMarkLength(int valueIndex)
        {
            if ((base.Count <= 0) || !base.Marks.Visible)
            {
                return 0;
            }
            base.chart.graphics3D.Font = base.Marks.Font;
            int num = base.Marks.ArrowLength + this.InternalCalcMarkLength(valueIndex);
            if (base.Marks.Pen.Visible)
            {
                num += Utils.Round((float) (2 * base.Marks.Pen.Width));
            }
            return num;
        }

        private void CalcRelativeGradient()
        {
            if (this.gradientRelative)
            {
                int top = base.CalcPosValue(base.mandatory.Maximum);
                if (base.chart.Aspect.View3D)
                {
                    top -= base.chart.Aspect.Width3D;
                }
                Rectangle rectangle = Rectangle.FromLTRB(this.iBarBounds.Left, top, this.iBarBounds.Top, this.iBarBounds.Bottom);
                this.Gradient.CustomTargetRectangle = rectangle;
            }
            else
            {
                this.Gradient.CustomTargetRectangle = Rectangle.Empty;
            }
        }

        protected internal override void CalcZOrder()
        {
            if (this.iMultiBar == MultiBars.None)
            {
                base.CalcZOrder();
            }
            else
            {
                int zOrder = -1;
                foreach (Series series in base.chart.Series)
                {
                    if (series.Active)
                    {
                        if (series == this)
                        {
                            break;
                        }
                        if (base.SameClass(series))
                        {
                            zOrder = series.ZOrder;
                            break;
                        }
                    }
                }
                if (zOrder == -1)
                {
                    base.CalcZOrder();
                }
                else
                {
                    base.iZOrder = zOrder;
                }
            }
        }

        public override int Clicked(int x, int y)
        {
            if (base.chart != null)
            {
                base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
            }
            if ((base.firstVisible > -1) && (base.lastVisible > -1))
            {
                Point point = new Point(x, y);
                for (int i = base.firstVisible; i <= Math.Min(base.lastVisible, base.Count - 1); i++)
                {
                    if (this.InternalClicked(i, point))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Pyramid);
            AddSubChart(Texts.Ellipse);
            AddSubChart(Texts.InvPyramid);
            AddSubChart(Texts.Gradient);
            if (this.SubGalleryStack())
            {
                AddSubChart(Texts.Stack);
                AddSubChart(Texts.Stack100);
                AddSubChart(Texts.SelfStack);
            }
            AddSubChart(Texts.Sides);
            AddSubChart(Texts.SideAll);
        }

        protected internal override void DoBeforeDrawChart()
        {
            this.IOrderPos = 1;
            this.IPreviousCount = 0;
            this.INumBars = 0;
            this.IMaxBarPoints = -1;
            this.numGroups = 0;
            if (((base.iColors == null) || (this.Brush.Color == Color.Transparent)) && (this.Brush.Color != base.Color))
            {
                base.Color = this.Brush.Color;
            }
            bool flag = false;
            foreach (Series series in base.chart.Series)
            {
                if (!series.bActive || !base.SameClass(series))
                {
                    continue;
                }
                flag = flag || (series == this);
                int count = series.Count;
                if ((this.IMaxBarPoints == -1) || (count > this.IMaxBarPoints))
                {
                    this.IMaxBarPoints = count;
                }
                switch (this.iMultiBar)
                {
                    case MultiBars.None:
                        this.INumBars = 1;
                        break;

                    case MultiBars.Side:
                    case MultiBars.SideAll:
                        this.INumBars++;
                        if (!flag)
                        {
                            this.IOrderPos++;
                        }
                        break;

                    case MultiBars.Stacked:
                    case MultiBars.Stacked100:
                        if (this.NewGroup(((CustomBar) series).stackGroup))
                        {
                            this.INumBars++;
                            if (!flag)
                            {
                                this.IOrderPos++;
                            }
                        }
                        break;

                    case MultiBars.SelfStack:
                        this.INumBars = 1;
                        break;
                }
                if (!flag)
                {
                    this.IPreviousCount += count;
                }
            }
            for (int i = 0; i < this.numGroups; i++)
            {
                if (this.groups[i] == this.stackGroup)
                {
                    this.IOrderPos = i + 1;
                    break;
                }
            }
            if (this.iMultiBar == MultiBars.SelfStack)
            {
                if (base.chart.iPage.MaxPointsPerPage == 0)
                {
                    this.IMaxBarPoints = this.INumBars;
                }
                else
                {
                    this.IMaxBarPoints = base.chart.Page.MaxPointsPerPage;
                }
            }
            else if (base.chart.iPage.MaxPointsPerPage > 0)
            {
                this.IMaxBarPoints = base.chart.Page.MaxPointsPerPage;
            }
        }

        private void DoCalcBarWidth()
        {
            if (this.customBarSize != 0)
            {
                this.IBarSize = this.customBarSize;
            }
            else if (this.IMaxBarPoints > 0)
            {
                Axis axis = base.yMandatory ? base.GetHorizAxis : base.GetVertAxis;
                if (this.sideMargins)
                {
                    this.IMaxBarPoints++;
                }
                int num = axis.IAxisSize / this.IMaxBarPoints;
                this.IBarSize = Utils.Round((double) ((this.barSizePercent * 0.01) * num)) / Math.Max(1, this.INumBars);
                if ((this.IBarSize % 2) == 1)
                {
                    this.IBarSize++;
                }
            }
            else
            {
                this.IBarSize = 0;
            }
        }

        protected BarStyles DoGetBarStyle(int valueIndex)
        {
            BarStyles barStyle = this.barStyle;
            if (this.GetBarStyle != null)
            {
                GetBarStyleEventArgs e = new GetBarStyleEventArgs(valueIndex, barStyle);
                this.GetBarStyle(this, e);
                barStyle = e.Style;
            }
            return barStyle;
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            if (this.Brush.Image != null)
            {
                g.Brush.Image = base.bBrush.Image;
            }
            base.DrawLegendShape(g, valueIndex, rect);
        }

        protected virtual void DrawTickLine(int tickPos, BarStyles aStyle)
        {
        }

        protected virtual void DrawTickLines(int startPos, int endPos, BarStyles aStyle)
        {
            if (this.TickLines.Visible)
            {
                base.chart.Graphics3D.Pen = this.TickLines;
                Axis mandatoryAxis = base.MandatoryAxis;
                for (int i = 0; i < mandatoryAxis.FAxisDraw.tmpTicks.Length; i++)
                {
                    if ((mandatoryAxis.FAxisDraw.tmpTicks[i] > startPos) && (mandatoryAxis.FAxisDraw.tmpTicks[i] < endPos))
                    {
                        this.DrawTickLine(mandatoryAxis.FAxisDraw.tmpTicks[i], aStyle);
                    }
                }
            }
        }

        protected void InternalApplyBarMargin(ref int marginA, ref int marginB)
        {
            this.DoCalcBarWidth();
            int num = this.BarMargin();
            marginA += num;
            marginB += num;
        }

        protected virtual int InternalCalcMarkLength(int valueIndex)
        {
            return 0;
        }

        protected virtual bool InternalClicked(int valueIndex, Point point)
        {
            return false;
        }

        protected int InternalGetOriginPos(int valueIndex, int defaultOrigin)
        {
            double num2 = this.PointOrigin(valueIndex, false);
            switch (this.iMultiBar)
            {
                case MultiBars.Stacked:
                case MultiBars.SelfStack:
                    return base.CalcPosValue(num2);

                case MultiBars.Stacked100:
                {
                    double num3 = this.PointOrigin(valueIndex, true);
                    return ((num3 != 0.0) ? base.CalcPosValue((num2 * 100.0) / num3) : 0);
                }
            }
            return (this.bUseOrigin ? base.CalcPosValue(num2) : defaultOrigin);
        }

        private double InternalPointOrigin(int valueIndex, bool sumAll)
        {
            double num = 0.0;
            double num2 = base.mandatory[valueIndex];
            double num1 = base.notMandatory[valueIndex];
            if (base.chart != null)
            {
                foreach (Series series in base.chart.Series)
                {
                    if (!sumAll && (series == this))
                    {
                        return num;
                    }
                    if ((series.Active && base.SameClass(series)) && ((series.Count > valueIndex) && (((CustomBar) series).stackGroup == this.stackGroup)))
                    {
                        double originValue = series.GetOriginValue(valueIndex);
                        double num4 = series.notMandatory[valueIndex];
                        if (num2 < 0.0)
                        {
                            if (originValue < 0.0)
                            {
                                num += originValue;
                            }
                        }
                        else if (originValue > 0.0)
                        {
                            num += originValue;
                        }
                    }
                }
            }
            return num;
        }

        protected double MaxMandatoryValue(double value)
        {
            if (this.iMultiBar == MultiBars.Stacked100)
            {
                return 100.0;
            }
            double total = value;
            if (this.iMultiBar == MultiBars.SelfStack)
            {
                total = base.mandatory.Total;
            }
            else if (this.iMultiBar == MultiBars.Stacked)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    double num3 = this.PointOrigin(i, false) + base.mandatory[i];
                    if (num3 > total)
                    {
                        total = num3;
                    }
                }
            }
            if (this.bUseOrigin && (total < this.dOrigin))
            {
                total = this.dOrigin;
            }
            return total;
        }

        protected double MinMandatoryValue(double value)
        {
            if (this.iMultiBar == MultiBars.Stacked100)
            {
                return 0.0;
            }
            double dOrigin = value;
            if ((this.iMultiBar == MultiBars.Stacked) || (this.iMultiBar == MultiBars.SelfStack))
            {
                for (int i = 0; i < base.Count; i++)
                {
                    double num3 = this.PointOrigin(i, false) + base.mandatory[i];
                    if (num3 < dOrigin)
                    {
                        dOrigin = num3;
                    }
                }
            }
            if (this.bUseOrigin && (dOrigin > this.dOrigin))
            {
                dOrigin = this.dOrigin;
            }
            return dOrigin;
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            this.SetPenBar(color, true);
        }

        private bool NewGroup(int AGroup)
        {
            for (int i = 0; i < this.numGroups; i++)
            {
                if (this.groups[i] == AGroup)
                {
                    return false;
                }
            }
            this.groups[this.numGroups] = AGroup;
            this.numGroups++;
            return true;
        }

        protected internal override int NumSampleValues()
        {
            if ((base.chart != null) && (base.chart.Series.Count > 1))
            {
                foreach (Series series in base.chart.Series)
                {
                    if (((series != this) && (series is CustomBar)) && (series.Count > 0))
                    {
                        return series.Count;
                    }
                }
            }
            return 6;
        }

        public virtual double PointOrigin(int valueIndex, bool sumAll)
        {
            if ((this.iMultiBar == MultiBars.Stacked) || (this.iMultiBar == MultiBars.Stacked100))
            {
                return this.InternalPointOrigin(valueIndex, sumAll);
            }
            if (this.iMultiBar != MultiBars.SelfStack)
            {
                return this.dOrigin;
            }
            double num = 0.0;
            for (int i = 0; i < valueIndex; i++)
            {
                num += base.mandatory[i];
            }
            return num;
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.barSizePercent = 0x55;
            this.MultiBar = MultiBars.None;
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            if (valueIndex > -1)
            {
                this.SetPenBar(this.ValueColor(valueIndex));
            }
            backColor = Utils.ColorIsEmpty(aBrush.Color) ? base.chart.Panel.Color : aBrush.Color;
        }

        protected internal void SetBarSizePercent(int value)
        {
            base.SetIntegerProperty(ref this.barSizePercent, value);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pPen != null)
            {
                this.pPen.Chart = base.chart;
            }
            if (base.bBrush != null)
            {
                base.bBrush.Chart = base.chart;
            }
            if (this.tickLines != null)
            {
                this.tickLines.Chart = base.chart;
            }
            this.SetOtherBars(false);
        }

        protected void SetDefaultCylinderGradient(Color barColor, LinearGradientMode gradientDirection)
        {
            if (this.BarStyle == BarStyles.Cylinder)
            {
                this.intBrush.Color = barColor;
                this.intBrush.Gradient.Visible = this.Gradient.Visible;
                this.intBrush.Gradient.StartColor = barColor;
                this.intBrush.Gradient.MiddleColor = Utils.CalcColorBlend(barColor, Color.White, 60);
                this.intBrush.Gradient.EndColor = Utils.CalcColorBlend(barColor, Color.Black, 60);
                this.intBrush.Gradient.Direction = gradientDirection;
                base.chart.graphics3D.Brush = this.intBrush;
            }
        }

        private void SetOtherBars(bool SetOthers)
        {
            if (base.chart != null)
            {
                foreach (Series series in base.chart.Series)
                {
                    if (base.SameClass(series))
                    {
                        CustomBar bar = (CustomBar) series;
                        if (SetOthers)
                        {
                            bar.iMultiBar = this.iMultiBar;
                            bar.sideMargins = this.sideMargins;
                        }
                        else
                        {
                            this.iMultiBar = bar.iMultiBar;
                            this.sideMargins = bar.sideMargins;
                            break;
                        }
                        bar.calcVisiblePoints = this.iMultiBar != MultiBars.SelfStack;
                    }
                }
            }
        }

        private void SetPenBar(Color color)
        {
            this.SetPenBar(color, false);
        }

        private void SetPenBar(Color color, bool darken)
        {
            if (base.ColorEach)
            {
                this.intPen = this.Pen.Clone() as ChartPen;
                this.intPen.Color = Utils.DarkenColor(color, 60);
                if (base.chart != null)
                {
                    base.chart.graphics3D.Pen = this.intPen;
                }
            }
            else
            {
                if (darken)
                {
                    this.Pen.Color = Utils.DarkenColor(color, 60);
                }
                if (base.chart != null)
                {
                    base.chart.graphics3D.Pen = this.Pen;
                }
            }
        }

        protected void SetPenBrushBar(Color barColor)
        {
            this.SetPenBar(barColor);
            if (base.ColorEach)
            {
                this.intBrush = this.Brush.Clone() as ChartBrush;
                this.intBrush.Color = barColor;
                if (this.intBrush.Gradient.Visible)
                {
                    this.intBrush.Gradient.StartColor = barColor;
                    this.intBrush.Gradient.MiddleColor = Utils.EmptyColor;
                    this.intBrush.Gradient.EndColor = Utils.CalcColorBlend(barColor, Color.White, 60);
                }
                base.chart.graphics3D.Brush = this.intBrush;
            }
            else
            {
                this.Brush.Color = barColor;
                base.chart.graphics3D.Brush = this.Brush;
            }
            if (this.Gradient.Visible)
            {
                this.CalcRelativeGradient();
            }
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 0:
                    break;

                case 1:
                    base.ColorEach = true;
                    return;

                case 2:
                    this.BarStyle = BarStyles.Pyramid;
                    return;

                case 3:
                    this.BarStyle = BarStyles.Ellipse;
                    return;

                case 4:
                    this.BarStyle = BarStyles.InvPyramid;
                    return;

                case 5:
                    this.BarStyle = BarStyles.RectGradient;
                    return;

                default:
                    if ((base.Chart != null) && (base.Chart.Series.Count == 1))
                    {
                        this.FillSampleValues(2);
                        Series series = base.Chart.Series.Add(base.GetType());
                        series.Title = "";
                        series.FillSampleValues(2);
                        series.Marks.Visible = false;
                        (series as CustomBar).barSizePercent = this.barSizePercent;
                        base.Marks.Visible = false;
                        series.SetSubGallery(index);
                    }
                    if (!this.SubGalleryStack())
                    {
                        index += 3;
                    }
                    switch (index)
                    {
                        case 6:
                            this.MultiBar = MultiBars.Stacked;
                            return;

                        case 7:
                            this.MultiBar = MultiBars.Stacked100;
                            return;

                        case 8:
                            this.MultiBar = MultiBars.SelfStack;
                            return;

                        case 9:
                            this.MultiBar = MultiBars.Side;
                            return;

                        case 10:
                            this.MultiBar = MultiBars.SideAll;
                            return;
                    }
                    base.SetSubGallery(index);
                    break;
            }
        }

        protected virtual bool ShouldSerializeYOrigin()
        {
            return false;
        }

        protected virtual bool SubGalleryStack()
        {
            return true;
        }

        [Description("Repositions Marks on BarSeries to prevent overlapping."), DefaultValue(true)]
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

        [Browsable(false), Description("Coordinates of current Bar point being displayed.")]
        public Rectangle BarBounds
        {
            get
            {
                return this.iBarBounds;
            }
        }

        protected int BarBoundsMidX
        {
            get
            {
                return ((this.iBarBounds.Left + this.iBarBounds.Right) / 2);
            }
        }

        [DefaultValue(0), Description("Sets the Bar shape used to draw Bars."), Category("Appearance")]
        public BarStyles BarStyle
        {
            get
            {
                return this.barStyle;
            }
            set
            {
                if (this.barStyle != value)
                {
                    this.barStyle = value;
                    if ((this.barStyle == BarStyles.RectGradient) || (this.BarStyle == BarStyles.Cylinder))
                    {
                        this.Gradient.Visible = true;
                    }
                    else
                    {
                        this.Gradient.Visible = false;
                    }
                    this.Invalidate();
                }
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines the Brush used to fill Bars.")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CustomBarWidth
        {
            get
            {
                return this.customBarSize;
            }
            set
            {
                this.customBarSize = value;
                base.chart.Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(true), Description("Darkens sides of bars to enhance 3D effect.")]
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

        [DefaultValue(50)]
        public int DepthPercent
        {
            get
            {
                return this.FDepthPercent;
            }
            set
            {
                base.SetIntegerProperty(ref this.FDepthPercent, value);
            }
        }

        [Description("Defines the color gradient to fill the bars."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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

        [DefaultValue(false), Description("When Gradient is Visible, calculates Colors based on highest bar."), Category("Appearance")]
        public bool GradientRelative
        {
            get
            {
                return this.gradientRelative;
            }
            set
            {
                base.SetBooleanProperty(ref this.gradientRelative, value);
            }
        }

        [DefaultValue(1), Description("Determines position of multiple BarSeries in the same Chart.")]
        public MultiBars MultiBar
        {
            get
            {
                return this.iMultiBar;
            }
            set
            {
                if (this.iMultiBar != value)
                {
                    this.iMultiBar = value;
                    this.SetOtherBars(true);
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("Sets the Bar displacement as percent of Bar size.")]
        public int OffsetPercent
        {
            get
            {
                return this.offsetPercent;
            }
            set
            {
                base.SetIntegerProperty(ref this.offsetPercent, value);
            }
        }

        [DefaultValue((double) 0.0), Description("Sets the common bottom value used for all Bar points.")]
        public double Origin
        {
            get
            {
                return this.dOrigin;
            }
            set
            {
                base.SetDoubleProperty(ref this.dOrigin, value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Pen used to draw the Bar rectangles.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pPen == null)
                {
                    this.pPen = new ChartPen(base.chart, true);
                }
                return this.pPen;
            }
        }

        [Description("Sets a margin between Chart rectangle and Bars."), DefaultValue(true)]
        public bool SideMargins
        {
            get
            {
                return this.sideMargins;
            }
            set
            {
                base.SetBooleanProperty(ref this.sideMargins, value);
                this.SetOtherBars(true);
            }
        }

        [Description("Allows stacking independent Series within the same Chart."), DefaultValue(0)]
        public int StackGroup
        {
            get
            {
                return this.stackGroup;
            }
            set
            {
                base.SetIntegerProperty(ref this.stackGroup, value);
            }
        }

        [Category("Appearance"), DefaultValue((string) null), Description("Pen used to draw tick lines on the bars."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen TickLines
        {
            get
            {
                if (this.tickLines == null)
                {
                    this.tickLines = new ChartPen(base.chart, Color.Black, false);
                }
                return this.tickLines;
            }
        }

        [Description(""), DefaultValue(true)]
        public bool UseOrigin
        {
            get
            {
                return this.bUseOrigin;
            }
            set
            {
                base.SetBooleanProperty(ref this.bUseOrigin, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), Obsolete("Please use Origin property.")]
        public double YOrigin
        {
            get
            {
                return this.dOrigin;
            }
            set
            {
                this.Origin = value;
            }
        }

        public class GetBarStyleEventArgs : EventArgs
        {
            private BarStyles style;
            private readonly int valueIndex;

            public GetBarStyleEventArgs(int valueIndex, BarStyles style)
            {
                this.valueIndex = valueIndex;
                this.style = style;
            }

            public BarStyles Style
            {
                get
                {
                    return this.style;
                }
                set
                {
                    this.style = value;
                }
            }

            public int ValueIndex
            {
                get
                {
                    return this.valueIndex;
                }
            }
        }

        public delegate void GetBarStyleEventHandler(CustomBar sender, CustomBar.GetBarStyleEventArgs e);
    }
}

