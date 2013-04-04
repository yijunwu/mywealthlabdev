namespace QWhale.Common
{
    using System;

    public interface ISerializationInfo
    {
        void FixupReferences(object owner);
        void Load();
    }
}

