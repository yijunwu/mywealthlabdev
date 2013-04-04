namespace QWhale.Editor.TextSource
{
    using System;

    [Flags]
    public enum SearchOptions
    {
        BackwardSearch = 8,
        CaseSensitive = 1,
        CycledSearch = 0x400,
        EntireScope = 0x20,
        FindSelectedText = 0x200,
        FindTextAtCursor = 0x80,
        None = 0,
        PromptOnReplace = 0x100,
        RegularExpressions = 4,
        SearchHiddenText = 0x40,
        SelectionOnly = 0x10,
        SilentSearch = 0x800,
        WholeWordsOnly = 2
    }
}

