namespace Fidelity.Components
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(SettingsManager), "SettingsManager")]
    public class SettingsManager : Component, ISettingsHost
    {
        private bool bool_0;
        private Dictionary<string, string> dictionary_0;
        private IContainer icontainer_0;
        private string string_0;
        private string string_1;

        public SettingsManager()
        {
            this.dictionary_0 = new Dictionary<string, string>();
            this.method_0();
        }

        public SettingsManager(IContainer container)
        {
            this.dictionary_0 = new Dictionary<string, string>();
            container.Add(this);
            this.method_0();
            this.LoadSettings();
        }

        public bool ContainsKey(string string_2)
        {
            return this.dictionary_0.ContainsKey(string_2);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Get(string string_2, bool defaultValue)
        {
            if (this.Settings.ContainsKey(string_2))
            {
                return bool.Parse(this.Settings[string_2]);
            }
            return defaultValue;
        }

        public DateTime Get(string string_2, DateTime defaultValue)
        {
            if (this.Settings.ContainsKey(string_2))
            {
                return new DateTime(long.Parse(this.Settings[string_2]));
            }
            return defaultValue;
        }

        public double Get(string string_2, double defaultValue)
        {
            if (this.Settings.ContainsKey(string_2))
            {
                return double.Parse(this.Settings[string_2]);
            }
            return defaultValue;
        }

        public Color Get(string string_2, Color defaultValue)
        {
            if (this.Settings.ContainsKey(string_2))
            {
                return Color.FromArgb(int.Parse(this.Settings[string_2]));
            }
            return defaultValue;
        }

        public Font Get(string string_2, Font defaultFont)
        {
            if (!this.Settings.ContainsKey(string_2))
            {
                return defaultFont;
            }
            string[] strArray = this.Settings[string_2].Split(new char[] { '|' });
            if (strArray.Length == 2)
            {
                return new Font(strArray[0], float.Parse(strArray[1]));
            }
            string str = strArray[2];
            FontStyle regular = FontStyle.Regular;
            if ((str != null) && (str == FontStyle.Bold.ToString()))
            {
                regular = FontStyle.Bold;
            }
            else if ((str != null) && (str == FontStyle.Italic.ToString()))
            {
                regular = FontStyle.Italic;
            }
            else if ((str != null) && (str == FontStyle.Regular.ToString()))
            {
                regular = FontStyle.Regular;
            }
            else if ((str != null) && (str == FontStyle.Strikeout.ToString()))
            {
                regular = FontStyle.Strikeout;
            }
            else if ((str != null) && (str == FontStyle.Underline.ToString()))
            {
                regular = FontStyle.Underline;
            }
            else if ((str != null) && (str == "Bold, Italic"))
            {
                regular = FontStyle.Italic | FontStyle.Bold;
            }
            return new Font(strArray[0], float.Parse(strArray[1]), regular);
        }

        public int Get(string string_2, int defaultValue)
        {
            if (this.Settings.ContainsKey(string_2))
            {
                return int.Parse(this.Settings[string_2]);
            }
            return defaultValue;
        }

        public string Get(string string_2, string defaultValue)
        {
            if (this.Settings.ContainsKey(string_2))
            {
                return this.Settings[string_2];
            }
            return defaultValue;
        }

        public bool Get(Form form_0, string string_2)
        {
            string str = this.Get(string_2 + ".Location", "");
            if (str == "")
            {
                return false;
            }
            string[] strArray = str.Split(new char[] { ',' });
            form_0.Left = int.Parse(strArray[0]);
            form_0.Top = int.Parse(strArray[1]);
            if (form_0.FormBorderStyle != FormBorderStyle.FixedDialog)
            {
                strArray = this.Get(string_2 + ".Size", "").Split(new char[] { ',' });
                form_0.Width = int.Parse(strArray[0]);
                form_0.Height = int.Parse(strArray[1]);
            }
            str = this.Get(string_2 + ".Tag", "");
            if (str != "")
            {
                form_0.Tag = str;
            }
            return ((form_0.Tag != null) && (form_0.Tag is string));
        }

        public void LoadSettings()
        {
            this.dictionary_0.Clear();
            if ((this.string_0 != null) && (this.string_1 != null))
            {
                string path = this.string_0 + this.string_1;
                if (File.Exists(path))
                {
                    foreach (string str4 in File.ReadAllLines(path))
                    {
                        string str = str4;
                        if (this.bool_0)
                        {
                            try
                            {
                                str = Cryptography.Crypt(str, this._password, false);
                            }
                            catch
                            {
                                str = str4;
                            }
                        }
                        int index = str.IndexOf('=');
                        if (index > 0)
                        {
                            string str2 = str.Substring(0, index);
                            string str3 = str.Substring(index + 1);
                            this.dictionary_0[str2] = str3;
                        }
                    }
                }
            }
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        public void SaveSettings()
        {
            string fileName = this.string_0 + this.string_1;
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> pair in this.dictionary_0)
            {
                string item = pair.Key + "=" + pair.Value;
                list.Add(item);
            }
            list.Sort();
            string[] contents = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                if (this.bool_0)
                {
                    contents[i] = Cryptography.Crypt(list[i], this._password, true);
                }
                else
                {
                    contents[i] = list[i];
                }
            }
            FileNameValidator.ValidateFileName(fileName);
            File.WriteAllLines(fileName, contents);
        }

        public void Set(string string_2, bool value)
        {
            this.Settings[string_2] = value.ToString();
        }

        public void Set(string string_2, DateTime value)
        {
            this.Settings[string_2] = value.Ticks.ToString();
        }

        public void Set(string string_2, double value)
        {
            this.Settings[string_2] = value.ToString();
        }

        public void Set(string string_2, Color color)
        {
            this.Settings[string_2] = color.ToArgb().ToString();
        }

        public void Set(string string_2, Font value)
        {
            string name = value.FontFamily.Name;
            float size = value.Size;
            string str2 = value.Style.ToString();
            this.Settings[string_2] = string.Concat(new object[] { name, "|", size, "|", str2 });
        }

        public void Set(string string_2, int value)
        {
            this.Settings[string_2] = value.ToString();
        }

        public void Set(string string_2, string value)
        {
            this.Settings[string_2] = value;
        }

        public void Set(Form form_0, string string_2)
        {
            if (form_0.WindowState != FormWindowState.Minimized)
            {
                string tag = form_0.Left + "," + form_0.Top;
                this.Set(string_2 + ".Location", tag);
                tag = form_0.Width + "," + form_0.Height;
                this.Set(string_2 + ".Size", tag);
                tag = "";
                if ((form_0.Tag != null) && (form_0.Tag is string))
                {
                    tag = form_0.Tag as string;
                }
                this.Set(string_2 + ".Tag", tag);
                this.Set(string_2 + ".Maximized", form_0.WindowState == FormWindowState.Maximized);
            }
        }

        private string _password
        {
            get
            {
                return "NWHwp+yolarU1q4QNvOSXyRS6sGTALXUDDb9lWvOIESmW05e4khN9cWQrTkge5GIzorPBc7KZUThrqEeMiFGPADAW6haHrhlxylECdEahf1IBkIg57hOyrItogl";
            }
        }

        public string FileName
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
                this.LoadSettings();
            }
        }

        public bool IsEncrypted
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public string RootPath
        {
            get
            {
                return this.string_0;
            }
            set
            {
                if (((value != null) && (value != "")) && (value[value.Length - 1] != '\\'))
                {
                    value = value + @"\";
                }
                this.string_0 = value;
                if (this.string_0 != null)
                {
                    this.LoadSettings();
                }
            }
        }

        public IDictionary<string, string> Settings
        {
            get
            {
                return this.dictionary_0;
            }
        }
    }
}

