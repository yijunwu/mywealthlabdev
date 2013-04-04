namespace QWhale.Editor.Serialization
{
    using QWhale.Syntax;
    using System;
    using System.Drawing;
    using System.Globalization;

    public class HtmlImporter : FmtImport
    {
        private const string htmlAmp = "&amp;";
        private const string htmlB = "b";
        private const string htmlBr = "br";
        private const string htmlColor = "color";
        private const string htmlFont = "font";
        private const string htmlI = "i";
        private const string htmlLt = "&lt;";
        private const string htmlNbsp = "&nbsp;";
        private const string htmlP = "p";
        private const string htmlQt = "&gt;";
        private const string htmlQuot = "&quot;";
        private const string htmlStrike = "strike";
        private const string htmlStrong = "strong";
        private const string htmlU = "u";
        private SyntaxParser parser = new HtmlParser();

        private Color HtmlToColor(string text)
        {
            if (!(text != string.Empty))
            {
                return Color.Empty;
            }
            if (text[0] == '#')
            {
                return Color.FromArgb(this.StringToHex(text, 1, 2), this.StringToHex(text, 3, 2), this.StringToHex(text, 5, 2));
            }
            return Color.FromName(text);
        }

        private string HtmlToString(string text)
        {
            return text.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&nbsp;", " ");
        }

        protected void ProcessFontTag()
        {
            while (!this.parser.Eof)
            {
                switch (this.parser.NextToken())
                {
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        return;

                    case 9:
                        if (((this.parser.TokenString.ToLower() == "color") && (this.parser.NextToken() == 7)) && (this.parser.NextToken() == 10))
                        {
                            base.WriteForeColor(this.HtmlToColor(this.parser.TokenString));
                        }
                        break;
                }
            }
        }

        protected void ProcessTag(bool negate)
        {
            if (this.parser.NextToken() == 8)
            {
                string tag = this.parser.TokenString.ToLower();
                if (!negate)
                {
                    base.Push(tag);
                    switch (tag)
                    {
                        case "br":
                        case "p":
                            base.WriteString("\r\n");
                            return;

                        case "b":
                        case "strong":
                            base.WriteFontStyle(FontStyle.Bold, true);
                            return;

                        case "i":
                            base.WriteFontStyle(FontStyle.Italic, true);
                            return;

                        case "strike":
                            base.WriteFontStyle(FontStyle.Strikeout, true);
                            return;

                        case "u":
                            base.WriteFontStyle(FontStyle.Underline, true);
                            return;

                        case "font":
                            this.ProcessFontTag();
                            return;
                    }
                }
                else
                {
                    base.Pop(tag);
                }
            }
        }

        protected override bool ReadContent()
        {
            bool flag = base.ReadContent();
            this.parser.Strings = new StringList(base.reader);
            this.parser.Reset();
            int y = 0;
            int num2 = 0;
            while (!this.parser.Eof)
            {
                switch (this.parser.NextToken())
                {
                    case 2:
                        if (((y == this.parser.CurrentPosition.Y) || (num2 == 6)) || (num2 == 4))
                        {
                            break;
                        }
                        this.WriteText(' '.ToString() + this.parser.TokenString);
                        goto Label_00BC;

                    case 3:
                        this.ProcessTag(false);
                        goto Label_00BC;

                    case 5:
                        this.ProcessTag(true);
                        goto Label_00BC;

                    default:
                        goto Label_00BC;
                }
                this.WriteText(this.parser.TokenString);
            Label_00BC:
                num2 = this.parser.Token;
                y = this.parser.CurrentPosition.Y;
            }
            return flag;
        }

        private int StringToHex(string text, int pos, int len)
        {
            if (text.Length >= (pos + len))
            {
                try
                {
                    return int.Parse(text.Substring(pos, len), NumberStyles.HexNumber);
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }

        protected void WriteText(string text)
        {
            base.WriteString(this.HtmlToString(text));
        }
    }
}

