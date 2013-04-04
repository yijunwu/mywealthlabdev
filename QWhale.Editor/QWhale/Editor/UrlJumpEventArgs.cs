namespace QWhale.Editor
{
    using System;

    public class UrlJumpEventArgs : EventArgs
    {
        public bool Handled;
        public string Text;

        public UrlJumpEventArgs(string text, bool handled)
        {
            this.Text = text;
            this.Handled = handled;
        }
    }
}

