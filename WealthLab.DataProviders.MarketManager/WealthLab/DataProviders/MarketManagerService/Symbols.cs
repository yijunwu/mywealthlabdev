namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

    public class Symbols
    {
        public char Delimeter;
        [XmlIgnore]
        public List<string> Items;
        public bool Sort;

        public Symbols()
        {
            this.Sort = true;
            this.Delimeter = ' ';
            this.Items = new List<string>();
        }

        public Symbols(List<string> symbols)
        {
            this.Sort = true;
            this.Delimeter = ' ';
            this.Items = new List<string>();
            this.Items = symbols;
        }

        public Symbols(string text)
        {
            this.Sort = true;
            this.Delimeter = ' ';
            this.Items = new List<string>();
            this.Text = text;
        }

        public void AddPrefix(string prefix, char delimeter)
        {
            prefix = (prefix == "None") ? "" : prefix;
            for (int i = 0; i < this.Items.Count; i++)
            {
                string[] strArray = this.Items[i].Split(new char[] { delimeter });
                if ((strArray.Length > 0) && (strArray[strArray.Length - 1] != ""))
                {
                    this.Items[i] = prefix + strArray[strArray.Length - 1];
                }
            }
        }

        public void AddSuffix(string suffix, char delimeter)
        {
            suffix = (suffix == "None") ? "" : suffix;
            for (int i = 0; i < this.Items.Count; i++)
            {
                string[] strArray = this.Items[i].Split(new char[] { delimeter });
                if ((strArray.Length > 0) && (strArray[0] != ""))
                {
                    this.Items[i] = strArray[0] + suffix;
                }
            }
        }

        public void AddText(string string_0)
        {
            if (string_0 != null)
            {
                foreach (string str in string_0.Split(new char[] { ' ', ',', '\n', '\r' }))
                {
                    this.method_0(str.Trim(new char[] { ' ', '\n', '\r' }));
                }
            }
        }

        public void Clear()
        {
            this.Items.Clear();
        }

        private bool method_0(string string_0)
        {
            if (!this.Items.Contains(string_0) && (string_0 != ""))
            {
                this.Items.Add(string_0);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (this.Sort)
            {
                this.Items.Sort();
            }
            foreach (string str in this.Items)
            {
                builder.Append(str);
                builder.Append(this.Delimeter);
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        public int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        public string this[int index]
        {
            get
            {
                return this.Items[index];
            }
        }

        public string Text
        {
            get
            {
                return this.ToString();
            }
            set
            {
                this.Items.Clear();
                this.AddText(value);
            }
        }
    }
}

