namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart.Drawing;
    using System;

    public class Spacing : TeeBase
    {
        private int horizontal;
        private Series iseries;
        private int vertical;

        public Spacing()
        {
            this.horizontal = 8;
            this.vertical = 0x10;
        }

        public Spacing(Series s)
        {
            this.horizontal = 8;
            this.vertical = 0x10;
            this.iseries = s;
        }

        public int Horizontal
        {
            get
            {
                return this.horizontal;
            }
            set
            {
                this.horizontal = value;
                if (this.iseries != null)
                {
                    this.iseries.Invalidate();
                }
            }
        }

        public int Vertical
        {
            get
            {
                return this.vertical;
            }
            set
            {
                this.vertical = value;
                if (this.iseries != null)
                {
                    this.iseries.Invalidate();
                }
            }
        }
    }
}

