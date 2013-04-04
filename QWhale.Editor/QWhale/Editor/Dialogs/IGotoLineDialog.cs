namespace QWhale.Editor.Dialogs
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IGotoLineDialog
    {
        event HelpEventHandler HelpRequested;

        DialogResult Execute(object sender, int lines, ref int line);
        DialogResult Execute(object sender, int lines, ref int line, IWin32Window owner);
    }
}

