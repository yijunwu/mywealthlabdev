namespace QWhale.Editor.TextSource
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public interface IHyperText
    {
        event HyperTextEvent HyperText;

        bool IsHyperText(string text);
        void ResetHighlightHyperText();

        bool HighlightHyperText { get; set; }

        Hashtable UrlTable { get; }
    }
}

