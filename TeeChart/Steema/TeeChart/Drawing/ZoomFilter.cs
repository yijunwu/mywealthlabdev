namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class ZoomFilter : Steema.TeeChart.Drawing.Filter
    {
        private double percent = 10.0;
        private bool smooth = false;

        public override void Apply(ref Bitmap bitmap, Rectangle rect)
        {
            int width = rect.Width;
            int height = rect.Height;
            int wp = Utils.Round((double) ((this.percent * width) * 0.005));
            int hp = Utils.Round((double) ((this.percent * height) * 0.005));
            Bitmap dst = new Bitmap(width, height);
            this.DoCrop(rect, wp, hp, ref bitmap);
            if (this.Smooth)
            {
                Steema.TeeChart.Drawing.Filter.SmoothStretch(bitmap, ref dst, SmoothStretchOption.BestQuality);
            }
            else
            {
                Steema.TeeChart.Drawing.Filter.SmoothStretch(bitmap, ref dst, SmoothStretchOption.BestPerformance);
            }
            bitmap = dst;
        }

        private void DoCrop(Rectangle rect, int wp, int hp, ref Bitmap bitmap)
        {
            new CropFilter { Left = rect.Left + wp, Top = rect.Top + hp, Width = Math.Max(1, rect.Width - (2 * wp)), Height = Math.Max(1, rect.Height - (2 * hp)), Smooth = this.smooth }.Apply(ref bitmap, rect);
        }

        [DefaultValue(10)]
        public double Percent
        {
            get
            {
                return this.percent;
            }
            set
            {
                this.percent = value;
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
    }
}

