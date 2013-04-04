namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;

    public class XmlCodeSnippetImportInfo : ISerializationInfo
    {
        private string nspace;
        private ICodeSnippetImport owner;

        public XmlCodeSnippetImportInfo()
        {
        }

        public XmlCodeSnippetImportInfo(ICodeSnippetImport owner)
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippetImport) owner;
            this.Namespace = this.nspace;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.nspace = this.Namespace;
            }
        }

        [DefaultValue("")]
        public string Namespace
        {
            get
            {
                if (this.owner == null)
                {
                    return this.nspace;
                }
                return this.owner.Namespace;
            }
            set
            {
                this.nspace = value;
                if (this.owner != null)
                {
                    this.owner.Namespace = value;
                }
            }
        }
    }
}

