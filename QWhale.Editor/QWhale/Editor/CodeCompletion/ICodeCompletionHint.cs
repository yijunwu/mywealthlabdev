namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;

    public interface ICodeCompletionHint : ICodeCompletionWindow, IControl, ISyntaxPaint
    {
        void ResetAutoHide();
        void ResetAutoHidePause();

        bool AutoHide { get; set; }

        int AutoHidePause { get; set; }
    }
}

