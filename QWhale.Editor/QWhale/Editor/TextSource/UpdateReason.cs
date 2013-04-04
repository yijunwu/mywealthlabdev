namespace QWhale.Editor.TextSource
{
    using System;

    public enum UpdateReason
    {
        Navigate,
        Insert,
        Delete,
        Break,
        UnBreak,
        DeleteBlock,
        InsertBlock,
        Other
    }
}

