using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

///WYJ fix, original name: Class17
[DefaultMember("Item")]
internal class SymbolList   ///WYJ note, Symbol list used in symbols page of new dataset wizard
{
    public bool sortNeeded;
    public char delimiterForDisplay;
    private char[] char_1;
    private char[] char_2;
    public List<string> list;

    public SymbolList()
    {
        this.sortNeeded = true;
        this.delimiterForDisplay = ',';
        this.list = new List<string>();
        this.char_1 = new char[] { ' ', ',', '\n', '\r' };
        this.char_2 = new char[] { ',', '\n', '\r' };
    }

    public SymbolList(List<string> list_1)
    {
        this.sortNeeded = true;
        this.delimiterForDisplay = ',';
        this.list = new List<string>();
        this.char_1 = new char[] { ' ', ',', '\n', '\r' };
        this.char_2 = new char[] { ',', '\n', '\r' };
        this.list = list_1;
    }

    public SymbolList(string string_0, Enum0 enum0_0)
    {
        this.sortNeeded = true;
        this.delimiterForDisplay = ',';
        this.list = new List<string>();
        this.char_1 = new char[] { ' ', ',', '\n', '\r' };
        this.char_2 = new char[] { ',', '\n', '\r' };
        this.AddSymbols(string_0, enum0_0);
    }

    ///WYJ fix, original name: method_0
    public string getSymbolAt(int i)
    {
        return this.list[i];
    }

    public int Count()
    {
        return this.list.Count;
    }

    ///WYJ fix, original name: method_2
    public void AddSymbols(string string_0, Enum0 enum0_0)
    {
        switch (enum0_0)
        {
            case Enum0.const_0:  ///WYJ note, do not check quote mark, split by ',', '\n', '\r', no ' ' as delimiter
                if (string_0 != null)
                {
                    foreach (string str3 in string_0.Split(this.char_2, StringSplitOptions.RemoveEmptyEntries))
                    {
                        this.add(str3.Trim(new char[] { ' ', '\n', '\r' }));
                    }
                }
                break;

            case Enum0.const_1:  ///WYJ note, check quote mark, split by ' ', ',', '\n', '\r', ' ' is one of the delimiters
            {
                if (string_0 != null)
                {
                    string_0 = string_0.Trim();
                    if (!string_0.Contains("\""))
                    {
                        foreach (string str in string_0.Split(this.char_1, StringSplitOptions.RemoveEmptyEntries))
                        {
                            this.add(str.Trim(new char[] { ' ', '\n', '\r' }));
                        }
                        //return;
                    }
                    else
                    {
                        int num4 = 1;
                        string[] strArray2 = string_0.Split(new char[] { '"' });
                        int index = 1;
                        while (index < strArray2.Length)
                        {
                            string str2 = strArray2[index];
                            this.add(str2.Trim(new char[] { ' ', '\n', '\r' }));
                            index += 2;
                        }

                        index = 0; ///WYJ fix, original: index = (num4 == 0) ? 1 : 0;

                        while (index < strArray2.Length)
                        {
                            foreach (string str4 in strArray2[index].Split(this.char_1, StringSplitOptions.RemoveEmptyEntries))
                            {
                                this.add(str4.Trim(new char[] { ' ', '\n', '\r' }));
                            }
                            index += 2;
                        }
                    }
                    
                    //return;
                }
                break;
                
            }
            default:
                return;
        }
    }

    ///WYJ fix, original name: method_3
    public void clear()
    {
        this.list.Clear();
    }

    ///WYJ fix, original name: method_4
    private bool add(string str)   ///WYJ note, add to list_0
    {
        if (!this.list.Contains(str) && !string.IsNullOrEmpty(str))
        {
            this.list.Add(str);
            return true;
        }
        return false;
    }

    ///WYJ note, looks like method to add prefix, but it's never used
    public void AddPrefixToSymbolNames(string prefix, char dotChar)
    {
        prefix = (prefix == "None") ? "" : prefix;
        for (int i = 0; i < this.list.Count; i++)
        {
            string[] strArray = this.list[i].Split(new char[] { dotChar });
            if ((strArray.Length > 0) && (strArray[strArray.Length - 1] != "")) //Split the symbol with prefix into two parts: prefix and symbol 
            {
                this.list[i] = prefix + strArray[strArray.Length - 1];
            }
        }
    }

    ///WYJ fix, original name: method_6
    public void AddSurfixToSymbolNames(string surfix, char dotChar)
    {
        surfix = (surfix == "None") ? "" : surfix;
        for (int i = 0; i < this.list.Count; i++)
        {
            string[] strArray = this.list[i].Split(new char[] { dotChar });  //Split the symbol with surfix into two parts: symbol and surfix
            if ((strArray.Length > 0) && (strArray[0] != ""))
            {
                this.list[i] = strArray[0] + surfix;
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
        if (this.sortNeeded)
        {
            this.list.Sort();
        }
        foreach (string list0 in this.list)
        {
            stringBuilder.Append(list0);
            stringBuilder.Append(this.delimiterForDisplay);
        }
        if (stringBuilder.Length > 0)
        {
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }
        return stringBuilder.ToString();
    }
}

