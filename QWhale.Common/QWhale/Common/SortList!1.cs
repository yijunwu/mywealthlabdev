namespace QWhale.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class SortList<T> : List<T>, ISortList<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private int compareIndex;

        public bool FindExact(object obj, out int index, IComparer comparer)
        {
            index = 0;
            if (comparer == null)
            {
                return false;
            }
            int num = 0;
            int num2 = this.Count - 1;
            int num3 = num;
            bool flag = false;
            while (num <= num2)
            {
                num3 = (num + num2) >> 1;
                this.compareIndex = num3;
                int num4 = comparer.Compare(base[num3], obj);
                if (num4 < 0)
                {
                    num = num3 + 1;
                }
                else
                {
                    num2 = num3 - 1;
                    if (num4 == 0)
                    {
                        flag = true;
                        num = num3;
                    }
                }
            }
            index = num;
            return flag;
        }

        public bool FindExact(T obj, out int index, IComparer<T> comparer)
        {
            index = 0;
            if (comparer == null)
            {
                return false;
            }
            int num = 0;
            int num2 = this.Count - 1;
            int num3 = num;
            bool flag = false;
            while (num <= num2)
            {
                num3 = (num + num2) >> 1;
                this.compareIndex = num3;
                int num4 = comparer.Compare(base[num3], obj);
                if (num4 < 0)
                {
                    num = num3 + 1;
                }
                else
                {
                    num2 = num3 - 1;
                    if (num4 == 0)
                    {
                        flag = true;
                        num = num3;
                    }
                }
            }
            index = num;
            return flag;
        }

        public bool FindFirst(object obj, out int index, IComparer comparer)
        {
            int num = 0;
            int num2 = this.Count - 1;
            int num3 = num;
            bool flag = false;
            while (num <= num2)
            {
                num3 = (num + num2) >> 1;
                this.compareIndex = num3;
                int num4 = comparer.Compare(base[num3], obj);
                if (num4 < 0)
                {
                    num = num3 + 1;
                }
                else
                {
                    num2 = num3 - 1;
                    if (num4 == 0)
                    {
                        flag = true;
                    }
                }
            }
            index = num;
            return flag;
        }

        public bool FindFirst(T obj, out int index, IComparer<T> comparer)
        {
            int num = 0;
            int num2 = this.Count - 1;
            int num3 = num;
            bool flag = false;
            while (num <= num2)
            {
                num3 = (num + num2) >> 1;
                this.compareIndex = num3;
                int num4 = comparer.Compare(base[num3], obj);
                if (num4 < 0)
                {
                    num = num3 + 1;
                }
                else
                {
                    num2 = num3 - 1;
                    if (num4 == 0)
                    {
                        flag = true;
                    }
                }
            }
            index = num;
            return flag;
        }

        public bool FindLast(object obj, out int index, IComparer comparer)
        {
            int num = 0;
            int num2 = this.Count - 1;
            int num3 = num;
            int num5 = 0;
            bool flag = false;
            while (num <= num2)
            {
                num3 = (num + num2) >> 1;
                this.compareIndex = num3;
                int num4 = comparer.Compare(base[num3], obj);
                if (num4 > 0)
                {
                    num2 = num3 - 1;
                }
                else
                {
                    num = num3 + 1;
                    if (num4 == 0)
                    {
                        num5 = num3;
                        flag = true;
                    }
                }
            }
            index = flag ? num5 : num;
            return flag;
        }

        public bool FindLast(T obj, out int index, IComparer<T> comparer)
        {
            int num = 0;
            int num2 = this.Count - 1;
            int num3 = num;
            int num5 = 0;
            bool flag = false;
            while (num <= num2)
            {
                num3 = (num + num2) >> 1;
                this.compareIndex = num3;
                int num4 = comparer.Compare(base[num3], obj);
                if (num4 > 0)
                {
                    num2 = num3 - 1;
                }
                else
                {
                    num = num3 + 1;
                    if (num4 == 0)
                    {
                        num5 = num3;
                        flag = true;
                    }
                }
            }
            index = flag ? num5 : num;
            return flag;
        }

        public static bool InsideRange(Point pt, Rectangle rect)
        {
            return QWhale.Common.Range.InsideRange(pt, rect);
        }

        public static bool InsideRange(Point pt, Rectangle rect, bool checkMaxInt)
        {
            return QWhale.Common.Range.InsideRange(pt, rect, checkMaxInt);
        }

        void ISortList<T>.Sort(IComparer<T> comparer1)
        {
            base.Sort(comparer1);
        }

        public static bool UpdatePos(int x, int y, int deltaX, int deltaY, ref Point pt, bool endPos)
        {
            return QWhale.Common.Range.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos);
        }

        public static bool UpdatePos(int x, int y, int deltaX, int deltaY, ref int ch, ref int ln, bool endPos)
        {
            return QWhale.Common.Range.UpdatePos(x, y, deltaX, deltaY, ref ch, ref ln, endPos);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int CompareIndex
        {
            get
            {
                return this.compareIndex;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Count
        {
            get
            {
                return base.Count;
            }
        }
    }
}

