namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.TextSource;
    using System;

    public interface IEditNotifier : INotifier
    {
        void OnStateChanged(object sender, NotifyState state);
    }
}

