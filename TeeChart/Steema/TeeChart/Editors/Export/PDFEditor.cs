namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;

    public class PDFEditor : ExportEditors
    {
        protected override void UpdateOptions(ImageExportFormat format)
        {
            PDFFormat format1 = (PDFFormat) format;
        }
    }
}

