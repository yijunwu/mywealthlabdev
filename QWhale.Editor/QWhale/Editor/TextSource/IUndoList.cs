namespace QWhale.Editor.TextSource
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IUndoList : IList<IUndoData>, ICollection<IUndoData>, IEnumerable<IUndoData>, IEnumerable
    {
    }
}

