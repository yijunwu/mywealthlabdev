namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Description("Properties to draw a shadow."), Editor(typeof(Shadow.ShadowComponentEditor), typeof(UITypeEditor))]
    public class Shadow : TeeBase, ICloneable
    {
        protected ChartBrush bBrush;
        protected internal bool bVisible;
        protected internal int defaultSize;
        protected internal bool defaultVisible;
        protected bool fullDraw;
        private int height;
        private bool smooth;
        private int smoothBlur;
        private int width;

        public Shadow(Chart c) : base(c)
        {
            this.height = 3;
            this.width = 3;
            this.defaultSize = 3;
            this.bBrush = new ChartBrush(c, System.Drawing.Color.DarkGray);
            this.defaultVisible = false;
            this.smooth = false;
        }

        public Shadow(Chart c, int size) : this(c)
        {
            this.defaultSize = size;
            this.width = size;
            this.height = size;
        }

        public void Assign(Shadow value)
        {
            this.height = value.height;
            this.width = value.width;
            this.bVisible = value.bVisible;
            this.bBrush = value.bBrush;
        }

        private void AssignShadow(Shadow value)
        {
            value.height = this.height;
            value.width = this.width;
            value.bVisible = this.bVisible;
            value.bBrush = this.bBrush.Clone() as ChartBrush;
        }

        public object Clone()
        {
            Shadow shadow = new Shadow(base.Chart);
            this.AssignShadow(shadow);
            return shadow;
        }

        public void Draw(Graphics3D g, Rectangle rect)
        {
            this.Draw(g, rect, 0, 0);
        }

        public void Draw(Graphics3D g, Rectangle rect, int angle, int aZ)
        {
            if ((this.height != 0) || (this.width != 0))
            {
                bool bVisible = g.Pen.bVisible;
                g.Pen.bVisible = false;
                g.Brush = this.Brush;
                if (this.Smooth)
                {
                    this.DrawSmooth(g, rect, false, false, 0, null);
                }
                else
                {
                    Rectangle r = Rectangle.FromLTRB(rect.Right, rect.Top + this.height, rect.Right + this.width, rect.Bottom + this.height);
                    Rectangle rectangle2 = Rectangle.FromLTRB(rect.Left + this.width, rect.Bottom, rect.Right + 2, rect.Bottom + this.height);
                    if (angle > 0)
                    {
                        g.Polygon(aZ, Graphics3D.RotateRectangle(rectangle2, angle));
                        g.Polygon(aZ, Graphics3D.RotateRectangle(r, angle));
                    }
                    else
                    {
                        g.Rectangle(rectangle2);
                        g.Rectangle(r);
                    }
                }
                g.Pen.bVisible = bVisible;
            }
        }

        public void Draw(Graphics3D g, int aWidth, int aHeight, params PointDouble[] points)
        {
            if ((aWidth != 0) || (aHeight != 0))
            {
                bool bVisible = g.Pen.bVisible;
                g.Pen.bVisible = false;
                g.Brush = this.Brush;
                PointDouble[] p = new PointDouble[points.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    p[i] = new PointDouble(points[i].X + aWidth, points[i].Y + aHeight);
                }
                if (this.Smooth)
                {
                    Rectangle rect = g.RectFromPolygon(p.Length, p);
                    for (int j = 0; j < p.Length; j++)
                    {
                        p[j].X -= rect.Left;
                        p[j].Y -= rect.Top;
                    }
                    this.DrawSmooth(g, rect, false, true, 0, p);
                }
                else
                {
                    g.Polygon(p);
                }
                g.Pen.bVisible = bVisible;
            }
        }

        public void Draw(Graphics3D g, int aWidth, int aHeight, params Point[] points)
        {
            this.Draw(g, aWidth, aHeight, PointDouble.FromPoint(points));
        }

        public void Draw(Graphics3D g, int x1, int y1, int x2, int y2)
        {
            this.Draw(g, x1, y1, x2, y2, 0);
        }

        public void Draw(Graphics3D g, int x1, int y1, int x2, int y2, int z)
        {
            if ((this.height != 0) || (this.width != 0))
            {
                bool bVisible = g.Pen.bVisible;
                g.Pen.bVisible = false;
                g.Brush = this.Brush;
                Rectangle rectBounds = Rectangle.FromLTRB(x1, y1, x2, y2);
                PointDouble[] p = new PointDouble[360];
                rectBounds.Offset(this.Width, this.Height);
                for (int i = 0; i < 360; i++)
                {
                    p[i] = g.PointFromEllipse(rectBounds, (double) i, z);
                }
                if (this.Smooth)
                {
                    for (int j = 0; j < 360; j++)
                    {
                        p[j].X -= rectBounds.Left;
                        p[j].Y -= rectBounds.Top;
                    }
                    this.DrawSmooth(g, rectBounds, false, true, 0, p);
                }
                else
                {
                    g.Polygon(p);
                }
                g.Pen.bVisible = bVisible;
            }
        }

        private void DrawSmooth(Graphics3D g, Rectangle rect, bool ellipse, bool polygon, int roundSize, PointDouble[] P)
        {
            int left = Math.Abs(this.Width);
            int top = Math.Abs(this.Height);
            int width = Math.Max(1, ((1 + rect.Right) - rect.Left) + (2 * left));
            int height = Math.Max(1, ((1 + rect.Bottom) - rect.Top) + (2 * top));
            Bitmap image = new Bitmap(width, height);
            Rectangle rectangle = Rectangle.FromLTRB(left, top, image.Width - left, image.Height - top);
            Graphics graphics = Graphics.FromImage(image);
            SolidBrush brush = new SolidBrush(this.Color);
            if (polygon)
            {
                graphics.FillPolygon(brush, PointDouble.RoundF(P));
            }
            else if (ellipse)
            {
                graphics.FillEllipse(brush, rectangle);
            }
            else if (roundSize == 0)
            {
                graphics.FillRectangle(brush, rectangle);
            }
            else
            {
                PointF[] points = PointDouble.RoundF(g.GetClipRoundRectangle(rectangle, roundSize, 0));
                graphics.FillPolygon(brush, points);
            }
            BufferedGraphics backBuffer = base.Chart.Graphics3D.BackBuffer;
            if (backBuffer != null)
            {
                Bitmap bitmap2 = new Bitmap(base.Chart.Width, base.Chart.Height);
                Graphics target = Graphics.FromImage(bitmap2);
                backBuffer.Render(target);
                Graphics3D.ShadowSmooth(image, bitmap2, rect.Left, rect.Top, width, height, left, top, 0.01 * (this.SmoothBlur + 180), ((ellipse || polygon) || this.fullDraw) || (roundSize != 0), g, false);
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.bBrush != null)
            {
                this.bBrush.Chart = base.chart;
            }
        }

        protected virtual bool ShouldSerializeHeight()
        {
            return ((this.height != this.defaultSize) && (this.height > 0));
        }

        protected virtual bool ShouldSerializeVisible()
        {
            return (this.bVisible != this.defaultVisible);
        }

        protected virtual bool ShouldSerializeWidth()
        {
            return ((this.width != this.defaultSize) && (this.width > 0));
        }

        [Description("Defines properties to fill shadow."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush Brush
        {
            get
            {
                return this.bBrush;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Gets or sets Color used to fill shadow.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.bBrush.Color;
            }
            set
            {
                this.bBrush.Color = value;
            }
        }

        [Description("The vertical shadow size.")]
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                base.SetIntegerProperty(ref this.height, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("System.Obsolete. Please use Width property"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public int HorizSize
        {
            get
            {
                return this.Width;
            }
            set
            {
                this.Width = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Size in pixels of shadow.")]
        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(this.width, this.height);
            }
            set
            {
                this.width = value.Width;
                this.height = value.Height;
                this.Invalidate();
            }
        }

        [DefaultValue(false), Description("Gets and sets if the shadow will be displayed as a rectangle with smooth edges.")]
        public bool Smooth
        {
            get
            {
                return this.smooth;
            }
            set
            {
                if (value != this.smooth)
                {
                    this.smooth = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("Gets and sets if the shadow will be displayed as a rectangle with smooth edges.")]
        public int SmoothBlur
        {
            get
            {
                return this.smoothBlur;
            }
            set
            {
                if (value != this.smoothBlur)
                {
                    this.smoothBlur = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Sets Transparency level from 0 to 100%."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Transparency
        {
            get
            {
                return this.bBrush.Transparency;
            }
            set
            {
                this.bBrush.Transparency = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), Obsolete("System.Obsolete. Please use Height property"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VertSize
        {
            get
            {
                return this.Height;
            }
            set
            {
                this.Height = value;
            }
        }

        [Description("Shows or hides shadow.")]
        public bool Visible
        {
            get
            {
                return this.bVisible;
            }
            set
            {
                base.SetBooleanProperty(ref this.bVisible, value);
            }
        }

        [Description("The horizontal shadow size.")]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                base.SetIntegerProperty(ref this.width, value);
            }
        }

        internal class ShadowComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Shadow s = (Shadow) value;
                bool flag = EditorUtils.ShowFormModal(new ShadowEditor(s, null));
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

