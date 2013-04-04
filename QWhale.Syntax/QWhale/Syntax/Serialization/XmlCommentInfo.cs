namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Xml.Serialization;

    [XmlRoot("CommentInfoCollection")]
    public class XmlCommentInfo : ISerializationInfo
    {
        private ICommentInfo owner;
        private string text;

        public XmlCommentInfo()
        {
        }

        public XmlCommentInfo(ICommentInfo owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICommentInfo) owner;
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

