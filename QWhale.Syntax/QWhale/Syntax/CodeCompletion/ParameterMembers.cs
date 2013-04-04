namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ParameterMembers : List<IParameterMember>, IParameterMembers, IList<IParameterMember>, ICollection<IParameterMember>, IEnumerable<IParameterMember>, IEnumerable
    {
        public virtual IParameterMember AddParameterMember()
        {
            IParameterMember item = new ParameterMember();
            base.Add(item);
            return item;
        }

        public virtual IParameterMember InsertParameterMember(int index)
        {
            IParameterMember item = new ParameterMember();
            base.Insert(index, item);
            return item;
        }
    }
}

