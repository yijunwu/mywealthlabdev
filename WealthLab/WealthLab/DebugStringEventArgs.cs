namespace WealthLab
{
    using System;

    public class DebugStringEventArgs : EventArgs
    {
        private string debugMessageStr;

        public DebugStringEventArgs(string debugMessage)
        {
            this.debugMessageStr = debugMessage;
        }

        public string DebugMessage
        {
            get
            {
                return this.debugMessageStr;
            }
        }
    }
}

