namespace QWhale.Common
{
    using Microsoft.Win32;
    using System;

    public class TrialVersion
    {
        private const string SubKeyName = @"QWed.QWutil.1\CLSID";

        public static void CheckTrialVersion()
        {
        }

        private static int GetDaysLeft(int initialDaysCount)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"QWed.QWutil.1\CLSID", false);
            if (key == null)
            {
                WriteFirstUseDate();
                return initialDaysCount;
            }
            DateTime now = DateTime.Now;
            DateTime time2 = DateTime.Parse((string) key.GetValue(""));
            TimeSpan span = (TimeSpan) (now - time2);
            return (initialDaysCount - span.Days);
        }

        private static void WriteFirstUseDate()
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"QWed.QWutil.1\CLSID", true);
            if (key == null)
            {
                key = Registry.ClassesRoot.CreateSubKey(@"QWed.QWutil.1\CLSID");
            }
            key.SetValue("", DateTime.Now.ToShortDateString());
        }
    }
}

