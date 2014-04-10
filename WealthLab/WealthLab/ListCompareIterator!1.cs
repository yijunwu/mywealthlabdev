namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public class ListCompareIterator<T> where T: IComparable
    {
        private IList<T> ilist_0;
        private IList<T> ilist_1;
        private int index1;
        private int index2;
        private ListCompareState listCompareState;

        public ListCompareIterator(IList<T> list1, IList<T> list2)
        {
            this.listCompareState = ListCompareState.Unmatch;
            this.ilist_0 = list1;
            this.ilist_1 = list2;
            if ((this.ilist_0.Count != 0) && (this.ilist_1.Count != 0))
            {
                T local = this.ilist_0[0];
                if (local.Equals(this.ilist_1[0]))
                {
                    this.listCompareState = ListCompareState.Match;
                }
                else
                {
                    T local2 = this.ilist_0[0];
                    if (local2.CompareTo(this.ilist_1[0]) < 0)
                    {
                        this.listCompareState = ListCompareState.Item1;
                    }
                    else
                    {
                        this.listCompareState = ListCompareState.Item2;
                    }
                }
            }
        }

        public ListCompareState Advance()
        {
            if (this.State != ListCompareState.Unmatch)
            {
                if (this.listCompareState == ListCompareState.Match)
                {
                    this.Index1++;
                    this.Index2++;
                }
                else if (this.listCompareState == ListCompareState.Item1)
                {
                    this.Index1++;
                }
                else
                {
                    this.Index2++;
                }
                if (this.State == ListCompareState.Unmatch)
                {
                    return this.State;
                }
                T local = this.ilist_0[this.index1];
                if (local.Equals(this.ilist_1[this.index2]))
                {
                    this.listCompareState = ListCompareState.Match;
                    return this.State;
                }
                if ((this.index1 != (this.ilist_0.Count - 1)) && (this.index2 != (this.ilist_1.Count - 1)))
                {
                    T local2 = this.ilist_0[this.index1 + 1];
                    if (local2.CompareTo(this.ilist_1[this.index2 + 1]) < 0)
                    {
                        this.listCompareState = ListCompareState.Item1;
                    }
                    else
                    {
                        this.listCompareState = ListCompareState.Item2;
                    }
                    return this.State;
                }
                this.listCompareState = ListCompareState.Unmatch;
            }
            return this.State;
        }

        public int Index1
        {
            get
            {
                return this.index1;
            }
            internal set
            {
                this.index1 = value;
                if (this.index1 >= this.ilist_0.Count)
                {
                    this.listCompareState = ListCompareState.Unmatch;
                }
            }
        }

        public int Index2
        {
            get
            {
                return this.index2;
            }
            internal set
            {
                this.index2 = value;
                if (this.index2 >= this.ilist_1.Count)
                {
                    this.listCompareState = ListCompareState.Unmatch;
                }
            }
        }

        public ListCompareState State
        {
            get
            {
                return this.listCompareState;
            }
        }
    }
}

