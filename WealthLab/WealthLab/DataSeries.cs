namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class DataSeries
    {
        [CompilerGenerated]
        private BarDataScale barDataScale_0;
        private Bars bars_0;
        private bool sumOnCollapse;
        private bool bool_1;
        private DataSeries dataSeries_0;
        private Dictionary<string, DataSeries> dictionary_0;
        private double partialValue;
        private IList<DateTime> ilist_0;
        private int firstValidValue;
        private List<double> dataList;
        private List<DateTime> dateList;
        private string description;

        public DataSeries(string description)
        {
            this.dataList = new List<double>();
            this.partialValue = double.NaN;
            this.dateList = new List<DateTime>();
            this.ilist_0 = this.dateList.AsReadOnly();
            this.description = description;
        }

        public DataSeries(Bars bars, string description)
        {
            this.dataList = new List<double>();
            this.partialValue = double.NaN;
            this.description = description;
            this.bars_0 = bars;
            this.Capacity = bars.Count;
            for (int i = 0; i < bars.Count; i++)
            {
                this.Add(0.0);
            }
            this.bool_1 = true;
            this.DataScale = bars.DataScale;
        }

        public DataSeries(DataSeries source, string description)
        {
            this.dataList = new List<double>();
            this.partialValue = double.NaN;
            this.description = description;
            this.dataSeries_0 = source;
            this.Capacity = source.Count;
            for (int i = 0; i < source.Count; i++)
            {
                this.Add(0.0);
            }
            this.bool_1 = true;
            this.DataScale = source.DataScale;
        }

        internal DataSeries(Bars bars_1, string string_1, bool bool_2)
        {
            this.dataList = new List<double>();
            this.partialValue = double.NaN;
            this.Cache = bars_1.Cache;
            this.description = string_1;
            this.bars_0 = bars_1;
            this.Capacity = bars_1.Count;
            if (bool_2)
            {
                for (int i = 0; i < bars_1.Count; i++)
                {
                    this.Add(0.0);
                }
            }
            this.bool_1 = true;
            this.DataScale = bars_1.DataScale;
        }

        internal DataSeries(DataSeries dataSeries_1, string string_1, bool bool_2)
        {
            this.dataList = new List<double>();
            this.partialValue = double.NaN;
            this.Cache = dataSeries_1.Cache;
            this.description = string_1;
            this.dataSeries_0 = dataSeries_1;
            this.Capacity = dataSeries_1.Count;
            if (bool_2)
            {
                for (int i = 0; i < dataSeries_1.Count; i++)
                {
                    this.Add(0.0);
                }
            }
            this.bool_1 = true;
            this.DataScale = dataSeries_1.DataScale;
        }

        public static DataSeries Abs(DataSeries source)
        {
            DataSeries series = new DataSeries(source, "Abs(" + source.Description + ")", false);
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] < 0.0)
                {
                    series.method_0(source[i] * -1.0);
                }
                else
                {
                    series.method_0(source[i]);
                }
            }
            if (!source.Cache.ContainsKey(series.Description))
            {
                source.Cache[series.Description] = series;
            }
            return series;
        }

        public void Add(double value)
        {
            if (this.bool_1)
            {
                throw new InvalidOperationException("Cannot add values to a DataSeries that is based on another DataSeries or a Bars object");
            }
            this.dataList.Add(value);
        }

        public void Add(double value, DateTime dateTime_0)
        {
            if (this.bool_1)
            {
                throw new InvalidOperationException("Cannot add values to a DataSeries that is based on another DataSeries or a Bars object");
            }
            this.dataList.Add(value);
            this.dateList.Add(dateTime_0);
        }

        public virtual void CalculatePartialValue()
        {
            this.PartialValue = double.NaN;
        }

        public int ConvertDateToBar(DateTime dateTime_0, bool exactMatch)
        {
            if (this.Count != 0)
            {
                if (dateTime_0 < this.Date[0])
                {
                    if (exactMatch)
                    {
                        return -1;
                    }
                    return 0;
                }
                if (dateTime_0 <= this.Date[this.Count - 1])
                {
                    int num = this.DateList.BinarySearch(dateTime_0);
                    if (num >= 0)
                    {
                        return num;
                    }
                    if (!exactMatch)
                    {
                        return ~num;
                    }
                }
            }
            return -1;
        }

        protected Bars FindParentBars()
        {
            if (this.bars_0 != null)
            {
                return this.bars_0;
            }
            if (this.dataSeries_0 != null)
            {
                return this.dataSeries_0.FindParentBars();
            }
            return null;
        }

        public bool IsLastBarOfDay(int int_1)
        {
            if (this.IsIntraday)
            {
                if (int_1 < (this.Count - 1))
                {
                    DateTime time3 = this.Date[int_1];
                    DateTime time4 = this.Date[int_1 + 1];
                    return (time3.Date != time4.Date);
                }
                DateTime time2 = this.Date[int_1];
                for (int i = int_1 - 1; i >= 0; i--)
                {
                    DateTime time5 = this.Date[i];
                    DateTime time6 = this.Date[i + 1];
                    if (time5.Date != time6.Date)
                    {
                        DateTime time = this.Date[i];
                        return ((time.Hour == time2.Hour) && (time.Minute == time2.Minute));
                    }
                }
            }
            return false;
        }

        internal void method_0(double double_1)
        {
            this.dataList.Add(double_1);
        }

        internal void method_1(int int_1, double double_1)
        {
            this.dataList.Insert(int_1, double_1);
        }

        internal void method_2()
        {
            this.dataList.Clear();
        }

        internal void method_3(int int_1)
        {
            this.dataList.RemoveAt(int_1);
        }

        public static DataSeries operator +(double value, DataSeries dataSeries_1)
        {
            return (dataSeries_1 + value);
        }

        public static DataSeries operator +(DataSeries dataSeries_1, double value)
        {
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + "+" + value, false);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                series.method_0(dataSeries_1[i] + value);
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator +(DataSeries dataSeries_1, DataSeries dataSeries_2)
        {
            if (dataSeries_1.Count != dataSeries_2.Count)
            {
                throw new DataSeriesOperatorException();
            }
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + "+" + dataSeries_2.Description);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                series[i] = dataSeries_1[i] + dataSeries_2[i];
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator /(double value, DataSeries dataSeries_1)
        {
            DataSeries series = new DataSeries(dataSeries_1, value + "/" + dataSeries_1.Description, false);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                if (dataSeries_1[i] != 0.0)
                {
                    series.method_0(value / dataSeries_1[i]);
                }
                else
                {
                    series.method_0(0.0);
                }
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator /(DataSeries dataSeries_1, double value)
        {
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + "/" + value, false);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                series.method_0(dataSeries_1[i] / value);
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator /(DataSeries dataSeries_1, DataSeries dataSeries_2)
        {
            if (dataSeries_1.Count != dataSeries_2.Count)
            {
                throw new DataSeriesOperatorException();
            }
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + "/" + dataSeries_2.Description, false);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                if (dataSeries_2[i] != 0.0)
                {
                    series.method_0(dataSeries_1[i] / dataSeries_2[i]);
                }
                else
                {
                    series.method_0(0.0);
                }
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator <<(DataSeries dataSeries_1, int value)
        {
            if ((value < 1) || (value >= dataSeries_1.Count))
            {
                throw new DataSeriesOperatorException();
            }
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + "<<" + value, false);
            for (int i = 0; i < (dataSeries_1.Count - value); i++)
            {
                series.method_0(dataSeries_1[i + value]);
            }
            for (int j = 0; j < value; j++)
            {
                series.method_0(0.0);
            }
            series.FirstValidValue = dataSeries_1.FirstValidValue - value;
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator *(double value, DataSeries dataSeries_1)
        {
            return (DataSeries) (dataSeries_1 * value);
        }

        public static DataSeries operator *(DataSeries dataSeries_1, double value)
        {
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + "*" + value, false);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                series.method_0(dataSeries_1[i] * value);
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator *(DataSeries dataSeries_1, DataSeries dataSeries_2)
        {
            if (dataSeries_1.Count != dataSeries_2.Count)
            {
                throw new DataSeriesOperatorException();
            }
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + "*" + dataSeries_2.Description, false);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                series.method_0(dataSeries_1[i] * dataSeries_2[i]);
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator >>(DataSeries dataSeries_1, int value)
        {
            if ((value < 1) || (value >= dataSeries_1.Count))
            {
                throw new DataSeriesOperatorException();
            }
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + ">>" + value, false);
            for (int i = 0; i < value; i++)
            {
                series.method_0(0.0);
            }
            for (int j = 0; j < (dataSeries_1.Count - value); j++)
            {
                series.method_0(dataSeries_1[j]);
            }
            series.FirstValidValue = dataSeries_1.FirstValidValue + value;
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator -(double value, DataSeries dataSeries_1)
        {
            DataSeries series = new DataSeries(dataSeries_1, value + "-" + dataSeries_1.Description, false);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                series.method_0(value - dataSeries_1[i]);
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator -(DataSeries dataSeries_1, double value)
        {
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + "-" + value, false);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                series.method_0(dataSeries_1[i] - value);
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public static DataSeries operator -(DataSeries dataSeries_1, DataSeries dataSeries_2)
        {
            if (dataSeries_1.Count != dataSeries_2.Count)
            {
                throw new DataSeriesOperatorException();
            }
            DataSeries series = new DataSeries(dataSeries_1, dataSeries_1.Description + "-" + dataSeries_2.Description, false);
            for (int i = 0; i < dataSeries_1.Count; i++)
            {
                series.method_0(dataSeries_1[i] - dataSeries_2[i]);
            }
            if (!dataSeries_1.Cache.ContainsKey(series.Description))
            {
                dataSeries_1.Cache[series.Description] = series;
            }
            return series;
        }

        public Dictionary<string, DataSeries> Cache
        {
            get
            {
                if (this.dictionary_0 != null)
                {
                    return this.dictionary_0;
                }
                Bars bars = this.FindParentBars();
                if (bars == null)
                {
                    this.dictionary_0 = new Dictionary<string, DataSeries>();
                    return this.dictionary_0;
                }
                return bars.Cache;
            }
            internal set
            {
                this.dictionary_0 = value;
            }
        }

        internal int Capacity
        {
            get
            {
                return this.dataList.Capacity;
            }
            set
            {
                this.dataList.Capacity = value;
            }
        }

        public bool ContainsInfinity
        {
            get
            {
                bool flag;
                using (List<double>.Enumerator enumerator = this.dataList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        double current = enumerator.Current;
                        if (double.IsInfinity(current))
                        {
                            ///goto  Label_002B; ///WYJ fix, simplify the flow
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        public int Count
        {
            get
            {
                return this.dataList.Count;
            }
        }

        public BarDataScale DataScale
        {
            [CompilerGenerated]
            get
            {
                return this.barDataScale_0;
            }
            [CompilerGenerated]
            set
            {
                this.barDataScale_0 = value;
            }
        }

        public IList<DateTime> Date
        {
            get
            {
                if (this.ilist_0 == null)
                {
                    if (this.dataSeries_0 != null)
                    {
                        this.ilist_0 = this.dataSeries_0.Date;
                    }
                    else
                    {
                        this.ilist_0 = this.bars_0.Date;
                    }
                }
                return this.ilist_0;
            }
        }

        private List<DateTime> DateList
        {
            get
            {
                if (this.dateList != null)
                {
                    return this.dateList;
                }
                if (this.bars_0 != null)
                {
                    return this.bars_0.DateList;
                }
                if (this.dataSeries_0 != null)
                {
                    return this.dataSeries_0.dateList;
                }
                return null;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public int FirstValidValue
        {
            get
            {
                return this.firstValidValue;
            }
            set
            {
                if (value < 0)
                {
                    this.firstValidValue = 0;
                }
                else
                {
                    this.firstValidValue = value;
                }
                if (this.firstValidValue >= this.Count)
                {
                    this.firstValidValue = this.Count - 1;
                }
            }
        }

        public bool IsIntraday
        {
            get
            {
                return this.DataScale.IsIntraday;
            }
        }

        public double this[int int_1]
        {
            get
            {
                return this.dataList[int_1];
            }
            set
            {
                this.dataList[int_1] = value;
            }
        }

        public double MaxValue
        {
            get
            {
                if (this.Count == 0)
                {
                    return 0.0;
                }
                double num2 = this.dataList[0];
                for (int i = this.FirstValidValue; i < this.Count; i++)
                {
                    if (this.dataList[i] > num2)
                    {
                        num2 = this.dataList[i];
                    }
                }
                return num2;
            }
        }

        public double MinValue
        {
            get
            {
                if (this.Count == 0)
                {
                    return 0.0;
                }
                double num2 = this.dataList[0];
                for (int i = this.FirstValidValue; i < this.Count; i++)
                {
                    if (this.dataList[i] < num2)
                    {
                        num2 = this.dataList[i];
                    }
                }
                return num2;
            }
        }

        public double PartialValue
        {
            get
            {
                return this.partialValue;
            }
            set
            {
                this.partialValue = value;
            }
        }

        public bool SumOnCollapse
        {
            get
            {
                return this.sumOnCollapse;
            }
            set
            {
                this.sumOnCollapse = value;
            }
        }
    }
}

