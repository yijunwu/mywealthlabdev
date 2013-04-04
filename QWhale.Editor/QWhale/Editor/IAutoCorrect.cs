namespace QWhale.Editor
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface IAutoCorrect
    {
        event AutoCorrectEvent AutoCorrect;

        bool HasAutoCorrection(string word, out string correctWord);

        char[] AutoCorrectDelimiters { get; set; }

        bool AutoCorrection { get; set; }
    }
}

