namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Dialogs;
    using System;
    using System.Drawing.Printing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IPrinting
    {
        event CreatePrintEditEvent CreatePrintEdit;

        event EventHandler Initialized;

        void Assign(IPrinting source);
        DialogResult ExecutePageSetupDialog();
        DialogResult ExecutePageSetupDialog(IWin32Window ownerwnd);
        DialogResult ExecutePrintDialog();
        DialogResult ExecutePrintDialog(IWin32Window ownerwnd);
        DialogResult ExecutePrintOptionsDialog();
        DialogResult ExecutePrintOptionsDialog(IWin32Window ownerwnd);
        DialogResult ExecutePrintPreviewDialog();
        DialogResult ExecutePrintPreviewDialog(IWin32Window ownerwnd);
        ISyntaxEdit OnCreatePrintEdit();
        void OnInitialized();
        void Print();
        void ResetAllowedOptions();
        void ResetOptions();

        PrintOptions AllowedOptions { get; set; }

        IEditPageHeader Footer { get; set; }

        IEditPageHeader Header { get; set; }

        PrintOptions Options { get; set; }

        System.Windows.Forms.PageSetupDialog PageSetupDialog { get; }

        System.Windows.Forms.PrintDialog PrintDialog { get; }

        System.Drawing.Printing.PrintDocument PrintDocument { get; }

        System.Drawing.Printing.PrinterSettings PrinterSettings { get; }

        IPrintOptionsDialog PrintOptionsDialog { get; set; }

        System.Windows.Forms.PrintPreviewDialog PrintPreviewDialog { get; }

        ISerializationInfo SerializationInfo { get; set; }

        bool ShowPrintOptionsDialog { get; set; }
    }
}

