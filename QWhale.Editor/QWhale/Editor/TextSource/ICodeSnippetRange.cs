namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using System;

    public interface ICodeSnippetRange : IRange, ICloneable
    {
        string ID { get; set; }

        bool IsEditable { get; set; }

        string Tooltip { get; set; }
    }
}

