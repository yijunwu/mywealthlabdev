namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Parsers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class ReflectionRepository : NETRepository, IReflectionRepository, ICodeCompletionRepository
    {
        private IList<Assembly> assemblies;
        private Hashtable internalTypes;
        private Hashtable restrictedTypes;
        private Hashtable types;

        public ReflectionRepository(bool caseSensitive, ISyntaxTree syntaxTree) : base(caseSensitive, syntaxTree)
        {
            this.assemblies = new List<Assembly>();
            this.types = new Hashtable();
            this.internalTypes = new Hashtable();
            this.restrictedTypes = new Hashtable();
        }

        public virtual void AllowTypeMembers(Type type)
        {
            if (this.restrictedTypes.Contains(type))
            {
                this.restrictedTypes.Remove(type);
            }
        }

        public virtual void ClearAssemblies()
        {
            this.assemblies.Clear();
        }

        public virtual void ClearTypes()
        {
            this.types.Clear();
            this.internalTypes.Clear();
        }

        protected override void DoFillMembers(ISyntaxNode node, Point position, IListMembers members, IList<ISyntaxNode> types, object member, string name, CodeCompletionScope scope, ref int selIndex)
        {
            base.DoFillMembers(node, position, members, types, member, name, scope, ref selIndex);
            if (member is Type)
            {
                if ((scope & CodeCompletionScope.Delegate) != CodeCompletionScope.None)
                {
                    this.FillDelegate(members, (Type) member, name);
                }
                else
                {
                    this.FillTypeMembers(members, (Type) member, scope);
                }
            }
        }

        protected virtual void FillCastExpressionMember(IListMembers members, ISyntaxNode node)
        {
            if (this.CanAddMember(members, node.Name, 10, false))
            {
                IListMember member = this.AddMember(members, node.Name, 10);
                member.Qualifier = "public";
                ISyntaxAttribute attribute = node.FindAttribute("Type");
                if (attribute != null)
                {
                    member.DataType = (string) attribute.Value;
                }
                member.Parameters = new ParameterMembers();
                IParameterMember member2 = member.Parameters.AddParameterMember();
                member2.DataType = typeof(object).Name;
                member2.Name = SyntaxParserConsts.ExpressionTag;
                if (node.NodeType != 0x97)
                {
                    member2 = member.Parameters.AddParameterMember();
                    member2.DataType = typeof(object).Name;
                    member2.Name = SyntaxParserConsts.TypeTag;
                    member.Parameters[1] = member2;
                }
            }
        }

        protected virtual void FillDelegate(IListMembers members, Type type, string name)
        {
            this.AddMember(members, string.Format(StringConsts.InsertDelegateHint, type.Name, name), -1).DisplayText = string.Format(StringConsts.InsertDelegateExtendedHint, type.Name, name);
        }

        protected override void FillGlobalTypes(IListMembers members, IList<INetNamespace> namespaces)
        {
            base.FillGlobalTypes(members, namespaces);
            IList<Type> types = new List<Type>();
            foreach (INetNamespace namespace2 in namespaces)
            {
                if (NetTypes.GetGlobalTypes(types, namespace2.Namespace, this.CaseSensitive) > 0)
                {
                    foreach (Type type in types)
                    {
                        this.FillTypeMembers(members, type, CodeCompletionScope.Static);
                    }
                }
            }
            this.FillInternalTypes(members);
        }

        protected virtual void FillInternalTypes(IListMembers members)
        {
            IDictionaryEnumerator enumerator = this.internalTypes.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                string name = (string) enumerator.Value;
                if ((name != string.Empty) && !name.StartsWith("__"))
                {
                    this.FillMember(members, (MemberInfo) ((Type) enumerator.Key), name, CodeCompletionScope.Global);
                }
            }
        }

        public override void FillMember(IListMembers members, object member, string name, CodeCompletionScope scope)
        {
            base.FillMember(members, member, name, scope);
            if (member is ISyntaxNode)
            {
                ISyntaxNode node = member as ISyntaxNode;
                if ((scope & CodeCompletionScope.Global) != CodeCompletionScope.None)
                {
                    IList<Type> types = new List<Type>();
                    foreach (INetNamespace namespace2 in this.GetNamespaces(node, false))
                    {
                        NetTypes.GetGlobalTypes(types, namespace2.Namespace, this.CaseSensitive);
                        foreach (Type type in types)
                        {
                            this.FillTypeMembers(members, type, name, scope);
                        }
                    }
                }
                if ((node.NodeType == 0x97) || (node.NodeType == 150))
                {
                    this.FillCastExpressionMember(members, node);
                }
            }
            if (member is Type)
            {
                this.FillTypeMembers(members, (Type) member, name, scope);
            }
        }

        protected virtual void FillMember(IListMembers members, MemberInfo info, string name, CodeCompletionScope scope)
        {
            MemberAttribute attribute;
            int memberIndex = this.GetMemberIndex(info, out attribute);
            string str = this.GetMemberDataType(info, false, members.UseHtmlFormatting);
            if (this.CanAddMember(members, name, memberIndex, !(info is Type)))
            {
                IListMember member = this.AddMember(members, name, memberIndex);
                member.Qualifier = this.GetMemberQualifier(info);
                member.DataType = str;
                member.Parameters = this.GetMemberParameters(info, members.UseHtmlFormatting);
                string text = this.GetDescription(null, null, info, XmlCommentType.Summary.ToString(), false);
                if (info is Type)
                {
                    member.Name = this.GetMemberDataType(info, true, members.UseHtmlFormatting);
                    member.Description = str;
                    member.AddDescription(text);
                }
                else
                {
                    member.Description = text;
                }
                member.Priority = this.GetPriority(info);
                member.Attributes = attribute;
                if ((info is Type) || ((info != null) && (info.DeclaringType == typeof(NETRepository.BoolEnum))))
                {
                    member.Attributes |= MemberAttribute.NoDescription;
                }
                if (memberIndex >= 0)
                {
                    member.ImageIndex = memberIndex + this.GetScopeIndex(info);
                }
                if ((scope & CodeCompletionScope.Overrides) != CodeCompletionScope.None)
                {
                    member.DisplayText = member.GetTemplate(true);
                }
            }
        }

        protected override void FillNamespaceTypes(IListMembers members, IList<INetNamespace> namespaces)
        {
            base.FillNamespaceTypes(members, namespaces);
            foreach (Type type in NetTypes.Types.Values)
            {
                if (((type.Namespace == null) || (type.Namespace == string.Empty)) || (this.IndexOfNamespace(namespaces, type.Namespace) >= 0))
                {
                    this.FillMember(members, (MemberInfo) type, this.GetTypeName(type, true, false, members.UseHtmlFormatting), CodeCompletionScope.Global);
                }
            }
        }

        protected override void FillNamespaceTypes(IListMembers members, INetNamespace nspace, ISyntaxNode node)
        {
            base.FillNamespaceTypes(members, nspace, node);
            string name = nspace.GetName();
            foreach (Type type in NetTypes.Types.Values)
            {
                if ((((string.Compare(type.Namespace, name, !this.CaseSensitive) == 0) && !type.IsNestedPublic) && (!type.IsNestedPrivate && !type.IsNestedAssembly)) && !type.IsNestedFamily)
                {
                    this.FillMember(members, (MemberInfo) type, type.Name, CodeCompletionScope.Global);
                }
            }
        }

        protected virtual void FillTypeMembers(IListMembers members, Type type, CodeCompletionScope scope)
        {
            BindingFlags bindingFlags = this.GetBindingFlags(scope);
            string str = (((scope & CodeCompletionScope.TypeName) != CodeCompletionScope.None) && (type != typeof(NETRepository.BoolEnum))) ? this.GetDataType(type, true) : string.Empty;
            this.FillTypeMembers(members, type.GetProperties(bindingFlags), type, str, string.Empty, scope);
            this.FillTypeMembers(members, type.GetMethods(bindingFlags), type, str, string.Empty, scope);
            this.FillTypeMembers(members, type.GetEvents(bindingFlags), type, str, string.Empty, scope);
            this.FillTypeMembers(members, type.GetFields(bindingFlags), type, str, string.Empty, scope);
            this.FillTypeMembers(members, type.GetNestedTypes(bindingFlags), type, str, string.Empty, scope);
            if (type.IsInterface)
            {
                foreach (Type type2 in type.GetInterfaces())
                {
                    this.FillTypeMembers(members, type2, scope);
                }
            }
        }

        protected virtual void FillTypeMembers(IListMembers members, Type type, string name, CodeCompletionScope scope)
        {
            switch (name)
            {
                case ".ctor":
                    this.FillTypeMembers(members, type.GetConstructors(), type, this.GetDataType(type, members.UseHtmlFormatting), scope);
                    return;

                case ".ElementAccess":
                    this.FillTypeMembers(members, type.GetDefaultMembers(), type, this.GetDataType(type, members.UseHtmlFormatting), scope);
                    return;
            }
            MemberInfo[] member = type.GetMember(name, this.GetBindingFlags(scope));
            if ((member == null) || (member.Length == 0))
            {
                foreach (Type type2 in type.GetInterfaces())
                {
                    member = type2.GetMember(name);
                    if ((member != null) && (member.Length > 0))
                    {
                        break;
                    }
                }
            }
            this.FillTypeMembers(members, member, type, ((scope & CodeCompletionScope.TypeName) != CodeCompletionScope.None) ? this.GetDataType(type, (scope & CodeCompletionScope.ShortType) != CodeCompletionScope.None, members.UseHtmlFormatting) : string.Empty, name, scope);
        }

        protected virtual void FillTypeMembers(IListMembers provider, MemberInfo[] members, Type declaringType, string name, CodeCompletionScope scope)
        {
            this.FillTypeMembers(provider, members, declaringType, string.Empty, name, scope);
        }

        protected virtual void FillTypeMembers(IListMembers provider, MemberInfo[] members, Type declaringType, string type, string name, CodeCompletionScope scope)
        {
            foreach (MemberInfo info in members)
            {
                if (((((scope & CodeCompletionScope.Overrides) == CodeCompletionScope.None) || this.IsVirtualMember(info)) && (!this.IsPrivateMember(info) || ((info.DeclaringType == declaringType) && ((scope & CodeCompletionScope.Private) != CodeCompletionScope.None)))) && (((info.DeclaringType == info.ReflectedType) || !this.IsRestrictedType(info.DeclaringType)) && ((name != string.Empty) || !this.IsSpecialName(info))))
                {
                    string str = (name != string.Empty) ? name : info.Name;
                    if ((declaringType == typeof(NETRepository.BoolEnum)) && this.CaseSensitive)
                    {
                        str = str.ToLower();
                    }
                    if (type != string.Empty)
                    {
                        str = type + "." + str;
                    }
                    this.FillMember(provider, info, str, scope);
                }
            }
        }

        protected virtual bool FindAssembly(string name, out Assembly assembly)
        {
            foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (string.Compare(assembly2.GetName().Name, name, true) == 0)
                {
                    assembly = assembly2;
                    return true;
                }
            }
            assembly = null;
            return false;
        }

        protected override object FindDeclaration(object member, IList<ISyntaxNode> types, Point position, string name, CodeCompletionScope scope, int paramCount)
        {
            object obj2 = base.FindDeclaration(member, types, position, name, scope, paramCount);
            if (obj2 == null)
            {
                if (member is ISyntaxNode)
                {
                    ISyntaxNode node = (ISyntaxNode) member;
                    if (!NETRepository.IsDeclarationNode(node) && !NETRepository.IsXmlCommentNode(node))
                    {
                        obj2 = this.FindNamespaceDeclaration(null, node, name);
                    }
                }
                if (member is Type)
                {
                    return this.FindTypeDeclaration((Type) member, name, scope);
                }
                if (member is INetNamespace)
                {
                    obj2 = this.FindNamespaceDeclaration((INetNamespace) member, null, name);
                }
            }
            return obj2;
        }

        protected override object FindMember(string text, Point position, ISyntaxNode node, ISyntaxNode refNode, string name, int paramCount)
        {
            CodeCompletionScope scope;
            object obj2 = base.FindMember(text, position, node, refNode, name, paramCount);
            if (obj2 != null)
            {
                return obj2;
            }
            string str = name;
            Point endPos = position;
            obj2 = this.GetMemberType(text, node, ref str, ref position, ref endPos, out scope);
            if (!(obj2 is Type))
            {
                goto Label_00AC;
            }
            Type type = (Type) obj2;
            MemberInfo[] member = null;
            string str2 = name;
            if (str2 != null)
            {
                if (!(str2 == ".ctor"))
                {
                    if (str2 == ".ElementAccess")
                    {
                        member = type.GetDefaultMembers();
                        goto Label_008B;
                    }
                }
                else
                {
                    member = type.GetConstructors();
                    goto Label_008B;
                }
            }
            member = type.GetMember(name, this.GetBindingFlags(scope));
        Label_008B:
            if ((member != null) && (member.Length > 0))
            {
                return member[((paramCount >= 0) && (paramCount < member.Length)) ? paramCount : 0];
            }
        Label_00AC:
            return null;
        }

        protected virtual object FindNamespaceDeclaration(INetNamespace nspace, ISyntaxNode node, string name)
        {
            string str = (nspace != null) ? (nspace.GetName() + "." + name) : name;
            foreach (INetNamespace namespace2 in this.GetNamespaces(null, true))
            {
                string str2 = namespace2.Namespace;
                if ((this.CaseSensitive ? str2.StartsWith(str) : str2.ToLower().StartsWith(str.ToLower())) && ((str2.Length == str.Length) || (str2[str.Length] == '.')))
                {
                    return new NetNamespace(str, true);
                }
            }
            return this.GetTypeByName(node, (nspace != null) ? (nspace.GetName() + "." + name) : name);
        }

        protected virtual object FindTypeDeclaration(Type type, string name, CodeCompletionScope scope)
        {
            MemberInfo[] member = type.GetMember(name, this.GetBindingFlags(scope));
            object obj2 = ((member != null) && (member.Length > 0)) ? member[0] : null;
            if ((obj2 == null) && type.IsInterface)
            {
                foreach (Type type2 in type.GetInterfaces())
                {
                    obj2 = this.FindTypeDeclaration(type2, name, scope);
                    if (obj2 != null)
                    {
                        return obj2;
                    }
                }
            }
            return obj2;
        }

        protected virtual string FixupArrayType(string name)
        {
            int index = name.IndexOf("[");
            if (index >= 0)
            {
                object registeredType = this.GetRegisteredType(name.Substring(0, index));
                if (registeredType != null)
                {
                    return (((Type) registeredType).FullName + name.Substring(index));
                }
            }
            return name;
        }

        protected BindingFlags GetBindingFlags(CodeCompletionScope scope)
        {
            BindingFlags flags = BindingFlags.Default;
            if ((scope & CodeCompletionScope.Instance) != CodeCompletionScope.None)
            {
                flags |= BindingFlags.Public | BindingFlags.Instance;
            }
            if ((scope & CodeCompletionScope.Static) != CodeCompletionScope.None)
            {
                flags |= BindingFlags.Public | BindingFlags.Static;
            }
            if ((scope & CodeCompletionScope.Protected) != CodeCompletionScope.None)
            {
                flags |= BindingFlags.NonPublic | BindingFlags.Static;
            }
            if (!this.CaseSensitive)
            {
                flags |= BindingFlags.IgnoreCase;
            }
            return flags;
        }

        protected virtual string GetDataType(Type type, bool useHtmlFormatting)
        {
            return this.GetDataType(type, false, true, false, useHtmlFormatting);
        }

        protected virtual string GetDataType(Type type, bool shortName, bool useHtmlFormatting)
        {
            return this.GetDataType(type, shortName, true, false, useHtmlFormatting);
        }

        protected virtual string GetDataType(Type type, bool shortName, bool checkShortName, bool useGenericArgs, bool useHtmlFormatting)
        {
            object obj2 = this.internalTypes[type];
            if (obj2 == null)
            {
                return this.GetTypeName(type, shortName || (checkShortName && (type.IsPrimitive || this.IsSystemType(type))), useGenericArgs, useHtmlFormatting);
            }
            return (string) obj2;
        }

        public override string GetDescription(IListMembers members, ISyntaxNode node, object member, string name, bool fullDescription)
        {
            string str = base.GetDescription(members, node, member, name, fullDescription);
            string description = string.Empty;
            if (member is MemberInfo)
            {
                description = DescriptionHelper.GetDescription((MemberInfo) member);
            }
            else if (member is System.Reflection.ParameterInfo)
            {
                description = DescriptionHelper.GetDescription((System.Reflection.ParameterInfo) member);
            }
            else if (member is INetNamespace)
            {
                description = SyntaxParserConsts.NamespaceDataType + " " + ((INetNamespace) member).Namespace;
            }
            if (str == null || str.Equals(string.Empty))
            {
                //case null:
                //case string.Empty:
                    return description;
            }
            if (description != string.Empty)
            {
                str = str + SyntaxConsts.DefaultBrTag + description;
            }
            return str;
        }

        protected override object GetElementAccessType(object member, ISyntaxNode refNode)
        {
            object elementAccessType = base.GetElementAccessType(member, refNode);
            if (elementAccessType == null)
            {
                if (member is ISyntaxNode)
                {
                    ISyntaxNode node = (ISyntaxNode) member;
                    if (NETRepository.IsDeclarationNode(node))
                    {
                        string baseType = this.GetBaseType(node);
                        if (baseType != string.Empty)
                        {
                            CodeCompletionScope scope;
                            member = this.GetMemberType(node, node.Position, null, baseType, out scope);
                        }
                    }
                }
                if (member is Type)
                {
                    elementAccessType = ((Type) member).GetElementType();
                    if (elementAccessType == null)
                    {
                        MemberInfo[] defaultMembers = ((Type) member).GetDefaultMembers();
                        if ((defaultMembers != null) && (defaultMembers.Length > 0))
                        {
                            MemberInfo[] infoArray2 = ((Type) member).GetMember(defaultMembers[0].Name);
                            if ((infoArray2 != null) && (infoArray2.Length > 0))
                            {
                                elementAccessType = this.GetMemberType(infoArray2[0]);
                            }
                        }
                    }
                }
            }
            return elementAccessType;
        }

        protected override object GetEnumType(object member)
        {
            object enumType = base.GetEnumType(member);
            if ((enumType == null) && (member is Type))
            {
                if (((Type) member).IsEnum)
                {
                    return member;
                }
                if (((Type) member).FullName == "System.Boolean")
                {
                    enumType = typeof(NETRepository.BoolEnum);
                }
            }
            return enumType;
        }

        protected override object GetExpressionType(ISyntaxNode node, ISyntaxNode refNode, Point position, string name, out CodeCompletionScope scope)
        {
            object obj2 = base.GetExpressionType(node, refNode, position, name, out scope);
            if (obj2 != null)
            {
                return obj2;
            }
            ISyntaxNode declarationNode = NETRepository.GetDeclarationNode(node);
            if (declarationNode != null)
            {
                string baseType = this.GetBaseType(declarationNode);
                if (baseType != string.Empty)
                {
                    obj2 = this.GetMemberType(declarationNode, declarationNode.Position, null, baseType, out scope);
                    if (obj2 is Type)
                    {
                        MemberInfo[] member = ((Type) obj2).GetMember(name, (BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance) | (this.CaseSensitive ? BindingFlags.Default : BindingFlags.IgnoreCase));
                        if ((member != null) && (member.Length > 0))
                        {
                            scope = CodeCompletionScope.Instance;
                            return this.GetMemberType(member[0]);
                        }
                    }
                }
            }
            return null;
        }

        protected virtual string GetMemberDataType(MemberInfo member, bool shortName, bool useHtmlFormatting)
        {
            Type memberType = this.GetMemberType(member);
            if (memberType == null)
            {
                return string.Empty;
            }
            return this.GetDataType(memberType, shortName, true, true, useHtmlFormatting);
        }

        protected virtual int GetMemberIndex(MemberInfo member, out MemberAttribute attributes)
        {
            int num = -1;
            attributes = MemberAttribute.None;
            if (member is PropertyInfo)
            {
                num = 11;
                if (((PropertyInfo) member).CanRead)
                {
                    attributes |= MemberAttribute.CanRead;
                }
                if (((PropertyInfo) member).CanWrite)
                {
                    attributes |= MemberAttribute.CanWrite;
                }
                return num;
            }
            if (member is EventInfo)
            {
                return 7;
            }
            if (member is MethodInfo)
            {
                return 10;
            }
            if (member is FieldInfo)
            {
                return 8;
            }
            if (!(member is Type))
            {
                return num;
            }
            Type type = (Type) member;
            if (!type.IsClass)
            {
                if (type.IsEnum)
                {
                    return 6;
                }
                if (type.IsInterface)
                {
                    return 9;
                }
                if (type.IsValueType)
                {
                    return 12;
                }
            }
            return 3;
        }

        protected virtual System.Reflection.ParameterInfo[] GetMemberParameterInfos(MemberInfo member)
        {
            System.Reflection.ParameterInfo[] indexParameters = null;
            try
            {
                if (member is MethodBase)
                {
                    return ((MethodBase) member).GetParameters();
                }
                if (member is PropertyInfo)
                {
                    indexParameters = ((PropertyInfo) member).GetIndexParameters();
                    if ((indexParameters != null) && (indexParameters.Length == 0))
                    {
                        indexParameters = null;
                    }
                }
            }
            catch
            {
            }
            return indexParameters;
        }

        protected virtual IParameterMembers GetMemberParameters(MemberInfo member, bool useHtmlFormatting)
        {
            System.Reflection.ParameterInfo[] memberParameterInfos = this.GetMemberParameterInfos(member);
            if (memberParameterInfos == null)
            {
                return null;
            }
            IParameterMembers members = new ParameterMembers();
            foreach (System.Reflection.ParameterInfo info in memberParameterInfos)
            {
                IParameterMember member2 = members.AddParameterMember();
                member2.Name = info.Name;
                member2.DataType = this.GetDataType(info.ParameterType, useHtmlFormatting);
                member2.Qualifier = this.GetParamQualifier(info);
                member2.Description = this.GetDescription(null, null, info, XmlCommentType.Param.ToString(), false);
                member2.Modifiers = ParameterModifer.None;
                if (info.IsOptional)
                {
                    member2.Modifiers |= ParameterModifer.Optional;
                }
                if (info.IsIn)
                {
                    member2.Modifiers |= ParameterModifer.In;
                }
                if (info.IsOut)
                {
                    member2.Modifiers |= ParameterModifer.Out;
                }
                if (info.IsRetval)
                {
                    member2.Modifiers |= ParameterModifer.Retval;
                }
            }
            return members;
        }

        protected virtual string GetMemberQualifier(MemberInfo member)
        {
            string str = string.Empty;
            if (member is Type)
            {
                return this.GetTypeQualifier((Type) member);
            }
            if (this.IsPrivateMember(member))
            {
                str = "private";
            }
            else if (this.IsProtectedMember(member))
            {
                str = "protected";
            }
            else if (this.IsPublicMember(member))
            {
                str = "public";
            }
            if (member is MethodBase)
            {
                MethodBase base2 = (MethodBase) member;
                if (base2.IsAbstract)
                {
                    str = str + " abstract";
                }
                if (base2.IsVirtual)
                {
                    str = str + " virtual";
                }
            }
            return str.Trim();
        }

        protected virtual Type GetMemberType(MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return ((PropertyInfo) member).PropertyType;
            }
            if (member is EventInfo)
            {
                return ((EventInfo) member).EventHandlerType;
            }
            if (member is MethodInfo)
            {
                return ((MethodInfo) member).ReturnType;
            }
            if (member is FieldInfo)
            {
                return ((FieldInfo) member).FieldType;
            }
            if (member is Type)
            {
                return (Type) member;
            }
            return null;
        }

        public override object GetMemberType(ISyntaxNode node, Point position, object member, string name, out CodeCompletionScope scope)
        {
            object obj2 = base.GetMemberType(node, position, member, name, out scope);
            if ((obj2 != null) || !(member is Type))
            {
                return obj2;
            }
            MemberInfo[] infoArray = ((Type) member).GetMember(name);
            if ((infoArray == null) || (infoArray.Length == 0))
            {
                foreach (Type type in ((Type) member).GetInterfaces())
                {
                    infoArray = type.GetMember(name);
                    if ((infoArray != null) && (infoArray.Length > 0))
                    {
                        break;
                    }
                }
            }
            if ((infoArray == null) || (infoArray.Length <= 0))
            {
                return obj2;
            }
            MemberInfo info = infoArray[0];
            scope = (info is Type) ? CodeCompletionScope.Static : CodeCompletionScope.Instance;
            if (info is MethodBase)
            {
                scope |= CodeCompletionScope.Method;
            }
            if (info is PropertyInfo)
            {
                scope |= CodeCompletionScope.Property;
            }
            if (info is FieldInfo)
            {
                scope |= CodeCompletionScope.Field;
            }
            return this.GetMemberType(info);
        }

        protected override IList<string> GetNamespaces()
        {
            IList<string> list = new List<string>(base.Namespaces);
            foreach (string str in NetTypes.GetNamespaces(this.assemblies))
            {
                list.Add(str);
            }
            return list;
        }

        protected override object GetParameterType(object member, ISyntaxNode refNode, int param)
        {
            object obj2 = base.GetParameterType(member, refNode, param);
            if (obj2 != null)
            {
                return obj2;
            }
            if (member is MemberInfo)
            {
                System.Reflection.ParameterInfo[] memberParameterInfos = this.GetMemberParameterInfos((MemberInfo) member);
                if ((memberParameterInfos != null) && (param < memberParameterInfos.Length))
                {
                    return memberParameterInfos[param].ParameterType;
                }
            }
            return null;
        }

        protected virtual string GetParamQualifier(System.Reflection.ParameterInfo param)
        {
            if (param.IsOut)
            {
                return "out";
            }
            if (!param.IsRetval)
            {
                return string.Empty;
            }
            return "ref";
        }

        public override int GetPriority(object member)
        {
            if (member is MemberInfo)
            {
                return DescriptionHelper.GetPriority((MemberInfo) member);
            }
            return base.GetPriority(member);
        }

        public virtual object GetRegisteredType(string name)
        {
            return this.types[this.CaseSensitive ? name : name.ToLower()];
        }

        private int GetScopeIndex(MemberInfo member)
        {
            if (this.IsPrivateMember(member))
            {
                return 10;
            }
            if (this.IsPublicMember(member))
            {
                return 30;
            }
            return 0;
        }

        public override object GetTypeByName(string type)
        {
            if ((type == null) || (type == string.Empty))
            {
                return null;
            }
            object registeredType = this.GetRegisteredType(type);
            if (registeredType == null)
            {
                return NetTypes.GetTypeByName(this.FixupArrayType(type), this.assemblies, this.CaseSensitive);
            }
            return registeredType;
        }

        protected override object GetTypeByName(ISyntaxNode node, string name)
        {
            object typeByName = this.GetTypeByName(name);
            if (typeByName == null)
            {
                foreach (INetNamespace namespace2 in this.GetNamespaces(node, false))
                {
                    typeByName = this.GetTypeByName(namespace2.GetName() + "." + name);
                    if (typeByName != null)
                    {
                        return typeByName;
                    }
                }
            }
            return typeByName;
        }

        protected virtual string GetTypeName(Type type, bool shortName, bool useGenericArgs, bool useHtmlFormatting)
        {
            string str = (shortName || (type.FullName == null)) ? type.Name : type.FullName;
            if (!type.IsGenericType || (str == null))
            {
                return str;
            }
            int index = str.IndexOf('`');
            if (index < 0)
            {
                return str;
            }
            str = str.Substring(0, index);
            string str2 = string.Empty;
            if (useGenericArgs)
            {
                foreach (Type type2 in type.GetGenericArguments())
                {
                    str2 = ((str2 == string.Empty) ? string.Empty : (str2 + ",")) + this.GetTypeName(type2, true, true, useHtmlFormatting);
                }
            }
            return (str + (useHtmlFormatting ? ("&lt;" + str2 + "&gt;") : ("<" + str2 + ">")));
        }

        protected virtual string GetTypeQualifier(Type type)
        {
            if (type.IsClass)
            {
                return SyntaxParserConsts.ClassDataType;
            }
            if (type.IsEnum)
            {
                return SyntaxParserConsts.EnumDataType;
            }
            if (type.IsInterface)
            {
                return SyntaxParserConsts.InterfaceDataType;
            }
            if (type.IsValueType)
            {
                return SyntaxParserConsts.StructDataType;
            }
            return SyntaxParserConsts.TypeDataType;
        }

        private int IndexOfString(IList<string> list, string str)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string strA = list[i];
                if (string.Compare(strA, str, !this.CaseSensitive) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        protected override bool IsEnumTypeRegistered()
        {
            return this.types.Contains("__enum");
        }

        protected virtual bool IsPrivateMember(MemberInfo member)
        {
            if (member is MethodBase)
            {
                return ((MethodBase) member).IsPrivate;
            }
            if (member is FieldInfo)
            {
                return ((FieldInfo) member).IsPrivate;
            }
            return ((member is Type) && ((Type) member).IsNotPublic);
        }

        protected virtual bool IsProtectedMember(MemberInfo member)
        {
            return (((member is MethodBase) || (member is FieldInfo)) && (!this.IsPublicMember(member) && !this.IsPrivateMember(member)));
        }

        protected virtual bool IsPublicMember(MemberInfo member)
        {
            if (member is MethodBase)
            {
                return ((MethodBase) member).IsPublic;
            }
            if (member is FieldInfo)
            {
                return ((FieldInfo) member).IsPublic;
            }
            if (member is Type)
            {
                return ((Type) member).IsPublic;
            }
            return true;
        }

        protected virtual bool IsRestrictedType(Type type)
        {
            return ((type != null) && this.restrictedTypes.Contains(type));
        }

        protected bool IsSpecialName(MemberInfo member)
        {
            if ((member.Name != null) && member.Name.StartsWith("__"))
            {
                return true;
            }
            if (member is MethodBase)
            {
                return ((MethodBase) member).IsSpecialName;
            }
            if (member is FieldInfo)
            {
                return ((FieldInfo) member).IsSpecialName;
            }
            return ((member is EventInfo) && ((EventInfo) member).IsSpecialName);
        }

        protected virtual bool IsSystemType(Type type)
        {
            if ((type == null) || (type.Assembly == null))
            {
                return false;
            }
            AssemblyName name = type.Assembly.GetName();
            return ((name != null) && (name.Name == "mscorlib"));
        }

        protected virtual bool IsVirtualMember(MemberInfo member)
        {
            if (member is MethodBase)
            {
                return ((MethodBase) member).IsVirtual;
            }
            if (member is PropertyInfo)
            {
                foreach (MethodInfo info in ((PropertyInfo) member).GetAccessors())
                {
                    if (info.IsVirtual)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void RegisterAllAssemblies()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                this.RegisterAssembly(assembly);
            }
        }

        public virtual void RegisterAssembly(Assembly assembly)
        {
            this.assemblies.Add(assembly);
            NetTypes.AddAssembly(assembly);
        }

        public virtual bool RegisterAssembly(string name)
        {
            if ((name == null) || (name == string.Empty))
            {
                return false;
            }
            Assembly assembly = null;
            if (!this.FindAssembly(name, out assembly))
            {
                FileInfo info = new FileInfo(name);
                if (info.Exists)
                {
                    assembly = Assembly.ReflectionOnlyLoadFrom(name);
                }
                else
                {
                    assembly = Assembly.Load(name);
                }
            }
            if (assembly != null)
            {
                this.RegisterAssembly(assembly);
            }
            return (assembly != null);
        }

        public virtual void RegisterDefaultAssemblies()
        {
            this.RegisterAssembly("mscorlib");
            this.RegisterAssembly("System");
            this.RegisterAssembly("System.Drawing");
            this.RegisterAssembly("System.Windows.Forms");
        }

        public virtual void RegisterType(string name, Type type)
        {
            this.RegisterType(name, type, false);
        }

        public virtual void RegisterType(string name, Type type, bool global)
        {
            this.types[this.CaseSensitive ? name : name.ToLower()] = type;
            this.internalTypes[type] = name;
            NetTypes.AddType(type, global);
            if ((global && (type.Namespace != null)) && ((type.Namespace != string.Empty) && (base.Namespaces.IndexOf(type.Namespace) < 0)))
            {
                this.RegisterNamespace(type.Namespace);
            }
        }

        public virtual void RestrictTypeMembers(Type type)
        {
            this.restrictedTypes.Add(type, type);
        }

        public virtual bool UnregisterAssembly(Assembly assembly, bool removeReferences)
        {
            int index = this.assemblies.IndexOf(assembly);
            if (index >= 0)
            {
                this.assemblies.RemoveAt(index);
                if (removeReferences)
                {
                    NetTypes.RemoveAssembly(assembly);
                    DescriptionHelper.UnloadAssembly(assembly);
                }
            }
            return (index >= 0);
        }

        public virtual bool UnregisterAssembly(string name, bool removeReferences)
        {
            Assembly assembly;
            if (!this.FindAssembly(name, out assembly))
            {
                return false;
            }
            return this.UnregisterAssembly(assembly, removeReferences);
        }

        public virtual bool UnregisterType(string name)
        {
            if (this.internalTypes.Contains(name))
            {
                this.internalTypes.Remove(name);
            }
            name = this.CaseSensitive ? name : name.ToLower();
            object obj2 = this.types[name];
            if (obj2 != null)
            {
                this.types.Remove(name);
                NetTypes.RemoveType((Type) obj2);
            }
            return (obj2 != null);
        }

        public virtual IList<Assembly> Assemblies
        {
            get
            {
                return this.assemblies;
            }
        }

        public virtual Hashtable Types
        {
            get
            {
                return this.types;
            }
        }
    }
}

