namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class VMLEditor : ExportEditors
    {
        public override Form Options()
        {
            return base.fOptions;
        }

        protected override void UpdateOptions(ImageExportFormat format)
        {
            VMLFormat format1 = (VMLFormat) format;
        }
    }
}

