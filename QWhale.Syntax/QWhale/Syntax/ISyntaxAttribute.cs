namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface ISyntaxAttribute : ICloneable
    {
        Point EndPosition { get; }

        string Name { get; set; }

        Point Position { get; set; }

        IRange Range { get; }

        object Value { get; set; }
    }
}

