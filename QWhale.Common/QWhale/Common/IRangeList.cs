namespace QWhale.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IRangeList : ISortList<IRange>, IList<IRange>, ICollection<IRange>, IEnumerable<IRange>, IEnumerable
    {
        bool UpdatePosition(IRange range, int x, int y, int deltaX, int deltaY);
    }
}

