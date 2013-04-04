namespace QWhale.Editor.TextSource
{
    using System;

    public class HyperTextEventArgs : EventArgs
    {
        public bool IsHyperText;
        public string Text;

        public HyperTextEventArgs(string text, bool isHyperText)
        {
            this.Text = text;
            this.IsHyperText = isHyperText;
        }
    }
}

