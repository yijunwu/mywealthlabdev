namespace WealthLab.StrategyProviders.Properties
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

        [DefaultSettingValue("http://www.wealth-lab.com/WebServices/StrategyWebservice.asmx"), ApplicationScopedSetting, DebuggerNonUserCode, SpecialSetting(SpecialSetting.WebServiceUrl)]
        public string WealthLab_StrategyProviders_WLWebServices_StrategyWebservice
        {
            get
            {
                return (string) this["WealthLab_StrategyProviders_WLWebServices_StrategyWebservice"];
            }
        }
    }
}

