namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(PieTool), "ToolsIcons.PieTool.bmp"), Description("Outlines or expands Pie slices when moving or clicking with mouse.")]
    public class PieTool : ToolSeries
    {
        private int iSlice;
        private PieToolStyle style;

        public PieTool() : this(null)
        {
        }

        public PieTool(Chart c) : base(c)
        {
            this.iSlice = -1;
            this.Pen.Width = 3;
        }

        protected override void Assign(Steema.TeeChart.Tools.Tool t)
        {
            base.Assign(t);
            PieTool tool = t as PieTool;
            tool.Pen = this.Pen.Clone() as ChartPen;
            tool.Style = this.Style;
        }

        private void FocusSlice(int valueIndex, bool focused)
        {
            Pie series = (Pie) base.Series;
            if (this.style == PieToolStyle.Explode)
            {
                int num = focused ? 1 : -1;
                for (int i = 1; i <= 20; i++)
                {
                    series.ExplodedSlice[valueIndex] = (2 * i) * num;
                    base.Chart.Invalidate();
                }
            }
            else if (!focused)
            {
                base.Chart.Invalidate();
            }
            else
            {
                Color white = this.Pen.Color;
                if (base.Series.ValueColor(valueIndex) == this.Pen.Color)
                {
                    if (this.Pen.Color == Color.Black)
                    {
                        white = Color.White;
                    }
                    else
                    {
                        white = Color.Black;
                    }
                }
                if (!base.chart.graphics3D.ValidState())
                {
                    Control control = base.chart.parent.GetControl();
                    if ((control == null) || (base.chart.graphics3D.CanvasType != CanvasType.GDIplus))
                    {
                        return;
                    }
                    ((Graphics3DGdiPlus) base.chart.graphics3D).g = control.CreateGraphics();
                }
                base.chart.graphics3D.Brush.Color = base.Series.ValueColor(valueIndex);
                base.chart.graphics3D.Pen = this.Pen;
                base.chart.graphics3D.Pen.Color = white;
                series.DrawPie(valueIndex);
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            Point point = new Point(e.X, e.Y);
            if ((kind == MouseEventKinds.Move) && (base.Series != null))
            {
                int valueIndex = base.Series.Clicked(point.X, point.Y);
                if (this.iSlice != valueIndex)
                {
                    if (this.iSlice != -1)
                    {
                        this.FocusSlice(this.iSlice, false);
                    }
                    this.iSlice = valueIndex;
                    if (this.iSlice != -1)
                    {
                        this.FocusSlice(this.iSlice, true);
                    }
                }
                else if ((valueIndex != -1) && (this.Style == PieToolStyle.Focus))
                {
                    this.FocusSlice(valueIndex, true);
                }
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.PieTool;
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

        public int Slice
        {
            get
            {
                return this.iSlice;
            }
        }

        [DefaultValue(0)]
        public PieToolStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.PieToolSummary;
            }
        }
    }
}

