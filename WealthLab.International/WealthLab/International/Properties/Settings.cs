namespace WealthLab.International.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), CompilerGenerated]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [SpecialSetting(SpecialSetting.WebServiceUrl), DebuggerNonUserCode, DefaultSettingValue("https://www.wealth-lab.com/WebServices/CustomersWebService.asmx"), ApplicationScopedSetting]
        public string WealthLab_International_localhost_CustomersWebService
        {
            get
            {
                return (string) this["WealthLab_International_localhost_CustomersWebService"];
            }
        }
    }
}

