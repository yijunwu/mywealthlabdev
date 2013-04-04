namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using QWhale.Syntax.Parsers;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class VbScriptRepository : ReflectionRepository
    {
        public VbScriptRepository(bool caseSensitive, ISyntaxTree syntaxTree) : base(caseSensitive, syntaxTree)
        {
        }

        protected override void AddUnitMember(IListMembers members, ISyntaxNode node, string filter, CodeCompletionScope scope)
        {
            base.AddUnitMember(members, node, filter, scope);
            if ((node.Options & SyntaxNodeOptions.CodeCompletion) != SyntaxNodeOptions.None)
            {
                this.FillMember(members, node, filter, scope);
            }
            if ((node.NodeType == 8) && node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.ChildList)
                {
                    this.AddUnitMember(members, node2, filter, scope);
                }
            }
        }

        protected override void FillMembers(IListMembers members, IList<INetNamespace> namespaces)
        {
        }

        protected override void FillMembers(IListMembers members, IList<ISyntaxNode> types, ISyntaxNode node, Point position, string filter, CodeCompletionScope scope, ref int selIndex)
        {
            ISyntaxNode declarationNode = NETRepository.GetDeclarationNode(node);
            ISyntaxNode node3 = node;
            while (node != null)
            {
                if (NETRepository.IsDeclarationNode(node) || (node.NodeType == 1))
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
                        bool flag = NETRepository.IsBlockNode(declarationNode);
                        foreach (ISyntaxNode node4 in declarationNode.ChildList)
                        {
                            if ((node4 != node3) && ((!flag || (node4.Position.Y < position.Y)) || ((node4.Position.Y == position.Y) && (node4.Position.X < position.X))))
                            {
                                this.FillMembers(members, node4, new Point(0x7fffffff, 0x7fffffff), filter, scope, true);
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
                            if (((declaration != null) && this.IsNodePartial(declaration)) && declaration.HasChildren)
                            {
                                foreach (ISyntaxNode node8 in declaration.ChildList)
                                {
                                    this.FillMembers(members, node8, new Point(0x7fffffff, 0x7fffffff), filter, scope, false);
                                }
                                this.FillBaseTypeMembers(members, types, declaration, filter, scope, ref selIndex);
                            }
                        }
                    }
                    if (node != this.SyntaxTree.Root)
                    {
                        this.FillBaseTypeMembers(members, types, declarationNode, filter, scope, ref selIndex);
                    }
                }
                if (node == declarationNode)
                {
                    return;
                }
                node3 = null;
                declarationNode = declarationNode.Parent;
            }
        }

        protected virtual ISyntaxNode FindAssignmentOrSetNode(ISyntaxNode node, string name, Point position, bool recursive)
        {
            if (!this.IsAssignmentOrSetNode(node, name))
            {
                while (node != null)
                {
                    if (node.HasChildren)
                    {
                        foreach (ISyntaxNode node2 in node.ChildList)
                        {
                            if ((node2.Position.Y < position.Y) || ((node2.Position.Y == position.Y) && (node2.Position.X < position.X)))
                            {
                                ISyntaxNode node3 = this.FindAssignmentOrSetNode(node2, name, position, false);
                                if (node3 != null)
                                {
                                    return node3;
                                }
                            }
                        }
                    }
                    if ((node.FindAttribute(SyntaxConsts.DeclarationScope) != null) || !recursive)
                    {
                        break;
                    }
                    node = node.Parent;
                }
            }
            else
            {
                return node;
            }
            return null;
        }

        protected override ISyntaxNode GetDeclaration(ISyntaxTree tree, ISyntaxNode node)
        {
            if (node == this.SyntaxTree.Root)
            {
                return tree.Root;
            }
            return base.GetDeclaration(tree, node);
        }

        protected virtual string GetExpressionType(ISyntaxNode node)
        {
            Point point;
            return GetExpressionType(node, out point);
        }

        public static string GetExpressionType(ISyntaxNode node, out Point typePos)
        {
            typePos = node.Position;
            switch (node.NodeType)
            {
                case 0x97:
                case 0x9a:
                case 0x9b:
                {
                    ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.Type.ToString());
                    if (attribute == null)
                    {
                        break;
                    }
                    typePos = attribute.Position;
                    return (attribute.Value.ToString() + ((node.NodeType == 0x9a) ? "[]" : string.Empty));
                }
                case 0x99:
                    if (node is PrimaryExpressionNode)
                    {
                        switch (((VbScriptLexerToken) ((PrimaryExpressionNode) node).Token))
                        {
                            case VbScriptLexerToken.False:
                            case VbScriptLexerToken.True:
                                return VbScriptLexerToken.Boolean.ToString();

                            case VbScriptLexerToken.Integer_Literal:
                                return VbScriptLexerToken.Integer.ToString();

                            case VbScriptLexerToken.Float_Literal:
                                return VbScriptLexerToken.Float.ToString();

                            case VbScriptLexerToken.Double_Literal:
                                return VbScriptLexerToken.Double.ToString();

                            case VbScriptLexerToken.Date_Literal:
                                return VbScriptLexerToken.DateTime.ToString();

                            case VbScriptLexerToken.Character_Literal:
                                return VbScriptLexerToken.Char.ToString();

                            case VbScriptLexerToken.String_Literal:
                                return VbScriptLexerToken.String.ToString();
                        }
                    }
                    break;
            }
            return string.Empty;
        }

        public override object GetMethodType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out int paramIndex, out int paramCount, out CodeCompletionScope scope)
        {
            object obj2 = base.GetMethodType(text, node, ref name, ref position, ref endPos, out paramIndex, out paramCount, out scope);
            if ((obj2 == null) && node.HasChildren)
            {
                ISyntaxNode node2 = node.ChildList[0];
                if (node2.HasChildren)
                {
                    node2 = node2.ChildList[0];
                }
                if (node2.NodeType == 0x99)
                {
                    obj2 = this.GetMemberType(text, node2, ref name, ref position, ref endPos, out scope);
                }
            }
            return obj2;
        }

        protected override string GetNodeDataType(ISyntaxNode node, ISyntaxNode refNode, ref Point position, ref bool isArray)
        {
            ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.Type.ToString());
            isArray = false;
            if ((attribute == null) && node.HasChildren)
            {
                foreach (ISyntaxNode node2 in node.Childs)
                {
                    attribute = node2.FindAttribute(NetNodeType.Type.ToString());
                    if (attribute != null)
                    {
                        break;
                    }
                }
            }
            if (attribute != null)
            {
                isArray = node.FindAttribute(NetNodeType.ArraySpecifier.ToString()) != null;
                position = attribute.Position;
                return attribute.Value.ToString();
            }
            node = (refNode != null) ? this.FindAssignmentOrSetNode(refNode, node.Name, refNode.Position, true) : null;
            if (node != null)
            {
                switch (((NetNodeType) node.NodeType))
                {
                    case NetNodeType.AssignmentExpression:
                        if (node.ChildCount < 2)
                        {
                            return string.Empty;
                        }
                        return this.GetExpressionType(node.ChildList[1]);

                    case NetNodeType.SetStatement:
                        if (!node.HasChildren)
                        {
                            return string.Empty;
                        }
                        return this.GetExpressionType(node.ChildList[0]);
                }
            }
            return string.Empty;
        }

        protected override bool HasSnippetMembers(string language)
        {
            return (language == "vbscript");
        }

        protected virtual bool IsAssignmentOrSetNode(ISyntaxNode node, string name)
        {
            NetNodeType nodeType = (NetNodeType) node.NodeType;
            if (nodeType != NetNodeType.AssignmentExpression)
            {
                if (nodeType != NetNodeType.SetStatement)
                {
                    return false;
                }
            }
            else
            {
                return (string.Compare(node.ChildList[0].Name, name, true) == 0);
            }
            ISyntaxAttribute attribute = node.FindAttribute(NetNodeType.Attribute.ToString());
            return ((attribute != null) && (string.Compare(attribute.Value.ToString(), name, true) == 0));
        }

        protected override bool IsNodePartial(ISyntaxNode node)
        {
            if (!base.IsNodePartial(node))
            {
                return (node.Parent == null);
            }
            return true;
        }

        protected override bool SkipGlobalMembers(ISyntaxNode node)
        {
            return (base.SkipGlobalMembers(node) && (node != this.SyntaxTree.Root));
        }
    }
}

