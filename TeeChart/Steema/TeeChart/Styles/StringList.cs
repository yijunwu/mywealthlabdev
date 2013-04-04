namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    [Serializable]
    public class StringList : List<string>
    {
        public StringList()
        {
        }

        public StringList(int capacity) : base(capacity)
        {
        }

        internal void Exchange(int a, int b)
        {
            string str = this[a];
            this[a] = this[b];
            this[b] = str;
        }

        public int IndexOf(string value, bool caseSensitive)
        {
            if (caseSensitive)
            {
                return base.IndexOf(value);
            }
            string str = value.ToUpper();
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].ToUpper() == str)
                {
                    return i;
                }
            }
            return -1;
        }

        internal void InsertLabel(string ALabel, int valueIndex)
        {
            while (base.Count < valueIndex)
            {
                base.Add("");
            }
            base.Insert(valueIndex, ALabel);
        }

        public string this[int index]
        {
            get
            {
                if (index >= base.Count)
                {
                    return "";
                }
                return base[index];
            }
            set
            {
                while (base.Count <= index)
                {
                    base.Add("");
                }
                base[index] = value;
            }
        }
    }
}

