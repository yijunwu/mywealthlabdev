namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICodeSnippetImports : IList<ICodeSnippetImport>, ICollection<ICodeSnippetImport>, IEnumerable<ICodeSnippetImport>, IEnumerable
    {
        ICodeSnippetImport AddImport();
        ICodeSnippetImport InsertImport(int index);
    }
}

