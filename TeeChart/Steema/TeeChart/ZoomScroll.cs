namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class ZoomScroll : TeeBase
    {
        private bool active;
        private Keys keyShift;
        public int x0;
        public int x1;
        public int y0;
        public int y1;

        public ZoomScroll(Chart c) : base(c)
        {
        }

        public void Activate(int x, int y)
        {
            this.x0 = x;
            this.y0 = y;
            this.x1 = x;
            this.y1 = y;
            this.Active = true;
        }

        public void Check()
        {
            int num;
            if (this.x0 > this.x1)
            {
                num = this.x0;
                this.x0 = this.x1;
                this.x1 = num;
            }
            if (this.y0 > this.y1)
            {
                num = this.y0;
                this.y0 = this.y1;
                this.y1 = num;
            }
        }

        protected virtual void SetActive(bool value)
        {
            this.active = value;
        }

        [DefaultValue(false), Description("Returns the active state of Chart Zoom and Scroll."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.SetActive(value);
            }
        }

        [Description("Sets a keyboard button as an extra condition to initiate the zoom."), DefaultValue(0)]
        public Keys KeyShift
        {
            get
            {
                return this.keyShift;
            }
            set
            {
                this.keyShift = value;
            }
        }
    }
}

