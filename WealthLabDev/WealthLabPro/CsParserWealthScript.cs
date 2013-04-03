namespace WealthLabPro
{
    using QWhale.Syntax.Parsers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(CsParserWealthScript), "Images.CsParser.bmp"), ToolboxItem(true)]
    public class CsParserWealthScript : CsParser
    {
        private Color color_0 = Color.Crimson;
        private Color color_1 = Color.DarkRed;
        private Hashtable hashtable_0 = new Hashtable();
        private Hashtable hashtable_1 = new Hashtable();
        private int int_0;
        private int int_1;
        private string string_0 = "WSMethods";
        private string string_1 = "WSProperties";

        protected override int GetLexerStyle(int token)
        {
            if (this.method_1())
            {
                this.method_3(this.string_0, ref this.int_0);
                return this.int_0;
            }
            if (this.method_2())
            {
                this.method_3(this.string_1, ref this.int_1);
                return this.int_1;
            }
            return base.GetLexerStyle(token);
        }

        protected override void InitReswords()
        {
            base.InitReswords();
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "WealthLab.dll");
            if (File.Exists(path))
            {
                System.Type type = Assembly.LoadFile(path).GetType("WealthLab.WealthScript");
                if (null != type)
                {
                    List<string> list2 = new List<string>();
                    List<string> list = new List<string>();
                    foreach (MemberInfo info in type.GetMembers())
                    {
                        if (info.MemberType == MemberTypes.Method)
                        {
                            if (!list2.Contains(info.Name))
                            {
                                list2.Add(info.Name);
                                this.hashtable_0.Add(info.Name, 1);
                            }
                        }
                        else if ((info.MemberType == MemberTypes.Property) && !list.Contains(info.Name))
                        {
                            list.Add(info.Name);
                            this.hashtable_1.Add(info.Name, 1);
                        }
                    }
                }
            }
        }

        protected override void InitStyles()
        {
            base.InitStyles();
            this.int_0 = this.LexStylesCount;
            base.AddStyle(this.string_0, this.color_0);
            this.int_1 = this.LexStylesCount;
            base.AddStyle(this.string_1, this.color_1);
        }

        private string method_0(int int_2)
        {
            return this.Scheme.Styles[int_2].Name;
        }

        private bool method_1()
        {
            object obj2 = this.hashtable_0[this.TokenString];
            return (obj2 != null);
        }

        private bool method_2()
        {
            object obj2 = this.hashtable_1[this.TokenString];
            return (obj2 != null);
        }

        private void method_3(string string_2, ref int int_2)
        {
            if ((this.LexStylesCount <= int_2) || ((this.LexStylesCount > int_2) && (this.method_0(int_2) != string_2)))
            {
                int_2 = 0;
                for (int i = this.LexStylesCount - 1; i >= 0; i--)
                {
                    if (string_2 == this.method_0(i))
                    {
                        int_2 = i;
                        return;
                    }
                }
            }
        }

        private int LexStylesCount
        {
            get
            {
                return this.Scheme.Styles.Count;
            }
        }
    }
}

