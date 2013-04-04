namespace QWhale.Editor.Serialization
{
    using QWhale.Editor;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;

    public class HtmlExport : FmtExport
    {
        private bool colorChanged;
        private const string htmlAmp = "&amp;";
        private const string htmlB = "b";
        private const string htmlBody = "body";
        private const string htmlBr = "br";
        private const string htmlColor = "color";
        private const string htmlFont = "font";
        private const string htmlHead = "head";
        private const string htmlHtml = "html";
        private const string htmlI = "i";
        private const string htmlLt = "&lt;";
        private const string htmlName = "name";
        private const string htmlNbsp = "&nbsp;";
        private const string htmlQt = "&gt;";
        private const string htmlQuot = "&quot;";
        private const string htmlStrike = "strike";
        private const string htmlTitle = "title";
        private const string htmlU = "u";
        private StringBuilder rBuilder = new StringBuilder();

        protected override void AddFontStyle(FontStyle style)
        {
            switch (style)
            {
                case FontStyle.Bold:
                    this.WriteTag("b");
                    return;

                case FontStyle.Italic:
                    this.WriteTag("i");
                    return;

                case (FontStyle.Italic | FontStyle.Bold):
                    break;

                case FontStyle.Underline:
                    this.WriteTag("u");
                    return;

                case FontStyle.Strikeout:
                    this.WriteTag("strike");
                    break;

                default:
                    return;
            }
        }

        public override void BeginWrite(TextWriter writer, object userData)
        {
            base.BeginWrite(writer, userData);
            this.WriteTag("html");
            writer.WriteLine();
            this.WriteTag("head");
            if (base.edit != null)
            {
                this.WriteTag("title");
                writer.Write(base.edit.Source.FileName);
                this.WriteEndTag("title");
            }
            this.WriteEndTag("head");
            writer.WriteLine();
            this.WriteTag("body");
            writer.WriteLine();
            if (base.edit != null)
            {
                this.WriteTag("font", "name", base.edit.Font.Name);
            }
            writer.WriteLine();
        }

        private string ColorToHtml(Color color)
        {
            return ("#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2"));
        }

        public override void EndWrite()
        {
            if (this.colorChanged)
            {
                this.WriteEndTag("font");
            }
            this.WriteFontStyle(FontStyle.Regular);
            this.WriteEndTag("font");
            base.writer.WriteLine();
            this.WriteEndTag("body");
            base.writer.WriteLine();
            this.WriteEndTag("html");
        }

        protected override void RemoveFontStyle(FontStyle style)
        {
            switch (style)
            {
                case FontStyle.Bold:
                    this.WriteEndTag("b");
                    return;

                case FontStyle.Italic:
                    this.WriteEndTag("i");
                    return;

                case (FontStyle.Italic | FontStyle.Bold):
                    break;

                case FontStyle.Underline:
                    this.WriteEndTag("u");
                    return;

                case FontStyle.Strikeout:
                    this.WriteEndTag("strike");
                    break;

                default:
                    return;
            }
        }

        protected override void StartLine()
        {
            if (!base.firstLine)
            {
                base.writer.WriteLine();
                this.WriteTag("br");
            }
        }

        private string StringToHtml(int pos, string str)
        {
            if ((str == string.Empty) || (str == null))
            {
                return str;
            }
            this.rBuilder.Length = 0;
            bool flag = true;
            foreach (char ch in str)
            {
                switch (ch)
                {
                    case '<':
                        this.rBuilder.Append("&lt;");
                        goto Label_0167;

                    case '>':
                        this.rBuilder.Append("&gt;");
                        goto Label_0167;

                    case '&':
                        this.rBuilder.Append("&amp;");
                        goto Label_0167;

                    case ' ':
                        if (!flag)
                        {
                            break;
                        }
                        this.rBuilder.Append("&nbsp;");
                        goto Label_0167;

                    case '"':
                        this.rBuilder.Append("&quot;");
                        goto Label_0167;

                    case '\t':
                    {
                        int count = (base.edit != null) ? (base.edit.Lines.GetTabStop(pos) - pos) : EditConsts.DefaultTabStop;
                        if (flag)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                this.rBuilder.Append("&nbsp;");
                            }
                        }
                        else
                        {
                            this.rBuilder.Append(new string(' ', count));
                        }
                        pos += count;
                        goto Label_0167;
                    }
                    default:
                        this.rBuilder.Append(ch);
                        goto Label_0167;
                }
                this.rBuilder.Append(ch);
            Label_0167:
                if ((ch != ' ') && (ch != '\t'))
                {
                    flag = false;
                }
                if (ch != '\t')
                {
                    pos++;
                }
            }
            return this.rBuilder.ToString();
        }

        protected override void WriteBackColor(Color backColor)
        {
        }

        private void WriteEndTag(string tag)
        {
            base.writer.Write("</" + tag + ">");
        }

        protected override void WriteForeColor(Color foreColor)
        {
            if (this.colorChanged)
            {
                this.WriteEndTag("font");
            }
            this.WriteTag("font", "color", this.ColorToHtml(foreColor));
            this.colorChanged = true;
        }

        private void WriteTag(string tag)
        {
            base.writer.Write("<" + tag + ">");
        }

        private void WriteTag(string tag, string name, string value)
        {
            base.writer.Write(string.Concat(new object[] { "<", tag, ' ', name, "=", value, ">" }));
        }

        protected override void WriteText(int pos, string text)
        {
            base.writer.Write(this.StringToHtml(pos, text));
        }
    }
}

