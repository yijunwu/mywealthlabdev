namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public interface ISyntaxAttributes : ISortList<ISyntaxAttribute>, IList<ISyntaxAttribute>, ICollection<ISyntaxAttribute>, IEnumerable<ISyntaxAttribute>, IEnumerable
    {
        bool BlockDeleting(Rectangle rect);
        bool PositionChanged(int x, int y, int deltaX, int deltaY);
        void Sort();
    }
}

