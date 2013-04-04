namespace QWhale.Syntax
{
    using System;

    public enum XmlLexerToken
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
        Comment,
        Interr,
        CDATA,
        CDATABody
    }
}

