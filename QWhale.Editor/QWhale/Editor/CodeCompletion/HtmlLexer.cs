namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Drawing;
    using System.IO;

    public class HtmlLexer : QWhale.Syntax.Lexer.Lexer
    {
        private IFmtImport importer = new HtmlImporter();

        public HtmlLexer()
        {
            this.importer.ReadFormattedText += new ReadFormattedTextEvent(this.ReadFormattedText);
        }

        protected int GetStyle(Color foreColor, Color backColor, FontStyle fontStyle)
        {
            foreach (ILexStyle style in this.Scheme.Styles)
            {
                if (((style.ForeColor == foreColor) && (style.BackColor == backColor)) && (style.FontStyle == fontStyle))
                {
                    return (style.Index + 1);
                }
            }
            ILexStyle style2 = this.Scheme.Styles.AddLexStyle();
            style2.ForeColor = foreColor;
            style2.BackColor = backColor;
            style2.FontStyle = fontStyle;
            return (style2.Index + 1);
        }

        public int ParseHtmlText(int state, FontStyle style, ref string s, ref short[] colorData)
        {
            StringReader reader = new StringReader(s);
            HtmlItem userData = new HtmlItem(string.Empty);
            this.importer.DefaultStyle = style;
            this.importer.BeginRead(reader, userData);
            try
            {
                this.importer.Read();
            }
            finally
            {
                this.importer.EndRead();
            }
            s = userData.String;
            colorData = userData.TextData;
            return state;
        }

        protected void ReadFormattedText(object sender, ReadFormattedTextEventArgs e)
        {
            if (e.UserData is HtmlItem)
            {
                ((HtmlItem) e.UserData).Add(e.Text, (short) this.GetStyle(e.ForeColor, e.BackColor, e.FontStyle));
            }
        }

        private class HtmlItem : StringItem
        {
            public HtmlItem(string s) : base(s)
            {
            }

            public void Add(string text, short style)
            {
                int length = this.TextData.Length;
                short[] array = new short[length + text.Length];
                if (length > 0)
                {
                    this.TextData.CopyTo(array, 0);
                }
                this.String = this.String + text;
                for (int i = 0; i < text.Length; i++)
                {
                    array[length + i] = style;
                }
                this.TextData = array;
            }
        }
    }
}

