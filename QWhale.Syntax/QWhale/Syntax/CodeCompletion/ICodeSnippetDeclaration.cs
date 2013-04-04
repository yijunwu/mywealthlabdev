namespace QWhale.Syntax.CodeCompletion
{
    public interface ICodeSnippetDeclaration : ICodeCompletionProviderItem
    {
        ICodeSnippetLiterals Literals { get; }

        ICodeSnippetObjects Objects { get; }
    }
}

