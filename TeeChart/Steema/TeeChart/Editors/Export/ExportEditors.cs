namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class ExportEditors
    {
        protected Form fOptions;

        public virtual Form Options()
        {
            return this.Options(null);
        }

        public virtual Form Options(Chart c)
        {
            return null;
        }

        public void SetOptions(ImageExportFormat format, int width, int height)
        {
            format.Width = width;
            format.Height = height;
            this.UpdateOptions(format);
        }

        protected virtual void UpdateOptions(ImageExportFormat format)
        {
        }
    }
}

