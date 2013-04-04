namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;

    public interface IOutlineRange : IRange, ICloneable
    {
        string DisplayText { get; }

        int Level { get; set; }

        string Text { get; }

        bool Visible { get; set; }
    }
}

