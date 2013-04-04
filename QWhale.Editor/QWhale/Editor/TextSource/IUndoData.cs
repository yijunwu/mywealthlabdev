namespace QWhale.Editor.TextSource
{
    using System;
    using System.Drawing;

    public interface IUndoData
    {
        object Data { get; set; }

        UndoOperation Operation { get; set; }

        Point Position { get; set; }

        UpdateReason Reason { get; set; }

        UndoFlags UndoFlag { get; set; }

        int UpdateCount { get; set; }
    }
}

