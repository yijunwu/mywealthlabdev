namespace QWhale.Syntax
{
    using System;

    [Flags]
    public enum SyntaxNodeOptions
    {
        BackIndentation = 2,
        CodeCompletion = 0x10,
        Indentation = 1,
        KeepIndentation = 4,
        None = 0,
        Outlining = 8
    }
}

