namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface ITextNotify : INotify, IUpdate
    {
        int BeginUpdate(UpdateReason reason);
        void LinesChanged(int first, int last);
        void LinesChanged(int first, int last, bool modified);

        int FirstChanged { get; }

        int LastChanged { get; }

        Rectangle SelectBlockRect { get; }

        NotifyState State { get; set; }
    }
}

