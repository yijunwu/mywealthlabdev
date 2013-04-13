namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public static class BarScaleConverter
    {
        private static MarketHours marketHours_0 = new MarketHours();

        public static Bars ReScale(Bars bars_0, BarScale scale, int interval)
        {
            switch (scale)
            {
                case BarScale.Daily:
                    return ToDaily(bars_0);

                case BarScale.Weekly:
                    return ToWeekly(bars_0);

                case BarScale.Monthly:
                    return ToMonthly(bars_0);

                case BarScale.Quarterly:
                    return ToQuarterly(bars_0);

                case BarScale.Yearly:
                    return ToYearly(bars_0);
            }
            return ToIntradayCompressed(bars_0, scale, interval);
        }

        private static void smethod_0(Bars bars_0, Bars bars_1)
        {
            if (bars_0.HasNamedDataSeries)
            {
                foreach (DataSeries series in bars_0.NamedSeries)
                {
                    DataSeries series2 = new DataSeries(bars_1, series.Description);
                    bars_1.RegisterNamedSeries(series2.Description, series.SumOnCollapse);
                    series2.PartialValue = 0.0;
                }
            }
        }

        private static void smethod_1(Bars bars_0, Bars bars_1)
        {
            if (bars_1.HasNamedDataSeries)
            {
                foreach (DataSeries series in bars_1.NamedSeries)
                {
                    int num = series.Count - 1;
                    series[num] = series.PartialValue;
                    series.PartialValue = 0.0;
                }
            }
        }

        private static void smethod_2(Bars bars_0, Bars bars_1, int int_0)
        {
            if (bars_1.HasNamedDataSeries)
            {
                foreach (DataSeries series in bars_1.NamedSeries)
                {
                    int num = series.Count - 1;
                    DataSeries series2 = bars_0.FindNamedSeries(series.Description);
                    series[num] = series2[int_0];
                }
            }
        }

        private static void smethod_3(Bars bars_0, double double_0)
        {
            if (bars_0.HasNamedDataSeries)
            {
                foreach (DataSeries series in bars_0.NamedSeries)
                {
                    int num = series.Count - 1;
                    series[num] = double_0;
                }
            }
        }

        private static void smethod_4(Bars bars_0, Bars bars_1, int int_0)
        {
            if (bars_1.HasNamedDataSeries)
            {
                foreach (DataSeries series in bars_1.NamedSeries)
                {
                    double num = bars_0.FindNamedSeries(series.Description)[int_0];
                    if (series.SumOnCollapse)
                    {
                        series.PartialValue += num;
                    }
                    else
                    {
                        series.PartialValue = num;
                    }
                }
            }
        }

        ///WYJ fix, code from Reflector, modified
        public static Bars Synchronize(Bars source, Bars bars)
        {
            if (bars.DataScale > source.DataScale)
            {
                source = ReScale(source, bars.Scale, bars.BarInterval);
            }
            Bars bars2 = new Bars(source.Symbol, bars.Scale, bars.BarInterval) {
                SecurityName = source.SecurityName,
                SymbolInfo = source.SymbolInfo,
                MarketInfo = source.MarketInfo
            };
            bool flag3 = !source.IsIntraday || !bars.IsIntraday;
            if (bars.Count != 0)
            {
                int num2;
                DateTime time;
                DateTime time2;
                if (source.Date[0] > bars.Date[bars.Count - 1])
                {
                    throw new SynchronizationException("Cannot Synchronize, no overlapping Dates");
                }
                smethod_0(source, bars2);
                if (source.DataScale < bars.DataScale)
                {
                    num2 = 0;
                    time = source.Date[0];
                    for (int j = 0; j < bars.Count; j++)
                    {
                        bool flag = false;
                        time2 = bars.Date[j];
                        DateTime time3 = time2;
                        if (source.IsIntraday && !bars.IsIntraday)
                        {
                            time3 = time2.Date.AddDays(1.0).AddMinutes(-1.0);
                        }
                        while (time <= time3)
                        {
                            if (num2 == (source.Count - 1))
                            {
                                ///goto  Label_0168;  ///WYJ fix, simplify the flow
                                flag = true;
                                int num9 = source.Count - 1;
                                bars2.Add(time2, source.Open[num9], source.High[num9], source.Low[num9], source.Close[num9], source.Volume[num9]);
                                smethod_2(source, bars2, num9);
                                break; 
                            }
                            num2++;
                            time = source.Date[num2];
                        }
                        if (!flag)
                        {
                            if (num2 == 0)
                            {
                                bars2.Add(time2, 0.0, 0.0, 0.0, 0.0, 0.0);
                                smethod_2(source, bars2, 0);
                                bars2.FirstActualBar = bars2.Count;
                            }
                            else
                            {
                                int num5 = num2 - 1;
                                bars2.Add(time2, source.Open[num5], source.High[num5], source.Low[num5], source.Close[num5], source.Volume[num5]);
                                smethod_2(source, bars2, num5);
                            }
                        }
                    }
                    return bars2;
                }
                if (!source.IsIntraday && bars.IsIntraday)
                {
                    num2 = 0;
                    for (int k = 0; k < bars.Count; k++)
                    {
                        DateTime time7 = bars.Date[k];
                        DateTime date = time7.Date;
                        bool flag2 = true;
                        while (flag2)
                        {
                            if (num2 == (source.Count - 1))
                            {
                                flag2 = false;
                            }
                            else
                            {
                                DateTime time5 = source.Date[num2];
                                if (time5.Date == date)
                                {
                                    flag2 = false;
                                }
                                else
                                {
                                    DateTime time6 = source.Date[num2 + 1];
                                    if (time6.Date > date)
                                    {
                                        flag2 = false;
                                    }
                                }
                            }
                            if (flag2)
                            {
                                num2++;
                            }
                        }
                        int num7 = num2;
                        DateTime time10 = source.Date[num2];
                        if ((date == time10.Date) && !bars.IsLastBarOfDay(k))
                        {
                            num7--;
                            if (num7 < 0)
                            {
                                num7 = 0;
                            }
                            if (date > source.Date[source.Count - 1])
                            {
                                num7 = source.Count - 1;
                            }
                        }
                        bars2.Add(source.Date[num7], source.Open[num7], source.High[num7], source.Low[num7], source.Close[num7], source.Volume[num7]);
                        smethod_2(source, bars2, num7);
                    }
                    return bars2;
                }
                num2 = 0;
                num2 = 0;
                for (int i = 0; i < bars.Count; i++)
                {
                    if (num2 >= source.Count)
                    {
                        num2 = source.Count - 1;
                    }
                    time2 = bars.Date[i];
                    time = source.Date[num2];
                    if (flag3 ? (time.Date == time2.Date) : (time == time2))
                    {
                        int num8 = num2;
                        bars2.Add(source.Date[num8], source.Open[num8], source.High[num8], source.Low[num8], source.Close[num8], source.Volume[num8]);
                        smethod_2(source, bars2, num8);
                        num2++;
                    }
                    else
                    {
                        if (time <= time2)
                        {
                            while (num2 < (source.Count - 1))
                            {
                                ///goto  Label_0593;  ///WYJ fix, simplify the flow
                                if (source.Date[num2 + 1] >= time2)
                                {
                                    ///goto Label_05B9;  ///WYJ fix, simplify the flow
                                    break;
                                }
                                num2++;
                            }
                            if (((source.Date[num2] < time2) && (num2 < (source.Count - 1))) && (source.Date[num2 + 1] == time2))
                            {
                                num2++;
                            }
                            bars2.Add(source.Date[num2], source.Open[num2], source.High[num2], source.Low[num2], source.Close[num2], source.Volume[num2]);
                            smethod_2(source, bars2, num2);
                            num2++;
                            continue;
                        }
                        if (num2 == 0)
                        {
                            bars2.Add(bars.Date[0], 0.0, 0.0, 0.0, 0.0, 0.0);
                            smethod_2(source, bars2, 0);
                            bars2.FirstActualBar = bars2.Count;
                        }
                        else
                        {
                            int num6 = num2 - 1;
                            if (num6 < 0)
                            {
                                num6 = 0;
                            }
                            bars2.Add(source.Date[num6], source.Open[num6], source.High[num6], source.Low[num6], source.Close[num6], source.Volume[num6]);
                            smethod_2(source, bars2, num6);
                        }
                    }
                }
                bars2.Open.Description = source.Open.Description;
                bars2.High.Description = source.High.Description;
                bars2.Low.Description = source.Low.Description;
                bars2.Close.Description = source.Close.Description;
                bars2.Volume.Description = source.Volume.Description;
            }
            return bars2;
        } 

        ///WYJ fix, code from JustDecompile
        /*
        public static Bars Synchronize(Bars source, Bars bars)
        {
            int count;
            DateTime item;
            DateTime dateTime;
            bool flag;
            bool flag1;
            if (bars.DataScale > source.DataScale)
            {
                source = BarScaleConverter.ReScale(source, bars.Scale, bars.BarInterval);
            }
            Bars bar = new Bars(source.Symbol, bars.Scale, bars.BarInterval);
            bar.SecurityName = source.SecurityName;
            bar.SymbolInfo = source.SymbolInfo;
            bar.MarketInfo = source.MarketInfo;
            flag = (!source.IsIntraday ? true : !bars.IsIntraday);
            bool flag2 = flag;
            if (bars.Count != 0)
            {
                if (source.Date[0] <= bars.Date[bars.Count - 1])
                {
                    BarScaleConverter.smethod_0(source, bar);
                    if (source.DataScale >= bars.DataScale)
                    {
                        if (source.IsIntraday || !bars.IsIntraday)
                        {
                            count = 0;
                            count = 0;
                            for (int i = 0; i < bars.Count; i++)
                            {
                                if (count >= source.Count)
                                {
                                    count = source.Count - 1;
                                }
                                dateTime = bars.Date[i];
                                item = source.Date[count];
                                flag1 = (flag2 ? item.Date == dateTime.Date : item == dateTime);
                                if (!flag1)
                                {
                                    if (item <= dateTime)
                                    {
                                        while (count < source.Count - 1 && source.Date[count + 1] < dateTime)
                                        {
                                            count++;
                                        }
                                        if (source.Date[count] < dateTime && count < source.Count - 1 && source.Date[count + 1] == dateTime)
                                        {
                                            count++;
                                        }
                                        bar.Add(source.Date[count], source.Open[count], source.High[count], source.Low[count], source.Close[count], source.Volume[count]);
                                        BarScaleConverter.smethod_2(source, bar, count);
                                        count++;
                                    }
                                    else
                                    {
                                        if (count != 0)
                                        {
                                            int num = count - 1;
                                            if (num < 0)
                                            {
                                                num = 0;
                                            }
                                            bar.Add(source.Date[num], source.Open[num], source.High[num], source.Low[num], source.Close[num], source.Volume[num]);
                                            BarScaleConverter.smethod_2(source, bar, num);
                                        }
                                        else
                                        {
                                            bar.Add(bars.Date[0], 0, 0, 0, 0, 0);
                                            BarScaleConverter.smethod_2(source, bar, 0);
                                            bar.FirstActualBar = bar.Count;
                                        }
                                    }
                                }
                                else
                                {
                                    int num1 = count;
                                    bar.Add(source.Date[num1], source.Open[num1], source.High[num1], source.Low[num1], source.Close[num1], source.Volume[num1]);
                                    BarScaleConverter.smethod_2(source, bar, num1);
                                    count++;
                                }
                            }
                            bar.Open.Description = source.Open.Description;
                            bar.High.Description = source.High.Description;
                            bar.Low.Description = source.Low.Description;
                            bar.Close.Description = source.Close.Description;
                            bar.Volume.Description = source.Volume.Description;
                            return bar;
                        }
                        else
                        {
                            count = 0;
                            for (int j = 0; j < bars.Count; j++)
                            {
                                DateTime item1 = bars.Date[j];
                                DateTime date = item1.Date;
                                bool flag3 = true;
                                while (flag3)
                                {
                                    if (count != source.Count - 1)
                                    {
                                        DateTime dateTime1 = source.Date[count];
                                        if (dateTime1.Date != date)
                                        {
                                            DateTime item2 = source.Date[count + 1];
                                            if (item2.Date > date)
                                            {
                                                flag3 = false;
                                            }
                                        }
                                        else
                                        {
                                            flag3 = false;
                                        }
                                    }
                                    else
                                    {
                                        flag3 = false;
                                    }
                                    if (!flag3)
                                    {
                                        continue;
                                    }
                                    count++;
                                }
                                int count1 = count;
                                DateTime dateTime2 = source.Date[count];
                                if (date == dateTime2.Date && !bars.IsLastBarOfDay(j))
                                {
                                    count1--;
                                    if (count1 < 0)
                                    {
                                        count1 = 0;
                                    }
                                    if (date > source.Date[source.Count - 1])
                                    {
                                        count1 = source.Count - 1;
                                    }
                                }
                                bar.Add(source.Date[count1], source.Open[count1], source.High[count1], source.Low[count1], source.Close[count1], source.Volume[count1]);
                                BarScaleConverter.smethod_2(source, bar, count1);
                            }
                            return bar;
                        }
                    }
                    else
                    {
                        count = 0;
                        item = source.Date[0];
                        for (int k = 0; k < bars.Count; k++)
                        {
                            bool flag4 = false;
                            dateTime = bars.Date[k];
                            DateTime dateTime3 = dateTime;
                            if (source.IsIntraday && !bars.IsIntraday)
                            {
                                DateTime date1 = dateTime.Date;
                                DateTime dateTime4 = date1.AddDays(1);
                                dateTime3 = dateTime4.AddMinutes(-1);
                            }
                            while (true)
                            {
                                if (item <= dateTime3)
                                {
                                    if (count != source.Count - 1)
                                    {
                                        count++;
                                        item = source.Date[count];
                                    }
                                    else
                                    {
                                        flag4 = true;
                                        int count2 = source.Count - 1;
                                        bar.Add(dateTime, source.Open[count2], source.High[count2], source.Low[count2], source.Close[count2], source.Volume[count2]);
                                        BarScaleConverter.smethod_2(source, bar, count2);
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (!flag4)
                            {
                                if (count != 0)
                                {
                                    int num2 = count - 1;
                                    bar.Add(dateTime, source.Open[num2], source.High[num2], source.Low[num2], source.Close[num2], source.Volume[num2]);
                                    BarScaleConverter.smethod_2(source, bar, num2);
                                }
                                else
                                {
                                    bar.Add(dateTime, 0, 0, 0, 0, 0);
                                    BarScaleConverter.smethod_2(source, bar, 0);
                                    bar.FirstActualBar = bar.Count;
                                }
                            }
                        }
                        return bar;
                    }
                }
                else
                {
                    throw new SynchronizationException("Cannot Synchronize, no overlapping Dates");
                }
            }
            else
            {
                return bar;
            }
        } */


        public static DataSeries Synchronize(DataSeries source, Bars bars)
        {
            int num = 0;
            bool flag3 = !source.IsIntraday || !bars.IsIntraday;
            if (source.Date == null)
            {
                throw new SynchronizationException("DataSeries does not have associated date list");
            }
            DataSeries series = new DataSeries(source.Description) {
                Cache = source.Cache
            };
            if (bars.Cache.ContainsKey(source.Description))
            {
                bars.Cache[source.Description] = series;
            }
            if (bars.Count != 0)
            {
                DateTime time2;
                DateTime time7;
                if (source.Date[0] > bars.Date[bars.Count - 1])
                {
                    throw new SynchronizationException("Cannot Synchronize, no overlapping Dates");
                }
                if (source.DataScale < bars.DataScale)
                {
                    num = 0;
                    time7 = source.Date[0];
                    for (int j = 0; j < bars.Count; j++)
                    {
                        bool flag2 = false;
                        time2 = bars.Date[j];
                        DateTime time8 = time2;
                        if (source.IsIntraday && !bars.IsIntraday)
                        {
                            time8 = time2.Date.AddDays(1.0).AddMinutes(-1.0);
                        }
                        while (time7 <= time8)
                        {
                            if (num == (source.Count - 1))
                            {
                                ///goto  Label_0152;  ///WYJ fix, simplify the flow
                                flag2 = true;
                                series.Add(source[num], time2);
                                break;
                            }
                            num++;
                            time7 = source.Date[num];
                        }
                        if (!flag2)
                        {
                            if (num == 0)
                            {
                                series.Add(0.0, time2);
                            }
                            else
                            {
                                series.Add(source[num - 1], time2);
                            }
                        }
                    }
                    return series;
                }
                if (!source.IsIntraday && bars.IsIntraday)
                {
                    num = 0;
                    for (int k = 0; k < bars.Count; k++)
                    {
                        DateTime time5 = bars.Date[k];
                        DateTime date = time5.Date;
                        bool flag = true;
                        while (flag)
                        {
                            if (num == (source.Count - 1))
                            {
                                flag = false;
                            }
                            else
                            {
                                DateTime time6 = source.Date[num];
                                if (time6.Date == date)
                                {
                                    flag = false;
                                }
                                else
                                {
                                    DateTime time3 = source.Date[num + 1];
                                    if (time3.Date > date)
                                    {
                                        flag = false;
                                    }
                                }
                            }
                            if (flag)
                            {
                                num++;
                            }
                        }
                        int num3 = num;
                        DateTime time4 = source.Date[num];
                        if ((date == time4.Date) && !bars.IsLastBarOfDay(k))
                        {
                            num3--;
                            if (num3 < 0)
                            {
                                num3 = 0;
                            }
                            if (date > source.Date[source.Count - 1])
                            {
                                num3 = source.Count - 1;
                            }
                        }
                        series.Add(source[num3], source.Date[num3]);
                    }
                    return series;
                }
                for (int i = 0; i < bars.Count; i++)
                {
                    if (num >= source.Count)
                    {
                        num = source.Count - 1;
                    }
                    time2 = bars.Date[i];
                    time7 = source.Date[num];
                    if (flag3 ? (time7.Date == time2.Date) : (time7 == time2))
                    {
                        int num6 = num;
                        series.Add(source[num6], source.Date[num6]);
                        num++;
                    }
                    else
                    {
                        if (time7 <= time2)
                        {
                            goto Label_03BC;
                        }
                        if (num == 0)
                        {
                            series.Add(0.0, source.Date[0]);
                        }
                        else
                        {
                            int num5 = num - 1;
                            if (num5 < 0)
                            {
                                num5 = 0;
                            }
                            series.Add(source[num5], source.Date[num5]);
                        }
                    }
                    continue;
                Label_03A1:
                    if (source.Date[num + 1] >= time2)
                    {
                        goto Label_03C7;
                    }
                    num++;
                Label_03BC:
                    if (num < (source.Count - 1))
                    {
                        goto Label_03A1;
                    }
                Label_03C7:
                    if (((source.Date[num] < time2) && (num < (source.Count - 1))) && (source.Date[num + 1] == time2))
                    {
                        num++;
                    }
                    series.Add(source[num], source.Date[num]);
                    num++;
                }
            }
            return series;
        }

        public static DataSeries Synchronize(DataSeries source, DataSeries synchAgainst)
        {
            int num = 0;
            bool flag3 = !source.IsIntraday || !synchAgainst.IsIntraday;
            if (source.Date == null)
            {
                throw new SynchronizationException("DataSeries does not have associated date list");
            }
            DataSeries series = new DataSeries(source.Description) {
                Cache = source.Cache
            };
            if (synchAgainst.Count != 0)
            {
                DateTime time;
                DateTime time2;
                if (source.Date[0] > synchAgainst.Date[synchAgainst.Count - 1])
                {
                    throw new SynchronizationException("Cannot Synchronize, no overlapping Dates");
                }
                if (source.DataScale < synchAgainst.DataScale)
                {
                    num = 0;
                    time = source.Date[0];
                    for (int j = 0; j < synchAgainst.Count; j++)
                    {
                        bool flag = false;
                        time2 = synchAgainst.Date[j];
                        DateTime time3 = time2;
                        if (source.IsIntraday && !synchAgainst.IsIntraday)
                        {
                            time3 = time2.Date.AddDays(1.0).AddMinutes(-1.0);
                        }
                        while (time <= time3)
                        {
                            if (num == (source.Count - 1))
                            {
                                goto Label_012B;
                            }
                            num++;
                            time = source.Date[num];
                        }
                        goto Label_013D;
                    Label_012B:
                        flag = true;
                        series.Add(source[num], time2);
                    Label_013D:
                        if (!flag)
                        {
                            if (num == 0)
                            {
                                series.Add(0.0, time2);
                            }
                            else
                            {
                                series.Add(source[num - 1], time2);
                            }
                        }
                    }
                    return series;
                }
                if (!source.IsIntraday && synchAgainst.IsIntraday)
                {
                    num = 0;
                    for (int k = 0; k < synchAgainst.Count; k++)
                    {
                        DateTime time7 = synchAgainst.Date[k];
                        DateTime date = time7.Date;
                        bool flag2 = true;
                        while (flag2)
                        {
                            if (num == (source.Count - 1))
                            {
                                flag2 = false;
                            }
                            else
                            {
                                DateTime time5 = source.Date[num];
                                if (time5.Date == date)
                                {
                                    flag2 = false;
                                }
                                else
                                {
                                    DateTime time6 = source.Date[num + 1];
                                    if (time6.Date > date)
                                    {
                                        flag2 = false;
                                    }
                                }
                            }
                            if (flag2)
                            {
                                num++;
                            }
                        }
                        int num6 = num;
                        DateTime time10 = source.Date[num];
                        if ((date == time10.Date) && !synchAgainst.IsLastBarOfDay(k))
                        {
                            num6--;
                            if (num6 < 0)
                            {
                                num6 = 0;
                            }
                            if (date > source.Date[source.Count - 1])
                            {
                                num6 = source.Count - 1;
                            }
                        }
                        series.Add(source[num6], source.Date[num6]);
                    }
                    return series;
                }
                for (int i = 0; i < synchAgainst.Count; i++)
                {
                    if (num >= source.Count)
                    {
                        num = source.Count - 1;
                    }
                    time2 = synchAgainst.Date[i];
                    time = source.Date[num];
                    if (flag3 ? (time.Date == time2.Date) : (time == time2))
                    {
                        int num7 = num;
                        series.Add(source[num7], source.Date[num7]);
                        num++;
                    }
                    else
                    {
                        if (time <= time2)
                        {
                            goto Label_039B;
                        }
                        if (num == 0)
                        {
                            series.Add(0.0, source.Date[0]);
                        }
                        else
                        {
                            int num5 = num - 1;
                            if (num5 < 0)
                            {
                                num5 = 0;
                            }
                            series.Add(source[num5], source.Date[num5]);
                        }
                    }
                    continue;
                Label_0380:
                    if (source.Date[num + 1] >= time2)
                    {
                        goto Label_03A6;
                    }
                    num++;
                Label_039B:
                    if (num < (source.Count - 1))
                    {
                        goto Label_0380;
                    }
                Label_03A6:
                    if (((source.Date[num] < time2) && (num < (source.Count - 1))) && (source.Date[num + 1] == time2))
                    {
                        num++;
                    }
                    series.Add(source[num], source.Date[num]);
                    num++;
                }
            }
            return series;
        }

        public static Bars ToDaily(Bars bars_0)
        {
            if (bars_0.Scale == BarScale.Daily)
            {
                return bars_0;
            }
            if (!bars_0.IsIntraday)
            {
                throw new BarConversionException("Cannot convert " + bars_0.Scale.ToString() + " Bars to Daily Scale");
            }
            Bars bars = new Bars(bars_0.Symbol, BarScale.Daily, 0) {
                SymbolInfo = bars_0.SymbolInfo,
                MarketInfo = bars_0.MarketInfo,
                SecurityName = bars_0.SecurityName
            };
            if (bars_0.Count != 0)
            {
                int num;
                double open = bars_0.Open[0];
                double high = bars_0.High[0];
                double num2 = bars_0.Low[0];
                double volume = bars_0.Volume[0];
                smethod_0(bars_0, bars);
                for (num = 1; num < bars_0.Count; num++)
                {
                    DateTime time = bars_0.Date[num];
                    DateTime time2 = bars_0.Date[num - 1];
                    if (time.Date != time2.Date)
                    {
                        bars.Add(bars_0.Date[num - 1], open, high, num2, bars_0.Close[num - 1], volume, false);
                        smethod_1(bars_0, bars);
                        open = bars_0.Open[num];
                        high = bars_0.High[num];
                        num2 = bars_0.Low[num];
                        volume = bars_0.Volume[num];
                    }
                    else
                    {
                        if (bars_0.High[num] > high)
                        {
                            high = bars_0.High[num];
                        }
                        if (bars_0.Low[num] < num2)
                        {
                            num2 = bars_0.Low[num];
                        }
                        volume += bars_0.Volume[num];
                        smethod_4(bars_0, bars, num);
                    }
                }
                num = bars_0.Count - 1;
                bars.Add(bars_0.Date[num], open, high, num2, bars_0.Close[num], volume, false);
                smethod_1(bars_0, bars);
            }
            return bars;
        }

        public static Bars ToIntradayCompressed(Bars bars_0, BarScale scale, int barInterval)
        {
            int num;
            DateTime time3;
            double num3;
            double num4;
            double num5;
            if ((bars_0.Scale == scale) && (bars_0.BarInterval == barInterval))
            {
                return bars_0;
            }
            bool flag = false;
            switch (bars_0.Scale)
            {
                case BarScale.Minute:
                    if (scale == BarScale.Minute)
                    {
                        flag = (barInterval % bars_0.BarInterval) == 0;
                        break;
                    }
                    flag = false;
                    break;

                case BarScale.Second:
                    switch (scale)
                    {
                        case BarScale.Minute:
                            flag = true;
                            goto Label_00A9;

                        case BarScale.Second:
                            flag = (barInterval % bars_0.BarInterval) == 0;
                            goto Label_00A9;
                    }
                    break;

                case BarScale.Tick:
                    switch (scale)
                    {
                        case BarScale.Minute:
                            flag = true;
                            goto Label_00A9;

                        case BarScale.Second:
                            flag = true;
                            goto Label_00A9;

                        case BarScale.Tick:
                            flag = (barInterval % bars_0.BarInterval) == 0;
                            goto Label_00A9;
                    }
                    break;
            }
        Label_00A9:
            if (!flag)
            {
                throw new BarConversionException(string.Concat(new object[] { "Cannot convert ", bars_0.DataScale.ToString(), " Bars to ", barInterval, " ", scale, " Bars" }));
            }
            Bars bars = new Bars(bars_0.Symbol, scale, barInterval) {
                SymbolInfo = bars_0.SymbolInfo,
                MarketInfo = bars_0.MarketInfo,
                SecurityName = bars_0.SecurityName
            };
            if (bars_0.Count == 0)
            {
                return bars;
            }
            if (scale == BarScale.Tick)
            {
                return bars;
            }
            Dictionary<TimeSpan, int> dictionary = new Dictionary<TimeSpan, int>();
            double open = num3 = num4 = num5 = 0.0;
            if (bars_0.MarketInfo != null)
            {
                DateTime openTimeNative = bars_0.MarketInfo.OpenTimeNative;
                DateTime time6 = bars_0.Date[0];
                time3 = new DateTime(time6.Year, time6.Month, time6.Day, openTimeNative.Hour, openTimeNative.Minute, openTimeNative.Second);
            }
            else
            {
                time3 = bars_0.Date[0];
                num = 0;
                while (num < bars_0.Count)
                {
                    DateTime time2 = bars_0.Date[num];
                    if (time2.Date > time3.Date)
                    {
                        time3 = bars_0.Date[num];
                        break;
                    }
                    num++;
                }
                switch (bars_0.Scale)
                {
                    case BarScale.Minute:
                        time3 = time3.AddMinutes((double) -bars_0.BarInterval);
                        break;

                    case BarScale.Second:
                        time3 = time3.AddSeconds((double) -bars_0.BarInterval);
                        break;
                }
            }
            time3 = new DateTime(time3.Year, time3.Month, time3.Day, time3.Hour, time3.Minute, 0);
            DateTime minValue = DateTime.MinValue;
            for (num = 0; num < bars_0.Count; num++)
            {
                if (bars_0.Date[num] <= minValue)
                {
                    goto Label_0550;
                }
                if (num > 0)
                {
                    DateTime time17 = bars_0.Date[num];
                    DateTime time18 = bars_0.Date[num - 1];
                    if (time17.Date == time18.Date)
                    {
                        bars.Add(minValue, open, num3, num4, bars_0.Close[num - 1], num5, false);
                        smethod_1(bars_0, bars);
                    }
                    else
                    {
                        bars.Add(bars_0.Date[num - 1], open, num3, num4, bars_0.Close[num - 1], num5, false);
                        smethod_1(bars_0, bars);
                        DateTime time12 = bars_0.Date[num];
                        DateTime time13 = bars_0.Date[num];
                        DateTime time14 = bars_0.Date[num];
                        minValue = new DateTime(time12.Year, time13.Month, time14.Day, time3.Hour, time3.Minute, time3.Second);
                        DateTime time15 = bars_0.Date[num - 1];
                        if (dictionary.ContainsKey(time15.TimeOfDay))
                        {
                            Dictionary<TimeSpan, int> dictionary2;
                            TimeSpan span2;
                            DateTime time16 = bars_0.Date[num - 1];
                            (dictionary2 = dictionary)[span2 = time16.TimeOfDay] = dictionary2[span2] + 1;
                        }
                        else
                        {
                            DateTime time7 = bars_0.Date[num - 1];
                            dictionary.Add(time7.TimeOfDay, 1);
                        }
                    }
                }
                else
                {
                    DateTime time8 = bars_0.Date[num];
                    DateTime time9 = bars_0.Date[num];
                    DateTime time10 = bars_0.Date[num];
                    minValue = new DateTime(time8.Year, time9.Month, time10.Day, time3.Hour, time3.Minute, time3.Second);
                    smethod_0(bars_0, bars);
                }
                DateTime time4 = bars_0.Date[num];
                if (time4.TimeOfDay < time3.TimeOfDay)
                {
                    goto Label_04E8;
                }
                while (bars_0.Date[num] > minValue)
                {
                    if (scale == BarScale.Minute)
                    {
                        minValue = minValue.AddMinutes((double) barInterval);
                    }
                    else
                    {
                        minValue = minValue.AddSeconds((double) barInterval);
                    }
                }
                goto Label_0516;
            Label_04CC:
                if (scale == BarScale.Minute)
                {
                    minValue = minValue.AddMinutes((double) -barInterval);
                }
                else
                {
                    minValue = minValue.AddSeconds((double) -barInterval);
                }
            Label_04E8:
                if (bars_0.Date[num] < minValue)
                {
                    goto Label_04CC;
                }
                if (scale == BarScale.Minute)
                {
                    minValue = minValue.AddMinutes((double) barInterval);
                }
                else
                {
                    minValue = minValue.AddSeconds((double) barInterval);
                }
            Label_0516:
                open = bars_0.Open[num];
                num3 = bars_0.High[num];
                num4 = bars_0.Low[num];
                num5 = bars_0.Volume[num];
                continue;
            Label_0550:
                num3 = (bars_0.High[num] > num3) ? bars_0.High[num] : num3;
                num4 = (bars_0.Low[num] < num4) ? bars_0.Low[num] : num4;
                num5 += bars_0.Volume[num];
                smethod_4(bars_0, bars, num);
            }
            TimeSpan? nullable = null;
            int num6 = 0;
            foreach (int num7 in dictionary.Values)
            {
                num6 += num7;
            }
            if (num6 > 2)
            {
                using (Dictionary<TimeSpan, int>.Enumerator enumerator = dictionary.GetEnumerator())
                {
                    KeyValuePair<TimeSpan, int> current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if ((((double) current.Value) / ((double) num6)) > 0.5)
                        {
                            goto Label_063F;
                        }
                    }
                    goto Label_065D;
                Label_063F:
                    nullable = new TimeSpan?(current.Key);
                }
            }
        Label_065D:
            if (nullable.HasValue)
            {
                TimeSpan timeOfDay = minValue.TimeOfDay;
                TimeSpan? nullable3 = nullable;
                if (nullable3.HasValue ? (timeOfDay > nullable3.GetValueOrDefault()) : false)
                {
                    DateTime time11 = (scale == BarScale.Minute) ? minValue.AddMinutes((double) -barInterval) : minValue.AddSeconds((double) -barInterval);
                    TimeSpan span = time11.TimeOfDay;
                    TimeSpan? nullable2 = nullable;
                    if (nullable2.HasValue ? (span < nullable2.GetValueOrDefault()) : false)
                    {
                        minValue = new DateTime(minValue.Year, minValue.Month, minValue.Day, nullable.Value.Hours, nullable.Value.Minutes, nullable.Value.Seconds);
                    }
                }
            }
            num = bars_0.Count - 1;
            bars.Add(minValue, open, num3, num4, bars_0.Close[num], num5, false);
            bars.IsLastCompressedBarPartial = minValue != bars_0.Date[num];
            smethod_2(bars_0, bars, num);
            return bars;
        }

        public static Bars ToMonthly(Bars bars_0)
        {
            if (bars_0.Scale == BarScale.Monthly)
            {
                return bars_0;
            }
            Bars bars = new Bars(bars_0.Symbol, BarScale.Monthly, 0) {
                SymbolInfo = bars_0.SymbolInfo,
                MarketInfo = bars_0.MarketInfo,
                SecurityName = bars_0.SecurityName
            };
            if (bars_0.Count != 0)
            {
                int num;
                double open = bars_0.Open[0];
                double high = bars_0.High[0];
                double num3 = bars_0.Low[0];
                double volume = bars_0.Volume[0];
                smethod_0(bars_0, bars);
                for (num = 1; num < bars_0.Count; num++)
                {
                    DateTime time = bars_0.Date[num];
                    DateTime time2 = bars_0.Date[num - 1];
                    if (time.Month != time2.Month)
                    {
                        bars.Add(bars_0.Date[num - 1], open, high, num3, bars_0.Close[num - 1], volume, false);
                        smethod_1(bars_0, bars);
                        open = bars_0.Open[num];
                        high = bars_0.High[num];
                        num3 = bars_0.Low[num];
                        volume = bars_0.Volume[num];
                    }
                    else
                    {
                        if (bars_0.High[num] > high)
                        {
                            high = bars_0.High[num];
                        }
                        if (bars_0.Low[num] < num3)
                        {
                            num3 = bars_0.Low[num];
                        }
                        volume += bars_0.Volume[num];
                        smethod_4(bars_0, bars, num);
                    }
                }
                num = bars_0.Count - 1;
                bars.Add(bars_0.Date[num], open, high, num3, bars_0.Close[num], volume, false);
                smethod_1(bars_0, bars);
            }
            return bars;
        }

        public static Bars ToQuarterly(Bars bars_0)
        {
            if (bars_0.Scale == BarScale.Quarterly)
            {
                return bars_0;
            }
            Bars bars = new Bars(bars_0.Symbol, BarScale.Quarterly, 0) {
                SymbolInfo = bars_0.SymbolInfo,
                MarketInfo = bars_0.MarketInfo,
                SecurityName = bars_0.SecurityName
            };
            if (bars_0.Count != 0)
            {
                int num;
                double open = bars_0.Open[0];
                double high = bars_0.High[0];
                double num2 = bars_0.Low[0];
                double volume = bars_0.Volume[0];
                smethod_0(bars_0, bars);
                for (num = 1; num < bars_0.Count; num++)
                {
                    DateTime time = bars_0.Date[num];
                    DateTime time2 = bars_0.Date[num - 1];
                    if ((time.Month != time2.Month) && (((time.Month - 1) % 3) == 0))
                    {
                        bars.Add(bars_0.Date[num - 1], open, high, num2, bars_0.Close[num - 1], volume, false);
                        smethod_1(bars_0, bars);
                        open = bars_0.Open[num];
                        high = bars_0.High[num];
                        num2 = bars_0.Low[num];
                        volume = bars_0.Volume[num];
                    }
                    else
                    {
                        if (bars_0.High[num] > high)
                        {
                            high = bars_0.High[num];
                        }
                        if (bars_0.Low[num] < num2)
                        {
                            num2 = bars_0.Low[num];
                        }
                        volume += bars_0.Volume[num];
                        smethod_4(bars_0, bars, num);
                    }
                }
                num = bars_0.Count - 1;
                bars.Add(bars_0.Date[num], open, high, num2, bars_0.Close[num], volume, false);
                smethod_1(bars_0, bars);
            }
            return bars;
        }

        public static Bars ToWeekly(Bars bars_0)
        {
            if (bars_0.Scale == BarScale.Weekly)
            {
                return bars_0;
            }
            if (bars_0.Scale == BarScale.Monthly)
            {
                throw new BarConversionException("Cannot convert Monthly bars to Weekly");
            }
            Bars bars = new Bars(bars_0.Symbol, BarScale.Weekly, 0) {
                SecurityName = bars_0.SecurityName,
                SymbolInfo = bars_0.SymbolInfo,
                MarketInfo = bars_0.MarketInfo
            };
            if (bars_0.Count != 0)
            {
                int num;
                double open = bars_0.Open[0];
                double high = bars_0.High[0];
                double num2 = bars_0.Low[0];
                double volume = bars_0.Volume[0];
                smethod_0(bars_0, bars);
                for (num = 1; num < bars_0.Count; num++)
                {
                    DateTime time = bars_0.Date[num];
                    DateTime time2 = bars_0.Date[num - 1];
                    if (time.DayOfWeek < time2.DayOfWeek)
                    {
                        bars.Add(bars_0.Date[num - 1], open, high, num2, bars_0.Close[num - 1], volume, false);
                        smethod_1(bars_0, bars);
                        open = bars_0.Open[num];
                        high = bars_0.High[num];
                        num2 = bars_0.Low[num];
                        volume = bars_0.Volume[num];
                    }
                    else
                    {
                        if (bars_0.High[num] > high)
                        {
                            high = bars_0.High[num];
                        }
                        if (bars_0.Low[num] < num2)
                        {
                            num2 = bars_0.Low[num];
                        }
                        volume += bars_0.Volume[num];
                        smethod_4(bars_0, bars, num);
                    }
                }
                num = bars_0.Count - 1;
                bars.Add(bars_0.Date[num], open, high, num2, bars_0.Close[num], volume, false);
                smethod_1(bars_0, bars);
            }
            return bars;
        }

        public static Bars ToYearly(Bars bars_0)
        {
            if (bars_0.Scale == BarScale.Yearly)
            {
                return bars_0;
            }
            Bars bars = new Bars(bars_0.Symbol, BarScale.Yearly, 0) {
                SymbolInfo = bars_0.SymbolInfo,
                MarketInfo = bars_0.MarketInfo,
                SecurityName = bars_0.SecurityName
            };
            if (bars_0.Count != 0)
            {
                int num;
                double open = bars_0.Open[0];
                double high = bars_0.High[0];
                double num3 = bars_0.Low[0];
                double volume = bars_0.Volume[0];
                smethod_0(bars_0, bars);
                for (num = 1; num < bars_0.Count; num++)
                {
                    DateTime time = bars_0.Date[num];
                    DateTime time2 = bars_0.Date[num - 1];
                    if (time.Year != time2.Year)
                    {
                        bars.Add(bars_0.Date[num - 1], open, high, num3, bars_0.Close[num - 1], volume, false);
                        smethod_1(bars_0, bars);
                        open = bars_0.Open[num];
                        high = bars_0.High[num];
                        num3 = bars_0.Low[num];
                        volume = bars_0.Volume[num];
                    }
                    else
                    {
                        if (bars_0.High[num] > high)
                        {
                            high = bars_0.High[num];
                        }
                        if (bars_0.Low[num] < num3)
                        {
                            num3 = bars_0.Low[num];
                        }
                        volume += bars_0.Volume[num];
                        smethod_4(bars_0, bars, num);
                    }
                }
                num = bars_0.Count - 1;
                bars.Add(bars_0.Date[num], open, high, num3, bars_0.Close[num], volume, false);
                smethod_1(bars_0, bars);
            }
            return bars;
        }
    }
}

