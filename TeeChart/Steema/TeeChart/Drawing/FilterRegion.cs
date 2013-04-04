namespace Steema.TeeChart.Drawing
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class FilterRegion
    {
        private int height;
        private int left;
        private int top;
        private int width;

        public void SetRectangle(Rectangle rect)
        {
            this.left = rect.Left;
            this.top = rect.Top;
            this.width = rect.Width;
            this.height = rect.Height;
        }

        [DefaultValue(0)]
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        [DefaultValue(0)]
        public int Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }

        [DefaultValue(0)]
        public int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }

        [DefaultValue(0)]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }
}

