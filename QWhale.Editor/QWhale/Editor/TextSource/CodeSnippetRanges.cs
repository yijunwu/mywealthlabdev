namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class CodeSnippetRanges : SortList<ICodeSnippetRange>, ICodeSnippetRanges, IList<ICodeSnippetRange>, ICollection<ICodeSnippetRange>, IEnumerable<ICodeSnippetRange>, IEnumerable
    {
        private IComparer<ICodeSnippetRange> snippetComparer = new SnippetComparer();

        public virtual bool BlockDeleting(Rectangle rect)
        {
            int num;
            bool flag = false;
            Point start = new Point(rect.Right, rect.Bottom);
            if (base.FindLast(new CodeSnippetRange(start, start), out num, this.snippetComparer))
            {
                num++;
            }
            for (int i = Math.Min(num, base.Count - 1); i >= 0; i--)
            {
                ICodeSnippetRange range = base[i];
                if (range.StartPoint.Y < rect.Top)
                {
                    break;
                }
                if ((!this.IsEditableRangeSelected(range, rect) && SortList<ICodeSnippetRange>.InsideRange(range.StartPoint, rect)) && (SortList<ICodeSnippetRange>.InsideRange(range.EndPoint, rect, true) || range.EndPoint.Equals(new Point(rect.Right, rect.Bottom))))
                {
                    base.RemoveAt(i);
                    flag = true;
                }
            }
            if (flag && (base.Count == 1))
            {
                ICodeSnippetRange range2 = base[0];
                if ((range2.ID == string.Empty) && !range2.IsEditable)
                {
                    base.RemoveAt(0);
                }
            }
            return flag;
        }

        public bool FindSnippet(Point position, bool exact, out int index)
        {
            bool flag = base.FindLast(new CodeSnippetRange(position, position), out index, this.snippetComparer);
            if (exact || flag)
            {
                return flag;
            }
            index--;
            if ((index < 0) || (index >= base.Count))
            {
                return flag;
            }
            IRange range = base[index];
            if ((range.StartPoint.Y >= position.Y) && ((range.StartPoint.Y != position.Y) || (range.StartPoint.X > position.X)))
            {
                return flag;
            }
            if ((range.EndPoint.Y <= position.Y) && ((range.EndPoint.Y != position.Y) || (range.EndPoint.X < position.X)))
            {
                return flag;
            }
            return true;
        }

        public int GetFirstSnippet()
        {
            for (int i = 0; i < base.Count; i++)
            {
                ICodeSnippetRange range = base[i];
                if ((range.ID != string.Empty) && range.IsEditable)
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetNextSnippet(int index)
        {
            ICodeSnippetRange range = base[index];
            for (int i = index + 1; i < base.Count; i++)
            {
                ICodeSnippetRange range2 = base[i];
                if (((range2.ID != string.Empty) && (range2.ID != range.ID)) && (range2.IsEditable && this.IsFirstSnippet(i)))
                {
                    return i;
                }
            }
            for (int j = 0; j < index; j++)
            {
                ICodeSnippetRange range3 = base[j];
                if (((range3.ID != string.Empty) && range3.IsEditable) && ((range3.ID != range.ID) && this.IsFirstSnippet(j)))
                {
                    return j;
                }
            }
            return index;
        }

        public int GetPrevSnippet(int index)
        {
            ICodeSnippetRange range = base[index];
            for (int i = index - 1; i >= 0; i--)
            {
                ICodeSnippetRange range2 = base[i];
                if (((range2.ID != string.Empty) && (range2.ID != range.ID)) && (range2.IsEditable && this.IsFirstSnippet(i)))
                {
                    return i;
                }
            }
            for (int j = base.Count - 1; j > index; j--)
            {
                ICodeSnippetRange range3 = base[j];
                if (((range3.ID != string.Empty) && range3.IsEditable) && ((range3.ID != range.ID) && this.IsFirstSnippet(j)))
                {
                    return j;
                }
            }
            return index;
        }

        protected bool IsEditableRangeSelected(ICodeSnippetRange range, Rectangle rect)
        {
            return ((range.IsEditable && range.StartPoint.Equals(rect.Location)) && range.EndPoint.Equals(new Point(rect.Right, rect.Bottom)));
        }

        public bool IsFirstSnippet(ICodeSnippetRange range)
        {
            int index = base.IndexOf(range);
            return ((index >= 0) && this.IsFirstSnippet(index));
        }

        public bool IsFirstSnippet(int index)
        {
            ICodeSnippetRange range = base[index];
            for (int i = index - 1; i >= 0; i--)
            {
                if (base[i].ID == range.ID)
                {
                    return false;
                }
            }
            return true;
        }

        public bool NeedClear(Rectangle rect)
        {
            if (!this.NeedClear(rect.Top))
            {
                return this.NeedClear(rect.Bottom);
            }
            return true;
        }

        public bool NeedClear(int y)
        {
            if (base.Count == 0)
            {
                return false;
            }
            if (base[0].StartPoint.Y <= y)
            {
                return (base[base.Count - 1].EndPoint.Y < y);
            }
            return true;
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, bool preserveBounds)
        {
            int num;
            bool flag = false;
            Point start = new Point(x, y);
            if (!base.FindLast(new CodeSnippetRange(start, start), out num, this.snippetComparer))
            {
                num--;
            }
            for (int i = Math.Max(num, 0); i < base.Count; i++)
            {
                ICodeSnippetRange range = base[i];
                start = range.StartPoint;
                Point endPoint = range.EndPoint;
                if ((deltaY == 0) && (start.Y > y))
                {
                    return flag;
                }
                if (SortList<ICodeSnippetRange>.UpdatePos(x, y, deltaX, deltaY, ref start, (!preserveBounds && (range.ID != string.Empty)) && range.IsEditable))
                {
                    range.StartPoint = start;
                    flag = true;
                }
                if (SortList<ICodeSnippetRange>.UpdatePos(x, y, deltaX, deltaY, ref endPoint, (preserveBounds || (range.ID == string.Empty)) || !range.IsEditable))
                {
                    range.EndPoint = endPoint;
                    flag = true;
                }
            }
            return flag;
        }

        public void Sort()
        {
            base.Sort(this.snippetComparer);
        }

        private class SnippetComparer : IComparer<ICodeSnippetRange>
        {
            public int Compare(ICodeSnippetRange x, ICodeSnippetRange y)
            {
                Point startPoint = x.StartPoint;
                Point point2 = y.StartPoint;
                int num = startPoint.Y - point2.Y;
                if (num == 0)
                {
                    num = startPoint.X - point2.X;
                }
                return num;
            }
        }
    }
}

