using System;
using System.Drawing;
using System.Windows.Forms;

internal class Class8 : Panel
{
    private Color color_0 = Color.Transparent;
    private int int_0 = 1;
    private Pen pen_0;

    public Class8()
    {
        this.method_4();
        base.SizeChanged += new EventHandler(this.Class8_SizeChanged);
    }

    private void Class8_SizeChanged(object sender, EventArgs e)
    {
        this.Refresh();
    }

    public Color method_0()
    {
        return this.color_0;
    }

    public void method_1(Color color_1)
    {
        this.color_0 = color_1;
        this.method_4();
    }

    public int method_2()
    {
        return this.int_0;
    }

    public void method_3(int int_1)
    {
        this.int_0 = int_1;
        this.method_4();
    }

    private void method_4()
    {
        this.pen_0 = new Pen(this.color_0, (float) this.int_0);
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        Rectangle rect = new Rectangle(base.ClientRectangle.Location, new Size(base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 1));
        pevent.Graphics.DrawRectangle(this.pen_0, rect);
    }
}

