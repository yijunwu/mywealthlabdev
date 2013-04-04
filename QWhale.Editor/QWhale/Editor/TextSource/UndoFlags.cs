namespace QWhale.Editor.TextSource
{
    using System;

    [Flags]
    public enum UndoFlags : byte
    {
        FirstTime = 1,
        None = 0,
        Saved = 2
    }
}

