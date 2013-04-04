namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class XmlScrollingButtonInfo : ISerializationInfo
    {
        private bool _checked;
        private bool allowCheck;
        private EditBorderStyle borderStyle;
        private string description;
        private int groupIndex;
        private int imageIndex;
        private ImageList images;
        private string name;
        private IScrollingButton owner;
        private bool visible;

        public XmlScrollingButtonInfo()
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.imageIndex = -1;
            this.visible = true;
            this.groupIndex = -1;
        }

        public XmlScrollingButtonInfo(IScrollingButton owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IScrollingButton) owner;
            this.Name = this.name;
            this.Description = this.description;
            this.ImageIndex = this.imageIndex;
            this.Images = this.images;
            this.Visible = this.visible;
            this.BorderStyle = this.borderStyle;
            this.Checked = this._checked;
            this.AllowCheck = this.allowCheck;
            this.GroupIndex = this.groupIndex;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.description = this.Description;
                this.imageIndex = this.ImageIndex;
                this.images = this.Images;
                this.visible = this.Visible;
                this.borderStyle = this.BorderStyle;
                this._checked = this.Checked;
                this.allowCheck = this.AllowCheck;
                this.groupIndex = this.GroupIndex;
            }
        }

        [DefaultValue(false)]
        public bool AllowCheck
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowCheck;
                }
                return this.owner.AllowCheck;
            }
            set
            {
                this.allowCheck = value;
                if (this.owner != null)
                {
                    this.owner.AllowCheck = value;
                }
            }
        }

        [DefaultValue(0)]
        public EditBorderStyle BorderStyle
        {
            get
            {
                if (this.owner == null)
                {
                    return this.borderStyle;
                }
                return this.owner.BorderStyle;
            }
            set
            {
                this.borderStyle = value;
                if (this.owner != null)
                {
                    this.owner.BorderStyle = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool Checked
        {
            get
            {
                if (this.owner == null)
                {
                    return this._checked;
                }
                return this.owner.Checked;
            }
            set
            {
                this._checked = value;
                if (this.owner != null)
                {
                    this.owner.Checked = value;
                }
            }
        }

        [DefaultValue("")]
        public string Description
        {
            get
            {
                if (this.owner == null)
                {
                    return this.description;
                }
                return this.owner.Description;
            }
            set
            {
                this.description = value;
                if (this.owner != null)
                {
                    this.owner.Description = value;
                }
            }
        }

        [DefaultValue(-1)]
        public int GroupIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.groupIndex;
                }
                return this.owner.GroupIndex;
            }
            set
            {
                this.groupIndex = value;
                if (this.owner != null)
                {
                    this.owner.GroupIndex = value;
                }
            }
        }

        [DefaultValue(-1)]
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

        [XmlIgnore]
        public ImageList Images
        {
            get
            {
                if (this.owner == null)
                {
                    return this.images;
                }
                return this.owner.Images;
            }
            set
            {
                this.images = value;
                if (this.owner != null)
                {
                    this.owner.Images = value;
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

