namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public class DisplayStrings : IDisplayStrings, IStringList, IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable, ITabulation, IWordWrap, ITextSearch, IUpdate, IWordBreak, ICollapsable, ITextExport, IExport, ITextImport, IImport
    {
        private bool allowOutlining;
        private IList<IRange> collapsedList;
        private int lastDisplayIndex = -1;
        private int lastWrapIndex = -1;
        private int lastWrapped = -1;
        private bool lineEnd;
        private ITextStrings lines;
        private int maxLineIndex = -1;
        private int maxLineWidth;
        private Outlining.OutlineList outlineList;
        private QWhale.Editor.OutlineOptions outlineOptions = EditConsts.DefaultOutlineOptions;
        private ISyntaxEdit owner;
        private bool recalcFlag = true;
        private char[] tabArray = new char[] { '\t' };
        private int updateCount;
        private bool wordWrap;
        private bool wrapAtMargin;
        private WrapList wrapList;
        private int wrapMargin;

        public DisplayStrings(ISyntaxEdit owner, ITextStrings strings)
        {
            this.owner = owner;
            this.outlineList = new Outlining.OutlineList(this);
            this.wrapList = new WrapList(this);
            this.collapsedList = new List<IRange>();
            this.Lines = strings;
        }

        public virtual void Add(string item)
        {
        }

        protected void ApplyWhiteSpace(string str, ref short[] colorData)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] == ' ') || (str[i] == '　'))
                {
                    StringItem.SetTextStyle(ref colorData, i, 1, TextStyle.WhiteSpace);
                }
            }
        }

        public virtual int BeginUpdate()
        {
            this.outlineList.BeginUpdate();
            this.updateCount++;
            return this.updateCount;
        }

        public virtual void BlockDeleting(Rectangle rect)
        {
            if (this.allowOutlining)
            {
                this.outlineList.BlockDeleting(rect);
            }
        }

        private void CheckPage(IEditPages pages, ref int page, int displayLine, ref int margin)
        {
            if (pages[page].EndLine < displayLine)
            {
                page++;
                pages.Update(pages[page]);
                if (!this.wrapAtMargin)
                {
                    margin = pages[page].DisplayWidth;
                }
            }
        }

        public virtual void Clear()
        {
            this.lines.Clear();
            this.outlineList.Clear();
            this.collapsedList.Clear();
        }

        private void ClearLastLine()
        {
            this.lastWrapped = -1;
            this.lastWrapIndex = -1;
            this.lastDisplayIndex = -1;
        }

        public virtual void Collapse(int index)
        {
            IList<IRange> ranges = new List<IRange>();
            this.GetOutlineRanges(ranges, index);
            this.BeginUpdate();
            try
            {
                foreach (IOutlineRange range in ranges)
                {
                    if ((range.StartPoint.Y == index) && range.Visible)
                    {
                        range.Visible = false;
                    }
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual void CollapseToDefinitions()
        {
            if (this.owner.Source.NeedOutlineText())
            {
                ISyntaxParser lexer = this.owner.Lexer as ISyntaxParser;
                this.BeginUpdate();
                try
                {
                    foreach (IOutlineRange range in this.outlineList)
                    {
                        ISyntaxNode nodeAt = lexer.GetNodeAt(range.StartPoint);
                        range.Visible = (nodeAt != null) && lexer.IsDeclaration(nodeAt);
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public virtual bool Contains(string item)
        {
            return (this.IndexOf(item) >= 0);
        }

        public virtual void CopyTo(string[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < Math.Min(this.DisplayCount + arrayIndex, array.Length); i++)
            {
                array[i] = this[i - arrayIndex];
            }
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual Point DisplayPointToPoint(Point point)
        {
            return this.DisplayPointToPoint(point.X, point.Y, false, false, false);
        }

        public virtual Point DisplayPointToPoint(int x, int y)
        {
            bool lineEnd = false;
            return this.DisplayPointToPoint(x, y, false, false, false, ref lineEnd);
        }

        public virtual Point DisplayPointToPoint(int x, int y, ref bool lineEnd)
        {
            return this.DisplayPointToPoint(x, y, true, false, false, ref lineEnd);
        }

        public virtual Point DisplayPointToPoint(int x, int y, bool wrapEnd, bool rangeStart, bool tabEnd)
        {
            bool lineEnd = false;
            return this.DisplayPointToPoint(x, y, wrapEnd, rangeStart, tabEnd, ref lineEnd);
        }

        protected Point DisplayPointToPoint(int x, int y, bool wrapEnd, bool rangeStart, bool tabEnd, ref bool lineEnd)
        {
            Point point = new Point(x, y);
            int num = point.Y - Math.Max(this.DisplayCount - 1, 0);
            if (num > 0)
            {
                if ((this.owner.NavigateOptions & NavigateOptions.BeyondEof) != NavigateOptions.None)
                {
                    return new Point(x, Math.Max(this.owner.Lines.Count - 1, 0) + num);
                }
                point.Y -= num;
            }
            lineEnd = false;
            if (this.wordWrap)
            {
                this.wrapList.GetRealPoint(ref point, wrapEnd, ref lineEnd);
            }
            if (point.X != 0x7fffffff)
            {
                string text = string.Empty;
                short[] data = null;
                this.GetOutlineString(this.allowOutlining ? this.outlineList.DisplayLineToLine(point.Y) : point.Y, ref text, ref data, false, false);
                point.X = this.lines.PosToTabPos(text, point.X, tabEnd);
            }
            if (this.allowOutlining)
            {
                this.outlineList.GetRealPoint(ref point, rangeStart);
            }
            return point;
        }

        public virtual int EnableUpdate()
        {
            this.updateCount--;
            return this.updateCount;
        }

        public virtual int EndUpdate()
        {
            this.outlineList.EndUpdate();
            this.updateCount--;
            return this.updateCount;
        }

        public virtual void EnsureExpanded(Point position)
        {
            if (this.allowOutlining)
            {
                IList<IRange> ranges = new List<IRange>();
                this.GetOutlineRanges(ranges, position);
                bool flag = false;
                foreach (IOutlineRange range in ranges)
                {
                    if (!range.Visible && !range.StartPoint.Equals(position))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    this.BeginUpdate();
                    try
                    {
                        foreach (IOutlineRange range2 in ranges)
                        {
                            if (!range2.Visible && !range2.StartPoint.Equals(position))
                            {
                                range2.Visible = true;
                            }
                        }
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        public virtual void EnsureExpanded(int index)
        {
            if (this.allowOutlining)
            {
                IList<IRange> ranges = new List<IRange>();
                this.GetOutlineRanges(ranges, index);
                bool flag = false;
                foreach (IOutlineRange range in ranges)
                {
                    if (!range.Visible)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    this.BeginUpdate();
                    try
                    {
                        foreach (IOutlineRange range2 in ranges)
                        {
                            range2.Visible = true;
                        }
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        private void EnsureWrapped(int index, bool wrapped)
        {
            if ((wrapped && (index > this.lastWrapIndex)) || (!wrapped && (index > this.lastDisplayIndex)))
            {
                if (index != 0x7fffffff)
                {
                    index += EditConsts.DefaultWrapDelta;
                }
                int displayLine = ((this.lastWrapped + 1) > 0) ? (this.PointToDisplayPoint(0x7fffffff, this.lastWrapped).Y + 1) : 0;
                this.WrapLines(this.lastWrapped + 1, index, displayLine);
            }
        }

        public virtual void Expand(int index)
        {
            IList<IRange> ranges = new List<IRange>();
            this.GetOutlineRanges(ranges, index);
            this.BeginUpdate();
            try
            {
                foreach (IOutlineRange range in ranges)
                {
                    if ((range.StartPoint.Y == index) && !range.Visible)
                    {
                        range.Visible = true;
                    }
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual bool Find(string s, SearchOptions options, Regex expression, ref Point position, out int len, out Match match)
        {
            return TextStrings.Find(this, this.lines.DelimTable, s, options, expression, ref position, out len, out match, this.LineTerminator);
        }

        public virtual void FullCollapse()
        {
            this.FullCollapse(this.outlineList.GetRanges());
        }

        public virtual void FullCollapse(IList<IRange> ranges)
        {
            this.BeginUpdate();
            try
            {
                foreach (IOutlineRange range in ranges)
                {
                    range.Visible = false;
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual void FullExpand()
        {
            this.FullExpand(this.outlineList.GetRanges());
        }

        public virtual void FullExpand(IList<IRange> ranges)
        {
            if (ranges.Count > 0)
            {
                this.BeginUpdate();
                try
                {
                    foreach (IOutlineRange range in ranges)
                    {
                        range.Visible = true;
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public virtual short[] GetColorData(int index)
        {
            string text = string.Empty;
            short[] data = null;
            this.GetStringAndColorData(index, ref text, ref data, true);
            return data;
        }

        public virtual IEnumerator<string> GetEnumerator()
        {
            return new DisplayEnumerator(this);
        }

        public virtual string GetIndentString(int count, int pos)
        {
            return this.lines.GetIndentString(count, pos);
        }

        public virtual string GetIndentString(int count, int p, bool useSpaces)
        {
            return this.lines.GetIndentString(count, p, useSpaces);
        }

        public virtual int GetLexStyle(Point position)
        {
            short[] colorData = this.GetColorData(position.Y);
            if (((colorData != null) && (position.X >= 0)) && (position.X < colorData.Length))
            {
                return (colorData[position.X] - 1);
            }
            return -1;
        }

        private int GetLineWidth(int index)
        {
            if (index < 0)
            {
                return 0;
            }
            string text = string.Empty;
            short[] data = null;
            this.GetStringAndColorData(index, ref text, ref data, true);
            return this.owner.SyntaxPaint.MeasureLine(text, data, 0, -1);
        }

        public virtual string GetOutlineHint(IOutlineRange range)
        {
            string str = string.Empty;
            int num = Math.Min(this.lines.Count - 1, range.EndPoint.Y);
            int num2 = 0;
            int num3 = -1;
            IList<string> list = new List<string>();
            for (int i = range.StartPoint.Y; i <= num; i++)
            {
                string s = this.lines[i];
                if (i == range.EndPoint.Y)
                {
                    s = s.Substring(0, Math.Min(s.Length, range.EndPoint.X));
                }
                if (i == range.StartPoint.Y)
                {
                    if (range.StartPoint.X >= s.TrimEnd(new char[0]).Length)
                    {
                        continue;
                    }
                    int x = range.StartPoint.X;
                    if (x <= (s.Length - s.TrimStart(new char[0]).Length))
                    {
                        x = 0;
                    }
                    s = s.Substring(Math.Min(x, s.TrimEnd(new char[0]).Length));
                }
                s = this.lines.GetTabString(s);
                if (s.Trim() != string.Empty)
                {
                    if (num3 == -1)
                    {
                        num3 = s.Length - s.TrimStart(new char[0]).Length;
                    }
                    else
                    {
                        num3 = Math.Min(num3, s.Length - s.TrimStart(new char[0]).Length);
                    }
                }
                list.Add(s);
                if (i == num)
                {
                    break;
                }
                num2++;
                if (num2 >= EditConsts.MaxHintWindowCount)
                {
                    break;
                }
            }
            foreach (string str3 in list)
            {
                if (str == string.Empty)
                {
                    str = ((num3 > 0) && (num3 < str3.Length)) ? str3.Substring(num3) : str3;
                }
                else
                {
                    str = str + "\r\n" + (((num3 > 0) && (num3 < str3.Length)) ? str3.Substring(num3) : str3);
                }
            }
            if (num2 >= EditConsts.MaxHintWindowCount)
            {
                str = str + EditConsts.DottedText;
            }
            return str;
        }

        private int GetOutlineLevel(Point point)
        {
            IOutlineRange outlineRange = this.GetOutlineRange(point);
            if (outlineRange == null)
            {
                return 0;
            }
            return (outlineRange.Level + 1);
        }

        public virtual IOutlineRange GetOutlineRange(Point position)
        {
            if (this.allowOutlining)
            {
                return (this.outlineList.FindRange(position) as IOutlineRange);
            }
            return null;
        }

        public virtual IOutlineRange GetOutlineRange(int index)
        {
            if (this.allowOutlining)
            {
                return (this.outlineList.FindRange(index) as IOutlineRange);
            }
            return null;
        }

        public virtual int GetOutlineRanges(IList<IRange> ranges)
        {
            if (this.allowOutlining)
            {
                this.outlineList.GetRanges(ranges);
            }
            else
            {
                ranges.Clear();
            }
            return ranges.Count;
        }

        public virtual int GetOutlineRanges(IList<IRange> ranges, Point position)
        {
            if (this.allowOutlining)
            {
                this.outlineList.GetRanges(ranges, position);
            }
            else
            {
                ranges.Clear();
            }
            return ranges.Count;
        }

        public virtual int GetOutlineRanges(IList<IRange> ranges, int index)
        {
            if (this.allowOutlining)
            {
                this.outlineList.GetRanges(ranges, index);
            }
            else
            {
                ranges.Clear();
            }
            return ranges.Count;
        }

        public virtual int GetOutlineRanges(IList<IRange> ranges, Point startPoint, Point endPoint)
        {
            if (this.allowOutlining)
            {
                this.outlineList.GetRanges(ranges, startPoint, endPoint);
            }
            else
            {
                ranges.Clear();
            }
            return ranges.Count;
        }

        private void GetOutlineString(int index, ref string text, ref short[] data, bool needData, bool applyTabs)
        {
            if (this.allowOutlining)
            {
                this.outlineList.CheckVisible(ref index);
                if (this.GetString(index, ref text, ref data, needData, false))
                {
                    int length = text.Length;
                    this.outlineList.GetCollapsedRanges(index, this.collapsedList);
                    for (int i = this.collapsedList.Count - 1; i >= 0; i--)
                    {
                        IOutlineRange range = this.collapsedList[i] as IOutlineRange;
                        if (!range.IsEmpty)
                        {
                            string displayText = range.DisplayText;
                            int startIndex = 0;
                            int count = 0;
                            int num5 = Math.Min(text.Length, length);
                            startIndex = Math.Min(range.StartPoint.X, num5);
                            if (range.EndPoint.Y == index)
                            {
                                count = Math.Min((int) (num5 - startIndex), (int) (range.EndPoint.X - range.StartPoint.X));
                                text = text.Remove(startIndex, count).Insert(startIndex, displayText);
                                if (needData)
                                {
                                    short[] destinationArray = new short[(data.Length - count) + displayText.Length];
                                    Array.Copy(data, 0, destinationArray, 0, startIndex);
                                    Array.Copy(data, (int) (startIndex + count), destinationArray, (int) (startIndex + displayText.Length), (int) ((data.Length - count) - startIndex));
                                    data = destinationArray;
                                    StringItem.SetTextStyle(ref data, startIndex, displayText.Length, TextStyle.OutlineSection);
                                }
                            }
                            else
                            {
                                string str2 = string.Empty;
                                short[] colorData = null;
                                if (this.GetString(range.EndPoint.Y, ref str2, ref colorData, needData, false))
                                {
                                    if (range.EndPoint.X < str2.Length)
                                    {
                                        text = text.Substring(0, Math.Min(startIndex, text.Length)) + displayText + str2.Substring(range.EndPoint.X);
                                    }
                                    else
                                    {
                                        text = text.Substring(0, Math.Min(startIndex, text.Length)) + displayText;
                                    }
                                    if (needData)
                                    {
                                        int num6 = Math.Max(colorData.Length - range.EndPoint.X, 0);
                                        short[] numArray3 = new short[(startIndex + displayText.Length) + num6];
                                        Array.Copy(data, 0, numArray3, 0, startIndex);
                                        if (num6 > 0)
                                        {
                                            Array.Copy(colorData, range.EndPoint.X, numArray3, startIndex + displayText.Length, num6);
                                        }
                                        data = numArray3;
                                        StringItem.SetTextStyle(ref data, startIndex, displayText.Length, TextStyle.OutlineSection);
                                    }
                                }
                            }
                        }
                    }
                }
                if (applyTabs)
                {
                    this.lines.GetTabString(ref text, ref data, needData && (data != null), null);
                }
            }
            else
            {
                this.GetString(index, ref text, ref data, needData, applyTabs);
            }
        }

        public virtual int GetPrevTabStop(int pos)
        {
            return this.lines.GetPrevTabStop(pos);
        }

        protected bool GetString(int index, ref string text, ref short[] colorData, bool needData, bool applyTabs)
        {
            if ((index < 0) || (index >= this.lines.Count))
            {
                return false;
            }
            IStringItem item = this.lines.GetItem(index);
            text = item.String;
            if (needData)
            {
                colorData = item.TextData;
                this.ApplyWhiteSpace(text, ref colorData);
            }
            if (applyTabs)
            {
                this.lines.GetTabString(ref text, ref colorData, needData, null);
            }
            return true;
        }

        public virtual int GetStringAndColorData(int index, ref string text, ref short[] data)
        {
            return this.GetStringAndColorData(index, ref text, ref data, true);
        }

        protected int GetStringAndColorData(int index, ref string text, ref short[] data, bool needData)
        {
            int y = index;
            if ((this.wordWrap && (this.wrapList.Count > 0)) || this.allowOutlining)
            {
                Point point = new Point(0, index);
                if (this.wordWrap)
                {
                    bool lineEnd = false;
                    this.wrapList.GetRealPoint(ref point, false, ref lineEnd);
                }
                if (this.allowOutlining)
                {
                    this.outlineList.GetRealPoint(ref point, false);
                }
                this.GetOutlineString(point.Y, ref text, ref data, needData, true);
                y = point.Y;
                if (this.wordWrap)
                {
                    int p = 0;
                    int len = 0x7fffffff;
                    this.GetWrapBounds(index, ref p, ref len);
                    if ((p != 0) || (len != 0x7fffffff))
                    {
                        this.GetSubString(p, len, ref text, ref data, needData && (data != null));
                    }
                }
                return y;
            }
            this.GetString(index, ref text, ref data, needData, true);
            return y;
        }

        private void GetSubString(int p, int len, ref string text, ref short[] data, bool needData)
        {
            int length = text.Length;
            p = Math.Min(p, length);
            len = Math.Min(len, length - p);
            text = text.Substring(p, len);
            if (needData)
            {
                short[] destinationArray = new short[len];
                Array.Copy(data, p, destinationArray, 0, len);
                data = destinationArray;
            }
        }

        public virtual int GetTabStop(int pos)
        {
            return this.lines.GetTabStop(pos);
        }

        public virtual string GetTabString(string s)
        {
            return this.lines.GetTabString(s);
        }

        public virtual void GetTabString(string s, ITextUndoList operations)
        {
            this.lines.GetTabString(s, operations);
        }

        public virtual string GetTextAt(Point position)
        {
            return this.GetTextAt(position.X, position.Y);
        }

        public virtual string GetTextAt(int pos, int line)
        {
            int num;
            int num2;
            string s = this[line];
            if (this.GetWord(s, pos, out num, out num2))
            {
                return s.Substring(num, (num2 - num) + 1);
            }
            return string.Empty;
        }

        public virtual bool GetWord(int index, int pos, out int left, out int right)
        {
            return this.GetWord(this[index], pos, out left, out right);
        }

        public virtual bool GetWord(string s, int pos, out int left, out int right)
        {
            return this.lines.GetWord(s, pos, out left, out right);
        }

        public virtual bool GetWord(string s, int pos, out int left, out int right, Hashtable delims)
        {
            return this.lines.GetWord(s, pos, out left, out right, delims);
        }

        private void GetWrapBounds(int index, ref int p, ref int len)
        {
            if (this.wordWrap)
            {
                this.wrapList.GetWrapBounds(index, ref p, ref len);
            }
        }

        public virtual int GetWrapMargin()
        {
            return this.wrapMargin;
        }

        public virtual int IndexOf(string item)
        {
            for (int i = 0; i < this.DisplayCount; i++)
            {
                if (this[i] == item)
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual void Insert(int index, string item)
        {
        }

        public virtual bool IsCollapsed(int index)
        {
            IOutlineRange outlineRange = this.GetOutlineRange(index);
            return (((outlineRange != null) && (outlineRange.StartPoint.Y == index)) && !outlineRange.Visible);
        }

        public virtual bool IsDelimiter(char ch)
        {
            return this.lines.IsDelimiter(ch);
        }

        public virtual bool IsDelimiter(int index, int pos)
        {
            return this.lines.IsDelimiter(this[index], pos);
        }

        public virtual bool IsDelimiter(string s, int pos)
        {
            return this.lines.IsDelimiter(s, pos);
        }

        public virtual bool IsExpanded(int index)
        {
            IOutlineRange outlineRange = this.GetOutlineRange(index);
            return ((outlineRange != null) && outlineRange.Visible);
        }

        public virtual bool IsPointCollapsed(Point position, out IRange range)
        {
            range = null;
            IList<IRange> ranges = new List<IRange>();
            this.GetOutlineRanges(ranges, position);
            foreach (IOutlineRange range2 in ranges)
            {
                if (!range2.Visible)
                {
                    range = range2;
                    return true;
                }
            }
            return false;
        }

        public bool IsPointVisible(Point position)
        {
            bool flag = true;
            Point point = this.owner.TextToScreen(position);
            if (this.owner.Scrolling.ScrollByPixels)
            {
                Rectangle clientRect = this.owner.ClientRect;
                if (this.owner.Pages.PageType != PageType.PageLayout)
                {
                    clientRect.X += this.owner.Gutter.Rect.Width;
                    clientRect.Width -= this.owner.Gutter.Rect.Width;
                }
                if (point.X < clientRect.Left)
                {
                    flag = false;
                }
                else
                {
                    int width = this.owner.GetCaretSize(position).Width;
                    if (point.X > (clientRect.Right - width))
                    {
                        flag = false;
                    }
                }
                if (point.Y < clientRect.Top)
                {
                    return false;
                }
                if (point.Y > (clientRect.Bottom - this.owner.Painter.FontHeight))
                {
                    flag = false;
                }
                return flag;
            }
            position = this.PointToDisplayPoint(position);
            int num2 = position.X - this.owner.Scrolling.WindowOriginX;
            int num3 = position.Y - this.owner.Scrolling.WindowOriginY;
            if (num2 < 0)
            {
                flag = (this.owner.Scrolling.Options & ScrollingOptions.UseScrollDelta) != ScrollingOptions.None;
            }
            else
            {
                int num4 = this.owner.GetCaretSize(this.owner.Position).Width;
                int right = this.owner.ClientRect.Right;
                if (point.X > (right - num4))
                {
                    if ((this.owner.Scrolling.Options & ScrollingOptions.UseScrollDelta) != ScrollingOptions.None)
                    {
                        right -= right / EditConsts.DefaultScrollDeltaRatio;
                    }
                    flag = false;
                }
            }
            if (num3 < 0)
            {
                return false;
            }
            if (num3 > (this.owner.LinesInHeight - 1))
            {
                flag = false;
            }
            return flag;
        }

        public virtual bool IsVisible(Point position)
        {
            return this.outlineList.IsVisible(position);
        }

        public virtual bool IsVisible(int index)
        {
            return this.outlineList.IsVisible(index);
        }

        protected void LinesChanged()
        {
            this.UpdateWordWrap();
            this.Recalulate();
        }

        public virtual bool LoadFile(string fileName)
        {
            return this.lines.LoadFile(fileName);
        }

        public virtual bool LoadFile(string fileName, IStringImport importer)
        {
            return this.lines.LoadFile(fileName, importer);
        }

        public virtual bool LoadFile(string fileName, Encoding encoding)
        {
            return this.lines.LoadFile(fileName, encoding);
        }

        public virtual bool LoadFile(string fileName, IStringImport importer, Encoding encoding)
        {
            return this.lines.LoadFile(fileName, importer, encoding);
        }

        public virtual bool LoadStream(Stream stream)
        {
            return this.lines.LoadStream(stream);
        }

        public virtual bool LoadStream(TextReader reader)
        {
            return this.lines.LoadStream(reader);
        }

        public virtual bool LoadStream(Stream stream, IStringImport importer)
        {
            return this.lines.LoadStream(stream, importer);
        }

        public virtual bool LoadStream(Stream stream, Encoding encoding)
        {
            return this.lines.LoadStream(stream, encoding);
        }

        public virtual bool LoadStream(TextReader reader, IStringImport importer)
        {
            return this.lines.LoadStream(reader, importer);
        }

        public virtual bool LoadStream(Stream stream, IStringImport importer, Encoding encoding)
        {
            return this.lines.LoadStream(stream, importer, encoding);
        }

        public void Notify(NotifyState state, int first, int last)
        {
            this.Notify(state, first, last, true);
        }

        protected void Notify(NotifyState state, int first, int last, bool update)
        {
            this.owner.Notification(this, new NotifyEventArgs(state, first, last, update));
        }

        protected virtual void OnAllowOutliningChanged()
        {
            if (!this.allowOutlining)
            {
                this.UnOutline();
            }
            else
            {
                this.owner.Outlining.OutlineText();
            }
        }

        protected virtual void OnDelimitersChanged()
        {
        }

        protected virtual void OnDelimiterStringChanged()
        {
        }

        protected virtual void OnLineEndChanged()
        {
        }

        protected virtual void OnOutlineOptionsChanged()
        {
        }

        protected virtual void OnTabStopsChanged()
        {
        }

        protected virtual void OnUseSpacesChanged()
        {
        }

        public virtual IOutlineRange Outline(Point startPoint, Point endPoint)
        {
            return this.Outline(startPoint, endPoint, this.GetOutlineLevel(startPoint), EditConsts.DefaultOutlineText);
        }

        public virtual IOutlineRange Outline(int first, int last)
        {
            return this.Outline(new Point(0, first), new Point(0x7fffffff, last), this.GetOutlineLevel(new Point(0, first)), EditConsts.DefaultOutlineText);
        }

        public virtual IOutlineRange Outline(Point startPoint, Point endPoint, int level)
        {
            return this.Outline(startPoint, endPoint, level, EditConsts.DefaultOutlineText);
        }

        public virtual IOutlineRange Outline(Point startPoint, Point endPoint, string outlineText)
        {
            return this.Outline(startPoint, endPoint, this.GetOutlineLevel(startPoint), outlineText);
        }

        public virtual IOutlineRange Outline(int first, int last, int level)
        {
            return this.Outline(new Point(0, first), new Point(0x7fffffff, last), level, EditConsts.DefaultOutlineText);
        }

        public virtual IOutlineRange Outline(int first, int last, string outlineText)
        {
            return this.Outline(new Point(0, first), new Point(0x7fffffff, last), this.GetOutlineLevel(new Point(0, first)), outlineText);
        }

        public virtual IOutlineRange Outline(Point startPoint, Point endPoint, int level, string outlineText)
        {
            if (this.allowOutlining)
            {
                IOutlineRange range = new Outlining.DisplayRange(this.outlineList, startPoint, endPoint, level, outlineText);
                this.outlineList.Add(range);
                return range;
            }
            return null;
        }

        public virtual IOutlineRange Outline(int first, int last, int level, string outlineText)
        {
            return this.Outline(new Point(0, first), new Point(0x7fffffff, last), level, outlineText);
        }

        public virtual Point PointToDisplayPoint(Point point)
        {
            return this.PointToDisplayPoint(point.X, point.Y, this.lineEnd);
        }

        public virtual Point PointToDisplayPoint(int x, int y)
        {
            return this.PointToDisplayPoint(x, y, this.lineEnd);
        }

        public virtual Point PointToDisplayPoint(int x, int y, bool lineEnd)
        {
            Point point = new Point(x, y);
            if (this.allowOutlining)
            {
                this.outlineList.GetDisplayPoint(ref point);
            }
            if (point.X != 0x7fffffff)
            {
                string text = string.Empty;
                short[] data = null;
                this.GetOutlineString(y, ref text, ref data, false, false);
                point.X = this.lines.TabPosToPos(text, point.X);
            }
            if (this.wordWrap)
            {
                this.wrapList.GetDisplayPoint(ref point, lineEnd);
            }
            return point;
        }

        public virtual void PositionChanged(UpdateReason reason, int x, int y, int deltaX, int deltaY)
        {
            if (this.allowOutlining)
            {
                this.outlineList.PositionChanged(x, y, deltaX, deltaY);
            }
            this.Recalulate(reason, x, y, deltaX, deltaY);
        }

        public virtual int PosToTabPos(string s, int pos)
        {
            return this.lines.PosToTabPos(s, pos);
        }

        public virtual int PosToTabPos(string s, int pos, bool tabEnd)
        {
            return this.lines.PosToTabPos(s, pos, tabEnd);
        }

        private void Recalulate()
        {
            int lineWidth = 0;
            this.maxLineWidth = 0;
            this.ScanToEnd(false);
            for (int i = 0; i < this.DisplayCount; i++)
            {
                lineWidth = this.GetLineWidth(i);
                if (this.maxLineWidth < lineWidth)
                {
                    this.maxLineWidth = lineWidth;
                    this.maxLineIndex = i;
                }
            }
            this.recalcFlag = false;
        }

        private void Recalulate(UpdateReason reason, int x, int y, int deltaX, int deltaY)
        {
            if (!this.recalcFlag && !this.owner.Scrolling.FixedScrollSize)
            {
                int index = this.PointToDisplayPoint(x, y).Y;
                switch (reason)
                {
                    case UpdateReason.Insert:
                    {
                        int lineWidth = this.GetLineWidth(index);
                        if (this.maxLineWidth >= lineWidth)
                        {
                            break;
                        }
                        this.maxLineWidth = lineWidth;
                        this.maxLineIndex = index;
                        return;
                    }
                    case UpdateReason.Delete:
                        if (index != this.maxLineIndex)
                        {
                            break;
                        }
                        this.recalcFlag = true;
                        return;

                    case UpdateReason.Break:
                        if (index != this.maxLineIndex)
                        {
                            if (index >= this.maxLineIndex)
                            {
                                break;
                            }
                            this.maxLineIndex++;
                            return;
                        }
                        this.recalcFlag = true;
                        return;

                    case UpdateReason.UnBreak:
                    {
                        int num7 = this.GetLineWidth(index);
                        if (this.maxLineWidth >= num7)
                        {
                            if (index >= this.maxLineIndex)
                            {
                                break;
                            }
                            this.maxLineIndex--;
                            if (this.maxLineIndex >= 0)
                            {
                                break;
                            }
                            this.recalcFlag = true;
                            return;
                        }
                        this.maxLineWidth = num7;
                        this.maxLineIndex = index;
                        return;
                    }
                    case UpdateReason.DeleteBlock:
                    {
                        int num6 = this.PointToDisplayPoint(x - deltaX, y - deltaY).Y;
                        if ((this.maxLineIndex < index) || (this.maxLineIndex > num6))
                        {
                            break;
                        }
                        this.recalcFlag = true;
                        return;
                    }
                    case UpdateReason.InsertBlock:
                    {
                        int num3 = this.PointToDisplayPoint(x + deltaX, y + deltaY).Y;
                        for (int i = index; i <= num3; i++)
                        {
                            int num5 = this.GetLineWidth(i);
                            if (this.maxLineWidth < num5)
                            {
                                this.maxLineWidth = num5;
                                this.maxLineIndex = index;
                            }
                        }
                        return;
                    }
                    default:
                        this.recalcFlag = true;
                        break;
                }
            }
        }

        public virtual bool Remove(string item)
        {
            return false;
        }

        public virtual void RemoveAt(int index)
        {
        }

        public virtual void ResetAllowOutlining()
        {
            this.AllowOutlining = false;
        }

        public virtual void ResetDelimiters()
        {
            this.Delimiters = EditConsts.DefaultDelimiters.ToCharArray();
        }

        public virtual void ResetOutlineOptions()
        {
            this.OutlineOptions = EditConsts.DefaultOutlineOptions;
        }

        public virtual void ResetTabStops()
        {
            this.TabStops = new int[] { EditConsts.DefaultTabStop };
        }

        public virtual void ResetUseSpaces()
        {
            this.UseSpaces = false;
        }

        public virtual void ResetWordWrap()
        {
            this.WordWrap = false;
        }

        public virtual void ResetWrapAtMargin()
        {
            this.WrapAtMargin = false;
        }

        public virtual bool SaveFile(string fileName)
        {
            return this.lines.SaveFile(fileName);
        }

        public virtual bool SaveFile(string fileName, IStringExport exporter)
        {
            return this.lines.SaveFile(fileName, exporter);
        }

        public virtual bool SaveFile(string fileName, Encoding encoding)
        {
            return this.lines.SaveFile(fileName, encoding);
        }

        public virtual bool SaveFile(string fileName, IStringExport exporter, Encoding encoding)
        {
            return this.lines.SaveFile(fileName, exporter, encoding);
        }

        public virtual bool SaveStream(Stream stream)
        {
            return this.lines.SaveStream(stream);
        }

        public virtual bool SaveStream(TextWriter writer)
        {
            return this.lines.SaveStream(writer);
        }

        public virtual bool SaveStream(Stream stream, IStringExport exporter)
        {
            return this.lines.SaveStream(stream, exporter);
        }

        public virtual bool SaveStream(Stream stream, Encoding encoding)
        {
            return this.lines.SaveStream(stream, encoding);
        }

        public virtual bool SaveStream(TextWriter writer, IStringExport exporter)
        {
            return this.lines.SaveStream(writer, exporter);
        }

        public virtual bool SaveStream(Stream stream, IStringExport exporter, Encoding encoding)
        {
            return this.lines.SaveStream(stream, exporter, encoding);
        }

        protected virtual void ScanToEnd(bool parseToEnd)
        {
            if (this.wordWrap)
            {
                this.EnsureWrapped(this.lines.Count - 1, false);
            }
            if (parseToEnd)
            {
                this.owner.Source.ParseToString(this.owner.Lines.Count - 1);
            }
        }

        public virtual void SetOutlineRanges(IList<IRange> ranges)
        {
            this.SetOutlineRanges(ranges, false);
        }

        public virtual void SetOutlineRanges(IList<IRange> ranges, bool preserveVisible)
        {
            this.BeginUpdate();
            try
            {
                IList<IRange> list = new List<IRange>();
                foreach (IOutlineRange range in ranges)
                {
                    bool visible = range.Visible;
                    if (preserveVisible)
                    {
                        if (visible)
                        {
                            visible = this.outlineList.FindCollapsedRange(range.StartPoint) == null;
                        }
                        else
                        {
                            IOutlineRange range2 = this.outlineList.FindExactRange(range.StartPoint) as IOutlineRange;
                            visible = (range2 != null) && range2.Visible;
                        }
                    }
                    list.Add(new Outlining.DisplayRange(this.outlineList, range.StartPoint, range.EndPoint, range.Level, range.DisplayText, visible));
                }
                this.outlineList.Clear();
                foreach (IOutlineRange range3 in list)
                {
                    this.outlineList.Add(range3);
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DisplayEnumerator(this);
        }

        public virtual int TabPosToPos(string s, int pos)
        {
            return this.lines.TabPosToPos(s, pos);
        }

        public virtual void ToggleOutlining()
        {
            this.ToggleOutlining(this.outlineList.GetRanges(), this.GetOutlineRange(this.owner.Position));
        }

        public virtual void ToggleOutlining(IList<IRange> ranges, IOutlineRange range)
        {
            if (ranges.Count != 0)
            {
                if (range == null)
                {
                    range = (IOutlineRange) ranges[0];
                }
                if (range.Visible)
                {
                    this.FullCollapse(ranges);
                }
                else
                {
                    this.FullExpand(ranges);
                }
            }
        }

        public void UnOutline()
        {
            this.outlineList.Clear();
        }

        public virtual void UnOutline(Point position)
        {
            this.outlineList.RemoveRange(position);
        }

        public virtual void UnOutline(int index)
        {
            this.outlineList.RemoveRange(index);
        }

        private void UnWrapLines(int first, int last)
        {
            this.ClearLastLine();
            if ((first == 0) && (last == 0x7fffffff))
            {
                this.wrapList.Clear();
            }
            else
            {
                if (this.allowOutlining)
                {
                    first = this.outlineList.LineToDisplayLine(first);
                    last = this.outlineList.LineToDisplayLine(last);
                }
                this.wrapList.ClearLines(first, last);
            }
        }

        public virtual void Update()
        {
            if (this.updateCount == 0)
            {
                this.outlineList.Update();
            }
        }

        public virtual void UpdateNeeded()
        {
            this.recalcFlag = true;
        }

        public virtual bool UpdateWordWrap()
        {
            return this.UpdateWordWrap(0, 0x7fffffff);
        }

        public virtual bool UpdateWordWrap(int first, int last)
        {
            bool flag;
            if (this.wordWrap)
            {
                int count = this.wrapList.Count;
                int displayLine = (first > 0) ? (this.PointToDisplayPoint(0x7fffffff, first - 1).Y + 1) : 0;
                this.UnWrapLines(first, last);
                this.WrapLines(first, last, displayLine);
                flag = (first != last) || (this.wrapList.Count != count);
            }
            else
            {
                flag = this.wrapList.Count != 0;
                this.UnWrapLines(0, 0x7fffffff);
            }
            if (flag)
            {
                last = 0x7fffffff;
            }
            this.Notify(NotifyState.WordWrap, first, last, flag);
            return flag;
        }

        protected virtual bool WrapLine(int index, IEditPages pages, ref int page, int displayIndex, ref int displayLine, ref int margin, bool needData)
        {
            if (this.allowOutlining && !this.IsVisible(index))
            {
                return false;
            }
            string text = string.Empty;
            short[] data = null;
            this.GetOutlineString(index, ref text, ref data, needData, true);
            int pos = 0;
            int length = text.Length;
            if (length != 0)
            {
                while (pos < length)
                {
                    int num3;
                    if (page >= 0)
                    {
                        this.CheckPage(pages, ref page, displayLine, ref margin);
                    }
                    this.owner.SyntaxPaint.MeasureLine(text, data, pos, -1, margin, out num3, true);
                    if ((pos + num3) < length)
                    {
                        for (int i = num3; i > 0; i--)
                        {
                            if (this.IsDelimiter(text, (pos + i) - 1))
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
                    pos += num3;
                    if (pos < length)
                    {
                        this.wrapList.AddItem(displayIndex, pos);
                    }
                    displayLine++;
                }
            }
            else
            {
                if (page >= 0)
                {
                    this.CheckPage(pages, ref page, displayLine, ref margin);
                }
                displayLine++;
            }
            return true;
        }

        private void WrapLines(int first, int last, int displayLine)
        {
            this.wrapMargin = this.WrapMargin;
            bool needData = (this.owner.Source.Lexer != null) && !this.owner.Painter.IsMonoSpaced;
            if (needData)
            {
                this.owner.Source.ParseToString(last);
            }
            int index = Math.Max(first, 0);
            int num2 = Math.Min(last, this.lines.Count - 1);
            int displayIndex = index;
            if (this.allowOutlining)
            {
                this.outlineList.CheckVisible(ref index);
                displayIndex = this.outlineList.LineToDisplayLine(index);
            }
            int num4 = 0;
            int height = this.owner.ClientRect.Height;
            int page = -1;
            IEditPages pages = this.owner.Pages;
            if (pages.PageType != PageType.Normal)
            {
                if (index > 0)
                {
                    page = pages.GetPageIndexAt(0, displayLine);
                }
                else
                {
                    page = 0;
                }
                if (!this.wrapAtMargin)
                {
                    this.wrapMargin = pages[page].DisplayWidth;
                }
            }
            else
            {
                int fontHeight = this.owner.Painter.FontHeight;
                num4 = (this.owner.Scrolling.WindowOriginY + ((fontHeight != 0) ? (this.owner.Height / fontHeight) : 0)) + EditConsts.DefaultWrapDelta;
            }
            for (int i = index; i <= num2; i++)
            {
                if (this.WrapLine(i, pages, ref page, displayIndex, ref displayLine, ref this.wrapMargin, needData))
                {
                    displayIndex++;
                    this.lastWrapped = i;
                    this.lastDisplayIndex = displayIndex;
                    if (last == 0x7fffffff)
                    {
                        if (pages.PageType != PageType.Normal)
                        {
                            if (pages[page].BoundsRect.Top <= height)
                            {
                                continue;
                            }
                            break;
                        }
                        if (displayIndex >= num4)
                        {
                            break;
                        }
                    }
                }
            }
            this.lastWrapIndex = this.wrapList.LineToDisplayLine(this.lastDisplayIndex);
        }

        [Description("Gets or sets a value indicating whether outlining is enabled.")]
        public virtual bool AllowOutlining
        {
            get
            {
                return this.allowOutlining;
            }
            set
            {
                if (this.allowOutlining != value)
                {
                    this.allowOutlining = value;
                    this.OnAllowOutliningChanged();
                }
                if (this.owner != null)
                {
                    this.owner.Invalidate();
                }
            }
        }

        public virtual int CollapsedCount
        {
            get
            {
                if (!this.allowOutlining)
                {
                    return 0;
                }
                return this.outlineList.CollapsedList.Count;
            }
        }

        public virtual int Count
        {
            get
            {
                return this.DisplayCount;
            }
        }

        [Description("Gets or sets an array of chars used to separate words in a text.")]
        public virtual char[] Delimiters
        {
            get
            {
                return this.lines.Delimiters;
            }
            set
            {
                if (this.lines.Delimiters != value)
                {
                    this.lines.Delimiters = value;
                    this.OnDelimitersChanged();
                }
            }
        }

        [Description("Gets or sets \"Delimiters\" as a single string.")]
        public virtual string DelimiterString
        {
            get
            {
                return this.lines.DelimiterString;
            }
            set
            {
                if (this.lines.DelimiterString != value)
                {
                    this.lines.DelimiterString = value;
                    this.OnDelimiterStringChanged();
                }
            }
        }

        public virtual int DisplayCount
        {
            get
            {
                return ((this.lines.Count + this.wrapList.Count) - this.outlineList.CollapsedCount);
            }
        }

        [Description("Gets a value indicating whether the collection is read-only.")]
        public virtual bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public virtual string this[int index]
        {
            get
            {
                string text = string.Empty;
                short[] data = null;
                this.GetStringAndColorData(index, ref text, ref data, false);
                return text;
            }
            set
            {
            }
        }

        public virtual bool LineEnd
        {
            get
            {
                return this.lineEnd;
            }
            set
            {
                if (this.lineEnd != value)
                {
                    this.lineEnd = value;
                    this.OnLineEndChanged();
                }
            }
        }

        [Description("Gets or sets the collection of underlying \"real\" collection of text lines.")]
        public virtual ITextStrings Lines
        {
            get
            {
                return this.lines;
            }
            set
            {
                if (this.lines != value)
                {
                    this.lines = value;
                    this.LinesChanged();
                }
            }
        }

        [Description("Gets or sets a string value that terminates line.")]
        public virtual string LineTerminator
        {
            get
            {
                return this.lines.LineTerminator;
            }
            set
            {
                this.lines.LineTerminator = value;
            }
        }

        public virtual bool Loaded
        {
            get
            {
                if (this.WordWrap)
                {
                    return (this.lastWrapped >= ((this.lines.Count - this.outlineList.CollapsedCount) - 1));
                }
                return true;
            }
            set
            {
                this.ScanToEnd(true);
            }
        }

        [Description("Gets width of the largest line in the \"DisplayString\" object.")]
        public virtual int MaxLineWidth
        {
            get
            {
                if (this.recalcFlag && !this.owner.Scrolling.FixedScrollSize)
                {
                    this.Recalulate();
                }
                return this.maxLineWidth;
            }
        }

        [Description("Gets or sets options representing outlining appearance and behaviour.")]
        public virtual QWhale.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                return this.outlineOptions;
            }
            set
            {
                if (this.outlineOptions != value)
                {
                    this.BeginUpdate();
                    try
                    {
                        this.outlineOptions = value;
                        this.OnOutlineOptionsChanged();
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlDisplayStringsInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Description("Gets or sets the character columns that the cursor will move to each time you press Tab.")]
        public virtual int[] TabStops
        {
            get
            {
                return this.lines.TabStops;
            }
            set
            {
                if (this.lines.TabStops != value)
                {
                    this.lines.TabStops = value;
                    this.OnTabStopsChanged();
                }
            }
        }

        public string Text
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                string lineTerminator = this.lines.LineTerminator;
                for (int i = 0; i < this.DisplayCount; i++)
                {
                    builder.Append(this[i] + lineTerminator);
                }
                if (builder.Length >= 2)
                {
                    builder.Remove(builder.Length - lineTerminator.Length, lineTerminator.Length);
                }
                return builder.ToString();
            }
            set
            {
            }
        }

        [Description("Keeps track of calls to \"BeginUndoUpdate\" and \"EndUndoUpdate\" so that they can be nested.")]
        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [Description("Gets or sets a value indicating whether indent operations insert space characters rather than TAB characters.")]
        public virtual bool UseSpaces
        {
            get
            {
                return this.lines.UseSpaces;
            }
            set
            {
                if (this.lines.UseSpaces != value)
                {
                    this.lines.UseSpaces = value;
                    this.OnUseSpacesChanged();
                }
            }
        }

        [Description("Gets or sets a value indicating whether a \"DisplayStrings\" object automatically wraps words to the beginning of the next line when necessary.")]
        public virtual bool WordWrap
        {
            get
            {
                return this.wordWrap;
            }
            set
            {
                if (this.wordWrap != value)
                {
                    this.wordWrap = value;
                    this.UpdateWordWrap();
                }
            }
        }

        [Description("Gets or sets a value indicating whether a \"DisplayStrings\" object automatically wraps words at margin position.")]
        public virtual bool WrapAtMargin
        {
            get
            {
                return this.wrapAtMargin;
            }
            set
            {
                if (this.wrapAtMargin != value)
                {
                    this.wrapAtMargin = value;
                    if (this.wordWrap)
                    {
                        this.UpdateWordWrap();
                    }
                }
            }
        }

        [Description("Gets position of the wrap margin.")]
        public virtual int WrapMargin
        {
            get
            {
                if (this.wrapAtMargin)
                {
                    return (this.owner.EditMargin.Position * this.owner.Painter.FontWidth);
                }
                return (this.owner.ClientRect.Width - this.owner.Gutter.DisplayWidth);
            }
        }

        internal class DisplayEnumerator : IEnumerator<string>, IDisposable, IEnumerator
        {
            private int currentIndex = -1;
            private IDisplayStrings owner;

            public DisplayEnumerator(IDisplayStrings owner)
            {
                this.owner = owner;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                this.currentIndex++;
                return (this.currentIndex < this.owner.DisplayCount);
            }

            public void Reset()
            {
                this.currentIndex = -1;
            }

            [Description("Gets an object that represents current string.")]
            public string Current
            {
                get
                {
                    if ((this.currentIndex >= 0) && (this.currentIndex < this.owner.DisplayCount))
                    {
                        return this.owner[this.currentIndex];
                    }
                    return null;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
        }

        private class DisplayWrapComparer : IComparer<Point>
        {
            private DisplayStrings.WrapList list;

            public DisplayWrapComparer(DisplayStrings.WrapList list)
            {
                this.list = list;
            }

            public int Compare(Point x, Point y)
            {
                return Math.Max(((x.Y + this.list.CompareIndex) + 1) - y.Y, 0);
            }
        }

        private class LineWrapComparer : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                return (x.Y - y.Y);
            }
        }

        private class RealWrapComparer : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                int num = x.Y - y.Y;
                if (num == 0)
                {
                    return Math.Max(x.X - y.X, 0);
                }
                return Math.Max(num, 0);
            }
        }

        private class WrapComparer : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                int num = x.Y - y.Y;
                if (num == 0)
                {
                    num = x.X - y.X;
                }
                return num;
            }
        }

        internal class WrapList : SortList<Point>
        {
            private IComparer<Point> displayWrapComparer;
            private IComparer<Point> lineWrapComparer;
            private DisplayStrings owner;
            private IComparer<Point> realWrapComparer;
            private IComparer<Point> wrapComparer;

            public WrapList(DisplayStrings owner)
            {
                this.owner = owner;
                this.wrapComparer = new DisplayStrings.WrapComparer();
                this.lineWrapComparer = new DisplayStrings.LineWrapComparer();
                this.realWrapComparer = new DisplayStrings.RealWrapComparer();
                this.displayWrapComparer = new DisplayStrings.DisplayWrapComparer(this);
            }

            public Point AddItem(int line, int ch)
            {
                int num;
                Point point = new Point(ch, line);
                if (base.FindExact(point, out num, this.wrapComparer))
                {
                    base[num] = point;
                    return point;
                }
                base.Insert(num, point);
                return point;
            }

            public void ClearLines(int first, int last)
            {
                int num;
                base.FindLast(new Point(0, last), out num, this.lineWrapComparer);
                for (int i = Math.Min(num, base.Count - 1); i >= 0; i--)
                {
                    Point point = base[i];
                    int y = point.Y;
                    if ((y >= first) && (y <= last))
                    {
                        base.RemoveAt(i);
                    }
                    else if (y < first)
                    {
                        return;
                    }
                }
            }

            public int DisplayLineToLine(int index)
            {
                int num;
                if (base.FindLast(new Point(0, index), out num, this.displayWrapComparer))
                {
                    return (index - (num + 1));
                }
                return index;
            }

            public void GetDisplayPoint(ref Point point, bool lineEnd)
            {
                int num;
                this.owner.EnsureWrapped(point.Y, false);
                if (base.FindLast(point, out num, this.realWrapComparer))
                {
                    Point point2 = base[num];
                    if ((lineEnd && (point2.X == point.X)) && (point2.Y == point.Y))
                    {
                        if (num > 0)
                        {
                            point2 = base[num - 1];
                            if (point2.Y == point.Y)
                            {
                                point.X -= point2.X;
                            }
                        }
                        point.Y += num;
                    }
                    else
                    {
                        if ((point2.Y == point.Y) && (point.X != 0x7fffffff))
                        {
                            point.X -= point2.X;
                        }
                        point.Y += num + 1;
                    }
                }
            }

            private void GetPosAndLen(int index, int idx, ref int p, ref int len)
            {
                if (idx >= 0)
                {
                    Point point = base[idx];
                    if (point.Y == ((index - idx) - 1))
                    {
                        p = point.X;
                    }
                }
                if ((idx + 1) < base.Count)
                {
                    Point point2 = base[idx + 1];
                    if (point2.Y == ((index - idx) - 1))
                    {
                        len = point2.X - p;
                    }
                }
            }

            public void GetRealPoint(ref Point point, bool checkEnd, ref bool lineEnd)
            {
                int num;
                this.owner.EnsureWrapped(point.Y, true);
                int y = point.Y;
                if (base.FindLast(new Point(0, point.Y), out num, this.displayWrapComparer))
                {
                    Point point2 = base[num];
                    point.Y -= num + 1;
                    if ((point2.Y == point.Y) && (point.X != 0x7fffffff))
                    {
                        point.X += point2.X;
                    }
                }
                else
                {
                    num = -1;
                }
                lineEnd = false;
                int p = 0;
                int len = 0x7fffffff;
                this.GetPosAndLen(y, num, ref p, ref len);
                if (len != 0x7fffffff)
                {
                    lineEnd = point.X >= (p + len);
                    if (checkEnd && lineEnd)
                    {
                        point.X = p + len;
                    }
                }
            }

            public void GetWrapBounds(int index, ref int p, ref int len)
            {
                if (base.Count > 0)
                {
                    int num;
                    if (!base.FindLast(new Point(0, index), out num, this.displayWrapComparer))
                    {
                        num = -1;
                    }
                    this.GetPosAndLen(index, num, ref p, ref len);
                }
            }

            public int LineToDisplayLine(int index)
            {
                int num;
                Point point = new Point(0x7fffffff, index);
                if (base.FindLast(point, out num, this.realWrapComparer))
                {
                    return ((index + num) + 1);
                }
                return index;
            }
        }
    }
}

