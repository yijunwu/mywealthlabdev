namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;
    using System.Drawing.Printing;

    public interface IEditPage : IUpdate
    {
        void Assign(IEditPage source);
        Rectangle GetBounds(bool includeSpace);
        void Invalidate();
        void Paint(IPainter painter);
        void Update(int startLine, int endLine, Point origin);

        int BottomIndent { get; set; }

        Rectangle BoundsRect { get; }

        Rectangle ClientRect { get; }

        int DisplayWidth { get; }

        int EndLine { get; }

        IEditPageHeader Footer { get; set; }

        IEditPageHeader Header { get; set; }

        int HorzOffset { get; set; }

        int Index { get; }

        bool IsFirstPage { get; }

        bool IsLastPage { get; }

        bool Landscape { get; set; }

        int LeftIndent { get; set; }

        System.Drawing.Printing.Margins Margins { get; set; }

        IEditPage NextPage { get; }

        Point Origin { get; }

        PaperKind PageKind { get; set; }

        Rectangle PageRect { get; }

        IEditPages Pages { get; set; }

        Size PageSize { get; set; }

        bool PaintNumber { get; set; }

        IEditPage PrevPage { get; }

        int RightIndent { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        int StartLine { get; }

        int TopIndent { get; set; }

        bool UsePrinterSettings { get; set; }

        int VertOffset { get; set; }

        Rectangle WhiteSpaceBottomRect { get; }

        Rectangle WhiteSpaceTopRect { get; }
    }
}

