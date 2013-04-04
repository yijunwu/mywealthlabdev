namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class CodeSnippetDeclaration : ICodeSnippetDeclaration, ICodeCompletionProviderItem
    {
        private ICodeSnippetLiterals literals = new CodeSnippetLiterals();
        private ICodeSnippetObjects objects = new CodeSnippetObjects();

        protected virtual void OnSnippetLiteralsChanged()
        {
        }

        protected virtual void OnSnippetObjectsChanged()
        {
        }

        public virtual ICodeSnippetLiterals Literals
        {
            get
            {
                return this.literals;
            }
        }

        public virtual ICodeSnippetObjects Objects
        {
            get
            {
                return this.objects;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetDeclarationInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

