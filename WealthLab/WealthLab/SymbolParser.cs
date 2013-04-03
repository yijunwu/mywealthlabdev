namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(SymbolParser), "SymbolParser")]
    public class SymbolParser : Component
    {
        private char[] char_0;
        private IContainer icontainer_0;
        private List<string> list_0;
        private string string_0;

        public SymbolParser()
        {
            this.list_0 = new List<string>();
            this.char_0 = new char[] { ' ', ',', '\n', '\r' };
            this.method_0();
        }

        public SymbolParser(IContainer container)
        {
            this.list_0 = new List<string>();
            this.char_0 = new char[] { ' ', ',', '\n', '\r' };
            container.Add(this);
            this.method_0();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        public List<string> Symbols
        {
            get
            {
                return this.list_0;
            }
        }

        public string Text
        {
            get
            {
                return this.string_0;
            }
            set
            {
                if (value != null)
                {
                    this.string_0 = value.Trim();
                    if (this.string_0.Contains("\""))
                    {
                        this.list_0.Clear();
                        int num3 = 1;
                        string[] strArray = this.string_0.Split(new char[] { '"' });
                        int index = 1;
                        while (index < strArray.Length)
                        {
                            string item = strArray[index].Trim();
                            if ((item != "") && !this.list_0.Contains(item))
                            {
                                this.list_0.Add(item);
                            }
                            index += 2;
                        }
                        if (num3 == 0)
                        {
                            index = 1;
                        }
                        else
                        {
                            index = 0;
                        }
                        while (index < strArray.Length)
                        {
                            foreach (string str3 in strArray[index].Split(this.char_0, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if ((str3.Trim(new char[] { ' ', '\n', '\r' }) != "") && !this.list_0.Contains(str3))
                                {
                                    this.list_0.Add(str3);
                                }
                            }
                            index += 2;
                        }
                    }
                    else
                    {
                        string[] strArray5 = this.string_0.Split(this.char_0, StringSplitOptions.RemoveEmptyEntries);
                        this.list_0.Clear();
                        foreach (string str in strArray5)
                        {
                            if ((str.Trim(new char[] { ' ', '\n', '\r' }) != "") && !this.list_0.Contains(str))
                            {
                                this.list_0.Add(str);
                            }
                        }
                    }
                }
            }
        }
    }
}

