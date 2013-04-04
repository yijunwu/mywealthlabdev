namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public class SyntaxNodes : SortList<ISyntaxNode>, ISyntaxNodes, ISortList<ISyntaxNode>, IList<ISyntaxNode>, ICollection<ISyntaxNode>, IEnumerable<ISyntaxNode>, IEnumerable
    {
        public virtual bool BlockDeleting(Rectangle rect, IComparer<ISyntaxNode> comparer)
        {
            int num;
            bool flag = false;
            if (base.FindLast(new SyntaxNode(new Point(rect.Right, rect.Bottom), string.Empty), out num, comparer))
            {
                num++;
            }
            for (int i = Math.Min(num, base.Count - 1); i >= 0; i--)
            {
                ISyntaxNode node = base[i];
                IRange range = node.Range;
                if (range.EndPoint.Y < rect.Top)
                {
                    return flag;
                }
                if (SortList<ISyntaxNode>.InsideRange(range.StartPoint, rect) && SortList<ISyntaxNode>.InsideRange(new Point(Math.Max(range.EndPoint.X - 1, 0), range.EndPoint.Y), rect))
                {
                    base.RemoveAt(i);
                    flag = true;
                }
                else
                {
                    node.BlockDeleting(rect, comparer);
                }
            }
            return flag;
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer<ISyntaxNode> comparer)
        {
            int num;
            bool flag = false;
            if (!base.FindLast(new SyntaxNode(new Point(x, y), string.Empty), out num, comparer))
            {
                num--;
            }
            for (int i = Math.Max(num, 0); i < base.Count; i++)
            {
                ISyntaxNode node = base[i];
                IRange range = node.Range;
                Point startPoint = range.StartPoint;
                if ((deltaY == 0) && (startPoint.Y > y))
                {
                    return flag;
                }
                if (QWhale.Common.Range.UpdatePos(x, y, deltaX, deltaY, ref startPoint, false))
                {
                    range.StartPoint = startPoint;
                    flag = true;
                }
                startPoint = range.EndPoint;
                if (QWhale.Common.Range.UpdatePos(x, y, deltaX, deltaY, ref startPoint, true))
                {
                    range.EndPoint = startPoint;
                    flag = true;
                }
                if (node.PositionChanged(x, y, deltaX, deltaY, comparer))
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
}

