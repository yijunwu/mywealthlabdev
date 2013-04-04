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
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class NETRepository : CodeCompletionRepository
    {
        private bool allowGlobalMembers;
        private string baseClassType;
        protected const int cImageDelta = 10;
        private Hashtable codeSnippets;
        private ImageList internalImages;
        private Hashtable internalObjects;
        private IComparer<INetNamespace> namespaceComparer;
        private IList<string> namespaces;
        private Hashtable objects;

        public NETRepository(bool caseSensitive, ISyntaxTree syntaxTree) : base(caseSensitive, syntaxTree)
        {
            this.allowGlobalMembers = true;
            this.baseClassType = "object";
            this.objects = new Hashtable();
            this.internalObjects = new Hashtable();
            this.namespaces = new List<string>();
            this.codeSnippets = new Hashtable();
            this.namespaceComparer = new NamespaceComparer(this);
            this.internalImages = new ImageList();
            this.internalImages.ImageSize = new Size(15, 15);
            try
            {
                ResourceManager manager = new ResourceManager(typeof(NetSyntaxParser));
                this.internalImages.ImageStream = (ImageListStreamer) manager.GetObject("NETImages.ImageStream");
            }
            catch
            {
            }
            this.internalImages.TransparentColor = SyntaxConsts.DefaultTransparentColor;
        }

        protected virtual void AddNetNamespace(IList<INetNamespace> namespaces, ISyntaxNode node)
        {
            if (this.IndexOfNamespace(namespaces, node.Name) < 0)
            {
                INetNamespace item = new NetNamespace(node.Name, false);
                ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.UsingAlias.ToString());
                if (attribute != null)
                {
                    item.Alias = (string) attribute.Value;
                }
                namespaces.Add(item);
            }
        }

        protected static void AddUndoOperation(string text, Point range, Point range1, Point range2, string delimiter, IList<ITextUndo> operations)
        {
            int num;
            int num2;
            if (CanModify(text, range1, range2, delimiter, out num, out num2) && ((num2 > 0) || (delimiter != string.Empty)))
            {
                operations.Add(new TextUndo(num, num2, delimiter));
            }
        }

        protected virtual void AddUnitMember(IListMembers members, ISyntaxNode node, string filter, CodeCompletionScope scope)
        {
            if (IsDeclarationNode(node) && (node.NodeType != 7))
            {
                this.FillMember(members, node, filter, scope);
            }
            int nodeType = node.NodeType;
            if ((((nodeType == 1) || (nodeType == 7)) || (nodeType == 40)) && node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    this.AddUnitMember(members, node2, filter, scope);
                }
            }
        }

        protected virtual void AddUnitNamespaces(ISyntaxNode node, IList<INetNamespace> namespaces)
        {
            switch (node.NodeType)
            {
                case 1:
                case 40:
                    if (node.HasChildren)
                    {
                        foreach (ISyntaxNode node2 in node.ChildList)
                        {
                            this.AddUnitNamespaces(node2, namespaces);
                        }
                    }
                    return;

                case 7:
                    this.AddNetNamespace(namespaces, node);
                    return;
            }
        }

        protected virtual bool CanFilter(IListMembers members, ISyntaxNode node, string filter)
        {
            bool flag = node.NodeType == 0x12;
            bool flag2 = node.NodeType == 0x19;
            if (((node.Options & SyntaxNodeOptions.CodeCompletion) == SyntaxNodeOptions.None) && ((!flag && !flag2) || ((members != null) && !this.ShouldDuplicate(members))))
            {
                return false;
            }
            if (((filter != string.Empty) && (!flag || (filter != ".ctor"))) && (!flag2 || (filter != ".ElementAccess")))
            {
                return (string.Compare(node.Name, filter, !this.CaseSensitive) == 0);
            }
            return true;
        }

        protected static bool CanModify(string text, Point range1, Point range2, string delimiter, out int start, out int len)
        {
            if ((range1.X != range1.Y) && (range2.X != range2.Y))
            {
                start = range1.Y;
                while ((start > 0) && ((text[start - 1] == ' ') || (text[start - 1] == '\t')))
                {
                    start--;
                }
                len = text.Length;
                int x = range2.X;
                while ((x < len) && ((text[x] == ' ') || (text[x] == '\t')))
                {
                    x++;
                }
                len = x - start;
                return ((len >= 0) && (text.Substring(start, len) != delimiter));
            }
            start = 0;
            len = 0;
            return false;
        }

        public virtual void ClearNamespaces()
        {
            this.namespaces.Clear();
        }

        public virtual void ClearObjects()
        {
            this.objects.Clear();
            this.internalObjects.Clear();
        }

        private int CompareNamespaces(INetNamespace x, INetNamespace y)
        {
            return string.Compare(x.Namespace, y.Namespace, !this.CaseSensitive);
        }

        protected static bool ContainsAlpha(string text)
        {
            foreach (char ch in text)
            {
                if (char.IsLetter(ch))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void DoFillMembers(ISyntaxNode node, Point position, IListMembers members, IList<ISyntaxNode> types, object member, string name, CodeCompletionScope scope, ref int selIndex)
        {
            base.DoFillMembers(node, position, members, types, member, name, scope, ref selIndex);
            if (member is ISyntaxNode)
            {
                ISyntaxNode node2 = (ISyntaxNode) member;
                this.FillMembers(members, types, node2, position, string.Empty, scope, ref selIndex);
                if (!this.SkipGlobalMembers(node2) && !IsXmlCommentNode(node2))
                {
                    this.FillMembers(members, this.GetNamespaces(node2, true));
                    IList<INetNamespace> namespaces = this.GetNamespaces(node2, false);
                    this.FillNamespaceTypes(members, namespaces);
                    this.FillGlobalTypes(members, namespaces);
                    this.FillGlobalObjects(members);
                    this.FillSnippets(members);
                    foreach (ISyntaxTree tree in this.SyntaxTrees)
                    {
                        this.AddUnitMember(members, tree.Root, string.Empty, scope);
                    }
                }
            }
            else if (member is INetNamespace)
            {
                this.FillMembers(members, (INetNamespace) member, node, scope);
            }
            members.Images = this.internalImages;
        }

        protected override object DoFindDeclaration(string text, ISyntaxNode node, ISyntaxNode refNode, Point position, int paramCount)
        {
            if (!this.IsReferenceType(node))
            {
                return null;
            }
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.MemberAccessExpression:
                case NetNodeType.PointerMemberAccessExpression:
                {
                    CodeCompletionScope scope;
                    IList<ISyntaxNode> list = new List<ISyntaxNode>();
                    string name = string.Empty;
                    Point endPos = new Point(-1, -1);
                    object member = this.GetMemberType(text, node, ref name, ref position, ref endPos, out scope);
                    if (member != null)
                    {
                        return this.FindDeclaration(member, list, position, name, scope, paramCount);
                    }
                    return null;
                }
                case NetNodeType.PrimaryExpression:
                {
                    IList<ISyntaxNode> list2 = new List<ISyntaxNode>();
                    return this.FindDeclaration(node, list2, position, node.Name, CodeCompletionScope.Private | CodeCompletionScope.Instance | CodeCompletionScope.Static, paramCount);
                }
            }
            if ((position.Y == node.Position.Y) && (position.X > node.Position.X))
            {
                string baseType;
                string subname = string.Empty;
                Point point2 = position;
                bool isArray = false;
                if (node.NodeType == 3)
                {
                    baseType = node.Name;
                    point2 = node.Position;
                }
                else
                {
                    baseType = this.GetNodeDataType(node, refNode, ref point2, ref isArray);
                    if ((baseType == "") && IsDeclarationNode(node))
                    {
                        baseType = this.GetBaseType(node, ref point2);
                    }
                }
                if ((baseType != string.Empty) && this.GetQualifiedName(ref baseType, ref subname, ref point2, position, true))
                {
                    CodeCompletionScope scope2;
                    return this.GetQualifiedType(node, position, baseType, out scope2, isArray);
                }
                if (IsDeclarationReference(node, true))
                {
                    return null;
                }
            }
            IList<ISyntaxNode> types = new List<ISyntaxNode>();
            return this.FindDeclaration(node, types, position, node.Name, CodeCompletionScope.Private | CodeCompletionScope.Instance | CodeCompletionScope.Static, paramCount);
        }

        protected virtual void FillBaseTypeMembers(IListMembers members, IList<ISyntaxNode> types, ISyntaxNode node, string filter, CodeCompletionScope scope, ref int selIndex)
        {
            string baseType = this.GetBaseType(node);
            if (baseType != string.Empty)
            {
                CodeCompletionScope scope2;
                object member = this.GetMemberType(node, node.Position, null, baseType, out scope2);
                if (member != null)
                {
                    if (filter == string.Empty)
                    {
                        this.DoFillMembers(node, node.Position, members, types, member, filter, (scope & ~CodeCompletionScope.Private) | CodeCompletionScope.BaseType, ref selIndex);
                    }
                    else
                    {
                        this.FillMember(members, member, filter, (scope & ~CodeCompletionScope.Private) | CodeCompletionScope.BaseType);
                    }
                }
            }
        }

        protected virtual void FillDeclarations(IListMembers members, ISyntaxNode node, Point position, string filter, CodeCompletionScope scope)
        {
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    if (this.IsEmbeddedDeclaration(node2))
                    {
                        this.FillMembers(members, node2, position, filter, scope, true);
                    }
                }
            }
        }

        protected virtual void FillGlobalObject(IListMembers members, string name)
        {
            if (this.CanAddMember(members, name, 8, false))
            {
                IListMember member1 = this.AddMember(members, name, 8);
                member1.ImageIndex += 30;
            }
        }

        protected virtual void FillGlobalObjects(IListMembers members)
        {
            IDictionaryEnumerator enumerator = this.internalObjects.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                this.FillGlobalObject(members, (string) enumerator.Key);
            }
        }

        protected virtual void FillGlobalTypes(IListMembers members, IList<INetNamespace> namespaces)
        {
        }

        protected virtual void FillMember(IListMembers members, INetNamespace nspace)
        {
            string name = nspace.Namespace;
            int index = name.IndexOf('.');
            if (index >= 0)
            {
                name = name.Substring(0, index);
            }
            if (this.CanAddMember(members, name, 2, false))
            {
                this.AddMember(members, name, 2).DataType = SyntaxParserConsts.NamespaceDataType;
            }
        }

        protected virtual void FillMember(IListMembers members, ISyntaxNode node, string filter, CodeCompletionScope scope)
        {
            if (this.CanFilter(members, node, filter) && this.IsScopeValid(scope, node))
            {
                MemberAttribute attribute;
                string str2;
                string nodeName = this.GetNodeName(node, scope);
                if (((str2 = filter) != null) && ((str2 == ".ctor") || (str2 == ".ElementAccess")))
                {
                    ISyntaxNode declarationNode = GetDeclarationNode(node);
                    nodeName = (declarationNode != null) ? declarationNode.Name : node.Name;
                }
                int memberIndex = this.GetMemberIndex(node, out attribute);
                if (this.CanAddMember(members, nodeName, memberIndex, !IsDeclarationNode(node)))
                {
                    IListMember member = this.AddMember(members, nodeName, memberIndex);
                    if (memberIndex >= 0)
                    {
                        member.ImageIndex = memberIndex + this.GetScopeImageIndex(node);
                    }
                    member.Qualifier = this.GetNodeQualifier(node);
                    member.DataType = this.GetNodeDataType(node, null);
                    member.Parameters = this.GetNodeParameters(node, null);
                    member.Description = this.GetDescription(members, node, node, XmlCommentType.Summary.ToString(), false);
                    member.Priority = this.GetPriority(node);
                    member.Attributes = attribute;
                    if ((scope & CodeCompletionScope.Overrides) != CodeCompletionScope.None)
                    {
                        member.DisplayText = member.GetTemplate(true);
                    }
                }
            }
        }

        public override void FillMember(IListMembers members, object member, string name, CodeCompletionScope scope)
        {
            base.FillMember(members, member, name, scope);
            if (member is ISyntaxNode)
            {
                IList<ISyntaxNode> types = new List<ISyntaxNode>();
                int selIndex = -1;
                this.FillMembers(members, types, (ISyntaxNode) member, new Point(0x7fffffff, 0x7fffffff), name, scope, ref selIndex);
            }
        }

        public override void FillMember(IListMembers members, object member, string name, int paramIndex, CodeCompletionScope scope)
        {
            base.FillMember(members, member, name, paramIndex, scope);
        }

        protected virtual void FillMembers(IListMembers members, IList<INetNamespace> namespaces)
        {
            IList<INetNamespace> list = new List<INetNamespace>(namespaces);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                string str = list[i].Namespace;
                int index = str.IndexOf(".");
                if (index >= 0)
                {
                    str = str.Substring(0, index);
                }
                for (int j = 0; j < i; j++)
                {
                    string str2 = list[j].Namespace;
                    if (this.CaseSensitive ? str.StartsWith(str2) : str.ToLower().StartsWith(str2.ToLower()))
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
            foreach (INetNamespace namespace2 in list)
            {
                this.FillMember(members, namespace2);
            }
        }

        protected virtual void FillMembers(IListMembers members, INetNamespace nspace, ISyntaxNode node, CodeCompletionScope scope)
        {
            string str = nspace.GetName() + ".";
            foreach (INetNamespace namespace2 in this.GetNamespaces(nspace.System ? null : node, true))
            {
                string str2 = namespace2.Namespace;
                if (this.CaseSensitive ? str2.StartsWith(str) : str2.ToLower().StartsWith(str.ToLower()))
                {
                    string name = str2.Substring(str.Length);
                    int index = name.IndexOf('.');
                    if (index >= 0)
                    {
                        name = name.Substring(0, index);
                    }
                    if ((name.Trim() != string.Empty) && (members.IndexOfName(name, this.CaseSensitive) < 0))
                    {
                        this.FillMember(members, new NetNamespace(name, nspace.System));
                    }
                }
            }
            if ((scope != CodeCompletionScope.None) && (this.SyntaxTree != null))
            {
                this.FillNamespaceTypes(members, nspace, this.SyntaxTree.Root);
                foreach (ISyntaxTree tree in this.SyntaxTrees)
                {
                    this.FillNamespaceTypes(members, nspace, tree.Root);
                }
            }
        }

        protected virtual void FillMembers(IListMembers members, ISyntaxNode node, Point position, string filter, CodeCompletionScope scope, bool recursive)
        {
            this.FillMember(members, node, filter, scope);
            if ((recursive || (node.NodeType == 40)) && node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    if ((node2.FindAttribute(SyntaxConsts.BlockScope) == null) && ((node2.Position.Y < position.Y) || ((node2.Position.Y == position.Y) && (node2.Position.X < position.X))))
                    {
                        this.FillMembers(members, node2, position, filter, scope, recursive);
                    }
                }
            }
        }

        protected virtual void FillMembers(IListMembers members, IList<ISyntaxNode> types, ISyntaxNode node, Point position, string filter, CodeCompletionScope scope, ref int selIndex)
        {
            if (IsXmlCommentNode(node))
            {
                this.FillXmlNodeMembers(members, node, position, filter, ref selIndex);
            }
            else
            {
                ISyntaxNode declarationNode = GetDeclarationNode(node);
                ISyntaxNode node3 = node;
                while (node != null)
                {
                    if (IsDeclarationNode(node) || (node.NodeType == 1))
                    {
                        break;
                    }
                    if (node.NodeType != 40)
                    {
                        this.FillMembers(members, node, position, filter, scope, false);
                        this.FillDeclarations(members, node, position, filter, scope);
                    }
                    node3 = node;
                    node = node.Parent;
                }
                while (declarationNode != null)
                {
                    if (types.IndexOf(declarationNode) < 0)
                    {
                        types.Add(declarationNode);
                        if (declarationNode.HasChildren)
                        {
                            bool flag = IsBlockNode(declarationNode);
                            foreach (ISyntaxNode node4 in declarationNode.ChildList)
                            {
                                if ((node4 != node3) && ((!flag || (node4.Position.Y < position.Y)) || ((node4.Position.Y == position.Y) && (node4.Position.X < position.X))))
                                {
                                    this.FillMembers(members, node4, new Point(0x7fffffff, 0x7fffffff), filter, scope, false);
                                }
                            }
                        }
                        if (this.IsNodePartial(declarationNode))
                        {
                            foreach (ISyntaxNode node5 in this.GetSameNodes(declarationNode))
                            {
                                if (((node5 != declarationNode) && this.IsNodePartial(node5)) && node5.HasChildren)
                                {
                                    foreach (ISyntaxNode node6 in node5.ChildList)
                                    {
                                        this.FillMembers(members, node6, new Point(0x7fffffff, 0x7fffffff), filter, scope, false);
                                    }
                                    this.FillBaseTypeMembers(members, types, node5, filter, scope, ref selIndex);
                                }
                            }
                            foreach (ISyntaxTree tree in this.SyntaxTrees)
                            {
                                ISyntaxNode declaration = this.GetDeclaration(tree, declarationNode);
                                if (((declaration != null) && (declaration != declarationNode)) && (this.IsNodePartial(declaration) && declaration.HasChildren))
                                {
                                    foreach (ISyntaxNode node8 in declaration.ChildList)
                                    {
                                        this.FillMembers(members, node8, new Point(0x7fffffff, 0x7fffffff), filter, scope, false);
                                    }
                                    this.FillBaseTypeMembers(members, types, declaration, filter, scope, ref selIndex);
                                }
                            }
                        }
                        this.FillBaseTypeMembers(members, types, declarationNode, filter, scope, ref selIndex);
                    }
                    if (node == declarationNode)
                    {
                        return;
                    }
                    node3 = null;
                    declarationNode = declarationNode.Parent;
                }
            }
        }

        protected virtual void FillNamespaceTypes(IListMembers members, IList<INetNamespace> namespaces)
        {
            foreach (INetNamespace namespace2 in namespaces)
            {
                ISyntaxNode namespaceNode;
                if (this.SyntaxTree != null)
                {
                    namespaceNode = this.GetNamespaceNode(this.SyntaxTree.Root, namespace2.Namespace);
                    if (namespaceNode != null)
                    {
                        this.FillNamespaceTypes(members, namespaceNode, true);
                    }
                }
                foreach (ISyntaxTree tree in this.SyntaxTrees)
                {
                    namespaceNode = this.GetNamespaceNode(tree.Root, namespace2.Namespace);
                    if (namespaceNode != null)
                    {
                        this.FillNamespaceTypes(members, namespaceNode, true);
                    }
                }
            }
        }

        protected virtual void FillNamespaceTypes(IListMembers members, INetNamespace nspace, ISyntaxNode node)
        {
            if (node != null)
            {
                ISyntaxNode namespaceNode = this.GetNamespaceNode(node, nspace.Namespace);
                if (namespaceNode != null)
                {
                    this.FillNamespaceTypes(members, namespaceNode, true);
                }
            }
        }

        protected virtual void FillNamespaceTypes(IListMembers members, ISyntaxNode node, bool recursive)
        {
            if (recursive)
            {
                if (node.HasChildren)
                {
                    foreach (ISyntaxNode node2 in node.ChildList)
                    {
                        this.FillNamespaceTypes(members, node2, false);
                    }
                }
            }
            else
            {
                if (IsDeclarationNode(node))
                {
                    this.FillMember(members, node, string.Empty, CodeCompletionScope.Instance | CodeCompletionScope.Static);
                }
                if ((node.NodeType == 40) && node.HasChildren)
                {
                    foreach (ISyntaxNode node3 in node.ChildList)
                    {
                        this.FillNamespaceTypes(members, node3, false);
                    }
                }
            }
        }

        protected virtual void FillSnippet(IListMembers members, string name, int index)
        {
            if (this.CanAddMember(members, name, index, false))
            {
                this.AddMember(members, name, index);
            }
        }

        protected virtual void FillSnippets(IListMembers members)
        {
            IDictionaryEnumerator enumerator = this.Snippets.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                this.FillSnippet(members, (string) enumerator.Key, ((bool) enumerator.Value) ? 0x2b : 0x2c);
            }
        }

        protected virtual void FillXmlNodeMembers(IListMembers members, ISyntaxNode node, Point position, string filter, ref int selIndex)
        {
            if (node != null)
            {
                ISyntaxAttribute attribute = node.FindAttribute(XmlLexerToken.CloseTag.ToString());
                if (attribute != null)
                {
                    Point point3 = new Point(attribute.Position.X + 1, attribute.Position.Y);
                    if (point3.Equals(position))
                    {
                        if (node.FindAttribute(XmlLexerToken.OpenEndTag.ToString()) == null)
                        {
                            string name = node.Name;
                            if (((filter == string.Empty) || (string.Compare(filter, name, !this.CaseSensitive) == 0)) && this.CanAddMember(members, name, 40, false))
                            {
                                IListMember member = this.AddMember(members, "|" + string.Format(members.UseHtmlFormatting ? SyntaxParserConsts.XmlFormatTag : SyntaxParserConsts.XmlTag, name), 40);
                                selIndex = members.Count - 1;
                                member.DisplayText = name;
                            }
                        }
                        return;
                    }
                }
            }
            foreach (string str2 in Enum.GetNames(typeof(XmlCommentType)))
            {
                string strB = str2.ToLower();
                if (((filter == string.Empty) || (string.Compare(filter, strB, !this.CaseSensitive) == 0)) && this.CanAddMember(members, strB, 40, false))
                {
                    this.AddMember(members, strB, 40).DisplayText = strB;
                }
            }
        }

        protected virtual object FindDeclaration(ISyntaxNode node, Point position, string name, CodeCompletionScope scope, bool recursive)
        {
            if (((name != string.Empty) && this.CanFilter(null, node, name)) && this.IsScopeValid(scope, node))
            {
                return node;
            }
            if ((recursive || (node.NodeType == 40)) && node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    if ((node2.FindAttribute(SyntaxConsts.BlockScope) == null) && ((node2.Position.Y < position.Y) || ((node2.Position.Y == position.Y) && (node2.Position.X < position.X))))
                    {
                        object obj2 = this.FindDeclaration(node2, position, name, scope, recursive);
                        if (obj2 != null)
                        {
                            return obj2;
                        }
                    }
                }
            }
            return null;
        }

        protected virtual object FindDeclaration(object member, IList<ISyntaxNode> types, Point position, string name, CodeCompletionScope scope, int paramCount)
        {
            object obj2 = null;
            ISyntaxNode parent = (member is ISyntaxNode) ? ((ISyntaxNode) member) : null;
            if (parent != null)
            {
                ISyntaxNode declarationNode = GetDeclarationNode(parent);
                ISyntaxNode node3 = parent;
                while (parent != null)
                {
                    if (IsDeclarationNode(parent))
                    {
                        break;
                    }
                    obj2 = this.FindDeclaration(parent, position, name, scope, false);
                    if (obj2 != null)
                    {
                        return obj2;
                    }
                    obj2 = this.FindDeclarations(parent, position, name, scope);
                    if (obj2 != null)
                    {
                        return obj2;
                    }
                    node3 = parent;
                    parent = parent.Parent;
                }
                while (declarationNode != null)
                {
                    if (types.IndexOf(declarationNode) < 0)
                    {
                        types.Add(declarationNode);
                        if (declarationNode.HasChildren)
                        {
                            bool flag = IsBlockNode(declarationNode);
                            foreach (ISyntaxNode node4 in declarationNode.ChildList)
                            {
                                if ((node4 != node3) && ((!flag || (node4.Position.Y < position.Y)) || ((node4.Position.Y == position.Y) && (node4.Position.X < position.X))))
                                {
                                    object obj3 = this.FindDeclaration(node4, new Point(0x7fffffff, 0x7fffffff), name, scope, false);
                                    if (obj3 != null)
                                    {
                                        obj2 = obj3;
                                        paramCount--;
                                        if (paramCount <= 0)
                                        {
                                            return obj2;
                                        }
                                    }
                                }
                            }
                            if (obj2 != null)
                            {
                                return obj2;
                            }
                        }
                        string baseType = this.GetBaseType(declarationNode);
                        if (baseType != string.Empty)
                        {
                            CodeCompletionScope scope2;
                            object obj4 = this.GetMemberType(declarationNode, declarationNode.Position, null, baseType, out scope2);
                            if (obj4 != null)
                            {
                                obj2 = this.FindDeclaration(obj4, types, new Point(0x7fffffff, 0x7fffffff), name, scope & ~CodeCompletionScope.Private, paramCount);
                                if (obj2 != null)
                                {
                                    return obj2;
                                }
                            }
                        }
                    }
                    if (parent == declarationNode)
                    {
                        break;
                    }
                    node3 = null;
                    declarationNode = declarationNode.Parent;
                }
                foreach (ISyntaxTree tree in this.SyntaxTrees)
                {
                    obj2 = this.FindUnitMember(tree.Root, name, scope);
                }
            }
            return obj2;
        }

        protected virtual object FindDeclarations(ISyntaxNode node, Point position, string name, CodeCompletionScope scope)
        {
            object obj2 = null;
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    if (this.IsEmbeddedDeclaration(node2))
                    {
                        obj2 = this.FindDeclaration(node2, position, name, scope, true);
                        if (obj2 != null)
                        {
                            return obj2;
                        }
                    }
                }
            }
            return obj2;
        }

        protected virtual object FindMember(string text, Point position, ISyntaxNode node, ISyntaxNode refNode, string name, int paramCount)
        {
            return this.DoFindDeclaration(text, node, refNode, position, paramCount);
        }

        public override int FindReferences(ISyntaxNode node, ISyntaxNodes references)
        {
            base.FindReferences(node, references);
            ISyntaxNode blockOrMethodNode = GetBlockOrMethodNode(node);
            Point position = node.Position;
            if (blockOrMethodNode == null)
            {
                position = Point.Empty;
                blockOrMethodNode = this.SyntaxTree.Root;
            }
            this.FindReferences(node, references, blockOrMethodNode, position);
            return references.Count;
        }

        protected virtual void FindReferences(ISyntaxNode node, ISyntaxNodes references, ISyntaxNode root, Point position)
        {
            if ((string.Compare(root.Name, node.Name, !this.CaseSensitive) == 0) && (this.FindDeclaration(null, root, root.Position) == node))
            {
                references.Add(root);
            }
            if (root.HasChildren)
            {
                foreach (ISyntaxNode node2 in root.ChildList)
                {
                    if ((node2.Position.Y > position.Y) || ((node2.Position.Y == position.Y) && (node2.Position.X >= position.X)))
                    {
                        this.FindReferences(node, references, node2, position);
                    }
                }
            }
        }

        protected virtual ISyntaxNode FindUnitMember(ISyntaxNode node, string name, CodeCompletionScope scope)
        {
            ISyntaxNode node2 = null;
            if ((IsDeclarationNode(node) && (name != string.Empty)) && (this.CanFilter(null, node, name) && this.IsScopeValid(scope, node)))
            {
                return node;
            }
            int nodeType = node.NodeType;
            if ((((nodeType == 1) || (nodeType == 7)) || (nodeType == 40)) && node.HasChildren)
            {
                foreach (ISyntaxNode node3 in node.ChildList)
                {
                    node2 = this.FindUnitMember(node3, name, scope);
                    if (node2 != null)
                    {
                        return node2;
                    }
                }
            }
            return null;
        }

        protected virtual ISyntaxNode FindUsingList(ISyntaxNode node)
        {
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    if (node2.NodeType == 2)
                    {
                        return node2;
                    }
                    if (node2.NodeType == 40)
                    {
                        ISyntaxNode node3 = this.FindUsingList(node2);
                        if (node3 != null)
                        {
                            return node3;
                        }
                    }
                }
            }
            return null;
        }

        protected ISyntaxNode FindXmlNode(ISyntaxNode node, string tagName, string paramName, string paramValue)
        {
            ISyntaxNode node2 = null;
            if ((node.NodeType == 0x3b) && (string.Compare(tagName, node.Name, true) == 0))
            {
                if (paramName != string.Empty)
                {
                    ISyntaxNode node3 = node.FindNode(0x3e);
                    if ((node3 != null) && node3.HasChildren)
                    {
                        foreach (ISyntaxNode node4 in node3.ChildList)
                        {
                            if (string.Compare(node4.Name, paramName, true) == 0)
                            {
                                ISyntaxAttribute attribute = node4.FindAttribute(NetNodeType.XmlParameter.ToString());
                                if (((attribute != null) && (attribute.Value is string)) && (paramValue == this.RemoveQuotes(((string) attribute.Value).Trim())))
                                {
                                    node2 = node;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    node2 = node;
                }
            }
            if ((node2 == null) && node.HasChildren)
            {
                foreach (ISyntaxNode node5 in node.ChildList)
                {
                    node2 = this.FindXmlNode(node5, tagName, paramName, paramValue);
                    if (node2 != null)
                    {
                        return node2;
                    }
                }
            }
            return node2;
        }

        protected static void FormatArgumentList(ISyntaxNode node, int index, string text, ITextUndoList operations)
        {
            if (!node.ContainsErrors())
            {
                Point point;
                Point point2;
                Point point3;
                int length = text.Length;
                if (node.HasChildren)
                {
                    int childCount = node.ChildCount;
                    ISyntaxNode node2 = node.ChildList[0];
                    point = GetTextRange(index, length, new QWhale.Common.Range(node.Position, node2.Range.EndPoint));
                    point2 = GetTextRange(index, length, new QWhale.Common.Range(node.Position, node2.Position));
                    point3 = GetTextRange(index, length, node2.Range);
                    AddUndoOperation(text, point, point2, point3, string.Empty, operations);
                    for (int i = 0; i < (childCount - 1); i++)
                    {
                        node2 = node.ChildList[i];
                        ISyntaxNode node3 = node.ChildList[i + 1];
                        point = GetTextRange(index, length, new QWhale.Common.Range(node2.Position, node2.Range.EndPoint));
                        point2 = GetTextRange(index, length, node2.Range);
                        point3 = GetTextRange(index, length, node3.Range);
                        Point point4 = GetTextRange(index, length, new QWhale.Common.Range(node2.Range.EndPoint, node3.Position));
                        AddUndoOperation(text, point, point2, point4, string.Empty, operations);
                        AddUndoOperation(text, point, point4, point3, ' '.ToString(), operations);
                    }
                    node2 = node.ChildList[childCount - 1];
                    point = GetTextRange(index, length, new QWhale.Common.Range(node2.Position, node.Range.EndPoint));
                    point2 = GetTextRange(index, length, node2.Range);
                    point3 = GetTextRange(index, length, new QWhale.Common.Range(node2.Range.EndPoint, node.Range.EndPoint));
                    AddUndoOperation(text, point, point2, point3, string.Empty, operations);
                }
                else
                {
                    point = GetTextRange(index, length, node.Range);
                    point2 = GetTextRange(index, length, new QWhale.Common.Range(node.Position, new Point(node.Position.X + 1, node.Position.Y)));
                    point3 = GetTextRange(index, length, new QWhale.Common.Range(new Point(Math.Max(node.Range.EndPoint.X - 1, 0), node.Range.EndPoint.Y), node.Range.EndPoint));
                    AddUndoOperation(text, point, point2, point3, string.Empty, operations);
                }
            }
        }

        protected static void FormatBinaryExpression(ISyntaxNode node, int index, string text, ITextUndoList operations)
        {
            int childCount = node.ChildCount;
            if (((childCount == 2) || (childCount == 3)) && !node.ContainsErrors())
            {
                int num1 = node.ChildCount;
                int length = text.Length;
                Point range = GetTextRange(index, length, node.Range);
                Point point2 = GetTextRange(index, length, node.ChildList[0].Range);
                Point point3 = GetTextRange(index, length, node.ChildList[1].Range);
                ISyntaxAttribute[] attributeArray = node.FindAttributes(NetNodeType.Expression.ToString());
                if ((attributeArray != null) && (attributeArray.Length != 0))
                {
                    ISyntaxAttribute attribute = attributeArray[0];
                    Point point4 = GetTextRange(index, length, attribute.Range);
                    AddUndoOperation(text, range, point2, point4, ' '.ToString(), operations);
                    AddUndoOperation(text, range, point4, point3, ' '.ToString(), operations);
                }
                if ((childCount == 3) && (attributeArray.Length >= 2))
                {
                    Point point5 = GetTextRange(index, length, node.ChildList[2].Range);
                    ISyntaxAttribute attribute2 = attributeArray[1];
                    Point point6 = GetTextRange(index, length, attribute2.Range);
                    AddUndoOperation(text, range, point3, point6, ' '.ToString(), operations);
                    AddUndoOperation(text, range, point6, point5, ' '.ToString(), operations);
                }
            }
        }

        protected static void FormatDeclaration(ISyntaxNode node, int index, string text, ITextUndoList operations)
        {
            if (!node.ContainsErrors())
            {
                ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.TypeList.ToString());
                if (attribute != null)
                {
                    ISyntaxAttribute attribute2 = node.FindAttribute(NetNodeType.Name.ToString());
                    if (attribute2 != null)
                    {
                        Point endPoint = (node.HasChildren && (node.ChildList[0].NodeType == 0x35)) ? node.ChildList[0].Range.EndPoint : attribute.EndPosition;
                        int length = text.Length;
                        Point range = GetTextRange(index, length, new QWhale.Common.Range(attribute2.Position, endPoint));
                        Point endPosition = attribute2.EndPosition;
                        Point point4 = GetTextRange(index, length, attribute2.Range);
                        Point point5 = GetTextRange(index, length, attribute.Range);
                        endPosition = GetTextRange(index, length, new QWhale.Common.Range(endPosition, attribute.Position));
                        AddUndoOperation(text, range, point4, attribute2.EndPosition, ' '.ToString(), operations);
                        AddUndoOperation(text, range, endPosition, point5, ' '.ToString(), operations);
                    }
                }
            }
        }

        protected static void FormatExpressionStatement(ISyntaxNode node, int index, string text, ITextUndoList operations)
        {
            if (node.HasChildren && !node.ContainsErrors())
            {
                ISyntaxNode node2 = node.ChildList[0];
                if ((node2.NodeType == 0x21) || IsExpressionNode(node2))
                {
                    int length = text.Length;
                    Point range = GetTextRange(index, length, node.Range);
                    Point point2 = GetTextRange(index, length, node2.Range);
                    Point point3 = GetTextRange(index, length, new QWhale.Common.Range(node2.Range.EndPoint, node.Range.EndPoint));
                    AddUndoOperation(text, range, point2, point3, string.Empty, operations);
                }
            }
        }

        protected static void FormatUnaryExpression(ISyntaxNode node, int index, string text, ITextUndoList operations)
        {
            int childCount = node.ChildCount;
            if (((node.NodeType != 0xa3) || (node.Name == "(")) && (((childCount == 1) || (childCount == 2)) && !node.ContainsErrors()))
            {
                int length = text.Length;
                ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.Expression.ToString());
                if (attribute != null)
                {
                    Point range = GetTextRange(index, length, node.Range);
                    Point endPosition = attribute.EndPosition;
                    Point point3 = GetTextRange(index, length, node.ChildList[0].Range);
                    Point point4 = GetTextRange(index, length, new QWhale.Common.Range(endPosition, node.Range.EndPoint));
                    endPosition = GetTextRange(index, length, new QWhale.Common.Range(attribute.Position, endPosition));
                    AddUndoOperation(text, range, point3, endPosition, string.Empty, operations);
                    AddUndoOperation(text, range, endPosition, point4, string.Empty, operations);
                }
                else
                {
                    bool flag = IsPrefixedUnaryExpressionNode(node);
                    Point point5 = GetTextRange(index, length, node.Range);
                    Point point6 = GetTextRange(index, length, node.ChildList[0].Range);
                    Point point7 = GetTextRange(index, length, flag ? new QWhale.Common.Range(node.Position, node.ChildList[0].Position) : new QWhale.Common.Range(node.ChildList[0].Range.EndPoint, node.Range.EndPoint));
                    string delimiter = (flag && ContainsAlpha(text.Substring(point7.X, point7.Y - point7.X))) ? ' '.ToString() : string.Empty;
                    AddUndoOperation(text, point5, flag ? point7 : point6, flag ? point6 : point7, delimiter, operations);
                }
            }
        }

        protected static void FormatVariableDeclaration(ISyntaxNode node, int index, string text, ITextUndoList operations)
        {
            ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.Name.ToString());
            if (attribute != null)
            {
                ISyntaxAttribute attribute2 = node.FindAttribute(NetNodeType.Type.ToString());
                if (attribute2 != null)
                {
                    int length = text.Length;
                    Point range = GetTextRange(index, length, new QWhale.Common.Range(attribute2.Position, node.Range.EndPoint));
                    Point point2 = GetTextRange(index, length, new QWhale.Common.Range(attribute2.Position, attribute.Position));
                    Point point3 = GetTextRange(index, length, attribute.Range);
                    AddUndoOperation(text, range, point2, point3, ' '.ToString(), operations);
                }
            }
        }

        protected static void FormatVariableInitializer(ISyntaxNode node, int index, string text, ITextUndoList operations)
        {
            int childCount = node.ChildCount;
            if (((node.Parent.ChildCount == 1) && (childCount >= 1)) && !node.ContainsErrors())
            {
                int length = text.Length;
                Point range = GetTextRange(index, length, node.Range);
                Point point2 = GetTextRange(index, length, new QWhale.Common.Range(node.Parent.Position, node.Position));
                Point point3 = GetTextRange(index, length, node.ChildList[0].Range);
                Point point4 = GetTextRange(index, length, new QWhale.Common.Range(node.Position, new Point(node.Position.X + node.Name.Length, node.Position.Y)));
                AddUndoOperation(text, range, point2, point4, ' '.ToString(), operations);
                AddUndoOperation(text, range, point4, point3, ' '.ToString(), operations);
            }
        }

        public static ISyntaxNode GetAutoFormatNode(ISyntaxNode node, bool extended, out Point startPt)
        {
            startPt = Point.Empty;
            if (node == null)
            {
                return null;
            }
            ISyntaxNode node2 = extended ? GetBlockNode(node) : GetStatementNode(node);
            if (node2 != null)
            {
                startPt = node2.Position;
                startPt.X++;
                if (!extended)
                {
                    return node2;
                }
                return node2.Parent;
            }
            node2 = (node != null) ? GetDeclarationNode(node) : null;
            if (node2 == null)
            {
                return null;
            }
            ISyntaxAttribute attribute = node2.FindAttribute(SyntaxConsts.DefinitionScope);
            startPt = (attribute != null) ? attribute.Position : node2.Position;
            if (attribute != null)
            {
                startPt.X++;
            }
            return node;
        }

        protected virtual string GetBaseType(ISyntaxNode node)
        {
            Point position = node.Position;
            return this.GetBaseType(node, ref position);
        }

        protected virtual string GetBaseType(ISyntaxNode node, ref Point position)
        {
            ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.TypeList.ToString());
            if (attribute != null)
            {
                position = attribute.Position;
                string str = (string) attribute.Value;
                int index = str.IndexOf(",");
                if (index >= 0)
                {
                    str = str.Substring(0, index).Trim();
                }
                return str;
            }
            if (node.NodeType == 8)
            {
                return this.baseClassType;
            }
            return string.Empty;
        }

        public static ISyntaxNode GetBlockNode(ISyntaxNode node)
        {
            while (node != null)
            {
                if (IsBlockNode(node))
                {
                    return node;
                }
                node = node.Parent;
            }
            return null;
        }

        protected virtual object GetBlockNodeType(ISyntaxNode node, ref Point position, bool checkBlock, bool updatePos, out CodeCompletionScope scope)
        {
            if ((!checkBlock || (GetBlockNode(node) != null)) && (GetDeclarationNode(node) != null))
            {
                if (updatePos)
                {
                    position = node.Position;
                }
                scope = CodeCompletionScope.Private | CodeCompletionScope.Protected | CodeCompletionScope.Global | CodeCompletionScope.Instance | CodeCompletionScope.Static;
                return node;
            }
            scope = CodeCompletionScope.None;
            return null;
        }

        public static ISyntaxNode GetBlockOrMethodNode(ISyntaxNode node)
        {
            while (node != null)
            {
                if (IsMethodNode(node) || IsBlockNode(node))
                {
                    return node;
                }
                node = node.Parent;
            }
            return null;
        }

        protected virtual ISyntaxNode GetCastExpression(ISyntaxNode node)
        {
            ISyntaxNode castExpression = null;
            if (node.NodeType == 0x95)
            {
                return node;
            }
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node3 in node.ChildList)
                {
                    castExpression = this.GetCastExpression(node3);
                    if (castExpression != null)
                    {
                        return castExpression;
                    }
                }
            }
            return castExpression;
        }

        protected virtual ISyntaxNode GetChildByName(ISyntaxNode node, string name, Point position, bool declarations, bool recursive)
        {
            if ((((node.Options & SyntaxNodeOptions.CodeCompletion) != SyntaxNodeOptions.None) && (string.Compare(node.Name, name, !this.CaseSensitive) == 0)) && (!declarations || IsDeclarationNode(node)))
            {
                return node;
            }
            if (recursive && node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    if (declarations || ((node2.FindAttribute(SyntaxConsts.BlockScope) == null) && ((node2.Position.Y < position.Y) || ((node2.Position.Y == position.Y) && (node2.Position.X < position.X)))))
                    {
                        ISyntaxNode node3 = this.GetChildByName(node2, name, position, declarations, true);
                        if (node3 != null)
                        {
                            return node3;
                        }
                    }
                }
            }
            return null;
        }

        protected virtual ISyntaxNode GetChildNode(ISyntaxNodes nodes, ISyntaxNode node, string name, Point position, bool declarations, bool baseTypes)
        {
            return this.GetChildNode(nodes, null, node, name, position, declarations, baseTypes);
        }

        protected virtual ISyntaxNode GetChildNode(ISyntaxNodes nodes, IList<string> types, ISyntaxNode node, string name, Point position, bool declarations, bool baseTypes)
        {
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    ISyntaxNode item = (node2.NodeType == 40) ? this.GetChildNode(null, node2, name, position, declarations, baseTypes) : this.GetChildByName(node2, name, position, declarations, false);
                    if (item != null)
                    {
                        if (nodes != null)
                        {
                            nodes.Add(item);
                        }
                        else
                        {
                            return item;
                        }
                    }
                }
            }
            if ((declarations || baseTypes) && (IsDeclarationNode(node) && (node.Parent != null)))
            {
                string baseType = this.GetBaseType(node);
                if (baseType != string.Empty)
                {
                    CodeCompletionScope scope;
                    if (types == null)
                    {
                        types = new List<string>();
                    }
                    if (types.IndexOf(baseType) >= 0)
                    {
                        return null;
                    }
                    types.Add(baseType);
                    object obj2 = this.GetMemberType(node.Parent, node.Parent.Position, null, baseType, out scope);
                    if (obj2 is ISyntaxNode)
                    {
                        ISyntaxNode node4 = this.GetChildNode(null, (ISyntaxNode) obj2, name, position, declarations, baseTypes);
                        if (nodes != null)
                        {
                            if (node4 != null)
                            {
                                nodes.Add(node4);
                            }
                        }
                        else
                        {
                            return node4;
                        }
                    }
                }
            }
            if ((nodes != null) && (nodes.Count > 0))
            {
                return nodes[0];
            }
            return null;
        }

        protected virtual object GetChildType(ISyntaxNode node, ISyntaxNode refNode, Point position, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.Instance;
            if (IsDeclarationNode(node))
            {
                scope = CodeCompletionScope.Static;
                return node;
            }
            object obj2 = null;
            Point point = node.Position;
            bool isArray = false;
            string name = this.GetNodeDataType(node, refNode, ref point, ref isArray);
            if (name != string.Empty)
            {
                obj2 = this.GetQualifiedType(node, position, name, out scope, isArray);
                scope = CodeCompletionScope.Instance;
            }
            return obj2;
        }

        protected virtual object GetChildType(IList<string> types, Point position, ISyntaxNode node, ISyntaxNode refNode, string name, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.Instance;
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    if (((node2.Options & SyntaxNodeOptions.CodeCompletion) != SyntaxNodeOptions.None) && (string.Compare(node2.Name, name, !this.CaseSensitive) == 0))
                    {
                        return this.GetChildType(node2, refNode, position, out scope);
                    }
                }
            }
            if (this.IsNodePartial(node))
            {
                foreach (ISyntaxNode node3 in this.GetSameNodes(node))
                {
                    if (((node3 != node) && this.IsNodePartial(node3)) && node3.HasChildren)
                    {
                        foreach (ISyntaxNode node4 in node3.ChildList)
                        {
                            if (((node4.Options & SyntaxNodeOptions.CodeCompletion) != SyntaxNodeOptions.None) && (string.Compare(node4.Name, name, !this.CaseSensitive) == 0))
                            {
                                return this.GetChildType(node4, refNode, node4.Position, out scope);
                            }
                        }
                    }
                }
                foreach (ISyntaxTree tree in this.SyntaxTrees)
                {
                    ISyntaxNode declaration = this.GetDeclaration(tree, node);
                    if (((declaration != null) && this.IsNodePartial(declaration)) && declaration.HasChildren)
                    {
                        foreach (ISyntaxNode node6 in declaration.ChildList)
                        {
                            if (((node6.Options & SyntaxNodeOptions.CodeCompletion) != SyntaxNodeOptions.None) && (string.Compare(node6.Name, name, !this.CaseSensitive) == 0))
                            {
                                return this.GetChildType(node6, refNode, node6.Position, out scope);
                            }
                        }
                    }
                }
            }
            string baseType = this.GetBaseType(node);
            if (baseType != string.Empty)
            {
                if (types.IndexOf(baseType) >= 0)
                {
                    return null;
                }
                types.Add(baseType);
                object member = this.GetMemberType(node, position, null, baseType, out scope);
                if (member != null)
                {
                    return this.GetExpressionType(types, node, refNode, position, member, name, out scope);
                }
            }
            return null;
        }

        public override ICodeSnippetsProvider GetCodeSnippets(string language)
        {
            ICodeSnippetsProvider provider;
            object obj2 = this.codeSnippets[language];
            if (obj2 != null)
            {
                return (ICodeSnippetsProvider) obj2;
            }
            if (this.HasSnippetMembers(language))
            {
                provider = new CodeSnippetMembers();
            }
            else
            {
                provider = new CodeSnippets();
            }
            provider.Images = this.internalImages;
            obj2 = new ResourceManager(typeof(NetSyntaxParser)).GetObject("CodeSnippets." + language);
            if (obj2 != null)
            {
                provider.LoadStream(new StringReader((string) obj2));
            }
            this.codeSnippets[language] = provider;
            return provider;
        }

        public static string GetCommentTemplate(Point position, ISyntaxNode node, string comment)
        {
            string str = string.Empty;
            if (((node.NodeType != 0x39) || node.HasChildren) || ((node.Size.Height != 0) || (node.Size.Width != comment.Length)))
            {
                return str;
            }
            ISyntaxNode xmlReferenceNode = GetXmlReferenceNode(node);
            if (xmlReferenceNode == null)
            {
                return str;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(comment + ' ' + string.Format(SyntaxParserConsts.XmlTag, XmlCommentType.Summary.ToString().ToLower()));
            builder.Append("\r\n");
            builder.Append(comment + ' ' + SyntaxConsts.DefaultCaretSymbol);
            builder.Append("\r\n");
            builder.Append(comment + ' ' + string.Format(SyntaxParserConsts.XmlEndTag, XmlCommentType.Summary.ToString().ToLower()));
            ISyntaxNode node3 = xmlReferenceNode.FindNode(0x1b);
            if ((node3 != null) && node3.HasChildren)
            {
                foreach (ISyntaxNode node4 in node3.ChildList)
                {
                    if (node4.NodeType == 0x1a)
                    {
                        builder.Append("\r\n");
                        builder.Append(comment + ' ' + string.Format(SyntaxParserConsts.XmlComplexTag, XmlCommentType.Param.ToString().ToLower(), XmlCommentType.Name.ToString().ToLower(), node4.Name));
                    }
                }
            }
            if (IsMethodNode(xmlReferenceNode) && (xmlReferenceNode.FindAttribute(NetNodeType.Type.ToString()) != null))
            {
                builder.Append("\r\n");
                builder.Append(comment + ' ' + string.Format(SyntaxParserConsts.XmlTag, XmlCommentType.Returns.ToString().ToLower()));
                builder.Append(string.Format(SyntaxParserConsts.XmlEndTag, XmlCommentType.Returns.ToString().ToLower()));
            }
            return builder.ToString();
        }

        protected virtual ISyntaxNode GetDeclaration(ISyntaxTree tree, ISyntaxNode node)
        {
            ISyntaxNode parent = node;
            while (parent != null)
            {
                if (parent.NodeType == 7)
                {
                    break;
                }
                parent = parent.Parent;
            }
            string name = node.Name;
            node = (parent != null) ? this.GetNamespaceNode(tree.Root, parent.Name) : tree.Root;
            if (node == null)
            {
                return null;
            }
            return this.GetChildNode(null, node, name, new Point(0x7fffffff, 0x7fffffff), true, false);
        }

        protected virtual object GetDeclaration(ISyntaxNode node, ISyntaxNode refNode, Point position, string name, out CodeCompletionScope scope)
        {
            ISyntaxNode node2 = null;
            ISyntaxNode root = node.Root;
            scope = CodeCompletionScope.Static;
            int length = name.LastIndexOf(".");
            if (length >= 0)
            {
                string nspace = (length >= 0) ? name.Substring(0, length) : string.Empty;
                name = (length >= 0) ? name.Substring(length + 1) : name;
                object obj2 = this.GetQualifiedNode(root, nspace, name, position);
                if (obj2 != null)
                {
                    return obj2;
                }
                foreach (ISyntaxTree tree in this.SyntaxTrees)
                {
                    obj2 = this.GetQualifiedNode(tree.Root, nspace, name, new Point(0x7fffffff, 0x7fffffff));
                    if (obj2 != null)
                    {
                        return obj2;
                    }
                }
                return null;
            }
            ISyntaxNode declarationOrWithNode = GetDeclarationOrWithNode(node);
            if (((declarationOrWithNode == null) || !IsDeclarationNode(declarationOrWithNode)) || (string.Compare(declarationOrWithNode.Name, name, !this.CaseSensitive) != 0))
            {
                while (declarationOrWithNode != null)
                {
                    if (declarationOrWithNode.NodeType == 7)
                    {
                        node2 = declarationOrWithNode;
                    }
                    if ((declarationOrWithNode.NodeType == 0x80) && (node.Parent != declarationOrWithNode))
                    {
                        object member = this.GetWithStatementExpression(declarationOrWithNode, refNode, out scope);
                        if (member != null)
                        {
                            return this.GetMemberType(node, position, member, name, out scope);
                        }
                    }
                    else
                    {
                        object obj4 = this.GetChildNode(null, declarationOrWithNode, name, position, true, false);
                        if (obj4 != null)
                        {
                            return obj4;
                        }
                    }
                    declarationOrWithNode = GetDeclarationOrWithNode(declarationOrWithNode.Parent);
                }
                foreach (ISyntaxTree tree2 in this.SyntaxTrees)
                {
                    ISyntaxNode namespaceNode = (node2 != null) ? this.GetNamespaceNode(tree2.Root, node2.Name) : tree2.Root;
                    if (namespaceNode != null)
                    {
                        namespaceNode = this.GetChildNode(null, namespaceNode, name, new Point(0x7fffffff, 0x7fffffff), true, false);
                        if (namespaceNode != null)
                        {
                            return namespaceNode;
                        }
                    }
                    foreach (INetNamespace namespace2 in this.GetNamespaces(node, false))
                    {
                        namespaceNode = this.GetNamespaceNode(tree2.Root, namespace2.Namespace);
                        if (namespaceNode != null)
                        {
                            namespaceNode = this.GetChildNode(null, namespaceNode, name, new Point(0x7fffffff, 0x7fffffff), true, false);
                            if (namespaceNode != null)
                            {
                                return namespaceNode;
                            }
                        }
                    }
                }
                return null;
            }
            return declarationOrWithNode;
        }

        public static ISyntaxNode GetDeclarationNode(ISyntaxNode node)
        {
            while (node != null)
            {
                if (IsDeclarationNode(node) || (node.NodeType == 1))
                {
                    return node;
                }
                node = node.Parent;
            }
            return null;
        }

        protected static ISyntaxNode GetDeclarationOrWithNode(ISyntaxNode node)
        {
            while (node != null)
            {
                if ((IsDeclarationNode(node) || (node.NodeType == 1)) || (node.NodeType == 0x80))
                {
                    return node;
                }
                node = node.Parent;
            }
            return null;
        }

        public override string GetDescription(IListMembers members, ISyntaxNode node, object member, string name, bool fullDescription)
        {
            string description = base.GetDescription(members, node, member, name, fullDescription);
            if ((member is ISyntaxNode) && ((description == null) || (description == string.Empty)))
            {
                ISyntaxNode parent = (ISyntaxNode) member;
                if (fullDescription)
                {
                    IListMember member2 = (members != null) ? members.CreateListMember() : new ListMember();
                    member2.Qualifier = "(" + ((NetNodeType) parent.NodeType).ToString() + ")";
                    member2.DataType = this.GetNodeDataType(parent, null);
                    member2.Name = parent.Name;
                    description = member2.Description;
                }
                string paramName = string.Empty;
                string paramValue = string.Empty;
                if (parent.NodeType == 0x1a)
                {
                    paramName = SyntaxParserConsts.NameTag;
                    paramValue = parent.Name;
                    if (parent.Parent != null)
                    {
                        parent = parent.Parent.Parent;
                    }
                }
                if ((parent == null) || (parent.Parent == null))
                {
                    return description;
                }
                int num = parent.Index - 1;
                if (num < 0)
                {
                    return description;
                }
                parent = parent.Parent.ChildList[num];
                if (parent.NodeType != 0x39)
                {
                    return description;
                }
                parent = this.FindXmlNode(parent, name, paramName, paramValue);
                if (parent == null)
                {
                    return description;
                }
                string str4 = this.GetXmlComment(parent).Trim();
                if (description==null || description.Equals(string.Empty))
                {
                    //case null:
                    //case string.Empty:
                        return str4;
                }
                if (str4 != string.Empty)
                {
                    description = description + SyntaxConsts.DefaultBrTag + str4;
                }
            }
            return description;
        }

        protected virtual object GetElementAccessType(object member, ISyntaxNode refNode)
        {
            if (member is ISyntaxNode)
            {
                ISyntaxNode node = ((ISyntaxNode) member).FindNode(0x19);
                if (node != null)
                {
                    CodeCompletionScope scope;
                    return this.GetChildType(node, refNode, node.Position, out scope);
                }
            }
            return null;
        }

        protected virtual object GetEnumMemberType(ISyntaxNode node, out CodeCompletionScope scope, bool assignment)
        {
            scope = CodeCompletionScope.None;
            ISyntaxAttribute attribute = assignment ? node.FindAttribute(NetNodeType.Assignment.ToString()) : null;
            if (!assignment || (attribute != null))
            {
                object member = this.GetExpressionType(node.ChildList[0].Position, node.ChildList[0], node, out scope);
                if (member != null)
                {
                    member = this.GetEnumType(member);
                    if (member != null)
                    {
                        scope = CodeCompletionScope.ShortType | CodeCompletionScope.TypeName | CodeCompletionScope.Static;
                        return member;
                    }
                }
            }
            return null;
        }

        protected virtual object GetEnumType(object member)
        {
            if ((member is ISyntaxNode) && (((ISyntaxNode) member).NodeType == 11))
            {
                return member;
            }
            return null;
        }

        protected virtual object GetExpressionType(Point position, ISyntaxNode node, ISyntaxNode refNode, out CodeCompletionScope scope)
        {
            object member = null;
            scope = CodeCompletionScope.None;
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.CastExpression:
                case NetNodeType.CastInvocationExpression:
                {
                    string nodeDataType = this.GetNodeDataType(node, refNode);
                    member = this.GetExpressionType(null, node, refNode, position, null, (nodeDataType != string.Empty) ? nodeDataType : node.Name, out scope);
                    scope = CodeCompletionScope.Instance;
                    return member;
                }
                case NetNodeType.CastInvocationTargetExpression:
                case NetNodeType.CastTargetExpression:
                case NetNodeType.ArrayCreationExpression:
                case NetNodeType.ObjectCreationExpression:
                case NetNodeType.ArrayInitializerExpression:
                case NetNodeType.NamespaceAliasExpression:
                    return member;

                case NetNodeType.PrimaryExpression:
                    return this.GetExpressionType(null, node, refNode, position, null, node.Name, out scope);

                case NetNodeType.BaseAccessExpression:
                    member = GetDeclarationNode(node);
                    if (member != null)
                    {
                        member = this.GetExpressionType(null, node, refNode, position, null, this.GetBaseType((ISyntaxNode) member), out scope);
                    }
                    scope = CodeCompletionScope.Instance | CodeCompletionScope.Static;
                    return member;

                case NetNodeType.ThisAccessExpression:
                    member = GetDeclarationNode(node);
                    scope = CodeCompletionScope.Private | CodeCompletionScope.Protected | CodeCompletionScope.Instance | CodeCompletionScope.Static;
                    return member;

                case NetNodeType.MemberAccessExpression:
                case NetNodeType.PointerMemberAccessExpression:
                    if (node.HasChildren)
                    {
                        member = this.GetExpressionType(node.ChildList[0].Position, node.ChildList[0], refNode, out scope);
                        if (member == null)
                        {
                            return member;
                        }
                        member = this.GetExpressionType(null, node, refNode, position, member, node.Name, out scope);
                        if (((member != null) && ((scope & CodeCompletionScope.Method) != CodeCompletionScope.None)) && ((node.Parent == null) || (node.Parent.NodeType != 0xa3)))
                        {
                            member = null;
                        }
                    }
                    return member;

                case NetNodeType.ElementAccessExpression:
                    if (node.HasChildren)
                    {
                        member = this.GetExpressionType(node.ChildList[0].Position, node.ChildList[0], refNode, out scope);
                        if (member != null)
                        {
                            member = this.GetElementAccessType(member, refNode);
                        }
                        scope = CodeCompletionScope.Instance;
                    }
                    return member;

                case NetNodeType.InvocationExpression:
                    if (node.HasChildren)
                    {
                        member = this.GetExpressionType(node.ChildList[0].Position, node.ChildList[0], refNode, out scope);
                        scope = CodeCompletionScope.Instance;
                    }
                    return member;

                case NetNodeType.ParenthesizedExpression:
                    if (node.HasChildren)
                    {
                        member = this.GetExpressionType(node.ChildList[0].Position, node.ChildList[0], refNode, out scope);
                    }
                    return member;
            }
            return member;
        }

        protected virtual object GetExpressionType(ISyntaxNode node, ISyntaxNode refNode, Point position, string name, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.Instance;
            ISyntaxNode node2 = this.GetNodeByName(node, name, position);
            ISyntaxAttribute attribute = (node2 != null) ? node2.FindAttribute(NetNodeType.VariableReference.ToString()) : null;
            if ((attribute != null) && (attribute.Value.ToString() != name))
            {
                node2 = this.GetNodeByName(node, attribute.Value.ToString(), position);
            }
            if (node2 == null)
            {
                return null;
            }
            return this.GetChildType(node2, refNode, position, out scope);
        }

        protected virtual object GetExpressionType(IList<string> types, ISyntaxNode node, ISyntaxNode refNode, Point position, object member, string name, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.Instance;
            if (member == null)
            {
                if (node != null)
                {
                    object obj2 = this.GetExpressionType(node, refNode, position, name, out scope);
                    if (obj2 != null)
                    {
                        return obj2;
                    }
                }
            }
            else if (member is ISyntaxNode)
            {
                if (types == null)
                {
                    types = new List<string>();
                }
                return this.GetChildType(types, position, (ISyntaxNode) member, refNode, name, out scope);
            }
            return this.GetMemberType(node, position, member, name, out scope);
        }

        public static ISyntaxNode GetInvocationNode(string text, ISyntaxNode node, Point position, out int paramIndex, out int paramCount)
        {
            paramIndex = -1;
            paramCount = 0;
            while (node != null)
            {
                if (((node.NodeType == 0x1d) && ((node.Position.Y < position.Y) || ((node.Position.Y == position.Y) && (node.Position.X <= position.X)))) && (((text == null) || (position.X <= 0)) || ((position.X > text.Length) || (text[position.X - 1] != ')'))))
                {
                    paramIndex = 0;
                    if (node.HasChildren)
                    {
                        foreach (ISyntaxNode node2 in node.ChildList)
                        {
                            if (node2.NodeType == 0x1c)
                            {
                                Point endPoint = node2.Range.EndPoint;
                                if ((endPoint.Y < position.Y) || ((endPoint.Y == position.Y) && (endPoint.X < position.X)))
                                {
                                    paramIndex++;
                                }
                                paramCount++;
                            }
                        }
                    }
                }
                if (IsInvocationNode(node))
                {
                    return node;
                }
                node = node.Parent;
            }
            return null;
        }

        protected virtual object GetMemberAccessType(ISyntaxNode node, ISyntaxNode refNode, ref string name, ref Point position, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.None;
            new SyntaxNodes();
            name = node.Name;
            object member = node.HasChildren ? this.GetExpressionType(node.ChildList[0].Position, node.ChildList[0], refNode, out scope) : null;
            if (member != null)
            {
                ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.Name.ToString());
                if (attribute != null)
                {
                    position = attribute.Position;
                }
                if (this.IsEnumTypeRegistered() && (this.GetEnumType(member) != null))
                {
                    scope |= CodeCompletionScope.Static;
                }
            }
            return member;
        }

        private int GetMemberIndex(ISyntaxNode node, out MemberAttribute attributes)
        {
            int num = -1;
            attributes = MemberAttribute.None;
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.Namespace:
                    return 2;

                case NetNodeType.Class:
                    return 3;

                case NetNodeType.Struct:
                    return 12;

                case NetNodeType.Interface:
                    return 9;

                case NetNodeType.Enum:
                    return 6;

                case NetNodeType.Module:
                case NetNodeType.FixedVariable:
                case NetNodeType.Operator:
                case NetNodeType.OperatorType:
                case NetNodeType.Indexer:
                    return num;

                case NetNodeType.Field:
                    return 8;

                case NetNodeType.Constant:
                    return 4;

                case NetNodeType.LocalVariable:
                case NetNodeType.Parameter:
                case NetNodeType.ImplicitVariable:
                    return 8;

                case NetNodeType.Method:
                case NetNodeType.Constructor:
                case NetNodeType.Destructor:
                    return 10;

                case NetNodeType.Delegate:
                    return 5;

                case NetNodeType.Property:
                    num = 11;
                    if (node.HasChildren)
                    {
                        foreach (ISyntaxNode node2 in node.ChildList)
                        {
                            if (node2.NodeType == 0x24)
                            {
                                if (string.Compare(node2.Name, "get", !this.CaseSensitive) == 0)
                                {
                                    attributes |= MemberAttribute.CanRead;
                                }
                                if (string.Compare(node2.Name, "get", !this.CaseSensitive) == 0)
                                {
                                    attributes |= MemberAttribute.CanWrite;
                                }
                            }
                        }
                    }
                    return num;

                case NetNodeType.Event:
                    return 7;

                case NetNodeType.LocalConst:
                    return 4;
            }
            return num;
        }

        public override object GetMemberType(ISyntaxNode node, Point position, object member, string name, out CodeCompletionScope scope)
        {
            object registeredObject = base.GetMemberType(node, position, member, name, out scope);
            if ((registeredObject == null) && (((member == null) || (member is INetNamespace)) || ((member is ISyntaxNode) && (((ISyntaxNode) member).NodeType == 7))))
            {
                scope = CodeCompletionScope.Static;
                if (member is ISyntaxNode)
                {
                    return this.GetDeclaration((ISyntaxNode) member, node, ((ISyntaxNode) member).Position, name, out scope);
                }
                if (member == null)
                {
                    registeredObject = this.GetDeclaration(node, node, position, name, out scope);
                    if (registeredObject != null)
                    {
                        return registeredObject;
                    }
                }
                string str = name;
                if (member is INetNamespace)
                {
                    str = (name != string.Empty) ? (((INetNamespace) member).GetName() + "." + name) : ((INetNamespace) member).GetName();
                }
                registeredObject = this.GetRegisteredObject(str);
                if (registeredObject != null)
                {
                    scope = CodeCompletionScope.Instance;
                    registeredObject = this.GetObjectType(registeredObject);
                }
                if (registeredObject == null)
                {
                    registeredObject = this.GetTypeByName(node, str);
                    if ((registeredObject is System.Type) && NetTypes.GlobalTypes.Contains(registeredObject))
                    {
                        scope |= CodeCompletionScope.Instance;
                    }
                }
                if (registeredObject == null)
                {
                    registeredObject = this.GetNamespace(str, node);
                }
            }
            return registeredObject;
        }

        public override object GetMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out CodeCompletionScope scope)
        {
            string str = string.Empty;
            Point point = position;
            Point point2 = endPos;
            object obj2 = this.GetSpecialMemberType(text, node, ref str, ref point, ref point2, out scope);
            if (obj2 != null)
            {
                name = str;
                position = point;
                endPos = point2;
                return obj2;
            }
            name = string.Empty;
            scope = CodeCompletionScope.None;
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.XmlComment:
                    return node;

                case NetNodeType.XmlTag:
                    if (node.FindAttribute(XmlLexerToken.CloseTag.ToString()) == null)
                    {
                        position = new Point(node.Position.X + 1, node.Position.Y);
                    }
                    name = node.Name;
                    return node;

                case NetNodeType.WithStatementMember:
                {
                    ISyntaxAttribute attribute2 = node.FindAttribute(NetNodeType.WithStatement.ToString());
                    if (attribute2 != null)
                    {
                        obj2 = this.GetWithStatementExpression((ISyntaxNode) attribute2.Value, node, out scope);
                        if (obj2 != null)
                        {
                            return obj2;
                        }
                    }
                    break;
                }
                case NetNodeType.Field:
                case NetNodeType.LocalVariable:
                case NetNodeType.ImplicitVariable:
                    return this.GetTypeOfNode(node, node, ref name, ref position, true, true, out scope);

                case NetNodeType.Using:
                    return this.GetUsingType(node, ref name, ref position, out scope);

                case NetNodeType.PrimaryExpression:
                    name = node.Name;
                    if (((position.X > 0) && (position.X <= text.Length)) && (text[position.X - 1] == '.'))
                    {
                        break;
                    }
                    return this.GetBlockNodeType(node, ref position, true, true, out scope);

                case NetNodeType.ObjectCreationExpression:
                case NetNodeType.AsIsExpression:
                    if ((position.Y == node.Position.Y) && (position.X > (node.Position.X + node.Name.Length)))
                    {
                        return this.GetTypeOfNode(node, node, ref name, ref position, false, true, out scope);
                    }
                    break;

                case NetNodeType.MemberAccessExpression:
                case NetNodeType.PointerMemberAccessExpression:
                    return this.GetMemberAccessType(node, node, ref name, ref position, out scope);
            }
            Point point3 = position;
            string str2 = string.Empty;
            if (!this.GetQualifiedName(text, ref str2, ref point3))
            {
                return null;
            }
            obj2 = this.GetTypeOfNode(node, str2, point3, ref name, ref position, false, true, out scope, false);
            if (name != null)
            {
                int index = name.IndexOf(".");
                if (index >= 0)
                {
                    name = name.Substring(0, index);
                }
            }
            return obj2;
        }

        public static ISyntaxNode GetMethodNode(ISyntaxNode node)
        {
            while (node != null)
            {
                if (IsMethodNode(node))
                {
                    return node;
                }
                node = node.Parent;
            }
            return null;
        }

        public override object GetMethodType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out int paramIndex, out int paramCount, out CodeCompletionScope scope)
        {
            ISyntaxNode node2 = GetInvocationNode(text, node, position, out paramIndex, out paramCount);
            name = string.Empty;
            object declarationNode = null;
            scope = CodeCompletionScope.None;
            if (node2 == null)
            {
                goto Label_0190;
            }
            position = node2.Position;
            endPos = node2.Range.EndPoint;
            switch (((NetNodeType) node2.NodeType))
            {
                case NetNodeType.CastInvocationExpression:
                case NetNodeType.CastInvocationTargetExpression:
                    declarationNode = this.GetMemberType(text, node2, ref name, ref position, ref endPos, out scope);
                    goto Label_0183;

                case NetNodeType.ObjectCreationExpression:
                {
                    Point point = node2.Position;
                    bool isArray = false;
                    string str = this.GetNodeDataType(node2, node, ref point, ref isArray);
                    name = ".ctor";
                    if (str != string.Empty)
                    {
                        declarationNode = this.GetExpressionType(null, node2, node, point, null, str, out scope);
                    }
                    scope = CodeCompletionScope.TypeName | CodeCompletionScope.Instance | CodeCompletionScope.Static;
                    goto Label_0183;
                }
                case NetNodeType.ElementAccessExpression:
                    declarationNode = this.GetMemberAccessType(node2, node, ref name, ref position, out scope);
                    if (node2.HasChildren)
                    {
                        position = node2.ChildList[0].Position;
                    }
                    name = ".ElementAccess";
                    scope = CodeCompletionScope.TypeName | CodeCompletionScope.Instance;
                    goto Label_0183;

                case NetNodeType.InvocationExpression:
                {
                    if (!node2.HasChildren)
                    {
                        declarationNode = GetDeclarationNode(node2);
                        break;
                    }
                    ISyntaxNode node3 = node2.ChildList[0];
                    name = node3.Name;
                    if (!this.IsMemberExpressionNode(node3))
                    {
                        declarationNode = GetDeclarationNode(node2);
                        break;
                    }
                    declarationNode = this.GetMemberType(text, node3, ref name, ref position, ref endPos, out scope);
                    break;
                }
                default:
                    goto Label_0183;
            }
            scope = (((CodeCompletionScope.ShortType | CodeCompletionScope.TypeName | CodeCompletionScope.Instance | CodeCompletionScope.Static) | (scope & CodeCompletionScope.Global)) | (scope & CodeCompletionScope.Protected)) | (scope & CodeCompletionScope.Private);
        Label_0183:
            position = node2.Position;
        Label_0190:
            if (name == string.Empty)
            {
                return null;
            }
            return declarationNode;
        }

        protected virtual object GetNamespace(string name, ISyntaxNode node)
        {
            foreach (INetNamespace namespace2 in this.GetNamespaces(node, true))
            {
                string strB = namespace2.Namespace;
                if (this.CaseSensitive ? strB.StartsWith(name) : strB.ToLower().StartsWith(name.ToLower()))
                {
                    if (string.Compare(name, strB, !this.CaseSensitive) == 0)
                    {
                        if (namespace2.Alias != string.Empty)
                        {
                            object typeByName = this.GetTypeByName(namespace2.Alias);
                            if (typeByName != null)
                            {
                                return typeByName;
                            }
                        }
                        return namespace2;
                    }
                    return new NetNamespace(name, node == null);
                }
            }
            return null;
        }

        protected virtual ISyntaxNode GetNamespaceNode(ISyntaxNode node, string name)
        {
            if (node != null)
            {
                if ((node.NodeType == 7) && (string.Compare(node.Name, name, !this.CaseSensitive) == 0))
                {
                    return node;
                }
                if (node.HasChildren)
                {
                    foreach (ISyntaxNode node2 in node.ChildList)
                    {
                        if (((node2.NodeType == 40) || (node2.NodeType == 7)) || (node2.NodeType == 1))
                        {
                            ISyntaxNode namespaceNode = this.GetNamespaceNode(node2, name);
                            if (namespaceNode != null)
                            {
                                return namespaceNode;
                            }
                        }
                    }
                }
            }
            return null;
        }

        protected virtual IList<string> GetNamespaces()
        {
            return this.namespaces;
        }

        protected virtual IList<INetNamespace> GetNamespaces(ISyntaxNode node, bool system)
        {
            IList<INetNamespace> namespaces = new List<INetNamespace>();
            foreach (string str in this.namespaces)
            {
                namespaces.Add(new NetNamespace(str, true));
            }
            if (node != null)
            {
                this.GetNamespaces(node, namespaces);
                foreach (ISyntaxTree tree in this.SyntaxTrees)
                {
                    this.AddUnitNamespaces(tree.Root, namespaces);
                }
            }
            if (system)
            {
                foreach (string str2 in this.GetNamespaces())
                {
                    if (this.IndexOfNamespace(namespaces, str2) < 0)
                    {
                        namespaces.Add(new NetNamespace(str2, true));
                    }
                }
            }
            ((List<INetNamespace>) namespaces).Sort(new Comparison<INetNamespace>(this.CompareNamespaces));
            return namespaces;
        }

        protected virtual void GetNamespaces(ISyntaxNode node, IList<INetNamespace> namespaces)
        {
            while (node != null)
            {
                switch (((NetNodeType) node.NodeType))
                {
                    case NetNodeType.Unit:
                    case NetNodeType.Namespace:
                        if (node.NodeType == 7)
                        {
                            this.AddNetNamespace(namespaces, node);
                        }
                        else
                        {
                            this.AddUnitNamespaces(node, namespaces);
                        }
                        if (node.HasChildren)
                        {
                            ISyntaxNode node2 = this.FindUsingList(node);
                            if ((node2 != null) && node2.HasChildren)
                            {
                                foreach (ISyntaxNode node3 in node2.ChildList)
                                {
                                    if (node3.NodeType == 3)
                                    {
                                        this.AddNetNamespace(namespaces, node3);
                                    }
                                }
                            }
                        }
                        break;
                }
                node = node.Parent;
            }
        }

        protected virtual ISyntaxNode GetNodeByName(ISyntaxNode node, string name, Point position)
        {
            ISyntaxNode declarationNode = GetDeclarationNode(node);
            ISyntaxNode node3 = null;
            while (node != null)
            {
                if (node == declarationNode)
                {
                    break;
                }
                node3 = this.GetChildByName(node, name, position, false, true);
                if (node3 != null)
                {
                    return node3;
                }
                node = node.Parent;
            }
            if (declarationNode != null)
            {
                node3 = this.GetChildNode(null, declarationNode, name, position, false, true);
                if (node3 != null)
                {
                    return node3;
                }
                if (!this.IsNodePartial(declarationNode))
                {
                    return node3;
                }
                foreach (ISyntaxNode node4 in this.GetSameNodes(declarationNode))
                {
                    if ((node4 != declarationNode) && this.IsNodePartial(node4))
                    {
                        node3 = this.GetChildNode(null, null, node4, name, new Point(0x7fffffff, 0x7fffffff), false, true);
                        if (node3 != null)
                        {
                            return node3;
                        }
                    }
                }
                foreach (ISyntaxTree tree in this.SyntaxTrees)
                {
                    ISyntaxNode declaration = this.GetDeclaration(tree, declarationNode);
                    if ((declaration != null) && this.IsNodePartial(declaration))
                    {
                        node3 = this.GetChildNode(null, declaration, name, new Point(0x7fffffff, 0x7fffffff), false, true);
                        if (node3 != null)
                        {
                            return node3;
                        }
                    }
                }
            }
            return node3;
        }

        protected virtual CodeCompletionScope GetNodeCodeCompletionScope(ISyntaxNode node)
        {
            if (IsDeclarationNode(node))
            {
                return CodeCompletionScope.Static;
            }
            ISyntaxNode validNode = this.GetValidNode(node.Parent);
            if (validNode != null)
            {
                if ((node.NodeType == 13) && (validNode.NodeType == 11))
                {
                    return CodeCompletionScope.Static;
                }
                if (((node.Options & SyntaxNodeOptions.CodeCompletion) != SyntaxNodeOptions.None) && (validNode.NodeType == 12))
                {
                    return CodeCompletionScope.Static;
                }
            }
            if (node.HasAttributes)
            {
                foreach (ISyntaxAttribute attribute in node.AttributeList)
                {
                    if ((attribute.Name == NetNodeType.Modifier.ToString()) && ((((string) attribute.Value) == "static") || (string.Compare((string) attribute.Value, "shared", true) == 0)))
                    {
                        return CodeCompletionScope.Static;
                    }
                }
            }
            return CodeCompletionScope.Instance;
        }

        protected virtual string GetNodeDataType(ISyntaxNode node, ISyntaxNode refNode)
        {
            Point position = node.Position;
            bool isArray = false;
            string str = this.GetNodeDataType(node, refNode, ref position, ref isArray);
            if ((str != string.Empty) && isArray)
            {
                return (str + "[]");
            }
            return str;
        }

        protected virtual string GetNodeDataType(ISyntaxNode node, ISyntaxNode refNode, ref Point position, ref bool isArray)
        {
            ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.Type.ToString());
            isArray = false;
            if (attribute != null)
            {
                position = attribute.Position;
                isArray = node.FindAttribute(NetNodeType.ArraySpecifier.ToString()) != null;
                return attribute.Value.ToString();
            }
            return string.Empty;
        }

        protected virtual string GetNodeName(ISyntaxNode node, CodeCompletionScope scope)
        {
            if ((scope & CodeCompletionScope.TypeName) != CodeCompletionScope.None)
            {
                ISyntaxNode declarationNode = GetDeclarationNode(node);
                if ((declarationNode != null) && (declarationNode.Name != string.Empty))
                {
                    return (declarationNode.Name + "." + node.Name);
                }
            }
            return node.Name;
        }

        protected virtual IParameterMembers GetNodeParameters(ISyntaxNode node, ISyntaxNode refNode)
        {
            IParameterMembers members = null;
            ISyntaxNode node2 = node.FindNode(0x1b);
            if ((node2 != null) && node2.HasChildren)
            {
                foreach (ISyntaxNode node3 in node2.ChildList)
                {
                    if (node3.NodeType == 0x1a)
                    {
                        if (members == null)
                        {
                            members = new ParameterMembers();
                        }
                        IParameterMember member = members.AddParameterMember();
                        member.Name = node3.Name;
                        member.DataType = this.GetNodeDataType(node3, refNode);
                        member.Qualifier = this.GetNodeQualifier(node3);
                        member.Description = this.GetDescription(null, node, node3, XmlCommentType.Param.ToString(), false);
                    }
                }
            }
            return members;
        }

        protected virtual string GetNodeQualifier(ISyntaxNode node)
        {
            string str = string.Empty;
            if (node.HasAttributes)
            {
                foreach (ISyntaxAttribute attribute in node.AttributeList)
                {
                    if ((attribute.Name == NetNodeType.Modifier.ToString()) || (attribute.Name == NetNodeType.ParameterModifier.ToString()))
                    {
                        str = str + ' ' + ((string) attribute.Value);
                    }
                }
            }
            string classDataType = string.Empty;
            switch (node.NodeType)
            {
                case 8:
                    classDataType = SyntaxParserConsts.ClassDataType;
                    break;

                case 9:
                    classDataType = SyntaxParserConsts.StructDataType;
                    break;

                case 10:
                    classDataType = SyntaxParserConsts.InterfaceDataType;
                    break;

                case 11:
                    classDataType = SyntaxParserConsts.EnumDataType;
                    break;
            }
            return (str.Trim() + " " + classDataType).Trim();
        }

        public override object GetNodeType(string text, ISyntaxNode node, Point position)
        {
            if (node != null)
            {
                Point point = position;
                bool isArray = false;
                string name = this.GetNodeDataType(node, node, ref point, ref isArray);
                if ((name == string.Empty) && IsDeclarationNode(node))
                {
                    name = this.GetBaseType(node, ref point);
                }
                if (((name != string.Empty) && (point.Y == position.Y)) && ((point.X <= position.X) && ((point.X + name.Length) > position.X)))
                {
                    CodeCompletionScope scope;
                    return this.GetQualifiedType(node, position, name, out scope, isArray);
                }
            }
            return base.GetNodeType(text, node, position);
        }

        protected virtual object GetObjectType(object obj)
        {
            return obj.GetType();
        }

        protected virtual object GetParameterType(object member, ISyntaxNode refNode, int param)
        {
            if (member is ISyntaxNode)
            {
                IParameterMembers nodeParameters = this.GetNodeParameters((ISyntaxNode) member, refNode);
                if ((nodeParameters != null) && (param < nodeParameters.Count))
                {
                    CodeCompletionScope scope;
                    return this.GetQualifiedType((ISyntaxNode) member, ((ISyntaxNode) member).Position, nodeParameters[param].DataType, out scope, false);
                }
            }
            return null;
        }

        private bool GetQualifiedName(string text, ref string name, ref Point position)
        {
            name = string.Empty;
            if (text == null)
            {
                return false;
            }
            if (text.Trim() == string.Empty)
            {
                return true;
            }
            int x = Math.Min(text.Length, position.X);
            int num2 = x;
            for (int i = x - 1; i >= 0; i--)
            {
                if (!this.IsQualifiedChar(text[i]))
                {
                    break;
                }
                x = i;
            }
            while ((num2 < text.Length) && this.IsQualifiedChar(text[num2]))
            {
                num2++;
            }
            position = new Point(x, position.Y);
            if (num2 > x)
            {
                name = text.Substring(x, num2 - x);
            }
            return (num2 > x);
        }

        private bool GetQualifiedName(ref string name, ref string subname, ref Point pt, Point position, bool direction)
        {
            int index;
            subname = string.Empty;
            if (((name == string.Empty) || (pt.Y != position.Y)) || ((position.X < pt.X) || (position.X > (pt.X + name.Length))))
            {
                return false;
            }
            if (direction)
            {
                index = name.IndexOf(".", Math.Min(position.X - pt.X, name.Length));
            }
            else
            {
                index = name.LastIndexOf(".", Math.Min(position.X - pt.X, name.Length));
            }
            if (index >= 0)
            {
                subname = name.Substring(index + 1);
                name = name.Substring(0, index);
                index = subname.IndexOf('.');
                if (index >= 0)
                {
                    subname = subname.Substring(0, index);
                }
                pt = new Point((pt.X + name.Length) + 1, pt.Y);
            }
            else
            {
                pt = new Point(-1, -1);
            }
            return true;
        }

        protected virtual ISyntaxNode GetQualifiedNode(ISyntaxNode root, string nspace, string name, Point position)
        {
            ISyntaxNode namespaceNode = this.GetNamespaceNode(root, nspace);
            if (namespaceNode == null)
            {
                return null;
            }
            return this.GetChildNode(null, namespaceNode, name, position, true, false);
        }

        protected virtual object GetQualifiedType(ISyntaxNode node, Point position, string name, out CodeCompletionScope scope, bool isArray)
        {
            scope = CodeCompletionScope.None;
            object member = null;
            string[] strArray = name.Split(new char[] { '.' });
            for (int i = 0; i < strArray.Length; i++)
            {
                string str = strArray[i];
                bool flag = isArray && (i == (strArray.Length - 1));
                member = this.GetMemberType(node, position, member, flag ? (str + "[]") : str, out scope);
                if ((member == null) && flag)
                {
                    member = this.GetMemberType(node, position, member, str, out scope);
                    if (!(member is ISyntaxNode))
                    {
                        member = null;
                    }
                }
                if (member == null)
                {
                    return member;
                }
            }
            return member;
        }

        public virtual object GetRegisteredObject(string name)
        {
            return this.objects[this.CaseSensitive ? name : name.ToLower()];
        }

        protected virtual ISyntaxNodes GetSameNodes(ISyntaxNode node)
        {
            string name = node.Name;
            node = node.Parent;
            while (node != null)
            {
                if (node.NodeType != 40)
                {
                    break;
                }
                node = node.Parent;
            }
            ISyntaxNodes nodes = new SyntaxNodes();
            this.GetChildNode(nodes, (node != null) ? node : this.SyntaxTree.Root, name, new Point(0x7fffffff, 0x7fffffff), true, false);
            return nodes;
        }

        private int GetScopeImageIndex(ISyntaxNode node)
        {
            ISyntaxAttribute attribute;
            string str;
            NetNodeType nodeType = (NetNodeType) node.NodeType;
            if (nodeType <= NetNodeType.Parameter)
            {
                switch (nodeType)
                {
                    case NetNodeType.LocalVariable:
                    case NetNodeType.Parameter:
                        goto Label_0025;
                }
                goto Label_0028;
            }
            if ((nodeType != NetNodeType.LocalConst) && (nodeType != NetNodeType.ImplicitVariable))
            {
                goto Label_0028;
            }
        Label_0025:
            return 30;
        Label_0028:
            attribute = node.FindAttribute(NetNodeType.Modifier.ToString());
            if ((attribute != null) && ((str = ((string) attribute.Value).ToLower()) != null))
            {
                if (str == "private")
                {
                    return 10;
                }
                if (str == "protected")
                {
                    return 20;
                }
                if (str == "public")
                {
                    return 30;
                }
            }
            return 0;
        }

        public override object GetSpecialMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out CodeCompletionScope scope)
        {
            name = string.Empty;
            scope = CodeCompletionScope.None;
            object member = null;
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.VariableInitializer:
                    if (node.Parent == null)
                    {
                        break;
                    }
                    return this.GetVariableMemberType(node.Parent, node, ref name, ref position, out scope);

                case NetNodeType.CaseStatement:
                {
                    if ((node.Parent == null) || (node.Parent.NodeType != 0x7d))
                    {
                        break;
                    }
                    ISyntaxNode node5 = node.Parent.ChildList[0];
                    member = this.GetExpressionType(node5.Position, node5, node, out scope);
                    if (member == null)
                    {
                        break;
                    }
                    member = this.GetEnumType(member);
                    if (member == null)
                    {
                        break;
                    }
                    scope = CodeCompletionScope.ShortType | CodeCompletionScope.TypeName | CodeCompletionScope.Static;
                    return member;
                }
                case NetNodeType.Field:
                case NetNodeType.LocalVariable:
                case NetNodeType.ImplicitVariable:
                    return this.GetVariableMemberType(node, node, ref name, ref position, out scope);

                case NetNodeType.ArgumentList:
                {
                    int num;
                    int num2;
                    ISyntaxNode node6 = GetInvocationNode(text, node, position, out num, out num2);
                    if ((node6 != null) && node6.HasChildren)
                    {
                        ISyntaxNode node7 = node6.ChildList[0];
                        member = this.DoFindDeclaration(text, node7, node, node7.Position, num2);
                        if ((member != null) && (num >= 0))
                        {
                            member = this.GetParameterType(member, node, num);
                            if (member != null)
                            {
                                member = this.GetEnumType(member);
                                if (member != null)
                                {
                                    scope = CodeCompletionScope.ShortType | CodeCompletionScope.TypeName | CodeCompletionScope.Static;
                                    return member;
                                }
                            }
                        }
                    }
                    break;
                }
                case NetNodeType.AssignmentExpression:
                    if (!node.HasChildren)
                    {
                        break;
                    }
                    if (node.Name == "+=")
                    {
                        ISyntaxNode node3 = node.ChildList[0];
                        member = this.GetExpressionType(node3.Position, node3, node, out scope);
                        if ((member is System.Type) && (((System.Type) member).BaseType == typeof(MulticastDelegate)))
                        {
                            ISyntaxNode declarationNode = GetDeclarationNode(node);
                            name = (declarationNode != null) ? (declarationNode.Name + "_" + node3.Name) : node3.Name;
                            scope = CodeCompletionScope.Delegate;
                            return member;
                        }
                        break;
                    }
                    if (!(node.Name == "="))
                    {
                        break;
                    }
                    return this.GetEnumMemberType(node, out scope, true);

                case NetNodeType.ObjectCreationExpression:
                    if ((position.Y != node.Position.Y) || (position.X <= (node.Position.X + node.Name.Length)))
                    {
                        break;
                    }
                    member = this.GetTypeOfNode(node, node, ref name, ref position, false, true, out scope);
                    if (member != null)
                    {
                        ISyntaxNode parent = node.Parent;
                        while (((parent != null) && (parent.NodeType != 15)) && ((parent.NodeType != 0xaf) && (parent.NodeType != 0x86)))
                        {
                            parent = parent.Parent;
                        }
                        if (parent == null)
                        {
                            return member;
                        }
                        if ((parent.NodeType == 15) || (parent.NodeType == 0xaf))
                        {
                            string nodeDataType = this.GetNodeDataType(parent, node);
                            if (nodeDataType != string.Empty)
                            {
                                name = nodeDataType;
                            }
                            return member;
                        }
                        if (parent.HasChildren)
                        {
                            object obj3 = this.GetExpressionType(parent.ChildList[0].Position, parent.ChildList[0], node, out scope);
                            if (obj3 != null)
                            {
                                name = obj3.ToString();
                            }
                        }
                    }
                    return member;

                case NetNodeType.EqualityOrAssignmentExpression:
                    return this.GetEnumMemberType(node, out scope, false);
            }
            return null;
        }

        public static ISyntaxNode GetStatementNode(ISyntaxNode node)
        {
            while (node != null)
            {
                if (IsStatementNode(node))
                {
                    return node;
                }
                node = node.Parent;
            }
            return null;
        }

        protected static Point GetTextRange(int index, int len, IRange range)
        {
            Point empty = Point.Empty;
            if (range.StartPoint.Y == index)
            {
                empty.X = range.StartPoint.X;
            }
            else if (range.StartPoint.Y > index)
            {
                empty.X = len;
            }
            empty.Y = len;
            if (range.EndPoint.Y == index)
            {
                empty.Y = range.EndPoint.X;
            }
            else if (range.EndPoint.Y < index)
            {
                empty.Y = 0;
            }
            empty.X = Math.Min(Math.Max(empty.X, 0), len);
            empty.Y = Math.Min(Math.Max(empty.Y, 0), len);
            if (empty.X < empty.Y)
            {
                return empty;
            }
            return Point.Empty;
        }

        public virtual object GetTypeByName(string type)
        {
            return null;
        }

        protected virtual object GetTypeByName(ISyntaxNode node, string name)
        {
            return null;
        }

        protected virtual object GetTypeOfNode(ISyntaxNode node, ISyntaxNode refNode, ref string name, ref Point position, bool localVar, bool checkBlock, out CodeCompletionScope scope)
        {
            Point point = position;
            bool isArray = false;
            string typeName = this.GetNodeDataType(node, refNode, ref point, ref isArray);
            return this.GetTypeOfNode(node, typeName, point, ref name, ref position, localVar, checkBlock, out scope, isArray);
        }

        protected virtual object GetTypeOfNode(ISyntaxNode node, string typeName, Point typePos, ref string name, ref Point position, bool localVar, bool checkBlock, out CodeCompletionScope scope, bool isArray)
        {
            scope = CodeCompletionScope.None;
            if (typeName != string.Empty)
            {
                name = typeName;
                string subname = string.Empty;
                Point pt = typePos;
                if (this.GetQualifiedName(ref name, ref subname, ref pt, position, false))
                {
                    object obj2 = null;
                    if (pt.Y >= 0)
                    {
                        obj2 = this.GetQualifiedType(node, position, name, out scope, isArray);
                        if (obj2 != null)
                        {
                            position = pt;
                        }
                        name = subname;
                        return obj2;
                    }
                    obj2 = this.GetBlockNodeType(node, ref position, !localVar, localVar, out scope);
                    position = typePos;
                    return obj2;
                }
            }
            if (!localVar)
            {
                name = string.Empty;
                return this.GetBlockNodeType(node, ref position, checkBlock, false, out scope);
            }
            return null;
        }

        protected virtual string GetUsingName(ISyntaxNode node)
        {
            ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.UsingAlias.ToString());
            if (attribute == null)
            {
                return node.Name;
            }
            return (string) attribute.Value;
        }

        protected virtual object GetUsingType(ISyntaxNode node, ref string name, ref Point position, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.None;
            name = node.Name;
            string subname = string.Empty;
            Point pt = node.Position;
            if (!this.GetQualifiedName(ref name, ref subname, ref pt, position, false) || (pt.Y < 0))
            {
                return null;
            }
            object obj2 = this.GetNamespace(name, null);
            name = subname;
            if (obj2 != null)
            {
                position = pt;
            }
            return obj2;
        }

        protected virtual ISyntaxNode GetValidNode(ISyntaxNode node)
        {
            while ((node.Parent != null) && (node.NodeType == 40))
            {
                node = node.Parent;
            }
            return node;
        }

        protected object GetVariableMemberType(ISyntaxNode node, ISyntaxNode refNode, ref string name, ref Point position, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.None;
            if (node.HasAttributes)
            {
                foreach (ISyntaxAttribute attribute in node.Attributes)
                {
                    if (attribute.Name == NetNodeType.TypeModifier.ToString())
                    {
                        if ((attribute.Position.Y < position.Y) || ((attribute.Position.Y == position.Y) && (attribute.Position.X < position.X)))
                        {
                            object member = this.GetChildType(node, refNode, node.Position, out scope);
                            if (member != null)
                            {
                                member = this.GetEnumType(member);
                                if (member != null)
                                {
                                    scope = CodeCompletionScope.ShortType | CodeCompletionScope.TypeName | CodeCompletionScope.Static;
                                    return member;
                                }
                            }
                            return this.GetTypeOfNode(node, refNode, ref name, ref position, false, (node.NodeType == 15) || (node.NodeType == 0xaf), out scope);
                        }
                        break;
                    }
                }
            }
            return null;
        }

        protected virtual object GetWithStatementExpression(ISyntaxNode node, ISyntaxNode refNode, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.None;
            if (!node.HasChildren)
            {
                return null;
            }
            return this.GetExpressionType(node.ChildList[0].Position, node.ChildList[0], refNode, out scope);
        }

        protected virtual string GetXmlComment(ISyntaxNode node)
        {
            string str = (node.NodeType == 0x3f) ? node.Name : string.Empty;
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    str = str + this.GetXmlComment(node2);
                }
            }
            return str;
        }

        public static ISyntaxNode GetXmlCommentNode(ISyntaxNode node)
        {
            while (node != null)
            {
                if (node.NodeType == 0x39)
                {
                    return node;
                }
                if (!IsXmlCommentNode(node))
                {
                    break;
                }
                node = node.Parent;
            }
            return null;
        }

        public static ISyntaxNode GetXmlReferenceNode(ISyntaxNode node)
        {
            if (IsXmlCommentNode(node))
            {
                node = GetXmlCommentNode(node);
            }
            if (((node != null) && (node.Parent != null)) && (node.NodeType == 0x39))
            {
                int num = node.Index + 1;
                if (num < node.Parent.ChildCount)
                {
                    ISyntaxNode node2 = node.Parent.ChildList[num];
                    if (IsDeclarationReference(node2, false))
                    {
                        return node2;
                    }
                }
            }
            return null;
        }

        protected virtual bool HasSnippetMembers(string language)
        {
            return (language == "vb");
        }

        protected virtual int IndexOfNamespace(IList<INetNamespace> namespaces, string name)
        {
            for (int i = 0; i < namespaces.Count; i++)
            {
                INetNamespace namespace2 = namespaces[i];
                if (string.Compare(namespace2.Namespace, name, !this.CaseSensitive) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool IsBinaryExpressionNode(ISyntaxNode node)
        {
            NetNodeType nodeType = (NetNodeType) node.NodeType;
            if (nodeType <= NetNodeType.NullCoalescingExpression)
            {
                switch (nodeType)
                {
                    case NetNodeType.ConditionalExpression:
                    case NetNodeType.AssignmentExpression:
                    case NetNodeType.ConditionalOrExpression:
                    case NetNodeType.ConditionalAndExpression:
                    case NetNodeType.InclusiveOrExpression:
                    case NetNodeType.ExclusiveOrExpression:
                    case NetNodeType.AndExpression:
                    case NetNodeType.NotExpression:
                    case NetNodeType.EqualityExpression:
                    case NetNodeType.RelationalExpression:
                    case NetNodeType.AsIsExpression:
                    case NetNodeType.ShiftExpression:
                    case NetNodeType.AdditiveExpression:
                    case NetNodeType.MultiplicativeExpression:
                    case NetNodeType.NullCoalescingExpression:
                        goto Label_006D;
                }
                goto Label_006F;
            }
            if ((nodeType != NetNodeType.LambdaExpression) && (nodeType != NetNodeType.EqualityOrAssignmentExpression))
            {
                goto Label_006F;
            }
        Label_006D:
            return true;
        Label_006F:
            return false;
        }

        public static bool IsBlockNode(ISyntaxNode node)
        {
            return (node.FindAttribute(SyntaxConsts.BlockScope) != null);
        }

        public static bool IsDeclarationNode(ISyntaxNode node)
        {
            return IsDeclarationNode(ref node, false);
        }

        public static bool IsDeclarationNode(ref ISyntaxNode node, bool checkRegions)
        {
            while (node != null)
            {
                switch (node.NodeType)
                {
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                        return true;
                }
                if (!checkRegions || (node.NodeType != 40))
                {
                    break;
                }
                node = node.Parent;
            }
            return false;
        }

        protected static bool IsDeclarationReference(ISyntaxNode node, bool all)
        {
            if (IsDeclarationNode(node))
            {
                return true;
            }
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.Using:
                case NetNodeType.LocalVariable:
                case NetNodeType.FixedVariable:
                case NetNodeType.Indexer:
                case NetNodeType.Parameter:
                case NetNodeType.Attribute:
                    return all;

                case NetNodeType.Field:
                case NetNodeType.Constant:
                case NetNodeType.Method:
                case NetNodeType.Constructor:
                case NetNodeType.Destructor:
                case NetNodeType.Delegate:
                case NetNodeType.Property:
                case NetNodeType.Event:
                case NetNodeType.Operator:
                    return true;

                case NetNodeType.LocalConst:
                    return all;

                case NetNodeType.ImplicitVariable:
                    return all;
            }
            return false;
        }

        protected virtual bool IsEmbeddedDeclaration(ISyntaxNode node)
        {
            NetNodeType nodeType = (NetNodeType) node.NodeType;
            if (nodeType <= NetNodeType.ParameterList)
            {
                switch (nodeType)
                {
                    case NetNodeType.Constant:
                    case NetNodeType.LocalVariable:
                    case NetNodeType.FixedVariable:
                    case NetNodeType.Parameter:
                    case NetNodeType.ParameterList:
                        goto Label_0046;
                }
                goto Label_0048;
            }
            if (((nodeType != NetNodeType.ForInitializerStatement) && (nodeType != NetNodeType.ForEachInitializerStatement)) && (nodeType != NetNodeType.ImplicitVariable))
            {
                goto Label_0048;
            }
        Label_0046:
            return true;
        Label_0048:
            return false;
        }

        protected virtual bool IsEnumTypeRegistered()
        {
            return false;
        }

        protected static bool IsExpressionNode(ISyntaxNode node)
        {
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.AddressofExpression:
                case NetNodeType.GetTypeExpression:
                case NetNodeType.EqualityOrAssignmentExpression:
                case NetNodeType.ImpExpression:
                case NetNodeType.LambdaExpression:
                case NetNodeType.QueryExpression:
                    return true;
            }
            return (((node.NodeType >= 0x84) && (node.NodeType <= 0xae)) || ((node.NodeType >= 200) && (node.NodeType <= 0xcc)));
        }

        public static bool IsInvocationNode(ISyntaxNode node)
        {
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.CastInvocationExpression:
                case NetNodeType.CastInvocationTargetExpression:
                case NetNodeType.ObjectCreationExpression:
                case NetNodeType.ElementAccessExpression:
                case NetNodeType.InvocationExpression:
                    return true;
            }
            return false;
        }

        protected virtual bool IsMemberExpressionNode(ISyntaxNode node)
        {
            NetNodeType nodeType = (NetNodeType) node.NodeType;
            if (((nodeType != NetNodeType.Expression) && (nodeType != NetNodeType.PrimaryExpression)) && (nodeType != NetNodeType.GenericExpression))
            {
                return IsUnaryExpressionNode(node);
            }
            return true;
        }

        public static bool IsMethodNode(ISyntaxNode node)
        {
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.Method:
                case NetNodeType.Constructor:
                case NetNodeType.Destructor:
                case NetNodeType.Indexer:
                    return true;
            }
            return false;
        }

        protected virtual bool IsNodePartial(ISyntaxNode node)
        {
            if (node.HasAttributes)
            {
                foreach (ISyntaxAttribute attribute in node.AttributeList)
                {
                    if ((attribute.Name == NetNodeType.Modifier.ToString()) && (string.Compare((string) attribute.Value, "partial", !this.CaseSensitive) == 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsPrefixedUnaryExpressionNode(ISyntaxNode node)
        {
            switch (node.NodeType)
            {
                case 0x93:
                case 0x95:
                    return true;
            }
            return false;
        }

        private bool IsQualifiedChar(char ch)
        {
            if ((((ch < 'a') || (ch > 'z')) && ((ch < 'A') || (ch > 'Z'))) && (((ch < '0') || (ch > '9')) && (ch != '_')))
            {
                return (ch == '.');
            }
            return true;
        }

        protected virtual bool IsReferenceType(ISyntaxNode node)
        {
            if (IsDeclarationReference(node, true))
            {
                return true;
            }
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.OperatorType:
                case NetNodeType.Argument:
                case NetNodeType.AttributeTarget:
                case NetNodeType.ObjectCreationExpression:
                case NetNodeType.AsIsExpression:
                case NetNodeType.MemberAccessExpression:
                case NetNodeType.PointerMemberAccessExpression:
                    return true;

                case NetNodeType.PrimaryExpression:
                    return (node.Name != string.Empty);
            }
            return false;
        }

        protected virtual bool IsScopeValid(CodeCompletionScope scope, ISyntaxNode node)
        {
            bool flag = false;
            if (((scope & CodeCompletionScope.Instance) != CodeCompletionScope.None) && ((scope & CodeCompletionScope.Static) != CodeCompletionScope.None))
            {
                flag = true;
            }
            else
            {
                flag = (this.GetNodeCodeCompletionScope(node) & scope) != CodeCompletionScope.None;
            }
            bool flag2 = (scope & CodeCompletionScope.BaseType) != CodeCompletionScope.None;
            bool flag3 = (scope & CodeCompletionScope.Overrides) != CodeCompletionScope.None;
            if ((flag && ((scope & CodeCompletionScope.Private) == CodeCompletionScope.None)) && ((flag2 || flag3) || ((GetBlockNode(node) == null) && (GetMethodNode(node) == null))))
            {
                ISyntaxNode declarationNode = GetDeclarationNode(node);
                if ((declarationNode != null) && (declarationNode.Parent != null))
                {
                    switch (declarationNode.NodeType)
                    {
                        case 8:
                        case 9:
                        case 12:
                        {
                            flag = false;
                            bool flag4 = false;
                            if (node.HasAttributes)
                            {
                                foreach (ISyntaxAttribute attribute in node.AttributeList)
                                {
                                    if (attribute.Name == NetNodeType.Modifier.ToString())
                                    {
                                        if ((flag3 && flag2) && (string.Compare((string) attribute.Value, "virtual", true) == 0))
                                        {
                                            flag4 = true;
                                        }
                                        if (string.Compare((string) attribute.Value, "public", true) == 0)
                                        {
                                            flag = true;
                                        }
                                        else if ((string.Compare((string) attribute.Value, "protected", true) == 0) && ((scope & CodeCompletionScope.Protected) != CodeCompletionScope.None))
                                        {
                                            flag = true;
                                        }
                                        if (flag && !flag3)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            if (flag3 && !flag4)
                            {
                                flag = false;
                            }
                            return flag;
                        }
                        case 10:
                        case 11:
                            return flag;
                    }
                }
            }
            return flag;
        }

        public static bool IsStatementNode(ISyntaxNode node)
        {
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.LocalVariable:
                case NetNodeType.FixedVariable:
                case NetNodeType.ImplicitVariable:
                    return true;
            }
            return ((node.NodeType >= 0x40) && (node.NodeType <= 130));
        }

        public static bool IsUnaryExpressionNode(ISyntaxNode node)
        {
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.PrefixedUnaryExpression:
                case NetNodeType.CastExpression:
                case NetNodeType.BaseAccessExpression:
                case NetNodeType.ThisAccessExpression:
                case NetNodeType.MemberAccessExpression:
                case NetNodeType.PointerMemberAccessExpression:
                case NetNodeType.ElementAccessExpression:
                case NetNodeType.InvocationExpression:
                case NetNodeType.PostIncrementExpression:
                case NetNodeType.PostDecrementExpression:
                    return true;
            }
            return false;
        }

        public static bool IsVariableInitializer(ISyntaxNode node)
        {
            return (node.NodeType == 0x21);
        }

        public static bool IsXmlCommentNode(ISyntaxNode node)
        {
            switch (node.NodeType)
            {
                case 0x39:
                case 0x3a:
                case 0x3b:
                case 60:
                case 0x3d:
                case 0x3e:
                case 0x3f:
                    return true;
            }
            return false;
        }

        protected virtual void OnAllowGlobalMembersChanged()
        {
        }

        protected virtual void OnBaseClassTypeChanged()
        {
        }

        public virtual void RegisterNamespace(string nspace)
        {
            this.namespaces.Add(nspace);
        }

        public virtual void RegisterObject(string name, object obj)
        {
            this.objects[this.CaseSensitive ? name : name.ToLower()] = obj;
            this.internalObjects[name] = obj;
        }

        protected string RemoveQuotes(string s)
        {
            if (((s.Length >= 2) && ((s[0] == '"') || (s[0] == '\''))) && (s[s.Length - 1] == s[0]))
            {
                return s.Substring(1, s.Length - 2);
            }
            return s;
        }

        protected virtual bool SkipGlobalMembers(ISyntaxNode node)
        {
            if (this.allowGlobalMembers)
            {
                return IsDeclarationNode(node);
            }
            return true;
        }

        public static void SmartFormatLine(int index, string text, short[] textData, ISyntaxNodes nodes, ITextUndoList operations)
        {
            foreach (ISyntaxNode node in nodes)
            {
                NetNodeType nodeType = (NetNodeType) node.NodeType;
                if (nodeType <= NetNodeType.ArgumentList)
                {
                    switch (nodeType)
                    {
                        case NetNodeType.Field:
                            goto Label_007E;

                        case NetNodeType.LocalVariable:
                        case NetNodeType.FixedVariable:
                            goto Label_005C;

                        case NetNodeType.ParameterList:
                        case NetNodeType.ArgumentList:
                        {
                            FormatArgumentList(node, index, text, operations);
                            continue;
                        }
                    }
                    goto Label_008A;
                }
                if (nodeType == NetNodeType.ExpressionStatement)
                {
                    goto Label_007E;
                }
                if (nodeType != NetNodeType.ImplicitVariable)
                {
                    goto Label_008A;
                }
            Label_005C:
                FormatVariableDeclaration(node, index, text, operations);
                FormatExpressionStatement(node, index, text, operations);
                continue;
            Label_007E:
                FormatExpressionStatement(node, index, text, operations);
                continue;
            Label_008A:
                if (IsBinaryExpressionNode(node))
                {
                    FormatBinaryExpression(node, index, text, operations);
                }
                else if (IsUnaryExpressionNode(node))
                {
                    FormatUnaryExpression(node, index, text, operations);
                }
                else if (IsVariableInitializer(node))
                {
                    FormatVariableInitializer(node, index, text, operations);
                }
                else if (IsDeclarationNode(node) && (node.Position.Y == index))
                {
                    FormatDeclaration(node, index, text, operations);
                }
            }
        }

        public virtual bool UnregisterNamespace(string nspace)
        {
            int index = this.namespaces.IndexOf(nspace);
            if (index >= 0)
            {
                this.namespaces.RemoveAt(index);
            }
            return (index >= 0);
        }

        public virtual bool UnregisterObject(string name)
        {
            if (this.internalObjects.Contains(name))
            {
                this.internalObjects.Remove(name);
            }
            name = this.CaseSensitive ? name : name.ToLower();
            bool flag = this.objects.Contains(name);
            if (flag)
            {
                this.objects.Remove(name);
            }
            return flag;
        }

        public bool AllowGlobalMembers
        {
            get
            {
                return this.allowGlobalMembers;
            }
            set
            {
                if (this.allowGlobalMembers != value)
                {
                    this.allowGlobalMembers = value;
                    this.OnAllowGlobalMembersChanged();
                }
            }
        }

        public string BaseClassType
        {
            get
            {
                return this.baseClassType;
            }
            set
            {
                if (this.baseClassType != value)
                {
                    this.baseClassType = value;
                    this.OnBaseClassTypeChanged();
                }
            }
        }

        public ImageList Images
        {
            get
            {
                return this.internalImages;
            }
        }

        public IList<string> Namespaces
        {
            get
            {
                return this.namespaces;
            }
        }

        public Hashtable Objects
        {
            get
            {
                return this.objects;
            }
        }

        internal enum BoolEnum
        {
            True,
            False
        }

        internal class NamespaceComparer : IComparer<INetNamespace>
        {
            private ICodeCompletionRepository owner;

            public NamespaceComparer(ICodeCompletionRepository owner)
            {
                this.owner = owner;
            }

            public int Compare(INetNamespace x, INetNamespace y)
            {
                return string.Compare(x.Namespace, y.Namespace, !this.owner.CaseSensitive);
            }
        }
    }
}

