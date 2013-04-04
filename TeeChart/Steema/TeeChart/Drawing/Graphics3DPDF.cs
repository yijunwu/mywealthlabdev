namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Export;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class Graphics3DPDF : Graphics3DVec
    {
        private PDFData.PDFFonts fonts;
        private int iClipStack;
        private int IHeight;
        private GraphicsImages images;
        private int IWidth;

        public Graphics3DPDF(PDFData pdfdata, Chart c) : base(pdfdata.Stream, c)
        {
            this.fonts = pdfdata.Fonts;
            this.images = pdfdata.Images;
            base.iCanvasType = CanvasType.PDF;
        }

        public override void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle)
        {
            this.DrawArc(base.Brush, base.Pen, new Rectangle(x1, y1, x2 - x1, y2 - y1), startAngle, sweepAngle, 1, false);
        }

        public override void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            double num;
            double num2;
            base.CalcArcAngles(x1, y1, x2, y2, x3, y3, x4, y4, out num, out num2);
            this.DrawArc(base.Brush, base.Pen, new Rectangle(x1, y1, x2 - x1, y2 - y1), (float) num, (float) num2, 1, false);
        }

        private void ArcSegment(float ax, float ay, float ra, float rb, float midtheta, float hangle, int amt0, float fccwc)
        {
            float num = Math.Abs(hangle);
            float num2 = (float) Math.Cos((double) num);
            float num3 = (float) Math.Sin((double) num);
            if (ra < rb)
            {
                float num4 = ra;
                ra = rb;
                rb = num4;
            }
            float num5 = ra * num2;
            float num6 = (-fccwc * ra) * num3;
            this.Rotate(ref num5, ref num6, midtheta);
            float num7 = ax + num5;
            float num8 = ay + num6;
            if (amt0 == 1)
            {
                base.m_string = num7.ToString("0.000") + " " + num8.ToString("0.000") + " m\n";
            }
            else if (amt0 == 0)
            {
                base.m_string = num7.ToString("0.000") + " " + num8.ToString("0.000") + " l\n";
            }
            else
            {
                base.m_string = "";
            }
            float num9 = (ra * (4f - num2)) / 3f;
            float num10 = num9;
            float num11 = (((ra * fccwc) * (1f - num2)) * (num2 - 3f)) / (3f * num3);
            float num12 = -num11;
            float num13 = ra * num2;
            float num14 = (fccwc * ra) * num3;
            this.Rotate(ref num9, ref num11, midtheta);
            this.Rotate(ref num10, ref num12, midtheta);
            this.Rotate(ref num13, ref num14, midtheta);
            base.m_string = base.m_string + this.InternalBezCurve(ax + num9, ay + num11, ax + num10, ay + num12, ax + num13, ay + num14);
            this.AddToStream(base.FixSeparator(base.m_string));
        }

        private string BrushProperties(ChartBrush abrush)
        {
            return (this.PDFColor(abrush.Color) + " rg");
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
            base.m_string = "q\n";
            int bottom = r.Bottom;
            this.TrVertCoord(ref bottom);
            string str = base.m_string;
            base.m_string = str + r.Left.ToString("0.00") + " " + bottom.ToString("0.00") + " " + r.Width.ToString("0.00") + " " + r.Height.ToString("0.00") + " re W n";
            this.AddToStream(base.FixSeparator(base.m_string));
        }

        protected override void DoText(int x, int y, string text, double rotrad, Color c)
        {
            base.m_string = this.PDFColor(base.Font.Color) + " rg ";
            base.m_string = base.m_string + "BT ";
            int num = this.fonts.FindFont(base.Font);
            string str2 = base.m_string;
            base.m_string = str2 + "/" + this.fonts[num].DictFontName() + " " + base.Font.Size.ToString() + " Tf ";
            float num2 = 0f;
            if (base.TextAlign == StringAlignment.Center)
            {
                num2 = this.TextWidth(text) * 0.5f;
            }
            else if (base.TextAlign == StringAlignment.Far)
            {
                num2 = this.TextWidth(text);
            }
            num2 *= base.FontDPI();
            float num3 = this.TextHeight(text) * base.FontDPI();
            this.TrVertCoord(ref y);
            float num4 = (float) Math.Cos(rotrad);
            float num5 = (float) Math.Sin(rotrad);
            float num6 = x - ((num2 * num4) - (num3 * num5));
            float num7 = y - ((num2 * num5) + (num3 * num4));
            string str3 = base.m_string;
            string[] strArray2 = new string[] { str3, num4.ToString("0.000"), " ", num5.ToString("0.000"), " ", (num5 * -1f).ToString("0.000"), " ", num4.ToString("0.000"), " ", num6.ToString("0.000"), " ", num7.ToString("0.000"), " Tm " };
            base.m_string = string.Concat(strArray2);
            string str = base.FixSeparator(base.m_string);
            base.m_string = str + "<" + this.TextToPDFText(text) + "> Tj ET";
            this.AddToStream(base.m_string);
        }

        public override void Draw(Rectangle r, Image image, bool transparent)
        {
            this.InternalDraw((float) r.Width, (float) r.Height, (float) r.Left, (float) (this.IHeight - r.Bottom), image);
        }

        public override void Draw(int x, int y, Image image)
        {
            this.InternalDraw((float) image.Width, (float) image.Height, (float) x, (float) ((this.IHeight - y) - image.Height), image);
        }

        private void DrawArc(ChartBrush br, ChartPen pn, Rectangle rect, float startAngle, float sweepAngle, int moveto0, bool drawpie)
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
                float x = (rect.Left + rect.Right) * 0.5f;
                float y = (rect.Top + rect.Bottom) * 0.5f;
                float ra = rect.Width * 0.5f;
                float rb = rect.Height * 0.5f;
                if (ra != rb)
                {
                    float num5;
                    float num6;
                    base.m_string = "q ";
                    if (ra > rb)
                    {
                        num5 = rb / ra;
                        num6 = y * (1f - num5);
                        string str = base.m_string;
                        base.m_string = str + "1 0 0 " + num5.ToString("0.000") + " 0 " + num6.ToString("0.000");
                    }
                    else
                    {
                        num5 = ra / rb;
                        num6 = x * (1f - num5);
                        string str2 = base.m_string;
                        base.m_string = str2 + num5.ToString("0.000") + " 0 0 1 " + num6.ToString("0.000") + " 0";
                    }
                    base.m_string = base.m_string + " cm";
                    this.AddToStream(base.FixSeparator(base.m_string));
                }
                startAngle = ((360f - startAngle) * 3.141593f) / 180f;
                sweepAngle *= -0.01745329f;
                if (startAngle < 0f)
                {
                    startAngle += 6.283185f;
                }
                float num7 = startAngle + sweepAngle;
                if (num7 < 0f)
                {
                    num7 += 6.283185f;
                }
                if (drawpie)
                {
                    this.AddToStream(this.PointToStr(x, y) + " m\n");
                }
                this.TrVertCoord(ref y);
                float fccwc = 1f;
                int num9 = 1;
                if (num7 < startAngle)
                {
                    fccwc = -1f;
                }
                while ((Math.Abs(sweepAngle) / ((float) num9)) > 1.5707963267948966)
                {
                    num9++;
                }
                float num10 = sweepAngle / ((float) num9);
                float hangle = 0.5f * num10;
                float midtheta = startAngle + hangle;
                for (int i = 0; i < num9; i++)
                {
                    if (i == 0)
                    {
                        this.ArcSegment(x, y, ra, rb, midtheta, hangle, moveto0, fccwc);
                    }
                    else
                    {
                        this.ArcSegment(x, y, ra, rb, midtheta, hangle, -1, fccwc);
                    }
                    midtheta += num10;
                }
                if (br.Visible && drawpie)
                {
                    if (pn.Visible)
                    {
                        this.AddToStream("h B");
                    }
                    else
                    {
                        this.AddToStream("h f");
                    }
                }
                else if (drawpie)
                {
                    this.AddToStream("s");
                }
                else if (!drawpie)
                {
                    this.AddToStream("S");
                }
                if (ra != rb)
                {
                    this.AddToStream("Q");
                }
            }
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

        private void DrawPolygon(ChartBrush br, ChartPen pn, bool PolyLine, params PointDouble[] p)
        {
            if (br.Visible || pn.Visible)
            {
                if (base.Pen.Visible)
                {
                    this.AddToStream(this.PenProperties(pn) + " ");
                }
                base.m_string = this.PointToStr(p[0]) + " m ";
                for (int i = 1; i <= p.GetUpperBound(0); i++)
                {
                    base.m_string = base.m_string + this.PointToStr(p[i]) + " l ";
                }
                if (!PolyLine)
                {
                    base.m_string = base.m_string + "h ";
                }
                if (br.Visible && !PolyLine)
                {
                    base.m_string = base.m_string + this.BrushProperties(br);
                    if (pn.Visible)
                    {
                        base.m_string = base.m_string + " B";
                    }
                    else
                    {
                        base.m_string = base.m_string + " f";
                    }
                }
                else
                {
                    base.m_string = base.m_string + " S";
                }
                this.AddToStream(base.m_string);
            }
        }

        private /*unsafe*/ void DrawPolygon(ChartBrush br, ChartPen pn, bool PolyLine, params Point[] p)
        {
            if (br.Visible || pn.Visible)
            {
                if (base.Pen.Visible)
                {
                    this.AddToStream(this.PenProperties(pn) + " ");
                }
                base.m_string = this.PointToStr(*((PointF*) &(p[0]))) + " m ";
                for (int i = 1; i <= p.GetUpperBound(0); i++)
                {
                    base.m_string = base.m_string + this.PointToStr(*((PointF*) &(p[i]))) + " l ";
                }
                if (!PolyLine)
                {
                    base.m_string = base.m_string + "h ";
                }
                if (br.Visible && !PolyLine)
                {
                    base.m_string = base.m_string + this.BrushProperties(br);
                    if (pn.Visible)
                    {
                        base.m_string = base.m_string + " B";
                    }
                    else
                    {
                        base.m_string = base.m_string + " f";
                    }
                }
                else
                {
                    base.m_string = base.m_string + " S";
                }
                this.AddToStream(base.m_string);
            }
        }

        public override void Ellipse(int x1, int y1, int x2, int y2)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                this.AddToStream(this.PenProperties(base.Pen) + " " + this.BrushProperties(base.Brush) + " ");
                float num = (x2 - x1) * 0.5f;
                float num2 = (y2 - y1) * 0.5f;
                float num3 = (x1 + x2) * 0.5f;
                float y = (y1 + y2) * 0.5f;
                this.TrVertCoord(ref y);
                base.m_string = ((num3 + num)).ToString("0.000") + " " + y.ToString("0.000") + " m ";
                base.m_string = base.m_string + this.InternalBezCurve(num3 + num, y + (0.552f * num2), num3 + (0.552f * num), y + num2, num3, y + num2) + "\n";
                base.m_string = base.m_string + this.InternalBezCurve(num3 - (0.552f * num), y + num2, num3 - num, y + (0.552f * num2), num3 - num, y) + "\n";
                base.m_string = base.m_string + this.InternalBezCurve(num3 - num, y - (0.552f * num2), num3 - (0.552f * num), y - num2, num3, y - num2) + "\n";
                base.m_string = base.m_string + this.InternalBezCurve(num3 + (0.552f * num), y - num2, num3 + num, y - (0.552f * num2), num3 + num, y);
                this.AddToStream(base.FixSeparator(base.m_string));
                if (base.Brush.Visible)
                {
                    if (base.Pen.Visible)
                    {
                        this.AddToStream(" B");
                    }
                    else
                    {
                        this.AddToStream(" f");
                    }
                }
                else
                {
                    this.AddToStream(" S");
                }
            }
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
        }

        private string InternalBezCurve(float ax1, float ay1, float ax2, float ay2, float ax3, float ay3)
        {
            return (ax1.ToString("0.000") + " " + ay1.ToString("0.000") + " " + ax2.ToString("0.000") + " " + ay2.ToString("0.000") + " " + ax3.ToString("0.000") + " " + ay3.ToString("0.000") + " c");
        }

        private void InternalDraw(float sx, float sy, float tx, float ty, Image image)
        {
            int num = this.images.FindImage(image);
            base.m_string = "q\n";
            base.m_string = base.m_string + " " + sx.ToString("0.000") + " 0 0 ";
            string str = base.m_string;
            base.m_string = str + sy.ToString("0.000") + " " + tx.ToString("0.000") + " ";
            base.m_string = base.m_string + ty.ToString("0.000");
            base.m_string = base.m_string + " cm /" + this.images[num].DictImageName() + " Do \n";
            base.m_string = base.m_string + "Q";
            this.AddToStream(base.FixSeparator(base.m_string));
        }

        protected override void InternalRect(ChartBrush b, Rectangle r, bool UsePen, bool IsRound)
        {
            if (b.Visible || (UsePen && base.Pen.Visible))
            {
                int bottom = r.Bottom;
                this.TrVertCoord(ref bottom);
                base.m_string = this.PenProperties(base.Pen) + " " + this.BrushProperties(b) + " ";
                string str = base.m_string;
                base.m_string = str + r.Left.ToString("0.00") + " " + bottom.ToString("0.00") + " " + r.Width.ToString("0.00") + " " + r.Height.ToString("0.00") + " re";
                if (b.Visible)
                {
                    if (base.Pen.Visible)
                    {
                        base.m_string = base.m_string + " B";
                    }
                    else
                    {
                        base.m_string = base.m_string + " f";
                    }
                }
                else
                {
                    base.m_string = base.m_string + " S";
                }
                this.AddToStream(base.FixSeparator(base.m_string));
            }
        }

        public override void LineTo(int x, int y)
        {
            base.m_string = this.PenProperties(base.Pen) + " " + this.PointToStr((float) base.fx, (float) base.fy) + " m " + this.PointToStr((float) x, (float) y) + " l S";
            this.AddToStream(base.m_string);
            base.fx = x;
            base.fy = y;
        }

        private string PDFColor(Color c)
        {
            float num = ((float) c.R) / 255f;
            float num2 = ((float) c.G) / 255f;
            float num3 = ((float) c.B) / 255f;
            base.m_string = num.ToString("0.00") + " " + num2.ToString("0.00") + " " + num3.ToString("0.00");
            return base.FixSeparator(base.m_string);
        }

        private string PenProperties(ChartPen apen)
        {
            string str = this.PDFColor(apen.Color) + " RG " + apen.Width.ToString() + " w " + this.PenStyle(apen.Style) + " ";
            switch (apen.EndCap)
            {
                case LineCap.Square:
                    return (str + "2 J ");

                case LineCap.Round:
                    return (str + "1 J ");
            }
            return (str + "0 J ");
        }

        private string PenStyle(DashStyle style)
        {
            switch (style)
            {
                case DashStyle.Dash:
                    return "[3 3] 0 d";

                case DashStyle.Dot:
                    return "[2] 1 d";

                case DashStyle.DashDot:
                    return "[3 2] 2 d";

                case DashStyle.DashDotDot:
                    return "[3 2 2 2 2] 2 d";
            }
            return "[ ] 0 d";
        }

        public override void Pie(int x1, int y1, int x2, int y2, double startAngle, double endAngle)
        {
            float sweepAngle = (float) (endAngle - startAngle);
            this.DrawArc(base.Brush, base.Pen, new Rectangle(x1, y1, x2 - x1, y2 - y1), (float) startAngle, sweepAngle, 0, true);
        }

        public override void Pixel(int x, int y, int z, Color color)
        {
            if (base.Pen.Visible)
            {
                base.Pen.Color = color;
                base.Calc3DPos(ref x, ref y, z);
                this.MoveTo(x, y);
                this.LineTo(x, y);
            }
        }

        protected override string PointToStr(PointDouble p)
        {
            return this.PointToStr(PointDouble.RoundF(p));
        }

        protected override string PointToStr(Point p)
        {
            return this.PointToStr(new PointF((float) p.X, (float) p.Y));
        }

        private string PointToStr(PointF p)
        {
            float y = p.Y;
            this.TrVertCoord(ref y);
            base.m_string = p.X.ToString("0.00") + " " + y.ToString("0.00");
            return base.FixSeparator(base.m_string);
        }

        protected override string PointToStr(int x, int y)
        {
            return this.PointToStr((PointF) new Point(x, y));
        }

        private string PointToStr(float x, float y)
        {
            return this.PointToStr(new PointF(x, y));
        }

        public override void Polygon(params PointDouble[] p)
        {
            this.DrawPolygon(base.Brush, base.Pen, false, p);
        }

        public override void Polygon(params Point[] p)
        {
            this.DrawPolygon(base.Brush, base.Pen, false, p);
        }

        public override void Polyline(params Point[] p)
        {
            this.DrawPolygon(base.Brush, base.Pen, true, p);
        }

        public override void PrepareDrawImage()
        {
        }

        private void Rotate(ref float ax, ref float ay, float angle)
        {
            float num = (float) Math.Cos((double) angle);
            float num2 = (float) Math.Sin((double) angle);
            float num3 = ax;
            float num4 = ay;
            ax = (num * num3) - (num2 * num4);
            ay = (num2 * num3) + (num * num4);
        }

        public override void RotateLabel(int x, int y, string text, double rotDegree)
        {
            if (base.Font.ShouldDrawShadow())
            {
                this.DoText(x + base.Font.Shadow.Width, y + base.Font.Shadow.Height, text, rotDegree * 0.01745329, Utils.EmptyColor);
            }
            else
            {
                this.DoText(x, y, text, rotDegree * 0.01745329, Utils.EmptyColor);
            }
        }

        public override void SetClipRegion(Region region)
        {
        }

        public override void ShowImage(Graphics g)
        {
            base.ShowImage(g);
        }

        private string TextToPDFText(string text)
        {
            char[] chArray = text.ToCharArray();
            text = "";
            foreach (char ch in chArray)
            {
                text = text + Convert.ToUInt16(ch).ToString("X");
            }
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
                this.AddToStream("Q");
            }
        }
    }
}

