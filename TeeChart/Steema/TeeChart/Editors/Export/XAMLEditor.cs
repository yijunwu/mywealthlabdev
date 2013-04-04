namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;

    public class XAMLEditor : ExportEditors
    {
        protected override void UpdateOptions(ImageExportFormat format)
        {
            XAMLFormat format1 = (XAMLFormat) format;
        }
    }
}

