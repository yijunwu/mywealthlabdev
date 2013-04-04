namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface ICodeSnippetHeader : ICodeCompletionProviderItem
    {
        void Assign(ICodeSnippetHeader source);

        string Author { get; set; }

        string Description { get; set; }

        string Shortcut { get; set; }

        string Title { get; set; }

        ICodeSnippetTypes Types { get; }
    }
}

