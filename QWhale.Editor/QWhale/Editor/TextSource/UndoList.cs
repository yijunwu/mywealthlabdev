namespace QWhale.Editor.TextSource
{
    using System.Collections;
    using System.Collections.Generic;

    public class UndoList : List<IUndoData>, IUndoList, IList<IUndoData>, ICollection<IUndoData>, IEnumerable<IUndoData>, IEnumerable
    {
    }
}

