namespace QWhale.Editor
{
    using QWhale.Syntax;
    using System;

    public class DrawInfo : IDrawInfo
    {
        private int ch = -1;
        private int gutterImage = -1;
        private int line = -1;
        private int page = -1;
        private bool selection;
        private short style = -1;
        private string text = string.Empty;
        private QWhale.Syntax.TextStyle textStyle;

        public void Reset()
        {
            this.text = string.Empty;
            this.selection = false;
            this.style = -1;
            this.textStyle = QWhale.Syntax.TextStyle.None;
            this.ch = -1;
            this.line = -1;
            this.page = -1;
            this.gutterImage = -1;
        }

        public int Char
        {
            get
            {
                return this.ch;
            }
            set
            {
                this.ch = value;
            }
        }

        public int GutterImage
        {
            get
            {
                return this.gutterImage;
            }
            set
            {
                this.gutterImage = value;
            }
        }

        public int Line
        {
            get
            {
                return this.line;
            }
            set
            {
                this.line = value;
            }
        }

        public int Page
        {
            get
            {
                return this.page;
            }
            set
            {
                this.page = value;
            }
        }

        public bool Selection
        {
            get
            {
                return this.selection;
            }
            set
            {
                this.selection = value;
            }
        }

        public short Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
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

        public QWhale.Syntax.TextStyle TextStyle
        {
            get
            {
                return this.textStyle;
            }
            set
            {
                this.textStyle = value;
            }
        }
    }
}

