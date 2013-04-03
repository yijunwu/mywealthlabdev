using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.WL4Files;

internal class PageDataSources : UserControl
{
    private App app_0;
    private Button btnSelectFolder;
    private FolderBrowserDialog folderBrowserDialog_0;
    private GroupBox grpDataSources;
    private GroupBox grpFolder;
    private IContainer icontainer_0;
    private Label lblDataSources;
    private Label lblFolder;
    private ListBox lstDataSources;
    private TextBox txtFolder;

    public PageDataSources()
    {
        this.InitializeComponent();
    }

    private void btnSelectFolder_Click(object sender, EventArgs e)
    {
        if (this.folderBrowserDialog_0.ShowDialog() == DialogResult.OK)
        {
            this.txtFolder.Text = this.folderBrowserDialog_0.SelectedPath;
            this.txtFolder.ForeColor = SystemColors.WindowText;
            this.method_5();
        }
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
        this.grpFolder = new GroupBox();
        this.btnSelectFolder = new Button();
        this.txtFolder = new TextBox();
        this.grpDataSources = new GroupBox();
        this.lstDataSources = new ListBox();
        this.lblDataSources = new Label();
        this.folderBrowserDialog_0 = new FolderBrowserDialog();
        this.lblFolder = new Label();
        this.grpFolder.SuspendLayout();
        this.grpDataSources.SuspendLayout();
        base.SuspendLayout();
        this.grpFolder.Controls.Add(this.lblFolder);
        this.grpFolder.Controls.Add(this.btnSelectFolder);
        this.grpFolder.Controls.Add(this.txtFolder);
        this.grpFolder.Location = new Point(7, 7);
        this.grpFolder.Name = "grpFolder";
        this.grpFolder.Size = new Size(550, 0x45);
        this.grpFolder.TabIndex = 0;
        this.grpFolder.TabStop = false;
        this.grpFolder.Text = "Path to database";
        this.btnSelectFolder.FlatStyle = FlatStyle.System;
        this.btnSelectFolder.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.btnSelectFolder.Location = new Point(0x204, 0x27);
        this.btnSelectFolder.Name = "btnSelectFolder";
        this.btnSelectFolder.Size = new Size(0x19, 0x16);
        this.btnSelectFolder.TabIndex = 2;
        this.btnSelectFolder.Text = "...";
        this.btnSelectFolder.UseVisualStyleBackColor = true;
        this.btnSelectFolder.Click += new EventHandler(this.btnSelectFolder_Click);
        this.txtFolder.BackColor = SystemColors.Window;
        this.txtFolder.Location = new Point(9, 40);
        this.txtFolder.Name = "txtFolder";
        this.txtFolder.ReadOnly = true;
        this.txtFolder.Size = new Size(0x1f8, 20);
        this.txtFolder.TabIndex = 1;
        this.txtFolder.TextChanged += new EventHandler(this.txtFolder_TextChanged);
        this.grpDataSources.Controls.Add(this.lstDataSources);
        this.grpDataSources.Controls.Add(this.lblDataSources);
        this.grpDataSources.Location = new Point(7, 0x52);
        this.grpDataSources.Name = "grpDataSources";
        this.grpDataSources.Size = new Size(550, 0x10c);
        this.grpDataSources.TabIndex = 1;
        this.grpDataSources.TabStop = false;
        this.grpDataSources.Text = "DataSources";
        this.lstDataSources.FormattingEnabled = true;
        this.lstDataSources.IntegralHeight = false;
        this.lstDataSources.Location = new Point(9, 40);
        this.lstDataSources.Name = "lstDataSources";
        this.lstDataSources.Size = new Size(0x214, 0xdb);
        this.lstDataSources.TabIndex = 2;
        this.lblDataSources.AutoSize = true;
        this.lblDataSources.Location = new Point(6, 0x13);
        this.lblDataSources.Name = "lblDataSources";
        this.lblDataSources.Size = new Size(0x1b3, 13);
        this.lblDataSources.TabIndex = 1;
        this.lblDataSources.Text = "Select DataSource (from the list of MSN, Yahoo!, Fidelity and Intraday Historical Datasources)";
        this.folderBrowserDialog_0.Description = "Select Folder";
        this.folderBrowserDialog_0.RootFolder = Environment.SpecialFolder.MyComputer;
        this.folderBrowserDialog_0.ShowNewFolderButton = false;
        this.lblFolder.AutoSize = true;
        this.lblFolder.Location = new Point(6, 0x13);
        this.lblFolder.Name = "lblFolder";
        this.lblFolder.Size = new Size(0xcf, 13);
        this.lblFolder.TabIndex = 3;
        this.lblFolder.Text = "Specify path to the Database folder";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.grpDataSources);
        base.Controls.Add(this.grpFolder);
        base.Name = "PageDataSources";
        base.Size = new Size(560, 0x161);
        this.grpFolder.ResumeLayout(false);
        this.grpFolder.PerformLayout();
        this.grpDataSources.ResumeLayout(false);
        this.grpDataSources.PerformLayout();
        base.ResumeLayout(false);
    }

    public App method_0()
    {
        return this.app_0;
    }

    public void method_1(App app_1)
    {
        Class14.smethod_8(new object[] { app_1 });
        if ((this.txtFolder.Text == string.Empty) || (this.app_0 != app_1))
        {
            Class14.smethod_8(new object[0]);
            this.txtFolder.ForeColor = SystemColors.WindowText;
            string str = Class13.smethod_9(app_1);
            this.lstDataSources.Items.Clear();
            if (str == null)
            {
                string str2 = "Database not found.";
                this.txtFolder.Text = str2 + " Specify correct path to the Database or return to the previous page.";
                this.txtFolder.ForeColor = Color.Red;
                Class14.smethod_4(TraceEventType.Warning, app_1 + " " + str2);
            }
            else
            {
                this.txtFolder.Text = str;
                this.method_5();
            }
        }
        this.app_0 = app_1;
    }

    public WL4DataSource method_2()
    {
        return (this.lstDataSources.SelectedItem as WL4DataSource);
    }

    public string method_3()
    {
        return this.txtFolder.Text.Trim();
    }

    public void method_4()
    {
    }

    public void method_5()
    {
        this.lstDataSources.Items.Clear();
        List<WL4DataSource> list = null;
        try
        {
            list = Class13.smethod_5(this.txtFolder.Text);
        }
        catch (Exception exception)
        {
            Class14.smethod_4(TraceEventType.Error, exception.Message);
            MessageBox.Show(this, exception.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
        }
        if (list != null)
        {
            foreach (WL4DataSource source in list)
            {
                this.lstDataSources.Items.Add(source);
            }
        }
    }

    private void txtFolder_TextChanged(object sender, EventArgs e)
    {
        this.folderBrowserDialog_0.SelectedPath = this.txtFolder.Text;
    }
}

