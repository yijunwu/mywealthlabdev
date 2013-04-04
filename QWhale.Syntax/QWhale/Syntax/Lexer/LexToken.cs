namespace QWhale.Syntax.Lexer
{
    using System;

    public enum LexToken
    {
        Identifier,
        Number,
        Resword,
        Comment,
        XmlComment,
        Symbol,
        Whitespace,
        String,
        Directive,
        Include
    }
}

