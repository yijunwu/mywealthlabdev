namespace QWhale.Syntax
{
    using System;

    [Flags]
    public enum SyntaxOptions
    {
        AutoComplete = 0x40,
        CodeCompletion = 4,
        CodeCompletionTabs = 0x200,
        FormatCase = 0x80,
        FormatSpaces = 0x100,
        None = 0,
        Outline = 1,
        QuickInfoTips = 0x20,
        ReparseOnLineChange = 0x10,
        SmartIndent = 2,
        SyntaxErrors = 8
    }
}

