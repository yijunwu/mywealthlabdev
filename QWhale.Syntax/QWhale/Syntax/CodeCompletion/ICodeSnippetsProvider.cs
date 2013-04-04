namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICodeSnippetsProvider : ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        ICodeSnippet FindByShortcut(string shortcut, bool caseSensitive);
    }
}

