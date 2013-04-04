namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Xml.Serialization;

    [XmlRoot("CodeSnippetCollection")]
    public class XmlCodeSnippetMembersInfo : ISerializationInfo
    {
        private XmlCodeSnippetMemberInfo[] members;
        private ICodeSnippetMembers owner;

        public XmlCodeSnippetMembersInfo()
        {
            this.members = new XmlCodeSnippetMemberInfo[0];
        }

        public XmlCodeSnippetMembersInfo(ICodeSnippetMembers owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippetMembers) owner;
            this.Members = this.members;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.members = this.Members;
                foreach (XmlCodeSnippetMemberInfo info in this.members)
                {
                    info.Load();
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
                XmlCodeSnippetMemberInfo[] infoArray = new XmlCodeSnippetMemberInfo[this.owner.Count];
                for (int i = 0; i < this.owner.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetMemberInfo) this.owner[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.members = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Clear();
                    foreach (XmlCodeSnippetMemberInfo info in value)
                    {
                        this.owner.AddSnippetMember().SerializationInfo = info;
                    }
                }
            }
        }
    }
}

