namespace Fidelity.Components
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public interface ISettingsHost
    {
        bool ContainsKey(string string_0);
        bool Get(string string_0, bool defaultValue);
        DateTime Get(string string_0, DateTime defaultValue);
        double Get(string string_0, double defaultValue);
        Color Get(string string_0, Color defaultValue);
        Font Get(string string_0, Font defaultFont);
        int Get(string string_0, int defaultValue);
        string Get(string string_0, string defaultValue);
        bool Get(Form form_0, string string_0);
        void Set(string string_0, bool value);
        void Set(string string_0, DateTime value);
        void Set(string string_0, double value);
        void Set(string string_0, Color color);
        void Set(string string_0, Font value);
        void Set(string string_0, int value);
        void Set(string string_0, string value);
        void Set(Form form_0, string string_0);
    }
}

