namespace Steema.TeeChart.Drawing
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class CropFilter : ResizeFilter
    {
        private int left;
        private bool smooth = false;
        private int top;

        public override void Apply(ref Bitmap bitmap, Rectangle rect)
        {
            if ((base.Width > 0) && (base.Height > 0))
            {
                Rectangle destRect = new Rectangle(0, 0, base.Width, base.Height);
                Bitmap image = new Bitmap(destRect.Width, destRect.Height);
                Graphics.FromImage(image).DrawImage(bitmap, destRect, Rectangle.FromLTRB(this.Left, this.Top, this.Left + base.Width, this.Top + base.Height), GraphicsUnit.Pixel);
                bitmap = image;
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

        [DefaultValue(false)]
        public bool Smooth
        {
            get
            {
                return this.smooth;
            }
            set
            {
                this.smooth = value;
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
    }
}

