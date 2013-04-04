namespace QWhale.Syntax
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ITextUndoList : IList<ITextUndo>, ICollection<ITextUndo>, IEnumerable<ITextUndo>, IEnumerable
    {
        void Sort(IComparer<ITextUndo> comparer);
    }
}

