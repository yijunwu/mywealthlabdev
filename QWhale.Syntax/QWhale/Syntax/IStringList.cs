namespace QWhale.Syntax
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IStringList : IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable, IImport, IExport
    {
        string Text { get; set; }
    }
}

