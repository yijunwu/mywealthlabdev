namespace QWhale.Syntax.CodeCompletion
{
    public class VbListMembers : ListMembers
    {
        public override IListMember CreateListMember()
        {
            return new VbListMember(this, true);
        }
    }
}

