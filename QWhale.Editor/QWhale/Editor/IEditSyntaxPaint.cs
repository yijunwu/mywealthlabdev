namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface IEditSyntaxPaint : ISyntaxPaint
    {
        Color GetBackColor(bool readOnly);
        Color GetForeColor(bool readOnly);
        Region GetRectRegion(Rectangle rect);
        Region GetRectRegion(SelectionType selectionType, Rectangle rect, bool atTopLeftEnd, bool atBottomRightEnd);
        void PaintLineBookMarks(IPainter painter, Rectangle rect);
        void PaintWindow(IPainter painter, int startLine, Rectangle rect, Point location, float scaleX, float scaleY, bool specialPaint, bool inPrinting);
        void ResetColumnsIndentForeColor();
        void ResetDisabledBackColor();
        void ResetDisabledForeColor();
        void ResetDrawColumnsIndent();
        void ResetReadonlyBackColor();
        void ResetReadonlyForeColor();
        void ResetSyntaxErrorsHints();

        Color ColumnsIndentForeColor { get; set; }

        Color DisabledBackColor { get; set; }

        Color DisabledForeColor { get; set; }

        bool DrawColumnsIndent { get; set; }

        Color ReadonlyBackColor { get; set; }

        Color ReadonlyForeColor { get; set; }

        bool SyntaxErrorsHints { get; set; }
    }
}

