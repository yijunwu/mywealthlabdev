using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

internal class Class47 : Panel
{
    private Color color_0;
    private Pen pen_0;

    public Class47()
    {
        base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        base.UpdateStyles();
        this.BackColor = SystemColors.Window;
        this.color_0 = VisualStyleInformation.TextControlBorder;
        base.BorderStyle = BorderStyle.None;
        this.pen_0 = new Pen(this.color_0);
    }

    public Color method_0()
    {
        return this.color_0;
    }

    public void method_1(Color color_1)
    {
        this.color_0 = color_1;
    }

    protected override void OnPaint(PaintEventArgs paintEventArgs_0)
    {
        base.OnPaint(paintEventArgs_0);
        Rectangle rect = new Rectangle(base.ClientRectangle.Location, new Size(base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 1));
        paintEventArgs_0.Graphics.DrawRectangle(this.pen_0, rect);
    }
}

