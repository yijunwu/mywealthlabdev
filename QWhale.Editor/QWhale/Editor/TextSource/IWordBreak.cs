namespace QWhale.Editor.TextSource
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public interface IWordBreak
    {
        string GetTextAt(Point position);
        string GetTextAt(int pos, int line);
        bool GetWord(int index, int pos, out int left, out int right);
        bool GetWord(string s, int pos, out int left, out int right);
        bool GetWord(string s, int pos, out int left, out int right, Hashtable delims);
        bool IsDelimiter(char ch);
        bool IsDelimiter(int index, int pos);
        bool IsDelimiter(string s, int pos);
        void ResetDelimiters();

        char[] Delimiters { get; set; }

        string DelimiterString { get; set; }
    }
}

