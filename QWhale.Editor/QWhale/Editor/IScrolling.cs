namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IScrolling : IUpdate
    {
        event EventHandler HorizontalScroll;

        event EventHandler ScrollButtonClick;

        event EventHandler VerticalScroll;

        void Assign(IScrolling source);
        void MouseScroll(int delta);
        void OnScrollButtonClick(object sender, EventArgs e);
        void ResetDefaultHorzScrollSize();
        void ResetOptions();
        void ResetScrollBars();
        void SystemScroll(int code, bool vert);
        void UpdateFlat();
        void UpdateScroll();
        void UpdateScroll(bool updateSize);

        int DefaultHorzScrollSize { get; set; }

        bool FixedScrollSize { get; }

        bool HasHScrollBar { get; }

        bool HasVScrollBar { get; }

        IScrollingButtons HorzButtons { get; }

        bool HorzScrollbarVisible { get; }

        ScrollBar HScrollBar { get; }

        ScrollingOptions Options { get; set; }

        ISyntaxEdit Owner { get; }

        RichTextBoxScrollBars ScrollBars { get; set; }

        bool ScrollByPixels { get; }

        ISerializationInfo SerializationInfo { get; set; }

        IScrollingButtons VertButtons { get; }

        bool VertScrollbarVisible { get; }

        ScrollBar VScrollBar { get; }

        int WindowOriginX { get; set; }

        int WindowOriginY { get; set; }
    }
}

