namespace QWhale.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public interface ISortList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        bool FindExact(T obj, out int index, IComparer<T> comparer);
        bool FindExact(object obj, out int index, IComparer comparer);
        bool FindFirst(object obj, out int index, IComparer comparer);
        bool FindFirst(T obj, out int index, IComparer<T> comparer);
        bool FindLast(T obj, out int index, IComparer<T> comparer);
        bool FindLast(object obj, out int index, IComparer comparer);
        void Sort(IComparer<T> comparer);
    }
}

