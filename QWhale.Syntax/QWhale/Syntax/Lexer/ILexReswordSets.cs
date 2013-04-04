namespace QWhale.Syntax.Lexer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ILexReswordSets : IList<ILexReswordSet>, ICollection<ILexReswordSet>, IEnumerable<ILexReswordSet>, IEnumerable
    {
        ILexReswordSet AddLexReswordSet();
        int FindResword(string resword);
        ILexReswordSet InsertLexReswordSet(int index);
    }
}

