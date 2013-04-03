using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WealthLab.International;

internal class WizardPageError : WizardPage, Interface0
{
    private Delegate20 delegate20_0;
    private IContainer icontainer_1;
    private Label lblError;
    private LinkLabel lnkCopyToClipboard;
    private TextBox txtError;

    public WizardPageError()
    {
        this.InitializeComponent_1();
    }

    public ButtonBehaviour imethod_1()
    {
        return (ButtonBehaviour.Cancel | ButtonBehaviour.Previous);
    }

    private void InitializeComponent_1()
    {
        this.lblError = new Label();
        this.txtError = new TextBox();
        this.lnkCopyToClipboard = new LinkLabel();
        base.SuspendLayout();
        this.lblError.AutoSize = true;
        this.lblError.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblError.ForeColor = Color.Red;
        this.lblError.Location = new Point(9, 3);
        this.lblError.Name = "lblError";
        this.lblError.Size = new Size(0x26, 13);
        this.lblError.TabIndex = 0;
        this.lblError.Text = "Error:";
        this.txtError.Location = new Point(13, 0x13);
        this.txtError.Multiline = true;
        this.txtError.Name = "txtError";
        this.txtError.ReadOnly = true;
        this.txtError.ScrollBars = ScrollBars.Both;
        this.txtError.Size = new Size(0x1d9, 0x6f);
        this.txtError.TabIndex = 1;
        this.lnkCopyToClipboard.AutoSize = true;
        this.lnkCopyToClipboard.LinkBehavior = LinkBehavior.AlwaysUnderline;
        this.lnkCopyToClipboard.Location = new Point(10, 0x85);
        this.lnkCopyToClipboard.Name = "lnkCopyToClipboard";
        this.lnkCopyToClipboard.Size = new Size(0x92, 13);
        this.lnkCopyToClipboard.TabIndex = 2;
        this.lnkCopyToClipboard.TabStop = true;
        this.lnkCopyToClipboard.Text = "Copy error details to clipboard";
        this.lnkCopyToClipboard.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkCopyToClipboard_LinkClicked);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.Controls.Add(this.lnkCopyToClipboard);
        base.Controls.Add(this.txtError);
        base.Controls.Add(this.lblError);
        base.Name = "WizardPageError";
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    string Interface0.imethod_0()
    {
        return base.method_0();
    }

    private void lnkCopyToClipboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        if (this.delegate20_0 != null)
        {
            this.delegate20_0(this, new EventArgs14("Exception - " + this.txtError.Text));
        }
    }

    public void method_6(Delegate20 delegate20_1)
    {
        Delegate20 delegate3;
        Delegate20 delegate2 = this.delegate20_0;
        do
        {
            delegate3 = delegate2;
            Delegate20 delegate4 = (Delegate20) Delegate.Combine(delegate3, delegate20_1);
            delegate2 = Interlocked.CompareExchange<Delegate20>(ref this.delegate20_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public void method_7(Delegate20 delegate20_1)
    {
        Delegate20 delegate3;
        Delegate20 delegate2 = this.delegate20_0;
        do
        {
            delegate3 = delegate2;
            Delegate20 delegate4 = (Delegate20) Delegate.Remove(delegate3, delegate20_1);
            delegate2 = Interlocked.CompareExchange<Delegate20>(ref this.delegate20_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public void method_8(string string_1)
    {
        this.txtError.Text = string_1;
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

