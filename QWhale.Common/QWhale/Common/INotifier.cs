namespace QWhale.Common
{
    using System;

    public interface INotifier
    {
        void Notification(object sender, EventArgs e);
    }
}

