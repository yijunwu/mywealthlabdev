namespace WealthLab
{
    using System;

    public interface IItemTracker<T>
    {
        void ItemAdded(T item);
        void ItemChanged(T item);
        void ItemRemoved(T item);
    }
}

