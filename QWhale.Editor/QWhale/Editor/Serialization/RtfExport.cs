namespace QWhale.Editor.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text;

    public class RtfExport : FmtExport
    {
        private Hashtable colorTable = new Hashtable();
        private bool needSpace;
        private StringBuilder rBuilder = new StringBuilder();
        private const string rtfB = @"\b";
        private const string rtfB0 = @"\b0";
        private const string rtfBlue = @"\blue";
        private const string rtfCf = @"\cf";
        private const string rtfColorTbl = @"\colortbl ";
        private const string rtfF1 = @"\f1 ";
        private const string rtfFontTbl = @"\fonttbl ";
        private const string rtfGreen = @"\green";
        private const string rtfGroupBegin = "{";
        private const string rtfGroupEnd = "}";
        private const string rtfHeader = @"{\rtf1\ansi ";
        private const string rtfHighlight = @"\highlight";
        private const string rtfI = @"\i";
        private const string rtfI0 = @"\i0";
        private const string rtfLine = @"\line";
        private const string rtfRed = @"\red";
        private const string rtfSize = @"\fs";
        private const string rtfStrike = @"\strike";
        private const string rtfStrike0 = @"\strike0";
        private const string rtfTab = @"\tab";
        private const string rtfUl = @"\ul";
        private const string rtfUlnone = @"\ulnone";
        private StringBuilder sBuilder = new StringBuilder();

        private int AddColor(Color color)
        {
            object obj2 = this.colorTable[color];
            if (obj2 != null)
            {
                return (int) obj2;
            }
            int count = this.colorTable.Count;
            this.colorTable.Add(color, count);
            return count;
        }

        protected override void AddFontStyle(FontStyle style)
        {
            switch (style)
            {
                case FontStyle.Bold:
                    this.WriteTag(@"\b");
                    return;

                case FontStyle.Italic:
                    this.WriteTag(@"\i");
                    return;

                case (FontStyle.Italic | FontStyle.Bold):
                    break;

                case FontStyle.Underline:
                    this.WriteTag(@"\ul");
                    return;

                case FontStyle.Strikeout:
                    this.WriteTag(@"\strike");
                    break;

                default:
                    return;
            }
        }

        public override void BeginWrite(TextWriter writer, object userData)
        {
            base.BeginWrite(writer, userData);
            this.AddColor(base.foreColor);
        }

        protected string ColorToRtf(Color color)
        {
            return (@"\red" + color.R.ToString() + @"\green" + color.G.ToString() + @"\blue" + color.B.ToString());
        }

        public override void EndWrite()
        {
            this.WriteHeader();
            this.WriteFontTable();
            this.WriteColorTable();
            base.writer.Write(@"\f1 ");
            if (base.edit != null)
            {
                base.writer.Write(@"\fs" + ((((int) base.edit.Font.Size) * 2)).ToString(), ' ');
            }
            base.writer.Write(this.sBuilder.ToString());
            base.writer.Write("}");
        }

        protected override void RemoveFontStyle(FontStyle style)
        {
            switch (style)
            {
                case FontStyle.Bold:
                    this.WriteTag(@"\b0");
                    return;

                case FontStyle.Italic:
                    this.WriteTag(@"\i0");
                    return;

                case (FontStyle.Italic | FontStyle.Bold):
                    break;

                case FontStyle.Underline:
                    this.WriteTag(@"\ulnone");
                    return;

                case FontStyle.Strikeout:
                    this.WriteTag(@"\strike0");
                    break;

                default:
                    return;
            }
        }

        protected override void StartLine()
        {
            if (!base.firstLine)
            {
                this.WriteString(string.Empty, true);
                this.WriteTag(@"\line");
            }
        }

        private string StringToRtf(string str, ref bool firstTab, ref bool lastTab)
        {
            if ((str == string.Empty) || (str == null))
            {
                return str;
            }
            this.rBuilder.Length = 0;
            firstTab = str[0] == '\t';
            lastTab = false;
            foreach (char ch in str)
            {
                switch (ch)
                {
                    case '{':
                    case '}':
                    case '\\':
                    {
                        lastTab = false;
                        this.rBuilder.Append(@"\" + ch);
                        continue;
                    }
                    case '\t':
                    {
                        lastTab = true;
                        this.rBuilder.Append(@"\tab");
                        continue;
                    }
                }
                if (lastTab)
                {
                    this.rBuilder.Append(' ');
                }
                lastTab = false;
                this.rBuilder.Append(ch);
            }
            return this.rBuilder.ToString();
        }

        protected override void WriteBackColor(Color backColor)
        {
            this.WriteTag(@"\highlight" + this.AddColor(backColor));
        }

        private void WriteColorTable()
        {
            if (this.colorTable.Count > 0)
            {
                List<DictionaryEntry> list = new List<DictionaryEntry>();
                base.writer.Write(@"{\colortbl ");
                IDictionaryEnumerator enumerator = this.colorTable.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Entry);
                }
                list.Sort(new ColorComparer());
                foreach (DictionaryEntry entry in list)
                {
                    base.writer.Write(this.ColorToRtf((Color) entry.Key) + ";");
                }
                base.writer.WriteLine("}");
            }
        }

        private void WriteFontTable()
        {
            base.writer.Write(@"{\fonttbl ");
            if (base.edit != null)
            {
                base.writer.Write(@"{\f1 " + base.edit.Font.Name + ";}");
            }
            base.writer.WriteLine("}");
        }

        protected override void WriteForeColor(Color foreColor)
        {
            this.WriteTag(@"\cf" + this.AddColor(foreColor));
        }

        private void WriteHeader()
        {
            base.writer.WriteLine(@"{\rtf1\ansi ");
        }

        private void WriteString(string str)
        {
            if (str != string.Empty)
            {
                if (this.needSpace)
                {
                    this.sBuilder.Append(' ');
                }
                this.needSpace = false;
                this.sBuilder.Append(str);
            }
        }

        private void WriteString(string str, bool newLine)
        {
            this.WriteString(str);
            if (newLine)
            {
                this.sBuilder.Append("\r\n");
                this.needSpace = false;
            }
        }

        private void WriteTag(string tag)
        {
            this.sBuilder.Append(tag);
            this.needSpace = true;
        }

        protected override void WriteText(int pos, string text)
        {
            bool firstTab = false;
            bool lastTab = false;
            string str = this.StringToRtf(text, ref firstTab, ref lastTab);
            if (firstTab)
            {
                this.needSpace = false;
            }
            this.WriteString(str);
            if (lastTab)
            {
                this.needSpace = true;
            }
        }

        private class ColorComparer : IComparer<DictionaryEntry>
        {
            public int Compare(DictionaryEntry x, DictionaryEntry y)
            {
                return (((int) x.Value) - ((int) y.Value));
            }
        }
    }
}

