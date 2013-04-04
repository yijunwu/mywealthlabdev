namespace QWhale.Syntax
{
    using System;

    [Flags]
    public enum TextStyle
    {
        Brace = 0x20,
        CodeSnippet = 0x80,
        HyperText = 0x10,
        MisSpelledWord = 8,
        None = 0,
        OutlineSection = 4,
        Tabulation = 2,
        WaveLine = 0x40,
        WhiteSpace = 1
    }
}

