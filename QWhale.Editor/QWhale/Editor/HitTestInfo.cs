namespace QWhale.Editor
{
    using QWhale.Syntax;
    using System;

    public class HitTestInfo : IHitTestInfo
    {
        private int gutterImage = -1;
        private QWhale.Editor.HitTest hitTest;
        private IStringItem item;
        private int line = -1;
        private int outlineIndex = -1;
        private IOutlineRange outlineRange;
        private int page = -1;
        private int pos = -1;
        private string str;
        private int style = -1;
        private QWhale.Syntax.TextStyle textStyle;
        private string url;
        private string word;

        public void Reset()
        {
            this.hitTest = QWhale.Editor.HitTest.None;
            this.line = -1;
            this.pos = -1;
            this.gutterImage = -1;
            this.outlineIndex = -1;
            this.page = -1;
            this.style = -1;
            this.item = null;
            this.str = null;
            this.word = null;
            this.url = null;
            this.outlineRange = null;
            this.textStyle = QWhale.Syntax.TextStyle.None;
        }

        public virtual int GutterImage
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

        public virtual QWhale.Editor.HitTest HitTest
        {
            get
            {
                return this.hitTest;
            }
            set
            {
                this.hitTest = value;
            }
        }

        public virtual IStringItem Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }

        public virtual int Line
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

        public virtual int OutlineIndex
        {
            get
            {
                return this.outlineIndex;
            }
            set
            {
                this.outlineIndex = value;
            }
        }

        public virtual IOutlineRange OutlineRange
        {
            get
            {
                return this.outlineRange;
            }
            set
            {
                this.outlineRange = value;
            }
        }

        public virtual int Page
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

        public virtual int Pos
        {
            get
            {
                return this.pos;
            }
            set
            {
                this.pos = value;
            }
        }

        public virtual string String
        {
            get
            {
                return this.str;
            }
            set
            {
                this.str = value;
            }
        }

        public virtual int Style
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

        public virtual QWhale.Syntax.TextStyle TextStyle
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

        public virtual string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }

        public virtual string Word
        {
            get
            {
                return this.word;
            }
            set
            {
                this.word = value;
            }
        }
    }
}

