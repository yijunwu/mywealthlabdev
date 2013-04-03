using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

internal class WizardPage : UserControl
{
    private ErrorProvider errorProvider_0;
    private IContainer icontainer_0;
    private LinkLabel lnkBuy;
    private string string_0;

    public WizardPage()
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
        this.icontainer_0 = new Container();
        ComponentResourceManager manager = new ComponentResourceManager(typeof(WizardPage));
        this.errorProvider_0 = new ErrorProvider(this.icontainer_0);
        this.lnkBuy = new LinkLabel();
        ((ISupportInitialize) this.errorProvider_0).BeginInit();
        base.SuspendLayout();
        this.errorProvider_0.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        this.errorProvider_0.ContainerControl = this;
        this.errorProvider_0.Icon = (Icon) manager.GetObject("errProvider.Icon");
        this.errorProvider_0.RightToLeft = true;
        this.lnkBuy.AutoSize = true;
        this.lnkBuy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.lnkBuy.LinkBehavior = LinkBehavior.AlwaysUnderline;
        this.lnkBuy.Location = new Point(11, 0x7e);
        this.lnkBuy.Name = "lnkBuy";
        this.lnkBuy.Size = new Size(0x39, 13);
        this.lnkBuy.TabIndex = 0;
        this.lnkBuy.TabStop = true;
        this.lnkBuy.Text = "Buy Now";
        this.lnkBuy.Visible = false;
        this.lnkBuy.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkBuy_LinkClicked);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.lnkBuy);
        base.Name = "WizardPage";
        base.Size = new Size(500, 150);
        ((ISupportInitialize) this.errorProvider_0).EndInit();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    private void lnkBuy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            Process.Start("http://www.wealth-lab.com/Products/PriceLicence/Default.aspx?id=XUZKymEygQfc9K49ZCYwkw==");
        }
        catch
        {
        }
    }

    public string method_0()
    {
        return this.string_0;
    }

    public void method_1(string string_1)
    {
        this.string_0 = string_1;
    }

    public void method_2()
    {
        this.errorProvider_0.Clear();
    }

    public void method_3(bool bool_0)
    {
        this.lnkBuy.Visible = bool_0;
    }

    public bool method_4(Control control_0)
    {
        if ((control_0 is TextBox) && ((control_0 as TextBox).Text.Trim() == string.Empty))
        {
            this.method_5(control_0);
            return false;
        }
        if ((control_0 is ComboBox) && ((control_0 as ComboBox).Text.Trim() == string.Empty))
        {
            this.method_5(control_0);
            return false;
        }
        return true;
    }

    private void method_5(Control control_0)
    {
        this.errorProvider_0.SetError(control_0, "Field is empty!");
    }

    public virtual bool vmethod_0()
    {
        return true;
    }

    public virtual void vmethod_1()
    {
    }
}

