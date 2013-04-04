namespace Steema.TeeChart
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;

    internal class FrAccessProvider : LicenseProvider
    {
        private string checkNo = "212022526396512817619191";

        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            FrAccess access = null;
            if (context != null)
            {
                if (context.UsageMode == LicenseUsageMode.Runtime)
                {
                    string savedLicenseKey = context.GetSavedLicenseKey(type, null);
                    if ((savedLicenseKey != null) && this.IsRuntimeKeyValid(savedLicenseKey, type))
                    {
                        access = new FrAccess(this, savedLicenseKey);
                    }
                }
                if (access != null)
                {
                    return access;
                }
                string key = Utils.DesignKeyV3();
                if ((key != null) && this.IsKeyValid(key, type))
                {
                    access = new FrAccess(this, string.Format("{0} is a licensed component.", type.FullName));
                }
                if (access != null)
                {
                    context.SetSavedLicenseKey(type, access.LicenseKey);
                }
            }
            return access;
        }

        internal virtual bool IsKeyValid(string key, Type type)
        {
            if ((key != null) && (key == "Steema.TeeChart.TChart is a licensed component."))
            {
                RegistryKey key2 = Registry.ClassesRoot.OpenSubKey(@"CLSID\{0E20716C-3949-4F41-832D-90171854F298}\xInt");
                if (key2 != null)
                {
                    string secCode = (string) key2.GetValue("", "");
                    if (this.wringer(secCode))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal virtual bool IsRuntimeKeyValid(string key, Type type)
        {
            return ((key != null) && key.StartsWith(string.Format("{0} is a licensed component.", type.FullName)));
        }

        private bool wringer(string secCode)
        {
            if ((((secCode.IndexOf('d') == -1) || (secCode.IndexOf("bB") == -1)) || ((secCode.IndexOf("yx") == -1) || (secCode.IndexOf("c") == -1))) || ((((secCode.IndexOf("q") == -1) || (secCode.IndexOf("i") == -1)) || ((secCode.IndexOf("o") == -1) || (secCode.IndexOf("s") == -1))) || ((secCode.IndexOf("ox") == -1) || (secCode.IndexOf("xt") == -1))))
            {
                return false;
            }
            string newValue = secCode.IndexOf('d').ToString();
            string str2 = (secCode.IndexOf('%') + 3).ToString();
            secCode = secCode.Replace("u", secCode.IndexOf('y').ToString()).Replace("d", newValue).Replace("sq", secCode.IndexOf("sq").ToString());
            secCode = secCode.Replace("bB", secCode.IndexOf('b').ToString()).Replace("b", secCode.IndexOf('b').ToString());
            secCode = secCode.Replace("a", (10 + secCode.IndexOf('c')).ToString());
            secCode = secCode.Replace("yx", secCode.IndexOf('y').ToString()).Replace("c", secCode.IndexOf('c').ToString());
            secCode = secCode.Replace("xt", secCode.LastIndexOfAny(new char[] { '3', '4', 'y' }).ToString()).Replace("i", secCode.IndexOf('i').ToString());
            secCode = secCode.Insert(secCode.IndexOf("ox"), secCode.IndexOf('o').ToString());
            secCode = secCode.Replace("o", str2).Replace("e", secCode.IndexOf("e").ToString()).Replace("x", str2);
            return (secCode.CompareTo(this.checkNo) == 0);
        }
    }
}

