using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.International;

internal class WizardPageHwidToRegister : WizardPage, Interface0
{
    private IContainer icontainer_1;
    private Label lblEG;
    private Label lblHwidToRegister;
    private Label lblPCFingerprint;
    private Label lblPCName;
    private TextBox txtComputerName;
    private TextBox txtHwid;

    public WizardPageHwidToRegister()
    {
        this.InitializeComponent_1();
        this.txtComputerName.Text = Environment.MachineName;
        if (Form0.smethod_0().method_8())
        {
            this.txtComputerName.Text = Form0.smethod_0().method_6();
        }
    }

    private void InitializeComponent_1()
    {
        this.lblHwidToRegister = new Label();
        this.lblPCFingerprint = new Label();
        this.txtHwid = new TextBox();
        this.txtComputerName = new TextBox();
        this.lblPCName = new Label();
        this.lblEG = new Label();
        base.SuspendLayout();
        this.lblHwidToRegister.AutoSize = true;
        this.lblHwidToRegister.Location = new Point(9, 3);
        this.lblHwidToRegister.Name = "lblHwidToRegister";
        this.lblHwidToRegister.Size = new Size(400, 13);
        this.lblHwidToRegister.TabIndex = 0;
        this.lblHwidToRegister.Text = "Enter this computer's name. It could be any representative title e.g. \"My Notebook\".";
        this.lblPCFingerprint.AutoSize = true;
        this.lblPCFingerprint.Location = new Point(9, 0x1c);
        this.lblPCFingerprint.Name = "lblPCFingerprint";
        this.lblPCFingerprint.Size = new Size(0x4c, 13);
        this.lblPCFingerprint.TabIndex = 1;
        this.lblPCFingerprint.Text = "PC Fingerprint:";
        this.txtHwid.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.txtHwid.Location = new Point(100, 0x19);
        this.txtHwid.Name = "txtHwid";
        this.txtHwid.ReadOnly = true;
        this.txtHwid.Size = new Size(0xb6, 20);
        this.txtHwid.TabIndex = 2;
        this.txtComputerName.Location = new Point(100, 0x33);
        this.txtComputerName.Name = "txtComputerName";
        this.txtComputerName.Size = new Size(0xb6, 20);
        this.txtComputerName.TabIndex = 4;
        this.lblPCName.AutoSize = true;
        this.lblPCName.Location = new Point(9, 0x36);
        this.lblPCName.Name = "lblPCName";
        this.lblPCName.Size = new Size(0x37, 13);
        this.lblPCName.TabIndex = 3;
        this.lblPCName.Text = "PC Name:";
        this.lblEG.AutoSize = true;
        this.lblEG.Font = new Font("Microsoft Sans Serif", 7.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.lblEG.ForeColor = SystemColors.ControlText;
        this.lblEG.Location = new Point(0x61, 0x4a);
        this.lblEG.Name = "lblEG";
        this.lblEG.Size = new Size(0, 13);
        this.lblEG.TabIndex = 5;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.Controls.Add(this.lblEG);
        base.Controls.Add(this.lblPCName);
        base.Controls.Add(this.txtComputerName);
        base.Controls.Add(this.lblPCFingerprint);
        base.Controls.Add(this.txtHwid);
        base.Controls.Add(this.lblHwidToRegister);
        base.Name = "WizardPageHwidToRegister";
        base.Size = new Size(500, 0x92);
        base.Controls.SetChildIndex(this.lblHwidToRegister, 0);
        base.Controls.SetChildIndex(this.txtHwid, 0);
        base.Controls.SetChildIndex(this.lblPCFingerprint, 0);
        base.Controls.SetChildIndex(this.txtComputerName, 0);
        base.Controls.SetChildIndex(this.lblPCName, 0);
        base.Controls.SetChildIndex(this.lblEG, 0);
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    string Interface0.imethod_0()
    {
        return Form0.smethod_1();
    }

    ButtonBehaviour Interface0.imethod_1()
    {
        return (ButtonBehaviour.Cancel | ButtonBehaviour.Previous | ButtonBehaviour.Next);
    }

    public string method_6()
    {
        return this.txtComputerName.Text.Trim();
    }

    public void method_7(string string_1)
    {
        this.txtComputerName.Text = string_1;
    }

    public override bool vmethod_0()
    {
        base.method_2();
        return (base.method_4(this.txtHwid) & base.method_4(this.txtComputerName));
    }

    public override void vmethod_1()
    {
        this.txtHwid.Text = Form0.smethod_4();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_1 != null))
        {
            this.icontainer_1.Dispose();
        }
        base.Dispose(disposing);
    }
}

