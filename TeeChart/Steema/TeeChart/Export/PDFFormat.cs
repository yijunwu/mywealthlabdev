namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class PDFFormat : ImageExportFormat
    {
        public PDFFormat(Chart c) : base(c)
        {
            base.FileExtension = "pdf";
        }

        internal override string FilterFiles()
        {
            return Texts.PDFFilter;
        }

        public override void Save(Stream stream)
        {
            PDFPage page = new PDFPage(stream, base.Width, base.Height);
            Graphics3D graphicsd = base.chart.Graphics3D;
            try
            {
                base.chart.Graphics3D = new Graphics3DPDF(page.pdfData, base.chart);
                base.chart.Draw(null, new Rectangle(0, 0, base.Width, base.Height));
                page.ConstructPage();
            }
            finally
            {
                base.chart.Graphics3D = graphicsd;
            }
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new PDFFormat(c).Save(fileName);
        }

        protected override string DataFormat
        {
            get
            {
                return DataFormats.Text;
            }
        }
    }
}

