namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Export;
    using System;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    public class MetafileEditor : ExportEditors
    {
        public override Form Options(Chart c)
        {
            if (base.fOptions == null)
            {
                base.fOptions = new EmfOptions();
            }
            return base.fOptions;
        }

        protected override void UpdateOptions(ImageExportFormat format)
        {
            MetafileFormat format2 = (MetafileFormat) format;
            EmfOptions fOptions = (EmfOptions) base.fOptions;
            switch (fOptions.EMFType)
            {
                case 0:
                    format2.EMFFormat = EmfType.EmfOnly;
                    break;

                case 1:
                    format2.EMFFormat = EmfType.EmfPlusDual;
                    break;

                case 2:
                    format2.EMFFormat = EmfType.EmfPlusOnly;
                    break;
            }
            format2.UpdateFileExtension();
        }
    }
}

