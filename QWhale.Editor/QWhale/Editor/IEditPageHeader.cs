namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface IEditPageHeader : IUpdate
    {
        void Assign(IEditPageHeader source);
        void Paint(IPainter painter, Rectangle rect, int pageIndex, int pageCount, bool pageNumbers);
        void ResetFont();
        void ResetFontColor();
        void ResetOffset();
        void ResetReverseOnEvenPages();

        string CenterText { get; set; }

        System.Drawing.Font Font { get; set; }

        Color FontColor { get; set; }

        string LeftText { get; set; }

        Point Offset { get; set; }

        bool ReverseOnEvenPages { get; set; }

        string RightText { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        bool Visible { get; set; }
    }
}

