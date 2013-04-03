using System;
using System.Collections.Generic;
using WealthLab;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.Yahoo;

internal class Class19
{
    private AdjustedModeWhenDataRange adjustedModeWhenDataRange_0;
    private DateTime dateTime_0;
    private Enum1 enum1_0;
    private IList<FundamentalItem> ilist_0;
    private IList<FundamentalItem> ilist_1;
    private List<FundamentalItem> list_0;
    private List<Class20> list_1;

    public Class19(IList<FundamentalItem> ilist_2, IList<FundamentalItem> ilist_3, Enum1 enum1_1)
    {
        this.dateTime_0 = DateTime.MaxValue;
        this.list_1 = new List<Class20>();
        Class21.smethod_7(new object[] { enum1_1 });
        this.ilist_0 = ilist_2;
        this.ilist_1 = ilist_3;
        this.enum1_0 = enum1_1;
    }

    public Class19(IList<FundamentalItem> ilist_2, IList<FundamentalItem> ilist_3, Enum1 enum1_1, AdjustedModeWhenDataRange adjustedModeWhenDataRange_1, DateTime dateTime_1) : this(ilist_2, ilist_3, enum1_1)
    {
        this.adjustedModeWhenDataRange_0 = adjustedModeWhenDataRange_1;
        this.dateTime_0 = dateTime_1;
    }

    private int method_0(FundamentalItem fundamentalItem_0, FundamentalItem fundamentalItem_1)
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

    private void method_1()
    {
        this.list_0 = new List<FundamentalItem>();
        this.list_0.AddRange(this.ilist_1);
        this.list_0.AddRange(this.ilist_0);
        this.list_0.Sort(new Comparison<FundamentalItem>(this.method_0));
        double num = 1.0;
        for (int i = this.list_0.Count - 1; i >= 0; i--)
        {
            if (((this.adjustedModeWhenDataRange_0 == AdjustedModeWhenDataRange.Ignore) || (this.enum1_0 == Enum1.flag_1)) && ((this.list_0[i].Date > this.dateTime_0) || (this.enum1_0 == Enum1.flag_1)))
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
    }

    private void method_2(List<FundamentalItem> list_2)
    {
        double num = 1.0;
        for (int i = list_2.Count - 1; i >= 0; i++)
        {
            if (list_2[i].Name.StartsWith("S"))
            {
                num *= this.list_0[i].Value;
            }
            if (list_2[i].Name.StartsWith("D"))
            {
                list_2[i].Value *= num;
            }
        }
    }

    public Class20 method_3(DateTime dateTime_1)
    {
        for (int i = 0; i < this.list_1.Count; i++)
        {
            if ((this.list_1[i].method_0() < dateTime_1) || (i == (this.list_1.Count - 1)))
            {
                if (i == 0)
                {
                    return new Class20(DateTime.MaxValue, 1.0, 1.0);
                }
                return this.list_1[i - 1];
            }
        }
        if (this.list_1.Count == 0)
        {
            return new Class20(DateTime.MaxValue, 1.0, 1.0);
        }
        return null;
    }

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
                goto Label_023B;
            }
            DateTime time3 = bars_0.Date[i];
            if (time3.Date >= this.list_0[this.list_0.Count - 1].Date.Date)
            {
                goto Label_023B;
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
    }
}

