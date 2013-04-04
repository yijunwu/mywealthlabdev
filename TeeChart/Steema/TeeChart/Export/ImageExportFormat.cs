namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Forms;

    public class ImageExportFormat : ExportFormat
    {
        internal Chart chart;
        internal double chartWidthHeightRatio;
        private int height;
        private int width;

        public ImageExportFormat(Chart c)
        {
            this.chart = c;
            Rectangle chartBounds = this.chart.chartBounds;
            this.chartWidthHeightRatio = ((double) chartBounds.Width) / ((double) chartBounds.Height);
        }

        internal void ConvertToGrayscale(ref Bitmap b)
        {
            MakeGrayScale(b);
        }

        public virtual void CopyToClipboard()
        {
            MemoryStream stream = new MemoryStream();
            this.Save(stream);
            IDataObject data = new DataObject();
            if (this.DataFormat == DataFormats.Text)
            {
                data.SetData(this.DataFormat, true, stream);
            }
            else
            {
                data.SetData(this.DataFormat, false, Image.FromStream(stream));
            }
            Clipboard.SetDataObject(data, false);
        }

        internal ColorPalette GetColorPalette(uint nColors)
        {
            PixelFormat format = PixelFormat.Format1bppIndexed;
            if (nColors > 2)
            {
                format = PixelFormat.Format4bppIndexed;
            }
            if (nColors > 0x10)
            {
                format = PixelFormat.Format8bppIndexed;
            }
            Bitmap bitmap = new Bitmap(1, 1, format);
            ColorPalette palette = bitmap.Palette;
            bitmap.Dispose();
            return palette;
        }

        internal ImageCodecInfo GetEncoderInfo(Guid g)
        {
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < imageEncoders.Length; i++)
            {
                if (imageEncoders[i].FormatID == g)
                {
                    return imageEncoders[i];
                }
            }
            return null;
        }

        internal virtual ImageFormat GetFormat()
        {
            return ImageFormat.Bmp;
        }

        internal virtual void GetImageOptions(ref Bitmap b)
        {
        }

        internal static bool MakeGrayScale(Bitmap b)
        {
            for (int i = 0; i < (b.Height - 1); i++)
            {
                for (int j = 0; j < (b.Width - 1); j++)
                {
                    Color pixel = b.GetPixel(j, i);
                    int red = (Utils.Round((double) (0.299 * pixel.R)) + Utils.Round((double) (0.587 * pixel.G))) + Utils.Round((double) (0.114 * pixel.B));
                    pixel = Color.FromArgb(red, red, red);
                    b.SetPixel(j, i, pixel);
                }
            }
            return true;
        }

        public virtual void Save(Stream stream)
        {
            if (this.Width <= 0)
            {
                this.Width = 400;
            }
            if (this.Height <= 0)
            {
                this.Height = 300;
            }
            bool flag = this.chart.Panel.Transparent && !this.SupportsTransparent();
            if (flag)
            {
                this.chart.Panel.Transparent = false;
            }
            Bitmap b = this.chart.Bitmap(this.Width, this.Height);
            this.Save(stream, b, this.Width, this.Height);
            if (flag)
            {
                this.chart.Panel.Transparent = true;
            }
        }

        public virtual void Save(string FileName)
        {
            using (FileStream stream = new FileStream(FileName, FileMode.Create))
            {
                this.Save(stream);
                stream.Flush();
                stream.Close();
            }
        }

        public virtual void Save(Stream stream, Bitmap b, int width, int height)
        {
            if (this.Width <= 0)
            {
                this.Width = 400;
            }
            else
            {
                this.Width = width;
            }
            if (this.Height <= 0)
            {
                this.Height = 300;
            }
            else
            {
                this.Height = height;
            }
            this.GetImageOptions(ref b);
            b.Save(stream, this.Encoder, this.EncoderParams);
            stream.Flush();
            b.Dispose();
        }

        protected virtual bool SupportsTransparent()
        {
            return false;
        }

        protected virtual string DataFormat
        {
            get
            {
                return DataFormats.Bitmap;
            }
        }

        internal ImageCodecInfo Encoder
        {
            get
            {
                return this.GetEncoderInfo(this.Format.Guid);
            }
        }

        internal virtual EncoderParameters EncoderParams
        {
            get
            {
                return null;
            }
        }

        protected ImageFormat Format
        {
            get
            {
                return this.GetFormat();
            }
        }

        public int Height
        {
            get
            {
                if (this.height == 0)
                {
                    return this.chart.ChartBounds.Height;
                }
                return this.height;
            }
            set
            {
                if (this.height != value)
                {
                    this.height = value;
                    this.chart.Invalidate();
                }
            }
        }

        public int Width
        {
            get
            {
                if (this.width == 0)
                {
                    return this.chart.ChartBounds.Width;
                }
                return this.width;
            }
            set
            {
                if (this.width != value)
                {
                    this.width = value;
                    this.chart.Invalidate();
                }
            }
        }
    }
}

