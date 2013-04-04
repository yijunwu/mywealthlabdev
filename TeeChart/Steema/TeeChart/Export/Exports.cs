namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors.Export;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.IO;

    [Description("Export chart picture and data."), Editor(typeof(Exports.ComponentEditor), typeof(UITypeEditor))]
    public class Exports
    {
        internal Chart chart;
        private DataExport data;
        private ImageExport image;
        private TemplateExport template;
        private ThemeExport theme;

        public Exports(Chart c)
        {
            this.chart = c;
        }

        public string GetBase64(Stream mstr)
        {
            byte[] buffer;
            string str = "";
            mstr.Position = 0L;
            try
            {
                buffer = new byte[mstr.Length];
                mstr.Read(buffer, 0, Utils.Round((float) mstr.Length));
                mstr.Flush();
                mstr.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("{0}", exception.Message);
                return null;
            }
            str = Convert.ToBase64String(buffer, 0, buffer.Length);
            buffer = null;
            return str;
        }

        public object InternalSaveViewState(Chart chart)
        {
            MemoryStream stream = new MemoryStream();
            chart.Export.Template.Save(stream);
            stream.Position = 0L;
            return stream;
        }

        public void ShowExportDialog()
        {
            ExportEditor.ShowModal(this.chart);
        }

        public void ShowExportDialog(ExportFormat expFmt)
        {
            ExportEditor.ShowModal(this.chart, expFmt);
        }

        public DataExport Data
        {
            get
            {
                if (this.data == null)
                {
                    this.data = new DataExport(this.chart);
                }
                return this.data;
            }
        }

        public ImageExport Image
        {
            get
            {
                if (this.image == null)
                {
                    this.image = new ImageExport(this.chart);
                }
                return this.image;
            }
        }

        public TemplateExport Template
        {
            get
            {
                if (this.template == null)
                {
                    this.template = new TemplateExport(this.chart);
                }
                return this.template;
            }
        }

        public ThemeExport Theme
        {
            get
            {
                if (this.theme == null)
                {
                    this.theme = new ThemeExport(this.chart);
                }
                return this.theme;
            }
        }

        internal class ComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Exports exports = (Exports) value;
                ExportEditor.ShowModal(exports.chart);
                return true;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

