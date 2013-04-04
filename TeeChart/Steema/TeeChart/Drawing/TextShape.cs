namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class TextShape : Shape
    {
        private bool autosize;
        private bool cliptext;
        private string crlf;
        protected internal string defaultText;
        private ChartFont font;
        protected internal bool iDrawText;
        private string[] lines;
        private TextShapeStyle shapeStyle;
        public ShapeSize Size;
        private StringAlignment textalign;
        private Steema.TeeChart.Drawing.TextFormat textFormat;

        public TextShape() : this(null)
        {
        }

        public TextShape(Chart c) : base(c)
        {
            this.crlf = Environment.NewLine;
            this.Size = new ShapeSize();
            this.cliptext = true;
            this.iDrawText = true;
            this.defaultText = "";
            this.autosize = true;
        }

        [Obsolete("Please use the TextShape.Clone method"), Description("Assign all properties from a TextShape to another.")]
        public void Assign(TextShape s)
        {
            if (s != null)
            {
                base.Assign(s);
                if (s.font != null)
                {
                    this.Font = s.font;
                }
                this.lines = s.lines;
                this.shapeStyle = s.shapeStyle;
                this.textalign = s.textalign;
                this.Size = s.Size;
            }
        }

        protected override void AssignShape(Shape s)
        {
            base.AssignShape(s);
            TextShape shape = s as TextShape;
            if (shape != null)
            {
                if (this.font != null)
                {
                    shape.font = this.font.Clone() as ChartFont;
                }
                shape.lines = this.lines;
                shape.shapeStyle = this.shapeStyle;
                shape.textalign = this.textalign;
                shape.Size = this.Size;
            }
        }

        protected internal void CalcBounds()
        {
            this.CalcBounds(base.chart.Graphics3D);
        }

        protected internal void CalcBounds(Graphics3D g)
        {
            int num5;
            int num = 4;
            int num2 = 2;
            g.Font = this.Font;
            int num6 = base.chart.MultiLineTextWidth(this.Text, out num5);
            int num7 = g.FontHeight * num5;
            int num8 = base.Pen.Visible ? ((1 + base.Pen.Width) / 2) : 0;
            num6 += num8;
            num7 += num8;
            int num3 = num6 + (2 * num);
            int num4 = num7 + (2 * num2);
            base.iLeft = 0;
            base.iTop = 0;
            base.iRight = num3 - 1;
            base.iBottom = num4 - 1;
            base.iWidth = num3 - 1;
            base.iHeight = num4 - 1;
            this.Size.Width = num3 - 1;
            this.Size.Height = num4 - 1;
        }

        public void DrawRectRotated(Graphics3D g, Rectangle rect, int angle, int aZ)
        {
            if (!base.bTransparent)
            {
                if (((angle == 0) && base.Shadow.bVisible) && (base.bBrush.visible && !g.SupportsFullRotation))
                {
                    base.shadow.Draw(g, rect, angle, aZ);
                }
                if (base.Gradient.Visible && (angle == 0))
                {
                    base.Gradient.Draw(g, rect);
                }
                else
                {
                    g.Brush = base.bBrush;
                }
                g.Pen = base.Pen;
                this.InternalDrawShape(g, rect, 0x10, angle, aZ);
            }
            if (base.bBevel != null)
            {
                base.bBevel.Draw(g, rect);
            }
            if (base.bImageBevel != null)
            {
                base.bImageBevel.Draw(g, rect, 0);
            }
        }

        protected virtual void DrawString(Graphics3D g, int x, int y, int t, int tmpHeight, string[] s)
        {
            g.TextOut(x, y + (t * tmpHeight), s[t]);
        }

        protected internal void DrawText()
        {
            this.DrawText(base.chart.Graphics3D, base.ShapeBounds);
        }

        protected internal void DrawText(Graphics3D g, Rectangle rect)
        {
            if (base.bBevel != null)
            {
                if (base.bBevel.inner != BevelStyles.None)
                {
                    rect.Inflate(1, 1);
                }
                if (base.bBevel.outer != BevelStyles.None)
                {
                    rect.Inflate(1, 1);
                }
            }
            switch (this.shapeStyle)
            {
                case TextShapeStyle.Rectangle:
                    base.BorderRound = 0;
                    break;

                case TextShapeStyle.RoundRectangle:
                    base.BorderRound = 8;
                    break;
            }
            base.Paint(g, rect);
            if (this.iDrawText && (this.lines != null))
            {
                int num2;
                int num3;
                g.Font = this.Font;
                int fontHeight = g.FontHeight;
                string text = this.Text;
                int num4 = base.chart.MultiLineTextWidth(text, out num3);
                switch (this.textalign)
                {
                    case StringAlignment.Center:
                        num2 = (((rect.Left + rect.Right) - num4) - 2) / 2;
                        break;

                    case StringAlignment.Far:
                        num2 = (rect.Right - num4) - 2;
                        break;

                    default:
                        num2 = rect.X + 2;
                        break;
                }
                if (this.cliptext)
                {
                    g.ClearClipRegions();
                    g.ClipRectangle(rect);
                }
                for (int i = 0; i < num3; i++)
                {
                    this.DrawString(g, num2, rect.Y + 2, i, fontHeight, this.lines);
                }
                if (this.cliptext)
                {
                    g.UnClip();
                }
            }
        }

        private void InternalDrawShape(Graphics3D g, Rectangle aRect, int teeDefaultRoundSize, int angle, int aZ)
        {
            if (angle > 0)
            {
                g.Polygon(Graphics3D.RotateRectangle(aRect, angle));
            }
            else if (g.SupportsFullRotation)
            {
                g.Rectangle(aRect, aZ);
            }
            else if (this.shapeStyle == TextShapeStyle.Rectangle)
            {
                g.Rectangle(aRect);
            }
            else
            {
                g.RoundRectangle(aRect);
            }
        }

        public void Paint()
        {
            this.Paint(base.chart.graphics3D, base.ShapeBounds);
        }

        public override void Paint(Graphics3D g, Rectangle rect)
        {
            this.DrawText(g, rect);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.font != null)
            {
                this.font.Chart = base.chart;
            }
        }

        protected virtual bool ShouldSerializeLines()
        {
            return (this.Text != this.defaultText);
        }

        [Description("Text shape size is automatic"), DefaultValue(true)]
        public bool AutoSize
        {
            get
            {
                return this.autosize;
            }
            set
            {
                if (this.autosize != value)
                {
                    this.autosize = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Clip Text."), DefaultValue(true)]
        public bool ClipText
        {
            get
            {
                return this.cliptext;
            }
            set
            {
                if (this.cliptext != value)
                {
                    this.cliptext = value;
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines the font attributes used to output TextShape strings.")]
        public ChartFont Font
        {
            get
            {
                if (this.font == null)
                {
                    this.font = new ChartFont(base.chart);
                }
                return this.font;
            }
            set
            {
                this.font = value;
            }
        }

        public string[] Lines
        {
            get
            {
                return this.lines;
            }
            set
            {
                this.lines = value;
                this.Invalidate();
            }
        }

        protected int LinesLength
        {
            get
            {
                if (this.lines != null)
                {
                    return this.lines.Length;
                }
                return 0;
            }
        }

        [Obsolete("Please use Shadow.Size property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(3), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public int ShadowSize
        {
            get
            {
                return base.Shadow.Width;
            }
            set
            {
                base.Shadow.Width = value;
            }
        }

        [Description("Shape may be rectagular or rounded rectangular."), DefaultValue(0)]
        public TextShapeStyle ShapeStyle
        {
            get
            {
                return this.shapeStyle;
            }
            set
            {
                if (this.shapeStyle != value)
                {
                    this.shapeStyle = value;
                    this.Invalidate();
                }
            }
        }

        [Description("The Text property is used to display customized strings inside Shapes."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Text
        {
            get
            {
                if (this.LinesLength != 0)
                {
                    return string.Join(this.crlf, this.lines);
                }
                return "";
            }
            set
            {
                if (this.Text != value)
                {
                    this.lines = null;
                    this.lines = value.Replace(this.crlf, "\n").Split(new char[] { '\n' });
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("Horizontal alignment of displayed text.")]
        public StringAlignment TextAlign
        {
            get
            {
                return this.textalign;
            }
            set
            {
                if (this.textalign != value)
                {
                    this.textalign = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("Determines if Text is drawn as Normal or HTML styles.")]
        public Steema.TeeChart.Drawing.TextFormat TextFormat
        {
            get
            {
                return this.textFormat;
            }
            set
            {
                if (this.textFormat != value)
                {
                    this.textFormat = value;
                }
                this.Invalidate();
            }
        }
    }
}

