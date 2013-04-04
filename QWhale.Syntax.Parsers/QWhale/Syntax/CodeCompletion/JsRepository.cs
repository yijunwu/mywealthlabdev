namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Drawing;

    public class JsRepository : CsRepository
    {
        public JsRepository(bool caseSensitive, ISyntaxTree syntaxTree) : base(caseSensitive, syntaxTree)
        {
        }

        public override void FillMembers(ISyntaxNode node, Point position, IListMembers members, object member, string name, CodeCompletionScope scope, ref int selIndex)
        {
            base.FillMembers(node, position, members, member, name, scope & ~CodeCompletionScope.Overrides, ref selIndex);
        }
    }
}

