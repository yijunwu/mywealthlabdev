using QWhale.Editor;
using QWhale.Syntax.Parsers;
using QWhale.SyntaxSettings;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;
using WealthLabPro;

internal class Class60
{
    private CsParser csParser_0;
    private Hashtable hashtable_0 = new Hashtable();
    private Hashtable hashtable_1 = new Hashtable();
    private string string_0;
    private string string_1;
    private SyntaxEdit syntaxEdit_0;

    public Class60(SyntaxEdit syntaxEdit_1, CsParser csParser_1)
    {
        this.syntaxEdit_0 = syntaxEdit_1;
        this.csParser_0 = csParser_1;
        if ((this.syntaxEdit_0 != null) && (this.csParser_0 != null))
        {
            this.method_7(MainModule.Instance.DataPath + @"\EditorSettings.xml");
            this.method_5(MainModule.Instance.DataPath + @"\EditorDefaultSettings.xml");
            this.method_0();
            this.method_8();
        }
    }

    public bool method_0()
    {
        bool flag = false;
        this.method_1();
        if (File.Exists(this.method_6()))
        {
            this.syntaxEdit_0.SyntaxSettings.LoadFile(this.method_6());
            this.syntaxEdit_0.SyntaxSettings.ApplyToEdit(this.syntaxEdit_0);
            return true;
        }
        if (File.Exists(this.method_4()))
        {
            this.syntaxEdit_0.SyntaxSettings.LoadFile(this.method_4());
            this.syntaxEdit_0.SyntaxSettings.ApplyToEdit(this.syntaxEdit_0);
            flag = true;
        }
        return flag;
    }

    private void method_1()
    {
        string str = "EditorVersion";
        string defaultValue = "1.47";
        string str3 = "1.51";
        if (MainModule.Instance.Settings.Get(str, defaultValue) == defaultValue)
        {
            if (File.Exists(this.method_6()))
            {
                this.method_2(this.method_6());
            }
            else if (File.Exists(this.method_4()))
            {
                this.method_2(this.method_4());
            }
            MainModule.Instance.Settings.Set(str, str3);
        }
    }

    private void method_10(string string_2)
    {
        Assembly assembly = Assembly.LoadFrom(string_2);
        this.method_11(assembly, true);
    }

    private void method_11(Assembly assembly_0, bool bool_0)
    {
        if (assembly_0 != null)
        {
            string name = assembly_0.GetName().Name;
            if (!this.hashtable_0.Contains(name))
            {
                this.hashtable_0.Add(name, assembly_0);
                if (bool_0)
                {
                    this.csParser_0.RegisterAssembly(assembly_0);
                }
                foreach (string str2 in this.method_9(assembly_0))
                {
                    ArrayList list = null;
                    if (this.hashtable_1.Contains(str2))
                    {
                        list = (ArrayList) this.hashtable_1[str2];
                        list.Add(name);
                    }
                    else
                    {
                        list = new ArrayList();
                        list.Add(assembly_0.FullName);
                        this.hashtable_1.Add(str2, list);
                    }
                    if (bool_0)
                    {
                        this.csParser_0.RegisterNamespace(str2);
                    }
                }
            }
        }
    }

    private void method_12(string string_2)
    {
        Assembly assembly = (Assembly) this.hashtable_0[string_2];
        if (assembly != null)
        {
            this.hashtable_0.Remove(string_2);
            this.csParser_0.UnregisterAssembly(assembly, true);
            foreach (string str in this.method_9(assembly))
            {
                ArrayList list2 = (ArrayList) this.hashtable_1[str];
                if (list2 != null)
                {
                    list2.Remove(string_2);
                }
                if (list2.Count == 0)
                {
                    this.csParser_0.UnregisterNamespace(str);
                }
            }
        }
    }

    private void method_2(string string_2)
    {
        string[] strArray = string_2.Split(new char[] { '\\' });
        string[] strArray2 = strArray[strArray.Length - 1].Split(new char[] { '.' });
        File.Copy(string_2, MainModule.Instance.DataPath + @"\" + strArray2[0] + ".147.xml", true);
        XmlDocument doc = new XmlDocument();
        doc.Load(string_2);
        new SyntaxSettingsConverter().UpdateSettings(ref doc);
        doc.Save(string_2);
    }

    public void method_3()
    {
        this.syntaxEdit_0.SyntaxSettings.SaveFile(this.method_6());
    }

    public string method_4()
    {
        return this.string_0;
    }

    public void method_5(string string_2)
    {
        this.string_0 = string_2;
    }

    public string method_6()
    {
        return this.string_1;
    }

    public void method_7(string string_2)
    {
        this.string_1 = string_2;
    }

    private void method_8()
    {
        string appPath = MainModule.Instance.AppPath;
        this.method_10(appPath + @"\WealthLab.dll");
        this.method_10(appPath + @"\WealthLab.Indicators.dll");
        this.method_10(appPath + @"\WealthLab.ChartStyles.dll");
    }

    private ArrayList method_9(Assembly assembly_0)
    {
        ArrayList list = new ArrayList();
        Type[] types = assembly_0.GetTypes();
        for (int i = 0; i < types.Length; i++)
        {
            if ((!list.Contains(types[i].Namespace) && (types[i].Namespace != string.Empty)) && (types[i].Namespace != null))
            {
                list.Add(types[i].Namespace);
            }
        }
        return list;
    }
}

