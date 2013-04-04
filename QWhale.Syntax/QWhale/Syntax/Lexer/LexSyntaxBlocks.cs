namespace QWhale.Syntax.Lexer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class LexSyntaxBlocks : List<ILexSyntaxBlock>, ILexSyntaxBlocks, IList<ILexSyntaxBlock>, ICollection<ILexSyntaxBlock>, IEnumerable<ILexSyntaxBlock>, IEnumerable
    {
        private ILexState state;

        public LexSyntaxBlocks(ILexState state)
        {
            this.state = state;
        }

        public virtual ILexSyntaxBlock AddLexSyntaxBlock()
        {
            ILexSyntaxBlock item = new LexSyntaxBlock(this.state);
            base.Add(item);
            return item;
        }

        public virtual ILexSyntaxBlock FindSyntaxBlock(string name)
        {
            foreach (ILexSyntaxBlock block in this)
            {
                if (block.Name == name)
                {
                    return block;
                }
            }
            return null;
        }

        public virtual ILexSyntaxBlock InsertLexSyntaxBlock(int index)
        {
            ILexSyntaxBlock item = new LexSyntaxBlock();
            base.Insert(index, item);
            return item;
        }
    }
}

