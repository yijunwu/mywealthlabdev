namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Export;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;

    public class Graphics3DFlex : Graphics3DVec, ICanvasHyperlinks, ICanvasToolTips
    {
        private bool embeddedImages;
        private bool iAddedInitApp;
        private int iApplication;
        private string iIdent;
        private int iImageID;
        private List<string> iItems;
        private string imagePath;
        private List<string> iPath;
        private int iScript;
        private bool iSmallDots;
        private int iTransp;

        public Graphics3DFlex(Stream istream, Chart c) : base(istream, c)
        {
            base.iCanvasType = CanvasType.Flex;
            base.swFromStream = new StreamWriter(istream, Encoding.Unicode);
            base.UseBuffer = false;
            this.embeddedImages = true;
            base.SupportsID = true;
            this.iPath = new List<string>();
            this.iItems = new List<string>();
            this.iImageID = 0;
        }

        public void AddLink(int x, int y, string Text, string URL, string Hint)
        {
            this.AddTag("mx:LinkButton", "label=\"" + Text + "\" " + this.PointToStr(x, y) + " toolTip=\"" + Hint + "\" " + this.FlexFont(base.Font) + "click=\"navigateToURL(new URLRequest('''+URL+'''), '''+Hint+''')\"");
        }

        private void AddTag(string ATag, string AText)
        {
            this.AddToStream(this.iIdent + "<" + ATag + this.TheID() + " " + AText + "/>");
        }

        public void AddToolTip(string Entity, string ToolTip)
        {
            bool iAddedInitApp = this.iAddedInitApp;
        }

        public override void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle)
        {
            if (base.Pen.Visible)
            {
                this.AddTag("tee:Arc", "x0=\"" + x1.ToString() + "\" y0=\"" + y1.ToString() + "\" x1=\"" + x2.ToString() + "\" y1=\"" + y2.ToString() + "\" startAngle=\"" + this.FloatToStr((double) startAngle) + "\" endAngle=\"" + this.FloatToStr((double) (startAngle + sweepAngle)) + "\" " + this.FlexAlpha(this.CalcAlpha(true)) + this.PenColor() + this.PenWidth());
            }
        }

        public override void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            if (base.Pen.Visible)
            {
                double num;
                double num2;
                base.CalcArcAngles(x1, y1, x2, y2, x3, y3, x4, y4, out num, out num2);
                this.Arc(x1, y1, x2, y2, (float) num, (float) num2);
            }
        }

        private string BrushColor()
        {
            if (!base.Brush.Visible)
            {
                return " brushColor=\"\"";
            }
            if (base.Brush.Gradient.Visible)
            {
                return this.FlexGradient(base.Brush.Gradient);
            }
            return (" brushColor=" + this.FlexColor(base.Brush.Color));
        }

        private int CalcAlpha(bool penOnly)
        {
            if (penOnly)
            {
                return base.Pen.Transparency;
            }
            if (base.Brush.Visible)
            {
                return base.Brush.Transparency;
            }
            if (base.Pen.Visible)
            {
                return base.Pen.Transparency;
            }
            return 0;
        }

        private string CalcResult(Image tmp)
        {
            string str2 = this.GraphicsExtension(tmp.RawFormat);
            string str = "TeeChart_Flex_Temp_" + this.iImageID.ToString() + "." + str2;
            this.iImageID++;
            tmp.Save(this.ImagePath + @"\" + str);
            return str;
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
        }

        private string ColorInternal(Color color)
        {
            switch (color.ToKnownColor())
            {
                case KnownColor.Gray:
                    return "gray";

                case KnownColor.Green:
                    return "green";

                case KnownColor.Lime:
                    return "lime";

                case KnownColor.Maroon:
                    return "maroon";

                case KnownColor.Black:
                    return "black";

                case KnownColor.Blue:
                    return "blue";

                case KnownColor.Fuchsia:
                    return "fuchsia";

                case KnownColor.Navy:
                    return "navy";

                case KnownColor.Olive:
                    return "olive";

                case KnownColor.Purple:
                    return "purple";

                case KnownColor.Red:
                    return "red";

                case KnownColor.White:
                    return "white";

                case KnownColor.Yellow:
                    return "yellow";

                case KnownColor.Teal:
                    return "teal";

                case KnownColor.Silver:
                    return "silver";
            }
            return ("0x" + Utils.ColorToHex(color, false));
        }

        private string CurrentID()
        {
            base.tmpStr.Length = 0;
            if (this.iPath.Count == 0)
            {
                if (base.Chart.Parent != null)
                {
                    base.tmpStr.Append(base.Chart.Parent.GetControl().Name + "_");
                }
                else
                {
                    base.tmpStr.Append("Chart");
                }
            }
            else
            {
                base.tmpStr.Append(this.iPath[0]);
            }
            for (int i = 1; i < this.iPath.Count; i++)
            {
                base.tmpStr.Append("_" + this.iPath[i]);
            }
            return base.tmpStr.ToString();
        }

        private string DashLenGap()
        {
            int num = 4;
            int num2 = 4;
            switch (base.Pen.Style)
            {
                case DashStyle.Dot:
                    num = 2;
                    num2 = 2;
                    break;

                case DashStyle.DashDot:
                    num = 4;
                    num2 = 2;
                    break;

                case DashStyle.DashDotDot:
                    num = 4;
                    num2 = 3;
                    break;
            }
            return (" len=\"" + num.ToString() + "\" gap=\"" + num2.ToString() + "\"");
        }

        protected override void DoText(int x, int y, string text, double degangle, Color c)
        {
            if (base.TextAlign == StringAlignment.Far)
            {
                x -= Utils.Round(this.TextWidth(text));
            }
            else if (base.TextAlign == StringAlignment.Center)
            {
                x -= Utils.Round((double) (((double) this.TextWidth(text)) / 2.0));
            }
            this.AddTag("mx:Label", "text=\"" + text + "\" " + this.PointToStr(x, y) + this.FlexFont(base.Font) + " textAlign=\"left\"");
        }

        public override void Draw(Rectangle r, Image image, bool transparent)
        {
            if (image != null)
            {
                this.AddTag("mx:Image", this.PointToStr(r.X, r.Y) + "  scaleX=\"" + this.FloatToStr(Convert.ToDouble(r.Width) / Convert.ToDouble(image.Width)) + "\"  scaleY=\"" + this.FloatToStr(Convert.ToDouble(r.Height) / Convert.ToDouble(image.Height)) + "\"  source=\"" + this.ImageSource(image) + "\"");
            }
        }

        public override void Draw(int x, int y, Image image)
        {
            if (image != null)
            {
                this.AddTag("mx:Image", this.PointToStr(x, y) + " source=\"" + this.ImageSource(image) + "\"");
            }
        }

        public override void DrawBeziers(params Point[] p)
        {
        }

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
        }

        public override void Ellipse(int x1, int y1, int x2, int y2)
        {
            this.AddTag("tee:Ellipse", this.PointToStr(x1, y1) + " " + this.FlexSize(x2 - x1, y2 - y1) + this.FlexAlpha(this.CalcAlpha(false)) + this.BrushColor() + this.PenColor() + this.PenWidth());
        }

        public override void EraseBackground(int left, int top, int right, int bottom)
        {
        }

        public override void FillRegion(Brush brush, Region region)
        {
        }

        private string FlexAlpha(int transparency)
        {
            if (transparency == 0)
            {
                return "";
            }
            return (" alpha=\"" + this.FloatToStr(1.0 - (((double) transparency) / 100.0)) + "\"");
        }

        private string FlexColor(Color aColor)
        {
            return ("\"" + this.ColorInternal(aColor) + "\"");
        }

        private string FlexFont(ChartFont AFont)
        {
            return (this.FontStyle(AFont) + this.FontWeight(AFont) + " fontSize=\"" + this.FloatToStr(AFont.Size * 1.2) + "\" " + this.TextDecoration(AFont) + " fontFamily=\"" + AFont.Name + "\"  color=" + this.FlexColor(AFont.Color) + " ");
        }

        private string FlexGradient(Gradient gradient)
        {
            string str = "Linear";
            return (" gradientType=\"" + str + "\" gradientDir=\"" + this.GradientDirection(gradient.Direction) + "\" startColor=" + this.FlexColor(gradient.StartColor) + " endColor=" + this.FlexColor(gradient.EndColor) + " ");
        }

        private string FlexPoints(PointDouble[] Points)
        {
            base.tmpStr.Length = 0;
            base.tmpStr.Append("points=\"[");
            if (Points.Length > 0)
            {
                base.tmpStr.Append(this.PointToStr(Points[0]));
                for (int i = 1; i < Points.Length; i++)
                {
                    base.tmpStr.Append("," + this.PointToStr(Points[i]));
                }
            }
            base.tmpStr.Append("]\"");
            return base.tmpStr.ToString();
        }

        private string FlexPoints(Point[] Points)
        {
            base.tmpStr.Length = 0;
            base.tmpStr.Append("points=\"[");
            if (Points.Length > 0)
            {
                base.tmpStr.Append(this.PointToStr(Points[0]));
                for (int i = 1; i < Points.Length; i++)
                {
                    base.tmpStr.Append("," + this.PointToStr(Points[i]));
                }
            }
            base.tmpStr.Append("]\"");
            return base.tmpStr.ToString();
        }

        private string FlexSize(Rectangle R)
        {
            return this.FlexSize(R.Width, R.Height);
        }

        private string FlexSize(int w, int h)
        {
            return ("width=\"" + Utils.Round((float) w).ToString() + "\" height=\"" + Utils.Round((float) h).ToString() + "\"");
        }

        private string FontStyle(ChartFont AFont)
        {
            if (AFont.Italic)
            {
                return " fontStyle=\"italic\" ";
            }
            return "";
        }

        private string FontWeight(ChartFont AFont)
        {
            if (AFont.Bold)
            {
                return " fontWeight=\"bold\" ";
            }
            return "";
        }

        private string GradientDirection(LinearGradientMode direction)
        {
            switch (direction)
            {
                case LinearGradientMode.Horizontal:
                    return "RightLeft";

                case LinearGradientMode.Vertical:
                    return "BottomTop";

                case LinearGradientMode.ForwardDiagonal:
                    return "LeftRight";

                case LinearGradientMode.BackwardDiagonal:
                    return "TopBottom";
            }
            return "";
        }

        private string GraphicsExtension(ImageFormat format)
        {
            if (format == ImageFormat.Emf)
            {
                return "emf";
            }
            if (format == ImageFormat.Gif)
            {
                return "gif";
            }
            if ((format != ImageFormat.Jpeg) && (format == ImageFormat.Tiff))
            {
                return "tif";
            }
            return "jpg";
        }

        private string ImageFileName(Image graphic)
        {
            if (graphic.RawFormat == ImageFormat.Bmp)
            {
                JPEGFormat format = new JPEGFormat(base.Chart);
                MemoryStream stream = new MemoryStream();
                Bitmap b = new Bitmap(graphic);
                format.Save(stream, b, b.Width, b.Height);
                b = new Bitmap(stream);
                return this.CalcResult(b);
            }
            return this.CalcResult(graphic);
        }

        private string ImageSource(Image graphic)
        {
            if (this.EmbeddedImages)
            {
                return ("@Embed('" + this.ImageFileName(graphic) + "')");
            }
            return this.ImageFileName(graphic);
        }

        protected internal override void InitWindow(Graphics graphics, Aspect a, Rectangle r, int MaxDepth)
        {
            base.InitWindow(graphics, a, r, MaxDepth);
            this.iAddedInitApp = false;
            this.AddToStream("<?xml version=\"1.0\"?>");
            this.AddToStream("<!-- Generated by TeeChart for .NET -->");
            this.AddToStream("<mx:Application xmlns:mx=\"http://www.adobe.com/2006/mxml\"");
            this.AddToStream("                xmlns:tee=\"com.steema.graphics.*\"");
            this.AddToStream(">");
            this.AddToStream("    <mx:Script>");
            this.AddToStream("        <![CDATA[");
            this.AddToStream("       ]]>");
            this.AddToStream("    </mx:Script>");
            this.AddToStream("    <mx:Canvas " + this.FlexSize(r) + ">");
        }

        protected override void InternalRect(ChartBrush b, Rectangle r, bool UsePen, bool IsRound)
        {
            if (b.Image != null)
            {
                this.Draw(r, b.Image, b.ImageTransparent);
            }
            else if (IsRound)
            {
                this.AddTag("tee:RoundRect", this.PointToStr(r.X, r.Y) + " " + this.FlexSize(r) + " " + this.RXY(r) + this.FlexAlpha(this.CalcAlpha(false)) + this.PenColor() + this.BrushColor() + this.PenWidth());
            }
            else
            {
                this.AddTag("tee:Rectangle", this.PointToStr(r.X, r.Y) + " " + this.FlexSize(r) + this.FlexAlpha(this.CalcAlpha(false)) + this.PenColor() + this.BrushColor() + this.PenWidth());
            }
        }

        public override void LineTo(int x, int y)
        {
            if (base.Pen.Style == DashStyle.Solid)
            {
                this.AddTag("tee:Line", this.Pos0() + " " + this.Pos1(x, y) + this.FlexAlpha(this.CalcAlpha(true)) + this.PenColor() + this.PenWidth());
            }
            else
            {
                this.AddTag("tee:DashLine", this.Pos0() + " " + this.Pos1(x, y) + this.FlexAlpha(this.CalcAlpha(true)) + this.PenColor() + this.PenWidth() + this.DashLenGap());
            }
        }

        private string PenColor()
        {
            if (base.Pen.Visible)
            {
                return (" strokeColor=" + this.FlexColor(base.Pen.Color));
            }
            return " strokeColor=\"\"";
        }

        private string PenWidth()
        {
            if (base.Pen.Width == 1)
            {
                return "";
            }
            return (" strokeWidth=\"" + base.Pen.Width.ToString() + "\"");
        }

        public override void Pixel(int x, int y, int z, Color color)
        {
        }

        protected override string PointToStr(int X, int Y)
        {
            return ("x=\"" + X.ToString() + "\" y=\"" + Y.ToString() + "\"");
        }

        public override void Polygon(params PointDouble[] p)
        {
            this.AddTag("tee:Polygon", this.FlexPoints(p) + " " + this.FlexAlpha(this.CalcAlpha(false)) + this.BrushColor() + this.PenColor() + this.PenWidth());
        }

        public override void Polygon(params Point[] p)
        {
            this.AddTag("tee:Polygon", this.FlexPoints(p) + " " + this.FlexAlpha(this.CalcAlpha(false)) + this.BrushColor() + this.PenColor() + this.PenWidth());
        }

        public override void Polyline(params Point[] p)
        {
            this.AddTag("tee:Polyline", string.Concat(new object[] { this.FlexPoints(p), ' ', this.FlexAlpha(this.CalcAlpha(true)), this.PenColor(), this.PenWidth() }));
        }

        private string Pos0()
        {
            return ("x0=\"" + this.fx.ToString() + "\" y0=\"" + this.fy.ToString() + "\"");
        }

        private string Pos1(int x, int y)
        {
            return ("x1=\"" + Utils.Round((float) x).ToString() + "\" y1=\"" + Utils.Round((float) y).ToString() + "\"");
        }

        public override void PrepareDrawImage()
        {
        }

        public override void RotateLabel(int x, int y, string text, double rotDegree)
        {
            this.DoText(x, y, text, rotDegree, base.Font.Color);
        }

        private string RXY(Rectangle r)
        {
            return "rx=\"8\" ry=\"8\"";
        }

        public override void SetClipRegion(Region region)
        {
        }

        public override void ShowImage(Graphics g)
        {
            this.AddToStream("    </mx:Canvas>");
            this.AddToStream("</mx:Application>");
        }

        private string TextDecoration(ChartFont AFont)
        {
            if (AFont.Underline)
            {
                return " textDecoration=\"underline\" ";
            }
            return "";
        }

        private string TheID()
        {
            string item = "";
            string str2 = this.CurrentID();
            int num = 0;
            item = str2;
            while (this.iItems.IndexOf(item) != -1)
            {
                num++;
                item = str2 + "_" + num.ToString();
            }
            this.iItems.Add(item);
            return (" id=\"" + item + "\"");
        }

        protected internal override void TransparentEllipse(int x1, int y1, int x2, int y2)
        {
        }

        public override void UnClip()
        {
        }

        public bool EmbeddedImages
        {
            get
            {
                return this.embeddedImages;
            }
            set
            {
                this.embeddedImages = value;
            }
        }

        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }
            set
            {
                this.imagePath = value;
            }
        }
    }
}

