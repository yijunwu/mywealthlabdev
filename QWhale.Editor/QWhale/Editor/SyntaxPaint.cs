namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class SyntaxPaint : ISyntaxPaint
    {
        protected Control control;
        protected CustomDrawEventArgs customDrawEventArgs;
        private bool disableColorPaint;
        private bool disableSyntaxPaint;
        protected IDrawInfo drawInfo;
        private ILexer lexer;
        protected IPainter painter;

        [Browsable(false), Description("Occurs when control draws its content.")]
        public event CustomDrawEvent CustomDraw;

        public SyntaxPaint(IPainter painter, Control control)
        {
            this.painter = painter;
            this.control = control;
            this.customDrawEventArgs = new CustomDrawEventArgs();
            this.drawInfo = new DrawInfo();
        }

        public virtual void DrawLine(int index, Point position, Rectangle clipRect)
        {
            string line = string.Empty;
            short[] colorData = null;
            this.GetString(index, ref line, ref colorData);
            this.DrawLine(index, line, colorData, position, clipRect);
        }

        public virtual void DrawLine(int index, string line, short[] colorData, Point position, Rectangle clipRect)
        {
            int right = clipRect.Right;
            int length = line.Length;
            if (line != string.Empty)
            {
                int num3 = (ushort) colorData[0];
                int num4 = num3;
                int startChar = 0;
                for (int i = 1; i < length; i++)
                {
                    num3 = (ushort) colorData[i];
                    if (!this.EqualStyles(num4, num3, true))
                    {
                        position.X += this.DrawTextFragment(position, line, num4, index, startChar, i - 1);
                        if (position.X >= right)
                        {
                            break;
                        }
                        num4 = num3;
                        startChar = i;
                    }
                }
                if ((startChar < length) && (position.X < right))
                {
                    position.X += this.DrawTextFragment(position, line, num4, index, startChar, length - 1);
                }
            }
        }

        private int DrawTextFragment(Point position, string line, int style, int index, int startChar, int endChar)
        {
            TextStyle none = TextStyle.None;
            ILexStyle lexStyle = this.GetLexStyle(style, ref none);
            if (this.disableSyntaxPaint)
            {
                lexStyle = null;
            }
            string text = ((startChar == 0) && (endChar == -1)) ? line : line.Substring(startChar, (endChar - startChar) + 1);
            if (lexStyle != null)
            {
                this.painter.FontStyle = this.GetFontStyle(lexStyle.FontStyle, none);
                this.painter.TextColor = this.GetFontColor(lexStyle.ForeColor, none);
                if (lexStyle.BackColor != Color.Empty)
                {
                    this.painter.BackColor = lexStyle.BackColor;
                }
                else if (this.control != null)
                {
                    this.painter.BackColor = this.control.BackColor;
                }
            }
            else if (this.control != null)
            {
                this.painter.FontStyle = this.GetFontStyle(this.control.Font.Style, none);
                this.painter.TextColor = this.GetFontColor(this.control.ForeColor, none);
                this.painter.BackColor = this.control.BackColor;
            }
            int width = this.painter.StringWidth(text);
            Rectangle rect = new Rectangle(position.X, position.Y, width, this.painter.FontHeight);
            DrawState state = DrawState.Text;
            this.drawInfo.Reset();
            this.drawInfo.Text = text;
            this.drawInfo.Style = (short) style;
            this.drawInfo.Line = index;
            this.drawInfo.Char = startChar;
            if (!this.OnCustomDraw(this.painter, rect, DrawStage.Before, state, this.drawInfo))
            {
                this.painter.TextOut(text, -1, rect, true, false);
            }
            this.OnCustomDraw(this.painter, rect, DrawStage.After, state, this.drawInfo);
            return width;
        }

        protected virtual bool EqualLineStyles(IEditLineStyle style1, IEditLineStyle style2, int style, int oldStyle)
        {
            if (style1 != style2)
            {
                TextStyle none = TextStyle.None;
                TextStyle textStyle = TextStyle.None;
                this.GetLexStyle(style, ref none);
                this.GetLexStyle(style, ref textStyle);
                if ((((none & TextStyle.OutlineSection) == TextStyle.None) && ((none & TextStyle.CodeSnippet) == TextStyle.None)) && ((textStyle & TextStyle.OutlineSection) == TextStyle.None))
                {
                    return ((textStyle & TextStyle.CodeSnippet) != TextStyle.None);
                }
            }
            return true;
        }

        protected virtual bool EqualStates(TextStyle state1, TextStyle state2)
        {
            return (state1 == state2);
        }

        public bool EqualStyles(int style1, int style2, bool useColors)
        {
            TextStyle none = TextStyle.None;
            TextStyle textStyle = TextStyle.None;
            ILexStyle lexStyle = this.GetLexStyle(style1, ref none);
            ILexStyle style5 = this.GetLexStyle(style2, ref textStyle);
            if (this.disableSyntaxPaint)
            {
                lexStyle = null;
                style5 = null;
            }
            if (!this.EqualStates(none, textStyle))
            {
                return false;
            }
            if (lexStyle == style5)
            {
                return true;
            }
            if (((lexStyle == null) || (style5 == null)) || (lexStyle.FontStyle != style5.FontStyle))
            {
                return false;
            }
            return ((!useColors || this.disableColorPaint) || ((lexStyle.ForeColor == style5.ForeColor) && (lexStyle.BackColor == style5.BackColor)));
        }

        public virtual Color GetBackColor(Color color, TextStyle state)
        {
            if (!(color != Color.Empty))
            {
                return this.painter.BackColor;
            }
            return color;
        }

        public virtual Color GetFontColor(Color color, TextStyle textStyle)
        {
            if (this.disableColorPaint && (this.control != null))
            {
                return this.control.ForeColor;
            }
            if (!(color != Color.Empty))
            {
                return this.painter.TextColor;
            }
            return color;
        }

        public virtual FontStyle GetFontStyle(FontStyle fontStyle, TextStyle textStyle)
        {
            return fontStyle;
        }

        public virtual ILexStyle GetLexStyle(int style, ref TextStyle textStyle)
        {
            if (style >= 0)
            {
                byte num = (byte) style;
                textStyle = (TextStyle) (style >> 8);
                if ((num == 0) || (this.Lexer == null))
                {
                    return null;
                }
                if ((num > 0) && ((num - 1) < this.Lexer.Scheme.Styles.Count))
                {
                    return this.Lexer.Scheme.Styles[num - 1];
                }
            }
            return null;
        }

        protected virtual void GetString(int index, ref string line, ref short[] colorData)
        {
        }

        public virtual int MeasureLine(int index, int pos, int len)
        {
            int num;
            string line = string.Empty;
            short[] colorData = null;
            this.GetString(index, ref line, ref colorData);
            return this.MeasureLine(line, colorData, pos, len, -1, out num, false, false, false);
        }

        public virtual int MeasureLine(string line, short[] colorData, int pos, int len)
        {
            int num;
            return this.MeasureLine(line, colorData, pos, len, -1, out num, false, false, false);
        }

        public virtual int MeasureLine(int index, int pos, int len, int width, out int chars, bool exact)
        {
            string line = string.Empty;
            short[] colorData = null;
            this.GetString(index, ref line, ref colorData);
            return this.MeasureLine(line, colorData, pos, len, width, out chars, true, true, exact);
        }

        public int MeasureLine(string line, short[] colorData, int pos, int len, int width, out int chars, bool exact)
        {
            return this.MeasureLine(line, colorData, pos, len, width, out chars, true, true, exact);
        }

        public virtual int MeasureLine(string line, short[] colorData, int pos, int len, int width, out int chars, bool measureChars, bool addSpace, bool exact)
        {
            chars = 0;
            if (measureChars)
            {
                if (width == 0x7fffffff)
                {
                    chars = 0x7fffffff;
                    return 0x7fffffff;
                }
                if (len == 0x7fffffff)
                {
                    return 0x7fffffff;
                }
            }
            else if (len == 0x7fffffff)
            {
                return 0x7fffffff;
            }
            if (line == string.Empty)
            {
                if (!measureChars)
                {
                    return this.painter.CharWidth(' ', len);
                }
                return this.painter.CharWidth(' ', width, out chars);
            }
            int num = 0;
            int num2 = 0;
            int num3 = (len < 0) ? (line.Length - pos) : Math.Min(len, line.Length - pos);
            if ((this.Lexer == null) || this.painter.IsMonoSpaced)
            {
                if (!measureChars)
                {
                    if (num3 >= 0)
                    {
                        num = this.painter.StringWidth(line, 0, num3);
                    }
                    else
                    {
                        num = this.painter.StringWidth(line);
                    }
                    if (len > num3)
                    {
                        num += this.painter.CharWidth(' ', len - num3);
                    }
                    return num;
                }
                num = this.painter.StringWidth(line, width, out chars, exact);
            }
            else
            {
                int num4 = (ushort) colorData[0];
                int num5 = num4;
                int startChar = pos;
                for (int i = pos + 1; i < (pos + num3); i++)
                {
                    num4 = (ushort) colorData[i];
                    if ((num4 != num5) && !this.EqualStyles(num5, num4, false))
                    {
                        num += this.MeasureTextFragment(line, num5, startChar, i - 1, width - num, out num2, measureChars, exact);
                        chars += num2;
                        num5 = num4;
                        if (measureChars && ((width < num) || (num2 < (i - startChar))))
                        {
                            width = -1;
                            break;
                        }
                        startChar = i;
                    }
                }
                if ((startChar < (pos + num3)) && ((measureChars && (width > num)) || !measureChars))
                {
                    num += this.MeasureTextFragment(line, num5, startChar, (pos + num3) - 1, width - num, out num2, measureChars, exact);
                    chars += num2;
                    if (measureChars && (num2 < ((pos + num3) - startChar)))
                    {
                        width = -1;
                    }
                }
                if (!measureChars && (len > num3))
                {
                    TextStyle none = TextStyle.None;
                    ILexStyle lexStyle = this.GetLexStyle(num5, ref none);
                    if (lexStyle != null)
                    {
                        this.painter.FontStyle = this.GetFontStyle(lexStyle.FontStyle, none);
                        num += this.painter.CharWidth(' ', len - num3);
                    }
                    else
                    {
                        if (this.control != null)
                        {
                            this.painter.FontStyle = this.GetFontStyle(this.control.Font.Style, none);
                        }
                        num += this.painter.CharWidth(' ', len - num3);
                    }
                }
            }
            if ((measureChars && addSpace) && (width > num))
            {
                num += this.painter.CharWidth(' ', width - num, out num2);
                chars += num2;
            }
            return num;
        }

        private int MeasureTextFragment(string line, int style, int startChar, int endChar, int width, out int chars, bool measureChars, bool exact)
        {
            TextStyle none = TextStyle.None;
            ILexStyle lexStyle = this.GetLexStyle(style, ref none);
            chars = 0;
            if ((lexStyle != null) && !this.disableSyntaxPaint)
            {
                this.painter.FontStyle = this.GetFontStyle(lexStyle.FontStyle, none);
            }
            else if (this.control != null)
            {
                this.painter.FontStyle = this.GetFontStyle(this.control.Font.Style, none);
            }
            if (measureChars)
            {
                return this.painter.StringWidth(line, startChar, (endChar - startChar) + 1, width, out chars, exact);
            }
            return this.painter.StringWidth(line, startChar, (endChar - startChar) + 1);
        }

        public virtual bool OnCustomDraw(IPainter painter, Rectangle rect, DrawStage stage, DrawState state, IDrawInfo info)
        {
            if (this.CustomDraw != null)
            {
                this.customDrawEventArgs.DrawStage = stage;
                this.customDrawEventArgs.DrawState = state;
                this.customDrawEventArgs.DrawInfo = info;
                this.customDrawEventArgs.Painter = painter;
                this.customDrawEventArgs.Rect = rect;
                this.customDrawEventArgs.Handled = false;
                this.CustomDraw(this, this.customDrawEventArgs);
                return this.customDrawEventArgs.Handled;
            }
            return false;
        }

        protected virtual void OnDisableColorPaintChanged()
        {
        }

        protected virtual void OnDisableSyntaxPaintChanged()
        {
        }

        protected virtual void OnLexerChaged()
        {
        }

        public virtual void PaintLineBookMarks(IPainter painter, Rectangle rect)
        {
        }

        public virtual void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint)
        {
            int fontHeight = painter.FontHeight;
            for (int i = startLine; i < endLine; i++)
            {
                this.DrawLine(i, position, rect);
                position.Y += fontHeight;
                if (position.Y > rect.Bottom)
                {
                    break;
                }
                if ((position.Y == rect.Bottom) && specialPaint)
                {
                    return;
                }
            }
        }

        public virtual void ResetDisableColorPaint()
        {
            this.DisableColorPaint = false;
        }

        public virtual void ResetDisableSyntaxPaint()
        {
            this.DisableSyntaxPaint = false;
        }

        [DefaultValue(false), Description("Gets or sets a value indicating whether control uses colors to paint its content.")]
        public virtual bool DisableColorPaint
        {
            get
            {
                return this.disableColorPaint;
            }
            set
            {
                if (this.disableColorPaint != value)
                {
                    this.disableColorPaint = value;
                    this.OnDisableColorPaintChanged();
                }
            }
        }

        [Description("Gets or sets a value indicating whether Edit control uses lexical colors/styles to paint its content."), DefaultValue(false)]
        public virtual bool DisableSyntaxPaint
        {
            get
            {
                return this.disableSyntaxPaint;
            }
            set
            {
                if (this.disableSyntaxPaint != value)
                {
                    this.disableSyntaxPaint = value;
                    this.OnDisableSyntaxPaintChanged();
                }
            }
        }

        [Description("Gets or sets object that can make lexical analysis for the control's content.")]
        public virtual ILexer Lexer
        {
            get
            {
                return this.lexer;
            }
            set
            {
                if (this.lexer != value)
                {
                    this.lexer = value;
                    this.OnLexerChaged();
                }
            }
        }
    }
}

