namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public interface ICodeSnippets : ICodeSnippetsProvider, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        ICodeSnippet AddSnippet();
        ICodeSnippet InsertSnippet(int index);

        ICodeSnippet this[int index] { get; }
    }
}

