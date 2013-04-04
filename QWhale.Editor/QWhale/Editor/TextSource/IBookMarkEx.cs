namespace QWhale.Editor.TextSource
{
    using System;

    public interface IBookMarkEx : IBookMark
    {
        string Description { get; set; }

        string Name { get; set; }

        string Url { get; set; }
    }
}

