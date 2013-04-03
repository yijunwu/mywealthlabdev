using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.DataProviders.WL4Files;

internal class PageDataFolder : UserControl
{
    private App app_0;
    private Button btnSelectFolder;
    private CheckedListBox clstSymbols;
    private ContextMenuStrip cmnuSymbols;
    private FolderBrowserDialog folderBrowserDialog_0;
    private GroupBox grpFolder;
    private GroupBox grpSymbols;
    private IContainer icontainer_0;
    private Label lblFolder;
    private Label lblSelect;
    private Label lblSymbols;
    private ToolStripMenuItem mniCheckAll;
    private ToolStripMenuItem mniUncheckAll;
    private TextBox txtFolder;

    public PageDataFolder()
    {
        this.InitializeComponent();
    }

    private void btnSelectFolder_Click(object sender, EventArgs e)
    {
        if (this.folderBrowserDialog_0.ShowDialog() == DialogResult.OK)
        {
            this.txtFolder.Text = this.folderBrowserDialog_0.SelectedPath;
            this.method_6();
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
        this.icontainer_0 = new Container();
        this.grpFolder = new GroupBox();
        this.lblFolder = new Label();
        this.btnSelectFolder = new Button();
        this.txtFolder = new TextBox();
        this.grpSymbols = new GroupBox();
        this.clstSymbols = new CheckedListBox();
        this.cmnuSymbols = new ContextMenuStrip(this.icontainer_0);
        this.mniCheckAll = new ToolStripMenuItem();
        this.mniUncheckAll = new ToolStripMenuItem();
        this.lblSelect = new Label();
        this.lblSymbols = new Label();
        this.folderBrowserDialog_0 = new FolderBrowserDialog();
        this.grpFolder.SuspendLayout();
        this.grpSymbols.SuspendLayout();
        this.cmnuSymbols.SuspendLayout();
        base.SuspendLayout();
        this.grpFolder.Controls.Add(this.lblFolder);
        this.grpFolder.Controls.Add(this.btnSelectFolder);
        this.grpFolder.Controls.Add(this.txtFolder);
        this.grpFolder.Location = new Point(7, 7);
        this.grpFolder.Name = "grpFolder";
        this.grpFolder.Size = new Size(550, 0x45);
        this.grpFolder.TabIndex = 1;
        this.grpFolder.TabStop = false;
        this.grpFolder.Text = "Folder path";
        this.lblFolder.AutoSize = true;
        this.lblFolder.Location = new Point(6, 0x13);
        this.lblFolder.Name = "lblFolder";
        this.lblFolder.Size = new Size(0xcb, 13);
        this.lblFolder.TabIndex = 4;
        this.lblFolder.Text = "Specify path to the folder which contains *.wl files";
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
        this.grpSymbols.Controls.Add(this.clstSymbols);
        this.grpSymbols.Controls.Add(this.lblSelect);
        this.grpSymbols.Controls.Add(this.lblSymbols);
        this.grpSymbols.Location = new Point(7, 0x52);
        this.grpSymbols.Name = "grpSymbols";
        this.grpSymbols.Size = new Size(550, 0x10c);
        this.grpSymbols.TabIndex = 2;
        this.grpSymbols.TabStop = false;
        this.grpSymbols.Text = "Symbols in the folder";
        this.clstSymbols.CheckOnClick = true;
        this.clstSymbols.ContextMenuStrip = this.cmnuSymbols;
        this.clstSymbols.FormattingEnabled = true;
        this.clstSymbols.IntegralHeight = false;
        this.clstSymbols.Location = new Point(9, 40);
        this.clstSymbols.MultiColumn = true;
        this.clstSymbols.Name = "clstSymbols";
        this.clstSymbols.Size = new Size(0x214, 0xdb);
        this.clstSymbols.TabIndex = 6;
        this.cmnuSymbols.Items.AddRange(new ToolStripItem[] { this.mniCheckAll, this.mniUncheckAll });
        this.cmnuSymbols.Name = "cmnuSymbols";
        this.cmnuSymbols.Size = new Size(140, 0x30);
        this.mniCheckAll.Name = "mniCheckAll";
        this.mniCheckAll.Size = new Size(0x8b, 0x16);
        this.mniCheckAll.Text = "Check All";
        this.mniCheckAll.Click += new EventHandler(this.mniCheckAll_Click);
        this.mniUncheckAll.Name = "mniUncheckAll";
        this.mniUncheckAll.Size = new Size(0x8b, 0x16);
        this.mniUncheckAll.Text = "Uncheck All";
        this.mniUncheckAll.Click += new EventHandler(this.mniUncheckAll_Click);
        this.lblSelect.AutoSize = true;
        this.lblSelect.Location = new Point(6, 0x13);
        this.lblSelect.Name = "lblSelect";
        this.lblSelect.Size = new Size(0x6a, 13);
        this.lblSelect.TabIndex = 5;
        this.lblSelect.Text = "Select symbols";
        this.lblSymbols.AutoSize = true;
        this.lblSymbols.Location = new Point(6, 0x10);
        this.lblSymbols.Name = "lblSymbols";
        this.lblSymbols.Size = new Size(0, 13);
        this.lblSymbols.TabIndex = 0;
        this.folderBrowserDialog_0.Description = "Select Folder";
        this.folderBrowserDialog_0.RootFolder = Environment.SpecialFolder.MyComputer;
        this.folderBrowserDialog_0.ShowNewFolderButton = false;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.grpSymbols);
        base.Controls.Add(this.grpFolder);
        base.Name = "PageDataFolder";
        base.Size = new Size(560, 0x161);
        this.grpFolder.ResumeLayout(false);
        this.grpFolder.PerformLayout();
        this.grpSymbols.ResumeLayout(false);
        this.grpSymbols.PerformLayout();
        this.cmnuSymbols.ResumeLayout(false);
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
            this.txtFolder.Text = string.Empty;
            this.clstSymbols.Items.Clear();
            string str = Class13.smethod_6(app_1);
            if (str != null)
            {
                this.folderBrowserDialog_0.SelectedPath = str;
            }
        }
        this.app_0 = app_1;
    }

    public string method_2()
    {
        return this.txtFolder.Text.Trim();
    }

    public List<string> method_3()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < this.clstSymbols.Items.Count; i++)
        {
            if (this.clstSymbols.GetItemChecked(i))
            {
                list.Add(this.clstSymbols.Items[i] as string);
            }
        }
        return list;
    }

    public bool method_4()
    {
        for (int i = 0; i < this.clstSymbols.Items.Count; i++)
        {
            if (!this.clstSymbols.GetItemChecked(i))
            {
                return false;
            }
        }
        return true;
    }

    public void method_5()
    {
    }

    private void method_6()
    {
        this.clstSymbols.Items.Clear();
        if (this.txtFolder.Text.Trim() != string.Empty)
        {
            List<string> list = Class13.smethod_10(this.txtFolder.Text);
            this.clstSymbols.BeginUpdate();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    this.clstSymbols.Items.Add(list[i]);
                    this.clstSymbols.SetItemChecked(i, true);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.clstSymbols.EndUpdate();
            }
        }
    }

    private void method_7(bool bool_0)
    {
        for (int i = 0; i < this.clstSymbols.Items.Count; i++)
        {
            this.clstSymbols.SetItemChecked(i, bool_0);
        }
    }

    private void mniCheckAll_Click(object sender, EventArgs e)
    {
        this.method_7(true);
    }

    private void mniUncheckAll_Click(object sender, EventArgs e)
    {
        this.method_7(false);
    }
}

