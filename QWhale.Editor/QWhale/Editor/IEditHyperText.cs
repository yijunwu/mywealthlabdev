namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.TextSource;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface IEditHyperText : IHyperText
    {
        event UrlJumpEvent JumpToUrl;

        void Assign(IEditHyperText source);
        bool IsUrlAtPoint(int x, int y);
        bool IsUrlAtPoint(int x, int y, out string url);
        bool IsUrlAtTextPoint(int x, int y, out string url);
        void ResetShowHints();
        void ResetUrlColor();
        void ResetUrlStyle();
        void UrlJump(string text);

        bool HighlightHyperText { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        bool ShowHints { get; set; }

        Color UrlColor { get; set; }

        FontStyle UrlStyle { get; set; }
    }
}

