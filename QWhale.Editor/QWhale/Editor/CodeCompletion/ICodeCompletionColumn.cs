namespace QWhale.Editor.CodeCompletion
{
    using System;
    using System.Drawing;

    public interface ICodeCompletionColumn
    {
        void ResetFontStyle();
        void ResetForeColor();
        void ResetVisible();

        System.Drawing.FontStyle FontStyle { get; set; }

        Color ForeColor { get; set; }

        string Name { get; set; }

        bool Visible { get; set; }
    }
}

