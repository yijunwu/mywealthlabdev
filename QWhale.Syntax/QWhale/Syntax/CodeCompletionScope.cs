namespace QWhale.Syntax
{
    using System;

    [Flags]
    public enum CodeCompletionScope
    {
        BaseType = 0x1000,
        Delegate = 0x20,
        Field = 0x100,
        Global = 4,
        Instance = 2,
        Method = 0x40,
        None = 0,
        Overrides = 0x800,
        Private = 0x400,
        Property = 0x80,
        Protected = 8,
        ShortType = 0x200,
        Static = 1,
        TypeName = 0x10
    }
}

