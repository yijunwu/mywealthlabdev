namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;

    public class XmlParameterMemberInfo : ISerializationInfo
    {
        private string dataType;
        private string description;
        private ParameterModifer modifiers;
        private string name;
        private IParameterMember owner;
        private string qualifier;

        public XmlParameterMemberInfo()
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.qualifier = string.Empty;
            this.dataType = string.Empty;
        }

        public XmlParameterMemberInfo(IParameterMember owner)
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.qualifier = string.Empty;
            this.dataType = string.Empty;
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IParameterMember) owner;
            this.Name = this.name;
            this.Description = this.description;
            this.Qualifier = this.qualifier;
            this.DataType = this.dataType;
            this.Modifiers = this.modifiers;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.description = this.Description;
                this.qualifier = this.Qualifier;
                this.dataType = this.DataType;
                this.modifiers = this.Modifiers;
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

        [DefaultValue(0)]
        public ParameterModifer Modifiers
        {
            get
            {
                if (this.owner == null)
                {
                    return this.modifiers;
                }
                return this.owner.Modifiers;
            }
            set
            {
                this.modifiers = value;
                if (this.owner != null)
                {
                    this.owner.Modifiers = value;
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

