namespace Steema.TeeChart.Languages
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    public interface ITranslator
    {
        bool AskLanguage(ref int language);
        string GetLanguage();
        bool HasUpperCase();
        void InitLanguage(int language);
        string LanguageToString(int language);
        void Translate(Control c);
        void Translate(Control c, ArrayList excludechildren);
    }
}

