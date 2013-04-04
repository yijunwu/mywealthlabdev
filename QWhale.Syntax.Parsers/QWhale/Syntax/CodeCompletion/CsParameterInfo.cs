namespace QWhale.Syntax.CodeCompletion
{
    public class CsParameterInfo : ParameterInfo
    {
        public override IListMember CreateListMember()
        {
            return new CsListMember(this, false);
        }
    }
}

