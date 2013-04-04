namespace QWhale.Editor
{
    using System;

    public class AutoCorrectEventArgs : EventArgs
    {
        public string CorrectWord;
        public bool HasCorrection;
        public string Word;
    }
}

