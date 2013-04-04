namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class FlexFormat : ImageExportFormat
    {
        private bool embeddedImages;
        private string imagePath;

        public FlexFormat(Chart c) : base(c)
        {
            base.FileExtension = "mxml";
            this.embeddedImages = true;
            this.imagePath = "";
        }

        internal override string FilterFiles()
        {
            return Texts.FlexFilter;
        }

        public override void Save(Stream stream)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            try
            {
                Graphics3DFlex flex = new Graphics3DFlex(stream, base.chart) {
                    ImagePath = this.ImagePath,
                    EmbeddedImages = this.EmbeddedImages
                };
                base.chart.Graphics3D = flex;
                base.chart.Draw(null, new Rectangle(0, 0, base.Width, base.Height));
            }
            finally
            {
                base.chart.Graphics3D = graphicsd;
            }
        }

        public override void Save(string FileName)
        {
            this.ImagePath = Path.GetDirectoryName(FileName);
            base.Save(FileName);
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new FlexFormat(c).Save(fileName);
        }

        protected override string DataFormat
        {
            get
            {
                return DataFormats.Text;
            }
        }

        public bool EmbeddedImages
        {
            get
            {
                return this.embeddedImages;
            }
            set
            {
                this.embeddedImages = value;
            }
        }

        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }
            set
            {
                this.imagePath = value;
            }
        }
    }
}

