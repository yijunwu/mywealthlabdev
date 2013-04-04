namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface IMargin : IUpdate
    {
        void Assign(IMargin source);
        void CancelDragging();
        bool Contains(int x, int y);
        void DragTo(int x, int y);
        void Paint(IPainter painter, Rectangle rect);
        void PaintColumn(IPainter painter, Rectangle rect);
        void ResetAllowDrag();
        void ResetColumnPositions();
        void ResetColumnsPenColor();
        void ResetColumnsVisible();
        void ResetPenColor();
        void ResetPosition();
        void ResetShowHints();
        void ResetVisible();

        bool AllowDrag { get; set; }

        System.Drawing.Pen ColumnPen { get; set; }

        Color ColumnPenColor { get; set; }

        int[] ColumnPositions { get; set; }

        bool ColumnsVisible { get; set; }

        bool IsDragging { get; set; }

        System.Drawing.Pen Pen { get; set; }

        Color PenColor { get; set; }

        int Position { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        bool ShowHints { get; set; }

        bool Visible { get; set; }
    }
}

