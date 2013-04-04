namespace QWhale.Editor
{
    using System;

    public interface IEventHandlers
    {
        string[] EventNames { get; }

        KeyEventEx MacroRecordEvent { get; }
    }
}

