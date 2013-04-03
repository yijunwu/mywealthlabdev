namespace WealthLab.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using WealthLab;

    public class FundamentalDateRules
    {
        private Bars bars_0;
        private IList<FundamentalItem> ilist_0;

        public FundamentalDateRules(WealthScript wealthScript_0, Bars bars, string symbol, string itemName)
        {
            this.bars_0 = bars;
            this.ilist_0 = wealthScript_0.FundamentalDataItems(symbol, itemName);
        }

        public bool DaysSinceFundamentalItem(int int_0, out int days)
        {
            FundamentalItem item = this.method_1(int_0);
            if (item == null)
            {
                days = 0;
                return false;
            }
            DateTime time = this.bars_0.Date[int_0];
            days = time.Subtract(item.Date).Days;
            return true;
        }

        public bool DaysToFundamentalItem(int int_0, out int days)
        {
            FundamentalItem item = this.method_0(int_0);
            if (item == null)
            {
                days = 0;
                return false;
            }
            days = item.Date.Subtract(this.bars_0.Date[int_0]).Days;
            return true;
        }

        private FundamentalItem method_0(int int_0)
        {
            int num = -1;
            for (int i = this.ilist_0.Count - 1; i >= 0; i--)
            {
                if (this.bars_0.Date[int_0] >= this.ilist_0[i].Date)
                {
                    break;
                }
                num = i;
            }
            if (num >= 0)
            {
                return this.ilist_0[num];
            }
            return null;
        }

        private FundamentalItem method_1(int int_0)
        {
            int num = -1;
            for (int i = 0; i < this.ilist_0.Count; i++)
            {
                if (this.bars_0.Date[int_0] < this.ilist_0[i].Date)
                {
                    break;
                }
                num = i;
            }
            if (num >= 0)
            {
                return this.ilist_0[num];
            }
            return null;
        }
    }
}

