namespace QWhale.Editor
{
    using System;
    using System.Windows.Forms;

    public class PromptReplaceEventArgs : EventArgs
    {
        public System.Windows.Forms.DialogResult DialogResult;
        public bool Handled;
        public string Text;
        public bool YesToAll;

        public PromptReplaceEventArgs(string text, bool handled, System.Windows.Forms.DialogResult dialogResult, bool yesToAll)
        {
            this.Text = text;
            this.Handled = handled;
            this.DialogResult = dialogResult;
            this.YesToAll = yesToAll;
        }
    }
}

