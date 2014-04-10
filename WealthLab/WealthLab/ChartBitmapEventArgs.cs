namespace WealthLab
{
    using System;
    using System.Drawing;

    public class ChartBitmapEventArgs : EventArgs
    {
        private System.Drawing.Bitmap bitmap;
        private int width;
        private int height;

        public ChartBitmapEventArgs(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public System.Drawing.Bitmap Bitmap
        {
            get
            {
                return this.bitmap;
            }
            set
            {
                this.bitmap = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
        }
    }
}

