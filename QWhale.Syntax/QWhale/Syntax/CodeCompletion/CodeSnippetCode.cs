namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class CodeSnippetCode : ICodeSnippetCode, ICodeCompletionProviderItem
    {
        private string code = string.Empty;
        private string delimiter = SyntaxConsts.DefaultSnippetDelimiter;
        private string kind = string.Empty;
        private string language = string.Empty;

        public virtual void Assign(ICodeSnippetCode source)
        {
            this.Delimiter = source.Delimiter;
            this.Kind = source.Kind;
            this.Language = source.Language;
            this.Code = source.Code;
        }

        protected virtual void OnCodeChanged()
        {
        }

        protected virtual void OnDelimiterChanged()
        {
        }

        protected virtual void OnKindChanged()
        {
        }

        protected virtual void OnLanguageChanged()
        {
        }

        public virtual string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                if (this.code != value)
                {
                    this.code = value;
                    this.OnCodeChanged();
                }
            }
        }

        public virtual string Delimiter
        {
            get
            {
                return this.delimiter;
            }
            set
            {
                if (this.delimiter != value)
                {
                    this.delimiter = value;
                    this.OnDelimiterChanged();
                }
            }
        }

        public virtual string Kind
        {
            get
            {
                return this.kind;
            }
            set
            {
                if (this.kind != value)
                {
                    this.kind = value;
                    this.OnKindChanged();
                }
            }
        }

        public virtual string Language
        {
            get
            {
                return this.language;
            }
            set
            {
                if (this.language != value)
                {
                    this.language = value;
                    this.OnLanguageChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetCodeInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

