namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public abstract class Graphics3DVec : Graphics3D
    {
        protected int fx;
        protected int fy;
        protected Stream istream;
        protected string m_string;
        protected bool SupportsID;
        protected StreamWriter swFromStream;
        private Bitmap tmpBmp;
        protected StringBuilder tmpStr;

        public Graphics3DVec(Stream stream, Chart c) : base(c)
        {
            this.tmpStr = new StringBuilder();
            this.istream = stream;
            this.swFromStream = new StreamWriter(this.istream);
            base.UseBuffer = false;
            Bitmap image = new Bitmap(1, 1);
            base.g = Graphics.FromImage(image);
        }

        protected virtual void AddToStream(string text)
        {
            string str = text + Environment.NewLine;
            this.swFromStream.Write(str);
            this.swFromStream.Flush();
        }

        protected override void DoDrawString(int x, int y, string text, ChartBrush aBrush)
        {
            if (base.Font.ShouldDrawShadow())
            {
                this.DoText(x + base.Font.Shadow.Width, y + base.Font.Shadow.Height, text, 0.0, aBrush.Color);
            }
            else
            {
                this.DoText(x, y, text, 0.0, aBrush.Color);
            }
        }

        protected abstract void DoText(int x, int y, string text, double degangle, Color c);
        protected string FixSeparator(string text)
        {
            text = text.Replace(',', '.');
            return text;
        }

        protected virtual string FloatToStr(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }

        protected float FontDPI()
        {
            return 0.75f;
        }

        public override void HorizontalLine(int left, int right, int y)
        {
            this.MoveTo(left, y);
            this.LineTo(right, y);
        }

        protected abstract void InternalRect(ChartBrush b, System.Drawing.Rectangle r, bool UsePen, bool IsRound);
        protected override void Line(ChartPen p, Point a, Point b)
        {
            this.MoveTo(a.X, a.Y);
            this.LineTo(b.X, b.Y);
        }

        public override void Line(int x0, int y0, int x1, int y1)
        {
            this.MoveTo(x0, y0);
            this.LineTo(x1, y1);
        }

        public override SizeF MeasureString(ChartFont f, string text)
        {
            if (this.tmpBmp == null)
            {
                this.tmpBmp = new Bitmap(1, 1);
            }
            Graphics graphics = Graphics.FromImage(this.tmpBmp);
            graphics.SmoothingMode = base.SmoothingMode;
            graphics.TextRenderingHint = base.TextRenderingHint;
            return graphics.MeasureString(text, f.DrawingFont);
        }

        public override void MoveTo(int x, int y)
        {
            this.fx = x;
            this.fy = y;
        }

        protected virtual string PointToStr(PointDouble p)
        {
            return (this.FloatToStr(p.X) + "," + this.FloatToStr(p.Y));
        }

        protected virtual string PointToStr(Point p)
        {
            return (p.X.ToString() + "," + p.Y.ToString());
        }

        protected virtual string PointToStr(double X, double Y)
        {
            return (this.FloatToStr(X) + "," + this.FloatToStr(Y));
        }

        protected virtual string PointToStr(int X, int Y)
        {
            return (X.ToString() + "," + Y.ToString());
        }

        public override void Rectangle(System.Drawing.Rectangle r)
        {
            this.InternalRect(base.Brush, r, true, false);
        }

        public override void VerticalLine(int x, int top, int bottom)
        {
            this.MoveTo(x, bottom);
            this.LineTo(x, top);
        }
    }
}

