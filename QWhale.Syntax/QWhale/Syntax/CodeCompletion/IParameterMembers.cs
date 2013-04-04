namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IParameterMembers : IList<IParameterMember>, ICollection<IParameterMember>, IEnumerable<IParameterMember>, IEnumerable
    {
        IParameterMember AddParameterMember();
        IParameterMember InsertParameterMember(int index);
    }
}

