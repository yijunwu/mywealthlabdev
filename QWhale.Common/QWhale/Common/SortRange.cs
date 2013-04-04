namespace QWhale.Common
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class SortRange : QWhale.Common.Range, ISortRange, IRange, ICloneable
    {
        private int index;
        private IRange range;
        private ISortList<IRange> ranges;

        public SortRange(int index)
        {
            this.index = index;
            this.ranges = new SortList<IRange>();
        }

        public SortRange(IRange range, int index) : this(index)
        {
            this.range = range;
        }

        public virtual int Add(ISortRange range)
        {
            this.ranges.Add(range);
            return (this.ranges.Count - 1);
        }

        public virtual bool BlockDeleting(Rectangle rect, IRangeList list, IComparer<IRange> comparer)
        {
            int num;
            bool flag = false;
            IRange range = new QWhale.Common.Range(rect.Right, rect.Bottom, rect.Right, rect.Bottom);
            if (this.ranges.FindLast(range, out num, comparer))
            {
                num++;
            }
            for (int i = Math.Min(num, this.ranges.Count - 1); i >= 0; i--)
            {
                SortRange range2 = (SortRange) this.ranges[i];
                IRange range3 = range2;
                if (range3.EndPoint.Y < rect.Top)
                {
                    return flag;
                }
                if (QWhale.Common.Range.InsideRange(range3.StartPoint, rect) && QWhale.Common.Range.InsideRange(new Point(Math.Max(range3.EndPoint.X - 1, 0), range3.EndPoint.Y), rect))
                {
                    if (range2.Index < list.Count)
                    {
                        list.RemoveAt(range2.Index);
                    }
                    this.ranges.RemoveAt(i);
                    flag = true;
                }
                else if (range2.BlockDeleting(rect, list, comparer))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public virtual void Clear()
        {
            foreach (ISortRange range in this.ranges)
            {
                range.Clear();
            }
            this.ranges.Clear();
        }

        public override object Clone()
        {
            ISortRange range = new SortRange(this.index) {
                StartPoint = this.StartPoint,
                EndPoint = this.EndPoint
            };
            foreach (IRange range2 in this.ranges)
            {
                range.Ranges.Add((IRange) range2.Clone());
            }
            return range;
        }

        public virtual bool Contains(IRange range)
        {
            if ((this.StartPoint.Y >= range.StartPoint.Y) && ((this.StartPoint.Y != range.StartPoint.Y) || (this.StartPoint.X >= range.StartPoint.X)))
            {
                return false;
            }
            return ((this.EndPoint.Y > range.EndPoint.Y) || ((this.EndPoint.Y == range.EndPoint.Y) && (this.EndPoint.X >= range.EndPoint.X)));
        }

        public void GetRanges(IList<IRange> ranges)
        {
            foreach (ISortRange range in ranges)
            {
                ranges.Add(this);
                range.GetRanges(ranges);
            }
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, IRangeList list, IComparer<IRange> comparer)
        {
            int num;
            bool flag = false;
            IRange range = new QWhale.Common.Range(x, y, x, y);
            if (!this.ranges.FindLast(range, out num, comparer))
            {
                num--;
            }
            for (int i = Math.Max(num, 0); i < this.ranges.Count; i++)
            {
                ISortRange range2 = (ISortRange) this.ranges[i];
                Point startPoint = range2.StartPoint;
                if ((deltaY == 0) && (startPoint.Y > y))
                {
                    return flag;
                }
                if (list.UpdatePosition(range2, x, y, deltaX, deltaY))
                {
                    flag = true;
                }
                if (range2.PositionChanged(x, y, deltaX, deltaY, list, comparer))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public override Point EndPoint
        {
            get
            {
                if (this.range == null)
                {
                    return base.EndPoint;
                }
                return this.range.EndPoint;
            }
            set
            {
                if (this.range != null)
                {
                    this.range.EndPoint = value;
                }
                else
                {
                    base.EndPoint = value;
                }
            }
        }

        public virtual int Index
        {
            get
            {
                return this.index;
            }
        }

        public IRange Range
        {
            get
            {
                return this.range;
            }
        }

        public virtual ISortList<IRange> Ranges
        {
            get
            {
                return this.ranges;
            }
        }

        public override Point StartPoint
        {
            get
            {
                if (this.range == null)
                {
                    return base.StartPoint;
                }
                return this.range.StartPoint;
            }
            set
            {
                if (this.range != null)
                {
                    this.range.StartPoint = value;
                }
                else
                {
                    base.StartPoint = value;
                }
            }
        }
    }
}

