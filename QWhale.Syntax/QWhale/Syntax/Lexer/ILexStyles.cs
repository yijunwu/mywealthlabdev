namespace QWhale.Syntax.Lexer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ILexStyles : IList<ILexStyle>, ICollection<ILexStyle>, IEnumerable<ILexStyle>, IEnumerable
    {
        ILexStyle AddLexStyle();
        ILexStyle FindLexStyle(string name);
        ILexStyle InsertLexStyle(int index);
    }
}

