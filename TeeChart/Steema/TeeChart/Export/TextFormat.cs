namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Text;

    public class TextFormat : DataExportFormat
    {
        public string TextDelimiter;

        public TextFormat(Chart c) : base(c)
        {
            this.TextDelimiter = Texts.TabDelimiter;
            base.FileExtension = "txt";
        }

        private void Add(string st, ref string tmpResult)
        {
            tmpResult = (tmpResult.Length == 0) ? st : (tmpResult + this.TextDelimiter + st);
        }

        private void DoSeries(int index, Series aSeries, ref int tmpNum, ref string tmpResult)
        {
            if (base.hasLabels)
            {
                if (aSeries.Count > index)
                {
                    this.Add(aSeries.Labels[index], ref tmpResult);
                }
                else
                {
                    this.Add("", ref tmpResult);
                }
            }
            if (base.hasNoMandatory)
            {
                if (aSeries.Count > index)
                {
                    this.Add(aSeries.notMandatory[index].ToString(), ref tmpResult);
                }
                else
                {
                    this.Add("", ref tmpResult);
                }
            }
            if (aSeries.Count > index)
            {
                this.Add(aSeries.mandatory[index].ToString(), ref tmpResult);
            }
            else
            {
                this.Add("", ref tmpResult);
            }
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                if (aSeries.Count > index)
                {
                    tmpResult = tmpResult + this.TextDelimiter + aSeries.valuesList[i][index].ToString();
                }
                else
                {
                    tmpResult = tmpResult + this.TextDelimiter;
                }
            }
        }

        internal override string FilterFiles()
        {
            return Texts.TextFilter;
        }

        internal override string GetContent()
        {
            base.Prepare();
            StringBuilder builder = new StringBuilder(1);
            builder.Append(base.IncludeSeriesTitle ? (this.HeaderTitle() + base.TextLineSeparator) : "");
            builder.Append(base.IncludeHeader ? (this.Header() + base.TextLineSeparator) : "");
            builder.Append(base.GetContent() + base.TextLineSeparator);
            return builder.ToString();
        }

        private string Header()
        {
            StringBuilder builder = new StringBuilder(1);
            builder.Append(base.IncludeIndex ? Texts.Index : "");
            if (builder.Length != 0)
            {
                builder.Append(this.TextDelimiter);
            }
            if (base.Series != null)
            {
                builder.Append(this.HeaderSeries(base.Series));
            }
            else if (base.Chart.Series.Count > 0)
            {
                builder.Append(this.HeaderSeries(base.Chart[0]));
                for (int i = 1; i < base.Chart.Series.Count; i++)
                {
                    builder.Append(this.TextDelimiter + this.HeaderSeries(base.Chart[i]));
                }
            }
            return builder.ToString();
        }

        private string HeaderSeries(Series aSeries)
        {
            StringBuilder builder = new StringBuilder(1);
            if (base.hasLabels)
            {
                builder.Append(Texts.Text);
                builder.Append(this.TextDelimiter);
            }
            if (base.hasNoMandatory)
            {
                builder.Append(aSeries.notMandatory.Name);
                builder.Append(this.TextDelimiter);
            }
            builder.Append(aSeries.mandatory.Name);
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                builder.Append(this.TextDelimiter + aSeries.valuesList[i].Name);
            }
            return builder.ToString();
        }

        private string HeaderSeriesTitle(Series aSeries)
        {
            StringBuilder builder = new StringBuilder(1);
            if (base.hasLabels)
            {
                builder.Append(aSeries.ToString() + this.TextDelimiter);
            }
            if (base.hasNoMandatory && !base.hasLabels)
            {
                builder.Append(aSeries.ToString() + this.TextDelimiter);
            }
            if (!base.hasLabels && !base.hasNoMandatory)
            {
                builder.Append(aSeries.ToString() + this.TextDelimiter);
            }
            else
            {
                builder.Append(this.TextDelimiter);
            }
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                builder.Append(this.TextDelimiter);
            }
            return builder.ToString();
        }

        private string HeaderTitle()
        {
            StringBuilder builder = new StringBuilder(1);
            if (base.IncludeIndex)
            {
                builder.Append(this.TextDelimiter);
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
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        internal override string PointToString(int index)
        {
            string tmpResult = base.IncludeIndex ? index.ToString() : "";
            int tmpNum = 0;
            if (base.Series != null)
            {
                this.DoSeries(index, base.Series, ref tmpNum, ref tmpResult);
                return tmpResult;
            }
            foreach (Series series in base.Chart.Series)
            {
                this.DoSeries(index, series, ref tmpNum, ref tmpResult);
            }
            return tmpResult;
        }
    }
}

