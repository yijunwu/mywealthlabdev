using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.International;

internal class WizardPageProgress : WizardPage, Interface0
{
    private IContainer icontainer_1;
    private Label lblAction;
    private Label lblPleaseWait;
    private ProgressBar prbRequest;

    public WizardPageProgress()
    {
        this.InitializeComponent_1();
    }

    private void InitializeComponent_1()
    {
        this.lblAction = new Label();
        this.prbRequest = new ProgressBar();
        this.lblPleaseWait = new Label();
        base.SuspendLayout();
        this.lblAction.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.lblAction.Location = new Point(5, 0x36);
        this.lblAction.Name = "lblAction";
        this.lblAction.Size = new Size(490, 0x12);
        this.lblAction.TabIndex = 1;
        this.lblAction.Text = "lblAction";
        this.lblAction.TextAlign = ContentAlignment.MiddleCenter;
        this.prbRequest.Location = new Point(100, 0x4c);
        this.prbRequest.Name = "prbRequest";
        this.prbRequest.Size = new Size(300, 0x13);
        this.prbRequest.Style = ProgressBarStyle.Marquee;
        this.prbRequest.TabIndex = 2;
        this.lblPleaseWait.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
        this.lblPleaseWait.Location = new Point(5, 0x24);
        this.lblPleaseWait.Name = "lblPleaseWait";
        this.lblPleaseWait.Size = new Size(490, 0x12);
        this.lblPleaseWait.TabIndex = 0;
        this.lblPleaseWait.Text = "Please wait...";
        this.lblPleaseWait.TextAlign = ContentAlignment.MiddleCenter;
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.Controls.Add(this.lblPleaseWait);
        base.Controls.Add(this.prbRequest);
        base.Controls.Add(this.lblAction);
        base.Name = "WizardPageProgress";
        base.ResumeLayout(false);
    }

    string Interface0.imethod_0()
    {
        return base.method_0();
    }

    ButtonBehaviour Interface0.imethod_1()
    {
        return ButtonBehaviour.None;
    }

    public string method_6()
    {
        return this.lblAction.Text;
    }

    public void method_7(string string_1)
    {
        this.lblAction.Text = string_1;
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

