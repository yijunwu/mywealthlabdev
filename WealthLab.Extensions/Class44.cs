using System;

internal static class Class44
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

    public static string smethod_1(string string_0)
    {
        foreach (char ch in char_0)
        {
            string_0 = string_0.Replace(ch.ToString(), "%" + ((int) ch));
        }
        return string_0;
    }
}

