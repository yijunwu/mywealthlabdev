namespace QWhale.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public class RangeList : SortList<IRange>, IRangeList, ISortList<IRange>, IList<IRange>, ICollection<IRange>, IEnumerable<IRange>, IEnumerable
    {
        private IComparer<IRange> inclusiveLineHitTest = new InclusiveLineHitTest();
        private IComparer<IRange> lineHitTest = new LineHitTest();
        private bool needSort;
        private IComparer<IRange> pointComparer = new PointComparer();
        private IComparer<IRange> pointHitTest = new PointHitTest();
        private IComparer<IRange> rangeComparer = new RangeComparer();
        private ISortRange topRange = new QWhale.Common.TopRange();
        private int updateCount;

        public int Add(IRange value)
        {
            if (this.updateCount == 0)
            {
                int num;
                IRange range = value;
                if (base.FindLast(range, out num, this.rangeComparer))
                {
                    this.RemoveAt(num);
                }
                this.Insert(num, range);
                return num;
            }
            this.needSort = true;
            base.Add(value);
            return (base.Count - 1);
        }

        public int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual bool BlockDeleting(Rectangle rect)
        {
            bool update = false;
            this.BeginUpdate();
            try
            {
                update = this.topRange.BlockDeleting(rect, this, this.pointComparer);
                if (update)
                {
                    this.SortLevels();
                }
            }
            finally
            {
                this.EndUpdate(update);
            }
            return update;
        }

        public void Clear()
        {
            base.Clear();
            this.topRange.Clear();
        }

        public void CopyFrom(IList<IRange> ranges)
        {
            this.BeginUpdate();
            try
            {
                this.Clear();
                foreach (IRange range in ranges)
                {
                    this.Add(range);
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public int EndUpdate()
        {
            return this.EndUpdate(true);
        }

        public int EndUpdate(bool update)
        {
            this.updateCount--;
            if ((this.updateCount == 0) && update)
            {
                this.Update();
            }
            return this.updateCount;
        }

        public IRange FindExactRange(Point point)
        {
            IRange range = this.FindRange(point, this.pointHitTest);
            if ((range != null) && range.StartPoint.Equals(point))
            {
                return range;
            }
            return null;
        }

        public IRange FindExactRange(int index)
        {
            IRange range = this.FindRange(index, this.lineHitTest);
            if ((range != null) && (range.StartPoint.Y == index))
            {
                return range;
            }
            return null;
        }

        public IRange FindInclusiveRange(int index)
        {
            new QWhale.Common.Range(0, index, 0, index);
            return this.FindRange(index, this.inclusiveLineHitTest);
        }

        public IRange FindRange(Point point)
        {
            return this.FindRange(point, this.pointHitTest);
        }

        public IRange FindRange(int index)
        {
            return this.FindRange(index, this.lineHitTest);
        }

        protected IRange FindRange(Point point, IComparer<IRange> comparer)
        {
            int num;
            ISortRange topRange = this.topRange;
            IRange range = null;
            IRange range3 = new QWhale.Common.Range(point, point);
            while (topRange.Ranges.FindLast(range3, out num, comparer))
            {
                topRange = (ISortRange) topRange.Ranges[num];
                range = topRange.Range;
            }
            return range;
        }

        protected IRange FindRange(int index, IComparer<IRange> comparer)
        {
            int num;
            ISortRange topRange = this.topRange;
            IRange range = null;
            IRange range3 = new QWhale.Common.Range(0, index, 0, index);
            while (topRange.Ranges.FindLast(range3, out num, comparer))
            {
                topRange = (ISortRange) topRange.Ranges[num];
                range = topRange.Range;
            }
            return range;
        }

        protected int FindRangeIndex(Point point, IComparer<IRange> comparer)
        {
            int num2;
            ISortRange topRange = this.topRange;
            int index = -1;
            IRange range2 = new QWhale.Common.Range(point, point);
            while (topRange.Ranges.FindLast(range2, out num2, comparer))
            {
                topRange = (ISortRange) topRange.Ranges[num2];
                index = topRange.Index;
            }
            return index;
        }

        protected int FindRangeIndex(int index, IComparer<IRange> comparer)
        {
            int num2;
            ISortRange topRange = this.topRange;
            int num = -1;
            IRange range2 = new QWhale.Common.Range(0, index, 0, index);
            while (topRange.Ranges.FindLast(range2, out num2, comparer))
            {
                topRange = (ISortRange) topRange.Ranges[num2];
                num = topRange.Index;
            }
            return num;
        }

        public int GetExactRanges(IList<IRange> ranges, int index)
        {
            int num;
            ranges.Clear();
            ISortRange topRange = this.topRange;
            IRange range2 = new QWhale.Common.Range(0, index, 0, index);
            while (topRange.Ranges.FindLast(range2, out num, this.lineHitTest))
            {
                topRange = (ISortRange) topRange.Ranges[num];
                if ((topRange.StartPoint.Y == index) && (topRange.Range != null))
                {
                    ranges.Add(topRange.Range);
                }
            }
            return ranges.Count;
        }

        public IList<IRange> GetRanges()
        {
            IList<IRange> ranges = new List<IRange>();
            this.GetRanges(ranges);
            return ranges;
        }

        public int GetRanges(IList<IRange> ranges)
        {
            ranges.Clear();
            foreach (IRange range in this)
            {
                ranges.Add(range);
            }
            return ranges.Count;
        }

        public int GetRanges(IList<IRange> ranges, Point point)
        {
            int num;
            ranges.Clear();
            ISortRange topRange = this.topRange;
            IRange range2 = new QWhale.Common.Range(point, point);
            while (topRange.Ranges.FindLast(range2, out num, this.pointHitTest))
            {
                topRange = (ISortRange) topRange.Ranges[num];
                if (topRange.Range != null)
                {
                    ranges.Add(topRange.Range);
                }
            }
            return ranges.Count;
        }

        public int GetRanges(IList<IRange> ranges, int index)
        {
            int num;
            ranges.Clear();
            ISortRange topRange = this.topRange;
            IRange range2 = new QWhale.Common.Range(0, index, 0, index);
            while (topRange.Ranges.FindLast(range2, out num, this.lineHitTest))
            {
                topRange = (ISortRange) topRange.Ranges[num];
                if (topRange.Range != null)
                {
                    ranges.Add(topRange.Range);
                }
            }
            return ranges.Count;
        }

        public int GetRanges(IList<IRange> ranges, Point startpoint, Point endpoint)
        {
            ranges.Clear();
            ISortRange x = new SortRange(new QWhale.Common.Range(startpoint, endpoint), 0);
            IRange y = new QWhale.Common.Range();
            IRange range3 = new QWhale.Common.Range();
            foreach (IRange range4 in this)
            {
                y.StartPoint = range4.StartPoint;
                range3.StartPoint = range4.EndPoint;
                if ((this.pointHitTest.Compare(x, y) == 0) && (this.pointHitTest.Compare(x, range3) == 0))
                {
                    ranges.Add(range4);
                }
            }
            return ranges.Count;
        }

        public void Insert(int index, IRange value)
        {
            base.Insert(index, value);
            if (this.updateCount == 0)
            {
                this.SortLevels();
            }
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY)
        {
            return this.topRange.PositionChanged(x, y, deltaX, deltaY, this, this.pointComparer);
        }

        public void RemoveAt(int index)
        {
            base.RemoveAt(index);
            if (this.updateCount == 0)
            {
                this.SortLevels();
            }
        }

        public void RemoveRange(Point point)
        {
            int num;
            IList<int> list = new List<int>();
            ISortRange topRange = this.topRange;
            IRange range2 = new QWhale.Common.Range(point, point);
            while (topRange.Ranges.FindLast(range2, out num, this.pointHitTest))
            {
                if (topRange.Index >= 0)
                {
                    list.Add(topRange.Index);
                }
                topRange = (ISortRange) topRange.Ranges[num];
            }
            if (list.Count > 0)
            {
                this.BeginUpdate();
                try
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        this.RemoveAt(list[i]);
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public void RemoveRange(int index)
        {
            int num;
            IList<int> list = new List<int>();
            ISortRange topRange = this.topRange;
            IRange range2 = new QWhale.Common.Range(0, index, 0, index);
            while (topRange.Ranges.FindLast(range2, out num, this.lineHitTest))
            {
                if (topRange.Index >= 0)
                {
                    list.Add(topRange.Index);
                }
                topRange = (SortRange) topRange.Ranges[num];
            }
            if (list.Count > 0)
            {
                this.BeginUpdate();
                try
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        this.RemoveAt(list[i]);
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public void Sort()
        {
            if (this.updateCount == 0)
            {
                this.needSort = false;
                base.Sort(this.rangeComparer);
                this.SortLevels();
            }
        }

        protected void SortLevels()
        {
            this.topRange.Clear();
            IList<ISortRange> list = new List<ISortRange>();
            ISortRange topRange = this.topRange;
            for (int i = 0; i < base.Count; i++)
            {
                IRange range3 = base[i];
                ISortRange range = new SortRange(range3, i);
                while ((list.Count > 0) && !topRange.Contains(range3))
                {
                    topRange = list[list.Count - 1];
                    list.RemoveAt(list.Count - 1);
                }
                list.Add(topRange);
                topRange.Add(range);
                topRange = range;
            }
        }

        public virtual void Update()
        {
            if (this.needSort)
            {
                this.Sort();
            }
        }

        public virtual bool UpdatePosition(IRange range, int x, int y, int deltaX, int deltaY)
        {
            Point startPoint = range.StartPoint;
            bool flag = false;
            if (SortList<IRange>.UpdatePos(x, y, deltaX, deltaY, ref startPoint, false))
            {
                range.StartPoint = startPoint;
                flag = true;
            }
            startPoint = range.EndPoint;
            if (SortList<IRange>.UpdatePos(x, y, deltaX, deltaY, ref startPoint, true))
            {
                range.EndPoint = startPoint;
                flag = true;
            }
            return flag;
        }

        public ISortRange TopRange
        {
            get
            {
                return this.topRange;
            }
        }

        public int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        private class InclusiveLineHitTest : IComparer<IRange>
        {
            public int Compare(IRange x, IRange y)
            {
                Point startPoint = x.StartPoint;
                Point endPoint = x.EndPoint;
                int num = y.StartPoint.Y;
                if (num <= startPoint.Y)
                {
                    return 1;
                }
                if (num > endPoint.Y)
                {
                    return -1;
                }
                return 0;
            }
        }

        private class LineHitTest : IComparer<IRange>
        {
            public int Compare(IRange x, IRange y)
            {
                Point startPoint = x.StartPoint;
                Point endPoint = x.EndPoint;
                int num = y.StartPoint.Y;
                if (num < startPoint.Y)
                {
                    return 1;
                }
                if (num > endPoint.Y)
                {
                    return -1;
                }
                return 0;
            }
        }

        private class PointComparer : IComparer<IRange>
        {
            public int Compare(IRange x, IRange y)
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

        private class PointHitTest : IComparer<IRange>
        {
            public int Compare(IRange x, IRange y)
            {
                Point startPoint = x.StartPoint;
                Point endPoint = x.EndPoint;
                Point point3 = y.StartPoint;
                if ((point3.Y <= startPoint.Y) && ((point3.Y != startPoint.Y) || (point3.X < startPoint.X)))
                {
                    return 1;
                }
                if ((point3.Y < endPoint.Y) || ((point3.Y == endPoint.Y) && (point3.X < endPoint.X)))
                {
                    return 0;
                }
                return -1;
            }
        }

        private class RangeComparer : IComparer<IRange>
        {
            public int Compare(IRange x, IRange y)
            {
                int num = x.StartPoint.Y - y.StartPoint.Y;
                if (num == 0)
                {
                    num = x.StartPoint.X - y.StartPoint.X;
                }
                return num;
            }
        }
    }
}

