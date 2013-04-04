namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface ICodeCompletionWindow : IControl
    {
        event ClosePopupEvent ClosePopup;

        event EventHandler Disposed;

        event HelpEventHandler HelpRequested;

        event QWhale.Editor.KeyPreviewEvent KeyPreviewEvent;

        event ShowPopupEvent ShowPopup;

        void Close(bool accept);
        void CloseDelayed(bool accept);
        bool ContainsControl(Control control);
        void EnsureVisible(ref Point position);
        bool IsFocused();
        bool PerformSearch();
        void Popup();
        void PopupAt(Point position);
        void PopupAt(int x, int y);
        void PositionChanged(int x, int y, int deltaX, int deltaY);
        void ResetAutoSize();
        void ResetCodeCompletionFlags();
        void ResetContent();
        void ResetSizeable();

        bool AutoSize { get; set; }

        CodeCompletionFlags CompletionFlags { get; set; }

        Point DisplayPos { get; set; }

        Point EndPos { get; set; }

        ImageList Images { get; set; }

        Control OwnerControl { get; set; }

        Control PopupControl { get; }

        ICodeCompletionProvider Provider { get; set; }

        bool Sizeable { get; set; }

        Point StartPos { get; set; }
    }
}

