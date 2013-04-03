using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.International;

internal class WizardPageActivateKey : WizardPage, Interface0
{
    private IContainer icontainer_1;
    private Label label1;
    private Label label2;
    private Label lblFirstName;
    private Label lblKey;
    private TextBox txtFirstName;
    private TextBox txtKey;
    private TextBox txtLastName;

    public WizardPageActivateKey()
    {
        this.InitializeComponent_1();
        if (Form0.smethod_0().method_8())
        {
            this.txtFirstName.Text = Form0.smethod_0().method_0();
            this.txtLastName.Text = Form0.smethod_0().method_2();
            this.txtKey.Text = Form0.smethod_0().method_4();
        }
        base.method_3(true);
    }

    private void InitializeComponent_1()
    {
        this.txtFirstName = new TextBox();
        this.lblFirstName = new Label();
        this.label1 = new Label();
        this.txtLastName = new TextBox();
        this.label2 = new Label();
        this.txtKey = new TextBox();
        this.lblKey = new Label();
        base.SuspendLayout();
        this.txtFirstName.Location = new Point(100, 0x19);
        this.txtFirstName.Name = "txtFirstName";
        this.txtFirstName.Size = new Size(0xb6, 20);
        this.txtFirstName.TabIndex = 2;
        this.lblFirstName.AutoSize = true;
        this.lblFirstName.Location = new Point(9, 0x1c);
        this.lblFirstName.Name = "lblFirstName";
        this.lblFirstName.Size = new Size(60, 13);
        this.lblFirstName.TabIndex = 1;
        this.lblFirstName.Text = "First Name:";
        this.label1.AutoSize = true;
        this.label1.Location = new Point(9, 0x36);
        this.label1.Name = "label1";
        this.label1.Size = new Size(0x3d, 13);
        this.label1.TabIndex = 3;
        this.label1.Text = "Last Name:";
        this.txtLastName.Location = new Point(100, 0x33);
        this.txtLastName.Name = "txtLastName";
        this.txtLastName.Size = new Size(0xb6, 20);
        this.txtLastName.TabIndex = 4;
        this.label2.AutoSize = true;
        this.label2.Location = new Point(9, 3);
        this.label2.Name = "label2";
        this.label2.Size = new Size(0x1c9, 13);
        this.label2.TabIndex = 0;
        this.label2.Text = "Fill in your credentials and the Activation Key received by e-mail when you ordered the software.";
        this.txtKey.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.txtKey.Location = new Point(100, 0x4d);
        this.txtKey.Name = "txtKey";
        this.txtKey.Size = new Size(0xb6, 20);
        this.txtKey.TabIndex = 6;
        this.lblKey.AutoSize = true;
        this.lblKey.Location = new Point(9, 80);
        this.lblKey.Name = "lblKey";
        this.lblKey.Size = new Size(0x4e, 13);
        this.lblKey.TabIndex = 5;
        this.lblKey.Text = "Activation Key:";
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.Controls.Add(this.lblKey);
        base.Controls.Add(this.txtKey);
        base.Controls.Add(this.label2);
        base.Controls.Add(this.label1);
        base.Controls.Add(this.txtLastName);
        base.Controls.Add(this.lblFirstName);
        base.Controls.Add(this.txtFirstName);
        base.Name = "WizardPageActivateKey";
        base.Size = new Size(500, 150);
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

    public string method_10()
    {
        return this.txtKey.Text.Trim();
    }

    public void method_11(string string_1)
    {
        this.txtKey.Text = string_1;
    }

    public string method_6()
    {
        return this.txtFirstName.Text.Trim();
    }

    public void method_7(string string_1)
    {
        this.txtFirstName.Text = string_1;
    }

    public string method_8()
    {
        return this.txtLastName.Text.Trim();
    }

    public void method_9(string string_1)
    {
        this.txtLastName.Text = string_1;
    }

    public override bool vmethod_0()
    {
        base.method_2();
        return ((base.method_4(this.txtFirstName) & base.method_4(this.txtLastName)) & base.method_4(this.txtKey));
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

