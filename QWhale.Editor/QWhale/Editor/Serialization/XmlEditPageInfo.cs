namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;

    public class XmlEditPageInfo : ISerializationInfo
    {
        private XmlPageHeaderInfo footer;
        private XmlPageHeaderInfo header;
        private int horzOffset;
        private bool landscape;
        private System.Drawing.Printing.Margins margins;
        private IEditPage owner;
        private PaperKind pageKind;
        private Size pageSize;
        private bool paintNumber;
        private bool usePrinterSettings;
        private int vertOffset;

        public XmlEditPageInfo()
        {
            this.horzOffset = EditConsts.DefaultPageHorzOffset;
            this.vertOffset = EditConsts.DefaultPageVertOffset;
            this.paintNumber = true;
            this.usePrinterSettings = true;
        }

        public XmlEditPageInfo(IEditPage owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IEditPage) owner;
            this.Header = this.header;
            this.Footer = this.footer;
            this.HorzOffset = this.horzOffset;
            this.Landscape = this.landscape;
            this.Margins = this.margins;
            this.PageKind = this.pageKind;
            this.PageSize = this.pageSize;
            this.PaintNumber = this.paintNumber;
            this.VertOffset = this.vertOffset;
            this.UsePrinterSettings = this.usePrinterSettings;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.header = this.Header;
                this.footer = this.Footer;
                this.horzOffset = this.HorzOffset;
                this.landscape = this.Landscape;
                this.margins = this.Margins;
                this.pageKind = this.PageKind;
                this.pageSize = this.PageSize;
                this.paintNumber = this.PaintNumber;
                this.vertOffset = this.VertOffset;
                this.usePrinterSettings = this.UsePrinterSettings;
                if (this.header != null)
                {
                    this.header.Load();
                }
                if (this.footer != null)
                {
                    this.footer.Load();
                }
            }
        }

        public bool ShouldSerializeHorzOffset()
        {
            return (this.HorzOffset != EditConsts.DefaultPageHorzOffset);
        }

        public bool ShouldSerializeLandscape()
        {
            IEditPages pages = (this.owner != null) ? this.owner.Pages : null;
            if (pages != null)
            {
                return (this.Landscape != pages.DefaultLandscape);
            }
            return true;
        }

        public bool ShouldSerializeMargins()
        {
            IEditPages pages = (this.owner != null) ? this.owner.Pages : null;
            System.Drawing.Printing.Margins margins = this.Margins;
            if (((pages != null) && (margins.Left == pages.DefaultMargins.Left)) && ((margins.Right == pages.DefaultMargins.Right) && (margins.Top == pages.DefaultMargins.Top)))
            {
                return (margins.Bottom != pages.DefaultMargins.Bottom);
            }
            return true;
        }

        public bool ShouldSerializePageKind()
        {
            IEditPages pages = (this.owner != null) ? this.owner.Pages : null;
            if (pages != null)
            {
                return (this.pageKind != pages.DefaultPageKind);
            }
            return true;
        }

        public bool ShouldSerializePageSize()
        {
            IEditPages pages = (this.owner != null) ? this.owner.Pages : null;
            Size pageSize = this.PageSize;
            if (((pages != null) && this.UsePrinterSettings) && (pageSize.Width == pages.DefaultPageSize.Width))
            {
                return (pageSize.Height != pages.DefaultPageSize.Height);
            }
            return true;
        }

        public bool ShouldSerializeVertOffset()
        {
            return (this.VertOffset != EditConsts.DefaultPageVertOffset);
        }

        public XmlPageHeaderInfo Footer
        {
            get
            {
                if (this.owner == null)
                {
                    return this.footer;
                }
                return (XmlPageHeaderInfo) this.owner.Footer.SerializationInfo;
            }
            set
            {
                this.footer = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Footer.SerializationInfo = value;
                }
            }
        }

        public XmlPageHeaderInfo Header
        {
            get
            {
                if (this.owner == null)
                {
                    return this.header;
                }
                return (XmlPageHeaderInfo) this.owner.Header.SerializationInfo;
            }
            set
            {
                this.header = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Header.SerializationInfo = value;
                }
            }
        }

        public int HorzOffset
        {
            get
            {
                if (this.owner == null)
                {
                    return this.horzOffset;
                }
                return this.owner.HorzOffset;
            }
            set
            {
                this.horzOffset = value;
                if (this.owner != null)
                {
                    this.owner.HorzOffset = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool Landscape
        {
            get
            {
                if (this.owner == null)
                {
                    return this.landscape;
                }
                return this.owner.Landscape;
            }
            set
            {
                this.landscape = value;
                if (this.owner != null)
                {
                    this.owner.Landscape = value;
                }
            }
        }

        public System.Drawing.Printing.Margins Margins
        {
            get
            {
                if (this.owner == null)
                {
                    return this.margins;
                }
                return this.owner.Margins;
            }
            set
            {
                this.margins = value;
                if (this.owner != null)
                {
                    this.owner.Margins = value;
                }
            }
        }

        public PaperKind PageKind
        {
            get
            {
                if (this.owner == null)
                {
                    return this.pageKind;
                }
                return this.owner.PageKind;
            }
            set
            {
                this.pageKind = value;
                if (this.owner != null)
                {
                    this.owner.PageKind = value;
                }
            }
        }

        public Size PageSize
        {
            get
            {
                if (this.owner == null)
                {
                    return this.pageSize;
                }
                return this.owner.PageSize;
            }
            set
            {
                this.pageSize = value;
                if (this.owner != null)
                {
                    this.owner.PageSize = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool PaintNumber
        {
            get
            {
                if (this.owner == null)
                {
                    return this.paintNumber;
                }
                return this.owner.PaintNumber;
            }
            set
            {
                this.paintNumber = value;
                if (this.owner != null)
                {
                    this.owner.PaintNumber = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool UsePrinterSettings
        {
            get
            {
                if (this.owner == null)
                {
                    return this.usePrinterSettings;
                }
                return this.owner.UsePrinterSettings;
            }
            set
            {
                this.usePrinterSettings = value;
                if (this.owner != null)
                {
                    this.usePrinterSettings = value;
                }
            }
        }

        public int VertOffset
        {
            get
            {
                if (this.owner == null)
                {
                    return this.vertOffset;
                }
                return this.owner.VertOffset;
            }
            set
            {
                this.vertOffset = value;
                if (this.owner != null)
                {
                    this.owner.VertOffset = value;
                }
            }
        }
    }
}

