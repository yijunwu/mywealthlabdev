namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource.Serialization;
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public class TextStrings : ITextStrings, IStringList, IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable, ITextExport, IExport, ITextImport, IImport, ITextSearch, ITabulation, IWordBreak, INotify, IUpdate
    {
        private char[] delimiters;
        private Hashtable delimTable;
        private int firstChanged;
        private int lastChanged;
        private string lineTerminator;
        private IList<IStringItem> list;
        private ISyntaxEdit owner;
        private bool removeTrailingSpaces;
        private ITextSource source;
        private char[] tabArray;
        private StringBuilder tabBuilder;
        private List<int> tabList;
        private int[] tabStops;
        private int updateCount;
        private bool useSpaces;

        private event EventHandler notifyHandler;

        public TextStrings()
        {
            this.lineTerminator = "\r\n";
            this.tabStops = new int[] { EditConsts.DefaultTabStop };
            this.tabArray = new char[] { '\t' };
            this.firstChanged = -1;
            this.lastChanged = -1;
            this.list = new List<IStringItem>();
            this.tabList = new List<int>();
            this.tabBuilder = new StringBuilder();
            this.delimTable = new Hashtable();
            this.InitDelimiters(EditConsts.DefaultDelimiters.ToCharArray());
        }

        public TextStrings(ITextSource source) : this()
        {
            this.source = source;
        }

        public virtual Point AbsolutePositionToTextPoint(int position)
        {
            return AbsolutePositionToTextPoint(this, position, this.lineTerminator);
        }

        public static Point AbsolutePositionToTextPoint(IList<string> list, int position, string lineTerminator)
        {
            Point point = new Point(0, 0);
            int length = lineTerminator.Length;
            int count = list.Count;
            while (position > 0)
            {
                int num = (point.Y < count) ? list[point.Y].Length : 0;
                if (position > num)
                {
                    point.Y++;
                }
                else
                {
                    point.X = position;
                    return point;
                }
                position -= num + length;
            }
            return point;
        }

        public virtual void Add(string item)
        {
            this.list.Add(this.CreateStringItem(item));
            int index = this.list.Count - 1;
            this.Changed(index);
        }

        protected virtual int AddData(string value, short[] colorData)
        {
            IStringItem item = this.CreateStringItem(value);
            if ((colorData != null) && (colorData.Length == value.Length))
            {
                item.TextData = colorData;
            }
            this.list.Add(item);
            int index = this.list.Count - 1;
            this.Changed(index);
            return index;
        }

        public virtual void AddNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Combine(this.notifyHandler, new EventHandler(sender.Notification));
        }

        public virtual void AfterSave()
        {
            if (this.source != null)
            {
                if ((this.source.UndoOptions & UndoOptions.UndoAfterSave) == UndoOptions.None)
                {
                    this.source.ClearUndo();
                    this.source.ClearRedo();
                }
                this.source.Modified = false;
            }
        }

        public virtual void Assign(ITextStrings source)
        {
            this.BeginUpdate();
            try
            {
                this.Clear();
                foreach (string str in source)
                {
                    this.Add(str);
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual int BeginUpdate()
        {
            if (this.updateCount == 0)
            {
                this.firstChanged = -1;
                this.lastChanged = -1;
            }
            this.updateCount++;
            return this.updateCount;
        }

        public virtual void Changed(int index)
        {
            this.Changed(index, index);
        }

        public virtual void Changed(int first, int last)
        {
            if (this.firstChanged == -1)
            {
                this.firstChanged = first;
            }
            else
            {
                this.firstChanged = Math.Min(this.firstChanged, first);
            }
            this.lastChanged = Math.Max(this.lastChanged, last);
            this.Update();
        }

        public virtual void Clear()
        {
            this.list.Clear();
            this.Changed(0, 0x7fffffff);
        }

        public virtual bool Contains(string item)
        {
            foreach (IStringItem item2 in this.list)
            {
                if (item2.String == item)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void CopyTo(string[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < Math.Min(this.Count + arrayIndex, array.Length); i++)
            {
                array[i] = this[i - arrayIndex];
            }
        }

        public virtual IStringItem CreateStringItem(string s)
        {
            if (this.source != null)
            {
                return this.source.CreateStringItem(s);
            }
            return new StringItem(s);
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual int EnableUpdate()
        {
            this.updateCount--;
            return this.updateCount;
        }

        public virtual int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
            return this.updateCount;
        }

        ~TextStrings()
        {
            this.delimTable.Clear();
        }

        public virtual bool Find(string s, SearchOptions options, Regex expression, ref Point position, out int len, out Match match)
        {
            return Find(this, this.delimTable, s, options, expression, ref position, out len, out match, this.lineTerminator);
        }

        public static bool Find(IStringList list, Hashtable delimTable, string s, SearchOptions options, Regex expression, ref Point position, out int len, out Match match, string lineTerminator)
        {
            match = null;
            len = (s != null) ? s.Length : 0;
            if ((s == null) || (s == string.Empty))
            {
                return false;
            }
            bool flag = ((options & SearchOptions.CaseSensitive) == SearchOptions.None) && (expression == null);
            if (flag)
            {
                s = s.ToUpper();
            }
            string input = null;
            int wordStart = 0;
            int wordEnd = 0;
            if ((options & SearchOptions.BackwardSearch) == SearchOptions.None)
            {
                if ((expression == null) || ((expression.Options & RegexOptions.Multiline) == RegexOptions.None))
                {
                Label_042B:
                    if (position.Y < list.Count)
                    {
                        input = list[position.Y];
                        if ((position.X < input.Length) || (((input == string.Empty) && (position.X == 0)) && (expression != null)))
                        {
                            if (flag)
                            {
                                input = input.ToUpper();
                            }
                            position.X = Math.Min(position.X, input.Length - 1);
                            if (expression != null)
                            {
                                match = (input == string.Empty) ? expression.Match(input) : expression.Match(input, position.X);
                                if (match.Success)
                                {
                                    len = match.Length;
                                    position.X = match.Index;
                                }
                                else
                                {
                                    position.X = -1;
                                }
                            }
                            else if (input != string.Empty)
                            {
                                position.X = input.IndexOf(s, position.X);
                            }
                            else
                            {
                                position.X = -1;
                            }
                            if (((position.X >= 0) && ((options & SearchOptions.WholeWordsOnly) != SearchOptions.None)) && !IsWholeWord(delimTable, input, position.X, len, ref wordStart, ref wordEnd))
                            {
                                position.X = (position.X == wordEnd) ? (position.X + 1) : wordEnd;
                                goto Label_042B;
                            }
                            if (position.X >= 0)
                            {
                                return true;
                            }
                        }
                        position.Y++;
                        position.X = 0;
                        goto Label_042B;
                    }
                    goto Label_043D;
                }
                int startat = TextPointToAbsolutePosition(list, position, lineTerminator);
                string text = list.Text;
                if (startat < text.Length)
                {
                    match = expression.Match(text, startat);
                    if (match.Success && (match.Length > 0))
                    {
                        len = match.Length;
                        position = AbsolutePositionToTextPoint(list, match.Index, lineTerminator);
                        return true;
                    }
                }
                return false;
            }
            if ((expression != null) && ((expression.Options & RegexOptions.Multiline) != RegexOptions.None))
            {
                int num3 = TextPointToAbsolutePosition(list, position, lineTerminator);
                string str2 = list.Text;
                match = expression.Match(str2, Math.Min(num3, str2.Length));
                if (match.Success && (match.Length > 0))
                {
                    len = match.Length;
                    position = AbsolutePositionToTextPoint(list, match.Index, lineTerminator);
                    return true;
                }
                return false;
            }
            position.Y = Math.Min(position.Y, list.Count - 1);
            while (position.Y >= 0)
            {
                input = list[position.Y];
                if ((position.X > 0) || (((input == string.Empty) && (position.X == 0)) && (expression != null)))
                {
                    if (flag)
                    {
                        input = input.ToUpper();
                    }
                    if (expression != null)
                    {
                        match = (input == string.Empty) ? expression.Match(input) : expression.Match(input, Math.Min(position.X, input.Length));
                        if (match.Success)
                        {
                            len = match.Length;
                            position.X = match.Index;
                        }
                        else
                        {
                            position.X = -1;
                        }
                    }
                    else if (input != string.Empty)
                    {
                        position.X = input.LastIndexOf(s, Math.Min((int) (position.X - 1), (int) (input.Length - 1)));
                    }
                    else
                    {
                        position.X = -1;
                    }
                    if (((position.X >= 0) && ((options & SearchOptions.WholeWordsOnly) != SearchOptions.None)) && !IsWholeWord(delimTable, input, position.X, len, ref wordStart, ref wordEnd))
                    {
                        position.X = wordStart - 1;
                        continue;
                    }
                    if (position.X >= 0)
                    {
                        return true;
                    }
                }
                position.Y--;
                if (position.Y >= 0)
                {
                    position.X = list[position.Y].Length;
                }
            }
        Label_043D:
            return false;
        }

        public virtual char GetCharAt(Point position)
        {
            return this.GetCharAt(position.X, position.Y);
        }

        public virtual char GetCharAt(int x, int y)
        {
            IStringItem item = this.GetItem(y);
            if (((item != null) && (x >= 0)) && (x < item.String.Length))
            {
                return item.String[x];
            }
            return '\0';
        }

        private short[] GetColorData(string s, int left, int len)
        {
            if (left >= s.Length)
            {
                return null;
            }
            if ((left + len) < s.Length)
            {
                s = s.Substring(left, len);
            }
            else
            {
                s = s.Substring(left);
            }
            short[] numArray = new short[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                numArray[i] = (short) s[i];
            }
            return numArray;
        }

        public virtual IEnumerator<string> GetEnumerator()
        {
            return ((List<IStringItem>) this.list).ConvertAll<string>(new Converter<IStringItem, string>(this.StringItemToString)).GetEnumerator();
        }

        public virtual string GetIndentString(int count, int pos)
        {
            return this.GetIndentString(count, pos, this.useSpaces);
        }

        public virtual string GetIndentString(int count, int p, bool useSpaces)
        {
            if (useSpaces)
            {
                return new string(' ', count);
            }
            int num = 0;
            int num2 = 0;
            while (num < count)
            {
                int num3 = this.GetTabStop(p) - p;
                if ((num + num3) > count)
                {
                    break;
                }
                num += num3;
                p += num3;
                num2++;
            }
            return (new string('\t', num2) + new string(' ', count - num));
        }

        public virtual IStringItem GetItem(int index)
        {
            if ((index >= 0) && (index < this.Count))
            {
                return this.list[index];
            }
            return this.CreateStringItem(string.Empty);
        }

        public virtual int GetLength(int index)
        {
            if ((index >= 0) && (index < this.Count))
            {
                return this.list[index].String.Length;
            }
            return 0;
        }

        public virtual int GetLexStyle(Point position)
        {
            short[] textData = this.list[position.Y].TextData;
            if (((textData != null) && (position.X >= 0)) && (position.X < textData.Length))
            {
                return (textData[position.X] - 1);
            }
            return -1;
        }

        public virtual int GetPrevTabStop(int pos)
        {
            this.GetTabStop(pos);
            int num = this.tabList[pos];
            while ((pos > 0) && (this.tabList[pos] == num))
            {
                pos--;
            }
            if ((num >= 0) && (pos > 0))
            {
                return this.tabList[pos];
            }
            return 0;
        }

        private int GetStop(int pos)
        {
            int length = this.tabStops.Length;
            if (length == 0)
            {
                return EditConsts.DefaultTabStop;
            }
            int num2 = pos - this.tabStops[length - 1];
            int index = 0;
            if (num2 >= 0)
            {
                if (length == 1)
                {
                    index = this.tabStops[0];
                }
                else
                {
                    index = this.tabStops[length - 1] - this.tabStops[length - 2];
                }
                return (this.tabStops[length - 1] + (((num2 / index) + 1) * index));
            }
            index = 0;
            while (pos >= this.tabStops[index])
            {
                index++;
            }
            return this.tabStops[index];
        }

        public virtual int GetTabStop(int pos)
        {
            if (pos == 0x7fffffff)
            {
                return pos;
            }
            for (int i = this.tabList.Count; i <= pos; i++)
            {
                this.tabList.Add(this.GetStop(i));
            }
            return this.tabList[pos];
        }

        public virtual string GetTabString(string s)
        {
            short[] data = null;
            this.GetTabString(ref s, ref data, false, null);
            return s;
        }

        public virtual void GetTabString(string s, ITextUndoList operations)
        {
            short[] data = null;
            this.GetTabString(ref s, ref data, false, operations);
        }

        public virtual void GetTabString(ref string str, ref short[] data, bool needData, ITextUndoList operations)
        {
            if (((str != null) && (str != string.Empty)) && (str.IndexOf('\t') >= 0))
            {
                if (operations == null)
                {
                    this.tabBuilder.Length = 0;
                }
                int pos = 0;
                int start = 0;
                int count = 0;
                bool flag = true;
                string[] strArray = str.Split(this.tabArray);
                foreach (string str2 in strArray)
                {
                    if (flag)
                    {
                        if (operations == null)
                        {
                            this.tabBuilder.Append(str2);
                        }
                        flag = false;
                        pos = str2.Length;
                        start = pos;
                    }
                    else
                    {
                        count = this.GetTabStop(pos) - pos;
                        if (operations != null)
                        {
                            operations.Add(new TextUndo(start, 1, new string(' ', count)));
                        }
                        else
                        {
                            this.tabBuilder.Append(new string(' ', count));
                            this.tabBuilder.Append(str2);
                        }
                        start += str2.Length + 1;
                        pos += str2.Length + count;
                    }
                }
                if (operations == null)
                {
                    str = this.tabBuilder.ToString();
                }
                if (needData)
                {
                    short[] destinationArray = new short[str.Length];
                    pos = 0;
                    int index = 0;
                    count = 0;
                    flag = true;
                    foreach (string str3 in strArray)
                    {
                        if (flag)
                        {
                            flag = false;
                            pos = str3.Length;
                            index = pos;
                            if (pos != 0)
                            {
                                Array.Copy(data, 0, destinationArray, 0, pos);
                            }
                        }
                        else
                        {
                            count = this.GetTabStop(pos) - pos;
                            for (int i = pos; i < (pos + count); i++)
                            {
                                destinationArray[i] = data[index];
                            }
                            StringItem.SetTextStyle(ref destinationArray, pos + (count / 2), 1, TextStyle.Tabulation);
                            int length = str3.Length;
                            Array.Copy(data, index + 1, destinationArray, pos + count, length);
                            pos += length + count;
                            index += length + 1;
                        }
                    }
                    data = destinationArray;
                }
            }
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
            return this.GetWord(s, pos, out left, out right, null);
        }

        public virtual bool GetWord(string s, int pos, out int left, out int right, Hashtable delims)
        {
            int length = s.Length;
            left = 0;
            right = 0;
            if (!(s != string.Empty) || (pos > length))
            {
                return false;
            }
            if (pos == length)
            {
                pos--;
            }
            if (this.IsDelimiter(delims, s, pos))
            {
                left = pos;
                while ((left > 0) && this.IsDelimiter(delims, s, left - 1))
                {
                    left--;
                }
                right = pos;
                while ((right < (length - 1)) && this.IsDelimiter(delims, s, right + 1))
                {
                    right++;
                }
            }
            else
            {
                left = pos;
                while ((left > 0) && !this.IsDelimiter(delims, s, left - 1))
                {
                    left--;
                }
                right = pos;
                while ((right < (length - 1)) && !this.IsDelimiter(delims, s, right + 1))
                {
                    right++;
                }
            }
            return true;
        }

        public virtual int IndexOf(string item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.list[i].String == item)
                {
                    return i;
                }
            }
            return -1;
        }

        private void InitDelimiters(char[] delims)
        {
            this.delimiters = new char[(((delims != null) ? delims.Length : 0) + 0x20) + 1];
            for (int i = 0; i <= 0x20; i++)
            {
                this.delimiters[i] = (char) i;
            }
            if (delims != null)
            {
                for (int j = 0; j < delims.Length; j++)
                {
                    this.delimiters[(0x20 + j) + 1] = delims[j];
                }
            }
            this.UpdateDelimTable();
        }

        public virtual void Insert(int index, string item)
        {
            this.list.Insert(index, this.CreateStringItem(item));
            this.Changed(index, 0x7fffffff);
        }

        public virtual bool IsDelimiter(char ch)
        {
            return this.delimTable.ContainsKey(ch);
        }

        public virtual bool IsDelimiter(int index, int pos)
        {
            return this.IsDelimiter(this[index], pos);
        }

        public virtual bool IsDelimiter(string s, int pos)
        {
            return this.delimTable.ContainsKey(s[pos]);
        }

        protected bool IsDelimiter(Hashtable delims, string s, int pos)
        {
            char key = s[pos];
            return (this.delimTable.ContainsKey(key) || ((delims != null) && delims.Contains(key)));
        }

        protected static bool IsDelimiter(string s, int pos, Hashtable delimTable)
        {
            char key = s[pos];
            return delimTable.ContainsKey(key);
        }

        protected static bool IsWholeWord(Hashtable delimTable, string s, int start, int len, ref int wordStart, ref int wordEnd)
        {
            wordStart = start;
            wordEnd = start + len;
            int length = s.Length;
            if ((wordStart <= 0) || !IsDelimiter(s, wordStart, delimTable))
            {
                while ((wordStart > 0) && !IsDelimiter(s, wordStart - 1, delimTable))
                {
                    wordStart--;
                }
            }
            if (((wordEnd >= (length - 1)) || (wordEnd <= 0)) || !IsDelimiter(s, wordEnd - 1, delimTable))
            {
                while ((wordEnd < length) && !IsDelimiter(s, wordEnd, delimTable))
                {
                    wordEnd++;
                }
            }
            return ((wordStart == start) && (len == (wordEnd - wordStart)));
        }

        public virtual bool LoadFile(string fileName)
        {
            return this.LoadFile(fileName, null, null);
        }

        public bool LoadFile(string fileName, IStringImport importer)
        {
            return this.LoadFile(fileName, importer, null);
        }

        public bool LoadFile(string fileName, Encoding encoding)
        {
            return this.LoadFile(fileName, null, encoding);
        }

        public bool LoadFile(string fileName, IStringImport importer, Encoding encoding)
        {
            bool flag = true;
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    return this.LoadStream(stream, importer, encoding);
                }
                finally
                {
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
                flag = false;
            }
            return flag;
        }

        public virtual bool LoadStream(Stream stream)
        {
            return this.LoadStream(stream, null, null);
        }

        public virtual bool LoadStream(TextReader reader)
        {
            return this.LoadStream(reader, null);
        }

        public bool LoadStream(Stream stream, IStringImport importer)
        {
            return this.LoadStream(stream, importer, null);
        }

        public virtual bool LoadStream(Stream stream, Encoding encoding)
        {
            bool flag;
            TextReader reader = (encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream);
            try
            {
                flag = this.LoadStream(reader);
            }
            finally
            {
                reader.Close();
            }
            return flag;
        }

        public virtual bool LoadStream(TextReader reader, IStringImport importer)
        {
            if (importer != null)
            {
                object userData = (this.source != null) ? this.source.ActiveEdit : null;
                importer.BeginRead(reader, userData);
                try
                {
                    return importer.Read();
                }
                finally
                {
                    importer.EndRead();
                }
            }
            if (this.source != null)
            {
                this.source.BeginUpdate(UpdateReason.Other);
            }
            this.BeginUpdate();
            try
            {
                string str;
                this.Clear();
                while ((str = reader.ReadLine()) != null)
                {
                    this.Add(this.RemoveSpaces(str));
                }
                if (this.source != null)
                {
                    this.source.State |= NotifyState.CountChanged;
                }
                this.Changed(0, 0x7fffffff);
            }
            finally
            {
                this.EndUpdate();
                if (this.source != null)
                {
                    this.source.EndUpdate();
                }
            }
            return true;
        }

        public bool LoadStream(Stream stream, IStringImport importer, Encoding encoding)
        {
            bool flag;
            StreamReader reader = (encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream);
            try
            {
                flag = this.LoadStream(reader, importer);
            }
            finally
            {
                reader.Close();
            }
            return flag;
        }

        public virtual void Notify()
        {
            if (this.notifyHandler != null)
            {
                this.notifyHandler(this, EventArgs.Empty);
            }
            this.firstChanged = -1;
            this.lastChanged = -1;
        }

        protected virtual void OnDelimitersChanged()
        {
            this.UpdateDelimTable();
        }

        protected virtual void OnLineTerminatorChanged()
        {
        }

        protected virtual void OnOwnerChanged()
        {
        }

        protected virtual void OnRemoveTrailingSpacesChanged()
        {
        }

        protected virtual void OnUseSpacesChanged()
        {
        }

        public virtual int PosToTabPos(string s, int pos)
        {
            return this.PosToTabPos(s, pos, false);
        }

        public virtual int PosToTabPos(string s, int pos, bool tabEnd)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            string[] strArray = s.Split(this.tabArray);
            for (int i = 0; i < (strArray.Length - 1); i++)
            {
                int length = strArray[i].Length;
                num2 = (this.GetTabStop(num + length) - num) - length;
                num += length + num2;
                if (pos < num)
                {
                    if (((num - pos) < num2) && !tabEnd)
                    {
                        num3 += num2 - (num - pos);
                    }
                    break;
                }
                num3 += num2 - 1;
            }
            return (pos - num3);
        }

        public virtual bool Remove(string item)
        {
            int index = this.IndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }
            return false;
        }

        public virtual void RemoveAt(int index)
        {
            if ((index >= 0) && (index < this.Count))
            {
                this.list.RemoveAt(index);
                if (index < this.Count)
                {
                    IStringItem item = this.list[index];
                    item.State = (ItemState) ((byte) (((int) item.State) & 0xfe));
                }
                this.Changed(index, 0x7fffffff);
            }
        }

        public virtual void RemoveNotifier(INotifier sender)
        {
            this.notifyHandler = (EventHandler) Delegate.Remove(this.notifyHandler, new EventHandler(sender.Notification));
        }

        protected string RemoveSpaces(string s)
        {
            if (!this.removeTrailingSpaces)
            {
                return s;
            }
            return s.TrimEnd(new char[0]);
        }

        public virtual void ResetDelimiters()
        {
            this.Delimiters = EditConsts.DefaultDelimiters.ToCharArray();
        }

        public virtual void ResetTabStops()
        {
            this.TabStops = new int[] { EditConsts.DefaultTabStop };
        }

        public virtual void ResetUseSpaces()
        {
            this.UseSpaces = false;
        }

        public virtual bool SaveFile(string fileName)
        {
            return this.SaveFile(fileName, null, null);
        }

        public virtual bool SaveFile(string fileName, IStringExport exporter)
        {
            return this.SaveFile(fileName, exporter, null);
        }

        public virtual bool SaveFile(string fileName, Encoding encoding)
        {
            return this.SaveFile(fileName, null, encoding);
        }

        public virtual bool SaveFile(string fileName, IStringExport exporter, Encoding encoding)
        {
            bool flag = true;
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                try
                {
                    this.SaveStream(stream, exporter);
                }
                finally
                {
                    stream.Close();
                }
                this.AfterSave();
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
                flag = false;
            }
            return flag;
        }

        public virtual bool SaveStream(Stream stream)
        {
            return this.SaveStream(stream, null, null);
        }

        public virtual bool SaveStream(TextWriter writer)
        {
            return this.SaveStream(writer, null);
        }

        public virtual bool SaveStream(Stream stream, IStringExport exporter)
        {
            return this.SaveStream(stream, exporter, null);
        }

        public virtual bool SaveStream(Stream stream, Encoding encoding)
        {
            return this.SaveStream(stream, null, encoding);
        }

        public virtual bool SaveStream(TextWriter writer, IStringExport exporter)
        {
            if (exporter != null)
            {
                object userData = null;
                if (this.source != null)
                {
                    this.source.ParseToString(this.Count - 1);
                    userData = this.source.ActiveEdit;
                }
                else
                {
                    userData = this.owner;
                }
                exporter.BeginWrite(writer, userData);
                try
                {
                    if (!exporter.Write())
                    {
                        foreach (IStringItem item in this.list)
                        {
                            exporter.WriteLine(item);
                        }
                    }
                }
                finally
                {
                    exporter.EndWrite();
                }
            }
            else
            {
                foreach (IStringItem item2 in this.list)
                {
                    writer.WriteLine(item2.String);
                }
            }
            return true;
        }

        public virtual bool SaveStream(Stream stream, IStringExport exporter, Encoding encoding)
        {
            TextWriter writer = (encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream);
            try
            {
                this.SaveStream(writer, exporter);
            }
            catch (Exception exception)
            {
                ErrorHandler.Error(exception);
                return false;
            }
            finally
            {
                writer.Close();
            }
            return true;
        }

        public virtual void SetTextAndData(string text, string data)
        {
            if (this.source != null)
            {
                this.source.BeginUpdate(UpdateReason.Other);
            }
            this.BeginUpdate();
            try
            {
                this.Clear();
                if ((text != null) && (text != string.Empty))
                {
                    string str;
                    StringReader reader = new StringReader(text);
                    int left = 0;
                    while ((str = reader.ReadLine()) != null)
                    {
                        if (data != null)
                        {
                            string str2 = this.RemoveSpaces(str);
                            this.AddData(str2, this.GetColorData(data, left, str2.Length));
                            left += str.Length;
                            if (((left < (text.Length - 1)) && (text[left] == '\r')) && (text[left + 1] == '\n'))
                            {
                                left += 2;
                            }
                            else if ((left < text.Length) && ((text[left] == '\r') || (text[left] == '\n')))
                            {
                                left++;
                            }
                        }
                        else
                        {
                            this.Add(this.RemoveSpaces(str));
                        }
                    }
                    switch (text[text.Length - 1])
                    {
                        case '\r':
                        case '\n':
                            this.Add(string.Empty);
                            break;
                    }
                }
                if (this.source != null)
                {
                    this.source.State |= NotifyState.CountChanged;
                    if (data != null)
                    {
                        this.source.SetLastParsed(this.Count);
                    }
                }
                this.Changed(0, 0x7fffffff);
            }
            finally
            {
                this.EndUpdate();
                if (this.source != null)
                {
                    this.source.EndUpdate();
                }
            }
        }

        protected string StringItemToString(IStringItem item)
        {
            return item.String;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public virtual int TabPosToPos(string s, int pos)
        {
            if (pos >= 0)
            {
                int num = pos - s.Length;
                pos = this.GetTabString(s.Substring(0, Math.Min(pos, s.Length))).Length;
                if (num > 0)
                {
                    return (pos + num);
                }
            }
            return pos;
        }

        public virtual int TextPointToAbsolutePosition(Point position)
        {
            return TextPointToAbsolutePosition(this, position, this.lineTerminator);
        }

        public static int TextPointToAbsolutePosition(IList<string> list, Point position, string lineTerminator)
        {
            int num = 0;
            int length = lineTerminator.Length;
            int count = list.Count;
            for (int i = 0; i < position.Y; i++)
            {
                num += ((i < count) ? list[i].Length : 0) + length;
            }
            if (position.Y < count)
            {
                num += Math.Min(position.X, list[position.Y].Length);
            }
            return num;
        }

        public virtual void Update()
        {
            if (this.updateCount == 0)
            {
                this.Notify();
            }
        }

        private void UpdateDelimTable()
        {
            this.delimTable.Clear();
            foreach (char ch in this.delimiters)
            {
                this.delimTable[ch] = ch;
            }
        }

        [Description("Represents number of strings in the collection.")]
        public virtual int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        [Description("Gets or sets an array of characters used as delimiters between words in the text.")]
        public virtual char[] Delimiters
        {
            get
            {
                return this.delimiters;
            }
            set
            {
                this.delimiters = new char[value.Length];
                Array.Copy(value, this.delimiters, value.Length);
                this.OnDelimitersChanged();
            }
        }

        [Description("Gets or sets \"Delimiters\" as a single string.")]
        public virtual string DelimiterString
        {
            get
            {
                string str = string.Empty;
                foreach (char ch in this.Delimiters)
                {
                    if (ch > ' ')
                    {
                        str = str + ch;
                    }
                }
                return str;
            }
            set
            {
                this.InitDelimiters(value.ToCharArray());
            }
        }

        public virtual Hashtable DelimTable
        {
            get
            {
                return this.delimTable;
            }
        }

        [Description("Represents number of the first changed line.")]
        public virtual int FirstChanged
        {
            get
            {
                return this.firstChanged;
            }
        }

        [Description("Gets a value indicating whether the collection is read-only.")]
        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual string this[int index]
        {
            get
            {
                if ((index >= 0) && (index < this.Count))
                {
                    return this.list[index].String;
                }
                return string.Empty;
            }
            set
            {
                if ((index >= 0) && (index < this.list.Count))
                {
                    IStringItem item = this.list[index];
                    item.String = value;
                    item.State = (ItemState) ((byte) (((int) item.State) & 0xfe));
                    this.Changed(index);
                }
            }
        }

        [Description("Represents number of the last changed line.")]
        public virtual int LastChanged
        {
            get
            {
                return this.lastChanged;
            }
        }

        [DefaultValue("\r\n"), Description("Gets or sets a string value that terminates line.")]
        public virtual string LineTerminator
        {
            get
            {
                return this.lineTerminator;
            }
            set
            {
                if (this.lineTerminator != value)
                {
                    this.lineTerminator = value;
                    this.OnLineTerminatorChanged();
                }
            }
        }

        public virtual ISyntaxEdit Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                if (this.owner != value)
                {
                    this.owner = value;
                    this.OnOwnerChanged();
                }
            }
        }

        [Description("Gets or set a boolean value thet indicates whether \"SyntaxStrings\" should remove trailing spaces from the end of each its strings.")]
        public virtual bool RemoveTrailingSpaces
        {
            get
            {
                return this.removeTrailingSpaces;
            }
            set
            {
                if (this.removeTrailingSpaces != value)
                {
                    this.removeTrailingSpaces = value;
                    this.OnRemoveTrailingSpacesChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlTextStringsInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Description("Gets the text source owning the \"SyntaxStrings\".")]
        public virtual ITextSource Source
        {
            get
            {
                return this.source;
            }
        }

        [Description("Gets or sets the character columns that the cursor will move to each time you press Tab.")]
        public virtual int[] TabStops
        {
            get
            {
                return this.tabStops;
            }
            set
            {
                this.tabStops = new int[value.Length];
                Array.Copy(value, this.tabStops, value.Length);
                this.tabList.Clear();
                int num = 0;
                foreach (int num2 in this.tabStops)
                {
                    if (num2 <= num)
                    {
                        ErrorHandler.Error(new Exception(string.Format(StringConsts.InvalidTabStop, value.ToString())));
                    }
                    num = num2;
                }
            }
        }

        [Description("Gets or sets the strings in the \"ITextStrings\" as a single string with the individual strings delimited by carriage returns.")]
        public virtual string Text
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (IStringItem item in this.list)
                {
                    builder.Append(item.String + this.lineTerminator);
                }
                if (builder.Length >= 2)
                {
                    builder.Remove(builder.Length - this.lineTerminator.Length, this.lineTerminator.Length);
                }
                return builder.ToString();
            }
            set
            {
                this.SetTextAndData(value, null);
            }
        }

        [Description("Keeps track of calls to \"BeginUpdate\" and \"EndUpdate\" so that they can be nested.")]
        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [Description("Gets or sets a value indicating whether indent or TAB operations insert space characters rather than TAB characters.")]
        public virtual bool UseSpaces
        {
            get
            {
                return this.useSpaces;
            }
            set
            {
                if (this.useSpaces != value)
                {
                    this.useSpaces = value;
                    this.OnUseSpacesChanged();
                }
            }
        }
    }
}

