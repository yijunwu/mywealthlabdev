namespace WealthLab
{
    using System.Windows.Forms;

    public interface IWizard
    {
        UserControl WizardFirstPage();
        UserControl WizardNextPage(UserControl currentPage);
        UserControl WizardPreviousPage(UserControl currentPage);
    }
}

