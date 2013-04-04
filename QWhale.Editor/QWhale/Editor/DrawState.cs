namespace QWhale.Editor
{
    using System;

    [Flags]
    public enum DrawState
    {
        BeyondEof = 0x200,
        BeyondEol = 0x100,
        BookMark = 0x1000,
        Brace = 0x80,
        CodeSnippet = 0x1000000,
        Control = 1,
        Gutter = 0x400,
        GutterImage = 0x800,
        LineBookMark = 0x40,
        LineHighlight = 0x10,
        LineModificator = 0x20000,
        LineNumber = 0x2000,
        LineSeparator = 0x20,
        LineStyle = 0x800000,
        None = 0,
        OutlineArea = 0x4000,
        OutlineButton = 0x10000,
        OutlineImage = 0x8000,
        Page = 0x100000,
        PageBorder = 0x400000,
        PageHeader = 0x200000,
        Selection = 4,
        Spelling = 0x40000,
        SyntaxError = 0x80000,
        Text = 2,
        UserMargin = 0x2000000,
        WhiteSpace = 8
    }
}

