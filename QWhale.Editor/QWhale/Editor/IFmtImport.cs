namespace QWhale.Editor
{
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public interface IFmtImport : IStringImport
    {
        event ReadFormattedTextEvent ReadFormattedText;

        FontStyle DefaultStyle { get; set; }
    }
}

