namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using System;

    public interface ILineStyle : IBookMark
    {
        int Priority { get; set; }

        IRange Range { get; set; }
    }
}

