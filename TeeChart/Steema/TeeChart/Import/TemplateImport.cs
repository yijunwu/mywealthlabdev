namespace Steema.TeeChart.Import
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Export;
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public sealed class TemplateImport : Imports
    {
        private string customType;
        internal string fileExtension;

        public TemplateImport()
        {
            this.fileExtension = "";
            this.customType = "";
            this.FileExtension = Texts.TeeFilesExtension;
        }

        public TemplateImport(Chart c)
        {
            this.fileExtension = "";
            this.customType = "";
            base.chart = c;
            this.FileExtension = Texts.TeeFilesExtension;
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
            IChart parent = base.chart.parent;
            SerializeBinder binder = new SerializeBinder();
            binder.BindToType("TeeChart", (this.customType == "") ? "Chart" : this.customType);
            BinaryFormatter formatter = new BinaryFormatter {
                AssemblyFormat = FormatterAssemblyStyle.Simple,
                Binder = binder
            };
            base.chart.RemoveAllComponents();
            base.chart = (Chart) formatter.Deserialize(stream);
            if (parent != null)
            {
                base.chart.parent = parent;
                base.chart.parent.SetChart(base.chart);
                base.chart.parent.DoSetControlStyle();
            }
            base.chart.BroadcastEvent(new View3DEvent());
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
                dialog.Title = Texts.OpenTeeChartFile;
                dialog.Filter = TemplateExport.FileFilter();
                if ((dialog.ShowDialog() == DialogResult.OK) && (dialog.FileName.Length != 0))
                {
                    System.IO.File.OpenRead(dialog.FileName);
                    this.Load(dialog.FileName);
                }
            }
        }

        public string CustomType
        {
            get
            {
                return this.customType;
            }
            set
            {
                this.customType = value;
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

