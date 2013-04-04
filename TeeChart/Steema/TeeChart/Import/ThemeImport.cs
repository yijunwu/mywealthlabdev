namespace Steema.TeeChart.Import
{
    using Steema.TeeChart;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Themes;
    using System;
    using System.IO;
    using System.Net;
    using System.Windows.Forms;

    public sealed class ThemeImport : Imports
    {
        internal string fileExtension;

        public ThemeImport()
        {
            this.fileExtension = "";
            this.FileExtension = Texts.XMLFilter;
        }

        public ThemeImport(Chart c) : this()
        {
            base.chart = c;
        }

        public Chart FromURL(string url)
        {
            using (WebClient client = new WebClient())
            {
                return this.Load(new MemoryStream(client.DownloadData(url)));
            }
        }

        public Chart Load(Stream stream)
        {
            Theme.ApplyChartTheme(stream, base.chart);
            if ((ThemeProperties.Instance.Base64 != "") && (ThemeProperties.Instance.Base64 != null))
            {
                MemoryStream savedState = base.chart.Import.DecodeBase64(ThemeProperties.Instance.Base64);
                base.chart.Import.InternalLoadViewState(savedState, ref this.chart);
            }
            return base.chart;
        }

        public Chart Load(string fileName)
        {
            Chart chart;
            Stream stream = System.IO.File.OpenRead(fileName);
            try
            {
                chart = this.Load(stream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return chart;
        }

        public void LoadFileDialog()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = Texts.OpenXmlThemeFile;
                dialog.Filter = ThemeExport.FileFilter();
                if ((dialog.ShowDialog() == DialogResult.OK) && (dialog.FileName.Length != 0))
                {
                    System.IO.File.OpenRead(dialog.FileName);
                    this.Load(dialog.FileName);
                }
            }
        }

        public string FileExtension
        {
            get
            {
                return this.fileExtension;
            }
            set
            {
                this.fileExtension = value;
            }
        }
    }
}

