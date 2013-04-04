namespace QWhale.Editor
{
    using System;
    using System.Windows.Forms;

    public interface IKeyList
    {
        void Add(Keys keys, KeyEvent action);
        void Add(Keys keys, KeyEventEx action, object param);
        void Add(Keys keys, KeyEvent action, int state, int leaveState);
        void Add(Keys keys, KeyEventEx action, object param, int state, int leaveState);
        void AddNormal(Keys keys, KeyEvent action);
        void AddNormal(Keys keys, KeyEventEx action, object param);
        void Clear();
        bool ExecuteKey(Keys keys, ref int state);
        bool FindKey(Keys keys, int state);
        void Remove(Keys keys);
        void Remove(Keys keys, int state);

        IKeyData[] EventData { get; }

        IEventHandlers Handlers { get; }
    }
}

