namespace QWhale.Syntax
{
    using QWhale.Syntax.CodeCompletion;
    using System;

    public class ShowingEventArgs : EventArgs
    {
        public bool NeedShow;
        public ICodeCompletionProvider Provider;

        public ShowingEventArgs(ICodeCompletionProvider provider)
        {
            this.Provider = provider;
            this.NeedShow = true;
        }
    }
}

