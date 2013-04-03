using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WealthLab.Extensions.Agent;

internal class CopyDataForm : Form
{
    private Delegate16 delegate16_0;
    private IContainer icontainer_0;

    public CopyDataForm()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        base.SuspendLayout();
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.ClientSize = new Size(0x124, 0x10f);
        base.Name = "CopyDataForm";
        base.ShowInTaskbar = false;
        base.StartPosition = FormStartPosition.Manual;
        this.Text = "EM_CopyDataForm";
        base.ResumeLayout(false);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_0(Delegate16 delegate16_1)
    {
        this.delegate16_0 = (Delegate16) Delegate.Combine(this.delegate16_0, delegate16_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_1(Delegate16 delegate16_1)
    {
        this.delegate16_0 = (Delegate16) Delegate.Remove(this.delegate16_0, delegate16_1);
    }

    //void Form.Dispose(bool disposing)
    void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }

    //void Form.WndProc(ref Message message_0)
    void WndProc(ref Message message_0)
    {
        if (message_0.Msg == 0x4a)
        {
            string str = InterProcessCopyData.ReceiveData(message_0);
            if ((str != null) && (this.delegate16_0 != null))
            {
                this.delegate16_0(this, new EventArgs13(str));
            }
        }
        base.WndProc(ref message_0);
    }

    public delegate void Delegate16(object sender, EventArgs13 e);
}

