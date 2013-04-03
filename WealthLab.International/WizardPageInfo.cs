using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WealthLab.International;

internal class WizardPageInfo : WizardPage, Interface0
{
    private bool bool_0;
    private CheckBox cbRemember;
    private Delegate20 delegate20_0;
    private IContainer icontainer_1;
    private Label lblInfo;
    private LinkLabel lnkCopyToClipboard;
    private LinkLabel lnkMaintenance;
    private string string_1 = string.Empty;
    private ToolTip toolTip_0;

    public WizardPageInfo()
    {
        this.InitializeComponent_1();
    }

    private void InitializeComponent_1()
    {
        this.icontainer_1 = new Container();
        this.lblInfo = new Label();
        this.lnkCopyToClipboard = new LinkLabel();
        this.cbRemember = new CheckBox();
        this.toolTip_0 = new ToolTip(this.icontainer_1);
        this.lnkMaintenance = new LinkLabel();
        base.SuspendLayout();
        this.lblInfo.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.lblInfo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblInfo.ForeColor = Color.Green;
        this.lblInfo.Location = new Point(9, 12);
        this.lblInfo.Name = "lblInfo";
        this.lblInfo.Size = new Size(0x1e1, 0x72);
        this.lblInfo.TabIndex = 0;
        this.lblInfo.Text = "lblInfo";
        this.lblInfo.TextAlign = ContentAlignment.MiddleCenter;
        this.lnkCopyToClipboard.AutoSize = true;
        this.lnkCopyToClipboard.LinkBehavior = LinkBehavior.AlwaysUnderline;
        this.lnkCopyToClipboard.Location = new Point(6, 130);
        this.lnkCopyToClipboard.Name = "lnkCopyToClipboard";
        this.lnkCopyToClipboard.Size = new Size(0x92, 13);
        this.lnkCopyToClipboard.TabIndex = 3;
        this.lnkCopyToClipboard.TabStop = true;
        this.lnkCopyToClipboard.Text = "Copy error details to clipboard";
        this.lnkCopyToClipboard.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkCopyToClipboard_LinkClicked);
        this.cbRemember.AutoSize = true;
        this.cbRemember.Location = new Point(0xa7, 0x81);
        this.cbRemember.Name = "cbRemember";
        this.cbRemember.Size = new Size(0xc3, 0x11);
        this.cbRemember.TabIndex = 4;
        this.cbRemember.Text = "Save data for future authentications";
        this.toolTip_0.SetToolTip(this.cbRemember, "Check this option to have your logon credentials (including key) automatically filled on next authentication.");
        this.cbRemember.UseVisualStyleBackColor = true;
        this.cbRemember.Visible = false;
        this.lnkMaintenance.AutoSize = true;
        this.lnkMaintenance.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lnkMaintenance.LinkBehavior = LinkBehavior.AlwaysUnderline;
        this.lnkMaintenance.Location = new Point(0x16a, 130);
        this.lnkMaintenance.Name = "lnkMaintenance";
        this.lnkMaintenance.Size = new Size(0x7b, 13);
        this.lnkMaintenance.TabIndex = 5;
        this.lnkMaintenance.TabStop = true;
        this.lnkMaintenance.Text = "Renew Maintenance";
        this.lnkMaintenance.Visible = false;
        this.lnkMaintenance.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkMaintenance_LinkClicked);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.Controls.Add(this.lnkMaintenance);
        base.Controls.Add(this.cbRemember);
        base.Controls.Add(this.lnkCopyToClipboard);
        base.Controls.Add(this.lblInfo);
        base.Name = "WizardPageInfo";
        base.Controls.SetChildIndex(this.lblInfo, 0);
        base.Controls.SetChildIndex(this.lnkCopyToClipboard, 0);
        base.Controls.SetChildIndex(this.cbRemember, 0);
        base.Controls.SetChildIndex(this.lnkMaintenance, 0);
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    string Interface0.imethod_0()
    {
        return base.method_0();
    }

    ButtonBehaviour Interface0.imethod_1()
    {
        return ButtonBehaviour.Cancel;
    }

    private void lnkCopyToClipboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        if (this.delegate20_0 != null)
        {
            this.delegate20_0(this, new EventArgs14(this.lblInfo.Text));
        }
    }

    private void lnkMaintenance_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            Process.Start(this.string_1);
        }
        catch
        {
        }
    }

    public void method_10(bool bool_1)
    {
        this.lnkMaintenance.Visible = bool_1;
    }

    public string method_11()
    {
        return this.string_1;
    }

    public void method_12(string string_2)
    {
        this.string_1 = string_2;
    }

    public void method_13(string string_2)
    {
        this.bool_0 = false;
        this.lblInfo.Text = string_2;
    }

    public bool method_14()
    {
        return this.cbRemember.Checked;
    }

    public void method_15(Enum15 enum15_0)
    {
        this.lnkCopyToClipboard.Visible = false;
        this.lnkMaintenance.Visible = false;
        this.cbRemember.Visible = false;
        switch (enum15_0)
        {
            case Enum15.const_0:
                this.lblInfo.ForeColor = Color.Green;
                this.cbRemember.Visible = true;
                this.cbRemember.Location = new Point(6, 0x81);
                this.cbRemember.Checked = Form0.smethod_0().method_8();
                return;

            case Enum15.const_1:
                this.lblInfo.ForeColor = Color.Brown;
                this.lnkCopyToClipboard.Visible = true;
                return;

            case Enum15.const_2:
                this.lblInfo.ForeColor = SystemColors.WindowText;
                return;
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

    public bool method_8()
    {
        return this.bool_0;
    }

    public void method_9(bool bool_1)
    {
        this.bool_0 = bool_1;
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

