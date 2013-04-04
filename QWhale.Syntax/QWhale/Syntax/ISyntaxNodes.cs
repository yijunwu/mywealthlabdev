namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public interface ISyntaxNodes : ISortList<ISyntaxNode>, IList<ISyntaxNode>, ICollection<ISyntaxNode>, IEnumerable<ISyntaxNode>, IEnumerable
    {
        bool BlockDeleting(Rectangle rect, IComparer<ISyntaxNode> comparer);
        bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer<ISyntaxNode> comparer);
    }
}

