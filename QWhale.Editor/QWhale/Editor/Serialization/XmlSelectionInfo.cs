namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class XmlSelectionInfo : ISerializationInfo
    {
        private QWhale.Editor.AllowedSelectionMode allowedSelectionMode;
        private string backColor;
        private string borderColor;
        private string foreColor;
        private string inActiveBackColor;
        private string inActiveForeColor;
        private SelectionOptions options;
        private ISelection owner;
        private Rectangle selectionRect;
        private QWhale.Editor.SelectionType selectionType;

        public XmlSelectionInfo()
        {
            this.options = EditConsts.DefaultSelectionOptions;
            this.foreColor = XmlColorInfo.SerializeColor(EditConsts.DefaultHighlightForeColor);
            this.backColor = XmlColorInfo.SerializeColor(EditConsts.DefaultHighlightBackColor);
            this.borderColor = XmlColorInfo.SerializeColor(EditConsts.DefaultSelectionBorderColor);
            this.inActiveForeColor = XmlColorInfo.SerializeColor(EditConsts.DefaultInactiveHighlightForeColor);
            this.inActiveBackColor = XmlColorInfo.SerializeColor(EditConsts.DefaultInactiveHighlightBackColor);
            this.allowedSelectionMode = EditConsts.DefaultSelectionMode;
        }

        public XmlSelectionInfo(ISelection owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ISelection) owner;
            this.SelectionRect = this.selectionRect;
            this.SelectionType = this.selectionType;
            this.Options = this.options;
            this.AllowedSelectionMode = this.allowedSelectionMode;
            this.ForeColor = this.foreColor;
            this.BackColor = this.backColor;
            this.BorderColor = this.borderColor;
            this.InActiveForeColor = this.inActiveForeColor;
            this.InActiveBackColor = this.inActiveBackColor;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.selectionRect = this.SelectionRect;
                this.selectionType = this.SelectionType;
                this.options = this.Options;
                this.allowedSelectionMode = this.AllowedSelectionMode;
                this.foreColor = this.ForeColor;
                this.backColor = this.BackColor;
                this.borderColor = this.BorderColor;
                this.inActiveForeColor = this.InActiveForeColor;
                this.inActiveBackColor = this.InActiveBackColor;
            }
        }

        public bool ShouldSerializeAllowedSelectionMode()
        {
            return (this.AllowedSelectionMode != EditConsts.DefaultSelectionMode);
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.BackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultHighlightBackColor));
        }

        public bool ShouldSerializeBorderColor()
        {
            return (this.BorderColor != XmlColorInfo.SerializeColor(EditConsts.DefaultSelectionBorderColor));
        }

        public bool ShouldSerializeForeColor()
        {
            return (this.ForeColor != XmlColorInfo.SerializeColor(EditConsts.DefaultHighlightForeColor));
        }

        public bool ShouldSerializeInActiveBackColor()
        {
            return (this.InActiveBackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultInactiveHighlightBackColor));
        }

        public bool ShouldSerializeInActiveForeColor()
        {
            return (this.InActiveForeColor != XmlColorInfo.SerializeColor(EditConsts.DefaultInactiveHighlightForeColor));
        }

        public bool ShouldSerializeOptions()
        {
            return (this.Options != EditConsts.DefaultSelectionOptions);
        }

        public bool ShouldSerializeSelectionRect()
        {
            return !this.SelectionRect.Equals(Rectangle.Empty);
        }

        public QWhale.Editor.AllowedSelectionMode AllowedSelectionMode
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowedSelectionMode;
                }
                return this.owner.AllowedSelectionMode;
            }
            set
            {
                this.allowedSelectionMode = value;
                if (this.owner != null)
                {
                    this.owner.AllowedSelectionMode = value;
                }
            }
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

        public string BorderColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.borderColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.BorderColor);
            }
            set
            {
                this.borderColor = value;
                if (this.owner != null)
                {
                    this.owner.BorderColor = XmlColorInfo.DeserializeColor(value);
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

        public string InActiveBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.inActiveBackColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.InActiveBackColor);
            }
            set
            {
                this.inActiveBackColor = value;
                if (this.owner != null)
                {
                    this.owner.InActiveBackColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string InActiveForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.inActiveForeColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.InActiveForeColor);
            }
            set
            {
                this.inActiveForeColor = value;
                if (this.owner != null)
                {
                    this.owner.InActiveForeColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public SelectionOptions Options
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

        public Rectangle SelectionRect
        {
            get
            {
                if (this.owner == null)
                {
                    return this.selectionRect;
                }
                return this.owner.SelectionRect;
            }
            set
            {
                this.selectionRect = value;
                if (this.owner != null)
                {
                    this.owner.SelectionRect = value;
                }
            }
        }

        [DefaultValue(0)]
        public QWhale.Editor.SelectionType SelectionType
        {
            get
            {
                if (this.owner == null)
                {
                    return this.selectionType;
                }
                return this.owner.SelectionType;
            }
            set
            {
                this.selectionType = value;
                if (this.owner != null)
                {
                    this.owner.SelectionType = value;
                }
            }
        }
    }
}

