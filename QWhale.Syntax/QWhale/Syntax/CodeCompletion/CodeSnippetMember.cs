namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class CodeSnippetMember : ICodeSnippetMember, ICodeCompletionProviderItem
    {
        private int imageIndex;
        private ICodeSnippetMembers members;
        private string name;
        private ICodeSnippetMembers parent;
        private string path;
        private ICodeSnippets snippets;

        public CodeSnippetMember()
        {
            this.name = string.Empty;
            this.path = string.Empty;
            this.imageIndex = -1;
            this.members = new CodeSnippetMembers(this);
            this.snippets = new CodeSnippets(this);
        }

        public CodeSnippetMember(ICodeSnippetMembers parent) : this()
        {
            this.parent = parent;
        }

        protected virtual void OnImageIndexChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnParentChanged()
        {
        }

        protected virtual void OnPathChanged()
        {
        }

        public virtual string EditPath
        {
            get
            {
                string str = string.Empty;
                for (ICodeSnippetMember member = this; member != null; member = (member.Parent != null) ? member.Parent.Parent : null)
                {
                    str = (str == string.Empty) ? member.Name : (member.Name + SyntaxConsts.DefaultSnippetPathSeparator + str);
                }
                if (str != string.Empty)
                {
                    str = str + SyntaxConsts.DefaultSnippetPathSeparator;
                }
                return str;
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

        public virtual ICodeSnippetMembers Members
        {
            get
            {
                return this.members;
            }
        }

        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnNameChanged();
                }
            }
        }

        public virtual ICodeSnippetMembers Parent
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

        public virtual string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                if (this.path != value)
                {
                    this.path = value;
                    this.OnPathChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetMemberInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual ICodeSnippets Snippets
        {
            get
            {
                return this.snippets;
            }
        }

        public virtual ICodeSnippetsProvider SnippetsAndMembers
        {
            get
            {
                if (this.members.Count == 0)
                {
                    return this.snippets;
                }
                if (this.snippets.Count == 0)
                {
                    return this.members;
                }
                ICodeSnippetsProvider provider = new CodeSnippetsProvider(this);
                foreach (ICodeSnippet snippet in this.snippets)
                {
                    provider.Add(snippet);
                }
                foreach (ICodeSnippetMember member in this.members)
                {
                    provider.Add(member);
                }
                return provider;
            }
        }
    }
}

