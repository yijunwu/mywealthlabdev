namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;

    public class XmlCodeSnippetReferenceInfo : ISerializationInfo
    {
        private string assembly;
        private ICodeSnippetReference owner;
        private string url;

        public XmlCodeSnippetReferenceInfo()
        {
            this.assembly = string.Empty;
            this.url = string.Empty;
        }

        public XmlCodeSnippetReferenceInfo(ICodeSnippetReference owner)
        {
            this.assembly = string.Empty;
            this.url = string.Empty;
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippetReference) owner;
            this.Assembly = this.assembly;
            this.Url = this.url;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.assembly = this.Assembly;
                this.url = this.Url;
            }
        }

        [DefaultValue("")]
        public string Assembly
        {
            get
            {
                if (this.owner == null)
                {
                    return this.assembly;
                }
                return this.owner.Assembly;
            }
            set
            {
                this.assembly = value;
                if (this.owner != null)
                {
                    this.owner.Assembly = value;
                }
            }
        }

        [DefaultValue("")]
        public string Url
        {
            get
            {
                if (this.owner == null)
                {
                    return this.url;
                }
                return this.owner.Url;
            }
            set
            {
                this.url = value;
                if (this.owner != null)
                {
                    this.owner.Url = value;
                }
            }
        }
    }
}

