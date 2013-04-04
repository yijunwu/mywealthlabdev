namespace QWhale.Syntax
{
    using System;

    internal class MemberItem
    {
        public int Index;
        public string String;

        public MemberItem(string s, int index)
        {
            this.String = s;
            this.Index = index;
        }

        public override string ToString()
        {
            return this.String;
        }
    }
}

