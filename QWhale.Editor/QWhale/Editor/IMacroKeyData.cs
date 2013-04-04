namespace QWhale.Editor
{
    using QWhale.Common;
    using System;

    public interface IMacroKeyData : IKeyData
    {
        string FullName { get; set; }

        string ParamStr { get; set; }

        KeyEventType ParamType { get; set; }

        ISerializationInfo SerializationInfo { get; set; }
    }
}

