namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICodeSnippetLiterals : IList<ICodeSnippetLiteral>, ICollection<ICodeSnippetLiteral>, IEnumerable<ICodeSnippetLiteral>, IEnumerable
    {
        ICodeSnippetLiteral AddLiteral();
        ICodeSnippetLiteral InsertLiteral(int index);
    }
}

