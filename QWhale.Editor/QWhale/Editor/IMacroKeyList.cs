namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IMacroKeyList : IList<IMacroKeyData>, ICollection<IMacroKeyData>, IEnumerable<IMacroKeyData>, IEnumerable
    {
        void LinkMacros(object owner, IEventHandlers keyHandlers);

        ISerializationInfo SerializationInfo { get; set; }
    }
}

