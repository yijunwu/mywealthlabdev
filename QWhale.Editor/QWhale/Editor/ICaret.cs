namespace QWhale.Editor
{
    using System;
    using System.Drawing;

    public interface ICaret
    {
        void CreateCaret();
        void DestroyCaret();
        void DisplayDragCaret();
        Size GetCaretSize(Point position);
        void HideDragCaret();
        void ResetHideCaret();
        void ResetKeepCaretOnLostFocus();
        void ShowCaret(int x, int y);
        void UpdateCaret();

        bool HideCaret { get; set; }

        bool KeepCaretOnLostFocus { get; set; }
    }
}

