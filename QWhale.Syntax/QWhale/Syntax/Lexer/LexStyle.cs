namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class LexStyle : ILexStyle
    {
        private Color backColor;
        private bool backColorEnabled;
        private bool boldEnabled;
        private string desc;
        private System.Drawing.FontStyle fontStyle;
        private Color foreColor;
        private bool foreColorEnabled;
        private bool italicEnabled;
        private string name;
        private bool plainText;
        private ILexScheme scheme;
        private bool underlineEnabled;

        public LexStyle()
        {
            this.name = string.Empty;
            this.desc = string.Empty;
            this.foreColor = Color.Empty;
            this.backColor = Color.Empty;
            this.foreColorEnabled = true;
            this.backColorEnabled = true;
            this.boldEnabled = true;
            this.italicEnabled = true;
            this.underlineEnabled = true;
            this.foreColor = Consts.DefaultControlForeColor;
            this.backColor = Color.Empty;
        }

        public LexStyle(ILexScheme scheme) : this()
        {
            this.scheme = scheme;
        }

        public virtual void Assign(ILexStyle source)
        {
            this.name = source.Name;
            this.desc = source.Desc;
            this.foreColor = source.ForeColor;
            this.backColor = source.BackColor;
            this.fontStyle = source.FontStyle;
            this.plainText = source.PlainText;
            this.foreColorEnabled = source.ForeColorEnabled;
            this.backColorEnabled = source.BackColorEnabled;
            this.boldEnabled = source.BoldEnabled;
            this.italicEnabled = source.ItalicEnabled;
            this.underlineEnabled = source.UnderlineEnabled;
        }

        protected virtual void OnBackColorChanged()
        {
        }

        protected virtual void OnBackColorEnabledChanged()
        {
        }

        protected virtual void OnBoldEnabledChanged()
        {
        }

        protected virtual void OnDescChanged()
        {
        }

        protected virtual void OnFontStyleChanged()
        {
        }

        protected virtual void OnForeColorChanged()
        {
        }

        protected virtual void OnForeColorEnabledChanged()
        {
        }

        protected virtual void OnItalicEnabledChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnPlainTextChanged()
        {
        }

        protected virtual void OnUnderlineEnabledChanged()
        {
        }

        public virtual void ResetBackColor()
        {
            this.BackColor = Color.Empty;
        }

        public virtual void ResetFontStyle()
        {
            this.FontStyle = System.Drawing.FontStyle.Regular;
        }

        public virtual void ResetForeColor()
        {
            this.ForeColor = Consts.DefaultControlForeColor;
        }

        public virtual void ResetPlainText()
        {
            this.PlainText = false;
        }

        [Description("Gets or sets a background color of the \"LexStyle\".")]
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

        public virtual bool BackColorEnabled
        {
            get
            {
                return this.backColorEnabled;
            }
            set
            {
                if (this.backColorEnabled != value)
                {
                    this.backColorEnabled = value;
                    this.OnBackColorEnabledChanged();
                }
            }
        }

        public virtual bool BoldEnabled
        {
            get
            {
                return this.boldEnabled;
            }
            set
            {
                if (this.boldEnabled != value)
                {
                    this.boldEnabled = value;
                    this.OnBoldEnabledChanged();
                }
            }
        }

        [Description("Gets or sets a description of the \"LexStyle\".")]
        public virtual string Desc
        {
            get
            {
                return this.desc;
            }
            set
            {
                if (this.desc != value)
                {
                    this.desc = value;
                    this.OnDescChanged();
                }
            }
        }

        [Description("Gets or sets a font style of the \"LexStyle\".")]
        public virtual System.Drawing.FontStyle FontStyle
        {
            get
            {
                return this.fontStyle;
            }
            set
            {
                if (this.fontStyle != value)
                {
                    this.fontStyle = value;
                    this.OnFontStyleChanged();
                }
            }
        }

        [Description("Gets or sets a foreground color of the \"LexStyle\".")]
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

        public virtual bool ForeColorEnabled
        {
            get
            {
                return this.foreColorEnabled;
            }
            set
            {
                if (this.foreColorEnabled != value)
                {
                    this.foreColorEnabled = value;
                    this.OnForeColorEnabledChanged();
                }
            }
        }

        [Description("Gets or sets index of this \"LexStyle\" within the lexical style collection.")]
        public virtual int Index
        {
            get
            {
                if (this.scheme == null)
                {
                    return -1;
                }
                return this.scheme.Styles.IndexOf(this);
            }
        }

        public virtual bool ItalicEnabled
        {
            get
            {
                return this.italicEnabled;
            }
            set
            {
                if (this.italicEnabled != value)
                {
                    this.italicEnabled = value;
                    this.OnItalicEnabledChanged();
                }
            }
        }

        [Description("Gets or sets name of the \"LexStyle\".")]
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

        [Description("Gets or sets value indicating whether Edit control should use text formatting rules, like checking spelling.")]
        public virtual bool PlainText
        {
            get
            {
                return this.plainText;
            }
            set
            {
                if (this.plainText != value)
                {
                    this.plainText = value;
                    this.OnPlainTextChanged();
                }
            }
        }

        [Description("Gets or sets \"ILexScheme\" that owns this \"LexStyle\".")]
        public ILexScheme Scheme
        {
            get
            {
                return this.scheme;
            }
            set
            {
                this.scheme = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlLexStyleInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual bool UnderlineEnabled
        {
            get
            {
                return this.underlineEnabled;
            }
            set
            {
                if (this.underlineEnabled != value)
                {
                    this.underlineEnabled = value;
                    this.OnUnderlineEnabledChanged();
                }
            }
        }
    }
}

