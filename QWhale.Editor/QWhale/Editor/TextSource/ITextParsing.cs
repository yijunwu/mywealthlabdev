namespace QWhale.Editor.TextSource
{
    using QWhale.Syntax.Lexer;
    using System;
    using System.Runtime.InteropServices;

    public interface ITextParsing
    {
        void FormatText();
        bool NeedAutoComplete();
        bool NeedCodeCompletion();
        bool NeedCodeCompletionTabs();
        bool NeedFormatText();
        bool NeedOutlineText();
        bool NeedParse();
        bool NeedQuickInfoTips();
        bool NeedReparseTextOnLineChange();
        void ParseString(int index);
        void ParseStrings(int first, int last);
        void ParseToString(int index);
        bool ProcessAutoComplete(out string code);
        void SetLastParsed(int index);

        ILexer Lexer { get; set; }

        int ParserLine { get; }
    }
}

