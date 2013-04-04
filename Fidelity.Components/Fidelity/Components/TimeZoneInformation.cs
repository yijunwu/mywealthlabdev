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

        private Struct3 method_0()
        {
            return new Struct3 { int_0 = this.struct2_0.int_0, struct1_0 = this.struct2_0.struct1_0, int_1 = this.struct2_0.int_1, struct1_1 = this.struct2_0.struct1_1, int_2 = this.struct2_0.int_2 };
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

        private static Struct1 smethod_0(DateTime dateTime_0)
        {
            Struct1 struct2;
            FILETIME filetime = new FILETIME {
                dwHighDateTime = (int) (dateTime_0.Ticks >> 0x20),
                dwLowDateTime = (int) (((ulong) dateTime_0.Ticks) & 0xffffffffL)
            };
            Struct4.FileTimeToSystemTime(ref filetime, out struct2);
            return struct2;
        }

        private static DateTime smethod_1(ref Struct1 struct1_0)
        {
            FILETIME filetime = new FILETIME();
            Struct4.SystemTimeToFileTime(ref struct1_0, out filetime);
            return new DateTime((filetime.dwHighDateTime << 0x20) | ((long) ((ulong) filetime.dwLowDateTime)));
        }

        public DateTime ToLocalTime(DateTime dateTime_0)
        {
            TimeZoneInformation.Struct1 struct1;
            TimeZoneInformation.Struct1 struct11 = TimeZoneInformation.smethod_0(dateTime_0);
            TimeZoneInformation.Struct3 struct3 = this.method_0();
            TimeZoneInformation.Struct4.SystemTimeToTzSpecificLocalTime(ref struct3, ref struct11, out struct1);
            return TimeZoneInformation.smethod_1(ref struct1);
        }

        public static DateTime ToLocalTime(DateTime dateTime_0, string targetTimeZoneName)
        {
            return GetTimeZone(targetTimeZoneName).ToLocalTime(dateTime_0);
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
            Struct1 struct2 = smethod_0(local);
            Struct3 struct3 = this.method_0();
            try
            {
                Struct1 struct4;
                Struct4.TzSpecificLocalTimeToSystemTime(ref struct3, ref struct2, out struct4);
                time = smethod_1(ref struct4);
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
                string standardName = TimeZone.CurrentTimeZone.StandardName;
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
        private struct Struct1
        {
            public ushort ushort_0;
            public ushort ushort_1;
            public ushort ushort_2;
            public ushort ushort_3;
            public ushort ushort_4;
            public ushort ushort_5;
            public ushort ushort_6;
            public ushort ushort_7;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct2
        {
            public int int_0;
            public int int_1;
            public int int_2;
            public TimeZoneInformation.Struct1 struct1_0;
            public TimeZoneInformation.Struct1 struct1_1;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        private struct Struct3
        {
            [MarshalAs(UnmanagedType.I4)]
            public int int_0;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
            public string string_0;
            public TimeZoneInformation.Struct1 struct1_0;
            [MarshalAs(UnmanagedType.I4)]
            public int int_1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
            public string string_1;
            public TimeZoneInformation.Struct1 struct1_1;
            [MarshalAs(UnmanagedType.I4)]
            public int int_2;
        }

        [StructLayout(LayoutKind.Sequential, Size=1)]
        private struct Struct4
        {
            private const string string_0 = "kernel32.dll";
            [DllImport("kernel32.dll")]
            public static extern uint GetTimeZoneInformation(out TimeZoneInformation.Struct3 struct3_0);
            [DllImport("kernel32.dll")]
            public static extern bool SystemTimeToTzSpecificLocalTime([In] ref TimeZoneInformation.Struct3 struct3_0, [In] ref TimeZoneInformation.Struct1 struct1_0, out TimeZoneInformation.Struct1 struct1_1);
            [DllImport("kernel32.dll")]
            public static extern bool SystemTimeToFileTime([In] ref TimeZoneInformation.Struct1 struct1_0, out FILETIME filetime_0);
            [DllImport("kernel32.dll")]
            public static extern bool FileTimeToSystemTime([In] ref FILETIME filetime_0, out TimeZoneInformation.Struct1 struct1_0);
            [DllImport("kernel32.dll")]
            public static extern bool TzSpecificLocalTimeToSystemTime([In] ref TimeZoneInformation.Struct3 struct3_0, [In] ref TimeZoneInformation.Struct1 struct1_0, out TimeZoneInformation.Struct1 struct1_1);
        }
    }
}

