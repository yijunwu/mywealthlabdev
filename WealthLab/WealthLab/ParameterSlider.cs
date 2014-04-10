namespace WealthLab
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using Panel = System.Windows.Forms.Panel;

    [ToolboxBitmap(typeof(ParameterSlider), "ParameterSlider")]
    public class ParameterSlider : UserControl
    {
        private EventHandler<EventArgs> eventHandler_0;
        private bool mouseDragging;
        private Font font;
        private IContainer components;
        private LinkLabel linkParameter;
        private Panel pnlSlider;
        private StrategyParameter strategyParameter;

        public event EventHandler<EventArgs> ValueChanged
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

        public ParameterSlider()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlSlider = new Panel();
            this.linkParameter = new LinkLabel();
            base.SuspendLayout();
            this.pnlSlider.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pnlSlider.BackColor = Color.Azure;
            this.pnlSlider.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSlider.Cursor = Cursors.Hand;
            this.pnlSlider.Location = new Point(0x4a, 2);
            this.pnlSlider.Name = "pnlSlider";
            this.pnlSlider.Size = new Size(0x6a, 13);
            this.pnlSlider.TabIndex = 1;
            this.pnlSlider.Paint += new PaintEventHandler(this.pnlSlider_Paint);
            this.pnlSlider.MouseMove += new MouseEventHandler(this.pnlSlider_MouseMove);
            this.pnlSlider.MouseDown += new MouseEventHandler(this.pnlSlider_MouseDown);
            this.pnlSlider.MouseUp += new MouseEventHandler(this.pnlSlider_MouseUp);
            this.linkParameter.AutoSize = true;
            this.linkParameter.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.linkParameter.Location = new Point(4, 2);
            this.linkParameter.Name = "linkParameter";
            this.linkParameter.Size = new Size(0x33, 12);
            this.linkParameter.TabIndex = 2;
            this.linkParameter.TabStop = true;
            this.linkParameter.Text = "Parameter:";
            this.linkParameter.DoubleClick += new EventHandler(this.linkParameter_Click);
            this.linkParameter.Click += new EventHandler(this.linkParameter_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(this.pnlSlider);
            base.Controls.Add(this.linkParameter);
            base.Name = "ParameterSlider";
            base.Size = new Size(0xb7, 0x11);
            base.Resize += new EventHandler(this.ParameterSlider_Resize);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void linkParameter_Click(object sender, EventArgs e)
        {
            ParameterForm form = new ParameterForm(this.linkParameter.Text, this.Parameter.Value, this.Parameter.Start, this.Parameter.Stop, this.Parameter.Step);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.method_1(form.NewValue);
            }
        }

        private void method_0(int int_0)
        {
            if (this.strategyParameter != null)
            {
                double num = ((double) int_0) / ((double) this.pnlSlider.Width);
                double stop = ((this.strategyParameter.Stop - this.strategyParameter.Start) * num) + this.strategyParameter.Start;
                double start = this.strategyParameter.Start;
                while (stop > start)
                {
                    start += this.strategyParameter.Step;
                }
                double num4 = start - this.strategyParameter.Step;
                if (num4 < this.strategyParameter.Start)
                {
                    num4 = this.strategyParameter.Start;
                }
                if (start > this.strategyParameter.Stop)
                {
                    start = this.strategyParameter.Stop;
                }
                double num5 = Math.Abs((double) (stop - num4));
                double num6 = Math.Abs((double) (stop - start));
                stop = (num5 < num6) ? num4 : start;
                if (stop > this.strategyParameter.Stop)
                {
                    stop = this.strategyParameter.Stop;
                }
                if (stop < this.strategyParameter.Start)
                {
                    stop = this.strategyParameter.Start;
                }
                this.method_1(stop);
            }
        }

        private void method_1(double double_0)
        {
            if (double_0 != this.strategyParameter.Value)
            {
                this.strategyParameter.Value = double_0;
                this.pnlSlider.Invalidate();
                if (this.eventHandler_0 != null)
                {
                    this.eventHandler_0(this, EventArgs.Empty);
                }
            }
        }

        private void ParameterSlider_Resize(object sender, EventArgs e)
        {
            this.pnlSlider.Invalidate();
        }

        private void pnlSlider_MouseDown(object sender, MouseEventArgs e)
        {
            bool flag = this.mouseDragging;
            this.mouseDragging = true;
            this.method_0(e.X);
            if (!flag)
            {
                this.OnMouseDown(e);
            }
        }

        private void pnlSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mouseDragging)
            {
                this.method_0(e.X);
            }
        }

        private void pnlSlider_MouseUp(object sender, MouseEventArgs e)
        {
            this.mouseDragging = false;
        }

        private void pnlSlider_Paint(object sender, PaintEventArgs e)
        {
            if (this.strategyParameter != null)
            {
                if (this.strategyParameter.Start < this.strategyParameter.Stop)
                {
                    float num3 = (float) ((this.strategyParameter.Value - this.strategyParameter.Start) / (this.strategyParameter.Stop - this.strategyParameter.Start));
                    float num4 = this.pnlSlider.Width * num3;
                    Pen pen = new Pen(Color.Black) {
                        Width = 3f
                    };
                    using (pen)
                    {
                        e.Graphics.DrawLine(pen, num4, 0f, num4, (float) base.Height);
                    }
                }
                if (this.font == null)
                {
                    this.font = new Font(this.linkParameter.Font, FontStyle.Bold);
                }
                string s = this.strategyParameter.Value.ToString();
                double num2 = ((this.strategyParameter.Stop - this.strategyParameter.Start) / 2.0) + this.strategyParameter.Start;
                if (this.strategyParameter.Value > num2)
                {
                    e.Graphics.DrawString(s, this.font, Brushes.Navy, (float) 4f, (float) 0f);
                }
                else
                {
                    SizeF ef = e.Graphics.MeasureString(s, this.font);
                    e.Graphics.DrawString(s, this.font, Brushes.Navy, (float) ((this.pnlSlider.Width - ef.Width) - 4f), (float) 0f);
                }
            }
        }

        public void StopMouseDrag()
        {
            this.mouseDragging = false;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public StrategyParameter Parameter
        {
            get
            {
                return this.strategyParameter;
            }
            set
            {
                this.strategyParameter = value;
                if (this.strategyParameter != null)
                {
                    this.linkParameter.Text = this.strategyParameter.Name + ":";
                }
                base.Invalidate();
            }
        }
    }
}

