namespace WealthLab.ChartControl
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl.Properties;

    public class ScaleSelector : UserControl
    {
        private BarDataScale barDataScale_0 = new BarDataScale();
        private bool bool_0;
        [CompilerGenerated]
        private bool bool_1;
        private IContainer icontainer_0;
        private int int_0;
        private ToolStripLabel lblArrow;
        private ToolStrip toolScale;
        private ToolStripButton toolStripButton_0;
        private ToolStripButton tsb1;
        private ToolStripButton tsb10;
        private ToolStripButton tsb15;
        private ToolStripButton tsb30;
        private ToolStripButton tsb5;
        private ToolStripButton tsb60;
        private ToolStripButton tsbD;
        private ToolStripButton tsbM;
        private ToolStripButton tsbW;

        private EventHandler<ScaleChangeEventArgs> eventHandler_0;

        public event EventHandler<ScaleChangeEventArgs> ScaleChangeEvent
        {
            add
            {
                EventHandler<ScaleChangeEventArgs> eventHandler;
                EventHandler<ScaleChangeEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<ScaleChangeEventArgs> eventHandler1 = (EventHandler<ScaleChangeEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<ScaleChangeEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<ScaleChangeEventArgs> eventHandler;
                EventHandler<ScaleChangeEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<ScaleChangeEventArgs> eventHandler1 = (EventHandler<ScaleChangeEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<ScaleChangeEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }
        

        public ScaleSelector()
        {
            this.InitializeComponent();
            this.tsbD.Tag = new BarDataScale(BarScale.Daily, 1);
            this.tsbW.Tag = new BarDataScale(BarScale.Weekly, 1);
            this.tsbM.Tag = new BarDataScale(BarScale.Monthly, 1);
            this.tsb1.Tag = new BarDataScale(BarScale.Minute, 1);
            this.tsb5.Tag = new BarDataScale(BarScale.Minute, 5);
            this.tsb10.Tag = new BarDataScale(BarScale.Minute, 10);
            this.tsb15.Tag = new BarDataScale(BarScale.Minute, 15);
            this.tsb30.Tag = new BarDataScale(BarScale.Minute, 30);
            this.tsb60.Tag = new BarDataScale(BarScale.Minute, 60);
            this.int_0 = base.Width;
            this.method_1();
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
            this.toolScale = new ToolStrip();
            this.lblArrow = new ToolStripLabel();
            this.tsb60 = new ToolStripButton();
            this.tsb30 = new ToolStripButton();
            this.tsb15 = new ToolStripButton();
            this.tsb10 = new ToolStripButton();
            this.tsb5 = new ToolStripButton();
            this.tsb1 = new ToolStripButton();
            this.tsbM = new ToolStripButton();
            this.tsbW = new ToolStripButton();
            this.tsbD = new ToolStripButton();
            this.toolScale.SuspendLayout();
            base.SuspendLayout();
            this.toolScale.AutoSize = false;
            this.toolScale.BackColor = Color.Transparent;
            this.toolScale.CanOverflow = false;
            this.toolScale.Dock = DockStyle.Fill;
            this.toolScale.GripMargin = new Padding(0);
            this.toolScale.GripStyle = ToolStripGripStyle.Hidden;
            this.toolScale.Items.AddRange(new ToolStripItem[] { this.lblArrow, this.tsb60, this.tsb30, this.tsb15, this.tsb10, this.tsb5, this.tsb1, this.tsbM, this.tsbW, this.tsbD });
            this.toolScale.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolScale.Location = new Point(0, 0);
            this.toolScale.Name = "toolScale";
            this.toolScale.Padding = new Padding(0);
            this.toolScale.Size = new Size(0xed, 0x18);
            this.toolScale.TabIndex = 15;
            this.toolScale.MouseLeave += new EventHandler(this.toolScale_MouseLeave);
            this.lblArrow.AutoToolTip = true;
            this.lblArrow.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.lblArrow.Image = Resources.arrow;
            this.lblArrow.Name = "lblArrow";
            this.lblArrow.Size = new Size(0x10, 0x1b);
            this.lblArrow.Text = "Scale";
            this.lblArrow.ToolTipText = "Scaling";
            this.lblArrow.MouseHover += new EventHandler(this.lblArrow_MouseHover);
            this.tsb60.Alignment = ToolStripItemAlignment.Right;
            this.tsb60.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsb60.Image = Resources.sixty;
            this.tsb60.ImageTransparentColor = Color.Magenta;
            this.tsb60.Name = "tsb60";
            this.tsb60.Size = new Size(0x17, 0x1b);
            this.tsb60.Text = "60";
            this.tsb60.ToolTipText = "Display 60 minute Bars";
            this.tsb60.Click += new EventHandler(this.tsbD_Click);
            this.tsb30.Alignment = ToolStripItemAlignment.Right;
            this.tsb30.BackColor = Color.Transparent;
            this.tsb30.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsb30.ForeColor = SystemColors.ControlText;
            this.tsb30.Image = Resources.thirty;
            this.tsb30.ImageTransparentColor = Color.Magenta;
            this.tsb30.Name = "tsb30";
            this.tsb30.Size = new Size(0x17, 0x1b);
            this.tsb30.Text = "30";
            this.tsb30.ToolTipText = "Display 30 minute Bars";
            this.tsb30.Click += new EventHandler(this.tsbD_Click);
            this.tsb15.Alignment = ToolStripItemAlignment.Right;
            this.tsb15.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsb15.Image = Resources.fifteen;
            this.tsb15.ImageTransparentColor = Color.Magenta;
            this.tsb15.Name = "tsb15";
            this.tsb15.Size = new Size(0x17, 0x1b);
            this.tsb15.Text = "15";
            this.tsb15.ToolTipText = "Display 15 minute Bars";
            this.tsb15.Click += new EventHandler(this.tsbD_Click);
            this.tsb10.Alignment = ToolStripItemAlignment.Right;
            this.tsb10.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsb10.Image = Resources.Bitmap_1;
            this.tsb10.ImageTransparentColor = Color.Magenta;
            this.tsb10.Name = "tsb10";
            this.tsb10.Size = new Size(0x17, 0x1b);
            this.tsb10.Text = "10";
            this.tsb10.ToolTipText = "Display 10 minute Bars";
            this.tsb10.Click += new EventHandler(this.tsbD_Click);
            this.tsb5.Alignment = ToolStripItemAlignment.Right;
            this.tsb5.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsb5.Image = Resources.five;
            this.tsb5.ImageTransparentColor = Color.Magenta;
            this.tsb5.Name = "tsb5";
            this.tsb5.Size = new Size(0x17, 0x1b);
            this.tsb5.Text = "5";
            this.tsb5.ToolTipText = "Display 5 minute Bars";
            this.tsb5.Click += new EventHandler(this.tsbD_Click);
            this.tsb1.Alignment = ToolStripItemAlignment.Right;
            this.tsb1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsb1.Image = Resources.Bitmap_0;
            this.tsb1.ImageTransparentColor = Color.Magenta;
            this.tsb1.Name = "tsb1";
            this.tsb1.Size = new Size(0x17, 0x1b);
            this.tsb1.Text = "1";
            this.tsb1.ToolTipText = "Display 1 Minute Bars";
            this.tsb1.Click += new EventHandler(this.tsbD_Click);
            this.tsbM.Alignment = ToolStripItemAlignment.Right;
            this.tsbM.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbM.Image = Resources.M;
            this.tsbM.ImageTransparentColor = Color.Magenta;
            this.tsbM.Name = "tsbM";
            this.tsbM.Size = new Size(0x17, 0x1b);
            this.tsbM.Text = "M";
            this.tsbM.ToolTipText = "Monthly Chart Data";
            this.tsbM.Click += new EventHandler(this.tsbD_Click);
            this.tsbW.Alignment = ToolStripItemAlignment.Right;
            this.tsbW.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbW.Image = Resources.W;
            this.tsbW.ImageTransparentColor = Color.Magenta;
            this.tsbW.Name = "tsbW";
            this.tsbW.Size = new Size(0x17, 0x1b);
            this.tsbW.Text = "W";
            this.tsbW.ToolTipText = "Weekly Chart Data";
            this.tsbW.Click += new EventHandler(this.tsbD_Click);
            this.tsbD.Alignment = ToolStripItemAlignment.Right;
            this.tsbD.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbD.Image = Resources.D;
            this.tsbD.ImageTransparentColor = Color.Magenta;
            this.tsbD.Name = "tsbD";
            this.tsbD.Size = new Size(0x17, 0x15);
            this.tsbD.Text = "D";
            this.tsbD.TextImageRelation = TextImageRelation.Overlay;
            this.tsbD.ToolTipText = "Daily Chart Data";
            this.tsbD.Click += new EventHandler(this.tsbD_Click);
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = Color.Transparent;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.toolScale);
            base.Name = "ScaleSelector";
            base.Size = new Size(0xed, 0x18);
            this.toolScale.ResumeLayout(false);
            this.toolScale.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lblArrow_MouseHover(object sender, EventArgs e)
        {
            this.IsActive = true;
            base.Width = this.int_0;
            base.BorderStyle = BorderStyle.FixedSingle;
        }

        private void method_0(ScaleChangeEventArgs scaleChangeEventArgs_0)
        {
            EventHandler<ScaleChangeEventArgs> handler = this.eventHandler_0;
            if (handler != null)
            {
                scaleChangeEventArgs_0.ChartScale = this.barDataScale_0;
                handler(this, scaleChangeEventArgs_0);
            }
        }

        private void method_1()
        {
            base.BorderStyle = BorderStyle.None;
            base.Width = this.lblArrow.Width;
            this.IsActive = false;
        }

        private bool method_2(ToolStripButton toolStripButton_1)
        {
            if (toolStripButton_1 == this.toolStripButton_0)
            {
                return false;
            }
            if (this.toolStripButton_0 != null)
            {
                this.toolStripButton_0.Checked = false;
            }
            this.toolStripButton_0 = toolStripButton_1;
            this.toolStripButton_0.Checked = true;
            return true;
        }

        private void toolScale_MouseLeave(object sender, EventArgs e)
        {
            if (this.IsActive)
            {
                this.method_1();
            }
        }

        private void tsbD_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripButton)
            {
                ToolStripButton button = sender as ToolStripButton;
                if (this.method_2(button))
                {
                    this.ChartScale = (BarDataScale) button.Tag;
                    if (this.ValidScale)
                    {
                        this.method_0(new ScaleChangeEventArgs(this.ChartScale));
                    }
                }
                this.method_1();
            }
        }

        public BarDataScale ChartScale
        {
            get
            {
                return this.barDataScale_0;
            }
            set
            {
                bool flag = false;
                switch (value.Scale)
                {
                    case BarScale.Daily:
                        this.method_2(this.tsbD);
                        goto Label_00E4;

                    case BarScale.Weekly:
                        this.method_2(this.tsbW);
                        goto Label_00E4;

                    case BarScale.Monthly:
                        this.method_2(this.tsbM);
                        goto Label_00E4;

                    case BarScale.Minute:
                    {
                        int barInterval = value.BarInterval;
                        if (barInterval > 10)
                        {
                            switch (barInterval)
                            {
                                case 15:
                                    this.method_2(this.tsb15);
                                    goto Label_00E4;

                                case 30:
                                    this.method_2(this.tsb30);
                                    goto Label_00E4;

                                case 60:
                                    this.method_2(this.tsb60);
                                    goto Label_00E4;
                            }
                            break;
                        }
                        switch (barInterval)
                        {
                            case 1:
                                this.method_2(this.tsb1);
                                goto Label_00E4;

                            case 5:
                                this.method_2(this.tsb5);
                                goto Label_00E4;
                        }
                        if (barInterval != 10)
                        {
                            break;
                        }
                        this.method_2(this.tsb10);
                        goto Label_00E4;
                    }
                    default:
                        flag = true;
                        goto Label_00E4;
                }
                flag = true;
            Label_00E4:
                if (!flag)
                {
                    this.barDataScale_0 = value;
                    this.bool_0 = true;
                }
                else
                {
                    this.bool_0 = false;
                }
            }
        }

        public bool IsActive
        {
            [CompilerGenerated]
            get
            {
                return this.bool_1;
            }
            [CompilerGenerated]
            set
            {
                this.bool_1 = value;
            }
        }

        public bool ValidScale
        {
            get
            {
                return this.bool_0;
            }
        }
    }
}

