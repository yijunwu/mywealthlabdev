namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Text;

    public class XMLFormat : DataExportFormat
    {
        private bool includeColors;

        public XMLFormat(Chart c) : base(c)
        {
            base.FileExtension = "xml";
        }

        internal override string FilterFiles()
        {
            return Texts.XMLFilter;
        }

        private string Get(ValueList aList, int index)
        {
            string[] strArray = new string[] { " ", aList.Name, "=\"", aList[index].ToString(), "\"" };
            return string.Concat(strArray);
        }

        internal override string GetContent()
        {
            StringBuilder builder = new StringBuilder(1);
            base.Prepare();
            if (base.Series != null)
            {
                builder.Append(this.XMLSeries(base.Series));
            }
            else
            {
                builder.Append("<chart>" + base.TextLineSeparator);
                foreach (Series series in base.Chart.Series)
                {
                    builder.Append(this.XMLSeries(series));
                }
                builder.Append("</chart>");
            }
            return builder.ToString();
        }

        private string GetPointString(int index, Series aSeries)
        {
            StringBuilder builder = new StringBuilder(1);
            builder.Append(base.IncludeIndex ? (" index=\"" + index.ToString() + "\"") : "");
            if (base.hasLabels)
            {
                builder.Append(" text=\"" + aSeries.Labels[index] + "\"");
            }
            if (this.includeColors || aSeries.IsNull(index))
            {
                builder.Append(" color=\"" + Utils.ColorToHex(aSeries.Colors[index]) + "\"");
            }
            if (base.hasNoMandatory)
            {
                builder.Append(this.Get(aSeries.notMandatory, index));
            }
            builder.Append(this.Get(aSeries.mandatory, index));
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                builder.Append(this.Get(aSeries.valuesList[i], index));
            }
            return builder.ToString();
        }

        private string SeriesPoints(Series aSeries)
        {
            StringBuilder builder = new StringBuilder(1);
            if (aSeries.Count > 0)
            {
                for (int i = 0; i < aSeries.Count; i++)
                {
                    builder.Append("<point" + this.GetPointString(i, aSeries) + "/>" + base.TextLineSeparator);
                }
            }
            return builder.ToString();
        }

        private string XMLSeries(Series aSeries)
        {
            return (" <series title=\"" + aSeries.ToString() + "\" type=\"" + aSeries.GetType().ToString().Substring(aSeries.GetType().ToString().LastIndexOf(".") + 1) + "\" color=\"" + Utils.ColorToHex(aSeries.Color) + "\">" + base.TextLineSeparator + "  <points count=\"" + aSeries.Count.ToString() + "\">" + base.TextLineSeparator + this.SeriesPoints(aSeries) + "  </points>" + base.TextLineSeparator + " </series>" + base.TextLineSeparator);
        }

        [DefaultValue(false)]
        public bool IncludeColors
        {
            get
            {
                return this.includeColors;
            }
            set
            {
                this.includeColors = value;
            }
        }
    }
}

