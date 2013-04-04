namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Xml.Serialization;

    [XmlRoot("ListMembersCollection")]
    public class XmlListMembersInfo : ISerializationInfo
    {
        private XmlListMemberInfo[] members;
        private IListMembers owner;
        private bool showDescriptions;
        private bool showHints;
        private bool showParams;
        private bool showQualifiers;
        private bool showResults;

        public XmlListMembersInfo()
        {
            this.members = new XmlListMemberInfo[0];
        }

        public XmlListMembersInfo(IListMembers owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IListMembers) owner;
            this.Members = this.members;
            this.ShowHints = this.showHints;
            this.ShowQualifiers = this.showQualifiers;
            this.ShowResults = this.showResults;
            this.ShowParams = this.showParams;
            this.ShowDescriptions = this.showDescriptions;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.members = this.Members;
                this.showHints = this.ShowHints;
                this.showQualifiers = this.ShowQualifiers;
                this.showResults = this.ShowResults;
                this.showParams = this.ShowParams;
                this.showDescriptions = this.ShowDescriptions;
                foreach (XmlListMemberInfo info in this.members)
                {
                    info.Load();
                }
            }
        }

        [XmlArrayItem("ListMember"), XmlArray("ListMembers")]
        public XmlListMemberInfo[] Members
        {
            get
            {
                if (this.owner == null)
                {
                    return this.members;
                }
                XmlListMemberInfo[] infoArray = new XmlListMemberInfo[this.owner.Count];
                for (int i = 0; i < this.owner.Count; i++)
                {
                    infoArray[i] = (XmlListMemberInfo) this.owner[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.members = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Clear();
                    foreach (XmlListMemberInfo info in value)
                    {
                        this.owner.AddListMember().SerializationInfo = info;
                    }
                }
            }
        }

        public bool ShowDescriptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showDescriptions;
                }
                return this.owner.ShowDescriptions;
            }
            set
            {
                this.showDescriptions = value;
                if (this.owner != null)
                {
                    this.owner.ShowDescriptions = value;
                }
            }
        }

        public bool ShowHints
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showHints;
                }
                return this.owner.ShowHints;
            }
            set
            {
                this.showHints = value;
                if (this.owner != null)
                {
                    this.owner.ShowHints = value;
                }
            }
        }

        public bool ShowParams
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showParams;
                }
                return this.owner.ShowParams;
            }
            set
            {
                this.showParams = value;
                if (this.owner != null)
                {
                    this.owner.ShowParams = value;
                }
            }
        }

        public bool ShowQualifiers
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showQualifiers;
                }
                return this.owner.ShowQualifiers;
            }
            set
            {
                this.showQualifiers = value;
                if (this.owner != null)
                {
                    this.owner.ShowQualifiers = value;
                }
            }
        }

        public bool ShowResults
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showResults;
                }
                return this.owner.ShowResults;
            }
            set
            {
                this.showResults = value;
                if (this.owner != null)
                {
                    this.owner.ShowResults = value;
                }
            }
        }
    }
}

