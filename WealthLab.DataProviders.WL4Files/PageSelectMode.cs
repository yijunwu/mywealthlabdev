using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.DataProviders.WL4Files;

internal class PageSelectMode : UserControl
{
    private App app_0;
    private GroupBox grpSelectMode;
    private IContainer icontainer_0;
    private Label lblSelectMode;
    private RadioButton rbDataSource;
    private RadioButton rbFolder;

    public PageSelectMode()
    {
        this.InitializeComponent();
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
        this.grpSelectMode = new GroupBox();
        this.rbFolder = new RadioButton();
        this.rbDataSource = new RadioButton();
        this.lblSelectMode = new Label();
        this.grpSelectMode.SuspendLayout();
        base.SuspendLayout();
        this.grpSelectMode.Controls.Add(this.rbFolder);
        this.grpSelectMode.Controls.Add(this.rbDataSource);
        this.grpSelectMode.Controls.Add(this.lblSelectMode);
        this.grpSelectMode.Location = new Point(7, 7);
        this.grpSelectMode.Name = "grpSelectMode";
        this.grpSelectMode.RightToLeft = RightToLeft.No;
        this.grpSelectMode.Size = new Size(550, 0x157);
        this.grpSelectMode.TabIndex = 0;
        this.grpSelectMode.TabStop = false;
        this.grpSelectMode.Text = "Symbol selection mode";
        this.rbFolder.AutoSize = true;
        this.rbFolder.Location = new Point(9, 0x3f);
        this.rbFolder.Name = "rbFolder";
        this.rbFolder.Size = new Size(0xba, 0x11);
        this.rbFolder.TabIndex = 2;
        this.rbFolder.TabStop = true;
        this.rbFolder.Text = "Select a folder containing *.wl files";
        this.rbFolder.UseVisualStyleBackColor = true;
        this.rbDataSource.AutoSize = true;
        this.rbDataSource.Location = new Point(9, 40);
        this.rbDataSource.Name = "rbDataSource";
        this.rbDataSource.Size = new Size(0x42, 0x11);
        this.rbDataSource.TabIndex = 1;
        this.rbDataSource.TabStop = true;
        this.rbDataSource.Text = "[in code]";
        this.rbDataSource.UseVisualStyleBackColor = true;
        this.lblSelectMode.AutoSize = true;
        this.lblSelectMode.Location = new Point(6, 0x13);
        this.lblSelectMode.Name = "lblSelectMode";
        this.lblSelectMode.Size = new Size(0xd8, 13);
        this.lblSelectMode.TabIndex = 0;
        this.lblSelectMode.Text = "How do you like to add symbols to DataSet?";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.grpSelectMode);
        base.Name = "PageSelectMode";
        this.RightToLeft = RightToLeft.Yes;
        base.Size = new Size(560, 0x161);
        this.grpSelectMode.ResumeLayout(false);
        this.grpSelectMode.PerformLayout();
        base.ResumeLayout(false);
    }

    public App method_0()
    {
        return this.app_0;
    }

    public void method_1(App app_1)
    {
        string str = "Select an existing DataSource from ";
        switch (app_1)
        {
            case App.Dev:
                str = str + "Wealth-Lab Developer 4";
                break;

            case App.Pro:
                str = str + "Wealth-Lab Pro 4";
                break;
        }
        this.rbDataSource.Text = str;
    }

    public SelectMode method_2()
    {
        if (this.rbDataSource.Checked)
        {
            return SelectMode.DataSource;
        }
        return SelectMode.Folder;
    }

    private bool method_3()
    {
        return (IntPtr.Size == 8);
    }

    public void method_4()
    {
        this.rbDataSource.Checked = true;
        if (this.method_3())
        {
            this.rbDataSource.Enabled = false;
            this.rbFolder.Checked = true;
        }
    }
}

