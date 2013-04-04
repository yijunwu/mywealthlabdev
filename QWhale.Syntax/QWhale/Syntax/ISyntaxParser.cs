namespace QWhale.Syntax
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface ISyntaxParser : IParser, ILexer, INotify, IUpdate, IImport
    {
        event EventHandler TextReparsed;

        void CodeCompletion(string text, short[] textData, Point position, CodeCompletionArgs e);
        object FindDeclaration(string text, Point position);
        int FindReferences(ISyntaxNode node, ISyntaxNodes references);
        ISyntaxNode GetAutoFormatNode(Point position, bool extended, out Point startPt);
        ISyntaxNode GetNodeAt(Point position);
        string GetSingleLineComment();
        int GetSmartIndent(int index, bool autoIndent);
        int GetSyntaxErrors(IList<ISyntaxError> errors);
        bool IsCodeCompletionChar(char ch, byte style, ref int interval);
        bool IsContentDivider(int index);
        bool IsDeclaration(ISyntaxNode node);
        int Outline(IList<IRange> ranges);
        bool ProcessAutoComplete(string text, Point position, out string code);
        bool ReparseBlock(Point position);
        bool ReparseBlock(Point position, string text, out ISyntaxNode node, CodeCompletionType completionType);
        void ReparseText();
        void ResetAutoIndentChars();
        void ResetCodeCompletionChars();
        void ResetCodeCompletionStopChars();
        void ResetOptions();
        void ResetSmartFormatChars();
        int SmartFormatLine(int index, string text, short[] textData, ITextUndoList operations);

        char[] AutoIndentChars { get; set; }

        bool CaseSensitive { get; }

        char[] CodeCompletionChars { get; set; }

        char[] CodeCompletionStopChars { get; set; }

        ICodeSnippetsProvider CodeSnippets { get; }

        ICodeCompletionRepository CompletionRepository { get; set; }

        SyntaxOptions Options { get; set; }

        char[] SmartFormatChars { get; set; }

        ISyntaxTree SyntaxTree { get; set; }

        bool UseScheme { get; set; }
    }
}

