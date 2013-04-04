namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Parsers;
    using System;

    public class VbListMember : ListMember
    {
        private bool fullDescription;

        public VbListMember()
        {
            this.fullDescription = true;
        }

        public VbListMember(IListMembers owner, bool fullDescription) : base(owner)
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
                    string name = member.Name;
                    if (!compact)
                    {
                        if (member.Text != string.Empty)
                        {
                            name = member.Text;
                        }
                        else
                        {
                            string str3 = member.Qualifier.ToLower();
                            ParameterModifer modifiers = member.Modifiers;
                            if (str3.IndexOf(VbLexerToken.ByVal.ToString().ToLower()) >= 0)
                            {
                                modifiers |= ParameterModifer.In;
                            }
                            if (str3.IndexOf(VbLexerToken.ByRef.ToString().ToLower()) >= 0)
                            {
                                modifiers |= ParameterModifer.Retval;
                            }
                            if (str3.IndexOf(VbLexerToken.Optional.ToString().ToLower()) >= 0)
                            {
                                modifiers |= ParameterModifer.Optional;
                            }
                            string dataType = member.DataType;
                            if (dataType != string.Empty)
                            {
                                if (dataType[dataType.Length - 1] == '&')
                                {
                                    dataType = dataType.Substring(0, dataType.Length - 1);
                                    modifiers |= ParameterModifer.Retval;
                                }
                                object obj2 = name;
                                name = string.Concat(new object[] { obj2, ' ', VbLexerToken.As.ToString(), ' ', dataType });
                            }
                            if ((modifiers & ParameterModifer.In) != ParameterModifer.None)
                            {
                                name = base.JoinWithSpace(VbLexerToken.ByVal.ToString(), name);
                            }
                            else if (((modifiers & ParameterModifer.Out) != ParameterModifer.None) || ((modifiers & ParameterModifer.Retval) != ParameterModifer.None))
                            {
                                name = base.JoinWithSpace(VbLexerToken.ByRef.ToString(), name);
                            }
                            if ((modifiers & ParameterModifer.Optional) != ParameterModifer.None)
                            {
                                name = "[" + name + "]";
                            }
                        }
                    }
                    if (useFormatting && (i == this.CurrentParamIndex))
                    {
                        name = SyntaxConsts.DefaultBoldTag + name + SyntaxConsts.DefaultBoldEndTag;
                    }
                    str = (str != string.Empty) ? (str + ", " + name) : name;
                }
            }
            if (!(str != string.Empty))
            {
                return str;
            }
            return ("(" + str + ")");
        }

        public override string GetTemplate(bool compact)
        {
            string str = string.Empty;
            if (!compact)
            {
                str = base.RemoveStrings(this.Qualifier, new string[] { VbLexerToken.MustInherit.ToString(), VbLexerToken.MustInherit.ToString(), VbLexerToken.Overridable.ToString(), "virtual" }, false).Trim();
                string str2 = VbLexerToken.Overrides.ToString().ToLower();
                if (str.IndexOf(str2) < 0)
                {
                    str = base.JoinWithSpace(str, str2);
                }
            }
            string str3 = string.Empty;
            if (!compact)
            {
                switch (this.MemberType)
                {
                    case 10:
                        str3 = (this.DataType != string.Empty) ? VbLexerToken.Function.ToString() : VbLexerToken.Sub.ToString();
                        break;

                    case 11:
                        str3 = VbLexerToken.Property.ToString();
                        break;
                }
            }
            string str4 = base.JoinWithSpace(new string[] { str, str3, this.Name }) + this.GetParamText(false, false);
            if (this.DataType != string.Empty)
            {
                object obj2 = str4;
                str4 = string.Concat(new object[] { obj2, ' ', VbLexerToken.As.ToString(), ' ', this.DataType });
            }
            if (!compact)
            {
                switch (this.MemberType)
                {
                    case 10:
                    {
                        object obj3 = str4;
                        string str5 = string.Concat(new object[] { obj3, "\r\n", '\t', SyntaxConsts.DefaultCaretSymbol });
                        object obj4 = str5 + VbLexerToken.MyBase.ToString() + "." + this.Name + this.GetParamText(false, true);
                        return string.Concat(new object[] { obj4, "\r\n", VbLexerToken.End.ToString(), ' ', str3 });
                    }
                    case 11:
                    {
                        if ((this.Attributes & MemberAttribute.CanRead) != MemberAttribute.None)
                        {
                            object obj5 = str4;
                            object obj6 = string.Concat(new object[] { obj5, "\r\n", '\t', VbLexerToken.Get.ToString() });
                            object obj7 = string.Concat(new object[] { obj6, "\r\n", '\t', SyntaxConsts.DefaultCaretSymbol });
                            object obj8 = string.Concat(new object[] { obj7, VbLexerToken.Return.ToString(), ' ', VbLexerToken.MyBase, ".", this.Name, this.GetParamText(false, true) });
                            str4 = string.Concat(new object[] { obj8, "\r\n", '\t', VbLexerToken.End.ToString(), ' ', VbLexerToken.Get.ToString() });
                        }
                        if ((this.Attributes & MemberAttribute.CanWrite) != MemberAttribute.None)
                        {
                            object obj9 = str4;
                            object obj10 = string.Concat(new object[] { obj9, "\r\n", '\t', VbLexerToken.Set.ToString() });
                            object obj11 = string.Concat(new object[] { obj10, "\r\n", '\t', VbLexerToken.MyBase.ToString(), ".", this.Name, this.GetParamText(false, true), "=value" });
                            str4 = string.Concat(new object[] { obj11, "\r\n", '\t', VbLexerToken.End.ToString(), ' ', VbLexerToken.Set.ToString() });
                        }
                        object obj12 = str4;
                        return string.Concat(new object[] { obj12, "\r\n", VbLexerToken.End.ToString(), ' ', str3 });
                    }
                }
            }
            return str4;
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
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, ' ', VbLexerToken.As.ToString().ToLower(), ' ', this.DataType });
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

