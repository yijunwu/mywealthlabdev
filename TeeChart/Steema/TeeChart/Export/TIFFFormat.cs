namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class TIFFFormat : ImageExportFormat
    {
        private TIFFCompression compression;

        public TIFFFormat(Chart c) : base(c)
        {
            base.FileExtension = "tif";
        }

        private Bitmap CreateOnePixelBitmap(Bitmap img)
        {
            BitmapData bitmapdata = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, img.PixelFormat);
            Bitmap bitmap = new Bitmap(img.Width, img.Height, PixelFormat.Format1bppIndexed);
            BitmapData bmd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    int ofs = (i * bitmapdata.Stride) + (j * 4);
                    if (Color.FromArgb(Marshal.ReadByte(bitmapdata.Scan0, ofs + 2), Marshal.ReadByte(bitmapdata.Scan0, ofs + 1), Marshal.ReadByte(bitmapdata.Scan0, ofs)).GetBrightness() > 0.5f)
                    {
                        this.SetIndexedPixel(j, i, bmd, true);
                    }
                }
            }
            bitmap.UnlockBits(bmd);
            img.UnlockBits(bitmapdata);
            return bitmap;
        }

        internal override string FilterFiles()
        {
            return Texts.TIFFFilter;
        }

        internal override ImageFormat GetFormat()
        {
            return ImageFormat.Tiff;
        }

        internal override void GetImageOptions(ref Bitmap b)
        {
            if (this.Compression == TIFFCompression.RLE)
            {
                b = this.CreateOnePixelBitmap(b);
            }
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new TIFFFormat(c).Save(fileName);
        }

        private void SetIndexedPixel(int x, int y, BitmapData bmd, bool pixel)
        {
            int ofs = (y * bmd.Stride) + (x >> 3);
            byte val = Marshal.ReadByte(bmd.Scan0, ofs);
            byte num3 = (byte) (((int) 0x80) >> (x & 7));
            if (pixel)
            {
                val = (byte) (val | num3);
            }
            else
            {
                val = (byte) (val & ((byte) (num3 ^ 0xff)));
            }
            Marshal.WriteByte(bmd.Scan0, ofs, val);
        }

        protected override bool SupportsTransparent()
        {
            return true;
        }

        public TIFFCompression Compression
        {
            get
            {
                return this.compression;
            }
            set
            {
                this.compression = value;
            }
        }

        protected override string DataFormat
        {
            get
            {
                return DataFormats.Tiff;
            }
        }

        internal override EncoderParameters EncoderParams
        {
            get
            {
                EncoderValue compressionLZW;
                if (this.Compression == TIFFCompression.LZW)
                {
                    compressionLZW = EncoderValue.CompressionLZW;
                }
                else
                {
                    compressionLZW = EncoderValue.CompressionRle;
                }
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)compressionLZW);
                return parameters;
            }
        }

        public enum TIFFCompression
        {
            LZW,
            RLE
        }
    }
}

