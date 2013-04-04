namespace QWhale.Editor
{
    using QWhale.Syntax;
    using System;

    public interface IHitTestInfo
    {
        void Reset();

        int GutterImage { get; set; }

        QWhale.Editor.HitTest HitTest { get; set; }

        IStringItem Item { get; set; }

        int Line { get; set; }

        int OutlineIndex { get; set; }

        IOutlineRange OutlineRange { get; set; }

        int Page { get; set; }

        int Pos { get; set; }

        string String { get; set; }

        int Style { get; set; }

        QWhale.Syntax.TextStyle TextStyle { get; set; }

        string Url { get; set; }

        string Word { get; set; }
    }
}

