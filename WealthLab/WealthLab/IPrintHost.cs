namespace WealthLab
{
    using System;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public interface IPrintHost
    {
        PageSettings GetPageSettings();
        DataObject GetReportTemplate(string title);
        void Print(string title, Bitmap graphic, bool bUseDefaultDisclosure);
        void Print(string title, ListView listView_0, bool bUseDefaultDisclosure);
        bool ShowPrintDialog();
        bool ShowPrintPreview();
    }
}

