namespace QWhale.Common
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface IControl
    {
        event EventHandler Click;

        void BringToFront();
        Graphics CreateGraphics();
        Form FindForm();
        bool Focus();
        void Invalidate();
        void Invalidate(Rectangle rect);
        void Invalidate(Region region);
        void Invalidate(Region region, bool invalidateChildren);
        Point PointToClient(Point p);
        Point PointToScreen(Point p);
        void Update();

        Color BackColor { get; set; }

        Image BackgroundImage { get; set; }

        Rectangle Bounds { get; set; }

        bool CanFocus { get; }

        Rectangle ClientRectangle { get; }

        System.Windows.Forms.ContextMenu ContextMenu { get; set; }

        System.Windows.Forms.ContextMenuStrip ContextMenuStrip { get; set; }

        bool Created { get; }

        DockStyle Dock { get; set; }

        bool Enabled { get; set; }

        bool Focused { get; }

        System.Drawing.Font Font { get; set; }

        Color ForeColor { get; set; }

        IntPtr Handle { get; }

        int Height { get; set; }

        bool IsHandleCreated { get; }

        int Left { get; set; }

        Point Location { get; set; }

        Control Parent { get; set; }

        int Top { get; set; }

        bool Visible { get; set; }

        int Width { get; set; }
    }
}

