namespace QWhale.Syntax
{
    using System;

    public class MemberLookupEventArgs : EventArgs
    {
        public object Member;
        public string Name;
        public object Result;
        public CodeCompletionScope Scope;

        public MemberLookupEventArgs(object member, string name)
        {
            this.Member = member;
            this.Name = name;
        }
    }
}

