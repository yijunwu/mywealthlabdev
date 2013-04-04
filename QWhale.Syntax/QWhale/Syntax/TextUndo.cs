namespace QWhale.Syntax
{
    using System;

    public class TextUndo : ITextUndo
    {
        private int len;
        private int start;
        private string text = string.Empty;

        public TextUndo(int start, int len, string text)
        {
            this.start = start;
            this.len = len;
            this.text = text;
        }

        public int Len
        {
            get
            {
                return this.len;
            }
            set
            {
                this.len = value;
            }
        }

        public int Start
        {
            get
            {
                return this.start;
            }
            set
            {
                this.start = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}

