namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    public class XmlCodeSnippetCodeInfo : ISerializationInfo
    {
        private string code;
        private ICodeSnippetCode owner;

        public XmlCodeSnippetCodeInfo()
        {
            this.code = string.Empty;
        }

        public XmlCodeSnippetCodeInfo(ICodeSnippetCode owner)
        {
            this.code = string.Empty;
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippetCode) owner;
            this.Code = this.code;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.code = this.Code;
            }
        }

        [XmlText(typeof(string)), DefaultValue("")]
        public string Code
        {
            get
            {
                if (this.owner == null)
                {
                    return this.code;
                }
                return this.owner.Code;
            }
            set
            {
                this.code = value;
                if (this.owner != null)
                {
                    this.owner.Code = value;
                }
            }
        }
    }
}

