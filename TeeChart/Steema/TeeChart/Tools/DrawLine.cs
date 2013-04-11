namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(DrawLine), "ToolsIcons.DrawLine.bmp"), Description("Allows drawing custom lines by dragging the mouse.")]
    public class DrawLine : ToolSeries
    {
        private MouseButtons button;
        private bool drawing;
        private bool enableDraw;
        private bool enableSelect;
        private bool firstMouseDown;
        public Point FromPoint;
        protected DrawLineHandle IHandle;
        private DrawLines lines;
        private Point point;
        private DrawLineItem selected;
        private DrawLineItem tmp;
        public Point ToPoint;

        public event DrawLineEventHandler DraggedLine;

        public event DrawLineEventHandler DragLine;

        public event DrawLineEventHandler NewLine;

        public event DrawLineEventHandler Select;

        public event DrawLineSelectingEventHandler Selecting;

        public DrawLine() : this(null)
        {
        }

        public DrawLine(Chart c) : base(c)
        {
            this.button = MouseButtons.Left;
            this.enableDraw = true;
            this.enableSelect = true;
            this.lines = new DrawLines();
            this.lines.tool = this;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            DrawLine line = t as DrawLine;
            line.Button = this.Button;
            line.EnableDraw = this.EnableDraw;
            line.EnableSelect = this.EnableSelect;
            line.Pen = this.Pen.Clone() as ChartPen;
        }

        public Point AxisPoint(PointDouble p)
        {
            return new Point(base.GetHorizAxis.CalcPosValue(p.X), base.GetVertAxis.CalcPosValue(p.Y));
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if ((e is AfterDrawEventArgs) && (this.lines.Count > 0))
            {
                this.ClipDrawingRegion();
                foreach (DrawLineItem item in this.lines)
                {
                    this.RedrawLine(item);
                }
                if (this.selected != null)
                {
                    this.selected.DrawHandles();
                }
                base.chart.graphics3D.UnClip();
            }
        }

        private bool CheckCursor(int x, int y, ref Cursor c)
        {
            if ((this.selected != null) && ((this.InternalClicked(x, y, DrawLineHandle.Start) == this.selected) || (this.InternalClicked(x, y, DrawLineHandle.End) == this.selected)))
            {
                c = Cursors.Cross;
                return true;
            }
            if (this.Clicked(x, y) != null)
            {
                c = Cursors.Hand;
                return true;
            }
            return false;
        }

        public DrawLineItem Clicked(int x, int y)
        {
            DrawLineItem item = this.InternalClicked(x, y, DrawLineHandle.None);
            if (item == null)
            {
                item = this.InternalClicked(x, y, DrawLineHandle.Start);
            }
            if (item == null)
            {
                item = this.InternalClicked(x, y, DrawLineHandle.End);
            }
            return item;
        }

        private bool ClickedLine(DrawLineItem line, DrawLineHandle handle, Point P)
        {
            Point point = this.AxisPoint(line.StartPos);
            Point point2 = this.AxisPoint(line.EndPos);
            switch (handle)
            {
                case DrawLineHandle.Start:
                    return line.StartHandle.Contains(P.X, P.Y);

                case DrawLineHandle.End:
                    return line.EndHandle.Contains(P.X, P.Y);
            }
            switch (line.Style)
            {
                case DrawLineStyle.Line:
                    return Graphics3D.PointInLineTolerance(P, point.X, point.Y, point2.X, point2.Y, 3);

                case DrawLineStyle.HorizParallel:
                {
                    bool flag = Graphics3D.PointInLineTolerance(P, point.X, point.Y, point2.X, point.Y, 3);
                    if (!flag)
                    {
                        flag = Graphics3D.PointInLineTolerance(P, point.X, point2.Y, point2.X, point2.Y, 3);
                    }
                    return flag;
                }
            }
            bool flag2 = Graphics3D.PointInLineTolerance(P, point.X, point.Y, point.X, point2.Y, 3);
            if (!flag2)
            {
                flag2 = Graphics3D.PointInLineTolerance(P, point2.X, point.Y, point2.X, point2.Y, 3);
            }
            return flag2;
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
                chartRect = base.chart.ChartRect;
            }
            if (base.chart.CanClip())
            {
                base.chart.graphics3D.ClipCube(chartRect, 0, base.chart.Aspect.Width3D);
            }
        }

        public void DeleteSelected()
        {
            if (this.selected != null)
            {
                this.drawing = false;
                this.IHandle = DrawLineHandle.None;
                this.lines.Remove(this.selected);
                this.selected = null;
            }
        }

        internal void DoDrawLine(Graphics3D g, Point StartPos, Point EndPos, DrawLineStyle s)
        {
            if (base.chart.aspect.view3D)
            {
                switch (s)
                {
                    case DrawLineStyle.Line:
                        g.MoveTo(StartPos, 0);
                        g.LineTo(EndPos, 0);
                        return;

                    case DrawLineStyle.HorizParallel:
                        g.HorizontalLine(StartPos.X, EndPos.X, StartPos.Y, 0);
                        g.HorizontalLine(StartPos.X, EndPos.X, EndPos.Y, 0);
                        return;
                }
                g.VerticalLine(StartPos.X, StartPos.Y, EndPos.Y, 0);
                g.VerticalLine(EndPos.X, StartPos.Y, EndPos.Y, 0);
            }
            else
            {
                switch (s)
                {
                    case DrawLineStyle.Line:
                        g.Line(StartPos.X, StartPos.Y, EndPos.X, EndPos.Y);
                        return;

                    case DrawLineStyle.HorizParallel:
                        g.HorizontalLine(StartPos.X, EndPos.X, StartPos.Y);
                        g.HorizontalLine(StartPos.X, EndPos.X, EndPos.Y);
                        return;
                }
                g.VerticalLine(StartPos.X, StartPos.Y, EndPos.Y);
                g.VerticalLine(EndPos.X, StartPos.Y, EndPos.Y);
            }
        }

        private void DoMouseDown(MouseEventArgs e, ref Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            if (Utils.GetMouseButton(e) == this.button)
            {
                this.tmp = this.enableSelect ? this.Clicked(point.X, point.Y) : null;
                this.firstMouseDown = true;
                if (this.tmp != null)
                {
                    this.FromPoint = this.AxisPoint(this.tmp.StartPos);
                    this.ToPoint = this.AxisPoint(this.tmp.EndPos);
                    this.IHandle = DrawLineHandle.Series;
                    this.point = new Point(point.X, point.Y);
                    if (this.tmp != this.selected)
                    {
                        bool allow = true;
                        if (this.Selecting != null)
                        {
                            this.Selecting(this, this.tmp, ref allow);
                        }
                        if (allow)
                        {
                            if (this.selected != null)
                            {
                                this.selected = this.tmp;
                                this.Invalidate();
                            }
                            else
                            {
                                this.selected = this.tmp;
                                this.selected.DrawHandles();
                            }
                            if (this.Select != null)
                            {
                                this.Select(this);
                            }
                        }
                    }
                    else if (this.InternalClicked(point.X, point.Y, DrawLineHandle.Start) != null)
                    {
                        this.IHandle = DrawLineHandle.Start;
                    }
                    else if (this.InternalClicked(point.X, point.Y, DrawLineHandle.End) != null)
                    {
                        this.IHandle = DrawLineHandle.End;
                    }
                    base.chart.CancelMouse = true;
                }
                else
                {
                    this.selected = null;
                    if (this.enableDraw && this.InternalGetAxisRect().Contains(point.X, point.Y))
                    {
                        this.drawing = true;
                        this.FromPoint = new Point(point.X, point.Y);
                        this.ToPoint = this.FromPoint;
                        base.chart.CancelMouse = true;
                    }
                }
            }
        }

        private void DoMouseMove(MouseEventArgs e, ref Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            if (this.drawing || (this.IHandle != DrawLineHandle.None))
            {
                if (this.drawing)
                {
                    this.ToPoint = point;
                    if (this.firstMouseDown)
                    {
                        this.tmp = new DrawLineItem(this);
                        this.firstMouseDown = false;
                    }
                }
                else
                {
                    if (this.IHandle == DrawLineHandle.Start)
                    {
                        this.FromPoint = point;
                    }
                    else if (this.IHandle == DrawLineHandle.End)
                    {
                        this.ToPoint = point;
                    }
                    else if (this.IHandle == DrawLineHandle.Series)
                    {
                        this.FromPoint.X += point.X - this.point.X;
                        this.FromPoint.Y += point.Y - this.point.Y;
                        this.ToPoint.X += point.X - this.point.X;
                        this.ToPoint.Y += point.Y - this.point.Y;
                        this.point = point;
                    }
                    if (this.DragLine != null)
                    {
                        this.DragLine(this);
                    }
                }
                if (this.tmp != null)
                {
                    this.RepositionLine(this.tmp);
                }
                if (this.selected != null)
                {
                    this.RepositionLine(this.selected);
                }
                this.Invalidate();
                base.chart.CancelMouse = true;
            }
            else if (this.enableSelect)
            {
                base.chart.CancelMouse = this.CheckCursor(point.X, point.Y, ref c);
            }
        }

        private void DoMouseUp(MouseEventArgs e, ref Cursor c)
        {
            if (Utils.GetMouseButton(e) == this.button)
            {
                if ((this.IHandle != DrawLineHandle.None) && (this.selected != null))
                {
                    if ((this.IHandle == DrawLineHandle.Start) || (this.IHandle == DrawLineHandle.Series))
                    {
                        this.selected.StartPos = this.ScreenPoint(this.FromPoint);
                    }
                    if ((this.IHandle == DrawLineHandle.End) || (this.IHandle == DrawLineHandle.Series))
                    {
                        this.selected.EndPos = this.ScreenPoint(this.ToPoint);
                    }
                    this.IHandle = DrawLineHandle.None;
                    this.Invalidate();
                    if (this.DraggedLine != null)
                    {
                        this.DraggedLine(this);
                    }
                }
                else if (this.drawing)
                {
                    this.drawing = false;
                    if (this.NewLine != null)
                    {
                        this.NewLine(this);
                    }
                }
            }
        }

        private DrawLineItem InternalClicked(int X, int Y, DrawLineHandle handle)
        {
            base.chart.graphics3D.Calculate2DPosition(ref X, ref Y, 0);
            if (this.InternalGetAxisRect().Contains(X, Y))
            {
                Point p = new Point(X, Y);
                if ((this.selected != null) && this.ClickedLine(this.selected, handle, p))
                {
                    return this.selected;
                }
                foreach (DrawLineItem item in this.lines)
                {
                    if (this.ClickedLine(item, handle, p))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        internal Rectangle InternalGetAxisRect()
        {
            Rectangle rectangle = new Rectangle();
            return new Rectangle { X = base.GetHorizAxis.IStartPos, Width = base.GetHorizAxis.IEndPos - rectangle.X, Y = base.GetVertAxis.IStartPos, Height = base.GetVertAxis.IEndPos - rectangle.Y };
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (kind == MouseEventKinds.Down)
            {
                this.DoMouseDown(e, ref c);
            }
            else if (kind == MouseEventKinds.Move)
            {
                this.DoMouseMove(e, ref c);
            }
            else if (kind == MouseEventKinds.Up)
            {
                this.DoMouseUp(e, ref c);
            }
        }

        private void RedrawLine(DrawLineItem line)
        {
            this.RedrawLine(base.chart.graphics3D, line);
        }

        internal void RedrawLine(Graphics3D g, DrawLineItem line)
        {
            if (((base.Chart != null) && g.ValidState()) && ((line != null) && line.Pen.Visible))
            {
                g.Pen = line.Pen;
                DrawLineStyle s = (line == null) ? DrawLineStyle.Line : line.Style;
                this.DoDrawLine(g, this.AxisPoint(line.StartPos), this.AxisPoint(line.EndPos), s);
            }
        }

        private void RepositionLine(DrawLineItem Line)
        {
            Line.StartPos = this.ScreenPoint(this.FromPoint);
            Line.EndPos = this.ScreenPoint(this.ToPoint);
        }

        public PointDouble ScreenPoint(Point p)
        {
            return new PointDouble(base.GetHorizAxis.CalcPosPoint(p.X), base.GetVertAxis.CalcPosPoint(p.Y));
        }

        [DefaultValue(0x100000), Description("Defines which mousebutton activates the DrawLineTool.")]
        public MouseButtons Button
        {
            get
            {
                return this.button;
            }
            set
            {
                this.button = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.DrawLineTool;
            }
        }

        [Description("Enables/Disables drawing of lines on the chart by the user."), DefaultValue(true)]
        public bool EnableDraw
        {
            get
            {
                return this.enableDraw;
            }
            set
            {
                this.enableDraw = value;
            }
        }

        [DefaultValue(true), Description("Enables selection of lines for repositioning on the Chart.")]
        public bool EnableSelect
        {
            get
            {
                return this.enableSelect;
            }
            set
            {
                this.enableSelect = value;
            }
        }

        [Browsable(false), Description("Returns the list of lines drawn on the Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DrawLines Lines
        {
            get
            {
                return this.lines;
            }
        }

        [Description("Element Pen characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
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

        [Browsable(false), Description("Returns the line or lines that are currently selected."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DrawLineItem Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                if (this.selected != value)
                {
                    this.selected = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.DrawLineSummary;
            }
        }
    }
}

