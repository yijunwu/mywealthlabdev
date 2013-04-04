namespace QWhale.Editor.TextSource
{
    using System;

    [Flags]
    public enum NavigateOptions
    {
        BeyondEof = 2,
        BeyondEol = 1,
        DownAtLineEnd = 8,
        KeepCaret = 0x20,
        MoveOnRightButton = 0x10,
        None = 0,
        UpAtLineBegin = 4
    }
}

