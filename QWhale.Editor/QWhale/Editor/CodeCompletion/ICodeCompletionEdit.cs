namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using System;

    public interface ICodeCompletionEdit : ICodeCompletionWindow, IControl
    {
        ICompletionEdit Edit { get; }

        string EditField { get; set; }

        string EditPath { get; set; }

        string EditText { get; set; }
    }
}

