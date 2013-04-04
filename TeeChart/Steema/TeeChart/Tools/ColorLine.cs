namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(ColorLine), "ToolsIcons.ColorLine.bmp"), Description("Displays a draggable line across axes.")]
    public class ColorLine : ToolAxis
    {
        private bool allowDrag;
        private int colorLineClickTolerance;
        private bool dragging;
        private bool dragRepaint;
        private bool draw3D;
        private bool drawBehind;
        private double lineValue;
        private bool noLimitDrag;

        public event EventHandler DragLine;

        public event ColorLineToolOnDragEventHandler EndDragLine;

        public ColorLine() : this(null)
        {
        }

        public ColorLine(Chart c) : base(c)
        {
            this.allowDrag = true;
            this.draw3D = true;
            this.colorLineClickTolerance = 3;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            ColorLine line = t as ColorLine;
            line.AllowDrag = this.AllowDrag;
            line.DragRepaint = this.DragRepaint;
            line.Draw3D = this.Draw3D;
            line.DrawBehind = this.DrawBehind;
            line.NoLimitDrag = this.NoLimitDrag;
            line.Value = this.Value;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if ((base.iAxis != null) && ((e is BeforeDrawSeriesEventArgs) || (e is AfterDrawEventArgs)))
            {
                base.chart.graphics3D.Pen = base.Pen;
                this.DrawColorLine(e is BeforeDrawSeriesEventArgs);
            }
        }

        private bool Clicked(int x, int y)
        {
            int num = base.iAxis.Horizontal ? x : y;
            if (Math.Abs((int) (num - base.iAxis.CalcPosValue(this.lineValue))) >= this.ColorLineClickTolerance)
            {
                return false;
            }
            Rectangle chartRect = base.chart.ChartRect;
            if (!base.iAxis.Horizontal)
            {
                return ((x >= chartRect.X) && (x <= chartRect.Right));
            }
            return ((y >= chartRect.Y) && (y <= chartRect.Bottom));
        }

        protected virtual void DoDragLine()
        {
            if (this.DragLine != null)
            {
                this.DragLine(this, EventArgs.Empty);
            }
        }

        protected virtual void DoEndDragLine()
        {
            if (this.EndDragLine != null)
            {
                this.EndDragLine(this);
            }
        }

        private void DoMouseDown(MouseEventArgs e, ref Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            this.dragging = this.Clicked(point.X, point.Y);
            if (this.dragging)
            {
                base.chart.CancelMouse = true;
            }
        }

        private void DoMouseMove(MouseEventArgs e, ref Cursor c)
        {
            bool flag = false;
            Point point = new Point(e.X, e.Y);
            if (this.dragging)
            {
                if (!this.dragRepaint)
                {
                    this.Invalidate();
                }
                int num = base.iAxis.Horizontal ? point.X : point.Y;
                double aValue = base.Axis.CalcPosPoint(num);
                if (!this.noLimitDrag)
                {
                    aValue = this.LimitValue(aValue);
                }
                if (this.dragRepaint)
                {
                    this.Value = aValue;
                }
                else
                {
                    flag = this.lineValue != aValue;
                    if (flag)
                    {
                        base.chart.graphics3D.Pen = base.Pen;
                        this.DrawColorLine(true);
                        this.DrawColorLine(false);
                        this.lineValue = aValue;
                    }
                }
                c = base.iAxis.Horizontal ? Cursors.VSplit : Cursors.HSplit;
                base.chart.CancelMouse = true;
                this.DoDragLine();
                if (flag)
                {
                    this.DrawColorLine(true);
                    this.DrawColorLine(false);
                }
            }
            else if (this.Clicked(point.X, point.Y))
            {
                c = base.iAxis.Horizontal ? Cursors.VSplit : Cursors.HSplit;
                base.chart.CancelMouse = true;
            }
            else
            {
                c = Cursors.Default;
            }
        }

        private void DoMouseUp(MouseEventArgs e, ref Cursor c)
        {
            if (this.dragging)
            {
                if (!this.dragRepaint)
                {
                    this.Invalidate();
                }
                this.DoEndDragLine();
                this.dragging = false;
            }
        }

        protected internal void DrawColorLine(bool Back)
        {
            this.DrawColorLine(base.chart.graphics3D, Back);
        }

        protected internal void DrawColorLine(Graphics3D g, bool Back)
        {
            Rectangle chartRect = base.chart.ChartRect;
            if (!g.Pen.Visible)
            {
                g.Pen.DrawingPen.Color = Utils.EmptyColor;
            }
            int num = base.chart.Aspect.Width3D;
            if (this.AllowDrag && !this.NoLimitDrag)
            {
                this.lineValue = this.LimitValue(this.lineValue);
            }
            int y = base.iAxis.CalcPosValue(this.lineValue);
            if (Back)
            {
                if (!base.iAxis.Horizontal)
                {
                    if (this.draw3D)
                    {
                        g.ZLine(chartRect.X, y, 0, num);
                    }
                    if (this.draw3D || this.drawBehind)
                    {
                        g.HorizontalLine(chartRect.X, chartRect.Right, y, num);
                    }
                }
                else
                {
                    if (this.draw3D)
                    {
                        g.ZLine(y, chartRect.Bottom, 0, num);
                    }
                    if (this.draw3D || this.drawBehind)
                    {
                        g.VerticalLine(y, chartRect.Y, chartRect.Bottom, num);
                    }
                }
            }
            else if ((base.chart.Aspect.View3D || !this.dragging) || this.dragRepaint)
            {
                if (base.iAxis.Horizontal)
                {
                    if (this.draw3D)
                    {
                        g.ZLine(y, chartRect.Y, 0, num);
                    }
                    if (!this.drawBehind)
                    {
                        g.VerticalLine(y, chartRect.Y, chartRect.Bottom, 0);
                    }
                }
                else
                {
                    if (this.draw3D)
                    {
                        g.ZLine(chartRect.Right, y, 0, num);
                    }
                    if (!this.drawBehind)
                    {
                        g.HorizontalLine(chartRect.X, chartRect.Right, y, 0);
                    }
                }
            }
        }

        private double LimitValue(double AValue)
        {
            double num2 = AValue;
            int iEndPos = base.iAxis.IEndPos;
            int iStartPos = base.iAxis.IStartPos;
            if (base.iAxis.Horizontal)
            {
                Utils.SwapInteger(ref iEndPos, ref iStartPos);
            }
            if (base.iAxis.Inverted)
            {
                Utils.SwapInteger(ref iEndPos, ref iStartPos);
            }
            double num = base.iAxis.CalcPosPoint(iEndPos);
            if (num2 < num)
            {
                return num;
            }
            num = base.iAxis.CalcPosPoint(iStartPos);
            if (num2 > num)
            {
                num2 = num;
            }
            return num2;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (this.allowDrag && (base.iAxis != null))
            {
                switch (kind)
                {
                    case MouseEventKinds.Down:
                        this.DoMouseDown(e, ref c);
                        break;

                    case MouseEventKinds.Move:
                        this.DoMouseMove(e, ref c);
                        return;

                    case MouseEventKinds.Up:
                        this.DoMouseUp(e, ref c);
                        return;

                    default:
                        return;
                }
            }
        }

        [DefaultValue(true), Description("")]
        public bool AllowDrag
        {
            get
            {
                return this.allowDrag;
            }
            set
            {
                base.SetBooleanProperty(ref this.allowDrag, value);
            }
        }

        [Description("Gets and sets pixel proximity tolerance for Clicks."), DefaultValue(3)]
        public int ColorLineClickTolerance
        {
            get
            {
                return this.colorLineClickTolerance;
            }
            set
            {
                this.colorLineClickTolerance = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ColorLineTool;
            }
        }

        [DefaultValue(false), Description("")]
        public bool DragRepaint
        {
            get
            {
                return this.dragRepaint;
            }
            set
            {
                base.SetBooleanProperty(ref this.dragRepaint, value);
            }
        }

        [DefaultValue(true), Description("Draws ColorLine in 3D when True.")]
        public bool Draw3D
        {
            get
            {
                return this.draw3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.draw3D, value);
            }
        }

        [DefaultValue(false), Description("Draws the ColorLine behind the series values.")]
        public bool DrawBehind
        {
            get
            {
                return this.drawBehind;
            }
            set
            {
                base.SetBooleanProperty(ref this.drawBehind, value);
            }
        }

        [DefaultValue(false), Description("Allows drag of ColorLine outside of the Chart rectangle.")]
        public bool NoLimitDrag
        {
            get
            {
                return this.noLimitDrag;
            }
            set
            {
                base.SetBooleanProperty(ref this.noLimitDrag, value);
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.ColorLineSummary;
            }
        }

        [DefaultValue((double) 0.0), Description("Determines Axis position where the ColorLine has to be drawn.")]
        public double Value
        {
            get
            {
                return this.lineValue;
            }
            set
            {
                base.SetDoubleProperty(ref this.lineValue, value);
            }
        }
    }
}

