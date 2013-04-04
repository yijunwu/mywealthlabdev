namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;

    [XmlRoot("Style")]
    public class XmlLexStyleInfo : ISerializationInfo
    {
        private string backColor;
        private bool backColorEnabled;
        private bool boldEnabled;
        private string desc;
        private System.Drawing.FontStyle fontStyle;
        private string foreColor;
        private bool foreColorEnabled;
        private bool italicEnabled;
        private string name;
        private ILexStyle owner;
        private bool plainText;
        private bool underlineEnabled;

        public XmlLexStyleInfo()
        {
            this.name = string.Empty;
            this.desc = string.Empty;
            this.foreColor = string.Empty;
            this.backColor = string.Empty;
            this.foreColorEnabled = true;
            this.backColorEnabled = true;
            this.boldEnabled = true;
            this.italicEnabled = true;
            this.underlineEnabled = true;
        }

        public XmlLexStyleInfo(ILexStyle owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ILexStyle) owner;
            this.Name = this.name;
            this.Desc = this.desc;
            this.ForeColor = this.foreColor;
            this.BackColor = this.backColor;
            this.FontStyle = this.fontStyle;
            this.ForeColorEnabled = this.foreColorEnabled;
            this.BackColorEnabled = this.backColorEnabled;
            this.BoldEnabled = this.boldEnabled;
            this.ItalicEnabled = this.italicEnabled;
            this.UnderlineEnabled = this.underlineEnabled;
            this.PlainText = this.plainText;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.desc = this.Desc;
                this.foreColor = this.ForeColor;
                this.backColor = this.BackColor;
                this.fontStyle = this.FontStyle;
                this.foreColorEnabled = this.ForeColorEnabled;
                this.backColorEnabled = this.BackColorEnabled;
                this.boldEnabled = this.BoldEnabled;
                this.italicEnabled = this.ItalicEnabled;
                this.underlineEnabled = this.UnderlineEnabled;
                this.plainText = this.PlainText;
            }
        }

        [DefaultValue("")]
        public string BackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.backColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.BackColor);
            }
            set
            {
                this.backColor = value;
                if (this.owner != null)
                {
                    this.owner.BackColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        [DefaultValue(true)]
        public bool BackColorEnabled
        {
            get
            {
                if (this.owner == null)
                {
                    return this.backColorEnabled;
                }
                return this.owner.BackColorEnabled;
            }
            set
            {
                this.backColorEnabled = value;
                if (this.owner != null)
                {
                    this.owner.BackColorEnabled = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool BoldEnabled
        {
            get
            {
                if (this.owner == null)
                {
                    return this.boldEnabled;
                }
                return this.owner.BoldEnabled;
            }
            set
            {
                this.boldEnabled = value;
                if (this.owner != null)
                {
                    this.owner.BoldEnabled = value;
                }
            }
        }

        [DefaultValue("")]
        public string Desc
        {
            get
            {
                if (this.owner == null)
                {
                    return this.desc;
                }
                return this.owner.Desc;
            }
            set
            {
                this.desc = value;
                if (this.owner != null)
                {
                    this.owner.Desc = value;
                }
            }
        }

        [DefaultValue(0)]
        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontStyle;
                }
                return this.owner.FontStyle;
            }
            set
            {
                this.fontStyle = value;
                if (this.owner != null)
                {
                    this.owner.FontStyle = value;
                }
            }
        }

        [DefaultValue("")]
        public string ForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.foreColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.ForeColor);
            }
            set
            {
                this.foreColor = value;
                if (this.owner != null)
                {
                    this.owner.ForeColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        [DefaultValue(true)]
        public bool ForeColorEnabled
        {
            get
            {
                if (this.owner == null)
                {
                    return this.foreColorEnabled;
                }
                return this.owner.ForeColorEnabled;
            }
            set
            {
                this.foreColorEnabled = value;
                if (this.owner != null)
                {
                    this.owner.ForeColorEnabled = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool ItalicEnabled
        {
            get
            {
                if (this.owner == null)
                {
                    return this.italicEnabled;
                }
                return this.owner.ItalicEnabled;
            }
            set
            {
                this.italicEnabled = value;
                if (this.owner != null)
                {
                    this.owner.ItalicEnabled = value;
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
        public bool PlainText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.plainText;
                }
                return this.owner.PlainText;
            }
            set
            {
                this.plainText = value;
                if (this.owner != null)
                {
                    this.owner.PlainText = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool UnderlineEnabled
        {
            get
            {
                if (this.owner == null)
                {
                    return this.underlineEnabled;
                }
                return this.owner.UnderlineEnabled;
            }
            set
            {
                this.underlineEnabled = value;
                if (this.owner != null)
                {
                    this.owner.UnderlineEnabled = value;
                }
            }
        }
    }
}

