namespace QWhale.Syntax
{
    using System;

    public class DescriptionLookupEventArgs : EventArgs
    {
        public string Description;
        public object Member;
        public string Name;

        public DescriptionLookupEventArgs(object member, string name)
        {
            this.Member = member;
            this.Name = name;
        }
    }
}

