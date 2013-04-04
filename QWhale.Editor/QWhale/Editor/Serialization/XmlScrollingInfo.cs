namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class XmlScrollingInfo : ISerializationInfo
    {
        private int defaultHorzScrollSize;
        private XmlScrollingButtonInfo[] horzButtons;
        private ScrollingOptions options;
        private IScrolling owner;
        private RichTextBoxScrollBars scrollBars;
        private XmlScrollingButtonInfo[] vertButtons;
        private int windowOriginX;
        private int windowOriginY;

        public XmlScrollingInfo()
        {
            this.scrollBars = RichTextBoxScrollBars.Both;
            this.defaultHorzScrollSize = EditConsts.DefaultHorzScrollSize;
            this.horzButtons = new XmlScrollingButtonInfo[0];
            this.vertButtons = new XmlScrollingButtonInfo[0];
        }

        public XmlScrollingInfo(IScrolling owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IScrolling) owner;
            this.WindowOriginX = this.windowOriginX;
            this.WindowOriginY = this.windowOriginY;
            this.ScrollBars = this.scrollBars;
            this.DefaultHorzScrollSize = this.defaultHorzScrollSize;
            this.Options = this.options;
            this.HorzButtons = this.horzButtons;
            this.VertButtons = this.vertButtons;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.windowOriginX = this.WindowOriginX;
                this.windowOriginY = this.WindowOriginY;
                this.scrollBars = this.ScrollBars;
                this.defaultHorzScrollSize = this.DefaultHorzScrollSize;
                this.options = this.Options;
                this.horzButtons = this.HorzButtons;
                this.vertButtons = this.VertButtons;
                foreach (XmlScrollingButtonInfo info in this.horzButtons)
                {
                    info.Load();
                }
                foreach (XmlScrollingButtonInfo info2 in this.vertButtons)
                {
                    info2.Load();
                }
            }
        }

        public bool ShouldSerializeDefaultHorzScrollSize()
        {
            return (this.DefaultHorzScrollSize != EditConsts.DefaultHorzScrollSize);
        }

        public bool ShouldSerializeHorzButtons()
        {
            return ((this.horzButtons.Length > 0) || ((this.owner != null) && (this.owner.HorzButtons.Count > 0)));
        }

        public bool ShouldSerializeOptions()
        {
            return (this.Options != EditConsts.DefaultScrollingOptions);
        }

        public bool ShouldSerializeVertButtons()
        {
            return ((this.vertButtons.Length > 0) || ((this.owner != null) && (this.owner.VertButtons.Count > 0)));
        }

        public int DefaultHorzScrollSize
        {
            get
            {
                if (this.owner == null)
                {
                    return this.defaultHorzScrollSize;
                }
                return this.owner.DefaultHorzScrollSize;
            }
            set
            {
                this.defaultHorzScrollSize = value;
                if (this.owner != null)
                {
                    this.owner.DefaultHorzScrollSize = value;
                }
            }
        }

        [XmlArrayItem("ScrollingButton"), XmlArray]
        public XmlScrollingButtonInfo[] HorzButtons
        {
            get
            {
                if (this.owner == null)
                {
                    return this.horzButtons;
                }
                XmlScrollingButtonInfo[] infoArray = new XmlScrollingButtonInfo[this.owner.HorzButtons.Count];
                for (int i = 0; i < this.owner.HorzButtons.Count; i++)
                {
                    infoArray[i] = (XmlScrollingButtonInfo) this.owner.HorzButtons[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.horzButtons = value;
                if (this.owner != null)
                {
                    this.owner.HorzButtons.Clear();
                    foreach (XmlScrollingButtonInfo info in value)
                    {
                        IScrollingButton button = this.owner.HorzButtons[this.owner.HorzButtons.AddScrollingButton()];
                        button.SerializationInfo = info;
                    }
                }
            }
        }

        public ScrollingOptions Options
        {
            get
            {
                if (this.owner == null)
                {
                    return EditConsts.DefaultScrollingOptions;
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

        [DefaultValue(3)]
        public RichTextBoxScrollBars ScrollBars
        {
            get
            {
                if (this.owner == null)
                {
                    return this.scrollBars;
                }
                return this.owner.ScrollBars;
            }
            set
            {
                this.scrollBars = value;
                if (this.owner != null)
                {
                    this.owner.ScrollBars = value;
                }
            }
        }

        [XmlArrayItem("ScrollingButton"), XmlArray]
        public XmlScrollingButtonInfo[] VertButtons
        {
            get
            {
                if (this.owner == null)
                {
                    return this.vertButtons;
                }
                XmlScrollingButtonInfo[] infoArray = new XmlScrollingButtonInfo[this.owner.VertButtons.Count];
                for (int i = 0; i < this.owner.VertButtons.Count; i++)
                {
                    infoArray[i] = (XmlScrollingButtonInfo) this.owner.VertButtons[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.vertButtons = value;
                if (this.owner != null)
                {
                    this.owner.VertButtons.Clear();
                    foreach (XmlScrollingButtonInfo info in value)
                    {
                        IScrollingButton button = this.owner.VertButtons[this.owner.VertButtons.AddScrollingButton()];
                        button.SerializationInfo = info;
                    }
                }
            }
        }

        [DefaultValue(0)]
        public int WindowOriginX
        {
            get
            {
                if (this.owner == null)
                {
                    return this.windowOriginX;
                }
                return this.owner.WindowOriginX;
            }
            set
            {
                this.windowOriginX = value;
                if (this.owner != null)
                {
                    this.owner.WindowOriginX = value;
                }
            }
        }

        [DefaultValue(0)]
        public int WindowOriginY
        {
            get
            {
                if (this.owner == null)
                {
                    return this.windowOriginY;
                }
                return this.owner.WindowOriginY;
            }
            set
            {
                this.windowOriginY = value;
                if (this.owner != null)
                {
                    this.owner.WindowOriginY = value;
                }
            }
        }
    }
}

