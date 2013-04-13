namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Bars
    {
        private BarScale barScale_0;
        private bool bool_0;
        private bool bool_1;
        [CompilerGenerated]
        private bool bool_2;
        private DataSeries dataSeries_0;
        private DataSeries dataSeries_1;
        private DataSeries dataSeries_2;
        private DataSeries dataSeries_3;
        private DataSeries dataSeries_4;
        private Dictionary<string, DataSeries> dictionary_0;
        private Dictionary<string, DataSeries> dictionary_1;
        private IList<DateTime> ilist_0;
        private int int_0;
        private int[] int_1;
        private int int_2;
        private List<DateTime> list_0;
        private List<DateTime> list_1;
        private List<DateTime> list_2;
        private WealthLab.MarketInfo marketInfo_0;
        private object object_0;
        private object object_1;
        private string string_0;
        private string string_1;
        private string string_2;
        private WealthLab.SymbolInfo symbolInfo_0;
        private static SymbolLock symbolLock_0 = new SymbolLock();

        public Bars(Bars bars_0) : this(bars_0.Symbol, bars_0.Scale, bars_0.BarInterval)
        {
            this.SecurityName = bars_0.SecurityName;
            this.MarketInfo = bars_0.MarketInfo;
            this.SymbolInfo = bars_0.SymbolInfo;
            this.Tag = bars_0.Tag;
            foreach (DataSeries series in bars_0.NamedSeries)
            {
                DataSeries series2 = new DataSeries(series.Description);
                this.dictionary_0[series.Description] = series2;
            }
            this.bool_1 = bars_0.bool_1;
        }

        public Bars(string symbol, BarScale scale, int barInterval)
        {
            this.string_0 = "";
            this.list_0 = new List<DateTime>();
            this.string_1 = "";
            this.string_2 = "";
            this.int_0 = 5;
            this.dictionary_0 = new Dictionary<string, DataSeries>();
            this.dictionary_1 = new Dictionary<string, DataSeries>();
            this.list_2 = new List<DateTime>();
            this.string_1 = symbol;
            this.barScale_0 = scale;
            if (this.IsIntraday)
            {
                this.int_0 = barInterval;
            }
            else
            {
                this.int_0 = 0;
            }
            this.dataSeries_0 = new DataSeries(this, "Open");
            this.dataSeries_0.DataScale = this.DataScale;
            this.dataSeries_1 = new DataSeries(this, "High");
            this.dataSeries_1.DataScale = this.DataScale;
            this.dataSeries_2 = new DataSeries(this, "Low");
            this.dataSeries_2.DataScale = this.DataScale;
            this.dataSeries_3 = new DataSeries(this, "Close");
            this.dataSeries_3.DataScale = this.DataScale;
            this.dataSeries_4 = new DataSeries(this, "Volume");
            this.dataSeries_4.DataScale = this.DataScale;
            this.list_0 = new List<DateTime>();
            this.ilist_0 = this.list_0.AsReadOnly();
        }

        internal Bars(string string_3, BarScale barScale_1, int int_3, List<DateTime> list_3, DataSeries dataSeries_5, DataSeries dataSeries_6, DataSeries dataSeries_7, DataSeries dataSeries_8, DataSeries dataSeries_9)
        {
            this.string_0 = "";
            this.list_0 = new List<DateTime>();
            this.string_1 = "";
            this.string_2 = "";
            this.int_0 = 5;
            this.dictionary_0 = new Dictionary<string, DataSeries>();
            this.dictionary_1 = new Dictionary<string, DataSeries>();
            this.list_2 = new List<DateTime>();
            this.string_1 = string_3;
            this.barScale_0 = barScale_1;
            this.int_0 = int_3;
            this.list_0 = list_3;
            this.ilist_0 = this.list_0.AsReadOnly();
            this.dataSeries_0 = dataSeries_5;
            this.dataSeries_1 = dataSeries_6;
            this.dataSeries_2 = dataSeries_7;
            this.dataSeries_3 = dataSeries_8;
            this.dataSeries_4 = dataSeries_9;
        }

        public void Add(DateTime dateTime_0, double open, double high, double double_0, double close, double volume)
        {
            this.Add(dateTime_0, open, high, double_0, close, volume, true);
        }

        public void Add(DateTime dateTime_0, double open, double high, double double_0, double close, double volume, bool eliminateTimeIfNonIntraday)
        {
            if (this.bool_0)
            {
                throw new BarsLockedException();
            }
            if (!this.IsIntraday && eliminateTimeIfNonIntraday)
            {
                this.list_0.Add(dateTime_0.Date);
            }
            else
            {
                this.list_0.Add(dateTime_0);
            }
            this.dataSeries_0.method_0(open);
            this.dataSeries_1.method_0(high);
            this.dataSeries_2.method_0(double_0);
            this.dataSeries_3.method_0(close);
            this.dataSeries_4.method_0(volume);
            foreach (DataSeries series in this.dictionary_0.Values)
            {
                series.method_0(0.0);
            }
        }

        public int Append(Bars bars)
        {
            DateTime minValue;
            if (this.bool_0)
            {
                throw new BarsLockedException();
            }
            int num = 0;
            if (this.Count > 0)
            {
                minValue = this.Date[this.Count - 1];
            }
            else
            {
                minValue = DateTime.MinValue;
            }
            for (int i = 0; i < bars.Count; i++)
            {
                if ((bars.Date[i] > minValue) && ((this.Count == 0) || (bars.Date[i] > this.Date[this.Count - 1])))
                {
                    this.Add(bars.Date[i], bars.Open[i], bars.High[i], bars.Low[i], bars.Close[i], bars.Volume[i]);
                    num++;
                    if (bars.HasNamedDataSeries)
                    {
                        foreach (DataSeries series in bars.dictionary_0.Values)
                        {
                            DataSeries series2 = this.FindNamedSeries(series.Description);
                            if (series2 != null)
                            {
                                series2[i] = series[i];
                            }
                        }
                    }
                }
            }
            return num;
        }

        public int AppendWithCorrections(Bars bars, out int CorrectionsApplied)
        {
            if (this.bool_0)
            {
                throw new BarsLockedException();
            }
            int num = 0;
            CorrectionsApplied = 0;
            ListCompareIterator<DateTime> iterator = new ListCompareIterator<DateTime>(this.Date, bars.Date);
            while (iterator.State != ListCompareState.Unmatch)
            {
                while (iterator.State == ListCompareState.Item1)
                {
                    iterator.Advance();
                }
                while (iterator.State != ListCompareState.Unmatch)
                {
                    int num2;
                    int num3;
                    if (iterator.State == ListCompareState.Match)
                    {
                        num2 = iterator.Index1;
                        num3 = iterator.Index2;
                        if ((((this.Open[num2] != bars.Open[num3]) || (this.High[num2] != bars.High[num3])) || (((this.Low[num2] != bars.Low[num3]) || (this.Close[num2] != bars.Close[num3])) || (this.Volume[num2] != bars.Volume[num3]))) && !this.UserEditedDates.Contains(this.Date[num2]))
                        {
                            CorrectionsApplied++;
                            this.Open[num2] = bars.Open[num3];
                            this.High[num2] = bars.High[num3];
                            this.Low[num2] = bars.Low[num3];
                            this.Close[num2] = bars.Close[num3];
                            this.Volume[num2] = bars.Volume[num3];
                            foreach (DataSeries series in bars.dictionary_0.Values)
                            {
                                DataSeries series2 = this.FindNamedSeries(series.Description);
                                if (series2 != null)
                                {
                                    series2[num2] = series[num3];
                                }
                            }
                        }
                    }
                    else if (iterator.State == ListCompareState.Item2)
                    {
                        num2 = iterator.Index1;
                        num3 = iterator.Index2;
                        this.method_4(num2, bars.Date[num3], bars.Open[num3], bars.High[num3], bars.Low[num3], bars.Close[num3], bars.Volume[num3]);
                        foreach (DataSeries series3 in bars.dictionary_0.Values)
                        {
                            DataSeries series4 = this.FindNamedSeries(series3.Description);
                            if (series4 != null)
                            {
                                series4[num2] = series3[num3];
                            }
                        }
                        num++;
                    }
                    iterator.Advance();
                }
            }
            return (this.Append(bars) + num);
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
                    int num = this.list_0.BinarySearch(dateTime_0);
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

        public void Delete(int int_3)
        {
            if (this.bool_0)
            {
                throw new BarsLockedException();
            }
            this.list_0.RemoveAt(int_3);
            this.dataSeries_0.method_3(int_3);
            this.dataSeries_1.method_3(int_3);
            this.dataSeries_2.method_3(int_3);
            this.dataSeries_3.method_3(int_3);
            this.dataSeries_4.method_3(int_3);
            if (this.HasNamedDataSeries)
            {
                foreach (DataSeries series in this.dictionary_0.Values)
                {
                    series.method_3(int_3);
                }
            }
        }

        public DataSeries FindNamedSeries(string name)
        {
            DataSeries series;
            using (Dictionary<string, DataSeries>.KeyCollection.Enumerator enumerator = this.dictionary_0.Keys.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == name)
                    {
                        ///goto  Label_0030;  ///WYJ fix, simplify the flow
                        series = this.dictionary_0[name];
                        return series;
                    }
                }
                return null;
            }
        }

        public string FormatValue(double value)
        {
            return value.ToString("N" + this.SymbolInfo.Decimals);
        }

        public int IntradayBarNumber(int int_3)
        {
            if (!this.IsIntraday)
            {
                return -1;
            }
            if ((this.int_1 == null) || (this.int_1.Length < this.Count))
            {
                this.int_1 = new int[this.Count];
                int num = 0;
                DateTime time3 = this.Date[0];
                DateTime date = time3.Date;
                this.int_1[0] = 0;
                for (int i = 1; i < this.Count; i++)
                {
                    DateTime time = this.Date[i];
                    DateTime time2 = this.Date[i - 1];
                    if (time.Date != time2.Date)
                    {
                        num = 0;
                    }
                    else
                    {
                        num++;
                    }
                    this.int_1[i] = num;
                }
            }
            return this.int_1[int_3];
        }

        public bool IsLastBarOfDay(int int_3)
        {
            if (this.IsIntraday)
            {
                if (int_3 < (this.Count - 1))
                {
                    DateTime time3 = this.Date[int_3];
                    DateTime time4 = this.Date[int_3 + 1];
                    return (time3.Date != time4.Date);
                }
                DateTime time2 = this.Date[int_3];
                for (int i = int_3 - 1; i >= 0; i--)
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

        public bool IsLimitDownDay(int int_3)
        {
            if (this.Scale != BarScale.Daily)
            {
                return false;
            }
            if (this.SymbolInfo.SecurityType != SecurityType.Future)
            {
                return false;
            }
            if (int_3 == 0)
            {
                return false;
            }
            return ((((this.Open[int_3] == this.High[int_3]) && (this.Open[int_3] == this.Low[int_3])) && (this.Open[int_3] == this.Close[int_3])) && (this.Open[int_3] < this.Close[int_3 - 1]));
        }

        public bool IsLimitUpDay(int int_3)
        {
            if (this.Scale != BarScale.Daily)
            {
                return false;
            }
            if (this.SymbolInfo.SecurityType != SecurityType.Future)
            {
                return false;
            }
            if (int_3 == 0)
            {
                return false;
            }
            return ((((this.Open[int_3] == this.High[int_3]) && (this.Open[int_3] == this.Low[int_3])) && (this.Open[int_3] == this.Close[int_3])) && (this.Open[int_3] > this.Close[int_3 - 1]));
        }

        public bool IsSynthetic(int int_3)
        {
            if (this.list_1 == null)
            {
                return false;
            }
            return this.list_1.Contains(this.Date[int_3]);
        }

        public void LoadFromFile(string fileName)
        {
            this.LoadFromFile(fileName, DateTime.MinValue, DateTime.MaxValue, 0);
        }

        public void LoadFromFile(string fileName, int maxBars)
        {
            this.LoadFromFile(fileName, DateTime.MinValue, DateTime.MaxValue, maxBars);
        }

        public void LoadFromFile(string fileName, DateTime startDate, DateTime endDate)
        {
            this.LoadFromFile(fileName, startDate, endDate, 0);
        }

        public void LoadFromFile(string fileName, DateTime startDate, DateTime endDate, int maxBars)
        {
            if (this.bool_0)
            {
                throw new BarsLockedException();
            }
            DateTime now = DateTime.Now;
            FileStream input = null;
            double num = 0.0;
            try
            {
                double num10;
                double num11;
                double num12;
                double num13;
                double num14;
                this.method_0();
                symbolLock_0.LockSymbol(this.Symbol);
                input = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader reader = new BinaryReader(input);
                num = reader.ReadDouble();
                reader.ReadString();
                this.SecurityName = reader.ReadString();
                this.barScale_0 = (BarScale) reader.ReadInt32();
                this.int_0 = reader.ReadInt32();
                int num2 = reader.ReadInt32();
                int num3 = reader.ReadInt32();
                for (int i = 0; i < num3; i++)
                {
                    string seriesName = reader.ReadString();
                    bool sumOnCollapse = reader.ReadBoolean();
                    this.RegisterNamedSeries(seriesName, sumOnCollapse);
                }
                if (num >= 2.0)
                {
                    int num16 = reader.ReadInt32();
                    this.SymbolInfo.SecurityType = (SecurityType) num16;
                }
                if (num >= 3.0)
                {
                    int num6 = reader.ReadInt32();
                    while (num6 > 0)
                    {
                        num6--;
                        this.list_2.Add(new DateTime(reader.ReadInt64()));
                    }
                }
                long num7 = 8 + (8 * (5 + num3));
                long offset = 8 * (5 + num3);
                long position = input.Position;
                if (((startDate == DateTime.MinValue) && (endDate == DateTime.MaxValue)) && (maxBars == 0))
                {
                    try
                    {
                        while (num2 > 0)
                        {
                            now = new DateTime(reader.ReadInt64());
                            num10 = reader.ReadDouble();
                            num11 = reader.ReadDouble();
                            num12 = reader.ReadDouble();
                            num13 = reader.ReadDouble();
                            num14 = reader.ReadDouble();
                            this.Add(now, num10, num11, num12, num13, num14);
                            foreach (DataSeries series2 in this.dictionary_0.Values)
                            {
                                series2[series2.Count - 1] = reader.ReadDouble();
                            }
                            num2--;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    return;
                }
                if (((startDate == DateTime.MinValue) && (endDate == DateTime.MaxValue)) && (maxBars > 0))
                {
                    int num5 = 0;
                    if (num2 > maxBars)
                    {
                        num5 = num2 - maxBars;
                    }
                    if (num5 > 0)
                    {
                        input.Seek(num7 * num5, SeekOrigin.Current);
                    }
                    num2 -= num5;
                    try
                    {
                        while (num2 > 0)
                        {
                            now = new DateTime(reader.ReadInt64());
                            num10 = reader.ReadDouble();
                            num11 = reader.ReadDouble();
                            num12 = reader.ReadDouble();
                            num13 = reader.ReadDouble();
                            num14 = reader.ReadDouble();
                            this.Add(now, num10, num11, num12, num13, num14);
                            foreach (DataSeries series in this.dictionary_0.Values)
                            {
                                series[series.Count - 1] = reader.ReadDouble();
                            }
                            num2--;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    return;
                }
                try
                {
                    if ((num2 > 0x3e8) && (startDate != DateTime.MinValue))
                    {
                        int num15 = 0x3e8;
                        while (num15 < num2)
                        {
                            input.Seek((num7 * num15) + position, SeekOrigin.Begin);
                            now = new DateTime(reader.ReadInt64());
                            if (now > startDate)
                            {
                                break;
                            }
                            num15 += 0x3e8;
                        }
                        num15 -= 0x3e8;
                        input.Seek((num7 * num15) + position, SeekOrigin.Begin);
                        num2 -= num15;
                    }
                    while (num2 > 0)
                    {
                        now = new DateTime(reader.ReadInt64());
                        if ((startDate != DateTime.MinValue) && (now < startDate))
                        {
                            input.Seek(offset, SeekOrigin.Current);
                        }
                        else
                        {
                            if ((endDate != DateTime.MaxValue) && (now.Date > endDate))
                            {
                                ///goto  Label_0468; ///WYJ fix, simplify the flow
                                break;
                            }
                            num10 = reader.ReadDouble();
                            num11 = reader.ReadDouble();
                            num12 = reader.ReadDouble();
                            num13 = reader.ReadDouble();
                            num14 = reader.ReadDouble();
                            foreach (DataSeries series3 in this.dictionary_0.Values)
                            {
                                series3[series3.Count - 1] = reader.ReadDouble();
                            }
                            this.Add(now, num10, num11, num12, num13, num14);
                        }
                        num2--;
                    }
                }
                catch (Exception)
                {
                }
            ///Label_0468:
                if (maxBars > 0)
                {
                    while (this.Count > maxBars)
                    {
                        this.Delete(0);
                    }
                }
            }
            catch (EndOfStreamException)
            {
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                }
                symbolLock_0.UnlockSymbol(this.Symbol);
            }
        }

        internal void method_0()
        {
            this.list_0.Clear();
            this.dataSeries_0.method_2();
            this.dataSeries_1.method_2();
            this.dataSeries_2.method_2();
            this.dataSeries_3.method_2();
            this.dataSeries_4.method_2();
            this.dictionary_0.Clear();
        }

        internal void method_1()
        {
            this.bool_0 = true;
        }

        internal void method_2()
        {
            this.bool_0 = false;
        }

        internal void method_3(FundamentalItem fundamentalItem_0)
        {
            fundamentalItem_0.Bar = -1;
            if (((this.Count != 0) && (fundamentalItem_0.Date >= this.Date[0])) && (fundamentalItem_0.Date <= this.Date[this.Count - 1]))
            {
                int num = this.list_0.BinarySearch(fundamentalItem_0.Date);
                if (num >= 0)
                {
                    fundamentalItem_0.Bar = num;
                }
                else if (num != ~this.Count)
                {
                    fundamentalItem_0.Bar = ~num - 1;
                }
            }
        }

        internal void method_4(int int_3, DateTime dateTime_0, double double_0, double double_1, double double_2, double double_3, double double_4)
        {
            this.int_1 = null;
            this.list_0.Insert(int_3, dateTime_0);
            this.dataSeries_0.method_1(int_3, double_0);
            this.dataSeries_1.method_1(int_3, double_1);
            this.dataSeries_2.method_1(int_3, double_2);
            this.dataSeries_3.method_1(int_3, double_3);
            this.dataSeries_4.method_1(int_3, double_4);
            foreach (DataSeries series in this.dictionary_0.Values)
            {
                series.method_1(int_3, 0.0);
            }
        }

        internal void method_5(int int_3)
        {
            this.int_1 = null;
            this.list_0.Insert(int_3, this.Date[int_3]);
            this.dataSeries_0.method_1(int_3, this.Open[int_3]);
            this.dataSeries_1.method_1(int_3, this.High[int_3]);
            this.dataSeries_2.method_1(int_3, this.Low[int_3]);
            this.dataSeries_3.method_1(int_3, this.Close[int_3]);
            this.dataSeries_4.method_1(int_3, this.Volume[int_3]);
            foreach (DataSeries series in this.dictionary_0.Values)
            {
                series.method_1(int_3, series[int_3]);
            }
        }

        internal void method_6(int int_3)
        {
            if (this.list_1 == null)
            {
                this.list_1 = new List<DateTime>();
            }
            if (!this.list_1.Contains(this.Date[int_3]))
            {
                this.list_1.Add(this.Date[int_3]);
            }
        }

        internal int method_7(bool bool_3)
        {
            if (this.Scale != BarScale.Daily)
            {
                throw new InvalidOperationException("AddCalendarDays allowed only on Daily data");
            }
            if (this.Count < 2)
            {
                return 0;
            }
            int num2 = 0;
            for (int i = 0; i < (this.Count - 1); i++)
            {
                DateTime time = this.Date[i];
                DateTime time2 = this.Date[i + 1];
                TimeSpan span = (TimeSpan) (time2 - time);
                int days = span.Days;
                if (days > 1)
                {
                    double num6 = (this.Open[i + 1] - this.Open[i]) / ((double) days);
                    double num7 = (this.High[i + 1] - this.High[i]) / ((double) days);
                    double num8 = (this.Low[i + 1] - this.Low[i]) / ((double) days);
                    double num9 = (this.Close[i + 1] - this.Close[i]) / ((double) days);
                    for (int j = 1; j <= (days - 1); j++)
                    {
                        this.method_5(i + 1);
                        num2++;
                    }
                    for (int k = 1; k <= (days - 1); k++)
                    {
                        this.method_6(i + k);
                        this.list_0[i + k] = this.Date[i].AddDays((double) k);
                        this.dataSeries_4[i + k] = 0.0;
                        if (bool_3)
                        {
                            this.dataSeries_0[i + k] = this.Open[i] + (num6 * k);
                            this.dataSeries_1[i + k] = this.High[i] + (num7 * k);
                            this.dataSeries_2[i + k] = this.Low[i] + (num8 * k);
                            this.dataSeries_3[i + k] = this.Close[i] + (num9 * k);
                        }
                    }
                    i += days - 1;
                }
            }
            return num2;
        }

        public DataSeries RegisterNamedSeries(string seriesName, bool sumOnCollapse)
        {
            if (this.bool_0)
            {
                throw new BarsLockedException();
            }
            DataSeries series = this.FindNamedSeries(seriesName);
            if (series == null)
            {
                series = new DataSeries(this, seriesName);
                this.dictionary_0.Add(seriesName, series);
            }
            series.SumOnCollapse = sumOnCollapse;
            for (int i = series.Count; i < this.Count; i++)
            {
                series.method_0(0.0);
            }
            this.bool_1 = true;
            return series;
        }

        public void SaveToFile(string fileName)
        {
            FileStream output = null;
            double num = 3.0;
            FileNameValidator.ValidateFileName(fileName);
            try
            {
                symbolLock_0.LockSymbol(this.Symbol);
                output = File.Create(fileName);
                BinaryWriter writer = new BinaryWriter(output);
                writer.Write(num);
                writer.Write(this.Symbol);
                writer.Write(this.SecurityName);
                writer.Write((int) this.Scale);
                writer.Write(this.BarInterval);
                writer.Write(this.Count);
                writer.Write(this.dictionary_0.Count);
                foreach (KeyValuePair<string, DataSeries> pair in this.dictionary_0)
                {
                    writer.Write(pair.Key);
                    writer.Write(pair.Value.SumOnCollapse);
                }
                writer.Write((int) this.SymbolInfo.SecurityType);
                writer.Write(this.list_2.Count);
                foreach (DateTime time in this.list_2)
                {
                    writer.Write(time.Ticks);
                }
                for (int i = 0; i < this.Count; i++)
                {
                    DateTime time2 = this.Date[i];
                    long ticks = time2.Ticks;
                    writer.Write(ticks);
                    writer.Write(this.Open[i]);
                    writer.Write(this.High[i]);
                    writer.Write(this.Low[i]);
                    writer.Write(this.Close[i]);
                    writer.Write(this.Volume[i]);
                    foreach (DataSeries series in this.dictionary_0.Values)
                    {
                        writer.Write(series[i]);
                    }
                }
            }
            finally
            {
                if (output != null)
                {
                    output.Close();
                }
                symbolLock_0.UnlockSymbol(this.Symbol);
            }
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "Bars(", this.Symbol, ",", this.Scale.ToString(), ",", this.BarInterval.ToString(), ",", this.Count, ")" });
        }

        public int BarInterval
        {
            get
            {
                return this.int_0;
            }
            set
            {
                if (!this.bool_0)
                {
                    this.int_0 = value;
                    if (this.dataSeries_0 != null)
                    {
                        this.dataSeries_0.DataScale = this.DataScale;
                        this.dataSeries_1.DataScale = this.DataScale;
                        this.dataSeries_2.DataScale = this.DataScale;
                        this.dataSeries_3.DataScale = this.DataScale;
                        this.dataSeries_4.DataScale = this.DataScale;
                    }
                }
            }
        }

        public Dictionary<string, DataSeries> Cache
        {
            get
            {
                return this.dictionary_1;
            }
        }

        public DataSeries Close
        {
            get
            {
                return this.dataSeries_3;
            }
        }

        public int Count
        {
            get
            {
                return this.list_0.Count;
            }
        }

        public BarDataScale DataScale
        {
            get
            {
                return new BarDataScale(this.Scale, this.BarInterval);
            }
        }

        public IList<DateTime> Date
        {
            get
            {
                return this.ilist_0;
            }
        }

        internal List<DateTime> DateList
        {
            get
            {
                return this.list_0;
            }
        }

        internal object DivTag
        {
            get
            {
                return this.object_0;
            }
            set
            {
                this.object_0 = value;
            }
        }

        public int FirstActualBar
        {
            get
            {
                return this.int_2;
            }
            internal set
            {
                this.int_2 = value;
            }
        }

        public bool HasNamedDataSeries
        {
            get
            {
                return this.bool_1;
            }
        }

        public DataSeries High
        {
            get
            {
                return this.dataSeries_1;
            }
        }

        public bool IsIntraday
        {
            get
            {
                return this.DataScale.IsIntraday;
            }
        }

        public bool IsLastCompressedBarPartial
        {
            [CompilerGenerated]
            get
            {
                return this.bool_2;
            }
            [CompilerGenerated]
            set
            {
                this.bool_2 = value;
            }
        }

        internal bool Locked
        {
            get
            {
                return this.bool_0;
            }
        }

        public DataSeries Low
        {
            get
            {
                return this.dataSeries_2;
            }
        }

        public WealthLab.MarketInfo MarketInfo
        {
            get
            {
                return this.marketInfo_0;
            }
            set
            {
                this.marketInfo_0 = value;
            }
        }

        public ICollection<DataSeries> NamedSeries
        {
            get
            {
                return this.dictionary_0.Values;
            }
        }

        public DataSeries Open
        {
            get
            {
                return this.dataSeries_0;
            }
        }

        public BarScale Scale
        {
            get
            {
                return this.barScale_0;
            }
            set
            {
                if (!this.bool_0)
                {
                    this.barScale_0 = value;
                    if (this.dataSeries_0 != null)
                    {
                        this.dataSeries_0.DataScale = this.DataScale;
                        this.dataSeries_1.DataScale = this.DataScale;
                        this.dataSeries_2.DataScale = this.DataScale;
                        this.dataSeries_3.DataScale = this.DataScale;
                        this.dataSeries_4.DataScale = this.DataScale;
                    }
                }
            }
        }

        public string SecurityName
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public string Symbol
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }

        public WealthLab.SymbolInfo SymbolInfo
        {
            get
            {
                if (this.symbolInfo_0 == null)
                {
                    this.symbolInfo_0 = new WealthLab.SymbolInfo();
                    this.symbolInfo_0.Symbol = this.Symbol;
                    this.symbolInfo_0.SecurityType = SecurityType.Equity;
                    this.symbolInfo_0.Margin = 0.0;
                    this.symbolInfo_0.PointValue = 1.0;
                    this.symbolInfo_0.Decimals = DecimalsManager.Instance.Pricing;
                    int decimals = this.symbolInfo_0.Decimals;
                    this.symbolInfo_0.Tick = 1.0;
                    while (decimals > 0)
                    {
                        this.symbolInfo_0.Tick /= 10.0;
                        decimals--;
                    }
                    this.symbolInfo_0.MarketName = "US Equities";
                }
                return this.symbolInfo_0;
            }
            set
            {
                this.symbolInfo_0 = value;
            }
        }

        public object Tag
        {
            get
            {
                return this.object_1;
            }
            set
            {
                this.object_1 = value;
            }
        }

        public string UniqueDescription
        {
            get
            {
                if (this.string_0 == "")
                {
                    this.string_0 = this.ToString();
                }
                return this.string_0;
            }
        }

        public List<DateTime> UserEditedDates
        {
            get
            {
                return this.list_2;
            }
        }

        public DataSeries Volume
        {
            get
            {
                return this.dataSeries_4;
            }
        }
    }
}

