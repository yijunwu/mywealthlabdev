namespace Steema.TeeChart.Drawing
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class ResizeFilter : Steema.TeeChart.Drawing.Filter
    {
        private int height;
        private int width;

        public override void Apply(ref Bitmap bitmap, Rectangle rect)
        {
            if ((this.Width > 0) && (this.Height > 0))
            {
                bitmap = Steema.TeeChart.Drawing.Filter.SmoothBitmap(bitmap, this.Width, this.Height);
            }
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

