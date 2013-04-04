namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Export;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;

    public class ScrollTool : ZoomTool
    {
        private ScrollMouseAction mouseAction;
        private ScrollAttributes scrollAtts;
        private const int SCROLLBARHEIGHT = 0x10;
        private ScrollToolViewUnit scrollSegmentUnits;
        private int startPosition;
        private int viewSegment;

        public ScrollTool() : this(null)
        {
        }

        public ScrollTool(Chart c) : base(c)
        {
            this.viewSegment = 20;
            this.mouseAction = ScrollMouseAction.drag;
            this.startPosition = 1;
            this.scrollAtts = new ScrollAttributes();
            base.Chart = c;
            if (base.Chart != null)
            {
                base.Chart.Aspect.View3D = false;
                base.Chart.Axes.Bottom.PositionUnits = PositionUnits.Pixels;
                base.Chart.Axes.Bottom.RelativePosition = -16.0;
                base.Chart.Panel.MarginBottom = 8.0;
            }
        }

        public string GenerateImageList(ref MemoryStream chartImg, ref int i, ImageExportFormat format, string fileRoot)
        {
            this.Attributes.frames.Add(this.Attributes.newChartRect);
            if ((this.Attributes.newWidth - (this.Attributes.newChartRect.X - this.Attributes.newLeftWall)) < this.Attributes.sourceRect.Width)
            {
                Rectangle srcRect = new Rectangle(this.Attributes.newChartRect.X, this.Attributes.newChartRect.Y, (this.Attributes.newWidth - (this.Attributes.newChartRect.X - this.Attributes.newLeftWall)) - 1, this.Attributes.newChartRect.Height);
                Rectangle destRect = new Rectangle(0, 0, this.Attributes.newWidth - (this.Attributes.newChartRect.X - this.Attributes.newLeftWall), this.Attributes.sourceRect.Height);
                Bitmap image = new Bitmap(destRect.Width, this.Attributes.destRect.Height);
                Graphics.FromImage(image).DrawImage(this.Attributes.b, destRect, srcRect, GraphicsUnit.Pixel);
                format.Save(chartImg, image, destRect.Width, this.Attributes.sourceRect.Height);
            }
            else
            {
                Bitmap bitmap2 = new Bitmap(this.Attributes.destRect.Width, this.Attributes.destRect.Height);
                Graphics.FromImage(bitmap2).DrawImage(this.Attributes.b, this.Attributes.destRect, this.Attributes.newChartRect, GraphicsUnit.Pixel);
                format.Save(chartImg, bitmap2, this.Attributes.sourceRect.Width, this.Attributes.sourceRect.Height);
            }
            this.Attributes.newChartRect.X += this.Attributes.newChartRect.Width;
            this.Attributes.imgList.Add("img" + ((int) i).ToString(), chartImg);
            string str = fileRoot + ((int) i).ToString();
            i++;
            this.Attributes.scrollPages = i;
            return str;
        }

        private int getPercent(double val)
        {
            double num = base.Chart.Axes.Bottom.CalcXPosValue(val);
            return Convert.ToInt32(Math.Round((double) ((num / ((double) base.Chart.Width)) * 100.0)));
        }

        public void initScrollVars(Chart chart)
        {
            this.setScrollPages();
            this.scrollAtts.mouseAction = Convert.ToInt32(this.mouseAction);
            Rectangle chartRect = chart.ChartRect;
            this.scrollAtts.oldWidth = chart.Width;
            this.scrollAtts.oldHeight = chart.Height;
            chart.Width *= this.scrollAtts.scrollPages;
            this.scrollAtts.sourceRect = new Rectangle(chartRect.Left + 1, chartRect.Top, chartRect.Width - 1, chartRect.Height);
            this.scrollAtts.b = chart.Bitmap();
            chartRect = chart.ChartRect;
            this.scrollAtts.newChartRect = new Rectangle(chartRect.Left + 2, chartRect.Top, chartRect.Width - 2, chartRect.Height);
            this.scrollAtts.newWidth = this.scrollAtts.newChartRect.Width;
            this.scrollAtts.newLeftWall = this.scrollAtts.newChartRect.X;
            this.scrollAtts.newChartRect.Width = this.scrollAtts.sourceRect.Width;
            this.scrollAtts.rightAxisMargin = this.scrollAtts.newWidth - (chart.Axes.Bottom.CalcXPosValue(chart.Axes.Bottom.Maximum) - this.scrollAtts.newLeftWall);
            chart.Axes.Bottom.PositionUnits = PositionUnits.Pixels;
            chart.Axes.Bottom.RelativePosition = -16.0;
            this.scrollAtts.bottomAxisPos = (chart.Axes.Bottom.Position - this.scrollAtts.sourceRect.Y) - 3;
            this.scrollAtts.newChartRect.Height = (this.scrollAtts.oldHeight - this.scrollAtts.sourceRect.Y) - 3;
            this.scrollAtts.sourceRect.Height = this.scrollAtts.newChartRect.Height;
            this.scrollAtts.renderRect = this.scrollAtts.sourceRect;
            this.scrollAtts.grabRect = this.scrollAtts.newChartRect;
            this.scrollAtts.destRect = new Rectangle(0, 0, this.scrollAtts.sourceRect.Width, this.scrollAtts.sourceRect.Height);
            new Bitmap(this.scrollAtts.destRect.Width, this.scrollAtts.destRect.Height);
        }

        private void setScrollPages()
        {
            if (this.scrollSegmentUnits == ScrollToolViewUnit.percent)
            {
                if (this.startPosition > 100)
                {
                    this.Attributes.startPos = 100;
                }
                else if (this.startPosition < 1)
                {
                    this.Attributes.startPos = 1;
                }
                else
                {
                    this.Attributes.startPos = this.startPosition;
                }
                this.Attributes.scrollPages = Utils.Round((double) (100.0 / ((double) this.viewSegment)));
            }
            else
            {
                if (this.startPosition > base.Chart.Axes.Bottom.Maximum)
                {
                    this.Attributes.startPos = this.getPercent(base.Chart.Axes.Bottom.Maximum);
                }
                else if (this.startPosition < base.Chart.Axes.Bottom.Minimum)
                {
                    this.Attributes.startPos = this.getPercent(base.Chart.Axes.Bottom.Minimum);
                }
                else
                {
                    this.Attributes.startPos = this.getPercent((double) this.startPosition);
                }
                if (this.viewSegment < (base.Chart.Axes.Bottom.Maximum - base.Chart.Axes.Bottom.Minimum))
                {
                    this.Attributes.scrollPages = Utils.Round((double) ((base.Chart.Axes.Bottom.Maximum - base.Chart.Axes.Bottom.Minimum) / ((double) this.viewSegment)));
                }
            }
        }

        public ScrollAttributes Attributes
        {
            get
            {
                return this.scrollAtts;
            }
            set
            {
                this.scrollAtts = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ScrollTool;
            }
        }

        public ScrollMouseAction MouseAction
        {
            get
            {
                return this.mouseAction;
            }
            set
            {
                if (this.mouseAction != value)
                {
                    this.mouseAction = value;
                }
            }
        }

        public ScrollToolViewUnit SegmentViewUnits
        {
            get
            {
                return this.scrollSegmentUnits;
            }
            set
            {
                if (this.scrollSegmentUnits != value)
                {
                    this.scrollSegmentUnits = value;
                }
            }
        }

        public int StartPosition
        {
            get
            {
                return this.startPosition;
            }
            set
            {
                if (this.startPosition != value)
                {
                    this.startPosition = value;
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.ScrollToolSummary;
            }
        }

        public int ViewSegmentSize
        {
            get
            {
                return this.viewSegment;
            }
            set
            {
                if (this.viewSegment != value)
                {
                    this.viewSegment = value;
                }
            }
        }
    }
}

