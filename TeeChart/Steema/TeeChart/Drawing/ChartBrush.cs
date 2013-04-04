namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    [Editor(typeof(ChartBrush.BrushComponentEditor), typeof(UITypeEditor)), Description("Brush (pattern) used to fill polygons and rectangles.")]
    public sealed class ChartBrush : TeeBase, ICloneable
    {
        internal System.Drawing.Color color;
        [NonSerialized]
        private Brush custom;
        internal System.Drawing.Color defaultColor;
        internal bool defaultVisible;
        private System.Drawing.Color foregroundColor;
        private Steema.TeeChart.Drawing.Gradient gradient;
        [NonSerialized]
        private Brush handle;
        private System.Drawing.Image image;
        private Steema.TeeChart.Drawing.ImageMode imageMode;
        private bool imageTransparent;
        private bool solid;
        private HatchStyle style;
        internal bool visible;
        private System.Drawing.Drawing2D.WrapMode wrapMode;

        public ChartBrush()
        {
            this.visible = true;
            this.defaultVisible = true;
            this.foregroundColor = System.Drawing.Color.Silver;
            this.color = System.Drawing.Color.White;
            this.defaultColor = System.Drawing.Color.White;
            this.style = HatchStyle.BackwardDiagonal;
            this.solid = true;
            this.imageMode = Steema.TeeChart.Drawing.ImageMode.Stretch;
        }

        public ChartBrush(Chart c) : this(c, System.Drawing.Color.White, true)
        {
        }

        public ChartBrush(Chart c, bool startVisible) : this(c, System.Drawing.Color.White, startVisible)
        {
        }

        public ChartBrush(Chart c, Brush brush) : base(c)
        {
            this.visible = true;
            this.defaultVisible = true;
            this.foregroundColor = System.Drawing.Color.Silver;
            this.color = System.Drawing.Color.White;
            this.defaultColor = System.Drawing.Color.White;
            this.style = HatchStyle.BackwardDiagonal;
            this.solid = true;
            this.imageMode = Steema.TeeChart.Drawing.ImageMode.Stretch;
            this.custom = brush;
        }

        public ChartBrush(Chart c, System.Drawing.Color aColor) : this(c, aColor, true)
        {
        }

        public ChartBrush(Chart c, System.Drawing.Color aColor, bool startVisible) : base(c)
        {
            this.visible = true;
            this.defaultVisible = true;
            this.foregroundColor = System.Drawing.Color.Silver;
            this.color = System.Drawing.Color.White;
            this.defaultColor = System.Drawing.Color.White;
            this.style = HatchStyle.BackwardDiagonal;
            this.solid = true;
            this.imageMode = Steema.TeeChart.Drawing.ImageMode.Stretch;
            this.color = aColor;
            this.defaultColor = this.color;
            this.visible = startVisible;
            this.defaultVisible = this.visible;
            this.handle = null;
            this.custom = null;
        }

        internal void ApplyDark(byte quantity)
        {
            Graphics3D.ApplyDark(ref this.color, quantity);
            this.SetNullHandle();
        }

        internal void ApplyDark(System.Drawing.Color c, byte quantity)
        {
            this.color = c;
            this.ApplyDark(quantity);
        }

        private void Assign(ChartBrush b)
        {
            b.foregroundColor = this.foregroundColor;
            b.color = this.color;
            b.visible = this.visible;
            b.style = this.style;
            b.image = this.image;
            b.imageTransparent = this.imageTransparent;
            b.solid = this.solid;
            b.wrapMode = this.wrapMode;
            b.imageMode = this.imageMode;
            if (this.gradient == null)
            {
                b.gradient = null;
            }
            else
            {
                b.gradient = this.gradient.Clone() as Steema.TeeChart.Drawing.Gradient;
            }
            b.SetNullHandle();
        }

        public void ClearImage()
        {
            if (this.image != null)
            {
                this.image.Dispose();
            }
        }

        public object Clone()
        {
            ChartBrush b = new ChartBrush(base.Chart);
            this.Assign(b);
            return b;
        }

        public override void Invalidate()
        {
            this.SetNullHandle();
            base.Invalidate();
        }

        public void LoadImage(string fileName)
        {
            this.ClearImage();
            this.image = System.Drawing.Image.FromFile(fileName);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.gradient != null)
            {
                this.gradient.Chart = base.chart;
            }
        }

        private void SetDrawingBrush()
        {
            if (this.image != null)
            {
                this.handle = new TextureBrush(this.image, this.wrapMode);
            }
            else if (this.solid)
            {
                this.handle = new SolidBrush(this.color);
            }
            else
            {
                this.handle = new HatchBrush(this.style, this.foregroundColor, this.color);
            }
        }

        private void SetNullHandle()
        {
            this.handle = null;
            this.custom = null;
            if ((base.chart != null) && (this == base.chart.graphics3D.Brush))
            {
                base.chart.graphics3D.Changed(base.chart.graphics3D.Brush);
            }
        }

        private bool ShouldSerializeColor()
        {
            return (this.color.ToArgb() != this.defaultColor.ToArgb());
        }

        private bool ShouldSerializeVisible()
        {
            return (this.visible != this.defaultVisible);
        }

        [Description("Determines the color used to fill a zone.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                base.SetColorProperty(ref this.color, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Access the internal Drawing.Brush")]
        public Brush DrawingBrush
        {
            get
            {
                if (this.custom == null)
                {
                    this.SetDrawingBrush();
                    return this.handle;
                }
                return this.custom;
            }
        }

        [Description("Color to fill inner portions of Brush, when Solid is false."), DefaultValue(typeof(System.Drawing.Color), "Silver")]
        public System.Drawing.Color ForegroundColor
        {
            get
            {
                return this.foregroundColor;
            }
            set
            {
                base.SetColorProperty(ref this.foregroundColor, value);
            }
        }

        [Description("Fill Gradient."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                if (this.gradient == null)
                {
                    this.gradient = new Steema.TeeChart.Drawing.Gradient(base.chart);
                }
                return this.gradient;
            }
            set
            {
                this.gradient = value;
                this.Invalidate();
            }
        }

        public bool GradientVisible
        {
            get
            {
                return ((this.gradient != null) && this.gradient.visible);
            }
        }

        [DefaultValue((string) null), Description("Image to use for fill.")]
        public System.Drawing.Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                this.solid = this.image == null;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Steema.TeeChart.Drawing.ImageMode), "Stretch"), Description("Style of drawing brush Image.")]
        public Steema.TeeChart.Drawing.ImageMode ImageMode
        {
            get
            {
                return this.imageMode;
            }
            set
            {
                if (this.imageMode != value)
                {
                    this.imageMode = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("Sets the Brush image to Transparent.")]
        public bool ImageTransparent
        {
            get
            {
                return this.imageTransparent;
            }
            set
            {
                base.SetBooleanProperty(ref this.imageTransparent, value);
            }
        }

        [DefaultValue(true), Description("Fills using Color only. Does not use Foreground color.")]
        public bool Solid
        {
            get
            {
                return this.solid;
            }
            set
            {
                base.SetBooleanProperty(ref this.solid, value);
            }
        }

        [Description("Determines the style in which the zone is filled or patterned using both Color and ForegroundColor."), DefaultValue(3)]
        public HatchStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.solid = false;
                    this.Invalidate();
                }
            }
        }

        [Description("Sets Transparency level from 0 to 100%."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0)]
        public int Transparency
        {
            get
            {
                return Graphics3D.Transparency(this.color);
            }
            set
            {
                this.Color = Graphics3D.TransparentColor(value, this.color);
                if (this.gradient != null)
                {
                    this.gradient.Transparency = value;
                }
            }
        }

        [Description("Empty of Brush when False.")]
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

        [Description("Style of drawing brush Image."), DefaultValue(typeof(System.Drawing.Drawing2D.WrapMode), "Tile")]
        public System.Drawing.Drawing2D.WrapMode WrapMode
        {
            get
            {
                return this.wrapMode;
            }
            set
            {
                if (this.wrapMode != value)
                {
                    this.wrapMode = value;
                    this.Invalidate();
                }
            }
        }

        internal class BrushComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                using (BrushEditor editor = new BrushEditor((ChartBrush) value))
                {
                    bool flag = editor.ShowDialog() == DialogResult.OK;
                    if ((context != null) && flag)
                    {
                        context.OnComponentChanged();
                    }
                    return flag;
                }
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override void PaintValue(PaintValueEventArgs e)
            {
                base.PaintValue(e);
                e.Graphics.FillRectangle(((ChartBrush) e.Value).DrawingBrush, e.Bounds);
            }
        }
    }
}

