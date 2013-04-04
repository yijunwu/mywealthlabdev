namespace QWhale.Editor.TextSource
{
    using System;

    public class WordSpellEventArgs : EventArgs
    {
        public int ColorStyle;
        public bool Correct = true;
        public string Text;

        public WordSpellEventArgs(string text, bool correct, int colorStyle)
        {
            this.Text = text;
            this.Correct = correct;
            this.ColorStyle = colorStyle;
        }
    }
}

