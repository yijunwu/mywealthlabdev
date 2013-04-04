namespace QWhale.Syntax.Lexer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class LexReswordSets : List<ILexReswordSet>, ILexReswordSets, IList<ILexReswordSet>, ICollection<ILexReswordSet>, IEnumerable<ILexReswordSet>, IEnumerable
    {
        private ILexSyntaxBlock block;

        public LexReswordSets(ILexSyntaxBlock block)
        {
            this.block = block;
        }

        public virtual ILexReswordSet AddLexReswordSet()
        {
            ILexReswordSet item = new LexReswordSet(this.block);
            base.Add(item);
            return item;
        }

        public virtual int FindResword(string resword)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].FindResword(resword))
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual ILexReswordSet InsertLexReswordSet(int index)
        {
            ILexReswordSet item = new LexReswordSet(this.block);
            base.Insert(index, item);
            return item;
        }
    }
}

