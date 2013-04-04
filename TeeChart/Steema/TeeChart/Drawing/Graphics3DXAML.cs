namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Text;

    public class Graphics3DXAML : Graphics3DVec
    {
        public Graphics3DXAML(Stream istream, Chart c) : base(istream, c)
        {
            base.iCanvasType = CanvasType.XAML;
            base.swFromStream = new StreamWriter(istream, Encoding.Unicode);
        }

        private string AddRectangle(Rectangle Rect)
        {
            base.OrientRectangle(ref Rect);
            return (" Canvas.Left=\"" + Rect.Left.ToString() + "\" Canvas.Top=\"" + Rect.Top.ToString() + "\" Width=\"" + Convert.ToString((int) ((Rect.Right - Rect.Left) + 1)) + "\" Height=\"" + Convert.ToString((int) ((Rect.Bottom - Rect.Top) + 1)) + "\"");
        }

        public override void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle)
        {
            if (base.Pen.Visible)
            {
                Rectangle rectBounds = Utils.FromLTRB(x1, y1, x2, y2);
                PointDouble p = base.PointFromEllipse(rectBounds, (double) Utils.Round((float) (startAngle + 90f)));
                PointDouble num2 = base.PointFromEllipse(rectBounds, (double) Utils.Round((float) ((startAngle + 90f) + sweepAngle)));
                this.PrepareShape("Path", true, false);
                base.m_string = "Data=\"M " + this.PointToStr(p) + " A " + this.PointToStr((double) (((double) rectBounds.Width) / 2.0), (double) (((double) rectBounds.Height) / 2.0)) + " 0 1 0 " + this.PointToStr(num2) + "\"/>";
                this.AddToStream(base.m_string);
            }
        }

        public override void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            if (base.Pen.Visible && base.Pen.Visible)
            {
                double num;
                double num2;
                base.CalcArcAngles(x1, y1, x2, y2, x3, y3, x4, y4, out num, out num2);
                this.Arc(x1, y1, x2, y2, (float) num, (float) num2);
            }
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
                    return "Gray";

                case KnownColor.Green:
                    return "Green";

                case KnownColor.Lime:
                    return "Lime";

                case KnownColor.Maroon:
                    return "Maroon";

                case KnownColor.Black:
                    return "Black";

                case KnownColor.Blue:
                    return "Blue";

                case KnownColor.Fuchsia:
                    return "Fuchsia";

                case KnownColor.Navy:
                    return "Navy";

                case KnownColor.Olive:
                    return "Olive";

                case KnownColor.Purple:
                    return "Purple";

                case KnownColor.Red:
                    return "Red";

                case KnownColor.White:
                    return "White";

                case KnownColor.Yellow:
                    return "Yellow";

                case KnownColor.Teal:
                    return "Teal";

                case KnownColor.Silver:
                    return "Silver";
            }
            return ("#" + Utils.ColorToHex(color, false));
        }

        protected override void DoText(int x, int y, string text, double degangle, Color c)
        {
            if (base.TextAlign == StringAlignment.Center)
            {
                x -= Utils.Round((float) (this.TextWidth(text) / 2f));
            }
            else if (base.TextAlign == StringAlignment.Far)
            {
                x -= Utils.Round(this.TextWidth(text));
            }
            x++;
            y++;
            this.AddToStream(("<TextBlock Canvas.Left=\"" + x.ToString() + "\" Canvas.Top=\"" + y.ToString() + "\" Foreground=" + this.XAMLColor(c) + " FontFamily=\"" + base.Font.Name + "\" FontSize=\"" + Convert.ToString(Utils.Round((double) (base.Font.Size * 1.4))) + "\" " + this.FontStyle()) + ">");
            this.AddToStream(text);
            if (base.Font.Underline)
            {
                this.AddToStream("  <TextBlock.TextDecorations>");
                this.AddToStream("    <TextDecorationCollection>");
                this.AddToStream("      <TextDecoration");
                this.AddToStream("        PenThicknessUnit=\"FontRecommended\">");
                this.AddToStream("        <TextDecoration.Pen>");
                this.AddToStream("          <Pen Brush=" + this.XAMLColor(base.Font.Color) + " Thickness=\"1\" />");
                this.AddToStream("        </TextDecoration.Pen>");
                this.AddToStream("      </TextDecoration>");
                this.AddToStream("    </TextDecorationCollection>");
                this.AddToStream("  </TextBlock.TextDecorations>");
            }
            this.AddToStream("</TextBlock>");
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

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
        }

        public override void Ellipse(int x1, int y1, int x2, int y2)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                this.PrepareShape("Ellipse");
                this.AddToStream(" Canvas.Left=\"" + x1.ToString() + "\" Canvas.Top=\"" + y1.ToString() + "\" Height=\"" + Convert.ToString((int) ((y2 - y1) + 1)) + "\" Width=\"" + Convert.ToString((int) ((x2 - x1) + 1)) + "\"");
                this.AddToStream("/>");
            }
        }

        public override void EraseBackground(int left, int top, int right, int bottom)
        {
        }

        public override void FillRegion(Brush brush, Region region)
        {
        }

        private string FontStyle()
        {
            base.tmpStr.Length = 0;
            if (base.Font.Bold)
            {
                base.tmpStr.Append(" FontWeight=\"Bold\"");
            }
            if (base.Font.Italic)
            {
                base.tmpStr.Append(" FontStyle=\"Italic\"");
            }
            return base.tmpStr.ToString();
        }

        protected internal override void InitWindow(Graphics graphics, Aspect a, Rectangle r, int MaxDepth)
        {
            base.InitWindow(graphics, a, r, MaxDepth);
            string name = "Chart";
            if ((base.Chart.Parent != null) && !Utils.IsNullOrEmpty(base.Chart.Parent.GetControl().Name))
            {
                name = base.Chart.Parent.GetControl().Name;
            }
            this.AddToStream("<Canvas Name=\"" + name + "\" Width=\"" + base.Chart.Width.ToString() + "\" Height=\"" + base.Chart.Height.ToString() + "\" Background=" + this.XAMLColor(base.Chart.Panel.Color) + " ClipToBounds=\"True\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" >");
        }

        protected override void InternalRect(ChartBrush brush, Rectangle Rect, bool UsePen, bool IsRound)
        {
            base.Brush = brush;
            if (brush.Visible || (UsePen && base.Pen.Visible))
            {
                this.PrepareShape("Rectangle", UsePen);
            }
            string str = "";
            if (IsRound)
            {
                str = str + " RadiusX=\"30\" RadiusY=\"30\"";
            }
            str = str + this.AddRectangle(Rect);
            if (brush.GradientVisible)
            {
                this.AddToStream(str + ">");
                this.XAMLGradient();
                this.AddToStream("</Rectangle>");
            }
            else
            {
                this.AddToStream(str + "/>");
            }
        }

        public override void LineTo(int x, int y)
        {
            this.AddToStream("<Line X1=\"" + this.fx.ToString() + "\" X2=\"" + x.ToString() + "\" Y1=\"" + this.fy.ToString() + "\" Y2=\"" + y.ToString() + "\" " + this.XAMLPen() + "/>");
            base.fx = x;
            base.fy = y;
        }

        private string PenStyle()
        {
            switch (base.Pen.Style)
            {
                case DashStyle.Dash:
                    return "Dash";

                case DashStyle.Dot:
                    return "Dot";

                case DashStyle.DashDot:
                    return "DashDot";

                case DashStyle.DashDotDot:
                    return "DashDotDot";
            }
            return "";
        }

        private string PenWidth()
        {
            if ((base.Pen.Style != DashStyle.Solid) && (base.Pen.Width == 1))
            {
                return "StrokeThickness=\"2\" ";
            }
            if (base.Pen.Width > 1)
            {
                return ("StrokeThickness=\"" + base.Pen.Width.ToString() + "\" ");
            }
            return "";
        }

        public override void Pixel(int x, int y, int z, Color color)
        {
            base.Calc3DPos(ref x, ref y, z);
            Color color2 = base.Pen.Color;
            base.Pen.Color = color;
            this.MoveTo(x, y);
            this.LineTo(x, y);
            base.Pen.Color = color2;
        }

        public override void Polygon(params PointDouble[] p)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                this.PrepareShape("Polygon");
                base.tmpStr.Length = 0;
                base.tmpStr.Append(" Points=\"");
                for (int i = 0; i < p.Length; i++)
                {
                    base.tmpStr.Append(this.PointToStr(p[i].X, p[i].Y) + " ");
                }
                base.tmpStr.Append("\"/>");
                this.AddToStream(base.tmpStr.ToString());
            }
        }

        public override void Polygon(params Point[] p)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                this.PrepareShape("Polygon");
                base.tmpStr.Length = 0;
                base.tmpStr.Append(" Points=\"");
                for (int i = 0; i < p.Length; i++)
                {
                    base.tmpStr.Append(this.PointToStr(p[i].X, p[i].Y) + " ");
                }
                base.tmpStr.Append("\"/>");
                this.AddToStream(base.tmpStr.ToString());
            }
        }

        public override void Polyline(params Point[] p)
        {
            if (base.Pen.Visible)
            {
                base.tmpStr.Length = 0;
                this.PrepareShape("Polyline", true, false);
                base.tmpStr.Append(" Points=\"");
                foreach (Point point in p)
                {
                    base.tmpStr.Append(this.PointToStr(point) + " ");
                }
                this.AddToStream(base.tmpStr.ToString() + "\"/>");
            }
        }

        public override void PrepareDrawImage()
        {
        }

        private string PrepareShape(string Prefix)
        {
            return this.PrepareShape(Prefix, true, true);
        }

        private string PrepareShape(string Prefix, bool UsePen)
        {
            return this.PrepareShape(Prefix, UsePen, true);
        }

        private string PrepareShape(string Prefix, bool UsePen, bool UseBrush)
        {
            string text = "<" + Prefix;
            if ((UseBrush && base.Brush.Visible) && !base.Brush.GradientVisible)
            {
                text = text + " Fill=" + this.XAMLColor(base.Brush.Color);
            }
            if (UsePen && base.Pen.Visible)
            {
                text = text + " " + this.XAMLPen();
            }
            this.AddToStream(text);
            return text;
        }

        public override void RotateLabel(int x, int y, string text, double rotDegree)
        {
            this.DoText(x, y, text, rotDegree, base.Font.Color);
        }

        public override void SetClipRegion(Region region)
        {
        }

        public override void ShowImage(Graphics g)
        {
            this.AddToStream("</Canvas>");
        }

        private string StartEndPoints()
        {
            string str = "";
            switch (base.Brush.Gradient.Direction)
            {
                case LinearGradientMode.Horizontal:
                case LinearGradientMode.ForwardDiagonal:
                case LinearGradientMode.BackwardDiagonal:
                    return str;

                case LinearGradientMode.Vertical:
                    return " StartPoint=\"0.5,0\" EndPoint=\"0.5,1\"";
            }
            return str;
        }

        protected internal override void TransparentEllipse(int x1, int y1, int x2, int y2)
        {
        }

        public override void UnClip()
        {
        }

        private string XAMLColor(Color color)
        {
            if (color.IsKnownColor)
            {
                return ("\"" + this.ColorInternal(color) + "\"");
            }
            return ("\"#" + Utils.ColorToHex(color) + "\"");
        }

        protected void XAMLGradient()
        {
            this.AddToStream(" <Rectangle.Fill>");
            if (base.Brush.Gradient.Style.Direction == PathGradientMode.Radial)
            {
                base.m_string = "RadialGradientBrush";
            }
            else
            {
                base.m_string = "LinearGradientBrush";
            }
            this.AddToStream("   <" + base.m_string + this.StartEndPoints() + ">");
            this.AddToStream("    <GradientStop Offset=\"0\" Color=" + this.XAMLColor(base.Brush.Gradient.StartColor) + "/>");
            this.AddToStream("    <GradientStop Offset=\"1\" Color=" + this.XAMLColor(base.Brush.Gradient.EndColor) + "/>");
            this.AddToStream("  </" + base.m_string + ">");
            this.AddToStream(" </Rectangle.Fill>");
        }

        protected string XAMLPen()
        {
            string str = this.PenWidth() + "Stroke=" + this.XAMLColor(base.Pen.Color);
            if (base.Pen.Style != DashStyle.Solid)
            {
                str = str + " StrokeDashArray=\"{Binding Source={x:Static DashStyles." + this.PenStyle() + "}, Path=Dashes}\" StrokeDashCap=\"Round\"";
            }
            return str;
        }
    }
}

