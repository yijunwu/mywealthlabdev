namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface ISyntaxError : ICloneable
    {
        string Description { get; set; }

        string Name { get; set; }

        Point Position { get; set; }

        IRange Range { get; set; }

        System.Drawing.Size Size { get; set; }
    }
}

