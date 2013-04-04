namespace QWhale.Syntax.Lexer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ILexSyntaxBlocks : IList<ILexSyntaxBlock>, ICollection<ILexSyntaxBlock>, IEnumerable<ILexSyntaxBlock>, IEnumerable
    {
        ILexSyntaxBlock AddLexSyntaxBlock();
        ILexSyntaxBlock FindSyntaxBlock(string name);
        ILexSyntaxBlock InsertLexSyntaxBlock(int index);
    }
}

