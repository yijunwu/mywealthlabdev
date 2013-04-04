namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class BookMarks : SortList<IBookMark>, IBookMarks, IList<IBookMark>, ICollection<IBookMark>, IEnumerable<IBookMark>, IEnumerable
    {
        protected IComparer<IBookMark> bookMarkComparer;
        protected IComparer<IBookMark> lineComparer;
        private ITextSource owner;
        protected IComparer<IBookMark> pointComparer;
        protected IComparer rangeComparer;

        public BookMarks()
        {
            this.lineComparer = new LineComparer();
            this.pointComparer = new PointComparer();
            this.bookMarkComparer = new BookMarkComparer();
            this.rangeComparer = new RangeComparer();
        }

        public BookMarks(ITextSource owner) : this()
        {
            this.owner = owner;
        }

        public virtual void Assign(IBookMarks source)
        {
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                this.Clear();
                foreach (IBookMark mark in source)
                {
                    base.Add(this.NewBookMark(mark.Line, mark.Pos, mark.Index));
                }
                this.owner.LinesChanged(0, 0x7fffffff);
                this.owner.State |= NotifyState.BookMarkChanged;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }

        public virtual bool BlockDeleting(Rectangle rect)
        {
            int num;
            bool flag = false;
            base.FindLast(new BookMark(rect.Bottom), out num, this.lineComparer);
            if (num >= 0)
            {
                for (int i = Math.Min(num, base.Count - 1); i >= 0; i--)
                {
                    IBookMark bm = base[i];
                    if (bm.Line < rect.Top)
                    {
                        return flag;
                    }
                    if (this.ShouldDelete(bm, rect))
                    {
                        flag = true;
                        base.RemoveAt(i);
                    }
                }
            }
            return flag;
        }

        public virtual void Clear()
        {
            if (base.Count > 0)
            {
                this.owner.BeginUpdate(UpdateReason.Other);
                try
                {
                    base.Clear();
                    this.owner.LinesChanged(0, 0x7fffffff);
                    this.owner.State |= NotifyState.BookMarkChanged;
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
            else
            {
                base.Clear();
            }
        }

        public virtual void ClearAllBookMarks()
        {
            this.Clear();
        }

        public virtual void ClearAllUnnumberedBookmarks()
        {
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                for (int i = base.Count - 1; i > 0; i--)
                {
                    IBookMark item = base[i];
                    if (item.Index == 0x7fffffff)
                    {
                        this.owner.LinesChanged(item.Line, item.Line);
                        base.Remove(item);
                    }
                }
                this.owner.State |= NotifyState.BookMarkChanged;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }

        public virtual void ClearBookMark(int bookMark)
        {
            int index = this.FindByIndex(bookMark);
            if (index >= 0)
            {
                this.ClearBookMarkByIndex(index);
            }
        }

        public virtual void ClearBookMark(int line, int bookmark)
        {
            int index = 0;
            if (base.FindLast(new BookMark(line), out index, this.lineComparer))
            {
                for (int i = index; i >= 0; i--)
                {
                    if (base[i].Line != line)
                    {
                        return;
                    }
                    if (base[i].Index == bookmark)
                    {
                        this.ClearBookMarkByIndex(i);
                        return;
                    }
                }
            }
        }

        public virtual void ClearBookMarkByIndex(int index)
        {
            if (index >= 0)
            {
                this.owner.BeginUpdate(UpdateReason.Other);
                try
                {
                    int line = base[index].Line;
                    base.RemoveAt(index);
                    this.owner.State |= NotifyState.BookMarkChanged;
                    this.owner.LinesChanged(line, line);
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
        }

        public virtual void ClearBookMarks(int line)
        {
            int index = 0;
            if (base.FindLast(new BookMark(line), out index, this.lineComparer))
            {
                this.owner.BeginUpdate(UpdateReason.Other);
                try
                {
                    for (int i = index; i >= 0; i--)
                    {
                        if (base[i].Line != line)
                        {
                            break;
                        }
                        base.RemoveAt(i);
                    }
                    this.owner.State |= NotifyState.BookMarkChanged;
                    this.owner.LinesChanged(line, line);
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
        }

        public virtual IBookMark FindBookMark(int bookMark)
        {
            int num = this.FindByIndex(bookMark);
            if (num >= 0)
            {
                return base[num];
            }
            return null;
        }

        public virtual IBookMark FindBookMark(string name)
        {
            foreach (IBookMark mark in this)
            {
                if ((mark is IBookMarkEx) && (((IBookMarkEx) mark).Name == name))
                {
                    return mark;
                }
            }
            return null;
        }

        public virtual bool FindBookMark(int bookMark, out Point position)
        {
            int num = this.FindByIndex(bookMark);
            if (num >= 0)
            {
                IBookMark mark = base[num];
                position = new Point(mark.Pos, mark.Line);
                return true;
            }
            position = new Point(0, 0);
            return false;
        }

        public virtual int FindBookMark(int bookMark, int line)
        {
            int num;
            if (bookMark == 0x7fffffff)
            {
                for (num = this.FindByLine(line); ((num >= 0) && (num < base.Count)) && (base[num].Line == line); num++)
                {
                    if (base[num].Index == bookMark)
                    {
                        break;
                    }
                }
            }
            else
            {
                num = this.FindByIndex(bookMark);
            }
            if (((num >= 0) && (num < base.Count)) && (base[num].Line == line))
            {
                return num;
            }
            return -1;
        }

        protected int FindByIndex(int index)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].Index == index)
                {
                    return i;
                }
            }
            return -1;
        }

        protected int FindByLine(int line)
        {
            int num;
            if (base.FindFirst(new BookMark(line), out num, this.lineComparer))
            {
                return num;
            }
            return -1;
        }

        public virtual int GetBookMark(int line)
        {
            return this.InternalGet(line);
        }

        public virtual int GetBookMark(Point startPoint, Point endPoint)
        {
            return this.InternalGet(startPoint, endPoint, this.rangeComparer);
        }

        public virtual int GetBookMarks(Point startPoint, Point endPoint, IList<IBookMark> list)
        {
            return this.InternalGet(startPoint, endPoint, list, this.rangeComparer);
        }

        public virtual void GotoBookMark(int bookMark)
        {
            int num = this.FindByIndex(bookMark);
            if (num >= 0)
            {
                IBookMark mark = base[num];
                this.owner.BeginUpdate(UpdateReason.Navigate);
                try
                {
                    this.owner.State |= NotifyState.CenterLine | NotifyState.GotoBookMark;
                    this.owner.MoveTo(mark.Pos, mark.Line);
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
        }

        private void GotoBookMarkIndex(bool direction)
        {
            int num = -1;
            if (!direction)
            {
                for (int i = base.Count - 1; i >= 0; i--)
                {
                    IBookMark mark2 = base[i];
                    if ((mark2.Index == 0x7fffffff) && (mark2.Line < this.owner.Position.Y))
                    {
                        num = i;
                        break;
                    }
                }
            }
            else
            {
                for (int j = 0; j < base.Count; j++)
                {
                    IBookMark mark = base[j];
                    if ((mark.Index == 0x7fffffff) && (mark.Line > this.owner.Position.Y))
                    {
                        num = j;
                        break;
                    }
                }
                if (num == -1)
                {
                    for (int k = 0; k < base.Count; k++)
                    {
                        if (base[k].Index == 0x7fffffff)
                        {
                            num = k;
                            break;
                        }
                    }
                }
                goto Label_0107;
            }
            if (num == -1)
            {
                for (int m = base.Count - 1; m >= 0; m--)
                {
                    if (base[m].Index == 0x7fffffff)
                    {
                        num = m;
                        break;
                    }
                }
            }
        Label_0107:
            if ((num >= 0) && (num < base.Count))
            {
                IBookMark mark3 = base[num];
                this.owner.BeginUpdate(UpdateReason.Navigate);
                try
                {
                    this.owner.State |= NotifyState.CenterLine | NotifyState.GotoBookMark;
                    this.owner.MoveTo(mark3.Pos, mark3.Line);
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
        }

        public virtual void GotoNextBookMark()
        {
            this.GotoBookMarkIndex(true);
        }

        public virtual void GotoPrevBookMark()
        {
            this.GotoBookMarkIndex(false);
        }

        protected void InternalClear(int index)
        {
            int num = this.FindByIndex(index);
            if (num >= 0)
            {
                base.RemoveAt(num);
            }
        }

        protected void InternalClearLine(int line)
        {
            int index = 0;
            if (base.FindLast(new BookMark(line), out index, this.lineComparer))
            {
                for (int i = index; i >= 0; i--)
                {
                    if (base[index].Line != line)
                    {
                        return;
                    }
                    base.RemoveAt(i);
                }
            }
        }

        protected int InternalGet(int line)
        {
            int num;
            if (base.FindFirst(new BookMark(line), out num, this.lineComparer))
            {
                return base[num].Index;
            }
            return -1;
        }

        protected int InternalGet(Point startPoint, Point endPoint, IComparer comparer)
        {
            int num;
            QWhale.Common.Range range = new QWhale.Common.Range(startPoint, endPoint);
            if (base.FindFirst(range, out num, comparer))
            {
                return base[num].Index;
            }
            return -1;
        }

        protected int InternalGet(Point startPoint, Point endPoint, IList<IBookMark> list, IComparer comparer)
        {
            int num;
            list.Clear();
            QWhale.Common.Range range = new QWhale.Common.Range(startPoint, endPoint);
            if (base.FindFirst(range, out num, comparer))
            {
                for (int i = num; i < base.Count; i++)
                {
                    if (comparer.Compare(base[i], range) != 0)
                    {
                        break;
                    }
                    list.Add(base[i]);
                }
            }
            return list.Count;
        }

        protected void InternalSet(IBookMark bookMark)
        {
            int num;
            if (base.FindFirst(bookMark, out num, this.pointComparer))
            {
                base.RemoveAt(num);
            }
            base.Insert(num, bookMark);
        }

        protected void InternalSet(int line, int index)
        {
            int num;
            if (base.FindFirst(new BookMark(line), out num, this.lineComparer))
            {
                base.RemoveAt(num);
            }
            base.Insert(num, this.NewBookMark(line, 0, index));
        }

        protected void InternalSet(int line, int pos, int index)
        {
            int num;
            if (base.FindFirst(new BookMark(line, pos, -1), out num, this.pointComparer))
            {
                base.RemoveAt(num);
            }
            base.Insert(num, this.NewBookMark(line, pos, index));
        }

        protected virtual IBookMark NewBookMark(int line, int pos, int index)
        {
            return new BookMark(line, pos, index);
        }

        public virtual int NextBookMark()
        {
            int index = -1;
            foreach (BookMark mark in this)
            {
                if (mark.Index > index)
                {
                    index = mark.Index;
                }
            }
            return (index + 1);
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY)
        {
            return this.PositionChanged(x, y, deltaX, deltaY, this.lineComparer, this.bookMarkComparer);
        }

        protected virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer<IBookMark> lineComparer, IComparer<IBookMark> sortComparer)
        {
            int index = 0;
            if (lineComparer != null)
            {
                base.FindFirst(new BookMark(y, 0, 0), out index, lineComparer);
            }
            if (index >= 0)
            {
                bool flag = false;
                for (int i = index; i < base.Count; i++)
                {
                    if (this.UpdatePosition(x, y, deltaX, deltaY, base[i]))
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    if (sortComparer != null)
                    {
                        base.Sort(sortComparer);
                    }
                    return true;
                }
            }
            return false;
        }

        public virtual void SetBookMark(IBookMark bookMark)
        {
            if (bookMark is IBookMarkEx)
            {
                IBookMarkEx ex = (IBookMarkEx) bookMark;
                this.SetBookMark(ex.Position, ex.Index, ex.Name, ex.Description, ex.Url);
            }
            else
            {
                this.SetBookMark(bookMark.Line, bookMark.Pos, bookMark.Index);
            }
        }

        public virtual void SetBookMark(Point position, int bookMark)
        {
            this.SetBookMark(position.Y, position.X, bookMark);
        }

        public virtual void SetBookMark(int line, int bookMark)
        {
            this.SetBookMark(line, 0, bookMark);
        }

        private void SetBookMark(int line, int pos, int bookMark)
        {
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                if (bookMark != 0x7fffffff)
                {
                    this.ClearBookMark(bookMark);
                }
                this.InternalSet(line, pos, bookMark);
                this.owner.LinesChanged(line, line);
                this.owner.State |= NotifyState.BookMarkChanged;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }

        public virtual void SetBookMark(Point position, int bookMark, string name, string description, string url)
        {
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                if (bookMark != 0x7fffffff)
                {
                    this.ClearBookMark(bookMark);
                }
                this.InternalSet(new BookMarkEx(position.Y, position.X, bookMark, name, description, url));
                this.owner.LinesChanged(position.Y, position.Y);
                this.owner.State |= NotifyState.BookMarkChanged;
            }
            finally
            {
                this.owner.EndUpdate();
            }
        }

        protected virtual bool ShouldDelete(IBookMark bm, Rectangle rect)
        {
            if (bm.Index != 0x7fffffff)
            {
                return SortList<IBookMark>.InsideRange(new Point(bm.Pos, bm.Line), rect);
            }
            if (!SortList<IBookMark>.InsideRange(new Point(0, bm.Line), rect))
            {
                return false;
            }
            if (rect.Width != 0x7fffffff)
            {
                return SortList<IBookMark>.InsideRange(new Point(0x7fffffff, bm.Line), rect);
            }
            return true;
        }

        public virtual void ToggleBookMark()
        {
            this.ToggleBookMark(this.owner.Position.Y, 0x7fffffff);
        }

        public virtual void ToggleBookMark(IBookMark bookMark)
        {
            if (bookMark is IBookMarkEx)
            {
                IBookMarkEx ex = (IBookMarkEx) bookMark;
                this.ToggleBookMark(ex.Position, ex.Index, ex.Name, ex.Description, ex.Url);
            }
            else
            {
                this.ToggleBookMark(bookMark.Position, bookMark.Index);
            }
        }

        public virtual void ToggleBookMark(int bookMark)
        {
            this.ToggleBookMark(this.owner.Position, bookMark);
        }

        public virtual void ToggleBookMark(Point position, int bookMark)
        {
            int index = this.FindBookMark(bookMark, position.Y);
            if (index >= 0)
            {
                this.ClearBookMarkByIndex(index);
            }
            else
            {
                this.SetBookMark(position.Y, position.X, bookMark);
            }
        }

        public virtual void ToggleBookMark(int line, int bookMark)
        {
            int index = this.FindBookMark(bookMark, line);
            if (index >= 0)
            {
                this.ClearBookMarkByIndex(index);
            }
            else
            {
                this.SetBookMark(line, bookMark);
            }
        }

        public virtual void ToggleBookMark(Point position, int bookMark, string name, string description, string url)
        {
            int index = this.FindBookMark(bookMark, position.Y);
            if (index >= 0)
            {
                this.ClearBookMarkByIndex(index);
            }
            else
            {
                this.SetBookMark(position, bookMark, name, description, url);
            }
        }

        protected virtual bool UpdatePosition(int x, int y, int deltaX, int deltaY, IBookMark bookmark)
        {
            Point pt = new Point(bookmark.Pos, bookmark.Line);
            if (SortList<IBookMark>.UpdatePos(x, y, deltaX, deltaY, ref pt, pt.X != 0))
            {
                bookmark = new BookMark((bookmark.Index != 0x7fffffff) ? pt.X : bookmark.Pos, pt.Y, bookmark.Index);
                return true;
            }
            return false;
        }

        private class BookMarkComparer : IComparer<IBookMark>
        {
            public int Compare(IBookMark x, IBookMark y)
            {
                int num = x.Line - y.Line;
                if (num == 0)
                {
                    num = x.Pos - y.Pos;
                }
                if (num == 0)
                {
                    num = x.Index - y.Index;
                }
                return num;
            }
        }

        private class LineComparer : IComparer<IBookMark>
        {
            public int Compare(IBookMark x, IBookMark y)
            {
                return (x.Line - y.Line);
            }
        }

        private class PointComparer : IComparer<IBookMark>
        {
            public int Compare(IBookMark x, IBookMark y)
            {
                int num = x.Line - y.Line;
                if (num == 0)
                {
                    num = x.Pos - y.Pos;
                }
                return num;
            }
        }

        private class RangeComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                IBookMark mark = (IBookMark) x;
                Point startPoint = ((IRange) y).StartPoint;
                Point endPoint = ((IRange) y).EndPoint;
                if ((mark.Line < startPoint.Y) || ((mark.Line == startPoint.Y) && (mark.Pos < startPoint.X)))
                {
                    return -1;
                }
                if ((mark.Line <= endPoint.Y) && ((mark.Line != endPoint.Y) || (mark.Pos < endPoint.X)))
                {
                    return 0;
                }
                return 1;
            }
        }
    }
}

