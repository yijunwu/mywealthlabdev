namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Dialogs;
    using QWhale.Editor.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Drawing.Printing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class Printing : IPrinting
    {
        private PrintOptions allowedOptions;
        private bool dialogCalled;
        private IEditPageHeader footer;
        private IEditPageHeader header;
        private IntPtr hookHandle = IntPtr.Zero;
        private HookHandler hookProc;
        private IntPtr lastWnd = IntPtr.Zero;
        private PrintOptions options;
        private ISyntaxEdit owner;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Windows.Forms.PrintDialog printDialog;
        private EditorPrintDocument printDocument;
        private System.Drawing.Printing.PrinterSettings printerSettings;
        private IPrintOptionsDialog printOptionsDialog;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
        private bool showPrintOptionsDialog;

        [Browsable(false)]
        public event CreatePrintEditEvent CreatePrintEdit;

        [Browsable(false)]
        public event EventHandler Initialized;

        public Printing(ISyntaxEdit owner)
        {
            this.owner = owner;
            this.printerSettings = new System.Drawing.Printing.PrinterSettings();
            this.printDocument = new EditorPrintDocument(this, this.printerSettings);
            this.printDocument.PrinterSettings = this.printerSettings;
            this.options = EditConsts.DefaultPrintOptions;
            this.allowedOptions = EditConsts.DefaultPrintOptions;
        }

        public virtual void Assign(IPrinting source)
        {
            this.Footer = source.Footer;
            this.Header = source.Header;
            this.Options = source.Options;
            this.AllowedOptions = source.AllowedOptions;
        }

        private IntPtr DialogHook(int ncode, IntPtr wParam, IntPtr lParam)
        {
            if ((ncode == 0) && !this.dialogCalled)
            {
                OSUtils.CWPSTRUCT cwpstruct = (OSUtils.CWPSTRUCT) Marshal.PtrToStructure(lParam, typeof(OSUtils.CWPSTRUCT));
                if (cwpstruct.message == 0x110)
                {
                    this.lastWnd = IntPtr.Zero;
                    OSUtils.EnumChildWindows(cwpstruct.hwnd, new EnumChildProc(this.EnumDialogChilds), IntPtr.Zero);
                    if (this.lastWnd != IntPtr.Zero)
                    {
                        OSUtils.SetText(this.lastWnd, StringConsts.PrintOptions);
                    }
                    this.dialogCalled = true;
                }
            }
            return OSUtils.CallNextHook(this.hookHandle, ncode, wParam, lParam);
        }

        private void DoHelpRequest(object sender, EventArgs e)
        {
            if ((this.showPrintOptionsDialog && (this.PrintOptionsDialog != null)) && (this.ExecutePrintOptionsDialog() == DialogResult.OK))
            {
                this.Options = this.PrintOptionsDialog.Options;
            }
        }

        private bool EnumDialogChilds(IntPtr hwnd, IntPtr lParam)
        {
            if (string.Compare(OSUtils.GetText(hwnd), "&Help", true) == 0)
            {
                OSUtils.SetText(hwnd, StringConsts.PrintOptionsButtonText);
                this.lastWnd = IntPtr.Zero;
                return false;
            }
            if (string.Compare(OSUtils.GetClassName(hwnd), "Button", true) == 0)
            {
                this.lastWnd = hwnd;
            }
            return true;
        }

        public virtual DialogResult ExecutePageSetupDialog()
        {
            return this.ExecutePageSetupDialog(null);
        }

        public virtual DialogResult ExecutePageSetupDialog(IWin32Window ownerwnd)
        {
            this.printDocument.Init(this.owner);
            try
            {
                return ((ownerwnd != null) ? this.PageSetupDialog.ShowDialog(ownerwnd) : this.PageSetupDialog.ShowDialog());
            }
            catch
            {
                return DialogResult.Cancel;
            }
        }

        public virtual DialogResult ExecutePrintDialog()
        {
            return this.ExecutePrintDialog(null);
        }

        public virtual DialogResult ExecutePrintDialog(IWin32Window ownerwnd)
        {
            this.printDocument.Init(this.owner, true);
            try
            {
                DialogResult result;
                if (this.PrintDialog.ShowHelp)
                {
                    this.HookPrintDialog();
                    try
                    {
                        result = (ownerwnd != null) ? this.PrintDialog.ShowDialog(ownerwnd) : this.PrintDialog.ShowDialog();
                    }
                    finally
                    {
                        this.UnhookPrintDialog();
                    }
                }
                else
                {
                    result = (ownerwnd != null) ? this.PrintDialog.ShowDialog(ownerwnd) : this.PrintDialog.ShowDialog();
                }
                return result;
            }
            catch
            {
                return DialogResult.Cancel;
            }
        }

        public virtual DialogResult ExecutePrintOptionsDialog()
        {
            return this.ExecutePrintOptionsDialog(null);
        }

        public virtual DialogResult ExecutePrintOptionsDialog(IWin32Window ownerwnd)
        {
            if (this.PrintOptionsDialog == null)
            {
                return DialogResult.None;
            }
            this.PrintOptionsDialog.Options = this.Options;
            this.PrintOptionsDialog.AllowedOptions = this.AllowedOptions;
            this.PrintOptionsDialog.FileName = this.owner.Source.FileName;
            if (ownerwnd == null)
            {
                return this.PrintOptionsDialog.ShowDialog();
            }
            return this.PrintOptionsDialog.ShowDialog(ownerwnd);
        }

        public virtual DialogResult ExecutePrintPreviewDialog()
        {
            return this.ExecutePrintPreviewDialog(null);
        }

        public virtual DialogResult ExecutePrintPreviewDialog(IWin32Window ownerwnd)
        {
            this.printDocument.Init(this.owner);
            try
            {
                return ((ownerwnd != null) ? this.PrintPreviewDialog.ShowDialog(ownerwnd) : this.PrintPreviewDialog.ShowDialog());
            }
            catch
            {
                return DialogResult.Cancel;
            }
        }

        private void HookPrintDialog()
        {
            this.dialogCalled = false;
            this.hookProc = new HookHandler(this.DialogHook);
            this.hookHandle = OSUtils.SetWndProcHook(this.hookProc);
        }

        protected virtual void OnAllowedOptionsChanged()
        {
        }

        public virtual ISyntaxEdit OnCreatePrintEdit()
        {
            if (this.CreatePrintEdit != null)
            {
                CreatePrintEditEventArgs e = new CreatePrintEditEventArgs();
                this.CreatePrintEdit(this, e);
                if (e.PrintEdit != null)
                {
                    return e.PrintEdit;
                }
            }
            return new SyntaxEdit();
        }

        protected virtual void OnFooterChanged()
        {
        }

        protected virtual void OnHeaderChanged()
        {
        }

        public virtual void OnInitialized()
        {
            if (this.Initialized != null)
            {
                this.Initialized(this, EventArgs.Empty);
            }
        }

        protected virtual void OnOptionsChanged()
        {
        }

        protected virtual void OnPrintOptionsDialogChanged()
        {
        }

        protected virtual void OnShowPrintOptionsDialogChanged()
        {
        }

        public virtual void Print()
        {
            this.printDocument.Init(this.owner);
            this.printDocument.Print();
        }

        public virtual void ResetAllowedOptions()
        {
            this.AllowedOptions = EditConsts.DefaultPrintOptions;
        }

        public virtual void ResetOptions()
        {
            this.Options = EditConsts.DefaultPrintOptions;
        }

        public bool ShouldSerializeAllowedOptions()
        {
            return (this.allowedOptions != EditConsts.DefaultPrintOptions);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.options != EditConsts.DefaultPrintOptions);
        }

        private void UnhookPrintDialog()
        {
            if (this.hookHandle != IntPtr.Zero)
            {
                OSUtils.ReleaseHook(this.hookHandle);
                this.hookHandle = IntPtr.Zero;
                this.hookProc = null;
            }
        }

        [Description("Gets or sets Print Options dialog options, that can be changed by user."), Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
        public virtual PrintOptions AllowedOptions
        {
            get
            {
                if (!this.owner.Selection.IsEmpty)
                {
                    return (this.allowedOptions | PrintOptions.PrintSelection);
                }
                return this.allowedOptions;
            }
            set
            {
                if (this.allowedOptions != value)
                {
                    this.allowedOptions = value;
                    this.OnAllowedOptionsChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEditPageHeader Footer
        {
            get
            {
                if (this.footer == null)
                {
                    this.footer = new PageHeader(this.owner.Pages.DefaultPage, this.owner);
                }
                return this.footer;
            }
            set
            {
                if (this.footer != value)
                {
                    this.footer = value;
                    this.OnFooterChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEditPageHeader Header
        {
            get
            {
                if (this.header == null)
                {
                    this.header = new PageHeader(this.owner.Pages.DefaultPage, this.owner);
                }
                return this.header;
            }
            set
            {
                if (this.header != value)
                {
                    this.header = value;
                    this.OnHeaderChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets \"PrintOptions\" that determines printing behaviour.")]
        public virtual PrintOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.OnOptionsChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual System.Windows.Forms.PageSetupDialog PageSetupDialog
        {
            get
            {
                if (this.pageSetupDialog == null)
                {
                    this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
                    this.pageSetupDialog.Document = this.printDocument;
                    this.pageSetupDialog.PageSettings = new PageSettings(this.printerSettings);
                    this.pageSetupDialog.PrinterSettings = this.printerSettings;
                }
                return this.pageSetupDialog;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual System.Windows.Forms.PrintDialog PrintDialog
        {
            get
            {
                if (this.printDialog == null)
                {
                    this.printDialog = new System.Windows.Forms.PrintDialog();
                    this.printDialog.Document = this.printDocument;
                    this.printDialog.PrinterSettings = this.printerSettings;
                    this.printDialog.AllowSomePages = true;
                    this.printDialog.ShowHelp = true;
                    this.printDialog.HelpRequest += new EventHandler(this.DoHelpRequest);
                }
                return this.printDialog;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual System.Drawing.Printing.PrintDocument PrintDocument
        {
            get
            {
                return this.printDocument;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual System.Drawing.Printing.PrinterSettings PrinterSettings
        {
            get
            {
                return this.printerSettings;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IPrintOptionsDialog PrintOptionsDialog
        {
            get
            {
                if (this.printOptionsDialog == null)
                {
                    this.printOptionsDialog = new DlgPrintOptions();
                }
                return this.printOptionsDialog;
            }
            set
            {
                if (this.printOptionsDialog != value)
                {
                    this.printOptionsDialog = value;
                    this.OnPrintOptionsDialogChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual System.Windows.Forms.PrintPreviewDialog PrintPreviewDialog
        {
            get
            {
                if (this.printPreviewDialog == null)
                {
                    this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
                    this.printPreviewDialog.Document = this.printDocument;
                }
                return this.printPreviewDialog;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlPrintingInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [DefaultValue(false)]
        public virtual bool ShowPrintOptionsDialog
        {
            get
            {
                return this.showPrintOptionsDialog;
            }
            set
            {
                if (this.showPrintOptionsDialog != value)
                {
                    this.showPrintOptionsDialog = value;
                    this.OnShowPrintOptionsDialogChanged();
                }
            }
        }
    }
}

