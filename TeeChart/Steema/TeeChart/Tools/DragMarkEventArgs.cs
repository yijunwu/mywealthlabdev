namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class DragMarkEventArgs
    {
        private MouseButtons button;
        private int clicks;
        private int delta;
        private int index;
        private Point location;
        private int x;
        private int y;

        public DragMarkEventArgs(int index, MouseEventArgs e)
        {
            this.button = Utils.GetMouseButton(e);
            this.clicks = e.Clicks;
            this.delta = e.Delta;
            this.location = e.Location;
            this.x = e.X;
            this.y = e.Y;
            this.index = index;
        }

        public MouseButtons Button
        {
            get
            {
                return this.button;
            }
            set
            {
                this.button = value;
            }
        }

        public int Clicks
        {
            get
            {
                return this.clicks;
            }
            set
            {
                this.clicks = value;
            }
        }

        public int Delta
        {
            get
            {
                return this.delta;
            }
            set
            {
                this.delta = value;
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        public Point Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        public int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }
    }
}

