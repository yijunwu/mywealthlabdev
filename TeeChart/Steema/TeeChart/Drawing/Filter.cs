namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    public class Filter
    {
        protected bool AllowRegion = true;
        private const int BytesPerPixel = 4;
        private bool enabled = true;
        private static Steema.TeeChart.Drawing.Filter instance;
        public RGB[,] Lines;
        private FilterRegion region;

        public void Apply(ref Bitmap bitmap)
        {
            if (this.region == null)
            {
                this.Apply(ref bitmap, Utils.FromLTRB(0, 0, bitmap.Width, bitmap.Height));
            }
            else
            {
                int num3;
                int num4;
                int left = this.region.Left;
                int top = this.region.Top;
                if (this.region.Width == 0)
                {
                    num3 = Math.Max(left, bitmap.Width);
                }
                else
                {
                    num3 = Math.Min(bitmap.Width, left + this.region.Width);
                }
                if (this.region.Height == 0)
                {
                    num4 = Math.Max(top, bitmap.Height);
                }
                else
                {
                    num4 = Math.Min(bitmap.Height, top + this.region.Height);
                }
                this.Apply(ref bitmap, Utils.FromLTRB(left, top, num3, num4));
            }
        }

        public virtual void Apply(ref Bitmap bitmap, Rectangle rect)
        {
            this.CalcLines(bitmap);
        }

        public static void ApplyTo(ref Bitmap bitmap)
        {
            GetFilter().Apply(ref bitmap);
        }

        public static Bitmap BlendBitmaps(double percent, Bitmap source, Bitmap destination, Point origin)
        {
            int srcWidth = Math.Min(source.Width, destination.Width - origin.X);
            int srcHeight = Math.Min(source.Height, destination.Height - origin.Y);
            float num3 = ((float) percent) / 100f;
            float[][] numArray2 = new float[5][];
            float[] numArray3 = new float[5];
            numArray3[0] = 1f;
            numArray2[0] = numArray3;
            float[] numArray4 = new float[5];
            numArray4[1] = 1f;
            numArray2[1] = numArray4;
            float[] numArray5 = new float[5];
            numArray5[2] = 1f;
            numArray2[2] = numArray5;
            float[] numArray6 = new float[5];
            numArray6[3] = num3;
            numArray2[3] = numArray6;
            float[] numArray7 = new float[5];
            numArray7[4] = 1f;
            numArray2[4] = numArray7;
            float[][] newColorMatrix = numArray2;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Rectangle destRect = new Rectangle(0, 0, destination.Width, destination.Height);
            Graphics.FromImage(destination).DrawImage(source, destRect, origin.X, origin.Y, srcWidth, srcHeight, GraphicsUnit.Pixel, imageAttr);
            return destination;
        }

        public void CalcLines(Bitmap bitmap)
        {
            Graphics3D.CalcLines(out this.Lines, bitmap);
        }

        public static void ColorToHLS(Color c, out float Hue, out float Luminance, out float Saturation)
        {
            Hue = c.GetHue();
            Luminance = c.GetBrightness();
            Saturation = c.GetSaturation();
        }

        private static Steema.TeeChart.Drawing.Filter GetFilter()
        {
            if (instance == null)
            {
                instance = new Steema.TeeChart.Drawing.Filter();
            }
            return instance;
        }

        public static Color HLSToColor(float H, float L, float S)
        {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = ((double) Math.Abs(H)) % 360.0;
            double num5 = Math.Max(Math.Min(1.0, (double) L), 0.0);
            double num6 = Math.Max(Math.Min(1.0, (double) S), 0.0);
            double num9 = num4 / 360.0;
            if (num5 == 0.0)
            {
                num = num2 = num3 = 0.0;
            }
            else if (num6 == 0.0)
            {
                num = num2 = num3 = num5;
            }
            else
            {
                double num8 = (num5 <= 0.5) ? (num5 * (1.0 + num6)) : ((num5 + num6) - (num5 * num6));
                double num7 = (2.0 * num5) - num8;
                double[] numArray = new double[] { num9 + 0.33333333333333331, num9, num9 - 0.33333333333333331 };
                double[] numArray2 = new double[3];
                for (int i = 0; i < 3; i++)
                {
                    if (numArray[i] < 0.0)
                    {
                        numArray[i]++;
                    }
                    if (numArray[i] > 1.0)
                    {
                        numArray[i]--;
                    }
                    if ((6.0 * numArray[i]) < 1.0)
                    {
                        numArray2[i] = num7 + (((num8 - num7) * numArray[i]) * 6.0);
                    }
                    else if ((2.0 * numArray[i]) < 1.0)
                    {
                        numArray2[i] = num8;
                    }
                    else if ((3.0 * numArray[i]) < 2.0)
                    {
                        numArray2[i] = num7 + (((num8 - num7) * (0.66666666666666663 - numArray[i])) * 6.0);
                    }
                    else
                    {
                        numArray2[i] = num7;
                    }
                }
                num = numArray2[0];
                num2 = numArray2[1];
                num3 = numArray2[2];
            }
            return Utils.FromArgb((int) (255.0 * num), (int) (255.0 * num2), (int) (255.0 * num3));
        }

        public static Bitmap SmoothBitmap(Bitmap bitmap, int width, int height)
        {
            Bitmap dst = new Bitmap(width, height);
            SmoothStretch(bitmap, ref dst);
            return dst;
        }

        public static void SmoothStretch(Bitmap src, ref Bitmap dst)
        {
            SmoothStretch(src, ref dst, SmoothStretchOption.BestQuality);
        }

        public static void SmoothStretch(Bitmap src, ref Bitmap dst, SmoothStretchOption option)
        {
            if (src != null)
            {
                Graphics graphics = Graphics.FromImage(dst);
                Rectangle rect = Utils.FromLTRB(0, 0, dst.Width, dst.Height);
                if (option == SmoothStretchOption.BestQuality)
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                }
                else
                {
                    graphics.InterpolationMode = InterpolationMode.Bicubic;
                }
                graphics.DrawImage(src, rect);
            }
        }

        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }

        public FilterRegion Region
        {
            get
            {
                if (this.region == null)
                {
                    this.region = new FilterRegion();
                }
                return this.region;
            }
            set
            {
                this.region = value;
            }
        }
    }
}

