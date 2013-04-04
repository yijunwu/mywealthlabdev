namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class HtmlSyntaxPaint : SyntaxPaint
    {
        private HtmlLexer internalLexer;
        private ICodeCompletionProvider provider;

        public HtmlSyntaxPaint(IPainter painter, Control control) : base(painter, control)
        {
            this.internalLexer = new HtmlLexer();
        }

        protected virtual void AddLine(string s, short[] data)
        {
        }

        protected void AddMultiLine(string s, short[] data)
        {
            int start = 0;
            int num2 = 0;
            int length = s.Length;
            while (num2 < length)
            {
                if ((s[num2] == '\r') || (s[num2] == '\n'))
                {
                    this.AddSubstring(s, data, start, num2 - start);
                    if (((s[num2] == '\r') && (num2 < (length - 1))) && (s[num2 + 1] == '\n'))
                    {
                        num2++;
                    }
                    num2++;
                    start = num2;
                }
                num2++;
            }
            this.AddSubstring(s, data, start, length - start);
        }

        protected void AddMultiLine(string s, int state, bool useLexer)
        {
            s = s.Replace("\t", new string(' ', EditConsts.DefaultSpacesInTab));
            short[] colorData = null;
            if (useLexer && this.PrepareData(state, FontStyle.Regular, ref s, ref colorData))
            {
                this.AddMultiLine(s, colorData);
            }
            else
            {
                int line = 0;
                foreach (string str in StringItem.Split(s))
                {
                    colorData = new short[str.Length];
                    if (useLexer && (this.Lexer != null))
                    {
                        state = this.Lexer.ParseText(state, line, str, ref colorData);
                    }
                    line++;
                    this.AddLine(str, colorData);
                }
            }
        }

        protected void AddSubstring(string s, short[] data, int start, int len)
        {
            short[] destinationArray = new short[len];
            Array.Copy(data, start, destinationArray, 0, len);
            this.AddLine(s.Substring(start, len), destinationArray);
        }

        protected bool IsDelimiter(string s, int pos)
        {
            char ch = s[pos];
            if (ch > ' ')
            {
                return (EditConsts.DefaultDelimiters.IndexOf(ch) >= 0);
            }
            return true;
        }

        protected virtual void OnProviderChanged()
        {
            if ((this.provider != null) && this.provider.UseHtmlFormatting)
            {
                if (this.Lexer == null)
                {
                    this.Lexer = this.internalLexer;
                }
            }
            else if (this.Lexer == this.internalLexer)
            {
                this.Lexer = null;
            }
        }

        public bool PrepareData(int state, FontStyle style, ref string s, ref short[] colorData)
        {
            if (this.Lexer is HtmlLexer)
            {
                ((HtmlLexer) this.Lexer).ParseHtmlText(state, style, ref s, ref colorData);
            }
            else
            {
                colorData = new short[s.Length];
            }
            return (this.Lexer is HtmlLexer);
        }

        public ICodeCompletionProvider Provider
        {
            get
            {
                return this.provider;
            }
            set
            {
                if (this.provider != value)
                {
                    this.provider = value;
                    this.OnProviderChanged();
                }
            }
        }
    }
}

