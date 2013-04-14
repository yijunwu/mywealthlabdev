namespace Fidelity.Components
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [Serializable]
    public class TimeZoneInformation
    {
        private string daylightName;
        private string displayName;
        private string name;
        private string standardName;
        private Struct2 struct2_0;
        private static readonly List<TimeZoneInformation> timeZones = new List<TimeZoneInformation>();

        static TimeZoneInformation()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones"))
            {
                foreach (string str in key.GetSubKeyNames())
                {
                    using (RegistryKey key2 = key.OpenSubKey(str))
                    {
                        TimeZoneInformation item = new TimeZoneInformation {
                            name = str,
                            displayName = (string) key2.GetValue("Display"),
                            standardName = (string) key2.GetValue("Std"),
                            daylightName = (string) key2.GetValue("Dlt")
                        };
                        object obj2 = key2.GetValue("Tzi");
                        if (obj2 != null)
                        {
                            item.method_1((byte[]) obj2);
                        }
                        timeZones.Add(item);
                    }
                }
            }
        }

        private TimeZoneInformation()
        {
        }

        public static TimeZoneInformation GetTimeZone(string standardTimeZoneName)
        {
            if (standardTimeZoneName == null)
            {
                standardTimeZoneName = ".";
            }
            if (standardTimeZoneName == ".")
            {
                standardTimeZoneName = CurrentTimeZone.Name;
            }
            foreach (TimeZoneInformation information in TimeZones)
            {
                if (information.Name.Equals(standardTimeZoneName, StringComparison.OrdinalIgnoreCase))
                {
                    return information;
                }
            }
            throw new ArgumentException("standardTimeZoneName not found.");
        }

        ///WYJ fix, original name: method_0
        private TimeZone getTimeZone()
        {
            return new TimeZone { bias = this.struct2_0.int_0, standardDate = this.struct2_0.struct1_0, standardBias = this.struct2_0.int_1, daylightDate = this.struct2_0.struct1_1, daylightBias = this.struct2_0.int_2 };
        }

        private void method_1(byte[] byte_0)
        {
            if (byte_0.Length != Marshal.SizeOf(this.struct2_0))
            {
                throw new ArgumentException("Information size is incorrect", "info");
            }
            GCHandle handle = GCHandle.Alloc(byte_0, GCHandleType.Pinned);
            try
            {
                this.struct2_0 = (Struct2) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Struct2));
            }
            finally
            {
                handle.Free();
            }
        }

        ///WYJ fix, original name: smethod_0
        private static SystemTime toSystemTime(DateTime dt)
        {
            SystemTime st = new SystemTime();
            /*
             Struct1 struct2;
             FILETIME filetime = new FILETIME {
                dwHighDateTime = (int) (dateTime_0.Ticks >> 0x20),
                dwLowDateTime = (int) (((ulong) dateTime_0.Ticks) & 0x0000ffffL) ///WYJ fix, original: 0xffffffffL
            };
            Struct4.FileTimeToSystemTime(ref filetime, out struct2); 
            return struct2; */
            dt = dt.ToLocalTime();
            st.year = Convert.ToUInt16(dt.Year);
            st.month = Convert.ToUInt16(dt.Month);
            st.dayOfWeek = Convert.ToUInt16(dt.DayOfWeek);
            st.day = Convert.ToUInt16(dt.Day);
            st.hour = Convert.ToUInt16(dt.Hour);
            st.minute = Convert.ToUInt16(dt.Minute);
            st.second = Convert.ToUInt16(dt.Second);
            st.milliseconds = Convert.ToUInt16(dt.Millisecond);
            return st;
        }

        /*private static DateTime smethod_1(ref SystemTime struct1_0)
        {
            FILETIME filetime = new FILETIME();
            Struct4.SystemTimeToFileTime(ref struct1_0, out filetime);
            return new DateTime((filetime.dwHighDateTime << 0x20) | ((long) ((ulong) filetime.dwLowDateTime)));
        } */

        private static DateTime fromSystemDate(ref SystemTime st, DateTimeKind kind)  ///WYJ fix, original name: smethod_1
        {
            DateTime dt = new DateTime(st.year, st.month, st.day, st.hour, st.minute, st.second, st.milliseconds, kind);
            return dt;
        }

        public DateTime ToLocalTime(DateTime dateTime_0)
        {
            TimeZoneInformation.SystemTime result;
            TimeZoneInformation.SystemTime systemTime = TimeZoneInformation.toSystemTime(dateTime_0);
            TimeZoneInformation.TimeZone timezone = this.getTimeZone();
            TimeZoneInformation.Struct4.SystemTimeToTzSpecificLocalTime(ref timezone, ref systemTime, out result);
            return TimeZoneInformation.fromSystemDate(ref result, DateTimeKind.Local);
        }

        public static DateTime ToLocalTime(DateTime utcTime, string targetTimeZoneName)
        {
            return GetTimeZone(targetTimeZoneName).ToLocalTime(utcTime);
        }

        public static DateTime ToLocalTime(string sourceTimeZoneName, DateTime localTime, string targetTimeZoneName)
        {
            return ToLocalTime(ToUniversalTime(sourceTimeZoneName, localTime), targetTimeZoneName);
        }

        public override string ToString()
        {
            return this.standardName;
        }

        public DateTime ToUniversalTime(DateTime local)
        {
            DateTime time;
            SystemTime struct2 = toSystemTime(local);
            TimeZone struct3 = this.getTimeZone();
            try
            {
                SystemTime struct4;
                Struct4.TzSpecificLocalTimeToSystemTime(ref struct3, ref struct2, out struct4);
                time = fromSystemDate(ref struct4, DateTimeKind.Utc);
            }
            catch (EntryPointNotFoundException exception)
            {
                throw new NotSupportedException("This method is not supported on this operating system", exception);
            }
            return time;
        }

        public static DateTime ToUniversalTime(string standardTimeZoneName, DateTime local)
        {
            return GetTimeZone(standardTimeZoneName).ToUniversalTime(local);
        }

        public static TimeZoneInformation CurrentTimeZone
        {
            get
            {
                string standardName = System.TimeZone.CurrentTimeZone.StandardName;
                foreach (TimeZoneInformation information in TimeZones)
                {
                    if (information.StandardName.Equals(standardName, StringComparison.OrdinalIgnoreCase))
                    {
                        return information;
                    }
                }
                throw new ArgumentException("CurrentTimeZone not found.");
            }
        }

        public int DaylightBias
        {
            get
            {
                return -(this.struct2_0.int_0 + this.struct2_0.int_2);
            }
        }

        public string DaylightName
        {
            get
            {
                return this.daylightName;
            }
        }

        public TimeSpan DaylightOffset
        {
            get
            {
                return TimeSpan.FromMinutes((double) this.DaylightBias);
            }
        }

        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int StandardBias
        {
            get
            {
                return -(this.struct2_0.int_0 + this.struct2_0.int_1);
            }
        }

        public string StandardName
        {
            get
            {
                return this.standardName;
            }
        }

        public TimeSpan StandardOffset
        {
            get
            {
                return TimeSpan.FromMinutes((double) this.StandardBias);
            }
        }

        public static string[] TimeZoneNames
        {
            get
            {
                List<string> list = new List<string>();
                foreach (TimeZoneInformation information in timeZones)
                {
                    list.Add(information.StandardName);
                }
                return list.ToArray();
            }
        }

        public static List<TimeZoneInformation> TimeZones
        {
            get
            {
                return new List<TimeZoneInformation>(timeZones);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SystemTime  ///WYJ fix, original name: Struct1
        {
            public ushort year; ///WYJ fix, original name: ushort_0
            public ushort month; ///WYJ fix, original name: ushort_1
            public ushort dayOfWeek; ///WYJ fix, original name: ushort_2
            public ushort day; ///WYJ fix, original name: ushort_3
            public ushort hour; ///WYJ fix, original name: ushort_4
            public ushort minute; ///WYJ fix, original name: ushort_5
            public ushort second; ///WYJ fix, original name: ushort_6
            public ushort milliseconds; ///WYJ fix, original name: ushort_7
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct2
        {
            public int int_0;
            public int int_1;
            public int int_2;
            public TimeZoneInformation.SystemTime struct1_0;
            public TimeZoneInformation.SystemTime struct1_1;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        private struct TimeZone  ///WYJ fix, original name: Struct3
        {
            [MarshalAs(UnmanagedType.I4)]
            public int bias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
            public string standardName;
            public TimeZoneInformation.SystemTime standardDate;
            [MarshalAs(UnmanagedType.I4)]
            public int standardBias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
            public string daylightName;
            public TimeZoneInformation.SystemTime daylightDate;
            [MarshalAs(UnmanagedType.I4)]
            public int daylightBias;
        }

        [StructLayout(LayoutKind.Sequential, Size=1)]
        private struct Struct4
        {
            private const string string_0 = "kernel32.dll";
            [DllImport("kernel32.dll")]
            public static extern uint GetTimeZoneInformation(out TimeZoneInformation.TimeZone struct3_0);
            [DllImport("kernel32.dll")]
            public static extern bool SystemTimeToTzSpecificLocalTime([In] ref TimeZoneInformation.TimeZone struct3_0, [In] ref TimeZoneInformation.SystemTime struct1_0, out TimeZoneInformation.SystemTime struct1_1);
            [DllImport("kernel32.dll")]
            public static extern bool SystemTimeToFileTime([In] ref TimeZoneInformation.SystemTime struct1_0, out FILETIME filetime_0);
            [DllImport("kernel32.dll")]
            public static extern bool FileTimeToSystemTime([In] ref FILETIME filetime_0, out TimeZoneInformation.SystemTime struct1_0);
            [DllImport("kernel32.dll")]
            public static extern bool TzSpecificLocalTimeToSystemTime([In] ref TimeZoneInformation.TimeZone struct3_0, [In] ref TimeZoneInformation.SystemTime struct1_0, out TimeZoneInformation.SystemTime struct1_1);
        }
    }
}

