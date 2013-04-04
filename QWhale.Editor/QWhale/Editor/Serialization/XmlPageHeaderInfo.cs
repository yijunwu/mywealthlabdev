namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;

    public class XmlPageHeaderInfo : ISerializationInfo
    {
        private string centerText;
        private System.Drawing.Font font;
        private string fontColor;
        private string fontName;
        private float fontSize;
        private System.Drawing.FontStyle fontStyle;
        private string leftText;
        private Point offset;
        private IEditPageHeader owner;
        private bool reverseOnEvenPages;
        private string rightText;
        private bool visible;

        public XmlPageHeaderInfo()
        {
            this.leftText = string.Empty;
            this.rightText = string.Empty;
            this.centerText = string.Empty;
            this.visible = true;
            this.font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f, EditConsts.DefaultHeaderFontStyle);
            this.fontName = FontFamily.GenericMonospace.Name;
            this.fontSize = 10f;
            this.fontStyle = EditConsts.DefaultHeaderFontStyle;
            this.fontColor = XmlColorInfo.SerializeColor(EditConsts.DefaultHeaderFontColor);
            this.fontName = FontFamily.GenericMonospace.Name;
            this.fontSize = 10f;
            this.fontStyle = System.Drawing.FontStyle.Regular;
        }

        public XmlPageHeaderInfo(IEditPageHeader owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IEditPageHeader) owner;
            this.Font = new System.Drawing.Font(this.fontName, this.fontSize, this.fontStyle);
            this.FontColor = this.fontColor;
            this.LeftText = this.leftText;
            this.CenterText = this.centerText;
            this.Offset = this.offset;
            this.ReverseOnEvenPages = this.reverseOnEvenPages;
            this.RightText = this.rightText;
            this.Visible = this.visible;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.fontName = this.Font.Name;
                this.fontSize = this.Font.Size;
                this.fontStyle = this.Font.Style;
                this.fontColor = this.FontColor;
                this.leftText = this.LeftText;
                this.centerText = this.CenterText;
                this.offset = this.Offset;
                this.reverseOnEvenPages = this.ReverseOnEvenPages;
                this.rightText = this.RightText;
                this.visible = this.Visible;
            }
        }

        public bool ShouldSerializeFontColor()
        {
            return (this.FontColor != XmlColorInfo.SerializeColor(EditConsts.DefaultHeaderFontColor));
        }

        public bool ShouldSerializeFontName()
        {
            return (this.FontName != FontFamily.GenericMonospace.Name);
        }

        public bool ShouldSerializeFontSize()
        {
            return (this.FontSize != 10.0);
        }

        public bool ShouldSerializeFontStyle()
        {
            return (this.FontStyle != EditConsts.DefaultHeaderFontStyle);
        }

        public bool ShouldSerializeOffset()
        {
            Point offset = this.Offset;
            if (offset.X == 0x18)
            {
                return (offset.Y != 8);
            }
            return true;
        }

        [DefaultValue("")]
        public string CenterText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.centerText;
                }
                return this.owner.CenterText;
            }
            set
            {
                this.centerText = value;
                if (this.owner != null)
                {
                    this.owner.CenterText = value;
                }
            }
        }

        [XmlIgnore]
        public System.Drawing.Font Font
        {
            get
            {
                if (this.owner == null)
                {
                    return this.font;
                }
                return this.owner.Font;
            }
            set
            {
                this.font = value;
                if (this.owner != null)
                {
                    this.owner.Font = value;
                }
            }
        }

        public string FontColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.FontColor);
            }
            set
            {
                this.fontColor = value;
                if (this.owner != null)
                {
                    this.owner.FontColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string FontName
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontName;
                }
                return this.owner.Font.Name;
            }
            set
            {
                this.fontName = value;
            }
        }

        public float FontSize
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontSize;
                }
                return this.owner.Font.Size;
            }
            set
            {
                this.fontSize = value;
            }
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontStyle;
                }
                return this.owner.Font.Style;
            }
            set
            {
                this.fontStyle = value;
            }
        }

        [DefaultValue("")]
        public string LeftText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.leftText;
                }
                return this.owner.LeftText;
            }
            set
            {
                this.leftText = value;
                if (this.owner != null)
                {
                    this.owner.LeftText = value;
                }
            }
        }

        public Point Offset
        {
            get
            {
                if (this.owner == null)
                {
                    return this.offset;
                }
                return this.owner.Offset;
            }
            set
            {
                this.offset = value;
                if (this.owner != null)
                {
                    this.owner.Offset = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool ReverseOnEvenPages
        {
            get
            {
                if (this.owner == null)
                {
                    return this.reverseOnEvenPages;
                }
                return this.owner.ReverseOnEvenPages;
            }
            set
            {
                this.reverseOnEvenPages = value;
                if (this.owner != null)
                {
                    this.owner.ReverseOnEvenPages = value;
                }
            }
        }

        [DefaultValue("")]
        public string RightText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rightText;
                }
                return this.owner.RightText;
            }
            set
            {
                this.rightText = value;
                if (this.owner != null)
                {
                    this.owner.RightText = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                if (this.owner == null)
                {
                    return this.visible;
                }
                return this.owner.Visible;
            }
            set
            {
                this.visible = value;
                if (this.owner != null)
                {
                    this.owner.Visible = value;
                }
            }
        }
    }
}

