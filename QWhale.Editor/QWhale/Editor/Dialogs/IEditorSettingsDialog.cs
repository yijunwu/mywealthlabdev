namespace QWhale.Editor.Dialogs
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IEditorSettingsDialog
    {
        event HelpEventHandler HelpRequested;

        DialogResult Execute(EditorSettingsTab hiddenTabs);
        DialogResult Execute(EditorSettingsTab hiddenTabs, IWin32Window owner);
        DialogResult ShowDialog();
        DialogResult ShowDialog(IWin32Window owner);

        ISyntaxSettings SyntaxSettings { get; set; }
    }
}

