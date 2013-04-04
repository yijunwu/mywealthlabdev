namespace QWhale.Editor.TextSource
{
    using System;
    using System.Drawing;

    public interface IEdit
    {
        bool BreakLine();
        bool DeleteBlock(Rectangle rect);
        bool DeleteLeft(int len);
        bool DeleteRight(int len);
        void IndentLine();
        bool Insert(string text);
        bool InsertBlock(string[] strings);
        bool InsertBlock(ITextStrings strings);
        bool InsertBlock(string text);
        bool InsertBlock(string[] strings, bool select);
        bool InsertFromFile(string fileName);
        bool LineIsReadonly(int index);
        bool NewLine();
        bool NewLineAbove();
        bool NewLineBelow();
        bool PositionIsReadonly(Point position);
        void ResetIndentOptions();
        void ResetMaxLength();
        void ResetModified();
        void ResetOverWrite();
        void ResetReadonly();
        void ResetSingleLineMode();
        void SetLineReadonly(int index, bool readOnly);
        bool UnBreakLine();

        QWhale.Editor.TextSource.IndentOptions IndentOptions { get; set; }

        int MaxLength { get; set; }

        bool Modified { get; set; }

        bool Overwrite { get; set; }

        bool Readonly { get; set; }

        bool SingleLineMode { get; set; }
    }
}

