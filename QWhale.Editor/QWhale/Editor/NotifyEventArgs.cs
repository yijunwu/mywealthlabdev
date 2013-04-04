namespace QWhale.Editor
{
    using QWhale.Editor.TextSource;
    using System;

    public class NotifyEventArgs : EventArgs
    {
        public int FirstChanged;
        public int LastChanged;
        public NotifyState State;
        public bool Update;

        public NotifyEventArgs()
        {
        }

        public NotifyEventArgs(NotifyState state, int first, int last, bool update)
        {
            this.State = state;
            this.FirstChanged = first;
            this.LastChanged = last;
            this.Update = update;
        }
    }
}

