namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class CodeSnippetReference : ICodeSnippetReference, ICodeCompletionProviderItem
    {
        private string assembly = string.Empty;
        private string url = string.Empty;

        protected virtual void OnAssemblyChanged()
        {
        }

        protected virtual void OnUrlChanged()
        {
        }

        public virtual string Assembly
        {
            get
            {
                return this.assembly;
            }
            set
            {
                if (this.assembly != value)
                {
                    this.assembly = value;
                    this.OnAssemblyChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetReferenceInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                if (this.url != value)
                {
                    this.url = value;
                    this.OnUrlChanged();
                }
            }
        }
    }
}

