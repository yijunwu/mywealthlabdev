namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public interface ITextStrings : IStringList, IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable, ITextExport, IExport, ITextImport, IImport, ITextSearch, ITabulation, IWordBreak, INotify, IUpdate
    {
        Point AbsolutePositionToTextPoint(int position);
        void AfterSave();
        void Assign(ITextStrings source);
        void Changed(int index);
        void Changed(int first, int last);
        char GetCharAt(Point position);
        char GetCharAt(int x, int y);
        IStringItem GetItem(int index);
        int GetLength(int index);
        int GetLexStyle(Point position);
        void GetTabString(ref string str, ref short[] data, bool needData, ITextUndoList operations);
        void SetTextAndData(string text, string data);
        int TextPointToAbsolutePosition(Point position);

        Hashtable DelimTable { get; }

        int FirstChanged { get; }

        int LastChanged { get; }

        ISyntaxEdit Owner { get; set; }

        bool RemoveTrailingSpaces { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        ITextSource Source { get; }
    }
}

