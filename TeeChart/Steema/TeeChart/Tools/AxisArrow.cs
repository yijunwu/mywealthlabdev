namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(AxisArrow), "ToolsIcons.AxisArrow.bmp"), Description("Displays clickable arrows at axis start and ending points, to scroll axis.")]
    public class AxisArrow : ToolAxis
    {
        private int length;
        private AxisArrowPosition position;
        private bool scrollInverted;
        private int scrollPercent;

        public event MouseEventHandler Click;

        public AxisArrow() : this((Chart) null)
        {
        }

        public AxisArrow(Axis a) : this(a.Chart)
        {
            base.iAxis = a;
        }

        public AxisArrow(Chart c) : base(c)
        {
            this.length = 0x10;
            this.position = AxisArrowPosition.Both;
            this.scrollPercent = 10;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            AxisArrow arrow = t as AxisArrow;
            arrow.Brush = this.Brush.Clone() as ChartBrush;
            arrow.Length = this.Length;
            arrow.Position = this.Position;
            arrow.ScrollInverted = this.ScrollInverted;
            arrow.ScrollPercent = this.ScrollPercent;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            if ((e is AfterDrawEventArgs) && (base.iAxis != null))
            {
                base.chart.graphics3D.Brush = this.Brush;
                base.chart.graphics3D.Pen = base.Pen;
                int z = (base.chart.Aspect.View3D && base.iAxis.OtherSide) ? base.chart.Aspect.Width3D : 0;
                if ((this.position == AxisArrowPosition.Start) || (this.position == AxisArrowPosition.Both))
                {
                    this.DrawArrow(base.iAxis.IStartPos, this.length, z);
                }
                if ((this.position == AxisArrowPosition.End) || (this.position == AxisArrowPosition.Both))
                {
                    this.DrawArrow(base.Axis.IEndPos, -this.length, z);
                }
            }
        }

        private int Check(int Pos1, int Pos2)
        {
            if (Math.Abs((int) (Pos1 - base.iAxis.Position)) < 3)
            {
                if (((this.position == AxisArrowPosition.Start) || (this.position == AxisArrowPosition.Both)) && ((Pos2 > base.iAxis.IStartPos) && (Pos2 < (base.iAxis.IStartPos + this.length))))
                {
                    return 0;
                }
                if (((this.position == AxisArrowPosition.End) || (this.position == AxisArrowPosition.Both)) && ((Pos2 < base.iAxis.IEndPos) && (Pos2 > (base.iAxis.IEndPos - this.length))))
                {
                    return 1;
                }
            }
            return -1;
        }

        private int ClickedArrow(int x, int y)
        {
            if (!base.iAxis.Horizontal)
            {
                return this.Check(x, y);
            }
            return this.Check(y, x);
        }

        private void DoClick(object sender, MouseEventArgs e)
        {
            if (this.Click != null)
            {
                base.chart.CancelMouse = true;
                this.Click(sender, e);
                base.chart.IClicked = base.chart.CancelMouse;
            }
        }

        private void DoScroll(double ADelta)
        {
            double minimum = base.iAxis.Minimum;
            double maximum = base.iAxis.Maximum;
            if ((base.chart.parent != null) && base.chart.parent.DoAllowScroll(base.iAxis, ADelta, ref minimum, ref maximum))
            {
                base.iAxis.Minimum = minimum;
                base.iAxis.Maximum = maximum;
                base.iAxis.Scroll(ADelta, false);
                if (base.chart.parent != null)
                {
                    base.chart.parent.DoScroll(this, EventArgs.Empty);
                }
            }
        }

        private void DrawArrow(int APos, int ALength, int z)
        {
            Point point;
            Point point2;
            if (base.iAxis.Horizontal)
            {
                point = new Point(APos + ALength, base.iAxis.Position);
                point2 = new Point(APos, base.iAxis.Position);
            }
            else
            {
                point = new Point(base.iAxis.Position, APos + ALength);
                point2 = new Point(base.iAxis.Position, APos);
            }
            base.chart.graphics3D.Arrow(true, point, point2, 8, 8, z);
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if ((base.iAxis != null) && base.iAxis.Visible)
            {
                Point point = new Point(e.X, e.Y);
                if (kind != MouseEventKinds.Down)
                {
                    if (kind == MouseEventKinds.Move)
                    {
                        if (this.ClickedArrow(point.X, point.Y) != -1)
                        {
                            c = Cursors.Hand;
                            base.chart.CancelMouse = true;
                        }
                        else
                        {
                            c = Cursors.Default;
                        }
                    }
                }
                else if (this.scrollPercent != 0)
                {
                    int num = this.ClickedArrow(point.X, point.Y);
                    if (num != -1)
                    {
                        this.DoClick(this, e);
                    }
                    double aDelta = ((base.iAxis.Maximum - base.iAxis.Minimum) * this.scrollPercent) * 0.01;
                    if (this.scrollInverted)
                    {
                        aDelta = -aDelta;
                    }
                    switch (num)
                    {
                        case 0:
                            this.DoScroll(aDelta);
                            break;

                        case 1:
                            this.DoScroll(-aDelta);
                            break;
                    }
                    if ((num == 0) || (num == 1))
                    {
                        base.chart.CancelMouse = true;
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Brush characteristics."), Category("Appearance")]
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

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.AxisArrowTool;
            }
        }

        [Description("Sets the length of the Arrows."), DefaultValue(0x10)]
        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                base.SetIntegerProperty(ref this.length, value);
            }
        }

        [Description("Sets where the arrows are drawn on the Axis."), DefaultValue(2)]
        public AxisArrowPosition Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("Reverses direction of applied Axis Arrow scroll.")]
        public bool ScrollInverted
        {
            get
            {
                return this.scrollInverted;
            }
            set
            {
                this.scrollInverted = value;
            }
        }

        [DefaultValue(10), Description("Sets TChart scroll rate as percentage of the associated axis.")]
        public int ScrollPercent
        {
            get
            {
                return this.scrollPercent;
            }
            set
            {
                this.scrollPercent = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.AxisArrowSummary;
            }
        }
    }
}

