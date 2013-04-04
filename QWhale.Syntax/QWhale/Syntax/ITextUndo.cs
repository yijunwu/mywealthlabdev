namespace QWhale.Syntax
{
    using System;

    public interface ITextUndo
    {
        int Len { get; set; }

        int Start { get; set; }

        string Text { get; set; }
    }
}

