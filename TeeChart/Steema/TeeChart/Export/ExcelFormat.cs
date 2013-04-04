namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;

    public class ExcelFormat : HTMLFormat
    {
        public ExcelFormat(Chart c) : base(c)
        {
            base.FileExtension = "xls";
        }

        internal override string FilterFiles()
        {
            return Texts.ExcelFilter;
        }
    }
}

