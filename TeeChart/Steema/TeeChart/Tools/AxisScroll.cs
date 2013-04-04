namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [Description("Allows the scrolling of Axes by mouse dragging"), ToolboxBitmap(typeof(AxisScroll), "ToolsIcons.AxisScroll.bmp")]
    public class AxisScroll : ToolAxis
    {
        private int Delta;
        private Axis InAxis;
        private int OldX;
        private int OldY;
        private Cursor originalCursor;
        private bool scrollInverted;

        public AxisScroll() : this((Chart) null)
        {
        }

        public AxisScroll(Axis a) : this(a.Chart)
        {
            base.iAxis = a;
        }

        public AxisScroll(Chart c) : base(c)
        {
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            AxisScroll scroll = t as AxisScroll;
            scroll.ScrollInverted = this.ScrollInverted;
        }

        private Axis AxisClicked(int X, int Y)
        {
            Axis iAxis = null;
            if (base.iAxis != null)
            {
                if (base.iAxis.Visible && base.iAxis.Clicked(X, Y))
                {
                    iAxis = base.iAxis;
                }
                return iAxis;
            }
            for (int i = 0; i < base.Chart.Axes.Count; i++)
            {
                if (base.Chart.Axes[i].Visible && base.Chart.Axes[i].Clicked(X, Y))
                {
                    return base.Chart.Axes[i];
                }
            }
            return iAxis;
        }

        private void CheckOtherAxes()
        {
            for (int i = 0; i < base.Chart.Axes.Count; i++)
            {
                if ((base.Chart.Axes[i] != this.InAxis) && (base.Chart.Axes[i].Horizontal == this.InAxis.Horizontal))
                {
                    for (int j = 0; j < base.Chart.Series.Count; j++)
                    {
                        if (base.Chart[j].AssociatedToAxis(this.InAxis) && base.Chart[j].AssociatedToAxis(base.Chart.Axes[i]))
                        {
                            this.DoAxisScroll(base.Chart.Axes[i]);
                            break;
                        }
                    }
                }
            }
        }

        private void DoAxisScroll(Axis AAxis)
        {
            if (AAxis.IAxisSize != 0)
            {
                AAxis.Scroll(this.Delta * ((AAxis.Maximum - AAxis.Minimum) / ((double) AAxis.IAxisSize)), false);
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (base.Active)
            {
                Point point = new Point(e.X, e.Y);
                switch (kind)
                {
                    case MouseEventKinds.Down:
                        this.InAxis = this.AxisClicked(point.X, point.Y);
                        this.OldX = point.X;
                        this.OldY = point.Y;
                        return;

                    case MouseEventKinds.Move:
                        if (this.InAxis == null)
                        {
                            if (this.AxisClicked(point.X, point.Y) != null)
                            {
                                if (this.originalCursor == null)
                                {
                                    this.originalCursor = c;
                                }
                                c = Cursors.Hand;
                                base.Chart.CancelMouse = true;
                                return;
                            }
                            if (this.originalCursor != null)
                            {
                                c = this.originalCursor;
                            }
                            else
                            {
                                this.originalCursor = c;
                            }
                            base.Chart.CancelMouse = false;
                            return;
                        }
                        if (!this.InAxis.Horizontal)
                        {
                            this.Delta = this.OldY - point.Y;
                            break;
                        }
                        this.Delta = this.OldX - point.X;
                        break;

                    case MouseEventKinds.Up:
                        this.InAxis = null;
                        return;

                    default:
                        return;
                }
                if (this.InAxis.Inverted)
                {
                    this.Delta = -this.Delta;
                }
                if (this.InAxis.Horizontal)
                {
                    if (this.ScrollInverted)
                    {
                        this.Delta = -this.Delta;
                    }
                }
                else if (!this.ScrollInverted)
                {
                    this.Delta = -this.Delta;
                }
                this.DoAxisScroll(this.InAxis);
                this.CheckOtherAxes();
                this.OldX = point.X;
                this.OldY = point.Y;
                base.Chart.CancelMouse = true;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.AxisScrollTool;
            }
        }

        [DefaultValue(false)]
        public bool ScrollInverted
        {
            get
            {
                return this.scrollInverted;
            }
            set
            {
                base.SetBooleanProperty(ref this.scrollInverted, value);
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.AxisScrollSummary;
            }
        }
    }
}

