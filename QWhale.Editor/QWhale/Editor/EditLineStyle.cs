namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Design;
    using QWhale.Editor.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Serializable, TypeConverter(typeof(LineStyleConverter))]
    public class EditLineStyle : IEditLineStyle
    {
        private Color backColor;
        private Color foreColor;
        private int imageIndex;
        private string name;
        private LineStyleOptions options;
        private ISyntaxEdit owner;
        private Color penColor;
        private int updateCount;

        public EditLineStyle()
        {
            this.name = string.Empty;
            this.foreColor = EditConsts.DefaultLineStyleForeColor;
            this.backColor = EditConsts.DefaultLineStyleBackColor;
            this.penColor = Color.Empty;
            this.imageIndex = -1;
            this.options = EditConsts.DefaultLineStyleOptions;
        }

        public EditLineStyle(ISyntaxEdit owner)
        {
            this.name = string.Empty;
            this.foreColor = EditConsts.DefaultLineStyleForeColor;
            this.backColor = EditConsts.DefaultLineStyleBackColor;
            this.penColor = Color.Empty;
            this.imageIndex = -1;
            this.options = EditConsts.DefaultLineStyleOptions;
            this.owner = owner;
        }

        public EditLineStyle(ISyntaxEdit owner, string name, Color foreColor, Color backColor, Color penColor, int imageIndex, LineStyleOptions options)
        {
            this.name = string.Empty;
            this.foreColor = EditConsts.DefaultLineStyleForeColor;
            this.backColor = EditConsts.DefaultLineStyleBackColor;
            this.penColor = Color.Empty;
            this.imageIndex = -1;
            this.options = EditConsts.DefaultLineStyleOptions;
            this.owner = owner;
            this.name = name;
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.penColor = penColor;
            this.imageIndex = imageIndex;
            this.options = options;
        }

        public virtual void Assign(IEditLineStyle source)
        {
            this.BeginUpdate();
            try
            {
                this.Name = source.Name;
                this.ForeColor = source.ForeColor;
                this.BackColor = source.BackColor;
                this.PenColor = source.PenColor;
                this.ImageIndex = source.ImageIndex;
                this.Options = source.Options;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected void BeginUpdate()
        {
            this.updateCount++;
        }

        protected void EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
        }

        public virtual Color GetBackColor(Color color)
        {
            Color color2 = ((this.options & LineStyleOptions.InvertColors) != LineStyleOptions.None) ? this.foreColor : this.backColor;
            if (!(color2 != Color.Empty))
            {
                return color;
            }
            return color2;
        }

        public virtual Color GetForeColor(Color color)
        {
            Color color2 = ((this.options & LineStyleOptions.InvertColors) != LineStyleOptions.None) ? this.backColor : this.foreColor;
            if (!(color2 != Color.Empty))
            {
                return color;
            }
            return color2;
        }

        protected virtual void OnBackColorChanged()
        {
            this.Update();
        }

        protected virtual void OnForeColorChanged()
        {
            this.Update();
        }

        protected virtual void OnImageIndexChanged()
        {
            this.Update();
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnOptionsChanged()
        {
            this.Update();
        }

        protected virtual void OnPenColorChanged()
        {
            this.Update();
        }

        public virtual void ResetBackColor()
        {
            this.BackColor = EditConsts.DefaultLineStyleBackColor;
        }

        public virtual void ResetForeColor()
        {
            this.ForeColor = EditConsts.DefaultLineStyleForeColor;
        }

        public virtual void ResetImageIndex()
        {
            this.ImageIndex = -1;
        }

        public virtual void ResetOptions()
        {
            this.Options = LineStyleOptions.InvertColors;
        }

        public virtual void ResetPenColor()
        {
            this.PenColor = Color.Empty;
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.backColor != EditConsts.DefaultLineStyleBackColor);
        }

        public bool ShouldSerializeForeColor()
        {
            return (this.foreColor != EditConsts.DefaultLineStyleForeColor);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.options != EditConsts.DefaultLineStyleOptions);
        }

        public bool ShouldSerializePenColor()
        {
            return (this.penColor != Color.Empty);
        }

        protected void Update()
        {
            if ((this.updateCount == 0) && (this.owner != null))
            {
                this.owner.Invalidate();
            }
        }

        [Description("Gets or sets background color of the \"EditLineStyle\".")]
        public virtual Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                if (this.backColor != value)
                {
                    this.backColor = value;
                    this.OnBackColorChanged();
                }
            }
        }

        [Description("Gets or sets foreground color of the \"EditLineStyle\".")]
        public virtual Color ForeColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                if (this.foreColor != value)
                {
                    this.foreColor = value;
                    this.OnForeColorChanged();
                }
            }
        }

        [DefaultValue(-1), Description("Gets or sets the index of the image displayed for the \"EditLineStyle\".")]
        public virtual int ImageIndex
        {
            get
            {
                return this.imageIndex;
            }
            set
            {
                if (this.imageIndex != value)
                {
                    this.imageIndex = value;
                    this.OnImageIndexChanged();
                }
            }
        }

        [DefaultValue(""), Description("Gets or sets name of the \"EditLineStyle\".")]
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnNameChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets display options for the \"EditLineStyle\".")]
        public virtual LineStyleOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.OnOptionsChanged();
                }
            }
        }

        [Description("Gets or sets pen color of the \"EditLineStyle\".")]
        public virtual Color PenColor
        {
            get
            {
                return this.penColor;
            }
            set
            {
                if (this.penColor != value)
                {
                    this.penColor = value;
                    this.OnPenColorChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlEditLineStyleInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

