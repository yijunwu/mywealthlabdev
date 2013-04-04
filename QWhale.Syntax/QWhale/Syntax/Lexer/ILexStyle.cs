namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface ILexStyle
    {
        void Assign(ILexStyle source);
        void ResetBackColor();
        void ResetFontStyle();
        void ResetForeColor();
        void ResetPlainText();

        Color BackColor { get; set; }

        bool BackColorEnabled { get; set; }

        bool BoldEnabled { get; set; }

        string Desc { get; set; }

        System.Drawing.FontStyle FontStyle { get; set; }

        Color ForeColor { get; set; }

        bool ForeColorEnabled { get; set; }

        int Index { get; }

        bool ItalicEnabled { get; set; }

        string Name { get; set; }

        bool PlainText { get; set; }

        ILexScheme Scheme { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        bool UnderlineEnabled { get; set; }
    }
}

