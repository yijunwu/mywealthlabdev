namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public interface ICodeSnippetMembers : ICodeSnippetsProvider, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        ICodeSnippetMember AddSnippetMember();
        ICodeSnippetMember InsertSnippetMember(int index);

        ICodeSnippetMember this[int index] { get; }

        ICodeSnippetMember Parent { get; set; }
    }
}

