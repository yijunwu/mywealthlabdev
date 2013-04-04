namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Drawing;

    public class XmlEditLineStyleInfo : ISerializationInfo
    {
        private string backColor;
        private string foreColor;
        private int imageIndex;
        private string name;
        private LineStyleOptions options;
        private IEditLineStyle owner;
        private string penColor;

        public XmlEditLineStyleInfo()
        {
            this.name = string.Empty;
            this.foreColor = XmlColorInfo.SerializeColor(EditConsts.DefaultLineStyleForeColor);
            this.backColor = XmlColorInfo.SerializeColor(EditConsts.DefaultLineStyleBackColor);
            this.penColor = string.Empty;
            this.imageIndex = -1;
            this.options = EditConsts.DefaultLineStyleOptions;
        }

        public XmlEditLineStyleInfo(IEditLineStyle owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IEditLineStyle) owner;
            this.Name = this.name;
            this.ForeColor = this.foreColor;
            this.BackColor = this.backColor;
            this.PenColor = this.penColor;
            this.ImageIndex = this.imageIndex;
            this.Options = this.options;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.foreColor = this.ForeColor;
                this.backColor = this.BackColor;
                this.penColor = this.PenColor;
                this.imageIndex = this.ImageIndex;
                this.options = this.Options;
            }
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.BackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultLineStyleBackColor));
        }

        public bool ShouldSerializeForeColor()
        {
            return (this.ForeColor != XmlColorInfo.SerializeColor(EditConsts.DefaultLineStyleForeColor));
        }

        public bool ShouldSerializeOptions()
        {
            return (this.Options != EditConsts.DefaultLineStyleOptions);
        }

        public bool ShouldSerializePenColor()
        {
            return (this.BackColor != XmlColorInfo.SerializeColor(Color.Empty));
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

        public int ImageIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.imageIndex;
                }
                return this.owner.ImageIndex;
            }
            set
            {
                this.imageIndex = value;
                if (this.owner != null)
                {
                    this.owner.ImageIndex = value;
                }
            }
        }

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

        public LineStyleOptions Options
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

        public string PenColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.penColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.PenColor);
            }
            set
            {
                this.penColor = value;
                if (this.owner != null)
                {
                    this.owner.PenColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }
    }
}

