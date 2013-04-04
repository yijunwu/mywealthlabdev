namespace QWhale.Syntax.CodeCompletion
{
    public class VbParameterInfo : ParameterInfo
    {
        public override IListMember CreateListMember()
        {
            return new VbListMember(this, false);
        }
    }
}

