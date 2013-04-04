namespace QWhale.Syntax.Lexer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class LexStyles : List<ILexStyle>, ILexStyles, IList<ILexStyle>, ICollection<ILexStyle>, IEnumerable<ILexStyle>, IEnumerable
    {
        private ILexScheme owner;

        public LexStyles(ILexScheme owner)
        {
            this.owner = owner;
        }

        public virtual ILexStyle AddLexStyle()
        {
            ILexStyle item = new LexStyle(this.owner);
            base.Add(item);
            return item;
        }

        public virtual ILexStyle FindLexStyle(string name)
        {
            foreach (ILexStyle style in this)
            {
                if (style.Name == name)
                {
                    return style;
                }
            }
            return null;
        }

        public virtual ILexStyle InsertLexStyle(int index)
        {
            ILexStyle item = new LexStyle();
            base.Insert(index, item);
            return item;
        }
    }
}

