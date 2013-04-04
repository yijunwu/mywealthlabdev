namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public class GraphicsImage
    {
        private GraphicsImages images;
        public Stream ImageStream = new MemoryStream();
        private int m_height;
        private System.Drawing.Imaging.ImageFormat m_imgformat;
        private PixelFormat m_imgpxformat;
        private int m_index;
        private int m_objectnumber;
        private int m_width;

        public GraphicsImage(GraphicsImages images, Image image)
        {
            this.images = images;
            this.m_index = images.Count;
            this.m_width = image.Width;
            this.m_height = image.Height;
            this.m_imgformat = image.RawFormat;
            this.m_imgpxformat = image.PixelFormat;
            image.Save(this.ImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public string DictImageName()
        {
            return ("Im" + this.Index.ToString());
        }

        public System.Drawing.Imaging.ImageFormat ImageFormat
        {
            get
            {
                return this.m_imgformat;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.m_height;
            }
        }

        public PixelFormat ImagePixelFormat
        {
            get
            {
                return this.m_imgpxformat;
            }
        }

        public int ImageWidth
        {
            get
            {
                return this.m_width;
            }
        }

        public int Index
        {
            get
            {
                return this.m_index;
            }
        }

        public int ObjectNumber
        {
            get
            {
                return this.m_objectnumber;
            }
            set
            {
                this.m_objectnumber = value;
            }
        }
    }
}

