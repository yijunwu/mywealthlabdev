namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class CodeSnippetImport : ICodeSnippetImport, ICodeCompletionProviderItem
    {
        private string nspace = string.Empty;

        protected virtual void OnNamespaceChanged()
        {
        }

        public virtual string Namespace
        {
            get
            {
                return this.nspace;
            }
            set
            {
                if (this.nspace != value)
                {
                    this.nspace = value;
                    this.OnNamespaceChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetImportInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

