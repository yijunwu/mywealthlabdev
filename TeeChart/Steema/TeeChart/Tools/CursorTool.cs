namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(CursorTool), "ToolsIcons.CursorTool.bmp"), Description("Displays draggable Cursor lines on top of Series.")]
    public class CursorTool : ToolSeries
    {
        private int cursorClickTolerance;
        private CursorClicked dragging;
        private Point end;
        private bool fastCursor;
        private bool followMouse;
        private int horizSize;
        private int IOldSnap;
        private Point IPoint;
        private double IXValue;
        private double IYValue;
        private Cursor originalCursor;
        private int penXOR;
        private int scopeSize;
        private ScopeCursorStyle scopeStyle;
        private bool snap;
        private Steema.TeeChart.Tools.SnapStyle snapStyle;
        private Point start;
        private CursorToolStyles style;
        private Point top;
        private bool useChartRect;
        private bool useSeriesZ;
        private int vertSize;

        public event CursorChangeEventHandler Change;

        public event CursorGetAxisRectEventHandler GetAxisRect;

        public event CursorChangeEventHandler SnapChange;

        public CursorTool() : this(null)
        {
        }

        public CursorTool(Chart c) : base(c)
        {
            this.style = CursorToolStyles.Both;
            this.scopeSize = 4;
            this.cursorClickTolerance = 3;
            this.IPoint = new Point(-1, -1);
            this.start = new Point(0, 0);
            this.end = new Point(0, 0);
            this.top = new Point(0, 0);
            this.IOldSnap = -1;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            CursorTool tool = t as CursorTool;
            tool.FollowMouse = this.FollowMouse;
            tool.Snap = this.Snap;
            tool.Style = this.Style;
            tool.FastCursor = this.FastCursor;
            tool.UseSeriesZ = this.UseSeriesZ;
            tool.Pen = this.Pen.Clone() as ChartPen;
        }

        protected void CalcScreenPositions()
        {
            if ((this.IPoint.X == -1) || (this.IPoint.Y == -1))
            {
                this.IPoint.X = (base.GetHorizAxis.IStartPos + base.GetHorizAxis.IEndPos) / 2;
                this.IPoint.Y = (base.GetVertAxis.IStartPos + base.GetVertAxis.IEndPos) / 2;
                this.CalcValuePositions(this.IPoint.X, this.IPoint.Y);
                int snapPoint = this.SnapToPoint();
                this.CalcScreenPositions();
                this.OnChange(snapPoint);
            }
            else if ((base.Series != null) && !base.Series.UseAxis)
            {
                this.IPoint = base.Series.ValuePointToScreenPoint(this.IXValue, this.IYValue);
            }
            else
            {
                this.IPoint.X = base.GetHorizAxis.CalcPosValue(this.IXValue);
                this.IPoint.Y = base.GetVertAxis.CalcPosValue(this.IYValue);
            }
        }

        private void CalcValuePositions(int x, int y)
        {
            if (this.UseSeriesZ)
            {
                base.Chart.Graphics3D.Calculate2DPosition(ref x, ref y, this.Z());
            }
            if ((base.Series != null) && !base.Series.UseAxis)
            {
                PointDouble num = base.Series.ScreenPointToValuePoint(x, y);
                switch (this.dragging)
                {
                    case CursorClicked.Horizontal:
                        this.IYValue = num.Y;
                        return;

                    case CursorClicked.Vertical:
                        this.IXValue = num.X;
                        return;
                }
                this.IXValue = num.X;
                this.IYValue = num.Y;
            }
            else
            {
                switch (this.dragging)
                {
                    case CursorClicked.Horizontal:
                        this.IYValue = base.GetVertAxis.CalcPosPoint(y);
                        return;

                    case CursorClicked.Vertical:
                        this.IXValue = base.GetHorizAxis.CalcPosPoint(x);
                        return;
                }
                this.IXValue = base.GetHorizAxis.CalcPosPoint(x);
                this.IYValue = base.GetVertAxis.CalcPosPoint(y);
            }
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (!this.FastCursor && (e is AfterDrawEventArgs))
            {
                this.CalcScreenPositions();
                this.RedrawCursor();
            }
        }

        public CursorClicked Clicked(int x, int y)
        {
            CursorClicked none = CursorClicked.None;
            Point p = new Point(0, 0);
            if (this.InMouseRectangle(x, y, true))
            {
                p.X = this.IPoint.X;
                p.Y = this.IPoint.Y;
                if (this.UseSeriesZ)
                {
                    p = base.Chart.Graphics3D.Calc3DPoint(p, this.Z());
                }
                if (((this.style == CursorToolStyles.Both) && (Math.Abs((int) (y - p.Y)) <= this.CursorClickTolerance)) && (Math.Abs((int) (x - p.X)) <= this.CursorClickTolerance))
                {
                    return CursorClicked.Both;
                }
                if (((this.style == CursorToolStyles.Horizontal) || (this.style == CursorToolStyles.Both)) && (Math.Abs((int) (y - p.Y)) <= this.CursorClickTolerance))
                {
                    return CursorClicked.Horizontal;
                }
                if ((this.style != CursorToolStyles.Vertical) && (this.style != CursorToolStyles.Both))
                {
                    return none;
                }
                if (Math.Abs((int) (x - p.X)) <= this.CursorClickTolerance)
                {
                    none = CursorClicked.Vertical;
                }
            }
            return none;
        }

        private void DoChange()
        {
            if (this.FastCursor)
            {
                this.RedrawCursor();
            }
            this.CalcScreenPositions();
            this.OnChange(-1);
            if (this.FastCursor)
            {
                this.RedrawCursor();
            }
            else
            {
                this.Invalidate();
            }
        }

        protected void DoGetAxisRect(ref Rectangle tmpResult)
        {
            if (this.GetAxisRect != null)
            {
                CursorGetAxisRectEventArgs e = new CursorGetAxisRectEventArgs(tmpResult);
                this.GetAxisRect(this, e);
                tmpResult = e.Rectangle;
            }
        }

        private void DrawCursorLines(bool Draw3D, Rectangle R, int X, int Y)
        {
            switch (this.style)
            {
                case CursorToolStyles.Horizontal:
                    if (Y >= 0)
                    {
                        this.DrawHorizontal(R, Y);
                    }
                    break;

                case CursorToolStyles.Vertical:
                    if (X >= 0)
                    {
                        this.DrawVertical(R, X);
                    }
                    break;

                case CursorToolStyles.Scope:
                    if ((X >= 0) && (Y >= 0))
                    {
                        this.DrawScope(R, X, Y);
                    }
                    if (X >= 0)
                    {
                        this.DrawHorizontal(R, Y);
                    }
                    if (Y >= 0)
                    {
                        this.DrawVertical(R, X);
                    }
                    break;

                case CursorToolStyles.ScopeOnly:
                    if ((X >= 0) && (Y >= 0))
                    {
                        this.DrawScope(R, X, Y);
                    }
                    break;

                default:
                    if (Y >= 0)
                    {
                        this.DrawHorizontal(R, Y);
                    }
                    if (X >= 0)
                    {
                        this.DrawVertical(R, X);
                    }
                    break;
            }
            base.chart.redrawing = false;
        }

        private void DrawHorizontal(Rectangle r, int Y)
        {
            this.HorizontalLine(r.X, r.Right, Y, this.Z());
        }

        private void DrawScope(Rectangle r, int X, int Y)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            Rectangle rectangle = Utils.FromLTRB(X - this.ScopeSize, Y - this.ScopeSize, X + this.ScopeSize, Y + this.ScopeSize);
            if (this.ScopeStyle == ScopeCursorStyle.Camera)
            {
                this.InternalRectangle(Utils.FromLTRB((X - this.HorizSize) - this.ScopeSize, (Y - this.VertSize) - this.ScopeSize, (X + this.HorizSize) + this.ScopeSize, (Y + this.VertSize) + this.ScopeSize), this.Z());
                this.HorizontalLine(X - this.ScopeSize, X + this.ScopeSize, Y, this.Z());
                this.VerticalLine(X, Y - this.ScopeSize, Y + this.ScopeSize, this.Z());
            }
            else
            {
                this.HorizontalLine((X - this.HorizSize) - this.ScopeSize, X - this.ScopeSize, Y, this.Z());
                this.HorizontalLine(X + this.ScopeSize, (X + this.HorizSize) + this.ScopeSize, Y, this.Z());
                this.VerticalLine(X, (Y - this.VertSize) - this.ScopeSize, Y - this.ScopeSize, this.Z());
                this.VerticalLine(X, Y + this.ScopeSize, (Y + this.VertSize) + this.ScopeSize, this.Z());
                switch (this.ScopeStyle)
                {
                    case ScopeCursorStyle.Rectangle:
                        this.InternalRectangle(rectangle, this.Z());
                        return;

                    case ScopeCursorStyle.Circle:
                        if (this.FastCursor)
                        {
                            break;
                        }
                        graphicsd.Ellipse(rectangle, this.Z());
                        return;

                    case ScopeCursorStyle.Diamond:
                        if (!this.FastCursor)
                        {
                            Point[] p = new Point[] { new Point(X - this.ScopeSize, Y), new Point(X, Y - this.ScopeSize), new Point(X + this.ScopeSize, Y), new Point(X, Y + this.ScopeSize) };
                            graphicsd.Polygon(this.Z(), p);
                        }
                        break;

                    default:
                        return;
                }
            }
        }

        private void DrawVertical(Rectangle r, int X)
        {
            this.VerticalLine(X, r.Y, r.Bottom, this.Z());
        }

        private void HorizontalLine(int left, int right, int y, int z)
        {
            if (this.FastCursor)
            {
                if (y > 0)
                {
                    this.start.X = left;
                    this.start.Y = y;
                    this.end.X = right;
                    this.end.Y = y;
                    if (z > 0)
                    {
                        this.start = base.chart.graphics3D.Calc3DPoint(this.start, z);
                        this.end = base.chart.graphics3D.Calc3DPoint(this.end, z);
                    }
                    this.start = base.chart.Parent.PointToScreen(this.start);
                    this.end = base.chart.Parent.PointToScreen(this.end);
                    if (!base.chart.redrawing)
                    {
                        ControlPaint.DrawReversibleLine(this.start, this.end, Color.FromArgb(this.penXOR));
                    }
                }
            }
            else if (z > 0)
            {
                base.chart.graphics3D.HorizontalLine(left, right, y, z);
            }
            else
            {
                base.chart.graphics3D.HorizontalLine(left, right, y);
            }
        }

        private bool InMouseRectangle(int x, int y)
        {
            return this.InMouseRectangle(x, y, false);
        }

        private bool InMouseRectangle(int x, int y, bool UseSize)
        {
            if (this.UseSeriesZ)
            {
                base.Chart.Graphics3D.Calculate2DPosition(ref x, ref y, this.Z());
            }
            return this.MouseRectangle(x, y, UseSize).Contains(x, y);
        }

        private Rectangle InternalGetAxisRect()
        {
            Rectangle chartRect;
            if (this.useChartRect)
            {
                chartRect = base.chart.ChartRect;
            }
            else
            {
                chartRect = Utils.FromLTRB(base.GetHorizAxis.IStartPos, base.GetVertAxis.IStartPos, base.GetHorizAxis.IEndPos, base.GetVertAxis.IEndPos);
            }
            this.DoGetAxisRect(ref chartRect);
            return chartRect;
        }

        private void InternalRectangle(Rectangle r, int z)
        {
            if (this.FastCursor)
            {
                this.top.X = r.X;
                this.top.Y = r.Y;
                if ((this.top.X > 0) && (this.top.Y > 0))
                {
                    if (z > 0)
                    {
                        this.top = base.chart.graphics3D.Calc3DPoint(this.top, z);
                    }
                    this.top = base.chart.Parent.PointToScreen(this.top);
                    if (!base.chart.redrawing)
                    {
                        ControlPaint.DrawReversibleFrame(new Rectangle(this.top, r.Size), Color.Transparent, FrameStyle.Thick);
                    }
                }
            }
            else
            {
                base.chart.graphics3D.Rectangle(r, z);
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (kind == MouseEventKinds.Up)
            {
                this.dragging = CursorClicked.None;
            }
            else if (kind == MouseEventKinds.Move)
            {
                this.MouseMove(e, ref c);
            }
            else if ((kind == MouseEventKinds.Down) && !this.followMouse)
            {
                Point point = new Point(e.X, e.Y);
                this.dragging = this.Clicked(point.X, point.Y);
                if (this.dragging != CursorClicked.None)
                {
                    base.chart.CancelMouse = true;
                }
            }
        }

        private void MouseMove(MouseEventArgs e, ref Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            if (((this.dragging != CursorClicked.None) || this.followMouse) && this.InMouseRectangle(point.X, point.Y))
            {
                double iXValue = this.IXValue;
                double iYValue = this.IYValue;
                if (this.FastCursor)
                {
                    this.RedrawCursor();
                }
                this.CalcValuePositions(point.X, point.Y);
                int snapPoint = this.SnapToPoint();
                if (this.FastCursor)
                {
                    this.CalcScreenPositions();
                    this.RedrawCursor();
                }
                if ((this.IXValue != iXValue) || (this.IYValue != iYValue))
                {
                    if (!this.FastCursor)
                    {
                        this.Invalidate();
                    }
                    this.OnChange(snapPoint);
                    if (this.IOldSnap != snapPoint)
                    {
                        this.OnSnapChange(snapPoint);
                    }
                    this.IOldSnap = snapPoint;
                }
            }
            else
            {
                CursorClicked clicked = this.Clicked(point.X, point.Y);
                if (this.originalCursor == null)
                {
                    this.originalCursor = c;
                }
                switch (clicked)
                {
                    case CursorClicked.Horizontal:
                        c = Cursors.HSplit;
                        break;

                    case CursorClicked.Vertical:
                        c = Cursors.VSplit;
                        break;

                    case CursorClicked.Both:
                        c = Cursors.SizeAll;
                        break;

                    default:
                        c = this.originalCursor;
                        break;
                }
                base.chart.CancelMouse = clicked != CursorClicked.None;
            }
        }

        private Rectangle MouseRectangle(int x, int y, bool UseSize)
        {
            Rectangle axisRect = this.InternalGetAxisRect();
            if (UseSize)
            {
                if (this.HorizSize != 0)
                {
                    axisRect.X = x - this.HorizSize;
                    axisRect.Width = (x + this.HorizSize) - axisRect.X;
                }
                if (this.VertSize != 0)
                {
                    axisRect.Y = y - this.VertSize;
                    axisRect.Height = (y + this.VertSize) - (y - this.VertSize);
                }
            }
            return axisRect;
        }

        [Description("Returns nearest point to Cursor and smallest distance value.")]
        public int NearestPoint(CursorToolStyles style, out double difference)
        {
            int num2;
            int num3;
            double num = 0.0;
            int num4 = -1;
            difference = -1.0;
            if (Steema.TeeChart.Tools.Tool.GetFirstLastSeries(base.iSeries, out num2, out num3))
            {
                for (int i = num2; i <= num3; i++)
                {
                    if (this.SnapStyle == Steema.TeeChart.Tools.SnapStyle.Default)
                    {
                        switch (style)
                        {
                            case CursorToolStyles.Horizontal:
                                num = Math.Abs((double) (this.IYValue - base.iSeries.YValues[i]));
                                goto Label_0128;

                            case CursorToolStyles.Vertical:
                                num = Math.Abs((double) (this.IXValue - base.iSeries.XValues[i]));
                                goto Label_0128;
                        }
                        num = Math.Sqrt(Utils.Sqr(this.IXValue - base.iSeries.XValues[i]) + Utils.Sqr(this.IYValue - base.iSeries.vyValues[i]));
                    }
                    else if (this.SnapStyle == Steema.TeeChart.Tools.SnapStyle.Horizontal)
                    {
                        num = Math.Abs((double) (this.IYValue - base.iSeries.YValues[i]));
                    }
                    else
                    {
                        num = Math.Abs((double) (this.IXValue - base.iSeries.XValues[i]));
                    }
                Label_0128:
                    if ((difference == -1.0) || (num < difference))
                    {
                        difference = num;
                        num4 = i;
                    }
                }
            }
            return num4;
        }

        protected virtual void OnChange(int snapPoint)
        {
            if (this.Change != null)
            {
                CursorChangeEventArgs e = new CursorChangeEventArgs {
                    x = this.IPoint.X,
                    y = this.IPoint.Y,
                    XValue = this.IXValue,
                    YValue = this.IYValue,
                    Series = base.iSeries,
                    SnapPoint = snapPoint
                };
                this.Change(this, e);
            }
        }

        protected virtual void OnSnapChange(int snapPoint)
        {
            if (this.SnapChange != null)
            {
                CursorChangeEventArgs e = new CursorChangeEventArgs {
                    x = this.IPoint.X,
                    y = this.IPoint.Y,
                    XValue = this.IXValue,
                    YValue = this.IYValue,
                    Series = base.iSeries,
                    SnapPoint = snapPoint
                };
                this.SnapChange(this, e);
            }
        }

        private void RedrawCursor()
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if ((base.Chart != null) && graphicsd.ValidState())
            {
                if (this.FastCursor)
                {
                    this.penXOR = -1 ^ this.Pen.Color.ToArgb();
                }
                else
                {
                    graphicsd.Pen = this.Pen;
                }
                bool visible = graphicsd.Brush.Visible;
                graphicsd.Brush.Visible = false;
                Rectangle r = this.MouseRectangle(this.IPoint.X, this.IPoint.Y, true);
                this.DrawCursorLines(base.chart.Aspect.View3D, r, this.IPoint.X, this.IPoint.Y);
                graphicsd.Brush.Visible = visible;
            }
        }

        protected override void SetSeries(Series value)
        {
            if (base.iSeries != value)
            {
                base.SetSeries(value);
                this.IOldSnap = -1;
                if (base.iSeries != null)
                {
                    this.SnapToPoint();
                    this.Invalidate();
                }
            }
        }

        [Description("Moves cursor to nearest Series point and returns point index.")]
        public int SnapToPoint()
        {
            int num2;
            if ((base.iSeries != null) && this.snap)
            {
                double num;
                num2 = this.NearestPoint(this.style, out num);
            }
            else
            {
                num2 = -1;
            }
            if (num2 != -1)
            {
                switch (this.style)
                {
                    case CursorToolStyles.Horizontal:
                        this.IYValue = base.iSeries.YValues[num2];
                        return num2;

                    case CursorToolStyles.Vertical:
                        this.IXValue = base.iSeries.XValues[num2];
                        return num2;
                }
                this.IXValue = base.iSeries.XValues[num2];
                this.IYValue = base.iSeries.YValues[num2];
            }
            return num2;
        }

        private void VerticalLine(int x, int top, int bottom, int z)
        {
            if (this.FastCursor)
            {
                if (x > 0)
                {
                    this.start.X = x;
                    this.start.Y = top;
                    this.end.X = x;
                    this.end.Y = bottom;
                    if (z > 0)
                    {
                        this.start = base.chart.graphics3D.Calc3DPoint(this.start, z);
                        this.end = base.chart.graphics3D.Calc3DPoint(this.end, z);
                    }
                    this.start = base.chart.Parent.PointToScreen(this.start);
                    this.end = base.chart.Parent.PointToScreen(this.end);
                    if (!base.chart.redrawing)
                    {
                        ControlPaint.DrawReversibleLine(this.start, this.end, Color.FromArgb(this.penXOR));
                    }
                }
            }
            else if (z > 0)
            {
                base.chart.graphics3D.VerticalLine(x, top, bottom, z);
            }
            else
            {
                base.chart.graphics3D.VerticalLine(x, top, bottom);
            }
        }

        private int Z()
        {
            if (((base.iSeries != null) && this.UseSeriesZ) && base.chart.Aspect.View3D)
            {
                return base.iSeries.StartZ;
            }
            return 0;
        }

        [Description("Gets and sets pixel proximity tolerance for Clicks."), DefaultValue(3)]
        public int CursorClickTolerance
        {
            get
            {
                return this.cursorClickTolerance;
            }
            set
            {
                this.cursorClickTolerance = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.CursorTool;
            }
        }

        [Description("Draws a cursor which does not repaint the chart but which only accepts the Pen.Color as a parameter."), DefaultValue(false)]
        public bool FastCursor
        {
            get
            {
                return this.fastCursor;
            }
            set
            {
                if (this.fastCursor != value)
                {
                    this.IPoint.X = -1;
                    this.IPoint.Y = -1;
                    this.fastCursor = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("Moves Cursor when moving the mouse.")]
        public bool FollowMouse
        {
            get
            {
                return this.followMouse;
            }
            set
            {
                this.followMouse = value;
            }
        }

        [Description("Horiz size of Cursortool (full width if 0)."), DefaultValue(0)]
        public int HorizSize
        {
            get
            {
                return this.horizSize;
            }
            set
            {
                if (this.horizSize != value)
                {
                    if (this.fastCursor)
                    {
                        this.IPoint.X = -1;
                        this.IPoint.Y = -1;
                    }
                    this.horizSize = value;
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Pen characteristics."), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (base.pPen == null)
                {
                    base.pPen = new ChartPen(base.chart, Color.Black);
                }
                return base.pPen;
            }
            set
            {
                base.pPen = value;
            }
        }

        [Description("When Style is Scope use ScopeSize to modify the size of the Scope element."), DefaultValue(4)]
        public int ScopeSize
        {
            get
            {
                return this.scopeSize;
            }
            set
            {
                if (this.scopeSize != value)
                {
                    if (this.fastCursor)
                    {
                        this.IPoint.X = -1;
                        this.IPoint.Y = -1;
                    }
                    this.scopeSize = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Select a ScopeCursorStyle option when the Style property is set to Scope."), DefaultValue(typeof(ScopeCursorStyle), "Rectangle")]
        public ScopeCursorStyle ScopeStyle
        {
            get
            {
                return this.scopeStyle;
            }
            set
            {
                if (this.scopeStyle != value)
                {
                    if (this.fastCursor)
                    {
                        this.IPoint.X = -1;
                        this.IPoint.Y = -1;
                    }
                    this.scopeStyle = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("Aligns the TCursorTool with the nearest series point.")]
        public bool Snap
        {
            get
            {
                return this.snap;
            }
            set
            {
                if (this.snap != value)
                {
                    if (this.fastCursor)
                    {
                        this.IPoint.X = -1;
                        this.IPoint.Y = -1;
                    }
                    this.snap = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Select a ScopeCursorStyle option when the Style property is set to Scope."), DefaultValue(typeof(Steema.TeeChart.Tools.SnapStyle), "Default")]
        public Steema.TeeChart.Tools.SnapStyle SnapStyle
        {
            get
            {
                return this.snapStyle;
            }
            set
            {
                if (this.snapStyle != value)
                {
                    if (this.fastCursor)
                    {
                        this.IPoint.X = -1;
                        this.IPoint.Y = -1;
                    }
                    this.snapStyle = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(2), Description("Defines which lines of the CursorTool are shown.")]
        public CursorToolStyles Style
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
                    if (this.fastCursor)
                    {
                        this.IPoint.X = -1;
                        this.IPoint.Y = -1;
                    }
                    this.SnapToPoint();
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.CursorToolSummary;
            }
        }

        [DefaultValue(false), Description("Sets full Chart rectangle instead of boundaries defined by Series axis.")]
        public bool UseChartRect
        {
            get
            {
                return this.useChartRect;
            }
            set
            {
                base.SetBooleanProperty(ref this.useChartRect, value);
            }
        }

        [DefaultValue(false), Description("Gets and sets whether the cursor will be displayed at the chart front position (the default), or will use the Series 'Z' position.")]
        public bool UseSeriesZ
        {
            get
            {
                return this.useSeriesZ;
            }
            set
            {
                if (this.useSeriesZ != value)
                {
                    if (this.fastCursor)
                    {
                        this.IPoint.X = -1;
                        this.IPoint.Y = -1;
                    }
                    this.useSeriesZ = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("Vertical size of Cursortool (full height if 0).")]
        public int VertSize
        {
            get
            {
                return this.vertSize;
            }
            set
            {
                if (this.vertSize != value)
                {
                    if (this.fastCursor)
                    {
                        this.IPoint.X = -1;
                        this.IPoint.Y = -1;
                    }
                    this.vertSize = value;
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Sets X Value for vertical component of Cursor Tool.")]
        public double XValue
        {
            get
            {
                return this.IXValue;
            }
            set
            {
                if (this.IXValue != value)
                {
                    this.IXValue = value;
                    this.IOldSnap = -1;
                    this.DoChange();
                }
            }
        }

        [Browsable(false), Description("Sets Y Value for horizontal component of Cursor Tool."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double YValue
        {
            get
            {
                return this.IYValue;
            }
            set
            {
                if (this.IYValue != value)
                {
                    this.IYValue = value;
                    this.IOldSnap = -1;
                    this.DoChange();
                }
            }
        }
    }
}

