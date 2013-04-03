using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

[DefaultMember("Item")]
internal class Class17
{
    public bool bool_0;
    public char char_0;
    private char[] char_1;
    private char[] char_2;
    public List<string> list_0;

    public Class17()
    {
        this.bool_0 = true;
        this.char_0 = ',';
        this.list_0 = new List<string>();
        this.char_1 = new char[] { ' ', ',', '\n', '\r' };
        this.char_2 = new char[] { ',', '\n', '\r' };
    }

    public Class17(List<string> list_1)
    {
        this.bool_0 = true;
        this.char_0 = ',';
        this.list_0 = new List<string>();
        this.char_1 = new char[] { ' ', ',', '\n', '\r' };
        this.char_2 = new char[] { ',', '\n', '\r' };
        this.list_0 = list_1;
    }

    public Class17(string string_0, Enum0 enum0_0)
    {
        this.bool_0 = true;
        this.char_0 = ',';
        this.list_0 = new List<string>();
        this.char_1 = new char[] { ' ', ',', '\n', '\r' };
        this.char_2 = new char[] { ',', '\n', '\r' };
        this.method_2(string_0, enum0_0);
    }

    public string method_0(int int_0)
    {
        return this.list_0[int_0];
    }

    public int method_1()
    {
        return this.list_0.Count;
    }

    public void method_2(string string_0, Enum0 enum0_0)
    {
        switch (enum0_0)
        {
            case Enum0.const_0:
                if (string_0 != null)
                {
                    foreach (string str3 in string_0.Split(this.char_2, StringSplitOptions.RemoveEmptyEntries))
                    {
                        this.method_4(str3.Trim(new char[] { ' ', '\n', '\r' }));
                    }
                }
                break;

            case Enum0.const_1:
            {
                if (string_0 == null)
                {
                    break;
                }
                string_0 = string_0.Trim();
                if (!string_0.Contains("\""))
                {
                    foreach (string str in string_0.Split(this.char_1, StringSplitOptions.RemoveEmptyEntries))
                    {
                        this.method_4(str.Trim(new char[] { ' ', '\n', '\r' }));
                    }
                    return;
                }
                int num4 = 1;
                string[] strArray2 = string_0.Split(new char[] { '"' });
                int index = 1;
                while (index < strArray2.Length)
                {
                    string str2 = strArray2[index];
                    this.method_4(str2.Trim(new char[] { ' ', '\n', '\r' }));
                    index += 2;
                }
                if (num4 == 0)
                {
                    index = 1;
                }
                else
                {
                    index = 0;
                }
                while (index < strArray2.Length)
                {
                    foreach (string str4 in strArray2[index].Split(this.char_1, StringSplitOptions.RemoveEmptyEntries))
                    {
                        this.method_4(str4.Trim(new char[] { ' ', '\n', '\r' }));
                    }
                    index += 2;
                }
                return;
            }
            default:
                return;
        }
    }

    public void method_3()
    {
        this.list_0.Clear();
    }

    private bool method_4(string string_0)
    {
        if (!this.list_0.Contains(string_0) && !string.IsNullOrEmpty(string_0))
        {
            this.list_0.Add(string_0);
            return true;
        }
        return false;
    }

    public void method_5(string string_0, char char_3)
    {
        string_0 = (string_0 == "None") ? "" : string_0;
        for (int i = 0; i < this.list_0.Count; i++)
        {
            string[] strArray = this.list_0[i].Split(new char[] { char_3 });
            if ((strArray.Length > 0) && (strArray[strArray.Length - 1] != ""))
            {
                this.list_0[i] = string_0 + strArray[strArray.Length - 1];
            }
        }
    }

    public void method_6(string string_0, char char_3)
    {
        string_0 = (string_0 == "None") ? "" : string_0;
        for (int i = 0; i < this.list_0.Count; i++)
        {
            string[] strArray = this.list_0[i].Split(new char[] { char_3 });
            if ((strArray.Length > 0) && (strArray[0] != ""))
            {
                this.list_0[i] = strArray[0] + string_0;
            }
        }
    }

    /* ///WYJ fix code from Reflector 
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
    } */
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (this.bool_0)
        {
            this.list_0.Sort();
        }
        foreach (string list0 in this.list_0)
        {
            stringBuilder.Append(list0);
            stringBuilder.Append(this.char_0);
        }
        if (stringBuilder.Length > 0)
        {
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }
        return stringBuilder.ToString();
    }
}

