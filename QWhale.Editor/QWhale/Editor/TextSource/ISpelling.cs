namespace QWhale.Editor.TextSource
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public interface ISpelling
    {
        event WordSpellEvent WordSpell;

        bool IsWordCorrect(string text);
        void ResetCheckSpelling();

        bool CheckSpelling { get; set; }

        Hashtable SpellTable { get; }
    }
}

