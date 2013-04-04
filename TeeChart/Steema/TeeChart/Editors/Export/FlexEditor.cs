namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class FlexEditor : ExportEditors
    {
        public override Form Options(Chart c)
        {
            if (base.fOptions == null)
            {
                base.fOptions = new FlexOptions(c);
            }
            return base.fOptions;
        }

        protected override void UpdateOptions(ImageExportFormat format)
        {
            FlexFormat format1 = (FlexFormat) format;
            FlexOptions fOptions = (FlexOptions) base.fOptions;
        }
    }
}

