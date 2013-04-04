namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class ListMember : IListMember, ICodeCompletionProviderItem
    {
        private MemberAttribute attributes;
        private int currentParamIndex;
        private object customData;
        private string dataType;
        private string description;
        private string displayText;
        private int imageIndex;
        private int memberType;
        private string name;
        private int overloads;
        private IListMembers owner;
        private IParameterMembers parameters;
        private string paramText;
        private int priority;
        private string qualifier;

        public ListMember()
        {
            this.qualifier = string.Empty;
            this.paramText = string.Empty;
            this.name = string.Empty;
            this.displayText = string.Empty;
            this.description = string.Empty;
            this.dataType = string.Empty;
            this.memberType = -1;
            this.imageIndex = -1;
            this.currentParamIndex = -1;
        }

        public ListMember(IListMembers owner)
        {
            this.qualifier = string.Empty;
            this.paramText = string.Empty;
            this.name = string.Empty;
            this.displayText = string.Empty;
            this.description = string.Empty;
            this.dataType = string.Empty;
            this.memberType = -1;
            this.imageIndex = -1;
            this.currentParamIndex = -1;
            this.owner = owner;
        }

        public virtual void AddDescription(string text)
        {
            if (text != string.Empty)
            {
                if (this.description != string.Empty)
                {
                    this.description = this.description + (((this.Owner != null) && this.Owner.UseHtmlFormatting) ? SyntaxConsts.DefaultBrTag : "\r\n") + text;
                }
                else
                {
                    this.description = text;
                }
            }
        }

        public virtual string GetParamText(bool useFormatting)
        {
            if (this.paramText != string.Empty)
            {
                return this.paramText;
            }
            string str = string.Empty;
            if (this.Parameters != null)
            {
                foreach (IParameterMember member in this.Parameters)
                {
                    string text = member.Text;
                    if (text != string.Empty)
                    {
                        str = str + ((str == string.Empty) ? text : ("," + ' ' + text));
                    }
                }
            }
            if (!(str != string.Empty))
            {
                return str;
            }
            return ("(" + str + ")");
        }

        public virtual string GetTemplate(bool addBase)
        {
            return this.Name;
        }

        protected string JoinWithSpace(string[] arr)
        {
            string str = string.Empty;
            foreach (string str2 in arr)
            {
                if (str == string.Empty)
                {
                    str = str2;
                }
                else if (str2 != string.Empty)
                {
                    str = str + ' ' + str2;
                }
            }
            return str;
        }

        protected string JoinWithSpace(string s1, string s2)
        {
            if ((s1 != string.Empty) && (s2 != string.Empty))
            {
                return (s1 + ' ' + s2);
            }
            if (!(s1 == string.Empty))
            {
                return s1;
            }
            return s2;
        }

        protected virtual void OnAttributesChanged()
        {
        }

        protected virtual void OnCurrentParamIndexChanged()
        {
        }

        protected virtual void OnCustomDataChanged()
        {
        }

        protected virtual void OnDataTypeChanged()
        {
        }

        protected virtual void OnDescriptionChanged()
        {
        }

        protected virtual void OnDisplayTextChanged()
        {
        }

        protected virtual void OnImageIndexChanged()
        {
        }

        protected virtual void OnMemberTypeChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnOverloadsChanged()
        {
        }

        protected virtual void OnOwnerChanged()
        {
        }

        protected virtual void OnParametersChanged()
        {
        }

        protected virtual void OnParamsChanged()
        {
        }

        protected virtual void OnParamTextChanged()
        {
        }

        protected virtual void OnPriorityChanged()
        {
        }

        protected virtual void OnQualifierChanged()
        {
        }

        protected string RemoveStrings(string s, string[] arr, bool caseSensitive)
        {
            string str = s;
            foreach (string str2 in arr)
            {
                int startIndex = caseSensitive ? str.IndexOf(str2) : str.ToLower().IndexOf(str2.ToLower());
                if (startIndex >= 0)
                {
                    str = str.Remove(startIndex, str2.Length);
                }
            }
            return str;
        }

        public virtual MemberAttribute Attributes
        {
            get
            {
                return this.attributes;
            }
            set
            {
                if (this.attributes != value)
                {
                    this.attributes = value;
                    this.OnAttributesChanged();
                }
            }
        }

        public virtual int CurrentParamIndex
        {
            get
            {
                return this.currentParamIndex;
            }
            set
            {
                if (this.currentParamIndex != value)
                {
                    this.currentParamIndex = value;
                    this.OnCurrentParamIndexChanged();
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

        public virtual string DisplayText
        {
            get
            {
                if (!(this.displayText != string.Empty))
                {
                    return this.Name;
                }
                return this.displayText;
            }
            set
            {
                if (this.displayText != value)
                {
                    this.displayText = value;
                    this.OnDisplayTextChanged();
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

        public virtual int MemberType
        {
            get
            {
                return this.memberType;
            }
            set
            {
                if (this.memberType != value)
                {
                    this.memberType = value;
                    this.OnMemberTypeChanged();
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

        public virtual int Overloads
        {
            get
            {
                return this.overloads;
            }
            set
            {
                if (this.overloads != value)
                {
                    this.overloads = value;
                    this.OnOverloadsChanged();
                }
            }
        }

        public virtual IListMembers Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                if (this.owner != value)
                {
                    this.owner = value;
                    this.OnOwnerChanged();
                }
            }
        }

        public virtual IParameterMembers Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                if (this.parameters != value)
                {
                    this.parameters = value;
                    this.OnParametersChanged();
                }
            }
        }

        public virtual string ParamText
        {
            get
            {
                return this.GetParamText(true);
            }
            set
            {
                if (this.paramText != value)
                {
                    this.paramText = value;
                    this.OnParamTextChanged();
                }
            }
        }

        public virtual int Priority
        {
            get
            {
                return this.priority;
            }
            set
            {
                if (this.priority != value)
                {
                    this.priority = value;
                    this.OnPriorityChanged();
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
                return new XmlListMemberInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

