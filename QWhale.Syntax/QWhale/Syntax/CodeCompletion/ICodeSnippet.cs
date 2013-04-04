namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface ICodeSnippet : ICodeCompletionProviderItem
    {
        ICodeSnippetCode Code { get; set; }

        object CustomData { get; set; }

        ICodeSnippetDeclarations Declarations { get; }

        string Description { get; }

        ICodeSnippetHeader Header { get; set; }

        int ImageIndex { get; set; }

        ICodeSnippetImports Imports { get; }

        ICodeSnippets Parent { get; set; }

        ICodeSnippetReferences References { get; }
    }
}

