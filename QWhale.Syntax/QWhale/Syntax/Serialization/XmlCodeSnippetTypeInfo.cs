namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;

    public class XmlCodeSnippetTypeInfo : ISerializationInfo
    {
        private ICodeSnippetType owner;
        private QWhale.Syntax.CodeCompletion.SnippetType snippetType;

        public XmlCodeSnippetTypeInfo()
        {
        }

        public XmlCodeSnippetTypeInfo(ICodeSnippetType owner)
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippetType) owner;
            this.SnippetType = this.snippetType;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.snippetType = this.SnippetType;
            }
        }

        public QWhale.Syntax.CodeCompletion.SnippetType SnippetType
        {
            get
            {
                if (this.owner == null)
                {
                    return this.snippetType;
                }
                return this.owner.SnippetType;
            }
            set
            {
                this.snippetType = value;
                if (this.owner != null)
                {
                    this.owner.SnippetType = value;
                }
            }
        }
    }
}

