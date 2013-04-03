using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.International;

internal class WizardPageSelectActivationMethod : WizardPage, Interface0
{
    private IContainer icontainer_1;
    private Label lblSelectActivation;
    private RadioButton rbEnterActivationKey;
    private RadioButton rbTrialVersion;

    public WizardPageSelectActivationMethod()
    {
        this.InitializeComponent_1();
        base.method_3(true);
    }

    private void InitializeComponent_1()
    {
        this.lblSelectActivation = new Label();
        this.rbTrialVersion = new RadioButton();
        this.rbEnterActivationKey = new RadioButton();
        base.SuspendLayout();
        this.lblSelectActivation.AutoSize = true;
        this.lblSelectActivation.Location = new Point(9, 3);
        this.lblSelectActivation.Name = "lblSelectActivation";
        this.lblSelectActivation.Size = new Size(0x1ba, 13);
        this.lblSelectActivation.TabIndex = 0;
        this.lblSelectActivation.Text = "Thank you for using Wealth-Lab Developer. Which way would you like to unlock your copy?";
        this.rbTrialVersion.AutoSize = true;
        this.rbTrialVersion.Location = new Point(12, 0x36);
        this.rbTrialVersion.Name = "rbTrialVersion";
        this.rbTrialVersion.Size = new Size(0xcd, 0x11);
        this.rbTrialVersion.TabIndex = 2;
        this.rbTrialVersion.Text = "I would like to register for a 30 day trial";
        this.rbTrialVersion.UseVisualStyleBackColor = true;
        this.rbEnterActivationKey.AutoSize = true;
        this.rbEnterActivationKey.Checked = true;
        this.rbEnterActivationKey.Location = new Point(12, 0x1f);
        this.rbEnterActivationKey.Name = "rbEnterActivationKey";
        this.rbEnterActivationKey.Size = new Size(0x160, 0x11);
        this.rbEnterActivationKey.TabIndex = 1;
        this.rbEnterActivationKey.TabStop = true;
        this.rbEnterActivationKey.Text = "I have purchased Wealth-Lab Developer and have an Activation Key";
        this.rbEnterActivationKey.UseVisualStyleBackColor = true;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.Controls.Add(this.rbTrialVersion);
        base.Controls.Add(this.rbEnterActivationKey);
        base.Controls.Add(this.lblSelectActivation);
        base.Name = "WizardPageSelectActivationMethod";
        base.Size = new Size(500, 0x91);
        base.Controls.SetChildIndex(this.lblSelectActivation, 0);
        base.Controls.SetChildIndex(this.rbEnterActivationKey, 0);
        base.Controls.SetChildIndex(this.rbTrialVersion, 0);
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    string Interface0.imethod_0()
    {
        if (Form0.smethod_6() == WizardFormMode.FromApplication)
        {
            this.lblSelectActivation.Text = "Which way would you like to authenticate?";
            this.rbTrialVersion.Text = "I am a trial user";
        }
        return string.Format("Choose {0} Method", Form0.smethod_8());
    }

    ButtonBehaviour Interface0.imethod_1()
    {
        return (ButtonBehaviour.Cancel | ButtonBehaviour.Next);
    }

    public Enum13 method_6()
    {
        if (this.rbEnterActivationKey.Checked)
        {
            return Enum13.const_1;
        }
        return Enum13.const_0;
    }

    void WizardPage.Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_1 != null))
        {
            this.icontainer_1.Dispose();
        }
        base.Dispose(disposing);
    }
}

