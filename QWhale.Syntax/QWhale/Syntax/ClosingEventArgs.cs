namespace QWhale.Syntax
{
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Drawing;

    public class ClosingEventArgs : EventArgs
    {
        public bool Accepted;
        public Point EndPosition;
        public bool Handled;
        public ICodeCompletionProvider Provider;
        public Point StartPosition;
        public string Text;
        public bool UseFormat;
        public bool UseIndent = true;

        public ClosingEventArgs(bool accepted, ICodeCompletionProvider provider)
        {
            this.Accepted = accepted;
            this.Provider = provider;
        }
    }
}

