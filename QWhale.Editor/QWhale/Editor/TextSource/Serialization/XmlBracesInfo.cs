namespace QWhale.Editor.TextSource.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;

    public class XmlBracesInfo : ISerializationInfo
    {
        private string backColor;
        private QWhale.Editor.TextSource.BracesOptions bracesOptions;
        private char[] closingBraces;
        private System.Drawing.FontStyle fontStyle;
        private string foreColor;
        private char[] openBraces;
        private IEditBraceMatching owner;
        private bool useRoundRect;

        public XmlBracesInfo()
        {
            this.openBraces = EditConsts.DefaultOpenBraces;
            this.closingBraces = EditConsts.DefaultClosingBraces;
            this.foreColor = XmlColorInfo.SerializeColor(EditConsts.DefaultBracesForeColor);
            this.backColor = XmlColorInfo.SerializeColor(EditConsts.DefaultBracesBackColor);
            this.fontStyle = EditConsts.DefaultBracesFontStyle;
        }

        public XmlBracesInfo(IEditBraceMatching owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IEditBraceMatching) owner;
            this.OpenBraces = this.openBraces;
            this.ClosingBraces = this.closingBraces;
            this.BracesOptions = this.bracesOptions;
            this.ForeColor = this.foreColor;
            this.BackColor = this.backColor;
            this.FontStyle = this.fontStyle;
            this.UseRoundRect = this.useRoundRect;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.openBraces = this.OpenBraces;
                this.closingBraces = this.ClosingBraces;
                this.bracesOptions = this.BracesOptions;
                this.foreColor = this.ForeColor;
                this.backColor = this.BackColor;
                this.fontStyle = this.FontStyle;
                this.useRoundRect = this.UseRoundRect;
            }
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.BackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultBracesBackColor));
        }

        public bool ShouldSerializeClosingBraces()
        {
            return (new string(this.ClosingBraces) != new string(EditConsts.DefaultClosingBraces));
        }

        public bool ShouldSerializeFontStyle()
        {
            return (this.FontStyle != EditConsts.DefaultBracesFontStyle);
        }

        public bool ShouldSerializeForeColor()
        {
            return (this.ForeColor != XmlColorInfo.SerializeColor(EditConsts.DefaultBracesForeColor));
        }

        public bool ShouldSerializeOpenBraces()
        {
            return (new string(this.OpenBraces) != new string(EditConsts.DefaultOpenBraces));
        }

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

        [DefaultValue(0)]
        public QWhale.Editor.TextSource.BracesOptions BracesOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.bracesOptions;
                }
                return this.owner.BracesOptions;
            }
            set
            {
                this.bracesOptions = value;
                if (this.owner != null)
                {
                    this.owner.BracesOptions = value;
                }
            }
        }

        public char[] ClosingBraces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.closingBraces;
                }
                return this.owner.ClosingBraces;
            }
            set
            {
                this.closingBraces = value;
                if (this.owner != null)
                {
                    this.owner.ClosingBraces = value;
                }
            }
        }

        [XmlElement("Style")]
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

        public char[] OpenBraces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.openBraces;
                }
                return this.owner.OpenBraces;
            }
            set
            {
                this.openBraces = value;
                if (this.owner != null)
                {
                    this.owner.OpenBraces = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool UseRoundRect
        {
            get
            {
                if (this.owner == null)
                {
                    return this.useRoundRect;
                }
                return this.owner.UseRoundRect;
            }
            set
            {
                this.useRoundRect = value;
                if (this.owner != null)
                {
                    this.owner.UseRoundRect = value;
                }
            }
        }
    }
}

