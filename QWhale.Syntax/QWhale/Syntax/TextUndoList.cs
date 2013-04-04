namespace QWhale.Syntax
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class TextUndoList : List<ITextUndo>, ITextUndoList, IList<ITextUndo>, ICollection<ITextUndo>, IEnumerable<ITextUndo>, IEnumerable
    {
        void ITextUndoList.Sort(IComparer<ITextUndo> comparer1)
        {
            base.Sort(comparer1);
        }
    }
}

