namespace WealthLab.Rules
{
    using System;
    using System.Collections.Generic;
    using WealthLab;

    public class FundamentalsRatio
    {
        private const string string_0 = "common shares outstanding";
        private const string string_1 = "employee";
        private const string string_2 = "common shares used to calculate eps diluted";
        private const string string_3 = "sales turnover";

        public static DateTime BarFundamentalItemDate(WealthScript _ws, string _name, int _bar)
        {
            IList<FundamentalItem> list = _ws.FundamentalDataItems(_name);
            if (_bar >= 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (_bar <= list[i].Bar)
                    {
                        return list[i].Date;
                    }
                }
            }
            return list[list.Count - 1].Date;
        }

        public static DataSeries ConsecutiveGrowthSeries(WealthScript wealthScript_0, string _name, int _periods, double _target, bool _above, bool _annual)
        {
            IList<FundamentalItem> list = wealthScript_0.FundamentalDataItems(_name);
            if (list.Count == 0)
            {
                return new DataSeries(_name + "CGS");
            }
            int num3 = 0;
            DateTime time = smethod_0(list, ref num3, _annual);
            int num2 = 0;
            DataSeries series = GrowthRateSeries(wealthScript_0, _name, _periods, _annual);
            for (int i = 0; i < series.Count; i++)
            {
                if (((wealthScript_0.Date[i] >= time) && (num3 >= 0)) && (num3 < list.Count))
                {
                    time = smethod_0(list, ref num3, _annual);
                    if (_above)
                    {
                        if (series[i] >= _target)
                        {
                            num2++;
                        }
                        else
                        {
                            num2 = 0;
                        }
                    }
                    else if (series[i] <= _target)
                    {
                        num2++;
                    }
                    else
                    {
                        num2 = 0;
                    }
                }
                series[i] = num2;
            }
            return series;
        }

        public static string ConvertName(string name)
        {
            if (name == "Assets")
            {
                return "assets";
            }
            if (name == "Current Assets")
            {
                return "current assets";
            }
            if ((name == "Book Value") || (name == "Equity"))
            {
                return "stockholder equity";
            }
            if ((name == "Income") || (name == "Earnings"))
            {
                return "net income";
            }
            if (name == "Sales")
            {
                return "sales turnover";
            }
            if (name == "Total Inventories")
            {
                return "total inventories";
            }
            if (name == "Total Receivables")
            {
                return "total receivables";
            }
            if (name == "Cash Flow")
            {
                return "operating activities";
            }
            if (name == "Dividend")
            {
                return "cash dividends";
            }
            if (name == "Shares")
            {
                return "common shares outstanding";
            }
            return name;
        }

        public static DataSeries DilutedSharesSeries(WealthScript wealthScript_0)
        {
            return (wealthScript_0.FundamentalDataSeries("common shares used to calculate eps diluted", 4, true, 0) * smethod_2(wealthScript_0));
        }

        public static DataSeries EnterpriseValueSeries(WealthScript wealthScript_0)
        {
            DataSeries series = wealthScript_0.FundamentalDataSeries("long term debt", 4, true, 0);
            DataSeries series2 = wealthScript_0.FundamentalDataSeries("cash", 4, true, 0);
            DataSeries series3 = ShareSeries(wealthScript_0) * wealthScript_0.Close;
            series3 += series;
            return (series3 - series2);
        }

        public static DateTime FundamentalItemDate(WealthScript _ws, string _name)
        {
            return _ws.FundamentalDataItems(_name)[0].Date;
        }

        public static DateTime FundamentalItemDate(WealthScript _ws, string _name, int _cnt)
        {
            IList<FundamentalItem> list = _ws.FundamentalDataItems(_name);
            if (_cnt >= 0)
            {
                return list[_cnt].Date;
            }
            return list[list.Count - 1].Date;
        }

        public static DataSeries FundamentalSeries(WealthScript wealthScript_0, string itemName, bool annual)
        {
            if (annual)
            {
                if (itemName == "common shares used to calculate eps diluted")
                {
                    return DilutedSharesSeries(wealthScript_0);
                }
                if (smethod_3(itemName))
                {
                    return wealthScript_0.FundamentalDataSeries(itemName, 4, true, 0);
                }
                if (smethod_4(itemName))
                {
                    return wealthScript_0.FundamentalDataSeries(itemName, 4, false, 0);
                }
                if (itemName == "employee")
                {
                    return (DataSeries) (wealthScript_0.FundamentalDataSeries(itemName) * 1000.0);
                }
                return wealthScript_0.FundamentalDataSeries(itemName);
            }
            if (itemName == "common shares outstanding")
            {
                return ShareSeries(wealthScript_0);
            }
            return wealthScript_0.FundamentalDataSeries(itemName);
        }

        public static DataSeries GrowthRateSeries(WealthScript wealthScript_0, string itemName, int periods, bool annual)
        {
            DataSeries series;
            DataSeries series2;
            if (annual)
            {
                series = wealthScript_0.FundamentalDataSeriesAnnual(itemName, 0);
                series2 = wealthScript_0.FundamentalDataSeriesAnnual(itemName, periods);
            }
            else
            {
                series = wealthScript_0.FundamentalDataSeries(itemName);
                series2 = wealthScript_0.FundamentalDataSeries(itemName, 0, false, periods);
            }
            DataSeries series3 = series - series2;
            series3 /= DataSeries.Abs(series2);
            return (DataSeries) (series3 * 100.0);
        }

        public static DataSeries MarketCapSeries(WealthScript wealthScript_0)
        {
            return (wealthScript_0.Close * ShareSeries(wealthScript_0));
        }

        public static DataSeries PerEmployeeSeries(WealthScript wealthScript_0, string itemName)
        {
            DataSeries series = FundamentalSeries(wealthScript_0, itemName, true);
            DataSeries series2 = wealthScript_0.FundamentalDataSeries("employee");
            return (series / series2);
        }

        public static DataSeries PerShareSeries(WealthScript wealthScript_0, string itemName, bool annual)
        {
            DataSeries series;
            if (annual)
            {
                series = wealthScript_0.FundamentalDataSeries(itemName, 4, true, 0);
            }
            else
            {
                series = wealthScript_0.FundamentalDataSeries(itemName);
            }
            return (series / ShareSeries(wealthScript_0));
        }

        public static DataSeries PriceToRatioSeries(WealthScript wealthScript_0, string itemName)
        {
            DataSeries series = wealthScript_0.FundamentalDataSeries(itemName, 4, false, 0) / ShareSeries(wealthScript_0);
            return (wealthScript_0.Close / series);
        }

        public static DataSeries RatioSeries(WealthScript wealthScript_0, string itemName1, string itemName2, bool annual)
        {
            if (annual)
            {
                return (wealthScript_0.FundamentalDataSeries(itemName1, 4, false, 0) / wealthScript_0.FundamentalDataSeries(itemName2, 4, false, 0));
            }
            return (wealthScript_0.FundamentalDataSeries(itemName1) / wealthScript_0.FundamentalDataSeries(itemName2));
        }

        public static DataSeries ReturnOnCapitalSeries(WealthScript wealthScript_0)
        {
            DataSeries series = wealthScript_0.FundamentalDataSeries("total inventories", 4, true, 0) + wealthScript_0.FundamentalDataSeries("total receivables", 4, true, 0);
            series -= wealthScript_0.FundamentalDataSeries("liabilities", 4, true, 0);
            series += wealthScript_0.FundamentalDataSeries("assets", 4, true, 0);
            return (DataSeries) ((wealthScript_0.FundamentalDataSeries("ebit") / series) * 100.0);
        }

        public static DataSeries ReturnOnSeries(WealthScript wealthScript_0, string itemName, bool annual)
        {
            DataSeries series;
            if (annual)
            {
                series = wealthScript_0.FundamentalDataSeries(itemName, 4, true, 0);
            }
            else
            {
                series = wealthScript_0.FundamentalDataSeries(itemName);
            }
            return (DataSeries) ((wealthScript_0.FundamentalDataSeries("net income", 4, false, 0) / series) * 100.0);
        }

        public static DataSeries SalesPerEmployeeSeries(WealthScript wealthScript_0)
        {
            return PerEmployeeSeries(wealthScript_0, "sales turnover");
        }

        public static DataSeries ShareSeries(WealthScript wealthScript_0)
        {
            return (wealthScript_0.FundamentalDataSeries("common shares outstanding") * smethod_2(wealthScript_0));
        }

        private static DateTime smethod_0(IList<FundamentalItem> ilist_0, ref int int_0, bool bool_0)
        {
            if (++int_0 >= ilist_0.Count)
            {
                int_0 = -1;
                return DateTime.FromOADate(0.0);
            }
            if (bool_0)
            {
                while (!smethod_1(ilist_0, int_0))
                {
                    if (++int_0 >= ilist_0.Count)
                    {
                        int_0 = -1;
                        return DateTime.FromOADate(0.0);
                    }
                }
            }
            return ilist_0[int_0].Date;
        }

        private static bool smethod_1(IList<FundamentalItem> ilist_0, int int_0)
        {
            FundamentalItem item = ilist_0[int_0];
            string detail = item.GetDetail("period");
            if (detail != "")
            {
                if (item.GetDetail("current quarter") != "")
                {
                    return (int.Parse(item.GetDetail("current quarter")) == 4);
                }
                string s = item.GetDetail("observation date");
                if (s != "")
                {
                    DateTime time5 = DateTime.Parse(s);
                    if (detail == "weekly")
                    {
                        return (time5.Year != time5.AddDays(7.0).Year);
                    }
                    return (time5.Month == 12);
                }
                if (detail == "weekly")
                {
                    return (item.Date.Year != item.Date.AddDays(7.0).Year);
                }
                if (detail == "monthly")
                {
                    return (item.Date.Month == 12);
                }
            }
            else if (int_0 < (ilist_0.Count - 1))
            {
                return (item.Date.Year < ilist_0[int_0 + 1].Date.Year);
            }
            return (item.Date.Month == 12);
        }

        private static DataSeries smethod_2(WealthScript wealthScript_0)
        {
            DataSeries series = wealthScript_0.FundamentalDataSeries("adjustment factor");
            IList<FundamentalItem> list = wealthScript_0.FundamentalDataItems("split");
            IList<FundamentalItem> list2 = wealthScript_0.FundamentalDataItems("adjustment factor");
            if (((list.Count > 0) && (list2.Count > 0)) && (list[list.Count - 1].Date > list2[list2.Count - 1].Date))
            {
                int num = list.Count - 1;
                int num2 = list2.Count - 1;
                while (list[num].Date > list2[num2].Date)
                {
                    num--;
                }
                while (++num < list.Count)
                {
                    series = (DataSeries) (series * list[num].Value);
                }
            }
            return series;
        }

        private static bool smethod_3(string string_4)
        {
            if (((!(string_4 == "assets") && !(string_4 == "current assets")) && (!(string_4 == "cash") && !(string_4 == "liabilities"))) && ((!(string_4 == "current liabilities") && !(string_4 == "long term debt")) && !(string_4 == "stockholder equity")))
            {
                return false;
            }
            return true;
        }

        private static bool smethod_4(string string_4)
        {
            if (((!(string_4 == "cash dividends") && !(string_4 == "net income")) && (!(string_4 == "sales turnover") && !(string_4 == "operating activities"))) && (!(string_4 == "operating income before depreciation") && !(string_4 == "pretax income")))
            {
                return false;
            }
            return true;
        }

        public static DataSeries YieldSeries(WealthScript wealthScript_0, string itemName)
        {
            DataSeries series = wealthScript_0.FundamentalDataSeries(itemName, 4, false, 0) / ShareSeries(wealthScript_0);
            series /= wealthScript_0.Close;
            return (DataSeries) (series * 100.0);
        }
    }
}

