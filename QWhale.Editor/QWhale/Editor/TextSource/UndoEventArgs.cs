namespace QWhale.Editor.TextSource
{
    using System;

    public class UndoEventArgs : EventArgs
    {
        public IUndoData UndoData;

        public UndoEventArgs(IUndoData uData)
        {
            this.UndoData = uData;
        }
    }
}

