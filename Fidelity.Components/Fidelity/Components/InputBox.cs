namespace Fidelity.Components
{
    using System;
    using System.Windows.Forms;

    public static class InputBox
    {
        public static string Show(string caption, string label)
        {
            return Show(caption, label, "", CharacterCasing.Normal);
        }

        public static string Show(string caption, string label, string defaultValue)
        {
            return Show(caption, label, defaultValue, CharacterCasing.Normal);
        }

        public static string Show(string caption, string label, string defaultValue, CharacterCasing casing)
        {
            InputForm form = new InputForm(caption, label, defaultValue, casing);
            if (form.ShowDialog() == DialogResult.OK)
            {
                return form.Input;
            }
            return "";
        }
    }
}

