namespace WealthLab
{
    using System;

    public class DebugStringEventArgs : EventArgs
    {
        private string string_0;

        public DebugStringEventArgs(string debugMessage)
        {
            this.string_0 = debugMessage;
        }

        public string DebugMessage
        {
            get
            {
                return this.string_0;
            }
        }
    }
}

