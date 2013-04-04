namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class SyntaxErrors : SortList<ISyntaxError>, ISyntaxErrors, ISortList<ISyntaxError>, IList<ISyntaxError>, ICollection<ISyntaxError>, IEnumerable<ISyntaxError>, IEnumerable
    {
        public virtual bool BlockDeleting(Rectangle rect)
        {
            bool flag = false;
            for (int i = base.Count - 1; i >= 0; i--)
            {
                IRange range = base[i].Range;
                if (SortList<ISyntaxError>.InsideRange(range.StartPoint, rect) && SortList<ISyntaxError>.InsideRange(new Point(Math.Max(range.EndPoint.X - 1, 0), range.EndPoint.Y), rect))
                {
                    base.RemoveAt(i);
                    flag = true;
                }
            }
            return flag;
        }

        public virtual bool BlockDeleting(Rectangle rect, IComparer<ISyntaxError> comparer)
        {
            int num;
            bool flag = false;
            if (base.FindLast(new SyntaxError(new Point(rect.Right, rect.Bottom)), out num, comparer))
            {
                num++;
            }
            for (int i = Math.Min(num, base.Count - 1); i >= 0; i--)
            {
                ISyntaxError error = base[i];
                if (error.Position.Y < rect.Top)
                {
                    return flag;
                }
                if (SortList<ISyntaxError>.InsideRange(error.Position, rect) && SortList<ISyntaxError>.InsideRange(error.Range.EndPoint, rect, true))
                {
                    base.RemoveAt(i);
                    flag = true;
                }
            }
            return flag;
        }

        public virtual bool FindErrorAt(Point position, bool exact, out int index, IComparer<ISyntaxError> comparer)
        {
            bool flag = base.FindLast(new SyntaxError(position), out index, comparer);
            if (exact || flag)
            {
                return flag;
            }
            index--;
            if ((index < 0) || (index >= base.Count))
            {
                return flag;
            }
            ISyntaxError error = base[index];
            Point point = error.Position;
            Point endPoint = error.Range.EndPoint;
            if ((point.Y >= position.Y) && ((point.Y != position.Y) || (point.X > position.X)))
            {
                return flag;
            }
            if ((endPoint.Y <= position.Y) && ((endPoint.Y != position.Y) || (endPoint.X < position.X)))
            {
                return flag;
            }
            return true;
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY)
        {
            bool flag = false;
            foreach (ISyntaxError error in this)
            {
                IRange range = error.Range;
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
            }
            return flag;
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer<ISyntaxError> comparer)
        {
            int num;
            bool flag = false;
            if (!base.FindLast(new SyntaxError(new Point(x, y)), out num, comparer))
            {
                num--;
            }
            for (int i = Math.Max(num, 0); i < base.Count; i++)
            {
                ISyntaxError error = base[i];
                Point position = error.Position;
                Point endPoint = error.Range.EndPoint;
                if ((deltaY == 0) && (position.Y > y))
                {
                    return flag;
                }
                if (SortList<ISyntaxError>.UpdatePos(x, y, deltaX, deltaY, ref position, false))
                {
                    error.Position = position;
                    flag = true;
                }
                if (SortList<ISyntaxError>.UpdatePos(x, y, deltaX, deltaY, ref endPoint, true))
                {
                    error.Range.EndPoint = endPoint;
                    flag = true;
                }
            }
            return flag;
        }
    }
}

