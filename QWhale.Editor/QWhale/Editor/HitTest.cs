namespace QWhale.Editor
{
    using System;

    [Flags]
    public enum HitTest
    {
        Above = 1,
        Below = 2,
        BeyondEof = 0x80,
        BeyondEol = 0x40,
        BookMark = 0x800,
        Gutter = 0x100,
        GutterImage = 0x400,
        HyperText = 0x20000,
        Left = 4,
        LineModificator = 0x8000,
        LineNumber = 0x10000,
        Margin = 0x200,
        None = 0,
        OutlineArea = 0x1000,
        OutlineButton = 0x4000,
        OutlineImage = 0x2000,
        Page = 0x40000,
        PageWhiteSpace = 0x80000,
        Right = 8,
        Selection = 0x20,
        Text = 0x10
    }
}

