namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ScrollBar : Steema.TeeChart.Tools.Tool
    {
        private bool done;
        private ChartBrush FArrowBrush;
        private bool FAutoRepeat;
        private Steema.TeeChart.Drawing.Bevel FBevel;
        private ChartBrush FBrush;
        private ScrollBarDrawStyle FDrawStyle;
        private bool FHorizontal;
        private bool FInDec;
        private bool FInInc;
        private int FInitial;
        private bool FInThumb;
        private bool FirstTime;
        private int FMinSize;
        private ChartPen FPen;
        private int FPosition;
        private int FSize;
        private ChartBrush FThumbBrush;
        private Timer FTimer;
        protected int Max;
        private Point OldPoint;
        private Point P1;
        private Point P2;
        private Point P3;
        private Point P4;
        private Point P5;
        private Point P6;
        private Rectangle r;
        private int ThumbBegin;
        private int ThumbEnd;
        protected int ThumbLength;
        private int ThumbSize;

        public event ScrollBarChangedSizeHandler ChangeSize;

        public event ScrollBarScrolledHandler Scrolled;

        public event ScrollBarSetPositionHandler SetPosition;

        public ScrollBar() : this(null)
        {
        }

        public ScrollBar(Chart c) : base(c)
        {
            this.FAutoRepeat = true;
            this.FirstTime = true;
            this.FMinSize = 4;
            this.FSize = 0x12;
            this.FInitial = 250;
            this.Brush.Color = Color.White;
            this.Pen.Color = SystemColors.WindowFrame;
            this.ThumbBrush.Color = Color.Silver;
            this.Bevel.Outer = BevelStyles.Raised;
        }

        private void ApplyScroll(double Delta, bool ActivateTimer)
        {
            if (Delta < 0.0)
            {
                Delta = Math.Max((double) -this.Position, Delta);
                if (this.Position > (Delta + 1.0))
                {
                    this.DoChange(this.Position + Delta, ActivateTimer);
                }
            }
            else if (((this.Position + Delta) - 1.0) < (this.TotalCount() - this.CurrentCount()))
            {
                this.DoChange(this.Position + Delta, ActivateTimer);
            }
            else
            {
                this.DoChange((double) (this.TotalCount() - this.CurrentCount()), ActivateTimer);
            }
        }

        private void BuckleGetLegendSize()
        {
            if ((base.Chart != null) && (base.Chart.parent != null))
            {
                if (base.Chart.parent is TChart)
                {
                    ((TChart) base.Chart.parent).GetLegendSize += new GetLegendSizeEventHandler(this.LegendCalcSize);
                }
                base.Chart.parent.DoInvalidate();
            }
        }

        protected virtual double CalcDelta(int A, int B)
        {
            double num;
            double num2;
            if (A == B)
            {
                return 0.0;
            }
            Rectangle rectangle = this.ScrollRectangle();
            if (this.Horizontal)
            {
                num2 = rectangle.Width / Math.Abs((int) (A - B));
                if (num2 == 0.0)
                {
                    num = num2;
                }
                else
                {
                    num = ((double) this.TotalCount()) / num2;
                }
            }
            else
            {
                num2 = rectangle.Height / Math.Abs((int) (A - B));
                if (num2 == 0.0)
                {
                    num = num2;
                }
                else
                {
                    num = ((double) this.TotalCount()) / num2;
                }
            }
            if (A < B)
            {
                num = -num;
            }
            return num;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (e is AfterDrawEventArgs)
            {
                if (!this.done)
                {
                    this.done = true;
                    this.BuckleGetLegendSize();
                }
                this.Draw();
            }
        }

        private bool ClickedDec(Point P)
        {
            return this.DecRectangle().Contains(P.X, P.Y);
        }

        private bool ClickedInc(Point P)
        {
            return this.IncRectangle().Contains(P.X, P.Y);
        }

        private bool ClickedThumb(Point P)
        {
            return this.ThumbRectangle().Contains(P.X, P.Y);
        }

        protected virtual int CurrentCount()
        {
            return this.ThumbLength;
        }

        private Rectangle DecRectangle()
        {
            if (this.Horizontal)
            {
                return Utils.FromLTRB(this.R.Left, this.R.Bottom, this.R.Left + this.FSize, this.R.Bottom + this.FSize);
            }
            return Utils.FromLTRB(this.R.Right - this.FSize, this.R.Top, this.R.Right, this.R.Top + this.FSize);
        }

        protected virtual int DeltaMain()
        {
            return (this.CurrentCount() / 4);
        }

        private void DoChange(double NewValue, bool ActivateTimer)
        {
            this.Position = Utils.Round(NewValue);
            this.DoScroll();
            if ((ActivateTimer && this.FirstTime) && this.FAutoRepeat)
            {
                if (this.FTimer == null)
                {
                    this.FTimer = new Timer();
                    this.FTimer.Tick += new EventHandler(this.DoTimer);
                }
                this.FTimer.Interval = this.FInitial;
                this.FTimer.Enabled = true;
            }
        }

        private void DoScroll()
        {
            if (this.Scrolled != null)
            {
                this.Scrolled(this, EventArgs.Empty);
            }
        }

        private void DoTimer(object sender, EventArgs e)
        {
            this.ProcessClick(this.OldPoint);
            if (this.FTimer.Interval > 50)
            {
                this.FTimer.Interval = 50;
            }
        }

        protected void Draw()
        {
            Graphics3D g = base.Chart.Graphics3D;
            if (this.ShouldDraw())
            {
                g.Brush = this.Brush;
                g.Pen = this.Pen;
                g.Rectangle(this.MainRectangle());
                g.Rectangle(this.DecRectangle());
                g.Rectangle(this.IncRectangle());
                this.DrawArrows(g);
                int num2 = this.TotalCount();
                if (num2 != 0)
                {
                    int width;
                    if (this.Horizontal)
                    {
                        width = this.ScrollRectangle().Width;
                    }
                    else
                    {
                        width = this.ScrollRectangle().Height;
                    }
                    double num = ((double) width) / ((double) num2);
                    this.ThumbSize = Math.Max(this.FMinSize, Utils.Round((double) (this.CurrentCount() * num)));
                    if (this.Horizontal)
                    {
                        this.ThumbBegin = this.ScrollRectangle().Left + Utils.Round((double) (this.Position * num));
                    }
                    else
                    {
                        this.ThumbBegin = this.ScrollRectangle().Top + Utils.Round((double) (this.Position * num));
                    }
                    this.ThumbEnd = (this.ThumbBegin + this.ThumbSize) + 1;
                    Rectangle r = this.ThumbRectangle();
                    g.Brush = this.ThumbBrush;
                    g.Pen = this.Pen;
                    g.Rectangle(r);
                    if (this.Bevel.Outer != BevelStyles.None)
                    {
                        r.Width--;
                        r.Height--;
                        this.Bevel.Draw(g, r);
                    }
                }
            }
        }

        protected void DrawArrows(Graphics3D g)
        {
            int num = this.Size / 6;
            Point[] p = null;
            if (this.Horizontal)
            {
                this.P1 = new Point(this.DecRectangle().Left + num, this.DecRectangle().Top + (this.Size / 2));
                this.P2 = new Point((this.DecRectangle().Left + this.Size) - num, this.DecRectangle().Top + num);
                this.P3 = new Point((this.DecRectangle().Left + this.Size) - num, this.DecRectangle().Bottom - num);
            }
            else
            {
                this.P1 = new Point(this.DecRectangle().Left + num, this.DecRectangle().Bottom - num);
                this.P2 = new Point(this.DecRectangle().Left + (this.Size / 2), this.DecRectangle().Top + num);
                this.P3 = new Point(this.DecRectangle().Right - num, this.DecRectangle().Bottom - num);
            }
            p = new Point[] { this.P1, this.P2, this.P3 };
            g.Brush = this.ArrowBrush;
            Color c = this.ArrowBrush.Color;
            Graphics3D.ApplyDark(ref c, 0x20);
            g.Pen = this.Pen;
            if (this.FInDec || (this.Position == 0))
            {
                g.Brush.Color = c;
            }
            else
            {
                g.Brush.Color = this.ArrowBrush.Color;
            }
            g.Polygon(p);
            if (this.Horizontal)
            {
                this.P4 = new Point(this.IncRectangle().Right - num, this.IncRectangle().Top + (this.Size / 2));
                this.P5 = new Point((this.IncRectangle().Right - this.Size) + num, this.IncRectangle().Top + num);
                this.P6 = new Point((this.IncRectangle().Right - this.Size) + num, this.IncRectangle().Bottom - num);
            }
            else
            {
                this.P4 = this.P1;
                this.P5 = this.P2;
                this.P6 = this.P3;
                this.P4.Y = this.IncRectangle().Top + num;
                this.P6.Y = this.P4.Y;
                this.P5.Y = this.IncRectangle().Bottom - num;
            }
            p = new Point[] { this.P4, this.P5, this.P6 };
            g.Brush = this.ArrowBrush;
            c = this.ArrowBrush.Color;
            Graphics3D.ApplyDark(ref c, 0x20);
            g.Pen = this.Pen;
            if (this.FInInc || (this.Position == (this.TotalCount() - this.CurrentCount())))
            {
                g.Brush.Color = c;
            }
            else
            {
                g.Brush.Color = this.ArrowBrush.Color;
            }
            g.Polygon(p);
        }

        protected virtual int GetPosition()
        {
            return this.FPosition;
        }

        private Rectangle IncRectangle()
        {
            if (this.Horizontal)
            {
                return Utils.FromLTRB(this.R.Right - this.FSize, this.R.Bottom, this.R.Right, this.R.Bottom + this.FSize);
            }
            return Utils.FromLTRB(this.R.Right - this.FSize, this.R.Bottom - this.FSize, this.R.Right, this.R.Bottom);
        }

        internal virtual void LegendCalcSize(object sender, GetLegendSizeEventArgs e)
        {
        }

        public Rectangle MainRectangle()
        {
            if (this.Horizontal)
            {
                return Utils.FromLTRB(this.R.Left, this.R.Bottom, this.R.Right, this.R.Bottom + this.FSize);
            }
            return Utils.FromLTRB(this.R.Right - this.FSize, this.R.Top, this.R.Right, this.R.Bottom);
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            base.MouseEvent(kind, e, ref c);
            switch (kind)
            {
                case MouseEventKinds.Down:
                    this.ProcessClick(new Point(e.X, e.Y));
                    return;

                case MouseEventKinds.Move:
                    this.MouseMove(e.X, e.Y);
                    return;

                case MouseEventKinds.Up:
                    this.MouseUp();
                    return;
            }
        }

        private void MouseMove(int X, int Y)
        {
            if (this.FInThumb)
            {
                double num;
                if (this.Horizontal)
                {
                    num = this.CalcDelta(X, this.OldPoint.X);
                }
                else
                {
                    num = this.CalcDelta(Y, this.OldPoint.Y);
                }
                if (Math.Abs(num) >= 1.0)
                {
                    this.ApplyScroll(num, false);
                    this.OldPoint.X = X;
                    this.OldPoint.Y = Y;
                }
            }
        }

        private void MouseUp()
        {
            this.FInThumb = false;
            this.FInDec = false;
            this.FInInc = false;
            this.FirstTime = true;
            if (this.FTimer != null)
            {
                this.FTimer.Enabled = false;
            }
            this.Invalidate();
        }

        private void ProcessClick(Point P)
        {
            this.OldPoint = P;
            this.FInThumb = false;
            this.FInDec = false;
            this.FInInc = false;
            if (this.ClickedDec(P))
            {
                this.FInDec = true;
                this.ApplyScroll(-1.0, true);
            }
            else if (this.ClickedInc(P))
            {
                this.FInInc = true;
                this.ApplyScroll(1.0, true);
            }
            else if (this.ClickedThumb(P))
            {
                this.FInThumb = true;
            }
            else if (this.MainRectangle().Contains(P.X, P.Y))
            {
                int x;
                if (this.Horizontal)
                {
                    x = P.X;
                }
                else
                {
                    x = P.Y;
                }
                if (x < this.ThumbBegin)
                {
                    this.ApplyScroll((double) -this.DeltaMain(), true);
                }
                else if (x > this.ThumbEnd)
                {
                    this.ApplyScroll((double) this.DeltaMain(), true);
                }
            }
        }

        public Rectangle ScrollRectangle()
        {
            Rectangle rectangle = this.MainRectangle();
            if (this.Horizontal)
            {
                rectangle.Inflate(this.Size * -1, 0);
                return rectangle;
            }
            rectangle.Inflate(0, this.Size * -1);
            return rectangle;
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            this.Invalidate();
        }

        protected virtual void SetIPosition(int value)
        {
            base.SetIntegerProperty(ref this.FPosition, value);
            if (this.SetPosition != null)
            {
                ScrollBarSetPositionEventArgs e = new ScrollBarSetPositionEventArgs(this.FPosition);
                this.SetPosition(this, e);
            }
        }

        protected virtual bool ShouldDraw()
        {
            return (this.TotalCount() > 0);
        }

        private Rectangle ThumbRectangle()
        {
            Rectangle empty = Rectangle.Empty;
            if (this.Horizontal)
            {
                empty = Utils.FromLTRB(this.ThumbBegin, this.R.Bottom, this.ThumbEnd, this.R.Bottom + this.FSize);
                if (empty.Width < 2)
                {
                    empty.Inflate(1, 0);
                }
                return empty;
            }
            return Utils.FromLTRB(this.R.Right - this.FSize, this.ThumbBegin, this.R.Right, this.ThumbEnd);
        }

        protected virtual int TotalCount()
        {
            return this.Max;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("The brush used to fill the up/left and down/right \"arrows\".")]
        public ChartBrush ArrowBrush
        {
            get
            {
                if (this.FArrowBrush == null)
                {
                    this.FArrowBrush = new ChartBrush(base.Chart);
                }
                this.Invalidate();
                return this.FArrowBrush;
            }
        }

        [Category("Appearance"), DefaultValue(true), Description("The color used to fill the scrollbar background.")]
        public bool AutoRepeat
        {
            get
            {
                return this.FAutoRepeat;
            }
            set
            {
                base.SetBooleanProperty(ref this.FAutoRepeat, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Style for the scrollbar border.")]
        public Steema.TeeChart.Drawing.Bevel Bevel
        {
            get
            {
                if (this.FBevel == null)
                {
                    this.FBevel = new Steema.TeeChart.Drawing.Bevel(base.Chart);
                }
                this.Invalidate();
                return this.FBevel;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Element Brush characteristics.")]
        public ChartBrush Brush
        {
            get
            {
                if (this.FBrush == null)
                {
                    this.FBrush = new ChartBrush(base.Chart);
                }
                this.Invalidate();
                return this.FBrush;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ScrollBarTool;
            }
        }

        [Description("Defines when the scrollbar is displayed.")]
        public ScrollBarDrawStyle DrawStyle
        {
            get
            {
                return this.FDrawStyle;
            }
            set
            {
                this.FDrawStyle = value;
            }
        }

        [Description("Gradient properties used to fill the main scrollbar rectangle."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                this.Invalidate();
                return this.Brush.Gradient;
            }
        }

        [Description("True when the scrollbar is horizontal, False if it is vertical."), Category("Appearance")]
        public bool Horizontal
        {
            get
            {
                return this.FHorizontal;
            }
            set
            {
                base.SetBooleanProperty(ref this.FHorizontal, value);
            }
        }

        [Description("The amount in milliseconds that the scrollbar will delay before starting the auto-repeat feature."), DefaultValue(250)]
        public int InitialDelay
        {
            get
            {
                return this.FInitial;
            }
            set
            {
                this.FInitial = value;
            }
        }

        [Description("Number of pixels that define the minimum size (width or height) of the scrollbar thumb indicator."), DefaultValue(4), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public int MinThumbSize
        {
            get
            {
                return this.FMinSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.FMinSize, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description(" Element Pen characteristics.")]
        public ChartPen Pen
        {
            get
            {
                if (this.FPen == null)
                {
                    this.FPen = new ChartPen(base.Chart, Color.Black);
                }
                this.Invalidate();
                return this.FPen;
            }
        }

        [Description("The value where the scrollbar thumb is positioned in respect the scrollbar virtual Size property."), Category("Appearance")]
        public int Position
        {
            get
            {
                return this.GetPosition();
            }
            set
            {
                this.SetIPosition(value);
            }
        }

        protected Rectangle R
        {
            get
            {
                return this.r;
            }
            set
            {
                this.r = value;
            }
        }

        [Description("The total scrollbar virtual Size magnitude."), Category("Appearance")]
        public int Size
        {
            get
            {
                return this.FSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.FSize, value);
                if (this.ChangeSize != null)
                {
                    this.ChangeSize(this, EventArgs.Empty);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("The brush used to fill the scrollbar thumb.")]
        public ChartBrush ThumbBrush
        {
            get
            {
                if (this.FThumbBrush == null)
                {
                    this.FThumbBrush = new ChartBrush(base.Chart);
                }
                this.Invalidate();
                return this.FThumbBrush;
            }
        }
    }
}

