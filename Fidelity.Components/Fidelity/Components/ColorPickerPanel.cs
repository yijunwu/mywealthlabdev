using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Fidelity.Components
{
    [DefaultEvent("ColorChanged")]
    [ToolboxBitmap(typeof(ColorPickerPanel), "ColorPickerPanel")]
    public class ColorPickerPanel : Control
    {
        private EventHandler<EventArgs> eventHandler_0;

        private Color color_0 = Color.Black;

        private bool bool_0;

        private IContainer icontainer_0;

        private ColorDialog colorDialog_0;

        public bool DrawOutline
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public Color OutlineColor
        {
            get
            {
                return this.color_0;
            }
            set
            {
                this.color_0 = value;
            }
        }

        public ColorPickerPanel()
        {
            this.method_0();
            this.Cursor = Cursors.Hand;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.icontainer_0 != null)
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void method_0()
        {
            this.colorDialog_0 = new ColorDialog();
            base.SuspendLayout();
            this.colorDialog_0.AnyColor = true;
            base.ResumeLayout(false);
        }

        protected override void OnClick(EventArgs eventArgs_0)
        {
            this.colorDialog_0.Color = this.BackColor;
            if (this.colorDialog_0.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = this.colorDialog_0.Color;
                if (this.eventHandler_0 != null)
                {
                    this.eventHandler_0(this, EventArgs.Empty);
                }
            }
            base.OnClick(eventArgs_0);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            if (this.DrawOutline)
            {
                Pen pen = new Pen(this.OutlineColor);
                using (pen)
                {
                    pevent.Graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
                }
            }
        }

        public event EventHandler<EventArgs> ColorChanged
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }
    }
}

