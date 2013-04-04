namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [Description("Banner"), ToolboxBitmap(typeof(BannerTool), "ToolsIcons.BannerTool.bmp")]
    public class BannerTool : Annotation
    {
        private int blinkdelayoff;
        private int blinkdelayon;
        private Timer blinker;
        private ScrollingDirection direction;
        private bool idraw;
        private int ixpos;
        private Timer scroll;
        private int scrolldelay;

        public BannerTool() : this(null)
        {
        }

        public BannerTool(Chart c) : base(c)
        {
            this.idraw = true;
            this.blinkdelayoff = 0x3e8;
            this.blinkdelayon = 0x3e8;
            this.scrolldelay = 50;
            this.blinker = new Timer();
            this.blinker.Interval = this.blinkdelayon;
            this.blinker.Tick += new EventHandler(this.DoBlink);
            this.blinker.Enabled = false;
            this.scroll = new Timer();
            this.scroll.Interval = this.scrolldelay;
            this.scroll.Tick += new EventHandler(this.DoScroll);
            this.scroll.Enabled = false;
        }

        protected override void CalcTextXY(ref int x, ref int y)
        {
            if (this.scroll.Enabled)
            {
                if (this.direction == ScrollingDirection.sdRightLeft)
                {
                    x = (x + base.Width) + this.ixpos;
                }
                else
                {
                    x -= base.Width - this.ixpos;
                }
            }
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            if ((base.Chart != null) && this.idraw)
            {
                base.ChartEvent(e);
            }
        }

        private void DoBlink(object sender, EventArgs e)
        {
            this.blinker.Enabled = false;
            try
            {
                this.idraw = !this.idraw;
                this.blinker.Interval = this.idraw ? this.blinkdelayon : this.blinkdelayoff;
                this.Invalidate();
            }
            finally
            {
                this.blinker.Enabled = true;
            }
        }

        private void DoScroll(object sender, EventArgs e)
        {
            this.scroll.Enabled = false;
            try
            {
                if (this.direction == ScrollingDirection.sdRightLeft)
                {
                    this.ixpos -= 5;
                    if (this.ixpos < (-2 * base.Width))
                    {
                        this.ixpos = 0;
                    }
                }
                else
                {
                    this.ixpos += 5;
                    if (this.ixpos > (2 * base.Width))
                    {
                        this.ixpos = 0;
                    }
                }
                this.Invalidate();
            }
            finally
            {
                this.scroll.Enabled = true;
            }
        }

        [DefaultValue(false)]
        public bool Blink
        {
            get
            {
                return this.blinker.Enabled;
            }
            set
            {
                this.blinker.Enabled = value;
                this.Invalidate();
            }
        }

        [DefaultValue(0x3e8)]
        public int BlinkDelayOff
        {
            get
            {
                return this.blinkdelayoff;
            }
            set
            {
                base.SetIntegerProperty(ref this.blinkdelayoff, value);
            }
        }

        [DefaultValue(0x3e8)]
        public int BlinkDelayOn
        {
            get
            {
                return this.blinkdelayon;
            }
            set
            {
                base.SetIntegerProperty(ref this.blinkdelayon, value);
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.BannerTool;
            }
        }

        [DefaultValue(false)]
        public bool Scroll
        {
            get
            {
                return this.scroll.Enabled;
            }
            set
            {
                this.scroll.Enabled = value;
                this.Invalidate();
            }
        }

        [DefaultValue(0)]
        public ScrollingDirection ScrollDirection
        {
            get
            {
                return this.direction;
            }
            set
            {
                if (value != this.direction)
                {
                    this.direction = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.BannerToolSummary;
            }
        }
    }
}

