namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public class LineStyles : SortList<ILineStyle>, ILineStyles, IList<ILineStyle>, ICollection<ILineStyle>, IEnumerable<ILineStyle>, IEnumerable
    {
        protected IComparer<ILineStyle> lineComparer;
        protected IComparer<ILineStyle> lineStyleComparer;
        private ITextSource owner;
        protected IComparer<ILineStyle> pointComparer;
        protected IComparer<ILineStyle> styleComparer;

        public LineStyles()
        {
            this.lineComparer = new LineComparer();
            this.pointComparer = new PointComparer();
            this.lineStyleComparer = new LineStyleComparer();
            this.styleComparer = new StyleComparer();
        }

        public LineStyles(ITextSource owner) : this()
        {
            this.owner = owner;
        }

        public virtual void Assign(ILineStyles source)
        {
            this.owner.BeginUpdate(UpdateReason.Other);
            try
            {
                base.Clear();
                foreach (ILineStyle style in source)
                {
                    base.Add(this.NewLineStyle(style.Line, style.Pos, style.Index, style.Priority, style.Range));
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
            base.FindLast(new LineStyle(rect.Bottom), out num, this.lineComparer);
            if (num >= 0)
            {
                for (int i = Math.Min(num, base.Count - 1); i >= 0; i--)
                {
                    ILineStyle style = base[i];
                    if (style.Line < rect.Top)
                    {
                        return flag;
                    }
                    if (this.ShouldDelete(style, rect))
                    {
                        flag = true;
                        base.RemoveAt(i);
                    }
                }
            }
            return flag;
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
            if (base.FindFirst(new LineStyle(line), out num, this.lineComparer))
            {
                return num;
            }
            return -1;
        }

        public virtual int GetLineStyle(int line)
        {
            return this.InternalGet(line);
        }

        public virtual int GetLineStyles(int line, IList<ILineStyle> list)
        {
            list.Clear();
            foreach (ILineStyle style in this)
            {
                int num = (style.Range != null) ? style.Range.EndPoint.Y : style.Line;
                if ((line >= style.Line) && (line <= num))
                {
                    list.Add(style);
                }
            }
            return list.Count;
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
            if (base.FindLast(new LineStyle(line), out index, this.lineComparer))
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
            if (base.FindFirst(new LineStyle(line), out num, this.lineComparer))
            {
                return base[num].Index;
            }
            return -1;
        }

        protected void InternalSet(ILineStyle style)
        {
            int num;
            if (base.FindFirst(style, out num, this.pointComparer))
            {
                base.RemoveAt(num);
            }
            base.Insert(num, style);
        }

        protected void InternalSet(int line, int index)
        {
            int num;
            if (base.FindFirst(new LineStyle(line), out num, this.lineComparer))
            {
                base.RemoveAt(num);
            }
            base.Insert(num, this.NewLineStyle(line, 0, index, -1, null));
        }

        protected void InternalSet(int line, int pos, int index)
        {
            int num;
            if (base.FindFirst(new LineStyle(line, pos, -1), out num, this.pointComparer))
            {
                base.RemoveAt(num);
            }
            base.Insert(num, this.NewLineStyle(line, pos, index, -1, null));
        }

        protected virtual ILineStyle NewLineStyle(int line, int ch, int index, int priority, IRange range)
        {
            return new LineStyle(line, ch, index, priority, range);
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY)
        {
            return this.PositionChanged(x, y, deltaX, deltaY, null, this.lineStyleComparer);
        }

        protected virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer<ILineStyle> lineComparer, IComparer<ILineStyle> sortComparer)
        {
            int index = 0;
            if (lineComparer != null)
            {
                base.FindFirst(new LineStyle(y), out index, lineComparer);
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

        protected virtual void RemoveLineAt(int idx, int line)
        {
            if (this.owner != null)
            {
                this.owner.BeginUpdate(UpdateReason.Other);
                try
                {
                    base.RemoveAt(idx);
                    this.owner.State |= NotifyState.BookMarkChanged;
                    this.owner.LinesChanged(line, line);
                }
                finally
                {
                    this.owner.EndUpdate();
                }
            }
            else
            {
                base.RemoveAt(idx);
            }
        }

        public virtual void RemoveLineStyle(int line)
        {
            int idx = this.FindByLine(line);
            if (idx >= 0)
            {
                this.RemoveLineAt(idx, line);
            }
        }

        public virtual void SetLineStyle(int line, int style)
        {
            this.SetLineStyle(new Point(-1, line), null, -1, style);
        }

        public virtual void SetLineStyle(IRange range, int priority, int style)
        {
            this.SetLineStyle(range.StartPoint, range, priority, style);
        }

        public virtual void SetLineStyle(Point position, int priority, int style)
        {
            this.SetLineStyle(position, null, priority, style);
        }

        public virtual void SetLineStyle(int line, int priority, int style)
        {
            this.SetLineStyle(new Point(-1, line), null, priority, style);
        }

        public virtual void SetLineStyle(Point position, IRange range, int priority, int style)
        {
            if (this.owner != null)
            {
                this.owner.BeginUpdate(UpdateReason.Other);
            }
            try
            {
                int num;
                ILineStyle style2 = this.NewLineStyle(position.Y, position.X, style, priority, range);
                if (base.FindFirst(style2, out num, this.styleComparer))
                {
                    base[num].Assign(style2);
                }
                else
                {
                    base.Insert(num, style2);
                }
                if (this.owner != null)
                {
                    if (range != null)
                    {
                        this.owner.LinesChanged(range.StartPoint.Y, range.EndPoint.Y);
                    }
                    else
                    {
                        this.owner.LinesChanged(position.Y, position.Y);
                    }
                    this.owner.State |= NotifyState.BookMarkChanged;
                }
            }
            finally
            {
                if (this.owner != null)
                {
                    this.owner.EndUpdate();
                }
            }
        }

        protected virtual bool ShouldDelete(ILineStyle style, Rectangle rect)
        {
            Point pt = new Point(Math.Max(style.Pos, 0), style.Line);
            Point point2 = (style.Range != null) ? style.Range.EndPoint : new Point(0x7fffffff, style.Line);
            return (SortList<ILineStyle>.InsideRange(pt, rect) && SortList<ILineStyle>.InsideRange(point2, rect, true));
        }

        public virtual void ToggleLineStyle(int line, int style)
        {
            if (this.FindByLine(line) >= 0)
            {
                this.RemoveLineStyle(line);
            }
            else
            {
                this.SetLineStyle(line, style);
            }
        }

        public virtual void ToggleLineStyle(int line, int priority, int style)
        {
            int num = this.FindByLine(line);
            if (num >= 0)
            {
                for (int i = num; i < base.Count; i++)
                {
                    if (((BookMark) base[i]).Line != line)
                    {
                        break;
                    }
                    if (((BookMark) base[i]).Index == style)
                    {
                        this.RemoveLineAt(i, line);
                        return;
                    }
                }
            }
            this.SetLineStyle(line, priority, style);
        }

        protected virtual bool UpdatePosition(int x, int y, int deltaX, int deltaY, ILineStyle style)
        {
            bool flag = true;
            Point pt = new Point(Math.Max(style.Pos, 0), style.Line);
            if (SortList<ILineStyle>.UpdatePos(x, y, deltaX, deltaY, ref pt, false))
            {
                style = new LineStyle((style.Pos >= 0) ? pt.X : style.Pos, pt.Y, style.Index);
                if (style.Range != null)
                {
                    style.Range.StartPoint = pt;
                }
                flag = true;
            }
            if (style.Range != null)
            {
                pt = style.Range.EndPoint;
                if (SortList<ILineStyle>.UpdatePos(x, y, deltaX, deltaY, ref pt, true))
                {
                    style.Range.EndPoint = pt;
                    flag = true;
                }
            }
            return flag;
        }

        private class LineComparer : IComparer<ILineStyle>
        {
            public int Compare(ILineStyle x, ILineStyle y)
            {
                return (x.Line - y.Line);
            }
        }

        private class LineStyleComparer : IComparer<ILineStyle>
        {
            public int Compare(ILineStyle x, ILineStyle y)
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

        private class PointComparer : IComparer<ILineStyle>
        {
            public int Compare(ILineStyle x, ILineStyle y)
            {
                int num = x.Line - y.Line;
                if (num == 0)
                {
                    num = x.Pos - y.Pos;
                }
                return num;
            }
        }

        private class StyleComparer : IComparer<ILineStyle>
        {
            public virtual int Compare(ILineStyle x, ILineStyle y)
            {
                int num = x.Line - y.Line;
                if (num == 0)
                {
                    num = x.Pos - y.Pos;
                }
                if (num == 0)
                {
                    num = x.Priority - y.Priority;
                }
                return num;
            }
        }
    }
}

