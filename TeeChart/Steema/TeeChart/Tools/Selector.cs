namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(NearestPoint), "ToolsIcons.Selector.bmp"), Description("This tool enables the end-user to click and drag chart elements like series, axes, legend, titles, etc.")]
    public class Selector : Steema.TeeChart.Tools.Tool
    {
        private bool allowDrag;
        private bool allowResizeChart;
        private Steema.TeeChart.Tools.Annotation annotation;
        private System.Windows.Forms.Cursor cursor;
        private bool drawHandles;
        private int handleSize;
        private Point iDif;
        private bool iDragged;
        private bool iDragging;
        private bool iResized;
        private bool iResizingChart;
        private System.Windows.Forms.Cursor oldCursor;
        public ChartClickedPart Part;
        private object[] selectableParts;
        private TextShapePosition shape;
        private Steema.TeeChart.Wall wall;

        public event SelectorDraggedEventHandler Dragged;

        public event SelectorDraggingEventHandler Dragging;

        public event SelectorResizedEventHandler Resized;

        public event SelectorResizingEventHandler Resizing;

        public event SelectorSelectedEventHandler Selected;

        public Selector() : this(null)
        {
        }

        public Selector(Chart c) : base(c)
        {
            this.iDif = new Point(0, 0);
            this.allowDrag = true;
            this.allowResizeChart = false;
            this.handleSize = 3;
            this.drawHandles = true;
            this.cursor = Cursors.Hand;
            this.Brush.Color = Color.Black;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            Selector selector = t as Selector;
            TextShapePosition position = this.shape.Clone() as TextShapePosition;
            selector.shape = position;
        }

        private Steema.TeeChart.Tools.Annotation CalcClickedAnnotion(MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            for (int i = 0; i < base.chart.Tools.Count; i++)
            {
                if ((base.chart.Tools[i] is Steema.TeeChart.Tools.Annotation) && (base.chart.Tools[i] as Steema.TeeChart.Tools.Annotation).Clicked(point.X, point.Y))
                {
                    return (base.chart.Tools[i] as Steema.TeeChart.Tools.Annotation);
                }
            }
            return null;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            if (((e is AfterDrawEventArgs) && !base.chart.printing) && this.drawHandles)
            {
                Graphics3D graphicsd = base.chart.Graphics3D;
                graphicsd.Pen = base.pPen;
                graphicsd.Brush = base.bBrush;
                if (this.shape != null)
                {
                    this.DrawHandle(this.shape.ShapeBounds);
                }
                else
                {
                    this.DoDrawHandles();
                }
            }
        }

        private bool ClickedCorner(MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            return ((Math.Abs((int) (point.X - base.chart.Width)) <= 8) && (Math.Abs((int) (point.Y - base.chart.Height)) <= 8));
        }

        private bool ContainsSelected(ChartClickedPart selected)
        {
            if (this.selectableParts != null)
            {
                foreach (object obj2 in this.selectableParts)
                {
                    if (obj2 is ChartClickedPartStyle)
                    {
                        if (((ChartClickedPartStyle) obj2) == selected.Part)
                        {
                            return true;
                        }
                    }
                    else if ((obj2 is Axis) && (selected.AAxis != null))
                    {
                        if (((Axis) obj2) == selected.AAxis)
                        {
                            return true;
                        }
                    }
                    else if ((obj2 is Steema.TeeChart.Styles.Series) && (selected.ASeries != null))
                    {
                        if (((Steema.TeeChart.Styles.Series) obj2) == selected.ASeries)
                        {
                            return true;
                        }
                    }
                    else if (((obj2 is Steema.TeeChart.Wall) && (selected.Part == ChartClickedPartStyle.ChartRect)) && (((Steema.TeeChart.Wall) obj2) == base.chart.Walls.Back))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void DoDragShape(MouseEventArgs e)
        {
            int width;
            base.chart.CancelMouse = true;
            Point point = new Point(e.X, e.Y);
            if ((this.shape.Left != (point.X + this.iDif.X)) && (this.iDragged || (Math.Abs((int) (this.shape.Left - (point.X + this.iDif.X))) > 2)))
            {
                width = this.shape.Width;
                this.shape.CustomPosition = true;
                this.shape.Left = point.X + this.iDif.X;
                this.shape.Width = width;
                this.iDragged = true;
            }
            if ((this.shape.Top != (point.Y + this.iDif.Y)) && (this.iDragged || (Math.Abs((int) (this.shape.Top - (point.Y + this.iDif.Y))) > 2)))
            {
                width = this.shape.Height;
                this.shape.CustomPosition = true;
                this.shape.Top = point.Y + this.iDif.Y;
                this.shape.Height = width;
                this.iDragged = true;
            }
            this.OnDragging();
        }

        private void DoDrawHandles()
        {
            Rectangle r = Utils.FromLTRB(0, 0, 0, 0);
            switch (this.Part.Part)
            {
                case ChartClickedPartStyle.None:
                    r = base.chart.ChartBounds;
                    r.Inflate(-this.HandleSize, -this.HandleSize);
                    break;

                case ChartClickedPartStyle.Legend:
                    if (base.chart.Legend.Visible)
                    {
                        r = base.chart.Legend.ShapeBounds;
                    }
                    break;

                case ChartClickedPartStyle.Axis:
                    r = this.Part.AAxis.AxisRect();
                    break;

                case ChartClickedPartStyle.Series:
                    this.DrawSeriesHandles(false);
                    break;

                case ChartClickedPartStyle.Header:
                    r = base.chart.Header.ShapeBounds;
                    break;

                case ChartClickedPartStyle.Foot:
                    r = base.chart.Footer.ShapeBounds;
                    break;

                case ChartClickedPartStyle.ChartRect:
                    this.DrawBackWallHandles();
                    break;

                case ChartClickedPartStyle.SeriesMarks:
                    this.DrawSeriesHandles(true);
                    break;

                case ChartClickedPartStyle.AxisTitle:
                    r = this.Part.AAxis.Title.ShapeBounds;
                    break;
            }
            if ((r.Right - r.Left) > 0)
            {
                this.DrawHandle(r);
            }
        }

        private void DrawBackWallHandles()
        {
            Rectangle chartRect = base.chart.ChartRect;
            this.DrawHandlePoint(chartRect.Left, chartRect.Top);
            this.DrawHandlePoint(chartRect.Right, chartRect.Top);
            this.DrawHandlePoint(chartRect.Left, chartRect.Bottom);
            this.DrawHandlePoint(chartRect.Right, chartRect.Bottom);
        }

        private void DrawHandle(Rectangle R)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            graphicsd.Rectangle(this.RectFromPoint(R.Left, R.Top));
            graphicsd.Rectangle(this.RectFromPoint(R.Left, R.Bottom));
            graphicsd.Rectangle(this.RectFromPoint(R.Right, R.Top));
            graphicsd.Rectangle(this.RectFromPoint(R.Right, R.Bottom));
        }

        private void DrawHandlePoint(int AX, int AY)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            Point point = graphicsd.Calculate3DPosition(AX, AY, base.chart.Aspect.Width3D);
            graphicsd.Rectangle(this.RectFromPoint(point.X, point.Y));
        }

        private void DrawHandleZ(Rectangle R, int Z)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            graphicsd.Rectangle(this.RectFromPoint(R.Left, R.Top), Z);
            graphicsd.Rectangle(this.RectFromPoint(R.Left, R.Bottom), Z);
            graphicsd.Rectangle(this.RectFromPoint(R.Right, R.Top), Z);
            graphicsd.Rectangle(this.RectFromPoint(R.Right, R.Bottom), Z);
        }

        private void DrawSeriesHandles(bool AtMarks)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            if (this.Part.ASeries != null)
            {
                int num4;
                Steema.TeeChart.Styles.Series aSeries = this.Part.ASeries;
                int count = aSeries.LastVisibleIndex - aSeries.FirstVisibleIndex;
                if (count == 0)
                {
                    count = aSeries.Count;
                }
                if (count > 20)
                {
                    num4 = count / 20;
                }
                else
                {
                    num4 = 1;
                }
                int firstVisibleIndex = aSeries.FirstVisibleIndex;
                if (firstVisibleIndex == -1)
                {
                    firstVisibleIndex = 0;
                }
                int lastVisibleIndex = aSeries.LastVisibleIndex;
                if (lastVisibleIndex == -1)
                {
                    lastVisibleIndex = aSeries.Count - 1;
                }
                while (firstVisibleIndex <= lastVisibleIndex)
                {
                    if (AtMarks)
                    {
                        this.DrawHandleZ(aSeries.Marks.Positions[firstVisibleIndex].Bounds, aSeries.Marks.ZPosition);
                    }
                    else
                    {
                        int x = aSeries.CalcXPos(firstVisibleIndex);
                        int y = aSeries.CalcYPos(firstVisibleIndex);
                        graphicsd.Rectangle(this.RectFromPoint(x, y), aSeries.MiddleZ);
                    }
                    firstVisibleIndex += num4;
                }
            }
        }

        private void EmptySelection()
        {
            this.Part.Part = ChartClickedPartStyle.None;
            this.Part.ASeries = null;
            this.Part.AAxis = null;
            this.annotation = null;
            this.shape = null;
            this.wall = null;
        }

        private void GuessClick(MouseEventArgs e)
        {
            this.iResizingChart = false;
            this.EmptySelection();
            Point pos = new Point(e.X, e.Y);
            this.annotation = this.CalcClickedAnnotion(e);
            if (this.annotation != null)
            {
                this.shape = this.annotation.Shape;
            }
            if (this.shape == null)
            {
                base.chart.CalcClickedPart(pos, out this.Part);
                switch (this.Part.Part)
                {
                    case ChartClickedPartStyle.Legend:
                        if (this.SetSelectedPart(this.Part))
                        {
                            this.shape = base.chart.Legend;
                        }
                        goto Label_0187;

                    case ChartClickedPartStyle.Header:
                        if (this.SetSelectedPart(this.Part))
                        {
                            this.shape = base.chart.Header;
                        }
                        goto Label_0187;

                    case ChartClickedPartStyle.Foot:
                        if (this.SetSelectedPart(this.Part))
                        {
                            this.shape = base.chart.Footer;
                        }
                        goto Label_0187;

                    case ChartClickedPartStyle.ChartRect:
                        if (this.SetSelectedPart(this.Part))
                        {
                            this.wall = base.chart.Walls.Back;
                        }
                        goto Label_0187;

                    case ChartClickedPartStyle.SubHeader:
                        if (this.SetSelectedPart(this.Part))
                        {
                            this.shape = base.chart.SubHeader;
                        }
                        goto Label_0187;

                    case ChartClickedPartStyle.SubFoot:
                        if (this.SetSelectedPart(this.Part))
                        {
                            this.shape = base.chart.SubFooter;
                        }
                        goto Label_0187;
                }
                this.SetSelectedPart(this.Part);
            }
        Label_0187:
            this.OnSelected();
            this.iDragging = (this.AllowDrag && (this.shape != null)) && !base.chart.CancelMouse;
            if (this.iDragging)
            {
                this.iDif.X = this.shape.Left - pos.X;
                this.iDif.Y = this.shape.Top - pos.Y;
                this.iDragged = false;
                base.chart.CancelMouse = true;
            }
        }

        private void GuessCursor(MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            if ((base.chart.Parent.GetCursor() != this.Cursor) || (this.oldCursor == null))
            {
                this.oldCursor = base.chart.Parent.GetCursor();
            }
            if (this.CalcClickedAnnotion(e) != null)
            {
                base.chart.Parent.SetCursor(this.Cursor);
                base.chart.CancelMouse = true;
            }
            else
            {
                ChartClickedPart part;
                base.chart.CalcClickedPart(new Point(point.X, point.Y), out part);
                bool flag = (this.selectableParts == null) || ((this.selectableParts != null) && this.ContainsSelected(part));
                if (((part.Part != ChartClickedPartStyle.None) && (part.Part != ChartClickedPartStyle.ChartRect)) && flag)
                {
                    base.chart.Parent.SetCursor(this.Cursor);
                    base.chart.CancelMouse = true;
                }
                else
                {
                    base.chart.Parent.SetCursor(this.oldCursor);
                    base.chart.CancelMouse = true;
                }
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref System.Windows.Forms.Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            switch (kind)
            {
                case MouseEventKinds.Down:
                    if (Utils.GetMouseButton(e) == MouseButtons.Left)
                    {
                        if (!this.AllowResizeChart || !this.ClickedCorner(e))
                        {
                            this.GuessClick(e);
                            break;
                        }
                        this.iResizingChart = true;
                        this.iResized = false;
                    }
                    break;

                case MouseEventKinds.Move:
                    if ((this.shape == null) || !this.iDragging)
                    {
                        if (this.iResizingChart)
                        {
                            Control control = (base.chart.Parent == null) ? null : base.chart.Parent.GetControl();
                            if ((base.chart.Width != point.X) && (control != null))
                            {
                                control.Width = point.X;
                            }
                            if ((base.chart.Height != point.Y) && (control != null))
                            {
                                control.Height = point.Y;
                            }
                            this.iResized = true;
                            this.OnResizing();
                        }
                        else
                        {
                            this.GuessCursor(e);
                        }
                        break;
                    }
                    this.DoDragShape(e);
                    break;

                case MouseEventKinds.Up:
                    if (this.iDragged)
                    {
                        this.OnDragged();
                    }
                    if (this.iResized)
                    {
                        this.OnResized();
                    }
                    this.StopDragging();
                    break;
            }
            base.chart.Parent.DoInvalidate();
        }

        public virtual void OnDragged()
        {
            if (this.Dragged != null)
            {
                this.Dragged(this, EventArgs.Empty);
            }
        }

        public virtual void OnDragging()
        {
            if (this.Dragging != null)
            {
                this.Dragging(this, EventArgs.Empty);
            }
        }

        public virtual void OnResized()
        {
            if (this.Resized != null)
            {
                this.Resized(this, EventArgs.Empty);
            }
        }

        public virtual void OnResizing()
        {
            if (this.Resizing != null)
            {
                this.Resizing(this, EventArgs.Empty);
            }
        }

        public virtual void OnSelected()
        {
            if (this.Selected != null)
            {
                this.Selected(this, EventArgs.Empty);
            }
        }

        private Rectangle RectFromPoint(int X, int Y)
        {
            return Utils.FromLTRB(X - this.HandleSize, Y - this.HandleSize, X + this.HandleSize, Y + this.HandleSize);
        }

        private bool SetSelectedPart(ChartClickedPart selected)
        {
            bool flag = this.ContainsSelected(selected);
            if (this.selectableParts == null)
            {
                return true;
            }
            if (flag)
            {
                this.Part = selected;
                return flag;
            }
            this.EmptySelection();
            return flag;
        }

        private void StopDragging()
        {
            this.iDragged = false;
            this.iDragging = false;
            this.iResizingChart = false;
            this.iResized = false;
        }

        [Description("Permits the end user to move selected chart elements like series marks, legend, titles, etc."), DefaultValue(true)]
        public bool AllowDrag
        {
            get
            {
                return this.allowDrag;
            }
            set
            {
                this.allowDrag = value;
            }
        }

        [Description("When True, the selector tool enables clicking the chart to select it and drag the chart corner handles to resize it."), DefaultValue(false)]
        public bool AllowResizeChart
        {
            get
            {
                return this.allowResizeChart;
            }
            set
            {
                this.allowResizeChart = value;
            }
        }

        [Description("Returns the currently selected Annotation tool, if any.")]
        public Steema.TeeChart.Tools.Annotation Annotation
        {
            get
            {
                return this.annotation;
            }
            set
            {
                if ((this.Part.Part != ChartClickedPartStyle.None) || (this.annotation != value))
                {
                    this.EmptySelection();
                    this.annotation = value.Clone() as Steema.TeeChart.Tools.Annotation;
                    if (this.annotation != null)
                    {
                        this.shape = this.annotation.Shape;
                    }
                    else
                    {
                        this.shape = null;
                    }
                    this.Part.Part = ChartClickedPartStyle.None;
                    this.OnSelected();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Element Brush characteristics.")]
        public ChartBrush Brush
        {
            get
            {
                if (base.bBrush == null)
                {
                    base.bBrush = new ChartBrush(base.chart);
                }
                return base.bBrush;
            }
            set
            {
                base.bBrush = value;
            }
        }

        [DefaultValue(typeof(Cursors), "Hand")]
        public System.Windows.Forms.Cursor Cursor
        {
            get
            {
                return this.cursor;
            }
            set
            {
                this.cursor = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.SelectorTool;
            }
        }

        [Description("Returns the object that is being currently dragged.")]
        public TextShapePosition DraggingShape
        {
            get
            {
                return this.shape;
            }
        }

        [Description("Controls if the selector tool should draw the small black rectangles at selected item corners.")]
        public bool DrawHandles
        {
            get
            {
                return this.drawHandles;
            }
            set
            {
                this.drawHandles = value;
            }
        }

        [DefaultValue(3), Description("The size in pixels of the small black rectangles displayed at selected item corners.")]
        public int HandleSize
        {
            get
            {
                return this.handleSize;
            }
            set
            {
                if (this.handleSize != value)
                {
                    this.handleSize = value;
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
                    base.pPen = new ChartPen(base.chart);
                }
                return base.pPen;
            }
            set
            {
                base.pPen = value;
            }
        }

        [DefaultValue((string) null)]
        public object[] SelectableParts
        {
            get
            {
                return this.selectableParts;
            }
            set
            {
                this.selectableParts = value;
            }
        }

        public object Selection
        {
            get
            {
                if (this.Annotation != null)
                {
                    return this.Annotation;
                }
                switch (this.Part.Part)
                {
                    case ChartClickedPartStyle.None:
                        return base.chart;

                    case ChartClickedPartStyle.Legend:
                    case ChartClickedPartStyle.Header:
                    case ChartClickedPartStyle.Foot:
                    case ChartClickedPartStyle.SubHeader:
                    case ChartClickedPartStyle.SubFoot:
                        return this.DraggingShape;

                    case ChartClickedPartStyle.Axis:
                        return this.Part.AAxis;

                    case ChartClickedPartStyle.Series:
                        return this.Part.ASeries;

                    case ChartClickedPartStyle.ChartRect:
                        return this.Wall;

                    case ChartClickedPartStyle.SeriesMarks:
                        return this.Part.ASeries.Marks;

                    case ChartClickedPartStyle.AxisTitle:
                        return this.Part.AAxis.Title;
                }
                return null;
            }
        }

        [Description("The currently selected chart Series, if any")]
        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.Part.ASeries;
            }
            set
            {
                if (((this.Part.Part != ChartClickedPartStyle.Series) || (this.Part.ASeries != value)) && (value != null))
                {
                    this.EmptySelection();
                    this.Part.Part = ChartClickedPartStyle.Series;
                    this.Part.ASeries = value;
                    base.Chart = value.chart;
                    this.Part.PointIndex = -1;
                    this.OnSelected();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.SelectorToolSummary;
            }
        }

        [Description("The currently selected chart Wall, if any.")]
        public Steema.TeeChart.Wall Wall
        {
            get
            {
                return this.wall;
            }
            set
            {
                if (((this.Part.Part != ChartClickedPartStyle.None) || (this.wall != value)) && (value != null))
                {
                    this.EmptySelection();
                    this.Part.Part = ChartClickedPartStyle.None;
                    this.wall = value;
                    if ((this.wall != null) && (this.wall == base.chart.Walls.Back))
                    {
                        this.Part.Part = ChartClickedPartStyle.ChartRect;
                    }
                    this.OnSelected();
                }
            }
        }
    }
}

