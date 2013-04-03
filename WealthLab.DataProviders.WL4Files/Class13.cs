using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using WealthLab;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.WL4Files;

internal static class Class13
{
    [DllImport("WealthLab.DataProviders.WL4Files.Win32DBClient.dll")]
    private static extern void GetDataSets(string string_0, ref string string_1, ref bool bool_0);
    public static void smethod_0(string string_0, App app_0, ref Bars bars_0, DateTime dateTime_0, DateTime dateTime_1, int int_0)
    {
        switch (app_0)
        {
            case App.Dev:
                smethod_1(string_0, ref bars_0, dateTime_0, dateTime_1, int_0);
                return;

            case App.Pro:
                smethod_3(string_0, ref bars_0, dateTime_0, dateTime_1, int_0);
                return;
        }
    }

    private static void smethod_1(string string_0, ref Bars bars_0, DateTime dateTime_0, DateTime dateTime_1, int int_0)
    {
        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(string_0, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                if (reader.BaseStream.Length > 0L)
                {
                    int num = reader.ReadInt32();
                    int num2 = 0;
                    if (int_0 > 0)
                    {
                        num2 = num - int_0;
                        if (num2 > 0)
                        {
                            reader.BaseStream.Seek((long) (0x1c * num2), SeekOrigin.Current);
                        }
                        else
                        {
                            num2 = 0;
                        }
                    }
                    for (int i = num2; i < num; i++)
                    {
                        DateTime time = DateTime.FromOADate(reader.ReadDouble());
                        float num4 = reader.ReadSingle();
                        float num5 = reader.ReadSingle();
                        float num6 = reader.ReadSingle();
                        float num7 = reader.ReadSingle();
                        float num8 = reader.ReadSingle();
                        if ((time.Date >= dateTime_0.Date) && (time.Date <= dateTime_1.Date))
                        {
                            bars_0.Add(time, (double) num4, (double) num5, (double) num6, (double) num7, (double) num8);
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            string str = "WLD4 file read error.\r\nFile: " + string_0 + "\r\n" + exception.Message;
            Class14.smethod_4(TraceEventType.Error, str);
        }
    }

    public static List<string> smethod_10(string string_0)
    {
        List<string> list = new List<string>();
        foreach (string str in Directory.GetFiles(string_0, "*.wl"))
        {
            list.Add(Path.GetFileNameWithoutExtension(str));
        }
        return list;
    }

    public static List<string> smethod_11(string string_0, List<string> list_0)
    {
        List<string> list = smethod_10(string_0);
        List<string> list2 = new List<string>();
        foreach (string str in list_0)
        {
            if (list.Contains(str))
            {
                list2.Add(str);
            }
        }
        return list2;
    }

    public static App smethod_2(string string_0)
    {
        App dev = App.Dev;
        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(string_0, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                if (reader.ReadInt32() < 0)
                {
                    dev = App.Pro;
                }
            }
        }
        catch
        {
        }
        return dev;
    }

    private static void smethod_3(string string_0, ref Bars bars_0, DateTime dateTime_0, DateTime dateTime_1, int int_0)
    {
        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(string_0, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                if (reader.BaseStream.Length > 0L)
                {
                    int num = reader.ReadInt32();
                    if (num < 0)
                    {
                        foreach (char ch in reader.ReadChars(-num))
                        {
                            bars_0.SecurityName = bars_0.SecurityName + ch;
                        }
                        num = reader.ReadInt32();
                    }
                    int num11 = 0;
                    if (int_0 > 0)
                    {
                        num11 = num - int_0;
                        if (num11 > 0)
                        {
                            reader.BaseStream.Seek((long) (0x20 * num11), SeekOrigin.Current);
                        }
                        else
                        {
                            num11 = 0;
                        }
                    }
                    DataSeries series = bars_0.RegisterNamedSeries("Open Interest", false);
                    int num10 = 0;
                    for (int i = num11; i < num; i++)
                    {
                        DateTime time = DateTime.FromOADate(reader.ReadDouble());
                        float num3 = reader.ReadSingle();
                        float num4 = reader.ReadSingle();
                        float num5 = reader.ReadSingle();
                        float num6 = reader.ReadSingle();
                        float num7 = reader.ReadSingle();
                        float num8 = reader.ReadSingle();
                        if ((time.Date >= dateTime_0.Date) && (time.Date <= dateTime_1.Date))
                        {
                            bars_0.Add(time, (double) num3, (double) num4, (double) num5, (double) num6, (double) num7);
                            series[num10] = num8;
                            num10++;
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            string str = "WLP4 file read error.\r\nFile: " + string_0 + "\r\n" + exception.Message;
            Class14.smethod_4(TraceEventType.Error, str);
        }
    }

    public static string smethod_4(string string_0, string string_1)
    {
        return (Path.Combine(string_0, string_1) + ".wl");
    }

    public static List<WL4DataSource> smethod_5(string string_0)
    {
        Class14.smethod_8(new object[] { string_0 });
        List<WL4DataSource> list = new List<WL4DataSource>();
        string str = string.Empty;
        bool flag = false;
        GetDataSets(string_0, ref str, ref flag);
        if (!flag)
        {
            throw new DBException("Database connection failed. Check validity of the Database path.");
        }
        foreach (string str2 in str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
        {
            string[] strArray = str2.Split(new char[] { '~' });
            if (strArray.Length != 5)
            {
                Class14.smethod_2("Length != 5");
            }
            else
            {
                WL4DataSource item = new WL4DataSource {
                    ID = Convert.ToInt32(strArray[0]),
                    Name = strArray[1],
                    Type = strArray[2],
                    Location = strArray[3],
                    Details = strArray[4]
                };
                list.Add(item);
            }
        }
        list.Sort();
        return list;
    }

    public static string smethod_6(App app_0)
    {
        string str = smethod_7(app_0);
        if (str != null)
        {
            switch (app_0)
            {
                case App.Dev:
                    str = str + @"Data\";
                    goto Label_0058;

                case App.Pro:
                {
                    str = str + @"Data\";
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Fidelity Wealth-Lab Pro\Data\Fidelity\";
                    if (Directory.Exists(path))
                    {
                        str = path;
                    }
                    goto Label_0058;
                }
            }
            str = null;
        }
    Label_0058:;
        Class14.smethod_7(new object[] { app_0, (str == null) ? "" : str });
        return str;
    }

    private static string smethod_7(App app_0)
    {
        object obj2 = null;
        switch (app_0)
        {
            case App.Dev:
                obj2 = Registry.GetValue(Registry.CurrentUser + @"\Software\Wealth-Lab\Wealth-Lab Developer 3.0\", "Directory", null);
                break;

            case App.Pro:
                obj2 = Registry.GetValue(Registry.CurrentUser + @"\Software\Fidelity Investments\Wealth-Lab Pro\", "Directory", null);
                break;

            default:
                obj2 = null;
                break;
        }
        Class14.smethod_7(new object[] { app_0, (obj2 == null) ? "" : obj2 });
        return (obj2 as string);
    }

    public static List<string> smethod_8(string string_0, int int_0)
    {
        int num;
        List<WL4DataSource> list = smethod_5(string_0);
        Class12 class2 = new Class12();
        using (List<WL4DataSource>.Enumerator enumerator = list.GetEnumerator())
        {
            WL4DataSource current;
            while (enumerator.MoveNext())
            {
                current = enumerator.Current;
                if (current.ID == int_0)
                {
                    goto Label_0037;
                }
            }
            goto Label_0054;
        Label_0037:
            class2.method_2(current.Details);
        }
    Label_0054:
        num = class2.list_0.Count - 1;
        while (num >= 0)
        {
            if (class2.list_0[num].StartsWith("**"))
            {
                class2.list_0.RemoveAt(num);
            }
            num--;
        }
        return class2.list_0;
    }

    public static string smethod_9(App app_0)
    {
        string str = smethod_7(app_0);
        if (str != null)
        {
            switch (app_0)
            {
                case App.Dev:
                    str = str + @"Database\";
                    goto Label_0058;

                case App.Pro:
                {
                    str = str + @"Database\";
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Fidelity Wealth-Lab Pro\Database\";
                    if (Directory.Exists(path))
                    {
                        str = path;
                    }
                    goto Label_0058;
                }
            }
            str = null;
        }
    Label_0058:;
        Class14.smethod_7(new object[] { app_0, (str == null) ? "" : str });
        return str;
    }
}

