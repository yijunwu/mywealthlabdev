namespace QWhale.Syntax.Serialization
{
    using QWhale.Syntax.CodeCompletion;
    using System;

    public class XmlCodeSnippetObjectInfo : XmlCodeSnippetLiteralInfo
    {
        public XmlCodeSnippetObjectInfo()
        {
        }

        public XmlCodeSnippetObjectInfo(ICodeSnippetObject owner) : base(owner)
        {
        }
    }
}

