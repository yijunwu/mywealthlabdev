using System;
using System.Collections.Generic;
using WealthLab;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.Yahoo;

///WYJ fix, original name: Class19
internal class SnDHandler  ///WYJ note, the class that handles split and dividend
{
    private AdjustedModeWhenDataRange adjustedModeWhenDataRange_0;
    private DateTime dateTime_0;
    private Enum1 enum1_0;
    private IList<FundamentalItem> splitList;
    private IList<FundamentalItem> dividendList;
    private List<FundamentalItem> splitAndDividendList;
    private List<SnDFactor> factorList;

    public SnDHandler(IList<FundamentalItem> splitList, IList<FundamentalItem> dividendList, Enum1 enum1_1)
    {
        this.dateTime_0 = DateTime.MaxValue;
        this.factorList = new List<SnDFactor>();
        Logger.LogWithStackTrace(new object[] { enum1_1 });
        this.splitList = splitList;
        this.dividendList = dividendList;
        this.enum1_0 = enum1_1;
    }

    public SnDHandler(IList<FundamentalItem> splitList, IList<FundamentalItem> dividendList, Enum1 enum1_1, AdjustedModeWhenDataRange adjustedModeWhenDataRange_1, DateTime dateTime_1) : this(splitList, dividendList, enum1_1)
    {
        this.adjustedModeWhenDataRange_0 = adjustedModeWhenDataRange_1;
        this.dateTime_0 = dateTime_1;
    }

    ///WYJ fix, original name method_0
    private int compare(FundamentalItem fundamentalItem_0, FundamentalItem fundamentalItem_1)
    {
        if (fundamentalItem_0.Date.CompareTo(fundamentalItem_1.Date) != 0)
        {
            return fundamentalItem_0.Date.CompareTo(fundamentalItem_1.Date);
        }
        if (fundamentalItem_0.Name == fundamentalItem_1.Name)
        {
            return 0;
        }
        if (fundamentalItem_0.Name.StartsWith("S"))
        {
            return 1;
        }
        return -1;
    }

    ///WYJ fix, workable backup before making flow change
    /*
    private void method_1()
    {
        this.list_0 = new List<FundamentalItem>();
        this.list_0.AddRange(this.ilist_1);
        this.list_0.AddRange(this.ilist_0);
        this.list_0.Sort(new Comparison<FundamentalItem>(this.compare));
        double num = 1.0;
        for (int i = this.list_0.Count - 1; i >= 0; i--)
        {
            ///WYJ fix, original: if (((this.adjustedModeWhenDataRange_0 == AdjustedModeWhenDataRange.Ignore) || (this.enum1_0 == Enum1.flag_1)) && ((this.list_0[i].Date > this.dateTime_0) || (this.enum1_0 == Enum1.flag_1)))
            if ((this.enum1_0 == Enum1.flag_1) || (this.adjustedModeWhenDataRange_0 == AdjustedModeWhenDataRange.Ignore) && (this.list_0[i].Date > this.dateTime_0))
            {
                if (this.list_0[i].Name.StartsWith("S"))
                {
                    num *= this.list_0[i].Value;
                }
                if ((this.adjustedModeWhenDataRange_0 == AdjustedModeWhenDataRange.Ignore) && (this.list_0[i].Date > this.dateTime_0))
                {
                    this.list_0.RemoveAt(i);
                    continue;
                }
            }
            if (this.list_0[i].Name.StartsWith("D"))
            {
                FundamentalItem local1 = this.list_0[i];
                local1.Value *= num;
            }
        }
    }*/

    ///WYJ fix, original name: method_1
    private void PreprocessSplitAndDividend()  ///WYJ note, looks like method handling split and dividend
    {
        this.splitAndDividendList = new List<FundamentalItem>();
        this.splitAndDividendList.AddRange(this.dividendList);
        this.splitAndDividendList.AddRange(this.splitList);
        this.splitAndDividendList.Sort(new Comparison<FundamentalItem>(this.compare));
        double num = 1.0;
        for (int i = this.splitAndDividendList.Count - 1; i >= 0; i--)
        {
            ///WYJ fix, original: if (((this.adjustedModeWhenDataRange_0 == AdjustedModeWhenDataRange.Ignore) || (this.enum1_0 == Enum1.flag_1)) && ((this.list_0[i].Date > this.dateTime_0) || (this.enum1_0 == Enum1.flag_1)))
            if ((this.enum1_0 == Enum1.flag_1) || (this.adjustedModeWhenDataRange_0 == AdjustedModeWhenDataRange.Ignore) && (this.splitAndDividendList[i].Date > this.dateTime_0))
            {
                if (this.splitAndDividendList[i].Name.StartsWith("S"))
                {
                    num *= this.splitAndDividendList[i].Value;
                }
                if ((this.adjustedModeWhenDataRange_0 == AdjustedModeWhenDataRange.Ignore) && (this.splitAndDividendList[i].Date > this.dateTime_0))
                {
                    this.splitAndDividendList.RemoveAt(i);
                    continue;
                }
            }

            if (this.splitAndDividendList[i].Name.StartsWith("D"))
            {
                FundamentalItem local1 = this.splitAndDividendList[i];
                local1.Value *= num;
            }
        }
    }

    ///WYJ note, this method is never used
    private void method_2(List<FundamentalItem> list_2)
    {
        double num = 1.0;
        for (int i = list_2.Count - 1; i >= 0; i++)
        {
            if (list_2[i].Name.StartsWith("S"))
            {
                num *= this.splitAndDividendList[i].Value;
            }
            if (list_2[i].Name.StartsWith("D"))
            {
                list_2[i].Value *= num;
            }
        }
    }

    public SnDFactor GetFactorForDateTime(DateTime dateTime_1)
    {
        for (int i = 0; i < this.factorList.Count; i++)
        {
            if ((this.factorList[i].DateTime() < dateTime_1) || (i == (this.factorList.Count - 1)))
            {
                if (i == 0)
                {
                    return new SnDFactor(DateTime.MaxValue, 1.0, 1.0);
                }
                return this.factorList[i - 1];
            }
        }
        if (this.factorList.Count == 0)
        {
            return new SnDFactor(DateTime.MaxValue, 1.0, 1.0);
        }
        return null;
    }

    ///WYJ fix, code from Reflector
    /*
    public Bars method_4(Bars bars_0)
    {
        Class21.smethod_7(new object[] { bars_0.Symbol });
        this.method_1();
        this.list_1.Clear();
        Bars bars = new Bars(bars_0.Symbol, bars_0.Scale, bars_0.BarInterval);
        double num = 1.0;
        double num2 = 1.0;
        double num3 = 1.0;
        for (int i = bars_0.Count - 1; i >= 0; i--)
        {
            DateTime time;
            DateTime time5;
            if (this.list_0.Count <= 0)
            {
                //goto Label_023B; ///WYJ fix
                bars.Add(bars_0.Date[i], bars_0.Open[i] * num, bars_0.High[i] * num, bars_0.Low[i] * num, bars_0.Close[i] * num, bars_0.Volume[i] * num2);
                break;
            }
            DateTime time3 = bars_0.Date[i];
            if (time3.Date >= this.list_0[this.list_0.Count - 1].Date.Date)
            {
                //goto Label_023B; ///WYJ fix
                bars.Add(bars_0.Date[i], bars_0.Open[i] * num, bars_0.High[i] * num, bars_0.Low[i] * num, bars_0.Close[i] * num, bars_0.Volume[i] * num2);
                break;
            }
            goto Label_01CD;
        Label_00C9:
            if (this.list_0[this.list_0.Count - 1].Name.StartsWith("D") && ((this.enum1_0 & Enum1.flag_1) != 0))
            {
                double num6 = this.list_0[this.list_0.Count - 1].Value;
                num *= 1.0 - (num6 / (bars_0.Close[i] * num3));
            }
            if (this.list_0[this.list_0.Count - 1].Name.StartsWith("S") && ((this.enum1_0 & Enum1.flag_0) != 0))
            {
                double num7 = 1.0 / this.list_0[this.list_0.Count - 1].Value;
                num *= num7;
                num2 *= num7;
                num3 *= num7;
            }
            this.list_0.RemoveAt(this.list_0.Count - 1);
            if (this.list_0.Count == 0)
            {
                goto Label_0213;
            }
        Label_01CD:
            time = bars_0.Date[i];
            if (time.Date < this.list_0[this.list_0.Count - 1].Date.Date)
            {
                goto Label_00C9;
            }
        Label_0213:
            time5 = bars_0.Date[i];
            this.list_1.Add(new Class20(time5.Date, num, num2));
        Label_023B:
            bars.Add(bars_0.Date[i], bars_0.Open[i] * num, bars_0.High[i] * num, bars_0.Low[i] * num, bars_0.Close[i] * num, bars_0.Volume[i] * num2);
        }
        Bars toBars = new Bars(bars_0.Symbol, bars_0.Scale, bars_0.BarInterval) {
            SecurityName = bars_0.SecurityName
        };
        if (YahooStaticProvider.VersionContainsUserEditedDates())
        {
            YahooStaticProvider.AddUserEditedDates(toBars, bars_0);
        }
        for (int j = bars.Count - 1; j >= 0; j--)
        {
            toBars.Add(bars.Date[j], bars.Open[j], bars.High[j], bars.Low[j], bars.Close[j], bars.Volume[j]);
        }
        return toBars;
    } */

    ///WYJ fix, original name: method_4
    public Bars ProcessSplitAndDividend(Bars bars_0)
    {
        object[] symbol = new object[] { bars_0.Symbol };
        Logger.LogWithStackTrace(symbol);
        this.PreprocessSplitAndDividend();
        this.factorList.Clear();
        Bars bar = new Bars(bars_0.Symbol, bars_0.Scale, bars_0.BarInterval);
        double factor = 1;
        double factorForSplit = 1;
        double num1 = 1;
        for (int i = bars_0.Count - 1; i >= 0; i--)
        {
            if (this.splitAndDividendList.Count > 0)
            {
                DateTime dateTime = bars_0.Date[i];
                DateTime date = this.splitAndDividendList[this.splitAndDividendList.Count - 1].Date;
                if (dateTime.Date < date.Date)
                {
                    do
                    {
                        DateTime item1 = bars_0.Date[i];
                        DateTime date1 = this.splitAndDividendList[this.splitAndDividendList.Count - 1].Date;
                        if (item1.Date >= date1.Date)
                        {
                            break;
                        }
                        if (this.splitAndDividendList[this.splitAndDividendList.Count - 1].Name.StartsWith("D") && (int)(this.enum1_0 & Enum1.flag_1) != 0)
                        {
                            double value = this.splitAndDividendList[this.splitAndDividendList.Count - 1].Value;
                            factor = factor * (1 - value / (bars_0.Close[i] * num1));
                        }
                        if (this.splitAndDividendList[this.splitAndDividendList.Count - 1].Name.StartsWith("S") && (int)(this.enum1_0 & Enum1.flag_0) != 0)
                        {
                            double value1 = 1 / this.splitAndDividendList[this.splitAndDividendList.Count - 1].Value;
                            factor = factor * value1;
                            factorForSplit = factorForSplit * value1;
                            num1 = num1 * value1;
                        }
                        this.splitAndDividendList.RemoveAt(this.splitAndDividendList.Count - 1);
                    }
                    while (this.splitAndDividendList.Count != 0);
                    DateTime dateTime1 = bars_0.Date[i];
                    this.factorList.Add(new SnDFactor(dateTime1.Date, factor, factorForSplit));
                }
            }
            bar.Add(bars_0.Date[i], bars_0.Open[i] * factor, bars_0.High[i] * factor, bars_0.Low[i] * factor, bars_0.Close[i] * factor, bars_0.Volume[i] * factorForSplit);
        }
        Bars barsToReturn = new Bars(bars_0.Symbol, bars_0.Scale, bars_0.BarInterval);
        barsToReturn.SecurityName = bars_0.SecurityName;
        if (YahooStaticProvider.VersionContainsUserEditedDates())
        {
            YahooStaticProvider.AddUserEditedDates(barsToReturn, bars_0);
        }
        for (int j = bar.Count - 1; j >= 0; j--)
        {
            barsToReturn.Add(bar.Date[j], bar.Open[j], bar.High[j], bar.Low[j], bar.Close[j], bar.Volume[j]);
        }
        return barsToReturn;
    }
} 



