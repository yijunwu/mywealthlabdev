namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [ToolboxBitmap(typeof(Histogram), "SeriesIcons.Histogram.bmp")]
    public class Histogram : BaseLine
    {
        private ChartPen linesPen;
        protected internal float previous;

        public Histogram() : this(null)
        {
        }

        public Histogram(Chart c) : base(c)
        {
            base.calcVisiblePoints = false;
            base.LinePen.Color = Color.Black;
        }

        internal override void CalcFirstLastVisibleIndex()
        {
            base.CalcFirstLastVisibleIndex();
            if (base.FirstVisibleIndex > 0)
            {
                base.firstVisible = -1;
            }
            if (base.LastVisibleIndex < (base.Count - 1))
            {
                base.lastVisible = 1;
            }
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            float num = this.VisiblePoints();
            if (num > 0f)
            {
                num = ((float) (base.GetHorizAxis.IAxisSize / this.VisiblePoints())) / 2f;
            }
            LeftMargin += Utils.Round(num);
            RightMargin += Utils.Round(num);
            if (base.LinePen.Visible)
            {
                RightMargin += Utils.Round((float) base.LinePen.Width);
            }
        }

        protected internal virtual RectangleF CalcRectangle(int valueIndex)
        {
            float num;
            RectangleF r = new RectangleF();
            if (this.VisiblePoints() <= 0)
            {
                return new RectangleF(0f, 0f, 0f, 0f);
            }
            if (this.VisiblePoints() > 1)
            {
                if (valueIndex == base.FirstDisplayedIndex())
                {
                    num = ((float) (this.CalcXPos(valueIndex + 1) - this.CalcXPos(valueIndex))) / 2f;
                }
                else
                {
                    num = ((float) (this.CalcXPos(valueIndex) - this.CalcXPos(valueIndex - 1))) / 2f;
                }
            }
            else
            {
                num = base.GetHorizAxis.IAxisSize / this.VisiblePoints();
            }
            if (valueIndex == base.FirstDisplayedIndex())
            {
                float a = this.CalcXPos(valueIndex) - num;
                float b = num * 2f;
                if (!this.DrawValuesForward())
                {
                    Utils.SwapFloat(ref a, ref b);
                }
                r.Width = b;
                r.X = a;
            }
            else
            {
                r.X = this.previous;
                if (this.DrawValuesForward())
                {
                    r.Width = (this.CalcXPos(valueIndex) + num) - r.X;
                }
                else
                {
                    r.Width = (this.CalcXPos(valueIndex) - num) - r.X;
                }
            }
            this.previous = r.Right;
            r.Y = this.CalcYPos(valueIndex);
            r.Height = base.GetVertAxis.Inverted ? (base.GetVertAxis.IStartPos - r.Y) : (base.GetVertAxis.IEndPos - r.Y);
            return base.chart.Graphics3D.CalcRect3D(r, base.MiddleZ);
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            if (base.LinePen.Visible)
            {
                TopMargin += Utils.Round((float) base.LinePen.Width);
            }
        }

        public override int Clicked(int x, int y)
        {
            int num = base.Clicked(x, y);
            if ((base.FirstVisibleIndex != -1) && (base.LastVisibleIndex != -1))
            {
                if (base.Chart != null)
                {
                    base.Chart.Graphics3D.Calculate2DPosition(ref x, ref y, base.MiddleZ);
                }
                for (int i = base.FirstVisibleIndex; i <= base.LastVisibleIndex; i++)
                {
                    if (this.CalcRectangle(i).Contains((float) x, (float) y))
                    {
                        return i;
                    }
                }
            }
            return num;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Hollow);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.Lines);
            AddSubChart(Texts.Transparency);
        }

        public override void DrawValue(int valueIndex)
        {
            RectangleF r = this.CalcRectangle(valueIndex);
            Graphics3D graphicsd = base.chart.graphics3D;
            if (this.Brush.Visible)
            {
                graphicsd.Brush = this.Brush;
                Color color = this.ValueColor(valueIndex);
                if (this.Transparency > 0)
                {
                    color = Graphics3D.TransparentColor(this.Transparency, color);
                }
                graphicsd.Brush.Color = color;
                bool visible = graphicsd.Pen.Visible;
                graphicsd.Pen.Visible = false;
                if (base.GetVertAxis.Inverted)
                {
                    r.Y++;
                }
                if ((graphicsd.SmoothingMode == SmoothingMode.HighQuality) || (graphicsd.SmoothingMode == SmoothingMode.AntiAlias))
                {
                    graphicsd.RectangleF(new RectangleF(r.X, r.Y, r.Width + 2f, r.Height + 2f));
                }
                else
                {
                    graphicsd.RectangleF(r);
                }
                graphicsd.Pen.Visible = visible;
                if (base.GetVertAxis.Inverted)
                {
                    r.Y--;
                }
            }
            if (base.LinePen.Visible)
            {
                graphicsd.Pen = base.LinePen;
                if (base.yMandatory)
                {
                    if (valueIndex == base.firstVisible)
                    {
                        this.VerticalLineF(r.X, r.Bottom, r.Y);
                    }
                    else
                    {
                        this.VerticalLineF(r.X, r.Y, this.CalcYPos(valueIndex - 1));
                    }
                    this.HorizLineF(r.X, r.Right, r.Y);
                    if (valueIndex == base.lastVisible)
                    {
                        this.VerticalLineF(r.Right - 1f, r.Y, r.Bottom);
                    }
                }
                else
                {
                    if (valueIndex == base.firstVisible)
                    {
                        this.HorizLineF(r.Left, r.Right - 1f, r.Bottom);
                    }
                    else
                    {
                        this.HorizLineF(this.CalcXPos(valueIndex - 1) - 1f, r.Right - 1f, r.Bottom);
                    }
                    this.VerticalLineF(r.Right - base.LinePen.Width, r.Top, r.Bottom);
                    if (valueIndex == base.lastVisible)
                    {
                        this.HorizLineF(r.Left, r.Right - 1f, r.Top);
                    }
                }
            }
            if (((valueIndex > base.firstVisible) && (this.linesPen != null)) && this.linesPen.Visible)
            {
                if (base.yMandatory)
                {
                    int num = this.CalcYPos(valueIndex - 1);
                    num = base.GetVertAxis.Inverted ? Math.Min(Utils.Round(r.Y), num) : Math.Max(Utils.Round(r.Y), num);
                    if (!base.LinePen.Visible)
                    {
                        num--;
                    }
                    graphicsd.Pen = this.linesPen;
                    this.VerticalLineF(r.X, r.Bottom, num);
                }
                else
                {
                    int num2 = this.CalcXPos(valueIndex - 1);
                    num2 = base.GetHorizAxis.Inverted ? Math.Min(Utils.Round(r.Right), num2) : Math.Max(Utils.Round(r.Left), num2);
                    if (!base.LinePen.Visible)
                    {
                        num2--;
                    }
                    graphicsd.Pen = this.linesPen;
                    this.HorizLineF(r.Left, (float) num2, r.Bottom);
                }
            }
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        private void HorizLine(int X0, int X1, int Y)
        {
            base.chart.graphics3D.HorizontalLine(X0, X1, Y);
        }

        private void HorizLineF(float X0, float X1, int Y)
        {
            base.chart.graphics3D.HorizontalLine(Utils.Round(X0), Utils.Round(X1), Y);
        }

        private void HorizLineF(float X0, float X1, float Y)
        {
            this.HorizLineF(X0, X1, Utils.Round(Y));
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            if (!base.ColorEach)
            {
                Color color2 = Utils.DarkenColor(color, 60);
                base.LinePen.Color = color2;
                this.LinesPen.Color = color2;
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.linesPen != null)
            {
                this.linesPen.Chart = c;
            }
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.Brush.Visible = false;
                    return;

                case 2:
                    base.LinePen.Visible = false;
                    return;

                case 3:
                    this.LinesPen.Visible = true;
                    return;

                case 4:
                    this.Transparency = 30;
                    return;
            }
            base.SetSubGallery(index);
        }

        private void VerticalLine(int X, int Y0, int Y1)
        {
            base.chart.graphics3D.VerticalLine(X, Y0, Y1);
        }

        private void VerticalLineF(float X, float Y0, int Y1)
        {
            base.chart.graphics3D.VerticalLine(Utils.Round(X), Utils.Round(Y0), Y1);
        }

        private void VerticalLineF(float X, float Y0, float Y1)
        {
            this.VerticalLineF(X, Y0, Utils.Round(Y1));
        }

        protected internal int VisiblePoints()
        {
            int maxPointsPerPage = base.chart.Page.MaxPointsPerPage;
            if (maxPointsPerPage != 0)
            {
                return maxPointsPerPage;
            }
            return base.Count;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryHistogram;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen LinesPen
        {
            get
            {
                if (this.linesPen == null)
                {
                    this.linesPen = new ChartPen(base.chart, Color.Black);
                }
                return this.linesPen;
            }
        }

        [DefaultValue(0), Description("Sets Transparency level from 0 to 100%."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance")]
        public int Transparency
        {
            get
            {
                return base.bBrush.Transparency;
            }
            set
            {
                base.bBrush.Transparency = value;
            }
        }
    }
}

