namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.TextSource;
    using System;
    using System.Drawing;

    public interface IEditBraceMatching : IBraceMatching
    {
        void Assign(IEditBraceMatching source);
        void ResetBackColor();
        void ResetFontStyle();
        void ResetForeColor();
        void ResetUseRoundRect();

        Color BackColor { get; set; }

        QWhale.Editor.TextSource.BracesOptions BracesOptions { get; set; }

        char[] ClosingBraces { get; set; }

        System.Drawing.FontStyle FontStyle { get; set; }

        Color ForeColor { get; set; }

        char[] OpenBraces { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        bool UseRoundRect { get; set; }
    }
}

