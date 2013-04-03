namespace WealthLab.International
{
    using System;

    [Flags]
    internal enum ButtonBehaviour
    {
        Cancel = 4,
        Continue = 8,
        Finish = 0x10,
        Next = 1,
        None = 0x20,
        Previous = 2
    }
}

