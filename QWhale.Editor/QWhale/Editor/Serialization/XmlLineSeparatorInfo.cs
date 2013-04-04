namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class XmlLineSeparatorInfo : ISerializationInfo
    {
        private string contentDividerColor;
        private string highlightBackColor;
        private string highlightForeColor;
        private string lineColor;
        private SeparatorOptions options;
        private ILineSeparator owner;

        public XmlLineSeparatorInfo()
        {
            this.highlightBackColor = XmlColorInfo.SerializeColor(EditConsts.DefaultLineSeparatorColor);
            this.highlightForeColor = string.Empty;
            this.lineColor = XmlColorInfo.SerializeColor(EditConsts.DefaultLineSeparatorLineColor);
            this.contentDividerColor = XmlColorInfo.SerializeColor(EditConsts.DefaultContentDividerColor);
        }

        public XmlLineSeparatorInfo(ILineSeparator owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ILineSeparator) owner;
            this.Options = this.options;
            this.HighlightBackColor = this.highlightBackColor;
            this.HighlightForeColor = this.highlightForeColor;
            this.LineColor = this.lineColor;
            this.ContentDividerColor = this.contentDividerColor;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.options = this.Options;
                this.highlightBackColor = this.HighlightBackColor;
                this.highlightForeColor = this.HighlightForeColor;
                this.lineColor = this.LineColor;
                this.contentDividerColor = this.ContentDividerColor;
            }
        }

        public bool ShouldSerializeContentDividerColor()
        {
            return (this.ContentDividerColor != XmlColorInfo.SerializeColor(EditConsts.DefaultContentDividerColor));
        }

        public bool ShouldSerializeHighlightBackColor()
        {
            return (this.HighlightBackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultLineSeparatorColor));
        }

        public bool ShouldSerializeHighlightForeColor()
        {
            return (this.HighlightForeColor != XmlColorInfo.SerializeColor(Color.Empty));
        }

        public bool ShouldSerializeLineColor()
        {
            return (this.LineColor != XmlColorInfo.SerializeColor(EditConsts.DefaultLineSeparatorLineColor));
        }

        public string ContentDividerColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.contentDividerColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.ContentDividerColor);
            }
            set
            {
                this.contentDividerColor = value;
                if (this.owner != null)
                {
                    this.owner.ContentDividerColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string HighlightBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.highlightBackColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.HighlightBackColor);
            }
            set
            {
                this.highlightBackColor = value;
                if (this.owner != null)
                {
                    this.owner.HighlightBackColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string HighlightForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.highlightForeColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.HighlightForeColor);
            }
            set
            {
                this.highlightForeColor = value;
                if (this.owner != null)
                {
                    this.owner.HighlightForeColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string LineColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.LineColor);
            }
            set
            {
                this.lineColor = value;
                if (this.owner != null)
                {
                    this.owner.LineColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        [DefaultValue(0)]
        public SeparatorOptions Options
        {
            get
            {
                if (this.owner == null)
                {
                    return this.options;
                }
                return this.owner.Options;
            }
            set
            {
                this.options = value;
                if (this.owner != null)
                {
                    this.owner.Options = value;
                }
            }
        }
    }
}

