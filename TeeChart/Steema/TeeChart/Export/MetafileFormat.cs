namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class MetafileFormat : ImageExportFormat
    {
        public bool bEnhanced;
        private EmfType emfFormat;

        public MetafileFormat(Chart c) : base(c)
        {
            this.bEnhanced = true;
            this.emfFormat = EmfType.EmfOnly;
            base.FileExtension = "emf";
        }

        [DllImport("user32.dll")]
        private static extern bool CloseClipboard();
        public override void CopyToClipboard()
        {
            MemoryStream fileOrStream = new MemoryStream();
            Metafile metafile = this.SaveMetafile(fileOrStream);
            uint uFormat = 14;
            if ((OpenClipboard(GetOpenClipboardWindow()) && EmptyClipboard()) && (SetClipboardData(uFormat, metafile.GetHenhmetafile()) != IntPtr.Zero))
            {
                CloseClipboard();
            }
        }

        [DllImport("user32.dll")]
        private static extern bool EmptyClipboard();
        internal override string FilterFiles()
        {
            return Texts.EMFFilter;
        }

        internal override ImageFormat GetFormat()
        {
            return ImageFormat.Emf;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetOpenClipboardWindow();
        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);
        public override void Save(Stream stream)
        {
            this.SaveMetafile(stream);
        }

        public override void Save(string FileName)
        {
            this.SaveMetafile(FileName);
        }

        private Metafile SaveMetafile(object FileOrStream)
        {
            if (base.Width <= 0)
            {
                base.Width = 400;
            }
            if (base.Height <= 0)
            {
                base.Height = 300;
            }
            Metafile metafile = null;
            if (FileOrStream is string)
            {
                FileStream stream = File.Create(FileOrStream as string);
                metafile = base.chart.Metafile(stream, base.chart, base.Width, base.Height, this.EMFFormat);
                stream.Flush();
                stream.Close();
                return metafile;
            }
            if (FileOrStream is Stream)
            {
                MemoryStream stream2 = new MemoryStream();
                metafile = base.chart.Metafile(stream2, base.chart, base.Width, base.Height, this.EMFFormat);
                stream2.WriteTo(FileOrStream as Stream);
            }
            return metafile;
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new MetafileFormat(c).Save(fileName);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
        protected override bool SupportsTransparent()
        {
            return true;
        }

        internal void UpdateFileExtension()
        {
            base.FileExtension = "emf";
        }

        protected override string DataFormat
        {
            get
            {
                return DataFormats.EnhancedMetafile;
            }
        }

        public EmfType EMFFormat
        {
            get
            {
                return this.emfFormat;
            }
            set
            {
                if (value != this.emfFormat)
                {
                    this.emfFormat = value;
                }
            }
        }

        [Obsolete("Please use EMFFormat property.")]
        public bool Enhanced
        {
            get
            {
                return this.bEnhanced;
            }
            set
            {
                this.bEnhanced = value;
            }
        }
    }
}

