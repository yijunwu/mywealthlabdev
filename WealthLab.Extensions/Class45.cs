using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

internal class Class45 : Panel
{
    private EventHandler eventHandler_0;
    private int int_0 = -1;

    public Class45()
    {
        this.BackColor = SystemColors.Window;
        this.AutoScroll = true;
        base.SizeChanged += new EventHandler(this.Class45_SizeChanged);
    }

    private void Class45_SizeChanged(object sender, EventArgs e)
    {
        this.method_5();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_0(EventHandler eventHandler_1)
    {
        this.eventHandler_0 = (EventHandler) Delegate.Combine(this.eventHandler_0, eventHandler_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_1(EventHandler eventHandler_1)
    {
        this.eventHandler_0 = (EventHandler) Delegate.Remove(this.eventHandler_0, eventHandler_1);
    }

    public void method_10()
    {
        int y = base.AutoScrollPosition.Y;
        foreach (Control control in base.Controls)
        {
            control.Location = new Point(0, y);
            y += control.Height;
        }
    }

    public void method_11()
    {
        base.Controls.Clear();
        base.Controls.Clear();
        this.int_0 = -1;
    }

    public void method_12(Control control_0)
    {
        if (base.Controls.IndexOf(control_0) == this.int_0)
        {
            this.int_0 = -1;
        }
        base.Controls.Remove(control_0);
        base.Controls.Remove(control_0);
        this.method_10();
    }

    public int method_2()
    {
        return this.int_0;
    }

    public void method_3(int int_1)
    {
        if (int_1 < base.Controls.Count)
        {
            this.int_0 = int_1;
        }
    }

    public Control method_4()
    {
        if (this.int_0 > -1)
        {
            return base.Controls[this.int_0];
        }
        return null;
    }

    public void method_5()
    {
        foreach (Control control in base.Controls)
        {
            if (base.VerticalScroll.Visible)
            {
                control.Width = base.Width - SystemInformation.VerticalScrollBarWidth;
            }
            else
            {
                control.Width = base.Width;
            }
        }
    }

    public void method_6(Control[] control_0)
    {
        foreach (Control control in control_0)
        {
            this.method_7(control);
        }
    }

    public void method_7(Control control_0)
    {
        int y = 0;
        if (base.Controls.Count > 0)
        {
            y = base.Controls[base.Controls.Count - 1].Location.Y + base.Controls[base.Controls.Count - 1].Height;
        }
        control_0.Location = new Point(0, y);
        control_0.Width = base.ClientRectangle.Width;
        control_0.Click += new EventHandler(this.method_9);
        base.Controls.Add(control_0);
    }

    public void method_8(Control control_0)
    {
        int index = base.Controls.IndexOf(control_0);
        if (this.int_0 != index)
        {
            if (this.int_0 > -1)
            {
                base.Controls[this.int_0].Invalidate();
            }
            base.Controls[index].Invalidate();
            this.int_0 = index;
            if (this.eventHandler_0 != null)
            {
                this.eventHandler_0(this, EventArgs.Empty);
            }
        }
        this.method_10();
    }

    private void method_9(object sender, EventArgs e)
    {
        this.method_8(sender as Control);
    }

    protected override void OnPaint(PaintEventArgs paintEventArgs_0)
    {
    }
}

