namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface ICodeSnippetCode : ICodeCompletionProviderItem
    {
        void Assign(ICodeSnippetCode source);

        string Code { get; set; }

        string Delimiter { get; set; }

        string Kind { get; set; }

        string Language { get; set; }
    }
}

