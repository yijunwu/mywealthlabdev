namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using System;

    public class JsListMember : ListMember
    {
        private bool fullDescription;

        public JsListMember()
        {
            this.fullDescription = true;
        }

        public JsListMember(IListMembers owner, bool fullDescription) : base(owner)
        {
            this.fullDescription = true;
            this.fullDescription = fullDescription;
        }

        public override string GetParamText(bool useFormatting)
        {
            string paramText = base.GetParamText(useFormatting);
            if ((paramText == null) || !(paramText != string.Empty))
            {
                return this.GetParamText(useFormatting && ((this.Owner == null) || this.Owner.UseHtmlFormatting), false);
            }
            return paramText;
        }

        protected virtual string GetParamText(bool useFormatting, bool compact)
        {
            string str = string.Empty;
            if (this.Parameters != null)
            {
                for (int i = 0; i < this.Parameters.Count; i++)
                {
                    IParameterMember member = this.Parameters[i];
                    string str2 = compact ? member.Name : ((member.Text != string.Empty) ? member.Text : (base.JoinWithSpace(member.Qualifier, member.Name) + ((member.DataType != string.Empty) ? (" : " + member.DataType) : string.Empty)));
                    if (useFormatting && (i == this.CurrentParamIndex))
                    {
                        str2 = SyntaxConsts.DefaultBoldTag + str2 + SyntaxConsts.DefaultBoldEndTag;
                    }
                    str = (str != string.Empty) ? (str + ", " + str2) : str2;
                }
            }
            if (this.MemberType != 11)
            {
                return ("(" + str + ")");
            }
            if (!(str != string.Empty))
            {
                return str;
            }
            return ("[" + str + "]");
        }

        protected virtual string Desc
        {
            get
            {
                string str = base.JoinWithSpace(new string[] { this.Qualifier, this.DisplayText });
                switch (this.MemberType)
                {
                    case 10:
                    case 11:
                    {
                        if (((this.Parameters == null) || (this.CurrentParamIndex < 0)) || (this.CurrentParamIndex >= this.Parameters.Count))
                        {
                            break;
                        }
                        string description = this.Parameters[this.CurrentParamIndex].Description;
                        if (!(description != string.Empty))
                        {
                            break;
                        }
                        return description;
                    }
                    default:
                        goto Label_0093;
                }
                str = base.JoinWithSpace(str, this.ParamText);
            Label_0093:
                if (this.DataType != string.Empty)
                {
                    str = str + " : " + this.DataType;
                }
                switch (this.MemberType)
                {
                    case 10:
                    case 11:
                        if (this.Overloads > 0)
                        {
                            str = base.JoinWithSpace(str, string.Format(StringConsts.MethodOverloads, this.Overloads));
                        }
                        return str;
                }
                return str;
            }
        }

        public override string Description
        {
            get
            {
                if (!this.fullDescription || ((this.Attributes & MemberAttribute.NoDescription) != MemberAttribute.None))
                {
                    return base.Description;
                }
                string desc = this.Desc;
                if (!(base.Description != string.Empty))
                {
                    return desc;
                }
                return (desc + (((this.Owner != null) && this.Owner.UseHtmlFormatting) ? SyntaxConsts.DefaultBrTag : "\r\n") + base.Description);
            }
        }
    }
}

