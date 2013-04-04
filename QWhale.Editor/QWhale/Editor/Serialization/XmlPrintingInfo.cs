namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.ComponentModel;

    public class XmlPrintingInfo : ISerializationInfo
    {
        private PrintOptions allowedOptions;
        private XmlPageHeaderInfo footer;
        private XmlPageHeaderInfo header;
        private PrintOptions options;
        private IPrinting owner;
        private bool showPrintOptionsDialog;

        public XmlPrintingInfo()
        {
            this.options = EditConsts.DefaultPrintOptions;
            this.allowedOptions = EditConsts.DefaultPrintOptions;
        }

        public XmlPrintingInfo(IPrinting owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IPrinting) owner;
            this.Options = this.options;
            this.AllowedOptions = this.allowedOptions;
            this.Header = this.header;
            this.Footer = this.footer;
            this.ShowPrintOptionsDialog = this.showPrintOptionsDialog;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.options = this.Options;
                this.allowedOptions = this.AllowedOptions;
                this.header = this.Header;
                this.footer = this.Footer;
                this.showPrintOptionsDialog = this.ShowPrintOptionsDialog;
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

        public bool ShouldSerializeAllowedOptions()
        {
            return (this.AllowedOptions != EditConsts.DefaultPrintOptions);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.Options != EditConsts.DefaultPrintOptions);
        }

        public PrintOptions AllowedOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowedOptions;
                }
                return this.owner.AllowedOptions;
            }
            set
            {
                this.allowedOptions = value;
                if (this.owner != null)
                {
                    this.owner.AllowedOptions = value;
                }
            }
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

        public PrintOptions Options
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

        [DefaultValue(false)]
        public bool ShowPrintOptionsDialog
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showPrintOptionsDialog;
                }
                return this.owner.ShowPrintOptionsDialog;
            }
            set
            {
                this.showPrintOptionsDialog = value;
                if (this.owner != null)
                {
                    this.owner.ShowPrintOptionsDialog = value;
                }
            }
        }
    }
}

