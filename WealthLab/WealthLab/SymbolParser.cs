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
        private List<string> symbolList;
        private string text;

        public SymbolParser()
        {
            this.symbolList = new List<string>();
            this.char_0 = new char[] { ' ', ',', '\n', '\r' };
            this.method_0();
        }

        public SymbolParser(IContainer container)
        {
            this.symbolList = new List<string>();
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
                return this.symbolList;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (value != null)
                {
                    this.text = value.Trim();
                    if (this.text.Contains("\""))
                    {
                        this.symbolList.Clear();
                        int num3 = 1;
                        string[] strArray = this.text.Split(new char[] { '"' });
                        int index = 1;
                        while (index < strArray.Length)
                        {
                            string item = strArray[index].Trim();
                            if ((item != "") && !this.symbolList.Contains(item))
                            {
                                this.symbolList.Add(item);
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
                                if ((str3.Trim(new char[] { ' ', '\n', '\r' }) != "") && !this.symbolList.Contains(str3))
                                {
                                    this.symbolList.Add(str3);
                                }
                            }
                            index += 2;
                        }
                    }
                    else
                    {
                        string[] strArray5 = this.text.Split(this.char_0, StringSplitOptions.RemoveEmptyEntries);
                        this.symbolList.Clear();
                        foreach (string str in strArray5)
                        {
                            if ((str.Trim(new char[] { ' ', '\n', '\r' }) != "") && !this.symbolList.Contains(str))
                            {
                                this.symbolList.Add(str);
                            }
                        }
                    }
                }
            }
        }
    }
}

