using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.International;

internal class WizardPageActivateTrial : WizardPage, Interface0
{
    private IContainer icontainer_1;
    private Label lblPassword;
    private Label lblSelectActivation;
    private Label lblUserName;
    private TextBox txtPassword;
    private TextBox txtUserName;

    public WizardPageActivateTrial()
    {
        this.InitializeComponent_1();
        if (Form0.smethod_0().method_8())
        {
            this.txtUserName.Text = Form0.smethod_0().method_12();
            this.txtPassword.Text = Form0.smethod_0().method_14();
        }
        base.method_3(true);
    }

    private void InitializeComponent_1()
    {
        this.lblPassword = new Label();
        this.lblUserName = new Label();
        this.lblSelectActivation = new Label();
        this.txtPassword = new TextBox();
        this.txtUserName = new TextBox();
        base.SuspendLayout();
        this.lblPassword.AutoSize = true;
        this.lblPassword.Location = new Point(9, 0x3b);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new Size(0x38, 13);
        this.lblPassword.TabIndex = 3;
        this.lblPassword.Text = "Password:";
        this.lblUserName.AutoSize = true;
        this.lblUserName.Location = new Point(9, 0x21);
        this.lblUserName.Name = "lblUserName";
        this.lblUserName.Size = new Size(0x3a, 13);
        this.lblUserName.TabIndex = 1;
        this.lblUserName.Text = "Username:";
        this.lblSelectActivation.AutoSize = true;
        this.lblSelectActivation.Location = new Point(9, 3);
        this.lblSelectActivation.Name = "lblSelectActivation";
        this.lblSelectActivation.Size = new Size(0xf7, 13);
        this.lblSelectActivation.TabIndex = 0;
        this.lblSelectActivation.Text = "Sign in using your Wealth-Lab.com site credentials:";
        this.txtPassword.Location = new Point(0x57, 0x38);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new Size(0xb1, 20);
        this.txtPassword.TabIndex = 4;
        this.txtPassword.UseSystemPasswordChar = true;
        this.txtUserName.Location = new Point(0x57, 30);
        this.txtUserName.Name = "txtUserName";
        this.txtUserName.Size = new Size(0xb1, 20);
        this.txtUserName.TabIndex = 2;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.Controls.Add(this.lblPassword);
        base.Controls.Add(this.lblUserName);
        base.Controls.Add(this.lblSelectActivation);
        base.Controls.Add(this.txtPassword);
        base.Controls.Add(this.txtUserName);
        base.Name = "WizardPageActivateTrial";
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    string Interface0.imethod_0()
    {
        return Form0.smethod_2();
    }

    ButtonBehaviour Interface0.imethod_1()
    {
        return (ButtonBehaviour.Cancel | ButtonBehaviour.Previous | ButtonBehaviour.Next);
    }

    public string method_6()
    {
        return this.txtUserName.Text.Trim();
    }

    public string method_7()
    {
        return this.txtPassword.Text.Trim();
    }

    public override bool vmethod_0()
    {
        base.method_2();
        return (base.method_4(this.txtUserName) & base.method_4(this.txtPassword));
    }

    public override void vmethod_1()
    {
        base.method_2();
    }

    void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_1 != null))
        {
            this.icontainer_1.Dispose();
        }
        base.Dispose(disposing);
    }
}

