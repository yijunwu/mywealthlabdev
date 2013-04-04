namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using QWhale.Syntax.Parsers;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class CsRepository : ReflectionRepository
    {
        public CsRepository(bool caseSensitive, ISyntaxTree syntaxTree) : base(caseSensitive, syntaxTree)
        {
        }

        protected virtual void CompleteBaseMember(object sender, ClosingEventArgs e)
        {
            if (e.Provider != null)
            {
                int selIndex = e.Provider.SelIndex;
                if ((selIndex >= 0) && (selIndex < e.Provider.Count))
                {
                    e.Text = (e.Provider[selIndex] as IListMember).GetTemplate(false);
                    e.StartPosition = new Point(0, e.StartPosition.Y);
                    e.EndPosition = new Point(0x7fffffff, e.StartPosition.Y);
                    e.UseFormat = true;
                }
            }
        }

        public override void FillMembers(ISyntaxNode node, Point position, IListMembers members, object member, string name, CodeCompletionScope scope, ref int selIndex)
        {
            base.FillMembers(node, position, members, member, name, scope, ref selIndex);
            if ((scope & CodeCompletionScope.Overrides) != CodeCompletionScope.None)
            {
                members.ClosePopup += new ClosePopupEvent(this.CompleteBaseMember);
            }
        }

        public override object GetSpecialMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out CodeCompletionScope scope)
        {
            object obj2 = base.GetSpecialMemberType(text, node, ref name, ref position, ref endPos, out scope);
            if (((obj2 == null) && NETRepository.IsDeclarationReference(node, false)) && text.Trim().EndsWith(CsLexerToken.Override.ToString().ToLower()))
            {
                scope = CodeCompletionScope.Overrides | CodeCompletionScope.Protected | CodeCompletionScope.Instance;
                return NETRepository.GetDeclarationNode(node);
            }
            return obj2;
        }
    }
}

