namespace QWhale.Editor.TextSource
{
    using QWhale.Syntax;
    using System;

    public interface ITextErrors
    {
        ISyntaxError GetSyntaxErrorAt(int x, int y);
        void HighlightSyntaxErrors();
        void UnhighlightSyntaxErrors();

        ISyntaxErrors SyntaxErrors { get; }
    }
}

