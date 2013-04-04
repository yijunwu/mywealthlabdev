namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class XAMLFormat : ImageExportFormat
    {
        public XAMLFormat(Chart c) : base(c)
        {
            base.FileExtension = "xaml";
        }

        internal override string FilterFiles()
        {
            return Texts.XAMLFilter;
        }

        public override void Save(Stream stream)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            try
            {
                base.chart.Graphics3D = new Graphics3DXAML(stream, base.chart);
                base.chart.Draw(null, new Rectangle(0, 0, base.Width, base.Height));
            }
            finally
            {
                base.chart.Graphics3D = graphicsd;
            }
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new XAMLFormat(c).Save(fileName);
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

