namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using System;
    using System.Windows.Forms;

    public interface ICompletionEdit : IControl
    {
        void UpdateSize();

        TextBox EditBox { get; }

        System.Windows.Forms.Label Label { get; }

        System.Windows.Forms.Label PathLabel { get; }
    }
}

