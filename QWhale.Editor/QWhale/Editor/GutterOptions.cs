namespace QWhale.Editor
{
    using System;

    [Flags]
    public enum GutterOptions
    {
        None = 0,
        PaintBookMarks = 8,
        PaintLineModificators = 0x10,
        PaintLineNumbers = 1,
        PaintLinesBeyondEof = 4,
        PaintLinesOnGutter = 2,
        PaintUserMargin = 0x20,
        SelectLineOnClick = 0x40
    }
}

