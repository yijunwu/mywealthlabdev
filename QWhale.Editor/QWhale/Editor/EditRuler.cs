namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class EditRuler : Control, IEditRuler, IControl
    {
        private const int cPointsPerInt = 4;
        private const int cRulerHeight = 0x10;
        private const int cRulerTop = 6;
        private const int cTenthMMPerInch = 0xfe;
        private Size dpi;
        private int drawX = -1;
        private Color indentBackColor;
        private IRulerIndent leftIndent;
        private Pen linePen;
        private int markWidth = 8;
        private RulerOptions options = EditConsts.DefaultRulerOptions;
        private int pageStart = 0x55;
        private int pageWidth = 0x1a9;
        private IRulerIndent rightIndent;
        private int rulerStart = 0x18;
        private int rulerWidth = 600;
        private RulerUnits units = EditConsts.DefaultRulerUnits;
        private bool vertical = false;

        [Description("Occurs when some of ruler indentations has changed its position due to the dragging operation.")]
        public event EventHandler Change;

        public EditRuler()
        {
            this.leftIndent = new RulerIndent(IndentOrientation.Near, this.pageStart - this.rulerStart);
            this.rightIndent = new RulerIndent(IndentOrientation.Far, (this.rulerWidth - this.pageWidth) - (this.pageStart - this.rulerStart));
            base.Width = 200;
            base.Height = EditConsts.DefaultRulerHeight;
            this.Font = new Font(EditConsts.DefaultRulerFontName, EditConsts.DefaultRulerFontSize);
            this.Cursor = Cursors.Default;
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.linePen = new Pen(Color.Black, 1f);
            this.linePen.DashStyle = DashStyle.DashDotDot;
            this.dpi = OSUtils.GetScreenCaps();
            this.BackColor = EditConsts.DefaultRulerBackColor;
            this.IndentBackColor = EditConsts.DefaultRulerIndentBackColor;
            this.UpdateRuler();
        }

        public virtual void Assign(IEditRuler source)
        {
            this.Vertical = source.Vertical;
            this.PageStart = source.PageStart;
            this.PageWidth = source.PageWidth;
            this.RulerStart = source.RulerStart;
            this.RulerWidth = source.RulerWidth;
            this.MarkWidth = source.MarkWidth;
            this.Units = source.Units;
            this.Options = source.Options;
        }

        public virtual void CancelDragging()
        {
            if (this.leftIndent.Dragging)
            {
                this.leftIndent.CancelDragging();
                this.pageWidth += (this.pageStart - this.rulerStart) - this.leftIndent.Indent;
                this.pageStart = this.rulerStart + this.leftIndent.Indent;
                if (this.drawX != -1)
                {
                    this.DrawLine(this.drawX, true);
                }
                this.drawX = -1;
                base.Invalidate();
            }
            if (this.rightIndent.Dragging)
            {
                this.rightIndent.CancelDragging();
                this.pageWidth = (this.rulerWidth - this.rightIndent.Indent) - (this.pageStart - this.rulerStart);
                if (this.drawX != -1)
                {
                    this.DrawLine(this.drawX, true);
                }
                this.drawX = -1;
                base.Invalidate();
            }
        }

        private bool CheckCursor(Point pt)
        {
            if (((this.options & RulerOptions.AllowDrag) != RulerOptions.None) && (this.GetIndentHitRect(this.leftIndent).Contains(pt) || this.GetIndentHitRect(this.rightIndent).Contains(pt)))
            {
                Cursor cursor = this.vertical ? Cursors.SizeNS : Cursors.SizeWE;
                OSUtils.SetCursor(cursor.Handle);
                return true;
            }
            return false;
        }

        private void DrawLine(int x, bool erase)
        {
            if (((this.options & RulerOptions.DisplayDragLine) != RulerOptions.None) && (base.Parent != null))
            {
                this.drawX = x;
                if (base.Parent != null)
                {
                    x += this.Vertical ? base.Top : base.Left;
                    Point location = this.vertical ? new Point(0, x) : new Point(x, 0);
                    Point point2 = this.vertical ? new Point(base.Parent.ClientSize.Width, x) : new Point(x, base.Parent.ClientSize.Height);
                    Size size = this.vertical ? new Size(base.Parent.ClientSize.Width, 1) : new Size(1, base.Parent.ClientSize.Height);
                    if (erase)
                    {
                        base.Parent.Invalidate(new Rectangle(location, size));
                    }
                    else
                    {
                        using (Graphics graphics = base.Parent.CreateGraphics())
                        {
                            graphics.DrawLine(this.linePen, location, point2);
                        }
                    }
                }
            }
        }

        private void DrawRuler(Graphics graphics, bool direction, Rectangle rect)
        {
            if (this.vertical)
            {
                graphics.RotateTransform(-90f);
                graphics.TranslateTransform(0f, (float) (rect.Top + rect.Bottom), MatrixOrder.Append);
            }
            float rulerStep = this.GetRulerStep();
            int num2 = this.vertical ? rect.Top : rect.Left;
            int num3 = this.vertical ? rect.Bottom : rect.Right;
            float x = direction ? ((float) num2) : ((float) (num3 - (((int) this.Font.Size) / 2)));
            int num5 = 1;
            int num7 = 0;
            string text = string.Empty;
            PointF empty = PointF.Empty;
            PointF tf2 = PointF.Empty;
            SizeF ef = SizeF.Empty;
            while (direction ? (x < ((num3 - rulerStep) - (((int) this.Font.Size) / 2))) : (x > (num2 + rulerStep)))
            {
                int height;
                x = direction ? (x + rulerStep) : (x - rulerStep);
                if ((num5 % 4) == 0)
                {
                    height = this.Font.Height;
                }
                else if ((num5 % 2) == 0)
                {
                    height = this.vertical ? (rect.Width / 3) : (rect.Height / 3);
                }
                else
                {
                    height = 1;
                }
                num7 = this.vertical ? (rect.Left + ((rect.Width - height) / 2)) : (rect.Top + ((rect.Height - height) / 2));
                empty = new PointF(x, (float) num7);
                tf2 = new PointF(x, (float) (num7 + height));
                if ((num5 % 4) == 0)
                {
                    text = (num5 / 4).ToString();
                    ef = graphics.MeasureString(text, this.Font);
                    empty.X -= (int) (ef.Width / 2f);
                    graphics.DrawString(text, this.Font, Brushes.Black, empty);
                }
                else
                {
                    graphics.DrawLine(Pens.Black, empty, tf2);
                }
                num5++;
            }
            graphics.ResetTransform();
        }

        ~EditRuler()
        {
            this.linePen.Dispose();
        }

        private int GetDiscreteInterval(int x, int prevIndent)
        {
            int num = x;
            int num2 = 0;
            int num3 = this.vertical ? this.dpi.Height : this.dpi.Width;
            if ((this.options & RulerOptions.Discrete) != RulerOptions.None)
            {
                switch (this.units)
                {
                    case RulerUnits.Milimeters:
                        if (x <= prevIndent)
                        {
                            num2 = (int) Math.Ceiling((double) (((num * 0xfe) * 4) / (num3 * 100)));
                        }
                        else
                        {
                            num2 = (int) Math.Floor((double) (((num * 0xfe) * 4) / (num3 * 100)));
                        }
                        return (int) Math.Round((double) (((num2 * num3) * 100) / 0x3f8));

                    case RulerUnits.Inches:
                        if (x <= prevIndent)
                        {
                            num2 = (int) Math.Ceiling((double) ((num * 4) / num3));
                        }
                        else
                        {
                            num2 = (int) Math.Floor((double) ((num * 4) / num3));
                        }
                        return (int) Math.Round((double) ((num2 * num3) / 4));
                }
            }
            return num;
        }

        private Rectangle GetIndentHitRect(IRulerIndent Indent)
        {
            if (this.vertical)
            {
                if (Indent.Orientation == IndentOrientation.Near)
                {
                    return new Rectangle(6, (this.rulerStart + Indent.Indent) - EditConsts.DefaultRulerHitWidth, 0x10, EditConsts.DefaultRulerHitWidth * 2);
                }
                return new Rectangle(6, (this.pageStart + this.pageWidth) - EditConsts.DefaultRulerHitWidth, 0x10, EditConsts.DefaultRulerHitWidth * 2);
            }
            if (Indent.Orientation == IndentOrientation.Near)
            {
                return new Rectangle((this.rulerStart + Indent.Indent) - EditConsts.DefaultRulerHitWidth, 6, EditConsts.DefaultRulerHitWidth * 2, 0x10);
            }
            return new Rectangle((this.pageStart + this.pageWidth) - EditConsts.DefaultRulerHitWidth, 6, EditConsts.DefaultRulerHitWidth * 2, 0x10);
        }

        private int GetIndentPos(IRulerIndent ind)
        {
            if (ind.Orientation == IndentOrientation.Near)
            {
                return (this.rulerStart + ind.Indent);
            }
            return ((this.rulerStart + this.rulerWidth) - ind.Indent);
        }

        private Rectangle GetIndentRect(IRulerIndent indent)
        {
            if (this.vertical)
            {
                if (indent.Orientation == IndentOrientation.Near)
                {
                    return new Rectangle(6, this.rulerStart, 0x10, indent.Indent);
                }
                return new Rectangle(6, this.pageWidth + this.pageStart, 0x10, indent.Indent);
            }
            if (indent.Orientation == IndentOrientation.Near)
            {
                return new Rectangle(this.rulerStart, 6, indent.Indent, 0x10);
            }
            return new Rectangle(this.pageWidth + this.pageStart, 6, indent.Indent, 0x10);
        }

        private Rectangle GetLeftRulerRect()
        {
            if (this.vertical)
            {
                return new Rectangle(6, this.rulerStart, 0x10, this.pageStart - this.rulerStart);
            }
            return new Rectangle(this.rulerStart, 6, this.pageStart - this.rulerStart, 0x10);
        }

        private Rectangle GetPageRect()
        {
            if (this.vertical)
            {
                return new Rectangle(6, this.pageStart, 0x10, this.pageWidth);
            }
            return new Rectangle(this.pageStart, 6, this.pageWidth, 0x10);
        }

        private Rectangle GetRightRulerRect()
        {
            if (this.vertical)
            {
                return new Rectangle(6, this.pageStart, 0x10, this.rulerWidth - (this.pageStart - this.rulerStart));
            }
            return new Rectangle(this.pageStart, 6, this.rulerWidth - (this.pageStart - this.rulerStart), 0x10);
        }

        private RulerHitTest GetRulerHitTest(int x, int y)
        {
            Point pt = new Point(x, y);
            if (this.GetIndentRect(this.leftIndent).Contains(pt))
            {
                return RulerHitTest.LeftIndent;
            }
            if (this.GetIndentRect(this.rightIndent).Contains(pt))
            {
                return RulerHitTest.RightIndent;
            }
            if (this.GetPageRect().Contains(pt))
            {
                return RulerHitTest.Page;
            }
            return RulerHitTest.None;
        }

        private Rectangle GetRulerRect()
        {
            if (this.vertical)
            {
                return new Rectangle(6, this.rulerStart, 0x10, this.rulerWidth);
            }
            return new Rectangle(this.rulerStart, 6, this.rulerWidth, 0x10);
        }

        private float GetRulerStep()
        {
            int num = this.vertical ? this.dpi.Height : this.dpi.Width;
            switch (this.units)
            {
                case RulerUnits.Milimeters:
                    return ((num * 100f) / 1016f);

                case RulerUnits.Inches:
                    return (((float) num) / 4f);
            }
            return (float) this.markWidth;
        }

        private void IndentDown(IRulerIndent indent)
        {
            indent.Dragging = true;
        }

        private void IndentMove(IRulerIndent indent, Point pt)
        {
            int indentPos = this.vertical ? pt.Y : pt.X;
            if (indent.Orientation == IndentOrientation.Near)
            {
                indentPos = Math.Max(Math.Min(indentPos, (this.pageStart + this.pageWidth) - EditConsts.DefaultRulerMinWidth) - this.rulerStart, EditConsts.DefaultRulerIndentSize);
                indent.Indent = this.GetDiscreteInterval(indentPos, indent.Indent);
                this.pageWidth += (this.pageStart - this.rulerStart) - indent.Indent;
                this.pageStart = this.rulerStart + indent.Indent;
            }
            else
            {
                indentPos = Math.Max(indentPos, this.pageStart + EditConsts.DefaultRulerMinWidth);
                indentPos = Math.Max((this.rulerStart + this.rulerWidth) - indentPos, EditConsts.DefaultRulerIndentSize);
                indent.Indent = this.GetDiscreteInterval(indentPos, indent.Indent);
                this.pageWidth = (this.rulerWidth - indent.Indent) - (this.pageStart - this.rulerStart);
            }
            indentPos = this.GetIndentPos(indent);
            if (this.drawX != indentPos)
            {
                if (this.drawX != -1)
                {
                    this.DrawLine(this.drawX, true);
                }
                this.DrawLine(indentPos, false);
            }
            base.Invalidate();
        }

        private void IndentUp(IRulerIndent Indent)
        {
            Indent.Dragging = false;
            if (this.drawX != -1)
            {
                this.DrawLine(this.drawX, true);
            }
        }

        protected virtual void OnChange(object sender)
        {
            if (this.Change != null)
            {
                this.Change(this, new RulerEventArgs(sender));
            }
        }

        protected virtual void OnIndentBackColorChanged()
        {
            base.Invalidate();
        }

        protected virtual void OnMarkWidthChanged()
        {
            base.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if ((base.Parent != null) && base.Parent.CanFocus)
            {
                base.Parent.Focus();
            }
            this.drawX = -1;
            if ((this.options & RulerOptions.AllowDrag) != RulerOptions.None)
            {
                Point pt = new Point(e.X, e.Y);
                if (this.GetIndentHitRect(this.leftIndent).Contains(pt))
                {
                    this.IndentDown(this.leftIndent);
                }
                else if (this.GetIndentHitRect(this.rightIndent).Contains(pt))
                {
                    this.IndentDown(this.rightIndent);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point pt = new Point(e.X, e.Y);
            if (this.leftIndent.Dragging)
            {
                this.IndentMove(this.leftIndent, pt);
            }
            if (this.rightIndent.Dragging)
            {
                this.IndentMove(this.rightIndent, pt);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.leftIndent.Dragging)
            {
                this.OnChange(this.leftIndent);
            }
            this.IndentUp(this.leftIndent);
            if (this.rightIndent.Dragging)
            {
                this.OnChange(this.rightIndent);
            }
            this.IndentUp(this.rightIndent);
        }

        protected virtual void OnOptionsChanged()
        {
            base.Invalidate();
        }

        protected virtual void OnPageStartChanged()
        {
            this.UpdatePage();
        }

        protected virtual void OnPageWidthChanged()
        {
            this.UpdatePage();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics graph = pe.Graphics;
            graph.FillRectangle(SystemBrushes.Window, this.GetPageRect());
            this.leftIndent.DrawIndent(graph, this.GetIndentRect(this.leftIndent), this.vertical, this.IndentBackColor, this.BackColor);
            this.rightIndent.DrawIndent(graph, this.GetIndentRect(this.rightIndent), this.vertical, this.IndentBackColor, this.BackColor);
            this.DrawRuler(graph, this.vertical, this.GetLeftRulerRect());
            this.DrawRuler(graph, !this.vertical, this.GetRightRulerRect());
        }

        protected virtual void OnRulerStartChanged()
        {
            this.UpdatePage();
        }

        protected virtual void OnRulerWidthChanged()
        {
            this.UpdatePage();
        }

        protected virtual void OnUnitsChanged()
        {
            base.Invalidate();
        }

        protected virtual void OnVerticalChanged()
        {
            this.UpdateRuler();
        }

        void IControl.add_Click(EventHandler handler1)
        {
            base.Click += handler1;
        }

        void IControl.BringToFront()
        {
            base.BringToFront();
        }

        Graphics IControl.CreateGraphics()
        {
            return base.CreateGraphics();
        }

        Form IControl.FindForm()
        {
            return base.FindForm();
        }

        bool IControl.Focus()
        {
            return base.Focus();
        }

        bool IControl.get_CanFocus()
        {
            return base.CanFocus;
        }

        Rectangle IControl.get_ClientRectangle()
        {
            return base.ClientRectangle;
        }

        bool IControl.get_Created()
        {
            return base.Created;
        }

        bool IControl.get_Enabled()
        {
            return base.Enabled;
        }

        int IControl.get_Height()
        {
            return base.Height;
        }

        bool IControl.get_IsHandleCreated()
        {
            return base.IsHandleCreated;
        }

        int IControl.get_Left()
        {
            return base.Left;
        }

        Point IControl.get_Location()
        {
            return base.Location;
        }

        Control IControl.get_Parent()
        {
            return base.Parent;
        }

        int IControl.get_Top()
        {
            return base.Top;
        }

        bool IControl.get_Visible()
        {
            return base.Visible;
        }

        int IControl.get_Width()
        {
            return base.Width;
        }

        void IControl.Invalidate()
        {
            base.Invalidate();
        }

        void IControl.Invalidate(Rectangle rectangle1)
        {
            base.Invalidate(rectangle1);
        }

        void IControl.Invalidate(Region region1)
        {
            base.Invalidate(region1);
        }

        void IControl.Invalidate(Region region1, bool flag1)
        {
            base.Invalidate(region1, flag1);
        }

        Point IControl.PointToClient(Point point1)
        {
            return base.PointToClient(point1);
        }

        Point IControl.PointToScreen(Point point1)
        {
            return base.PointToScreen(point1);
        }

        void IControl.remove_Click(EventHandler handler1)
        {
            base.Click -= handler1;
        }

        void IControl.set_Bounds(Rectangle rectangle1)
        {
            base.Bounds = rectangle1;
        }

        void IControl.set_Enabled(bool flag1)
        {
            base.Enabled = flag1;
        }

        void IControl.set_Height(int num1)
        {
            base.Height = num1;
        }

        void IControl.set_Left(int num1)
        {
            base.Left = num1;
        }

        void IControl.set_Location(Point point1)
        {
            base.Location = point1;
        }

        void IControl.set_Parent(Control control1)
        {
            base.Parent = control1;
        }

        void IControl.set_Top(int num1)
        {
            base.Top = num1;
        }

        void IControl.set_Visible(bool flag1)
        {
            base.Visible = flag1;
        }

        void IControl.set_Width(int num1)
        {
            base.Width = num1;
        }

        void IControl.Update()
        {
            base.Update();
        }

        void IEditRuler.SendToBack()
        {
            base.SendToBack();
        }

        public virtual void ResetIndentBackColor()
        {
            this.IndentBackColor = EditConsts.DefaultRulerIndentBackColor;
        }

        public virtual void ResetOptions()
        {
            this.Options = EditConsts.DefaultRulerOptions;
        }

        public virtual void ResetUnits()
        {
            this.Units = EditConsts.DefaultRulerUnits;
        }

        private void UpdateIndents()
        {
            this.leftIndent.Indent = this.pageStart - this.rulerStart;
            this.rightIndent.Indent = (this.rulerWidth - this.pageWidth) - (this.pageStart - this.rulerStart);
        }

        private void UpdatePage()
        {
            this.UpdateIndents();
            base.Invalidate();
        }

        private void UpdateRuler()
        {
            if (this.vertical)
            {
                base.Width = EditConsts.DefaultRulerHeight;
            }
            else
            {
                base.Height = EditConsts.DefaultRulerHeight;
            }
            this.UpdateIndents();
            base.Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == 0x20) && this.CheckCursor(base.PointToClient(Control.MousePosition)))
            {
                m.Result = (IntPtr) 1;
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        [Description("Gets or sets background color of indentation parts of the ruler.")]
        public virtual Color IndentBackColor
        {
            get
            {
                return this.indentBackColor;
            }
            set
            {
                if (this.indentBackColor != value)
                {
                    this.indentBackColor = value;
                    this.OnIndentBackColorChanged();
                }
            }
        }

        [Description("Indicates whether some of ruler indentation is in dragging state.")]
        public virtual bool IsDragging
        {
            get
            {
                if (!this.leftIndent.Dragging)
                {
                    return this.rightIndent.Dragging;
                }
                return true;
            }
        }

        [Description("Gets or sets default distance between adjacent marks.")]
        public virtual int MarkWidth
        {
            get
            {
                return this.markWidth;
            }
            set
            {
                if (this.markWidth != value)
                {
                    this.markWidth = value;
                    this.OnMarkWidthChanged();
                }
            }
        }

        [Description("Gets or sets \"RulerOptions\" determining ruler behaviour.")]
        public virtual RulerOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.OnOptionsChanged();
                }
            }
        }

        [Description("Gets or sets beginning of the page part of the ruler.")]
        public virtual int PageStart
        {
            get
            {
                return this.pageStart;
            }
            set
            {
                if (this.pageStart != value)
                {
                    this.pageStart = value;
                    this.OnPageStartChanged();
                }
            }
        }

        [Description("Gets or sets width, in pixels between left and right indentations indicating width of the page.")]
        public virtual int PageWidth
        {
            get
            {
                return this.pageWidth;
            }
            set
            {
                if (this.pageWidth != value)
                {
                    this.pageWidth = value;
                    this.OnPageWidthChanged();
                }
            }
        }

        [Description("Gets or sets position, in pixels, of left indentation specifying start of the page.")]
        public int RulerStart
        {
            get
            {
                return this.rulerStart;
            }
            set
            {
                if (this.rulerStart != value)
                {
                    this.rulerStart = value;
                    this.OnRulerStartChanged();
                }
            }
        }

        [Description("Gets or sets ruler width.")]
        public virtual int RulerWidth
        {
            get
            {
                return this.rulerWidth;
            }
            set
            {
                if (this.rulerWidth != value)
                {
                    this.rulerWidth = value;
                    this.OnRulerWidthChanged();
                }
            }
        }

        [Description("Gets or sets ruler measurement units.")]
        public virtual RulerUnits Units
        {
            get
            {
                return this.units;
            }
            set
            {
                if (this.units != value)
                {
                    this.units = value;
                    this.OnUnitsChanged();
                }
            }
        }

        [Description("Gets or sets a boolean value indicating whether ruler has vertical or horizontal direction.")]
        public virtual bool Vertical
        {
            get
            {
                return this.vertical;
            }
            set
            {
                if (this.vertical != value)
                {
                    this.vertical = value;
                    this.OnVerticalChanged();
                }
            }
        }

        internal enum RulerHitTest
        {
            None,
            LeftIndent,
            RightIndent,
            Page
        }
    }
}

