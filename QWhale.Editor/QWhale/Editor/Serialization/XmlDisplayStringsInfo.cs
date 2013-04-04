namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.ComponentModel;

    public class XmlDisplayStringsInfo : ISerializationInfo
    {
        private IDisplayStrings owner;
        private bool wordWrap;
        private bool wrapAtMargin;

        public XmlDisplayStringsInfo()
        {
        }

        public XmlDisplayStringsInfo(IDisplayStrings owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IDisplayStrings) owner;
            this.WordWrap = this.wordWrap;
            this.WrapAtMargin = this.wrapAtMargin;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.wordWrap = this.WordWrap;
                this.wrapAtMargin = this.WrapAtMargin;
            }
        }

        [DefaultValue(false)]
        public bool WordWrap
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wordWrap;
                }
                return this.owner.WordWrap;
            }
            set
            {
                this.wordWrap = value;
                if (this.owner != null)
                {
                    this.owner.WordWrap = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool WrapAtMargin
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wrapAtMargin;
                }
                return this.owner.WrapAtMargin;
            }
            set
            {
                this.wrapAtMargin = value;
                if (this.owner != null)
                {
                    this.owner.WrapAtMargin = value;
                }
            }
        }
    }
}

