namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [Description("Outlines or expands Pie slices when moving or clicking with mouse."), ToolboxBitmap(typeof(SeriesRegionTool), "ToolsIcons.SeriesRegionTool.bmp")]
    public class SeriesRegionTool : ToolSeries
    {
        private bool autobound;
        private bool drawbehindseries;
        private double lbound;
        private double origin;
        private double ubound;
        private bool useorigin;

        public SeriesRegionTool() : this(null)
        {
        }

        public SeriesRegionTool(Chart c) : base(c)
        {
            this.autobound = true;
            this.drawbehindseries = true;
            this.Brush.Color = System.Drawing.Color.White;
        }

        private void DrawRegion()
        {
            if ((base.Active && (base.Chart != null)) && (base.Series != null))
            {
                double minimum = base.Series.notMandatory.Minimum;
                double maximum = base.Series.notMandatory.Maximum;
                if (!this.autobound)
                {
                    minimum = Math.Max(minimum, this.lbound);
                    maximum = Math.Min(maximum, this.ubound);
                }
                if ((maximum > base.Series.notMandatory.Minimum) && (minimum < base.Series.notMandatory.Maximum))
                {
                    double num3;
                    double num4;
                    int a = this.IntersectionPoint(minimum, out num3);
                    int b = this.IntersectionPoint(maximum, out num4);
                    if (b < a)
                    {
                        Utils.SwapInteger(ref a, ref b);
                    }
                    int index = (b - a) + 1;
                    Point[] p = new Point[index + 4];
                    for (int i = 0; i < index; i++)
                    {
                        p[i].X = base.Series.CalcXPos(i + a);
                        p[i].Y = base.Series.CalcYPos(i + a);
                    }
                    p[index].X = base.Series.CalcXPosValue(maximum);
                    p[index].Y = base.Series.CalcYPosValue(num4);
                    p[index + 1].X = p[index].X;
                    p[index + 1].Y = this.useorigin ? base.Series.CalcYPosValue(this.origin) : base.Series.GetVertAxis.IEndPos;
                    p[index + 2].X = base.Series.CalcXPosValue(minimum);
                    p[index + 2].Y = p[index + 1].Y;
                    p[index + 3].X = p[index + 2].X;
                    p[index + 3].Y = base.Series.CalcYPosValue(num3);
                    Graphics3D graphicsd = base.Chart.graphics3D;
                    graphicsd.Brush = this.Brush;
                    graphicsd.Pen = this.Pen;
                    Rectangle r = new Rectangle();
                    int z = this.drawbehindseries ? base.Series.EndZ : base.Series.StartZ;
                    r = graphicsd.RectFromRectZ(base.Chart.ChartRect, z);
                    graphicsd.ClipRectangle(r);
                    graphicsd.Polygon(z, p);
                    graphicsd.UnClip();
                }
            }
        }

        private int IntersectionPoint(double val, out double y)
        {
            int num = 0;
            y = base.Series.mandatory[num];
            while ((val > base.Series.notMandatory[num]) && (num < base.Series.Count))
            {
                num++;
            }
            if (val == base.Series.notMandatory[num])
            {
                y = base.Series.mandatory[num];
                return num;
            }
            if ((num > 0) && (num < base.Series.Count))
            {
                double num2 = base.Series.mandatory[num] - base.Series.mandatory[num - 1];
                num2 /= base.Series.notMandatory[num] - base.Series.notMandatory[num - 1];
                y = base.Series.mandatory[num - 1] + (num2 * (val - base.Series.notMandatory[num - 1]));
            }
            return num;
        }

        protected internal override void SeriesEvent(EventArgs e)
        {
            if ((base.Series != null) && (((e is BeforeDrawSeriesEventArgs) && this.DrawBehindSeries) || ((e is AfterDrawSeriesEventsArgs) && !this.DrawBehindSeries)))
            {
                this.DrawRegion();
            }
        }

        public bool AutoBound
        {
            get
            {
                return this.autobound;
            }
            set
            {
                base.SetBooleanProperty(ref this.autobound, value);
            }
        }

        [Description("Element Brush characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
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

        [Description("Sets region Color."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Color Color
        {
            get
            {
                return this.Brush.Color;
            }
            set
            {
                this.Brush.Color = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.SeriesRegionTool;
            }
        }

        public bool DrawBehindSeries
        {
            get
            {
                return this.drawbehindseries;
            }
            set
            {
                base.SetBooleanProperty(ref this.drawbehindseries, value);
            }
        }

        [Description("Sets region colour gradient."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
        }

        public double LowerBound
        {
            get
            {
                return this.lbound;
            }
            set
            {
                base.SetDoubleProperty(ref this.lbound, value);
            }
        }

        public double Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                base.SetDoubleProperty(ref this.origin, value);
            }
        }

        [Category("Appearance"), Description("Element Pen characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen Pen
        {
            get
            {
                if (base.pPen == null)
                {
                    base.pPen = new ChartPen(base.chart, System.Drawing.Color.Black);
                }
                return base.pPen;
            }
            set
            {
                base.pPen = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.SeriesRegionToolSummary;
            }
        }

        [DefaultValue(0), Description("Sets the Transparency of region as percentage."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance")]
        public int Transparency
        {
            get
            {
                return this.Brush.Transparency;
            }
            set
            {
                this.Brush.Transparency = value;
            }
        }

        public double UpperBound
        {
            get
            {
                return this.ubound;
            }
            set
            {
                base.SetDoubleProperty(ref this.ubound, value);
            }
        }

        public bool UseOrigin
        {
            get
            {
                return this.useorigin;
            }
            set
            {
                base.SetBooleanProperty(ref this.useorigin, value);
            }
        }
    }
}

