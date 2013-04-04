namespace QWhale.Editor
{
    using System;
    using System.Drawing;

    public interface IRulerIndent
    {
        void CancelDragging();
        void DrawIndent(Graphics graph, Rectangle rect, bool vertical, Color indentBackColor, Color backColor);

        bool Dragging { get; set; }

        int Indent { get; set; }

        IndentOrientation Orientation { get; set; }
    }
}

