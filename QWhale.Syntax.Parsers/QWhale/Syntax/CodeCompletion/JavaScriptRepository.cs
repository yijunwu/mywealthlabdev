namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Collections.Generic;

    public class JavaScriptRepository : ReflectionRepository
    {
        public JavaScriptRepository(bool caseSensitive, ISyntaxTree syntaxTree) : base(caseSensitive, syntaxTree)
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

        protected override ISyntaxNode GetDeclaration(ISyntaxTree tree, ISyntaxNode node)
        {
            if (node == this.SyntaxTree.Root)
            {
                return tree.Root;
            }
            return base.GetDeclaration(tree, node);
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

