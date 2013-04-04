namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class ParameterMember : IParameterMember, ICodeCompletionProviderItem
    {
        private string dataType = string.Empty;
        private string description = string.Empty;
        private ParameterModifer modifiers;
        private string name = string.Empty;
        private string qualifier = string.Empty;
        private string text = string.Empty;

        protected virtual void OnDataTypeChanged()
        {
        }

        protected virtual void OnDescriptionChanged()
        {
        }

        protected virtual void OnModifiersChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnQualifierChanged()
        {
        }

        protected virtual void OnTextChanged()
        {
        }

        public virtual string DataType
        {
            get
            {
                return this.dataType;
            }
            set
            {
                if (this.dataType != value)
                {
                    this.dataType = value;
                    this.OnDataTypeChanged();
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

        public virtual ParameterModifer Modifiers
        {
            get
            {
                return this.modifiers;
            }
            set
            {
                if (this.modifiers != value)
                {
                    this.modifiers = value;
                    this.OnModifiersChanged();
                }
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

        public virtual string Qualifier
        {
            get
            {
                return this.qualifier;
            }
            set
            {
                if (this.qualifier != value)
                {
                    this.qualifier = value;
                    this.OnQualifierChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlParameterMemberInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnTextChanged();
                }
            }
        }
    }
}

