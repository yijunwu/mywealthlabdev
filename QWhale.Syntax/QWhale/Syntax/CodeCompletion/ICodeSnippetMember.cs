namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface ICodeSnippetMember : ICodeCompletionProviderItem
    {
        string EditPath { get; }

        int ImageIndex { get; set; }

        ICodeSnippetMembers Members { get; }

        string Name { get; set; }

        ICodeSnippetMembers Parent { get; set; }

        string Path { get; set; }

        ICodeSnippets Snippets { get; }

        ICodeSnippetsProvider SnippetsAndMembers { get; }
    }
}

