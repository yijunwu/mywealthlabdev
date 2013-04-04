namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Candle), "SeriesIcons.Candle.bmp")]
    public class Candle : OHLC
    {
        private int candleWidth;
        private const int DefaultCandleWidth = 4;
        private Color downCloseColor;
        private ChartPen highLowPen;
        private Point OldP;
        private bool showCloseTick;
        private bool showOpenTick;
        private CandleStyles style;
        private Color upCloseColor;

        public Candle() : this(null)
        {
        }

        public Candle(Chart c) : base(c)
        {
            this.upCloseColor = Color.White;
            this.downCloseColor = Color.Red;
            this.candleWidth = 4;
            this.showOpenTick = true;
            this.showCloseTick = true;
            base.Pointer.Draw3D = false;
        }

        private Color CalculateColor(int valueIndex)
        {
            Color color = this.ValueColor(valueIndex);
            if (!(color == base.Color))
            {
                return color;
            }
            if (base.vOpenValues[valueIndex] > base.CloseValues[valueIndex])
            {
                return this.downCloseColor;
            }
            if (base.vOpenValues[valueIndex] >= base.CloseValues[valueIndex])
            {
                if (valueIndex == 0)
                {
                    return this.upCloseColor;
                }
                if (base.CloseValues[valueIndex - 1] > base.CloseValues[valueIndex])
                {
                    return this.downCloseColor;
                }
                if (base.CloseValues[valueIndex - 1] < base.CloseValues[valueIndex])
                {
                    return this.upCloseColor;
                }
            }
            return this.upCloseColor;
        }

        private void CheckHighLowPen(Graphics3D g)
        {
            if (this.HighLowPen.Color == Utils.EmptyColor)
            {
                this.HighLowPen.Color = this.Pen.Color;
            }
            g.Pen = this.HighLowPen;
        }

        public override int Clicked(int x, int y)
        {
            if ((base.firstVisible > -1) && (base.lastVisible > -1))
            {
                if (base.chart != null)
                {
                    base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
                }
                Point p = new Point(x, y);
                for (int i = base.firstVisible; i <= base.lastVisible; i++)
                {
                    if (this.ClickedCandle(i, p))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public bool ClickedCandle(int valueIndex, Point p)
        {
            bool flag = false;
            int px = base.CalcXPosValue(base.DateValues[valueIndex]);
            int top = base.CalcYPosValue(base.vOpenValues[valueIndex]);
            int qy = base.CalcYPosValue(base.vHighValues[valueIndex]);
            int num4 = base.CalcYPosValue(base.vLowValues[valueIndex]);
            int bottom = base.CalcYPosValue(base.CloseValues[valueIndex]);
            int num6 = this.candleWidth / 2;
            int num7 = this.candleWidth - num6;
            Point point = new Point();
            if ((this.style == CandleStyles.CandleStick) || (this.style == CandleStyles.OpenClose))
            {
                if (base.chart.aspect.view3D && base.Pointer.Draw3D)
                {
                    int a = bottom;
                    int b = top;
                    if (a > b)
                    {
                        Utils.SwapInteger(ref a, ref b);
                    }
                    if (((this.style != CandleStyles.CandleStick) || (!Graphics3D.PointInLineTolerance(p, px, b, px, num4, 3) && !Graphics3D.PointInLineTolerance(p, px, a, px, qy, 3))) && !Utils.FromLTRB(px - num6, a, px + num7, b).Contains(p.X, p.Y))
                    {
                        return flag;
                    }
                    return true;
                }
                if ((this.style != CandleStyles.CandleStick) || !Graphics3D.PointInLineTolerance(p, px, num4, px, qy, 3))
                {
                    if (top == bottom)
                    {
                        bottom--;
                    }
                    if (base.chart.aspect.view3D)
                    {
                        return (Utils.FromLTRB(px - num6, top, px + num7, bottom).Contains(p.X, p.Y) || flag);
                    }
                    if (!this.Pen.Visible)
                    {
                        if (top < bottom)
                        {
                            top--;
                        }
                        else
                        {
                            bottom--;
                        }
                    }
                    if (!Utils.FromLTRB(px - num6, top, (px + num7) + 1, bottom).Contains(p.X, p.Y))
                    {
                        return flag;
                    }
                }
                return true;
            }
            if (this.style == CandleStyles.Line)
            {
                int firstVisible = base.firstVisible;
                point.X = px;
                point.Y = bottom;
                if (valueIndex != firstVisible)
                {
                    flag = Graphics3D.PointInLineTolerance(p, this.OldP.X, this.OldP.Y, point.X, point.Y, 3);
                }
                this.OldP = point;
                return flag;
            }
            if ((!Graphics3D.PointInLineTolerance(p, px, num4, px, qy, 3) && (!this.showOpenTick || !Graphics3D.PointInLineTolerance(p, px, top, (px - num6) - 1, top, 3))) && (!this.showCloseTick || !Graphics3D.PointInLineTolerance(p, px, bottom, (px + num7) + 1, bottom, 3)))
            {
                return flag;
            }
            return true;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.CandleBar);
            AddSubChart(Texts.CandleNoOpen);
            AddSubChart(Texts.CandleNoClose);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.Line);
        }

        protected internal override void DoBeforeDrawChart()
        {
        }

        public override void DrawValue(int valueIndex)
        {
            PointerStyles rectangle = PointerStyles.Rectangle;
            Color aColor = this.CalculateColor(valueIndex);
            Color color = this.Pen.Color;
            Color color3 = this.HighLowPen.Color;
            base.OnGetPointerStyle(valueIndex, ref rectangle, ref aColor);
            Graphics3D g = base.chart.graphics3D;
            int x = base.CalcXPosValue(base.DateValues[valueIndex]);
            int y = base.CalcYPosValue(base.vOpenValues[valueIndex]);
            int bottom = base.CalcYPosValue(base.vHighValues[valueIndex]);
            int num4 = base.CalcYPosValue(base.vLowValues[valueIndex]);
            int num5 = base.CalcYPosValue(base.CloseValues[valueIndex]);
            int num6 = this.candleWidth / 2;
            int num7 = this.candleWidth - num6;
            base.Brush.Color = aColor;
            g.Brush = base.Brush;
            if ((this.style == CandleStyles.CandleStick) || (this.style == CandleStyles.OpenClose))
            {
                int num8;
                int num9;
                this.CheckHighLowPen(g);
                if (num5 > y)
                {
                    num8 = y;
                    num9 = num5;
                }
                else
                {
                    num8 = num5;
                    num9 = y;
                }
                if (base.chart.Aspect.View3D && base.point.Draw3D)
                {
                    if ((this.style == CandleStyles.CandleStick) && g.Pen.Visible)
                    {
                        g.VerticalLine(x, num9, num4, base.MiddleZ);
                    }
                    if (y == num5)
                    {
                        g.Pen.Color = this.CalculateColor(valueIndex);
                    }
                    if (base.Transparency > 0)
                    {
                        g.Brush.Transparency = base.Transparency;
                    }
                    g.Pen = this.Pen;
                    g.Cube(x - num6, num8, x + num7, num9, base.StartZ, base.EndZ, base.point.Dark3D);
                    this.CheckHighLowPen(g);
                    if ((this.style == CandleStyles.CandleStick) && g.Pen.Visible)
                    {
                        g.VerticalLine(x, num8, bottom, base.MiddleZ);
                    }
                }
                else
                {
                    if ((this.style == CandleStyles.CandleStick) && g.Pen.Visible)
                    {
                        if (base.chart.Aspect.View3D)
                        {
                            g.VerticalLine(x, num9, num4, base.MiddleZ);
                        }
                        else
                        {
                            g.VerticalLine(x, num9, num4);
                        }
                    }
                    g.Pen = this.Pen;
                    g.Brush.Color = this.CalculateColor(valueIndex);
                    if ((y == num5) && !this.Pen.Visible)
                    {
                        g.Pen.Color = this.CalculateColor(valueIndex);
                    }
                    if (base.Transparency > 0)
                    {
                        g.Brush.Transparency = base.Transparency;
                    }
                    if (base.chart.aspect.view3D)
                    {
                        if (y == num5)
                        {
                            num5--;
                        }
                        g.Rectangle(new Rectangle(x - num6, y, num6 + num7, num5 - y), base.MiddleZ);
                    }
                    else if (Math.Abs((int) (y - num5)) < 2)
                    {
                        bool visible = g.Pen.Visible;
                        Color color4 = g.Pen.DrawingPen.Color;
                        g.Pen.Visible = true;
                        g.Pen.DrawingPen.Color = g.Pen.Color;
                        g.Line(x - num6, y, (x + num7) + 1, y);
                        g.Pen.Visible = visible;
                        g.Pen.DrawingPen.Color = color4;
                    }
                    else
                    {
                        g.Rectangle(x - num6, y, (x + num7) + 1, num5);
                    }
                    this.CheckHighLowPen(g);
                    if ((this.style == CandleStyles.CandleStick) && g.Pen.Visible)
                    {
                        if (base.chart.Aspect.View3D)
                        {
                            g.VerticalLine(x, num8, bottom, base.MiddleZ);
                        }
                        else
                        {
                            g.VerticalLine(x, num8, bottom);
                        }
                    }
                }
            }
            else if (this.style == CandleStyles.CandleBar)
            {
                g.Pen = base.point.Pen;
                g.Pen.Color = this.CalculateColor(valueIndex);
                if (base.chart.Aspect.View3D)
                {
                    g.VerticalLine(x, num4, bottom, base.MiddleZ);
                    if (this.showOpenTick)
                    {
                        g.HorizontalLine(x, (x - num6) - 1, y, base.MiddleZ);
                    }
                    if (this.showCloseTick)
                    {
                        g.HorizontalLine(x, (x + num7) + 1, num5, base.MiddleZ);
                    }
                }
                else
                {
                    g.VerticalLine(x, num4, bottom);
                    if (this.showOpenTick)
                    {
                        g.HorizontalLine(x, (x - num6) - 1, y);
                    }
                    if (this.showCloseTick)
                    {
                        g.HorizontalLine(x, (x + num7) + 1, num5);
                    }
                }
            }
            else
            {
                Point point = new Point(x, num5);
                int num10 = this.DrawValuesForward() ? base.firstVisible : base.lastVisible;
                if ((valueIndex != num10) && !base.IsNull(valueIndex))
                {
                    this.Pen.Color = this.CalculateColor(valueIndex);
                    g.Pen = this.Pen;
                    if (base.chart.aspect.view3D)
                    {
                        g.Line(this.OldP, point, base.MiddleZ);
                    }
                    else
                    {
                        g.Line(this.OldP, point);
                    }
                }
                this.OldP = point;
            }
            this.Pen.Color = color;
            this.Pen.DrawingPen.Color = color;
            base.point.Pen.DrawingPen.Color = color;
            this.highLowPen.Color = color3;
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        protected internal override Color LegendItemColor(int index)
        {
            return this.CalculateColor(index);
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.FillSampleValues(4);
            base.ColorEach = true;
            if (IsEnabled)
            {
                this.upCloseColor = Color.Blue;
            }
            else
            {
                this.upCloseColor = Color.Silver;
                this.downCloseColor = Color.Silver;
                base.point.Pen.Color = Color.Gray;
            }
            base.point.Pen.Width = 2;
            this.candleWidth = 12;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.highLowPen != null)
            {
                this.highLowPen.Chart = base.chart;
            }
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.Style = CandleStyles.CandleBar;
                    return;

                case 2:
                    this.Pen.Visible = true;
                    this.Style = CandleStyles.CandleBar;
                    this.showOpenTick = false;
                    return;

                case 3:
                    this.Pen.Visible = true;
                    this.Style = CandleStyles.CandleBar;
                    this.showCloseTick = false;
                    return;

                case 4:
                    this.Style = CandleStyles.CandleStick;
                    this.Pen.Visible = false;
                    return;

                case 5:
                    this.Style = CandleStyles.Line;
                    return;
            }
            base.SetSubGallery(index);
        }

        [DefaultValue(4), Description("Sets the horizontal Candle Size.")]
        public int CandleWidth
        {
            get
            {
                return this.candleWidth;
            }
            set
            {
                base.SetIntegerProperty(ref this.candleWidth, value);
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryCandle;
            }
        }

        [DefaultValue(typeof(Color), "Red"), Description("Candle color when Close value is greater than Open value.")]
        public Color DownCloseColor
        {
            get
            {
                return this.downCloseColor;
            }
            set
            {
                base.SetColorProperty(ref this.downCloseColor, value);
            }
        }

        [DefaultValue((string) null), Description("Pen used to draw \"high\" and \"low\" lines."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen HighLowPen
        {
            get
            {
                if (this.highLowPen == null)
                {
                    this.highLowPen = new ChartPen(base.chart, Utils.EmptyColor);
                }
                return this.highLowPen;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                return base.Pointer.Pen;
            }
        }

        [DefaultValue(true), Description("Determines whether Close prices will be displayed.")]
        public bool ShowClose
        {
            get
            {
                return this.showCloseTick;
            }
            set
            {
                base.SetBooleanProperty(ref this.showCloseTick, value);
            }
        }

        [DefaultValue(true), Description("Determines whether Open prices will be displayed.")]
        public bool ShowOpen
        {
            get
            {
                return this.showOpenTick;
            }
            set
            {
                base.SetBooleanProperty(ref this.showOpenTick, value);
            }
        }

        [DefaultValue(0), Description("Determines how  the Candle points will be drawn.")]
        public CandleStyles Style
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

        [DefaultValue(typeof(Color), "White"), Description("Candle color fill when Open value is greater than Close.")]
        public Color UpCloseColor
        {
            get
            {
                return this.upCloseColor;
            }
            set
            {
                base.SetColorProperty(ref this.upCloseColor, value);
            }
        }
    }
}

