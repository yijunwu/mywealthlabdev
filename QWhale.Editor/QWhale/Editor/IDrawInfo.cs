namespace QWhale.Editor
{
    using QWhale.Syntax;
    using System;

    public interface IDrawInfo
    {
        void Reset();

        int Char { get; set; }

        int GutterImage { get; set; }

        int Line { get; set; }

        int Page { get; set; }

        bool Selection { get; set; }

        short Style { get; set; }

        string Text { get; set; }

        QWhale.Syntax.TextStyle TextStyle { get; set; }
    }
}

