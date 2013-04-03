namespace WealthLab.DataProviders.Helper
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BorderLabel : Label
    {
        private Color color_0 = Color.Transparent;
        private IContainer icontainer_0;
        private int int_0 = 1;
        private Pen pen_0;

        public BorderLabel()
        {
            this.method_1();
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
            this.pen_0 = new Pen(this.color_0, (float) this.int_0);
        }

        private void method_1()
        {
            this.icontainer_0 = new Container();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Rectangle rect = new Rectangle(base.ClientRectangle.Location, new Size(base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 1));
            pevent.Graphics.DrawRectangle(this.pen_0, rect);
            base.OnPaint(pevent);
        }

        public Color BorderColor
        {
            get
            {
                return this.color_0;
            }
            set
            {
                this.color_0 = value;
                this.method_0();
            }
        }

        public int BorderWidth
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
                this.method_0();
            }
        }
    }
}

