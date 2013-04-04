namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Xml.Serialization;

    [XmlRoot("QuickInfoCollection")]
    public class XmlQuickInfo : ISerializationInfo
    {
        private IQuickInfo owner;
        private string text;

        public XmlQuickInfo()
        {
        }

        public XmlQuickInfo(IQuickInfo owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IQuickInfo) owner;
            this.Text = this.text;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.text = this.Text;
            }
        }

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

