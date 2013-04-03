namespace WealthLab.Extensions.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
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

        [DefaultSettingValue("http://www.wealth-lab.com/WebServices/ExtensionManager.asmx"), SpecialSetting(SpecialSetting.WebServiceUrl), ApplicationScopedSetting, DebuggerNonUserCode]
        public string WealthLab_Extensions_net_wli5_ExtensionWebservice
        {
            get
            {
                return (string) this["WealthLab_Extensions_net_wli5_ExtensionWebservice"];
            }
        }
    }
}

