namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Parsers;
    using System;

    public class CsListMember : ListMember
    {
        private bool fullDescription;

        public CsListMember()
        {
            this.fullDescription = true;
        }

        public CsListMember(IListMembers owner, bool fullDescription) : base(owner)
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
                    string str2 = compact ? member.Name : ((member.Text != string.Empty) ? member.Text : base.JoinWithSpace(new string[] { member.Qualifier, member.DataType, member.Name }));
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

        public override string GetTemplate(bool compact)
        {
            string str = string.Empty;
            if (!compact)
            {
                str = base.RemoveStrings(this.Qualifier, new string[] { CsLexerToken.Virtual.ToString().ToLower(), CsLexerToken.Abstract.ToString().ToLower() }, true).Trim();
                string str2 = CsLexerToken.Override.ToString().ToLower();
                if (str.IndexOf(str2) < 0)
                {
                    str = base.JoinWithSpace(str, str2);
                }
            }
            string str3 = base.JoinWithSpace(new string[] { str, this.DataType, this.Name }) + this.GetParamText(false, false);
            switch (this.MemberType)
            {
                case 10:
                    if (!compact)
                    {
                        object obj2 = str3 + "\r\n{";
                        string str4 = string.Concat(new object[] { obj2, "\r\n", '\t', SyntaxConsts.DefaultCaretSymbol });
                        str3 = (str4 + CsLexerToken.Base.ToString().ToLower() + "." + this.Name + this.GetParamText(false, true) + ";") + "\r\n}";
                    }
                    return str3;

                case 11:
                    str3 = str3 + (compact ? "{" : "\r\n{");
                    if ((this.Attributes & MemberAttribute.CanRead) != MemberAttribute.None)
                    {
                        if (compact)
                        {
                            object obj8 = str3;
                            str3 = string.Concat(new object[] { obj8, ' ', CsLexerToken.Get.ToString().ToLower(), ";" });
                            break;
                        }
                        object obj3 = str3;
                        object obj4 = string.Concat(new object[] { obj3, "\r\n", '\t', CsLexerToken.Get.ToString().ToLower() });
                        object obj5 = string.Concat(new object[] { obj4, "\r\n", '\t', "{" });
                        object obj6 = string.Concat(new object[] { obj5, "\r\n", '\t', SyntaxConsts.DefaultCaretSymbol });
                        object obj7 = string.Concat(new object[] { obj6, CsLexerToken.Return.ToString().ToLower(), ' ', CsLexerToken.Base.ToString().ToLower(), ".", this.Name, this.GetParamText(false, true), ";" });
                        str3 = string.Concat(new object[] { obj7, "\r\n", '\t', "}" });
                    }
                    break;

                default:
                    return str3;
            }
            if ((this.Attributes & MemberAttribute.CanWrite) != MemberAttribute.None)
            {
                if (!compact)
                {
                    object obj9 = str3;
                    object obj10 = string.Concat(new object[] { obj9, "\r\n", '\t', CsLexerToken.Set.ToString().ToLower() });
                    object obj11 = string.Concat(new object[] { obj10, "\r\n", '\t', "{" });
                    object obj12 = string.Concat(new object[] { obj11, "\r\n", '\t', CsLexerToken.Base.ToString().ToLower(), ".", this.Name, this.GetParamText(false, true), "=", CsLexerToken.Value.ToString().ToLower(), ";" });
                    str3 = string.Concat(new object[] { obj12, "\r\n", '\t', "}" });
                }
                else
                {
                    object obj13 = str3;
                    str3 = string.Concat(new object[] { obj13, ' ', CsLexerToken.Set.ToString().ToLower(), ";" });
                }
            }
            return (str3 + (compact ? "}" : "\r\n}"));
        }

        protected virtual string Desc
        {
            get
            {
                string str = base.JoinWithSpace(new string[] { this.Qualifier, this.DataType, this.DisplayText });
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
                        return str;
                }
                str = base.JoinWithSpace(str, this.ParamText);
                if (this.Overloads > 0)
                {
                    str = base.JoinWithSpace(str, string.Format(StringConsts.MethodOverloads, this.Overloads));
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

