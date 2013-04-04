namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.Dialogs;
    using QWhale.Syntax.Lexer;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;

    public class XmlColorThemeInfo : ISerializationInfo
    {
        private System.Drawing.Font font;
        private string fontName;
        private float fontSize;
        private System.Drawing.FontStyle fontStyle;
        private XmlLexStyleInfo[] lexStyles;
        private string name;
        private IColorTheme owner;
        private bool readOnly;

        public XmlColorThemeInfo()
        {
            this.name = string.Empty;
            this.font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f, EditConsts.DefaultHeaderFontStyle);
            this.fontName = FontFamily.GenericMonospace.Name;
            this.fontSize = 10f;
            this.lexStyles = new XmlLexStyleInfo[0];
        }

        public XmlColorThemeInfo(IColorTheme owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IColorTheme) owner;
            this.Name = this.name;
            this.Readonly = this.readOnly;
            this.LexStyles = this.lexStyles;
            this.Font = new System.Drawing.Font(this.fontName, this.fontSize, this.fontStyle);
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.readOnly = this.Readonly;
                this.fontName = this.Font.Name;
                this.fontSize = this.Font.Size;
                this.fontStyle = this.Font.Style;
                this.lexStyles = this.LexStyles;
                foreach (XmlLexStyleInfo info in this.lexStyles)
                {
                    info.Load();
                }
            }
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
            return (this.FontStyle != System.Drawing.FontStyle.Regular);
        }

        public bool ShouldSerializeLexStyles()
        {
            return ((this.lexStyles.Length > 0) || ((this.owner != null) && (this.owner.LexStyles.Count > 0)));
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

        [XmlArray, XmlArrayItem("Style")]
        public XmlLexStyleInfo[] LexStyles
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lexStyles;
                }
                XmlLexStyleInfo[] infoArray = new XmlLexStyleInfo[this.owner.LexStyles.Count];
                for (int i = 0; i < this.owner.LexStyles.Count; i++)
                {
                    infoArray[i] = (XmlLexStyleInfo) this.owner.LexStyles[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.lexStyles = value;
                if ((this.owner != null) && (value != null))
                {
                    ILexStyles styles = new QWhale.Syntax.Lexer.LexStyles(null);
                    foreach (XmlLexStyleInfo info in value)
                    {
                        ILexStyle item = new LexStyle();
                        styles.Add(item);
                        item.SerializationInfo = info;
                    }
                    this.owner.LexStyles = styles;
                }
            }
        }

        [DefaultValue("")]
        public string Name
        {
            get
            {
                if (this.owner == null)
                {
                    return this.name;
                }
                return this.owner.Name;
            }
            set
            {
                this.name = value;
                if (this.owner != null)
                {
                    this.owner.Name = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool Readonly
        {
            get
            {
                if (this.owner == null)
                {
                    return this.readOnly;
                }
                return this.owner.Readonly;
            }
            set
            {
                this.readOnly = value;
                if (this.owner != null)
                {
                    this.owner.Readonly = value;
                }
            }
        }
    }
}

