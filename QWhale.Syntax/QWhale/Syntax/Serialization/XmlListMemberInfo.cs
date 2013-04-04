namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    public class XmlListMemberInfo : ISerializationInfo
    {
        private MemberAttribute attributes;
        private int currentParamIndex;
        private string dataType;
        private string description;
        private string displayText;
        private int imageIndex;
        private int memberType;
        private string name;
        private int overloads;
        private IListMember owner;
        private XmlParameterMemberInfo[] parameters;
        private string paramText;
        private int priority;
        private string qualifier;

        public XmlListMemberInfo()
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.qualifier = string.Empty;
            this.paramText = string.Empty;
            this.displayText = string.Empty;
            this.dataType = string.Empty;
            this.memberType = -1;
            this.imageIndex = -1;
            this.currentParamIndex = -1;
            this.parameters = new XmlParameterMemberInfo[0];
        }

        public XmlListMemberInfo(IListMember owner)
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.qualifier = string.Empty;
            this.paramText = string.Empty;
            this.displayText = string.Empty;
            this.dataType = string.Empty;
            this.memberType = -1;
            this.imageIndex = -1;
            this.currentParamIndex = -1;
            this.parameters = new XmlParameterMemberInfo[0];
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IListMember) owner;
            this.Name = this.name;
            this.Description = this.description;
            this.Qualifier = this.qualifier;
            this.ParamText = this.paramText;
            this.DisplayText = this.displayText;
            this.DataType = this.dataType;
            this.Overloads = this.overloads;
            this.MemberType = this.memberType;
            this.ImageIndex = this.imageIndex;
            this.Attributes = this.attributes;
            this.Priority = this.priority;
            this.CurrentParamIndex = this.currentParamIndex;
            this.Parameters = this.parameters;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.description = this.Description;
                this.qualifier = this.Qualifier;
                this.paramText = this.ParamText;
                this.displayText = this.DisplayText;
                this.dataType = this.DataType;
                this.overloads = this.Overloads;
                this.memberType = this.MemberType;
                this.attributes = this.Attributes;
                this.imageIndex = this.ImageIndex;
                this.priority = this.Priority;
                this.currentParamIndex = this.CurrentParamIndex;
                this.parameters = this.Parameters;
                foreach (XmlParameterMemberInfo info in this.parameters)
                {
                    info.Load();
                }
            }
        }

        public bool ShouldSerializeParameters()
        {
            return ((this.parameters.Length > 0) || ((this.owner != null) && (this.owner.Parameters.Count > 0)));
        }

        [DefaultValue(0)]
        public MemberAttribute Attributes
        {
            get
            {
                if (this.owner == null)
                {
                    return this.attributes;
                }
                return this.owner.Attributes;
            }
            set
            {
                this.attributes = value;
                if (this.owner != null)
                {
                    this.owner.Attributes = value;
                }
            }
        }

        [DefaultValue(-1)]
        public int CurrentParamIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.currentParamIndex;
                }
                return this.owner.CurrentParamIndex;
            }
            set
            {
                this.currentParamIndex = value;
                if (this.owner != null)
                {
                    this.owner.CurrentParamIndex = value;
                }
            }
        }

        [DefaultValue("")]
        public string DataType
        {
            get
            {
                if (this.owner == null)
                {
                    return this.dataType;
                }
                return this.owner.DataType;
            }
            set
            {
                this.dataType = value;
                if (this.owner != null)
                {
                    this.owner.DataType = value;
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
        public string DisplayText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.displayText;
                }
                return this.owner.DisplayText;
            }
            set
            {
                this.displayText = value;
                if (this.owner != null)
                {
                    this.owner.DisplayText = value;
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

        [DefaultValue(-1)]
        public int MemberType
        {
            get
            {
                if (this.owner == null)
                {
                    return this.memberType;
                }
                return this.owner.MemberType;
            }
            set
            {
                this.memberType = value;
                if (this.owner != null)
                {
                    this.owner.MemberType = value;
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

        [DefaultValue(0)]
        public int Overloads
        {
            get
            {
                if (this.owner == null)
                {
                    return this.overloads;
                }
                return this.owner.Overloads;
            }
            set
            {
                this.overloads = value;
                if (this.owner != null)
                {
                    this.owner.Overloads = value;
                }
            }
        }

        [XmlArray("Parameters"), XmlArrayItem("Parameter")]
        public XmlParameterMemberInfo[] Parameters
        {
            get
            {
                if ((this.owner == null) || (this.owner.Parameters == null))
                {
                    return this.parameters;
                }
                XmlParameterMemberInfo[] infoArray = new XmlParameterMemberInfo[this.owner.Parameters.Count];
                for (int i = 0; i < this.owner.Parameters.Count; i++)
                {
                    infoArray[i] = (XmlParameterMemberInfo) this.owner.Parameters[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.parameters = value;
                if (this.owner != null)
                {
                    if (this.owner.Parameters == null)
                    {
                        this.owner.Parameters = new ParameterMembers();
                    }
                    this.owner.Parameters.Clear();
                    foreach (XmlParameterMemberInfo info in value)
                    {
                        this.owner.Parameters.AddParameterMember().SerializationInfo = info;
                    }
                }
            }
        }

        [DefaultValue("")]
        public string ParamText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.paramText;
                }
                return this.owner.ParamText;
            }
            set
            {
                this.paramText = value;
                if (this.owner != null)
                {
                    this.owner.ParamText = value;
                }
            }
        }

        [DefaultValue(0)]
        public int Priority
        {
            get
            {
                if (this.owner == null)
                {
                    return this.priority;
                }
                return this.owner.Priority;
            }
            set
            {
                this.priority = value;
                if (this.owner != null)
                {
                    this.owner.Priority = value;
                }
            }
        }

        [DefaultValue("")]
        public string Qualifier
        {
            get
            {
                if (this.owner == null)
                {
                    return this.qualifier;
                }
                return this.owner.Qualifier;
            }
            set
            {
                this.qualifier = value;
                if (this.owner != null)
                {
                    this.owner.Qualifier = value;
                }
            }
        }
    }
}

