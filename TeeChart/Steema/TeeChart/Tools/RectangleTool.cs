namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [Description("Displays custom text at any location inside Chart within a resizable box."), ToolboxBitmap(typeof(RectangleTool), "ToolsIcons.RectangleTool.bmp")]
    public class RectangleTool : Annotation
    {
        private bool allowDrag;
        private bool allowResize;
        private bool iDrag;
        private int iEdge;
        private Point P;

        public event RectangleToolDraggedEventHandler Dragged;

        public event RectangleToolDraggingEventHandler Dragging;

        public event RectangleToolResizedEventHandler Resized;

        public event RectangleToolResizingEventHandler Resizing;

        public RectangleTool() : this(null)
        {
        }

        public RectangleTool(Chart c) : base(c)
        {
            base.AutoSize = false;
            this.allowDrag = true;
            this.allowResize = true;
            this.iEdge = -1;
            base.Shape.CustomPosition = true;
            base.Shape.Shadow.Visible = false;
            base.Shape.Transparency = 0x4b;
            base.Shape.Left = 10;
            base.Shape.Top = 10;
            base.Shape.Width = 50;
            base.Shape.Height = 50;
            base.Shape.Bevel.Inner = BevelStyles.None;
            base.Shape.Bevel.Outer = BevelStyles.None;
            base.PositionUnits = PositionUnits.Pixels;
            base.Cursor = Cursors.Hand;
            this.P = new Point(0, 0);
        }

        private void ChangeBottom(ref int Bottom, int Y)
        {
            Bottom = Math.Max((int) (base.Shape.Top + 3), (int) (Y - this.P.Y));
        }

        private void ChangeLeft(ref int Left, int X)
        {
            Left = Math.Min((int) (base.Shape.Right - 3), (int) (X - this.P.X));
        }

        private void ChangeRight(ref int Right, int X)
        {
            Right = Math.Max((int) (base.Shape.Left + 3), (int) (X - this.P.X));
        }

        private void ChangeTop(ref int Top, int Y)
        {
            Top = Math.Min((int) (base.Shape.Bottom - 3), (int) (Y - this.P.Y));
        }

        private int ClickedEdge(int x, int y)
        {
            int num = -1;
            if (base.Clicked(x, y))
            {
                Rectangle shapeBounds = base.Shape.ShapeBounds;
                if (Math.Abs((int) (x - shapeBounds.Left)) < 4)
                {
                    if (Math.Abs((int) (y - shapeBounds.Top)) < 4)
                    {
                        return 4;
                    }
                    if (Math.Abs((int) (y - shapeBounds.Bottom)) < 4)
                    {
                        return 5;
                    }
                    return 0;
                }
                if (Math.Abs((int) (y - shapeBounds.Top)) < 4)
                {
                    if (Math.Abs((int) (x - shapeBounds.Right)) < 4)
                    {
                        return 6;
                    }
                    return 1;
                }
                if (Math.Abs((int) (x - shapeBounds.Right)) < 4)
                {
                    if (Math.Abs((int) (y - shapeBounds.Bottom)) < 4)
                    {
                        return 7;
                    }
                    return 2;
                }
                if (Math.Abs((int) (y - shapeBounds.Bottom)) < 4)
                {
                    num = 3;
                }
            }
            return num;
        }

        private void DoResize(int X, int Y)
        {
            Rectangle empty = Rectangle.Empty;
            int left = base.Shape.Left;
            int right = base.Shape.Right;
            int top = base.Shape.Top;
            int bottom = base.Shape.Bottom;
            switch (this.iEdge)
            {
                case 0:
                    this.ChangeLeft(ref left, X);
                    break;

                case 1:
                    this.ChangeTop(ref top, Y);
                    break;

                case 2:
                    this.ChangeRight(ref right, X);
                    break;

                case 3:
                    this.ChangeBottom(ref bottom, Y);
                    break;

                case 4:
                    this.ChangeLeft(ref left, X);
                    this.ChangeTop(ref top, Y);
                    break;

                case 5:
                    this.ChangeLeft(ref left, X);
                    this.ChangeBottom(ref bottom, Y);
                    break;

                case 6:
                    this.ChangeRight(ref right, X);
                    this.ChangeTop(ref top, Y);
                    break;

                case 7:
                    this.ChangeRight(ref right, X);
                    this.ChangeBottom(ref bottom, Y);
                    break;
            }
            empty = Utils.FromLTRB(left, top, right, bottom);
            base.Shape.CustomPosition = true;
            base.Shape.ShapeBounds = empty;
            base.Shape.Invalidate();
            this.OnResizing(EventArgs.Empty);
        }

        private bool GuessEdgeCursor(int x, int y)
        {
            bool flag = false;
            base.chart.parent.SetCursor(Cursors.Default);
            switch (this.ClickedEdge(x, y))
            {
                case 0:
                    return this.TrySet(Cursors.SizeWE);

                case 1:
                    return this.TrySet(Cursors.SizeNS);

                case 2:
                    return this.TrySet(Cursors.SizeWE);

                case 3:
                    return this.TrySet(Cursors.SizeNS);

                case 4:
                    return this.TrySet(Cursors.SizeNWSE);

                case 5:
                    return this.TrySet(Cursors.SizeNESW);

                case 6:
                    return this.TrySet(Cursors.SizeNESW);

                case 7:
                    return this.TrySet(Cursors.SizeNWSE);
            }
            if (base.Clicked(x, y) && (base.Cursor != Cursors.Default))
            {
                flag = this.TrySet(base.Cursor);
            }
            return flag;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (base.Active && !base.isEditing)
            {
                Point point = new Point(e.X, e.Y);
                switch (kind)
                {
                    case MouseEventKinds.Down:
                        if (Utils.GetMouseButton(e) == MouseButtons.Left)
                        {
                            if (this.AllowResize)
                            {
                                this.iEdge = this.ClickedEdge(point.X, point.Y);
                            }
                            if (this.iEdge != -1)
                            {
                                this.StartResizing(point.X, point.Y);
                            }
                            else if (base.Clicked(point.X, point.Y) && this.AllowDrag)
                            {
                                this.StartDragging(point.X, point.Y);
                            }
                        }
                        if (e.Clicks == 2)
                        {
                            this.OnDoubleClick(e);
                            return;
                        }
                        this.OnClick(e);
                        return;

                    case MouseEventKinds.Move:
                        if (!this.iDrag)
                        {
                            if (this.iEdge != -1)
                            {
                                this.DoResize(point.X, point.Y);
                            }
                            else
                            {
                                base.chart.CancelMouse = this.GuessEdgeCursor(point.X, point.Y);
                            }
                            break;
                        }
                        this.TryDrag(point.X, point.Y);
                        break;

                    case MouseEventKinds.Up:
                        if (!this.iDrag)
                        {
                            if (this.iEdge != -1)
                            {
                                this.StopResize();
                            }
                            return;
                        }
                        this.StopDrag();
                        return;

                    default:
                        return;
                }
                base.tmpX = point.X;
                base.tmpY = point.Y;
            }
        }

        protected virtual void OnDragged(EventArgs e)
        {
            if (this.Dragged != null)
            {
                this.Dragged(this, e);
            }
        }

        protected virtual void OnDragging(EventArgs e)
        {
            if (this.Dragging != null)
            {
                this.Dragging(this, e);
            }
        }

        protected virtual void OnResized(EventArgs e)
        {
            if (this.Resized != null)
            {
                this.Resized(this, e);
            }
        }

        protected virtual void OnResizing(EventArgs e)
        {
            if (this.Resizing != null)
            {
                this.Resizing(this, e);
            }
        }

        private void StartDragging(int X, int Y)
        {
            this.P.X = X - base.Shape.Left;
            this.P.Y = Y - base.Shape.Top;
            this.iDrag = true;
            base.chart.CancelMouse = true;
        }

        private void StartResizing(int X, int Y)
        {
            if (((this.iEdge == 2) || (this.iEdge == 6)) || (this.iEdge == 7))
            {
                this.P.X = X - base.Shape.ShapeBounds.Right;
            }
            else
            {
                this.P.X = X - base.Shape.Left;
            }
            if (((this.iEdge == 3) || (this.iEdge == 5)) || (this.iEdge == 7))
            {
                this.P.Y = Y - base.Shape.ShapeBounds.Bottom;
            }
            else
            {
                this.P.Y = Y - base.Shape.Top;
            }
            base.chart.CancelMouse = true;
        }

        private void StopDrag()
        {
            this.OnDragged(EventArgs.Empty);
            this.iDrag = false;
        }

        private void StopResize()
        {
            this.OnResized(EventArgs.Empty);
            this.iEdge = -1;
        }

        private void TryDrag(int X, int Y)
        {
            if ((X != this.P.X) || (Y != this.P.Y))
            {
                int width = base.Shape.Width;
                int height = base.Shape.Height;
                base.Shape.CustomPosition = true;
                base.Shape.Left = X - this.P.X;
                base.Shape.Top = Y - this.P.Y;
                base.Shape.Width = width;
                base.Shape.Height = height;
                this.OnDragging(EventArgs.Empty);
            }
        }

        private bool TrySet(Cursor aCursor)
        {
            if (this.AllowResize)
            {
                base.chart.parent.SetCursor(aCursor);
                return true;
            }
            return false;
        }

        [DefaultValue(true)]
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

        [DefaultValue(true)]
        public bool AllowResize
        {
            get
            {
                return this.allowResize;
            }
            set
            {
                this.allowResize = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.RectangleTool;
            }
        }
    }
}

