namespace QWhale.Common
{
    using System;

    public interface IUpdate
    {
        int BeginUpdate();
        int DisableUpdate();
        int EnableUpdate();
        int EndUpdate();
        void Update();

        int UpdateCount { get; }
    }
}

