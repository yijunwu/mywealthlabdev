namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Description("Grid Band tool, use it to display a coloured rectangles (bands) at the grid lines of the specified axis and position."), ToolboxBitmap(typeof(GridBand), "ToolsIcons.GridBand.bmp")]
    public class GridBand : ToolAxis
    {
        private ChartBrush FBand1;
        private ChartBrush FBand2;
        private ChartBrush tmpBand;

        public GridBand() : this(null)
        {
        }

        public GridBand(Chart c) : base(c)
        {
        }

        protected override void Assign(Tool t)
        {
            base.Assign(t);
            GridBand band = t as GridBand;
            band.Band1 = this.Band1.Clone() as ChartBrush;
            band.Band2 = this.Band2.Clone() as ChartBrush;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (((e is BeforeDrawSeriesEventArgs) && (base.chart != null)) && (base.Axis != null))
            {
                this.DrawGrids();
            }
        }

        private void DrawBand(int tmpPos1, int tmpPos2)
        {
            Rectangle rectangle;
            Rectangle chartRect = base.chart.ChartRect;
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Brush = this.tmpBand;
            bool visible = graphicsd.Pen.Visible;
            graphicsd.Pen.Visible = false;
            if (base.iAxis.horizontal)
            {
                rectangle = Utils.FromLTRB(tmpPos1, chartRect.Top, tmpPos2, chartRect.Bottom);
            }
            else
            {
                rectangle = Utils.FromLTRB(chartRect.Left + 1, tmpPos1, chartRect.Right, tmpPos2 + 1);
            }
            base.chart.Graphics3D.Rectangle(rectangle, base.chart.Aspect.Width3D);
            graphicsd.Pen.Visible = visible;
        }

        private void DrawGrids()
        {
            if (base.Active)
            {
                int tmpNumTicks = base.iAxis.FAxisDraw.tmpNumTicks;
                if (tmpNumTicks > 0)
                {
                    int num;
                    this.tmpBand = this.Band1;
                    if (base.iAxis.horizontal)
                    {
                        if (base.iAxis.Inverted)
                        {
                            if (base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1] < base.iAxis.IEndPos)
                            {
                                this.DrawBand(base.iAxis.IEndPos - 1, base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1]);
                                this.tmpBand = this.Band2;
                            }
                        }
                        else if (base.iAxis.FAxisDraw.tmpTicks[0] < base.iAxis.IEndPos)
                        {
                            this.DrawBand(base.iAxis.IEndPos - 1, base.iAxis.FAxisDraw.tmpTicks[0]);
                            this.tmpBand = this.Band2;
                        }
                    }
                    else if (base.iAxis.Inverted)
                    {
                        if (base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1] > base.iAxis.IStartPos)
                        {
                            this.DrawBand(base.iAxis.IStartPos + 1, base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1]);
                            this.tmpBand = this.Band2;
                        }
                    }
                    else if (base.iAxis.FAxisDraw.tmpTicks[0] > base.iAxis.IStartPos)
                    {
                        this.DrawBand(base.iAxis.IStartPos + 1, base.iAxis.FAxisDraw.tmpTicks[0]);
                        this.tmpBand = this.Band2;
                    }
                    if (base.iAxis.Inverted)
                    {
                        for (num = tmpNumTicks - 1; num > 0; num--)
                        {
                            this.DrawBand(base.iAxis.FAxisDraw.tmpTicks[num - 1], base.iAxis.FAxisDraw.tmpTicks[num]);
                            if (this.tmpBand == this.Band1)
                            {
                                this.tmpBand = this.Band2;
                            }
                            else
                            {
                                this.tmpBand = this.Band1;
                            }
                        }
                    }
                    else
                    {
                        for (num = 1; num < tmpNumTicks; num++)
                        {
                            this.DrawBand(base.iAxis.FAxisDraw.tmpTicks[num - 1], base.iAxis.FAxisDraw.tmpTicks[num]);
                            if (this.tmpBand == this.Band1)
                            {
                                this.tmpBand = this.Band2;
                            }
                            else
                            {
                                this.tmpBand = this.Band1;
                            }
                        }
                    }
                    if (base.iAxis.horizontal)
                    {
                        if (!base.iAxis.Inverted)
                        {
                            if (base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1] > base.iAxis.IStartPos)
                            {
                                this.DrawBand(base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1], base.iAxis.IStartPos);
                            }
                        }
                        else if (base.iAxis.FAxisDraw.tmpTicks[0] > base.iAxis.IStartPos)
                        {
                            this.DrawBand(base.iAxis.FAxisDraw.tmpTicks[0], base.iAxis.IStartPos);
                        }
                    }
                    else if (base.iAxis.Inverted)
                    {
                        if (base.iAxis.FAxisDraw.tmpTicks[0] < base.iAxis.IEndPos)
                        {
                            this.DrawBand(base.iAxis.FAxisDraw.tmpTicks[0], base.iAxis.IEndPos);
                        }
                    }
                    else if (base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1] < base.iAxis.IEndPos)
                    {
                        this.DrawBand(base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1], base.iAxis.IEndPos);
                    }
                }
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The Brush characteristics of the first GridBand tool Band.")]
        public ChartBrush Band1
        {
            get
            {
                if (this.FBand1 == null)
                {
                    this.FBand1 = new ChartBrush(base.chart, Color.Black);
                }
                return this.FBand1;
            }
            set
            {
                this.FBand1 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("The Brush characteristics of the second GridBand tool Band.")]
        public ChartBrush Band2
        {
            get
            {
                if (this.FBand2 == null)
                {
                    this.FBand2 = new ChartBrush(base.chart, Color.Black);
                }
                return this.FBand2;
            }
            set
            {
                this.FBand2 = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GridBandTool;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.GridBandSummary;
            }
        }
    }
}

