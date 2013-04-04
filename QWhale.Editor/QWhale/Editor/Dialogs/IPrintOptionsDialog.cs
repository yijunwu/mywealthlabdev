namespace QWhale.Editor.Dialogs
{
    using QWhale.Editor;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IPrintOptionsDialog
    {
        event HelpEventHandler HelpRequested;

        void ResetAllowedOptions();
        void ResetOptions();
        DialogResult ShowDialog();
        DialogResult ShowDialog(IWin32Window owner);

        PrintOptions AllowedOptions { get; set; }

        string FileName { get; set; }

        PrintOptions Options { get; set; }
    }
}

