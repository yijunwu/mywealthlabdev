namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public interface ILineStyles : IList<ILineStyle>, ICollection<ILineStyle>, IEnumerable<ILineStyle>, IEnumerable
    {
        void Assign(ILineStyles source);
        bool BlockDeleting(Rectangle rect);
        int GetLineStyle(int index);
        int GetLineStyles(int line, IList<ILineStyle> list);
        bool PositionChanged(int x, int y, int deltaX, int deltaY);
        void RemoveLineStyle(int line);
        void SetLineStyle(int index, int style);
        void SetLineStyle(IRange range, int priority, int style);
        void SetLineStyle(Point position, int priority, int style);
        void SetLineStyle(int line, int priority, int style);
        void SetLineStyle(Point position, IRange range, int priority, int style);
        void ToggleLineStyle(int line, int style);
        void ToggleLineStyle(int line, int priority, int style);
    }
}

