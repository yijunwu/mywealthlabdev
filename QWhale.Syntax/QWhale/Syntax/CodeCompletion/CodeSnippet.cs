namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class CodeSnippet : ICodeSnippet, ICodeCompletionProviderItem
    {
        private ICodeSnippetCode code;
        private object customData;
        private ICodeSnippetDeclarations declarations;
        private ICodeSnippetHeader header;
        private int imageIndex;
        private ICodeSnippetImports imports;
        private ICodeSnippets parent;
        private ICodeSnippetReferences references;

        public CodeSnippet()
        {
            this.header = new CodeSnippetHeader();
            this.declarations = new CodeSnippetDeclarations();
            this.imports = new CodeSnippetImports();
            this.references = new CodeSnippetReferences();
            this.code = new CodeSnippetCode();
            this.imageIndex = -1;
        }

        public CodeSnippet(ICodeSnippets parent) : this()
        {
            this.parent = parent;
        }

        protected virtual void OnCodeChanged()
        {
        }

        protected virtual void OnCustomDataChanged()
        {
        }

        protected virtual void OnHeaderChanged()
        {
        }

        protected virtual void OnImageIndexChanged()
        {
        }

        protected virtual void OnParentChanged()
        {
        }

        protected virtual void OnSnippetDeclarationsChanged()
        {
        }

        protected virtual void OnSnippetImportsChanged()
        {
        }

        protected virtual void OnSnippetReferencesChanged()
        {
        }

        public virtual ICodeSnippetCode Code
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

        public virtual object CustomData
        {
            get
            {
                return this.customData;
            }
            set
            {
                if (this.customData != value)
                {
                    this.customData = value;
                    this.OnCustomDataChanged();
                }
            }
        }

        public virtual ICodeSnippetDeclarations Declarations
        {
            get
            {
                return this.declarations;
            }
        }

        public virtual string Description
        {
            get
            {
                string description = this.header.Description;
                if ((this.header.Shortcut != null) && (this.header.Shortcut != string.Empty))
                {
                    description = description + (((this.parent != null) && this.parent.UseHtmlFormatting) ? SyntaxConsts.DefaultBrTag : "\r\n") + string.Format(StringConsts.CodeSnippetTooltipHeader, this.header.Shortcut);
                }
                return description.Trim();
            }
        }

        public virtual ICodeSnippetHeader Header
        {
            get
            {
                return this.header;
            }
            set
            {
                if (this.header != value)
                {
                    this.header = value;
                    this.OnHeaderChanged();
                }
            }
        }

        public virtual int ImageIndex
        {
            get
            {
                return this.imageIndex;
            }
            set
            {
                if (this.imageIndex != value)
                {
                    this.imageIndex = value;
                    this.OnImageIndexChanged();
                }
            }
        }

        public virtual ICodeSnippetImports Imports
        {
            get
            {
                return this.imports;
            }
        }

        public virtual ICodeSnippets Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if (this.parent != value)
                {
                    this.parent = value;
                    this.OnParentChanged();
                }
            }
        }

        public virtual ICodeSnippetReferences References
        {
            get
            {
                return this.references;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

