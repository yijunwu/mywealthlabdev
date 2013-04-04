namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CodeSnippetObjects : List<ICodeSnippetObject>, ICodeSnippetObjects, IList<ICodeSnippetObject>, ICollection<ICodeSnippetObject>, IEnumerable<ICodeSnippetObject>, IEnumerable
    {
        public virtual ICodeSnippetObject AddObject()
        {
            ICodeSnippetObject item = new CodeSnippetObject();
            base.Add(item);
            return item;
        }

        public virtual ICodeSnippetObject InsertObject(int index)
        {
            ICodeSnippetObject item = new CodeSnippetObject();
            base.Insert(index, item);
            return item;
        }
    }
}

