namespace QWhale.Syntax.CodeCompletion
{
    using System;

    [Flags]
    public enum MemberAttribute
    {
        CanRead = 1,
        CanWrite = 2,
        NoDescription = 4,
        None = 0
    }
}

