namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface ICodeSnippetReference : ICodeCompletionProviderItem
    {
        string Assembly { get; set; }

        string Url { get; set; }
    }
}

