namespace WealthLab.International
{
    using System;

    [Flags]
    internal enum Enum16
    {
        flag_1 = 1,
        flag_2 = 2,
        flag_3 = 4,
        flag_4 = 8,
        flag_5 = 0x10,
        flag_6 = 0x20,
        flag_7 = 0x40,
        flag_8 = 0x80,
        flag_9 = 0x100,
        None = 0
    }
}

