namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Timers;

    [ToolboxBitmap(typeof(FaderTool), "ToolsIcons.FaderTool.bmp")]
    public class FaderTool : Tool
    {
        private System.Drawing.Color color;
        private Bitmap iDest;
        private int initialDelay;
        private Timer iTimer;
        private double iTransp;
        private double speed;
        private FaderStyle style;
        private Bitmap tmpSource;

        public event EventHandler FaderStop;

        public FaderTool() : this(null)
        {
        }

        public FaderTool(Chart c) : base(c)
        {
            this.color = System.Drawing.Color.Black;
            this.style = FaderStyle.FadeIn;
            this.initialDelay = 100;
            this.speed = 3.0;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            if ((e is AfterDrawEventArgs) && (this.iDest != null))
            {
                bool enabled = this.iTimer.Enabled;
                this.iTimer.Enabled = false;
                base.Chart.Graphics3D.Draw(0, 0, this.iDest);
                this.iTimer.Enabled = enabled;
            }
        }

        private void iTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (base.Chart != null)
            {
                double iTransp;
                this.iTimer.Enabled = false;
                if (this.tmpSource == null)
                {
                    this.tmpSource = base.Chart.Bitmap();
                }
                this.iDest = new Bitmap(this.tmpSource.Width, this.tmpSource.Height);
                Graphics graphics = Graphics.FromImage(this.iDest);
                graphics.FillRectangle(new SolidBrush(this.Color), new Rectangle(0, 0, this.iDest.Width, this.iDest.Height));
                graphics.Dispose();
                if (this.Style == FaderStyle.FadeIn)
                {
                    iTransp = 100.0 - this.iTransp;
                }
                else
                {
                    iTransp = this.iTransp;
                }
                this.iDest = Steema.TeeChart.Drawing.Filter.BlendBitmaps(iTransp, this.tmpSource, this.iDest, new Point(0, 0));
                if (this.iTransp < 100.0)
                {
                    if ((this.iTransp + this.Speed) < 100.0)
                    {
                        this.iTransp += this.Speed;
                    }
                    else
                    {
                        this.iTransp = 100.0;
                    }
                    this.iTimer.Enabled = true;
                }
                else
                {
                    this.Stop();
                }
                base.Chart.Invalidate();
            }
        }

        protected virtual void OnFaderStop(EventArgs e)
        {
            if (this.FaderStop != null)
            {
                this.FaderStop(this, e);
            }
        }

        public void Reset()
        {
            this.iDest = null;
            base.Chart.Invalidate();
        }

        public void Start()
        {
            if (this.iTimer == null)
            {
                this.iTimer = new Timer((double) this.InitialDelay);
                this.iTimer.Enabled = false;
                this.iTimer.Elapsed += new ElapsedEventHandler(this.iTimer_Elapsed);
            }
            this.iTransp = 0.0;
            this.iTimer.Interval = Math.Max(1, this.InitialDelay);
            this.iTimer.Enabled = true;
        }

        public void Stop()
        {
            this.iTimer.Enabled = false;
            this.OnFaderStop(EventArgs.Empty);
        }

        [DefaultValue(typeof(System.Drawing.Color), "Color.Black")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.FaderTool;
            }
        }

        [DefaultValue(100)]
        public int InitialDelay
        {
            get
            {
                return this.initialDelay;
            }
            set
            {
                this.initialDelay = value;
            }
        }

        public double Speed
        {
            get
            {
                return this.speed;
            }
            set
            {
                this.speed = value;
            }
        }

        [DefaultValue(typeof(FaderStyle), "FadeIn")]
        public FaderStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        public override string Summary
        {
            get
            {
                return Texts.FaderToolSummary;
            }
        }
    }
}

