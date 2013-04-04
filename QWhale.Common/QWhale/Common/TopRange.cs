namespace QWhale.Common
{
    using System;

    public class TopRange : SortRange
    {
        public TopRange() : base(-1)
        {
        }

        public override bool Contains(IRange range)
        {
            return true;
        }
    }
}

