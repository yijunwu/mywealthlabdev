using System;
using System.Collections.Generic;

internal static class Class56
{
    private static string string_0 = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";

    public static string smethod_0(int int_0)
    {
        int num = int_0 % 0x20;
        if ((int_0 - num) == 0)
        {
            char ch = string_0[0];
            char ch2 = string_0[num];
            return (ch.ToString() + ch2.ToString());
        }
        char ch3 = string_0[((int_0 - num) / 0x20) % 0x20];
        char ch4 = string_0[num];
        return (ch3.ToString() + ch4.ToString());
    }

    public static int smethod_1(string string_1)
    {
        return ((string_0.IndexOf(string_1[0]) * 0x20) + string_0.IndexOf(string_1[1]));
    }

    public static string smethod_2(byte[] byte_0)
    {
        string str = string.Empty;
        for (int i = 0; i < byte_0.Length; i++)
        {
            if ((i % 2) == 0)
            {
                int num4 = byte_0[i];
                int num3 = 0;
                if ((i + 1) < byte_0.Length)
                {
                    num3 = byte_0[i + 1];
                }
                int num2 = num4 + num3;
                if (num3 > num4)
                {
                    num2 += 0x200;
                }
                str = str + smethod_0(num2);
            }
        }
        return str;
    }

    public static string smethod_3(string string_1, int int_0)
    {
        string str = (string) string_1.Clone();
        List<int> list = new List<int>();
        for (int i = 1; i < (string_1.Length + 1); i++)
        {
            if (((i % int_0) == 0) && (i != string_1.Length))
            {
                list.Add(i);
            }
        }
        for (int j = list.Count - 1; j >= 0; j--)
        {
            str = str.Insert(list[j], "-");
        }
        return str;
    }
}

