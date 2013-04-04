namespace QWhale.Syntax.CodeCompletion
{
    public class CsListMembers : ListMembers
    {
        public override IListMember CreateListMember()
        {
            return new CsListMember(this, true);
        }
    }
}

