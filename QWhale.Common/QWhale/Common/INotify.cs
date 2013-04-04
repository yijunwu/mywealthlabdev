namespace QWhale.Common
{
    using System;

    public interface INotify : IUpdate
    {
        void AddNotifier(INotifier sender);
        void Notify();
        void RemoveNotifier(INotifier sender);
    }
}

