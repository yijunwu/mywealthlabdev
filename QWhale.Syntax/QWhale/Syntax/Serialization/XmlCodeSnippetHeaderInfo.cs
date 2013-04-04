namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    public class XmlCodeSnippetHeaderInfo : ISerializationInfo
    {
        private string author;
        private string description;
        private ICodeSnippetHeader owner;
        private string shortcut;
        private XmlCodeSnippetTypeInfo[] snippetTypes;
        private string title;

        public XmlCodeSnippetHeaderInfo()
        {
            this.title = string.Empty;
            this.description = string.Empty;
            this.author = string.Empty;
            this.shortcut = string.Empty;
            this.snippetTypes = new XmlCodeSnippetTypeInfo[0];
        }

        public XmlCodeSnippetHeaderInfo(ICodeSnippetHeader owner)
        {
            this.title = string.Empty;
            this.description = string.Empty;
            this.author = string.Empty;
            this.shortcut = string.Empty;
            this.snippetTypes = new XmlCodeSnippetTypeInfo[0];
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippetHeader) owner;
            this.Title = this.title;
            this.Description = this.description;
            this.Author = this.author;
            this.Shortcut = this.shortcut;
            this.SnippetTypes = this.snippetTypes;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.title = this.Title;
                this.description = this.Description;
                this.author = this.Author;
                this.shortcut = this.Shortcut;
                this.snippetTypes = this.SnippetTypes;
                foreach (XmlCodeSnippetTypeInfo info in this.snippetTypes)
                {
                    info.Load();
                }
            }
        }

        public bool ShouldSerializeSnippetTypes()
        {
            return ((this.snippetTypes.Length > 0) || ((this.owner != null) && (this.owner.Types.Count > 0)));
        }

        [DefaultValue("")]
        public string Author
        {
            get
            {
                if (this.owner == null)
                {
                    return this.author;
                }
                return this.owner.Author;
            }
            set
            {
                this.author = value;
                if (this.owner != null)
                {
                    this.owner.Author = value;
                }
            }
        }

        [DefaultValue("")]
        public string Description
        {
            get
            {
                if (this.owner == null)
                {
                    return this.description;
                }
                return this.owner.Description;
            }
            set
            {
                this.description = value;
                if (this.owner != null)
                {
                    this.owner.Description = value;
                }
            }
        }

        [DefaultValue("")]
        public string Shortcut
        {
            get
            {
                if (this.owner == null)
                {
                    return this.shortcut;
                }
                return this.owner.Shortcut;
            }
            set
            {
                this.shortcut = value;
                if (this.owner != null)
                {
                    this.owner.Shortcut = value;
                }
            }
        }

        [XmlArray("SnippetTypes"), XmlArrayItem("SnippetType")]
        public XmlCodeSnippetTypeInfo[] SnippetTypes
        {
            get
            {
                if (this.owner == null)
                {
                    return this.snippetTypes;
                }
                XmlCodeSnippetTypeInfo[] infoArray = new XmlCodeSnippetTypeInfo[this.owner.Types.Count];
                for (int i = 0; i < this.owner.Types.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetTypeInfo) this.owner.Types[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.snippetTypes = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Types.Clear();
                    foreach (XmlCodeSnippetTypeInfo info in value)
                    {
                        this.owner.Types.AddSnippetType().SerializationInfo = info;
                    }
                }
            }
        }

        [DefaultValue("")]
        public string Title
        {
            get
            {
                if (this.owner == null)
                {
                    return this.title;
                }
                return this.owner.Title;
            }
            set
            {
                this.title = value;
                if (this.owner != null)
                {
                    this.owner.Title = value;
                }
            }
        }
    }
}

