namespace QWhale.Editor
{
    using System;

    [Flags]
    public enum ScrollingOptions
    {
        AllowSplitHorz = 0x20,
        AllowSplitVert = 0x40,
        FlatScrollbars = 0x10,
        HorzButtons = 0x80,
        None = 0,
        ScrollByPixels = 0x200,
        ShowScrollHint = 2,
        SmoothScroll = 1,
        SystemScrollbars = 8,
        UseScrollDelta = 4,
        VertButtons = 0x100
    }
}

