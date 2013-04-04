namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    public class XmlCodeSnippetMemberInfo : ISerializationInfo
    {
        private int imageIndex;
        private XmlCodeSnippetMemberInfo[] members;
        private string name;
        private ICodeSnippetMember owner;
        private string path;
        private XmlCodeSnippetInfo[] snippets;

        public XmlCodeSnippetMemberInfo()
        {
            this.members = new XmlCodeSnippetMemberInfo[0];
            this.snippets = new XmlCodeSnippetInfo[0];
            this.name = string.Empty;
            this.path = string.Empty;
            this.imageIndex = -1;
        }

        public XmlCodeSnippetMemberInfo(ICodeSnippetMember owner)
        {
            this.members = new XmlCodeSnippetMemberInfo[0];
            this.snippets = new XmlCodeSnippetInfo[0];
            this.name = string.Empty;
            this.path = string.Empty;
            this.imageIndex = -1;
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippetMember) owner;
            this.Name = this.name;
            this.Path = this.path;
            this.ImageIndex = this.imageIndex;
            this.Members = this.members;
            this.Snippets = this.snippets;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.path = this.Path;
                this.imageIndex = this.ImageIndex;
                this.members = this.Members;
                this.snippets = this.Snippets;
                foreach (XmlCodeSnippetMemberInfo info in this.members)
                {
                    info.Load();
                }
                foreach (XmlCodeSnippetInfo info2 in this.snippets)
                {
                    info2.Load();
                }
            }
        }

        [DefaultValue(-1)]
        public int ImageIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.imageIndex;
                }
                return this.owner.ImageIndex;
            }
            set
            {
                this.imageIndex = value;
                if (this.owner != null)
                {
                    this.owner.ImageIndex = value;
                }
            }
        }

        [XmlArray("CodeSnippetMembers"), XmlArrayItem("Member")]
        public XmlCodeSnippetMemberInfo[] Members
        {
            get
            {
                if (this.owner == null)
                {
                    return this.members;
                }
                XmlCodeSnippetMemberInfo[] infoArray = new XmlCodeSnippetMemberInfo[this.owner.Members.Count];
                for (int i = 0; i < this.owner.Members.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetMemberInfo) this.owner.Members[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.members = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Members.Clear();
                    foreach (XmlCodeSnippetMemberInfo info in value)
                    {
                        this.owner.Members.AddSnippetMember().SerializationInfo = info;
                    }
                }
            }
        }

        [DefaultValue("")]
        public string Name
        {
            get
            {
                if (this.owner == null)
                {
                    return this.name;
                }
                return this.owner.Name;
            }
            set
            {
                this.name = value;
                if (this.owner != null)
                {
                    this.owner.Name = value;
                }
            }
        }

        [DefaultValue("")]
        public string Path
        {
            get
            {
                if (this.owner == null)
                {
                    return this.path;
                }
                return this.owner.Path;
            }
            set
            {
                this.path = value;
                if (this.owner != null)
                {
                    this.owner.Path = value;
                }
            }
        }

        [XmlArray("CodeSnippets"), XmlArrayItem("Snippet")]
        public XmlCodeSnippetInfo[] Snippets
        {
            get
            {
                if (this.owner == null)
                {
                    return this.snippets;
                }
                XmlCodeSnippetInfo[] infoArray = new XmlCodeSnippetInfo[this.owner.Snippets.Count];
                for (int i = 0; i < this.owner.Snippets.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetInfo) this.owner.Snippets[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.snippets = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Snippets.Clear();
                    foreach (XmlCodeSnippetInfo info in value)
                    {
                        this.owner.Snippets.AddSnippet().SerializationInfo = info;
                    }
                }
            }
        }
    }
}

