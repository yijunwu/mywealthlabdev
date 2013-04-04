namespace QWhale.Editor.TextSource
{
    using System;

    [Flags]
    public enum NotifyState
    {
        BlockChanged = 0x40,
        BookMarkChanged = 0x80,
        CenterLine = 0x40000,
        CountChanged = 2,
        Edit = 0x800,
        FirstSearchChanged = 0x20000,
        GotoBookMark = 0x8000,
        IncrementalSearchChanged = 0x100,
        Modified = 0x1000,
        ModifiedChanged = 8,
        None = 0,
        Outline = 0x2000,
        OverWriteChanged = 4,
        PageOptionsChanged = 0x400000,
        PositionChanged = 1,
        ReadonlyChanged = 0x20,
        ScrollingOptionsChanged = 0x800000,
        ScrollingOriginChanged = 0x1000000,
        SearcRectChanged = 0x200,
        SelectBlock = 0x10000,
        SelectedTextChanged = 0x8000000,
        SelectionChanged = 0x4000000,
        SelectionOptionsChanged = 0x2000000,
        SmartFormat = 0x100000,
        StringsChanged = 0x200000,
        SyntaxChanged = 0x10,
        TextParsed = 0x80000,
        Undo = 0x400,
        WordWrap = 0x4000
    }
}

