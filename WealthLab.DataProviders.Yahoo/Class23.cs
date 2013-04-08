using System;

///WYJ fix, original name: Class23
internal static class Encoder
{
    private static char[] char_0 = new char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

    public static string smethod_0(string string_0)
    {
        foreach (char ch in char_0)
        {
            string oldValue = "%" + ((int) ch);
            string_0 = string_0.Replace(oldValue, ch.ToString());
        }
        return string_0;
    }

    ///WYJ fix, original signature, public static string smethod_1(string string_0)
    public static string encode(string string_0)
    {
        foreach (char ch in char_0)
        {
            string_0 = string_0.Replace(ch.ToString(), "%" + ((int) ch));
        }
        return string_0;
    }
}

