namespace QWhale.Editor.Serialization
{
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Drawing;
    using System.IO;

    public class FmtExport : IFmtExport, IStringExport
    {
        protected Color backColor = Consts.DefaultControlBackColor;
        protected ISyntaxEdit edit;
        protected bool firstLine = true;
        protected FontStyle fontStyle;
        protected Color foreColor = Consts.DefaultControlForeColor;
        protected TextWriter writer;

        protected virtual void AddFontStyle(FontStyle style)
        {
        }

        protected virtual void ApplyStyle(FontStyle style, Color foreColor, Color backColor)
        {
            if ((foreColor != this.foreColor) && (foreColor != Color.Empty))
            {
                this.WriteForeColor(foreColor);
                this.foreColor = foreColor;
            }
            if ((backColor != this.backColor) && (backColor != Color.Empty))
            {
                this.WriteBackColor(backColor);
                this.backColor = backColor;
            }
            if (style != this.fontStyle)
            {
                this.WriteFontStyle(style);
                this.fontStyle = style;
            }
        }

        public virtual void BeginWrite(TextWriter writer, object userData)
        {
            this.writer = writer;
            this.edit = userData as ISyntaxEdit;
            this.firstLine = true;
        }

        protected virtual void EndLine()
        {
        }

        public virtual void EndWrite()
        {
        }

        protected virtual void RemoveFontStyle(FontStyle style)
        {
        }

        protected virtual void StartLine()
        {
        }

        public virtual bool Write()
        {
            return false;
        }

        protected virtual void WriteBackColor(Color backColor)
        {
        }

        protected virtual void WriteFontStyle(FontStyle style)
        {
            FontStyle style2 = this.fontStyle & ~style;
            FontStyle style3 = style & ~this.fontStyle;
            if ((style2 & FontStyle.Strikeout) != FontStyle.Regular)
            {
                this.RemoveFontStyle(FontStyle.Strikeout);
            }
            if ((style2 & FontStyle.Underline) != FontStyle.Regular)
            {
                this.RemoveFontStyle(FontStyle.Underline);
            }
            if ((style2 & FontStyle.Italic) != FontStyle.Regular)
            {
                this.RemoveFontStyle(FontStyle.Italic);
            }
            if ((style2 & FontStyle.Bold) != FontStyle.Regular)
            {
                this.RemoveFontStyle(FontStyle.Bold);
            }
            if ((style3 & FontStyle.Bold) != FontStyle.Regular)
            {
                this.AddFontStyle(FontStyle.Bold);
            }
            if ((style3 & FontStyle.Italic) != FontStyle.Regular)
            {
                this.AddFontStyle(FontStyle.Italic);
            }
            if ((style3 & FontStyle.Underline) != FontStyle.Regular)
            {
                this.AddFontStyle(FontStyle.Underline);
            }
            if ((style3 & FontStyle.Strikeout) != FontStyle.Regular)
            {
                this.AddFontStyle(FontStyle.Strikeout);
            }
        }

        protected virtual void WriteForeColor(Color foreColor)
        {
        }

        public virtual void WriteLine(IStringItem item)
        {
            int length = item.String.Length;
            this.StartLine();
            this.firstLine = false;
            IEditSyntaxPaint syntaxPaint = (this.edit != null) ? this.edit.SyntaxPaint : null;
            if ((syntaxPaint != null) && (item.String != string.Empty))
            {
                int num = item.TextData[0];
                int num2 = num;
                int start = 0;
                for (int i = 1; i < length; i++)
                {
                    num = item.TextData[i];
                    if ((num != num2) && !syntaxPaint.EqualStyles(num2, num, true))
                    {
                        this.WriteLine(syntaxPaint, item.String, num2, start, i - 1);
                        num2 = num;
                        start = i;
                    }
                }
                if (start < length)
                {
                    this.WriteLine(syntaxPaint, item.String, num2, start, length - 1);
                }
            }
            else
            {
                this.WriteLine(null, item.String, -1, 0, length - 1);
            }
            this.EndLine();
        }

        protected virtual void WriteLine(IEditSyntaxPaint syntaxPaint, string line, int style, int start, int end)
        {
            TextStyle none = TextStyle.None;
            ILexStyle style3 = (syntaxPaint != null) ? syntaxPaint.GetLexStyle(style, ref none) : null;
            if (style3 != null)
            {
                this.ApplyStyle(syntaxPaint.GetFontStyle(style3.FontStyle, none), (style3.ForeColor != Color.Empty) ? syntaxPaint.GetFontColor(style3.ForeColor, none) : syntaxPaint.GetForeColor(false), (style3.BackColor != Color.Empty) ? style3.BackColor : syntaxPaint.GetBackColor(false));
            }
            else if (syntaxPaint != null)
            {
                this.ApplyStyle(syntaxPaint.GetFontStyle(FontStyle.Regular, none), syntaxPaint.GetFontColor(syntaxPaint.GetForeColor(false), none), syntaxPaint.GetBackColor(false));
            }
            this.WriteText(start, line.Substring(start, (end - start) + 1));
        }

        protected virtual void WriteText(int pos, string text)
        {
        }
    }
}

