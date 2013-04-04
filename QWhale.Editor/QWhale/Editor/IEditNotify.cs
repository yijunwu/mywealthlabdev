namespace QWhale.Editor
{
    using QWhale.Common;
    using System;

    public interface IEditNotify : INotify, IUpdate
    {
        int FirstChanged { get; }

        int LastChanged { get; }
    }
}

