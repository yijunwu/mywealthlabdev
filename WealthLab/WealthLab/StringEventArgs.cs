namespace WealthLab
{
    using System;

    public class StringEventArgs : EventArgs
    {
        private string messageStr;

        public StringEventArgs(string string_1)
        {
            this.messageStr = string_1;
        }

        public string Message
        {
            get
            {
                return this.messageStr;
            }
        }
    }
}

