namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface ISelection
    {
        event EventHandler SelectionChanged;

        void Assign(ISelection source);
        int BeginUpdate();
        bool CanCopy();
        bool CanCut();
        bool CanDrag(Point position);
        bool CanPaste();
        void Capitalize();
        void ChangeBlock(StringEvent action);
        void ChangeBlock(TextUndoEvent action);
        void ChangeBlock(StringEvent action, bool changeIfEmpty, bool extendFirstLine);
        void ChangeBlock(TextUndoEvent action, bool changeIfEmpty, bool extendFirstLine);
        void CharTransponse();
        void Clear();
        void CollapseToDefinitions();
        void CommentSelection();
        void Copy();
        void Cut();
        void CutLine();
        bool Delete();
        void DeleteLeft();
        void DeleteLine();
        void DeleteRight();
        void DeleteWhiteSpace();
        void DeleteWordLeft();
        void DeleteWordRight();
        void DragTo(Point position, bool deleteOrigin);
        void EndSelection();
        int EndUpdate();
        bool GetSelectionForLine(int index, out int left, out int right);
        void Indent();
        void InsertString(string s);
        void Invalidate();
        bool IsPosInSelection(Point position);
        bool IsPosInSelection(int x, int y);
        bool IsValidSelectionPoint(Point position);
        void LineTransponse();
        void LowerCase();
        bool Move(Point position, bool deleteOrigin);
        void NewLine();
        void NewLineAbove();
        void NewLineBelow();
        void OnSelect(object source, EventArgs e);
        void OnSelectionChanged();
        void Paste();
        void PositionChanged(int x, int y, int deltaX, int deltaY);
        void ProcessEscape();
        void ProcessShiftTab();
        void ProcessTab();
        void ResetAllowedSelectionMode();
        void ResetBackColor();
        void ResetBorderColor();
        void ResetForeColor();
        void ResetInActiveBackColor();
        void ResetInActiveForeColor();
        void ResetOptions();
        bool ScrollIfNeeded(Point pt);
        void SelectAll();
        void SelectCharLeft();
        void SelectCharLeft(QWhale.Editor.SelectionType selectionType);
        void SelectCharRight();
        void SelectCharRight(QWhale.Editor.SelectionType selectionType);
        int SelectedCount();
        string SelectedString(int index);
        void SelectFileBegin();
        void SelectFileBegin(QWhale.Editor.SelectionType selectionType);
        void SelectFileEnd();
        void SelectFileEnd(QWhale.Editor.SelectionType selectionType);
        Rectangle SelectionToScreen();
        Point SelectionToTextPoint(Point position);
        void SelectLine();
        void SelectLineBegin();
        void SelectLineBegin(QWhale.Editor.SelectionType selectionType);
        void SelectLineDown();
        void SelectLineDown(QWhale.Editor.SelectionType selectionType);
        void SelectLineEnd();
        void SelectLineEnd(QWhale.Editor.SelectionType selectionType);
        void SelectLineUp();
        void SelectLineUp(QWhale.Editor.SelectionType selectionType);
        void SelectPageDown();
        void SelectPageDown(QWhale.Editor.SelectionType selectionType);
        void SelectPageUp();
        void SelectPageUp(QWhale.Editor.SelectionType selectionType);
        void SelectScreenBottom();
        void SelectScreenBottom(QWhale.Editor.SelectionType selectionType);
        void SelectScreenTop();
        void SelectScreenTop(QWhale.Editor.SelectionType selectionType);
        void SelectToBrace();
        void SelectToCloseBrace();
        void SelectToOpenBrace();
        void SelectWord();
        void SelectWordLeft();
        void SelectWordLeft(QWhale.Editor.SelectionType selectionType);
        void SelectWordRight();
        void SelectWordRight(QWhale.Editor.SelectionType selectionType);
        void SetSelection(QWhale.Editor.SelectionType selectionType, Rectangle selectionRect);
        void SetSelection(QWhale.Editor.SelectionType selectionType, Point selectionStart, Point selectionEnd);
        void SmartFormat();
        bool SmartFormat(char ch);
        void SmartFormat(int line);
        void SmartFormatBlock(bool extended);
        void SmartFormatDocument();
        void SmartIndent();
        bool SmartIndent(char ch);
        void StartSelection();
        void SwapAnchor();
        void Tabify();
        Point TextToSelectionPoint(Point position);
        void ToggleOutlining();
        void ToggleOverWrite();
        void UncommentSelection();
        void UnIndent();
        void UnTabify();
        void UpdateSelection();
        void UpdateSelStart(bool checkIfEmpty);
        void UpdateSelStart(Point position);
        void UpperCase();
        void WordTransponse();

        QWhale.Editor.AllowedSelectionMode AllowedSelectionMode { get; set; }

        Color BackColor { get; set; }

        Color BorderColor { get; set; }

        Color ForeColor { get; set; }

        Color InActiveBackColor { get; set; }

        Color InActiveForeColor { get; set; }

        bool IsEmpty { get; }

        SelectionOptions Options { get; set; }

        string SelectedColorData { get; }

        string SelectedText { get; set; }

        int SelectionLength { get; set; }

        Rectangle SelectionRect { get; set; }

        int SelectionStart { get; set; }

        QWhale.Editor.SelectionState SelectionState { get; set; }

        QWhale.Editor.SelectionType SelectionType { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        int UpdateCount { get; }
    }
}

