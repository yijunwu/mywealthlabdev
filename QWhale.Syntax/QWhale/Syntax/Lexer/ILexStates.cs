namespace QWhale.Syntax.Lexer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ILexStates : IList<ILexState>, ICollection<ILexState>, IEnumerable<ILexState>, IEnumerable
    {
        ILexState AddLexState();
        ILexState FindLexState(string name);
        ILexState InsertLexState(int index);
    }
}

