namespace WealthLab
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using Panel = System.Windows.Forms.Panel;

    [ToolboxBitmap(typeof(ParameterSlidersContainer), "ParameterSlidersContainer")]
    public class ParameterSlidersContainer : UserControl
    {
        private IContainer icontainer_0;
        private Panel pnlParameters;
        private Panel pnlParamHousing;
        private WealthLab.WealthScript wealthScript_0;

        private EventHandler<EventArgs> eventHandler_0;

        private EventHandler<EventArgs> eventHandler_1;


        public event EventHandler<EventArgs> SliderMouseDown
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<EventArgs> eventHandler2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<EventArgs> eventHandler2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
        }

        public event EventHandler<EventArgs> SliderValueChanged
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

        public ParameterSlidersContainer()
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
            this.pnlParamHousing = new Panel();
            this.pnlParameters = new Panel();
            this.pnlParamHousing.SuspendLayout();
            base.SuspendLayout();
            this.pnlParamHousing.AutoScroll = true;
            this.pnlParamHousing.BackColor = SystemColors.Window;
            this.pnlParamHousing.Controls.Add(this.pnlParameters);
            this.pnlParamHousing.Dock = DockStyle.Fill;
            this.pnlParamHousing.Location = new Point(0, 0);
            this.pnlParamHousing.Name = "pnlParamHousing";
            this.pnlParamHousing.Size = new Size(0xa5, 0x72);
            this.pnlParamHousing.TabIndex = 3;
            this.pnlParamHousing.Resize += new EventHandler(this.pnlParamHousing_Resize);
            this.pnlParameters.BackColor = SystemColors.Window;
            this.pnlParameters.Location = new Point(0, 0);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.Size = new Size(0x87, 0x2f);
            this.pnlParameters.TabIndex = 0;
            this.pnlParameters.Resize += new EventHandler(this.pnlParameters_Resize);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(this.pnlParamHousing);
            base.Name = "ParameterSlidersContainer";
            base.Size = new Size(0xa5, 0x72);
            this.pnlParamHousing.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void method_0(object sender, EventArgs e)
        {
            if (this.eventHandler_0 != null)
            {
                this.eventHandler_0(sender, EventArgs.Empty);
            }
        }

        private void method_1(object sender, EventArgs e)
        {
            if (this.eventHandler_1 != null)
            {
                this.eventHandler_1(sender, e);
            }
        }

        private void pnlParameters_Resize(object sender, EventArgs e)
        {
            foreach (Control control in this.pnlParameters.Controls)
            {
                control.Width = this.pnlParameters.Width - 4;
            }
        }

        private void pnlParamHousing_Resize(object sender, EventArgs e)
        {
            this.pnlParameters.Width = this.pnlParamHousing.Width - 0x18;
            this.pnlParamHousing.Invalidate();
        }

        public void StopMouseDrag()
        {
            foreach (Control control in this.pnlParameters.Controls)
            {
                if (control is ParameterSlider)
                {
                    (control as ParameterSlider).StopMouseDrag();
                }
            }
        }

        public WealthLab.WealthScript WealthScript
        {
            get
            {
                return this.wealthScript_0;
            }
            set
            {
                this.pnlParameters.Controls.Clear();
                this.wealthScript_0 = value;
                if (this.wealthScript_0 == null)
                {
                    this.pnlParameters.Height = 4;
                    return;
                }
                else
                {
                    foreach (StrategyParameter parameter in this.wealthScript_0.Parameters)
                    {
                        ParameterSlider parameterSlider = new ParameterSlider();
                        parameterSlider.Width = this.pnlParameters.Width - 4;
                        parameterSlider.Left = 2;
                        parameterSlider.Top = this.pnlParameters.Controls.Count * parameterSlider.Height;
                        parameterSlider.Parameter = parameter;
                        parameterSlider.ValueChanged += new EventHandler<EventArgs>(this.method_0);
                        parameterSlider.MouseDown += new MouseEventHandler(this.method_1);
                        this.pnlParameters.Controls.Add(parameterSlider);
                    }
                    this.pnlParameters.Height = this.pnlParameters.Controls.Count * 17 + 4;
                    return;
                }
            }
        }
    }
}

