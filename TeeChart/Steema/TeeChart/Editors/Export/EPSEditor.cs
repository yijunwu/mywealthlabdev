namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;

    public class EPSEditor : ExportEditors
    {
        protected override void UpdateOptions(ImageExportFormat format)
        {
            EPSFormat format1 = (EPSFormat) format;
        }
    }
}

