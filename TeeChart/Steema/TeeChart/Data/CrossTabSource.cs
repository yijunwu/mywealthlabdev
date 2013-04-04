namespace Steema.TeeChart.Data
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;

    [Serializable, Description("Fills multiple series from a crosstab of database records."), ToolboxBitmap(typeof(CrossTabSource), "Images.CrossTabSource.bmp")]
    public class CrossTabSource : SeriesSource
    {
        private bool bCase;
        private GroupFormula formula;
        private string groupField;
        private string labelField;
        private object oDataSource;
        private string valueField;

        public CrossTabSource()
        {
            this.bCase = true;
            this.groupField = "";
            this.labelField = "";
            this.valueField = "";
            this.formula = GroupFormula.Sum;
        }

        public CrossTabSource(object source) : this()
        {
            this.oDataSource = source;
        }

        private void AddFromAdapter(IDataAdapter d)
        {
            DataSet dataSet = new DataSet();
            d.Fill(dataSet);
            if (dataSet.Tables.Count > 0)
            {
                this.AddFromTable(dataSet.Tables[0]);
            }
        }

        private void AddFromTable(DataTable d)
        {
            int index = 0;
            DataColumn column2 = d.Columns[this.ValueField];
            if (column2 != null)
            {
                Steema.TeeChart.Styles.Series series;
                DataColumn column;
                DataColumn column3 = (this.LabelField == "") ? null : d.Columns[this.LabelField];
                if (this.GroupField == "")
                {
                    series = this.Series;
                    this.Series.Title = this.ValueField;
                    column = null;
                }
                else
                {
                    series = null;
                    column = d.Columns[this.GroupField];
                }
                double num = 1.0;
                foreach (DataRow row in d.Rows)
                {
                    string str;
                    if (this.GroupField != "")
                    {
                        string title = row[column].ToString();
                        series = this.LocateSeries(title);
                        if (series == null)
                        {
                            if (this.Series.Title == "")
                            {
                                series = this.Series;
                            }
                            else
                            {
                                series = Steema.TeeChart.Styles.Series.CreateNewSeries(this.Series.chart, this.Series.GetType(), null);
                                series.Clear();
                                series.manualData = true;
                                series.AssignFormat(this.Series);
                                series.InternalUse = true;
                                series.Color = this.Series.chart.FreeSeriesColor(false);
                                series.Tag = this.Series;
                                for (int i = 0; i < this.Series.Count; i++)
                                {
                                    series.Add((double) 0.0, this.Series.Labels[i]);
                                }
                            }
                            series.Title = title;
                        }
                    }
                    if (this.formula != GroupFormula.Count)
                    {
                        num = Convert.ToDouble(row[column2]);
                    }
                    if (column3 == null)
                    {
                        str = "";
                        index = (series.Count > 0) ? 0 : -1;
                    }
                    else
                    {
                        str = row[column3].ToString();
                        index = series.Labels.IndexOf(str, this.bCase);
                    }
                    if (index == -1)
                    {
                        series.Add(num, str);
                        foreach (Steema.TeeChart.Styles.Series series2 in this.Series.chart.series)
                        {
                            if ((series != series2) && (series.Count > series2.Count))
                            {
                                for (int j = 1; j <= (series.Count - series2.Count); j++)
                                {
                                    series2.Add((double) 0.0, str);
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (this.formula)
                        {
                            case GroupFormula.Count:
                            case GroupFormula.Sum:
                                ValueList list;
                                int num5;
                                (list = series.mandatory)[num5 = index] = list[num5] + num;
                                break;

                            case GroupFormula.Min:
                                series.mandatory[index] = Math.Min(series.mandatory[index], num);
                                break;

                            case GroupFormula.Max:
                                series.mandatory[index] = Math.Max(series.mandatory[index], num);
                                break;

                            case GroupFormula.Product:
                                ValueList list2;
                                int num6;
                                (list2 = series.mandatory)[num6 = index] = list2[num6] * num;
                                break;
                        }
                    }
                }
            }
            this.Series.chart.BroadcastEvent(this.Series, SeriesEventStyle.Add);
        }

        private Steema.TeeChart.Styles.Series LocateSeries(string title)
        {
            if (this.bCase)
            {
                foreach (Steema.TeeChart.Styles.Series series in this.Series.chart.series)
                {
                    if (series.Title == title)
                    {
                        return series;
                    }
                }
            }
            else
            {
                string str = title.ToUpper();
                foreach (Steema.TeeChart.Styles.Series series2 in this.Series.chart.series)
                {
                    if (series2.Title.ToUpper() == str)
                    {
                        return series2;
                    }
                }
            }
            return null;
        }

        public override void RefreshData()
        {
            this.RemoveSeries();
            this.Series.BeginUpdate();
            try
            {
                if (this.DataSource is DataTable)
                {
                    this.AddFromTable((DataTable) this.DataSource);
                }
                else if (this.DataSource is DataSet)
                {
                    DataSet dataSource = (DataSet) this.DataSource;
                    if (dataSource.Tables.Count > 0)
                    {
                        this.AddFromTable(dataSource.Tables[0]);
                    }
                }
                else if (this.DataSource is DataView)
                {
                    this.AddFromTable(((DataView) this.DataSource).Table);
                }
                else if (this.DataSource is IDbDataAdapter)
                {
                    IDbDataAdapter d = (IDbDataAdapter) this.DataSource;
                    if (d.SelectCommand != null)
                    {
                        this.AddFromAdapter(d);
                    }
                }
                else if (this.DataSource is DataAdapter)
                {
                    this.AddFromAdapter((DataAdapter) this.DataSource);
                }
            }
            finally
            {
                this.Series.EndUpdate();
            }
        }

        private void RemoveSeries()
        {
            if (this.Series.chart != null)
            {
                for (int i = this.Series.chart.Series.Count - 1; i >= 0; i--)
                {
                    if ((this.Series.chart.Series[i] != this.Series) && (this.Series.chart.Series[i].Tag == this.Series))
                    {
                        this.Series.chart.Series[i].Dispose();
                    }
                }
            }
            this.Series.Clear();
            this.Series.Title = "";
        }

        [Description("When records in the database contain labels or group texts that are only different in case, setting this property to true will consider texts as different, thus creating new series or groups in the final chart.")]
        public bool CaseSensitive
        {
            get
            {
                return this.bCase;
            }
            set
            {
                if (this.bCase != value)
                {
                    this.bCase = value;
                }
            }
        }

        [Description("Stores the Datasource associated with this CrossTabSource.")]
        public object DataSource
        {
            get
            {
                return this.oDataSource;
            }
            set
            {
                this.oDataSource = value;
            }
        }

        [Description("Determines how will the crosstab use database values to add points to series.")]
        public GroupFormula Formula
        {
            get
            {
                return this.formula;
            }
            set
            {
                if (this.formula != value)
                {
                    this.formula = value;
                }
            }
        }

        [Description("The field in the dataset to be used as grouping field. Different values in this field will be used to create additional series.")]
        public string GroupField
        {
            get
            {
                return this.groupField;
            }
            set
            {
                if (this.groupField != value)
                {
                    this.groupField = value;
                }
            }
        }

        [Description("The field in the dataset to be used to add labels (text) to series points.")]
        public string LabelField
        {
            get
            {
                return this.labelField;
            }
            set
            {
                if (this.labelField != value)
                {
                    this.labelField = value;
                }
            }
        }

        [Browsable(false)]
        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return base.Series;
            }
            set
            {
                base.Series = value;
            }
        }

        [Description("The field in the dataset where values will be obtained to perform accumulation in the final crosstab chart.")]
        public string ValueField
        {
            get
            {
                return this.valueField;
            }
            set
            {
                if (this.valueField != value)
                {
                    this.valueField = value;
                }
            }
        }
    }
}

