namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface INetNamespace
    {
        string GetName();

        string Alias { get; set; }

        string Namespace { get; set; }

        bool System { get; set; }
    }
}

