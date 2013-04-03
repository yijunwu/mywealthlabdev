using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using WealthLab.DataProviders.AsciiFilesStatic;
using WealthLab.DataProviders.AsciiFilesStatic.Properties;

internal class AsciiFilesWizardFolderPage : UserControl
{
    private Button btnRefresh;
    private Button btnSelectFolder;
    private ComboBox cmbFilesExtension;
    private FolderBrowserDialog folderBrowserDialog_0;
    private GroupBox grpFolder;
    private GroupBox grpSymbols;
    private IContainer icontainer_0;
    private Label label1;
    private Label lblFilesExtension;
    private Label lblSelectFolder;
    private Label lblVersion;
    private ListBox lstFiles;
    private string string_0 = "";
    private ToolTip toolTip_0;
    private TextBox txtFolder;

    public AsciiFilesWizardFolderPage()
    {
        this.InitializeComponent();
        this.lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        this.method_4();
    }

    private void btnSelectFolder_Click(object sender, EventArgs e)
    {
        if (this.folderBrowserDialog_0.ShowDialog(this) == DialogResult.OK)
        {
            this.string_0 = this.txtFolder.Text = this.folderBrowserDialog_0.SelectedPath;
            this.method_4();
        }
    }

    private void cmbFilesExtension_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            this.method_4();
        }
    }

    private void cmbFilesExtension_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.method_4();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.icontainer_0 = new Container();
        this.grpFolder = new GroupBox();
        this.lblSelectFolder = new Label();
        this.btnSelectFolder = new Button();
        this.txtFolder = new TextBox();
        this.folderBrowserDialog_0 = new FolderBrowserDialog();
        this.grpSymbols = new GroupBox();
        this.btnRefresh = new Button();
        this.lstFiles = new ListBox();
        this.cmbFilesExtension = new ComboBox();
        this.lblFilesExtension = new Label();
        this.toolTip_0 = new ToolTip(this.icontainer_0);
        this.label1 = new Label();
        this.lblVersion = new Label();
        this.grpFolder.SuspendLayout();
        this.grpSymbols.SuspendLayout();
        base.SuspendLayout();
        this.grpFolder.Controls.Add(this.lblSelectFolder);
        this.grpFolder.Controls.Add(this.btnSelectFolder);
        this.grpFolder.Controls.Add(this.txtFolder);
        this.grpFolder.Location = new Point(7, 7);
        this.grpFolder.Name = "grpFolder";
        this.grpFolder.Size = new Size(0x225, 0x4d);
        this.grpFolder.TabIndex = 3;
        this.grpFolder.TabStop = false;
        this.grpFolder.Tag = "";
        this.grpFolder.Text = "Select Folder that contains ASCII Files";
        this.lblSelectFolder.AutoSize = true;
        this.lblSelectFolder.Location = new Point(3, 0x10);
        this.lblSelectFolder.MaximumSize = new Size(540, 30);
        this.lblSelectFolder.Name = "lblSelectFolder";
        this.lblSelectFolder.Size = new Size(0x21a, 0x1a);
        this.lblSelectFolder.TabIndex = 1;
        this.lblSelectFolder.Text = "You can create Wealth-Lab DataSet for files that reside in a single folder in your system. Each file should contain data for an individual security. \r\n";
        this.btnSelectFolder.FlatStyle = FlatStyle.System;
        this.btnSelectFolder.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.btnSelectFolder.Location = new Point(0x207, 0x31);
        this.btnSelectFolder.Name = "btnSelectFolder";
        this.btnSelectFolder.Size = new Size(0x19, 0x16);
        this.btnSelectFolder.TabIndex = 1;
        this.btnSelectFolder.Text = "...";
        this.btnSelectFolder.UseVisualStyleBackColor = true;
        this.btnSelectFolder.Click += new EventHandler(this.btnSelectFolder_Click);
        this.txtFolder.BackColor = SystemColors.Window;
        this.txtFolder.Location = new Point(6, 50);
        this.txtFolder.Name = "txtFolder";
        this.txtFolder.ReadOnly = true;
        this.txtFolder.Size = new Size(0x200, 20);
        this.txtFolder.TabIndex = 0;
        this.folderBrowserDialog_0.Description = "Choose Directory";
        this.folderBrowserDialog_0.RootFolder = Environment.SpecialFolder.MyComputer;
        this.folderBrowserDialog_0.ShowNewFolderButton = false;
        this.grpSymbols.Controls.Add(this.btnRefresh);
        this.grpSymbols.Controls.Add(this.lstFiles);
        this.grpSymbols.Controls.Add(this.cmbFilesExtension);
        this.grpSymbols.Controls.Add(this.lblFilesExtension);
        this.grpSymbols.Location = new Point(7, 90);
        this.grpSymbols.Name = "grpSymbols";
        this.grpSymbols.Size = new Size(0x225, 0xf7);
        this.grpSymbols.TabIndex = 5;
        this.grpSymbols.TabStop = false;
        this.grpSymbols.Text = "The selected Folder contains the following Files";
        this.btnRefresh.Image = Resources.Refresh;
        this.btnRefresh.ImageAlign = ContentAlignment.MiddleLeft;
        this.btnRefresh.Location = new Point(0x1b1, 0xd9);
        this.btnRefresh.Name = "btnRefresh";
        this.btnRefresh.Size = new Size(0x6f, 0x18);
        this.btnRefresh.TabIndex = 3;
        this.btnRefresh.Text = "Refresh";
        this.btnRefresh.UseVisualStyleBackColor = true;
        this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
        this.lstFiles.FormattingEnabled = true;
        this.lstFiles.IntegralHeight = false;
        this.lstFiles.Location = new Point(5, 50);
        this.lstFiles.Name = "lstFiles";
        this.lstFiles.Size = new Size(0x21a, 0xa1);
        this.lstFiles.TabIndex = 2;
        this.cmbFilesExtension.FormattingEnabled = true;
        this.cmbFilesExtension.Items.AddRange(new object[] { "txt", "csv", "asc", "dat" });
        this.cmbFilesExtension.Location = new Point(0x5c, 0x13);
        this.cmbFilesExtension.Name = "cmbFilesExtension";
        this.cmbFilesExtension.Size = new Size(0x65, 0x15);
        this.cmbFilesExtension.TabIndex = 2;
        this.cmbFilesExtension.SelectedIndexChanged += new EventHandler(this.cmbFilesExtension_SelectedIndexChanged);
        this.cmbFilesExtension.KeyDown += new KeyEventHandler(this.cmbFilesExtension_KeyDown);
        this.lblFilesExtension.AutoSize = true;
        this.lblFilesExtension.Location = new Point(6, 0x16);
        this.lblFilesExtension.Name = "lblFilesExtension";
        this.lblFilesExtension.Size = new Size(80, 13);
        this.lblFilesExtension.TabIndex = 0;
        this.lblFilesExtension.Text = "Files Extension:";
        this.label1.AutoSize = true;
        this.label1.Font = new Font("Tahoma", 8f, FontStyle.Italic, GraphicsUnit.Pixel, 0xcc);
        this.label1.Location = new Point(5, 340);
        this.label1.Name = "label1";
        this.label1.Size = new Size(0xff, 10);
        this.label1.TabIndex = 6;
        this.label1.Text = "The Wizards's GUI is decorated with \"Silk icon set 1.3\" \x00a9 Mark James";
        this.lblVersion.AutoSize = true;
        this.lblVersion.Font = new Font("Microsoft Sans Serif", 6.5f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.lblVersion.Location = new Point(0x20b, 0x152);
        this.lblVersion.Name = "lblVersion";
        this.lblVersion.RightToLeft = RightToLeft.Yes;
        this.lblVersion.Size = new Size(0x22, 12);
        this.lblVersion.TabIndex = 7;
        this.lblVersion.Text = "1.0.2.3";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.lblVersion);
        base.Controls.Add(this.label1);
        base.Controls.Add(this.grpSymbols);
        base.Controls.Add(this.grpFolder);
        base.Name = "AsciiFilesWizardFolderPage";
        base.Size = new Size(560, 0x161);
        this.grpFolder.ResumeLayout(false);
        this.grpFolder.PerformLayout();
        this.grpSymbols.ResumeLayout(false);
        this.grpSymbols.PerformLayout();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    public string method_0()
    {
        return this.string_0;
    }

    public string method_1()
    {
        this.cmbFilesExtension.Text = this.cmbFilesExtension.Text.Trim();
        if (this.cmbFilesExtension.Text == "")
        {
            return "*.*";
        }
        return ("*." + this.cmbFilesExtension.Text);
    }

    public int method_2()
    {
        return this.lstFiles.Items.Count;
    }

    public void method_3()
    {
        this.lstFiles.DataSource = null;
        this.cmbFilesExtension.Text = "csv";
        if (this.string_0 != "")
        {
            this.method_4();
        }
    }

    private void method_4()
    {
        this.lstFiles.DataSource = AsciiFilesDataSet.GetFilesFromDir(this.string_0, this.method_1(), false, true);
    }
}

