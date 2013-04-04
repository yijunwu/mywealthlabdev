namespace QWhale.Editor
{
    using System;

    [Flags]
    public enum SelectionOptions
    {
        ClearOnDrag = 0x10000,
        ConvertToSpacesOnPaste = 0x4000,
        CopyLineWhenEmpty = 0x20000,
        DeselectOnCopy = 0x40,
        DeselectOnDblClick = 0x2000,
        DisableCodeSnippetOnTab = 0x40000,
        DisableDragging = 2,
        DisableSelection = 1,
        DrawBorder = 0x800,
        ExtendedBlockMode = 0x80000,
        HideSelection = 0x10,
        None = 0,
        OverwriteBlocks = 0x100,
        PersistentBlocks = 0x80,
        RtfClipboard = 0x8000,
        SelectBeyondEol = 4,
        SelectLineOnDblClick = 0x20,
        SelectLineOnTripleClick = 0x1000,
        SmartFormat = 0x200,
        UseColors = 8,
        WordSelect = 0x400
    }
}

