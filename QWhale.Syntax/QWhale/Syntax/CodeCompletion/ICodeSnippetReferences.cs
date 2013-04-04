namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICodeSnippetReferences : IList<ICodeSnippetReference>, ICollection<ICodeSnippetReference>, IEnumerable<ICodeSnippetReference>, IEnumerable
    {
        ICodeSnippetReference AddReference();
        ICodeSnippetReference InsertReference(int index);
    }
}

