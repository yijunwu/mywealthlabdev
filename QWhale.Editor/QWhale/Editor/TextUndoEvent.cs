namespace QWhale.Editor
{
    using QWhale.Syntax;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void TextUndoEvent(string s, ITextUndoList operations);
}

