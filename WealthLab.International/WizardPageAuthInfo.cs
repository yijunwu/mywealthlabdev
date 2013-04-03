using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.International;

internal class WizardPageAuthInfo : WizardPage, Interface0
{
    private CheckBox cbNotShow;
    private IContainer icontainer_1;
    private Label lblInfo;
    private Label lblInfo2;

    public WizardPageAuthInfo()
    {
        this.InitializeComponent_1();
    }

    private void InitializeComponent_1()
    {
        ComponentResourceManager manager = new ComponentResourceManager(typeof(WizardPageAuthInfo));
        this.lblInfo = new Label();
        this.cbNotShow = new CheckBox();
        this.lblInfo2 = new Label();
        base.SuspendLayout();
        this.lblInfo.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.lblInfo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblInfo.ForeColor = SystemColors.ControlText;
        this.lblInfo.Location = new Point(7, 0);
        this.lblInfo.Name = "lblInfo";
        this.lblInfo.Size = new Size(0x1e4, 0x3d);
        this.lblInfo.TabIndex = 0;
        this.lblInfo.Text = "Registered users need to authenticate once every 30 days with an Activation Key in order to continue using the application on their PCs. An Activation Key is provided after ordering the application.";
        this.lblInfo.TextAlign = ContentAlignment.MiddleCenter;
        this.cbNotShow.AutoSize = true;
        this.cbNotShow.Location = new Point(6, 0x83);
        this.cbNotShow.Name = "cbNotShow";
        this.cbNotShow.Size = new Size(0x9a, 0x11);
        this.cbNotShow.TabIndex = 1;
        this.cbNotShow.Text = "Don't show this page again";
        this.cbNotShow.UseVisualStyleBackColor = true;
        this.lblInfo2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.lblInfo2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lblInfo2.ForeColor = SystemColors.ControlText;
        this.lblInfo2.Location = new Point(7, 0x3d);
        this.lblInfo2.Name = "lblInfo2";
        this.lblInfo2.Size = new Size(0x1e6, 60);
        this.lblInfo2.TabIndex = 2;
        this.lblInfo2.Text = manager.GetString("lblInfo2.Text");
        this.lblInfo2.TextAlign = ContentAlignment.MiddleCenter;
        this.lblInfo2.Click += new EventHandler(this.lblInfo2_Click);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        this.BackColor = SystemColors.Control;
        base.Controls.Add(this.lblInfo2);
        base.Controls.Add(this.cbNotShow);
        base.Controls.Add(this.lblInfo);
        base.Name = "WizardPageAuthInfo";
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    string Interface0.imethod_0()
    {
        return "Authentication information";
    }

    ButtonBehaviour Interface0.imethod_1()
    {
        return (ButtonBehaviour.Cancel | ButtonBehaviour.Next);
    }

    private void lblInfo2_Click(object sender, EventArgs e)
    {
    }

    public bool method_6()
    {
        return this.cbNotShow.Checked;
    }

    private void method_7(object sender, EventArgs e)
    {
        if (this.lblInfo.Visible)
        {
            MessageBox.Show("Visible");
        }
        MessageBox.Show(this.lblInfo.Location.ToString());
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

