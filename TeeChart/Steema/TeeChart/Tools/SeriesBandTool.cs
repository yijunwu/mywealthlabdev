namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(SeriesBandTool), "ToolsIcons.SeriesBandTool.bmp"), Description("Use it to display a band between two (line) series.")]
    public class SeriesBandTool : ToolSeries
    {
        private double boundValue;
        private bool drawBehindSeries;
        private bool iSerie1Drawed;
        private bool iSerie2Drawed;
        private Series series2;

        public SeriesBandTool() : this(null)
        {
        }

        public SeriesBandTool(Chart c) : base(c)
        {
            this.drawBehindSeries = true;
            this.Brush.Color = Color.White;
            this.Pen.Visible = false;
        }

        protected virtual void AfterDrawValues(object sender, Graphics3D g)
        {
            if (!this.drawBehindSeries)
            {
                if ((base.Series != null) && (sender == base.Series))
                {
                    this.iSerie1Drawed = true;
                }
                if ((this.Series2 != null) && (sender == this.Series2))
                {
                    this.iSerie2Drawed = true;
                }
                if (this.iSerie1Drawed && (this.iSerie2Drawed || (this.Series2 == null)))
                {
                    this.DrawBandTool();
                }
            }
        }

        protected virtual void BeforeDrawValues(object sender, Graphics3D g)
        {
            if (this.drawBehindSeries)
            {
                if ((base.Series != null) && (sender == base.Series))
                {
                    this.iSerie1Drawed = true;
                }
                if ((this.Series2 != null) && (sender == this.Series2))
                {
                    this.iSerie2Drawed = true;
                }
                if (this.Series2 != null)
                {
                    if (!this.iSerie1Drawed || !this.iSerie2Drawed)
                    {
                        this.DrawBandTool();
                    }
                }
                else
                {
                    this.DrawBandTool();
                }
            }
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (e is BeforeDrawSeriesEventArgs)
            {
                this.iSerie1Drawed = false;
                this.iSerie2Drawed = false;
            }
        }

        protected virtual void DrawBandTool()
        {
            int z = 0;
            Graphics3D graphicsd = base.Chart.Graphics3D;
            if ((base.Active && (base.Chart != null)) && (base.Series != null))
            {
                base.Series.CalcFirstLastVisibleIndex();
                if (this.Series2 != null)
                {
                    this.Series2.CalcFirstLastVisibleIndex();
                }
                if ((((this.Series2 != null) && (base.Series.FirstVisibleIndex != -1)) && (this.Series2.FirstVisibleIndex != -1)) || (base.Series.FirstVisibleIndex != -1))
                {
                    int num2;
                    int num = (base.Series.LastVisibleIndex - base.Series.FirstVisibleIndex) + 1;
                    if (base.Series.DrawBetweenPoints && (base.Series.FirstVisibleIndex > 0))
                    {
                        num++;
                    }
                    if (this.Series2 != null)
                    {
                        num2 = (this.Series2.LastVisibleIndex - this.Series2.FirstVisibleIndex) + 1;
                        if (base.Series.DrawBetweenPoints && (this.Series2.FirstVisibleIndex > 0))
                        {
                            num2++;
                        }
                    }
                    else
                    {
                        num2 = 2;
                    }
                    Point[] p = new Point[num + num2];
                    if (p != null)
                    {
                        int index = 0;
                        if (base.Series.FirstVisibleIndex != -1)
                        {
                            for (int i = Math.Max(0, base.Series.FirstVisibleIndex - 1); i <= base.Series.LastVisibleIndex; i++)
                            {
                                p[index].X = base.Series.CalcXPos(i);
                                p[index].Y = base.Series.CalcYPos(i);
                                index++;
                            }
                        }
                        if (this.Series2 != null)
                        {
                            if (this.Series2.FirstVisibleIndex != -1)
                            {
                                int num5 = Math.Max(0, this.Series2.FirstVisibleIndex - 1);
                                for (int j = this.Series2.LastVisibleIndex; j >= num5; j--)
                                {
                                    p[index].X = this.Series2.CalcXPos(j);
                                    p[index].Y = this.Series2.CalcYPos(j);
                                    index++;
                                }
                            }
                        }
                        else
                        {
                            p[index].X = p[index - 1].X;
                            p[index].Y = base.Series.CalcYPosValue(this.boundValue);
                            p[index + 1].X = p[0].X;
                            p[index + 1].Y = p[index].Y;
                        }
                        if (this.Series2 != null)
                        {
                            z = Math.Max(base.Series.StartZ, this.Series2.StartZ);
                        }
                        else
                        {
                            z = base.Series.StartZ;
                        }
                        graphicsd.Brush = this.Brush;
                        graphicsd.Pen = this.Pen;
                        graphicsd.ClipCube(graphicsd.Chart.ChartRect, 0, graphicsd.Chart.Aspect.Width3D);
                        graphicsd.Polygon(z, p);
                        graphicsd.UnClip();
                    }
                }
            }
        }

        private void SetEvents(Series aSeries)
        {
            if (aSeries != null)
            {
                aSeries.AfterDrawValues -= new PaintChartEventHandler(this.AfterDrawValues);
                aSeries.BeforeDrawValues -= new PaintChartEventHandler(this.BeforeDrawValues);
                aSeries.AfterDrawValues += new PaintChartEventHandler(this.AfterDrawValues);
                aSeries.BeforeDrawValues += new PaintChartEventHandler(this.BeforeDrawValues);
            }
        }

        protected override void SetSeries(Series value)
        {
            base.SetSeries(value);
            this.SetEvents(base.Series);
        }

        protected virtual void SetSeries2(Series value)
        {
            if (this.series2 != value)
            {
                this.series2 = value;
                this.SetEvents(this.series2);
            }
        }

        [Description("Gets and sets a constant value to be used as one of the limits of band filled areas.")]
        public double BoundValue
        {
            get
            {
                return this.boundValue;
            }
            set
            {
                this.boundValue = value;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Brush characteristics.")]
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
                return Texts.SeriesBandTool;
            }
        }

        [Description("Gets and sets a flag which causes filling to occur before or after the series are displayed."), DefaultValue(true)]
        public bool DrawBehindSeries
        {
            get
            {
                return this.drawBehindSeries;
            }
            set
            {
                if (this.drawBehindSeries != value)
                {
                    this.drawBehindSeries = value;
                    this.Invalidate();
                }
            }
        }

        [Description("The gradient colors used to fill the area between the two series."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
            set
            {
                this.Brush.Gradient = value;
            }
        }

        [Description("Element Pen characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
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

        [Description("Gets and sets the second series associated to this tool.")]
        public Series Series2
        {
            get
            {
                return this.series2;
            }
            set
            {
                this.SetSeries2(value);
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.SeriesBandToolSummary;
            }
        }

        [Description("The amount of semi-glass effect (opacity) to apply when filling the area between the two series."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    }
}

