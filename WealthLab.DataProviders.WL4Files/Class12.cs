using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

[DefaultMember("Item")]
internal class Class12
{
    public bool bool_0;
    public char char_0;
    public List<string> list_0;

    public Class12()
    {
        this.bool_0 = true;
        this.char_0 = ',';
        this.list_0 = new List<string>();
    }

    public Class12(string string_0)
    {
        this.bool_0 = true;
        this.char_0 = ',';
        this.list_0 = new List<string>();
        this.method_2(string_0);
    }

    public Class12(List<string> list_1)
    {
        this.bool_0 = true;
        this.char_0 = ',';
        this.list_0 = new List<string>();
        this.list_0 = list_1;
    }

    public string method_0(int int_0)
    {
        return this.list_0[int_0];
    }

    public string method_1()
    {
        return this.ToString();
    }

    public void method_2(string string_0)
    {
        this.list_0.Clear();
        this.method_4(string_0);
    }

    public int method_3()
    {
        return this.list_0.Count;
    }

    public void method_4(string string_0)
    {
        if (string_0 != null)
        {
            foreach (string str in string_0.Split(new char[] { ' ', ',', '\n', '\r' }))
            {
                this.method_6(str.Trim(new char[] { ' ', '\n', '\r' }));
            }
        }
    }

    public void method_5()
    {
        this.list_0.Clear();
    }

    private bool method_6(string string_0)
    {
        if (!this.list_0.Contains(string_0) && (string_0 != ""))
        {
            this.list_0.Add(string_0);
            return true;
        }
        return false;
    }

    public void method_7(string string_0, char char_1)
    {
        string_0 = (string_0 == "None") ? "" : string_0;
        for (int i = 0; i < this.list_0.Count; i++)
        {
            string[] strArray = this.list_0[i].Split(new char[] { char_1 });
            if ((strArray.Length > 0) && (strArray[strArray.Length - 1] != ""))
            {
                this.list_0[i] = string_0 + strArray[strArray.Length - 1];
            }
        }
    }

    public void method_8(string string_0, char char_1)
    {
        string_0 = (string_0 == "None") ? "" : string_0;
        for (int i = 0; i < this.list_0.Count; i++)
        {
            string[] strArray = this.list_0[i].Split(new char[] { char_1 });
            if ((strArray.Length > 0) && (strArray[0] != ""))
            {
                this.list_0[i] = strArray[0] + string_0;
            }
        }
    }

    string object.ToString()
    {
        StringBuilder builder = new StringBuilder();
        if (this.bool_0)
        {
            this.list_0.Sort();
        }
        foreach (string str in this.list_0)
        {
            builder.Append(str);
            builder.Append(this.char_0);
        }
        if (builder.Length > 0)
        {
            builder.Remove(builder.Length - 1, 1);
        }
        return builder.ToString();
    }
}

