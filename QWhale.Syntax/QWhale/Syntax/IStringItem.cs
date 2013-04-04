namespace QWhale.Syntax
{
    using System;

    public interface IStringItem
    {
        void ClearTextStyle(int start, int len, TextStyle style);
        void SetTextStyle(int start, int len, TextStyle style);
        TextStyle TextStyleAt(int pos);

        int LexState { get; set; }

        int PrevLexState { get; set; }

        ItemState State { get; set; }

        string String { get; set; }

        short[] TextData { get; set; }
    }
}

