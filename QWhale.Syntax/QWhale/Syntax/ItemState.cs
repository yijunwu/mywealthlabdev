namespace QWhale.Syntax
{
    using System;

    [Flags]
    public enum ItemState : byte
    {
        None = 0,
        Parsed = 1,
        Readonly = 2
    }
}

