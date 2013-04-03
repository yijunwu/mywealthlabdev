using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

internal class Class58 : Panel
{
    private bool bool_0;
    private bool bool_1;
    private bool bool_2;
    private bool bool_3;
    private Color color_0 = VisualStyleInformation.TextControlBorder;
    private float float_0 = 1f;
    private IContainer icontainer_0;

    public Class58()
    {
        this.method_0();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void method_0()
    {
        this.icontainer_0 = new Container();
    }

    public float method_1()
    {
        return this.float_0;
    }

    public void method_10(bool bool_4)
    {
        this.bool_1 = bool_4;
    }

    public bool method_11()
    {
        return this.bool_0;
    }

    public void method_12(bool bool_4)
    {
        this.bool_0 = bool_4;
    }

    public void method_2(float float_1)
    {
        this.float_0 = float_1;
    }

    public Color method_3()
    {
        return this.color_0;
    }

    public void method_4(Color color_1)
    {
        this.color_0 = color_1;
    }

    public bool method_5()
    {
        return this.bool_3;
    }

    public void method_6(bool bool_4)
    {
        this.bool_3 = bool_4;
    }

    public bool method_7()
    {
        return this.bool_2;
    }

    public void method_8(bool bool_4)
    {
        this.bool_2 = bool_4;
    }

    public bool method_9()
    {
        return this.bool_1;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        using (Pen pen = new Pen(this.method_3(), this.float_0))
        {
            if (this.bool_1)
            {
                pevent.Graphics.DrawLine(pen, base.ClientRectangle.X, base.ClientRectangle.Y, base.ClientRectangle.X, base.ClientRectangle.Y + base.Height);
            }
            if (this.bool_0)
            {
                pevent.Graphics.DrawLine(pen, base.ClientRectangle.X, base.ClientRectangle.Y, base.ClientRectangle.X + base.Width, base.ClientRectangle.Y);
            }
            if (this.bool_2)
            {
                pevent.Graphics.DrawLine(pen, (base.ClientRectangle.X + base.Width) - 1, base.ClientRectangle.Y, (base.ClientRectangle.X + base.Width) - 1, base.ClientRectangle.Y + base.Height);
            }
            if (this.bool_3)
            {
                pevent.Graphics.DrawLine(pen, base.ClientRectangle.X, (base.ClientRectangle.Y + base.Height) - 1, (base.ClientRectangle.X + base.Width) - 1, (base.ClientRectangle.Y + base.Height) - 1);
            }
        }
        base.OnPaint(pevent);
    }
}

