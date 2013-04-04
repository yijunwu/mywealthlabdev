namespace QWhale.Syntax.CodeCompletion
{
    public class JsListMembers : ListMembers
    {
        public override IListMember CreateListMember()
        {
            return new JsListMember(this, true);
        }
    }
}

