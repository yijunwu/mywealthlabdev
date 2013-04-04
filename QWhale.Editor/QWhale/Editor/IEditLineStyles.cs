namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public interface IEditLineStyles : IList<IEditLineStyle>, ICollection<IEditLineStyle>, IEnumerable<IEditLineStyle>, IEnumerable
    {
        int AddLineStyle();
        int AddLineStyle(string name, Color foreColor, Color backColor, Color penColor, int imageIndex, LineStyleOptions options);
        void Assign(IEditLineStyles source);
        int IndexOfName(string name);

        ISerializationInfo SerializationInfo { get; set; }
    }
}

