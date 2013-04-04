namespace QWhale.Editor.TextSource
{
    using System;

    [Flags]
    public enum IndentOptions
    {
        AutoIndent = 1,
        JumpToIndent = 8,
        None = 0,
        SmartIndent = 2,
        UsePrevIndent = 4
    }
}

