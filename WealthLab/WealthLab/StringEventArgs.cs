namespace WealthLab
{
    using System;

    public class StringEventArgs : EventArgs
    {
        private string string_0;

        public StringEventArgs(string string_1)
        {
            this.string_0 = string_1;
        }

        public string Message
        {
            get
            {
                return this.string_0;
            }
        }
    }
}

