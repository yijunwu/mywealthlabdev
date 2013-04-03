namespace WealthLab
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Web;

    public static class SymbolFileNameConverter
    {
        private static Hashtable hashtable_0;
        private static Hashtable hashtable_1;

        static SymbolFileNameConverter()
        {
            if ((hashtable_0 == null) && (hashtable_1 == null))
            {
                hashtable_1 = new Hashtable();
                foreach (char ch in Path.GetInvalidFileNameChars())
                {
                    string str = HttpUtility.UrlEncode(ch.ToString());
                    hashtable_1.Add(ch.ToString(), str);
                }
                hashtable_1.Add(".", "%2E");
                hashtable_1.Add("+", "%2B");
                hashtable_1.Add("#", "%23");
                hashtable_0 = new Hashtable();
                hashtable_0.Add("CON", "%43%4F%4E");
                hashtable_0.Add("PRN", "%50%52%4E");
                hashtable_0.Add("AUX", "%41%55%58");
                hashtable_0.Add("CLOCK$", "%43%4C%4F%43%4B%24");
                hashtable_0.Add("NUL", "%4E%55%4C");
                hashtable_0.Add("COM1", "%43%4F%4D%31");
                hashtable_0.Add("COM2", "%43%4F%4D%32");
                hashtable_0.Add("COM3", "%43%4F%4D%33");
                hashtable_0.Add("COM4", "%43%4F%4D%34");
                hashtable_0.Add("COM5", "%43%4F%4D%35");
                hashtable_0.Add("COM6", "%43%4F%4D%36");
                hashtable_0.Add("COM7", "%43%4F%4D%37");
                hashtable_0.Add("COM8", "%43%4F%4D%38");
                hashtable_0.Add("COM9", "%43%4F%4D%39");
                hashtable_0.Add("LPT1", "%4C%50%54%31");
                hashtable_0.Add("LPT2", "%4C%50%54%32");
                hashtable_0.Add("LPT3", "%4C%50%54%33");
                hashtable_0.Add("LPT4", "%4C%50%54%34");
                hashtable_0.Add("LPT5", "%4C%50%54%35");
                hashtable_0.Add("LPT6", "%4C%50%54%36");
                hashtable_0.Add("LPT7", "%4C%50%54%37");
                hashtable_0.Add("LPT8", "%4C%50%54%38");
                hashtable_0.Add("LPT9", "%4C%50%54%39");
            }
        }

        public static string FileNameToSymbol(string fileName)
        {
            string empty = string.Empty;
            if (fileName != string.Empty)
            {
                empty = Path.GetFileNameWithoutExtension(fileName);
                bool flag = false;
                IDictionaryEnumerator enumerator = SymbolFileNameConverter.hashtable_0.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            DictionaryEntry current = (DictionaryEntry)enumerator.Current;
                            if (empty.Equals(current.Value as string))
                            {
                                empty = current.Key as string;
                                flag = true;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                if (!flag)
                {
                    foreach (DictionaryEntry hashtable1 in SymbolFileNameConverter.hashtable_1)
                    {
                        if (!empty.Contains(hashtable1.Value as string))
                        {
                            continue;
                        }
                        empty = empty.Replace(hashtable1.Value as string, hashtable1.Key as string);
                    }
                }
            }
            return empty;
        }


        public static string StripInvalidChars(string name)
        {
            foreach (DictionaryEntry entry in hashtable_1)
            {
                if (name.Contains(entry.Key as string))
                {
                    name = name.Replace(entry.Key as string, string.Empty);
                }
            }
            return name;
        }

        public static string SymbolToFileName(string symbol)
        {
            bool flag = false;
            foreach (DictionaryEntry hashtable1 in SymbolFileNameConverter.hashtable_1)
            {
                if (!symbol.Contains(hashtable1.Key as string))
                {
                    continue;
                }
                symbol = symbol.Replace(hashtable1.Key as string, hashtable1.Value as string);
                flag = true;
            }
            if (!flag)
            {
                IDictionaryEnumerator enumerator = SymbolFileNameConverter.hashtable_0.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            DictionaryEntry current = (DictionaryEntry)enumerator.Current;
                            if (symbol.Equals(current.Key as string))
                            {
                                symbol = current.Value as string;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            return symbol;
        }


        public static string UrlEncode(string name)
        {
            foreach (DictionaryEntry entry in hashtable_1)
            {
                if (name.Contains(entry.Key as string))
                {
                    name = name.Replace(entry.Key as string, entry.Value as string);
                }
            }
            return name;
        }
    }
}

