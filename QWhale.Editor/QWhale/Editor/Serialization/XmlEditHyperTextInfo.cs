namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class XmlEditHyperTextInfo : ISerializationInfo
    {
        private IEditHyperText owner;
        private bool showHints;
        private string urlColor;
        private FontStyle urlStyle;

        public XmlEditHyperTextInfo()
        {
            this.urlStyle = EditConsts.DefaultUrlFontStyle;
            this.urlColor = XmlColorInfo.SerializeColor(EditConsts.DefaultUrlForeColor);
            this.showHints = true;
        }

        public XmlEditHyperTextInfo(IEditHyperText owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IEditHyperText) owner;
            this.UrlStyle = this.urlStyle;
            this.UrlColor = this.urlColor;
            this.ShowHints = this.showHints;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.urlStyle = this.UrlStyle;
                this.urlColor = this.UrlColor;
                this.showHints = this.ShowHints;
            }
        }

        public bool ShouldSerializeUrlColor()
        {
            return (this.UrlColor != XmlColorInfo.SerializeColor(EditConsts.DefaultUrlForeColor));
        }

        public bool ShouldSerializeUrlStyle()
        {
            return (this.UrlStyle != EditConsts.DefaultUrlFontStyle);
        }

        [DefaultValue(true)]
        public bool ShowHints
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showHints;
                }
                return this.owner.ShowHints;
            }
            set
            {
                this.showHints = value;
                if (this.owner != null)
                {
                    this.owner.ShowHints = value;
                }
            }
        }

        public string UrlColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.urlColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.UrlColor);
            }
            set
            {
                this.urlColor = value;
                if (this.owner != null)
                {
                    this.owner.UrlColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public FontStyle UrlStyle
        {
            get
            {
                if (this.owner == null)
                {
                    return this.urlStyle;
                }
                return this.owner.UrlStyle;
            }
            set
            {
                this.urlStyle = value;
                if (this.owner != null)
                {
                    this.owner.UrlStyle = value;
                }
            }
        }
    }
}

