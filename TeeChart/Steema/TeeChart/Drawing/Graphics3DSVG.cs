namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Text;

    public class Graphics3DSVG : Graphics3DVec
    {
        public bool AntiAliasing;
        public string DocType;
        private int iClipCount;
        private int iClipStack;
        private int iGradientCount;

        public Graphics3DSVG(Stream istream, Chart c) : base(istream, c)
        {
            base.iCanvasType = CanvasType.SVG;
            base.swFromStream = new StreamWriter(istream, Encoding.Unicode);
            this.AddToStream("<?xml version=\"1.0\" standalone=\"no\"?>");
            this.DocType = "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 20000303 Stylable//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">";
            this.AntiAliasing = true;
        }

        private string AddColors(Color start, Color end)
        {
            return (("<stop offset=\"0%\" stop-color=" + this.SVGColor(start) + ">\r\n") + "<stop offset=\"100%\" stop-color=" + this.SVGColor(end) + ">\r\n");
        }

        private void AddEnd(string st)
        {
            this.AddToStream(st + "/>");
        }

        public override void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle)
        {
            if (base.Pen.Visible)
            {
                int num;
                int num2;
                int num3;
                int num4;
                base.CalcArcPoints(Utils.Round((float) x1), Utils.Round((float) y2), Utils.Round((float) x2), Utils.Round((float) y2), (double) startAngle, (double) sweepAngle, out num, out num2, out num3, out num4);
                this.Arc(Utils.Round((float) x1), Utils.Round((float) y1), Utils.Round((float) x2), Utils.Round((float) y2), num, num2, num3, num4);
            }
        }

        public override void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            if (base.Pen.Visible)
            {
                this.PrepareShape();
                base.m_string = "points=\"" + this.PointToStr(x1, y1) + " " + this.PointToStr(x2, y2) + " ";
                string str = base.m_string;
                base.m_string = str + this.PointToStr(x3, y3) + " " + this.PointToStr(x4, y4) + "\"";
                this.AddEnd(base.m_string);
            }
        }

        public override void ClearClipRegions()
        {
        }

        public override void ClipEllipse(Rectangle r)
        {
            this.SVGClip();
            this.AddEnd("<ellipse " + this.SVGEllipse(Utils.Round((float) r.Left), Utils.Round((float) r.Top), Utils.Round((float) r.Right), Utils.Round((float) r.Bottom)));
            this.SVGEndClip();
        }

        public override void ClipPolygon(params Point[] p)
        {
            this.SVGClip();
            this.AddEnd("<polygon " + this.SVGPoints(p));
            this.SVGEndClip();
        }

        public override void ClipRectangle(Rectangle r)
        {
            this.SVGClip();
            this.AddEnd("<rect " + this.SVGRect(r));
            this.SVGEndClip();
        }

        protected override void DoText(int x, int y, string text, double degangle, Color c)
        {
            y += Utils.Round((double) (1.25 * base.Font.Size));
            if (degangle != 0.0)
            {
                degangle *= -1.0;
                int num = Utils.Round(degangle);
                base.m_string = "<g transform=\"translate(" + x.ToString() + "," + y.ToString() + ") ";
                base.m_string = base.m_string + "rotate(" + num.ToString() + ")\">";
                this.AddToStream(base.m_string);
                base.m_string = "<text x=\"0\" y=\"0\" ";
            }
            else
            {
                base.m_string = "<text x=\"" + x.ToString() + "\" y=\"" + y.ToString() + "\" ";
            }
            base.m_string = base.m_string + this.FontProperties(base.Font);
            if (base.TextAlign == StringAlignment.Center)
            {
                base.m_string = base.m_string + " text-anchor=\"middle\"";
            }
            else if (base.TextAlign == StringAlignment.Far)
            {
                base.m_string = base.m_string + " text-anchor=\"end\"";
            }
            else
            {
                base.m_string = base.m_string + " text-anchor=\"start\"";
            }
            base.m_string = base.m_string + " fill=" + this.SVGColor(c) + ">";
            this.AddToStream(base.m_string);
            this.AddToStream(this.SVGText(text));
            this.AddToStream("</text>");
            if (degangle != 0.0)
            {
                this.AddToStream("</g>");
            }
        }

        public override void Draw(Rectangle r, Image image, bool transparent)
        {
        }

        public override void Draw(int x, int y, Image image)
        {
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
            if (base.Brush.Visible || base.Pen.Visible)
            {
                base.m_string = "<ellipse " + this.SVGEllipse(Utils.Round((float) x1), Utils.Round((float) y1), Utils.Round((float) x2), Utils.Round((float) y2));
                this.AddEnd(base.m_string + this.SVGBrushPen(true));
            }
        }

        public override void EraseBackground(int left, int top, int right, int bottom)
        {
        }

        public override void FillRegion(Brush brush, Region region)
        {
        }

        private string FontProperties(ChartFont f)
        {
            base.m_string = "font-family=\"" + f.Name.ToString() + "\" font-size=\"" + f.Size.ToString() + "pt\" ";
            if (f.Bold)
            {
                base.m_string = base.m_string + " font-weight=\"bold\"";
            }
            if (f.Italic)
            {
                base.m_string = base.m_string + " font-style=\"italic\"";
            }
            if (f.Underline)
            {
                base.m_string = base.m_string + " text-decoration=\"underline\"";
            }
            if (f.Strikeout)
            {
                base.m_string = base.m_string + " text-decoration=\"line-through\"";
            }
            return base.m_string;
        }

        private string GradientTransform(LinearGradientMode direction)
        {
            switch (direction)
            {
                case LinearGradientMode.Vertical:
                    return " x1=\"0%\" y1=\"100%\" x2=\"0%\" y2=\"0%\" ";

                case LinearGradientMode.ForwardDiagonal:
                    return " x1=\"0%\" y1=\"100%\" x2=\"100%\" y2=\"0%\" ";

                case LinearGradientMode.BackwardDiagonal:
                    return " x1=\"100%\" y1=\"100%\" x2=\"0%\" y2=\"0%\" ";
            }
            return " x1=\"100%\" y1=\"0%\" x2=\"0%\" y2=\"0%\" ";
        }

        protected internal override void InitWindow(Graphics graphics, Aspect a, Rectangle r, int MaxDepth)
        {
            base.InitWindow(graphics, a, r, MaxDepth);
            this.AddToStream(this.DocType);
            base.m_string = "<svg " + this.TheBounds();
            if (this.AntiAliasing)
            {
                base.m_string = base.m_string + " style=\"text-antialiasing:true\"";
            }
            this.AddToStream(base.m_string + ">");
        }

        protected override void InternalRect(ChartBrush b, Rectangle r, bool UsePen, bool IsRound)
        {
            if (b.Visible || (UsePen && base.Pen.Visible))
            {
                base.m_string = "<rect " + this.SVGRect(r) + this.SVGBrushPen(UsePen);
                if (IsRound)
                {
                    base.m_string = base.m_string + " rx=\"5\"";
                }
                this.AddEnd(base.m_string);
            }
        }

        public override void LineTo(int x, int y)
        {
            base.m_string = "<line x1=\"" + this.fx.ToString() + "\" y1=\"" + this.fy.ToString() + "\" ";
            string str = base.m_string;
            base.m_string = str + "x2=\"" + x.ToString() + "\" y2=\"" + y.ToString() + "\" fill=\"none\" " + this.SVGPen();
            this.AddEnd(base.m_string);
            base.fx = x;
            base.fy = y;
        }

        private string PenStyle(ChartPen ipen)
        {
            base.m_string = "";
            if (ipen.Style == DashStyle.Dot)
            {
                base.m_string = "2, 2";
            }
            else if (ipen.Style == DashStyle.Dash)
            {
                base.m_string = "4, 2";
            }
            else if (ipen.Style == DashStyle.DashDot)
            {
                base.m_string = "4, 2, 2, 2";
            }
            else if (ipen.Style == DashStyle.DashDotDot)
            {
                base.m_string = "4, 2, 2, 2, 2, 2";
            }
            return base.m_string;
        }

        public override void Pixel(int x, int y, int z, Color color)
        {
        }

        public override void Polygon(params PointDouble[] p)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                this.PrepareShape();
                this.AddEnd(this.SVGPoints(p));
            }
        }

        public override void Polygon(params Point[] p)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                this.PrepareShape();
                this.AddEnd(this.SVGPoints(p));
            }
        }

        public override void Polyline(params Point[] p)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                this.AddToStream("<polyline fill=\"none\" " + this.SVGPen());
                this.AddEnd(this.SVGPoints(p));
            }
        }

        public override void PrepareDrawImage()
        {
        }

        private void PrepareShape()
        {
            this.AddToStream("<polygon" + this.SVGBrushPen(true));
        }

        public override void RotateLabel(int x, int y, string text, double rotDegree)
        {
            if (base.Font.ShouldDrawShadow())
            {
                this.DoText(x + base.Font.Shadow.Width, y + base.Font.Shadow.Height, text, rotDegree, base.Font.Shadow.Color);
            }
            else
            {
                this.DoText(x, y, text, rotDegree, base.Font.Color);
            }
        }

        public override void SetClipRegion(Region region)
        {
        }

        public override void ShowImage(Graphics g)
        {
            this.AddToStream("</svg>");
        }

        private string SVGBrushPen(bool usePen)
        {
            if (base.Brush.Visible)
            {
                base.m_string = " fill=" + this.SVGColor(base.Brush.Color);
                float num = base.Brush.Transparency * 0.01f;
                if (num > 0f)
                {
                    string str = num.ToString().Replace(',', '.');
                    base.m_string = base.m_string + " fill-opacity=\"" + str + "\"";
                }
            }
            else
            {
                base.m_string = " fill=\"none\"";
            }
            if (usePen)
            {
                base.m_string = base.m_string + this.SVGPen();
            }
            return base.m_string;
        }

        private void SVGClip()
        {
            this.iClipStack++;
            this.iClipCount++;
            string str = "Clip" + this.iClipCount.ToString();
            this.AddToStream("<g clip-path=\"url(#" + str + ")\">");
            this.AddToStream("<defs>");
            this.AddToStream("<clipPath id=\"" + str + "\" style=\"clip-rule:nonzero\">");
        }

        private string SVGColor(Color c)
        {
            base.m_string = "";
            if (c == Color.Black)
            {
                base.m_string = "\"black\"";
            }
            else if (c == Color.Silver)
            {
                base.m_string = "\"silver\"";
            }
            else if (c == Color.Gray)
            {
                base.m_string = "\"gray\"";
            }
            else if (c == Color.White)
            {
                base.m_string = "\"white\"";
            }
            else if (c == Color.Maroon)
            {
                base.m_string = "\"maroon\"";
            }
            else if (c == Color.Red)
            {
                base.m_string = "\"red\"";
            }
            else if (c == Color.Purple)
            {
                base.m_string = "\"purple\"";
            }
            else if (c == Color.Fuchsia)
            {
                base.m_string = "\"fuchsia\"";
            }
            else if (c == Color.Green)
            {
                base.m_string = "\"green\"";
            }
            else if (c == Color.Lime)
            {
                base.m_string = "\"lime\"";
            }
            else if (c == Color.Olive)
            {
                base.m_string = "\"olive\"";
            }
            else if (c == Color.Yellow)
            {
                base.m_string = "\"yellow\"";
            }
            else if (c == Color.Navy)
            {
                base.m_string = "\"navy\"";
            }
            else if (c == Color.Blue)
            {
                base.m_string = "\"blue\"";
            }
            else if (c == Color.Teal)
            {
                base.m_string = "\"teal\"";
            }
            else if (c == Color.Aqua)
            {
                base.m_string = "\"aqua\"";
            }
            else
            {
                base.m_string = base.m_string + "\"rgb(" + c.R.ToString() + ",";
                base.m_string = base.m_string + c.G.ToString() + ",";
                base.m_string = base.m_string + c.B.ToString() + ")\"";
            }
            return base.m_string;
        }

        private string SVGEllipse(int x1, int y1, int x2, int y2)
        {
            int num = (x1 + x2) / 2;
            int num2 = (y1 + y2) / 2;
            int num3 = (x2 - x1) / 2;
            int num4 = (y2 - y1) / 2;
            base.m_string = "cx=\"" + num.ToString() + "\" cy=\"" + num2.ToString();
            string str = base.m_string;
            base.m_string = str + "\" rx=\"" + num3.ToString() + "\" ry=\"" + num4.ToString() + "\"";
            return base.m_string;
        }

        private void SVGEndClip()
        {
            this.AddToStream("</clipPath>");
            this.AddToStream("</defs>");
        }

        private string SVGPen()
        {
            if (base.Pen.Visible)
            {
                base.m_string = " stroke=" + this.SVGColor(base.Pen.Color);
                if (base.Pen.Width > 1)
                {
                    base.m_string = base.m_string + " stroke-width=\"" + base.Pen.Width.ToString() + "\"";
                }
                if (base.Pen.Style != DashStyle.Solid)
                {
                    base.m_string = base.m_string + " stroke-dasharray=\"" + this.PenStyle(base.Pen) + "\" ";
                }
                if (base.Pen.EndCap == LineCap.Square)
                {
                    base.m_string = base.m_string + " stroke-linecap=\"square\"";
                }
                else if (base.Pen.EndCap == LineCap.Flat)
                {
                    base.m_string = base.m_string + " stroke-linecap=\"flat\"";
                }
            }
            else
            {
                base.m_string = " stroke=\"none\"";
            }
            return base.m_string;
        }

        private string SVGPoints(params PointDouble[] p)
        {
            base.m_string = "points=\"";
            for (int i = p.GetLowerBound(0); i <= p.GetUpperBound(0); i++)
            {
                base.m_string = base.m_string + this.PointToStr(p[i].X, p[i].Y) + " ";
            }
            base.m_string = base.m_string + "\"";
            return base.m_string;
        }

        private string SVGPoints(params Point[] p)
        {
            base.m_string = "points=\"";
            for (int i = p.GetLowerBound(0); i <= p.GetUpperBound(0); i++)
            {
                base.m_string = base.m_string + this.PointToStr(p[i].X, p[i].Y) + " ";
            }
            base.m_string = base.m_string + "\"";
            return base.m_string;
        }

        private string SVGRect(Rectangle r)
        {
            int num = Utils.Round((float) r.Width);
            int num2 = Utils.Round((float) r.Height);
            base.m_string = "x=\"" + r.Left.ToString() + "\" y=\"" + r.Top.ToString() + "\" ";
            string str = base.m_string;
            base.m_string = str + " width=\"" + num.ToString() + "\" height=\"" + num2.ToString() + "\"";
            return base.m_string;
        }

        private string SVGText(string text)
        {
            char[] chArray = text.ToCharArray();
            text = "";
            foreach (char ch in chArray)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    text = text + ch;
                    continue;
                }
                ushort num = Convert.ToUInt16(ch);
                switch (num)
                {
                    case 0x26:
                    {
                        text = text + "&amp;";
                        continue;
                    }
                    case 0x27:
                    {
                        text = text + "&apos;";
                        continue;
                    }
                    case 0x22:
                    {
                        text = text + "&quot;";
                        continue;
                    }
                    case 60:
                    {
                        text = text + "&lt;";
                        continue;
                    }
                    case 0x3e:
                    {
                        text = text + "&gt;";
                        continue;
                    }
                }
                text = text + "&#" + num.ToString() + ";";
            }
            return text;
        }

        private string TheBounds()
        {
            base.m_string = "width=\"" + base.chart.ChartBounds.Width.ToString() + "px\" ";
            base.m_string = base.m_string + "height=\"" + base.chart.ChartBounds.Height.ToString() + "px\"";
            return base.m_string;
        }

        protected internal override void TransparentEllipse(int x1, int y1, int x2, int y2)
        {
        }

        public override void UnClip()
        {
            if (this.iClipStack > 0)
            {
                this.iClipStack--;
                this.AddToStream("</g>");
            }
        }
    }
}

