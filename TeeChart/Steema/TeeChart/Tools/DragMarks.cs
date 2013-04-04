namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(DragMarks), "ToolsIcons.DragMarks.bmp"), Description("Moves Series marks when dragging them with mouse.")]
    public class DragMarks : ToolSeries
    {
        private int Index;
        private Cursor oldCursor;
        private int oldX;
        private int oldY;
        private SeriesMarks.Position position;

        public event DragMarkDraggedEventHandler Dragged;

        public event DragMarkDraggingEventHandler Dragging;

        public DragMarks() : this(null)
        {
        }

        public DragMarks(Chart c) : base(c)
        {
        }

        private void CheckCursor(ref Cursor c, int x, int y)
        {
            bool flag = false;
            if (base.iSeries != null)
            {
                flag = this.CheckCursorSeries(base.iSeries, x, y);
            }
            else
            {
                for (int i = base.chart.Series.Count - 1; i >= 0; i--)
                {
                    flag = this.CheckCursorSeries(base.chart[i], x, y);
                    if (flag)
                    {
                        break;
                    }
                }
            }
            if ((base.chart.Parent.GetCursor() != c) || (this.oldCursor == null))
            {
                this.oldCursor = base.chart.Parent.GetCursor();
            }
            if (flag)
            {
                base.chart.CancelMouse = true;
                c = Cursors.Hand;
            }
            else
            {
                c = this.oldCursor;
            }
        }

        private bool CheckCursorSeries(Series s, int x, int y)
        {
            return ((s.Active && s.Marks.Visible) && (s.Marks.Clicked(x, y) != -1));
        }

        private int CheckSeries(Series s, int x, int y)
        {
            int num = -1;
            if (s.Active)
            {
                num = s.Marks.Clicked(x, y);
                if (num != -1)
                {
                    this.position = s.Marks.Positions[num];
                }
            }
            this.Index = num;
            return num;
        }

        private void MouseDown(MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            if (base.iSeries != null)
            {
                this.CheckSeries(base.iSeries, point.X, point.Y);
            }
            else
            {
                for (int i = base.chart.Series.Count - 1; i >= 0; i--)
                {
                    if (this.CheckSeries(base.chart[i], point.X, point.Y) != -1)
                    {
                        break;
                    }
                }
            }
            if (this.position != null)
            {
                this.oldX = point.X;
                this.oldY = point.Y;
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            switch (kind)
            {
                case MouseEventKinds.Down:
                    this.MouseDown(e);
                    if (this.position == null)
                    {
                        break;
                    }
                    base.chart.CancelMouse = true;
                    return;

                case MouseEventKinds.Move:
                    this.MouseMove(e, ref c);
                    if (this.position == null)
                    {
                        break;
                    }
                    this.OnDragging(new DragMarkEventArgs(this.Index, e));
                    return;

                case MouseEventKinds.Up:
                    if (this.position != null)
                    {
                        this.OnDragged(new DragMarkEventArgs(this.Index, e));
                    }
                    this.position = null;
                    break;

                default:
                    return;
            }
        }

        private void MouseMove(MouseEventArgs e, ref Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            if (this.position == null)
            {
                this.CheckCursor(ref c, point.X, point.Y);
            }
            else
            {
                int num = point.X - this.oldX;
                int num2 = point.Y - this.oldY;
                this.position.Custom = true;
                this.position.LeftTop.X += num;
                this.position.LeftTop.Y += num2;
                this.position.ArrowTo.X += num;
                this.position.ArrowTo.Y += num2;
                this.oldX = point.X;
                this.oldY = point.Y;
                base.chart.CancelMouse = true;
                this.Invalidate();
            }
        }

        protected virtual void OnDragged(DragMarkEventArgs e)
        {
            if (this.Dragged != null)
            {
                this.Dragged(this, e);
            }
        }

        protected virtual void OnDragging(DragMarkEventArgs e)
        {
            if (this.Dragging != null)
            {
                this.Dragging(this, e);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.DragMarksTool;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.DragMarksSummary;
            }
        }
    }
}

