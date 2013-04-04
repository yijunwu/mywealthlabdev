namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;

    public class XmlQuickInfoItemInfo : ISerializationInfo
    {
        private IQuickInfoItem owner;
        private string text;

        public XmlQuickInfoItemInfo()
        {
            this.text = string.Empty;
        }

        public XmlQuickInfoItemInfo(IQuickInfoItem owner)
        {
            this.text = string.Empty;
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IQuickInfoItem) owner;
            this.Text = this.text;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.text = this.Text;
            }
        }

        [DefaultValue("")]
        public string Text
        {
            get
            {
                if (this.owner == null)
                {
                    return this.text;
                }
                return this.owner.Text;
            }
            set
            {
                this.text = value;
                if (this.owner != null)
                {
                    this.owner.Text = value;
                }
            }
        }
    }
}

