namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Serializable]
    public class Shape : TeeBase, ICloneable
    {
        protected Steema.TeeChart.Drawing.Bevel bBevel;
        protected int bBorderRound;
        protected internal ChartBrush bBrush;
        protected Steema.TeeChart.Drawing.ImageBevel bImageBevel;
        protected bool bTransparent;
        internal bool defaultTransparent;
        protected internal bool defaultVisible;
        protected int iBottom;
        protected int iHeight;
        protected int iLeft;
        protected int iRight;
        protected int iTop;
        protected int iWidth;
        protected internal ChartPen pPen;
        internal Steema.TeeChart.Drawing.Shadow shadow;
        internal bool visible;

        public Shape() : this(null)
        {
        }

        public Shape(Chart c) : base(c)
        {
            this.defaultVisible = true;
            this.visible = true;
        }

        [Description("Assign all properties from a shape to another."), Obsolete("Please use the Shape.Clone method")]
        public void Assign(Shape s)
        {
            if (s != null)
            {
                if (s.bImageBevel != null)
                {
                    this.ImageBevel.Assign(s.bImageBevel);
                }
                if (s.bBevel != null)
                {
                    this.Bevel.Assign(s.bBevel);
                }
                if (s.bBrush != null)
                {
                    this.Brush = s.bBrush;
                }
                this.Left = s.Left;
                this.Right = s.Right;
                this.Top = s.Top;
                this.Bottom = s.Bottom;
                if (s.pPen != null)
                {
                    this.Pen = s.pPen;
                }
                if (s.shadow != null)
                {
                    this.Shadow.Assign(s.shadow);
                }
                this.Visible = s.Visible;
                this.Transparent = s.Transparent;
            }
        }

        protected virtual void AssignShape(Shape s)
        {
            if (s != null)
            {
                if (this.bImageBevel != null)
                {
                    s.bImageBevel = this.bImageBevel.Clone() as Steema.TeeChart.Drawing.ImageBevel;
                }
                if (this.bBevel != null)
                {
                    s.bBevel = this.bBevel.Clone() as Steema.TeeChart.Drawing.Bevel;
                }
                if (this.bBrush != null)
                {
                    s.bBrush = this.bBrush.Clone() as ChartBrush;
                }
                if (this.pPen != null)
                {
                    s.pPen = this.pPen.Clone() as ChartPen;
                }
                if (this.shadow != null)
                {
                    s.shadow = this.shadow.Clone() as Steema.TeeChart.Drawing.Shadow;
                }
                if (this.Gradient != null)
                {
                    s.Gradient = this.Gradient.Clone() as Steema.TeeChart.Drawing.Gradient;
                }
                s.Left = this.Left;
                s.Right = this.Right;
                s.Top = this.Top;
                s.Bottom = this.Bottom;
                s.Visible = this.Visible;
                s.Transparent = this.Transparent;
            }
        }

        public object Clone()
        {
            Shape s = CreateNewShape(base.Chart, base.GetType());
            this.AssignShape(s);
            return s;
        }

        public static Shape CreateNewShape(Chart chart, Type type)
        {
            Shape shape = NewFromType(type);
            shape.Chart = chart;
            return shape;
        }

        public static Shape NewFromType(Type type)
        {
            return (Shape) Activator.CreateInstance(type);
        }

        public virtual void Paint(Graphics3D g, Rectangle rect)
        {
            if (!this.bTransparent)
            {
                g.Pen = this.Pen;
                g.Brush = this.Brush;
                Rectangle r = rect;
                if (this.BorderRound > 0)
                {
                    g.RoundRectangle(r, this.BorderRound);
                }
                else
                {
                    g.Rectangle(r);
                }
                if ((this.shadow != null) && this.shadow.Visible)
                {
                    r = rect;
                    if ((this.bImageBevel != null) && this.bImageBevel.Visible)
                    {
                        r.Inflate(this.bImageBevel.Width, this.bImageBevel.Width);
                    }
                    this.shadow.Draw(g, r);
                }
                if ((this.bImageBevel != null) && this.bImageBevel.Visible)
                {
                    r = rect;
                    r.Inflate(this.bImageBevel.Width, this.bImageBevel.Width);
                    this.bImageBevel.Draw(g, r, this.BorderRound);
                }
                if (((this.bBevel != null) && (this.bImageBevel != null)) && !this.bImageBevel.Visible)
                {
                    this.bBevel.Draw(g, rect);
                }
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pPen != null)
            {
                this.pPen.Chart = base.chart;
            }
            if (this.shadow != null)
            {
                this.shadow.Chart = base.chart;
            }
            if (this.bBrush != null)
            {
                this.bBrush.Chart = base.chart;
            }
            if (this.bBevel != null)
            {
                this.bBevel.Chart = base.chart;
            }
            if (this.bImageBevel != null)
            {
                this.bImageBevel.Chart = base.chart;
            }
        }

        protected virtual bool ShouldSerializeBottom()
        {
            return false;
        }

        protected virtual bool ShouldSerializeLeft()
        {
            return false;
        }

        protected virtual bool ShouldSerializeRight()
        {
            return false;
        }

        protected virtual bool ShouldSerializeTop()
        {
            return false;
        }

        protected virtual bool ShouldSerializeTransparent()
        {
            return (this.bTransparent != this.defaultTransparent);
        }

        protected virtual bool ShouldSerializeVisible()
        {
            return (this.visible != this.defaultVisible);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets the bevel characteristics of the Shape.")]
        public Steema.TeeChart.Drawing.Bevel Bevel
        {
            get
            {
                if (this.bBevel == null)
                {
                    this.bBevel = new Steema.TeeChart.Drawing.Bevel(base.chart);
                }
                return this.bBevel;
            }
        }

        [Obsolete("Please use Bevel.Inner property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(0)]
        public BevelStyles BevelInner
        {
            get
            {
                return this.Bevel.Inner;
            }
            set
            {
                this.Bevel.Inner = value;
            }
        }

        [DefaultValue(0), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Please use Bevel.Outer property."), Browsable(false)]
        public BevelStyles BevelOuter
        {
            get
            {
                return this.Bevel.Outer;
            }
            set
            {
                this.Bevel.Outer = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Obsolete("Please use Bevel.Width property."), DefaultValue(1)]
        public int BevelWidth
        {
            get
            {
                return this.Bevel.Width;
            }
            set
            {
                this.Bevel.Width = value;
            }
        }

        [DefaultValue(0), Description("Rounds the Border of the Chart Panel.")]
        public int BorderRound
        {
            get
            {
                return this.bBorderRound;
            }
            set
            {
                base.SetIntegerProperty(ref this.bBorderRound, value);
            }
        }

        [Browsable(false)]
        public int Bottom
        {
            get
            {
                return this.iBottom;
            }
            set
            {
                base.SetIntegerProperty(ref this.iBottom, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines the kind of brush used to fill shape background.")]
        public ChartBrush Brush
        {
            get
            {
                if (this.bBrush == null)
                {
                    this.bBrush = new ChartBrush(base.chart);
                }
                return this.bBrush;
            }
            set
            {
                this.bBrush = value;
            }
        }

        [Description("Defines the shape Color."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Color Color
        {
            get
            {
                return this.Brush.Color;
            }
            set
            {
                if (this.Transparency > 0)
                {
                    this.Brush.Color = Graphics3D.TransparentColor(this.Transparency, value);
                }
                else
                {
                    this.Brush.Color = value;
                }
                this.Brush.Solid = true;
            }
        }

        [Description("Calls the  Gradient characteristics for the shape."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
            set
            {
                this.Brush.Gradient = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("The Height of the shape.")]
        public int Height
        {
            get
            {
                return this.ShapeBounds.Height;
            }
            set
            {
                base.SetIntegerProperty(ref this.iHeight, value);
            }
        }

        [Editor(typeof(System.Drawing.Design.BitmapEditor), typeof(UITypeEditor)), DefaultValue((string) null)]
        public System.Drawing.Image Image
        {
            get
            {
                return this.Brush.Image;
            }
            set
            {
                this.Brush.Image = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets the image bevel characteristics of the Shape.")]
        public Steema.TeeChart.Drawing.ImageBevel ImageBevel
        {
            get
            {
                if (this.bImageBevel == null)
                {
                    this.bImageBevel = new Steema.TeeChart.Drawing.ImageBevel(base.chart);
                }
                return this.bImageBevel;
            }
        }

        [DefaultValue(1), Description("Gets or sets how Image will be displayed.")]
        public Steema.TeeChart.Drawing.ImageMode ImageMode
        {
            get
            {
                return this.Brush.ImageMode;
            }
            set
            {
                this.Brush.ImageMode = value;
            }
        }

        [DefaultValue(false), Description("Sets the shape image to transparent.")]
        public bool ImageTransparent
        {
            get
            {
                return this.Brush.ImageTransparent;
            }
            set
            {
                this.Brush.ImageTransparent = value;
            }
        }

        [Browsable(false)]
        public int Left
        {
            get
            {
                return this.iLeft;
            }
            set
            {
                base.SetIntegerProperty(ref this.iLeft, value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Specifies the pen used to draw the shape.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pPen == null)
                {
                    this.pPen = new ChartPen(base.chart, System.Drawing.Color.Black);
                }
                return this.pPen;
            }
            set
            {
                this.pPen = value;
            }
        }

        [Browsable(false)]
        public int Right
        {
            get
            {
                return this.iRight;
            }
            set
            {
                base.SetIntegerProperty(ref this.iRight, value);
            }
        }

        [Description("Defines the shape's Shadow characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Shadow Shadow
        {
            get
            {
                if (this.shadow == null)
                {
                    this.shadow = new Steema.TeeChart.Drawing.Shadow(base.chart);
                }
                return this.shadow;
            }
        }

        [Description("Defines the boundaries of the Shape."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ShapeBounds
        {
            get
            {
                Rectangle rectangle = Utils.FromLTRB(this.iLeft, this.iTop, this.iRight, this.iBottom);
                if (this.iWidth != 0)
                {
                    rectangle.Width = this.iWidth;
                }
                if (this.iHeight != 0)
                {
                    rectangle.Height = this.iHeight;
                }
                return rectangle;
            }
            set
            {
                this.iLeft = value.X;
                this.iTop = value.Y;
                this.iRight = value.Right;
                this.iBottom = value.Bottom;
                this.iWidth = value.Width;
                this.iHeight = value.Height;
            }
        }

        [Browsable(false)]
        public int Top
        {
            get
            {
                return this.iTop;
            }
            set
            {
                base.SetIntegerProperty(ref this.iTop, value);
            }
        }

        [Category("Appearance"), DefaultValue(0), Description("Sets Transparency level from 0 to 100%."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Transparency
        {
            get
            {
                return this.Brush.Transparency;
            }
            set
            {
                this.Brush.Transparency = value;
                this.Shadow.Transparency = value;
            }
        }

        [Description("Enables/disables transparency of shape.")]
        public bool Transparent
        {
            get
            {
                return this.bTransparent;
            }
            set
            {
                base.SetBooleanProperty(ref this.bTransparent, value);
            }
        }

        [Description("Shows or hides the Shape.")]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                base.SetBooleanProperty(ref this.visible, value);
            }
        }

        [Description("The Width of the shape."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Width
        {
            get
            {
                return this.ShapeBounds.Width;
            }
            set
            {
                base.SetIntegerProperty(ref this.iWidth, value);
            }
        }
    }
}

