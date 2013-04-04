namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public interface ISyntaxErrors : ISortList<ISyntaxError>, IList<ISyntaxError>, ICollection<ISyntaxError>, IEnumerable<ISyntaxError>, IEnumerable
    {
        bool BlockDeleting(Rectangle rect);
        bool BlockDeleting(Rectangle rect, IComparer<ISyntaxError> comparer);
        bool FindErrorAt(Point position, bool exact, out int index, IComparer<ISyntaxError> comparer);
        bool PositionChanged(int x, int y, int deltaX, int deltaY);
        bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer<ISyntaxError> comparer);
    }
}

