namespace QWhale.Editor.TextSource
{
    using System;

    public enum UndoOperation
    {
        Insert,
        Delete,
        Break,
        UnBreak,
        InsertBlock,
        DeleteBlock,
        Navigate,
        NavigateEx,
        UndoBlock,
        Unknown
    }
}

