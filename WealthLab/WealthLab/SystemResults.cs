namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SystemResults : IComparer<Position>
    {
        private DataSeries dataSeries_EquityCurve = new DataSeries("Equity");
        private DataSeries dataSeries_CashCurve = new DataSeries("Cash");
        private DataSeries dataSeries_2 = new DataSeries("DrawDown");
        private DataSeries dataSeries_3 = new DataSeries("DrawDownPct");
        [CompilerGenerated]
        private DataSeries dataSeries_OpenPositionCount;
        private double currentEquity;
        private double currentCash;
        private double totalCommission;
        private double cashReturn;
        private double marginInterest;
        private double dividendsPaid;
        private double double_6;
        private IList<Position> positions;
        private int tradesNSF;
        private static int int_1 = -1;
        private List<Position> list_0 = new List<Position>();
        private List<Alert> alerts = new List<Alert>();
        private List<Position> list_2 = new List<Position>();
        private List<Position> list_3 = new List<Position>();
        private List<Position> list_4 = new List<Position>();
        private static Random random_0 = new Random();
        private SystemPerformance systemPerformance_0;

        public SystemResults(SystemPerformance sysPerf)
        {
            this.systemPerformance_0 = sysPerf;
            if (int_1 == -1)
            {
                int_1 = random_0.Next(100);
            }
        }

        public void BuildEquityCurve(IList<Bars> barsList, TradingSystemExecutor tradingSystemExecutor_0, bool callbackToSizePositions, PosSizer posSizer)
        {
            this.method_2(tradingSystemExecutor_0);
            this.totalCommission = 0.0;
            this.dataSeries_EquityCurve = new DataSeries("Equity");
            this.dataSeries_CashCurve = new DataSeries("Cash");
            this.dataSeries_2 = new DataSeries("DrawDown");
            this.dataSeries_3 = new DataSeries("DrawDownPct");
            this.double_6 = double.MinValue;
            PositionSize positionSize = this.systemPerformance_0.PositionSize;
            double num = 0.0;
            this.OpenPositionCount = new DataSeries("OpenPositions");
            List<Bars> barCollection = new List<Bars>();
            foreach (Bars bars in barsList)
            {
                barCollection.Add(bars);
            }
            foreach (Position position in tradingSystemExecutor_0.MasterPositions)
            {
                if (!barCollection.Contains(position.Bars))
                {
                    barCollection.Add(position.Bars);
                }
            }
            foreach (Position position2 in this.Positions)
            {
                if (!barCollection.Contains(position2.Bars))
                {
                    barCollection.Add(position2.Bars);
                }
            }
            foreach (Bars bars2 in barCollection)
            {
                if (!tradingSystemExecutor_0.PosSize.RawProfitMode && tradingSystemExecutor_0.ApplyDividends)
                {
                    IList<FundamentalItem> list2 = tradingSystemExecutor_0.FundamentalsLoader.RequestSymbolItems(bars2, bars2.Symbol, "dividend");
                    if (list2 == null)
                    {
                        bars2.DivTag = null;
                    }
                    else if (list2.Count == 0)
                    {
                        bars2.DivTag = null;
                    }
                    else
                    {
                        bars2.DivTag = list2;
                    }
                }
                else
                {
                    bars2.DivTag = null;
                }
            }
            this.list_2.Clear();
            this.list_4.Clear();
            this.list_3.Clear();
            SynchronizedBarIterator iterator = new SynchronizedBarIterator(barCollection);
            if (iterator.Date != DateTime.MaxValue)
            {
                List<Position> list3 = new List<Position>();
                List<Position> list4 = new List<Position>();
                if (callbackToSizePositions)
                {
                    foreach (Position position3 in tradingSystemExecutor_0.MasterPositions)
                    {
                        list3.Add(position3);
                    }
                }
                else
                {
                    foreach (Position position4 in this.Positions)
                    {
                        list3.Add(position4);
                    }
                }
                if (posSizer != null)
                {
                    posSizer.method_0(tradingSystemExecutor_0, this.list_2, this.list_4, this.list_3, this.dataSeries_EquityCurve, this.dataSeries_CashCurve, this.dataSeries_2, this.dataSeries_3);
                    posSizer.Initialize();
                }
                this.currentCash = positionSize.RawProfitMode ? 0.0 : positionSize.StartingCapital;
                this.currentEquity = this.currentCash;
                this.method_1(tradingSystemExecutor_0);
                while (true)
                {
                    double num2 = 0.0;
                    for (int i = this.list_2.Count - 1; i >= 0; i--)
                    {
                        Position item = this.list_2[i];
                        if ((!item.Active && (item.ExitDate == iterator.Date)) && (item.ExitOrderType == OrderType.Market))
                        {
                            this.list_2.RemoveAt(i);
                            list4.Add(item);
                            this.list_3.Add(item);
                            num += item.NetProfit;
                            this.currentCash += item.Size;
                            this.currentCash += item.NetProfit;
                            num2 += item.NetProfit;
                            this.currentCash += item.EntryCommission;
                        }
                    }
                    if (posSizer != null)
                    {
                        List<Position> list5 = new List<Position>();
                        foreach (Position position6 in list3)
                        {
                            if (position6.EntryDate == iterator.Date)
                            {
                                list5.Add(position6);
                            }
                        }
                        posSizer.Candidates = list5;
                    }
                    double thisBarCash = this.currentCash;
                    while (list3.Count > 0)
                    {
                        Position position7 = list3[0];
                        if (!(position7.EntryDate == iterator.Date))
                        {
                            break;
                        }
                        if (callbackToSizePositions)
                        {
                            position7.Shares = tradingSystemExecutor_0.CalcPositionSize(position7, position7.Bars, position7.EntryBar, position7.BasisPrice, position7.PositionType, position7.RiskStopLevel, true, position7.OverrideShareSize, thisBarCash) * position7.SplitFactor;
                            double num5 = (position7.Shares * position7.EntryPrice) + position7.EntryCommission;
                            thisBarCash -= num5;
                            if ((tradingSystemExecutor_0.Commission != null) && tradingSystemExecutor_0.ApplyCommission)
                            {
                                position7.EntryCommission = tradingSystemExecutor_0.Commission.Calculate((position7.PositionType == PositionType.Long) ? TradeType.Buy : TradeType.Short, position7.EntryOrderType, position7.EntryPrice, position7.Shares, position7.Bars);
                                if (!position7.Active)
                                {
                                    position7.ExitCommission = tradingSystemExecutor_0.Commission.Calculate((position7.PositionType == PositionType.Long) ? TradeType.Sell : TradeType.Cover, position7.ExitOrderType, position7.ExitPrice, position7.Shares, position7.Bars);
                                }
                            }
                        }
                        list3.RemoveAt(0);
                        if (position7.Shares > 0.0)
                        {
                            bool flag;
                            double num6 = this.currentCash;
                            if (!tradingSystemExecutor_0.PosSize.RawProfitMode)
                            {
                                double num7 = this.currentEquity - this.currentCash;
                                num6 = (this.currentEquity * tradingSystemExecutor_0.PosSize.MarginFactor) - num7;
                            }
                            if (!(flag = !callbackToSizePositions))
                            {
                                if (positionSize.RawProfitMode)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    flag = num6 >= (position7.Size + position7.EntryCommission);
                                }
                            }
                            if (flag)
                            {
                                this.currentCash -= position7.Size;
                                this.currentCash -= position7.EntryCommission;
                                num6 -= position7.Size;
                                num6 -= position7.EntryCommission;
                                this.list_2.Add(position7);
                                this.list_4.Add(position7);
                                this.totalCommission += position7.EntryCommission + position7.ExitCommission;
                            }
                            else
                            {
                                position7.Shares = 0.0;
                            }
                        }
                    }
                    for (int j = this.list_2.Count - 1; j >= 0; j--)
                    {
                        Position position8 = this.list_2[j];
                        if (!position8.Active && (position8.ExitDate == iterator.Date))
                        {
                            this.list_2.RemoveAt(j);
                            list4.Add(position8);
                            this.list_3.Add(position8);
                            num += position8.NetProfit;
                            this.currentCash += position8.Size;
                            this.currentCash += position8.NetProfit;
                            num2 += position8.NetProfit;
                            this.currentCash += position8.EntryCommission;
                        }
                    }
                    this.currentEquity = positionSize.RawProfitMode ? 0.0 : positionSize.StartingCapital;
                    foreach (Position position9 in this.list_2)
                    {
                        int num9 = iterator.Bar(position9.Bars);
                        this.currentEquity += position9.NetProfitAsOfBar(num9);
                        this.method_3(position9, num9, ref num);
                    }
                    foreach (Position position10 in list4)
                    {
                        int num10 = iterator.Bar(position10.Bars);
                        this.currentEquity += position10.NetProfitAsOfBar(num10);
                        this.method_3(position10, num10, ref num);
                    }
                    list4.Clear();
                    this.currentEquity += num - num2;
                    this.dataSeries_EquityCurve.Add(this.currentEquity, iterator.Date);
                    this.dataSeries_CashCurve.Add(this.currentCash, iterator.Date);
                    this.OpenPositionCount.Add((double) this.list_2.Count, iterator.Date);
                    int num11 = this.dataSeries_CashCurve.Count - 1;
                    if ((tradingSystemExecutor_0.ApplyInterest && !tradingSystemExecutor_0.PosSize.RawProfitMode) && (this.dataSeries_CashCurve.Count > 1))
                    {
                        DateTime time = this.dataSeries_CashCurve.Date[num11];
                        DateTime time2 = this.dataSeries_CashCurve.Date[num11 - 1];
                        if (time.Date != time2.Date)
                        {
                            TimeSpan span = this.dataSeries_CashCurve.Date[num11] - this.dataSeries_CashCurve.Date[num11 - 1];
                            double cashAdjustmentFactor = 1.0;
                            double num13 = this.dataSeries_CashCurve[num11];
                            if (num13 > 0.0)
                            {
                                cashAdjustmentFactor = tradingSystemExecutor_0.CashAdjustmentFactor;
                            }
                            else if (num13 < 0.0)
                            {
                                cashAdjustmentFactor = tradingSystemExecutor_0.MarginAdjustmentFactor;
                            }
                            for (int k = 1; k <= span.Days; k++)
                            {
                                num13 *= cashAdjustmentFactor;
                            }
                            cashAdjustmentFactor = num13 - this.dataSeries_CashCurve[num11];
                            if (num13 > 0.0)
                            {
                                this.CashReturn += cashAdjustmentFactor;
                            }
                            else
                            {
                                this.MarginInterest += cashAdjustmentFactor;
                            }
                            this.dataSeries_CashCurve[num11] = num13;
                            this.dataSeries_EquityCurve[num11] += cashAdjustmentFactor;
                            this.currentCash = this.dataSeries_CashCurve[num11];
                            this.currentEquity = this.dataSeries_EquityCurve[num11];
                            num += cashAdjustmentFactor;
                        }
                    }
                    if (posSizer != null)
                    {
                        if (this.currentEquity > this.double_6)
                        {
                            this.double_6 = this.currentEquity;
                        }
                        double num15 = this.currentEquity - this.double_6;
                        double num16 = (num15 * 100.0) / this.double_6;
                        this.dataSeries_2.Add(num15, this.dataSeries_EquityCurve.Date[num11]);
                        this.dataSeries_3.Add(num16, this.dataSeries_EquityCurve.Date[num11]);
                    }
                    if (!iterator.Next())
                    {
                        return;
                    }
                }
            }
        }

        public int Compare(Position position_0, Position position_1)
        {
            if (position_0.EntryDate == position_1.EntryDate)
            {
                return position_0.CombinedPriority.CompareTo(position_1.CombinedPriority);
            }
            return position_0.EntryDate.CompareTo(position_1.EntryDate);
        }

        internal void method_0()
        {
            foreach (Position position in this.Positions)
            {
                position.method_2();
            }
        }

        private void method_1(TradingSystemExecutor tradingSystemExecutor_0)
        {
            if (tradingSystemExecutor_0.TNP < _secureCodeMin)
            {
                this.currentEquity *= tradingSystemExecutor_0.TNPAdjustment;
            }
        }

        private void method_2(TradingSystemExecutor tradingSystemExecutor_0)
        {
            if (tradingSystemExecutor_0.TNP < _secureCodeMin)
            {
                int num = random_0.Next(100);
                bool flag = false;
                if (int_1 == 0)
                {
                    flag = true;
                }
                else if (num == 0x42)
                {
                    if (int_1 < 1)
                    {
                        flag = true;
                    }
                    else
                    {
                        int_1--;
                    }
                }
                while (flag)
                {
                }
            }
        }

        ///WYJ fix, code from Reflector, workable, but deprecated because of having too many goto statements. Try version from ILSpy
        /*
        private void method_3(Position position_0, int int_2, ref double double_7)
        {
            if (position_0.Bars.DivTag != null)
            {
                DateTime time5;
                IList<FundamentalItem> divTag = (IList<FundamentalItem>) position_0.Bars.DivTag;
                DateTime time = position_0.Bars.Date[int_2];
                DateTime date = time.Date;
                if (int_2 >= 1)
                {
                    DateTime time8 = position_0.Bars.Date[int_2 - 1];
                    time5 = time8.Date;
                }
                else
                {
                    DateTime time6 = position_0.Bars.Date[int_2];
                    time5 = time6.Date.AddYears(-1);
                }
                using (IEnumerator<FundamentalItem> enumerator = divTag.GetEnumerator())
                {
                    double num;
                    FundamentalItem current;
                    double num2;
                    double num3;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        DateTime time3 = current.Date;
                        if (position_0.Bars.IsIntraday)
                        {
                            if ((!(time3 == date) || (position_0.Bars.IntradayBarNumber(int_2) != 0)) || !(position_0.EntryDate.Date != time3.Date))
                            {
                                continue;
                            }
                            goto Label_016C;
                        }
                        if (position_0.Bars.Scale == BarScale.Daily)
                        {
                            if (!(time3 == date) || (position_0.EntryDate >= time3))
                            {
                                continue;
                            }
                            goto Label_01CB;
                        }
                        if (((time3 <= date) && (time3 > time5)) && ((position_0.EntryDate < time3) && (position_0.Active || (position_0.ExitDate >= time3))))
                        {
                            goto Label_0224;
                        }
                    }
                    return;
                Label_016C:
                    num3 = current.Value * position_0.Shares;
                    if (position_0.PositionType == PositionType.Short)
                    {
                        num3 = -num3;
                    }
                    this.double_1 += num3;
                    double_7 += num3;
                    this.DividendsPaid += num3;
                    if (divTag.Count == 0)
                    {
                        position_0.Bars.DivTag = null;
                    }
                    return;
                Label_01CB:
                    num2 = current.Value * position_0.Shares;
                    if (position_0.PositionType == PositionType.Short)
                    {
                        num2 = -num2;
                    }
                    this.double_1 += num2;
                    double_7 += num2;
                    this.DividendsPaid += num2;
                    if (divTag.Count == 0)
                    {
                        position_0.Bars.DivTag = null;
                    }
                    return;
                Label_0224:
                    num = current.Value * position_0.Shares;
                    if (position_0.PositionType == PositionType.Short)
                    {
                        num = -num;
                    }
                    this.double_1 += num;
                    double_7 += num;
                    this.DividendsPaid += num;
                    if (divTag.Count == 0)
                    {
                        position_0.Bars.DivTag = null;
                    }
                }
            }
        } */

        ///WYJ fix, code from ILSpy
        private void method_3(Position position_0, int int_2, ref double double_7)
        {
            if (position_0.Bars.DivTag != null)
            {
                IList<FundamentalItem> list = (IList<FundamentalItem>)position_0.Bars.DivTag;
                DateTime date = position_0.Bars.Date[int_2].Date;
                DateTime t;
                if (int_2 >= 1)
                {
                    t = position_0.Bars.Date[int_2 - 1].Date;
                }
                else
                {
                    t = position_0.Bars.Date[int_2].Date.AddYears(-1);
                }
                foreach (FundamentalItem current in list)
                {
                    DateTime date2 = current.Date;
                    if (position_0.Bars.IsIntraday)
                    {
                        if (!(date2 == date) || position_0.Bars.IntradayBarNumber(int_2) != 0 || !(position_0.EntryDate.Date != date2.Date))
                        {
                            continue;
                        }
                        double num = current.Value * position_0.Shares;
                        if (position_0.PositionType == PositionType.Short)
                        {
                            num = -num;
                        }
                        this.currentCash += num;
                        double_7 += num;
                        this.DividendsPaid += num;
                        if (list.Count == 0)
                        {
                            position_0.Bars.DivTag = null;
                        }
                    }
                    else
                    {
                        if (position_0.Bars.Scale == BarScale.Daily)
                        {
                            if (!(date2 == date) || !(position_0.EntryDate < date2))
                            {
                                continue;
                            }
                            double num2 = current.Value * position_0.Shares;
                            if (position_0.PositionType == PositionType.Short)
                            {
                                num2 = -num2;
                            }
                            this.currentCash += num2;
                            double_7 += num2;
                            this.DividendsPaid += num2;
                            if (list.Count == 0)
                            {
                                position_0.Bars.DivTag = null;
                            }
                        }
                        else
                        {
                            if (!(date2 <= date) || !(date2 > t) || !(position_0.EntryDate < date2) || (!position_0.Active && !(position_0.ExitDate >= date2)))
                            {
                                continue;
                            }
                            double num3 = current.Value * position_0.Shares;
                            if (position_0.PositionType == PositionType.Short)
                            {
                                num3 = -num3;
                            }
                            this.currentCash += num3;
                            double_7 += num3;
                            this.DividendsPaid += num3;
                            if (list.Count == 0)
                            {
                                position_0.Bars.DivTag = null;
                            }
                        }
                    }
                    break;
                }
            }
        }


        internal void method_4(Position position_0)
        {
            this.list_0.Add(position_0);
        }

        internal void method_5(Alert alert_0)
        {
            this.alerts.Add(alert_0);
        }

        internal void method_6()
        {
            this.tradesNSF = 0;
            this.cashReturn = 0.0;
            this.marginInterest = 0.0;
            this.dividendsPaid = 0.0;
            this.list_0.Clear();
            this.alerts.Clear();
            if (this.dataSeries_EquityCurve != null)
            {
                this.dataSeries_EquityCurve.method_2();
                this.dataSeries_CashCurve.method_2();
            }
        }

        internal void method_7(bool bool_0)
        {
            this.list_0.Clear();
            if (!bool_0)
            {
                this.dataSeries_EquityCurve.method_2();
                this.dataSeries_CashCurve.method_2();
            }
        }

        internal void method_8()
        {
            this.list_0.Sort(this);
        }

        internal void method_9(PosSizer posSizer_0)
        {
            posSizer_0.ActivePositions = this.list_2;
            posSizer_0.Positions = this.list_4;
            posSizer_0.ClosedPositions = this.list_3;
        }

        private static double _secureCode
        {
            get
            {
                return DateTime.Now.Add(new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)).ToOADate();
            }
        }

        private static double _secureCodeMin
        {
            get
            {
                return DateTime.FromOADate(_secureCode).Subtract(new TimeSpan(0, 0, 0, 1)).ToOADate();
            }
        }

        public List<Alert> Alerts
        {
            get
            {
                return this.alerts;
            }
        }

        public double APR
        {
            get
            {
                if (this.EquityCurve.Count < 2)
                {
                    return 0.0;
                }
                if (this.EquityCurve[0] == 0.0)
                {
                    return 0.0;
                }
                TimeSpan span = this.EquityCurve.Date[this.EquityCurve.Count - 1] - this.EquityCurve.Date[0];
                double num = this.EquityCurve[0];
                double num2 = this.EquityCurve[this.EquityCurve.Count - 1];
                return ((Math.Pow(num2 / num, 365.25 / ((double) span.Days)) - 1.0) * 100.0);
            }
        }

        public DataSeries CashCurve
        {
            get
            {
                return this.dataSeries_CashCurve;
            }
            internal set
            {
                this.dataSeries_CashCurve = value;
            }
        }

        public double CashReturn
        {
            get
            {
                return this.cashReturn;
            }
            internal set
            {
                this.cashReturn = value;
            }
        }

        internal double CurrentCash
        {
            get
            {
                return this.currentCash;
            }
            set
            {
                this.currentCash = value;
            }
        }

        internal double CurrentEquity
        {
            get
            {
                return this.currentEquity;
            }
            set
            {
                this.currentEquity = value;
            }
        }

        public double DividendsPaid
        {
            get
            {
                return this.dividendsPaid;
            }
            internal set
            {
                this.dividendsPaid = value;
            }
        }

        public DataSeries EquityCurve
        {
            get
            {
                return this.dataSeries_EquityCurve;
            }
            internal set
            {
                this.dataSeries_EquityCurve = value;
            }
        }

        public double MarginInterest
        {
            get
            {
                return this.marginInterest;
            }
            internal set
            {
                this.marginInterest = value;
            }
        }

        public double NetProfit
        {
            get
            {
                double num = 0.0;
                foreach (Position position in this.Positions)
                {
                    num += position.NetProfit;
                }
                return (((num + this.cashReturn) + this.marginInterest) + this.dividendsPaid);
            }
        }

        public DataSeries OpenPositionCount
        {
            [CompilerGenerated]
            get
            {
                return this.dataSeries_OpenPositionCount;
            }
            [CompilerGenerated]
            set
            {
                this.dataSeries_OpenPositionCount = value;
            }
        }

        public IList<Position> Positions
        {
            get
            {
                if (this.positions == null)
                {
                    this.positions = this.list_0.AsReadOnly();
                }
                return this.positions;
            }
        }

        public double ProfitPerBar
        {
            get
            {
                double num = 0.0;
                double num2 = 0.0;
                foreach (Position position in this.Positions)
                {
                    num2 += position.NetProfit;
                    num += position.BarsHeld;
                }
                num2 = ((num2 + this.cashReturn) + this.marginInterest) + this.dividendsPaid;
                if (num == 0.0)
                {
                    return 0.0;
                }
                return (num2 / num);
            }
        }

        public double TotalCommission
        {
            get
            {
                return this.totalCommission;
            }
        }

        public int TradesNSF
        {
            get
            {
                return this.tradesNSF;
            }
            set
            {
                this.tradesNSF = value;
            }
        }
    }
}

