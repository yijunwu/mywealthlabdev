namespace WealthLab
{
    using System;
    using System.Drawing;

    public interface IAuthenticationHost
    {
        void AddMenuItem(string text, string mainMenuItemText, string subMenuItemText, ClickMenuItem callback);
        void AddMenuItem(string text, string mainMenuItemText, string subMenuItemText, ClickMenuItem callback, Image itemImage);
        void AddWorkspaceMenuItem(string workspace);
        void LoginSuccessful();
        void LoginUnsuccessful();
        bool NavigateToThirdPartySite(string string_0);
        void UpdateDaysBeforeNextLogin(int days);

        DateTime GracePeriodEndDate { get; }

        bool InGracePeriod { get; }
    }
}

