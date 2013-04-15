namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using WealthLab;
    using WealthLab.DataProviders.MarketManagerService.Properties;

    public static class MarketManager
    {
        private static IDataHost idataHost_0;
        private static List<MarketInfo> list_0 = new List<MarketInfo>();
        private static List<MarketInfoSettings> list_1 = new List<MarketInfoSettings>();
        private static List<MarketInfo> list_2 = new List<MarketInfo>();
        private static List<string> list_3 = new List<string>();
        private static MarketInfo marketInfo_0;
        private static MarketManagerConfig marketManagerConfig_0;
        private static object object_0 = new object();
        private static object object_1 = new object();
        private static object object_2 = new object();
        private static string string_0 = Path.Combine(Application.UserAppDataPath, "Data");
        private static SupportedProviders supportedProviders_0;
        private static TimeSpan timeSpan_0 = TimeSpan.Zero;
        private static TimeSpan timeSpan_1 = new TimeSpan(0x17, 0x3b, 0x3b);
        private static System.Type type_0;

        static MarketManager()
        {
            lock (object_0)
            {
                smethod_4();
                smethod_13<List<MarketInfo>>(ref list_2, smethod_8());
                smethod_13<List<MarketInfo>>(ref list_0, smethod_9());
                smethod_0();
                list_0.Sort(new Comparison<MarketInfo>(MarketManager.smethod_1));
                smethod_13<List<MarketInfoSettings>>(ref list_1, smethod_11());
                supportedProviders_0 = new SupportedProviders();
                supportedProviders_0.Search();
                smethod_5();
                smethod_2();
            }
        }

        public static Bars ConvertBars(Bars bars, string providerName)
        {
            return smethod_34(bars, null, providerName);
        }

        public static Bars ConvertBars(Bars bars, DateTime startDate, DateTime endDate, int maxBars, string providerName)
        {
            return smethod_33(bars, null, startDate, endDate, maxBars, providerName);
        }

        public static MarketInfo GetMarketInfo(string symbol, string timeZoneName)
        {
            MarketInfo info = smethod_27(symbol);
            info.TimeZoneName = timeZoneName;
            return info;
        }

        public static MarketInfo GetMarketInfo(string symbol, string timeZoneName, string providerName)
        {
            MarketManagerInfoAttribute attribute = smethod_7(providerName);
            if (attribute == null)
            {
                return marketInfo_0;
            }
            if (!attribute.Enabled)
            {
                return marketInfo_0;
            }
            return GetMarketInfo(symbol, timeZoneName);
        }

        public static bool IsFilteredQuote(Quote quote, string providerName)
        {
            lock (object_1)
            {
                MarketManagerInfoAttribute attribute = smethod_7(providerName);
                if (attribute == null)
                {
                    return false;
                }
                MarketInfo info = smethod_27(quote.Symbol.ToUpper());
                if (info == null)
                {
                    return false;
                }
                if (!attribute.Enabled)
                {
                    return false;
                }
                return (smethod_40(quote.TimeStamp, info) || (smethod_39(quote.TimeStamp, info.SpecialHours) || smethod_38(quote.TimeStamp, info.Holidays)));
            }
        }

        private static void smethod_0()
        {
            for (int i = 0; i < list_2.Count; i++)
            {
                for (int j = list_0.Count - 1; j >= 0; j--)
                {
                    if (list_0[j].Name == list_2[i].Name)
                    {
                        list_0.RemoveAt(j);
                    }
                }
                list_0.Add(list_2[i]);
            }
        }

        private static int smethod_1(MarketInfo marketInfo_1, MarketInfo marketInfo_2)
        {
            return marketInfo_1.Name.CompareTo(marketInfo_2.Name);
        }

        private static string smethod_10()
        {
            return Path.Combine(string_0, "TimeZones.xml");
        }

        private static string smethod_11()
        {
            return Path.Combine(string_0, "MarketsSettings.xml");
        }

        private static string smethod_12()
        {
            return Path.Combine(string_0, "MarketManagerConfig.xml");
        }

        private static void smethod_13<T>(ref T gparam_0, string string_1)
        {
            if (File.Exists(string_1))
            {
                lock (object_0)
                {
                    XmlSerializer serializer = new XmlSerializer(gparam_0.GetType());
                    using (TextReader reader = new StreamReader(string_1))
                    {
                        gparam_0 = (T) serializer.Deserialize(reader);
                    }
                }
            }
        }

        private static void smethod_14<T>(T gparam_0, string string_1)
        {
            lock (object_0)
            {
                XmlSerializer serializer = new XmlSerializer(gparam_0.GetType());
                using (TextWriter writer = new StreamWriter(string_1))
                {
                    serializer.Serialize(writer, gparam_0);
                }
            }
        }

        internal static void smethod_15()
        {
            smethod_14<List<MarketInfo>>(list_0, smethod_9());
        }

        internal static void smethod_16()
        {
            smethod_14<List<MarketInfoSettings>>(list_1, smethod_11());
        }

        internal static MarketInfoSettings smethod_17(string string_1)
        {
            MarketInfoSettings item = null;
            using (List<MarketInfoSettings>.Enumerator enumerator = list_1.GetEnumerator())
            {
                MarketInfoSettings current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Name == string_1)
                    {
                        ///goto  Label_0031;
                        item = current;
                        break;
                    }
                }
            }
            if (item == null)
            {
                item = new MarketInfoSettings(string_1);
                list_1.Add(item);
                smethod_16();
            }
            return item;
        }

        private static int smethod_18(DateTime dateTime_0, DateTime dateTime_1)
        {
            return dateTime_0.CompareTo(dateTime_1);
        }

        internal static void smethod_19(MarketInfo marketInfo_1)
        {
            marketInfo_1.Holidays.Sort(new Comparison<DateTime>(MarketManager.smethod_18));
        }

        private static void smethod_2()
        {
            marketInfo_0 = new MarketInfo();
            marketInfo_0.OpenTimeNative = new DateTime(0x7d7, 9, 0x13, 0, 0, 0);
            marketInfo_0.CloseTimeNative = new DateTime(0x7d7, 9, 0x13, 0x17, 0x3b, 0x3b);
            marketInfo_0.TimeZoneName = "Eastern Standard Time";
        }

        private static int smethod_20(MarketSpecialHours marketSpecialHours_0, MarketSpecialHours marketSpecialHours_1)
        {
            return marketSpecialHours_0.Date.CompareTo(marketSpecialHours_1.Date);
        }

        internal static void smethod_21(MarketInfo marketInfo_1)
        {
            marketInfo_1.SpecialHours.Sort(new Comparison<MarketSpecialHours>(MarketManager.smethod_20));
        }

        internal static void smethod_22(MarketInfo marketInfo_1)
        {
            foreach (MarketInfoSettings settings in list_1)
            {
                settings.UseMarketByDefault = false;
            }
            if (marketInfo_1 != null)
            {
                smethod_17(marketInfo_1.Name).UseMarketByDefault = true;
            }
            smethod_16();
        }

        internal static Symbols smethod_23(Symbols symbols_0, MarketInfo marketInfo_1)
        {
            Symbols symbols = new Symbols();
            foreach (MarketInfo info in list_0)
            {
                if (info != marketInfo_1)
                {
                    Symbols symbols2 = smethod_17(info.Name).CheckForDuplicates(symbols_0);
                    symbols.AddText(symbols2.Text);
                }
            }
            return symbols;
        }

        internal static void smethod_24(Symbols symbols_0, Symbols symbols_1, MarketInfo marketInfo_1)
        {
            foreach (string str in symbols_1.Items)
            {
                symbols_0.Items.Remove(str);
            }
            smethod_17(marketInfo_1.Name).SymbolList = symbols_0;
        }

        internal static void smethod_25(Symbols symbols_0, MarketInfo marketInfo_1)
        {
            foreach (MarketInfo info in list_0)
            {
                if (info != marketInfo_1)
                {
                    smethod_24(smethod_17(info.Name).SymbolList, symbols_0, info);
                }
            }
        }

        private static string smethod_26(string string_1)
        {
            string str;
            string_1 = string_1.ToUpper().Trim();
            foreach (MarketInfoSettings settings in list_1)
            {
                if (settings.SymbolList.Items.Contains(string_1))
                {
                    return settings.Name;
                }
            }
            using (List<MarketInfoSettings>.Enumerator enumerator2 = list_1.GetEnumerator())
            {
                MarketInfoSettings current;
                while (enumerator2.MoveNext())
                {
                    current = enumerator2.Current;
                    if (current.UseMarketByDefault)
                    {
                        ///goto  Label_0083;
                        str = current.Name;
                        return str;
                    }
                }
                return null;
            }
        }

        internal static MarketInfo smethod_27(string string_1)
        {
            lock (object_2)
            {
                string str = smethod_26(string_1);
                foreach (MarketInfo info in list_0)
                {
                    if (info.Name == str)
                    {
                        return info;
                    }
                }
                return marketInfo_0;
            }
        }

        internal static MarketManagerControl smethod_28(System.Type type_1, IDataHost idataHost_1)
        {
            MarketManagerControl control = null;
            lock (object_0)
            {
                if (type_0 == null)
                {
                    type_0 = type_1;
                    idataHost_0 = idataHost_1;
                }
                if (type_0 == type_1)
                {
                    control = new MarketManagerControl();
                }
            }
            return control;
        }

        private static bool smethod_29(Class9 class9_0, DateTime dateTime_0, DateTime dateTime_1, int int_0)
        {
            return (((dateTime_1.Year != 0x270f) && (class9_0.dateTime_0 > dateTime_1.AddDays((double) (int_0 + 1)))) || ((dateTime_0.Year != 1) && (class9_0.dateTime_0 < dateTime_0.AddDays((double) -int_0))));
        }

        internal static bool smethod_3(MarketInfo marketInfo_1)
        {
            bool flag;
            using (List<MarketInfo>.Enumerator enumerator = list_2.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    MarketInfo current = enumerator.Current;
                    if (current.Name == marketInfo_1.Name)
                    {
                        ///goto  Label_0034;
                        return true;
                    }
                }
                return false;
            }
        }

        private static void smethod_30(Class10 class10_0, TimeZoneInfo timeZoneInfo_0, MarketInfo marketInfo_1, DateTime dateTime_0, DateTime dateTime_1, int int_0)
        {
            List<DateTime> list = null;
            List<MarketSpecialHours> list2 = null;
            bool flag = false;
            if (marketInfo_1 != null)
            {
                list = smethod_36(marketInfo_1, class10_0);
                list2 = smethod_37(marketInfo_1, class10_0);
                flag = (marketInfo_1.OpenTimeNative.TimeOfDay == timeSpan_0) && (marketInfo_1.CloseTimeNative.TimeOfDay == timeSpan_1);
            }
            int num = 0;
            bool flag2 = false;
            int num2 = 1;
            for (int i = class10_0.method_3().Count - 1; i >= 0; i--)
            {
                Class9 class2 = class10_0.method_3()[i];
                if ((((num2 % 0x7530) == 0) && (list != null)) && (list2 != null))
                {
                    smethod_35(class2.dateTime_0, list, list2);
                }
                num2++;
                if (smethod_29(class2, dateTime_0, dateTime_1, 3))
                {
                    class2.bool_0 = true;
                }
                else if (flag2)
                {
                    class2.bool_0 = true;
                }
                else if (smethod_29(class2, dateTime_0, dateTime_1, 0))
                {
                    class2.bool_0 = true;
                }
                else
                {
                    if (marketInfo_1 != null)
                    {
                        if (smethod_31(class2.dateTime_0, list2))
                        {
                            if (smethod_39(class2.dateTime_0, list2))
                            {
                                class2.bool_0 = true;
                            }
                        }
                        else if (!flag && smethod_40(class2.dateTime_0, marketInfo_1))
                        {
                            class2.bool_0 = true;
                        }
                        if (smethod_38(class2.dateTime_0, list))
                        {
                            class2.bool_0 = true;
                        }
                    }
                    if (!class2.bool_0)
                    {
                        num++;
                    }
                    if ((int_0 != 0) && (num >= int_0))
                    {
                        flag2 = true;
                    }
                }
            }
        }

        private static bool smethod_31(DateTime dateTime_0, List<MarketSpecialHours> list_4)
        {
            using (List<MarketSpecialHours>.Enumerator enumerator = list_4.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    MarketSpecialHours current = enumerator.Current;
                    if (dateTime_0.Date == current.Date)
                    {
                        ///goto  Label_0031;
                        return true;
                    }
                }
                return false;
            }
        }

        private static void smethod_32(Class10 class10_0, MarketInfo marketInfo_1, DateTime dateTime_0, DateTime dateTime_1, int int_0)
        {
            List<DateTime> list = null;
            if ((marketInfo_1 != null) && (class10_0.method_1() == BarScale.Daily))
            {
                list = smethod_36(marketInfo_1, class10_0);
            }
            int num2 = 0;
            bool flag = false;
            for (int i = class10_0.method_3().Count - 1; i >= 0; i--)
            {
                Class9 class2 = class10_0.method_3()[i];
                if (flag)
                {
                    class2.bool_0 = true;
                }
                else if (smethod_29(class2, dateTime_0, dateTime_1, 0))
                {
                    class2.bool_0 = true;
                }
                else
                {
                    if ((list != null) && smethod_38(class2.dateTime_0, list))
                    {
                        class2.bool_0 = true;
                    }
                    if (!class2.bool_0)
                    {
                        num2++;
                    }
                    if ((int_0 != 0) && (num2 >= int_0))
                    {
                        flag = true;
                    }
                }
            }
        }

        internal static Bars smethod_33(Bars bars_0, TimeZoneInfo timeZoneInfo_0, DateTime dateTime_0, DateTime dateTime_1, int int_0, string string_1)
        {
            lock (object_1)
            {
                Trace.WriteLine("------------------");
                if (bars_0.Count == 0)
                {
                    return bars_0;
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                MarketInfo info = smethod_27(bars_0.Symbol.ToUpper());
                MarketManagerInfoAttribute attribute = smethod_7(string_1);
                if ((attribute != null) && !attribute.Enabled)
                {
                    info = marketInfo_0;
                }
                Class10 class2 = new Class10(bars_0);
                class2.method_4();
                if ((((bars_0.Scale != BarScale.Daily) && (bars_0.Scale != BarScale.Weekly)) && ((bars_0.Scale != BarScale.Monthly) && (bars_0.Scale != BarScale.Quarterly))) && (bars_0.Scale != BarScale.Yearly))
                {
                    smethod_30(class2, timeZoneInfo_0, info, dateTime_0, dateTime_1, int_0);
                }
                else
                {
                    smethod_32(class2, info, dateTime_0, dateTime_1, int_0);
                }
                Bars bars2 = class2.method_5();
                stopwatch.Stop();
                Trace.WriteLine("ConvertBars:  " + stopwatch.ElapsedMilliseconds);
                return bars2;
            }
        }

        internal static Bars smethod_34(Bars bars_0, TimeZoneInfo timeZoneInfo_0, string string_1)
        {
            return smethod_33(bars_0, timeZoneInfo_0, DateTime.MinValue, DateTime.MaxValue, 0, string_1);
        }

        private static void smethod_35(DateTime dateTime_0, List<DateTime> list_4, List<MarketSpecialHours> list_5)
        {
            for (int i = list_4.Count - 1; i >= 0; i--)
            {
                if (list_4[i] > dateTime_0.Date)
                {
                    list_4.RemoveAt(i);
                }
            }
            for (int j = list_5.Count - 1; j >= 0; j--)
            {
                if (list_5[j].Date > dateTime_0.Date)
                {
                    list_5.RemoveAt(j);
                }
            }
        }

        private static List<DateTime> smethod_36(MarketInfo marketInfo_1, Class10 class10_0)
        {
            List<DateTime> list = new List<DateTime>();
            int num = class10_0.method_3().Count - 1;
            foreach (DateTime time in marketInfo_1.Holidays)
            {
                if ((time >= class10_0.method_3()[0].dateTime_0) && (time <= class10_0.method_3()[num].dateTime_0))
                {
                    list.Add(time);
                }
            }
            return list;
        }

        private static List<MarketSpecialHours> smethod_37(MarketInfo marketInfo_1, Class10 class10_0)
        {
            List<MarketSpecialHours> list = new List<MarketSpecialHours>();
            int num = class10_0.method_3().Count - 1;
            foreach (MarketSpecialHours hours in marketInfo_1.SpecialHours)
            {
                if ((hours.Date >= class10_0.method_3()[0].dateTime_0) && (hours.Date <= class10_0.method_3()[num].dateTime_0))
                {
                    list.Add(hours);
                }
            }
            return list;
        }

        private static bool smethod_38(DateTime dateTime_0, List<DateTime> list_4)
        {
            using (List<DateTime>.Enumerator enumerator = list_4.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    DateTime current = enumerator.Current;
                    if (dateTime_0.Date == current.Date)
                    {
                        ///goto  Label_0032;
                        return true;
                    }
                }
                return false;
            }
        }

        private static bool smethod_39(DateTime dateTime_0, List<MarketSpecialHours> list_4)
        {
            foreach (MarketSpecialHours hours in list_4)
            {
                if (dateTime_0.Date == hours.Date)
                {
                    if ((hours.OpenTimeNative.TimeOfDay == timeSpan_0) && (hours.CloseTimeNative.TimeOfDay == timeSpan_1))
                    {
                        return false;
                    }
                    if ((dateTime_0.TimeOfDay <= hours.OpenTimeNative.TimeOfDay) || (dateTime_0.TimeOfDay > hours.CloseTimeNative.TimeOfDay))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void smethod_4()
        {
            if (!Directory.Exists(string_0))
            {
                Directory.CreateDirectory(string_0);
            }
            if (!File.Exists(smethod_11()))
            {
                using (StreamWriter writer = new StreamWriter(smethod_11()))
                {
                    writer.Write(Resources.MarketsSettings);
                }
            }
        }

        private static bool smethod_40(DateTime dateTime_0, MarketInfo marketInfo_1)
        {
            if ((marketInfo_1.OpenTimeNative.TimeOfDay == timeSpan_0) && (marketInfo_1.CloseTimeNative.TimeOfDay == timeSpan_1))
            {
                return false;
            }
            if ((dateTime_0.TimeOfDay > marketInfo_1.OpenTimeNative.TimeOfDay) && (dateTime_0.TimeOfDay <= marketInfo_1.CloseTimeNative.TimeOfDay))
            {
                return false;
            }
            return true;
        }

        internal static void smethod_5()
        {
            marketManagerConfig_0 = new MarketManagerConfig();
            try
            {
                if (File.Exists(smethod_12()))
                {
                    smethod_13<MarketManagerConfig>(ref marketManagerConfig_0, smethod_12());
                    foreach (string str in marketManagerConfig_0.DisabledProviders)
                    {
                        MarketManagerInfoAttribute attribute = smethod_7(str);
                        if (attribute != null)
                        {
                            attribute.Enabled = false;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        internal static void smethod_6()
        {
            lock (object_0)
            {
                marketManagerConfig_0.DisabledProviders.Clear();
                foreach (MarketManagerInfoAttribute attribute in supportedProviders_0.Items)
                {
                    if (!attribute.Enabled)
                    {
                        marketManagerConfig_0.DisabledProviders.Add(attribute.Name);
                    }
                }
                smethod_14<MarketManagerConfig>(marketManagerConfig_0, smethod_12());
            }
        }

        private static MarketManagerInfoAttribute smethod_7(string string_1)
        {
            for (int i = 0; i < supportedProviders_0.Items.Count; i++)
            {
                if (supportedProviders_0.Items[i].Name == string_1)
                {
                    return supportedProviders_0.Items[i];
                }
            }
            return null;
        }

        private static string smethod_8()
        {
            return Path.Combine(Path.Combine(Application.StartupPath, "Data"), "Markets.xml");
        }

        private static string smethod_9()
        {
            return Path.Combine(string_0, "MarketManagerMarkets.xml");
        }

        internal static List<MarketInfo> ArrayOfMarketInfo
        {
            get
            {
                return list_0;
            }
            set
            {
                list_0 = value;
            }
        }

        internal static List<MarketInfoSettings> ArrayOfMarketInfoSettings
        {
            get
            {
                return list_1;
            }
            set
            {
                list_1 = value;
            }
        }

        internal static System.Type ControlOwner
        {
            get
            {
                return type_0;
            }
        }

        internal static SupportedProviders Providers
        {
            get
            {
                return supportedProviders_0;
            }
            set
            {
                supportedProviders_0 = value;
            }
        }
    }
}

