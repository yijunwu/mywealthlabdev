namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class HintSyntaxPaint : HtmlSyntaxPaint
    {
        private const int arrowBottom = 6;
        private const int arrowHeight = 11;
        private const int arrowTop = 3;
        private const int arrowWidth = 9;
        private const int drawBottom = 4;
        private const int drawLeft = 2;
        private const int drawTop = 1;
        private string nameStr;
        private int selectedIndex;
        private IList<IStringItem> strings;

        public HintSyntaxPaint(IPainter painter, Control control) : base(painter, control)
        {
            this.nameStr = string.Empty;
            this.strings = new List<IStringItem>();
        }

        protected override void AddLine(string s, short[] data)
        {
            bool flag = this.strings.Count == 0;
            if (s == string.Empty)
            {
                this.strings.Add(new HintItem(string.Empty, 0));
            }
            else
            {
                int pos = 0;
                int length = s.Length;
                int maxHintWindowWidth = EditConsts.MaxHintWindowWidth;
                if (flag)
                {
                    maxHintWindowWidth -= this.MeasureArrows();
                }
                while (pos < length)
                {
                    int num3;
                    int width = this.MeasureLine(s, data, pos, length, maxHintWindowWidth, out num3, true, false, true);
                    if ((pos + num3) < length)
                    {
                        for (int i = num3; i > 0; i--)
                        {
                            if (base.IsDelimiter(s, (pos + i) - 1))
                            {
                                num3 = i;
                                break;
                            }
                        }
                    }
                    if (num3 <= 0)
                    {
                        num3 = 1;
                    }
                    num3 = Math.Min(num3, length - pos);
                    IStringItem item = new HintItem(s.Substring(pos, num3), width);
                    if (data != null)
                    {
                        Array.Copy(data, pos, item.TextData, 0, num3);
                    }
                    this.strings.Add(item);
                    pos += num3;
                    maxHintWindowWidth = EditConsts.MaxHintWindowWidth;
                }
            }
        }

        private void DrawArrows()
        {
            string text = string.Format(StringConsts.NofMstr, this.selectedIndex + 1, base.Provider.Count);
            int num = base.painter.StringWidth(text);
            base.painter.BackColor = EditConsts.DefaultArrowBackColor;
            base.painter.FillRectangle(2, 3, 9, 11);
            int x = (6 + num) + 9;
            base.painter.FillRectangle(x, 3, 9, 11);
            Point point = new Point(3, 9);
            Point point2 = new Point(6, 6);
            Point point3 = new Point(9, 9);
            Point[] points = new Point[] { point, point2, point3 };
            base.painter.DrawPolygon(points, EditConsts.DefaultArrowForeColor);
            point = new Point(x + 1, 7);
            point2 = new Point(x + 4, 10);
            point3 = new Point((x + 9) - 2, 7);
            Point[] pointArray2 = new Point[] { point, point2, point3 };
            base.painter.DrawPolygon(pointArray2, EditConsts.DefaultArrowForeColor);
            base.painter.TextOut(text, text.Length, 13, 2);
        }

        public override void DrawLine(int index, string line, short[] colorData, Point position, Rectangle clipRect)
        {
            if ((index == 0) && this.NeedArrows)
            {
                this.DrawArrows();
            }
            base.DrawLine(index, line, colorData, position, clipRect);
        }

        protected string GetNameStr()
        {
            string str = string.Empty;
            if (((base.Provider != null) && (this.selectedIndex >= 0)) && (this.selectedIndex < base.Provider.Count))
            {
                if (base.Provider.ColumnCount == 0)
                {
                    return ((CodeCompletionProvider) base.Provider).GetName(this.selectedIndex);
                }
                str = string.Empty;
                for (int i = 0; i < base.Provider.ColumnCount; i++)
                {
                    if (base.Provider.ColumnVisible(i))
                    {
                        string columnText = base.Provider.GetColumnText(this.selectedIndex, i);
                        if ((columnText != null) && (columnText != string.Empty))
                        {
                            if (str == string.Empty)
                            {
                                str = columnText;
                            }
                            else
                            {
                                str = str + ' ' + columnText;
                            }
                        }
                    }
                }
            }
            return str;
        }

        protected override void GetString(int index, ref string line, ref short[] colorData)
        {
            IStringItem item = this.strings[index];
            line = item.String;
            colorData = item.TextData;
        }

        private int MeasureArrows()
        {
            if (this.NeedArrows)
            {
                string text = string.Format(StringConsts.NofMstr, this.selectedIndex + 1, base.Provider.Count);
                return ((base.painter.StringWidth(text) + 0x12) + 8);
            }
            return 0;
        }

        public override void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint)
        {
            painter.BackColor = EditConsts.DefaultInfoBackColor;
            painter.FontStyle = FontStyle.Regular;
            painter.FillRectangle(0, 0, rect.Width, rect.Height);
            if (base.Provider != null)
            {
                painter.TextColor = base.control.ForeColor;
                painter.Opaque = false;
                int x = 0;
                int y = 1;
                for (int i = 0; i < this.strings.Count; i++)
                {
                    if ((i == 0) && (base.Provider.Count > 1))
                    {
                        this.DrawArrows();
                        x = this.MeasureArrows() + 2;
                    }
                    else
                    {
                        x = 2;
                    }
                    this.DrawLine(i, new Point(x, y), rect);
                    y += painter.FontHeight + 4;
                }
            }
        }

        public void ProviderChanged(ICodeCompletionProvider provider, int index)
        {
            base.Provider = provider;
            this.selectedIndex = index;
        }

        protected virtual void TextChanged()
        {
            this.strings.Clear();
            if (((base.Provider != null) && (this.selectedIndex >= 0)) && (this.selectedIndex < base.Provider.Count))
            {
                base.AddMultiLine(this.GetNameStr(), (this.Lexer != null) ? this.Lexer.DefaultState : 0, true);
                string description = ((CodeCompletionProvider) base.Provider).GetDescription(this.selectedIndex);
                if ((description != null) && (description != string.Empty))
                {
                    base.AddMultiLine(description, (this.Lexer != null) ? this.Lexer.DefaultState : 0, true);
                }
            }
        }

        public bool UpdateHint(ICodeCompletionProvider provider, int index)
        {
            string nameStr = this.GetNameStr();
            if (this.nameStr != nameStr)
            {
                this.nameStr = nameStr;
                return true;
            }
            return false;
        }

        public Size UpdateSize()
        {
            this.TextChanged();
            int num = 0;
            for (int i = 0; i < this.strings.Count; i++)
            {
                int width = ((HintItem) this.strings[i]).Width;
                if (i == 0)
                {
                    width += this.MeasureArrows();
                }
                num = Math.Max(num, width);
            }
            return new Size(num + 6, ((base.painter.FontHeight + 4) * Math.Max(this.strings.Count, 1)) + 1);
        }

        public Rectangle LeftArrowArea
        {
            get
            {
                return new Rectangle(2, 3, 9, 11);
            }
        }

        public bool NeedArrows
        {
            get
            {
                return ((base.Provider != null) && (base.Provider.Count > 1));
            }
        }

        [Description("Represents an array of strings displayed in a hint.")]
        public IList<IStringItem> Strings
        {
            get
            {
                return this.strings;
            }
        }

        internal class HintItem : StringItem
        {
            private int width;

            public HintItem(string s, int width) : base(s)
            {
                this.width = width;
            }

            public int Width
            {
                get
                {
                    return this.width;
                }
                set
                {
                    this.width = value;
                }
            }
        }
    }
}

