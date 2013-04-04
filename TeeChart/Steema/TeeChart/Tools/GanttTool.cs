namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(GanttTool), "ToolsIcons.GanttTool.bmp"), Description("Allows dragging and resizing Gantt bars.")]
    public class GanttTool : ToolSeries
    {
        private bool allowDrag;
        private bool allowResize;
        private int bar;
        private GanttBarPart barPart;
        private Cursor cursorDrag;
        private Cursor cursorResize;
        private int minPixels;
        private double xOriginal;

        public event GanttDragEventHandler DragBar;

        public event GanttResizeEventHandler ResizeBar;

        public GanttTool() : this(null)
        {
        }

        public GanttTool(Chart c) : base(c)
        {
            this.allowDrag = true;
            this.allowResize = true;
            this.bar = -1;
            this.minPixels = 5;
            this.cursorDrag = Cursors.Hand;
            this.cursorResize = Cursors.SizeWE;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            GanttTool tool = t as GanttTool;
            tool.AllowDrag = this.AllowDrag;
            tool.AllowResize = this.AllowResize;
            tool.CursorDrag = this.CursorDrag;
            tool.CursorResize = this.CursorResize;
            tool.Gantt = this.Gantt;
            tool.MinPixels = this.MinPixels;
        }

        private void MouseDown(MouseEventArgs e, ref Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            this.bar = -1;
            this.xOriginal = base.Chart.axes.Bottom.CalcPosPoint(point.X);
            if (this.allowResize)
            {
                int num = this.Gantt.Pointer.VertSize / 2;
                for (int i = 0; i < this.Gantt.Count; i++)
                {
                    int num2 = this.Gantt.GetVertAxis.CalcPosValue(this.Gantt.YValues[i]);
                    if ((point.Y >= (num2 - num)) && (point.Y <= (num2 + num)))
                    {
                        int num4 = this.Gantt.GetHorizAxis.CalcPosValue(this.Gantt.StartValues[i]);
                        int num5 = this.Gantt.GetHorizAxis.CalcPosValue(this.Gantt.EndValues[i]);
                        if (Math.Abs((int) (point.X - num4)) < this.minPixels)
                        {
                            this.bar = i;
                            this.barPart = GanttBarPart.Start;
                            break;
                        }
                        if (Math.Abs((int) (point.X - num5)) < this.minPixels)
                        {
                            this.bar = i;
                            this.barPart = GanttBarPart.End;
                            break;
                        }
                    }
                }
            }
            if ((this.bar == -1) && this.allowDrag)
            {
                this.bar = this.Gantt.Clicked(point.X, point.Y);
                if (this.bar != -1)
                {
                    this.barPart = GanttBarPart.All;
                }
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (this.Gantt != null)
            {
                if (kind == MouseEventKinds.Up)
                {
                    this.bar = -1;
                }
                else if (kind == MouseEventKinds.Move)
                {
                    this.MouseMove(e, ref c);
                }
                else if (kind == MouseEventKinds.Down)
                {
                    this.MouseDown(e, ref c);
                }
            }
        }

        private void MouseMove(MouseEventArgs e, ref Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            if (this.bar != -1)
            {
                double num4 = this.Gantt.GetHorizAxis.CalcPosPoint(point.X) - this.xOriginal;
                switch (this.barPart)
                {
                    case GanttBarPart.Start:
                        this.Gantt.StartValues.Value[this.bar] += num4;
                        if (this.Gantt.StartValues[this.bar] > this.Gantt.EndValues[this.bar])
                        {
                            this.Gantt.StartValues[this.bar] = this.Gantt.EndValues[this.bar];
                        }
                        if (this.ResizeBar != null)
                        {
                            this.ResizeBar(this, new GanttResizeEventArgs(this.bar, GanttBarPart.Start));
                        }
                        break;

                    case GanttBarPart.All:
                        this.Gantt.StartValues.Value[this.bar] += num4;
                        this.Gantt.EndValues.Value[this.bar] += num4;
                        if (this.DragBar != null)
                        {
                            this.DragBar(this, new GanttDragEventArgs(this.bar));
                        }
                        break;

                    case GanttBarPart.End:
                        this.Gantt.EndValues.Value[this.bar] += num4;
                        if (this.Gantt.EndValues[this.bar] < this.Gantt.StartValues[this.bar])
                        {
                            this.Gantt.EndValues[this.bar] = this.Gantt.StartValues[this.bar];
                        }
                        if (this.ResizeBar != null)
                        {
                            this.ResizeBar(this, new GanttResizeEventArgs(this.bar, GanttBarPart.End));
                        }
                        break;
                }
            }
            else
            {
                if (this.allowResize)
                {
                    int num = this.Gantt.Pointer.VertSize / 2;
                    for (int i = 0; i < this.Gantt.Count; i++)
                    {
                        int num3 = this.Gantt.GetVertAxis.CalcPosValue(this.Gantt.YValues[i]);
                        c = Cursors.Default;
                        if ((point.Y >= (num3 - num)) && (point.Y <= (num3 + num)))
                        {
                            num3 = this.Gantt.GetHorizAxis.CalcPosValue(this.Gantt.StartValues[i]);
                            if (Math.Abs((int) (point.X - num3)) < this.minPixels)
                            {
                                c = this.cursorResize;
                                base.chart.CancelMouse = true;
                                break;
                            }
                            if (Math.Abs((int) (this.Gantt.GetHorizAxis.CalcPosValue(this.Gantt.EndValues[i]) - point.X)) < this.minPixels)
                            {
                                c = this.cursorResize;
                                base.chart.CancelMouse = true;
                                break;
                            }
                        }
                    }
                }
                if ((this.allowDrag && (this.bar == -1)) && (this.Gantt.Clicked(point.X, point.Y) != -1))
                {
                    c = this.cursorDrag;
                    base.chart.CancelMouse = true;
                }
                return;
            }
            this.xOriginal = this.Gantt.GetHorizAxis.CalcPosPoint(point.X);
            this.Gantt.Invalidate();
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

        [Category("Appearance"), DefaultValue(typeof(Cursor), "Hand")]
        public Cursor CursorDrag
        {
            get
            {
                return this.cursorDrag;
            }
            set
            {
                this.cursorDrag = value;
            }
        }

        [DefaultValue(typeof(Cursor), "SizeWE"), Category("Appearance")]
        public Cursor CursorResize
        {
            get
            {
                return this.cursorResize;
            }
            set
            {
                this.cursorResize = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GanttTool;
            }
        }

        public Steema.TeeChart.Styles.Gantt Gantt
        {
            get
            {
                if ((base.iSeries != null) && (base.iSeries is Steema.TeeChart.Styles.Gantt))
                {
                    return (base.iSeries as Steema.TeeChart.Styles.Gantt);
                }
                return null;
            }
            set
            {
                base.iSeries = value;
            }
        }

        [DefaultValue(5)]
        public int MinPixels
        {
            get
            {
                return this.minPixels;
            }
            set
            {
                this.minPixels = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.GanttToolSummary;
            }
        }
    }
}

