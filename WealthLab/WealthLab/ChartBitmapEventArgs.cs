namespace WealthLab
{
    using System;
    using System.Drawing;

    public class ChartBitmapEventArgs : EventArgs
    {
        private System.Drawing.Bitmap bitmap_0;
        private int int_0;
        private int int_1;

        public ChartBitmapEventArgs(int width, int height)
        {
            this.int_0 = width;
            this.int_1 = height;
        }

        public System.Drawing.Bitmap Bitmap
        {
            get
            {
                return this.bitmap_0;
            }
            set
            {
                this.bitmap_0 = value;
            }
        }

        public int Height
        {
            get
            {
                return this.int_1;
            }
        }

        public int Width
        {
            get
            {
                return this.int_0;
            }
        }
    }
}

