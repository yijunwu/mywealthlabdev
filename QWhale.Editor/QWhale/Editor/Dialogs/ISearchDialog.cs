namespace QWhale.Editor.Dialogs
{
    using QWhale.Editor;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface ISearchDialog
    {
        event HelpEventHandler HelpRequested;

        void Close();
        void DoneSearch(ISearch search);
        void EnsureVisible(Rectangle rect);
        DialogResult Execute(ISearch search, bool isModal, bool isReplace);
        DialogResult Execute(ISearch search, bool isModal, bool isReplace, IWin32Window owner);
        void ToggleHiddenText();
        void ToggleMatchCase();
        void ToggleRegularExpressions();
        void ToggleSearchUp();
        void ToggleWholeWord();

        Form OwnerForm { get; set; }

        ISearchSettings SearchSettings { get; }

        bool Visible { get; set; }
    }
}

