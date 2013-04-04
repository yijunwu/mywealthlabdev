namespace QWhale.Editor
{
    using System;

    [Flags]
    public enum SeparatorOptions
    {
        HideHighlighting = 2,
        HighlightCurrentLine = 1,
        None = 0,
        SeparateBeyondEof = 0x20,
        SeparateContent = 0x10,
        SeparateLines = 4,
        SeparateWrapLines = 8
    }
}

