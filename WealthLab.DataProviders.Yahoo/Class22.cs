using System;
using WealthLab.DataProviders.Helper;

internal class Class22
{
    private Class17 class17_0 = new Class17();
    private Class17 class17_1 = new Class17();
    private ClassificationGroup classificationGroup_0;
    private string string_0;

    public Class22(ClassificationGroup classificationGroup_1)
    {
        this.classificationGroup_0 = classificationGroup_1;
    }

    public Class17 method_0()
    {
        return this.class17_0;
    }

    public Class17 method_1()
    {
        return this.class17_1;
    }

    private void method_2(string string_1, ClassificationGroup classificationGroup_1)
    {
        for (int i = 0; i < classificationGroup_1.Groups.Count; i++)
        {
            if ((classificationGroup_1.Groups[i].Type == "Symbols") && (classificationGroup_1.Groups[i].ID == string_1))
            {
                this.string_0 = classificationGroup_1.Groups[i].Symbols;
                return;
            }
            this.method_2(string_1, classificationGroup_1.Groups[i]);
        }
    }

    public Class17 method_3(Class17 class17_2, params string[] string_1)
    {
        Class21.smethod_8(new object[] { class17_2.ToString() });
        Class17 class2 = new Class17();
        foreach (string str in string_1)
        {
            this.string_0 = string.Empty;
            this.method_2(str, this.classificationGroup_0);
            class2.method_2(this.string_0, Enum0.const_1);
        }
        this.class17_0.list_0.Clear();
        this.class17_1.list_0.Clear();
        foreach (string str3 in class2.list_0)
        {
            if (!class17_2.list_0.Contains(str3))
            {
                this.class17_1.list_0.Add(str3);
            }
        }
        foreach (string str2 in class17_2.list_0)
        {
            if (!class2.list_0.Contains(str2))
            {
                this.class17_0.list_0.Add(str2);
            }
        }
        return class2;
    }
}

