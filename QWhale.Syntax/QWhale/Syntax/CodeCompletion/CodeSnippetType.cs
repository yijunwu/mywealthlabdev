namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class CodeSnippetType : ICodeSnippetType, ICodeCompletionProviderItem
    {
        private QWhale.Syntax.CodeCompletion.SnippetType snippetType;

        protected virtual void OnSnippetTypeChanged()
        {
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetTypeInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual QWhale.Syntax.CodeCompletion.SnippetType SnippetType
        {
            get
            {
                return this.snippetType;
            }
            set
            {
                if (this.snippetType != value)
                {
                    this.snippetType = value;
                    this.OnSnippetTypeChanged();
                }
            }
        }
    }
}

