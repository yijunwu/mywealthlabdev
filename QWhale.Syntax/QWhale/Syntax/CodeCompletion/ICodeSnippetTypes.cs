namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICodeSnippetTypes : IList<ICodeSnippetType>, ICollection<ICodeSnippetType>, IEnumerable<ICodeSnippetType>, IEnumerable
    {
        ICodeSnippetType AddSnippetType();
        ICodeSnippetType InsertSnippetType(int index);
    }
}

