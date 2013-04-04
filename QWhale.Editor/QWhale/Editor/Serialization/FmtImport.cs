namespace QWhale.Editor.Serialization
{
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class FmtImport : IFmtImport, IStringImport
    {
        private FontStyle defaultStyle;
        protected ISyntaxEdit edit;
        protected TextReader reader;
        protected ReadFormattedTextEventArgs readFormattedArgs = new ReadFormattedTextEventArgs();
        private IList<FormattedItem> stack = new List<FormattedItem>();

        public event ReadFormattedTextEvent ReadFormattedText;

        public virtual void BeginRead(TextReader reader, object userData)
        {
            this.reader = reader;
            this.edit = userData as ISyntaxEdit;
            this.readFormattedArgs.UserData = userData;
        }

        public virtual void EndRead()
        {
        }

        protected void GetCurrentAttributes(out Color foreColor, out Color backColor, out FontStyle fontStyle)
        {
            foreColor = Color.Empty;
            backColor = Color.Empty;
            fontStyle = this.DefaultStyle;
            foreach (FormattedItem item in this.stack)
            {
                if (item.ForeColor != Color.Empty)
                {
                    foreColor = item.ForeColor;
                }
                if (item.BackColor != Color.Empty)
                {
                    backColor = item.BackColor;
                }
                if (item.StyleValid)
                {
                    fontStyle = (fontStyle | item.FontStyle) & ~item.ExcludeStyle;
                }
            }
        }

        protected FormattedItem GetCurrentItem()
        {
            if (this.stack.Count == 0)
            {
                this.stack.Add(new FormattedItem());
            }
            return this.stack[this.stack.Count - 1];
        }

        protected virtual void OnReadFormattedText(Color foreColor, Color backColor, FontStyle fontStyle, string text)
        {
            if (this.ReadFormattedText != null)
            {
                this.readFormattedArgs.ForeColor = foreColor;
                this.readFormattedArgs.BackColor = backColor;
                this.readFormattedArgs.FontStyle = fontStyle;
                this.readFormattedArgs.Text = text;
                this.ReadFormattedText(this, this.readFormattedArgs);
            }
        }

        protected void Pop(string tag)
        {
            for (int i = this.stack.Count - 1; i >= 0; i--)
            {
                FormattedItem item = this.stack[i];
                if (item.Tag == tag)
                {
                    this.stack.RemoveAt(i);
                    return;
                }
            }
        }

        protected void Push(string tag)
        {
            FormattedItem item = new FormattedItem {
                Tag = tag
            };
            this.stack.Add(item);
        }

        public virtual bool Read()
        {
            return (this.ReadHeader() && this.ReadContent());
        }

        protected virtual bool ReadContent()
        {
            this.stack.Clear();
            return true;
        }

        protected virtual bool ReadHeader()
        {
            return true;
        }

        protected void WriteBackColor(Color backColor)
        {
            this.GetCurrentItem().BackColor = backColor;
        }

        protected void WriteFontStyle(FontStyle style, bool value)
        {
            FormattedItem currentItem = this.GetCurrentItem();
            currentItem.StyleValid = true;
            if (value)
            {
                currentItem.ExcludeStyle &= ~style;
                currentItem.FontStyle |= style;
            }
            else
            {
                currentItem.FontStyle &= ~style;
                currentItem.ExcludeStyle |= style;
            }
        }

        protected void WriteForeColor(Color foreColor)
        {
            this.GetCurrentItem().ForeColor = foreColor;
        }

        protected void WriteString(string s)
        {
            Color color;
            Color color2;
            FontStyle style;
            this.GetCurrentAttributes(out color, out color2, out style);
            this.OnReadFormattedText(color, color2, style, s);
        }

        public FontStyle DefaultStyle
        {
            get
            {
                return this.defaultStyle;
            }
            set
            {
                this.defaultStyle = value;
            }
        }

        internal protected class FormattedItem
        {
            public Color BackColor;
            public System.Drawing.FontStyle ExcludeStyle;
            public System.Drawing.FontStyle FontStyle;
            public Color ForeColor;
            public bool StyleValid;
            public string Tag;
        }
    }
}

