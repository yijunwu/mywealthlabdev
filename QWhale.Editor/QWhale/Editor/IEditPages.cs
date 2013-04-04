namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public interface IEditPages : IUpdate, IEnumerable<IEditPage>, IEnumerable
    {
        event DrawHeaderEvent DrawHeader;

        IEditPage Add();
        void CancelDragging();
        void Clear();
        void DisplayRulers();
        IEditPage GetPageAt(Point position);
        IEditPage GetPageAt(int x, int y);
        IEditPage GetPageAtCursor();
        IEditPage GetPageAtPoint(Point position);
        IEditPage GetPageAtPoint(int x, int y);
        int GetPageIndexAt(Point position);
        int GetPageIndexAt(int x, int y);
        int GetPageIndexAtCursor();
        int GetPageIndexAtPoint(Point position);
        int GetPageIndexAtPoint(int x, int y);
        int IndexOf(IEditPage page);
        void InitDefaultPageSettings(PageSettings settings);
        void Invalidate(IEditPage page);
        void OnDrawHeader(ref string text);
        void Paint(IPainter painter, Rectangle rect);
        void ResetBackColor();
        void ResetBorderColor();
        void ResetDisplayWhiteSpace();
        void ResetPageType();
        void ResetRulerBackColor();
        void ResetRulerIndentBackColor();
        void ResetRulerOptions();
        void ResetRulers();
        void ResetRulerUnits();
        void Update(IEditPage page);
        void Update(IEditPage page, bool changed);
        void UpdatePages(int index);

        bool ApplyRulerToAllPages { get; set; }

        Color BackColor { get; set; }

        Color BorderColor { get; set; }

        Size Caps { get; }

        int Count { get; }

        bool DefaultLandscape { get; }

        Margins DefaultMargins { get; }

        IEditPage DefaultPage { get; set; }

        PaperKind DefaultPageKind { get; }

        Size DefaultPageSize { get; }

        bool DisplayWhiteSpace { get; set; }

        int Height { get; }

        IEditRuler HorzRuler { get; }

        IEditPage this[int index] { get; set; }

        IList<IEditPage> List { get; }

        PaperKind PageKind { get; set; }

        QWhale.Editor.PageType PageType { get; set; }

        Color RulerBackColor { get; set; }

        Color RulerIndentBackColor { get; set; }

        QWhale.Editor.RulerOptions RulerOptions { get; set; }

        EditRulers Rulers { get; set; }

        QWhale.Editor.RulerUnits RulerUnits { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        bool Transparent { get; set; }

        IEditRuler VertRuler { get; }

        int Width { get; }
    }
}

