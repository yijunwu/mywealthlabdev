namespace QWhale.Common
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public interface ISortRange : IRange, ICloneable
    {
        int Add(ISortRange range);
        bool BlockDeleting(Rectangle rect, IRangeList list, IComparer<IRange> comparer);
        void Clear();
        bool Contains(IRange range);
        void GetRanges(IList<IRange> ranges);
        bool PositionChanged(int x, int y, int deltaX, int deltaY, IRangeList list, IComparer<IRange> comparer);

        int Index { get; }

        IRange Range { get; }

        ISortList<IRange> Ranges { get; }
    }
}

