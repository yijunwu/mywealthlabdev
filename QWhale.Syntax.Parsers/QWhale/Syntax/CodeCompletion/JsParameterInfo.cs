namespace QWhale.Syntax.CodeCompletion
{
    public class JsParameterInfo : ParameterInfo
    {
        public override IListMember CreateListMember()
        {
            return new JsListMember(this, false);
        }
    }
}

