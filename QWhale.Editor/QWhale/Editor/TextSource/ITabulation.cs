namespace QWhale.Editor.TextSource
{
    using QWhale.Syntax;
    using System;

    public interface ITabulation
    {
        string GetIndentString(int count, int pos);
        string GetIndentString(int count, int p, bool useSpaces);
        int GetPrevTabStop(int pos);
        int GetTabStop(int pos);
        string GetTabString(string s);
        void GetTabString(string s, ITextUndoList operations);
        int PosToTabPos(string s, int pos);
        int PosToTabPos(string s, int pos, bool tabEnd);
        void ResetTabStops();
        void ResetUseSpaces();
        int TabPosToPos(string s, int pos);

        int[] TabStops { get; set; }

        bool UseSpaces { get; set; }
    }
}

