namespace QWhale.Editor.TextSource
{
    using System;
    using System.Drawing;

    public interface ITextSnippets
    {
        int BeginUpdateSnippet();
        int EndUpdateSnippet();
        ICodeSnippetRange GetCodeSnippetRangeAt(Point position);
        void HighlightCodeSnippets();
        void UnhighlightCodeSnippets();

        ICodeSnippetRanges CodeSnippets { get; set; }

        ICodeSnippetRange CurrentSnippet { get; set; }
    }
}

