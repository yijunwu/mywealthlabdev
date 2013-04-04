namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(NearestPoint), "ToolsIcons.NearestPoint.bmp"), Description("Displays a graphical signal when the mouse is moving near a series point.")]
    public class NearestPoint : ToolSeries
    {
        private bool drawLine;
        private bool fullRepaint;
        private System.Drawing.Point IMouse;
        public int Point;
        private int size;
        private NearestPointStyles style;

        public event EventHandler Change;

        public NearestPoint() : this((Chart) null)
        {
        }

        public NearestPoint(Chart c) : base(c)
        {
            this.drawLine = true;
            this.fullRepaint = true;
            this.size = 20;
            this.style = NearestPointStyles.Circle;
            this.Point = -1;
            this.Pen.Style = DashStyle.Dot;
            this.Pen.defaultStyle = DashStyle.Dot;
            this.Pen.Color = Color.White;
        }

        public NearestPoint(Series s) : this(s.chart)
        {
            base.iSeries = s;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            NearestPoint point = t as NearestPoint;
            point.Brush = this.Brush.Clone() as ChartBrush;
            point.DrawLine = this.DrawLine;
            point.FullRepaint = this.FullRepaint;
            point.Pen = this.Pen.Clone() as ChartPen;
            point.Style = this.Style;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (e is AfterDrawEventArgs)
            {
                this.PaintHint();
            }
        }

        private int GetNearestPoint(System.Drawing.Point p)
        {
            int num;
            int num2;
            int num3 = -1;
            int num4 = 0x2710;
            if (Steema.TeeChart.Tools.Tool.GetFirstLastSeries(base.iSeries, out num, out num2))
            {
                for (int i = num; i <= num2; i++)
                {
                    int x = base.iSeries.CalcXPos(i);
                    int y = base.iSeries.CalcYPos(i);
                    if (base.chart.ChartRect.Contains(x, y))
                    {
                        int num8 = Utils.Round(Math.Sqrt(Utils.Sqr((double) (p.X - x)) + Utils.Sqr((double) (p.Y - y))));
                        if (num8 < num4)
                        {
                            num4 = num8;
                            num3 = i;
                        }
                    }
                }
            }
            return num3;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if ((kind == MouseEventKinds.Move) && (base.iSeries != null))
            {
                this.IMouse = new System.Drawing.Point(e.X, e.Y);
                if (!this.fullRepaint)
                {
                    this.PaintHint();
                }
                this.Point = this.GetNearestPoint(this.IMouse);
                if (!this.fullRepaint)
                {
                    this.PaintHint();
                }
                if (this.Change != null)
                {
                    this.Change(this, EventArgs.Empty);
                }
                if (this.fullRepaint)
                {
                    this.Invalidate();
                }
            }
        }

        private void PaintHint()
        {
            if (((base.iSeries != null) && (this.Point != -1)) && (base.iSeries.Count > 0))
            {
                Graphics3D graphicsd = base.chart.graphics3D;
                graphicsd.Pen = this.Pen;
                int x = base.iSeries.CalcXPos(this.Point);
                int y = base.iSeries.CalcYPos(this.Point);
                if (this.style != NearestPointStyles.None)
                {
                    graphicsd.Brush = this.Brush;
                    Rectangle r = Rectangle.FromLTRB(x - this.size, y - this.size, x + this.size, y + this.size);
                    switch (this.style)
                    {
                        case NearestPointStyles.Circle:
                            if (!base.chart.aspect.view3D)
                            {
                                graphicsd.Ellipse(r);
                                break;
                            }
                            graphicsd.Ellipse(r, base.iSeries.StartZ);
                            break;

                        case NearestPointStyles.Rectangle:
                            if (!base.chart.aspect.view3D)
                            {
                                graphicsd.Rectangle(r);
                                break;
                            }
                            graphicsd.Rectangle(r, base.iSeries.StartZ);
                            break;

                        case NearestPointStyles.Diamond:
                        {
                            System.Drawing.Point[] p = new System.Drawing.Point[] { new System.Drawing.Point(x, y - this.size), new System.Drawing.Point(x + this.size, y), new System.Drawing.Point(x, y + this.size), new System.Drawing.Point(x - this.size, y) };
                            graphicsd.Polygon(base.iSeries.StartZ, p);
                            break;
                        }
                    }
                }
                if (this.drawLine)
                {
                    graphicsd.Pen.Style = DashStyle.Solid;
                    graphicsd.MoveTo(this.IMouse);
                    graphicsd.LineTo(x, y);
                }
            }
        }

        [Description("Element Brush characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                if (base.bBrush == null)
                {
                    base.bBrush = new ChartBrush(base.chart, false);
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
                return Texts.NearestTool;
            }
        }

        [Description("Draws temporary line from mouse coordinates to nearest point."), DefaultValue(true)]
        public bool DrawLine
        {
            get
            {
                return this.drawLine;
            }
            set
            {
                base.SetBooleanProperty(ref this.drawLine, value);
            }
        }

        [DefaultValue(true), Description("Allows the whole Parent Chart to repainted when true.")]
        public bool FullRepaint
        {
            get
            {
                return this.fullRepaint;
            }
            set
            {
                base.SetBooleanProperty(ref this.fullRepaint, value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Pen characteristics.")]
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

        [DefaultValue(20), Description("Defines the Size of the NearestTool shape.")]
        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                base.SetIntegerProperty(ref this.size, value);
            }
        }

        [DefaultValue(1), Description("Sets the shape of the NearestTool.")]
        public NearestPointStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.NearestPointSummary;
            }
        }
    }
}

