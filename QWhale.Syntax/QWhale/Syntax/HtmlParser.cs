namespace QWhale.Syntax
{
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;

    [ToolboxItem(true)]
    public class HtmlParser : XmlParser
    {
        protected LexerProc lexParamColorProc;

        protected override int GetLexerStyle(int token)
        {
            if (token == 11)
            {
                return 3;
            }
            return base.GetLexerStyle(token);
        }

        protected override void InitLanguage()
        {
            this.Scheme.FileType = "html";
        }

        protected override void InitLexer()
        {
            base.InitLexer();
            this.lexParamColorProc = new LexerProc(this.LexParamColor);
            base.RegisterLexerProc(2, '#', this.lexParamColorProc);
        }

        protected override void InitStyles()
        {
            this.InitDefaultStyles();
        }

        protected virtual int LexParamColor()
        {
            base.currentPos++;
            int length = base.source.Length;
            while (base.currentPos < length)
            {
                char ch = base.source[base.currentPos];
                if ((((ch < '0') || (ch > '9')) && ((ch < 'a') || (ch > 'f'))) && ((ch < 'A') || (ch > 'F')))
                {
                    break;
                }
                base.currentPos++;
            }
            return 10;
        }

        public override void ResetAutoIndentChars()
        {
            this.AutoIndentChars = SyntaxConsts.DefaultHtmlAutoIndentChars.ToCharArray();
        }
    }
}

