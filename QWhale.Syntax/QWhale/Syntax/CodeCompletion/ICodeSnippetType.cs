namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface ICodeSnippetType : ICodeCompletionProviderItem
    {
        QWhale.Syntax.CodeCompletion.SnippetType SnippetType { get; set; }
    }
}

