namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IGutter : IUpdate
    {
        event EventHandler Click;

        event EventHandler DoubleClick;

        event DrawUserMarginEvent DrawUserMargin;

        void Assign(IGutter source);
        void GetHitTest(int x, int y, IHitTestInfo hitTestInfo);
        bool InvalidateLineNumberArea();
        void OnClick(EventArgs e);
        void OnDoubleClick(EventArgs e);
        void Paint(IPainter painter, Rectangle rect);
        void Paint(IPainter painter, Rectangle rect, int startLine);
        void ResetBookMarkImageIndex();
        void ResetBrushColor();
        void ResetDrawLineBookmarks();
        void ResetLineBookmarksColor();
        void ResetLineModificatorChangedColor();
        void ResetLineModificatorSavedColor();
        void ResetLineNumbersAlignment();
        void ResetLineNumbersBackColor();
        void ResetLineNumbersForeColor();
        void ResetLineNumbersLeftIndent();
        void ResetLineNumbersRightIndent();
        void ResetLineNumbersStart();
        void ResetOptions();
        void ResetOutliningLeftIndent();
        void ResetOutliningRightIndent();
        void ResetPenColor();
        void ResetShowBookmarkHints();
        void ResetUserMarginBackColor();
        void ResetUserMarginForeColor();
        void ResetUserMarginText();
        void ResetUserMarginWidth();
        void ResetVisible();
        void ResetWidth();
        void ResetWrapImageIndex();

        int BookMarkImageIndex { get; set; }

        System.Drawing.Brush Brush { get; set; }

        Color BrushColor { get; set; }

        int DisplayArea { get; }

        int DisplayWidth { get; }

        bool DrawLineBookmarks { get; set; }

        ImageList Images { get; set; }

        Color LineBookmarksColor { get; set; }

        Color LineModificatorChangedColor { get; set; }

        Color LineModificatorSavedColor { get; set; }

        StringAlignment LineNumbersAlignment { get; set; }

        Color LineNumbersBackColor { get; set; }

        Color LineNumbersForeColor { get; set; }

        int LineNumbersLeftIndent { get; set; }

        int LineNumbersRightIndent { get; set; }

        int LineNumbersStart { get; set; }

        int MaxLineNumberLength { get; set; }

        GutterOptions Options { get; set; }

        int OutliningLeftIndent { get; set; }

        int OutliningRightIndent { get; set; }

        System.Drawing.Pen Pen { get; set; }

        Color PenColor { get; set; }

        Rectangle Rect { get; }

        ISerializationInfo SerializationInfo { get; set; }

        bool ShowBookmarkHints { get; set; }

        Color UserMarginBackColor { get; set; }

        Color UserMarginForeColor { get; set; }

        string UserMarginText { get; set; }

        int UserMarginWidth { get; set; }

        bool Visible { get; set; }

        int Width { get; set; }

        int WrapImageIndex { get; set; }
    }
}

