namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.CodeCompletion;
    using QWhale.Editor.Dialogs;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface ISyntaxEdit : ISearch, ITextSearch, IEditNotifier, INotifier, ICaret, IEditNavigate, INavigate, IEdit, IWordWrap, ITextExport, IExport, ITextImport, IImport, ICodeCompletion, IRecordPlayBack, ISplitView, IAutoCorrect, IControl
    {
        event EventHandler ModifiedChanged;

        event PaintEventHandler PaintBackground;

        event PromptOnReplaceEvent PromptOnReplace;

        event NotifyEvent SourceStateChanged;

        void Assign(ISyntaxEdit source);
        DialogResult DisplayEditorSettingsDialog();
        DialogResult DisplayEditorSettingsDialog(EditorSettingsTab hiddenTabs);
        DialogResult DisplayEditorSettingsDialog(EditorSettingsTab hiddenTabs, IWin32Window owner);
        DialogResult DisplayGotoLineDialog();
        DialogResult DisplayGotoLineDialog(IWin32Window owner);
        DialogResult DisplayReplaceDialog();
        DialogResult DisplayReplaceDialog(IWin32Window owner);
        DialogResult DisplaySearchDialog();
        DialogResult DisplaySearchDialog(IWin32Window owner);
        Point DisplayToScreen(int x, int y);
        Point DisplayToScreen(int x, int y, bool average);
        int GetCharsInWidth(int width);
        void GetHitTest(Point position, IHitTestInfo hitTestInfo);
        void GetHitTest(int x, int y, IHitTestInfo hitTestInfo);
        void GetHitTestAtTextPoint(Point position, IHitTestInfo hitTestInfo);
        void GetHitTestAtTextPoint(int x, int y, IHitTestInfo hitTestInfo);
        int GetLinesInHeight(int height);
        string GetTextAtCursor();
        void HideScrollHint();
        void MakeVisible(Point position);
        void MakeVisible(Point position, bool centerLine);
        void MoveCaretOnDrag();
        bool ProcessKeyMsg(ref Message msg);
        void ProcessKeyPress(char keyChar);
        void ResetAcceptReturns();
        void ResetAcceptTabs();
        void ResetBorderColor();
        void ResetBorderStyle();
        Point ScreenToDisplay(int x, int y);
        Point ScreenToText(Point position);
        Point ScreenToText(int x, int y);
        Point ScreenToText(int x, int y, ref bool lineEnd);
        void ShowScrollHint(int pos);
        Point TextToScreen(Point position);
        Point TextToScreen(Point position, bool lineEnd);
        Point TextToScreen(int x, int y);
        void UpdateView();

        bool AcceptReturns { get; set; }

        bool AcceptTabs { get; set; }

        Color BorderColor { get; set; }

        EditBorderStyle BorderStyle { get; set; }

        IEditBraceMatching Braces { get; set; }

        int CharsInWidth { get; }

        Rectangle ClientArea { get; }

        int ClientHeight { get; }

        Rectangle ClientRect { get; }

        int ClientWidth { get; }

        IDisplayStrings DisplayLines { get; }

        IMargin EditMargin { get; set; }

        IEventHandlers EventHandlers { get; }

        IGutter Gutter { get; set; }

        IEditHyperText HyperText { get; set; }

        IKeyList KeyList { get; }

        ILexer Lexer { get; set; }

        ITextStrings Lines { get; set; }

        ILineSeparator LineSeparator { get; set; }

        int LinesInHeight { get; }

        IEditLineStyles LineStyles { get; set; }

        IOutlining Outlining { get; set; }

        IEditPages Pages { get; set; }

        IPainter Painter { get; }

        IPrinting Printing { get; set; }

        IScrolling Scrolling { get; set; }

        ISelection Selection { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        ITextSource Source { get; set; }

        IEditSpelling Spelling { get; set; }

        string[] Strings { get; }

        IEditSyntaxPaint SyntaxPaint { get; set; }

        ISyntaxSettings SyntaxSettings { get; }

        bool Transparent { get; set; }

        bool UseDefaultMenu { get; set; }

        IWhiteSpace WhiteSpace { get; set; }
    }
}

