namespace QWhale.Editor.TextSource
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    public interface ITextSearch
    {
        bool Find(string s, SearchOptions options, Regex expression, ref Point position, out int len, out Match match);
    }
}

