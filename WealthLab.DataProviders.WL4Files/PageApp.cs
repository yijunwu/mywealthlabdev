using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.DataProviders.WL4Files;

internal class PageApp : UserControl
{
    private GroupBox grpSelectApp;
    private IContainer icontainer_0;
    private Label lblSelectApp;
    private RadioButton rbWLD;
    private RadioButton rbWLP;

    public PageApp()
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
        this.grpSelectApp = new GroupBox();
        this.rbWLP = new RadioButton();
        this.rbWLD = new RadioButton();
        this.lblSelectApp = new Label();
        this.grpSelectApp.SuspendLayout();
        base.SuspendLayout();
        this.grpSelectApp.Controls.Add(this.rbWLP);
        this.grpSelectApp.Controls.Add(this.rbWLD);
        this.grpSelectApp.Controls.Add(this.lblSelectApp);
        this.grpSelectApp.Location = new Point(7, 7);
        this.grpSelectApp.Name = "grpSelectApp";
        this.grpSelectApp.Size = new Size(550, 0x157);
        this.grpSelectApp.TabIndex = 0;
        this.grpSelectApp.TabStop = false;
        this.grpSelectApp.Text = "Select Application";
        this.rbWLP.AutoSize = true;
        this.rbWLP.Location = new Point(9, 40);
        this.rbWLP.Name = "rbWLP";
        this.rbWLP.Size = new Size(0x6c, 0x11);
        this.rbWLP.TabIndex = 2;
        this.rbWLP.TabStop = true;
        this.rbWLP.Text = "Wealth-Lab Pro 4";
        this.rbWLP.UseVisualStyleBackColor = true;
        this.rbWLD.AutoSize = true;
        this.rbWLD.Location = new Point(9, 0x3f);
        this.rbWLD.Name = "rbWLD";
        this.rbWLD.Size = new Size(0x8d, 0x11);
        this.rbWLD.TabIndex = 1;
        this.rbWLD.TabStop = true;
        this.rbWLD.Text = "Wealth-Lab Developer 4";
        this.rbWLD.UseVisualStyleBackColor = true;
        this.lblSelectApp.AutoSize = true;
        this.lblSelectApp.Location = new Point(6, 0x13);
        this.lblSelectApp.Name = "lblSelectApp";
        this.lblSelectApp.Size = new Size(0x13c, 13);
        this.lblSelectApp.TabIndex = 0;
        this.lblSelectApp.Text = "A DataSet of which source application's data you want to create?";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.grpSelectApp);
        base.Name = "PageApp";
        base.Size = new Size(560, 0x161);
        this.grpSelectApp.ResumeLayout(false);
        this.grpSelectApp.PerformLayout();
        base.ResumeLayout(false);
    }

    public App method_0()
    {
        if (this.rbWLP.Checked)
        {
            return App.Pro;
        }
        return App.Dev;
    }

    public void method_1()
    {
        this.rbWLP.Checked = true;
    }
}

