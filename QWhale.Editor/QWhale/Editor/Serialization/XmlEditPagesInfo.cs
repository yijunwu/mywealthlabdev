namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class XmlEditPagesInfo : ISerializationInfo
    {
        private bool applyRulerToAllPages;
        private string backColor;
        private string borderColor;
        private XmlEditPageInfo defaultPage;
        private bool displayWhiteSpace;
        private IEditPages owner;
        private QWhale.Editor.PageType pageType;
        private string rulerBackColor;
        private string rulerIndentBackColor;
        private QWhale.Editor.RulerOptions rulerOptions;
        private EditRulers rulers;
        private QWhale.Editor.RulerUnits rulerUnits;
        private bool transparent;

        public XmlEditPagesInfo()
        {
            this.backColor = XmlColorInfo.SerializeColor(EditConsts.DefaultPageBackColor);
            this.borderColor = XmlColorInfo.SerializeColor(EditConsts.DefaultPageBorderColor);
            this.rulerBackColor = XmlColorInfo.SerializeColor(EditConsts.DefaultRulerBackColor);
            this.rulerIndentBackColor = XmlColorInfo.SerializeColor(EditConsts.DefaultRulerIndentBackColor);
            this.displayWhiteSpace = true;
            this.rulerOptions = EditConsts.DefaultRulerOptions;
            this.rulerUnits = EditConsts.DefaultRulerUnits;
        }

        public XmlEditPagesInfo(IEditPages owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IEditPages) owner;
            this.BackColor = this.backColor;
            this.BorderColor = this.borderColor;
            this.RulerBackColor = this.rulerBackColor;
            this.RulerIndentBackColor = this.rulerIndentBackColor;
            this.DefaultPage = this.defaultPage;
            this.DisplayWhiteSpace = this.displayWhiteSpace;
            this.PageType = this.pageType;
            this.RulerOptions = this.rulerOptions;
            this.Rulers = this.rulers;
            this.RulerUnits = this.rulerUnits;
            this.Transparent = this.transparent;
            this.ApplyRulerToAllPages = this.applyRulerToAllPages;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.backColor = this.BackColor;
                this.borderColor = this.BorderColor;
                this.rulerBackColor = this.RulerBackColor;
                this.rulerIndentBackColor = this.RulerIndentBackColor;
                this.defaultPage = this.DefaultPage;
                this.displayWhiteSpace = this.DisplayWhiteSpace;
                this.pageType = this.PageType;
                this.rulerOptions = this.RulerOptions;
                this.rulers = this.Rulers;
                this.rulerUnits = this.RulerUnits;
                this.transparent = this.Transparent;
                this.applyRulerToAllPages = this.ApplyRulerToAllPages;
                if (this.defaultPage != null)
                {
                    this.defaultPage.Load();
                }
            }
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.BackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultPageBackColor));
        }

        public bool ShouldSerializeBorderColor()
        {
            return (this.BorderColor != XmlColorInfo.SerializeColor(EditConsts.DefaultPageBorderColor));
        }

        public bool ShouldSerializeRulerBackColor()
        {
            return (this.RulerBackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultRulerBackColor));
        }

        public bool ShouldSerializeRulerIndentBackColor()
        {
            return (this.RulerIndentBackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultRulerIndentBackColor));
        }

        public bool ShouldSerializeRulerOptions()
        {
            return (this.RulerOptions != EditConsts.DefaultRulerOptions);
        }

        [DefaultValue(false)]
        public bool ApplyRulerToAllPages
        {
            get
            {
                if (this.owner == null)
                {
                    return this.applyRulerToAllPages;
                }
                return this.owner.ApplyRulerToAllPages;
            }
            set
            {
                this.applyRulerToAllPages = value;
                if (this.owner != null)
                {
                    this.owner.ApplyRulerToAllPages = value;
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

        public XmlEditPageInfo DefaultPage
        {
            get
            {
                if (this.owner == null)
                {
                    return this.defaultPage;
                }
                return (XmlEditPageInfo) this.owner.DefaultPage.SerializationInfo;
            }
            set
            {
                this.defaultPage = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.DefaultPage.SerializationInfo = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool DisplayWhiteSpace
        {
            get
            {
                if (this.owner == null)
                {
                    return this.displayWhiteSpace;
                }
                return this.owner.DisplayWhiteSpace;
            }
            set
            {
                this.displayWhiteSpace = value;
                if (this.owner != null)
                {
                    this.owner.DisplayWhiteSpace = value;
                }
            }
        }

        [DefaultValue(0)]
        public QWhale.Editor.PageType PageType
        {
            get
            {
                if (this.owner == null)
                {
                    return this.pageType;
                }
                return this.owner.PageType;
            }
            set
            {
                this.pageType = value;
                if (this.owner != null)
                {
                    this.owner.PageType = value;
                }
            }
        }

        public string RulerBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rulerBackColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.RulerBackColor);
            }
            set
            {
                this.rulerBackColor = value;
                if (this.owner != null)
                {
                    this.owner.RulerBackColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string RulerIndentBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rulerIndentBackColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.RulerIndentBackColor);
            }
            set
            {
                this.rulerIndentBackColor = value;
                if (this.owner != null)
                {
                    this.owner.RulerIndentBackColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public QWhale.Editor.RulerOptions RulerOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rulerOptions;
                }
                return this.owner.RulerOptions;
            }
            set
            {
                this.rulerOptions = value;
                if (this.owner != null)
                {
                    this.owner.RulerOptions = value;
                }
            }
        }

        [DefaultValue(0)]
        public EditRulers Rulers
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rulers;
                }
                return this.owner.Rulers;
            }
            set
            {
                this.rulers = value;
                if (this.owner != null)
                {
                    this.owner.Rulers = value;
                }
            }
        }

        [DefaultValue(1)]
        public QWhale.Editor.RulerUnits RulerUnits
        {
            get
            {
                if (this.owner == null)
                {
                    return this.rulerUnits;
                }
                return this.owner.RulerUnits;
            }
            set
            {
                this.rulerUnits = value;
                if (this.owner != null)
                {
                    this.owner.RulerUnits = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool Transparent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.transparent;
                }
                return this.owner.Transparent;
            }
            set
            {
                this.transparent = value;
                if (this.owner != null)
                {
                    this.owner.Transparent = value;
                }
            }
        }
    }
}

