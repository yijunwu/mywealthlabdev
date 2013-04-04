namespace QWhale.Syntax.Lexer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class LexStates : List<ILexState>, ILexStates, IList<ILexState>, ICollection<ILexState>, IEnumerable<ILexState>, IEnumerable
    {
        private ILexScheme scheme;

        public LexStates(ILexScheme owner)
        {
            this.scheme = owner;
        }

        public virtual ILexState AddLexState()
        {
            ILexState item = new LexState(this.scheme);
            base.Add(item);
            return item;
        }

        public virtual ILexState FindLexState(string name)
        {
            foreach (ILexState state in this)
            {
                if (state.Name == name)
                {
                    return state;
                }
            }
            return null;
        }

        public virtual ILexState InsertLexState(int index)
        {
            ILexState item = new LexState();
            base.Insert(index, item);
            return item;
        }
    }
}

