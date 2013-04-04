namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class CodeSnippetHeader : ICodeSnippetHeader, ICodeCompletionProviderItem
    {
        private string author = string.Empty;
        private string description = string.Empty;
        private string shortcut = string.Empty;
        private string title = string.Empty;
        private CodeSnippetTypes types = new CodeSnippetTypes();

        public virtual void Assign(ICodeSnippetHeader source)
        {
            this.Title = source.Title;
            this.Description = source.Description;
            this.Author = source.Author;
            this.Shortcut = source.Shortcut;
            this.types.Clear();
            foreach (ICodeSnippetType type in source.Types)
            {
                this.types.AddSnippetType().SnippetType = type.SnippetType;
            }
        }

        protected virtual void OnAuthorChanged()
        {
        }

        protected virtual void OnDescriptionChanged()
        {
        }

        protected virtual void OnShortcutChanged()
        {
        }

        protected virtual void OnTitleChanged()
        {
        }

        public virtual string Author
        {
            get
            {
                return this.author;
            }
            set
            {
                if (this.author != value)
                {
                    this.author = value;
                    this.OnAuthorChanged();
                }
            }
        }

        public virtual string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.OnDescriptionChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetHeaderInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual string Shortcut
        {
            get
            {
                return this.shortcut;
            }
            set
            {
                if (this.shortcut != value)
                {
                    this.shortcut = value;
                    this.OnShortcutChanged();
                }
            }
        }

        public virtual string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.OnTitleChanged();
                }
            }
        }

        public virtual ICodeSnippetTypes Types
        {
            get
            {
                return this.types;
            }
        }
    }
}

