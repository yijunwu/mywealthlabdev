namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface ISyntaxPaint
    {
        event CustomDrawEvent CustomDraw;

        void DrawLine(int index, Point position, Rectangle clipRect);
        void DrawLine(int index, string line, short[] colorData, Point position, Rectangle clipRect);
        bool EqualStyles(int style1, int style2, bool useColors);
        Color GetFontColor(Color color, TextStyle textStyle);
        FontStyle GetFontStyle(FontStyle fontStyle, TextStyle textStyle);
        ILexStyle GetLexStyle(int style, ref TextStyle textStyle);
        int MeasureLine(int index, int pos, int len);
        int MeasureLine(string line, short[] colorData, int pos, int len);
        int MeasureLine(int index, int pos, int len, int width, out int chars, bool exact);
        int MeasureLine(string line, short[] colorData, int pos, int len, int width, out int chars, bool exact);
        bool OnCustomDraw(IPainter painter, Rectangle rect, DrawStage stage, DrawState state, IDrawInfo info);
        void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint);
        void ResetDisableColorPaint();
        void ResetDisableSyntaxPaint();

        bool DisableColorPaint { get; set; }

        bool DisableSyntaxPaint { get; set; }

        ILexer Lexer { get; set; }
    }
}

