namespace QWhale.Syntax
{
    using System;

    public enum HtmlLexerToken
    {
        None,
        WhiteSpace,
        Body,
        OpenTag,
        CloseTag,
        OpenEndTag,
        CloseEndTag,
        Equal,
        TagName,
        ParamName,
        ParamValue,
        Comment
    }
}

