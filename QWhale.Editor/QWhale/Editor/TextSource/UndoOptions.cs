namespace QWhale.Editor.TextSource
{
    using System;

    [Flags]
    public enum UndoOptions
    {
        AllowUndo = 1,
        GroupUndo = 2,
        None = 0,
        UndoAfterSave = 8,
        UndoNavigations = 4,
        UngroupBreaks = 0x10
    }
}

