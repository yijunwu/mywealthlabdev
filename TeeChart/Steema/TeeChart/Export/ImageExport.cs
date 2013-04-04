namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;

    public sealed class ImageExport
    {
        private BitmapFormat bitmapFormat;
        internal Chart chart;
        private EPSFormat epsFormat;
        private FlexFormat flexFormat;
        private GIFFormat gifFormat;
        private JPEGFormat jpegFormat;
        private MetafileFormat metafileFormat;
        private PDFFormat pdfFormat;
        private PNGFormat pngFormat;
        private SVGFormat svgFormat;
        private TIFFFormat tifFormat;
        private VMLFormat vmlFormat;
        private XAMLFormat xamlFormat;

        public ImageExport(Chart c)
        {
            this.chart = c;
        }

        internal ImageExportFormat FromFormat(PictureFormats format)
        {
            switch (format)
            {
                case PictureFormats.Metafile:
                    return this.Metafile;

                case PictureFormats.JPEG:
                    return this.JPEG;

                case PictureFormats.PNG:
                    return this.PNG;

                case PictureFormats.GIF:
                    return this.GIF;

                case PictureFormats.TIFF:
                    return this.TIFF;

                case PictureFormats.PDF:
                    return this.PDF;

                case PictureFormats.VML:
                    return this.VML;

                case PictureFormats.SVG:
                    return this.SVG;

                case PictureFormats.EPS:
                    return this.EPS;

                case PictureFormats.XAML:
                    return this.XAML;
            }
            return this.Bitmap;
        }

        public BitmapFormat Bitmap
        {
            get
            {
                if (this.bitmapFormat == null)
                {
                    this.bitmapFormat = new BitmapFormat(this.chart);
                }
                return this.bitmapFormat;
            }
        }

        public EPSFormat EPS
        {
            get
            {
                if (this.epsFormat == null)
                {
                    this.epsFormat = new EPSFormat(this.chart);
                }
                return this.epsFormat;
            }
        }

        public FlexFormat Flex
        {
            get
            {
                if (this.flexFormat == null)
                {
                    this.flexFormat = new FlexFormat(this.chart);
                }
                return this.flexFormat;
            }
        }

        public GIFFormat GIF
        {
            get
            {
                if (this.gifFormat == null)
                {
                    this.gifFormat = new GIFFormat(this.chart);
                }
                return this.gifFormat;
            }
        }

        public JPEGFormat JPEG
        {
            get
            {
                if (this.jpegFormat == null)
                {
                    this.jpegFormat = new JPEGFormat(this.chart);
                }
                return this.jpegFormat;
            }
        }

        public MetafileFormat Metafile
        {
            get
            {
                if (this.metafileFormat == null)
                {
                    this.metafileFormat = new MetafileFormat(this.chart);
                }
                return this.metafileFormat;
            }
        }

        public PDFFormat PDF
        {
            get
            {
                if (this.pdfFormat == null)
                {
                    this.pdfFormat = new PDFFormat(this.chart);
                }
                return this.pdfFormat;
            }
        }

        public PNGFormat PNG
        {
            get
            {
                if (this.pngFormat == null)
                {
                    this.pngFormat = new PNGFormat(this.chart);
                }
                return this.pngFormat;
            }
        }

        public SVGFormat SVG
        {
            get
            {
                if (this.svgFormat == null)
                {
                    this.svgFormat = new SVGFormat(this.chart);
                }
                return this.svgFormat;
            }
        }

        public TIFFFormat TIFF
        {
            get
            {
                if (this.tifFormat == null)
                {
                    this.tifFormat = new TIFFFormat(this.chart);
                }
                return this.tifFormat;
            }
        }

        public VMLFormat VML
        {
            get
            {
                if (this.vmlFormat == null)
                {
                    this.vmlFormat = new VMLFormat(this.chart);
                }
                return this.vmlFormat;
            }
        }

        public XAMLFormat XAML
        {
            get
            {
                if (this.xamlFormat == null)
                {
                    this.xamlFormat = new XAMLFormat(this.chart);
                }
                return this.xamlFormat;
            }
        }
    }
}

