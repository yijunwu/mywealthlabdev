namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public interface ICodeCompletion
    {
        event CodeCompletionEvent NeedCodeCompletion;

        void CodeCompletion(CodeCompletionArgs e);
        bool CodeCompletionWindowFocused(out Control control);
        void CodeSnippets();
        void CompleteWord();
        bool IsValidText(Point position);
        void ListMembers();
        void ParameterInfo();
        void QuickInfo();
        void ShowCodeCompletionBox(ICodeCompletionProvider provider);
        void ShowCodeCompletionBox(ICodeCompletionProvider provider, Point position);
        void ShowCodeCompletionHint(ICodeCompletionProvider provider);
        void ShowCodeCompletionHint(ICodeCompletionProvider provider, Point position);

        ICodeCompletionBox CodeCompletionBox { get; }

        char[] CodeCompletionChars { get; set; }

        ICodeCompletionHint CodeCompletionHint { get; }

        bool IsCodeCompletionWindowFocused { get; }
    }
}

