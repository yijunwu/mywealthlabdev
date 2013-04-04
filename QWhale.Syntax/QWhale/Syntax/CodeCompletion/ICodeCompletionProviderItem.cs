namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using System;

    public interface ICodeCompletionProviderItem
    {
        ISerializationInfo SerializationInfo { get; set; }
    }
}

