namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;

    public class SVGEditor : ExportEditors
    {
        protected override void UpdateOptions(ImageExportFormat format)
        {
            SVGFormat format1 = (SVGFormat) format;
        }
    }
}

