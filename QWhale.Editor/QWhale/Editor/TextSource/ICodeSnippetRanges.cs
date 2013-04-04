namespace QWhale.Editor.TextSource
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public interface ICodeSnippetRanges : IList<ICodeSnippetRange>, ICollection<ICodeSnippetRange>, IEnumerable<ICodeSnippetRange>, IEnumerable
    {
        bool BlockDeleting(Rectangle rect);
        bool FindSnippet(Point position, bool exact, out int index);
        int GetFirstSnippet();
        int GetNextSnippet(int index);
        int GetPrevSnippet(int index);
        bool IsFirstSnippet(ICodeSnippetRange range);
        bool IsFirstSnippet(int index);
        bool NeedClear(Rectangle rect);
        bool NeedClear(int y);
        bool PositionChanged(int x, int y, int deltaX, int deltaY, bool preserveBounds);
        void Sort();
    }
}

