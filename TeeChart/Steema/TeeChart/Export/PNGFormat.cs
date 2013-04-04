namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Forms;

    public class PNGFormat : ImageExportFormat
    {
        private bool grayScale;

        public PNGFormat(Chart c) : base(c)
        {
            base.FileExtension = "png";
        }

        public override void CopyToClipboard()
        {
            MemoryStream stream = new MemoryStream();
            this.Save(stream);
            IDataObject data = new DataObject();
            if (base.chart.Panel.Transparent)
            {
                data.SetData(this.DataFormat, false, stream);
            }
            else
            {
                data.SetData(this.DataFormat, false, Image.FromStream(stream));
            }
            Clipboard.SetDataObject(data, false);
        }

        internal override string FilterFiles()
        {
            return Texts.PNGFilter;
        }

        internal override ImageFormat GetFormat()
        {
            return ImageFormat.Png;
        }

        internal override void GetImageOptions(ref Bitmap b)
        {
            if (this.GrayScale)
            {
                base.ConvertToGrayscale(ref b);
            }
        }

        internal static void SaveToFile(Chart c, string fileName)
        {
            new PNGFormat(c).Save(fileName);
        }

        protected override bool SupportsTransparent()
        {
            return true;
        }

        protected override string DataFormat
        {
            get
            {
                if (base.chart.Panel.Transparent)
                {
                    return "PNG";
                }
                return base.DataFormat;
            }
        }

        public bool GrayScale
        {
            get
            {
                return this.grayScale;
            }
            set
            {
                this.grayScale = value;
            }
        }
    }
}

