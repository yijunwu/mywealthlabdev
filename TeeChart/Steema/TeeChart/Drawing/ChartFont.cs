namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Description("Font properties used at several objects."), Editor(typeof(ChartFont.FontComponentEditor), typeof(UITypeEditor))]
    public sealed class ChartFont : TeeBase, ICloneable
    {
        private ChartBrush bBrush;
        private bool bold;
        [NonSerialized]
        private Font custom;
        internal bool defaultBold;
        [NonSerialized]
        private Font handle;
        private bool italic;
        private string name;
        private FontFamily privateFontFamily;
        internal Steema.TeeChart.Drawing.Shadow shadow;
        private float size;
        private bool strikeout;
        private bool underline;
        private GraphicsUnit unit;
        private bool usePrivateFont;

        public ChartFont()
        {
            this.name = Texts.DefaultFontName;
            this.size = 8f;
        }

        public ChartFont(Chart c) : base(c)
        {
            this.name = Texts.DefaultFontName;
            this.size = 8f;
            this.custom = null;
            this.handle = null;
        }

        public ChartFont(Chart c, Font font) : base(c)
        {
            this.name = Texts.DefaultFontName;
            this.size = 8f;
            this.custom = font;
        }

        private void Assign(ChartFont f)
        {
            if (this.bBrush == null)
            {
                f.bBrush = null;
            }
            else
            {
                f.bBrush = this.bBrush.Clone() as ChartBrush;
            }
            if (this.shadow == null)
            {
                f.shadow = null;
            }
            else
            {
                f.shadow = this.shadow.Clone() as Steema.TeeChart.Drawing.Shadow;
            }
            f.bold = this.bold;
            f.strikeout = this.strikeout;
            f.underline = this.underline;
            f.italic = this.italic;
            f.name = this.name;
            f.size = this.size;
            f.handle = null;
            f.Changed();
        }

        private void Changed()
        {
            if (base.chart != null)
            {
                base.chart.graphics3D.Changed(base.chart.graphics3D.Font);
            }
        }

        public object Clone()
        {
            ChartFont f = new ChartFont(base.Chart);
            this.Assign(f);
            return f;
        }

        public override void Invalidate()
        {
            this.SetNullHandle();
            base.Invalidate();
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.shadow != null)
            {
                this.shadow.chart = base.chart;
            }
            if (this.bBrush != null)
            {
                this.bBrush.chart = base.chart;
            }
        }

        private void SetDrawingFont()
        {
            FontStyle regular;
            if (!this.UsePrivateFont)
            {
                regular = FontStyle.Regular;
                if (this.bold)
                {
                    regular |= FontStyle.Bold;
                }
                if (this.italic)
                {
                    regular |= FontStyle.Italic;
                }
                if (this.underline)
                {
                    regular |= FontStyle.Underline;
                }
                if (this.strikeout)
                {
                    regular |= FontStyle.Strikeout;
                }
                this.handle = new Font(this.name, this.size, regular);
                return;
            }
            if (!Utils.IsPrivateFont(this.name, ref this.privateFontFamily))
            {
                return;
            }
            string name = this.privateFontFamily.Name;
            if (name != null)
            {
                if (!(name == "DS-Digital"))
                {
                    if (name == "Elektra")
                    {
                        regular = FontStyle.Bold;
                        if (this.italic)
                        {
                            regular |= FontStyle.Italic;
                        }
                        if (this.underline)
                        {
                            regular |= FontStyle.Underline;
                        }
                        if (this.strikeout)
                        {
                            regular |= FontStyle.Strikeout;
                        }
                        goto Label_0086;
                    }
                }
                else
                {
                    regular = FontStyle.Bold;
                    if (this.italic)
                    {
                        regular |= FontStyle.Italic;
                    }
                    goto Label_0086;
                }
            }
            regular = FontStyle.Regular;
        Label_0086:
            this.handle = new Font(this.privateFontFamily, this.size, regular);
        }

        private void SetNullHandle()
        {
            this.handle = null;
            this.custom = null;
            if ((base.chart != null) && (this == base.chart.graphics3D.Font))
            {
                base.chart.graphics3D.Changed(base.chart.graphics3D.Font);
            }
        }

        public bool ShouldDrawShadow()
        {
            if ((this.shadow == null) || !this.shadow.bVisible)
            {
                return false;
            }
            if (this.shadow.Width == 0)
            {
                return (this.shadow.Height != 0);
            }
            return true;
        }

        private bool ShouldSerializeBold()
        {
            return (this.bold != this.defaultBold);
        }

        [Description("Sets Font bold for text.")]
        public bool Bold
        {
            get
            {
                return this.bold;
            }
            set
            {
                base.SetBooleanProperty(ref this.bold, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets the Brush characteristics of the font"), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                if (this.bBrush == null)
                {
                    this.bBrush = new ChartBrush(base.chart, System.Drawing.Color.Black);
                }
                return this.bBrush;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Defines a Font colour for text.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.Brush.Color;
            }
            set
            {
                this.Brush.Color = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font DrawingFont
        {
            get
            {
                if (this.custom == null)
                {
                    this.SetDrawingFont();
                    return this.handle;
                }
                return this.custom;
            }
            set
            {
                this.handle = value;
            }
        }

        [Description("Applies a gradient fill to the font."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
        }

        [DefaultValue(false), Description("Sets Font italic (true or false) for text.")]
        public bool Italic
        {
            get
            {
                return this.italic;
            }
            set
            {
                base.SetBooleanProperty(ref this.italic, value);
            }
        }

        [DefaultValue("Verdana"), Description("Defines a Font type for text.")]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.UsePrivateFont = Utils.IsPrivateFont(value);
                base.SetStringProperty(ref this.name, value);
            }
        }

        [Description("Accesses the shadow properties of the font."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Shadow Shadow
        {
            get
            {
                if (this.shadow == null)
                {
                    this.shadow = new Steema.TeeChart.Drawing.Shadow(base.chart, 1);
                }
                return this.shadow;
            }
        }

        [DefaultValue(8), Description("Sets Font sizing (in points) for text.")]
        public int Size
        {
            get
            {
                return Convert.ToInt32(Math.Round((double) this.size));
            }
            set
            {
                base.SetFloatProperty(ref this.size, Convert.ToSingle(value));
            }
        }

        [DefaultValue((float) 8f), Description("Sets Font sizing as em-size float for text to enable use of Font.Unit.")]
        public float SizeFloat
        {
            get
            {
                return this.size;
            }
            set
            {
                base.SetFloatProperty(ref this.size, value);
            }
        }

        [Description("Sets Font Strikeout on/off."), DefaultValue(false)]
        public bool Strikeout
        {
            get
            {
                return this.strikeout;
            }
            set
            {
                base.SetBooleanProperty(ref this.strikeout, value);
            }
        }

        [DefaultValue(false), Description("Sets Font underline on/off.")]
        public bool Underline
        {
            get
            {
                return this.underline;
            }
            set
            {
                base.SetBooleanProperty(ref this.underline, value);
            }
        }

        [Description("Gets the unit of measure for this Font."), DefaultValue(typeof(GraphicsUnit), "World")]
        public GraphicsUnit Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                if (value != this.unit)
                {
                    this.unit = value;
                }
            }
        }

        [DefaultValue(false), Description("Sets or returns whether to include TeeChart's private fonts in the ChartFont class.")]
        public bool UsePrivateFont
        {
            get
            {
                return this.usePrivateFont;
            }
            set
            {
                base.SetBooleanProperty(ref this.usePrivateFont, value);
            }
        }

        internal class FontComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                ChartFont font = (ChartFont) value;
                bool flag = EditorUtils.EditFont(font);
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

