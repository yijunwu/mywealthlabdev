namespace Fidelity.Components
{
    using System;
    using System.Windows.Forms;

    public interface ICustomSettings
    {
        void ChangeSettings(UserControl userControl_0);
        UserControl GetSettingsUI();
        void ReadSettings(ISettingsHost host);
        void WriteSettings(ISettingsHost host);
    }
}

