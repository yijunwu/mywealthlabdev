namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Text;

    public class HTMLFormat : DataExportFormat
    {
        private string emptyCell;

        public HTMLFormat(Chart c) : base(c)
        {
            this.emptyCell = "<td></td>";
            base.FileExtension = "htm";
        }

        private string Celldouble(double value)
        {
            return ("<td>" + value.ToString() + "</td>");
        }

        internal override string FilterFiles()
        {
            return Texts.HTMLFilter;
        }

        internal override string GetContent()
        {
            base.Prepare();
            StringBuilder builder = new StringBuilder(1);
            builder.Append("<table border=\"1\">" + base.TextLineSeparator);
            if (base.IncludeSeriesTitle)
            {
                builder.Append(this.TitleHeader() + base.TextLineSeparator);
            }
            if (base.IncludeHeader)
            {
                builder.Append(this.Header() + base.TextLineSeparator);
            }
            builder.Append(base.GetContent() + "</table>");
            return builder.ToString();
        }

        private string GetPointString(int index)
        {
            StringBuilder builder = new StringBuilder(1);
            builder.Append(base.IncludeIndex ? ("<td>" + index.ToString() + "</td>") : "");
            if (base.Series != null)
            {
                builder.Append(this.GetPointStringSeries(base.Series, index));
            }
            else
            {
                foreach (Series series in base.Chart.Series)
                {
                    builder.Append(this.GetPointStringSeries(series, index));
                }
            }
            return builder.ToString();
        }

        private string GetPointStringSeries(Series aSeries, int index)
        {
            int num;
            StringBuilder builder = new StringBuilder("");
            if ((aSeries.Count - 1) < index)
            {
                if (base.hasLabels)
                {
                    builder.Append(this.emptyCell);
                }
                if (base.hasNoMandatory)
                {
                    builder.Append(this.emptyCell);
                }
                builder.Append(this.emptyCell);
                for (num = 2; num < aSeries.valuesList.Count; num++)
                {
                    builder.Append(this.emptyCell);
                }
            }
            else
            {
                if (base.hasLabels)
                {
                    builder.Append("<td>" + aSeries.Labels[index] + "</td>");
                }
                if (base.hasNoMandatory)
                {
                    builder.Append(this.Celldouble(aSeries.notMandatory[index]));
                }
                builder.Append(this.Celldouble(aSeries.mandatory[index]));
                for (num = 2; num < aSeries.valuesList.Count; num++)
                {
                    builder.Append(this.Celldouble(aSeries.valuesList[num][index]));
                }
            }
            return builder.ToString();
        }

        private string Header()
        {
            StringBuilder builder = new StringBuilder("<tr>");
            if (base.IncludeIndex)
            {
                builder.Append("<td>" + Texts.Index + "</td>");
            }
            if (base.Series != null)
            {
                builder.Append(this.HeaderSeries(base.Series));
            }
            else
            {
                foreach (Series series in base.Chart.Series)
                {
                    builder.Append(this.HeaderSeries(series));
                }
            }
            builder.Append("</tr>");
            return builder.ToString();
        }

        private string HeaderSeries(Series aSeries)
        {
            StringBuilder builder = new StringBuilder(1);
            if (base.hasLabels)
            {
                builder.Append("<td>" + Texts.Text + "</td>");
            }
            if (base.hasNoMandatory)
            {
                builder.Append("<td>" + aSeries.notMandatory.Name + "</td>");
            }
            builder.Append("<td>" + aSeries.mandatory.Name + "</td>");
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                builder.Append("<td>" + aSeries.valuesList[i].Name + "</td>");
            }
            return builder.ToString();
        }

        private string HeaderSeriesTitle(Series aSeries)
        {
            StringBuilder builder = new StringBuilder(1);
            if (base.hasLabels)
            {
                builder.Append("<td>" + aSeries.ToString() + "</td>");
            }
            if (base.hasNoMandatory && !base.hasLabels)
            {
                builder.Append("<td>" + aSeries.ToString() + "</td>");
            }
            if (!base.hasLabels && !base.hasNoMandatory)
            {
                builder.Append("<td>" + aSeries.ToString() + "</td>");
            }
            else
            {
                builder.Append("<td></td>");
            }
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                builder.Append("<td></td>");
            }
            return builder.ToString();
        }

        internal override string PointToString(int index)
        {
            return ("<tr>" + this.GetPointString(index) + "</tr>");
        }

        private string TitleHeader()
        {
            StringBuilder builder = new StringBuilder("<tr>");
            if (base.IncludeIndex)
            {
                builder.Append("<td></td>");
            }
            if (base.Series != null)
            {
                builder.Append(this.HeaderSeriesTitle(base.Series));
            }
            else
            {
                foreach (Series series in base.Chart.Series)
                {
                    builder.Append(this.HeaderSeriesTitle(series));
                }
            }
            builder.Append("</tr>");
            return builder.ToString();
        }
    }
}

