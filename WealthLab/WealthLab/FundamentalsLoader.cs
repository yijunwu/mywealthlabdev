namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class FundamentalsLoader : Component, IComparer<FundamentalDataProvider>, IComparer<FundamentalItem>
    {
        private Dictionary<string, FundamentalDataProvider> dictionary_0;
        private Dictionary<string, FundamentalDataProvider> dictionary_1;
        private IContainer icontainer_0;
        private IDataHost idataHost_0;
        private List<FundamentalDataProvider> list_0;

        public FundamentalsLoader()
        {
            this.list_0 = new List<FundamentalDataProvider>();
            this.dictionary_0 = new Dictionary<string, FundamentalDataProvider>();
            this.dictionary_1 = new Dictionary<string, FundamentalDataProvider>();
            this.method_12();
        }

        public FundamentalsLoader(IContainer container)
        {
            this.list_0 = new List<FundamentalDataProvider>();
            this.dictionary_0 = new Dictionary<string, FundamentalDataProvider>();
            this.dictionary_1 = new Dictionary<string, FundamentalDataProvider>();
            container.Add(this);
            this.method_12();
        }

        public int Compare(FundamentalDataProvider fundamentalDataProvider_0, FundamentalDataProvider fundamentalDataProvider_1)
        {
            return fundamentalDataProvider_0.GetType().Name.CompareTo(fundamentalDataProvider_1.GetType().Name);
        }

        public int Compare(FundamentalItem fundamentalItem_0, FundamentalItem fundamentalItem_1)
        {
            return fundamentalItem_0.Name.CompareTo(fundamentalItem_1.Name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        ///WYJ fix, code from Reflector, workable, but deprecated because of having to many goto statements, try using version from ILSpy
        /*
        public List<FundamentalItem> FundamentalItemsOffset(List<FundamentalItem> item1, int offset)
        {
            if ((offset < 1) || (offset > item1.Count))
            {
                return item1;
            }
            List<FundamentalItem> list = new List<FundamentalItem>();
            string name = item1[0].Name;
            string detail = item1[0].GetDetail("period");
            if (((detail == "quarterly") && (item1[0].GetDetail("current quarter") != "")) && (item1[0].GetDetail("fiscal year") != ""))
            {
                for (int j = item1.Count - 1; j >= 0; j--)
                {
                    FundamentalItem item3;
                    int num2 = int.Parse(item1[j].GetDetail("current quarter"));
                    int num3 = int.Parse(item1[j].GetDetail("fiscal year"));
                    int num5 = num2 - (offset % 4);
                    int num4 = num3 - (offset / 4);
                    if (num5 <= 0)
                    {
                        num5 += 4;
                        num4--;
                    }
                    double num9 = 0.0;
                    int num = j - offset;
                    goto Label_0141;
                Label_00FA:
                    if (num < 0)
                    {
                        goto Label_0156;
                    }
                    num2 = int.Parse(item1[num].GetDetail("current quarter"));
                    num3 = int.Parse(item1[num].GetDetail("fiscal year"));
                    if (num4 == num3)
                    {
                        if (num5 == num2)
                        {
                            goto Label_0148;
                        }
                        if (num5 < num2)
                        {
                            goto Label_0156;
                        }
                    }
                    num++;
                Label_0141:
                    if (num >= j)
                    {
                        goto Label_0156;
                    }
                    goto Label_00FA;
                Label_0148:
                    num9 = item1[num].Value;
                Label_0156:
                    item3 = new FundamentalItem(name);
                    this.method_11(item1[j], item3);
                    item3.Value = num9;
                    list.Insert(0, item3);
                }
                return list;
            }
            if ((detail != "") && (detail != "quarterly"))
            {
                for (int k = item1.Count - 1; k >= 0; k--)
                {
                    FundamentalItem item;
                    DateTime time2 = this.method_10(item1[k], offset);
                    double num7 = 0.0;
                    int num11 = k - offset;
                    goto Label_0281;
                Label_01EC:
                    if (num11 < 0)
                    {
                        goto Label_02AC;
                    }
                    DateTime time = this.method_6(item1[num11], false);
                    if (time == time2)
                    {
                        goto Label_028C;
                    }
                    if (detail != "weekly")
                    {
                        if (time.Year == time2.Year)
                        {
                            if ((detail == "annual") || (time.Month == time2.Month))
                            {
                                goto Label_029D;
                            }
                            if (time.Month <= time2.Month)
                            {
                                goto Label_027B;
                            }
                        }
                        else if (time.Year <= time2.Year)
                        {
                            goto Label_027B;
                        }
                        goto Label_02AC;
                    }
                    if (time > time2)
                    {
                        goto Label_02AC;
                    }
                Label_027B:
                    num11++;
                Label_0281:
                    if (num11 >= k)
                    {
                        goto Label_02AC;
                    }
                    goto Label_01EC;
                Label_028C:
                    num7 = item1[num11].Value;
                    goto Label_02AC;
                Label_029D:
                    num7 = item1[num11].Value;
                Label_02AC:
                    item = new FundamentalItem(name);
                    this.method_11(item1[k], item);
                    item.Value = num7;
                    list.Insert(0, item);
                }
                return list;
            }
            for (int i = 0; i < item1.Count; i++)
            {
                FundamentalItem item2 = new FundamentalItem(name);
                this.method_11(item1[i], item2);
                if (i < offset)
                {
                    item2.Value = 0.0;
                }
                else
                {
                    item2.Value = item1[i - offset].Value;
                }
                list.Add(item2);
            }
            return list;
        } */

        // WealthLab.FundamentalsLoader
        public List<FundamentalItem> FundamentalItemsOffset(List<FundamentalItem> item1, int offset)
        {
            if (offset >= 1 && offset <= item1.Count)
            {
                List<FundamentalItem> list = new List<FundamentalItem>();
                string name = item1[0].Name;
                string detail = item1[0].GetDetail("period");
                if (detail == "quarterly" && item1[0].GetDetail("current quarter") != "" && item1[0].GetDetail("fiscal year") != "")
                {
                    for (int i = item1.Count - 1; i >= 0; i--)
                    {
                        int num = int.Parse(item1[i].GetDetail("current quarter"));
                        int num2 = int.Parse(item1[i].GetDetail("fiscal year"));
                        int num3 = num - offset % 4;
                        int num4 = num2 - offset / 4;
                        if (num3 <= 0)
                        {
                            num3 += 4;
                            num4--;
                        }
                        double value = 0.0;
                        int num5 = i - offset;
                        while (num5 < i && num5 >= 0)
                        {
                            num = int.Parse(item1[num5].GetDetail("current quarter"));
                            num2 = int.Parse(item1[num5].GetDetail("fiscal year"));
                            if (num4 == num2)
                            {
                                if (num3 == num)
                                {
                                    value = item1[num5].Value;
                                    break;
                                }
                                if (num3 < num)
                                {
                                    break;
                                }
                            }
                            num5++;
                        }
                        FundamentalItem fundamentalItem = new FundamentalItem(name);
                        this.method_11(item1[i], fundamentalItem);
                        fundamentalItem.Value = value;
                        list.Insert(0, fundamentalItem);
                    }
                }
                else
                {
                    if (detail != "" && detail != "quarterly")
                    {
                        for (int j = item1.Count - 1; j >= 0; j--)
                        {
                            DateTime dateTime = this.method_10(item1[j], offset);
                            double value2 = 0.0;
                            int num6 = j - offset;
                            while (num6 < j && num6 >= 0)
                            {
                                DateTime dateTime2 = this.method_6(item1[num6], false);
                                if (dateTime2 == dateTime)
                                {
                                    value2 = item1[num6].Value;
                                    break;
                                }
                                if (detail != "weekly")
                                {
                                    if (dateTime2.Year == dateTime.Year)
                                    {
                                        if (!(detail == "annual"))
                                        {
                                            if (dateTime2.Month != dateTime.Month)
                                            {
                                                if (dateTime2.Month > dateTime.Month)
                                                {
                                                    break;
                                                }
                                                goto IL_27B;
                                            }
                                        }
                                        value2 = item1[num6].Value;
                                        break;
                                    }
                                    if (dateTime2.Year > dateTime.Year)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    if (dateTime2 > dateTime)
                                    {
                                        break;
                                    }
                                }
                            IL_27B:
                                num6++;
                            }
                            FundamentalItem fundamentalItem2 = new FundamentalItem(name);
                            this.method_11(item1[j], fundamentalItem2);
                            fundamentalItem2.Value = value2;
                            list.Insert(0, fundamentalItem2);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < item1.Count; k++)
                        {
                            FundamentalItem fundamentalItem3 = new FundamentalItem(name);
                            this.method_11(item1[k], fundamentalItem3);
                            if (k < offset)
                            {
                                fundamentalItem3.Value = 0.0;
                            }
                            else
                            {
                                fundamentalItem3.Value = item1[k - offset].Value;
                            }
                            list.Add(fundamentalItem3);
                        }
                    }
                }
                return list;
            }
            return item1;
        }


        private void method_0()
        {
            if (this.idataHost_0 == null)
            {
                throw new InvalidOperationException("DataHost property must be set before accessing FundamentalsLoader");
            }
        }

        private void method_1()
        {
            if (!base.DesignMode && (this.list_0.Count <= 0))
            {
                AssemblyLoader loader = new AssemblyLoader {
                    BaseClass = "FundamentalDataProvider",
                    Path = Path.GetDirectoryName(Application.ExecutablePath)
                };
                foreach (System.Type type in loader.Types)
                {
                    FundamentalDataProvider item = (FundamentalDataProvider) loader.CreateInstance(type);
                    item.Initialize(this.idataHost_0);
                    this.list_0.Add(item);
                    if (item.SymbolSpecificItemsProvided != null)
                    {
                        foreach (string str2 in item.SymbolSpecificItemsProvided)
                        {
                            this.dictionary_0[str2] = item;
                        }
                    }
                    if (item.NonSymbolSpecificItemsProvided != null)
                    {
                        foreach (string str in item.NonSymbolSpecificItemsProvided)
                        {
                            this.dictionary_1[str] = item;
                        }
                    }
                }
                this.list_0.Sort(this);
            }
        }

        private DateTime method_10(FundamentalItem fundamentalItem_0, int int_0)
        {
            DateTime time = this.method_6(fundamentalItem_0, false);
            switch (fundamentalItem_0.GetDetail("period"))
            {
                case "annual":
                    return new DateTime(time.Year - int_0, time.Month, 1);

                case "semi-annual":
                {
                    int year = time.Year - (int_0 / 2);
                    int month = time.Month;
                    if ((int_0 % 2) > 0)
                    {
                        month -= 6;
                        if (month <= 0)
                        {
                            month += 12;
                            year--;
                        }
                    }
                    return new DateTime(year, month, 1);
                }
                case "monthly":
                {
                    int num2 = time.Year - (int_0 / 12);
                    int num3 = time.Month - (int_0 % 12);
                    if (num3 <= 0)
                    {
                        num3 += 12;
                        num2--;
                    }
                    return new DateTime(num2, num3, 1);
                }
                case "weekly":
                    return time.Subtract(new TimeSpan(int_0 * 7, 0, 0, 0));
            }
            return time.Subtract(new TimeSpan(int_0, 0, 0, 0));
        }

        private bool method_11(FundamentalItem fundamentalItem_0, FundamentalItem fundamentalItem_1)
        {
            fundamentalItem_1.Bar = fundamentalItem_0.Bar;
            fundamentalItem_1.Date = fundamentalItem_0.Date;
            fundamentalItem_1.Value = fundamentalItem_0.Value;
            string detail = fundamentalItem_0.GetDetail("period");
            if (detail != "")
            {
                fundamentalItem_1.SetDetail("period", detail);
                if (detail == "quarterly")
                {
                    fundamentalItem_1.SetDetail("current quarter", fundamentalItem_0.GetDetail("current quarter"));
                    fundamentalItem_1.SetDetail("fiscal year", fundamentalItem_0.GetDetail("fiscal year"));
                }
                else if (fundamentalItem_0.GetDetail("observation date") != "")
                {
                    fundamentalItem_1.SetDetail("observation date", fundamentalItem_0.GetDetail("observation date"));
                }
            }
            return true;
        }

        private void method_12()
        {
            this.icontainer_0 = new Container();
        }

        private void method_2(Bars bars_0, IList<FundamentalItem> ilist_0)
        {
            if (bars_0.Count == 0)
            {
                foreach (FundamentalItem item in ilist_0)
                {
                    item.Bar = -1;
                }
            }
            else
            {
                foreach (FundamentalItem item2 in ilist_0)
                {
                    item2.Bar = bars_0.ConvertDateToBar(item2.Date, false);
                    if ((item2.Bar == 0) && (item2.Date < bars_0.Date[0]))
                    {
                        item2.Bar = -1;
                    }
                }
            }
        }

        private DataSeries method_3(Bars bars_0, IList<FundamentalItem> ilist_0, string string_0)
        {
            int index;
            int num3;
            if (ilist_0 == null)
            {
                return null;
            }
            FundamentalItem item = null;
            DataSeries series = new DataSeries(bars_0, string_0);
            foreach (FundamentalItem item2 in ilist_0)
            {
                if (item2.Bar >= 0)
                {
                    if (item == null)
                    {
                        item = item2;
                    }
                    index = ilist_0.IndexOf(item2);
                    if (index < (ilist_0.Count - 1))
                    {
                        if (ilist_0[index + 1].Bar == -1)
                        {
                            num3 = bars_0.Count - 1;
                        }
                        else
                        {
                            num3 = ilist_0[index + 1].Bar - 1;
                        }
                    }
                    else
                    {
                        num3 = bars_0.Count - 1;
                    }
                    for (int i = item2.Bar; i <= num3; i++)
                    {
                        series[i] = item2.Value;
                    }
                }
            }
            if ((item != null) && (item.Bar > 0))
            {
                index = ilist_0.IndexOf(item);
                if (index <= 0)
                {
                    return series;
                }
                num3 = item.Bar - 1;
                item = ilist_0[index - 1];
                for (int j = 0; j <= num3; j++)
                {
                    series[j] = item.Value;
                }
            }
            return series;
        }

        private bool method_4(Bars bars_0, IList<FundamentalItem> ilist_0, int int_0)
        {
            FundamentalItem item = ilist_0[int_0];
            bool flag = false;
            if (item.GetDetail("current quarter") != "")
            {
                return (int.Parse(item.GetDetail("current quarter")) == 4);
            }
            if (item.GetDetail("period") == "weekly")
            {
                if (item.GetDetail("observation date") != "")
                {
                    DateTime time14 = this.method_6(item, true);
                    if (time14 != DateTime.FromOADate(0.0))
                    {
                        DateTime time16 = time14.AddDays(7.0);
                        flag = time14.Year != time16.Year;
                    }
                    return flag;
                }
                DateTime time7 = item.Date.AddDays(7.0);
                return (item.Date.Year != time7.Year);
            }
            if (item.GetDetail("observation date") != "")
            {
                return (this.method_8(item) == 12);
            }
            bool flag2 = false;
            if ((this.dictionary_0.ContainsKey(item.Name) && this.dictionary_0.ContainsKey("earnings per share")) && (item.Name != "earnings per share"))
            {
                IList<FundamentalItem> list = this.RequestSymbolItems(bars_0, bars_0.Symbol, "earnings per share");
                int month = 0;
                int num = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].GetDetail("current quarter") == "1")
                    {
                        month = list[i].Date.Month;
                    }
                    else if (list[i].GetDetail("current quarter") == "4")
                    {
                        num = list[i].Date.Month;
                    }
                    if ((month != 0) && (num != 0))
                    {
                        if (month < num)
                        {
                            flag = ((item.Date.Month < num) || (item.Date.Month > 12)) ? ((item.Date.Month >= 1) && (item.Date.Month < month)) : true;
                        }
                        else
                        {
                            flag = (item.Date.Month >= num) && (item.Date.Month < month);
                        }
                        flag2 = true;
                        break;
                    }
                }
            }
            if (flag2)
            {
                return flag;
            }
            if (int_0 < (ilist_0.Count - 1))
            {
                return (item.Date.Year < ilist_0[int_0 + 1].Date.Year);
            }
            return (item.Date.Month == 12);
        }

        private int method_5(FundamentalItem fundamentalItem_0)
        {
            if (fundamentalItem_0.GetDetail("fiscal year") != "")
            {
                return int.Parse(fundamentalItem_0.GetDetail("fiscal year"));
            }
            if (fundamentalItem_0.GetDetail("observation date") != "")
            {
                return this.method_7(fundamentalItem_0);
            }
            return fundamentalItem_0.Date.Year;
        }

        private DateTime method_6(FundamentalItem fundamentalItem_0, bool bool_0)
        {
            string detail = fundamentalItem_0.GetDetail("observation date");
            if (detail != "")
            {
                return DateTime.Parse(detail);
            }
            if (bool_0)
            {
                return DateTime.FromOADate(0.0);
            }
            return fundamentalItem_0.Date;
        }

        private int method_7(FundamentalItem fundamentalItem_0)
        {
            int year = 0;
            string detail = fundamentalItem_0.GetDetail("observation date");
            if (detail != "")
            {
                year = DateTime.Parse(detail).Year;
            }
            return year;
        }

        private int method_8(FundamentalItem fundamentalItem_0)
        {
            int month = 0;
            string detail = fundamentalItem_0.GetDetail("observation date");
            if (detail != "")
            {
                month = DateTime.Parse(detail).Month;
            }
            return month;
        }

        private int method_9(FundamentalItem fundamentalItem_0)
        {
            int num = 0;
            string detail = fundamentalItem_0.GetDetail("observation date");
            if (detail != "")
            {
                DateTime time = DateTime.Parse(detail);
                num = time.DayOfYear / 7;
                int num1 = time.DayOfYear % 7;
            }
            return num;
        }

        ///WYJ fix, code from Reflector, workable, but deprecated because of having too many goto statements. Try using version from ILSpy
        /*
        public DataSeries RequestDataSeries(Bars bars, string itemName, int offset, int aggregate, bool average)
        {
            this.method_0();
            IList<FundamentalItem> list = this.RequestNonSymbolItems(bars, itemName);
            if ((list == null) || ((aggregate + offset) > list.Count))
            {
                return this.method_3(bars, list, itemName);
            }
            string str = "";
            List<FundamentalItem> list2 = new List<FundamentalItem>();
            if (aggregate < 2)
            {
                foreach (FundamentalItem item4 in list)
                {
                    list2.Add(item4);
                }
            }
            else
            {
                string detail = list[0].GetDetail("period");
                if (((detail == "quarterly") && (list[0].GetDetail("current quarter") != "")) && (list[0].GetDetail("fiscal year") != ""))
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        double num12;
                        int num2 = int.Parse(list[i].GetDetail("current quarter"));
                        int num4 = int.Parse(list[i].GetDetail("fiscal year"));
                        int num = num2 - (aggregate % 4);
                        int num5 = num4 - (aggregate / 4);
                        if (num <= 0)
                        {
                            num += 4;
                            num5--;
                        }
                        int num10 = i - aggregate;
                        if (num10 == -1)
                        {
                            num10 = 0;
                        }
                        goto Label_019A;
                    Label_0143:
                        if (num10 < 0)
                        {
                            goto Label_01A8;
                        }
                        num2 = int.Parse(list[num10].GetDetail("current quarter"));
                        num4 = int.Parse(list[num10].GetDetail("fiscal year"));
                        if (num5 == num4)
                        {
                            if (num == num2)
                            {
                                goto Label_01A2;
                            }
                            if (num >= num2)
                            {
                                goto Label_0194;
                            }
                            goto Label_01A8;
                        }
                        if (num5 < num4)
                        {
                            goto Label_01A8;
                        }
                    Label_0194:
                        num10++;
                    Label_019A:
                        if (num10 >= i)
                        {
                            goto Label_01A8;
                        }
                        goto Label_0143;
                    Label_01A2:
                        num10++;
                    Label_01A8:
                        num12 = 0.0;
                        int num13 = 0;
                        while (num10 <= i)
                        {
                            if (num10 < 0)
                            {
                                break;
                            }
                            num12 += list[num10++].Value;
                            num13++;
                        }
                        FundamentalItem item3 = new FundamentalItem(itemName);
                        this.method_11(list[i], item3);
                        if (average && (num13 > 0))
                        {
                            item3.Value = num12 / ((double) num13);
                        }
                        else
                        {
                            item3.Value = num12;
                        }
                        list2.Insert(0, item3);
                    }
                }
                else if ((detail != "") && (detail != "quarterly"))
                {
                    for (int j = list.Count - 1; j >= 0; j--)
                    {
                        double num14;
                        DateTime time = this.method_6(list[j], false);
                        DateTime time2 = this.method_10(list[j], aggregate);
                        int num6 = j - aggregate;
                        if (num6 == -1)
                        {
                            num6 = 0;
                        }
                        goto Label_0332;
                    Label_02A0:
                        if (num6 < 0)
                        {
                            goto Label_034B;
                        }
                        time = this.method_6(list[num6], false);
                        if (time == time2)
                        {
                            goto Label_033D;
                        }
                        if (detail != "weekly")
                        {
                            if (time2.Year == time.Year)
                            {
                                if ((detail == "annual") || (time2.Month == time.Month))
                                {
                                    goto Label_0345;
                                }
                                if (time.Month <= time2.Month)
                                {
                                    goto Label_032C;
                                }
                            }
                            else if (time.Year <= time2.Year)
                            {
                                goto Label_032C;
                            }
                            goto Label_034B;
                        }
                        if (time > time2)
                        {
                            goto Label_034B;
                        }
                    Label_032C:
                        num6++;
                    Label_0332:
                        if (num6 >= j)
                        {
                            goto Label_034B;
                        }
                        goto Label_02A0;
                    Label_033D:
                        num6++;
                        goto Label_034B;
                    Label_0345:
                        num6++;
                    Label_034B:
                        num14 = 0.0;
                        int num16 = 0;
                        while (num6 <= j)
                        {
                            if (num6 < 0)
                            {
                                break;
                            }
                            num14 += list[num6++].Value;
                            num16++;
                        }
                        FundamentalItem item2 = new FundamentalItem(itemName);
                        this.method_11(list[j], item2);
                        if (average && (num16 > 0))
                        {
                            item2.Value = num14 / ((double) num16);
                        }
                        else
                        {
                            item2.Value = num14;
                        }
                        list2.Insert(0, item2);
                    }
                }
                else
                {
                    for (int k = 0; k < list.Count; k++)
                    {
                        FundamentalItem item = new FundamentalItem(itemName);
                        this.method_11(list[k], item);
                        if (k < (aggregate - 1))
                        {
                            item.Value = 0.0;
                        }
                        else
                        {
                            int num15 = (k - aggregate) + 1;
                            double num8 = 0.0;
                            int num9 = 0;
                            while (num15 <= k)
                            {
                                num8 += list[num15++].Value;
                                num9++;
                            }
                            if (average && (num9 > 0))
                            {
                                item.Value = num8 / ((double) num9);
                            }
                            else
                            {
                                item.Value = num8;
                            }
                        }
                        list2.Add(item);
                    }
                }
                if (average)
                {
                    str = ":Avg(" + aggregate.ToString() + ")";
                }
                else
                {
                    str = ":Sum(" + aggregate.ToString() + ")";
                }
            }
            string str3 = "";
            if (offset > 0)
            {
                list2 = this.FundamentalItemsOffset(list2, offset);
                str3 = ":Offset(" + offset.ToString() + ")";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(str);
            builder.Append(str3);
            return this.method_3(bars, list2, itemName + builder.ToString());
        } */

        ///WYJ fix, code from ILSpy
        public DataSeries RequestDataSeries(Bars bars, string itemName, int offset, int aggregate, bool average)
        {
            this.method_0();
            IList<FundamentalItem> list = this.RequestNonSymbolItems(bars, itemName);
            if (list != null && aggregate + offset <= list.Count)
            {
                string value = "";
                List<FundamentalItem> list2 = new List<FundamentalItem>();
                if (aggregate < 2)
                {
                    using (IEnumerator<FundamentalItem> enumerator = list.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            FundamentalItem current = enumerator.Current;
                            list2.Add(current);
                        }
                        goto IL_4C4;
                    }
                }
                string detail = list[0].GetDetail("period");
                if (detail == "quarterly" && list[0].GetDetail("current quarter") != "" && list[0].GetDetail("fiscal year") != "")
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        int num = int.Parse(list[i].GetDetail("current quarter"));
                        int num2 = int.Parse(list[i].GetDetail("fiscal year"));
                        int num3 = num - aggregate % 4;
                        int num4 = num2 - aggregate / 4;
                        if (num3 <= 0)
                        {
                            num3 += 4;
                            num4--;
                        }
                        int num5 = i - aggregate;
                        if (num5 == -1)
                        {
                            num5 = 0;
                        }
                        while (num5 < i && num5 >= 0)
                        {
                            num = int.Parse(list[num5].GetDetail("current quarter"));
                            num2 = int.Parse(list[num5].GetDetail("fiscal year"));
                            if (num4 == num2)
                            {
                                if (num3 == num)
                                {
                                    num5++;
                                    break;
                                }
                                if (num3 < num)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (num4 < num2)
                                {
                                    break;
                                }
                            }
                            num5++;
                        }
                        double num6 = 0.0;
                        int num7 = 0;
                        while (num5 <= i && num5 >= 0)
                        {
                            num6 += list[num5++].Value;
                            num7++;
                        }
                        FundamentalItem fundamentalItem = new FundamentalItem(itemName);
                        this.method_11(list[i], fundamentalItem);
                        if (average && num7 > 0)
                        {
                            fundamentalItem.Value = num6 / (double)num7;
                        }
                        else
                        {
                            fundamentalItem.Value = num6;
                        }
                        list2.Insert(0, fundamentalItem);
                    }
                }
                else
                {
                    if (detail != "" && detail != "quarterly")
                    {
                        for (int j = list.Count - 1; j >= 0; j--)
                        {
                            DateTime dateTime = this.method_6(list[j], false);
                            DateTime dateTime2 = this.method_10(list[j], aggregate);
                            int num8 = j - aggregate;
                            if (num8 == -1)
                            {
                                num8 = 0;
                            }
                            while (num8 < j && num8 >= 0)
                            {
                                dateTime = this.method_6(list[num8], false);
                                if (dateTime == dateTime2)
                                {
                                    num8++;
                                    break;
                                }
                                if (detail != "weekly")
                                {
                                    if (dateTime2.Year == dateTime.Year)
                                    {
                                        if (!(detail == "annual"))
                                        {
                                            if (dateTime2.Month != dateTime.Month)
                                            {
                                                if (dateTime.Month > dateTime2.Month)
                                                {
                                                    break;
                                                }
                                                goto IL_32C;
                                            }
                                        }
                                        num8++;
                                        break;
                                    }
                                    if (dateTime.Year > dateTime2.Year)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    if (dateTime > dateTime2)
                                    {
                                        break;
                                    }
                                }
                            IL_32C:
                                num8++;
                            }
                            double num9 = 0.0;
                            int num10 = 0;
                            while (num8 <= j && num8 >= 0)
                            {
                                num9 += list[num8++].Value;
                                num10++;
                            }
                            FundamentalItem fundamentalItem2 = new FundamentalItem(itemName);
                            this.method_11(list[j], fundamentalItem2);
                            if (average && num10 > 0)
                            {
                                fundamentalItem2.Value = num9 / (double)num10;
                            }
                            else
                            {
                                fundamentalItem2.Value = num9;
                            }
                            list2.Insert(0, fundamentalItem2);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < list.Count; k++)
                        {
                            FundamentalItem fundamentalItem3 = new FundamentalItem(itemName);
                            this.method_11(list[k], fundamentalItem3);
                            if (k < aggregate - 1)
                            {
                                fundamentalItem3.Value = 0.0;
                            }
                            else
                            {
                                int l = k - aggregate + 1;
                                double num11 = 0.0;
                                int num12 = 0;
                                while (l <= k)
                                {
                                    num11 += list[l++].Value;
                                    num12++;
                                }
                                if (average && num12 > 0)
                                {
                                    fundamentalItem3.Value = num11 / (double)num12;
                                }
                                else
                                {
                                    fundamentalItem3.Value = num11;
                                }
                            }
                            list2.Add(fundamentalItem3);
                        }
                    }
                }
                if (average)
                {
                    value = ":Avg(" + aggregate.ToString() + ")";
                }
                else
                {
                    value = ":Sum(" + aggregate.ToString() + ")";
                }
            IL_4C4:
                string value2 = "";
                if (offset > 0)
                {
                    List<FundamentalItem> list3 = this.FundamentalItemsOffset(list2, offset);
                    list2 = list3;
                    value2 = ":Offset(" + offset.ToString() + ")";
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(value);
                stringBuilder.Append(value2);
                return this.method_3(bars, list2, itemName + stringBuilder.ToString());
            }
            return this.method_3(bars, list, itemName);
        }

        ///WYJ fix, code from Reflector, workable, but deprecated because of having too many goto statements. Try version from ILSpy
        /*
        public DataSeries RequestDataSeriesAnnual(Bars bars, string itemName, int offset)
        {
            this.method_0();
            IList<FundamentalItem> list = this.RequestNonSymbolItems(bars, itemName);
            if ((list == null) || (offset > list.Count))
            {
                return this.method_3(bars, list, itemName);
            }
            List<FundamentalItem> list2 = new List<FundamentalItem>();
            double num5 = 0.0;
            int num6 = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                FundamentalItem item;
                int num = this.method_5(list[i]);
                if (num == 0)
                {
                    num5 = 0.0;
                    goto Label_00DE;
                }
                if ((num == num6) || this.method_4(bars, list, i))
                {
                    goto Label_00DE;
                }
                num6 = num;
                num5 = 0.0;
                int num2 = num6 - (offset + 1);
                int num3 = i;
                goto Label_00A5;
            Label_008F:
                if (num <= num2)
                {
                    goto Label_00D8;
                }
                num = this.method_5(list[--num3]);
            Label_00A5:
                if ((num3 - 1) < 0)
                {
                    goto Label_00D8;
                }
                goto Label_008F;
            Label_00AD:
                if (num3 < 0)
                {
                    goto Label_00DE;
                }
                num5 += list[num3].Value;
                if (--num3 >= 0)
                {
                    num = this.method_5(list[num3]);
                }
            Label_00D8:
                if (num == num2)
                {
                    goto Label_00AD;
                }
            Label_00DE:
                item = new FundamentalItem(itemName);
                this.method_11(list[i], item);
                item.Value = num5;
                list2.Insert(0, item);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("(Annual");
            if (offset > 0)
            {
                builder.Append("-");
                builder.Append(offset.ToString());
            }
            builder.Append(")");
            return this.method_3(bars, list2, itemName + builder.ToString());
        } */

        ///WYJ fix, code from ILSpy
        public DataSeries RequestDataSeriesAnnual(Bars bars, string itemName, int offset)
        {
            this.method_0();
            IList<FundamentalItem> list = this.RequestNonSymbolItems(bars, itemName);
            if (list != null && offset <= list.Count)
            {
                List<FundamentalItem> list2 = new List<FundamentalItem>();
                double num = 0.0;
                int num2 = 0;
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    int num3 = this.method_5(list[i]);
                    if (num3 == 0)
                    {
                        num = 0.0;
                    }
                    else
                    {
                        if (num3 != num2 && !this.method_4(bars, list, i))
                        {
                            num2 = num3;
                            num = 0.0;
                            int num4 = num2 - (offset + 1);
                            int num5 = i;
                            while (num5 - 1 >= 0 && num3 > num4)
                            {
                                num3 = this.method_5(list[--num5]);
                            }
                            while (num3 == num4 && num5 >= 0)
                            {
                                num += list[num5].Value;
                                if (--num5 >= 0)
                                {
                                    num3 = this.method_5(list[num5]);
                                }
                            }
                        }
                    }
                    FundamentalItem fundamentalItem = new FundamentalItem(itemName);
                    this.method_11(list[i], fundamentalItem);
                    fundamentalItem.Value = num;
                    list2.Insert(0, fundamentalItem);
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("(Annual");
                if (offset > 0)
                {
                    stringBuilder.Append("-");
                    stringBuilder.Append(offset.ToString());
                }
                stringBuilder.Append(")");
                return this.method_3(bars, list2, itemName + stringBuilder.ToString());
            }
            return this.method_3(bars, list, itemName);
        }


        public DataSeries RequestNonSymbolDataSeries(Bars bars, string itemName)
        {
            this.method_0();
            IList<FundamentalItem> list = this.RequestNonSymbolItems(bars, itemName);
            return this.method_3(bars, list, itemName);
        }

        public IList<FundamentalItem> RequestNonSymbolItems(Bars bars, string itemName)
        {
            this.method_0();
            if (this.dictionary_1.ContainsKey(itemName))
            {
                IList<FundamentalItem> list = this.dictionary_1[itemName].RequestItems(itemName);
                if (bars != null)
                {
                    this.method_2(bars, list);
                }
                return list;
            }
            if (this.dictionary_0.ContainsKey(itemName))
            {
                return this.RequestSymbolItems(bars, bars.Symbol, itemName);
            }
            return null;
        }

        public DataSeries RequestSymbolDataSeries(Bars bars, string symbol, string itemName)
        {
            this.method_0();
            IList<FundamentalItem> list = this.RequestSymbolItems(bars, symbol, itemName);
            return this.method_3(bars, list, itemName + "(" + symbol + ")");
        }

        public IList<FundamentalItem> RequestSymbolItems(Bars bars, string symbol, string itemName)
        {
            this.method_0();
            if (!this.dictionary_0.ContainsKey(itemName))
            {
                return null;
            }
            IList<FundamentalItem> list = this.dictionary_0[itemName].RequestItems(symbol, itemName);
            if ((bars != null) && (list != null))
            {
                this.method_2(bars, list);
            }
            return list;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<FundamentalItem> ChartableItems
        {
            get
            {
                List<FundamentalItem> list = new List<FundamentalItem>();
                foreach (FundamentalDataProvider provider in this.Providers)
                {
                    IList<string> chartableItems = provider.ChartableItems;
                    if (chartableItems != null)
                    {
                        foreach (string str in chartableItems)
                        {
                            FundamentalItem item = provider.CreateItem(str);
                            list.Add(item);
                        }
                    }
                }
                list.Sort(this);
                return list;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDataHost DataHost
        {
            get
            {
                return this.idataHost_0;
            }
            set
            {
                this.idataHost_0 = value;
                if (this.idataHost_0 != null)
                {
                    this.method_1();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool HasDragDropFundamentals
        {
            get
            {
                bool flag;
                using (List<FundamentalDataProvider>.Enumerator enumerator = this.list_0.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        FundamentalDataProvider current = enumerator.Current;
                        if (current.HasDragDropItems)
                        {
                            //goto  Label_002A;  ///WYJ fix, simplify the flow
                            flag = true;
                            return flag;
                        }
                    }
                    return false;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, FundamentalDataProvider> NonSymbolSpecificItemProvider
        {
            get
            {
                return this.dictionary_1;
            }
            set
            {
                this.dictionary_1 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IList<FundamentalDataProvider> Providers
        {
            get
            {
                return this.list_0;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Dictionary<string, FundamentalDataProvider> SymbolSpecificItemProvider
        {
            get
            {
                return this.dictionary_0;
            }
            set
            {
                this.dictionary_0 = value;
            }
        }
    }
}

