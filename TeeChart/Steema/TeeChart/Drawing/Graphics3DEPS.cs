namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Export;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class Graphics3DEPS : Graphics3DVec
    {
        private EPSData.EPSFonts fonts;
        private int iClipStack;
        private int IHeight;
        private GraphicsImages images;
        private int IWidth;

        public Graphics3DEPS(EPSData epsdata, Chart c) : base(epsdata.Stream, c)
        {
            this.fonts = epsdata.Fonts;
            this.images = epsdata.Images;
            base.iCanvasType = CanvasType.EPS;
        }

        public override void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle)
        {
            this.InternalArc(base.Brush, base.Pen, new Rectangle(x1, y1, x2 - x1, y2 - y1), startAngle, sweepAngle, false);
        }

        public override void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
        }

        private string BrushProperties(ChartBrush abrush)
        {
            return this.PSColor(abrush.Color);
        }

        public override void Changed(object o)
        {
        }

        public override void ClearClipRegions()
        {
        }

        public override void ClipEllipse(Rectangle r)
        {
        }

        public override void ClipPolygon(params Point[] p)
        {
        }

        public override void ClipRectangle(Rectangle r)
        {
            this.iClipStack++;
            base.m_string = "clipsave ";
            int bottom = r.Bottom;
            this.TrVertCoord(ref bottom);
            string str = base.m_string;
            base.m_string = str + r.Left.ToString("0.00") + " " + bottom.ToString("0.00") + " " + r.Width.ToString("0.00") + " " + r.Height.ToString("0.00") + " rectclip";
            this.AddToStream(base.FixSeparator(base.m_string));
        }

        protected override void DoText(int x, int y, string text, double degangle, Color c)
        {
            int num = this.fonts.FindFont(base.Font);
            y += Utils.Round((float) (this.TextHeight(text) * base.FontDPI()));
            int num2 = Utils.Round((float) (((float) base.Font.Size) / base.FontDPI()));
            base.m_string = this.PSColor(base.Font.Color) + " /" + this.fonts[num].DictFontName() + " findfont ";
            base.m_string = base.m_string + num2.ToString() + " scalefont setfont ";
            base.m_string = base.m_string + this.PointToStr((float) x, (float) y) + " m";
            this.AddToStream(base.m_string);
            base.m_string = "(" + this.TextToPSText(text) + ")";
            if (base.TextAlign == StringAlignment.Center)
            {
                base.m_string = base.m_string + " ctext";
            }
            else if (base.TextAlign == StringAlignment.Far)
            {
                base.m_string = base.m_string + " rtext";
            }
            else
            {
                base.m_string = base.m_string + " ltext";
            }
            this.AddToStream(base.m_string);
        }

        public override void Draw(Rectangle r, Image image, bool transparent)
        {
        }

        public override void Draw(int x, int y, Image image)
        {
        }

        private string DrawArc(float centerX, float centerY, float rA, float rB, float startAngle, float sweepAngle, bool drawpie)
        {
            string str2 = centerX.ToString("0.000") + " " + centerY.ToString("0.000") + " ";
            string str3 = str2 + rA.ToString("0.000") + " " + rB.ToString("0.000") + " ";
            string text = str3 + startAngle.ToString("0.000") + " " + sweepAngle.ToString("0.000") + " ";
            if (drawpie)
            {
                text = text + "pie";
            }
            else
            {
                text = text + "arc";
            }
            return base.FixSeparator(text);
        }

        public override void DrawBeziers(params Point[] p)
        {
        }

        private void DrawBrushImage(Rectangle rect)
        {
        }

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
        }

        public override void Ellipse(int x1, int y1, int x2, int y2)
        {
        }

        public override void EraseBackground(int left, int top, int right, int bottom)
        {
        }

        public override void FillRegion(Brush brush, Region region)
        {
        }

        protected internal override void InitWindow(Graphics graphics, Aspect a, Rectangle r, int MaxDepth)
        {
            base.InitWindow(graphics, a, r, MaxDepth);
            this.TheBounds();
            this.AddToStream("gs");
            this.AddToStream("np");
        }

        private void InternalArc(ChartBrush br, ChartPen pn, Rectangle rect, float startAngle, float sweepAngle, bool drawpie)
        {
            if (br.Visible || pn.Visible)
            {
                base.m_string = this.PenProperties(pn);
                if (br.Visible && drawpie)
                {
                    base.m_string = base.m_string + " " + this.BrushProperties(br) + "\n";
                }
                else
                {
                    base.m_string = base.m_string + " ";
                }
                this.AddToStream(base.m_string);
                float centerX = (rect.Left + rect.Right) * 0.5f;
                float centerY = (rect.Top + rect.Bottom) * 0.5f;
                float rA = rect.Width * 0.5f;
                float rB = rect.Height * 0.5f;
                if (drawpie)
                {
                    if (br.Visible)
                    {
                        base.m_string = "gs " + this.BrushProperties(br) + "\n";
                        base.m_string = base.m_string + this.DrawArc(centerX, centerY, rA, rB, startAngle, sweepAngle, true) + "\n";
                        base.m_string = base.m_string + "fi gr";
                        this.AddToStream(base.FixSeparator(base.m_string));
                    }
                    if (pn.Visible)
                    {
                        base.m_string = "gs " + this.PenProperties(pn) + "\n";
                        base.m_string = base.m_string + this.DrawArc(centerX, centerY, rA, rB, startAngle, sweepAngle, true) + "\n";
                        base.m_string = base.m_string + "st gr";
                        this.AddToStream(base.FixSeparator(base.m_string));
                    }
                }
                else if (pn.Visible)
                {
                    base.m_string = "gs " + this.PenProperties(pn) + "\n";
                    base.m_string = base.m_string + this.DrawArc(centerX, centerY, rA, rB, startAngle, sweepAngle, false) + "\n";
                    base.m_string = base.m_string + "st gr";
                    this.AddToStream(base.FixSeparator(base.m_string));
                }
            }
        }

        private void InternalPolygon(ChartBrush br, ChartPen pn, bool PolyLine, params PointDouble[] p)
        {
            if (br.Visible || pn.Visible)
            {
                string str = "np " + this.PointToStr(p[0].X, p[0].Y) + " m\n";
                for (int i = 1; i <= p.GetUpperBound(0); i++)
                {
                    str = str + this.PointToStr(p[i].X, p[i].Y) + " l\n";
                }
                str = str + "cp\n";
                if (br.Visible && !PolyLine)
                {
                    base.m_string = "gs\n" + this.BrushProperties(br) + "\n";
                    base.m_string = base.m_string + str;
                    base.m_string = base.m_string + "fi\ngr";
                    this.AddToStream(base.m_string);
                }
                if (pn.Visible)
                {
                    base.m_string = "gs\n" + this.PenProperties(pn) + "\n";
                    base.m_string = base.m_string + str;
                    base.m_string = base.m_string + "st\ngr";
                    this.AddToStream(base.m_string);
                }
            }
        }

        private void InternalPolygon(ChartBrush br, ChartPen pn, bool PolyLine, params Point[] p)
        {
            if (br.Visible || pn.Visible)
            {
                string str = "np " + this.PointToStr((float) p[0].X, (float) p[0].Y) + " m\n";
                for (int i = 1; i <= p.GetUpperBound(0); i++)
                {
                    str = str + this.PointToStr((float) p[i].X, (float) p[i].Y) + " l\n";
                }
                str = str + "cp\n";
                if (br.Visible && !PolyLine)
                {
                    base.m_string = "gs\n" + this.BrushProperties(br) + "\n";
                    base.m_string = base.m_string + str;
                    base.m_string = base.m_string + "fi\ngr";
                    this.AddToStream(base.m_string);
                }
                if (pn.Visible)
                {
                    base.m_string = "gs\n" + this.PenProperties(pn) + "\n";
                    base.m_string = base.m_string + str;
                    base.m_string = base.m_string + "st\ngr";
                    this.AddToStream(base.m_string);
                }
            }
        }

        protected override void InternalRect(ChartBrush b, Rectangle r, bool UsePen, bool IsRound)
        {
            if (b.Visible || (UsePen && base.Pen.Visible))
            {
                int bottom = r.Bottom;
                this.TrVertCoord(ref bottom);
                string str = (" " + r.Left.ToString("0.00") + " " + bottom.ToString("0.00") + " ") + r.Width.ToString("0.00") + " " + r.Height.ToString("0.00");
                if (b.Visible)
                {
                    base.m_string = "gs " + this.PSColor(b.Color);
                    base.m_string = base.m_string + str + " rectfill gr";
                    this.AddToStream(base.FixSeparator(base.m_string));
                }
                if (UsePen && base.Pen.Visible)
                {
                    base.m_string = "gs " + this.PenProperties(base.Pen);
                    base.m_string = base.m_string + str + " rectstroke gr";
                    this.AddToStream(base.FixSeparator(base.m_string));
                }
            }
        }

        public override void LineTo(int x, int y)
        {
            base.m_string = "gs " + this.PenProperties(base.Pen) + " " + this.PointToStr((float) base.fx, (float) base.fy) + " m ";
            base.m_string = base.m_string + this.PointToStr((float) x, (float) y) + " l st gr";
            this.AddToStream(base.m_string);
            base.fx = x;
            base.fy = y;
        }

        private string PenProperties(ChartPen apen)
        {
            string str = this.PSColor(apen.Color) + " " + this.PenStyle(apen.Style) + " ";
            switch (apen.EndCap)
            {
                case LineCap.Square:
                    str = str + "2 setlinecap ";
                    break;

                case LineCap.Round:
                    str = str + "1 setlinecap ";
                    break;

                default:
                    str = str + "0 setlinecap ";
                    break;
            }
            return (str + apen.Width.ToString() + " sw");
        }

        private string PenStyle(DashStyle style)
        {
            switch (style)
            {
                case DashStyle.Dash:
                    return "[3] 0 sd";

                case DashStyle.Dot:
                    return "[2] 1 sd";

                case DashStyle.DashDot:
                    return "[3 2] 2 sd";

                case DashStyle.DashDotDot:
                    return "[3 2 2 2 2] 2 sd";
            }
            return "[] 0 sd";
        }

        public override void Pixel(int x, int y, int z, Color color)
        {
            if (base.Pen.Visible)
            {
                base.Calc3DPos(ref x, ref y, z);
                base.Pen.Color = color;
                this.MoveTo(x, y);
                this.LineTo(x, y);
            }
        }

        protected override string PointToStr(double X, double Y)
        {
            return this.PointToStr((float) X, (float) Y);
        }

        protected override string PointToStr(int x, int y)
        {
            return this.PointToStr((float) x, (float) y);
        }

        private string PointToStr(float x, float y)
        {
            this.TrVertCoord(ref y);
            base.m_string = x.ToString("0.00") + " " + y.ToString("0.00");
            return base.FixSeparator(base.m_string);
        }

        public override void Polygon(params PointDouble[] p)
        {
            this.InternalPolygon(base.Brush, base.Pen, false, p);
        }

        public override void Polygon(params Point[] p)
        {
            this.InternalPolygon(base.Brush, base.Pen, false, p);
        }

        public override void Polyline(params Point[] p)
        {
            this.InternalPolygon(base.Brush, base.Pen, true, p);
        }

        public override void PrepareDrawImage()
        {
        }

        private string PSColor(Color c)
        {
            float num = ((float) c.R) / 255f;
            float num2 = ((float) c.G) / 255f;
            float num3 = ((float) c.B) / 255f;
            base.m_string = num.ToString("0.00") + " " + num2.ToString("0.00") + " " + num3.ToString("0.00") + " rgb";
            return base.FixSeparator(base.m_string);
        }

        public override void RotateLabel(int x, int y, string text, double rotDegree)
        {
            if (base.Font.ShouldDrawShadow())
            {
                x += base.Font.Shadow.Width;
                y += base.Font.Shadow.Height;
            }
            base.m_string = "gs " + this.PointToStr((float) x, (float) y) + " tr " + rotDegree.ToString("0.00") + " rot";
            this.AddToStream(base.FixSeparator(base.m_string));
            this.DoText(0, this.IHeight, text, 0.0, Utils.EmptyColor);
            this.AddToStream("gr");
        }

        public override void SetClipRegion(Region region)
        {
        }

        public override void ShowImage(Graphics g)
        {
            base.ShowImage(g);
            this.AddToStream("gr");
        }

        private string TextToPSText(string text)
        {
            text = text.Replace(@"\", @"\\");
            text = text.Replace("(", @"\(");
            text = text.Replace(")", @"\)");
            return text;
        }

        private void TheBounds()
        {
            this.IWidth = base.Chart.Width;
            this.IHeight = base.Chart.Height;
        }

        protected internal override void TransparentEllipse(int x1, int y1, int x2, int y2)
        {
        }

        private void TrVertCoord(ref int Y)
        {
            Y = this.IHeight - Y;
        }

        private void TrVertCoord(ref float Y)
        {
            Y = this.IHeight - Y;
        }

        public override void UnClip()
        {
            if (this.iClipStack > 0)
            {
                this.iClipStack--;
                this.AddToStream("cliprestore");
            }
        }
    }
}

