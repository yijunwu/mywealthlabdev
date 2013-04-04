namespace QWhale.Syntax.Parsers
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class NetSyntaxParser : SyntaxParser
    {
        protected ISyntaxNodes comments = new SyntaxNodes();
        protected Point prevPosition;
        protected const int reswordStyle = 2;
        protected Point savePrevPosition;
        protected const int xmlCommentStyle = 4;
        private XmlCommentParser xmlParser;

        protected virtual void AddAttribute(ISyntaxAttribute attr)
        {
            this.SyntaxTree.Current.AddAttribute(attr);
        }

        protected virtual void AddNode(ISyntaxNode node)
        {
            this.SyntaxTree.Current.AddChild(node);
        }

        protected virtual void AddXmlNode(ISyntaxNode parent, ISyntaxNode node, Point pos)
        {
            ISyntaxNode node2 = new SyntaxNode(this.ShiftPoint(node.Position, pos), node.Name, this.XmlToNetNodeType(node.NodeType)) {
                Range = { EndPoint = this.ShiftPoint(node.Range.EndPoint, pos) }
            };
            parent.AddChild(node2);
            if (node.HasAttributes)
            {
                foreach (ISyntaxAttribute attribute in node.AttributeList)
                {
                    node2.AddAttribute(new SyntaxAttribute(this.ShiftPoint(attribute.Position, pos), attribute.Name, attribute.Value));
                }
            }
            if (node.HasChildren)
            {
                foreach (ISyntaxNode node3 in node.ChildList)
                {
                    this.AddXmlNode(node2, node3, pos);
                }
            }
        }

        protected virtual bool AfterDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            NetNodeType nodeType = (NetNodeType) this.GetValidNode(this.SyntaxTree.Current).NodeType;
            switch (((NetNodeType) node.NodeType))
            {
                case NetNodeType.Method:
                case NetNodeType.Constructor:
                case NetNodeType.Destructor:
                case NetNodeType.Event:
                case NetNodeType.PropertyAccessor:
                {
                    ISyntaxAttribute attribute = node.FindAttribute(SyntaxConsts.DefinitionScope);
                    if (attribute != null)
                    {
                        node.Options |= SyntaxNodeOptions.Outlining;
                        if (nodeType == NetNodeType.Interface)
                        {
                            node.AddError(new QWhale.Syntax.SyntaxError(attribute.Position, attribute.Name, StringConsts.ErrInterfaceMemberDeclaration));
                            flag = false;
                        }
                    }
                    break;
                }
            }
            if (flag && node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    if (!this.AfterDeclaration(node2))
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        protected virtual bool BeforeDeclaration(ISyntaxNode node)
        {
            bool flag = true;
            NetNodeType nodeType = (NetNodeType) this.GetValidNode(this.SyntaxTree.Current).NodeType;
            switch (node.NodeType)
            {
                case 2:
                case 3:
                    if ((nodeType != NetNodeType.Unit) && (nodeType != NetNodeType.Namespace))
                    {
                        flag = false;
                        this.SyntaxError();
                    }
                    return flag;

                case 4:
                case 5:
                case 6:
                case 12:
                case 14:
                case 15:
                case 0x10:
                case 0x18:
                    return flag;

                case 7:
                    switch (nodeType)
                    {
                        case NetNodeType.Unit:
                        case NetNodeType.Namespace:
                            if (node.HasAttributes)
                            {
                                foreach (ISyntaxAttribute attribute in node.AttributeList)
                                {
                                    if ((attribute.Name == NetNodeType.Modifier.ToString()) && (attribute.Value is string))
                                    {
                                        switch (((string) attribute.Value))
                                        {
                                            case "internal":
                                            case "private":
                                            case "protected":
                                                this.SyntaxError(StringConsts.ErrNamespaceModifiers);
                                                return false;
                                        }
                                    }
                                }
                            }
                            return flag;
                    }
                    this.SyntaxError();
                    return flag;

                case 8:
                case 9:
                case 10:
                case 11:
                case 0x17:
                    if (nodeType == NetNodeType.Interface)
                    {
                        this.SyntaxError(node.Position, this.TokenPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
                        flag = false;
                    }
                    return flag;

                case 13:
                    if ((nodeType != NetNodeType.Namespace) && (nodeType != NetNodeType.Unit))
                    {
                        if (nodeType == NetNodeType.Interface)
                        {
                            this.SyntaxError(node.Position, this.prevPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
                            flag = false;
                        }
                        return flag;
                    }
                    this.SyntaxError(node.Position, this.prevPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
                    return false;

                case 0x11:
                case 0x15:
                case 0x19:
                    if ((nodeType != NetNodeType.Namespace) && (nodeType != NetNodeType.Unit))
                    {
                        return flag;
                    }
                    this.SyntaxError(node.Position, this.prevPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
                    return false;

                case 0x12:
                case 0x13:
                    if ((nodeType != NetNodeType.Namespace) && (nodeType != NetNodeType.Unit))
                    {
                        if (nodeType == NetNodeType.Interface)
                        {
                            this.SyntaxError(node.Position, this.TokenPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
                            flag = false;
                        }
                        return flag;
                    }
                    this.SyntaxError(node.Position, this.TokenPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
                    return false;

                case 20:
                    if (nodeType != NetNodeType.Interface)
                    {
                        if (nodeType == NetNodeType.Struct)
                        {
                            this.SyntaxError();
                            flag = false;
                        }
                        return flag;
                    }
                    this.SyntaxError(node.Position, this.TokenPosition, node.Name, StringConsts.ErrInterfaceDeclaration);
                    return false;

                case 0x16:
                    if ((nodeType != NetNodeType.Namespace) && (nodeType != NetNodeType.Unit))
                    {
                        return flag;
                    }
                    this.SyntaxError(node.Position, this.TokenPosition, node.Name, StringConsts.ErrNamespaceDeclaration);
                    return false;
            }
            return flag;
        }

        protected virtual bool ClearStack()
        {
            bool flag = true;
            ISyntaxNode current = this.SyntaxTree.Current;
            while (current != this.SyntaxTree.Root)
            {
                this.SyntaxError(current.Position, new Point(current.Position.X + 1, current.Position.Y), current.Name, StringConsts.ErrEndOfFileFound);
                current.Range.EndPoint = this.prevPosition;
                this.SyntaxTree.Pop();
                current = this.SyntaxTree.Current;
                flag = false;
            }
            return flag;
        }

        public override void CodeCompletion(string text, short[] textData, Point position, CodeCompletionArgs e)
        {
            CodeCompletionType completeComment = CodeCompletionType.CompleteComment;
            if (completeComment == CodeCompletionType.CompleteComment)
            {
                
                {
                    string xmlComment = this.GetXmlComment();
                    if ((xmlComment != string.Empty) && this.IsCommentEndAt(text, xmlComment, position.X))
                    {
                        ISyntaxNode nodeForCodeCompletion;
                        if (e.NeedReparse)
                        {
                            this.ReparseBlock(position, text, out nodeForCodeCompletion, completeComment);
                        }
                        else
                        {
                            nodeForCodeCompletion = this.GetNodeForCodeCompletion(position);
                        }
                        if (nodeForCodeCompletion != null)
                        {
                            string str2 = NETRepository.GetCommentTemplate(position, nodeForCodeCompletion, xmlComment);
                            if (str2 != string.Empty)
                            {
                                e.StartPosition = nodeForCodeCompletion.Position;
                                e.EndPosition = new Point(nodeForCodeCompletion.Position.X + xmlComment.Length, nodeForCodeCompletion.Position.Y);
                                e.Provider = new CommentInfo();
                                ((CommentInfo) e.Provider).Text = str2;
                                e.SelIndex = 0;
                                e.CompletionType = completeComment;
                            }
                        }
                    }
                }
            }
            base.CodeCompletion(text, textData, position, e);
        }

        protected virtual ISyntaxNode CreateExpressionNode(Point position, string name, int nodeType, ISyntaxNode refNode, bool addAttrbute)
        {
            ISyntaxNode node = new SyntaxNode(position, name, nodeType);
            if (refNode != null)
            {
                if (addAttrbute)
                {
                    node.AddAttribute(new SyntaxAttribute(position, NetNodeType.Expression.ToString(), name));
                }
                node.Range.StartPoint = refNode.Position;
                node.AddChild(refNode);
            }
            return node;
        }

        public override ICodeCompletionRepository CreateRepository()
        {
            ReflectionRepository repository = new ReflectionRepository(this.CaseSensitive, this.SyntaxTree);
            repository.RegisterDefaultAssemblies();
            return repository;
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

        protected virtual void FixupComments()
        {
            foreach (ISyntaxNode node in this.comments)
            {
                ISyntaxNode node2 = this.SyntaxTree.FindNode(node, base.pointNodeComparer);
                if (node2 != null)
                {
                    if (this.ShouldOutlineCommentNode(node2))
                    {
                        node.Options |= SyntaxNodeOptions.Outlining;
                    }
                    node2.InsertChild(node, base.nodeComparer);
                }
            }
            this.comments.Clear();
        }

        protected virtual bool FixupRegions(ISyntaxNode node)
        {
            bool flag = true;
            if (node.HasChildren)
            {
                for (int i = node.ChildList.Count - 1; i >= 0; i--)
                {
                    if (!this.FixupRegions(node.ChildList[i]))
                    {
                        flag = false;
                    }
                }
            }
            if (((node.NodeType == 40) && ((node.Options & SyntaxNodeOptions.Outlining) == SyntaxNodeOptions.None)) && (node.Parent != null))
            {
                if (node.HasChildren)
                {
                    foreach (ISyntaxNode node2 in node.ChildList)
                    {
                        node.Parent.AddChild(node2);
                    }
                    node.ChildList.Clear();
                }
                node.Parent.AddError(new QWhale.Syntax.SyntaxError(node.Position, node.Name, StringConsts.ErrEndOfFileFound));
            }
            return flag;
        }

        public override ISyntaxNode GetAutoFormatNode(Point position, bool extended, out Point startPt)
        {
            return NETRepository.GetAutoFormatNode(this.SyntaxTree.FindNode(new SyntaxNode(position, string.Empty), base.pointNodeComparer), extended, out startPt);
        }

        protected virtual ISyntaxNode GetBlockNode(ISyntaxNode node, Point position)
        {
            ISyntaxNode blockNode = NETRepository.GetBlockNode(node);
            while (blockNode != null)
            {
                if (blockNode.FindAttribute(SyntaxConsts.SuppressReparsingScope) == null)
                {
                    return blockNode;
                }
                node = NETRepository.GetBlockNode(blockNode.Parent);
                if (node == null)
                {
                    return blockNode;
                }
                blockNode = node;
            }
            return blockNode;
        }

        public override CodeCompletionType GetCompletionType(char ch)
        {
            switch (ch)
            {
                case '(':
                    return CodeCompletionType.ParameterInfo;

                case '.':
                    return CodeCompletionType.ListMembers;
            }
            if (Array.IndexOf<char>(this.CodeCompletionChars, ch) >= 0)
            {
                return CodeCompletionType.ListMembers;
            }
            return base.GetCompletionType(ch);
        }

        protected virtual ISyntaxNode GetLastChild(ISyntaxNode node)
        {
            if (node.HasChildren)
            {
                return this.GetLastChild(node.ChildList[node.ChildList.Count - 1]);
            }
            return node;
        }

        protected virtual ISyntaxNode GetValidNode(ISyntaxNode node)
        {
            while ((node.Parent != null) && (node.NodeType == 40))
            {
                node = node.Parent;
            }
            return node;
        }

        public virtual string GetXmlComment()
        {
            return string.Empty;
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

        protected virtual string GetXmlDescription(ISyntaxNode node, string tagName, string paramName, string paramValue)
        {
            if (node.Parent != null)
            {
                int num = node.Index - 1;
                if (num >= 0)
                {
                    node = node.Parent.ChildList[num];
                    if (node.NodeType == 0x39)
                    {
                        node = this.FindXmlNode(node, tagName, paramName, paramValue);
                        if (node != null)
                        {
                            return this.GetXmlComment(node);
                        }
                    }
                }
            }
            return string.Empty;
        }

        protected override void InitStyles()
        {
            this.InitDefaultStyles();
        }

        protected virtual bool IsComment(int token)
        {
            return false;
        }

        protected virtual bool IsCommentEndAt(string text, string comment, int position)
        {
            string str = text.TrimEnd(new char[0]);
            return ((position == str.Length) && str.EndsWith(comment));
        }

        protected virtual bool IsCommentStartAt(string text, string comment, int position)
        {
            return text.TrimStart(new char[0]).StartsWith(comment);
        }

        protected virtual bool IsContentDivider(ISyntaxNode node)
        {
            switch (node.NodeType)
            {
                case 0x11:
                case 0x12:
                case 0x13:
                    return true;
            }
            return false;
        }

        public override bool IsContentDivider(int index)
        {
            ISyntaxNode root = this.SyntaxTree.Root;
            ISyntaxNode node2 = new SyntaxNode(new Point(0, index), string.Empty);
            while (root != null)
            {
                if ((root.Position.Y == index) && (root != this.SyntaxTree.Root))
                {
                    break;
                }
                root = root.FindNode(node2, base.lineNodeComparer);
                if (((root != null) && (root.Range.EndPoint.Y == index)) && this.IsContentDivider(root))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool IsDeclaration(ISyntaxNode node)
        {
            return ((node != null) && NETRepository.IsDeclarationNode(node));
        }

        protected virtual int MoveNext()
        {
            this.prevPosition = this.CurrentPosition;
            int token = this.NextToken();
            while (!this.Eof && !this.IsValidToken(token))
            {
                if (this.IsComment(this.Token))
                {
                    this.ParseComment();
                    token = this.Token;
                }
                else
                {
                    token = this.NextToken();
                }
            }
            return token;
        }

        protected virtual bool ParseBlock()
        {
            return false;
        }

        protected virtual bool ParseComment()
        {
            while (this.IsComment(this.Token))
            {
                this.NextToken();
            }
            return true;
        }

        protected virtual bool ParseDeclaration(ISyntaxNode node)
        {
            return false;
        }

        protected virtual bool ParseUnit()
        {
            bool flag = true;
            ISyntaxNode root = this.SyntaxTree.Root;
            root.NodeType = 1;
            root.Position = Point.Empty;
            flag = this.ParseUnitBody();
            this.SyntaxTree.Root.Range.EndPoint = this.prevPosition;
            if (!this.ClearStack())
            {
                flag = false;
            }
            if (!this.FixupRegions(this.SyntaxTree.Current))
            {
                flag = false;
            }
            return flag;
        }

        protected virtual bool ParseUnitBody()
        {
            return false;
        }

        protected virtual bool ParseUsingDeclaration(ISyntaxNode node)
        {
            return false;
        }

        protected virtual bool ParseUsingList(ISyntaxNode node)
        {
            return false;
        }

        protected virtual bool ParseXmlComment(ISyntaxNode node)
        {
            QWhale.Syntax.XmlParser xmlParser = this.XmlParser;
            xmlParser.Strings.Clear();
            for (int i = node.Range.StartPoint.Y; i <= node.Range.EndPoint.Y; i++)
            {
                xmlParser.Strings.Add((i == node.Range.StartPoint.Y) ? this.Strings[i].Substring(node.Range.StartPoint.X) : this.Strings[i]);
            }
            xmlParser.ReparseText();
            if (xmlParser.SyntaxTree.Root.HasChildren)
            {
                foreach (ISyntaxNode node2 in xmlParser.SyntaxTree.Root.ChildList)
                {
                    this.AddXmlNode(node, node2, node.Position);
                }
            }
            return true;
        }

        public override bool ProcessAutoComplete(string text, Point position, out string code)
        {
            string xmlComment = this.GetXmlComment();
            if ((xmlComment != string.Empty) && this.IsCommentStartAt(text, xmlComment, position.X))
            {
                ISyntaxNode node;
                this.ReparseBlock(position, text, out node, CodeCompletionType.CompleteComment);
                if ((!this.IsCommentEndAt(text, xmlComment, position.X) || (node.Range.EndPoint.Y > position.Y)) && ((node != null) && (NETRepository.GetXmlReferenceNode(node) != null)))
                {
                    code = "\r\n" + xmlComment;
                    return true;
                }
            }
            return base.ProcessAutoComplete(text, position, out code);
        }

        public virtual void RegisterAllAssemblies()
        {
            ((IReflectionRepository) this.CompletionRepository).RegisterAllAssemblies();
        }

        public virtual void RegisterAssembly(Assembly assembly)
        {
            ((IReflectionRepository) this.CompletionRepository).RegisterAssembly(assembly);
        }

        public virtual bool RegisterAssembly(string name)
        {
            return ((IReflectionRepository) this.CompletionRepository).RegisterAssembly(name);
        }

        public virtual void RegisterDefaultAssemblies()
        {
            ((IReflectionRepository) this.CompletionRepository).RegisterDefaultAssemblies();
        }

        public virtual void RegisterNamespace(string nspace)
        {
            ((IReflectionRepository) this.CompletionRepository).RegisterNamespace(nspace);
        }

        public virtual void RegisterObject(string name, object obj)
        {
            ((IReflectionRepository) this.CompletionRepository).RegisterObject(name, obj);
        }

        public virtual void RegisterType(string name, Type type)
        {
            ((IReflectionRepository) this.CompletionRepository).RegisterType(name, type);
        }

        public virtual void RegisterType(string name, Type type, bool global)
        {
            ((IReflectionRepository) this.CompletionRepository).RegisterType(name, type, global);
        }

        private string RemoveQuotes(string s)
        {
            if (((s.Length >= 2) && ((s[0] == '"') || (s[0] == '\''))) && (s[s.Length - 1] == s[0]))
            {
                return s.Substring(1, s.Length - 2);
            }
            return s;
        }

        public override bool ReparseBlock(Point position)
        {
            bool flag = false;
            this.comments.Clear();
            ISyntaxNode xmlCommentNode = this.SyntaxTree.FindNode(new SyntaxNode(position, string.Empty), base.pointNodeComparer);
            if (xmlCommentNode != null)
            {
                if (NETRepository.IsXmlCommentNode(xmlCommentNode))
                {
                    xmlCommentNode = NETRepository.GetXmlCommentNode(xmlCommentNode);
                    flag = (xmlCommentNode != null) ? this.ReparseXmlComment(xmlCommentNode) : false;
                }
                else if (xmlCommentNode.NodeType == 3)
                {
                    flag = this.ReparseUsing(xmlCommentNode);
                }
                else if (xmlCommentNode.NodeType == 2)
                {
                    flag = this.ReparseUsingList(xmlCommentNode);
                }
                else if (NETRepository.IsDeclarationNode(ref xmlCommentNode, true))
                {
                    flag = this.ReparseDeclaration(xmlCommentNode);
                }
                else if (xmlCommentNode.NodeType == 1)
                {
                    ISyntaxNode node = xmlCommentNode.FindNode(2);
                    if (node != null)
                    {
                        flag = this.ReparseUsingList(node);
                    }
                }
                else
                {
                    xmlCommentNode = this.GetBlockNode(xmlCommentNode, position);
                    flag = (xmlCommentNode != null) ? this.ReparseBlock(xmlCommentNode, position) : false;
                }
            }
            this.FixupComments();
            return flag;
        }

        protected virtual bool ReparseBlock(ISyntaxNode node, Point position)
        {
            ISyntaxAttribute attribute = node.FindAttribute(SyntaxConsts.BlockScope);
            if (attribute == null)
            {
                return false;
            }
            this.Reset(attribute.Position.Y, attribute.Position.X, 0);
            node.ClearAfter(attribute.Position);
            this.SyntaxTree.Push(node);
            try
            {
                this.MoveNext();
                this.ParseBlock();
                if (node.Parent != null)
                {
                    ISyntaxAttribute attribute2;
                    attribute = node.FindAttribute(SyntaxConsts.DefinitionScope);
                    if (attribute != null)
                    {
                        attribute2 = node.Parent.FindAttribute(SyntaxConsts.DefinitionScope);
                        if (attribute2 != null)
                        {
                            attribute2.Position = attribute.Position;
                        }
                    }
                    attribute = node.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
                    if (attribute != null)
                    {
                        attribute2 = node.Parent.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
                        if (attribute2 != null)
                        {
                            attribute2.Position = attribute.Position;
                        }
                    }
                }
            }
            finally
            {
                this.SyntaxTree.Pop();
            }
            return true;
        }

        protected virtual bool ReparseDeclaration(ISyntaxNode node)
        {
            this.Reset(node.Position.Y, node.Position.X, 0);
            node.Clear();
            this.MoveNext();
            this.ParseDeclaration(node);
            return true;
        }

        public override void ReparseText()
        {
            this.Reset();
            this.MoveNext();
            this.ParseUnit();
            this.FixupComments();
            base.ReparseText();
        }

        protected virtual bool ReparseUsing(ISyntaxNode node)
        {
            this.Reset(node.Position.Y, node.Position.X, 0);
            node.Clear();
            this.MoveNext();
            this.ParseUsingDeclaration(node);
            return true;
        }

        protected virtual bool ReparseUsingList(ISyntaxNode node)
        {
            this.Reset(node.Position.Y, node.Position.X, 0);
            node.Clear();
            this.MoveNext();
            this.ParseUsingList(node);
            return true;
        }

        protected virtual bool ReparseXmlComment(ISyntaxNode node)
        {
            this.Reset(node.Position.Y, node.Position.X, 0);
            node.Clear();
            this.MoveNext();
            this.ParseXmlComment(node);
            this.comments.Clear();
            return true;
        }

        public override void Reset()
        {
            base.Reset();
            this.comments.Clear();
        }

        public override void ResetAutoIndentChars()
        {
            this.AutoIndentChars = SyntaxParserConsts.DefaultNetAutoIndentChars.ToCharArray();
        }

        public override void ResetCodeCompletionChars()
        {
            this.CodeCompletionChars = SyntaxParserConsts.DefaultNetCodeCompletionChars.ToCharArray();
        }

        public override void ResetCodeCompletionStopChars()
        {
            this.CodeCompletionStopChars = SyntaxParserConsts.DefaultNetCodeCompletionStopChars.ToCharArray();
        }

        public override void ResetOptions()
        {
            this.Options = SyntaxParserConsts.DefaultNetSyntaxOptions;
        }

        public override void RestoreState(bool restore)
        {
            base.RestoreState(restore);
            if (restore)
            {
                this.prevPosition = this.savePrevPosition;
            }
        }

        public override void SaveState()
        {
            base.SaveState();
            this.savePrevPosition = this.prevPosition;
        }

        protected Point ShiftPoint(Point destPos, Point srcPos)
        {
            return new Point(destPos.X + ((destPos.Y == 0) ? srcPos.X : 0), destPos.Y + srcPos.Y);
        }

        protected virtual bool ShouldOutlineCommentNode(ISyntaxNode node)
        {
            return NETRepository.IsDeclarationNode(this.GetValidNode(node));
        }

        public override bool ShouldSerializeAutoIndentChars()
        {
            return (new string(this.AutoIndentChars) != SyntaxParserConsts.DefaultNetAutoIndentChars);
        }

        public override bool ShouldSerializeCodeCompletionChars()
        {
            return (new string(this.CodeCompletionChars) != SyntaxParserConsts.DefaultNetCodeCompletionChars);
        }

        public override bool ShouldSerializeCodeCompletionStopChars()
        {
            return (new string(this.CodeCompletionStopChars) != SyntaxParserConsts.DefaultNetCodeCompletionStopChars);
        }

        public override int SmartFormatLine(int index, string text, short[] textData, ITextUndoList operations)
        {
            ISyntaxNodes nodes = new SyntaxNodes();
            this.SyntaxTree.FindNodes(new SyntaxNode(new Point(0, index), string.Empty), base.lineNodeComparer, nodes);
            int num = this.GetSmartIndent(nodes, index, false);
            if ((this.Options & SyntaxOptions.FormatSpaces) != SyntaxOptions.None)
            {
                NETRepository.SmartFormatLine(index, text, textData, nodes, operations);
            }
            return num;
        }

        protected virtual void SyntaxError()
        {
            if (base.Stack.Count == 0)
            {
                this.SyntaxError(StringConsts.ErrSyntax);
            }
        }

        protected virtual void SyntaxError(string error)
        {
            if (base.Stack.Count == 0)
            {
                this.SyntaxTree.Current.AddError(new QWhale.Syntax.SyntaxError(this.TokenPosition, this.TokenString, error));
            }
        }

        protected virtual void SyntaxError(Point position, Point endPos, string name, string error)
        {
            if (base.Stack.Count == 0)
            {
                ISyntaxError err = new QWhale.Syntax.SyntaxError(position, name, error) {
                    Range = { EndPoint = endPos }
                };
                this.SyntaxTree.Current.AddError(err);
            }
        }

        public virtual bool UnregisterAssembly(Assembly assembly, bool removeReferences)
        {
            return ((IReflectionRepository) this.CompletionRepository).UnregisterAssembly(assembly, removeReferences);
        }

        public virtual bool UnregisterAssembly(string name, bool removeReferences)
        {
            return ((IReflectionRepository) this.CompletionRepository).UnregisterAssembly(name, removeReferences);
        }

        public virtual bool UnregisterNamespace(string nspace)
        {
            return ((IReflectionRepository) this.CompletionRepository).UnregisterNamespace(nspace);
        }

        public virtual bool UnregisterObject(string name)
        {
            return ((IReflectionRepository) this.CompletionRepository).UnregisterObject(name);
        }

        public virtual bool UnregisterType(string name)
        {
            return ((IReflectionRepository) this.CompletionRepository).UnregisterType(name);
        }

        protected int XmlToNetNodeType(int nodeType)
        {
            return (0x39 + nodeType);
        }

        [Description("Gets or sets a boolean value that indicates whether \"NetSyntaxParser\" should perform case-sensitive analysis of its content.")]
        public override bool CaseSensitive
        {
            get
            {
                return true;
            }
        }

        protected QWhale.Syntax.XmlParser XmlParser
        {
            get
            {
                if (this.xmlParser == null)
                {
                    this.xmlParser = new XmlCommentParser();
                    this.xmlParser.Strings = new StringList();
                }
                return this.xmlParser;
            }
        }
    }
}

