namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections;
    using System.Drawing;

    public class GaugeSeriesPointer : SeriesPointer
    {
        private double IStartAngle;
        private int PXMinusSmall;
        private int PXPlusSmall;
        private int PYMinusSmall;
        private int PYPlusSmall;
        private Steema.TeeChart.Drawing.Shadow shadow;
        private GaugePointerStyles style;
        private PointerStyles tmpStyle;

        public GaugeSeriesPointer(Chart c, Series s) : base(c, s)
        {
        }

        public void Draw(Graphics3D g, Point Inner, Point InnerPlus, Point InnerMinus, Point Outer, Point OuterPlus, Point OuterMinus)
        {
            if ((this.Style != GaugePointerStyles.Tick) && (this.Style != GaugePointerStyles.MinorTick))
            {
                this.Draw(g, false, Outer.X, Outer.Y - Utils.Round((float) (base.VertSize / 2)), Utils.Round((float) (base.HorizSize / 2)), Utils.Round((float) (base.VertSize / 2)), base.Color, this.Style);
            }
            else if ((this.Style == GaugePointerStyles.Tick) && base.Visible)
            {
                Color color = base.Pen.Color;
                Color color2 = base.Brush.Color;
                Gradient gradient = base.Brush.Gradient;
                base.PrepareCanvas(g, base.Color);
                Point[] p = new Point[] { OuterPlus, OuterMinus, InnerMinus, InnerPlus };
                g.Polygon(p);
                base.Brush.Color = color2;
                base.Brush.Gradient = gradient;
                base.Pen.Color = color;
            }
            else if ((this.Style == GaugePointerStyles.MinorTick) && base.Visible)
            {
                this.Draw(g, false, Outer.X, Outer.Y, base.HorizSize, base.VertSize, base.Color, 1);
            }
        }

        public void Draw(Graphics3D g, bool is3D, int px, int py, int tmpHoriz, int tmpVert, Color colorValue, GaugePointerStyles aStyle)
        {
            if ((aStyle != GaugePointerStyles.Hand) && (aStyle != GaugePointerStyles.Center))
            {
                base.Draw(g, is3D, px, py, tmpHoriz, tmpVert, colorValue, (PointerStyles) aStyle);
            }
            else if (base.Visible)
            {
                Color color = base.Pen.Color;
                Color color2 = base.Brush.Color;
                Gradient gradient = base.Brush.Gradient;
                base.PrepareCanvas(g, base.Color);
                this.SetValues(px, py, tmpHoriz, tmpVert);
                if (this.Style == GaugePointerStyles.Hand)
                {
                    Point[] p = new Point[] { new Point(base.PXMinus, base.PYMinus), new Point(base.PXPlus, base.PYMinus), new Point(this.PXPlusSmall, this.PYPlusSmall), new Point(px, base.PYPlus), new Point(this.PXMinusSmall, this.PYPlusSmall) };
                    g.Polygon(p);
                }
                else if (this.Style == GaugePointerStyles.Center)
                {
                    g.Ellipse(base.PXMinus, base.PYMinus, base.PXPlus, base.PYPlus);
                }
                base.Brush.Color = color2;
                base.Brush.Gradient = gradient;
                base.Pen.Color = color;
            }
        }

        public void DrawColorLine(Graphics3D g, int StartAngle, int EndAngle, Rectangle Container)
        {
            if (base.Visible)
            {
                Color color = base.Pen.Color;
                Color color2 = base.Brush.Color;
                Gradient gradient = base.Brush.Gradient;
                base.PrepareCanvas(g, base.Color);
                int num = Math.Abs((int) (EndAngle - StartAngle));
                ArrayList list = new ArrayList();
                int num2 = Utils.Round((float) StartAngle) + 90;
                int num3 = num2 + num;
                for (int i = num2; i < num3; i++)
                {
                    list.Add(g.PointFromCircle(Container, (double) i, 0, true));
                }
                for (int j = num3; j > num2; j--)
                {
                    double twist = ((double) base.VertSize) / -100.0;
                    list.Add(g.PointFromSpiral(Container, (double) j, twist));
                }
                PointDouble[] p = (PointDouble[]) list.ToArray(typeof(PointDouble));
                if (p.Length > 0)
                {
                    if ((base.Gradient != null) && base.Gradient.Visible)
                    {
                        base.Gradient.Angle = StartAngle - 90;
                        g.Polygon(p);
                    }
                    else
                    {
                        g.Polygon(p);
                    }
                }
                base.Brush.Color = color2;
                base.Brush.Gradient = gradient;
                base.Pen.Color = color;
            }
        }

        public void DrawShadow(Graphics3D g, float angle, int px, int py, int tmpHoriz, int tmpVert, GaugePointerStyles aStyle)
        {
            if (this.Shadow.Visible)
            {
                this.SetValues(px, py, tmpHoriz, tmpVert);
                if (this.Style == GaugePointerStyles.Hand)
                {
                    Point[] points = new Point[] { new Point(base.PXMinus, base.PYMinus), new Point(base.PXPlus, base.PYMinus), new Point(this.PXPlusSmall, this.PYPlusSmall), new Point(px, base.PYPlus), new Point(this.PXMinusSmall, this.PYPlusSmall) };
                    double num = ((double) (this.Shadow.Height * 2)) / 180.0;
                    double num2 = ((double) (this.Shadow.Width * 2)) / 180.0;
                    double num3 = angle - this.IStartAngle;
                    int height = Utils.Round((double) (num3 * num));
                    int width = Utils.Round((double) (num3 * num2));
                    if (height > this.Shadow.Height)
                    {
                        height = this.Shadow.Height;
                    }
                    else if (height < (this.Shadow.Height * -1))
                    {
                        height = this.Shadow.Height * -1;
                    }
                    if (width > this.Shadow.Width)
                    {
                        width = this.Shadow.Width;
                    }
                    else if (width < (this.Shadow.Width * -1))
                    {
                        width = this.Shadow.Width * -1;
                    }
                    this.Shadow.Draw(g, -width, -height, points);
                }
                else if (this.Style == GaugePointerStyles.Center)
                {
                    this.Shadow.Draw(g, base.PXMinus, base.PYMinus, base.PXPlus, base.PYPlus);
                }
            }
        }

        private void SetValues(int x, int y, int horiz, int vert)
        {
            this.IStartAngle = Math.Atan((double) (this.Shadow.Height / this.Shadow.Width)) * 57.295779513082323;
            this.IStartAngle += 90.0;
            base.PXMinus = x - horiz;
            base.PXPlus = x + horiz;
            base.PYMinus = y - vert;
            base.PYPlus = y + vert;
            this.PXMinusSmall = base.PXMinus + 2;
            this.PXPlusSmall = base.PXPlus - 2;
            this.PYMinusSmall = base.PYMinus + (horiz * 2);
            this.PYPlusSmall = base.PYPlus - (horiz * 2);
        }

        protected override bool ShouldSerializeVisible()
        {
            return true;
        }

        public Steema.TeeChart.Drawing.Shadow Shadow
        {
            get
            {
                if (this.shadow == null)
                {
                    this.shadow = new Steema.TeeChart.Drawing.Shadow(base.chart, 3);
                    this.shadow.defaultVisible = true;
                    this.shadow.Visible = true;
                }
                return this.shadow;
            }
            set
            {
                this.shadow = value;
            }
        }

        public GaugePointerStyles Style
        {
            get
            {
                this.tmpStyle = base.Style;
                if (this.tmpStyle != PointerStyles.Nothing)
                {
                    this.style = this.tmpStyle;
                }
                return this.style;
            }
            set
            {
                this.style = value;
                base.Style = (PointerStyles) this.style;
            }
        }
    }
}

