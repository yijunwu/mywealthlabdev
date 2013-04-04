namespace QWhale.Editor.TextSource
{
    using System;
    using System.Drawing;

    public interface IBraceMatching
    {
        bool FindClosingBrace(ref Point position);
        bool FindClosingBrace(ref int x, ref int y);
        bool FindOpenBrace(ref Point position);
        bool FindOpenBrace(ref int x, ref int y);
        void HighlightBraces();
        void ResetBracesOptions();
        void ResetClosingBraces();
        void ResetOpenBraces();
        void TempHighlightBraces(Rectangle[] rects);
        void TempUnhighlightBraces();
        void UnhighlightBraces();

        QWhale.Editor.TextSource.BracesOptions BracesOptions { get; set; }

        char[] ClosingBraces { get; set; }

        char[] OpenBraces { get; set; }
    }
}

