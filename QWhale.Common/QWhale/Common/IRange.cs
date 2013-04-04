namespace QWhale.Common
{
    using System;
    using System.Drawing;

    public interface IRange : ICloneable
    {
        Point EndPoint { get; set; }

        bool IsEmpty { get; }

        System.Drawing.Size Size { get; set; }

        Point StartPoint { get; set; }
    }
}

