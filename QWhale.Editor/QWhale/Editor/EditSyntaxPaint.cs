namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class EditSyntaxPaint : SyntaxPaint, IEditSyntaxPaint, ISyntaxPaint
    {
        private IEditBraceMatching braces;
        private Color columnsIndentForeColor;
        private Color disabledBackColor;
        private Color disabledForeColor;
        private IDisplayStrings displayLines;
        private bool drawColumnsIndent;
        private bool drawSelection;
        private ILexStyle errorStyle;
        private IGutter gutter;
        private IEditHyperText hyperText;
        private bool inPrinting;
        private IList<ILineStyle> internalLineStyles;
        private ILineSeparator lineSeparator;
        private IEditLineStyles lineStyles;
        private IOutlining outlining;
        private ISyntaxEdit owner;
        private Color readonlyBackColor;
        private Color readonlyForeColor;
        private Color selBackColor;
        private ISelection selection;
        private Color selForeColor;
        private ILexStyle snippetStyle;
        private IEditSpelling spelling;
        private Hashtable syntaxErrors;
        private bool syntaxErrorsHints;
        private IWhiteSpace whiteSpace;

        public EditSyntaxPaint(IPainter painter, ISyntaxEdit owner) : base(painter, (Control) owner)
        {
            this.readonlyForeColor = Color.Empty;
            this.readonlyBackColor = Color.Empty;
            this.disabledForeColor = Color.Empty;
            this.disabledBackColor = Color.Empty;
            this.columnsIndentForeColor = EditConsts.DefaultColumnsIndentForeColor;
            this.syntaxErrorsHints = true;
            this.owner = owner;
            this.displayLines = owner.DisplayLines;
            this.whiteSpace = owner.WhiteSpace;
            this.selection = owner.Selection;
            this.lineStyles = owner.LineStyles;
            this.lineSeparator = owner.LineSeparator;
            this.gutter = owner.Gutter;
            this.braces = owner.Braces;
            this.outlining = owner.Outlining;
            this.spelling = owner.Spelling;
            this.hyperText = owner.HyperText;
            this.internalLineStyles = new List<ILineStyle>();
        }

        private void DrawAfterLineEnd(int w, Point position, IEditLineStyle lineStyle, bool selLine, bool emptySel, int offset, int left, int right, int index, bool highlight, bool readOnly, bool drawColumnIndent, int start, int firstIndentChar)
        {
            if (w > position.X)
            {
                Color color = ((lineStyle != null) && ((lineStyle.Options & LineStyleOptions.BeyondEol) != LineStyleOptions.None)) ? lineStyle.GetBackColor(this.GetBackColor(readOnly)) : ((highlight && (this.lineSeparator.HighlightBackColor != Color.Empty)) ? this.lineSeparator.HighlightBackColor : this.GetBackColor(readOnly));
                bool flag = (selLine && ((SelectionOptions.SelectBeyondEol & this.selection.Options) != SelectionOptions.None)) || ((this.selection.SelectionType == SelectionType.Block) || emptySel);
                bool flag2 = ((this.owner.Transparent || this.inPrinting) && !highlight) && (lineStyle == null);
                int fontHeight = base.painter.FontHeight;
                if (flag)
                {
                    if (emptySel)
                    {
                        right = left + 1;
                    }
                    int displayWidth = this.gutter.DisplayWidth;
                    int num3 = this.MeasureLine(index, 0, left);
                    if (num3 != 0x7fffffff)
                    {
                        num3 -= offset - displayWidth;
                    }
                    int x = this.MeasureLine(index, 0, right);
                    if (x != 0x7fffffff)
                    {
                        x -= offset - displayWidth;
                    }
                    else
                    {
                        x = w;
                    }
                    if (x > position.X)
                    {
                        num3 = Math.Max(num3, position.X);
                        if ((num3 > position.X) && !flag2)
                        {
                            base.painter.BackColor = color;
                            this.FillRectangleAfterLineEnd(position.X, position.Y, num3 - position.X, fontHeight, index, false, highlight, lineStyle);
                        }
                        base.painter.BackColor = this.selBackColor;
                        this.FillRectangleAfterLineEnd(num3, position.Y, x - num3, fontHeight, index, true, highlight, lineStyle);
                        if ((x < w) && !flag2)
                        {
                            base.painter.BackColor = color;
                            this.FillRectangleAfterLineEnd(x, position.Y, w - x, fontHeight, index, false, highlight, lineStyle);
                        }
                        return;
                    }
                }
                if (!flag2)
                {
                    base.painter.BackColor = color;
                    this.FillRectangleAfterLineEnd(position.X, position.Y, w - position.X, base.painter.FontHeight, index, false, highlight, lineStyle);
                }
                if (drawColumnIndent)
                {
                    for (int i = 1; i <= firstIndentChar; i++)
                    {
                        if ((i == this.owner.Lines.GetTabStop(i - 1)) && ((!selLine || (i < left)) || (i > right)))
                        {
                            int num6 = base.painter.CharWidth(' ', i) + start;
                            this.DrawColumnIndent(num6, position.Y, position.Y + fontHeight);
                        }
                    }
                }
            }
        }

        private void DrawCodeSnippets(IPainter painter)
        {
            ITextSource source = this.owner.Source;
            if ((source.CurrentSnippet != null) && source.CodeSnippets.IsFirstSnippet(source.CurrentSnippet))
            {
                foreach (ICodeSnippetRange range in source.CodeSnippets)
                {
                    if ((range.ID == source.CurrentSnippet.ID) && ((range.EndPoint.Y != range.StartPoint.Y) || (range.EndPoint.X != range.StartPoint.X)))
                    {
                        Point location = this.owner.TextToScreen(range.StartPoint);
                        Point point2 = this.owner.TextToScreen(range.EndPoint);
                        Rectangle rectangle = new Rectangle(location, new Size(point2.X - location.X, (point2.Y - location.Y) + painter.FontHeight));
                        Color color = (this.snippetStyle != null) ? this.snippetStyle.ForeColor : SyntaxConsts.DefaultCodeSnippetForeColor;
                        if (range == source.CurrentSnippet)
                        {
                            Color backColor = painter.BackColor;
                            painter.BackColor = color;
                            painter.DrawRectangle(rectangle.Left, rectangle.Top, rectangle.Width - 1, rectangle.Height - 1);
                            painter.BackColor = backColor;
                        }
                        else
                        {
                            painter.DrawFocusRect(rectangle.Left, rectangle.Top, rectangle.Width - 1, rectangle.Height - 1, color);
                        }
                    }
                }
            }
        }

        private void DrawColumnIndent(int x, int top, int bottom)
        {
            Color foreColor = base.painter.ForeColor;
            base.painter.ForeColor = this.ColumnsIndentForeColor;
            base.painter.DrawLine(x, top, x, bottom);
            base.painter.ForeColor = foreColor;
        }

        private bool DrawEndLine(Rectangle rect, Point position, int endLine, bool readOnly, IEditLineStyle lineStyle)
        {
            if (this.whiteSpace.Visible && (this.whiteSpace.EofSymbol != '\0'))
            {
                int right = rect.Right;
                int count = this.owner.Lines.Count;
                bool highlight = this.lineSeparator.NeedHighlightLine(count);
                position.X += this.DrawTextFragment(position, this.whiteSpace.EofString, 0x100, lineStyle, false, endLine, 0, 0, true, highlight, readOnly, false);
                this.DrawAfterLineEnd(right, position, lineStyle, false, false, 0, 0, 0, count, highlight, readOnly, false, 0, 0);
                return true;
            }
            return false;
        }

        private void DrawErrorAfterLineEnd(int w, Point position, int line, IEditLineStyle lineStyle)
        {
            Rectangle rect = new Rectangle(position.X, position.Y, base.painter.FontWidth, base.painter.FontHeight);
            base.drawInfo.Reset();
            base.drawInfo.Line = line;
            if (!this.OnCustomDraw(base.painter, rect, DrawStage.Before, DrawState.SyntaxError | DrawState.BeyondEol, base.drawInfo))
            {
                Color color = (lineStyle != null) ? lineStyle.GetForeColor(base.painter.TextColor) : Color.Empty;
                base.painter.DrawWave(rect, (color != Color.Empty) ? color : ((this.errorStyle != null) ? this.errorStyle.ForeColor : SyntaxConsts.DefaultSyntaxErrorsForeColor));
            }
            this.OnCustomDraw(base.painter, rect, DrawStage.After, DrawState.SyntaxError | DrawState.BeyondEol, base.drawInfo);
        }

        private void DrawHighlighter(int left, int top, int right, int bottom, int style, int pos, int line, string s, bool inSelection, bool afterText)
        {
            if (this.lineSeparator.LineColor != Color.Empty)
            {
                base.painter.ForeColor = this.lineSeparator.LineColor;
                Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
                DrawState lineHighlight = DrawState.LineHighlight;
                if (!afterText)
                {
                    lineHighlight |= DrawState.Text;
                }
                else
                {
                    lineHighlight |= DrawState.BeyondEol;
                }
                if (inSelection)
                {
                    lineHighlight |= DrawState.Selection;
                }
                base.drawInfo.Reset();
                base.drawInfo.Text = s;
                base.drawInfo.Line = line;
                base.drawInfo.Char = pos;
                base.drawInfo.Style = (short) style;
                if (!this.OnCustomDraw(base.painter, rect, DrawStage.Before, lineHighlight, base.drawInfo))
                {
                    base.painter.DrawLine(left, top, right, top);
                    base.painter.DrawLine(left, bottom, right, bottom);
                }
                this.OnCustomDraw(base.painter, rect, DrawStage.After, lineHighlight, base.drawInfo);
            }
        }

        public override void DrawLine(int index, Point position, Rectangle clipRect)
        {
            string text = string.Empty;
            short[] data = null;
            int realLine = this.displayLines.GetStringAndColorData(index, ref text, ref data);
            this.DrawLine(index, realLine, text, data, position, clipRect);
        }

        public virtual void DrawLine(int index, Point position, Rectangle clipRect, out int realLine)
        {
            string text = string.Empty;
            short[] data = null;
            realLine = this.displayLines.GetStringAndColorData(index, ref text, ref data);
            this.DrawLine(index, realLine, text, data, position, clipRect);
        }

        private void DrawLine(int index, int realLine, string line, short[] colorData, Point position, Rectangle clipRect)
        {
            int right = clipRect.Right;
            int left = 0;
            int num3 = 0;
            int x = position.X;
            bool selLine = (this.drawSelection & !this.selection.IsEmpty) && this.selection.GetSelectionForLine(index, out left, out num3);
            bool inSelection = false;
            bool columnIndent = false;
            bool readOnly = this.owner.Source.LineIsReadonly(realLine);
            IList<ILineStyle> internalLineStyles = this.internalLineStyles;
            this.owner.Source.LineStyles.GetLineStyles(realLine, internalLineStyles);
            IEditLineStyle style = this.GetLineStyleAt(index, 0, internalLineStyles);
            bool drawColumnIndent = ((style == null) && this.DrawColumnsIndent) && (this.ColumnsIndentForeColor != Color.Empty);
            int length = line.Length;
            bool highlight = this.lineSeparator.NeedHighlightDisplayLine(index);
            int offset = this.gutter.DisplayWidth - position.X;
            bool flag7 = false;
            if ((this.whiteSpace.Visible || (this.syntaxErrors.Count > 0)) || drawColumnIndent)
            {
                flag7 = this.owner.WordWrap && (this.displayLines.DisplayPointToPoint(0, index + 1).X > 0);
            }
            if (flag7)
            {
                drawColumnIndent = false;
            }
            int num7 = 0;
            if (drawColumnIndent)
            {
                num7 = line.TrimStart(new char[0]).Length;
                if (num7 == 0)
                {
                    for (int i = index - 1; i >= 0; i--)
                    {
                        short[] numArray = null;
                        string str = string.Empty;
                        this.GetString(i, ref str, ref numArray);
                        num7 = str.TrimStart(new char[0]).Length;
                        if (num7 != 0)
                        {
                            num7 = str.Length - num7;
                            break;
                        }
                    }
                }
                else
                {
                    num7 = length - num7;
                }
            }
            if (line != string.Empty)
            {
                int num9 = (ushort) colorData[0];
                int num10 = num9;
                int startChar = 0;
                inSelection = (selLine && (left == 0)) && (num3 > 0);
                bool flag8 = inSelection;
                bool flag9 = false;
                IEditLineStyle style2 = style;
                for (int j = 1; j < length; j++)
                {
                    num9 = (ushort) colorData[j];
                    flag8 = (selLine && (left <= j)) && (num3 > j);
                    style2 = this.GetLineStyleAt(index, j, internalLineStyles);
                    flag9 = ((drawColumnIndent && !inSelection) && (j < num7)) && (j == this.owner.Lines.GetTabStop(j - 1));
                    if ((((j - startChar) > EditConsts.MaxPaintChar) || (flag8 != inSelection)) || (((columnIndent != flag9) || !base.EqualStyles(num10, num9, true)) || !this.EqualLineStyles(style2, style, num9, num10)))
                    {
                        position.X += this.DrawTextFragment(position, line, num10, style, inSelection, index, startChar, j - 1, false, highlight, readOnly, columnIndent);
                        if (position.X >= right)
                        {
                            break;
                        }
                        num10 = num9;
                        startChar = j;
                        style = style2;
                        inSelection = flag8;
                        columnIndent = flag9;
                    }
                }
                if ((startChar < length) && (position.X < right))
                {
                    position.X += this.DrawTextFragment(position, line, num10, style, inSelection, index, startChar, length - 1, false, highlight, readOnly, columnIndent);
                }
            }
            if (position.X < right)
            {
                if (this.whiteSpace.Visible)
                {
                    string s = string.Empty;
                    if (flag7 && (this.whiteSpace.WordWrapSymbol != '\0'))
                    {
                        s = this.whiteSpace.WordWrapString;
                    }
                    else if (!flag7)
                    {
                        bool flag10 = index == (this.displayLines.DisplayCount - 1);
                        if (flag10 && (this.whiteSpace.EofSymbol != '\0'))
                        {
                            s = this.whiteSpace.EofString;
                        }
                        else if (!flag10 && (this.whiteSpace.EolSymbol != '\0'))
                        {
                            s = this.whiteSpace.EolString;
                        }
                    }
                    if (s != string.Empty)
                    {
                        inSelection = (selLine && (((SelectionOptions.SelectBeyondEol & this.owner.Selection.Options) != SelectionOptions.None) || (this.selection.SelectionType == SelectionType.Block))) && ((selLine && (left <= length)) && (num3 > length));
                        position.X += this.DrawTextFragment(position, s, 0x100, style, inSelection, index, 0, 0, true, highlight, readOnly, columnIndent);
                    }
                }
                this.DrawAfterLineEnd(right, position, style, selLine, ((selLine && (line == string.Empty)) && ((left == 0) && (num3 == 0x7fffffff))) && ((SelectionOptions.SelectBeyondEol & this.selection.Options) == SelectionOptions.None), offset, left, num3, index, highlight, readOnly, drawColumnIndent, x, (line.TrimStart(new char[0]) == string.Empty) ? num7 : 0);
                if ((selLine && ((this.selection.Options & SelectionOptions.DrawBorder) != SelectionOptions.None)) && (this.selection.BorderColor != Color.Empty))
                {
                    this.DrawSelectionBorder(left, num3, position.Y, index, line.Length);
                }
                if (((right > position.X) && !flag7) && (this.syntaxErrors[realLine] != null))
                {
                    this.DrawErrorAfterLineEnd(right, position, realLine, !inSelection ? style : null);
                }
            }
        }

        private void DrawLineStyle(int left, int top, int right, int bottom, int style, int pos, int line, string s, bool inSelection, bool afterText, IEditLineStyle lineStyle)
        {
            if (lineStyle.PenColor != Color.Empty)
            {
                base.painter.ForeColor = lineStyle.PenColor;
                Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
                DrawState state = DrawState.LineStyle;
                if (!afterText)
                {
                    state |= DrawState.Text;
                }
                else
                {
                    state |= DrawState.BeyondEol;
                }
                if (inSelection)
                {
                    state |= DrawState.Selection;
                }
                base.drawInfo.Reset();
                base.drawInfo.Text = s;
                base.drawInfo.Line = line;
                base.drawInfo.Char = pos;
                base.drawInfo.Style = (short) style;
                if (!this.OnCustomDraw(base.painter, rect, DrawStage.Before, state, base.drawInfo))
                {
                    base.painter.DrawLine(left, top, right, top);
                    base.painter.DrawLine(left, bottom, right, bottom);
                }
                this.OnCustomDraw(base.painter, rect, DrawStage.After, state, base.drawInfo);
            }
        }

        private void DrawSelectionBorder(Rectangle rect, Border3DSide sides, Color borderColor)
        {
            if (((borderColor != Color.Empty) && (sides != 0)) && (rect.Width >= 0))
            {
                Color foreColor = base.painter.ForeColor;
                base.painter.ForeColor = borderColor;
                if ((sides & Border3DSide.Left) != 0)
                {
                    base.painter.DrawLine(rect.X, rect.Y, rect.X, (rect.Y + rect.Height) - 1);
                }
                if ((sides & Border3DSide.Right) != 0)
                {
                    base.painter.DrawLine((rect.X + rect.Width) - 1, rect.Y, (rect.X + rect.Width) - 1, (rect.Y + rect.Height) - 1);
                }
                if ((sides & Border3DSide.Top) != 0)
                {
                    base.painter.DrawLine(rect.X, rect.Y, (rect.X + rect.Width) - 1, rect.Y);
                }
                if ((sides & Border3DSide.Bottom) != 0)
                {
                    base.painter.DrawLine(rect.X, (rect.Y + rect.Height) - 1, (rect.X + rect.Width) - 1, (rect.Y + rect.Height) - 1);
                }
                base.painter.ForeColor = foreColor;
            }
        }

        private void DrawSelectionBorder(int left, int right, int top, int index, int len)
        {
            int selectionBorder = this.GetSelectionBorder(left, index);
            int num2 = this.GetSelectionBorder(right, index);
            if (num2 > selectionBorder)
            {
                this.DrawSelectionBorder(new Rectangle(selectionBorder, top, num2 - selectionBorder, base.painter.FontHeight), Border3DSide.Right | Border3DSide.Left, this.selection.BorderColor);
                if (this.GetSelectionBorder(index - 1, out left, out right))
                {
                    if (left > selectionBorder)
                    {
                        left = Math.Min(left, num2);
                        this.DrawSelectionBorder(new Rectangle(selectionBorder, top, Math.Min(left, right) - selectionBorder, base.painter.FontHeight), Border3DSide.Top, this.selection.BorderColor);
                    }
                    if (right < num2)
                    {
                        this.DrawSelectionBorder(new Rectangle(right, top, num2 - right, base.painter.FontHeight), Border3DSide.Top, this.selection.BorderColor);
                    }
                }
                else
                {
                    this.DrawSelectionBorder(new Rectangle(selectionBorder, top, num2 - selectionBorder, base.painter.FontHeight), Border3DSide.Top, this.selection.BorderColor);
                }
                if (this.GetSelectionBorder(index + 1, out left, out right))
                {
                    if (left > selectionBorder)
                    {
                        this.DrawSelectionBorder(new Rectangle(selectionBorder, top, selectionBorder - left, base.painter.FontHeight), Border3DSide.Bottom, this.selection.BorderColor);
                    }
                    if (right < num2)
                    {
                        right = Math.Max(right, selectionBorder);
                        this.DrawSelectionBorder(new Rectangle(right, top, num2 - right, base.painter.FontHeight), Border3DSide.Bottom, this.selection.BorderColor);
                    }
                }
                else
                {
                    this.DrawSelectionBorder(new Rectangle(selectionBorder, top, num2 - selectionBorder, base.painter.FontHeight), Border3DSide.Bottom, this.selection.BorderColor);
                }
            }
        }

        private int DrawTextFragment(Point position, string s, int style, IEditLineStyle lineStyle, bool inSelection, int line, int startChar, int endChar, bool specialSymbol, bool highlight, bool readOnly, bool columnIndent)
        {
            TextStyle none = TextStyle.None;
            ILexStyle lexStyle = this.GetLexStyle(style, ref none);
            if (this.DisableSyntaxPaint)
            {
                lexStyle = null;
            }
            string text = ((startChar == 0) && (endChar == -1)) ? s : s.Substring(startChar, (endChar - startChar) + 1);
            if (lexStyle != null)
            {
                base.painter.FontStyle = this.GetFontStyle(lexStyle.FontStyle, none);
                if (inSelection)
                {
                    base.painter.TextColor = this.GetSelectionForeColor(this.GetFontColor(lexStyle.ForeColor, none));
                    base.painter.BackColor = this.selBackColor;
                }
                else
                {
                    if (highlight && (this.lineSeparator.HighlightForeColor != Color.Empty))
                    {
                        base.painter.TextColor = this.lineSeparator.HighlightForeColor;
                    }
                    else
                    {
                        base.painter.TextColor = this.GetFontColor(lexStyle.ForeColor, none);
                    }
                    if (highlight && (this.lineSeparator.HighlightBackColor != Color.Empty))
                    {
                        base.painter.BackColor = this.lineSeparator.HighlightBackColor;
                    }
                    else
                    {
                        base.painter.BackColor = this.GetBackColor((lexStyle.BackColor != Color.Empty) ? lexStyle.BackColor : this.GetBackColor(readOnly), none);
                    }
                }
            }
            else
            {
                base.painter.FontStyle = this.GetFontStyle(this.owner.Font.Style, none);
                if (inSelection)
                {
                    base.painter.TextColor = this.selForeColor;
                    base.painter.BackColor = this.selBackColor;
                }
                else
                {
                    if (highlight && (this.lineSeparator.HighlightForeColor != Color.Empty))
                    {
                        base.painter.TextColor = this.lineSeparator.HighlightForeColor;
                    }
                    else
                    {
                        base.painter.TextColor = this.GetFontColor(this.owner.ForeColor, none);
                    }
                    if (highlight && (this.lineSeparator.HighlightBackColor != Color.Empty))
                    {
                        base.painter.BackColor = this.lineSeparator.HighlightBackColor;
                    }
                    else
                    {
                        base.painter.BackColor = this.GetBackColor(this.GetBackColor(readOnly), none);
                    }
                }
            }
            Color empty = Color.Empty;
            if (((!inSelection && (lineStyle != null)) && ((lexStyle == null) || (lexStyle.BackColor == Color.Empty))) && (((none & TextStyle.OutlineSection) == TextStyle.None) && ((none & TextStyle.CodeSnippet) == TextStyle.None)))
            {
                base.painter.BackColor = lineStyle.GetBackColor(base.painter.BackColor);
                empty = lineStyle.GetForeColor(base.painter.TextColor);
                base.painter.TextColor = empty;
            }
            int width = base.painter.StringWidth(text);
            Rectangle rect = new Rectangle(position.X, position.Y, width, base.painter.FontHeight);
            int space = -1;
            if (!specialSymbol && this.whiteSpace.Visible)
            {
                if ((this.whiteSpace.SpaceSymbol != '\0') && ((none & TextStyle.WhiteSpace) != TextStyle.None))
                {
                    text = new string(this.whiteSpace.SpaceSymbol, text.Length);
                    space = base.painter.CharWidth(' ', 1);
                }
                else if ((this.whiteSpace.TabSymbol != '\0') && ((none & TextStyle.Tabulation) != TextStyle.None))
                {
                    text = new string(this.whiteSpace.TabSymbol, text.Length);
                    space = base.painter.CharWidth(' ', 1);
                }
            }
            DrawState state = DrawState.Text;
            if (inSelection)
            {
                state |= DrawState.Selection;
            }
            if (specialSymbol)
            {
                state |= DrawState.WhiteSpace;
            }
            if ((none & TextStyle.CodeSnippet) != TextStyle.None)
            {
                state |= DrawState.CodeSnippet;
            }
            bool flag = ((this.owner.Transparent || this.inPrinting) && (!inSelection && !highlight)) && (lineStyle == null);
            if (flag)
            {
                base.painter.Opaque = false;
            }
            base.drawInfo.Reset();
            base.drawInfo.Text = text;
            base.drawInfo.Style = (short) style;
            base.drawInfo.Line = line;
            base.drawInfo.Char = startChar;
            if (!this.OnCustomDraw(base.painter, rect, DrawStage.Before, state, base.drawInfo))
            {
                base.painter.TextOut(text, -1, rect, rect.Left, rect.Top, !this.owner.Transparent && !this.inPrinting, !this.owner.Transparent && !this.inPrinting, space);
            }
            this.OnCustomDraw(base.painter, rect, DrawStage.After, state, base.drawInfo);
            if (flag)
            {
                base.painter.Opaque = true;
            }
            if ((lineStyle != null) && (lineStyle.PenColor != Color.Empty))
            {
                this.DrawLineStyle(rect.Left, rect.Top - 1, rect.Right, rect.Bottom - 1, style, startChar, line, text, inSelection, false, lineStyle);
            }
            else if (highlight)
            {
                this.DrawHighlighter(rect.Left, rect.Top - 1, rect.Right, rect.Bottom - 1, style, startChar, line, text, inSelection, false);
            }
            if ((none & TextStyle.OutlineSection) != TextStyle.None)
            {
                Color foreColor = base.painter.ForeColor;
                Color backColor = base.painter.BackColor;
                base.painter.BackColor = this.outlining.OutlineColor;
                base.painter.ForeColor = this.outlining.OutlineColor;
                if (!this.OnCustomDraw(base.painter, rect, DrawStage.Before, state | DrawState.OutlineButton, base.drawInfo))
                {
                    if (this.outlining.UseRoundRect)
                    {
                        base.painter.DrawRoundRectangle(rect.Left, rect.Top, rect.Right - 1, rect.Bottom - 1, 2, 2);
                    }
                    else
                    {
                        base.painter.DrawRectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
                    }
                }
                this.OnCustomDraw(base.painter, rect, DrawStage.After, state | DrawState.OutlineButton, base.drawInfo);
                base.painter.ForeColor = foreColor;
                base.painter.BackColor = backColor;
            }
            if (columnIndent)
            {
                this.DrawColumnIndent(rect.Left, rect.Top, rect.Bottom);
            }
            if (((this.braces.UseRoundRect && ((none & TextStyle.Brace) != TextStyle.None)) && (((none & TextStyle.Tabulation) == TextStyle.None) && ((none & TextStyle.WhiteSpace) == TextStyle.None))) && (text.Trim() != string.Empty))
            {
                Color color4 = base.painter.BackColor;
                base.painter.BackColor = this.braces.BackColor;
                if (!this.OnCustomDraw(base.painter, rect, DrawStage.Before, state | DrawState.Brace, base.drawInfo))
                {
                    base.painter.DrawRectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
                }
                this.OnCustomDraw(base.painter, rect, DrawStage.After, state | DrawState.Brace, base.drawInfo);
                base.painter.BackColor = color4;
            }
            if (this.spelling.CheckSpelling && ((none & TextStyle.MisSpelledWord) != TextStyle.None))
            {
                if (!this.OnCustomDraw(base.painter, rect, DrawStage.Before, state | DrawState.Spelling, base.drawInfo))
                {
                    base.painter.DrawWave(rect, (empty != Color.Empty) ? empty : this.spelling.SpellColor);
                }
                this.OnCustomDraw(base.painter, rect, DrawStage.After, state | DrawState.Spelling, base.drawInfo);
            }
            if ((none & TextStyle.WaveLine) != TextStyle.None)
            {
                if (!this.OnCustomDraw(base.painter, rect, DrawStage.Before, state | DrawState.SyntaxError, base.drawInfo))
                {
                    base.painter.DrawWave(rect, (empty != Color.Empty) ? empty : ((this.errorStyle != null) ? this.errorStyle.ForeColor : SyntaxConsts.DefaultSyntaxErrorsForeColor));
                }
                this.OnCustomDraw(base.painter, rect, DrawStage.After, state | DrawState.SyntaxError, base.drawInfo);
            }
            return width;
        }

        protected void EnsureLastLineParsed()
        {
            int y;
            if (this.owner.Pages.PageType == PageType.PageLayout)
            {
                y = this.owner.Pages.GetPageAtPoint(0, this.owner.ClientHeight).EndLine;
            }
            else if (this.owner.Scrolling.ScrollByPixels)
            {
                y = this.owner.GetLinesInHeight(this.owner.Scrolling.WindowOriginY) + 1;
            }
            else
            {
                y = (this.owner.Scrolling.WindowOriginY + this.owner.LinesInHeight) + 1;
            }
            y = this.owner.DisplayLines.DisplayPointToPoint(0, y).Y;
            this.owner.Source.ParseToString(y + EditConsts.DefaultParserDelta);
        }

        private void FillRectangleAfterLineEnd(int x, int y, int w, int h, int line, bool inSelection, bool highlight, IEditLineStyle lineStyle)
        {
            Rectangle rect = new Rectangle(x, y, w, h);
            DrawState beyondEol = DrawState.BeyondEol;
            if (inSelection)
            {
                beyondEol |= DrawState.Selection;
            }
            base.drawInfo.Reset();
            base.drawInfo.Line = line;
            if (!this.OnCustomDraw(base.painter, rect, DrawStage.Before, beyondEol, base.drawInfo))
            {
                base.painter.FillRectangle(x, y, w, h);
            }
            this.OnCustomDraw(base.painter, rect, DrawStage.After, beyondEol, base.drawInfo);
            if (((lineStyle != null) && (lineStyle.PenColor != Color.Empty)) && ((lineStyle.Options & LineStyleOptions.BeyondEol) != LineStyleOptions.None))
            {
                this.DrawLineStyle(x, y - 1, x + w, (y + h) - 1, -1, 0, line, string.Empty, inSelection, true, lineStyle);
            }
            if (highlight)
            {
                this.DrawHighlighter(x, y - 1, x + w, (y + h) - 1, -1, 0, line, string.Empty, inSelection, true);
            }
        }

        public virtual Color GetBackColor(bool readOnly)
        {
            if (!this.owner.Enabled)
            {
                Color disabledBackColor = this.DisabledBackColor;
                if (disabledBackColor != Color.Empty)
                {
                    return disabledBackColor;
                }
            }
            if (this.owner.Readonly || readOnly)
            {
                Color readonlyBackColor = this.ReadonlyBackColor;
                if (readonlyBackColor != Color.Empty)
                {
                    return readonlyBackColor;
                }
            }
            return this.owner.BackColor;
        }

        public override Color GetBackColor(Color color, TextStyle state)
        {
            if ((state & TextStyle.CodeSnippet) != TextStyle.None)
            {
                Color color2 = (this.snippetStyle != null) ? this.snippetStyle.BackColor : SyntaxConsts.DefaultCodeSnippetBackColor;
                if (color2 != Color.Empty)
                {
                    return color2;
                }
            }
            if (((state & TextStyle.Brace) != TextStyle.None) && !this.braces.UseRoundRect)
            {
                Color backColor = this.braces.BackColor;
                if (backColor != Color.Empty)
                {
                    return backColor;
                }
            }
            if (!(color != Color.Empty))
            {
                return base.painter.BackColor;
            }
            return color;
        }

        public override Color GetFontColor(Color color, TextStyle textStyle)
        {
            Color urlColor;
            if (this.DisableColorPaint)
            {
                return this.owner.ForeColor;
            }
            if ((textStyle & TextStyle.HyperText) != TextStyle.None)
            {
                urlColor = this.hyperText.UrlColor;
                if (urlColor != Color.Empty)
                {
                    return urlColor;
                }
            }
            if (((textStyle & TextStyle.WhiteSpace) != TextStyle.None) || ((textStyle & TextStyle.Tabulation) != TextStyle.None))
            {
                urlColor = this.whiteSpace.SymbolColor;
                if (urlColor != Color.Empty)
                {
                    return urlColor;
                }
            }
            if ((textStyle & TextStyle.OutlineSection) != TextStyle.None)
            {
                urlColor = this.outlining.OutlineColor;
                if (urlColor != Color.Empty)
                {
                    return urlColor;
                }
            }
            if (((textStyle & TextStyle.Brace) != TextStyle.None) && !this.braces.UseRoundRect)
            {
                urlColor = this.braces.ForeColor;
                if (urlColor != Color.Empty)
                {
                    return urlColor;
                }
            }
            return color;
        }

        public override FontStyle GetFontStyle(FontStyle fontStyle, TextStyle textStyle)
        {
            if ((textStyle & TextStyle.HyperText) != TextStyle.None)
            {
                return (fontStyle | this.hyperText.UrlStyle);
            }
            if (((textStyle & TextStyle.Brace) != TextStyle.None) && !this.braces.UseRoundRect)
            {
                return (fontStyle | this.braces.FontStyle);
            }
            return fontStyle;
        }

        public virtual Color GetForeColor(bool readOnly)
        {
            if (!this.owner.Enabled)
            {
                Color disabledForeColor = this.DisabledForeColor;
                if (disabledForeColor != Color.Empty)
                {
                    return disabledForeColor;
                }
            }
            if (this.owner.Readonly || readOnly)
            {
                Color readonlyForeColor = this.ReadonlyForeColor;
                if (readonlyForeColor != Color.Empty)
                {
                    return readonlyForeColor;
                }
            }
            return this.owner.ForeColor;
        }

        protected IEditLineStyle GetLineStyleAt(int line, int pos, IList<ILineStyle> styles)
        {
            if (styles.Count > 0)
            {
                Point point = this.displayLines.DisplayPointToPoint(pos, line);
                foreach (ILineStyle style in styles)
                {
                    Point point2 = (style.Range != null) ? style.Range.StartPoint : new Point(style.Pos, style.Line);
                    Point point3 = (style.Range != null) ? style.Range.EndPoint : new Point(0x7fffffff, style.Line);
                    if ((((point.Y > point2.Y) || ((point.Y == point2.Y) && (point.X >= point2.X))) && ((point.Y < point3.Y) || ((point.Y == point3.Y) && (point.X < point3.X)))) && ((style.Index >= 0) && (style.Index < this.lineStyles.Count)))
                    {
                        return this.lineStyles[style.Index];
                    }
                }
            }
            return null;
        }

        public virtual Region GetRectRegion(Rectangle rect)
        {
            return this.GetRectRegion(SelectionType.Stream, rect, false, false);
        }

        public virtual Region GetRectRegion(SelectionType selectionType, Rectangle rect, bool atTopLeftEnd, bool atBottomRightEnd)
        {
            Region region = null;
            if (selectionType != SelectionType.None)
            {
                int fontHeight = base.painter.FontHeight;
                Point position = this.displayLines.PointToDisplayPoint(rect.Left, rect.Top, atTopLeftEnd);
                Point point2 = this.displayLines.PointToDisplayPoint(rect.Right, rect.Bottom, atBottomRightEnd);
                if (((selectionType == SelectionType.Block) && (position.X == point2.X)) && (position.Y != point2.Y))
                {
                    if (position.X == 0)
                    {
                        point2.Y--;
                    }
                    position.X = 0;
                    point2.X = 0x7fffffff;
                }
                position = this.owner.DisplayToScreen(position.X, position.Y);
                point2 = this.owner.DisplayToScreen(point2.X, point2.Y);
                if (point2.X == 0x7fffffff)
                {
                    point2.X = this.owner.ClientRect.Right;
                }
                if ((position.Y == point2.Y) || (selectionType == SelectionType.Block))
                {
                    int num2 = Math.Min(position.X, point2.X);
                    int num3 = Math.Max(position.X, point2.X);
                    return new Region(new Rectangle(num2, position.Y, (num3 - num2) + 1, (point2.Y - position.Y) + fontHeight));
                }
                int num4 = point2.Y - position.Y;
                if (this.owner.Pages.PageType == PageType.PageLayout)
                {
                    IEditPage pageAtPoint = this.owner.Pages.GetPageAtPoint(position);
                    IEditPage page2 = this.owner.Pages.GetPageAtPoint(point2);
                    Rectangle clientRect = pageAtPoint.ClientRect;
                    Rectangle rectangle2 = page2.ClientRect;
                    region = new Region(new Rectangle(position.X, position.Y, (clientRect.Right - position.X) + 1, fontHeight));
                    int num5 = Math.Min(clientRect.Left, rectangle2.Left);
                    region.Union(new Rectangle(num5, position.Y + fontHeight, Math.Max(clientRect.Right, rectangle2.Right) - num5, num4 - fontHeight));
                    region.Union(new Rectangle(rectangle2.Left, point2.Y, (point2.X - rectangle2.Left) + 1, fontHeight));
                    return region;
                }
                int x = this.gutter.DisplayWidth + this.owner.ClientRect.Left;
                int right = this.owner.ClientRect.Right;
                region = new Region(new Rectangle(position.X, position.Y, (right - position.X) + 1, fontHeight));
                region.Union(new Rectangle(x, position.Y + fontHeight, (right - x) + 1, num4 - fontHeight));
                region.Union(new Rectangle(x, point2.Y, (point2.X - x) + 1, fontHeight));
            }
            return region;
        }

        private int GetSelectionBorder(int pos, int index)
        {
            if (pos == 0x7fffffff)
            {
                if (((SelectionOptions.SelectBeyondEol & this.owner.Selection.Options) != SelectionOptions.None) || (this.selection.SelectionType == SelectionType.Block))
                {
                    pos = this.owner.ClientRect.Width;
                    return pos;
                }
                pos = this.owner.DisplayToScreen(this.displayLines[index].Length, index).X;
                return pos;
            }
            pos = this.owner.DisplayToScreen(pos, index).X;
            return pos;
        }

        private bool GetSelectionBorder(int index, out int left, out int right)
        {
            left = 0;
            right = 0;
            if (((index >= 0) && (index < this.displayLines.DisplayCount)) && (this.selection.GetSelectionForLine(index, out left, out right) && (right > left)))
            {
                left = this.GetSelectionBorder(left, index);
                right = this.GetSelectionBorder(right, index);
                return (right > left);
            }
            return false;
        }

        protected Color GetSelectionForeColor(Color color)
        {
            if ((SelectionOptions.UseColors & this.selection.Options) != SelectionOptions.None)
            {
                return color;
            }
            return this.selForeColor;
        }

        protected override void GetString(int index, ref string text, ref short[] colorData)
        {
            this.displayLines.GetStringAndColorData(index, ref text, ref colorData);
        }

        private void InitSyntaxErrors()
        {
            if (this.syntaxErrors == null)
            {
                this.syntaxErrors = new Hashtable();
            }
            this.syntaxErrors.Clear();
            ITextSource source = this.owner.Source;
            foreach (ISyntaxError error in source.SyntaxErrors)
            {
                if (error.Position.X >= source.Lines.GetLength(error.Position.Y))
                {
                    this.syntaxErrors[error.Position.Y] = true;
                }
            }
        }

        protected virtual void OnColumnsIndentForeColorChanged()
        {
            if (this.drawColumnsIndent)
            {
                this.owner.Invalidate();
            }
        }

        protected override void OnDisableColorPaintChanged()
        {
            base.OnDisableColorPaintChanged();
            this.owner.Invalidate();
        }

        protected virtual void OnDisabledBackColorChanged()
        {
            if (!this.owner.Enabled)
            {
                this.owner.Invalidate();
            }
        }

        protected virtual void OnDisabledForeColorChanged()
        {
            if (!this.owner.Enabled)
            {
                this.owner.Invalidate();
            }
        }

        protected override void OnDisableSyntaxPaintChanged()
        {
            base.OnDisableSyntaxPaintChanged();
            this.owner.Invalidate();
        }

        protected virtual void OnDrawColumnsIndentChanged()
        {
            this.owner.Invalidate();
        }

        protected virtual void OnReadonlyBackColorChanged()
        {
            if (this.owner.Readonly)
            {
                this.owner.Invalidate();
            }
        }

        protected virtual void OnReadonlyForeColorChanged()
        {
            if (this.owner.Readonly)
            {
                this.owner.Invalidate();
            }
        }

        protected virtual void OnSyntaxErrorsHintsChanged()
        {
        }

        public virtual void PaintLineBookMark(IPainter painter, IBookMark bookMark, Rectangle rect)
        {
            if (!this.outlining.AllowOutlining || this.outlining.IsVisible(new Point(bookMark.Pos, bookMark.Line)))
            {
                int num = 3;
                Point point = this.displayLines.PointToDisplayPoint(bookMark.Position.X, bookMark.Position.Y, false);
                Point point2 = this.owner.DisplayToScreen(point.X, point.Y);
                point2.Y += painter.FontHeight - 1;
                if ((((point2.X + num) >= this.gutter.DisplayWidth) && ((point2.X - num) < rect.Width)) && ((point2.Y > 0) && ((point2.Y - num) < rect.Height)))
                {
                    Point[] points = new Point[] { new Point(point2.X - num, point2.Y), new Point(point2.X + num, point2.Y), new Point(point2.X, point2.Y - num) };
                    Rectangle rectangle = new Rectangle(point2.X - num, point2.Y - num, num * 2, num * 2);
                    base.drawInfo.Reset();
                    base.drawInfo.Line = point.Y;
                    base.drawInfo.Char = point.X;
                    if (!this.OnCustomDraw(painter, rectangle, DrawStage.Before, DrawState.LineBookMark, base.drawInfo))
                    {
                        painter.DrawPolygon(points, this.gutter.LineBookmarksColor);
                    }
                    this.OnCustomDraw(painter, rectangle, DrawStage.After, DrawState.LineBookMark, base.drawInfo);
                }
            }
        }

        public override void PaintLineBookMarks(IPainter painter, Rectangle rect)
        {
            IBookMarks bookMarks = this.owner.Source.BookMarks;
            for (int i = 0; i < bookMarks.Count; i++)
            {
                this.PaintLineBookMark(painter, bookMarks[i], rect);
            }
        }

        public override void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint)
        {
            this.InitSyntaxErrors();
            this.internalLineStyles.Clear();
            if (this.owner.Focused || this.owner.IsCodeCompletionWindowFocused)
            {
                this.selBackColor = this.selection.BackColor;
                this.selForeColor = this.selection.ForeColor;
                this.drawSelection = true;
            }
            else
            {
                this.drawSelection = (SelectionOptions.HideSelection & this.selection.Options) == SelectionOptions.None;
                this.selBackColor = this.selection.InActiveBackColor;
                this.selForeColor = this.selection.InActiveForeColor;
            }
            this.errorStyle = (this.Lexer != null) ? this.Lexer.Scheme.Styles.FindLexStyle(StringConsts.SyntaxErrorsInternalName) : null;
            this.snippetStyle = (this.Lexer != null) ? this.Lexer.Scheme.Styles.FindLexStyle(StringConsts.CodeSnippetsInternalName) : null;
            int fontHeight = painter.FontHeight;
            int realLine = 0;
            for (int i = startLine; i < endLine; i++)
            {
                this.DrawLine(i, position, rect, out realLine);
                position.Y += fontHeight;
                if ((position.Y > rect.Bottom) || ((position.Y == rect.Bottom) && specialPaint))
                {
                    break;
                }
                if ((((this.owner.LineSeparator.Options & SeparatorOptions.SeparateContent) != SeparatorOptions.None) && (this.Lexer != null)) && ((this.Lexer is ISyntaxParser) && ((ISyntaxParser) this.Lexer).IsContentDivider(realLine)))
                {
                    painter.ForeColor = this.owner.LineSeparator.ContentDividerColor;
                    painter.DrawLine(rect.Left, position.Y - 1, rect.Right - 1, position.Y - 1);
                }
                else if (((this.lineSeparator.Options & SeparatorOptions.SeparateLines) != SeparatorOptions.None) && (((this.lineSeparator.Options & SeparatorOptions.SeparateWrapLines) != SeparatorOptions.None) || (this.displayLines.DisplayPointToPoint(0, i + 1).X == 0)))
                {
                    painter.ForeColor = this.lineSeparator.LineColor;
                    Rectangle rectangle = new Rectangle(rect.Left, position.Y, rect.Width, painter.FontHeight);
                    base.drawInfo.Reset();
                    base.drawInfo.Line = i;
                    if (!this.OnCustomDraw(painter, rectangle, DrawStage.Before, DrawState.LineSeparator, base.drawInfo))
                    {
                        painter.DrawLine(rect.Left, position.Y - 1, rect.Right - 1, position.Y - 1);
                    }
                    this.OnCustomDraw(painter, rectangle, DrawStage.After, DrawState.LineSeparator, base.drawInfo);
                }
            }
            this.DrawCodeSnippets(painter);
            if (((position.Y < rect.Bottom) && (endLine == startLine)) && this.DrawEndLine(rect, position, endLine, false, null))
            {
                position.Y += fontHeight;
            }
            if (position.Y < rect.Bottom)
            {
                painter.BackColor = this.GetBackColor(false);
                Rectangle rectangle2 = new Rectangle(0, position.Y, rect.Right, rect.Bottom - position.Y);
                base.drawInfo.Reset();
                if (!this.owner.Transparent && !this.inPrinting)
                {
                    if (!this.OnCustomDraw(painter, rectangle2, DrawStage.Before, DrawState.BeyondEof, base.drawInfo))
                    {
                        painter.FillRectangle(rectangle2);
                    }
                    this.OnCustomDraw(painter, rectangle2, DrawStage.After, DrawState.BeyondEof, base.drawInfo);
                }
                if (((this.lineSeparator.Options & SeparatorOptions.SeparateLines) != SeparatorOptions.None) && ((this.lineSeparator.Options & SeparatorOptions.SeparateBeyondEof) != SeparatorOptions.None))
                {
                    painter.ForeColor = this.lineSeparator.LineColor;
                    rectangle2 = new Rectangle(rect.Left, position.Y, rect.Width, painter.FontHeight);
                    base.drawInfo.Reset();
                    base.drawInfo.Line = endLine;
                    while (position.Y < rect.Bottom)
                    {
                        base.drawInfo.Line++;
                        if (!this.OnCustomDraw(painter, rectangle2, DrawStage.Before, DrawState.LineSeparator, base.drawInfo))
                        {
                            painter.DrawLine(rect.Left, position.Y - 1, rect.Right - 1, position.Y - 1);
                        }
                        this.OnCustomDraw(painter, rectangle2, DrawStage.After, DrawState.LineSeparator, base.drawInfo);
                        position.Y += fontHeight;
                    }
                }
            }
        }

        public virtual void PaintWindow(IPainter painter, int startLine, Rectangle rect, Point location, float scaleX, float scaleY, bool specialPaint, bool inPrinting)
        {
            if (specialPaint)
            {
                painter.Transform(location.X, location.Y, scaleX, scaleY);
            }
            this.inPrinting = inPrinting;
            try
            {
                DrawInfo info = new DrawInfo();
                info.Reset();
                if (this.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Control, info))
                {
                    return;
                }
                Rectangle clientArea = this.owner.ClientArea;
                Rectangle b = new Rectangle(clientArea.Left, rect.Top, this.gutter.DisplayWidth, rect.Bottom);
                Rectangle rectangle3 = new Rectangle(this.owner.DisplayToScreen(this.owner.EditMargin.Position, startLine, true).X, rect.Top, 1, rect.Height);
                Point position = new Point(b.Width, 0);
                if (this.owner.Pages.PageType == PageType.PageLayout)
                {
                    rectangle3.Offset(-location.X, 0);
                }
                else if (this.owner.Scrolling.ScrollByPixels)
                {
                    position.X -= this.owner.Scrolling.WindowOriginX;
                }
                else
                {
                    position.X -= painter.FontWidth * this.owner.Scrolling.WindowOriginX;
                }
                int num = 0;
                if ((painter.FontHeight != 0) && !this.owner.Scrolling.ScrollByPixels)
                {
                    num = (rect.Top - clientArea.Top) / painter.FontHeight;
                    position.Y += num * painter.FontHeight;
                }
                num += startLine;
                int fontHeight = painter.FontHeight;
                bool flag = (b.Width > 0) && !Rectangle.Intersect(rect, b).IsEmpty;
                bool flag2 = (this.owner.EditMargin.Visible && !this.owner.EditMargin.IsDragging) && !Rectangle.Intersect(rect, rectangle3).IsEmpty;
                IntPtr rgn = painter.SaveClip(this.owner.ClientRectangle);
                painter.FontStyle = this.owner.Font.Style;
                if (flag)
                {
                    b.Offset(-clientArea.Left, -clientArea.Top);
                    this.gutter.Paint(painter, b, startLine);
                    painter.ExcludeClipRect(b.Left, b.Top, b.Width, b.Height);
                }
                if (flag2)
                {
                    rectangle3.Offset(-clientArea.Left, -clientArea.Top);
                    this.owner.EditMargin.Paint(painter, rectangle3);
                    painter.ExcludeClipRect(rectangle3.Left, rectangle3.Top, rectangle3.Width, rectangle3.Height);
                }
                try
                {
                    if (specialPaint)
                    {
                        painter.IntersectClipRect(rect.Left - clientArea.Left, rect.Top - clientArea.Top, rect.Width, rect.Height);
                    }
                    painter.TextColor = this.owner.ForeColor;
                    painter.BackColor = this.GetBackColor(false);
                    if (this.owner.Source.NeedParse())
                    {
                        this.EnsureLastLineParsed();
                    }
                    this.PaintSyntax(painter, num, this.displayLines.DisplayCount, position, rect, specialPaint);
                    if (this.owner.EditMargin.ColumnsVisible)
                    {
                        foreach (int num2 in this.owner.EditMargin.ColumnPositions)
                        {
                            rectangle3 = new Rectangle(this.owner.DisplayToScreen(num2, startLine, true).X, rect.Top, 1, rect.Height);
                            if (rectangle3.Left > rect.Right)
                            {
                                goto Label_0416;
                            }
                            if (this.owner.Pages.PageType == PageType.PageLayout)
                            {
                                rectangle3.Offset(-location.X, 0);
                            }
                            rectangle3.Offset(-clientArea.Left, -clientArea.Top);
                            this.owner.EditMargin.PaintColumn(painter, rectangle3);
                        }
                    }
                }
                finally
                {
                    painter.RestoreClip(rgn);
                }
            Label_0416:
                info.Reset();
                this.OnCustomDraw(painter, rect, DrawStage.After, DrawState.Control, info);
            }
            finally
            {
                this.inPrinting = false;
                if (specialPaint)
                {
                    painter.EndTransform();
                }
            }
        }

        public virtual void ResetColumnsIndentForeColor()
        {
            this.ColumnsIndentForeColor = EditConsts.DefaultColumnsIndentForeColor;
        }

        public virtual void ResetDisabledBackColor()
        {
            this.DisabledBackColor = Color.Empty;
        }

        public virtual void ResetDisabledForeColor()
        {
            this.DisabledForeColor = Color.Empty;
        }

        public virtual void ResetDrawColumnsIndent()
        {
            this.DrawColumnsIndent = false;
        }

        public virtual void ResetReadonlyBackColor()
        {
            this.ReadonlyBackColor = Color.Empty;
        }

        public virtual void ResetReadonlyForeColor()
        {
            this.ReadonlyForeColor = Color.Empty;
        }

        public virtual void ResetSyntaxErrorsHints()
        {
            this.SyntaxErrorsHints = true;
        }

        public bool ShouldSerializeColumnsIndentForeColor()
        {
            return (this.columnsIndentForeColor != EditConsts.DefaultColumnsIndentForeColor);
        }

        public bool ShouldSerializeDisabledBackColor()
        {
            return (this.disabledBackColor != Color.Empty);
        }

        public bool ShouldSerializeDisabledForeColor()
        {
            return (this.disabledForeColor != Color.Empty);
        }

        public bool ShouldSerializeReadonlyBackColor()
        {
            return (this.readonlyBackColor != Color.Empty);
        }

        public bool ShouldSerializeReadonlyForeColor()
        {
            return (this.readonlyForeColor != Color.Empty);
        }

        [Description("Specifies columns indentation color.")]
        public virtual Color ColumnsIndentForeColor
        {
            get
            {
                return this.columnsIndentForeColor;
            }
            set
            {
                if (this.columnsIndentForeColor != value)
                {
                    this.columnsIndentForeColor = value;
                    this.OnColumnsIndentForeColorChanged();
                }
            }
        }

        [Description("Gets or sets background color used in the disabled state.")]
        public virtual Color DisabledBackColor
        {
            get
            {
                return this.disabledBackColor;
            }
            set
            {
                if (this.disabledBackColor != value)
                {
                    this.disabledBackColor = value;
                    this.OnDisabledBackColorChanged();
                }
            }
        }

        [Description("Gets or sets foreground color used in the disabled state.")]
        public virtual Color DisabledForeColor
        {
            get
            {
                return this.disabledForeColor;
            }
            set
            {
                if (this.disabledForeColor != value)
                {
                    this.disabledForeColor = value;
                    this.OnDisabledForeColorChanged();
                }
            }
        }

        [Description("Indicates whether edit control should draw columns indentation marks."), DefaultValue(false)]
        public virtual bool DrawColumnsIndent
        {
            get
            {
                return this.drawColumnsIndent;
            }
            set
            {
                if (this.drawColumnsIndent != value)
                {
                    this.drawColumnsIndent = value;
                    this.OnDrawColumnsIndentChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Gets or sets object that can make lexical analysis for the control's content.")]
        public override ILexer Lexer
        {
            get
            {
                return this.owner.Lexer;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Represents \"SyntaxEdit\" object that owns this \"EditSyntaxPaint\"."), Browsable(false)]
        public virtual ISyntaxEdit Owner
        {
            get
            {
                return this.owner;
            }
        }

        [Description("Gets or sets background color used in the readonly state.")]
        public virtual Color ReadonlyBackColor
        {
            get
            {
                return this.readonlyBackColor;
            }
            set
            {
                if (this.readonlyBackColor != value)
                {
                    this.readonlyBackColor = value;
                    this.OnReadonlyBackColorChanged();
                }
            }
        }

        [Description("Gets or sets foreground color used in the readonly state.")]
        public virtual Color ReadonlyForeColor
        {
            get
            {
                return this.readonlyForeColor;
            }
            set
            {
                if (this.readonlyForeColor != value)
                {
                    this.readonlyForeColor = value;
                    this.OnReadonlyForeColorChanged();
                }
            }
        }

        [DefaultValue(true), Description("Gets or sets a boolean value that indicates whether Edit control should display hint over each syntax error in it's content.")]
        public virtual bool SyntaxErrorsHints
        {
            get
            {
                return this.syntaxErrorsHints;
            }
            set
            {
                if (this.syntaxErrorsHints != value)
                {
                    this.syntaxErrorsHints = value;
                    this.OnSyntaxErrorsHintsChanged();
                }
            }
        }
    }
}

