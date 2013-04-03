namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public class ListCompareIterator<T> where T: IComparable
    {
        private IList<T> ilist_0;
        private IList<T> ilist_1;
        private int int_0;
        private int int_1;
        private ListCompareState listCompareState_0;

        public ListCompareIterator(IList<T> list1, IList<T> list2)
        {
            this.listCompareState_0 = ListCompareState.Unmatch;
            this.ilist_0 = list1;
            this.ilist_1 = list2;
            if ((this.ilist_0.Count != 0) && (this.ilist_1.Count != 0))
            {
                T local = this.ilist_0[0];
                if (local.Equals(this.ilist_1[0]))
                {
                    this.listCompareState_0 = ListCompareState.Match;
                }
                else
                {
                    T local2 = this.ilist_0[0];
                    if (local2.CompareTo(this.ilist_1[0]) < 0)
                    {
                        this.listCompareState_0 = ListCompareState.Item1;
                    }
                    else
                    {
                        this.listCompareState_0 = ListCompareState.Item2;
                    }
                }
            }
        }

        public ListCompareState Advance()
        {
            if (this.State != ListCompareState.Unmatch)
            {
                if (this.listCompareState_0 == ListCompareState.Match)
                {
                    this.Index1++;
                    this.Index2++;
                }
                else if (this.listCompareState_0 == ListCompareState.Item1)
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
                T local = this.ilist_0[this.int_0];
                if (local.Equals(this.ilist_1[this.int_1]))
                {
                    this.listCompareState_0 = ListCompareState.Match;
                    return this.State;
                }
                if ((this.int_0 != (this.ilist_0.Count - 1)) && (this.int_1 != (this.ilist_1.Count - 1)))
                {
                    T local2 = this.ilist_0[this.int_0 + 1];
                    if (local2.CompareTo(this.ilist_1[this.int_1 + 1]) < 0)
                    {
                        this.listCompareState_0 = ListCompareState.Item1;
                    }
                    else
                    {
                        this.listCompareState_0 = ListCompareState.Item2;
                    }
                    return this.State;
                }
                this.listCompareState_0 = ListCompareState.Unmatch;
            }
            return this.State;
        }

        public int Index1
        {
            get
            {
                return this.int_0;
            }
            internal set
            {
                this.int_0 = value;
                if (this.int_0 >= this.ilist_0.Count)
                {
                    this.listCompareState_0 = ListCompareState.Unmatch;
                }
            }
        }

        public int Index2
        {
            get
            {
                return this.int_1;
            }
            internal set
            {
                this.int_1 = value;
                if (this.int_1 >= this.ilist_1.Count)
                {
                    this.listCompareState_0 = ListCompareState.Unmatch;
                }
            }
        }

        public ListCompareState State
        {
            get
            {
                return this.listCompareState_0;
            }
        }
    }
}

