namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface ICodeSnippetImport : ICodeCompletionProviderItem
    {
        string Namespace { get; set; }
    }
}

