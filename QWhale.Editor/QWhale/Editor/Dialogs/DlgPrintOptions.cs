namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgPrintOptions : Form, IPrintOptionsDialog
    {
        private PrintOptions allowedOptions;
        private Button btCancel;
        private Button btOk;
        private CheckBox chbDisplayProgress;
        private CheckBox chbLineNumbers;
        private CheckBox chbPageNumbers;
        private CheckBox chbPrintSelection;
        private CheckBox chbUseColors;
        private CheckBox chbUseFooter;
        private CheckBox chbUseHeader;
        private CheckBox chbUseSyntax;
        private CheckBox chbWordWrap;
        private Container components;
        private string fileName;
        private GroupBox gbFileToPrint;
        private GroupBox gbOptions;
        private Label laFileName;
        private PrintOptions options;

        public DlgPrintOptions()
        {
            this.InitializeComponent();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            this.OptionsFromControls();
        }

        private void ControlsFromOptions()
        {
            if ((this.FileName != string.Empty) && (this.FileName != null))
            {
                this.laFileName.Text = this.laFileName.Text + ": " + this.FileName;
            }
            this.chbPrintSelection.Enabled = (this.AllowedOptions & PrintOptions.PrintSelection) > PrintOptions.None;
            this.chbUseHeader.Enabled = (this.AllowedOptions & PrintOptions.UseHeader) > PrintOptions.None;
            this.chbUseFooter.Enabled = (this.AllowedOptions & PrintOptions.UseFooter) > PrintOptions.None;
            this.chbLineNumbers.Enabled = (this.AllowedOptions & PrintOptions.LineNumbers) > PrintOptions.None;
            this.chbPageNumbers.Enabled = (this.AllowedOptions & PrintOptions.PageNumbers) > PrintOptions.None;
            this.chbWordWrap.Enabled = (this.AllowedOptions & PrintOptions.WordWrap) > PrintOptions.None;
            this.chbUseSyntax.Enabled = (this.AllowedOptions & PrintOptions.UseSyntax) > PrintOptions.None;
            this.chbUseColors.Enabled = (this.AllowedOptions & PrintOptions.UseColors) > PrintOptions.None;
            this.chbDisplayProgress.Enabled = (this.AllowedOptions & PrintOptions.DisplayProgress) > PrintOptions.None;
            this.chbPrintSelection.Checked = (this.Options & PrintOptions.PrintSelection) > PrintOptions.None;
            this.chbUseHeader.Checked = (this.Options & PrintOptions.UseHeader) > PrintOptions.None;
            this.chbUseFooter.Checked = (this.Options & PrintOptions.UseFooter) > PrintOptions.None;
            this.chbLineNumbers.Checked = (this.Options & PrintOptions.LineNumbers) > PrintOptions.None;
            this.chbPageNumbers.Checked = (this.Options & PrintOptions.PageNumbers) > PrintOptions.None;
            this.chbWordWrap.Checked = (this.Options & PrintOptions.WordWrap) > PrintOptions.None;
            this.chbUseSyntax.Checked = (this.Options & PrintOptions.UseSyntax) > PrintOptions.None;
            this.chbUseColors.Checked = (this.Options & PrintOptions.UseColors) > PrintOptions.None;
            this.chbDisplayProgress.Checked = (this.Options & PrintOptions.DisplayProgress) > PrintOptions.None;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DlgPrintOptions_Load(object sender, EventArgs e)
        {
            this.LoadFromResource();
            this.ControlsFromOptions();
        }

        private void InitializeComponent()
        {
            this.btCancel = new Button();
            this.btOk = new Button();
            this.gbOptions = new GroupBox();
            this.chbDisplayProgress = new CheckBox();
            this.chbUseColors = new CheckBox();
            this.chbUseSyntax = new CheckBox();
            this.chbWordWrap = new CheckBox();
            this.chbPageNumbers = new CheckBox();
            this.chbLineNumbers = new CheckBox();
            this.chbUseFooter = new CheckBox();
            this.chbUseHeader = new CheckBox();
            this.gbFileToPrint = new GroupBox();
            this.chbPrintSelection = new CheckBox();
            this.laFileName = new Label();
            this.gbOptions.SuspendLayout();
            this.gbFileToPrint.SuspendLayout();
            base.SuspendLayout();
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.FlatStyle = FlatStyle.System;
            this.btCancel.Location = new Point(260, 0xe0);
            this.btCancel.Name = "btCancel";
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "Cancel";
            this.btOk.DialogResult = DialogResult.OK;
            this.btOk.FlatStyle = FlatStyle.System;
            this.btOk.Location = new Point(0xb0, 0xe0);
            this.btOk.Name = "btOk";
            this.btOk.TabIndex = 8;
            this.btOk.Text = "Ok";
            this.btOk.Click += new EventHandler(this.btOk_Click);
            this.gbOptions.Controls.Add(this.chbDisplayProgress);
            this.gbOptions.Controls.Add(this.chbUseColors);
            this.gbOptions.Controls.Add(this.chbUseSyntax);
            this.gbOptions.Controls.Add(this.chbWordWrap);
            this.gbOptions.Controls.Add(this.chbPageNumbers);
            this.gbOptions.Controls.Add(this.chbLineNumbers);
            this.gbOptions.Controls.Add(this.chbUseFooter);
            this.gbOptions.Controls.Add(this.chbUseHeader);
            this.gbOptions.FlatStyle = FlatStyle.System;
            this.gbOptions.Location = new Point(8, 0x60);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new Size(0x148, 120);
            this.gbOptions.TabIndex = 6;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options:";
            this.chbDisplayProgress.FlatStyle = FlatStyle.System;
            this.chbDisplayProgress.Location = new Point(120, 0x5b);
            this.chbDisplayProgress.Name = "chbDisplayProgress";
            this.chbDisplayProgress.Size = new Size(0xc0, 0x19);
            this.chbDisplayProgress.TabIndex = 7;
            this.chbDisplayProgress.Text = "Display progress";
            this.chbUseColors.FlatStyle = FlatStyle.System;
            this.chbUseColors.Location = new Point(120, 0x42);
            this.chbUseColors.Name = "chbUseColors";
            this.chbUseColors.Size = new Size(0xc0, 0x19);
            this.chbUseColors.TabIndex = 6;
            this.chbUseColors.Text = "Color print";
            this.chbUseSyntax.FlatStyle = FlatStyle.System;
            this.chbUseSyntax.Location = new Point(120, 0x29);
            this.chbUseSyntax.Name = "chbUseSyntax";
            this.chbUseSyntax.Size = new Size(0xc0, 0x19);
            this.chbUseSyntax.TabIndex = 5;
            this.chbUseSyntax.Text = "Syntax print";
            this.chbWordWrap.FlatStyle = FlatStyle.System;
            this.chbWordWrap.Location = new Point(120, 0x10);
            this.chbWordWrap.Name = "chbWordWrap";
            this.chbWordWrap.Size = new Size(0xc0, 0x19);
            this.chbWordWrap.TabIndex = 4;
            this.chbWordWrap.Text = "Word Wrap";
            this.chbPageNumbers.FlatStyle = FlatStyle.System;
            this.chbPageNumbers.Location = new Point(8, 0x5b);
            this.chbPageNumbers.Name = "chbPageNumbers";
            this.chbPageNumbers.Size = new Size(0x68, 0x19);
            this.chbPageNumbers.TabIndex = 3;
            this.chbPageNumbers.Text = "Page Numbers";
            this.chbLineNumbers.FlatStyle = FlatStyle.System;
            this.chbLineNumbers.Location = new Point(8, 0x42);
            this.chbLineNumbers.Name = "chbLineNumbers";
            this.chbLineNumbers.Size = new Size(0x68, 0x19);
            this.chbLineNumbers.TabIndex = 2;
            this.chbLineNumbers.Text = "Line Numbers";
            this.chbUseFooter.FlatStyle = FlatStyle.System;
            this.chbUseFooter.Location = new Point(8, 0x29);
            this.chbUseFooter.Name = "chbUseFooter";
            this.chbUseFooter.Size = new Size(0x68, 0x19);
            this.chbUseFooter.TabIndex = 1;
            this.chbUseFooter.Text = "Footer";
            this.chbUseHeader.FlatStyle = FlatStyle.System;
            this.chbUseHeader.Location = new Point(8, 0x10);
            this.chbUseHeader.Name = "chbUseHeader";
            this.chbUseHeader.Size = new Size(0x68, 0x19);
            this.chbUseHeader.TabIndex = 0;
            this.chbUseHeader.Text = "Header";
            this.gbFileToPrint.Controls.Add(this.chbPrintSelection);
            this.gbFileToPrint.Controls.Add(this.laFileName);
            this.gbFileToPrint.FlatStyle = FlatStyle.System;
            this.gbFileToPrint.Location = new Point(8, 8);
            this.gbFileToPrint.Name = "gbFileToPrint";
            this.gbFileToPrint.Size = new Size(0x148, 0x48);
            this.gbFileToPrint.TabIndex = 5;
            this.gbFileToPrint.TabStop = false;
            this.gbFileToPrint.Text = "File to Print:";
            this.chbPrintSelection.FlatStyle = FlatStyle.System;
            this.chbPrintSelection.Location = new Point(0x10, 0x2a);
            this.chbPrintSelection.Name = "chbPrintSelection";
            this.chbPrintSelection.Size = new Size(0x68, 0x19);
            this.chbPrintSelection.TabIndex = 1;
            this.chbPrintSelection.Text = "Print selection";
            this.laFileName.AutoSize = true;
            this.laFileName.FlatStyle = FlatStyle.System;
            this.laFileName.Location = new Point(0x10, 0x18);
            this.laFileName.Name = "laFileName";
            this.laFileName.Size = new Size(0x3b, 0x10);
            this.laFileName.TabIndex = 0;
            this.laFileName.Text = "File Name:";
            base.AcceptButton = this.btOk;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btCancel;
            base.ClientSize = new Size(0x158, 0x100);
            base.Controls.Add(this.btCancel);
            base.Controls.Add(this.btOk);
            base.Controls.Add(this.gbOptions);
            base.Controls.Add(this.gbFileToPrint);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "DlgPrintOptions";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Print Settings";
            base.Load += new EventHandler(this.DlgPrintOptions_Load);
            this.gbOptions.ResumeLayout(false);
            this.gbFileToPrint.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadFromResource()
        {
            this.Text = StringConsts.DlgPrintOptionsCaption;
            this.gbFileToPrint.Text = StringConsts.FileToPrintCaption;
            this.gbOptions.Text = StringConsts.OptionsCaption;
            this.laFileName.Text = StringConsts.FileNameCaption;
            this.chbPrintSelection.Text = StringConsts.PrintSelectionCaption;
            this.chbUseHeader.Text = StringConsts.UseHeaderCaption;
            this.chbUseFooter.Text = StringConsts.UseFooterCaption;
            this.chbLineNumbers.Text = StringConsts.LineNumbersCaption_PrintOptionsDlg;
            this.chbPageNumbers.Text = StringConsts.PageNumbersCaption;
            this.chbWordWrap.Text = StringConsts.WordWrapCaption_PrintOptionsDlg;
            this.chbUseSyntax.Text = StringConsts.UseSyntaxCaption;
            this.chbUseColors.Text = StringConsts.UseColorsCaption;
            this.chbDisplayProgress.Text = StringConsts.DisplayProgressCaption;
            this.btOk.Text = StringConsts.OkCaption_PrintOptionsDlg;
            this.btCancel.Text = StringConsts.CancelCaption_PrintOptionsDlg;
        }

        protected virtual void OnAllowedOptionsChanged()
        {
        }

        protected virtual void OnFileNameChanged()
        {
        }

        protected virtual void OnOptionsChanged()
        {
        }

        private void OptionsFromControls()
        {
            this.Options = this.chbPrintSelection.Checked ? (this.Options | PrintOptions.PrintSelection) : (this.Options & ~PrintOptions.PrintSelection);
            this.Options = this.chbUseHeader.Checked ? (this.Options | PrintOptions.UseHeader) : (this.Options & ~PrintOptions.UseHeader);
            this.Options = this.chbUseFooter.Checked ? (this.Options | PrintOptions.UseFooter) : (this.Options & ~PrintOptions.UseFooter);
            this.Options = this.chbLineNumbers.Checked ? (this.Options | PrintOptions.LineNumbers) : (this.Options & ~PrintOptions.LineNumbers);
            this.Options = this.chbPageNumbers.Checked ? (this.Options | PrintOptions.PageNumbers) : (this.Options & ~PrintOptions.PageNumbers);
            this.Options = this.chbWordWrap.Checked ? (this.Options | PrintOptions.WordWrap) : (this.Options & ~PrintOptions.WordWrap);
            this.Options = this.chbUseSyntax.Checked ? (this.Options | PrintOptions.UseSyntax) : (this.Options & ~PrintOptions.UseSyntax);
            this.Options = this.chbUseColors.Checked ? (this.Options | PrintOptions.UseColors) : (this.Options & ~PrintOptions.UseColors);
            this.Options = this.chbDisplayProgress.Checked ? (this.Options | PrintOptions.DisplayProgress) : (this.Options & ~PrintOptions.DisplayProgress);
        }

        void IPrintOptionsDialog.add_HelpRequested(HelpEventHandler handler1)
        {
            base.HelpRequested += handler1;
        }

        void IPrintOptionsDialog.remove_HelpRequested(HelpEventHandler handler1)
        {
            base.HelpRequested -= handler1;
        }

        DialogResult IPrintOptionsDialog.ShowDialog()
        {
            return base.ShowDialog();
        }

        DialogResult IPrintOptionsDialog.ShowDialog(IWin32Window window1)
        {
            return base.ShowDialog(window1);
        }

        public virtual void ResetAllowedOptions()
        {
            this.AllowedOptions = EditConsts.DefaultPrintOptions;
        }

        public virtual void ResetOptions()
        {
            this.Options = EditConsts.DefaultPrintOptions;
        }

        public PrintOptions AllowedOptions
        {
            get
            {
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

        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                if (this.fileName != value)
                {
                    this.fileName = value;
                    this.OnFileNameChanged();
                }
            }
        }

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
    }
}

