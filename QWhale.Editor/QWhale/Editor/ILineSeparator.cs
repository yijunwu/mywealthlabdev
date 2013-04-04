namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface ILineSeparator : IUpdate
    {
        void Assign(ILineSeparator source);
        bool NeedHide();
        bool NeedHighlight();
        bool NeedHighlightDisplayLine(int index);
        bool NeedHighlightLine(int index);
        void ResetContentDividerColor();
        void ResetHighlightBackColor();
        void ResetHighlightForeColor();
        void ResetLineColor();
        void ResetOptions();
        void TempHighlightLine(int index);
        void TempUnhighlightLine();

        Color ContentDividerColor { get; set; }

        Color HighlightBackColor { get; set; }

        Color HighlightForeColor { get; set; }

        Color LineColor { get; set; }

        SeparatorOptions Options { get; set; }

        ISerializationInfo SerializationInfo { get; set; }
    }
}

