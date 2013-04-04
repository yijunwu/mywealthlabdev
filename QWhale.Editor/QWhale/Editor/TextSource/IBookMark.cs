namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface IBookMark
    {
        void Assign(IBookMark source);

        int Index { get; }

        int Line { get; }

        int Pos { get; }

        Point Position { get; }

        ISerializationInfo SerializationInfo { get; set; }
    }
}

